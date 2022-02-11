using projAPI.Model;
using projContext;
using projContext.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Classes
{
    public class clsManualAttendance
    {
        private readonly Context _context;
        private readonly List<tbl_attendance_details_manual> _tadm = null;
        public clsManualAttendance(List<tbl_attendance_details_manual> tadm, Context _context)
        {
            _tadm = tadm;
            this._context = _context;
        }
       

        public void SaveData()
        {

            DeleteData();

            for (int i = 0; i < _tadm.Count; i++)
            {
                if (_tadm[i].start_in != null)
                {
                    if (_tadm[i].start_in.Value.ToString("dd-MMM-yyyy").ToLower() == "01-jan-2000")
                    {
                        _tadm[i].start_in = Convert.ToDateTime(_tadm[i].attendance_dt).AddHours(_tadm[i].start_in.Value.Hour).AddHours(_tadm[i].start_in.Value.Minute);
                    }
                }

                if (_tadm[i].start_out != null)
                {
                    if (_tadm[i].start_out.Value.ToString("dd-MMM-yyyy").ToLower() == "01-jan-2000")
                    {
                        _tadm[i].start_out = Convert.ToDateTime(_tadm[i].attendance_dt).AddHours(_tadm[i].start_out.Value.Hour).AddHours(_tadm[i].start_out.Value.Minute);
                    }
                }
                if (_tadm[i].start_in.HasValue && _tadm[i].start_out.HasValue)
                {
                    if (_tadm[i].start_in.Value > _tadm[i].start_out.Value)
                    {
                        _tadm[i].start_out = _tadm[i].start_out.Value.AddDays(1);
                    }
                }


            }
            _context.tbl_attendance_details_manual.AttachRange(_tadm);
            _context.SaveChanges();

        }

        public void DeleteData()
        {
            var EmpIds = _tadm.Select(p => p.emp_id).ToArray();
            var attendance_dt = _tadm.Select(p => p.attendance_dt).ToArray();
            List<tbl_attendance_details_manual> tadm_dbs = _context.tbl_attendance_details_manual.Where(p => EmpIds.Contains(p.emp_id) && attendance_dt.Contains(p.attendance_dt)).ToList();
            foreach (var tadm_db in tadm_dbs)
            {
                if (_tadm.Any(p => p.emp_id == tadm_db.emp_id && p.attendance_dt == tadm_db.attendance_dt))
                {
                    _context.Attach(tadm_db);
                    _context.Entry(tadm_db).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                }
            }
            _context.SaveChanges();

        }

    }



    public class clsCalcualteDailyAttendance
    {

        class clsEmployeeLeave
        {
           public int EmpId { get; set; }
            public DateTime LeaveDay { get; set; }
            public byte LeaveType { get; set; }
            public bool IsLWP { get; set; }
            public bool IsShortLeave { get; set; }
            public bool IsWeekOff { get; set; }
        }

        public string _Message = "";
        public byte _Status = 1, _PunchType, _EmpOtEnable, _EmpCompOffEnable;
        private const int _AttendancePickUpperLimit = 690, _AttendancePickLowerLimit = 10;
        private readonly byte _IsFixedWeekoff;
        //private readonly DateTime _currentDate = DateTime.Now;
        private readonly int _empId, _empDetailId, _payrollDate, _empLocationId, _empReligionId;
        private readonly DateTime _fromDate, _toDate, _currentDate = DateTime.Now, _defaultDate = new DateTime(2000, 1, 1);
        private List<tbl_daily_attendance> _Tdas;
        private List<tbl_attendance_details_manual> _AttendanceDataManual;
        private List<tbl_attendance_details> _AttendanceData;
        private List<tbl_comp_off_ledger> _CompOffLeger;
        private List<clsEmployeeLeave> _EmpLeaves;
        //declare context
        private readonly Context _context;
        //Declare Delegate
        private delegate void DeleteExiting();
        private delegate void AttendanceBasicSet();
        private DeleteExiting _deleteExiting;
        private AttendanceBasicSet _attendanceBasicSet;
        //declare Some class varbles
        private class GraceLimit
        {
            public int ShiftId { get; set; }
            public int AlreadyApplied { get; set; }
            public int TotalAvailable { get; set; }
            public DateTime grace_time_for_late_punch { get; set; }
        }

        

        public clsCalcualteDailyAttendance(Context context, int EmpId, int EmpDetailId,
            int EmpLocationId, int EmpReligionId, byte IsFixedWeekoff, int PayrollDate, byte PunchType,
            byte EmpOtEnable, byte EmpCompOffEnable,
            DateTime FromDate, DateTime ToDate, DateTime JoiningDate)
        {
            _context = context;
            _empId = EmpId;
            _empDetailId = EmpDetailId;
            _empReligionId = EmpReligionId;
            _empLocationId = EmpLocationId;
            _payrollDate = PayrollDate;
            if (JoiningDate > FromDate)
            {
                FromDate = JoiningDate;
            }
            _fromDate = Convert.ToDateTime(FromDate.ToString("dd-MMM-yyyy"));
            _toDate = Convert.ToDateTime(ToDate.ToString("dd-MMM-yyyy"));
            _IsFixedWeekoff = IsFixedWeekoff;
            _PunchType = PunchType;
            _EmpOtEnable = EmpOtEnable;
            _EmpCompOffEnable = EmpCompOffEnable;
            _EmpLeaves = new List<clsEmployeeLeave>();
            CastDelegate();
        }

        /// <summary>
        /// assign the delegate so in future we will 
        /// also asign the log for delete exiting
        /// </summary>
        private void CastDelegate()
        {
            _deleteExiting = new DeleteExiting(DeleteExitingCompOff);
            _deleteExiting += new DeleteExiting(DeleteExitingAttendance);
            _attendanceBasicSet = new AttendanceBasicSet(SetShift);
            _attendanceBasicSet += new AttendanceBasicSet(SetEmpPunchTime);
            _attendanceBasicSet += new AttendanceBasicSet(SetEmpOutdoor);
            _attendanceBasicSet += new AttendanceBasicSet(SetEmpAttendaceRegularize);
            _attendanceBasicSet += new AttendanceBasicSet(SetEmpWeekoff);
            _attendanceBasicSet += new AttendanceBasicSet(SetEmpHoliday);
            _attendanceBasicSet += new AttendanceBasicSet(SetEmpLeave);
            _attendanceBasicSet += new AttendanceBasicSet(SetEmpCompoff);

        }

        #region  ********* delete the existing Data ****************
        /// <summary>
        /// delete the eixting data of comp off
        /// </summary>
        private void DeleteExitingCompOff()
        {
            List<DateTime> NotFrezeDates = _context.tbl_daily_attendance.Where(p => p.emp_id == _empId && p.attendance_dt >= _fromDate && p.attendance_dt <= _toDate && p.is_freezed == 0).Select(p => p.attendance_dt).ToList();
            _context.tbl_comp_off_ledger.RemoveRange(_context.tbl_comp_off_ledger.Where(p => NotFrezeDates.Contains(p.compoff_date) && p.e_id == _empId && p.transaction_type == 1 && p.credit > 0));
            _context.SaveChanges();
        }
        /// <summary>
        /// delete the existing data of daily Attendace
        /// </summary>
        private void DeleteExitingAttendance()
        {
            _context.tbl_daily_attendance.RemoveRange(_context.tbl_daily_attendance.Where(p => p.emp_id == _empId && p.attendance_dt >= _fromDate && p.attendance_dt <= _toDate && p.is_freezed == 0));
            _context.SaveChanges();
        }


        #endregion


        private void BasicSet()
        {
            _Tdas = new List<tbl_daily_attendance>();
            DateTime ProcessingDate = _fromDate;
            List<DateTime> AllDates = new List<DateTime>();
            while (DateTime.Compare(ProcessingDate, _toDate) <= 0)
            {
                AllDates.Add(ProcessingDate);
                ProcessingDate = ProcessingDate.AddDays(1);
            }
            //if Not Data found then throw Exception so it will not
            //execute furter more
            if (AllDates.Count == 0) { new Exception("No data for process"); }
            List<DateTime> FrezeDates = _context.tbl_daily_attendance.Where(p => p.emp_id == _empId && p.attendance_dt >= _fromDate && p.attendance_dt <= _toDate && p.is_freezed == 1).Select(p => p.attendance_dt).ToList();
            _AttendanceDataManual = _context.tbl_attendance_details_manual.Where(p => p.emp_id == _empId && p.attendance_dt >= _fromDate && p.attendance_dt <= _toDate).ToList();
            //Remove from all date which are frezed
            AllDates.RemoveAll(p => FrezeDates.Contains(p));
            if (AllDates.Count == 0)
            {
                new Exception("No data for process");
            }
            _Tdas = AllDates.Select(p => new tbl_daily_attendance
            {
                emp_id = _empId,
                attendance_dt = p,
                payrollmonthyear = 0,
                last_process_dt = _currentDate,
                emp_offcl_id = _empDetailId,
                shift_id = null,
                s_d_id = null,
                in_time = _defaultDate,
                out_time = _defaultDate,
                shift_max_in_time = _defaultDate,
                shift_in_time = _defaultDate,
                shift_out_time = _defaultDate,
                day_status = 0,
                leave_type = 0,
                is_weekly_off = 0,
                is_holiday = 0,
                is_comp_off = 0,
                is_outdoor = 0,
                is_regularize = 0,
                is_grace_applied = 0,
                grace_period_hour = 0,
                grace_period_minute = 0,
                short_leave_hour = 0,
                short_leave_minute = 0,
                is_late_in = 0,
                is_early_out = 0,
                early_or_late_in_hour = 0,
                early_or_late_in_minute = 0,
                early_or_late_out_hour = 0,
                early_or_late_out_minute = 0,
                working_hour_done = 0,
                working_minute_done = 0,
                w_hour_req_for_full_day = 0,
                w_min_req_for_full_day = 0,
                w_hour_req_for_half_day = 0,
                w_min_req_for_half_day = 0,
                is_ot_given = 0,
                ot_hour_done = 0,
                ot_minute_done = 0,
                is_freezed = 0,
                holiday_id = null,
                leave_request_id = null
            }).ToList();
            DateTime AttendanceLastDay = _toDate.AddDays(2);
            _AttendanceData = _context.tbl_attendance_details.Where(p => p.emp_id == _empId && p.punch_time >= _fromDate && p.punch_time <= AttendanceLastDay).ToList();
        }

        

        private bool IsLWP(int LeaveRequestId)
        {
            bool IsLWP_ = false;
            var leaveRequest = _context.tbl_leave_request.Where(p => p.leave_request_id == LeaveRequestId).FirstOrDefault();
            if (leaveRequest != null)
            {
                if (_context.tbl_leave_type.Where(p => p.leave_type_id == leaveRequest.leave_type_id && p.leave_type_name == "LWP").Count()>0)
                { IsLWP_ = true; }
            }
            return IsLWP_;
        }

        private bool IsWeekOff(int LeaveRequestId)
        {
            bool IsWeekOff_ = false;
            var leaveRequest = _context.tbl_leave_request.Where(p => p.leave_request_id == LeaveRequestId).FirstOrDefault();
            if (leaveRequest != null)
            {
                if (_context.tbl_leave_type.Where(p => p.leave_type_id == leaveRequest.leave_type_id && p.leave_type_name == "WeekOff").Count() > 0)
                { IsWeekOff_ = true; }
            }
            return IsWeekOff_;
        }

        private void SetShift()
        {
            List<tbl_emp_shift_allocation> Tesas = null;
            //first Check is their Any Roster for the Emp 
            List<tbl_shift_roster_master> Tsrms = _context.tbl_shift_roster_master.Where(p => p.emp_id == _empId &&
           ((p.applicable_from_date <= _fromDate && p.applicable_to_date >= _fromDate) ||
           (p.applicable_from_date <= _toDate && p.applicable_to_date >= _toDate) ||
           (p.applicable_from_date >= _fromDate && p.applicable_to_date <= _toDate))
            ).OrderByDescending(p => p.shift_roster_id).ToList();
            //get the default shift of the employee if roster not found
            if (Tsrms == null || Tsrms.Count == 0)
            {
                Tesas = _context.tbl_emp_shift_allocation.Where(p => p.employee_id == _empId && p.is_deleted == 0).OrderByDescending(p => p.emp_shift_id).ToList();
                //Tesas = _context.tbl_emp_shift_allocation.Where(p => p.employee_id == _empId && p.is_deleted==0 ).OrderByDescending(p => p.created_date).ToList();
            }
            int? DefaultShiftId = null;
            var tsmdefault = _context.tbl_shift_master.FirstOrDefault(p => p.is_default == 1);
            if (tsmdefault != null)
            {
                DefaultShiftId = tsmdefault.shift_id;
            }

            DateTime ProcessingDate;
            //set the Shift of employee one by one as per processing date
            for (int Index = 0; Index < _Tdas.Count; Index++)
            {
                ProcessingDate = _Tdas[Index].attendance_dt;
                tbl_shift_roster_master tsrm = Tsrms.FirstOrDefault(p => p.applicable_from_date <= ProcessingDate && p.applicable_to_date >= ProcessingDate);
                if (tsrm != null)
                {
                    _Tdas[Index].shift_id = GetRosterShiftMasterId(tsrm, ProcessingDate);
                }
                else if (_Tdas[Index].shift_id == null)
                {
                    if (Tesas != null)
                    {
                        tbl_emp_shift_allocation tesa = Tesas.Where(p => p.applicable_from_date <= ProcessingDate).OrderByDescending(p=>p.emp_shift_id).FirstOrDefault() ;
                        if (tesa != null)
                        {
                            _Tdas[Index].shift_id = tesa.shift_id;
                        }
                        else
                        {
                            var Sid = _context.tbl_shift_master.Where(p => p.is_default == 1).FirstOrDefault();
                            _Tdas[Index].shift_id = Sid.shift_id;
                        }
                    }
                }
                if (_Tdas[Index].shift_id == null)
                {
                    _Tdas[Index].shift_id = DefaultShiftId;
                }
                //Throw Excetion if Shift Roster master not defied
                if (_Tdas[Index].shift_id == null)
                {
                    throw new Exception("Shift not define coresponding to Emp " + _empId + " for date " + ProcessingDate.ToString("dd-MMM-yyyy"));
                }
            }
            int?[] ShiftIds = _Tdas.Select(p => p.shift_id).ToArray();
            //find the Shift details


            List<tbl_shift_details> Tsds = _context.tbl_shift_details.Where(p => p.is_deleted == 0 && ShiftIds.Contains(p.shift_id)).ToList();
            for (int Index = 0; Index < _Tdas.Count; Index++)
            {
                tbl_shift_details Tsd = Tsds.FirstOrDefault(p => p.shift_id == _Tdas[Index].shift_id);
                _Tdas[Index].shift_in_time = _Tdas[Index].attendance_dt.Add(Tsd.punch_in_time.Subtract(_defaultDate));
                _Tdas[Index].shift_max_in_time = _Tdas[Index].attendance_dt.Add(Tsd.punch_in_max_time.Subtract(_defaultDate));
                _Tdas[Index].shift_out_time = _Tdas[Index].attendance_dt.Add(Tsd.punch_out_time.Subtract(_defaultDate));
                _Tdas[Index].s_d_id = Tsd.shift_details_id;
                _Tdas[Index].grace_period_hour = 0; //Tsd.grace_time_for_late_punch.Hour;
                _Tdas[Index].grace_period_minute = 0;// Tsd.grace_time_for_late_punch.Minute;
                _Tdas[Index].w_hour_req_for_full_day = Tsd.maximum_working_hours;
                _Tdas[Index].w_min_req_for_full_day = Tsd.maximum_working_minute;
                _Tdas[Index].w_hour_req_for_half_day = Tsd.min_halfday_working_hour.Hour;
                _Tdas[Index].w_min_req_for_half_day = Tsd.min_halfday_working_hour.Minute;
            }

        }

        public int? GetRosterShiftMasterId(tbl_shift_roster_master tsrm, DateTime processing_date)
        {
            int totalday = Convert.ToInt32((processing_date - tsrm.applicable_from_date).TotalDays) - 1;
            int totalshiftcount = tsrm.shft_id1 == null ? 0 : tsrm.shft_id2 == null ? 1 : tsrm.shft_id3 == null ? 2 : tsrm.shft_id4 == null ? 3 : tsrm.shft_id5 == null ? 4 : 5;
            totalday = totalday % (tsrm.shift_rotat_in_day * totalshiftcount);
            int currentshiftno = totalday / tsrm.shift_rotat_in_day;
            return currentshiftno == 0 ? tsrm.shft_id1 : currentshiftno == 1 ? tsrm.shft_id2 : currentshiftno == 2 ? tsrm.shft_id3 : currentshiftno == 3 ? tsrm.shft_id4 : tsrm.shft_id5;
        }

        private void SetEmpPunchTime()
        {


            for (int Index = 0; Index < _Tdas.Count; Index++)
            {

                if (_AttendanceData.Count == 0)
                {
                    _Tdas[Index].in_time = _defaultDate;
                    _Tdas[Index].out_time = _defaultDate;
                }
                else
                {
                    ////get the In time

                    tbl_attendance_details Tempattddata = _AttendanceData.Where(p => p.punch_time >= _Tdas[Index].shift_in_time.AddHours(0 - _AttendancePickLowerLimit)).OrderByDescending(p => p.punch_time).FirstOrDefault();
                    if (Tempattddata == null)
                    {
                        Tempattddata = _AttendanceData.Where(p => p.punch_time >= _Tdas[Index].shift_in_time &&
                        p.punch_time <= _Tdas[Index].shift_max_in_time).OrderBy(p => p.punch_time).FirstOrDefault();
                    }
                        
                    DateTime? tad_in = null;
                    DateTime? tad_out = null;


                    if (Tempattddata != null)
                    {
                        tad_in = Tempattddata.punch_time;
                    }
                    if (tad_in == null)
                    {
                        //punch in time is not find
                        Tempattddata = null;
                        Tempattddata = _AttendanceData.Where(p => p.punch_time >= _Tdas[Index].attendance_dt && p.punch_time <= _Tdas[Index].shift_in_time.AddMinutes(_AttendancePickUpperLimit)).OrderBy(p => p.punch_time).FirstOrDefault();
                        if (Tempattddata != null)
                        {
                            tad_in = Tempattddata.punch_time;
                        }

                    }
                    Tempattddata = null;
                    Tempattddata = _AttendanceData.Where(p => p.punch_time >= _Tdas[Index].shift_out_time.AddHours(0 - _AttendancePickLowerLimit) && p.punch_time <= _Tdas[Index].shift_out_time.AddHours(_AttendancePickLowerLimit)).OrderByDescending(p => p.punch_time).FirstOrDefault();
                    if (Tempattddata != null)
                    {
                        tad_out = Tempattddata.punch_time;
                    }
                    _Tdas[Index].in_time = tad_in ?? _defaultDate;
                    _Tdas[Index].out_time = tad_out ?? _defaultDate;

                    if (DateTime.Compare(_Tdas[Index].in_time, _Tdas[Index].out_time) == 0)
                    {
                        _Tdas[Index].out_time = _defaultDate;
                    }
                }
                var _AttendanceDataManualtemp = _AttendanceDataManual.Where(p => p.attendance_dt == _Tdas[Index].attendance_dt && p.emp_id== _Tdas[Index].emp_id ).OrderByDescending(P => P.entry_date).FirstOrDefault();
                if (_AttendanceDataManualtemp != null)
                {
                    if (_AttendanceDataManualtemp.start_in.HasValue)
                    {
                        _Tdas[Index].in_time = _AttendanceDataManualtemp.start_in.Value;
                    }
                    if (_AttendanceDataManualtemp.start_out.HasValue)
                    {
                        _Tdas[Index].out_time = _AttendanceDataManualtemp.start_out.Value;
                    }
                }
            }
        }

        private void SetEmpOutdoor()
        {
            DateTime[] Temparr = _Tdas.Select(p => p.attendance_dt).ToArray();
            List<tbl_outdoor_request> tors = _context.tbl_outdoor_request.Where(p => p.r_e_id == _empId && Temparr.Contains(p.from_date) && p.is_deleted == 0 && p.is_final_approve == 1).ToList();
            if (tors == null)
            {
                return;
            }
            else if (tors.Count == 0)
            {
                return;
            }
            for (int Index = 0; Index < _Tdas.Count; Index++)
            {
                var tor = tors.FirstOrDefault(p => p.from_date == _Tdas[Index].attendance_dt);
                if (tor != null)
                {
                    _Tdas[Index].is_outdoor = 1;
                    _Tdas[Index].in_time = _Tdas[Index].in_time == _defaultDate ? tor.manual_in_time : DateTime.Compare(_Tdas[Index].in_time, tor.manual_in_time) < 0 ? _Tdas[Index].in_time : tor.manual_in_time;
                    _Tdas[Index].out_time = DateTime.Compare(_Tdas[Index].out_time, tor.manual_out_time) > 0 ? _Tdas[Index].out_time : tor.manual_out_time;
                }
            }

        }

        private void SetEmpAttendaceRegularize_8aug_21()
        {
            DateTime[] Temparr = _Tdas.Select(p => p.attendance_dt).ToArray();
            var tors = _context.tbl_attendace_request.Where(p => p.r_e_id == _empId && Temparr.Contains(p.from_date) && p.is_deleted == 0 && p.is_final_approve == 1).ToList();
            if (tors == null)
            {
                return;
            }
            else if (tors.Count == 0)
            {
                return;
            }
            for (int Index = 0; Index < _Tdas.Count; Index++)
            {
                var tor = tors.FirstOrDefault(p => p.from_date == _Tdas[Index].attendance_dt);
                if (tor != null)
                {
                    _Tdas[Index].is_regularize = 1;
                    _Tdas[Index].in_time = tor.manual_in_time;
                    _Tdas[Index].out_time = tor.manual_out_time;
                }

            }
        }

        private void SetEmpAttendaceRegularize() // modified by anil for cancelled applied revert to previous postion
        {
            DateTime[] Temparr = _Tdas.Select(p => p.attendance_dt).ToArray();
            var tors = _context.tbl_attendace_request.Where(p => p.r_e_id == _empId && Temparr.Contains(p.from_date)  && p.is_final_approve == 1).ToList();
            if (tors == null)
            {
                return;
            }
            else if (tors.Count == 0)
            {
                return;
            }
            for (int Index = 0; Index < _Tdas.Count; Index++)
            {
                var tor = tors.FirstOrDefault(p => p.is_deleted==0 && p.from_date == _Tdas[Index].attendance_dt);
                if (tor != null)
                {
                    _Tdas[Index].is_regularize = 1;
                    _Tdas[Index].in_time = tor.manual_in_time;
                    _Tdas[Index].out_time = tor.manual_out_time;
                }
                else
                {
                    var tor1 = tors.FirstOrDefault(p => p.is_deleted == 2 && p.from_date == _Tdas[Index].attendance_dt);
                    if (tor1 != null)
                    {
                        _Tdas[Index].is_regularize = 0;
                        _Tdas[Index].in_time = tor1.system_in_time;
                        _Tdas[Index].out_time = tor1.system_out_time;
                    }
                }

            }
        }

        private void SetEmpWeekoff()
        {
            //List<tbl_shift_week_off> WeekLists = null;
            //if (_IsFixedWeekoff != 2)
            //{
            //    int?[] Temparr = _Tdas.Select(p => p.s_d_id).ToArray();
            //    if (Temparr != null)
            //    {
            //        WeekLists = _context.tbl_shift_week_off.Where(p => Temparr.Contains(p.shift_detail_id) && p.is_active == 1).ToList();
            //        // WeekLists = _context.tbl_shift_week_off.Where(p => p.emp_id==_empId && p.is_active == 1).ToList();
            //    }
            //}
            //else
            //{
            //    WeekLists = _context.tbl_shift_week_off.Where(p => p.emp_id == _empId && p.is_active == 1).ToList();
            //}
            int is_fixed_weekly_off = 0;
            try
            {
                for (int Index = 0; Index < _Tdas.Count; Index++)
                {
                    is_fixed_weekly_off = 0;
                    int DayOfWeek_ = ((int)_Tdas[Index].attendance_dt.DayOfWeek == 0) ? 7 : (int)_Tdas[Index].attendance_dt.DayOfWeek;
                    int CurrentWeek = ((_Tdas[Index].attendance_dt.Day - 1) / 7) + 1;
                    var Datas = _context.tbl_emp_weekoff.Where(p => p.employee_id == _empId && p.effective_from_date <= _Tdas[Index].attendance_dt && p.is_deleted == 0).OrderByDescending(p => p.effective_from_date).ThenByDescending(p=>p.created_date) .FirstOrDefault();

                    if (Datas != null)
                    {
                        is_fixed_weekly_off = Datas.is_fixed_weekly_off;
                    }
                    if (is_fixed_weekly_off != 2)
                    {
                        if (_context.tbl_shift_week_off.Where(p => p.shift_detail_id == _Tdas[Index].s_d_id && p.days == CurrentWeek && p.week_day == DayOfWeek_ && p.is_active == 1).Count() > 0)
                        {
                            _Tdas[Index].is_weekly_off = 1;
                        }
                        else
                        {
                            _Tdas[Index].is_weekly_off = 0;
                        }
                    }
                    else
                    {
                        if (_context.tbl_shift_week_off.Where(p => p.emp_weekoff_id == Datas.emp_weekoff_id && p.days == CurrentWeek && p.week_day == DayOfWeek_ && p.is_active == 1).Count() > 0)
                        {
                            _Tdas[Index].is_weekly_off = 1;
                        }
                        else
                        {
                            _Tdas[Index].is_weekly_off = 0;
                        }
                    }
                    //if (_IsFixedWeekoff != 2)
                    //{
                    //    if (WeekLists.Any(p => p.week_day == DayOfWeek_ && p.days == CurrentWeek && p.shift_detail_id == _Tdas[Index].s_d_id))
                    //    {
                    //        _Tdas[Index].is_weekly_off = 1;
                    //    }
                    //    else
                    //    {
                    //        _Tdas[Index].is_weekly_off = 0;
                    //    }
                    //}
                    //else
                    //{
                    //    if (WeekLists.Any(p => p.week_day == DayOfWeek_ && p.days == CurrentWeek))
                    //    {
                    //        _Tdas[Index].is_weekly_off = 1;
                    //    }
                    //    else
                    //    {
                    //        _Tdas[Index].is_weekly_off = 0;
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {
            }

        }

        private void SetEmpHoliday()
        {
            int company_id = _context.tbl_user_master.Where(a => a.is_active == 1 && a.employee_id == _empId).Select(b => b.default_company_id).FirstOrDefault();
            List<tbl_holiday_master> Thms = _context.tbl_holiday_master.Where(p => p.is_active == 1 &&
            (p.is_applicable_on_all_emp == 1 ?p.is_applicable_on_all_emp == 1:(p.is_applicable_on_all_emp == 0 && p.tbl_holiday_master_emp_list.Where(q1 => q1.employee_id == _empId && q1.is_deleted == 0).Count() > 0)) &&
            (p.is_applicable_on_all_comp == 1?p.is_applicable_on_all_comp==1:(p.is_applicable_on_all_comp == 0 && p.tbl_holiday_master_comp_list.Where(q3 => q3.company_id == company_id && q3.is_deleted == 0).Count() > 0)) &&
            (p.is_applicable_on_all_location == 1?p.is_applicable_on_all_location==1 :(p.is_applicable_on_all_location == 0 && p.tbl_holiday_master_comp_list.Where(q3 => q3.location_id == _empLocationId && q3.is_deleted == 0).Count() > 0)) &&
            (p.is_applicable_on_all_religion == 1? p.is_applicable_on_all_religion == 1:(p.is_applicable_on_all_religion == 0 && p.tbl_holiday_mstr_rel_list.Where(q2 => q2.religion_id == _empReligionId && q2.is_deleted == 0).Count() > 0)) &&
             ((p.from_date <= _fromDate && p.to_date >= _fromDate) ||
            (p.from_date <= _toDate && p.to_date >= _toDate) ||
            (p.from_date >= _fromDate && p.to_date <= _toDate))
            ).OrderByDescending(p => p.holiday_id).ToList();


            //List<tbl_holiday_master> Thms = _context.tbl_holiday_master.Where(p => p.is_active == 1 &&
            //(p.is_applicable_on_all_emp == 1 || (p.is_applicable_on_all_emp == 0 && p.tbl_holiday_master_emp_list.Where(q1 => q1.employee_id == _empId && q1.is_deleted == 0).Count() > 0)) &&
            //(p.is_applicable_on_all_comp == 1 || (p.is_applicable_on_all_comp == 0 && p.tbl_holiday_master_comp_list.Where(q3 => q3.company_id == company_id && q3.is_deleted == 0).Count() > 0)) &&
            //(p.is_applicable_on_all_location == 1 || (p.is_applicable_on_all_location == 0 && p.tbl_holiday_master_comp_list.Where(q3 => q3.location_id == _empLocationId && q3.is_deleted == 0).Count() > 0)) &&
            //(p.is_applicable_on_all_religion == 1 || (p.is_applicable_on_all_religion == 0 && p.tbl_holiday_mstr_rel_list.Where(q2 => q2.religion_id == _empReligionId && q2.is_deleted == 0).Count() > 0)) &&
            // ((p.from_date <= _fromDate && p.to_date >= _fromDate) ||
            //(p.from_date <= _toDate && p.to_date >= _toDate) ||
            //(p.from_date >= _fromDate && p.to_date <= _toDate))
            //).OrderByDescending(p => p.holiday_id).ToList();

            if (Thms.Count == 0)
            {
                return;
            }
            for (int Index = 0; Index < _Tdas.Count; Index++)
            {
                var thm = Thms.FirstOrDefault(p => p.from_date <= _Tdas[Index].attendance_dt && p.to_date >= _Tdas[Index].attendance_dt && p.holiday_date.Date == _Tdas[Index].attendance_dt.Date);
                if (thm != null)
                {
                    _Tdas[Index].is_holiday = 1;
                    _Tdas[Index].holiday_id = thm.holiday_id;
                }
            }
        }

        private void SetEmpLeave()
        {
            List<tbl_leave_request> tlrs = _context.tbl_leave_request.Where(p => p.r_e_id == _empId && p.is_deleted == 0 && p.is_final_approve == 1 &&
            ((p.from_date <= _fromDate && p.to_date >= _fromDate) ||
            (p.from_date <= _toDate && p.to_date >= _toDate) ||
            (p.from_date >= _fromDate && p.to_date <= _toDate))
            ).ToList();

            if (tlrs.Count == 0)
            {
                return;
            }
            for (int Index = 0; Index < _Tdas.Count; Index++)
            {
                var thm = tlrs.Where(p => p.from_date <= _Tdas[Index].attendance_dt && p.to_date >= _Tdas[Index].attendance_dt).OrderBy(p=>p.leave_applicable_for).ToList();
                for (int thmIndex = 0; thmIndex < thm.Count; thmIndex++)
                {
                    if (thm[thmIndex].leave_type_id == 3) // for short leave
                    {
                        _Tdas[Index].leave_type = 3;
                    }
                    else
                    {
                        _Tdas[Index].leave_type = thm[thmIndex].leave_applicable_for;
                    }
                    _Tdas[Index].leave_request_id = thm[thmIndex].leave_request_id;
                    if (_Tdas[Index].leave_type == 3)
                    {
                        _Tdas[Index].short_leave_hour = 1; // thm[thmIndex].leave_applicable_in_hours_and_minutes.Hour;
                        _Tdas[Index].short_leave_minute = 30; // thm[thmIndex].leave_applicable_in_hours_and_minutes.Minute;
                    }

                    if (_context.tbl_leave_type.Where(p => p.leave_type_id == thm[thmIndex].leave_type_id && p.leave_type_name == "LWP").Count()>0)
                    {
                        _EmpLeaves.Add(new clsEmployeeLeave() { EmpId = _Tdas[Index].emp_id.Value, LeaveType = thm[thmIndex].leave_applicable_for, LeaveDay = _Tdas[Index].attendance_dt, IsLWP = true, IsShortLeave= thm[thmIndex].leave_type_id==3?true:false });
                    }
                    else
                    {
                        _EmpLeaves.Add(new clsEmployeeLeave() { EmpId = _Tdas[Index].emp_id.Value, LeaveType = thm[thmIndex].leave_applicable_for, LeaveDay = _Tdas[Index].attendance_dt, IsLWP = false, IsShortLeave = thm[thmIndex].leave_type_id == 3 ? true : false });
                    }
                    
                }
            }

        }

        private void SetEmpCompoff()
        {
            DateTime[] Temparr = _Tdas.Select(p => p.attendance_dt).ToArray();
            var tors = _context.tbl_comp_off_request_master.Where(p => p.r_e_id == _empId && Temparr.Contains(p.compoff_date) && p.is_deleted == 0 && p.is_final_approve == 1).ToList();
            if (tors == null)
            {
                return;
            }
            else if (tors.Count == 0)
            {
                return;
            }
            for (int Index = 0; Index < _Tdas.Count; Index++)
            {
                var tor = tors.FirstOrDefault(p => p.compoff_date == _Tdas[Index].attendance_dt);
                if (tor != null)
                {
                    _Tdas[Index].is_comp_off = 1;

                }
            }


        }

        private void SetEmpWorkingHour()
        {
            int OtRequiredMinute = 0;
            var Ot = _context.tbl_ot_rule_master.FirstOrDefault(p => p.is_active == 1);
            if (Ot != null)
            {
                OtRequiredMinute = (Ot.grace_working_hour * 60) + Ot.grace_working_minute;
            }
            int[] TempShiftID = _Tdas.Select(p => p.shift_id ?? 0).ToArray();
            var ShiftDetail = _context.tbl_shift_details.Where(p => p.is_ot_applicable == 1 && TempShiftID.Contains(p.shift_id ?? 0) && p.is_deleted == 0).ToList();

            for (int Index = 0; Index < _Tdas.Count; Index++)
            {
                if ((_Tdas[Index].in_time != _defaultDate) && (_Tdas[Index].out_time != _defaultDate))
                {

                    var workour = _Tdas[Index].out_time.Subtract(DateTime.Compare(_Tdas[Index].in_time, _Tdas[Index].shift_in_time) > 0 ? _Tdas[Index].in_time : _Tdas[Index].shift_in_time);
                    _Tdas[Index].working_hour_done = workour.Hours;
                    _Tdas[Index].working_minute_done = workour.Minutes;
                    if (!(_Tdas[Index].is_holiday == 1 || _Tdas[Index].is_comp_off == 1 || _Tdas[Index].is_weekly_off == 1))
                    {
                        //OT is enable so set the OT Hour
                        if (_EmpOtEnable == 1 ||
                           (_EmpOtEnable == 0 && ShiftDetail.FindIndex(p => p.shift_id == _Tdas[Index].shift_id) > -1)
                            )
                        {
                            var OTDone = ((_Tdas[Index].working_hour_done * 60) + _Tdas[Index].working_minute_done) - ((_Tdas[Index].w_hour_req_for_full_day * 60) + _Tdas[Index].w_min_req_for_full_day);
                            if (OTDone >= OtRequiredMinute)
                            {
                                _Tdas[Index].ot_hour_done = Convert.ToInt32(OTDone / 60);
                                _Tdas[Index].ot_minute_done = Convert.ToInt32(OTDone % 60);
                            }
                        }

                    }

                }

            }
        }

        private void SetEmpGraceperiod()
        {


            DateTime Payrollfromdate = Convert.ToDateTime("" + _payrollDate + _fromDate.ToString("-MMM-yyyy"));
            DateTime PayrolltoDate = Payrollfromdate.AddMonths(1).AddDays(-1);
            int InnerIndex = -1;
            int Index = -1;
            int totalAllowed = 0;
            //Payroll date is Greater then the curren from date then reduce the payroll date by one month
            if (DateTime.Compare(_fromDate, Payrollfromdate) < 0)
            {
                Payrollfromdate = Payrollfromdate.AddMonths(-1);
                PayrolltoDate = PayrolltoDate.AddMonths(-1);
            }

            do
            {
                List<GraceLimit> graceLimit = _context.tbl_daily_attendance.Where(p => p.emp_id == _empId && p.is_grace_applied == 1 && p.attendance_dt >= Payrollfromdate && p.attendance_dt <= PayrolltoDate)
                    .GroupBy(p => p.shift_id).Select(q => new GraceLimit { ShiftId = q.Key.Value, AlreadyApplied = q.Count(), TotalAvailable = 0, grace_time_for_late_punch = _defaultDate }).ToList();
                //now update the How many totalallowed for this Emp
                List<int> shifts = new List<int>();
                shifts.AddRange(graceLimit.Select(p => p.ShiftId).ToList());
                shifts.AddRange(_Tdas.Select(p => p.shift_id ?? 0).Distinct().ToList());
                var Overalldatas = _context.tbl_shift_details.Where(p => p.is_deleted == 0 && shifts.Contains(p.shift_id ?? 0) && p.is_deleted == 0).
                Select(p => new { p.shift_id, p.grace_time_for_late_punch, p.number_of_grace_time_applicable_in_month, is_fixed_shift = p.shift_type });

                foreach (var od in Overalldatas)
                {
                    Index = -1;
                    Index = graceLimit.FindIndex(p => p.ShiftId == od.shift_id);
                    if (Index > -1)
                    {
                        graceLimit[Index].TotalAvailable = od.number_of_grace_time_applicable_in_month;
                        graceLimit[Index].grace_time_for_late_punch = od.grace_time_for_late_punch;
                    }
                    else
                    {
                        graceLimit.Add(new GraceLimit() { ShiftId = od.shift_id ?? 0, AlreadyApplied = 0, TotalAvailable = od.number_of_grace_time_applicable_in_month, grace_time_for_late_punch = od.grace_time_for_late_punch });
                    }
                }

                DateTime Processingdate = DateTime.Compare(Payrollfromdate, _fromDate) > 0 ? Payrollfromdate : _fromDate;
                while (DateTime.Compare(Processingdate, PayrolltoDate) <= 0 && DateTime.Compare(Processingdate, _toDate) <= 0)
                {
                    Index = _Tdas.FindIndex(p => p.attendance_dt == Processingdate);
                    if (Index == -1)
                    {
                        goto EndofInnerWhileLoop;
                    }
                    if (_Tdas[Index].is_comp_off == 1)
                    {
                        goto EndofInnerWhileLoop;
                    }
                    if (_Tdas[Index].leave_type == 1)
                    {
                        goto EndofInnerWhileLoop;
                    }
                    if (_Tdas[Index].is_holiday == 1)
                    {
                        goto EndofInnerWhileLoop;
                    }
                    InnerIndex = -1;
                    InnerIndex = graceLimit.FindIndex(p => p.ShiftId == _Tdas[Index].shift_id);
                    totalAllowed = 0;
                    if (InnerIndex != -1)
                    {
                        totalAllowed = graceLimit[InnerIndex].grace_time_for_late_punch.Hour * 60 + graceLimit[InnerIndex].grace_time_for_late_punch.Minute;
                    }

                    //for Late in
                    if (DateTime.Compare(_Tdas[Index].in_time, _Tdas[Index].shift_max_in_time) > 0)
                    {
                        var times = _Tdas[Index].in_time.Subtract(_Tdas[Index].shift_max_in_time);
                        var totalminutelate = times.Hours * 60 + times.Minutes;

                        if (totalminutelate > 0 && totalminutelate <= totalAllowed && graceLimit[InnerIndex].TotalAvailable > graceLimit[InnerIndex].AlreadyApplied)
                        {
                            _Tdas[Index].is_grace_applied = 1;
                            _Tdas[Index].grace_period_hour = graceLimit[InnerIndex].grace_time_for_late_punch.Hour;
                            _Tdas[Index].grace_period_minute = graceLimit[InnerIndex].grace_time_for_late_punch.Minute;
                            graceLimit[InnerIndex].AlreadyApplied = graceLimit[InnerIndex].AlreadyApplied = 1;
                        }
                        _Tdas[Index].is_late_in = 1;
                        _Tdas[Index].early_or_late_in_hour = times.Hours;
                        _Tdas[Index].early_or_late_in_minute = times.Minutes;
                    }
                    //for early out
                    if ((DateTime.Compare(_Tdas[Index].out_time, _Tdas[Index].shift_out_time) < 0) && _defaultDate != _Tdas[Index].out_time)
                    {
                        //bool is_dynamicShift = false;
                        //if (Overalldata_ != -1)
                        //{
                        //    if (Overalldata[Overalldata_].is_fixed_shift == 2)
                        //    { is_dynamicShift = true; }
                        //}

                        //get the Total Working hour if total working hour is less required working hour
                        var totalminuteearly = (_Tdas[Index].w_hour_req_for_full_day * 60 + _Tdas[Index].w_min_req_for_full_day) -
                        (_Tdas[Index].working_hour_done * 60 + _Tdas[Index].working_minute_done);
                        if (totalminuteearly > 0 && totalminuteearly <= totalAllowed)
                        {
                            if (_Tdas[Index].is_grace_applied == 0 && graceLimit[InnerIndex].TotalAvailable > graceLimit[InnerIndex].AlreadyApplied)
                            {
                                _Tdas[Index].is_grace_applied = 1;
                                _Tdas[Index].grace_period_hour = graceLimit[InnerIndex].grace_time_for_late_punch.Hour;
                                _Tdas[Index].grace_period_minute = graceLimit[InnerIndex].grace_time_for_late_punch.Minute;
                                graceLimit[InnerIndex].AlreadyApplied = graceLimit[InnerIndex].AlreadyApplied = 1;
                            }

                        }
                        _Tdas[Index].is_early_out = 1;
                        _Tdas[Index].early_or_late_out_hour = Convert.ToInt32(totalminuteearly / 60);
                        _Tdas[Index].early_or_late_out_minute = Convert.ToInt32(totalminuteearly % 60);
                    }
                    //Now we are Calculate the Daily Status of the Emp

                    EndofInnerWhileLoop:
                    Processingdate = Processingdate.AddDays(1);
                    //Also Set the payroll Monthyear
                    if (Index != -1)
                    {
                        _Tdas[Index].payrollmonthyear = Convert.ToInt32(PayrolltoDate.ToString("yyyyMM"));
                    }
                }

                Payrollfromdate = Payrollfromdate.AddMonths(1);
                PayrolltoDate = PayrolltoDate.AddMonths(1);

            } while (DateTime.Compare(PayrolltoDate, _toDate.AddMonths(1)) < 0);

        }

        private void SetEmpDailyStatus()
        {
            bool IsFullDayLeave = false;
            bool IshalfDayLeave = false;
            bool IsShortDayLeave = false;
            for (int Index = 0; Index < _Tdas.Count; Index++)
            {
                IshalfDayLeave = false; IsFullDayLeave = false;IsShortDayLeave = false;

                if (_Tdas[Index].leave_request_id > 0)
                {
                    if (_EmpLeaves.Any(p => p.EmpId == _Tdas[Index].emp_id && p.LeaveDay == _Tdas[Index].attendance_dt && p.LeaveType == 1 && !p.IsLWP))
                    {
                        IsFullDayLeave = true;
                    }
                    else if (_EmpLeaves.Where(p => p.EmpId == _Tdas[Index].emp_id && p.LeaveDay == _Tdas[Index].attendance_dt && (p.LeaveType == 1 || p.LeaveType == 2) && !p.IsLWP).Count() > 1)
                    {
                        IsFullDayLeave = true;
                    }
                    else if(_EmpLeaves.Where(p => p.EmpId == _Tdas[Index].emp_id && p.LeaveDay == _Tdas[Index].attendance_dt && (p.LeaveType == 1 || p.LeaveType == 2) && !p.IsLWP).Count() == 1)
                    {
                        IshalfDayLeave = true;
                    }
                    if (_EmpLeaves.Any(p => p.EmpId == _Tdas[Index].emp_id && p.LeaveDay == _Tdas[Index].attendance_dt && p.LeaveType == 2 && p.IsShortLeave && !p.IsLWP))
                    {
                        IsShortDayLeave = true;
                    }
                }

                if (IsWeekOff(_Tdas[Index].leave_request_id ?? 0))
                {
                    _Tdas[Index].is_weekly_off = 1;
                }                

                if (!(_Tdas[Index].is_holiday == 1 || _Tdas[Index].is_comp_off == 1 || _Tdas[Index].is_weekly_off == 1))
                {
                    //working hour is greater then wRequired then marked as Present
                    if (((_Tdas[Index].w_hour_req_for_full_day * 60) + _Tdas[Index].w_min_req_for_full_day) <=
                            ((_Tdas[Index].working_hour_done * 60) + _Tdas[Index].working_minute_done))
                    {
                        _Tdas[Index].day_status = 1;
                    }
                    // if Workinghour +Grace Period is greater the Working hour then also present
                    else if (
                        ((_Tdas[Index].w_hour_req_for_full_day * 60) + _Tdas[Index].w_min_req_for_full_day) <=
                            ((_Tdas[Index].working_hour_done * 60) + _Tdas[Index].working_minute_done) +
                            ((_Tdas[Index].grace_period_hour * 60) + _Tdas[Index].grace_period_minute)
                       )
                    {
                        _Tdas[Index].day_status = 1;
                    }       
                    // if Short leave is taken then Also mark as present
                    else if (
                        ((_Tdas[Index].w_hour_req_for_full_day * 60) + _Tdas[Index].w_min_req_for_full_day) <=
                            ((_Tdas[Index].working_hour_done * 60) + _Tdas[Index].working_minute_done) +
                            ((_Tdas[Index].grace_period_hour * 60) + _Tdas[Index].grace_period_minute) +
                            ((_Tdas[Index].short_leave_hour * 60) + _Tdas[Index].short_leave_minute)
                       )
                    {
                        _Tdas[Index].day_status = 1;
                    }
                    // if Leave takken --check it is full day Leave
                    else if (IsFullDayLeave)
                    {
                        _Tdas[Index].day_status = 3; // change 2 to 3 by ravi
                    }
                    //add by ravi
                    else if ( (!IsFullDayLeave && !IshalfDayLeave) && 
                        ((_Tdas[Index].working_hour_done * 60) + _Tdas[Index].working_minute_done) < 
                        ((_Tdas[Index].w_hour_req_for_half_day * 60) + _Tdas[Index].w_min_req_for_half_day))
                    {
                        _Tdas[Index].day_status = 2;
                    }
                    //end by ravi

                    //half day leave takken but not short leave
                    else if (IshalfDayLeave==true && IsShortDayLeave==false)
                    {
                        //set Status half day Present and half day leave
                        if (((_Tdas[Index].w_hour_req_for_half_day * 60) + _Tdas[Index].w_min_req_for_half_day) <=
                            ((_Tdas[Index].working_hour_done * 60) + _Tdas[Index].working_minute_done) +
                            ((_Tdas[Index].grace_period_hour * 60) + _Tdas[Index].grace_period_minute))
                        {
                            _Tdas[Index].day_status = 5;
                        }
                        //set Status half day leave and half day absent
                        else
                        {
                            _Tdas[Index].day_status = 6;
                        }
                    }
                    //set Halday present and half day absent
                    else if (((_Tdas[Index].w_hour_req_for_half_day * 60) + _Tdas[Index].w_min_req_for_half_day) <=
                            ((_Tdas[Index].working_hour_done * 60) + _Tdas[Index].working_minute_done) +
                            ((_Tdas[Index].grace_period_hour * 60) + _Tdas[Index].grace_period_minute))
                    {
                        _Tdas[Index].day_status = 4;
                    }
                    else
                    {
                        _Tdas[Index].day_status = 10;
                    }
                    // if he is employee working hour is >required working hour but he is late more then grace hour
                    if (_Tdas[Index].day_status == 1 && ((_Tdas[Index].short_leave_hour * 60) + _Tdas[Index].short_leave_minute) == 0)
                    {
                        
                        if (((_Tdas[Index].grace_period_hour * 60) + _Tdas[Index].grace_period_minute) < ((_Tdas[Index].early_or_late_in_hour * 60) + _Tdas[Index].early_or_late_in_minute))
                        {
                            if (!(IsShortDayLeave || IshalfDayLeave))
                            {
                                _Tdas[Index].day_status = 4;
                            }
                            else
                            {
                                _Tdas[Index].day_status = 5;
                            }
                        }
                        

                    }

                    //if Single Punch Required then Check the single punch is avaible
                    if (_PunchType == 2)
                    {
                        _Tdas[Index].day_status = 1;
                    }
                    else if (_PunchType == 1)
                    {
                        if (DateTime.Compare(_defaultDate, _Tdas[Index].in_time) < 0)
                        { _Tdas[Index].day_status = 1; }
                        else if (DateTime.Compare(_defaultDate, _Tdas[Index].out_time) < 0)
                        { _Tdas[Index].day_status = 1; }
                    }
                }

                var _AttendanceDataManualtemp = _AttendanceDataManual.Where(p => p.attendance_dt == _Tdas[Index].attendance_dt).FirstOrDefault();
                if (_AttendanceDataManualtemp != null)
                {
                    if (_AttendanceDataManualtemp.day_status.HasValue)
                    {
                        _Tdas[Index].day_status = _AttendanceDataManualtemp.day_status.Value;
                    }
                }

            }
        }

        private void SetCompoffBalance()
        {
            _CompOffLeger = new List<tbl_comp_off_ledger>();
            int CompOffhourReq = 6 * 60;
            var Compoff = _context.tbl_comb_off_rule_master.FirstOrDefault(p => p.is_active == 1);
            if (Compoff != null)
            {
                CompOffhourReq = Compoff.minimum_working_hours * 60 + Compoff.minimum_working_minute;
            }
            if (_EmpCompOffEnable == 1)
            {
                for (int Index = 0; Index < _Tdas.Count; Index++)
                {

                    if ((_Tdas[Index].is_holiday == 1 || _Tdas[Index].is_comp_off == 1 || _Tdas[Index].is_weekly_off == 1))
                    {
                        if (
                           !(DateTime.Compare(_Tdas[Index].in_time, _defaultDate) == 0 || DateTime.Compare(_Tdas[Index].out_time, _defaultDate) == 0)
                            )
                        {
                            var CompDone = _Tdas[Index].out_time.Subtract(_Tdas[Index].in_time);
                            if (CompDone.Hours * 60 + CompDone.Minutes >= CompOffhourReq)
                            {
                                _CompOffLeger.Add(new tbl_comp_off_ledger()
                                {
                                    compoff_date = _Tdas[Index].attendance_dt.Date,
                                    credit = 1,
                                    dredit = 0,
                                    transaction_date = _currentDate,
                                    transaction_type = 1,
                                    monthyear = _Tdas[Index].payrollmonthyear,
                                    transaction_no = 0 - Convert.ToInt32(_Tdas[Index].attendance_dt.ToString("yyyyMMdd")),
                                    remarks = "",
                                    e_id = _empId
                                });
                            }
                        }

                    }
                    //else
                    //{
                    //    var CompDone =
                    //            ((_Tdas[Index].working_hour_done * 60) + _Tdas[Index].working_minute_done) -
                    //    ((_Tdas[Index].w_hour_req_for_full_day * 60) + _Tdas[Index].w_min_req_for_full_day);
                    //    if (CompDone >= CompOffhourReq)
                    //    {
                    //        _CompOffLeger.Add(new tbl_comp_off_ledger()
                    //        {
                    //            compoff_date = _Tdas[Index].attendance_dt,
                    //            credit = 1,
                    //            dredit = 0,
                    //            transaction_date = _currentDate,
                    //            transaction_type = 1,
                    //            monthyear = _Tdas[Index].payrollmonthyear,
                    //            transaction_no = 0 - Convert.ToInt32(_Tdas[Index].attendance_dt.ToString("yyyyMMdd")),
                    //            remarks = "",
                    //            e_id = _empId
                    //        });
                    //    }
                    //}
                }
            }
        }

        public void CalculateEmployeeAttendance()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    BasicSet();
                    _deleteExiting();
                    _attendanceBasicSet();
                    SetEmpWorkingHour();
                    SetEmpGraceperiod();
                    SetEmpDailyStatus();
                    SetCompoffBalance();

                    //Now Save the All data
                    _context.tbl_daily_attendance.AddRange(_Tdas);
                    _context.tbl_comp_off_ledger.AddRange(_CompOffLeger);
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Dictionary<List<HolidayData>, List<ShiftWeekOffData>> GetEmpWeekOff()
        {
            Dictionary<List<HolidayData>, List<ShiftWeekOffData>> AllData = new Dictionary<List<HolidayData>, List<ShiftWeekOffData>>();
            try
            {
                BasicSet();
                SetShift();
                SetEmpHoliday();
                SetEmpWeekoff();
                List<tbl_holiday_master> thm = new List<tbl_holiday_master>();
                thm = _context.tbl_holiday_master.Where(p => p.is_active == 1).ToList();

                List<HolidayData> HolidayData = (from a in _Tdas
                                                 join b in thm on a.holiday_id ?? 0 equals b.holiday_id //into b_temp                            
                                                                                                        //from b_value in b_temp.DefaultIfEmpty()
                                                 select (new HolidayData() { HolidayDate = a.attendance_dt, IsWeekOff = a.is_weekly_off, IsHoliday = a.is_holiday, HolidayId = a.holiday_id ?? 0, HolidayName = b.holiday_name ?? "" })
                            ).ToList();


                List<ShiftWeekOffData> ShiftData = _Tdas.Where(p => p.is_weekly_off == 1).Select(p => new ShiftWeekOffData() { AttendanceDate = p.attendance_dt, ShiftId = p.shift_id ?? 0, IsWeekOff = p.is_weekly_off }).ToList();
                //var TotalData = new
                //{
                //    HolidayData,
                //    ShiftData
                //};
                AllData.Add(HolidayData, ShiftData);

                return AllData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CurrentShiftData> Change_shift_dtl(bool is_notification = false)
        {
            List<CurrentShiftData> shift_dtl = new List<CurrentShiftData>();
            try
            {
                BasicSet();
                SetShift();
                SetEmpHoliday();
                SetEmpWeekoff();


                if (is_notification)
                {
                    var current_shift = (from a in _Tdas where a.attendance_dt.Date == DateTime.Now.Date select a).FirstOrDefault();

                    var tomorrow_shift = (from b in _Tdas where b.attendance_dt.Date == DateTime.Now.AddDays(1).Date select b).FirstOrDefault();
                    if (current_shift.shift_id != tomorrow_shift.shift_id && current_shift.s_d_id != tomorrow_shift.s_d_id)
                    {
                        CurrentShiftData objshift = new CurrentShiftData();
                        objshift.ShiftDetailId = tomorrow_shift.s_d_id ?? 0;
                        objshift.InTime = tomorrow_shift.shift_in_time.ToString();
                        objshift.OutTime = tomorrow_shift.shift_out_time.ToString();


                        shift_dtl.Add(objshift);
                    }
                }
                else
                {
                    for (int i = 0; i < _Tdas.Count; i++)
                    {
                        CurrentShiftData objcurrent = new CurrentShiftData();

                        objcurrent.ShiftDetailId = _Tdas[i].s_d_id ?? 0;
                        objcurrent.InTime = Convert.ToString(_Tdas[i].shift_in_time);
                        objcurrent.OutTime = Convert.ToString(_Tdas[i].shift_out_time);
                        objcurrent.shiftt_dt = _Tdas[i].attendance_dt;

                        shift_dtl.Add(objcurrent);

                    }
                }


            }
            catch (Exception ex)
            {

            }

            return shift_dtl;
        }



    }
}

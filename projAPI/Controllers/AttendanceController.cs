using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projAPI.Model;
using projContext;
using projContext.DB;
using Microsoft.AspNetCore.Hosting;
using projAPI.Classes;
using Microsoft.EntityFrameworkCore;

namespace projAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IHttpContextAccessor _AC;
        private readonly Context _context;
        private IHostingEnvironment _hostingEnvironment;
        clsCurrentUser _clsCurrentUser;
        clsLeaveCredit _clsLeaveCredit;
        public AttendanceController(Context context, IHostingEnvironment environment, IHttpContextAccessor AC, clsCurrentUser _clsCurrentUser, clsLeaveCredit _clsLeaveCredit)
        {
            _context = context;
            _hostingEnvironment = environment;
            _AC = AC;
            this._clsCurrentUser = _clsCurrentUser;
            this._clsLeaveCredit = _clsLeaveCredit;
        }

        [Route("SaveAttd")]
        [HttpPost]
        ////[Authorize(Policy = "10001")]
        public IActionResult SaveAttd(mdlSaveAttendance[] mdl_)
        {

            Response_Msg objResult = new Response_Msg();
            objResult.StatusCode = 0;
            try
            {
                //get the All Emp ID from their Card Number
                if (mdl_ == null)
                {
                    objResult.Message = "No Data for upload";
                    return Ok(objResult);
                }
                //_context.tbl_emp

                objResult.Message = "Save Succesully";
                return Ok(objResult);
            }
            catch (Exception ex)
            {

                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        //GET: api/Attendance
        [HttpGet]
        [Route("getmachines/{id}")]
        ////[Authorize(Policy = "10002")]
        public IEnumerable<tbl_machine_master> GetMachine(int id)
        {
            if (id > 0)
            {
                return _context.tbl_machine_master.Where(p => p.company_id == id && p.is_active == 1);
            }
            else
            {
                return _context.tbl_machine_master.Where(p => p.is_active == 1);
            }

        }



        // POST: api/Attendance
        [HttpPost]

        [Route("save_attendance")]
        //[Authorize(Policy = "10003")]
        public void save_attendance([FromBody] mdlSaveAttendance[] mdl, int companyID_)
        {
            if (mdl != null)
            {
                int companyID = companyID_;
                List<string> lstcardno = mdl.Select(p => p.card_number).Distinct().ToList();
                //cardno. 222  /     1054 employeeid brajesh sir
                List<mdlcommonReturn> mdlcommonReturn_ = _context.tbl_emp_officaial_sec.Where(p => p.is_deleted == 0 && lstcardno.Contains(p.card_number)
                // && p.tbl_employee_id_details.tbl_user_master.Any(q => q.default_company_id == companyID)
                ).Select(p => new mdlcommonReturn { val1 = p.employee_id, val2 = p.card_number }).ToList();
                // int? empID = null;
                DateTime currenttime = DateTime.Now;
                DateTime punch_time_ = DateTime.Now;
                // List<tbl_attendance_details> alldata = new List<tbl_attendance_details>();

                #region updated by anil kumar on 24th dec 20
                var attrecords = _context.tbl_attendance_details.ToList();
                var attndncelist = (from m in mdl
                                    join md in mdlcommonReturn_ on m.card_number equals md.val2
                                    // join at in _context.tbl_attendance_details on md.val1 equals at.emp_id
                                    where m.punch_time != null &&
                                    !attrecords.Any(p => p.emp_id == md.val1 && p.punch_time.AddSeconds(-p.punch_time.Second) == m.punch_time.AddSeconds(-m.punch_time.Second))
                                    //at.punch_time == m.punch_time 
                                    select new tbl_attendance_details
                                    {
                                        machine_id = m.machine_id,
                                        emp_id = md.val1.Value,
                                        punch_time = m.punch_time.AddSeconds(-m.punch_time.Second), // set second as 00
                                        entry_time = currenttime.AddSeconds(-currenttime.Second), // set second as 00
                                        enter_by_interface = 0
                                    }).ToList().Distinct();

                #endregion


                #region commented by anil kumar on 24th dec 20
                //for (int i = 0; i < mdl.Count(); i++)
                //{
                //    // empID = null;
                //    mdlcommonReturn mdlEmp = mdlcommonReturn_.FirstOrDefault(p => p.val2 == mdl[i].card_number);
                //    if (mdlEmp != null)
                //    {
                //        //empID = mdlEmp.val1;
                //        punch_time_ = Convert.ToDateTime(mdl[i].punch_time.ToString("dd-MMM-yyyy hh:mm:00 tt"));
                //        var attndncd = _context.tbl_attendance_details.Where(p => p.emp_id == mdlEmp.val1 && p.punch_time == punch_time_).FirstOrDefault();
                //        if (attndncd == null)
                //        {
                //            if (!alldata.Any(p => p.emp_id == mdlEmp.val1 && p.punch_time == punch_time_))
                //            {
                //                alldata.Add(new tbl_attendance_details { machine_id = mdl[i].machine_id, emp_id = mdlEmp.val1.Value, punch_time = punch_time_, entry_time = currenttime, enter_by_interface = 0 });
                //            }

                //        }
                //    }

                //}
                #endregion

                _context.tbl_attendance_details.AddRange(attndncelist);
                _context.SaveChanges();

                //tbl_attendance_details
            }

        }

        // PUT: api/Attendance/5
        [HttpPost]
        //[Authorize(Policy = "10004")]
        [Route("save_daily_attendance")]
        public void Post_save_attendance_details(DateTime? fromdate, DateTime? todate, int empid = 0)
        {
            try
            {
                if (fromdate == null)
                {
                    fromdate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy 00:00:00 tt"));
                }
                if (todate == null)
                {
                    todate = Convert.ToDateTime(DateTime.Now.AddDays(1).AddMinutes(-1).ToString("dd-MMM-yyyy hh:mm:ss tt"));
                }
                // empid = 1141;
                //if empid=0 it means we have process all Employee
                //else Process the Single Api
                List<tbl_emp_officaial_sec> teos = _context.tbl_emp_officaial_sec.Where(p => p.is_deleted == 0 && p.employee_id == (empid == 0 ? p.employee_id : empid)).ToList();
                Int32 PayrollDate = 1;
                var payroll = _context.tbl_payroll_month_setting.Where(p => p.is_deleted == 0).FirstOrDefault();
                if (payroll != null)
                {
                    // PayrollDate = Convert.ToInt32(payroll.from_date.ToString("dd"));
                    PayrollDate = Convert.ToInt32(payroll.from_date);
                }
                for (int Index = 0; Index < teos.Count; Index++)
                {
                    Classes.clsCalcualteDailyAttendance CalculateAttendance = new Classes.clsCalcualteDailyAttendance(
                    _context, teos[Index].employee_id ?? 0, teos[Index].emp_official_section_id, teos[Index].location_id ?? 0,
                    teos[Index].religion_id ?? 0, Convert.ToByte(teos[Index].is_fixed_weekly_off), PayrollDate, teos[Index].punch_type,
                    teos[Index].is_ot_allowed, Convert.ToByte(teos[Index].is_comb_off_allowed), fromdate.Value
                    , todate.Value, teos[Index].date_of_joining);
                    CalculateAttendance.CalculateEmployeeAttendance();

                }


            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        //[HttpGet]
        ////[Authorize(Policy = "129")]
        //[Route("Get_shift_of_emp")]
        public Dictionary<List<HolidayData>, List<ShiftWeekOffData>> Get_shift_of_emp(DateTime currentDt, int empid)
        {
            Dictionary<List<HolidayData>, List<ShiftWeekOffData>> Data = new Dictionary<List<HolidayData>, List<ShiftWeekOffData>>();
            try
            {
                DateTime? fromdate; DateTime? todate;

                fromdate = Convert.ToDateTime(currentDt.AddMonths(-1).ToString("dd-MMM-yyyy 00:00:00 tt"));
                todate = Convert.ToDateTime(currentDt.AddMonths(1).ToString("dd-MMM-yyyy 00:00:00 tt"));

                //if empid=0 it means we have process all Employee
                //else Process the Single Api
                List<tbl_emp_officaial_sec> teos = _context.tbl_emp_officaial_sec.Where(p => p.employee_id == empid && p.is_deleted == 0).ToList();
                Int32 PayrollDate = 1;
                var payroll = _context.tbl_payroll_month_setting.Where(p => p.is_deleted == 0).FirstOrDefault();
                if (payroll != null)
                {
                    // PayrollDate = Convert.ToInt32(payroll.from_date.ToString("dd"));
                    PayrollDate = Convert.ToInt32(payroll.from_date);
                }
                for (int Index = 0; Index < teos.Count; Index++)
                {
                    Classes.clsCalcualteDailyAttendance CalculateAttendance = new Classes.clsCalcualteDailyAttendance(
                        _context, teos[Index].employee_id ?? 0, teos[Index].emp_official_section_id, teos[Index].location_id ?? 0,
                        teos[Index].religion_id ?? 0, Convert.ToByte(teos[Index].is_fixed_weekly_off), PayrollDate, teos[Index].punch_type,
                        teos[Index].is_ot_allowed, Convert.ToByte(teos[Index].is_comb_off_allowed), fromdate.Value
                        , todate.Value, teos[Index].date_of_joining);
                    return CalculateAttendance.GetEmpWeekOff();

                }
                return Data;


            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        //GET: api/Attendance
        [HttpGet]
        //[Authorize(Policy = "10005")]
        [Route("getmachines1")]
        public IEnumerable<tbl_daily_attendance> Getatt1()
        {
            return _context.tbl_daily_attendance;
        }


        //GET: api/Attendance
        [HttpGet]
        [Route("getmachines2")]
        ////[Authorize(Policy = "10006")]
        public IEnumerable<tbl_employee_master> GetAtt2()
        {
            return _context.tbl_employee_master;
        }



        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        ////[Authorize(Policy = "10007")]
        public void Delete(int id)
        {
        }

        #region get employee Montahly and daily attendance data


        [Route("GetEmployeeMonthalyAtten/{CalenderDate}/{EmployeeId}/{for_all_emp}/{is_managerr_dec}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult GetEmployeeMonthalyAtten([FromRoute] DateTime CalenderDate, int EmployeeId, int for_all_emp, int is_managerr_dec)
        {
            try
            {
                ResponseMsg objResult = new ResponseMsg();

                if (for_all_emp == 1)
                {
                    var empids = _context.tbl_daily_attendance.Where(x => _clsCurrentUser.DownlineEmpId.Contains(x.emp_id ?? 0)).ToList();
                    foreach (var ids in empids)
                    {
                        if (!_clsCurrentUser.DownlineEmpId.Any(p => p == ids.emp_id))
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Unauthorize Access...!";
                            return Ok(objResult);
                        }
                    }
                }
                else
                {
                    if (!_clsCurrentUser.DownlineEmpId.Any(p => p == EmployeeId))
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Unauthorize Access...!";
                        return Ok(objResult);
                    }
                }


                DateTime FromDate = new DateTime();
                DateTime ToDate = new DateTime();
                //if from date null then set it with current month 
                //if (CalenderDate == null)
                //{
                //    FromDate = new DateTime(DateTime.Now.AddMonths(-1).Month == 12 ? DateTime.Now.AddYears(-1).Year : DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 26);
                //    ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 25);
                //}
                //else
                //{
                //    FromDate = new DateTime(CalenderDate.Value.AddMonths(-1).Month == 12 ? CalenderDate.Value.AddYears(-1).Year : CalenderDate.Value.Year, CalenderDate.Value.AddMonths(-1).Month, 26);
                //    ToDate = new DateTime(CalenderDate.Value.Year, CalenderDate.Value.Month, 25);
                //}

                //START get attadence detail according to payroll month circle

                var get_company_id = _context.tbl_employee_company_map.Where(x => x.employee_id == EmployeeId && x.is_default == true).FirstOrDefault();
                if (get_company_id == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid Access";
                    Ok(objResult);
                }

                var get_payroll_circle = _context.tbl_payroll_month_setting.Where(a => a.is_deleted == 0 && a.company_id == get_company_id.company_id).OrderByDescending(x => x.payroll_month_setting_id).FirstOrDefault();

                var getmonth = CalenderDate.Month;
                var getyear = CalenderDate.Year;
                DateTime nowdate = Convert.ToDateTime(getyear + "-" + getmonth + "-" + (get_payroll_circle != null ? get_payroll_circle.from_date : 1));

                var currnt_mnth_start_date = Convert.ToDateTime(getyear + "-" + getmonth + "-01"); // added by anil
                var currnt_mnth_end_date = Convert.ToDateTime(currnt_mnth_start_date.AddMonths(1).AddDays(-1)); // added by anil

                //if (CalenderDate >= nowdate)
                //{
                //    FromDate = Convert.ToDateTime(nowdate.ToString("dd-MMM-yyyy 00:00:00 tt"));
                //}
                //else
                //{
                FromDate = Convert.ToDateTime(nowdate.AddMonths(-1).ToString("dd-MMM-yyyy 00:00:00 tt"));
                //}


                //int todatee = get_payroll_circle[0].from_date - 1;
                //if (todatee == 0)
                //{
                //    todatee = 31;
                //}

                ToDate = FromDate.AddMonths(1).AddDays(-1);
                //if (ToDate > DateTime.Now)
                //{
                //    ToDate = DateTime.Now;
                //}

                // DateTime todateee = Convert.ToDateTime(getyear + "-" + getmonth + "-" + todatee);
                //ToDate = Convert.ToDateTime(todateee.AddMonths(1).ToString("dd-MMM-yyyy 00:00:00 tt"));

                ToDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy 00:00:00 tt")); //Convert.ToDateTime(todateee.ToString("dd-MMM-yyyy 00:00:00 tt")).AddDays(6);

                //END get attadence detail according to payroll month circle
                // FromDate = Convert.ToDateTime(CalenderDate.AddMonths(-1).ToString("dd-MMM-yyyy 00:00:00 tt"));
                // ToDate = Convert.ToDateTime(CalenderDate.AddMonths(1).ToString("dd-MMM-yyyy 00:00:00 tt"));

                List<tbl_daily_attendance> AttenList = _context.tbl_daily_attendance.Where(x => x.attendance_dt.Date >= FromDate.Date && x.attendance_dt.Date <= ToDate.Date && x.emp_id == EmployeeId).ToList();
                var outdoorlist = _context.tbl_outdoor_request.Where(x => x.from_date >= currnt_mnth_start_date && x.from_date <= currnt_mnth_end_date && x.is_deleted == 0 && x.r_e_id == EmployeeId && x.is_final_approve == 1).ToList();
                // var leavelist = _context.tbl_leave_request.Where(x => x.from_date >= currnt_mnth_start_date && x.to_date <= currnt_mnth_end_date && x.is_deleted == 0 && x.r_e_id == EmployeeId && x.is_final_approve == 1).ToList();

                var leavelist = _context.tbl_leave_request.Where(x => (x.from_date >= currnt_mnth_start_date && x.from_date <= currnt_mnth_end_date) && x.is_deleted == 0 && x.r_e_id == EmployeeId && x.is_final_approve == 1).ToList();
                var leavelist_next = _context.tbl_leave_request.Where(x => (x.to_date >= currnt_mnth_start_date && x.to_date <= currnt_mnth_end_date) && x.is_deleted == 0 && x.r_e_id == EmployeeId && x.is_final_approve == 1).ToList();
                // added by anil start
                var leave_dates = new List<DateTime>();
                for (var i = 0; i < leavelist.Count; i++)
                {
                    for (var dt = leavelist[i].from_date; dt <= leavelist[i].to_date; dt = dt.AddDays(1))
                    {
                        leave_dates.Add(dt);
                    }
                }
                for (var i = 0; i < leavelist_next.Count; i++)
                {
                    for (var dt = leavelist_next[i].from_date; dt <= leavelist_next[i].to_date; dt = dt.AddDays(1))
                    {
                        leave_dates.Add(dt);
                    }
                }
                leave_dates = leave_dates.Distinct().ToList();
                // added by anil end


                int company_id = _context.tbl_user_master.Where(a => a.is_active == 1 && a.employee_id == EmployeeId).Select(b => b.default_company_id).FirstOrDefault();

                var HolidayList = _context.tbl_holiday_master_comp_list.Where(a => a.tbl_holiday_master.is_active == 1 && a.company_id == company_id).Select(a => new
                {
                    holidaydate = a.tbl_holiday_master.holiday_date,
                    holidayname = a.tbl_holiday_master.holiday_name
                }).ToList().Distinct();

                // List<tbl_daily_attendance> AttenList = _context.tbl_daily_attendance.Where(x => x.attendance_dt.Date == Convert.ToDateTime("2021-01-24") && x.emp_id == EmployeeId).ToList();
                //----------set payroll calender data--------------------------
                List<PayrollCalenderData> PayrollCalender = new List<PayrollCalenderData>();


                PayrollCalender = AttenList.Select(TempIndex => new PayrollCalenderData
                {
                    AttendanceDate = TempIndex.attendance_dt,
                    AttStatus = TempIndex.day_status,
                    AttendanceStatus = TempIndex.is_outdoor == 1 ? "Outdoor" : TempIndex.is_holiday == 1 ? "Holiday" : TempIndex.is_weekly_off == 1 ? "Week Off" : TempIndex.is_comp_off == 1 ? "Com Off" : TempIndex.day_status == 1 ? "Present" : TempIndex.day_status == 2 ? "Absent" : TempIndex.day_status == 3 ? "On Leave" :
                     TempIndex.day_status == 4 ? "Half Day" : TempIndex.day_status == 5 ? "On Leave" : TempIndex.day_status == 6 ? "Half Day " : "",
                    // TempIndex.day_status == 4 ? "Half Day P/A" : TempIndex.day_status == 5 ? "Half Day P/L" : TempIndex.day_status == 6 ? "Half Day L/A"  : "", // add by ravi
                    //AttendanceStatus = TempIndex.day_status == 2 ? "Absent" : TempIndex.day_status == 3 ? "On Leave" : TempIndex.day_status == 4 ? "Half Day P" : "Present",

                    //InTime = TempIndex.in_time == null ? "" : ((TempIndex.is_weekly_off == 1 || TempIndex.is_holiday == 1) && (_context.tbl_compoff_raise.Where(x => x.comp_off_date.Date== TempIndex.attendance_dt.Date && x.emp_id == EmployeeId && x.is_final_approve == 1 && x.is_deleted == 0).Count() > 0)) ? TempIndex.in_time.ToString("MMM-dd-yyyy hh:mm tt") : (TempIndex.is_weekly_off == 0 && TempIndex.is_holiday == 0) ? TempIndex.in_time.ToString("MMM-dd-yyyy hh:mm tt") : "",
                    //OutTime = TempIndex.out_time == null ? "" : ((TempIndex.is_weekly_off == 1 || TempIndex.is_holiday == 1) && (_context.tbl_compoff_raise.Where(x => x.comp_off_date.Date == TempIndex.attendance_dt.Date && x.emp_id == EmployeeId && x.is_final_approve == 1 && x.is_deleted == 0).Count() > 0)) ? TempIndex.out_time.ToString("MMM-dd-yyyy hh:mm tt") : (TempIndex.is_weekly_off == 0 && TempIndex.is_holiday == 0) ? TempIndex.out_time.ToString("MMM-dd-yyyy hh:mm tt") : "",

                    InTime = TempIndex.in_time == null ? "" : TempIndex.in_time.ToString("MMM-dd-yyyy hh:mm tt"),
                    OutTime = TempIndex.out_time == null ? "" : TempIndex.out_time.ToString("MMM-dd-yyyy hh:mm tt"),


                    Events = TempIndex.is_outdoor == 1 ? "Outdoor" : TempIndex.day_status == 1 ? "Present" : TempIndex.day_status == 2 ? "Absent" : TempIndex.day_status == 3 ? "On Leave" : TempIndex.day_status == 4 ? "Half Day" : TempIndex.day_status == 5 ? "On Leave" : TempIndex.day_status == 6 ? "Half Day" : "",
                    //: TempIndex.day_status == 4 ? "Half Day P/A" : TempIndex.day_status == 5 ? "Half Day P/L" : TempIndex.day_status == 6 ? "Half Day L/A" :  "",
                    // ColorCode = TempIndex.is_weekly_off == 1 ? "#b3aeae" : TempIndex.is_holiday == 1 ? "pink" : TempIndex.is_comp_off == 1 ? "#e4e47e" : TempIndex.day_status == 2 ? "#ef925e" : TempIndex.day_status == 3 ? "#e46ee4" : TempIndex.day_status == 4 ? "#f7be9e" : TempIndex.day_status == 5 ? "#d2a1d2" : TempIndex.day_status == 6 ? "#e46ee447" : "#8b8be8",

                    ColorCode = TempIndex.is_outdoor == 1 ? "rgba(45, 123, 244, 0.18);color: #2d7bf4;" : TempIndex.is_weekly_off == 1 ? " #f1efef;" : TempIndex.is_holiday == 1 ? "rgba(234, 175, 4, 0.34); color: #946f04;" : TempIndex.is_comp_off == 1 ? "rgba(255, 103, 155, 0.18);color:#ff679b;" : TempIndex.day_status == 1 ? "rgba(10, 207, 151, 0.18);color:#059a70;" : TempIndex.day_status == 2 ? "rgba(241, 85, 108, 0.18);color: #f1556c;" : TempIndex.day_status == 3 ? "rgba(45, 123, 244, 0.18);color: #2d7bf4;" : TempIndex.day_status == 4 ? "#f9d9c6;color: #985c38;" : TempIndex.day_status == 5 ? " #f7d7f7;color: #a750a7;" : TempIndex.day_status == 6 ? "#9b6ee447;color: #785fa0;" : "",
                    IsWeekOff = TempIndex.is_weekly_off == 1 ? 1 : 0,
                    IsHoliday = TempIndex.is_holiday == 1 || HolidayList.Where(x => x.holidaydate.Date == TempIndex.attendance_dt.Date).Count() > 0 ? 1 : 0, //TempIndex.is_holiday
                    is_comp_off = TempIndex.is_comp_off,
                    is_outdoor = outdoorlist.Where(x => x.from_date == TempIndex.attendance_dt && x.is_auto == 0).Count() > 0 ? 1 : outdoorlist.Where(x => x.from_date == TempIndex.attendance_dt && x.is_auto == 1).Count() > 0 ? 0 : TempIndex.is_outdoor,
                    leave_request_id = TempIndex.leave_request_id == null ? 0 : Convert.ToInt32(TempIndex.leave_request_id),
                    is_leave_aprvd = 0,
                    is_outdoor_aprvd = 0,
                    is_shl = leavelist.Where(x => x.from_date == TempIndex.attendance_dt && x.leave_type_id == 3 && x.is_final_approve == 1).Count() > 0 ? 1 : 0,
                    is_comp_off_apprvd = _context.tbl_compoff_raise.Where(x => x.comp_off_date.Date == TempIndex.attendance_dt.Date && x.emp_id == EmployeeId && x.is_final_approve == 1 && x.is_deleted == 0).Count() > 0 ? 1 : 0,
                }).ToList();


                //update 0 for all days in list
                // Day.ToList().ForEach(p => new PayrollCalenderData { AttStatus = 0 });

                //PayrollCalender = AttenList.Select(p => new PayrollCalenderData
                //{
                //    AttendanceDate = Convert.ToDateTime(p.attendance_dt.ToString("dd-MMM-yyyy")),
                //}).ToList();
                //get all half day statuis id
                List<byte> HalfDayStatus = new List<byte>();
                HalfDayStatus.Add(4);
                HalfDayStatus.Add(5);
                HalfDayStatus.Add(6);
                //--------------get attendance summary data-----------------
                //1-Full day Present,2-Full day Absent,3 Full Day Leave, 4 half day present-half day Absent,5 half day present- half day leave,6 Half day leave-Halfday absent


                //star Get Data according to monthly setting

                int month_circle_to_date = get_payroll_circle != null ? (get_payroll_circle.from_date == 1 ? 31 : get_payroll_circle.from_date - 1) : 30;

                DateTime cirle_to_date = Convert.ToDateTime(getyear + "-" + getmonth + "-" + month_circle_to_date);

                var month_circle_atten_summary = AttenList.Where(p => p.attendance_dt.Date >= FromDate.Date && p.attendance_dt.Date <= cirle_to_date.Date);


                AttendanceSummaryData AttSummary = new AttendanceSummaryData();
                AttSummary.Present = month_circle_atten_summary.Where(p => p.day_status == 1).Count();
                AttSummary.Absent = month_circle_atten_summary.Where(p => p.day_status == 2).Count();
                AttSummary.LateInEarliOut = month_circle_atten_summary.Where(p => p.shift_in_time < p.in_time && p.shift_out_time > p.out_time).Count();
                AttSummary.Leave = month_circle_atten_summary.Where(p => p.day_status == 3).Count();
                AttSummary.Outdoor = month_circle_atten_summary.Where(p => p.is_outdoor == 1).Count();
                AttSummary.Compoff = month_circle_atten_summary.Where(p => p.is_comp_off == 1).Count();
                AttSummary.WeeklyOff = month_circle_atten_summary.Where(p => p.is_weekly_off == 1).Count();
                AttSummary.Holidays = month_circle_atten_summary.Where(p => p.is_holiday == 1).Count();
                AttSummary.HalfDay = month_circle_atten_summary.Where(p => HalfDayStatus.Contains(p.day_status)).Count();
                AttSummary.FromDate = FromDate.ToString();
                AttSummary.ToDate = cirle_to_date.ToString();


                int halfdaypresent = month_circle_atten_summary.Where(p => p.day_status == 4 || p.day_status == 5).Count();
                decimal totalhalfday = Convert.ToDecimal(halfdaypresent * (0.5));
                AttSummary.Present = AttSummary.Present + totalhalfday; // add half day present or full day present

                // 4 half day present - half day Absent,6 Half day leave - Halfday absent
                int halfdayabsent = month_circle_atten_summary.Where(p => p.day_status == 4 || p.day_status == 6).Count();
                decimal totalhalfdayabsent = Convert.ToDecimal(halfdayabsent * 0.5);
                AttSummary.Absent = AttSummary.Absent + totalhalfdayabsent; // add half day absent or full day absent

                //5 half day present - half day leave,6 Half day leave - Halfday absent

                int halfdayleave = month_circle_atten_summary.Where(p => p.day_status == 5 || p.day_status == 6).Count();
                decimal totalhalfdayleave = Convert.ToDecimal(halfdayleave * 0.5);
                AttSummary.Leave = AttSummary.Leave + totalhalfdayleave; // add half day leave or full day leave

                //end Get Data according to monthly setting

                //AttendanceSummaryData AttSummary = new AttendanceSummaryData();
                //AttSummary.Present = AttenList.Where(p => p.day_status == 1).Count();
                //AttSummary.Absent = AttenList.Where(p => p.day_status == 2).Count();
                //AttSummary.LateInEarliOut = AttenList.Where(p => p.shift_in_time < p.in_time && p.shift_out_time > p.out_time).Count();
                //AttSummary.Leave = AttenList.Where(p => p.day_status == 3).Count();
                //AttSummary.Outdoor = AttenList.Where(p => p.is_outdoor == 1).Count();
                //AttSummary.Compoff = AttenList.Where(p => p.is_comp_off == 1).Count();
                //AttSummary.WeeklyOff = AttenList.Where(p => p.is_weekly_off == 1).Count();
                //AttSummary.Holidays = AttenList.Where(p => p.is_holiday == 1).Count();
                //AttSummary.HalfDay = AttenList.Where(p => HalfDayStatus.Contains(p.day_status)).Count();
                //AttSummary.FromDate = FromDate.ToString();
                //AttSummary.ToDate = ToDate.ToString();

                //supriya 26-12-2019

                // 4 half day present - half day Absent,5 half day present - half day leave
                // int halfdaypresent = AttenList.Where(p => p.day_status == 4||p.day_status==5).Count();
                // decimal totalhalfday = Convert.ToDecimal(halfdaypresent * (0.5));
                // AttSummary.Present = AttSummary.Present + totalhalfday; // add half day present or full day present

                //// 4 half day present - half day Absent,6 Half day leave - Halfday absent
                // int halfdayabsent = AttenList.Where(p => p.day_status == 4 || p.day_status==6).Count();
                // decimal totalhalfdayabsent = Convert.ToDecimal(halfdayabsent * 0.5);
                // AttSummary.Absent = AttSummary.Absent + totalhalfdayabsent; // add half day absent or full day absent

                // //5 half day present - half day leave,6 Half day leave - Halfday absent

                // int halfdayleave = AttenList.Where(p => p.day_status == 5|| p.day_status == 6).Count();
                // decimal totalhalfdayleave = Convert.ToDecimal(halfdayleave * 0.5);
                // AttSummary.Leave = AttSummary.Leave + totalhalfdayleave; // add half day leave or full day leave

                //supriya 26-12-2019

                //--------------------balance leave from leave ledger table---------------

                List<BalanceLeaveData> BalanceLeave = new List<BalanceLeaveData>();
                BalanceLeave = _context.tbl_leave_type.Where(p => p.is_active == 1).Select(p => new BalanceLeaveData { LeaveTypeId = p.leave_type_id, LeaveInfoName = p.leave_type_name }).ToList();

                var TempLeaveBData = _clsLeaveCredit.Get_Leave_Ledger_By_Employee_Id(EmployeeId).Select(p => new
                {
                    LeaveTypeId = p.leave_type_id,
                    TotalCredit = p.credit,
                    TotalDebit = p.dredit,
                    LeaveBalance = p.balance
                }).ToList();


                //    _context.tbl_leave_ledger.Where(p => p.e_id == EmployeeId && p.tbl_leave_type.leave_type_id == p.leave_type_id
                //  && p.tbl_leave_type.is_active == 1).ToList().GroupBy(q => q.leave_type_id)
                //.Select(r => new //BalanceLeaveData()
                //{
                //    LeaveTypeId = Convert.ToInt32(r.Key),
                //    TotalCredit = r.Sum(a => a.credit),
                //    TotalDebit = r.Sum(a => a.dredit),
                //    LeaveBalance = r.Sum(a => a.credit) - r.Sum(a => a.dredit),
                //}).ToList();


                //BalanceLeave.ForEach(p =>
                //{
                //    var LeaveTypeName = _context.tbl_leave_type.FirstOrDefault(a => a.leave_type_id == p.LeaveTypeId);
                //    p.LeaveInfoName = LeaveTypeName.leave_type_name;
                //});

                BalanceLeave.ForEach(a =>
                {
                    var Leave = TempLeaveBData.FirstOrDefault(p => p.LeaveTypeId == a.LeaveTypeId);
                    a.TotalCredit = Leave != null ? Leave.TotalCredit : 0;
                    a.TotalDebit = Leave != null ? Leave.TotalDebit : 0;
                    a.LeaveBalance = Leave != null ? Leave.LeaveBalance : 0;
                });


                //----------------Get Current Shift Data---------------------
                CurrentShiftData CurrentShift = new CurrentShiftData();
                if (AttenList.Count > 0)
                {
                    CurrentShift = AttenList.Where(p => p.s_d_id != null || p.s_d_id != 0).OrderByDescending(q => q.attendance_dt)
                        .Select(p => new CurrentShiftData
                        {
                            ShiftDetailId = p.s_d_id ?? 0,
                            AttendanceStatus = p.day_status == 1 ? "Present" : p.day_status == 2 ? "Absent" : p.day_status == 3 ? "Leave" : p.day_status == 4 ? "Half Day P/A" : p.day_status == 5 ? "Half Day P/L" : p.day_status == 6 ? "Half Day L/A" : "",
                        }).Last();
                    var ShiftDetail = _context.tbl_shift_details.Where(p => p.shift_details_id == CurrentShift.ShiftDetailId && p.is_deleted == 0).FirstOrDefault();
                    //set data
                    if (ShiftDetail != null)
                    {
                        CurrentShift.InTime = ShiftDetail.punch_in_time.ToString("hh:mm tt");
                        CurrentShift.OutTime = ShiftDetail.punch_out_time.ToString("hh:mm tt");
                        CurrentShift.GraceInTime = ShiftDetail.punch_in_max_time.ToString("hh:mm tt");
                        CurrentShift.TotalShiftHrs = Convert.ToString(ShiftDetail.maximum_working_hours) + ":" + Convert.ToString(ShiftDetail.maximum_working_minute);
                        CurrentShift.CurrentShiftName = ShiftDetail.shift_name;
                    }

                }
                else
                {
                    var emp_shift = _context.tbl_emp_shift_allocation.Where(p => p.employee_id == EmployeeId && p.applicable_from_date <= CalenderDate.Date && p.is_deleted == 0).OrderByDescending(p => p.created_date).FirstOrDefault()?.shift_id ?? 0;

                    if (emp_shift != 0)
                    {
                        var ShiftDetail = _context.tbl_shift_details.Where(p => p.shift_id == emp_shift && p.is_deleted == 0).FirstOrDefault();
                        if (ShiftDetail != null)
                        {
                            CurrentShift.InTime = ShiftDetail.punch_in_time.ToString("hh:mm tt");
                            CurrentShift.OutTime = ShiftDetail.punch_out_time.ToString("hh:mm tt");
                            CurrentShift.GraceInTime = ShiftDetail.punch_in_max_time.ToString("hh:mm tt");
                            CurrentShift.TotalShiftHrs = Convert.ToString(ShiftDetail.maximum_working_hours) + ":" + Convert.ToString(ShiftDetail.maximum_working_minute);
                            CurrentShift.CurrentShiftName = ShiftDetail.shift_name;
                        }
                    }


                }
                //--------get holiday list for current year


                //var HolidayList = _context.tbl_holiday_master.Where(p => p.is_active == 1 && p.holiday_date.Year == FromDate.Year && p.co).Select(p => new
                //{
                //    holidaydate = p.holiday_date.ToString("MMM-dd-yyyy"),
                //    holidayname = p.holiday_name
                //}).ToList();

                //set employee holiday list and shift data for weekoff
                Dictionary<List<HolidayData>, List<ShiftWeekOffData>> WH = Get_shift_of_emp(CalenderDate, EmployeeId);
                List<HolidayData> HDL = new List<HolidayData>();
                List<ShiftWeekOffData> SDL = new List<ShiftWeekOffData>();
                foreach (KeyValuePair<List<HolidayData>, List<ShiftWeekOffData>> pair in WH)
                {
                    HDL = pair.Key;
                    SDL = pair.Value;
                }
                //update payrollcalender list for employee holiday                
                PayrollCalender.ForEach(p =>
                {
                    var h = HDL.FirstOrDefault(a => a.HolidayDate == p.AttendanceDate);
                    p.IsHoliday = h != null ? 1 : 0;
                    p.HolidayDate = h != null ? h.HolidayDate : Convert.ToDateTime("01-01-2000");
                    p.HolidayName = h != null ? h.HolidayName : "";
                    p.ColorCode = h != null ? "rgba(234, 175, 4, 0.34); color: #946f04;" : p.ColorCode;

                    //now remove
                    if (h != null)
                    {
                        HDL.RemoveAll(b => b.HolidayDate == h.HolidayDate);
                    }



                    //shift
                    var s = SDL.FirstOrDefault(a => a.AttendanceDate == p.AttendanceDate);
                    p.IsWeekOff = s != null ? 1 : 0;
                    p.WeekOffDate = s != null ? s.AttendanceDate : Convert.ToDateTime("01-01-2000");
                    p.AttendanceStatus = s != null ? "Week Off" : p.AttendanceStatus;
                    //p.ColorCode = s != null ? "#b3aeae" : p.ColorCode;
                    p.ColorCode = s != null ? "#f1efef" : p.ColorCode;

                    //now remove
                    if (s != null)
                    {
                        SDL.RemoveAll(b => b.AttendanceDate == p.AttendanceDate);
                    }

                    leave_dates.RemoveAll(x => x.Date == p.AttendanceDate);
                    outdoorlist.RemoveAll(x => x.from_date == p.AttendanceDate);
                    //HolidayList.RemoveAll(x =>Convert.ToDateTime(x.holidaydate) == p.AttendanceDate);
                    ////future approved leave added by anil
                    //var lv = leave_dates.FirstOrDefault(x => x.Date == p.AttendanceDate);
                    //p.leave_request_id = lv != null ? 1 : 0;

                    //if(lv!=null)
                    //{
                    //    leave_dates.RemoveAll(x => x.Date == p.AttendanceDate);
                    //}
                    //// added by anil end              
                });

                HDL.ForEach(TempIndex =>
                {
                    PayrollCalender.Add(new PayrollCalenderData
                    {
                        AttendanceDate = TempIndex.HolidayDate,
                        AttStatus = 0,
                        AttendanceStatus = TempIndex.HolidayName,
                        InTime = "01-01-2000",
                        OutTime = "01-01-2000",
                        Events = TempIndex.HolidayName,
                        ColorCode = "rgba(234, 175, 4, 0.34); color: #946f04;",
                        IsHoliday = 1,
                        HolidayDate = TempIndex.HolidayDate,
                        HolidayName = TempIndex.HolidayName
                    });
                });

                SDL.ForEach(TempIndex =>
                {
                    PayrollCalender.Add(new PayrollCalenderData
                    {
                        AttendanceDate = TempIndex.AttendanceDate,
                        AttStatus = 0,
                        AttendanceStatus = "Week Off",
                        InTime = "01-01-2000",
                        OutTime = "01-01-2000",
                        Events = "Week Off",
                        ColorCode = "#f1efef", //"#b3aeae",
                        IsWeekOff = 1,
                        WeekOffDate = TempIndex.AttendanceDate
                    });
                });

                // added by anil start
                leave_dates.ForEach(TempIndex =>
                {
                    PayrollCalender.Add(new PayrollCalenderData
                    {
                        AttendanceDate = TempIndex.Date,
                        AttStatus = 0,
                        InTime = "01-01-2000",
                        OutTime = "01-01-2000",
                        AttendanceStatus = "On Leave",
                        Events = "On Leave",
                        //ColorCode = "#f1efef", //"#b3aeae",
                        is_leave_aprvd = 1,
                    });
                });

                var outdoorlist_no_app = outdoorlist.Where(x => x.is_auto == 0).ToList();
                outdoorlist_no_app.ForEach(TempIndex =>
                {
                    PayrollCalender.Add(new PayrollCalenderData
                    {
                        AttendanceDate = TempIndex.from_date,
                        AttStatus = 0,
                        InTime = TempIndex.manual_in_time == null ? "" : TempIndex.manual_in_time.ToString("MMM-dd-yyyy hh:mm tt"),
                        OutTime = TempIndex.manual_out_time == null ? "" : TempIndex.manual_out_time.ToString("MMM-dd-yyyy hh:mm tt"),
                        //InTime = TempIndex.manual_in_time.ToString("hh:mm tt"),
                        //OutTime = TempIndex.manual_out_time.ToString("hh:mm tt"),
                        //InTime = "01-01-2000",
                        //OutTime = "01-01-2000",
                        AttendanceStatus = "Outdoor",
                        Events = "Outdoor",
                        is_outdoor_aprvd = 1,
                    });
                });

                var duplicateholiday = PayrollCalender.Where(x => x.IsHoliday == 1).Select(x => x.AttendanceDate).ToList();
                var HolidayList_ = HolidayList.Where(x => !duplicateholiday.Contains(Convert.ToDateTime(x.holidaydate))).ToList();

                PayrollCalender.ForEach(p =>
                {
                    var h = HolidayList_.FirstOrDefault(a => a.holidaydate == p.AttendanceDate);
                    p.IsHoliday = h != null ? 1 : 0;
                    p.HolidayDate = h != null ? h.holidaydate : Convert.ToDateTime("01-01-2000");
                    p.HolidayName = h != null ? h.holidayname : "";
                    p.ColorCode = h != null ? "rgba(234, 175, 4, 0.34); color: #946f04;" : p.ColorCode;

                    //now remove
                    if (h != null)
                    {
                        HolidayList_.RemoveAll(b => b.holidaydate == h.holidaydate);
                    }

                });

                HolidayList_.ForEach(TempIndex =>
                {
                    PayrollCalender.Add(new PayrollCalenderData
                    {
                        AttendanceDate = Convert.ToDateTime(TempIndex.holidaydate),
                        AttStatus = 0,
                        AttendanceStatus = TempIndex.holidayname,
                        InTime = "01-01-2000",
                        OutTime = "01-01-2000",
                        Events = TempIndex.holidayname,
                        ColorCode = "rgba(234, 175, 4, 0.34); color: #946f04;",
                        IsHoliday = 1,
                        HolidayDate = Convert.ToDateTime(TempIndex.holidaydate),
                        HolidayName = TempIndex.holidayname
                    });
                });
                // added by anil end

                var Data = new
                {
                    PayrollCalender,
                    AttSummary,
                    BalanceLeave,
                    CurrentShift,
                    HolidayList_
                };

                return Ok(Data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //get all holidaylist first time
        [Route("GetHolidayList")]
        [HttpGet]
        ////[Authorize(Policy = "10009")]
        // ////[Authorize(Policy ="")]
        public IActionResult GetHolidayList()
        {
            try
            {
                var HolidayList = _context.tbl_holiday_master.Where(p => p.is_active == 1 && p.holiday_date.Year == DateTime.Now.Year).Select(p => new
                {
                    holidaydate = p.holiday_date.ToString("MMM-dd-yyyy"),
                    holidayname = p.holiday_name
                }).ToList();

                return Ok(HolidayList);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //get selected date attendance details
        [HttpGet]
        [Route("GetSelectedDateAtten/{CalenderDate}/{EmployeeId}")]
        ////[Authorize(Policy = "10010")]
        public IActionResult GetSelectedDateAtten([FromRoute] DateTime? CalenderDate, int EmployeeId)
        {
            try
            {
                SelectedDateData GetSelectedData = new SelectedDateData();
                GetSelectedData = _context.tbl_daily_attendance.Where(p => p.tbl_shift_details.shift_details_id == p.s_d_id
                  && p.attendance_dt.Date == CalenderDate.Value.Date && p.emp_id == EmployeeId).Select(p => new SelectedDateData
                  {
                      ActualTimeIn = p.in_time.ToString("hh:mm tt"),
                      ActualTimeOut = p.out_time.ToString("hh:mm tt"),
                      ActualToatlShiftHrs = Convert.ToString(p.working_hour_done) + ":" + Convert.ToString(p.working_minute_done),
                      ActualAttendanceStatus = p.day_status == 2 ? "Absent" : p.day_status == 3 ? "On Leave" : p.day_status == 4 ? "Half Day Present" : "Present",
                      ShiftName = p.tbl_shift_details.shift_name,

                  }).FirstOrDefault();


                if (GetSelectedData == null)
                {
                    GetSelectedData = new SelectedDateData();
                }

                //GetSelectedData.leave_d_per_day_list = _context.tbl_leave_request.Where(x => CalenderDate.Value.Date >= x.from_date && CalenderDate.Value.Date <= x.to_date && x.r_e_id == EmployeeId && x.is_final_approve == 1 && x.is_deleted == 0)
                //                                   .Select(v => new leavedetails_per_day
                //                                   {
                //                                       leave_request_id = Convert.ToInt32(v.leave_request_id),
                //                                       leavetype = v.leave_type_id == 3 ? "Short Leave" : v.leave_applicable_for == 1 ? "Full Day" : v.leave_applicable_for == 2 ? "Half Day" : v.leave_applicable_for == 3 ? "HH:MM" : "",
                //                                       // leavetype = v.leave_type_id == 1 ? "Full Day" : v.leave_type_id == 2 ? "Half Day" : v.leave_type_id == 3 ? "Short Leave" : "",
                //                                       fromdate = Convert.ToDateTime(v.from_date).ToString("dd-MMM-yyyy"),
                //                                       todate = Convert.ToDateTime(v.to_date).ToString("dd-MMM-yyyy"),
                //                                       //fromdate = Convert.ToDateTime(p.in_time).ToString("dd-MMM-yyyy"),
                //                                       //todate = Convert.ToDateTime(p.out_time).ToString("dd-MMM-yyyy"),
                //                                       partialdays = v.leave_applicable_for == 1 ? "" : v.day_part == 1 ? "First Half" : "Second Half",// v.leave_applicable_for == 1 ? "" : (v.leave_applicable_for == 2 && v.day_part == 1) ? "Half Day - First Half" : (v.leave_applicable_for == 2 && v.day_part == 2) ? "Half Day - Second Half" : "",
                //                                       reason = v.requester_remarks,
                //                                       attendance_status = v.is_final_approve == 1 ? "Approved" : "Rejected",

                //                                   }).ToList();

                GetSelectedData.leave_d_per_day_list = (from lv in _context.tbl_leave_request
                                                        join ty in _context.tbl_leave_type on lv.leave_type_id equals ty.leave_type_id
                                                        where CalenderDate.Value.Date >= lv.from_date && CalenderDate.Value.Date <= lv.to_date
                                                        && lv.r_e_id == EmployeeId && lv.is_final_approve == 1 && lv.is_deleted == 0
                                                        select new leavedetails_per_day
                                                        {
                                                            leave_request_id = Convert.ToInt32(lv.leave_request_id),
                                                            leavetype = ty.leave_type_name,
                                                            duration = lv.leave_type_id == 3 ? "Short Leave" : lv.leave_applicable_for == 1 ? "Full Day" : lv.leave_applicable_for == 2 ? "Half Day" : lv.leave_applicable_for == 3 ? "HH:MM" : "",
                                                            // leavetype = v.leave_type_id == 1 ? "Full Day" : v.leave_type_id == 2 ? "Half Day" : v.leave_type_id == 3 ? "Short Leave" : "",
                                                            fromdate = Convert.ToDateTime(lv.from_date).ToString("dd-MMM-yyyy"),
                                                            todate = Convert.ToDateTime(lv.to_date).ToString("dd-MMM-yyyy"),
                                                            //fromdate = Convert.ToDateTime(p.in_time).ToString("dd-MMM-yyyy"),
                                                            //todate = Convert.ToDateTime(p.out_time).ToString("dd-MMM-yyyy"),
                                                            partialdays = lv.leave_applicable_for == 1 ? "Full Day" : lv.day_part == 1 ? "First Half" : "Second Half",// v.leave_applicable_for == 1 ? "" : (v.leave_applicable_for == 2 && v.day_part == 1) ? "Half Day - First Half" : (v.leave_applicable_for == 2 && v.day_part == 2) ? "Half Day - Second Half" : "",
                                                            reason = lv.requester_remarks,
                                                            attendance_status = lv.is_final_approve == 1 ? "Approved" : "Rejected",

                                                        }).ToList();

                GetSelectedData.outdoor_details_list = _context.tbl_outdoor_request.Where(x => x.from_date == CalenderDate.Value.Date && x.r_e_id == EmployeeId && x.is_final_approve == 1 && x.is_deleted == 0)
                                                  .Select(v => new outdoor_details_per_day
                                                  {
                                                      outdoor_request_id = Convert.ToInt32(v.leave_request_id),
                                                      outdoor_date = Convert.ToDateTime(v.from_date).ToString("dd-MMM-yyyy"),
                                                      in_time = v.manual_in_time.ToString("hh:mm tt"),
                                                      out_time = v.manual_out_time.ToString("hh:mm tt"),
                                                      user_reason = v.requester_remarks,
                                                      outdoor_status = v.is_final_approve == 1 ? "Approved" : "Rejected",
                                                      aprv_remarks = string.IsNullOrEmpty(v.admin_remarks) ? "" : v.admin_remarks,

                                                  }).ToList();


                return Ok(GetSelectedData);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetSelectedDateAtten11/{CalenderDate}/{EmployeeId}")]
        ////[Authorize(Policy = "10010")]
        public IActionResult GetSelectedDateAtten11([FromRoute] DateTime? CalenderDate, int EmployeeId)
        {
            try
            {
                SelectedDateData GetSelectedData = new SelectedDateData();
                GetSelectedData = _context.tbl_daily_attendance.Where(p => p.tbl_shift_details.shift_details_id == p.s_d_id
                  && p.attendance_dt.Date == CalenderDate.Value.Date && p.emp_id == EmployeeId).Select(p => new SelectedDateData
                  {
                      ActualTimeIn = p.in_time.ToString("hh:mm tt"),
                      ActualTimeOut = p.out_time.ToString("hh:mm tt"),
                      ActualToatlShiftHrs = Convert.ToString(p.working_hour_done) + ":" + Convert.ToString(p.working_minute_done),
                      ActualAttendanceStatus = p.day_status == 2 ? "Absent" : p.day_status == 3 ? "On Leave" : p.day_status == 4 ? "Half Day Present" : "Present",
                      ShiftName = p.tbl_shift_details.shift_name,

                      leave_d_per_day_list = _context.tbl_leave_request.Where(x => x.leave_request_id == p.leave_request_id && x.is_final_approve == 1 && x.is_deleted == 0)
                                             .Select(v => new leavedetails_per_day
                                             {
                                                 leave_request_id = Convert.ToInt32(p.leave_request_id),
                                                 leavetype = p.leave_type == 1 ? "Full Day" : p.leave_type == 2 ? "Half Day" : p.leave_type == 3 ? "Short Leave" : "",
                                                 fromdate = Convert.ToDateTime(v.from_date).ToString("dd-MMM-yyyy"),
                                                 todate = Convert.ToDateTime(v.to_date).ToString("dd-MMM-yyyy"),
                                                 //fromdate = Convert.ToDateTime(p.in_time).ToString("dd-MMM-yyyy"),
                                                 //todate = Convert.ToDateTime(p.out_time).ToString("dd-MMM-yyyy"),
                                                 partialdays = v.leave_applicable_for == 1 ? "" : (v.leave_applicable_for == 2 && v.day_part == 1) ? "Half Day - First Half" : (v.leave_applicable_for == 2 && v.day_part == 2) ? "Half Day - Second Half" : "",
                                                 reason = v.requester_remarks,
                                                 attendance_status = v.is_final_approve == 1 ? "Approved" : "Rejected",
                                             }).ToList(),
                  }).FirstOrDefault();

                if (GetSelectedData == null)
                {
                    GetSelectedData = _context.tbl_leave_request.Where(x => x.from_date >= CalenderDate.Value.Date && x.to_date <= CalenderDate.Value.Date && x.r_e_id == EmployeeId && x.is_final_approve == 1 && x.is_deleted == 0)
                                                 .Select(p => new SelectedDateData
                                                 {
                                                     ActualTimeIn = "",
                                                     ActualTimeOut = "",
                                                     ActualToatlShiftHrs = "",
                                                     ActualAttendanceStatus = "",
                                                     ShiftName = "",

                                                     leave_d_per_day_list = _context.tbl_leave_request.Where(x => x.from_date >= CalenderDate.Value.Date && x.to_date <= CalenderDate.Value.Date && x.r_e_id == EmployeeId && x.is_final_approve == 1 && x.is_deleted == 0)
                                                 .Select(v => new leavedetails_per_day
                                                 {
                                                     leave_request_id = Convert.ToInt32(v.leave_request_id),
                                                     leavetype = v.leave_type_id == 1 ? "Full Day" : v.leave_type_id == 2 ? "Half Day" : v.leave_type_id == 3 ? "Short Leave" : "",
                                                     fromdate = Convert.ToDateTime(v.from_date).ToString("dd-MMM-yyyy"),
                                                     todate = Convert.ToDateTime(v.to_date).ToString("dd-MMM-yyyy"),
                                                     //fromdate = Convert.ToDateTime(p.in_time).ToString("dd-MMM-yyyy"),
                                                     //todate = Convert.ToDateTime(p.out_time).ToString("dd-MMM-yyyy"),
                                                     partialdays = v.leave_applicable_for == 1 ? "" : (v.leave_applicable_for == 2 && v.day_part == 1) ? "Half Day - First Half" : (v.leave_applicable_for == 2 && v.day_part == 2) ? "Half Day - Second Half" : "",
                                                     reason = v.requester_remarks,
                                                     attendance_status = v.is_final_approve == 1 ? "Approved" : "Rejected",
                                                 }).ToList()
                                                 }).FirstOrDefault();
                }

                return Ok(GetSelectedData);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        #endregion


        int GetQuarter(DateTime date)
        {
            if (date.Month >= 4 && date.Month <= 6)
                return 2;
            else if (date.Month >= 7 && date.Month <= 9)
                return 3;
            else if (date.Month >= 10 && date.Month <= 12)
                return 4;
            else
                return 1;
        }
        int GetHalfyear(DateTime date)
        {
            if (date.Month >= 1 && date.Month <= 6)
                return 1;
            else
                return 2;
        }

        #region---Call Leave Credit And Debit AND Expire
        [HttpGet]
        [Route("EmployeeLeaveCreditAndDebit")]
        ////[Authorize(Policy = "10011")]
        public IActionResult EmployeeLeaveCreditAndDebit(DateTime? FromDate, DateTime? ToDate)
        {
            bool RunMonthly = false, RunQuaterly = false, RunHalfYearly = false, RunYearly = false;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    DateTime currentTime = DateTime.Now;
                    int MonthId = Convert.ToInt32(currentTime.ToString("yyyyMM"));
                    int QuaterId = Convert.ToInt32(currentTime.ToString("yyyy") + GetQuarter(DateTime.Now));
                    int HalfyerId = Convert.ToInt32(currentTime.ToString("yyyy") + GetHalfyear(DateTime.Now));
                    int Yearid = Convert.ToInt32(currentTime.ToString("yyyy"));
                    //List<tbl_leave_info> Tli = _context.tbl_leave_info.Where(p => p.leave_tenure_from_date <= currentTime && p.leave_tenure_to_date < ToDate.Value && p.is_active == 1).ToList();
                    tbl_scheduler_master monthly_leave = _context.tbl_scheduler_master.Where(p => p.scheduler_name == "monthly_leave" && p.is_deleted == 0).FirstOrDefault();
                    if (monthly_leave == null)
                    {
                        monthly_leave = new tbl_scheduler_master()
                        {
                            is_deleted = 0,
                            last_runing_date = currentTime,
                            schduler_type = enmSchdulerType.Monthly,
                            scheduler_name = "monthly_leave"
                        };
                        _context.tbl_scheduler_master.Add(monthly_leave);
                        _context.SaveChanges();
                    }
                    tbl_scheduler_master quaterly_leave = _context.tbl_scheduler_master.Where(p => p.scheduler_name == "quaterly_leave" && p.is_deleted == 0).FirstOrDefault();
                    if (quaterly_leave == null)
                    {
                        quaterly_leave = new tbl_scheduler_master()
                        {
                            is_deleted = 0,
                            last_runing_date = currentTime,
                            schduler_type = enmSchdulerType.Quaterly,
                            scheduler_name = "quaterly_leave"
                        };
                        _context.tbl_scheduler_master.Add(quaterly_leave);
                        _context.SaveChanges();
                    }
                    tbl_scheduler_master halfyear_leave = _context.tbl_scheduler_master.Where(p => p.scheduler_name == "halfyear_leave" && p.is_deleted == 0).FirstOrDefault();
                    if (halfyear_leave == null)
                    {
                        halfyear_leave = new tbl_scheduler_master()
                        {
                            is_deleted = 0,
                            last_runing_date = currentTime,
                            schduler_type = enmSchdulerType.Halfyearly,
                            scheduler_name = "halfyear_leave"
                        };
                        _context.tbl_scheduler_master.Add(halfyear_leave);
                        _context.SaveChanges();
                    }
                    tbl_scheduler_master year_leave = _context.tbl_scheduler_master.Where(p => p.scheduler_name == "year_leave" && p.is_deleted == 0).FirstOrDefault();
                    if (year_leave == null)
                    {
                        year_leave = new tbl_scheduler_master()
                        {
                            is_deleted = 0,
                            last_runing_date = currentTime,
                            schduler_type = enmSchdulerType.Yearly,
                            scheduler_name = "year_leave"
                        };
                        _context.tbl_scheduler_master.Add(year_leave);
                        _context.SaveChanges();
                    }
                    tbl_scheduler_master expired_leave = _context.tbl_scheduler_master.Where(p => p.scheduler_name == "expired_leave" && p.is_deleted == 0).FirstOrDefault();
                    if (expired_leave == null)
                    {
                        expired_leave = new tbl_scheduler_master()
                        {
                            is_deleted = 0,
                            last_runing_date = currentTime,
                            schduler_type = enmSchdulerType.Yearly,
                            scheduler_name = "expired_leave"
                        };
                        _context.tbl_scheduler_master.Add(expired_leave);
                        _context.SaveChanges();
                    }
                    //add monthly leave
                    var monthly_schdeuler = _context.tbl_scheduler_details.Where(p => p.scheduler_id == monthly_leave.scheduler_id && p.is_deleted == 0 && p.transaction_no == MonthId).FirstOrDefault();
                    if (monthly_schdeuler == null)
                    {

                        _context.tbl_scheduler_details.Add(new tbl_scheduler_details() { transaction_no = MonthId, is_deleted = 0, last_runing_date = currentTime, scheduler_id = monthly_leave.scheduler_id });
                        RunMonthly = true;
                    }
                    var Quaterly_schdeuler = _context.tbl_scheduler_details.Where(p => p.scheduler_id == quaterly_leave.scheduler_id && p.is_deleted == 0 && p.transaction_no == QuaterId).FirstOrDefault();
                    if (Quaterly_schdeuler == null)
                    {
                        _context.tbl_scheduler_details.Add(new tbl_scheduler_details() { transaction_no = QuaterId, is_deleted = 0, last_runing_date = currentTime, scheduler_id = quaterly_leave.scheduler_id });
                        RunQuaterly = true;
                    }
                    var halfyear_schdeuler = _context.tbl_scheduler_details.Where(p => p.scheduler_id == halfyear_leave.scheduler_id && p.is_deleted == 0 && p.transaction_no == HalfyerId).FirstOrDefault();
                    if (halfyear_schdeuler == null)
                    {
                        _context.tbl_scheduler_details.Add(new tbl_scheduler_details() { transaction_no = HalfyerId, is_deleted = 0, last_runing_date = currentTime, scheduler_id = halfyear_leave.scheduler_id });
                        RunHalfYearly = true;
                    }
                    var year_schdeuler = _context.tbl_scheduler_details.Where(p => p.scheduler_id == year_leave.scheduler_id && p.is_deleted == 0 && p.transaction_no == Yearid).FirstOrDefault();
                    if (year_schdeuler == null)
                    {
                        _context.tbl_scheduler_details.Add(new tbl_scheduler_details() { transaction_no = Yearid, is_deleted = 0, last_runing_date = currentTime, scheduler_id = year_leave.scheduler_id });
                        RunYearly = true;
                    }

                    var expiredleave_schdeuler = _context.tbl_scheduler_details.Where(p => p.scheduler_id == year_leave.scheduler_id && p.is_deleted == 0 && p.transaction_no == Yearid).FirstOrDefault();
                    if (expiredleave_schdeuler == null)
                    {
                        _context.tbl_scheduler_details.Add(new tbl_scheduler_details()
                        { transaction_no = Yearid, is_deleted = 0, last_runing_date = currentTime, scheduler_id = expired_leave.scheduler_id });
                        var leavetypes = _context.tbl_leave_type.Where(p => p.is_active == 1).ToList();
                        DateTime PreviousDatetime = currentTime.AddYears(-1).AddDays(1);
                        foreach (var leavetype in leavetypes)
                        {
                            var LeaveInfo = _context.tbl_leave_info.Where(p => p.leave_type_id == leavetype.leave_type_id && p.is_active == 1 && p.leave_tenure_from_date <= PreviousDatetime && p.leave_tenure_to_date >= PreviousDatetime).OrderByDescending(p => p.leave_info_id).FirstOrDefault();
                            if (LeaveInfo != null)
                            {
                                clsLeaveCreditNew cls = new clsLeaveCreditNew(leaveInfoId: LeaveInfo.leave_info_id,
                                    leavetypeId: leavetype.leave_type_id, transactionNo: 0, leave_tenure_from_date: LeaveInfo.leave_tenure_from_date,
                                    leave_tenure_to_date: LeaveInfo.leave_tenure_to_date, RunMonthly: RunMonthly, RunQuaterly: RunQuaterly,
                                    RunHalfYearly: RunHalfYearly, RunYearly: RunYearly, MonthId: MonthId, QuaterId: QuaterId, HalfyerId: HalfyerId, Yearid: Yearid, Currentdate: currentTime, context: _context);
                                cls.expiredLeave();
                            }
                        }
                    }
                    {
                        //run schedule for Current Month
                        var leavetypes = _context.tbl_leave_type.Where(p => p.is_active == 1).ToList();
                        foreach (var leavetype in leavetypes)
                        {
                            var LeaveInfo = _context.tbl_leave_info.Where(p => p.leave_type_id == leavetype.leave_type_id && p.is_active == 1 && p.leave_tenure_from_date <= currentTime && p.leave_tenure_to_date >= currentTime).OrderByDescending(p => p.leave_info_id).FirstOrDefault();
                            if (LeaveInfo == null)
                            {
                                LeaveInfo = _context.tbl_leave_info.Where(p => p.leave_type_id == leavetype.leave_type_id && p.is_active == 1).OrderByDescending(p => p.leave_info_id).FirstOrDefault();
                                if (LeaveInfo != null)
                                {
                                    tbl_leave_info NewLeaveInf = new tbl_leave_info()
                                    {
                                        leave_type = LeaveInfo.leave_type,
                                        leave_tenure_from_date = LeaveInfo.leave_tenure_from_date.AddYears(1),
                                        leave_tenure_to_date = LeaveInfo.leave_tenure_to_date.AddYears(1),
                                        is_active = 1,
                                        leave_type_id = LeaveInfo.leave_type_id
                                    };
                                    _context.tbl_leave_info.Add(NewLeaveInf);
                                    _context.SaveChanges();
                                    var tbl_Leave_Credits = _context.tbl_leave_credit.Where(p => p.leave_info_id == LeaveInfo.leave_info_id && p.is_deleted == 0).FirstOrDefault();
                                    var tbl_leave_rule = _context.tbl_leave_rule.Where(p => p.leave_info_id == LeaveInfo.leave_info_id && p.is_deleted == 0).FirstOrDefault();
                                    var tbl_leave_applicablity = _context.tbl_leave_applicablity.Where(p => p.leave_info_id == LeaveInfo.leave_info_id && p.is_deleted == 0).FirstOrDefault();
                                    tbl_Leave_Credits.leave_info_id = NewLeaveInf.leave_info_id;
                                    tbl_leave_rule.leave_info_id = NewLeaveInf.leave_info_id;
                                    tbl_leave_rule.leave_credit_id = 0;
                                    tbl_Leave_Credits.leave_credit_id = 0;
                                    _context.tbl_leave_credit.Add(tbl_Leave_Credits);
                                    _context.tbl_leave_rule.Add(tbl_leave_rule);
                                    var tbl_leave_app_on_emp_type = _context.tbl_leave_app_on_emp_type.Where(p => p.l_app_id == tbl_leave_applicablity.leave_applicablity_id && p.is_deleted == 0).ToList();
                                    var tbl_leave_appcbl_on_company = _context.tbl_leave_appcbl_on_company.Where(p => p.l_a_id == tbl_leave_applicablity.leave_applicablity_id && p.is_deleted == 0).ToList();
                                    var tbl_leave_app_on_dept = _context.tbl_leave_app_on_dept.Where(p => p.lid == tbl_leave_applicablity.leave_applicablity_id && p.is_deleted == 0).ToList();
                                    var tbl_leave_appcbl_on_religion = _context.tbl_leave_appcbl_on_religion.Where(p => p.l_app_id == tbl_leave_applicablity.leave_applicablity_id && p.is_deleted == 0).ToList();
                                    tbl_leave_applicablity.leave_info_id = NewLeaveInf.leave_info_id;
                                    tbl_leave_applicablity.leave_applicablity_id = 0;
                                    _context.tbl_leave_applicablity.Add(tbl_leave_applicablity);
                                    _context.SaveChanges();
                                    tbl_leave_app_on_emp_type.ForEach(p => { p.l_app_id = tbl_leave_applicablity.leave_applicablity_id; p.sno = 0; });
                                    tbl_leave_appcbl_on_company.ForEach(p => { p.l_a_id = tbl_leave_applicablity.leave_applicablity_id; p.sno = 0; });
                                    tbl_leave_app_on_dept.ForEach(p => { p.lid = tbl_leave_applicablity.leave_applicablity_id; p.sno = 0; });
                                    tbl_leave_appcbl_on_religion.ForEach(p => { p.l_app_id = tbl_leave_applicablity.leave_applicablity_id; p.sno = 0; });
                                    _context.tbl_leave_app_on_emp_type.AddRange(tbl_leave_app_on_emp_type);
                                    _context.tbl_leave_appcbl_on_company.AddRange(tbl_leave_appcbl_on_company);
                                    _context.tbl_leave_app_on_dept.AddRange(tbl_leave_app_on_dept);
                                    _context.tbl_leave_appcbl_on_religion.AddRange(tbl_leave_appcbl_on_religion);
                                    _context.SaveChanges();
                                    LeaveInfo = NewLeaveInf;
                                }
                            }
                            if (LeaveInfo != null)
                            {
                                clsLeaveCreditNew cls = new clsLeaveCreditNew(leaveInfoId: LeaveInfo.leave_info_id,
                                    leavetypeId: leavetype.leave_type_id, transactionNo: 0, leave_tenure_from_date: LeaveInfo.leave_tenure_from_date,
                                    leave_tenure_to_date: LeaveInfo.leave_tenure_to_date, RunMonthly: RunMonthly, RunQuaterly: RunQuaterly,
                                    RunHalfYearly: RunHalfYearly, RunYearly: RunYearly, MonthId: MonthId, QuaterId: QuaterId, HalfyerId: HalfyerId, Yearid: Yearid, Currentdate: currentTime, context: _context);
                                cls.saveLeaveCredit();
                            }
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return Ok(ex.Message); ;
                }

            }

            return Ok("Process Successfully !!");
            //try
            //{

            //    string msg = string.Empty;
            //    //get all leave info
            //    if (FromDate == null)
            //    {
            //        FromDate = new DateTime(DateTime.Now.AddMonths(-1).Month == 12 ? DateTime.Now.AddYears(-1).Year : DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 26);
            //        ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 25);
            //    }

            //    List<tbl_leave_info> Tli = _context.tbl_leave_info.Where(p => p.leave_tenure_from_date.Year <= ToDate.Value.Year && p.leave_tenure_to_date.Year >= ToDate.Value.Year && p.is_active == 1).ToList();

            //    int PayrollMonthYear = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
            //    for (int Indexi = 0; Indexi < Tli.Count; Indexi++)
            //    {
            //        //get all leave credit master data for process the request
            //        List<tbl_leave_credit> Tlc = _context.tbl_leave_credit.Where(p => p.leave_info_id == Tli[Indexi].leave_info_id && p.is_deleted == 0).ToList();
            //        for (int Index = 0; Index < Tlc.Count; Index++)
            //        {
            //            DateTime LeaveCreditDt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, Convert.ToInt32(Tlc[Index].leave_credit_day));

            //            //                        DateTime LeaveCreditDt = new DateTime(DateTime.Now.AddYears(1).Year, DateTime.Now.AddMonths(-9).Month, Convert.ToInt32(Tlc[Index].leave_credit_day)); ;

            //            Classes.clsLeaveCredit LeaveCredit = new Classes.clsLeaveCredit(_context, Tlc[Index].leave_info_id ?? 0, PayrollMonthYear, LeaveCreditDt,
            //                Tli[Index].leave_tenure_from_date, Tli[Index].leave_tenure_to_date, Tlc[Index].frequency_type, Tlc[Index].is_leave_accrue, Convert.ToByte(Tlc[Index].max_accrue), Convert.ToByte(Tlc[Index].leave_credit_day),
            //                Tlc[Index].leave_credit_number, Tli[Indexi].leave_type_id ?? 0);

            //            //supriya check previous data means already exist in table or not
            //            //var _exist_data = _context.tbl_leave_ledger.Where(x => x.leave_info_id == LeaveCredit._leaveInfoId && x.leave_type_id == LeaveCredit._leave_type_id && x.monthyear == LeaveCredit._payrollMonthyear).ToList();
            //            //if (_exist_data.Count > 0)
            //            //{
            //            //    msg = "Already Process";

            //            //}
            //            //else
            //            //{

            //            //    LeaveCredit.CalculateEmpLeave();
            //            //    msg = "Process Successfully !!";

            //            // }
            //            //supriya
            //            LeaveCredit.CalculateEmpLeave();
            //        }
            //    }

            //    //if (!string.IsNullOrEmpty(msg))
            //    //{
            //    //    return Ok(msg);
            //    //}
            //    //else
            //    //{
            //    //    return Ok("Something went wrong...");
            //    //}


            //    return Ok("Process Successfully !!");
            //}
            //catch (Exception ex)
            //{
            //    return Ok(ex.Message);
            //}
        }

        #endregion

        //attendance data for pichart
        [HttpGet]
        [Route("GetAttendanceStatusCount")]
        ////[Authorize(Policy = "10012")]
        public IActionResult GetAttendanceStatusCount()
        {
            try
            {
                //DateTime FromDate, ToDate;
                //FromDate = new DateTime(DateTime.Now.AddMonths(-1).Month == 12 ? DateTime.Now.AddYears(-1).Year : DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 26);
                //ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 25);

                //1-Full day Present,2-Full day Absent,3 Full Day Leave, 4 half day present-half day Absent
                int[] DayStatus = { 1, 2, 3 };
                List<DailyAttenStatusCount> DailyCount = new List<DailyAttenStatusCount>();
                DailyCount = DayStatus.Select(p => new DailyAttenStatusCount
                {
                    DayStatus = p,
                    TotalCount = 0,
                    StatusName = p == 1 ? "Present" : p == 2 ? "Absent" : p == 3 ? "On Leave" : ""
                }).ToList();


                var ResultCount = _context.tbl_daily_attendance.Where(a => DayStatus.Contains(a.day_status) && a.attendance_dt.Date == DateTime.Now.Date)
                    .GroupBy(p => new { p.day_status }).Select(q => new
                    {
                        daystatus = q.Key,
                        TotalCount = q.Count()
                    }).ToList();

                DailyCount.ForEach(p =>
                {
                    byte DayStatu_ = Convert.ToByte(p.DayStatus);
                    var d = ResultCount.FirstOrDefault(q => q.daystatus.day_status == DayStatu_);
                    p.TotalCount = d != null ? d.TotalCount : p.TotalCount;
                });

                return Ok(DailyCount);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        #region **START BY SUPRIYA ON 28-09-2019 ,SAVE ATTANDANCE FROM EXCEL
        [Route("SaveAttandance_ExcelUpload")]
        [HttpPost]
        ////[Authorize(Policy = "10013")]
        public async Task<IActionResult> SaveAttandance_ExcelUpload()
        {
            try
            {
                ResponseMsg objresponse = new ResponseMsg();
                StringBuilder excelResult = new StringBuilder();
                int TotalRows_to_be_proce = 1000;
                int totalProcesed = 0;
                string _file_pathh = string.Empty;
                List<mdlSaveAttendance> objsaveattandance = new List<mdlSaveAttendance>();
                var files = HttpContext.Request.Form.Files;
                var a = HttpContext.Request.Form["AllData"];
                if (a.ToString() == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid data !!";
                    return Ok(objresponse);
                }

                CommonClass com = new CommonClass();
                mdlSaveAttendance objdetaills = new mdlSaveAttendance();
                objdetaills = com.ToObjectFromJSON<mdlSaveAttendance>(a.ToString());

                foreach (var FileData in files)
                {
                    if (FileData != null && FileData.Length > 0)
                    {
                        var allowedExtensions = new[] { ".xlsx" };

                        var ext = Path.GetExtension(FileData.FileName); //getting the extension
                        if (allowedExtensions.Contains(ext.ToLower()))//check what type of extension  
                        {
                            string name = Path.GetFileNameWithoutExtension(FileData.FileName); //getting file name without extension  

                            string company_name = _context.tbl_company_master.OrderByDescending(x => x.company_id).Where(y => y.company_id == objdetaills.company_idd && y.is_active == 1).Select(p => p.company_name).FirstOrDefault();
                            string MyFileName = "EmpAttandanceDetail_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_tt") + ext; //Guid.NewGuid().ToString().Replace("-", "") +


                            var webRoot = _hostingEnvironment.WebRootPath;

                            string currentmonth = Convert.ToString(DateTime.Now.Month).Length.ToString() == "1" ? "0" + Convert.ToString(DateTime.Now.Month) : Convert.ToString(DateTime.Now.Month);

                            var currentyearmonth = Convert.ToString(DateTime.Now.Year) + currentmonth;


                            if (!Directory.Exists(webRoot + "/" + company_name + "/EmpAttandanceDtl/" + currentyearmonth + "/"))
                            {
                                Directory.CreateDirectory(webRoot + "/" + company_name + "/EmpAttandanceDtl/" + currentyearmonth + "/");

                            }



                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/" + company_name + "/EmpAttandanceDtl/" + currentyearmonth + "/");

                            //save file
                            using (var fileStream = new FileStream(Path.Combine(path, MyFileName), FileMode.Create))
                            {
                                FileData.CopyTo(fileStream);

                                _file_pathh = fileStream.Name;
                                //using (SpreadsheetDocument doc = SpreadsheetDocument.Open("F:\\Documentss\\Employee Officaial Section.xlsx", false))

                            }


                        }
                        else
                        {
                            objresponse.StatusCode = 1;
                            objresponse.Message = "Please Select Only Excel File";
                            return Ok(objresponse);
                        }
                    }
                    else
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Please Select File For Upload";
                        return Ok(objresponse);
                    }





                }





                if (!string.IsNullOrEmpty(_file_pathh))
                {

                    //START read data from excel and saved in DB

                    using (SpreadsheetDocument doc = SpreadsheetDocument.Open(_file_pathh, false))
                    {

                        //create the object for workbook part  
                        WorkbookPart workbookPart = doc.WorkbookPart;
                        Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();
                        //StringBuilder excelResult = new StringBuilder();

                        //array list to store employee code

                        //var pathh = Path.GetTempPath();

                        //using for each loop to get the sheet from the sheetcollection  
                        foreach (Sheet thesheet in thesheetcollection)
                        {
                            excelResult.AppendLine("Excel Sheet Name : " + thesheet.Name);
                            excelResult.AppendLine("----------------------------------------------- ");
                            //statement to get the worksheet object by using the sheet id  
                            Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;


                            SheetData thesheetdata = (SheetData)theWorksheet.GetFirstChild<SheetData>();


                            foreach (Row thecurrentrow in thesheetdata)
                            {
                                //skip header row
                                if (thecurrentrow.RowIndex != 1)
                                {
                                    EmployeePersonalSection list = new EmployeePersonalSection();

                                    mdlSaveAttendance objattandance = new mdlSaveAttendance();
                                    string currentcolumnnoo = string.Empty;
                                    foreach (Cell thecurrentcell in thecurrentrow)
                                    {
                                        currentcolumnnoo = thecurrentcell.CellReference.ToString().Substring(0, 1).ToUpper();
                                        //skip sr no.
                                        if (currentcolumnnoo != "A")
                                        {

                                            //statement to take the integer value  
                                            string currentcellvalue = string.Empty;

                                            if (thecurrentcell.DataType != null)
                                            {
                                                if (thecurrentcell.DataType == CellValues.SharedString)
                                                {
                                                    int id;
                                                    if (Int32.TryParse(thecurrentcell.InnerText, out id))
                                                    {
                                                        SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                                        if (item.Text != null)
                                                        {
                                                            //code to take the string value  

                                                            excelResult.Append(item.Text.Text + " ");
                                                            if (!string.IsNullOrEmpty(item.Text.Text) && currentcolumnnoo == "B")
                                                            {
                                                                objattandance.emp_code = item.Text.Text;
                                                            }
                                                            else if (!string.IsNullOrEmpty(item.Text.Text) && currentcolumnnoo == "C")
                                                            {
                                                                objattandance.card_number = item.Text.Text;
                                                            }
                                                            else if (!string.IsNullOrEmpty(item.Text.Text) && currentcolumnnoo == "D")
                                                            {
                                                                objattandance.punch_time = Convert.ToDateTime(DateTime.FromOADate(Convert.ToDouble(item.Text.Text)).ToString("dd-MMM-yyyy hh:mm:ss tt"));
                                                            }
                                                            else if (currentcolumnnoo == "E")
                                                            {

                                                                if (!string.IsNullOrEmpty(item.Text.Text))
                                                                {
                                                                    int Num;
                                                                    bool isNum = int.TryParse(item.Text.Text.ToString(), out Num); //c is your variable
                                                                    if (isNum)
                                                                        objattandance.machine_id = Convert.ToInt32(item.Text.Text);  //integer

                                                                }
                                                                else
                                                                {
                                                                    objattandance.machine_id = 1;
                                                                }
                                                            }
                                                        }
                                                        else if (item.InnerText != null)
                                                        {
                                                            currentcellvalue = item.InnerText;
                                                        }
                                                        else if (item.InnerXml != null)
                                                        {
                                                            currentcellvalue = item.InnerXml;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //read columns value
                                                excelResult.Append(Convert.ToString(thecurrentcell.InnerText) + " ");

                                                if (!string.IsNullOrEmpty(thecurrentcell.InnerText) && currentcolumnnoo == "B")
                                                {
                                                    objattandance.emp_code = thecurrentcell.InnerText;
                                                }
                                                else if (!string.IsNullOrEmpty(thecurrentcell.InnerText) && currentcolumnnoo == "C")
                                                {
                                                    objattandance.card_number = thecurrentcell.InnerText;
                                                }
                                                else if (!string.IsNullOrEmpty(thecurrentcell.InnerText) && currentcolumnnoo == "D")
                                                {
                                                    objattandance.punch_time = Convert.ToDateTime(DateTime.FromOADate(Convert.ToDouble(thecurrentcell.InnerText)).ToString("dd-MMM-yyyy hh:mm:ss tt"));
                                                }
                                                else if (currentcolumnnoo == "E")
                                                {
                                                    //bool machinetype = false;

                                                    //var testtt = thecurrentcell.InnerText.GetType().IsValueType;
                                                    //if (!string.IsNullOrEmpty(thecurrentcell.InnerText))
                                                    //{
                                                    //    objattandance.machine_id = Convert.ToInt32(thecurrentcell.InnerText);
                                                    //}
                                                    //else
                                                    //{
                                                    //    objattandance.machine_id = 1;
                                                    //}

                                                    if (!string.IsNullOrEmpty(thecurrentcell.InnerText))
                                                    {
                                                        int Num;
                                                        bool isNum = int.TryParse(thecurrentcell.InnerText.ToString(), out Num); //c is your variable
                                                        if (isNum)
                                                            objattandance.machine_id = Convert.ToInt32(thecurrentcell.InnerText);  //integer

                                                    }
                                                    else
                                                    {
                                                        objattandance.machine_id = 1;
                                                    }
                                                }
                                            }


                                        }



                                    }
                                    excelResult.AppendLine();
                                    objsaveattandance.Add(objattandance);
                                }

                            }

                            excelResult.Append("");


                        }
                    }


                    var check_result = CheckEmpAttandanceDetailsfromexcel(objsaveattandance);
                    var duplicatedtl = check_result.duplicatedtl;
                    var missingdtl = check_result.missingdtl;
                    var dblist = check_result.adddblist;

                    if (duplicatedtl.Count > 0 || missingdtl.Count > 0)
                    {
                        return Ok(check_result);
                    }
                    else
                    {
                        save_attendance(objsaveattandance.ToArray(), 1);
                        objresponse.StatusCode = 0;
                        objresponse.Message = "Successfully Upload";

                        return Ok(objresponse);
                    }

                    //START read data from excel and saved in DB
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Something Went Wrong";
                    return Ok(objresponse);
                }

                // return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        public mdlSaveAttendanceList CheckEmpAttandanceDetailsfromexcel(List<mdlSaveAttendance> list)
        {
            List<mdlSaveAttendance> missingdetaillist = new List<mdlSaveAttendance>();
            List<mdlSaveAttendance> adddblist = new List<mdlSaveAttendance>();
            List<mdlSaveAttendance> duplicatedetaillist = new List<mdlSaveAttendance>();
            // bool exist_emp_code = false;
            //bool exist_official_email = false;
            //bool _exist_card_no = false;
            //bool _religion = false;
            //bool _blank_marital_status = false;
            //bool _employement_type = false;
            //bool _location = false;
            //bool _department = false;
            //bool _subdepartment = false;
            //bool _gender = false;
            bool _emp_code = false;
            bool _card_noo = false;
            bool _punch_time = false;
            bool _machine_id = false;

            //int marital_status_id = 0;
            //int _religion_id = 0;
            //int _location_id = 0;
            //int employement_type_id = 0;
            //int _department_id = 0;
            //int _subdepartment_id = 0;
            //int gender_id = 0;

            StringBuilder MissingDtlMessage = new StringBuilder();

            try
            {

                MissingDtlMessage.Append("");
                int i = 1;
                foreach (var item in list)
                {

                    MissingDtlMessage.Append(i + " Detail:-");

                    if (string.IsNullOrEmpty(item.emp_code))
                    {
                        _emp_code = true;
                        MissingDtlMessage.Append(" Emp Code is Missing,");
                    }

                    if (string.IsNullOrEmpty(item.card_number))
                    {
                        _card_noo = true;
                        MissingDtlMessage.Append(" Card Number is Missing,");
                    }
                    else
                    {
                        var exist_card_no = _context.tbl_emp_officaial_sec.OrderByDescending(x => x.emp_official_section_id).Where(x => x.card_number == item.card_number).Select(p => p.card_number).FirstOrDefault();
                        if (exist_card_no == null)
                        {
                            _card_noo = true;
                            MissingDtlMessage.Append("Card No not exist");
                        }
                    }

                    if (string.IsNullOrEmpty(item.punch_time.ToString("dd-MMM-yyyy hh:mm:ss tt")))
                    {
                        _punch_time = true;
                        MissingDtlMessage.Append("Punch Time is missing");
                    }

                    if (string.IsNullOrEmpty(item.machine_id.ToString()))
                    {
                        _machine_id = true;
                        MissingDtlMessage.Append("Machine ID is missing");
                    }
                    else
                    {

                        if (item.machine_id == 0)
                        {
                            _machine_id = true;
                            MissingDtlMessage.Append("Machine ID must be in numeric");
                        }
                    }



                    if (_emp_code || _card_noo || _punch_time || _machine_id)
                    {
                        mdlSaveAttendance objlist = new mdlSaveAttendance();
                        //objlist.emp_code = item.emp_code;
                        objlist.emp_code = item.emp_code;
                        objlist.card_number = item.card_number;
                        objlist.punch_time = item.punch_time;
                        objlist.machine_id = item.machine_id;

                        missingdetaillist.Add(objlist);

                        i++; //increase serial no 

                        MissingDtlMessage.Append("</br>Missing/Invalid Detail:-</br>");
                    }
                    else
                    {
                        //if (list.Where(b => b.emp_code == item.emp_code || b.card_number == item.card_number).Count > 0)

                        bool _checkduplicate = adddblist.Any(x => x.emp_code == item.emp_code || x.card_number == item.card_number);

                        if (_checkduplicate) //If any field detail is found duplicate in list
                        {
                            mdlSaveAttendance objduplist = new mdlSaveAttendance();


                            // objduplist.emp_code = item.emp_code;
                            objduplist.card_number = item.card_number;
                            objduplist.emp_code = item.emp_code;

                            duplicatedetaillist.Add(objduplist);

                            MissingDtlMessage.Append("Duplicate Entry in Excel</br>");

                            i++; //increase serial no 
                        }
                        else
                        {
                            mdlSaveAttendance objdblist = new mdlSaveAttendance();

                            // objdblist.emp_code = item.emp_code;
                            objdblist.emp_code = item.emp_code;
                            objdblist.card_number = item.card_number;
                            objdblist.punch_time = item.punch_time;
                            objdblist.machine_id = item.machine_id;

                            adddblist.Add(objdblist);

                            i++;
                        }

                    }

                }
            }
            catch (Exception ex)
            {

            }

            return new mdlSaveAttendanceList { missingdtl = missingdetaillist, duplicatedtl = duplicatedetaillist, adddblist = adddblist, DtlMessage = MissingDtlMessage.ToString() };
        }


        #endregion ** END BY SUPRIYA ON 28-09-2019,SAVE ATTANDACNE FROM EXCEL
    }
}

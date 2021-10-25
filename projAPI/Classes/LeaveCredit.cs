using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using projAPI.Model;
using projContext;
using projContext.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Classes
{
#if(false)
    public class clsLeaveCreditNew
    {
        private readonly DateTime Currentdate;
        bool _RunMonthly = false, _RunQuaterly = false, _RunHalfYearly = false, _RunYearly = false;
        int _MonthId = 0, _QuaterId = 0, _HalfyerId = 0, _Yearid = 0, _Monthyear = 0, _transactionNo = 0;
        public readonly int _leaveInfoId, _leavetypeId;
        private readonly Context _context;

        tbl_leave_rule _tbl_leave_rule;
        tbl_leave_credit _tbl_leave_credit;

        public List<int> _ApplicableEmpID = new List<int>();
        public List<int> _ApplicableEmpIDPrevious = new List<int>();
        public List<int> _ApplicableReligionID = new List<int>();
        public List<int> _ApplicableEmploymentTypeID = new List<int>();
        public List<int> _ApplicableLocationID = new List<int>();
        public List<int> _ApplicableDepartmentID = new List<int>();
        public List<int> _ApplicablePrevoiusEmpLLedger = new List<int>();
        DateTime _leave_tenure_from_date, _leave_tenure_to_date;
        public List<EmpLeaveBalence> _EmpLeaveBalenceList;

        public clsLeaveCreditNew(int leaveInfoId, int leavetypeId, int transactionNo, DateTime leave_tenure_from_date
            , DateTime leave_tenure_to_date,
            bool RunMonthly, bool RunQuaterly, bool RunHalfYearly, bool RunYearly,
            int MonthId, int QuaterId, int HalfyerId, int Yearid, DateTime Currentdate,
        Context context)
        {
            _Monthyear = Convert.ToInt32(Currentdate.ToString("yyyyMM"));
            _RunMonthly = RunMonthly; _RunQuaterly = RunQuaterly; _RunHalfYearly = RunHalfYearly; _RunYearly = RunYearly;
            _MonthId = MonthId; _QuaterId = QuaterId; _HalfyerId = HalfyerId; _Yearid = Yearid; ;
            _transactionNo = transactionNo;
            _leaveInfoId = leaveInfoId;
            _leavetypeId = leavetypeId;
            _leave_tenure_from_date = leave_tenure_from_date;
            _leave_tenure_to_date = leave_tenure_to_date;
            this.Currentdate = Currentdate;
            _context = context;
            _tbl_leave_rule = _context.tbl_leave_rule.Where(p => p.leave_info_id == leaveInfoId && p.is_deleted == 0).FirstOrDefault();
            _tbl_leave_credit = _context.tbl_leave_credit.Where(p => p.leave_info_id == leaveInfoId && p.is_deleted == 0).FirstOrDefault();

        }
        private void GetEmployeeList()
        {
            tbl_leave_applicablity Tla = _context.tbl_leave_applicablity.FirstOrDefault(p => p.leave_info_id == _leaveInfoId && p.is_deleted == 0);
            if (Tla != null)
            {
                //get the Religion
                if (Tla.is_aplicable_on_all_religion == 1)
                {
                    _ApplicableReligionID = _context.tbl_religion_master.Select(p => p.religion_id).ToList();
                }
                else
                {
                    _ApplicableReligionID = _context.tbl_leave_appcbl_on_religion.Where(p => p.l_app_id == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p => p.religion_id ?? 0).ToList();
                }
                //get the all department                
                if (Tla.is_aplicable_on_all_department == 1)
                {
                    _ApplicableDepartmentID = _context.tbl_department_master.Select(p => p.department_id).ToList();
                }
                else
                {
                    _ApplicableDepartmentID = _context.tbl_leave_app_on_dept.Where(p => p.lid == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p => p.id ?? 0).ToList();
                }
                //get the all employment type
                if (Tla.is_aplicable_on_all_emp_type == 1)
                {
                    _ApplicableEmploymentTypeID = new List<int>();
                    foreach (int en in Enum.GetValues(typeof(enm_employment_type)))
                    { _ApplicableEmploymentTypeID.Add(en); }

                }
                else
                {
                    _ApplicableEmploymentTypeID = _context.tbl_leave_app_on_emp_type.Where(p => p.l_app_id == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p => new { employment_type = Convert.ToInt32(p.employment_type) }).Select(p => p.employment_type).ToList(); // add by ravi
                    //  _ApplicableEmploymentTypeID = _context.tbl_leave_app_on_emp_type.Where(p => p.l_app_id == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p =>new { employment_type = (int)p.employment_type } ).Select(p=>p.employment_type).ToList();
                }

                //get the Location ID
                if (Tla.is_aplicable_on_all_company == 1)
                {
                    _ApplicableLocationID = _context.tbl_location_master.Select(p => p.location_id).ToList();
                }
                else if (Tla.is_aplicable_on_all_company == 0 && Tla.is_aplicable_on_all_location == 1)
                {
                    var cid = _context.tbl_leave_appcbl_on_company.Where(p => p.l_a_id == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p => p.c_id).ToArray();
                    _ApplicableLocationID = _context.tbl_location_master.Where(p => cid.Contains(p.company_id)).Select(p => p.location_id).ToList();

                }
                else if (Tla.is_aplicable_on_all_company == 0 && Tla.is_aplicable_on_all_location == 0)
                {
                    _ApplicableLocationID = _context.tbl_leave_appcbl_on_company.Where(p => p.l_a_id == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p => p.location_id ?? 0).ToList();
                }

                _ApplicableLocationID.RemoveAll(p => p == 0);
                _ApplicableEmploymentTypeID.RemoveAll(p => p == 0);
                _ApplicableDepartmentID.RemoveAll(p => p == 0);

                //Now get the Employee List
                _ApplicableLocationID.Add(0);
                _ApplicableDepartmentID.Add(0);
                _ApplicableReligionID.Add(0);

                //_ApplicableEmpID = _context.tbl_emp_officaial_sec.Where(p => p.is_deleted == 0 && p.current_employee_type != 100 && _ApplicableLocationID.Contains((p.location_id == null ? 0 : p.location_id) ?? 0) &&
                //_ApplicableDepartmentID.Contains((p.department_id == null ? 0 : p.department_id) ?? 0) && _ApplicableEmploymentTypeID.Contains((p.current_employee_type))
                //&& _ApplicableReligionID.Contains((p.religion_id == null ? 0 : p.religion_id) ?? 0)
                //).Select(p => p.employee_id ?? 0).Distinct().ToList();

            }
        }
        //delete existing leave with same payroll month and leave info id and transaction type 1 (added by system)
        private void deleteLeaveCredit()
        {
            if (!(_tbl_leave_credit.is_leave_accrue == 1))
            {


                var ExpireExitingLeaves = _context.tbl_leave_ledger.Where(p => p.leave_type_id == _leavetypeId).GroupBy(p => p.e_id)
                     .Select(g => new { empId = g.Key, debit = g.Sum(p => p.dredit), credit = g.Sum(p => p.credit) })
                     .Where(p => (p.credit - p.debit) > 0).ToList();
                List<tbl_leave_ledger> ExpireExitingLeave = new List<tbl_leave_ledger>();
                for (int i = 0; i < ExpireExitingLeaves.Count(); i++)
                {
                    ExpireExitingLeave.Add(new tbl_leave_ledger
                    {
                        leave_type_id = _leavetypeId,
                        leave_info_id = _leaveInfoId,
                        credit = 0,
                        dredit = ExpireExitingLeaves[i].credit - ExpireExitingLeaves[i].debit,
                        transaction_date = Currentdate,
                        entry_date = Currentdate,
                        monthyear = _Monthyear,
                        transaction_no = _transactionNo,
                        transaction_type = Convert.ToByte(3),
                        leave_addition_type = Convert.ToByte(_tbl_leave_credit.frequency_type),
                        remarks = string.Empty,
                        e_id = ExpireExitingLeaves[i].empId,
                        created_by = 378
                    });
                    _context.tbl_leave_ledger.AddRange(ExpireExitingLeave);
                }

            }
            _context.SaveChanges();

        }
        public void saveLeaveCredit()
        {
            if (_tbl_leave_credit.frequency_type == 5)
            {
                return;
            }

            if (_tbl_leave_credit.frequency_type == 1)
            {
                if (_RunMonthly)
                {
                    _transactionNo = _MonthId;
                }
                else
                {
                    return;
                }
            }
            else if (_tbl_leave_credit.frequency_type == 2)
            {
                if (_RunQuaterly)
                {
                    _transactionNo = _QuaterId;
                }
                else
                {
                    return;
                }
            }
            else if (_tbl_leave_credit.frequency_type == 3)
            {
                if (_RunHalfYearly)
                {
                    _transactionNo = _HalfyerId;
                }
                else
                {
                    return;
                }
            }
            else if (_tbl_leave_credit.frequency_type == 4)
            {
                if (_RunYearly)
                {
                    _transactionNo = _Yearid;
                }
                else
                {
                    return;
                }
            }

            deleteLeaveCredit();
            GetEmployeeList();
            var ExitingLeaveGiven = _context.tbl_leave_ledger.Where(p => p.leave_type_id == _leavetypeId && p.transaction_date >= _leave_tenure_from_date && p.transaction_date <= _leave_tenure_to_date && p.credit > 0)
                .GroupBy(p => p.e_id).Select(p => new { empid = p.Key, totalcredit = p.Sum(q => q.credit) });

            List<tbl_leave_ledger> ledger = _ApplicableEmpID.Select(p => new tbl_leave_ledger
            {
                leave_type_id = _leavetypeId,
                leave_info_id = _leaveInfoId,
                credit = _tbl_leave_credit.leave_credit_number,
                dredit = 0,
                transaction_date = Currentdate,
                entry_date = Currentdate,
                monthyear = _Monthyear,
                transaction_no = _transactionNo,
                transaction_type = 1,
                leave_addition_type = _tbl_leave_credit.frequency_type,
                remarks = string.Empty,
                e_id = p,
                created_by = 378
            }).ToList();
            if (_tbl_leave_credit.is_leave_accrue == 1)
            {
                for (int i = 0; i < ledger.Count; i++)
                {
                    var data = ExitingLeaveGiven.Where(p => p.empid == ledger[i].e_id).FirstOrDefault();
                    if (data != null)
                    {
                        if (data.totalcredit + ledger[i].credit > _tbl_leave_credit.max_accrue)
                        {
                            ledger[i].credit = _tbl_leave_credit.max_accrue - data.totalcredit;
                        }
                    }
                    if (ledger[i].credit < 0)
                    {
                        ledger[i].credit = 0;
                    }
                }
            }
            _context.tbl_leave_ledger.AddRange(ledger.Where(P => P.credit > 0));
            _context.SaveChanges();
        }
        public void expiredLeave()
        {
          var prvday=  Convert.ToDateTime(DateTime.Now.ToString("01-MMM-yyyy 23:59:59")).AddDays(-1);
            var ExpireExitingLeaves = _context.tbl_leave_ledger.Where(p => p.leave_type_id == _leavetypeId).GroupBy(p => p.e_id)
                    .Select(g => new { empId = g.Key, debit = g.Sum(p => p.dredit), credit = g.Sum(p => p.credit) }).Where(p => (p.credit - p.debit) > 0).ToList();

            List<tbl_leave_ledger> ExpireExitingLeave = new List<tbl_leave_ledger>();
            for (int i = 0; i < ExpireExitingLeaves.Count(); i++)
            {
                ExpireExitingLeave.Add(new tbl_leave_ledger
                {
                    leave_type_id = _leavetypeId,
                    leave_info_id = _leaveInfoId,
                    credit = 0,
                    dredit = (!(_tbl_leave_rule.can_carried_forward == 1)) ? ExpireExitingLeaves[i].credit - ExpireExitingLeaves[i].debit :
                    (ExpireExitingLeaves[i].credit - ExpireExitingLeaves[i].debit) > _tbl_leave_rule.maximum_carried_forward ? (ExpireExitingLeaves[i].credit - ExpireExitingLeaves[i].debit) - _tbl_leave_rule.maximum_carried_forward : 0,
                    monthyear = _Monthyear,
                    //transaction_date = Currentdate,
                    //entry_date = Convert.ToDateTime( Currentdate.ToString("01-MMM-yyyy")).AddMinutes(-1),
                    //transaction_no = Convert.ToInt32(Currentdate.ToString("yyyy")),

                    transaction_date = prvday, 
                    entry_date = prvday,
                    transaction_no = Convert.ToInt32(prvday.ToString("yyyy")),

                    transaction_type = 3,
                    leave_addition_type = _tbl_leave_credit.frequency_type,
                    remarks = string.Empty,
                    e_id = ExpireExitingLeaves[i].empId,
                    created_by = 378
                });
            }
            _context.tbl_leave_ledger.AddRange(ExpireExitingLeave.Where(p => p.dredit > 0));
            _context.SaveChanges();

        }

    }
    public class clsLeaveCredit
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _AC;

        //Also Check Wheather it is ferzed or not
        //already Check whater on First page wheather leave is in Between or not of leave tenure        
        public byte _IsLeaveTypeSeq = 0; //1 for First
        public readonly DateTime _leave_credit_date, _applicable_if_employee_joined_before_dt, _leave_tenure_from_date, _leave_tenure_to_date, _dateTime;
        public readonly int _leaveInfoId, _payrollMonthyear, _leave_type_id;
        public readonly byte _frequency_type, _is_leave_accrue, _max_accrue, _leave_credit_day;
        public readonly double _leave_credit_number;
        private readonly Context _context;
        public List<int> _ApplicableEmpID = new List<int>();
        public List<int> _ApplicableEmpIDPrevious = new List<int>();
        public List<int> _ApplicableReligionID = new List<int>();
        public List<int> _ApplicableEmploymentTypeID = new List<int>();
        public List<int> _ApplicableLocationID = new List<int>();
        public List<int> _ApplicableDepartmentID = new List<int>();
        public List<int> _ApplicablePrevoiusEmpLLedger = new List<int>();
        public List<EmpLeaveBalence> _EmpLeaveBalenceList;
        public List<EmpLeaveBalence> _EmpLeaveBalenceListPrev;
        public List<tbl_leave_ledger> _tlledger;

        clsCurrentUser _clsCurrentUser;

        // variable Used to calcualte last leave type
        private int _PreviousleaveInfoId, _maximum_leave_clubbed_in_tenure_number_of_leave, _maximum_carried_forward, _can_carried_forward;
        private DateTime _Previousleave_tenure_from_date, _Previousleave_tenure_to_date;

        public int _companyidd;

        public List<int> _mdclist;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="leaveInfoId"></param>
        /// <param name="payrollMonthyear"></param>
        /// <param name="leave_credit_date"></param>
        /// <param name="leave_tenure_from_date"></param>
        /// <param name="leave_tenure_to_date"></param>
        /// <param name="frequency_type"></param>
        /// <param name="is_leave_accrue"></param>
        /// <param name="max_accrue"></param>
        /// <param name="maximum_leave_clubbed_in_tenure_number_of_leave"></param>
        /// <param name="maximum_carried_forward"></param>
        /// <param name="leave_credit_day"></param>
        /// <param name="leave_credit_number"></param>
        /// <param name="leave_type_id"></param>
        //initialize costructor

        public clsLeaveCredit(Context context, IHttpContextAccessor AC, IConfiguration config, clsCurrentUser _clsCurrentUser)
        {
            _context = context;
            _AC = AC;
            _config = config;
            this._clsCurrentUser = _clsCurrentUser;
        }
        public clsLeaveCredit(Context context, int companyid)
        {
            _context = context;
            _companyidd = companyid;
        }
        public clsLeaveCredit(Context context, int leaveInfoId, int payrollMonthyear, DateTime leave_credit_date, DateTime leave_tenure_from_date
            , DateTime leave_tenure_to_date, byte frequency_type, byte is_leave_accrue, byte max_accrue,
            byte leave_credit_day, double leave_credit_number, int leave_type_id)
        {
            _dateTime = DateTime.Now;
            _context = context;
            _leaveInfoId = leaveInfoId;
            _payrollMonthyear = payrollMonthyear;
            _leave_credit_date = leave_credit_date;

            _leave_tenure_from_date = leave_tenure_from_date;
            _leave_tenure_to_date = leave_tenure_to_date;
            _frequency_type = frequency_type;
            _is_leave_accrue = is_leave_accrue;
            _max_accrue = max_accrue;

            _leave_credit_day = leave_credit_day;
            _leave_credit_number = leave_credit_number;
            _leave_type_id = leave_type_id;
        }


        //execute function to calculate leave
        public void CalculateEmpLeave()
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    _tlledger = new List<tbl_leave_ledger>();
                    CalculateLeaveSeq();
                    deleteLeaveCredit();
                    GetEmployeeList();
                    GetPrevoiusEmpLeaveCredit();
                    GetPrevoiusEmpLeaveDebit();
                    SetTenureEmpLeaveExpired();
                    SetTenureEmpLeaveCredit();
                    if (_IsLeaveTypeSeq == 1)
                    {
                        //Process on last tenure
                        SetPreviousVaribale();
                        if (_PreviousleaveInfoId > 0)//then Expire Previous one
                        {
                            GetPrevoiusTenureEmpLeaveCredit();
                            GetPrevoiusTenureEmpLeaveDebit();
                            SetClubMaxLeave();
                        }
                    }

                    for (int i = 0; i < _tlledger.Count; i++)
                    {
                        if (_tlledger == null)
                        {
                            _tlledger[i].created_by = 378;
                        }

                    }
                    _context.tbl_leave_ledger.AddRange(_tlledger);
                    _context.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }

            }
        }
        //get leave month sequence for calculate leave 
        private void CalculateLeaveSeq()
        {
            if (DateTime.Compare(_leave_credit_date, _leave_tenure_to_date) > 0)
            {
                throw new Exception("Leave Credit date should be less then tenure date");
            }

            for (Byte Index = 0; Index <= 120; Index++)
            {
                if (DateTime.Compare(_leave_credit_date.AddMonths(0 - Index), _leave_tenure_from_date) >= 0)
                {
                    _IsLeaveTypeSeq = (Byte)(Index + (Byte)1);
                }
            }
        }
        //delete existing leave with same payroll month and leave info id and transaction type 1 (added by system)
        private void deleteLeaveCredit()
        {
            //delete the Existing 
            List<tbl_leave_ledger> data = _context.tbl_leave_ledger.Where(p => p.leave_info_id == _leaveInfoId && p.monthyear == _payrollMonthyear && (p.transaction_type == 1 || p.transaction_type == 3)).ToList();
            for (int i = 0; i < data.Count; i++)
            {
                _context.Entry(data[i]).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }
            _context.tbl_leave_ledger.AttachRange(data);
            _context.SaveChanges();
        }

        //get all employee list which are applicable for leave credit
        private void GetEmployeeList()
        {
            tbl_leave_applicablity Tla = _context.tbl_leave_applicablity.FirstOrDefault(p => p.leave_info_id == _leaveInfoId && p.is_deleted == 0);
            if (Tla != null)
            {
                //get the Religion
                if (Tla.is_aplicable_on_all_religion == 1)
                {
                    _ApplicableReligionID = _context.tbl_religion_master.Select(p => p.religion_id).ToList();
                }
                else
                {
                    _ApplicableReligionID = _context.tbl_leave_appcbl_on_religion.Where(p => p.l_app_id == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p => p.religion_id ?? 0).ToList();
                }
                //get the all department                
                if (Tla.is_aplicable_on_all_department == 1)
                {
                    _ApplicableDepartmentID = _context.tbl_department_master.Select(p => p.department_id).ToList();
                }
                else
                {
                    _ApplicableDepartmentID = _context.tbl_leave_app_on_dept.Where(p => p.lid == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p => p.id ?? 0).ToList();
                }
                //get the all employment type
                if (Tla.is_aplicable_on_all_emp_type == 1)
                {
                    _ApplicableEmploymentTypeID = new List<int>();
                    foreach (int en in Enum.GetValues(typeof(enm_employment_type)))
                    { _ApplicableEmploymentTypeID.Add(en); }

                }
                else
                {
                    _ApplicableEmploymentTypeID = _context.tbl_leave_app_on_emp_type.Where(p => p.l_app_id == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p => new { employment_type = Convert.ToInt32(p.employment_type) }).Select(p => p.employment_type).ToList(); // add by ravi
                    //  _ApplicableEmploymentTypeID = _context.tbl_leave_app_on_emp_type.Where(p => p.l_app_id == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p =>new { employment_type = (int)p.employment_type } ).Select(p=>p.employment_type).ToList();
                }

                //get the Location ID
                if (Tla.is_aplicable_on_all_company == 1)
                {
                    _ApplicableLocationID = _context.tbl_location_master.Select(p => p.location_id).ToList();
                }
                else if (Tla.is_aplicable_on_all_company == 0 && Tla.is_aplicable_on_all_location == 1)
                {
                    var cid = _context.tbl_leave_appcbl_on_company.Where(p => p.l_a_id == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p => p.c_id).ToArray();
                    _ApplicableLocationID = _context.tbl_location_master.Where(p => cid.Contains(p.company_id)).Select(p => p.location_id).ToList();

                }
                else if (Tla.is_aplicable_on_all_company == 0 && Tla.is_aplicable_on_all_location == 0)
                {
                    _ApplicableLocationID = _context.tbl_leave_appcbl_on_company.Where(p => p.l_a_id == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p => p.location_id ?? 0).ToList();
                }

                _ApplicableLocationID.RemoveAll(p => p == 0);
                _ApplicableEmploymentTypeID.RemoveAll(p => p == 0);
                _ApplicableDepartmentID.RemoveAll(p => p == 0);

                //Now get the Employee List
                //_ApplicableEmpID = _context.tbl_emp_officaial_sec.Where(p => p.is_deleted == 0 && p.current_employee_type != 100 && _ApplicableLocationID.Contains(p.location_id ?? 0) &&
                //_ApplicableDepartmentID.Contains(p.department_id ?? 0) && _ApplicableEmploymentTypeID.Contains(p.current_employee_type)
                //&& _ApplicableReligionID.Contains(p.religion_id ?? 0)
                //).Select(p => p.employee_id ?? 0).Distinct().ToList();

            }
        }



        /// <summary>
        /// This Function get the Previous total Leave which are credited till now
        /// </summary>
        private void GetPrevoiusEmpLeaveCredit()
        {

            _EmpLeaveBalenceList = _ApplicableEmpID.Select(p => new EmpLeaveBalence { EmployeeId = p, TotalCredit = 0, TotalDebit = 0 }).ToList();
            var TempVariable = _context.tbl_leave_ledger.Where(p => p.leave_type_id == _leave_type_id && p.transaction_date >= _leave_tenure_from_date &&
              p.transaction_date <= _leave_tenure_to_date && p.credit > 0 && p.transaction_type < 100)
            .GroupBy(p => p.e_id).Select(g => new { eid = g.Key, totalCredit = g.Sum(p => p.credit) }).ToList();
            _EmpLeaveBalenceList.ForEach(p =>
            {
                var TempIndex = TempVariable.FirstOrDefault(q => q.eid == p.EmployeeId);
                p.TotalCredit = TempIndex == null ? 0 : TempIndex.totalCredit;
            });
        }
        /// <summary>
        /// This Function get the total Leave which are debit till now
        /// </summary>
        private void GetPrevoiusEmpLeaveDebit()
        {
            var TempVariable = _context.tbl_leave_ledger.Where(p => p.leave_type_id == _leave_type_id && p.transaction_date >= _leave_tenure_from_date &&
              p.transaction_date <= _leave_tenure_to_date && p.dredit > 0)
            .GroupBy(p => p.e_id).Select(g => new { eid = g.Key, totalDebit = g.Sum(p => p.dredit) }).ToList();
            _EmpLeaveBalenceList.ForEach(p =>
            {
                var TempIndex = TempVariable.FirstOrDefault(q => q.eid == p.EmployeeId);
                p.TotalDebit = TempIndex == null ? 0 : TempIndex.totalDebit;
            });
        }
        // get employee list which are applicable for debit/expire leave according to leave frequency type (1-monthly,2 Quaterly,3 Half yealry,4 yearly)
        // and month like (1-montahly,3-quaterly,6-halfyearly,12-yearly)
        private void SetTenureEmpLeaveExpired()
        {
            int FreqMonth = 1;
            switch (_frequency_type)
            {
                case 1:
                    FreqMonth = 1; break;
                case 2:
                    FreqMonth = 3; break;
                case 3:
                    FreqMonth = 6; break;
                case 4:
                    FreqMonth = 12; break;
            }
            if ((_IsLeaveTypeSeq - 1) % FreqMonth == 0 && _is_leave_accrue != 1)
            {
                _tlledger.AddRange(_EmpLeaveBalenceList.Where(p => p.TotalCredit - p.TotalDebit > 0).Select(p => new tbl_leave_ledger
                {
                    leave_type_id = _leave_type_id,
                    leave_info_id = _leaveInfoId,
                    transaction_date = _leave_credit_date,
                    entry_date = _dateTime,
                    transaction_type = 3,
                    monthyear = _payrollMonthyear,
                    transaction_no = _payrollMonthyear,
                    leave_addition_type = _frequency_type,
                    credit = 0,
                    dredit = p.TotalCredit - p.TotalDebit,
                    remarks = "",
                    e_id = p.EmployeeId
                }));
            }
        }
        // get employee list which are applicable for credit leave according to leave frequency type (1-monthly,2 Quaterly,3 Half yealry,4 yearly)
        // and month like (1-montahly,3-quaterly,6-halfyearly,12-yearly)
        private void SetTenureEmpLeaveCredit()
        {
            int FreqMonth = 1;
            switch (_frequency_type)
            {
                case 1:
                    FreqMonth = 1; break;
                case 2:
                    FreqMonth = 3; break;
                case 3:
                    FreqMonth = 6; break;
                case 4:
                    FreqMonth = 12; break;
            }
            if ((_IsLeaveTypeSeq - 1) % FreqMonth == 0)
            {
                _tlledger.AddRange(_EmpLeaveBalenceList.Where(p => (_is_leave_accrue == 2 || (_is_leave_accrue == 1 && p.TotalCredit <= _max_accrue))).Select(p => new tbl_leave_ledger
                {
                    leave_type_id = _leave_type_id,
                    leave_info_id = _leaveInfoId,
                    transaction_date = _leave_credit_date,
                    entry_date = _dateTime,
                    transaction_type = 1,
                    monthyear = _payrollMonthyear,
                    transaction_no = _payrollMonthyear,
                    leave_addition_type = _frequency_type,
                    credit = _is_leave_accrue == 1 ? (p.TotalCredit + _leave_credit_number > _max_accrue ? _max_accrue - p.TotalCredit : _leave_credit_number) : _leave_credit_number,
                    //credit = _is_leave_accrue == 1 ? (p.TotalCredit + _leave_credit_number > _max_accrue ? _max_accrue - p.TotalCredit : _leave_credit_number) : _leave_credit_day,
                    dredit = 0,
                    remarks = "",
                    e_id = p.EmployeeId
                }));
            }
        }

        //set variable for calculate last year leave in new year first calender motnh that is _IsLeaveTypeSeq==1
        private void SetPreviousVaribale()
        {
            var TempVaribale = _context.tbl_leave_rule.Where(p => p.tbl_leave_info.leave_type_id == _leave_type_id
            && p.is_deleted == 0 && p.tbl_leave_info.leave_tenure_to_date <= _leave_tenure_from_date
            ).OrderByDescending(p => p.tbl_leave_info.leave_tenure_to_date).Select(
                p => new
                {
                    p.leave_info_id,
                    p.maximum_leave_clubbed_in_tenure_number_of_leave,
                    p.maximum_carried_forward,
                    p.can_carried_forward,
                    p.tbl_leave_info.leave_tenure_from_date,
                    p.tbl_leave_info.leave_tenure_to_date
                }).FirstOrDefault();

            if (TempVaribale == null)
            {
                _can_carried_forward = 0;
                _maximum_leave_clubbed_in_tenure_number_of_leave = 1000;
                _maximum_carried_forward = 1000;
                _PreviousleaveInfoId = 0;
                _Previousleave_tenure_from_date = Convert.ToDateTime("1-jan-2000");
                _Previousleave_tenure_to_date = Convert.ToDateTime("1-jan-2000");
            }
            else
            {
                _can_carried_forward = TempVaribale.can_carried_forward;
                _maximum_leave_clubbed_in_tenure_number_of_leave = TempVaribale.maximum_leave_clubbed_in_tenure_number_of_leave;
                _maximum_carried_forward = TempVaribale.maximum_carried_forward;
                _PreviousleaveInfoId = TempVaribale.leave_info_id ?? 0;
                _Previousleave_tenure_from_date = TempVaribale.leave_tenure_from_date;
                _Previousleave_tenure_to_date = TempVaribale.leave_tenure_to_date;
            }

        }
        //get employee list which are applicable in from last year leave credit ploicy
        private void GetPrevoiusTenureEmpLeaveCredit()
        {

            _EmpLeaveBalenceListPrev = _ApplicableEmpID.Select(p => new EmpLeaveBalence { EmployeeId = p, TotalCredit = 0, TotalDebit = 0 }).ToList();

            var TempVariable = _context.tbl_leave_ledger.Where(p => p.leave_type_id == _leave_type_id && p.transaction_date >= _Previousleave_tenure_from_date &&
              p.transaction_date <= _Previousleave_tenure_to_date && p.credit > 0 && p.transaction_type < 100)
            .GroupBy(p => p.e_id).Select(g => new { eid = g.Key, totalCredit = g.Sum(p => p.credit) }).ToList();

            var TempVariable1 = _context.tbl_leave_ledger.Where(p => p.leave_type_id == _leave_type_id && p.transaction_date >= _Previousleave_tenure_from_date &&
              p.transaction_date <= _Previousleave_tenure_to_date && p.credit > 0 && p.transaction_type == 100)
            .GroupBy(p => p.e_id).Select(g => new { eid = g.Key, totalCredit = g.Sum(p => p.credit) }).ToList();


            _EmpLeaveBalenceListPrev.ForEach(p =>
            {
                var TempIndex = TempVariable.FirstOrDefault(q => q.eid == p.EmployeeId);
                var TempIndex1 = TempVariable.FirstOrDefault(q => q.eid == p.EmployeeId);
                p.TotalCredit = TempIndex == null ? 0 : TempIndex.totalCredit;
                p.TotalPreviousCredit = TempIndex == null ? 0 : TempIndex1.totalCredit;
            });
        }
        //get employee list which are applicable in from last year leave debit ploicy
        private void GetPrevoiusTenureEmpLeaveDebit()
        {
            var TempVariable = _context.tbl_leave_ledger.Where(p => p.leave_type_id == _leave_type_id && p.transaction_date >= _Previousleave_tenure_from_date &&
             p.transaction_date <= _Previousleave_tenure_to_date && p.dredit > 0)
           .GroupBy(p => p.e_id).Select(g => new { eid = g.Key, totalDebit = g.Sum(p => p.dredit) }).ToList();
            _EmpLeaveBalenceList.ForEach(p =>
            {
                var TempIndex = TempVariable.FirstOrDefault(q => q.eid == p.EmployeeId);
                p.TotalDebit = TempIndex == null ? 0 : TempIndex.totalDebit;
            });

            _EmpLeaveBalenceListPrev.ForEach(p =>
            {
                var TempIndex = TempVariable.FirstOrDefault(q => q.eid == p.EmployeeId);
                var TempIndex1 = TempVariable.FirstOrDefault(q => q.eid == p.EmployeeId);
                p.TotalDebit = TempIndex == null ? 0 : TempIndex.totalDebit;

            });
        }

        public void SetClubMaxLeave()
        {
            //get all last year employee list 
            _EmpLeaveBalenceListPrev.ForEach(p =>
            {
                p.TotalClub = (p.TotalCredit - p.TotalDebit) > _maximum_leave_clubbed_in_tenure_number_of_leave ? _maximum_leave_clubbed_in_tenure_number_of_leave : (p.TotalCredit - p.TotalDebit) > 0 ? (p.TotalCredit - p.TotalDebit) : 0;
                p.TotalExpired = p.TotalCredit + p.TotalPreviousCredit - p.TotalDebit > 0 ? p.TotalCredit + p.TotalPreviousCredit - p.TotalDebit : 0;
            });
            _EmpLeaveBalenceListPrev.ForEach(p =>
            {
                p.TotalCarryforward = _can_carried_forward == 1 ? (p.TotalClub + p.TotalPreviousCredit) > _maximum_carried_forward ? _maximum_carried_forward : (p.TotalClub + p.TotalPreviousCredit) : 0;
            });

            //get employee list for leave debit
            _tlledger.AddRange(_EmpLeaveBalenceListPrev.Where(p => p.TotalExpired > 0).Select(p => new tbl_leave_ledger
            {
                leave_type_id = _leave_type_id,
                leave_info_id = _PreviousleaveInfoId,
                transaction_date = _Previousleave_tenure_to_date,
                entry_date = _dateTime,
                transaction_type = 3,
                monthyear = _payrollMonthyear,
                transaction_no = _payrollMonthyear,
                leave_addition_type = _frequency_type,
                credit = 0,
                dredit = p.TotalExpired,
                e_id = p.EmployeeId
            }).ToList());

            //get employee list for leave credit
            _tlledger.AddRange(_EmpLeaveBalenceListPrev.Where(p => p.TotalExpired > 0).Select(p => new tbl_leave_ledger
            {
                leave_type_id = _leave_type_id,
                leave_info_id = _leaveInfoId,
                transaction_date = _leave_tenure_from_date,
                entry_date = _dateTime,
                transaction_type = 100,
                monthyear = _payrollMonthyear,
                transaction_no = _payrollMonthyear,
                leave_addition_type = _frequency_type,
                credit = p.TotalCarryforward,
                dredit = 0,
                e_id = p.EmployeeId
            }).ToList());


        }





        public void CalculateNewJoinEmpLeave(List<int> _list)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {

                    _mdclist = _list;
                    _tlledger = new List<tbl_leave_ledger>();
                    CalculateLeaveSeq();
                    Get_new_joiningleave();

                    _context.tbl_leave_ledger.AddRange(_tlledger);
                    _context.SaveChanges();
                    trans.Commit();

                    _tlledger.Clear();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }

            }
        }
        private void Get_new_joiningleave()
        {

            DateTime last_schedulerdate = _context.tbl_leave_ledger.OrderByDescending(x => x.sno).FirstOrDefault().transaction_date;

            tbl_leave_applicablity Tla = _context.tbl_leave_applicablity.FirstOrDefault(p => p.leave_info_id == _leaveInfoId && p.is_deleted == 0);
            if (Tla != null)
            {
                //get the Religion
                if (Tla.is_aplicable_on_all_religion == 1)
                {
                    _ApplicableReligionID = _context.tbl_religion_master.Select(p => p.religion_id).ToList();
                }
                else
                {
                    _ApplicableReligionID = _context.tbl_leave_appcbl_on_religion.Where(p => p.l_app_id == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p => p.religion_id ?? 0).ToList();
                }
                //get the all department                
                if (Tla.is_aplicable_on_all_department == 1)
                {
                    _ApplicableDepartmentID = _context.tbl_department_master.Select(p => p.department_id).ToList();
                }
                else
                {
                    _ApplicableDepartmentID = _context.tbl_leave_app_on_dept.Where(p => p.lid == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p => p.id ?? 0).ToList();
                }
                //get the all employment type
                if (Tla.is_aplicable_on_all_emp_type == 1)
                {
                    _ApplicableEmploymentTypeID = new List<int>();
                    foreach (int en in Enum.GetValues(typeof(enm_employment_type)))
                    { _ApplicableEmploymentTypeID.Add(en); }

                }
                else
                {
                    _ApplicableEmploymentTypeID = _context.tbl_leave_app_on_emp_type.Where(p => p.l_app_id == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p => new { employment_type = Convert.ToInt32(p.employment_type) }).Select(p => p.employment_type).ToList(); // add by ravi
                                                                                                                                                                                                                                                                                 //  _ApplicableEmploymentTypeID = _context.tbl_leave_app_on_emp_type.Where(p => p.l_app_id == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p =>new { employment_type = (int)p.employment_type } ).Select(p=>p.employment_type).ToList();
                }

                //get the Location ID
                if (Tla.is_aplicable_on_all_company == 1)
                {
                    _ApplicableLocationID = _context.tbl_location_master.Select(p => p.location_id).ToList();
                }
                else if (Tla.is_aplicable_on_all_company == 0 && Tla.is_aplicable_on_all_location == 1)
                {
                    var cid = _context.tbl_leave_appcbl_on_company.Where(p => p.l_a_id == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p => p.c_id).ToArray();
                    _ApplicableLocationID = _context.tbl_location_master.Where(p => cid.Contains(p.company_id)).Select(p => p.location_id).ToList();

                }
                else if (Tla.is_aplicable_on_all_company == 0 && Tla.is_aplicable_on_all_location == 0)
                {
                    _ApplicableLocationID = _context.tbl_leave_appcbl_on_company.Where(p => p.l_a_id == Tla.leave_applicablity_id && p.is_deleted == 0).Select(p => p.location_id ?? 0).ToList();
                }

                _ApplicableLocationID.RemoveAll(p => p == 0);
                _ApplicableEmploymentTypeID.RemoveAll(p => p == 0);
                _ApplicableDepartmentID.RemoveAll(p => p == 0);


            }
            //Now get the Employee List

            //var new_joinings = _context.tbl_emp_officaial_sec.Where(p => p.is_deleted == 0 && p.current_employee_type != 100 && p.current_employee_type == 3 && _ApplicableLocationID.Contains(p.location_id ?? 0) &&
            //   _ApplicableDepartmentID.Contains(p.department_id ?? 0) && _ApplicableEmploymentTypeID.Contains(p.current_employee_type)
            //   && _ApplicableReligionID.Contains(p.religion_id ?? 0) && _mdclist.Contains(Convert.ToInt32(p.employee_id))
            // ).Select(p => new { p.employee_id, p.date_of_joining, diff_month = (DateTime.Now.Month - p.date_of_joining.Month) + 1 }).Distinct().ToList();


            int FreqMonth = 1;
            switch (_frequency_type)
            {
                case 1:
                    FreqMonth = 1; break;
                case 2:
                    FreqMonth = 3; break;
                case 3:
                    FreqMonth = 6; break;
                case 4:
                    FreqMonth = 12; break;
            }


            List<EmpLeaveBalence> EmpLeaveBalence = new List<EmpLeaveBalence>();

#if false
            foreach (var joining_ in new_joinings)
            {
                EmpLeaveBalence objempleave = new EmpLeaveBalence();

                double _leavecredit = (_leave_credit_number / FreqMonth) * joining_.diff_month;

                objempleave.TotalCredit = Math.Round(_leavecredit, 2);
                objempleave.TotalDebit = 0;
                objempleave.EmployeeId = joining_.employee_id ?? 0;

                EmpLeaveBalence.Add(objempleave);
            }
#endif


            _tlledger.AddRange(EmpLeaveBalence.Where(p => (_is_leave_accrue == 2 || (_is_leave_accrue == 1 && p.TotalCredit <= _max_accrue))).Select(p => new tbl_leave_ledger
            {
                leave_type_id = _leave_type_id,
                leave_info_id = _leaveInfoId,
                transaction_date = _leave_credit_date,
                entry_date = _dateTime,
                transaction_type = 1,
                monthyear = _payrollMonthyear,
                transaction_no = _payrollMonthyear,
                leave_addition_type = _frequency_type,
                credit = p.TotalCredit,
                dredit = p.TotalDebit,
                remarks = "",
                e_id = p.EmployeeId
            }));


        }


        #if false
#region ** START SAVE MANNUAL LEAVE **

        public List<tbl_leave_type> Get_Leave_typesByStarting_Year_Date()
        {
            List<tbl_leave_type> objlist = new List<tbl_leave_type>();

            DateTime startyear = new DateTime(DateTime.Now.Year, 1, 1);
            var data = _context.tbl_leave_info.AsEnumerable().Where(a => a.leave_tenure_from_date.Date <= startyear.Date && startyear.Date <= a.leave_tenure_to_date.Date && a.is_active == 1).ToList();
            if (data.Count > 0)
            {

                foreach (var item in data)
                {
                    tbl_leave_type objleavetype = new tbl_leave_type();

                    var leavetypee = _context.tbl_leave_type.OrderByDescending(a => a.leave_type_id).Where(b => b.leave_type_id == item.leave_type_id).FirstOrDefault();

                    objleavetype.leave_type_id = leavetypee.leave_type_id;
                    objleavetype.leave_type_name = leavetypee.leave_type_name;

                    objlist.Add(objleavetype);
                }

            }

            return objlist;
        }
        public int Save_Mannual_Leave(List<LeaveLedgerModell> _leave)
        {
            try
            {
                int monthyear_ = Convert.ToInt32(DateTime.Now.Year.ToString() + (DateTime.Now.Month.ToString().Length > 1 ? DateTime.Now.Month.ToString() : "0" + DateTime.Now.Month.ToString()));
                var leave_info = _context.tbl_leave_info.Where(x => x.is_active == 1).ToList();

                List<tbl_leave_ledger> _add_leaves = _leave.Select(p => new tbl_leave_ledger
                {

                    leave_type_id = p.leave_type_id,
                    leave_info_id = (leave_info.Where(x => x.leave_type_id == p.leave_type_id).FirstOrDefault() != null ? leave_info.Where(x => x.leave_type_id == p.leave_type_id).FirstOrDefault().leave_info_id : 0),
                    transaction_date = DateTime.Now,
                    entry_date = DateTime.Now,
                    transaction_type = p.transaction_type,
                    monthyear = monthyear_,
                    transaction_no = monthyear_,
                    leave_addition_type = 1,
                    credit = p.credit,
                    dredit = p.dredit,
                    remarks = p.remarks,
                    e_id = p.e_id,
                    created_by = p.created_by
                }).ToList();

                _context.tbl_leave_ledger.AddRange(_add_leaves);
                _context.SaveChanges();

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<LeaveLedgerModell> Get_Leave_LedgerMonthly()
        {
            List<LeaveLedgerModell> emp_leave_list = new List<LeaveLedgerModell>();

            try
            {
                var data = _context.tbl_leave_ledger.Where(p => _clsCurrentUser.DownlineEmpId.Contains(p.e_id ?? 0)).GroupBy(l => new { l.e_id, l.leave_type_id, l.monthyear }).Select(q => new
                {

                    leave_type_id = q.FirstOrDefault().leave_type_id,
                    leave_type_name = q.FirstOrDefault().tbl_leave_type.leave_type_name,
                    leave_info_id = q.FirstOrDefault().tbl_leave_info.leave_info_id,
                    monthyear = q.FirstOrDefault().transaction_date.Year,
                    totalcredit = Math.Round(q.Sum(r => r.credit), 2),
                    totaldebit = Math.Round(q.Sum(r => r.dredit), 2),
                    leavebalance = Math.Round(q.Sum(x => x.credit) - q.Sum(x => x.dredit), 2),
                    emp_id = q.FirstOrDefault().e_id,
                    total_leave_type = q.Count(),
                }).ToList();

                List<int?> _empidd = data.Select(p => p.emp_id).ToList();

                var emp_dataa = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && _empidd.Contains(x.employee_id)).Select(p => new
                {
                    p.employee_id,
                    p.employee_first_name,
                    p.employee_middle_name,
                    p.employee_last_name,
                    p.tbl_employee_id_details.emp_code,
                    p.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(y => y.is_deleted == 0).company_id,
                    comp_name = p.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(y => y.is_deleted == 0).tbl_company_master.company_name,
                    dept_id = p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).department_id,
                    dept_name = p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).tbl_department_master.department_name,
                    loc_id = p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).location_id,
                    loc_name = p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).tbl_location_master.location_name,
                }).ToList();

                emp_leave_list = data.Select(p => new LeaveLedgerModell
                {
                    company_id = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).company_id ?? 0 : 0,
                    emp_name = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? string.Format("{0} {1} {2}", emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_first_name,
                                emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_middle_name, emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_last_name) : "",
                    emp_code = emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).emp_code,
                    credit = p.totalcredit,
                    dredit = p.totaldebit,
                    monthyear = p.monthyear,
                    leave_type_id = p.leave_type_id ?? 0,
                    leave_type_name = p.leave_type_name,
                    balance = p.leavebalance,
                    e_id = p.emp_id ?? 0,
                    dept = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).dept_id ?? 0 : 0,
                    dept_name = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).dept_name : "",
                    location_id = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).loc_id ?? 0 : 0,
                    loc_name = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).loc_name : "",
                }).ToList();

            }
            catch (Exception ex)
            {

            }

            return emp_leave_list;
        }
        public List<LeaveLedgerModell> Get_Leave_Ledger()
        {
            List<LeaveLedgerModell> emp_leave_list = new List<LeaveLedgerModell>();

            //string ActionName = "";
            //int _emp_id = 0;

            //int empidd_ = 0;


            //var temp_emp = _AC.HttpContext.User.Claims.Where(p => p.Type == "empid").FirstOrDefault();
            //if (temp_emp != null)
            //{
            //    _emp_id = Convert.ToInt32(temp_emp.Value);
            //}

            //clsEmployeeDetail objempcls = new clsEmployeeDetail(_context, _config, _AC);
            //Dictionary<int, string> _emp_action = objempcls.GetEmpID_and_action_ForAuthorization(_emp_id);


            //foreach (KeyValuePair<int, string> item in _emp_action)
            //{
            //    empidd_ = item.Key;
            //    ActionName = item.Value;
            //}


            //Classes.clsLoginEmpDtl ob = new clsLoginEmpDtl(_context, Convert.ToInt32(empidd_), ActionName, _AC);

            //if (!ob.is_valid())
            //{
            //    return emp_leave_list;
            //}


            try
            {
                var data = _context.tbl_leave_ledger.Where(p => _clsCurrentUser.DownlineEmpId.Contains(p.e_id ?? 0)).GroupBy(l => new { l.e_id, l.leave_type_id }).Select(q => new
                {
                    entry_date = q.FirstOrDefault().entry_date,
                    leave_type_id = q.FirstOrDefault().leave_type_id,
                    leave_type_name = q.FirstOrDefault().tbl_leave_type.leave_type_name,
                    leave_info_id = q.FirstOrDefault().tbl_leave_info.leave_info_id,
                    totalcredit = q.Sum(r => r.credit),
                    totaldebit = q.Sum(r => r.dredit),
                    leavebalance = q.Sum(x => x.credit) - q.Sum(x => x.dredit),
                    emp_id = q.FirstOrDefault().e_id,
                    total_leave_type = q.Count(),
                }).ToList();

                List<int?> _empidd = data.Select(p => p.emp_id).ToList();

                var emp_dataa = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && _empidd.Contains(x.employee_id)).Select(p => new
                {
                    p.employee_id,
                    p.employee_first_name,
                    p.employee_middle_name,
                    p.employee_last_name,
                    p.tbl_employee_id_details.emp_code,
                    //p.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(y => y.is_deleted == 0).company_id,
                    //comp_name = p.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(y => y.is_deleted == 0).tbl_company_master.company_name,
                    //dept_id = p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).department_id,
                    //dept_name = p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).tbl_department_master.department_name,
                    //loc_id = p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).location_id,
                    //loc_name = p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).tbl_location_master.location_name,
                }).ToList();

                emp_leave_list = data.Select(p => new LeaveLedgerModell
                {
                    year = p.entry_date.Year,
                    company_id = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).company_id ?? 0 : 0,
                    emp_name = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ?
                    string.Format("{0} {1} {2}", emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_first_name,
                                emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_middle_name,
                                emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_last_name) : "",
                    emp_code = emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).emp_code,
                    credit = p.totalcredit,
                    dredit = p.totaldebit,
                    leave_type_id = p.leave_type_id ?? 0,
                    leave_type_name = p.leave_type_name,
                    balance = p.leavebalance,
                    e_id = p.emp_id ?? 0,
                    //dept = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).dept_id ?? 0 : 0,
                    //dept_name = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).dept_name : "",
                    //location_id = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).loc_id ?? 0 : 0,
                    //loc_name = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).loc_name : "",
                }).ToList();


                //if (ob.is_Admin == 1)
                //{
                //    emp_leave_list = data.Select(p => new LeaveLedgerModell
                //    {
                //        company_id = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).company_id ?? 0 : 0,
                //        emp_name_code = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? string.Format("{0} {1} {2} ({3})", emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_first_name,
                //                emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_middle_name, emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_last_name,
                //                emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).emp_code) : "",
                //        credit = p.totalcredit,
                //        dredit = p.totaldebit,
                //        leave_type_id = p.leave_type_id ?? 0,
                //        leave_type_name = p.leave_type_name,
                //        balance = p.leavebalance,
                //        e_id = p.emp_id ?? 0,
                //        dept = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).dept_id ?? 0 : 0,
                //        dept_name = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).dept_name : "",
                //        location_id = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).loc_id ?? 0 : 0,
                //        loc_name = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).loc_name : "",
                //    }).ToList();
                //}
                //else if (ob.is_manager == 1)
                //{
                //    List<int?> emp_lst = _context.tbl_emp_manager.Where(x => x.is_deleted == 0 && (x.m_one_id == _emp_id || x.m_two_id == _emp_id || x.m_three_id == _emp_id)).Select(p => p.employee_id).ToList();

                //    emp_leave_list = data.Where(x=> emp_lst.Contains(x.emp_id)).Select(p => new LeaveLedgerModell
                //    {
                //        company_id = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).company_id ?? 0 : 0,
                //        emp_name_code = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? string.Format("{0} {1} {2} ({3})", emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_first_name,
                //                emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_middle_name, emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_last_name,
                //                emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).emp_code) : "",
                //        credit = p.totalcredit,
                //        dredit = p.totaldebit,
                //        leave_type_id = p.leave_type_id ?? 0,
                //        leave_type_name = p.leave_type_name,
                //        balance = p.leavebalance,
                //        e_id = p.emp_id ?? 0,
                //        dept = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).dept_id ?? 0 : 0,
                //        dept_name = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).dept_name : "",
                //        location_id = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).loc_id ?? 0 : 0,
                //        loc_name = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).loc_name : "",
                //    }).ToList();

                //}
                //else
                //{
                //    emp_leave_list = data.Where(x=>x.emp_id==_emp_id).Select(p => new LeaveLedgerModell
                //    {
                //        company_id = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).company_id ?? 0 : 0,
                //        emp_name_code = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? string.Format("{0} {1} {2} ({3})", emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_first_name,
                //                emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_middle_name, emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_last_name,
                //                emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).emp_code) : "",
                //        credit = p.totalcredit,
                //        dredit = p.totaldebit,
                //        leave_type_id = p.leave_type_id ?? 0,
                //        leave_type_name = p.leave_type_name,
                //        balance = p.leavebalance,
                //        e_id = p.emp_id ?? 0,
                //        dept = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).dept_id ?? 0 : 0,
                //        dept_name = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).dept_name : "",
                //        location_id = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).loc_id ?? 0 : 0,
                //        loc_name = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).loc_name : "",
                //    }).ToList();
                //}



            }
            catch (Exception ex)
            {

            }

            return emp_leave_list;
        }
        public List<LeaveLedgerModell> Get_Leave_Ledger_By_Employee_Id(int employee_id)
        {
            List<LeaveLedgerModell> emp_leave_list = new List<LeaveLedgerModell>();


            try
            {
                var data = _context.tbl_leave_ledger.Where(p => p.e_id == employee_id).GroupBy(l => new { l.e_id, l.leave_type_id }).Select(q => new
                {

                    leave_type_id = q.FirstOrDefault().leave_type_id,
                    leave_type_name = q.FirstOrDefault().tbl_leave_type.leave_type_name,
                    leave_info_id = q.FirstOrDefault().tbl_leave_info.leave_info_id,
                    totalcredit = q.Sum(r => r.credit),
                    totaldebit = q.Sum(r => r.dredit),
                    leavebalance = q.Sum(x => x.credit) - q.Sum(x => x.dredit),
                    emp_id = q.FirstOrDefault().e_id,
                    total_leave_type = q.Count(),
                }).ToList();

                List<int?> _empidd = data.Select(p => p.emp_id).ToList();

                var emp_dataa = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && _empidd.Contains(x.employee_id)).Select(p => new
                {
                    p.employee_id,
                    p.employee_first_name,
                    p.employee_middle_name,
                    p.employee_last_name,
                    p.tbl_employee_id_details.emp_code,
                    p.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(y => y.is_deleted == 0).company_id,
                    comp_name = p.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(y => y.is_deleted == 0).tbl_company_master.company_name,
                    //dept_id = p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).department_id,
                    //dept_name = p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).tbl_department_master.department_name,
                    //loc_id = p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).location_id,
                    //loc_name = p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).tbl_location_master.location_name,
                }).ToList();

                emp_leave_list = data.Select(p => new LeaveLedgerModell
                {
                    company_id = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).company_id ?? 0 : 0,
                    emp_name = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? string.Format("{0} {1} {2}", emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_first_name,
                                emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_middle_name, emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).employee_last_name) : "",
                    emp_code = emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).emp_code,
                    credit = p.totalcredit,
                    dredit = p.totaldebit,
                    leave_type_id = p.leave_type_id ?? 0,
                    leave_type_name = p.leave_type_name,
                    balance = p.leavebalance,
                    e_id = p.emp_id ?? 0,
                    //dept = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).dept_id ?? 0 : 0,
                    //dept_name = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).dept_name : "",
                    //location_id = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).loc_id ?? 0 : 0,
                    //loc_name = emp_dataa.Where(q => q.employee_id == p.emp_id).FirstOrDefault() != null ? emp_dataa.FirstOrDefault(q => q.employee_id == p.emp_id).loc_name : "",
                }).ToList();



            }
            catch (Exception ex)
            {

            }

            return emp_leave_list;
        }
#endregion ** END SAVE MANNUAL LEAVE **
#endif
    }
#endif

}

using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using projAPI.Classes;
using projAPI.Model;
using projContext;
using projContext.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI
{
    public class Payroll
    {
        private readonly Context _context;
        private readonly int EmpID_, PayrollMonthyear_, userId_, SGID_, _company_id;
        private readonly DateTime currentdateTime_;
        private IHostingEnvironment _hostingEnvironment;
        private IConverter _converter;
        private readonly string url_;
        private readonly int _SepId;
        private readonly IConfiguration _config;

        public Payroll(int company_id, string apiurl, IConverter converter, IHostingEnvironment environment, Context context, int EmpID, int PayrollMonthyear, int userId, int SGID, IConfiguration config)
        {
            _config = config;
            PayrollMonthyear_ = PayrollMonthyear;
            EmpID_ = EmpID;
            userId_ = userId;
            currentdateTime_ = DateTime.Now;
            SGID_ = SGID;
            _context = context;
            _converter = converter;
            _company_id = company_id;
            _hostingEnvironment = environment;
            url_ = apiurl;
        }
        public Payroll(int company_id, string apiurl, IConverter converter, IHostingEnvironment environment, Context context, int EmpID, int PayrollMonthyear, int userId, int SGID, IConfiguration config, int sepId)
        {
            _config = config;
            PayrollMonthyear_ = PayrollMonthyear;
            EmpID_ = EmpID;
            userId_ = userId;
            currentdateTime_ = DateTime.Now;
            SGID_ = SGID;
            _context = context;
            _converter = converter;
            _company_id = company_id;
            _hostingEnvironment = environment;
            url_ = apiurl;
            _SepId = sepId;
        }

        private void delete_exiting_data_fnf()
        {
            try
            {

                //remove from salary Input
                List<tbl_salary_input> tbl_salary_inputs = _context.tbl_salary_input.Where(p => p.emp_id == EmpID_ && p.monthyear == PayrollMonthyear_ && p.is_active == 1 && p.company_id == _company_id && p.is_fnf_comp==1).ToList();
                //tbl_salary_inputs.ForEach(p => { p.is_active = 0; p.modified_by = userId_; p.modified_dt = currentdateTime_; });
                //_context.tbl_salary_input.UpdateRange(tbl_salary_inputs);

                _context.tbl_salary_input.RemoveRange(tbl_salary_inputs);

              //  List<tbl_loan_repayments> tbl_loan_repayments = _context.tbl_loan_repayments.Include(p => p.tbl_loan_req).Where(p => p.req_emp_id == EmpID_ && p.status == 0 && p.is_deleted == 0).ToList().Where(p => p.date.ToString("yyyyMM") == Convert.ToString(PayrollMonthyear_)).ToList();
                ////tbl_loan_repayments.ForEach(p =>
                ////{
                ////    p.is_deleted = 0; p.last_modified_by = userId_; p.last_modified_date = currentdateTime_;
                ////    p.tbl_loan_req.is_closed = 0;
                ////});
               // _context.tbl_loan_repayments.RemoveRange(tbl_loan_repayments);
                // do not remove from tbl_lossofpay_masters because it will take time
                //tbl_lossofpay_master obj_lop = _context.tbl_lossofpay_master.Where(a => a.emp_id == EmpID_ && a.monthyear == PayrollMonthyear_ && a.is_active == 1 && a.company_id == _company_id).FirstOrDefault();
                //if (obj_lop != null)
                //{
                //    obj_lop.is_active = 0;
                //    _context.tbl_lossofpay_master.Update(obj_lop);
                //}
                _context.SaveChanges();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private void delete_exiting_data()
        {
            try
            {

                //remove from salary Input
                List<tbl_salary_input> tbl_salary_inputs = _context.tbl_salary_input.Where(p => p.emp_id == EmpID_ && p.monthyear == PayrollMonthyear_ && p.is_active == 1 && p.company_id == _company_id).ToList();
                //tbl_salary_inputs.ForEach(p => { p.is_active = 0; p.modified_by = userId_; p.modified_dt = currentdateTime_; });
                //_context.tbl_salary_input.UpdateRange(tbl_salary_inputs);

                _context.tbl_salary_input.RemoveRange(tbl_salary_inputs);

                List<tbl_loan_repayments> tbl_loan_repayments = _context.tbl_loan_repayments.Include(p => p.tbl_loan_req).Where(p => p.req_emp_id == EmpID_ && p.status == 0 && p.is_deleted == 0).ToList().Where(p => p.date.ToString("yyyyMM") == Convert.ToString(PayrollMonthyear_)).ToList();
                //tbl_loan_repayments.ForEach(p =>
                //{
                //    p.is_deleted = 0; p.last_modified_by = userId_; p.last_modified_date = currentdateTime_;
                //    p.tbl_loan_req.is_closed = 0;
                //});
                _context.tbl_loan_repayments.RemoveRange(tbl_loan_repayments);
                // do not remove from tbl_lossofpay_masters because it will take time
                //tbl_lossofpay_master obj_lop = _context.tbl_lossofpay_master.Where(a => a.emp_id == EmpID_ && a.monthyear == PayrollMonthyear_ && a.is_active == 1 && a.company_id == _company_id).FirstOrDefault();
                //if (obj_lop != null)
                //{
                //    obj_lop.is_active = 0;
                //    _context.tbl_lossofpay_master.Update(obj_lop);
                //}
                _context.SaveChanges();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void freze_attendance()
        {
            List<tbl_daily_attendance> tbl_daily_attendances = _context.tbl_daily_attendance.Where(p => p.emp_id == EmpID_ && p.payrollmonthyear == PayrollMonthyear_ && p.is_freezed == 0).ToList();
            tbl_daily_attendances.ForEach(p => p.is_freezed = 1);
            _context.tbl_daily_attendance.UpdateRange(tbl_daily_attendances);
            _context.SaveChanges();
        }
        class mdlDayStatus
        {
            public DateTime AttendanceDay { get; set; }
            public byte DayStatus { get; set; }
            public double presentValue { get; set; }
            public double absentValue { get; set; }
            public double weekOffValue { get; set; }
            public double holidayValue { get; set; }
            public double leaveValue { get; set; }
            public double totalPaidValue { get; set; }
        }
        private List<mdlDayStatus> GetDayStatus(DateTime fromdt, DateTime todate)
        {
            List<mdlDayStatus> dayStatuses = new List<mdlDayStatus>();
            while (fromdt <= todate)
            {
                dayStatuses.Add(new mdlDayStatus() { AttendanceDay = fromdt, DayStatus = 2, absentValue = 1 });
                fromdt = fromdt.AddDays(1);
            }
            return dayStatuses;
        }


        private void insert_into_loss_of_pay()
        {
            bool isUpdate = true;
            double no_of_loss_day = 0, no_of_present_day = 0, no_of_absent_day = 0, no_of_leave_day = 0, no_of_holiday_day = 0, no_of_paid_day = 0, no_of_weekOff_day = 0; int totaldays = 30;
            tbl_lossofpay_master tbl_lossofpay_masters = _context.tbl_lossofpay_master.Where(p => p.emp_id == EmpID_ && p.monthyear == PayrollMonthyear_ && p.is_active == 1).FirstOrDefault();

            //Get the Total Number of Attendance in
            DateTime Payrollstartdate = DateTime.Now;//Convert.ToDateTime(PayrollMonthyear_.ToString().Substring(0, 4) + "-" + PayrollMonthyear_.ToString().Substring(4, 2) + "-01");
            DateTime Payrollenddate = Payrollstartdate.AddMonths(1).AddDays(-1);
            int Startingday = 1;
            var tbl_payroll_month_setting = _context.tbl_payroll_month_setting.FirstOrDefault(p => p.company_id == _company_id && p.is_deleted == 0);
            if (tbl_payroll_month_setting != null)
            {
                Startingday = tbl_payroll_month_setting.from_date;
            }
            if (Startingday == 1)
            {
                Payrollstartdate = Convert.ToDateTime(PayrollMonthyear_.ToString().Substring(0, 4) + "-" + PayrollMonthyear_.ToString().Substring(4, 2) + "-" + Startingday);
                Payrollenddate = Payrollstartdate.AddMonths(1).AddDays(-1);
            }
            else
            {
                Payrollenddate = Convert.ToDateTime(PayrollMonthyear_.ToString().Substring(0, 4) + "-" + PayrollMonthyear_.ToString().Substring(4, 2) + "-" + (Startingday - 1));
                Payrollstartdate = Payrollenddate.AddMonths(-1).AddDays(1);
            }
            // Get the Joining date of Employee
            var tbl_emp_officaial_sec = _context.tbl_emp_officaial_sec.FirstOrDefault(p => p.employee_id == EmpID_ && p.is_deleted == 0);



            var actualdaystatus = _context.tbl_daily_attendance.Where(p => p.emp_id == EmpID_ && p.attendance_dt >= Payrollstartdate && p
             .attendance_dt <= Payrollenddate).Select(p => new { p.is_holiday, p.is_weekly_off, p.attendance_dt, p.day_status, leave_type = (p.tlr == null ? 0 : p.tlr.tbl_leave_info.leave_type) }).ToList();

            var dayStatus = GetDayStatus(Payrollstartdate, Payrollenddate);
            dayStatus.ForEach(p =>
            {
                var tempdata = actualdaystatus.FirstOrDefault(q => q.attendance_dt == p.AttendanceDay);
                if (tempdata != null)
                {
                    if (tempdata.is_holiday == 1)
                    {
                        p.presentValue = 0;
                        p.absentValue = 0;
                        if (tempdata.is_weekly_off == 1) { p.weekOffValue = 1; }
                        else { p.weekOffValue = 0; }
                        p.holidayValue = 1;
                        p.leaveValue = 0;
                        p.totalPaidValue = 1;
                    }
                    if (tempdata.is_weekly_off == 1)
                    {
                        p.presentValue = 0;
                        p.absentValue = 0;
                        p.weekOffValue = 1;
                        if (tempdata.is_holiday == 1) { p.holidayValue = 1; }
                        else { p.holidayValue = 0; }
                        p.leaveValue = 0;
                        p.totalPaidValue = 1;
                    }

                    if (tempdata.day_status == Convert.ToInt32(enm_DayStatus.Present))
                    {
                        p.presentValue = 1;
                        p.absentValue = 0;
                        if (tempdata.is_weekly_off == 1) { p.weekOffValue = 1; }
                        else { p.weekOffValue = 0; }
                        if (tempdata.is_holiday == 1) { p.holidayValue = 1; }
                        else { p.holidayValue = 0; }
                        p.leaveValue = 0;
                        p.totalPaidValue = 1;
                    }
                    else if (tempdata.day_status == Convert.ToInt32(enm_DayStatus.Absent))
                    {
                        p.presentValue = 0;
                        p.absentValue = 1;
                        p.weekOffValue = 0;
                        p.holidayValue = 0;
                        p.leaveValue = 0;
                        p.totalPaidValue = 0;
                    }
                    else if (tempdata.day_status == Convert.ToInt32(enm_DayStatus.Leave))
                    {
                        p.presentValue = 1;
                        p.absentValue = 0;
                        p.weekOffValue = 0;
                        p.holidayValue = 0;
                        p.leaveValue = 1;
                        p.totalPaidValue = 1;
                    }
                    else if (tempdata.day_status == Convert.ToInt32(enm_DayStatus.HalfPresentAbsent))
                    {
                        p.presentValue = 0.5;
                        p.absentValue = 0.5;
                        if (tempdata.is_weekly_off == 1) { p.weekOffValue = 1; }
                        else { p.weekOffValue = 0; }
                        if (tempdata.is_holiday == 1) { p.holidayValue = 1; }
                        else { p.holidayValue = 0; }
                        p.leaveValue = 0;
                        p.totalPaidValue = 0.5;
                    }
                    else if (tempdata.day_status == Convert.ToInt32(enm_DayStatus.HalfPresentLeave))
                    {
                        p.presentValue = 0.5;
                        p.absentValue = 0;
                        if (tempdata.is_weekly_off == 1) { p.weekOffValue = 1; }
                        else { p.weekOffValue = 0; }
                        if (tempdata.is_holiday == 1) { p.holidayValue = 1; }
                        else { p.holidayValue = 0; }
                        p.leaveValue = 0.5;
                        p.totalPaidValue = 1;
                    }
                    else if (tempdata.day_status == Convert.ToInt32(enm_DayStatus.HalfAbsentLeave))
                    {
                        p.presentValue = 0.5;
                        p.absentValue = 0.5;
                        p.weekOffValue = 0;
                        p.holidayValue = 0;
                        p.leaveValue = 0.5;
                        p.totalPaidValue = 0.5;
                    }
                    p.DayStatus = tempdata.day_status;
                }
            });
            no_of_loss_day = dayStatus.Sum(p => p.absentValue);
            no_of_present_day = dayStatus.Sum(p => p.presentValue);
            no_of_absent_day = dayStatus.Sum(p => p.absentValue);
            no_of_leave_day = dayStatus.Sum(p => p.leaveValue);
            no_of_holiday_day = dayStatus.Sum(p => p.holidayValue);
            no_of_weekOff_day = dayStatus.Sum(p => p.weekOffValue);
            no_of_paid_day = dayStatus.Sum(p => p.totalPaidValue);


            if (tbl_lossofpay_masters == null)
            {
                isUpdate = false;
                tbl_lossofpay_masters = new tbl_lossofpay_master();
                tbl_lossofpay_masters.final_lop_days = Convert.ToDecimal(no_of_loss_day);
            }
            var tlp = _context.tbl_lossofpay_setting.FirstOrDefault(p => p.companyid == _company_id);
            if (tlp == null || tlp.lop_setting_name == 1)
            {
                totaldays = Payrollenddate.Subtract(Payrollstartdate).Days + 1;
                //totaldays = 30;
                //tbl_lossofpay_masters.totaldays = 30;
            }
            else
            {
                //Get Days in Month
                string year = PayrollMonthyear_.ToString().Substring(0, 4);
                string month = PayrollMonthyear_.ToString().Substring(4, 2);
                totaldays = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
                //tbl_lossofpay_masters.totaldays = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));                
            }

            //if (tbl_emp_officaial_sec != null)
            //{
            //    if (tbl_emp_officaial_sec.date_of_joining > Payrollenddate)
            //    {
            //        return;
            //    }
            //    if (tbl_emp_officaial_sec.date_of_joining > Payrollstartdate)
            //    {
            //        int totalattendancedate = Payrollenddate.Subtract(tbl_emp_officaial_sec.date_of_joining).Days;
            //        if (totalattendancedate < 15)
            //        {
            //            double totalPresentday = 0;
            //            //count the Present day status
            //            totalPresentday = _context.tbl_daily_attendance.Where(p => p.emp_id == EmpID_ && p.payrollmonthyear == PayrollMonthyear_ && (p.day_status == 1 || p.day_status == 0 || p.day_status == 3 || p.day_status == 6 || p.day_status == 5))
            //             .Select(p => new { day_status = p.day_status, PresentCount = ((p.day_status == 1 || p.is_holiday == 1 || p.is_weekly_off == 1 || p.day_status == 3) ? 1 : (p.day_status == 5 || p.day_status == 6) ? 0.5 : 0) }).Select(p => p.PresentCount).Sum();
            //            no_of_loss_day = totaldays - totalPresentday;
            //        }
            //        else
            //        {
            //            //count the absent day//Check the Number of day from joining to payroll End                        
            //            no_of_loss_day = no_of_loss_day + tbl_emp_officaial_sec.date_of_joining.Subtract(Payrollstartdate).Days;
            //        }


            //    }
            //}
            tbl_lossofpay_masters.totaldays = totaldays;
            tbl_lossofpay_masters.acutual_lop_days = Convert.ToDecimal(no_of_loss_day);
            tbl_lossofpay_masters.emp_id = EmpID_;
            tbl_lossofpay_masters.monthyear = PayrollMonthyear_;
            tbl_lossofpay_masters.is_active = 1;
            tbl_lossofpay_masters.modified_by = 0;
            tbl_lossofpay_masters.modified_date = new DateTime(2000, 1, 1);
            tbl_lossofpay_masters.created_by = userId_;
            tbl_lossofpay_masters.created_date = currentdateTime_;
            tbl_lossofpay_masters.company_id = _company_id;

            if (tbl_lossofpay_masters.totaldays < tbl_lossofpay_masters.acutual_lop_days)
            {
                tbl_lossofpay_masters.acutual_lop_days = tbl_lossofpay_masters.totaldays;
            }
            tbl_lossofpay_masters.final_lop_days = tbl_lossofpay_masters.acutual_lop_days;
            tbl_lossofpay_masters.Week_off_days = Convert.ToDecimal(no_of_weekOff_day);
            tbl_lossofpay_masters.Holiday_days = Convert.ToDecimal(no_of_holiday_day);
            tbl_lossofpay_masters.Present_days = Convert.ToDecimal(no_of_present_day);
            tbl_lossofpay_masters.Absent_days = Convert.ToDecimal(no_of_absent_day);
            tbl_lossofpay_masters.Leave_days = Convert.ToDecimal(no_of_leave_day);
            tbl_lossofpay_masters.Total_Paid_days = Convert.ToDecimal(no_of_paid_day);
            tbl_lossofpay_masters.is_freezed = 1;

            _context.tbl_lossofpay_master.AttachRange(tbl_lossofpay_masters);

            if (isUpdate)
            {
                _context.Entry(tbl_lossofpay_masters).State = EntityState.Modified;
            }
            else
            {
                _context.Entry(tbl_lossofpay_masters).State = EntityState.Added;
            }
            _context.SaveChanges();

        }

        private void insert_into_loan()
        {
            DateTime start_date_ = Convert.ToDateTime(PayrollMonthyear_.ToString().Substring(0, 4) + "-" + PayrollMonthyear_.ToString().Substring(4, 2) + "-01");
            List<tbl_loan_request> tlr = new List<tbl_loan_request>();
            List<tbl_loan_repayments> objloans = new List<tbl_loan_repayments>();

            //Get loan request data
            var loan_amount = _context.tbl_loan_request.Where(a => a.is_closed == 0 && a.is_deleted == 0 && a.is_final_approval == 1 && a.req_emp_id == EmpID_ && a.start_date <= start_date_)
                .Select(a => new { a.monthly_emi, a.loan_amt, a.interest_rate, a.loan_req_id, a.req_emp_id, a.is_deleted, a.is_final_approval, a.is_closed, a.start_date, }).ToList();

            // Loop for save the date in loan repayment table
            double interest_component = 0, monthly_rate_of_intererst = 0, totalPrinciple = 0, PrincipalComponent = 0, balanceComponent = 0;
            for (int Index = 0; Index < loan_amount.Count; Index++)
            {
                if (_context.tbl_loan_repayments.Where(a => a.loan_id == loan_amount[Index].loan_req_id && a.is_deleted == 0 && a.status == 0 && a.date >= start_date_).Count() == 0)
                {
                    monthly_rate_of_intererst = Convert.ToDouble(loan_amount[Index].interest_rate) / 12.0 / 100.0;
                    totalPrinciple = _context.tbl_loan_repayments.Where(a => a.loan_id == loan_amount[Index].loan_req_id && a.is_deleted == 0).Sum(p => p.principal_amount);
                    interest_component = Convert.ToInt32((Convert.ToDouble(loan_amount[Index].loan_amt) - totalPrinciple) * monthly_rate_of_intererst);
                    PrincipalComponent = loan_amount[Index].monthly_emi - interest_component;
                    balanceComponent = Convert.ToInt32(Convert.ToDouble(loan_amount[Index].loan_amt) - (totalPrinciple + PrincipalComponent));
                    tbl_loan_repayments objloan = new tbl_loan_repayments()
                    {
                        interest_component = interest_component,
                        principal_amount = PrincipalComponent,
                        loan_balance = balanceComponent,
                        req_emp_id = EmpID_,
                        loan_id = loan_amount[Index].loan_req_id,
                        date = start_date_,
                        status = 0,
                        is_deleted = 0,
                        created_date = DateTime.Now
                    };

                    //Check here overall laon amount paid or not 
                    if (Math.Abs(Convert.ToDouble(loan_amount[Index].loan_amt) - totalPrinciple) < 5)
                    {
                        var loan_req = _context.tbl_loan_request.Where(a => a.req_emp_id == EmpID_ && a.loan_req_id == loan_amount[Index].loan_req_id).FirstOrDefault();
                        loan_req.is_closed = 1;
                        loan_req.last_modified_date = DateTime.Now;
                        tlr.Add(loan_req);
                    }
                    objloans.Add(objloan);
                }
            }
            _context.tbl_loan_request.AttachRange(tlr);
            //_context.Entry(tlr).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.tbl_loan_repayments.AddRange(objloans);
            _context.SaveChanges();

        }

        public void insert_into_tables_fnf()
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {

                    delete_exiting_data_fnf();
                    //freze_attendance();

                    clsComponentValues ob = new clsComponentValues(_company_id, context: _context, EmpID: EmpID_, PayrollMonthyear: PayrollMonthyear_, ComponentId: 0, SGID: SGID_, ComponentValue: "", config: _config, sepId: _SepId);
                    List<mdlSalaryInputValues> mdlSalaryInputValues_ = ob.CalculateComponentValues(true);
                    //insert into salary input            
                    List<tbl_salary_input> lstSI = mdlSalaryInputValues_.Select(p => new tbl_salary_input { monthyear = PayrollMonthyear_, emp_id = EmpID_, component_id = p.compId, values = p.compValue, created_by = userId_, created_dt = currentdateTime_, modified_by = userId_, modified_dt = currentdateTime_, is_active = 1, company_id = _company_id, rate = p.rate, arrear_value = p.arrear_value, current_month_value = p.current_month_value, component_type = p.component_type, is_fnf_comp = 1 }).ToList();
                    _context.tbl_salary_input.AddRange(lstSI);
                    _context.SaveChanges();

                    //payslip_generator();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

        }
        public void insert_into_tables()
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    delete_exiting_data();
                    freze_attendance();
                    clsLossPay obj = new clsLossPay(_context);
                    obj.insert_into_loss_of_pay_schedular(EmpID_, _company_id, PayrollMonthyear_, 0, 30);

                    insert_into_loan();
                    clsComponentValues ob = new clsComponentValues(_company_id, context: _context, EmpID: EmpID_, PayrollMonthyear: PayrollMonthyear_, ComponentId: 0, SGID: SGID_, ComponentValue: "", config: _config);
                    List<mdlSalaryInputValues> mdlSalaryInputValues_ = ob.CalculateComponentValues(true);
                    //insert into salary input            
                    _context.tbl_salary_input.AddRange(mdlSalaryInputValues_.Select(p => new tbl_salary_input { monthyear = PayrollMonthyear_, emp_id = EmpID_, component_id = p.compId, values = p.compValue, created_by = userId_, created_dt = currentdateTime_, modified_by = userId_, modified_dt = currentdateTime_, is_active = 1, company_id = _company_id, rate = p.rate, arrear_value = p.arrear_value, current_month_value = p.current_month_value, component_type = p.component_type,is_fnf_comp=0 }).ToList());
                    _context.SaveChanges();

                    payslip_generator();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

        }

        public void payslip_generator()
        {
            //clsSystemKeysfunction cls_payroll = new clsSystemKeysfunction(_context, EmpID_, PayrollMonthyear_);



            //string payroll_date = DateTime.Now.ToString("dd-MMM-yyyy");
            //string emp_name = cls_payroll.func_emp_name();
            //string emp_email_id = cls_payroll.func_emp_official_email_id();
            //string employee_code = cls_payroll.fncEMPID_sys();
            //string emp_grade = cls_payroll.func_grade();
            //string dateofjoining = cls_payroll.fncEmpJoiningDt_sys();
            //string dept_name = cls_payroll.func_dept();
            //string designation_name = cls_payroll.func_designationid();
            //int days_in_month = DateTime.DaysInMonth(Convert.ToInt32(PayrollMonthyear_.ToString().Substring(0, 4)), Convert.ToInt32(PayrollMonthyear_.ToString().Substring(4, 2)));
            //double prsnt_count_in_mnth = cls_payroll.func_prsnt_count_in_mnth();
            //double absnt_count_in_mnth = cls_payroll.func_absnt_count_in_mnth();
            //string emp_pan_number = cls_payroll.func_Pan_Number();

            //int company_id = _context.tbl_user_master.Where(a => a.employee_id == EmpID_ && a.is_active == 1).Select(b => b.default_company_id).FirstOrDefault();

            //var company_info = _context.tbl_company_master.Where(a => a.company_id == company_id && a.is_active == 1).Select(a => new { a.company_name, a.company_logo }).FirstOrDefault();

            //int compo_id = _context.tbl_component_master.Where(a => a.component_name == "@Salary_sys" && a.is_active == 1).Select(b => b.component_id).FirstOrDefault();

            //string employee_monthly_slary = _context.tbl_salary_input.Where(a => a.monthyear == PayrollMonthyear_ && a.is_active == 1 && a.emp_id == EmpID_ && a.component_id == compo_id).Select(b => b.values).FirstOrDefault();



            //string path = _hostingEnvironment.WebRootPath + "/" + company_info.company_logo;
            //byte[] byt = System.IO.File.ReadAllBytes(path);
            //var company_logo = "data:image/png;base64," + Convert.ToBase64String(byt);

            ////var webRoot = _hostingEnvironment.WebRootPath;

            ////if (!Directory.Exists(webRoot + "/Payslips/" + PayrollMonthyear_ + "/" + employee_code + "/"))
            ////{
            ////    Directory.CreateDirectory(webRoot + "/Payslips/" + PayrollMonthyear_ + "/" + employee_code + "/");
            ////}

            ////var globalSettings = new GlobalSettings
            ////{
            ////    DocumentTitle = "Payslip - " + payroll_date,
            ////    Out = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/Payslips/" + PayrollMonthyear_ + "/" + employee_code + "/" + emp_name + ".pdf"),
            ////};



            //var obj_incom_comp = _context.tbl_salary_input.Where(a => a.is_active == 1 && a.monthyear == PayrollMonthyear_ && a.emp_id == EmpID_ && a.tcm.is_payslip == 1 && a.values != "0").Select(b => new { component_value = b.values, component_name = b.tcm.property_details }).ToList();


            //List<IncomeComponent> obj_incom_comp_ = new List<IncomeComponent>();
            //for (int Index = 0; Index < obj_incom_comp.Count; Index++)
            //{
            //    IncomeComponent obj_inc = new IncomeComponent();

            //    obj_inc.component_name = obj_incom_comp[Index].component_name;
            //    obj_inc.component_value = obj_incom_comp[Index].component_value;

            //    obj_incom_comp_.Add(obj_inc);

            //}

            ////Get Salary Component here
            ////var get_salary_component = _context.tbl_component_master.Where(a => a.is_payslip == 1 && a.is_active == 1).Select(b => new { b.property_details, b.component_id, b.component_type });



            ////List<DeductionComponent> obj_deduc_comp = new List<DeductionComponent>();

            ////for (int Index = 0; Index < data_of_salary_input.Count; Index++)
            ////{

            ////    IncomeComponent obj_inc = new IncomeComponent();
            ////    DeductionComponent obj_deduc = new DeductionComponent();


            ////    // Check here for income component
            ////    var sal_comp_inc = get_salary_component.Where(a => a.component_type == 1).Select(a => a.component_id).ToList();
            ////    if (sal_comp_inc.Contains(Convert.ToInt32(data_of_salary_input[Index].component_id)))
            ////    {
            ////        obj_inc.component_name = get_salary_component.Where(a => a.component_id == data_of_salary_input[Index].component_id).Select(b => b.property_details).FirstOrDefault();
            ////        obj_inc.component_value = data_of_salary_input[Index].values;

            ////        obj_incom_comp.Add(obj_inc);
            ////    }


            ////    //Check here for Deduction component
            ////    var sal_comp_dud = get_salary_component.Where(a => a.component_type == 2).Select(b => b.component_id).ToList();
            ////    if (sal_comp_dud.Contains(Convert.ToInt32(data_of_salary_input[Index].component_id)))
            ////    {
            ////        obj_deduc.component_name = get_salary_component.Where(a => a.component_id == data_of_salary_input[Index].component_id).Select(b => b.property_details).FirstOrDefault();
            ////        obj_deduc.component_value = data_of_salary_input[Index].values;

            ////        obj_deduc_comp.Add(obj_deduc);
            ////    }
            ////}


            //var objectSettings = new ObjectSettings
            //{
            //    HtmlContent = PayslipGenerator.GetHTMLString(company_info.company_name, payroll_date, emp_name, emp_email_id, employee_code, emp_grade, dateofjoining, dept_name, designation_name, days_in_month, prsnt_count_in_mnth, absnt_count_in_mnth, emp_pan_number, employee_monthly_slary, obj_incom_comp_, company_logo),
            //};

            ////var pdf = new HtmlToPdfDocument()
            ////{
            ////    //GlobalSettings = globalSettings,
            ////    Objects = { objectSettings }
            ////};

            /////_converter.Convert(pdf);

            ////string payslip_file_path = "Payslips/" + PayrollMonthyear_ + "/" + employee_code + "/" + emp_name + ".pdf";


            var obj_payroll_status = _context.tbl_payroll_process_status.Where(a => a.emp_id == EmpID_ && a.is_deleted == 0 && a.payroll_month_year == PayrollMonthyear_).FirstOrDefault();
            obj_payroll_status.is_calculated = 1;
            obj_payroll_status.payslip_path = "";

            _context.tbl_payroll_process_status.Add(obj_payroll_status).State = EntityState.Modified;
            _context.SaveChangesAsync();

        }



    }
}


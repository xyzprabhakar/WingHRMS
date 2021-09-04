using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using projContext;
using projContext.DB;

namespace projAPI.Classes
{
    public class clsLossPay
    {
        private class mdlDayStatus
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

        private readonly Context _context;
        public clsLossPay(Context context)
        {
            _context = context;
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


        public void insert_into_loss_of_pay_schedular(int empid, int companyid, int PayrollMonthyear, int Startingday_Setting, int totaldays = 30)
        {

            tbl_lossofpay_master tbl_lossofpay_masters = _context.tbl_lossofpay_master.Where(p => p.emp_id == empid && p.monthyear == PayrollMonthyear
                && p.is_active == 1).FirstOrDefault();

            if (Startingday_Setting == 0)
            {
                Startingday_Setting = 1;
                var tbl_payroll_month_setting = _context.tbl_payroll_month_setting.FirstOrDefault(p => p.company_id == companyid && p.is_deleted == 0);
                if (tbl_payroll_month_setting != null)
                {
                    Startingday_Setting = tbl_payroll_month_setting.from_date;
                }
            }

            if ((tbl_lossofpay_masters == null) || (tbl_lossofpay_masters != null && tbl_lossofpay_masters.is_freezed == 0))
            {
                bool isUpdate = true;
                double no_of_loss_day = 0, no_of_present_day = 0, no_of_absent_day = 0, no_of_leave_day = 0, no_of_holiday_day = 0, Actual_Paid_days = 0, no_of_weekOff_day = 0, Additional_Paid_days = 0, Total_Paid_days = 0;
                DateTime Attendancestartdate, Attendanceenddate;
                var temp_start_date = Convert.ToDateTime(PayrollMonthyear.ToString().Substring(0, 4) + "-" + PayrollMonthyear.ToString().Substring(4, 2) + "-01");

                if (Startingday_Setting == 1)
                {
                    Attendancestartdate = temp_start_date;
                    Attendanceenddate = Attendancestartdate.AddMonths(1).AddDays(-1);
                }
                else
                {
                    Attendancestartdate = temp_start_date.AddMonths(-1).AddDays(Startingday_Setting - 1);
                    Attendanceenddate = Attendancestartdate.AddMonths(1).AddDays(-1);

                }

                // Get the Joining date of Employee
                var tbl_emp_officaial_sec = _context.tbl_emp_officaial_sec.FirstOrDefault(p => p.employee_id == empid && p.is_deleted == 0);
                var actualdaystatus = _context.tbl_daily_attendance.Where(p => p.emp_id == empid && p.attendance_dt >= Attendanceenddate && p
                 .attendance_dt <= Attendanceenddate).Select(p => new { p.is_holiday, p.is_weekly_off, p.attendance_dt, p.day_status, leave_type = (p.tlr == null ? 0 : p.tlr.tbl_leave_info.leave_type) }).ToList();

                var dayStatus = GetDayStatus(Attendancestartdate, Attendanceenddate);
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
                Actual_Paid_days = dayStatus.Sum(p => p.totalPaidValue);



                if (tbl_lossofpay_masters == null)
                {
                    isUpdate = false;
                    tbl_lossofpay_masters = new tbl_lossofpay_master();
                    tbl_lossofpay_masters.final_lop_days = Convert.ToDecimal(no_of_loss_day);
                    tbl_lossofpay_masters.Total_Paid_days = Convert.ToDecimal(Actual_Paid_days);
                }

                var tlp = _context.tbl_lossofpay_setting.FirstOrDefault(p => p.companyid == companyid);
                if (tlp == null || tlp.lop_setting_name == 1)
                {
                    totaldays = Attendanceenddate.Subtract(Attendancestartdate).Days + 1;
                }
                else
                {
                    //Get Days in Month
                    string year = PayrollMonthyear.ToString().Substring(0, 4);
                    string month = PayrollMonthyear.ToString().Substring(4, 2);
                    totaldays = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
                }

                tbl_lossofpay_masters.totaldays = totaldays;
                tbl_lossofpay_masters.acutual_lop_days = Convert.ToDecimal(no_of_loss_day);
                tbl_lossofpay_masters.emp_id = empid;
                tbl_lossofpay_masters.monthyear = PayrollMonthyear;
                tbl_lossofpay_masters.is_active = 1;
                tbl_lossofpay_masters.modified_by = 0;
                tbl_lossofpay_masters.modified_date = new DateTime(2000, 1, 1);
                tbl_lossofpay_masters.created_by = 1451;
                tbl_lossofpay_masters.created_date = DateTime.Now;
                tbl_lossofpay_masters.company_id = companyid;

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
                tbl_lossofpay_masters.Additional_Paid_days = Convert.ToDecimal(0);
                tbl_lossofpay_masters.Actual_Paid_days = Convert.ToDecimal(Actual_Paid_days);
                tbl_lossofpay_masters.Total_Paid_days = Convert.ToDecimal(Actual_Paid_days);
                tbl_lossofpay_masters.is_freezed = 0;

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
        }

        public Response_Msg Update_LOP_Master(tbl_lossofpay_master objtable)
        {
            Response_Msg objresponse = new Response_Msg();

            try
            {
                var exist_data = _context.tbl_lossofpay_master.Where(a => a.lop_master_id == objtable.lop_master_id).FirstOrDefault();
                if (exist_data != null)
                {
                    exist_data.monthyear = objtable.monthyear;
                    exist_data.acutual_lop_days = objtable.acutual_lop_days;
                    exist_data.final_lop_days = objtable.final_lop_days;
                    exist_data.Holiday_days = objtable.Holiday_days;
                    exist_data.Week_off_days = objtable.Week_off_days;
                    exist_data.Present_days = objtable.Present_days;
                    exist_data.Absent_days = objtable.Absent_days;
                    exist_data.Leave_days = objtable.Leave_days;
                    exist_data.Actual_Paid_days = objtable.Actual_Paid_days;
                    exist_data.Additional_Paid_days = objtable.Additional_Paid_days;
                    exist_data.Total_Paid_days = objtable.Total_Paid_days;
                    exist_data.totaldays = objtable.totaldays;
                    exist_data.remarks = objtable.remarks;
                    exist_data.company_id = objtable.company_id;
                    exist_data.emp_id = objtable.emp_id;
                    exist_data.is_freezed = 1;
                    exist_data.modified_date = DateTime.Now;

                    _context.Entry(exist_data).State = EntityState.Modified;
                    _context.SaveChanges();
                    objresponse.StatusCode = 0;
                }
                else
                {

                    var exist = _context.tbl_lossofpay_master.Where(a => a.emp_id == objtable.emp_id && a.monthyear == objtable.monthyear).FirstOrDefault();
                    if (exist != null)
                    {
                        exist.monthyear = objtable.monthyear;
                        exist.company_id = objtable.company_id;
                        exist.acutual_lop_days = objtable.acutual_lop_days;
                        exist.final_lop_days = objtable.final_lop_days;
                        exist.Holiday_days = objtable.Holiday_days;
                        exist.Week_off_days = objtable.Week_off_days;
                        exist.Present_days = objtable.Present_days;
                        exist.Absent_days = objtable.Absent_days;
                        exist.Leave_days = objtable.Leave_days;
                        exist.Total_Paid_days = (objtable.Actual_Paid_days + objtable.Additional_Paid_days);
                        exist.Actual_Paid_days = objtable.Actual_Paid_days;
                        exist.Additional_Paid_days = objtable.Additional_Paid_days;
                        exist.totaldays = objtable.totaldays;
                        exist.emp_id = objtable.emp_id;
                        exist.fkid_sepration = objtable.fkid_sepration;
                        exist.modified_date = DateTime.Now;

                        _context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    else
                    {
                        tbl_lossofpay_master objlop = new tbl_lossofpay_master();
                        objlop.monthyear = objtable.monthyear;
                        objlop.company_id = objtable.company_id;
                        objlop.acutual_lop_days = objtable.acutual_lop_days;
                        objlop.final_lop_days = objtable.final_lop_days;
                        objlop.Holiday_days = objtable.Holiday_days;
                        objlop.Week_off_days = objtable.Week_off_days;
                        objlop.Present_days = objtable.Present_days;
                        objlop.Absent_days = objtable.Absent_days;
                        objlop.Leave_days = objtable.Leave_days;
                        objlop.Total_Paid_days = (objtable.Actual_Paid_days + objtable.Additional_Paid_days);
                        objlop.Actual_Paid_days = objtable.Actual_Paid_days;
                        objlop.Additional_Paid_days = objtable.Additional_Paid_days;
                        objlop.totaldays = objtable.totaldays;
                        objlop.emp_id = objtable.emp_id;
                        objlop.fkid_sepration = objtable.fkid_sepration;
                        objlop.created_date = DateTime.Now;
                        objlop.modified_date = DateTime.Now;
                        _context.Entry(objlop).State = EntityState.Added;
                        _context.SaveChanges();
                    }

                }

                return objresponse;
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return objresponse;
            }

        }




    }
}

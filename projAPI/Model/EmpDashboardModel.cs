using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model
{
    public class EmpDashboardModel
    {
    }

    public class PayrollCalenderData
    {
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public string AttendanceStatus { get; set; }
        public string Events { get; set; }
        public string ColorCode { get; set; }
        public int AttStatus { get; set; }
        public int IsWeekOff { get; set; }
        public DateTime? WeekOffDate { get; set; }
        public int IsHoliday { get; set; }
        public DateTime? HolidayDate { get; set; }
        public string HolidayName { get; set; }
        public int is_comp_off { get; set; }
        public int is_outdoor { get; set; }
        public int leave_request_id { get; set; }
        public int is_outdoor_aprvd { get; set; }
        public int is_leave_aprvd { get; set; }
        public int is_shl { get; set; }
        public int is_comp_off_apprvd { get; set; }
    }

    //set count of given parameters for selected calender month
    public class AttendanceSummaryData
    {
        public decimal Present { get; set; }
        public decimal Absent { get; set; }
        public decimal LateInEarliOut { get; set; }
        public decimal Leave { get; set; }
        public decimal Outdoor { get; set; }
        public decimal Compoff { get; set; }
        public decimal WeeklyOff { get; set; }
        public decimal Holidays { get; set; }
        public decimal HalfDay { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    //set count
    public class BalanceLeaveData
    {
        public int LeaveTypeId { get; set; }
        public string LeaveInfoName { get; set; }
        public double LeaveBalance { get; set; }
        public double TotalCredit { get; set; }
        public double TotalDebit { get; set; }
    }
    //get data
    public class SelectedDateData
    {
        public string ShiftName { get; set; }
        public string ActualTimeIn { get; set; }
        public string ActualTimeOut { get; set; }
        public string ActualToatlShiftHrs { get; set; }
        public string ActualAttendanceStatus { get; set; }
        public List<leavedetails_per_day> leave_d_per_day_list { get; set; }
        public List<outdoor_details_per_day> outdoor_details_list { get; set; }
    }
    public class leavedetails_per_day
    {
        public int leave_request_id { get; set; }
        public string leavetype { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string partialdays { get; set; }
        public string reason { get; set; }
        public string attendance_status { get; set; }
        public string duration { get; set; }
       
    }
    public class outdoor_details_per_day
    {
        public int outdoor_request_id { get; set; }
        public string outdoor_date { get; set; }
        public string in_time { get; set; }
        public string out_time { get; set; }
        public string user_reason { get; set; }
        public string outdoor_status { get; set; }
        public string aprv_remarks { get; set; }
    }
    //get data
    public class CurrentShiftData
    {
        public int ShiftDetailId { get; set; }
        public string CurrentShiftName { get; set; }
        public string InTime { get; set; }
        public string GraceInTime { get; set; }
        public string OutTime { get; set; }
        public string GraceOutTime { get; set; }
        public string TotalShiftHrs { get; set; }
        public string AttendanceStatus { get; set; }

        public DateTime shiftt_dt { get; set; }
    }
    public class HolidayData
    {
        public DateTime? HolidayDate { get; set; }
        public int HolidayId { get; set; }
        public string HolidayName { get; set; }
        public byte IsHoliday { get; set; }
        public byte IsWeekOff { get; set; }
    }
    public class ShiftWeekOffData
    {        
        public DateTime? AttendanceDate { get; set; }
        public int ShiftId { get; set; }
        public byte IsWeekOff { get; set; }
    }
    public class DailyAttenStatusCount
    {
        public int DayStatus { get; set; }
        public int TotalCount { get; set; }
        public string StatusName { get; set; }
    }


    public class ShiftData
    {
        public List<CurrentShiftData> currentshift { get; set; }

        public List<CurrentShiftData> tomorrowshift { get; set; }

       
    }
}

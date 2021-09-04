using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model
{
    public class Reports
    {
        public int company_id { get; set; }
        public int location_id { get; set; }
        public int department_id { get; set; }
        public int Shift_id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double No_of_days { get; set; }
        public int state_id { get; set; }
    }

    public class PresentReportData
    {
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public string AttendanceStatus { get; set; }
        public string Events { get; set; }
        public string ColorCode { get; set; }
        public string employee_first_name { get; set; }
        public string employee_middle_name { get; set; }
        public string employee_last_name { get; set; }
        public DateTime? shift_in_time { get; set; }
        public DateTime? shift_out_time { get; set; }
        public int working_hour_done { get; set; }
        public int working_minute_done { get; set; }        
        public string EmployeeCode { get; set; }
        public string CardNo { get; set; }
        public int AttStatus { get; set; }
        public string ShiftName { get; set; }
        public string location_name { get; set; }
        public string sub_location_name { get; set; }
        public string working_hrs { get; set; }
        public int sno { get; set; }

        public int emp_id { get; set; }
        public int company_id { get; set; }

    }

}

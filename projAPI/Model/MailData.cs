using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model
{
    public class MailData
    {
        public string MailSubject { get; set; }
        public string HtmlBody { get; set; }
        public List<string> MailSendTo = new List<string>();
    }

    public class MailDetails
    {
        public string email_id { get; set; }
        public string employee_first_name { get; set; }
        public string employee_last_name { get; set; }
        public string employee_code { get; set; }
        public string absent_date { get; set; }
        public string day_status { get; set; }

        public DateTime punch_in { get; set; }
        public DateTime punch_out { get; set; }
        public string shift_name { get; set; }

        public DateTime effective_dt { get; set; }
        public int emp_id { get; set; }
    }

    public class SeprationMailDetails
    {
        public int req_id { get; set; }
        public DateTime resign_dt { get; set; }
        public DateTime final_relieve_dt { get; set; }

        public DateTime policy_relieve_dt { get; set; }

        public int req_notice_days { get; set; }

        public int diff_notice_days { get; set; }
        public string emp_name_code { get; set; }

        public int emp_id { get; set; }
        public int? is_final_approve { get; set; }

        public DateTime created_dt { get; set; }

        public int is_cancel { get; set; }

        public DateTime cncl_dt { get; set; }

        public string cncl_remarks { get; set; }
        public int is_deleted { get; set; }

    }

    public class ApplicatonMailDetails
    {
        public int emp_id { get; set; }
        public string emp_name_code { get; set; }
        public int req_id { get; set; }

        public DateTime from_date { get; set; }

        public int is_approve { get; set; }
        public int? a_e_id { get; set; }

        public string approver_remarks { get; set; }

        public DateTime mannual_in_time { get; set; }

        public DateTime mannual_out_time { get; set; }
        public DateTime to_date { get; set; }

        public DateTime system_in_time { get; set; }

        public DateTime system_out_time { get; set; }

        public string leave_type { get; set; }

        public int leave_type_id { get; set; }
    }

}
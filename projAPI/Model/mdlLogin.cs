using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model
{
    public class mdlcommonReturn
    {
        public int? val1 { get; set; }
        public string val2 { get; set; }
    }

    public class mdlLogin
    {
        public int user_id { get; set; }
        //[Required]
        public string user_name { get; set; }
        //[Required]
        public string password { get; set; }
        public int emp_id { get; set; }
        public string emp_name { get; set; }
        public string Email { get; set; }
        public string tokken { get; set; }
        public int default_company { get; set; }
        public int?[] company_list { get; set; }

        public string employee_first_name { get; set; }
        public string employee_middle_name { get; set; }
        public string employee_last_name { get; set; }
        public string employee_photo_path { get; set; }
        public string attempt_count { get; set; }
        public string company_name { get; set; }
        public string company_logo { get; set; }
        //[StringLength(4)]
        //[Required]
        public string CaptchaCode { get; set; }
        //[Required]
        public string CcCode { get; set; }
        public byte fromexe { get; set; }

        public int isgauth { get; set; }


    }

    public class mdlLoginOutput
    {
        public string Token { get; set; }
        public byte StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public int emp_id { get; set; }
        public string user_name { get; set; }
        public int user_id { get; set; }
        public int user_Dep_id { get; set; }
        public string user_Dep_name { get; set; }
        public int default_company { get; set; }
        public int?[] company_list { get; set; }
        public int version_android { get; set; }
        public int version_ios { get; set; }
        public byte is_hod { get; set; } //0 for normal user ,1 for HOD , 2 for Super Admin
        public byte is_reporting_manager { get; set; }
        public string employee_first_name { get; set; }
        public string employee_middle_name { get; set; }
        public string employee_last_name { get; set; }
        public List<int> employee_role_id { get; set; }
        public List<int> manager_emp_list { get; set; }
        public List<int> emp_claim_id { get; set; }
        public int wrong_attempt { get; set; }
        public string employee_photo_path { get; set; }
        public string dashboardd_role_menu_data { get; set; }
        public string _appSetting_domainn { get; set; }

        public Dictionary<string, string> app_setting_dic { get; set; }
        public string app_setting { get; set; }

        public string company_name { get; set; }

        public string company_logo { get; set; }

        public List<object> emp_company_lst { get; set; }

        public List<EmployeeList> _under_emp_lst { get; set; }

        public List<TreeViewNode> menu_lst { get; set; }

        public byte _firsttimelogin { get; set; } //1 Yes,0 No
        public int is_mobile_access { get; set; } //1 Yes,0 No
        public int is_mobile_attendence_access { get; set; } //1 Yes,0 No
    }

    public class mdlSaveAttendance
    {
        public string card_number { get; set; }
        public DateTime punch_time { get; set; }
        public int machine_id { get; set; }

        public string emp_code { get; set; }

        public int company_idd { get; set; }


    }


    public class mdUserProfile
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string name { get; set; }
        public string card_number { get; set; }
        public string employee_code { get; set; }
        public string address { get; set; }
        public string department { get; set; }
        public string designation { get; set; }
        public string phone_no { get; set; }
        public string emial_id { get; set; }
        public int is_hod { get; set; } // 0 Not HOD, 1 HOD
        public string soft_version_ { get; set; }
        public string token_msg { get; set; }
        public string token_status { get; set; }
        public string company_name { get; set; }
        public int default_company { get; set; }
    }

    public class mdlSaveAttendanceList
    {
        public List<mdlSaveAttendance> duplicatedtl { get; set; }
        public List<mdlSaveAttendance> missingdtl { get; set; }

        public List<mdlSaveAttendance> adddblist { get; set; }

        public string DtlMessage { get; set; }
    }


    //Tree Component Model
    public class TreeViewNode
    {
        public int id { get; set; } // menu_id(id)
        public string text { get; set; } //menu_name(text)
        public int parent_id { get; set; }
        public string Urll { get; set; }
        public string icon_url { get; set; }
        public int sortingorder { get; set; }
        public bool state { get; set; }
        public List<TreeViewNode> children { get; set; }
        public byte type { get; set; }
    }

    public class EmployeeList
    {
        public int _empid { get; set; }

        public string emp_code { get; set; }

        public string emp_name { get; set; }
        public int emp_status { get; set; }
        public string emp_name_code { get; set; }
        public string dept_name { get; set; }
        public int dept_id { get; set; }

        public string location_name { get; set; }
        public int location_id { get; set; }

        public int company_id { get; set; }

        public string company_name { get; set; }

        public int state_id { get; set; }

        public string state_name { get; set; }
        public string mobileno { get; set; }
        public string email { get; set; }
        public string desig_name { get; set; }
        public string emp_img { get; set; }
        public string dob { get; set; }
        public string doanv { get; set; }
        public string official_contact_no { get; set; }
        public string Official_email_id { get; set; }



    }
}

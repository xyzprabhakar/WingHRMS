using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model
{
    public class LeaveInfo_ApplicabilityModel
    {

        public string leave_info_id { get; set; }  // primary key  must be public!
        public string leave_type { get; set; } // 1 for Paid, 2 Unpaid
        public string leave_tenure_from_date { get; set; } //
        public string leave_tenure_to_date { get; set; }
        public string leave_type_id { get; set; }
        public byte is_active { get; set; }
        //leave applicability
        public string leave_applicablity_id { get; set; }
        public string is_aplicable_on_all_company { get; set; }
        public string is_aplicable_on_all_location { get; set; }
        public string is_aplicable_on_all_department { get; set; }
        public string is_aplicable_on_all_religion { get; set; }
        public string leave_applicable_for { get; set; } // 1 for Full day , 2 for Half day , 3 for hours and minutes (HH:MM)
        public string day_part { get; set; } // 1 For First Half, 2 Second half 
        public string leave_applicable_in_hours_and_minutes { get; set; } // If leave applicable in hours and minutes (HH:MM)
        public string employee_can_apply { get; set; } // 1 for yes, 2 for No
        public string admin_can_apply { get; set; } // 1 for yes, 2 for No
        public string is_aplicable_on_all_emp_type { get; set; }

        public string is_deleted { get; set; }

        //tbl_leave_app_on_emp_type
        public string sno { get; set; }
        public string l_app_id { get; set; }
        public string employment_type { get; set; } //1 temporary,2 Probation,3 Confirmend, 4 Contract, 10 notice,99 FNF,(100 terminate     no entry coreposnding to 100   )

        //tbl_leave_appcbl_on_company
        public string l_a_id { get; set; }
        public string c_id { get; set; }
        public string location_id { get; set; }
        //tbl_leave_app_on_dept
        public string lid { get; set; }
        public string id { get; set; }

        //tbl_leave_appcbl_on_religion
        public string religion_id { get; set; }

        //leave credit
        public string leave_credit_id { get; set; }
        public string frequency_type { get; set; }//1-monthly,2 Quaterly,3 Half yealry,4 yearly
        public decimal leave_credit_day { get; set; }
        public int leave_credit_number { get; set; }//how much leave going to credit for frequency
        public string is_half_day_applicable { get; set; }
        public string is_applicable_for_advance { get; set; }
        public string advance_applicable_day { get; set; }
        public string is_leave_accrue { get; set; }
        public string max_accrue { get; set; }//if no Limit then 10000
        public string is_required_certificate { get; set; }
        public string certificate_path { get; set; }

        //leave rule

        public string applicable_if_employee_joined_before_dt { get; set; } //default 1-jan-2300

        public string maximum_leave_clubbed_in_tenure_number_of_leave { get; set; }
        public string maxi_negative_leave_applicable { get; set; }
        public string certificate_require_for_min_no_of_day { get; set; }

        public string minimum_leave_applicable { get; set; } // 1 for	0.5 (half day) , 2  for 1 (full day)
        public string number_maximum_negative_leave_balance { get; set; }

        public string maximum_leave_can_be_taken_type { get; set; } // if 1 for days, 2 for quarter
        public string maximum_leave_can_be_taken { get; set; }
        public string maximum_leave_can_be_in_day { get; set; }
        public string maximum_leave_can_be_taken_in_quater { get; set; }

        public string can_carried_forward { get; set; } // 1 for Yes, 2 for No
        public string maximum_carried_forward { get; set; } // 1 for Yes, 2 for No

        public string applied_sandwich_rule { get; set; }//0 no Rule Applied, 
                                                         //1.	H / WO counted as absent, if employees absent before WO / H*
                                                         //2	H / WO counted as absent, if employees absent after WO / H * (drop down)
                                                         //3  H / WO counted as absent, if employees absent before / after WO / H* (drop down)

        //encashment
        public string is_cashable { get; set; }
        public string cashable_type { get; set; }//0-for all, 1 dor on separation,2 for after n year
        public string cashable_after_year { get; set; }//default 1
        public string maximum_cashable_leave { get; set; }//default is 10000
    }

    public class LeaveApplicationModel
    {
        public int leave_request_id { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public double leave_qty { get; set; }
        public int? leave_type_id { get; set; }
        public int leave_info_id { get; set; }
        public byte leave_applicable_for { get; set; } // 1 for Full day , 2 for Half day , 3 for hours and minutes (HH:MM)
        public byte day_part { get; set; } // 1 For First Half, 2 Second half 
        public string leave_applicable_in_hours_and_minutes { get; set; } // If leave applicable in hours and minutes (HH:MM)


        public int? r_e_id { get; set; }
        public int? a1_e_id { get; set; }
        public int? a2_e_id { get; set; }
        public int? a3_e_id { get; set; }
        public DateTime requester_date { get; set; }
        public DateTime approval_date1 { get; set; }
        public DateTime approval_date2 { get; set; }
        public DateTime approval_date3 { get; set; }
        public byte is_approved1 { get; set; }
        public byte is_approved2 { get; set; }
        public byte is_approved3 { get; set; }
        public byte is_final_approve { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "requester_remarks")]
        public string requester_remarks { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval1_remarks")]
        public string approval1_remarks { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval2_remarks")]
        public string approval2_remarks { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval3_remarks")]
        public string approval3_remarks { get; set; }

        public byte is_deleted { get; set; }
        public int deleted_by { get; set; }
        public DateTime deleted_dt { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "deleted_remarks")]
        public string deleted_remarks { get; set; }
        public int created_by { get; set; }

        public DateTime created_dt { get; set; }

        public int? company_id { get; set; }

    }

    public class cls_comp_off_ledger
    {

        public int sno { get; set; }
        public string compoff_date1 { get; set; }
        public DateTime compoff_date { get; set; }
        public double credit { get; set; }
        public double dredit { get; set; }
        public DateTime transaction_date { get; set; }
        public byte transaction_type { get; set; }//1-on Working off(addition in compoff),2- Consumed ,3- Expired, 4-compoff in cash,5 manual add, 6 maula deduction,7 delete by system
        public int monthyear { get; set; }
        public int transaction_no { get; set; }// comp request no
        public string remarks { get; set; }
        public int? e_id { get; set; }

    }

    public class CompOffDetail
    {

        public List<int> compoff_id { get; set; }

        public int a1_e_id { get; set; }
        public string remarks { get; set; }
        public byte is_deleted { get; set; } //1 true, 0 false
    }

    public class LeaveAppModel
    {
        public List<object> leave_request_id { get; set; }
        public int a1_e_id { get; set; }
        public string approval1_remarks { get; set; }
        public byte is_approved1 { get; set; }
        public byte is_final_approve { get; set; }


        public List<compoff_raise> compoff_raise_id { get; set; }
    }



    public class AllLeaveRequestReport
    {
        public int employee_id { get; set; }
        public string employee_name { get; set; }
        public int leave_request_id { get; set; }
        public DateTime from_date { get; set; }
        public DateTime manual_in_time { get; set; }
        public DateTime manual_out_time { get; set; }
        public string requester_remarks { get; set; }
        public string status { get; set; }
        public string my_status { get; set; }
        public int requester_id { get; set; }
        public DateTime system_in_time { get; set; }
        public DateTime system_out_time { get; set; }
        public int is_final_approve { get; set; }

        public DateTime to_date { get; set; }

        public string leave_type { get; set; }

        public int leave_applicable_for { get; set; }
        public double leave_qty { get; set; }
        public DateTime requester_date { get; set; }

        public int approver_no { get; set; }

        public string approver_remarks { get; set; }

        public double workinghrs { get; set; }

        public TimeSpan diff { get; set; }

        public int my_status_level { get; set; }

        public double diff_time { get; set; }



    }
    public class LeaveLedgerModellCls
    {
        public int leave_type_id { get; set; }
        public string leave_type_name { get; set; }
        public double credit { get; set; }
        public double dredit { get; set; }
        public double balance { get; set; }
        public int e_id { get; set; }

        public string emp_name_code { get; set; }
        public string emp_name { get; set; }
        public string month { get; set; }
        public string current_emptype { get; set; }

        public int company_id { get; set; }

        public byte transaction_type { get; set; }

        public string remarks { get; set; }

        public int created_by { get; set; }

        public int dept { get; set; }

        public string dept_name { get; set; }

        public int location_id { get; set; }

        public string loc_name { get; set; }

        public double leave_encash { get; set; }

        public int req_idd { get; set; }
    }




    public class LeaveLedgerModell
    {
        public int leave_type_id { get; set; }
        public string leave_type_name { get; set; }
        public double credit { get; set; }
        public double dredit { get; set; }
        public double balance { get; set; }
        public int e_id { get; set; }

        public string emp_name_code { get; set; }
        public string emp_name { get; set; }
        public string emp_code { get; set; }

        public string current_emptype { get; set; }

        public int company_id { get; set; }

        public byte transaction_type { get; set; }

        public string remarks { get; set; }

        public int created_by { get; set; }

        public int dept { get; set; }

        public string dept_name { get; set; }

        public int location_id { get; set; }
        public int monthyear { get; set; }

        public string loc_name { get; set; }

        public double leave_encash { get; set; }

        public int req_idd { get; set; }
        public int year { get; set; }
        public string department_name { get; set; }
        public string location_name { get; set; }
        public double openingbalance { get; set; }
        public string transactionmonth { get; set; }
        public int month_numb { get; set; }
    }


    public class LeaveLedgerModellOpeningBalence : LeaveLedgerModell
    {
        public double OpeningBalance { get; set; }
    }

    public class compoff_raise
    {

        public int emp_comp_id { get; set; }

        public DateTime comp_off_date { get; set; }


        public int? emp_id { get; set; }


        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "requester_remarks")]
        public string requester_remarks { get; set; }


        [Range(0, 2)]
        public byte is_approved1 { get; set; }
        [Range(0, 2)]
        public byte is_approved2 { get; set; }
        [Range(0, 2)]
        public byte is_approved3 { get; set; }
        [Range(0, 2)]
        public byte is_final_approve { get; set; }




        public int? a1_e_id { get; set; }


        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval1_remarks")]
        public string approval1_remarks { get; set; }



        public int? a2_e_id { get; set; }


        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval2_remarks")]
        public string approval2_remarks { get; set; }



        public int? a3_e_id { get; set; }



        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval3_remarks")]
        public string approval3_remarks { get; set; }

        public byte is_deleted { get; set; } //1 delete by user,3 delete by admin
        public int deleted_by { get; set; }
        public DateTime deleted_dt { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "deleted_remarks")]
        public string deleted_remarks { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public DateTime approver1_dt { get; set; }

        public DateTime approver2_dt { get; set; }

        public DateTime approver3_dt { get; set; }

        public string req_name_code { get; set; }

    }



    public class LeaveLedgerList
    {

        public List<LeaveLedgerModell> adddbleavelst { get; set; }
        public List<LeaveLedgerModell> duplicateleavelst { get; set; }
        public List<LeaveLedgerModell> missingleavelst { get; set; }

        public string Message_ { get; set; }
    }

    public class LeaveReport
    {
        public List<object> empdtl { get; set; }

        public string from_date { get; set; }

        public string to_date { get; set; }

        public int all_emp { get; set; }

        public byte is_ar_dtl { get; set; } // 0 no its not approve or reject dtl,1 yes it is.
        public int company_id { get; set; }
        public int department_id { get; set; }
        public int location_id { get; set; }
    }


    public class LeaveLedgerMdl
    {
        public int company_id { get; set; }
        public int department_id { get; set; }
        public int location_id { get; set; }
        public List<int> emp_ids { get; set; }
        public int year { get; set; }
    }
    public class LeaveLedgerMonthwise
    {
        public double openingbalance { get; set; }
        public double credit { get; set; }
        public double dredit { get; set; }
        public double balance { get; set; }
        public string emp_name { get; set; }
        public int e_id { get; set; }
        public string emp_code { get; set; }
        public int company_id { get; set; }
        public int leave_type_id { get; set; }
        public string leave_type_name { get; set; }
        public string department_name { get; set; }
        public string location_name { get; set; }
        public int monthyear { get; set; }
        public string transactionmonth { get; set; }
    }
}



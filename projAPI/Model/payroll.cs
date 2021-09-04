using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace projAPI.Model
{
    public class payroll
    {
        public class salarygroupdetails
        {
            public int group_id { get; set; }
            [MaxLength(200)]
            [StringLength(200)]
            [Display(Description = "group_name")]
            [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Salary Group Name")]
            public string group_name { get; set; }
            [MaxLength(500)]
            [StringLength(500)]
            [Display(Description = "description")]
            [RegularExpression(@"^[a-zA-Z0-9'\s'\,'\.]{1,500}$", ErrorMessage = "Invalid Description")]
            public string description { get; set; }

            [Display(Description = "minvalue")]
            [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Salary")]
            public double minvalue { get; set; } // name on html page is salary
            public double maxvalue { get; set; }
            public int grade_Id { get; set; }
            public int created_by { get; set; }
            public DateTime created_dt { get; set; }
            public int modified_by { get; set; }
            public DateTime modified_dt { get; set; }
            public int is_active { get; set; }
            public string[] key_id { get; set; }
        }
    }

    public class Keymasterdetails
    {
        public int key_id { get; set; }
        public string key_name { get; set; }
        public bool ischecked { get; set; }
    }

    //Created By Amarjeet
    public class CreateFormula
    {
        public int component_id { get; set; }
        public string component_name { get; set; }
        public int parentid { get; set; }
        public byte is_system_key { get; set; }
        public string property_details { get; set; }
        public int company_id { get; set; }
        public int salary_group_id { get; set; }
        public int component_type { get; set; } // Here If component type=1 then Income, If component type=2 then Deduction If component type=3 then other If component type=4 then Other Income If component type=5 then Other Deduction 
        public byte is_salary_comp { get; set; }
        public byte is_tds_comp { get; set; }
        public byte is_data_entry_comp { get; set; }
        public int payment_type { get; set; }
        public string formula { get; set; }
        public int created_by { get; set; }
        public int modified_by { get; set; }
        public int is_active { get; set; }
        public int function_calling_order { get; set; }
        public int user_interface { get; set; }
        public byte is_payslip { get; set; }
        public string datatype { get; set; }

    }
    public class GetSalaryCalculate
    {
        public int emp_id { get; set; }
        public string monthyear { get; set; }
        public int sgid { get; set; }
        public int component_id { get; set; }
        public string componentvalue { get; set; }

        public int company_id { get; set; }

    }
    public class SalaryReport
    {
        public int emp_id { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class SalaryRevison
    {
        public List<int> emp_ids { get; set; }
        public int emp_id { get; set; }
        public List<DateTime> applicable_dates { get; set; }
        public DateTime applicable_date { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_active { get; set; } // 0: Inactive , // 1: Active , // 2:Pending for approval
        //[StringLength(50)]
        [Display(Description = "salaryrevision")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Salary")]
        public decimal salaryrevision { get; set; }
        public List<lst_salary> SalaryValue { get; set; }
        public string maker_remark { get; set; }
        public string checker_remark { get; set; }
    }
    public class lst_salary
    {
        public int component_id { get; set; }
        //[StringLength(50)]
        [Display(Description = "componentvalue")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Component Salary")]
        public string componentvalue { get; set; }

    }

    public class md_reimbursement_request
    {
        public int emp_id { get; set; }
        public int request_type { get; set; }
        public int request_month_year { get; set; }
        public int fiscal_year_id { get; set; }
        public double total_request_amount { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_active { get; set; }
        public List<md_reimbursement_request_details> md_reimbursement_request_details { get; set; }

    }
    public class md_reimbursement_request_details
    {
        public int rrm_id { get; set; }
        public int rclm_id { get; set; }
        public double Bill_amount { get; set; }
        public DateTime Bill_date { get; set; }
        public double approved_amount { get; set; }
        public int is_approvred { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public double monthly_limitt { get; set; }
        public double yearly_limitt { get; set; }
    }


    public class clsProcessEmployee
    {
        public List<int> emp_id { get; set; }
        public int company_id { get; set; }
        public int payroll_month_year { get; set; }
        public int? created_by { get; set; }
    }

    public class clsEmployeeListForPayroll
    {
        public int userid { get; set; }
        public int empid { get; set; }
        public string empcode { get; set; }
        public string empname { get; set; }
        public string empdept { get; set; }
        public string empdesig { get; set; }
        public decimal salary { get; set; }
        public bool ischecked { get; set; }
        public string partially_calculated { get; set; }
        public string partially_freezed { get; set; }
        public string is_lock { get; set; }

        public string salary_group { get; set; }


    }
    public class mdlReimbursementGradelimitMaster
    {
        public int rglm_id { get; set; }
        public int? grade_id { get; set; }
        public int? emp_id { get; set; }
        public double monthly_limit { get; set; }
        public double yearly_limit { get; set; }

        public string fiscal_year_id { get; set; }
        public int created_by { get; set; }

        public DateTime created_dt { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_dt { get; set; }

        public int is_active { get; set; }
        public List<mdlReimbursementCategorylimitMaster> mdlrclm { get; set; }
    }
    public class mdlReimbursementCategorylimitMaster
    {
        public int rglm_id { get; set; }
        public int category_id { get; set; }
        public double monthly_limit { get; set; }
        public double yearly_limit { get; set; }
    }

    public class ReimbursementRequestMaster
    {
        public int compId { get; set; }
        public int rclm_id { get; set; }
        public int empId { get; set; }
        public int monthyear { get; set; }
        public int req_type { get; set; }
        public double monthly_limit { get; set; }
        public double yearly_limit { get; set; }
        public string fiscal_year_id { get; set; }
        public double total_amt { get; set; }
        public List<ReimbursementRequestDetails> categoryValue { get; set; }
    }
    public class ReimbursementRequestDetails
    {
        public int rglm_id { get; set; }
        public DateTime billdate { get; set; }
        public double billamt { get; set; }
        public double reqamt { get; set; }
    }


    public class IncomeComponent
    {
        public string component_name { get; set; }
        public string component_value { get; set; }
    }

    public class DeductionComponent
    {
        public string component_name { get; set; }
        public string component_value { get; set; }
    }



    public class Component_property_master
    {
        public int sno { get; set; }
        public int component_id { get; set; }
        public int company_id { get; set; }
        public int salary_group_id { get; set; }
        public int component_type { get; set; } // Here If component type=1 then Income, If component type=2 then Deduction If component type=3 then other If component type=4 then Other Income If component type=5 then Other Deduction 
        public byte is_salary_comp { get; set; }
        public byte is_tds_comp { get; set; }
        public byte is_data_entry_comp { get; set; }
        public int payment_type { get; set; }
        public string formula { get; set; }
        public int function_calling_order { get; set; }
        public int is_user_interface { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_active { get; set; }
        public int is_payslip { get; set; }

        public List<Component_property_master> component_mstr { get; set; }
    }

    //Reimbursment Request Master
    public class Category_Reimbursment_request
    {
        public int compId { get; set; }
        public int emp_id { get; set; }
        public int req_type { get; set; }
        public int monthyear { get; set; }
        public string fiscal_year_id { get; set; }
        public double total_amt { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public int is_active { get; set; }
        public int is_deleted { get; set; }
        public int is_approved { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remarks { get; set; }
        public double total_request_amount { get; set; }
        //Reimbursment request Detail

        public List<md_reimbursement_request_details> reimbursement_request_details { get; set; }
    }

    public class SalaryInput
    {
        public int component_id { get; set; }
        public string component_name { get; set; }
        public string property_details { get; set; }

        [Display(Description = "component_value")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Value")]
        public decimal component_value { get; set; }
        public int parentid { get; set; }
        public byte is_system_key { get; set; }

        public int company_id { get; set; }
        public int salary_group_id { get; set; }
        public int component_type { get; set; } // Here If component type=1 then Income, If component type=2 then Deduction If component type=3 then other If component type=4 then Other Income If component type=5 then Other Deduction 
        public byte is_salary_comp { get; set; }
        public byte is_tds_comp { get; set; }
        public byte is_data_entry_comp { get; set; }
        public int created_by { get; set; }

        public DateTime creadted_dt { get; set; }

        public string emp_code { get; set; }

        public int monthyear { get; set; }

        public int emp_id { get; set; }

    }


    public class SalaryInputList
    {


        public List<SalaryInput> adddbSalaryInputlst { get; set; }

        public List<SalaryInput> duplicatesalaryinputlst { get; set; }
        public List<SalaryInput> missingsalaryinputlst { get; set; }
        public List<SalaryInput> employeenotexist { get; set; }

        public string DtlMessage { get; set; }
    }


    #region ** STARTED BY SUPRIYA ON 19-07-2019

    public class AssetRequest
    {
        public int asset_req_id { get; set; }

        public int company_id { get; set; }

        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "asset_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Asset Name")]
        public string asset_name { get; set; }

        [MaxLength(500)]
        [StringLength(500)]
        [Display(Description = "description")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,500}$", ErrorMessage = "Invalid Description")]
        public string asset_description { get; set; }
        public int? emp_id { get; set; }
        public int? assets_master_id { get; set; }

        public string asset_number { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }

        public string reqt_type { get; set; } // asset type as a string

        public int is_active { get; set; }
        public string emp_name_code { get; set; }
        public byte is_final_approver { get; set; }
        public byte is_approve { get; set; }
        public byte approval_order { get; set; }
        public DateTime created_dt { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

        public byte is_closed { get; set; }
        //[ForeignKey("approval_setting")]
        public int? approver_role_id { get; set; }

        public int approver_type { get; set; }

        public int approver_emp_id { get; set; }

        public string company_name { get; set; }
        public List<AssetRequest> assetreqlist { get; set; }

        public DateTime asset_issue_date { get; set; }
        public byte asset_type { get; set; } // 0 for new Asset Request, 1 for replacement ,2 for submission

        public DateTime submission_date { get; set; }

        public byte is_permanent { get; set; }
    }


    public class assetdetails
    {
        public int asset_id { get; set; }
        public string asset_name { get; set; }

        public string asset_desc { get; set; }

        public DateTime from_date { get; set; }

        public DateTime to_date { get; set; }

        public string assetno { get; set; }

        public int req_type { get; set; }

        public string req_type_name { get; set; }
    }

    #endregion ** END BY SUPIRYA ON 19-07-2019


    public class AssetApprovalHistory
    {
        public string Approved_By { get; set; }
        public string Approvel_Remark { get; set; }
        public string Approvel_Status { get; set; }
        public string Approved_Date { get; set; }
    }

    public class AssetApprovalSave
    {
        public List<AssetRequest> asset_req_dtl { get; set; }
        // public List<object> asset_number { get; set; }
        public int emp_id { get; set; }
        public byte is_approve { get; set; } //0 for pending, 1 for approve,2 for Reject
        public int last_modified_by { get; set; }
        public int approver_role_id { get; set; }
    }


    #region ** STARTED BY SUPRIYA ON 22-07-2019


    public class LoanRequest
    {
        public int sno { get; set; }

        public int loan_req_id { get; set; }

        public int emp_id { get; set; }

        public int req_emp_id { get; set; }
        public byte is_final_approver { get; set; }
        public byte is_approve { get; set; }

        public byte approval_order { get; set; }
        public DateTime created_dt { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

        //[ForeignKey("approver_setting")]
        public int? approver_role_id { get; set; }

        public string emp_code { get; set; }

        public int loan_type { get; set; }

        public decimal loan_amt { get; set; }

        public decimal interest_rate { get; set; }

        public decimal loan_tenure { get; set; }

        public string loan_purpose { get; set; }

        public DateTime start_date { get; set; }

        public int approver_emp_id { get; set; }

        public byte is_closed { get; set; }

        public int approver_type { get; set; }
        public int company_id { get; set; }
    }

    public class fnf_variable
    {
        public int variable_id { get; set; }

        public int req_id { get; set; }
        public int? company_id { get; set; }
        public int? emp_id { get; set; }
        public int? component_id { get; set; }

        public string component_name { get; set; }
        public string component_value { get; set; }
        public int component_type { get; set; }
        public int monthyear { get; set; }

        [Display(Description = "variable_amt")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Amount")]
        public double amt { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remarks { get; set; }

        public byte is_deleted { get; set; }

        public byte is_process { get; set; }

        public DateTime created_dt { get; set; }
        public DateTime modified_dt { get; set; }
    }

    public class fnf_attandance_dtl
    {
        public int req_id { get; set; }
        public int fnf_attend_id { get; set; }
        public int? company_id { get; set; }
        public int? emp_id { get; set; }

        [Display(Description = "monthyear")]
        [RegularExpression(@"^[0-9]{1,6}$", ErrorMessage = "Invalid Month Year")]
        public int monthyear { get; set; }

        public int totaldays { get; set; }

        [Display(Description = "acutual_lop_days")]
        [RegularExpression(@"^[0-9'\.]{1,6}$", ErrorMessage = "Invalid actual LOP Days")]
        public double acutual_lop_days { get; set; }

        [Display(Description = "final_lop_days")]
        [RegularExpression(@"^[0-9'\.]{1,6}$", ErrorMessage = "Invalid final LOP Days")]
        public double final_lop_days { get; set; }
        public double Week_off_days { get; set; }
        public double Holiday_days { get; set; }
        public double Present_days { get; set; }
        public double Absent_days { get; set; }
        public double Leave_days { get; set; }
        public double Actual_Paid_days { get; set; }
        public double Additional_Paid_days { get; set; }
        public string remarks { get; set; }

        [Display(Description = "paid_days")]
        [RegularExpression(@"^[0-9'\.]{1,6}$", ErrorMessage = "Invalid Paid Days")]
        public double Total_Paid_days { get; set; }

        [Display(Description = "paid_amount")]
        [RegularExpression(@"^[0-9'\.]{1,10}$", ErrorMessage = "Invalid Paid amount")]
        public double paid_amount { get; set; }


        public byte is_process { get; set; }

        public byte is_deleted { get; set; }

        public DateTime created_dt { get; set; }
        public DateTime modified_dt { get; set; }
    }
    #endregion ** END BY SUPIRYA ON 22-07-2019

    public class LoanModel
    {
        public int loan_recovery_id { get; set; }
        public int empid { get; set; }

        public string emp_name { get; set; }

        public string emp_code { get; set; }

        public int company_id { get; set; }

        public string company_name { get; set; }

        public int loan_req_id { get; set; }

        public string loan_type { get; set; }
        public decimal loan_amt { get; set; }

        public double monthly_emi { get; set; }

        public decimal credit { get; set; } // Loan amount receive from employee

        public decimal debit { get; set; } // Loan amount pending from employee end

        public decimal remaining_balance { get; set; }

        public decimal recover_amt { get; set; }
    }

    public class mdlSalaryRevision
    {
        public int company_id { get; set; }
        public DateTime _applicabledt { get; set; }

        public decimal gross_salary { get; set; }
        public List<SalaryComponent> _Salcomponent { get; set; }
    }

    public class SalaryComponent
    {
        public int component_id { get; set; }
        public string property_details { get; set; }

        public string applicable_value { get; set; }

        public string compValue { get; set; }

        public byte is_salary_comp { get; set; }

        public int is_active { get; set; }

        public int emp_id { get; set; }
    }

    public class Salary_input_change
    {
        public int salary_input_id { get; set; }
        public string monthyear { get; set; }
        public int? emp_id { get; set; }
        public int? component_id { get; set; }
        public List<int> component_ids { get; set; }
        public string values { get; set; }
        public List<string> component_values { get; set; }
        public List<string> previous_values { get; set; }
        public string previousvalues { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_active { get; set; }
        public int? company_id { get; set; }

    }

    public class mdlDayStatus
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
}




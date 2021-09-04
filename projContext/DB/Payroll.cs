using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB
{

    public class mdlSalaryInputValues
    {
        [Key]
        public int compId { get; set; }
        public string compName { get; set; }
        public string compValue { get; set; }
        public double rate { get; set; }
        public double current_month_value { get; set; }
        public double arrear_value { get; set; }
        public int component_type { get; set; }
    }

    public class EmployeeBasicDataProc
    {
        public int user_id { get; set; }
        [Key]
        public int employee_id { get; set; }
        public string emp_code { get; set; }
        public string emp_name { get; set; }
        public int company_id { get; set; }
        public string company_name { get; set; }
        public string location_name { get; set; }
        public int location_id { get; set; }
        public string dept_name { get; set; }
        public int dept_id { get; set; }
        public int state_id { get; set; }
        public string state_name { get; set; }

        public int emp_status { get; set; }
        public int isActive { get; set; }
        public string mobileno { get; set; }
        public string email { get; set; }
        public string desig_name { get; set; }
        public string emp_img { get; set; }
        public string dob { get; set; }
        public string doanv { get; set; }
    }


    public class tbl_lossofpay_setting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int lop_setting_id { get; set; }
        public int companyid { get; set; }
        public int lop_setting_name { get; set; }
        public int is_active { get; set; }
        public DateTime created_date { get; set; }
        public int created_by { get; set; }
        public DateTime modified_date { get; set; }
        public int modified_by { get; set; }
    }

    public class tbl_lossofpay_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int lop_master_id { get; set; }
        //[ForeignKey("tbl_lossofpay_setting")]
        //public int? lop_setting_id { get; set; }
        public int? emp_id { get; set; }
        [ForeignKey("tbl_lossofpay_setting")]
        public int? company_id { get; set; }
        public int? totaldays { get; set; }
        public decimal? acutual_lop_days { get; set; }
        [Range(1, 30, ErrorMessage = "Final LOP Digit must be between 1 to 30")]
        public decimal? final_lop_days { get; set; }
        public decimal Week_off_days { get; set; }
        public decimal Holiday_days { get; set; }
        public decimal Present_days { get; set; }
        public decimal Absent_days { get; set; }
        public decimal Leave_days { get; set; }
        public decimal Actual_Paid_days { get; set; }
        public decimal Additional_Paid_days { get; set; }
        public decimal Total_Paid_days { get; set; }
        public int is_freezed { get; set; }
        public string remarks { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_date { get; set; }
        public int? monthyear { get; set; }
        public int fkid_sepration { get; set; }

    }

    public class tbl_lop_detail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int lop_detail_id { get; set; }
        [ForeignKey("tbl_lossofpay_master")]
        public int? lop_master_id { get; set; }
        public int companyid { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }


    // Payroll Process Status Lock Previous Payroll
    public class tbl_payroll_process_status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pay_pro_status_id { get; set; }  // primary key  must be public!     

        [ForeignKey("comp_master")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master comp_master { get; set; }

        public int payroll_month_year { get; set; }

        [ForeignKey("tbl_emp")] // Foreign Key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_emp { get; set; }

        public byte is_calculated { get; set; }// 0 UnLock , 1 Lock
        public byte is_freezed { get; set; }// 0 UnLock , 1 Lock
        public byte is_lock { get; set; }// 0 UnLock , 1 Lock
        public byte payroll_status { get; set; } // 0 UnLock , 1 Lock

        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

        public string payslip_path { get; set; }
    }



    public class tbl_key_master

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int key_id { get; set; }
        [StringLength(50)]
        [Display(Description = "key_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Key Name")]
        public string key_name { get; set; }
        [StringLength(50)]
        [Display(Description = "display_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Display Name")]
        public string display_name { get; set; }// display for the print required
        [StringLength(200)]
        [Display(Description = "description")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Description")]
        public string description { get; set; }
        public int type { get; set; } //1-for system key,2 for user defined key
        [StringLength(50)]
        [Display(Description = "calling_function_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Calling Function Name")]
        public string calling_function_name { get; set; } //call the function of a class using reflection that Will get the data from a class coresponding to employee
        public int computation_order { get; set; }// in which formate the key will be computed
        public string defaultvalue { get; set; }
        public string data_type { get; set; } // typeof(variable)
        [StringLength(50)]
        [Display(Description = "forumal_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Formula Name")]
        public string forumal_name { get; set; } // if User defined then
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_active { get; set; }
    }
    public class tbl_salary_group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public double minvalue { get; set; }

        public double maxvalue { get; set; }

        [ForeignKey("tgm")]
        public int? grade_Id { get; set; }
        public tbl_grade_master tgm { get; set; } //reffrence of Grade master table

        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_active { get; set; }
    }
    public class tbl_sg_maping
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int map_id { get; set; }

        [ForeignKey("tsg")]
        public int? salary_group_id { get; set; }
        public tbl_salary_group tsg { get; set; } //reffrence key for tbl_salary_group

        [ForeignKey("tem")]
        public int? emp_id { get; set; }
        public tbl_employee_master tem { get; set; } //reffrence key for tbl_employee_master

        public DateTime applicable_from_dt { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_active { get; set; }

    }
    public class tbl_tax_slab_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tax_slab_id { get; set; }

        public string gender { get; set; }

        public int min_age { get; set; }

        public int max_age { get; set; }

        public int created_by { get; set; }

        public DateTime created_dt { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_dt { get; set; }

        public int is_active { get; set; }

    }
    public class tbl_tax_slab_Details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }

        [ForeignKey("ttsm")]
        public int? tax_slab_id { get; set; }
        public tbl_tax_slab_master ttsm { get; set; } //reffrence key for tbl_slab_master
        public string gender { get; set; }

        public int min_value { get; set; }

        public int max_value { get; set; }

        public decimal TaxPercentage { get; set; }

        public string OtherTaxName { get; set; }

        public decimal OtherTaxPercentage { get; set; }

        public decimal Surcharge_percentage { get; set; }

        public decimal Exemption { get; set; }

        public int created_by { get; set; }

        public DateTime created_dt { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_dt { get; set; }

        public int is_active { get; set; }

    }

    public class tbl_salary_input
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int salary_input_id { get; set; }
        public int monthyear { get; set; }
        [ForeignKey("tem")]
        public int? emp_id { get; set; }
        public tbl_employee_master tem { get; set; } //reffrence key for tbl_emp_master
        [ForeignKey("tcm")]
        public int? component_id { get; set; }
        public tbl_component_master tcm { get; set; } //reffrence key for tbl_component_master
        public string values { get; set; }
        public double rate { get; set; }
        public double current_month_value { get; set; }
        public double arrear_value { get; set; }
        public int component_type { get; set; } // Here If component type=1 then Income, If component type=2 then Deduction If component type=3 then other If component type=4 then Other Income If component type=5 then Other Deduction 
        public int created_by { get; set; }

        public DateTime created_dt { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_dt { get; set; }

        public int is_active { get; set; }
        public int is_fnf_comp { get; set; }

        [ForeignKey("company_master")]
        public int? company_id { get; set; }

        public tbl_company_master company_master { get; set; }

        
    }
    public class tbl_salary_input_change
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int salary_input_id { get; set; }
        public string monthyear { get; set; }
        [ForeignKey("tem")]
        public int? emp_id { get; set; }
        public tbl_employee_master tem { get; set; } //reffrence key for tbl_emp_master
        [ForeignKey("tcm")]
        public int? component_id { get; set; }
        public tbl_component_master tcm { get; set; } //reffrence key for tbl_component_master
        public string values { get; set; }
        public string previousvalues { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_active { get; set; }
        public int is_fnf_comp { get; set; }
        [ForeignKey("company_master")]
        public int? company_id { get; set; }

        public tbl_company_master company_master { get; set; }

    }
    public class log_tbl_salary_input
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int log_id { get; set; }
        public int salary_input_id { get; set; }
        public string monthyear { get; set; }
        [ForeignKey("tem")]
        public int? emp_id { get; set; }
        public tbl_employee_master tem { get; set; } //reffrence key for tbl_emp_master

        [ForeignKey("tcm")]
        public int? component_id { get; set; }
        public tbl_component_master tcm { get; set; } //reffrence key for tbl_component_master

        public int values { get; set; }

        public int created_by { get; set; }

        public DateTime created_dt { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_dt { get; set; }

        public int log_created_by { get; set; }

        public DateTime log_created_date { get; set; }

        public int is_active { get; set; }

    }
    public class tbl_ot_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ot_id { get; set; }

        [ForeignKey("tem")]
        public int? emp_id { get; set; }
        public tbl_employee_master tem { get; set; } //reffrence key for tbl_emp_master

        public DateTime ot_date { get; set; }

        public int ot_type { get; set; }

        public decimal ot_hours_worked { get; set; }
        public decimal normal_rate_per_hour { get; set; }
        public decimal ot_rate_per_hour { get; set; }

        public decimal Total_ot_amount_earned { get; set; }
        public DateTime ot_paid_date { get; set; }
        public string remarks { get; set; }
        public int created_by { get; set; }

        public DateTime created_dt { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_dt { get; set; }

        public int is_active { get; set; }

    }
    public class tbl_remimb_cat_mstr
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int rcm_id { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "reimbursement_category_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,50}$", ErrorMessage = "Invalid Reimbursment Category Name")]

        public string reimbursement_category_name { get; set; }

        public int created_by { get; set; }

        public DateTime created_dt { get; set; }

        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_active { get; set; }
        public int is_delete { get; set; }

    }
    public class tbl_rimb_grd_lmt_mstr
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int rglm_id { get; set; }

        [ForeignKey("tgm")]
        public int? grade_id { get; set; }
        public tbl_grade_master tgm { get; set; } //reffrence key for tbl grade master

        [ForeignKey("tem")]
        public int? emp_id { get; set; }
        public tbl_employee_master tem { get; set; } //reffrence key for tbl_emp_master
        public double monthly_limit { get; set; }
        public double yearly_limit { get; set; }

        public string fiscal_year_id { get; set; }
        public int created_by { get; set; }

        public DateTime created_dt { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_dt { get; set; }

        public int is_active { get; set; }
        public int is_delete { get; set; }
    }
    public class tbl_rimb_cat_lmt_mstr
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int rclm_id { get; set; }

        [ForeignKey("trglm")]
        public int? rglm_id { get; set; }
        public tbl_rimb_grd_lmt_mstr trglm { get; set; } //reffrence key for tbl reimbursement grade limit master

        [ForeignKey("trcm")]
        public int? rcm_id { get; set; }
        public tbl_remimb_cat_mstr trcm { get; set; } //reffrence key for tbl reimbursement category master

        //[ForeignKey("tgm")]
        //public int? grade_id { get; set; }
        //public tbl_grade_master tgm { get; set; } //reffrence key for tbl grade master
        public double monthly_limit { get; set; }
        public double yearly_limit { get; set; }

        public int created_by { get; set; }

        public DateTime created_dt { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_dt { get; set; }

        public int is_active { get; set; }
        public int is_delete { get; set; }
    }
    public class tbl_rimb_req_mstr
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int rrm_id { get; set; }
        [ForeignKey("tem")]
        public int? emp_id { get; set; }
        public tbl_employee_master tem { get; set; } //reffrence key for tbl_emp_master
        public int request_type { get; set; }
        public int request_month_year { get; set; }
        public string fiscal_year_id { get; set; }

        [Display(Description = "total_request_amount")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Requested Amount")]
        public double total_request_amount { get; set; }
        public int is_approvred { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remarks { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_active { get; set; }
        public int is_delete { get; set; }
    }
    public class tbl_rimb_req_details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int rrd_id { get; set; }
        [ForeignKey("trrm")]
        public int? rrm_id { get; set; }
        public tbl_rimb_req_mstr trrm { get; set; } //reffrence key for tbl reimbursement category limit master
        [ForeignKey("trclm")]
        public int? rclm_id { get; set; }
        public tbl_rimb_cat_lmt_mstr trclm { get; set; } //reffrence key for tbl reimbursement category limit master
        [Display(Description = "Bill_amount")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Bill Amount")]
        public double Bill_amount { get; set; }
        public DateTime Bill_date { get; set; }
        [Display(Description = "request_amount")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Requested Amount")]
        public double request_amount { get; set; }

        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_active { get; set; }
        public int is_delete { get; set; }

    }
    public class tbl_loan_request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int loan_req_id { get; set; }
        [ForeignKey("emp_master")] // Foreign Key here
        public int? req_emp_id { get; set; }
        public tbl_employee_master emp_master { get; set; }
        public string emp_code { get; set; }
        public int loan_type { get; set; } // if Loan then 1 if else advance then 2
        [Display(Description = "loan_amt")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Loan Amount")]
        public decimal loan_amt { get; set; }
        public int loan_tenure { get; set; } // Loan Tenure always in month 

        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "loan_purpose")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Loan Purpose")]
        public string loan_purpose { get; set; }
        [Display(Description = "interest_rate")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Rate of Interest")]
        public decimal interest_rate { get; set; }
        public double monthly_emi { get; set; }
        public byte is_final_approval { get; set; }
        public byte is_closed { get; set; }
        public DateTime created_dt { get; set; }
        //public int policy { get; set; }
        //public int loan_approval { get; set; } // if loan_approval =0  then Pending for Approval1, 1 then pending for Approval2 etc 
        public DateTime start_date { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

    }
    public class tbl_loan_request_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }
        public int loan_type { get; set; }
        public int em_status { get; set; }

        [Display(Description = "loan_amount")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Loan Amount")]
        public decimal loan_amount { get; set; }
        [Display(Description = "rate_of_interest")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Rate of Interest")]
        public decimal rate_of_interest { get; set; }
        [Display(Description = "on_salary")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Salary")]
        public decimal on_salary { get; set; }//monthly
        [Display(Description = "max_tenure")]
        [RegularExpression(@"^[0-9]{1,40}$", ErrorMessage = "Invalid Maximum Tenure")]
        public int max_tenure { get; set; }
        [Display(Description = "min_top_up_duration")]
        [RegularExpression(@"^[0-9]{1,40}$", ErrorMessage = "Invalid Top up Duration")]
        public int min_top_up_duration { get; set; }
        public DateTime created_dt { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public int grade_id { get; set; }
        public int companyid { get; set; }
        public byte is_reporting_mgr_approval { get; set; }
    }
    public class tbl_loan_approval
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }
        [ForeignKey("tbl_loan_request")] // Foreign Key here
        public int? loan_req_id { get; set; }
        public tbl_loan_request tbl_loan_request { get; set; }
        [ForeignKey("tbl_employee_master")] // Foreign Key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }
        public byte is_final_approver { get; set; }
        public byte is_approve { get; set; }

        public byte approval_order { get; set; }
        public DateTime created_dt { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

        //[ForeignKey("approver_setting")]
        public int approver_role_id { get; set; }

        // public tbl_loan_approval_setting approver_setting { get; set; }
    }
    public class tbl_component_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int component_id { get; set; }
        public string component_name { get; set; }
        public string datatype { get; set; }
        public string defaultvalue { get; set; }
        public int parentid { get; set; }
        public byte is_system_key { get; set; }
        public string property_details { get; set; }
        public string System_function { get; set; }
        public string System_table { get; set; }
        public int component_type { get; set; } // Here If component type=1 then Income, If component type=2 then Deduction If component type=3 then other If component type=4 then Other Income If component type=5 then Other Deduction 
        public byte is_salary_comp { get; set; }
        public byte is_tds_comp { get; set; }
        public byte is_data_entry_comp { get; set; }
        public int payment_type { get; set; }
        public int is_user_interface { get; set; }
        public int is_fnf_component { get; set; }
        public byte is_payslip { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public int is_active { get; set; }
        public DateTime modified_dt { get; set; }

    }
    public class tbl_component_formula_details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }
        [ForeignKey("comp_master")] // Foreign Key here
        public int? component_id { get; set; }
        public tbl_component_master comp_master { get; set; }
        [ForeignKey("company_master")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master company_master { get; set; }
        [ForeignKey("salary_group")] // Foreign Key here
        public int? salary_group_id { get; set; }
        public tbl_salary_group salary_group { get; set; }
        public string formula { get; set; }
        public int function_calling_order { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int deleted_by { get; set; }
        public DateTime deleted_dt { get; set; }
        public int is_deleted { get; set; }
    }
    public class tbl_emp_salary_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }
        [ForeignKey("tbl_employee_master")] // Foreign Key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }
        [ForeignKey("component_master")] // Foreign Key here
        public int? component_id { get; set; }
        public tbl_component_master component_master { get; set; }
        public DateTime applicable_from_dt { get; set; }

        [Display(Description = "applicable_value")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Component Salary")]
        public decimal applicable_value { get; set; }

        [Display(Description = "salaryrevision")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Salary")]
        public decimal salaryrevision { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_active { get; set; } //1: Approved  //0: Rejected  // 2: Pending
        public string maker_remark { get; set; }
        public string checker_remark { get; set; }
    }
    public class tbl_ot_rate_details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ot_id { get; set; }
        public int companyid { get; set; }
        [ForeignKey("grade_master")] // Foreign Key here
        public int? grade_id { get; set; }
        public tbl_grade_master grade_master { get; set; }
        [ForeignKey("employee_master")] // Foreign Key here
        public int? emp_id { get; set; }
        public tbl_employee_master employee_master { get; set; }
        [Display(Description = "ot_amt")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid OT Amount")]
        public double ot_amt { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }
    public class tbl_loan_approval_setting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        public int order { get; set; }
        public int emp_id { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

        [ForeignKey("company_master")]
        public int company_id { get; set; }

        public tbl_company_master company_master { get; set; }

        public byte is_final_approver { get; set; }

        [ForeignKey("role_master")]
        public int? approver_role_id { get; set; }

        public tbl_role_master role_master { get; set; }

        public int approver_type { get; set; } //1. Loan ,2. Asset
    }
    public class tbl_loan_repayments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int loan_repay_id { get; set; }  // primary key  must be public!     

        [ForeignKey("tbl_emp")] // foreign key here
        public int? req_emp_id { get; set; }
        public tbl_employee_master tbl_emp { get; set; }

        [ForeignKey("tbl_loan_req")] // foreign key here
        public int? loan_id { get; set; }
        public tbl_loan_request tbl_loan_req { get; set; }

        public double interest_component { get; set; }
        public double principal_amount { get; set; }
        public double loan_balance { get; set; }
        public DateTime date { get; set; }
        [StringLength(200)]
        [Display(Description = "remark")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remark { get; set; }
        public byte status { get; set; } // 0 Deduction /1 Repayment
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

    }
    public class tbl_employee_income_tax_amount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pre_emp_inc_id { get; set; }  // primary key  must be public!     

        [ForeignKey("tbl_emp")] // foreign key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_emp { get; set; }
        [Display(Description = "income_tax_amount")]
        [RegularExpression(@"^[0-9]{1,40}$", ErrorMessage = "Invalid Tax Amount")]
        public double income_tax_amount { get; set; }

        public byte is_deleted { get; set; }

        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }


    #region **CREATED BY SUPRIYA ON 19-08-2019 **
    public class tbl_muster_form1
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int form_I_mstr_id { get; set; }

        [ForeignKey("n_component_mstr")]
        public int? nature_and_date_c_id { get; set; }
        public tbl_component_master n_component_mstr { get; set; }


        [ForeignKey("w_component_mstr")]
        public int? whether_workman_c_id { get; set; }
        public tbl_component_master w_component_mstr { get; set; }

        [ForeignKey("r_component_mstr")]
        public int? rate_of_wages_c_id { get; set; }
        public tbl_component_master r_component_mstr { get; set; }

        [ForeignKey("d_component_mstr")]
        public int? date_c_id { get; set; }
        public tbl_component_master d_component_mstr { get; set; }

        [ForeignKey("amt_component_mstr")]
        public int? amt_c_id { get; set; }
        public tbl_component_master amt_component_mstr { get; set; }

        [ForeignKey("re_component_mstr")]
        public int? date_realised_c_id { get; set; }
        public tbl_component_master re_component_mstr { get; set; }

        [ForeignKey("rm_component_mstr")]
        public int? remarks_c_id { get; set; }
        public tbl_component_master rm_component_mstr { get; set; }

        [ForeignKey("company_master")]
        public int comp_id { get; set; }
        public tbl_company_master company_master { get; set; }

        public int is_deleted { get; set; }
        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }
    }

    public class tbl_muster_form2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int form_II_mstr_id { get; set; }

        [ForeignKey("dmg_component_mstr")]
        public int? damage_orloss_and_date_c_id { get; set; }
        public tbl_component_master dmg_component_mstr { get; set; }

        [ForeignKey("w_component_mstr")]
        public int? whether_workman_c_id { get; set; }
        public tbl_component_master w_component_mstr { get; set; }

        [ForeignKey("d_component_mstr")]
        public int? date_ofdeduc_c_id { get; set; }
        public tbl_component_master d_component_mstr { get; set; }

        [ForeignKey("amt_component_mstr")]
        public int? amt_ofdeduc_c_id { get; set; }

        public tbl_component_master amt_component_mstr { get; set; }

        [ForeignKey("in_component_mstr")]
        public int? no_of_installment_c_id { get; set; }
        public tbl_component_master in_component_mstr { get; set; }


        [ForeignKey("re_component_mstr")]
        public int? date_realised_c_id { get; set; }
        public tbl_component_master re_component_mstr { get; set; }

        [ForeignKey("rm_component_mstr")]
        public int? remarks_c_id { get; set; }
        public tbl_component_master rm_component_mstr { get; set; }

        [ForeignKey("company_master")]
        public int comp_id { get; set; }
        public tbl_company_master company_master { get; set; }

        public int is_deleted { get; set; }
        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }
    }


    public class tbl_muster_form2_data
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int form_ii_id { get; set; }

        [ForeignKey("tbl_emp")] // foreign key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_emp { get; set; }

        [ForeignKey("company_master")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master company_master { get; set; }

        public int payroll_month { get; set; }

        public string employee_code { get; set; }
        [MaxLength(150)]
        [StringLength(150)]
        [Display(Description = "employee_name")]
        [RegularExpression(@"^[a-zA-Z'\s'\.]{1,150}$", ErrorMessage = "Invalid Employee Name")]
        public string employee_name { get; set; }
        [MaxLength(150)]
        [StringLength(150)]
        [Display(Description = "father_or_husband_name")]
        [RegularExpression(@"^[a-zA-Z'\s'\.]{1,150}$", ErrorMessage = "Invalid Father/Husband Name")]
        public string father_or_husband_name { get; set; }

        public string department { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "damage_orloss_and_date")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.'\-]{1,100}$", ErrorMessage = "Invalid Damage or Loss and Date")]
        public string damage_orloss_and_date { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "whether_workman")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,100}$", ErrorMessage = "Invalid Whether Workman")]
        public string whether_workman { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "date_of_deduc_imposed")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,50}$", ErrorMessage = "Invalid Date of Deduction Imposed")]
        public string date_of_deduc_imposed { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "amt_of_deduc_imposed")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Amount of Deduction Imposed")]
        public string amt_of_deduc_imposed { get; set; }
        [MaxLength(10)]
        [StringLength(10)]
        [Display(Description = "no_of_installment")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,10}$", ErrorMessage = "Invalid No of Installment")]
        public string no_of_installment { get; set; }
        [MaxLength(10)]
        [StringLength(10)]
        [Display(Description = "date_realised")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,10}$", ErrorMessage = "Invalid Realised Date")]
        public string date_realised { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remarks { get; set; }

        public int is_deleted { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }

        public string gender { get; set; }
    }


    //public class tbl_rate_ofdeduction
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int rate_id { get; set; }
    //    [ForeignKey("component_master")]
    //    public int deduction_id { get; set; }
    //    public tbl_component_master component_master { get; set; }

    //    [ForeignKey("company_master")]
    //    public int comp_id { get; set; }
    //    public tbl_company_master company_master { get; set; }

    //    public int is_deleted { get; set; }
    //    public int created_by { get; set; }

    //    public DateTime created_date { get; set; }

    //    public int modified_by { get; set; }

    //    public DateTime modified_date { get; set; }
    //}

    #endregion ** END BY SUPRIYA ON 19-08-2019**


    #region     ** CREATE BY AMARJEET ON 21-08-2019**

    public class tbl_muster_form1_data
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int form_i_id { get; set; }

        [ForeignKey("tbl_emp")] // foreign key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_emp { get; set; }

        [ForeignKey("company_master")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master company_master { get; set; }

        public int payroll_month { get; set; }

        public string employee_code { get; set; }
        [MaxLength(150)]
        [StringLength(150)]
        [Display(Description = "employee_name")]
        [RegularExpression(@"^[a-zA-Z'\s'\.]{1,150}$", ErrorMessage = "Invalid Employee Name")]
        public string employee_name { get; set; }
        [MaxLength(150)]
        [StringLength(150)]
        [Display(Description = "father_or_husband_name")]
        [RegularExpression(@"^[a-zA-Z'\s'\.]{1,150}$", ErrorMessage = "Invalid Father/Husband Name")]
        public string father_or_husband_name { get; set; }

        public string department { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "nature_and_date")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,100}$", ErrorMessage = "Invalid Naute and Date of Offence")]
        public string nature_and_date { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "whether_workman")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-'\.]{1,100}$", ErrorMessage = "Invalid Whether Workman")]
        public string whether_workman { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "rate_of_wages")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Rate of Wages")]
        public string rate_of_wages { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "date_of_fine_imposed")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,50}$", ErrorMessage = "Invalid Date of Fine Imposed")]
        public string date_of_fine_imposed { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "amt_of_fine_imposed")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Amount of Fine Imposed")]
        public string amt_of_fine_imposed { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "date_realised")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,50}$", ErrorMessage = "Invalid Realised Date")]
        public string date_realised { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remarks { get; set; }

        public int is_deleted { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }
    }

    #endregion

    #region **START BY SUPRIYA ON 26-08-2019

    public class tbl_muster_form3
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int form_III_mstr_id { get; set; }

        [ForeignKey("ad_component_mstr")]
        public int? advance_date_c_id { get; set; }
        public tbl_component_master ad_component_mstr { get; set; }

        [ForeignKey("aa_component_mstr")]
        public int? advance_amt_c_id { get; set; }

        public tbl_component_master aa_component_mstr { get; set; }
        [ForeignKey("p_component_mstr")]
        public int? purpose_c_id { get; set; }
        public tbl_component_master p_component_mstr { get; set; }

        [ForeignKey("in_component_mstr")]
        public int? no_of_installment_c_id { get; set; }
        public tbl_component_master in_component_mstr { get; set; }

        [ForeignKey("pg_component_mstr")]
        public int? postponement_granted_c_id { get; set; }
        public tbl_component_master pg_component_mstr { get; set; }

        [ForeignKey("re_component_mstr")]
        public int? date_total_repaid_c_id { get; set; }
        public tbl_component_master re_component_mstr { get; set; }

        [ForeignKey("rm_component_mstr")]
        public int? remarks_c_id { get; set; }
        public tbl_component_master rm_component_mstr { get; set; }

        [ForeignKey("company_master")]
        public int comp_id { get; set; }
        public tbl_company_master company_master { get; set; }

        public int is_deleted { get; set; }
        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }
    }


    public class tbl_muster_form3_data
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int form_iii_id { get; set; }

        [ForeignKey("tbl_emp")] // foreign key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_emp { get; set; }

        [ForeignKey("company_master")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master company_master { get; set; }

        public int payroll_month { get; set; }

        public string employee_code { get; set; }
        [MaxLength(150)]
        [StringLength(150)]
        [Display(Description = "employee_name")]
        [RegularExpression(@"^[a-zA-Z'\s'\.]{1,150}$", ErrorMessage = "Invalid Employee Name")]
        public string employee_name { get; set; }
        [MaxLength(150)]
        [StringLength(150)]
        [Display(Description = "father_or_husband_name")]
        [RegularExpression(@"^[a-zA-Z'\s'\.]{1,150}$", ErrorMessage = "Invalid Father/Husband Name")]
        public string father_or_husband_name { get; set; }

        public string department { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "advance_date")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,50}$", ErrorMessage = "Invalid Advance Date")]
        public string advance_date { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "advance_amount")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,100}$", ErrorMessage = "Invalid Advance Amount")]
        public string advance_amount { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "purpose")]
        [RegularExpression(@"^[a-zA-Z0-9'\s']{1,200}$", ErrorMessage = "Invalid Purpose")]
        public string purpose { get; set; }
        [MaxLength(10)]
        [StringLength(10)]
        [Display(Description = "no_of_installment")]
        [RegularExpression(@"^[a-zA-Z0-9'\s']{1,10}$", ErrorMessage = "Invalid No of Installment")]
        public string no_of_installment { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "postponse_granted")]
        [RegularExpression(@"^[a-zA-Z0-9'\s']{1,50}$", ErrorMessage = "Invalid Postpone Granted")]
        public string postponse_granted { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "date_of_repaid")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,50}$", ErrorMessage = "Invalid Date of Rapaid")]
        public string date_of_repaid { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s']{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remarks { get; set; }

        public int is_deleted { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }
    }



    public class tbl_muster_form4
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int form_IV_mstr_id { get; set; }

        [ForeignKey("d_component_mstr")]
        public int? overtime_date_c_id { get; set; }
        public tbl_component_master d_component_mstr { get; set; }

        [ForeignKey("eo_component_mstr")]
        public int? extent_overtime_c_id { get; set; }
        public tbl_component_master eo_component_mstr { get; set; }

        [ForeignKey("to_component_mstr")]
        public int? total_overtime_c_id { get; set; }
        public tbl_component_master to_component_mstr { get; set; }

        [ForeignKey("hr_component_mstr")]
        public int? normal_hr_c_id { get; set; }
        public tbl_component_master hr_component_mstr { get; set; }

        [ForeignKey("r_component_mstr")]
        public int? normal_rate_c_id { get; set; }
        public tbl_component_master r_component_mstr { get; set; }

        [ForeignKey("or_component_mstr")]
        public int? overtime_rate_c_id { get; set; }
        public tbl_component_master or_component_mstr { get; set; }



        [ForeignKey("ne_component_mstr")]
        public int? normal_earning_c_id { get; set; }
        public tbl_component_master ne_component_mstr { get; set; }

        [ForeignKey("oe_component_mstr")]
        public int? overtime_earning_c_id { get; set; }
        public tbl_component_master oe_component_mstr { get; set; }

        [ForeignKey("te_component_mstr")]
        public int? total_earning_c_id { get; set; }
        public tbl_component_master te_component_mstr { get; set; }

        [ForeignKey("dt_component_mstr")]
        public int? date_ofpayment_c_id { get; set; }
        public tbl_component_master dop_component_mstr { get; set; }


        [ForeignKey("company_master")]
        public int comp_id { get; set; }
        public tbl_company_master company_master { get; set; }

        public int is_deleted { get; set; }
        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }
    }

    public class tbl_muster_form4_data
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int form_iv_id { get; set; }

        [ForeignKey("tbl_emp")] // foreign key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_emp { get; set; }

        [ForeignKey("company_master")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master company_master { get; set; }

        public int payroll_month { get; set; }

        public string employee_code { get; set; }
        [MaxLength(150)]
        [StringLength(150)]
        [Display(Description = "employee_name")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,150}$", ErrorMessage = "Invalid Employee Name")]
        public string employee_name { get; set; }
        [MaxLength(150)]
        [StringLength(150)]
        [Display(Description = "father_or_husband_name")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,150}$", ErrorMessage = "Invalid Father/Husband Name")]
        public string father_or_husband_name { get; set; }
        [MaxLength(10)]
        [StringLength(10)]
        [Display(Description = "sex")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,10}$", ErrorMessage = "Invalid Sex Name")]
        public string sex { get; set; }
        public string designation { get; set; }
        public string department { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "overtime_work_dt")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,50}$", ErrorMessage = "Invalid Overtime Worked Date")]
        public string overtime_work_dt { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "extent_overtime")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,50}$", ErrorMessage = "Invalid Extend Overtime")]
        public string extent_overtime { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "total_overtime_worked")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Total Overtime Overtime")]
        public string total_overtime_worked { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "normal_hr")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Normal Hour")]
        public string normal_hr { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "normal_rate")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Normal Rate")]
        public string normal_rate { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "overtime_rate")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Overtime Rate")]
        public string overtime_rate { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "normal_earning")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Normal Earning")]
        public string normal_earning { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "overtime_earning")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Overtime Earning")]
        public string overtime_earning { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "total_earning")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Total Earning")]
        public string total_earning { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "date_ofpayment")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,50}$", ErrorMessage = "Invalid Date of Payment")]
        public string date_ofpayment { get; set; }

        public int is_deleted { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }
    }


    #endregion ** END BY SUPRIYA ON 27-08-2019


    #region ** START BY SUPRIYA ON 29-08-2019
    public class tbl_muster_form10
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int form_X_mstr_id { get; set; }

        [ForeignKey("mb_component_mstr")]
        public int? basic_min_rate_pay_c_id { get; set; }
        public tbl_component_master mb_component_mstr { get; set; }

        [ForeignKey("mda_component_mstr")]
        public int? da_min_rate_pay_c_id { get; set; }
        public tbl_component_master mda_component_mstr { get; set; }

        [ForeignKey("ab_component_mstr")]
        public int? basic_wages_actually_pay_c_id { get; set; }
        public tbl_component_master ab_component_mstr { get; set; }

        [ForeignKey("ada_component_mstr")]
        public int? da_wages_actually_pay_c_id { get; set; }
        public tbl_component_master ada_component_mstr { get; set; }

        [ForeignKey("ta_component_mstr")]
        public int? total_attan_orunit_ofworkdone_c_id { get; set; }
        public tbl_component_master ta_component_mstr { get; set; }

        [ForeignKey("ow_component_mstr")]
        public int? overtime_worked_c_id { get; set; }
        public tbl_component_master ow_component_mstr { get; set; }


        [ForeignKey("gw_component_mstr")]
        public int? gross_wages_pay_c_id { get; set; }
        public tbl_component_master gw_component_mstr { get; set; }

        [ForeignKey("epf_component_mstr")]
        public int? employers_pf_c_id { get; set; }
        public tbl_component_master epf_component_mstr { get; set; }

        [ForeignKey("hrd_component_mstr")]
        public int? hr_deduction_c_id { get; set; }
        public tbl_component_master hrd_component_mstr { get; set; }

        [ForeignKey("od_component_mstr")]
        public int? other_deduction_c_id { get; set; }
        public tbl_component_master od_component_mstr { get; set; }

        [ForeignKey("td_component_mstr")]
        public int? total_deduction_c_id { get; set; }
        public tbl_component_master td_component_mstr { get; set; }

        [ForeignKey("wp_component_mstr")]
        public int? wages_paid_c_id { get; set; }
        public tbl_component_master wp_component_mstr { get; set; }

        [ForeignKey("dop_component_mstr")]
        public int? date_ofpayment_c_id { get; set; }
        public tbl_component_master dop_component_mstr { get; set; }

        [ForeignKey("company_master")]
        public int comp_id { get; set; }
        public tbl_company_master company_master { get; set; }

        public int is_deleted { get; set; }
        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }
    }



    public class tbl_muster_form10_data
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int form_x_id { get; set; }

        [ForeignKey("tbl_emp")] // foreign key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_emp { get; set; }

        [ForeignKey("company_master")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master company_master { get; set; }

        public int payroll_month { get; set; }

        public string employee_code { get; set; }
        [MaxLength(150)]
        [StringLength(150)]
        [Display(Description = "employee_name")]
        [RegularExpression(@"^[a-zA-Z'\s'\.]{1,150}$", ErrorMessage = "Invalid Employee Name")]
        public string employee_name { get; set; }
        [MaxLength(150)]
        [StringLength(150)]
        [Display(Description = "father_or_husband_name")]
        [RegularExpression(@"^[a-zA-Z'\s'\.]{1,150}$", ErrorMessage = "Invalid Father/Husband Name")]
        public string father_or_husband_name { get; set; }
        public string designation { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "basic_minimum_payable")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Minimum Basic Pay")]
        public string basic_minimum_payable { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "da_minimum_payable")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Minimum DA Pay")]
        public string da_minimum_payable { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "basic_wages_actually_pay")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Basic Wages Actually Pay")]
        public string basic_wages_actually_pay { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "da_wages_actually_pay")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid DA Wages Actually Pay")]
        public string da_wages_actually_pay { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "total_attand_or_unitof_work_done")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Total Attandance or Unit of work done")]
        public string total_attand_or_unitof_work_done { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "overtime_worked")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Overtime Worked")]
        public string overtime_worked { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "gross_wages_pay")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Gross Wages Pay")]
        public string gross_wages_pay { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "contri_of_employer_pf")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Contribution of Employer PF")]
        public string contri_of_employer_pf { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "hr_deduction")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid HR Deduction")]
        public string hr_deduction { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "other_deduction")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Other Deduction")]
        public string other_deduction { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "total_deduction")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Total Deduction")]
        public string total_deduction { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "wages_paid")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Wages")]
        public string wages_paid { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "date_of_payment")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,50}$", ErrorMessage = "Invalid Date of Payment")]
        public string date_of_payment { get; set; }

        public string emp_sign_orthump_exp { get; set; }

        public int is_deleted { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }
    }

    public class tbl_muster_form11
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int form_XI_mstr_id { get; set; }

        [ForeignKey("bwp_component_mstr")]
        public int? basic_wages_pay_c_id { get; set; }
        public tbl_component_master bwp_component_mstr { get; set; }

        [ForeignKey("dawp_component_mstr")]
        public int? da_wages_pay_c_id { get; set; }
        public tbl_component_master dawp_component_mstr { get; set; }

        [ForeignKey("ta_component_mstr")]
        public int? total_attand_orwork_done_c_id { get; set; }
        public tbl_component_master ta_component_mstr { get; set; }

        [ForeignKey("ow_component_mstr")]
        public int? overtime_wages_c_id { get; set; }
        public tbl_component_master ow_component_mstr { get; set; }

        [ForeignKey("gw_component_mstr")]
        public int? gross_wages_pay_c_id { get; set; }
        public tbl_component_master gw_component_mstr { get; set; }

        [ForeignKey("td_component_mstr")]
        public int? total_deduction_c_id { get; set; }
        public tbl_component_master td_component_mstr { get; set; }


        [ForeignKey("nw_component_mstr")]
        public int? net_wages_pay_c_id { get; set; }
        public tbl_component_master nw_component_mstr { get; set; }

        [ForeignKey("pinch_component_mstr")]
        public int? pay_incharge_c_id { get; set; }

        public tbl_component_master pinch_component_mstr { get; set; }
        [ForeignKey("company_master")]
        public int comp_id { get; set; }
        public tbl_company_master company_master { get; set; }

        public int is_deleted { get; set; }
        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }
    }

    public class tbl_muster_form11_data
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int form_xi_id { get; set; }

        [ForeignKey("tbl_emp")] // foreign key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_emp { get; set; }

        [ForeignKey("company_master")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master company_master { get; set; }

        public int payroll_month { get; set; }

        public string employee_code { get; set; }
        [MaxLength(150)]
        [StringLength(150)]
        [Display(Description = "employee_name")]
        [RegularExpression(@"^[a-zA-Z'\s'\.]{1,150}$", ErrorMessage = "Invalid Employee Name")]
        public string employee_name { get; set; }
        [MaxLength(150)]
        [StringLength(150)]
        [Display(Description = "father_or_husband_name")]
        [RegularExpression(@"^[a-zA-Z'\s'\.]{1,150}$", ErrorMessage = "Invalid Father/Husband Name")]
        public string father_or_husband_name { get; set; }
        public string designation { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "basic_wage_payable")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Basic Wages Payable")]
        public string basic_wage_payable { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "da_wage_payable")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid DA Wages Payable")]
        public string da_wage_payable { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "total_attand_orwork_done")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Total Attendance or Work Done")]
        public string total_attand_orwork_done { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "overtime_wages")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Overtime Wages")]
        public string overtime_wages { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "gross_wage_pay")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Gross Wages Pay")]
        public string gross_wage_pay { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "total_deduction")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Total Deduction")]
        public string total_deduction { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "net_wage_pay")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Net Wages Pay")]
        public string net_wage_pay { get; set; }

        public string emp_sign_orthump_exp { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "pay_incharge")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,50}$", ErrorMessage = "Invalid Payin Charge")]
        public string pay_incharge { get; set; }
        public int is_deleted { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }
    }

    #endregion ** END BY SUPRIYA ON 29-08-2019


    #region ** START FNF Process  **

    public class tbl_fnf_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pkid_fnfMaster { get; set; }

        [ForeignKey("emp_mstr")]
        public int? emp_id { get; set; }

        public tbl_employee_master emp_mstr { get; set; }

        [ForeignKey("emp_sep_mstr")]
        public int? fkid_empSepration { get; set; }

        public tbl_emp_separation emp_sep_mstr { get; set; }

        public DateTime resign_dt { get; set; }
        public DateTime last_working_date { get; set; }
        public int notice_recovery_days { get; set; }
        public int monthYear { get; set; }
        public int notice_payment_days { get; set; }
        public decimal net_amt { get; set; }
        public decimal settlment_amt { get; set; }
        public DateTime settlment_dt { get; set; }
        public int settlement_type { get; set; }
        public int is_freezed { get; set; }
        public int is_gratuity { get; set; }
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public string remarks { get; set; }
    }

    public class tbl_emp_fnf_asset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int fnf_asset_id { get; set; }

        public string asset_name { get; set; }

        public string asset_number { get; set; }

        public double recovery_amt { get; set; }
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_process { get; set; }
        public int x_id { get; set; }
        [ForeignKey("fn_mstr")]
        public int? fnf_id { get; set; }
        public tbl_fnf_master fn_mstr { get; set; }

    }

    public class tbl_fnf_reimburesment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int fnf_reim_id { get; set; }


        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "reim_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Reimbursment Head")]
        public string reim_name { get; set; }
        [Display(Description = "reim_amt")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Amount")]
        public decimal reim_amt { get; set; }

        public decimal reim_bal { get; set; }

        [ForeignKey("fnf_mstr")]
        public int? fnf_id { get; set; }

        public tbl_fnf_master fnf_mstr { get; set; }
        public int x_id { get; set; }
    }

    public class tbl_fnf_leave_encash
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_encash_id { get; set; }


        [ForeignKey("leave_type")]
        public int? leave_type_id { get; set; }

        public tbl_leave_type leave_type { get; set; }

        [Display(Description = "leave_balance")]
        [RegularExpression(@"^[0-9'\.]{1,6}$", ErrorMessage = "Invalid Leave Balance")]
        public double leave_balance { get; set; }

        [Display(Description = "leave_encash_day")]
        [RegularExpression(@"^[0-9'\.]{1,6}$", ErrorMessage = "Invalid Leave Encash")]
        public double leave_encash_day { get; set; }

        [Display(Description = "leave_encash_cal")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Leave Encashment Calculated Amount")]
        public decimal leave_encash_cal { get; set; }

        [Display(Description = "leave_encash_final")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Leave Encashment Final Amount")]
        public decimal leave_encash_final { get; set; }

        [ForeignKey("fnf_mstr")]
        public int? fnf_id { get; set; }

        public tbl_fnf_master fnf_mstr { get; set; }
        public int x_id { get; set; }

        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_process { get; set; }

        public int emp_sep_id { get; set; }


    }

    public class tbl_fnf_attendance_dtl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int fnf_attend_id { get; set; }


        [Display(Description = "monthyear")]
        [RegularExpression(@"^[0-9]{1,4}$", ErrorMessage = "Invalid Month Year")]
        public int monthyear { get; set; }

        [Display(Description = "total_paid_days")]
        [RegularExpression(@"^[0-9'\.]{1,6}$", ErrorMessage = "Invalid Total Paid Days")]
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

        [Display(Description = "paid_days")]
        [RegularExpression(@"^[0-9'\.]{1,6}$", ErrorMessage = "Invalid Paid Days")]
        public double Total_Paid_days { get; set; }

        [Display(Description = "paid_amount")]
        [RegularExpression(@"^[0-9'\.]{1,10}$", ErrorMessage = "Invalid Paid amount")]
        public double paid_amount { get; set; }

        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_process { get; set; }

        [ForeignKey("fnf_mstr")]
        public int? fnf_id { get; set; }

        public tbl_fnf_master fnf_mstr { get; set; }

        public int x_id { get; set; }

        public int emp_sep_id { get; set; }
    }

    public class tbl_fnf_component
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int variable_id { get; set; }

        [ForeignKey("component_mstr")]
        [Display(Description = "component_id")]
        [RegularExpression(@"^[0-9]{1,10}$", ErrorMessage = "Invalid Component")]
        public int? component_id { get; set; }

        public tbl_component_master component_mstr { get; set; }

        public int monthyear { get; set; }

        public int component_type { get; set; } // Here If component type=1 then Income, If component type=2 then Deduction If component type=3 then other If component type=4 then Other Income If component type=5 then Other Deduction 

        [Display(Description = "variable_amt")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Amount")]
        public double variable_amt { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remarks { get; set; }

        [ForeignKey("fnf_mstr")]
        public int? fnf_id { get; set; }

        public tbl_fnf_master fnf_mstr { get; set; }
        public int x_id { get; set; }

        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_process { get; set; }
        public int emp_sep_id { get; set; }

    }

    public class tbl_fnf_loan_recover
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int loan_recovery_id { get; set; }

        [ForeignKey("loan_req")]
        public int? loan_req_id { get; set; }

        public tbl_loan_request loan_req { get; set; }

        //[ForeignKey("loan_repay")]
        //public int? loan_repayment_id { get; set; }

        //public tbl_loan_repayments loan_repay { get; set; } // receive amount
        //[Display(Description = "credit")]
        //[RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Credit Amount")]
        //public double credit { get; set; } // receive amount
        //[Display(Description = "debit")]
        //[RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Debit Amount")]
        //public double debit { get; set; } //pending amount
        [Display(Description = "loan_recover_amt")]
        [RegularExpression(@"^[0-9'\.]{1,40}$", ErrorMessage = "Invalid Recover Amount")]
        public decimal loan_recover_amt { get; set; }

        public byte is_process { get; set; }

        public int x_id { get; set; }
        public int emp_sep_id { get; set; }

    }

    #endregion ** END **



}
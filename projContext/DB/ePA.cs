using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB
{

    public class tbl_epa_fiscal_yr_mstr
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int fiscal_year_id { get; set; }
        public string financial_year_name { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        [ForeignKey("tbl_company_master")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master tbl_company_master { get; set; }
        public DateTime created_date { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_epa_cycle_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cycle_id { get; set; }
        public byte cycle_type { get; set; } // 0 Monthly, 1 Quaterly, 2 Half Yearly, 3 Yearly
        public int number_of_day { get; set; }
        public DateTime from_date { get; set; } // Number of day for Which EPA Will be open for employee
        [ForeignKey("tbl_company_master")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master tbl_company_master { get; set; }
        public DateTime created_date { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

    }

    public class tbl_working_role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int working_role_id { get; set; }
        public string working_role_name { get; set; }

        [ForeignKey("tbl_company_master")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master tbl_company_master { get; set; }

        [ForeignKey("tbl_dept_master")] // Foreign Key here
        public int? dept_id { get; set; }
        public tbl_department_master tbl_dept_master { get; set; }


        public byte is_default { get; set; } // 0 no , 1 yes
        public byte is_active { get; set; }

        public DateTime created_date { get; set; }
        public int created_by { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

    }

    public class tbl_epa_status_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int epa_status_id { get; set; }

        [ForeignKey("comp_master")]
        public int? company_id { get; set; }

        public tbl_company_master comp_master { get; set; }
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "status_name")]
        //[RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Status Name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,250}$", ErrorMessage = "Invalid statuts name")]
        public string status_name { get; set; }
        [MaxLength(500)]
        [StringLength(500)]
        [Display(Description = "description")]
        //[RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Description")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,500}$", ErrorMessage = "Invalid Description")]
        public string description { get; set; }

        [Display(Description = "display_for")]
        [RegularExpression(@"^[0-9]{1,40}$", ErrorMessage = "Invalid User")]
        public int display_for { get; set; }

        [Display(Description = "display_for_rm1")]
        [RegularExpression(@"^[0-9]{1,40}$", ErrorMessage = "Invalid Display RM1")]
        public int display_for_rm1 { get; set; }

        [Display(Description = "display_for_rm2")]
        [RegularExpression(@"^[0-9]{1,40}$", ErrorMessage = "Invalid Display For")]
        public int display_for_rm2 { get; set; }

        [Display(Description = "display_for_rm3")]
        [RegularExpression(@"^[0-9]{1,40}$", ErrorMessage = "Invalid Display For")]
        public int display_for_rm3 { get; set; }

        [Display(Description = "display_order")]
        [RegularExpression(@"^[0-9]{1,40}$", ErrorMessage = "Invalid Display Order")]
        public int display_order { get; set; } // for user

        public int status { get; set; }// 0 For Start,1 For Closed,2 For Middle Level

        public byte is_deleted { get; set; }

        public int created_by { get; set; }
        public DateTime created_date { get; set; }


        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }


    }

    public class tbl_kpi_objective_type
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int obj_type_id { get; set; }

        [ForeignKey("comp_master")]
        public int? company_id { get; set; }

        public tbl_company_master comp_master { get; set; }
        [MaxLength(500)]
        [StringLength(500)]
        //[Display(Description = "objective_name")]
        //[RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Objective Name")]
        public string objective_name { get; set; }
        [MaxLength(1000)]
        [StringLength(1000)]
        //[Display(Description = "description")]
        //[RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Description")]
        public string description { get; set; }
        public byte is_deleted { get; set; }

        public int deleted_by { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }


    public class tbl_kpi_criteria
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int kpi_cr_id { get; set; }


        [ForeignKey("tbl_company")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master tbl_company { get; set; }



        //[RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Criteria")]
        public int criteria_count { get; set; }

        [MaxLength(500)]
        [StringLength(500)]
        [Display(Description = "description")]
        //  [RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Description")]
        public string description { get; set; }

        public byte is_active { get; set; }

        public DateTime created_date { get; set; }
        public int created_by { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

    }


    public class tbl_status_flow_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int flow_mster_id { get; set; }

        [ForeignKey("status_master")]
        public int start_status_id { get; set; }

        public tbl_epa_status_master status_master { get; set; }

        [ForeignKey("next_status")]
        public int next_status_id { get; set; }

        public tbl_epa_status_master next_status { get; set; }
        [MaxLength(500)]
        [StringLength(500)]
        [Display(Description = "description")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,500}$", ErrorMessage = "Invalid description")]
        public string description { get; set; }
        public int is_deleted { get; set; }

        public int deleted_by { get; set; }

        public DateTime created_dt { get; set; }

        public int created_by { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }
    }

    public class tbl_kpi_key_area_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int key_area_id { get; set; }

        [ForeignKey("comp_master")]
        public int? company_id { get; set; }
        public tbl_company_master comp_master { get; set; }

        //[ForeignKey("tbl_f_year")]
        //public int? f_year_id { get; set; } // fiscal_year_id
        //public tbl_epa_fiscal_yr_mstr tbl_f_year { get; set; }

        [ForeignKey("tbl_w_role")]
        public int? w_r_id { get; set; } // working_role_id
        public tbl_working_role tbl_w_role { get; set; }

        [ForeignKey("tbl_obj_type")]
        public int? otype_id { get; set; }//objective_type id
        public tbl_kpi_objective_type tbl_obj_type { get; set; }

        public string key_area { get; set; } // need to 

        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "expected_result")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,250}$", ErrorMessage = "Invalid Expected Result")]
        public string expected_result { get; set; }
        public int wtg { get; set; }
        [MaxLength(1000)]
        [StringLength(1000)]
        public string comment { get; set; }

        public byte is_deleted { get; set; }

        public int deleted_by { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public virtual ICollection<tbl_kpi_criteria_master> tbl_kpi_criteria_master { get; set; }
     
    }

    public class tbl_kpi_criteria_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int crit_id { get; set; }

        [ForeignKey("tbl_key_area")]
        public int? k_a_id { get; set; }
        public tbl_kpi_key_area_master tbl_key_area { get; set; }

        public int criteria_number { get; set; }
        public string criteria { get; set; }

        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_kpi_rating_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int kpi_rating_id { get; set; }

        [ForeignKey("comp_master")]
        public int? company_id { get; set; }

        public tbl_company_master comp_master { get; set; }
        [MaxLength(10)]
        [StringLength(10)]
        [Display(Description = "rating_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\+'\-]{1,10}$", ErrorMessage = "Invalid Name")]
        public string rating_name { get; set; }
        [MaxLength(500)]
        [StringLength(500)]
        [Display(Description = "description")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,500}$", ErrorMessage = "Invalid Description")]
        public string description { get; set; }

        public int display_order { get; set; }

        public int is_deleted { get; set; }

        public int deleted_by { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }


    }

    public class tbl_kra_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int kra_mstr_id { get; set; }

        [ForeignKey("comp_master")]
        public int? company_id { get; set; }

        public tbl_company_master comp_master { get; set; }

        //[ForeignKey("financial_mstr")]
        //public int? financial_yr { get; set; }
        //public tbl_epa_fiscal_yr_mstr financial_mstr { get; set; }


        //public int cycle_id { get; set; } //if 0 is selected in tbl_epa_cycle_master mean Monthly than 1 to 12 any Month no is bind in cycle_id,if 1 means quaterly is selected than 1 to 4 any quater is no is bind in cycle_id etc. 


        [ForeignKey("depart_mstr")]
        public int? department_id { get; set; }
        public tbl_department_master depart_mstr { get; set; }

        [ForeignKey("work_role")]
        public int? wrk_role_id { get; set; }
        public tbl_working_role work_role { get; set; }

        [MaxLength(1000)]
        [StringLength(1000)]
        //[Display(Description = "description")]
        //[RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Description")]
        public string description { get; set; }
        [MaxLength(1000)]
        [StringLength(1000)]
        //[Display(Description = "factor_result")]
        //[RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Factor/Result")]
        public string factor_result { get; set; }
        public int display_order { get; set; }
        public int is_deleted { get; set; }
        public int deleted_by { get; set; }

        public int created_by { get; set; }

        public DateTime creatd_dt { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_dt { get; set; }
    }

    public class tbl_tab_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int tab_mstr_id { get; set; }

        [ForeignKey("comp_master")]
        public int? company_id { get; set; }

        public tbl_company_master comp_master { get; set; }
        [MaxLength(500)]
        [StringLength(500)]
        //[Display(Description = "tab_name")]
        //[RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,40}$", ErrorMessage = "Invalid Tab Name")]
        public string tab_name { get; set; }
        [MaxLength(1000)]
        [StringLength(1000)]
        //[Display(Description = "description")]
        //[RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,40}$", ErrorMessage = "Invalid Description")]
        public string description { get; set; }
        public byte for_rm1 { get; set; }
        public byte for_rm2 { get; set; }
        public byte for_rm3 { get; set; }
        public byte for_user { get; set; }
        public int display_order { get; set; }

        public byte is_deleted { get; set; }

        public int deleted_by { get; set; }

        public int created_by { get; set; }

        public DateTime created_dt { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }

    }

    public class tbl_question_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ques_mstr_id { get; set; }

        [ForeignKey("comp_mstr")]
        public int? company_id { get; set; }

        public tbl_company_master comp_mstr { get; set; }

        [ForeignKey("wrk_role")]
        public int? wrk_role_id { get; set; }

        public tbl_working_role wrk_role { get; set; }

        [ForeignKey("tab_mstr")]
        public int? tab_id { get; set; }

        public tbl_tab_master tab_mstr { get; set; }
        [MaxLength(1000)]
        [StringLength(1000)]
        //[Display(Description = "ques")]
        //[RegularExpression(@"^[a-zA-Z0-9'\s'\?]{1,40}$", ErrorMessage = "Invalid Question")]
        public string ques { get; set; }

        public int ans_type { get; set; } //1 for textbox,2 for dropdown,3 for Yes/No with Remarks,4 Yes/No without Remarks
        [MaxLength(300)]
        [StringLength(300)]
        [Display(Description = "ans_type_ddl")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\,]{1,300}$", ErrorMessage = "Invalid Data Source")]

        public string ans_type_ddl { get; set; }
        [MaxLength(1000)]
        [StringLength(1000)]
        [Display(Description = "description")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,1000}$", ErrorMessage = "Invalid Description")]

        public string description { get; set; }
        public int deleted_by { get; set; }

        public int is_deleted { get; set; }

        public int created_by { get; set; }

        public DateTime created_dt { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }

        public int ques_display_order { get; set; }
    }


    #region ********* epa submission *******************
    public class tbl_epa_submission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int submission_id { get; set; }
        [ForeignKey("comp_mstr")]
        public int? company_id { get; set; }
        public tbl_company_master comp_mstr { get; set; }
        [ForeignKey("tbl_employee_master")] // Foreign Key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }
        [ForeignKey("tbl_emp_officaial_sec")] // Foreign Key here
        public int? emp_off_id { get; set; }
        public tbl_emp_officaial_sec tbl_emp_officaial_sec { get; set; }
        [ForeignKey("tbl_epa_fiscal_yr_mstr")] // Foreign Key here
        public int? fiscal_year_id { get; set; }
        public tbl_epa_fiscal_yr_mstr tbl_epa_fiscal_yr_mstr { get; set; }
        public int cycle_id { get; set; }
        public string cycle_name { get; set; }
        [ForeignKey("tbl_designation_master")] // Foreign Key here
        public int? desig_id { get; set; }
        public tbl_designation_master tbl_designation_master { get; set; }
        [ForeignKey("tbl_department_master")] // Foreign Key here
        public int? department_id { get; set; }
        public tbl_department_master tbl_department_master { get; set; }
        public DateTime epa_start_date { get; set; }
        public DateTime epa_end_date { get; set; }
        public byte epa_close_status { get; set; }//0 for Not initiated, 1 for close, 2 for processing
        [ForeignKey("tbl_epa_status_master")] // Foreign Key here
        public int? epa_current_status { get; set; }
        public tbl_epa_status_master tbl_epa_status_master { get; set; }
        [ForeignKey("tbl_w_role")]
        public int? w_r_id { get; set; } // working_role_id
        public tbl_working_role tbl_w_role { get; set; }
        public int total_score { get; set; }
        public int get_score { get; set; }
        public byte final_review { get; set; }
        public string final_remarks { get; set; }
        public byte final_review_rm1 { get; set; }
        public string final_remarks_rm1 { get; set; }
        public byte final_review_rm2 { get; set; }
        public string final_remarks_rm2 { get; set; }
        public byte final_review_rm3 { get; set; }
        public string final_remarks_rm3 { get; set; }
        public int? rm_id1 { get; set; }
        public int? rm_id2 { get; set; }
        public int? rm_id3 { get; set; }
        public byte is_deleted { get; set; }
        public int user_id { get; set; }
        public DateTime submission_dt { get; set; }
        public ICollection<tbl_epa_kpi_submission> tbl_epa_kpi_submissions { get; set; }
        public ICollection<tbl_epa_kra_submission> tbl_epa_kra_submissions { get; set; }
        public ICollection<tbl_epa_question_submission> tbl_epa_question_submissions { get; set; }

    }

    public class tbl_epa_kpi_submission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int kpi_submission_id { get; set; }
        [ForeignKey("tbl_epa_submission")] //Foreign Key here
        public int? submission_id { get; set; }
        public tbl_epa_submission tbl_epa_submission { get; set; }

        [ForeignKey("tbl_kpi_key_area_master")] //Foreign Key here
        public int? key_area_id { get; set; }
        public tbl_kpi_key_area_master tbl_kpi_key_area_master { get; set; }

        public int self { get; set; }
        public int aggreed { get; set; }
        public int score { get; set; }
        public string comment { get; set; }
        public DateTime change_dt { get; set; }
        public byte is_deleted { get; set; }

        public ICollection<tbl_epa_kpi_criteria_submission> tbl_epa_kpi_criteria_submissions { get; set; }
    }
    public class tbl_epa_kpi_criteria_submission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int kpi_criteria_submission_id { get; set; }

        [ForeignKey("tbl_kpi_criteria_master")] //Foreign Key here
        public int? crit_id { get; set; }
        public tbl_kpi_criteria_master tbl_kpi_criteria_master { get; set; }

        [ForeignKey("teks")] //Foreign Key here
        public int? kpi_submission_id { get; set; }
        public tbl_epa_kpi_submission teks { get; set; }

    }
    public class tbl_epa_kra_submission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int kra_submission_id { get; set; }
        [ForeignKey("tbl_epa_submission")] // Foreign Key here
        public int? submission_id { get; set; }
        public tbl_epa_submission tbl_epa_submission { get; set; }
        [ForeignKey("tbl_kra_master")] // Foreign Key here
        public int? kra_id { get; set; }
        public tbl_kra_master tbl_kra_master { get; set; }
        [ForeignKey("tbl_kpi_rating_master")] // Foreign Key here
        public int? rating_id { get; set; }
        public tbl_kpi_rating_master tbl_kpi_rating_master { get; set; }
        public DateTime change_dt { get; set; }
        public byte is_deleted { get; set; }
    }
    public class tbl_epa_question_submission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int question_submission_id { get; set; }
        [ForeignKey("tbl_epa_submission")] // Foreign Key here
        public int? submission_id { get; set; }
        public tbl_epa_submission tbl_epa_submission { get; set; }

        [ForeignKey("tbl_tab_master")] // Foreign Key here
        public int? tab_id { get; set; }
        public tbl_tab_master tbl_tab_master { get; set; }
        [ForeignKey("tbl_question_master")] // Foreign Key here
        public int? question_id { get; set; }
        public tbl_question_master tbl_question_master { get; set; }
        public string question_answer { get; set; }
        public string remarks { get; set; }
        public DateTime change_dt { get; set; }
        public byte is_deleted { get; set; }
    }
    #endregion
}

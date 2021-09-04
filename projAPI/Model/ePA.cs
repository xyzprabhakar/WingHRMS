using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model
{
    public class ePA
    {
        public class ePa_FiscalYearMaster
        {
            public int fiscal_year_id { get; set; }
            public string financial_year_name { get; set; }
            public DateTime from_date { get; set; }
            public DateTime to_date { get; set; }
            public DateTime created_date { get; set; }
            public byte is_deleted { get; set; }
            public int user_id { get; set; }
            public int last_modified_by { get; set; }
            public DateTime last_modified_date { get; set; }
            public int isview { get; set; }

        }



        //public class epa_StatusMaster
        //{
        //    public int epa_status_id { get; set; }
        //    public int? company_id { get; set; }

        //    [MaxLength(50)]
        //    [StringLength(50)]
        //    [Display(Description = "status_name")]
        //    [RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Status Name")]
        //    public string status_name { get; set; }
        //    [MaxLength(500)]
        //    [StringLength(500)]
        //    [Display(Description = "description")]
        //    [RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Description")]
        //    public string description { get; set; }
        //    public List<object> display_for { get; set; }//1 For Reporting Manager(RM1),2 For RM2, 3 For RM3,4 For User

        //    [Display(Description = "display_order")]
        //    [RegularExpression(@"^[0-9]{1,40}$", ErrorMessage = "Invalid Display Order")]
        //    public int display_order { get; set; }//1 For Name,2 For Status,3 For Display For

        //    public int status { get; set; }// 0 For Start,1 For Closed,2 For Middle Level

        //    public byte is_deleted { get; set; }

        //    public int created_by { get; set; }
        //    public DateTime created_date { get; set; }
        //    public int last_modified_by { get; set; }
        //    public DateTime last_modified_date { get; set; }
        //}

        public class epa_TabMaster
        {
            public int tab_id { get; set; }
            public string tab_name { get; set; }
            public string description { get; set; }
            public string order { get; set; }
            public byte is_view { get; set; }
            public byte have_save_button { get; set; }
           
            public byte is_deleted { get; set; }
            public int user_id { get; set; }
            public DateTime created_date { get; set; }
            public int last_modified_by { get; set; }
            public DateTime last_modified_date { get; set; }
        }
        public class ePa_StatustabPermission
        {
            public int sno { get; set; }
            public int tab_id { get; set; }
            public int status_id { get; set; }
            public int isenabled { get; set; }
            public int user_id { get; set; }
            public DateTime created_date { get; set; }
            public int last_modified_by { get; set; }
            public DateTime last_modified_date { get; set; }
        }


        public class epa_status_flow_master
        {
            public int flow_mster_id { get; set; }
            public int start_status_id { get; set; }
            public List<object> next_status_id { get; set; }

            public string description { get; set; }
            public int is_deleted { get; set; }
            public int deleted_by { get; set; }
            public DateTime created_dt { get; set; }
            public int created_by { get; set; }
            public int modified_dt { get; set; }
            public DateTime modified_date { get; set; }

            public int company_id { get; set; }
        }




        public class kra_master
        {
           
            public int kra_mstr_id { get; set; }
            public int? company_id { get; set; }

            public string company_name { get; set; }
            public int? financial_yr { get; set; }
            public string financial_name { get; set; }
            public int cycle_id { get; set; } //if 0 is selected in tbl_epa_cycle_master mean Monthly than 1 to 12 any Month no is bind in cycle_id,if 1 means quaterly is selected than 1 to 4 any quater is no is bind in cycle_id etc. 

            public string cycle_name { get; set; }
            public int? department_id { get; set; }
            public string department_name { get; set; }
            public int? wrk_role_id { get; set; }
            public string work_role_name { get; set; }

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

            public int emp_id { get; set; }

            public int for_all_emp { get; set; }

            public int is_manager { get; set; }
        }

        public class workrolecomponent
        {
          public int companyid { get; set; }

            public int from_dept { get; set; }

            public int from_wrk_role { get; set; }
            public int to_dept { get; set; }

            public int to_wrk_role { get; set; }

            public int created_by { get; set; }
        }


        
    }
    #region ***********Epa Submission***********************

    public class mdlEpaSubmission
    {
        public int submission_id { get; set; }
        public int? company_id { get; set; }
        public int? emp_id { get; set; }
        public string emp_name { get; set; }
        public string emp_code { get; set; }
        public int? emp_off_id { get; set; }
        public DateTime joining_dt { get; set; }
        public int? fiscal_year_id { get; set; }
        public string fiscal_year_name { get; set; }
        public int cycle_id { get; set; }
        public string cycle_name { get; set; }
        public int? desig_id { get; set; }
        public string desig_name { get; set; }
        public int? department_id { get; set; }
        public string department_name { get; set; }
        public DateTime epa_start_date { get; set; }
        public DateTime epa_end_date { get; set; }
        public byte epa_close_status { get; set; }//0 for Not initiated, 1 for close, 2 for processing            
        public string epa_close_status_name { get; set; }
        public int? epa_current_status { get; set; }
        public string epa_current_status_name { get; set; }
        public int? w_r_id { get; set; } // working_role_id
        public string w_r_name { get; set; }
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
        public string rm1_name { get; set; }
        public string rm2_name { get; set; }
        public string rm3_name { get; set; }        
        public List<mdlEpaKPIDetail> mdlEpaKPIDetails { get; set; }
        public List<mdlEpaKRADetail> mdlEpaKRADetails { get; set; }
        public List<mdlEpaTabQuestion> mdlEpaTabQuestions { get; set; }
        public int user_id { get; set; }

        public List<EPAStatus> mdlEPANextStatusDetails { get; set; }
    }
    public class mdlEpaKPIDetail
    {
        public int kpi_submission_id { get; set; }
        public int key_area_id { get; set; }
        public int? otype_id { get; set; }
        public string objective_name { get; set; }
        public string key_area { get; set; }
        public string expected_result { get; set; }
        public int wtg { get; set; }
        public int self { get; set; }
        public int aggreed { get; set; }
        public int score { get; set; }
        public string comment { get; set; }
        public byte is_deleted { get; set; }
        public List<mdlEpaKPICriteriaDetail> mdlEpaKPICriteriaDetails { get; set; }
    }
    public class mdlEpaKPICriteriaDetail
    {
        public int kpi_criteria_submission_id { get; set; }
        public int crit_number { get; set; }
        public int? crit_id { get; set; }
        public string criteria_name { get; set; }
    }
    public class mdlEpaKRADetail
    {
        public int kra_submission_id { get; set; }
        public int? kra_id { get; set; }
        public string description { get; set; }
        public string factor_result { get; set; }
        public int? rating_id { get; set; }
        public string rating_name { get; set; }
        
    }
    public class mdlEpaTabQuestion
    {
        public int question_submission_id { get; set; }

        public int? tab_id { get; set; }
        public string tab_name { get; set; }
        public int? question_id { get; set; }
        public string ques { get; set; }
        public int ans_type { get; set; }
        public string ans_type_ddl { get; set; }
        public string question_answer { get; set; }
        public string remarks { get; set; }
        
        public int? dept_id { get; set; }

        public int? wrk_role_id { get; set; }

        public int? company_id { get; set; }

        public string description { get; set; }

        public int created_by { get; set; }

        public int emp_id { get; set; }

        public int for_all_emp { get; set; }

        public int is_manager { get; set; }
    }
    #endregion


    public class EPAStatus
    {
        public int current_status_id { get; set; }

        public string current_status_name { get; set; }

        public int next_status_id { get; set; }

        public string next_Status_name { get; set; }
    }
}

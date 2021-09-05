using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using projContext.DB;

namespace projContext
{
    public partial class Context : DbContext
    {
        /// <summary>
        /// Devlopment Server
        /// </summary>
        public string _connectionString = "Data Source = localhost; port = 3306; Initial Catalog = db_hrms; User ID = root; Password = DHRUV@123;Allow User Variables=True ; MaximumPoolsize=5000;Convert Zero Datetime=True;Default Command Timeout=600;";

        /// <summary>
        /// QA Server
        /// </summary>
        //public string _connectionString = "Data Source = 192.168.10.6; port = 3306; Initial Catalog = db_hrms_glaze_05_11_20; User ID = sa; Password = glaze@123;Allow User Variables=True ; Use Default Command Timeout For EF=true;Default Command Timeout=600;MaximumPoolsize=5000;Convert Zero Datetime=True;";
        //public string _connectionString = "Data Source = 192.168.10.6; port = 3306; Initial Catalog = db_hrms_demo1; User ID = sa; Password = glaze@123;Allow User Variables=True ; Use Default Command Timeout For EF=true;Default Command Timeout=600;MaximumPoolsize=5000;Convert Zero Datetime=True;";

        /// <summary>
        /// Live Server
        /// </summary>
        // public string _connectionString = "Data Source = 192.168.10.26; port = 3306; Initial Catalog =db_hrms_glaze; User ID = sa; Password = glaze@123;Allow User Variables=True ; Use Default Command Timeout For EF=true;Default Command Timeout=600;MaximumPoolsize=5000;Convert Zero Datetime=True; ";

        // public string _connectionString = "Data Source = localhost; port = 3306; Initial Catalog = db_hrms_glaze; User ID = root; Password = glaze@123;Allow User Variables=True ; ";
        //string _connectionString = "";
        public Context() : base()
        {
            Database.SetCommandTimeout((int)TimeSpan.FromMinutes(30).TotalSeconds);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(_connectionString, opt => opt.CommandTimeout(600));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbl_daily_attendance>()
                .HasKey(c => new { c.emp_id, c.attendance_dt });
            //Some Unique key Validation 
            modelBuilder.Entity<tbl_employee_master>().HasIndex(c => new { c.emp_code }).IsUnique();
            modelBuilder.Entity<tbl_component_formula_details>().HasOne<tbl_component_master>(s => s.comp_master).WithMany().HasConstraintName("FK_tcpd_component_id");
            modelBuilder.Entity<tbl_component_formula_details>().HasOne<tbl_salary_group>(s => s.salary_group).WithMany().HasConstraintName("FK_tcpd_tsg_salarygroup_id");
            modelBuilder.Entity<tbl_muster_form10>().HasOne<tbl_component_master>(s => s.mb_component_mstr).WithMany().HasConstraintName("FK_tblmst_tcpd_mb");
            modelBuilder.Entity<tbl_muster_form10>().HasOne<tbl_component_master>(s => s.mda_component_mstr).WithMany().HasConstraintName("FK_tblmst_tcpd_mda");
            modelBuilder.Entity<tbl_muster_form10>().HasOne<tbl_component_master>(s => s.ab_component_mstr).WithMany().HasConstraintName("FK_tblmst_tcpd_ab");
            modelBuilder.Entity<tbl_muster_form10>().HasOne<tbl_component_master>(s => s.ada_component_mstr).WithMany().HasConstraintName("FK_tblmst_tcpd_ada");
            modelBuilder.Entity<tbl_muster_form10>().HasOne<tbl_component_master>(s => s.ta_component_mstr).WithMany().HasConstraintName("FK_tblmst_tcpd_ta");
            modelBuilder.Entity<tbl_muster_form10>().HasOne<tbl_component_master>(s => s.ow_component_mstr).WithMany().HasConstraintName("FK_tblmst_tcpd_ow");
            modelBuilder.Entity<tbl_muster_form10>().HasOne<tbl_component_master>(s => s.gw_component_mstr).WithMany().HasConstraintName("FK_tblmst_tcpd_gw");
            modelBuilder.Entity<tbl_muster_form10>().HasOne<tbl_component_master>(s => s.epf_component_mstr).WithMany().HasConstraintName("FK_tblmst_tcpd_epf");
            modelBuilder.Entity<tbl_muster_form10>().HasOne<tbl_component_master>(s => s.hrd_component_mstr).WithMany().HasConstraintName("FK_tblmst_tcpd_hrd");
            modelBuilder.Entity<tbl_muster_form10>().HasOne<tbl_component_master>(s => s.od_component_mstr).WithMany().HasConstraintName("FK_tblmst_tcpd_od");
            modelBuilder.Entity<tbl_muster_form10>().HasOne<tbl_component_master>(s => s.td_component_mstr).WithMany().HasConstraintName("FK_tblmst_tcpd_td");
            modelBuilder.Entity<tbl_muster_form10>().HasOne<tbl_component_master>(s => s.wp_component_mstr).WithMany().HasConstraintName("FK_tblmst_tcpd_wp");
            modelBuilder.Entity<tbl_muster_form10>().HasOne<tbl_component_master>(s => s.dop_component_mstr).WithMany().HasConstraintName("FK_tblmst_tcpd_dop");
            modelBuilder.Entity<tbl_muster_form11>().HasOne<tbl_component_master>(s => s.ta_component_mstr).WithMany().HasConstraintName("FK_tblmst11_tcpd_ta");
            modelBuilder.Entity<tbl_muster_form2>().HasOne<tbl_component_master>(s => s.dmg_component_mstr).WithMany().HasConstraintName("FK_tblmst2_tcpd_dmg");
            modelBuilder.Entity<tbl_muster_form3>().HasOne<tbl_component_master>(s => s.re_component_mstr).WithMany().HasConstraintName("FK_tblmst3_tcpd_re");
            modelBuilder.Entity<tbl_muster_form3>().HasOne<tbl_component_master>(s => s.in_component_mstr).WithMany().HasConstraintName("FK_tblmst3_tcpd_in");
            modelBuilder.Entity<tbl_muster_form3>().HasOne<tbl_component_master>(s => s.pg_component_mstr).WithMany().HasConstraintName("FK_tblmst3_tcpd_pg");
            modelBuilder.Entity<tbl_muster_form4>().HasOne<tbl_component_master>(s => s.dop_component_mstr).WithMany().HasConstraintName("FK_tblmst4_tcpd_dop");
            modelBuilder.Entity<tbl_emp_working_role_allocation>().HasOne<tbl_employee_master>(s => s.tem).WithMany().HasConstraintName("FK_tewp_tem_emp_id");

            modelBuilder.Entity<tbl_epa_kpi_submission>().HasMany(c => c.tbl_epa_kpi_criteria_submissions).WithOne(e => e.teks).HasConstraintName("FK_tblepa_submissionid");
            //modelBuilder.Entity<tbl_epa_kpi_submission>().HasMany<tbl_epa_kpi_criteria_submission>(s => s.tbl_epa_kpi_criteria_submissions).WithOne().HasConstraintName("FK_tblepa_submissionid");
            //modelBuilder.Entity<tbl_epa_kpi_criteria_submission>().HasOne<tbl_epa_kpi_submission>(s => s.teks).WithMany().HasConstraintName("FK_tblepa_submissionid1");
            modelBuilder.Entity<tbl_epa_kpi_criteria_submission>().HasOne<tbl_kpi_criteria_master>(s => s.tbl_kpi_criteria_master).WithMany().HasConstraintName("FK_tblepa_critid");
            modelBuilder.Entity<tbl_user_login_logs>().HasIndex(b => b.login_date_time);

            modelBuilder.Entity<tbl_emp_bank_details>().HasIndex(c => new { c.bank_acc }).IsUnique().HasFilter("[is_deleted] = 0");
            modelBuilder.Entity<tbl_emp_esic_details>().HasIndex(c => new { c.esic_number }).IsUnique().HasFilter("[is_deleted] = 0");
            modelBuilder.Entity<tbl_emp_pf_details>().HasIndex(c => new { c.pf_number }).IsUnique().HasFilter("[is_deleted] = 0");
            //SetPayrolldata(modelBuilder);
            //SetRoleMaster(modelBuilder);


            //SetClaimMaster(modelBuilder);
            //SetClaimRolemaping(modelBuilder);


            //Default Data
            InsertCountryMaster(modelBuilder);
        }

        public DbSet<tbl_guid_detail> tbl_guid_detail { get; set; }
        public DbSet<tbl_captcha_code_details> tbl_captcha_code_details { get; set; }
        public DbSet<tbl_user_master> tbl_user_master { get; set; }        
        public DbSet<tbl_No_dues_particular_master> tbl_No_dues_particular_master { get; set; }
        public DbSet<tbl_No_dues_particular_responsible> tbl_No_dues_particular_responsible { get; set; }
        public DbSet<tbl_No_dues_clearance_Department> tbl_No_dues_clearance_Department { get; set; }
        public DbSet<tbl_No_dues_emp_particular_Clearence_detail> tbl_No_dues_emp_particular_Clearence_detail { get; set; }
        public DbSet<tbl_role_master> tbl_role_master { get; set; }
        public DbSet<tbl_claim_master> tbl_claim_master { get; set; }
        public DbSet<tbl_role_claim_map> tbl_role_claim_map { get; set; }
        public DbSet<tbl_user_role_map> tbl_user_role_map { get; set; }
        public DbSet<tbl_employee_master> tbl_employee_master { get; set; }
        public DbSet<tbl_employee_company_map> tbl_employee_company_map { get; set; }
        public DbSet<tbl_emp_shift_allocation> tbl_emp_shift_allocation { get; set; }
        public DbSet<tbl_emp_manager> tbl_emp_manager { get; set; }
        public DbSet<tbl_emp_grade_allocation> tbl_emp_grade_allocation { get; set; }
        public DbSet<tbl_emp_desi_allocation> tbl_emp_desi_allocation { get; set; }
        public DbSet<tbl_emp_officaial_sec> tbl_emp_officaial_sec { get; set; }
        public DbSet<tbl_emp_weekoff> tbl_emp_weekoff { get; set; }        
        public DbSet<tbl_emp_family_sec> tbl_emp_family_sec { get; set; }
        public DbSet<tbl_emp_bank_details> tbl_emp_bank_details { get; set; }
        public DbSet<tbl_emp_pan_details> tbl_emp_pan_details { get; set; }
        public DbSet<tbl_emp_adhar_details> tbl_emp_adhar_details { get; set; }
        public DbSet<tbl_emp_pf_details> tbl_emp_pf_details { get; set; }
        public DbSet<tbl_emp_esic_details> tbl_emp_esic_details { get; set; }
        public DbSet<tbl_emp_personal_sec> tbl_emp_personal_sec { get; set; }
        public DbSet<tbl_emp_qualification_sec> tbl_emp_qualification_sec { get; set; }
        public DbSet<tbl_employment_type_master> tbl_employment_type_master { get; set; }
        public DbSet<tbl_role_master_log> tbl_role_master_log { get; set; }
        public DbSet<tbl_claim_master_log> tbl_claim_master_log { get; set; }
        public DbSet<tbl_employee_company_map_log> tbl_employee_company_map_log { get; set; }
        public DbSet<tbl_company_master> tbl_company_master { get; set; }
        public DbSet<tbl_company_emp_setting> tbl_company_emp_setting { get; set; }
        public DbSet<tbl_company_master_log> tbl_company_master_log { get; set; }
        public DbSet<tbl_payroll_month_setting> tbl_payroll_month_setting { get; set; }
        public DbSet<tbl_country> tbl_country { get; set; }
        public DbSet<tbl_state> tbl_state { get; set; }
        public DbSet<tbl_city> tbl_city { get; set; }
        public DbSet<tbl_location_master> tbl_location_master { get; set; }
        public DbSet<tbl_sub_location_master> tbl_sub_location_master { get; set; }
        public DbSet<tbl_department_master> tbl_department_master { get; set; }
        public DbSet<tbl_sub_department_master> tbl_sub_department_master { get; set; }
        public DbSet<tbl_designation_master> tbl_designation_master { get; set; }
        public DbSet<tbl_grade_master> tbl_grade_master { get; set; }
        public DbSet<tbl_religion_master> tbl_religion_master { get; set; }
        public DbSet<tbl_machine_master> tbl_machine_master { get; set; }
        public DbSet<tbl_ot_rule_master> tbl_ot_rule_master { get; set; }
        public DbSet<tbl_comb_off_rule_master> tbl_comb_off_rule_master { get; set; }
        public DbSet<tbl_holiday_master> tbl_holiday_master { get; set; }
        public DbSet<tbl_policy_master> tbl_policy_master { get; set; }
        public DbSet<tbl_current_openings> tbl_current_openings { get; set; }
        public DbSet<tbl_policy_master_documents> tbl_policy_master_documents { get; set; }
        public DbSet<tbl_app_setting> tbl_app_setting { get; set; }
        public DbSet<tbl_holiday_master_comp_list> tbl_holiday_master_comp_list { get; set; }
        public DbSet<tbl_holiday_master_emp_list> tbl_holiday_master_emp_list { get; set; }
        public DbSet<tbl_holiday_mstr_rel_list> tbl_holiday_mstr_rel_list { get; set; }
        public DbSet<tbl_pay_set_log> tbl_pay_set_log { get; set; }
        public DbSet<tbl_location_master_log> tbl_location_master_log { get; set; }
        public DbSet<tbl_department_master_log> tbl_department_master_log { get; set; }
        public DbSet<tbl_sub_department_master_log> tbl_sub_department_master_log { get; set; }
        public DbSet<tbl_designation_log> tbl_designation_log { get; set; }
        public DbSet<tbl_grade_master_log> tbl_grade_master_log { get; set; }
        public DbSet<tbl_religion_master_log> tbl_religion_master_log { get; set; }
        public DbSet<tbl_machine_master_log> tbl_machine_master_log { get; set; }
        public DbSet<tbl_ot_rule_master_log> tbl_ot_rule_master_log { get; set; }
        public DbSet<tbl_comb_off_log> tbl_comb_off_log { get; set; }
        public DbSet<tbl_holiday_master_log> tbl_holiday_master_log { get; set; }
        public DbSet<tbl_holi_comp_list_log> tbl_holi_comp_list_log { get; set; }
        public DbSet<tbl_holyd_emp_list_log> tbl_holyd_emp_list_log { get; set; }
        public DbSet<tbl_holiday_rel_list_log> tbl_holiday_rel_list_log { get; set; }
        public DbSet<tbl_shift_master> tbl_shift_master { get; set; }
        public DbSet<tbl_shift_week_off> tbl_shift_week_off { get; set; }
        public DbSet<tbl_shift_master_log> tbl_shift_master_log { get; set; }
        public DbSet<tbl_shift_week_off_log> tbl_shift_week_off_log { get; set; }
        public DbSet<tbl_leave_type> tbl_leave_type { get; set; }
        public DbSet<tbl_leave_applicablity> tbl_leave_applicablity { get; set; }
        public DbSet<tbl_leave_app_on_emp_type> tbl_leave_app_on_emp_type { get; set; }
        public DbSet<tbl_leave_appcbl_on_company> tbl_leave_appcbl_on_company { get; set; }
        public DbSet<tbl_leave_app_on_dept> tbl_leave_app_on_dept { get; set; }
        public DbSet<tbl_leave_appcbl_on_religion> tbl_leave_appcbl_on_religion { get; set; }
        public DbSet<tbl_leave_credit> tbl_leave_credit { get; set; }
        public DbSet<tbl_leave_rule> tbl_leave_rule { get; set; }
        public DbSet<tbl_leave_cashable> tbl_leave_cashable { get; set; }
        public DbSet<tbl_leave_type_log> tbl_leave_type_log { get; set; }
        public DbSet<tbl_leave_info> tbl_leave_info { get; set; }
        public DbSet<tbl_leave_info_log> tbl_leave_info_log { get; set; }
        public DbSet<tbl_leave_applicablity_log> tbl_leave_applicablity_log { get; set; }
        public DbSet<tbl_leave_app_emp_type_log> tbl_leave_app_emp_type_log { get; set; }
        public DbSet<tbl_leave_appcbly_on_comp_log> tbl_leave_appcbly_on_comp_log { get; set; }
        public DbSet<tbl_leave_appcbl_on_dept_log> tbl_leave_appcbl_on_dept_log { get; set; }
        public DbSet<tbl_leave_appcbl_rel_log> tbl_leave_appcbl_rel_log { get; set; }
        public DbSet<tbl_leave_credit_log> tbl_leave_credit_log { get; set; }
        public DbSet<tbl_leave_rule_log> tbl_leave_rule_log { get; set; }
        public DbSet<tbl_leave_cashable_log> tbl_leave_cashable_log { get; set; }
        public DbSet<tbl_shift_location> tbl_shift_location { get; set; }
        public DbSet<tbl_shift_department> tbl_shift_department { get; set; }
        public DbSet<tbl_shift_details> tbl_shift_details { get; set; }
        public DbSet<tbl_shift_roster_master> tbl_shift_roster_master { get; set; }
        public DbSet<tbl_shift_roster_master_log> tbl_shift_roster_master_log { get; set; }
        public DbSet<tbl_attendance_details_manual> tbl_attendance_details_manual { get; set; }
        public DbSet<tbl_attendance_details> tbl_attendance_details { get; set; }
        public DbSet<tbl_daily_attendance> tbl_daily_attendance { get; set; }
        public DbSet<tbl_comp_off_ledger> tbl_comp_off_ledger { get; set; }
        public DbSet<tbl_leave_ledger> tbl_leave_ledger { get; set; }
        public DbSet<tbl_comp_off_request_master> tbl_comp_off_request_master { get; set; }
        public DbSet<tbl_leave_request> tbl_leave_request { get; set; }
        public DbSet<tbl_attendace_request> tbl_attendace_request { get; set; }
        public DbSet<tbl_outdoor_request> tbl_outdoor_request { get; set; }
        public DbSet<tbl_attendance_details_log> tbl_attendance_details_log { get; set; }
        public DbSet<tbl_comp_off_ledger_log> tbl_comp_off_ledger_log { get; set; }
        public DbSet<tbl_leave_ledger_log> tbl_leave_ledger_log { get; set; }
        public DbSet<tbl_emp_working_role_allocation> tbl_emp_working_role_allocation { get; set; }

        /// <summary>
        /// //////////////////////   EPA
        /// </summary>
        public DbSet<tbl_epa_fiscal_yr_mstr> tbl_epa_fiscal_yr_mstr { set; get; }
        public DbSet<tbl_epa_cycle_master> tbl_epa_cycle_master { set; get; }
        public DbSet<tbl_working_role> tbl_working_role { set; get; }
        public DbSet<tbl_kpi_criteria> tbl_kpi_criteria { set; get; }
        public DbSet<tbl_kpi_key_area_master> tbl_kpi_key_area_master { set; get; }
        public DbSet<tbl_kpi_criteria_master> tbl_kpi_criteria_master { set; get; }


        // public DbSet<tbl_epa_tab_mstr> tbl_epa_tab_mstr { get; set; }
        //public DbSet<tbl_epa_status> tbl_epa_status { get; set; }
        //public DbSet<tbl_epa_status_tab_prmsn> tbl_epa_status_tab_prmsn { get; set; }
        //public DbSet<tbl_epa_status_quespermsn> tbl_epa_status_quespermsn { get; set; }
        //public DbSet<tbl_epa_ques_mstr> tbl_epa_ques_mstr { get; set; }
        //public DbSet<tbl_epa_ques_mltpl_answr> tbl_epa_ques_mltpl_answr { get; set; }
        //public DbSet<tbl_epa_quarter_master> tbl_epa_quarter_master { get; set; }
        //public DbSet<tbl_epa_quarter_submsn_mstr> tbl_epa_quarter_submsn_mstr { get; set; }
        //public DbSet<tbl_epa_quarter_submsn_dtl> tbl_epa_quarter_submsn_dtl { get; set; }

        public DbSet<tbl_salary_group> tbl_salary_group { get; set; }
        public DbSet<tbl_sg_maping> tbl_sg_maping { get; set; }
        public DbSet<tbl_loan_request> tbl_loan_request { get; set; }
        public DbSet<tbl_component_master> tbl_component_master { get; set; }
        public DbSet<tbl_component_formula_details> tbl_component_formula_details { get; set; }
        public DbSet<tbl_tax_slab_master> tbl_tax_slab_master { get; set; }
        public DbSet<tbl_tax_slab_Details> tbl_tax_slab_Details { get; set; }
        public DbSet<tbl_salary_input> tbl_salary_input { get; set; }
        public DbSet<tbl_salary_input_change> tbl_salary_input_change { get; set; }
        public DbSet<log_tbl_salary_input> log_tbl_salary_input { get; set; }
        public DbSet<tbl_emp_salary_master> tbl_emp_salary_master { get; set; }
        public DbSet<tbl_ot_master> tbl_ot_master { get; set; }
        public DbSet<tbl_loan_request_master> tbl_loan_request_master { get; set; }
        public DbSet<tbl_loan_approval> tbl_loan_approval { get; set; }
        public DbSet<tbl_loan_repayments> tbl_loan_repayments { get; set; }
        public DbSet<mdlSalaryInputValues> mdlSalaryInputValues { get; set; }
        public DbSet<EmployeeBasicDataProc> EmployeeBasicDataProc { get; set; }

        public DbSet<tbl_remimb_cat_mstr> tbl_remimb_cat_mstr { get; set; }
        public DbSet<tbl_rimb_grd_lmt_mstr> tbl_rimb_grd_lmt_mstr { get; set; }
        public DbSet<tbl_rimb_cat_lmt_mstr> tbl_rimb_cat_lmt_mstr { get; set; }
        public DbSet<tbl_rimb_req_mstr> tbl_rimb_req_mstr { get; set; }
        public DbSet<tbl_rimb_req_details> tbl_rimb_req_details { get; set; }
        public DbSet<tbl_employee_income_tax_amount> tbl_employee_income_tax_amount { get; set; }
        public DbSet<tbl_menu_master> tbl_menu_master { get; set; }
        public DbSet<tbl_role_menu_master> tbl_role_menu_master { get; set; }
        public DbSet<tbl_payroll_process_status> tbl_payroll_process_status { get; set; }

        public DbSet<tbl_lossofpay_setting> tbl_lossofpay_setting { get; set; }
        public DbSet<tbl_lossofpay_master> tbl_lossofpay_master { get; set; }
        public DbSet<tbl_lop_detail> tbl_lop_detail { get; set; }

        public DbSet<tbl_ot_rate_details> tbl_ot_rate_details { get; set; }

        public DbSet<tbl_loan_approval_setting> tbl_loan_approval_setting { get; set; }

        public DbSet<tbl_health_card_master> tbl_health_card_master { get; set; }

        public DbSet<tbl_assets_request_master> tbl_assets_request_master { get; set; }

        public DbSet<tbl_assets_approval> tbl_assets_approval { get; set; }

        public DbSet<tbl_event_master> tbl_event_master { get; set; }

        public DbSet<tbl_right_menu_link> tbl_right_menu_link { get; set; }
        public DbSet<tbl_muster_form1> tbl_muster_form1 { get; set; }

        public DbSet<tbl_muster_form1_data> tbl_muster_form1_data { get; set; }
        public DbSet<tbl_muster_form2> tbl_muster_form2 { get; set; }

        public DbSet<tbl_muster_form2_data> tbl_muster_form2_data { get; set; }

        public DbSet<tbl_muster_form3> tbl_muster_form3 { get; set; }

        public DbSet<tbl_muster_form3_data> tbl_muster_form3_data { get; set; }

        public DbSet<tbl_muster_form4> tbl_muster_form4 { get; set; }

        public DbSet<tbl_muster_form4_data> tbl_muster_form4_data { get; set; }

        public DbSet<tbl_muster_form10> tbl_muster_form10 { get; set; }

        public DbSet<tbl_muster_form10_data> tbl_muster_form10_data { get; set; }

        public DbSet<tbl_muster_form11> tbl_muster_form11 { get; set; }

        public DbSet<tbl_muster_form11_data> tbl_muster_form11_data { get; set; }

        public DbSet<tbl_user_login_logs> tbl_user_login_logs { get; set; }

        public DbSet<tbl_active_inactive_user_log> tbl_active_inactive_user_log { get; set; }

        public DbSet<tbl_document_type_master> tbl_document_type_master { get; set; }

        public DbSet<tbl_emp_documents> tbl_emp_documents { get; set; }

        public DbSet<tbl_employeementtype_settings> tbl_employeementtype_settings { get; set; }


        public DbSet<tbl_compoff_raise> tbl_compoff_raise { get; set; }
        public DbSet<tbl_scheduler_master> tbl_scheduler_master { get; set; }
        public DbSet<tbl_scheduler_details> tbl_scheduler_details { get; set; }

        public DbSet<tbl_assets_master> tbl_assets_master { get; set; }


        public DbSet<tbl_epa_status_master> tbl_epa_status_master { get; set; }

        public DbSet<tbl_kpi_objective_type> tbl_kpi_objective_type { get; set; }

        public DbSet<tbl_status_flow_master> tbl_status_flow_master { get; set; }

        public DbSet<tbl_kpi_rating_master> tbl_kpi_rating_master { get; set; }

        public DbSet<tbl_kra_master> tbl_kra_master { get; set; }

        public DbSet<tbl_tab_master> tbl_tab_master { get; set; }

        public DbSet<tbl_question_master> tbl_question_master { get; set; }

        public DbSet<tbl_epa_submission> tbl_epa_submission { get; set; }
        //public DbSet<tbl_epa_submission_status_log> tbl_epa_submission_status_log { get; set; }
        public DbSet<tbl_epa_kpi_submission> tbl_epa_kpi_submission { get; set; }
        public DbSet<tbl_epa_kpi_criteria_submission> tbl_epa_kpi_criteria_submission { get; set; }
        public DbSet<tbl_epa_kra_submission> tbl_epa_kra_submission { get; set; }
        public DbSet<tbl_epa_question_submission> tbl_epa_question_submission { get; set; }

        public DbSet<tbl_report_master> tbl_report_master { get; set; }
        public DbSet<tbl_rpt_title_master> tbl_rpt_title_master { get; set; }

        public DbSet<tbl_bank_master> tbl_bank_master { get; set; }

        public DbSet<tbl_emp_separation> tbl_emp_separation { get; set; }
        public DbSet<tbl_approved_emp_separation_cancellation> tbl_approved_emp_separation_cancellation { get; set; }

        public DbSet<tbl_emp_prev_employement> tbl_emp_prev_employement { get; set; }

        public DbSet<tbl_emp_withdrawal> tbl_emp_withdrawal { get; set; }

        public DbSet<tbl_emp_fnf_asset> tbl_emp_fnf_asset { get; set; }

        public DbSet<tbl_fnf_reimburesment> tbl_fnf_reimburesment { get; set; }

        public DbSet<tbl_fnf_leave_encash> tbl_fnf_leave_encash { get; set; }

        public DbSet<tbl_fnf_attendance_dtl> tbl_fnf_attendance_dtl { get; set; }

        public DbSet<tbl_fnf_component> tbl_fnf_component { get; set; }

        public DbSet<tbl_fnf_loan_recover> tbl_fnf_loan_recover { get; set; }

        public DbSet<tbl_fnf_master> tbl_fnf_master { get; set; }

        public DbSet<tbl_kt_task_master> tbl_kt_task_master { get; set; }

        public DbSet<tbl_kt_task_emp_details> tbl_kt_task_emp_details { get; set; }

        public DbSet<tbl_kt_status> tbl_kt_status { get; set; }

        public DbSet<tbl_kt_file> tbl_kt_file { get; set; }

        public void SetPayrolldata(ModelBuilder modelBuilder)
        {
            List<tbl_component_master> component_Masters = new List<tbl_component_master>();
            List<tbl_component_formula_details> Formula_Details = new List<tbl_component_formula_details>();

            #region
            component_Masters.Add(new tbl_component_master()
            {
                component_id = 1,
                component_name = "@SystemComponent",
                datatype = "3",
                defaultvalue = "-",
                parentid = 0,
                is_system_key = 1,
                System_function = "",
                System_table = null,
                component_type = (int)enmComponentType.Other,
                is_salary_comp = 0,
                is_tds_comp = 0,
                is_data_entry_comp = 0,
                payment_type = 0,
                is_user_interface = 0,
                is_payslip = 0,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                modified_by = 1,
                modified_dt = new DateTime(2020, 1, 1),
                is_active = 1,
                property_details = "System Component"
            });
            component_Masters.Add(new tbl_component_master()
            {
                component_id = 1000,
                component_name = "@EmployeeSalary",
                datatype = "3",
                defaultvalue = "-",
                parentid = 0,
                is_system_key = 1,
                System_function = "",
                System_table = null,
                component_type = (int)enmComponentType.Other,
                is_salary_comp = 0,
                is_tds_comp = 0,
                is_data_entry_comp = 0,
                payment_type = 0,
                is_user_interface = 0,
                is_payslip = 0,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                modified_by = 1,
                modified_dt = new DateTime(2020, 1, 1),
                is_active = 1,
                property_details = "Employee Salary"
            });
            component_Masters.Add(new tbl_component_master()
            {
                component_id = 2000,
                component_name = "@EmployeeIncome",
                datatype = "3",
                defaultvalue = "0",
                parentid = 1000,
                is_system_key = 1,
                System_function = "",
                System_table = null,
                component_type = (int)enmComponentType.Other,
                is_salary_comp = 0,
                is_tds_comp = 0,
                is_data_entry_comp = 0,
                payment_type = 0,
                is_user_interface = 0,
                is_payslip = 0,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                modified_by = 1,
                modified_dt = new DateTime(2020, 1, 1),
                is_active = 1,
                property_details = "Employee Income"
            });
            component_Masters.Add(new tbl_component_master()
            {
                component_id = 3000,
                component_name = "@EmployeeDeduction",
                datatype = "3",
                defaultvalue = "0",
                parentid = 1000,
                is_system_key = 1,
                System_function = "",
                System_table = null,
                component_type = (int)enmComponentType.Other,
                is_salary_comp = 0,
                is_tds_comp = 0,
                is_data_entry_comp = 0,
                payment_type = 0,
                is_user_interface = 0,
                is_payslip = 0,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                modified_by = 1,
                modified_dt = new DateTime(2020, 1, 1),
                is_active = 1,
                property_details = "Employee Deduction"
            });

            Formula_Details.Add(new tbl_component_formula_details()
            {
                sno = 1,
                component_id = 1,
                company_id = 1,
                salary_group_id = 1,
                formula = "0",
                function_calling_order = 1,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                deleted_by = 1,
                deleted_dt = new DateTime(2020, 1, 1),
                is_deleted = 1,

            });
            Formula_Details.Add(new tbl_component_formula_details()
            {
                sno = 1000,
                component_id = 1000,
                company_id = 1,
                salary_group_id = 1,
                formula = "0",
                function_calling_order = 1,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                deleted_by = 1,
                deleted_dt = new DateTime(2020, 1, 1),
                is_deleted = 1,

            });
            Formula_Details.Add(new tbl_component_formula_details()
            {
                sno = 2000,
                component_id = 2000,
                company_id = 1,
                salary_group_id = 1,
                formula = "0",
                function_calling_order = 1,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                deleted_by = 1,
                deleted_dt = new DateTime(2020, 1, 1),
                is_deleted = 1,

            });
            Formula_Details.Add(new tbl_component_formula_details()
            {
                sno = 3000,
                component_id = 3000,
                company_id = 1,
                salary_group_id = 1,
                formula = "0",
                function_calling_order = 1,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                deleted_by = 1,
                deleted_dt = new DateTime(2020, 1, 1),
                is_deleted = 1,

            });


            foreach (enmOtherComponent _enm in Enum.GetValues(typeof(enmOtherComponent)))
            {
                PayrollComponent payrollComponent = _enm.GetComponentDetails();
                component_Masters.Add(new tbl_component_master()
                {
                    component_id = (int)_enm,
                    component_name = "@" + _enm,
                    datatype = payrollComponent.datatype.ToString(),
                    defaultvalue = payrollComponent.defaultvalue,
                    parentid = payrollComponent.parentid,
                    is_system_key = payrollComponent.is_system_key,
                    System_function = payrollComponent.System_function,
                    System_table = null,
                    component_type = (int)payrollComponent.ComponentType,
                    is_salary_comp = payrollComponent.is_salary_comp,
                    is_tds_comp = 0,
                    is_data_entry_comp = payrollComponent.is_data_entry_comp,
                    payment_type = 0,
                    is_user_interface = payrollComponent.is_user_interface,
                    is_payslip = payrollComponent.is_payslip,
                    created_by = 1,
                    created_dt = new DateTime(2020, 1, 1),
                    modified_by = 1,
                    modified_dt = new DateTime(2020, 1, 1),
                    is_active = 1,
                    property_details = payrollComponent.component_name
                });
                Formula_Details.Add(new tbl_component_formula_details()
                {
                    sno = (int)_enm,
                    component_id = (int)_enm,
                    company_id = 1,
                    salary_group_id = 1,
                    formula = payrollComponent.formula,
                    function_calling_order = payrollComponent.function_calling_order,
                    created_by = 1,
                    created_dt = new DateTime(2020, 1, 1),
                    deleted_by = 1,
                    deleted_dt = new DateTime(2020, 1, 1),
                    is_deleted = 0,

                });

            }


            modelBuilder.Entity<tbl_component_master>().HasData(component_Masters);
            modelBuilder.Entity<tbl_component_formula_details>().HasData(Formula_Details);

            #endregion

            #region **************** Salary Report ***************************

            List<tbl_report_master> tbl_Report_Masters = new List<tbl_report_master>();
            tbl_Report_Masters.Add(new tbl_report_master() { rpt_id = 1, rpt_name = "Salary Report(Arrear)", rpt_description = "salary Report with arrear", is_active = 1, created_by = 1, created_date = new DateTime(2020, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2020, 1, 1) });
            tbl_Report_Masters.Add(new tbl_report_master() { rpt_id = 2, rpt_name = "Salary Report", rpt_description = "salary Report", is_active = 1, created_by = 1, created_date = new DateTime(2020, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2020, 1, 1) });
            modelBuilder.Entity<tbl_report_master>().HasData(tbl_Report_Masters);



            List<tbl_rpt_title_master> tbl_rpt_title_masters = new List<tbl_rpt_title_master>();
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 1, component_id = (int)enmOtherComponent.Gender, rpt_title = "Gender", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 1, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 2, component_id = (int)enmOtherComponent.EmpFatherHusbandName, rpt_title = "Father Husband Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 2, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 3, component_id = (int)enmOtherComponent.EmpDOB, rpt_title = "Date of Birth", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 3, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 4, component_id = (int)enmOtherComponent.EmpNationality, rpt_title = "Nationality", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 4, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 5, component_id = (int)enmOtherComponent.EducationLevel, rpt_title = "Education Level", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 5, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 6, component_id = null, rpt_title = "Category Address", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 6, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 7, component_id = null, rpt_title = "Type Of Employment", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 7, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 8, component_id = (int)enmOtherComponent.EmpContact, rpt_title = "Mobile", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 8, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 9, component_id = (int)enmOtherComponent.Uan, rpt_title = "UAN Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 9, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 10, component_id = (int)enmOtherComponent.PanNo, rpt_title = "PAN No", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 10, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 11, component_id = (int)enmOtherComponent.PanName, rpt_title = "PAN Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 11, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 12, component_id = (int)enmOtherComponent.ESICNo, rpt_title = "ESIC Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 12, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 13, component_id = (int)enmOtherComponent.AdharNo, rpt_title = "Aadhar card Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 13, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 14, component_id = (int)enmOtherComponent.AdharName, rpt_title = "Aadhar name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 14, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 15, component_id = (int)enmOtherComponent.BankAccountNo, rpt_title = "Salary Account No", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 15, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 16, component_id = (int)enmOtherComponent.BankName, rpt_title = "Bank Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 16, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 17, component_id = (int)enmOtherComponent.IFSCCode, rpt_title = "Bank IFSC Code", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 17, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 18, component_id = null, rpt_title = "Address", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 18, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 19, component_id = null, rpt_title = "service book no", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 19, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 20, component_id = null, rpt_title = "Resignation date", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 20, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 21, component_id = null, rpt_title = "Last working date", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 21, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 22, component_id = null, rpt_title = "Reason Master", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 22, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 23, component_id = null, rpt_title = "mark of identification", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 23, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 24, component_id = null, rpt_title = "specimen impression", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 24, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 25, component_id = null, rpt_title = "Is Active", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 25, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 26, component_id = (int)enmOtherComponent.CompanyName, rpt_title = "Company Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 26, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 27, component_id = null, rpt_title = "Division Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 27, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 28, component_id = (int)enmOtherComponent.EmpWorkingState, rpt_title = "State Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 28, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 29, component_id = (int)enmOtherComponent.EmpLocation, rpt_title = "Branch Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 29, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 30, component_id = (int)enmOtherComponent.EmpLocation, rpt_title = "Location Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 30, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 31, component_id = (int)enmOtherComponent.EmpDepartment, rpt_title = "Department Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 31, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 32, component_id = (int)enmOtherComponent.EmpJoiningDt, rpt_title = "Date Of Joining", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 32, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 33, component_id = (int)enmOtherComponent.EmpDesignation, rpt_title = "Designation Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 33, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 34, component_id = (int)enmOtherComponent.EmpGrade, rpt_title = "Grade", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 34, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 35, component_id = (int)enmOtherComponent.PfApplicableYesNo, rpt_title = "PF Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 35, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 36, component_id = (int)enmOtherComponent.PfNo, rpt_title = "PF Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 36, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 37, component_id = (int)enmOtherComponent.PfGroup, rpt_title = "PF Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 37, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 38, component_id = (int)enmOtherComponent.pf_celling, rpt_title = "PF Ceiling", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 38, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 39, component_id = null, rpt_title = "ESIC Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 39, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 40, component_id = null, rpt_title = "PT Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 40, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 41, component_id = null, rpt_title = "PT Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 41, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 42, component_id = (int)enmOtherComponent.Vpf_applicableYesNo, rpt_title = "VPF Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 42, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 43, component_id = (int)enmOtherComponent.vpf_percentage, rpt_title = "VPF Percentage", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 43, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 44, component_id = null, rpt_title = "Is daily", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 44, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 45, component_id = null, rpt_title = "Salary On Hold", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 45, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 46, component_id = null, rpt_title = "Salary Process Type", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 46, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 47, component_id = (int)enmOtherComponent.GrossSalary, rpt_title = "Monthly Gross ", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 47, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 48, component_id = (int)enmOtherComponent.PaidDays, rpt_title = "Days Worked", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 48, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 49, component_id = (int)enmOtherComponent.ArrearDays, rpt_title = "Arrears Days", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 49, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 50, component_id = (int)enmOtherComponent.ToalPayrollDay, rpt_title = "Days In Month", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 50, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 51, component_id = (int)enmOtherComponent.Gratuity, rpt_title = "Gratuity", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 51, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 52, component_id = (int)enmOtherComponent.Leen, rpt_title = "Leen", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 52, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 53, component_id = null, rpt_title = "Notice Pay", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 53, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 54, component_id = null, rpt_title = "Pre hold  salary released", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 54, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 55, component_id = (int)enmOtherComponent.BasicSalary, rpt_title = "Basic Salary", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 55, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 56, component_id = (int)enmOtherComponent.BasicSalary, rpt_title = "Arrear Basic Salary", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 56, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 57, component_id = (int)enmOtherComponent.HRA, rpt_title = "HRA Allowance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 57, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 58, component_id = (int)enmOtherComponent.HRA, rpt_title = "Arrear HRA Allowance", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 58, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 59, component_id = (int)enmOtherComponent.Conveyance, rpt_title = "Conveyance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 59, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 60, component_id = (int)enmOtherComponent.Conveyance, rpt_title = "Arrear conveyance", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 60, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 61, component_id = (int)enmOtherComponent.Medical_Allowance, rpt_title = "Medical Allowance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 61, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 62, component_id = (int)enmOtherComponent.Medical_Allowance, rpt_title = "Arrear Medical Allowance", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 62, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 63, component_id = (int)enmOtherComponent.SPL, rpt_title = "Special Allowance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 63, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 64, component_id = (int)enmOtherComponent.SPL, rpt_title = "Arrear Special Allowance", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 64, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 65, component_id = (int)enmOtherComponent.Pf_amount, rpt_title = "PF Employee", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 65, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 66, component_id = (int)enmOtherComponent.Pf_amount, rpt_title = "Arrear PF Employee", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 66, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 67, component_id = (int)enmOtherComponent.EsicAmount, rpt_title = "ESIC Employee", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 67, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 68, component_id = (int)enmOtherComponent.EsicAmount, rpt_title = "Arrear ESIC Employee", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 68, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 69, component_id = (int)enmOtherComponent.ChildrenEducationAllowance, rpt_title = "Children Education Allowance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 69, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 70, component_id = (int)enmOtherComponent.ChildrenEducationAllowance, rpt_title = "Arrear Children Education Allowance", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 70, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 71, component_id = (int)enmOtherComponent.TotalGross, rpt_title = "Gross", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 71, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 72, component_id = (int)enmOtherComponent.TotalGross, rpt_title = "Arrear Gross", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 72, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 73, component_id = (int)enmOtherComponent.OT, rpt_title = "Over Time", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 73, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 74, component_id = (int)enmOtherComponent.CityCompensatoryAllowances, rpt_title = "CityCompensatoryAllowances", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 74, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 75, component_id = null, rpt_title = "OtherAllowance", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 75, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 76, component_id = (int)enmOtherComponent.TotalGross, rpt_title = "Gross Salary", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 76, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 77, component_id = (int)enmOtherComponent.Pf_amount, rpt_title = "Provident Fund", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 77, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 78, component_id = (int)enmOtherComponent.PT, rpt_title = "Professional Tax", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 78, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 79, component_id = (int)enmOtherComponent.EsicAmount, rpt_title = "ESIC", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 79, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 80, component_id = null, rpt_title = "LWF", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 80, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 81, component_id = (int)enmOtherComponent.Tax, rpt_title = "TDS", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 81, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 82, component_id = (int)enmOtherComponent.Vpf_amount, rpt_title = "Voluntary Provident Fund", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 82, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 83, component_id = null, rpt_title = "Notice Reco", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 83, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 84, component_id = (int)enmOtherComponent.OtherDeduction, rpt_title = "Other Deduction", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 84, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 85, component_id = (int)enmOtherComponent.Advance_Loan, rpt_title = "Advance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 85, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 86, component_id = (int)enmOtherComponent.Recovery, rpt_title = "Recovery", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 86, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 87, component_id = (int)enmOtherComponent.Deduction, rpt_title = "Gross Deduction", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 87, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 88, component_id = (int)enmOtherComponent.Net, rpt_title = "Net Salary", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 88, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 89, component_id = null, rpt_title = "Remarks", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 89, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 501, component_id = (int)enmOtherComponent.Gender, rpt_title = "Gender", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 1, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 502, component_id = (int)enmOtherComponent.EmpFatherHusbandName, rpt_title = "Father Husband Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 2, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 503, component_id = (int)enmOtherComponent.EmpDOB, rpt_title = "Date of Birth", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 3, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 504, component_id = (int)enmOtherComponent.EmpNationality, rpt_title = "Nationality", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 4, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 505, component_id = (int)enmOtherComponent.EducationLevel, rpt_title = "Education Level", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 5, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 506, component_id = null, rpt_title = "Category Address", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 6, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 507, component_id = null, rpt_title = "Type Of Employment", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 7, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 508, component_id = (int)enmOtherComponent.EmpContact, rpt_title = "Mobile", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 8, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 509, component_id = (int)enmOtherComponent.Uan, rpt_title = "UAN Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 9, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 510, component_id = (int)enmOtherComponent.PanNo, rpt_title = "PAN No", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 10, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 511, component_id = (int)enmOtherComponent.PanName, rpt_title = "PAN Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 11, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 512, component_id = (int)enmOtherComponent.ESICNo, rpt_title = "ESIC Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 12, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 513, component_id = (int)enmOtherComponent.AdharNo, rpt_title = "Aadhar card Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 13, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 514, component_id = (int)enmOtherComponent.AdharName, rpt_title = "Aadhar name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 14, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 515, component_id = (int)enmOtherComponent.BankAccountNo, rpt_title = "Salary Account No", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 15, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 516, component_id = (int)enmOtherComponent.BankName, rpt_title = "Bank Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 16, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 517, component_id = (int)enmOtherComponent.IFSCCode, rpt_title = "Bank IFSC Code", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 17, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 518, component_id = null, rpt_title = "Address", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 18, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 519, component_id = null, rpt_title = "service book no", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 19, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 520, component_id = null, rpt_title = "Resignation date", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 20, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 521, component_id = null, rpt_title = "Last working date", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 21, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 522, component_id = null, rpt_title = "Reason Master", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 22, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 523, component_id = null, rpt_title = "mark of identification", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 23, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 524, component_id = null, rpt_title = "specimen impression", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 24, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 525, component_id = null, rpt_title = "Is Active", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 25, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 526, component_id = (int)enmOtherComponent.CompanyName, rpt_title = "Company Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 26, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 527, component_id = null, rpt_title = "Division Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 27, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 528, component_id = (int)enmOtherComponent.EmpWorkingState, rpt_title = "State Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 28, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 529, component_id = (int)enmOtherComponent.EmpLocation, rpt_title = "Branch Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 29, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 530, component_id = (int)enmOtherComponent.EmpLocation, rpt_title = "Location Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 30, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 531, component_id = (int)enmOtherComponent.EmpDepartment, rpt_title = "Department Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 31, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 532, component_id = (int)enmOtherComponent.EmpJoiningDt, rpt_title = "Date Of Joining", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 32, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 533, component_id = (int)enmOtherComponent.EmpDesignation, rpt_title = "Designation Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 33, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 534, component_id = (int)enmOtherComponent.EmpGrade, rpt_title = "Grade", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 34, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 535, component_id = (int)enmOtherComponent.PfApplicableYesNo, rpt_title = "PF Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 35, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 536, component_id = (int)enmOtherComponent.PfNo, rpt_title = "PF Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 36, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 537, component_id = (int)enmOtherComponent.PfGroup, rpt_title = "PF Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 37, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 538, component_id = (int)enmOtherComponent.pf_celling, rpt_title = "PF Ceiling", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 38, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 539, component_id = null, rpt_title = "ESIC Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 39, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 540, component_id = null, rpt_title = "PT Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 40, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 541, component_id = null, rpt_title = "PT Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 41, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 542, component_id = (int)enmOtherComponent.Vpf_applicableYesNo, rpt_title = "VPF Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 42, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 543, component_id = (int)enmOtherComponent.vpf_percentage, rpt_title = "VPF Percentage", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 43, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 544, component_id = null, rpt_title = "Is daily", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 44, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 545, component_id = null, rpt_title = "Salary On Hold", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 45, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 546, component_id = null, rpt_title = "Salary Process Type", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 46, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 547, component_id = (int)enmOtherComponent.GrossSalary, rpt_title = "Monthly Gross ", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 47, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 548, component_id = (int)enmOtherComponent.PaidDays, rpt_title = "Days Worked", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 48, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 549, component_id = (int)enmOtherComponent.ArrearDays, rpt_title = "Arrears Days", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 49, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 550, component_id = (int)enmOtherComponent.ToalPayrollDay, rpt_title = "Days In Month", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 50, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 551, component_id = (int)enmOtherComponent.Gratuity, rpt_title = "Gratuity", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 51, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 552, component_id = (int)enmOtherComponent.Leen, rpt_title = "Leen", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 52, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 553, component_id = null, rpt_title = "Notice Pay", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 53, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 554, component_id = null, rpt_title = "Pre hold  salary released", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 54, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 555, component_id = (int)enmOtherComponent.BasicSalary, rpt_title = "Basic Salary", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 55, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 556, component_id = (int)enmOtherComponent.HRA, rpt_title = "HRA Allowance", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 56, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 557, component_id = (int)enmOtherComponent.Conveyance, rpt_title = "Conveyance", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 57, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 558, component_id = (int)enmOtherComponent.Medical_Allowance, rpt_title = "Medical Allowance", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 58, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 559, component_id = (int)enmOtherComponent.SPL, rpt_title = "Special Allowance", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 59, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 560, component_id = (int)enmOtherComponent.Pf_amount, rpt_title = "PF Employee", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 60, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 561, component_id = (int)enmOtherComponent.EsicAmount, rpt_title = "ESIC Employee", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 61, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 562, component_id = (int)enmOtherComponent.ChildrenEducationAllowance, rpt_title = "Children Education Allowance", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 62, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 563, component_id = (int)enmOtherComponent.TotalGross, rpt_title = "Gross", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 63, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 564, component_id = (int)enmOtherComponent.OT, rpt_title = "Over Time", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 64, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 565, component_id = (int)enmOtherComponent.CityCompensatoryAllowances, rpt_title = "CityCompensatoryAllowances", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 65, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 566, component_id = null, rpt_title = "OtherAllowance", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 66, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 567, component_id = (int)enmOtherComponent.TotalGross, rpt_title = "Gross Salary", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 67, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 568, component_id = (int)enmOtherComponent.Pf_amount, rpt_title = "Provident Fund", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 68, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 569, component_id = (int)enmOtherComponent.PT, rpt_title = "Professional Tax", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 69, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 570, component_id = (int)enmOtherComponent.EsicAmount, rpt_title = "ESIC", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 70, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 571, component_id = null, rpt_title = "LWF", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 71, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 572, component_id = (int)enmOtherComponent.Tax, rpt_title = "TDS", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 72, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 573, component_id = (int)enmOtherComponent.Vpf_amount, rpt_title = "Voluntary Provident Fund", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 73, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 574, component_id = null, rpt_title = "Notice Reco", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 74, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 575, component_id = (int)enmOtherComponent.OtherDeduction, rpt_title = "Other Deduction", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 75, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 576, component_id = (int)enmOtherComponent.Advance_Loan, rpt_title = "Advance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 76, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 577, component_id = (int)enmOtherComponent.Recovery, rpt_title = "Recovery", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 77, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 578, component_id = (int)enmOtherComponent.Deduction, rpt_title = "Gross Deduction", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 78, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 579, component_id = (int)enmOtherComponent.Net, rpt_title = "Net Salary", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 79, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 580, component_id = null, rpt_title = "Remarks", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 80, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });

            #endregion
            modelBuilder.Entity<tbl_rpt_title_master>().HasData(tbl_rpt_title_masters);

        }


        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="modelBuilder"></param>



        private void SetRoleMaster(ModelBuilder modelBuilder)
        {
            List<tbl_role_master> role_data = new List<tbl_role_master>();
            foreach (enmRoleMaster _enm in Enum.GetValues(typeof(enmRoleMaster)))
            {
                role_data.Add(new tbl_role_master() { created_by = 1, created_date = new DateTime(2020, 1, 1), is_active = 1, last_modified_by = 1, last_modified_date = new DateTime(2020, 1, 1), role_id = (int)_enm, role_name = _enm.ToString() });
            }


            List<tbl_menu_master> menu_Masters = new List<tbl_menu_master>();
            List<tbl_role_menu_master> trmm = new List<tbl_role_menu_master>();
            foreach (enmMenuMaster _enm in Enum.GetValues(typeof(enmMenuMaster)))
            {
                if (_enm != enmMenuMaster.None)
                {
                    MenuComponent menuComponent = _enm.GetMenuDetails();
                    menu_Masters.Add(new tbl_menu_master()
                    {
                        menu_id = _enm,
                        menu_name = menuComponent.menu_name,
                        IconUrl = menuComponent.icon,
                        urll = menuComponent.link,
                        type = menuComponent.type,
                        parent_menu_id = (int)menuComponent.parent_menu,
                        modified_by = 1,
                        modified_date = new DateTime(2020, 1, 1),
                        created_by = 1,
                        created_date = new DateTime(2020, 1, 1),
                        is_active = 1,

                    });

                    foreach (var enrole in menuComponent.RoleMaster)
                    {

                        trmm.Add(new tbl_role_menu_master()
                        {
                            modified_by = 1,
                            modified_date = new DateTime(2020, 1, 1),
                            created_by = 1,
                            created_date = new DateTime(2020, 1, 1),
                            menu_id = _enm,
                            role_id = enrole,
                            role_menu_id = (((int)_enm * 10000) + (int)enrole)
                        });
                        if (enrole == enmRoleMaster.Manager)
                        {
                            trmm.Add(new tbl_role_menu_master()
                            {
                                modified_by = 1,
                                modified_date = new DateTime(2020, 1, 1),
                                created_by = 1,
                                created_date = new DateTime(2020, 1, 1),
                                menu_id = _enm,
                                role_id = enmRoleMaster.SectionHead,
                                role_menu_id = (((int)_enm * 10000) + (int)enmRoleMaster.SectionHead)
                            });
                        }
                    }
                }
            }
            modelBuilder.Entity<tbl_role_master>().HasData(role_data);
            modelBuilder.Entity<tbl_menu_master>().HasData(menu_Masters);
            modelBuilder.Entity<tbl_role_menu_master>().HasData(trmm);
        }
        private void SetCountryState(ModelBuilder modelBuilder)
        {

        }
    }




}



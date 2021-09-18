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
        //public string _connectionString = "Data Source = localhost; port = 3306; Initial Catalog = db_glaze_hrms; User ID = root; Password = DHRUV@123;Allow User Variables=True ; MaximumPoolsize=5000;Convert Zero Datetime=True;Default Command Timeout=600;";
        public string _connectionString = "Data Source = 103.208.201.116; port = 3306; Initial Catalog = db_hrms_test; User ID = hrms; Password = DHRUV@123;Allow User Variables=True ; MaximumPoolsize=5000;Convert Zero Datetime=True;Default Command Timeout=600;";

        /// <summary>
        /// QA Server
        /// </summary>
        //public string _connectionString = "Data Source = 192.168.10.6; port = 3306; Initial Catalog = db_hrms_glaze_05_11_20; User ID = sa; Password = glaze@123;Allow User Variables=True ; MaximumPoolsize=5000;Convert Zero Datetime=True;";
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
            InsertCompany(modelBuilder);
            InsertLocation(modelBuilder);
            InsertDepartmentMaster(modelBuilder);
            InsertAppSetting(modelBuilder);
            InsertBanksMaster(modelBuilder);
            insertDesignation(modelBuilder);
            InsertDocumentTypemaster(modelBuilder);
            InsertGradeMaster(modelBuilder);
            InsertLeaveType(modelBuilder);
            InsertReligionType(modelBuilder);
            InsertPayrolldata(modelBuilder);
            InsertRoleClaim(modelBuilder);
            DefaultEmployee(modelBuilder);
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

        

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="modelBuilder"></param>



        
        private void SetCountryState(ModelBuilder modelBuilder)
        {

        }
    }




}



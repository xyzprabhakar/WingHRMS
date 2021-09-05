using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class _1_Alltables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeBasicDataProc",
                columns: table => new
                {
                    employee_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(nullable: false),
                    emp_code = table.Column<string>(nullable: true),
                    emp_name = table.Column<string>(nullable: true),
                    company_id = table.Column<int>(nullable: false),
                    company_name = table.Column<string>(nullable: true),
                    location_name = table.Column<string>(nullable: true),
                    location_id = table.Column<int>(nullable: false),
                    dept_name = table.Column<string>(nullable: true),
                    dept_id = table.Column<int>(nullable: false),
                    state_id = table.Column<int>(nullable: false),
                    state_name = table.Column<string>(nullable: true),
                    emp_status = table.Column<int>(nullable: false),
                    isActive = table.Column<int>(nullable: false),
                    mobileno = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    desig_name = table.Column<string>(nullable: true),
                    emp_img = table.Column<string>(nullable: true),
                    dob = table.Column<string>(nullable: true),
                    doanv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeBasicDataProc", x => x.employee_id);
                });

            migrationBuilder.CreateTable(
                name: "mdlSalaryInputValues",
                columns: table => new
                {
                    compId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    compName = table.Column<string>(nullable: true),
                    compValue = table.Column<string>(nullable: true),
                    rate = table.Column<double>(nullable: false),
                    current_month_value = table.Column<double>(nullable: false),
                    arrear_value = table.Column<double>(nullable: false),
                    component_type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mdlSalaryInputValues", x => x.compId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_app_setting",
                columns: table => new
                {
                    pkid_setting = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppSettingKey = table.Column<string>(nullable: true),
                    AppSettingKeyDisplay = table.Column<string>(nullable: true),
                    AppSettingValue = table.Column<string>(nullable: true),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_app_setting", x => x.pkid_setting);
                });

            migrationBuilder.CreateTable(
                name: "tbl_bank_master",
                columns: table => new
                {
                    bank_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    bank_name = table.Column<string>(maxLength: 100, nullable: true),
                    bank_status = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    deleted_by = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_bank_master", x => x.bank_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_captcha_code_details",
                columns: table => new
                {
                    guid = table.Column<string>(nullable: false),
                    captcha_code = table.Column<string>(nullable: true),
                    genration_dt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_captcha_code_details", x => x.guid);
                });

            migrationBuilder.CreateTable(
                name: "tbl_claim_master",
                columns: table => new
                {
                    claim_master_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    claim_master_name = table.Column<string>(maxLength: 100, nullable: true),
                    description = table.Column<string>(maxLength: 500, nullable: true),
                    method_type = table.Column<string>(maxLength: 500, nullable: true),
                    is_post = table.Column<byte>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_claim_master", x => x.claim_master_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_comb_off_rule_master",
                columns: table => new
                {
                    comp_off_rule_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    minimum_working_hours = table.Column<int>(maxLength: 200, nullable: false),
                    minimum_working_minute = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_comb_off_rule_master", x => x.comp_off_rule_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_component_master",
                columns: table => new
                {
                    component_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    component_name = table.Column<string>(nullable: true),
                    datatype = table.Column<string>(nullable: true),
                    defaultvalue = table.Column<string>(nullable: true),
                    parentid = table.Column<int>(nullable: false),
                    is_system_key = table.Column<byte>(nullable: false),
                    property_details = table.Column<string>(nullable: true),
                    System_function = table.Column<string>(nullable: true),
                    System_table = table.Column<string>(nullable: true),
                    component_type = table.Column<int>(nullable: false),
                    is_salary_comp = table.Column<byte>(nullable: false),
                    is_tds_comp = table.Column<byte>(nullable: false),
                    is_data_entry_comp = table.Column<byte>(nullable: false),
                    payment_type = table.Column<int>(nullable: false),
                    is_user_interface = table.Column<int>(nullable: false),
                    is_fnf_component = table.Column<int>(nullable: false),
                    is_payslip = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_component_master", x => x.component_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_country",
                columns: table => new
                {
                    country_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sort_name = table.Column<string>(maxLength: 10, nullable: true),
                    name = table.Column<string>(maxLength: 100, nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_country", x => x.country_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_current_openings",
                columns: table => new
                {
                    pkid_current_openings = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: false),
                    department_id = table.Column<int>(nullable: false),
                    current_status = table.Column<int>(nullable: false),
                    opening_detail = table.Column<string>(nullable: true),
                    posted_date = table.Column<DateTime>(nullable: false),
                    experience = table.Column<string>(nullable: true),
                    job_description = table.Column<string>(nullable: true),
                    role_responsibility = table.Column<string>(nullable: true),
                    doc_path = table.Column<string>(nullable: true),
                    remarks = table.Column<string>(nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_current_openings", x => x.pkid_current_openings);
                });

            migrationBuilder.CreateTable(
                name: "tbl_designation_master",
                columns: table => new
                {
                    designation_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    designation_name = table.Column<string>(maxLength: 200, nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_designation_master", x => x.designation_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_employee_master",
                columns: table => new
                {
                    employee_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_code = table.Column<string>(maxLength: 100, nullable: true),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_employee_master", x => x.employee_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_grade_master",
                columns: table => new
                {
                    grade_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    grade_name = table.Column<string>(maxLength: 50, nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_grade_master", x => x.grade_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_guid_detail",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    genration_dt = table.Column<DateTime>(nullable: false),
                    genrated_by = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_guid_detail", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_holiday_master",
                columns: table => new
                {
                    holiday_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    from_date = table.Column<DateTime>(nullable: false),
                    to_date = table.Column<DateTime>(nullable: false),
                    holiday_name = table.Column<string>(maxLength: 200, nullable: true),
                    holiday_date = table.Column<DateTime>(nullable: false),
                    is_applicable_on_all_comp = table.Column<byte>(nullable: false),
                    is_applicable_on_all_emp = table.Column<byte>(nullable: false),
                    is_applicable_on_all_religion = table.Column<byte>(nullable: false),
                    is_applicable_on_all_location = table.Column<byte>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    holidayno = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_holiday_master", x => x.holiday_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_kt_file",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ktfile = table.Column<string>(nullable: true),
                    seperation_id = table.Column<int>(nullable: false),
                    uploadedOn = table.Column<DateTime>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: false),
                    uploaded_by = table.Column<int>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_kt_file", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_type",
                columns: table => new
                {
                    leave_type_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    leave_type_name = table.Column<string>(maxLength: 200, nullable: true),
                    description = table.Column<string>(maxLength: 200, nullable: true),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    _is_el = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_type", x => x.leave_type_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_loan_request_master",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    loan_type = table.Column<int>(nullable: false),
                    em_status = table.Column<int>(nullable: false),
                    loan_amount = table.Column<decimal>(nullable: false),
                    rate_of_interest = table.Column<decimal>(nullable: false),
                    on_salary = table.Column<decimal>(nullable: false),
                    max_tenure = table.Column<int>(nullable: false),
                    min_top_up_duration = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    grade_id = table.Column<int>(nullable: false),
                    companyid = table.Column<int>(nullable: false),
                    is_reporting_mgr_approval = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_loan_request_master", x => x.sno);
                });

            migrationBuilder.CreateTable(
                name: "tbl_lop_detail",
                columns: table => new
                {
                    lop_detail_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    lop_master_id = table.Column<int>(nullable: true),
                    companyid = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_lop_detail", x => x.lop_detail_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_lossofpay_master",
                columns: table => new
                {
                    lop_master_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: true),
                    company_id = table.Column<int>(nullable: true),
                    totaldays = table.Column<int>(nullable: true),
                    acutual_lop_days = table.Column<decimal>(nullable: true),
                    final_lop_days = table.Column<decimal>(nullable: true),
                    Week_off_days = table.Column<decimal>(nullable: false),
                    Holiday_days = table.Column<decimal>(nullable: false),
                    Present_days = table.Column<decimal>(nullable: false),
                    Absent_days = table.Column<decimal>(nullable: false),
                    Leave_days = table.Column<decimal>(nullable: false),
                    Actual_Paid_days = table.Column<decimal>(nullable: false),
                    Additional_Paid_days = table.Column<decimal>(nullable: false),
                    Total_Paid_days = table.Column<decimal>(nullable: false),
                    is_freezed = table.Column<int>(nullable: false),
                    remarks = table.Column<string>(nullable: true),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false),
                    monthyear = table.Column<int>(nullable: true),
                    fkid_sepration = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_lossofpay_master", x => x.lop_master_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_lossofpay_setting",
                columns: table => new
                {
                    lop_setting_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    companyid = table.Column<int>(nullable: false),
                    lop_setting_name = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_lossofpay_setting", x => x.lop_setting_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_menu_master",
                columns: table => new
                {
                    menu_id = table.Column<int>(nullable: false),
                    menu_name = table.Column<string>(maxLength: 50, nullable: true),
                    parent_menu_id = table.Column<int>(nullable: true),
                    IconUrl = table.Column<string>(maxLength: 300, nullable: true),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false),
                    urll = table.Column<string>(nullable: true),
                    SortingOrder = table.Column<int>(nullable: false),
                    type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_menu_master", x => x.menu_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ot_rule_master",
                columns: table => new
                {
                    ot_rule_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    grace_working_hour = table.Column<int>(maxLength: 200, nullable: false),
                    grace_working_minute = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ot_rule_master", x => x.ot_rule_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_payroll_month_setting",
                columns: table => new
                {
                    payroll_month_setting_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    from_month = table.Column<int>(nullable: false),
                    from_date = table.Column<int>(nullable: false),
                    applicable_from_date = table.Column<int>(nullable: false),
                    applicable_to_date = table.Column<int>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    company_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_payroll_month_setting", x => x.payroll_month_setting_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_policy_master",
                columns: table => new
                {
                    pkid_policy = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    policy_name = table.Column<string>(nullable: true),
                    remarks = table.Column<string>(nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_policy_master", x => x.pkid_policy);
                });

            migrationBuilder.CreateTable(
                name: "tbl_policy_master_documents",
                columns: table => new
                {
                    pkid_tpmDoc = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fkid_policy = table.Column<int>(nullable: false),
                    document_path = table.Column<string>(nullable: true),
                    is_active = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_policy_master_documents", x => x.pkid_tpmDoc);
                });

            migrationBuilder.CreateTable(
                name: "tbl_religion_master",
                columns: table => new
                {
                    religion_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    religion_name = table.Column<string>(maxLength: 200, nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_religion_master", x => x.religion_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_remimb_cat_mstr",
                columns: table => new
                {
                    rcm_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reimbursement_category_name = table.Column<string>(maxLength: 50, nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    is_delete = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_remimb_cat_mstr", x => x.rcm_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_report_master",
                columns: table => new
                {
                    rpt_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    rpt_name = table.Column<string>(maxLength: 200, nullable: true),
                    rpt_description = table.Column<string>(maxLength: 200, nullable: true),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_report_master", x => x.rpt_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_role_master",
                columns: table => new
                {
                    role_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    role_name = table.Column<string>(maxLength: 50, nullable: true),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_role_master", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_role_menu_master",
                columns: table => new
                {
                    role_menu_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    role_id = table.Column<int>(nullable: false),
                    menu_id = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_role_menu_master", x => x.role_menu_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_scheduler_master",
                columns: table => new
                {
                    scheduler_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    scheduler_name = table.Column<string>(nullable: true),
                    schduler_type = table.Column<int>(nullable: false),
                    last_runing_date = table.Column<DateTime>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_scheduler_master", x => x.scheduler_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_shift_master",
                columns: table => new
                {
                    shift_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    shift_code = table.Column<string>(maxLength: 100, nullable: true),
                    is_active = table.Column<int>(nullable: false),
                    is_default = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_shift_master", x => x.shift_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_tax_slab_master",
                columns: table => new
                {
                    tax_slab_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    gender = table.Column<string>(nullable: true),
                    min_age = table.Column<int>(nullable: false),
                    max_age = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_tax_slab_master", x => x.tax_slab_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_claim_master_log",
                columns: table => new
                {
                    claim_master_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    claim_master_id = table.Column<int>(nullable: true),
                    claim_master_name = table.Column<string>(maxLength: 50, nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_claim_master_log", x => x.claim_master_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_claim_master_log_tbl_claim_master_claim_master_id",
                        column: x => x.claim_master_id,
                        principalTable: "tbl_claim_master",
                        principalColumn: "claim_master_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_comb_off_log",
                columns: table => new
                {
                    comp_off_rule_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    comp_off_id = table.Column<int>(nullable: true),
                    minimum_working_hours = table.Column<int>(maxLength: 200, nullable: false),
                    minimum_working_minute = table.Column<int>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_comb_off_log", x => x.comp_off_rule_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_comb_off_log_tbl_comb_off_rule_master_comp_off_id",
                        column: x => x.comp_off_id,
                        principalTable: "tbl_comb_off_rule_master",
                        principalColumn: "comp_off_rule_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_state",
                columns: table => new
                {
                    state_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    country_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_state", x => x.state_id);
                    table.ForeignKey(
                        name: "FK_tbl_state_tbl_country_country_id",
                        column: x => x.country_id,
                        principalTable: "tbl_country",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_designation_log",
                columns: table => new
                {
                    designation_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    desig_id = table.Column<int>(nullable: true),
                    designation_name = table.Column<string>(maxLength: 200, nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_designation_log", x => x.designation_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_designation_log_tbl_designation_master_desig_id",
                        column: x => x.desig_id,
                        principalTable: "tbl_designation_master",
                        principalColumn: "designation_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "log_tbl_salary_input",
                columns: table => new
                {
                    log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    salary_input_id = table.Column<int>(nullable: false),
                    monthyear = table.Column<string>(nullable: true),
                    emp_id = table.Column<int>(nullable: true),
                    component_id = table.Column<int>(nullable: true),
                    values = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    log_created_by = table.Column<int>(nullable: false),
                    log_created_date = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_log_tbl_salary_input", x => x.log_id);
                    table.ForeignKey(
                        name: "FK_log_tbl_salary_input_tbl_component_master_component_id",
                        column: x => x.component_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_log_tbl_salary_input_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_attendance_details_manual",
                columns: table => new
                {
                    attendance_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: true),
                    attendance_dt = table.Column<DateTime>(nullable: false),
                    start_in = table.Column<DateTime>(nullable: true),
                    start_out = table.Column<DateTime>(nullable: true),
                    day_status = table.Column<byte>(nullable: true),
                    user_id = table.Column<int>(nullable: false),
                    entry_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_attendance_details_manual", x => x.attendance_id);
                    table.ForeignKey(
                        name: "FK_tbl_attendance_details_manual_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_comp_off_ledger",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    compoff_date = table.Column<DateTime>(nullable: false),
                    credit = table.Column<double>(nullable: false),
                    dredit = table.Column<double>(nullable: false),
                    transaction_date = table.Column<DateTime>(nullable: false),
                    transaction_type = table.Column<byte>(nullable: false),
                    monthyear = table.Column<int>(nullable: false),
                    transaction_no = table.Column<int>(nullable: false),
                    remarks = table.Column<string>(nullable: true),
                    e_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    deleted_by = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_comp_off_ledger", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_comp_off_ledger_tbl_employee_master_deleted_by",
                        column: x => x.deleted_by,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_comp_off_ledger_tbl_employee_master_e_id",
                        column: x => x.e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_compoff_raise",
                columns: table => new
                {
                    emp_comp_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    comp_off_date = table.Column<DateTime>(nullable: false),
                    emp_id = table.Column<int>(nullable: true),
                    requester_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    is_approved1 = table.Column<byte>(nullable: false),
                    is_approved2 = table.Column<byte>(nullable: false),
                    is_approved3 = table.Column<byte>(nullable: false),
                    is_final_approve = table.Column<byte>(nullable: false),
                    a1_e_id = table.Column<int>(nullable: true),
                    approval1_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    a2_e_id = table.Column<int>(nullable: true),
                    approval2_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    a3_e_id = table.Column<int>(nullable: true),
                    approval3_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    deleted_by = table.Column<int>(nullable: false),
                    deleted_dt = table.Column<DateTime>(nullable: false),
                    deleted_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    approver1_dt = table.Column<DateTime>(nullable: false),
                    approver2_dt = table.Column<DateTime>(nullable: false),
                    approver3_dt = table.Column<DateTime>(nullable: false),
                    admin_id = table.Column<int>(nullable: true),
                    is_admin_approve = table.Column<int>(nullable: false),
                    admin_remarks = table.Column<string>(nullable: true),
                    admin_ar_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_compoff_raise", x => x.emp_comp_id);
                    table.ForeignKey(
                        name: "FK_tbl_compoff_raise_tbl_employee_master_a1_e_id",
                        column: x => x.a1_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_compoff_raise_tbl_employee_master_a2_e_id",
                        column: x => x.a2_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_compoff_raise_tbl_employee_master_a3_e_id",
                        column: x => x.a3_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_compoff_raise_tbl_employee_master_admin_id",
                        column: x => x.admin_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_compoff_raise_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_department_master",
                columns: table => new
                {
                    department_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    department_code = table.Column<string>(maxLength: 100, nullable: true),
                    department_name = table.Column<string>(maxLength: 200, nullable: true),
                    department_short_name = table.Column<string>(maxLength: 100, nullable: true),
                    employee_id = table.Column<int>(nullable: true),
                    department_head_employee_code = table.Column<string>(maxLength: 100, nullable: true),
                    department_head_employee_name = table.Column<string>(maxLength: 200, nullable: true),
                    company_id = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_department_master", x => x.department_id);
                    table.ForeignKey(
                        name: "FK_tbl_department_master_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_adhar_details",
                columns: table => new
                {
                    pan_details_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    aadha_card_name = table.Column<string>(maxLength: 100, nullable: true),
                    aadha_card_number = table.Column<string>(maxLength: 12, nullable: true),
                    aadha_card_image = table.Column<string>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_adhar_details", x => x.pan_details_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_adhar_details_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_bank_details",
                columns: table => new
                {
                    bank_details_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    payment_mode = table.Column<int>(nullable: false),
                    bank_id = table.Column<int>(nullable: true),
                    branch_name = table.Column<string>(maxLength: 150, nullable: true),
                    ifsc_code = table.Column<string>(maxLength: 11, nullable: true),
                    bank_acc = table.Column<string>(maxLength: 18, nullable: true),
                    employee_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_bank_details", x => x.bank_details_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_bank_details_tbl_bank_master_bank_id",
                        column: x => x.bank_id,
                        principalTable: "tbl_bank_master",
                        principalColumn: "bank_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_bank_details_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_desi_allocation",
                columns: table => new
                {
                    emp_grade_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    applicable_from_date = table.Column<DateTime>(nullable: false),
                    applicable_to_date = table.Column<DateTime>(nullable: false),
                    desig_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_desi_allocation", x => x.emp_grade_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_desi_allocation_tbl_designation_master_desig_id",
                        column: x => x.desig_id,
                        principalTable: "tbl_designation_master",
                        principalColumn: "designation_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_desi_allocation_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_esic_details",
                columns: table => new
                {
                    esic_details_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    is_esic_applicable = table.Column<byte>(nullable: false),
                    esic_number = table.Column<string>(maxLength: 20, nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_esic_details", x => x.esic_details_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_esic_details_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_family_sec",
                columns: table => new
                {
                    emp_family_section_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    relation = table.Column<string>(nullable: true),
                    occupation = table.Column<string>(maxLength: 200, nullable: true),
                    name_as_per_aadhar_card = table.Column<string>(maxLength: 200, nullable: true),
                    date_of_birth = table.Column<DateTime>(nullable: false),
                    gender = table.Column<string>(nullable: true),
                    dependent = table.Column<byte>(nullable: false),
                    remark = table.Column<string>(maxLength: 200, nullable: true),
                    document_image = table.Column<string>(nullable: true),
                    is_nominee = table.Column<byte>(nullable: false),
                    nominee_percentage = table.Column<double>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    aadhar_card_no = table.Column<string>(maxLength: 12, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_family_sec", x => x.emp_family_section_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_family_sec_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_manager",
                columns: table => new
                {
                    emp_mgr_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    final_approval = table.Column<byte>(nullable: false),
                    m_one_id = table.Column<int>(nullable: true),
                    m_two_id = table.Column<int>(nullable: true),
                    m_three_id = table.Column<int>(nullable: true),
                    employee_id = table.Column<int>(nullable: true),
                    applicable_from_date = table.Column<DateTime>(nullable: false),
                    applicable_to_date = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    notify_manager_1 = table.Column<byte>(nullable: false),
                    notify_manager_2 = table.Column<byte>(nullable: false),
                    notify_manager_3 = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_manager", x => x.emp_mgr_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_manager_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_manager_tbl_employee_master_m_one_id",
                        column: x => x.m_one_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_manager_tbl_employee_master_m_three_id",
                        column: x => x.m_three_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_manager_tbl_employee_master_m_two_id",
                        column: x => x.m_two_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_pan_details",
                columns: table => new
                {
                    pan_details_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    pan_card_name = table.Column<string>(maxLength: 100, nullable: true),
                    pan_card_number = table.Column<string>(maxLength: 10, nullable: true),
                    pan_card_image = table.Column<string>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_pan_details", x => x.pan_details_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_pan_details_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_personal_sec",
                columns: table => new
                {
                    emp_personal_section_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    blood_group = table.Column<byte>(nullable: false),
                    blood_group_doc = table.Column<string>(nullable: true),
                    primary_contact_number = table.Column<string>(maxLength: 15, nullable: true),
                    secondary_contact_number = table.Column<string>(maxLength: 15, nullable: true),
                    primary_email_id = table.Column<string>(maxLength: 100, nullable: true),
                    secondary_email_id = table.Column<string>(maxLength: 100, nullable: true),
                    permanent_address_line_one = table.Column<string>(maxLength: 250, nullable: true),
                    permanent_address_line_two = table.Column<string>(maxLength: 250, nullable: true),
                    permanent_pin_code = table.Column<int>(nullable: false),
                    permanent_city = table.Column<int>(nullable: false),
                    permanent_state = table.Column<int>(nullable: false),
                    permanent_country = table.Column<int>(nullable: false),
                    permanent_document_type = table.Column<string>(nullable: true),
                    permanent_address_proof_document = table.Column<string>(nullable: true),
                    corresponding_address_line_one = table.Column<string>(maxLength: 250, nullable: true),
                    corresponding_address_line_two = table.Column<string>(maxLength: 250, nullable: true),
                    corresponding_pin_code = table.Column<int>(nullable: false),
                    corresponding_city = table.Column<int>(nullable: false),
                    corresponding_state = table.Column<int>(nullable: false),
                    corresponding_country = table.Column<int>(nullable: false),
                    corresponding_document_type = table.Column<string>(nullable: true),
                    corresponding_address_proof_document = table.Column<string>(nullable: true),
                    emergency_contact_name = table.Column<string>(maxLength: 200, nullable: true),
                    emergency_contact_relation = table.Column<string>(nullable: true),
                    emergency_contact_mobile_number = table.Column<string>(maxLength: 15, nullable: true),
                    is_emg_same_as_permanent = table.Column<byte>(nullable: false),
                    emergency_contact_line_one = table.Column<string>(maxLength: 250, nullable: true),
                    emergency_contact_line_two = table.Column<string>(maxLength: 250, nullable: true),
                    emergency_contact_pin_code = table.Column<int>(nullable: false),
                    emergency_contact_city = table.Column<int>(nullable: false),
                    emergency_contact_state = table.Column<int>(nullable: false),
                    emergency_contact_country = table.Column<int>(nullable: false),
                    emergency_contact_document_type = table.Column<string>(nullable: true),
                    emergency_contact_address_proof_document = table.Column<string>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_personal_sec", x => x.emp_personal_section_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_personal_sec_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_pf_details",
                columns: table => new
                {
                    pf_details_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    is_pf_applicable = table.Column<byte>(nullable: false),
                    uan_number = table.Column<string>(nullable: true),
                    pf_number = table.Column<string>(maxLength: 30, nullable: true),
                    pf_group = table.Column<int>(nullable: false),
                    is_vpf_applicable = table.Column<byte>(nullable: false),
                    vpf_Group = table.Column<int>(nullable: false),
                    is_eps_applicable = table.Column<byte>(nullable: false),
                    vpf_amount = table.Column<double>(nullable: false),
                    pf_celing = table.Column<double>(nullable: false),
                    bank_id = table.Column<int>(nullable: true),
                    ifsc_code = table.Column<string>(maxLength: 11, nullable: true),
                    bank_acc = table.Column<string>(maxLength: 18, nullable: true),
                    is_pt_applicable = table.Column<byte>(nullable: false),
                    pt_group = table.Column<string>(nullable: true),
                    spt_Description = table.Column<string>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_pf_details", x => x.pf_details_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_pf_details_tbl_bank_master_bank_id",
                        column: x => x.bank_id,
                        principalTable: "tbl_bank_master",
                        principalColumn: "bank_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_pf_details_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_qualification_sec",
                columns: table => new
                {
                    emp_qualification_section_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    board_or_university = table.Column<string>(maxLength: 200, nullable: true),
                    institute_or_school = table.Column<string>(maxLength: 200, nullable: true),
                    passing_year = table.Column<string>(nullable: true),
                    stream = table.Column<string>(maxLength: 100, nullable: true),
                    title = table.Column<string>(nullable: true),
                    education_type = table.Column<byte>(nullable: false),
                    education_level = table.Column<byte>(nullable: false),
                    marks_division_cgpa = table.Column<string>(nullable: true),
                    remark = table.Column<string>(maxLength: 200, nullable: true),
                    document_image = table.Column<string>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_qualification_sec", x => x.emp_qualification_section_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_qualification_sec_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_salary_master",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: true),
                    component_id = table.Column<int>(nullable: true),
                    applicable_from_dt = table.Column<DateTime>(nullable: false),
                    applicable_value = table.Column<decimal>(nullable: false),
                    salaryrevision = table.Column<decimal>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    maker_remark = table.Column<string>(nullable: true),
                    checker_remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_salary_master", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_emp_salary_master_tbl_component_master_component_id",
                        column: x => x.component_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_salary_master_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_weekoff",
                columns: table => new
                {
                    emp_weekoff_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    is_fixed_weekly_off = table.Column<int>(nullable: false),
                    effective_from_date = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modifed_by = table.Column<int>(nullable: false),
                    modifed_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_weekoff", x => x.emp_weekoff_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_weekoff_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_employee_income_tax_amount",
                columns: table => new
                {
                    pre_emp_inc_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: true),
                    income_tax_amount = table.Column<double>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_employee_income_tax_amount", x => x.pre_emp_inc_id);
                    table.ForeignKey(
                        name: "FK_tbl_employee_income_tax_amount_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_employment_type_master",
                columns: table => new
                {
                    employment_type_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    employment_type = table.Column<byte>(nullable: false),
                    duration_days = table.Column<int>(nullable: false),
                    duration_start_period = table.Column<DateTime>(nullable: false),
                    duration_end_period = table.Column<DateTime>(nullable: false),
                    actual_duration_days = table.Column<int>(nullable: false),
                    actual_duration_start_period = table.Column<DateTime>(nullable: false),
                    actual_duration_end_period = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    effective_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_employment_type_master", x => x.employment_type_id);
                    table.ForeignKey(
                        name: "FK_tbl_employment_type_master_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_health_card_master",
                columns: table => new
                {
                    health_card_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: false),
                    employee_id = table.Column<int>(nullable: false),
                    health_card_path = table.Column<string>(nullable: true),
                    remarks = table.Column<string>(maxLength: 200, nullable: true),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_health_card_master", x => x.health_card_id);
                    table.ForeignKey(
                        name: "FK_tbl_health_card_master_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_loan_request",
                columns: table => new
                {
                    loan_req_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    req_emp_id = table.Column<int>(nullable: true),
                    emp_code = table.Column<string>(nullable: true),
                    loan_type = table.Column<int>(nullable: false),
                    loan_amt = table.Column<decimal>(nullable: false),
                    loan_tenure = table.Column<int>(nullable: false),
                    loan_purpose = table.Column<string>(maxLength: 200, nullable: true),
                    interest_rate = table.Column<decimal>(nullable: false),
                    monthly_emi = table.Column<double>(nullable: false),
                    is_final_approval = table.Column<byte>(nullable: false),
                    is_closed = table.Column<byte>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    start_date = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_loan_request", x => x.loan_req_id);
                    table.ForeignKey(
                        name: "FK_tbl_loan_request_tbl_employee_master_req_emp_id",
                        column: x => x.req_emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ot_master",
                columns: table => new
                {
                    ot_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: true),
                    ot_date = table.Column<DateTime>(nullable: false),
                    ot_type = table.Column<int>(nullable: false),
                    ot_hours_worked = table.Column<decimal>(nullable: false),
                    normal_rate_per_hour = table.Column<decimal>(nullable: false),
                    ot_rate_per_hour = table.Column<decimal>(nullable: false),
                    Total_ot_amount_earned = table.Column<decimal>(nullable: false),
                    ot_paid_date = table.Column<DateTime>(nullable: false),
                    remarks = table.Column<string>(nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ot_master", x => x.ot_id);
                    table.ForeignKey(
                        name: "FK_tbl_ot_master_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_rimb_req_mstr",
                columns: table => new
                {
                    rrm_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: true),
                    request_type = table.Column<int>(nullable: false),
                    request_month_year = table.Column<int>(nullable: false),
                    fiscal_year_id = table.Column<string>(nullable: true),
                    total_request_amount = table.Column<double>(nullable: false),
                    is_approvred = table.Column<int>(nullable: false),
                    remarks = table.Column<string>(maxLength: 200, nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    is_delete = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_rimb_req_mstr", x => x.rrm_id);
                    table.ForeignKey(
                        name: "FK_tbl_rimb_req_mstr_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_master",
                columns: table => new
                {
                    user_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(maxLength: 50, nullable: true),
                    password = table.Column<string>(maxLength: 50, nullable: true),
                    user_type = table.Column<byte>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    is_logged_in = table.Column<byte>(nullable: false),
                    is_logged_blocked = table.Column<byte>(nullable: false),
                    logged_blocked_dt = table.Column<DateTime>(nullable: false),
                    last_logged_dt = table.Column<DateTime>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    default_company_id = table.Column<int>(nullable: false),
                    employee_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_master", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_tbl_user_master_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_grade_allocation",
                columns: table => new
                {
                    emp_grade_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    applicable_from_date = table.Column<DateTime>(nullable: false),
                    applicable_to_date = table.Column<DateTime>(nullable: false),
                    grade_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_grade_allocation", x => x.emp_grade_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_grade_allocation_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_grade_allocation_tbl_grade_master_grade_id",
                        column: x => x.grade_id,
                        principalTable: "tbl_grade_master",
                        principalColumn: "grade_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_grade_master_log",
                columns: table => new
                {
                    grade_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    grade_id = table.Column<int>(nullable: true),
                    grade_name = table.Column<string>(maxLength: 200, nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_grade_master_log", x => x.grade_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_grade_master_log_tbl_grade_master_grade_id",
                        column: x => x.grade_id,
                        principalTable: "tbl_grade_master",
                        principalColumn: "grade_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ot_rate_details",
                columns: table => new
                {
                    ot_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    companyid = table.Column<int>(nullable: false),
                    grade_id = table.Column<int>(nullable: true),
                    emp_id = table.Column<int>(nullable: true),
                    ot_amt = table.Column<double>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ot_rate_details", x => x.ot_id);
                    table.ForeignKey(
                        name: "FK_tbl_ot_rate_details_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_ot_rate_details_tbl_grade_master_grade_id",
                        column: x => x.grade_id,
                        principalTable: "tbl_grade_master",
                        principalColumn: "grade_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_rimb_grd_lmt_mstr",
                columns: table => new
                {
                    rglm_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    grade_id = table.Column<int>(nullable: true),
                    emp_id = table.Column<int>(nullable: true),
                    monthly_limit = table.Column<double>(nullable: false),
                    yearly_limit = table.Column<double>(nullable: false),
                    fiscal_year_id = table.Column<string>(nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    is_delete = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_rimb_grd_lmt_mstr", x => x.rglm_id);
                    table.ForeignKey(
                        name: "FK_tbl_rimb_grd_lmt_mstr_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_rimb_grd_lmt_mstr_tbl_grade_master_grade_id",
                        column: x => x.grade_id,
                        principalTable: "tbl_grade_master",
                        principalColumn: "grade_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_salary_group",
                columns: table => new
                {
                    group_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    group_name = table.Column<string>(maxLength: 200, nullable: true),
                    description = table.Column<string>(maxLength: 500, nullable: true),
                    minvalue = table.Column<double>(nullable: false),
                    maxvalue = table.Column<double>(nullable: false),
                    grade_Id = table.Column<int>(nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_salary_group", x => x.group_id);
                    table.ForeignKey(
                        name: "FK_tbl_salary_group_tbl_grade_master_grade_Id",
                        column: x => x.grade_Id,
                        principalTable: "tbl_grade_master",
                        principalColumn: "grade_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_holiday_master_log",
                columns: table => new
                {
                    holiday_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    holiday_id = table.Column<int>(nullable: true),
                    from_date = table.Column<DateTime>(nullable: false),
                    to_date = table.Column<DateTime>(nullable: false),
                    holiday_name = table.Column<string>(nullable: true),
                    holiday_date = table.Column<DateTime>(nullable: false),
                    is_applicable_on_all_comp = table.Column<byte>(nullable: false),
                    is_applicable_on_all_emp = table.Column<byte>(nullable: false),
                    is_applicable_on_all_religion = table.Column<byte>(nullable: false),
                    is_applicable_on_all_location = table.Column<byte>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_holiday_master_log", x => x.holiday_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_holiday_master_log_tbl_holiday_master_holiday_id",
                        column: x => x.holiday_id,
                        principalTable: "tbl_holiday_master",
                        principalColumn: "holiday_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_info",
                columns: table => new
                {
                    leave_info_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    leave_type = table.Column<byte>(nullable: false),
                    leave_tenure_from_date = table.Column<DateTime>(nullable: false),
                    leave_tenure_to_date = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<byte>(nullable: false),
                    leave_type_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_info", x => x.leave_info_id);
                    table.ForeignKey(
                        name: "FK_tbl_leave_info_tbl_leave_type_leave_type_id",
                        column: x => x.leave_type_id,
                        principalTable: "tbl_leave_type",
                        principalColumn: "leave_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_type_log",
                columns: table => new
                {
                    leave_type_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    leave_type_id = table.Column<int>(nullable: true),
                    leave_type_name = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_type_log", x => x.leave_type_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_leave_type_log_tbl_leave_type_leave_type_id",
                        column: x => x.leave_type_id,
                        principalTable: "tbl_leave_type",
                        principalColumn: "leave_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ot_rule_master_log",
                columns: table => new
                {
                    ot_rule_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ot_rule_id = table.Column<int>(nullable: true),
                    grace_working_hour = table.Column<int>(maxLength: 200, nullable: false),
                    grace_working_minute = table.Column<int>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ot_rule_master_log", x => x.ot_rule_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_ot_rule_master_log_tbl_ot_rule_master_ot_rule_id",
                        column: x => x.ot_rule_id,
                        principalTable: "tbl_ot_rule_master",
                        principalColumn: "ot_rule_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_pay_set_log",
                columns: table => new
                {
                    payroll_month_setting_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pay_set_id = table.Column<int>(nullable: true),
                    from_month = table.Column<int>(maxLength: 200, nullable: false),
                    from_date = table.Column<int>(nullable: false),
                    applicable_from_date = table.Column<int>(nullable: false),
                    applicable_to_date = table.Column<int>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_pay_set_log", x => x.payroll_month_setting_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_pay_set_log_tbl_payroll_month_setting_pay_set_id",
                        column: x => x.pay_set_id,
                        principalTable: "tbl_payroll_month_setting",
                        principalColumn: "payroll_month_setting_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_holiday_mstr_rel_list",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    h_id = table.Column<int>(nullable: true),
                    religion_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_holiday_mstr_rel_list", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_holiday_mstr_rel_list_tbl_holiday_master_h_id",
                        column: x => x.h_id,
                        principalTable: "tbl_holiday_master",
                        principalColumn: "holiday_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_holiday_mstr_rel_list_tbl_religion_master_religion_id",
                        column: x => x.religion_id,
                        principalTable: "tbl_religion_master",
                        principalColumn: "religion_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_religion_master_log",
                columns: table => new
                {
                    religion_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    religion_id = table.Column<int>(nullable: true),
                    religion_name = table.Column<string>(maxLength: 200, nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_religion_master_log", x => x.religion_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_religion_master_log_tbl_religion_master_religion_id",
                        column: x => x.religion_id,
                        principalTable: "tbl_religion_master",
                        principalColumn: "religion_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_rpt_title_master",
                columns: table => new
                {
                    title_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    component_id = table.Column<int>(nullable: true),
                    rpt_id = table.Column<int>(nullable: false),
                    display_order = table.Column<int>(nullable: false),
                    rpt_title = table.Column<string>(maxLength: 200, nullable: true),
                    payroll_report_property = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_rpt_title_master", x => x.title_id);
                    table.ForeignKey(
                        name: "FK_tbl_rpt_title_master_tbl_component_master_component_id",
                        column: x => x.component_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_rpt_title_master_tbl_report_master_rpt_id",
                        column: x => x.rpt_id,
                        principalTable: "tbl_report_master",
                        principalColumn: "rpt_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_role_claim_map",
                columns: table => new
                {
                    claim_master_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    role_id = table.Column<int>(nullable: true),
                    claim_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_role_claim_map", x => x.claim_master_id);
                    table.ForeignKey(
                        name: "FK_tbl_role_claim_map_tbl_claim_master_claim_id",
                        column: x => x.claim_id,
                        principalTable: "tbl_claim_master",
                        principalColumn: "claim_master_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_role_claim_map_tbl_role_master_role_id",
                        column: x => x.role_id,
                        principalTable: "tbl_role_master",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_role_master_log",
                columns: table => new
                {
                    role_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(nullable: true),
                    role_name = table.Column<string>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_role_master_log", x => x.role_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_role_master_log_tbl_role_master_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_role_master",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_scheduler_details",
                columns: table => new
                {
                    scheduler_detail_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    scheduler_id = table.Column<int>(nullable: true),
                    transaction_no = table.Column<int>(nullable: true),
                    last_runing_date = table.Column<DateTime>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_scheduler_details", x => x.scheduler_detail_id);
                    table.ForeignKey(
                        name: "FK_tbl_scheduler_details_tbl_scheduler_master_scheduler_id",
                        column: x => x.scheduler_id,
                        principalTable: "tbl_scheduler_master",
                        principalColumn: "scheduler_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_shift_allocation",
                columns: table => new
                {
                    emp_shift_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    shift_id = table.Column<int>(nullable: true),
                    employee_id = table.Column<int>(nullable: true),
                    applicable_from_date = table.Column<DateTime>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_shift_allocation", x => x.emp_shift_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_shift_allocation_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_shift_allocation_tbl_shift_master_shift_id",
                        column: x => x.shift_id,
                        principalTable: "tbl_shift_master",
                        principalColumn: "shift_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_shift_details",
                columns: table => new
                {
                    shift_details_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    shift_for_all_location = table.Column<byte>(nullable: false),
                    shift_for_all_company = table.Column<byte>(nullable: false),
                    shift_for_all_department = table.Column<byte>(nullable: false),
                    shift_id = table.Column<int>(nullable: true),
                    shift_type = table.Column<byte>(nullable: false),
                    shift_name = table.Column<string>(maxLength: 50, nullable: true),
                    shift_short_name = table.Column<string>(maxLength: 20, nullable: true),
                    punch_in_time = table.Column<DateTime>(nullable: false),
                    punch_in_max_time = table.Column<DateTime>(nullable: false),
                    punch_out_time = table.Column<DateTime>(nullable: false),
                    maximum_working_hours = table.Column<byte>(nullable: false),
                    maximum_working_minute = table.Column<byte>(nullable: false),
                    grace_time_for_late_punch = table.Column<DateTime>(nullable: false),
                    number_of_grace_time_applicable_in_month = table.Column<int>(nullable: false),
                    is_lunch_punch_applicable = table.Column<byte>(nullable: false),
                    lunch_punch_out_time = table.Column<DateTime>(nullable: false),
                    lunch_punch_in_time = table.Column<DateTime>(nullable: false),
                    maximum_lunch_time = table.Column<DateTime>(nullable: false),
                    is_ot_applicable = table.Column<byte>(nullable: false),
                    maximum_ot_hours = table.Column<DateTime>(nullable: false),
                    tea_punch_applicable_one = table.Column<int>(nullable: false),
                    tea_punch_out_time_one = table.Column<DateTime>(nullable: false),
                    tea_punch_in_time_one = table.Column<DateTime>(nullable: false),
                    maximum_tea_time_one = table.Column<DateTime>(nullable: false),
                    tea_punch_applicable_two = table.Column<int>(nullable: false),
                    tea_punch_out_time_two = table.Column<DateTime>(nullable: false),
                    tea_punch_in_time_two = table.Column<DateTime>(nullable: false),
                    maximum_tea_time_two = table.Column<DateTime>(nullable: false),
                    min_halfday_working_hour = table.Column<DateTime>(nullable: false),
                    is_night_shift = table.Column<byte>(nullable: false),
                    weekly_off = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_shift_details", x => x.shift_details_id);
                    table.ForeignKey(
                        name: "FK_tbl_shift_details_tbl_shift_master_shift_id",
                        column: x => x.shift_id,
                        principalTable: "tbl_shift_master",
                        principalColumn: "shift_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_shift_roster_master",
                columns: table => new
                {
                    shift_roster_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    applicable_from_date = table.Column<DateTime>(nullable: false),
                    applicable_to_date = table.Column<DateTime>(nullable: false),
                    shift_rotat_in_day = table.Column<int>(nullable: false),
                    emp_id = table.Column<int>(nullable: true),
                    shft_id1 = table.Column<int>(nullable: true),
                    shft_id2 = table.Column<int>(nullable: true),
                    shft_id3 = table.Column<int>(nullable: true),
                    shft_id4 = table.Column<int>(nullable: true),
                    shft_id5 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_shift_roster_master", x => x.shift_roster_id);
                    table.ForeignKey(
                        name: "FK_tbl_shift_roster_master_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_roster_master_tbl_shift_master_shft_id1",
                        column: x => x.shft_id1,
                        principalTable: "tbl_shift_master",
                        principalColumn: "shift_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_roster_master_tbl_shift_master_shft_id2",
                        column: x => x.shft_id2,
                        principalTable: "tbl_shift_master",
                        principalColumn: "shift_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_roster_master_tbl_shift_master_shft_id3",
                        column: x => x.shft_id3,
                        principalTable: "tbl_shift_master",
                        principalColumn: "shift_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_roster_master_tbl_shift_master_shft_id4",
                        column: x => x.shft_id4,
                        principalTable: "tbl_shift_master",
                        principalColumn: "shift_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_roster_master_tbl_shift_master_shft_id5",
                        column: x => x.shft_id5,
                        principalTable: "tbl_shift_master",
                        principalColumn: "shift_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_tax_slab_Details",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    tax_slab_id = table.Column<int>(nullable: true),
                    gender = table.Column<string>(nullable: true),
                    min_value = table.Column<int>(nullable: false),
                    max_value = table.Column<int>(nullable: false),
                    TaxPercentage = table.Column<decimal>(nullable: false),
                    OtherTaxName = table.Column<string>(nullable: true),
                    OtherTaxPercentage = table.Column<decimal>(nullable: false),
                    Surcharge_percentage = table.Column<decimal>(nullable: false),
                    Exemption = table.Column<decimal>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_tax_slab_Details", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_tax_slab_Details_tbl_tax_slab_master_tax_slab_id",
                        column: x => x.tax_slab_id,
                        principalTable: "tbl_tax_slab_master",
                        principalColumn: "tax_slab_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_city",
                columns: table => new
                {
                    city_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(maxLength: 200, nullable: true),
                    state_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    pincode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_city", x => x.city_id);
                    table.ForeignKey(
                        name: "FK_tbl_city_tbl_state_state_id",
                        column: x => x.state_id,
                        principalTable: "tbl_state",
                        principalColumn: "state_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_comp_off_ledger_log",
                columns: table => new
                {
                    log_sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sno = table.Column<int>(nullable: true),
                    compoff_date = table.Column<DateTime>(nullable: false),
                    credit = table.Column<double>(nullable: false),
                    dredit = table.Column<double>(nullable: false),
                    transaction_date = table.Column<DateTime>(nullable: false),
                    transaction_type = table.Column<byte>(nullable: false),
                    monthyear = table.Column<int>(nullable: false),
                    transaction_no = table.Column<int>(nullable: false),
                    remarks = table.Column<string>(nullable: true),
                    e_id = table.Column<int>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_comp_off_ledger_log", x => x.log_sno);
                    table.ForeignKey(
                        name: "FK_tbl_comp_off_ledger_log_tbl_employee_master_e_id",
                        column: x => x.e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_comp_off_ledger_log_tbl_comp_off_ledger_sno",
                        column: x => x.sno,
                        principalTable: "tbl_comp_off_ledger",
                        principalColumn: "sno",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_department_master_log",
                columns: table => new
                {
                    department_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    department_id = table.Column<int>(nullable: true),
                    department_code = table.Column<string>(maxLength: 100, nullable: true),
                    department_name = table.Column<string>(maxLength: 200, nullable: true),
                    department_short_name = table.Column<string>(maxLength: 100, nullable: true),
                    department_head_employee_code = table.Column<string>(maxLength: 100, nullable: true),
                    department_head_employee_name = table.Column<string>(maxLength: 200, nullable: true),
                    company_id = table.Column<int>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_department_master_log", x => x.department_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_department_master_log_tbl_department_master_department_id",
                        column: x => x.department_id,
                        principalTable: "tbl_department_master",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_sub_department_master",
                columns: table => new
                {
                    sub_department_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sub_department_code = table.Column<string>(maxLength: 100, nullable: true),
                    sub_department_name = table.Column<string>(maxLength: 200, nullable: true),
                    department_id = table.Column<int>(nullable: true),
                    company_id = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sub_department_master", x => x.sub_department_id);
                    table.ForeignKey(
                        name: "FK_tbl_sub_department_master_tbl_department_master_department_id",
                        column: x => x.department_id,
                        principalTable: "tbl_department_master",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_fnf_loan_recover",
                columns: table => new
                {
                    loan_recovery_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    loan_req_id = table.Column<int>(nullable: true),
                    loan_recover_amt = table.Column<decimal>(nullable: false),
                    is_process = table.Column<byte>(nullable: false),
                    x_id = table.Column<int>(nullable: false),
                    emp_sep_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_fnf_loan_recover", x => x.loan_recovery_id);
                    table.ForeignKey(
                        name: "FK_tbl_fnf_loan_recover_tbl_loan_request_loan_req_id",
                        column: x => x.loan_req_id,
                        principalTable: "tbl_loan_request",
                        principalColumn: "loan_req_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_loan_approval",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    loan_req_id = table.Column<int>(nullable: true),
                    emp_id = table.Column<int>(nullable: true),
                    is_final_approver = table.Column<byte>(nullable: false),
                    is_approve = table.Column<byte>(nullable: false),
                    approval_order = table.Column<byte>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    approver_role_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_loan_approval", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_loan_approval_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_loan_approval_tbl_loan_request_loan_req_id",
                        column: x => x.loan_req_id,
                        principalTable: "tbl_loan_request",
                        principalColumn: "loan_req_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_loan_repayments",
                columns: table => new
                {
                    loan_repay_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    req_emp_id = table.Column<int>(nullable: true),
                    loan_id = table.Column<int>(nullable: true),
                    interest_component = table.Column<double>(nullable: false),
                    principal_amount = table.Column<double>(nullable: false),
                    loan_balance = table.Column<double>(nullable: false),
                    date = table.Column<DateTime>(nullable: false),
                    remark = table.Column<string>(maxLength: 200, nullable: true),
                    status = table.Column<byte>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_loan_repayments", x => x.loan_repay_id);
                    table.ForeignKey(
                        name: "FK_tbl_loan_repayments_tbl_loan_request_loan_id",
                        column: x => x.loan_id,
                        principalTable: "tbl_loan_request",
                        principalColumn: "loan_req_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_loan_repayments_tbl_employee_master_req_emp_id",
                        column: x => x.req_emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_active_inactive_user_log",
                columns: table => new
                {
                    acinac_user_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(nullable: true),
                    transaction_type = table.Column<byte>(nullable: false),
                    remarks = table.Column<string>(nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_active_inactive_user_log", x => x.acinac_user_id);
                    table.ForeignKey(
                        name: "FK_tbl_active_inactive_user_log_tbl_user_master_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_user_master",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_login_logs",
                columns: table => new
                {
                    log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    login_ip = table.Column<string>(nullable: true),
                    user_agent = table.Column<string>(nullable: true),
                    is_wrong_attempt = table.Column<byte>(nullable: false),
                    user_id = table.Column<int>(nullable: true),
                    emp_id = table.Column<int>(nullable: true),
                    login_date_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_login_logs", x => x.log_id);
                    table.ForeignKey(
                        name: "FK_tbl_user_login_logs_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_user_login_logs_tbl_user_master_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_user_master",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_user_role_map",
                columns: table => new
                {
                    claim_master_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    role_id = table.Column<int>(nullable: true),
                    user_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_user_role_map", x => x.claim_master_id);
                    table.ForeignKey(
                        name: "FK_tbl_user_role_map_tbl_role_master_role_id",
                        column: x => x.role_id,
                        principalTable: "tbl_role_master",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_user_role_map_tbl_user_master_user_id",
                        column: x => x.user_id,
                        principalTable: "tbl_user_master",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_rimb_cat_lmt_mstr",
                columns: table => new
                {
                    rclm_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    rglm_id = table.Column<int>(nullable: true),
                    rcm_id = table.Column<int>(nullable: true),
                    monthly_limit = table.Column<double>(nullable: false),
                    yearly_limit = table.Column<double>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    is_delete = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_rimb_cat_lmt_mstr", x => x.rclm_id);
                    table.ForeignKey(
                        name: "FK_tbl_rimb_cat_lmt_mstr_tbl_remimb_cat_mstr_rcm_id",
                        column: x => x.rcm_id,
                        principalTable: "tbl_remimb_cat_mstr",
                        principalColumn: "rcm_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_rimb_cat_lmt_mstr_tbl_rimb_grd_lmt_mstr_rglm_id",
                        column: x => x.rglm_id,
                        principalTable: "tbl_rimb_grd_lmt_mstr",
                        principalColumn: "rglm_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_sg_maping",
                columns: table => new
                {
                    map_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    salary_group_id = table.Column<int>(nullable: true),
                    emp_id = table.Column<int>(nullable: true),
                    applicable_from_dt = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sg_maping", x => x.map_id);
                    table.ForeignKey(
                        name: "FK_tbl_sg_maping_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_sg_maping_tbl_salary_group_salary_group_id",
                        column: x => x.salary_group_id,
                        principalTable: "tbl_salary_group",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_applicablity",
                columns: table => new
                {
                    leave_applicablity_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    leave_info_id = table.Column<int>(nullable: true),
                    is_aplicable_on_all_company = table.Column<byte>(nullable: false),
                    is_aplicable_on_all_location = table.Column<byte>(nullable: false),
                    is_aplicable_on_all_department = table.Column<byte>(nullable: false),
                    is_aplicable_on_all_religion = table.Column<byte>(nullable: false),
                    leave_applicable_for = table.Column<byte>(nullable: false),
                    day_part = table.Column<byte>(nullable: false),
                    leave_applicable_in_hours_and_minutes = table.Column<DateTime>(nullable: false),
                    employee_can_apply = table.Column<byte>(nullable: false),
                    admin_can_apply = table.Column<byte>(nullable: false),
                    is_aplicable_on_all_emp_type = table.Column<byte>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_applicablity", x => x.leave_applicablity_id);
                    table.ForeignKey(
                        name: "FK_tbl_leave_applicablity_tbl_leave_info_leave_info_id",
                        column: x => x.leave_info_id,
                        principalTable: "tbl_leave_info",
                        principalColumn: "leave_info_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_cashable",
                columns: table => new
                {
                    leave_cashable_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    leave_info_id = table.Column<int>(nullable: true),
                    is_cashable = table.Column<byte>(nullable: false),
                    cashable_type = table.Column<byte>(nullable: false),
                    cashable_after_year = table.Column<byte>(nullable: false),
                    maximum_cashable_leave = table.Column<int>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_cashable", x => x.leave_cashable_id);
                    table.ForeignKey(
                        name: "FK_tbl_leave_cashable_tbl_leave_info_leave_info_id",
                        column: x => x.leave_info_id,
                        principalTable: "tbl_leave_info",
                        principalColumn: "leave_info_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_credit",
                columns: table => new
                {
                    leave_credit_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    leave_info_id = table.Column<int>(nullable: true),
                    frequency_type = table.Column<byte>(nullable: false),
                    leave_credit_day = table.Column<decimal>(nullable: false),
                    is_half_day_applicable = table.Column<byte>(nullable: false),
                    leave_credit_number = table.Column<double>(nullable: false),
                    is_applicable_for_advance = table.Column<byte>(nullable: false),
                    advance_applicable_day = table.Column<int>(nullable: false),
                    is_leave_accrue = table.Column<byte>(nullable: false),
                    max_accrue = table.Column<int>(nullable: false),
                    is_required_certificate = table.Column<byte>(nullable: false),
                    certificate_path = table.Column<string>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_credit", x => x.leave_credit_id);
                    table.ForeignKey(
                        name: "FK_tbl_leave_credit_tbl_leave_info_leave_info_id",
                        column: x => x.leave_info_id,
                        principalTable: "tbl_leave_info",
                        principalColumn: "leave_info_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_info_log",
                columns: table => new
                {
                    leave_info_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    leave_info_id = table.Column<int>(nullable: true),
                    leave_type = table.Column<byte>(nullable: false),
                    leave_tenure_from_date = table.Column<DateTime>(nullable: false),
                    leave_tenure_to_date = table.Column<DateTime>(nullable: false),
                    leave_type_id = table.Column<int>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_info_log", x => x.leave_info_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_leave_info_log_tbl_leave_info_leave_info_id",
                        column: x => x.leave_info_id,
                        principalTable: "tbl_leave_info",
                        principalColumn: "leave_info_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_info_log_tbl_leave_type_leave_type_id",
                        column: x => x.leave_type_id,
                        principalTable: "tbl_leave_type",
                        principalColumn: "leave_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_ledger",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    leave_type_id = table.Column<int>(nullable: true),
                    leave_info_id = table.Column<int>(nullable: true),
                    transaction_date = table.Column<DateTime>(nullable: false),
                    entry_date = table.Column<DateTime>(nullable: false),
                    transaction_type = table.Column<byte>(nullable: false),
                    monthyear = table.Column<int>(nullable: false),
                    transaction_no = table.Column<int>(nullable: false),
                    leave_addition_type = table.Column<byte>(nullable: false),
                    credit = table.Column<double>(nullable: false),
                    dredit = table.Column<double>(nullable: false),
                    remarks = table.Column<string>(maxLength: 200, nullable: true),
                    e_id = table.Column<int>(nullable: true),
                    created_by = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_ledger", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_leave_ledger_tbl_employee_master_e_id",
                        column: x => x.e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_ledger_tbl_leave_info_leave_info_id",
                        column: x => x.leave_info_id,
                        principalTable: "tbl_leave_info",
                        principalColumn: "leave_info_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_ledger_tbl_leave_type_leave_type_id",
                        column: x => x.leave_type_id,
                        principalTable: "tbl_leave_type",
                        principalColumn: "leave_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_rule",
                columns: table => new
                {
                    leave_credit_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    leave_info_id = table.Column<int>(nullable: true),
                    applicable_if_employee_joined_before_dt = table.Column<DateTime>(nullable: false),
                    maximum_leave_clubbed_in_tenure_number_of_leave = table.Column<byte>(nullable: false),
                    maxi_negative_leave_applicable = table.Column<int>(nullable: false),
                    certificate_require_for_min_no_of_day = table.Column<byte>(nullable: false),
                    minimum_leave_applicable = table.Column<int>(nullable: false),
                    number_maximum_negative_leave_balance = table.Column<byte>(nullable: false),
                    maximum_leave_can_be_taken_type = table.Column<byte>(nullable: false),
                    maximum_leave_can_be_taken = table.Column<byte>(nullable: false),
                    maximum_leave_can_be_in_day = table.Column<byte>(nullable: false),
                    maximum_leave_can_be_taken_in_quater = table.Column<byte>(nullable: false),
                    can_carried_forward = table.Column<byte>(nullable: false),
                    maximum_carried_forward = table.Column<int>(nullable: false),
                    applied_sandwich_rule = table.Column<byte>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_rule", x => x.leave_credit_id);
                    table.ForeignKey(
                        name: "FK_tbl_leave_rule_tbl_leave_info_leave_info_id",
                        column: x => x.leave_info_id,
                        principalTable: "tbl_leave_info",
                        principalColumn: "leave_info_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_holiday_rel_list_log",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    h_r_id = table.Column<int>(nullable: true),
                    holiday_id = table.Column<int>(nullable: true),
                    religion_id = table.Column<int>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_holiday_rel_list_log", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_holiday_rel_list_log_tbl_holiday_mstr_rel_list_h_r_id",
                        column: x => x.h_r_id,
                        principalTable: "tbl_holiday_mstr_rel_list",
                        principalColumn: "sno",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_holiday_rel_list_log_tbl_holiday_master_holiday_id",
                        column: x => x.holiday_id,
                        principalTable: "tbl_holiday_master",
                        principalColumn: "holiday_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_holiday_rel_list_log_tbl_religion_master_religion_id",
                        column: x => x.religion_id,
                        principalTable: "tbl_religion_master",
                        principalColumn: "religion_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_shift_department",
                columns: table => new
                {
                    shift_department_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    shift_detail_id = table.Column<int>(nullable: true),
                    dept_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_shift_department", x => x.shift_department_id);
                    table.ForeignKey(
                        name: "FK_tbl_shift_department_tbl_department_master_dept_id",
                        column: x => x.dept_id,
                        principalTable: "tbl_department_master",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_department_tbl_shift_details_shift_detail_id",
                        column: x => x.shift_detail_id,
                        principalTable: "tbl_shift_details",
                        principalColumn: "shift_details_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_shift_week_off",
                columns: table => new
                {
                    shift_week_off_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    week_day = table.Column<byte>(nullable: false),
                    days = table.Column<byte>(nullable: false),
                    shift_detail_id = table.Column<int>(nullable: true),
                    emp_id = table.Column<int>(nullable: true),
                    emp_weekoff_id = table.Column<int>(nullable: true),
                    company_id = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_shift_week_off", x => x.shift_week_off_id);
                    table.ForeignKey(
                        name: "FK_tbl_shift_week_off_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_week_off_tbl_emp_weekoff_emp_weekoff_id",
                        column: x => x.emp_weekoff_id,
                        principalTable: "tbl_emp_weekoff",
                        principalColumn: "emp_weekoff_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_week_off_tbl_shift_details_shift_detail_id",
                        column: x => x.shift_detail_id,
                        principalTable: "tbl_shift_details",
                        principalColumn: "shift_details_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_shift_roster_master_log",
                columns: table => new
                {
                    shift_roster_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    r_id = table.Column<int>(nullable: true),
                    applicable_from_date = table.Column<DateTime>(nullable: false),
                    applicable_to_date = table.Column<DateTime>(nullable: false),
                    shift_rotat_in_day = table.Column<int>(nullable: false),
                    emp_id = table.Column<int>(nullable: true),
                    shift_id1 = table.Column<int>(nullable: true),
                    shift_id2 = table.Column<int>(nullable: true),
                    shift_id3 = table.Column<int>(nullable: true),
                    shift_id4 = table.Column<int>(nullable: true),
                    shift_id5 = table.Column<int>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_shift_roster_master_log", x => x.shift_roster_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_shift_roster_master_log_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_roster_master_log_tbl_shift_roster_master_r_id",
                        column: x => x.r_id,
                        principalTable: "tbl_shift_roster_master",
                        principalColumn: "shift_roster_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_roster_master_log_tbl_shift_details_shift_id1",
                        column: x => x.shift_id1,
                        principalTable: "tbl_shift_details",
                        principalColumn: "shift_details_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_roster_master_log_tbl_shift_details_shift_id2",
                        column: x => x.shift_id2,
                        principalTable: "tbl_shift_details",
                        principalColumn: "shift_details_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_roster_master_log_tbl_shift_details_shift_id3",
                        column: x => x.shift_id3,
                        principalTable: "tbl_shift_details",
                        principalColumn: "shift_details_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_roster_master_log_tbl_shift_details_shift_id4",
                        column: x => x.shift_id4,
                        principalTable: "tbl_shift_details",
                        principalColumn: "shift_details_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_roster_master_log_tbl_shift_details_shift_id5",
                        column: x => x.shift_id5,
                        principalTable: "tbl_shift_details",
                        principalColumn: "shift_details_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_company_master",
                columns: table => new
                {
                    company_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_code = table.Column<string>(maxLength: 50, nullable: false),
                    company_name = table.Column<string>(maxLength: 100, nullable: false),
                    is_emp_code_manual_genrate = table.Column<byte>(nullable: false),
                    address_line_one = table.Column<string>(maxLength: 250, nullable: true),
                    address_line_two = table.Column<string>(maxLength: 250, nullable: true),
                    pin_code = table.Column<int>(nullable: false),
                    city_id = table.Column<int>(nullable: true),
                    state_id = table.Column<int>(nullable: true),
                    country_id = table.Column<int>(nullable: true),
                    primary_email_id = table.Column<string>(maxLength: 50, nullable: true),
                    secondary_email_id = table.Column<string>(maxLength: 50, nullable: true),
                    primary_contact_number = table.Column<string>(maxLength: 20, nullable: true),
                    secondary_contact_number = table.Column<string>(maxLength: 20, nullable: true),
                    company_website = table.Column<string>(maxLength: 100, nullable: true),
                    company_logo = table.Column<string>(nullable: true),
                    user_type = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    total_emp = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_company_master", x => x.company_id);
                    table.ForeignKey(
                        name: "FK_tbl_company_master_tbl_city_city_id",
                        column: x => x.city_id,
                        principalTable: "tbl_city",
                        principalColumn: "city_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_company_master_tbl_country_country_id",
                        column: x => x.country_id,
                        principalTable: "tbl_country",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_company_master_tbl_state_state_id",
                        column: x => x.state_id,
                        principalTable: "tbl_state",
                        principalColumn: "state_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_location_master",
                columns: table => new
                {
                    location_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    location_code = table.Column<string>(maxLength: 100, nullable: true),
                    location_name = table.Column<string>(maxLength: 200, nullable: true),
                    type_of_location = table.Column<byte>(nullable: false),
                    address_line_one = table.Column<string>(maxLength: 250, nullable: true),
                    address_line_two = table.Column<string>(maxLength: 250, nullable: true),
                    pin_code = table.Column<int>(nullable: false),
                    city_id = table.Column<int>(nullable: true),
                    state_id = table.Column<int>(nullable: true),
                    country_id = table.Column<int>(nullable: true),
                    primary_email_id = table.Column<string>(maxLength: 100, nullable: true),
                    secondary_email_id = table.Column<string>(maxLength: 100, nullable: true),
                    primary_contact_number = table.Column<string>(maxLength: 20, nullable: true),
                    secondary_contact_number = table.Column<string>(maxLength: 20, nullable: true),
                    company_id = table.Column<int>(nullable: false),
                    website = table.Column<string>(nullable: true),
                    image = table.Column<string>(nullable: true),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_location_master", x => x.location_id);
                    table.ForeignKey(
                        name: "FK_tbl_location_master_tbl_city_city_id",
                        column: x => x.city_id,
                        principalTable: "tbl_city",
                        principalColumn: "city_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_location_master_tbl_country_country_id",
                        column: x => x.country_id,
                        principalTable: "tbl_country",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_location_master_tbl_state_state_id",
                        column: x => x.state_id,
                        principalTable: "tbl_state",
                        principalColumn: "state_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_sub_department_master_log",
                columns: table => new
                {
                    sub_department_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sid = table.Column<int>(nullable: true),
                    sub_department_code = table.Column<string>(maxLength: 100, nullable: true),
                    sub_department_name = table.Column<string>(maxLength: 200, nullable: true),
                    d_id = table.Column<int>(nullable: true),
                    company_id = table.Column<int>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sub_department_master_log", x => x.sub_department_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_sub_department_master_log_tbl_department_master_d_id",
                        column: x => x.d_id,
                        principalTable: "tbl_department_master",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_sub_department_master_log_tbl_sub_department_master_sid",
                        column: x => x.sid,
                        principalTable: "tbl_sub_department_master",
                        principalColumn: "sub_department_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_rimb_req_details",
                columns: table => new
                {
                    rrd_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    rrm_id = table.Column<int>(nullable: true),
                    rclm_id = table.Column<int>(nullable: true),
                    Bill_amount = table.Column<double>(nullable: false),
                    Bill_date = table.Column<DateTime>(nullable: false),
                    request_amount = table.Column<double>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    is_delete = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_rimb_req_details", x => x.rrd_id);
                    table.ForeignKey(
                        name: "FK_tbl_rimb_req_details_tbl_rimb_cat_lmt_mstr_rclm_id",
                        column: x => x.rclm_id,
                        principalTable: "tbl_rimb_cat_lmt_mstr",
                        principalColumn: "rclm_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_rimb_req_details_tbl_rimb_req_mstr_rrm_id",
                        column: x => x.rrm_id,
                        principalTable: "tbl_rimb_req_mstr",
                        principalColumn: "rrm_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_app_on_dept",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    lid = table.Column<int>(nullable: true),
                    id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_app_on_dept", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_leave_app_on_dept_tbl_department_master_id",
                        column: x => x.id,
                        principalTable: "tbl_department_master",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_app_on_dept_tbl_leave_applicablity_lid",
                        column: x => x.lid,
                        principalTable: "tbl_leave_applicablity",
                        principalColumn: "leave_applicablity_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_app_on_emp_type",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    l_app_id = table.Column<int>(nullable: true),
                    employment_type = table.Column<byte>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_app_on_emp_type", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_leave_app_on_emp_type_tbl_leave_applicablity_l_app_id",
                        column: x => x.l_app_id,
                        principalTable: "tbl_leave_applicablity",
                        principalColumn: "leave_applicablity_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_appcbl_on_religion",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    l_app_id = table.Column<int>(nullable: true),
                    religion_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_appcbl_on_religion", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_leave_appcbl_on_religion_tbl_leave_applicablity_l_app_id",
                        column: x => x.l_app_id,
                        principalTable: "tbl_leave_applicablity",
                        principalColumn: "leave_applicablity_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_appcbl_on_religion_tbl_religion_master_religion_id",
                        column: x => x.religion_id,
                        principalTable: "tbl_religion_master",
                        principalColumn: "religion_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_applicablity_log",
                columns: table => new
                {
                    leave_applicablity_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    l_app_id = table.Column<int>(nullable: true),
                    leave_info_id = table.Column<int>(nullable: true),
                    is_aplicable_on_all_company = table.Column<byte>(nullable: false),
                    is_aplicable_on_all_location = table.Column<byte>(nullable: false),
                    is_aplicable_on_all_department = table.Column<byte>(nullable: false),
                    is_aplicable_on_all_religion = table.Column<byte>(nullable: false),
                    leave_applicable_for = table.Column<byte>(nullable: false),
                    day_part = table.Column<byte>(nullable: false),
                    leave_applicable_in_hours_and_minutes = table.Column<DateTime>(nullable: false),
                    employee_can_apply = table.Column<byte>(nullable: false),
                    admin_can_apply = table.Column<byte>(nullable: false),
                    is_aplicable_on_all_emp_type = table.Column<byte>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_applicablity_log", x => x.leave_applicablity_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_leave_applicablity_log_tbl_leave_applicablity_l_app_id",
                        column: x => x.l_app_id,
                        principalTable: "tbl_leave_applicablity",
                        principalColumn: "leave_applicablity_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_applicablity_log_tbl_leave_info_leave_info_id",
                        column: x => x.leave_info_id,
                        principalTable: "tbl_leave_info",
                        principalColumn: "leave_info_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_cashable_log",
                columns: table => new
                {
                    leave_cashable_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    leave_cashable_id = table.Column<int>(nullable: true),
                    leave_info_id = table.Column<int>(nullable: true),
                    is_cashable = table.Column<byte>(nullable: false),
                    cashable_type = table.Column<byte>(nullable: false),
                    cashable_after_year = table.Column<byte>(nullable: false),
                    maximum_cashable_leave = table.Column<int>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_cashable_log", x => x.leave_cashable_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_leave_cashable_log_tbl_leave_cashable_leave_cashable_id",
                        column: x => x.leave_cashable_id,
                        principalTable: "tbl_leave_cashable",
                        principalColumn: "leave_cashable_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_cashable_log_tbl_leave_info_leave_info_id",
                        column: x => x.leave_info_id,
                        principalTable: "tbl_leave_info",
                        principalColumn: "leave_info_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_credit_log",
                columns: table => new
                {
                    leave_credit_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    leave_credit_id = table.Column<int>(nullable: true),
                    leave_info_id = table.Column<int>(nullable: true),
                    frequency_type = table.Column<byte>(nullable: false),
                    is_half_day_applicable = table.Column<byte>(nullable: false),
                    is_applicable_for_advance = table.Column<byte>(nullable: false),
                    advance_applicable_day = table.Column<int>(nullable: false),
                    is_leave_accrue = table.Column<byte>(nullable: false),
                    max_accrue = table.Column<int>(nullable: false),
                    is_required_certificate = table.Column<byte>(nullable: false),
                    certificate_path = table.Column<string>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_credit_log", x => x.leave_credit_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_leave_credit_log_tbl_leave_credit_leave_credit_id",
                        column: x => x.leave_credit_id,
                        principalTable: "tbl_leave_credit",
                        principalColumn: "leave_credit_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_credit_log_tbl_leave_info_leave_info_id",
                        column: x => x.leave_info_id,
                        principalTable: "tbl_leave_info",
                        principalColumn: "leave_info_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_ledger_log",
                columns: table => new
                {
                    log_sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sno = table.Column<int>(nullable: true),
                    leave_type_id = table.Column<int>(nullable: true),
                    transaction_date = table.Column<DateTime>(nullable: false),
                    transaction_type = table.Column<string>(nullable: true),
                    monthyear = table.Column<int>(nullable: false),
                    transaction_no = table.Column<int>(nullable: false),
                    leave_addition_type = table.Column<byte>(nullable: false),
                    credit = table.Column<double>(nullable: false),
                    dredit = table.Column<double>(nullable: false),
                    remarks = table.Column<string>(nullable: true),
                    e_id = table.Column<int>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_ledger_log", x => x.log_sno);
                    table.ForeignKey(
                        name: "FK_tbl_leave_ledger_log_tbl_employee_master_e_id",
                        column: x => x.e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_ledger_log_tbl_leave_type_leave_type_id",
                        column: x => x.leave_type_id,
                        principalTable: "tbl_leave_type",
                        principalColumn: "leave_type_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_ledger_log_tbl_leave_ledger_sno",
                        column: x => x.sno,
                        principalTable: "tbl_leave_ledger",
                        principalColumn: "sno",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_rule_log",
                columns: table => new
                {
                    leave_credit_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    leave_credit_id = table.Column<int>(nullable: true),
                    leave_info_id = table.Column<int>(nullable: true),
                    applicable_if_employee_joined_before_dt = table.Column<DateTime>(nullable: false),
                    maximum_leave_clubbed_in_tenure_number_of_leave = table.Column<byte>(nullable: false),
                    maxi_negative_leave_applicable = table.Column<int>(nullable: false),
                    certificate_require_for_min_no_of_day = table.Column<byte>(nullable: false),
                    minimum_leave_applicable = table.Column<int>(nullable: false),
                    number_maximum_negative_leave_balance = table.Column<byte>(nullable: false),
                    maximum_leave_can_be_taken_type = table.Column<byte>(nullable: false),
                    maximum_leave_can_be_taken = table.Column<byte>(nullable: false),
                    maximum_leave_can_be_in_day = table.Column<byte>(nullable: false),
                    maximum_leave_can_be_taken_in_quater = table.Column<byte>(nullable: false),
                    can_carried_forward = table.Column<byte>(nullable: false),
                    maximum_carried_forward = table.Column<int>(nullable: false),
                    applied_sandwich_rule = table.Column<byte>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_rule_log", x => x.leave_credit_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_leave_rule_log_tbl_leave_rule_leave_credit_id",
                        column: x => x.leave_credit_id,
                        principalTable: "tbl_leave_rule",
                        principalColumn: "leave_credit_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_rule_log_tbl_leave_info_leave_info_id",
                        column: x => x.leave_info_id,
                        principalTable: "tbl_leave_info",
                        principalColumn: "leave_info_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_shift_week_off_log",
                columns: table => new
                {
                    shift_week_off_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    shift_week_off_id = table.Column<int>(nullable: true),
                    week_day = table.Column<byte>(nullable: false),
                    days = table.Column<byte>(nullable: false),
                    shift_id = table.Column<int>(nullable: true),
                    emp_id = table.Column<int>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_shift_week_off_log", x => x.shift_week_off_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_shift_week_off_log_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_week_off_log_tbl_shift_master_shift_id",
                        column: x => x.shift_id,
                        principalTable: "tbl_shift_master",
                        principalColumn: "shift_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_week_off_log_tbl_shift_week_off_shift_week_off_id",
                        column: x => x.shift_week_off_id,
                        principalTable: "tbl_shift_week_off",
                        principalColumn: "shift_week_off_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_approved_emp_separation_cancellation",
                columns: table => new
                {
                    pkid_AppEmpSepCancel = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fkid_empSepration = table.Column<int>(nullable: false),
                    cancellation_dt = table.Column<DateTime>(nullable: false),
                    cancel_remarks = table.Column<string>(nullable: true),
                    approver1_id = table.Column<int>(nullable: true),
                    is_approved1 = table.Column<int>(nullable: false),
                    app1_remarks = table.Column<string>(nullable: true),
                    app1_dt = table.Column<DateTime>(nullable: false),
                    approver2_id = table.Column<int>(nullable: true),
                    is_approved2 = table.Column<int>(nullable: false),
                    app2_remarks = table.Column<string>(nullable: true),
                    app2_dt = table.Column<DateTime>(nullable: false),
                    apprver3_id = table.Column<int>(nullable: true),
                    is_approved3 = table.Column<int>(nullable: false),
                    app3_remarks = table.Column<string>(nullable: true),
                    app3_dt = table.Column<DateTime>(nullable: false),
                    admin_id = table.Column<int>(nullable: true),
                    is_admin_approved = table.Column<int>(nullable: false),
                    admin_remarks = table.Column<string>(nullable: true),
                    admin_dt = table.Column<DateTime>(nullable: false),
                    is_final_approve = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_approved_emp_separation_cancellation", x => x.pkid_AppEmpSepCancel);
                    table.ForeignKey(
                        name: "FK_tbl_approved_emp_separation_cancellation_tbl_employee_master~",
                        column: x => x.admin_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_approved_emp_separation_cancellation_tbl_employee_maste~1",
                        column: x => x.approver1_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_approved_emp_separation_cancellation_tbl_employee_maste~2",
                        column: x => x.approver2_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_approved_emp_separation_cancellation_tbl_employee_maste~3",
                        column: x => x.apprver3_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_approved_emp_separation_cancellation_tbl_company_master_~",
                        column: x => x.fkid_empSepration,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_assets_master",
                columns: table => new
                {
                    asset_master_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    asset_name = table.Column<string>(maxLength: 50, nullable: true),
                    short_name = table.Column<string>(maxLength: 50, nullable: true),
                    description = table.Column<string>(maxLength: 50, nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    company_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_assets_master", x => x.asset_master_id);
                    table.ForeignKey(
                        name: "FK_tbl_assets_master_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_attendace_request",
                columns: table => new
                {
                    leave_request_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    from_date = table.Column<DateTime>(nullable: false),
                    system_in_time = table.Column<DateTime>(nullable: false),
                    system_out_time = table.Column<DateTime>(nullable: false),
                    manual_in_time = table.Column<DateTime>(nullable: false),
                    manual_out_time = table.Column<DateTime>(nullable: false),
                    r_e_id = table.Column<int>(nullable: true),
                    a1_e_id = table.Column<int>(nullable: true),
                    a2_e_id = table.Column<int>(nullable: true),
                    a3_e_id = table.Column<int>(nullable: true),
                    requester_date = table.Column<DateTime>(nullable: false),
                    approval_date1 = table.Column<DateTime>(nullable: false),
                    approval_date2 = table.Column<DateTime>(nullable: false),
                    approval_date3 = table.Column<DateTime>(nullable: false),
                    is_approved1 = table.Column<byte>(nullable: false),
                    is_approved2 = table.Column<byte>(nullable: false),
                    is_approved3 = table.Column<byte>(nullable: false),
                    is_final_approve = table.Column<byte>(nullable: false),
                    requester_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    approval1_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    approval2_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    approval3_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    deleted_by = table.Column<int>(nullable: false),
                    deleted_dt = table.Column<DateTime>(nullable: false),
                    deleted_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    company_id = table.Column<int>(nullable: true),
                    admin_id = table.Column<int>(nullable: true),
                    is_admin_approve = table.Column<int>(nullable: false),
                    admin_remarks = table.Column<string>(nullable: true),
                    admin_ar_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_attendace_request", x => x.leave_request_id);
                    table.ForeignKey(
                        name: "FK_tbl_attendace_request_tbl_employee_master_a1_e_id",
                        column: x => x.a1_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_attendace_request_tbl_employee_master_a2_e_id",
                        column: x => x.a2_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_attendace_request_tbl_employee_master_a3_e_id",
                        column: x => x.a3_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_attendace_request_tbl_employee_master_admin_id",
                        column: x => x.admin_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_attendace_request_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_attendace_request_tbl_employee_master_r_e_id",
                        column: x => x.r_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_comp_off_request_master",
                columns: table => new
                {
                    comp_off_request_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    compoff_date = table.Column<DateTime>(nullable: false),
                    compoff_against_date = table.Column<DateTime>(nullable: false),
                    compoff_request_qty = table.Column<double>(nullable: false),
                    r_e_id = table.Column<int>(nullable: true),
                    a1_e_id = table.Column<int>(nullable: true),
                    a2_e_id = table.Column<int>(nullable: true),
                    a3_e_id = table.Column<int>(nullable: true),
                    requester_date = table.Column<DateTime>(nullable: false),
                    approval_date1 = table.Column<DateTime>(nullable: false),
                    approval_date2 = table.Column<DateTime>(nullable: false),
                    approval_date3 = table.Column<DateTime>(nullable: false),
                    is_approved1 = table.Column<byte>(nullable: false),
                    is_approved2 = table.Column<byte>(nullable: false),
                    is_approved3 = table.Column<byte>(nullable: false),
                    is_final_approve = table.Column<byte>(nullable: false),
                    requester_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    approval1_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    approval2_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    approval3_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    deleted_by = table.Column<int>(nullable: false),
                    deleted_dt = table.Column<DateTime>(nullable: false),
                    deleted_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    company_id = table.Column<int>(nullable: true),
                    admin_id = table.Column<int>(nullable: true),
                    is_admin_approve = table.Column<int>(nullable: false),
                    admin_remarks = table.Column<string>(nullable: true),
                    admin_ar_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_comp_off_request_master", x => x.comp_off_request_id);
                    table.ForeignKey(
                        name: "FK_tbl_comp_off_request_master_tbl_employee_master_a1_e_id",
                        column: x => x.a1_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_comp_off_request_master_tbl_employee_master_a2_e_id",
                        column: x => x.a2_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_comp_off_request_master_tbl_employee_master_a3_e_id",
                        column: x => x.a3_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_comp_off_request_master_tbl_employee_master_admin_id",
                        column: x => x.admin_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_comp_off_request_master_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_comp_off_request_master_tbl_employee_master_r_e_id",
                        column: x => x.r_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_company_emp_setting",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    prefix_for_employee_code = table.Column<string>(nullable: true),
                    number_of_character_for_employee_code = table.Column<int>(nullable: false),
                    from_range = table.Column<int>(nullable: false),
                    current_range = table.Column<int>(nullable: false),
                    to_range = table.Column<int>(nullable: false),
                    is_active = table.Column<byte>(nullable: false),
                    last_genrated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_company_emp_setting", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_company_emp_setting_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_company_master_log",
                columns: table => new
                {
                    company_id_log = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    company_code = table.Column<string>(maxLength: 50, nullable: false),
                    company_name = table.Column<string>(nullable: false),
                    prefix_for_employee_code = table.Column<string>(nullable: true),
                    number_of_character_for_employee_code = table.Column<int>(nullable: false),
                    address_line_one = table.Column<string>(maxLength: 250, nullable: true),
                    address_line_two = table.Column<string>(maxLength: 250, nullable: true),
                    pin_code = table.Column<int>(nullable: false),
                    city = table.Column<int>(nullable: false),
                    state = table.Column<int>(nullable: false),
                    country = table.Column<int>(nullable: false),
                    primary_email_id = table.Column<string>(maxLength: 200, nullable: true),
                    secondary_email_id = table.Column<string>(maxLength: 200, nullable: true),
                    primary_contact_number = table.Column<string>(maxLength: 20, nullable: true),
                    secondary_contact_number = table.Column<string>(maxLength: 20, nullable: true),
                    company_website = table.Column<string>(maxLength: 100, nullable: true),
                    company_logo = table.Column<string>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_company_master_log", x => x.company_id_log);
                    table.ForeignKey(
                        name: "FK_tbl_company_master_log_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_component_formula_details",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    component_id = table.Column<int>(nullable: true),
                    company_id = table.Column<int>(nullable: true),
                    salary_group_id = table.Column<int>(nullable: true),
                    formula = table.Column<string>(nullable: true),
                    function_calling_order = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    deleted_by = table.Column<int>(nullable: false),
                    deleted_dt = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_component_formula_details", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_component_formula_details_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tcpd_component_id",
                        column: x => x.component_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tcpd_tsg_salarygroup_id",
                        column: x => x.salary_group_id,
                        principalTable: "tbl_salary_group",
                        principalColumn: "group_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_document_type_master",
                columns: table => new
                {
                    doc_type_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: false),
                    doc_name = table.Column<string>(maxLength: 50, nullable: true),
                    remarks = table.Column<string>(maxLength: 200, nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_document_type_master", x => x.doc_type_id);
                    table.ForeignKey(
                        name: "FK_tbl_document_type_master_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_prev_employement",
                columns: table => new
                {
                    emp_pr_employment_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    current_comp_id = table.Column<int>(nullable: false),
                    emp_id = table.Column<int>(nullable: true),
                    pr_comp_name = table.Column<string>(maxLength: 100, nullable: true),
                    pr_comp_address = table.Column<string>(maxLength: 500, nullable: true),
                    pr_comp_doj = table.Column<DateTime>(nullable: false),
                    pr_comp_relieve_dt = table.Column<DateTime>(nullable: false),
                    is_relieved = table.Column<int>(nullable: false),
                    designation = table.Column<string>(maxLength: 100, nullable: true),
                    salary = table.Column<decimal>(nullable: false),
                    job_type = table.Column<int>(nullable: false),
                    relieve_reason = table.Column<string>(maxLength: 500, nullable: true),
                    reporting_to = table.Column<string>(nullable: true),
                    reporting_name = table.Column<string>(maxLength: 100, nullable: true),
                    rpting_no = table.Column<string>(nullable: true),
                    rpting_email = table.Column<string>(maxLength: 100, nullable: true),
                    remarks = table.Column<string>(maxLength: 500, nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modifed_dt = table.Column<DateTime>(nullable: false),
                    relevant_exp = table.Column<decimal>(nullable: false),
                    total_exp = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_prev_employement", x => x.emp_pr_employment_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_prev_employement_tbl_company_master_current_comp_id",
                        column: x => x.current_comp_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_emp_prev_employement_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_separation",
                columns: table => new
                {
                    sepration_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: false),
                    company_id = table.Column<int>(nullable: false),
                    resignation_dt = table.Column<DateTime>(nullable: false),
                    req_relieving_date = table.Column<DateTime>(nullable: false),
                    req_notice_days = table.Column<int>(nullable: false),
                    diff_notice_days = table.Column<int>(nullable: false),
                    policy_relieving_dt = table.Column<DateTime>(nullable: false),
                    is_relieving_dt_change = table.Column<int>(nullable: false),
                    last_wrking_dt = table.Column<DateTime>(nullable: false),
                    req_reason = table.Column<string>(nullable: true),
                    req_remarks = table.Column<string>(nullable: true),
                    approver1_id = table.Column<int>(nullable: true),
                    is_approved1 = table.Column<int>(nullable: false),
                    app1_remarks = table.Column<string>(nullable: true),
                    app1_dt = table.Column<DateTime>(nullable: false),
                    approver2_id = table.Column<int>(nullable: true),
                    is_approved2 = table.Column<int>(nullable: false),
                    app2_remarks = table.Column<string>(nullable: true),
                    app2_dt = table.Column<DateTime>(nullable: false),
                    apprver3_id = table.Column<int>(nullable: true),
                    is_approved3 = table.Column<int>(nullable: false),
                    app3_remarks = table.Column<string>(nullable: true),
                    app3_dt = table.Column<DateTime>(nullable: false),
                    admin_id = table.Column<int>(nullable: true),
                    is_admin_approved = table.Column<int>(nullable: false),
                    admin_remarks = table.Column<string>(nullable: true),
                    admin_dt = table.Column<DateTime>(nullable: false),
                    is_final_approve = table.Column<int>(nullable: false),
                    final_relieve_dt = table.Column<DateTime>(nullable: false),
                    is_cancel = table.Column<int>(nullable: false),
                    cancel_remarks = table.Column<string>(nullable: true),
                    cancelation_dt = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    notice_day = table.Column<int>(nullable: false),
                    withdrawal_type = table.Column<string>(maxLength: 250, nullable: true),
                    salary_process_type = table.Column<int>(nullable: false),
                    gratuity = table.Column<int>(nullable: false),
                    is_withdrawal = table.Column<int>(nullable: false),
                    is_no_due_cleared = table.Column<int>(nullable: false),
                    is_kt_transfered = table.Column<int>(nullable: false),
                    ref_doc_path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_separation", x => x.sepration_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_separation_tbl_employee_master_admin_id",
                        column: x => x.admin_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_separation_tbl_employee_master_approver1_id",
                        column: x => x.approver1_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_separation_tbl_employee_master_approver2_id",
                        column: x => x.approver2_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_separation_tbl_employee_master_apprver3_id",
                        column: x => x.apprver3_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_separation_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_emp_separation_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_withdrawal",
                columns: table => new
                {
                    withdrawal_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    emp_id = table.Column<int>(nullable: true),
                    resign_dt = table.Column<DateTime>(nullable: false),
                    last_wrking_dt = table.Column<DateTime>(nullable: false),
                    notice_day = table.Column<int>(nullable: false),
                    reason = table.Column<string>(maxLength: 250, nullable: true),
                    withdrawal_type = table.Column<string>(maxLength: 250, nullable: true),
                    salary_process_type = table.Column<byte>(nullable: false),
                    gratuity = table.Column<byte>(nullable: false),
                    remarks = table.Column<string>(maxLength: 500, nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modify_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_withdrawal", x => x.withdrawal_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_withdrawal_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_withdrawal_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_employee_company_map",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    company_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    is_default = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_employee_company_map", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_employee_company_map_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_employee_company_map_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_employeementtype_settings",
                columns: table => new
                {
                    typesetting_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    grade_id = table.Column<int>(nullable: false),
                    employeement_type = table.Column<int>(nullable: false),
                    notice_period = table.Column<int>(nullable: false),
                    notice_period_days = table.Column<int>(nullable: false),
                    remarks = table.Column<string>(maxLength: 200, nullable: true),
                    company_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_employeementtype_settings", x => x.typesetting_id);
                    table.ForeignKey(
                        name: "FK_tbl_employeementtype_settings_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_employeementtype_settings_tbl_grade_master_grade_id",
                        column: x => x.grade_id,
                        principalTable: "tbl_grade_master",
                        principalColumn: "grade_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_epa_cycle_master",
                columns: table => new
                {
                    cycle_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cycle_type = table.Column<byte>(nullable: false),
                    number_of_day = table.Column<int>(nullable: false),
                    from_date = table.Column<DateTime>(nullable: false),
                    company_id = table.Column<int>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_epa_cycle_master", x => x.cycle_id);
                    table.ForeignKey(
                        name: "FK_tbl_epa_cycle_master_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_epa_fiscal_yr_mstr",
                columns: table => new
                {
                    fiscal_year_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    financial_year_name = table.Column<string>(nullable: true),
                    from_date = table.Column<DateTime>(nullable: false),
                    to_date = table.Column<DateTime>(nullable: false),
                    company_id = table.Column<int>(nullable: true),
                    created_date = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_epa_fiscal_yr_mstr", x => x.fiscal_year_id);
                    table.ForeignKey(
                        name: "FK_tbl_epa_fiscal_yr_mstr_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_epa_status_master",
                columns: table => new
                {
                    epa_status_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    status_name = table.Column<string>(maxLength: 250, nullable: true),
                    description = table.Column<string>(maxLength: 500, nullable: true),
                    display_for = table.Column<int>(nullable: false),
                    display_for_rm1 = table.Column<int>(nullable: false),
                    display_for_rm2 = table.Column<int>(nullable: false),
                    display_for_rm3 = table.Column<int>(nullable: false),
                    display_order = table.Column<int>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_epa_status_master", x => x.epa_status_id);
                    table.ForeignKey(
                        name: "FK_tbl_epa_status_master_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_event_master",
                columns: table => new
                {
                    event_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    event_name = table.Column<string>(maxLength: 50, nullable: true),
                    event_place = table.Column<string>(maxLength: 150, nullable: true),
                    event_date = table.Column<DateTime>(nullable: false),
                    event_time = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_event_master", x => x.event_id);
                    table.ForeignKey(
                        name: "FK_tbl_event_master_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_holiday_master_emp_list",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    holiday_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    company_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_holiday_master_emp_list", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_holiday_master_emp_list_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_holiday_master_emp_list_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_holiday_master_emp_list_tbl_holiday_master_holiday_id",
                        column: x => x.holiday_id,
                        principalTable: "tbl_holiday_master",
                        principalColumn: "holiday_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_kpi_criteria",
                columns: table => new
                {
                    kpi_cr_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    criteria_count = table.Column<int>(nullable: false),
                    description = table.Column<string>(maxLength: 500, nullable: true),
                    is_active = table.Column<byte>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_kpi_criteria", x => x.kpi_cr_id);
                    table.ForeignKey(
                        name: "FK_tbl_kpi_criteria_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_kpi_objective_type",
                columns: table => new
                {
                    obj_type_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    objective_name = table.Column<string>(maxLength: 500, nullable: true),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    deleted_by = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_kpi_objective_type", x => x.obj_type_id);
                    table.ForeignKey(
                        name: "FK_tbl_kpi_objective_type_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_kpi_rating_master",
                columns: table => new
                {
                    kpi_rating_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    rating_name = table.Column<string>(maxLength: 10, nullable: true),
                    description = table.Column<string>(maxLength: 500, nullable: true),
                    display_order = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    deleted_by = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_kpi_rating_master", x => x.kpi_rating_id);
                    table.ForeignKey(
                        name: "FK_tbl_kpi_rating_master_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_request",
                columns: table => new
                {
                    leave_request_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    from_date = table.Column<DateTime>(nullable: false),
                    to_date = table.Column<DateTime>(nullable: false),
                    leave_qty = table.Column<double>(nullable: false),
                    leave_type_id = table.Column<int>(nullable: true),
                    leave_info_id = table.Column<int>(nullable: true),
                    leave_applicable_for = table.Column<byte>(nullable: false),
                    day_part = table.Column<byte>(nullable: false),
                    leave_applicable_in_hours_and_minutes = table.Column<DateTime>(nullable: false),
                    r_e_id = table.Column<int>(nullable: true),
                    a1_e_id = table.Column<int>(nullable: true),
                    a2_e_id = table.Column<int>(nullable: true),
                    a3_e_id = table.Column<int>(nullable: true),
                    requester_date = table.Column<DateTime>(nullable: false),
                    approval_date1 = table.Column<DateTime>(nullable: false),
                    approval_date2 = table.Column<DateTime>(nullable: false),
                    approval_date3 = table.Column<DateTime>(nullable: false),
                    is_approved1 = table.Column<byte>(nullable: false),
                    is_approved2 = table.Column<byte>(nullable: false),
                    is_approved3 = table.Column<byte>(nullable: false),
                    is_final_approve = table.Column<byte>(nullable: false),
                    requester_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    approval1_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    approval2_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    approval3_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    deleted_by = table.Column<int>(nullable: false),
                    deleted_dt = table.Column<DateTime>(nullable: false),
                    deleted_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    company_id = table.Column<int>(nullable: true),
                    admin_id = table.Column<int>(nullable: true),
                    is_admin_approve = table.Column<int>(nullable: false),
                    admin_remarks = table.Column<string>(nullable: true),
                    admin_ar_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_request", x => x.leave_request_id);
                    table.ForeignKey(
                        name: "FK_tbl_leave_request_tbl_employee_master_a1_e_id",
                        column: x => x.a1_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_request_tbl_employee_master_a2_e_id",
                        column: x => x.a2_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_request_tbl_employee_master_a3_e_id",
                        column: x => x.a3_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_request_tbl_employee_master_admin_id",
                        column: x => x.admin_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_request_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_request_tbl_leave_info_leave_info_id",
                        column: x => x.leave_info_id,
                        principalTable: "tbl_leave_info",
                        principalColumn: "leave_info_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_request_tbl_leave_type_leave_type_id",
                        column: x => x.leave_type_id,
                        principalTable: "tbl_leave_type",
                        principalColumn: "leave_type_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_request_tbl_employee_master_r_e_id",
                        column: x => x.r_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_loan_approval_setting",
                columns: table => new
                {
                    Sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    order = table.Column<int>(nullable: false),
                    emp_id = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    company_id = table.Column<int>(nullable: false),
                    is_final_approver = table.Column<byte>(nullable: false),
                    approver_role_id = table.Column<int>(nullable: true),
                    approver_type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_loan_approval_setting", x => x.Sno);
                    table.ForeignKey(
                        name: "FK_tbl_loan_approval_setting_tbl_role_master_approver_role_id",
                        column: x => x.approver_role_id,
                        principalTable: "tbl_role_master",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_loan_approval_setting_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_muster_form1",
                columns: table => new
                {
                    form_I_mstr_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nature_and_date_c_id = table.Column<int>(nullable: true),
                    whether_workman_c_id = table.Column<int>(nullable: true),
                    rate_of_wages_c_id = table.Column<int>(nullable: true),
                    date_c_id = table.Column<int>(nullable: true),
                    amt_c_id = table.Column<int>(nullable: true),
                    date_realised_c_id = table.Column<int>(nullable: true),
                    remarks_c_id = table.Column<int>(nullable: true),
                    comp_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_muster_form1", x => x.form_I_mstr_id);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form1_tbl_component_master_amt_c_id",
                        column: x => x.amt_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form1_tbl_company_master_comp_id",
                        column: x => x.comp_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form1_tbl_component_master_date_c_id",
                        column: x => x.date_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form1_tbl_component_master_date_realised_c_id",
                        column: x => x.date_realised_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form1_tbl_component_master_nature_and_date_c_id",
                        column: x => x.nature_and_date_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form1_tbl_component_master_rate_of_wages_c_id",
                        column: x => x.rate_of_wages_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form1_tbl_component_master_remarks_c_id",
                        column: x => x.remarks_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form1_tbl_component_master_whether_workman_c_id",
                        column: x => x.whether_workman_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_muster_form1_data",
                columns: table => new
                {
                    form_i_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: true),
                    company_id = table.Column<int>(nullable: true),
                    payroll_month = table.Column<int>(nullable: false),
                    employee_code = table.Column<string>(nullable: true),
                    employee_name = table.Column<string>(maxLength: 150, nullable: true),
                    father_or_husband_name = table.Column<string>(maxLength: 150, nullable: true),
                    department = table.Column<string>(nullable: true),
                    nature_and_date = table.Column<string>(maxLength: 100, nullable: true),
                    whether_workman = table.Column<string>(maxLength: 100, nullable: true),
                    rate_of_wages = table.Column<string>(maxLength: 50, nullable: true),
                    date_of_fine_imposed = table.Column<string>(maxLength: 50, nullable: true),
                    amt_of_fine_imposed = table.Column<string>(maxLength: 50, nullable: true),
                    date_realised = table.Column<string>(maxLength: 50, nullable: true),
                    remarks = table.Column<string>(maxLength: 200, nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_muster_form1_data", x => x.form_i_id);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form1_data_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form1_data_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_muster_form10",
                columns: table => new
                {
                    form_X_mstr_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    basic_min_rate_pay_c_id = table.Column<int>(nullable: true),
                    da_min_rate_pay_c_id = table.Column<int>(nullable: true),
                    basic_wages_actually_pay_c_id = table.Column<int>(nullable: true),
                    da_wages_actually_pay_c_id = table.Column<int>(nullable: true),
                    total_attan_orunit_ofworkdone_c_id = table.Column<int>(nullable: true),
                    overtime_worked_c_id = table.Column<int>(nullable: true),
                    gross_wages_pay_c_id = table.Column<int>(nullable: true),
                    employers_pf_c_id = table.Column<int>(nullable: true),
                    hr_deduction_c_id = table.Column<int>(nullable: true),
                    other_deduction_c_id = table.Column<int>(nullable: true),
                    total_deduction_c_id = table.Column<int>(nullable: true),
                    wages_paid_c_id = table.Column<int>(nullable: true),
                    date_ofpayment_c_id = table.Column<int>(nullable: true),
                    comp_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_muster_form10", x => x.form_X_mstr_id);
                    table.ForeignKey(
                        name: "FK_tblmst_tcpd_mb",
                        column: x => x.basic_min_rate_pay_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblmst_tcpd_ab",
                        column: x => x.basic_wages_actually_pay_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form10_tbl_company_master_comp_id",
                        column: x => x.comp_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblmst_tcpd_mda",
                        column: x => x.da_min_rate_pay_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblmst_tcpd_ada",
                        column: x => x.da_wages_actually_pay_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblmst_tcpd_dop",
                        column: x => x.date_ofpayment_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblmst_tcpd_epf",
                        column: x => x.employers_pf_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblmst_tcpd_gw",
                        column: x => x.gross_wages_pay_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblmst_tcpd_hrd",
                        column: x => x.hr_deduction_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblmst_tcpd_od",
                        column: x => x.other_deduction_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblmst_tcpd_ow",
                        column: x => x.overtime_worked_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblmst_tcpd_ta",
                        column: x => x.total_attan_orunit_ofworkdone_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblmst_tcpd_td",
                        column: x => x.total_deduction_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblmst_tcpd_wp",
                        column: x => x.wages_paid_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_muster_form10_data",
                columns: table => new
                {
                    form_x_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: true),
                    company_id = table.Column<int>(nullable: true),
                    payroll_month = table.Column<int>(nullable: false),
                    employee_code = table.Column<string>(nullable: true),
                    employee_name = table.Column<string>(maxLength: 150, nullable: true),
                    father_or_husband_name = table.Column<string>(maxLength: 150, nullable: true),
                    designation = table.Column<string>(nullable: true),
                    basic_minimum_payable = table.Column<string>(maxLength: 50, nullable: true),
                    da_minimum_payable = table.Column<string>(maxLength: 50, nullable: true),
                    basic_wages_actually_pay = table.Column<string>(maxLength: 50, nullable: true),
                    da_wages_actually_pay = table.Column<string>(maxLength: 50, nullable: true),
                    total_attand_or_unitof_work_done = table.Column<string>(maxLength: 50, nullable: true),
                    overtime_worked = table.Column<string>(maxLength: 50, nullable: true),
                    gross_wages_pay = table.Column<string>(maxLength: 50, nullable: true),
                    contri_of_employer_pf = table.Column<string>(maxLength: 50, nullable: true),
                    hr_deduction = table.Column<string>(maxLength: 50, nullable: true),
                    other_deduction = table.Column<string>(maxLength: 50, nullable: true),
                    total_deduction = table.Column<string>(maxLength: 50, nullable: true),
                    wages_paid = table.Column<string>(maxLength: 50, nullable: true),
                    date_of_payment = table.Column<string>(maxLength: 50, nullable: true),
                    emp_sign_orthump_exp = table.Column<string>(nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_muster_form10_data", x => x.form_x_id);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form10_data_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form10_data_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_muster_form11",
                columns: table => new
                {
                    form_XI_mstr_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    basic_wages_pay_c_id = table.Column<int>(nullable: true),
                    da_wages_pay_c_id = table.Column<int>(nullable: true),
                    total_attand_orwork_done_c_id = table.Column<int>(nullable: true),
                    overtime_wages_c_id = table.Column<int>(nullable: true),
                    gross_wages_pay_c_id = table.Column<int>(nullable: true),
                    total_deduction_c_id = table.Column<int>(nullable: true),
                    net_wages_pay_c_id = table.Column<int>(nullable: true),
                    pay_incharge_c_id = table.Column<int>(nullable: true),
                    comp_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_muster_form11", x => x.form_XI_mstr_id);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form11_tbl_component_master_basic_wages_pay_c_id",
                        column: x => x.basic_wages_pay_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form11_tbl_company_master_comp_id",
                        column: x => x.comp_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form11_tbl_component_master_da_wages_pay_c_id",
                        column: x => x.da_wages_pay_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form11_tbl_component_master_gross_wages_pay_c_id",
                        column: x => x.gross_wages_pay_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form11_tbl_component_master_net_wages_pay_c_id",
                        column: x => x.net_wages_pay_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form11_tbl_component_master_overtime_wages_c_id",
                        column: x => x.overtime_wages_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form11_tbl_component_master_pay_incharge_c_id",
                        column: x => x.pay_incharge_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblmst11_tcpd_ta",
                        column: x => x.total_attand_orwork_done_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form11_tbl_component_master_total_deduction_c_id",
                        column: x => x.total_deduction_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_muster_form11_data",
                columns: table => new
                {
                    form_xi_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: true),
                    company_id = table.Column<int>(nullable: true),
                    payroll_month = table.Column<int>(nullable: false),
                    employee_code = table.Column<string>(nullable: true),
                    employee_name = table.Column<string>(maxLength: 150, nullable: true),
                    father_or_husband_name = table.Column<string>(maxLength: 150, nullable: true),
                    designation = table.Column<string>(nullable: true),
                    basic_wage_payable = table.Column<string>(maxLength: 50, nullable: true),
                    da_wage_payable = table.Column<string>(maxLength: 50, nullable: true),
                    total_attand_orwork_done = table.Column<string>(maxLength: 50, nullable: true),
                    overtime_wages = table.Column<string>(maxLength: 50, nullable: true),
                    gross_wage_pay = table.Column<string>(maxLength: 50, nullable: true),
                    total_deduction = table.Column<string>(maxLength: 50, nullable: true),
                    net_wage_pay = table.Column<string>(maxLength: 50, nullable: true),
                    emp_sign_orthump_exp = table.Column<string>(nullable: true),
                    pay_incharge = table.Column<string>(maxLength: 50, nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_muster_form11_data", x => x.form_xi_id);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form11_data_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form11_data_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_muster_form2",
                columns: table => new
                {
                    form_II_mstr_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    damage_orloss_and_date_c_id = table.Column<int>(nullable: true),
                    whether_workman_c_id = table.Column<int>(nullable: true),
                    date_ofdeduc_c_id = table.Column<int>(nullable: true),
                    amt_ofdeduc_c_id = table.Column<int>(nullable: true),
                    no_of_installment_c_id = table.Column<int>(nullable: true),
                    date_realised_c_id = table.Column<int>(nullable: true),
                    remarks_c_id = table.Column<int>(nullable: true),
                    comp_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_muster_form2", x => x.form_II_mstr_id);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form2_tbl_component_master_amt_ofdeduc_c_id",
                        column: x => x.amt_ofdeduc_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form2_tbl_company_master_comp_id",
                        column: x => x.comp_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblmst2_tcpd_dmg",
                        column: x => x.damage_orloss_and_date_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form2_tbl_component_master_date_ofdeduc_c_id",
                        column: x => x.date_ofdeduc_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form2_tbl_component_master_date_realised_c_id",
                        column: x => x.date_realised_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form2_tbl_component_master_no_of_installment_c_id",
                        column: x => x.no_of_installment_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form2_tbl_component_master_remarks_c_id",
                        column: x => x.remarks_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form2_tbl_component_master_whether_workman_c_id",
                        column: x => x.whether_workman_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_muster_form2_data",
                columns: table => new
                {
                    form_ii_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: true),
                    company_id = table.Column<int>(nullable: true),
                    payroll_month = table.Column<int>(nullable: false),
                    employee_code = table.Column<string>(nullable: true),
                    employee_name = table.Column<string>(maxLength: 150, nullable: true),
                    father_or_husband_name = table.Column<string>(maxLength: 150, nullable: true),
                    department = table.Column<string>(nullable: true),
                    damage_orloss_and_date = table.Column<string>(maxLength: 100, nullable: true),
                    whether_workman = table.Column<string>(maxLength: 100, nullable: true),
                    date_of_deduc_imposed = table.Column<string>(maxLength: 50, nullable: true),
                    amt_of_deduc_imposed = table.Column<string>(maxLength: 50, nullable: true),
                    no_of_installment = table.Column<string>(maxLength: 10, nullable: true),
                    date_realised = table.Column<string>(maxLength: 10, nullable: true),
                    remarks = table.Column<string>(maxLength: 200, nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false),
                    gender = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_muster_form2_data", x => x.form_ii_id);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form2_data_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form2_data_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_muster_form3",
                columns: table => new
                {
                    form_III_mstr_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    advance_date_c_id = table.Column<int>(nullable: true),
                    advance_amt_c_id = table.Column<int>(nullable: true),
                    purpose_c_id = table.Column<int>(nullable: true),
                    no_of_installment_c_id = table.Column<int>(nullable: true),
                    postponement_granted_c_id = table.Column<int>(nullable: true),
                    date_total_repaid_c_id = table.Column<int>(nullable: true),
                    remarks_c_id = table.Column<int>(nullable: true),
                    comp_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_muster_form3", x => x.form_III_mstr_id);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form3_tbl_component_master_advance_amt_c_id",
                        column: x => x.advance_amt_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form3_tbl_component_master_advance_date_c_id",
                        column: x => x.advance_date_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form3_tbl_company_master_comp_id",
                        column: x => x.comp_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblmst3_tcpd_re",
                        column: x => x.date_total_repaid_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblmst3_tcpd_in",
                        column: x => x.no_of_installment_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblmst3_tcpd_pg",
                        column: x => x.postponement_granted_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form3_tbl_component_master_purpose_c_id",
                        column: x => x.purpose_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form3_tbl_component_master_remarks_c_id",
                        column: x => x.remarks_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_muster_form3_data",
                columns: table => new
                {
                    form_iii_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: true),
                    company_id = table.Column<int>(nullable: true),
                    payroll_month = table.Column<int>(nullable: false),
                    employee_code = table.Column<string>(nullable: true),
                    employee_name = table.Column<string>(maxLength: 150, nullable: true),
                    father_or_husband_name = table.Column<string>(maxLength: 150, nullable: true),
                    department = table.Column<string>(nullable: true),
                    advance_date = table.Column<string>(maxLength: 50, nullable: true),
                    advance_amount = table.Column<string>(maxLength: 100, nullable: true),
                    purpose = table.Column<string>(maxLength: 200, nullable: true),
                    no_of_installment = table.Column<string>(maxLength: 10, nullable: true),
                    postponse_granted = table.Column<string>(maxLength: 50, nullable: true),
                    date_of_repaid = table.Column<string>(maxLength: 50, nullable: true),
                    remarks = table.Column<string>(maxLength: 200, nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_muster_form3_data", x => x.form_iii_id);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form3_data_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form3_data_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_muster_form4",
                columns: table => new
                {
                    form_IV_mstr_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    overtime_date_c_id = table.Column<int>(nullable: true),
                    extent_overtime_c_id = table.Column<int>(nullable: true),
                    total_overtime_c_id = table.Column<int>(nullable: true),
                    normal_hr_c_id = table.Column<int>(nullable: true),
                    normal_rate_c_id = table.Column<int>(nullable: true),
                    overtime_rate_c_id = table.Column<int>(nullable: true),
                    normal_earning_c_id = table.Column<int>(nullable: true),
                    overtime_earning_c_id = table.Column<int>(nullable: true),
                    total_earning_c_id = table.Column<int>(nullable: true),
                    date_ofpayment_c_id = table.Column<int>(nullable: true),
                    dop_component_mstrcomponent_id = table.Column<int>(nullable: true),
                    comp_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_muster_form4", x => x.form_IV_mstr_id);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form4_tbl_company_master_comp_id",
                        column: x => x.comp_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblmst4_tcpd_dop",
                        column: x => x.dop_component_mstrcomponent_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form4_tbl_component_master_extent_overtime_c_id",
                        column: x => x.extent_overtime_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form4_tbl_component_master_normal_earning_c_id",
                        column: x => x.normal_earning_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form4_tbl_component_master_normal_hr_c_id",
                        column: x => x.normal_hr_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form4_tbl_component_master_normal_rate_c_id",
                        column: x => x.normal_rate_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form4_tbl_component_master_overtime_date_c_id",
                        column: x => x.overtime_date_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form4_tbl_component_master_overtime_earning_c_id",
                        column: x => x.overtime_earning_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form4_tbl_component_master_overtime_rate_c_id",
                        column: x => x.overtime_rate_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form4_tbl_component_master_total_earning_c_id",
                        column: x => x.total_earning_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form4_tbl_component_master_total_overtime_c_id",
                        column: x => x.total_overtime_c_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_muster_form4_data",
                columns: table => new
                {
                    form_iv_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: true),
                    company_id = table.Column<int>(nullable: true),
                    payroll_month = table.Column<int>(nullable: false),
                    employee_code = table.Column<string>(nullable: true),
                    employee_name = table.Column<string>(maxLength: 150, nullable: true),
                    father_or_husband_name = table.Column<string>(maxLength: 150, nullable: true),
                    sex = table.Column<string>(maxLength: 10, nullable: true),
                    designation = table.Column<string>(nullable: true),
                    department = table.Column<string>(nullable: true),
                    overtime_work_dt = table.Column<string>(maxLength: 50, nullable: true),
                    extent_overtime = table.Column<string>(maxLength: 50, nullable: true),
                    total_overtime_worked = table.Column<string>(maxLength: 50, nullable: true),
                    normal_hr = table.Column<string>(maxLength: 50, nullable: true),
                    normal_rate = table.Column<string>(maxLength: 50, nullable: true),
                    overtime_rate = table.Column<string>(maxLength: 50, nullable: true),
                    normal_earning = table.Column<string>(maxLength: 50, nullable: true),
                    overtime_earning = table.Column<string>(maxLength: 50, nullable: true),
                    total_earning = table.Column<string>(maxLength: 50, nullable: true),
                    date_ofpayment = table.Column<string>(maxLength: 50, nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_muster_form4_data", x => x.form_iv_id);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form4_data_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_muster_form4_data_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_No_dues_particular_master",
                columns: table => new
                {
                    pkid_ParticularMaster = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: false),
                    department_id = table.Column<int>(nullable: false),
                    particular_name = table.Column<string>(nullable: true),
                    remarks = table.Column<string>(nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_No_dues_particular_master", x => x.pkid_ParticularMaster);
                    table.ForeignKey(
                        name: "FK_tbl_No_dues_particular_master_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_No_dues_particular_master_tbl_department_master_departme~",
                        column: x => x.department_id,
                        principalTable: "tbl_department_master",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_No_dues_particular_responsible",
                columns: table => new
                {
                    pkid_ParticularResponsible = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: false),
                    company_id = table.Column<int>(nullable: false),
                    department_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_No_dues_particular_responsible", x => x.pkid_ParticularResponsible);
                    table.ForeignKey(
                        name: "FK_tbl_No_dues_particular_responsible_tbl_company_master_compan~",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_No_dues_particular_responsible_tbl_department_master_dep~",
                        column: x => x.department_id,
                        principalTable: "tbl_department_master",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_No_dues_particular_responsible_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_outdoor_request",
                columns: table => new
                {
                    leave_request_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    from_date = table.Column<DateTime>(nullable: false),
                    system_in_time = table.Column<DateTime>(nullable: false),
                    system_out_time = table.Column<DateTime>(nullable: false),
                    manual_in_time = table.Column<DateTime>(nullable: false),
                    manual_out_time = table.Column<DateTime>(nullable: false),
                    r_e_id = table.Column<int>(nullable: true),
                    a1_e_id = table.Column<int>(nullable: true),
                    a2_e_id = table.Column<int>(nullable: true),
                    a3_e_id = table.Column<int>(nullable: true),
                    requester_date = table.Column<DateTime>(nullable: false),
                    approval_date1 = table.Column<DateTime>(nullable: false),
                    approval_date2 = table.Column<DateTime>(nullable: false),
                    approval_date3 = table.Column<DateTime>(nullable: false),
                    is_approved1 = table.Column<byte>(nullable: false),
                    is_approved2 = table.Column<byte>(nullable: false),
                    is_approved3 = table.Column<byte>(nullable: false),
                    is_final_approve = table.Column<byte>(nullable: false),
                    requester_remarks = table.Column<string>(maxLength: 500, nullable: true),
                    approval1_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    approval2_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    approval3_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    deleted_by = table.Column<int>(nullable: false),
                    deleted_dt = table.Column<DateTime>(nullable: false),
                    deleted_remarks = table.Column<string>(maxLength: 200, nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    creted_dt = table.Column<DateTime>(nullable: false),
                    company_id = table.Column<int>(nullable: true),
                    admin_id = table.Column<int>(nullable: true),
                    is_admin_approve = table.Column<int>(nullable: false),
                    admin_remarks = table.Column<string>(nullable: true),
                    admin_ar_date = table.Column<DateTime>(nullable: false),
                    location = table.Column<string>(nullable: true),
                    longitude = table.Column<string>(nullable: true),
                    latitude = table.Column<string>(nullable: true),
                    is_auto = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_outdoor_request", x => x.leave_request_id);
                    table.ForeignKey(
                        name: "FK_tbl_outdoor_request_tbl_employee_master_a1_e_id",
                        column: x => x.a1_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_outdoor_request_tbl_employee_master_a2_e_id",
                        column: x => x.a2_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_outdoor_request_tbl_employee_master_a3_e_id",
                        column: x => x.a3_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_outdoor_request_tbl_employee_master_admin_id",
                        column: x => x.admin_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_outdoor_request_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_outdoor_request_tbl_employee_master_r_e_id",
                        column: x => x.r_e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_payroll_process_status",
                columns: table => new
                {
                    pay_pro_status_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    payroll_month_year = table.Column<int>(nullable: false),
                    emp_id = table.Column<int>(nullable: true),
                    is_calculated = table.Column<byte>(nullable: false),
                    is_freezed = table.Column<byte>(nullable: false),
                    is_lock = table.Column<byte>(nullable: false),
                    payroll_status = table.Column<byte>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    payslip_path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_payroll_process_status", x => x.pay_pro_status_id);
                    table.ForeignKey(
                        name: "FK_tbl_payroll_process_status_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_payroll_process_status_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_right_menu_link",
                columns: table => new
                {
                    menu_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    menu_name = table.Column<string>(maxLength: 50, nullable: true),
                    icon_url = table.Column<string>(maxLength: 20, nullable: true),
                    url = table.Column<string>(nullable: true),
                    sorting_order = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_right_menu_link", x => x.menu_id);
                    table.ForeignKey(
                        name: "FK_tbl_right_menu_link_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_salary_input",
                columns: table => new
                {
                    salary_input_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    monthyear = table.Column<int>(nullable: false),
                    emp_id = table.Column<int>(nullable: true),
                    component_id = table.Column<int>(nullable: true),
                    values = table.Column<string>(nullable: true),
                    rate = table.Column<double>(nullable: false),
                    current_month_value = table.Column<double>(nullable: false),
                    arrear_value = table.Column<double>(nullable: false),
                    component_type = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    is_fnf_comp = table.Column<int>(nullable: false),
                    company_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_salary_input", x => x.salary_input_id);
                    table.ForeignKey(
                        name: "FK_tbl_salary_input_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_salary_input_tbl_component_master_component_id",
                        column: x => x.component_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_salary_input_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_salary_input_change",
                columns: table => new
                {
                    salary_input_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    monthyear = table.Column<string>(nullable: true),
                    emp_id = table.Column<int>(nullable: true),
                    component_id = table.Column<int>(nullable: true),
                    values = table.Column<string>(nullable: true),
                    previousvalues = table.Column<string>(nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    is_fnf_comp = table.Column<int>(nullable: false),
                    company_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_salary_input_change", x => x.salary_input_id);
                    table.ForeignKey(
                        name: "FK_tbl_salary_input_change_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_salary_input_change_tbl_component_master_component_id",
                        column: x => x.component_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_salary_input_change_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_tab_master",
                columns: table => new
                {
                    tab_mstr_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    tab_name = table.Column<string>(maxLength: 500, nullable: true),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    for_rm1 = table.Column<byte>(nullable: false),
                    for_rm2 = table.Column<byte>(nullable: false),
                    for_rm3 = table.Column<byte>(nullable: false),
                    for_user = table.Column<byte>(nullable: false),
                    display_order = table.Column<int>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false),
                    deleted_by = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_tab_master", x => x.tab_mstr_id);
                    table.ForeignKey(
                        name: "FK_tbl_tab_master_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_working_role",
                columns: table => new
                {
                    working_role_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    working_role_name = table.Column<string>(nullable: true),
                    company_id = table.Column<int>(nullable: true),
                    dept_id = table.Column<int>(nullable: true),
                    is_default = table.Column<byte>(nullable: false),
                    is_active = table.Column<byte>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_working_role", x => x.working_role_id);
                    table.ForeignKey(
                        name: "FK_tbl_working_role_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_working_role_tbl_department_master_dept_id",
                        column: x => x.dept_id,
                        principalTable: "tbl_department_master",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_holiday_master_comp_list",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    location_id = table.Column<int>(nullable: true),
                    holiday_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_holiday_master_comp_list", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_holiday_master_comp_list_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_holiday_master_comp_list_tbl_holiday_master_holiday_id",
                        column: x => x.holiday_id,
                        principalTable: "tbl_holiday_master",
                        principalColumn: "holiday_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_holiday_master_comp_list_tbl_location_master_location_id",
                        column: x => x.location_id,
                        principalTable: "tbl_location_master",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_appcbl_on_company",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    l_a_id = table.Column<int>(nullable: true),
                    c_id = table.Column<int>(nullable: true),
                    location_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_appcbl_on_company", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_leave_appcbl_on_company_tbl_company_master_c_id",
                        column: x => x.c_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_appcbl_on_company_tbl_leave_applicablity_l_a_id",
                        column: x => x.l_a_id,
                        principalTable: "tbl_leave_applicablity",
                        principalColumn: "leave_applicablity_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_appcbl_on_company_tbl_location_master_location_id",
                        column: x => x.location_id,
                        principalTable: "tbl_location_master",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_location_master_log",
                columns: table => new
                {
                    location_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    location_id = table.Column<int>(nullable: true),
                    location_code = table.Column<string>(maxLength: 100, nullable: true),
                    location_name = table.Column<string>(maxLength: 200, nullable: true),
                    type_of_location = table.Column<byte>(nullable: false),
                    address_line_one = table.Column<string>(maxLength: 250, nullable: true),
                    address_line_two = table.Column<string>(maxLength: 250, nullable: true),
                    pin_code = table.Column<int>(nullable: false),
                    city_id = table.Column<int>(nullable: true),
                    state_id = table.Column<int>(nullable: true),
                    country_id = table.Column<int>(nullable: true),
                    primary_email_id = table.Column<string>(maxLength: 200, nullable: true),
                    secondary_email_id = table.Column<string>(maxLength: 200, nullable: true),
                    primary_contact_number = table.Column<string>(maxLength: 20, nullable: true),
                    secondary_contact_number = table.Column<string>(maxLength: 20, nullable: true),
                    website = table.Column<string>(nullable: true),
                    image = table.Column<string>(nullable: true),
                    company_id = table.Column<int>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_location_master_log", x => x.location_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_location_master_log_tbl_city_city_id",
                        column: x => x.city_id,
                        principalTable: "tbl_city",
                        principalColumn: "city_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_location_master_log_tbl_country_country_id",
                        column: x => x.country_id,
                        principalTable: "tbl_country",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_location_master_log_tbl_location_master_location_id",
                        column: x => x.location_id,
                        principalTable: "tbl_location_master",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_location_master_log_tbl_state_state_id",
                        column: x => x.state_id,
                        principalTable: "tbl_state",
                        principalColumn: "state_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_shift_location",
                columns: table => new
                {
                    shift_location_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    location_id = table.Column<int>(nullable: true),
                    shift_detail_id = table.Column<int>(nullable: true),
                    company_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_shift_location", x => x.shift_location_id);
                    table.ForeignKey(
                        name: "FK_tbl_shift_location_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_location_tbl_location_master_location_id",
                        column: x => x.location_id,
                        principalTable: "tbl_location_master",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_location_tbl_shift_details_shift_detail_id",
                        column: x => x.shift_detail_id,
                        principalTable: "tbl_shift_details",
                        principalColumn: "shift_details_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_shift_master_log",
                columns: table => new
                {
                    shift_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    shift_id = table.Column<int>(nullable: true),
                    location_id = table.Column<int>(nullable: true),
                    shift_name = table.Column<string>(maxLength: 50, nullable: true),
                    shift_short_name = table.Column<string>(maxLength: 20, nullable: true),
                    shift_code = table.Column<string>(maxLength: 100, nullable: true),
                    punch_in_time = table.Column<DateTime>(nullable: false),
                    punch_out_time = table.Column<DateTime>(nullable: false),
                    maximum_working_hours = table.Column<byte>(nullable: false),
                    maximum_working_minute = table.Column<byte>(nullable: false),
                    grace_time_for_late_punch_in = table.Column<DateTime>(nullable: false),
                    grace_time_for_late_punch_out = table.Column<DateTime>(nullable: false),
                    number_of_grace_time_applicable_in_month = table.Column<int>(nullable: false),
                    is_lunch_punch_applicable = table.Column<byte>(nullable: false),
                    lunch_punch_out_time = table.Column<DateTime>(nullable: false),
                    lunch_punch_in_time = table.Column<DateTime>(nullable: false),
                    maximum_lunch_time = table.Column<DateTime>(nullable: false),
                    is_ot_applicable = table.Column<byte>(nullable: false),
                    maximum_ot_hours = table.Column<DateTime>(nullable: false),
                    tea_punch_applicable_one = table.Column<int>(nullable: false),
                    tea_punch_out_time_one = table.Column<DateTime>(nullable: false),
                    tea_punch_in_time_one = table.Column<DateTime>(nullable: false),
                    maximum_tea_time_one = table.Column<DateTime>(nullable: false),
                    tea_punch_applicable_two = table.Column<int>(nullable: false),
                    tea_punch_out_time_two = table.Column<DateTime>(nullable: false),
                    tea_punch_in_time_two = table.Column<DateTime>(nullable: false),
                    maximum_tea_time_two = table.Column<DateTime>(nullable: false),
                    mark_as_half_day_for_working_hours_less_than = table.Column<DateTime>(nullable: false),
                    is_night_shift = table.Column<byte>(nullable: false),
                    weekly_off = table.Column<int>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_shift_master_log", x => x.shift_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_shift_master_log_tbl_location_master_location_id",
                        column: x => x.location_id,
                        principalTable: "tbl_location_master",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_shift_master_log_tbl_shift_master_shift_id",
                        column: x => x.shift_id,
                        principalTable: "tbl_shift_master",
                        principalColumn: "shift_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_sub_location_master",
                columns: table => new
                {
                    sub_location_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    location_name = table.Column<string>(maxLength: 200, nullable: true),
                    location_id = table.Column<int>(nullable: true),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_sub_location_master", x => x.sub_location_id);
                    table.ForeignKey(
                        name: "FK_tbl_sub_location_master_tbl_location_master_location_id",
                        column: x => x.location_id,
                        principalTable: "tbl_location_master",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_appcbl_on_dept_log",
                columns: table => new
                {
                    log_sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sno = table.Column<int>(nullable: true),
                    l_a_id = table.Column<int>(nullable: true),
                    d_id = table.Column<int>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_appcbl_on_dept_log", x => x.log_sno);
                    table.ForeignKey(
                        name: "FK_tbl_leave_appcbl_on_dept_log_tbl_department_master_d_id",
                        column: x => x.d_id,
                        principalTable: "tbl_department_master",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_appcbl_on_dept_log_tbl_leave_applicablity_l_a_id",
                        column: x => x.l_a_id,
                        principalTable: "tbl_leave_applicablity",
                        principalColumn: "leave_applicablity_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_appcbl_on_dept_log_tbl_leave_app_on_dept_sno",
                        column: x => x.sno,
                        principalTable: "tbl_leave_app_on_dept",
                        principalColumn: "sno",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_app_emp_type_log",
                columns: table => new
                {
                    log_sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sno = table.Column<int>(nullable: true),
                    lid = table.Column<int>(nullable: true),
                    employment_type = table.Column<byte>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_app_emp_type_log", x => x.log_sno);
                    table.ForeignKey(
                        name: "FK_tbl_leave_app_emp_type_log_tbl_leave_applicablity_lid",
                        column: x => x.lid,
                        principalTable: "tbl_leave_applicablity",
                        principalColumn: "leave_applicablity_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_app_emp_type_log_tbl_leave_app_on_emp_type_sno",
                        column: x => x.sno,
                        principalTable: "tbl_leave_app_on_emp_type",
                        principalColumn: "sno",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_appcbl_rel_log",
                columns: table => new
                {
                    log_sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sno = table.Column<int>(nullable: true),
                    lid = table.Column<int>(nullable: true),
                    r_id = table.Column<int>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_appcbl_rel_log", x => x.log_sno);
                    table.ForeignKey(
                        name: "FK_tbl_leave_appcbl_rel_log_tbl_leave_applicablity_lid",
                        column: x => x.lid,
                        principalTable: "tbl_leave_applicablity",
                        principalColumn: "leave_applicablity_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_appcbl_rel_log_tbl_religion_master_r_id",
                        column: x => x.r_id,
                        principalTable: "tbl_religion_master",
                        principalColumn: "religion_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_appcbl_rel_log_tbl_leave_appcbl_on_religion_sno",
                        column: x => x.sno,
                        principalTable: "tbl_leave_appcbl_on_religion",
                        principalColumn: "sno",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_assets_request_master",
                columns: table => new
                {
                    asset_req_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: false),
                    req_employee_id = table.Column<int>(nullable: false),
                    asset_name = table.Column<string>(maxLength: 200, nullable: true),
                    description = table.Column<string>(maxLength: 500, nullable: true),
                    assets_master_id = table.Column<int>(nullable: true),
                    asset_number = table.Column<string>(maxLength: 200, nullable: true),
                    from_date = table.Column<DateTime>(nullable: false),
                    to_date = table.Column<DateTime>(nullable: false),
                    asset_type = table.Column<byte>(nullable: false),
                    is_finalapprove = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    asset_issue_date = table.Column<DateTime>(nullable: false),
                    submission_date = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false),
                    is_closed = table.Column<byte>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_permanent = table.Column<byte>(nullable: false),
                    replace_dt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_assets_request_master", x => x.asset_req_id);
                    table.ForeignKey(
                        name: "FK_tbl_assets_request_master_tbl_assets_master_assets_master_id",
                        column: x => x.assets_master_id,
                        principalTable: "tbl_assets_master",
                        principalColumn: "asset_master_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_assets_request_master_tbl_employee_master_req_employee_id",
                        column: x => x.req_employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_documents",
                columns: table => new
                {
                    emp_doc_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    doc_type_id = table.Column<int>(nullable: false),
                    emp_id = table.Column<int>(nullable: false),
                    doc_no = table.Column<string>(maxLength: 50, nullable: true),
                    doc_path = table.Column<string>(nullable: true),
                    remarks = table.Column<string>(maxLength: 200, nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false),
                    company_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_documents", x => x.emp_doc_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_documents_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_emp_documents_tbl_document_type_master_doc_type_id",
                        column: x => x.doc_type_id,
                        principalTable: "tbl_document_type_master",
                        principalColumn: "doc_type_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_emp_documents_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_fnf_master",
                columns: table => new
                {
                    pkid_fnfMaster = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: true),
                    fkid_empSepration = table.Column<int>(nullable: true),
                    resign_dt = table.Column<DateTime>(nullable: false),
                    last_working_date = table.Column<DateTime>(nullable: false),
                    notice_recovery_days = table.Column<int>(nullable: false),
                    monthYear = table.Column<int>(nullable: false),
                    notice_payment_days = table.Column<int>(nullable: false),
                    net_amt = table.Column<decimal>(nullable: false),
                    settlment_amt = table.Column<decimal>(nullable: false),
                    settlment_dt = table.Column<DateTime>(nullable: false),
                    settlement_type = table.Column<int>(nullable: false),
                    is_freezed = table.Column<int>(nullable: false),
                    is_gratuity = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_fnf_master", x => x.pkid_fnfMaster);
                    table.ForeignKey(
                        name: "FK_tbl_fnf_master_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_fnf_master_tbl_emp_separation_fkid_empSepration",
                        column: x => x.fkid_empSepration,
                        principalTable: "tbl_emp_separation",
                        principalColumn: "sepration_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_kt_task_master",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_sepration_Id = table.Column<int>(nullable: true),
                    Task_Sno = table.Column<string>(nullable: true),
                    taskName = table.Column<string>(maxLength: 500, nullable: true),
                    Procedure = table.Column<string>(nullable: true),
                    remarks = table.Column<string>(maxLength: 500, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ModHandover = table.Column<string>(maxLength: 500, nullable: true),
                    HandoverDate = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_kt_task_master", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_kt_task_master_tbl_emp_separation_emp_sepration_Id",
                        column: x => x.emp_sepration_Id,
                        principalTable: "tbl_emp_separation",
                        principalColumn: "sepration_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_No_dues_clearance_Department",
                columns: table => new
                {
                    pkid_ClearanceDepartment = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fkid_EmpSaperationId = table.Column<int>(nullable: false),
                    department_id = table.Column<int>(nullable: false),
                    is_Cleared = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_No_dues_clearance_Department", x => x.pkid_ClearanceDepartment);
                    table.ForeignKey(
                        name: "FK_tbl_No_dues_clearance_Department_tbl_department_master_depar~",
                        column: x => x.department_id,
                        principalTable: "tbl_department_master",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_No_dues_clearance_Department_tbl_emp_separation_fkid_Emp~",
                        column: x => x.fkid_EmpSaperationId,
                        principalTable: "tbl_emp_separation",
                        principalColumn: "sepration_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_employee_company_map_log",
                columns: table => new
                {
                    sno_log = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    mid = table.Column<int>(nullable: true),
                    eid = table.Column<int>(nullable: true),
                    cid = table.Column<int>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_employee_company_map_log", x => x.sno_log);
                    table.ForeignKey(
                        name: "FK_tbl_employee_company_map_log_tbl_company_master_cid",
                        column: x => x.cid,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_employee_company_map_log_tbl_employee_master_eid",
                        column: x => x.eid,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_employee_company_map_log_tbl_employee_company_map_mid",
                        column: x => x.mid,
                        principalTable: "tbl_employee_company_map",
                        principalColumn: "sno",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_status_flow_master",
                columns: table => new
                {
                    flow_mster_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    start_status_id = table.Column<int>(nullable: false),
                    next_status_id = table.Column<int>(nullable: false),
                    description = table.Column<string>(maxLength: 500, nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    deleted_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_status_flow_master", x => x.flow_mster_id);
                    table.ForeignKey(
                        name: "FK_tbl_status_flow_master_tbl_epa_status_master_next_status_id",
                        column: x => x.next_status_id,
                        principalTable: "tbl_epa_status_master",
                        principalColumn: "epa_status_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_status_flow_master_tbl_epa_status_master_start_status_id",
                        column: x => x.start_status_id,
                        principalTable: "tbl_epa_status_master",
                        principalColumn: "epa_status_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_holyd_emp_list_log",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    h_id = table.Column<int>(nullable: true),
                    employee_id = table.Column<int>(nullable: true),
                    holiday_id = table.Column<int>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_holyd_emp_list_log", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_holyd_emp_list_log_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_holyd_emp_list_log_tbl_holiday_master_emp_list_h_id",
                        column: x => x.h_id,
                        principalTable: "tbl_holiday_master_emp_list",
                        principalColumn: "sno",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_holyd_emp_list_log_tbl_holiday_master_holiday_id",
                        column: x => x.holiday_id,
                        principalTable: "tbl_holiday_master",
                        principalColumn: "holiday_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_No_dues_emp_particular_Clearence_detail",
                columns: table => new
                {
                    pkid_EmpParticularClearance = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    fkid_EmpSaperationId = table.Column<int>(nullable: false),
                    fkid_department_id = table.Column<int>(nullable: false),
                    fkid_ParticularId = table.Column<int>(nullable: false),
                    is_Outstanding = table.Column<int>(nullable: false),
                    remarks = table.Column<string>(nullable: true),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_No_dues_emp_particular_Clearence_detail", x => x.pkid_EmpParticularClearance);
                    table.ForeignKey(
                        name: "FK_tbl_No_dues_emp_particular_Clearence_detail_tbl_emp_separati~",
                        column: x => x.fkid_EmpSaperationId,
                        principalTable: "tbl_emp_separation",
                        principalColumn: "sepration_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_No_dues_emp_particular_Clearence_detail_tbl_No_dues_part~",
                        column: x => x.fkid_ParticularId,
                        principalTable: "tbl_No_dues_particular_master",
                        principalColumn: "pkid_ParticularMaster",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_No_dues_emp_particular_Clearence_detail_tbl_department_m~",
                        column: x => x.fkid_department_id,
                        principalTable: "tbl_department_master",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_working_role_allocation",
                columns: table => new
                {
                    emp_working_role_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    work_r_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_working_role_allocation", x => x.emp_working_role_id);
                    table.ForeignKey(
                        name: "FK_tewp_tem_emp_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_working_role_allocation_tbl_working_role_work_r_id",
                        column: x => x.work_r_id,
                        principalTable: "tbl_working_role",
                        principalColumn: "working_role_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_kpi_key_area_master",
                columns: table => new
                {
                    key_area_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    w_r_id = table.Column<int>(nullable: true),
                    otype_id = table.Column<int>(nullable: true),
                    key_area = table.Column<string>(nullable: true),
                    expected_result = table.Column<string>(maxLength: 250, nullable: true),
                    wtg = table.Column<int>(nullable: false),
                    comment = table.Column<string>(maxLength: 1000, nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    deleted_by = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_kpi_key_area_master", x => x.key_area_id);
                    table.ForeignKey(
                        name: "FK_tbl_kpi_key_area_master_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_kpi_key_area_master_tbl_kpi_objective_type_otype_id",
                        column: x => x.otype_id,
                        principalTable: "tbl_kpi_objective_type",
                        principalColumn: "obj_type_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_kpi_key_area_master_tbl_working_role_w_r_id",
                        column: x => x.w_r_id,
                        principalTable: "tbl_working_role",
                        principalColumn: "working_role_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_kra_master",
                columns: table => new
                {
                    kra_mstr_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    department_id = table.Column<int>(nullable: true),
                    wrk_role_id = table.Column<int>(nullable: true),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    factor_result = table.Column<string>(maxLength: 1000, nullable: true),
                    display_order = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    deleted_by = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    creatd_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_kra_master", x => x.kra_mstr_id);
                    table.ForeignKey(
                        name: "FK_tbl_kra_master_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_kra_master_tbl_department_master_department_id",
                        column: x => x.department_id,
                        principalTable: "tbl_department_master",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_kra_master_tbl_working_role_wrk_role_id",
                        column: x => x.wrk_role_id,
                        principalTable: "tbl_working_role",
                        principalColumn: "working_role_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_question_master",
                columns: table => new
                {
                    ques_mstr_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    wrk_role_id = table.Column<int>(nullable: true),
                    tab_id = table.Column<int>(nullable: true),
                    ques = table.Column<string>(maxLength: 1000, nullable: true),
                    ans_type = table.Column<int>(nullable: false),
                    ans_type_ddl = table.Column<string>(maxLength: 300, nullable: true),
                    description = table.Column<string>(maxLength: 1000, nullable: true),
                    deleted_by = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false),
                    ques_display_order = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_question_master", x => x.ques_mstr_id);
                    table.ForeignKey(
                        name: "FK_tbl_question_master_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_question_master_tbl_tab_master_tab_id",
                        column: x => x.tab_id,
                        principalTable: "tbl_tab_master",
                        principalColumn: "tab_mstr_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_question_master_tbl_working_role_wrk_role_id",
                        column: x => x.wrk_role_id,
                        principalTable: "tbl_working_role",
                        principalColumn: "working_role_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_holi_comp_list_log",
                columns: table => new
                {
                    sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    lid = table.Column<int>(nullable: true),
                    cid = table.Column<int>(nullable: true),
                    l_id = table.Column<int>(nullable: true),
                    h_id = table.Column<int>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_holi_comp_list_log", x => x.sno);
                    table.ForeignKey(
                        name: "FK_tbl_holi_comp_list_log_tbl_company_master_cid",
                        column: x => x.cid,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_holi_comp_list_log_tbl_holiday_master_h_id",
                        column: x => x.h_id,
                        principalTable: "tbl_holiday_master",
                        principalColumn: "holiday_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_holi_comp_list_log_tbl_location_master_l_id",
                        column: x => x.l_id,
                        principalTable: "tbl_location_master",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_holi_comp_list_log_tbl_holiday_master_comp_list_lid",
                        column: x => x.lid,
                        principalTable: "tbl_holiday_master_comp_list",
                        principalColumn: "sno",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_leave_appcbly_on_comp_log",
                columns: table => new
                {
                    log_sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sno = table.Column<int>(nullable: true),
                    l_a_id = table.Column<int>(nullable: true),
                    c_id = table.Column<int>(nullable: true),
                    l_id = table.Column<int>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_leave_appcbly_on_comp_log", x => x.log_sno);
                    table.ForeignKey(
                        name: "FK_tbl_leave_appcbly_on_comp_log_tbl_company_master_c_id",
                        column: x => x.c_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_appcbly_on_comp_log_tbl_leave_applicablity_l_a_id",
                        column: x => x.l_a_id,
                        principalTable: "tbl_leave_applicablity",
                        principalColumn: "leave_applicablity_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_appcbly_on_comp_log_tbl_location_master_l_id",
                        column: x => x.l_id,
                        principalTable: "tbl_location_master",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_leave_appcbly_on_comp_log_tbl_leave_appcbl_on_company_sno",
                        column: x => x.sno,
                        principalTable: "tbl_leave_appcbl_on_company",
                        principalColumn: "sno",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_officaial_sec",
                columns: table => new
                {
                    emp_official_section_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    is_applicable_for_all_comp = table.Column<byte>(nullable: false),
                    employee_id = table.Column<int>(nullable: true),
                    state_id = table.Column<int>(nullable: true),
                    location_id = table.Column<int>(nullable: true),
                    sub_location_id = table.Column<int>(nullable: true),
                    department_id = table.Column<int>(nullable: true),
                    sub_dept_id = table.Column<int>(nullable: true),
                    card_number = table.Column<string>(maxLength: 50, nullable: true),
                    gender = table.Column<int>(nullable: false),
                    salutation = table.Column<string>(maxLength: 1, nullable: true),
                    employee_first_name = table.Column<string>(maxLength: 30, nullable: true),
                    employee_middle_name = table.Column<string>(maxLength: 30, nullable: true),
                    employee_last_name = table.Column<string>(maxLength: 30, nullable: true),
                    emp_father_name = table.Column<string>(nullable: true),
                    nationality = table.Column<string>(nullable: true),
                    group_joining_date = table.Column<DateTime>(nullable: false),
                    date_of_joining = table.Column<DateTime>(nullable: false),
                    department_date_of_joining = table.Column<DateTime>(nullable: false),
                    date_of_birth = table.Column<DateTime>(nullable: false),
                    religion_id = table.Column<int>(nullable: true),
                    marital_status = table.Column<byte>(nullable: false),
                    hr_spoc = table.Column<string>(maxLength: 100, nullable: true),
                    official_email_id = table.Column<string>(maxLength: 100, nullable: true),
                    empmnt__id = table.Column<int>(nullable: true),
                    is_ot_allowed = table.Column<byte>(nullable: false),
                    is_comb_off_allowed = table.Column<int>(nullable: false),
                    is_mobile_access = table.Column<int>(nullable: false),
                    is_mobile_attendence_access = table.Column<int>(nullable: false),
                    mobile_punch_from_date = table.Column<DateTime>(nullable: false),
                    mobile_punch_to_date = table.Column<DateTime>(nullable: false),
                    punch_type = table.Column<byte>(nullable: false),
                    employee_photo_path = table.Column<string>(nullable: true),
                    last_working_date = table.Column<DateTime>(nullable: false),
                    is_fixed_weekly_off = table.Column<int>(nullable: false),
                    current_employee_type = table.Column<byte>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    remarks = table.Column<string>(maxLength: 200, nullable: true),
                    user_type = table.Column<int>(nullable: false),
                    notice_period = table.Column<string>(nullable: true),
                    employement_type = table.Column<string>(nullable: true),
                    confirmation_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_officaial_sec", x => x.emp_official_section_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_officaial_sec_tbl_department_master_department_id",
                        column: x => x.department_id,
                        principalTable: "tbl_department_master",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_officaial_sec_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_officaial_sec_tbl_employment_type_master_empmnt__id",
                        column: x => x.empmnt__id,
                        principalTable: "tbl_employment_type_master",
                        principalColumn: "employment_type_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_officaial_sec_tbl_location_master_location_id",
                        column: x => x.location_id,
                        principalTable: "tbl_location_master",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_officaial_sec_tbl_religion_master_religion_id",
                        column: x => x.religion_id,
                        principalTable: "tbl_religion_master",
                        principalColumn: "religion_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_officaial_sec_tbl_state_state_id",
                        column: x => x.state_id,
                        principalTable: "tbl_state",
                        principalColumn: "state_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_officaial_sec_tbl_sub_department_master_sub_dept_id",
                        column: x => x.sub_dept_id,
                        principalTable: "tbl_sub_department_master",
                        principalColumn: "sub_department_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_officaial_sec_tbl_sub_location_master_sub_location_id",
                        column: x => x.sub_location_id,
                        principalTable: "tbl_sub_location_master",
                        principalColumn: "sub_location_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_machine_master",
                columns: table => new
                {
                    machine_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    machine_description = table.Column<string>(nullable: true),
                    machine_number = table.Column<int>(nullable: false),
                    port_number = table.Column<int>(nullable: false),
                    machine_type = table.Column<byte>(nullable: false),
                    ip_address = table.Column<string>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    company_id = table.Column<int>(nullable: true),
                    location_id = table.Column<int>(nullable: true),
                    sub_location_id = table.Column<int>(nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_machine_master", x => x.machine_id);
                    table.ForeignKey(
                        name: "FK_tbl_machine_master_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_machine_master_tbl_location_master_location_id",
                        column: x => x.location_id,
                        principalTable: "tbl_location_master",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_machine_master_tbl_sub_location_master_sub_location_id",
                        column: x => x.sub_location_id,
                        principalTable: "tbl_sub_location_master",
                        principalColumn: "sub_location_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_assets_approval",
                columns: table => new
                {
                    asset_approval_sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    asset_req_id = table.Column<int>(nullable: true),
                    emp_id = table.Column<int>(nullable: true),
                    is_final_approver = table.Column<byte>(nullable: false),
                    is_approve = table.Column<byte>(nullable: false),
                    approval_order = table.Column<byte>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    approver_role_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_assets_approval", x => x.asset_approval_sno);
                    table.ForeignKey(
                        name: "FK_tbl_assets_approval_tbl_assets_request_master_asset_req_id",
                        column: x => x.asset_req_id,
                        principalTable: "tbl_assets_request_master",
                        principalColumn: "asset_req_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_assets_approval_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_fnf_asset",
                columns: table => new
                {
                    fnf_asset_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    asset_name = table.Column<string>(nullable: true),
                    asset_number = table.Column<string>(nullable: true),
                    recovery_amt = table.Column<double>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_process = table.Column<int>(nullable: false),
                    x_id = table.Column<int>(nullable: false),
                    fnf_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_fnf_asset", x => x.fnf_asset_id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_fnf_asset_tbl_fnf_master_fnf_id",
                        column: x => x.fnf_id,
                        principalTable: "tbl_fnf_master",
                        principalColumn: "pkid_fnfMaster",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_fnf_attendance_dtl",
                columns: table => new
                {
                    fnf_attend_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    monthyear = table.Column<int>(nullable: false),
                    totaldays = table.Column<int>(nullable: false),
                    acutual_lop_days = table.Column<double>(nullable: false),
                    final_lop_days = table.Column<double>(nullable: false),
                    Week_off_days = table.Column<double>(nullable: false),
                    Holiday_days = table.Column<double>(nullable: false),
                    Present_days = table.Column<double>(nullable: false),
                    Absent_days = table.Column<double>(nullable: false),
                    Leave_days = table.Column<double>(nullable: false),
                    Total_Paid_days = table.Column<double>(nullable: false),
                    paid_amount = table.Column<double>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_process = table.Column<int>(nullable: false),
                    fnf_id = table.Column<int>(nullable: true),
                    x_id = table.Column<int>(nullable: false),
                    emp_sep_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_fnf_attendance_dtl", x => x.fnf_attend_id);
                    table.ForeignKey(
                        name: "FK_tbl_fnf_attendance_dtl_tbl_fnf_master_fnf_id",
                        column: x => x.fnf_id,
                        principalTable: "tbl_fnf_master",
                        principalColumn: "pkid_fnfMaster",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_fnf_component",
                columns: table => new
                {
                    variable_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    component_id = table.Column<int>(nullable: true),
                    monthyear = table.Column<int>(nullable: false),
                    component_type = table.Column<int>(nullable: false),
                    variable_amt = table.Column<double>(nullable: false),
                    remarks = table.Column<string>(maxLength: 200, nullable: true),
                    fnf_id = table.Column<int>(nullable: true),
                    x_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_process = table.Column<int>(nullable: false),
                    emp_sep_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_fnf_component", x => x.variable_id);
                    table.ForeignKey(
                        name: "FK_tbl_fnf_component_tbl_component_master_component_id",
                        column: x => x.component_id,
                        principalTable: "tbl_component_master",
                        principalColumn: "component_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_fnf_component_tbl_fnf_master_fnf_id",
                        column: x => x.fnf_id,
                        principalTable: "tbl_fnf_master",
                        principalColumn: "pkid_fnfMaster",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_fnf_leave_encash",
                columns: table => new
                {
                    leave_encash_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    leave_type_id = table.Column<int>(nullable: true),
                    leave_balance = table.Column<double>(nullable: false),
                    leave_encash_day = table.Column<double>(nullable: false),
                    leave_encash_cal = table.Column<decimal>(nullable: false),
                    leave_encash_final = table.Column<decimal>(nullable: false),
                    fnf_id = table.Column<int>(nullable: true),
                    x_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_dt = table.Column<DateTime>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_dt = table.Column<DateTime>(nullable: false),
                    is_process = table.Column<int>(nullable: false),
                    emp_sep_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_fnf_leave_encash", x => x.leave_encash_id);
                    table.ForeignKey(
                        name: "FK_tbl_fnf_leave_encash_tbl_fnf_master_fnf_id",
                        column: x => x.fnf_id,
                        principalTable: "tbl_fnf_master",
                        principalColumn: "pkid_fnfMaster",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_fnf_leave_encash_tbl_leave_type_leave_type_id",
                        column: x => x.leave_type_id,
                        principalTable: "tbl_leave_type",
                        principalColumn: "leave_type_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_fnf_reimburesment",
                columns: table => new
                {
                    fnf_reim_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    reim_name = table.Column<string>(maxLength: 200, nullable: true),
                    reim_amt = table.Column<decimal>(nullable: false),
                    reim_bal = table.Column<decimal>(nullable: false),
                    fnf_id = table.Column<int>(nullable: true),
                    x_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_fnf_reimburesment", x => x.fnf_reim_id);
                    table.ForeignKey(
                        name: "FK_tbl_fnf_reimburesment_tbl_fnf_master_fnf_id",
                        column: x => x.fnf_id,
                        principalTable: "tbl_fnf_master",
                        principalColumn: "pkid_fnfMaster",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_kt_status",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Kt_Master_id = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_kt_status", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_kt_status_tbl_kt_task_master_Kt_Master_id",
                        column: x => x.Kt_Master_id,
                        principalTable: "tbl_kt_task_master",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_kt_task_emp_details",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Kt_Master_id = table.Column<int>(nullable: true),
                    EmpId = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    modifed_on = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_kt_task_emp_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_kt_task_emp_details_tbl_kt_task_master_Kt_Master_id",
                        column: x => x.Kt_Master_id,
                        principalTable: "tbl_kt_task_master",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_kpi_criteria_master",
                columns: table => new
                {
                    crit_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    k_a_id = table.Column<int>(nullable: true),
                    criteria_number = table.Column<int>(nullable: false),
                    criteria = table.Column<string>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_kpi_criteria_master", x => x.crit_id);
                    table.ForeignKey(
                        name: "FK_tbl_kpi_criteria_master_tbl_kpi_key_area_master_k_a_id",
                        column: x => x.k_a_id,
                        principalTable: "tbl_kpi_key_area_master",
                        principalColumn: "key_area_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_daily_attendance",
                columns: table => new
                {
                    emp_id = table.Column<int>(nullable: false),
                    attendance_dt = table.Column<DateTime>(nullable: false),
                    payrollmonthyear = table.Column<int>(nullable: false),
                    last_process_dt = table.Column<DateTime>(nullable: false),
                    emp_offcl_id = table.Column<int>(nullable: true),
                    shift_id = table.Column<int>(nullable: true),
                    s_d_id = table.Column<int>(nullable: true),
                    in_time = table.Column<DateTime>(nullable: false),
                    shift_in_time = table.Column<DateTime>(nullable: false),
                    shift_max_in_time = table.Column<DateTime>(nullable: false),
                    out_time = table.Column<DateTime>(nullable: false),
                    shift_out_time = table.Column<DateTime>(nullable: false),
                    day_status = table.Column<byte>(nullable: false),
                    leave_type = table.Column<byte>(nullable: false),
                    is_weekly_off = table.Column<byte>(nullable: false),
                    is_holiday = table.Column<byte>(nullable: false),
                    is_comp_off = table.Column<byte>(nullable: false),
                    is_outdoor = table.Column<byte>(nullable: false),
                    is_regularize = table.Column<byte>(nullable: false),
                    is_grace_applied = table.Column<byte>(nullable: false),
                    grace_period_hour = table.Column<int>(nullable: false),
                    grace_period_minute = table.Column<int>(nullable: false),
                    short_leave_hour = table.Column<int>(nullable: false),
                    short_leave_minute = table.Column<int>(nullable: false),
                    is_late_in = table.Column<byte>(nullable: false),
                    early_or_late_in_hour = table.Column<int>(nullable: false),
                    early_or_late_in_minute = table.Column<int>(nullable: false),
                    is_early_out = table.Column<byte>(nullable: false),
                    early_or_late_out_hour = table.Column<int>(nullable: false),
                    early_or_late_out_minute = table.Column<int>(nullable: false),
                    working_hour_done = table.Column<int>(nullable: false),
                    working_minute_done = table.Column<int>(nullable: false),
                    w_hour_req_for_full_day = table.Column<int>(nullable: false),
                    w_min_req_for_full_day = table.Column<int>(nullable: false),
                    w_hour_req_for_half_day = table.Column<int>(nullable: false),
                    w_min_req_for_half_day = table.Column<int>(nullable: false),
                    is_ot_given = table.Column<byte>(nullable: false),
                    ot_hour_done = table.Column<int>(nullable: false),
                    ot_minute_done = table.Column<int>(nullable: false),
                    is_freezed = table.Column<byte>(nullable: false),
                    holiday_id = table.Column<int>(nullable: true),
                    leave_request_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_daily_attendance", x => new { x.emp_id, x.attendance_dt });
                    table.UniqueConstraint("AK_tbl_daily_attendance_attendance_dt_emp_id", x => new { x.attendance_dt, x.emp_id });
                    table.ForeignKey(
                        name: "FK_tbl_daily_attendance_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_daily_attendance_tbl_emp_officaial_sec_emp_offcl_id",
                        column: x => x.emp_offcl_id,
                        principalTable: "tbl_emp_officaial_sec",
                        principalColumn: "emp_official_section_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_daily_attendance_tbl_holiday_master_holiday_id",
                        column: x => x.holiday_id,
                        principalTable: "tbl_holiday_master",
                        principalColumn: "holiday_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_daily_attendance_tbl_leave_request_leave_request_id",
                        column: x => x.leave_request_id,
                        principalTable: "tbl_leave_request",
                        principalColumn: "leave_request_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_daily_attendance_tbl_shift_details_s_d_id",
                        column: x => x.s_d_id,
                        principalTable: "tbl_shift_details",
                        principalColumn: "shift_details_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_daily_attendance_tbl_shift_master_shift_id",
                        column: x => x.shift_id,
                        principalTable: "tbl_shift_master",
                        principalColumn: "shift_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_epa_submission",
                columns: table => new
                {
                    submission_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: true),
                    emp_id = table.Column<int>(nullable: true),
                    emp_off_id = table.Column<int>(nullable: true),
                    fiscal_year_id = table.Column<int>(nullable: true),
                    cycle_id = table.Column<int>(nullable: false),
                    cycle_name = table.Column<string>(nullable: true),
                    desig_id = table.Column<int>(nullable: true),
                    department_id = table.Column<int>(nullable: true),
                    epa_start_date = table.Column<DateTime>(nullable: false),
                    epa_end_date = table.Column<DateTime>(nullable: false),
                    epa_close_status = table.Column<byte>(nullable: false),
                    epa_current_status = table.Column<int>(nullable: true),
                    w_r_id = table.Column<int>(nullable: true),
                    total_score = table.Column<int>(nullable: false),
                    get_score = table.Column<int>(nullable: false),
                    final_review = table.Column<byte>(nullable: false),
                    final_remarks = table.Column<string>(nullable: true),
                    final_review_rm1 = table.Column<byte>(nullable: false),
                    final_remarks_rm1 = table.Column<string>(nullable: true),
                    final_review_rm2 = table.Column<byte>(nullable: false),
                    final_remarks_rm2 = table.Column<string>(nullable: true),
                    final_review_rm3 = table.Column<byte>(nullable: false),
                    final_remarks_rm3 = table.Column<string>(nullable: true),
                    rm_id1 = table.Column<int>(nullable: true),
                    rm_id2 = table.Column<int>(nullable: true),
                    rm_id3 = table.Column<int>(nullable: true),
                    is_deleted = table.Column<byte>(nullable: false),
                    user_id = table.Column<int>(nullable: false),
                    submission_dt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_epa_submission", x => x.submission_id);
                    table.ForeignKey(
                        name: "FK_tbl_epa_submission_tbl_company_master_company_id",
                        column: x => x.company_id,
                        principalTable: "tbl_company_master",
                        principalColumn: "company_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_epa_submission_tbl_department_master_department_id",
                        column: x => x.department_id,
                        principalTable: "tbl_department_master",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_epa_submission_tbl_designation_master_desig_id",
                        column: x => x.desig_id,
                        principalTable: "tbl_designation_master",
                        principalColumn: "designation_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_epa_submission_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_epa_submission_tbl_emp_officaial_sec_emp_off_id",
                        column: x => x.emp_off_id,
                        principalTable: "tbl_emp_officaial_sec",
                        principalColumn: "emp_official_section_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_epa_submission_tbl_epa_status_master_epa_current_status",
                        column: x => x.epa_current_status,
                        principalTable: "tbl_epa_status_master",
                        principalColumn: "epa_status_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_epa_submission_tbl_epa_fiscal_yr_mstr_fiscal_year_id",
                        column: x => x.fiscal_year_id,
                        principalTable: "tbl_epa_fiscal_yr_mstr",
                        principalColumn: "fiscal_year_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_epa_submission_tbl_working_role_w_r_id",
                        column: x => x.w_r_id,
                        principalTable: "tbl_working_role",
                        principalColumn: "working_role_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_attendance_details",
                columns: table => new
                {
                    attendance_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    machine_id = table.Column<int>(nullable: true),
                    emp_id = table.Column<int>(nullable: true),
                    punch_time = table.Column<DateTime>(nullable: false),
                    entry_time = table.Column<DateTime>(nullable: false),
                    enter_by_interface = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_attendance_details", x => x.attendance_id);
                    table.ForeignKey(
                        name: "FK_tbl_attendance_details_tbl_employee_master_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_attendance_details_tbl_machine_master_machine_id",
                        column: x => x.machine_id,
                        principalTable: "tbl_machine_master",
                        principalColumn: "machine_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_machine_master_log",
                columns: table => new
                {
                    machine_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    machine_id = table.Column<int>(nullable: true),
                    machine_description = table.Column<string>(nullable: true),
                    machine_number = table.Column<int>(nullable: false),
                    port_number = table.Column<int>(nullable: false),
                    ip_address = table.Column<string>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_machine_master_log", x => x.machine_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_machine_master_log_tbl_machine_master_machine_id",
                        column: x => x.machine_id,
                        principalTable: "tbl_machine_master",
                        principalColumn: "machine_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_epa_kpi_submission",
                columns: table => new
                {
                    kpi_submission_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    submission_id = table.Column<int>(nullable: true),
                    key_area_id = table.Column<int>(nullable: true),
                    self = table.Column<int>(nullable: false),
                    aggreed = table.Column<int>(nullable: false),
                    score = table.Column<int>(nullable: false),
                    comment = table.Column<string>(nullable: true),
                    change_dt = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_epa_kpi_submission", x => x.kpi_submission_id);
                    table.ForeignKey(
                        name: "FK_tbl_epa_kpi_submission_tbl_kpi_key_area_master_key_area_id",
                        column: x => x.key_area_id,
                        principalTable: "tbl_kpi_key_area_master",
                        principalColumn: "key_area_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_epa_kpi_submission_tbl_epa_submission_submission_id",
                        column: x => x.submission_id,
                        principalTable: "tbl_epa_submission",
                        principalColumn: "submission_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_epa_kra_submission",
                columns: table => new
                {
                    kra_submission_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    submission_id = table.Column<int>(nullable: true),
                    kra_id = table.Column<int>(nullable: true),
                    rating_id = table.Column<int>(nullable: true),
                    change_dt = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_epa_kra_submission", x => x.kra_submission_id);
                    table.ForeignKey(
                        name: "FK_tbl_epa_kra_submission_tbl_kra_master_kra_id",
                        column: x => x.kra_id,
                        principalTable: "tbl_kra_master",
                        principalColumn: "kra_mstr_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_epa_kra_submission_tbl_kpi_rating_master_rating_id",
                        column: x => x.rating_id,
                        principalTable: "tbl_kpi_rating_master",
                        principalColumn: "kpi_rating_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_epa_kra_submission_tbl_epa_submission_submission_id",
                        column: x => x.submission_id,
                        principalTable: "tbl_epa_submission",
                        principalColumn: "submission_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_epa_question_submission",
                columns: table => new
                {
                    question_submission_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    submission_id = table.Column<int>(nullable: true),
                    tab_id = table.Column<int>(nullable: true),
                    question_id = table.Column<int>(nullable: true),
                    question_answer = table.Column<string>(nullable: true),
                    remarks = table.Column<string>(nullable: true),
                    change_dt = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_epa_question_submission", x => x.question_submission_id);
                    table.ForeignKey(
                        name: "FK_tbl_epa_question_submission_tbl_question_master_question_id",
                        column: x => x.question_id,
                        principalTable: "tbl_question_master",
                        principalColumn: "ques_mstr_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_epa_question_submission_tbl_epa_submission_submission_id",
                        column: x => x.submission_id,
                        principalTable: "tbl_epa_submission",
                        principalColumn: "submission_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_epa_question_submission_tbl_tab_master_tab_id",
                        column: x => x.tab_id,
                        principalTable: "tbl_tab_master",
                        principalColumn: "tab_mstr_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_attendance_details_log",
                columns: table => new
                {
                    attendance_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    a_id = table.Column<int>(nullable: true),
                    machine_id = table.Column<int>(nullable: true),
                    e_id = table.Column<int>(nullable: true),
                    punch_time = table.Column<DateTime>(nullable: false),
                    entry_time = table.Column<DateTime>(nullable: false),
                    enter_by_interface = table.Column<byte>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_attendance_details_log", x => x.attendance_log_id);
                    table.ForeignKey(
                        name: "FK_tbl_attendance_details_log_tbl_attendance_details_a_id",
                        column: x => x.a_id,
                        principalTable: "tbl_attendance_details",
                        principalColumn: "attendance_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_attendance_details_log_tbl_employee_master_e_id",
                        column: x => x.e_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_attendance_details_log_tbl_machine_master_machine_id",
                        column: x => x.machine_id,
                        principalTable: "tbl_machine_master",
                        principalColumn: "machine_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_epa_kpi_criteria_submission",
                columns: table => new
                {
                    kpi_criteria_submission_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    crit_id = table.Column<int>(nullable: true),
                    kpi_submission_id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_epa_kpi_criteria_submission", x => x.kpi_criteria_submission_id);
                    table.ForeignKey(
                        name: "FK_tblepa_critid",
                        column: x => x.crit_id,
                        principalTable: "tbl_kpi_criteria_master",
                        principalColumn: "crit_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblepa_submissionid",
                        column: x => x.kpi_submission_id,
                        principalTable: "tbl_epa_kpi_submission",
                        principalColumn: "kpi_submission_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_log_tbl_salary_input_component_id",
                table: "log_tbl_salary_input",
                column: "component_id");

            migrationBuilder.CreateIndex(
                name: "IX_log_tbl_salary_input_emp_id",
                table: "log_tbl_salary_input",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_active_inactive_user_log_user_id",
                table: "tbl_active_inactive_user_log",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_approved_emp_separation_cancellation_admin_id",
                table: "tbl_approved_emp_separation_cancellation",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_approved_emp_separation_cancellation_approver1_id",
                table: "tbl_approved_emp_separation_cancellation",
                column: "approver1_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_approved_emp_separation_cancellation_approver2_id",
                table: "tbl_approved_emp_separation_cancellation",
                column: "approver2_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_approved_emp_separation_cancellation_apprver3_id",
                table: "tbl_approved_emp_separation_cancellation",
                column: "apprver3_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_approved_emp_separation_cancellation_fkid_empSepration",
                table: "tbl_approved_emp_separation_cancellation",
                column: "fkid_empSepration");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_assets_approval_asset_req_id",
                table: "tbl_assets_approval",
                column: "asset_req_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_assets_approval_emp_id",
                table: "tbl_assets_approval",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_assets_master_company_id",
                table: "tbl_assets_master",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_assets_request_master_assets_master_id",
                table: "tbl_assets_request_master",
                column: "assets_master_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_assets_request_master_req_employee_id",
                table: "tbl_assets_request_master",
                column: "req_employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attendace_request_a1_e_id",
                table: "tbl_attendace_request",
                column: "a1_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attendace_request_a2_e_id",
                table: "tbl_attendace_request",
                column: "a2_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attendace_request_a3_e_id",
                table: "tbl_attendace_request",
                column: "a3_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attendace_request_admin_id",
                table: "tbl_attendace_request",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attendace_request_company_id",
                table: "tbl_attendace_request",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attendace_request_r_e_id",
                table: "tbl_attendace_request",
                column: "r_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attendance_details_emp_id",
                table: "tbl_attendance_details",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attendance_details_machine_id",
                table: "tbl_attendance_details",
                column: "machine_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attendance_details_log_a_id",
                table: "tbl_attendance_details_log",
                column: "a_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attendance_details_log_e_id",
                table: "tbl_attendance_details_log",
                column: "e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attendance_details_log_machine_id",
                table: "tbl_attendance_details_log",
                column: "machine_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_attendance_details_manual_emp_id",
                table: "tbl_attendance_details_manual",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_city_state_id",
                table: "tbl_city",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_claim_master_log_claim_master_id",
                table: "tbl_claim_master_log",
                column: "claim_master_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_comb_off_log_comp_off_id",
                table: "tbl_comb_off_log",
                column: "comp_off_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_comp_off_ledger_deleted_by",
                table: "tbl_comp_off_ledger",
                column: "deleted_by");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_comp_off_ledger_e_id",
                table: "tbl_comp_off_ledger",
                column: "e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_comp_off_ledger_log_e_id",
                table: "tbl_comp_off_ledger_log",
                column: "e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_comp_off_ledger_log_sno",
                table: "tbl_comp_off_ledger_log",
                column: "sno");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_comp_off_request_master_a1_e_id",
                table: "tbl_comp_off_request_master",
                column: "a1_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_comp_off_request_master_a2_e_id",
                table: "tbl_comp_off_request_master",
                column: "a2_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_comp_off_request_master_a3_e_id",
                table: "tbl_comp_off_request_master",
                column: "a3_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_comp_off_request_master_admin_id",
                table: "tbl_comp_off_request_master",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_comp_off_request_master_company_id",
                table: "tbl_comp_off_request_master",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_comp_off_request_master_r_e_id",
                table: "tbl_comp_off_request_master",
                column: "r_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_company_emp_setting_company_id",
                table: "tbl_company_emp_setting",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_company_master_city_id",
                table: "tbl_company_master",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_company_master_country_id",
                table: "tbl_company_master",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_company_master_state_id",
                table: "tbl_company_master",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_company_master_log_company_id",
                table: "tbl_company_master_log",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_compoff_raise_a1_e_id",
                table: "tbl_compoff_raise",
                column: "a1_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_compoff_raise_a2_e_id",
                table: "tbl_compoff_raise",
                column: "a2_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_compoff_raise_a3_e_id",
                table: "tbl_compoff_raise",
                column: "a3_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_compoff_raise_admin_id",
                table: "tbl_compoff_raise",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_compoff_raise_emp_id",
                table: "tbl_compoff_raise",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_component_formula_details_company_id",
                table: "tbl_component_formula_details",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_component_formula_details_component_id",
                table: "tbl_component_formula_details",
                column: "component_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_component_formula_details_salary_group_id",
                table: "tbl_component_formula_details",
                column: "salary_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_daily_attendance_emp_offcl_id",
                table: "tbl_daily_attendance",
                column: "emp_offcl_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_daily_attendance_holiday_id",
                table: "tbl_daily_attendance",
                column: "holiday_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_daily_attendance_leave_request_id",
                table: "tbl_daily_attendance",
                column: "leave_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_daily_attendance_s_d_id",
                table: "tbl_daily_attendance",
                column: "s_d_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_daily_attendance_shift_id",
                table: "tbl_daily_attendance",
                column: "shift_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_department_master_employee_id",
                table: "tbl_department_master",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_department_master_log_department_id",
                table: "tbl_department_master_log",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_designation_log_desig_id",
                table: "tbl_designation_log",
                column: "desig_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_document_type_master_company_id",
                table: "tbl_document_type_master",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_adhar_details_employee_id",
                table: "tbl_emp_adhar_details",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_bank_details_bank_acc",
                table: "tbl_emp_bank_details",
                column: "bank_acc",
                unique: true,
                filter: "[is_deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_bank_details_bank_id",
                table: "tbl_emp_bank_details",
                column: "bank_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_bank_details_employee_id",
                table: "tbl_emp_bank_details",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_desi_allocation_desig_id",
                table: "tbl_emp_desi_allocation",
                column: "desig_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_desi_allocation_employee_id",
                table: "tbl_emp_desi_allocation",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_documents_company_id",
                table: "tbl_emp_documents",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_documents_doc_type_id",
                table: "tbl_emp_documents",
                column: "doc_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_documents_emp_id",
                table: "tbl_emp_documents",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_esic_details_employee_id",
                table: "tbl_emp_esic_details",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_esic_details_esic_number",
                table: "tbl_emp_esic_details",
                column: "esic_number",
                unique: true,
                filter: "[is_deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_family_sec_employee_id",
                table: "tbl_emp_family_sec",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_fnf_asset_fnf_id",
                table: "tbl_emp_fnf_asset",
                column: "fnf_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_grade_allocation_employee_id",
                table: "tbl_emp_grade_allocation",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_grade_allocation_grade_id",
                table: "tbl_emp_grade_allocation",
                column: "grade_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_manager_employee_id",
                table: "tbl_emp_manager",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_manager_m_one_id",
                table: "tbl_emp_manager",
                column: "m_one_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_manager_m_three_id",
                table: "tbl_emp_manager",
                column: "m_three_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_manager_m_two_id",
                table: "tbl_emp_manager",
                column: "m_two_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_officaial_sec_department_id",
                table: "tbl_emp_officaial_sec",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_officaial_sec_employee_id",
                table: "tbl_emp_officaial_sec",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_officaial_sec_empmnt__id",
                table: "tbl_emp_officaial_sec",
                column: "empmnt__id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_officaial_sec_location_id",
                table: "tbl_emp_officaial_sec",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_officaial_sec_religion_id",
                table: "tbl_emp_officaial_sec",
                column: "religion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_officaial_sec_state_id",
                table: "tbl_emp_officaial_sec",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_officaial_sec_sub_dept_id",
                table: "tbl_emp_officaial_sec",
                column: "sub_dept_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_officaial_sec_sub_location_id",
                table: "tbl_emp_officaial_sec",
                column: "sub_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_pan_details_employee_id",
                table: "tbl_emp_pan_details",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_personal_sec_employee_id",
                table: "tbl_emp_personal_sec",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_pf_details_bank_id",
                table: "tbl_emp_pf_details",
                column: "bank_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_pf_details_employee_id",
                table: "tbl_emp_pf_details",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_pf_details_pf_number",
                table: "tbl_emp_pf_details",
                column: "pf_number",
                unique: true,
                filter: "[is_deleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_prev_employement_current_comp_id",
                table: "tbl_emp_prev_employement",
                column: "current_comp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_prev_employement_emp_id",
                table: "tbl_emp_prev_employement",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_qualification_sec_employee_id",
                table: "tbl_emp_qualification_sec",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_salary_master_component_id",
                table: "tbl_emp_salary_master",
                column: "component_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_salary_master_emp_id",
                table: "tbl_emp_salary_master",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_separation_admin_id",
                table: "tbl_emp_separation",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_separation_approver1_id",
                table: "tbl_emp_separation",
                column: "approver1_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_separation_approver2_id",
                table: "tbl_emp_separation",
                column: "approver2_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_separation_apprver3_id",
                table: "tbl_emp_separation",
                column: "apprver3_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_separation_company_id",
                table: "tbl_emp_separation",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_separation_emp_id",
                table: "tbl_emp_separation",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_shift_allocation_employee_id",
                table: "tbl_emp_shift_allocation",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_shift_allocation_shift_id",
                table: "tbl_emp_shift_allocation",
                column: "shift_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_weekoff_employee_id",
                table: "tbl_emp_weekoff",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_withdrawal_company_id",
                table: "tbl_emp_withdrawal",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_withdrawal_emp_id",
                table: "tbl_emp_withdrawal",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_working_role_allocation_employee_id",
                table: "tbl_emp_working_role_allocation",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_working_role_allocation_work_r_id",
                table: "tbl_emp_working_role_allocation",
                column: "work_r_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_employee_company_map_company_id",
                table: "tbl_employee_company_map",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_employee_company_map_employee_id",
                table: "tbl_employee_company_map",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_employee_company_map_log_cid",
                table: "tbl_employee_company_map_log",
                column: "cid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_employee_company_map_log_eid",
                table: "tbl_employee_company_map_log",
                column: "eid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_employee_company_map_log_mid",
                table: "tbl_employee_company_map_log",
                column: "mid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_employee_income_tax_amount_emp_id",
                table: "tbl_employee_income_tax_amount",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_employee_master_emp_code",
                table: "tbl_employee_master",
                column: "emp_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_employeementtype_settings_company_id",
                table: "tbl_employeementtype_settings",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_employeementtype_settings_grade_id",
                table: "tbl_employeementtype_settings",
                column: "grade_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_employment_type_master_employee_id",
                table: "tbl_employment_type_master",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_cycle_master_company_id",
                table: "tbl_epa_cycle_master",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_fiscal_yr_mstr_company_id",
                table: "tbl_epa_fiscal_yr_mstr",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_kpi_criteria_submission_crit_id",
                table: "tbl_epa_kpi_criteria_submission",
                column: "crit_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_kpi_criteria_submission_kpi_submission_id",
                table: "tbl_epa_kpi_criteria_submission",
                column: "kpi_submission_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_kpi_submission_key_area_id",
                table: "tbl_epa_kpi_submission",
                column: "key_area_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_kpi_submission_submission_id",
                table: "tbl_epa_kpi_submission",
                column: "submission_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_kra_submission_kra_id",
                table: "tbl_epa_kra_submission",
                column: "kra_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_kra_submission_rating_id",
                table: "tbl_epa_kra_submission",
                column: "rating_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_kra_submission_submission_id",
                table: "tbl_epa_kra_submission",
                column: "submission_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_question_submission_question_id",
                table: "tbl_epa_question_submission",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_question_submission_submission_id",
                table: "tbl_epa_question_submission",
                column: "submission_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_question_submission_tab_id",
                table: "tbl_epa_question_submission",
                column: "tab_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_status_master_company_id",
                table: "tbl_epa_status_master",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_submission_company_id",
                table: "tbl_epa_submission",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_submission_department_id",
                table: "tbl_epa_submission",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_submission_desig_id",
                table: "tbl_epa_submission",
                column: "desig_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_submission_emp_id",
                table: "tbl_epa_submission",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_submission_emp_off_id",
                table: "tbl_epa_submission",
                column: "emp_off_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_submission_epa_current_status",
                table: "tbl_epa_submission",
                column: "epa_current_status");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_submission_fiscal_year_id",
                table: "tbl_epa_submission",
                column: "fiscal_year_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_epa_submission_w_r_id",
                table: "tbl_epa_submission",
                column: "w_r_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_event_master_company_id",
                table: "tbl_event_master",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_fnf_attendance_dtl_fnf_id",
                table: "tbl_fnf_attendance_dtl",
                column: "fnf_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_fnf_component_component_id",
                table: "tbl_fnf_component",
                column: "component_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_fnf_component_fnf_id",
                table: "tbl_fnf_component",
                column: "fnf_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_fnf_leave_encash_fnf_id",
                table: "tbl_fnf_leave_encash",
                column: "fnf_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_fnf_leave_encash_leave_type_id",
                table: "tbl_fnf_leave_encash",
                column: "leave_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_fnf_loan_recover_loan_req_id",
                table: "tbl_fnf_loan_recover",
                column: "loan_req_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_fnf_master_emp_id",
                table: "tbl_fnf_master",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_fnf_master_fkid_empSepration",
                table: "tbl_fnf_master",
                column: "fkid_empSepration");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_fnf_reimburesment_fnf_id",
                table: "tbl_fnf_reimburesment",
                column: "fnf_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_grade_master_log_grade_id",
                table: "tbl_grade_master_log",
                column: "grade_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_health_card_master_employee_id",
                table: "tbl_health_card_master",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holi_comp_list_log_cid",
                table: "tbl_holi_comp_list_log",
                column: "cid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holi_comp_list_log_h_id",
                table: "tbl_holi_comp_list_log",
                column: "h_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holi_comp_list_log_l_id",
                table: "tbl_holi_comp_list_log",
                column: "l_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holi_comp_list_log_lid",
                table: "tbl_holi_comp_list_log",
                column: "lid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holiday_master_comp_list_company_id",
                table: "tbl_holiday_master_comp_list",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holiday_master_comp_list_holiday_id",
                table: "tbl_holiday_master_comp_list",
                column: "holiday_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holiday_master_comp_list_location_id",
                table: "tbl_holiday_master_comp_list",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holiday_master_emp_list_company_id",
                table: "tbl_holiday_master_emp_list",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holiday_master_emp_list_employee_id",
                table: "tbl_holiday_master_emp_list",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holiday_master_emp_list_holiday_id",
                table: "tbl_holiday_master_emp_list",
                column: "holiday_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holiday_master_log_holiday_id",
                table: "tbl_holiday_master_log",
                column: "holiday_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holiday_mstr_rel_list_h_id",
                table: "tbl_holiday_mstr_rel_list",
                column: "h_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holiday_mstr_rel_list_religion_id",
                table: "tbl_holiday_mstr_rel_list",
                column: "religion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holiday_rel_list_log_h_r_id",
                table: "tbl_holiday_rel_list_log",
                column: "h_r_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holiday_rel_list_log_holiday_id",
                table: "tbl_holiday_rel_list_log",
                column: "holiday_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holiday_rel_list_log_religion_id",
                table: "tbl_holiday_rel_list_log",
                column: "religion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holyd_emp_list_log_employee_id",
                table: "tbl_holyd_emp_list_log",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holyd_emp_list_log_h_id",
                table: "tbl_holyd_emp_list_log",
                column: "h_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_holyd_emp_list_log_holiday_id",
                table: "tbl_holyd_emp_list_log",
                column: "holiday_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_kpi_criteria_company_id",
                table: "tbl_kpi_criteria",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_kpi_criteria_master_k_a_id",
                table: "tbl_kpi_criteria_master",
                column: "k_a_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_kpi_key_area_master_company_id",
                table: "tbl_kpi_key_area_master",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_kpi_key_area_master_otype_id",
                table: "tbl_kpi_key_area_master",
                column: "otype_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_kpi_key_area_master_w_r_id",
                table: "tbl_kpi_key_area_master",
                column: "w_r_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_kpi_objective_type_company_id",
                table: "tbl_kpi_objective_type",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_kpi_rating_master_company_id",
                table: "tbl_kpi_rating_master",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_kra_master_company_id",
                table: "tbl_kra_master",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_kra_master_department_id",
                table: "tbl_kra_master",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_kra_master_wrk_role_id",
                table: "tbl_kra_master",
                column: "wrk_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_kt_status_Kt_Master_id",
                table: "tbl_kt_status",
                column: "Kt_Master_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_kt_task_emp_details_Kt_Master_id",
                table: "tbl_kt_task_emp_details",
                column: "Kt_Master_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_kt_task_master_emp_sepration_Id",
                table: "tbl_kt_task_master",
                column: "emp_sepration_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_app_emp_type_log_lid",
                table: "tbl_leave_app_emp_type_log",
                column: "lid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_app_emp_type_log_sno",
                table: "tbl_leave_app_emp_type_log",
                column: "sno");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_app_on_dept_id",
                table: "tbl_leave_app_on_dept",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_app_on_dept_lid",
                table: "tbl_leave_app_on_dept",
                column: "lid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_app_on_emp_type_l_app_id",
                table: "tbl_leave_app_on_emp_type",
                column: "l_app_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_appcbl_on_company_c_id",
                table: "tbl_leave_appcbl_on_company",
                column: "c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_appcbl_on_company_l_a_id",
                table: "tbl_leave_appcbl_on_company",
                column: "l_a_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_appcbl_on_company_location_id",
                table: "tbl_leave_appcbl_on_company",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_appcbl_on_dept_log_d_id",
                table: "tbl_leave_appcbl_on_dept_log",
                column: "d_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_appcbl_on_dept_log_l_a_id",
                table: "tbl_leave_appcbl_on_dept_log",
                column: "l_a_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_appcbl_on_dept_log_sno",
                table: "tbl_leave_appcbl_on_dept_log",
                column: "sno");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_appcbl_on_religion_l_app_id",
                table: "tbl_leave_appcbl_on_religion",
                column: "l_app_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_appcbl_on_religion_religion_id",
                table: "tbl_leave_appcbl_on_religion",
                column: "religion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_appcbl_rel_log_lid",
                table: "tbl_leave_appcbl_rel_log",
                column: "lid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_appcbl_rel_log_r_id",
                table: "tbl_leave_appcbl_rel_log",
                column: "r_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_appcbl_rel_log_sno",
                table: "tbl_leave_appcbl_rel_log",
                column: "sno");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_appcbly_on_comp_log_c_id",
                table: "tbl_leave_appcbly_on_comp_log",
                column: "c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_appcbly_on_comp_log_l_a_id",
                table: "tbl_leave_appcbly_on_comp_log",
                column: "l_a_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_appcbly_on_comp_log_l_id",
                table: "tbl_leave_appcbly_on_comp_log",
                column: "l_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_appcbly_on_comp_log_sno",
                table: "tbl_leave_appcbly_on_comp_log",
                column: "sno");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_applicablity_leave_info_id",
                table: "tbl_leave_applicablity",
                column: "leave_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_applicablity_log_l_app_id",
                table: "tbl_leave_applicablity_log",
                column: "l_app_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_applicablity_log_leave_info_id",
                table: "tbl_leave_applicablity_log",
                column: "leave_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_cashable_leave_info_id",
                table: "tbl_leave_cashable",
                column: "leave_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_cashable_log_leave_cashable_id",
                table: "tbl_leave_cashable_log",
                column: "leave_cashable_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_cashable_log_leave_info_id",
                table: "tbl_leave_cashable_log",
                column: "leave_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_credit_leave_info_id",
                table: "tbl_leave_credit",
                column: "leave_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_credit_log_leave_credit_id",
                table: "tbl_leave_credit_log",
                column: "leave_credit_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_credit_log_leave_info_id",
                table: "tbl_leave_credit_log",
                column: "leave_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_info_leave_type_id",
                table: "tbl_leave_info",
                column: "leave_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_info_log_leave_info_id",
                table: "tbl_leave_info_log",
                column: "leave_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_info_log_leave_type_id",
                table: "tbl_leave_info_log",
                column: "leave_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_ledger_e_id",
                table: "tbl_leave_ledger",
                column: "e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_ledger_leave_info_id",
                table: "tbl_leave_ledger",
                column: "leave_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_ledger_leave_type_id",
                table: "tbl_leave_ledger",
                column: "leave_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_ledger_log_e_id",
                table: "tbl_leave_ledger_log",
                column: "e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_ledger_log_leave_type_id",
                table: "tbl_leave_ledger_log",
                column: "leave_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_ledger_log_sno",
                table: "tbl_leave_ledger_log",
                column: "sno");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_request_a1_e_id",
                table: "tbl_leave_request",
                column: "a1_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_request_a2_e_id",
                table: "tbl_leave_request",
                column: "a2_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_request_a3_e_id",
                table: "tbl_leave_request",
                column: "a3_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_request_admin_id",
                table: "tbl_leave_request",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_request_company_id",
                table: "tbl_leave_request",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_request_leave_info_id",
                table: "tbl_leave_request",
                column: "leave_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_request_leave_type_id",
                table: "tbl_leave_request",
                column: "leave_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_request_r_e_id",
                table: "tbl_leave_request",
                column: "r_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_rule_leave_info_id",
                table: "tbl_leave_rule",
                column: "leave_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_rule_log_leave_credit_id",
                table: "tbl_leave_rule_log",
                column: "leave_credit_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_rule_log_leave_info_id",
                table: "tbl_leave_rule_log",
                column: "leave_info_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leave_type_log_leave_type_id",
                table: "tbl_leave_type_log",
                column: "leave_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_loan_approval_emp_id",
                table: "tbl_loan_approval",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_loan_approval_loan_req_id",
                table: "tbl_loan_approval",
                column: "loan_req_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_loan_approval_setting_approver_role_id",
                table: "tbl_loan_approval_setting",
                column: "approver_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_loan_approval_setting_company_id",
                table: "tbl_loan_approval_setting",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_loan_repayments_loan_id",
                table: "tbl_loan_repayments",
                column: "loan_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_loan_repayments_req_emp_id",
                table: "tbl_loan_repayments",
                column: "req_emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_loan_request_req_emp_id",
                table: "tbl_loan_request",
                column: "req_emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_location_master_city_id",
                table: "tbl_location_master",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_location_master_country_id",
                table: "tbl_location_master",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_location_master_state_id",
                table: "tbl_location_master",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_location_master_log_city_id",
                table: "tbl_location_master_log",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_location_master_log_country_id",
                table: "tbl_location_master_log",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_location_master_log_location_id",
                table: "tbl_location_master_log",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_location_master_log_state_id",
                table: "tbl_location_master_log",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_machine_master_company_id",
                table: "tbl_machine_master",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_machine_master_location_id",
                table: "tbl_machine_master",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_machine_master_sub_location_id",
                table: "tbl_machine_master",
                column: "sub_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_machine_master_log_machine_id",
                table: "tbl_machine_master_log",
                column: "machine_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form1_amt_c_id",
                table: "tbl_muster_form1",
                column: "amt_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form1_comp_id",
                table: "tbl_muster_form1",
                column: "comp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form1_date_c_id",
                table: "tbl_muster_form1",
                column: "date_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form1_date_realised_c_id",
                table: "tbl_muster_form1",
                column: "date_realised_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form1_nature_and_date_c_id",
                table: "tbl_muster_form1",
                column: "nature_and_date_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form1_rate_of_wages_c_id",
                table: "tbl_muster_form1",
                column: "rate_of_wages_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form1_remarks_c_id",
                table: "tbl_muster_form1",
                column: "remarks_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form1_whether_workman_c_id",
                table: "tbl_muster_form1",
                column: "whether_workman_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form1_data_company_id",
                table: "tbl_muster_form1_data",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form1_data_emp_id",
                table: "tbl_muster_form1_data",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_basic_min_rate_pay_c_id",
                table: "tbl_muster_form10",
                column: "basic_min_rate_pay_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_basic_wages_actually_pay_c_id",
                table: "tbl_muster_form10",
                column: "basic_wages_actually_pay_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_comp_id",
                table: "tbl_muster_form10",
                column: "comp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_da_min_rate_pay_c_id",
                table: "tbl_muster_form10",
                column: "da_min_rate_pay_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_da_wages_actually_pay_c_id",
                table: "tbl_muster_form10",
                column: "da_wages_actually_pay_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_date_ofpayment_c_id",
                table: "tbl_muster_form10",
                column: "date_ofpayment_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_employers_pf_c_id",
                table: "tbl_muster_form10",
                column: "employers_pf_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_gross_wages_pay_c_id",
                table: "tbl_muster_form10",
                column: "gross_wages_pay_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_hr_deduction_c_id",
                table: "tbl_muster_form10",
                column: "hr_deduction_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_other_deduction_c_id",
                table: "tbl_muster_form10",
                column: "other_deduction_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_overtime_worked_c_id",
                table: "tbl_muster_form10",
                column: "overtime_worked_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_total_attan_orunit_ofworkdone_c_id",
                table: "tbl_muster_form10",
                column: "total_attan_orunit_ofworkdone_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_total_deduction_c_id",
                table: "tbl_muster_form10",
                column: "total_deduction_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_wages_paid_c_id",
                table: "tbl_muster_form10",
                column: "wages_paid_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_data_company_id",
                table: "tbl_muster_form10_data",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form10_data_emp_id",
                table: "tbl_muster_form10_data",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form11_basic_wages_pay_c_id",
                table: "tbl_muster_form11",
                column: "basic_wages_pay_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form11_comp_id",
                table: "tbl_muster_form11",
                column: "comp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form11_da_wages_pay_c_id",
                table: "tbl_muster_form11",
                column: "da_wages_pay_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form11_gross_wages_pay_c_id",
                table: "tbl_muster_form11",
                column: "gross_wages_pay_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form11_net_wages_pay_c_id",
                table: "tbl_muster_form11",
                column: "net_wages_pay_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form11_overtime_wages_c_id",
                table: "tbl_muster_form11",
                column: "overtime_wages_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form11_pay_incharge_c_id",
                table: "tbl_muster_form11",
                column: "pay_incharge_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form11_total_attand_orwork_done_c_id",
                table: "tbl_muster_form11",
                column: "total_attand_orwork_done_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form11_total_deduction_c_id",
                table: "tbl_muster_form11",
                column: "total_deduction_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form11_data_company_id",
                table: "tbl_muster_form11_data",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form11_data_emp_id",
                table: "tbl_muster_form11_data",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form2_amt_ofdeduc_c_id",
                table: "tbl_muster_form2",
                column: "amt_ofdeduc_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form2_comp_id",
                table: "tbl_muster_form2",
                column: "comp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form2_damage_orloss_and_date_c_id",
                table: "tbl_muster_form2",
                column: "damage_orloss_and_date_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form2_date_ofdeduc_c_id",
                table: "tbl_muster_form2",
                column: "date_ofdeduc_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form2_date_realised_c_id",
                table: "tbl_muster_form2",
                column: "date_realised_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form2_no_of_installment_c_id",
                table: "tbl_muster_form2",
                column: "no_of_installment_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form2_remarks_c_id",
                table: "tbl_muster_form2",
                column: "remarks_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form2_whether_workman_c_id",
                table: "tbl_muster_form2",
                column: "whether_workman_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form2_data_company_id",
                table: "tbl_muster_form2_data",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form2_data_emp_id",
                table: "tbl_muster_form2_data",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form3_advance_amt_c_id",
                table: "tbl_muster_form3",
                column: "advance_amt_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form3_advance_date_c_id",
                table: "tbl_muster_form3",
                column: "advance_date_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form3_comp_id",
                table: "tbl_muster_form3",
                column: "comp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form3_date_total_repaid_c_id",
                table: "tbl_muster_form3",
                column: "date_total_repaid_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form3_no_of_installment_c_id",
                table: "tbl_muster_form3",
                column: "no_of_installment_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form3_postponement_granted_c_id",
                table: "tbl_muster_form3",
                column: "postponement_granted_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form3_purpose_c_id",
                table: "tbl_muster_form3",
                column: "purpose_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form3_remarks_c_id",
                table: "tbl_muster_form3",
                column: "remarks_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form3_data_company_id",
                table: "tbl_muster_form3_data",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form3_data_emp_id",
                table: "tbl_muster_form3_data",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form4_comp_id",
                table: "tbl_muster_form4",
                column: "comp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form4_dop_component_mstrcomponent_id",
                table: "tbl_muster_form4",
                column: "dop_component_mstrcomponent_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form4_extent_overtime_c_id",
                table: "tbl_muster_form4",
                column: "extent_overtime_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form4_normal_earning_c_id",
                table: "tbl_muster_form4",
                column: "normal_earning_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form4_normal_hr_c_id",
                table: "tbl_muster_form4",
                column: "normal_hr_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form4_normal_rate_c_id",
                table: "tbl_muster_form4",
                column: "normal_rate_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form4_overtime_date_c_id",
                table: "tbl_muster_form4",
                column: "overtime_date_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form4_overtime_earning_c_id",
                table: "tbl_muster_form4",
                column: "overtime_earning_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form4_overtime_rate_c_id",
                table: "tbl_muster_form4",
                column: "overtime_rate_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form4_total_earning_c_id",
                table: "tbl_muster_form4",
                column: "total_earning_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form4_total_overtime_c_id",
                table: "tbl_muster_form4",
                column: "total_overtime_c_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form4_data_company_id",
                table: "tbl_muster_form4_data",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_muster_form4_data_emp_id",
                table: "tbl_muster_form4_data",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_No_dues_clearance_Department_department_id",
                table: "tbl_No_dues_clearance_Department",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_No_dues_clearance_Department_fkid_EmpSaperationId",
                table: "tbl_No_dues_clearance_Department",
                column: "fkid_EmpSaperationId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_No_dues_emp_particular_Clearence_detail_fkid_EmpSaperati~",
                table: "tbl_No_dues_emp_particular_Clearence_detail",
                column: "fkid_EmpSaperationId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_No_dues_emp_particular_Clearence_detail_fkid_ParticularId",
                table: "tbl_No_dues_emp_particular_Clearence_detail",
                column: "fkid_ParticularId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_No_dues_emp_particular_Clearence_detail_fkid_department_~",
                table: "tbl_No_dues_emp_particular_Clearence_detail",
                column: "fkid_department_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_No_dues_particular_master_company_id",
                table: "tbl_No_dues_particular_master",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_No_dues_particular_master_department_id",
                table: "tbl_No_dues_particular_master",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_No_dues_particular_responsible_company_id",
                table: "tbl_No_dues_particular_responsible",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_No_dues_particular_responsible_department_id",
                table: "tbl_No_dues_particular_responsible",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_No_dues_particular_responsible_emp_id",
                table: "tbl_No_dues_particular_responsible",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ot_master_emp_id",
                table: "tbl_ot_master",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ot_rate_details_emp_id",
                table: "tbl_ot_rate_details",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ot_rate_details_grade_id",
                table: "tbl_ot_rate_details",
                column: "grade_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ot_rule_master_log_ot_rule_id",
                table: "tbl_ot_rule_master_log",
                column: "ot_rule_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_outdoor_request_a1_e_id",
                table: "tbl_outdoor_request",
                column: "a1_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_outdoor_request_a2_e_id",
                table: "tbl_outdoor_request",
                column: "a2_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_outdoor_request_a3_e_id",
                table: "tbl_outdoor_request",
                column: "a3_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_outdoor_request_admin_id",
                table: "tbl_outdoor_request",
                column: "admin_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_outdoor_request_company_id",
                table: "tbl_outdoor_request",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_outdoor_request_r_e_id",
                table: "tbl_outdoor_request",
                column: "r_e_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_pay_set_log_pay_set_id",
                table: "tbl_pay_set_log",
                column: "pay_set_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payroll_process_status_company_id",
                table: "tbl_payroll_process_status",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_payroll_process_status_emp_id",
                table: "tbl_payroll_process_status",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_question_master_company_id",
                table: "tbl_question_master",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_question_master_tab_id",
                table: "tbl_question_master",
                column: "tab_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_question_master_wrk_role_id",
                table: "tbl_question_master",
                column: "wrk_role_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_religion_master_log_religion_id",
                table: "tbl_religion_master_log",
                column: "religion_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_right_menu_link_company_id",
                table: "tbl_right_menu_link",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rimb_cat_lmt_mstr_rcm_id",
                table: "tbl_rimb_cat_lmt_mstr",
                column: "rcm_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rimb_cat_lmt_mstr_rglm_id",
                table: "tbl_rimb_cat_lmt_mstr",
                column: "rglm_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rimb_grd_lmt_mstr_emp_id",
                table: "tbl_rimb_grd_lmt_mstr",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rimb_grd_lmt_mstr_grade_id",
                table: "tbl_rimb_grd_lmt_mstr",
                column: "grade_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rimb_req_details_rclm_id",
                table: "tbl_rimb_req_details",
                column: "rclm_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rimb_req_details_rrm_id",
                table: "tbl_rimb_req_details",
                column: "rrm_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rimb_req_mstr_emp_id",
                table: "tbl_rimb_req_mstr",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_role_claim_map_claim_id",
                table: "tbl_role_claim_map",
                column: "claim_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_role_claim_map_role_id",
                table: "tbl_role_claim_map",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_role_master_log_user_id",
                table: "tbl_role_master_log",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rpt_title_master_component_id",
                table: "tbl_rpt_title_master",
                column: "component_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_rpt_title_master_rpt_id",
                table: "tbl_rpt_title_master",
                column: "rpt_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_salary_group_grade_Id",
                table: "tbl_salary_group",
                column: "grade_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_salary_input_company_id",
                table: "tbl_salary_input",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_salary_input_component_id",
                table: "tbl_salary_input",
                column: "component_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_salary_input_emp_id",
                table: "tbl_salary_input",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_salary_input_change_company_id",
                table: "tbl_salary_input_change",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_salary_input_change_component_id",
                table: "tbl_salary_input_change",
                column: "component_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_salary_input_change_emp_id",
                table: "tbl_salary_input_change",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_scheduler_details_scheduler_id",
                table: "tbl_scheduler_details",
                column: "scheduler_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sg_maping_emp_id",
                table: "tbl_sg_maping",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sg_maping_salary_group_id",
                table: "tbl_sg_maping",
                column: "salary_group_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_department_dept_id",
                table: "tbl_shift_department",
                column: "dept_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_department_shift_detail_id",
                table: "tbl_shift_department",
                column: "shift_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_details_shift_id",
                table: "tbl_shift_details",
                column: "shift_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_location_company_id",
                table: "tbl_shift_location",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_location_location_id",
                table: "tbl_shift_location",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_location_shift_detail_id",
                table: "tbl_shift_location",
                column: "shift_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_master_log_location_id",
                table: "tbl_shift_master_log",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_master_log_shift_id",
                table: "tbl_shift_master_log",
                column: "shift_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_roster_master_emp_id",
                table: "tbl_shift_roster_master",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_roster_master_shft_id1",
                table: "tbl_shift_roster_master",
                column: "shft_id1");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_roster_master_shft_id2",
                table: "tbl_shift_roster_master",
                column: "shft_id2");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_roster_master_shft_id3",
                table: "tbl_shift_roster_master",
                column: "shft_id3");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_roster_master_shft_id4",
                table: "tbl_shift_roster_master",
                column: "shft_id4");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_roster_master_shft_id5",
                table: "tbl_shift_roster_master",
                column: "shft_id5");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_roster_master_log_emp_id",
                table: "tbl_shift_roster_master_log",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_roster_master_log_r_id",
                table: "tbl_shift_roster_master_log",
                column: "r_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_roster_master_log_shift_id1",
                table: "tbl_shift_roster_master_log",
                column: "shift_id1");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_roster_master_log_shift_id2",
                table: "tbl_shift_roster_master_log",
                column: "shift_id2");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_roster_master_log_shift_id3",
                table: "tbl_shift_roster_master_log",
                column: "shift_id3");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_roster_master_log_shift_id4",
                table: "tbl_shift_roster_master_log",
                column: "shift_id4");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_roster_master_log_shift_id5",
                table: "tbl_shift_roster_master_log",
                column: "shift_id5");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_week_off_emp_id",
                table: "tbl_shift_week_off",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_week_off_emp_weekoff_id",
                table: "tbl_shift_week_off",
                column: "emp_weekoff_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_week_off_shift_detail_id",
                table: "tbl_shift_week_off",
                column: "shift_detail_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_week_off_log_emp_id",
                table: "tbl_shift_week_off_log",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_week_off_log_shift_id",
                table: "tbl_shift_week_off_log",
                column: "shift_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_shift_week_off_log_shift_week_off_id",
                table: "tbl_shift_week_off_log",
                column: "shift_week_off_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_state_country_id",
                table: "tbl_state",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_status_flow_master_next_status_id",
                table: "tbl_status_flow_master",
                column: "next_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_status_flow_master_start_status_id",
                table: "tbl_status_flow_master",
                column: "start_status_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sub_department_master_department_id",
                table: "tbl_sub_department_master",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sub_department_master_log_d_id",
                table: "tbl_sub_department_master_log",
                column: "d_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sub_department_master_log_sid",
                table: "tbl_sub_department_master_log",
                column: "sid");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_sub_location_master_location_id",
                table: "tbl_sub_location_master",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_tab_master_company_id",
                table: "tbl_tab_master",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_tax_slab_Details_tax_slab_id",
                table: "tbl_tax_slab_Details",
                column: "tax_slab_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_login_logs_emp_id",
                table: "tbl_user_login_logs",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_login_logs_login_date_time",
                table: "tbl_user_login_logs",
                column: "login_date_time");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_login_logs_user_id",
                table: "tbl_user_login_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_master_employee_id",
                table: "tbl_user_master",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_role_map_role_id",
                table: "tbl_user_role_map",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_role_map_user_id",
                table: "tbl_user_role_map",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_working_role_company_id",
                table: "tbl_working_role",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_working_role_dept_id",
                table: "tbl_working_role",
                column: "dept_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeBasicDataProc");

            migrationBuilder.DropTable(
                name: "log_tbl_salary_input");

            migrationBuilder.DropTable(
                name: "mdlSalaryInputValues");

            migrationBuilder.DropTable(
                name: "tbl_active_inactive_user_log");

            migrationBuilder.DropTable(
                name: "tbl_app_setting");

            migrationBuilder.DropTable(
                name: "tbl_approved_emp_separation_cancellation");

            migrationBuilder.DropTable(
                name: "tbl_assets_approval");

            migrationBuilder.DropTable(
                name: "tbl_attendace_request");

            migrationBuilder.DropTable(
                name: "tbl_attendance_details_log");

            migrationBuilder.DropTable(
                name: "tbl_attendance_details_manual");

            migrationBuilder.DropTable(
                name: "tbl_captcha_code_details");

            migrationBuilder.DropTable(
                name: "tbl_claim_master_log");

            migrationBuilder.DropTable(
                name: "tbl_comb_off_log");

            migrationBuilder.DropTable(
                name: "tbl_comp_off_ledger_log");

            migrationBuilder.DropTable(
                name: "tbl_comp_off_request_master");

            migrationBuilder.DropTable(
                name: "tbl_company_emp_setting");

            migrationBuilder.DropTable(
                name: "tbl_company_master_log");

            migrationBuilder.DropTable(
                name: "tbl_compoff_raise");

            migrationBuilder.DropTable(
                name: "tbl_component_formula_details");

            migrationBuilder.DropTable(
                name: "tbl_current_openings");

            migrationBuilder.DropTable(
                name: "tbl_daily_attendance");

            migrationBuilder.DropTable(
                name: "tbl_department_master_log");

            migrationBuilder.DropTable(
                name: "tbl_designation_log");

            migrationBuilder.DropTable(
                name: "tbl_emp_adhar_details");

            migrationBuilder.DropTable(
                name: "tbl_emp_bank_details");

            migrationBuilder.DropTable(
                name: "tbl_emp_desi_allocation");

            migrationBuilder.DropTable(
                name: "tbl_emp_documents");

            migrationBuilder.DropTable(
                name: "tbl_emp_esic_details");

            migrationBuilder.DropTable(
                name: "tbl_emp_family_sec");

            migrationBuilder.DropTable(
                name: "tbl_emp_fnf_asset");

            migrationBuilder.DropTable(
                name: "tbl_emp_grade_allocation");

            migrationBuilder.DropTable(
                name: "tbl_emp_manager");

            migrationBuilder.DropTable(
                name: "tbl_emp_pan_details");

            migrationBuilder.DropTable(
                name: "tbl_emp_personal_sec");

            migrationBuilder.DropTable(
                name: "tbl_emp_pf_details");

            migrationBuilder.DropTable(
                name: "tbl_emp_prev_employement");

            migrationBuilder.DropTable(
                name: "tbl_emp_qualification_sec");

            migrationBuilder.DropTable(
                name: "tbl_emp_salary_master");

            migrationBuilder.DropTable(
                name: "tbl_emp_shift_allocation");

            migrationBuilder.DropTable(
                name: "tbl_emp_withdrawal");

            migrationBuilder.DropTable(
                name: "tbl_emp_working_role_allocation");

            migrationBuilder.DropTable(
                name: "tbl_employee_company_map_log");

            migrationBuilder.DropTable(
                name: "tbl_employee_income_tax_amount");

            migrationBuilder.DropTable(
                name: "tbl_employeementtype_settings");

            migrationBuilder.DropTable(
                name: "tbl_epa_cycle_master");

            migrationBuilder.DropTable(
                name: "tbl_epa_kpi_criteria_submission");

            migrationBuilder.DropTable(
                name: "tbl_epa_kra_submission");

            migrationBuilder.DropTable(
                name: "tbl_epa_question_submission");

            migrationBuilder.DropTable(
                name: "tbl_event_master");

            migrationBuilder.DropTable(
                name: "tbl_fnf_attendance_dtl");

            migrationBuilder.DropTable(
                name: "tbl_fnf_component");

            migrationBuilder.DropTable(
                name: "tbl_fnf_leave_encash");

            migrationBuilder.DropTable(
                name: "tbl_fnf_loan_recover");

            migrationBuilder.DropTable(
                name: "tbl_fnf_reimburesment");

            migrationBuilder.DropTable(
                name: "tbl_grade_master_log");

            migrationBuilder.DropTable(
                name: "tbl_guid_detail");

            migrationBuilder.DropTable(
                name: "tbl_health_card_master");

            migrationBuilder.DropTable(
                name: "tbl_holi_comp_list_log");

            migrationBuilder.DropTable(
                name: "tbl_holiday_master_log");

            migrationBuilder.DropTable(
                name: "tbl_holiday_rel_list_log");

            migrationBuilder.DropTable(
                name: "tbl_holyd_emp_list_log");

            migrationBuilder.DropTable(
                name: "tbl_kpi_criteria");

            migrationBuilder.DropTable(
                name: "tbl_kt_file");

            migrationBuilder.DropTable(
                name: "tbl_kt_status");

            migrationBuilder.DropTable(
                name: "tbl_kt_task_emp_details");

            migrationBuilder.DropTable(
                name: "tbl_leave_app_emp_type_log");

            migrationBuilder.DropTable(
                name: "tbl_leave_appcbl_on_dept_log");

            migrationBuilder.DropTable(
                name: "tbl_leave_appcbl_rel_log");

            migrationBuilder.DropTable(
                name: "tbl_leave_appcbly_on_comp_log");

            migrationBuilder.DropTable(
                name: "tbl_leave_applicablity_log");

            migrationBuilder.DropTable(
                name: "tbl_leave_cashable_log");

            migrationBuilder.DropTable(
                name: "tbl_leave_credit_log");

            migrationBuilder.DropTable(
                name: "tbl_leave_info_log");

            migrationBuilder.DropTable(
                name: "tbl_leave_ledger_log");

            migrationBuilder.DropTable(
                name: "tbl_leave_rule_log");

            migrationBuilder.DropTable(
                name: "tbl_leave_type_log");

            migrationBuilder.DropTable(
                name: "tbl_loan_approval");

            migrationBuilder.DropTable(
                name: "tbl_loan_approval_setting");

            migrationBuilder.DropTable(
                name: "tbl_loan_repayments");

            migrationBuilder.DropTable(
                name: "tbl_loan_request_master");

            migrationBuilder.DropTable(
                name: "tbl_location_master_log");

            migrationBuilder.DropTable(
                name: "tbl_lop_detail");

            migrationBuilder.DropTable(
                name: "tbl_lossofpay_master");

            migrationBuilder.DropTable(
                name: "tbl_lossofpay_setting");

            migrationBuilder.DropTable(
                name: "tbl_machine_master_log");

            migrationBuilder.DropTable(
                name: "tbl_menu_master");

            migrationBuilder.DropTable(
                name: "tbl_muster_form1");

            migrationBuilder.DropTable(
                name: "tbl_muster_form1_data");

            migrationBuilder.DropTable(
                name: "tbl_muster_form10");

            migrationBuilder.DropTable(
                name: "tbl_muster_form10_data");

            migrationBuilder.DropTable(
                name: "tbl_muster_form11");

            migrationBuilder.DropTable(
                name: "tbl_muster_form11_data");

            migrationBuilder.DropTable(
                name: "tbl_muster_form2");

            migrationBuilder.DropTable(
                name: "tbl_muster_form2_data");

            migrationBuilder.DropTable(
                name: "tbl_muster_form3");

            migrationBuilder.DropTable(
                name: "tbl_muster_form3_data");

            migrationBuilder.DropTable(
                name: "tbl_muster_form4");

            migrationBuilder.DropTable(
                name: "tbl_muster_form4_data");

            migrationBuilder.DropTable(
                name: "tbl_No_dues_clearance_Department");

            migrationBuilder.DropTable(
                name: "tbl_No_dues_emp_particular_Clearence_detail");

            migrationBuilder.DropTable(
                name: "tbl_No_dues_particular_responsible");

            migrationBuilder.DropTable(
                name: "tbl_ot_master");

            migrationBuilder.DropTable(
                name: "tbl_ot_rate_details");

            migrationBuilder.DropTable(
                name: "tbl_ot_rule_master_log");

            migrationBuilder.DropTable(
                name: "tbl_outdoor_request");

            migrationBuilder.DropTable(
                name: "tbl_pay_set_log");

            migrationBuilder.DropTable(
                name: "tbl_payroll_process_status");

            migrationBuilder.DropTable(
                name: "tbl_policy_master");

            migrationBuilder.DropTable(
                name: "tbl_policy_master_documents");

            migrationBuilder.DropTable(
                name: "tbl_religion_master_log");

            migrationBuilder.DropTable(
                name: "tbl_right_menu_link");

            migrationBuilder.DropTable(
                name: "tbl_rimb_req_details");

            migrationBuilder.DropTable(
                name: "tbl_role_claim_map");

            migrationBuilder.DropTable(
                name: "tbl_role_master_log");

            migrationBuilder.DropTable(
                name: "tbl_role_menu_master");

            migrationBuilder.DropTable(
                name: "tbl_rpt_title_master");

            migrationBuilder.DropTable(
                name: "tbl_salary_input");

            migrationBuilder.DropTable(
                name: "tbl_salary_input_change");

            migrationBuilder.DropTable(
                name: "tbl_scheduler_details");

            migrationBuilder.DropTable(
                name: "tbl_sg_maping");

            migrationBuilder.DropTable(
                name: "tbl_shift_department");

            migrationBuilder.DropTable(
                name: "tbl_shift_location");

            migrationBuilder.DropTable(
                name: "tbl_shift_master_log");

            migrationBuilder.DropTable(
                name: "tbl_shift_roster_master_log");

            migrationBuilder.DropTable(
                name: "tbl_shift_week_off_log");

            migrationBuilder.DropTable(
                name: "tbl_status_flow_master");

            migrationBuilder.DropTable(
                name: "tbl_sub_department_master_log");

            migrationBuilder.DropTable(
                name: "tbl_tax_slab_Details");

            migrationBuilder.DropTable(
                name: "tbl_user_login_logs");

            migrationBuilder.DropTable(
                name: "tbl_user_role_map");

            migrationBuilder.DropTable(
                name: "tbl_assets_request_master");

            migrationBuilder.DropTable(
                name: "tbl_attendance_details");

            migrationBuilder.DropTable(
                name: "tbl_comb_off_rule_master");

            migrationBuilder.DropTable(
                name: "tbl_comp_off_ledger");

            migrationBuilder.DropTable(
                name: "tbl_leave_request");

            migrationBuilder.DropTable(
                name: "tbl_document_type_master");

            migrationBuilder.DropTable(
                name: "tbl_bank_master");

            migrationBuilder.DropTable(
                name: "tbl_employee_company_map");

            migrationBuilder.DropTable(
                name: "tbl_kpi_criteria_master");

            migrationBuilder.DropTable(
                name: "tbl_epa_kpi_submission");

            migrationBuilder.DropTable(
                name: "tbl_kra_master");

            migrationBuilder.DropTable(
                name: "tbl_kpi_rating_master");

            migrationBuilder.DropTable(
                name: "tbl_question_master");

            migrationBuilder.DropTable(
                name: "tbl_fnf_master");

            migrationBuilder.DropTable(
                name: "tbl_holiday_master_comp_list");

            migrationBuilder.DropTable(
                name: "tbl_holiday_mstr_rel_list");

            migrationBuilder.DropTable(
                name: "tbl_holiday_master_emp_list");

            migrationBuilder.DropTable(
                name: "tbl_kt_task_master");

            migrationBuilder.DropTable(
                name: "tbl_leave_app_on_emp_type");

            migrationBuilder.DropTable(
                name: "tbl_leave_app_on_dept");

            migrationBuilder.DropTable(
                name: "tbl_leave_appcbl_on_religion");

            migrationBuilder.DropTable(
                name: "tbl_leave_appcbl_on_company");

            migrationBuilder.DropTable(
                name: "tbl_leave_cashable");

            migrationBuilder.DropTable(
                name: "tbl_leave_credit");

            migrationBuilder.DropTable(
                name: "tbl_leave_ledger");

            migrationBuilder.DropTable(
                name: "tbl_leave_rule");

            migrationBuilder.DropTable(
                name: "tbl_loan_request");

            migrationBuilder.DropTable(
                name: "tbl_No_dues_particular_master");

            migrationBuilder.DropTable(
                name: "tbl_ot_rule_master");

            migrationBuilder.DropTable(
                name: "tbl_payroll_month_setting");

            migrationBuilder.DropTable(
                name: "tbl_rimb_cat_lmt_mstr");

            migrationBuilder.DropTable(
                name: "tbl_rimb_req_mstr");

            migrationBuilder.DropTable(
                name: "tbl_claim_master");

            migrationBuilder.DropTable(
                name: "tbl_report_master");

            migrationBuilder.DropTable(
                name: "tbl_component_master");

            migrationBuilder.DropTable(
                name: "tbl_scheduler_master");

            migrationBuilder.DropTable(
                name: "tbl_salary_group");

            migrationBuilder.DropTable(
                name: "tbl_shift_roster_master");

            migrationBuilder.DropTable(
                name: "tbl_shift_week_off");

            migrationBuilder.DropTable(
                name: "tbl_tax_slab_master");

            migrationBuilder.DropTable(
                name: "tbl_role_master");

            migrationBuilder.DropTable(
                name: "tbl_user_master");

            migrationBuilder.DropTable(
                name: "tbl_assets_master");

            migrationBuilder.DropTable(
                name: "tbl_machine_master");

            migrationBuilder.DropTable(
                name: "tbl_kpi_key_area_master");

            migrationBuilder.DropTable(
                name: "tbl_epa_submission");

            migrationBuilder.DropTable(
                name: "tbl_tab_master");

            migrationBuilder.DropTable(
                name: "tbl_holiday_master");

            migrationBuilder.DropTable(
                name: "tbl_emp_separation");

            migrationBuilder.DropTable(
                name: "tbl_leave_applicablity");

            migrationBuilder.DropTable(
                name: "tbl_remimb_cat_mstr");

            migrationBuilder.DropTable(
                name: "tbl_rimb_grd_lmt_mstr");

            migrationBuilder.DropTable(
                name: "tbl_emp_weekoff");

            migrationBuilder.DropTable(
                name: "tbl_shift_details");

            migrationBuilder.DropTable(
                name: "tbl_kpi_objective_type");

            migrationBuilder.DropTable(
                name: "tbl_designation_master");

            migrationBuilder.DropTable(
                name: "tbl_emp_officaial_sec");

            migrationBuilder.DropTable(
                name: "tbl_epa_status_master");

            migrationBuilder.DropTable(
                name: "tbl_epa_fiscal_yr_mstr");

            migrationBuilder.DropTable(
                name: "tbl_working_role");

            migrationBuilder.DropTable(
                name: "tbl_leave_info");

            migrationBuilder.DropTable(
                name: "tbl_grade_master");

            migrationBuilder.DropTable(
                name: "tbl_shift_master");

            migrationBuilder.DropTable(
                name: "tbl_employment_type_master");

            migrationBuilder.DropTable(
                name: "tbl_religion_master");

            migrationBuilder.DropTable(
                name: "tbl_sub_department_master");

            migrationBuilder.DropTable(
                name: "tbl_sub_location_master");

            migrationBuilder.DropTable(
                name: "tbl_company_master");

            migrationBuilder.DropTable(
                name: "tbl_leave_type");

            migrationBuilder.DropTable(
                name: "tbl_department_master");

            migrationBuilder.DropTable(
                name: "tbl_location_master");

            migrationBuilder.DropTable(
                name: "tbl_employee_master");

            migrationBuilder.DropTable(
                name: "tbl_city");

            migrationBuilder.DropTable(
                name: "tbl_state");

            migrationBuilder.DropTable(
                name: "tbl_country");
        }
    }
}

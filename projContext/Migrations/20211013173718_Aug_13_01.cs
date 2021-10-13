using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class Aug_13_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_emp_officaial_sec_tbl_department_master_department_id",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_emp_officaial_sec_tbl_employment_type_master_empmnt__id",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_emp_officaial_sec_tbl_location_master_location_id",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_emp_officaial_sec_tbl_sub_department_master_sub_dept_id",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_emp_officaial_sec_tbl_sub_location_master_sub_location_id",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropTable(
                name: "tbl_claim_master_log");

            migrationBuilder.DropTable(
                name: "tbl_employee_company_map_log");

            migrationBuilder.DropTable(
                name: "tbl_kt_file");

            migrationBuilder.DropTable(
                name: "tbl_kt_status");

            migrationBuilder.DropTable(
                name: "tbl_kt_task_emp_details");

            migrationBuilder.DropTable(
                name: "tbl_No_dues_clearance_Department");

            migrationBuilder.DropTable(
                name: "tbl_No_dues_emp_particular_Clearence_detail");

            migrationBuilder.DropTable(
                name: "tbl_No_dues_particular_responsible");

            migrationBuilder.DropTable(
                name: "tbl_role_master_log");

            migrationBuilder.DropTable(
                name: "tbl_kt_task_master");

            migrationBuilder.DropTable(
                name: "tbl_No_dues_particular_master");

            migrationBuilder.DropIndex(
                name: "IX_tbl_emp_officaial_sec_department_id",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropIndex(
                name: "IX_tbl_emp_officaial_sec_empmnt__id",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropIndex(
                name: "IX_tbl_emp_officaial_sec_location_id",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropIndex(
                name: "IX_tbl_emp_officaial_sec_sub_dept_id",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropIndex(
                name: "IX_tbl_emp_officaial_sec_sub_location_id",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "actual_duration_days",
                table: "tbl_employment_type_master");

            migrationBuilder.DropColumn(
                name: "actual_duration_end_period",
                table: "tbl_employment_type_master");

            migrationBuilder.DropColumn(
                name: "actual_duration_start_period",
                table: "tbl_employment_type_master");

            migrationBuilder.DropColumn(
                name: "duration_days",
                table: "tbl_employment_type_master");

            migrationBuilder.DropColumn(
                name: "duration_end_period",
                table: "tbl_employment_type_master");

            migrationBuilder.DropColumn(
                name: "duration_start_period",
                table: "tbl_employment_type_master");

            migrationBuilder.DropColumn(
                name: "confirmation_date",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "current_employee_type",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "date_of_birth",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "date_of_joining",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "department_date_of_joining",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "department_id",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "employement_type",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "empmnt__id",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "group_joining_date",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "is_applicable_for_all_comp",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "is_comb_off_allowed",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "is_fixed_weekly_off",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "is_mobile_access",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "is_mobile_attendence_access",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "is_ot_allowed",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "is_sandwiche_applicable",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "last_working_date",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "location_id",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "mobile_punch_from_date",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "mobile_punch_to_date",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "notice_period",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "punch_type",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "sub_dept_id",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "sub_location_id",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.DropColumn(
                name: "user_type",
                table: "tbl_emp_officaial_sec");

            migrationBuilder.AlterColumn<bool>(
                name: "is_default",
                table: "tbl_employee_company_map",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "BIT");

            migrationBuilder.AlterColumn<string>(
                name: "salutation",
                table: "tbl_emp_officaial_sec",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "official_email_id",
                table: "tbl_emp_officaial_sec",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nationality",
                table: "tbl_emp_officaial_sec",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "is_deleted",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "employee_middle_name",
                table: "tbl_emp_officaial_sec",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "employee_last_name",
                table: "tbl_emp_officaial_sec",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "employee_first_name",
                table: "tbl_emp_officaial_sec",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "emp_father_name",
                table: "tbl_emp_officaial_sec",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "card_number",
                table: "tbl_emp_officaial_sec",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "tbl_emp_attendance_setting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    is_ot_allowed = table.Column<byte>(nullable: false),
                    is_comb_off_allowed = table.Column<int>(nullable: false),
                    MinOtHourReq = table.Column<int>(nullable: false),
                    is_sandwiche_applicable = table.Column<bool>(nullable: false),
                    punch_type = table.Column<byte>(nullable: false),
                    is_mobile_attendence_access = table.Column<int>(nullable: false),
                    mobile_punch_from_date = table.Column<DateTime>(nullable: false),
                    mobile_punch_to_date = table.Column<DateTime>(nullable: false),
                    effective_from_date = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modifed_by = table.Column<int>(nullable: false),
                    modifed_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_attendance_setting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_attendance_setting_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_department_allocation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    department_id = table.Column<int>(nullable: true),
                    sub_dept_id = table.Column<int>(nullable: true),
                    effective_from_date = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modifed_by = table.Column<int>(nullable: false),
                    modifed_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_department_allocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_department_allocation_tbl_department_master_departme~",
                        column: x => x.department_id,
                        principalTable: "tbl_department_master",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_department_allocation_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_department_allocation_tbl_sub_department_master_sub_~",
                        column: x => x.sub_dept_id,
                        principalTable: "tbl_sub_department_master",
                        principalColumn: "sub_department_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_emp_location_allocation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    employee_id = table.Column<int>(nullable: true),
                    location_id = table.Column<int>(nullable: true),
                    sub_location_id = table.Column<int>(nullable: true),
                    effective_from_date = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    modifed_by = table.Column<int>(nullable: false),
                    modifed_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_emp_location_allocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbl_emp_location_allocation_tbl_employee_master_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tbl_employee_master",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_location_allocation_tbl_location_master_location_id",
                        column: x => x.location_id,
                        principalTable: "tbl_location_master",
                        principalColumn: "location_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_emp_location_allocation_tbl_sub_location_master_sub_loca~",
                        column: x => x.sub_location_id,
                        principalTable: "tbl_sub_location_master",
                        principalColumn: "sub_location_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_attendance_setting_employee_id",
                table: "tbl_emp_attendance_setting",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_department_allocation_department_id",
                table: "tbl_emp_department_allocation",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_department_allocation_employee_id",
                table: "tbl_emp_department_allocation",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_department_allocation_sub_dept_id",
                table: "tbl_emp_department_allocation",
                column: "sub_dept_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_location_allocation_employee_id",
                table: "tbl_emp_location_allocation",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_location_allocation_location_id",
                table: "tbl_emp_location_allocation",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_location_allocation_sub_location_id",
                table: "tbl_emp_location_allocation",
                column: "sub_location_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_emp_attendance_setting");

            migrationBuilder.DropTable(
                name: "tbl_emp_department_allocation");

            migrationBuilder.DropTable(
                name: "tbl_emp_location_allocation");

            migrationBuilder.AddColumn<int>(
                name: "actual_duration_days",
                table: "tbl_employment_type_master",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "actual_duration_end_period",
                table: "tbl_employment_type_master",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "actual_duration_start_period",
                table: "tbl_employment_type_master",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "duration_days",
                table: "tbl_employment_type_master",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "duration_end_period",
                table: "tbl_employment_type_master",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "duration_start_period",
                table: "tbl_employment_type_master",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<bool>(
                name: "is_default",
                table: "tbl_employee_company_map",
                type: "BIT",
                nullable: false,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>(
                name: "salutation",
                table: "tbl_emp_officaial_sec",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "official_email_id",
                table: "tbl_emp_officaial_sec",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nationality",
                table: "tbl_emp_officaial_sec",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "is_deleted",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<string>(
                name: "employee_middle_name",
                table: "tbl_emp_officaial_sec",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "employee_last_name",
                table: "tbl_emp_officaial_sec",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "employee_first_name",
                table: "tbl_emp_officaial_sec",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "emp_father_name",
                table: "tbl_emp_officaial_sec",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "card_number",
                table: "tbl_emp_officaial_sec",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "confirmation_date",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte>(
                name: "current_employee_type",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "date_of_birth",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "date_of_joining",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "department_date_of_joining",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "department_id",
                table: "tbl_emp_officaial_sec",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "employement_type",
                table: "tbl_emp_officaial_sec",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "empmnt__id",
                table: "tbl_emp_officaial_sec",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "group_joining_date",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte>(
                name: "is_applicable_for_all_comp",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "is_comb_off_allowed",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "is_fixed_weekly_off",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "is_mobile_access",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "is_mobile_attendence_access",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "is_ot_allowed",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<bool>(
                name: "is_sandwiche_applicable",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "last_working_date",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "location_id",
                table: "tbl_emp_officaial_sec",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "mobile_punch_from_date",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "mobile_punch_to_date",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "notice_period",
                table: "tbl_emp_officaial_sec",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "punch_type",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "sub_dept_id",
                table: "tbl_emp_officaial_sec",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "sub_location_id",
                table: "tbl_emp_officaial_sec",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "user_type",
                table: "tbl_emp_officaial_sec",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tbl_claim_master_log",
                columns: table => new
                {
                    claim_master_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    claim_master_id = table.Column<int>(nullable: true),
                    claim_master_name = table.Column<string>(maxLength: 50, nullable: true),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
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
                name: "tbl_employee_company_map_log",
                columns: table => new
                {
                    sno_log = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    cid = table.Column<int>(nullable: true),
                    eid = table.Column<int>(nullable: true),
                    is_approved = table.Column<byte>(nullable: false),
                    mid = table.Column<int>(nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
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
                name: "tbl_kt_file",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ktfile = table.Column<string>(nullable: true),
                    last_modified_by = table.Column<int>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: false),
                    seperation_id = table.Column<int>(nullable: false),
                    uploadedOn = table.Column<DateTime>(nullable: false),
                    uploaded_by = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_kt_file", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_kt_task_master",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    HandoverDate = table.Column<DateTime>(nullable: false),
                    ModHandover = table.Column<string>(maxLength: 500, nullable: true),
                    Procedure = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Task_Sno = table.Column<string>(nullable: true),
                    created_by = table.Column<int>(nullable: false),
                    emp_sepration_Id = table.Column<int>(nullable: true),
                    is_active = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: false),
                    remarks = table.Column<string>(maxLength: 500, nullable: true),
                    taskName = table.Column<string>(maxLength: 500, nullable: true)
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
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    department_id = table.Column<int>(nullable: false),
                    fkid_EmpSaperationId = table.Column<int>(nullable: false),
                    is_Cleared = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
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
                name: "tbl_No_dues_particular_master",
                columns: table => new
                {
                    pkid_ParticularMaster = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    company_id = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    department_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false),
                    particular_name = table.Column<string>(nullable: true),
                    remarks = table.Column<string>(nullable: true)
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
                    company_id = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    department_id = table.Column<int>(nullable: false),
                    emp_id = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
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
                name: "tbl_role_master_log",
                columns: table => new
                {
                    role_log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false),
                    role_name = table.Column<string>(nullable: true),
                    user_id = table.Column<int>(nullable: true)
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
                name: "tbl_kt_status",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Kt_Master_id = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    modified_on = table.Column<DateTime>(nullable: false)
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
                    EmpId = table.Column<int>(nullable: false),
                    Kt_Master_id = table.Column<int>(nullable: true),
                    created_on = table.Column<DateTime>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
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
                name: "tbl_No_dues_emp_particular_Clearence_detail",
                columns: table => new
                {
                    pkid_EmpParticularClearance = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    fkid_EmpSaperationId = table.Column<int>(nullable: false),
                    fkid_ParticularId = table.Column<int>(nullable: false),
                    fkid_department_id = table.Column<int>(nullable: false),
                    is_Outstanding = table.Column<int>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false),
                    remarks = table.Column<string>(nullable: true)
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

            migrationBuilder.UpdateData(
                table: "tbl_employment_type_master",
                keyColumn: "employment_type_id",
                keyValue: 1,
                columns: new[] { "actual_duration_days", "actual_duration_end_period", "actual_duration_start_period", "duration_days", "duration_end_period", "duration_start_period" },
                values: new object[] { 1000, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_officaial_sec_department_id",
                table: "tbl_emp_officaial_sec",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_officaial_sec_empmnt__id",
                table: "tbl_emp_officaial_sec",
                column: "empmnt__id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_officaial_sec_location_id",
                table: "tbl_emp_officaial_sec",
                column: "location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_officaial_sec_sub_dept_id",
                table: "tbl_emp_officaial_sec",
                column: "sub_dept_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_emp_officaial_sec_sub_location_id",
                table: "tbl_emp_officaial_sec",
                column: "sub_location_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_claim_master_log_claim_master_id",
                table: "tbl_claim_master_log",
                column: "claim_master_id");

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
                name: "IX_tbl_role_master_log_user_id",
                table: "tbl_role_master_log",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_emp_officaial_sec_tbl_department_master_department_id",
                table: "tbl_emp_officaial_sec",
                column: "department_id",
                principalTable: "tbl_department_master",
                principalColumn: "department_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_emp_officaial_sec_tbl_employment_type_master_empmnt__id",
                table: "tbl_emp_officaial_sec",
                column: "empmnt__id",
                principalTable: "tbl_employment_type_master",
                principalColumn: "employment_type_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_emp_officaial_sec_tbl_location_master_location_id",
                table: "tbl_emp_officaial_sec",
                column: "location_id",
                principalTable: "tbl_location_master",
                principalColumn: "location_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_emp_officaial_sec_tbl_sub_department_master_sub_dept_id",
                table: "tbl_emp_officaial_sec",
                column: "sub_dept_id",
                principalTable: "tbl_sub_department_master",
                principalColumn: "sub_department_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_emp_officaial_sec_tbl_sub_location_master_sub_location_id",
                table: "tbl_emp_officaial_sec",
                column: "sub_location_id",
                principalTable: "tbl_sub_location_master",
                principalColumn: "sub_location_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

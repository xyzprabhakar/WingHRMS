using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class _20_oct_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_tbl_role_claim_map_tbl_claim_master_claim_id",
            //    table: "tbl_role_claim_map");

            //migrationBuilder.DropTable(
            //    name: "tbl_active_inactive_user_log");

            //migrationBuilder.DropTable(
            //    name: "tbl_captcha_code_details");

            //migrationBuilder.DropTable(
            //    name: "tbl_claim_master");

            //migrationBuilder.DropTable(
            //    name: "tbl_company_master_log");

            //migrationBuilder.DropTable(
            //    name: "tbl_user_login_logs");

            //migrationBuilder.DropIndex(
            //    name: "IX_tbl_role_claim_map_claim_id",
            //    table: "tbl_role_claim_map");

            //migrationBuilder.DeleteData(
            //    table: "tbl_user_master",
            //    keyColumn: "user_id",
            //    keyValue: 1);

            //migrationBuilder.DropColumn(
            //    name: "default_company_id",
            //    table: "tbl_user_master");

            //migrationBuilder.DropColumn(
            //    name: "claim_id",
            //    table: "tbl_role_claim_map");

            //migrationBuilder.AlterColumn<ulong>(
            //    name: "user_id",
            //    table: "tbl_user_role_map",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "username",
            //    table: "tbl_user_master",
            //    maxLength: 64,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldMaxLength: 50,
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "user_type",
            //    table: "tbl_user_master",
            //    nullable: false,
            //    oldClrType: typeof(byte));

            //migrationBuilder.AlterColumn<string>(
            //    name: "password",
            //    table: "tbl_user_master",
            //    maxLength: 512,
            //    nullable: true,
            //    oldClrType: typeof(string),
            //    oldMaxLength: 50,
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<ulong>(
            //    name: "user_id",
            //    table: "tbl_user_master",
            //    nullable: false,
            //    oldClrType: typeof(int))
            //    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            //    .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            //migrationBuilder.AddColumn<int>(
            //    name: "CustomerId",
            //    table: "tbl_user_master",
            //    nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "tbl_user_master",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "role_name",
                table: "tbl_role_master",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentMaster",
                table: "tbl_role_claim_map",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "PermissionType",
                table: "tbl_role_claim_map",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "tbl_guid_detail",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateTable(
                name: "tblUserOTPValidation",
                columns: table => new
                {
                    Sno = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<ulong>(nullable: false),
                    SecurityStamp = table.Column<string>(maxLength: 256, nullable: true),
                    SecurityStampValue = table.Column<string>(maxLength: 32, nullable: true),
                    EffectiveFromDt = table.Column<DateTime>(nullable: false),
                    EffectiveToDt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserOTPValidation", x => x.Sno);
                });

            migrationBuilder.CreateTable(
                name: "tblUsersApplication",
                columns: table => new
                {
                    Sno = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<ulong>(nullable: false),
                    Applications = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUsersApplication", x => x.Sno);
                });

            //migrationBuilder.InsertData(
            //    table: "tbl_user_master",
            //    columns: new[] { "user_id", "CustomerId", "NormalizedName", "created_by", "created_date", "employee_id", "is_active", "is_logged_blocked", "is_logged_in", "last_logged_dt", "last_modified_by", "last_modified_date", "logged_blocked_dt", "password", "user_type", "username" },
            //    values: new object[] { 1ul, null, null, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, (byte)0, (byte)0, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "jnZWe3S+++aQtpmKlibdOA==", 2, "Admin" });

            migrationBuilder.UpdateData(
                table: "tbl_user_role_map",
                keyColumn: "claim_master_id",
                keyValue: 1,
                column: "user_id",
                value: 1ul);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_user_master_CustomerId",
                table: "tbl_user_master",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_user_master_tbl_employee_master_CustomerId",
                table: "tbl_user_master",
                column: "CustomerId",
                principalTable: "tbl_employee_master",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_user_master_tbl_employee_master_CustomerId",
                table: "tbl_user_master");

            migrationBuilder.DropTable(
                name: "tblUserOTPValidation");

            migrationBuilder.DropTable(
                name: "tblUsersApplication");

            migrationBuilder.DropIndex(
                name: "IX_tbl_user_master_CustomerId",
                table: "tbl_user_master");

            migrationBuilder.DeleteData(
                table: "tbl_user_master",
                keyColumn: "user_id",
                keyValue: 1ul);

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "tbl_user_master");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "tbl_user_master");

            migrationBuilder.DropColumn(
                name: "DocumentMaster",
                table: "tbl_role_claim_map");

            migrationBuilder.DropColumn(
                name: "PermissionType",
                table: "tbl_role_claim_map");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "tbl_user_role_map",
                nullable: true,
                oldClrType: typeof(ulong),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "username",
                table: "tbl_user_master",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "user_type",
                table: "tbl_user_master",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "tbl_user_master",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "tbl_user_master",
                nullable: false,
                oldClrType: typeof(ulong))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "default_company_id",
                table: "tbl_user_master",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "role_name",
                table: "tbl_role_master",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "claim_id",
                table: "tbl_role_claim_map",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "tbl_guid_detail",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 128);

            migrationBuilder.CreateTable(
                name: "tbl_active_inactive_user_log",
                columns: table => new
                {
                    acinac_user_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_by = table.Column<int>(nullable: false),
                    created_on = table.Column<DateTime>(nullable: false),
                    is_deleted = table.Column<int>(nullable: false),
                    modified_by = table.Column<int>(nullable: false),
                    modified_date = table.Column<DateTime>(nullable: false),
                    remarks = table.Column<string>(nullable: true),
                    transaction_type = table.Column<byte>(nullable: false),
                    user_id = table.Column<int>(nullable: true)
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
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    description = table.Column<string>(maxLength: 500, nullable: true),
                    is_active = table.Column<int>(nullable: false),
                    is_post = table.Column<byte>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false),
                    method_type = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_claim_master", x => x.claim_master_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_company_master_log",
                columns: table => new
                {
                    company_id_log = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    address_line_one = table.Column<string>(maxLength: 250, nullable: true),
                    address_line_two = table.Column<string>(maxLength: 250, nullable: true),
                    approved_by = table.Column<int>(nullable: false),
                    approved_date = table.Column<DateTime>(nullable: false),
                    city = table.Column<int>(nullable: false),
                    company_code = table.Column<string>(maxLength: 50, nullable: false),
                    company_id = table.Column<int>(nullable: true),
                    company_logo = table.Column<string>(nullable: true),
                    company_name = table.Column<string>(nullable: false),
                    company_website = table.Column<string>(maxLength: 100, nullable: true),
                    country = table.Column<int>(nullable: false),
                    is_approved = table.Column<byte>(nullable: false),
                    number_of_character_for_employee_code = table.Column<int>(nullable: false),
                    pin_code = table.Column<int>(nullable: false),
                    prefix_for_employee_code = table.Column<string>(nullable: true),
                    primary_contact_number = table.Column<string>(maxLength: 20, nullable: true),
                    primary_email_id = table.Column<string>(maxLength: 200, nullable: true),
                    requested_by = table.Column<int>(nullable: false),
                    requested_date = table.Column<DateTime>(nullable: false),
                    requested_type = table.Column<byte>(nullable: false),
                    secondary_contact_number = table.Column<string>(maxLength: 20, nullable: true),
                    secondary_email_id = table.Column<string>(maxLength: 200, nullable: true),
                    state = table.Column<int>(nullable: false)
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
                name: "tbl_user_login_logs",
                columns: table => new
                {
                    log_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_id = table.Column<int>(nullable: true),
                    is_wrong_attempt = table.Column<byte>(nullable: false),
                    login_date_time = table.Column<DateTime>(nullable: false),
                    login_ip = table.Column<string>(nullable: true),
                    user_agent = table.Column<string>(nullable: true),
                    user_id = table.Column<int>(nullable: true)
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

            migrationBuilder.InsertData(
                table: "tbl_user_master",
                columns: new[] { "user_id", "created_by", "created_date", "default_company_id", "employee_id", "is_active", "is_logged_blocked", "is_logged_in", "last_logged_dt", "last_modified_by", "last_modified_date", "logged_blocked_dt", "password", "user_type", "username" },
                values: new object[] { 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1, (byte)0, (byte)0, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "jnZWe3S+++aQtpmKlibdOA==", (byte)1, "Admin" });

            migrationBuilder.UpdateData(
                table: "tbl_user_role_map",
                keyColumn: "claim_master_id",
                keyValue: 1,
                column: "user_id",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_role_claim_map_claim_id",
                table: "tbl_role_claim_map",
                column: "claim_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_active_inactive_user_log_user_id",
                table: "tbl_active_inactive_user_log",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_company_master_log_company_id",
                table: "tbl_company_master_log",
                column: "company_id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_role_claim_map_tbl_claim_master_claim_id",
                table: "tbl_role_claim_map",
                column: "claim_id",
                principalTable: "tbl_claim_master",
                principalColumn: "claim_master_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

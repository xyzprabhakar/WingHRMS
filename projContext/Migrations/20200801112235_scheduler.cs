using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class scheduler : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
              name: "tbl_scheduler_details");
            migrationBuilder.CreateTable(
                name: "tbl_scheduler_master",
                columns: table => new
                {
                    scheduler_id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
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
                name: "tbl_scheduler_details",
                columns: table => new
                {
                    scheduler_detail_id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    scheduler_id = table.Column<int>(nullable: true),                    
                    transaction_no = table.Column<int>(nullable: false),
                    last_runing_date = table.Column<DateTime>(nullable: true),                    
                    is_deleted = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_scheduler_details", x => x.scheduler_detail_id);
                    table.ForeignKey(
                       name: "FK_tbl_scheduler_details_master",
                       column: x => x.scheduler_id,
                       principalTable: "tbl_scheduler_master",
                       principalColumn: "scheduler_id",
                       onDelete: ReferentialAction.Restrict);
                });

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_scheduler_details_tbl_scheduler_master_scheduler_id",
                table: "tbl_scheduler_details");

            migrationBuilder.DropTable(
                name: "tbl_scheduler_master");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_scheduler_details",
                table: "tbl_scheduler_details");

            migrationBuilder.DropIndex(
                name: "IX_tbl_scheduler_details_scheduler_id",
                table: "tbl_scheduler_details");

            migrationBuilder.DropColumn(
                name: "scheduler_detail_id",
                table: "tbl_scheduler_details");

            migrationBuilder.RenameColumn(
                name: "transaction_no",
                table: "tbl_scheduler_details",
                newName: "number_of_week");

            migrationBuilder.AlterColumn<int>(
                name: "scheduler_id",
                table: "tbl_scheduler_details",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true)
                .Annotation("MySQL:AutoIncrement", true);

            migrationBuilder.AddColumn<int>(
                name: "is_runing",
                table: "tbl_scheduler_details",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "scheduler_name",
                table: "tbl_scheduler_details",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "pf_number",
                table: "tbl_emp_pf_details",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_scheduler_details",
                table: "tbl_scheduler_details",
                column: "scheduler_id");

            migrationBuilder.UpdateData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 904,
                column: "menu_name",
                value: "Config-II");

            migrationBuilder.InsertData(
                table: "tbl_menu_master",
                columns: new[] { "menu_id", "IconUrl", "SortingOrder", "created_by", "created_date", "is_active", "menu_name", "modified_by", "modified_date", "parent_menu_id", "type", "urll" },
                values: new object[] { 903, "fa fa-briefcase", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Config-I", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 902, (byte)1, "payroll/LoanMaster" });

            migrationBuilder.InsertData(
                table: "tbl_role_menu_master",
                columns: new[] { "role_menu_id", "created_by", "created_date", "menu_id", "modified_by", "modified_date", "role_id" },
                values: new object[,]
                {
                    { 9030001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 903, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 9030101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 903, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 9030102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 903, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 9030103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 903, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 13020011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1302, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 13020021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1302, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 14100011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1410, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 14100021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1410, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 }
                });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projLicensing.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_instance",
                columns: table => new
                {
                    i_id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    instance_id = table.Column<string>(nullable: true),
                    db_connection = table.Column<string>(nullable: true),
                    organisation = table.Column<string>(nullable: true),
                    company_count = table.Column<int>(nullable: false),
                    design_project_url = table.Column<string>(nullable: true),
                    api_project_url = table.Column<string>(nullable: true),
                    superadmin_password = table.Column<string>(nullable: true),
                    superadmin_username = table.Column<string>(nullable: true),
                    premises_type = table.Column<string>(nullable: true),
                    is_active = table.Column<byte>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_instance", x => x.i_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_instance_details",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    i_id = table.Column<int>(nullable: true),
                    instance_id = table.Column<int>(nullable: false),
                    company_id = table.Column<int>(nullable: false),
                    employee_count = table.Column<int>(nullable: false),
                    is_active = table.Column<int>(nullable: false),
                    created_by = table.Column<int>(nullable: false),
                    created_date = table.Column<DateTime>(nullable: false),
                    last_modified_by = table.Column<int>(nullable: false),
                    last_modified_date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_instance_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_tbl_instance_details_tbl_instance_i_id",
                        column: x => x.i_id,
                        principalTable: "tbl_instance",
                        principalColumn: "i_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_instance_details_i_id",
                table: "tbl_instance_details",
                column: "i_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_instance_details");

            migrationBuilder.DropTable(
                name: "tbl_instance");
        }
    }
}

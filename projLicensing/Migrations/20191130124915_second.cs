using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projLicensing.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_api_log",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    instance_id = table.Column<string>(nullable: true),
                    api_type = table.Column<string>(nullable: true),
                    entrydate = table.Column<DateTime>(nullable: false),
                    response = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_api_log", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_api_log");
        }
    }
}

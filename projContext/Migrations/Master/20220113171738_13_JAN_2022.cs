using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Master
{
    public partial class _13_JAN_2022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequiredChangePassword",
                table: "tblUsersMaster",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "tblCodeGenrationMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    CodeGenrationType = table.Column<int>(nullable: false),
                    Prefix = table.Column<string>(nullable: true),
                    IncludeCountryCode = table.Column<bool>(nullable: false),
                    IncludeStateCode = table.Column<bool>(nullable: false),
                    IncludeCompanyCode = table.Column<bool>(nullable: false),
                    IncludeZoneCode = table.Column<bool>(nullable: false),
                    IncludeLocationCode = table.Column<bool>(nullable: false),
                    IncludeYear = table.Column<bool>(nullable: false),
                    IncludeMonthYear = table.Column<bool>(nullable: false),
                    IncludeYearWeek = table.Column<bool>(nullable: false),
                    DigitFormate = table.Column<byte>(nullable: false),
                    OrgId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCodeGenrationMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblCodeGenrationDetails",
                columns: table => new
                {
                    Sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<int>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true),
                    StateCode = table.Column<string>(nullable: true),
                    CompanyCode = table.Column<string>(nullable: true),
                    ZoneCode = table.Column<string>(nullable: true),
                    LocationCode = table.Column<string>(nullable: true),
                    MonthYear = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    YearWeek = table.Column<int>(nullable: false),
                    Counter = table.Column<int>(nullable: false),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCodeGenrationDetails", x => x.Sno);
                    table.ForeignKey(
                        name: "FK_tblCodeGenrationDetails_tblCodeGenrationMaster_Id",
                        column: x => x.Id,
                        principalTable: "tblCodeGenrationMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblCodeGenrationDetails_Id",
                table: "tblCodeGenrationDetails",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblCodeGenrationDetails");

            migrationBuilder.DropTable(
                name: "tblCodeGenrationMaster");

            migrationBuilder.DropColumn(
                name: "RequiredChangePassword",
                table: "tblUsersMaster");
        }
    }
}

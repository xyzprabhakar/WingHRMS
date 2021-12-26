using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Master
{
    public partial class _26_dec_20211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblBankMaster",
                columns: table => new
                {
                    BankId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    BankName = table.Column<string>(maxLength: 256, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBankMaster", x => x.BankId);
                });

            migrationBuilder.CreateTable(
                name: "tblCompanyMaster",
                columns: table => new
                {
                    CompanyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    OfficeAddress = table.Column<string>(maxLength: 254, nullable: true),
                    Locality = table.Column<string>(maxLength: 254, nullable: true),
                    City = table.Column<string>(maxLength: 254, nullable: true),
                    Pincode = table.Column<string>(maxLength: 32, nullable: true),
                    StateId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 254, nullable: true),
                    AlternateEmail = table.Column<string>(maxLength: 254, nullable: true),
                    ContactNo = table.Column<string>(maxLength: 16, nullable: true),
                    AlternateContactNo = table.Column<string>(maxLength: 16, nullable: true),
                    Name = table.Column<string>(maxLength: 254, nullable: true),
                    Logo = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCompanyMaster", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "tblCountry",
                columns: table => new
                {
                    CountryId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(maxLength: 10, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    ContactPrefix = table.Column<string>(maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCountry", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "tblCurrency",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Symbol = table.Column<string>(maxLength: 8, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCurrency", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "tblFileMaster",
                columns: table => new
                {
                    FileId = table.Column<string>(maxLength: 64, nullable: false),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    File = table.Column<byte[]>(nullable: true),
                    FileType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFileMaster", x => x.FileId);
                });

            migrationBuilder.CreateTable(
                name: "tblLocationMaster",
                columns: table => new
                {
                    LocationId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    OfficeAddress = table.Column<string>(maxLength: 254, nullable: true),
                    Locality = table.Column<string>(maxLength: 254, nullable: true),
                    City = table.Column<string>(maxLength: 254, nullable: true),
                    Pincode = table.Column<string>(maxLength: 32, nullable: true),
                    StateId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 254, nullable: true),
                    AlternateEmail = table.Column<string>(maxLength: 254, nullable: true),
                    ContactNo = table.Column<string>(maxLength: 16, nullable: true),
                    AlternateContactNo = table.Column<string>(maxLength: 16, nullable: true),
                    Name = table.Column<string>(maxLength: 254, nullable: true),
                    LocationType = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblLocationMaster", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "tblOrganisation",
                columns: table => new
                {
                    OrgId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    OfficeAddress = table.Column<string>(maxLength: 254, nullable: true),
                    Locality = table.Column<string>(maxLength: 254, nullable: true),
                    City = table.Column<string>(maxLength: 254, nullable: true),
                    Pincode = table.Column<string>(maxLength: 32, nullable: true),
                    StateId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 254, nullable: true),
                    AlternateEmail = table.Column<string>(maxLength: 254, nullable: true),
                    ContactNo = table.Column<string>(maxLength: 16, nullable: true),
                    AlternateContactNo = table.Column<string>(maxLength: 16, nullable: true),
                    Name = table.Column<string>(maxLength: 254, nullable: true),
                    Logo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOrganisation", x => x.OrgId);
                });

            migrationBuilder.CreateTable(
                name: "tblZoneMaster",
                columns: table => new
                {
                    ZoneId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    OfficeAddress = table.Column<string>(maxLength: 254, nullable: true),
                    Locality = table.Column<string>(maxLength: 254, nullable: true),
                    City = table.Column<string>(maxLength: 254, nullable: true),
                    Pincode = table.Column<string>(maxLength: 32, nullable: true),
                    StateId = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 254, nullable: true),
                    AlternateEmail = table.Column<string>(maxLength: 254, nullable: true),
                    ContactNo = table.Column<string>(maxLength: 16, nullable: true),
                    AlternateContactNo = table.Column<string>(maxLength: 16, nullable: true),
                    Name = table.Column<string>(maxLength: 254, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblZoneMaster", x => x.ZoneId);
                });

            migrationBuilder.CreateTable(
                name: "tblState",
                columns: table => new
                {
                    StaeId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(maxLength: 10, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CountryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblState", x => x.StaeId);
                    table.ForeignKey(
                        name: "FK_tblState_tblCountry_CountryId",
                        column: x => x.CountryId,
                        principalTable: "tblCountry",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblState_CountryId",
                table: "tblState",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblBankMaster");

            migrationBuilder.DropTable(
                name: "tblCompanyMaster");

            migrationBuilder.DropTable(
                name: "tblCurrency");

            migrationBuilder.DropTable(
                name: "tblFileMaster");

            migrationBuilder.DropTable(
                name: "tblLocationMaster");

            migrationBuilder.DropTable(
                name: "tblOrganisation");

            migrationBuilder.DropTable(
                name: "tblState");

            migrationBuilder.DropTable(
                name: "tblZoneMaster");

            migrationBuilder.DropTable(
                name: "tblCountry");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Master
{
    public partial class _28_dec_20211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "tblZoneMaster",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ZoneId",
                table: "tblLocationMaster",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrgId",
                table: "tblCompanyMaster",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblRoleMaster",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    RoleName = table.Column<string>(maxLength: 64, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRoleMaster", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "tblUsersMaster",
                columns: table => new
                {
                    UserId = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    UserName = table.Column<string>(maxLength: 64, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 16, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    Password = table.Column<string>(maxLength: 512, nullable: true),
                    UserType = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LoginFailCount = table.Column<byte>(nullable: false),
                    LoginFailCountdt = table.Column<DateTime>(nullable: false),
                    is_logged_blocked = table.Column<byte>(nullable: false),
                    logged_blocked_dt = table.Column<DateTime>(nullable: false),
                    logged_blocked_Enddt = table.Column<DateTime>(nullable: false),
                    Id = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUsersMaster", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "tblRoleClaim",
                columns: table => new
                {
                    RoleClaimId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    RoleId = table.Column<int>(nullable: true),
                    DocumentMaster = table.Column<int>(nullable: false),
                    PermissionType = table.Column<byte>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRoleClaim", x => x.RoleClaimId);
                    table.ForeignKey(
                        name: "FK_tblRoleClaim_tblRoleMaster_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tblRoleMaster",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblUserAllClaim",
                columns: table => new
                {
                    RoleClaimId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<ulong>(nullable: true),
                    DocumentMaster = table.Column<int>(nullable: false),
                    PermissionType = table.Column<byte>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserAllClaim", x => x.RoleClaimId);
                    table.ForeignKey(
                        name: "FK_tblUserAllClaim_tblUsersMaster_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsersMaster",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblUserAllLocationPermission",
                columns: table => new
                {
                    Sno = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<ulong>(nullable: true),
                    LocationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserAllLocationPermission", x => x.Sno);
                    table.ForeignKey(
                        name: "FK_tblUserAllLocationPermission_tblLocationMaster_LocationId",
                        column: x => x.LocationId,
                        principalTable: "tblLocationMaster",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblUserAllLocationPermission_tblUsersMaster_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsersMaster",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblUserClaim",
                columns: table => new
                {
                    RoleClaimId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<ulong>(nullable: true),
                    DocumentMaster = table.Column<int>(nullable: false),
                    PermissionType = table.Column<byte>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserClaim", x => x.RoleClaimId);
                    table.ForeignKey(
                        name: "FK_tblUserClaim_tblUsersMaster_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsersMaster",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblUserCompanyPermission",
                columns: table => new
                {
                    Sno = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<ulong>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    HaveZoneCompanyAccess = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserCompanyPermission", x => x.Sno);
                    table.ForeignKey(
                        name: "FK_tblUserCompanyPermission_tblCompanyMaster_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblCompanyMaster",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblUserCompanyPermission_tblUsersMaster_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsersMaster",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblUserLocationPermission",
                columns: table => new
                {
                    Sno = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<ulong>(nullable: true),
                    LocationId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserLocationPermission", x => x.Sno);
                    table.ForeignKey(
                        name: "FK_tblUserLocationPermission_tblLocationMaster_LocationId",
                        column: x => x.LocationId,
                        principalTable: "tblLocationMaster",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblUserLocationPermission_tblUsersMaster_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsersMaster",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblUserLoginLog",
                columns: table => new
                {
                    Sno = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<ulong>(nullable: true),
                    IPAddress = table.Column<string>(maxLength: 256, nullable: true),
                    DeviceDetails = table.Column<string>(maxLength: 256, nullable: true),
                    LoginStatus = table.Column<bool>(maxLength: 128, nullable: false),
                    FromLocation = table.Column<string>(maxLength: 128, nullable: true),
                    Longitude = table.Column<string>(maxLength: 128, nullable: true),
                    Latitude = table.Column<string>(maxLength: 128, nullable: true),
                    LoginDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserLoginLog", x => x.Sno);
                    table.ForeignKey(
                        name: "FK_tblUserLoginLog_tblUsersMaster_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsersMaster",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblUserOrganisationPermission",
                columns: table => new
                {
                    Sno = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<ulong>(nullable: true),
                    OrgId = table.Column<int>(nullable: true),
                    HaveAllCompanyAccess = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserOrganisationPermission", x => x.Sno);
                    table.ForeignKey(
                        name: "FK_tblUserOrganisationPermission_tblOrganisation_OrgId",
                        column: x => x.OrgId,
                        principalTable: "tblOrganisation",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblUserOrganisationPermission_tblUsersMaster_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsersMaster",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblUserOTP",
                columns: table => new
                {
                    SecurityStamp = table.Column<string>(maxLength: 256, nullable: false),
                    DescId = table.Column<string>(nullable: true),
                    UserId = table.Column<ulong>(nullable: true),
                    TempUserId = table.Column<string>(maxLength: 256, nullable: true),
                    OTP = table.Column<string>(nullable: true),
                    EffectiveFromDt = table.Column<DateTime>(nullable: false),
                    EffectiveToDt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserOTP", x => x.SecurityStamp);
                    table.ForeignKey(
                        name: "FK_tblUserOTP_tblUsersMaster_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsersMaster",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblUserZonePermission",
                columns: table => new
                {
                    Sno = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<ulong>(nullable: true),
                    ZoneId = table.Column<int>(nullable: true),
                    HaveAllLocationAccess = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserZonePermission", x => x.Sno);
                    table.ForeignKey(
                        name: "FK_tblUserZonePermission_tblUsersMaster_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsersMaster",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblUserZonePermission_tblZoneMaster_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "tblZoneMaster",
                        principalColumn: "ZoneId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblZoneMaster_CompanyId",
                table: "tblZoneMaster",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblLocationMaster_ZoneId",
                table: "tblLocationMaster",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_tblCompanyMaster_OrgId",
                table: "tblCompanyMaster",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_tblRoleClaim_RoleId",
                table: "tblRoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserAllClaim_UserId",
                table: "tblUserAllClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserAllLocationPermission_LocationId",
                table: "tblUserAllLocationPermission",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserAllLocationPermission_UserId",
                table: "tblUserAllLocationPermission",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserClaim_UserId",
                table: "tblUserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserCompanyPermission_CompanyId",
                table: "tblUserCompanyPermission",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserCompanyPermission_UserId",
                table: "tblUserCompanyPermission",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserLocationPermission_LocationId",
                table: "tblUserLocationPermission",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserLocationPermission_UserId",
                table: "tblUserLocationPermission",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserLoginLog_UserId",
                table: "tblUserLoginLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserOrganisationPermission_OrgId",
                table: "tblUserOrganisationPermission",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserOrganisationPermission_UserId",
                table: "tblUserOrganisationPermission",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserOTP_UserId",
                table: "tblUserOTP",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserZonePermission_UserId",
                table: "tblUserZonePermission",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserZonePermission_ZoneId",
                table: "tblUserZonePermission",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblCompanyMaster_tblOrganisation_OrgId",
                table: "tblCompanyMaster",
                column: "OrgId",
                principalTable: "tblOrganisation",
                principalColumn: "OrgId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblLocationMaster_tblZoneMaster_ZoneId",
                table: "tblLocationMaster",
                column: "ZoneId",
                principalTable: "tblZoneMaster",
                principalColumn: "ZoneId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblZoneMaster_tblCompanyMaster_CompanyId",
                table: "tblZoneMaster",
                column: "CompanyId",
                principalTable: "tblCompanyMaster",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblCompanyMaster_tblOrganisation_OrgId",
                table: "tblCompanyMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_tblLocationMaster_tblZoneMaster_ZoneId",
                table: "tblLocationMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_tblZoneMaster_tblCompanyMaster_CompanyId",
                table: "tblZoneMaster");

            migrationBuilder.DropTable(
                name: "tblRoleClaim");

            migrationBuilder.DropTable(
                name: "tblUserAllClaim");

            migrationBuilder.DropTable(
                name: "tblUserAllLocationPermission");

            migrationBuilder.DropTable(
                name: "tblUserClaim");

            migrationBuilder.DropTable(
                name: "tblUserCompanyPermission");

            migrationBuilder.DropTable(
                name: "tblUserLocationPermission");

            migrationBuilder.DropTable(
                name: "tblUserLoginLog");

            migrationBuilder.DropTable(
                name: "tblUserOrganisationPermission");

            migrationBuilder.DropTable(
                name: "tblUserOTP");

            migrationBuilder.DropTable(
                name: "tblUserZonePermission");

            migrationBuilder.DropTable(
                name: "tblRoleMaster");

            migrationBuilder.DropTable(
                name: "tblUsersMaster");

            migrationBuilder.DropIndex(
                name: "IX_tblZoneMaster_CompanyId",
                table: "tblZoneMaster");

            migrationBuilder.DropIndex(
                name: "IX_tblLocationMaster_ZoneId",
                table: "tblLocationMaster");

            migrationBuilder.DropIndex(
                name: "IX_tblCompanyMaster_OrgId",
                table: "tblCompanyMaster");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "tblZoneMaster");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "tblLocationMaster");

            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "tblCompanyMaster");
        }
    }
}

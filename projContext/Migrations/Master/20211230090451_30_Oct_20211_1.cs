using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Master
{
    public partial class _30_Oct_20211_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "tblUserAllClaim");

            migrationBuilder.RenameColumn(
                name: "HaveZoneCompanyAccess",
                table: "tblUserCompanyPermission",
                newName: "HaveAllZoneAccess");

            migrationBuilder.AddColumn<int>(
                name: "OrgId",
                table: "tblZoneMaster",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "tblOrganisation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OrgId",
                table: "tblLocationMaster",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tblUserRole",
                columns: table => new
                {
                    UserRoleId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<ulong>(nullable: true),
                    RoleId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserRole", x => x.UserRoleId);
                    table.ForeignKey(
                        name: "FK_tblUserRole_tblRoleMaster_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tblRoleMaster",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tblUserRole_tblUsersMaster_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsersMaster",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblZoneMaster_OrgId",
                table: "tblZoneMaster",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_tblLocationMaster_OrgId",
                table: "tblLocationMaster",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserRole_RoleId",
                table: "tblUserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserRole_UserId",
                table: "tblUserRole",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblLocationMaster_tblOrganisation_OrgId",
                table: "tblLocationMaster",
                column: "OrgId",
                principalTable: "tblOrganisation",
                principalColumn: "OrgId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tblZoneMaster_tblOrganisation_OrgId",
                table: "tblZoneMaster",
                column: "OrgId",
                principalTable: "tblOrganisation",
                principalColumn: "OrgId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblLocationMaster_tblOrganisation_OrgId",
                table: "tblLocationMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_tblZoneMaster_tblOrganisation_OrgId",
                table: "tblZoneMaster");

            migrationBuilder.DropTable(
                name: "tblUserRole");

            migrationBuilder.DropIndex(
                name: "IX_tblZoneMaster_OrgId",
                table: "tblZoneMaster");

            migrationBuilder.DropIndex(
                name: "IX_tblLocationMaster_OrgId",
                table: "tblLocationMaster");

            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "tblZoneMaster");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "tblOrganisation");

            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "tblLocationMaster");

            migrationBuilder.RenameColumn(
                name: "HaveAllZoneAccess",
                table: "tblUserCompanyPermission",
                newName: "HaveZoneCompanyAccess");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "tblUserAllClaim",
                nullable: false,
                defaultValue: false);
        }
    }
}

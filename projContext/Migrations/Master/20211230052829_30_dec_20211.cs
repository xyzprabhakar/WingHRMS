using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Master
{
    public partial class _30_dec_20211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrgId",
                table: "tblUsersMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "tblOrganisation",
                maxLength: 16,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblUsersMaster_OrgId",
                table: "tblUsersMaster",
                column: "OrgId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblUsersMaster_tblOrganisation_OrgId",
                table: "tblUsersMaster",
                column: "OrgId",
                principalTable: "tblOrganisation",
                principalColumn: "OrgId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblUsersMaster_tblOrganisation_OrgId",
                table: "tblUsersMaster");

            migrationBuilder.DropIndex(
                name: "IX_tblUsersMaster_OrgId",
                table: "tblUsersMaster");

            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "tblUsersMaster");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "tblOrganisation");
        }
    }
}

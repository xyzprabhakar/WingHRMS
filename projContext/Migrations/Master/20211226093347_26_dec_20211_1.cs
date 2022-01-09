using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.Master
{
    public partial class _26_dec_20211_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StaeId",
                table: "tblState",
                newName: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StateId",
                table: "tblState",
                newName: "StaeId");
        }
    }
}

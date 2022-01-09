using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class _31_Oct_20211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerType",
                table: "tblCustomerOrganisation",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerType",
                table: "tblCustomerOrganisation");
        }
    }
}

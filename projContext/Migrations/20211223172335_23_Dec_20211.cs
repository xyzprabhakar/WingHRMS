using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class _23_Dec_20211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<ulong>(
                name: "last_modified_by",
                table: "tbl_user_role_map",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<ulong>(
                name: "created_by",
                table: "tbl_user_role_map",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "tbl_user_role_map",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<ulong>(
                name: "last_modified_by",
                table: "tbl_role_claim_map",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<ulong>(
                name: "created_by",
                table: "tbl_role_claim_map",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "tbl_user_role_map");

            migrationBuilder.AlterColumn<int>(
                name: "last_modified_by",
                table: "tbl_user_role_map",
                nullable: false,
                oldClrType: typeof(ulong));

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "tbl_user_role_map",
                nullable: false,
                oldClrType: typeof(ulong));

            migrationBuilder.AlterColumn<int>(
                name: "last_modified_by",
                table: "tbl_role_claim_map",
                nullable: false,
                oldClrType: typeof(ulong));

            migrationBuilder.AlterColumn<int>(
                name: "created_by",
                table: "tbl_role_claim_map",
                nullable: false,
                oldClrType: typeof(ulong));
        }
    }
}

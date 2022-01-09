using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class _20_oct_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblUserOTPValidation",
                table: "tblUserOTPValidation");

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "tblUserOTPValidation",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<ulong>(
                name: "Sno",
                table: "tblUserOTPValidation",
                nullable: false,
                oldClrType: typeof(ulong))
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "TempUserId",
                table: "tblUserOTPValidation",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "tbl_user_master",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "tbl_user_master",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "tbl_user_master",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "tbl_user_master",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblUserOTPValidation",
                table: "tblUserOTPValidation",
                column: "SecurityStamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblUserOTPValidation",
                table: "tblUserOTPValidation");

            migrationBuilder.DropColumn(
                name: "TempUserId",
                table: "tblUserOTPValidation");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "tbl_user_master");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "tbl_user_master");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "tbl_user_master");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "tbl_user_master");

            migrationBuilder.AlterColumn<ulong>(
                name: "Sno",
                table: "tblUserOTPValidation",
                nullable: false,
                oldClrType: typeof(ulong))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "SecurityStamp",
                table: "tblUserOTPValidation",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblUserOTPValidation",
                table: "tblUserOTPValidation",
                column: "Sno");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class _16_dec_2021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pan_details_id",
                table: "tbl_emp_adhar_details",
                newName: "adhar_details_id");

            migrationBuilder.AlterColumn<string>(
                name: "IPAddress",
                table: "tblUserLoginLog",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "permanent_pin_code",
                table: "tbl_emp_personal_sec",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "pan_card_image",
                table: "tbl_emp_pan_details",
                maxLength: 254,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "aadha_card_image",
                table: "tbl_emp_adhar_details",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "adhar_details_id",
                table: "tbl_emp_adhar_details",
                newName: "pan_details_id");

            migrationBuilder.AlterColumn<string>(
                name: "IPAddress",
                table: "tblUserLoginLog",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "permanent_pin_code",
                table: "tbl_emp_personal_sec",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "pan_card_image",
                table: "tbl_emp_pan_details",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 254,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "aadha_card_image",
                table: "tbl_emp_adhar_details",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);
        }
    }
}

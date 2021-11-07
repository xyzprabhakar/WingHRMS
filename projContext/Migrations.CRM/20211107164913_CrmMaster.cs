using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations.CRM
{
    public partial class CrmMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblCustomerIPFilter",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    AllowedAllIp = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCustomerIPFilter", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "tblCustomerMarkup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    Nid = table.Column<int>(nullable: false),
                    BookingType = table.Column<int>(nullable: false),
                    MarkupAmount = table.Column<double>(nullable: false),
                    EffectiveFromDt = table.Column<DateTime>(nullable: false),
                    EffectiveToDt = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCustomerMarkup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblCustomerNotification",
                columns: table => new
                {
                    Sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    UserId = table.Column<ulong>(nullable: false),
                    NotificationType = table.Column<int>(nullable: false),
                    SendSms = table.Column<bool>(nullable: false),
                    SendEmail = table.Column<bool>(nullable: false),
                    SendDeviceNotification = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCustomerNotification", x => x.Sno);
                });

            migrationBuilder.CreateTable(
                name: "tblCustomerWalletAmount",
                columns: table => new
                {
                    Sno = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nid = table.Column<ulong>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    WalletAmount = table.Column<double>(nullable: false),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCustomerWalletAmount", x => x.Sno);
                });

            migrationBuilder.CreateTable(
                name: "tblPaymentRequest",
                columns: table => new
                {
                    PaymentRequestId = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RequestedRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    RequestedBy = table.Column<ulong>(nullable: false),
                    RequestedDt = table.Column<DateTime>(nullable: true),
                    ApprovalStatus = table.Column<byte>(nullable: false),
                    ApprovalRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ApprovedBy = table.Column<ulong>(nullable: true),
                    ApprovedDt = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<int>(nullable: true),
                    Nid = table.Column<ulong>(nullable: false),
                    RequestedAmt = table.Column<double>(nullable: false),
                    Status = table.Column<byte>(nullable: false),
                    TransactionNumber = table.Column<string>(maxLength: 256, nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    TransactionType = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RequestType = table.Column<int>(nullable: false),
                    UploadImages = table.Column<string>(nullable: true),
                    RowVersion = table.Column<DateTime>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPaymentRequest", x => x.PaymentRequestId);
                });

            migrationBuilder.CreateTable(
                name: "tblWalletBalanceAlert",
                columns: table => new
                {
                    Sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<ulong>(nullable: false),
                    CreatedDt = table.Column<DateTime>(nullable: false),
                    ModifyRemarks = table.Column<string>(maxLength: 128, nullable: true),
                    ModifiedBy = table.Column<ulong>(nullable: true),
                    ModifiedDt = table.Column<DateTime>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false),
                    Nid = table.Column<ulong>(nullable: false),
                    MinBalance = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblWalletBalanceAlert", x => x.Sno);
                });

            migrationBuilder.CreateTable(
                name: "tblWalletDetailLedger",
                columns: table => new
                {
                    Sno = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TransactionDt = table.Column<DateTime>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    UserId = table.Column<ulong>(nullable: false),
                    Credit = table.Column<double>(nullable: false),
                    Debit = table.Column<double>(nullable: false),
                    TransactionType = table.Column<int>(nullable: false),
                    TransactionDetails = table.Column<string>(maxLength: 100, nullable: false),
                    Remarks = table.Column<string>(maxLength: 200, nullable: false),
                    PaymentRequestId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblWalletDetailLedger", x => x.Sno);
                });

            migrationBuilder.CreateTable(
                name: "tblCustomerIPFilterDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: true),
                    IPAddress = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCustomerIPFilterDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblCustomerIPFilterDetails_tblCustomerIPFilter_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "tblCustomerIPFilter",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblCustomerIPFilterDetails_CustomerId",
                table: "tblCustomerIPFilterDetails",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblCustomerIPFilterDetails");

            migrationBuilder.DropTable(
                name: "tblCustomerMarkup");

            migrationBuilder.DropTable(
                name: "tblCustomerNotification");

            migrationBuilder.DropTable(
                name: "tblCustomerWalletAmount");

            migrationBuilder.DropTable(
                name: "tblPaymentRequest");

            migrationBuilder.DropTable(
                name: "tblWalletBalanceAlert");

            migrationBuilder.DropTable(
                name: "tblWalletDetailLedger");

            migrationBuilder.DropTable(
                name: "tblCustomerIPFilter");
        }
    }
}

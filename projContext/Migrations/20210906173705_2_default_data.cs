using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace projContext.Migrations
{
    public partial class _2_default_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "tbl_app_setting",
                columns: new[] { "pkid_setting", "AppSettingKey", "AppSettingKeyDisplay", "AppSettingValue", "created_by", "created_dt", "is_active", "last_modified_by", "last_modified_date" },
                values: new object[,]
                {
                    { 1, "attandance_application_freezed_for_Emp", "EMPLOYEE ATTENDENCE FREEZE", "false", 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "attandance_application_freezed__for_Admin", "ADMIN ATTENDENCE FREEZE", "false", 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "tbl_bank_master",
                columns: new[] { "bank_id", "bank_name", "bank_status", "created_by", "created_dt", "deleted_by", "is_deleted", "modified_by", "modified_dt" },
                values: new object[,]
                {
                    { 22, "Indian Overseas Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, "Punjab National Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, "State Bank of India", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 25, "State Bank of Bikaner & Jaipur", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 26, "State Bank of Mysore", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 27, "State Bank of Hyderabad", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 28, "UCO Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 29, "United Bank of India", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 30, "Vijaya Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 31, "Private Banks in Delhi NCR:-", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 32, "HDFC Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 34, "IDBI Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 35, "Axis Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 36, "Syndicate Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 37, "Lord Krishna Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 38, "IndusInd Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, "Dena Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, "Central Bank of India", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 33, "ICICI Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, "Bank of Maharashtra", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, "Canara Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1, "BankName", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "American Express", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "ANZ Grindlays", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Bank of Nova Scotia", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Bank of Tokyo", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Banque Nationale de Paris", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "Citibank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Bank of America", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "Deutsche bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, "Hong Kong & Shanghai", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, "Standard Chartered", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, "Societe Generale", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, "Sanwa Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, "Allahabad Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, "Andhra Bank", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "Credit Lyonnais", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, "Bank of Baroda", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "tbl_component_master",
                columns: new[] { "component_id", "System_function", "System_table", "component_name", "component_type", "created_by", "created_dt", "datatype", "defaultvalue", "is_active", "is_data_entry_comp", "is_fnf_component", "is_payslip", "is_salary_comp", "is_system_key", "is_tds_comp", "is_user_interface", "modified_by", "modified_dt", "parentid", "payment_type", "property_details" },
                values: new object[,]
                {
                    { 545, "fncNoticePaymentDay_sys", null, "@NoticePaymentDay", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)1, (byte)0, (byte)1, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000, 0, "NoticePaymentDay" },
                    { 546, "fncNoticeRecoveryDay_sys", null, "@NoticeRecoveryDay", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)1, (byte)0, (byte)1, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000, 0, "NoticeRecoveryDay" },
                    { 2001, "", null, "@BasicSalary", 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)1, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000, 0, "Basic" },
                    { 2002, "", null, "@HRA", 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)1, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000, 0, "HRA" },
                    { 2003, "", null, "@Conveyance", 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)1, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000, 0, "Conveyance" },
                    { 2007, "", null, "@Medical_Allowance", 4, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000, 0, "Medical Allowance" },
                    { 2005, "", null, "@Bonus", 4, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000, 0, "Bonus" },
                    { 2006, "", null, "@OT", 4, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000, 0, "OT" },
                    { 2008, "", null, "@ChildrenEducationAllowance", 4, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000, 0, "Children Education Allowance" },
                    { 544, "", null, "@NetPaidDays", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000, 0, "Net Paid Days" },
                    { 2009, "", null, "@CityCompensatoryAllowances", 4, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000, 0, "CityCompensatoryAllowances" },
                    { 2004, "", null, "@SPL", 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)1, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000, 0, "Special Allowance" },
                    { 543, "fncAddtitionalPaidDay_sys", null, "@AddtitionalPaidDay", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)1, (byte)0, (byte)1, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000, 0, "Addtitional Paid Day" },
                    { 531, "fncOTHour_sys", null, "@OTHour_sys", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "OT Hour System" },
                    { 541, "fncELEncasment_sys", null, "@ELEncashmentDay", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)1, (byte)0, (byte)1, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000, 0, "EL Encashment Day" },
                    { 540, "", null, "@PfApplicableManual", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)0, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "PF Applicable Flag manual" },
                    { 537, "", null, "@TotalGross", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Total Gross" },
                    { 536, "", null, "@CTC", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)1, (byte)1, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "CTC" },
                    { 535, "fncEMI_sys", null, "@Advance_Loan_sys", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Advance Sys" },
                    { 534, "fncIncomeTax_sys", null, "@Tax_sys", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Tax Sys" },
                    { 533, "", null, "@OTHour", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "OT Hour" },
                    { 532, "", null, "@OTRate", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "OT Rate" },
                    { 530, "fncOTRate_sys", null, "@OTRate_sys", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "OT Rate System" },
                    { 529, "", null, "@GrossSalary", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)1, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Gross Salary" },
                    { 528, "fncSalary_sys", null, "@GrossSalary_sys", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Gross Salary Sys" },
                    { 527, "fncEmpSalaryGroupName_sys", null, "@EmpSalaryGroupName", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Salary Group" },
                    { 2010, "", null, "@CompoffEncashment", 4, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000, 0, "Compoff Encashment" },
                    { 542, "fncComp_off_Encasment_sys", null, "@CompOffEncashmentDay", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)1, (byte)0, (byte)1, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000, 0, "CompOff Encashment Day" },
                    { 2011, "", null, "@ImprestPayable", 4, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000, 0, "Imprest Payable" },
                    { 4004, "", null, "@EPF_Administration_charges_amount", 6, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4000, 0, "ER AdminCharge EPF" },
                    { 2013, "", null, "@Leen", 4, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000, 0, "Leen" },
                    { 5007, "", null, "@Notice_Period_Recovery", 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Notice Period Recovery" },
                    { 5006, "", null, "@Mobile_Phone_Recovery", 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Mobile Phone Recovery" },
                    { 5005, "", null, "@Imprest_Deductions", 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Imprest Deductions" },
                    { 5004, "", null, "@Other_Allowance", 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Other Allowance" },
                    { 5003, "", null, "@Overtime", 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Overtime" },
                    { 5002, "", null, "@Salary_Arrear", 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Salary Arrear" },
                    { 5001, "", null, "@TDS", 2, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "TDS" },
                    { 5000, "", null, "@LWF", 2, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "LWF" },
                    { 4005, "", null, "@EDLIS_Administration_charges_amount", 6, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4000, 0, "ER AdminCharge EDLIS" },
                    { 526, "fncSalaryGroupId_sys", null, "@EmpSalaryGroupId", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Salary Group Id" },
                    { 4003, "", null, "@Employer_EDLIS_amount", 6, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4000, 0, "ER EDLIS" },
                    { 4002, "", null, "@Employer_Pension_Scheme_amount", 6, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4000, 0, "ER EPS" },
                    { 4001, "", null, "@Employer_pf_amount", 6, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)1, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4000, 0, "ER PF" },
                    { 3999, "", null, "@Deduction", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)1, (byte)1, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Total Deduction" },
                    { 3012, "", null, "@Cess", 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3000, 0, "Cess" },
                    { 3011, "", null, "@Surcharge", 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3000, 0, "Surcharge" },
                    { 3010, "", null, "@PT", 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3000, 0, "PT" },
                    { 3008, "", null, "@Recovery", 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3000, 0, "Recovery" },
                    { 3007, "", null, "@ProductPurchase", 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3000, 0, "Product Purchase" },
                    { 3006, "", null, "@Vpf_amount", 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)1, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3000, 0, "VPF" },
                    { 3005, "", null, "@Pf_amount", 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)1, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3000, 0, "PF" },
                    { 3004, "", null, "@EmployerEsicAmount", 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3000, 0, "ER ESIC" },
                    { 3003, "", null, "@EsicAmount", 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)1, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3000, 0, "ESIC" },
                    { 3002, "", null, "@Advance_Loan", 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3000, 0, "Advance" },
                    { 3001, "", null, "@Tax", 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3000, 0, "Tax" },
                    { 2999, "", null, "@Net", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)1, (byte)1, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Net" },
                    { 2014, "", null, "@DA", 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "30", "0", 1, (byte)1, 0, (byte)1, (byte)1, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000, 0, "DA" },
                    { 2012, "", null, "@Gratuity", 4, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2000, 0, "Gratuity" },
                    { 525, "fncEDLIS_Administration_charges_percentage_sys", null, "@EDLIS_Administration_charges_percentage", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Admin EDLIS per" },
                    { 3009, "", null, "@OtherDeduction", 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3000, 0, "Other Deduction" },
                    { 523, "fncEmployer_EDLIS_percetage_sys", null, "@Employer_EDLIS_percetage", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "EDLIS Per" },
                    { 122, "fncAdharName_sys", null, "@AdharName", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Adhar Name" },
                    { 120, "fncPanName_sys", null, "@PanName", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Pan Name" },
                    { 119, "fncPanNo_sys", null, "@PanNo", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Pan" },
                    { 118, "fncEmpWorkingState_sys", null, "@EmpWorkingState", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "WorkingState" },
                    { 117, "fncEmpLocation_sys", null, "@EmpLocation", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Location" },
                    { 116, "fncEmpDepartment_sys", null, "@EmpDepartment", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Department" },
                    { 115, "fncEmpDesignation_sys", null, "@EmpDesignation", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Designation" },
                    { 114, "fncEMPGradeName_sys", null, "@EmpGrade", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Grade" },
                    { 113, "fncEmpContact_sys", null, "@EmpContact", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "ContactNo" },
                    { 524, "fncEPF_Administration_charges_percentage_sys", null, "@EPF_Administration_charges_percentage", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Admin Epf per" },
                    { 112, "fncEducationLevel_sys", null, "@EducationLevel", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Education Level" },
                    { 111, "fncNationality_sys", null, "@EmpNationality", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Nationality" },
                    { 110, "fncEmpEmailId_sys", null, "@EmpEmail", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Email" },
                    { 109, "fncEmpDOB_sys", null, "@EmpDOB", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "4", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "DOB" },
                    { 108, "fncGender_sys", null, "@Gender", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Gender" },
                    { 107, "fncEmpStatus_sys", null, "@EmpStatus", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Emp Status" },
                    { 106, "fncEmpJoiningDt_sys", null, "@EmpJoiningDt", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "4", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Joining Dt" }
                });

            migrationBuilder.InsertData(
                table: "tbl_component_master",
                columns: new[] { "component_id", "System_function", "System_table", "component_name", "component_type", "created_by", "created_dt", "datatype", "defaultvalue", "is_active", "is_data_entry_comp", "is_fnf_component", "is_payslip", "is_salary_comp", "is_system_key", "is_tds_comp", "is_user_interface", "modified_by", "modified_dt", "parentid", "payment_type", "property_details" },
                values: new object[,]
                {
                    { 105, "fncEmpFatherHusbandName_sys", null, "@EmpFatherHusbandName", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Father/Husband Name" },
                    { 104, "fncEmpName_sys", null, "@EmpName", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Emp Name" },
                    { 103, "fncEMP_Code_sys", null, "@EmpCode", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Emp Code" },
                    { 102, "fncCompanyLogoPath_sys", null, "@CompanyLogoPath", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Company Logo" },
                    { 101, "fncCompanyName_sys", null, "@CompanyName", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Company Name" },
                    { 100, "fncCompanyId_sys", null, "@CompanyId", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Company Id" },
                    { 3000, "", null, "@EmployeeDeduction", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000, 0, "Employee Deduction" },
                    { 2000, "", null, "@EmployeeIncome", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1000, 0, "Employee Income" },
                    { 1000, "", null, "@EmployeeSalary", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "-", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 0, "Employee Salary" },
                    { 1, "", null, "@SystemComponent", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "-", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 0, "System Component" },
                    { 201, "fncBankAccountNo_sys", null, "@BankAccountNo", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Account No" },
                    { 202, "fncIFSCCode_sys", null, "@IFSCCode", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "IFSC" },
                    { 121, "fncAdharNo_sys", null, "@AdharNo", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Adhar" },
                    { 204, "fncEsicAppliable_sys", null, "@EsicApplicable", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "ESIC flag" },
                    { 522, "fncEmployer_Pension_Scheme_percentage_sys", null, "@Employer_Pension_Scheme_percentage", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "EPS Per" },
                    { 521, "fncEmployer_pf_percentage_sys", null, "@Employer_pf_percentage", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Employer Pf Per" },
                    { 520, "fncPFCeling_sys", null, "@pf_celling", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "PF ceiling" },
                    { 519, "fncVpfPercentage_sys", null, "@vpf_percentage", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "VPF Per" },
                    { 518, "fncVPFApplicableyesNo_sys", null, "@Vpf_applicableYesNo", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "VPF Applicable" },
                    { 517, "fncVPFApplicable_sys", null, "@Vpf_applicable", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "VPF Applicable Flag" },
                    { 516, "fncPfPercentage_sys", null, "@PfPercentage", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "PF Per" },
                    { 515, "fncPfSalarySlab_sys", null, "@PfSalarySlab", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "PF ceiling" },
                    { 514, "fncVPFGroup_sys", null, "@VPfGroup", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "VPF Group" },
                    { 513, "fncPFGroupid_sys", null, "@PfGroup", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "PF Group" },
                    { 512, "fncPfNo_sys", null, "@PfNo", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "PF No" },
                    { 203, "fncBankName_sys", null, "@BankName", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Bank" },
                    { 510, "fncPFApplicableyesNo_sys", null, "@PfApplicableYesNo", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "PF Applicable" },
                    { 509, "fncEPSApplicable_sys", null, "@EpsApplicable", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "EPS Applicable Flag" },
                    { 511, "fncEPSApplicableyesNo_sys", null, "@EpsApplicableYesNo", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "EPS Applicable" },
                    { 507, "fncUanNo_sys", null, "@Uan", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "UAN" },
                    { 205, "fncEsicNo_sys", null, "@ESICNo", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "ESIC No" },
                    { 206, "fncEsicAppliableName_sys", null, "@EsicApplicableYesNo", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "ESIC" },
                    { 508, "fncPFApplicable_sys", null, "@PfApplicable", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "PF Applicable Flag" },
                    { 208, "fncEmployerEsicPercentage_sys", null, "@EmployerEsicPercentage", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "ESIC Employer Per" },
                    { 210, "", null, "@EsicApplicableManual", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "1", 1, (byte)0, 0, (byte)0, (byte)0, (byte)0, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "ESIC flag manual" },
                    { 207, "fncEsicPercentage_sys", null, "@EsicPercentage", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "ESIC Per" },
                    { 502, "fncLOD_sys", null, "@LodSys", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "LOD Sys" },
                    { 503, "", null, "@LopDays", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "LOP" },
                    { 504, "", null, "@PaidDays", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)0, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Paid Day" },
                    { 505, "", null, "@ArrearDays", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Arrear Day" },
                    { 506, "", null, "@ArrearMonthYear", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "0", 1, (byte)1, 0, (byte)1, (byte)0, (byte)0, (byte)0, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Arrear YearMonth" },
                    { 501, "fncToalPayrollDay_sys", null, "@ToalPayrollDay", 3, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2", "30", 1, (byte)0, 0, (byte)0, (byte)0, (byte)1, (byte)0, 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 0, "Total Payroll Day" }
                });

            migrationBuilder.InsertData(
                table: "tbl_country",
                columns: new[] { "country_id", "created_by", "created_date", "is_deleted", "last_modified_by", "last_modified_date", "name", "sort_name" },
                values: new object[] { 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "India", "IN" });

            migrationBuilder.InsertData(
                table: "tbl_department_master",
                columns: new[] { "department_id", "company_id", "created_by", "created_date", "department_code", "department_head_employee_code", "department_head_employee_name", "department_name", "department_short_name", "employee_id", "is_active", "last_modified_by", "last_modified_date" },
                values: new object[,]
                {
                    { 9, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IT-SW", "", "", "IT Software", "IT Software", null, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Exchange", "", "", "Exchange", "Exchange", null, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Support", "", "", "Support", "Support", null, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer Services", "", "", "Customer Services", "Customer Services", null, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Accounts", "", "", "Accounts", "Accounts", null, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IT Infra", "", "", "IT Infra", "IT Infra", null, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marketing", "", "", "Marketing", "Marketing", null, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Purchase", "", "", "Purchase", "Purchase", null, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operations", "", "", "Operations", "Operations", null, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sales", "", "", "Sales", "Sales", null, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR", "", "", "Human Resource", "HR", null, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finance", "", "", "Finance", "Finance", null, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "", "", "Admin", "Admin", null, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "tbl_designation_master",
                columns: new[] { "designation_id", "created_by", "created_date", "designation_name", "is_active", "last_modified_by", "last_modified_date" },
                values: new object[,]
                {
                    { 153, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "QC Chemist", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 152, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "QC Assistant", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 151, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "QA Chemist", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 150, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Project Manager - CSR", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 147, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Production Supervisor", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 148, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Project Coordinator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 154, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "QC Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 146, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Production Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 145, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product Trainer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 144, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Procurement Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 149, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Project Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 155, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "QC Officer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 158, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "R&D Chemist", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 157, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quality Auditor", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 159, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Raw material and Personal ingerient care executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 160, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Regional Executive Trainer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 161, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Relationship Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 162, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sales Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 163, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SCM Coordinator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 164, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Section Head- CRD & DST", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 165, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Section Head- Operations", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 166, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Security Guard", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 167, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Agronomist", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 168, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Audit Officer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 156, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quality Analyst", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 143, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Plant HR", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 140, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Packaging Supervisor", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 141, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Packing Assistant", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 169, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Executive- Finance & Accounts", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 117, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 118, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manager - Legal & Company Secretary", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 119, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manager-Finance & Accounts", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 120, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Managing Director", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 121, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manufacturing Operator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 122, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marketing Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 123, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mfg. Chemist", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 124, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "MIS Data Analyst", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 125, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "MIS Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 126, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "MIS Senior Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 127, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mobile App. Developer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 128, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "National Head", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 129, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "National Head-Sales & Training", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 130, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Network Security Engineer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 131, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "NRD Coordinator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 132, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Office Boy", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 133, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operation Assistant", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 134, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator Electrical", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 116, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maintenance Technician", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 135, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator Filling", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 136, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator Labelling", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 137, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator Manufacturing", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 138, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator Mechanical", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 139, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Packaging Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 142, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pantry Boy", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 170, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Executive- Operations", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 207, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Talent Acquisition Specialist", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 172, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Executive- Warehouse", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 202, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Store Supervisor", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 203, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sub Editor", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 204, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Supervisor-Combo", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 205, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Supervisor-Packaging", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 206, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System Administrator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 208, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tally Administrator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 209, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Team Coordinator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 210, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Team Leader", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 211, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Technical Head", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 212, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "TECHNOLOGY OFFICER", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 213, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trainee Electrician", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 201, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Store Officer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 214, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trainee Engineer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 216, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tube and Jar Filling Operator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 217, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "UI Developer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 218, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Utility & Maintenance Head", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 219, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Utility Operator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 220, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "VCM Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 221, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Video Editor", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 222, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warehouse Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 223, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warehouse Incharge", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 224, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warehouse Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 225, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Web Designer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 115, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Labelling Machine Operator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 215, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Training Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 200, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Store Incharge", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 199, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Store Helper", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 198, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Store Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 173, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Executive-Distributor Record", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 174, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Executive-MIS", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 175, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Executive-SCM", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 176, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior General Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 177, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 178, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Manager - Finance & Accounts", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 179, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Manager Purchase and Procurement", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 180, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Manager-SCM", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 181, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Operation Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 182, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior R&D Chemist", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 183, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Software Developer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 184, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Visualizer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 185, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SEO-Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 186, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Software Developer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 187, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Software Tester", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 188, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Assistant Supervisor", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 189, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Assistant-Operation", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 190, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 191, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Executive - Procurement & Packaging", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 192, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Executive -Procurement RM & CM Audit", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 193, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 194, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Supervisor-Administration", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 195, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Supervisor-Inward", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 196, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Store Assistant", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 197, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Store Attendent", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 171, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Executive Warehouse", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 114, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Labelling and Compression Operator and  Bottle Filling", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 83, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-MIS", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 112, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L2 Support Engineer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 29, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Store", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 30, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Utility & Maintenance", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 31, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Warehouse", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 32, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Supervisor", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 33, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Supervisor-Administration (Security and Labour)", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 34, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Team Leader", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 35, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Associate Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 36, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Associate Executive - Warehouse", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 37, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Associate Executive Warehouse", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 38, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Audit Officer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 39, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BDT Coordinator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 28, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-SCM", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 40, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Billing", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 42, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Business Analyst", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 43, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Business Analyst- Senior Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 44, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Business Data Analyst", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 45, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Business Development Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 46, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 47, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chief Executive Officer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 48, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Combo Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 49, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Complaint Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 50, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compliance Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 52, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Content Writer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 53, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer Care Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 41, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bottle and Jar Filling Operator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 27, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-QC", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 26, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-QA", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 25, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Production", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "tbl_designation_master",
                columns: new[] { "designation_id", "created_by", "created_date", "designation_name", "is_active", "last_modified_by", "last_modified_date" },
                values: new object[,]
                {
                    { 113, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lab Assistant", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Account Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin Incharge", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin Supervisor", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Agronomist", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AM Warehouse account operation", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Application Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Application Manager  Android App Developer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Application Support Engineer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Area Supervisor", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Asistant Team Leader", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Executive-Commercial", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant General Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager - QC", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager SCM", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Accounts", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Administration", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Distributor Record", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Event", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Finance & Accounts", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Human Resource", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Legal", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Operations", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 54, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Data Entry Operator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 55, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deputy Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 51, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Consultant", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 57, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deputy Manager Warehouse", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 87, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Factory Head", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 56, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deputy Manager - Product Development (Nutrition & Wellness)", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 89, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Field Boy", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 90, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Field Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 91, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Filling Operator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 92, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fork lift Operator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 93, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Front Office Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 94, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "General Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 95, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "General Manager SCM Procurement And Marketing", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 96, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Graphics Designer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 97, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Head BD", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 98, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Head BT", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 99, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Head NR", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 100, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Helper", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 101, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Housekeeping Supervisor", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 102, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR Generalist", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 103, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR Operations Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 104, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR Supervisior", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 105, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR Supervisor", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 106, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR Trainee", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 107, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HVAC Trainee Operator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 108, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Internal Auditor", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 109, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inward  Combo Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 110, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inward Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 111, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Joining and  Billing", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 86, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-Warehouse", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 85, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-SCM", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 88, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Factory Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 65, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Director", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 58, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deputy Manager-Administration", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 59, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deputy Manager-Finance & Accounts", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 60, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deputy Manager-Legal", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 61, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deputy Manager-SCM", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 62, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Desktop Support Engineer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 63, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Digital Marketing Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 64, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Digital Marketing Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 84, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-Operations", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 66, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Director- Galway Kart", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 67, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dispatch  Senior Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 69, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Driver", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 70, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "E Commerce Warehouse Incharge", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 68, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dispatch Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 72, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Event Coordinator", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 82, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-Legal", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 81, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-Finance & Accounts", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 80, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-Distributor Support", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 71, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Editor", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 78, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-Administration", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 79, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-Distributor Record", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 76, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive Assistant-Procurement & Packaging", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 75, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive Assistant", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 74, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive - Warehouse", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 73, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive - Distributor Support", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 77, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive Social Media", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "tbl_employee_master",
                columns: new[] { "employee_id", "created_by", "created_date", "emp_code", "is_active", "last_modified_by", "last_modified_date" },
                values: new object[] { 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "tbl_grade_master",
                columns: new[] { "grade_id", "created_by", "created_date", "grade_name", "is_active", "last_modified_by", "last_modified_date" },
                values: new object[,]
                {
                    { 14, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Assistant", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Managing Director", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chief Executive Officer", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior General Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Field Boy", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "General Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trainee", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Supervisor", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant General Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deputy Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Director", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Associate Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "NA", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Manager", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "tbl_leave_type",
                columns: new[] { "leave_type_id", "_is_el", "created_by", "created_date", "description", "is_active", "last_modified_by", "last_modified_date", "leave_type_name" },
                values: new object[,]
                {
                    { 8, null, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Leave Without Pay", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "LWP" },
                    { 7, null, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bereavement Leave", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BL" },
                    { 6, null, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paternity Leave", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ptl" },
                    { 5, null, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maternity Leave", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "MtL" },
                    { 3, null, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Short leave Leave", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SHL" },
                    { 2, null, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Earned Leave", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EL" },
                    { 1, null, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Casual Leave", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CL" },
                    { 4, null, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compensatory Off", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Comp Off" }
                });

            migrationBuilder.InsertData(
                table: "tbl_menu_master",
                columns: new[] { "menu_id", "IconUrl", "SortingOrder", "created_by", "created_date", "is_active", "menu_name", "modified_by", "modified_date", "parent_menu_id", "type", "urll" },
                values: new object[,]
                {
                    { 1410, "fa fa-rocket", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Calender", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, (byte)1, "view/Dashboard" },
                    { 1405, "fa fa-list-alt", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Shift Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, (byte)1, "Shift/View" },
                    { 1406, "fa fa-list-alt", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Emp Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, (byte)1, "#" },
                    { 1407, "fa fa-user-clock", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Employee", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1406, (byte)1, "Employee/ViewEmployee" },
                    { 1408, "fa fa-user-clock", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Employee Shift", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1406, (byte)1, "Shift/ShiftAllignmentDetail" },
                    { 1404, "fa fa-book-open", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Location Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1402, (byte)1, "Masters/DetailLocationMaster" },
                    { 1409, "fa fa-list-alt", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Attendance Reports", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, (byte)1, "#" },
                    { 1411, "fa fa-rocket", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Application Reports", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, (byte)1, "#" },
                    { 1419, "fa fa-rocket", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Mispunch Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, (byte)1, "Report/Mispunch" },
                    { 1413, "fa fa-rocket", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Attendance Application Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1411, (byte)1, "view/AttendenceApplicationReport" },
                    { 1414, "fa fa-rocket", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Compoff Credit Application Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1411, (byte)1, "view/CompoffRaiseReport" },
                    { 1415, "fa fa-rocket", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Compoff Application Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1411, (byte)1, "view/CompOffApplicationReport" },
                    { 1416, "fa fa-rocket", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Attendance Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, (byte)1, "Report/Attendance" },
                    { 1417, "fa fa-rocket", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Absent Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, (byte)1, "Report/Absent" },
                    { 1418, "fa fa-rocket", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Present Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, (byte)1, "Report/Present" },
                    { 1403, "fa fa-gopuram", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Company Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1402, (byte)1, "CompanyMaster/View" },
                    { 1420, "fa fa-rocket", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manul Punch", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, (byte)1, "Report/ManualPunch" },
                    { 1412, "fa fa-rocket", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Outdoor Application Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1411, (byte)1, "view/OutdoorApplicationReport" },
                    { 1402, "fa fa-list-alt", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Organization Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, (byte)1, "#" },
                    { 1311, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "KRA Creation", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, (byte)1, "epa/KRACreationMaster" },
                    { 1317, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "EPA Submission", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1301, (byte)1, "EPA/EpaSubmissionForm" },
                    { 1115, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Account Details", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1113, (byte)1, "Employee/AccountDetails" },
                    { 1201, "fa fa-file", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Upload", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "Employee/BulkUpload" },
                    { 1301, "fa fa-file", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "EPA", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "#" },
                    { 1302, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "WrokingRole", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1301, (byte)1, "Employee/WorkingRoleAllocation" },
                    { 1303, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "EPA Setting", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1301, (byte)1, "#" },
                    { 1304, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Fiscal Year", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, (byte)1, "epa/FinancialYear" },
                    { 1305, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "EPA Cycle", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, (byte)1, "epa/epa_cycle" },
                    { 1306, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Working Role", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, (byte)1, "epa/ePAWorkingrole" },
                    { 1401, "fas fa-chart-pie", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "#" },
                    { 1307, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "KPI Objective", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, (byte)1, "epa/KPIObjective" },
                    { 1309, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "KRA Rating Master", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, (byte)1, "epa/KRA_Rating_Master" },
                    { 1310, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "KPI Key Area Master", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, (byte)1, "epa/KpiKeyAreaMaster" },
                    { 1421, "fa fa-rocket", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Late Punch In", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, (byte)1, "Report/LatePunchIn" },
                    { 1312, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "EPA Status Master", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, (byte)1, "epa/StatusMaster" },
                    { 1313, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "EPA Status Flow", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, (byte)1, "epa/StatusFlowMaster" },
                    { 1314, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "EPA Tab Master", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, (byte)1, "epa/TabMaster" },
                    { 1315, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "EPA Question Master", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, (byte)1, "epa/QuestionMaster" },
                    { 1316, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "EPA Allignment", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, (byte)1, "epa/WorkingRoleComponent" },
                    { 1308, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "KPI Criteria", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, (byte)1, "epa/KpiCriteria" },
                    { 1422, "fa fa-rocket", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Early Punch out", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, (byte)1, "Report/EarlyPunchOut" },
                    { 1431, "fa fa-diamond", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Advance Form-III", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1428, (byte)1, "payroll/RegisterofAdvanceFormIIIMaster" },
                    { 1424, "fa fa-list-alt", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Leave Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, (byte)1, "#" },
                    { 1503, "fa fa-cog", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Role Allocation", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1501, (byte)1, "Masters/assignrolemenu" },
                    { 1601, "fa fa-cog", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Emp Sepration", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "#" },
                    { 1602, "fa fa-cog", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Resignation Approval", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1601, (byte)1, "View/EmpSeprationApproval" },
                    { 1603, "fa fa-pie-chart", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Resignation Cancellation", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, (byte)1, "View/EmpSeprationDetail" },
                    { 1607, "fa fa-paypal", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Employee Withdrawal", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 701, (byte)1, "Employee/Withdrawal" },
                    { 1608, "fa fa-paypal", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Employee FNF", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1601, (byte)1, "" },
                    { 1609, "fa fa-paypal", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "FNF Process", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1608, (byte)1, "Payroll/EmployeeFNF" },
                    { 1610, "far fa-file-image", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Documents Type", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, (byte)1, "Masters/DocumentType" },
                    { 1502, "fa fa-cog", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Loan Approval Setting", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1501, (byte)1, "Payroll/LoanApprovalSettingMaster" },
                    { 1611, "fa fa-user", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Employee Dump", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "Employee/EmployeeDump" },
                    { 1613, "fa fa-user", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Monthly Summary Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "Report/MonthlySummaryReport" },
                    { 1614, "fa fa-user", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Attendance Monthly Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "Report/AttendanceMonthlyReport" },
                    { 1615, "fa fa-user", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Attendence Summary Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "Report/AttendenceSummaryReport" },
                    { 1616, "fa fa-user", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Attendance In Out Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "Report/AttendanceInOutReport" },
                    { 1641, "fa fa-user", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "KT Task", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "Employee/Save_KT_Task" },
                    { 1642, "fa fa-user", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "KT Task Status", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "Employee/Save_KT_Task" },
                    { 1644, "fa fa-user", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "KT Task Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "Employee/Save_KT_Task" },
                    { 1114, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Provident Fund", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1113, (byte)1, "Employee/UANDetails" },
                    { 1612, "fa fa-user", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Leave Adjust Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "Report/LeaveAdjustReport" },
                    { 1423, "fa fa-rocket", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Location Punch Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, (byte)1, "Report/LocationBy" },
                    { 1501, "fa fas fa-cog", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Setting", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "#" },
                    { 1444, "fa fa-list-alt", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Leave Ledeger", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1424, (byte)1, "Leave/MannualLeaveReport" },
                    { 1425, "fa fa-list-alt", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Leave Balance", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1424, (byte)1, "Leave/LeaveBalance" },
                    { 1426, "fa fa-list-alt", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Leave Application Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1424, (byte)1, "view/LeaveApplicationReport" },
                    { 1427, "fa fa-list-alt", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Payroll Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, (byte)1, "#" },
                    { 1428, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Muster Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1427, (byte)1, "#" },
                    { 1429, "fa fa-diamond", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Fine Master Form-I", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1428, (byte)1, "payroll/RegisterOfFinesFormIMaster" },
                    { 1430, "fa fa-diamond", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Deduction Form-II", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1428, (byte)1, "payroll/RegisterOfDeductionsFormIIMaster" },
                    { 1432, "fa fa-diamond", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Overtime Form -IV", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1428, (byte)1, "payroll/RegisterofOvertimeFormIVMaster" },
                    { 1433, "fa fa-diamond", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Minimum Wages Central Rules FORM-V", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1428, (byte)1, "payroll/MinimumWagesCentralRulesFORMV" },
                    { 1445, "fa fa-cog", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Resignation Application", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1601, (byte)1, "View/EmpSepration" },
                    { 1434, "fa fa-diamond", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Register of Wages Form-X", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1428, (byte)1, "payroll/RegisterOfWagesFormXMaster" },
                    { 1436, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Salary Slip", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1427, (byte)1, "view/SalarySlips" },
                    { 1437, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "SalaryReport (Arrea)", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1427, (byte)1, "Report/DynamicReport?1" },
                    { 1438, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Salary Report", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1427, (byte)1, "Report/DynamicReport?2" },
                    { 1439, "fa fa-list-alt", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Emp Asset Reports", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, (byte)1, "View/EmpReqAssetReport" },
                    { 1440, "fa fa-list-alt", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "EPAReport", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, (byte)1, "#" },
                    { 1441, "fa fa-tachometer", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "EPA Details", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1440, (byte)1, "epa/TabDetails" },
                    { 1442, "fa fa-bar-chart", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Bar Chart", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1440, (byte)1, "epa/EpaBarchart" },
                    { 1443, "fa fa-pie-chart", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "EPA Graph", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1440, (byte)1, "epa/EPAGraphChart" },
                    { 1435, "fa fa-diamond", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Register of Wages Slip Form XI", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1428, (byte)1, "payroll/RegisterOfWagesSlipFormXIMaster" },
                    { 1113, "fa fa-user-clock", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Saturity", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, (byte)1, "#" },
                    { 1112, "fa fa-user-clock", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Employee Setting", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, (byte)1, "Employee/ActiveInActiveUser" },
                    { 101, "fa fa-building", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Organisation", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "#" },
                    { 504, "fa fa-user-circle-o", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Outdoor Approval", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 501, (byte)1, "Leave/OutdoorLeaveApproval" },
                    { 505, "fa fa-user-circle-o", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Outdoor Cancel", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 501, (byte)1, "Admin/OutdoorLeaveApproval" },
                    { 506, "fa fa-user-circle-o", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Attendance Application", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 501, (byte)1, "view/AttendenceApplication" },
                    { 507, "fa fa-user-circle-o", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Attendance Approval", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 501, (byte)1, "Leave/AttandenceApproval" },
                    { 508, "fa fa-user-circle-o", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Attendance Cancel", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 501, (byte)1, "Admin/AttandenceApproval" },
                    { 509, "fa fa-user-circle-o", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Compoff", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 501, (byte)1, "#" },
                    { 511, "fa fa-futbol-o", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Compoff Credit Approval", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 509, (byte)1, "view/CompoffRaisedApproval" },
                    { 512, "fa fa-futbol-o", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Compoff Credit Cancel", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 509, (byte)1, "Admin/CompoffAdditionApplication" },
                    { 503, "fa fa-user-circle-o", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Outdoor Application", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 501, (byte)1, "view/OutdoorApplication" },
                    { 513, "fa fa-futbol-o", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Compoff Application", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 509, (byte)1, "view/CompOffApplication" },
                    { 515, "fa fa-futbol-o", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Compoff Cancel", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 509, (byte)1, "Admin/CompOffLeaveApproval" },
                    { 601, "fa fa-coffee", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Leave", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "#" },
                    { 602, "fa fa-coffee", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Leave Type", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 601, (byte)1, "Masters/AddLeaveType" },
                    { 603, "fa fa-coffee", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Leave Setting", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 601, (byte)1, "Leave/AddLeaveSetting" },
                    { 604, "fa fa-coffee", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manual Leave", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 601, (byte)1, "leave/mannualleave" },
                    { 605, "fa fa-coffee", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Leave Application", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 601, (byte)1, "View/LeaveApplication" },
                    { 606, "fa fa-coffee", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Leave Approval", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 601, (byte)1, "Leave/LeaveApproval" },
                    { 607, "fa fa-coffee", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Leave Cancel", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 601, (byte)1, "Admin/LeaveApproved" },
                    { 514, "fa fa-futbol-o", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Compoff Approval", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 509, (byte)1, "Leave/CompOffLeaveApproval" },
                    { 701, "fa fa-credit-card", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Payroll", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "#" }
                });

            migrationBuilder.InsertData(
                table: "tbl_menu_master",
                columns: new[] { "menu_id", "IconUrl", "SortingOrder", "created_by", "created_date", "is_active", "menu_name", "modified_by", "modified_date", "parent_menu_id", "type", "urll" },
                values: new object[,]
                {
                    { 502, "fa fa-file-word-o", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manual Compoff", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 501, (byte)1, "Leave/MannualCompoff" },
                    { 404, "fa fa-clipboard-list", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Shift Assignemnt", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 401, (byte)1, "Shift/ShiftAllignment" },
                    { 1, "fa fa-home", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Dashboard", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "Dashboard" },
                    { 1111, "fa fa-user-clock", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Prev Employer", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, (byte)1, "Employee/EmpPreviousCompanyDetail" },
                    { 102, "fa fa-gopuram", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Create Company", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101, (byte)1, "CompanyMaster/Create" },
                    { 103, "fa fa-gopuram", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Create Location", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101, (byte)1, "Masters/AddLocationMaster" },
                    { 201, "fa fa-building", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Master", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "#" },
                    { 202, "fa fa-hospital", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Department", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, (byte)1, "Masters/AddDepartmentMaster" },
                    { 203, "fa fa-wheelchair", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Designation", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, (byte)1, "masters/AddDesignationMaster" },
                    { 204, "fa fa-chalkboard-teacher", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Grade", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, (byte)1, "Masters/AddGradeMaster" },
                    { 501, "fa fa-bars", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Attendance", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "#" },
                    { 205, "fa fa-digital-tachograph", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Machine Master", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, (byte)1, "Masters/MachineMaster" },
                    { 302, "fa fa-map-marker-alt", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Event", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, (byte)1, "Masters/addEvent" },
                    { 303, "fa fa-clipboard-list", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Holiday Master", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, (byte)1, "Masters/CompanyHoliday" },
                    { 304, "fa fa-clipboard-list", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Country", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, (byte)1, "Masters/Addcountry" },
                    { 305, "fa fa-clipboard-list", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "State", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, (byte)1, "Masters/addstate" },
                    { 306, "fa fa-clipboard-list", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "City", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, (byte)1, "Masters/addcity" },
                    { 401, "fa fa-user-clock", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Shift", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "#" },
                    { 402, "fa fa-user-tie", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Create Shift", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 401, (byte)1, "shift/createshift" },
                    { 403, "fa fa-gopuram", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Shift Roster", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 401, (byte)1, "Shift/AddRoasterMaster" },
                    { 301, "fa fa-university", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Bank Master", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, (byte)1, "Masters/BankMasters" },
                    { 702, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Payroll Setting", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 701, (byte)1, "#" },
                    { 510, "fa fa-futbol-o", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Compoff Credit Application", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 509, (byte)1, "view/compoffdetails" },
                    { 704, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Salary Group Alignment", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 702, (byte)1, "payroll/FormulaComponent" },
                    { 904, "fa fa-briefcase", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Config-I", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 902, (byte)1, "Payroll/LoanRequestMaster" },
                    { 905, "fa fa-briefcase", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Loan Request", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 901, (byte)1, "payroll/LoanRequest" },
                    { 906, "fa fa-briefcase", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Loan Approval", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 901, (byte)1, "Payroll/LoanRequestApproval" },
                    { 907, "fa fa-briefcase", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Loan Repayment", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 901, (byte)1, "payroll/LoanRepayments" },
                    { 1001, "fa fa-font", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Asset", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "#" },
                    { 1002, "fa fa-font", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Asset Master", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1001, (byte)1, "Masters/AssetMaster" },
                    { 1003, "fa fa-font", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Asset Request", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1001, (byte)1, "view/AssetRequest" },
                    { 1004, "fa fa-font", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Asset Approval", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1001, (byte)1, "Payroll/AssetRequestApproval" },
                    { 902, "fa fa-briefcase", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Loan Setting", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 901, (byte)1, "#" },
                    { 1101, "fa fa-user", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Employee", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "#" },
                    { 1103, "fa fa-user-clock", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Official Section", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, (byte)1, "Employee/OfficaialSection" },
                    { 703, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Salary Group", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 702, (byte)1, "payroll/SalGroupMaster" },
                    { 1105, "fa fa-user-clock", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Family Details", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, (byte)1, "Employee/FamilySection" },
                    { 1106, "fa fa-user-clock", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Personal Details", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, (byte)1, "Employee/PersonalDetails" },
                    { 1107, "fa fa-user-clock", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Emp Allocation", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, (byte)1, "Employee/Allocation" },
                    { 1108, "fa fa-user-clock", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Emp Helth Card", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, (byte)1, "Employee/HealthCard" },
                    { 1109, "fa fa-user-clock", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Emp Documents", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, (byte)1, "Employee/MaintainDocuments" },
                    { 1110, "fa fa-user-clock", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "EmpStatus", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, (byte)1, "Employee/EmploymentType" },
                    { 1102, "fa fa-user-clock", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Create Employee", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, (byte)1, "Employee/CreateUser" },
                    { 901, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Loan", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "#" },
                    { 1104, "fa fa-user-clock", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Qualification", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, (byte)1, "Employee/QualificationSection" },
                    { 801, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Tax", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, (byte)1, "#" },
                    { 802, "fa fa-gift", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Employee Tax", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 801, (byte)1, "payroll/EmployeeIncomeTaxAmount" },
                    { 705, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Total Payroll Days Setting", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 702, (byte)1, "Payroll/LODSetting" },
                    { 706, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Payroll Cycle Setting", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 702, (byte)1, "payroll/PayrollMonthCircle" },
                    { 707, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "OT Compoff Setting", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 702, (byte)1, "Shift/Settings" },
                    { 708, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "OT Rate", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 702, (byte)1, "payroll/OTRateMaster" },
                    { 709, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Repository", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 702, (byte)0, "payroll/Repository" },
                    { 711, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Emp Salary", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 701, (byte)1, "Payroll/SalaryRevision" },
                    { 712, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "LOP", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 701, (byte)1, "payroll/LODMaster" },
                    { 713, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Payroll Input", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 701, (byte)1, "payroll/SalaryInput" },
                    { 710, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Emp Salary Group", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 701, (byte)1, "Payroll/SalaryGroupMapEmp" },
                    { 715, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Musters", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 701, (byte)1, "#" },
                    { 716, "fa fa-diamond", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Muster Setting", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 715, (byte)1, "#" },
                    { 722, "fa fa-diamond", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Register of Wages Slip Form XI", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 716, (byte)1, "payroll/RegisterOfWagesSlipFormXIMaster" },
                    { 714, "fa fa-money", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Process Payroll", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 701, (byte)1, "payroll/ProcessPayroll" },
                    { 717, "fa fa-diamond", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Fine Master Form-I", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 716, (byte)1, "payroll/RegisterOfFinesFormIMaster" },
                    { 718, "fa fa-diamond", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Deduction Form-II", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 716, (byte)1, "payroll/RegisterOfDeductionsFormIIMaster" },
                    { 719, "fa fa-diamond", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Advance Form-III", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 716, (byte)1, "payroll/RegisterofAdvanceFormIIIMaster" },
                    { 720, "fa fa-diamond", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Overtime Form -IV", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 716, (byte)1, "payroll/RegisterofOvertimeFormIVMaster" },
                    { 721, "fa fa-diamond", 0, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Register of Wages Form-X", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 716, (byte)1, "payroll/RegisterOfWagesFormXMaster" }
                });

            migrationBuilder.InsertData(
                table: "tbl_payroll_month_setting",
                columns: new[] { "payroll_month_setting_id", "applicable_from_date", "applicable_to_date", "company_id", "created_by", "created_date", "from_date", "from_month", "is_deleted", "last_modified_by", "last_modified_date" },
                values: new object[] { 1, 0, 20230331, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 202103, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "tbl_religion_master",
                columns: new[] { "religion_id", "created_by", "created_date", "is_active", "last_modified_by", "last_modified_date", "religion_name" },
                values: new object[,]
                {
                    { 17, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Zoroastrianism" },
                    { 16, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unitarian-Universalism" },
                    { 15, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tenrikyo" },
                    { 100, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Other" },
                    { 14, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spiritism" },
                    { 13, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cao Dai" },
                    { 12, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sikhism" },
                    { 11, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shinto" },
                    { 10, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Neo-Paganism " },
                    { 4, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chinese" },
                    { 8, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Juche" },
                    { 7, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Islam" },
                    { 6, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hinduism" },
                    { 5, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Christianity" },
                    { 3, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cao Dai" },
                    { 2, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Buddhism" },
                    { 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bahai " },
                    { 9, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Judaism" }
                });

            migrationBuilder.InsertData(
                table: "tbl_report_master",
                columns: new[] { "rpt_id", "created_by", "created_date", "is_active", "last_modified_by", "last_modified_date", "rpt_description", "rpt_name" },
                values: new object[,]
                {
                    { 2, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "salary Report", "Salary Report" },
                    { 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "salary Report with arrear", "Salary Report(Arrear)" }
                });

            migrationBuilder.InsertData(
                table: "tbl_role_master",
                columns: new[] { "role_id", "created_by", "created_date", "is_active", "last_modified_by", "last_modified_date", "role_name" },
                values: new object[,]
                {
                    { 107, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "NoDuesAdmin" },
                    { 106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PayrollAdmin" },
                    { 105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AttendanceAdmin" },
                    { 104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EmployeeMasterAdmin" },
                    { 103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "MastersAdmin" },
                    { 102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "FinanceAdmin" },
                    { 101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HRAdmin" },
                    { 23, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trainee" },
                    { 12, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SectionHead" },
                    { 21, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Employee" },
                    { 15, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Consultant" },
                    { 14, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Management" },
                    { 13, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "TeamLeader" },
                    { 22, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Intern" },
                    { 11, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manager" },
                    { 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SuperAdmin" }
                });

            migrationBuilder.InsertData(
                table: "tbl_role_menu_master",
                columns: new[] { "role_menu_id", "created_by", "created_date", "menu_id", "modified_by", "modified_date", "role_id" },
                values: new object[,]
                {
                    { 141300011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1413, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 141300012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1413, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 141300021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1413, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 141300013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1413, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 141400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1414, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 141400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1414, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 141400105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1414, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 141400011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1414, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 141400012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1414, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 141400013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1414, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 141600101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1416, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 141300105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1413, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 141500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1415, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 141500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1415, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 141500105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1415, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 141500011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1415, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 141500012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1415, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 141500021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1415, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 141500013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1415, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 141600001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1416, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 141400021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1414, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 141300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1413, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 141200012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1412, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 141200013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1412, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 140900106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 140900011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 140900012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 140900013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 140900021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 141000001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1410, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 141000101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1410, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 141000105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1410, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 141000012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1410, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 141100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1411, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 141100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1411, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 141100105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1411, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 141100011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1411, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 141100012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1411, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 141100021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1411, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 141100013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1411, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 141200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1412, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 141200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1412, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 141200105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1412, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 141200011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1412, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 141200021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1412, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 141300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1413, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 141600105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1416, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 130100013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1301, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 141600011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1416, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 142000013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1420, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 142100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1421, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 142100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1421, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 142100105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1421, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 142100011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1421, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 142100012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1421, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 142100013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1421, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 142200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1422, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 142200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1422, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 142200105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1422, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 142200011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1422, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 142200012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1422, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 142200013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1422, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 142300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1423, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 142300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1423, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 142300105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1423, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 142300011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1423, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 142300012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1423, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 142300013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1423, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 142400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1424, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 140900105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 142000012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1420, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 141600106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1416, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 142000011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1420, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 142000101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1420, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 141600012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1416, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 141600013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1416, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 141700001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1417, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 141700101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1417, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 141700105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1417, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 141700011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1417, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 141700012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1417, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 141700013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1417, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 141800001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1418, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 141800101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1418, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 141800105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1418, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 141800011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1418, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 141800012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1418, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 141800013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1418, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 141900001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1419, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 141900101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1419, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 141900105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1419, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 141900011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1419, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 141900012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1419, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 141900013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1419, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 142000001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1420, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 142000105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1420, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 140900101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 131400012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1314, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 140800104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1408, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 131000012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1310, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 131000013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1310, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 131100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1311, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 131100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1311, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 131100011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1311, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 131100012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1311, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 131100013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1311, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 131200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1312, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 131300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1313, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 131000011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1310, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 131400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1314, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 142400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1424, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 131400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1314, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 131400013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1314, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 131500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1315, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 131500011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1315, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 131500012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1315, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 131500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1315, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 131500013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1315, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 131600001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1316, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 131400011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1314, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 131000101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1310, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 131000001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1310, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 130900101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1309, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 130200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1302, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 130200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1302, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 130200103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1302, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 130200104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1302, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 130200012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1302, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 130300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 130300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 130300011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 130300012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 130300013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1303, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 130400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1304, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 130400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1304, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 130500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1305, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 130500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1305, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 130600001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1306, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 130600101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1306, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 130700001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1307, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 130700101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1307, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 130800001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1308, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 130800101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1308, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 130900001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1309, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 131600101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1316, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 131600011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1316, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 131600012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1316, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 131600013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1316, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 140400103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1404, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 140500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1405, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.InsertData(
                table: "tbl_role_menu_master",
                columns: new[] { "role_menu_id", "created_by", "created_date", "menu_id", "modified_by", "modified_date", "role_id" },
                values: new object[,]
                {
                    { 140500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1405, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 140500104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1405, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 140500105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1405, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 140600001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1406, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 140600101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1406, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 140600102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1406, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 140600103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1406, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 140600104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1406, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 140700001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1407, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 140700101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1407, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 140700102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1407, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 140700103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1407, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 140700104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1407, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 140700105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1407, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 140700106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1407, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 140800001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1408, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 140800101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1408, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 140800102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1408, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 140800103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1408, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 140400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1404, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 140900001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1409, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 140400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1404, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 140300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1403, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 131700001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1317, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 131700101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1317, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 131700011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1317, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 131700012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1317, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 131700021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1317, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 131700013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1317, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 140100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 140100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 140100102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 140100103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 140100104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 140100105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 140100106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 140100011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 140100012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 140100021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 140100013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 140200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1402, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 140200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1402, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 140200103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1402, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 140300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1403, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 140300103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1403, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 142400105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1424, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 143600106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1436, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 142400012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1424, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 160100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1601, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 160100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1601, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 160100105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1601, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 160100011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1601, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 160100012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1601, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 160100021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1601, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 160200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1602, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 160200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1602, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 160200104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1602, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 150300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1503, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 160200011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1602, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 160200013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1602, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 160300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1603, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 160300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1603, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 160300104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1603, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 160700001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1607, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 160700101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1607, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 160800001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1608, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 160800101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1608, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 160900001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1609, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 160200012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1602, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 150300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1503, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 150200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1502, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 150200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1502, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 144200013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1442, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 144300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1443, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 144300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1443, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 144300011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1443, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 144300012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1443, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 144300021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1443, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 144300013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1443, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 144400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1444, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 144400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1444, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 144400105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1444, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 144400011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1444, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 144400012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1444, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 144400013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1444, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 144500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1445, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 144500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1445, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 144500104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1445, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 144500011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1445, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 144500012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1445, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 144500021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1445, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 150100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1501, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 150100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1501, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 160900101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1609, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 161000001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1610, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 161000101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1610, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 161100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1611, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 164100103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1641, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 164100104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1641, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 164100011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1641, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 164100012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1641, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 164100021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1641, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 164100013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1641, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 164200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1642, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 164200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1642, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 164200103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1642, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 164200104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1642, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 164200011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1642, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 164200012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1642, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 164200021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1642, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 164200013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1642, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 164400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1644, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 164400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1644, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 164400103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1644, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 164400104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1644, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 164400011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1644, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 164400012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1644, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 164400021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1644, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 164100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1641, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 144200021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1442, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 164100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1641, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 161600103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1616, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 161100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1611, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 161100103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1611, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 161100104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1611, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 161200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1612, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 161200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1612, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 161200103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1612, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 161200104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1612, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 161300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1613, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 161300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1613, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 161300103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1613, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 161300104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1613, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 161400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1614, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 161400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1614, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 161400103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1614, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 161400104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1614, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 161500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1615, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 161500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1615, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 161500103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1615, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 161500104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1615, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 161600001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1616, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 161600101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1616, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 161600104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1616, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 142400011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1424, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 144200012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1442, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 144200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1442, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 142800101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1428, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 142800102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1428, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 142800106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1428, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 142900001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1429, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 142900101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1429, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 142900102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1429, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 142900106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1429, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 143000001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1430, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 143000101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1430, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 142800001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1428, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 143000102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1430, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 143100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1431, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 143100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1431, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 143100102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1431, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 143100106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1431, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 143200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1432, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 143200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1432, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 143200102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1432, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 143200106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1432, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 143300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1433, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 143000106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1430, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 142700013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1427, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 142700021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1427, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 142700012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1427, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 142400021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1424, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 142400013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1424, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 142500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1425, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 142500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1425, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 142500105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1425, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 142500011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1425, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 142500012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1425, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 142500021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1425, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 142500013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1425, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 142600001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1426, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 142600101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1426, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 142600105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1426, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 142600011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1426, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 142600012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1426, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 142600021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1426, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 142600013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1426, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 142700001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1427, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 142700101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1427, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 142700102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1427, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 142700106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1427, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 142700011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1427, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 143300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1433, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 143300102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1433, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 143300106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1433, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 143400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1434, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 143900103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1439, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 143900104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1439, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 143900011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1439, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 143900012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1439, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 143900013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1439, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 143900021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1439, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 144000001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1440, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 144000101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1440, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 144000104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1440, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 144000011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1440, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 144000012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1440, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 144000021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1440, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 144000013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1440, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 144100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1441, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 144100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1441, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 144100104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1441, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 144100011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1441, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 144100012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1441, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 144100021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1441, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 144100013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1441, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 144200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1442, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 143900102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1439, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 144200011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1442, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 143900101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1439, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 143800106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1438, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 143400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1434, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 143400102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1434, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 143400106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1434, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 143500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1435, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 143500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1435, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 143500102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1435, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 143500106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1435, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 143600001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1436, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 143600101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1436, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 143600102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1436, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 143600011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1436, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 143600012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1436, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 143600021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1436, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 143600013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1436, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 143700001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1437, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 143700101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1437, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 143700102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1437, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 143700106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1437, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 143800001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1438, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 143800101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1438, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 143800102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1438, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 143900001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1439, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 130100021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1301, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 40400104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 404, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 130100011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1301, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 50900021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 509, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 50900013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 509, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 51000001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 510, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 51000101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 510, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 51000105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 510, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 51000011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 510, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 51000012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 510, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 51000021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 510, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 51000013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 510, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 50900012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 509, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 51100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 511, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 51100105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 511, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 51100011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 511, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 51100012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 511, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 51100013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 511, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 51200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 512, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 51200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 512, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 51200105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 512, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 51300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 513, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 51300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 513, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 51100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 511, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 50900011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 509, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 50900105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 509, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 50900101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 509, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 50400013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 504, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 50500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 505, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 50500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 505, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 50500105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 505, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 50600001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 506, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 50600101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 506, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 50600105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 506, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 50600011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 506, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 50600012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 506, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 50600013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 506, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 50600021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 506, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 50700001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 507, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 50700101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 507, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 50700105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 507, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 50700011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 507, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 50700012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 507, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 50700013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 507, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 50800001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 508, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 50800101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 508, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 50800105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 508, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 50900001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 509, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 51300105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 513, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 51300011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 513, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 51300012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 513, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 51300021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 513, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 60300105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 603, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 60400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 604, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 60400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 604, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 60400105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 604, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 }
                });

            migrationBuilder.InsertData(
                table: "tbl_role_menu_master",
                columns: new[] { "role_menu_id", "created_by", "created_date", "menu_id", "modified_by", "modified_date", "role_id" },
                values: new object[,]
                {
                    { 60500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 605, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 60500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 605, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 60500105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 605, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 60500011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 605, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 60500012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 605, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 60500021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 605, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 60500013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 605, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 60600001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 606, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 60600101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 606, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 60600105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 606, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 60600011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 606, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 60600012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 606, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 60600013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 606, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 60700001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 607, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 60700101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 607, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 60700105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 607, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 70100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 701, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 60300103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 603, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 50400012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 504, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 60300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 603, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 60200105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 602, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 51300013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 513, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 51400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 514, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 51400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 514, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 51400105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 514, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 51400011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 514, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 51400012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 514, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 51400013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 514, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 51500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 515, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 51500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 515, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 51500105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 515, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 60100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 601, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 60100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 601, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 60100103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 601, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 60100105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 601, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 60100011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 601, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 60100012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 601, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 60100021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 601, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 60100013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 601, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 60200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 602, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 60200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 602, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 60200103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 602, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 60300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 603, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 70100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 701, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 50400011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 504, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 50400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 504, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 20100105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 20100106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 20200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 202, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 20200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 202, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 20200103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 202, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 20300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 203, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 20300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 203, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 20300103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 203, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 20400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 204, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 20100104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 20400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 204, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 20500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 205, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 20500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 205, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 20500103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 205, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 30100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 301, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 30100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 301, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 30100102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 301, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 30100103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 301, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 30200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 302, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 30200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 302, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 20400103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 204, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 20100103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 20100102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 20100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 100102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 100103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 100104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 100105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 100106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 100011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 100012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 100021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 100013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 100107, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 107 },
                    { 10100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 10100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 10100103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 10200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 10200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 10300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 10300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 10300103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 20100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 201, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 30200102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 302, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 30200103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 302, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 30300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 303, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 30300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 303, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 40400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 404, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 164400013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1644, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 40400105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 404, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 50100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 501, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 50100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 501, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 50100105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 501, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 50100011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 501, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 50100012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 501, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 50100021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 501, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 50100013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 501, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 50200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 502, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 50200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 502, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 50200105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 502, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 50300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 503, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 50300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 503, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 50300105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 503, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 50300011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 503, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 50300012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 503, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 50300013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 503, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 50300021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 503, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 50400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 504, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 40400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 404, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 50400105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 504, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 40300105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 403, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 40300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 403, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 30300103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 303, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 30300105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 303, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 30400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 304, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 30400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 304, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 30400103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 304, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 30500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 305, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 30500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 305, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 30500103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 305, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 30600001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 306, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 30600101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 306, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 30600103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 306, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 40100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 40100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 40100103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 40100104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 40100105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 401, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 40200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 402, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 40200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 402, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 40200103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 402, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 40200105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 402, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 105 },
                    { 40300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 403, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 40300104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 403, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 70100104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 701, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 70100106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 701, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 70200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 702, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 100400103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1004, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 100400011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1004, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 100400012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1004, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 100400013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1004, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 110100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 110100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 110100103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 110100104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 110200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 100400102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1004, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 110200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 110200104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 110300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 110300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 110300103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 110300104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 110400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 110400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 110400103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 110400104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 110200103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 100400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1004, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 100400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1004, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 100300013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1003, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 90700102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 907, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 100100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 100100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 100100102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 100100103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 100100104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 100100011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 100100012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 100100013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 100100021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 100200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1002, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 100200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1002, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 100200102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1002, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 100200103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1002, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 100300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1003, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 100300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1003, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 100300102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1003, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 100300103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1003, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 100300011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1003, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 100300012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1003, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 100300021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1003, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 110500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 110500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 110500103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 110500104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 111200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1112, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 111200103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1112, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 111200104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1112, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 111300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1113, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 111300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1113, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 111300103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1113, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 111300104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1113, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 111400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1114, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 111400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1114, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 111400103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1114, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 111400104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1114, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 111500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1115, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 111500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1115, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 111500103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1115, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 111500104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1115, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 120100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1201, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 120100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1201, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 130100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1301, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 130100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1301, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 130100103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1301, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 130100104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1301, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 111200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1112, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 90700101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 907, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 111100104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1111, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 111100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1111, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 110600001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 110600101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 110600103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 110600104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 110700001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1107, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 110700101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1107, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 110700103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1107, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 110700104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1107, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 110800001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1108, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 110800101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1108, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 110800103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1108, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 110800104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1108, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 110900001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1109, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 110900101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1109, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 110900103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1109, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 110900104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1109, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 111000001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1110, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 111000101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1110, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 111000103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1110, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 111000104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1110, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 111100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1111, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 111100103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1111, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 90700001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 907, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 90600013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 906, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 90600012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 906, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 71200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 712, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 71200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 712, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 71200106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 712, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 71300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 713, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 71300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 713, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 71300106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 713, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 71400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 714, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 71400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 714, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 71400102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 714, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 71400106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 714, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 71500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 715, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 71500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 715, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 71500102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 715, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 71500106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 715, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 71600001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 716, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 71600101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 716, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 71600102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 716, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 71600106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 716, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 71700001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 717, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 71700101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 717, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 71700102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 717, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 71100107, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 711, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 107 },
                    { 71700106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 717, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 71100104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 711, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 71100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 711, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 70200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 702, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 70200106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 702, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 70300001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 703, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 70300101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 703, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 70300106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 703, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 70400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 704, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 70400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 704, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 70500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 705, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 70600001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 706, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 70700001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 707, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 70700101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 707, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 70700106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 707, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 70800001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 708, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 70800101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 708, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 70800106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 708, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 70900001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 709, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 71000001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 710, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 71000101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 710, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 71000102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 710, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 71000104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 710, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 104 },
                    { 71100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 711, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 71100102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 711, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 130100012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1301, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 71800001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 718, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 71800102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 718, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 90100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 901, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 90100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 901, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 90200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 902, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 90200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 902, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 90200102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 902, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 }
                });

            migrationBuilder.InsertData(
                table: "tbl_role_menu_master",
                columns: new[] { "role_menu_id", "created_by", "created_date", "menu_id", "modified_by", "modified_date", "role_id" },
                values: new object[,]
                {
                    { 90200103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 902, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 90400001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 904, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 90400101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 904, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 90400102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 904, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 90400103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 904, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 103 },
                    { 90500001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 905, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 90500101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 905, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 90500102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 905, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 90500011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 905, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 90500012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 905, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12 },
                    { 90500021, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 905, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21 },
                    { 90500013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 905, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13 },
                    { 90600001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 906, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 90600101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 906, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 90600102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 906, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 90600011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 906, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11 },
                    { 80200106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 802, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 71800101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 718, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 80200102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 802, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 80200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 802, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 71800106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 718, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 71900001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 719, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 71900101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 719, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 71900102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 719, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 71900106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 719, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 72000001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 720, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 72000101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 720, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 72000102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 720, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 72000106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 720, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 72100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 721, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 72100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 721, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 72100102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 721, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 72100106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 721, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 72200001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 722, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 72200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 722, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 72200102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 722, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 72200106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 722, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 80100001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 801, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 80100101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 801, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 },
                    { 80100102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 801, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 102 },
                    { 80100106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 801, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 106 },
                    { 80200101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 802, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 101 }
                });

            migrationBuilder.InsertData(
                table: "tbl_salary_group",
                columns: new[] { "group_id", "created_by", "created_dt", "description", "grade_Id", "group_name", "is_active", "maxvalue", "minvalue", "modified_by", "modified_dt" },
                values: new object[] { 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", null, "SG1", 1, 0.0, 0.0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "tbl_employment_type_master",
                columns: new[] { "employment_type_id", "actual_duration_days", "actual_duration_end_period", "actual_duration_start_period", "created_by", "created_date", "duration_days", "duration_end_period", "duration_start_period", "effective_date", "employee_id", "employment_type", "is_deleted", "last_modified_by", "last_modified_date" },
                values: new object[] { 1, 1000, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, (byte)3, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "tbl_rpt_title_master",
                columns: new[] { "title_id", "component_id", "created_by", "created_date", "display_order", "is_active", "last_modified_by", "last_modified_date", "payroll_report_property", "rpt_id", "rpt_title" },
                values: new object[,]
                {
                    { 65, 3005, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 65, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "PF Employee" },
                    { 506, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Category Address" },
                    { 507, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Type Of Employment" },
                    { 508, 113, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Mobile" },
                    { 509, 507, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "UAN Number" },
                    { 510, 119, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "PAN No" },
                    { 511, 120, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "PAN Name" },
                    { 512, 205, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "ESIC Number" },
                    { 513, 121, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Aadhar card Number" },
                    { 514, 122, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Aadhar name" },
                    { 505, 112, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Education Level" },
                    { 515, 201, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Salary Account No" },
                    { 517, 202, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Bank IFSC Code" },
                    { 518, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Address" },
                    { 519, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "service book no" },
                    { 520, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Resignation date" },
                    { 521, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Last working date" },
                    { 522, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 22, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Reason Master" },
                    { 523, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 23, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "mark of identification" },
                    { 524, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 24, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "specimen impression" },
                    { 525, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 25, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Is Active" },
                    { 526, 101, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 26, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Company Name" },
                    { 516, 203, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Bank Name" },
                    { 504, 111, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Nationality" },
                    { 503, 109, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Date of Birth" },
                    { 502, 105, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Father Husband Name" },
                    { 68, 3003, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 68, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, "Arrear ESIC Employee" },
                    { 69, 2008, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 69, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "Children Education Allowance" },
                    { 70, 2008, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 70, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, "Arrear Children Education Allowance" },
                    { 71, 537, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 71, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "Gross" },
                    { 72, 537, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 72, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, "Arrear Gross" },
                    { 73, 2006, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 73, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "Over Time" },
                    { 74, 2009, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 74, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "CityCompensatoryAllowances" },
                    { 75, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 75, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "OtherAllowance" },
                    { 76, 537, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 76, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1, "Gross Salary" },
                    { 77, 3005, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 77, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1, "Provident Fund" },
                    { 78, 3010, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 78, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Professional Tax" },
                    { 79, 3003, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 79, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1, "ESIC" },
                    { 80, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "LWF" },
                    { 81, 3001, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 81, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "TDS" },
                    { 82, 3006, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 82, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "Voluntary Provident Fund" },
                    { 83, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 83, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Notice Reco" },
                    { 84, 3009, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 84, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "Other Deduction" },
                    { 85, 3002, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 85, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "Advance" },
                    { 86, 3008, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 86, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "Recovery" },
                    { 87, 3999, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 87, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1, "Gross Deduction" },
                    { 88, 2999, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 88, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1, "Net Salary" },
                    { 89, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 89, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Remarks" },
                    { 501, 108, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Gender" },
                    { 527, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 27, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Division Name" },
                    { 528, 118, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 28, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "State Name" },
                    { 529, 117, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 29, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Branch Name" },
                    { 530, 117, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Location Name" },
                    { 557, 2003, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 57, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "Conveyance" },
                    { 558, 2007, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 58, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "Medical Allowance" },
                    { 559, 2004, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "Special Allowance" },
                    { 560, 3005, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "PF Employee" },
                    { 561, 3003, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 61, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "ESIC Employee" },
                    { 562, 2008, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 62, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "Children Education Allowance" },
                    { 563, 537, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 63, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "Gross" },
                    { 564, 2006, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 64, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "Over Time" },
                    { 565, 2009, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 65, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "CityCompensatoryAllowances" },
                    { 566, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 66, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "OtherAllowance" },
                    { 556, 2002, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 56, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "HRA Allowance" },
                    { 567, 537, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 67, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "Gross Salary" },
                    { 569, 3010, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 69, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Professional Tax" },
                    { 570, 3003, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 70, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "ESIC" },
                    { 571, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 71, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "LWF" },
                    { 572, 3001, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 72, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, "TDS" },
                    { 573, 3006, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 73, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, "Voluntary Provident Fund" },
                    { 574, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 74, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Notice Reco" },
                    { 575, 3009, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 75, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, "Other Deduction" },
                    { 576, 3002, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 76, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, "Advance" },
                    { 577, 3008, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 77, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 2, "Recovery" },
                    { 578, 3999, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 78, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "Gross Deduction" },
                    { 568, 3005, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 68, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "Provident Fund" },
                    { 67, 3003, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 67, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "ESIC Employee" },
                    { 555, 2001, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "Basic Salary" },
                    { 553, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 53, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Notice Pay" },
                    { 531, 116, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 31, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Department Name" },
                    { 532, 106, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 32, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Date Of Joining" },
                    { 533, 115, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 33, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Designation Name" },
                    { 534, 114, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 34, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Grade" },
                    { 535, 510, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "PF Applicable" },
                    { 536, 512, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 36, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "PF Number" },
                    { 537, 513, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 37, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "PF Group" },
                    { 538, 520, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 38, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "PF Ceiling" },
                    { 539, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 39, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "ESIC Group" },
                    { 540, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "PT Applicable" },
                    { 554, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 54, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Pre hold  salary released" },
                    { 541, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 41, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "PT Group" },
                    { 543, 519, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 43, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "VPF Percentage" },
                    { 544, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 44, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Is daily" },
                    { 545, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Salary On Hold" },
                    { 546, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 46, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Salary Process Type" },
                    { 547, 529, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 47, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Monthly Gross " },
                    { 548, 504, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 48, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Days Worked" },
                    { 549, 505, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 49, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Arrears Days" },
                    { 550, 501, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Days In Month" },
                    { 551, 2012, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 51, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Gratuity" },
                    { 552, 2013, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 52, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Leen" },
                    { 542, 518, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 42, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "VPF Applicable" },
                    { 66, 3005, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 66, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, "Arrear PF Employee" },
                    { 580, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 80, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Remarks" },
                    { 64, 2004, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 64, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, "Arrear Special Allowance" },
                    { 29, 117, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 29, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Branch Name" },
                    { 28, 118, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 28, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "State Name" },
                    { 27, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 27, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Division Name" },
                    { 26, 101, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 26, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Company Name" },
                    { 25, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 25, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Is Active" },
                    { 24, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 24, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "specimen impression" },
                    { 23, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 23, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "mark of identification" },
                    { 22, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 22, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Reason Master" },
                    { 21, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Last working date" },
                    { 20, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Resignation date" },
                    { 19, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "service book no" },
                    { 18, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Address" },
                    { 17, 202, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Bank IFSC Code" },
                    { 16, 203, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Bank Name" },
                    { 15, 201, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Salary Account No" },
                    { 14, 122, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Aadhar name" },
                    { 579, 2999, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 79, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 2, "Net Salary" },
                    { 12, 205, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "ESIC Number" },
                    { 11, 120, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "PAN Name" },
                    { 10, 119, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "PAN No" },
                    { 9, 507, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "UAN Number" },
                    { 8, 113, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Mobile" },
                    { 7, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Type Of Employment" },
                    { 6, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Category Address" },
                    { 5, 112, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Education Level" },
                    { 4, 111, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Nationality" },
                    { 3, 109, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Date of Birth" },
                    { 2, 105, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Father Husband Name" },
                    { 1, 108, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Gender" },
                    { 30, 117, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Location Name" },
                    { 31, 116, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 31, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Department Name" },
                    { 13, 121, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Aadhar card Number" },
                    { 33, 115, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 33, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Designation Name" },
                    { 32, 106, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 32, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Date Of Joining" },
                    { 63, 2004, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 63, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "Special Allowance" },
                    { 62, 2007, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 62, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, "Arrear Medical Allowance" },
                    { 61, 2007, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 61, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "Medical Allowance" },
                    { 60, 2003, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, "Arrear conveyance" },
                    { 59, 2003, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 59, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "Conveyance" },
                    { 58, 2002, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 58, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, "Arrear HRA Allowance" },
                    { 56, 2001, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 56, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, "Arrear Basic Salary" },
                    { 55, 2001, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 55, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "Basic Salary" },
                    { 54, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 54, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Pre hold  salary released" },
                    { 53, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 53, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Notice Pay" },
                    { 52, 2013, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 52, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Leen" },
                    { 51, 2012, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 51, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Gratuity" },
                    { 50, 501, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Days In Month" },
                    { 49, 505, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 49, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Arrears Days" },
                    { 57, 2002, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 57, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, "HRA Allowance" },
                    { 47, 529, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 47, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Monthly Gross " },
                    { 34, 114, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 34, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Grade" },
                    { 48, 504, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 48, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Days Worked" },
                    { 35, 510, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "PF Applicable" },
                    { 36, 512, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 36, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "PF Number" },
                    { 38, 520, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 38, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "PF Ceiling" },
                    { 39, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 39, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "ESIC Group" },
                    { 40, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "PT Applicable" },
                    { 37, 513, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 37, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "PF Group" },
                    { 41, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 41, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "PT Group" },
                    { 42, 518, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 42, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "VPF Applicable" },
                    { 43, 519, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 43, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "VPF Percentage" },
                    { 44, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 44, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Is daily" },
                    { 45, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Salary On Hold" },
                    { 46, null, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 46, 1, 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "Salary Process Type" }
                });

            migrationBuilder.InsertData(
                table: "tbl_state",
                columns: new[] { "state_id", "country_id", "created_by", "created_date", "is_deleted", "last_modified_by", "last_modified_date", "name" },
                values: new object[,]
                {
                    { 11, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Goa" },
                    { 16, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jharkhand" },
                    { 15, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jammu and Kashmir" },
                    { 14, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Himachal Pradesh" },
                    { 13, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Haryana" },
                    { 12, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gujarat" },
                    { 17, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Karnataka" },
                    { 10, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Delhi" },
                    { 4, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assam" },
                    { 8, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dadra and Nagar Haveli" },
                    { 7, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chhattisgarh" },
                    { 6, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chandigarh" },
                    { 5, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bihar" },
                    { 3, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Arunachal Pradesh" },
                    { 2, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Andhra Pradesh" },
                    { 18, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kerala" },
                    { 9, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daman and Diu" },
                    { 19, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ladakh" },
                    { 35, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Uttar Pradesh" },
                    { 21, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Madhya Pradesh" },
                    { 37, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "West Bengal" },
                    { 36, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Uttarakhand" },
                    { 34, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tripura" },
                    { 33, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Telangana" },
                    { 32, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tamil Nadu" },
                    { 31, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sikkim" },
                    { 20, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lakshadweep" },
                    { 30, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rajasthan" }
                });

            migrationBuilder.InsertData(
                table: "tbl_state",
                columns: new[] { "state_id", "country_id", "created_by", "created_date", "is_deleted", "last_modified_by", "last_modified_date", "name" },
                values: new object[,]
                {
                    { 28, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Puducherry" },
                    { 27, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Odisha" },
                    { 26, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nagaland" },
                    { 25, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mizoram" },
                    { 24, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Meghalaya" },
                    { 23, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manipur" },
                    { 22, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maharashtra" },
                    { 29, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Punjab" },
                    { 1, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Andaman and Nicobar Islands" }
                });

            migrationBuilder.InsertData(
                table: "tbl_user_master",
                columns: new[] { "user_id", "created_by", "created_date", "default_company_id", "employee_id", "is_active", "is_logged_blocked", "is_logged_in", "last_logged_dt", "last_modified_by", "last_modified_date", "logged_blocked_dt", "password", "user_type", "username" },
                values: new object[] { 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1, (byte)0, (byte)0, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "jnZWe3S+++aQtpmKlibdOA==", (byte)1, "Admin" });

            migrationBuilder.InsertData(
                table: "tbl_city",
                columns: new[] { "city_id", "created_by", "created_date", "is_deleted", "last_modified_by", "last_modified_date", "name", "pincode", "state_id" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 1 },
                    { 22, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 22 },
                    { 23, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 23 },
                    { 24, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 24 },
                    { 25, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 25 },
                    { 26, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 26 },
                    { 27, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 27 },
                    { 28, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 28 },
                    { 29, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 29 },
                    { 30, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 30 },
                    { 31, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 31 },
                    { 32, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 32 },
                    { 33, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 33 },
                    { 34, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 34 },
                    { 35, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 35 },
                    { 36, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 36 },
                    { 21, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 21 },
                    { 20, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 20 },
                    { 19, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 19 },
                    { 18, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 18 },
                    { 2, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 2 },
                    { 3, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 3 },
                    { 4, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 4 },
                    { 5, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 5 },
                    { 6, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 6 },
                    { 7, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 7 },
                    { 8, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 8 },
                    { 37, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 37 },
                    { 9, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 9 },
                    { 11, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 11 },
                    { 12, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 12 },
                    { 13, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 13 },
                    { 14, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 14 },
                    { 15, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 15 },
                    { 16, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 16 },
                    { 17, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 17 },
                    { 10, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "000001", 10 }
                });

            migrationBuilder.InsertData(
                table: "tbl_user_role_map",
                columns: new[] { "claim_master_id", "created_by", "created_date", "is_deleted", "last_modified_by", "last_modified_date", "role_id", "user_id" },
                values: new object[] { 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 });

            migrationBuilder.InsertData(
                table: "tbl_company_master",
                columns: new[] { "company_id", "address_line_one", "address_line_two", "city_id", "company_code", "company_logo", "company_name", "company_website", "country_id", "created_by", "created_date", "is_active", "is_emp_code_manual_genrate", "last_modified_by", "last_modified_date", "pin_code", "primary_contact_number", "primary_email_id", "secondary_contact_number", "secondary_email_id", "state_id", "total_emp", "user_type" },
                values: new object[] { 1, "836, 2nd floor, Udyog Vihar Phase V, Sector 19", "Gurugram", 13, "TR", null, "Travolook", "https://www.travolook.in/", 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, (byte)1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 122008, "01246434800", "support@travolook.in", null, null, 13, 1, 1 });

            migrationBuilder.InsertData(
                table: "tbl_location_master",
                columns: new[] { "location_id", "address_line_one", "address_line_two", "city_id", "company_id", "country_id", "created_by", "created_date", "image", "is_active", "last_modified_by", "last_modified_date", "location_code", "location_name", "pin_code", "primary_contact_number", "primary_email_id", "secondary_contact_number", "secondary_email_id", "state_id", "type_of_location", "website" },
                values: new object[] { 1, "836, 2nd floor, Udyog Vihar Phase V, Sector 19", "Gurugram", 13, 1, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HO", "Head Office", 122008, "01246434800", "support@travolook.in", null, null, 13, (byte)1, null });

            migrationBuilder.InsertData(
                table: "tbl_company_emp_setting",
                columns: new[] { "sno", "company_id", "current_range", "from_range", "is_active", "last_genrated", "number_of_character_for_employee_code", "prefix_for_employee_code", "to_range" },
                values: new object[] { 1, 1, 1, 1, (byte)1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "TR", 1 });

            migrationBuilder.InsertData(
                table: "tbl_component_formula_details",
                columns: new[] { "sno", "company_id", "component_id", "created_by", "created_dt", "deleted_by", "deleted_dt", "formula", "function_calling_order", "is_deleted", "salary_group_id" },
                values: new object[,]
                {
                    { 2009, 1, 2009, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 2008, 1, 2008, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 2007, 1, 2007, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 2006, 1, 2006, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IFNULL(@OTHour,0)* IFNULL(@OTRate,0)", 30, 0, 1 },
                    { 2005, 1, 2005, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 2004, 1, 2004, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "GREATEST(ROUND(@GrossSalary-@BasicSalary-@HRA-@Conveyance,0),0)", 60, 0, 1 },
                    { 2003, 1, 2003, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "GREATEST(IF((@GrossSalary-@BasicSalary-@HRA)>=1600,1600,(@GrossSalary-@BasicSalary-@HRA)),0)", 50, 0, 1 },
                    { 2002, 1, 2002, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ROUND(if( @GrossSalary>25000,@BasicSalary*0.5,@GrossSalary*0.33),0)", 40, 0, 1 },
                    { 2001, 1, 2001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ROUND(if(@GrossSalary>25000,@GrossSalary*0.6,@GrossSalary*0.67),0)", 30, 0, 1 },
                    { 546, 1, 546, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 1, 0, 1 },
                    { 545, 1, 545, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 1, 0, 1 },
                    { 544, 1, 544, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ifnull(@PaidDays,0)+ifnull(@AddtitionalPaidDay,0) + ifnull( @CompOffEncashmentDay ,0)+IfNull( @ELEncashmentDay,0) + IfNull( @NoticePaymentDay,0) - IfNull( @NoticeRecoveryDay,0)", 4, 0, 1 },
                    { 543, 1, 543, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 1, 0, 1 },
                    { 542, 1, 542, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 1, 0, 1 },
                    { 541, 1, 541, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 1, 0, 1 },
                    { 540, 1, 540, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", 10, 0, 1 },
                    { 537, 1, 537, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Round(IFNULL(@BasicSalary,0)+IFNULL(@HRA,0)+IFNULL(@Conveyance,0)+IFNULL(@SPL,0)+IFNULL(@DA,0),0)", 100, 0, 1 },
                    { 536, 1, 536, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@GrossSalary+@Employer_pf_amount", 201, 0, 1 },
                    { 535, 1, 535, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 534, 1, 534, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 533, 1, 533, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@OTHour_sys", 2, 0, 1 },
                    { 532, 1, 532, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@OTRate_sys", 2, 0, 1 },
                    { 531, 1, 531, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 530, 1, 530, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 529, 1, 529, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@GrossSalary_sys", 2, 0, 1 },
                    { 528, 1, 528, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 527, 1, 527, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 2010, 1, 2010, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 526, 1, 526, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 2011, 1, 2011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 2013, 1, 2013, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 5006, 1, 5006, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 5005, 1, 5005, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 5004, 1, 5004, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 5003, 1, 5003, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 5002, 1, 5002, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 5001, 1, 5001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 5000, 1, 5000, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 4005, 1, 4005, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 4004, 1, 4004, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 4003, 1, 4003, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 4002, 1, 4002, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 4001, 1, 4001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@Pf_amount", 71, 0, 1 },
                    { 3999, 1, 3999, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IFNULL(@Tax,0)+IFNULL(@Advance_Loan,0)+IFNULL(@EsicAmount,0)+IFNULL(@Pf_amount,0)+IFNULL(@OtherDeduction,0)", 100, 0, 1 },
                    { 3012, 1, 3012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 3011, 1, 3011, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 3010, 1, 3010, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 3009, 1, 3009, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 3008, 1, 3008, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 3007, 1, 3007, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 3006, 1, 3006, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "If(@Vpf_applicable=1,If(@VPfGroup=1,@vpf_percentage, ROUND( @BasicSalary * @vpf_percentage/100.0 ,0)) ,0)", 70, 0, 1 },
                    { 3005, 1, 3005, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "If ( (@BasicSalary)<=@PfSalarySlab, If ((@BasicSalary)*(@PfPercentage/100.0) >=@pf_celling,@pf_celling,ROUND((@BasicSalary)*(@PfPercentage/100.0),0)),If(@PfApplicable=1, 	If ( if(@PfGroup=1,@PfSalarySlab,@BasicSalary) *(@PfPercentage/100.0) >=@pf_celling    ,@pf_celling,ROUND(if(@PfGroup=1,@PfSalarySlab,@BasicSalary)*(@PfPercentage/100.0),0)),0))", 70, 0, 1 },
                    { 3004, 1, 3004, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "If ( @EsicApplicableManual=1, round((@GrossSalary* @EmployerEsicPercentage/100.0),0),0)", 30, 0, 1 },
                    { 3003, 1, 3003, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "If ( @EsicApplicableManual=1, ceiling(@GrossSalary* @EsicPercentage/100.0),0)", 30, 0, 1 },
                    { 3002, 1, 3002, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@Advance_Loan_sys", 30, 0, 1 },
                    { 3001, 1, 3001, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@Tax_sys", 30, 0, 1 },
                    { 2999, 1, 2999, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IFNULL(@TotalGross,0)-IFNULL(@Deduction,0)", 200, 0, 1 },
                    { 2014, 1, 2014, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 2012, 1, 2012, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 525, 1, 525, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 5007, 1, 5007, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 30, 0, 1 },
                    { 523, 1, 523, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 121, 1, 121, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 120, 1, 120, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 119, 1, 119, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 118, 1, 118, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 117, 1, 117, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 116, 1, 116, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 115, 1, 115, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 114, 1, 114, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 113, 1, 113, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 112, 1, 112, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 111, 1, 111, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 110, 1, 110, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 109, 1, 109, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 108, 1, 108, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 107, 1, 107, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 106, 1, 106, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 105, 1, 105, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 104, 1, 104, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 103, 1, 103, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 102, 1, 102, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 101, 1, 101, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 100, 1, 100, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 3000, 1, 3000, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 1, 1, 1 },
                    { 2000, 1, 2000, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 1, 1, 1 },
                    { 1000, 1, 1000, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 1, 1, 1 },
                    { 1, 1, 1, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 1, 1, 1 },
                    { 524, 1, 524, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 201, 1, 201, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 202, 1, 202, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 122, 1, 122, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 204, 1, 204, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 522, 1, 522, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 521, 1, 521, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 520, 1, 520, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 519, 1, 519, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 518, 1, 518, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 517, 1, 517, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 516, 1, 516, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 515, 1, 515, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 514, 1, 514, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 513, 1, 513, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 512, 1, 512, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 511, 1, 511, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 203, 1, 203, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 509, 1, 509, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 510, 1, 510, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 507, 1, 507, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 205, 1, 205, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 206, 1, 206, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 207, 1, 207, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 508, 1, 508, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 210, 1, 210, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "if(@GrossSalary>=21000,0,1)", 10, 0, 1 },
                    { 208, 1, 208, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 502, 1, 502, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 },
                    { 503, 1, 503, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@LodSys", 2, 0, 1 },
                    { 504, 1, 504, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@ToalPayrollDay-@LopDays", 3, 0, 1 },
                    { 505, 1, 505, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 3, 0, 1 },
                    { 506, 1, 506, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "0", 3, 0, 1 },
                    { 501, 1, 501, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 0, 1 }
                });

            migrationBuilder.InsertData(
                table: "tbl_document_type_master",
                columns: new[] { "doc_type_id", "company_id", "created_by", "created_date", "doc_name", "is_deleted", "modified_by", "modified_date", "remarks" },
                values: new object[,]
                {
                    { 7, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Secondary Certificate", 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "" },
                    { 10, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Diploma", 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "" },
                    { 9, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Post Graduation", 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "" },
                    { 8, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Graduation", 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "" },
                    { 6, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Electricity Bill", 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "" },
                    { 3, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Passport", 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "" },
                    { 4, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Voter Card", 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "" },
                    { 2, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pan", 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "" },
                    { 1, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adhar", 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "" },
                    { 5, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rent Agrement", 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "" }
                });

            migrationBuilder.InsertData(
                table: "tbl_employee_company_map",
                columns: new[] { "sno", "company_id", "created_by", "created_date", "employee_id", "is_default", "is_deleted", "last_modified_by", "last_modified_date" },
                values: new object[] { 1, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, true, 0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "tbl_working_role",
                columns: new[] { "working_role_id", "company_id", "created_by", "created_date", "dept_id", "is_active", "is_default", "last_modified_by", "last_modified_date", "working_role_name" },
                values: new object[,]
                {
                    { 152, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "QC Assistant" },
                    { 151, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "QA Chemist" },
                    { 150, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Project Manager - CSR" },
                    { 149, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Project Manager" },
                    { 148, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Project Coordinator" },
                    { 146, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Production Executive" },
                    { 145, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Product Trainer" },
                    { 144, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Procurement Executive" },
                    { 143, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Plant HR" },
                    { 142, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pantry Boy" },
                    { 147, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Production Supervisor" },
                    { 153, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "QC Chemist" },
                    { 155, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "QC Officer" },
                    { 156, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quality Analyst" },
                    { 157, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Quality Auditor" },
                    { 158, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "R&D Chemist" },
                    { 159, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Raw material and Personal ingerient care executive" },
                    { 160, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Regional Executive Trainer" },
                    { 161, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Relationship Manager" },
                    { 162, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sales Executive" },
                    { 163, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SCM Coordinator" },
                    { 164, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Section Head- CRD & DST" },
                    { 165, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Section Head- Operations" },
                    { 166, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Security Guard" },
                    { 154, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "QC Executive" },
                    { 141, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Packing Assistant" },
                    { 139, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Packaging Executive" },
                    { 138, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator Mechanical" },
                    { 167, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Agronomist" },
                    { 116, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maintenance Technician" },
                    { 117, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manager" },
                    { 118, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manager - Legal & Company Secretary" },
                    { 119, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manager-Finance & Accounts" },
                    { 120, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Managing Director" },
                    { 121, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manufacturing Operator" },
                    { 122, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marketing Executive" },
                    { 123, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mfg. Chemist" },
                    { 124, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "MIS Data Analyst" },
                    { 125, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "MIS Executive" },
                    { 140, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Packaging Supervisor" },
                    { 126, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "MIS Senior Executive" },
                    { 128, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "National Head" },
                    { 129, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "National Head-Sales & Training" },
                    { 130, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Network Security Engineer" },
                    { 131, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "NRD Coordinator" },
                    { 132, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Office Boy" },
                    { 133, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operation Assistant" },
                    { 134, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator Electrical" },
                    { 115, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Labelling Machine Operator" },
                    { 135, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator Filling" },
                    { 136, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator Labelling" },
                    { 137, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operator Manufacturing" },
                    { 127, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mobile App. Developer" },
                    { 168, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Audit Officer" },
                    { 204, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Supervisor-Combo" },
                    { 170, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Executive- Operations" },
                    { 200, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Store Incharge" },
                    { 201, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Store Officer" },
                    { 202, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Store Supervisor" },
                    { 203, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sub Editor" },
                    { 205, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Supervisor-Packaging" },
                    { 206, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System Administrator" },
                    { 207, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Talent Acquisition Specialist" },
                    { 208, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tally Administrator" },
                    { 209, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Team Coordinator" }
                });

            migrationBuilder.InsertData(
                table: "tbl_working_role",
                columns: new[] { "working_role_id", "company_id", "created_by", "created_date", "dept_id", "is_active", "is_default", "last_modified_by", "last_modified_date", "working_role_name" },
                values: new object[,]
                {
                    { 210, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Team Leader" },
                    { 211, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Technical Head" },
                    { 199, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Store Helper" },
                    { 212, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "TECHNOLOGY OFFICER" },
                    { 214, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trainee Engineer" },
                    { 215, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Training Executive" },
                    { 216, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tube and Jar Filling Operator" },
                    { 217, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "UI Developer" },
                    { 218, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Utility & Maintenance Head" },
                    { 219, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Utility Operator" },
                    { 220, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "VCM Executive" },
                    { 221, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Video Editor" },
                    { 222, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warehouse Executive" },
                    { 223, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warehouse Incharge" },
                    { 114, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Labelling and Compression Operator and  Bottle Filling" },
                    { 213, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trainee Electrician" },
                    { 198, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Store Executive" },
                    { 197, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Store Attendent" },
                    { 196, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Store Assistant" },
                    { 171, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Executive Warehouse" },
                    { 172, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Executive- Warehouse" },
                    { 173, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Executive-Distributor Record" },
                    { 174, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Executive-MIS" },
                    { 175, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Executive-SCM" },
                    { 176, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior General Manager" },
                    { 177, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Manager" },
                    { 178, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Manager - Finance & Accounts" },
                    { 179, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Manager Purchase and Procurement" },
                    { 180, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Manager-SCM" },
                    { 181, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Operation Executive" },
                    { 182, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior R&D Chemist" },
                    { 183, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Software Developer" },
                    { 184, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Visualizer" },
                    { 185, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "SEO-Executive" },
                    { 186, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Software Developer" },
                    { 187, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Software Tester" },
                    { 188, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Assistant Supervisor" },
                    { 189, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Assistant-Operation" },
                    { 190, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Executive" },
                    { 191, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Executive - Procurement & Packaging" },
                    { 192, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Executive -Procurement RM & CM Audit" },
                    { 193, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Manager" },
                    { 194, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Supervisor-Administration" },
                    { 195, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sr. Supervisor-Inward" },
                    { 169, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Senior Executive- Finance & Accounts" },
                    { 113, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lab Assistant" },
                    { 46, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Category Manager" },
                    { 111, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Joining and  Billing" },
                    { 30, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Utility & Maintenance" },
                    { 31, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Warehouse" },
                    { 32, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Supervisor" },
                    { 33, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Supervisor-Administration (Security and Labour)" },
                    { 34, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Team Leader" },
                    { 35, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Associate Executive" },
                    { 36, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Associate Executive - Warehouse" },
                    { 37, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Associate Executive Warehouse" },
                    { 38, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Audit Officer" },
                    { 39, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BDT Coordinator" },
                    { 40, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Billing" },
                    { 29, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Store" },
                    { 41, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bottle and Jar Filling Operator" },
                    { 43, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Business Analyst- Senior Manager" },
                    { 44, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Business Data Analyst" },
                    { 45, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Business Development Executive" },
                    { 224, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Warehouse Manager" },
                    { 47, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Chief Executive Officer" },
                    { 48, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Combo Executive" },
                    { 49, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Complaint Executive" },
                    { 50, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Compliance Manager" },
                    { 51, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Consultant" },
                    { 52, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Content Writer" },
                    { 53, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer Care Executive" },
                    { 42, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Business Analyst" },
                    { 28, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-SCM" },
                    { 27, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-QC" },
                    { 26, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-QA" },
                    { 1, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Account Executive" },
                    { 2, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin Executive" },
                    { 3, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin Incharge" },
                    { 4, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin Supervisor" },
                    { 5, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Agronomist" },
                    { 6, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AM Warehouse account operation" },
                    { 7, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Application Manager" },
                    { 8, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Application Manager  Android App Developer" },
                    { 9, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Application Support Engineer" },
                    { 10, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Area Supervisor" },
                    { 11, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Asistant Team Leader" },
                    { 12, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Executive-Commercial" },
                    { 13, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant General Manager" },
                    { 14, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager" },
                    { 15, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager - QC" },
                    { 16, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager SCM" },
                    { 17, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Accounts" },
                    { 18, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Administration" },
                    { 19, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Distributor Record" },
                    { 20, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Event" },
                    { 21, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Finance & Accounts" },
                    { 22, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Human Resource" },
                    { 23, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Legal" },
                    { 24, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Operations" },
                    { 25, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assistant Manager-Production" },
                    { 54, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Data Entry Operator" },
                    { 112, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "L2 Support Engineer" },
                    { 55, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deputy Manager" },
                    { 57, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deputy Manager Warehouse" },
                    { 87, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Factory Head" },
                    { 88, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Factory Manager" },
                    { 89, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Field Boy" },
                    { 90, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Field Executive" },
                    { 91, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Filling Operator" },
                    { 92, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fork lift Operator" },
                    { 93, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Front Office Executive" },
                    { 94, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "General Manager" },
                    { 95, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "General Manager SCM Procurement And Marketing" },
                    { 96, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Graphics Designer" },
                    { 97, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Head BD" },
                    { 86, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-Warehouse" },
                    { 98, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Head BT" },
                    { 100, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Helper" },
                    { 101, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Housekeeping Supervisor" },
                    { 102, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR Generalist" },
                    { 103, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR Operations Executive" },
                    { 104, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR Supervisior" },
                    { 105, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR Supervisor" },
                    { 106, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR Trainee" },
                    { 107, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HVAC Trainee Operator" },
                    { 108, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Internal Auditor" },
                    { 109, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inward  Combo Executive" },
                    { 110, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Inward Executive" },
                    { 99, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Head NR" },
                    { 85, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-SCM" },
                    { 84, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-Operations" },
                    { 83, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-MIS" },
                    { 58, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deputy Manager-Administration" },
                    { 59, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deputy Manager-Finance & Accounts" },
                    { 60, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deputy Manager-Legal" },
                    { 61, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deputy Manager-SCM" },
                    { 62, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Desktop Support Engineer" },
                    { 63, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Digital Marketing Executive" },
                    { 64, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Digital Marketing Manager" },
                    { 65, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Director" },
                    { 66, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Director- Galway Kart" },
                    { 67, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dispatch  Senior Executive" },
                    { 68, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dispatch Executive" },
                    { 69, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Driver" },
                    { 70, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "E Commerce Warehouse Incharge" },
                    { 71, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Editor" },
                    { 72, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Event Coordinator" },
                    { 73, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive - Distributor Support" },
                    { 74, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive - Warehouse" },
                    { 75, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive Assistant" },
                    { 76, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive Assistant-Procurement & Packaging" },
                    { 77, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive Social Media" },
                    { 78, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-Administration" },
                    { 79, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-Distributor Record" },
                    { 80, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-Distributor Support" },
                    { 81, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-Finance & Accounts" },
                    { 82, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Executive-Legal" },
                    { 56, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deputy Manager - Product Development (Nutrition & Wellness)" },
                    { 225, 1, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)1, (byte)0, 1, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Web Designer" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tbl_app_setting",
                keyColumn: "pkid_setting",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_app_setting",
                keyColumn: "pkid_setting",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "tbl_bank_master",
                keyColumn: "bank_id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "tbl_company_emp_setting",
                keyColumn: "sno",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 501);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 502);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 503);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 504);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 505);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 506);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 507);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 508);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 509);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 510);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 511);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 512);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 513);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 514);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 515);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 516);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 517);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 518);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 519);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 520);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 521);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 522);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 523);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 524);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 525);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 526);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 527);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 528);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 529);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 530);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 531);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 532);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 533);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 534);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 535);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 536);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 537);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 540);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 541);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 542);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 543);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 544);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 545);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 546);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 1000);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2000);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2001);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2002);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2003);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2004);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2005);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2006);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2007);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2008);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2009);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2010);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2011);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2012);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2013);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2014);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 2999);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 3000);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 3001);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 3002);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 3003);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 3004);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 3005);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 3006);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 3007);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 3008);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 3009);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 3010);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 3011);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 3012);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 3999);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 4001);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 4002);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 4003);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 4004);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 4005);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 5000);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 5001);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 5002);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 5003);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 5004);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 5005);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 5006);

            migrationBuilder.DeleteData(
                table: "tbl_component_formula_details",
                keyColumn: "sno",
                keyValue: 5007);

            migrationBuilder.DeleteData(
                table: "tbl_department_master",
                keyColumn: "department_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_department_master",
                keyColumn: "department_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tbl_department_master",
                keyColumn: "department_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "tbl_department_master",
                keyColumn: "department_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "tbl_department_master",
                keyColumn: "department_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "tbl_department_master",
                keyColumn: "department_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "tbl_department_master",
                keyColumn: "department_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "tbl_department_master",
                keyColumn: "department_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "tbl_department_master",
                keyColumn: "department_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "tbl_department_master",
                keyColumn: "department_id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "tbl_department_master",
                keyColumn: "department_id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "tbl_department_master",
                keyColumn: "department_id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "tbl_department_master",
                keyColumn: "department_id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "tbl_designation_master",
                keyColumn: "designation_id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "tbl_document_type_master",
                keyColumn: "doc_type_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_document_type_master",
                keyColumn: "doc_type_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tbl_document_type_master",
                keyColumn: "doc_type_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "tbl_document_type_master",
                keyColumn: "doc_type_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "tbl_document_type_master",
                keyColumn: "doc_type_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "tbl_document_type_master",
                keyColumn: "doc_type_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "tbl_document_type_master",
                keyColumn: "doc_type_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "tbl_document_type_master",
                keyColumn: "doc_type_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "tbl_document_type_master",
                keyColumn: "doc_type_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "tbl_document_type_master",
                keyColumn: "doc_type_id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "tbl_employee_company_map",
                keyColumn: "sno",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_employment_type_master",
                keyColumn: "employment_type_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "tbl_grade_master",
                keyColumn: "grade_id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "tbl_leave_type",
                keyColumn: "leave_type_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_leave_type",
                keyColumn: "leave_type_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tbl_leave_type",
                keyColumn: "leave_type_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "tbl_leave_type",
                keyColumn: "leave_type_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "tbl_leave_type",
                keyColumn: "leave_type_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "tbl_leave_type",
                keyColumn: "leave_type_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "tbl_leave_type",
                keyColumn: "leave_type_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "tbl_leave_type",
                keyColumn: "leave_type_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "tbl_location_master",
                keyColumn: "location_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 301);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 302);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 303);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 304);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 305);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 306);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 401);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 402);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 403);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 404);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 501);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 502);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 503);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 504);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 505);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 506);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 507);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 508);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 509);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 510);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 511);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 512);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 513);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 514);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 515);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 601);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 602);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 603);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 604);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 605);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 606);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 607);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 701);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 702);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 703);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 704);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 705);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 706);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 707);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 708);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 709);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 710);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 711);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 712);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 713);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 714);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 715);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 716);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 717);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 718);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 719);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 720);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 721);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 722);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 801);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 802);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 901);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 902);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 904);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 905);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 906);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 907);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1101);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1102);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1103);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1104);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1105);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1106);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1107);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1108);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1109);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1110);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1111);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1112);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1113);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1114);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1115);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1201);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1301);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1302);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1303);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1304);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1305);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1306);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1307);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1308);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1309);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1310);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1311);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1312);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1313);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1314);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1315);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1316);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1317);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1401);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1402);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1403);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1404);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1405);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1406);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1407);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1408);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1409);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1410);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1411);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1412);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1413);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1414);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1415);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1416);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1417);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1418);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1419);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1420);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1421);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1422);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1423);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1424);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1425);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1426);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1427);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1428);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1429);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1430);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1431);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1432);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1433);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1434);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1435);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1436);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1437);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1438);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1439);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1440);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1441);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1442);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1443);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1444);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1445);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1501);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1502);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1503);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1601);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1602);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1603);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1607);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1608);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1609);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1610);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1611);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1612);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1613);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1614);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1615);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1616);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1641);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1642);

            migrationBuilder.DeleteData(
                table: "tbl_menu_master",
                keyColumn: "menu_id",
                keyValue: 1644);

            migrationBuilder.DeleteData(
                table: "tbl_payroll_month_setting",
                keyColumn: "payroll_month_setting_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "tbl_religion_master",
                keyColumn: "religion_id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100107);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 10100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 10100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 10100103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 10200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 10200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 10300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 10300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 10300103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20100102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20100103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20100104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20100105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20100106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20200103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20300103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20400103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 20500103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30100102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30100103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30200102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30200103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30300103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30300105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30400103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30500103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30600001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30600101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 30600103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40100103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40100104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40100105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40200103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40200105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40300104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40300105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40400104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 40400105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50100011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50100012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50100013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50100021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50100105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50200105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50300011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50300012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50300013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50300021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50300105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50400011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50400012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50400013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50400105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50500105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50600001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50600011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50600012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50600013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50600021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50600101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50600105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50700001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50700011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50700012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50700013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50700101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50700105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50800001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50800101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50800105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50900001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50900011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50900012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50900013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50900021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50900101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 50900105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51000001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51000011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51000012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51000013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51000021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51000101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51000105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51100011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51100012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51100013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51100105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51200105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51300011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51300012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51300013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51300021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51300105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51400011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51400012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51400013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51400105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 51500105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60100011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60100012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60100013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60100021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60100103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60100105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60200103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60200105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60300103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60300105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60400105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60500011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60500012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60500013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60500021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60500105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60600001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60600011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60600012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60600013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60600101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60600105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60700001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60700101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 60700105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70100104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70100106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70200106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70300106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70600001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70700001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70700101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70700106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70800001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70800101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70800106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 70900001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71000001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71000101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71000102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71000104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71100102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71100104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71100107);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71200106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71300106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71400102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71400106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71500102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71500106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71600001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71600101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71600102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71600106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71700001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71700101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71700102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71700106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71800001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71800101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71800102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71800106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71900001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71900101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71900102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 71900106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 72000001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 72000101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 72000102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 72000106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 72100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 72100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 72100102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 72100106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 72200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 72200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 72200102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 72200106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 80100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 80100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 80100102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 80100106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 80200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 80200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 80200102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 80200106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90200102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90200103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90400102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90400103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90500011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90500012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90500013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90500021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90500102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90600001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90600011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90600012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90600013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90600101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90600102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90700001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90700101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 90700102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100100011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100100012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100100013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100100021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100100102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100100103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100100104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100200102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100200103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100300011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100300012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100300013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100300021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100300102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100300103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100400011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100400012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100400013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100400102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 100400103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110100103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110100104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110200103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110200104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110300103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110300104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110400103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110400104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110500103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110500104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110600001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110600101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110600103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110600104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110700001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110700101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110700103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110700104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110800001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110800101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110800103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110800104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110900001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110900101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110900103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 110900104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111000001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111000101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111000103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111000104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111100103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111100104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111200103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111200104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111300103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111300104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111400103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111400104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111500103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 111500104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 120100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 120100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130100011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130100012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130100013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130100021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130100103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130100104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130200012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130200103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130200104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130300011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130300012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130300013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130600001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130600101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130700001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130700101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130800001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130800101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130900001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 130900101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131000001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131000011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131000012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131000013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131000101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131100011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131100012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131100013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131400011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131400012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131400013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131500011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131500012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131500013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131600001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131600011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131600012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131600013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131600101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131700001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131700011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131700012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131700013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131700021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 131700101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140100011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140100012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140100013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140100021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140100102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140100103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140100104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140100105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140100106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140200103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140300103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140400103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140500104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140500105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140600001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140600101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140600102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140600103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140600104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140700001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140700101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140700102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140700103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140700104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140700105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140700106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140800001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140800101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140800102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140800103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140800104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140900001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140900011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140900012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140900013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140900021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140900101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140900105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 140900106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141000001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141000012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141000101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141000105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141100011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141100012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141100013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141100021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141100105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141200011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141200012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141200013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141200021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141200105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141300011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141300012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141300013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141300021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141300105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141400011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141400012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141400013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141400021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141400105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141500011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141500012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141500013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141500021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141500105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141600001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141600011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141600012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141600013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141600101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141600105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141600106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141700001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141700011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141700012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141700013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141700101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141700105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141800001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141800011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141800012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141800013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141800101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141800105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141900001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141900011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141900012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141900013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141900101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 141900105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142000001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142000011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142000012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142000013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142000101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142000105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142100011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142100012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142100013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142100105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142200011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142200012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142200013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142200105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142300011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142300012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142300013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142300105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142400011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142400012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142400013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142400021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142400105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142500011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142500012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142500013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142500021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142500105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142600001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142600011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142600012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142600013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142600021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142600101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142600105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142700001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142700011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142700012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142700013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142700021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142700101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142700102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142700106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142800001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142800101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142800102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142800106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142900001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142900101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142900102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 142900106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143000001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143000101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143000102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143000106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143100102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143100106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143200102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143200106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143300102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143300106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143400102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143400106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143500102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143500106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143600001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143600011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143600012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143600013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143600021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143600101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143600102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143600106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143700001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143700101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143700102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143700106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143800001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143800101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143800102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143800106);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143900001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143900011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143900012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143900013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143900021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143900101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143900102);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143900103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 143900104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144000001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144000011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144000012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144000013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144000021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144000101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144000104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144100011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144100012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144100013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144100021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144100104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144200011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144200012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144200013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144200021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144300011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144300012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144300013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144300021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144400011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144400012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144400013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144400105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144500011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144500012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144500021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 144500104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 150100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 150100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 150200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 150200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 150300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 150300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160100011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160100012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160100021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160100105);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160200011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160200012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160200013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160200104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160300104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160700001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160700101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160800001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160800101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160900001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 160900101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161000001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161000101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161100103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161100104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161200103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161200104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161300001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161300101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161300103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161300104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161400103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161400104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161500001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161500101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161500103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161500104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161600001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161600101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161600103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 161600104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164100001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164100011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164100012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164100013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164100021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164100101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164100103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164100104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164200001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164200011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164200012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164200013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164200021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164200101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164200103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164200104);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164400001);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164400011);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164400012);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164400013);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164400021);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164400101);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164400103);

            migrationBuilder.DeleteData(
                table: "tbl_role_menu_master",
                keyColumn: "role_menu_id",
                keyValue: 164400104);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 501);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 502);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 503);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 504);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 505);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 506);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 507);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 508);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 509);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 510);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 511);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 512);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 513);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 514);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 515);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 516);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 517);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 518);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 519);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 520);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 521);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 522);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 523);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 524);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 525);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 526);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 527);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 528);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 529);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 530);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 531);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 532);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 533);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 534);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 535);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 536);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 537);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 538);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 539);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 540);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 541);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 542);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 543);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 544);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 545);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 546);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 547);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 548);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 549);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 550);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 551);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 552);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 553);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 554);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 555);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 556);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 557);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 558);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 559);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 560);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 561);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 562);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 563);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 564);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 565);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 566);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 567);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 568);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 569);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 570);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 571);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 572);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 573);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 574);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 575);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 576);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 577);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 578);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 579);

            migrationBuilder.DeleteData(
                table: "tbl_rpt_title_master",
                keyColumn: "title_id",
                keyValue: 580);

            migrationBuilder.DeleteData(
                table: "tbl_user_role_map",
                keyColumn: "claim_master_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "tbl_working_role",
                keyColumn: "working_role_id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "tbl_company_master",
                keyColumn: "company_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 501);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 502);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 503);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 504);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 505);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 506);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 507);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 508);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 509);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 510);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 511);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 512);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 513);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 514);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 515);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 516);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 517);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 518);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 519);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 520);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 521);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 522);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 523);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 524);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 525);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 526);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 527);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 528);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 529);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 530);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 531);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 532);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 533);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 534);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 535);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 536);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 537);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 540);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 541);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 542);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 543);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 544);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 545);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 546);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 1000);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2000);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2001);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2002);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2003);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2004);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2005);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2006);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2007);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2008);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2009);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2010);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2011);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2012);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2013);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2014);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 2999);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 3000);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 3001);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 3002);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 3003);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 3004);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 3005);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 3006);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 3007);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 3008);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 3009);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 3010);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 3011);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 3012);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 3999);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 4001);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 4002);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 4003);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 4004);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 4005);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 5000);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 5001);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 5002);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 5003);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 5004);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 5005);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 5006);

            migrationBuilder.DeleteData(
                table: "tbl_component_master",
                keyColumn: "component_id",
                keyValue: 5007);

            migrationBuilder.DeleteData(
                table: "tbl_report_master",
                keyColumn: "rpt_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_report_master",
                keyColumn: "rpt_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tbl_role_master",
                keyColumn: "role_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_salary_group",
                keyColumn: "group_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "tbl_user_master",
                keyColumn: "user_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_city",
                keyColumn: "city_id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "tbl_employee_master",
                keyColumn: "employee_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_state",
                keyColumn: "state_id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "tbl_country",
                keyColumn: "country_id",
                keyValue: 1);
        }
    }
}

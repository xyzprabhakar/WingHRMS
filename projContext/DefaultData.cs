using Microsoft.EntityFrameworkCore;
using projContext.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace projContext
{
    public partial class Context
    {
        private DateTime CurrentDate = new DateTime(2021, 1, 1);
        public void InsertCountryMaster(ModelBuilder modelBuilder)
        {
            tbl_country countryData = new tbl_country() { 
                country_id=1,                
                sort_name="IN",
                name="India",
                is_deleted=0,
                created_by = 1,
                created_date = CurrentDate,
                last_modified_by=1,
                last_modified_date= CurrentDate,
            };
            modelBuilder.Entity<tbl_country>().HasData(countryData);
            //insert into the state
            List<tbl_state> all_states = new List<tbl_state>();
            all_states.Add(new tbl_state() { country_id = 1, state_id = 1, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Andaman and Nicobar Islands" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 2, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Andhra Pradesh" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 3, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Arunachal Pradesh" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 4, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Assam" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 5, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Bihar" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 6, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Chandigarh" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 7, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Chhattisgarh" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 8, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Dadra and Nagar Haveli" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 9, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Daman and Diu" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 10, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Delhi" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 11, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Goa" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 12, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Gujarat" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 13, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Haryana" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 14, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Himachal Pradesh" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 15, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Jammu and Kashmir" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 16, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Jharkhand" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 17, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Karnataka" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 18, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Kerala" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 19, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Ladakh" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 20, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Lakshadweep" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 21, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Madhya Pradesh" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 22, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Maharashtra" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 23, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Manipur" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 24, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Meghalaya" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 25, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Mizoram" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 26, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Nagaland" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 27, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Odisha" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 28, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Puducherry" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 29, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Punjab" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 30, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Rajasthan" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 31, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Sikkim" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 32, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Tamil Nadu" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 33, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Telangana" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 34, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Tripura" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 35, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Uttar Pradesh" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 36, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "Uttarakhand" });
            all_states.Add(new tbl_state() { country_id = 1, state_id = 37, created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, name = "West Bengal" });
            modelBuilder.Entity<tbl_state>().HasData(all_states);
            List<tbl_city> cityData = new List<tbl_city>();            
            for (int i = 0; i < 37; i++)
            {
                cityData.Add(new tbl_city() { city_id = i + 1, name = "", created_by = 1, created_date = CurrentDate, is_deleted = 0, last_modified_by = 1, last_modified_date = CurrentDate, pincode = "000001", state_id = (i + 1) });
            }
            modelBuilder.Entity<tbl_city>().HasData(cityData);
        }

        public void InsertCompany(ModelBuilder modelBuilder)
        {
            tbl_company_master company_Master = new tbl_company_master()
            {
                company_id = 1,
                company_name = "Travolook",
                address_line_one = "836, 2nd floor, Udyog Vihar Phase V, Sector 19",
                address_line_two= "Gurugram",
                city_id=13,
                state_id=13,
                country_id=1,
                company_code="TR",
                company_website = "https://www.travolook.in/",
                is_emp_code_manual_genrate=1,
                company_logo=null,
                primary_email_id = "support@travolook.in",
                secondary_email_id = null,
                primary_contact_number = "01246434800",
                secondary_contact_number = null,                
                is_active = 1,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                created_by=1,
                created_date= CurrentDate,
                pin_code= 122008,
                total_emp=1,
                user_type=1
            };
            modelBuilder.Entity<tbl_company_master>().HasData(company_Master);
            tbl_company_emp_setting empSetting = new tbl_company_emp_setting()
            {
                sno = 1,
                company_id = 1,
                prefix_for_employee_code = "TR",
                number_of_character_for_employee_code = 8,
                from_range = 1,
                current_range = 1,
                to_range = 1,
                is_active = 1,
                last_genrated = CurrentDate,

            };
            modelBuilder.Entity<tbl_company_emp_setting>().HasData(empSetting);
        }

        public void InsertLocation(ModelBuilder modelBuilder)
        {
            tbl_location_master Master = new tbl_location_master()
            {
                location_id=1,
                company_id = 1,
                location_code = "HO",
                location_name="Head Office",
                image=null,
                address_line_one = "836, 2nd floor, Udyog Vihar Phase V, Sector 19",
                address_line_two = "Gurugram",
                city_id = 13,
                state_id = 13,
                country_id = 1,
                primary_email_id = "support@travolook.in",
                secondary_email_id = null,
                primary_contact_number = "01246434800",
                secondary_contact_number = null,
                is_active = 1,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                created_by = 1,
                created_date = CurrentDate,
                pin_code = 122008,
                type_of_location= 1,                
            };
            modelBuilder.Entity<tbl_location_master>().HasData(Master);
            
        }

        public void InsertDepartmentMaster(ModelBuilder modelBuilder)
        {
            List<tbl_department_master> departments = new List<tbl_department_master>();
            departments.Add(new tbl_department_master()
            {
                department_id = 1,
                company_id = 1,
                department_code = "Admin",
                department_name = "Admin",
                department_short_name = "Admin",
                created_by = 1,
                created_date = CurrentDate,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                department_head_employee_name = string.Empty,
                department_head_employee_code = string.Empty,
                employee_id = null,
                is_active = 1
            });
            departments.Add(new tbl_department_master()
            {
                department_id = 2,
                company_id = 1,
                department_code = "Finance",
                department_name = "Finance",
                department_short_name = "Finance",
                created_by = 1,
                created_date = CurrentDate,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                department_head_employee_name = string.Empty,
                department_head_employee_code = string.Empty,
                employee_id = null,
                is_active = 1
            });
            departments.Add(new tbl_department_master()
            {
                department_id = 3,
                company_id = 1,
                department_code = "HR",
                department_name = "Human Resource",
                department_short_name = "HR",
                created_by = 1,
                created_date = CurrentDate,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                department_head_employee_name = string.Empty,
                department_head_employee_code = string.Empty,
                employee_id = null,
                is_active = 1
            });
            departments.Add(new tbl_department_master()
            {
                department_id = 4,
                company_id = 1,
                department_code = "Marketing",
                department_name = "Marketing",
                department_short_name = "Marketing",
                created_by = 1,
                created_date = CurrentDate,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                department_head_employee_name = string.Empty,
                department_head_employee_code = string.Empty,
                employee_id = null,
                is_active = 1
            });
            departments.Add(new tbl_department_master()
            {
                department_id = 5,
                company_id = 1,
                department_code = "Operations",
                department_name = "Operations",
                department_short_name = "Operations",
                created_by = 1,
                created_date = CurrentDate,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                department_head_employee_name = string.Empty,
                department_head_employee_code = string.Empty,
                employee_id = null,
                is_active = 1
            });
            departments.Add(new tbl_department_master()
            {
                department_id = 6,
                company_id = 1,
                department_code = "Purchase",
                department_name = "Purchase",
                department_short_name = "Purchase",
                created_by = 1,
                created_date = CurrentDate,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                department_head_employee_name = string.Empty,
                department_head_employee_code = string.Empty,
                employee_id = null,
                is_active = 1
            });
            departments.Add(new tbl_department_master()
            {
                department_id = 7,
                company_id = 1,
                department_code = "Sales",
                department_name = "Sales",
                department_short_name = "Sales",
                created_by = 1,
                created_date = CurrentDate,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                department_head_employee_name = string.Empty,
                department_head_employee_code = string.Empty,
                employee_id = null,
                is_active = 1
            });
            departments.Add(new tbl_department_master()
            {
                department_id = 8,
                company_id = 1,
                department_code = "IT Infra",
                department_name = "IT Infra",
                department_short_name = "IT Infra",
                created_by = 1,
                created_date = CurrentDate,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                department_head_employee_name = string.Empty,
                department_head_employee_code = string.Empty,
                employee_id = null,
                is_active = 1
            });
            departments.Add(new tbl_department_master()
            {
                department_id = 9,
                company_id = 1,
                department_code = "IT-SW",
                department_name = "IT Software",
                department_short_name = "IT Software",
                created_by = 1,
                created_date = CurrentDate,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                department_head_employee_name = string.Empty,
                department_head_employee_code = string.Empty,
                employee_id = null,
                is_active = 1
            });
            //addition Department for the company

            departments.Add(new tbl_department_master()
            {
                department_id = 20,
                company_id = 1,
                department_code = "Accounts",
                department_name = "Accounts",
                department_short_name = "Accounts",
                created_by = 1,
                created_date = CurrentDate,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                department_head_employee_name = string.Empty,
                department_head_employee_code = string.Empty,
                employee_id = null,
                is_active = 1
            });
            departments.Add(new tbl_department_master()
            {
                department_id = 21,
                company_id = 1,
                department_code = "Customer Services",
                department_name = "Customer Services",
                department_short_name = "Customer Services",
                created_by = 1,
                created_date = CurrentDate,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                department_head_employee_name = string.Empty,
                department_head_employee_code = string.Empty,
                employee_id = null,
                is_active = 1
            });
            departments.Add(new tbl_department_master()
            {
                department_id = 22,
                company_id = 1,
                department_code = "Support",
                department_name = "Support",
                department_short_name = "Support",
                created_by = 1,
                created_date = CurrentDate,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                department_head_employee_name = string.Empty,
                department_head_employee_code = string.Empty,
                employee_id = null,
                is_active = 1
            });

            departments.Add(new tbl_department_master()
            {
                department_id = 23,
                company_id = 1,
                department_code = "Exchange",
                department_name = "Exchange",
                department_short_name = "Exchange",
                created_by = 1,
                created_date = CurrentDate,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                department_head_employee_name = string.Empty,
                department_head_employee_code = string.Empty,
                employee_id = null,
                is_active = 1
            });
            modelBuilder.Entity<tbl_department_master>().HasData(departments);

        }


        public void InsertAppSetting(ModelBuilder modelBuilder)
        {
            List<tbl_app_setting> Masters = new List<tbl_app_setting>();

            Masters.Add(new tbl_app_setting()
            {

                pkid_setting = 1,
                AppSettingKey = "attandance_application_freezed_for_Emp",
                AppSettingKeyDisplay = "EMPLOYEE ATTENDENCE FREEZE",
                AppSettingValue = "false",
                is_active = 1,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                created_by = 1,
                created_dt = CurrentDate
            });
            Masters.Add(new tbl_app_setting()
            {

                pkid_setting = 2,
                AppSettingKey = "attandance_application_freezed__for_Admin",
                AppSettingKeyDisplay = "ADMIN ATTENDENCE FREEZE",
                AppSettingValue = "false",
                is_active = 1,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                created_by = 1,
                created_dt = CurrentDate
            });
            modelBuilder.Entity<tbl_app_setting>().HasData(Masters);

            tbl_payroll_month_setting payrollsetting = new tbl_payroll_month_setting()
            {
                payroll_month_setting_id = 1,
                from_month = 202103,
                from_date = 1,
                applicable_to_date = 20230331,
                is_deleted = 0,
                created_by = 1,
                created_date = CurrentDate,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                company_id = 1
            };
            modelBuilder.Entity<tbl_payroll_month_setting>().HasData(payrollsetting);

        }

        public void InsertBanksMaster(ModelBuilder modelBuilder)
        {
            List<tbl_bank_master> Masters = new List<tbl_bank_master>();
            Masters.Add(new tbl_bank_master() { bank_id = 1, bank_name = "BankName", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 2, bank_name = "American Express", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 3, bank_name = "ANZ Grindlays", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 4, bank_name = "Bank of America", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 5, bank_name = "Bank of Nova Scotia", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 6, bank_name = "Bank of Tokyo", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 7, bank_name = "Banque Nationale de Paris", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 8, bank_name = "Citibank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 9, bank_name = "Credit Lyonnais", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 10, bank_name = "Deutsche bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 11, bank_name = "Hong Kong & Shanghai", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 12, bank_name = "Standard Chartered", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 13, bank_name = "Societe Generale", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 14, bank_name = "Sanwa Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 15, bank_name = "Allahabad Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 16, bank_name = "Andhra Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 17, bank_name = "Bank of Baroda", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 18, bank_name = "Bank of Maharashtra", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 19, bank_name = "Canara Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 20, bank_name = "Central Bank of India", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 21, bank_name = "Dena Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 22, bank_name = "Indian Overseas Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 23, bank_name = "Punjab National Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 24, bank_name = "State Bank of India", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 25, bank_name = "State Bank of Bikaner & Jaipur", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 26, bank_name = "State Bank of Mysore", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 27, bank_name = "State Bank of Hyderabad", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 28, bank_name = "UCO Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 29, bank_name = "United Bank of India", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 30, bank_name = "Vijaya Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 31, bank_name = "Private Banks in Delhi NCR:-", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 32, bank_name = "HDFC Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 33, bank_name = "ICICI Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 34, bank_name = "IDBI Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 35, bank_name = "Axis Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 36, bank_name = "Syndicate Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 37, bank_name = "Lord Krishna Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            Masters.Add(new tbl_bank_master() { bank_id = 38, bank_name = "IndusInd Bank", bank_status = 1, created_by = 1, created_dt = CurrentDate, deleted_by = 1, is_deleted = 0, modified_by = 1, modified_dt = CurrentDate });
            modelBuilder.Entity<tbl_bank_master>().HasData(Masters);

        }

        public void insertDesignation(ModelBuilder modelBuilder)
        {
            List<tbl_designation_master> datas = new List<tbl_designation_master>();
            datas.Add(new tbl_designation_master() { designation_id = 1, designation_name = "Account Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 2, designation_name = "Admin Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 3, designation_name = "Admin Incharge", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 4, designation_name = "Admin Supervisor", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 5, designation_name = "Agronomist", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 6, designation_name = "AM Warehouse account operation", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 7, designation_name = "Application Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 8, designation_name = "Application Manager  Android App Developer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 9, designation_name = "Application Support Engineer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 10, designation_name = "Area Supervisor", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 11, designation_name = "Asistant Team Leader", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 12, designation_name = "Assistant Executive-Commercial", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 13, designation_name = "Assistant General Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 14, designation_name = "Assistant Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 15, designation_name = "Assistant Manager - QC", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 16, designation_name = "Assistant Manager SCM", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 17, designation_name = "Assistant Manager-Accounts", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 18, designation_name = "Assistant Manager-Administration", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 19, designation_name = "Assistant Manager-Distributor Record", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 20, designation_name = "Assistant Manager-Event", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 21, designation_name = "Assistant Manager-Finance & Accounts", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 22, designation_name = "Assistant Manager-Human Resource", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 23, designation_name = "Assistant Manager-Legal", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 24, designation_name = "Assistant Manager-Operations", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 25, designation_name = "Assistant Manager-Production", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 26, designation_name = "Assistant Manager-QA", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 27, designation_name = "Assistant Manager-QC", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 28, designation_name = "Assistant Manager-SCM", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 29, designation_name = "Assistant Manager-Store", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 30, designation_name = "Assistant Manager-Utility & Maintenance", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 31, designation_name = "Assistant Manager-Warehouse", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 32, designation_name = "Assistant Supervisor", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 33, designation_name = "Assistant Supervisor-Administration (Security and Labour)", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 34, designation_name = "Assistant Team Leader", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 35, designation_name = "Associate Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 36, designation_name = "Associate Executive - Warehouse", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 37, designation_name = "Associate Executive Warehouse", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 38, designation_name = "Audit Officer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 39, designation_name = "BDT Coordinator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 40, designation_name = "Billing", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 41, designation_name = "Bottle and Jar Filling Operator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 42, designation_name = "Business Analyst", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 43, designation_name = "Business Analyst- Senior Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 44, designation_name = "Business Data Analyst", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 45, designation_name = "Business Development Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 46, designation_name = "Category Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 47, designation_name = "Chief Executive Officer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 48, designation_name = "Combo Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 49, designation_name = "Complaint Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 50, designation_name = "Compliance Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 51, designation_name = "Consultant", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 52, designation_name = "Content Writer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 53, designation_name = "Customer Care Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 54, designation_name = "Data Entry Operator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 55, designation_name = "Deputy Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 56, designation_name = "Deputy Manager - Product Development (Nutrition & Wellness)", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 57, designation_name = "Deputy Manager Warehouse", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 58, designation_name = "Deputy Manager-Administration", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 59, designation_name = "Deputy Manager-Finance & Accounts", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 60, designation_name = "Deputy Manager-Legal", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 61, designation_name = "Deputy Manager-SCM", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 62, designation_name = "Desktop Support Engineer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 63, designation_name = "Digital Marketing Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 64, designation_name = "Digital Marketing Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 65, designation_name = "Director", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 66, designation_name = "Director- Galway Kart", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 67, designation_name = "Dispatch  Senior Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 68, designation_name = "Dispatch Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 69, designation_name = "Driver", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 70, designation_name = "E Commerce Warehouse Incharge", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 71, designation_name = "Editor", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 72, designation_name = "Event Coordinator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 73, designation_name = "Executive - Distributor Support", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 74, designation_name = "Executive - Warehouse", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 75, designation_name = "Executive Assistant", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 76, designation_name = "Executive Assistant-Procurement & Packaging", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 77, designation_name = "Executive Social Media", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 78, designation_name = "Executive-Administration", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 79, designation_name = "Executive-Distributor Record", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 80, designation_name = "Executive-Distributor Support", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 81, designation_name = "Executive-Finance & Accounts", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 82, designation_name = "Executive-Legal", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 83, designation_name = "Executive-MIS", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 84, designation_name = "Executive-Operations", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 85, designation_name = "Executive-SCM", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 86, designation_name = "Executive-Warehouse", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 87, designation_name = "Factory Head", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 88, designation_name = "Factory Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 89, designation_name = "Field Boy", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 90, designation_name = "Field Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 91, designation_name = "Filling Operator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 92, designation_name = "Fork lift Operator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 93, designation_name = "Front Office Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 94, designation_name = "General Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 95, designation_name = "General Manager SCM Procurement And Marketing", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 96, designation_name = "Graphics Designer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 97, designation_name = "Head BD", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 98, designation_name = "Head BT", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 99, designation_name = "Head NR", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 100, designation_name = "Helper", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 101, designation_name = "Housekeeping Supervisor", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 102, designation_name = "HR Generalist", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 103, designation_name = "HR Operations Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 104, designation_name = "HR Supervisior", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 105, designation_name = "HR Supervisor", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 106, designation_name = "HR Trainee", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 107, designation_name = "HVAC Trainee Operator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 108, designation_name = "Internal Auditor", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 109, designation_name = "Inward  Combo Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 110, designation_name = "Inward Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 111, designation_name = "Joining and  Billing", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 112, designation_name = "L2 Support Engineer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 113, designation_name = "Lab Assistant", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 114, designation_name = "Labelling and Compression Operator and  Bottle Filling", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 115, designation_name = "Labelling Machine Operator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 116, designation_name = "Maintenance Technician", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 117, designation_name = "Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 118, designation_name = "Manager - Legal & Company Secretary", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 119, designation_name = "Manager-Finance & Accounts", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 120, designation_name = "Managing Director", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 121, designation_name = "Manufacturing Operator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 122, designation_name = "Marketing Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 123, designation_name = "Mfg. Chemist", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 124, designation_name = "MIS Data Analyst", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 125, designation_name = "MIS Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 126, designation_name = "MIS Senior Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 127, designation_name = "Mobile App. Developer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 128, designation_name = "National Head", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 129, designation_name = "National Head-Sales & Training", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 130, designation_name = "Network Security Engineer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 131, designation_name = "NRD Coordinator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 132, designation_name = "Office Boy", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 133, designation_name = "Operation Assistant", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 134, designation_name = "Operator Electrical", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 135, designation_name = "Operator Filling", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 136, designation_name = "Operator Labelling", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 137, designation_name = "Operator Manufacturing", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 138, designation_name = "Operator Mechanical", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 139, designation_name = "Packaging Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 140, designation_name = "Packaging Supervisor", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 141, designation_name = "Packing Assistant", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 142, designation_name = "Pantry Boy", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 143, designation_name = "Plant HR", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 144, designation_name = "Procurement Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 145, designation_name = "Product Trainer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 146, designation_name = "Production Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 147, designation_name = "Production Supervisor", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 148, designation_name = "Project Coordinator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 149, designation_name = "Project Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 150, designation_name = "Project Manager - CSR", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 151, designation_name = "QA Chemist", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 152, designation_name = "QC Assistant", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 153, designation_name = "QC Chemist", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 154, designation_name = "QC Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 155, designation_name = "QC Officer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 156, designation_name = "Quality Analyst", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 157, designation_name = "Quality Auditor", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 158, designation_name = "R&D Chemist", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 159, designation_name = "Raw material and Personal ingerient care executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 160, designation_name = "Regional Executive Trainer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 161, designation_name = "Relationship Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 162, designation_name = "Sales Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 163, designation_name = "SCM Coordinator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 164, designation_name = "Section Head- CRD & DST", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 165, designation_name = "Section Head- Operations", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 166, designation_name = "Security Guard", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 167, designation_name = "Senior Agronomist", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 168, designation_name = "Senior Audit Officer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 169, designation_name = "Senior Executive- Finance & Accounts", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 170, designation_name = "Senior Executive- Operations", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 171, designation_name = "Senior Executive Warehouse", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 172, designation_name = "Senior Executive- Warehouse", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 173, designation_name = "Senior Executive-Distributor Record", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 174, designation_name = "Senior Executive-MIS", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 175, designation_name = "Senior Executive-SCM", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 176, designation_name = "Senior General Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 177, designation_name = "Senior Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 178, designation_name = "Senior Manager - Finance & Accounts", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 179, designation_name = "Senior Manager Purchase and Procurement", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 180, designation_name = "Senior Manager-SCM", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 181, designation_name = "Senior Operation Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 182, designation_name = "Senior R&D Chemist", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 183, designation_name = "Senior Software Developer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 184, designation_name = "Senior Visualizer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 185, designation_name = "SEO-Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 186, designation_name = "Software Developer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 187, designation_name = "Software Tester", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 188, designation_name = "Sr. Assistant Supervisor", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 189, designation_name = "Sr. Assistant-Operation", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 190, designation_name = "Sr. Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 191, designation_name = "Sr. Executive - Procurement & Packaging", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 192, designation_name = "Sr. Executive -Procurement RM & CM Audit", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 193, designation_name = "Sr. Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 194, designation_name = "Sr. Supervisor-Administration", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 195, designation_name = "Sr. Supervisor-Inward", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 196, designation_name = "Store Assistant", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 197, designation_name = "Store Attendent", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 198, designation_name = "Store Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 199, designation_name = "Store Helper", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 200, designation_name = "Store Incharge", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 201, designation_name = "Store Officer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 202, designation_name = "Store Supervisor", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 203, designation_name = "Sub Editor", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 204, designation_name = "Supervisor-Combo", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 205, designation_name = "Supervisor-Packaging", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 206, designation_name = "System Administrator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 207, designation_name = "Talent Acquisition Specialist", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 208, designation_name = "Tally Administrator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 209, designation_name = "Team Coordinator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 210, designation_name = "Team Leader", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 211, designation_name = "Technical Head", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 212, designation_name = "TECHNOLOGY OFFICER", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 213, designation_name = "Trainee Electrician", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 214, designation_name = "Trainee Engineer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 215, designation_name = "Training Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 216, designation_name = "Tube and Jar Filling Operator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 217, designation_name = "UI Developer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 218, designation_name = "Utility & Maintenance Head", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 219, designation_name = "Utility Operator", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 220, designation_name = "VCM Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 221, designation_name = "Video Editor", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 222, designation_name = "Warehouse Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 223, designation_name = "Warehouse Incharge", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 224, designation_name = "Warehouse Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_designation_master() { designation_id = 225, designation_name = "Web Designer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });


            List<tbl_working_role> workRole = datas.Select(p => new tbl_working_role { company_id = 1, dept_id = null, is_active = 1, created_by = 1, created_date = p.created_date, last_modified_by = p.last_modified_by, last_modified_date = p.last_modified_date, is_default = 0, working_role_id = p.designation_id, working_role_name = p.designation_name }).ToList();

            modelBuilder.Entity<tbl_designation_master>().HasData(datas);
            modelBuilder.Entity<tbl_working_role>().HasData(workRole);
        }

        public void InsertDocumentTypemaster(ModelBuilder modelBuilder)
        {
            List<tbl_document_type_master> datas = new List<tbl_document_type_master>();
            datas.Add(new tbl_document_type_master() { doc_type_id = 1, doc_name = "Adhar", created_by = 1, created_date = CurrentDate, modified_by = 1, modified_date= CurrentDate, is_deleted = 0 ,company_id=1,remarks=string.Empty});
            datas.Add(new tbl_document_type_master() { doc_type_id = 2, doc_name = "Pan", created_by = 1, created_date = CurrentDate, modified_by = 1, modified_date = CurrentDate, is_deleted = 0, company_id = 1, remarks = string.Empty });
            datas.Add(new tbl_document_type_master() { doc_type_id = 3, doc_name = "Passport", created_by = 1, created_date = CurrentDate, modified_by = 1, modified_date = CurrentDate, is_deleted = 0, company_id = 1, remarks = string.Empty });
            datas.Add(new tbl_document_type_master() { doc_type_id = 4, doc_name = "Voter Card", created_by = 1, created_date = CurrentDate, modified_by = 1, modified_date = CurrentDate, is_deleted = 0, company_id = 1, remarks = string.Empty });
            datas.Add(new tbl_document_type_master() { doc_type_id = 5, doc_name = "Rent Agrement", created_by = 1, created_date = CurrentDate, modified_by = 1, modified_date = CurrentDate, is_deleted = 0, company_id = 1, remarks = string.Empty });
            datas.Add(new tbl_document_type_master() { doc_type_id = 6, doc_name = "Electricity Bill", created_by = 1, created_date = CurrentDate, modified_by = 1, modified_date = CurrentDate, is_deleted = 0, company_id = 1, remarks = string.Empty });
            datas.Add(new tbl_document_type_master() { doc_type_id = 7, doc_name = "Secondary Certificate", created_by = 1, created_date = CurrentDate, modified_by = 1, modified_date = CurrentDate, is_deleted = 0, company_id = 1, remarks = string.Empty });
            datas.Add(new tbl_document_type_master() { doc_type_id = 8, doc_name = "Graduation", created_by = 1, created_date = CurrentDate, modified_by = 1, modified_date = CurrentDate, is_deleted = 0, company_id = 1, remarks = string.Empty });
            datas.Add(new tbl_document_type_master() { doc_type_id = 9, doc_name = "Post Graduation", created_by = 1, created_date = CurrentDate, modified_by = 1, modified_date = CurrentDate, is_deleted = 0, company_id = 1, remarks = string.Empty });
            datas.Add(new tbl_document_type_master() { doc_type_id = 10, doc_name = "Diploma", created_by = 1, created_date = CurrentDate, modified_by = 1, modified_date = CurrentDate, is_deleted = 0, company_id = 1, remarks = string.Empty });
            modelBuilder.Entity<tbl_document_type_master>().HasData(datas);
        }

        public void InsertGradeMaster(ModelBuilder modelBuilder)
        {
            List<tbl_grade_master> datas = new List<tbl_grade_master>();
            datas.Add(new tbl_grade_master() { grade_id = 1, grade_name = "Supervisor", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 2, grade_name= "Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 3, grade_name= "Assistant", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 4, grade_name= "Assistant Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 5, grade_name= "NA", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 6, grade_name= "Associate Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 7, grade_name= "Senior Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 8, grade_name= "Director", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 9, grade_name = "Deputy Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 10, grade_name= "Assistant General Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 11, grade_name= "Senior Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 12, grade_name= "Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 13, grade_name= "General Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 14, grade_name= "Senior Assistant", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 15, grade_name= "Chief Executive Officer", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 16, grade_name= "Managing Director", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 17, grade_name= "Senior General Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 18, grade_name= "Trainee", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 19, grade_name= "Sr. Executive", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 20, grade_name= "Field Boy", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_grade_master() { grade_id = 21, grade_name = "Sr. Manager", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            modelBuilder.Entity<tbl_grade_master>().HasData(datas);
        }

        public void InsertLeaveType(ModelBuilder modelBuilder)
        {
            List<tbl_leave_type> datas = new List<tbl_leave_type>();
            datas.Add(new tbl_leave_type() { leave_type_id = 1, leave_type_name = "CL",description= "Casual Leave", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_leave_type() { leave_type_id = 2, leave_type_name = "EL", description = "Earned Leave", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_leave_type() { leave_type_id = 3, leave_type_name = "SHL", description = "Short leave Leave", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_leave_type() { leave_type_id = 4, leave_type_name = "Comp Off", description = "Compensatory Off", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_leave_type() { leave_type_id = 5, leave_type_name = "MtL", description = "Maternity Leave", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_leave_type() { leave_type_id = 6, leave_type_name = "Ptl", description = "Paternity Leave", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_leave_type() { leave_type_id = 7, leave_type_name = "BL", description = "Bereavement Leave", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_leave_type() { leave_type_id = 8, leave_type_name = "LWP", description = "Leave Without Pay", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            
            modelBuilder.Entity<tbl_leave_type>().HasData(datas);
        }

        public void InsertReligionType(ModelBuilder modelBuilder)
        {
            List<tbl_religion_master> datas = new List<tbl_religion_master>();
            datas.Add(new tbl_religion_master() { religion_id = 1, religion_name = "Bahai ",  created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 2, religion_name = "Buddhism",  created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 3, religion_name = "Cao Dai", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 4, religion_name = "Chinese", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 5, religion_name = "Christianity", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 6, religion_name = "Hinduism", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 7, religion_name = "Islam", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 8, religion_name = "Juche", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 9, religion_name = "Judaism", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 10, religion_name = "Neo-Paganism ", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 11, religion_name = "Shinto", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 12, religion_name = "Sikhism", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 13, religion_name = "Cao Dai", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 14, religion_name = "Spiritism", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 15, religion_name = "Tenrikyo", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 16, religion_name = "Unitarian-Universalism", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 17, religion_name = "Zoroastrianism", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });
            datas.Add(new tbl_religion_master() { religion_id = 100, religion_name = "Other", created_by = 1, created_date = CurrentDate, last_modified_by = 1, last_modified_date = CurrentDate, is_active = 1 });


            modelBuilder.Entity<tbl_religion_master>().HasData(datas);
        }

        public void InsertPayrolldata(ModelBuilder modelBuilder)
        {
            tbl_salary_group _Salary_Group = new tbl_salary_group()
            {
                group_id = 1,
                group_name = "SG1",
                description = string.Empty,
                minvalue = 0,
                maxvalue = 0,
                grade_Id = null,
                created_by = 1,
                created_dt = CurrentDate,
                modified_by = 1,
                modified_dt = CurrentDate,
                is_active=1
            };
            modelBuilder.Entity<tbl_salary_group>().HasData(_Salary_Group);

            List<tbl_component_master> component_Masters = new List<tbl_component_master>();
            List<tbl_component_formula_details> Formula_Details = new List<tbl_component_formula_details>();

            #region
            component_Masters.Add(new tbl_component_master()
            {
                component_id = 1,
                component_name = "@SystemComponent",
                datatype = "3",
                defaultvalue = "-",
                parentid = 0,
                is_system_key = 1,
                System_function = "",
                System_table = null,
                component_type = (int)enmComponentType.Other,
                is_salary_comp = 0,
                is_tds_comp = 0,
                is_data_entry_comp = 0,
                payment_type = 0,
                is_user_interface = 0,
                is_payslip = 0,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                modified_by = 1,
                modified_dt = new DateTime(2020, 1, 1),
                is_active = 1,
                property_details = "System Component"
            });
            component_Masters.Add(new tbl_component_master()
            {
                component_id = 1000,
                component_name = "@EmployeeSalary",
                datatype = "3",
                defaultvalue = "-",
                parentid = 0,
                is_system_key = 1,
                System_function = "",
                System_table = null,
                component_type = (int)enmComponentType.Other,
                is_salary_comp = 0,
                is_tds_comp = 0,
                is_data_entry_comp = 0,
                payment_type = 0,
                is_user_interface = 0,
                is_payslip = 0,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                modified_by = 1,
                modified_dt = new DateTime(2020, 1, 1),
                is_active = 1,
                property_details = "Employee Salary"
            });
            component_Masters.Add(new tbl_component_master()
            {
                component_id = 2000,
                component_name = "@EmployeeIncome",
                datatype = "3",
                defaultvalue = "0",
                parentid = 1000,
                is_system_key = 1,
                System_function = "",
                System_table = null,
                component_type = (int)enmComponentType.Other,
                is_salary_comp = 0,
                is_tds_comp = 0,
                is_data_entry_comp = 0,
                payment_type = 0,
                is_user_interface = 0,
                is_payslip = 0,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                modified_by = 1,
                modified_dt = new DateTime(2020, 1, 1),
                is_active = 1,
                property_details = "Employee Income"
            });
            component_Masters.Add(new tbl_component_master()
            {
                component_id = 3000,
                component_name = "@EmployeeDeduction",
                datatype = "3",
                defaultvalue = "0",
                parentid = 1000,
                is_system_key = 1,
                System_function = "",
                System_table = null,
                component_type = (int)enmComponentType.Other,
                is_salary_comp = 0,
                is_tds_comp = 0,
                is_data_entry_comp = 0,
                payment_type = 0,
                is_user_interface = 0,
                is_payslip = 0,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                modified_by = 1,
                modified_dt = new DateTime(2020, 1, 1),
                is_active = 1,
                property_details = "Employee Deduction"
            });

            Formula_Details.Add(new tbl_component_formula_details()
            {
                sno = 1,
                component_id = 1,
                company_id = 1,
                salary_group_id = 1,
                formula = "0",
                function_calling_order = 1,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                deleted_by = 1,
                deleted_dt = new DateTime(2020, 1, 1),
                is_deleted = 1,

            });
            Formula_Details.Add(new tbl_component_formula_details()
            {
                sno = 1000,
                component_id = 1000,
                company_id = 1,
                salary_group_id = 1,
                formula = "0",
                function_calling_order = 1,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                deleted_by = 1,
                deleted_dt = new DateTime(2020, 1, 1),
                is_deleted = 1,

            });
            Formula_Details.Add(new tbl_component_formula_details()
            {
                sno = 2000,
                component_id = 2000,
                company_id = 1,
                salary_group_id = 1,
                formula = "0",
                function_calling_order = 1,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                deleted_by = 1,
                deleted_dt = new DateTime(2020, 1, 1),
                is_deleted = 1,

            });
            Formula_Details.Add(new tbl_component_formula_details()
            {
                sno = 3000,
                component_id = 3000,
                company_id = 1,
                salary_group_id = 1,
                formula = "0",
                function_calling_order = 1,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                deleted_by = 1,
                deleted_dt = new DateTime(2020, 1, 1),
                is_deleted = 1,

            });


            foreach (enmOtherComponent _enm in Enum.GetValues(typeof(enmOtherComponent)))
            {
                PayrollComponent payrollComponent = _enm.GetComponentDetails();
                component_Masters.Add(new tbl_component_master()
                {
                    component_id = (int)_enm,
                    component_name = "@" + _enm,
                    datatype = payrollComponent.datatype.ToString(),
                    defaultvalue = payrollComponent.defaultvalue,
                    parentid = payrollComponent.parentid,
                    is_system_key = payrollComponent.is_system_key,
                    System_function = payrollComponent.System_function,
                    System_table = null,
                    component_type = (int)payrollComponent.ComponentType,
                    is_salary_comp = payrollComponent.is_salary_comp,
                    is_tds_comp = 0,
                    is_data_entry_comp = payrollComponent.is_data_entry_comp,
                    payment_type = 0,
                    is_user_interface = payrollComponent.is_user_interface,
                    is_payslip = payrollComponent.is_payslip,
                    created_by = 1,
                    created_dt = new DateTime(2020, 1, 1),
                    modified_by = 1,
                    modified_dt = new DateTime(2020, 1, 1),
                    is_active = 1,
                    property_details = payrollComponent.component_name
                });
                Formula_Details.Add(new tbl_component_formula_details()
                {
                    sno = (int)_enm,
                    component_id = (int)_enm,
                    company_id = 1,
                    salary_group_id = 1,
                    formula = payrollComponent.formula,
                    function_calling_order = payrollComponent.function_calling_order,
                    created_by = 1,
                    created_dt = new DateTime(2020, 1, 1),
                    deleted_by = 1,
                    deleted_dt = new DateTime(2020, 1, 1),
                    is_deleted = 0,

                });

            }


            modelBuilder.Entity<tbl_component_master>().HasData(component_Masters);
            modelBuilder.Entity<tbl_component_formula_details>().HasData(Formula_Details);

            #endregion

            #region **************** Salary Report ***************************

            List<tbl_report_master> tbl_Report_Masters = new List<tbl_report_master>();
            tbl_Report_Masters.Add(new tbl_report_master() { rpt_id = 1, rpt_name = "Salary Report(Arrear)", rpt_description = "salary Report with arrear", is_active = 1, created_by = 1, created_date = new DateTime(2020, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2020, 1, 1) });
            tbl_Report_Masters.Add(new tbl_report_master() { rpt_id = 2, rpt_name = "Salary Report", rpt_description = "salary Report", is_active = 1, created_by = 1, created_date = new DateTime(2020, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2020, 1, 1) });
            modelBuilder.Entity<tbl_report_master>().HasData(tbl_Report_Masters);



            List<tbl_rpt_title_master> tbl_rpt_title_masters = new List<tbl_rpt_title_master>();
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 1, component_id = (int)enmOtherComponent.Gender, rpt_title = "Gender", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 1, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 2, component_id = (int)enmOtherComponent.EmpFatherHusbandName, rpt_title = "Father Husband Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 2, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 3, component_id = (int)enmOtherComponent.EmpDOB, rpt_title = "Date of Birth", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 3, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 4, component_id = (int)enmOtherComponent.EmpNationality, rpt_title = "Nationality", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 4, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 5, component_id = (int)enmOtherComponent.EducationLevel, rpt_title = "Education Level", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 5, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 6, component_id = null, rpt_title = "Category Address", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 6, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 7, component_id = null, rpt_title = "Type Of Employment", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 7, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 8, component_id = (int)enmOtherComponent.EmpContact, rpt_title = "Mobile", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 8, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 9, component_id = (int)enmOtherComponent.Uan, rpt_title = "UAN Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 9, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 10, component_id = (int)enmOtherComponent.PanNo, rpt_title = "PAN No", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 10, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 11, component_id = (int)enmOtherComponent.PanName, rpt_title = "PAN Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 11, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 12, component_id = (int)enmOtherComponent.ESICNo, rpt_title = "ESIC Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 12, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 13, component_id = (int)enmOtherComponent.AdharNo, rpt_title = "Aadhar card Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 13, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 14, component_id = (int)enmOtherComponent.AdharName, rpt_title = "Aadhar name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 14, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 15, component_id = (int)enmOtherComponent.BankAccountNo, rpt_title = "Salary Account No", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 15, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 16, component_id = (int)enmOtherComponent.BankName, rpt_title = "Bank Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 16, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 17, component_id = (int)enmOtherComponent.IFSCCode, rpt_title = "Bank IFSC Code", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 17, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 18, component_id = null, rpt_title = "Address", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 18, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 19, component_id = null, rpt_title = "service book no", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 19, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 20, component_id = null, rpt_title = "Resignation date", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 20, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 21, component_id = null, rpt_title = "Last working date", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 21, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 22, component_id = null, rpt_title = "Reason Master", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 22, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 23, component_id = null, rpt_title = "mark of identification", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 23, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 24, component_id = null, rpt_title = "specimen impression", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 24, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 25, component_id = null, rpt_title = "Is Active", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 25, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 26, component_id = (int)enmOtherComponent.CompanyName, rpt_title = "Company Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 26, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 27, component_id = null, rpt_title = "Division Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 27, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 28, component_id = (int)enmOtherComponent.EmpWorkingState, rpt_title = "State Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 28, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 29, component_id = (int)enmOtherComponent.EmpLocation, rpt_title = "Branch Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 29, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 30, component_id = (int)enmOtherComponent.EmpLocation, rpt_title = "Location Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 30, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 31, component_id = (int)enmOtherComponent.EmpDepartment, rpt_title = "Department Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 31, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 32, component_id = (int)enmOtherComponent.EmpJoiningDt, rpt_title = "Date Of Joining", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 32, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 33, component_id = (int)enmOtherComponent.EmpDesignation, rpt_title = "Designation Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 33, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 34, component_id = (int)enmOtherComponent.EmpGrade, rpt_title = "Grade", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 34, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 35, component_id = (int)enmOtherComponent.PfApplicableYesNo, rpt_title = "PF Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 35, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 36, component_id = (int)enmOtherComponent.PfNo, rpt_title = "PF Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 36, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 37, component_id = (int)enmOtherComponent.PfGroup, rpt_title = "PF Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 37, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 38, component_id = (int)enmOtherComponent.pf_celling, rpt_title = "PF Ceiling", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 38, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 39, component_id = null, rpt_title = "ESIC Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 39, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 40, component_id = null, rpt_title = "PT Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 40, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 41, component_id = null, rpt_title = "PT Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 41, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 42, component_id = (int)enmOtherComponent.Vpf_applicableYesNo, rpt_title = "VPF Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 42, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 43, component_id = (int)enmOtherComponent.vpf_percentage, rpt_title = "VPF Percentage", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 43, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 44, component_id = null, rpt_title = "Is daily", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 44, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 45, component_id = null, rpt_title = "Salary On Hold", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 45, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 46, component_id = null, rpt_title = "Salary Process Type", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 46, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 47, component_id = (int)enmOtherComponent.GrossSalary, rpt_title = "Monthly Gross ", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 47, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 48, component_id = (int)enmOtherComponent.PaidDays, rpt_title = "Days Worked", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 48, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 49, component_id = (int)enmOtherComponent.ArrearDays, rpt_title = "Arrears Days", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 49, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 50, component_id = (int)enmOtherComponent.ToalPayrollDay, rpt_title = "Days In Month", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 50, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 51, component_id = (int)enmOtherComponent.Gratuity, rpt_title = "Gratuity", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 51, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 52, component_id = (int)enmOtherComponent.Leen, rpt_title = "Leen", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 52, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 53, component_id = null, rpt_title = "Notice Pay", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 53, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 54, component_id = null, rpt_title = "Pre hold  salary released", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 54, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 55, component_id = (int)enmOtherComponent.BasicSalary, rpt_title = "Basic Salary", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 55, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 56, component_id = (int)enmOtherComponent.BasicSalary, rpt_title = "Arrear Basic Salary", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 56, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 57, component_id = (int)enmOtherComponent.HRA, rpt_title = "HRA Allowance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 57, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 58, component_id = (int)enmOtherComponent.HRA, rpt_title = "Arrear HRA Allowance", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 58, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 59, component_id = (int)enmOtherComponent.Conveyance, rpt_title = "Conveyance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 59, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 60, component_id = (int)enmOtherComponent.Conveyance, rpt_title = "Arrear conveyance", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 60, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 61, component_id = (int)enmOtherComponent.Medical_Allowance, rpt_title = "Medical Allowance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 61, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 62, component_id = (int)enmOtherComponent.Medical_Allowance, rpt_title = "Arrear Medical Allowance", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 62, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 63, component_id = (int)enmOtherComponent.SPL, rpt_title = "Special Allowance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 63, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 64, component_id = (int)enmOtherComponent.SPL, rpt_title = "Arrear Special Allowance", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 64, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 65, component_id = (int)enmOtherComponent.Pf_amount, rpt_title = "PF Employee", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 65, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 66, component_id = (int)enmOtherComponent.Pf_amount, rpt_title = "Arrear PF Employee", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 66, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 67, component_id = (int)enmOtherComponent.EsicAmount, rpt_title = "ESIC Employee", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 67, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 68, component_id = (int)enmOtherComponent.EsicAmount, rpt_title = "Arrear ESIC Employee", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 68, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 69, component_id = (int)enmOtherComponent.ChildrenEducationAllowance, rpt_title = "Children Education Allowance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 69, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 70, component_id = (int)enmOtherComponent.ChildrenEducationAllowance, rpt_title = "Arrear Children Education Allowance", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 70, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 71, component_id = (int)enmOtherComponent.TotalGross, rpt_title = "Gross", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 71, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 72, component_id = (int)enmOtherComponent.TotalGross, rpt_title = "Arrear Gross", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 72, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 73, component_id = (int)enmOtherComponent.OT, rpt_title = "Over Time", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 73, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 74, component_id = (int)enmOtherComponent.CityCompensatoryAllowances, rpt_title = "CityCompensatoryAllowances", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 74, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 75, component_id = null, rpt_title = "OtherAllowance", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 75, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 76, component_id = (int)enmOtherComponent.TotalGross, rpt_title = "Gross Salary", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 76, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 77, component_id = (int)enmOtherComponent.Pf_amount, rpt_title = "Provident Fund", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 77, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 78, component_id = (int)enmOtherComponent.PT, rpt_title = "Professional Tax", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 78, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 79, component_id = (int)enmOtherComponent.EsicAmount, rpt_title = "ESIC", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 79, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 80, component_id = null, rpt_title = "LWF", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 80, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 81, component_id = (int)enmOtherComponent.Tax, rpt_title = "TDS", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 81, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 82, component_id = (int)enmOtherComponent.Vpf_amount, rpt_title = "Voluntary Provident Fund", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 82, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 83, component_id = null, rpt_title = "Notice Reco", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 83, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 84, component_id = (int)enmOtherComponent.OtherDeduction, rpt_title = "Other Deduction", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 84, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 85, component_id = (int)enmOtherComponent.Advance_Loan, rpt_title = "Advance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 85, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 86, component_id = (int)enmOtherComponent.Recovery, rpt_title = "Recovery", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 86, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 87, component_id = (int)enmOtherComponent.Deduction, rpt_title = "Gross Deduction", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 87, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 88, component_id = (int)enmOtherComponent.Net, rpt_title = "Net Salary", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 88, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 89, component_id = null, rpt_title = "Remarks", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 89, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 501, component_id = (int)enmOtherComponent.Gender, rpt_title = "Gender", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 1, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 502, component_id = (int)enmOtherComponent.EmpFatherHusbandName, rpt_title = "Father Husband Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 2, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 503, component_id = (int)enmOtherComponent.EmpDOB, rpt_title = "Date of Birth", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 3, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 504, component_id = (int)enmOtherComponent.EmpNationality, rpt_title = "Nationality", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 4, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 505, component_id = (int)enmOtherComponent.EducationLevel, rpt_title = "Education Level", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 5, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 506, component_id = null, rpt_title = "Category Address", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 6, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 507, component_id = null, rpt_title = "Type Of Employment", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 7, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 508, component_id = (int)enmOtherComponent.EmpContact, rpt_title = "Mobile", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 8, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 509, component_id = (int)enmOtherComponent.Uan, rpt_title = "UAN Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 9, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 510, component_id = (int)enmOtherComponent.PanNo, rpt_title = "PAN No", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 10, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 511, component_id = (int)enmOtherComponent.PanName, rpt_title = "PAN Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 11, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 512, component_id = (int)enmOtherComponent.ESICNo, rpt_title = "ESIC Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 12, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 513, component_id = (int)enmOtherComponent.AdharNo, rpt_title = "Aadhar card Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 13, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 514, component_id = (int)enmOtherComponent.AdharName, rpt_title = "Aadhar name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 14, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 515, component_id = (int)enmOtherComponent.BankAccountNo, rpt_title = "Salary Account No", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 15, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 516, component_id = (int)enmOtherComponent.BankName, rpt_title = "Bank Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 16, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 517, component_id = (int)enmOtherComponent.IFSCCode, rpt_title = "Bank IFSC Code", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 17, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 518, component_id = null, rpt_title = "Address", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 18, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 519, component_id = null, rpt_title = "service book no", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 19, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 520, component_id = null, rpt_title = "Resignation date", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 20, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 521, component_id = null, rpt_title = "Last working date", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 21, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 522, component_id = null, rpt_title = "Reason Master", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 22, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 523, component_id = null, rpt_title = "mark of identification", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 23, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 524, component_id = null, rpt_title = "specimen impression", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 24, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 525, component_id = null, rpt_title = "Is Active", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 25, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 526, component_id = (int)enmOtherComponent.CompanyName, rpt_title = "Company Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 26, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 527, component_id = null, rpt_title = "Division Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 27, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 528, component_id = (int)enmOtherComponent.EmpWorkingState, rpt_title = "State Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 28, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 529, component_id = (int)enmOtherComponent.EmpLocation, rpt_title = "Branch Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 29, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 530, component_id = (int)enmOtherComponent.EmpLocation, rpt_title = "Location Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 30, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 531, component_id = (int)enmOtherComponent.EmpDepartment, rpt_title = "Department Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 31, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 532, component_id = (int)enmOtherComponent.EmpJoiningDt, rpt_title = "Date Of Joining", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 32, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 533, component_id = (int)enmOtherComponent.EmpDesignation, rpt_title = "Designation Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 33, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 534, component_id = (int)enmOtherComponent.EmpGrade, rpt_title = "Grade", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 34, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 535, component_id = (int)enmOtherComponent.PfApplicableYesNo, rpt_title = "PF Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 35, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 536, component_id = (int)enmOtherComponent.PfNo, rpt_title = "PF Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 36, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 537, component_id = (int)enmOtherComponent.PfGroup, rpt_title = "PF Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 37, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 538, component_id = (int)enmOtherComponent.pf_celling, rpt_title = "PF Ceiling", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 38, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 539, component_id = null, rpt_title = "ESIC Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 39, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 540, component_id = null, rpt_title = "PT Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 40, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 541, component_id = null, rpt_title = "PT Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 41, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 542, component_id = (int)enmOtherComponent.Vpf_applicableYesNo, rpt_title = "VPF Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 42, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 543, component_id = (int)enmOtherComponent.vpf_percentage, rpt_title = "VPF Percentage", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 43, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 544, component_id = null, rpt_title = "Is daily", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 44, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 545, component_id = null, rpt_title = "Salary On Hold", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 45, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 546, component_id = null, rpt_title = "Salary Process Type", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 46, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 547, component_id = (int)enmOtherComponent.GrossSalary, rpt_title = "Monthly Gross ", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 47, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 548, component_id = (int)enmOtherComponent.PaidDays, rpt_title = "Days Worked", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 48, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 549, component_id = (int)enmOtherComponent.ArrearDays, rpt_title = "Arrears Days", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 49, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 550, component_id = (int)enmOtherComponent.ToalPayrollDay, rpt_title = "Days In Month", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 50, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 551, component_id = (int)enmOtherComponent.Gratuity, rpt_title = "Gratuity", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 51, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 552, component_id = (int)enmOtherComponent.Leen, rpt_title = "Leen", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 52, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 553, component_id = null, rpt_title = "Notice Pay", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 53, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 554, component_id = null, rpt_title = "Pre hold  salary released", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 54, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 555, component_id = (int)enmOtherComponent.BasicSalary, rpt_title = "Basic Salary", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 55, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 556, component_id = (int)enmOtherComponent.HRA, rpt_title = "HRA Allowance", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 56, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 557, component_id = (int)enmOtherComponent.Conveyance, rpt_title = "Conveyance", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 57, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 558, component_id = (int)enmOtherComponent.Medical_Allowance, rpt_title = "Medical Allowance", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 58, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 559, component_id = (int)enmOtherComponent.SPL, rpt_title = "Special Allowance", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 59, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 560, component_id = (int)enmOtherComponent.Pf_amount, rpt_title = "PF Employee", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 60, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 561, component_id = (int)enmOtherComponent.EsicAmount, rpt_title = "ESIC Employee", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 61, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 562, component_id = (int)enmOtherComponent.ChildrenEducationAllowance, rpt_title = "Children Education Allowance", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 62, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 563, component_id = (int)enmOtherComponent.TotalGross, rpt_title = "Gross", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 63, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 564, component_id = (int)enmOtherComponent.OT, rpt_title = "Over Time", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 64, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 565, component_id = (int)enmOtherComponent.CityCompensatoryAllowances, rpt_title = "CityCompensatoryAllowances", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 65, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 566, component_id = null, rpt_title = "OtherAllowance", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 66, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 567, component_id = (int)enmOtherComponent.TotalGross, rpt_title = "Gross Salary", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 67, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 568, component_id = (int)enmOtherComponent.Pf_amount, rpt_title = "Provident Fund", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 68, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 569, component_id = (int)enmOtherComponent.PT, rpt_title = "Professional Tax", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 69, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 570, component_id = (int)enmOtherComponent.EsicAmount, rpt_title = "ESIC", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 70, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 571, component_id = null, rpt_title = "LWF", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 71, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 572, component_id = (int)enmOtherComponent.Tax, rpt_title = "TDS", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 72, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 573, component_id = (int)enmOtherComponent.Vpf_amount, rpt_title = "Voluntary Provident Fund", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 73, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 574, component_id = null, rpt_title = "Notice Reco", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 74, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 575, component_id = (int)enmOtherComponent.OtherDeduction, rpt_title = "Other Deduction", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 75, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 576, component_id = (int)enmOtherComponent.Advance_Loan, rpt_title = "Advance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 76, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 577, component_id = (int)enmOtherComponent.Recovery, rpt_title = "Recovery", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 77, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 578, component_id = (int)enmOtherComponent.Deduction, rpt_title = "Gross Deduction", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 78, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 579, component_id = (int)enmOtherComponent.Net, rpt_title = "Net Salary", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 79, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 580, component_id = null, rpt_title = "Remarks", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 80, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });

            #endregion
            modelBuilder.Entity<tbl_rpt_title_master>().HasData(tbl_rpt_title_masters);

        }

        public void InsertRoleClaim(ModelBuilder modelBuilder)
        {
            List<tbl_role_master> role_data = new List<tbl_role_master>();
            foreach (enmRoleMaster _enm in Enum.GetValues(typeof(enmRoleMaster)))
            {
                role_data.Add(new tbl_role_master() { created_by = 1, created_date = new DateTime(2020, 1, 1), is_active = 1, last_modified_by = 1, last_modified_date = new DateTime(2020, 1, 1), role_id = (int)_enm, role_name = _enm.ToString() });
            }


            List<tbl_menu_master> menu_Masters = new List<tbl_menu_master>();
            List<tbl_role_menu_master> trmm = new List<tbl_role_menu_master>();
            foreach (enmMenuMaster _enm in Enum.GetValues(typeof(enmMenuMaster)))
            {
                if (_enm != enmMenuMaster.None)
                {
                    MenuComponent menuComponent = _enm.GetMenuDetails();
                    menu_Masters.Add(new tbl_menu_master()
                    {
                        menu_id = _enm,
                        menu_name = menuComponent.menu_name,
                        IconUrl = menuComponent.icon,
                        urll = menuComponent.link,
                        type = menuComponent.type,
                        parent_menu_id = (int)menuComponent.parent_menu,
                        modified_by = 1,
                        modified_date = new DateTime(2020, 1, 1),
                        created_by = 1,
                        created_date = new DateTime(2020, 1, 1),
                        is_active = 1,

                    });

                    foreach (var enrole in menuComponent.RoleMaster)
                    {

                        trmm.Add(new tbl_role_menu_master()
                        {
                            modified_by = 1,
                            modified_date = new DateTime(2020, 1, 1),
                            created_by = 1,
                            created_date = new DateTime(2020, 1, 1),
                            menu_id = _enm,
                            role_id = enrole,
                            role_menu_id = (((int)_enm * 100000) + (int)enrole)
                        });
                        if (enrole == enmRoleMaster.Manager)
                        {
                            trmm.Add(new tbl_role_menu_master()
                            {
                                modified_by = 1,
                                modified_date = new DateTime(2020, 1, 1),
                                created_by = 1,
                                created_date = new DateTime(2020, 1, 1),
                                menu_id = _enm,
                                role_id = enmRoleMaster.SectionHead,
                                role_menu_id = (((int)_enm * 100000) + (int)enmRoleMaster.SectionHead)
                            });
                        }
                    }
                }
            }
            modelBuilder.Entity<tbl_role_master>().HasData(role_data);
            modelBuilder.Entity<tbl_menu_master>().HasData(menu_Masters);
            modelBuilder.Entity<tbl_role_menu_master>().HasData(trmm);
        }

        public void DefaultEmployee(ModelBuilder modelBuilder)
        {
            tbl_employee_master empMaster = new tbl_employee_master() { 
                employee_id=1, emp_code="Admin", created_by=1,created_date=CurrentDate,is_active=1,
                last_modified_by=1, last_modified_date=CurrentDate,                
            };
            tbl_user_master userData = new tbl_user_master()
            {
                user_id = 1,
                username = "Admin",
                created_by = 1,
                last_modified_date = CurrentDate,
                last_modified_by = 1,
                created_date = CurrentDate,
                default_company_id = 1,
                employee_id = 1,
                is_active = 1,
                is_logged_blocked = 0,
                is_logged_in = 0,
                last_logged_dt = CurrentDate,
                logged_blocked_dt = CurrentDate,
                password = "jnZWe3S+++aQtpmKlibdOA==",
                user_type = 1,                
            };

            tbl_user_role_map userRole = new tbl_user_role_map()
            {
                claim_master_id = 1,
                created_by = 1,
                created_date = CurrentDate,
                is_deleted = 0,
                last_modified_by = 1,
                last_modified_date = CurrentDate,
                role_id = 1,
                user_id = 1
            };

            tbl_employee_company_map empCompanyMap = new tbl_employee_company_map()
            {
                company_id = 1,
                employee_id = 1,
                sno = 1,
                created_by = 1,
                created_date = CurrentDate,
                is_default = true,
                is_deleted = 0,
                last_modified_by = 1,
                last_modified_date = CurrentDate
            };

            tbl_employment_type_master employment_type = new tbl_employment_type_master()
            {
                employee_id = 1,
                is_deleted = 0,
                duration_days = 1,
                created_by = 1,
                created_date = CurrentDate,
                actual_duration_days = 1000,
                actual_duration_end_period = CurrentDate,
                actual_duration_start_period = CurrentDate,
                duration_end_period = CurrentDate,
                duration_start_period = CurrentDate,
                effective_date = CurrentDate,
                employment_type = 3,
                employment_type_id = 1,
                last_modified_by = 1,
                last_modified_date = CurrentDate
            };

            modelBuilder.Entity<tbl_employee_master>().HasData(empMaster);
            modelBuilder.Entity<tbl_user_master>().HasData(userData);
            modelBuilder.Entity<tbl_user_role_map>().HasData(userRole);
            modelBuilder.Entity<tbl_employee_company_map>().HasData(empCompanyMap);
            modelBuilder.Entity<tbl_employment_type_master>().HasData(employment_type);
            


        }


        public void InsertPayrollProcessMaster(ModelBuilder modelBuilder)
        {
            List<tblProcessMaster> datas = new List<tblProcessMaster>();
            datas.Add(new tblProcessMaster { ProcessId = 1, ProcessName = "Frezee Attendance", DisplayOrder = 1, IsActive = true });
            datas.Add(new tblProcessMaster { ProcessId = 2, ProcessName = "Lock Attendance", DisplayOrder = 2, IsActive = true });
            datas.Add(new tblProcessMaster { ProcessId = 3, ProcessName = "HoldRelase Salary", DisplayOrder = 3, IsActive = true });
            datas.Add(new tblProcessMaster { ProcessId = 4, ProcessName = "Calculate Loan", DisplayOrder = 4, IsActive = true });
            datas.Add(new tblProcessMaster { ProcessId = 5, ProcessName = "Calculate TDS", DisplayOrder = 5, IsActive = true });
            datas.Add(new tblProcessMaster { ProcessId = 6, ProcessName = "Calculate Salary", DisplayOrder = 6, IsActive = true });
            datas.Add(new tblProcessMaster { ProcessId = 7, ProcessName = "Freeze Salary", DisplayOrder = 6, IsActive = true });
            datas.Add(new tblProcessMaster { ProcessId = 8, ProcessName = "Freeze Salary", DisplayOrder = 6, IsActive = true });
            modelBuilder.Entity<tblProcessMaster>().HasData(datas);

            List<tblDependentProcess> DependentDatas = new List<tblDependentProcess>();
            DependentDatas.Add(new tblDependentProcess { Id=1,ProcessId = 6, DependentProcessId = 1,  IsActive = true });
            DependentDatas.Add(new tblDependentProcess { Id=2,ProcessId = 6, DependentProcessId = 2, IsActive = true });
            modelBuilder.Entity<tblDependentProcess>().HasData(DependentDatas);
        }
    }
}

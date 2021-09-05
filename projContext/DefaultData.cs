using Microsoft.EntityFrameworkCore;
using projContext.DB;
using System;
using System.Collections.Generic;
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

                pkid_setting = 1,
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

    }
}

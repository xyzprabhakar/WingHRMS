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

        

        public void InsertDepartmentMaster(ModelBuilder modelBuilder)
        {
            List<tbl_department_master> departments = new List<tbl_department_master>();
            departments.Add(new tbl_department_master() { department_id=1, company_id=1 })
        }

    }
}

using Microsoft.AspNetCore.Http;
using projContext.DB.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model
{
    public class tblOrganisationWraper : tblOrganisation
    {
        public IFormFile LogoImageFile { get; set; }        
    }


#if (false)

    public class clsLocationMaster
    {
        public int location_id { get; set; }
        public string location_code { get; set; }

        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "location_name")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,200}$", ErrorMessage = "Invalid Location Name")]
        public string location_name { get; set; }
        public string type_of_location { get; set; }
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "address_line_one")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/]{1,250}$", ErrorMessage = "Invalid Address in Address Line one")]
        public string address_line_one { get; set; }
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "address_line_two")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/]{1,250}$", ErrorMessage = "Invalid Address in Address Line Two")]
        public string address_line_two { get; set; }
        public string pin_code { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        [MaxLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Priamary Email ID")]
        public string primary_email_id { get; set; }
        [MaxLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Seconday Email ID")]
        public string secondary_email_id { get; set; }
        public string primary_contact_number { get; set; }
        public string secondary_contact_number { get; set; }
        public int company_id { get; set; }
        public string website { get; set; }
        public string image { get; set; }
        public bool is_active { get; set; }

        public int created_by { get; set; }

        public int location_type_id { get; set; }

        public string company_name { get; set; }
        public DateTime created_on { get; set; }

        public string sub_location_name { get; set; }

        public int sub_location_id { get; set; }
    }
    public class clsCountry
    {
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "CountryName")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,100}$", ErrorMessage = "Invalid Country Name")]
        public string CountryName { get; set; }
        [MaxLength(10)]
        [StringLength(10)]
        [Display(Description = "ShortName")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Invalid Short Name")]
        public string ShortName { get; set; }
        public int CountryId { get; set; }

        public int created_by { get; set; }
    }
    public class clsState
    {
        public int CountryId { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "StateName")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,200}$", ErrorMessage = "Invalid State Name")]
        public string StateName { get; set; }
        public int StateId { get; set; }
        public string ActionMode { get; set; }
        public int created_by { get; set; }

    }
    public class clsCity
    {
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "CityName")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,200}$", ErrorMessage = "Invalid City Name")]
        public string CityName { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string ActionMode { get; set; }

        public int created_by { get; set; }
    }

    public class App_setting_Master
    {
        public int pkid_setting { get; set; }
        public string AppSettingKey { get; set; }
        public string AppSettingKeyDisplay { get; set; }
        public string AppSettingValue { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class holiday_master
    {
        public int holiday_id { get; set; }  // primary key  must be public!
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }

        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "holiday_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Holiday Name")]
        public string holiday_name { get; set; }
        public DateTime holiday_date { get; set; }// default it will Store the Day, and Month and year contain default 2000 like 26-jan-2000

        public int is_applicable_on_all_comp { get; set; }//1 for all, 0 for particular 
        public int is_applicable_on_all_emp { get; set; }//1 for all, 0 for particular
        public int is_applicable_on_all_religion { get; set; }//1 for all, 0 for particular
        public int is_applicable_on_all_location { get; set; }//1 for all, 0 for particular
        public byte company_id { get; set; }

        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public List<location_id_list> location_id_list { get; set; }
        public List<emp_id_list> emp_id_list { get; set; }
        public string holidayno { get; set; }
        public List<company_id_list> company_id_list { get; set; }
    }

    public class location_id_list
    {
        public int location_id { get; set; }
    }

    public class emp_id_list
    {
        public int employee_id { get; set; }
    }

    public class companylistcls
    {
        public List<company_id_list> company_id_list { get; set; }
    }

    public class locationlistcls
    {
        public List<location_id_list> location_id_list { get; set; }
    }

    public class company_id_list
    {
        public int company_id { get; set; }
    }

    public class clsClaimMaster
    {
        [StringLength(50)]
        [Display(Description = "claim_master_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\\]{1,50}$", ErrorMessage = "Invalid Claim Master Name")]
        public string claim_master_name { get; set; }
        public int claim_master_id { get; set; }
    }

    public class clsRoleClaimModel
    {
        public int claimid { get; set; }
        public string claimname { get; set; }
        public bool ischecked { get; set; }
    }

    public class clsRoleClaimMap
    {
        public List<int> claimids { get; set; }
        public int roleid { get; set; }
    }
    public class clsUserRoleModel
    {
        public int userid { get; set; }
        public int empid { get; set; }
        public string empcode { get; set; }
        public string empname { get; set; }
        public string empdept { get; set; }
        public string empdesig { get; set; }
        public bool ischecked { get; set; }
    }

    public class clsUserRoleMap
    {
        public List<int> userid { get; set; }
        public int roleid { get; set; }
        public int created_by { get; set; }
    }


    public class UserMasterSec
    {
        public int user_id { get; set; }  // primary key  must be public!    

        public string username { get; set; }

        public string old_password { get; set; }

        public string new_password { get; set; }
        public byte user_type { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public int default_company_id { get; set; }
        public int employee_id { get; set; }

    }
    public class policy_master_documents
    {
        public int pkid_tpmDoc { get; set; }  // primary key  must be public!
        public int fkid_policy { get; set; }
        public string document_path { get; set; }
        public int is_active { get; set; }

    }
    public class policy_master
    {
        public int pkid_policy { get; set; }  // primary key  must be public!
        public string policy_name { get; set; }
        public string remarks { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public byte is_deleted { get; set; }
        public List<string> lstdocumentsName { get; set; }
        public IFormFileCollection documents { get; set; }


    }


    public class Current_Openings
    {

        public int pkid_current_openings { get; set; }  // primary key  must be public!
        public int company_id { get; set; }
        public int department_id { get; set; }
        public int current_status { get; set; }
        public string opening_detail { get; set; }
        public string posted_date { get; set; }
        public string experience { get; set; }
        public string job_description { get; set; }
        public string role_responsibility { get; set; }
        public string doc_path { get; set; }
        public string remarks { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int is_active { get; set; }
    }
    public class kt_task_master
    {       
        public int id { get; set; }

        public int? emp_sepration_Id { get; set; }

        public string Task_Sno { get; set; }

        public string taskName { get; set; }

        public string Procedure { get; set; }

        public string remarks { get; set; }

        public int Status { get; set; }

        public string ModHandover { get; set; }

        public DateTime HandoverDate { get; set; }

        public int created_by { get; set; }

        public int last_modified_by { get; set; }

        public int is_active { get; set; }

        public int is_deleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime modified_on { get; set; }

        public List<emp_id_list> emp_id_listd { get; set; }

        public string emp_id_list { get; set; }
    }   
    
#endif
}

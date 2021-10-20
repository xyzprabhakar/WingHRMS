using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB
{
    /// <summary>
    /// Created By: Amarjeet
    /// Created on: 30-11-2018
    /// </summary>
    /// <remarks>Company Master</remarks>


    public class tbl_company_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int company_id { get; set; }  // primary key  must be public!
        [Required] // Required create a NOT NULL column
        [MaxLength(50)] // MaxLength Exmp. Varchar(50)
        public string company_code { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Description = "company_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.'\-]{1,100}$", ErrorMessage = "Invalid Company Name")]
        public string company_name { get; set; }
        
        public byte is_emp_code_manual_genrate { get; set; }
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "address_line_one")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/]{1,250}$", ErrorMessage = "Invalid Address in Address Line one ((.),(,) and special characters are not allowed)")]
        public string address_line_one { get; set; }
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "address_line_two")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/]{1,250}$", ErrorMessage = "Invalid Address in Address Line Two ((.),(,) and special characters are not allowed)")]
        public string address_line_two { get; set; }
        public int pin_code { get; set; }

        [ForeignKey("tbl_city")]
        public int? city_id { get; set; }
        public tbl_city tbl_city { get; set; }
        [ForeignKey("tbl_state")]
        public int? state_id { get; set; }
        public tbl_state tbl_state { get; set; }
        [ForeignKey("tbl_country")]
        public int? country_id { get; set; }
        public tbl_country tbl_country { get; set; }


        [MaxLength(50)]
        [StringLength(50)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Primary Email ID")]
        public string primary_email_id { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Seconday Email ID")]
        public string secondary_email_id { get; set; }
        [MaxLength(20)]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Primary Contact No.")]
        public string primary_contact_number { get; set; }
        [MaxLength(20)]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Seconday Contact No.")]
        public string secondary_contact_number { get; set; }
        [MaxLength(100)]
        public string company_website { get; set; }
        public string company_logo { get; set; }
        public int user_type { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

        public int total_emp { get; set; }
    }

    public class tbl_company_emp_setting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }
        [ForeignKey("tbl_company_master")]
        public int? company_id { get; set; }
        public tbl_company_master tbl_company_master { get; set; }
        public string prefix_for_employee_code { get; set; }
        public int number_of_character_for_employee_code { get; set; }
        public int from_range { get; set; }  
        public int current_range { get; set; }
        public int to_range { get; set; }
        public byte is_active { get; set; }
        public DateTime last_genrated { get; set; }
    }


    public class tblCustomerOrganisation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        [MaxLength(256)]
        public string OrganisationName { get; set; }// It is the OTP for Validating Email and Password 
        [MaxLength(32)]
        public string OrganisationCode { get; set; }
        [MaxLength(128)]
        public string Email { get; set; }
        [MaxLength(32)]
        public string PhoneNumber { get; set; }
        [MaxLength(128)]
        public string OrgLogo { get; set; }
        public DateTime EffectiveFromDt { get; set; } = DateTime.Now;
        public DateTime EffectiveToDt { get; set; } = DateTime.Now;
    }

}

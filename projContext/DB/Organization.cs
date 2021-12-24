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

    public class tbl_organisation_master : d_Contact_With_Address_With_Modify_by
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int organisation_id { get; set; }
        [MaxLength(254)]
        public string organisation_name { get; set; }
        public string logo { get; set; }
    }

    public class tbl_company_master : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int company_id { get; set; }
        [MaxLength(64)] 
        public string company_code { get; set; }
        [Required]
        [MaxLength(254)]
        public string company_name { get; set; }
        [MaxLength(254)]
        public string OfficeAddress{ get; set; }
        [MaxLength(254)]
        public string Locality { get; set; }
        [MaxLength(254)]
        public string City { get; set; }
        [MaxLength(32)]
        public int pin_code { get; set; }
        [ForeignKey("tbl_state")]
        public int? state_id { get; set; }
        public tbl_state tbl_state { get; set; }
        [ForeignKey("tbl_country")]
        public int? country_id { get; set; }
        public tbl_country tbl_country { get; set; }
        [MaxLength(254)]
        public string Email { get; set; }
        [MaxLength(254)]
        public string AlternateEmail { get; set; }
        [MaxLength(16)]
        public string ContactNo { get; set; }
        [MaxLength(16)]
        public string AlternateContactNo { get; set; }
        [MaxLength(100)]
        public string company_website { get; set; }
        public string company_logo { get; set; }
        public int is_active { get; set; }
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


    

}

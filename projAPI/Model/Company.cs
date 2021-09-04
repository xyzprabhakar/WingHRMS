using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model
{
    public class Company
    {
        public int company_id { get; set; } 
        public string company_code { get; set; }
        public string company_name { get; set; }

        public byte is_emp_code_manual_genrate { get; set; }
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "address_line_one")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/]{1,250}$", ErrorMessage = "Invalid Address in Address Line one((.),(,) and special characters are not allowed)")]
        public string address_line_one { get; set; }
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "address_line_two")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/]{1,250}$", ErrorMessage = "Invalid Address in Address Line Two ((.),(,) and special characters are not allowed)")]
        public string address_line_two { get; set; }
        public int pin_code { get; set; }

        public string prefix_for_employee_code { get; set; }
        public int number_of_character_for_employee_code { get; set; }

        public int city_id { get; set; }

        public int state_id { get; set; }
        public int country_id { get; set; }

        [MaxLength(200)]
        [StringLength(200)]
        [DataType(DataType.EmailAddress,ErrorMessage ="Invalid Primary Email ID")]
        public string primary_email_id { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Seconday Email ID")]
        public string secondary_email_id { get; set; }
        [MaxLength(20)]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber,ErrorMessage ="Invalid Primary Contact No.")]
        public string primary_contact_number { get; set; }
        [MaxLength(20)]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Seconday Contact No.")]
        public string secondary_contact_number { get; set; }
        public string company_website { get; set; }
        public string company_logo { get; set; }
        public int user_type { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }
}

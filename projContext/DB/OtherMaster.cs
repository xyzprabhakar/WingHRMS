using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB
{

    public class tbl_payroll_month_setting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int payroll_month_setting_id { get; set; }  // primary key  must be public!
        //[MaxLength(200)]
        public int from_month { get; set; }
        public int from_date { get; set; }
        public int applicable_from_date { get; set; }
        public int applicable_to_date { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public int company_id { get; set; }
        //public int to_date { get; set; }
    }


    public class tbl_country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int country_id { get; set; }  // primary key  must be public!
        [MaxLength(10)]
        [StringLength(10)]
        [Display(Description = "sort_name")]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Invalid Short Name")]
        public string sort_name { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "name")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,100}$", ErrorMessage = "Invalid Country Name")]
        public string name { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_state
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int state_id { get; set; }  // primary key  must be public!
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "name")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,200}$", ErrorMessage = "Invalid State Name")]
        public string name { get; set; }
        [ForeignKey("tbl_country")]
        public int? country_id { get; set; }
        public tbl_country tbl_country { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_city
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int city_id { get; set; }  // primary key  must be public!
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "name")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,200}$", ErrorMessage = "Invalid City Name")]
        public string name { get; set; }
        [ForeignKey("tbl_state")]
        public int? state_id { get; set; }
        public tbl_state tbl_state { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public string pincode { get; set; }
    }


    /// <summary>
    /// Developed By : Amarjeet
    /// Date : 28-11-2018
    /// Decs : Location Master
    /// </summary>
    /// 
    public class tbl_location_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int location_id { get; set; }  // primary key  must be public!
        [MaxLength(100)] // MaxLength Exmp. Varchar(100)
        public string location_code { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "location_name")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,200}$", ErrorMessage = "Invalid Location Name")]
        public string location_name { get; set; }
        public byte type_of_location { get; set; }
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
        [MaxLength(100)]
        [Display(Description = "primary_email_id")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Invalid Priamary Email ID")]
        
        public string primary_email_id { get; set; }
        [MaxLength(100)]
        [Display(Description = "secondary_email_id")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Seconday Email ID")]
        public string secondary_email_id { get; set; }
        [MaxLength(20)]
        public string primary_contact_number { get; set; }
        [MaxLength(20)]
        public string secondary_contact_number { get; set; }
        public int company_id { get; set; }
        public string website { get; set; }
        public string image { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_sub_location_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sub_location_id { get; set; }  // primary key  must be public!
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "location_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Sub Location")]
        public string location_name { get; set; }
        [ForeignKey("tbl_location_master")] // Foreign Key here
        public int? location_id { get; set; }
        public tbl_location_master tbl_location_master { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }


    /// <summary>
    /// Developed By : Amarjeet
    /// Date : 28-11-2018
    /// Decs :  Department Master
    /// </summary>
    /// 
    public class tbl_department_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int department_id { get; set; }  // primary key  must be public!
        [MaxLength(100)] // MaxLength Exmp. Varchar(100)
        public string department_code { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "department_name")]
      //  [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Department Name")]
        public string department_name { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "department_short_name")]
     //   [RegularExpression(@"^[a-zA-Z0-9'\s]{1,100}$", ErrorMessage = "Invalid Department Short Name")]
        public string department_short_name { get; set; }

        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }

        [MaxLength(100)]
        public string department_head_employee_code { get; set; }
        [MaxLength(200)]
        public string department_head_employee_name { get; set; }
        public int company_id { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    /// <summary>
    /// Developed By : Amarjeet
    /// Date : 28-11-2018
    /// Decs :  Sub Department Master
    /// </summary>
    public class tbl_sub_department_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sub_department_id { get; set; }  // primary key  must be public!
        [MaxLength(100)] // MaxLength Exmp. Varchar(100)
        [StringLength(100)]
        [Display(Description = "sub_department_code")]
       // [RegularExpression(@"^[a-zA-Z0-9'\s]{1,100}$", ErrorMessage = "Invalid Sub Department Code")]
        public string sub_department_code { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "sub_department_name")]
       // [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Sub Department Name")]
        public string sub_department_name { get; set; }
        [ForeignKey("tbl_department_master")] // Foreign Key here
        public int? department_id { get; set; }
        public tbl_department_master tbl_department_master { get; set; }

        public int company_id { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }


    /// <summary>
    /// Developed By : Amarjeet
    /// Date : 28-11-2018
    /// Decs :   Designation Master
    /// </summary>
    public class tbl_designation_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int designation_id { get; set; }  // primary key  must be public!
        [Required]
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "designation_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.'\/]{1,200}$", ErrorMessage = "Invalid Designation Name")]
        public string designation_name { get; set; }

        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }


    /// <summary>
    /// Developed By : Amarjeet
    /// Date : 28-11-2018
    /// Decs :  Grade Master
    /// </summary>
    public class tbl_grade_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int grade_id { get; set; }  // primary key  must be public!
        [Required]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "grade_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,50}$", ErrorMessage = "Invalid Grade Name")]
        public string grade_name { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_religion_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int religion_id { get; set; }  // primary key  must be public!
        [Required]
        [MaxLength(200)]
        public string religion_name { get; set; }

        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    /// <summary>
    /// Developed By : Amarjeet
    /// Date : 28-11-2018
    /// Decs : Machine Master
    /// </summary>
    /// 
    public class tbl_machine_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int machine_id { get; set; }  // primary key  must be public!
        public string machine_description { get; set; }
        [Required]
        public int machine_number { get; set; }
        [Required]
        public int port_number { get; set; }
        public byte machine_type { get; set; }//1 for essl
        [Required]
        public string ip_address { get; set; }
        public int is_active { get; set; }
        [ForeignKey("tbl_company_master")]
        public int? company_id { get; set; }
        public tbl_company_master tbl_company_master { get; set; }
        [ForeignKey("tbl_location_master")] // Foreign Key here
        public int? location_id { get; set; }
        public tbl_location_master tbl_location_master { get; set; }
        [ForeignKey("tbl_sub_location_master")] // Foreign Key here
        public int? sub_location_id { get; set; }
        public tbl_sub_location_master tbl_sub_location_master { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }


    /// <summary>
    /// Developed By : Amarjeet
    /// Date : 28-11-2018
    /// Decs :  OT RULES 
    /// </summary>
    public class tbl_ot_rule_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ot_rule_id { get; set; }  // primary key  must be public!
        [Required]
        [MaxLength(200)]
        public int grace_working_hour { get; set; }//min working our
        public int grace_working_minute { get; set; }//min working minu
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }


    /// <summary>
    /// Developed By : Amarjeet
    /// Date : 28-11-2018
    /// Decs :  Comb Off Rulemaster
    /// </summary>
    public class tbl_comb_off_rule_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int comp_off_rule_id { get; set; }  // primary key  must be public!
        [Required]
        [MaxLength(200)]
        public int minimum_working_hours { get; set; }
        public int minimum_working_minute { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_holiday_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int holiday_id { get; set; }  // primary key  must be public!
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }

        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "holiday_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Holiday Name")]
        public string holiday_name { get; set; }
        public DateTime holiday_date { get; set; }// default it will Store the Day, and Month and year contain default 2000 like 26-jan-2000
        public byte is_applicable_on_all_comp { get; set; }//1 for all, 0 for particular
        public byte is_applicable_on_all_emp { get; set; }//1 for all, 0 for particular
        public byte is_applicable_on_all_religion { get; set; }//1 for all, 0 for particular
        public byte is_applicable_on_all_location { get; set; }//1 for all, 0 for particular

        public ICollection<tbl_holiday_master_comp_list> tbl_holiday_master_comp_list { get; set; }
        public ICollection<tbl_holiday_master_emp_list> tbl_holiday_master_emp_list { get; set; }
        public ICollection<tbl_holiday_mstr_rel_list> tbl_holiday_mstr_rel_list { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public string holidayno { get; set; }
    }

    public class tbl_app_setting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pkid_setting { get; set; }  // primary key  must be public!
        public string AppSettingKey { get; set; }
        public string AppSettingKeyDisplay { get; set; }
        public string AppSettingValue { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }
    public class tbl_holiday_master_comp_list
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }  // primary key  must be public!
        [ForeignKey("tbl_company_master")]
        public int? company_id { get; set; }
        public tbl_company_master tbl_company_master { get; set; }
        [ForeignKey("tbl_location_master")]
        public int? location_id { get; set; }
        public tbl_location_master tbl_location_master { get; set; }
        [ForeignKey("tbl_holiday_master")]
        public int? holiday_id { get; set; }
        public tbl_holiday_master tbl_holiday_master { get; set; }
        public byte is_deleted { get; set; }
        
    }

    public class tbl_holiday_master_emp_list
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }  // primary key  must be public!
        [ForeignKey("tbl_employee_master")]
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }

        [ForeignKey("tbl_holiday_master")]
        public int? holiday_id { get; set; }
        public tbl_holiday_master tbl_holiday_master { get; set; }
        public byte is_deleted { get; set; }

        [ForeignKey("tbl_company_master")]
        public int? company_id { get; set; }
        public tbl_company_master tbl_company_master { get; set; }
    }

    public class tbl_holiday_mstr_rel_list
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }  // primary key  must be public!
        [ForeignKey("tbl_holiday")]
        public int? h_id { get; set; }
        public tbl_holiday_master tbl_holiday { get; set; }
        [ForeignKey("tbl_religion_master")]
        public int? religion_id { get; set; }
        public tbl_religion_master tbl_religion_master { get; set; }
        public byte is_deleted { get; set; }
    }


    #region Log Tables
    public class tbl_pay_set_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int payroll_month_setting_log_id { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_payroll_month_setting")]
        public int? pay_set_id { get; set; }
        public tbl_payroll_month_setting tbl_payroll_month_setting { get; set; }

        [MaxLength(200)]
        public int from_month { get; set; }
        public int from_date { get; set; }
        public int applicable_from_date { get; set; }
        public int applicable_to_date { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted





    }

    public class tbl_location_master_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int location_log_id { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_location_master")]
        public int? location_id { get; set; }
        public tbl_location_master tbl_location_master { get; set; }

        [MaxLength(100)] // MaxLength Exmp. Varchar(100)
        public string location_code { get; set; }
        [MaxLength(200)]
        public string location_name { get; set; }
        public byte type_of_location { get; set; }
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "address_line_two")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/]{1,250}$", ErrorMessage = "Invalid Address in Address Line Two ((.),(,) and special characters are not allowed)")]
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
        [MaxLength(200)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Primary Email ID")]
        public string primary_email_id { get; set; }
        [MaxLength(200)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Secondary Email ID")]
        public string secondary_email_id { get; set; }
        [MaxLength(20)]
        public string primary_contact_number { get; set; }
        [MaxLength(20)]
        public string secondary_contact_number { get; set; }
        public string website { get; set; }
        public string image { get; set; }
        public int company_id { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted

    }

    public class tbl_department_master_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int department_log_id { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_department_master")]
        public int? department_id { get; set; }
        public tbl_department_master tbl_department_master { get; set; }


        [MaxLength(100)] // MaxLength Exmp. Varchar(100)
        public string department_code { get; set; }
        [MaxLength(200)]
        public string department_name { get; set; }
        [MaxLength(100)]
        public string department_short_name { get; set; }
        [MaxLength(100)]
        public string department_head_employee_code { get; set; }
        [MaxLength(200)]
        public string department_head_employee_name { get; set; }
        public int company_id { get; set; }


        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted

    }

    public class tbl_sub_department_master_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sub_department_log_id { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_sub_department_master")] // Foreign Key here
        public int? sid { get; set; }
        public tbl_sub_department_master tbl_sub_department_master { get; set; }


        [MaxLength(100)] // MaxLength Exmp. Varchar(100)
        public string sub_department_code { get; set; }
        [MaxLength(200)]
        public string sub_department_name { get; set; }
        [ForeignKey("tbl_department_master")] // Foreign Key here
        public int? d_id { get; set; }
        public tbl_department_master tbl_department_master { get; set; }

        public int company_id { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }

    public class tbl_designation_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int designation_log_id { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_designation_master")]
        public int? desig_id { get; set; }
        public tbl_designation_master tbl_designation_master { get; set; }

        [Required]
        [MaxLength(200)]
        public string designation_name { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }

    public class tbl_grade_master_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int grade_log_id { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_grade_master")]
        public int? grade_id { get; set; }
        public tbl_grade_master tbl_grade_master { get; set; }

        [Required]
        [MaxLength(200)]
        public string grade_name { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }

    public class tbl_religion_master_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int religion_log_id { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_religion_master")]
        public int? religion_id { get; set; }
        public tbl_religion_master tbl_religion_master { get; set; }

        [Required]
        [MaxLength(200)]
        public string religion_name { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted

    }

    public class tbl_machine_master_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int machine_log_id { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_machine_master")]
        public int? machine_id { get; set; }
        public tbl_machine_master tbl_machine_master { get; set; }

        public string machine_description { get; set; }
        [Required]
        public int machine_number { get; set; }
        [Required]
        public int port_number { get; set; }
        [Required]
        public string ip_address { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }

    public class tbl_ot_rule_master_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ot_rule_log_id { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_ot_rule_master")]
        public int? ot_rule_id { get; set; }
        public tbl_ot_rule_master tbl_ot_rule_master { get; set; }

        [Required]
        [MaxLength(200)]
        public int grace_working_hour { get; set; }
        public int grace_working_minute { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleteds
    }

    public class tbl_comb_off_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int comp_off_rule_log_id { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_comb")]
        public int? comp_off_id { get; set; }
        public tbl_comb_off_rule_master tbl_comb { get; set; }

        [Required]
        [MaxLength(200)]
        public int minimum_working_hours { get; set; }
        public int minimum_working_minute { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted

    }

    public class tbl_holiday_master_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int holiday_log_id { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_holiday_master")]
        public int? holiday_id { get; set; }
        public tbl_holiday_master tbl_holiday_master { get; set; }

        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }

        public string holiday_name { get; set; }
        public DateTime holiday_date { get; set; }
        public byte is_applicable_on_all_comp { get; set; }//1 for all, 0 for particular
        public byte is_applicable_on_all_emp { get; set; }//1 for all, 0 for particular
        public byte is_applicable_on_all_religion { get; set; }//1 for all, 0 for particular
        public byte is_applicable_on_all_location { get; set; }//1 for all, 0 for particular


        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted


    }

    public class tbl_holi_comp_list_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_holiday_master_comp_list")]
        public int? lid { get; set; }
        public tbl_holiday_master_comp_list tbl_holiday_master_comp_list { get; set; }

        [ForeignKey("tbl_company_master")]
        public int? cid { get; set; }
        public tbl_company_master tbl_company_master { get; set; }

        [ForeignKey("tbl_location_master")]
        public int? l_id { get; set; }
        public tbl_location_master tbl_location_master { get; set; }

        [ForeignKey("tbl_holiday_master")]
        public int? h_id { get; set; }
        public tbl_holiday_master tbl_holiday_master { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted



    }

    public class tbl_holyd_emp_list_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_hol_emp")]
        public int? h_id { get; set; }
        public tbl_holiday_master_emp_list tbl_hol_emp { get; set; }


        [ForeignKey("tbl_employee_master")]
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }

        [ForeignKey("tbl_holiday_master")]
        public int? holiday_id { get; set; }
        public tbl_holiday_master tbl_holiday_master { get; set; }
        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted

    }

    public class tbl_holiday_rel_list_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_holi_rel")]
        public int? h_r_id { get; set; }
        public tbl_holiday_mstr_rel_list tbl_holi_rel { get; set; }

        [ForeignKey("tbl_holiday_master")]
        public int? holiday_id { get; set; }
        public tbl_holiday_master tbl_holiday_master { get; set; }
        [ForeignKey("tbl_religion_master")]
        public int? religion_id { get; set; }
        public tbl_religion_master tbl_religion_master { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted


    }

    #endregion


    #region **Start on 24-05-2019**
    public class tbl_menu_master
    {
        [Key]        
        public enmMenuMaster menu_id { get; set; }
        [StringLength(50)]
        [Display(Description = "menu_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,50}$", ErrorMessage = "Invalid Menu Name")]
        public string menu_name { get; set; }
        [ForeignKey("tbl_menu_master")]
        public int? parent_menu_id { get; set; }
        [StringLength(300)]
        [Display(Description = "IconUrl")]
        [RegularExpression(@"^[a-zA-Z'\s'\-]{1,300}$", ErrorMessage = "Invalid Icon Url")]
        public string IconUrl { get; set; } //SubMenu name
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_date { get; set; }
        //[StringLength(400)]
        //[Display(Description = "urll")]
        //[RegularExpression(@"^[a-zA-Z0-9'\s'\-'\/]{1,40}$", ErrorMessage = "Invalid Url")]
        public string urll { get; set; }
        public int SortingOrder { get; set; }
        public byte type { get; set; }
    }

    public class tbl_role_menu_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int role_menu_id { get; set; }
        public enmRoleMaster role_id { get; set; }
        public enmMenuMaster menu_id { get; set; }
       // public enmDocumentPermissionType permission_type { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_date { get; set; }

    }
    #endregion **End on 24-05-2019**

    #region **STARTED BY SUPRIYA KAKKAR ON 18-07-2019

    public class tbl_assets_request_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int asset_req_id { get; set; }

        public int company_id { get; set; }

        [ForeignKey("emp_master")]
        public int req_employee_id { get; set; }
        public tbl_employee_master emp_master { get; set; }

        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "asset_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Asset Name")]
        public string asset_name { get; set; }

        [MaxLength(500)]
        [StringLength(500)]
        [Display(Description = "description")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,500}$", ErrorMessage = "Invalid Description")]
        public string description { get; set; }


        [ForeignKey("assets_master")]
        public int? assets_master_id { get; set; }
        public tbl_assets_master assets_master { get; set; }


        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "asset_number")]        
        public string asset_number { get; set; }

        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }

        public byte asset_type { get; set; } // 0 for new Asset Request, 1 for replacement, 2 for submission

        public int is_finalapprove { get; set; }
        public int is_active { get; set; }

        public int created_by { get; set; }

        public int modified_by { get; set; }

        public DateTime created_dt { get; set; }

        public DateTime asset_issue_date { get; set; }
        public DateTime submission_date { get; set; }

        public byte is_deleted { get; set; }
        public byte is_closed { get; set; }
        public DateTime modified_dt { get; set; }

        public byte is_permanent { get; set; }

        public DateTime replace_dt { get; set; }

    }

    public class tbl_assets_approval
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int asset_approval_sno { get; set; }
        [ForeignKey("asset_req_mastr")] // Foreign Key here
        public int? asset_req_id { get; set; }
        public tbl_assets_request_master asset_req_mastr { get; set; }
        [ForeignKey("employee_master")] // Foreign Key here
        public int? emp_id { get; set; }
        public tbl_employee_master employee_master { get; set; }
        public byte is_final_approver { get; set; }
        public byte is_approve { get; set; }

        public byte approval_order { get; set; }
        public DateTime created_dt { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

        //[ForeignKey("approval_setting")]
        public int? approver_role_id { get; set; }


        //public tbl_loan_approval_setting approval_setting { get; set; }

    }


    #endregion ** END BY SUPRIYA ON 18-07-2019


    #region ADDED BY AMARJEET ON 18-07-2019 EVENT

    public class tbl_event_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int event_id { get; set; }

        [ForeignKey("tbl_company_master")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master tbl_company_master { get; set; }

        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "event_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,50}$", ErrorMessage = "Invalid Event Name")]
        public string event_name { get; set; }
        [MaxLength(150)]
        [StringLength(150)]
        [Display(Description = "event_place")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/]{1,150}$", ErrorMessage = "Invalid Event Place")]
        public string event_place { get; set; }

        public DateTime event_date { get; set; }

        public DateTime event_time { get; set; }

        public int is_active { get; set; }

        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
    }

    #endregion

    #region ADDED BY AMARJEET ON 22-07-2019 ADD LINK

    public class tbl_right_menu_link
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int menu_id { get; set; }

        [ForeignKey("tbl_company_master")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master tbl_company_master { get; set; }
        [StringLength(50)]
        [Display(Description = "menu_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,50}$", ErrorMessage = "Invalid Menu Name")]
        public string menu_name { get; set; }
        [StringLength(20)]
        [Display(Description = "icon_url")]
        [RegularExpression(@"^[a-zA-Z'\s'\-]{1,20}$", ErrorMessage = "Invalid Icon Url")]
        public string icon_url { get; set; } //SubMenu name
        [Display(Description = "url")]
       [RegularExpression(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$",ErrorMessage ="Invalid Url")]
        public string url { get; set; }

        public int sorting_order { get; set; }

        public int is_active { get; set; }

        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }


    }


    #endregion


    #region ** ADDED BY SUPRIYA ON 20-11-2019 **

    public class tbl_document_type_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int doc_type_id { get; set; }

        [ForeignKey("company_master")]
        public int company_id { get; set; }

        public tbl_company_master company_master { get; set; }
        [StringLength(50)]
        [Display(Description = "doc_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\%'\-]{1,50}$", ErrorMessage = "Invalid Document Name")]
        public string doc_name { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remarks { get; set; }

        public int is_deleted { get; set; }

        public int created_by { get; set; }
        public DateTime created_date { get; set; }

        public int modified_by { get; set; }
        public DateTime modified_date { get; set; }
    }

    public class tbl_emp_documents
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int emp_doc_id { get; set; }

        [ForeignKey("doc_master")]
        public int doc_type_id { get; set; }

        public tbl_document_type_master doc_master { get; set; }

        [ForeignKey("emp_master")]
        public int emp_id { get; set; }

        public tbl_employee_master emp_master { get; set; }

        [StringLength(50)]
        [Display(Description = "doc_no")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,50}$", ErrorMessage = "Invalid Document No")]
        public string doc_no { get; set; }

        public string doc_path { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remarks { get; set; }

        public int is_deleted { get; set; }

        public int created_by { get; set; }
        public DateTime created_date { get; set; }

        public int modified_by { get; set; }
        public DateTime modified_date { get; set; }

        [ForeignKey("company_master")]
        public int company_id { get; set; }

        public tbl_company_master company_master { get; set; }
    }

    public class tbl_employeementtype_settings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int typesetting_id { get; set; }

      
        [Display(Description = "grade_id")]
        [RegularExpression(@"^[0-9]{1,40}$", ErrorMessage = "Invalid Grade</br>")]

        [ForeignKey("grade_master")]
        public int grade_id { get; set; }

        public tbl_grade_master grade_master { get; set; }
       
        [Display(Description = "employeement_type")]
        [RegularExpression(@"^[0-9]{1,40}$", ErrorMessage = "Invalid Employeement Type</br>")]
        public int employeement_type { get; set; }
        
        [Display(Description = "notice_period")]
        [RegularExpression(@"^[0-9]{1,40}$", ErrorMessage = "Invalid Duration(in months)</br>")]
        public int notice_period { get; set; } // in months

        [Display(Description = "notice_period_days")]
        [RegularExpression(@"^[0-9]{1,40}$", ErrorMessage = "Invalid Duration(in days)</br>")]
        public int notice_period_days { get; set; } //in days

        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,40}$", ErrorMessage = "Invalid Remarks</br>")]
        public string remarks { get; set; }

       
        [Display(Description = "company_id")]
        [RegularExpression(@"^[0-9]{1,40}$", ErrorMessage = "Invalid Company ID</br>")]
        [ForeignKey("company_master")]
        public int company_id { get; set; }

        public tbl_company_master company_master { get; set; }

        public int is_deleted { get; set; }
        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public int modified_by { get; set; }

        public DateTime modified_date { get; set; }
    }
    #endregion ** END BY SUPRIYA ON 20-11-2019 **

    public class tbl_assets_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int asset_master_id { get; set; }
        [StringLength(50)]
        [Display(Description = "asset_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,50}$", ErrorMessage = "Invalid Asset Name")]
        public string asset_name { get; set; }


        [StringLength(50)]
        [Display(Description = "short_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,50}$", ErrorMessage = "Invalid Short Name")]
        public string short_name { get; set; }

        [StringLength(50)]
        [Display(Description = "description")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,50}$", ErrorMessage = "Invalid Description")]
        public string description { get; set; }

        public int is_deleted { get; set; }
        public int created_by { get; set; }

        public int modified_by { get; set; }

        public DateTime created_dt { get; set; }

        public DateTime modified_dt { get; set; }

        [ForeignKey("comp_master")]
        public int? company_id { get; set; }
        public tbl_company_master comp_master { get; set; }

        public virtual ICollection<tbl_assets_request_master> tbl_assets_request_master { get; set; }
    }


    public class tbl_bank_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int bank_id { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "bank_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\-]{1,100}$", ErrorMessage = "Invalid ")]
        public string bank_name { get; set; }

        public int bank_status { get; set; }

        public int is_deleted { get; set; }

        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }

        public int deleted_by { get; set; }
    }


    public class tbl_policy_master_documents
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pkid_tpmDoc { get; set; }  // primary key  must be public!
        public int fkid_policy { get; set; }
        public string document_path { get; set; }
        public int is_active { get; set; }
    }
    public class tbl_policy_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pkid_policy { get; set; }  // primary key  must be public!
        public string policy_name { get; set; }
        public string remarks { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public int is_deleted { get; set; }

    }

    public class tbl_current_openings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pkid_current_openings { get; set; }  // primary key  must be public!
        public int company_id { get; set; }
        public int department_id { get; set; }
        public int current_status { get; set; }
        public string opening_detail { get; set; }
        public DateTime posted_date { get; set; }
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
}

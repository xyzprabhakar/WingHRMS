using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB
{
    public class tbl_guid_detail
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        public DateTime genration_dt { get; set; }
        public int genrated_by { get; set; }
    }

    public class tbl_captcha_code_details
    {
        [Key]
        public string guid { get; set; }
        public string captcha_code { get; set; }
        public DateTime genration_dt { get; set; }
    }


    public class tbl_user_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }  // primary key  must be public!    
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "username")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,50}$", ErrorMessage = "Invalid User Name")]
        public string username { get; set; }
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "password")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\@]{1,50}$", ErrorMessage = "Invalid Password")]
        public string password { get; set; }
        public byte user_type { get; set; }
        public int is_active { get; set; }// this can be Block by admin so the user not able to log in the system
        public int created_by { get; set; }
        public byte is_logged_in { get; set; }
        public byte is_logged_blocked { get; set; }// block log in due to wrong attemp
        public DateTime logged_blocked_dt { get; set; }
        public DateTime last_logged_dt { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public int default_company_id { get; set; }
        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }

    }

    public class tbl_role_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int role_id { get; set; }  // primary key  must be public!   
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Description = "role_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,50}$", ErrorMessage = "Invalid Role Name")]
        public string role_name { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_claim_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int claim_master_id { get; set; }  // primary key  must be public!   
        [StringLength(100)]
        [Display(Description = "claim_master_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\\]{1,100}$", ErrorMessage = "Invalid Claim Master Name")]
        public string claim_master_name { get; set; }
        [StringLength(500)]
        public string description { get; set; }
        [StringLength(500)]
        public string method_type { get; set; }
        public byte is_post { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_role_claim_map
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int claim_master_id { get; set; }  // primary key  must be public!   
        [ForeignKey("tbl_role_master")] // Foreign Key here
        public int? role_id { get; set; }
        public tbl_role_master tbl_role_master { get; set; }
        [ForeignKey("tbl_claim_master")] // Foreign Key here
        public int? claim_id { get; set; }
        public tbl_claim_master tbl_claim_master { get; set; }
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_user_role_map
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int claim_master_id { get; set; }  // primary key  must be public!   
        [ForeignKey("tbl_role_master")] // Foreign Key here
        public int? role_id { get; set; }
        public tbl_role_master tbl_role_master { get; set; }
        [ForeignKey("tbl_user_master")] // Foreign Key here
        public int? user_id { get; set; }
        public tbl_user_master tbl_user_master { get; set; }
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    /// <summary>
    /// Developed By : Amarjeet
    /// Date : 29-11-2018
    /// Decs :  Employee id auto generated in this table 
    /// </summary>
    /// 
    public class tbl_employee_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int employee_id { get; set; }  // primary key  must be public!        
        [MaxLength(100)]
        public string emp_code { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

        public ICollection<tbl_employee_company_map> tbl_employee_company_map { get; set; }
        [InverseProperty("tbl_employee_id_details")]
        public ICollection<tbl_emp_officaial_sec> tbl_emp_officaial_sec { get; set; }
        public ICollection<tbl_emp_family_sec> tbl_emp_family_sec { get; set; }
        public ICollection<tbl_emp_personal_sec> tbl_emp_personal_sec { get; set; }
        public ICollection<tbl_emp_qualification_sec> tbl_emp_qualification_sec { get; set; }
        public ICollection<tbl_employment_type_master> tbl_employment_type_master { get; set; }
        public ICollection<tbl_user_master> tbl_user_master { get; set; }
        [InverseProperty("tem")]
        public ICollection<tbl_emp_manager> tbl_emp_manager { get; set; }
        public ICollection<tbl_emp_shift_allocation> tbl_emp_shift_allocation { get; set; }
        public ICollection<tbl_emp_grade_allocation> tbl_emp_grade_allocation { get; set; }
        public ICollection<tbl_emp_desi_allocation> tbl_emp_desi_allocation { get; set; }
    }

    public class tbl_employee_company_map
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }  // primary key  must be public!        
        [ForeignKey("tbl_employee_master")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }
        [ForeignKey("tbl_company_master")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master tbl_company_master { get; set; }
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }        
        public bool is_default { get; set; }
    }

    /// <summary>
    /// Developed By : Amarjeet
    /// Date : 29-11-2018
    /// Decs :  Employee Official Section Table
    /// </summary>
    /// 
    public class tbl_emp_shift_allocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int emp_shift_id { get; set; }  // primary key  must be public!  
        [ForeignKey("tbl_shift_master")] // Foreign Key here
        public int? shift_id { get; set; }
        public tbl_shift_master tbl_shift_master { get; set; }

        [ForeignKey("tem")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tem { get; set; }
        public DateTime applicable_from_date { get; set; }
        public DateTime created_date { get; set; }
        public int is_deleted { get; set; } = 0;
    }

    public class tbl_emp_manager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int emp_mgr_id { get; set; }  // primary key  must be public!  
        public byte final_approval { get; set; }//1 for first,2 for secon ...

        [ForeignKey("tbl_employee_master1")] // Foreign Key here
        public int? m_one_id { get; set; }
        public tbl_employee_master tbl_employee_master1 { get; set; }
        
        [ForeignKey("tem")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tem { get; set; }
        public DateTime applicable_from_date { get; set; }        
        public int is_deleted { get; set; }
        //public byte notify_manager_1 { get; set; }//0 for not Notify ,1 for Notify ...
        //public byte notify_manager_2 { get; set; }//0 for not Notify ,1 for Notify ...
        //public byte notify_manager_3 { get; set; }//0 for not Notify ,1 for Notify ...

    }

    public class tbl_emp_grade_allocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int emp_grade_id { get; set; }  // primary key  must be public!  
        [ForeignKey("tem")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tem { get; set; }
        public DateTime applicable_from_date { get; set; }
        public DateTime applicable_to_date { get; set; }
        [ForeignKey("tbl_grade_master")] // Foreign Key here
        public int? grade_id { get; set; }
        public tbl_grade_master tbl_grade_master { get; set; }
    }

    public class tbl_emp_desi_allocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int emp_grade_id { get; set; }  // primary key  must be public!  
        [ForeignKey("tem")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tem { get; set; }
        public DateTime applicable_from_date { get; set; }
        public DateTime applicable_to_date { get; set; }
        [ForeignKey("tbl_designation_master")] // Foreign Key here
        public int? desig_id { get; set; }
        public tbl_designation_master tbl_designation_master { get; set; }
    }

    public class tbl_emp_department_allocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("tbl_employee_master")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }
        [ForeignKey("tbl_department_master")] // Foreign Key here
        public int? department_id { get; set; }
        public tbl_department_master tbl_department_master { get; set; }
        [ForeignKey("tbl_sub_department_master")] // Foreign Key here
        public int? sub_dept_id { get; set; }
        public tbl_sub_department_master tbl_sub_department_master { get; set; }
        public DateTime effective_from_date { get; set; } = DateTime.Now;
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public int modifed_by { get; set; }
        public DateTime modifed_date { get; set; } = DateTime.Now;
    }

    public class tbl_emp_location_allocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("tbl_employee_master")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }
        [ForeignKey("tbl_location_master")] // Foreign Key here
        public int? location_id { get; set; }
        public tbl_location_master tbl_location_master { get; set; }
        [ForeignKey("tbl_sub_location_master")] // Foreign Key here
        public int? sub_location_id { get; set; }
        public tbl_sub_location_master tbl_sub_location_master { get; set; }
        public DateTime effective_from_date { get; set; } = DateTime.Now;
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public int modifed_by { get; set; }
        public DateTime modifed_date { get; set; } = DateTime.Now;
    }

    public class tbl_emp_weekoff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int emp_weekoff_id { get; set; }  // primary key  must be public!  
        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }
        public int is_fixed_weekly_off { get; set; } = 1; // If Fixed then set 1, id Dynamic then set 2
        public DateTime effective_from_date { get; set; } = DateTime.Now;
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public int modifed_by { get; set; }
        public DateTime modifed_date { get; set; } = DateTime.Now;

    }

    public class tbl_emp_joining_settings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [ForeignKey("tbl_employee_master")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }
        public DateTime group_joining_date { get; set; } = DateTime.Now;
        public DateTime date_of_joining { get; set; } = DateTime.Now;
        public DateTime date_of_birth { get; set; } = DateTime.Now;
        public DateTime confirmation_date { get; set; }
        public DateTime last_working_date { get; set; }
        public bool IsDefault { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public int modifed_by { get; set; }
        public DateTime modifed_date { get; set; } = DateTime.Now;
    }

    public class tbl_emp_attendance_setting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("tbl_employee_master")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }
        public byte is_ot_allowed { get; set; } //0 for Default, 1 for yes, 2 for No
        public int is_comb_off_allowed { get; set; }//0 for Default, 1 for yes, 2 for No
        public int MinOtHourReq { get; set; }//Minimum OT Hour Required to Calcualte OT
        public bool is_sandwiche_applicable { get; set; } = false;
        public byte punch_type { get; set; } // 0 for In single punch, Absent /1 for In single punch, Present /  2 for Punch exempted        
        public int is_mobile_attendence_access { get; set; }// 1 for yes, 0 for No
        public DateTime mobile_punch_from_date { get; set; }= DateTime.Now;
        public DateTime mobile_punch_to_date { get; set; } = DateTime.Now;
        public DateTime effective_from_date { get; set; } = DateTime.Now;
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; } = DateTime.Now;
        public int modifed_by { get; set; }
        public DateTime modifed_date { get; set; } = DateTime.Now;
    }

    public class tbl_emp_officaial_sec
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int emp_official_section_id { get; set; }  // primary key  must be public!  
        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }
        [ForeignKey("tbl_state")] // Foreign Key here
        public int? state_id { get; set; }
        public tbl_state tbl_state { get; set; }
        
        [MaxLength(64)]
        [Display(Description = "card_number")]        
        public string card_number { get; set; }
        public int gender { get; set; } // 1 for Female , 2 Male , 3 Other 

        [StringLength(16)]
        [Display(Description = "salutation")]
        [RegularExpression("^([a-zA-Z .&'-]+)$", ErrorMessage = "Invalid Salutation")]
        public string salutation { get; set; }

        [StringLength(32)]
        [Display(Description = "employee_first_name")]
        [RegularExpression("^([a-zA-Z .&'-]+)$", ErrorMessage = "Invalid First Name")]
        [MaxLength(32)]
        public string employee_first_name { get; set; }

        [StringLength(32)]
        [Display(Description = "employee_middle_name")]
        [RegularExpression("^([a-zA-Z .&'-]+)$", ErrorMessage = "Invalid Middle Name")]
        [MaxLength(32)]
        public string employee_middle_name { get; set; }

        [StringLength(32)]
        [Display(Description = "employee_last_name")]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Last Name")]
        [MaxLength(32)]
        public string employee_last_name { get; set; }
        [MaxLength(32)]
        public string emp_father_name { get; set; }
        [MaxLength(32)]
        public string nationality { get; set; }
        [ForeignKey("tbl_religion_master")] // Foreign Key here
        public int? religion_id { get; set; }
        public tbl_religion_master tbl_religion_master { get; set; }
        public byte marital_status { get; set; }
        [MaxLength(64)]
        [StringLength(100)]
        [Display(Description = "hr_spoc")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid HR Spoc")]
        public string hr_spoc { get; set; }
        [MaxLength(256)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string official_email_id { get; set; }
        [MaxLength(20)]
        public string official_contact_no { get; set; } = "0000";
        public string employee_photo_path { get; set; }
                
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remarks { get; set; }        
        
    }

    /// <summary>
    /// Developed By : Amarjeet
    /// Date : 28-11-2018
    /// Decs :  Employee Family Section
    /// </summary>
    public class tbl_emp_family_sec
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int emp_family_section_id { get; set; }  // primary key  must be public!  

        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }

        public string relation { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "occupation")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,200}$", ErrorMessage = "Invalid Occupation ")]
        public string occupation { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "name_as_per_aadhar_card")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,200}$", ErrorMessage = "Invalid Name as Per Aadhar Card")]
        public string name_as_per_aadhar_card { get; set; }
        public DateTime date_of_birth { get; set; }
        public string gender { get; set; }
        public byte dependent { get; set; } // 0 Yes, 1 No
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remark")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remark { get; set; }
        public string document_image { get; set; }

        public byte is_nominee { get; set; } // 0 Yes , 1 No
        public double nominee_percentage { get; set; } // If nominee yes then %         
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

        [MaxLength(12)]
        [StringLength(12)]
        [Display(Description = "aadha_card_number")]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Invalid Aadhar CARD Number...")]
        public string aadhar_card_no { get; set; }
    }

    public class tbl_emp_bank_details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int bank_details_id { get; set; }
        public enmPaymentMode payment_mode { get; set; }
        [ForeignKey("bank_mstr")]
        public int? bank_id { get; set; }
        public tbl_bank_master bank_mstr { get; set; }
        [MaxLength(150)]
        [RegularExpression(@"^[A-Za-z0-9,.]*$", ErrorMessage = "Invalid Branch")]
        public string branch_name { get; set; }
        [MaxLength(11)]
        [StringLength(11)]
        [Display(Description = "ifsc_code")]
        [RegularExpression(@"^[A-Za-z]{4}0[A-Z0-9a-z]{6}$", ErrorMessage = "Invalid IFSC Code")]
        public string ifsc_code { get; set; }
        [MaxLength(18)]
        [StringLength(18)]
        [Display(Description = "bank_acc")]
        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Invalid Bank Account No.")]
        public string bank_acc { get; set; }
        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_emp_pan_details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pan_details_id { get; set; }
        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "pan_card_name")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,100}$", ErrorMessage = "Invalid PAN CARD Name...")]
        public string pan_card_name { get; set; }

        [MaxLength(10)]
        [StringLength(10)]
        [Display(Description = "pan_card_number")]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN CARD Number...")]
        public string pan_card_number { get; set; }
        public string pan_card_image { get; set; }

        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_emp_adhar_details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pan_details_id { get; set; }
        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z'\s]{1,100}$", ErrorMessage = "Invalid PAN CARD Name...")]
        public string aadha_card_name { get; set; }
        [MaxLength(12)]
        [StringLength(12)]
        [Display(Description = "aadha_card_number")]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Invalid Aadhar CARD Number...")]
        public string aadha_card_number { get; set; }
        public string aadha_card_image { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_emp_pf_details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pf_details_id { get; set; }

        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }

        public byte is_pf_applicable { get; set; }
        [Display(Description = "aadha_card_number")]
        [RegularExpression(@"^[0-9]$", ErrorMessage = "Invalid UAN...")]
        public string uan_number { get; set; }
        [MaxLength(30)]
        [StringLength(30)]

        //[RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Invalid PF Number")]
        //[RegularExpression(@" ^[A-Z]{2}/\d{5}/\d{7}$", ErrorMessage = "Invalid PF Number")]
        public string pf_number { get; set; }
        public enmPFGroup pf_group { get; set; }
        public byte is_vpf_applicable { get; set; }
        public enmVPFGroup vpf_Group { get; set; }
        public byte is_eps_applicable { get; set; }
        public double vpf_amount { get; set; }
        public double pf_celing { get; set; }
        [ForeignKey("bank_mstr")]
        public int? bank_id { get; set; }
        public tbl_bank_master bank_mstr { get; set; }
        [MaxLength(11)]
        [StringLength(11)]
        [Display(Description = "ifsc_code")]
        [RegularExpression(@"^[A-Za-z]{4}0[A-Z0-9a-z]{6}$", ErrorMessage = "Invalid IFSC Code")]
        public string ifsc_code { get; set; }
        [MaxLength(18)]
        [StringLength(18)]
        [Display(Description = "bank_acc")]
        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Invalid Bank Account No.")]
        public string bank_acc { get; set; }

        public byte is_pt_applicable { get; set; }
        public string pt_group { get; set; }
        public string spt_Description { get; set; }


        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_emp_esic_details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int esic_details_id { get; set; }

        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }

        public byte is_esic_applicable { get; set; }
        [MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Invalid PF Number")]
        public string esic_number { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }



    /// <summary>
    /// Developed By : Amarjeet
    /// Date : 28-11-2018
    /// Decs :  Employee Personal Section
    /// </summary>
    public class tbl_emp_personal_sec
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int emp_personal_section_id { get; set; }  // primary key  must be public!  

        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }

        public byte blood_group { get; set; }//from enum
        public string blood_group_doc { get; set; }
        [MaxLength(15)]
        public string primary_contact_number { get; set; }
        [MaxLength(15)]
        public string secondary_contact_number { get; set; }
        [MaxLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Primary Email ID..")]

        public string primary_email_id { get; set; }
        [MaxLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Secondary Email ID")]
        public string secondary_email_id { get; set; }

        //	Permanent Address
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "permanent_address_line_one")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/'\.'\,]{1,250}$", ErrorMessage = "Invalid Permanent Address in Address Line one ((.),(,) and special characters are not allowed)")]
        public string permanent_address_line_one { get; set; }
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "permanent_address_line_two")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/'\.'\,]{1,250}$", ErrorMessage = "Invalid Permanent Address in Address Line Two ((.),(,) and special characters are not allowed)")]
        public string permanent_address_line_two { get; set; }
        public int permanent_pin_code { get; set; }
        public int permanent_city { get; set; }
        public int permanent_state { get; set; }
        public int permanent_country { get; set; }
        public string permanent_document_type { get; set; }
        public string permanent_address_proof_document { get; set; }

        //	Corresponding Address
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "corresponding_address_line_one")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/'\.'\,]{1,250}$", ErrorMessage = "Invalid Corresponding Address in Address Line one ((.),(,) and special characters are not allowed)")]
        public string corresponding_address_line_one { get; set; }

        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "corresponding_address_line_two")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/'\.'\,]{1,250}$", ErrorMessage = "Invalid Corresponding Address in Address Line Two ((.),(,) and special characters are not allowed)")]
        public string corresponding_address_line_two { get; set; }
        public int corresponding_pin_code { get; set; }
        public int corresponding_city { get; set; }
        public int corresponding_state { get; set; }
        public int corresponding_country { get; set; }
        public string corresponding_document_type { get; set; }
        public string corresponding_address_proof_document { get; set; }

        //Emergency contact
        [StringLength(200)]
        [Display(Description = "emergency_contact_name")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,200}$", ErrorMessage = "Invalid Emergency Contact Name")]
        public string emergency_contact_name { get; set; }
        public string emergency_contact_relation { get; set; }
        [MaxLength(15)]
        public string emergency_contact_mobile_number { get; set; }
        public byte is_emg_same_as_permanent { get; set; } //0-means fill new, if 1 then permanent address, 2 for corresponding address 

        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "emergency_contact_line_one")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/'\.'\,]{1,250}$", ErrorMessage = "Invalid Emergency Address in Address Line one")]
        public string emergency_contact_line_one { get; set; }
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "emergency_contact_line_two")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/'\.'\,]{1,250}$", ErrorMessage = "Invalid Emergency Address in Address Line Two")]
        public string emergency_contact_line_two { get; set; }
        public int emergency_contact_pin_code { get; set; }
        public int emergency_contact_city { get; set; }
        public int emergency_contact_state { get; set; }
        public int emergency_contact_country { get; set; }
        public string emergency_contact_document_type { get; set; }
        public string emergency_contact_address_proof_document { get; set; }


        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }




    }

    /// <summary>
    /// Developed By : Amarjeet
    /// Date : 28-11-2018
    /// Decs :  Employee Qualification Section
    /// </summary>
    public class tbl_emp_qualification_sec
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int emp_qualification_section_id { get; set; }  // primary key  must be public!  

        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "board_or_university")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,200}$", ErrorMessage = "Invalid Board/Universty Name")]
        public string board_or_university { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "institute_or_school")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,200}$", ErrorMessage = "Invalid Institute/School Name")]
        public string institute_or_school { get; set; }
        public string passing_year { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "stream")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,200}$", ErrorMessage = "Invalid Stream")]
        public string stream { get; set; }
        public string title { get; set; }
        public byte education_type { get; set; } // 1 for Regular, 2 for Part-time , 3 for distance
        public byte education_level { get; set; } // 1 Not Educated, 2 Primary Education, 3 Secondary, 4 Sr Secondary, 5 Graduation, 6 Post Graduation, 7 Doctorate 

        public string marks_division_cgpa { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remark")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remark { get; set; }
        public string document_image { get; set; }
        public byte is_deleted { get; set; }// 0: deleted  ,  //1: Active  ,  //2:Pending Approval 
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

    }

    public class tbl_employment_type_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int employment_type_id { get; set; }  // primary key  must be public!
        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }
        public byte employment_type { get; set; } //1 temporary,2 Probation,3 Confirmend, 4 Contract, 10 notice,99 FNF,(100 terminate     no entry coreposnding to 100   )
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public DateTime effective_date { get; set; }
    }


    public class tbl_health_card_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int health_card_id { get; set; }
        public int company_id { get; set; }
        [ForeignKey("tbl_emp_master")]
        public int employee_id { get; set; }
        public tbl_employee_master tbl_emp_master { get; set; }
        public string health_card_path { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remark")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remarks { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }

    }


    #region **STARTED BY SUPRIYA ON 16-07-2019 **

    #endregion ** END BY SUPRIYA ON 16-07-2019 ** 


    #region **STARTED BY SUPRIYA ON 03-09-2019 **
    public class tbl_user_login_logs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int log_id { get; set; }
        public string login_ip { get; set; }
        public string user_agent { get; set; }
        public byte is_wrong_attempt { get; set; }
        [ForeignKey("user_master")]
        public int? user_id { get; set; }
        public tbl_user_master user_master { get; set; }
        [ForeignKey("emp_master")]
        public int? emp_id { get; set; }
        public tbl_employee_master emp_master { get; set; }
        public DateTime login_date_time { get; set; }

    }

    public class tbl_active_inactive_user_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int acinac_user_id { get; set; }

        [ForeignKey("user_master")]
        public int? user_id { get; set; }
        public tbl_user_master user_master { get; set; }
        public byte transaction_type { get; set; }//  1 For Change Password (Self), 2 Password Change by Admin 
                                                  //  11 Login Block (By Wrong Attempt), 12 -Login block by Admin
                                                  //   13 User Block by Admin
                                                  // 21 Login un Blocked by Admin, 22 User Unblocked by Admin

        public string remarks { get; set; }
        public int is_deleted { get; set; }
        public DateTime created_on { get; set; }
        public int created_by { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_date { get; set; }

    }
    #endregion**END BY SUPRIYA ON 03-09-2019 **



    public class tbl_emp_working_role_allocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int emp_working_role_id { get; set; }  // primary key  must be public!  

        [ForeignKey("tem")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tem { get; set; }

        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        [ForeignKey("tbl_working_role")] // Foreign Key here
        public int? work_r_id { get; set; }
        public tbl_working_role tbl_working_role { get; set; }
    }

    public class tbl_emp_separation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sepration_id { get; set; }  //withdrawal_id

        [ForeignKey("emp_mstr")]
        public int emp_id { get; set; }
        public tbl_employee_master emp_mstr { get; set; }

        [ForeignKey("comp_master")]
        public int company_id { get; set; }
        public tbl_company_master comp_master { get; set; }

        public DateTime resignation_dt { get; set; }//resign_dt
        public DateTime req_relieving_date { get; set; }
        public int req_notice_days { get; set; }
        public int diff_notice_days { get; set; }
        public DateTime policy_relieving_dt { get; set; }
        public int is_relieving_dt_change { get; set; }
        public DateTime last_wrking_dt { get; set; }

        public string req_reason { get; set; } //reason
        public string req_remarks { get; set; }  //remark

        [ForeignKey("app1_emp")]
        public int? approver1_id { get; set; }
        public tbl_employee_master app1_emp { get; set; }
        public int is_approved1 { get; set; }
        public string app1_remarks { get; set; }
        public DateTime app1_dt { get; set; }

        [ForeignKey("app2_emp")]
        public int? approver2_id { get; set; }
        public tbl_employee_master app2_emp { get; set; }
        public int is_approved2 { get; set; }
        public string app2_remarks { get; set; }
        public DateTime app2_dt { get; set; }

        [ForeignKey("app3_emp")]
        public int? apprver3_id { get; set; }
        public tbl_employee_master app3_emp { get; set; }
        public int is_approved3 { get; set; }
        public string app3_remarks { get; set; }
        public DateTime app3_dt { get; set; }

        [ForeignKey("admin_emp")]
        public int? admin_id { get; set; }
        public tbl_employee_master admin_emp { get; set; }
        public int is_admin_approved { get; set; }
        public string admin_remarks { get; set; }
        public DateTime admin_dt { get; set; }

        public int is_final_approve { get; set; } //0 for pending,1 for approve,2 for reject,3 for Cancel,4 In Process
        public DateTime final_relieve_dt { get; set; }

        public int is_cancel { get; set; }
        public string cancel_remarks { get; set; }
        public DateTime cancelation_dt { get; set; }

        public int is_deleted { get; set; } //0 for request raised,1 for delete by system,2// delete before request approval
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
        public int notice_day { get; set; }

        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "withdrawal_type")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,250}$", ErrorMessage = "Invalid Withdrawal Type")]
        public string withdrawal_type { get; set; }
        public int salary_process_type { get; set; } // 1 for Salary Hold for Resign,2 for Salary Process and Hold, 3 for Salary Process
        public int gratuity { get; set; }
        public int is_withdrawal { get; set; }
        public int is_no_due_cleared { get; set; }
        public int is_kt_transfered { get; set; }
        public string ref_doc_path { get; set; }

    }
    public class tbl_approved_emp_separation_cancellation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int pkid_AppEmpSepCancel { get; set; }

        [ForeignKey("emp_sep")]
        public int fkid_empSepration { get; set; }
        public tbl_company_master emp_sep { get; set; }

        public DateTime cancellation_dt { get; set; }
        public string cancel_remarks { get; set; }

        [ForeignKey("app1_emp")]
        public int? approver1_id { get; set; }
        public tbl_employee_master app1_emp { get; set; }
        public int is_approved1 { get; set; }
        public string app1_remarks { get; set; }
        public DateTime app1_dt { get; set; }

        [ForeignKey("app2_emp")]
        public int? approver2_id { get; set; }
        public tbl_employee_master app2_emp { get; set; }
        public int is_approved2 { get; set; }
        public string app2_remarks { get; set; }
        public DateTime app2_dt { get; set; }

        [ForeignKey("app3_emp")]
        public int? apprver3_id { get; set; }
        public tbl_employee_master app3_emp { get; set; }
        public int is_approved3 { get; set; }
        public string app3_remarks { get; set; }
        public DateTime app3_dt { get; set; }

        [ForeignKey("admin_emp")]
        public int? admin_id { get; set; }
        public tbl_employee_master admin_emp { get; set; }
        public int is_admin_approved { get; set; }
        public string admin_remarks { get; set; }
        public DateTime admin_dt { get; set; }

        public int is_final_approve { get; set; } //0 for pending,1 for approve,2 for reject,3 for Cancel,4 In Process
        public int is_deleted { get; set; } //0 for request raised,1 for delete by system,2// delete before request approval
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_dt { get; set; }
    }

    public class tbl_emp_prev_employement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int emp_pr_employment_id { get; set; }

        [ForeignKey("comp_mstr")]
        public int current_comp_id { get; set; }

        public tbl_company_master comp_mstr { get; set; }

        [ForeignKey("emp_mstr")]
        public int? emp_id { get; set; }
        public tbl_employee_master emp_mstr { get; set; }

        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "pr_comp_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\.]{1,100}$", ErrorMessage = "Invalid Company Name")]
        public string pr_comp_name { get; set; }
        [MaxLength(500)]
        [StringLength(500)]
        [Display(Description = "pr_comp_address")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/]{1,500}$", ErrorMessage = "Invalid Company Address")]
        public string pr_comp_address { get; set; }

        public DateTime pr_comp_doj { get; set; }

        public DateTime pr_comp_relieve_dt { get; set; }

        public int is_relieved { get; set; } //1 For Yes,2 For No,3 For Pending
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "designation")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,100}$", ErrorMessage = "Invalid Designation")]
        public string designation { get; set; }

        public decimal salary { get; set; }

        public int job_type { get; set; }//1 For full time,2 for Part time
        [MaxLength(500)]
        [StringLength(500)]
        [Display(Description = "relieve_reason")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,500}$", ErrorMessage = "Invalid Reason")]
        public string relieve_reason { get; set; }

        public string reporting_to { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "reporting_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,100}$", ErrorMessage = "Invalid Reporting Name")]
        public string reporting_name { get; set; }

        public string rpting_no { get; set; }
        [MaxLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Primary Email ID..")]
        public string rpting_email { get; set; }
        [MaxLength(500)]
        [StringLength(500)]
        [Display(Description = "remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,500}$", ErrorMessage = "Invalid Remarks")]
        public string remarks { get; set; }

        public int is_deleted { get; set; }

        public int created_by { get; set; }

        public DateTime created_dt { get; set; }

        public int last_modified_by { get; set; }

        public DateTime last_modifed_dt { get; set; }

        public decimal relevant_exp { get; set; }

        public decimal total_exp { get; set; }

    }

    public class tbl_emp_withdrawal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int withdrawal_id { get; set; }

        [ForeignKey("comp_mstr")]
        public int? company_id { get; set; }

        public tbl_company_master comp_mstr { get; set; }

        [ForeignKey("emp_mstr")]
        public int? emp_id { get; set; }

        public tbl_employee_master emp_mstr { get; set; }

        public DateTime resign_dt { get; set; }

        public DateTime last_wrking_dt { get; set; }

        public int notice_day { get; set; }
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "reason")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,250}$", ErrorMessage = "Invalid Reason")]
        public string reason { get; set; }

        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "withdrawal_type")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,250}$", ErrorMessage = "Invalid Withdrawal Type")]
        public string withdrawal_type { get; set; }

        public byte salary_process_type { get; set; } // 1 for Salary Hold for Resign,2 for Salary Process and Hold, 3 for Salary Process

        public byte gratuity { get; set; }
        [MaxLength(500)]
        [StringLength(500)]
        [Display(Description = "remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,500}$", ErrorMessage = "Invalid Remarks")]
        public string remarks { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_dt { get; set; }
        public int modify_by { get; set; }
        public DateTime modified_dt { get; set; }
    }

    
}


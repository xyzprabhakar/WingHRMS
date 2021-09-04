using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB
{
    public class tbl_leave_type
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_type_id { get; set; }  // primary key  must be public! 
        [StringLength(200)]
        [Display(Description = "leave_type_name")]
        [RegularExpression(@"^[0-9a-zA-Z_'\s'\,]{1,200}$", ErrorMessage = "Invalid Leave Type Name")]
        public string leave_type_name { get; set; }
        [StringLength(200)]
        [Display(Description = "description")]
        [RegularExpression(@"^[0-9a-zA-Z_'\s'\,]{1,200}$", ErrorMessage = "Invalid Description")]
        public string description { get; set; }
        [Range(0, 1)]
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

        public int? _is_el { get; set; }
        //public ICollection<tbl_leave_info> tbl_leave_info { get; set; }

    }

    public class tbl_leave_info
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_info_id { get; set; }  // primary key  must be public!
        [Range(1,2)]
        public byte leave_type { get; set; } // 1 for Paid, 2 Unpaid
        public DateTime leave_tenure_from_date { get; set; } //
        public DateTime leave_tenure_to_date { get; set; } //
        public byte is_active { get; set; }
        [ForeignKey("tbl_leave_type")] // Foreign Key here
        public int? leave_type_id { get; set; }
        public tbl_leave_type tbl_leave_type { get; set; }
        //public ICollection<tbl_leave_applicablity> tbl_leave_applicablity { get; set; }
        //public ICollection<tbl_leave_credit> tbl_leave_credit { get; set; }
        //public ICollection<tbl_leave_rule> tbl_leave_rule { get; set; }
        //public ICollection<tbl_leave_cashable> tbl_leave_cashable { get; set; }
        

    }

    public class tbl_leave_applicablity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_applicablity_id { get; set; }  // primary key  must be public!
        [ForeignKey("tbl_leave_info")] // Foreign Key here
        public int? leave_info_id { get; set; }
        public tbl_leave_info tbl_leave_info { get; set; }
        [Range(0, 1)]
        public byte is_aplicable_on_all_company { get; set; }
        [Range(0, 1)]
        public byte is_aplicable_on_all_location { get; set; }
        [Range(0, 1)]
        public byte is_aplicable_on_all_department { get; set; }
        [Range(0, 1)]
        public byte is_aplicable_on_all_religion { get; set; }
        [Range(1, 3)]
        public byte leave_applicable_for { get; set; } // 1 for Full day , 2 for Half day , 3 for hours and minutes (HH:MM)
        [Range(1, 2)]
        public byte day_part { get; set; } // 1 For First Half, 2 Second half 
        [Range(typeof(DateTime), "1/1/2000", "1/2/2012", ErrorMessage = "Date is out of Range")]
        public DateTime leave_applicable_in_hours_and_minutes { get; set; } // If leave applicable in hours and minutes (HH:MM)
        [Range(1, 2)]
        public byte employee_can_apply { get; set; } // 1 for yes, 2 for No
        [Range(1, 2)]
        public byte admin_can_apply { get; set; } // 1 for yes, 2 for No
        [Range(0, 1)]
        public byte is_aplicable_on_all_emp_type { get; set; }
        [Range(0, 1)]
        public byte is_deleted { get; set; }

        //public ICollection<tbl_leave_app_on_emp_type> tbl_leave_app_on_emp_type { get; set; }
        //public ICollection<tbl_leave_appcbl_on_company> tbl_leave_appcbl_on_company { get; set; }
        //public ICollection<tbl_leave_app_on_dept> tbl_leave_app_on_dept { get; set; }
        //public ICollection<tbl_leave_appcbl_on_religion> tbl_leave_appcbl_on_religion { get; set; }
        
    }

    public class tbl_leave_app_on_emp_type
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }
        [ForeignKey("tbl_l_app")] // Foreign Key here
        public int? l_app_id { get; set; }
        public tbl_leave_applicablity tbl_l_app { get; set; }
        public byte employment_type { get; set; } //1 temporary,2 Probation,3 Confirmend, 4 Contract, 10 notice,,99 FNF,(100 terminate     no entry coreposnding to 100   )
        public byte is_deleted { get; set; }
    }

    public class tbl_leave_appcbl_on_company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }
        [ForeignKey("tbl_leave_applicablity")] // Foreign Key here
        public int? l_a_id { get; set; }
        public tbl_leave_applicablity tbl_leave_applicablity { get; set; }
        [ForeignKey("tbl_company_master")] // Foreign Key here
        public int? c_id { get; set; }
        public tbl_company_master tbl_company_master { get; set; }
        [ForeignKey("tbl_location_master")] // Foreign Key here
        public int? location_id { get; set; }
        public tbl_location_master tbl_location_master { get; set; }
        public byte is_deleted { get; set; }
    }

    public class tbl_leave_app_on_dept
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }
        [ForeignKey("l")] // Foreign Key here
        public int? lid { get; set; }
        public tbl_leave_applicablity l { get; set; }
        [ForeignKey("d")] // Foreign Key here
        public int? id { get; set; }
        public tbl_department_master d { get; set; }
        public byte is_deleted { get; set; }
    }

    public class tbl_leave_appcbl_on_religion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }
        [ForeignKey("tbl_leave_applicablity")] // Foreign Key here
        public int? l_app_id { get; set; }
        public tbl_leave_applicablity tbl_leave_applicablity { get; set; }
        [ForeignKey("tbl_religion_master")] // Foreign Key here
        public int? religion_id { get; set; }
        public tbl_religion_master tbl_religion_master { get; set; }
        public byte is_deleted { get; set; }
    }
    
    public class tbl_leave_credit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_credit_id { get; set; }  // primary key  must be public! 
        [ForeignKey("tbl_leave_info")] // Foreign Key here
        public int? leave_info_id { get; set; }
        public tbl_leave_info tbl_leave_info { get; set; }
        [Range(1, 4)]
        public byte frequency_type { get; set; }//1-monthly,2 Quaterly,3 Half yealry,4 yearly
        [Range(1, 31)]
        public decimal leave_credit_day { get; set; }//day on Which Leave is Going to be Credit
        [Range(0, 1)]
        public byte is_half_day_applicable { get; set; }
        [Range(1, 100)]
        public double leave_credit_number { get; set; }//how many leave is going to credit in a frequence
        // public byte is_credit_attd_depend { get; set; }
        [Range(0,1)]
        public byte is_applicable_for_advance { get; set; }
        [Range(-60, 60)]
        public int advance_applicable_day { get; set; }
        [Range(0, 1)]
        public byte is_leave_accrue { get; set; }//Leave is Accured within the leave tenure date
        [Range(0, 1000)]
        public int max_accrue { get; set; }//if no Limit then 10000
        [Range(0, 1 )]
        public byte is_required_certificate { get; set; }
        [RegularExpression(@"^[0-9a-zA-Z_'\s'\,]{1,100}$", ErrorMessage = "Invalid data")]
        public string certificate_path { get; set; }
        [Range(0, 1)]
        public byte is_deleted { get; set; }
    }

    public class tbl_leave_rule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_credit_id { get; set; }  // primary key  must be public! 
        [ForeignKey("tbl_leave_info")] // Foreign Key here
        public int? leave_info_id { get; set; }
        public tbl_leave_info tbl_leave_info { get; set; }

        [DefaultValue(typeof(DateTime), "2999-12-31")]
        [Range(typeof(DateTime), "1900-01-01", "2999-12-31")]
        public DateTime applicable_if_employee_joined_before_dt { get; set; } //default 1-jan-2300
        //for those Policy if Hr Want that if Employee did not n take  these no of 
        //Holiday with the Fincial Tenure then it  will expired
        //ex employee has 14 Live in Current Fiscal and 10 Leave in Previous Fiscal
        //but the maximum_leave_clubbed_in_tenure_number_of_leave = 7 then in next Fiscal year it will only transfer 7+10
        [Range(0,50)]
        public byte maximum_leave_clubbed_in_tenure_number_of_leave { get; set; }
        [Range(-100, 100)]
        public int maxi_negative_leave_applicable { get; set; }
        [Range(0,1)]
        public byte certificate_require_for_min_no_of_day { get; set; }
        [Range(1, 2)]
        public int minimum_leave_applicable { get; set; } // 1 for	0.5 (half day) , 2  for 1 (full day)
        [Range(-100, 100)]
        public byte number_maximum_negative_leave_balance { get; set; }

        [Range(1, 2)]
        public byte maximum_leave_can_be_taken_type { get; set; } // if 1 for days, 2 for quarter
        [Range(1, 30)]
        public byte maximum_leave_can_be_taken { get; set; }
        [Range(1, 30)]
        public byte maximum_leave_can_be_in_day { get; set; }
        [Range(1, 30)]
        public byte maximum_leave_can_be_taken_in_quater { get; set; }

        [Range(1, 2)]
        public byte can_carried_forward { get; set; } // 1 for Yes, 2 for No
        [Range(1, 50)]
        public int maximum_carried_forward { get; set; } // 1 for Yes, 2 for No
        [Range(0, 4)]
        public byte applied_sandwich_rule { get; set; }//0 no Rule Applied, 
                                                       //1.	H / WO counted as absent, if employees absent before WO / H*
                                                       //2	H / WO counted as absent, if employees absent after WO / H * (drop down)
                                                       //3  H / WO counted as absent, if employees absent before / after WO / H* (drop down)

        [Range(0, 1)]
        public byte is_deleted { get; set; }
    }

    public class tbl_leave_cashable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_cashable_id { get; set; }  // primary key  must be public! 
        [ForeignKey("tbl_leave_info")] // Foreign Key here
        public int? leave_info_id { get; set; }
        public tbl_leave_info tbl_leave_info { get; set; }
        public byte is_cashable { get; set; }
        public byte cashable_type { get; set; }//0-for all, 1 dor on separation,2 for after n year
        public byte cashable_after_year { get; set; }//default 1
        public int maximum_cashable_leave { get; set; }//default is 10000
        public byte is_deleted { get; set; }
    }


    #region Log table

    public class tbl_leave_type_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_type_log_id { get; set; }  // primary key  must be public! 

        [ForeignKey("tbl_leave_type")] // Foreign Key here
        public int? leave_type_id { get; set; }
        public tbl_leave_type tbl_leave_type { get; set; }

        public string leave_type_name { get; set; }
        public string description { get; set; }


        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted

    }


    public class tbl_leave_info_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_info_log_id { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_leave_info")] // Foreign Key here
        public int? leave_info_id { get; set; }
        public tbl_leave_info tbl_leave_info { get; set; }

        public byte leave_type { get; set; } // 1 for Paid, 2 Unpaid
        public DateTime leave_tenure_from_date { get; set; } //
        public DateTime leave_tenure_to_date { get; set; } //

        [ForeignKey("tbl_leave_type")] // Foreign Key here
        public int? leave_type_id { get; set; }
        public tbl_leave_type tbl_leave_type { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted

    }

    public class tbl_leave_applicablity_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_applicablity_log_id { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_lev_app")] // Foreign Key here
        public int? l_app_id { get; set; }
        public tbl_leave_applicablity tbl_lev_app { get; set; }

        [ForeignKey("tbl_leave_info")] // Foreign Key here
        public int? leave_info_id { get; set; }
        public tbl_leave_info tbl_leave_info { get; set; }

        public byte is_aplicable_on_all_company { get; set; }
        public byte is_aplicable_on_all_location { get; set; }
        public byte is_aplicable_on_all_department { get; set; }
        public byte is_aplicable_on_all_religion { get; set; }
        public byte leave_applicable_for { get; set; } // 1 for Full day , 2 for Half day , 3 for hours and minutes (HH:MM)
        public byte day_part { get; set; } // 1 For First Half, 2 Second half 
        public DateTime leave_applicable_in_hours_and_minutes { get; set; } // If leave applicable in hours and minutes (HH:MM)
        public byte employee_can_apply { get; set; } // 1 for yes, 2 for No
        public byte admin_can_apply { get; set; } // 1 for yes, 2 for No
        public byte is_aplicable_on_all_emp_type { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }

    public class tbl_leave_app_emp_type_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int log_sno { get; set; }

        [ForeignKey("tbl_leave_app_on_emp_type")] // Foreign Key here
        public int? sno { get; set; }
        public tbl_leave_app_on_emp_type tbl_leave_app_on_emp_type { get; set; }

        [ForeignKey("tbl_leave_applicablity")] // Foreign Key here
        public int? lid { get; set; }
        public tbl_leave_applicablity tbl_leave_applicablity { get; set; }

        public byte employment_type { get; set; } //1 temporary,2 Probation,3 Confirmend, 4 Contract, 10 notice,99FNF,(100 terminate     no entry coreposnding to 100   )

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }

    public class tbl_leave_appcbly_on_comp_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int log_sno { get; set; }

        [ForeignKey("tbl_leave_appcbl_on_company")] // Foreign Key here
        public int? sno { get; set; }
        public tbl_leave_appcbl_on_company tbl_leave_appcbl_on_company { get; set; }


        [ForeignKey("tbl_leave_applicablity")] // Foreign Key here
        public int? l_a_id { get; set; }
        public tbl_leave_applicablity tbl_leave_applicablity { get; set; }

        [ForeignKey("tbl_company_master")] // Foreign Key here
        public int? c_id { get; set; }
        public tbl_company_master tbl_company_master { get; set; }
        [ForeignKey("tbl_location_master")] // Foreign Key here
        public int? l_id { get; set; }
        public tbl_location_master tbl_location_master { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }

    public class tbl_leave_appcbl_on_dept_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int log_sno { get; set; }


        [ForeignKey("tbl_leave_app_on_dept")] // Foreign Key here
        public int? sno { get; set; }
        public tbl_leave_app_on_dept tbl_leave_app_on_dept { get; set; }

        [ForeignKey("tbl_leave_applicablity")] // Foreign Key here
        public int? l_a_id { get; set; }
        public tbl_leave_applicablity tbl_leave_applicablity { get; set; }

        [ForeignKey("tbl_department_master")] // Foreign Key here
        public int? d_id { get; set; }
        public tbl_department_master tbl_department_master { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }

    public class tbl_leave_appcbl_rel_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int log_sno { get; set; }

        [ForeignKey("tbl_leave_appcbl_on_religion")] // Foreign Key here
        public int? sno { get; set; }
        public tbl_leave_appcbl_on_religion tbl_leave_appcbl_on_religion { get; set; }

        [ForeignKey("tbl_leave_applicablity")] // Foreign Key here
        public int? lid { get; set; }
        public tbl_leave_applicablity tbl_leave_applicablity { get; set; }

        [ForeignKey("tbl_religion_master")] // Foreign Key here
        public int? r_id { get; set; }
        public tbl_religion_master tbl_religion_master { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }

    public class tbl_leave_credit_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_credit_log_id { get; set; }  // primary key  must be public! 

        [ForeignKey("tbl_leave_credit")] // Foreign Key here
        public int? leave_credit_id { get; set; }
        public tbl_leave_credit tbl_leave_credit { get; set; }

        [ForeignKey("tbl_leave_info")] // Foreign Key here
        public int? leave_info_id { get; set; }
        public tbl_leave_info tbl_leave_info { get; set; }

        public byte frequency_type { get; set; }//1-monthly,2 Quaterly,3 Half yealry,4 yearly
        public byte is_half_day_applicable { get; set; }
        // public byte is_credit_attd_depend { get; set; }
        public byte is_applicable_for_advance { get; set; }
        public int advance_applicable_day { get; set; }
        public byte is_leave_accrue { get; set; }
        public int max_accrue { get; set; }//if no Limit then 10000
        public byte is_required_certificate { get; set; }
        public string certificate_path { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }

    public class tbl_leave_rule_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_credit_log_id { get; set; }  // primary key  must be public! 

        [ForeignKey("tbl_leave_rule")] // Foreign Key here
        public int? leave_credit_id { get; set; }
        public tbl_leave_rule tbl_leave_rule { get; set; }

        [ForeignKey("tbl_leave_info")] // Foreign Key here
        public int? leave_info_id { get; set; }
        public tbl_leave_info tbl_leave_info { get; set; }

        public DateTime applicable_if_employee_joined_before_dt { get; set; } //default 1-jan-2300

        public byte maximum_leave_clubbed_in_tenure_number_of_leave { get; set; }
        public int maxi_negative_leave_applicable { get; set; }
        public byte certificate_require_for_min_no_of_day { get; set; }

        public int minimum_leave_applicable { get; set; } // 1 for	0.5 (half day) , 2  for 1 (full day)
        public byte number_maximum_negative_leave_balance { get; set; }

        public byte maximum_leave_can_be_taken_type { get; set; } // if 1 for days, 2 for quarter
        public byte maximum_leave_can_be_taken { get; set; }
        public byte maximum_leave_can_be_in_day { get; set; }
        public byte maximum_leave_can_be_taken_in_quater { get; set; }

        public byte can_carried_forward { get; set; } // 1 for Yes, 2 for No
        public int maximum_carried_forward { get; set; } // 1 for Yes, 2 for No

        public byte applied_sandwich_rule { get; set; }//0 no Rule Applied, 
                                                       //1.	H / WO counted as absent, if employees absent before WO / H*
                                                       //2	H / WO counted as absent, if employees absent after WO / H * (drop down)
                                                       //3  H / WO counted as absent, if employees absent before / after WO / H* (drop down)


        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }

    public class tbl_leave_cashable_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_cashable_log_id { get; set; }  // primary key  must be public! 

        [ForeignKey("tbl_leave_cashable")] // Foreign Key here
        public int? leave_cashable_id { get; set; }
        public tbl_leave_cashable tbl_leave_cashable { get; set; }

        [ForeignKey("tbl_leave_info")] // Foreign Key here
        public int? leave_info_id { get; set; }
        public tbl_leave_info tbl_leave_info { get; set; }


        public byte is_cashable { get; set; }
        public byte cashable_type { get; set; }//0-for all, 1 dor on separation,2 for after n year
        public byte cashable_after_year { get; set; }//default 1
        public int maximum_cashable_leave { get; set; }//default is 10000

        public int requested_by { get; set; }

        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }



    #endregion


}

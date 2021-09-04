using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB
{
    /// <summary>
    /// Developed By : Amarjeet
    /// Date : 29-11-2018
    /// Decs :  shift master
    /// </summary>
    /// 
    public class tbl_shift_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int shift_id { get; set; }  // primary key  must be public!  
        [MaxLength(100)]
        public string shift_code { get; set; }
        public int is_active { get; set; }
        public byte is_default{ get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public virtual ICollection<tbl_shift_details> tbl_shift_details { get; set; }

    }

    public class tbl_shift_location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int shift_location_id { get; set; }  // primary key  must be public!  

        [ForeignKey("tbl_location_master")] // Foreign Key here
        public int? location_id { get; set; }
        public tbl_location_master tbl_location_master { get; set; }

        [ForeignKey("tbl_shift_details")] // Foreign Key here
        public int? shift_detail_id { get; set; }
        public tbl_shift_details tbl_shift_details { get; set; }

        [ForeignKey("tbl_company_master")] // Foreign Key here
        public int? company_id { get; set; }
        public tbl_company_master tbl_company_master { get; set; }

        public byte is_deleted { get; set; }
        
    }

    public class tbl_shift_department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int shift_department_id { get; set; }  // primary key  must be public!  

        [ForeignKey("tbl_shift_details")] // Foreign Key here
        public int? shift_detail_id { get; set; }
        public tbl_shift_details tbl_shift_details { get; set; }
        [ForeignKey("tbl_department_master")] // Foreign Key here
        public int? dept_id { get; set; }
        public tbl_department_master tbl_department_master { get; set; }
        public byte is_deleted { get; set; }


    }

    public class tbl_shift_details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int shift_details_id { get; set; }  // primary key  must be public!  

        public byte shift_for_all_location { get; set; }
        public byte shift_for_all_company { get; set; }
        public byte shift_for_all_department { get; set; }

        [ForeignKey("tbl_shift_master")] // Foreign Key here
        public int? shift_id { get; set; }
        public tbl_shift_master tbl_shift_master { get; set; }
                
        public byte shift_type { get; set; }// 1 for Fixed , 2 Flexible

        [MaxLength(200)]
        [StringLength(50)]
        [Display(Description = "shift_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,50}$", ErrorMessage = "Invalid Shift Name")]
        public string shift_name { get; set; }
        [MaxLength(100)]
        [StringLength(20)]
        [Display(Description = "shift_short_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,20}$", ErrorMessage = "Invalid Shift Short Name")]
        public string shift_short_name { get; set; }

        public DateTime punch_in_time { get; set; }//default 1-jan-2000
        public DateTime punch_in_max_time { get; set; }//default 1-jan-2000
        public DateTime punch_out_time { get; set; }//default 1-jan-2000


        public byte maximum_working_hours { get; set; }
        public byte maximum_working_minute { get; set; }
        //public DateTime grace_time_for_late_punch_in { get; set; }//default 1-jan-2000
        //public DateTime grace_time_for_late_punch_out { get; set; }//default 1-jan-2000
        public DateTime grace_time_for_late_punch { get; set; }
        
        public int number_of_grace_time_applicable_in_month { get; set; }

        public byte is_lunch_punch_applicable { get; set; } // 0 for punch not applicable  1 for Fixed / 2 for Flexible 
        public DateTime lunch_punch_out_time { get; set; } // if lunch punch applicable Fixed
        public DateTime lunch_punch_in_time { get; set; } // if lunch punch applicable Fixed
        public DateTime maximum_lunch_time { get; set; } // if lunch punch applicable Flexible
        public byte is_ot_applicable { get; set; } // 0 for not applicable / 1 for applicable 
        public DateTime maximum_ot_hours { get; set; }

        public int tea_punch_applicable_one { get; set; } // 0 for Fixed / 1 for Flexible 
        public DateTime tea_punch_out_time_one { get; set; } // if one tea  punch applicable Fixed
        public DateTime tea_punch_in_time_one { get; set; } // if one tea punch applicable Fixed
        public DateTime maximum_tea_time_one { get; set; } // if one tea punch applicable Flexible

        public int tea_punch_applicable_two { get; set; } // 0 for Fixed / 1 for Flexible 
        public DateTime tea_punch_out_time_two { get; set; } // if two tea  punch applicable Fixed
        public DateTime tea_punch_in_time_two { get; set; } // if two tea punch applicable Fixed
        public DateTime maximum_tea_time_two { get; set; } // if two tea punch applicable Flexible


        //public DateTime mark_as_half_day_for_working_hours_less_than { get; set; }
        public DateTime min_halfday_working_hour { get; set; }
        public byte is_night_shift { get; set; } // 0 for not night shift / 1 for is night shift
        //public byte is_punch_ignore { get; set; } // 0 for Early punch in ignore / 1 for Late punch out ignore
        //public byte punch_type { get; set; } // 0 for In single punch, Absent /1 for In single punch, Present /  2 for Punch exempted
        public int weekly_off { get; set; } // If 0 then no any week off , 1 means week off and week off day in other table tbl_shift_week_off

        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_shift_week_off
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int shift_week_off_id { get; set; }  // primary key  must be public!  

        public byte week_day { get; set; }//0 sunday,1 monday etc        
        public byte days { get; set; }//1,2,3,4,5


        //As per amarjit if Shift Detail is Created then it will necearry to Delete the previous data and Insert the new data 
        //[ForeignKey("tbl_shift_master")] // Foreign Key here
        //public int? shift_master_id { get; set; }
        //public tbl_shift_master tbl_shift_master { get; set; }

        [ForeignKey("tbl_shift_details")] // Foreign Key here
        public int? shift_detail_id { get; set; }
        public tbl_shift_details tbl_shift_details { get; set; }

        [ForeignKey("tbl_employee_master")] // Foreign Key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }

        [ForeignKey("tbl_emp_weekoff")] // Foreign Key here
        public int? emp_weekoff_id { get; set; }
        public tbl_emp_weekoff tbl_emp_weekoff { get; set; }
        

        public int company_id { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_shift_roster_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int shift_roster_id { get; set; }  // primary key  must be public!  

        public DateTime applicable_from_date { get; set; }
        public DateTime applicable_to_date { get; set; }

        public int shift_rotat_in_day { get; set; }

        [ForeignKey("tbl_employee_master")] // Foreign Key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }

        //[ForeignKey("tbl_shift_master1")]
        //public int? shift_id1 { get; set; }
        //public tbl_shift_details tbl_shift_master1 { get; set; }
        //[ForeignKey("tbl_shift_master2")]
        //public int? shift_id2 { get; set; }
        //public tbl_shift_details tbl_shift_master2 { get; set; }
        //[ForeignKey("tbl_shift_master3")]
        //public int? shift_id3 { get; set; }
        //public tbl_shift_details tbl_shift_master3 { get; set; }
        //[ForeignKey("tbl_shift_master4")]
        //public int? shift_id4 { get; set; }
        //public tbl_shift_details tbl_shift_master4 { get; set; }
        //[ForeignKey("tbl_shift_master5")]
        //public int? shift_id5 { get; set; }
        //public tbl_shift_details tbl_shift_master5 { get; set; }


        [ForeignKey("tsm1")]
        public int? shft_id1 { get; set; }
        public tbl_shift_master tsm1 { get; set; }
        [ForeignKey("tsm2")]
        public int? shft_id2 { get; set; }
        public tbl_shift_master tsm2 { get; set; }
        [ForeignKey("tsm3")]
        public int? shft_id3 { get; set; }
        public tbl_shift_master tsm3 { get; set; }
        [ForeignKey("tsm4")]
        public int? shft_id4 { get; set; }
        public tbl_shift_master tsm4 { get; set; }
        [ForeignKey("tsm5")]
        public int? shft_id5 { get; set; }
        public tbl_shift_master tsm5 { get; set; }

    }


    #region Log Tables

    public class tbl_shift_master_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int shift_log_id { get; set; }  // primary key  must be public!  


        [ForeignKey("tbl_shift_master")] // Foreign Key here
        public int? shift_id { get; set; }
        public tbl_shift_master tbl_shift_master { get; set; }

        [ForeignKey("tbl_location_master")] // Foreign Key here
        public int? location_id { get; set; }
        public tbl_location_master tbl_location_master { get; set; }


        [MaxLength(200)]
        [StringLength(50)]
        [Display(Description = "shift_name")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,50}$", ErrorMessage = "Invalid Shift Name")]
        public string shift_name { get; set; }
        [MaxLength(100)]
        [StringLength(20)]
        [Display(Description = "shift_short_name")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,20}$", ErrorMessage = "Invalid Shift Short Name")]
        public string shift_short_name { get; set; }
        [MaxLength(100)]
        public string shift_code { get; set; }
        public DateTime punch_in_time { get; set; }//default 1-jan-2000
        public DateTime punch_out_time { get; set; }//default 1-jan-2000
        public byte maximum_working_hours { get; set; }
        public byte maximum_working_minute { get; set; }
        public DateTime grace_time_for_late_punch_in { get; set; }//default 1-jan-2000
        public DateTime grace_time_for_late_punch_out { get; set; }//default 1-jan-2000
        public int number_of_grace_time_applicable_in_month { get; set; }

        public byte is_lunch_punch_applicable { get; set; } // 0 for punch not applicable  1 for Fixed / 2 for Flexible 
        public DateTime lunch_punch_out_time { get; set; } // if lunch punch applicable Fixed
        public DateTime lunch_punch_in_time { get; set; } // if lunch punch applicable Fixed
        public DateTime maximum_lunch_time { get; set; } // if lunch punch applicable Flexible
        public byte is_ot_applicable { get; set; } // 0 for not applicable / 1 for applicable 
        public DateTime maximum_ot_hours { get; set; }

        public int tea_punch_applicable_one { get; set; } // 0 for Fixed / 1 for Flexible 
        public DateTime tea_punch_out_time_one { get; set; } // if one tea  punch applicable Fixed
        public DateTime tea_punch_in_time_one { get; set; } // if one tea punch applicable Fixed
        public DateTime maximum_tea_time_one { get; set; } // if one tea punch applicable Flexible

        public int tea_punch_applicable_two { get; set; } // 0 for Fixed / 1 for Flexible 
        public DateTime tea_punch_out_time_two { get; set; } // if two tea  punch applicable Fixed
        public DateTime tea_punch_in_time_two { get; set; } // if two tea punch applicable Fixed
        public DateTime maximum_tea_time_two { get; set; } // if two tea punch applicable Flexible


        public DateTime mark_as_half_day_for_working_hours_less_than { get; set; }
        public byte is_night_shift { get; set; } // 0 for not night shift / 1 for is night shift
        //public byte is_punch_ignore { get; set; } // 0 for Early punch in ignore / 1 for Late punch out ignore
        //public byte punch_type { get; set; } // 0 for In single punch, Absent /1 for In single punch, Present /  2 for Punch exempted
        public int weekly_off { get; set; } // If 0 then no any week off , 1 means week off and week off day in other table tbl_shift_week_off


        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }


    public class tbl_shift_week_off_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int shift_week_off_log_id { get; set; }  // primary key  must be public!  

        [ForeignKey("tbl_shift_week_off")] // Foreign Key here
        public int? shift_week_off_id { get; set; }
        public tbl_shift_week_off tbl_shift_week_off { get; set; }

        public byte week_day { get; set; }//0 monday,1 tuesday etc        
        public byte days { get; set; }//1,2,3,4,5

        [ForeignKey("tbl_shift_master")] // Foreign Key here
        public int? shift_id { get; set; }
        public tbl_shift_master tbl_shift_master { get; set; }

        [ForeignKey("tbl_employee_master")] // Foreign Key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }


    public class tbl_shift_location_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int shift_location_log_id { get; set; }  // primary key  must be public!

        [ForeignKey("tbl_shift_location")] // Foreign Key here
        public int? shift_location_id { get; set; }
        public tbl_shift_location tbl_shift_location { get; set; }


        [ForeignKey("tbl_location_master")] // Foreign Key here
        public int? location_id { get; set; }
        public tbl_location_master tbl_location_master { get; set; }
        [ForeignKey("tbl_shift_details")] // Foreign Key here
        public int? shift_detail_id { get; set; }
        public tbl_shift_details tbl_shift_details { get; set; }
        public byte is_deleted { get; set; }
    }

    public class tbl_shift_details_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int shift_details_id { get; set; }  // primary key  must be public!  

        [ForeignKey("tbl_shift_details")] // Foreign Key here
        public int? shift_details_log_id { get; set; }
        public tbl_shift_details tbl_shift_details { get; set; }

        public byte shift_for_all_location { get; set; }
        public byte shift_for_all_company { get; set; }

        [ForeignKey("tbl_shift_master")] // Foreign Key here
        public int? shift_id { get; set; }
        public tbl_shift_master tbl_shift_master { get; set; }


        [MaxLength(200)]
        public string shift_name { get; set; }
        [MaxLength(100)]
        public string shift_short_name { get; set; }

        public DateTime punch_in_time { get; set; }//default 1-jan-2000
        public DateTime punch_out_time { get; set; }//default 1-jan-2000
        public byte maximum_working_hours { get; set; }
        public byte maximum_working_minute { get; set; }
        public DateTime grace_time_for_late_punch_in { get; set; }//default 1-jan-2000
        public DateTime grace_time_for_late_punch_out { get; set; }//default 1-jan-2000
        public int number_of_grace_time_applicable_in_month { get; set; }

        public byte is_lunch_punch_applicable { get; set; } // 0 for punch not applicable  1 for Fixed / 2 for Flexible 
        public DateTime lunch_punch_out_time { get; set; } // if lunch punch applicable Fixed
        public DateTime lunch_punch_in_time { get; set; } // if lunch punch applicable Fixed
        public DateTime maximum_lunch_time { get; set; } // if lunch punch applicable Flexible
        public byte is_ot_applicable { get; set; } // 0 for not applicable / 1 for applicable 
        public DateTime maximum_ot_hours { get; set; }

        public int tea_punch_applicable_one { get; set; } // 0 for Fixed / 1 for Flexible 
        public DateTime tea_punch_out_time_one { get; set; } // if one tea  punch applicable Fixed
        public DateTime tea_punch_in_time_one { get; set; } // if one tea punch applicable Fixed
        public DateTime maximum_tea_time_one { get; set; } // if one tea punch applicable Flexible

        public int tea_punch_applicable_two { get; set; } // 0 for Fixed / 1 for Flexible 
        public DateTime tea_punch_out_time_two { get; set; } // if two tea  punch applicable Fixed
        public DateTime tea_punch_in_time_two { get; set; } // if two tea punch applicable Fixed
        public DateTime maximum_tea_time_two { get; set; } // if two tea punch applicable Flexible


        public DateTime mark_as_half_day_for_working_hours_less_than { get; set; }
        public byte is_night_shift { get; set; } // 0 for not night shift / 1 for is night shift
        public byte is_punch_ignore { get; set; } // 0 for Early punch in ignore / 1 for Late punch out ignore
        public byte punch_type { get; set; } // 0 for In single punch, Absent /1 for In single punch, Present /  2 for Punch exempted
        public int weekly_off { get; set; } // If 0 then no any week off , 1 means week off and week off day in other table tbl_shift_week_off

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted

    }

    public class tbl_shift_roster_master_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int shift_roster_log_id { get; set; }  // primary key  must be public!  

        [ForeignKey("tbl_shift_roster_master")] // Foreign Key here
        public int? r_id { get; set; }
        public tbl_shift_roster_master tbl_shift_roster_master { get; set; }


        public DateTime applicable_from_date { get; set; }
        public DateTime applicable_to_date { get; set; }

        public int shift_rotat_in_day { get; set; }

        [ForeignKey("tbl_employee_master")] // Foreign Key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }

        [ForeignKey("tbl_shift_master1")]
        public int? shift_id1 { get; set; }
        public tbl_shift_details tbl_shift_master1 { get; set; }
        [ForeignKey("tbl_shift_master2")]
        public int? shift_id2 { get; set; }
        public tbl_shift_details tbl_shift_master2 { get; set; }
        [ForeignKey("tbl_shift_master3")]
        public int? shift_id3 { get; set; }
        public tbl_shift_details tbl_shift_master3 { get; set; }
        [ForeignKey("tbl_shift_master4")]
        public int? shift_id4 { get; set; }
        public tbl_shift_details tbl_shift_master4 { get; set; }
        [ForeignKey("tbl_shift_master5")]
        public int? shift_id5 { get; set; }
        public tbl_shift_details tbl_shift_master5 { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }


    #endregion
}

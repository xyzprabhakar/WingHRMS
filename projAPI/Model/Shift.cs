using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model
{
    public class ShiftMaster
    {
        public int shift_id { get; set; }  // primary key  must be public!  
        public string shift_code { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class ShiftLocation
    {
        public int shift_location_id { get; set; }  // primary key  must be public!                 
        public int location_id { get; set; }
        public int shift_detail_id { get; set; }
        public int company_id { get; set; }
        public byte is_deleted { get; set; }
    }

    public class ShiftDetails
    {
        public int shift_details_id { get; set; }  // primary key  must be public!  


        public byte shift_for_all_location { get; set; }// 1 For All Location , 2 For Selected Location
        public List<ShiftLocation> ShiftLocation { get; set; }

        public byte shift_for_all_company { get; set; }// 1 For All Company , 2 For Selected Company
        public byte shift_for_all_department { get; set; }
        public List<ShiftDept> ShiftDept { get; set; }

        public int company_id { get; set; }
        public int location_id { get; set; }
        public int department_id { get; set; }

        public int shift_id { get; set; }
        public string shift_code { get; set; }
        public byte shift_type { get; set; }// 1 for Fixed , 2 Flexible
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "shift_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Shift Name")]
        public string shift_name { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "shift_short_name")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,100}$", ErrorMessage = "Invalid Shift Short Name")]
        public string shift_short_name { get; set; }

        public DateTime punch_in_time { get; set; }//default 1-jan-2000
        public DateTime punch_in_max_time { get; set; }//default 1-jan-2000
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
                                                   //  public DateTime maximum_ot_hours { get; set; }

        public int tea_punch_applicable_one { get; set; } // 0 for Fixed / 1 for Flexible 
        public DateTime tea_punch_out_time_one { get; set; } // if one tea  punch applicable Fixed
        public DateTime tea_punch_in_time_one { get; set; } // if one tea punch applicable Fixed
        public DateTime maximum_tea_time_one { get; set; } // if one tea punch applicable Flexible

        public int tea_punch_applicable_two { get; set; } // 0 for Fixed / 1 for Flexible 
        public DateTime tea_punch_out_time_two { get; set; } // if two tea  punch applicable Fixed
        public DateTime tea_punch_in_time_two { get; set; } // if two tea punch applicable Fixed
        public DateTime maximum_tea_time_two { get; set; } // if two tea punch applicable Flexible


        public DateTime mark_as_half_day_for_working_hours_less_than { get; set; }
        public DateTime min_halfday_working_hour { get; set; }
        public byte is_night_shift { get; set; } // 0 for not night shift / 1 for is night shift
        public byte is_punch_ignore { get; set; } // 0 for Early punch in ignore / 1 for Late punch out ignore
        public byte punch_type { get; set; } // 0 for In single punch, Absent /1 for In single punch, Present /  2 for Punch exempted
        public int weekly_off { get; set; } // If 0 then no any week off , 1 means week off and week off day in other table tbl_shift_week_off
        public List<ShiftWeekOff> ShiftWeekOff { get; set; }


        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
        public int is_default { get; set; }
    }


    public class ShiftDept
    {
        public int dept_id { get; set; }
        public byte is_deleted { get; set; }
    }

    public class ShiftWeekOff
    {
        public int shift_week_off_id { get; set; }  // primary key  must be public!  

        public byte week_day { get; set; }//0 monday,1 tuesday etc        
        public byte days { get; set; }//1,2,3,4,5

        public int shift_detail_id { get; set; }

        public int emp_id { get; set; }

        public int company_id { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }


    public class OtRule
    {
        public int ot_rule_id { get; set; }
        public int grace_working_hour { get; set; }
        public int grace_working_minute { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class CommbOffRule
    {
        public int comp_off_rule_id { get; set; }
        public int minimum_working_hours { get; set; }
        public int minimum_working_minute { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

   


    //added by vibhav on 27 dec 2018
    public class RoasterMasterModel
    {
        public int shift_roster_id { get; set; }
        public DateTime? applicable_from_date { get; set; }
        public DateTime? applicable_to_date { get; set; }
        public int shift_rotat_in_day { get; set; }
        public List<object> emp_id { get; set; }
        public int shft_id1 { get; set; }
        public int shft_id2 { get; set; }
        public int shft_id3 { get; set; }
        public int shft_id4 { get; set; }
        public int shft_id5 { get; set; }
    }


    // add by supriya on 22-04-2020
    public class ShiftAssign
    {
        public int shift_id { get; set; }

        public int? company_id { get; set; }

        public int? dept_id { get; set; }

        public int? loc_id { get; set; }

        public DateTime effective_dt { get; set; }

        public List<object> emp_ { get; set; }

        public int created_by { get; set; }
    }

    public class shiftreport
    {
        public int sd_id { get; set; }

        public DateTime effective_Dt { get; set; }

        public string emp_name { get; set; }

        public DateTime shift_in_time { get; set; }

        public DateTime shift_out_time { get; set; }

        public string shift_name { get; set; }

        public string department_name { get; set; }

        public int emp_id { get; set; }

        public string location_name { get; set; }

        public string emp_code { get; set; }
    }
}

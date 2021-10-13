using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB
{
    public class tbl_attendance_details_manual
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int attendance_id { get; set; }
        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }
        public DateTime attendance_dt { get; set; }
        public DateTime? start_in { get; set; }
        public DateTime? start_out { get; set; }
        public byte? day_status { get; set; }
        public int user_id { get; set; }
        public DateTime entry_date { get; set; }
    }

    

    public class tbl_attendance_details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int attendance_id { get; set; }
        [ForeignKey("tbl_machine_master")] // Foreign Key here
        public int? machine_id { get; set; }
        public tbl_machine_master tbl_machine_master { get; set; }
        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }
        public DateTime punch_time { get; set; }
        public DateTime entry_time { get; set; }
        public byte enter_by_interface { get; set; } //1 by Schdule,2 by GUI 

    }

    public class tbl_daily_attendance //all data inserted in single , after that it will updated automatclly
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("tbl_employee_master")] // Foreign Key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }
        [Key]
        [Column(Order = 2)]
        public DateTime attendance_dt { get; set; }
        public int payrollmonthyear { get; set; }
        public DateTime last_process_dt { get; set; }
        [ForeignKey("tbl_emp_officaial_sec")] // Foreign Key here
        public int? emp_offcl_id { get; set; }
        public tbl_emp_officaial_sec tbl_emp_officaial_sec { get; set; }
        [ForeignKey("tbl_shift_master")] // Foreign Key here
        public int? shift_id { get; set; }
        public tbl_shift_master tbl_shift_master { get; set; }
        [ForeignKey("tbl_shift_details")] // Foreign Key here
        public int? s_d_id { get; set; }
        public tbl_shift_details tbl_shift_details { get; set; }
        public DateTime in_time { get; set; }//not time then default will be 1-jan-2000
        public DateTime shift_in_time { get; set; }
        public DateTime shift_max_in_time { get; set; }
        public DateTime out_time { get; set; }//not time then default will be 1-jan-2000
        public DateTime shift_out_time { get; set; }
        public byte day_status { get; set; }//1-Full day Present,2-Full day Absent,3 Full Day Leave, 4 half day present-half day Absent,5 half day present- half day leave,6 Half day leave-Halfday absent
        public byte leave_type { get; set; }//1-full day, 2 half day,3- short leave
        public byte is_weekly_off { get; set; }
        public byte is_holiday { get; set; }
        public byte is_comp_off { get; set; }
        public byte is_outdoor { get; set; }
        public byte is_regularize { get; set; }
        public byte is_grace_applied { get; set; }
        public int grace_period_hour { get; set; }
        public int grace_period_minute { get; set; }
        public int short_leave_hour { get; set; }
        public int short_leave_minute { get; set; }
        public byte is_late_in { get; set; }
        public int early_or_late_in_hour { get; set; }
        public int early_or_late_in_minute { get; set; }
        public byte is_early_out { get; set; }
        public int early_or_late_out_hour { get; set; }
        public int early_or_late_out_minute { get; set; }
        public int working_hour_done { get; set; }
        public int working_minute_done { get; set; }
        public int w_hour_req_for_full_day { get; set; }
        public int w_min_req_for_full_day { get; set; }
        public int w_hour_req_for_half_day { get; set; }
        public int w_min_req_for_half_day { get; set; }
        public byte is_ot_given { get; set; } //1 for if ot is give
        public int ot_hour_done { get; set; }
        public int ot_minute_done { get; set; }
        public byte is_freezed { get; set; }//default 0
        [ForeignKey("thm")] // Foreign Key here
        public int? holiday_id { get; set; }
        public tbl_holiday_master thm { get; set; }
        [ForeignKey("tlr")] // Foreign Key here
        public int? leave_request_id { get; set; }
        public tbl_leave_request tlr { get; set; }
    }

    public class tbl_attendance_summary
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("tbl_employee_master")] // Foreign Key here
        public int? emp_id { get; set; }
        public tbl_employee_master tbl_employee_master { get; set; }
        [Key]
        [Column(Order = 2)]
        public int Monthyear { get; set; }

        public double TotalDay { get; set; } = 30;
        public double Present{ get; set; }
        public double Leave{ get; set; }
        public double Holiday { get; set; }
        public double Absent { get; set; }
        public double WeekOff { get; set; }
        public double PaidDay { get; set; }

        
        public double FinalPresent { get; set; }= 30;
        public double FinalLeave { get; set; }
        public double FinalHoliday { get; set; }
        public double FinalAbsent { get; set; }
        public double FinalWeekOff { get; set; }
        public double FinalPaidDay { get; set; }

        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int modified_by { get; set; }
        public DateTime modified_date { get; set; }

    }


    public class tbl_comp_off_ledger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }
        public DateTime compoff_date { get; set; }
        public double credit { get; set; }
        public double dredit { get; set; }
        public DateTime transaction_date { get; set; }
        public byte transaction_type { get; set; }//1-on Working off(addition in compoff),2- Consumed ,3- Expired, 4-compoff in cash,5 manual add, 6 maula deduction,7 delete by system
        public int monthyear { get; set; }
        public int transaction_no { get; set; }// comp request no
        public string remarks { get; set; }
        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? e_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }
        public int is_deleted { get; set; }

        [ForeignKey("deleted_by_detail")]
        public int deleted_by { get; set; }
        public tbl_employee_master deleted_by_detail { get; set; }

        //public int? is_emp_raised { get; set; }

    }

    public class tbl_leave_ledger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }
        [ForeignKey("tbl_leave_type")] // Foreign Key here
        public int? leave_type_id { get; set; }
        public tbl_leave_type tbl_leave_type { get; set; }
        [ForeignKey("tbl_leave_info")] // Foreign Key here
        public int? leave_info_id { get; set; }
        public tbl_leave_info tbl_leave_info { get; set; }
        public DateTime transaction_date { get; set; }
        public DateTime entry_date { get; set; }
        public byte transaction_type { get; set; }//1 Leave add by System, 2 leave Consume, 3 Leave Expired,4 Leave in cash, 5-maul add, 6 maul delete, 100 previous leave credit by system,7 deleted by system
        public int monthyear { get; set; }
        public int transaction_no { get; set; }// comp request no
        public byte leave_addition_type { get; set; }//1-monthly leave add,2 quaterly, 3 half yearly ,4 annually
        public double credit { get; set; }
        public double dredit { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remarks { get; set; }
        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? e_id { get; set; }
        public int? created_by { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }

    }

    public class tbl_comp_off_request_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int comp_off_request_id { get; set; }
        public DateTime compoff_date { get; set; }
        public DateTime compoff_against_date { get; set; }
        public double compoff_request_qty { get; set; }
        [ForeignKey("tbl_employee_requester")] // Foreign Key here
        public int? r_e_id { get; set; }
        public tbl_employee_master tbl_employee_requester { get; set; }
        [ForeignKey("tbl_employee_approval1")] // Foreign Key here
        public int? a1_e_id { get; set; }
        public tbl_employee_master tbl_employee_approval1 { get; set; }
        [ForeignKey("tbl_employee_approval2")] // Foreign Key here
        public int? a2_e_id { get; set; }
        public tbl_employee_master tbl_employee_approval2 { get; set; }
        [ForeignKey("tbl_employee_approval3")] // Foreign Key here
        public int? a3_e_id { get; set; }
        public tbl_employee_master tbl_employee_approval3 { get; set; }
        public DateTime requester_date { get; set; }
        public DateTime approval_date1 { get; set; }
        public DateTime approval_date2 { get; set; }
        public DateTime approval_date3 { get; set; }
        [Range(0, 2)]
        public byte is_approved1 { get; set; }
        [Range(0, 2)]
        public byte is_approved2 { get; set; }
        [Range(0, 2)]
        public byte is_approved3 { get; set; }
        [Range(0, 2)]
        public byte is_final_approve { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "requester_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string requester_remarks { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval1_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string approval1_remarks { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval2_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string approval2_remarks { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval3_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string approval3_remarks { get; set; }

        public byte is_deleted { get; set; }
        public int deleted_by { get; set; }
        public DateTime deleted_dt { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "deleted_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string deleted_remarks { get; set; }

        public int created_by { get; set; }

        public DateTime created_dt { get; set; }
        [ForeignKey("comp_master")]
        public int? company_id { get; set; }

        public tbl_company_master comp_master { get; set; }

        [ForeignKey("tem_admin")]
        public int? admin_id { get; set; }

        public tbl_employee_master tem_admin { get; set; }

        public int is_admin_approve { get; set; }
        public string admin_remarks { get; set; }

        public DateTime admin_ar_date { get; set; }
    }

    public class tbl_leave_request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_request_id { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public double leave_qty { get; set; }
        [ForeignKey("tbl_leave_type")] // Foreign Key here
        public int? leave_type_id { get; set; }
        public tbl_leave_type tbl_leave_type { get; set; }
        [ForeignKey("tbl_leave_info")] // Foreign Key here
        public int? leave_info_id { get; set; }
        public tbl_leave_info tbl_leave_info { get; set; }
        public byte leave_applicable_for { get; set; } // 1 for Full day , 2 for Half day , 3 for hours and minutes (HH:MM)
        public byte day_part { get; set; } // 1 For First Half, 2 Second half 
        public DateTime leave_applicable_in_hours_and_minutes { get; set; } // If leave applicable in hours and minutes (HH:MM)

        [ForeignKey("tbl_employee_requester")] // Foreign Key here
        public int? r_e_id { get; set; }
        public tbl_employee_master tbl_employee_requester { get; set; }
        [ForeignKey("tbl_employee_approval1")] // Foreign Key here
        public int? a1_e_id { get; set; }
        public tbl_employee_master tbl_employee_approval1 { get; set; }
        [ForeignKey("tbl_employee_approval2")] // Foreign Key here
        public int? a2_e_id { get; set; }
        public tbl_employee_master tbl_employee_approval2 { get; set; }
        [ForeignKey("tbl_employee_approval3")] // Foreign Key here
        public int? a3_e_id { get; set; }
        public tbl_employee_master tbl_employee_approval3 { get; set; }
        public DateTime requester_date { get; set; }
        public DateTime approval_date1 { get; set; }
        public DateTime approval_date2 { get; set; }
        public DateTime approval_date3 { get; set; }
        [Range(0, 2)]
        public byte is_approved1 { get; set; }
        [Range(0, 2)]
        public byte is_approved2 { get; set; }
        [Range(0, 2)]
        public byte is_approved3 { get; set; }
        [Range(0, 2)]
        public byte is_final_approve { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "requester_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string requester_remarks { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval1_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string approval1_remarks { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval2_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string approval2_remarks { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval3_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string approval3_remarks { get; set; }

        public byte is_deleted { get; set; }
        public int deleted_by { get; set; }
        public DateTime deleted_dt { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "deleted_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string deleted_remarks { get; set; }

        public int created_by { get; set; }

        public DateTime created_dt { get; set; }

        [ForeignKey("comp_master")]
        public int? company_id { get; set; }

        public tbl_company_master comp_master { get; set; }

        [ForeignKey("tem_admin")]
        public int? admin_id { get; set; }

        public tbl_employee_master tem_admin { get; set; }

        public int is_admin_approve { get; set; }
        public string admin_remarks { get; set; }

        public DateTime admin_ar_date { get; set; }
    }

    public class tbl_attendace_request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_request_id { get; set; }
        public DateTime from_date { get; set; }
        public DateTime system_in_time { get; set; }
        public DateTime system_out_time { get; set; }
        public DateTime manual_in_time { get; set; }
        public DateTime manual_out_time { get; set; }
        [ForeignKey("tbl_employee_requester")] // Foreign Key here
        public int? r_e_id { get; set; }
        public tbl_employee_master tbl_employee_requester { get; set; }
        [ForeignKey("tbl_employee_approval1")] // Foreign Key here
        public int? a1_e_id { get; set; }
        public tbl_employee_master tbl_employee_approval1 { get; set; }
        [ForeignKey("tbl_employee_approval2")] // Foreign Key here
        public int? a2_e_id { get; set; }
        public tbl_employee_master tbl_employee_approval2 { get; set; }
        [ForeignKey("tbl_employee_approval3")] // Foreign Key here
        public int? a3_e_id { get; set; }
        public tbl_employee_master tbl_employee_approval3 { get; set; }
        public DateTime requester_date { get; set; }
        public DateTime approval_date1 { get; set; }
        public DateTime approval_date2 { get; set; }
        public DateTime approval_date3 { get; set; }
        [Range(0, 2)]
        public byte is_approved1 { get; set; }
        [Range(0, 2)]
        public byte is_approved2 { get; set; }
        [Range(0, 2)]
        public byte is_approved3 { get; set; }
        [Range(0, 2)]
        public byte is_final_approve { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "requester_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string requester_remarks { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval1_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string approval1_remarks { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval2_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string approval2_remarks { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval3_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string approval3_remarks { get; set; }

        public byte is_deleted { get; set; }
        public int deleted_by { get; set; }
        public DateTime deleted_dt { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "deleted_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string deleted_remarks { get; set; }

        public int created_by { get; set; }

        public DateTime created_dt { get; set; }

        [ForeignKey("comp_master")]
        public int? company_id { get; set; }

        public tbl_company_master comp_master { get; set; }

        [ForeignKey("tem_admin")]
        public int? admin_id { get; set; }

        public tbl_employee_master tem_admin { get; set; }

        public int is_admin_approve { get; set; }
        public string admin_remarks { get; set; }

        public DateTime admin_ar_date { get; set; }
    }

    public class tbl_outdoor_request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int leave_request_id { get; set; }
        public DateTime from_date { get; set; }
        public DateTime system_in_time { get; set; }
        public DateTime system_out_time { get; set; }
        public DateTime manual_in_time { get; set; }
        public DateTime manual_out_time { get; set; }
        [ForeignKey("tbl_employee_requester")] // Foreign Key here
        public int? r_e_id { get; set; }
        public tbl_employee_master tbl_employee_requester { get; set; }
        [ForeignKey("tbl_employee_approval1")] // Foreign Key here
        public int? a1_e_id { get; set; }
        public tbl_employee_master tbl_employee_approval1 { get; set; }
        [ForeignKey("tbl_employee_approval2")] // Foreign Key here
        public int? a2_e_id { get; set; }
        public tbl_employee_master tbl_employee_approval2 { get; set; }
        [ForeignKey("tbl_employee_approval3")] // Foreign Key here
        public int? a3_e_id { get; set; }
        public tbl_employee_master tbl_employee_approval3 { get; set; }
        public DateTime requester_date { get; set; }
        public DateTime approval_date1 { get; set; }
        public DateTime approval_date2 { get; set; }
        public DateTime approval_date3 { get; set; }
        [Range(0, 2)]
        public byte is_approved1 { get; set; }
        [Range(0, 2)]
        public byte is_approved2 { get; set; }
        [Range(0, 2)]
        public byte is_approved3 { get; set; }
        [Range(0, 2)]
        public byte is_final_approve { get; set; }
        [MaxLength(500)]
        [StringLength(500)]
        [Display(Description = "requester_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,500}$", ErrorMessage = "Invalid Remarks")]
        public string requester_remarks { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval1_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,500}$", ErrorMessage = "Invalid Remarks")]
        public string approval1_remarks { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval2_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,500}$", ErrorMessage = "Invalid Remarks")]
        public string approval2_remarks { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval3_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,500}$", ErrorMessage = "Invalid Remarks")]
        public string approval3_remarks { get; set; }

        public byte is_deleted { get; set; } //1 delete before approve,2 delete by admin after approve
        public int deleted_by { get; set; }
        public DateTime deleted_dt { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "deleted_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string deleted_remarks { get; set; }

        public int created_by { get; set; }

        public DateTime creted_dt { get; set; }

        [ForeignKey("comp_mster")]
        public int? company_id { get; set; }

        public tbl_company_master comp_mster { get; set; }

        [ForeignKey("tem_admin")]
        public int? admin_id { get; set; }

        public tbl_employee_master tem_admin { get; set; }

        public int is_admin_approve { get; set; }
        public string admin_remarks { get; set; }
        public DateTime admin_ar_date { get; set; }
        public string location { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public int is_auto { get; set; }
    }


    #region Log Tables 

    public class tbl_attendance_details_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int attendance_log_id { get; set; }

        [ForeignKey("tbl_attendance_details")] // Foreign Key here
        public int? a_id { get; set; }
        public tbl_attendance_details tbl_attendance_details { get; set; }

        [ForeignKey("tbl_machine_master")] // Foreign Key here
        public int? machine_id { get; set; }
        public tbl_machine_master tbl_machine_master { get; set; }
        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? e_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }
        public DateTime punch_time { get; set; }
        public DateTime entry_time { get; set; }
        public byte enter_by_interface { get; set; } //1 by Schdule,2 by GUI 

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        [Range(0, 2)]
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }

    public class tbl_comp_off_ledger_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int log_sno { get; set; }

        [ForeignKey("tbl_comp_off_ledger")] // Foreign Key here
        public int? sno { get; set; }
        public tbl_comp_off_ledger tbl_comp_off_ledger { get; set; }

        public DateTime compoff_date { get; set; }
        public double credit { get; set; }
        public double dredit { get; set; }
        public DateTime transaction_date { get; set; }
        public byte transaction_type { get; set; }//1-on Working off(addition in compoff),2- Consumed ,3- Expired, 4-compoff in cash,5 manual add, 6 maula deduction
        public int monthyear { get; set; }
        public int transaction_no { get; set; }// comp request no
        public string remarks { get; set; }
        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? e_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        [Range(0, 2)]
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted
    }

    public class tbl_leave_ledger_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int log_sno { get; set; }

        [ForeignKey("tbl_leave_ledger")] // Foreign Key here
        public int? sno { get; set; }
        public tbl_leave_ledger tbl_leave_ledger { get; set; }

        [ForeignKey("tbl_leave_type")] // Foreign Key here
        public int? leave_type_id { get; set; }
        public tbl_leave_type tbl_leave_type { get; set; }
        public DateTime transaction_date { get; set; }
        public string transaction_type { get; set; }//1 Leave add by System, 2 leave Consume, 3 Leave Expired,4 Leave in cash, 5-maul add, 6 maul delete
        public int monthyear { get; set; }
        public int transaction_no { get; set; }// comp request no
        public byte leave_addition_type { get; set; }//1-monthly leave add,2 quaterly, 3 half yearly ,4 annually
        public double credit { get; set; }
        public double dredit { get; set; }
        public string remarks { get; set; }
        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? e_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }

        public int requested_by { get; set; }
        public DateTime requested_date { get; set; }

        public int approved_by { get; set; }
        public DateTime approved_date { get; set; }
        [Range(0, 2)]
        public byte is_approved { get; set; } // 0 pending , 1 approved, 2 reject

        public byte requested_type { get; set; } // 1 new , 2 update, 3 deleted

    }

    #endregion


    public class tbl_compoff_raise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int emp_comp_id { get; set; }

        public DateTime comp_off_date { get; set; }

        [ForeignKey("emp_requester")]
        public int? emp_id { get; set; }

        public tbl_employee_master emp_requester { get; set; }


        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "requester_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string requester_remarks { get; set; }


        [Range(0, 2)]
        public byte is_approved1 { get; set; }
        [Range(0, 2)]
        public byte is_approved2 { get; set; }
        [Range(0, 2)]
        public byte is_approved3 { get; set; }
        [Range(0, 2)]
        public byte is_final_approve { get; set; }



        [ForeignKey("approval1_emp")] // Foreign Key here
        public int? a1_e_id { get; set; }
        public tbl_employee_master approval1_emp { get; set; }

        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval1_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string approval1_remarks { get; set; }


        [ForeignKey("approval2_emp")] // Foreign Key here
        public int? a2_e_id { get; set; }
        public tbl_employee_master approval2_emp { get; set; }

        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval2_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string approval2_remarks { get; set; }


        [ForeignKey("approver3_emp")] // Foreign Key here
        public int? a3_e_id { get; set; }

        public tbl_employee_master approver3_emp { get; set; }

        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "approval3_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string approval3_remarks { get; set; }

        public byte is_deleted { get; set; } //1 delete by user,3 delete by admin
        public int deleted_by { get; set; }
        public DateTime deleted_dt { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "deleted_remarks")]
        [RegularExpression(@"^[a-zA-Z0-9'\s\d|\.|\-|\/]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string deleted_remarks { get; set; }

        public int created_by { get; set; }

        public DateTime created_date { get; set; }

        public DateTime approver1_dt { get; set; }

        public DateTime approver2_dt { get; set; }

        public DateTime approver3_dt { get; set; }

        [ForeignKey("tem_admin")]
        public int? admin_id { get; set; }

        public tbl_employee_master tem_admin { get; set; }

        public int is_admin_approve { get; set; }
        public string admin_remarks { get; set; }

        public DateTime admin_ar_date { get; set; }
    }

    public class tbl_scheduler_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int scheduler_id { get; set; }
        public string scheduler_name { get; set; }
        public enmSchdulerType schduler_type { get; set; }
        public DateTime? last_runing_date { get; set; }
        public byte is_deleted { get; set; }
    }
    public class tbl_scheduler_details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int scheduler_detail_id { get; set; }
        [ForeignKey("tbl_scheduler_master")]
        public int? scheduler_id { get; set; }
        public tbl_scheduler_master tbl_scheduler_master { get; set; }
        public int? transaction_no { get; set; }
        public DateTime? last_runing_date { get; set; }
        public byte is_deleted { get; set; }

    }
}

using projContext;
using projContext.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projAPI.Model
{



    public class mdlEmployee
    {
        public int empId {get;set;}
        public string empCode{ get; set; }
        public string empName { get; set; }
        public int company_id { get; set; }
        public int state_id { get; set; }
        public int location_id { get; set; }
        public int dept_id { get; set; }
        public int isActive { get; set; }
        public int empstatus { get; set; }
        public int depthLevel { get; set; }
    }




    


    public class EmployeeBasicData : EmployeeBasicDataProc
    {
        public string Official_email_id { get; set; }
        public string official_contact_no { get; set; }
    }

    public class EmployeeMaster
    {
        public int employee_id { get; set; }
        public string emp_code { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }


    public class EmployeeMobileApplicationAccess
    {
        public List<int> mobile_access_employee_ids { get; set; }
        public List<int> mobile_attendence_access_employee_ids { get; set; }
        public int company_id { get; set; }

    }

    public class EmployeeOfficaialSection
    {
        public int emp_official_section_id { get; set; }
        public byte is_applicable_for_all_comp { get; set; }
        public int default_company_id { get; set; }
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

        //public List<EmployeeCompanyMaping> EmployeeCompanyMaping { get; set; }
        //public List<EmployeeShiftAllocation> EmployeeShiftAllocation { get; set; }
        //public List<EmployeeManager> EmployeeManager { get; set; }
        //public List<EmployeeGradeAllocation> EmployeeGradeAllocation { get; set; }
        //public List<EmployeeDesigAllocation> EmployeeDesigAllocation { get; set; }

        public int employee_id { get; set; }
        public int location_id { get; set; }
        public int department_id { get; set; }
        public int sub_dept_id { get; set; }
        [MaxLength(50)]
        [Display(Description = "card_number")]
        [RegularExpression("^([0-9]+)$", ErrorMessage = "Invalid Card No.")]
        public string card_number { get; set; }
        [StringLength(1)]
        [Display(Description = "salutation")]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Salutation")]
        public string salutation { get; set; }

        [StringLength(30)]
        [Display(Description = "employee_first_name")]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid First Name")]
        public string employee_first_name { get; set; }
        [StringLength(30)]
        [Display(Description = "employee_middle_name")]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Middle Name")]
        public string employee_middle_name { get; set; }
        [StringLength(30)]
        [Display(Description = "employee_last_name")]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Last Name")]
        public string employee_last_name { get; set; }
        public DateTime group_joining_date { get; set; }
        public DateTime date_of_joining { get; set; }
        public DateTime department_date_of_joining { get; set; }

        public DateTime date_of_birth { get; set; }
        public int religion_id { get; set; }

        public byte marital_status { get; set; }
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "hr_spoc")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,100}$", ErrorMessage = "Invalid HR Spoc")]
        public string hr_spoc { get; set; }
        [MaxLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string official_email_id { get; set; }

        public int empmnt_id { get; set; }

        public byte is_ot_allowed { get; set; }
        public int is_comb_off_allowed { get; set; }


        public DateTime mobile_punch_from_date { get; set; }
        public DateTime mobile_punch_to_date { get; set; }
        public string employee_photo_path { get; set; }
        public DateTime last_working_date { get; set; }

        public int is_fixed_weekly_off { get; set; } //1 Fixed , //2 Dynamic
        public string weekly_off { get; set; }
        public DateTime effectiveFromDt { get; set; }
        public List<ShiftWeekOff> ShiftWeekOff { get; set; }


        public byte current_employee_type { get; set; }
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

        public string emp_code { get; set; }

        public string Gender { get; set; }

        public string religionname { get; set; }

        public string maritalname { get; set; }

        public string current_employeementname { get; set; }

        public string locationname { get; set; }

        public string departmentname { get; set; }

        public string subdepartmentname { get; set; }

        public string company_name { get; set; }

        public DateTime effective_empmnt_type_dt { get; set; }

        public int sub_locaiton_id { get; set; }
        public string sub_loc_name { get; set; }
    }

    public class EmployeeCompanyMaping
    {
        public int sno { get; set; }
        public int employee_id { get; set; }
        public int company_id { get; set; }
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class EmployeeShiftAllocation
    {
        public int emp_shift_id { get; set; }
        public int shift_id { get; set; }
        public int employee_id { get; set; }
        public DateTime applicable_from_date { get; set; }
        public DateTime applicable_to_date { get; set; }
    }

    public class EmployeeManager
    {
        public int emp_mgr_id { get; set; }  // primary key  must be public!  
        public byte final_approval1 { get; set; }//1 for first,2 for secon ...
        public byte final_approval2 { get; set; }//1 for first,2 for secon ...
        public byte final_approval3 { get; set; }//1 for first,2 for secon ...
        public int m_one_id { get; set; }
        public int m_two_id { get; set; }
        public int m_three_id { get; set; }
        public int employee_id { get; set; }
        public byte final_approval { get; set; }
        public DateTime applicable_from_date1 { get; set; }
        public DateTime applicable_to_date1 { get; set; }
        public DateTime applicable_from_date2 { get; set; }
        public DateTime applicable_to_date2 { get; set; }
        public DateTime applicable_from_date3 { get; set; }
        public DateTime applicable_to_date3 { get; set; }
        public byte notify_manager_1 { get; set; }
        public byte notify_manager_2 { get; set; }
        public byte notify_manager_3 { get; set; }

        public int company_id { get; set; }

        public string manager_name_code { get; set; }

        public string m_two_name_code { get; set; }
        public string m_three_name_code { get; set; }

        public string emp_code { get; set; }
    }

    public class EmployeeGradeAllocation
    {
        public int emp_grade_id { get; set; }
        public int employee_id { get; set; }
        public int grade_id { get; set; }
        public DateTime applicable_from_date { get; set; }
        public DateTime applicable_to_date { get; set; }

    }

    public class EmployeeDesigAllocation
    {
        public int emp_desi_id { get; set; }  // primary key  must be public!  
        public int employee_id { get; set; }
        public int desig_id { get; set; }
        public DateTime applicable_from_date { get; set; }
        public DateTime applicable_to_date { get; set; }
    }


    public class EmployeeDetailListt
    {
        public List<EmployeeOfficaialSection> missingdetaillist { get; set; }

        public List<EmployeeOfficaialSection> duplicatedetaillist { get; set; }

        public List<EmployeeOfficaialSection> adddblist { get; set; }

        public List<EmployeePersonalSection> missingpersonaldtl { get; set; }

        public List<EmployeePersonalSection> duplicatepersonaldtl { get; set; }

        public List<EmployeePersonalSection> adddbpersonaldtl { get; set; }


        public List<EmployeeTaxDetail> missingemptaxdtl { get; set; }

        public List<EmployeeTaxDetail> duplicateemptaxdtl { get; set; }
        public List<EmployeeTaxDetail> addbemptaxdtl { get; set; }


        public List<EmployeeQualificationSection> missingQualification { get; set; }
        public List<EmployeeQualificationSection> duplicateQualification { get; set; }
        public List<EmployeeQualificationSection> addbempQualificationl { get; set; }
        public List<EmployeeQualificationSection> issueQualificationl { get; set; }



        public List<EmployeeFamilySection> missingFamily { get; set; }
        public List<EmployeeFamilySection> duplicateFamily { get; set; }
        public List<EmployeeFamilySection> addbempFamily { get; set; }



        public List<EmployeeShiftAlloc> missingShiftAlloc { get; set; }
        public List<EmployeeShiftAlloc> duplicateShiftAlloc { get; set; }
        public List<EmployeeShiftAlloc> addbempShiftAlloc { get; set; }


        public List<EmployeeGradeAlloc> missingGradeAlloc { get; set; }
        public List<EmployeeGradeAlloc> duplicateGradeAlloc { get; set; }
        public List<EmployeeGradeAlloc> addbempGradeAlloc { get; set; }


        public List<EmployeeDesignationAlloc> missingDesignationAlloc { get; set; }
        public List<EmployeeDesignationAlloc> duplicateDesignationAlloc { get; set; }
        public List<EmployeeDesignationAlloc> addbempDesignationAlloc { get; set; }


        public List<EmployeeManagerAlloc> missingManagerAlloc { get; set; }
        public List<EmployeeManagerAlloc> duplicateManagerAlloc { get; set; }
        public List<EmployeeManagerAlloc> addbempManagerAlloc { get; set; }

        public List<EployeeAccountDetailsForUpload> missingAccountdtlc { get; set; }
        public List<EployeeAccountDetailsForUpload> duplicateAccountdtl { get; set; }
        public List<EployeeAccountDetailsForUpload> addbempAccountdtl { get; set; }


        public List<EployeeUanEsicDetailsForUpload> missingUanEsictlc { get; set; }
        public List<EployeeUanEsicDetailsForUpload> duplicateUanEsicdtl { get; set; }
        public List<EployeeUanEsicDetailsForUpload> addbempUanEsicdtl { get; set; }



        public List<tbl_attendance_details_manual> missinAttendancetlc { get; set; }
        public List<tbl_attendance_details_manual> duplicateAttendancedtl { get; set; }
        public List<tbl_attendance_details_manual> addbempAttendancedtl { get; set; }
        public List<tbl_attendance_details_manual> issueAttendencedtl { get; set; }
        public List<EployeeManualAttendanceDetails> missinAttendancetlist { get; set; }
        public List<EployeeManualAttendanceDetails> duplicateAttendancelist { get; set; }
        public List<EployeeManualAttendanceDetails> addbempAttendancelist { get; set; }
        public List<EployeeManualAttendanceDetails> issueAttendencelist { get; set; }


        public List<EmployeeAllDataUpload> missinFullUpladtlc { get; set; }
        public List<EmployeeAllDataUpload> duplicateFullUpladdtl { get; set; }
        public List<EmployeeAllDataUpload> addbempFullUpladdtl { get; set; }
        public List<EmployeeAllDataUpload> issueFullUpladdtl { get; set; }
        
        public string MissingDtlMessage { get; set; }
        public Response_Msg objresponse { get; set; }

    }

    public class EmployeePersonalSection
    {
        public int emp_personal_section_id { get; set; }  // primary key  must be public!  
        public List<int> requests_ids { get; set; }  // primary key  must be public!  

        public int employee_id { get; set; }

        public string emp_code { get; set; }
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
        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "aadha_card_name")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,100}$", ErrorMessage = "Invalid Aadhar CARD Name...")]
        public string aadha_card_name { get; set; }

        [MaxLength(12)]
        [StringLength(12)]
        [Display(Description = "aadha_card_number")]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Invalid Aadhar CARD Number...")]
        public string aadha_card_number { get; set; }
        public string aadha_card_image { get; set; }
        public byte blood_group { get; set; }//from enum
        public string blood_group_doc { get; set; }
        public string primary_contact_number { get; set; }
        public string secondary_contact_number { get; set; }
        [MaxLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Primary Email ID")]
        public string primary_email_id { get; set; }
        [MaxLength(100)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Secondary Email ID")]
        public string secondary_email_id { get; set; }

        //	Permanent Address
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "permanent_address_line_one")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/]{1,250}$", ErrorMessage = "Invalid Permanent Address in Address Line one ((.),(,) and special characters are not allowed)")]
        public string permanent_address_line_one { get; set; }
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "permanent_address_line_two")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/]{1,250}$", ErrorMessage = "Invalid Permanent Address in Address Line Two ((.),(,) and special characters are not allowed)")]
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
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/]{1,250}$", ErrorMessage = "Invalid Corresponding Address in Address Line one ((.),(,) and special characters are not allowed)")]
        public string corresponding_address_line_one { get; set; }
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "corresponding_address_line_two")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/]{1,250}$", ErrorMessage = "Invalid Corresponding Address in Address Line Two ((.),(,) and special characters are not allowed)")]
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
        public string emergency_contact_mobile_number { get; set; }
        public byte is_emg_same_as_permanent { get; set; } //0-means fill new, if 1 then permanent address, 2 for corresponding address 
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "emergency_contact_line_one")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/]{1,250}$", ErrorMessage = "Invalid Emergency Address in Address Line one")]
        public string emergency_contact_line_one { get; set; }
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Description = "emergency_contact_line_two")]
        [RegularExpression(@"^[a-zA-Z0-9'\s'\#'\-'\/]{1,250}$", ErrorMessage = "Invalid Emergency Address in Address Line Two")]
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

        public string country { get; set; }

        public string state { get; set; }

        public string city { get; set; }

        public int default_company_id { get; set; }

        public string blood_group_name { get; set; }

        [MaxLength(18)]
        [StringLength(18)]
        [Display(Description = "bank_acc")]
        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Invalid Bank Account No.")]
        public string bank_acc { get; set; }
        [MaxLength(11)]
        [StringLength(11)]
        [Display(Description = "ifsc_code")]
        [RegularExpression(@"^[A-Za-z]{4}0[A-Z0-9a-z]{6}$", ErrorMessage = "Invalid IFSC Code")]
        public string ifsc_code { get; set; }
        public int bank_id { get; set; }

        public string bank_name { get; set; }

        public string uan { get; set; }

        public string esic { get; set; }
        public int pf_applicable { get; set; }

        public string pf_app_name { get; set; } // 1.yes 0 no

        public string pf_number { get; set; }
        public int pf_group { get; set; }

        public string pf_group_name { get; set; }
        public double pf_ceilling { get; set; }

        public string pf_ceiling_name { get; set; }

    }

    public class EmployeeTaxDetail
    {

        public int company_id { get; set; }
        public int emp_id { get; set; }
        public string emp_code { get; set; }

        [Display(Description = "income_tax_amount")]
        [RegularExpression(@"^[0-9]{1,40}$", ErrorMessage = "Invalid Tax Amount")]
        public double income_tax_amount { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }



    }

    public class E_Sepration
    {
        public int company_id { get; set; }
        public List<EmpReqApproval> emp_req { get; set; }

        public int is_approve { get; set; }

        public int created_by { get; set; }
    }

    public class EmpReqApproval
    {
        public int req_id { get; set; }

        public int emp_id { get; set; }

        public string remarks { get; set; }

        public int is_relieve_change { get; set; }

        public DateTime final_relieve_dt { get; set; }
    }

    public class EmployeeQualificationSection
    {
        public string emp_code { get; set; }
        public int emp_qualification_section_id { get; set; }
        public List<int> q_ids { get; set; }
        public int? employee_id { get; set; }
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
        [RegularExpression(@"^[a-zA-Z'\s]{1,100}$", ErrorMessage = "Invalid Stream")]
        public string stream { get; set; }
        public string title { get; set; }
        public string education_type { get; set; } // 1 for Regular, 2 for Part-time , 3 for distance
        public string education_level { get; set; } // 1 Not Educated, 2 Primary Education, 3 Secondary, 4 Sr Secondary, 5 Graduation, 6 Post Graduation, 7 Doctorate 

        public string marks_division_cgpa { get; set; }
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remark")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remark { get; set; }
        public string document_image { get; set; }
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

        public int company_id { get; set; }

        public int education_level_ { get; set; }

        public int education_type_ { get; set; }
        public string error_message { get; set; }

    }

    public class EmployeeFamilySection
    {
        public string emp_code { get; set; }
        public int emp_family_section_id { get; set; }  // primary key  must be public!  
        public List<int> request_ids { get; set; }  // list of primary key 

        public int? employee_id { get; set; }

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
        public string dependent { get; set; } // 0 Yes, 1 No
        [MaxLength(200)]
        [StringLength(200)]
        [Display(Description = "remark")]
        [RegularExpression(@"^[a-zA-Z0-9'\s]{1,200}$", ErrorMessage = "Invalid Remarks")]
        public string remark { get; set; }
        public string document_image { get; set; }

        public string is_nominee { get; set; } // 0 Yes , 1 No
        public double nominee_percentage { get; set; } // If nominee yes then %         
        public byte is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

        public string aadhar_card_no { get; set; }

        public int company_id { get; set; }

        public int relation_ { get; set; }
    }
    public class EmployeeShiftAlloc
    {
        public string emp_code { get; set; }
        public int emp_shift_id { get; set; }
        public string shift_id { get; set; }
        public int employee_id { get; set; }
        public DateTime applicable_from_date { get; set; }
        public DateTime applicable_to_date { get; set; }
    }
    public class EmployeeWeekOfflloc
    {
        public string emp_code { get; set; }
        public int emp_shift_id { get; set; }
        public string shift_id { get; set; }
        public int employee_id { get; set; }
        public DateTime applicable_from_date { get; set; }
        public DateTime applicable_to_date { get; set; }
    }


    public class EmployeeGradeAlloc
    {
        public string emp_code { get; set; }
        public int emp_shift_id { get; set; }
        public string grade_id { get; set; }
        public int employee_id { get; set; }
        public DateTime applicable_from_date { get; set; }
        public DateTime applicable_to_date { get; set; }
    }

    public class EmployeeDesignationAlloc
    {
        public string emp_code { get; set; }
        public int emp_shift_id { get; set; }
        public string designation_id { get; set; }
        public int employee_id { get; set; }
        public DateTime applicable_from_date { get; set; }
        public DateTime applicable_to_date { get; set; }
    }

    public class EmployeeManagerAlloc
    {
        public string emp_code { get; set; }
        public byte final_approval { get; set; }//1 for first,2 for secon ...
        public string m_one_id { get; set; }
        public string m_one_final_approval { get; set; }
        public string m_two_id { get; set; }
        public string m_two_final_approval { get; set; }
        public string m_three_id { get; set; }
        public string m_three_final_approval { get; set; }
        public int employee_id { get; set; }
        public DateTime applicable_from_date { get; set; }
        public DateTime applicable_to_date { get; set; }

        public int company_id { get; set; }

        public int? m_one { get; set; }
        public int? m_two { get; set; }
        public int? m_three { get; set; }
    }


    public class EployeeAccountDetails
    {
        public string emp_code { get; set; }
        public int bank_details_id { get; set; }
        public enmPaymentMode payment_mode { get; set; }
        public int? bank_id { get; set; }
        [MaxLength(150)]
        //[RegularExpression(@"^[A-Za-z0-9,.]*$", ErrorMessage = "Invalid Branch")]
        public string branch_name { get; set; }
        [MaxLength(11)]
        [StringLength(11)]
        [Display(Description = "ifsc_code")]
        public string ifsc_code { get; set; }
        [MaxLength(18)]
        [StringLength(18)]
        [Display(Description = "bank_acc")]
        //[RegularExpression(@"^[0-9]{9,18}\s$", ErrorMessage = "Invalid Bank Account No.")]
        public string bank_acc { get; set; }
        public int? employee_id { get; set; }


        [MaxLength(100)]
        [StringLength(100)]
        [Display(Description = "pan_card_name")]
        [RegularExpression(@"^[a-zA-Z'\s]{1,100}$", ErrorMessage = "Invalid PAN CARD Name...")]
        public string pan_card_name { get; set; }

        [MaxLength(10)]
        [StringLength(10)]
        [Display(Description = "pan_card_number")]
        //[RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}\S$", ErrorMessage = "Invalid PAN CARD Number...")]
        public string pan_card_number { get; set; }
        public string pan_card_image { get; set; }

        [MaxLength(100)]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z'\s]{1,100}$", ErrorMessage = "Invalid ADHAR CARD Name...")]
        public string aadha_card_name { get; set; }
        [MaxLength(12)]
        [StringLength(12)]
        [Display(Description = "aadha_card_number")]
        //[RegularExpression(@"^[0-9]{12}\S$", ErrorMessage = "Invalid Aadhar CARD Number...")]
        public string aadha_card_number { get; set; }
        public string aadha_card_image { get; set; }
        public int Created_by { get; set; }
    }

    public class EmployeeStatutoryDetail
    {
        public string emp_code { get; set; }
        public int? employee_id { get; set; }
        public List<int> pf_Ids { get; set; }
        public List<int> esic_Ids { get; set; }
        public List<int> pan_Ids { get; set; }
        public List<int> adhar_Ids { get; set; }
        public List<int> bank_Ids { get; set; }
        public byte is_deleted { get; set; }
        public int pf_Id { get; set; }
        public int esic_Id { get; set; }
        public int pan_Id { get; set; }
        public int adhar_Id { get; set; }
        public int bank_Id { get; set; }

    }
    public class EployeePfEsicDetails
    {
        public string emp_code { get; set; }
        public int? employee_id { get; set; }
        public byte is_pf_applicable { get; set; }
        [Display(Description = "is_pf_applicable")]
        public string uan_number { get; set; }
        [MaxLength(30)]
        public string pf_number { get; set; }
        public enmPFGroup pf_group { get; set; }
        public byte is_vpf_applicable { get; set; }
        public enmVPFGroup vpf_Group { get; set; }
        public double vpf_amount { get; set; }
        public double pf_celing { get; set; }
        public int? bank_id { get; set; }
        [MaxLength(11)]
        [StringLength(11)]
        [Display(Description = "ifsc_code")]
        public string ifsc_code { get; set; }
        [MaxLength(18)]
        [StringLength(18)]
        [Display(Description = "bank_acc")]
        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Invalid Bank Account No.")]
        public string bank_acc { get; set; }
        public byte is_esic_applicable { get; set; }
        [MaxLength(20)]
        public string esic_number { get; set; }
        public int created_by { get; set; }
        public byte is_eps_applicable { get; set; }
    }

    public class EployeeAccountDetailsForUpload
    {
        public string emp_code { get; set; }
        public string payment_mode { get; set; }
        public string bank_id { get; set; }
        public string branch_name { get; set; }
        public string ifsc_code { get; set; }
        public string bank_acc { get; set; }
        public int? employee_id { get; set; }
        public string pan_card_name { get; set; }
        public string pan_card_number { get; set; }
        public string aadha_card_name { get; set; }
        public string aadha_card_number { get; set; }
        public int created_by { get; set; }
    }

    public class EployeeUanEsicDetailsForUpload
    {
        public string emp_code { get; set; }
        public int? employee_id { get; set; }
        public string uan_number { get; set; }
        public string pf_number { get; set; }
        public string pf_group { get; set; }
        public string vpf_Group { get; set; }
        public double vpf_amount { get; set; }
        public double pf_celing { get; set; }
        public string bank_id { get; set; }
        public string ifsc_code { get; set; }
        public string bank_acc { get; set; }
        public string esic_number { get; set; }
        public int created_by { get; set; }
    }


    public class EployeeManualAttendanceDetails
    {
        public string emp_code { get; set; }
        public int? emp_id { get; set; }
        public DateTime attendance_dt { get; set; }
        public DateTime? start_in { get; set; }
        public DateTime? start_out { get; set; }
        public string day_status { get; set; }
        public int user_id { get; set; }
        public DateTime entry_date { get; set; }
        public string error_message { get; set; } = "";
    }

    public class EmployeeAllDataUpload
    {
        public int? user_id { get; set; }
        public int? emp_id { get; set; }
        public string emp_code { get; set; }
        public string salutation { get; set; }
        public string emp_name { get; set; }
        public string father_husband_name { get; set; }
        public DateTime date_of_birth { get; set; }
        public string nationality { get; set; }
        public string gender { get; set; }
        public string is_active { get; set; }
        public string blood_group { get; set; }
        public string marital_status { get; set; }
        public string adhar_no { get; set; }
        public string adhar_name { get; set; }
        public string employee_status { get; set; }
        public DateTime date_of_joining { get; set; }
        public int probation_period { get; set; }
        public DateTime confirmation_date { get; set; }
        public DateTime resignation_date { get; set; }
        public int notice_period { get; set; }
        public DateTime last_working_date { get; set; }
        public int card_number { get; set; }
        public string email_work { get; set; }
        public int company_id { get; set; }
        public string company_name { get; set; }
        public int location_id { get; set; }
        public string location_name { get; set; }
        public int dept_id { get; set; }
        public string department_name { get; set; }
        public int desig_id { get; set; }
        public string designation_name { get; set; }
        public int grade_id { get; set; }
        public string grade_name { get; set; }
        public int salary_group_id { get; set; }
        public string salary_group { get; set; }
        public int? bank_id { get; set; }
        public string bank_name { get; set; }
        public string bank_IFSC_Code { get; set; }
        public string salary_account_no { get; set; }
        public string salary_bank_branch { get; set; }
        public string current_address1 { get; set; }
        public string current_address2 { get; set; }
        public int current_pincode { get; set; }
        public string permenant_address1 { get; set; }
        public string permenant_address2 { get; set; }
        public int permenant_pincode { get; set; }
        public string permenant_city_name { get; set; }
        public string permenant_state_name { get; set; }
        public int country_id { get; set; }
        public int state_id { get; set; }
        public string state_name { get; set; }
        public string city_name { get; set; }
        public int? city_id { get; set; }
        public string payment_mode { get; set; }
        public string PAN_No { get; set; }
        public string UAN_number { get; set; }
        public string PF_applicable { get; set; }
        public string PF_number { get; set; }
        public string PF_ceiling { get; set; }
        public string PF_group { get; set; }
        public string ESIC_applicable { get; set; }
        public string ESIC_number { get; set; }
        public string ESIC_group { get; set; }
        public string PT_applicable { get; set; }
        public string PT_group { get; set; }
        public string VPF_percentage { get; set; }
        public string SPT_description { get; set; }
        public string is_branch_ESIC_applicable { get; set; }
        public int created_by { get; set; }

        public int gender_id { get; set; } = 0;
        public int salutation_id { get; set; } = 0;
        public int is_active_id { get; set; } = 1;
        public int blood_group_id { get; set; } = 0;
        public int marital_status_id { get; set; } = 0;
        public int employee_status_id { get; set; } = 0;
        public int payment_mode_id { get; set; } = 0;
        public int IS_PF_applicable { get; set; } = 0;
        public int Is_ESIC_applicable { get; set; } = 0;
        public int is_PT_applicable { get; set; } = 0;
        public int pf_group_id { get; set; } = 0;
        public double pf_celing_value { get; set; } = 0;
        public string error_message { get; set; } = "";


    }

    public class mdlFNFProcess
    {
        public int sep_id { get; set; }
        public int emp_id { get; set; }
        public int company_id { get; set; }
        public int x_id { get; set; }

        public string company_name { get; set; }
        public string emp_name { get; set; }

        public string emp_code { get; set; }

        public int dept_id { get; set; }

        public string dept_name { get; set; }

        public int desig_id { get; set; }

        public string desig_name { get; set; }

        public DateTime dob { get; set; }

        public DateTime doj { get; set; }

        public string lcoation { get; set; }

        public string nationality { get; set; }

        public string emptype { get; set; }

        public string religion { get; set; }

        public int grade_id { get; set; }
        public int is_gratuity { get; set; }
        public string grade { get; set; }

        public DateTime resign_dt { get; set; }

        public DateTime last_working_date { get; set; }

        public int notice_days { get; set; }
        public string monthYear { get; set; }

        [MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Invalid PF Number")]
        public string pf_number { get; set; }
        [MaxLength(10)]
        [StringLength(10)]
        [Display(Description = "pan_card_number")]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Invalid PAN CARD Number...")]
        public string pan_card_number { get; set; }

        [MaxLength(20)]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Invalid PF Number")]
        public string esic_number { get; set; }

        public int fnf_id { get; set; }
        public int is_notice_recovery { get; set; }
        public int notice_recovery_days { get; set; }
        public int is_notice_payment { get; set; }
        public int notice_Payment { get; set; }
        public int settlement_type { get; set; }
        public decimal settlment_amt { get; set; }
        public DateTime settlment_dt { get; set; }
        public string remarks { get; set; }
        public decimal net_amt { get; set; }
        public List<asset_request> mdl_assetrequest { get; set; }

        public fnf_attandance mdl_attendance { get; set; }

        public List<tbl_fnf_reimburesment> mdl_reimbursment { get; set; }

        public List<LoanModel> mdl_Loan { get; set; }
        public int? created_by { get; set; }



    }

    public class asset_request
    {
        public int asset_req_id { get; set; }

        public string asset_name { get; set; }

        public string req_remarks { get; set; }

        public string asset_no { get; set; }

        public string asset_type { get; set; }

        public DateTime from_dt { get; set; }

        public DateTime to_dt { get; set; }

        public int is_permanent { get; set; }

        public string final_approve { get; set; }

        public int final_status { get; set; }
        public DateTime issue_dt { get; set; }
        public DateTime replace_dt { get; set; }
        public DateTime submission_dt { get; set; }

        public double recovery_amt { get; set; }

        public int empid { get; set; }

        public int fnf_id { get; set; }
    }

    public class fnf_attandance
    {
        public List<LeaveLedgerModell> mdl_LeaveEncashed { get; set; }

        public List<fnf_attandance_dtl> mdl_FNFAttendance { get; set; }

        public List<fnf_variable> mdl_VariablePay { get; set; }
    }


    public class EmployeeDumpData
    {
        public string company_name { get; set; }
        public string employee_code { get; set; }
        public string status { get; set; }
        public string salutation { get; set; }
        public string employee_name { get; set; }
        public string card_number { get; set; }
        public string gender { get; set; }
        public string group_joining_date { get; set; }
        public string date_of_joining { get; set; }
        public string date_of_birth { get; set; }
        public string department_joining_date { get; set; }
        public string religion { get; set; }
        public string marital_status { get; set; }
        public string official_email_id { get; set; }
        public string location_name { get; set; }
        public string sub_location { get; set; }
        public string department_name { get; set; }
        public string sub_department { get; set; }
        public string nationality { get; set; }
        public string ot_allowed { get; set; }
        public string compoff_allwoed { get; set; }
        public string punch_type { get; set; }
        public string role_name { get; set; }
        public string weekoff { get; set; }
        public string current_employment_type { get; set; }
        public string last_working_date { get; set; }
        public string blood_group { get; set; }
        public string primary_contact_no { get; set; }
        public string secondary_contact_no { get; set; }
        public string primary_email_id { get; set; }
        public string secondary_email_id { get; set; }
        public string permanent_address_line1 { get; set; }
        public string permanent_address_line2 { get; set; }
        public string permanent_pin_code { get; set; }
        public string permanent_country { get; set; }
        public string permanent_state { get; set; }
        public string permanent_city { get; set; }
        public string permanent_document_type { get; set; }
        public string corresponding_address_line1 { get; set; }
        public string corresponding_address_line2 { get; set; }
        public string corresponding_pin_code { get; set; }
        public string corresponding_country { get; set; }
        public string corresponding_state { get; set; }
        public string corresponding_city { get; set; }
        public string corresponding_document_type { get; set; }
        public string emergency_contact_name { get; set; }
        public string emergency_contact_relation { get; set; }
        public string emergency_contact_mobile_no { get; set; }
        public string emergency_address_line1 { get; set; }
        public string emergency_address_line2 { get; set; }
        public string emergency_pin_code { get; set; }
        public string emergency_country { get; set; }
        public string emergency_state { get; set; }
        public string emergency_city { get; set; }
        public string emergency_document_type { get; set; }
        public string uan_number { get; set; }

        public string pf_applicable { get; set; }
        public string pf_number { get; set; }
        public string pf_group { get; set; }
        public string pf_ceiling { get; set; }
        public string is_vpf_applicable { get; set; }
        public string vpf_group { get; set; }
        public string vpf_amount { get; set; }
        public string eps_applicable { get; set; }
        public string is_esic_applicable { get; set; }
        public string esic_no { get; set; }
        public string pan_card_name { get; set; }
        public string pan_card_no { get; set; }
        public string aadhar_card_name { get; set; }
        public string aadhar_card_no { get; set; }
        public string bank_name { get; set; }
        public string account_no { get; set; }
        public string ifsc_code { get; set; }
        public string branch_name { get; set; }
        public string payment_mode { get; set; }
        public string manager_code { get; set; }
        public string manager_name { get; set; }
        public string confirmation_date { get; set; }
        public string grade_name { get; set; }
        public string designation_name { get; set; }
        public string notice_period { get; set; }
        public string emp_type { get; set; }

    }
    public class GetAttendenceSummaryReport
    {
        public string company_name { get; set; }
        public string employee_code { get; set; }
        public string employee_name { get; set; }
        public string date_of_joining { get; set; }
        public string location_name { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
        public string manager_code { get; set; }
        public string manager_name { get; set; }
        public string no_of_working_Days { get; set; }
        public string no_of_days_worked { get; set; }
        public string no_of_Week_off { get; set; }
        public string no_of_Holidays { get; set; }
        public string no_of_leaves_taken { get; set; }
        public string no_of_Half_days_leave_Applied { get; set; }
        public string no_of_days_worked_less_than_8_hours { get; set; }
        public string no_of_days_worked_for_more_than_8_hours_but_less_than_9_hours { get; set; }
        public string no_of_day_applied_on_Date_Outdoor { get; set; }
        public string no_of_Regularised_Days { get; set; }
        public string comp_off_Availed { get; set; }
        public string optional_Holiday_Availed { get; set; }
        public string no_Of_absent_day_Unapplied_leaves { get; set; }
        public string average_Working_hours { get; set; }
    }



    public class EmployeeLeaveAdjustReport
    {
        public string company_name { get; set; }
        public string employee_code { get; set; }
        public string employee_name { get; set; }
        public string location_name { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
        public string leave_type { get; set; }
        public string adjustment_type { get; set; }
        public string particular { get; set; }
        public string value { get; set; }
        public string created_by { get; set; }
        public string credit { get; set; }
        public string dredit { get; set; }
        public string balance { get; set; }
        public string remark { get; set; }
        public string action_date { get; set; }
    }

    public class MonthlySummaryReport
    {
        public string company_name { get; set; }
        public string employee_code { get; set; }
        public string employee_name { get; set; }
        public string location_name { get; set; }
        public string department_name { get; set; }
        public string designation_name { get; set; }
        public string present_count { get; set; }
        public string absent_count { get; set; }
        public string leave_count { get; set; }
        public string unpaid_leave { get; set; }
        public string total { get; set; }
        public string weekly_off_count { get; set; }
        public string comp_off_count { get; set; }
        public string holidays { get; set; }
        public string total_working_days { get; set; }
        public string total_days { get; set; }
    }
    public class AttendanceInOutReport
    {
        public string employee_code { get; set; }
        public string employee_name { get; set; }
        public string company_name { get; set; }
        public string branch_name { get; set; }
        public string department_name { get; set; }
        public string doj { get; set; }
        public string shift_type { get; set; }
        public string shift_timing { get; set; }
        public string shift_hrs { get; set; }
        public string empdata { get; set; }
    }

    public class empWeekoffModel
    {
        public int emp_weekoff_id { get; set; }
        public DateTime effectiveFromDt { get; set; }
        public string weeklyOff { get; set; }
        public string mondayOff { get; set; }
        public string tuesdayOff { get; set; }
        public string wednessdayOff { get; set; }
        public string thursdayOff { get; set; }
        public string fridayOff { get; set; }
        public string saturdayOff { get; set; }
        public string sundayOff { get; set; }
        public DateTime created_date { get; set; }
    }



    public class Emp_Separation
    {
        public int sepration_id { get; set; }  //withdrawal_id
        public int emp_id { get; set; }
        public tbl_employee_master emp_mstr { get; set; }
        public int company_id { get; set; }
        public tbl_company_master comp_master { get; set; }
        public DateTime resignation_dt { get; set; }//resign_dt
        public DateTime req_relieving_date { get; set; }
        public string req_relieving_date1 { get; set; }
        public int req_notice_days { get; set; }
        public int diff_notice_days { get; set; }
        public DateTime policy_relieving_dt { get; set; }
        public int is_relieving_dt_change { get; set; }
        public DateTime last_wrking_dt { get; set; }

        public string req_reason { get; set; } //reason
        public string req_remarks { get; set; }  //remark

        public int? approver1_id { get; set; }
        public tbl_employee_master app1_emp { get; set; }
        public int is_approved1 { get; set; }
        public string app1_remarks { get; set; }
        public DateTime app1_dt { get; set; }

        public int? approver2_id { get; set; }
        public tbl_employee_master app2_emp { get; set; }
        public int is_approved2 { get; set; }
        public string app2_remarks { get; set; }
        public DateTime app2_dt { get; set; }

        public int? apprver3_id { get; set; }
        public tbl_employee_master app3_emp { get; set; }
        public int is_approved3 { get; set; }
        public string app3_remarks { get; set; }
        public DateTime app3_dt { get; set; }
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

    public class Approved_ESeparation_Cancellation
    {

        public int pkid_AppEmpSepCancel { get; set; }
        public int fkid_empSepration { get; set; }
        public tbl_company_master emp_sep { get; set; }
        public DateTime cancellation_dt { get; set; }
        public string cancel_remarks { get; set; }
        public int? approver1_id { get; set; }
        public tbl_employee_master app1_emp { get; set; }
        public int is_approved1 { get; set; }
        public string app1_remarks { get; set; }
        public DateTime app1_dt { get; set; }
        public int? approver2_id { get; set; }
        public tbl_employee_master app2_emp { get; set; }
        public int is_approved2 { get; set; }
        public string app2_remarks { get; set; }
        public DateTime app2_dt { get; set; }
        public int? apprver3_id { get; set; }
        public tbl_employee_master app3_emp { get; set; }
        public int is_approved3 { get; set; }
        public string app3_remarks { get; set; }
        public DateTime app3_dt { get; set; }
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

}

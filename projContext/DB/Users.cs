using projContext.DB.MLM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB
{
    public class tbl_user_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong user_id { get; set; }  // primary key  must be public!    
        [MaxLength(256)]
        public string NormalizedName { get; set; }
        [MaxLength(64)]        
        public string username { get; set; }
        [MaxLength(256)]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        [MaxLength(16)]
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        [MaxLength(512)]
        public string password { get; set; }
        public enmUserType user_type { get; set; }
        public int is_active { get; set; }// this can be Block by admin so the user not able to log in the system
        public int created_by { get; set; }
        public byte LoginFailCount { get; set; }
        public DateTime LoginFailCountdt { get; set; } = DateTime.Now;
        public byte is_logged_in { get; set; }
        public byte is_logged_blocked { get; set; }// block log in due to wrong attemp
        public DateTime logged_blocked_dt { get; set; }
        public DateTime logged_blocked_Enddt { get; set; }
        public DateTime last_logged_dt { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }        
        [ForeignKey("tbl_employee_id_details")] // Foreign Key here
        public int? employee_id { get; set; }
        public tbl_employee_master tbl_employee_id_details { get; set; }
        [ForeignKey("tblCustomerOrganisation")] // Foreign Key here
        public int? CustomerId { get; set; }
        public tblCustomerOrganisation tblCustomerOrganisation { get; set; }
        [ForeignKey("tblDistributorMaster")] // Foreign Key here
        public ulong? DistributorId { get; set; }
        public tblDistributorMaster tblDistributorMaster { get; set; }
    }

    public class tblUserProfilePhoto:d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Sno{ get; set; }  // primary key  must be public!    
        public ulong user_id { get; set; }  // primary key  must be public!    
        [MaxLength(256)]
        public string PhotoPath { get; set; }
        [MaxLength(128)]
        public string FileName { get; set; }
        public bool IsActive { get; set; }
    }

    public class tblUserOTPValidation
    {
        [Key]
        [MaxLength(256)]
        public string SecurityStamp { get; set; }// It is the OTP for Validating Email and Password 
        public ulong Sno { get; set; }
        public ulong UserId { get; set; }
        [MaxLength(256)]
        public string TempUserId { get; set; }// It is the OTP for Validating Email and Password 
        [MaxLength(32)]
        public string SecurityStampValue { get; set; }
        public DateTime EffectiveFromDt { get; set; } = DateTime.Now;
        public DateTime EffectiveToDt { get; set; } = DateTime.Now;
    }

    public class tblUserLoginLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Sno { get; set; }  // primary key  must be public!    
        public ulong user_id { get; set; }  // primary key  must be public!    
        [MaxLength(32)]
        public string IPAddress{ get; set; }
        [MaxLength(256)]
        public string DeviceDetails { get; set; }
        [MaxLength(128)]
        public bool LoginStatus { get; set; }
        [MaxLength(128)]
        public string FromLocation { get; set; }
        public DateTime LoginDateTime { get; set; }
    }



    public class tblUsersApplication: d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Sno { get; set; }
        public ulong UserId { get; set; }
        public enmApplication Applications{ get; set; }
        public bool IsActive { get; set; }
    }

    public class tbl_role_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int role_id { get; set; }  // primary key  must be public!   
        [MaxLength(64)]
        public string role_name { get; set; }
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
        public enmDocumentMaster DocumentMaster { get; set; }
        public enmDocumentType PermissionType { get; set; }
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_guid_detail
    {
        [Key]
        [MaxLength(128)]
        public string id { get; set; }
        public DateTime genration_dt { get; set; }
        [MaxLength(256)]
        public string DeviceId { get; set; }
        [MaxLength(256)]
        public string FromIP { get; set; }
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
        public ulong? user_id { get; set; }
        public tbl_user_master tbl_user_master { get; set; }
        public int is_deleted { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }


}

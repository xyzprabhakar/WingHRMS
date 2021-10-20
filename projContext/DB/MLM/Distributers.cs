using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB.MLM
{
    public class tblDistributorMaster
    {
        [Key]
        public ulong Nid { get; set; }
        [MaxLength(32)]
        public string FirstName { get; set; }
        [MaxLength(32)]
        public string MiddleName { get; set; }
        [MaxLength(32)]
        public string LastName { get; set; }
        public int JoiningCountryId { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DOJ { get; set; }
        public ulong SponsorNid { get; set; }
        public bool IsTerminate { get; set; }
        public DateTime TerminatedDt { get; set; }
        public int DepthId { get; set; }
        public int WidthId { get; set; }
    }
    public class tblDistributorKycStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Sno { get; set; }
        public ulong Nid { get; set; }
        public enmIsKycUpdated Status { get; set; }
        public enmApprovalType IsEmailConfirmed { get; set; }
        public enmApprovalType IsPhoneConfirmed { get; set; }
        public enmApprovalType IsPanConfirmed { get; set; }
        public enmApprovalType IsBankConfirmed { get; set; }
        public enmApprovalType IsIdentityConfirmed { get; set; }
        public enmApprovalType IsNomineeConfirmed { get; set; }
        public DateTime ModifiedDt { get; set; } = DateTime.Now;
    }

    public class tblDistributorTree
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Sno { get; set; }
        public ulong SpNid { get; set; }
        public ulong Nid { get; set; }
    }

    public class tblDistributorAddress
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Sno { get; set; }
        public ulong Nid { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int LocationId { get; set; }
        [MaxLength(128)]
        public string Address { get; set; }
        [MaxLength(64)]
        public string Longitude { get; set; }
        [MaxLength(64)]
        public string Latitude { get; set; }
        public bool IsDelfault { get; set; }
        public bool IsDeleted { get; set; }
    }


    public class tblDistributorCurrency : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }        
        public ulong Nid { get; set; }
        public int CurrencyId { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class tblDistributorCultureInfo : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }
        public ulong Nid { get; set; }
        [MaxLength(8)]
        public string CultureInfo { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class tblDistributorBanks : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }        
        public ulong Nid { get; set; }
        public int BankId { get; set; }
        [MaxLength(32)]
        public string BranchCode { get; set; }
        [MaxLength(32)]
        public string AccountNumber { get; set; }
        [MaxLength(64)]
        public string UPICode { get; set; }
        [MaxLength(128)]
        public string FilePath { get; set; }
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class tblDistributorPan : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }        
        public ulong Nid { get; set; }
        [MaxLength(32)]
        public string PanNo { get; set; }
        [MaxLength(128)]
        public string NameOnPan { get; set; }
        [MaxLength(128)]
        public string FilePath { get; set; }
        public bool IsDeleted { get; set; }

    }

    public class tblIdentityProof : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sno { get; set; }        
        public ulong Nid { get; set; }
        public enmIdentityProof IdentityType { get; set; }
        [MaxLength(32)]
        public string DocumentNumber { get; set; }
        [MaxLength(128)]
        public string NameOnDocument { get; set; }
        [MaxLength(128)]
        public string FilePath { get; set; }
        public bool IsDeleted { get; set; }

    }

    


}

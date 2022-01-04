using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB
{
    public class d_CreatedBy
    {
        public ulong CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; } = DateTime.Now;
        [NotMapped]
        public string CreatedByName { get; set; }
    }
    public class d_ModifiedBy : d_CreatedBy
    {
        [MaxLength(128)]
        public string ModifyRemarks { get; set; }
        public ulong? ModifiedBy { get; set; }
        public DateTime? ModifiedDt { get; set; } = DateTime.Now;
        [NotMapped]
        public string ModifiedByName { get; set; }
    }

    public class d_RequestedBy
    {
        [RegularExpression(@"^[^<>]+$", ErrorMessage = "Character < > are not allowed")]
        [MaxLength(128)]
        public string RequestedRemarks { get; set; }
        public ulong RequestedBy { get; set; }
        public DateTime? RequestedDt { get; set; } = DateTime.Now;
    }


    public class d_ApprovedBy : d_RequestedBy
    {
        public enmApprovalType ApprovalStatus { get; set; }
        [MaxLength(128)]
        [RegularExpression(@"^[^<>]+$", ErrorMessage = "Character < > are not allowed")]
        public string ApprovalRemarks { get; set; }
        public ulong? ApprovedBy { get; set; }
        public DateTime? ApprovedDt { get; set; }
    }


    public class d_Address 
    {
        [RegularExpression(@"^[^<>]+$", ErrorMessage = "Character < > are not allowed")]
        [MaxLength(254)]
        public string OfficeAddress { get; set; }
        [RegularExpression(@"^[^<>]+$", ErrorMessage = "Character < > are not allowed")]
        [MaxLength(254)]
        public string Locality { get; set; }
        [RegularExpression(@"^[^<>]+$", ErrorMessage = "Character < > are not allowed")]
        [MaxLength(254)]
        public string City { get; set; }
        [MaxLength(32)]
        [RegularExpression(@"^[^<>]+$", ErrorMessage = "Character < > are not allowed")]
        [DataType(DataType.PostalCode)]
        public string Pincode { get; set; }
        public int StateId { get; set; }
        public int  CountryId { get; set; }
        [NotMapped]
        public string StateName { get; set; }
        [NotMapped]
        public string CountryName { get; set; }
    }
    public class d_Address_with_Modify_by :d_ModifiedBy
    {
        [RegularExpression(@"^[^<>]+$", ErrorMessage = "Character < > are not allowed")]
        [MaxLength(254)]
        public string OfficeAddress { get; set; }
        [MaxLength(254)]
        [RegularExpression(@"^[^<>]+$", ErrorMessage = "Character < > are not allowed")]
        public string Locality { get; set; }
        [MaxLength(254)]
        [RegularExpression(@"^[^<>]+$", ErrorMessage = "Character < > are not allowed")]
        public string City { get; set; }
        [MaxLength(32)]
        [DataType(DataType.PostalCode)]
        [RegularExpression(@"^[^<>]+$", ErrorMessage = "Character < > are not allowed")]
        public string Pincode { get; set; }        
        public int StateId { get; set; }
        public int CountryId { get; set; }
        [NotMapped]
        public string StateName { get; set; }
        [NotMapped]
        public string CountryName { get; set; }
    }

    public class d_Contact : d_ModifiedBy
    {
        [MaxLength(254)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [MaxLength(254)]
        [DataType(DataType.EmailAddress)]
        public string AlternateEmail { get; set; }
        [MaxLength(16)]
        [DataType(DataType.PhoneNumber)]
        public string ContactNo { get; set; }
        [MaxLength(16)]
        [DataType(DataType.PhoneNumber)]
        public string AlternateContactNo { get; set; }
    }

    public class d_Contact_With_Address : d_Address
    {
        [MaxLength(254)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [MaxLength(254)]
        [DataType(DataType.EmailAddress)]
        public string AlternateEmail { get; set; }
        [MaxLength(16)]
        [DataType(DataType.PhoneNumber)]
        public string ContactNo { get; set; }
        [MaxLength(16)]
        [DataType(DataType.PhoneNumber)]
        public string AlternateContactNo { get; set; }
    }
    public class d_Contact_With_Address_With_Modify_by : d_Address_with_Modify_by
    {
        [MaxLength(254)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [MaxLength(254)]
        [DataType(DataType.EmailAddress)]
        public string AlternateEmail { get; set; }
        [MaxLength(16)]
        [DataType(DataType.PhoneNumber)]
        public string ContactNo { get; set; }
        [MaxLength(16)]
        [DataType(DataType.PhoneNumber)]
        public string AlternateContactNo { get; set; }
    }



}

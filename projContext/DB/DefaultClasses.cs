using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace projContext.DB
{
    public class d_CreatedBy
    {
        public ulong CreatedBy { get; set; }
        public DateTime CreatedDt { get; set; } = DateTime.Now;
    }
    public class d_ModifiedBy : d_CreatedBy
    {
        [MaxLength(128)]
        public string ModifyRemarks { get; set; }
        public ulong? ModifiedBy { get; set; }
        public DateTime? ModifiedDt { get; set; }
    }

    public class d_RequestedBy
    {
        [MaxLength(128)]
        public string RequestedRemarks { get; set; }
        public ulong RequestedBy { get; set; }
        public DateTime? RequestedDt { get; set; } = DateTime.Now;
    }


    public class d_ApprovedBy : d_RequestedBy
    {
        public enmApprovalType ApprovalStatus { get; set; }
        [MaxLength(128)]
        public string ApprovalRemarks { get; set; }
        public ulong? ApprovedBy { get; set; }
        public DateTime? ApprovedDt { get; set; }
    }


    public class d_Address 
    {
        [MaxLength(254)]
        public string OfficeAddress { get; set; }
        [MaxLength(254)]
        public string Locality { get; set; }
        [MaxLength(254)]
        public string City { get; set; }
        [MaxLength(32)]
        public string Pincode { get; set; }
        public int StateId { get; set; }
        public int  CountryId { get; set; }
    }
    public class d_Address_with_Modify_by :d_ModifiedBy
    {
        [MaxLength(254)]
        public string OfficeAddress { get; set; }
        [MaxLength(254)]
        public string Locality { get; set; }
        [MaxLength(254)]
        public string City { get; set; }
        [MaxLength(32)]
        public string Pincode { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
    }

    public class d_Contact : d_ModifiedBy
    {
        [MaxLength(254)]
        public string Email { get; set; }
        [MaxLength(254)]
        public string AlternateEmail { get; set; }
        [MaxLength(16)]
        public string ContactNo { get; set; }
        [MaxLength(16)]
        public string AlternateContactNo { get; set; }
    }

    public class d_Contact_With_Address : d_Address
    {
        [MaxLength(254)]
        public string Email { get; set; }
        [MaxLength(254)]
        public string AlternateEmail { get; set; }
        [MaxLength(16)]
        public string ContactNo { get; set; }
        [MaxLength(16)]
        public string AlternateContactNo { get; set; }
    }
    public class d_Contact_With_Address_With_Modify_by : d_Address_with_Modify_by
    {
        [MaxLength(254)]
        public string Email { get; set; }
        [MaxLength(254)]
        public string AlternateEmail { get; set; }
        [MaxLength(16)]
        public string ContactNo { get; set; }
        [MaxLength(16)]
        public string AlternateContactNo { get; set; }
    }



}

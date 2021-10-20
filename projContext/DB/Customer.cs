using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB
{
    public class tblCustomerOrganisation:d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        [MaxLength(256)]
        public string OrganisationName { get; set; }// It is the OTP for Validating Email and Password 
        [MaxLength(32)]
        public string OrganisationCode { get; set; }
        [MaxLength(128)]
        public string Email { get; set; }
        [MaxLength(32)]
        public string PhoneNumber { get; set; }
        [MaxLength(128)]
        public string OrgLogo { get; set; }
        public DateTime EffectiveFromDt { get; set; } = DateTime.Now;
        public DateTime EffectiveToDt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
    }



}

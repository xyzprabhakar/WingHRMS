using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB.CRM
{

    public class tblCustomerMaster:d_Contact_With_Address_With_Modify_by
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        public int OrgId { get; set; }
        [MaxLength(32)]
        public string Code { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }        
        public DateTime EffectiveFromDt { get; set; } = DateTime.Now;
        public DateTime EffectiveToDt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public enmCustomerType CustomerType { get; set; }
        public string Logo { get; set; }
        [NotMapped]
        public string LogoImage { get; set; }//base 64
        [NotMapped]
        public string LogoImageType { get; set; }
    }
}

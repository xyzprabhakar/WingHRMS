using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB.Masters
{
    public class tblOrganisation :d_Contact_With_Address_With_Modify_by
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrgId { get; set; }
        [MaxLength(16)]
        [RegularExpression(@"^[^<>]+$", ErrorMessage = "Character < > are not allowed")]
        public string Code { get; set; }
        [MaxLength(254)]
        [RegularExpression(@"^[^<>]+$", ErrorMessage = "Character < > are not allowed")]
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public string LogoImage { get; set; }//base 64
        [NotMapped]
        public string LogoImageType { get; set; }
    }

    public class tblCompanyMaster : d_Contact_With_Address_With_Modify_by
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }
        [MaxLength(254)]
        [RegularExpression(@"^[^<>]+$", ErrorMessage = "Character < > are not allowed")]
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public string LogoImage { get; set; }//base 64
        [NotMapped]
        public string LogoImageType { get; set; }
        [NotMapped]
        public string OrgName { get; set; }
        [ForeignKey("tblOrganisation")] // Foreign Key here
        public int? OrgId { get; set; }
        public tblOrganisation tblOrganisation { get; set; }
    }
    public class tblZoneMaster : d_Contact_With_Address_With_Modify_by
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ZoneId { get; set; }
        [MaxLength(254)]
        [RegularExpression(@"^[^<>]+$", ErrorMessage = "Character < > are not allowed")]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
        [ForeignKey("tblCompanyMaster")] // Foreign Key here
        public int? CompanyId { get; set; }
        public tblCompanyMaster tblCompanyMaster { get; set; }
        [ForeignKey("tblOrganisation")] // Foreign Key here
        public int? OrgId { get; set; }
        public tblOrganisation tblOrganisation { get; set; }
    }
    public class tblLocationMaster : d_Contact_With_Address_With_Modify_by
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }
        [MaxLength(254)]
        public string Name { get; set; }
        public enmLocationType LocationType { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public string ZoneName { get; set; }
        [ForeignKey("tblZoneMaster")] // Foreign Key here
        public int? ZoneId { get; set; }
        public tblZoneMaster tblZoneMaster { get; set; }
        [ForeignKey("tblOrganisation")] // Foreign Key here
        public int? OrgId { get; set; }
        public tblOrganisation tblOrganisation { get; set; }
    }

}

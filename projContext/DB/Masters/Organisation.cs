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
        [MaxLength(254)]
        public string Name { get; set; }
        public string Logo { get; set; }        
    }

    public class tblCompanyMaster : d_Contact_With_Address_With_Modify_by
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }
        [MaxLength(254)]
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool IsActive { get; set; }
    }
    public class tblZoneMaster : d_Contact_With_Address_With_Modify_by
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ZoneId { get; set; }
        [MaxLength(254)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
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
    }

}

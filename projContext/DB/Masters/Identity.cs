using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB.Masters
{

    public class tblCodeGenrationMaster:d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public enmCodeGenrationType CodeGenrationType { get; set; }
        public string Prefix { get; set; }
        public bool IncludeCountryCode { get; set; }
        public bool IncludeStateCode { get; set; }
        public bool IncludeCompanyCode { get; set; }
        public bool IncludeZoneCode { get; set; }
        public bool IncludeLocationCode { get; set; }
        public bool IncludeYear { get; set; }
        public bool IncludeMonthYear { get; set; }
        public bool IncludeYearWeek { get; set; }
        public byte DigitFormate { get; set; }
        public int OrgId { get; set; }
        
    }
    public class tblCodeGenrationDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        [ForeignKey("tblCodeGenrationMaster")] // Foreign Key here
        public int? Id { get; set; }
        public tblCodeGenrationMaster tblCodeGenrationMaster { get; set; }
        public int Counter { get; set; }
        [Timestamp]
        [ConcurrencyCheck]
        public byte[] RowVersion { get; set; }

    }
}

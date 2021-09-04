using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB
{
    /// <summary>
    /// Developed by : Rajesh yati
    /// Developed on : 29-Jan-2020
    /// </summary>
    public class tbl_report_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int rpt_id { get; set; }  // primary key  must be public! 
        [StringLength(200)]
        [Display(Description = "report_name")]
        [RegularExpression(@"^[0-9a-zA-Z_'\s'\,]{1,200}$", ErrorMessage = "Invalid report Name")]
        public string rpt_name { get; set; }
        [StringLength(200)]
        [Display(Description = "description")]
        [RegularExpression(@"^[0-9a-zA-Z_'\s'\,]{1,200}$", ErrorMessage = "Invalid description")]
        public string rpt_description { get; set; }
        [Range(0, 1)]
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_rpt_title_master
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int title_id { get; set; }  // primary key  must be public! 
        [ForeignKey("tbl_Component_Master")]
        public int? component_id { get; set; }
        public tbl_component_master tbl_Component_Master { get; set; }
        [ForeignKey("tbl_report_master")]
        public int rpt_id { get; set; }  
        public tbl_report_master tbl_report_master { get; set; }
        public int display_order { get; set; }
        [StringLength(200)]
        [Display(Description = "report_title")]
        [RegularExpression(@"^[0-9a-zA-Z_'\s'\,]{1,200}$", ErrorMessage = "Invalid report title")]
        public string rpt_title { get; set; }
        public  enmPayrollReportProperty payroll_report_property { get; set; }
        [Range(0, 1)]
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }

    }


}

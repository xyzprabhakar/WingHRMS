using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace projLicensing
{
    public class Context : DbContext
    {

        //string connectionString = "Data Source = 192.168.10.129; port = 3306; Initial Catalog = db_hrms_licensing_release2; User ID = root; Password = glaze@123;Allow User Variables=True ; ";
        //string connectionString = "Data Source = 192.168.10.6; port = 3306; Initial Catalog = db_hrms_licensing; User ID = sa; Password = glaze@123;Allow User Variables=True ; ";
        string connectionString = "Data Source = 192.168.10.26; port = 3306; Initial Catalog =db_hrms_licensing; User ID = sa; Password = glaze@123;Allow User Variables=True ; Use Default Command Timeout For EF=true;Default Command Timeout=600;MaximumPoolsize=5000;Convert Zero Datetime=True; ";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(connectionString);
        }
        
        public DbSet<tbl_instance> tbl_instance { get; set; }
        public DbSet<tbl_instance_details> tbl_instance_details { get; set; }
        public DbSet<tbl_api_log> tbl_api_log { get; set; }

    }

   
    public class tbl_instance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int i_id { get; set; }
        public string instance_id { get; set; }
        public string db_connection { get; set; }
        public string organisation { get; set; }
        public int company_count { get; set; }
        public string design_project_url { get; set; }
        public string api_project_url { get; set; }
        public string superadmin_password { get; set; }
        public string superadmin_username { get; set; }
        public string premises_type { get; set; }
        public byte is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }
    public class tbl_instance_details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [ForeignKey("tbl_instance")]
        public int? i_id { get; set; }
        public tbl_instance tbl_instance { get; set; }
        public string instance_id { get; set; }
        public int company_id { get; set; }
        public int employee_count { get; set; }
        public int is_active { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int last_modified_by { get; set; }
        public DateTime last_modified_date { get; set; }
    }

    public class tbl_api_log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string instance_id { get; set; }
        public string api_type { get; set; }
        public DateTime entrydate { get; set; }
        public string response { get; set; }
    }

}

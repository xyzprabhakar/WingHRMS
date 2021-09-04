using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI
{
    public class AppSettings
    {
        public string domain_url { get; set; }
        //public string DbConnectionString { get { return "Data Source = 192.168.10.129; port = 3306; Initial Catalog = db_hrms_glaze_09_10_2020; User ID = root; Password = glaze@123;Allow User Variables=True ; Use Default Command Timeout For EF=true;Default Command Timeout=600;MaximumPoolsize=5000;Convert Zero Datetime=True;"; } set { } }
         public string DbConnectionString { get { return "Data Source = 192.168.10.26; port = 3306; Initial Catalog =db_hrms_glaze; User ID = sa; Password = glaze@123;Allow User Variables=True ; Use Default Command Timeout For EF=true;Default Command Timeout=600;MaximumPoolsize=5000;Convert Zero Datetime=True; "; } set { } }

        //public string DbConnectionString = "Data Source = 192.168.10.6; port = 3306; Initial Catalog = db_hrms_glaze_02_02_21; User ID = sa; Password = glaze@123;Allow User Variables=True ; Use Default Command Timeout For EF=true;Default Command Timeout=600;MaximumPoolsize=5000;Convert Zero Datetime=True;";
       // public string DbConnectionString = "Data Source = 192.168.10.6; port = 3306; Initial Catalog = db_hrms_3_aug_21; User ID = sa; Password = glaze@123;Allow User Variables=True ; Use Default Command Timeout For EF=true;Default Command Timeout=600;MaximumPoolsize=5000;Convert Zero Datetime=True;";

        public string License_domain_url { get; set; }

        public string instance_id { get; set; }
    }
}

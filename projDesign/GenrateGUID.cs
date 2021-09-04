using Microsoft.Extensions.Configuration;
using projContext.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace projDesign
{
    public static class Config
    {
        private static IConfiguration configuration;
        static Config()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            configuration = builder.Build();
        }

        public static string Get(string name)
        {
            string appSettings = configuration[name];
            return appSettings;
        }
        public static void Set(string name,string value)
        {
             configuration[name]=value;            
        }

    }

    public class GenrateGUID
    {
        private int _UserID = 1;
        public string GetGuid
        {
            get
            {
                return _Guid();
            }
        }
        public GenrateGUID()
        {
           
        }
        public GenrateGUID(int UserID)
        {
            _UserID = UserID;
        }
        

        string _Guid()
        {
            projContext.Context db = new projContext.Context();
            tbl_guid_detail tbl = new tbl_guid_detail();
            string gg= Guid.NewGuid().ToString().Replace("-","");
            tbl.id =gg;
            tbl.genrated_by = _UserID;
            tbl.genration_dt = DateTime.Now;
            db.tbl_guid_detail.Add(tbl);
            db.SaveChanges();
            return tbl.id.ToString();
        }
    }
}

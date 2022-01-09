using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projContext;
using projContext.DB;
using projAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Data;
using MySql.Data.MySqlClient;

namespace projAPI.Classes
{
#if(false)
    public class clsReport
    {

        public int _company_id;
        private DateTime PayrollLastdate = DateTime.Now;
        private DateTime PayrollStartdate = DateTime.Now;
        private readonly IHttpContextAccessor _AC;
        private readonly Context _context;
        private readonly IConfiguration _config;

        public clsReport(Context context, IConfiguration config, IHttpContextAccessor AC,int companyid)
        {
            _context = context;
            _config = config;
            _AC = AC;
            _company_id = companyid;
        }

        public DataSet Get_DynamicReport(int monthyear, int reportid)
        {
            DataSet ds = new DataSet();

            using (MySqlConnection connection = new MySqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("proc_payroll_dynamic_repor", connection))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adapter.SelectCommand.Parameters.Add(new MySqlParameter("_monthyear", monthyear));
                    adapter.SelectCommand.Parameters.Add(new MySqlParameter("_reportid", reportid));
                    adapter.SelectCommand.Parameters.Add(new MySqlParameter("_company_id", _company_id));
                    adapter.Fill(ds);
                }
            }

            return ds;


           
        }
    }

#endif
}

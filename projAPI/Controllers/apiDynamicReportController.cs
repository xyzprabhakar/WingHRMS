using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using DinkToPdf;
using DinkToPdf.Contracts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using projAPI.Classes;
using projAPI.Model;
using projContext;
using projContext.DB;
using static projAPI.Model.payroll;

namespace projAPI.Controllers
{
#if(false)
    [Route("api/[controller]")]
    [ApiController]
    public class apiDynamicReportController : Controller
    {
        private readonly Context _context;
        private IHostingEnvironment _hostingEnvironment;
        private IConverter _converter;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IHttpContextAccessor _AC;
        IConfiguration _config;

        public apiDynamicReportController(Context context, IConverter converter, IHostingEnvironment environment, IOptions<AppSettings> appSettings, IHttpContextAccessor AC, IConfiguration config)
        {
            _context = context;
            _converter = converter;
            _hostingEnvironment = environment;
            _appSettings = appSettings;
            _AC = AC;
            _config = config;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("Get_Salary_input_Report_by_monthyearandcompID/{monthyear}/{company_id}/{rpt_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.SalaryReportArrea))]
       
        public IActionResult Get_Salary_input_Report_by_monthyearandcompID([FromRoute] int monthyear, int company_id, int rpt_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            try
            {
                DataSet ds = new DataSet();
                clsReport objcls = new clsReport(_context, _config, _AC, company_id);
                ds = objcls.Get_DynamicReport(monthyear, rpt_id);


                Dictionary<string, string> _colums = new Dictionary<string, string>();
                DataTable dt = new DataTable();

                int j = 1;
                //Add column name
                foreach (DataColumn col in ds.Tables[0].Columns)
                {
                    _colums.Add("col_" + j, col.ColumnName);
                    dt.Columns.Add(new DataColumn("col_" + j));
                    j++;
                }


                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int i = 1;
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn column in ds.Tables[0].Columns)
                    {
                        dr["col_" + i] = row[column].ToString();
                        i++;
                    }

                    dt.Rows.Add(dr);
                }



                var _finaldata = new { allcolumns = _colums.Select(p => new { title = p.Value, data = p.Key }).ToList(), data = dt };
                return Ok(_finaldata);

            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;

                return Ok(objresponse);
            }
        }

       

    }

#endif
}
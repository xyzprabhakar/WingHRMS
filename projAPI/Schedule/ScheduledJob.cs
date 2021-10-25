using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using projAPI.Controllers;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using projContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using projAPI.Classes;

namespace projAPI.Schedule
{
#if(false)
    public class ScheduledJob:IJob
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<ScheduledJob> logger;
        private readonly Context _context;
        private readonly IHttpContextAccessor _AC;
        private IHostingEnvironment _hostingEnvironment;
        clsCurrentUser _clsCurrentUser;
        clsLeaveCredit _clsLeaveCredit;
        public ScheduledJob(Context context,IConfiguration configuration, ILogger<ScheduledJob> logger, IHttpContextAccessor AC,clsCurrentUser _clsCurrentUser, clsLeaveCredit _clsLeaveCredit)
        {
            _context = context;
            this.logger = logger;
            this.configuration = configuration;
            _AC = AC;
            this._clsCurrentUser = _clsCurrentUser;
            this._clsLeaveCredit = _clsLeaveCredit;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {

              
        AttendanceController attController = new AttendanceController(_context, _hostingEnvironment, _AC, _clsCurrentUser, _clsLeaveCredit);

                //DateTime cdate = DateTime.Now;
                //DateTime pdate;
                //int CYear = cdate.Year;
                //int CMonth = cdate.Month;
                //int CDay = cdate.Day;
                //int PMonth;
                //int PYear;

                //if (CDay>=25)
                //{
                //    PMonth = CMonth;
                //    pdate = DateTime.Parse(CMonth +"/25/"+ CYear +"");
                //    PYear = CYear;
                //}
                //else
                //{
                //        if (CMonth==1)
                //        {
                //            PMonth = CMonth + 11;
                //            PYear = CYear - 1;
                //        }
                //        else
                //        {
                //            PMonth = CMonth - 1;
                //            PYear = CYear;
                //        }

                //    pdate = DateTime.Parse(PMonth.ToString() + "/25/" + PYear + "");
                //}


                // Now that you have an instance of your controller call the method
                //attController.Post_save_attendance_details(Convert.ToDateTime("01-Mar-2019"), Convert.ToDateTime("18-Mar-2019"), 0);
                // attController.EmployeeLeaveCreditAndDebit(Convert.ToDateTime("26-Jan-2019"), Convert.ToDateTime("25-Feb-2019"));

                //         attController.Post_save_attendance_details(pdate, cdate, 1);

                //            attController.EmployeeLeaveCreditAndDebit(pdate,cdate);



                await Task.CompletedTask;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
#endif
}

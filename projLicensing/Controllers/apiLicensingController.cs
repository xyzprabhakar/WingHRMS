using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace projLicensing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class apiLicensingController : ControllerBase
    {
        private readonly Context _context;

        public apiLicensingController(Context context)
        {
            _context = context;
        }


        [HttpGet("{company_id}/{instance_id}")]       
        public async Task<IActionResult> GetCompanyLicensingInfo([FromRoute] int company_id, string instance_id)
        {
            int total_no_of_employee =_context.tbl_instance_details.Where(a => a.is_active == 1 && a.company_id == company_id && a.instance_id == instance_id).Select(a => a.employee_count).FirstOrDefault();

            return Ok(total_no_of_employee);
        }
    }
}

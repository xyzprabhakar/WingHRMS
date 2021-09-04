using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace projLicensing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class apiLicenseController : ControllerBase
    {
        private readonly Context _context;

        public apiLicenseController(Context context)
        {
            _context = context;
        }
        // GET: api/apiLicense
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/apiLicense/5
        [HttpGet("{company_id}", Name = "Get")]
        public int Get(int company_id)
        {
            int total_no_of_employee = 0;

            return total_no_of_employee;
        }

        // POST: api/apiLicense
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/apiLicense/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("GetInsatnceData")]
        public IActionResult GetInsatnceData()
        {
            try
            {
                List<tbl_instance> InstanceDetails = _context.tbl_instance.Where(p => p.is_active == 1).ToList();

                return Ok(InstanceDetails);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }
        [HttpPost]
        [Route("SaveApiLogData")]
        public async Task<IActionResult> Save_ApiLog(tbl_api_log tbllog)
        {
            try
            {
                if (tbllog == null)
                {
                    return BadRequest(ModelState);
                }

                var result = _context.tbl_api_log.Add(tbllog);
                await _context.SaveChangesAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}

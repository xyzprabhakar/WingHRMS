using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projContext.DB;
using projContext;
using System.Xml.Linq;
using projAPI.Classes;

namespace projAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly Context _context;        

        public ValuesController(Context context)
        {
            _context = context;
            
        }

        // GET api/values
        [HttpGet]
        // //[Authorize(Policy = "1")]
        //public ActionResult<IEnumerable<mdlSalaryInputValues>> Get()
        //{
        //    clsComponentValues ob = new clsComponentValues(_context, 1, 201905, 0, 8, "0");
        //    return ob.CalculateComponentValues(); ;
        //}
        public ActionResult<string> Get()
        {
            int v = 0x12;
            float c = 23.5f;
            double b = 16.45;
            decimal a = Convert.ToDecimal( b);
            return a.ToString();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            List<tbl_attendance_details_manual> tadms = new List<tbl_attendance_details_manual>();
            for (int i = 0; i < 10; i++)
            {
                tbl_attendance_details_manual t1 = new tbl_attendance_details_manual()
                {
                    attendance_dt = Convert.ToDateTime("1-may-2020").AddDays(i),
                    start_in = Convert.ToDateTime("1-jan-2000 10:00:00"),
                    start_out = Convert.ToDateTime("1-jan-2000 18:30:00"),
                    day_status = 1,
                    emp_id = 10022,
                    entry_date = DateTime.Now,
                    user_id = 1
                };
                tadms.Add(t1);
            }
            clsManualAttendance ob = new clsManualAttendance(tadms, _context);
            ob.SaveData();
            //XElement payrollcompdet = XElement.Load("../projContext/XMLData/ComponentDetailMaster.xml");
            //IEnumerable<XElement> compdet = payrollcompdet.Elements();
            //List<tbl_component_property_details> tblcompdets = new List<tbl_component_property_details>();
            //foreach (var cmp in compdet)
            //{
            //    tbl_component_property_details tblcompdet = new tbl_component_property_details();
            //    tblcompdet.component_id = Convert.ToInt16(cmp.Element("component_id").Value);
            //    tblcompdet.company_id = Convert.ToInt16(cmp.Element("company_id").Value);
            //    tblcompdet.salary_group_id = Convert.ToInt16(cmp.Element("salary_group_id").Value);
            //    tblcompdet.component_type = Convert.ToInt16(cmp.Element("component_type").Value);
            //    tblcompdet.is_salary_comp = Convert.ToByte(cmp.Element("is_salary_comp").Value);
            //    tblcompdet.is_tds_comp = Convert.ToByte(cmp.Element("is_tds_comp").Value);
            //    tblcompdet.is_data_entry_comp = Convert.ToByte(cmp.Element("is_data_entry_comp").Value);
            //    tblcompdet.payment_type = Convert.ToByte(cmp.Element("payment_type").Value);
            //    tblcompdet.formula = cmp.Element("formula").Value;
            //    tblcompdet.function_calling_order = Convert.ToInt16(cmp.Element("function_calling_order").Value);
            //    tblcompdet.is_user_interface = Convert.ToInt16(cmp.Element("is_user_interface").Value);
            //    tblcompdets.Add(tblcompdet);
            //}
            //_context.tbl_component_property_details.AddRange(tblcompdets);
            //_context.SaveChanges();
             return "Save Data";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("GetGUID")]
        public string GetGUID(int id)
        {
            return new GenrateGUID(id).GetGuid;
        }
    }
}

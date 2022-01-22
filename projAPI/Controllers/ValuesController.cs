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
using System.Runtime.Serialization;
using projAPI.Services;
using projAPI.Model;
using projContext.DB.Masters;
using System.IO;
using projContext.DB.CRM.Travel;

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
      
        // //[Authorize(Policy = "1")]
        //public ActionResult<IEnumerable<mdlSalaryInputValues>> Get()
        //{
        //    clsComponentValues ob = new clsComponentValues(_context, 1, 201905, 0, 8, "0");
        //    return ob.CalculateComponentValues(); ;
        //}
        public class mdl {
            public string Name { get; set; }
            [IgnoreDataMember]
            public DateTime DOB{ get; set; }
        }
        [HttpGet]
        public ActionResult<List<Tuple<string,string>>> Get()
        {
            List<Tuple<string, string>> ob = new List<Tuple<string, string>>();
            ob.Add(Tuple.Create("Prabhakar", "Kumar"));
            ob.Add(Tuple.Create("Divakar", "Kumar"));
            ob.Add(Tuple.Create("Shresht", "Kumar"));

            return ob;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
           // SetUserData();
            //List<tbl_attendance_details_manual> tadms = new List<tbl_attendance_details_manual>();
            //for (int i = 0; i < 10; i++)
            //{
            //    tbl_attendance_details_manual t1 = new tbl_attendance_details_manual()
            //    {
            //        attendance_dt = Convert.ToDateTime("1-may-2020").AddDays(i),
            //        start_in = Convert.ToDateTime("1-jan-2000 10:00:00"),
            //        start_out = Convert.ToDateTime("1-jan-2000 18:30:00"),
            //        day_status = 1,
            //        emp_id = 10022,
            //        entry_date = DateTime.Now,
            //        user_id = 1
            //    };
            //    tadms.Add(t1);
            //}
            //clsManualAttendance ob = new clsManualAttendance(tadms, _context);
            //ob.SaveData();
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
        [Route("DefaultApplication")]
        public void DefaultApplication([FromServices] IsrvUsers isrvUsers )
        {
            return;

            List<enmApplication> Application = new List<enmApplication>();
            foreach (var d in Enum.GetValues(typeof(enmApplication)))
            {
                Application.Add((enmApplication)d);
            }
         //   isrvUsers.SetUserApplication(1,Application,1);
        }


        [HttpGet]
        [Route("DefaultDocuments")]
        public void DefaultDocument([FromServices] IsrvUsers isrvUsers,[FromServices]MasterContext masterContext)
        {
            
            DateTime dt = DateTime.Now;
            {
                //Super Admin Role
                var defaultRole = masterContext.tblRoleMaster.Where(p => p.RoleId == (int)enmDefaultRole.SuperAdmin).FirstOrDefault();
                if (defaultRole == null)
                {
                    defaultRole = new tblRoleMaster() {RoleId = (int)enmDefaultRole.SuperAdmin, RoleName = nameof(enmDefaultRole.SuperAdmin), CreatedBy = 1, CreatedDt = dt, IsActive = true, ModifiedBy = 1, ModifiedDt = dt };
                    masterContext.tblRoleMaster.Add(defaultRole);
                    masterContext.SaveChanges();
                }
                //role Claim
                //role Claim

                List<mdlRoleDocument> document = new List<mdlRoleDocument>();
                foreach (var d in Enum.GetValues(typeof(enmDocumentMaster)))
                {
                    var edm = (enmDocumentMaster)d;
                    document.Add(new mdlRoleDocument() { documentId = edm, PermissionType = edm.GetDocumentDetails().DocumentType });
                }
                isrvUsers.SetRoleDocument(new mdlRoleMaster() { roleId = defaultRole.RoleId, roleDocument = document }, 1);
            }
            {
                //Customer
                var defaultRole = masterContext.tblRoleMaster.Where(p => p.RoleId == (int)enmDefaultRole.CustomerAdmin).FirstOrDefault();
                if (defaultRole == null)
                {
                    defaultRole = new tblRoleMaster() { RoleId= (int)enmDefaultRole.CustomerAdmin, RoleName = nameof(enmDefaultRole.CustomerAdmin), CreatedBy = 1, CreatedDt = dt, IsActive = true, ModifiedBy = 1, ModifiedDt = dt };
                    masterContext.tblRoleMaster.Add(defaultRole);
                    masterContext.SaveChanges();
                }
                List<mdlRoleDocument> document = new List<mdlRoleDocument>();
                document.Add(new mdlRoleDocument() {documentId=enmDocumentMaster.CRM_Customer_Markup,PermissionType=enmDocumentType.Create});
                document.Add(new mdlRoleDocument() { documentId = enmDocumentMaster.CRM_Customer_Markup, PermissionType = enmDocumentType.Delete });
                document.Add(new mdlRoleDocument() { documentId = enmDocumentMaster.CRM_Customer_Markup, PermissionType = enmDocumentType.DisplayMenu });
                
                isrvUsers.SetRoleDocument(new mdlRoleMaster() { roleId = defaultRole.RoleId, roleDocument = document }, 1);
            }


            
        }

        [HttpGet]
        [Route("DefaultFlightFareClass")]
        public bool DefaultFlightFareClass([FromServices] TravelContext travelContext)
        {
            
            List<tblFlightClassOfBooking> tbl = new List<tblFlightClassOfBooking>();

            DateTime currentDt = DateTime.Now;
            string[] code= { "A",  "B",    "C",    "D",    "E",    "F",    "G",    "H",    "J",    "K",    "L",    "M",    "M",    "P",    "Q",    "R",    "S",    "T",    "U",    "V",    "W",    "X",    "Y",    "Z"};
            string[] name = { "First Class Discounted", "Economy/Coach – Usually an upgradable fare to Business ",  "Business Class",   "Business Class Discounted",    "Shuttle Service (no reservation allowed) or Economy/Coach Discounted", "First Class",  "Conditional Reservation",  "Economy/Coach Discounted – Usually an upgradable fare to Business",    "Business Class Premium",   "Economy/Coach Discounted", "Economy/Coach Discounted", "Economy/Coach Discounted – Usually an upgradable fare to Business",    "Economy/Coach Discounted", "First Class Premium",  "Economy/Coach Discounted", "First Class Suite or Supersonic (discontinued)",   "Economy/Coach",    "Economy/Coach Discounted", "Shuttle Service (no reservation needed/seat guaranteed)",  "Economy/Coach Discounted", "Economy/Coach Premium",    "Economy/Coach Discounted", "Economy/Coach",    "Business Class Discounted"};
            string[] description = { "FIRST CLASS", "ECONOMY",  "BUSINESS", "BUSINESS", "ECONOMY",  "FIRST CLASS",  "ECONOMY",  "ECONOMY",  "BUSINESS CLASS",   "ECONOMY",  "ECONOMY",  "ECONOMY",  "ECONOMY",  "FIRST CLASS",  "ECONOMY",  "FIRST CLASS",  "ECONOMY",  "ECONOMY",  "ECONOMY",  "ECONOMY",  "ECONOMY",  "ECONOMY",  "ECONOMY",  "BUSINESS CLASS",};
            for (int i = 0; i < code.Length; i++)
            {
                if (travelContext.tblFlightClassOfBooking.Where(p => p.BookingClassCode == code[i]).Count() == 0)
                {
                    travelContext.tblFlightClassOfBooking.Add(new tblFlightClassOfBooking() { BookingClassCode=code[i],Name=name[i],GenerlizedName=description[i],IsActive=true, CreatedBy=1,CreatedDt= currentDt ,ModifiedBy=1,ModifiedDt=currentDt,ModifyRemarks=string.Empty});
                }
            }
            travelContext.SaveChanges();

            return true;
        }

        [HttpGet]
        [Route("SetCountryState")]
        public bool SetCountryState([FromServices]MasterContext masterContext )
        {
            return false;
            DateTime currentDt = DateTime.Now; 
            List<tblCountry> countrys = new List<tblCountry>();
            var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ImportData", "CountryState","countries.csv");
            int Id = 0;
            int CountryId = 0;
            using (var rd = new StreamReader(file))
            {
                while (!rd.EndOfStream)
                {
                    var splits = rd.ReadLine().Split(',');
                    Id = 0;
                    int.TryParse(splits[0], out Id);
                    if (Id> 0)
                    {
                        countrys.Add(new tblCountry()
                        {
                            CountryId = Id,
                            Code = splits[3],
                            Name = splits[1].Replace("\"", ""),
                            ContactPrefix = splits[4],
                            CreatedBy = 1,
                            ModifiedBy = 1,
                            CreatedDt = currentDt,
                            ModifiedDt = currentDt,
                            IsActive = true
                        });
                    }
                    
                    
                }
            }
            masterContext.tblCountry.UpdateRange(countrys);

            List<tblState> States = new List<tblState>();
            var file1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ImportData", "CountryState", "states.csv");
            using (var rd = new StreamReader(file1))
            {
                while (!rd.EndOfStream)
                {
                    var splits = rd.ReadLine().Split(',');
                    Id = 0; CountryId=0;
                    int.TryParse(splits[0], out Id);
                    int.TryParse(splits[2], out CountryId);
                    if (Id > 0 && CountryId>0)
                    {
                        States.Add(new tblState()
                        {
                            StateId = Id,
                            Code = splits[4],
                            Name = splits[1].Replace("\"", ""),
                            CountryId = CountryId,
                            CreatedBy = 1,
                            ModifiedBy = 1,
                            CreatedDt = currentDt,
                            ModifiedDt = currentDt,
                            IsActive = true

                        }); ;
                    }
                    
                }
            }
            masterContext.tblState.AddRange(States);
            masterContext.SaveChanges();
            return true;
        }


        private void SetUserData()
        {
            throw new NotImplementedException();
#if false
            var AllEmp=_context.tbl_employee_master.Where(p => p.is_active == 1).ToList();
            var ExistingCompanyMap=_context.tbl_employee_company_map.Where(p => p.is_deleted == 0).ToList();
            List<tbl_employee_company_map> NewCompanyMap = AllEmp.Select(p => new tbl_employee_company_map
            {
                company_id=1, is_deleted=0,created_by=1,created_date=new DateTime(2021,01,01),last_modified_by=1, 
                last_modified_date= new DateTime(2021, 01, 01),
                employee_id=p.employee_id,
                is_default=true
            }).ToList();
            for (int i = NewCompanyMap.Count - 1; i>= 0; i--)
            {
                if (ExistingCompanyMap.Any(p => p.employee_id == NewCompanyMap[i].employee_id && p.company_id == 1))
                {
                    NewCompanyMap.RemoveAt(i);
                }
            }            
             _context.tbl_employee_company_map.AddRange(NewCompanyMap);
            List<tbl_user_master> NewUserMaster = AllEmp.Select(p => new tbl_user_master
            {
                username = p.emp_code.Trim().ToUpper(),
                password = AESEncrytDecry.EncryptStringAES(p.emp_code.Trim().ToLower()),
                user_type=1,
                created_by = 1,
                created_date = new DateTime(2021, 01, 01),
                last_modified_by = 1,
                last_modified_date = new DateTime(2021, 01, 01),
                employee_id = p.employee_id,
                is_active =1,
                default_company_id=1,
                
            }).ToList();
            var ExistinguserId=_context.tbl_user_master.Where(p => p.is_active == 0).ToList();
            for (int i = NewUserMaster.Count - 1; i >= 0; i--)
            {
                if (ExistinguserId.Any(p => p.employee_id == NewUserMaster[i].employee_id && p.username == NewUserMaster[i].username))
                {
                    NewUserMaster.RemoveAt(i);
                }
            }
            _context.tbl_user_master.AddRange(NewUserMaster);

            _context.SaveChanges();

            List<tbl_user_role_map> NewUserRole = ExistinguserId.Select(p => new tbl_user_role_map
            {
                role_id = (int)enmRoleMaster.Manager,
                user_id =  p.user_id,
                created_by = 1,
                created_date = new DateTime(2021, 01, 01),
                last_modified_by = 1,
                last_modified_date = new DateTime(2021, 01, 01),
                is_deleted = 0,
            }).ToList();
            var ExistingUserRole = _context.tbl_user_role_map.Where(p => p.is_deleted == 0).ToList();
            for (int i = NewUserRole.Count - 1; i >= 0; i--)
            {
                if (ExistingUserRole.Any(p => p.user_id == NewUserRole[i].user_id))
                {
                    NewUserRole.RemoveAt(i);
                }
            }
            _context.tbl_user_role_map.AddRange(NewUserRole);


            var AllEmpOfficail =_context.tbl_emp_officaial_sec.Where(p => p.is_deleted == 0).ToList();
            List<tbl_employment_type_master> NewemploymentType = AllEmpOfficail.Select(p => new tbl_employment_type_master
            {
                employee_id = p.employee_id,
                employment_type = (byte)EmployeeType.Probation,
                duration_days = 0,
                duration_start_period = DateTime.Now,
                duration_end_period = new DateTime(2200, 1, 1),
                actual_duration_days = 0,
                actual_duration_start_period = DateTime.Now,
                actual_duration_end_period = new DateTime(2200, 1, 1),
                is_deleted = 0,
                created_by = 1,
                created_date = new DateTime(2200, 1, 1),
                last_modified_by = 0,
                last_modified_date = new DateTime(2200, 1, 1),
                effective_date = p.date_of_joining,
            }).ToList();

            //NewemploymentType.AddRange(AllEmpOfficail.Where(p=>p.current_employee_type==3).Select(p => new tbl_employment_type_master
            //{
            //    employee_id = p.employee_id,
            //    employment_type = (byte)EmployeeType.Confirmend,
            //    duration_days = 0,
            //    duration_start_period = DateTime.Now,
            //    duration_end_period = new DateTime(2200, 1, 1),
            //    actual_duration_days = 0,
            //    actual_duration_start_period = DateTime.Now,
            //    actual_duration_end_period = new DateTime(2200, 1, 1),
            //    is_deleted = 0,
            //    created_by = 1,
            //    created_date = new DateTime(2200, 1, 1),
            //    last_modified_by = 0,
            //    last_modified_date = new DateTime(2200, 1, 1),
            //    effective_date = p.confirmation_date,
            //}));

            var ExistingemploymentType = _context.tbl_employment_type_master.Where(p => p.is_deleted== 0).ToList();
            for (int i = NewemploymentType.Count - 1; i >= 0; i--)
            {
                if (ExistingemploymentType.Any(p => p.employee_id == NewemploymentType[i].employee_id && p.employment_type == NewemploymentType[i].employment_type))
                {
                    NewemploymentType.RemoveAt(i);
                }
            }
            _context.tbl_employment_type_master.AddRange(NewemploymentType);



            _context.SaveChanges();

#endif

        }
    }
}

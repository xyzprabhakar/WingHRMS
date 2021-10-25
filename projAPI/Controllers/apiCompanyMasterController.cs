using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projAPI.Classes;
using projAPI.Model;
using projContext;
using projContext.DB;

namespace projAPI.Controllers
{
    #if (false)
    [Route("api/[controller]")]
    [ApiController]
    public class apiCompanyMasterController : ControllerBase
    {
        private readonly Context _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly clsCurrentUser _clsCurrentUser;

        public apiCompanyMasterController(Context context, IHostingEnvironment environment, clsCurrentUser _clsCurrentUser)
        {
            _context = context;
            _hostingEnvironment = environment;
            this._clsCurrentUser = _clsCurrentUser;
        }



        // GET: api/apiCompanyMaster/GetLastCompanyId
        [Route("GetLastCompanyId")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.CompanyReport))]
        public ActionResult GetLastCompanyId()
        {
            try
            {
                var result = (from a in _context.tbl_company_master select a).OrderByDescending(x => x.company_id).FirstOrDefault();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        // GET: api/apiCompanyMaster/fromdate/todate
        //[HttpGet("{fromdate}/{todate}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.CompanyReport))]
        public ActionResult Gettbl_company_master()
        {
            ResponseMsg objResult = new ResponseMsg();

            var result = _context.tbl_company_emp_setting.Where(x => _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0) && x.is_active != 3).Select(p => new
            {

                company_id = p.tbl_company_master.company_id,
                company_name = p.tbl_company_master.company_name,
                company_code = p.tbl_company_master.company_code,
                prefix_for_employee_code = p.prefix_for_employee_code,
                number_of_character_for_employee_code = p.number_of_character_for_employee_code,
                address_line_one = p.tbl_company_master.address_line_one.ToString() + " " + p.tbl_company_master.address_line_two.ToString(),
                primary_email_id = p.tbl_company_master.primary_email_id,
                primary_contact_number = p.tbl_company_master.primary_contact_number,
                company_website = p.tbl_company_master.company_website,
                is_active = p.tbl_company_master.is_active,
                created_by = p.tbl_company_master.created_by,
                created_date = p.tbl_company_master.created_date,
                last_modified_by = p.tbl_company_master.last_modified_by,
                last_modified_date = p.tbl_company_master.last_modified_date,

            }).ToList();

            if (result.Count == 0)
            {
                objResult.Message = "Record Not Found...!";
                objResult.StatusCode = 0;
                return Ok(objResult);
            }
            else
            {
                return Ok(result);
            }
        }
        // GET: api/apiCompanyMaster/5
        [HttpGet("{id}")]
        //[Authorize(Policy = "1002")]
        [Authorize(Policy = nameof(enmMenuMaster.CompanyReport))]
        public ActionResult Gettbl_company_master([FromRoute] int id)
        {
            ResponseMsg objResult = new ResponseMsg();



            var result =
 from c in _context.tbl_company_master
 join s in _context.tbl_company_emp_setting on c.company_id equals s.company_id
 where c.company_id == id && _clsCurrentUser.CompanyId.Contains(c.company_id)
 select new
 {
     company_id = c.company_id,
     company_code = c.company_code,
     company_name = c.company_name,
     is_emp_code_manual_genrate = c.is_emp_code_manual_genrate,
     address_line_one = c.address_line_one,
     address_line_two = c.address_line_two,
     pin_code = c.pin_code,
     city_id = c.city_id,
     state_id = c.state_id,
     country_id = c.country_id,
     primary_email_id = c.primary_email_id,
     secondary_email_id = c.secondary_email_id,
     primary_contact_number = c.primary_contact_number,
     prefix_for_employee_code = s.prefix_for_employee_code,
     number_of_character_for_employee_code = s.number_of_character_for_employee_code,
     secondary_contact_number = c.secondary_contact_number,
     company_website = c.company_website,
     company_logo = c.company_logo,
     is_active = c.is_active,
     created_by = c.created_by,
     created_date = c.created_date,
     last_modified_by = c.last_modified_by,
     last_modified_date = c.last_modified_date,
     tbl_Company_Master_Log = c.is_emp_code_manual_genrate,
 };
            if (result == null)
            {
                objResult.Message = "Record Not Found...!";
                objResult.StatusCode = 0;
                return Ok(objResult);
            }

            return Ok(result);
        }

        // PUT: api/apiCompanyMaster/5
        [HttpPost("{id}")]
        //[Authorize(Policy = "1003")]
        [Authorize(Policy = nameof(enmMenuMaster.CreateCompany))]
        public async Task<IActionResult> Puttbl_company_master([FromRoute] int id, [FromBody]  Company company_master)
        {
            ResponseMsg objResult = new ResponseMsg();

            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        //Create new obj of tbl_company_master table
                        tbl_company_master tbl_com_master = _context.tbl_company_master.FirstOrDefault(p => p.company_id == id && _clsCurrentUser.CompanyId.Contains(p.company_id));

                        // Check Company Name
                        string CompanyName = company_master.company_name;

                        //chaeck Email Id
                        string CompanyEmail = company_master.primary_email_id.Trim().ToUpper();

                        //Get Company Data
                        var all_comp_data = _context.tbl_company_master.Where(a => a.company_id != id).Select(a => a.company_name.Trim().ToUpper()).ToList();


                        if (tbl_com_master != null)
                        {
                            // Check Company Name
                            if (all_comp_data.Contains(company_master.company_name.Trim().ToUpper()))
                            {
                                objResult.Message = "Company Name Already Exist...!";
                                objResult.StatusCode = 0;
                                return Ok(objResult);
                            }
                            //chaeck Email Id
                            else if (tbl_com_master.primary_email_id.Trim().ToUpper() == CompanyEmail && tbl_com_master.company_id != id)
                            {
                                objResult.Message = "Company Email Id Already Exist...!";
                                objResult.StatusCode = 0;
                                return Ok(objResult);
                            }
                            else
                            {
                                //Update Code Here

                                // Image Upload
                                if (tbl_com_master.company_logo != company_master.company_logo)
                                {
                                    string[] replaceThese = { "data:image/png;base64,", "data:image/jpeg;base64,", "data:image/jpg;base64," };
                                    var data = company_master.company_logo;


                                    if (data != null && data != "")
                                    {
                                        foreach (string curr in replaceThese)
                                        {
                                            data = data.Replace(curr, string.Empty);
                                        }

                                        byte[] imageBytes = System.Convert.FromBase64String(data);
                                        string imageName = company_master.company_name + ".jpg";

                                        var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg", ".JPG", ".PNG", ".JPG", "JPEG" };

                                        var ext = ".jpg"; //getting the extension
                                        if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                                        {
                                            string name = Path.GetFileNameWithoutExtension(imageName); //getting file name without extension  
                                            string MyFileName = name + ext;

                                            var webRoot = _hostingEnvironment.WebRootPath;

                                            if (!Directory.Exists(webRoot + "/CompanyLogo/" + company_master.company_name + "/"))
                                            {
                                                Directory.CreateDirectory(webRoot + "/CompanyLogo/" + company_master.company_name + "/");
                                            }

                                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/CompanyLogo/" + company_master.company_name + "/", MyFileName);
                                            System.IO.File.WriteAllBytes(path, imageBytes);//save image file
                                                                                           //update file name
                                            tbl_com_master.company_logo = "/CompanyLogo/" + company_master.company_name + "/" + MyFileName;
                                        }
                                        else
                                        {
                                            objResult.StatusCode = 1;
                                            objResult.Message = "Please select only JPG, PNG, JPEG Images";
                                            return Ok(objResult);

                                        }
                                    }
                                }

                                tbl_com_master.company_name = company_master.company_name;
                                tbl_com_master.is_emp_code_manual_genrate = company_master.is_emp_code_manual_genrate;
                                // tbl_com_master.prefix_for_employee_code = tbl_company_master.prefix_for_employee_code;
                                //  tbl_com_master.number_of_character_for_employee_code = tbl_company_master.number_of_character_for_employee_code;
                                tbl_com_master.address_line_one = company_master.address_line_one;
                                tbl_com_master.address_line_two = company_master.address_line_two;
                                tbl_com_master.country_id = company_master.country_id;
                                tbl_com_master.state_id = company_master.state_id;
                                tbl_com_master.city_id = company_master.city_id;
                                tbl_com_master.pin_code = company_master.pin_code;
                                tbl_com_master.primary_email_id = company_master.primary_email_id;
                                tbl_com_master.secondary_email_id = company_master.secondary_email_id;
                                tbl_com_master.primary_contact_number = company_master.primary_contact_number;
                                tbl_com_master.secondary_contact_number = company_master.secondary_contact_number;
                                tbl_com_master.company_website = company_master.company_website;
                                tbl_com_master.is_active = company_master.is_active;
                                tbl_com_master.last_modified_by = _clsCurrentUser.EmpId;
                                tbl_com_master.last_modified_date = DateTime.Now;

                                tbl_company_master tbl_com_master_for_log = _context.tbl_company_master.FirstOrDefault(p => p.company_id == id);

                                _context.tbl_company_master.Update(tbl_com_master);
                                // _context.Entry(tbl_com_master).State = EntityState.Modified;
                                // await _context.SaveChangesAsync();


                                

                                tbl_company_emp_setting tbl_company_emp_setting = (from a in _context.tbl_company_emp_setting select a).Where(x => x.company_id == id && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).FirstOrDefault();
                                if (tbl_company_emp_setting != null)
                                {
                                    tbl_company_emp_setting.number_of_character_for_employee_code = company_master.number_of_character_for_employee_code;
                                    tbl_company_emp_setting.prefix_for_employee_code = company_master.prefix_for_employee_code;
                                    // tbl_company_emp_setting.last_genrated = DateTime.Now;

                                    _context.tbl_company_emp_setting.Update(tbl_company_emp_setting);


                                }

                                await _context.SaveChangesAsync();
                                trans.Commit();

                                objResult.StatusCode = 1;
                                objResult.Message = "Data Updated Successfully...!";

                                return Ok(objResult);
                            }

                        }
                        else
                        {
                            trans.Rollback();
                            objResult.StatusCode = 1;
                            objResult.Message = "Company Not exist";
                            return Ok(objResult);
                        }

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        objResult.Message = ex.Message;
                        objResult.StatusCode = 1;
                        return Ok(objResult);
                    }
                }
            }
            catch (Exception ex)
            {
                objResult.Message = ex.ToString();
                return Ok(objResult);
            }
        }

        // POST: api/apiCompanyMaster    
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.CreateCompany))]
        public async Task<IActionResult> Posttbl_company_master([FromBody] Company company_master)
        {
            ResponseMsg objResult = new ResponseMsg();
            tbl_company_master tbl_company_master = new tbl_company_master();
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        //Check is Company Name Already Exist
                        var CompanyNameExist = (from a in _context.tbl_company_master.Where(x => x.company_name.Trim().ToUpper() == company_master.company_name.Trim().ToUpper()) select a).FirstOrDefault();

                        //Check is Company Code Already Exist
                        var CompanyCodeExist = (from a in _context.tbl_company_master.Where(x => x.company_code.Trim().ToUpper() == company_master.company_code.Trim().ToUpper()) select a).FirstOrDefault();

                        //Check Company Email Already Exist
                        var CompanyEmailExist = (from a in _context.tbl_company_master.Where(x => x.primary_email_id.Trim().ToUpper() == company_master.primary_email_id.Trim().ToUpper()) select a).FirstOrDefault();


                        if (CompanyNameExist != null)
                        {
                            objResult.Message = "Company Name Already Exist...!";
                            objResult.StatusCode = 0;
                            return Ok(objResult);
                        }
                        else if (CompanyCodeExist != null)
                        {
                            objResult.Message = "Company Code Already Exist...!";
                            objResult.StatusCode = 0;
                            return Ok(objResult);
                        }
                        else if (CompanyEmailExist != null)
                        {
                            objResult.Message = "Company Email ID Already Exist...!";
                            objResult.StatusCode = 0;
                            return Ok(objResult);
                        }
                        else
                        {


                            string[] replaceThese = { "data:image/png;base64,", "data:image/jpeg;base64,", "data:image/jpg;base64," };
                            string data = company_master.company_logo;

                            if (data != null && data != "")
                            {
                                foreach (string curr in replaceThese)
                                {
                                    data = data.Replace(curr, string.Empty);
                                }

                                byte[] imageBytes = System.Convert.FromBase64String(data);
                                string imageName = company_master.company_name + ".jpg";

                                var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };

                                var ext = ".jpg"; //getting the extension
                                if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                                {
                                    string name = Path.GetFileNameWithoutExtension(imageName); //getting file name without extension  
                                    string MyFileName = name + ext;

                                    var webRoot = _hostingEnvironment.WebRootPath;

                                    if (!Directory.Exists(webRoot + "/CompanyLogo/" + company_master.company_name + "/"))
                                    {
                                        Directory.CreateDirectory(webRoot + "/CompanyLogo/" + company_master.company_name + "/");
                                    }

                                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/CompanyLogo/" + company_master.company_name + "/", MyFileName);
                                    System.IO.File.WriteAllBytes(path, imageBytes);//save image file
                                                                                   //update file name
                                    tbl_company_master.company_logo = "/CompanyLogo/" + company_master.company_name + "/" + MyFileName;
                                }
                            }


                            //Get Last Company Id
                            var LastCompanyId = (from a in _context.tbl_company_master select a).OrderByDescending(x => x.company_id).First();

                            tbl_company_master.company_code = "C0000" + (LastCompanyId.company_id + 1);
                            tbl_company_master.company_name = company_master.company_name.Trim();
                            tbl_company_master.is_emp_code_manual_genrate = company_master.is_emp_code_manual_genrate;
                            tbl_company_master.address_line_one = company_master.address_line_one;
                            tbl_company_master.address_line_two = company_master.address_line_two;
                            tbl_company_master.pin_code = company_master.pin_code;
                            tbl_company_master.city_id = company_master.city_id;
                            tbl_company_master.state_id = company_master.state_id;
                            tbl_company_master.country_id = company_master.country_id;
                            tbl_company_master.primary_email_id = company_master.primary_email_id;
                            tbl_company_master.secondary_email_id = company_master.secondary_email_id;
                            tbl_company_master.primary_contact_number = company_master.primary_contact_number;
                            tbl_company_master.secondary_contact_number = company_master.secondary_contact_number;
                            tbl_company_master.company_website = company_master.company_website;
                            tbl_company_master.user_type = company_master.user_type;
                            tbl_company_master.is_active = company_master.is_active;
                            tbl_company_master.created_by = _clsCurrentUser.EmpId;
                            tbl_company_master.last_modified_by = 0;
                            tbl_company_master.created_date = DateTime.Now;
                            tbl_company_master.last_modified_date = Convert.ToDateTime("01-01-2000");//DateTime.Now;

                            _context.tbl_company_master.Add(tbl_company_master);
                            await _context.SaveChangesAsync();


                            // company_id
                            var company_id = tbl_company_master.company_id;

                            tbl_company_emp_setting company_sett = new tbl_company_emp_setting();

                            company_sett.company_id = company_id;
                            company_sett.prefix_for_employee_code = company_master.prefix_for_employee_code;
                            company_sett.number_of_character_for_employee_code = company_master.number_of_character_for_employee_code;
                            company_sett.from_range = 0;
                            company_sett.current_range = 0;
                            company_sett.to_range = 10000;
                            company_sett.is_active = 1;
                            company_sett.last_genrated = DateTime.Now;

                            _context.tbl_company_emp_setting.Add(company_sett);
                            await _context.SaveChangesAsync();

#region Get Employee Id And Genrate Employee Code 
                            //Get data from company master
                            var GenrateEmployeeCode = (from a in _context.tbl_company_emp_setting.Where(x => x.is_active == 1 && x.company_id == company_id)
                                                       select new
                                                       {
                                                           a.prefix_for_employee_code,
                                                           a.number_of_character_for_employee_code,
                                                           a.current_range,
                                                           a.from_range,
                                                           a.to_range
                                                       }).ToList();


                            string prefix_for_employee_code = "";
                            int number_of_character_for_employee_code = 0;
                            int current_range = 0;
                            int from_range = 0;
                            int to_range = 0;



                            foreach (var a in GenrateEmployeeCode)
                            {
                                prefix_for_employee_code = a.prefix_for_employee_code;
                                number_of_character_for_employee_code = a.number_of_character_for_employee_code;
                                current_range = a.current_range;
                                from_range = a.from_range;
                                to_range = a.to_range;
                            }

                            //Employee Id
                            int emp_id = current_range + 1;

                            //number of character
                            string TotalChar = prefix_for_employee_code.PadRight(number_of_character_for_employee_code, '0');

                            //Remove Last Character
                            string myString = TotalChar.Substring(0, TotalChar.Length - 1);

                            //Mail Empaloyee Code
                            string EmployeeCode = myString + emp_id;

#endregion

                            // tbl_employee_master
                            tbl_employee_master employee_master = new tbl_employee_master();


                            employee_master.emp_code = EmployeeCode;
                            employee_master.is_active = 1;
                            employee_master.created_by = company_master.created_by;//company_id;
                            employee_master.created_date = DateTime.Now;
                            employee_master.last_modified_by = 0;//company_id;
                            employee_master.last_modified_date = Convert.ToDateTime("01-01-2000"); //DateTime.Now;


                            //Save data in tbl_employee_master
                            _context.tbl_employee_master.Add(employee_master);
                            await _context.SaveChangesAsync();


#region update company setting table 

                            // tbl_company_emp_setting

                            tbl_company_emp_setting tbl_company_emp_setting = (from a in _context.tbl_company_emp_setting select a).Where(x => x.company_id == company_id).First();
                            //Create new obj of tbl_shift_details table for log


                            tbl_company_emp_setting.current_range = emp_id;
                            tbl_company_emp_setting.last_genrated = DateTime.Now;

                            _context.tbl_company_emp_setting.Attach(tbl_company_emp_setting);
                            _context.Entry(tbl_company_emp_setting).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

#endregion


                            var employee_id = employee_master.employee_id;

#region Save Data In User Master
                            tbl_user_master tbl_user_master = new tbl_user_master();
                            tbl_user_master.username = EmployeeCode;
                            tbl_user_master.password = EmployeeCode;
                            //tbl_user_master.user_type = 1;
                            tbl_user_master.is_active = 1;
                            tbl_user_master.created_by = company_master.created_by;//company_id;
                            tbl_user_master.created_date = DateTime.Now;
                            tbl_user_master.last_modified_by = 0; //company_id;
                            tbl_user_master.last_modified_date = Convert.ToDateTime("01-01-2000");//DateTime.Now;                            
                            tbl_user_master.employee_id = employee_id;


                            //Save data in tbl_user_master
                            _context.tbl_user_master.Add(tbl_user_master);
                            await _context.SaveChangesAsync();

#endregion


#region Save Data in Employment Type Master

                            DateTime DurationStartPeriod = Convert.ToDateTime("2018-01-01");
                            DateTime DurationEndPeriod = Convert.ToDateTime("2500-01-01");

                            double DurationDays = 180;

                            for (int i = 1; i < 5; i++)
                            {
                                tbl_employment_type_master tbl_employment_type_master = new tbl_employment_type_master();
                                tbl_employment_type_master.employee_id = employee_id;
                                tbl_employment_type_master.employment_type = Convert.ToByte(i);
                               
                                tbl_employment_type_master.is_deleted = 0;
                                tbl_employment_type_master.created_by = company_master.created_by;
                                tbl_employment_type_master.created_date = DateTime.Now;
                                tbl_employment_type_master.last_modified_by = 0;
                                tbl_employment_type_master.last_modified_date = Convert.ToDateTime("01-01-2000");

                                //Save data in tbl_employment_type_master
                                _context.tbl_employment_type_master.Add(tbl_employment_type_master);
                                await _context.SaveChangesAsync();

                            }

                            tbl_employment_type_master tbl_employment_type_master10 = new tbl_employment_type_master();
                            tbl_employment_type_master10.employee_id = employee_id;
                            tbl_employment_type_master10.employment_type = Convert.ToByte(10);

                           
                            tbl_employment_type_master10.is_deleted = 0;
                            tbl_employment_type_master10.created_by = company_master.created_by; //company_id;
                            tbl_employment_type_master10.created_date = DateTime.Now;
                            tbl_employment_type_master10.last_modified_by = 0; //company_id;
                            tbl_employment_type_master10.last_modified_date = Convert.ToDateTime("01-01-2000");//DateTime.Now;

                            //Save data in tbl_employment_type_master
                            _context.tbl_employment_type_master.Add(tbl_employment_type_master10);
                            await _context.SaveChangesAsync();



                            tbl_employment_type_master objtblemptype_99 = new tbl_employment_type_master();
                            objtblemptype_99.employee_id = employee_id;
                            objtblemptype_99.employment_type = Convert.ToByte(99);
                            
                            objtblemptype_99.is_deleted = 0;
                            objtblemptype_99.created_by = company_master.created_by; //company_id;
                            objtblemptype_99.created_date = DateTime.Now;
                            objtblemptype_99.last_modified_by = 0;//user_master.last_modified_by;
                            objtblemptype_99.last_modified_date = Convert.ToDateTime("01-01-2000");//DateTime.Now;



                            _context.tbl_employment_type_master.Add(objtblemptype_99);
                            await _context.SaveChangesAsync();


                            tbl_employment_type_master tbl_employment_type_master_ = new tbl_employment_type_master();
                            tbl_employment_type_master_.employee_id = employee_id;
                            tbl_employment_type_master_.employment_type = Convert.ToByte(100);

                         
                            tbl_employment_type_master_.is_deleted = 0;
                            tbl_employment_type_master_.created_by = company_master.created_by;//company_id;
                            tbl_employment_type_master_.created_date = DateTime.Now;
                            tbl_employment_type_master_.last_modified_by = 0; //company_id;
                            tbl_employment_type_master_.last_modified_date = Convert.ToDateTime("01-01-2000");//DateTime.Now;

                            //Save data in tbl_employment_type_master
                            _context.tbl_employment_type_master.Add(tbl_employment_type_master_);
                            await _context.SaveChangesAsync();
#endregion


#region Save Data In Employee Officaial Sec
                            // tbl_emp_officaial_sec
                            tbl_emp_officaial_sec emp_officaial_sec = new tbl_emp_officaial_sec();
                            emp_officaial_sec.employee_id = employee_id;
                            emp_officaial_sec.card_number = "0";
                            emp_officaial_sec.employee_first_name = "Default";
                            emp_officaial_sec.employee_middle_name = "User";
                            //Save data in tbl_user_master
                            _context.tbl_emp_officaial_sec.Add(emp_officaial_sec);
                            await _context.SaveChangesAsync();

#endregion


#region Save Data in emp company maping 
                            //tbl_employee_company_map
                            tbl_employee_company_map tbl_emp_comp_map = new tbl_employee_company_map();
                            tbl_emp_comp_map.employee_id = employee_id;
                            tbl_emp_comp_map.company_id = company_id;
                            tbl_emp_comp_map.is_deleted = 0;
                            tbl_emp_comp_map.created_by = company_master.created_by; //company_id;
                            tbl_emp_comp_map.last_modified_by = 0;//company_id;
                            tbl_emp_comp_map.created_date = DateTime.Now;
                            tbl_emp_comp_map.last_modified_date = Convert.ToDateTime("01-01-2000");//DateTime.Now;

                            //Save data in tbl_employee_company_map
                            _context.tbl_employee_company_map.Add(tbl_emp_comp_map);
                            await _context.SaveChangesAsync();
#endregion


#region Save data in emp desig allocation
                            tbl_emp_desi_allocation emp_desi_alloc = new tbl_emp_desi_allocation();
                            emp_desi_alloc.employee_id = employee_id;
                            emp_desi_alloc.applicable_from_date = DateTime.Now;
                            emp_desi_alloc.applicable_to_date = Convert.ToDateTime("2500-01-01");

                            //Save data in tbl_emp_desi_allocation
                            _context.tbl_emp_desi_allocation.Add(emp_desi_alloc);
                            await _context.SaveChangesAsync();

#endregion


#region Save Data in emp_manager
                            tbl_emp_manager emp_manager = new tbl_emp_manager();
                            emp_manager.employee_id = employee_id;
                            emp_manager.applicable_from_date = DateTime.Now;                            
                            emp_manager.is_deleted = 0;
                            //Save data in tbl_emp_managers
                            _context.tbl_emp_manager.Add(emp_manager);
                            await _context.SaveChangesAsync();
#endregion


#region Save Emp Grade Allocation
                            tbl_emp_grade_allocation emp_grade_allocation = new tbl_emp_grade_allocation();
                            emp_grade_allocation.employee_id = employee_id;
                            emp_grade_allocation.applicable_from_date = DateTime.Now;
                            emp_grade_allocation.applicable_to_date = Convert.ToDateTime("2500-01-01");


                            //Save data in tbl_emp_managers
                            _context.tbl_emp_grade_allocation.Add(emp_grade_allocation);
                            await _context.SaveChangesAsync();

#endregion


#region Save Emp Personal Sec
                            tbl_emp_personal_sec emp_personal_sec = new tbl_emp_personal_sec();
                            emp_personal_sec.employee_id = employee_id;
                            emp_personal_sec.blood_group = 0;
                            emp_personal_sec.permanent_pin_code = 0;
                            emp_personal_sec.permanent_city = 0;
                            emp_personal_sec.permanent_state = 0;
                            emp_personal_sec.permanent_city = 0;
                            emp_personal_sec.corresponding_pin_code = 0;
                            emp_personal_sec.corresponding_city = 0;
                            emp_personal_sec.corresponding_state = 0;
                            emp_personal_sec.corresponding_country = 0;
                            emp_personal_sec.is_emg_same_as_permanent = 0;
                            emp_personal_sec.emergency_contact_pin_code = 0;
                            emp_personal_sec.emergency_contact_city = 0;
                            emp_personal_sec.emergency_contact_state = 0;
                            emp_personal_sec.emergency_contact_country = 0;
                            emp_personal_sec.is_deleted = 0;
                            emp_personal_sec.created_by = company_master.created_by; //company_id;
                            emp_personal_sec.last_modified_by = 0; //company_id;
                            emp_personal_sec.created_date = DateTime.Now;
                            emp_personal_sec.last_modified_date = Convert.ToDateTime("01-01-2000"); //DateTime.Now;

                            //Save data in tbl_emp_personal_sec
                            _context.tbl_emp_personal_sec.Add(emp_personal_sec);
                            await _context.SaveChangesAsync();

#endregion

                            objResult.StatusCode = 1;
                            objResult.Message = "Company Added Successfully...!";
                            trans.Commit();
                            return Ok(objResult);


                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        objResult.Message = ex.Message;
                        objResult.StatusCode = 1;
                        return Ok(objResult);
                    }
                }
            }
            catch (Exception ex)
            {
                objResult.Message = ex.ToString();
                return Ok(objResult);
            }

        }

        // DELETE: api/apiCompanyMaster/5
        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(enmMenuMaster.CreateCompany))]
        public async Task<IActionResult> Deletetbl_company_master([FromRoute] int id)
        {
            ResponseMsg objResult = new ResponseMsg();

            try
            {

                //Create new obj of tbl_company_master table
                tbl_company_master tbl_com_master = _context.tbl_company_master.FirstOrDefault(p => p.company_id == id && _clsCurrentUser.CompanyId.Contains(p.company_id));

                // Is Active 3 for delete
                tbl_com_master.is_active = 3;

                _context.tbl_company_master.Attach(tbl_com_master);
                _context.Entry(tbl_com_master).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                objResult.StatusCode = 1;
                objResult.Message = "Data Deleted Successfully...!";

                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.Message = ex.ToString();
                return Ok(objResult);
            }
        }





    }
#endif
}
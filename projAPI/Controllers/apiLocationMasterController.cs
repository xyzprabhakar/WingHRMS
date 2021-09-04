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
    [Route("api/[controller]")]
    [ApiController]
    public class apiLocationMasterController : ControllerBase
    {
        private readonly Context _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly clsCompanyDetails _clsCompany;
        private readonly clsCurrentUser _clsCurrentUser;


        //public apiLocationMasterController(Context context)
        //{
        //    _context = context;
        //}
        public apiLocationMasterController(Context context, IHostingEnvironment environment,clsCompanyDetails _clsCompany,clsCurrentUser _clsCurrentUser)
        {
            _context = context;
            _hostingEnvironment = environment;
            this._clsCompany = _clsCompany;
            this._clsCurrentUser = _clsCurrentUser;
        }

        // GET: api/apiLocationMaster
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LocationReport))]
        public IActionResult Gettbl_location_master()
        {
            var data = _clsCompany.GetLocations();



            return Ok(data);//_context.tbl_location_master;
        }

        // GET: api/apiLocationMaster/5
        [HttpGet("{locationid}")]        ////[Authorize]
        //No Need of autrozie police
        public async Task<IActionResult> Gettbl_location_master([FromRoute] int locationid)
        {
            Response_Msg objResult = new Response_Msg();
            try
            {
                if (locationid > 0)
                {
                   var data= _context.tbl_location_master.Find(locationid);

                    return Ok(data);
                }
                else
                {
                    var data = (from a in _context.tbl_location_master
                                join b in _context.tbl_state on a.state_id equals b.state_id
                                join c in _context.tbl_city on a.city_id equals c.city_id
                                join d in _context.tbl_company_master on a.company_id equals d.company_id
                                select new
                                {
                                    sno = a.location_id,
                                    locationid = a.location_id,
                                    locationcode = a.location_code,
                                    locationname = a.location_name,
                                    companyname = d.company_name,
                                    statename = b.name,
                                    cityname = c.name,
                                    createdon = a.created_date,
                                    status = a.is_active,
                                    companyid = a.company_id,
                                    locationtype = a.type_of_location,
                                    address1 = a.address_line_one,
                                    address2 = a.address_line_two,
                                    pincode = a.pin_code,
                                    countryid = a.country_id,
                                    stateid = a.state_id,
                                    cityid = a.city_id,
                                    primaryemail = a.primary_email_id,
                                    secondemail = a.secondary_email_id,
                                    contact1 = a.primary_contact_number,
                                    contact2 = a.secondary_contact_number,
                                    website = a.website

                                }).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        // Post: api/apiLocationMaster/5
        [Route("Puttbl_location_master")]
        [HttpPost]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.CreateLocation))]
        public async Task<IActionResult> Puttbl_location_master([FromBody] clsLocationMaster objlocation)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.DownlineEmpId.Contains(objlocation.created_by))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access";
                return Ok(objResult);
            }

            if (!_clsCurrentUser.CompanyId.Contains(objlocation.company_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access....!!";
                return Ok(objResult);
            }

            tbl_location_master Obj_tbl_locat = new tbl_location_master();
            try
            {
                //check if exist
                var check_location = (from a in _context.tbl_location_master.Where(x => x.location_id != objlocation.location_id && x.location_name.Trim().ToUpper() == objlocation.location_name.Trim().ToUpper() && x.company_id == objlocation.company_id ) select a).FirstOrDefault();


                if (check_location != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Location already exist in the company !!";
                    return Ok(objResult);
                }
                else
                {
                    //check if exist
                    var Exist = (from a in _context.tbl_location_master.Where(x => x.location_id == objlocation.location_id) select a).FirstOrDefault();

                    if (Exist == null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Location id not exist please try again !!";
                        return Ok(objResult);
                    }
                    else if (objlocation.type_of_location.Equals((int)CommonClass.LocationType.HeadOffice) && _context.tbl_location_master.Where(p => p.company_id == objlocation.company_id && p.type_of_location == ((byte)CommonClass.LocationType.HeadOffice) && p.is_active == 1).FirstOrDefault() != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Head Office is already define !!";
                        return Ok(objResult);
                    }
                    else
                    {
                        //get count and genrate location code
                        var LocCount = _context.tbl_location_master.Count();
                        LocCount = LocCount == 0 ? 1 : LocCount + 1;
                        string locationcode = "";
                        string str = objlocation.location_name;
                        str.Split(' ').ToList().ForEach(i => locationcode = locationcode + i[0]);

                        locationcode = locationcode.ToUpper() + LocCount.ToString("000");

                        string[] replaceThese = { "data:image/png;base64,", "data:image/jpeg;base64,", "data:image/jpg;base64," };
                        string data = objlocation.image;


                        if (data != null && data != "")
                        {
                            foreach (string curr in replaceThese)
                            {
                                data = data.Replace(curr, string.Empty);
                            }

                            byte[] imageBytes = System.Convert.FromBase64String(data);
                            string imageName = objlocation.location_name + ".jpg";

                            var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };

                            var ext = ".jpg"; //getting the extension
                            if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                            {
                                string name = Path.GetFileNameWithoutExtension(imageName); //getting file name without extension  
                                string MyFileName = name + ext;

                                var webRoot = _hostingEnvironment.WebRootPath;

                                if (!Directory.Exists(webRoot + "/UserFiles/" + objlocation.location_name + "/Image/"))
                                {
                                    Directory.CreateDirectory(webRoot + "/UserFiles/" + objlocation.location_name + "/Image/");
                                }
                                //Delete User Files if exist
                                //var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/UserFiles/" + objlocation.location_name + "/Image/" + MyFileName);
                                //Directory.Delete(dirPath, true);

                                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/UserFiles/" + objlocation.location_name + "/Image/", MyFileName);
                                System.IO.File.WriteAllBytes(path, imageBytes);//save image file
                                                                               //update file name
                                objlocation.image = MyFileName;
                            }
                        }
                        //save data
                        Exist.location_code = locationcode;
                        Exist.location_name = objlocation.location_name;
                        Exist.type_of_location = Convert.ToByte(objlocation.type_of_location);
                        Exist.company_id = objlocation.company_id;
                        Exist.country_id = Convert.ToInt32(objlocation.country);
                        Exist.state_id = Convert.ToInt32(objlocation.state);
                        Exist.city_id = Convert.ToInt32(objlocation.city);
                        Exist.pin_code = Convert.ToInt32(objlocation.pin_code);
                        Exist.primary_contact_number = objlocation.primary_contact_number;
                        Exist.secondary_contact_number = objlocation.secondary_contact_number;
                        Exist.address_line_one = objlocation.address_line_one;
                        Exist.address_line_two = objlocation.address_line_two;
                        Exist.primary_email_id = objlocation.primary_email_id;
                        Exist.secondary_email_id = objlocation.secondary_email_id;
                        Exist.website = objlocation.website;
                        Exist.image = objlocation.image;

                        //added new logic
                        Exist.is_active = Convert.ToInt32(objlocation.is_active);
                        Exist.last_modified_by = _clsCurrentUser.EmpId;
                        Exist.last_modified_date = DateTime.Now;

                        _context.Entry(Exist).State = EntityState.Modified;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "Location updated successfully !!";
                        return Ok(objResult);
                    }

                }
            }

            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        // POST: api/apiLocationMaster
        //[Route("tbl_location_master")]
        [HttpPost]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.CreateLocation))]
        public async Task<IActionResult> Posttbl_location_master([FromBody] clsLocationMaster objlocation)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(objlocation.company_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access....!!";
                return Ok(objResult);
            }

            if (!_clsCurrentUser.DownlineEmpId.Contains(objlocation.created_by))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access";
            }

            tbl_location_master Obj_tbl_locat = new tbl_location_master();
            try
            {
                //check if exist
                var Exist = (from a in _context.tbl_location_master.Where(x => x.location_name.Trim().ToUpper() == objlocation.location_name.Trim().ToUpper() && x.company_id == objlocation.company_id) select a).FirstOrDefault();

                if (Exist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Location already exist in the company !!";
                    return Ok(objResult);
                }
                else
                {
                    //get count and genrate location code
                    var LocCount = _context.tbl_location_master.Count();
                    LocCount = LocCount == 0 ? 1 : LocCount + 1;
                    string locationcode = "";
                    string str = objlocation.location_name;
                    str.Split(' ').ToList().ForEach(i => locationcode = locationcode + i[0]);

                    locationcode = locationcode.ToUpper() + LocCount.ToString("000");



                    string[] replaceThese = { "data:image/png;base64,", "data:image/jpeg;base64,", "data:image/jpg;base64," };
                    string data = objlocation.image;


                    if (data != null && data != "")
                    {
                        foreach (string curr in replaceThese)
                        {
                            data = data.Replace(curr, string.Empty);
                        }

                        byte[] imageBytes = System.Convert.FromBase64String(data);
                        string imageName = objlocation.location_name + ".jpg";

                        var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };

                        var ext = ".jpg"; //getting the extension
                        if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                        {
                            string name = Path.GetFileNameWithoutExtension(imageName); //getting file name without extension  
                            string MyFileName = name + ext;

                            var webRoot = _hostingEnvironment.WebRootPath;

                            if (!Directory.Exists(webRoot + "/UserFiles/" + objlocation.location_name + "/Image/"))
                            {
                                Directory.CreateDirectory(webRoot + "/UserFiles/" + objlocation.location_name + "/Image/");
                            }
                            //Delete User Files if exist
                            //var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/UserFiles/" + objlocation.location_name + "/Image/" + MyFileName);
                            //Directory.Delete(dirPath, true);

                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/UserFiles/" + objlocation.location_name + "/Image/", MyFileName);
                            System.IO.File.WriteAllBytes(path, imageBytes);//save image file
                                                                           //update file name
                            objlocation.image = MyFileName;
                        }
                    }
                    //save data
                    Obj_tbl_locat.location_code = locationcode;
                    Obj_tbl_locat.location_name = objlocation.location_name;
                    Obj_tbl_locat.type_of_location = Convert.ToByte(objlocation.type_of_location);
                    Obj_tbl_locat.company_id = objlocation.company_id;
                    Obj_tbl_locat.country_id = Convert.ToInt32(objlocation.country);
                    Obj_tbl_locat.state_id = Convert.ToInt32(objlocation.state);
                    Obj_tbl_locat.city_id = Convert.ToInt32(objlocation.city);
                    Obj_tbl_locat.pin_code = Convert.ToInt32(objlocation.pin_code);
                    Obj_tbl_locat.primary_contact_number = objlocation.primary_contact_number;
                    Obj_tbl_locat.secondary_contact_number = objlocation.secondary_contact_number;
                    Obj_tbl_locat.address_line_one = objlocation.address_line_one;
                    Obj_tbl_locat.address_line_two = objlocation.address_line_two;
                    Obj_tbl_locat.primary_email_id = objlocation.primary_email_id;
                    Obj_tbl_locat.secondary_email_id = objlocation.secondary_email_id;
                    Obj_tbl_locat.website = objlocation.website;
                    Obj_tbl_locat.image = objlocation.image;
                    Obj_tbl_locat.is_active = Convert.ToInt32(objlocation.is_active);
                    Obj_tbl_locat.created_by = objlocation.created_by;
                    Obj_tbl_locat.created_date = DateTime.Now;

                    _context.Entry(Obj_tbl_locat).State = EntityState.Added;
                    _context.SaveChanges();

                    ////Start Sub Location Enter for NA - Added by Amarjeet : Date - 21-09-2020 ////////////////////////////

                    var location_id = Obj_tbl_locat.location_id;


                    tbl_sub_location_master sub_location = new tbl_sub_location_master();
                    sub_location.location_name = "NA";
                    sub_location.location_id = location_id;
                    sub_location.is_active = 1;
                    sub_location.created_by = 1;
                    sub_location.created_date = DateTime.Now;
                    sub_location.last_modified_by = 1;
                    sub_location.last_modified_date = DateTime.Now;

                    _context.Entry(sub_location).State = EntityState.Added;
                    _context.SaveChanges();

                    /////////////////////////////End Sub Department Enter for NA  ///////////////////////////////////////////

                    objResult.StatusCode = 0;
                    objResult.Message = "Location save successfully !!";
                    return Ok(objResult);
                }
            }

            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
           
        }
      

        // DELETE: api/apiLocationMaster/5
        [HttpDelete("{id}")]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.CreateLocation))]
        public async Task<IActionResult> Deletetbl_location_master([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tbl_location_master = await _context.tbl_location_master.FindAsync(id);
            if (tbl_location_master == null)
            {
                return NotFound();
            }

            _context.tbl_location_master.Remove(tbl_location_master);
            await _context.SaveChangesAsync();

            return Ok(tbl_location_master);
        }

        private bool tbl_location_masterExists(int id)
        {
            return _context.tbl_location_master.Any(e => e.location_id == id);
        }

        #region ** START BY SUPRIYA ON 30-07-2019
        [HttpGet("Gettbl_location_masterByCompID/{company_id}/{fromdate}/{todate}/{location_type}")]    
        [Authorize(Policy =nameof(enmMenuMaster.LocationReport))]
        public async Task<IActionResult> Gettbl_location_masterByCompID([FromRoute] int company_id,DateTime fromdate,DateTime todate,int location_type)
        {
            Response_Msg objResult = new Response_Msg();
            try
            {
                if (company_id < 0)// all company
                {
                    if (location_type>0)
                    {
                        var data = _clsCompany.GetLocations().Where(x => x.location_type_id == location_type && fromdate.Date <= x.created_on.Date && x.created_on.Date <= todate.Date).ToList();
                        

                        return Ok(data);
                    }
                    else
                    { //// all location type
                        var data = _clsCompany.GetLocations().Where(x => fromdate.Date <= x.created_on.Date && x.created_on.Date <= todate.Date);
                        return Ok(data);
                    }
                }
                else //selected company
                {
                    if (location_type>0)
                    {
                        var data = _clsCompany.GetLocations().Where(x => x.company_id == company_id && x.location_type_id == location_type && fromdate.Date <= x.created_on.Date && x.created_on.Date <= todate.Date).ToList();
                        
                        return Ok(data);
                    }
                    else
                    {//// all location type
                        var data = _clsCompany.GetLocations().Where(x => x.company_id == company_id && fromdate.Date<=x.created_on.Date && x.created_on.Date<=todate.Date).ToList();
                        return Ok(data);
                    }
                }
               
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #endregion ** END BY SUPRIYA ON 30-07-2019
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using projAPI.Classes;
using projAPI.Model;
using projContext;
using projContext.DB;
using static projContext.CommonClass;
using QRCoder;
using System.Drawing;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Text.RegularExpressions;

namespace projAPI.Controllers
{
    //created by : vibhav kant
    //created date :
    [Route("api/[controller]")]
    [ApiController]
    public class apiMastersController : ControllerBase
    {
        private readonly Context _context;
        private readonly IOptions<AppSettings> _appSettings;
        private IHostingEnvironment _hostingEnvironment;
        private readonly string _img_file_path = @"\\192.168.10.129\wwwroot\Document\mobile_app_test";// just for testing for mobile app - Image upload
        private readonly IHttpContextAccessor _AC;
        //private readonly IConfiguration _config;
        private IConverter _converter;
        IConfiguration _config;
        clsCurrentUser _clsCurrentUser;
        clsEmployeeDetail _clsEmployeeDetail;
        private clsCompanyDetails _clsCompanyDetails;
        public apiMastersController(Context context, IOptions<AppSettings> appSettings, IHostingEnvironment environment, IHttpContextAccessor AC, IConfiguration config, clsCurrentUser _clsCurrentUser, clsEmployeeDetail _clsEmployeeDetail, clsCompanyDetails _clsCompanyDetails)
        {
            _context = context;
            _hostingEnvironment = environment;
            _appSettings = appSettings;
            _AC = AC;
            _config = config;
            this._clsCurrentUser = _clsCurrentUser;
            this._clsEmployeeDetail = _clsEmployeeDetail;
            this._clsCompanyDetails = _clsCompanyDetails;
        }

        // GET: api/apiMasters
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IEnumerable<tbl_country> Gettbl_country()
        {
            return _context.tbl_country;
        }

        // GET: api/apiMasters/5
        [Route("GetCountryList/{CountryId}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public async Task<IActionResult> GetCountryList([FromRoute] int CountryId)
        {
            try
            {
                if (CountryId > 0)
                {
                    //var result = (from a in _context.tbl_country.Where(x => x.country_id == CountryId) select a).ToList();
                    var result = _context.tbl_country.Where(x => x.country_id == CountryId && x.is_deleted == 0).ToList();
                    return Ok(result);
                }
                else
                {
                    //var result = (from a in _context.tbl_country select a ).ToList();
                    var result = _context.tbl_country.Where(x => x.is_deleted == 0).ToList();
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [Route("SaveCountry")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Country))]
        public IActionResult SaveCountry([FromBody] clsCountry ObjCountry)
        {

            Response_Msg objResult = new Response_Msg();
            try
            {
                var Exist = (from a in _context.tbl_country.Where(x => x.name.Trim().ToUpper() == ObjCountry.CountryName.Trim().ToUpper() || x.name.Trim().ToUpper() == ObjCountry.ShortName.Trim().ToUpper() || x.sort_name.Trim().ToUpper() == ObjCountry.ShortName.Trim().ToUpper() || x.sort_name.Trim().ToUpper() == ObjCountry.CountryName.Trim().ToUpper()) select a).FirstOrDefault();

                if (Exist == null)
                {
                    //save
                    tbl_country objcountry = new tbl_country();
                    objcountry.name = ObjCountry.CountryName;
                    objcountry.sort_name = ObjCountry.ShortName;
                    objcountry.created_by = ObjCountry.created_by;
                    objcountry.created_date = DateTime.Now;
                    _context.Entry(objcountry).State = EntityState.Added;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Country Added Successfully !";

                    return Ok(objResult);
                }
                else
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Country Name or short name Already Exist !!";

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
        [Route("UpdateCountry")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Country))]
        public IActionResult UpdateCountry([FromBody] clsCountry ObjCountry)
        {

            Response_Msg objResult = new Response_Msg();
            try
            {
                var Exist = (from a in _context.tbl_country.Where(x => x.country_id == ObjCountry.CountryId) select a).FirstOrDefault();

                if (Exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Country does not exist, please check and try !!";

                }
                else
                {
                    var duplicate = _context.tbl_country.Where(x => x.country_id != ObjCountry.CountryId && (x.name.Trim().ToUpper() == ObjCountry.CountryName.Trim().ToUpper() || x.name.Trim().ToUpper() == ObjCountry.ShortName.Trim().ToUpper() || x.sort_name.Trim().ToUpper() == ObjCountry.ShortName.Trim().ToUpper() || x.sort_name.Trim().ToUpper() == ObjCountry.CountryName.Trim().ToUpper())).ToList();
                    if (duplicate.Count > 0)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Country Name or short name already exist";
                    }
                    else
                    {
                        Exist.name = ObjCountry.CountryName;
                        Exist.sort_name = ObjCountry.ShortName;
                        Exist.last_modified_by = ObjCountry.created_by;
                        Exist.last_modified_date = DateTime.Now;
                        _context.tbl_country.Add(Exist).State = EntityState.Modified;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "Country Updated Successfully !";

                    }

                    //return Ok(objResult);

                }

                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;

                return Ok(objResult);
            }
        }

        [Route("SaveAndUpdateState")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.State))]
        public IActionResult SaveAndUpdateState([FromBody] clsState ObjState)
        {

            Response_Msg objResult = new Response_Msg();
            try
            {
                //add new state
                if (ObjState.ActionMode == "Add")
                {
                    var Exist = (from a in _context.tbl_state.Where(x => x.name.Trim().ToUpper() == ObjState.StateName.Trim().ToUpper() && x.is_deleted == 0) select a).FirstOrDefault();

                    if (Exist == null)
                    {
                        //save
                        tbl_state objstate = new tbl_state();
                        objstate.name = ObjState.StateName;
                        objstate.country_id = ObjState.CountryId;
                        objstate.created_by = ObjState.created_by;
                        objstate.created_date = DateTime.Now;
                        _context.tbl_state.Add(objstate).State = EntityState.Added;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "State Added Successfully !";

                        return Ok(objResult);
                    }
                    else
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "State Name Already Exist !!";

                        return Ok(objResult);
                    }
                }
                else //update state data
                {
                    var Exist = (from a in _context.tbl_state.Where(x => x.state_id == ObjState.StateId && x.is_deleted == 0) select a).FirstOrDefault();

                    if (Exist == null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "State does not eixst, please check and try !!";

                        return Ok(objResult);

                    }
                    else
                    {
                        //check if same state name exist in same country more than 1
                        var StateCount = 0;
                        try
                        {
                            StateCount = _context.tbl_state.Where(x => x.state_id != ObjState.StateId && x.name.Trim().ToUpper() == ObjState.StateName.Trim().ToUpper() && x.country_id == ObjState.CountryId).Count();
                        }
                        catch
                        {

                        }

                        if (StateCount > 0)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "State name already exist in country, please check and try !!";

                            return Ok(objResult);
                        }
                        else
                        {

                            tbl_state objstate = new tbl_state();
                            Exist.name = ObjState.StateName;
                            Exist.country_id = ObjState.CountryId;
                            Exist.last_modified_by = ObjState.created_by;
                            Exist.last_modified_date = DateTime.Now;
                            _context.tbl_state.Add(Exist).State = EntityState.Modified;
                            _context.SaveChanges();

                            objResult.StatusCode = 0;
                            objResult.Message = "State Updated Successfully !";

                            return Ok(objResult);
                        }

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
        // GET: api/apiMasters/GetStateList/0/5
        [Route("GetStateList/{StateId}/{CountryId}")]
        [HttpGet]
        ////[Authorize(Policy ="",Roles ="")]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public async Task<IActionResult> GetStateList([FromRoute] int StateId, int CountryId)
        {
            try
            {
                if (StateId > 0 && CountryId == 0)
                {
                    var result = (from a in _context.tbl_state.Where(x => x.state_id == StateId && x.is_deleted == 0) select a).ToList();

                    return Ok(result);
                }
                else if (StateId == 0 && CountryId == -1)
                {

                    var result = _context.tbl_state.Where(a => a.is_deleted == 0).Select(p => new
                    {
                        stateid = p.state_id,
                        statename = p.name,
                        countryid = p.tbl_country.country_id,
                        countryname = p.tbl_country.name,
                        shortname = p.tbl_country.sort_name
                    }).ToList();

                    //var result = (from a in _context.tbl_state
                    //              join b in _context.tbl_country on a.country_id equals b.country_id
                    //              select new
                    //              {
                    //                  stateid = a.state_id,
                    //                  statename = a.name,
                    //                  countryname = b.name,
                    //                  shortname = b.sort_name.ToUpper()
                    //              }).ToList();

                    return Ok(result);
                }
                else
                {
                    var result = (from a in _context.tbl_state.Where(x => x.country_id == CountryId && x.is_deleted == 0) select a).ToList();

                    return Ok(result);
                }



            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("SaveAndUpdateCity")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.City))]
        public IActionResult SaveAndUpdateCity([FromBody] clsCity ObjCity)
        {

            Response_Msg objResult = new Response_Msg();
            bool _existcityname = false;
            try
            {
                //add new city
                if (ObjCity.ActionMode == "Add")
                {
                    var Exist = (from a in _context.tbl_city.Where(x => x.name.Trim().ToUpper() == ObjCity.CityName.Trim().ToUpper() && x.is_deleted == 0) select a).FirstOrDefault();

                    if (Exist == null)
                    {
                        //save
                        tbl_city objcity = new tbl_city();
                        objcity.name = ObjCity.CityName;
                        objcity.state_id = ObjCity.StateId;
                        objcity.created_by = ObjCity.created_by;
                        objcity.created_date = DateTime.Now;
                        _context.tbl_city.Add(objcity).State = EntityState.Added;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "City Added Successfully !";

                        return Ok(objResult);
                    }
                    else
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "City Name Already Exist !!";

                        return Ok(objResult);
                    }
                }
                else //update city data
                {
                    var Exist = (from a in _context.tbl_city.Where(x => x.city_id == ObjCity.CityId && x.is_deleted == 0) select a).FirstOrDefault();

                    if (Exist == null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "City does not eixst, please check and try !!";

                        return Ok(objResult);

                    }
                    else
                    {
                        //check if same state name exist in same country more than 1
                        var CityCount = (from a in _context.tbl_city.Where(x => x.city_id != ObjCity.CityId && x.name == ObjCity.CityName && x.state_id == ObjCity.StateId) select a).Count();
                        // var CityCount = _context.tbl_city.OrderByDescending(y => y.city_id).Where(x => x.city_id == ObjCity.CityId).ToList();
                        if (CityCount > 0)
                        {
                            //var _citynamelist = _context.tbl_city.OrderByDescending(x => x.city_id).Where(x => x.state_id == ObjCity.StateId).ToList();
                            //foreach (var item in _citynamelist)
                            //{
                            //    if (ObjCity.CityName.Trim().ToUpper() == item.name.Trim().ToUpper())
                            //    {
                            //        _existcityname = true;
                            //        break;
                            //    }

                            //}

                            //if (_existcityname)
                            //{
                            //    objResult.StatusCode = 1;
                            //    objResult.Message = "City Already Exist..";
                            //}
                            objResult.StatusCode = 1;
                            objResult.Message = "City name already exist in this State, please check and try !!";
                            return Ok(objResult);
                        }
                        else
                        {
                            Exist.name = ObjCity.CityName;
                            Exist.state_id = ObjCity.StateId;
                            Exist.last_modified_by = ObjCity.created_by;
                            Exist.last_modified_date = DateTime.Now;
                            _context.tbl_city.Add(Exist).State = EntityState.Modified;
                            _context.SaveChanges();


                            objResult.StatusCode = 0;
                            objResult.Message = "City Successfully Updated";

                        }
                        //}
                        //else
                        //{
                        //    objResult.StatusCode = 1;
                        //    objResult.Message = "Invalid City ID..";
                        //}

                        return Ok(objResult);

                        //if (CityCount > 0)
                        //{
                        //    objResult.StatusCode = 1;
                        //    objResult.Message = "City name already exist in state, please check and try !!";

                        //    return Ok(objResult);
                        //}
                        //else
                        //{
                        //    Exist.name = ObjCity.CityName;
                        //    Exist.state_id = ObjCity.StateId;
                        //    _context.tbl_city.Add(Exist).State = EntityState.Modified;
                        //    _context.SaveChanges();

                        //    objResult.StatusCode = 0;
                        //    objResult.Message = "City Updated Successfully !";

                        //    return Ok(objResult);
                        //}

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

        // GET: api/apiMasters/GetStateList/0/5
        [Route("GetCityList/{StateId}/{CityId}")]
        [HttpGet]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public async Task<IActionResult> GetCityList([FromRoute] int StateId, int CityId)
        {
            try
            {
                if (CityId > 0 && StateId == 0)
                {
                    //var result = (from a in _context.tbl_city.Where(x => x.city_id == CityId) select a).ToList();
                    var result = _context.tbl_city.Where(x => x.city_id == CityId && x.is_deleted == 0).Select(p => new
                    {
                        city_id = p.city_id,
                        name = p.name,
                        state_id = p.state_id,
                        state_name = p.tbl_state.name,
                        country_id = p.tbl_state.country_id,
                        country_name = p.tbl_state.tbl_country.name
                    }).ToList();

                    return Ok(result);
                }
                else if (StateId == 0 && CityId == -1)
                {
                    var result = (from a in _context.tbl_city
                                  join b in _context.tbl_state on a.state_id equals b.state_id
                                  join c in _context.tbl_country on
                                  b.country_id equals c.country_id
                                  where a.is_deleted == 0 && b.is_deleted == 0
                                  select new
                                  {
                                      cityid = a.city_id,
                                      cityname = a.name,
                                      stateid = a.state_id,
                                      statename = b.name,
                                      countryname = c.name,
                                      pincode = a.pincode,
                                      shortname = c.sort_name.ToUpper()
                                  }).ToList();

                    return Ok(result);
                }
                else
                {
                    var result = (from a in _context.tbl_city.Where(x => x.state_id == StateId && x.is_deleted == 0) select a).ToList();
                    return Ok(result);
                }



            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("GetLocationType")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult GetLocationType()
        {
            try
            {
                List<object> list = new List<object>();
                foreach (LocationType location in Enum.GetValues(typeof(LocationType)))
                {
                    int value = (int)Enum.Parse(typeof(LocationType), Enum.GetName(typeof(LocationType), location));
                    // string Str = Enum.GetName(typeof(LocationType), value);

                    Type type = location.GetType();
                    MemberInfo[] memInfo = type.GetMember(location.ToString());

                    if (memInfo != null && memInfo.Length > 0)
                    {
                        object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (attrs != null && attrs.Length > 0)
                        {
                            string strvalue = ((DescriptionAttribute)attrs[0]).Description;
                            list.Add(new
                            {
                                LocationId = value,
                                LocationName = strvalue
                            });
                        }
                    }
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        // PUT: api/apiMasters/5
        [HttpPost("{id}")]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.Country))]
        public async Task<IActionResult> Puttbl_country([FromRoute] int id, [FromBody] tbl_country tbl_country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_country.country_id)
            {
                return BadRequest();
            }

            _context.Entry(tbl_country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_countryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/apiMasters
        [HttpPost]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.Country))]
        public async Task<IActionResult> Posttbl_country([FromBody] tbl_country tbl_country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.tbl_country.Add(tbl_country);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Gettbl_country", new { id = tbl_country.country_id }, tbl_country);
        }

        // DELETE: api/apiMasters/5
        [HttpDelete("{id}")]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.Country))]
        public async Task<IActionResult> Deletetbl_country([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tbl_country = await _context.tbl_country.FindAsync(id);
            if (tbl_country == null)
            {
                return NotFound();
            }

            _context.tbl_country.Remove(tbl_country);
            await _context.SaveChangesAsync();

            return Ok(tbl_country);
        }

        private bool tbl_countryExists(int id)
        {
            return _context.tbl_country.Any(e => e.country_id == id);
        }

        //save grade master
        [Route("Save_GradeMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Grade))]
        public IActionResult Save_GradeMaster([FromBody] tbl_grade_master objgradem)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.DownlineEmpId.Contains(objgradem.created_by))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access";
                return Ok(objResult);
            }
            tbl_grade_master_log objlog = new tbl_grade_master_log();
            try
            {
                var exist = _context.tbl_grade_master.Where(x => x.grade_name.Trim().ToUpper() == objgradem.grade_name.Trim().ToUpper()).FirstOrDefault();

                if (exist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Grade name already exist !!";
                    return Ok(objResult);
                }
                else
                {
                    objgradem.is_active = Convert.ToInt32(objgradem.is_active);
                    objgradem.created_by = _clsCurrentUser.EmpId;
                    objgradem.created_date = DateTime.Now;

                    _context.Entry(objgradem).State = EntityState.Added;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Grade name save successfully !!";
                    return Ok(objResult);
                }

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        //get grade master data
        [Route("Get_GradeMasterData/{gradeid}")]
        [HttpGet]
        //[Authorize(Policy = "7014")]
        public IActionResult Get_GradeMasterData([FromRoute] int gradeid)
        {
            try
            {
                if (gradeid > 0)
                {
                    var data = _context.tbl_grade_master.Where(x => x.grade_id == gradeid).FirstOrDefault();
                    return Ok(data);
                }
                else
                {
                    var data = _context.tbl_grade_master.AsEnumerable().Select((a, index) => new
                    {
                        gradeid = a.grade_id,
                        gradename = a.grade_name,
                        status = a.is_active,
                        createdby = a.created_by,
                        createdon = a.created_date,
                        modifiedon = a.last_modified_date,
                        sno = index + 1
                    }).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //update grade master
        [Route("Update_GradeMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Grade))]
        public IActionResult Update_GradeMaster([FromBody] tbl_grade_master objgradem)
        {
            Response_Msg objResult = new Response_Msg();
            tbl_grade_master_log objlog = new tbl_grade_master_log();
            try
            {
                var exist = _context.tbl_grade_master.Where(x => x.grade_id == objgradem.grade_id).FirstOrDefault();
                var duplicatee = false;
                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid grade id please try again !!";
                    return Ok(objResult);
                }
                else
                {
                    //check with same grade name in other greade id
                    //var duplicate = _context.tbl_grade_master.Where(x => x.grade_id != objgradem.grade_id && x.grade_name.Trim() == objgradem.grade_name.Trim()).FirstOrDefault();
                    var duplicate = _context.tbl_grade_master.Where(x => x.grade_id == objgradem.grade_id && x.grade_name.Trim().ToUpper() == objgradem.grade_name.Trim().ToUpper()).FirstOrDefault();
                    if (duplicate != null)
                    {
                        exist.is_active = Convert.ToInt32(objgradem.is_active);
                        exist.last_modified_by = 1;
                        exist.last_modified_date = DateTime.Now;
                        _context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();

                        //objResult.StatusCode = 1;
                        //objResult.Message = "Grade name exist in the system please check & try !!";
                        objResult.StatusCode = 0;
                        objResult.Message = "Grade Successfully Updated !!";
                        return Ok(objResult);
                    }
                    else
                    {
                        var all_grade_name = _context.tbl_grade_master.ToList();

                        foreach (var item in all_grade_name)
                        {
                            if (objgradem.grade_name.Trim().ToUpper().Equals(item.grade_name.Trim().ToUpper(), StringComparison.InvariantCultureIgnoreCase))
                            {
                                duplicatee = true;
                            }
                        }

                        if (duplicatee)
                        {

                            objResult.StatusCode = 1;
                            objResult.Message = "Grade name exist in the system please check & try !!";
                        }
                        else
                        {
                            exist.grade_name = objgradem.grade_name;
                            exist.is_active = Convert.ToInt32(objgradem.is_active);
                            exist.last_modified_by = 1;
                            exist.last_modified_date = DateTime.Now;

                            _context.Entry(exist).State = EntityState.Modified;
                            _context.SaveChanges();

                            objResult.StatusCode = 0;
                            objResult.Message = "Grade name updated successfully !!";
                        }
                    }


                    return Ok(objResult);
                }

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        #region-------Designation Master-------
        //save Designation master
        [Route("Save_DesignationMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Designation))]
        public IActionResult Save_DesignationMaster([FromBody] tbl_designation_master objdesignation)
        {
            Response_Msg objResult = new Response_Msg();
            tbl_grade_master_log objlog = new tbl_grade_master_log();
            try
            {
                if (!_clsCurrentUser.DownlineEmpId.Contains(objdesignation.created_by))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access...!!";
                    return Ok(objResult);
                }

                var exist = _context.tbl_designation_master.Where(x => x.designation_name.Trim().ToUpper() == objdesignation.designation_name.Trim().ToUpper()).FirstOrDefault();

                if (exist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Designation name already exist !!";
                    return Ok(objResult);
                }
                else
                {
                    objdesignation.is_active = Convert.ToInt32(objdesignation.is_active);
                    objdesignation.created_by = _clsCurrentUser.EmpId;
                    objdesignation.created_date = DateTime.Now;

                    _context.Entry(objdesignation).State = EntityState.Added;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Designation name save successfully !!";
                    return Ok(objResult);
                }

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        //get designation master data
        [Route("Get_DesignationMaster/{desigid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_DesignationMaster([FromRoute] int desigid)
        {
            try
            {
                if (desigid > 0)
                {
                    var data = _context.tbl_designation_master.Where(x => x.designation_id == desigid).FirstOrDefault();
                    return Ok(data);
                }
                else
                {
                    var data = _context.tbl_designation_master.AsEnumerable().Select((a, index) => new
                    {
                        designationname = a.designation_name,
                        designationid = a.designation_id,
                        status = a.is_active,
                        createdby = a.created_by,
                        createdon = a.created_date,
                        modifiedon = a.last_modified_date,
                        sno = index + 1
                    }).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //update designation master
        [Route("Update_DesignationMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Designation))]
        public IActionResult Update_DesignationMaster([FromBody] tbl_designation_master objdesignation)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.DownlineEmpId.Contains(objdesignation.last_modified_by))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access....!!";
                return Ok(objResult);
            }

            tbl_grade_master_log objlog = new tbl_grade_master_log();
            try
            {

                var exist = _context.tbl_designation_master.Where(x => x.designation_id == objdesignation.designation_id).FirstOrDefault();

                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid desgnation id please try again !!";
                    return Ok(objResult);
                }
                else
                {
                    //check with same designation name in other designation id
                    var duplicate = _context.tbl_designation_master.Where(x => x.designation_id != objdesignation.designation_id && x.designation_name.Trim().ToUpper() == objdesignation.designation_name.Trim().ToUpper()).FirstOrDefault();
                    if (duplicate != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Designation name exist in the system please check & try !!";
                        return Ok(objResult);
                    }

                    exist.designation_name = objdesignation.designation_name;
                    exist.is_active = Convert.ToInt32(objdesignation.is_active);
                    exist.last_modified_by = _clsCurrentUser.EmpId;
                    exist.last_modified_date = DateTime.Now;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Designation name updated successfully !!";
                    return Ok(objResult);
                }

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }
        #endregion

        #region-------Department Master-------
        //save Designation master
        [Route("Save_DepartmentMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Department))]
        public IActionResult Save_DepartmentMaster([FromBody] tbl_department_master objdepartment)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {
                var exist = _context.tbl_department_master.Where(x => x.department_name.Trim().ToUpper() == objdepartment.department_name.Trim().ToUpper() && x.company_id == objdepartment.company_id).FirstOrDefault();

                if (exist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Department name already exist !!";
                    return Ok(objResult);
                }
                else
                {
                    //---get count and genrate dept code
                    var DeptCount = _context.tbl_department_master.Count();
                    DeptCount = DeptCount == 0 ? 1 : DeptCount + 1;
                    string DeptCode = "";
                    string str = objdepartment.department_name;
                    if (DeptCode != "" && DeptCode != null)
                    {
                        str.Split(' ').ToList().ForEach(i => DeptCode = DeptCode + i[0]);
                        DeptCode = DeptCode.ToUpper() + DeptCount.ToString("000");
                    }
                    else
                    {
                        str.Split(' ').ToList().ForEach(i => DeptCode = DeptCode + i[0]);
                        DeptCode = DeptCount.ToString("000");
                    }

                    //----end here

                    objdepartment.department_code = DeptCode;
                    objdepartment.is_active = Convert.ToInt32(objdepartment.is_active);
                    objdepartment.created_by = 1;
                    objdepartment.created_date = DateTime.Now;

                    _context.Entry(objdepartment).State = EntityState.Added;
                    _context.SaveChanges();

                    ////Start Sub Department Enter for NA - Added by Amarjeet : Date - 21-09-2020 ////////////////////////////

                    var department_id = objdepartment.department_id;

                    tbl_sub_department_master sub_department = new tbl_sub_department_master();
                    sub_department.sub_department_code = "NA";
                    sub_department.sub_department_name = "NA";
                    sub_department.department_id = department_id;
                    sub_department.company_id = objdepartment.company_id;
                    sub_department.is_active = 1;
                    sub_department.created_by = 1;
                    sub_department.created_date = DateTime.Now;
                    sub_department.last_modified_by = 1;
                    sub_department.last_modified_date = DateTime.Now;

                    _context.Entry(sub_department).State = EntityState.Added;
                    _context.SaveChanges();

                    /////////////////////////////End Sub Department Enter for NA  ///////////////////////////////////////////

                    objResult.StatusCode = 0;
                    objResult.Message = "Department name save successfully !!";
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

        //get designation master data
        [Route("Get_DepartmentMaster/{deptid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_DepartmentMaster([FromRoute] int deptid)
        {
            try
            {


                if (deptid > 0)
                {
                    var data = _context.tbl_department_master.Where(x => _clsCurrentUser.CompanyId.Contains(x.company_id) && x.department_id == deptid).FirstOrDefault();
                    return Ok(data);
                }
                else
                {
                    var data = (from a in _context.tbl_department_master
                                join b in _context.tbl_company_master on a.company_id equals b.company_id
                                where _clsCurrentUser.CompanyId.Contains(a.company_id)
                                select new
                                {
                                    a.department_id,
                                    a.company_id,
                                    a.department_code,
                                    a.department_name,
                                    a.department_short_name,
                                    a.department_head_employee_name,
                                    a.department_head_employee_code,
                                    a.created_by,
                                    a.created_date,
                                    b.company_name,
                                    a.is_active,
                                    emp_head_name_code = string.Format("{0} {1} {2} ({3})", a.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).employee_first_name,
                                    a.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).employee_middle_name,
                                    a.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).employee_last_name,
                                    a.tbl_employee_id_details.emp_code),
                                    a.employee_id,
                                }).AsEnumerable().Select((a, index) => new
                                {
                                    departmentname = a.department_name,
                                    shortname = a.department_short_name,
                                    departmentid = a.department_id,
                                    departmentcode = a.department_code,
                                    deptheadname = a.department_head_employee_name,
                                    deptheadcode = a.department_head_employee_code,
                                    companyname = a.company_name,
                                    status = a.is_active,
                                    createdby = a.created_by,
                                    createdon = a.created_date,
                                    emp_head_name_code = a.emp_head_name_code,
                                    employee_id = a.employee_id,
                                    sno = index + 1
                                }).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //get designation master data
        [Route("Get_DepartmentMasterByCompanyId/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "7021")]
        public IActionResult Get_DepartmentMasterByCompanyId([FromRoute] int company_id)
        {
            try
            {
                var data = (from a in _context.tbl_department_master
                            join b in _context.tbl_company_master on a.company_id equals b.company_id
                            where a.company_id == company_id
                            select new
                            {
                                a.department_id,
                                a.company_id,
                                a.department_code,
                                a.department_name,
                                a.department_short_name,
                                a.department_head_employee_name,
                                a.department_head_employee_code,
                                a.created_by,
                                a.created_date,
                                b.company_name,
                                a.is_active,
                                emp_head_name_code = string.Format("{0} {1} {2} ({3})", a.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).employee_first_name,
                                    a.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).employee_middle_name,
                                    a.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).employee_last_name,
                                    a.tbl_employee_id_details.emp_code),
                                a.employee_id,
                            }).AsEnumerable().Select((a, index) => new
                            {
                                departmentname = a.department_name,
                                shortname = a.department_short_name,
                                departmentid = a.department_id,
                                departmentcode = a.department_code,
                                deptheadname = a.department_head_employee_name,
                                deptheadcode = a.department_head_employee_code,
                                companyname = a.company_name,
                                status = a.is_active,
                                createdby = a.created_by,
                                createdon = a.created_date,
                                emp_head_name_code = a.emp_head_name_code,
                                employee_id = a.employee_id,
                                sno = index + 1
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        //update designation master
        [Route("Update_DepartmentMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Department))]
        public IActionResult Update_DepartmentMaster([FromBody] tbl_department_master objdepartment)
        {
            Response_Msg objResult = new Response_Msg();
            tbl_grade_master_log objlog = new tbl_grade_master_log();
            try
            {
                var exist = _context.tbl_department_master.Where(x => x.department_id == objdepartment.department_id && x.company_id == objdepartment.company_id).FirstOrDefault();

                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid department id please try again !!";
                    return Ok(objResult);
                }
                else
                {
                    //---get count and genrate dept code
                    var DeptCount = _context.tbl_department_master.Count();
                    DeptCount = DeptCount == 0 ? 1 : DeptCount + 1;
                    string DeptCode = "";
                    string str = objdepartment.department_name;
                    str.Split(' ').ToList().ForEach(i => DeptCode = DeptCode + i[0]);
                    DeptCode = DeptCode.ToUpper() + DeptCount.ToString("000");
                    //----end here

                    //check with same department name in other department id
                    var duplicate = _context.tbl_department_master.Where(x => x.department_id != objdepartment.department_id && x.department_name.Trim().ToUpper() == objdepartment.department_name.Trim().ToUpper() && x.company_id == objdepartment.company_id).FirstOrDefault();

                    if (duplicate != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Department name exist in the system please check & try !!";
                        return Ok(objResult);
                    }
                    exist.company_id = objdepartment.company_id;
                    exist.department_name = objdepartment.department_name;
                    exist.department_short_name = objdepartment.department_short_name;
                    exist.employee_id = objdepartment.employee_id;
                    exist.department_code = DeptCode;
                    exist.is_active = Convert.ToInt32(objdepartment.is_active); ;
                    exist.last_modified_by = 1;
                    exist.last_modified_date = DateTime.Now;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Department name updated successfully !!";
                    return Ok(objResult);
                }

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }
        #endregion

        //get company list 
        [Route("Get_CompanyList")]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_CompanyList()
        {
            try
            {
                var data = (from a in _context.tbl_company_master.Where(b => b.is_active == 1)
                            select new
                            {
                                CompanyName = a.company_name,
                                CompanyId = a.company_id
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //bind employee head list

        [Route("Get_EmployeeHeadList")]
        [HttpGet]
        //[Authorize(Policy = "7024")]
        public IActionResult Get_EmployeeHeadList()
        {
            try
            {


                var data = _context.tbl_employee_master.Where(x => x.is_active == 1).Select(a => new
                {
                    a.created_by,
                    a.created_date,
                    a.last_modified_by,
                    a.last_modified_date,
                    a.employee_id,
                    emp_code1 = string.Format("{0} {1} {2} ({3})", a.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && q.employee_first_name != "").employee_first_name,
                 a.tbl_emp_officaial_sec.FirstOrDefault(r => r.is_deleted == 0 && r.employee_middle_name != "").employee_middle_name,
                 a.tbl_emp_officaial_sec.FirstOrDefault(s => s.is_deleted == 0 && s.employee_last_name != "").employee_last_name, a.emp_code)
                }).ToList();
                return Ok(data);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #region-------Sub Department Master-------
        //save sub department master
        [Route("Save_SubDepartmentMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Department))]
        public async Task<IActionResult> Save_SubDepartmentMaster([FromBody] tbl_sub_department_master objsubdept)
        {
            Response_Msg objResult = new Response_Msg();
            tbl_sub_department_master_log objlog = new tbl_sub_department_master_log();
            try
            {
                var exist = _context.tbl_sub_department_master.Where(
                    x => x.sub_department_name.Trim().ToUpper() == objsubdept.sub_department_name.Trim().ToUpper()
                    && x.company_id == objsubdept.company_id && x.department_id == objsubdept.department_id
                    ).FirstOrDefault();

                if (exist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Sub Department name already exist !!";
                    return Ok(objResult);
                }
                else
                {
                    using (var trans = _context.Database.BeginTransaction())
                    {
                        //tbl_sub_department_master tbl_Sub = new tbl_sub_department_master()
                        //{
                        //    department_id = objsubdept.department_id,
                        //    sub_department_code = objsubdept.sub_department_code,
                        //    sub_department_name = objsubdept.sub_department_name,
                        //    company_id = objsubdept.company_id,
                        //    is_active = 1,
                        //    created_by = 1,
                        //    created_date = DateTime.Now,
                        //    last_modified_by = 1,
                        //    last_modified_date = DateTime.Now,
                        //};
                        objsubdept.is_active = Convert.ToInt32(objsubdept.is_active);
                        objsubdept.created_by = 1;
                        objsubdept.created_date = DateTime.Now;
                        objsubdept.last_modified_by = 1;
                        objsubdept.last_modified_date = DateTime.Now;

                        //_context.tbl_sub_department_master.Add(tbl_Sub);
                        //await _context.SaveChangesAsync();


                        _context.Entry(objsubdept).State = EntityState.Added;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "Sub Department name save successfully !!";

                        //save in log table
                        int sub_dept_id = objsubdept.sub_department_id;
                        objlog.sid = sub_dept_id;
                        objlog.sub_department_code = objsubdept.sub_department_code;
                        objlog.sub_department_name = objsubdept.sub_department_name;
                        objlog.d_id = objsubdept.department_id;
                        objlog.company_id = objsubdept.company_id;
                        objlog.requested_by = 1;
                        objlog.requested_date = DateTime.Now;

                        _context.Entry(objlog).State = EntityState.Added;
                        _context.SaveChanges();

                        trans.Commit();

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

        //get sub department master data
        [Route("Get_SubDepartmentMaster/{subdeptid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_SubDepartmentMaster([FromRoute] int subdeptid)
        {
            try
            {
                if (subdeptid > 0)
                {
                    var data = _context.tbl_sub_department_master.Where(x => x.sub_department_id == subdeptid).FirstOrDefault();
                    return Ok(data);
                }
                else
                {
                    var data = (from a in _context.tbl_sub_department_master
                                join b in _context.tbl_department_master on a.department_id equals b.department_id
                                join c in _context.tbl_company_master on b.company_id equals c.company_id
                                select new
                                {
                                    a.department_id,
                                    a.company_id,
                                    a.sub_department_id,
                                    a.sub_department_code,
                                    a.sub_department_name,
                                    a.created_by,
                                    a.created_date,
                                    b.department_name,
                                    c.company_name,
                                    a.is_active
                                }).AsEnumerable().Select((a, index) => new
                                {
                                    subdeptname = a.sub_department_name,
                                    subdeptcode = a.sub_department_code,
                                    subdeptid = a.sub_department_id,
                                    departmentname = a.department_name,
                                    companyid = a.company_id,
                                    companyname = a.company_name,
                                    status = a.is_active,
                                    createdby = a.created_by,
                                    createdon = a.created_date,
                                    sno = index + 1
                                }).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //update sub department master
        [Route("Update_SubDepartmentMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Department))]
        public IActionResult Update_SubDepartmentMaster([FromBody] tbl_sub_department_master objsubdept)
        {
            Response_Msg objResult = new Response_Msg();
            tbl_sub_department_master_log objlog = new tbl_sub_department_master_log();
            try
            {
                var exist = _context.tbl_sub_department_master.Where(x => x.sub_department_id == objsubdept.sub_department_id).FirstOrDefault();

                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid sub department id please try again !!";
                    return Ok(objResult);
                }
                else
                {

                    //check with same department name in other department id
                    var duplicate = _context.tbl_sub_department_master.Where(x => x.sub_department_id != objsubdept.sub_department_id
                    && x.company_id == objsubdept.company_id && x.department_id == objsubdept.department_id
                    && x.sub_department_name.Trim().ToUpper() == objsubdept.sub_department_name.Trim().ToUpper()).FirstOrDefault();

                    if (duplicate != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Sub department name exist in company dept. please check & try !!";
                        return Ok(objResult);
                    }
                    using (var trans = _context.Database.BeginTransaction())
                    {
                        exist.company_id = objsubdept.company_id;
                        exist.sub_department_name = objsubdept.sub_department_name;
                        exist.sub_department_code = objsubdept.sub_department_code;
                        exist.department_id = objsubdept.department_id;

                        exist.is_active = Convert.ToInt32(objsubdept.is_active); ;
                        exist.last_modified_by = 1;
                        exist.last_modified_date = DateTime.Now;

                        _context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();

                        //save in log table
                        int sub_dept_id = objsubdept.sub_department_id;
                        objlog.sid = sub_dept_id;
                        objlog.sub_department_code = objsubdept.sub_department_code;
                        objlog.sub_department_name = objsubdept.sub_department_name;
                        objlog.d_id = objsubdept.department_id;
                        objlog.company_id = objsubdept.company_id;
                        objlog.requested_by = 1;
                        objlog.requested_date = DateTime.Now;

                        _context.Entry(objlog).State = EntityState.Added;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "Sub department updated successfully !!";
                        trans.Commit(); //commit transaction

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
        #endregion

        //get department by company id
        [Route("Get_DepartmentListAll")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_DepartmentListAll()
        {
            ResponseMsg objresponse = new ResponseMsg();

            try
            {
                var data = (from a in _context.tbl_department_master.Where(x => x.is_active == 1)
                            select new
                            {
                                a.department_id,
                                a.department_name
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }
        //get department by company id
        [Route("Get_DepartmentByCompany/{companyid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_DepartmentByCompany([FromRoute] int companyid)
        {
            ResponseMsg objresponse = new ResponseMsg();

            try
            {
                var data = (from a in _context.tbl_department_master.Where(x => x.company_id == companyid && x.is_active == 1 && _clsCurrentUser.CompanyId.Contains(x.company_id))
                            select new
                            {
                                a.department_id,
                                a.department_name
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        //get sub department by department id
        [Route("Get_SubDepartmentByDepartment/{department_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_SubDepartmentByDepartment([FromRoute] int department_id)
        {
            try
            {
                var data = (from a in _context.tbl_sub_department_master.Where(x => x.department_id == department_id && x.is_active == 1)
                            select new
                            {
                                a.sub_department_id,
                                a.sub_department_name
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //get sub department by department id
        [Route("Get_SubLocationByDepartment/{location_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_SubLocationByDepartment([FromRoute] int location_id)
        {
            try
            {
                var data = (from a in _context.tbl_sub_location_master.Where(x => x.location_id == location_id && x.is_active == 1)
                            select new
                            {
                                a.sub_location_id,
                                a.location_name
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }




        //get sub department by department id
        [Route("Get_ShiftList/{companyid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_ShiftList([FromRoute] int companyid)
        {
            try
            {
                var data = (from a in _context.tbl_shift_details.Where(x => x.shift_for_all_company == companyid && x.is_deleted == 0)
                            select new
                            {
                                a.shift_details_id,
                                a.shift_name
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //get sub department by department id
        [Route("Get_AllShiftList/{companyid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Report))]
        public IActionResult Get_AllShiftList(int companyid)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(companyid))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }
            try
            {
                var data = (from t1 in _context.tbl_shift_master
                            join t2 in _context.tbl_shift_details on t1.shift_id equals t2.shift_id
                            where t1.is_active == 1 && t2.is_deleted == 0
                            select new { shift_id = t1.shift_id, shift_name = string.Concat(t1.shift_code, "-", t2.shift_name) }).ToList().Distinct();

                //var data = _context.tbl_shift_location.Where(a => (a.tbl_shift_details.shift_for_all_company == 1 || a.company_id == companyid) && a.tbl_shift_details.is_deleted == 0).Select(p => new
                //{
                //    p.tbl_shift_details.shift_name,
                //    p.tbl_shift_details.shift_id
                //}).ToList();

                //var data = (from a in _context.tbl_shift_details
                //            join b in _context.tbl_shift_location
                //            on a.shift_details_id equals b.shift_detail_id into c
                //            from e in c.DefaultIfEmpty()
                //            where (a.shift_for_all_company == 1 || e.company_id == companyid) && a.is_deleted == 0
                //            select new
                //            {
                //                a.shift_id,
                //                a.shift_name
                //            }).ToList();

                //var data = (from a in _context.tbl_shift_details.Where(x => x.shift_for_all_company == companyid && x.is_deleted == 0)
                //            select new
                //            {
                //                a.shift_id,
                //                a.shift_name
                //            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }
        //get sub Get_GradeMaster by companyid id
        [Route("Get_GradeMaster/{companyid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_GradeMaster([FromRoute] int companyid)
        {
            try
            {
                var data = (from a in _context.tbl_grade_master.Where(x => x.is_active == 1)
                            select new
                            {
                                a.grade_id,
                                a.grade_name
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        //get sub Get_GradeMaster by companyid id
        [Route("Get_DesignationList/{companyid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_DesignationList([FromRoute] int companyid)
        {
            try
            {
                var data = (from a in _context.tbl_designation_master.Where(x => x.is_active == 1)
                            select new
                            {
                                a.designation_id,
                                a.designation_name
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }




        #region ----Leave Type Master----
        //save leave type master
        [Route("Save_LeaveTypeMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveType))]
        public IActionResult Save_LeaveTypeMaster([FromBody] tbl_leave_type objleavetype)
        {
            Response_Msg objResult = new Response_Msg();
            tbl_leave_type_log objlog = new tbl_leave_type_log();
            try
            {
                // var exist = _context.tbl_leave_type.Where(x => x.leave_type_name == objleavetype.leave_type_name).FirstOrDefault();
                var exist = _context.tbl_leave_type.Where(x => x.leave_type_name.Trim().ToUpper().Contains(objleavetype.leave_type_name.Trim().ToUpper())).ToList();

                if (exist.Count > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Leave type name already exist !!";
                    return Ok(objResult);
                }
                else
                {
                    using (var trans = _context.Database.BeginTransaction())
                    {
                        objleavetype.is_active = Convert.ToInt32(objleavetype.is_active);
                        objleavetype.created_by = 1;
                        objleavetype.created_date = DateTime.Now;

                        _context.Entry(objleavetype).State = EntityState.Added;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "Leave type name save successfully !!";

                        //save in log table
                        int leavetype_id = objleavetype.leave_type_id;
                        objlog.leave_type_id = leavetype_id;
                        objlog.leave_type_name = objleavetype.leave_type_name;
                        objlog.description = objleavetype.description;
                        objlog.requested_by = 1;
                        objlog.requested_date = DateTime.Now;

                        _context.Entry(objlog).State = EntityState.Added;
                        _context.SaveChanges();

                        trans.Commit();

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

        //get sub department master data
        [Route("Get_LeaveTypeMaster/{leavetypeid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_LeaveTypeMaster([FromRoute] int leavetypeid)
        {
            try
            {
                if (leavetypeid > 0)
                {
                    var data = _context.tbl_leave_type.Where(x => x.leave_type_id == leavetypeid).FirstOrDefault();
                    return Ok(data);
                }
                else
                {
                    var data = (from a in _context.tbl_leave_type
                                where a.is_active == 1
                                select new
                                {
                                    a.leave_type_id,
                                    a.leave_type_name,
                                    a.description,
                                    a.created_by,
                                    a.created_date,
                                    a.is_active
                                }).AsEnumerable().Select((a, index) => new
                                {
                                    leavetypeid = a.leave_type_id,
                                    leavetype = a.leave_type_name,
                                    descriptions = a.description,
                                    status = a.is_active,
                                    createdby = a.created_by,
                                    createdon = a.created_date,
                                    sno = index + 1
                                }).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //update sub department master
        [Route("Update_LeaveTypeMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveType))]
        public IActionResult Update_LeaveTypeMaster([FromBody] tbl_leave_type objleavetype)
        {
            Response_Msg objResult = new Response_Msg();
            tbl_leave_type_log objlog = new tbl_leave_type_log();
            try
            {
                var exist = _context.tbl_leave_type.Where(x => x.leave_type_id == objleavetype.leave_type_id).FirstOrDefault();
                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid leave type id please try again !!";
                    return Ok(objResult);
                }
                else
                {

                    //check for duplicate
                    var duplicate = _context.tbl_leave_type.Where(x => x.leave_type_id != objleavetype.leave_type_id
                    && x.leave_type_name.Trim().ToUpper() == objleavetype.leave_type_name.Trim().ToUpper()).FirstOrDefault();

                    if (duplicate != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Leave type duplicate found please check & try !!";
                        return Ok(objResult);
                    }
                    using (var trans = _context.Database.BeginTransaction())
                    {
                        exist.is_active = Convert.ToInt32(objleavetype.is_active); ;
                        exist.last_modified_by = 1;
                        exist.leave_type_name = objleavetype.leave_type_name;
                        exist.description = objleavetype.description;
                        exist.last_modified_date = DateTime.Now;

                        _context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();

                        //save in log table
                        int leavetype_id = objleavetype.leave_type_id;
                        objlog.leave_type_id = leavetype_id;
                        objlog.leave_type_name = objleavetype.leave_type_name;
                        objlog.description = objleavetype.description;
                        objlog.requested_by = 1;
                        objlog.requested_date = DateTime.Now;

                        _context.Entry(objlog).State = EntityState.Added;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "Information updated successfully !!";
                        trans.Commit(); //commit transaction

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

        //delete leave type
        [Route("Delete_LeaveTypeMaster/{leavetypeid}")]
        [HttpDelete]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveType))]
        public IActionResult Delete_LeaveTypeMaster([FromRoute] int leavetypeid)
        {
            Response_Msg objResult = new Response_Msg();
            tbl_leave_type_log objlog = new tbl_leave_type_log();

            try
            {
                var delete = _context.tbl_leave_type.Where(x => x.is_active == 1 && x.leave_type_id == leavetypeid).FirstOrDefault();

                if (delete == null)
                {

                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid leave tyep id please try later !!";
                    return Ok(objResult);
                }
                else
                {
                    using (var trans = _context.Database.BeginTransaction())
                    {
                        delete.is_active = 0;
                        delete.last_modified_by = 1;
                        delete.last_modified_date = DateTime.Now;

                        _context.Entry(delete).State = EntityState.Modified;
                        _context.SaveChanges();

                        //save in log table                        
                        objlog.leave_type_id = leavetypeid;
                        objlog.leave_type_name = delete.leave_type_name;
                        objlog.description = delete.description;
                        objlog.requested_by = 1;
                        objlog.requested_date = DateTime.Now;

                        _context.Entry(objlog).State = EntityState.Added;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "Leave type deleted successfully !!";
                        trans.Commit(); //commit transaction

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

        [Route("Get_AllLeaveType/{leavetypeid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_AllLeaveType([FromRoute] int leavetypeid)
        {
            try
            {
                if (leavetypeid > 0)
                {
                    var data = _context.tbl_leave_type.Where(a => a.leave_type_id == leavetypeid).FirstOrDefault();

                    return Ok(data);
                }
                else
                {
                    var data = _context.tbl_leave_type.Select(p => new
                    {
                        p.leave_type_id,
                        p.leave_type_name,
                        p.description,
                        p.created_by,
                        p.created_date,
                        p.is_active,
                        p.last_modified_date
                    }).AsEnumerable().Select((e, index) => new
                    {
                        leavetypeid = e.leave_type_id,
                        leavetype = e.leave_type_name,
                        descriptions = e.description,
                        status = e.is_active,
                        createdby = e.created_by,
                        createdon = e.created_date,
                        modifiedon = e.last_modified_date,
                        sno = index + 1

                    }).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #endregion


        //get location by company id for ddl
        [Route("Get_LocationByCompany/{companyid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_LocationByCompany([FromRoute] int companyid)
        {
            ResponseMsg objresponse = new ResponseMsg();
            try
            {
                if (!_clsCurrentUser.CompanyId.Contains(companyid))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unauthorize Company Access....!!";
                    return Ok(objresponse);
                }
                var data = (from a in _context.tbl_location_master.Where(x => x.company_id == companyid && x.is_active == 1)
                            select new
                            {
                                a.location_id,
                                a.location_name
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [Route("Get_LocationByCompanyState/{companyid}/{stateid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_LocationByCompanyState([FromRoute] int companyid, int stateid)
        {
            ResponseMsg objresponse = new ResponseMsg();
            try
            {
                if (!_clsCurrentUser.CompanyId.Contains(companyid))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unauthorize Company Access....!!";
                    return Ok(objresponse);
                }
                var data = (from a in _context.tbl_location_master.Where(x => x.company_id == companyid && x.state_id == stateid && x.is_active == 1)
                            select new
                            {
                                a.location_id,
                                a.location_name
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //bind religion master data list

        [Route("Get_ReligionMaster")]
        [HttpGet]
        //[Authorize(Policy = "7041")]
        public IActionResult Get_ReligionMaster()
        {
            try
            {
                var data = (from a in _context.tbl_religion_master.Where(x => x.is_active == 1)
                            select new
                            {
                                a.religion_id,
                                a.religion_name
                            });

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //get all employee type from enum
        [Route("GetEmployeeType")]
        [HttpGet]
        //[Authorize(Policy = "7042")]
        public IActionResult GetEmployeeType()
        {
            try
            {
                List<object> list = new List<object>();
                foreach (EmployeeType employee in Enum.GetValues(typeof(EmployeeType)))
                {
                    int value = (int)Enum.Parse(typeof(EmployeeType), Enum.GetName(typeof(EmployeeType), employee));
                    // string Str = Enum.GetName(typeof(LocationType), value);

                    Type type = employee.GetType();
                    MemberInfo[] memInfo = type.GetMember(employee.ToString());

                    if (memInfo != null && memInfo.Length > 0)
                    {
                        object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (attrs != null && attrs.Length > 0)
                        {
                            string strvalue = ((DescriptionAttribute)attrs[0]).Description;
                            if (value != (int)EmployeeType.FNF)// && value != (int)EmployeeType.Terminate)
                            {
                                list.Add(new
                                {
                                    emptypeid = value,
                                    emptypename = strvalue
                                });
                            }
                        }
                    }
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //get all frequency type from enum
        [Route("GetFrequencyType")]
        [HttpGet]
        //[Authorize(Policy = "7043")]
        public IActionResult GetFrequencyType()
        {
            try
            {
                List<object> list = new List<object>();
                foreach (FrequencyType frequency in Enum.GetValues(typeof(FrequencyType)))
                {
                    int value = (int)Enum.Parse(typeof(FrequencyType), Enum.GetName(typeof(FrequencyType), frequency));
                    // string Str = Enum.GetName(typeof(LocationType), value);

                    Type type = frequency.GetType();
                    MemberInfo[] memInfo = type.GetMember(frequency.ToString());

                    if (memInfo != null && memInfo.Length > 0)
                    {
                        object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (attrs != null && attrs.Length > 0)
                        {
                            string strvalue = ((DescriptionAttribute)attrs[0]).Description;
                            list.Add(new
                            {
                                frequencyid = value,
                                frequencyname = strvalue
                            });
                        }
                    }
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        //Get all Role from table
        [Route("GetRole")]
        [HttpGet]
        //[Authorize(Policy = "7044")]
        public IActionResult GetRole()
        {
            try
            {
                var data = (from a in _context.tbl_role_master.Where(x => x.is_active == 1)
                            select new
                            {
                                a.role_id,
                                a.role_name
                            });

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        //bind employee head list

        [Route("Get_EmployeeCodeFromEmpMaster")]
        [HttpGet]
        //[Authorize(Policy = "7045")]
        public IActionResult Get_EmployeeCodeFromEmpMaster()
        {
            try
            {
                var data = (from a in _context.tbl_employee_master.Where(x => x.is_active == 1)
                            select new
                            {
                                a.employee_id,
                                a.emp_code
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [Route("Get_EmployeeCodeFromEmpMasterByComp/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "7046")]
        public IActionResult Get_EmployeeCodeFromEmpMasterByComp(int company_id)
        {
            try
            {
                //started by supriya on 03-06-2019
                var temp = _context.tbl_user_master.Join(_context.tbl_company_master, c => c.default_company_id, d => d.company_id, (c, d) => new
                {
                    c.employee_id,
                    c.default_company_id,
                    d.company_name
                }).Distinct().ToList();


                var data = _context.tbl_emp_officaial_sec.Join(temp, i => i.employee_id, j => j.employee_id, (i, j) => new
                {
                    i.is_deleted,
                    i.tbl_employee_id_details,
                    j.default_company_id,
                    j.employee_id,
                    i.is_mobile_access,
                    i.is_mobile_attendence_access,
                    i.employee_first_name,
                    i.employee_middle_name,
                    i.employee_last_name
                }).Where(b => b.is_deleted == 0 && b.tbl_employee_id_details.is_active == 1 && b.default_company_id == company_id)
                    .Select(a => new { a.tbl_employee_id_details.emp_code, a.employee_id, a.employee_first_name, a.employee_middle_name, a.employee_last_name, a.is_mobile_access, a.is_mobile_attendence_access })
                    .Select(j => new
                    {
                        j.employee_id,
                        emp_code = j.emp_code,
                        emp_name = string.Format("{0} {1} {2}", j.employee_first_name, j.employee_middle_name, j.employee_last_name),
                        mobile_access = j.is_mobile_access,
                        mobile_attendence_access = j.is_mobile_attendence_access
                    }).ToList();

                //end by supriya on 03-06-2019

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [Route("Get_EmployeeByCompAndDeptId/{company_id}/{dept_id}")]
        [HttpGet]
        public IActionResult Get_EmployeeByCompAndDeptId(int company_id, int dept_id)
        {
            try
            {
                var temp = _context.tbl_user_master.Join(_context.tbl_company_master, c => c.default_company_id, d => d.company_id, (c, d) => new
                {
                    c.employee_id,
                    c.default_company_id,
                    d.company_name
                }).Distinct().ToList();


                var data = _context.tbl_emp_officaial_sec.Join(temp, i => i.employee_id, j => j.employee_id, (i, j) => new
                {
                    i.is_deleted,
                    i.tbl_employee_id_details,
                    j.default_company_id,
                    j.employee_id,
                    i.is_mobile_access,
                    i.is_mobile_attendence_access,
                    i.employee_first_name,
                    i.employee_middle_name,
                    i.employee_last_name,
                    i.department_id
                }).Where(b => b.is_deleted == 0 && b.tbl_employee_id_details.is_active == 1 && b.default_company_id == company_id && b.department_id == dept_id)
                    .Select(a => new { a.tbl_employee_id_details.emp_code, a.employee_id, a.employee_first_name, a.employee_middle_name, a.employee_last_name, a.is_mobile_access, a.is_mobile_attendence_access })
                    .Select(j => new
                    {
                        j.employee_id,
                        emp_code = j.emp_code,
                        emp_name = string.Format("{0} {1} {2}", j.employee_first_name, j.employee_middle_name, j.employee_last_name),
                        emp_name_code = string.Format("{0} {1} {2} ({3})", j.employee_first_name, j.employee_middle_name, j.employee_last_name, j.emp_code),
                        mobile_access = j.is_mobile_access,
                        mobile_attendence_access = j.is_mobile_attendence_access
                    }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Get_EmpNameAndCodeByeComp/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "7047")]
        public IActionResult Get_EmpNameAndCodeByeComp(int company_id)
        {
            try
            {
                var data = (
                from e in _context.tbl_employee_master
                join u in _context.tbl_user_master on e.employee_id equals u.employee_id
                join ed in _context.tbl_emp_officaial_sec on e.employee_id equals ed.employee_id
                where u.default_company_id == company_id && ed.is_deleted == 0
                select new
                {
                    e.employee_id,
                    e.emp_code,
                    ed.employee_first_name,
                    ed.employee_middle_name,
                    ed.employee_last_name,
                    emp_name_code = string.Format("{0} {1} {2} ({3})", ed.employee_first_name, ed.employee_middle_name, ed.employee_last_name, e.emp_code)

                }).ToList().OrderBy(x => x.emp_name_code);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Get_EmpNameAndCodeByDept/{company_id}/{empid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_EmpNameAndCodeByDept([FromRoute] int company_id, int empid)
        {
            try
            {

                var deptid = _context.tbl_emp_officaial_sec.Where(x => x.employee_id == empid && x.is_deleted == 0).FirstOrDefault().department_id;

                var data = (
                from e in _context.tbl_employee_master
                join u in _context.tbl_user_master on e.employee_id equals u.employee_id
                join ed in _context.tbl_emp_officaial_sec on e.employee_id equals ed.employee_id
                where u.default_company_id == company_id && ed.is_deleted == 0 && ed.department_id == deptid && e.employee_id != empid
                select new
                {
                    e.employee_id,
                    e.emp_code,
                    ed.employee_first_name,
                    ed.employee_middle_name,
                    ed.employee_last_name,
                    emp_name_code = string.Format("{0} {1} {2} ({3})", ed.employee_first_name, ed.employee_middle_name, ed.employee_last_name, e.emp_code)

                }).ToList().OrderBy(x => x.emp_name_code);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Get_EmpNameAndCodeByeComp_ESeperation/{company_id}/{loginid}")]
        [HttpGet]
        //[Authorize(Policy = "7047")]
        public IActionResult Get_EmpNameAndCodeByeComp_ESeperation(int company_id, int loginid)
        {
            try
            {
                var data = (
                from e in _context.tbl_employee_master
                join es in _context.tbl_emp_separation on e.employee_id equals es.emp_id
                join u in _context.tbl_user_master on e.employee_id equals u.employee_id
                join ed in _context.tbl_emp_officaial_sec on e.employee_id equals ed.employee_id
                where u.default_company_id == company_id && ed.is_deleted == 0 && es.is_deleted == 0 && es.is_cancel == 0 && es.is_final_approve == 1
                select new
                {
                    e.employee_id,
                    e.emp_code,
                    ed.employee_first_name,
                    ed.employee_middle_name,
                    ed.employee_last_name,
                    emp_name_code = string.Format("{0} {1} {2} ({3})", ed.employee_first_name, ed.employee_middle_name, ed.employee_last_name, e.emp_code)

                }).ToList().OrderBy(x => x.emp_name_code);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #region get employee/location and holiday list on multiple selection of company made by Anil kumar
        [Route("Get_EmpNameAndCodeByMultiComp")]
        [HttpPost]
        //[Authorize(Policy = "7047")]
        public IActionResult Get_EmpNameAndCodeByMultiComp([FromBody] companylistcls companyids)
        {
            try
            {
                var companyidlist = companyids.company_id_list.Select(x => x.company_id).Distinct().ToList();
                if (companyidlist.Count == 1 && companyidlist[0] == 0)
                {
                    var data = (
                from e in _context.tbl_employee_master
                join u in _context.tbl_user_master on e.employee_id equals u.employee_id
                join ed in _context.tbl_emp_officaial_sec on e.employee_id equals ed.employee_id
                where ed.is_deleted == 0
                select new
                {
                    e.employee_id,
                    e.emp_code,
                    ed.employee_first_name,
                    ed.employee_middle_name,
                    ed.employee_last_name,
                    emp_name_code = string.Format("{0} {1} {2} ({3})", ed.employee_first_name, ed.employee_middle_name, ed.employee_last_name, e.emp_code)

                }).Distinct().ToList();

                    return Ok(data);
                }
                else
                {
                    var data = (
                    from e in _context.tbl_employee_master
                    join u in _context.tbl_user_master on e.employee_id equals u.employee_id
                    join ed in _context.tbl_emp_officaial_sec on e.employee_id equals ed.employee_id
                    where companyidlist.Contains(u.default_company_id) && ed.is_deleted == 0
                    select new
                    {
                        e.employee_id,
                        e.emp_code,
                        ed.employee_first_name,
                        ed.employee_middle_name,
                        ed.employee_last_name,
                        emp_name_code = string.Format("{0} {1} {2} ({3})", ed.employee_first_name, ed.employee_middle_name, ed.employee_last_name, e.emp_code)

                    }).Distinct().ToList();

                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Get_EmpNameAndCodeByMultiLoc")]
        [HttpPost]
        //[Authorize(Policy = "7047")]
        public IActionResult Get_EmpNameAndCodeByMultiLoc([FromBody] locationlistcls locationids)
        {
            try
            {
                var locationidlist = locationids.location_id_list.Select(x => x.location_id).Distinct().ToList();
                if (locationidlist.Count == 1 && locationidlist[0] == 0)
                {
                    var data = (
                from e in _context.tbl_employee_master
                join u in _context.tbl_user_master on e.employee_id equals u.employee_id
                join ed in _context.tbl_emp_officaial_sec on e.employee_id equals ed.employee_id
                where ed.is_deleted == 0
                select new
                {
                    e.employee_id,
                    e.emp_code,
                    ed.employee_first_name,
                    ed.employee_middle_name,
                    ed.employee_last_name,
                    emp_name_code = string.Format("{0} {1} {2} ({3})", ed.employee_first_name, ed.employee_middle_name, ed.employee_last_name, e.emp_code)

                }).Distinct().ToList();

                    return Ok(data);
                }
                else
                {
                    var data = (
                    from e in _context.tbl_employee_master
                    join u in _context.tbl_user_master on e.employee_id equals u.employee_id
                    join ed in _context.tbl_emp_officaial_sec on e.employee_id equals ed.employee_id
                    where locationidlist.Contains(Convert.ToInt32(ed.location_id)) && ed.is_deleted == 0
                    select new
                    {
                        e.employee_id,
                        e.emp_code,
                        ed.employee_first_name,
                        ed.employee_middle_name,
                        ed.employee_last_name,
                        emp_name_code = string.Format("{0} {1} {2} ({3})", ed.employee_first_name, ed.employee_middle_name, ed.employee_last_name, e.emp_code)

                    }).Distinct().ToList();

                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Get_LocationByMultiCompany")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_LocationByMultiCompany([FromBody] companylistcls companyids)
        {
            ResponseMsg objresponse = new ResponseMsg();
            try
            {
                var companyidlist = companyids.company_id_list.Select(x => x.company_id).Distinct().ToList();
                if (companyidlist.Count == 1 && companyidlist[0] == 0)
                {
                    var data = (from a in _context.tbl_location_master.Where(x => x.is_active == 1)
                                select new
                                {
                                    a.location_id,
                                    a.location_name
                                }).Distinct().ToList();

                    return Ok(data);
                }
                else
                {
                    var data = (from a in _context.tbl_location_master.Where(x => companyidlist.Contains(x.company_id) && x.is_active == 1)
                                select new
                                {
                                    a.location_id,
                                    a.location_name
                                }).Distinct().ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Get_CompanyHolidayByMultiCompany")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public async Task<IActionResult> Get_CompanyHolidayByMultiCompany([FromBody] companylistcls companyids)
        {
            ResponseMsg objResult = new ResponseMsg();
            var companyidlist = companyids.company_id_list.Select(x => x.company_id).Distinct().ToList();

            if (companyids == null || (companyids != null && companyidlist.Count == 1 && companyidlist[0] == 0))
            {
                var result = (from hm in _context.tbl_holiday_master
                              join hc in _context.tbl_holiday_master_comp_list on hm.holiday_id equals hc.holiday_id into hcc
                              from hc in hcc.DefaultIfEmpty()
                              where (hm.is_active == 1 || hm.is_active == 0) && hc.is_deleted == 0
                              select new
                              {
                                  holiday_no = hm.holidayno,
                                  from_date = hm.to_date.Year == 2500 ? hm.holiday_date : hm.from_date,
                                  to_date = hm.to_date.Year == 2500 ? hm.holiday_date : hm.to_date,
                                  holiday_name = hm.holiday_name,
                                  //holiday_date = hm.holiday_date,
                                  is_applicable_on_all_emp = hm.is_applicable_on_all_emp,
                                  is_applicable_on_all_religion = hm.is_applicable_on_all_religion,
                                  is_applicable_on_all_location = hm.is_applicable_on_all_location,
                                  company_id = hc.company_id,
                                  created_by = hm.created_by,
                                  created_date = Convert.ToDateTime(hm.created_date).ToString("dd-MMM-yyyy"),
                                  is_active = hm.is_active,
                                  company_name = hc.tbl_company_master.company_name
                                  //company_name = _context.tbl_holiday_master_comp_list.Where(x => x.holiday_id == hm.holiday_id && x.is_deleted == 0).Select(x => x.tbl_company_master.company_name).FirstOrDefault(),

                              }).OrderByDescending(x => x.from_date).ToList().Distinct();

                return Ok(result);
            }
            else
            {
                var result = (from hm in _context.tbl_holiday_master
                              join hc in _context.tbl_holiday_master_comp_list on hm.holiday_id equals hc.holiday_id
                              where (hm.is_active == 1 || hm.is_active == 0)
                              && companyidlist.Contains(Convert.ToInt32(hc.company_id)) && hc.is_deleted == 0
                              //&& _clsCurrentUser.CompanyId.Contains(company_id)
                              // && hno.Contains(hm.holidayno)
                              select new
                              {
                                  holiday_no = hm.holidayno,
                                  from_date = hm.to_date.Year == 2500 ? hm.holiday_date : hm.from_date,
                                  to_date = hm.to_date.Year == 2500 ? hm.holiday_date : hm.to_date,
                                  holiday_name = hm.holiday_name,
                                  //holiday_date = hm.holiday_date,
                                  is_applicable_on_all_emp = hm.is_applicable_on_all_emp,
                                  is_applicable_on_all_religion = hm.is_applicable_on_all_religion,
                                  is_applicable_on_all_location = hm.is_applicable_on_all_location,
                                  company_id = hc.company_id,
                                  created_by = hm.created_by,
                                  created_date = Convert.ToDateTime(hm.created_date).ToString("dd-MMM-yyyy"),
                                  company_name = hc.tbl_company_master.company_name,
                                  is_active = hm.is_active,
                                  //company = _context.tbl_holiday_master_comp_list.Where(x => x.holiday_id == hm.holiday_id && x.is_deleted == 0)
                                  //         .Select(x => new { company_id = x.company_id, company_name = x.tbl_company_master.company_name }).FirstOrDefault(),

                              }).OrderByDescending(x => x.from_date).ToList().Distinct();//.GroupBy(x => x.holiday_no);


                return Ok(result);

            }

        }

        #endregion

        //Save Machine Master
        [Route("Save_MachineMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.MachineMaster))]
        public IActionResult Save_MachineMaster([FromBody] tbl_machine_master objmachine)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(objmachine.company_id ?? 0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access";
                return Ok(objResult);
            }

            try
            {
                var exist = _context.tbl_machine_master.Where(x => x.ip_address == objmachine.ip_address && x.company_id == objmachine.company_id).FirstOrDefault();

                if (exist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "IP Address already exist...!";
                    return Ok(objResult);
                }
                else
                {

                    objmachine.created_by = _clsCurrentUser.EmpId;
                    objmachine.created_date = DateTime.Now;
                    objmachine.last_modified_date = DateTime.Now;

                    _context.Entry(objmachine).State = EntityState.Added;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Machine Details Save successfully...!";
                    return Ok(objResult);
                }

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }


        //get machine master data
        [Route("Get_MachineMasterData/{machine_master_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.MachineMaster))]
        public IActionResult Get_MachineMasterData([FromRoute] int machine_master_id)
        {
            try
            {
                if (machine_master_id > 0)
                {
                    var data = _context.tbl_machine_master.Where(x => x.machine_id == machine_master_id).FirstOrDefault();
                    return Ok(data);
                }
                else
                {
                    var data = _context.tbl_machine_master.Where(x => _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).AsEnumerable().Select((a, index) => new
                    {
                        machine_id = a.machine_id,
                        ip_address = a.ip_address,
                        port_number = a.port_number,
                        machine_number = a.machine_number,
                        machine_type = a.machine_type,
                        machine_description = a.machine_description,
                        is_active = a.is_active,
                        location_id = a.location_id,
                        company_id = a.company_id,
                        sub_location_id = a.sub_location_id,
                        created_by = a.created_by,
                        createdon = a.created_date,
                        sno = index + 1
                    }).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //get machine master data
        [Route("Get_MachineMasterDataByCompany/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_MachineMasterDataByCompany([FromRoute] int company_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }

            try
            {

                var data = _context.tbl_machine_master.Where(x => x.company_id == company_id && x.is_active == 1).Select(a => new
                {
                    machine_id = a.machine_id,
                    ip_address = a.ip_address,
                    port_number = a.port_number,
                    machine_number = a.machine_number,
                    machine_type = a.machine_type,
                    machine_description = a.machine_description,
                    is_active = a.is_active,
                    location_id = a.location_id,
                    company_id = a.company_id,
                    sub_location_id = a.sub_location_id,
                    _location = a.tbl_location_master.location_name,
                    _sub_location = a.tbl_sub_location_master.location_name,
                    created_by = a.created_by,
                    createdon = a.created_date,
                }).ToList();


                return Ok(data);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        //update grade master
        [Route("Update_MachineMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.MachineMaster))]
        public IActionResult Update_MachineMaster([FromBody] tbl_machine_master objmachine)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(objmachine.last_modified_by))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access";
                return Ok(objResult);
            }

            if (!_clsCurrentUser.CompanyId.Contains(objmachine.company_id ?? 0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access";
                return Ok(objResult);
            }

            try
            {
                var exist = _context.tbl_machine_master.Where(x => x.machine_id == objmachine.machine_id).FirstOrDefault();

                if (exist == null)
                {
                    objResult.StatusCode = 2;
                    objResult.Message = "IP Address already exist...!";
                    return Ok(objResult);
                }
                else
                {
                    //check with same grade name in other greade id
                    var duplicate = _context.tbl_machine_master.Where(x => x.machine_id != objmachine.machine_id && x.ip_address == objmachine.ip_address && x.company_id == objmachine.company_id).FirstOrDefault();
                    if (duplicate != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "IP Address exist in the system please check & try...!";
                        return Ok(objResult);
                    }

                    exist.company_id = objmachine.company_id;
                    exist.location_id = objmachine.location_id;
                    exist.sub_location_id = objmachine.sub_location_id;
                    exist.ip_address = objmachine.ip_address;
                    exist.port_number = objmachine.port_number;
                    exist.machine_number = objmachine.machine_number;
                    exist.machine_type = objmachine.machine_type;
                    exist.is_active = objmachine.is_active;
                    exist.last_modified_by = _clsCurrentUser.EmpId;
                    exist.last_modified_date = DateTime.Now;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Machine Details Updated Successfully...!";
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

        //Save Machine Master
        [Route("Save_CompanyHoliday")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.HolidayMaster))]
        public IActionResult Save_CompanyHoliday([FromBody] holiday_master objholidayMaster)
        {
            Response_Msg objResult = new Response_Msg();
            //if (!_clsCurrentUser.CompanyId.Contains(objholidayMaster.company_id))
            //{
            //    objResult.StatusCode = 1;
            //    objResult.Message = "Unauthorize Company Access...!!";
            //    return Ok(objResult);
            //}
            try
            {
                var company_id_list = objholidayMaster.company_id_list.Select(x => x.company_id).ToList();
                //var exist = _context.tbl_holiday_master.Where(x => x.holiday_name.Trim().ToUpper() == objholidayMaster.holiday_name.Trim().ToUpper() && x.holiday_date == objholidayMaster.holiday_date && x.tbl_holiday_master_comp_list.FirstOrDefault(g => g.is_deleted == 0).company_id == objholidayMaster.company_id && x.holiday_date.Year == objholidayMaster.holiday_date.Year).FirstOrDefault();  // add by Ravi
                // var exist = _context.tbl_holiday_master.Where(x => x.holiday_name == objholidayMaster.holiday_name && x.holiday_date != objholidayMaster.holiday_date).FirstOrDefault();
                var exist = _context.tbl_holiday_master.Where(x => x.holiday_name.Trim().ToUpper() == objholidayMaster.holiday_name.Trim().ToUpper() && x.is_active != 3 && (x.from_date >= objholidayMaster.from_date && x.to_date <= objholidayMaster.to_date) && company_id_list.Contains(Convert.ToInt32(x.tbl_holiday_master_comp_list.FirstOrDefault(g => g.is_deleted == 0).company_id))).ToList();  // add by Anil
                if (exist != null && exist.Count > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Holiday Name already exist...!";
                    return Ok(objResult);
                }
                else
                {
                    using (var trans = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            Random random_obj = new Random();
                            int random_no_1 = random_obj.Next();
                            var holiday_no_generate = random_no_1.ToString();

                            for (DateTime i = objholidayMaster.from_date; i <= objholidayMaster.to_date; i = i.AddDays(1))
                            {
                                //Console.WriteLine(i.ToShortDateString());
                                //}
                                // tbl_holiday_master
                                tbl_holiday_master tbl_holiday_master = new tbl_holiday_master();

                                tbl_holiday_master.from_date = objholidayMaster.from_date; //new DateTime(2000, 01, 01);
                                tbl_holiday_master.to_date = objholidayMaster.to_date;//new DateTime(2500, 01, 01);
                                tbl_holiday_master.holiday_name = objholidayMaster.holiday_name;
                                tbl_holiday_master.holiday_date = Convert.ToDateTime(i.ToShortDateString()); //objholidayMaster.holiday_date;
                                tbl_holiday_master.holidayno = holiday_no_generate;

                                //if (objholidayMaster.company_id == 0) // add by Ravi
                                //{
                                //    tbl_holiday_master.is_applicable_on_all_comp = 1;
                                //    tbl_holiday_master.is_applicable_on_all_location = 1;
                                //}
                                //else
                                //{
                                //    tbl_holiday_master.is_applicable_on_all_comp = 0;
                                //}

                                if (objholidayMaster.is_applicable_on_all_comp == 0) // add by anil
                                {
                                    tbl_holiday_master.is_applicable_on_all_comp = 0;

                                }
                                else
                                {
                                    tbl_holiday_master.is_applicable_on_all_comp = 1;
                                }

                                if (objholidayMaster.is_applicable_on_all_location == 0) // add by Ravi 
                                {
                                    tbl_holiday_master.is_applicable_on_all_location = 0;// modified by anil
                                }
                                else
                                {
                                    tbl_holiday_master.is_applicable_on_all_location = 1;// modified by anil
                                }

                                if (objholidayMaster.is_applicable_on_all_emp == 0) // add by Ravi
                                {
                                    tbl_holiday_master.is_applicable_on_all_emp = 0;// modified by anil
                                }
                                else
                                {
                                    tbl_holiday_master.is_applicable_on_all_emp = 1;// modified by anil
                                }

                                if (objholidayMaster.is_applicable_on_all_religion == 1) // add by Ravi
                                {
                                    tbl_holiday_master.is_applicable_on_all_religion = 1;
                                }
                                else if (objholidayMaster.is_applicable_on_all_religion == 9)
                                {
                                    tbl_holiday_master.is_applicable_on_all_religion = 1;
                                }
                                else
                                {
                                    tbl_holiday_master.is_applicable_on_all_religion = 0;
                                }


                                tbl_holiday_master.is_active = objholidayMaster.is_active;
                                tbl_holiday_master.created_by = objholidayMaster.created_by;
                                tbl_holiday_master.last_modified_by = objholidayMaster.last_modified_by;
                                tbl_holiday_master.created_date = DateTime.Now;
                                tbl_holiday_master.last_modified_date = DateTime.Now;

                                _context.Entry(tbl_holiday_master).State = EntityState.Added;
                                _context.SaveChanges();
                                // holiday_id
                                var holiday_id = tbl_holiday_master.holiday_id;

                                #region for multiple company and multiple location
                                //if (objholidayMaster.is_applicable_on_all_comp != 0 && objholidayMaster.is_applicable_on_all_location != 0) //  for all company and all location// added by anil
                                //{
                                //    // var companylist = objholidayMaster.company_id_list;
                                //    tbl_holiday_master_comp_list Holiday_Company = new tbl_holiday_master_comp_list();
                                //    // Holiday_Company.company_id = companylist[0].company_id;// objholidayMaster.company_id;
                                //    // Holiday_Company.location_id = objholidayMaster.is_applicable_on_all_location;
                                //    Holiday_Company.holiday_id = holiday_id;
                                //    Holiday_Company.is_deleted = 0;
                                //    _context.Entry(Holiday_Company).State = EntityState.Added;
                                //    //_context.SaveChanges();
                                //}
                                //else if (objholidayMaster.is_applicable_on_all_comp != 0 && objholidayMaster.is_applicable_on_all_location == 0) // for all company and  for selected multiple locaiton// added by anil
                                //{
                                //    //  var companylist = objholidayMaster.company_id_list;
                                //    var locationlist = objholidayMaster.location_id_list;
                                //    List<tbl_holiday_master_comp_list> objlocationlist = new List<tbl_holiday_master_comp_list>();

                                //    for (int loc = 0; loc < locationlist.Count; loc++) // for multiple selection of location/ added by anil
                                //    {
                                //        tbl_holiday_master_comp_list Holiday_Company = new tbl_holiday_master_comp_list();
                                //        // Holiday_Company.company_id = companylist[0].company_id; //objholidayMaster.company_id;
                                //        Holiday_Company.location_id = locationlist[loc].location_id;//objholidayMaster.is_applicable_on_all_location;
                                //        Holiday_Company.holiday_id = holiday_id;
                                //        Holiday_Company.is_deleted = 0;
                                //        _context.Entry(Holiday_Company).State = EntityState.Added;
                                //        objlocationlist.Add(Holiday_Company);
                                //    }
                                //    _context.tbl_holiday_master_comp_list.AddRange(objlocationlist);
                                //}

                                var companylist = objholidayMaster.company_id_list.Where(x => x.company_id != 0).ToList();
                                if (objholidayMaster.is_applicable_on_all_location == 0) // for selected multiple locaiton// added by anil
                                {

                                    var locationlist = objholidayMaster.location_id_list;
                                    var location_selected_ids = locationlist.Select(x => x.location_id).Distinct().ToList();

                                    for (int compid = 0; compid < companylist.Count; compid++) // for all company or multiple selected company
                                    {
                                        List<tbl_holiday_master_comp_list> objlocationlist = new List<tbl_holiday_master_comp_list>();

                                        var locationidlist = (from a in _context.tbl_location_master
                                                              .Where(x => x.company_id == companylist[compid].company_id && x.is_active == 1 && location_selected_ids.Contains(x.location_id))
                                                              select new { a.location_id }).Distinct().ToList();

                                        for (int loc = 0; loc < locationidlist.Count; loc++) // for multiple selection of location/ added by anil
                                        {
                                            tbl_holiday_master_comp_list Holiday_Company = new tbl_holiday_master_comp_list();
                                            Holiday_Company.company_id = companylist[compid].company_id; //objholidayMaster.company_id;
                                            Holiday_Company.location_id = locationidlist[loc].location_id;//objholidayMaster.is_applicable_on_all_location;
                                            Holiday_Company.holiday_id = holiday_id;
                                            Holiday_Company.is_deleted = 0;
                                            _context.Entry(Holiday_Company).State = EntityState.Added;
                                            objlocationlist.Add(Holiday_Company);
                                        }
                                        _context.tbl_holiday_master_comp_list.AddRange(objlocationlist);
                                    }
                                    _context.SaveChanges();
                                }
                                else if (objholidayMaster.is_applicable_on_all_location != 0) // for all locaiton// added by anil
                                {

                                    // var locationlist = objholidayMaster.location_id_list;
                                    //var location_selected_ids = locationlist.Select(x => x.location_id).Distinct().ToList();

                                    for (int compid = 0; compid < companylist.Count; compid++) // for multiple selection of company/ added by anil
                                    {
                                        List<tbl_holiday_master_comp_list> objlocationlist = new List<tbl_holiday_master_comp_list>();

                                        tbl_holiday_master_comp_list Holiday_Company = new tbl_holiday_master_comp_list();
                                        Holiday_Company.company_id = companylist[compid].company_id; //objholidayMaster.company_id;
                                                                                                     // Holiday_Company.location_id = locationidlist[loc].location_id;//objholidayMaster.is_applicable_on_all_location;
                                        Holiday_Company.holiday_id = holiday_id;
                                        Holiday_Company.is_deleted = 0;
                                        _context.Entry(Holiday_Company).State = EntityState.Added;
                                        objlocationlist.Add(Holiday_Company);

                                        _context.tbl_holiday_master_comp_list.AddRange(objlocationlist);
                                    }
                                    _context.SaveChanges();
                                }
                                #endregion
                                #region for multiple company and multiple employee
                                //if (objholidayMaster.is_applicable_on_all_comp != 0 && objholidayMaster.is_applicable_on_all_emp != 0) //  for all company and all employee// added by anil
                                //{
                                //    // var companylist = objholidayMaster.company_id_list;
                                //    tbl_holiday_master_emp_list Holiday_Employee = new tbl_holiday_master_emp_list();
                                //    // Holiday_Company.company_id = companylist[0].company_id;// objholidayMaster.company_id;
                                //    // Holiday_Company.location_id = objholidayMaster.is_applicable_on_all_emp;
                                //    Holiday_Employee.holiday_id = holiday_id;
                                //    Holiday_Employee.is_deleted = 0;
                                //    _context.Entry(Holiday_Employee).State = EntityState.Added;
                                //    //_context.SaveChanges();
                                //}
                                //else if (objholidayMaster.is_applicable_on_all_comp != 0 && objholidayMaster.is_applicable_on_all_emp == 0) // for all company and  for selected multiple employee// added by anil
                                //{
                                //    //  var companylist = objholidayMaster.company_id_list;
                                //    var emplist = objholidayMaster.emp_id_list;
                                //    List<tbl_holiday_master_emp_list> objemplist = new List<tbl_holiday_master_emp_list>();

                                //    for (int loc = 0; loc < emplist.Count; loc++) // for multiple selection of employee/ added by anil
                                //    {
                                //        tbl_holiday_master_emp_list Holiday_Employee = new tbl_holiday_master_emp_list();
                                //        // Holiday_Company.company_id = companylist[0].company_id; //objholidayMaster.company_id;
                                //        Holiday_Employee.employee_id = emplist[loc].employee_id;//objholidayMaster.is_applicable_on_all_emp;
                                //        Holiday_Employee.holiday_id = holiday_id;
                                //        Holiday_Employee.is_deleted = 0;
                                //        _context.Entry(Holiday_Employee).State = EntityState.Added;
                                //        objemplist.Add(Holiday_Employee);
                                //    }
                                //    _context.tbl_holiday_master_emp_list.AddRange(objemplist);
                                //}
                                if (objholidayMaster.is_applicable_on_all_emp == 0) // for selected multiple employee// added by anil
                                {

                                    var employeelist = objholidayMaster.emp_id_list;

                                    var emp_selected_ids = employeelist.Select(x => x.employee_id).Distinct().ToList();


                                    for (int compid = 0; compid < companylist.Count; compid++) // for multiple selection of employee/ added by anil
                                    {
                                        List<tbl_holiday_master_emp_list> objemplist = new List<tbl_holiday_master_emp_list>();

                                        var empidlist = (
                                                  from e in _context.tbl_employee_master
                                                  join u in _context.tbl_user_master on e.employee_id equals u.employee_id
                                                  join ed in _context.tbl_emp_officaial_sec on e.employee_id equals ed.employee_id
                                                  where u.default_company_id == companylist[compid].company_id && ed.is_deleted == 0
                                                  && emp_selected_ids.Contains(e.employee_id)
                                                  select new { e.employee_id }).Distinct().ToList();

                                        for (int emp = 0; emp < empidlist.Count; emp++) // for multiple selection of employee/ added by anil
                                        {
                                            tbl_holiday_master_emp_list Holiday_Employee = new tbl_holiday_master_emp_list();
                                            Holiday_Employee.company_id = companylist[compid].company_id; //objholidayMaster.company_id;
                                            Holiday_Employee.employee_id = empidlist[emp].employee_id;//objholidayMaster.is_applicable_on_all_emp;
                                            Holiday_Employee.holiday_id = holiday_id;
                                            Holiday_Employee.is_deleted = 0;
                                            _context.Entry(Holiday_Employee).State = EntityState.Added;
                                            objemplist.Add(Holiday_Employee);
                                        }
                                        _context.tbl_holiday_master_emp_list.AddRange(objemplist);

                                    }
                                    _context.SaveChanges();
                                }
                                else if (objholidayMaster.is_applicable_on_all_emp != 0) // for all employee// added by anil
                                {

                                    // var locationlist = objholidayMaster.location_id_list;
                                    //var location_selected_ids = locationlist.Select(x => x.location_id).Distinct().ToList();

                                    for (int compid = 0; compid < companylist.Count; compid++) // for multiple selection of company/ added by anil
                                    {
                                        List<tbl_holiday_master_emp_list> objemplist = new List<tbl_holiday_master_emp_list>();

                                        tbl_holiday_master_emp_list Holiday_Employee = new tbl_holiday_master_emp_list();
                                        Holiday_Employee.company_id = companylist[compid].company_id; //objholidayMaster.company_id;
                                                                                                      // Holiday_Company.location_id = locationidlist[loc].location_id;//objholidayMaster.is_applicable_on_all_location;
                                        Holiday_Employee.holiday_id = holiday_id;
                                        Holiday_Employee.is_deleted = 0;
                                        _context.Entry(Holiday_Employee).State = EntityState.Added;
                                        objemplist.Add(Holiday_Employee);

                                        _context.tbl_holiday_master_emp_list.AddRange(objemplist);
                                    }
                                    _context.SaveChanges();
                                }
                                #endregion


                                tbl_holiday_mstr_rel_list Holiday_Religion = new tbl_holiday_mstr_rel_list(); // add by Ravi
                                if (objholidayMaster.is_applicable_on_all_religion > 1)
                                {
                                    Holiday_Religion.religion_id = objholidayMaster.is_applicable_on_all_religion;
                                    Holiday_Religion.h_id = holiday_id;
                                    Holiday_Religion.is_deleted = 0;
                                    _context.Entry(Holiday_Religion).State = EntityState.Added;
                                    //   _context.SaveChanges();
                                }

                                _context.SaveChanges();

                            }
                            trans.Commit();
                            objResult.StatusCode = 0;
                            objResult.Message = "Holiday Details Save successfully...!";
                            return Ok(objResult);

                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            objResult.StatusCode = 2;
                            objResult.Message = ex.Message;
                            return Ok(objResult);
                        }
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


        // GET: api/Get_CompanyHoliday
        //Save Company Holiday
        [Route("Get_CompanyHoliday/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public async Task<IActionResult> Get_CompanyHoliday(int company_id)
        {
            ResponseMsg objResult = new ResponseMsg();
            if (company_id > 0)
            {
                // string[] hno = { "123ss", "2223w", "343dd", "455fddf" };

                var result = (from hm in _context.tbl_holiday_master
                              where (hm.is_active == 1 || hm.is_active == 0)
                              && hm.tbl_holiday_master_comp_list.FirstOrDefault(x => x.is_deleted == 0).company_id == company_id
                              && _clsCurrentUser.CompanyId.Contains(company_id)
                              && hm.holiday_date.Year == DateTime.Now.Year
                              // && hno.Contains(hm.holidayno)
                              select new
                              {
                                  holiday_no = hm.holidayno,
                                  from_date = hm.to_date.Year == 2500 ? hm.holiday_date : hm.from_date,
                                  to_date = hm.to_date.Year == 2500 ? hm.holiday_date : hm.to_date,
                                  holiday_name = hm.holiday_name,
                                  //holiday_date = hm.holiday_date,
                                  is_applicable_on_all_emp = hm.is_applicable_on_all_emp,
                                  is_applicable_on_all_religion = hm.is_applicable_on_all_religion,
                                  is_applicable_on_all_location = hm.is_applicable_on_all_location,
                                  //company_id = hc.company_id,
                                  created_by = hm.created_by,
                                  created_date = Convert.ToDateTime(hm.created_date).ToString("dd-MMM-yyyy"),

                                  is_active = hm.is_active,
                                  company = _context.tbl_holiday_master_comp_list.Where(x => x.holiday_id == hm.holiday_id && x.is_deleted == 0)
                                             .Select(x => new { company_id = x.company_id, company_name = x.tbl_company_master.company_name }).FirstOrDefault(),

                                  //hc.tbl_company_master.company_name
                                  // locaitonlist = _context.tbl_holiday_master_comp_list.Where(x => x.holiday_id == hm.holiday_id && x.is_deleted == 0)
                                  //          .Select(x => new { company_name = x.tbl_company_master.company_name, location_name = x.tbl_location_master.location_name == null ? "" : x.tbl_location_master.location_name }).ToList(),
                                  //emplist = _context.tbl_holiday_master_emp_list.Where(x => x.holiday_id == hm.holiday_id && x.is_deleted == 0)
                                  //.Select(x => new { employeename = x.employee_id == null ? 0 : x.employee_id }).ToList()
                                  //.Select(x => x.tbl_employee_master.tbl_emp_officaial_sec.Select(z => new { employeename = string.Join(" ", z.employee_first_name, z.employee_last_name) })).ToList(),

                              }).OrderByDescending(x => x.from_date).ToList().Distinct();//.GroupBy(x => x.holiday_no);


                return Ok(result);
            }
            else
            {
                var result = (from hm in _context.tbl_holiday_master
                                  // join hc in _context.tbl_holiday_master_comp_list on hm.holiday_id equals hc.holiday_id into hcc
                                  //from hc in hcc.DefaultIfEmpty()
                              where hm.is_active == 1 || hm.is_active == 0 && hm.holiday_date.Year == DateTime.Now.Year
                              select new
                              {
                                  holiday_no = hm.holidayno,
                                  from_date = hm.to_date.Year == 2500 ? hm.holiday_date : hm.from_date,
                                  to_date = hm.to_date.Year == 2500 ? hm.holiday_date : hm.to_date,
                                  holiday_name = hm.holiday_name,
                                  //holiday_date = hm.holiday_date,
                                  is_applicable_on_all_emp = hm.is_applicable_on_all_emp,
                                  is_applicable_on_all_religion = hm.is_applicable_on_all_religion,
                                  is_applicable_on_all_location = hm.is_applicable_on_all_location,
                                  //company_id = hc.company_id,
                                  created_by = hm.created_by,
                                  created_date = Convert.ToDateTime(hm.created_date).ToString("dd-MMM-yyyy"),
                                  is_active = hm.is_active,
                                  //company_name = hc.tbl_company_master.company_name
                                  company_name = _context.tbl_holiday_master_comp_list.Where(x => x.holiday_id == hm.holiday_id && x.is_deleted == 0).Select(x => x.tbl_company_master.company_name).FirstOrDefault(),
                                  // locaitonlist = _context.tbl_holiday_master_comp_list.Where(x => x.holiday_id == hm.holiday_id && x.is_deleted == 0)
                                  //               .Select(x => new { company_name = x.tbl_company_master.company_name, location_name = x.tbl_location_master.location_name }).ToList(),
                                  //emplist = _context.tbl_holiday_master_emp_list.Where(x => x.holiday_id == hm.holiday_id && x.is_deleted == 0)
                                  //        .Select(x => x.tbl_employee_master.tbl_emp_officaial_sec.Select(z => new { employeename = string.Join(" ", z.employee_first_name, z.employee_last_name) })).ToList(),

                              }).OrderByDescending(x => x.from_date).ToList().Distinct();

                // result.RemoveAll(p => !_clsCurrentUser.CompanyId.Contains(p.company_id ?? 0));

                return Ok(result);
            }



            //if (result == null)
            //{
            //    objResult.Message = "Record Not Found...!";
            //    objResult.StatusCode = 0;
            //    return Ok(objResult);
            //}
            //else
            //{
            //    return Ok(result);
            //}

        }

        // GET: api/Get_CompanyHoliday
        //Save Company Holiday
        [Route("Get_CompanyHolidayById/{holiday_no}/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public async Task<IActionResult> Get_CompanyHolidayById([FromRoute] string holiday_no, int company_id)
        {
            ResponseMsg objResult = new ResponseMsg();

            var result = (from hm in _context.tbl_holiday_master
                              //join hc in _context.tbl_holiday_master_comp_list on hm.holiday_id equals hc.holiday_id into hcc
                              //from hc in hcc.DefaultIfEmpty()
                              //join he in _context.tbl_holiday_master_emp_list on hm.holiday_id equals he.holiday_id into hee
                              //from he in hee.DefaultIfEmpty()
                              //join hr in _context.tbl_holiday_mstr_rel_list on hm.holiday_id equals hr.h_id into hrr
                              //from hr in hrr.DefaultIfEmpty()
                          where hm.holidayno == holiday_no
                          select new
                          {
                              holiday_no = hm.holidayno,
                              from_date = hm.to_date.Year == 2500 ? hm.holiday_date : hm.from_date,
                              to_date = hm.to_date.Year == 2500 ? hm.holiday_date : hm.to_date,
                              holiday_name = hm.holiday_name,
                              //holiday_date = hm.holiday_date,
                              is_applicable_on_all_comp = hm.is_applicable_on_all_comp,
                              is_applicable_on_all_emp = hm.is_applicable_on_all_emp,// == 1 ? 0 : he.employee_id,
                              is_applicable_on_all_religion = hm.is_applicable_on_all_religion,// == 1 ? 1 : hr.religion_id,
                              is_applicable_on_all_location = hm.is_applicable_on_all_location,// == 1 ? 0 : hc.location_id,
                                                                                               //company_id = hm.is_applicable_on_all_comp == 0 ? hc.company_id : 0,
                              company_id = _context.tbl_holiday_master_comp_list.Where(x => x.holiday_id == hm.holiday_id && x.company_id == company_id && x.is_deleted == 0).Select(x => x.tbl_company_master.company_id).FirstOrDefault(),
                              //companylist = _context.tbl_holiday_master_comp_list.Where(x => x.holiday_id == hm.holiday_id && x.is_deleted == 0).Select(x => new { company_id = x.company_id == null ? 0 : x.company_id }).ToList(),
                              locaitonlist = _context.tbl_holiday_master_comp_list.Where(x => x.holiday_id == hm.holiday_id && x.company_id == company_id && x.is_deleted == 0).Select(x => new { location_id = x.location_id == null ? 0 : x.location_id }).ToList(),
                              emplist = _context.tbl_holiday_master_emp_list.Where(x => x.holiday_id == hm.holiday_id && x.is_deleted == 0).Select(x => x.employee_id == null ? 0 : x.employee_id).ToList(),
                              rellist = _context.tbl_holiday_mstr_rel_list.Where(x => x.h_id == hm.holiday_id && x.is_deleted == 0).Select(x => x.religion_id).ToList(),

                              created_by = hm.created_by,
                              created_date = hm.created_date,
                              is_active = hm.is_active


                          }).FirstOrDefault();

            if (result == null)
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


        //update grade master
        [Route("Update_CompanyHoliday")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.HolidayMaster))]
        public async Task<IActionResult> Update_CompanyHoliday([FromBody] holiday_master objholidayMaster)
        {
            Response_Msg objResult = new Response_Msg();
            //if (!_clsCurrentUser.CompanyId.Contains(objholidayMaster.company_id))
            //{
            //    objResult.StatusCode = 1;
            //    objResult.Message = "Unauthorize Company Access...!!";
            //    return Ok(objResult);
            //}
            try
            {

                var exist = _context.tbl_holiday_master.Where(x => x.holidayno == objholidayMaster.holidayno).ToList();

                if (exist == null && exist.Count == 0)
                {
                    objResult.StatusCode = 2;
                    objResult.Message = "Holiday id not exist...!";
                    return Ok(objResult);
                }
                else
                {
                    List<tbl_holiday_master> holiday_master = (from a in _context.tbl_holiday_master select a).Where(x => x.holidayno == objholidayMaster.holidayno).OrderByDescending(x => x.holiday_id).ToList();
                    if (holiday_master == null && holiday_master.Count == 0)
                    {
                        objResult.StatusCode = 2;
                        objResult.Message = "Invalid Data...";
                        return Ok(objResult);
                    }
                    // var exist_holiday = _context.tbl_holiday_master.Where(x => x.holidayno != objholidayMaster.holidayno && x.holiday_name.Trim().ToUpper() == objholidayMaster.holiday_name.Trim().ToUpper() && (x.from_date >= objholidayMaster.from_date && x.to_date <= objholidayMaster.to_date) && x.tbl_holiday_master_comp_list.FirstOrDefault(g => g.is_deleted == 0).company_id == objholidayMaster.company_id).ToList();  // add by Anil
                    var exist_holiday = _context.tbl_holiday_master.Where(x => x.holidayno != objholidayMaster.holidayno && x.is_active != 3 && x.holiday_name.ToUpper().Trim() == objholidayMaster.holiday_name.ToUpper().Trim() && x.is_applicable_on_all_comp == (objholidayMaster.company_id_list[0].company_id > 0 ? 0 : 1)).ToList();
                    if (exist_holiday != null)
                    {
                        var _holiday_comp = _context.tbl_holiday_master_comp_list.Where(x => x.company_id == objholidayMaster.company_id_list[0].company_id && x.is_deleted == 0 && exist_holiday.Any(y => y.holiday_id == x.holiday_id)).FirstOrDefault();
                        if (_holiday_comp != null)
                        {
                            // if (exist_holiday.Any(y => y.holiday_date.Year == objholidayMaster.holiday_date.Year))
                            if (exist_holiday.Any(y => y.from_date.Year == objholidayMaster.from_date.Year && y.to_date.Year == objholidayMaster.to_date.Year))
                            {
                                objResult.StatusCode = 2;
                                objResult.Message = "Hoiday Name already exist...!!";
                                return Ok(objResult);
                            }
                        }
                    }

                    //check with same Holiday name name in other holiday id
                    //var duplicate = _context.tbl_holiday_master.Where(x => x.holiday_id != objholidayMaster.holiday_id && x.holiday_name == objholidayMaster.holiday_name && x.is_active == 1).FirstOrDefault();
                    //if (duplicate != null)
                    //{
                    //    objResult.StatusCode = 1;
                    //    objResult.Message = "Holiday name exist in the system please check & try...!";
                    //    return Ok(objResult);
                    //}

                    using (var trans = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (holiday_master != null)
                            {
                                holiday_master.ForEach(p => p.is_active = 3);
                                _context.UpdateRange(holiday_master);

                                //holiday_master.is_active = 3;

                                //_context.tbl_holiday_master.Attach(holiday_master);
                                //_context.Entry(holiday_master).State = EntityState.Modified;
                            }
                            Random random_obj = new Random();
                            int random_no_1 = random_obj.Next();
                            var holiday_no_generate = random_no_1.ToString();

                            for (DateTime i = objholidayMaster.from_date; i <= objholidayMaster.to_date; i = i.AddDays(1))
                            {
                                tbl_holiday_master tblholiday_master = new tbl_holiday_master();

                                tblholiday_master.from_date = objholidayMaster.from_date; //new DateTime(2000, 01, 01);
                                tblholiday_master.to_date = objholidayMaster.to_date;//new DateTime(2500, 01, 01);
                                tblholiday_master.holiday_name = objholidayMaster.holiday_name;
                                tblholiday_master.holiday_date = Convert.ToDateTime(i.ToShortDateString()); //objholidayMaster.holiday_date;
                                tblholiday_master.holidayno = holiday_no_generate;

                                //if (objholidayMaster.company_id_list[0].company_id == 0) // add by Ravi
                                //{
                                //    tblholiday_master.is_applicable_on_all_comp = 1;
                                //}
                                //else
                                //{
                                //    tblholiday_master.is_applicable_on_all_comp = 0;
                                //}

                                if (objholidayMaster.is_applicable_on_all_comp == 0) // add by anil 
                                {
                                    tblholiday_master.is_applicable_on_all_comp = 0;
                                }
                                else
                                {
                                    tblholiday_master.is_applicable_on_all_comp = 1;
                                }

                                if (objholidayMaster.is_applicable_on_all_location == 0) // add by Ravi 
                                {
                                    tblholiday_master.is_applicable_on_all_location = 0;// modified by anil
                                }
                                else
                                {
                                    tblholiday_master.is_applicable_on_all_location = 1;// modified by anil
                                }

                                if (objholidayMaster.is_applicable_on_all_emp == 0) // add by Ravi
                                {
                                    tblholiday_master.is_applicable_on_all_emp = 0;// modified by anil
                                }
                                else
                                {
                                    tblholiday_master.is_applicable_on_all_emp = 1;// modified by anil
                                }


                                if (objholidayMaster.is_applicable_on_all_religion == 1) // add by Ravi
                                {
                                    tblholiday_master.is_applicable_on_all_religion = 1;
                                }
                                else if (objholidayMaster.is_applicable_on_all_religion == 9)
                                {
                                    tblholiday_master.is_applicable_on_all_religion = 1;
                                }
                                else
                                {
                                    tblholiday_master.is_applicable_on_all_religion = 0;
                                }

                                //tblholiday_master.is_applicable_on_all_emp = objholidayMaster.is_applicable_on_all_emp;
                                //tblholiday_master.is_applicable_on_all_religion = objholidayMaster.is_applicable_on_all_religion;
                                //tblholiday_master.is_applicable_on_all_location = objholidayMaster.is_applicable_on_all_location;
                                tblholiday_master.is_active = objholidayMaster.is_active;
                                tblholiday_master.created_by = objholidayMaster.created_by;
                                tblholiday_master.last_modified_by = objholidayMaster.last_modified_by;
                                tblholiday_master.created_date = DateTime.Now;
                                tblholiday_master.last_modified_date = DateTime.Now;

                                _context.Entry(tblholiday_master).State = EntityState.Added;
                                _context.SaveChanges();

                                //delete exists records from complistholiday
                                List<tbl_holiday_master_comp_list> holiday_master_comp = (from a in _context.tbl_holiday_master_comp_list select a).Where(x => x.holiday_id == exist.Where(z => z.holidayno == objholidayMaster.holidayno).FirstOrDefault().holiday_id && x.is_deleted == 0 && x.company_id == objholidayMaster.company_id).OrderByDescending(x => x.sno).ToList();
                                if (holiday_master_comp != null)
                                {
                                    holiday_master_comp.ForEach(p => p.is_deleted = 1);
                                    _context.UpdateRange(holiday_master_comp);
                                }


                                //delete exists records from emplistholiday
                                List<tbl_holiday_master_emp_list> holiday_master_emp = (from a in _context.tbl_holiday_master_emp_list select a).Where(x => x.holiday_id == exist.Where(z => z.holidayno == objholidayMaster.holidayno).FirstOrDefault().holiday_id && x.is_deleted == 0 && x.company_id == objholidayMaster.company_id).OrderByDescending(x => x.sno).ToList();
                                if (holiday_master_emp != null)
                                {
                                    holiday_master_emp.ForEach(p => p.is_deleted = 1);
                                    _context.UpdateRange(holiday_master_emp);
                                }

                                List<tbl_holiday_mstr_rel_list> holiday_master_religion = (from a in _context.tbl_holiday_mstr_rel_list select a).Where(x => x.h_id == exist.Where(z => z.holidayno == objholidayMaster.holidayno).FirstOrDefault().holiday_id && x.is_deleted == 0).OrderByDescending(x => x.sno).ToList();
                                if (holiday_master_religion != null)
                                {
                                    holiday_master_religion.ForEach(p => p.is_deleted = 1);
                                    _context.UpdateRange(holiday_master_religion);
                                    //  _context.SaveChanges();
                                }

                                var holiday_id = tblholiday_master.holiday_id;
                                var company_id = objholidayMaster.company_id_list[0].company_id;

                                #region for location save
                                if (objholidayMaster.is_applicable_on_all_location == 0) // for selected multiple locaiton// added by anil
                                {
                                    var locationlist = objholidayMaster.location_id_list;
                                    var location_selected_ids = locationlist.Select(x => x.location_id).Distinct().ToList();

                                    List<tbl_holiday_master_comp_list> objlocationlist = new List<tbl_holiday_master_comp_list>();

                                    var locationidlist = (from a in _context.tbl_location_master
                                                          .Where(x => x.company_id == company_id && x.is_active == 1 && location_selected_ids.Contains(x.location_id))
                                                          select new { a.location_id }).Distinct().ToList();

                                    for (int loc = 0; loc < locationidlist.Count; loc++) // for multiple selection of location/ added by anil
                                    {
                                        tbl_holiday_master_comp_list Holiday_Company = new tbl_holiday_master_comp_list();
                                        Holiday_Company.company_id = company_id; //objholidayMaster.company_id;
                                        Holiday_Company.location_id = locationidlist[loc].location_id;//objholidayMaster.is_applicable_on_all_location;
                                        Holiday_Company.holiday_id = holiday_id;
                                        Holiday_Company.is_deleted = 0;
                                        _context.Entry(Holiday_Company).State = EntityState.Added;
                                        objlocationlist.Add(Holiday_Company);
                                    }
                                    _context.tbl_holiday_master_comp_list.AddRange(objlocationlist);
                                    _context.SaveChanges();

                                }
                                else if (objholidayMaster.is_applicable_on_all_location != 0) // for all locaiton// added by anil
                                {
                                    List<tbl_holiday_master_comp_list> objlocationlist = new List<tbl_holiday_master_comp_list>();

                                    tbl_holiday_master_comp_list Holiday_Company = new tbl_holiday_master_comp_list();
                                    Holiday_Company.company_id = company_id; //objholidayMaster.company_id;
                                                                             // Holiday_Company.location_id = locationidlist[loc].location_id;//objholidayMaster.is_applicable_on_all_location;
                                    Holiday_Company.holiday_id = holiday_id;
                                    Holiday_Company.is_deleted = 0;
                                    _context.Entry(Holiday_Company).State = EntityState.Added;
                                    objlocationlist.Add(Holiday_Company);

                                    _context.tbl_holiday_master_comp_list.AddRange(objlocationlist);
                                    _context.SaveChanges();
                                }
                                #endregion
                                #region for multiple company and multiple employee

                                if (objholidayMaster.is_applicable_on_all_emp == 0) // for selected multiple employee// added by anil
                                {
                                    var locationlist = objholidayMaster.location_id_list;
                                    var location_selected_ids = locationlist.Select(x => x.location_id).Distinct().ToList();

                                    List<tbl_holiday_master_emp_list> objemplist = new List<tbl_holiday_master_emp_list>();

                                    var empidlist = (
                                              from e in _context.tbl_employee_master
                                              join u in _context.tbl_user_master on e.employee_id equals u.employee_id
                                              join ed in _context.tbl_emp_officaial_sec on e.employee_id equals ed.employee_id
                                              where u.default_company_id == company_id && ed.is_deleted == 0
                                              select new { e.employee_id }).Distinct().ToList();

                                    int[] emp_ids = objholidayMaster.emp_id_list.Select(x => x.employee_id).ToArray();
                                    empidlist = empidlist.Where(x => emp_ids.Contains(x.employee_id)).ToList();

                                    for (int emp = 0; emp < empidlist.Count; emp++) // for multiple selection of employee/ added by anil
                                    {
                                        tbl_holiday_master_emp_list Holiday_Employee = new tbl_holiday_master_emp_list();
                                        Holiday_Employee.company_id = company_id; //objholidayMaster.company_id;
                                        Holiday_Employee.employee_id = empidlist[emp].employee_id;//objholidayMaster.is_applicable_on_all_emp;
                                        Holiday_Employee.holiday_id = holiday_id;
                                        Holiday_Employee.is_deleted = 0;
                                        _context.Entry(Holiday_Employee).State = EntityState.Added;
                                        objemplist.Add(Holiday_Employee);
                                    }
                                    _context.tbl_holiday_master_emp_list.AddRange(objemplist);
                                    _context.SaveChanges();
                                }
                                else if (objholidayMaster.is_applicable_on_all_emp != 0) // for all employee// added by anil
                                {
                                    List<tbl_holiday_master_emp_list> objemplist = new List<tbl_holiday_master_emp_list>();

                                    tbl_holiday_master_emp_list Holiday_Employee = new tbl_holiday_master_emp_list();
                                    Holiday_Employee.company_id = company_id; //objholidayMaster.company_id;
                                                                              // Holiday_Company.location_id = locationidlist[loc].location_id;//objholidayMaster.is_applicable_on_all_location;
                                    Holiday_Employee.holiday_id = holiday_id;
                                    Holiday_Employee.is_deleted = 0;
                                    _context.Entry(Holiday_Employee).State = EntityState.Added;
                                    objemplist.Add(Holiday_Employee);

                                    _context.tbl_holiday_master_emp_list.AddRange(objemplist);
                                    _context.SaveChanges();
                                }
                                #endregion

                                tbl_holiday_mstr_rel_list tbl_holiday_master_rel = new tbl_holiday_mstr_rel_list();
                                if (objholidayMaster.is_applicable_on_all_religion > 1)
                                {
                                    tbl_holiday_master_rel.religion_id = objholidayMaster.is_applicable_on_all_religion;
                                    tbl_holiday_master_rel.h_id = holiday_id;
                                    tbl_holiday_master_rel.is_deleted = 0;
                                    _context.Entry(tbl_holiday_master_rel).State = EntityState.Added;
                                    // _context.SaveChanges();
                                }
                                await _context.SaveChangesAsync();
                            }

                            trans.Commit();
                            objResult.StatusCode = 0;
                            objResult.Message = "Holiday Details Updated Successfully...!";
                            return Ok(objResult);
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            objResult.StatusCode = 2;
                            objResult.Message = ex.Message;
                            return Ok(objResult);
                        }
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


        #region  Outdoor Application
        //Save Outdoor Application Report
        [Route("Save_OutdoorApplicationRequest")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.OutdoorApplication))]
        public IActionResult Save_OutdoorApplicationRequest([FromBody] tbl_outdoor_request objoutdoor_request)
        {

            Response_Msg objResult = new Response_Msg();
            try
            {
                if (!_clsCurrentUser.DownlineEmpId.Any(p => p == objoutdoor_request.r_e_id))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access...!";
                    return Ok(objResult);
                }


                if (Convert.ToBoolean(FunIsApplicationFreezed()) == true)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Outdoor Application has been freezed for this month";
                    return Ok(objResult);
                }

                if (!_clsCurrentUser.CompanyId.Any(p => p == objoutdoor_request.company_id))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Company Access...!";
                    return Ok(objResult);
                }

                if (objoutdoor_request.is_final_approve != 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid data";
                    return Ok(objResult);
                }

                if (DateTime.Compare(objoutdoor_request.manual_in_time, objoutdoor_request.manual_out_time) > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Out Time must be greater than in time";
                    return Ok(objResult);
                }

                //string dateinti=objoutdoor_request.from_date.ToString("dd-MMM-yyyy");
                objoutdoor_request.manual_in_time = objoutdoor_request.from_date.AddHours(objoutdoor_request.manual_in_time.Hour).AddMinutes(objoutdoor_request.manual_in_time.Minute);
                objoutdoor_request.manual_out_time = objoutdoor_request.from_date.AddHours(objoutdoor_request.manual_out_time.Hour).AddMinutes(objoutdoor_request.manual_out_time.Minute);


                var freeze_attandance = _context.tbl_daily_attendance.Where(x => x.emp_id == objoutdoor_request.r_e_id && x.attendance_dt.Date == objoutdoor_request.from_date.Date && x.is_freezed == 1).ToList();

                if (freeze_attandance.Count > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Attandance of selected leave period is freezed";
                    return Ok(objResult);

                }
                var exist_request = _context.tbl_outdoor_request.OrderByDescending(x => x.leave_request_id).Where(x => x.r_e_id == objoutdoor_request.r_e_id && x.company_id == objoutdoor_request.company_id && x.from_date.Date == objoutdoor_request.from_date.Date && x.is_deleted == 0 && x.is_final_approve != 2).FirstOrDefault();


                if (exist_request != null)
                {
                    if (exist_request.is_final_approve == 0)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Outdoor application request already exist for this date...!";
                        return Ok(objResult);
                    }
                    else if (exist_request.is_final_approve == 1)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Outdoor application request already approve for this date...!";
                        return Ok(objResult);
                    }

                }

                var emp_comp = _context.tbl_employee_company_map.Where(x => _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0) && x.is_deleted == 0 && x.employee_id == objoutdoor_request.r_e_id).ToList();
                if (emp_comp.Count > 0)
                {
                    //int empidd = emp_comp.Select(p => p.employee_id).FirstOrDefault() ?? 0;

                    //end check attandance already freezed or not
                    var tbl_daily_attendance = _context.tbl_daily_attendance.Where(x => x.emp_id == objoutdoor_request.r_e_id).FirstOrDefault();
                    if (tbl_daily_attendance != null)
                    {
                        objoutdoor_request.system_in_time = tbl_daily_attendance.shift_in_time;
                        objoutdoor_request.system_out_time = tbl_daily_attendance.shift_out_time;
                    }
                    else
                    {
                        objoutdoor_request.system_in_time = Convert.ToDateTime("2000-01-01 12:00:00");
                        objoutdoor_request.system_out_time = Convert.ToDateTime("2000-01-01 12:00:00");
                    }

                    var teos = _context.tbl_emp_officaial_sec.Where(q => q.is_deleted == 0 && q.employee_id == objoutdoor_request.r_e_id).FirstOrDefault();

                    objoutdoor_request.requester_date = DateTime.Now;
                    objoutdoor_request.creted_dt = DateTime.Now;
                    objoutdoor_request.is_approved1 = 0;
                    objoutdoor_request.is_approved2 = 0;
                    objoutdoor_request.is_approved3 = 0;
                    objoutdoor_request.is_admin_approve = 0;
                    objoutdoor_request.location = !string.IsNullOrEmpty(objoutdoor_request.location) ? objoutdoor_request.location : "";
                    objoutdoor_request.longitude = !string.IsNullOrEmpty(objoutdoor_request.longitude) ? objoutdoor_request.longitude : "";
                    objoutdoor_request.latitude = !string.IsNullOrEmpty(objoutdoor_request.latitude) ? objoutdoor_request.latitude : "";
                    objoutdoor_request.is_auto = 0;
                    _context.Entry(objoutdoor_request).State = EntityState.Added;
                    _context.SaveChanges();


                    objResult.StatusCode = 0;
                    objResult.Message = "outdoor application submited successfully...!";

                    MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                    Task task = Task.Run(() => obj_ms.OutdoorApplicationRequestMail(Convert.ToInt32(objoutdoor_request.r_e_id), objoutdoor_request.from_date,
                        objoutdoor_request.manual_in_time, objoutdoor_request.manual_out_time, objoutdoor_request.system_in_time, objoutdoor_request.system_out_time, objoutdoor_request.requester_remarks));
                    task.Wait();

                }
                else
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Employee Not Bind with Company";
                }

                return Ok(objResult);

                //start Add by supriya to check leave request exist only
                //var exist_attendance = _context.tbl_attendace_request.Where(x => x.from_date == objoutdoor_request.from_date && x.r_e_id == objoutdoor_request.r_e_id && x.company_id==objoutdoor_request.company_id && x.is_deleted == 0 && x.is_final_approve != 2).FirstOrDefault();

                //  var exist_leave = _context.tbl_leave_request.FirstOrDefault(x => x.r_e_id == objoutdoor_request.r_e_id && x.is_deleted == 0 && x.company_id == objoutdoor_request.company_id && x.is_final_approve != 2 &&
                //((x.from_date.Date <= objoutdoor_request.from_date.Date && objoutdoor_request.from_date.Date <= x.to_date.Date)));
                //  var exist_CompOff = _context.tbl_comp_off_request_master.Where(x => x.r_e_id == objoutdoor_request.r_e_id && x.company_id == objoutdoor_request.company_id && x.compoff_against_date == objoutdoor_request.from_date && x.is_deleted == 0 && x.is_final_approve != 2).FirstOrDefault();

                //else if (exist_leave != null)
                //{
                //    if (exist_leave.is_final_approve == 0)
                //    {
                //        objResult.StatusCode = 1;
                //        objResult.Message = "Leave application request already exist for this date...!";
                //        return Ok(objResult);
                //    }
                //    else if (exist_leave.is_final_approve == 1)
                //    {
                //        objResult.StatusCode = 1;
                //        objResult.Message = "Leave application request already approve for this date...!";
                //        return Ok(objResult);
                //    }
                //}
                //else if (exist_attendance != null)
                //{

                //    if (exist_attendance != null)
                //    {
                //        objResult.StatusCode = 1;
                //        objResult.Message = "Raised attandance request either already approved or pending";
                //        return Ok(objResult);
                //    }
                //}
                //else if (exist_CompOff != null)
                //{
                //    objResult.StatusCode = 1;
                //    objResult.Message = "CompOff request either already approved or pending";
                //    return Ok(objResult);
                //}


                //start check attandance already freezed or not



            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        [Route("Save_OutdoorApplicationRequest_v1")] // call from mobile app so in this we have set is_auto=1
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.OutdoorApplication))]
        public IActionResult Save_OutdoorApplicationRequest_v1([FromBody] tbl_outdoor_request objoutdoor_request)
        {

            Response_Msg objResult = new Response_Msg();
            try
            {
                //if (!_clsCurrentUser.DownlineEmpId.Any(p => p == objoutdoor_request.r_e_id))
                //{
                //    objResult.StatusCode = 1;
                //    objResult.Message = "Unauthorize Access...!";
                //    return Ok(objResult);
                //}


                if (Convert.ToBoolean(FunIsApplicationFreezed()) == true)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Outdoor Application has been freezed for this month";
                    return Ok(objResult);
                }

                if (!_clsCurrentUser.CompanyId.Any(p => p == objoutdoor_request.company_id))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Company Access...!";
                    return Ok(objResult);
                }

                if (objoutdoor_request.is_final_approve != 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid data";
                    return Ok(objResult);
                }

                if (DateTime.Compare(objoutdoor_request.manual_in_time, objoutdoor_request.manual_out_time) > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Out Time must be greater than in time";
                    return Ok(objResult);
                }

                //string dateinti=objoutdoor_request.from_date.ToString("dd-MMM-yyyy");
                objoutdoor_request.manual_in_time = objoutdoor_request.from_date.AddHours(objoutdoor_request.manual_in_time.Hour).AddMinutes(objoutdoor_request.manual_in_time.Minute);
                objoutdoor_request.manual_out_time = objoutdoor_request.from_date.AddHours(objoutdoor_request.manual_out_time.Hour).AddMinutes(objoutdoor_request.manual_out_time.Minute);


                var freeze_attandance = _context.tbl_daily_attendance.Where(x => x.emp_id == objoutdoor_request.r_e_id && x.attendance_dt.Date == objoutdoor_request.from_date.Date && x.is_freezed == 1).ToList();

                if (freeze_attandance.Count > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Attandance of selected leave period is freezed";
                    return Ok(objResult);

                }
                var exist_request = _context.tbl_outdoor_request.OrderByDescending(x => x.leave_request_id).Where(x => x.r_e_id == objoutdoor_request.r_e_id && x.company_id == objoutdoor_request.company_id && x.from_date.Date == objoutdoor_request.from_date.Date && x.is_deleted == 0 && x.is_final_approve != 2).FirstOrDefault();


                if (exist_request != null)
                {
                    if (exist_request.is_final_approve == 0)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Outdoor application request already exist for this date...!";
                        return Ok(objResult);
                    }
                    else if (exist_request.is_final_approve == 1)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Outdoor application request already approve for this date...!";
                        return Ok(objResult);
                    }

                }

                var emp_comp = _context.tbl_employee_company_map.Where(x => _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0) && x.is_deleted == 0 && x.employee_id == objoutdoor_request.r_e_id).ToList();
                if (emp_comp.Count > 0)
                {
                    //int empidd = emp_comp.Select(p => p.employee_id).FirstOrDefault() ?? 0;

                    //end check attandance already freezed or not
                    var tbl_daily_attendance = _context.tbl_daily_attendance.Where(x => x.emp_id == objoutdoor_request.r_e_id).FirstOrDefault();
                    if (tbl_daily_attendance != null)
                    {
                        objoutdoor_request.system_in_time = tbl_daily_attendance.shift_in_time;
                        objoutdoor_request.system_out_time = tbl_daily_attendance.shift_out_time;
                    }
                    else
                    {
                        objoutdoor_request.system_in_time = Convert.ToDateTime("2000-01-01 12:00:00");
                        objoutdoor_request.system_out_time = Convert.ToDateTime("2000-01-01 12:00:00");
                    }

                    var teos = _context.tbl_emp_officaial_sec.Where(q => q.is_deleted == 0 && q.employee_id == objoutdoor_request.r_e_id).FirstOrDefault();

                    objoutdoor_request.requester_date = DateTime.Now;
                    objoutdoor_request.creted_dt = DateTime.Now;
                    objoutdoor_request.is_approved1 = 0;
                    objoutdoor_request.is_approved2 = 0;
                    objoutdoor_request.is_approved3 = 0;
                    objoutdoor_request.is_admin_approve = 1;
                    objoutdoor_request.location = !string.IsNullOrEmpty(objoutdoor_request.location) ? objoutdoor_request.location : "";
                    objoutdoor_request.longitude = !string.IsNullOrEmpty(objoutdoor_request.longitude) ? objoutdoor_request.longitude : "";
                    objoutdoor_request.latitude = !string.IsNullOrEmpty(objoutdoor_request.latitude) ? objoutdoor_request.latitude : "";
                    objoutdoor_request.is_auto = 1;
                    objoutdoor_request.is_final_approve = 1;
                    objoutdoor_request.admin_remarks = "Auto Approved by App";
                    objoutdoor_request.admin_id = 1451; // admin id
                    objoutdoor_request.admin_ar_date = DateTime.Now;
                    _context.Entry(objoutdoor_request).State = EntityState.Added;
                    _context.SaveChanges();


                    objResult.StatusCode = 0;
                    objResult.Message = "outdoor application submited successfully...!";

                    MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                    Task task = Task.Run(() => obj_ms.OutdoorApplicationRequestMail(Convert.ToInt32(objoutdoor_request.r_e_id), objoutdoor_request.from_date,
                        objoutdoor_request.manual_in_time, objoutdoor_request.manual_out_time, objoutdoor_request.system_in_time, objoutdoor_request.system_out_time, objoutdoor_request.requester_remarks));
                    task.Wait();

                }
                else
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Employee Not Bind with Company";
                }

                return Ok(objResult);

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }
        //Get Outdoor Application Request By EmpID
        [Route("Get_OutdoorApplicationRequestbyEmpId")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.OutdoorApplication))]
        public IActionResult Get_OutdoorApplicationRequestbyEmpId(int emp_id)
        {
            try
            {
                ResponseMsg objResult = new ResponseMsg();

                if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access...!!";
                    return Ok(objResult);
                }


                var result = _context.tbl_outdoor_request.Where(x => _clsCurrentUser.DownlineEmpId.Contains(x.r_e_id ?? 0) && x.is_deleted == 0 && (x.is_final_approve == 0)).Select(a => new
                {
                    emp_code = a.tbl_employee_requester.emp_code,
                    emp_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                        a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                        a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name),
                    leave_request_id = a.leave_request_id,
                    from_date = a.from_date,
                    manual_in_time = a.manual_in_time,
                    manual_out_time = a.manual_out_time,
                    requester_remarks = a.requester_remarks,
                    status = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : "Reject") : a.is_deleted == 1 ? "Deleted" : a.is_deleted == 2 ? "Cancel" : "",
                    requester_id = a.r_e_id,
                    is_approved1 = a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : "Reject",
                    is_approved2 = a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : "Reject",
                    is_approved3 = a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : "Reject",
                    a.is_final_approve,
                    a.is_deleted,
                    a.comp_mster.company_name,
                    a.company_id,
                    approver_remarks = string.Concat(a.approval1_remarks ?? "", a.approval2_remarks ?? "", a.approval3_remarks ?? "", a.admin_remarks ?? ""),
                    location = a.location,
                    longitude = a.longitude,
                    latitude = a.latitude,
                    isauto_approval = a.is_auto == 0 ? "No" : "Yes",

                }).OrderByDescending(y => y.leave_request_id).ToList();


                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //Get Outdoor Application Request
        [Route("Get_OutdoorApplicationRequest")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.OutdoorApplicationReport))]
        public IActionResult Get_OutdoorApplicationRequest([FromBody]LeaveReport objmodel) // 0 for all employee , 1 for selected emp
        {
            try
            {
                ResponseMsg objResult = new ResponseMsg();

                objmodel.empdtl.RemoveAll(p => Convert.ToInt32(p) < 0 || Convert.ToInt32(p) == 0);

                foreach (var Ids in objmodel.empdtl)
                {
                    if (!_clsCurrentUser.DownlineEmpId.Contains(Convert.ToInt32(Ids)))
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Unauthorize Access...!!";
                        return Ok(objResult);
                    }
                }

                DateTime fromDate = Convert.ToDateTime(objmodel.from_date);
                DateTime toDate = DateTime.Parse(objmodel.to_date);

                var result = _context.tbl_outdoor_request.Where(x => DateTime.Parse(objmodel.from_date).Date <= x.from_date.Date && x.from_date.Date <= DateTime.Parse(objmodel.to_date).Date &&
                ((_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin) ? (objmodel.empdtl.Count > 1 ? true : objmodel.empdtl.Contains(x.r_e_id ?? 0)) : objmodel.empdtl.Contains(x.r_e_id ?? 0))).Select(a => new
                {
                    emp_code = a.tbl_employee_requester.tbl_user_master.FirstOrDefault().username,
                    emp_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                     a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                     a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name),//a.tbl_employee_requester.tbl_emp_officaial_sec.Where(b => b.employee_id == a.r_e_id && b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).Select(c => c.employee_first_name).FirstOrDefault(),

                    dept_Name = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_department_master.department_name,
                    designation = a.tbl_employee_requester.tbl_emp_desi_allocation.FirstOrDefault().tbl_designation_master.designation_name,
                    location = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_location_master.location_name,
                    leave_request_id = a.leave_request_id,
                    from_date = a.from_date,
                    manual_in_time = a.manual_in_time,
                    manual_out_time = a.manual_out_time,
                    day_type = Convert.ToInt32(a.manual_out_time.Subtract(a.manual_in_time).TotalHours) >= 8 ? "Full Day" : Convert.ToInt32(a.manual_out_time.Subtract(a.manual_in_time).TotalHours) >= 4 ? Convert.ToInt32(a.manual_in_time.TimeOfDay.TotalHours) > 14 ? "Second Half" : "First Half" : Convert.ToInt32(a.manual_in_time.TimeOfDay.TotalHours) > 14 ? "Second Half" : "First Half",
                    requester_remarks = a.requester_remarks,
                    status = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : "Reject") : a.is_deleted == 1 ? "Deleted" : a.is_deleted == 2 ? "Cancel" : "",
                    requester_id = a.r_e_id,
                    is_approved1 = a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : "Reject",
                    is_approved2 = a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : "Reject",
                    is_approved3 = a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : "Reject",
                    a.is_final_approve,
                    a.is_deleted,
                    a.comp_mster.company_name,
                    a.requester_date,
                    a.company_id,
                    approved_on = a.approval_date1 >= a.requester_date ? a.approval_date1 : a.approval_date2 >= a.requester_date ? a.approval_date2 : a.approval_date3 >= a.requester_date ? a.approval_date3 : a.admin_ar_date >= a.requester_date ? a.admin_ar_date : a.approval_date1,
                    approved_by = Convert.ToInt32(a.is_approved1) == Convert.ToInt32(1) ? string.Format("{0} {1} {2}",
                                                      a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                      a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                      a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                               Convert.ToInt32(a.is_approved2) == Convert.ToInt32(1) ? string.Format("{0} {1} {2}",
                                                      a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                      a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                      a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                               Convert.ToInt32(a.is_approved3) == Convert.ToInt32(1) ? string.Format("{0} {1} {2}",
                                                      a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                      a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                      a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                               Convert.ToInt32(a.is_admin_approve) == Convert.ToInt32(1) ? string.Format("{0} {1} {2}",
                                                      a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                      a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                      a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) : "",
                    approver_remarks = string.Concat(a.approval1_remarks ?? "", a.approval2_remarks ?? "", a.approval3_remarks ?? "", a.admin_remarks ?? ""),

                    location1 = a.location,
                    longitude = a.longitude,
                    latitude = a.latitude,
                    isauto_approval = a.is_auto == 0 ? "No" : "Yes",
                }).OrderByDescending(y => y.leave_request_id).ToList();


                return Ok(result);

                #region old
                //if (_clsCurrentUser.Is_Hod == 1)
                //{
                //    var manager_emp_list = _context.tbl_emp_manager.Where(a => (a.m_one_id == Employee_Id || a.m_two_id == Employee_Id || a.m_three_id == Employee_Id) || (a.m_one_id == _emp_id || a.m_two_id == _emp_id || a.m_three_id == _emp_id) && a.is_deleted == 0).Select(p => p.employee_id).Distinct().ToList();

                //    if (for_all_emp == 1)
                //    {
                //        var result = _context.tbl_outdoor_request.Where(x => x.r_e_id == Employee_Id && x.company_id==company_id && from_date.Date <= x.from_date.Date && x.from_date.Date <= to_date.Date).Select(a => new {
                //            emp_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                //            a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                //            a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name), //a.tbl_employee_requester.tbl_emp_officaial_sec.Where(b => b.employee_id == a.r_e_id && b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).Select(c => c.employee_first_name).FirstOrDefault(),
                //            leave_request_id = a.leave_request_id,
                //            from_date = a.from_date,
                //            manual_in_time = a.manual_in_time,
                //            manual_out_time = a.manual_out_time,
                //            requester_remarks = a.requester_remarks,
                //            status = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : "Reject") : a.is_deleted == 1 ? "Deleted" : "",
                //            requester_id = a.r_e_id,
                //            is_approved1 = a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : "Reject",
                //            is_approved2 = a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : "Reject",
                //            is_approved3 = a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : "Reject",
                //            a.is_final_approve,
                //            a.is_deleted,
                //        }).OrderByDescending(y=>y.leave_request_id).ToList();



                //            return Ok(result);

                //    }
                //    else
                //    {
                //        var result = _context.tbl_outdoor_request.Where(x => x.company_id == company_id &&(x.r_e_id == _emp_id || manager_emp_list.Contains(x.r_e_id)) 
                //        && from_date.Date <= x.from_date.Date && x.from_date.Date <= to_date.Date).Select(a => new
                //        {
                //            emp_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                //            a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                //            a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name),//a.tbl_employee_requester.tbl_emp_officaial_sec.Where(b => b.employee_id == a.r_e_id && b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).Select(c => c.employee_first_name).FirstOrDefault(),
                //            leave_request_id = a.leave_request_id,
                //            from_date = a.from_date,
                //            manual_in_time = a.manual_in_time,
                //            manual_out_time = a.manual_out_time,
                //            requester_remarks = a.requester_remarks,
                //            status = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : "Reject") : a.is_deleted == 1 ? "Deleted" : "",
                //            requester_id = a.r_e_id,
                //            is_approved1 = a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : "Reject",
                //            is_approved2 = a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : "Reject",
                //            is_approved3 = a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : "Reject",
                //            a.is_deleted,
                //            a.is_final_approve,

                //        }).OrderByDescending(y=>y.leave_request_id).ToList();


                //            return Ok(result);

                //    }

                //}
                //else if (_clsCurrentUser.Is_Hod==2)
                //{
                //    if (company_id > 0)
                //    {
                //        var company_id_ = _context.tbl_employee_company_map.Where(x => x.is_deleted == 0 && x.employee_id == _emp_id && x.company_id == company_id).ToList();

                //        if (company_id_.Count > 0)
                //        {
                //            if (for_all_emp == 1) // only selected employee
                //            {
                //                var result = _context.tbl_outdoor_request.Where(x =>x.r_e_id==Employee_Id && x.company_id == company_id &&
                //           from_date.Date <= x.from_date.Date && x.from_date.Date <= to_date.Date).Select(a => new
                //           {
                //               emp_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                //                   a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                //                   a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name),//a.tbl_employee_requester.tbl_emp_officaial_sec.Where(b => b.employee_id == a.r_e_id && b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).Select(c => c.employee_first_name).FirstOrDefault(),
                //                leave_request_id = a.leave_request_id,
                //               from_date = a.from_date,
                //               manual_in_time = a.manual_in_time,
                //               manual_out_time = a.manual_out_time,
                //               requester_remarks = a.requester_remarks,
                //               status = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : "Reject") : a.is_deleted == 1 ? "Deleted" : "",
                //               requester_id = a.r_e_id,
                //               is_approved1 = a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : "Reject",
                //               is_approved2 = a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : "Reject",
                //               is_approved3 = a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : "Reject",
                //               a.is_final_approve,
                //               a.is_deleted,
                //           }).OrderByDescending(y => y.leave_request_id).ToList();


                //                return Ok(result);
                //            }
                //            else
                //            {
                //                var emp_lst = _context.tbl_employee_company_map.Where(x => x.is_deleted == 0 && x.company_id == company_id).Select(p=>p.employee_id).ToList();

                //                var result = _context.tbl_outdoor_request.Where(x => x.company_id == company_id && emp_lst.Contains(x.r_e_id) &&
                //           from_date.Date <= x.from_date.Date && x.from_date.Date <= to_date.Date).Select(a => new
                //           {
                //               emp_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                //                   a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                //                   a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name),//a.tbl_employee_requester.tbl_emp_officaial_sec.Where(b => b.employee_id == a.r_e_id && b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).Select(c => c.employee_first_name).FirstOrDefault(),
                //                leave_request_id = a.leave_request_id,
                //               from_date = a.from_date,
                //               manual_in_time = a.manual_in_time,
                //               manual_out_time = a.manual_out_time,
                //               requester_remarks = a.requester_remarks,
                //               status = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : "Reject") : a.is_deleted == 1 ? "Deleted" : "",
                //               requester_id = a.r_e_id,
                //               is_approved1 = a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : "Reject",
                //               is_approved2 = a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : "Reject",
                //               is_approved3 = a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : "Reject",
                //               a.is_final_approve,
                //               a.is_deleted,
                //           }).OrderByDescending(y => y.leave_request_id).ToList();


                //                return Ok(result);
                //            }


                //        }
                //        else
                //        {
                //            objResult.Message = "Record Not Found...!!";
                //            objResult.StatusCode = 0;
                //            return Ok(objResult);
                //        }
                //    }
                //    else
                //    {
                //        if (for_all_emp == 1)
                //        {
                //            var result = _context.tbl_outdoor_request.Where(x =>x.r_e_id==Employee_Id  && from_date.Date <= x.from_date.Date && x.from_date.Date <= to_date.Date).Select(a => new {
                //                emp_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                //            a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                //            a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name), //a.tbl_employee_requester.tbl_emp_officaial_sec.Where(b => b.employee_id == a.r_e_id && b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).Select(c => c.employee_first_name).FirstOrDefault(),
                //                leave_request_id = a.leave_request_id,
                //                from_date = a.from_date,
                //                manual_in_time = a.manual_in_time,
                //                manual_out_time = a.manual_out_time,
                //                requester_remarks = a.requester_remarks,
                //                status = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : "Reject") : a.is_deleted == 1 ? "Deleted" : "",
                //                requester_id = a.r_e_id,
                //                is_approved1 = a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : "Reject",
                //                is_approved2 = a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : "Reject",
                //                is_approved3 = a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : "Reject",
                //                is_deleted = a.is_deleted,
                //                a.is_final_approve,

                //            }).OrderByDescending(y => y.leave_request_id).ToList();


                //            return Ok(result);
                //        }
                //        else
                //        {
                //            var result = _context.tbl_outdoor_request.Where(x => from_date.Date <= x.from_date.Date && x.from_date.Date <= to_date.Date).Select(a => new {
                //                emp_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                //            a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                //            a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name), //a.tbl_employee_requester.tbl_emp_officaial_sec.Where(b => b.employee_id == a.r_e_id && b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).Select(c => c.employee_first_name).FirstOrDefault(),
                //                leave_request_id = a.leave_request_id,
                //                from_date = a.from_date,
                //                manual_in_time = a.manual_in_time,
                //                manual_out_time = a.manual_out_time,
                //                requester_remarks = a.requester_remarks,
                //                status = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : "Reject") : a.is_deleted == 1 ? "Deleted" : "",
                //                requester_id = a.r_e_id,
                //                is_approved1 = a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : "Reject",
                //                is_approved2 = a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : "Reject",
                //                is_approved3 = a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : "Reject",
                //                is_deleted = a.is_deleted,
                //                a.is_final_approve,

                //            }).OrderByDescending(y => y.leave_request_id).ToList();


                //            return Ok(result);
                //        }


                //    }
                //}
                //else
                //{


                //    var result = _context.tbl_outdoor_request.Where(a => a.r_e_id == Employee_Id && a.company_id==company_id && from_date.Date <= a.from_date.Date && a.from_date.Date <= to_date.Date).Select(a => new
                //    {
                //        emp_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                //            a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                //            a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name), //a.tbl_employee_requester.tbl_emp_officaial_sec.Where(b => b.employee_id == a.r_e_id && b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).Select(c => c.employee_first_name).FirstOrDefault(),
                //        leave_request_id = a.leave_request_id,
                //        from_date = a.from_date,
                //        manual_in_time = a.manual_in_time,
                //        manual_out_time = a.manual_out_time,
                //        requester_remarks = a.requester_remarks,
                //        status = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : "Reject") : a.is_deleted == 1 ? "Deleted" : "",
                //        requester_id = a.r_e_id,
                //        is_approved1 = a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : "Reject",
                //        is_approved2 = a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : "Reject",
                //        is_approved3 = a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : "Reject",
                //        is_deleted = a.is_deleted,
                //        a.is_final_approve,
                //    }).OrderByDescending(y => y.leave_request_id).ToList();


                //        return Ok(result);

                //}
                #endregion
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        // Get Outdoor Application Request by Id
        [Route("GetOutdoorApplicationRequestbyId/{requestid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.OutdoorApplication))]
        public IActionResult GetOutdoorApplicationRequestbyId([FromRoute] int requestid)
        {
            Response_Msg objresponse = new Response_Msg();
            try
            {
                var data = _context.tbl_outdoor_request.Where(x => x.leave_request_id == requestid).FirstOrDefault();
                if (data != null)
                {
                    if (!_clsCurrentUser.DownlineEmpId.Contains(data.r_e_id ?? 0))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Unauthorize Access...!!";
                        return Ok(objresponse);
                    }
                    else if (!_clsCurrentUser.CompanyId.Contains(data.company_id ?? 0))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Unauthorize Company Access...!!";
                        return Ok(objresponse);
                    }
                    return Ok(data);
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Request";
                    return Ok(objresponse);
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        //Update Outdoor Application Request 
        [Route("Update_OutdoorApplicationRequest")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.OutdoorApplication))]
        public IActionResult Update_OutdoorApplicationRequest([FromBody] tbl_outdoor_request objoutdoor_request)
        {
            Response_Msg objResult = new Response_Msg();
            tbl_outdoor_request obkleavereq = new tbl_outdoor_request();


            try
            {
                var rejected_rqst = _context.tbl_outdoor_request.Where(x => x.leave_request_id != objoutdoor_request.leave_request_id && x.r_e_id == objoutdoor_request.r_e_id && x.manual_in_time == objoutdoor_request.manual_in_time && x.from_date.Date >= objoutdoor_request.from_date.Date && x.manual_out_time <= objoutdoor_request.manual_out_time && x.is_deleted == 0 && x.is_final_approve == 2).OrderByDescending(x => x.leave_request_id).FirstOrDefault();


                var duplicate = _context.tbl_outdoor_request.Where(x => x.leave_request_id != objoutdoor_request.leave_request_id && x.r_e_id == objoutdoor_request.r_e_id && x.manual_in_time == objoutdoor_request.manual_in_time && x.from_date.Date >= objoutdoor_request.from_date.Date && x.manual_out_time <= objoutdoor_request.manual_out_time && x.is_deleted == 0 && x.is_final_approve == 0).OrderByDescending(x => x.leave_request_id).FirstOrDefault();

                if (rejected_rqst != null)
                {

                    var exist = _context.tbl_outdoor_request.Where(x => x.leave_request_id == objoutdoor_request.leave_request_id && x.is_deleted == 0 && (int)x.is_final_approve == 0).FirstOrDefault();
                    if (exist == null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Invalid outdoor request id, please try after refresh page !!";
                        return Ok(objResult);
                    }

                    exist.r_e_id = objoutdoor_request.r_e_id;
                    exist.from_date = objoutdoor_request.from_date;
                    exist.manual_in_time = objoutdoor_request.manual_in_time;
                    exist.manual_out_time = objoutdoor_request.manual_out_time;
                    exist.requester_remarks = objoutdoor_request.requester_remarks;
                    exist.company_id = objoutdoor_request.company_id;
                    exist.requester_date = DateTime.Now;
                    exist.is_auto = 0;
                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Outdoor application request updated successfully !!";
                    MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                    Task task = Task.Run(() => obj_ms.OutdoorApplicationRequestMail(Convert.ToInt32(objoutdoor_request.r_e_id), objoutdoor_request.from_date,
                        objoutdoor_request.manual_in_time, objoutdoor_request.manual_out_time, objoutdoor_request.system_in_time, objoutdoor_request.system_out_time, objoutdoor_request.requester_remarks));
                    task.Wait();
                }

                else if (duplicate != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Outdoor application already applied for the selected date range still in pending stage !!";
                }
                else
                {
                    var exist = _context.tbl_outdoor_request.Where(x => x.leave_request_id == objoutdoor_request.leave_request_id && x.is_deleted == 0 && (int)x.is_final_approve == 0).FirstOrDefault();
                    if (exist == null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Invalid leave request id, please try after refresh page !!";
                    }

                    else
                    {

                        exist.r_e_id = objoutdoor_request.r_e_id;
                        exist.from_date = objoutdoor_request.from_date;
                        exist.manual_in_time = objoutdoor_request.manual_in_time;
                        exist.manual_out_time = objoutdoor_request.manual_out_time;
                        exist.requester_remarks = objoutdoor_request.requester_remarks;
                        exist.company_id = objoutdoor_request.company_id;
                        exist.requester_date = DateTime.Now;
                        exist.is_auto = 0;
                        _context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "Outdoor application request updated successfully !!";
                        MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                        Task task = Task.Run(() => obj_ms.OutdoorApplicationRequestMail(Convert.ToInt32(objoutdoor_request.r_e_id), objoutdoor_request.from_date,
                            objoutdoor_request.manual_in_time, objoutdoor_request.manual_out_time, objoutdoor_request.system_in_time, objoutdoor_request.system_out_time, objoutdoor_request.requester_remarks));
                        task.Wait();
                    }
                }
                //check for valid request id
                return Ok(objResult);

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 1;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        #endregion


        #region Comp Off Application

        [Route("GetEmployeeCompOffData/{EmployeeId}")]
        [HttpGet]
        //[Authorize(Policy = "7058")]
        public IActionResult GetEmployeeCompOffData(int EmployeeId)
        {
            try
            {


                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                ResponseMsg objresponse = new ResponseMsg();


                Classes.clsLoginEmpDtl ob = new clsLoginEmpDtl(_context, Convert.ToInt32(EmployeeId), "read", _AC);

                if (!ob.is_valid())
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unauthorize Access...!";
                    return Ok(objresponse);
                }



                var data = (
                from co in _context.tbl_comp_off_ledger
                where co.e_id == EmployeeId && co.credit == 1
                select new
                {
                    // co.sno,
                    co.compoff_date
                }).Distinct().ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        //Save Comp Off Application
        [Route("Save_CompOffApplicationRequest")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffApplication))]
        public IActionResult Save_CompOffApplicationRequest([FromBody] tbl_comp_off_request_master objcomp_off_requestt)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.DownlineEmpId.Any(p => p == objcomp_off_requestt.r_e_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!";
                return Ok(objResult);
            }

            if (Convert.ToBoolean(FunIsApplicationFreezed()) == true)
            {
                objResult.StatusCode = 1;
                objResult.Message = "Comp Off Application has been freezed for this month";
                return Ok(objResult);
            }


            try
            {
                if (DateTime.Compare(objcomp_off_requestt.compoff_against_date.Date, objcomp_off_requestt.compoff_date.Date) > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Compoff date must be greater than compoff against date...!";
                    return Ok(objResult);
                }

                if (objcomp_off_requestt.is_final_approve != 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid data";
                    return Ok(objResult);
                }

                var freeze_attandance = _context.tbl_daily_attendance.Where(x => x.emp_id == objcomp_off_requestt.r_e_id && x.is_freezed == 1 && x.attendance_dt.Date == objcomp_off_requestt.compoff_date.Date && x.tbl_employee_master.tbl_employee_company_map.FirstOrDefault(q => q.is_deleted == 0).company_id == objcomp_off_requestt.company_id).ToList();
                if (freeze_attandance.Count > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Attandance of selected period is freezed";
                    return Ok(objResult);

                }


                var exist_request = _context.tbl_comp_off_request_master.Where(x => x.r_e_id == objcomp_off_requestt.r_e_id && x.company_id == objcomp_off_requestt.company_id && x.compoff_against_date.Date == objcomp_off_requestt.compoff_against_date.Date && x.is_deleted == 0 && x.is_final_approve != 2).FirstOrDefault();
                if (exist_request != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Compoff request either already approve or pending!!";
                    return Ok(objResult);
                }


                var comp_raise_req_exit = _context.tbl_compoff_raise.Where(x => x.is_deleted == 0 && x.is_final_approve != 1 && x.is_final_approve != 2 && x.comp_off_date.Date == objcomp_off_requestt.compoff_against_date.Date && x.emp_id == objcomp_off_requestt.r_e_id && x.is_final_approve != 1).FirstOrDefault();
                if (comp_raise_req_exit != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Eighter compoff addition request not raise or pending or reject, please check compoff addition request report";
                    return Ok(objResult);
                }

                //end check attandance already freezed or not
                objcomp_off_requestt.requester_date = DateTime.Now;
                objcomp_off_requestt.created_dt = DateTime.Now;
                objcomp_off_requestt.a1_e_id = null;
                objcomp_off_requestt.a2_e_id = null;
                objcomp_off_requestt.a3_e_id = null;
                objcomp_off_requestt.admin_id = null;
                objcomp_off_requestt.is_approved1 = 0;
                objcomp_off_requestt.is_approved2 = 0;
                objcomp_off_requestt.is_approved3 = 0;
                objcomp_off_requestt.is_admin_approve = 0;
                objcomp_off_requestt.is_final_approve = 0;

                _context.Entry(objcomp_off_requestt).State = EntityState.Added;
                _context.SaveChanges();

                objResult.StatusCode = 0;
                objResult.Message = "CompOff application submited successfully...!";

                MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                Task task = Task.Run(() => obj_ms.CompoffApplicationRequestMail(Convert.ToInt32(objcomp_off_requestt.r_e_id), objcomp_off_requestt.compoff_against_date, objcomp_off_requestt.compoff_date, objcomp_off_requestt.requester_remarks));
                // }
                task.Wait();

                return Ok(objResult);
                //   var exist_attendance = _context.tbl_attendace_request.Where(x => x.from_date.Date == objcomp_off_requestt.compoff_date.Date && x.company_id == objcomp_off_requestt.company_id && x.r_e_id == objcomp_off_requestt.r_e_id && x.is_deleted == 0 && x.is_final_approve != 2).FirstOrDefault();
                //   var exist_outdoor = _context.tbl_outdoor_request.OrderByDescending(x => x.leave_request_id).Where(x => x.company_id == objcomp_off_requestt.company_id &&  x.r_e_id == objcomp_off_requestt.r_e_id && x.from_date.Date == objcomp_off_requestt.compoff_date.Date && x.is_deleted == 0 && x.is_final_approve != 2).FirstOrDefault();
                //   //  var exist_leave = _context.tbl_leave_request.FirstOrDefault(x => x.r_e_id == objcomp_off_requestt.r_e_id && x.is_deleted == 0 && x.is_final_approve != 2 &&
                //   //((x.from_date.Date >= objcomp_off_requestt.compoff_date.Date && objcomp_off_requestt.compoff_date.Date <= x.to_date.Date)));

                //   var exist_leave = _context.tbl_leave_request.FirstOrDefault(x => x.r_e_id == objcomp_off_requestt.r_e_id && x.is_deleted == 0 && x.company_id == objcomp_off_requestt.company_id && x.is_final_approve != 2 &&
                //(x.from_date.Date <= objcomp_off_requestt.compoff_date.Date && objcomp_off_requestt.compoff_date.Date <= x.to_date.Date));

                //else if (exist_attendance != null)
                //{
                //    objResult.StatusCode = 1;
                //    objResult.Message = "Raised attandance request either already approved or pending";
                //    return Ok(objResult);
                //}
                //else if (exist_outdoor != null)
                //{
                //    if (exist_outdoor.is_final_approve == 0)
                //    {
                //        objResult.StatusCode = 1;
                //        objResult.Message = "Outdoor application request already exist for this date...!";
                //        return Ok(objResult);
                //    }
                //    else if (exist_outdoor.is_final_approve == 1)
                //    {
                //        objResult.StatusCode = 1;
                //        objResult.Message = "Outdoor application request already approve for this date...!";
                //        return Ok(objResult);
                //    }

                //}
                //else if (exist_leave != null)
                //{
                //    if (exist_leave.is_final_approve == 0)
                //    {
                //        objResult.StatusCode = 1;
                //        objResult.Message = "Leave application request already exist for this date...!";
                //        return Ok(objResult);
                //    }
                //    else if (exist_leave.is_final_approve == 1)
                //    {
                //        objResult.StatusCode = 1;
                //        objResult.Message = "Leave application request already approve for this date...!";
                //        return Ok(objResult);
                //    }
                //}

                //start check attandance already freezed or not





            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        // Get CompOff Application  Request by Id
        [Route("Get_CompOffApplicationRequestById/{requestid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffApplication))]
        public IActionResult Get_CompOffApplicationRequestById([FromRoute] int requestid)
        {
            Response_Msg objresponse = new Response_Msg();
            try
            {
                var data = _context.tbl_comp_off_request_master.Where(x => x.comp_off_request_id == requestid).FirstOrDefault();
                if (data != null)
                {
                    if (!_clsCurrentUser.DownlineEmpId.Contains(data.r_e_id ?? 0))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Unauthorize Access...!!";
                        return Ok(objresponse);
                    }
                    else if (!_clsCurrentUser.CompanyId.Contains(data.company_id ?? 0))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Unauthorize Company Access...!!";
                        return Ok(objresponse);
                    }
                    return Ok(data);
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Request";
                    return Ok(objresponse);
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }



        //Get CompOff Application Request by Emp Id
        [Route("Get_CompOffApplicationRequestByEmpId")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffApplication))]
        public IActionResult Get_CompOffApplicationRequestByEmpId(int emp_id) // 0 for all employee , 1 for selected emp
        {
            try
            {
                ResponseMsg objResult = new ResponseMsg();

                if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access....!!";
                    return Ok(objResult);
                }

                var result = _context.tbl_comp_off_request_master.Where(a => _clsCurrentUser.DownlineEmpId.Contains(a.r_e_id ?? 0) && a.is_deleted == 0 && a.is_final_approve == 0).Select(a => new
                {
                    emp_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                                            a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                                            a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name),//a.tbl_employee_requester.tbl_emp_officaial_sec.Where(b => b.employee_id == a.r_e_id && b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).Select(c => c.employee_first_name).FirstOrDefault(),
                    leave_request_id = a.comp_off_request_id,
                    compoff_date = a.compoff_date,
                    from_date = a.compoff_against_date,
                    manual_in_time = a.compoff_against_date,
                    manual_out_time = a.compoff_against_date,
                    requester_remarks = a.requester_remarks,
                    approver_remarks = string.Concat(a.approval1_remarks ?? "", a.approval2_remarks ?? "", a.approval3_remarks ?? "", a.admin_remarks ?? ""),
                    status = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : "Reject") : a.is_deleted == 1 ? "Deleted" : a.is_deleted == 2 ? "Cancel" : "",
                    requester_id = a.r_e_id,
                    is_approved1 = a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : "Reject",
                    is_approved2 = a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : "Reject",
                    is_approved3 = a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : "Reject",
                    is_deleted = a.is_deleted,
                    is_final_approve = a.is_final_approve,
                }).OrderByDescending(y => y.leave_request_id).ToList();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //Get CompOff Application Request
        [Route("Get_CompOffApplicationRequest")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffApplicationReport))]
        public IActionResult Get_CompOffApplicationRequest([FromBody] LeaveReport objmodel) // 0 for all employee , 1 for selected emp
        {
            try
            {
                ResponseMsg objResult = new ResponseMsg();


                objmodel.empdtl.RemoveAll(p => Convert.ToInt32(p) < 0 || Convert.ToInt32(p) == 0);

                foreach (var ids in objmodel.empdtl)
                {
                    if (!_clsCurrentUser.DownlineEmpId.Contains(Convert.ToInt32(ids)))
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Unauthorize Access....!!";
                        return Ok(objResult);
                    }
                }

                var result = _context.tbl_comp_off_request_master.Where(a => DateTime.Parse(objmodel.from_date).Date <= a.compoff_against_date.Date && a.compoff_against_date.Date <= DateTime.Parse(objmodel.to_date).Date && ((_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin) ? (objmodel.empdtl.Count > 1 ? true : objmodel.empdtl.Contains(a.r_e_id ?? 0)) : objmodel.empdtl.Contains(a.r_e_id ?? 0))).Select(a => new
                {
                    emp_code = a.tbl_employee_requester.emp_code,
                    emp_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                                 a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                                 a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name),//a.tbl_employee_requester.tbl_emp_officaial_sec.Where(b => b.employee_id == a.r_e_id && b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).Select(c => c.employee_first_name).FirstOrDefault(),
                    dept_name = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).tbl_department_master.department_name,
                    designation = a.tbl_employee_requester.tbl_emp_desi_allocation.FirstOrDefault().tbl_designation_master.designation_name,
                    location = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_location_master.location_name,
                    leave_request_id = a.comp_off_request_id,
                    compoff_date = a.compoff_date.ToString("dd/MMM/yyyy"),
                    compoff_day = a.compoff_date.DayOfWeek.ToString(),
                    duration = a.compoff_date.Subtract(a.compoff_date).TotalDays + 1,
                    compoff_date_in = a.compoff_date,
                    compoff_date_out = a.compoff_date,
                    from_date = a.compoff_against_date,
                    manual_in_time = a.compoff_against_date,
                    manual_out_time = a.compoff_against_date,
                    requester_remarks = a.requester_remarks,
                    approver_remarks = string.Concat(a.approval1_remarks ?? "", a.approval2_remarks ?? "", a.approval3_remarks ?? "", a.admin_remarks ?? ""),
                    status = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : "Reject") : a.is_deleted == 1 ? "Deleted" : a.is_deleted == 2 ? "Cancel" : "",
                    approved_by = Convert.ToInt32(a.is_approved1) == 1 ? string.Format("{0} {1} {2}", a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                                 a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                                 a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name) :
                                 Convert.ToInt32(a.is_approved2) == 1 ? string.Format("{0} {1} {2}", a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                                 a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                                 a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name) :
                                 Convert.ToInt32(a.is_approved3) == 1 ? string.Format("{0} {1} {2}", a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                                 a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                                 a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name) :
                                 Convert.ToInt32(a.is_admin_approve) == 1 ? string.Format("{0} {1} {2}", a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                                 a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                                 a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name) : " ",
                    applied_on = a.requester_date,
                    requester_id = a.r_e_id,
                    is_approved1 = a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : "Reject",
                    is_approved2 = a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : "Reject",
                    is_approved3 = a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : "Reject",
                    is_deleted = a.is_deleted,
                    is_final_approve = a.is_final_approve,
                }).OrderByDescending(y => y.leave_request_id).ToList();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Update_CompOffApplicationRequest")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffApplication))]
        public IActionResult Update_CompOffApplicationRequest([FromBody] tbl_comp_off_request_master objcomp_off_requestt)
        {
            Response_Msg objResult = new Response_Msg();
            tbl_comp_off_request_master obkleavereq = new tbl_comp_off_request_master();

            if (!_clsCurrentUser.DownlineEmpId.Any(p => p == objcomp_off_requestt.r_e_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!";
                return Ok(objResult);
            }


            try
            {
                if (DateTime.Compare(objcomp_off_requestt.compoff_against_date.Date, objcomp_off_requestt.compoff_date.Date) > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Compoff date must be greater than compoff against date...!";
                    return Ok(objResult);
                }

                if (objcomp_off_requestt.is_final_approve != 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid data";
                    return Ok(objResult);
                }

                var freeze_attandance = _context.tbl_daily_attendance.Where(x => x.emp_id == objcomp_off_requestt.r_e_id && x.is_freezed == 1 && x.attendance_dt.Date == objcomp_off_requestt.compoff_date.Date && x.tbl_employee_master.tbl_employee_company_map.FirstOrDefault(q => q.is_deleted == 0).company_id == objcomp_off_requestt.company_id).ToList();
                if (freeze_attandance.Count > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Attandance of selected period is freezed";
                    return Ok(objResult);

                }

                //end check attandance already freezed or not


                var rejected_rqst = _context.tbl_comp_off_request_master.Where(x => x.comp_off_request_id != objcomp_off_requestt.comp_off_request_id && x.r_e_id == objcomp_off_requestt.r_e_id && x.compoff_date.Date == objcomp_off_requestt.compoff_date.Date && x.compoff_against_date.Date == objcomp_off_requestt.compoff_against_date.Date && x.is_deleted == 0 && x.is_final_approve == 2).OrderByDescending(x => x.comp_off_request_id).FirstOrDefault();

                var duplicate = _context.tbl_comp_off_request_master.Where(x => x.comp_off_request_id != objcomp_off_requestt.comp_off_request_id && x.r_e_id == objcomp_off_requestt.r_e_id && x.compoff_date.Date == objcomp_off_requestt.compoff_date.Date && x.compoff_against_date.Date == objcomp_off_requestt.compoff_against_date.Date && x.is_deleted == 0 && x.is_final_approve == 0).OrderByDescending(x => x.comp_off_request_id).FirstOrDefault();

                if (rejected_rqst != null)
                {
                    var exist = _context.tbl_comp_off_request_master.Where(x => x.comp_off_request_id == objcomp_off_requestt.comp_off_request_id && x.is_deleted == 0 && (int)x.is_final_approve == 0).FirstOrDefault();
                    if (exist == null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Invalid CompOff request id, please try after refresh page !!";
                        return Ok(objResult);
                    }

                    exist.r_e_id = objcomp_off_requestt.r_e_id;
                    exist.compoff_against_date = objcomp_off_requestt.compoff_against_date;
                    exist.compoff_date = objcomp_off_requestt.compoff_date;
                    exist.compoff_request_qty = objcomp_off_requestt.compoff_request_qty;
                    exist.requester_remarks = objcomp_off_requestt.requester_remarks;
                    exist.requester_date = DateTime.Now;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "CompOff application updated successfully !!";

                    MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);
                    Task task = Task.Run(() => obj_ms.CompoffApplicationRequestMail(Convert.ToInt32(objcomp_off_requestt.r_e_id), objcomp_off_requestt.compoff_against_date, objcomp_off_requestt.compoff_date, objcomp_off_requestt.requester_remarks));
                    task.Wait();
                }

                else if (duplicate != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "CompOff application already applied for the selected date still in pending stage !!";
                }
                else
                {
                    var exist = _context.tbl_comp_off_request_master.Where(x => x.comp_off_request_id == objcomp_off_requestt.comp_off_request_id && x.is_deleted == 0 && (int)x.is_final_approve == 0).FirstOrDefault();
                    if (exist == null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Invalid CompOff request id, please try after refresh page !!";
                    }

                    else
                    {

                        exist.r_e_id = objcomp_off_requestt.r_e_id;
                        exist.compoff_against_date = objcomp_off_requestt.compoff_against_date;
                        exist.compoff_date = objcomp_off_requestt.compoff_date;
                        exist.compoff_request_qty = objcomp_off_requestt.compoff_request_qty;
                        exist.requester_remarks = objcomp_off_requestt.requester_remarks;
                        exist.requester_date = DateTime.Now;

                        _context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "CompOff application updated successfully !!";

                        MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);
                        Task task = Task.Run(() => obj_ms.CompoffApplicationRequestMail(Convert.ToInt32(objcomp_off_requestt.r_e_id), objcomp_off_requestt.compoff_against_date, objcomp_off_requestt.compoff_date, objcomp_off_requestt.requester_remarks));
                        task.Wait();
                    }
                }
                //check for valid request id
                return Ok(objResult);

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 1;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }




        [Route("GetInOrOutTimeByDate/{Date}/{EmployeeId}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.AttendanceApplication))]
        public IActionResult GetInOrOutTimeByDate(DateTime Date, int EmployeeId)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(EmployeeId))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access....!!";
                return Ok(objresponse);
            }

            try
            {
                var data = (
                from da in _context.tbl_daily_attendance
                where da.emp_id == EmployeeId && da.attendance_dt == Date
                select new
                {
                    da.in_time,
                    da.out_time
                }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }
        #endregion
        #region Attendence Application
        //Save Attendance Application
        [Route("Save_AttendanceApplicationRequest")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.AttendanceApplication))]
        public IActionResult Save_AttendanceApplicationRequest([FromBody] tbl_attendace_request objattendace_request)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.DownlineEmpId.Any(p => p == objattendace_request.r_e_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!";
                return Ok(objResult);
            }

            if (Convert.ToBoolean(FunIsApplicationFreezed()) == true)
            {
                objResult.StatusCode = 1;
                objResult.Message = "Attendence Application has been freezed for this month";
                return Ok(objResult);
            }

            if (!_clsCurrentUser.CompanyId.Any(p => p == objattendace_request.company_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!";
                return Ok(objResult);
            }

            try
            {
                if (objattendace_request.is_final_approve != 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid data";
                    return Ok(objResult);
                }

                if (DateTime.Compare(objattendace_request.manual_in_time, objattendace_request.manual_out_time) > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Out Time must be greater than in time";
                    return Ok(objResult);
                }

                //objattendace_request.manual_in_time = objattendace_request.from_date.AddHours(objattendace_request.manual_in_time.Hour).AddMinutes(objattendace_request.manual_in_time.Minute);
                // objattendace_request.manual_out_time = objattendace_request.from_date.AddHours(objattendace_request.manual_out_time.Hour).AddMinutes(objattendace_request.manual_out_time.Minute);

                objattendace_request.system_in_time = objattendace_request.from_date.AddHours(objattendace_request.system_in_time.Hour).AddMinutes(objattendace_request.system_in_time.Minute);
                objattendace_request.system_out_time = objattendace_request.from_date.AddHours(objattendace_request.system_out_time.Hour).AddMinutes(objattendace_request.system_out_time.Minute);



                //start check attandance already freezed or not

                var freeze_attandance = _context.tbl_daily_attendance.Where(x => x.emp_id == objattendace_request.r_e_id && x.tbl_employee_master.tbl_employee_company_map.FirstOrDefault(q => q.is_deleted == 0).company_id == objattendace_request.company_id && x.is_freezed == 1 && x.attendance_dt.Date == objattendace_request.from_date.Date).ToList();

                if (freeze_attandance.Count > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Attandance of selected period is freezed";
                    return Ok(objResult);

                }
                //end check attandance already freezed or not



                var exist_att_req = _context.tbl_attendace_request.Where(x => x.r_e_id == objattendace_request.r_e_id && x.company_id == objattendace_request.company_id && x.is_deleted == 0 && x.is_final_approve != 2).ToList();

                // cannot raise unlimited application in month for regularize


                //START get attadence detail according to payroll month circle
                var get_payroll_circle = _context.tbl_payroll_month_setting.Where(a => a.is_deleted == 0 && a.company_id == objattendace_request.company_id).OrderByDescending(x => x.payroll_month_setting_id).FirstOrDefault();

                var getmonth = objattendace_request.from_date.Month;
                var getyear = objattendace_request.from_date.Year;
                DateTime nowdate = Convert.ToDateTime(getyear + "-" + getmonth + "-" + (get_payroll_circle != null ? get_payroll_circle.from_date : 1));
                DateTime FromDate = new DateTime();
                //DateTime ToDate = new DateTime();

                //if (objattendace_request.from_date.Date >= nowdate.Date)
                //{
                //    FromDate = nowdate;

                //    DateTime todate_ = nowdate.AddMonths(1);
                //    ToDate = Convert.ToDateTime(todate_.Year.ToString() + "-" + todate_.Month.ToString() + "-" + (get_payroll_circle != null ? get_payroll_circle.from_date - 1 : 30));
                //}
                //else
                //{
                //    FromDate = Convert.ToDateTime(nowdate.AddMonths(-1).ToString("dd-MMM-yyyy 00:00:00 tt"));
                //    ToDate = Convert.ToDateTime(getyear.ToString() + "-" + getmonth.ToString() + "-" + (get_payroll_circle != null ? get_payroll_circle.from_date - 1 : 30));
                //}

                ////FromDate = Convert.ToDateTime(nowdate.AddMonths(-1).ToString("dd-MMM-yyyy 00:00:00 tt"));

                ////ToDate = Convert.ToDateTime(getyear.ToString() + "-" + getmonth.ToString() + "-" + (get_payroll_circle != null ? get_payroll_circle.from_date - 1 : 30));

                ////Convert.ToInt32(_config["attandance_regularize_pm"]);

                //DateTime from26thofmonth = new DateTime();
                //DateTime to25thofmonth = new DateTime();

                //var selectedfromdate = objattendace_request.from_date.Date;

                //if (objattendace_request.from_date.Date.Day <= 25)
                //{
                //    var prvmonth = selectedfromdate.AddMonths(-1);
                //    from26thofmonth = Convert.ToDateTime(prvmonth.Year.ToString() + "-" + prvmonth.Month + "-" + "26");
                //    to25thofmonth = Convert.ToDateTime(selectedfromdate.Year.ToString() + "-" + selectedfromdate.Month + "-" + "25");
                //}
                //else
                //{
                //    var nextmonth = selectedfromdate.AddMonths(1);
                //    from26thofmonth = Convert.ToDateTime(selectedfromdate.Year.ToString() + "-" + selectedfromdate.Month + "-" + "26");
                //    to25thofmonth = Convert.ToDateTime(nextmonth.Year.ToString() + "-" + nextmonth.Month + "-" + "25");
                //}

                //// var current_month_att = exist_att_req.Where(x => FromDate.Date <= x.from_date.Date && x.from_date.Date <= ToDate.Date).ToList();
                //var current_month_att = exist_att_req.Where(x => x.from_date.Date >= from26thofmonth && x.from_date.Date <= to25thofmonth).ToList();

                //if (current_month_att.Count >= Convert.ToInt32(_config["attandance_regularize_pm"]))
                //{
                //    objResult.StatusCode = 1;
                //    objResult.Message = "Cannot raise more than " + _config["attandance_regularize_pm"] + " in a month (26th to 25th)";
                //    return Ok(objResult);
                //}

                //end

                var exist_request = exist_att_req.Where(x => x.from_date.Date == objattendace_request.from_date.Date).FirstOrDefault();

                if (exist_request != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Raised attandance request either already approved or pending";
                    return Ok(objResult);
                }

                if (objattendace_request.from_date.Date > DateTime.Now.Date)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Regularize Date cannot be greater than Today date";
                    return Ok(objResult);
                }
                //check that date attendance availble or not

                //var exist_attend = _context.tbl_daily_attendance.Where(x => x.emp_id == objattendace_request.r_e_id && x.tbl_employee_master.tbl_employee_company_map.FirstOrDefault(q => q.is_deleted == 0).company_id == objattendace_request.company_id && x.attendance_dt.Date == objattendace_request.from_date.Date).ToList();
                //if (exist_attend.Count == 0)
                //{
                //    objResult.StatusCode = 1;
                //    objResult.Message = "Selected Regularize Date attandance not availble";
                //    return Ok(objResult);
                //}


                objattendace_request.requester_date = DateTime.Now;
                objattendace_request.created_dt = DateTime.Now;
                objattendace_request.is_final_approve = 0;
                objattendace_request.a1_e_id = null;
                objattendace_request.a2_e_id = null;
                objattendace_request.a3_e_id = null;
                objattendace_request.admin_id = null;
                objattendace_request.is_approved1 = 0;
                objattendace_request.is_approved2 = 0;
                objattendace_request.is_approved3 = 0;

                _context.Entry(objattendace_request).State = EntityState.Added;
                _context.SaveChanges();

                objResult.StatusCode = 0;
                objResult.Message = "Attendance Application Submited Successfully...!";

                MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);
                Task task = Task.Run(() => obj_ms.AttendanceApplicationRequestMail(Convert.ToInt32(objattendace_request.r_e_id), objattendace_request.from_date, objattendace_request.manual_in_time, objattendace_request.manual_out_time, objattendace_request.requester_remarks));

                task.Wait();
                //   }
                return Ok(objResult);


                // var exist_outdoor = _context.tbl_outdoor_request.OrderByDescending(x => x.leave_request_id).Where(x => x.r_e_id == objattendace_request.r_e_id && x.company_id==objattendace_request.company_id && x.from_date == objattendace_request.from_date && x.is_deleted == 0 && x.is_final_approve != 2).FirstOrDefault();

                // var exist_leave = _context.tbl_leave_request.FirstOrDefault(x => x.r_e_id == objattendace_request.r_e_id && x.company_id==objattendace_request.company_id && x.is_deleted == 0 && x.is_final_approve != 2 &&
                //(objattendace_request.from_date.Date >= x.from_date.Date && objattendace_request.from_date.Date <= x.to_date.Date));
                // //((x.from_date.Date >= objattendace_request.from_date.Date && objattendace_request.from_date.Date <= x.to_date.Date)));

                // var exist_CompOff = _context.tbl_comp_off_request_master.Where(x => x.r_e_id == objattendace_request.r_e_id && x.company_id==objattendace_request.company_id && x.compoff_against_date == objattendace_request.from_date && x.is_deleted == 0 && x.is_final_approve != 2).FirstOrDefault();

                //else if (exist_outdoor != null)
                //{
                //    if (exist_outdoor.is_final_approve == 0)
                //    {
                //        objResult.StatusCode = 1;
                //        objResult.Message = "Outdoor application request already exist for this date...!";
                //        return Ok(objResult);
                //    }
                //    else if (exist_outdoor.is_final_approve == 1)
                //    {
                //        objResult.StatusCode = 1;
                //        objResult.Message = "Outdoor application request already approve for this date...!";
                //        return Ok(objResult);
                //    }

                //}
                //else if (exist_leave != null)
                //{
                //    if (exist_leave.is_final_approve == 0)
                //    {
                //        objResult.StatusCode = 1;
                //        objResult.Message = "Leave application request already exist for this date...!";
                //        return Ok(objResult);
                //    }
                //    else if (exist_leave.is_final_approve == 1)
                //    {
                //        objResult.StatusCode = 1;
                //        objResult.Message = "Leave application request already approve for this date...!";
                //        return Ok(objResult);
                //    }
                //}
                //else if (exist_CompOff != null)
                //{
                //    objResult.StatusCode = 1;
                //    objResult.Message = "Compoff request either alerady approve or pending!!";
                //    return Ok(objResult);
                //}






            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }


        // Get Attendence Application Request by Id
        [Route("Get_AttendanceApplicationRequestById/{requestid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.AttendanceApplication))]
        public IActionResult Get_AttendanceApplicationRequestById([FromRoute] int requestid)
        {
            Response_Msg objresponse = new Response_Msg();
            try
            {
                var data = _context.tbl_attendace_request.Where(x => x.leave_request_id == requestid).FirstOrDefault();
                if (data != null)
                {
                    if (!_clsCurrentUser.DownlineEmpId.Contains(data.r_e_id ?? 0))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Unauthorize Access...!!";
                        return Ok(objresponse);
                    }
                    else if (!_clsCurrentUser.CompanyId.Contains(data.company_id ?? 0))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Unauthorize Company Access...!!";
                        return Ok(objresponse);
                    }
                    return Ok(data);
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Request";
                    return Ok(objresponse);
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }


        //Get Attendance Application Request by Emp id
        [Route("Get_AttendanceApplicationByEmpId")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.AttendanceApplication))]
        public IActionResult Get_AttendanceApplicationByEmpId(int emp_id) // 0 for all employee , 1 for selected emp
        {
            ResponseMsg objResult = new ResponseMsg();
            try
            {
                if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize access...!!";
                    return Ok(objResult);
                }

                var result = (from a in _context.tbl_attendace_request
                              join b in _context.tbl_daily_attendance on new { _date = a.from_date, _empidd = a.r_e_id } equals new { _date = b.attendance_dt, _empidd = b.emp_id } //into ej
                              //from d in ej.DefaultIfEmpty()
                              where (_clsCurrentUser.DownlineEmpId.Contains(a.r_e_id ?? 0) && a.is_deleted == 0 && a.is_final_approve == 0)
                              select new
                              {
                                  emp_code = a.tbl_employee_requester.emp_code,
                                  emp_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                                             a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                                                 a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name), //a.tbl_employee_requester.tbl_emp_officaial_sec.Where(b => b.employee_id == a.r_e_id && b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).Select(c => c.employee_first_name).FirstOrDefault(),
                                  leave_request_id = a.leave_request_id,
                                  from_date = a.from_date,
                                  manual_in_time = a.manual_in_time,
                                  manual_out_time = a.manual_out_time,
                                  a.r_e_id,
                                  requester_remarks = a.requester_remarks,
                                  status = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : "Reject") : a.is_deleted == 1 ? "Deleted" : a.is_deleted == 2 ? "Cancel" : "",
                                  requester_id = a.r_e_id,
                                  system_in_time = b.in_time ,//d != null ?  d.in_time : new DateTime(2000, 01, 01),
                                  system_out_time = b.out_time, //d != null ? d.out_time : new DateTime(2000, 01, 01),
                                  is_approved1 = a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : "Reject",
                                  is_approved2 = a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : "Reject",
                                  is_approved3 = a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : "Reject",
                                  is_delted = a.is_deleted,
                                  is_final_approve = a.is_final_approve,
                                  a.company_id,
                                  approver_remarks = string.Concat(a.approval1_remarks ?? "", a.approval2_remarks ?? "", a.approval3_remarks ?? "", a.admin_remarks ?? ""),
                              }).ToList();


                return Ok(result);

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 1;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }


        //Get Attendance Application Request
        [Route("Get_AttendanceApplicationRequest")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.AttendanceApplicationReport))]
        public IActionResult Get_AttendanceApplicationRequest([FromBody] LeaveReport objmodel) // 0 for all employee , 1 for selected emp
        {
            DateTime d_DefaultDt = new DateTime(2000, 1, 1);
            ResponseMsg objResult = new ResponseMsg();
            try
            {


                objmodel.empdtl.RemoveAll(x => Convert.ToInt32(x) < 0 || Convert.ToInt32(x) == 0);

                foreach (var ids in objmodel.empdtl)
                {
                    if (!_clsCurrentUser.DownlineEmpId.Contains(Convert.ToInt32(ids)))
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Unauthorize access...!!";
                        return Ok(objResult);
                    }
                }


                //var attendance_dtl = _context.tbl_daily_attendance.Where(x => DateTime.Parse(objmodel.from_date).Date <= x.attendance_dt.Date && x.attendance_dt.Date <= DateTime.Parse(objmodel.to_date).Date &&
                //((_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin) ? (objmodel.empdtl.Count > 1 ? true : objmodel.empdtl.Contains(x.emp_id ?? 0)) : objmodel.empdtl.Contains(x.emp_id ?? 0))).Select(p => new
                //{
                //    p.attendance_dt,
                //    p.emp_id,
                //    p.in_time,
                //    p.out_time,
                //    p.shift_in_time,
                //    p.shift_out_time,
                //}).ToList();

                //   var result = _context.tbl_attendace_request.Where(x => DateTime.Parse(objmodel.from_date).Date <= x.from_date.Date && x.from_date.Date <= DateTime.Parse(objmodel.to_date).Date &&
                //  ((_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin) ? (objmodel.empdtl.Count > 1 ? true : objmodel.empdtl.Contains(x.r_e_id ?? 0)) : objmodel.empdtl.Contains(x.r_e_id ?? 0))).Select(a => new

                var result = (from a in _context.tbl_attendace_request
                              join b in _context.tbl_daily_attendance on new { _date = a.from_date.Date, _empidd = a.r_e_id } equals new { _date = b.attendance_dt.Date, _empidd = b.emp_id } into ej
                              from d in ej.DefaultIfEmpty()
                                  // join d in attendance_dtl on a.r_e_id equals d.emp_id
                              where DateTime.Parse(objmodel.from_date).Date <= a.from_date.Date && a.from_date.Date <= DateTime.Parse(objmodel.to_date).Date &&
                   ((_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin) ? (objmodel.empdtl.Count > 1 ? true : objmodel.empdtl.Contains(a.r_e_id ?? 0)) : objmodel.empdtl.Contains(a.r_e_id ?? 0))
                              select new
                              {
                                  emp_code = a.tbl_employee_requester.emp_code,
                                  emp_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                                                         a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                                                             a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name), //a.tbl_employee_requester.tbl_emp_officaial_sec.Where(b => b.employee_id == a.r_e_id && b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).Select(c => c.employee_first_name).FirstOrDefault(),
                                  dept_name = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).tbl_department_master.department_name,
                                  designation = a.tbl_employee_requester.tbl_emp_desi_allocation.FirstOrDefault().tbl_designation_master.designation_name,
                                  location = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_location_master.location_name,
                                  leave_request_id = a.leave_request_id,
                                  from_date = a.from_date,
                                  manual_in_time = a.manual_in_time,
                                  manual_out_time = a.manual_out_time,
                                  date = a.from_date,
                                  shift_Name = _context.tbl_shift_master.Where(x => x.shift_id == Convert.ToInt32(a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id)).FirstOrDefault().tbl_shift_details == null ? "" : _context.tbl_shift_master.Where(x => x.shift_id == Convert.ToInt32(a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id)).FirstOrDefault().tbl_shift_details.FirstOrDefault(z => z.is_deleted == 0).shift_name,
                                  shift_in_time = _context.tbl_shift_master.Where(x => x.shift_id == Convert.ToInt32(a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id)).FirstOrDefault().tbl_shift_details != null ? _context.tbl_shift_master.Where(x => x.shift_id == Convert.ToInt32(a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id)).FirstOrDefault().tbl_shift_details.FirstOrDefault(z => z.is_deleted == 0).punch_in_time : new DateTime(2000, 01, 01),
                                  shift_out_time = _context.tbl_shift_master.Where(x => x.shift_id == Convert.ToInt32(a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id)).FirstOrDefault().tbl_shift_details != null ? _context.tbl_shift_master.Where(x => x.shift_id == Convert.ToInt32(a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id)).FirstOrDefault().tbl_shift_details.FirstOrDefault(z => z.is_deleted == 0).punch_out_time : new DateTime(2000, 01, 01),
                                  //shift_Name = _context.tbl_shift_master.Where(x => x.shift_id == a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id).FirstOrDefault().tbl_shift_details.FirstOrDefault(z => z.is_deleted == 0).shift_name,
                                  //shift_in_time = _context.tbl_shift_master.Where(x => x.shift_id == a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id).FirstOrDefault().tbl_shift_details.FirstOrDefault(z => z.is_deleted == 0).punch_in_time,
                                  //shift_out_time = _context.tbl_shift_master.Where(x => x.shift_id == a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id).FirstOrDefault().tbl_shift_details.FirstOrDefault(z => z.is_deleted == 0).punch_out_time,
                                  actual_in_time = d != null ? d.in_time : d_DefaultDt,
                                  actual_out_time = d != null ? d.out_time : d_DefaultDt,
                                  actual_working_hour = d != null ? d.out_time.Subtract(d.in_time).TotalHours.ToString() : "0",
                                  //     actual_working_hour = d != null ? Convert.ToInt32(d.out_time.Subtract(d.in_time).TotalHours / 8.5).ToString() + ":" + Convert.ToInt32(d.out_time.Subtract(d.in_time).TotalHours % 8.5).ToString() : "0",
                                  actual_status = d != null ? d.out_time.Subtract(d.in_time).TotalHours >= 9 ? "Present" : d.out_time.Subtract(d.in_time).TotalHours >= 4.5 ? "Half Day" : "Absent" : "Absent",
                                  in_time = a.manual_in_time,
                                  out_time = a.manual_out_time,
                                  worked_hour = a.manual_out_time.Subtract(a.manual_in_time).TotalHours,
                                  new_status = a.manual_out_time.Subtract(a.manual_in_time).TotalHours >= 9 ? "Present" : a.manual_out_time.Subtract(a.manual_in_time).TotalHours >= 4.5 ? "Half Day" : "Absent",
                                  requester_remarks = a.requester_remarks,
                                  status = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : "Reject") : a.is_deleted == 1 ? "Deleted" : a.is_deleted == 2 ? "Cancel" : "",
                                  applied_on = a.requester_date,
                                  approved_on = a.approval_date1 >= a.requester_date ? a.approval_date1 : a.approval_date2 >= a.requester_date ? a.approval_date2 : a.approval_date3 >= a.requester_date ? a.approval_date3 : a.admin_ar_date >= a.requester_date ? a.admin_ar_date : a.approval_date1,
                                  approved_by = Convert.ToInt32(a.is_approved1) == Convert.ToInt32(1) ? string.Format("{0} {1} {2}",
                                                                      a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                            a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                            a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                                     Convert.ToInt32(a.is_approved2) == Convert.ToInt32(1) ? string.Format("{0} {1} {2}",
                                                            a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                            a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                            a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                                     Convert.ToInt32(a.is_approved3) == Convert.ToInt32(1) ? string.Format("{0} {1} {2}",
                                                            a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                            a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                            a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                                     Convert.ToInt32(a.is_admin_approve) == Convert.ToInt32(1) ? string.Format("{0} {1} {2}",
                                                            a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                            a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                            a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) : "",
                                  requester_id = a.r_e_id,
                                  system_in_time = d.in_time,
                                  system_out_time = d.out_time,
                                  is_approved1 = a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : "Reject",
                                  is_approved2 = a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : "Reject",
                                  is_approved3 = a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : "Reject",
                                  is_delted = a.is_deleted,
                                  is_final_approve = a.is_final_approve,
                                  a.company_id,
                                  approver_remarks = string.Concat(a.approval1_remarks ?? "", a.approval2_remarks ?? "", a.approval3_remarks ?? "", a.admin_remarks ?? "")
                              }).ToList();

                //var result11 = (from a in _context.tbl_attendace_request
                //                join b in attendance_dtl on new { _date = a.from_date.Date, _empidd = a.r_e_id } equals new { _date = b.attendance_dt.Date, _empidd = b.emp_id } into ej
                //                // where ((_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin) ? (objmodel.empdtl.Count > 1 ? true : objmodel.empdtl.Contains(a.r_e_id ?? 0)) : objmodel.empdtl.Contains(a.r_e_id ?? 0))
                //                from d in ej.DefaultIfEmpty()
                //                where DateTime.Parse(objmodel.from_date).Date <= a.from_date.Date && a.from_date.Date <= DateTime.Parse(objmodel.to_date).Date
                //                //&& (objmodel.empdtl.Count > 1 ? true : objmodel.empdtl.Contains(a.r_e_id ?? 0))
                //                select new
                //                {
                //                    emp_code = a.tbl_employee_requester.emp_code,
                //                    emp_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                //                               a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                //                                   a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name), //a.tbl_employee_requester.tbl_emp_officaial_sec.Where(b => b.employee_id == a.r_e_id && b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).Select(c => c.employee_first_name).FirstOrDefault(),
                //                    dept_name = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).tbl_department_master.department_name,
                //                    designation = a.tbl_employee_requester.tbl_emp_desi_allocation.FirstOrDefault().tbl_designation_master.designation_name,
                //                    location = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_location_master.location_name,
                //                    leave_request_id = a.leave_request_id,
                //                    from_date = a.from_date,
                //                    manual_in_time = a.manual_in_time,
                //                    manual_out_time = a.manual_out_time,
                //                    date = a.from_date,
                //                    shift_Name = _context.tbl_shift_master.Where(x => x.shift_id == Convert.ToInt32(a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id)).FirstOrDefault().tbl_shift_details == null ? "" : _context.tbl_shift_master.Where(x => x.shift_id == Convert.ToInt32(a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id)).FirstOrDefault().tbl_shift_details.FirstOrDefault(z => z.is_deleted == 0).shift_name,
                //                    shift_in_time = _context.tbl_shift_master.Where(x => x.shift_id == Convert.ToInt32(a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id)).FirstOrDefault().tbl_shift_details != null ? _context.tbl_shift_master.Where(x => x.shift_id == Convert.ToInt32(a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id)).FirstOrDefault().tbl_shift_details.FirstOrDefault(z => z.is_deleted == 0).punch_in_time : new DateTime(2000, 01, 01),
                //                    shift_out_time = _context.tbl_shift_master.Where(x => x.shift_id == Convert.ToInt32(a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id)).FirstOrDefault().tbl_shift_details != null ? _context.tbl_shift_master.Where(x => x.shift_id == Convert.ToInt32(a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id)).FirstOrDefault().tbl_shift_details.FirstOrDefault(z => z.is_deleted == 0).punch_out_time : new DateTime(2000, 01, 01),
                //                    //shift_Name = _context.tbl_shift_master.Where(x => x.shift_id == a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id).FirstOrDefault().tbl_shift_details.FirstOrDefault(z => z.is_deleted == 0).shift_name,
                //                    //shift_in_time = _context.tbl_shift_master.Where(x => x.shift_id == a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id).FirstOrDefault().tbl_shift_details.FirstOrDefault(z => z.is_deleted == 0).punch_in_time,
                //                    //shift_out_time = _context.tbl_shift_master.Where(x => x.shift_id == a.tbl_employee_requester.tbl_emp_shift_allocation.FirstOrDefault().shift_id).FirstOrDefault().tbl_shift_details.FirstOrDefault(z => z.is_deleted == 0).punch_out_time,
                //                    actual_in_time = d != null ? d.in_time : new DateTime(2000, 01, 01),
                //                    actual_out_time = d != null ? d.out_time : new DateTime(2000, 01, 01),
                //                    actual_working_hour = d != null ? d.out_time.Subtract(d.in_time).TotalHours.ToString() : "0",
                //                    //     actual_working_hour = d != null ? Convert.ToInt32(d.out_time.Subtract(d.in_time).TotalHours / 8.5).ToString() + ":" + Convert.ToInt32(d.out_time.Subtract(d.in_time).TotalHours % 8.5).ToString() : "0",
                //                    actual_status = d != null ? d.out_time.Subtract(d.in_time).TotalHours >= 4 ? "Present" : d.out_time.Subtract(d.in_time).TotalHours >= 4 ? "Half Day" : "Absent" : "Absent",
                //                    in_time = a.manual_in_time,
                //                    out_time = a.manual_out_time,
                //                    worked_hour = a.manual_out_time.Subtract(a.manual_in_time).TotalHours,
                //                    new_status = a.manual_out_time.Subtract(a.manual_in_time).TotalHours >= 8.5 ? "Present" : a.manual_out_time.Subtract(a.manual_in_time).TotalHours >= 4 ? "Half Day" : "Absent",
                //                    requester_remarks = a.requester_remarks,
                //                    status = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : "Reject") : a.is_deleted == 1 ? "Deleted" : a.is_deleted == 2 ? "Cancel" : "",
                //                    applied_on = a.requester_date,
                //                    approved_on = a.approval_date1 >= a.requester_date ? a.approval_date1 : a.approval_date2 >= a.requester_date ? a.approval_date2 : a.approval_date3 >= a.requester_date ? a.approval_date3 : a.admin_ar_date >= a.requester_date ? a.admin_ar_date : a.approval_date1,
                //                    approved_by = Convert.ToInt32(a.is_approved1) == Convert.ToInt32(1) ? string.Format("{0} {1} {2}",
                //                                            a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                //                                  a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                //                                  a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                //                           Convert.ToInt32(a.is_approved2) == Convert.ToInt32(1) ? string.Format("{0} {1} {2}",
                //                                  a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                //                                  a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                //                                  a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                //                           Convert.ToInt32(a.is_approved3) == Convert.ToInt32(1) ? string.Format("{0} {1} {2}",
                //                                  a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                //                                  a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                //                                  a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                //                           Convert.ToInt32(a.is_admin_approve) == Convert.ToInt32(1) ? string.Format("{0} {1} {2}",
                //                                  a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                //                                  a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                //                                  a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) : "",
                //                    requester_id = a.r_e_id,
                //                    system_in_time = d != null ? d.in_time : new DateTime(2000, 01, 01),
                //                    system_out_time = d != null ? d.out_time : new DateTime(2000, 01, 01),
                //                    is_approved1 = a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : "Reject",
                //                    is_approved2 = a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : "Reject",
                //                    is_approved3 = a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : "Reject",
                //                    is_delted = a.is_deleted,
                //                    is_final_approve = a.is_final_approve,
                //                    a.company_id,
                //                    approver_remarks = string.Concat(a.approval1_remarks ?? "", a.approval2_remarks ?? "", a.approval3_remarks ?? "", a.admin_remarks ?? "")
                //                }).ToList();

                #region old
                //var result = _context.tbl_attendace_request.Join(_context.tbl_daily_attendance, a => new { _date = a.from_date.Date, _empidd = a.r_e_id }, b => new { _date = b.attendance_dt.Date, _empidd = b.emp_id }, (a, b) => new
                //{

                //    emp_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                //                               a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                //                                   a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name), //a.tbl_employee_requester.tbl_emp_officaial_sec.Where(b => b.employee_id == a.r_e_id && b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).Select(c => c.employee_first_name).FirstOrDefault(),
                //    leave_request_id = a.leave_request_id,
                //    from_date = a.from_date,
                //    manual_in_time = a.manual_in_time,
                //    manual_out_time = a.manual_out_time,
                //    requester_remarks = a.requester_remarks,
                //    status = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : "Reject") : a.is_deleted == 1 ? "Deleted" : "",
                //    requester_id = a.r_e_id,
                //    system_in_time = b.in_time,
                //    system_out_time = b.out_time,
                //    is_approved1 = a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : "Reject",
                //    is_approved2 = a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : "Reject",
                //    is_approved3 = a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : "Reject",
                //    is_delted = a.is_deleted,
                //    is_final_approve = a.is_final_approve,
                //    a.company_id,
                //    approver_remarks = string.Concat(a.approval1_remarks ?? "", a.approval2_remarks ?? "", a.approval3_remarks ?? "", a.admin_remarks ?? ""),
                //}).Where(x =>objmodel.from_date.Date <= x.from_date.Date && x.from_date.Date <= objmodel.to_date.Date && objmodel.empdtl.Contains(x.requester_id)).ToList();
                #endregion

                return Ok(result);

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 1;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }


        //Update Outdoor Application Request 
        [Route("Update_AttendanceApplicationRequest")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.AttendanceApplication))]
        public IActionResult Update_AttendanceApplicationRequest([FromBody] tbl_attendace_request objattendace_request)
        {
            Response_Msg objResult = new Response_Msg();
            tbl_attendace_request obkleavereq = new tbl_attendace_request();

            if (!_clsCurrentUser.DownlineEmpId.Any(p => p == objattendace_request.r_e_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!";
                return Ok(objResult);
            }

            if (!_clsCurrentUser.CompanyId.Any(p => p == objattendace_request.company_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!";
                return Ok(objResult);
            }


            try
            {
                if (objattendace_request.is_final_approve != 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid data";
                    return Ok(objResult);
                }

                if (DateTime.Compare(objattendace_request.manual_in_time, objattendace_request.manual_out_time) > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Out Time must be greater than in time";
                    return Ok(objResult);
                }

                objattendace_request.system_in_time = objattendace_request.from_date.AddHours(objattendace_request.system_in_time.Hour).AddMinutes(objattendace_request.system_in_time.Minute);
                objattendace_request.system_out_time = objattendace_request.from_date.AddHours(objattendace_request.system_out_time.Hour).AddMinutes(objattendace_request.system_out_time.Minute);

                if (objattendace_request.from_date.Date > DateTime.Now.Date)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Regularize Date cannot be greater than Today date";
                    return Ok(objResult);
                }


                var rejected_rqst = _context.tbl_attendace_request.Where(x => x.leave_request_id != objattendace_request.leave_request_id && x.r_e_id == objattendace_request.r_e_id && x.manual_in_time == objattendace_request.manual_in_time && x.from_date.Date >= objattendace_request.from_date.Date && x.manual_out_time <= objattendace_request.manual_out_time && x.is_deleted == 0 && x.is_final_approve == 2).OrderByDescending(x => x.leave_request_id).FirstOrDefault();


                var duplicate = _context.tbl_attendace_request.Where(x => x.leave_request_id != objattendace_request.leave_request_id && x.r_e_id == objattendace_request.r_e_id && x.manual_in_time == objattendace_request.manual_in_time && x.from_date.Date >= objattendace_request.from_date.Date && x.manual_out_time <= objattendace_request.manual_out_time && x.is_deleted == 0 && x.is_final_approve == 0).OrderByDescending(x => x.leave_request_id).FirstOrDefault();

                if (rejected_rqst != null)
                {

                    var exist = _context.tbl_attendace_request.Where(x => x.leave_request_id == objattendace_request.leave_request_id && x.is_deleted == 0 && (int)x.is_final_approve == 0).FirstOrDefault();
                    if (exist == null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Invalid Attendence application request id, please try after refresh page !!";
                        return Ok(objResult);
                    }

                    exist.r_e_id = objattendace_request.r_e_id;
                    exist.from_date = objattendace_request.from_date;
                    exist.manual_in_time = objattendace_request.manual_in_time;
                    exist.manual_out_time = objattendace_request.manual_out_time;
                    exist.requester_remarks = objattendace_request.requester_remarks;
                    exist.company_id = objattendace_request.company_id;
                    exist.requester_date = DateTime.Now;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Attendence application request updated successfully !!";
                    MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);
                    Task task = Task.Run(() => obj_ms.AttendanceApplicationRequestMail(Convert.ToInt32(objattendace_request.r_e_id), objattendace_request.from_date, objattendace_request.manual_in_time, objattendace_request.manual_out_time, objattendace_request.requester_remarks));

                    task.Wait();
                }

                else if (duplicate != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Attendece application already applied for the selected date still in pending stage !!";
                }
                else
                {
                    var exist = _context.tbl_attendace_request.Where(x => x.leave_request_id == objattendace_request.leave_request_id && x.is_deleted == 0 && (int)x.is_final_approve == 0).FirstOrDefault();
                    if (exist == null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Invalid Attendence request id, please try after refresh page !!";
                    }

                    else
                    {

                        exist.r_e_id = objattendace_request.r_e_id;
                        exist.from_date = objattendace_request.from_date;
                        exist.manual_in_time = objattendace_request.manual_in_time;
                        exist.manual_out_time = objattendace_request.manual_out_time;
                        exist.requester_remarks = objattendace_request.requester_remarks;
                        exist.company_id = objattendace_request.company_id;
                        exist.requester_date = DateTime.Now;

                        _context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "Attendence application request updated successfully !!";
                        MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);
                        Task task = Task.Run(() => obj_ms.AttendanceApplicationRequestMail(Convert.ToInt32(objattendace_request.r_e_id), objattendace_request.from_date, objattendace_request.manual_in_time, objattendace_request.manual_out_time, objattendace_request.requester_remarks));

                        task.Wait();
                    }
                }
                //check for valid request id
                return Ok(objResult);

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 1;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        #endregion

        #region  Policy Master
        //Save Policies
        [Route("Save_Policies")]
        [HttpPost]
        //[Authorize(Policy = nameof(enmMenuMaster.HolidayMaster))]
        public IActionResult Save_Policies()
        {
            Response_Msg objResult = new Response_Msg();

            try
            {
                var files = Request.Form.Files;
                var a = HttpContext.Request.Form["AllData"];
                if (a.ToString() == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid data !!";
                    return Ok(objResult);
                }

                CommonClass com = new CommonClass();
                tbl_policy_master objPolicyMaster = new tbl_policy_master();
                objPolicyMaster = com.ToObjectFromJSON<tbl_policy_master>(a.ToString());

                string _file_pathh = string.Empty;
                var exist = _context.tbl_policy_master.Where(x => x.policy_name.Trim().ToUpper() == objPolicyMaster.policy_name.Trim().ToUpper() && x.is_deleted == 0).FirstOrDefault();

                if (exist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Policy Name already exist...!";
                    return Ok(objResult);
                }
                else
                {

                    try
                    {
                        tbl_policy_master obj_tpm = new tbl_policy_master();
                        obj_tpm.policy_name = objPolicyMaster.policy_name;
                        obj_tpm.remarks = objPolicyMaster.remarks;
                        obj_tpm.created_by = objPolicyMaster.created_by;
                        obj_tpm.last_modified_by = objPolicyMaster.created_by;
                        obj_tpm.created_date = DateTime.Now;
                        obj_tpm.last_modified_date = DateTime.Now;
                        obj_tpm.is_deleted = 0;

                        _context.Entry(obj_tpm).State = EntityState.Added;
                        //_context.SaveChanges();
                        // getting id of latest saved row
                        var policy_id = _context.tbl_policy_master.Count();
                        var policy_id1 = obj_tpm.pkid_policy;

                        using (var trans = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                foreach (var FileData in files)
                                {
                                    if (FileData != null && FileData.Length > 0)
                                    {

                                        var ext = Path.GetExtension(FileData.FileName); //getting the extension

                                        string name = Path.GetFileNameWithoutExtension(FileData.FileName); //getting file name without extension  
                                        var webRoot = _hostingEnvironment.WebRootPath;
                                        string MyFileName = "CompanyPolicy" + DateTime.Now.Ticks.ToString() + ext;


                                        if (!Directory.Exists(webRoot + "/CompanyPolicies"))
                                        {
                                            Directory.CreateDirectory(webRoot + "/CompanyPolicies");
                                        }

                                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/CompanyPolicies");
                                        //save file
                                        using (var fileStream = new FileStream(Path.Combine(path, MyFileName), FileMode.Create))
                                        {
                                            FileData.CopyTo(fileStream);
                                            _file_pathh = "/CompanyPolicies/" + MyFileName;
                                        }

                                        tbl_policy_master_documents obj_tpmd = new tbl_policy_master_documents();
                                        obj_tpmd.fkid_policy = (policy_id + 1);
                                        obj_tpmd.document_path = _file_pathh;
                                        obj_tpmd.is_active = 1;

                                        _context.Entry(obj_tpmd).State = EntityState.Added;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                objResult.StatusCode = 2;
                                objResult.Message = ex.Message;
                                return Ok(objResult);
                            }

                            _context.SaveChanges();
                            trans.Commit();
                        }

                        objResult.StatusCode = 0;
                        objResult.Message = "Policy Details Save Successfully...!";
                        return Ok(objResult);

                    }
                    catch (Exception ex)
                    {

                        objResult.StatusCode = 2;
                        objResult.Message = ex.Message;
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


        //Update Policies
        [Route("Update_Policies")]
        [HttpPost]
        //[Authorize(Policy = nameof(enmMenuMaster.HolidayMaster))]
        public IActionResult Update_Policies()
        {
            Response_Msg objResult = new Response_Msg();

            try
            {
                var files = Request.Form.Files;
                var a = HttpContext.Request.Form["AllData"];
                if (a.ToString() == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid data !!";
                    return Ok(objResult);
                }

                CommonClass com = new CommonClass();
                tbl_policy_master objPolicyMaster = new tbl_policy_master();
                objPolicyMaster = com.ToObjectFromJSON<tbl_policy_master>(a.ToString());

                string _file_pathh = string.Empty;
                var exist = _context.tbl_policy_master.Where(x => x.pkid_policy == objPolicyMaster.pkid_policy && x.is_deleted == 0).FirstOrDefault();

                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid Policy Id please check and retry...!";
                    return Ok(objResult);
                }
                else
                {

                    try
                    {
                        exist.policy_name = objPolicyMaster.policy_name;
                        exist.remarks = objPolicyMaster.remarks;
                        exist.last_modified_by = objPolicyMaster.last_modified_by;
                        exist.last_modified_date = DateTime.Now;

                        _context.Entry(exist).State = EntityState.Modified;

                        var policy_id = exist.pkid_policy;


                        using (var trans = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                if (files.Count > 0)
                                {
                                    _context.tbl_policy_master_documents.Where(x => x.fkid_policy == policy_id).ForEachAsync(y => y.is_active = 0);
                                    foreach (var FileData in files)
                                    {
                                        if (FileData != null && FileData.Length > 0)
                                        {

                                            var ext = Path.GetExtension(FileData.FileName); //getting the extension

                                            string name = Path.GetFileNameWithoutExtension(FileData.FileName); //getting file name without extension  
                                            var webRoot = _hostingEnvironment.WebRootPath;
                                            string MyFileName = "CompanyPolicy" + name + "" + DateTime.Now.Ticks.ToString() + ext;


                                            if (!Directory.Exists(webRoot + "/CompanyPolicies"))
                                            {
                                                Directory.CreateDirectory(webRoot + "/CompanyPolicies");
                                            }

                                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/CompanyPolicies");
                                            //save file
                                            using (var fileStream = new FileStream(Path.Combine(path, MyFileName), FileMode.Create))
                                            {
                                                FileData.CopyTo(fileStream);
                                                _file_pathh = fileStream.Name;
                                            }

                                            tbl_policy_master_documents obj_tpmd = new tbl_policy_master_documents();
                                            obj_tpmd.fkid_policy = (policy_id);
                                            obj_tpmd.document_path = _file_pathh;
                                            obj_tpmd.is_active = 1;

                                            _context.Entry(obj_tpmd).State = EntityState.Added;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                objResult.StatusCode = 2;
                                objResult.Message = ex.Message;
                                return Ok(objResult);
                            }

                            _context.SaveChanges();
                            trans.Commit();
                        }

                        objResult.StatusCode = 0;
                        objResult.Message = "Policy Details Updated Successfully...!";
                        return Ok(objResult);

                    }
                    catch (Exception ex)
                    {

                        objResult.StatusCode = 2;
                        objResult.Message = ex.Message;
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


        [Route("DeletePoliciesById/{request_id}")]
        [HttpPost]
        //[Authorize(Policy = nameof(enmMenuMaster.AttendanceApplication))]
        public IActionResult DeletePoliciesById([FromRoute] int request_id)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {

                var exist = _context.tbl_policy_master.Where(x => x.pkid_policy == request_id && x.is_deleted == 0).FirstOrDefault();
                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid request id, please check and try !!";
                    return Ok(objResult);
                }

                exist.is_deleted = 1;
                exist.last_modified_by = _clsCurrentUser.EmpId;
                exist.last_modified_date = DateTime.Now;

                _context.Entry(exist).State = EntityState.Modified;
                _context.SaveChanges();

                objResult.StatusCode = 0;
                objResult.Message = "Policy deleted successfully !!";
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }


        // GET: api/Get_Policies
        [Route("Get_PoliciesById/{request_id}")]
        [HttpPost]
        //[Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public async Task<IActionResult> Get_PoliciesById([FromRoute] int request_id)
        {
            ResponseMsg objResult = new ResponseMsg();

            var result = (from pm in _context.tbl_policy_master
                          where pm.is_deleted == 0 && pm.pkid_policy == request_id
                          select new
                          {
                              pm.pkid_policy,
                              pm.policy_name,
                              pm.remarks,
                              pm.created_date,
                              created_by = string.Format("{0} {1} {2}", _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_id == pm.created_by).employee_first_name, _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_id == pm.created_by).employee_middle_name, _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_id == pm.created_by).employee_last_name),
                              pm.is_deleted
                          }).FirstOrDefault();

            return Ok(result);

        }



        // GET: api/Get_Policies
        [Route("Get_Policies")]
        [HttpGet]
        //[Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public async Task<IActionResult> Get_Policies()
        {
            ResponseMsg objResult = new ResponseMsg();

            var result = (from pm in _context.tbl_policy_master
                          join pmd in _context.tbl_policy_master_documents on pm.pkid_policy equals pmd.fkid_policy into hcc
                          from pmd in hcc.DefaultIfEmpty()
                          where pm.is_deleted == 0 && pmd.is_active == 1
                          select new
                          {
                              pm.pkid_policy,
                              pm.policy_name,
                              pm.remarks,
                              pm.created_date,
                              created_by = string.Format("{0} {1} {2}", _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_id == pm.created_by).employee_first_name, _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_id == pm.created_by).employee_middle_name, _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_id == pm.created_by).employee_last_name),
                              pm.is_deleted,
                              doc_path = string.Format("{0}{1}", Convert.ToString(_config["domain_url"].Contains("api") ? _config["domain_url"].Replace("api/", "") : _config["domain_url"]), pmd.document_path)
                          }).OrderBy(x => x.created_date).ToList();

            return Ok(result);

        }

        #endregion  Policy Master

        #region Application Setting

        //Save ApplicationSetting
        [Route("Save_ApplicationSetting")]
        [HttpPost]
        //[Authorize(Policy = nameof(enmMenuMaster.HolidayMaster))]
        public IActionResult Save_ApplicationSetting([FromBody] App_setting_Master objASM)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {
                var exist = _context.tbl_app_setting.Where(x => x.AppSettingKey.Trim().ToUpper() == objASM.AppSettingKey.Trim().ToUpper()).FirstOrDefault();

                if (exist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Application Setting already exist...!";
                    return Ok(objResult);
                }
                else
                {
                    try
                    {
                        tbl_app_setting objappsetting = new tbl_app_setting();
                        objappsetting.AppSettingKey = objASM.AppSettingKey;
                        objappsetting.AppSettingValue = objASM.AppSettingValue;
                        objappsetting.created_by = objASM.created_by;
                        objappsetting.created_dt = DateTime.Now;
                        objappsetting.is_active = 1;

                        _context.Entry(objappsetting).State = EntityState.Added;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "Record Save Successfully...!";
                        return Ok(objResult);
                    }
                    catch (Exception ex)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = ex.Message;
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

        [Route("Get_ApplicationSettingById/{request_id}")]
        [HttpPost]
        //[Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public async Task<IActionResult> Get_ApplicationSettingById([FromRoute] int request_id)
        {
            ResponseMsg objResult = new ResponseMsg();

            var result = (from s in _context.tbl_app_setting
                          where s.pkid_setting == request_id
                          select new
                          {
                              s.pkid_setting,
                              s.AppSettingKey,
                              AppSettingKeyDisplay = string.IsNullOrEmpty(s.AppSettingKeyDisplay) ? s.AppSettingKey : s.AppSettingKeyDisplay,
                              AppSettingValue = s.AppSettingValue == "false" ? "False" : s.AppSettingValue == "true" ? "True" : s.AppSettingValue,
                              AppSettingValue_ = s.AppSettingValue,
                              _created_by = string.Format("{0} {1} {2}", _context.tbl_emp_officaial_sec.Where(x => x.employee_id == s.created_by && x.is_deleted == 0).FirstOrDefault().employee_first_name, _context.tbl_emp_officaial_sec.Where(x => x.employee_id == s.created_by && x.is_deleted == 0).FirstOrDefault().employee_middle_name, _context.tbl_emp_officaial_sec.Where(x => x.employee_id == s.created_by && x.is_deleted == 0).FirstOrDefault().employee_last_name),
                              s.created_by,
                              created_dt = s.created_dt.Date,
                              s.is_active,
                              _is_active = s.is_active == 1 ? "Active" : "Inactive"
                          }).FirstOrDefault();

            return Ok(result);

        }

        [Route("Delete_ApplicationSettingById/{request_id}")]
        [HttpPost]
        //[Authorize(Policy = nameof(enmMenuMaster.AttendanceApplication))]
        public IActionResult Delete_ApplicationSettingById([FromRoute] int request_id)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {
                var exist = _context.tbl_app_setting.Where(x => x.pkid_setting == request_id).FirstOrDefault();
                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid request id, please check and try !!";
                    return Ok(objResult);
                }

                exist.is_active = 0;
                exist.last_modified_by = _clsCurrentUser.EmpId;
                exist.last_modified_date = DateTime.Now;

                _context.Entry(exist).State = EntityState.Modified;
                _context.SaveChanges();

                objResult.StatusCode = 0;
                objResult.Message = "Deleted successfully!!";
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }



        [Route("Update_ApplicationSetting")]
        [HttpPost]
        //[Authorize(Policy = nameof(enmMenuMaster.HolidayMaster))]
        public IActionResult Update_ApplicationSetting([FromBody] App_setting_Master objASM)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {

                tbl_app_setting objappsetting = new tbl_app_setting();

                var exist = _context.tbl_app_setting.Where(x => x.AppSettingKey == objASM.AppSettingKey).FirstOrDefault();
                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid Request Id please check and retry...!";
                    return Ok(objResult);
                }
                else
                {
                    try
                    {
                        exist.AppSettingValue = objASM.AppSettingValue;
                        exist.last_modified_by = objASM.created_by;
                        exist.last_modified_date = DateTime.Now;
                        exist.is_active = 1;

                        _context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "Record Updated Successfully...!";
                        return Ok(objResult);
                    }
                    catch (Exception ex)
                    {
                        objResult.StatusCode = 2;
                        objResult.Message = ex.Message;
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


        // GET: api/Get_AppSettings
        [Route("Get_AppSettings")]
        [HttpGet]
        //[Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public async Task<IActionResult> Get_AppSettings()
        {
            ResponseMsg objResult = new ResponseMsg();

            var result = (from s in _context.tbl_app_setting
                          select new
                          {
                              s.pkid_setting,
                              s.AppSettingKey,
                              AppSettingKeyDisplay = string.IsNullOrEmpty(s.AppSettingKeyDisplay) ? s.AppSettingKey : s.AppSettingKeyDisplay,
                              AppSettingValue = s.AppSettingValue == "false" ? "False" : s.AppSettingValue == "true" ? "True" : s.AppSettingValue,
                              _created_by = string.Format("{0} {1} {2}", _context.tbl_emp_officaial_sec.Where(x => x.employee_id == s.created_by && x.is_deleted == 0).FirstOrDefault().employee_first_name, _context.tbl_emp_officaial_sec.Where(x => x.employee_id == s.created_by && x.is_deleted == 0).FirstOrDefault().employee_middle_name, _context.tbl_emp_officaial_sec.Where(x => x.employee_id == s.created_by && x.is_deleted == 0).FirstOrDefault().employee_last_name),
                              s.created_by,
                              created_dt = s.created_dt.Date,
                              s.is_active,
                              _is_active = s.is_active == 1 ? "Active" : "Inactive"
                          }).OrderBy(x => x.pkid_setting).ToList();

            return Ok(result);

        }

        #endregion  Application Setting

        #region Current openings

        [Route("Save_Current_Openings")]
        [HttpPost]
        //[Authorize(Policy = nameof(enmMenuMaster.HelthCard))]
        public async Task<IActionResult> Save_Current_Openings()
        {
            try
            {
                Response_Msg objreponse = new Response_Msg();
                var files = HttpContext.Request.Form.Files;
                var a = HttpContext.Request.Form["AllData"];
                if (a.ToString() == null)
                {
                    objreponse.StatusCode = 1;
                    objreponse.Message = "Invalid data !!";
                    return Ok(objreponse);
                }



                CommonClass com = new CommonClass();
                Current_Openings objCurrentOPenings = new Current_Openings();

                objCurrentOPenings = com.ToObjectFromJSON<Current_Openings>(a.ToString());
                tbl_current_openings objdata = new tbl_current_openings();


                //if (!_clsCurrentUser.CompanyId.Any(p => p == objCurrentOPenings.company_id))
                //{
                //    objreponse.StatusCode = 1;
                //    objreponse.Message = "Unauthorize Company Access...!";
                //    return Ok(objreponse);
                //}



                var exist = _context.tbl_current_openings.Where(x => x.company_id == objCurrentOPenings.company_id && x.is_active == 1 && x.department_id == objCurrentOPenings.department_id && x.opening_detail.ToUpper().Trim() == objCurrentOPenings.opening_detail.ToUpper().Trim()).FirstOrDefault();
                if (exist != null)
                {
                    objreponse.StatusCode = 1;
                    objreponse.Message = "Current Opening Already Exists";
                    return Ok(objreponse);
                }
                else
                {
                    //file upload logic
                    if (files.Count > 0)
                    {
                        foreach (var FileData in files)
                        {
                            if (FileData != null && FileData.Length > 0)
                            {
                                var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg", ".JPG", ".PNG", ".JPG", "JPEG", ".pdf", ".PDF" };

                                var ext = Path.GetExtension(FileData.FileName); //getting the extension
                                if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                                {
                                    string name = Path.GetFileNameWithoutExtension(FileData.FileName); //getting file name without extension  
                                    string MyFileName = "cu" + DateTime.Now.Ticks.ToString() + ext; //Guid.NewGuid().ToString().Replace("-", "") +


                                    var webRoot = _hostingEnvironment.WebRootPath;

                                    string currentmonth = Convert.ToString(DateTime.Now.Month).Length.ToString() == "1" ? "0" + Convert.ToString(DateTime.Now.Month) : Convert.ToString(DateTime.Now.Month);

                                    var currentyearmonth = Convert.ToString(DateTime.Now.Year) + currentmonth;


                                    if (!Directory.Exists(webRoot + "/CurrentOpening/" + currentyearmonth + "/"))
                                    {
                                        Directory.CreateDirectory(webRoot + "/CurrentOpening/" + currentyearmonth + "/");

                                    }

                                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/CurrentOpening/" + currentyearmonth + "/");

                                    //save file
                                    using (var fileStream = new FileStream(Path.Combine(path, MyFileName), FileMode.Create))
                                    {
                                        FileData.CopyTo(fileStream);
                                        objdata.doc_path = "/CurrentOpening/" + currentyearmonth + "/" + MyFileName;
                                    }

                                    objdata.company_id = objCurrentOPenings.company_id;
                                    objdata.department_id = objCurrentOPenings.department_id;
                                    objdata.opening_detail = objCurrentOPenings.opening_detail;
                                    objdata.posted_date = Convert.ToDateTime(objCurrentOPenings.posted_date);
                                    objdata.experience = objCurrentOPenings.experience;
                                    objdata.job_description = objCurrentOPenings.job_description;
                                    objdata.role_responsibility = objCurrentOPenings.role_responsibility;
                                    objdata.remarks = objCurrentOPenings.remarks;
                                    objdata.current_status = objCurrentOPenings.created_by;

                                    objdata.created_by = objCurrentOPenings.created_by;
                                    objdata.modified_by = objCurrentOPenings.created_by;
                                    objdata.created_dt = DateTime.Now;
                                    objdata.is_active = 1;

                                    _context.Entry(objdata).State = EntityState.Added;
                                    _context.SaveChanges();

                                    objreponse.StatusCode = 0;
                                    objreponse.Message = "Current Opening Added Successfully";
                                }
                                else
                                {
                                    objreponse.StatusCode = 1;
                                    objreponse.Message = "Please Upload Image .jpg";
                                }
                            }
                            else
                            {
                                objreponse.StatusCode = 1;
                                objreponse.Message = "Please attach Document";

                            }
                        }
                    }
                    else
                    {
                        objreponse.StatusCode = 1;
                        objreponse.Message = "Please select a file to upload!!!";
                        return Ok(objreponse);
                    }
                    return Ok(objreponse);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        // GET: api/Get_Policies
        [Route("Get_CurrentOpenings")]
        [HttpGet]
        //[Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_CurrentOpenings()
        {
            ResponseMsg objResult = new ResponseMsg();
            try
            {

                var result = (from a in _context.tbl_current_openings
                              where a.is_active == 1
                              select new
                              {
                                  a.pkid_current_openings,
                                  a.company_id,
                                  a.department_id,
                                  company_name = _context.tbl_company_master.Where(x => x.company_id == a.company_id && x.is_active == 1).FirstOrDefault().company_name,
                                  dept_name = _context.tbl_department_master.Where(x => x.department_id == a.department_id && x.is_active == 1).FirstOrDefault().department_name,
                                  a.opening_detail,
                                  a.posted_date,
                                  a.experience,
                                  a.job_description,
                                  a.role_responsibility,
                                  a.remarks,
                                  current_status = a.current_status == 1 ? "On Hold" : a.current_status == 2 ? "Hiring Ongoing" : a.current_status == 3 ? "Vacancy Expired" : "",
                                  created_by = string.Format("{0} {1} {2}", _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_id == a.created_by).employee_first_name, _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_id == a.created_by).employee_middle_name, _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_id == a.created_by).employee_last_name),
                                  a.is_active,
                                  doc_path = string.Format("{0}{1}", Convert.ToString(_config["domain_url"].Contains("api") ? _config["domain_url"].Replace("api/", "") : _config["domain_url"]), a.doc_path)
                              }).OrderBy(x => x.created_by).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }

        // GET: api/Get_Policies
        [Route("Get_CurrentOpeningsById/{request_id}")]
        [HttpPost]
        //[Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public async Task<IActionResult> Get_CurrentOpeningsById([FromRoute] int request_id)
        {
            ResponseMsg objResult = new ResponseMsg();

            try
            {

                var result = (from a in _context.tbl_current_openings
                              where a.is_active == 1 && a.pkid_current_openings == request_id
                              select new
                              {
                                  a.pkid_current_openings,
                                  a.company_id,
                                  a.department_id,
                                  current_statusid = a.current_status,
                                  company_name = _context.tbl_company_master.Where(x => x.company_id == a.company_id && x.is_active == 1).FirstOrDefault().company_name,
                                  dept_name = _context.tbl_department_master.Where(x => x.department_id == a.department_id && x.is_active == 1).FirstOrDefault().department_name,
                                  a.opening_detail,
                                  a.posted_date,
                                  a.experience,
                                  a.job_description,
                                  a.role_responsibility,
                                  a.remarks,
                                  current_status = a.current_status == 1 ? "On Hold" : a.current_status == 2 ? "Hiring Ongoing" : a.current_status == 3 ? "Vacancy Expired" : "",
                                  created_by = string.Format("{0} {1} {2}", _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_id == a.created_by).employee_first_name, _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_id == a.created_by).employee_middle_name, _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_id == a.created_by).employee_last_name),
                                  a.is_active,
                                  doc_path = string.Format("{0}{1}", Convert.ToString(_config["domain_url"].Contains("api") ? _config["domain_url"].Replace("api/", "") : _config["domain_url"]), a.doc_path)
                              }).OrderBy(x => x.created_by).FirstOrDefault();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }



        }


        [Route("DeleteCurrentOpeningsById/{request_id}")]
        [HttpPost]
        //[Authorize(Policy = nameof(enmMenuMaster.AttendanceApplication))]
        public IActionResult DeleteCurrentOpeningsById([FromRoute] int request_id)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {

                var exist = _context.tbl_current_openings.Where(x => x.pkid_current_openings == request_id && x.is_active == 1).FirstOrDefault();
                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid request id, please check and try !!";
                    return Ok(objResult);
                }

                exist.is_active = 0;
                exist.modified_by = _clsCurrentUser.EmpId;
                exist.modified_dt = DateTime.Now;

                _context.Entry(exist).State = EntityState.Modified;
                _context.SaveChanges();

                objResult.StatusCode = 0;
                objResult.Message = "Current Opening Deleted Successfully !!";
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        [Route("Update_Current_Openings")]
        [HttpPost]
        //[Authorize(Policy = nameof(enmMenuMaster.HelthCard))]
        public async Task<IActionResult> Update_Current_Openings()
        {
            try
            {
                Response_Msg objreponse = new Response_Msg();
                var files = HttpContext.Request.Form.Files;
                var a = HttpContext.Request.Form["AllData"];
                if (a.ToString() == null)
                {
                    objreponse.StatusCode = 1;
                    objreponse.Message = "Invalid data !!";
                    return Ok(objreponse);
                }



                CommonClass com = new CommonClass();
                Current_Openings objCurrentOPenings = new Current_Openings();

                objCurrentOPenings = com.ToObjectFromJSON<Current_Openings>(a.ToString());




                //if (!_clsCurrentUser.CompanyId.Any(p => p == objCurrentOPenings.company_id))
                //{
                //    objreponse.StatusCode = 1;
                //    objreponse.Message = "Unauthorize Company Access...!";
                //    return Ok(objreponse);
                //}



                var exist = _context.tbl_current_openings.Where(x => x.company_id == objCurrentOPenings.company_id && x.is_active == 1 && x.department_id == objCurrentOPenings.department_id && x.pkid_current_openings == objCurrentOPenings.pkid_current_openings).FirstOrDefault();
                if (exist == null)
                {
                    objreponse.StatusCode = 1;
                    objreponse.Message = "Invalid Current Opening Id please check and retry...!";
                    return Ok(objreponse);
                }
                else
                {
                    //file upload logic
                    if (files.Count > 0)
                    {
                        foreach (var FileData in files)
                        {
                            if (FileData != null && FileData.Length > 0)
                            {
                                var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg", ".JPG", ".PNG", ".JPG", "JPEG", ".pdf", ".PDF" };

                                var ext = Path.GetExtension(FileData.FileName); //getting the extension
                                if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                                {
                                    string name = Path.GetFileNameWithoutExtension(FileData.FileName); //getting file name without extension  
                                    string MyFileName = "cu" + DateTime.Now.Ticks.ToString() + ext; //Guid.NewGuid().ToString().Replace("-", "") +


                                    var webRoot = _hostingEnvironment.WebRootPath;

                                    string currentmonth = Convert.ToString(DateTime.Now.Month).Length.ToString() == "1" ? "0" + Convert.ToString(DateTime.Now.Month) : Convert.ToString(DateTime.Now.Month);

                                    var currentyearmonth = Convert.ToString(DateTime.Now.Year) + currentmonth;


                                    if (!Directory.Exists(webRoot + "/CurrentOpening/" + currentyearmonth + "/"))
                                    {
                                        Directory.CreateDirectory(webRoot + "/CurrentOpening/" + currentyearmonth + "/");

                                    }

                                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/CurrentOpening/" + currentyearmonth + "/");

                                    //save file
                                    using (var fileStream = new FileStream(Path.Combine(path, MyFileName), FileMode.Create))
                                    {
                                        FileData.CopyTo(fileStream);
                                        objCurrentOPenings.doc_path = "/CurrentOpening/" + currentyearmonth + "/" + MyFileName;
                                    }

                                    exist.company_id = objCurrentOPenings.company_id;
                                    exist.department_id = objCurrentOPenings.department_id;
                                    exist.opening_detail = objCurrentOPenings.opening_detail;
                                    exist.posted_date = DateTime.Parse(objCurrentOPenings.posted_date);
                                    exist.experience = objCurrentOPenings.experience;
                                    exist.current_status = objCurrentOPenings.current_status;
                                    exist.job_description = objCurrentOPenings.job_description;
                                    exist.role_responsibility = objCurrentOPenings.role_responsibility;
                                    exist.remarks = objCurrentOPenings.remarks;
                                    exist.modified_by = objCurrentOPenings.created_by;
                                    exist.modified_dt = DateTime.Now;
                                    exist.is_active = 1;

                                    _context.Entry(exist).State = EntityState.Modified;
                                    _context.SaveChanges();

                                    objreponse.StatusCode = 0;
                                    objreponse.Message = "Current Opening Updated Successfully";
                                }
                                else
                                {
                                    objreponse.StatusCode = 1;
                                    objreponse.Message = "Please Upload Image .jpg";
                                }
                            }
                            else
                            {
                                objreponse.StatusCode = 1;
                                objreponse.Message = "Please attach Document";

                            }
                        }
                    }
                    else
                    {
                        objreponse.StatusCode = 1;
                        objreponse.Message = "Please select a file to upload!!!";
                        return Ok(objreponse);
                    }
                    return Ok(objreponse);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        #endregion  Current openings
        [Route("Get_EmployeeDetails/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "7064")]
        public IActionResult Get_EmployeeDetails(int company_id)
        {
            try
            {

                var data = (
                from e in _context.tbl_employee_master
                join u in _context.tbl_user_master on e.employee_id equals u.employee_id
                join o in _context.tbl_emp_officaial_sec on e.employee_id equals o.employee_id
                where u.default_company_id == company_id && u.is_active == 1 && e.is_active == 1 && o.is_deleted == 0
                select new
                {
                    e.employee_id,
                    e.emp_code,
                    o.card_number,
                    o.employee_first_name,
                    o.employee_middle_name,
                    o.employee_last_name,

                }).ToList();



                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>
        /// REPORTS /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="Reports"></param>
        /// <returns></returns>

        //Present  Report
        [Route("PresentReport")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.PresentReport))]
        public IActionResult PresentReport([FromForm] Reports objReports)
        {
            try
            {
                bool allcompany_id = objReports.company_id == 0 ? true : false;
                bool allstate_id = objReports.state_id == 0 ? true : false;
                bool alllocation_id = objReports.location_id == 0 ? true : false;
                bool alldepartment_id = objReports.department_id == 0 ? true : false;
                bool allshift_id = objReports.Shift_id == 0 ? true : false;

                var data = (
                    from d in _context.tbl_daily_attendance
                    join em in _context.tbl_employee_master on d.emp_id equals em.employee_id
                    join e in _context.tbl_user_master on em.employee_id equals e.employee_id
                    join o in _context.tbl_emp_officaial_sec on d.emp_id equals o.employee_id
                    join s in _context.tbl_shift_details on d.shift_id equals s.shift_id
                    where
                    e.default_company_id == (allcompany_id ? e.default_company_id : objReports.company_id) &&
                    o.tbl_location_master.state_id == (allstate_id ? o.tbl_location_master.state_id : objReports.state_id) &&
                    o.location_id == (alllocation_id ? o.location_id : objReports.location_id) &&
                    o.department_id == (alldepartment_id ? o.department_id : objReports.department_id) &&
                    d.shift_id == (allshift_id ? d.shift_id : objReports.Shift_id) &&
                    d.attendance_dt >= objReports.FromDate && d.attendance_dt <= objReports.ToDate &&
                    em.is_active == 1 && e.is_active == 1 && o.is_deleted == 0 && s.is_deleted == 0 &&
                    d.day_status == 1 && _clsCurrentUser.DownlineEmpId.Contains(e.employee_id ?? 0) && _clsCurrentUser.CompanyId.Contains(e.default_company_id)
                    select new
                    {
                        e.employee_id,
                        o.card_number,
                        o.employee_first_name,
                        o.employee_middle_name,
                        o.employee_last_name,
                        d.shift_in_time,
                        d.shift_out_time,
                        d.in_time,
                        d.out_time,
                        d.attendance_dt,
                        d.day_status,
                        d.working_hour_done,
                        d.working_minute_done,
                        em.emp_code,
                        s.shift_name
                    }).ToList();

                //----------set payroll calender data--------------------------
                List<PresentReportData> PayrollCalender = new List<PresentReportData>();

                PayrollCalender = data.Select(TempIndex => new PresentReportData
                {
                    sno = data.IndexOf(TempIndex) + 1,
                    CardNo = TempIndex.card_number,
                    AttendanceDate = TempIndex.attendance_dt,
                    employee_first_name = !string.IsNullOrEmpty(TempIndex.employee_first_name) ? TempIndex.employee_first_name : " ",
                    employee_middle_name = !string.IsNullOrEmpty(TempIndex.employee_middle_name) ? TempIndex.employee_middle_name : " ",
                    employee_last_name = !string.IsNullOrEmpty(TempIndex.employee_last_name) ? TempIndex.employee_last_name : " ",
                    shift_in_time = TempIndex.shift_in_time,
                    shift_out_time = TempIndex.shift_out_time,
                    working_hour_done = TempIndex.working_hour_done,
                    working_minute_done = TempIndex.working_minute_done,
                    EmployeeCode = TempIndex.emp_code,
                    AttStatus = TempIndex.day_status,
                    AttendanceStatus = TempIndex.day_status == 1 ? "Present" : "Present",
                    InTime = TempIndex.in_time == null ? "" : TempIndex.in_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.in_time.ToString("hh:mm tt"),
                    OutTime = TempIndex.out_time == null ? "" : TempIndex.out_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.out_time.ToString("hh:mm tt"),
                    ColorCode = TempIndex.day_status == 1 ? "#8b8be8" : "#8b8be8",
                    ShiftName = TempIndex.shift_name,
                    working_hrs = TempIndex.working_minute_done.ToString().Contains("-") ? "00:00" : TempIndex.working_hour_done.ToString().Contains("-") ? "00:00" : Convert.ToDateTime(TempIndex.working_hour_done + ":" + TempIndex.working_minute_done).ToString("hh:mm") == ("12:00").ToString() ? "00:00" : Convert.ToDateTime(TempIndex.working_hour_done + ":" + TempIndex.working_minute_done).ToString("hh:mm"),
                }).ToList();



                return Ok(PayrollCalender);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        //Absent  Report
        [Route("AbsentReport")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.AbsentReport))]
        public IActionResult AbsentReport([FromForm]  Reports objReports)
        {
            try
            {
                bool allcompany_id = objReports.company_id == 0 ? true : false;
                bool allstate_id = objReports.state_id == 0 ? true : false;
                bool alllocation_id = objReports.location_id == 0 ? true : false;
                bool alldepartment_id = objReports.department_id == 0 ? true : false;
                bool allshift_id = objReports.Shift_id == 0 ? true : false;

                var data = (
                    from d in _context.tbl_daily_attendance
                    join em in _context.tbl_employee_master on d.emp_id equals em.employee_id
                    join e in _context.tbl_user_master on em.employee_id equals e.employee_id
                    join o in _context.tbl_emp_officaial_sec on d.emp_id equals o.employee_id
                    join s in _context.tbl_shift_details on d.shift_id equals s.shift_id
                    join r in _context.tbl_user_role_map on e.user_id equals r.user_id
                    where
                    e.default_company_id == (allcompany_id ? e.default_company_id : objReports.company_id) &&
                    o.tbl_location_master.state_id == (allstate_id ? o.tbl_location_master.state_id : objReports.state_id) &&
                    o.location_id == (alllocation_id ? o.location_id : objReports.location_id) &&
                    o.department_id == (alldepartment_id ? o.department_id : objReports.department_id) &&
                    d.shift_id == (allshift_id ? d.shift_id : objReports.Shift_id) &&
                    d.attendance_dt >= objReports.FromDate && d.attendance_dt <= objReports.ToDate &&
                    em.is_active == 1 && e.is_active == 1 && o.is_deleted == 0 && s.is_deleted == 0 &&
                    d.day_status == 2 && _clsCurrentUser.DownlineEmpId.Contains(e.employee_id ?? 0) && _clsCurrentUser.CompanyId.Contains(e.default_company_id)//&& r.role_id == 6
                    select new
                    {
                        e.employee_id,
                        o.card_number,
                        o.employee_first_name,
                        o.employee_middle_name,
                        o.employee_last_name,
                        d.shift_in_time,
                        d.shift_out_time,
                        d.in_time,
                        d.out_time,
                        d.attendance_dt,
                        d.day_status,
                        d.working_hour_done,
                        d.working_minute_done,
                        em.emp_code,
                        s.shift_name
                    }).ToList();

                //----------set payroll calender data--------------------------
                List<PresentReportData> PayrollCalender = new List<PresentReportData>();

                PayrollCalender = data.Select(TempIndex => new PresentReportData
                {
                    sno = data.IndexOf(TempIndex) + 1,
                    CardNo = TempIndex.card_number,
                    AttendanceDate = TempIndex.attendance_dt,
                    employee_first_name = !string.IsNullOrEmpty(TempIndex.employee_first_name) ? TempIndex.employee_first_name : " ",
                    employee_middle_name = !string.IsNullOrEmpty(TempIndex.employee_middle_name) ? TempIndex.employee_middle_name : " ",
                    employee_last_name = !string.IsNullOrEmpty(TempIndex.employee_last_name) ? TempIndex.employee_last_name : " ",
                    shift_in_time = TempIndex.shift_in_time,
                    shift_out_time = TempIndex.shift_out_time,
                    working_hour_done = TempIndex.working_hour_done,
                    working_minute_done = TempIndex.working_minute_done,
                    EmployeeCode = TempIndex.emp_code,
                    AttStatus = TempIndex.day_status,
                    AttendanceStatus = TempIndex.day_status == 2 ? "Absent" : "Absent",
                    InTime = TempIndex.in_time == null ? "" : TempIndex.in_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.in_time.ToString("hh:mm tt"),
                    OutTime = TempIndex.out_time == null ? "" : TempIndex.out_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.out_time.ToString("hh:mm tt"),
                    ColorCode = TempIndex.day_status == 2 ? "#ef925e" : "#ef925e",
                    ShiftName = TempIndex.shift_name,
                    working_hrs = "00:00",
                }).ToList();



                return Ok(PayrollCalender);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        //Leave  Report
        [Route("LeaveReport")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveReport))]
        public IActionResult LeaveReport([FromForm]  Reports objReports)
        {
            try
            {
                bool allcompany_id = objReports.company_id == 0 ? true : false;
                bool allstate_id = objReports.state_id == 0 ? true : false;
                bool alllocation_id = objReports.location_id == 0 ? true : false;
                bool alldepartment_id = objReports.department_id == 0 ? true : false;
                bool allshift_id = objReports.Shift_id == 0 ? true : false;

                using (var trans = _context.Database.BeginTransaction())
                {

                    var data = _context.tbl_leave_request.Join(_context.tbl_daily_attendance, a => a.r_e_id, b => b.emp_id, (a, b) => new
                    {
                        a.r_e_id,
                        a.from_date,
                        a.to_date,
                        a.tbl_employee_requester.tbl_employee_company_map.FirstOrDefault(x => x.is_deleted == 0).company_id,
                        a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).location_id,
                        emp_off_isdel = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).is_deleted,
                        a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).department_id,
                        req_del = a.is_deleted,
                        shift_id = b.shift_id == null ? 0 : b.shift_id,

                        emp_name_code = string.Format("{0} {1} {2} ({3})", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(c => c.is_deleted == 0 && !string.IsNullOrEmpty(c.employee_first_name)).employee_first_name,
                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(c => c.is_deleted == 0 && !string.IsNullOrEmpty(c.employee_first_name)).employee_middle_name,
                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(c => c.is_deleted == 0 && !string.IsNullOrEmpty(c.employee_first_name)).employee_last_name,
                       a.tbl_employee_requester.emp_code),

                        cardNo = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).card_number,

                        b.day_status,
                        b.attendance_dt

                    }).Where(x => x.req_del == 0 && x.emp_off_isdel == 0 && x.company_id == (allcompany_id ? x.company_id : objReports.company_id) &&
                    x.location_id == (alllocation_id ? x.location_id : objReports.location_id) &&
                    x.department_id == (alldepartment_id ? x.department_id : objReports.department_id) &&
                    x.shift_id == (allshift_id ? x.shift_id : objReports.Shift_id) &&
                    x.from_date.Date >= objReports.FromDate.Date && x.to_date.Date <= objReports.ToDate.Date && _clsCurrentUser.DownlineEmpId.Contains(x.r_e_id ?? 0) && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).ToList().Distinct();
                    trans.Commit();
                    return Ok(data);
                }




                // var data = _context.tbl_daily_attendance.Join(_context.tbl_leave_request, a => a.emp_id, b => b.r_e_id, (a, b) => new
                // {
                //     employee_id = a.emp_id,
                //     a.,
                //     b.from_date,
                //     b.to_date,
                //     a.shift_id,
                //     a.,
                //     b.is_deleted,


                //     ,
                //     emp_is_active = b.tbl_employee_requester.is_active,
                //     location_id = (b.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).location_id)!=null? (b.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).location_id):0,
                //     department_id = b.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).department_id,
                //     emp_official_sec_is_deleted = b.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).is_deleted,
                //     default_company_id = b.tbl_employee_requester.tbl_employee_company_map.FirstOrDefault(y => y.is_deleted == 0).company_id,
                // }).Where(x => x.default_company_id== (allcompany_id ? x.default_company_id : objReports.company_id) &&
                // x.location_id== (alllocation_id ? x.location_id : objReports.location_id) &&
                //// x.department_id == (alldepartment_id ? x.department_id : objReports.department_id) &&
                //// x.shift_id == (allshift_id ? x.shift_id : objReports.Shift_id) &&
                // x.attendance_dt.Date >= objReports.FromDate.Date && x.attendance_dt.Date <= objReports.ToDate.Date && x.emp_is_active == 1 && x.emp_official_sec_is_deleted == 0 && x.is_deleted == 0).ToList().Distinct();

                // return Ok(data);


                //var data = (
                //    from d in _context.tbl_daily_attendance
                //    join em in _context.tbl_employee_master on d.emp_id equals em.employee_id
                //    join e in _context.tbl_user_master on em.employee_id equals e.employee_id
                //    join o in _context.tbl_emp_officaial_sec on d.emp_id equals o.employee_id
                //    join s in _context.tbl_shift_details on d.shift_id equals s.shift_id
                //    where
                //    e.default_company_id == (allcompany_id ? e.default_company_id : objReports.company_id) &&
                //    o.location_id == (alllocation_id ? o.location_id : objReports.location_id) &&
                //    o.department_id == (alldepartment_id ? o.department_id : objReports.department_id) &&
                //    d.shift_id == (allshift_id ? d.shift_id : objReports.Shift_id) &&
                //    d.attendance_dt.Date >= objReports.FromDate.Date && d.attendance_dt.Date <= objReports.ToDate.Date &&
                //    em.is_active == 1 && e.is_active == 1 && o.is_deleted == 0 && s.is_deleted == 0 &&
                //    d.day_status == 3
                //    select new
                //    {
                //        e.employee_id,
                //        o.card_number,
                //        o.employee_first_name,
                //        o.employee_middle_name,
                //        o.employee_last_name,
                //        d.shift_in_time,
                //        d.shift_out_time,
                //        d.in_time,
                //        d.out_time,
                //        d.attendance_dt,
                //        d.day_status,
                //        d.working_hour_done,
                //        d.working_minute_done,
                //        em.emp_code,
                //        s.shift_name
                //    }).ToList();

                //----------set payroll calender data--------------------------
                //List < PresentReportData> PayrollCalender = new List<PresentReportData>();

                //PayrollCalender = data.Select(TempIndex => new PresentReportData
                //{
                //    sno = data.IndexOf(TempIndex) + 1,
                //    CardNo = TempIndex.card_number.ToString(),
                //    AttendanceDate = TempIndex.attendance_dt,
                //    employee_first_name = !string.IsNullOrEmpty(TempIndex.employee_first_name.ToString()) ? TempIndex.employee_first_name.ToString() : " ",
                //    employee_middle_name = !string.IsNullOrEmpty(TempIndex.employee_middle_name.ToString()) ? TempIndex.employee_middle_name.ToString() : " ",
                //    employee_last_name = !string.IsNullOrEmpty(TempIndex.employee_last_name.ToString()) ? TempIndex.employee_last_name.ToString() : " ",
                //    //shift_in_time = TempIndex.shift_in_time,
                //    //shift_out_time = TempIndex.shift_out_time,
                //    //working_hour_done = TempIndex.working_hour_done,
                //    //working_minute_done = TempIndex.working_minute_done,
                //    EmployeeCode = TempIndex.emp_code.ToString(),
                //    AttStatus = TempIndex.day_status,
                //    AttendanceStatus = TempIndex.day_status == 3 ? "On Leave" : "On Leave",
                //    //InTime = TempIndex.in_time == null ? "" : TempIndex.in_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.in_time.ToString("hh:mm tt"),
                //    //OutTime = TempIndex.out_time == null ? "" : TempIndex.out_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.out_time.ToString("hh:mm tt"),
                //    ColorCode = TempIndex.day_status == 3 ? "#e46ee4" : "#e46ee4",
                //    //ShiftName = TempIndex.shift_name,
                //    //working_hrs = TempIndex.working_minute_done.ToString().Contains("-") ? "00:00" : TempIndex.working_hour_done.ToString().Contains("-") ? "00:00" : Convert.ToDateTime(TempIndex.working_hour_done + ":" + TempIndex.working_minute_done).ToString("hh:mm") == ("12:00").ToString() ? "00:00" : Convert.ToDateTime(TempIndex.working_hour_done + ":" + TempIndex.working_minute_done).ToString("hh:mm"),
                //}).ToList();

                //return Ok(PayrollCalender);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        //Performance Individual
        [Route("PerformanceIndividual")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Report))]
        public IActionResult PerformanceIndividual([FromForm]  Reports objReports)
        {
            try
            {
                bool allcompany_id = objReports.company_id == 0 ? true : false;
                bool allstate_id = objReports.state_id == 0 ? true : false;
                bool alllocation_id = objReports.location_id == 0 ? true : false;
                bool alldepartment_id = objReports.department_id == 0 ? true : false;
                bool allshift_id = objReports.Shift_id == 0 ? true : false;

                var data = (
                    from d in _context.tbl_daily_attendance
                    join em in _context.tbl_employee_master on d.emp_id equals em.employee_id
                    join e in _context.tbl_user_master on em.employee_id equals e.employee_id
                    join o in _context.tbl_emp_officaial_sec on d.emp_id equals o.employee_id
                    join s in _context.tbl_shift_details on d.shift_id equals s.shift_id
                    join r in _context.tbl_user_role_map on e.user_id equals r.user_id
                    where
                    e.default_company_id == (allcompany_id ? e.default_company_id : objReports.company_id) &&
                    o.tbl_location_master.state_id == (allstate_id ? o.tbl_location_master.state_id : objReports.state_id) &&
                    o.location_id == (alllocation_id ? o.location_id : objReports.location_id) &&
                    o.department_id == (alldepartment_id ? o.department_id : objReports.department_id) &&
                    d.shift_id == (allshift_id ? d.shift_id : objReports.Shift_id) &&
                    d.attendance_dt >= objReports.FromDate && d.attendance_dt <= objReports.ToDate &&
                    //em.is_active == 1 && e.is_active == 1 &&
                    o.is_deleted == 0 && s.is_deleted == 0 && _clsCurrentUser.DownlineEmpId.Contains(e.employee_id ?? 0) && _clsCurrentUser.CompanyId.Contains(e.default_company_id) //&& r.role_id == 6
                                                                                                                                                                                     //&& (d.day_status == 1 || d.day_status == 2 || d.day_status == 3)
                    select new
                    {
                        e.employee_id,
                        o.card_number,
                        o.employee_first_name,
                        o.employee_middle_name,
                        o.employee_last_name,
                        d.shift_in_time,
                        d.shift_out_time,
                        d.in_time,
                        d.out_time,
                        d.attendance_dt,
                        d.day_status,
                        d.working_hour_done,
                        d.working_minute_done,
                        em.emp_code,
                        d.is_holiday,
                        d.is_weekly_off,
                        s.shift_name
                    }).ToList();

                //----------set payroll calender data--------------------------
                List<PresentReportData> PayrollCalender = new List<PresentReportData>();

                PayrollCalender = data.Select(TempIndex => new PresentReportData
                {
                    sno = data.IndexOf(TempIndex) + 1,
                    CardNo = TempIndex.card_number,
                    AttendanceDate = TempIndex.attendance_dt,
                    employee_first_name = !string.IsNullOrEmpty(TempIndex.employee_first_name) ? TempIndex.employee_first_name : " ",
                    employee_middle_name = !string.IsNullOrEmpty(TempIndex.employee_middle_name) ? TempIndex.employee_middle_name : " ",
                    employee_last_name = !string.IsNullOrEmpty(TempIndex.employee_last_name) ? TempIndex.employee_last_name : " ",
                    shift_in_time = TempIndex.shift_in_time,
                    shift_out_time = TempIndex.shift_out_time,
                    working_hour_done = TempIndex.working_hour_done,
                    working_minute_done = TempIndex.working_minute_done,
                    EmployeeCode = TempIndex.emp_code,
                    AttStatus = TempIndex.day_status,
                    AttendanceStatus = TempIndex.day_status == 1 ? "Present" : TempIndex.day_status == 2 ? "Absent" : TempIndex.day_status == 3 ? "On Leave" : TempIndex.is_holiday == 1 ? "Holiday" : TempIndex.is_weekly_off == 1 ? "W/O" : TempIndex.day_status == 4 ? "Half Day Present - Half Day Absent" : TempIndex.day_status == 5 ? "Half Day Present - Half Day Leave" : TempIndex.day_status == 6 ? "Half Day Leave - Halfday Absent" : "Present",
                    InTime = TempIndex.in_time == null ? "" : TempIndex.in_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.in_time.ToString("hh:mm tt"),
                    OutTime = TempIndex.out_time == null ? "" : TempIndex.out_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.out_time.ToString("hh:mm tt"),
                    ColorCode = TempIndex.day_status == 1 ? "#8b8be8" : TempIndex.day_status == 2 ? "#ef925e" : TempIndex.day_status == 3 ? "#e46ee4" : TempIndex.is_holiday == 1 ? "#9dea27" : TempIndex.is_weekly_off == 1 ? "#b3aeae" : TempIndex.day_status == 4 ? "#ef925e" : TempIndex.day_status == 5 ? "#e46ee4" : TempIndex.day_status == 6 ? "#ef925e" : "#ffc000",
                    ShiftName = TempIndex.shift_name,
                    working_hrs = TempIndex.working_minute_done.ToString().Contains("-") ? "00:00" : TempIndex.working_hour_done.ToString().Contains("-") ? "00:00" : Convert.ToDateTime(TempIndex.working_hour_done + ":" + TempIndex.working_minute_done).ToString("hh:mm") == ("12:00").ToString() ? "00:00" : Convert.ToDateTime(TempIndex.working_hour_done + ":" + TempIndex.working_minute_done).ToString("hh:mm"),
                }).ToList();



                return Ok(PayrollCalender);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        //Mispunch Report
        [Route("MispunchReport")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.MispunchReport))]
        public IActionResult MispunchReport([FromForm]  Reports objReports)
        {
            try
            {
                bool allcompany_id = objReports.company_id == 0 ? true : false;
                bool allstate_id = objReports.state_id == 0 ? true : false;
                bool alllocation_id = objReports.location_id == 0 ? true : false;
                bool alldepartment_id = objReports.department_id == 0 ? true : false;
                bool allshift_id = objReports.Shift_id == 0 ? true : false;

                var data = (
                    from d in _context.tbl_daily_attendance
                    join em in _context.tbl_employee_master on d.emp_id equals em.employee_id
                    join e in _context.tbl_user_master on em.employee_id equals e.employee_id
                    join o in _context.tbl_emp_officaial_sec on d.emp_id equals o.employee_id
                    join s in _context.tbl_shift_details on d.shift_id equals s.shift_id
                    where
                    e.default_company_id == (allcompany_id ? e.default_company_id : objReports.company_id) &&
                    o.tbl_location_master.state_id == (allstate_id ? o.tbl_location_master.state_id : objReports.state_id) &&
                    o.location_id == (alllocation_id ? o.location_id : objReports.location_id) &&
                    o.department_id == (alldepartment_id ? o.department_id : objReports.department_id) &&
                    d.shift_id == (allshift_id ? d.shift_id : objReports.Shift_id) &&
                    d.attendance_dt >= objReports.FromDate && d.attendance_dt <= objReports.ToDate &&
                    em.is_active == 1 && e.is_active == 1 && o.is_deleted == 0 && s.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(e.default_company_id) && _clsCurrentUser.DownlineEmpId.Contains(e.employee_id ?? 0) &&
                    (d.in_time != Convert.ToDateTime("2000-01-01 00:00:00") && d.out_time == Convert.ToDateTime("2000-01-01 00:00:00"))
                    select new
                    {
                        e.employee_id,
                        o.card_number,
                        o.employee_first_name,
                        o.employee_middle_name,
                        o.employee_last_name,
                        d.shift_in_time,
                        d.shift_out_time,
                        d.in_time,
                        d.out_time,
                        d.attendance_dt,
                        d.day_status,
                        d.working_hour_done,
                        d.working_minute_done,
                        em.emp_code,
                        s.shift_name
                    }).ToList();

                //----------set payroll calender data--------------------------
                List<PresentReportData> PayrollCalender = new List<PresentReportData>();

                PayrollCalender = data.Select(TempIndex => new PresentReportData
                {
                    sno = data.IndexOf(TempIndex) + 1,
                    CardNo = TempIndex.card_number,
                    AttendanceDate = TempIndex.attendance_dt,
                    employee_first_name = !string.IsNullOrEmpty(TempIndex.employee_first_name) ? TempIndex.employee_first_name : " ",
                    employee_middle_name = !string.IsNullOrEmpty(TempIndex.employee_middle_name) ? TempIndex.employee_middle_name : " ",
                    employee_last_name = !string.IsNullOrEmpty(TempIndex.employee_last_name) ? TempIndex.employee_last_name : " ",
                    shift_in_time = TempIndex.shift_in_time,
                    shift_out_time = TempIndex.shift_out_time,
                    working_hour_done = TempIndex.working_hour_done,
                    working_minute_done = TempIndex.working_minute_done,
                    EmployeeCode = TempIndex.emp_code,
                    AttStatus = TempIndex.day_status,
                    InTime = TempIndex.in_time == null ? "" : TempIndex.in_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.in_time.ToString("hh:mm tt"),
                    OutTime = TempIndex.out_time == null ? "" : TempIndex.out_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.out_time.ToString("hh:mm tt"),
                    ShiftName = TempIndex.shift_name,
                    working_hrs = TempIndex.working_minute_done.ToString().Contains("-") ? "00:00" : TempIndex.working_hour_done.ToString().Contains("-") ? "00:00" : Convert.ToDateTime(TempIndex.working_hour_done + ":" + TempIndex.working_minute_done).ToString("hh:mm") == ("12:00").ToString() ? "00:00" : Convert.ToDateTime(TempIndex.working_hour_done + ":" + TempIndex.working_minute_done).ToString("hh:mm"),
                }).ToList();

                return Ok(PayrollCalender);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }


        //Manual Punch Report
        [Route("ManualPunchReport")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.ManulPunch))]
        public IActionResult ManualPunchReport([FromForm]  Reports objReports)
        {
            try
            {
                bool allcompany_id = objReports.company_id == 0 ? true : false;
                bool allstate_id = objReports.state_id == 0 ? true : false;
                bool alllocation_id = objReports.location_id == 0 ? true : false;
                bool alldepartment_id = objReports.department_id == 0 ? true : false;
                bool allshift_id = objReports.Shift_id == 0 ? true : false;

                var data = (
                    from d in _context.tbl_daily_attendance
                    join em in _context.tbl_employee_master on d.emp_id equals em.employee_id
                    join e in _context.tbl_user_master on em.employee_id equals e.employee_id
                    join o in _context.tbl_emp_officaial_sec on d.emp_id equals o.employee_id
                    join s in _context.tbl_shift_details on d.shift_id equals s.shift_id
                    where
                    e.default_company_id == (allcompany_id ? e.default_company_id : objReports.company_id) &&
                    o.tbl_location_master.state_id == (allstate_id ? o.tbl_location_master.state_id : objReports.state_id) &&
                    o.location_id == (alllocation_id ? o.location_id : objReports.location_id) &&
                    o.department_id == (alldepartment_id ? o.department_id : objReports.department_id) &&
                    d.shift_id == (allshift_id ? d.shift_id : objReports.Shift_id) &&
                    d.attendance_dt >= objReports.FromDate && d.attendance_dt <= objReports.ToDate &&
                    em.is_active == 1 && e.is_active == 1 && o.is_deleted == 0 && s.is_deleted == 0 &&
                    d.is_regularize == 1 && _clsCurrentUser.CompanyId.Contains(e.default_company_id) && _clsCurrentUser.DownlineEmpId.Contains(e.employee_id ?? 0)
                    select new
                    {
                        e.employee_id,
                        o.card_number,
                        o.employee_first_name,
                        o.employee_middle_name,
                        o.employee_last_name,
                        d.shift_in_time,
                        d.shift_out_time,
                        d.in_time,
                        d.out_time,
                        d.attendance_dt,
                        d.day_status,
                        d.working_hour_done,
                        d.working_minute_done,
                        em.emp_code,
                        s.shift_name
                    }).ToList();

                //----------set payroll calender data--------------------------
                List<PresentReportData> PayrollCalender = new List<PresentReportData>();

                PayrollCalender = data.Select(TempIndex => new PresentReportData
                {
                    sno = data.IndexOf(TempIndex) + 1,
                    CardNo = TempIndex.card_number,
                    AttendanceDate = TempIndex.attendance_dt,
                    employee_first_name = !string.IsNullOrEmpty(TempIndex.employee_first_name) ? TempIndex.employee_first_name : " ",
                    employee_middle_name = !string.IsNullOrEmpty(TempIndex.employee_middle_name) ? TempIndex.employee_middle_name : " ",
                    employee_last_name = !string.IsNullOrEmpty(TempIndex.employee_last_name) ? TempIndex.employee_last_name : " ",
                    shift_in_time = TempIndex.shift_in_time,
                    shift_out_time = TempIndex.shift_out_time,
                    working_hour_done = TempIndex.working_hour_done,
                    working_minute_done = TempIndex.working_minute_done,
                    EmployeeCode = TempIndex.emp_code,
                    AttStatus = TempIndex.day_status,
                    InTime = TempIndex.in_time == null ? "" : TempIndex.in_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.in_time.ToString("hh:mm tt"),
                    OutTime = TempIndex.out_time == null ? "" : TempIndex.out_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.out_time.ToString("hh:mm tt"),
                    ShiftName = TempIndex.shift_name,
                    working_hrs = TempIndex.working_minute_done.ToString().Contains("-") ? "00:00" : TempIndex.working_hour_done.ToString().Contains("-") ? "00:00" : Convert.ToDateTime(TempIndex.working_hour_done + ":" + TempIndex.working_minute_done).ToString("hh:mm") == ("12:00").ToString() ? "00:00" : Convert.ToDateTime(TempIndex.working_hour_done + ":" + TempIndex.working_minute_done).ToString("hh:mm"),
                }).ToList();

                return Ok(PayrollCalender);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        #region ------Claim Master
        [Route("SaveClaimMasters")]
        [HttpPost]
        //[Authorize(Policy = "7071")]
        public IActionResult SaveClaimMasters([FromBody] clsClaimMaster claimmasters)
        {
            ResponseMsg objResult = new ResponseMsg();
            try
            {
                //check if exist
                tbl_claim_master tblclaim = _context.tbl_claim_master.FirstOrDefault(p => p.claim_master_name == claimmasters.claim_master_name && p.is_active == 1);

                if (tblclaim != null)
                {
                    objResult.Message = "Claim name already exist !";
                    objResult.StatusCode = 1;
                    return Ok(objResult);
                }

                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        tbl_claim_master claimmaster = new tbl_claim_master();

                        claimmaster.claim_master_name = claimmasters.claim_master_name;
                        claimmaster.is_active = 1;
                        claimmaster.created_by = 1;
                        claimmaster.created_date = DateTime.Now;
                        _context.tbl_claim_master.Add(claimmaster);
                        _context.SaveChanges();

                        //log table
                        tbl_claim_master_log tbllog = new tbl_claim_master_log();
                        tbllog.claim_master_id = claimmaster.claim_master_id;
                        tbllog.claim_master_name = claimmaster.claim_master_name;
                        tbllog.approved_by = 1;
                        tbllog.approved_date = DateTime.Now;
                        tbllog.is_approved = 1;
                        tbllog.requested_by = 1;
                        tbllog.requested_date = DateTime.Now;
                        tbllog.requested_type = 1;
                        _context.tbl_claim_master_log.Add(tbllog);
                        _context.SaveChanges();

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        objResult.Message = ex.Message;
                        objResult.StatusCode = 1;
                        return Ok(objResult);
                    }

                }

                objResult.Message = "Claim name save successfully !";
                objResult.StatusCode = 0;
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.Message = ex.Message;
                objResult.StatusCode = 1;
                return Ok(objResult);
            }
        }

        [Route("UpdateClaimMaster")]
        [HttpPost]
        //[Authorize(Policy = "7072")]
        public IActionResult UpdateClaimMaster([FromBody] tbl_claim_master claimmaster)
        {
            ResponseMsg objResult = new ResponseMsg();
            try
            {
                //check for valid claim id
                tbl_claim_master exist = _context.tbl_claim_master.FirstOrDefault(p => p.claim_master_id == claimmaster.claim_master_id);

                if (exist.claim_master_id == 0)
                {
                    objResult.Message = "Invalid claim id, please check and try !";
                    objResult.StatusCode = 1;
                    return Ok(objResult);
                }

                //check if duplicate
                tbl_claim_master tblclaim = _context.tbl_claim_master.FirstOrDefault(
                    p => p.claim_master_name == claimmaster.claim_master_name
                    && p.is_active == 1 && p.claim_master_id != claimmaster.claim_master_id);

                if (tblclaim != null)
                {
                    objResult.Message = "Claim name already exist !";
                    objResult.StatusCode = 1;
                    return Ok(objResult);
                }
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        exist.claim_master_name = claimmaster.claim_master_name;
                        exist.last_modified_by = 1;
                        exist.last_modified_date = DateTime.Now;
                        _context.tbl_claim_master.Update(exist);
                        _context.SaveChanges();

                        //insert in log table
                        tbl_claim_master_log tbllog = new tbl_claim_master_log();
                        tbllog.claim_master_id = claimmaster.claim_master_id;
                        tbllog.claim_master_name = claimmaster.claim_master_name;
                        tbllog.approved_by = 1;
                        tbllog.approved_date = DateTime.Now;
                        tbllog.is_approved = 1;
                        tbllog.requested_by = 1;
                        tbllog.requested_date = DateTime.Now;
                        tbllog.requested_type = 1;
                        _context.tbl_claim_master_log.Add(tbllog);
                        _context.SaveChanges();

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        objResult.Message = ex.Message;
                        objResult.StatusCode = 1;
                        return Ok(objResult);
                    }
                }

                objResult.Message = "Claim name updated successfully !";
                objResult.StatusCode = 0;
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.Message = ex.Message;
                objResult.StatusCode = 1;
                return Ok(objResult);
            }
        }

        [Route("GetClaimMaster/{claimid}")]
        [HttpGet]
        //[Authorize(Policy = "7073")]
        public IActionResult GetClaimMaster([FromRoute] int claimid)
        {
            try
            {
                if (claimid == 0)
                {
                    List<tbl_claim_master> claim = _context.tbl_claim_master.Where(p => p.is_active == 1).ToList();

                    return Ok(claim);
                }
                else
                {
                    List<tbl_claim_master> claim = _context.tbl_claim_master.Where(p => p.is_active == 1 && p.claim_master_id == claimid).ToList();
                    return Ok(claim);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #endregion
        //Late Punch In
        [Route("LatePunchIn")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LatePunchIn))]
        public IActionResult LatePunchIn([FromForm]  Reports objReports)
        {
            try
            {
                bool allcompany_id = objReports.company_id == 0 ? true : false;
                bool allstate_id = objReports.state_id == 0 ? true : false;
                bool alllocation_id = objReports.location_id == 0 ? true : false;
                bool alldepartment_id = objReports.department_id == 0 ? true : false;
                bool allshift_id = objReports.Shift_id == 0 ? true : false;

                var data = (
                    from d in _context.tbl_daily_attendance
                    join em in _context.tbl_employee_master on d.emp_id equals em.employee_id
                    join e in _context.tbl_user_master on em.employee_id equals e.employee_id
                    join o in _context.tbl_emp_officaial_sec on d.emp_id equals o.employee_id
                    join s in _context.tbl_shift_details on d.shift_id equals s.shift_id
                    where
                    e.default_company_id == (allcompany_id ? e.default_company_id : objReports.company_id) &&
                    o.tbl_location_master.state_id == (allstate_id ? o.tbl_location_master.state_id : objReports.state_id) &&
                    o.location_id == (alllocation_id ? o.location_id : objReports.location_id) &&
                    o.department_id == (alldepartment_id ? o.department_id : objReports.department_id) &&
                    d.shift_id == (allshift_id ? d.shift_id : objReports.Shift_id) &&
                    d.attendance_dt >= objReports.FromDate && d.attendance_dt <= objReports.ToDate &&
                    em.is_active == 1 && e.is_active == 1 && o.is_deleted == 0 && s.is_deleted == 0 &&
                    d.is_late_in == 1 && _clsCurrentUser.CompanyId.Contains(e.default_company_id) && _clsCurrentUser.DownlineEmpId.Contains(e.employee_id ?? 0)
                    select new
                    {
                        e.employee_id,
                        o.card_number,
                        o.employee_first_name,
                        o.employee_middle_name,
                        o.employee_last_name,
                        d.shift_in_time,
                        d.shift_out_time,
                        d.in_time,
                        d.out_time,
                        d.attendance_dt,
                        d.day_status,
                        d.working_hour_done,
                        d.working_minute_done,
                        em.emp_code,
                        s.shift_name
                    }).ToList();

                //----------set payroll calender data--------------------------
                List<PresentReportData> PayrollCalender = new List<PresentReportData>();

                PayrollCalender = data.Select(TempIndex => new PresentReportData
                {
                    sno = data.IndexOf(TempIndex) + 1,
                    CardNo = TempIndex.card_number,
                    AttendanceDate = TempIndex.attendance_dt,
                    employee_first_name = !string.IsNullOrEmpty(TempIndex.employee_first_name) ? TempIndex.employee_first_name : " ",
                    employee_middle_name = !string.IsNullOrEmpty(TempIndex.employee_middle_name) ? TempIndex.employee_middle_name : " ",
                    employee_last_name = !string.IsNullOrEmpty(TempIndex.employee_last_name) ? TempIndex.employee_last_name : " ",
                    shift_in_time = TempIndex.shift_in_time,
                    shift_out_time = TempIndex.shift_out_time,
                    working_hour_done = TempIndex.working_hour_done,
                    working_minute_done = TempIndex.working_minute_done,
                    EmployeeCode = TempIndex.emp_code,
                    AttStatus = TempIndex.day_status,
                    InTime = TempIndex.in_time == null ? "" : TempIndex.in_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.in_time.ToString("hh:mm tt"),
                    OutTime = TempIndex.out_time == null ? "" : TempIndex.out_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.out_time.ToString("hh:mm tt"),
                    ShiftName = TempIndex.shift_name,
                    working_hrs = TempIndex.working_minute_done.ToString().Contains("-") ? "00:00" : TempIndex.working_hour_done.ToString().Contains("-") ? "00:00" : Convert.ToDateTime(TempIndex.working_hour_done + ":" + TempIndex.working_minute_done).ToString("hh:mm") == ("12:00").ToString() ? "00:00" : Convert.ToDateTime(TempIndex.working_hour_done + ":" + TempIndex.working_minute_done).ToString("hh:mm"),
                }).ToList();

                return Ok(PayrollCalender);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        //Early Punch Out
        [Route("EarlyPunchOut")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.EarlyPunchout))]
        public IActionResult EarlyPunchOut([FromForm]  Reports objReports)
        {
            try
            {
                bool allcompany_id = objReports.company_id == 0 ? true : false;
                bool allstate_id = objReports.state_id == 0 ? true : false;
                bool alllocation_id = objReports.location_id == 0 ? true : false;
                bool alldepartment_id = objReports.department_id == 0 ? true : false;
                bool allshift_id = objReports.Shift_id == 0 ? true : false;

                var data = (
                    from d in _context.tbl_daily_attendance
                    join em in _context.tbl_employee_master on d.emp_id equals em.employee_id
                    join e in _context.tbl_user_master on em.employee_id equals e.employee_id
                    join o in _context.tbl_emp_officaial_sec on d.emp_id equals o.employee_id
                    join s in _context.tbl_shift_details on d.shift_id equals s.shift_id
                    where
                    e.default_company_id == (allcompany_id ? e.default_company_id : objReports.company_id) &&
                    o.tbl_location_master.state_id == (allstate_id ? o.tbl_location_master.state_id : objReports.state_id) &&
                    o.location_id == (alllocation_id ? o.location_id : objReports.location_id) &&
                    o.department_id == (alldepartment_id ? o.department_id : objReports.department_id) &&
                    d.shift_id == (allshift_id ? d.shift_id : objReports.Shift_id) &&
                    d.attendance_dt >= objReports.FromDate && d.attendance_dt <= objReports.ToDate &&
                    em.is_active == 1 && e.is_active == 1 && o.is_deleted == 0 && s.is_deleted == 0 &&
                    d.is_early_out == 1 && _clsCurrentUser.DownlineEmpId.Contains(e.employee_id ?? 0) && _clsCurrentUser.CompanyId.Contains(e.default_company_id)
                    select new
                    {
                        e.employee_id,
                        o.card_number,
                        o.employee_first_name,
                        o.employee_middle_name,
                        o.employee_last_name,
                        d.shift_in_time,
                        d.shift_out_time,
                        d.in_time,
                        d.out_time,
                        d.attendance_dt,
                        d.day_status,
                        d.working_hour_done,
                        d.working_minute_done,
                        em.emp_code,
                        s.shift_name
                    }).ToList();

                //----------set payroll calender data--------------------------
                List<PresentReportData> PayrollCalender = new List<PresentReportData>();

                PayrollCalender = data.Select(TempIndex => new PresentReportData
                {
                    sno = data.IndexOf(TempIndex) + 1,
                    CardNo = TempIndex.card_number,
                    AttendanceDate = TempIndex.attendance_dt,
                    employee_first_name = !string.IsNullOrEmpty(TempIndex.employee_first_name) ? TempIndex.employee_first_name : " ",
                    employee_middle_name = !string.IsNullOrEmpty(TempIndex.employee_middle_name) ? TempIndex.employee_middle_name : " ",
                    employee_last_name = !string.IsNullOrEmpty(TempIndex.employee_last_name) ? TempIndex.employee_last_name : " ",
                    shift_in_time = TempIndex.shift_in_time,
                    shift_out_time = TempIndex.shift_out_time,
                    working_hour_done = TempIndex.working_hour_done,
                    working_minute_done = TempIndex.working_minute_done,
                    EmployeeCode = TempIndex.emp_code,
                    AttStatus = TempIndex.day_status,
                    InTime = TempIndex.in_time == null ? "" : TempIndex.in_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.in_time.ToString("hh:mm tt"),
                    OutTime = TempIndex.out_time == null ? "" : TempIndex.out_time == Convert.ToDateTime("2000-01-01 00:00:00") ? "00:00:00" : TempIndex.out_time.ToString("hh:mm tt"),
                    ShiftName = TempIndex.shift_name,
                    working_hrs = TempIndex.working_minute_done.ToString().Contains("-") ? "00:00" : TempIndex.working_hour_done.ToString().Contains("-") ? "00:00" : Convert.ToDateTime(TempIndex.working_hour_done + ":" + TempIndex.working_minute_done).ToString("hh:mm") == ("12:00").ToString() ? "00:00" : Convert.ToDateTime(TempIndex.working_hour_done + ":" + TempIndex.working_minute_done).ToString("hh:mm"),
                }).ToList();

                return Ok(PayrollCalender);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        //Location By Report
        [Route("LocationByReport")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LocationPunchReport))]
        public IActionResult LocationByReport([FromForm]  Reports objReports)
        {
            try
            {
                bool allcompany_id = objReports.company_id == 0 ? true : false;
                bool allstate_id = objReports.state_id == 0 ? true : false;
                bool alllocation_id = objReports.location_id == 0 ? true : false;
                bool alldepartment_id = objReports.department_id == 0 ? true : false;
                bool allshift_id = objReports.Shift_id == 0 ? true : false;
                int company_id = objReports.company_id;
                int department_id = objReports.department_id;
                int location_id = objReports.location_id;
                int shift_id = objReports.Shift_id;

                var data = _context.tbl_attendance_details
                    .Where(a => a.tbl_machine_master.machine_id == a.machine_id
                    && a.tbl_employee_id_details.tbl_user_master.Where(q => q.default_company_id == (allcompany_id ? q.default_company_id : company_id)).Count() > 0
                     && a.tbl_machine_master.tbl_location_master.state_id == (allstate_id ? a.tbl_machine_master.tbl_location_master.state_id : objReports.state_id)
                    && a.tbl_machine_master.tbl_location_master.location_id == (alllocation_id ? a.tbl_machine_master.tbl_location_master.location_id : objReports.location_id)
                    && a.punch_time >= objReports.FromDate && a.punch_time <= objReports.ToDate.AddDays(1))
                    .Select(b => new
                    {
                        b.punch_time,
                        employee_first_name = b.tbl_employee_id_details.tbl_emp_officaial_sec.Where(d => d.is_deleted == 0).Select(s => s.employee_first_name).FirstOrDefault(),
                        employee_middle_name = b.tbl_employee_id_details.tbl_emp_officaial_sec.Where(d => d.is_deleted == 0).Select(s => s.employee_middle_name).FirstOrDefault(),
                        employee_last_name = b.tbl_employee_id_details.tbl_emp_officaial_sec.Where(d => d.is_deleted == 0).Select(s => s.employee_last_name).FirstOrDefault(),
                        card_numbers = b.tbl_employee_id_details.tbl_emp_officaial_sec.Where(d => d.is_deleted == 0).Select(s => s.card_number).FirstOrDefault(),
                        attendance_dt = b.entry_time,
                        b.tbl_machine_master.tbl_location_master.location_name,
                        sub_location_name = b.tbl_machine_master.tbl_sub_location_master.location_name,
                        b.emp_id,
                        sub_location_id = b.tbl_machine_master.sub_location_id != null ? b.tbl_machine_master.sub_location_id : 0,
                        dept_id = b.tbl_employee_id_details.tbl_emp_officaial_sec.Where(d => d.is_deleted == 0 && d.employee_first_name != null).Select(s => s.department_id).FirstOrDefault(),
                    }).OrderBy(a => a.punch_time).ToList();


                int employee_id = 0;
                int sub_location = 0;


                List<PresentReportData> list_obj_pres_1 = new List<PresentReportData>();

                DateTime FromDate = new DateTime();

                FromDate = objReports.FromDate;

                List<PresentReportData> list_obj_pres = new List<PresentReportData>();

                while (FromDate <= objReports.ToDate)
                {
                    var FilterDate = data.Where(a => a.punch_time.ToString("dd-MM-yyyy") == FromDate.ToString("dd-MM-yyyy") && a.dept_id == (alldepartment_id ? a.dept_id : objReports.department_id)).ToList();

                    while (FilterDate.Count() > 0)
                    {
                        PresentReportData obj_pres = new PresentReportData();

                        int i = 0;

                        employee_id = Convert.ToInt32(FilterDate[i].emp_id);
                        sub_location = Convert.ToInt32(FilterDate[i].sub_location_id);

                        //Get In Time 
                        var in_time = FilterDate.OrderBy(a => a.punch_time).Where(a => a.emp_id == employee_id && a.sub_location_id == sub_location).Select(a => new { a.punch_time, a.attendance_dt, a.emp_id }).FirstOrDefault();

                        obj_pres.EmployeeCode = _context.tbl_employee_master.Where(a => a.employee_id == employee_id).Select(a => a.emp_code).FirstOrDefault();
                        obj_pres.employee_first_name = FilterDate[i].employee_first_name;
                        obj_pres.employee_middle_name = FilterDate[i].employee_middle_name;
                        obj_pres.location_name = FilterDate[i].location_name;
                        obj_pres.sub_location_name = FilterDate[i].sub_location_name;
                        obj_pres.CardNo = FilterDate[i].card_numbers;
                        obj_pres.AttendanceDate = FilterDate[i].punch_time;
                        obj_pres.InTime = in_time != null ? in_time.punch_time.ToString("HH:mm:tt") : "00:00:00";

                        obj_pres.emp_id = employee_id;

                        if (in_time != null)
                        {
                            //Remove Data From Data List
                            FilterDate.Remove(FilterDate.Where(a => a.emp_id == employee_id && a.sub_location_id == sub_location).OrderBy(m => m.punch_time).FirstOrDefault());
                        }

                        //                      Get Out Time
                        var out_time = FilterDate.Where(a => a.emp_id == employee_id && a.sub_location_id == sub_location).Select(a => new { a.punch_time }).OrderBy(m => m.punch_time).FirstOrDefault();

                        string workhrs = "";
                        TimeSpan diff = new TimeSpan();

                        if (out_time != null)
                        {
                            diff = out_time.punch_time - (in_time != null ? in_time.punch_time : new DateTime(2000, 01, 01));
                        }
                        else
                        {
                            diff = in_time != null ? (in_time.punch_time - in_time.punch_time) : (new DateTime(2000, 01, 01) - new DateTime(2000, 01, 01));
                        }
                        workhrs = diff.ToString();

                        if (out_time == null)
                        {
                            obj_pres.OutTime = "00:00:00";
                        }
                        else
                        {
                            obj_pres.OutTime = out_time.punch_time.ToString("HH:mm:tt");
                        }


                        obj_pres.ShiftName = workhrs;

                        if (out_time != null)
                        {
                            FilterDate.Remove(FilterDate.Where(a => a.emp_id == employee_id && a.sub_location_id == sub_location).OrderBy(m => m.punch_time).FirstOrDefault());
                        }


                        list_obj_pres.Add(obj_pres);

                    }

                    list_obj_pres_1 = list_obj_pres;

                    FromDate = FromDate.AddDays(1);
                }

                var test = list_obj_pres_1;

                list_obj_pres_1.OrderBy(a => a.AttendanceDate);
                list_obj_pres_1.RemoveAll(p => !_clsCurrentUser.DownlineEmpId.Contains(p.emp_id));

                return Ok(list_obj_pres_1);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        //Performance Detail Report
        [Route("PerformanceDetailReport")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Report))]
        public IActionResult PerformanceDetailReport([FromBody] Reports objReports)
        {

            try
            {
                bool allcompany_id = objReports.company_id == 0 ? true : false;
                bool alllocation_id = objReports.location_id == 0 ? true : false;
                bool alldepartment_id = objReports.department_id == 0 ? true : false;
                bool allshift_id = objReports.Shift_id == 0 ? true : false;

                int dt_year = objReports.FromDate.Year;
                int dt_month = objReports.FromDate.Month;
                string filter_dt = "";
                if (dt_month < 10)
                {
                    filter_dt = dt_year + "0" + dt_month;
                }
                else
                {
                    filter_dt = dt_year + "" + dt_month;
                }

                int days = DateTime.DaysInMonth(objReports.FromDate.Year, objReports.FromDate.Month);

                DateTime from_dt = Convert.ToDateTime((dt_year) + "-" + dt_month + "-" + 01);
                DateTime to_dt = Convert.ToDateTime(dt_year + "-" + dt_month + "-" + days);

                var data = (
                    from d in _context.tbl_daily_attendance
                    join em in _context.tbl_employee_master on d.emp_id equals em.employee_id
                    join e in _context.tbl_user_master on em.employee_id equals e.employee_id
                    join o in _context.tbl_emp_officaial_sec on d.emp_id equals o.employee_id
                    join s in _context.tbl_shift_details on d.shift_id equals s.shift_id

                    where
                    e.default_company_id == (allcompany_id ? e.default_company_id : objReports.company_id) &&
                    o.location_id == (alllocation_id ? o.location_id : objReports.location_id) &&
                    o.department_id == (alldepartment_id ? o.department_id : objReports.department_id) &&
                    d.shift_id == (allshift_id ? d.shift_id : objReports.Shift_id) &&
                    d.attendance_dt >= from_dt && d.attendance_dt <= to_dt &&
                    em.is_active == 1 && e.is_active == 1 && o.is_deleted == 0 && s.is_deleted == 0 &&
                    (d.day_status == 1 || d.day_status == 2 || d.day_status == 3 || d.day_status == 4 || d.day_status == 5 || d.day_status == 6 || d.day_status == 0) && _clsCurrentUser.DownlineEmpId.Contains(e.employee_id ?? 0) && _clsCurrentUser.CompanyId.Contains(e.default_company_id)
                    select new
                    {
                        e.employee_id,
                        o.card_number,
                        o.employee_first_name,
                        o.employee_middle_name,
                        o.employee_last_name,
                        d.shift_in_time,
                        d.shift_out_time,
                        d.in_time,
                        d.out_time,
                        d.attendance_dt,
                        d.day_status,
                        d.is_weekly_off,
                        d.is_holiday,
                        d.working_hour_done,
                        d.working_minute_done,
                        em.emp_code,
                        s.shift_name
                    }).ToList();

                //----------set payroll calender data--------------------------

                List<InputRows> list = data.Select(p => new InputRows
                {
                    empID = p.employee_id ?? 0,
                    EmpCode = p.emp_code,
                    card_number = p.card_number,
                    employee_first_name = p.employee_first_name,
                    attendancday = Convert.ToString(p.attendance_dt.Day),
                    attendancstatus = p.day_status == 1 ? "<span style='color:#8b8be8'>P</span>" : p.is_weekly_off == 1 ? "<span style='color:#b3aeae'>W/O</span>" : p.is_holiday == 1 ? "<span style='color:#9dea27'>H</span>" : p.day_status == 2 ? "<span style='color:#ef925e'>A</span>" : p.day_status == 3 ? "<span style='color:#e46ee4'>L</span>" : p.day_status == 4 ? "<span style='color:#ef925e'>HP/HA</span>" : p.day_status == 5 ? "<span style='color:#e46ee4'>HP/HL</span>" : p.day_status == 6 ? "<span style='color:#ef925e'>HL/HA</span>" : "<span style='color:red'>A</span>"

                }).ToList();

                var result = list.Select(x => new
                {
                    //convert cols to list of rows
                    RowData = new List<Tuple<int, string, string, string, string, string>>()
                        {
                        Tuple.Create(x.empID ,x.attendancday, x.card_number, x.employee_first_name,x.attendancstatus, x.EmpCode),
                    }
                })
                    //get one result list
                    .SelectMany(x => x.RowData)
                    //group data by year
                    .GroupBy(x => x.Item1)
                    //finally get pivoted data
                    .Select((grp, counter) => new
                    {
                        card_number = grp.Key,
                        employee_code = list.FirstOrDefault(a => a.empID == grp.Key).EmpCode,
                        employee_first_name = list.FirstOrDefault(a => a.empID == grp.Key).employee_first_name,
                        day2 = grp.Where(y => y.Item2 == "2").Select(y => y.Item5).FirstOrDefault(),
                        day3 = grp.Where(y => y.Item2 == "3").Select(y => y.Item5).FirstOrDefault(),
                        day1 = grp.Where(y => y.Item2 == "1").Select(y => y.Item5).FirstOrDefault(),
                        day4 = grp.Where(y => y.Item2 == "4").Select(y => y.Item5).FirstOrDefault(),
                        day5 = grp.Where(y => y.Item2 == "5").Select(y => y.Item5).FirstOrDefault(),
                        day6 = grp.Where(y => y.Item2 == "6").Select(y => y.Item5).FirstOrDefault(),
                        day7 = grp.Where(y => y.Item2 == "7").Select(y => y.Item5).FirstOrDefault(),
                        day8 = grp.Where(y => y.Item2 == "8").Select(y => y.Item5).FirstOrDefault(),
                        day9 = grp.Where(y => y.Item2 == "9").Select(y => y.Item5).FirstOrDefault(),
                        day10 = grp.Where(y => y.Item2 == "10").Select(y => y.Item5).FirstOrDefault(),
                        day11 = grp.Where(y => y.Item2 == "11").Select(y => y.Item5).FirstOrDefault(),
                        day12 = grp.Where(y => y.Item2 == "12").Select(y => y.Item5).FirstOrDefault(),
                        day13 = grp.Where(y => y.Item2 == "13").Select(y => y.Item5).FirstOrDefault(),
                        day14 = grp.Where(y => y.Item2 == "14").Select(y => y.Item5).FirstOrDefault(),
                        day15 = grp.Where(y => y.Item2 == "15").Select(y => y.Item5).FirstOrDefault(),
                        day16 = grp.Where(y => y.Item2 == "16").Select(y => y.Item5).FirstOrDefault(),
                        day17 = grp.Where(y => y.Item2 == "17").Select(y => y.Item5).FirstOrDefault(),
                        day18 = grp.Where(y => y.Item2 == "18").Select(y => y.Item5).FirstOrDefault(),
                        day19 = grp.Where(y => y.Item2 == "19").Select(y => y.Item5).FirstOrDefault(),
                        day20 = grp.Where(y => y.Item2 == "20").Select(y => y.Item5).FirstOrDefault(),
                        day21 = grp.Where(y => y.Item2 == "21").Select(y => y.Item5).FirstOrDefault(),
                        day22 = grp.Where(y => y.Item2 == "22").Select(y => y.Item5).FirstOrDefault(),
                        day23 = grp.Where(y => y.Item2 == "23").Select(y => y.Item5).FirstOrDefault(),
                        day24 = grp.Where(y => y.Item2 == "24").Select(y => y.Item5).FirstOrDefault(),
                        day25 = grp.Where(y => y.Item2 == "25").Select(y => y.Item5).FirstOrDefault(),
                        day26 = grp.Where(y => y.Item2 == "26").Select(y => y.Item5).FirstOrDefault(),
                        day27 = grp.Where(y => y.Item2 == "27").Select(y => y.Item5).FirstOrDefault(),
                        day28 = grp.Where(y => y.Item2 == "28").Select(y => y.Item5).FirstOrDefault(),
                        day29 = grp.Where(y => y.Item2 == "29").Select(y => y.Item5).FirstOrDefault(),
                        day30 = grp.Where(y => y.Item2 == "30").Select(y => y.Item5).FirstOrDefault(),
                        day31 = grp.Where(y => y.Item2 == "31").Select(y => y.Item5).FirstOrDefault(),
                    });


                List<object> column = new List<object>();
                column.Add(new { title = "Emp ID", data = "card_number" });
                column.Add(new { title = "Code", data = "employee_code" });
                column.Add(new { title = "Employee Name", data = "employee_first_name" });

                for (int i = 1; i <= days; i++)
                {
                    column.Add(new
                    {
                        title = i,
                        data = "day" + i
                    });
                }

                var d2 = new { list = result, column };

                return Ok(d2);



            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        //Performance Detail Report
        [Route("PerformanceSummaryReport")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Report))]
        public IActionResult PerformanceSummaryReport([FromBody] Reports objReports)
        {

            try
            {
                bool allcompany_id = objReports.company_id == 0 ? true : false;
                bool alllocation_id = objReports.location_id == 0 ? true : false;
                bool alldepartment_id = objReports.department_id == 0 ? true : false;
                bool allshift_id = objReports.Shift_id == 0 ? true : false;
                int company_id = objReports.company_id;
                int department_id = objReports.department_id;
                int location_id = objReports.location_id;
                int shift_id = objReports.Shift_id;

                int dt_year = objReports.FromDate.Year;
                int dt_month = objReports.FromDate.Month;
                string filter_dt = "";
                if (dt_month < 10)
                {
                    filter_dt = dt_year + "0" + dt_month;
                }
                else
                {
                    filter_dt = dt_year + "" + dt_month;
                }

                int days = DateTime.DaysInMonth(objReports.FromDate.Year, objReports.FromDate.Month);

                DateTime from_dt = Convert.ToDateTime((dt_year) + "-" + dt_month + "-" + 01);
                DateTime to_dt = Convert.ToDateTime(dt_year + "-" + dt_month + "-" + days);


                ///Amarjeet
                var data = (
                    from d in _context.tbl_daily_attendance
                    join em in _context.tbl_employee_master on d.emp_id equals em.employee_id
                    join e in _context.tbl_user_master on em.employee_id equals e.employee_id
                    join s in _context.tbl_shift_details on d.shift_id equals s.shift_id
                    join o in _context.tbl_emp_officaial_sec on d.emp_id equals o.employee_id
                    where
                     e.default_company_id == (allcompany_id ? e.default_company_id : objReports.company_id) &&
                    o.location_id == (alllocation_id ? o.location_id : objReports.location_id) &&
                    o.department_id == (alldepartment_id ? o.department_id : objReports.department_id) &&
                    d.shift_id == (allshift_id ? d.shift_id : objReports.Shift_id) &&
                    d.attendance_dt >= from_dt && d.attendance_dt <= to_dt &&
                    em.is_active == 1 && e.is_active == 1 && o.is_deleted == 0 && s.is_deleted == 0 &&
                    (d.day_status == 1 || d.day_status == 2 || d.day_status == 3 || d.day_status == 4 || d.day_status == 5 || d.day_status == 6 || d.day_status == 0)
                    select new
                    {
                        o.card_number,
                        e.employee_id,
                        o.employee_first_name,
                        o.employee_middle_name,
                        o.employee_last_name,
                        d.shift_in_time,
                        d.shift_out_time,
                        d.in_time,
                        d.out_time,
                        d.attendance_dt,
                        d.day_status,
                        d.is_weekly_off,
                        d.is_holiday,
                        d.working_hour_done,
                        d.working_minute_done,
                        em.emp_code,
                        s.shift_name

                    }).ToList();

                //----------set payroll calender data--------------------------

                List<InputRows> list = data.Select(p => new InputRows
                {
                    empID = p.employee_id ?? 0,
                    EmpCode = p.emp_code,
                    card_number = p.card_number,
                    employee_first_name = String.Format("{0} {1} {2}", p.employee_first_name, p.employee_middle_name, p.employee_last_name),
                    attendancday = Convert.ToString(p.attendance_dt.Day),
                    attendancstatus = p.day_status == 1 ? "<b>S</b> -<span style='color:#8b8be8'>P</span> <br />" + "<b>I/T</b> - " + p.in_time.ToString("HH:mm tt") + "<br /><b>O/T</b> - " + p.out_time.ToString("HH:mm tt") + "<br /><b>W/H</b> - " + p.working_hour_done + ":" + p.working_minute_done : p.is_weekly_off == 1 ? "<b>S</b> -<span style='color:#b3aeae'>W/O</span> <br />" + "<b>I/T</b> - " + p.in_time.ToString("HH:mm tt") + "<br /><b>O/T</b> - " + p.out_time.ToString("HH:mm tt") + "<br /><b>W/H</b> - " + p.working_hour_done + ":" + p.working_minute_done : p.is_holiday == 1 ? "<b>S</b> -<span style='color:#9dea27'>H</span> <br />" + "<b>I/T</b> - " + p.in_time.ToString("HH:mm tt") + "<br /><b>O/T</b> - " + p.out_time.ToString("HH:mm tt") + "<br /><b>W/H</b> - " + p.working_hour_done + ":" + p.working_minute_done : p.day_status == 2 ? "<br /><b>S</b> - <span style='color:#ef925e'>A</span>" + "<br /><b>I/T</b> - " + p.in_time.ToString("HH:mm tt") + "<br /><b>O/T</b> - " + p.out_time.ToString("HH:mm tt") + "<br /><b>W/H</b> - " + p.working_hour_done + ":" + p.working_minute_done : p.day_status == 3 ? "<br /><b>S</b> - <span style='color:#e46ee4'>L</span>" + "<br /><b>I/T</b> - " + p.in_time.ToString("HH:mm tt") + "<br /><b>O/T</b> - " + p.out_time.ToString("HH:mm tt") + "<br /><b>W/H</b> - " + p.working_hour_done + ":" + p.working_minute_done : p.day_status == 4 ? "<br /><b>S</b> - <span style='color:#ef925e'>HP/HA</span>" + "<br /><b>I/T</b> - " + p.in_time.ToString("HH:mm tt") + "<br /><b>O/T</b> - " + p.out_time.ToString("HH:mm tt") + "<br /><b>W/H</b> - " + p.working_hour_done + ":" + p.working_minute_done : p.day_status == 5 ? "<br /><b>S</b> - <span style='color:#e46ee4'>HP/HL</span>" + "<br /><b>I/T</b> - " + p.in_time.ToString("HH:mm tt") + "<br /><b>O/T</b> - " + p.out_time.ToString("HH:mm tt") + "<br /><b>W/H</b> - " + p.working_hour_done + ":" + p.working_minute_done : p.day_status == 6 ? "<br /><b>S</b> - <span style='color:#ef925e'>HL/HA</span>" + "<br /><b>I/T</b> - " + p.in_time.ToString("HH:mm tt") + "<br /><b>O/T</b> - " + p.out_time.ToString("HH:mm tt") : "<br /><b>S</b> - <span style='color:#ef925e'>A</span>" + "<br /><b>I/T</b> - " + p.in_time.ToString("dd-MM-YYYY") + "<br /><b>O/T</b> - " + p.out_time.ToString("dd-MM-YYYY")

                }).ToList();

                list.RemoveAll(p => _clsCurrentUser.DownlineEmpId.Contains(p.empID));

                var result = list.Select(x => new
                {
                    //convert cols to list of rows
                    RowData = new List<Tuple<int, string, string, string, string, string>>()
                        {
                        Tuple.Create(x.empID ,x.attendancday, x.card_number, x.employee_first_name,x.attendancstatus,x.EmpCode),
                    }
                })
                    //get one result list
                    .SelectMany(x => x.RowData)
                    //group data by year
                    .GroupBy(x => x.Item1)
                    //finally get pivoted data
                    .Select((grp, counter) => new
                    {
                        card_number = grp.Key,
                        employee_code = list.FirstOrDefault(a => a.empID == grp.Key).EmpCode,
                        employee_first_name = list.FirstOrDefault(a => a.empID == grp.Key).employee_first_name,
                        day1 = grp.Where(y => y.Item2 == "1").Select(y => y.Item5).FirstOrDefault(),
                        day2 = grp.Where(y => y.Item2 == "2").Select(y => y.Item5).FirstOrDefault(),
                        day3 = grp.Where(y => y.Item2 == "3").Select(y => y.Item5).FirstOrDefault(),
                        day4 = grp.Where(y => y.Item2 == "4").Select(y => y.Item5).FirstOrDefault(),
                        day5 = grp.Where(y => y.Item2 == "5").Select(y => y.Item5).FirstOrDefault(),
                        day6 = grp.Where(y => y.Item2 == "6").Select(y => y.Item5).FirstOrDefault(),
                        day7 = grp.Where(y => y.Item2 == "7").Select(y => y.Item5).FirstOrDefault(),
                        day8 = grp.Where(y => y.Item2 == "8").Select(y => y.Item5).FirstOrDefault(),
                        day9 = grp.Where(y => y.Item2 == "9").Select(y => y.Item5).FirstOrDefault(),
                        day10 = grp.Where(y => y.Item2 == "10").Select(y => y.Item5).FirstOrDefault(),
                        day11 = grp.Where(y => y.Item2 == "11").Select(y => y.Item5).FirstOrDefault(),
                        day12 = grp.Where(y => y.Item2 == "12").Select(y => y.Item5).FirstOrDefault(),
                        day13 = grp.Where(y => y.Item2 == "13").Select(y => y.Item5).FirstOrDefault(),
                        day14 = grp.Where(y => y.Item2 == "14").Select(y => y.Item5).FirstOrDefault(),
                        day15 = grp.Where(y => y.Item2 == "15").Select(y => y.Item5).FirstOrDefault(),
                        day16 = grp.Where(y => y.Item2 == "16").Select(y => y.Item5).FirstOrDefault(),
                        day17 = grp.Where(y => y.Item2 == "17").Select(y => y.Item5).FirstOrDefault(),
                        day18 = grp.Where(y => y.Item2 == "18").Select(y => y.Item5).FirstOrDefault(),
                        day19 = grp.Where(y => y.Item2 == "19").Select(y => y.Item5).FirstOrDefault(),
                        day20 = grp.Where(y => y.Item2 == "20").Select(y => y.Item5).FirstOrDefault(),
                        day21 = grp.Where(y => y.Item2 == "21").Select(y => y.Item5).FirstOrDefault(),
                        day22 = grp.Where(y => y.Item2 == "22").Select(y => y.Item5).FirstOrDefault(),
                        day23 = grp.Where(y => y.Item2 == "23").Select(y => y.Item5).FirstOrDefault(),
                        day24 = grp.Where(y => y.Item2 == "24").Select(y => y.Item5).FirstOrDefault(),
                        day25 = grp.Where(y => y.Item2 == "25").Select(y => y.Item5).FirstOrDefault(),
                        day26 = grp.Where(y => y.Item2 == "26").Select(y => y.Item5).FirstOrDefault(),
                        day27 = grp.Where(y => y.Item2 == "27").Select(y => y.Item5).FirstOrDefault(),
                        day28 = grp.Where(y => y.Item2 == "28").Select(y => y.Item5).FirstOrDefault(),
                        day29 = grp.Where(y => y.Item2 == "29").Select(y => y.Item5).FirstOrDefault(),
                        day30 = grp.Where(y => y.Item2 == "30").Select(y => y.Item5).FirstOrDefault(),
                        day31 = grp.Where(y => y.Item2 == "31").Select(y => y.Item5).FirstOrDefault(),
                    });


                //int days = DateTime.DaysInMonth(objReports.FromDate.Year, objReports.FromDate.Month);
                List<object> column = new List<object>();


                //      column.Add(new { title = "Emp Id", data = "card_number" });
                column.Add(new { title = "Code", data = "employee_code" });
                column.Add(new { title = "Employee Name", data = "employee_first_name" });

                for (int i = 1; i <= days; i++)
                {
                    column.Add(new
                    {
                        title = i,
                        data = "day" + i
                    });
                }

                ////var d1 = new { list = dd, column };
                //int payrollmonthyear = 0;
                //int.TryParse(filter_dt, out payrollmonthyear);
                //var data =
                //  _context.tbl_daily_attendance.Where(p => p.payrollmonthyear == payrollmonthyear
                //  && p.tbl_employee_master.tbl_user_master.Where(q => q.default_company_id == (allcompany_id ? q.default_company_id : company_id)).Count() > 0
                //  && p.tbl_emp_officaial_sec.location_id == (alllocation_id ? p.tbl_emp_officaial_sec.location_id : location_id)
                //  && p.tbl_emp_officaial_sec.department_id == (alldepartment_id ? p.tbl_emp_officaial_sec.department_id : department_id)
                //  && p.shift_id == (allshift_id ? p.shift_id : shift_id)
                //  //&& p.day_status == 1 && p.day_status == 2
                //  ).Select(p => new { p.in_time, p.out_time, p.tbl_emp_officaial_sec.employee_first_name, p.tbl_emp_officaial_sec.employee_middle_name, p.tbl_emp_officaial_sec.card_number, p.attendance_dt, p.tbl_employee_master.emp_code, p.day_status }).ToList();

                var d2 = new { list = result, column };

                return Ok(d2);

                //  return Ok(data);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        #region----role and claim map
        [Route("GetRoleMaster")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.RoleAllocation))]
        public IActionResult GetRoleData()
        {
            try
            {
                var role = _context.tbl_role_master.Where(p => p.is_active == 1).Select(a => new
                {
                    roleid = a.role_id,
                    rolename = a.role_name
                }).ToList();

                return Ok(role);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //get role claim master
        [Route("GetClaimByRoleId/{roleid}")]
        [HttpGet]
        //[Authorize(Policy = "7080")]
        public IActionResult GetClaimByRoleId([FromRoute] int roleid)
        {
            try
            {
                List<clsRoleClaimModel> Claim = _context.tbl_claim_master.Where(x => x.is_active == 1).Select(a => new clsRoleClaimModel
                {
                    claimid = a.claim_master_id,
                    claimname = a.claim_master_name,
                    ischecked = false
                }).ToList();

                //check if role
                if (roleid > 0)
                {
                    var RoleClaimList = _context.tbl_role_claim_map.Where(p => p.role_id == roleid && p.is_deleted == 0).ToList();

                    Claim.ForEach(p =>
                    {
                        var Temp = RoleClaimList.FirstOrDefault(a => a.claim_id == p.claimid);
                        p.ischecked = Temp != null ? true : false;
                    });
                }

                return Ok(Claim);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //save role and claim mapping data
        [Route("SaveRoleRoleAndClaimMap")]
        [HttpPost]
        //[Authorize(Policy = "7081")]
        public IActionResult SaveRoleRoleAndClaimMap([FromBody] clsRoleClaimMap rolemap)
        {
            ResponseMsg ObjResult = new ResponseMsg();
            try
            {
                if (rolemap.claimids.Count > 0)
                {
                    //check if exist
                    List<int> existclaim = new List<int>();
                    List<tbl_role_claim_map> exist = _context.tbl_role_claim_map.Where(p => rolemap.claimids.Contains(p.claim_id ?? 0) && p.role_id == rolemap.roleid).ToList();

                    //get all already mapped role data
                    var mappedclaim = _context.tbl_role_claim_map.Where(p => p.role_id == rolemap.roleid).ToList();
                    //now set data for update delete/inactive claims rows
                    mappedclaim.ForEach(p =>
                    {
                        var temp = rolemap.claimids.Contains(p.claim_id ?? 0);
                        p.is_deleted = temp == false ? 1 : 0;
                        p.last_modified_by = 1;
                        p.last_modified_date = DateTime.Now;
                    });

                    if (exist != null)
                    {
                        //var ClaimName = _context.tbl_claim_master.FirstOrDefault(p => p.is_active == 1 && rolemap.claimids.Contains(p.claim_master_id)).claim_master_name;

                        //ObjResult.Message = "Claim name " + ClaimName + " already mapped with selected role, Please check and try";
                        //ObjResult.StatusCode = 1;
                        //return Ok(ObjResult);

                        //remove which are already exist 
                        existclaim = exist.Select(p => p.claim_id ?? 0).ToList();
                        rolemap.claimids.RemoveAll(p => existclaim.Contains(p));
                    }

                    //save in db
                    using (var trans = _context.Database.BeginTransaction())
                    {
                        try
                        {

                            List<tbl_role_claim_map> map = new List<tbl_role_claim_map>();
                            for (int i = 0; i < rolemap.claimids.Count; i++)
                            {
                                map.Add(new tbl_role_claim_map
                                {
                                    claim_id = rolemap.claimids[i],
                                    role_id = rolemap.roleid,
                                    created_by = 1,
                                    created_date = DateTime.Now,
                                    is_deleted = 0
                                });
                            }

                            _context.tbl_role_claim_map.AddRange(map);

                            //now update
                            _context.tbl_role_claim_map.UpdateRange(mappedclaim);
                            _context.SaveChanges();

                            trans.Commit();
                            ObjResult.Message = "Claim name mapped successfully !!";
                            ObjResult.StatusCode = 0;
                            return Ok(ObjResult);


                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            ObjResult.Message = ex.Message;
                            ObjResult.StatusCode = 1;
                            return Ok(ObjResult);
                        }

                    }


                }
                else
                {
                    ObjResult.Message = "Please select data !!";
                    ObjResult.StatusCode = 1;
                    return Ok(ObjResult);
                }
            }
            catch (Exception ex)
            {
                ObjResult.Message = ex.Message;
                ObjResult.StatusCode = 1;
                return Ok(ObjResult);
            }
        }

        #endregion

        #region---User role mapping
        //get all employeeuser list
        [Route("GetEmployeeUserList/{roleid}")]
        [HttpGet]
        //[Authorize(Policy = "7082")]
        public IActionResult GetEmployeeUserList([FromRoute] int roleid)
        {

            try
            {
                var AllUsers = _context.tbl_user_master.Where(a => a.is_active == 1).Select(a => new { employee_id = a.employee_id, a.user_id }).ToList();

                List<clsUserRoleModel> EmpData = new List<clsUserRoleModel>();

                for (int index = 0; index < AllUsers.Count; index++)
                {
                    var UserDetails = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == AllUsers[index].employee_id && a.is_deleted == 0 && a.employee_first_name != null).OrderByDescending(a => a.emp_official_section_id).Select(a => new { userid = AllUsers[index].user_id, empid = a.employee_id, empcode = a.tbl_employee_id_details.emp_code, empname = a.employee_first_name ?? "", empdept = a.tbl_department_master.department_name, empdesig = "", ischecked = false }).FirstOrDefault();
                    if (UserDetails != null)
                    {
                        EmpData.Add(new clsUserRoleModel
                        {
                            empid = UserDetails.empid ?? 0,
                            empname = UserDetails.empname,
                            empcode = UserDetails.empcode,
                            empdept = UserDetails.empdept,
                            empdesig = "",
                            ischecked = false,
                            userid = Convert.ToInt32(UserDetails.userid)
                        });
                    }
                }


                //List<clsUserRoleModel> EmpData = _context.tbl_emp_officaial_sec.Where
                //    (p => p.tbl_department_master.department_id == p.department_id &&
                //  p.tbl_employee_id_details.tbl_user_master.FirstOrDefault(a => a.is_active == 1).employee_id == p.employee_id && p.is_deleted == 0).OrderBy(b => b.employee_id)
                //.Select(c => new clsUserRoleModel
                //{
                //    empid = c.employee_id ?? 0,
                //    empname = c.employee_first_name,
                //    empcode = c.tbl_employee_id_details.emp_code,
                //    empdept = c.tbl_department_master.department_name,
                //    empdesig = "",
                //    ischecked = false,
                //    userid = c.tbl_employee_id_details.tbl_user_master.First(a => a.is_active == 1 && a.employee_id == c.employee_id).user_id,
                //}).ToList();

                //---update designation here
                var DesgList = _context.tbl_emp_desi_allocation.Where(p => p.tbl_designation_master.designation_id == p.desig_id && p.tbl_designation_master.designation_name != null)
                    .Select(a => new
                    {
                        empid = a.employee_id,
                        designame = a.tbl_designation_master.designation_name,
                        fromdate = a.applicable_from_date
                    }).ToList();

                EmpData.ForEach(p =>
                {
                    var Temp = DesgList.Where(a => a.empid == p.empid).OrderByDescending(b => b.fromdate).FirstOrDefault();
                    p.empdesig = Temp != null ? Temp.designame : "";
                });

                //check if role
                if (roleid > 0)
                {
                    var RoleUserList = _context.tbl_user_role_map.Where(p => p.role_id == roleid && p.is_deleted == 0).ToList();

                    EmpData.ForEach(p =>
                    {
                        var Temp = RoleUserList.FirstOrDefault(a => a.user_id == p.userid);
                        p.ischecked = Temp != null ? true : false;
                    });
                }

                return Ok(EmpData);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #region ** STARTED BY SUPRIYA ON 10-08-2019, SAVE ROLE **

        [Route("SaveUserRoleMapping")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.RoleAllocation))]
        public IActionResult SaveUserRoleMapping([FromBody] clsUserRoleMap roleuser)
        {
            ResponseMsg ObjResult = new ResponseMsg();




            List<int> _empid = _context.tbl_user_master.Where(x => x.is_active == 1 && roleuser.userid.Contains(x.user_id)).Select(p => p.employee_id ?? 0).ToList();

            foreach (var id in _empid)
            {
                if (!_clsCurrentUser.DownlineEmpId.Contains(id))
                {
                    ObjResult.StatusCode = 1;
                    ObjResult.Message = "Unauthorize Access...!!";
                    return Ok(ObjResult);
                }
            }



            try
            {
                var rolee_id = roleuser.roleid;
                var created_by = roleuser.created_by;

                using (var db = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var user_Dtl = _context.tbl_user_role_map.Where(x => x.is_deleted == 0 && roleuser.userid.Contains(x.user_id ?? 0)).ToList();
                        _context.tbl_user_role_map.RemoveRange(user_Dtl);


                        List<tbl_user_role_map> _newrole = new List<tbl_user_role_map>();

                        _newrole = roleuser.userid.Select(p => new tbl_user_role_map
                        {
                            role_id = rolee_id,
                            user_id = p,
                            is_deleted = 0,
                            created_by = created_by,
                            created_date = DateTime.Now
                        }).ToList();

                        _context.tbl_user_role_map.AddRange(_newrole);
                        _context.SaveChanges();

                        db.Commit();

                        ObjResult.StatusCode = 0;
                        ObjResult.Message = "User role mapped successfully !!";
                    }
                    catch (Exception ex)
                    {
                        db.Rollback();
                        ObjResult.StatusCode = 1;
                        ObjResult.Message = ex.Message;
                    }
                }

                return Ok(ObjResult);
            }
            catch (Exception ex)
            {
                ObjResult.StatusCode = 1;
                ObjResult.Message = ex.Message;
                return Ok(ObjResult);
            }
        }


        #endregion ** END BY SUPRIYA ON 10-08-2019, SAVE ROLE **

        #endregion




        #region-------Sub Location Master-------
        //save sub location master
        [Route("Save_SubLocationMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.CreateLocation))]
        public IActionResult Save_SubLocationMaster([FromBody] tbl_sub_location_master objsubloc)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(objsubloc.created_by))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize access....!!";
                return Ok(objResult);
            }
            try
            {
                var exist = _context.tbl_sub_location_master.Where(
                    x => x.location_name.Trim().ToUpper() == objsubloc.location_name.Trim().ToUpper()
                    && x.location_id == objsubloc.location_id
                    ).FirstOrDefault();

                if (exist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Sub location name already exist !!";
                    return Ok(objResult);
                }
                else
                {
                    using (var tran = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            objsubloc.is_active = Convert.ToInt32(objsubloc.is_active);
                            objsubloc.created_by = _clsCurrentUser.EmpId;
                            objsubloc.created_date = DateTime.Now;

                            _context.Entry(objsubloc).State = EntityState.Added;
                            _context.SaveChanges();
                            tran.Commit();
                            objResult.StatusCode = 0;
                            objResult.Message = "Sub location name save successfully !!";


                        }
                        catch (Exception ex)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = ex.Message;
                            tran.Rollback();
                        }

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



        //get sub location master data
        [Route("Get_SubLocationMaster/{sublocid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LocationReport))]
        public IActionResult Get_SubLocationMaster([FromRoute] int sublocid)
        {
            try
            {
                if (sublocid > 0)
                {
                    var data = _context.tbl_sub_location_master.Join(
                         _context.tbl_location_master,
                         Subloc => Subloc.location_id,
                         loc => loc.location_id,
                         (subloc, loc) => new
                         {
                             company_id = loc.company_id,
                             location_id = subloc.location_id,
                             location_name = subloc.location_name,
                             sub_location_id = subloc.sub_location_id,
                             is_active = subloc.is_active
                         }
                        ).Where(x => x.sub_location_id == sublocid).FirstOrDefault();

                    // var data = _context.tbl_sub_location_master.Where(x => x.sub_location_id == sublocid).FirstOrDefault();
                    return Ok(data);
                }
                else
                {
                    var data = _clsCompanyDetails.GetSubLocations();
                    //var data = (from a in _context.tbl_sub_location_master
                    //            join b in _context.tbl_location_master on a.location_id equals b.location_id
                    //            join c in _context.tbl_company_master on b.company_id equals c.company_id
                    //            join e in _context.tbl_user_master on a.created_by equals e.user_id
                    //            select new
                    //            {
                    //                a.location_id,
                    //                c.company_id,
                    //                a.sub_location_id,
                    //                sub_locationname = a.location_name,
                    //                loc_name = b.location_name,
                    //                c.company_name,
                    //                e.username,
                    //                a.created_date,
                    //                a.is_active
                    //            }).AsEnumerable().Select((a, index) => new
                    //            {
                    //                sublocationname = a.sub_locationname,
                    //                locationname = a.loc_name,
                    //                sublocid = a.sub_location_id,
                    //                companyid = a.company_id,
                    //                companyname = a.company_name,
                    //                status = a.is_active,
                    //                createdby = a.username,
                    //                createdon = a.created_date,
                    //                sno = index + 1
                    //            }).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Get_SubLocationMasterByCompany/{company_id}/{fromdate}/{todate}/{locationtype}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LocationReport))]
        public IActionResult Get_SubLocationMasterByCompany([FromRoute] int company_id, DateTime fromdate, DateTime todate, int locationtype)
        {
            try
            {
                if (company_id > 0)
                {
                    if (locationtype > 0)
                    {
                        var data = _clsCompanyDetails.GetSubLocations().Where(x => x.company_id == company_id && x.location_type_id == locationtype && _clsCurrentUser.CompanyId.Contains(x.company_id) && fromdate.Date <= x.created_on.Date && x.created_on.Date <= todate.Date).ToList();

                        return Ok(data);
                    }
                    else
                    {
                        var data = _clsCompanyDetails.GetSubLocations().Where(x => x.company_id == company_id && _clsCurrentUser.CompanyId.Contains(x.company_id) && fromdate.Date <= x.created_on.Date && x.created_on.Date <= todate.Date).ToList();
                        return Ok(data);
                    }
                }
                else
                {
                    if (locationtype > 0)
                    {
                        var data = _clsCompanyDetails.GetSubLocations().Where(x => x.location_type_id == locationtype && _clsCurrentUser.CompanyId.Contains(x.company_id) && fromdate.Date <= x.created_on.Date && x.created_on.Date <= todate.Date).ToList();

                        return Ok(data);
                    }
                    else
                    {
                        var data = _clsCompanyDetails.GetSubLocations().Where(x => _clsCurrentUser.CompanyId.Contains(x.company_id) && fromdate.Date <= x.created_on.Date && x.created_on.Date <= todate.Date).ToList();
                        return Ok(data);
                    }
                }
            }

            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //update sub department master
        [Route("Update_SubLocationMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.CreateLocation))]
        public async Task<IActionResult> Update_SubLocationMaster([FromBody] tbl_sub_location_master objsubloc)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.DownlineEmpId.Contains(objsubloc.last_modified_by))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access";
                return Ok(objResult);
            }

            //  tbl_sub_location_master_log objlog = new tbl_sub_location_master_log();
            try
            {
                var exist = _context.tbl_sub_location_master.Where(x => x.sub_location_id == objsubloc.sub_location_id).FirstOrDefault();

                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid sub location id please try again !!";
                    return Ok(objResult);
                }
                else
                {

                    //check with same department name in other department id
                    var duplicate = _context.tbl_sub_location_master.Where(x => x.sub_location_id != objsubloc.sub_location_id
                    && x.location_id == objsubloc.location_id
                    && x.location_name.Trim().ToUpper() == objsubloc.location_name.Trim().ToUpper()).FirstOrDefault();

                    if (duplicate != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Sub location name exist in company dept. please check & try !!";
                        return Ok(objResult);
                    }
                    using (var trans = _context.Database.BeginTransaction())
                    {
                        exist.is_active = Convert.ToInt32(objsubloc.is_active);
                        exist.last_modified_by = 1;
                        exist.last_modified_date = DateTime.Now;
                        exist.location_id = objsubloc.location_id;
                        exist.location_name = objsubloc.location_name;

                        _context.tbl_sub_location_master.Attach(exist);
                        _context.Entry(exist).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                        //_context.Entry(exist).State = EntityState.Added;
                        //_context.SaveChanges();
                        trans.Commit(); //commit transaction
                        objResult.StatusCode = 0;
                        objResult.Message = "Sub location name updated successfully !!";


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


        #endregion



        [HttpGet]
        [Route("Gettbl_SubLocation")]
        //[Authorize(Policy = "7088")]
        public IEnumerable<tbl_sub_location_master> Gettbl_SubLocation()
        {
            return _context.tbl_sub_location_master;
        }

        public class InputRows
        {
            public int empID { get; set; }
            public string EmpCode { get; set; }
            public string card_number { get; set; }
            public string employee_first_name { get; set; }
            public string attendancday { get; set; }
            public string attendancstatus { get; set; }
            public string department_name { get; set; }
            public string location_name { get; set; }
            public DateTime date_of_joining { get; set; }
            public DateTime attendance_dt { get; set; }
            public int day_status { get; set; }
            public int is_weekly_off { get; set; }
            public int is_holiday { get; set; }
            public int is_comp_off { get; set; }
            public int is_outdoor { get; set; }
            public decimal present_count { get; set; }
            public decimal absent_count { get; set; }
            public decimal leave_count { get; set; }
            public decimal weekly_off_count { get; set; }
            public decimal holidays_count { get; set; }
            public decimal comp_off_count { get; set; }
            public decimal outdoor_count { get; set; }

        }



        #region *********** Created By : Amarjeet, Created Date : 03-04-2019, Mobile API ****************************************************************

        //UserProfile API
        //[Authorize()]
        [HttpGet("UserProfile/{emp_id}/{company_id}")]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public async Task<IActionResult> UserProfile([FromRoute] int emp_id, int company_id)
        {

            ResponseMsg objResult = new ResponseMsg();

            if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }

            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }

            try
            {

                int is_hod = 1;
                int total_employee_in_company = 0;
                int present_employee = 0;
                int absent_employee = 0;
                int total_employee_under_hod = 0;


                //var is_reporting_manager = _context.tbl_emp_manager.Where(r => (r.m_one_id == emp_id || r.m_two_id == emp_id || r.m_three_id == emp_id) && r.is_deleted == 0).OrderByDescending(r => r.emp_mgr_id).FirstOrDefault();
                var is_reporting_manager = _clsEmployeeDetail.Get_Emp_manager_dtl(emp_id);
                if (is_reporting_manager != null && is_reporting_manager.Count > 0)
                {
                    if (is_reporting_manager[0].final_approval == 1 && is_reporting_manager[0].m_one_id == emp_id)
                    {
                        is_hod = 1;
                    }
                    else if (is_reporting_manager[0].final_approval == 2 && is_reporting_manager[0].m_two_id == emp_id)
                    {
                        is_hod = 1;
                    }
                    else if (is_reporting_manager[0].final_approval == 3 && is_reporting_manager[0].m_three_id == 3)
                    {
                        is_hod = 1;
                    }
                }

                ///
                // var get_user_id = _context.tbl_user_master.Where(a => a.employee_id == emp_id && a.is_active == 1).FirstOrDefault();

                //var is_superadmin = _context.tbl_user_role_map.Where(r => r.user_id == get_user_id.user_id && r.is_deleted == 0 && r.role_id == 1).FirstOrDefault();
                string current_date = DateTime.Now.Date.ToString("dd-MMM-yyyy");
                DateTime current_date_ = Convert.ToDateTime(current_date);
                try
                {
                    if (_clsCurrentUser.Is_Hod == 2)
                    {
                        is_hod = 2;


                        //Get Total Employee in this company
                        total_employee_in_company = _context.tbl_user_master.Where(a => a.default_company_id == company_id && a.is_active == 1).Select(a => a.employee_id).Count();

                        //Get Total Present Employee

                        present_employee = (from u in _context.tbl_user_master
                                            join e in _context.tbl_daily_attendance on u.employee_id equals e.emp_id
                                            where u.default_company_id == company_id && u.is_active == 1 && e.day_status == 1 && e.attendance_dt == current_date_
                                            select new
                                            {
                                                u.employee_id
                                            }
                                                ).Count();


                        //Get Total Absent Employee

                        absent_employee = (from u in _context.tbl_user_master
                                           join e in _context.tbl_daily_attendance on u.employee_id equals e.emp_id
                                           where u.default_company_id == company_id && u.is_active == 1 && e.day_status == 2 && e.attendance_dt == Convert.ToDateTime(current_date)
                                           select new
                                           {
                                               u.employee_id
                                           }).Count();
                    }
                    else if (_clsCurrentUser.Is_Hod == 1)
                    {
                        is_hod = 1;

                        //Get Total Employee under Hod

                        //Count Emp Under hod
                        List<int> _loginid = new List<int>();
                        _loginid.Add(_clsCurrentUser.EmpId);
                        total_employee_under_hod = _clsCurrentUser.DownlineEmpId.Except(_loginid).Distinct().Count(); ;//employee_under_hod.Select(a => a.employee_id).Distinct().Count();

                        // emp under hod
                        var emp_id_under_hod = _clsCurrentUser.DownlineEmpId.Except(_loginid).Distinct().ToList();//employee_under_hod.Select(a => new { a.employee_id }).Distinct();


                        present_employee = (from u in _context.tbl_user_master
                                            join e in _context.tbl_daily_attendance on u.employee_id equals e.emp_id
                                            where u.default_company_id == company_id && u.is_active == 1 && e.day_status == 1 && e.attendance_dt == Convert.ToDateTime(current_date)
                                            select new
                                            {
                                                u.employee_id
                                            }
                                                ).Count();


                        //Get Total Absent Employee

                        absent_employee = (from u in _context.tbl_user_master
                                           join e in _context.tbl_daily_attendance on u.employee_id equals e.emp_id
                                           where u.default_company_id == company_id && u.is_active == 1 && e.day_status == 2 && e.attendance_dt == Convert.ToDateTime(current_date)
                                           select new
                                           {
                                               u.employee_id
                                           }).Count();
                    }
                }
                finally { }
                string domain_url = _appSettings.Value.domain_url;

                var personal_dtl = _context.tbl_emp_personal_sec.Where(x => x.is_deleted == 0 && x.employee_id == emp_id).FirstOrDefault();
                var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && x.employee_id == emp_id).Select(p => new
                {
                    p.official_email_id,
                    p.employee_photo_path,
                    p.date_of_birth,
                    p.date_of_joining,
                    p.tbl_department_master.department_name,
                    p.tbl_location_master.location_name,
                }).FirstOrDefault();


                var designation_ = _context.tbl_emp_desi_allocation.OrderByDescending(q => q.emp_grade_id).Where(p => p.employee_id == emp_id && p.applicable_from_date <= DateTime.Now.Date && DateTime.Now.Date <= p.applicable_to_date.Date && p.desig_id != null)
                                   .GroupBy(t => t.employee_id).Select(g => g.OrderByDescending(t => t.emp_grade_id).FirstOrDefault()).Select(p => p.tbl_designation_master.designation_name).FirstOrDefault();




                //  var designation_ = _context.tbl_emp_desi_allocation.OrderByDescending(y => y.emp_grade_id).Where(x => x.employee_id == emp_id && x.desig_id != null 
                //  && x.applicable_from_date.Date <= DateTime.Now.Date && DateTime.Now.Date <= x.applicable_to_date.Date).Select(p=>p.tbl_designation_master.designation_name).FirstOrDefault();
                var grade_ = _context.tbl_emp_grade_allocation.OrderByDescending(y => y.emp_grade_id).Where(x => x.employee_id == emp_id && x.grade_id != null
                                      && x.applicable_from_date.Date <= DateTime.Now.Date && DateTime.Now.Date <= x.applicable_to_date.Date).GroupBy(t => t.employee_id).Select(g => g.OrderByDescending(t => t.emp_grade_id).FirstOrDefault()).Select(p => p.tbl_grade_master.grade_name).FirstOrDefault();

                var data = _clsEmployeeDetail.GetEmployeeByDate(company_id, Convert.ToDateTime("01-01-2000"), DateTime.Now.AddDays(1)).Where(x => x.employee_id == emp_id);

                var User_Profile_details = data.Select(p => new
                {

                    p.emp_name,
                    p.emp_code,
                    p.company_name,
                    permanent_address_line_one = personal_dtl != null ? (personal_dtl.permanent_address_line_one != null ? personal_dtl.permanent_address_line_one : "") : "",
                    permanent_address_line_two = personal_dtl != null ? (personal_dtl.permanent_address_line_two != null ? personal_dtl.permanent_address_line_two : "") : "",
                    country_name = personal_dtl != null ? personal_dtl.permanent_country < 1 ? "" : _context.tbl_country.OrderByDescending(y => y.country_id).FirstOrDefault(x => x.country_id == personal_dtl.permanent_country).name : "",
                    state_name = personal_dtl != null ? personal_dtl.permanent_state < 1 ? "" : _context.tbl_state.OrderByDescending(y => y.state_id).FirstOrDefault(x => x.state_id == personal_dtl.permanent_state).name : "",
                    city_name = personal_dtl != null ? personal_dtl.permanent_city < 1 ? "" : _context.tbl_city.OrderByDescending(y => y.city_id).FirstOrDefault(x => x.city_id == personal_dtl.permanent_city).name : "",
                    designation_name = designation_ != null ? designation_ : "",
                    official_email_id = teos != null ? teos.official_email_id : "",
                    primary_contact_number = personal_dtl != null ? personal_dtl.primary_contact_number : "",
                    //username=,
                    is_hod = _clsCurrentUser.Is_Hod,
                    profile_img = string.Format("{0}{1}", domain_url, teos != null ? (teos.employee_photo_path != null ? teos.employee_photo_path : "") : ""),
                    total_employee_in_company,
                    present_employee,
                    absent_employee,
                    StatusCode = "1",
                    StatusMessage = "Success",
                    emp_name_code = string.Format("{0} ({1})", p.emp_name, p.emp_code),
                    dob_ = teos != null ? teos.date_of_birth.ToString() : "",
                    dobj_ = teos != null ? teos.date_of_joining.ToString() : "",
                    dept_name = teos != null ? teos.department_name : "",
                    loc_name = teos != null ? teos.location_name : "",
                    grade_name = grade_ != null ? grade_ : "",
                    current_emp_status = Enum.GetName(typeof(EmployeeType), p.emp_status),
                    required_notice_period_ = p.emp_status == 2 ? Convert.ToInt32(_config["NoticeSetting:Probation"]) : p.emp_status == 3 ? Convert.ToInt32(_config["NoticeSetting:Confirmed"]) : 0,
                    p.emp_status,
                    emp_sep_Detail = _context.tbl_emp_separation.Where(x => x.is_deleted == 0 && x.company_id == company_id && x.emp_id == emp_id).DefaultIfEmpty().FirstOrDefault(),
                }).ToList();

                #region old
                //var User_Profile_details = _context.tbl_employee_master.OrderByDescending(y => y.employee_id).Where(x => x.is_active == 1 && x.employee_id == emp_id && x.tbl_user_master.FirstOrDefault(z => z.is_active == 1).default_company_id == company_id).Select(p => new
                //{
                //    employee_first_name = p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).employee_first_name,
                //    employee_middle_name = p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).employee_middle_name,
                //    employee_last_name = p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).employee_last_name,
                //    emp_code = p.emp_code,
                //    company_name = p.tbl_employee_company_map.OrderByDescending(q => q.sno).FirstOrDefault(q => q.is_deleted == 0).tbl_company_master.company_name,
                //    company_code = p.tbl_employee_company_map.OrderByDescending(q => q.sno).FirstOrDefault(q => q.is_deleted == 0).tbl_company_master.company_code,
                //    permanent_address_line_one = p.tbl_emp_personal_sec.OrderByDescending(q => q.emp_personal_section_id).FirstOrDefault(q => q.is_deleted == 0).permanent_address_line_one,
                //    permanent_address_line_two = p.tbl_emp_personal_sec.OrderByDescending(q => q.emp_personal_section_id).FirstOrDefault(q => q.is_deleted == 0).permanent_address_line_two,
                //    country_name = _context.tbl_country.FirstOrDefault(g => g.is_deleted == 0 && g.country_id == p.tbl_emp_personal_sec.OrderByDescending(q => q.emp_personal_section_id).FirstOrDefault(q => q.is_deleted == 0).permanent_country).name,
                //    state_name = _context.tbl_state.FirstOrDefault(g => g.is_deleted == 0 && g.state_id == p.tbl_emp_personal_sec.OrderByDescending(q => q.emp_personal_section_id).FirstOrDefault(q => q.is_deleted == 0).permanent_state).name,
                //    city_name = _context.tbl_city.FirstOrDefault(g => g.is_deleted == 0 && g.city_id == p.tbl_emp_personal_sec.OrderByDescending(q => q.emp_personal_section_id).FirstOrDefault(q => q.is_deleted == 0).permanent_city).name,
                //    designation_name = p.tbl_emp_desi_allocation.OrderByDescending(q => q.emp_grade_id).FirstOrDefault(q => q.applicable_from_date.Date <= DateTime.Now.Date && DateTime.Now.Date <= q.applicable_to_date.Date && q.desig_id != null).tbl_designation_master.designation_name,
                //    official_email_id = p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).official_email_id,
                //    primary_contact_number = p.tbl_emp_personal_sec.OrderByDescending(q => q.emp_personal_section_id).FirstOrDefault(q => q.is_deleted == 0).primary_contact_number,
                //    username = p.tbl_user_master.FirstOrDefault(q => q.is_active == 1).username,
                //    is_hod = is_hod,
                //    hod_code_status = " 0 Superadmin, 1 HOD, 2 Normal User",
                //    profile_img = string.Format("{0}{1}", domain_url, p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(h => h.is_deleted == 0 && !string.IsNullOrEmpty(h.employee_first_name)).employee_photo_path),
                //    total_employee_in_company,
                //    present_employee,
                //    absent_employee,
                //    StatusCode = "1",
                //    StatusMessage = "Success",
                //    emp_name_code = string.Format("{0} {1} {2} ({3})",
                //            p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                //            p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                //            p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0).employee_last_name, p.emp_code),
                //    dob_ = p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0).date_of_birth,
                //    dobj_ = p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0).date_of_joining,
                //    dept_name = p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0).tbl_department_master.department_name,
                //    loc_name = p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0).tbl_location_master.location_name,

                //    grade_name = p.tbl_emp_grade_allocation.OrderByDescending(q => q.emp_grade_id).FirstOrDefault(q => q.applicable_from_date.Date <= DateTime.Now.Date && DateTime.Now.Date <= q.applicable_to_date.Date && q.grade_id != null).tbl_grade_master.grade_name,

                //    current_emp_status = p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0).current_employee_type == 1 ? "Temporary" :
                //            p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0).current_employee_type == 2 ? "Probation" :
                //            p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0).current_employee_type == 3 ? "Confirmed" :
                //            p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).current_employee_type == 4 ? "Contract" :
                //            p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).current_employee_type == 10 ? "Notice" :
                //            p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).current_employee_type == 99 ? "FNF" :
                //            p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).current_employee_type == 100 ? "Separate" : "-",

                //    required_notice_period_ = p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).current_employee_type == 2 ? Convert.ToInt32(_config["NoticeSetting:Probation"]) :
                //            p.tbl_emp_officaial_sec.OrderByDescending(q => q.emp_official_section_id).FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).current_employee_type == 3 ? Convert.ToInt32(_config["NoticeSetting:Confirmed"]) : 0,




                //}).ToList();
                #endregion

                if (User_Profile_details.Count() == 0)
                {
                    return Ok(new { StatusCode = "0", StatusMessage = "Record not found...!" });
                }

                return Ok(User_Profile_details);
            }
            catch (Exception ex)
            {
                objResult.Message = ex.Message.ToString();
                return Ok(objResult);
            }

        }


        //get grade master data
        //[Authorize()]
        [Route("GetAllEmployee/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "7090")]
        public IActionResult GetAllEmployee([FromRoute] int company_id)
        {
            try
            {

                ResponseMsg objResult = new ResponseMsg();

                var result = from em in _context.tbl_employee_master
                             join e_of in _context.tbl_emp_officaial_sec on em.employee_id equals e_of.employee_id
                             join um in _context.tbl_user_master on em.employee_id equals um.employee_id
                             join cm in _context.tbl_company_master on um.default_company_id equals cm.company_id
                             where e_of.is_deleted == 0 && um.default_company_id == company_id
                             select new
                             {
                                 employee_id = em.employee_id,
                                 employee_code = em.emp_code,
                                 employee_first_name = e_of.employee_first_name,
                                 employee_middle_name = e_of.employee_middle_name,
                                 employee_last_name = e_of.employee_last_name,
                                 card_number = e_of.card_number,
                                 date_of_joining = e_of.date_of_joining,
                                 date_of_birth = e_of.date_of_birth,
                                 official_email_id = e_of.official_email_id,
                                 username = um.username,
                                 password = um.password,
                                 company_name = cm.company_name,
                                 created_date = em.created_date
                             };


                if (result == null)
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
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //Get All Employee Under Hod      
        [Route("GetAllEmployeeUnderHod/{hod_id}")]
        [HttpGet]
        //[Authorize(Policy = "7091")]
        public IActionResult GetAllEmployeeUnderHod([FromRoute] int hod_id)
        {
            try
            {

                ResponseMsg objResult = new ResponseMsg();


                var result1 = (from em in _context.tbl_employee_master
                               join e_of in _context.tbl_emp_officaial_sec on em.employee_id equals e_of.employee_id
                               join emp_mgr in _context.tbl_emp_manager on em.employee_id equals emp_mgr.employee_id
                               where emp_mgr.m_one_id == hod_id || emp_mgr.m_two_id == hod_id || emp_mgr.m_three_id == hod_id && e_of.is_deleted == 0 || e_of.employee_id == hod_id
                               select new
                               {
                                   employee_id = em.employee_id,
                                   employee_code = em.emp_code,
                                   employee_first_name = e_of.employee_first_name,
                                   employee_middle_name = e_of.employee_middle_name,
                                   employee_last_name = e_of.employee_last_name,
                                   card_number = e_of.card_number,
                                   date_of_joining = e_of.date_of_joining,
                                   date_of_birth = e_of.date_of_birth,
                                   official_email_id = e_of.official_email_id,
                                   created_date = em.created_date,
                                   emp_id = e_of.employee_id,
                                   is_deleted = e_of.is_deleted,
                                   StatusCode = 1
                               }).ToList();

                var result = result1.Where(a => a.is_deleted == 0).GroupBy(a => new { a.employee_code, a.employee_id, a.employee_first_name, a.employee_middle_name, a.employee_last_name }).Select(a => a.Key).ToList();

                if (result.Count() == 0)
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
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [HttpGet("EmpPerformanceReportMonthly/{emp_id}/{date}")]
        //[Authorize(Policy = "7092")]
        public async Task<IActionResult> EmpPerformanceReportMonthly([FromRoute] int emp_id, DateTime date)
        {
            int[] day_status = new int[] { 1, 2 };

            var data = _context.tbl_daily_attendance.Select(a => new { status = a.day_status == 1 ? "P" : a.day_status == 2 ? "A" : "", a.day_status, a.attendance_dt, a.emp_id }).
                Where(a => a.attendance_dt.Month == date.Month && a.emp_id == emp_id).ToList();

            if (data.Count == 0)
            {
                return Ok(new { Status = 0, Status_Msg = "No record found...!" });
            }
            return Ok(data.Where(a => day_status.Contains(a.day_status)));
        }


        [HttpGet("EmpPerformanceReportDateRange/{emp_id}/{from_date}/{to_date}")]
        //[Authorize(Policy = "7093")]
        public async Task<IActionResult> EmpPerformanceReportDateRange([FromRoute] int emp_id, DateTime from_date, DateTime to_date)
        {
            int[] day_status = new int[] { 1, 2 };

            var data = _context.tbl_daily_attendance.Select(a => new { status = a.day_status == 1 ? "P" : a.day_status == 2 ? "A" : "", a.day_status, a.attendance_dt, a.emp_id }).
                Where(a => a.emp_id == emp_id && a.attendance_dt >= from_date && a.attendance_dt <= to_date).ToList();

            if (data.Count == 0)
            {
                return Ok(new { Status = 0, Status_Msg = "No record found...!" });
            }
            return Ok(data.Where(a => day_status.Contains(a.day_status)));
        }

        #endregion **************************************************************************************************************************************


        #region ** Start Created by Supriya on 24-05-2019

        [Route("Get_MenuMaster/{menu_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Setting))]
        public IActionResult Get_MenuMaster([FromRoute]int menu_id)
        {
            Response_Msg objresult = new Response_Msg();
            try
            {
                if (menu_id > 0)
                {
                    var data = _context.tbl_menu_master.Where(x => (int)x.menu_id == menu_id).FirstOrDefault();
                    return Ok(data);
                }
                else
                {
                    var data = (from a in _context.tbl_menu_master
                                join b in _context.tbl_menu_master on a.parent_menu_id equals (int)b.menu_id into ass
                                from c in ass.DefaultIfEmpty()
                                where a.is_active == 1 && a.type == 1
                                select new
                                {
                                    menu_id = a.menu_id,
                                    menu_name = a.menu_name,
                                    parent_menu_id = a.parent_menu_id,
                                    iconUrl = a.IconUrl,
                                    urll = a.urll,
                                    sortingOrder = a.SortingOrder,
                                    created_date = a.created_date,
                                    modified_date = a.modified_date,
                                    parent_menu_name = c.menu_name
                                    //type = c.type

                                }).AsEnumerable().Select((a, index) => new
                                {
                                    menu_id = a.menu_id,
                                    menu_name = a.menu_name,
                                    parent_menu_id = a.parent_menu_id,
                                    iconUrl = a.iconUrl,
                                    urll = a.urll,
                                    sortingOrder = a.sortingOrder,
                                    created_date = a.created_date,
                                    modified_date = a.modified_date,
                                    parent_menu_name = a.parent_menu_name,
                                    //type = a.type,
                                    s_no = index + 1
                                }).ToList();
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }

        [Route("Save_MenuMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Setting))]
        public IActionResult Save_MenuMaster([FromBody] tbl_menu_master obj_tbl_menu)
        {
            Response_Msg obj_result = new Response_Msg();

            if (!_clsCurrentUser.DownlineEmpId.Contains(obj_tbl_menu.created_by))
            {
                obj_result.StatusCode = 1;
                obj_result.Message = "Unauthorize Access...!!";
                return Ok(obj_result);
            }

            try
            {


                //var exist = _context.tbl_menu_master.Where(a => a.menu_name.Contains(obj_tbl_menu.menu_name.Trim())).ToList();
                var exist = _context.tbl_menu_master.Where(a => a.menu_name.Trim().ToUpper() == obj_tbl_menu.menu_name.Trim().ToUpper()).ToList();
                if (exist.Count > 0)
                {
                    obj_result.StatusCode = 1;
                    obj_result.Message = "Menus already exist, Please try with another one...";
                }
                else
                {
                    obj_tbl_menu.type = 1;
                    obj_tbl_menu.is_active = 1;
                    //obj_tbl_menu.created_by = 1;
                    obj_tbl_menu.created_date = DateTime.Now;

                    _context.Entry(obj_tbl_menu).State = EntityState.Added;
                    _context.SaveChanges();
                    obj_result.StatusCode = 0;
                    obj_result.Message = "Menu Successfully Saved..";
                }
                return Ok(obj_result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }


        }

        [Route("Edit_MenuMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Setting))]
        public IActionResult Edit_MenuMaster([FromBody] tbl_menu_master obj_tbl_menumaster)
        {
            Response_Msg objresponse = new Response_Msg();

            if (!_clsCurrentUser.DownlineEmpId.Contains(obj_tbl_menumaster.modified_by))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access....!!";
                return Ok(objresponse);
            }
            try
            {
                var exist_data = _context.tbl_menu_master.Where(a => a.menu_id == obj_tbl_menumaster.menu_id).FirstOrDefault();
                if (exist_data != null)
                {
                    var duplicate = _context.tbl_menu_master.Where(a => a.menu_id != obj_tbl_menumaster.menu_id && a.menu_name.Trim().ToUpper() == obj_tbl_menumaster.menu_name.Trim().ToUpper()).ToList();
                    if (duplicate.Count > 0)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Menu Already Exist";
                    }
                    else
                    {
                        exist_data.menu_name = obj_tbl_menumaster.menu_name.Trim();
                        exist_data.parent_menu_id = obj_tbl_menumaster.parent_menu_id;
                        exist_data.IconUrl = obj_tbl_menumaster.IconUrl;
                        exist_data.urll = obj_tbl_menumaster.urll;
                        exist_data.SortingOrder = obj_tbl_menumaster.SortingOrder;
                        // exist_data.modified_by = 1;
                        exist_data.modified_date = DateTime.Now;

                        _context.Entry(exist_data).State = EntityState.Modified;
                        _context.SaveChanges();

                        objresponse.StatusCode = 0;
                        objresponse.Message = "Menu Master Successfully Updated";
                    }


                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Menu ID not exist/ Invalid Detail Please try again";
                }
                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }

        #endregion **End by Supriya on 25-05-2019


        #region Assign Menu Start by Supriya on 25-05-2019

        //[Route("Save_Edit_AssignRoleMenu")]
        //[HttpPost]
        ////[Authorize(Policy = "7097")]
        //public IActionResult Save_Edit_AssignRoleMenu([FromBody] tbl_role_menu_master objrole_menu)
        //{
        //    //Response_Msg objresponse = new Response_Msg();
        //    //try
        //    //{
        //    //    string _main_menu_id = string.Empty;

        //    //   _main_menu_id = objrole_menu.role_id == 1 ? "150,151" : objrole_menu.role_id == 2 ? "150" : "152";

        //    //    var exist_data = _context.tbl_role_menu_master.Where(a => a.role_id == objrole_menu.role_id).FirstOrDefault();
        //    //    if (exist_data != null)
        //    //    {
        //    //        if (objrole_menu.menu_id != "" || objrole_menu.menu_id != null)
        //    //        {
        //    //            _main_menu_id = _main_menu_id + "," + objrole_menu.menu_id;
        //    //        }

        //    //        exist_data.menu_id = _main_menu_id;
        //    //        exist_data.menu_id = objrole_menu.menu_id;
        //    //        exist_data.modified_by = objrole_menu.created_by;
        //    //        exist_data.modified_date = DateTime.Now;

        //    //        _context.Entry(exist_data).State = EntityState.Modified;
        //    //        _context.SaveChanges();
        //    //        objresponse.Message = "Menus Successfully upadated";
        //    //    }
        //    //    else
        //    //    {
        //    //        if (objrole_menu.menu_id != "" || objrole_menu.menu_id != null)
        //    //        {
        //    //            _main_menu_id = _main_menu_id + "," + objrole_menu.menu_id;
        //    //        }

        //    //        objrole_menu.menu_id = _main_menu_id;

        //    //        objrole_menu.created_date = DateTime.Now;

        //    //        _context.Entry(objrole_menu).State = EntityState.Added;
        //    //        _context.SaveChanges();
        //    //        objresponse.Message = "Menus Successfully Assigned";

        //    //    }
        //    //    objresponse.StatusCode = 0;
        //    //    return Ok(objresponse);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return Ok(ex.Message.ToString());
        //    //}
        //    return Ok("");
        //}

        //[Route("Get_AssignRoleMenu/{role_id}")]
        //[HttpGet]
        ////[Authorize(Policy = "7098")]
        //public IActionResult Get_AssignRoleMenu([FromRoute] int role_id)
        //{
        //    Response_Msg objresponse = new Response_Msg();
        //    try
        //    {
        //        if (role_id > 0)
        //        {
        //            List<object> _tbl_menu = new List<object>();
        //            var _menu_master = (from a in _context.tbl_menu_master
        //                               join b in _context.tbl_menu_master on a.parent_menu_id equals (int)b.menu_id into ass
        //                               from c in ass.DefaultIfEmpty()
        //                               where a.is_active == 1 && a.type==1 select new {
        //                                   a.menu_id,
        //                                   a.urll,
        //                                   a.menu_name,
        //                                   a.parent_menu_id,
        //                                   parent_menu_name=c.menu_name

        //                               }).ToList();

        //            //var data = _context.tbl_role_menu_master.Where(x => x.role_id == role_id).FirstOrDefault();
        //            //if (data != null && !string.IsNullOrEmpty(data.menu_id))
        //            //{
        //            //    List<int> _id = data.menu_id.Split(',').ToList().ConvertAll(int.Parse);

        //            //    _menu_master.ForEach(p =>
        //            //        _tbl_menu.Add(new {
        //            //            menu_id = p.menu_id,
        //            //            menu_name = p.menu_name,
        //            //            urll = p.urll,
        //            //            is_active = _id.Contains((int)p.menu_id) ? 1 : 0,
        //            //            parent_menu_name = p.parent_menu_name
        //            //        })
        //            //    );


        //            //}
        //            //else
        //            //{
        //            //    _menu_master.ForEach(p =>
        //            //        _tbl_menu.Add(new
        //            //        {
        //            //            menu_id = p.menu_id,
        //            //            menu_name = p.menu_name,
        //            //            urll = p.urll,
        //            //            is_active = 0,
        //            //            parent_menu_name = p.parent_menu_name
        //            //        })
        //            //    );

        //            //}

        //            return Ok(_tbl_menu);
        //        }
        //        else
        //        {
        //            objresponse.StatusCode = 0;
        //            objresponse.Message = "Invalid Role ID";
        //            return Ok(objresponse);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message.ToString());
        //    }
        //}
        #endregion Assign Menu End by Supriya on 25-05-2019



        #region STARTED BY SUPRIYA ON 17-06-2019
        [Route("GetAll_PendingRequetsCount/{employeeid}/{company_id}/{emp_role_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.ApplicationReports))]
        public IActionResult GetAll_PendingRequetsCount([FromRoute] int employeeid, int company_id, int emp_role_id)
        {
            try
            {
                List<int> _loginid = new List<int>();
                _loginid.Add(_clsCurrentUser.EmpId);

                var manager_emp_list = _clsCurrentUser.DownlineEmpId.Except(_loginid).ToList();

                var unfreezed_attendence = _context.tbl_daily_attendance.Where(x => x.is_freezed == 0 && manager_emp_list.Contains(x.emp_id ?? 0)).Select(p => new { is_freezed = p.is_freezed, attendence_dt = p.attendance_dt, emp_id = p.emp_id }).ToList();

                //var manager_emp_list = _context.tbl_emp_manager.OrderByDescending(b => b.emp_mgr_id).Where(a => (a.m_one_id == employeeid || a.m_two_id == employeeid || a.m_three_id == employeeid) && a.is_deleted == 0).Select(p =>
                //         p.employee_id
                //).Distinct().ToList();

                var outdoor_reqst = _context.tbl_outdoor_request.Where(x => x.is_deleted == 0 && x.is_final_approve == 0 && manager_emp_list.Contains(x.r_e_id ?? 0)).Select(a => new { a.r_e_id, a.leave_request_id }).ToList();


                var outdoor_pending = outdoor_reqst.Count();

                var leave_reqst = _context.tbl_leave_request.Where(b => b.is_deleted == 0 && b.is_final_approve == 0 && manager_emp_list.Contains(b.r_e_id ?? 0)).Select(c => new { c.r_e_id, c.leave_request_id }).ToList();

                var leave_pending = leave_reqst.Count();

                var attandance_reqst = _context.tbl_attendace_request.Join(unfreezed_attendence, a => new { _date = a.from_date, emp_id = a.r_e_id }, b => new { _date = b.attendence_dt.Date, emp_id = b.emp_id }, (a, b) => new
                {
                    a.r_e_id,
                    a.leave_request_id,
                    a.is_final_approve,
                    a.is_deleted,
                    b.is_freezed
                }).Where(d => d.is_deleted == 0 && d.is_final_approve != 1 && d.is_final_approve != 2 && manager_emp_list.Contains(d.r_e_id ?? 0)).ToList();

                var attendance_pending = attandance_reqst.Count();

                var compoff_reqst = _context.tbl_comp_off_request_master.Where(f => f.is_deleted == 0 && f.is_final_approve == 0 && manager_emp_list.Contains(f.r_e_id ?? 0)).Select(g => new { g.r_e_id, g.comp_off_request_id }).ToList();

                var compoff_pending = compoff_reqst.Count();

                var loan_reqst = _context.tbl_loan_approval.Join(_context.tbl_loan_approval_setting, a => a.approval_order, b => b.order, (a, b) => new
                {

                    a.sno,
                    a.loan_req_id,
                    a.emp_id,
                    approver_emp_id = b.emp_id,
                    a.is_deleted,
                    a.tbl_loan_request.is_closed,
                    b.approver_type,
                    b.approver_role_id,
                    a.is_approve

                }).Where(x => x.approver_type == 1 && (x.approver_emp_id == employeeid || x.approver_role_id == emp_role_id) && x.is_approve == 0 && x.is_deleted == 0).OrderBy(y => y.sno).ToList();


                var total_loan_request = loan_reqst.Count();


                var first_final_manager = _context.tbl_emp_manager.OrderByDescending(b => b.emp_mgr_id).Where(a => a.m_one_id == employeeid && a.final_approval == 1 && a.is_deleted == 0).Select(p => p.employee_id).Distinct().ToList();

                var second_final_manager = _context.tbl_emp_manager.OrderByDescending(b => b.emp_mgr_id).Where(a => a.m_two_id == employeeid && a.final_approval == 2 && a.is_deleted == 0).Select(p => p.employee_id).Distinct().ToList();

                var third_final_mangaer = _context.tbl_emp_manager.OrderByDescending(b => b.emp_mgr_id).Where(a => a.m_three_id == employeeid && a.final_approval == 3 && a.is_deleted == 0).Select(p => p.employee_id).Distinct().ToList();

                List<int> termsList = new List<int>();
                for (int i = 0; i < first_final_manager.Count; i++)
                {
                    termsList.Add(Convert.ToInt32(first_final_manager[i]));
                }


                foreach (var item in second_final_manager)
                {
                    bool containitem = termsList.Contains(Convert.ToInt32(item));

                    if (!containitem)
                    {
                        termsList.Add(Convert.ToInt32(item));
                    }
                }


                var reimbursment_reqst = _context.tbl_rimb_req_mstr.Where(a => a.is_delete == 0 && termsList.Contains(Convert.ToInt32(a.emp_id)) && a.is_approvred == 0).Select(p => new
                {
                    p.emp_id,
                    p.rrm_id
                }).ToList();

                var total_reimbursment_reqst = reimbursment_reqst.Count();




                var dataa = _context.tbl_assets_approval.Join(_context.tbl_loan_approval_setting, a => a.approval_order, b => b.order, (a, b) => new
                {
                    a.asset_approval_sno,
                    a.asset_req_id,
                    a.approval_order,
                    b.order,
                    approver_emp_id = b.emp_id,
                    approver_role_id = b.approver_role_id,
                    a.asset_req_mastr.is_active,
                    a.asset_req_mastr.is_deleted,
                    a.is_approve,
                    a.emp_id,
                    a.asset_req_mastr.is_closed,
                    b.approver_type,
                    approves_active = b.is_active
                }).Where(x => x.approver_type == 2 && x.approves_active == 1 && (x.approver_role_id == emp_role_id || x.approver_emp_id == employeeid) && x.is_deleted == 0 && x.is_approve == 0 && x.is_closed == 0
                ).OrderBy(x => x.asset_approval_sno).ToList();


                List<AssetRequest> objrequestlist = new List<AssetRequest>();

                if (dataa.Count > 0)
                {
                    foreach (var item in dataa)
                    {
                        bool containsItem = objrequestlist.Any(x => x.asset_req_id == item.asset_req_id);

                        if (!containsItem)
                        {
                            AssetRequest objrequest = new AssetRequest();

                            objrequest.asset_req_id = Convert.ToInt32(item.asset_req_id);
                            objrequest.approval_order = item.approval_order;
                            objrequest.approver_emp_id = item.approver_emp_id;
                            objrequest.approver_role_id = item.approver_role_id;
                            objrequest.is_approve = item.is_approve;
                            objrequest.emp_id = item.emp_id;
                            objrequest.is_closed = item.is_closed;
                            objrequest.approver_type = item.approver_type;


                            objrequestlist.Add(objrequest);
                        }
                        else
                        {

                            if (objrequestlist.Where(a => a.asset_req_id == item.asset_req_id && a.is_approve == 1).Count() > 0)
                            {
                                objrequestlist.Remove(objrequestlist.Single(s => s.asset_req_id == item.asset_req_id));

                                AssetRequest objrequest = new AssetRequest();

                                objrequest.asset_req_id = Convert.ToInt32(item.asset_req_id);
                                objrequest.approval_order = item.approval_order;
                                objrequest.approver_emp_id = item.approver_emp_id;
                                objrequest.approver_role_id = item.approver_role_id;
                                objrequest.is_approve = item.is_approve;
                                objrequest.emp_id = item.emp_id;
                                objrequest.is_closed = item.is_closed;
                                objrequest.approver_type = item.approver_type;


                                objrequestlist.Add(objrequest);
                            }
                        }

                    }

                }

                var asset_request = objrequestlist.Count();

                var _finaldata = new { outdoor_pending, leave_pending, attendance_pending, compoff_pending, total_loan_request, total_reimbursment_reqst, asset_request };
                return Ok(_finaldata);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }
        #endregion ENDED BY SUPRIYA ON 18-06-2019


        #region STARTED BY SUPRIYA ON 18-06-2019
        [Route("Get_Employee_Under_LoginEmp/{employeeid}")]
        [HttpGet]
        //[Authorize(Policy = "7100")] //Only Manager 
        public IActionResult Get_Employee_Under_LoginEmp([FromRoute] int employeeid)
        {
            var manager_emp_list = _context.tbl_emp_manager.Where(a => (a.m_one_id == employeeid || a.m_two_id == employeeid || a.m_three_id == employeeid) && a.is_deleted == 0).Select(p => new
            {
                p.employee_id
            }).Distinct().OrderBy(s => s.employee_id).ToList();

            List<clsUserRoleModel> userlist = new List<clsUserRoleModel>();
            foreach (var item in manager_emp_list)
            {
                clsUserRoleModel objuser = new clsUserRoleModel();

                objuser.empid = Convert.ToInt32(item.employee_id);

                var get_code = _context.tbl_employee_master.Where(a => a.employee_id == item.employee_id).FirstOrDefault();

                objuser.empcode = get_code != null ? get_code.emp_code.ToString() : "-";

                var get_name = _context.tbl_emp_officaial_sec.Where(b => b.employee_id == item.employee_id && !string.IsNullOrEmpty(b.employee_first_name)).FirstOrDefault();

                objuser.empname = get_name != null ? (get_name.employee_first_name + " " + get_name.employee_middle_name + " " + get_name.employee_last_name) : "-";

                userlist.Add(objuser);
            }

            return Ok(userlist);
        }

        [Route("Get_Logs_Detail/{employeeid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_Logs_Detail([FromRoute]int employeeid)
        {
            try
            {

                var daily_attandance_last_processdt = _context.tbl_daily_attendance.OrderByDescending(x => x.last_process_dt).Select(p => new
                {
                    p.last_process_dt
                }).FirstOrDefault(); // Data Process

                var last_entytime_attandancedtl = _context.tbl_attendance_details.OrderByDescending(x => x.attendance_id).Select(r => new
                {
                    r.entry_time,
                    r.attendance_id
                }).FirstOrDefault(); // Import Data

                var emp_last_modified_record = _context.tbl_emp_officaial_sec.OrderByDescending(y => y.emp_official_section_id).Where(x => x.is_deleted == 0).Select(q => new
                {
                    q.emp_official_section_id,
                    q.created_date
                    // q.last_modified_date
                }).FirstOrDefault();// Employee Modify

                var final_data = new { daily_attandance_last_processdt, last_entytime_attandancedtl, emp_last_modified_record };

                return Ok(final_data);
            }
            catch
            {
                return Ok();
            }
        }
        #endregion END BY SUPRIYA ON 18-06-2019

        #region ** STARTED BY SUPRIYA ON 24-06-2019 **

        [Route("GetRelation")]
        [HttpGet]
        //[Authorize(Policy = "7102")]
        public IActionResult GetRelation()
        {
            try
            {
                List<object> list = new List<object>();
                foreach (RelationType relation in Enum.GetValues(typeof(RelationType)))
                {
                    //string value = (string)Enum.Parse(typeof(RelationType), Enum.GetName(typeof(RelationType), relation));

                    Type type = relation.GetType();
                    MemberInfo[] memInfo = type.GetMember(relation.ToString());

                    if (memInfo != null && memInfo.Length > 0)
                    {
                        object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                        if (attrs != null && attrs.Length > 0)
                        {
                            string strvalue = ((DescriptionAttribute)attrs[0]).Description;

                            list.Add(new
                            {

                                relationid = strvalue,
                                relationname = strvalue
                            });
                        }

                    }
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        #endregion ** ENDED BY SUPRIYA ON 24-06-2019 **


        #region CREATED BY AMARJEET - 19-07-2019 - EVENT 

        [Route("Save_Event")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Event))]
        public async Task<IActionResult> Save_Event(tbl_event_master tbl_event_master)
        {
            ResponseMsg objResult = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(tbl_event_master.company_id ?? 0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company access...!!";
                return Ok(objResult);
            }

            try
            {
                var IfExist = _context.tbl_event_master.Where(a => a.is_active == 1 && a.event_name.Trim().ToUpper() == tbl_event_master.event_name.Trim().ToUpper() && a.event_date == tbl_event_master.event_date && a.company_id == tbl_event_master.company_id).FirstOrDefault();

                if (IfExist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Event already exist...!";
                    return Ok(objResult);
                }
                else
                {

                    tbl_event_master.created_dt = DateTime.Now;
                    tbl_event_master.modified_by = tbl_event_master.created_by;
                    tbl_event_master.modified_dt = DateTime.Now;


                    _context.tbl_event_master.Attach(tbl_event_master);
                    _context.Entry(tbl_event_master).State = EntityState.Added;

                    await _context.SaveChangesAsync();


                    objResult.StatusCode = 0;
                    objResult.Message = "Event submitted successfully...!";

                }
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.Message = ex.ToString();
                return Ok(objResult);
            }
        }


        [Route("Get_Event_by_id/{event_id}/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = nameof(enmMenuMaster.Event))]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_Event_by_id([FromRoute] int event_id, int company_id)
        {
            ResponseMsg objresponse = new ResponseMsg();

            try
            {
                if (event_id > 0)
                {
                    var data = _context.tbl_event_master.Where(x => x.event_id == event_id && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).ToList();
                    return Ok(data);
                }
                else if (event_id == -1)
                {
                    var data = _context.tbl_event_master.Where(a => a.is_active == 1 && a.company_id == company_id).ToList();
                    return Ok(data);
                }
                else
                {
                    var data = _context.tbl_event_master.Where(a => a.company_id == company_id && _clsCurrentUser.CompanyId.Contains(a.company_id ?? 0)).Select(p => new
                    {
                        p.event_id,
                        p.event_name,
                        p.event_date,
                        p.event_time,
                        p.event_place,
                        p.is_active,
                        p.created_dt
                    }).ToList();
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [HttpPost("Update_Event")]
        [Authorize(Policy = nameof(enmMenuMaster.Event))]
        public async Task<IActionResult> Update_Event([FromBody] tbl_event_master tbl_event_master)
        {
            Response_Msg objresponse = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(tbl_event_master.company_id ?? 0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access....!!";
                return Ok(objresponse);
            }
            try
            {
                var exist_data = _context.tbl_event_master.Where(a => a.event_id == tbl_event_master.event_id).FirstOrDefault();
                if (exist_data != null)
                {
                    var duplicate_event = _context.tbl_event_master.Where(a => a.event_id != tbl_event_master.event_id && a.company_id == tbl_event_master.company_id && a.event_name.Trim().ToUpper() == tbl_event_master.event_name.Trim().ToUpper() && a.event_date == tbl_event_master.event_date).FirstOrDefault();
                    if (duplicate_event != null)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Event Already Exist...!";
                    }
                    else
                    {
                        exist_data.event_id = tbl_event_master.event_id;
                        exist_data.event_name = tbl_event_master.event_name.Trim();
                        exist_data.event_place = tbl_event_master.event_place;
                        exist_data.event_date = tbl_event_master.event_date;
                        exist_data.event_time = tbl_event_master.event_time;
                        exist_data.company_id = tbl_event_master.company_id;
                        exist_data.modified_by = _clsCurrentUser.EmpId;
                        exist_data.is_active = tbl_event_master.is_active;
                        exist_data.modified_dt = DateTime.Now;


                        _context.Entry(exist_data).State = EntityState.Modified;
                        _context.SaveChanges();

                        objresponse.StatusCode = 0;
                        objresponse.Message = "Successfully Updated...!";
                    }




                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Event Not exist / Invalid Detail Please try again";
                }
                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }



        #endregion

        #region CREATED BY AMARJEET - 22-07-2019  Right Menu Link

        [Route("Save_Right_Menu_Link")]
        [HttpPost]
        //[Authorize(Policy = "7106")]
        public async Task<IActionResult> Save_Right_Menu_Link(tbl_right_menu_link tbl_right_menu_link)
        {
            ResponseMsg objResult = new ResponseMsg();
            try
            {
                var IfExist = _context.tbl_right_menu_link.Where(a => a.is_active == 1 && a.menu_name == tbl_right_menu_link.menu_name).FirstOrDefault();

                if (IfExist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Menu already exist...!";
                    return Ok(objResult);
                }
                else
                {

                    tbl_right_menu_link.created_dt = DateTime.Now;
                    tbl_right_menu_link.modified_by = tbl_right_menu_link.created_by;
                    tbl_right_menu_link.modified_dt = DateTime.Now;


                    _context.tbl_right_menu_link.Attach(tbl_right_menu_link);
                    _context.Entry(tbl_right_menu_link).State = EntityState.Added;

                    await _context.SaveChangesAsync();


                    objResult.StatusCode = 0;
                    objResult.Message = "Submitted successfully...!";

                }
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.Message = ex.ToString();
                return Ok(objResult);
            }
        }


        [Route("Get_Right_Menu_Link/{menu_id}/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "7107")]
        public IActionResult Get_Right_Menu_Link([FromRoute] int menu_id, int company_id)
        {
            try
            {
                if (menu_id > 0)
                {
                    var data = _context.tbl_right_menu_link.Where(x => x.menu_id == menu_id).ToList();
                    return Ok(data);
                }
                else if (menu_id == -1)
                {
                    var data = _context.tbl_right_menu_link.Where(a => a.is_active == 1 && a.company_id == company_id).ToList();
                    return Ok(data);
                }
                else
                {
                    var data = _context.tbl_right_menu_link.Where(a => a.company_id == company_id).ToList();
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [HttpPost("Update_Right_Menu_Link")]
        //[Authorize(Policy = "7108")]
        public async Task<IActionResult> Update_Right_Menu_Link([FromBody] tbl_right_menu_link tbl_right_menu_link)
        {
            Response_Msg objresponse = new Response_Msg();
            try
            {
                var exist_data = _context.tbl_right_menu_link.Where(a => a.menu_id == tbl_right_menu_link.menu_id).FirstOrDefault();
                if (exist_data != null)
                {
                    exist_data.menu_id = tbl_right_menu_link.menu_id;
                    exist_data.menu_name = tbl_right_menu_link.menu_name;
                    exist_data.url = tbl_right_menu_link.url;
                    exist_data.sorting_order = tbl_right_menu_link.sorting_order;
                    exist_data.icon_url = tbl_right_menu_link.icon_url;
                    exist_data.company_id = tbl_right_menu_link.company_id;
                    exist_data.modified_by = tbl_right_menu_link.modified_by;
                    exist_data.is_active = tbl_right_menu_link.is_active;
                    exist_data.modified_dt = DateTime.Now;


                    _context.Entry(exist_data).State = EntityState.Modified;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Successfully Updated...!";
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Menu Not exist / Invalid Detail Please try again";
                }
                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }


        #endregion



        #region ** STARTED BY SUPRIYA ON 18-07-2019 **

        [Route("GetApproverType")]
        [HttpGet]
        //[Authorize(Policy = "7109")]
        public IActionResult GetApproverType()
        {
            try
            {
                List<object> list = new List<object>();
                foreach (ApproverType aprrovertypee in Enum.GetValues(typeof(ApproverType)))
                {
                    int value = (int)Enum.Parse(typeof(ApproverType), Enum.GetName(typeof(ApproverType), aprrovertypee));

                    Type type = aprrovertypee.GetType();
                    MemberInfo[] memInfo = type.GetMember(aprrovertypee.ToString());

                    if (memInfo != null && memInfo.Length > 0)
                    {
                        object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                        if (attrs != null && attrs.Length > 0)
                        {
                            string strvalue = ((DescriptionAttribute)attrs[0]).Description;

                            list.Add(new
                            {

                                approvertype_id = value,
                                approvertype_name = strvalue
                            });
                        }

                    }
                }

                return Ok(list);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        #endregion ** END BY SUPRIYA ON 18-07-2019 **


        #region ** STARTED BY SUPRIYA ON 24-07-2019**

        [Route("GetOnlyUserRole")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Employee))]
        public IActionResult GetOnlyUserRole()
        {
            try
            {
                var data = _context.tbl_role_master.Where(a => a.is_active == 1).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("GetRoleUser")]
        [HttpGet]
        //[Authorize(Policy = "7111")]
        public IActionResult GetRoleUser()
        {
            try
            {
                var data = _context.tbl_role_master.Where(a => a.role_id == 6 && a.is_active == 1).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #endregion ** END BY SUPRIYA ON 24-07-2019

        #region ** STARTED BY SUPRIYA ON 25-07-2019 **

        [Route("GetPieChart/{employee_id}")]
        [HttpGet]
        //[Authorize(Policy = "7112")]
        public IActionResult GetPieChart([FromRoute] int employee_id)
        {
            try
            {
                var manager_emp_list = _context.tbl_emp_manager.Where(a => (a.m_one_id == employee_id || a.m_two_id == employee_id || a.m_three_id == employee_id) && a.is_deleted == 0).Select(p =>
                      p.employee_id
               ).Distinct().ToList();

                var all_emp_data = _context.tbl_daily_attendance.Where(a => a.attendance_dt == DateTime.Now.Date && manager_emp_list.Contains(a.emp_id)).ToList();

                var present_data = _context.tbl_daily_attendance.Where(a => a.attendance_dt == DateTime.Now.Date && manager_emp_list.Contains(a.emp_id) && a.day_status == 1).ToList();

                var total_present = present_data.Count();

                var absent_data = _context.tbl_daily_attendance.Where(a => a.attendance_dt == DateTime.Now.Date && manager_emp_list.Contains(a.emp_id) && a.day_status == 2).ToList();

                var total_absent = absent_data.Count();

                var leave_data = _context.tbl_daily_attendance.Where(a => a.attendance_dt == DateTime.Now.Date && manager_emp_list.Contains(a.emp_id) && a.day_status == 3).ToList();

                var total_leave = leave_data.Count();

                var final_result = new { total_present = total_present, total_absent = total_absent, total_leave = total_leave };



                return Ok(final_result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [Route("GetPieChartByCompany/{company_id}/{EmpId}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult GetPieChartByCompany([FromRoute] int company_id, int EmpId)
        {
            try
            {

                ////var temp = _context.tbl_user_master.Where(a => a.default_company_id == company_id && _clsCurrentUser.DownlineEmpId.Contains(a.employee_id??0) /*a.employee_id==_clsCurrentUser.EmpId*/).Select(p => new
                ////{
                ////    p.default_company_id,
                ////    p.employee_id,
                ////    p.username,
                ////    p.password,
                ////   p.user_id
                ////}).Distinct().FirstOrDefault();

                ////mdlLoginOutput mdl_output = new mdlLoginOutput();
                ////List<int> _empids = new List<int>();
                ////if (temp != null)
                ////{
                ////    clsUsersDetails objuser_dtl = new clsUsersDetails(_context, _config, temp.username.Trim().ToUpper(), temp.password.Trim(), 0);

                ////    mdl_output.user_id = (temp.user_id == _clsCurrentUser.UserId) ? _clsCurrentUser.UserId : 0;
                ////    mdl_output.user_name = temp.password;
                ////    mdl_output.emp_id = _clsCurrentUser.EmpId;
                ////    objuser_dtl.LoadEmpSpecificDetail(mdl_output);
                ////    mdl_output.emp_company_lst = objuser_dtl.Get_emp_company_lst(mdl_output.emp_id, mdl_output);
                ////    List<EmployeeList> emplist = objuser_dtl.Get_Emp_dtl_under_login_emp(mdl_output, _clsEmployeeDetail).ToList();

                ////    _empids = emplist.Select(p => p._empid).ToList();

                ////}

                var all_emp_data = _context.tbl_daily_attendance.Where(a => a.attendance_dt == DateTime.Now.Date.AddDays(-1) && _clsCurrentUser.DownlineEmpId.Contains(a.emp_id ?? 0) /*_empids.Contains(a.emp_id??0)*/).ToList();

                var present_data = all_emp_data.Where(a => a.day_status == 1).ToList();
                // var present_data = _context.tbl_daily_attendance.Where(a => a.attendance_dt == DateTime.Now.Date.AddDays(-1) && _empids.Contains(a.emp_id ?? 0) && a.day_status == 1).ToList();

                var total_present = present_data.Count();
                var absent_data = all_emp_data.Where(a => a.day_status == 2).ToList();
                // var absent_data = _context.tbl_daily_attendance.Where(a => a.attendance_dt == DateTime.Now.Date.AddDays(-1) && temp.Any(b => b.employee_id == a.emp_id) && a.day_status == 2).ToList();

                var total_absent = absent_data.Count();
                var leave_data = all_emp_data.Where(a => a.day_status == 3).ToList();
                // var leave_data = _context.tbl_daily_attendance.Where(a => a.attendance_dt == DateTime.Now.Date.AddDays(-1) && temp.Any(b => b.employee_id == a.emp_id) && a.day_status == 3).ToList();

                var total_leave = leave_data.Count();

                var final_result = new { total_present = total_present, total_absent = total_absent, total_leave = total_leave };



                return Ok(final_result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        #endregion ** END BY SUPIRYA ON 25-07-2019 **


        #region CREATED BY AMARJEET ON 25-07-2019 Get Managers of employee


        [HttpGet("GetEmployeeManagers/{employee_id}/{companyid}")]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public async Task<IActionResult> GetEmployeeManagers([FromRoute] int employee_id, int companyid)
        {
            ResponseMsg objResult = new ResponseMsg();
            try
            {
                if (!_clsCurrentUser.DownlineEmpId.Any(p => p == employee_id))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access";
                    return Ok(objResult);
                }

                _clsEmployeeDetail._company_id = companyid;
                var data = _clsEmployeeDetail.Get_Emp_manager_dtl(employee_id);

                //var data = _context.tbl_emp_manager.Where(a => a.employee_id == employee_id && a.is_deleted == 0).OrderByDescending(a => a.emp_mgr_id).Select(a => new {

                //    master1 =string.Format("{0} {1} {2}", a.tbl_employee_master1.tbl_emp_officaial_sec.FirstOrDefault(b => b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).employee_first_name,
                //            a.tbl_employee_master1.tbl_emp_officaial_sec.FirstOrDefault(b => b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).employee_middle_name,
                //            a.tbl_employee_master1.tbl_emp_officaial_sec.FirstOrDefault(b => b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).employee_last_name),
                //    master2 =string.Format("{0} {1} {2}",a.tbl_employee_master2.tbl_emp_officaial_sec.FirstOrDefault(b=>b.is_deleted==0 && !string.IsNullOrEmpty(b.employee_first_name)).employee_first_name,
                //            a.tbl_employee_master2.tbl_emp_officaial_sec.FirstOrDefault(b => b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).employee_middle_name,
                //            a.tbl_employee_master2.tbl_emp_officaial_sec.FirstOrDefault(b => b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).employee_last_name),

                //    master3 = string.Format("{0} {1} {2}", a.tbl_employee_master3.tbl_emp_officaial_sec.FirstOrDefault(b => b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).employee_first_name,
                //            a.tbl_employee_master3.tbl_emp_officaial_sec.FirstOrDefault(b => b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).employee_middle_name,
                //            a.tbl_employee_master3.tbl_emp_officaial_sec.FirstOrDefault(b => b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).employee_last_name),


                //master1 = a.tbl_employee_master1.tbl_emp_officaial_sec.Where(b => b.employee_id == a.m_one_id && b.is_deleted == 0).Select(b => b.employee_first_name).FirstOrDefault(),
                //master2 = a.tbl_employee_master2.tbl_emp_officaial_sec.Where(b => b.employee_id == a.m_two_id && b.is_deleted == 0).Select(b => b.employee_first_name).FirstOrDefault(),
                //master3 = a.tbl_employee_master3.tbl_emp_officaial_sec.Where(b => b.employee_id == a.m_three_id && b.is_deleted == 0).Select(b => b.employee_first_name).FirstOrDefault(),

                // a.final_approval }).FirstOrDefault();

                return Ok(data);
            }
            catch (Exception ex)
            {
                objResult.Message = ex.ToString();
                return Ok(objResult);
            }
        }
        #endregion


        #region ** CREATED BY SUPRIYA ON 29-07-2019 DISPLAY EVENT NOTIFICATION


        [HttpGet("GetEventNotification/{company_id}")]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public async Task<IActionResult> GetEventNotification([FromRoute] int company_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access";
                return Ok(objresponse);
            }
            try
            {

                var data = _context.tbl_event_master.Where(a => a.company_id == company_id && a.event_date.Date >= DateTime.Now.Date && a.is_active == 1).Select(p => new
                {
                    p.event_id,
                    p.event_name,
                    p.company_id,
                    p.event_date,
                    p.event_time,
                    p.created_dt,
                    p.modified_dt,
                    p.event_place,
                    p.tbl_company_master.company_name,
                    event_date_time = string.Format("{0} {1}", p.event_date.ToShortDateString(), p.event_time.ToShortTimeString())
                }).ToList();
                return Ok(data);

            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }


        [HttpGet("Get_SubDepartmentMasterByCompID/{company_id}")]
        //[Authorize(Policy = "7116")]
        public IActionResult Get_SubDepartmentMasterByCompID([FromRoute] int company_id)
        {
            try
            {

                var data = (from a in _context.tbl_sub_department_master
                            join b in _context.tbl_department_master on a.department_id equals b.department_id
                            join c in _context.tbl_company_master on b.company_id equals c.company_id
                            where a.company_id == company_id
                            select new
                            {
                                a.department_id,
                                a.company_id,
                                a.sub_department_id,
                                a.sub_department_code,
                                a.sub_department_name,
                                a.created_by,
                                a.created_date,
                                b.department_name,
                                c.company_name,
                                a.is_active
                            }).AsEnumerable().Select((a, index) => new
                            {
                                subdeptname = a.sub_department_name,
                                subdeptcode = a.sub_department_code,
                                subdeptid = a.sub_department_id,
                                departmentname = a.department_name,
                                companyid = a.company_id,
                                companyname = a.company_name,
                                status = a.is_active,
                                createdby = a.created_by,
                                createdon = a.created_date,
                                sno = index + 1
                            }).ToList();

                return Ok(data);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        #endregion  ** END BY SUPRIYA ON 29-07-2019 

        #region ** START BY SUPRIYA ON 31-07-2019

        [HttpGet("GetEmployeeUserListBycompID/{company_id}/{roleid}")]
        [Authorize(Policy = nameof(enmMenuMaster.RoleAllocation))]
        public IActionResult GetEmployeeUserListBycompID([FromRoute] int company_id, int roleid)
        {
            try
            {

                var AllUsers = _context.tbl_user_master.Where(a => a.is_active == 1 && a.default_company_id == company_id && _clsCurrentUser.DownlineEmpId.Contains(a.employee_id ?? 0)).Select(a => new { employee_id = a.employee_id, a.user_id, a.username }).ToList();

                List<clsUserRoleModel> EmpData = new List<clsUserRoleModel>();

                for (int index = 0; index < AllUsers.Count; index++)
                {
                    var UserDetails = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == AllUsers[index].employee_id && a.is_deleted == 0).OrderByDescending(a => a.emp_official_section_id).Select(a => new { userid = AllUsers[index].user_id, empid = a.employee_id, empcode = a.tbl_employee_id_details.emp_code, empname = a.employee_first_name ?? "", empdept = a.tbl_department_master.department_name, empdesig = "", ischecked = false }).FirstOrDefault();
                    if (UserDetails != null)
                    {
                        EmpData.Add(new clsUserRoleModel
                        {
                            empid = UserDetails.empid ?? 0,
                            empname = UserDetails.empname,
                            empcode = UserDetails.empcode,
                            empdept = UserDetails.empdept,
                            empdesig = "",
                            ischecked = false,
                            userid = Convert.ToInt32(UserDetails.userid)
                        });
                    }
                }

                //---update designation here
                var DesgList = _context.tbl_emp_desi_allocation.Where(p => p.tbl_designation_master.designation_id == p.desig_id && p.tbl_designation_master.designation_name != null)
                    .Select(a => new
                    {
                        empid = a.employee_id,
                        designame = a.tbl_designation_master.designation_name,
                        fromdate = a.applicable_from_date
                    }).ToList();

                EmpData.ForEach(p =>
                {
                    var Temp = DesgList.Where(a => a.empid == p.empid).OrderByDescending(b => b.fromdate).FirstOrDefault();
                    p.empdesig = Temp != null ? Temp.designame : "";
                });



                if (roleid > 0)
                {
                    var RoleUserList = _context.tbl_user_role_map.Where(p => p.tbl_user_master.default_company_id == company_id && p.role_id == roleid && p.is_deleted == 0).ToList();

                    EmpData.ForEach(p =>
                    {
                        var Temp = RoleUserList.FirstOrDefault(a => a.user_id == p.userid);
                        p.ischecked = Temp != null ? true : false;
                    });
                }


                return Ok(EmpData);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #endregion ** END BY SUPRIYA ON 31-07-2019

        #region ** STARTED BY SUPRIYA, 09-08-2019
        [Route("BindManagerByCompID/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult BindManagerByCompID([FromRoute] int company_id)
        {
            ResponseMsg objrespone = new ResponseMsg();


            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objrespone.StatusCode = 1;
                objrespone.Message = "Unauthorize access";
            }



            try
            {
                List<EmployeeManager> _lst = new List<EmployeeManager>();
                for (int i = 0; i < _clsCurrentUser.CompanyId.Count; i++)
                {
                    var data = _clsEmployeeDetail.GetEmployeeByDate(_clsCurrentUser.CompanyId[i], new DateTime(2000, 1, 1), DateTime.Now.AddDays(1)).Select(p => new
                    {
                        p.employee_id,
                        emp_name_code = string.Format("{0} ({1})", p.emp_name, p.emp_code)
                    }).ToList();

                    var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && (x.user_type == (int)enmRoleMaster.Manager || x.user_type == (int)enmRoleMaster.Consultant || x.user_type == (int)enmRoleMaster.Management || x.user_type == (int)enmRoleMaster.TeamLeader || x.user_type == (int)enmRoleMaster.SectionHead)).Select(p => p.employee_id).ToList();

                    data.RemoveAll(p => !teos.Contains(p.employee_id));

                    if (data.Count > 0)
                    {
                        _lst.AddRange(data.Select(p => new EmployeeManager
                        {
                            employee_id = p.employee_id,
                            manager_name_code = p.emp_name_code
                        }).ToList());
                    }

                }


                //var data = _context.tbl_user_role_map.Where(x => x.role_id != 1 && x.role_id != 2 && x.is_deleted == 0
                //&& x.tbl_user_master.default_company_id == company_id && x.is_deleted == 0).
                //  //&& x.tbl_user_master.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(g => g.is_deleted == 0).company_id == company_id).
                //  Select(p => new
                //  {
                //      p.tbl_user_master.tbl_employee_id_details.employee_id,
                //      p.tbl_user_master.tbl_employee_id_details.emp_code,
                //      p.tbl_user_master.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(h => h.is_deleted == 0 && h.employee_first_name != "").employee_first_name,
                //      p.tbl_user_master.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(h => h.is_deleted == 0 && h.employee_middle_name != "").employee_middle_name,
                //      p.tbl_user_master.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(h => h.is_deleted == 0 && h.employee_last_name != "").employee_last_name,

                //      emp_name_code = string.Format("{0} {1} {2} ({3})",
                //      p.tbl_user_master.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(h => h.is_deleted == 0 && h.employee_first_name != "").employee_first_name,
                //       p.tbl_user_master.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(h => h.is_deleted == 0 && h.employee_middle_name != "").employee_middle_name,
                //      p.tbl_user_master.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(h => h.is_deleted == 0 && h.employee_last_name != "").employee_last_name,
                //      p.tbl_user_master.tbl_employee_id_details.emp_code)
                //  }).ToList();

                return Ok(_lst);
            }
            catch (Exception ex)
            {
                objrespone.StatusCode = 1;
                objrespone.Message = ex.Message;
                return Ok(objrespone);
            }
        }
        #endregion ** END BY SUPRIYA ON 09-08-2019


        public class MobileImgFile
        {
            public IFormFile files { get; set; }
            public int type { get; set; }
            public string log_zn { get; set; }
            public string join_zn { get; set; }
            public string id { get; set; }
        }

        [HttpPost]
        [Route("PostUploadMultipleFiles")]
        [Consumes("multipart/form-data")]
        // //[Authorize(Policy = "7119")]
        public async Task<IActionResult> PostUploadMultipleFiles([FromForm] MobileImgFile files)
        {
            try
            {
                string MyFileName = "";
                if (files.files.Length > 0)
                {
                    var allowedExtensions = new[] { ".Jpg", ".pdf", "jpeg", ".jpg", ".Pdf", "Jpeg", ".png", ".Png" };

                    var ext = Path.GetExtension(files.files.FileName); //getting the extension

                    if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                    {
                        string name = Path.GetFileNameWithoutExtension(files.files.FileName); //getting file name without extension 

                        Random Ran = new Random();

                        string date_time = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_tt");

                        MyFileName = files.id + "_" + date_time + "_" + Ran.Next() + ext; //Guid.NewGuid().ToString().Replace("-", "") +

                        var filename = ContentDispositionHeaderValue.Parse(files.files.ContentDisposition).FileName.Trim('"');

                        filename = _img_file_path + "/" + files.log_zn + "_" + "/" + $@"\{MyFileName}";


                        if (!Directory.Exists(_img_file_path + "/" + files.log_zn + "_" + "/"))
                        {
                            Directory.CreateDirectory(_img_file_path + "/" + files.log_zn + "_" + "/");
                        }

                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            files.files.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    else
                    {
                        return Ok(new { response = "1", file_name = "" });
                    }

                    return Ok(new { response = "0", file_name = MyFileName });
                }
                else
                {
                    return Ok(new { response = "1", file_name = "" });
                }

            }
            catch (Exception ex)
            {
                return Ok(new { response = "1", file_name = "" });
            }
        }

        [HttpGet]
        [Route("UploadMultipleFiles")]
        //[Authorize(Policy = "7120")]
        public async Task<IActionResult> UploadMultipleFiles(List<IFormFile> files, int type, string log_zn, string join_zn, string id)
        {

            try
            {
                string MyFileName = "";
                if (files.Count > 0)
                {
                    foreach (var formFile in files)
                    {
                        if (formFile.Length > 0)
                        {
                            var allowedExtensions = new[] { ".Jpg", ".pdf", "jpeg", ".jpg", ".Pdf", "Jpeg", ".png", ".Png" };

                            var ext = Path.GetExtension(formFile.FileName); //getting the extension

                            if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                            {
                                string name = Path.GetFileNameWithoutExtension(formFile.FileName); //getting file name without extension 

                                Random Ran = new Random();

                                string date_time = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_tt");

                                MyFileName = id + "_" + date_time + "_" + Ran.Next() + ext; //Guid.NewGuid().ToString().Replace("-", "") +

                                var filename = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');

                                filename = _img_file_path + "/" + log_zn + "_" + "/" + $@"\{MyFileName}";


                                if (!Directory.Exists(_img_file_path + "/" + log_zn + "_" + "/"))
                                {
                                    Directory.CreateDirectory(_img_file_path + "/" + log_zn + "_" + "/");
                                }

                                using (FileStream fs = System.IO.File.Create(filename))
                                {
                                    formFile.CopyTo(fs);
                                    fs.Flush();
                                }
                            }
                            else
                            {
                                return Ok(new { response = "1", file_name = "" });
                            }

                        }
                    }
                    return Ok(new { response = "0", file_name = MyFileName });
                }
                else
                {
                    return Ok(new { response = "1", file_name = "" });
                }

            }
            catch (Exception ex)
            {
                return Ok(new { response = "1", file_name = "" });
            }
        }


        [HttpPost]
        [Route("UploadMultipleFilesOnlyFile")]
        //[Authorize(Policy = "7121")]
        public async Task<IActionResult> UploadMultipleFilesOnlyFile(List<IFormFile> files)
        {

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".Jpg", ".pdf", "jpeg", ".jpg", ".Pdf", "Jpeg", ".png", ".Png" };

                    var ext = Path.GetExtension(formFile.FileName); //getting the extension

                    if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                    {
                        string name = Path.GetFileNameWithoutExtension(formFile.FileName); //getting file name without extension  
                        string MyFileName = formFile.FileName + ext; //Guid.NewGuid().ToString().Replace("-", "") +


                        var webRoot = _hostingEnvironment.WebRootPath;

                        string currentmonth = Convert.ToString(DateTime.Now.Month).Length.ToString() == "1" ? "0" + Convert.ToString(DateTime.Now.Month) : Convert.ToString(DateTime.Now.Month);

                        var currentyearmonth = Convert.ToString(DateTime.Now.Year) + currentmonth;


                        if (!Directory.Exists(webRoot + "/MobileFiles/" + currentyearmonth + "/"))
                        {
                            Directory.CreateDirectory(webRoot + "/MobileFiles/" + currentyearmonth + "/");

                        }


                        if (!Directory.Exists(webRoot + "/MobileFiles/" + currentyearmonth + "/" + formFile.FileName + "/"))
                        {
                            Directory.CreateDirectory(webRoot + "/MobileFiles/" + currentyearmonth + "/" + formFile.FileName + "/");
                        }

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/MobileFiles/" + currentyearmonth + "/" + formFile.FileName + "/");

                        using (var fileStream = new FileStream(Path.Combine(path, MyFileName), FileMode.Create))
                        {
                            formFile.CopyTo(fileStream);

                        }
                    }
                }
            }

            return Ok(new { count = files.Count });
        }


        #region
        [HttpPost]
        [Route("ChangeUserPassword")]

        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult ChangeUserPassword([FromBody] UserMasterSec objuser)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                string new_pwd = AESEncrytDecry.DecryptStringAES(objuser.new_password);

                string old_pwd = AESEncrytDecry.DecryptStringAES(objuser.old_password);

                var exist_dtl = _context.tbl_user_master.Where(x => x.default_company_id == objuser.default_company_id && x.user_id == objuser.user_id && x.is_active == 1).FirstOrDefault();
                if (exist_dtl != null)
                {
                    string existpwd = AESEncrytDecry.DecryptStringAES(exist_dtl.password);
                    if (existpwd == old_pwd)
                    {
                        exist_dtl.password = objuser.new_password;
                        exist_dtl.last_modified_by = objuser.last_modified_by;
                        exist_dtl.last_modified_date = DateTime.Now;

                        _context.Entry(exist_dtl).State = EntityState.Modified;
                        _context.SaveChanges();

                        objresponse.StatusCode = 0;
                        objresponse.Message = "Password Successfully Updated...";
                    }
                    else
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Old Password not match, please try again later";
                    }
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "User Name not exist...";
                }


                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        #endregion

        [Route("ForgotUserPwd/{username}")]
        [HttpPost]
        //[Authorize(Policy = "7123")]
        public IActionResult ForgotUserPwd(string username)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var get_user_password_and_id = _context.tbl_user_master.Where(a => a.username == username && a.is_active == 1).Select(a => new { a.password, a.employee_id, a.username }).FirstOrDefault();

                if (get_user_password_and_id != null)
                {
                    var get_emp_details = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == get_user_password_and_id.employee_id && a.is_deleted == 0).Select(a => new { a.official_email_id }).FirstOrDefault();
                    if (get_emp_details != null)
                    {
                        MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                        string password = AESEncrytDecry.DecryptStringAES(get_user_password_and_id.password);

                        //Task task = Task.Factory.StartNew(() => obj_ms.ForgotPasswordMail(get_emp_details.official_email_id, password, get_user_password_and_id.username));
                        //Task task = Task.Run();


                        var Status= obj_ms.ForgotPasswordMail(get_emp_details.official_email_id, password, get_user_password_and_id.username);
                        if (Status)
                        {
                            objresponse.StatusCode = 0;
                            objresponse.Message = "Password sent to your registered email address...";
                        }
                        else
                        {
                            objresponse.StatusCode = 1;
                            objresponse.Message = "Error in sending mail, contact ";
                        }
                        
                    }
                    else
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Email id not exist...";
                    }
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "User Name not exist...";
                }
                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }



        #region ** START BY SUPRIYA ON 20-11-2019**
        [Route("AddUpdateDocumentTypeMaster")]
        [HttpPost]

        [Authorize(Policy = nameof(enmMenuMaster.DocumentType))]
        public IActionResult AddUpdateDocumentTypeMaster([FromBody] tbl_document_type_master objdocumentmaster)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                if (objdocumentmaster.doc_type_id == 0)
                {
                    var exist = _context.tbl_document_type_master.Where(x => x.doc_name.Trim().ToUpper() == objdocumentmaster.doc_name.Trim().ToUpper() && x.is_deleted == 0).FirstOrDefault();
                    if (exist != null)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Document Type already Exist";
                    }
                    else
                    {
                        objdocumentmaster.created_date = DateTime.Now;
                        objdocumentmaster.is_deleted = 0;

                        _context.Entry(objdocumentmaster).State = EntityState.Added;
                        _context.SaveChanges();

                        objresponse.StatusCode = 0;
                        objresponse.Message = "Document Type Successfully Saved";


                    }
                }
                else
                {
                    var existt = _context.tbl_document_type_master.Where(x => x.doc_type_id == objdocumentmaster.doc_type_id && x.is_deleted == 0).FirstOrDefault();

                    if (existt != null)
                    {
                        var duplicate_record = _context.tbl_document_type_master.Where(x => x.doc_name.Trim().ToUpper() == objdocumentmaster.doc_name.Trim().ToUpper() && x.doc_type_id != objdocumentmaster.doc_type_id && x.is_deleted == 0).FirstOrDefault();

                        if (duplicate_record != null)
                        {
                            objresponse.StatusCode = 1;
                            objresponse.Message = "Document Type Already Exist";
                        }
                        else
                        {
                            existt.company_id = objdocumentmaster.company_id;
                            existt.doc_name = objdocumentmaster.doc_name;
                            existt.remarks = objdocumentmaster.remarks;
                            existt.modified_by = objdocumentmaster.modified_by;
                            existt.modified_date = DateTime.Now;




                            _context.Entry(existt).State = EntityState.Modified;
                            _context.SaveChanges();

                            objresponse.StatusCode = 0;
                            objresponse.Message = "Successfully Updated";

                        }
                    }
                    else
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Invalid Document Type ID...";
                    }
                }


                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("GetDocumentTypeMaster/{doc_type_id}/{company_id}")]
        [HttpGet]

        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult GetDocumentTypeMaster([FromRoute] int doc_type_id, int company_id)
        {
            try
            {
                if (doc_type_id == 0)
                {
                    if (company_id == 0)
                    {
                        var data = _context.tbl_document_type_master.Where(x => x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id)).Select(p => new
                        {
                            p.doc_type_id,
                            p.doc_name,
                            p.company_id,
                            p.company_master.company_name,
                            p.is_deleted,
                            p.created_date,
                            p.modified_date,
                            p.remarks
                        }).ToList();

                        return Ok(data);
                    }
                    else
                    {
                        var data = _context.tbl_document_type_master.Where(x => x.company_id == company_id && x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id)).Select(p => new
                        {
                            p.doc_type_id,
                            p.doc_name,
                            p.company_id,
                            p.company_master.company_name,
                            p.is_deleted,
                            p.created_date,
                            p.modified_date,
                            p.remarks
                        }).ToList();

                        return Ok(data);
                    }

                }
                else
                {
                    var data = _context.tbl_document_type_master.Where(x => x.doc_type_id == doc_type_id && x.is_deleted == 0).OrderByDescending(y => y.doc_type_id).FirstOrDefault();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("DeleteDocumentTypeMaster/{doc_type_id}")]
        [HttpDelete]
        //[Authorize(Policy = "7126")]
        public IActionResult DeleteDocumentTypeMaster([FromRoute] int doc_type_id)
        {
            try
            {

                Response_Msg objresponse = new Response_Msg();

                var exist = _context.tbl_document_type_master.Where(x => x.doc_type_id == doc_type_id && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    exist.is_deleted = 1;
                    exist.modified_date = DateTime.Now;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Document Type Successfully Deleted";
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Document Type ID";
                }

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("GetEmployeementTypeSetting/{company_id}/{_typesettingid}")]
        [HttpGet]
        //[Authorize(Policy = "7127")]
        public IActionResult GetEmployeementTypeSetting(int company_id, int _typesettingid)
        {
            try
            {
                ResponseMsg objresponse = new ResponseMsg();
                if (_typesettingid == 0)
                {
                    if (company_id > 0)
                    {
                        var data = _context.tbl_employeementtype_settings.Where(x => x.is_deleted == 0 && x.company_id == company_id).Select(p => new
                        {
                            p.typesetting_id,
                            p.grade_id,
                            p.grade_master.grade_name,
                            p.employeement_type,
                            p.company_id,
                            p.company_master.company_name,
                            p.created_date,
                            p.modified_date,
                            p.notice_period,
                            p.remarks,
                            p.notice_period_days
                        }).ToList();

                        return Ok(data);
                    }
                    else
                    {
                        var data = _context.tbl_employeementtype_settings.Where(x => x.is_deleted == 0).Select(p => new
                        {
                            p.typesetting_id,
                            p.grade_id,
                            p.grade_master.grade_name,
                            p.employeement_type,
                            p.company_id,
                            p.company_master.company_name,
                            p.created_date,
                            p.modified_date,
                            p.notice_period,
                            p.remarks,
                            p.notice_period_days
                        }).ToList();

                        return Ok(data);
                    }

                }
                else
                {
                    var exist_data = _context.tbl_employeementtype_settings.Where(x => x.is_deleted == 0 && x.typesetting_id == _typesettingid).FirstOrDefault();
                    if (exist_data != null)
                    {
                        return Ok(exist_data);
                    }
                    else
                    {
                        objresponse.StatusCode = 0;
                        objresponse.Message = "Invalid ID..";
                        return Ok(objresponse);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Save_EmployeementTypeSetting")]
        [HttpPost]
        //[Authorize(Policy = "7128")]
        public IActionResult Save_EmployeementTypeSetting([FromBody] tbl_employeementtype_settings objtbl)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var already_exist = _context.tbl_employeementtype_settings.Where(x => x.is_deleted == 0 && x.employeement_type == objtbl.employeement_type && x.grade_id == objtbl.grade_id).ToList();
                if (already_exist.Count > 0)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Employee Type Setting already exist";
                }
                else
                {
                    objtbl.created_date = DateTime.Now;
                    objtbl.is_deleted = 0;
                    _context.Entry(objtbl).State = EntityState.Added;
                    _context.SaveChanges();


                    objresponse.StatusCode = 0;
                    objresponse.Message = "Successfully Saved";
                }

                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Edit_EmployeementTypeSetting")]
        [HttpPost]
        //[Authorize(Policy = "7128")]
        public IActionResult Edit_EmployeementTypeSetting([FromBody] tbl_employeementtype_settings objtbl)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var exist = _context.tbl_employeementtype_settings.Where(x => x.is_deleted == 0 && x.typesetting_id == objtbl.typesetting_id).FirstOrDefault();
                if (exist != null)
                {
                    var duplicate_exist = _context.tbl_employeementtype_settings.Where(x => x.is_deleted == 0 && x.typesetting_id != objtbl.typesetting_id && x.employeement_type == objtbl.employeement_type && x.grade_id == objtbl.grade_id).ToList();
                    if (duplicate_exist.Count > 0)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Employeement Type Already Exist";
                    }
                    else
                    {

                        exist.is_deleted = 1;
                        exist.modified_by = objtbl.modified_by;
                        exist.modified_date = DateTime.Now;
                        _context.Entry(exist).State = EntityState.Modified;


                        objtbl.typesetting_id = 0;
                        objtbl.created_by = exist.created_by; //objtbl.modified_by;
                        objtbl.created_date = exist.created_date;//DateTime.Now;
                        objtbl.modified_date = DateTime.Now;
                        objtbl.is_deleted = 0;

                        _context.Entry(objtbl).State = EntityState.Added;
                        _context.SaveChanges();

                        //exist.grade_id = objtbl.grade_id;
                        //exist.employeement_type = objtbl.employeement_type;
                        //exist.notice_period = objtbl.notice_period;
                        //exist.remarks = objtbl.remarks;
                        //exist.company_id = objtbl.company_id;
                        //exist.is_deleted = 0;
                        //exist.modified_by = objtbl.modified_by;
                        //exist.modified_date = DateTime.Now;
                        //exist.notice_period_days = objtbl.notice_period_days;

                        //_context.Entry(exist).State = EntityState.Modified;
                        //_context.SaveChanges();

                        objresponse.StatusCode = 0;
                        objresponse.Message = "Successfully Updated";
                    }


                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid ID";
                }

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #endregion ** END BY SUPIRYA ON 20-11-2019**


        #region **START BY SUPRIYA ON 16-12-2019,COMPOFF RAISED AND APPROVE BEFORE APPLY FOR COMPOFF REQUEST**
        [Route("GetEmployeeCompOffDataForAdd/{emp_id}/{year}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffCreditApplication))]
        public IActionResult GetEmployeeCompOffDataForAdd([FromRoute] int emp_id, int year)
        {
            ResponseMsg objResult = new ResponseMsg();

            try
            {

                if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access";
                    return Ok(objResult);
                }


                var data = _clsEmployeeDetail.Get_employee_total_compoff_ledger(_clsCurrentUser.DownlineEmpId).ToList();

                if (data.Count > 0)
                {
                    List<int> _emp_id_compoff = data.Where(x => x.balance > 0).Select(p => p.e_id).ToList();

                    var exist_compoff = _context.tbl_comp_off_ledger.Where(x => x.compoff_date.Year == year && x.credit == 1 && x.is_deleted == 0 && _emp_id_compoff.Contains(x.e_id ?? 0)).Select(p => new
                    {
                        p.sno,
                        p.compoff_date,
                        p.e_id,
                        emp_code = p.tbl_employee_id_details.emp_code,
                        emp_name = string.Format("{0} {1} {2}", p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                              p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                              p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name),
                        dept_name = p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).tbl_department_master.department_name
                    }).OrderByDescending(y => y.sno).Distinct().ToList();

                    List<DateTime> exist_compoff_date = exist_compoff.Select(p => p.compoff_date.Date).Distinct().ToList();
                    for (int i = 0; i < _emp_id_compoff.Count; i++)
                    {
                        var already_reg = _context.tbl_compoff_raise.Where(es => (es.emp_id == _emp_id_compoff[i]) && exist_compoff_date.Contains(es.comp_off_date.Date)
                     && es.is_deleted == 0 && es.is_final_approve != 2).Select(p => p.comp_off_date).Distinct().ToList();
                        exist_compoff.RemoveAll(p => already_reg.Contains(p.compoff_date) && p.e_id == _emp_id_compoff[i]);
                    }


                    return Ok(exist_compoff);
                }
                else
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Compoff not available in your account, please check and try !!";
                    return Ok(objResult);
                }
                //select employee whose compoff available







                //var check_comp_bal = _context.tbl_comp_off_ledger.Where(x => x.e_id == emp_id).ToList()
                //                       .GroupBy(p => p.e_id).Select(q => new
                //                       {
                //                           totalcredit = q.Sum(r => r.credit),
                //                           totaldebit = q.Sum(r => r.dredit),
                //                           leavebalance = q.Sum(x => x.credit) - q.Sum(x => x.dredit)
                //                       }).FirstOrDefault();

                //if (check_comp_bal == null || check_comp_bal.leavebalance <= 0)
                //{
                //    objResult.StatusCode = 1;
                //    objResult.Message = "Compoff not available in your account, please check and try !!";
                //    return Ok(objResult);
                //}


                //var exist_compoff = _context.tbl_comp_off_ledger.Where(x => x.e_id == emp_id && x.credit == 1).Select(p => new
                //{
                //    p.sno,
                //    p.compoff_date,
                //    p.e_id
                //}).Distinct().ToList();

                //List<DateTime> exist_compoff_date = exist_compoff.Select(p => p.compoff_date.Date).Distinct().ToList();

                //var already_reg = _context.tbl_compoff_raise.Where(es => (es.emp_id == emp_id) && exist_compoff_date.Contains(es.comp_off_date.Date)
                // && es.is_deleted == 0).Select(p => p.comp_off_date).Distinct().ToList();
                //exist_compoff.RemoveAll(p => already_reg.Contains(p.compoff_date));





                //return Ok(exist_compoff);

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        [Route("AddCompoffForRequest")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffCreditApplication))]
        public IActionResult AddCompoffForRequest([FromBody] tbl_compoff_raise objcompoffraise)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ResponseMsg objresponse = new ResponseMsg();

                //clsEmployeeDetail objempdtl = new clsEmployeeDetail(_context, 0, _AC);
                //bool _finalresult = objempdtl.func_is_user_profile();


                //string actionname = "";
                //if (_finalresult)
                //{
                //    actionname = "read";
                //}
                //else
                //{
                //    actionname = "write";
                //}

                if (!_clsCurrentUser.DownlineEmpId.Any(p => p == objcompoffraise.emp_id))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unauthorize Access...!";
                    return Ok(objresponse);
                }

                var _compoffavailable = _context.tbl_comp_off_ledger.Where(x => x.sno == objcompoffraise.emp_comp_id && x.is_deleted == 0 && x.compoff_date.Date == objcompoffraise.comp_off_date.Date && x.e_id == objcompoffraise.emp_id).FirstOrDefault();
                if (_compoffavailable != null)
                {
                    objcompoffraise.emp_comp_id = 0;
                    var exist = _context.tbl_compoff_raise.Where(x => x.emp_id == objcompoffraise.emp_id && x.is_deleted == 0 && x.comp_off_date.Date == objcompoffraise.comp_off_date.Date && x.is_final_approve != 1 && x.is_final_approve != 2).FirstOrDefault();
                    if (exist != null)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Request Already Raised";
                    }
                    else
                    {
                        var tbl_daily_attendance = _context.tbl_daily_attendance.Where(x => x.emp_id == objcompoffraise.emp_id && x.attendance_dt.Date == objcompoffraise.comp_off_date.Date && x.is_freezed == 1).FirstOrDefault();
                        if (tbl_daily_attendance != null)
                        {
                            objresponse.StatusCode = 1;
                            objresponse.Message = "Attandance of selected period is freezed";
                            return Ok(objresponse);
                        }



                        objcompoffraise.is_deleted = 0;
                        objcompoffraise.created_date = DateTime.Now;
                        objcompoffraise.is_approved1 = 0;
                        objcompoffraise.is_approved2 = 0;
                        objcompoffraise.is_approved3 = 0;
                        objcompoffraise.is_admin_approve = 0;
                        objcompoffraise.is_final_approve = 0;
                        objcompoffraise.a1_e_id = null;
                        objcompoffraise.a2_e_id = null;
                        objcompoffraise.a3_e_id = null;
                        objcompoffraise.admin_id = null;

                        _context.Entry(objcompoffraise).State = EntityState.Added;
                        _context.SaveChanges();


                        MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                        Task task = Task.Run(() => obj_ms.CompoffRaiseApplicationRequestMail(Convert.ToInt32(objcompoffraise.emp_id), objcompoffraise.comp_off_date, objcompoffraise.requester_remarks));

                        task.Wait();

                        objresponse.StatusCode = 0;
                        objresponse.Message = "Request Successfully Raised";
                    }

                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Sorry compoff not available";
                }


                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("GetCompOffRaisedRequest/{loginempid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffCreditApproval))]
        public IActionResult GetCompOffRaisedRequest(int loginempid)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ResponseMsg objresponse = new ResponseMsg();


                List<int> emplst = new List<int>();
                if (!_clsCurrentUser.RoleId.Contains((int)enmRoleMaster.SuperAdmin))
                {

                    List<int> _loginid = new List<int>();
                    _loginid.Add(_clsCurrentUser.EmpId);

                    emplst = _clsCurrentUser.DownlineEmpId.Except(_loginid).ToList();
                }
                else
                {
                    emplst = _clsCurrentUser.DownlineEmpId.ToList();
                }


                if (_clsCurrentUser.Is_Hod == 1)
                {

                    var pending_request = _context.tbl_compoff_raise.Join(_context.tbl_daily_attendance, a => new { _against_dt = a.comp_off_date.Date, emp_id = a.emp_id },
                        b => new { _against_dt = b.attendance_dt.Date, emp_id = b.emp_id }, (a, b) => new
                        {

                            employee_id = a.emp_id,
                            emp_name = string.Format("{0} {1} {2}", a.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                            a.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                            a.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name),
                            emp_code = a.emp_requester.emp_code,
                            leave_request_id = a.emp_comp_id,
                            from_date = a.comp_off_date,
                            system_in_time = b.in_time,
                            system_out_time = b.out_time,
                            requester_remarks = a.requester_remarks,
                            status = a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : a.is_final_approve == 2 ? "Reject" : a.is_final_approve == 3 ? "In Process" : "",//Final approver status
                            approver_remarks = (a.emp_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_one_id == _clsCurrentUser.EmpId ? a.approval1_remarks :
                            a.emp_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_two_id == _clsCurrentUser.EmpId ? a.approval2_remarks :
                            a.emp_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_three_id == _clsCurrentUser.EmpId ? a.approval3_remarks : ""),
                            my_status = (a.emp_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_one_id == _clsCurrentUser.EmpId ? (a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approve" : a.is_approved1 == 2 ? "Reject" : "") :
                            a.emp_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_two_id == _clsCurrentUser.EmpId ? (a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approve" : a.is_approved2 == 2 ? "Reject" : "") :
                            a.emp_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_three_id == _clsCurrentUser.EmpId ? (a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approve" : a.is_approved3 == 2 ? "Reject" : "") : ""),//Manager Status
                            requester_date = a.created_date,
                            a.is_deleted,
                            a.is_final_approve,
                            mgr1 = a.emp_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_one_id,
                            mgr2 = a.emp_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_two_id,
                            mgr3 = a.emp_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_three_id,


                            b.is_freezed,
                        }).Where(x => x.is_deleted == 0 && x.is_final_approve != 1 && x.is_final_approve != 2 && x.is_freezed == 0 && emplst.Contains(x.employee_id ?? 0)).ToList();

                    return Ok(pending_request);
                }
                else if (_clsCurrentUser.Is_Hod == 2)
                {
                    var pending_request = _context.tbl_compoff_raise.Join(_context.tbl_daily_attendance, a => new { _against_dt = a.comp_off_date.Date, emp_id = a.emp_id },
                             b => new { _against_dt = b.attendance_dt.Date, emp_id = b.emp_id }, (a, b) => new
                             {

                                 employee_id = a.emp_id,
                                 emp_name = string.Format("{0} {1} {2}", a.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                            a.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                            a.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name),
                                 emp_code = a.emp_requester.emp_code,
                                 leave_request_id = a.emp_comp_id,
                                 from_date = a.comp_off_date,
                                 system_in_time = b.in_time,
                                 system_out_time = b.out_time,
                                 requester_remarks = a.requester_remarks,
                                 status = a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : a.is_final_approve == 2 ? "Reject" : a.is_final_approve == 3 ? "In Process" : "",//Final approver status
                                 approver_remarks = a.admin_remarks,
                                 my_status = a.is_admin_approve == 0 ? "Pending" : a.is_admin_approve == 1 ? "Approve" : a.is_admin_approve == 2 ? "Reject" : "",//Manager Status
                                 requester_date = a.created_date,
                                 a.is_deleted,
                                 a.is_final_approve,
                                 b.is_freezed,
                             }).Where(x => x.is_deleted == 0 && emplst.Contains(x.employee_id ?? 0) && x.is_final_approve != 1 && x.is_final_approve != 2 && x.is_freezed == 0).ToList();

                    return Ok(pending_request);
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unauthorize Access...!";
                    return Ok(objresponse);
                }







                //clsEmployeeDetail objempdtl = new clsEmployeeDetail(_context, 0, _AC);
                //bool _finalresult = objempdtl.func_is_user_profile();


                //string actionname = "";
                //if (_finalresult)
                //{
                //    actionname = "read";
                //}
                //else
                //{
                //    actionname = "write";
                //}

                //Classes.clsLoginEmpDtl ob = new clsLoginEmpDtl(_context, Convert.ToInt32(loginempid), actionname, _AC);

                //if (!ob.is_valid())
                //{
                //    objresponse.StatusCode = 1;
                //    objresponse.Message = "Unauthorize Access...!";
                //    return Ok(objresponse);
                //}


                // var managerss = _context.tbl_emp_manager.Where(a => (a.m_one_id == loginempid || a.m_two_id == loginempid || a.m_three_id == loginempid) && a.is_deleted == 0).OrderByDescending(b => b.emp_mgr_id).Select(p =>
                //         p.employee_id
                //).Distinct().ToList();
                // if (managerss.Count > 0)
                // {
                //     var pending_request = (from a in _context.tbl_compoff_raise
                //                            from b in _context.tbl_daily_attendance
                //                            where a.is_deleted == 0 && a.comp_off_date.Date == b.attendance_dt.Date && a.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).emp_official_section_id == b.emp_offcl_id
                //                            && managerss.Contains(a.emp_id)
                //                            select new
                //                            {
                //                                a.emp_comp_id,
                //                                a.emp_id,
                //                                a.comp_off_date,
                //                                a.requester_remarks,
                //                                a.is_approved1,
                //                                a.is_approved2,
                //                                a.is_approved3,
                //                                a.is_final_approve,
                //                                a.approval1_remarks,
                //                                a.approval2_remarks,
                //                                a.approval3_remarks,
                //                                status = a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approved" : a.is_final_approve == 3 ? "Rejected" : "",
                //                                req_emp_name = string.Format("{0} {1} {2} ({3})",
                //                                               a.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(d => d.is_deleted == 0 && !string.IsNullOrEmpty(d.employee_first_name)).employee_first_name,
                //                                               a.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(d => d.is_deleted == 0 && !string.IsNullOrEmpty(d.employee_first_name)).employee_middle_name,
                //                                               a.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(d => d.is_deleted == 0 && !string.IsNullOrEmpty(d.employee_first_name)).employee_last_name,
                //                                               a.emp_requester.emp_code),
                //                                a.is_deleted,
                //                                a.created_date,
                //                                system_in_time = b.in_time,
                //                                system_out_time = b.out_time
                //                            }).ToList();




                //     List<AllLeaveRequestReport> obj_leave_req = new List<AllLeaveRequestReport>();

                //     for (int Index = 0; Index < pending_request.Count; Index++)
                //     {
                //         var tbl_emp_m = _context.tbl_emp_manager.Where(x => x.employee_id == pending_request[Index].emp_id && x.is_deleted == 0).OrderByDescending(a => a.emp_mgr_id).FirstOrDefault();
                //         if (tbl_emp_m.m_one_id == loginempid)
                //         {
                //             obj_leave_req = pending_request.Select(TempIndex => new AllLeaveRequestReport
                //             {
                //                 employee_id = Convert.ToInt32(TempIndex.emp_id),
                //                 employee_name = TempIndex.req_emp_name,
                //                 leave_request_id = TempIndex.emp_comp_id,
                //                 from_date = TempIndex.comp_off_date,
                //                 system_in_time = TempIndex.system_in_time,
                //                 system_out_time = TempIndex.system_out_time,
                //                 requester_remarks = TempIndex.requester_remarks,
                //                 status = TempIndex.status,//Final approver status
                //                 approver_remarks = TempIndex.approval1_remarks,
                //                 my_status = TempIndex.is_approved1 == 0 ? "Pending" : TempIndex.is_approved1 == 1 ? "Approve" : "Reject", //Manager Status
                //                 requester_date = TempIndex.created_date
                //                 // my_status_level = 1,
                //             }).ToList();
                //         }
                //         else if (tbl_emp_m.m_two_id == loginempid)
                //         {
                //             obj_leave_req = pending_request.Select(TempIndex => new AllLeaveRequestReport
                //             {
                //                 employee_id = Convert.ToInt32(TempIndex.emp_id),
                //                 employee_name = TempIndex.req_emp_name,
                //                 leave_request_id = TempIndex.emp_comp_id,
                //                 from_date = TempIndex.comp_off_date,
                //                 system_in_time = TempIndex.system_in_time,
                //                 system_out_time = TempIndex.system_out_time,
                //                 requester_remarks = TempIndex.requester_remarks,
                //                 status = TempIndex.status,//Final approver status
                //                 approver_remarks = TempIndex.approval1_remarks,
                //                 my_status = TempIndex.is_approved2 == 0 ? "Pending" : TempIndex.is_approved2 == 1 ? "Approve" : "Reject",//Manager Status
                //                 requester_date = TempIndex.created_date
                //                 //my_status_level = 2,
                //             }).ToList();
                //         }
                //         else if (tbl_emp_m.m_three_id == loginempid)
                //         {
                //             obj_leave_req = pending_request.Select(TempIndex => new AllLeaveRequestReport
                //             {
                //                 employee_id = Convert.ToInt32(TempIndex.emp_id),
                //                 employee_name = TempIndex.req_emp_name,
                //                 leave_request_id = TempIndex.emp_comp_id,
                //                 from_date = TempIndex.comp_off_date,
                //                 system_in_time = TempIndex.system_in_time,
                //                 system_out_time = TempIndex.system_out_time,
                //                 requester_remarks = TempIndex.requester_remarks,
                //                 status = TempIndex.status,//Final approver status
                //                 approver_remarks = TempIndex.approval1_remarks,
                //                 my_status = TempIndex.is_approved3 == 0 ? "Pending" : TempIndex.is_approved3 == 1 ? "Approve" : "Reject",//Manager Status
                //                 requester_date = TempIndex.created_date
                //                 //my_status_level = 3,
                //             }).ToList();
                //         }
                //     }


                //     obj_leave_req.RemoveAll(p => p.my_status.Trim() == "Approve" || p.my_status.Trim() == "Reject");

                //     return Ok(obj_leave_req);
                //}
                //else
                //{
                //    objresponse.StatusCode = 1;
                //    objresponse.Message = "Sorry you are not authorize to approve this application";
                //    return Ok(objresponse);
                //}

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("RaisedCompoffApproveReject")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffCreditApproval))]
        public IActionResult RaisedCompoffApproveReject([FromBody] LeaveAppModel objcompraise)
        {
            ResponseMsg objResult = new ResponseMsg();
            objResult.StatusCode = 0;
            List<string> ErrorMessage = new List<string>();
            int LeaveTypeId = 4;
            int? leaveInfoIds = null;
            var varleaveInfoIds = _context.tbl_leave_info.Where(p => p.leave_type_id == LeaveTypeId && p.is_active == 1 && p.leave_tenure_from_date < DateTime.Now && p.leave_tenure_to_date >= DateTime.Now).FirstOrDefault();
            if (varleaveInfoIds != null)
            {
                leaveInfoIds = varleaveInfoIds.leave_info_id;
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string remarksRegex = @"^[a-zA-Z'\s'\.]{1,200}$";
                Regex rename = new Regex(remarksRegex);
                if (!string.IsNullOrEmpty(objcompraise.approval1_remarks))
                {
                    if (!rename.IsMatch(objcompraise.approval1_remarks))
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Invalid remarks, special characters are not allowed in remarks";
                        return Ok(objResult);
                    }
                }

                var ReqEmpIDs = _context.tbl_compoff_raise.Where(x => x.is_deleted == 0 && x.is_final_approve == 0 && objcompraise.compoff_raise_id.Any(p => p.emp_comp_id == x.emp_comp_id && p.emp_id == x.emp_id)).ToList();
                List<int?> _ReqEmps = ReqEmpIDs.Select(p => p.emp_id).ToList();
                foreach (var ids in _ReqEmps)
                {
                    if (!_clsCurrentUser.DownlineEmpId.Any(p => p == ids))
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Unauthorize Access...!";
                        return Ok(objResult);
                    }
                }

                List<ApplicatonMailDetails> objmail_lst = new List<ApplicatonMailDetails>();

                for (int i = 0; i < objcompraise.compoff_raise_id.Count; i++)
                {
                    var exist = ReqEmpIDs.Where(x => x.emp_id == objcompraise.compoff_raise_id[i].emp_id && x.comp_off_date.Date == objcompraise.compoff_raise_id[i].comp_off_date.Date && x.emp_comp_id == objcompraise.compoff_raise_id[i].emp_comp_id && x.is_final_approve == 0).FirstOrDefault();
                    if (exist == null)
                    {
                        objResult.StatusCode = 1;
                        if (!ErrorMessage.Any(p => p == "Selected Application already approve or reject, please check report !!"))
                        {
                            ErrorMessage.Add("Selected Application already approve or reject, please check report !!");
                        }
                        continue;
                    }

                    if (_clsCurrentUser.Is_Hod == 1)
                    {
                        var tbl_emp_m = _context.tbl_emp_manager.Where(x => x.employee_id == exist.emp_id && x.is_deleted == 0).OrderByDescending(a => a.emp_mgr_id).FirstOrDefault();
                        if (tbl_emp_m != null)
                        {
                            int final_mgr_id = 0;
                            int final_approvel = tbl_emp_m.final_approval;

                            if (tbl_emp_m.m_one_id == objcompraise.a1_e_id)
                            {
                                exist.is_approved1 = objcompraise.is_approved1;
                                exist.approval1_remarks = objcompraise.approval1_remarks;
                                exist.a1_e_id = objcompraise.a1_e_id;
                                exist.approver1_dt = DateTime.Now;
                            }
                            else if (tbl_emp_m.m_two_id == objcompraise.a1_e_id)
                            {
                                exist.is_approved2 = objcompraise.is_approved1;
                                exist.approval2_remarks = objcompraise.approval1_remarks;
                                exist.a2_e_id = objcompraise.a1_e_id;
                                exist.approver2_dt = DateTime.Now;
                            }
                            else if (tbl_emp_m.m_three_id == objcompraise.a1_e_id)
                            {
                                exist.is_approved3 = objcompraise.is_approved1;
                                exist.approval3_remarks = objcompraise.approval1_remarks;
                                exist.a3_e_id = objcompraise.a1_e_id;
                                exist.approver3_dt = DateTime.Now;
                            }

                            final_mgr_id = (final_approvel == 1 ? tbl_emp_m.m_one_id : final_approvel == 2 ? tbl_emp_m.m_two_id : final_approvel == 3 ? tbl_emp_m.m_three_id : 0) ?? 0;

                            if (final_mgr_id == objcompraise.a1_e_id)
                            {
                                exist.is_final_approve = objcompraise.is_approved1;
                            }
                        }
                        else
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Manager are not set for approve or reject selected request";
                            return Ok(objResult);
                        }


                    }
                    else if (_clsCurrentUser.Is_Hod == 2)
                    {
                        exist.is_final_approve = objcompraise.is_approved1;
                        exist.is_admin_approve = objcompraise.is_approved1;
                        exist.admin_remarks = objcompraise.approval1_remarks;
                        exist.admin_id = objcompraise.a1_e_id;
                        exist.admin_ar_date = DateTime.Now;
                    }
                    else
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "You cannot approve or reject compoff addition request";
                        return Ok(objResult);
                    }

                    if (exist.is_final_approve == 1 || exist.is_final_approve == 2)
                    {
                        ApplicatonMailDetails _maill = new ApplicatonMailDetails();
                        _maill.req_id = exist.emp_comp_id;
                        _maill.emp_id = exist.emp_id ?? 0;
                        _maill.from_date = exist.comp_off_date;
                        _maill.approver_remarks = objcompraise.approval1_remarks;
                        _maill.is_approve = exist.is_final_approve;
                        _maill.a_e_id = exist.a1_e_id > 0 ? exist.a1_e_id : exist.a2_e_id > 0 ? exist.a2_e_id : exist.a3_e_id > 0 ? exist.a3_e_id : exist.admin_id;
                        objmail_lst.Add(_maill);
                    }
                    _context.tbl_compoff_raise.UpdateRange(exist);
                    if (exist.is_final_approve == 1)
                    {
                        if (_context.tbl_leave_ledger.Where(p => p.leave_type_id == LeaveTypeId && p.transaction_date == exist.comp_off_date && p.e_id == exist.emp_id && p.credit > 0).Count() == 0)
                        {
                            tbl_leave_ledger objtbl_leave_ledger = new tbl_leave_ledger();
                            objtbl_leave_ledger.leave_type_id = LeaveTypeId;
                            objtbl_leave_ledger.leave_info_id = leaveInfoIds;
                            objtbl_leave_ledger.transaction_date = exist.comp_off_date;
                            objtbl_leave_ledger.entry_date = DateTime.Now;
                            objtbl_leave_ledger.transaction_type = 1; // leave add
                            objtbl_leave_ledger.monthyear = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
                            objtbl_leave_ledger.transaction_no = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
                            objtbl_leave_ledger.leave_addition_type = 0;
                            objtbl_leave_ledger.credit = 1;
                            objtbl_leave_ledger.dredit = 0;
                            objtbl_leave_ledger.remarks = objcompraise.approval1_remarks;
                            objtbl_leave_ledger.e_id = exist.emp_id;
                            objtbl_leave_ledger.created_by = exist.a1_e_id > 0 ? exist.a1_e_id : exist.a2_e_id > 0 ? exist.a2_e_id : exist.a3_e_id > 0 ? exist.a3_e_id : exist.admin_id;
                            _context.tbl_leave_ledger.AddRange(objtbl_leave_ledger);
                        }
                    }
                    _context.SaveChanges();
                }
                if (objcompraise.is_approved1 == 1) objResult.Message = "" + objmail_lst.Count + " application approved successfully !! <br/>" + string.Join("<br/>", ErrorMessage);
                if (objcompraise.is_approved1 == 2) objResult.Message = "" + objmail_lst.Count + " application rejected successfully !! <br/>" + string.Join("<br/>", ErrorMessage);
                objResult.Message = "Successfully " + (objcompraise.is_approved1 == 1 ? "Approved" : "Rejected") + "";
                if (objmail_lst.Count > 0)
                {
                    MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);
                    Task task = Task.Run(() => obj_ms.CompoffRaiseApplicationApprovalMail(objmail_lst));
                    task.Wait();
                }
                return Ok(objResult);


                //return Ok(objresponse);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 1;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }

        }

        [Route("GetEmployeeCompOff/{EmployeeId}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffApplication))]
        public IActionResult GetEmployeeCompOff(int EmployeeId)
        {
            ResponseMsg objresponse = new ResponseMsg();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_clsCurrentUser.DownlineEmpId.Contains(EmployeeId))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unauthorize Access....!!";
                    return Ok(objresponse);
                }

                var data = _context.tbl_compoff_raise.Where(x => x.is_deleted == 0 && x.emp_id == EmployeeId && x.is_final_approve == 1 && x.comp_off_date.Year == DateTime.Now.Year).Select(p => new
                {
                    p.comp_off_date
                }).ToList();


                var raisedcompoff = _context.tbl_comp_off_request_master.Where(x => x.is_deleted == 0 && x.r_e_id == EmployeeId && x.is_final_approve != 2)
                                    .GroupBy(t => new { t.r_e_id, t.compoff_against_date.Date }).Select(g => g.OrderByDescending(t => t.comp_off_request_id).FirstOrDefault()).Select(p => p.compoff_against_date.Date).ToList();


                if (raisedcompoff.Count > 0)
                {
                    data.RemoveAll(p => raisedcompoff.Contains(p.comp_off_date.Date));
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("GetCompOffRaisedRequestByAdmin/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffCreditCancel))]
        public IActionResult GetCompOffRaisedRequestByAdmin(int company_id)
        {
            try
            {



                var attandace_data = _context.tbl_daily_attendance.Where(x => x.is_freezed == 0 && _clsCurrentUser.DownlineEmpId.Contains(x.emp_id ?? 0)).ToList();

                var result = (from a in _context.tbl_compoff_raise
                              join b in attandace_data on new { _date = a.comp_off_date.Date, _empidd = a.emp_id } equals
                              new { _date = b.attendance_dt.Date, _empidd = b.emp_id } into ej
                              from b in ej.DefaultIfEmpty()
                              where a.is_deleted == 0 && a.is_final_approve == 1 && _clsCurrentUser.DownlineEmpId.Contains(a.emp_id ?? 0)
                              select new
                              {

                                  a.emp_comp_id,
                                  a.emp_id,
                                  a.comp_off_date,
                                  a.requester_remarks,
                                  a.is_approved1,
                                  a.is_approved2,
                                  a.is_approved3,
                                  a.is_final_approve,
                                  a.approval1_remarks,
                                  a.approval2_remarks,
                                  a.approval3_remarks,
                                  status = a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approved" : a.is_final_approve == 3 ? "Rejected" : "",
                                  emp_name = string.Format("{0} {1} {2}",
                                                 a.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(d => d.is_deleted == 0 && !string.IsNullOrEmpty(d.employee_first_name)).employee_first_name,
                                                 a.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(d => d.is_deleted == 0 && !string.IsNullOrEmpty(d.employee_first_name)).employee_middle_name,
                                                 a.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(d => d.is_deleted == 0 && !string.IsNullOrEmpty(d.employee_first_name)).employee_last_name),
                                  emp_code = a.emp_requester.emp_code,
                                  a.is_deleted,
                                  a.created_date,
                                  system_in_time = b != null ? b.in_time : Convert.ToDateTime("2000-01-01 00:00"),
                                  system_out_time = b != null ? b.out_time : Convert.ToDateTime("2000-01-01 00:00")
                              }).ToList();



                var process_data = _context.tbl_payroll_process_status.Where(x => x.is_deleted == 0 && x.is_freezed == 1 && _clsCurrentUser.DownlineEmpId.Contains(x.emp_id ?? 0)).ToList();


                for (int i = result.Count() - 1; i >= 0; i--)
                {
                    int from_date = Convert.ToInt32(result[i].comp_off_date.Year.ToString() + Convert.ToString(result[i].comp_off_date.Month.ToString().Length > 1 ? result[i].comp_off_date.Month.ToString() : "0" + result[i].comp_off_date.Month.ToString()));

                    bool exist = process_data.Any(x => x.emp_id == result[i].emp_id && x.payroll_month_year == from_date);
                    if (exist)
                    {
                        result.RemoveAll(x => x.emp_comp_id == result[i].emp_comp_id);
                    }

                }


                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("DeleteCompOffRaiseByAdmin")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffCreditCancel))]
        public IActionResult DeleteCompOffRaiseByAdmin([FromBody] LeaveAppModel objtbl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ResponseMsg objresponse = new ResponseMsg();

                var ReqEmIDs = _context.tbl_compoff_raise.Where(x => x.is_deleted == 0 && x.is_final_approve == 1 && objtbl.compoff_raise_id.Any(p => p.emp_comp_id == x.emp_comp_id && p.emp_id == x.emp_id)).ToList();

                List<int?> _reqempids = ReqEmIDs.Select(p => p.emp_id).ToList();
                foreach (var ids in _reqempids)
                {

                    if (!_clsCurrentUser.DownlineEmpId.Any(p => p == ids))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Unauthorize Access...!";
                        return Ok(objresponse);
                    }
                }

                string remarksRegex = @"^[a-zA-Z'\s'\.]{1,200}$";

                Regex rename = new Regex(remarksRegex);

                if (!string.IsNullOrEmpty(objtbl.approval1_remarks))
                {
                    if (!rename.IsMatch(objtbl.approval1_remarks))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Invalid remarks, special characters are not allowed in remarks";
                        return Ok(objresponse);
                    }
                }
                int LeaveTypeId = 4;
                int? leaveInfoIds = null;
                var varleaveInfoIds = _context.tbl_leave_info.Where(p => p.leave_type_id == LeaveTypeId && p.is_active == 1 && p.leave_tenure_from_date < DateTime.Now && p.leave_tenure_to_date >= DateTime.Now).FirstOrDefault();
                if (varleaveInfoIds != null)
                {
                    leaveInfoIds = varleaveInfoIds.leave_info_id;
                }

                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        //  var _data = _context.tbl_compoff_raise.Where(x => x.is_deleted == 0 && x.is_final_approve == 1).ToList();

                        var compoff_master = _context.tbl_comp_off_request_master.Where(x => x.is_deleted == 0).ToList();

                        for (int i = 0; i < objtbl.compoff_raise_id.Count; i++)
                        {
                            var exist_request = ReqEmIDs.Where(x => x.emp_comp_id == Convert.ToInt32(objtbl.compoff_raise_id[i].emp_comp_id) && x.comp_off_date.Date == objtbl.compoff_raise_id[i].comp_off_date.Date && x.emp_id == objtbl.compoff_raise_id[i].emp_id).FirstOrDefault();
                            if (exist_request != null)
                            {
                                var _applied_data = compoff_master.Where(x => x.compoff_against_date.Date == objtbl.compoff_raise_id[i].comp_off_date.Date && x.r_e_id == objtbl.compoff_raise_id[i].emp_id && x.is_deleted == 0).FirstOrDefault();
                                if (_applied_data != null)
                                {
                                    trans.Rollback();
                                    objresponse.StatusCode = 1;
                                    objresponse.Message = "" + objtbl.compoff_raise_id[i].comp_off_date.Date.ToString("dd-MM-yyyy") + " CompOff Already applied,sorry you cannot delete this request";
                                    return Ok(objresponse);
                                }
                                else
                                {
                                    var tbl_daily_attendance = _context.tbl_daily_attendance.Where(x => x.emp_id == objtbl.compoff_raise_id[i].emp_id && x.attendance_dt.Date == objtbl.compoff_raise_id[i].comp_off_date.Date && x.is_freezed == 1).FirstOrDefault();
                                    if (tbl_daily_attendance != null)
                                    {
                                        trans.Rollback();

                                        objresponse.StatusCode = 1;
                                        objresponse.Message = "" + objtbl.compoff_raise_id[i].comp_off_date.Date.ToString("dd-MM-yyy") + " Attandance of selected period is freezed";
                                        return Ok(objresponse);
                                    }

                                    var exist_comp_off = _context.tbl_leave_ledger.Where(x => x.e_id == exist_request.emp_id && x.leave_type_id == 4).ToList()
                                                  .GroupBy(p => p.e_id).Select(q => new
                                                  {
                                                      totalcredit = q.Sum(r => r.credit),
                                                      totaldebit = q.Sum(r => r.dredit),
                                                      leavebalance = q.Sum(x => x.credit) - q.Sum(x => x.dredit)
                                                  }).FirstOrDefault();

                                    if (exist_comp_off == null || exist_comp_off.totalcredit <= 0 || exist_comp_off.leavebalance <= 0)
                                    {
                                        trans.Rollback();
                                        objresponse.StatusCode = 1;
                                        objresponse.Message = "Compoff not available in your account, please check and try !!";
                                        return Ok(objresponse);
                                    }


                                    exist_request.is_deleted = 2;
                                    exist_request.deleted_by = objtbl.a1_e_id;
                                    exist_request.deleted_dt = DateTime.Now;
                                    exist_request.deleted_remarks = objtbl.approval1_remarks;


                                    _context.tbl_compoff_raise.UpdateRange(exist_request);





                                    //start if compoff raise request delete by admin, than again debit leave of same date in compoff leave ledger


                                    tbl_leave_ledger objtbl_leave_ledger = new tbl_leave_ledger();
                                    objtbl_leave_ledger.leave_type_id = LeaveTypeId;
                                    objtbl_leave_ledger.leave_info_id = leaveInfoIds;
                                    objtbl_leave_ledger.transaction_date = exist_request.comp_off_date;
                                    objtbl_leave_ledger.entry_date = DateTime.Now;
                                    objtbl_leave_ledger.transaction_type = 1; // leave add
                                    objtbl_leave_ledger.monthyear = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
                                    objtbl_leave_ledger.transaction_no = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
                                    objtbl_leave_ledger.leave_addition_type = 0;
                                    objtbl_leave_ledger.credit = 1;
                                    objtbl_leave_ledger.dredit = 0;
                                    objtbl_leave_ledger.remarks = objtbl.approval1_remarks;
                                    objtbl_leave_ledger.e_id = exist_request.emp_id;
                                    objtbl_leave_ledger.created_by = exist_request.a1_e_id > 0 ? exist_request.a1_e_id : exist_request.a2_e_id > 0 ? exist_request.a2_e_id : exist_request.a3_e_id > 0 ? exist_request.a3_e_id : exist_request.admin_id;
                                    _context.tbl_leave_ledger.AddRange(objtbl_leave_ledger);

                                    //tbl_comp_off_ledger objcomledger = new tbl_comp_off_ledger();

                                    //objcomledger.compoff_date = exist_request.comp_off_date.Date;
                                    //objcomledger.credit = 0;
                                    //objcomledger.dredit = 1;
                                    //objcomledger.transaction_date = DateTime.Now;
                                    //objcomledger.transaction_type = 6; //Mannual Deduction
                                    //objcomledger.monthyear = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
                                    //objcomledger.transaction_no = Convert.ToInt32("-" + exist_request.comp_off_date.ToString("yyyyMMdd"));
                                    //objcomledger.remarks = "Delete by Admin:-" + objtbl.approval1_remarks;
                                    //objcomledger.e_id = exist_request.emp_id;


                                    //_context.tbl_comp_off_ledger.AddRange(objcomledger);


                                    //end if compoff raise request delete by admin, than again debit leave of same date in compoff leave ledger

                                }
                            }
                            _context.SaveChanges();
                        }
                        trans.Commit();

                        objresponse.StatusCode = 0;
                        objresponse.Message = "Request successfully Deleted";

                        return Ok(objresponse);
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        objresponse.StatusCode = 1;
                        objresponse.Message = ex.Message;
                        return Ok(objresponse);
                    }
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Save_MannualCompoff")]
        [HttpPost]
        //[Authorize("7135")]
        public IActionResult Save_MannualCompoff([FromBody] cls_comp_off_ledger objtbl1)
        {
            ResponseMsg objresponse = new ResponseMsg();
            try
            {
                tbl_comp_off_ledger objtbl = new tbl_comp_off_ledger();

                if (objtbl1.e_id < 1)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Please select Employee...";
                }

                var exist_dtl = _context.tbl_comp_off_ledger.Where(x => x.e_id == objtbl1.e_id && x.is_deleted == 0 && x.compoff_date.Date == DateTime.Parse(objtbl1.compoff_date1).Date).ToList();
                if (exist_dtl.Count() > 0)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Compoff already available";
                }
                else
                {
                    objtbl.compoff_date = Convert.ToDateTime(objtbl1.compoff_date1);
                    objtbl.e_id = objtbl1.e_id;
                    objtbl.monthyear = objtbl1.monthyear;
                    objtbl.credit = 1;
                    objtbl.dredit = 0;
                    objtbl.transaction_date = DateTime.Now;
                    objtbl.transaction_type = 5;
                    objtbl.transaction_no = Convert.ToInt32("-" + DateTime.Parse(objtbl1.compoff_date1).ToString("yyyyMMdd"));
                    objtbl.remarks = "Add by Admin:-" + objtbl.remarks;

                    _context.Entry(objtbl).State = EntityState.Added;
                    _context.SaveChanges();

                    MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                    Task task = Task.Run(() => obj_ms.CompoffRaiseApplicationRequestMail(Convert.ToInt32(objtbl.e_id), DateTime.Parse(objtbl1.compoff_date1), objtbl.remarks));

                    task.Wait();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Successfully added";
                }

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 0;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("GetMannaulCompOff/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "7136")]
        public IActionResult GetMannaulCompOff(int company_id)
        {
            try
            {
                var data = _context.tbl_comp_off_ledger.Where(x => x.tbl_employee_id_details.tbl_employee_company_map.OrderByDescending(y => y.sno).FirstOrDefault(z => z.is_deleted == 0).company_id == company_id && x.is_deleted == 0 && x.transaction_type == 5).Select(p => new
                {
                    p.sno,
                    p.compoff_date,
                    p.e_id,
                    p.transaction_date,
                    emp_name = string.Format("{0} {1} {2}",
                          p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(z => z.is_deleted == 0 && !string.IsNullOrEmpty(z.employee_first_name)).employee_first_name,
                           p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(z => z.is_deleted == 0 && !string.IsNullOrEmpty(z.employee_first_name)).employee_middle_name,
                            p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(z => z.is_deleted == 0 && !string.IsNullOrEmpty(z.employee_first_name)).employee_last_name),
                    emp_code = p.tbl_employee_id_details.emp_code,
                    p.remarks

                }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("GetCompoffRaisedReport")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffCreditApplicationReport))]
        public IActionResult GetCompoffRaisedReport([FromBody] LeaveReport objmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ResponseMsg objresponse = new ResponseMsg();

            try
            {
                if (DateTime.Compare(DateTime.Parse(objmodel.from_date).Date, DateTime.Parse(objmodel.to_date).Date) > 0)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "To Date must be greater than from date";
                    return Ok(objresponse);
                }


                objmodel.empdtl.RemoveAll(p => Convert.ToInt32(p) < 0 || Convert.ToInt32(p) == 0);
                foreach (var ids in objmodel.empdtl)
                {
                    if (!_clsCurrentUser.DownlineEmpId.Contains(Convert.ToInt32(ids)))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Unauthorize Access...!!";
                        return Ok(objresponse);
                    }
                }


                var result = _context.tbl_compoff_raise.Where(x => DateTime.Parse(objmodel.from_date).Date <= x.comp_off_date.Date && x.comp_off_date.Date <= DateTime.Parse(objmodel.to_date).Date && ((_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin) ? (objmodel.empdtl.Count > 1 ? true : objmodel.empdtl.Contains(x.emp_id ?? 0)) : objmodel.empdtl.Contains(x.emp_id ?? 0))).Select(p => new
                {
                    emp_comp_id = p.emp_comp_id,
                    comp_off_date = p.comp_off_date,
                    comp_off_day = p.comp_off_date.DayOfWeek.ToString(),
                    actual_in_time = p.comp_off_date,
                    actual_out_time = p.comp_off_date,
                    tot_working_hours = p.comp_off_date.Subtract(p.comp_off_date).TotalHours,
                    emp_id = p.emp_id,
                    req_code = p.emp_requester.emp_code,
                    req_name = string.Format("{0} {1} {2}",
                        p.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                        p.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                        p.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name),
                    req_name_code = string.Format("{0} {1} {2} ({3})",
                        p.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                        p.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                        p.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name,
                        p.emp_requester.emp_code),
                    dept_name = p.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).tbl_department_master.department_name,
                    designation = p.emp_requester.tbl_emp_desi_allocation.FirstOrDefault().tbl_designation_master.designation_name,
                    location = p.emp_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_location_master.location_name,
                    requester_remarks = p.requester_remarks,
                    is_final_approve = p.is_final_approve,
                    duration = p.comp_off_date.Subtract(p.comp_off_date).TotalDays + 1,
                    approved_by = Convert.ToInt32(p.is_approved1) == 1 ? string.Format("{0} {1} {2}",
                        p.approval1_emp.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                        p.approval1_emp.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                        p.approval1_emp.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name) :
                        Convert.ToInt32(p.is_approved2) == 1 ? string.Format("{0} {1} {2}",
                        p.approval2_emp.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                        p.approval2_emp.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                        p.approval2_emp.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name) :
                        Convert.ToInt32(p.is_approved3) == 1 ? string.Format("{0} {1} {2}",
                        p.approver3_emp.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                        p.approver3_emp.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                        p.approver3_emp.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name) :
                        Convert.ToInt32(p.is_admin_approve) == 1 ? string.Format("{0} {1} {2}",
                        p.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                        p.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                        p.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name) : "",
                    approved_on = p.is_approved1 == 1 ? p.approver1_dt : p.is_approved2 == 1 ? p.approver2_dt : p.is_approved3 == 1 ? p.approver3_dt : p.is_admin_approve == 1 ? p.admin_ar_date : new DateTime(2000, 01, 01),
                    requ_status = p.is_deleted == 0 ? (p.is_final_approve == 0 ? "Pending" : p.is_final_approve == 1 ? "Accepted" : p.is_final_approve == 2 ? "Rejected" : "") : p.is_deleted == 1 ? "Deleted" : p.is_deleted == 2 ? "Cancel" : "",
                    is_deleted = p.is_deleted,
                    deleted_remarks = p.deleted_remarks,
                    created_date = p.created_date,
                    approver_remarks = string.Concat(p.approval1_remarks ?? "", p.approval2_remarks ?? "", p.approval3_remarks ?? "", p.admin_remarks ?? ""),
                }).Distinct().OrderByDescending(y => y.emp_comp_id).ToList();

                return Ok(result);

            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }

        }
        #endregion **END BY SUPRIYA ON 16-12-2019,COMPOFF RAISED AND APPROVE BEFORE APPLY FOR COMPOFF REQUEST**



        //[Route("LastDayAbsentNotification")]
        //[HttpGet]
        ////[Authorize(Policy = "7138")]
        //public void LastDayAbsentNotification()
        //{
        //    try
        //    {

        //        var get_scheduler_details = _context.tbl_scheduler_details.Where(a => a.scheduler_name == "LastDayAbsentNotification" && a.is_deleted == 0 && a.is_runing == 0 && a.last_runing_date.Value.Date == DateTime.Today.Date.AddDays(-1)).FirstOrDefault();
        //        if (get_scheduler_details != null)
        //        {

        //            if (get_scheduler_details.is_runing != 1)
        //            {
        //                var data = _context.tbl_daily_attendance.Where(a => (a.in_time == Convert.ToDateTime("01/01/2000 00:00") || a.out_time == Convert.ToDateTime("01/01/2000 00:00"))
        //        && a.is_weekly_off == 0
        //        && a.is_holiday == 0
        //        && a.attendance_dt.ToString("dd-MMM-yyyy") == DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy")
        //        && a.tbl_emp_officaial_sec.is_deleted == 0
        //        && a.tbl_emp_officaial_sec.official_email_id != null
        //        )
        //       .Select(a => new
        //       {
        //           employee_first_name = a.tbl_emp_officaial_sec.employee_first_name,
        //           employee_last_name = a.tbl_emp_officaial_sec.employee_last_name == null ? "" : a.tbl_emp_officaial_sec.employee_last_name,
        //           email_id = a.tbl_emp_officaial_sec.official_email_id,
        //           employee_code = a.tbl_employee_master.emp_code,
        //           absent_date = a.attendance_dt.ToString("dd-MMM-yyyy"),
        //           userid = a.tbl_employee_master.tbl_user_master.First(b => b.is_active == 1 && b.employee_id == a.emp_id).user_id,
        //       }).ToList();

        //                if (data.Count > 0)
        //                {
        //                    List<int> _admin_userid = new List<int>();
        //                    var get_roles = _context.tbl_role_menu_master.Select(x => new { x.role_id, x.menu_id }).ToList();
        //                    var menu_list = _context.tbl_menu_master.Where(x => x.is_active == 1).ToList();
        //                    data.ForEach(p =>
        //                    {
        //                        int get_rol_id = _context.tbl_user_role_map.Where(x => x.is_deleted == 0 && x.user_id == p.userid).Select(q => q.role_id).FirstOrDefault() ?? 0;
        //                        var menus_ = get_roles.Where(y => y.role_id == get_rol_id).Select(q => q.menu_id).FirstOrDefault();
        //                        List<string> menu_llist = menus_.Split(',').ToList();


        //                        for (int i = 0; i < menu_llist.Count; i++)
        //                        {
        //                            bool is_admin_superadmin = menu_list.Any(x => (int)x.menu_id == Convert.ToInt32(menu_llist[i]) && (x.menu_name == "Is Company Admin" || x.menu_name == "Display Company List"));

        //                            if (is_admin_superadmin)
        //                            {
        //                                _admin_userid.Add(p.userid);
        //                            }
        //                        }
        //                    });


        //                    data.RemoveAll(x => _admin_userid.Contains(x.userid));

        //                    List<MailDetails> obj_maild = new List<MailDetails>();

        //                    obj_maild = data.Select(TempIndex => new MailDetails
        //                    {
        //                        employee_first_name = TempIndex.employee_first_name,
        //                        employee_last_name = TempIndex.employee_last_name,
        //                        email_id = TempIndex.email_id,
        //                        employee_code = TempIndex.employee_code,
        //                        absent_date = TempIndex.absent_date
        //                    }).ToList();

        //                    MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString);

        //                    Task task = Task.Run(() => obj_ms.LastDayAbsentNotification(obj_maild));


        //                    get_scheduler_details.is_deleted = 1;
        //                    get_scheduler_details.is_runing = 1;
        //                    _context.Entry(get_scheduler_details).State = EntityState.Modified;

        //                    tbl_scheduler_details objshc = new tbl_scheduler_details();
        //                    objshc.scheduler_name = "LastDayAbsentNotification";
        //                    objshc.is_runing = 0;
        //                    objshc.is_deleted = 0;
        //                    objshc.number_of_week = 0;
        //                    objshc.last_runing_date = DateTime.Now;

        //                    _context.Entry(objshc).State = EntityState.Added;
        //                    _context.SaveChanges();

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}


        //[Route("WeeklyAttendanceNotification")]
        //[HttpGet]
        ////[Authorize(Policy = "7139")]
        //public void WeeklyAttendanceNotification()
        //{
        //    //tbl_scheduler_details



        //    try
        //    {
        //        DateTime inputDate = DateTime.Parse(DateTime.Now.Date.ToString());
        //        var d = inputDate;
        //        CultureInfo cul = CultureInfo.CurrentCulture;
        //        int weekNum = cul.Calendar.GetWeekOfYear(d, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

        //        var get_scheduler_details = _context.tbl_scheduler_details.Where(a => a.scheduler_name == "WeeklyAttendanceNotification" && a.is_deleted == 0 && a.is_runing == 0).FirstOrDefault();
        //        if (get_scheduler_details != null)
        //        {
        //            if (get_scheduler_details.number_of_week != weekNum)
        //            {

        //                var data = _context.tbl_daily_attendance.Where(a => (a.attendance_dt >= DateTime.Now.AddDays(-7) && a.attendance_dt <= DateTime.Now)
        //                && a.tbl_emp_officaial_sec.is_deleted == 0
        //                && a.tbl_emp_officaial_sec.official_email_id != null)
        //               .Select(a => new
        //               {
        //                   employee_first_name = a.tbl_emp_officaial_sec.employee_first_name,
        //                   employee_last_name = a.tbl_emp_officaial_sec.employee_last_name == null ? "" : a.tbl_emp_officaial_sec.employee_last_name,
        //                   email_id = a.tbl_emp_officaial_sec.official_email_id,
        //                   employee_code = a.tbl_employee_master.emp_code,
        //                   absent_date = a.attendance_dt.ToString("dd-MMM-yyyy"),
        //                   day_status = a.day_status,
        //                   a.is_weekly_off,
        //                   a.is_holiday,
        //                   userid = a.tbl_employee_master.tbl_user_master.First(b => b.is_active == 1 && b.employee_id == a.emp_id).user_id,
        //                   a.is_comp_off
        //               }).ToList();

        //                if (data.Count > 0)
        //                {

        //                    List<int> _admin_userid = new List<int>();
        //                    var get_roles = _context.tbl_role_menu_master.Select(x => new { x.role_id, x.menu_id }).ToList();
        //                    var menu_list = _context.tbl_menu_master.Where(x => x.is_active == 1).ToList();
        //                    data.ForEach(p =>
        //                    {
        //                        int get_rol_id = _context.tbl_user_role_map.Where(x => x.is_deleted == 0 && x.user_id == p.userid).Select(q => q.role_id).FirstOrDefault() ?? 0;
        //                        var menus_ = get_roles.Where(y => y.role_id == get_rol_id).Select(q => q.menu_id).FirstOrDefault();
        //                        List<string> menu_llist = menus_.Split(',').ToList();


        //                        for (int i = 0; i < menu_llist.Count; i++)
        //                        {
        //                            bool is_admin_superadmin = menu_list.Any(x => (int)x.menu_id == Convert.ToInt32(menu_llist[i]) && (x.menu_name == "Is Company Admin" || x.menu_name == "Display Company List"));

        //                            if (is_admin_superadmin)
        //                            {
        //                                _admin_userid.Add(p.userid);
        //                            }
        //                        }
        //                    });


        //                    data.RemoveAll(x => _admin_userid.Contains(x.userid));

        //                    List<MailDetails> obj_maild = new List<MailDetails>();

        //                    obj_maild = data.Select(TempIndex => new MailDetails
        //                    {
        //                        employee_first_name = TempIndex.employee_first_name,
        //                        employee_last_name = TempIndex.employee_last_name,
        //                        email_id = TempIndex.email_id,
        //                        employee_code = TempIndex.employee_code,
        //                        absent_date = TempIndex.absent_date,
        //                        day_status = TempIndex.is_holiday == 1 ? "Holiday" : TempIndex.is_weekly_off == 1 ? "Week Off" : TempIndex.is_comp_off == 1 ? "Com Off" : TempIndex.day_status == 1 ? "Present" : TempIndex.day_status == 2 ? "Absent" : TempIndex.day_status == 3 ? "On Leave" : TempIndex.day_status == 4 ? "Half Day P/A" : TempIndex.day_status == 5 ? "Half Day P/L" : TempIndex.day_status == 6 ? "Half Day L/A" : "", // add by ravi
        //                    }).ToList();

        //                    MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString);

        //                    Task task = Task.Run(() => obj_ms.WeeklyAttendanceNotification(obj_maild));


        //                    get_scheduler_details.is_deleted = 1;
        //                    get_scheduler_details.is_runing = 1;
        //                    _context.Entry(get_scheduler_details).State = EntityState.Modified;

        //                    tbl_scheduler_details objshc = new tbl_scheduler_details();
        //                    objshc.scheduler_name = "WeeklyAttendanceNotification";
        //                    objshc.is_runing = 0;
        //                    objshc.is_deleted = 0;
        //                    objshc.number_of_week = weekNum;
        //                    objshc.last_runing_date = DateTime.Now;

        //                    _context.Entry(objshc).State = EntityState.Added;
        //                    _context.SaveChanges();


        //                }
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        #region **START BY SUPRIYA ON 27-12-2019, GENERATE QR CODE**
        [Route("GenereateQRCode")]
        [HttpPost]
        //[Authorize(Policy = "7140")]
        public IActionResult GenereateQRCode()
        {
            try
            {
                string _instance_id = _appSettings.Value.instance_id;

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(_instance_id, QRCodeGenerator.ECCLevel.Q);

                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                var bitmapBytes = BitmapToBytes(qrCodeImage);

                return Ok(File(bitmapBytes, "image/png"));
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                return stream.ToArray();

            }
        }
        #endregion ** END BY SUPRIYA ON 27-12-2019,GENERATE QR CODE**


        #region ** START BY SUPRIYA ON 03-01-2020,ASSET MASTER**

        [Route("Save_AssetMaster")]
        [HttpPost]
        //[Authorize(Policy = "7141")]
        public IActionResult Save_AssetMaster(tbl_assets_master objtbl)
        {
            ResponseMsg objresponse = new ResponseMsg();
            try
            {
                var exist_asset = _context.tbl_assets_master.Where(x => ((x.asset_name.Trim().ToUpper() == objtbl.asset_name.Trim().ToUpper() || x.short_name.Trim().ToUpper() == objtbl.asset_name.Trim().ToUpper()) || (x.asset_name == objtbl.short_name.Trim().ToUpper() || x.short_name.Trim().ToUpper() == objtbl.short_name.Trim().ToUpper())) && x.company_id == objtbl.company_id).FirstOrDefault();

                if (exist_asset != null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Asset Already Exist";
                }
                else
                {
                    objtbl.created_dt = DateTime.Now;
                    objtbl.modified_by = 0;
                    objtbl.modified_dt = Convert.ToDateTime("01-Jan-2000");

                    _context.Entry(objtbl).State = EntityState.Added;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Asset Detail Successfully Saved";
                }

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("Get_AssetMaster/{companyid}/{assetmasterid}")]
        [HttpGet]
        //[Authorize(Policy = "7142")]
        public IActionResult Get_AssetMaster(int companyid, int assetmasterid)
        {
            try
            {
                if (assetmasterid > 0)
                {
                    var data = _context.tbl_assets_master.Where(x => x.asset_master_id == assetmasterid).FirstOrDefault();

                    return Ok(data);
                }
                else
                {
                    var data = _context.tbl_assets_master.Where(x => x.company_id == companyid).ToList();
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Update_AssetMaster")]
        [HttpPost]
        //[Authorize(Policy = "7143")]
        public IActionResult Update_AssetMaster(tbl_assets_master objtbl)
        {
            ResponseMsg objresponse = new ResponseMsg();
            try
            {
                var exist_data = _context.tbl_assets_master.Where(x => x.asset_master_id == objtbl.asset_master_id).FirstOrDefault();
                if (exist_data != null)
                {
                    var duplicate = _context.tbl_assets_master.Where(x => x.asset_master_id != objtbl.asset_master_id && x.company_id == objtbl.company_id
                      && (x.asset_name.Trim().ToUpper() == objtbl.asset_name.Trim().ToUpper() || x.short_name.Trim().ToUpper() == objtbl.asset_name.Trim().ToUpper()
                      || x.asset_name.Trim().ToUpper() == objtbl.short_name.Trim().ToUpper() || x.short_name.Trim().ToUpper() == objtbl.short_name.Trim().ToUpper())).FirstOrDefault();

                    if (duplicate != null)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Asset Name or Short Name already exist";
                    }
                    else
                    {
                        exist_data.asset_name = objtbl.asset_name;
                        exist_data.short_name = objtbl.short_name;
                        exist_data.description = objtbl.description;
                        exist_data.is_deleted = objtbl.is_deleted;
                        exist_data.modified_by = objtbl.modified_by;
                        exist_data.modified_dt = DateTime.Now;

                        _context.Entry(exist_data).State = EntityState.Modified;
                        _context.SaveChanges();

                        objresponse.StatusCode = 0;
                        objresponse.Message = "Asset detail successfully updated...";
                    }
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Asset Master";
                }

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        #endregion ** END BY SUPRIYA ON 03-01-2020,ASSET MASTER**


        //get sub Get_GradeMaster by companyid id
        [Route("Get_WorkingRoleList/{companyid}")]
        [HttpGet]
        // //[Authorize(Policy = "7034")]
        public IActionResult Get_WorkingRoleList([FromRoute] int companyid)
        {
            try
            {
                var data = (from a in _context.tbl_working_role.Where(x => x.is_active == 1)
                            select new
                            {
                                a.working_role_id,
                                a.working_role_name
                            }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPaySlip/{emp_id}/{month_year}")]
        public IActionResult GetPaySlip(int emp_id, int month_year)
        {


            //var get_employee_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == emp_id && a.is_deleted == 0).Select(a => new
            //{
            //    a.employee_first_name,
            //    a.employee_middle_name,
            //    a.employee_last_name,
            //    a.official_email_id,
            //    a.tbl_employee_id_details.emp_code,
            //    emp_grade = _context.tbl_emp_grade_allocation.Where(b => b.employee_id == a.employee_id && a.is_deleted == 0).Select(b => b.tbl_grade_master.grade_name).FirstOrDefault(),
            //    a.date_of_joining,
            //    a.tbl_department_master.department_name,
            //    designation_name = _context.tbl_emp_desi_allocation.Where(c => c.employee_id == a.employee_id && a.is_deleted == 0).Select(c => c.tbl_designation_master.designation_name).FirstOrDefault(),
            //    emp_pan_number = _context.tbl_emp_personal_sec.Where(d => d.employee_id == a.employee_id && a.is_deleted == 0).Select(d => d.pan_card_number).FirstOrDefault(),
            //}).FirstOrDefault();

            //string year = month_year.ToString().Substring(0, 4);
            //string month = month_year.ToString().Substring(4, 2);
            //string payroll_date = year + "-" + month + "-01";


            //int days_in_month = DateTime.DaysInMonth(Convert.ToInt32(month_year.ToString().Substring(0, 4)), Convert.ToInt32(month_year.ToString().Substring(4, 2)));

            ////double prsnt_count_in_mnth = cls_payroll.func_prsnt_count_in_mnth();

            ////double absnt_count_in_mnth = cls_payroll.func_absnt_count_in_mnth();

            //int company_id = _context.tbl_user_master.Where(a => a.employee_id == emp_id && a.is_active == 1).Select(b => b.default_company_id).FirstOrDefault();

            //var company_info = _context.tbl_company_master.Where(a => a.company_id == company_id && a.is_active == 1).Select(a => new { a.company_name, a.company_logo }).FirstOrDefault();

            //int compo_id = _context.tbl_component_master.Where(a => a.component_name == "@Salary_sys" && a.is_active == 1).Select(b => b.component_id).FirstOrDefault();

            //string employee_monthly_slary = _context.tbl_salary_input.Where(a => a.monthyear == month_year && a.is_active == 1 && a.emp_id == emp_id && a.component_id == compo_id).Select(b => b.values).FirstOrDefault();



            //string path = _hostingEnvironment.WebRootPath + "/" + company_info.company_logo;
            //byte[] byt = System.IO.File.ReadAllBytes(path);
            //var company_logo = "data:image/png;base64," + Convert.ToBase64String(byt);


            //var obj_incom_comp = _context.tbl_salary_input.Where(a => a.is_active == 1 && a.monthyear == month_year && a.emp_id == emp_id && a.tcm.is_payslip == 1 && a.values != "0").Select(b => new { component_value = b.values, component_name = b.tcm.property_details }).ToList();


            //List<IncomeComponent> obj_incom_comp_ = new List<IncomeComponent>();
            //for (int Index = 0; Index < obj_incom_comp.Count; Index++)
            //{
            //    IncomeComponent obj_inc = new IncomeComponent();
            //    obj_inc.component_name = obj_incom_comp[Index].component_name;
            //    obj_inc.component_value = obj_incom_comp[Index].component_value;
            //    obj_incom_comp_.Add(obj_inc);
            //}


            //string employee_name = String.Format("{0} {1} {2}", get_employee_data.employee_first_name, get_employee_data.employee_middle_name, get_employee_data.employee_last_name);

            PayslipGenerator payslipGenerator = new PayslipGenerator();

            var objectSettings = new ObjectSettings
            {
                HtmlContent = payslipGenerator.GetPayrollHTMLString(emp_id, month_year, _hostingEnvironment.WebRootPath), //payslipGenerator.GetHTMLString(emp_id, month_year, _hostingEnvironment.WebRootPath),
            };


            return Ok(new
            {
                pdf_data = objectSettings.HtmlContent
            });
        }


        #region **START BY SUPIRYA ON 10-02-2020, BANK MASTER**


        [Route("Get_BankMaster/{bank_id}")]
        [HttpGet]
        public IActionResult Get_BankMaster(int bank_id)
        {
            if (bank_id > 0)
            {
                var data = _context.tbl_bank_master.Where(x => x.bank_id == bank_id && x.is_deleted == 0).FirstOrDefault();
                return Ok(data);
            }
            else
            {
                var data = _context.tbl_bank_master.Where(x => x.is_deleted == 0).Select(p => new
                {
                    p.bank_name,
                    p.bank_id,
                    bank_status = p.bank_status == 1 ? "Active" : p.bank_status == 0 ? "In Active" : "",
                    p.created_dt,
                    p.modified_dt
                }).ToList();

                return Ok(data);
            }
        }

        [Route("Save_BankMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.BankMaster))]
        public IActionResult Save_BankMaster([FromBody] tbl_bank_master objtbl)
        {
            Response_Msg objresponse = new Response_Msg();
            try
            {
                var exist = _context.tbl_bank_master.Where(x => x.bank_name.Trim().ToUpper() == objtbl.bank_name.Trim().ToUpper() && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Bank Name already exist";
                }
                else
                {
                    objtbl.is_deleted = 0;
                    objtbl.deleted_by = 0;
                    objtbl.created_dt = DateTime.Now;
                    objtbl.modified_by = 0;
                    objtbl.modified_dt = Convert.ToDateTime("01-01-2000");

                    _context.Entry(objtbl).State = EntityState.Added;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Bank name successfully saved...";
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
            }
            return Ok(objresponse);
        }

        [Route("Update_BankMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.BankMaster))]
        public IActionResult Update_BankMaster([FromBody] tbl_bank_master objtbl)
        {
            Response_Msg objresponse = new Response_Msg();
            try
            {
                var exist = _context.tbl_bank_master.Where(x => x.bank_id == objtbl.bank_id && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    var duplicate = _context.tbl_bank_master.Where(x => x.bank_id != objtbl.bank_id && x.is_deleted == 0 && x.bank_name.Trim().ToUpper() == objtbl.bank_name.Trim().ToUpper()).FirstOrDefault();
                    if (duplicate != null)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Bank master already exist";
                    }
                    else
                    {
                        using (var trans = _context.Database.BeginTransaction())
                        {

                            try
                            {

                                exist.is_deleted = 1;
                                exist.modified_by = objtbl.created_by;
                                exist.modified_dt = DateTime.Now;

                                _context.Entry(exist).State = EntityState.Modified;


                                tbl_bank_master objbank = new tbl_bank_master();
                                objbank.bank_name = objtbl.bank_name.Trim();
                                objbank.bank_status = objtbl.bank_status;
                                objbank.is_deleted = 0;
                                objbank.deleted_by = 0;
                                objbank.created_by = exist.created_by;
                                objbank.created_dt = exist.created_dt;
                                objbank.modified_by = objtbl.created_by;
                                objbank.modified_dt = DateTime.Now;


                                _context.Entry(objbank).State = EntityState.Added;
                                //_context.SaveChanges();

                                //int new_bank_id = _context.tbl_bank_master.FirstOrDefault(x => x.bank_name.Trim().ToUpper() == objtbl.bank_name.Trim().ToUpper()).bank_id;

                                var bank_details = _context.tbl_emp_bank_details.Where(x => x.is_deleted == 0 && x.bank_id == exist.bank_id).ToList();
                                bank_details.ForEach(p => p.bank_id = objbank.bank_id);

                                _context.tbl_emp_bank_details.UpdateRange(bank_details);


                                var pf_dtl = _context.tbl_emp_pf_details.Where(x => x.is_deleted == 0 && x.bank_id == exist.bank_id).ToList();
                                pf_dtl.ForEach(p => p.bank_id = objbank.bank_id);

                                _context.tbl_emp_pf_details.UpdateRange(pf_dtl);

                                _context.SaveChanges();
                                trans.Commit();
                                objresponse.StatusCode = 0;
                                objresponse.Message = "Bank master successfully updated";
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                objresponse.StatusCode = 1;
                                objresponse.Message = ex.Message;
                            }
                        }

                    }
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Detail";
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
            }

            return Ok(objresponse);
        }
        #endregion ** END BY SUPRIYA ON 10-02-2020,BANK MASTER**


        [HttpGet("Get_EducationLevel")]
        public IActionResult Get_EducationLevel()
        {
            try
            {
                List<object> list = new List<object>();
                foreach (EducationLevel educationLevel in Enum.GetValues(typeof(EducationLevel)))
                {
                    int value = (int)Enum.Parse(typeof(EducationLevel), Enum.GetName(typeof(EducationLevel), educationLevel));

                    Type type = educationLevel.GetType();
                    MemberInfo[] memInfo = type.GetMember(educationLevel.ToString());
                    if (memInfo != null && memInfo.Length > 0)
                    {
                        object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (attrs != null && attrs.Length > 0)
                        {
                            string strvalue = ((DescriptionAttribute)attrs[0]).Description;

                            list.Add(new
                            {

                                edu_level_id = value,
                                edu_level_name = strvalue
                            });
                        }
                    }
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Get_OnlyManagersEmpCode/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Employee))]
        public IActionResult Get_OnlyManagersEmpCode([FromRoute]int company_id)
        {
            Response_Msg objresponse = new Response_Msg();
            try
            {
                var data = _clsEmployeeDetail.GetEmployeeByDate(company_id, new DateTime(2000, 01, 01), DateTime.Now.AddDays(1));

                List<int> _empid = data.Select(p => p.employee_id).ToList();

                var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && x.user_type == 11 && _empid.Contains(x.employee_id ?? 0)).Select(p => new
                {
                    p.employee_id,
                    emp_name = data.FirstOrDefault(q => q.employee_id == p.employee_id).emp_name
                }).ToList();


                List<EmployeeManager> mgrlist = teos.Select(p => new EmployeeManager
                {
                    emp_mgr_id = p.employee_id ?? 0,
                    manager_name_code = p.emp_name,
                }).ToList();

                return Ok(mgrlist);
                //var data = _context.tbl_emp_manager.Select(p => new
                //{
                //    mgr1_id = p.m_one_id,
                //    mgr1_name_code = string.Format("{0} {1} {2} ({3})", p.tbl_employee_master1.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                //    p.tbl_employee_master1.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                //    p.tbl_employee_master1.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name,
                //    p.tbl_employee_master1.emp_code),
                //    mgr1_role = _context.tbl_user_role_map.FirstOrDefault(x => x.is_deleted == 0 && x.user_id == p.tbl_employee_master1.tbl_user_master.FirstOrDefault(y => y.is_active == 1 && y.default_company_id == company_id && y.employee_id == p.m_one_id).user_id).role_id,

                //    mgr2_id = p.m_two_id,
                //    mgr2_name_code = string.Format("{0} {1} {2} ({3})", p.tbl_employee_master2.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                //    p.tbl_employee_master2.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                //    p.tbl_employee_master2.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name,
                //    p.tbl_employee_master2.emp_code),
                //    mgr2_role = _context.tbl_user_role_map.FirstOrDefault(x => x.is_deleted == 0 && x.user_id == p.tbl_employee_master2.tbl_user_master.FirstOrDefault(y => y.is_active == 1 && y.default_company_id == company_id && y.employee_id == p.m_two_id).user_id).role_id,


                //    mgr3_id = p.m_three_id,
                //    mgr_3_name_code = string.Format("{0} {1} {2} ({3})", p.tbl_employee_master3.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                //    p.tbl_employee_master3.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                //    p.tbl_employee_master3.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name,
                //    p.tbl_employee_master3.emp_code),
                //    mgr3_role = _context.tbl_user_role_map.FirstOrDefault(x => x.is_deleted == 0 && x.user_id == p.tbl_employee_master3.tbl_user_master.FirstOrDefault(y => y.is_active == 1 && y.default_company_id == company_id && y.employee_id == p.m_three_id).user_id).role_id,

                //    p.is_deleted,
                //    mgr1_company_id = p.tbl_employee_master1.tbl_employee_company_map.FirstOrDefault(x => x.is_deleted == 0).company_id,
                //    mgr2_company_id = p.tbl_employee_master2.tbl_employee_company_map.FirstOrDefault(x => x.is_deleted == 0).company_id,
                //    mgr3_company_id = p.tbl_employee_master3.tbl_employee_company_map.FirstOrDefault(x => x.is_deleted == 0).company_id,

                //}).Where(x => x.is_deleted == 0).ToList();

                //List<EmployeeManager> mgrlist = new List<EmployeeManager>();

                //var _mgr1_data = data.Where(x => x.mgr1_company_id == company_id && x.mgr1_role == 6).Select(p => new { p.mgr1_id, p.mgr1_name_code }).ToList(); // company id match and its a user

                //for (int i = 0; i < _mgr1_data.Count; i++)
                //{
                //    EmployeeManager objmgr = new EmployeeManager();

                //    bool containsItem = mgrlist.Any(item => item.emp_mgr_id == _mgr1_data[i].mgr1_id);

                //    if (containsItem == false)
                //    {
                //        if (_mgr1_data[i].mgr1_id != null && _mgr1_data[i].mgr1_id != 0)
                //        {
                //            objmgr.emp_mgr_id = _mgr1_data[i].mgr1_id ?? 0;
                //            objmgr.manager_name_code = _mgr1_data[i].mgr1_name_code;


                //            mgrlist.Add(objmgr);
                //        }
                //    }

                //}
                //var _mgr2_data = data.Where(x => x.mgr2_company_id == company_id && x.mgr2_role == 6).Select(p => new { p.mgr2_id, p.mgr2_name_code }).ToList();

                //for (int i = 0; i < _mgr2_data.Count; i++)
                //{
                //    EmployeeManager objmgr = new EmployeeManager();
                //    bool containsItem = mgrlist.Any(item => item.emp_mgr_id == _mgr2_data[i].mgr2_id);
                //    if (containsItem == false)
                //    {
                //        if (_mgr2_data[i].mgr2_id != null && _mgr2_data[i].mgr2_id != 0)
                //        {
                //            objmgr.emp_mgr_id = _mgr2_data[i].mgr2_id ?? 0;
                //            objmgr.manager_name_code = _mgr2_data[i].mgr2_name_code;


                //            mgrlist.Add(objmgr);
                //        }
                //    }

                //}
                //var _mgr3_data = data.Where(x => x.mgr3_company_id == company_id && x.mgr3_role == 6).Select(p => new { p.mgr3_id, p.mgr_3_name_code }).ToList();

                //for (int i = 0; i < _mgr3_data.Count; i++)
                //{
                //    EmployeeManager objmgr = new EmployeeManager();

                //    bool containsItem = mgrlist.Any(item => item.emp_mgr_id == _mgr3_data[i].mgr3_id);

                //    if (containsItem == false)
                //    {
                //        if (_mgr3_data[i].mgr3_id != null && _mgr3_data[i].mgr3_id != 0)
                //        {
                //            objmgr.emp_mgr_id = _mgr3_data[i].mgr3_id ?? 0;
                //            objmgr.manager_name_code = _mgr3_data[i].mgr_3_name_code;


                //            mgrlist.Add(objmgr);
                //        }
                //    }

                //}

                // return Ok(mgrlist);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
            }
            return Ok(objresponse);
        }


        [Route("GetResignReasons/{company_id}")]
        [HttpGet]
        public IActionResult GetResignReasons(int company_id)
        {
            try
            {
                var data = _config.GetSection("UserResignReason:Resign_reason").Get<List<string>>();

                List<object> _reason_lst = new List<object>();
                for (int i = 1; i < data.Count; i++)
                {
                    _reason_lst.Add(new
                    {
                        reason_id = i,
                        reason_ = data[i]
                    });
                }

                return Ok(_reason_lst);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        //get company list 
        [Route("Get_LocationForReport/{company_id}")]
        [Authorize(Policy = nameof(enmMenuMaster.EarlyPunchout))]
        public IActionResult Get_LocationForReport(int company_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }
            try
            {
                var data = _context.tbl_location_master.Where(a => a.is_active == 1 && a.company_id == company_id).Select(a => new
                {
                    state_id = a.state_id,
                    name = a.state_id != null && a.state_id != 0 ? a.tbl_state.name : "",
                }).Distinct().ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("GetWithdrawalType/{company_id}")]
        [HttpGet]
        public IActionResult GetWithdrawalType(int company_id)
        {
            try
            {
                List<string> _withdrawal_lst = _config.GetSection("UserResignReason:WithdrawalType").Get<List<string>>();

                return Ok(_withdrawal_lst);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //Save MobileApplicationAccessMaster
        [Route("Save_MobileApplicationAccessMaster")]
        [HttpPost]
        //[Authorize(Policy = nameof(enmMenuMaster.CompoffApproval))]
        public async Task<IActionResult> Save_MobileApplicationAccessMaster([FromBody] EmployeeMobileApplicationAccess objmodel)
        {
            Response_Msg objResult = new Response_Msg();


            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var temp = _context.tbl_user_master.Join(_context.tbl_company_master, c => c.default_company_id, d => d.company_id, (c, d) => new
                        {
                            c.employee_id,
                            c.default_company_id
                        }).Where(z => z.default_company_id == objmodel.company_id).Distinct().ToList();

                        var empDetails = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && temp.Select(y => y.employee_id).Contains(x.employee_id)).ToList();
                        if (empDetails.Count > 0)
                        {
                            empDetails.ForEach(p => { p.is_mobile_access = 0; p.is_mobile_attendence_access = 0; p.last_modified_by = _clsCurrentUser.EmpId; p.last_modified_date = DateTime.Now; });
                        }
                        _context.tbl_emp_officaial_sec.UpdateRange(empDetails);

                        //Updating mobile access for employees
                        empDetails = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && objmodel.mobile_access_employee_ids.Contains(Convert.ToInt32(x.employee_id))).ToList();
                        if (empDetails.Count > 0)
                        {
                            empDetails.ForEach(p => { p.is_mobile_access = 1; p.last_modified_by = _clsCurrentUser.EmpId; p.last_modified_date = DateTime.Now; });
                        }
                        _context.tbl_emp_officaial_sec.UpdateRange(empDetails);

                        //Updating mobile attendence access for employees
                        empDetails = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && objmodel.mobile_attendence_access_employee_ids.Contains(Convert.ToInt32(x.employee_id))).ToList();
                        if (empDetails.Count > 0)
                        {
                            empDetails.ForEach(p => { p.is_mobile_attendence_access = 1; p.last_modified_by = _clsCurrentUser.EmpId; p.last_modified_date = DateTime.Now; });
                        }
                        _context.tbl_emp_officaial_sec.UpdateRange(empDetails);

                        _context.SaveChanges();
                        trans.Commit();


                        objResult.StatusCode = 0;
                        objResult.Message = "Submited Successfully...!";
                        return Ok(objResult);

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
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        [Route("Get_MobileApplicationAccess")]
        [HttpGet]
        public IActionResult Get_MobileApplicationAccess()
        {
            try
            {
                var temp = _context.tbl_user_master.Join(_context.tbl_company_master, c => c.default_company_id, d => d.company_id, (c, d) => new
                {
                    c.employee_id,
                    c.default_company_id,
                    d.company_name
                }).Distinct().ToList();


                var data = _context.tbl_emp_officaial_sec.Join(temp, i => i.employee_id, j => j.employee_id, (i, j) => new
                {
                    i.is_deleted,
                    i.tbl_employee_id_details,
                    j.default_company_id,
                    j.employee_id,
                    i.is_mobile_access,
                    i.is_mobile_attendence_access,
                    i.employee_first_name,
                    i.employee_middle_name,
                    i.employee_last_name,
                    i.department_id,
                    i.location_id
                }).Where(b => b.is_deleted == 0 && ((_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin) ? b.tbl_employee_id_details.is_active == 1 : true))
                    .Select(a => new { a.tbl_employee_id_details.emp_code, a.employee_id, a.employee_first_name, a.employee_middle_name, a.employee_last_name, a.is_mobile_access, a.is_mobile_attendence_access, a.default_company_id, a.department_id, a.location_id })
                    .Select(j => new
                    {
                        j.employee_id,
                        emp_code = j.emp_code,
                        emp_name = string.Format("{0} {1} {2}", j.employee_first_name, j.employee_middle_name, j.employee_last_name),
                        mobile_access = j.is_mobile_access == 1 ? "YES" : "NO",
                        mobile_attendence_access = j.is_mobile_attendence_access == 1 ? "YES" : "NO",
                        company = _context.tbl_company_master.Where(x => x.company_id == j.default_company_id && x.is_active == 1).FirstOrDefault().company_name,
                        department = _context.tbl_department_master.Where(x => x.department_id == j.department_id && x.is_active == 1).FirstOrDefault().department_name,
                        location = _context.tbl_location_master.Where(x => x.location_id == j.location_id && x.is_active == 1).FirstOrDefault().location_name,
                    }).OrderBy(z => z.emp_name).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        protected string FunIsApplicationFreezed()
        {
            Dictionary<string, string> app_setting_dic = _context.tbl_app_setting.Where(x => x.is_active == 1).ToDictionary(x => x.AppSettingKey.ToString(), y => y.AppSettingValue.ToString());
            string is_attendence_freezed_for_Emp = app_setting_dic.Where(x => x.Key.ToLower() == "attandance_application_freezed_for_emp").Select(y => y.Value).FirstOrDefault().ToString();
            string is_attendence_freezed_for_Admin = app_setting_dic.Where(x => x.Key.ToLower() == "attandance_application_freezed__for_admin").Select(y => y.Value).FirstOrDefault().ToString();
            bool is_Admin = _clsCurrentUser.RoleId.Contains(1);

            if (is_Admin)
            {
                if (is_attendence_freezed_for_Admin.ToLower() == "true" || is_attendence_freezed_for_Admin.ToLower() == "yes") return "true";
                else return "false";
            }
            else
            {
                if (is_attendence_freezed_for_Emp.ToLower() == "true" || is_attendence_freezed_for_Emp.ToLower() == "yes") return "true";
                else return "false";
            }
        }

        [Route("DeleteCompoff/{id}")]
        [HttpPost]
        //[Authorize(Policy = nameof(enmMenuMaster.AttendanceApplication))]
        public IActionResult DeleteCompoff([FromRoute] int id)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {
                var exist = _context.tbl_comp_off_ledger.Where(x => x.sno == id).FirstOrDefault();
                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid request id, please check and try !!";
                    return Ok(objResult);
                }

                _context.tbl_comp_off_ledger.Remove(exist);
                _context.SaveChanges();

                objResult.StatusCode = 0;
                objResult.Message = "Compoff Deleted Successfully !!";
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        [Route("DeleteCompoff")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffCreditApplication))]
        public IActionResult DeleteCompoff([FromBody] CompOffDetail objcompoffdtl)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {

                        for (int i = 0; i < objcompoffdtl.compoff_id.Count(); i++)
                        {

                            var exist = _context.tbl_comp_off_ledger.Where(x => x.sno == objcompoffdtl.compoff_id[i]).FirstOrDefault();
                            if (exist == null)
                            {
                                objResult.StatusCode = 1;
                                objResult.Message = "Invalid request id, please check and try !!";
                                return Ok(objResult);
                            }
                            else
                            {
                                exist.is_deleted = 1;
                                exist.deleted_by = _clsCurrentUser.EmpId;
                                _context.tbl_comp_off_ledger.UpdateRange(exist);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        objResult.StatusCode = 2;
                        objResult.Message = ex.Message;
                        return Ok(objResult);
                    }
                    _context.SaveChanges();
                    trans.Commit();
                }

                objResult.StatusCode = 0;
                objResult.Message = "Compoff Deleted Successfully !!";
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        [HttpGet]
        [Route("GetFNFSlip/{emp_id}/{monthyear}")]
        public IActionResult GetFNFSlip(int emp_id, int monthyear)
        {
            PayslipGenerator payslipGenerator = new PayslipGenerator();

            var objectSettings = new ObjectSettings
            {
                HtmlContent = payslipGenerator.GetFNFHTMLString(emp_id, monthyear, _hostingEnvironment.WebRootPath), //payslipGenerator.GetHTMLString(emp_id, month_year, _hostingEnvironment.WebRootPath),
            };

            return Ok(new
            {
                pdf_data = objectSettings.HtmlContent
            });
        }
    }
}
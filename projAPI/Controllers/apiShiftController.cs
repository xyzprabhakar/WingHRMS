using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

namespace projAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class apiShiftController : ControllerBase
    {
        private readonly Context _context;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IHttpContextAccessor _AC;
        private IHostingEnvironment _hostingEnvironment;
        private readonly clsEmployeeDetail _clsEmployeeDetail;
        private readonly clsCurrentUser _clsCurrentUser;
        private readonly IConfiguration _config;

        public apiShiftController(Context context, IOptions<AppSettings> appSettings, IHostingEnvironment environment, IHttpContextAccessor AC, clsEmployeeDetail _clsEmployeeDetail, clsCurrentUser _clsCurrentUser, IConfiguration config)
        {
            _context = context;
            _appSettings = appSettings;
            _hostingEnvironment = environment;
            _AC = AC;
            this._clsEmployeeDetail = _clsEmployeeDetail;
            this._clsCurrentUser = _clsCurrentUser;
            _config = config;
        }

        // GET: api/apiShift
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.ShiftReport))]
        public async Task<IActionResult> Gettbl_shift_details(DateTime fromdate, DateTime todate)
        {


            ResponseMsg objResult = new ResponseMsg();

            var result = from sm in _context.tbl_shift_master
                         join sd in _context.tbl_shift_details on sm.shift_id equals sd.shift_id
                         where sd.is_deleted == 0
                         select new
                         {
                             shift_id = sd.shift_details_id,
                             shift_code = sm.shift_code,
                             shift_name = sd.shift_name,
                             shift_short_name = sd.shift_short_name,
                             punch_in_time = sd.punch_in_time,
                             punch_in_max_time = sd.punch_in_max_time,
                             punch_out_time = sd.punch_out_time,
                             maximum_working_hours = sd.maximum_working_hours,
                             grace_time_for_late_punch_in = sd.grace_time_for_late_punch,
                             grace_time_for_late_punch_out = sd.grace_time_for_late_punch,
                             number_of_grace_time_applicable_in_month = sd.number_of_grace_time_applicable_in_month,
                             is_lunch_punch_applicable = sd.is_lunch_punch_applicable,
                             lunch_punch_out_time = sd.lunch_punch_out_time,
                             lunch_punch_in_time = sd.lunch_punch_in_time,
                             maximum_lunch_time = sd.maximum_lunch_time,
                             is_ot_applicable = sd.is_ot_applicable,
                             maximum_ot_hours = sd.maximum_ot_hours,
                             is_default = sm.is_default == 1 ? "Yes" : "No",
                             shift_mstr_id = sm.shift_id

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


        [Route("Gettbl_shift_detailsByCompanyId/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.ShiftReport))]
        public async Task<IActionResult> Gettbl_shift_detailsByCompanyId([FromRoute] int company_id)
        {
            ResponseMsg objResult = new ResponseMsg();

            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }

            var result = from sm in _context.tbl_shift_master
                         join sd in _context.tbl_shift_details on sm.shift_id equals sd.shift_id
                         join sl in _context.tbl_shift_location on sd.shift_details_id equals sl.shift_detail_id
                         where sd.is_deleted == 0 && sl.company_id == company_id
                         select new
                         {
                             shift_id = sd.shift_details_id,
                             shift_code = sm.shift_code,
                             shift_name = sd.shift_name,
                             shift_short_name = sd.shift_short_name,
                             punch_in_time = sd.punch_in_time,
                             punch_in_max_time = sd.punch_in_max_time,
                             punch_out_time = sd.punch_out_time,
                             maximum_working_hours = sd.maximum_working_hours,
                             grace_time_for_late_punch_in = sd.grace_time_for_late_punch,
                             grace_time_for_late_punch_out = sd.grace_time_for_late_punch,
                             number_of_grace_time_applicable_in_month = sd.number_of_grace_time_applicable_in_month,
                             is_lunch_punch_applicable = sd.is_lunch_punch_applicable,
                             lunch_punch_out_time = sd.lunch_punch_out_time,
                             lunch_punch_in_time = sd.lunch_punch_in_time,
                             maximum_lunch_time = sd.maximum_lunch_time,
                             is_ot_applicable = sd.is_ot_applicable,
                             maximum_ot_hours = sd.maximum_ot_hours,
                             is_default = sm.is_default == 1 ? "Yes" : "No"

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
        // GET: api/apiShift/5
        [HttpGet("{id}")]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.ShiftReport))]
        public async Task<IActionResult> Gettbl_shift_details([FromRoute] int id)
        {
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        ShiftDetails objResult = new ShiftDetails();
                        // Get data from shift details table
                        var ShiftDetails = _context.tbl_shift_details.FirstOrDefault(p => p.shift_details_id == id && p.is_deleted == 0);
                        if (ShiftDetails == null)
                        {
                            throw new Exception("Record Not Found...!");
                        }

                        //Get Shift Detail Id
                        int shift_detail_id = ShiftDetails.shift_details_id;

                        //Get Shift Detail Id
                        int shift_id = Convert.ToInt32(ShiftDetails.shift_id);

                        // Get shift master data
                        var ShiftMaster = _context.tbl_shift_master.FirstOrDefault(p => p.shift_id == shift_id);


                        // Get Data From  Shift Week off 
                        List<tbl_shift_week_off> ShiftWeekOff_ = _context.tbl_shift_week_off.Where(p => p.shift_detail_id == shift_detail_id && p.is_active == 1).ToList();

                        //Get Shift Location Id
                        var ShiftLocation = _context.tbl_shift_location.FirstOrDefault(a => a.shift_detail_id == shift_detail_id && a.is_deleted == 0);

                        //Get Shift Dept Id
                        var ShiftDept = _context.tbl_shift_department.FirstOrDefault(a => a.shift_detail_id == shift_detail_id && a.is_deleted == 0);


                        List<ShiftWeekOff> sWO = new List<ShiftWeekOff>();

                        //loop and insert records
                        foreach (tbl_shift_week_off shiftLoc in ShiftWeekOff_)
                        {
                            sWO.Add(new ShiftWeekOff
                            {
                                week_day = shiftLoc.week_day,
                                days = shiftLoc.days
                            });
                        }


                        //Bind Data in ShiftDetail model
                        objResult.shift_for_all_company = ShiftDetails.shift_for_all_company;
                        objResult.shift_for_all_location = ShiftDetails.shift_for_all_location; // Convert.ToByte(ShiftLocation.location_id);
                        objResult.shift_for_all_department = ShiftDetails.shift_for_all_department;//

                        if (ShiftLocation != null)
                        {
                            objResult.company_id = Convert.ToInt32(ShiftLocation.company_id);
                            objResult.location_id = Convert.ToInt32(ShiftLocation.location_id);

                        }
                        else
                        {
                            objResult.company_id = 0;
                            objResult.location_id = 0;
                        }

                        if (ShiftDept != null)
                        {
                            objResult.department_id = Convert.ToInt32(ShiftDept.dept_id);
                        }
                        else
                        {
                            objResult.department_id = 0;
                        }
                        objResult.shift_id = shift_id;
                        objResult.shift_type = ShiftDetails.shift_type;
                        objResult.shift_details_id = ShiftDetails.shift_details_id;
                        objResult.shift_code = ShiftMaster.shift_code;
                        objResult.shift_name = ShiftDetails.shift_name;
                        objResult.shift_short_name = ShiftDetails.shift_short_name;
                        objResult.punch_in_time = ShiftDetails.punch_in_time;
                        objResult.punch_in_max_time = ShiftDetails.punch_in_max_time;
                        objResult.punch_out_time = ShiftDetails.punch_out_time;
                        objResult.maximum_working_hours = ShiftDetails.maximum_working_hours;
                        objResult.maximum_working_minute = ShiftDetails.maximum_working_minute;
                        objResult.grace_time_for_late_punch_in = ShiftDetails.grace_time_for_late_punch;
                        objResult.grace_time_for_late_punch_out = ShiftDetails.grace_time_for_late_punch;
                        objResult.number_of_grace_time_applicable_in_month = ShiftDetails.number_of_grace_time_applicable_in_month;
                        objResult.is_lunch_punch_applicable = ShiftDetails.is_lunch_punch_applicable;
                        objResult.lunch_punch_out_time = ShiftDetails.lunch_punch_out_time;
                        objResult.lunch_punch_in_time = ShiftDetails.lunch_punch_in_time;
                        objResult.maximum_lunch_time = ShiftDetails.maximum_lunch_time;
                        objResult.tea_punch_applicable_one = ShiftDetails.tea_punch_applicable_one;
                        objResult.tea_punch_in_time_one = ShiftDetails.tea_punch_in_time_one;
                        objResult.tea_punch_out_time_one = ShiftDetails.tea_punch_out_time_one;
                        objResult.maximum_tea_time_one = ShiftDetails.maximum_tea_time_one;
                        objResult.tea_punch_applicable_two = ShiftDetails.tea_punch_applicable_two;
                        objResult.tea_punch_in_time_two = ShiftDetails.tea_punch_in_time_two;
                        objResult.tea_punch_out_time_two = ShiftDetails.tea_punch_out_time_two;
                        objResult.maximum_tea_time_two = ShiftDetails.maximum_tea_time_two;
                        objResult.is_ot_applicable = ShiftDetails.is_ot_applicable;
                        objResult.min_halfday_working_hour = ShiftDetails.min_halfday_working_hour;
                        // objResult.maximum_ot_hours = ShiftDetails.maximum_ot_hours;
                        objResult.is_night_shift = ShiftDetails.is_night_shift;
                        //objResult.is_punch_ignore = ShiftDetails.is_punch_ignore;
                        objResult.weekly_off = ShiftDetails.weekly_off;
                        objResult.ShiftWeekOff = sWO;
                        objResult.is_default = ShiftMaster.is_default;
                        trans.Commit();
                        return Ok(objResult);

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return Ok(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }
        // Obj Of Shift Details Model



        [HttpPost("{id}")]
        [Authorize(Policy = nameof(enmMenuMaster.CreateShift))]
        public async Task<IActionResult> Puttbl_shift_details([FromRoute] int id, [FromBody]  ShiftDetails ShiftModel)
        {
            ResponseMsg objResult = new ResponseMsg();

            try
            {

                //Create new obj of tbl_shift_details table for log
                tbl_shift_details shift_details_for_log = _context.tbl_shift_details.FirstOrDefault(a => a.shift_details_id == id);


                //Create new obj of tbl_shift_details table
                tbl_shift_details shift_details = new tbl_shift_details();
                // Check Shift Name
                string ShiftName = ShiftModel.shift_name;

                // Check Shift Name
                if (shift_details.shift_name == ShiftName && shift_details.shift_details_id == id)
                {
                    objResult.Message = "Shift Name Already Exist For This Company...!";
                    objResult.StatusCode = 0;
                    return Ok(objResult);
                }
                else
                {
                    using (var Tran = _context.Database.BeginTransaction())
                    {
                        try
                        {

                            if (Convert.ToByte(ShiftModel.is_default) == 1)
                            {
                                List<tbl_shift_master> tsms = _context.tbl_shift_master.Where(p => p.is_default == 1).ToList();
                                tsms.ForEach(p => { p.is_default = 0; });
                                _context.tbl_shift_master.UpdateRange(tsms);
                                await _context.SaveChangesAsync();
                                tbl_shift_master tsm = _context.tbl_shift_master.FirstOrDefault(p => p.shift_id == shift_details_for_log.shift_id);
                                tsm.is_default = 1;
                                _context.tbl_shift_master.Update(tsm);
                                await _context.SaveChangesAsync();
                            }




                            shift_details_for_log.is_deleted = 1;
                            _context.tbl_shift_details.Attach(shift_details_for_log);
                            _context.Entry(shift_details_for_log).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                            shift_details.shift_id = shift_details_for_log.shift_id;
                            //shift_details.shift_for_all_location = ShiftModel.shift_for_all_location;
                            //shift_details.shift_for_all_company = ShiftModel.shift_for_all_company;
                            //shift_details.shift_for_all_department = ShiftModel.shift_for_all_department;

                            if (ShiftModel.shift_for_all_company == 0)
                            {
                                shift_details.shift_for_all_location = 1;
                                shift_details.shift_for_all_company = 1;
                            }
                            else if (ShiftModel.shift_for_all_company > 0 && ShiftModel.shift_for_all_location == 0)
                            {
                                shift_details.shift_for_all_location = 1;
                                shift_details.shift_for_all_company = 0;

                            }
                            else
                            {
                                shift_details.shift_for_all_location = 0;
                                shift_details.shift_for_all_company = 0;
                            }
                            if (ShiftModel.shift_for_all_department == 0)
                            {
                                shift_details.shift_for_all_department = 1;
                            }
                            else
                            {
                                shift_details.shift_for_all_department = 0;
                            }




                            shift_details.shift_type = ShiftModel.shift_type;
                            shift_details.shift_name = ShiftModel.shift_name;
                            shift_details.shift_short_name = ShiftModel.shift_short_name;
                            shift_details.punch_in_time = ShiftModel.punch_in_time;
                            shift_details.punch_in_max_time = ShiftModel.punch_in_max_time;
                            shift_details.punch_out_time = ShiftModel.punch_out_time;
                            shift_details.maximum_working_hours = ShiftModel.maximum_working_hours;
                            shift_details.maximum_working_minute = ShiftModel.maximum_working_minute;
                            shift_details.grace_time_for_late_punch = ShiftModel.grace_time_for_late_punch_in;
                            //shift_details.grace_time_for_late_punch_in = ShiftModel.grace_time_for_late_punch_in;
                            //shift_details.grace_time_for_late_punch_out = ShiftModel.grace_time_for_late_punch_out;
                            shift_details.number_of_grace_time_applicable_in_month = ShiftModel.number_of_grace_time_applicable_in_month;
                            shift_details.is_lunch_punch_applicable = ShiftModel.is_lunch_punch_applicable;
                            shift_details.lunch_punch_out_time = ShiftModel.lunch_punch_out_time;
                            shift_details.lunch_punch_in_time = ShiftModel.lunch_punch_in_time;
                            shift_details.maximum_lunch_time = ShiftModel.maximum_lunch_time;
                            shift_details.is_ot_applicable = ShiftModel.is_ot_applicable;
                            //shift_details.maximum_ot_hours = ShiftModel.maximum_ot_hours;
                            shift_details.tea_punch_applicable_one = ShiftModel.tea_punch_applicable_one;
                            shift_details.tea_punch_out_time_one = ShiftModel.tea_punch_out_time_one;
                            shift_details.tea_punch_in_time_one = ShiftModel.tea_punch_in_time_one;
                            shift_details.maximum_tea_time_one = ShiftModel.maximum_tea_time_one;
                            shift_details.tea_punch_applicable_two = ShiftModel.tea_punch_applicable_two;
                            shift_details.tea_punch_out_time_two = ShiftModel.tea_punch_out_time_two;
                            shift_details.tea_punch_in_time_two = ShiftModel.tea_punch_in_time_two;
                            shift_details.maximum_tea_time_two = ShiftModel.maximum_tea_time_two;
                            shift_details.min_halfday_working_hour = ShiftModel.mark_as_half_day_for_working_hours_less_than;
                            //shift_details.mark_as_half_day_for_working_hours_less_than = ShiftModel.mark_as_half_day_for_working_hours_less_than;
                            shift_details.is_night_shift = ShiftModel.is_night_shift;
                            //shift_details.is_punch_ignore = ShiftModel.is_punch_ignore;
                            //shift_details.punch_type = ShiftModel.punch_type;
                            shift_details.weekly_off = ShiftModel.weekly_off;
                            shift_details.created_by = ShiftModel.created_by;
                            shift_details.last_modified_by = ShiftModel.last_modified_by;
                            shift_details.created_date = DateTime.Now;
                            shift_details.last_modified_date = DateTime.Now;

                            _context.tbl_shift_details.Attach(shift_details);
                            _context.Entry(shift_details).State = EntityState.Added;
                            await _context.SaveChangesAsync();

                            // shift_details_id
                            var shift_details_id = shift_details.shift_details_id;
                            //Create new obj of tbl_shift_department table for log                    
                            var tbl_shift_department = _context.tbl_shift_department.Where(x => x.shift_detail_id == id).ToList();
                            tbl_shift_department.ForEach(m => m.is_deleted = 1);
                            _context.SaveChanges();

                            //save in shift Dept

                            if (ShiftModel.shift_for_all_department > 0)
                            {
                                tbl_shift_department shiftDept = new tbl_shift_department();
                                //shiftDept.dept_id = ShiftModel.shift_for_all_department;
                                shiftDept.dept_id = ShiftModel.ShiftDept[0].dept_id;
                                shiftDept.shift_detail_id = shift_details.shift_details_id;
                                shiftDept.is_deleted = 0;
                                _context.tbl_shift_department.Add(shiftDept);
                                _context.SaveChanges();
                            }

                            await _context.SaveChangesAsync();

                            //Create new obj of tbl_shift_details table for log                    
                            var tbl_shift_location = _context.tbl_shift_location.Where(x => x.shift_detail_id == id).ToList();
                            tbl_shift_location.ForEach(m => m.is_deleted = 1);
                            _context.SaveChanges();

                            //save in shift location
                            //save in shift location

                            if (ShiftModel.shift_for_all_company > 0)
                            {

                                tbl_shift_location shiftLoc = new tbl_shift_location();
                                //shiftLoc.company_id = ShiftModel.shift_for_all_company;
                                shiftLoc.company_id = ShiftModel.ShiftLocation[0].company_id;
                                if (ShiftModel.shift_for_all_location > 0)
                                {
                                    //shiftLoc.location_id = ShiftModel.shift_for_all_location;
                                    shiftLoc.location_id = ShiftModel.ShiftLocation[0].location_id;
                                }
                                shiftLoc.shift_detail_id = shift_details_id;
                                _context.tbl_shift_location.Add(shiftLoc);
                                _context.SaveChanges();
                            }

                            await _context.SaveChangesAsync();


                            //Create new obj of tbl_shift_details table for log                    
                            var tbl_shift_week_off = _context.tbl_shift_week_off.Where(x => x.shift_detail_id == id).ToList();

                            tbl_shift_week_off.ForEach(m => m.is_active = 0);
                            _context.SaveChanges();


                            //Save in weekly Off
                            //loop and insert records
                            foreach (ShiftWeekOff sWeekOf in ShiftModel.ShiftWeekOff)
                            {
                                tbl_shift_week_off ShiftWeekOf = new tbl_shift_week_off();
                                ShiftWeekOf.company_id = sWeekOf.company_id;
                                ShiftWeekOf.week_day = sWeekOf.week_day;
                                ShiftWeekOf.days = sWeekOf.days;
                                ShiftWeekOf.shift_detail_id = shift_details_id;
                                ShiftWeekOf.company_id = sWeekOf.company_id;
                                ShiftWeekOf.is_active = sWeekOf.is_active;
                                ShiftWeekOf.created_by = sWeekOf.created_by;
                                ShiftWeekOf.last_modified_by = sWeekOf.last_modified_by;
                                ShiftWeekOf.created_date = DateTime.Now;
                                ShiftWeekOf.last_modified_date = DateTime.Now;

                                _context.tbl_shift_week_off.Attach(ShiftWeekOf);
                                _context.Entry(ShiftWeekOf).State = EntityState.Added;
                            }

                            await _context.SaveChangesAsync();

                            Tran.Commit();
                            objResult.StatusCode = 1;
                            objResult.Message = "Data Updated Successfully...!";
                        }
                        catch (Exception ex)
                        {
                            objResult.StatusCode = 0;
                            objResult.Message = ex.Message;
                            Tran.Rollback();
                        }
                    }


                    return Ok(objResult);
                }

            }
            catch (Exception ex)
            {
                objResult.Message = ex.ToString();
                return Ok(objResult);
            }

        }

        // POST: api/apiShift
        [Authorize(Policy = nameof(enmMenuMaster.CreateShift))]
        [HttpPost]
        public async Task<IActionResult> Posttbl_shift_details([FromBody] ShiftDetails ShiftModel)
        {
            ResponseMsg objResult = new ResponseMsg();
            try
            {
                //Check is Shift Name Already Exist
                var ShiftNameExist = (from a in _context.tbl_shift_details.Where(x => x.shift_name == ShiftModel.shift_name && x.shift_for_all_company == ShiftModel.shift_for_all_company) select a).FirstOrDefault();


                if (ShiftNameExist != null)
                {
                    objResult.Message = "Shift Name Already Exist For This Company...!";
                    objResult.StatusCode = 0;
                    return Ok(objResult);
                }


                // tbl_shift_master
                tbl_shift_master tbl_shift_master = new tbl_shift_master();

                // tbl_shift_master
                tbl_shift_details tbl_shift_details = new tbl_shift_details();

                //Get Last Shif Id
                var LastShiftId = (from a in _context.tbl_shift_master select a).OrderByDescending(x => x.shift_id).FirstOrDefault();

                int shift_id = LastShiftId != null ? (LastShiftId.shift_id + 1) : 1;

                using (var Trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        tbl_shift_master.shift_code = "S0000" + shift_id;
                        tbl_shift_master.is_active = 1;
                        tbl_shift_master.is_default = Convert.ToByte(ShiftModel.is_default);
                        tbl_shift_master.created_date = DateTime.Now;
                        tbl_shift_master.last_modified_date = DateTime.Now;

                        if (Convert.ToByte(ShiftModel.is_default) == 1)
                        {
                            List<tbl_shift_master> tsms = _context.tbl_shift_master.Where(p => p.is_default == 1).ToList();
                            tsms.ForEach(p => { p.is_default = 0; });
                            _context.tbl_shift_master.UpdateRange(tsms);
                            await _context.SaveChangesAsync();
                        }

                        //Save data in tbl_shift_master
                        _context.tbl_shift_master.Add(tbl_shift_master);
                        await _context.SaveChangesAsync();

                        int s_id = _context.tbl_shift_master.Where(a => a.is_active == 1).OrderByDescending(a => a.shift_id).Select(a => a.shift_id).FirstOrDefault();

                        //Insert Data in tbl_shift_details
                        tbl_shift_details.shift_id = s_id;
                        if (ShiftModel.shift_for_all_company == 0)
                        {
                            tbl_shift_details.shift_for_all_location = 1;
                            tbl_shift_details.shift_for_all_company = 1;
                        }
                        else if (ShiftModel.shift_for_all_company > 0 && ShiftModel.shift_for_all_location == 0)
                        {
                            tbl_shift_details.shift_for_all_location = 1;
                            tbl_shift_details.shift_for_all_company = 0;

                        }
                        else
                        {
                            tbl_shift_details.shift_for_all_location = 0;
                            tbl_shift_details.shift_for_all_company = 0;
                        }
                        if (ShiftModel.shift_for_all_department == 0)
                        {
                            tbl_shift_details.shift_for_all_department = 1;
                        }
                        else
                        {
                            tbl_shift_details.shift_for_all_department = 0;
                        }

                        tbl_shift_details.shift_type = ShiftModel.shift_type;
                        tbl_shift_details.shift_name = ShiftModel.shift_name;
                        tbl_shift_details.shift_short_name = ShiftModel.shift_short_name;
                        tbl_shift_details.punch_in_time = ShiftModel.punch_in_time;
                        tbl_shift_details.punch_in_max_time = ShiftModel.punch_in_max_time;
                        tbl_shift_details.punch_out_time = ShiftModel.punch_out_time;
                        tbl_shift_details.maximum_working_hours = ShiftModel.maximum_working_hours;
                        tbl_shift_details.maximum_working_minute = ShiftModel.maximum_working_minute;
                        tbl_shift_details.grace_time_for_late_punch = ShiftModel.grace_time_for_late_punch_in;
                        //tbl_shift_details.grace_time_for_late_punch_in = ShiftModel.grace_time_for_late_punch_in;
                        //tbl_shift_details.grace_time_for_late_punch_out = ShiftModel.grace_time_for_late_punch_out;
                        tbl_shift_details.number_of_grace_time_applicable_in_month = ShiftModel.number_of_grace_time_applicable_in_month;
                        tbl_shift_details.is_lunch_punch_applicable = ShiftModel.is_lunch_punch_applicable;
                        tbl_shift_details.lunch_punch_out_time = ShiftModel.lunch_punch_out_time;
                        tbl_shift_details.lunch_punch_in_time = ShiftModel.lunch_punch_in_time;
                        tbl_shift_details.maximum_lunch_time = ShiftModel.maximum_lunch_time;
                        tbl_shift_details.is_ot_applicable = ShiftModel.is_ot_applicable;
                        //tbl_shift_details.maximum_ot_hours = ShiftModel.maximum_ot_hours;
                        tbl_shift_details.tea_punch_applicable_one = ShiftModel.tea_punch_applicable_one;
                        tbl_shift_details.tea_punch_out_time_one = ShiftModel.tea_punch_out_time_one;
                        tbl_shift_details.tea_punch_in_time_one = ShiftModel.tea_punch_in_time_one;
                        tbl_shift_details.maximum_tea_time_one = ShiftModel.maximum_tea_time_one;
                        tbl_shift_details.tea_punch_applicable_two = ShiftModel.tea_punch_applicable_two;
                        tbl_shift_details.tea_punch_out_time_two = ShiftModel.tea_punch_out_time_two;
                        tbl_shift_details.tea_punch_in_time_two = ShiftModel.tea_punch_in_time_two;
                        tbl_shift_details.maximum_tea_time_two = ShiftModel.maximum_tea_time_two;
                        tbl_shift_details.min_halfday_working_hour = ShiftModel.mark_as_half_day_for_working_hours_less_than;
                        //tbl_shift_details.mark_as_half_day_for_working_hours_less_than = ShiftModel.mark_as_half_day_for_working_hours_less_than;
                        tbl_shift_details.is_night_shift = ShiftModel.is_night_shift;
                        //tbl_shift_details.is_punch_ignore = ShiftModel.is_punch_ignore;
                        //tbl_shift_details.punch_type = ShiftModel.punch_type;
                        tbl_shift_details.weekly_off = ShiftModel.weekly_off;
                        tbl_shift_details.created_by = ShiftModel.created_by;
                        tbl_shift_details.last_modified_by = ShiftModel.last_modified_by;
                        tbl_shift_details.created_date = DateTime.Now;
                        tbl_shift_details.last_modified_date = DateTime.Now;


                        _context.tbl_shift_details.Add(tbl_shift_details);
                        await _context.SaveChangesAsync();
                        var shift_details_id = tbl_shift_details.shift_details_id;
                        // shift_details_id
                        if (ShiftModel.shift_for_all_company > 0)
                        {

                            tbl_shift_location shiftLoc = new tbl_shift_location();
                            //shiftLoc.company_id = ShiftModel.shift_for_all_company;
                            shiftLoc.company_id = ShiftModel.ShiftLocation[0].company_id;
                            if (ShiftModel.shift_for_all_location > 0)
                            {
                                //shiftLoc.location_id = ShiftModel.shift_for_all_location;
                                shiftLoc.location_id = ShiftModel.ShiftLocation[0].location_id;
                            }
                            shiftLoc.shift_detail_id = shift_details_id;
                            _context.tbl_shift_location.Add(shiftLoc);
                            _context.SaveChanges();
                        }
                        //save in shift location
                        //Loop and insert records.
                        //foreach (ShiftLocation sLoc in ShiftModel.ShiftLocation)
                        //{
                        //    tbl_shift_location shiftLoc = new tbl_shift_location();

                        //    shiftLoc.shift_detail_id = shift_details_id;

                        //    if (sLoc.location_id != 0)
                        //    {
                        //        shiftLoc.location_id = sLoc.location_id;
                        //    }
                        //    if (sLoc.company_id != 0)
                        //    {
                        //        shiftLoc.company_id = sLoc.company_id;
                        //    }
                        //    shiftLoc.is_deleted = 0;
                        //    _context.tbl_shift_location.Attach(shiftLoc);
                        //    _context.Entry(shiftLoc).State = EntityState.Added;

                        //}
                        //await _context.SaveChangesAsync();



                        if (ShiftModel.shift_for_all_department > 0)
                        {
                            tbl_shift_department shiftDept = new tbl_shift_department();
                            //shiftDept.dept_id = ShiftModel.shift_for_all_department;
                            shiftDept.dept_id = ShiftModel.ShiftDept[0].dept_id;
                            shiftDept.shift_detail_id = shift_details_id;
                            shiftDept.is_deleted = 0;
                            _context.tbl_shift_department.Add(shiftDept);
                            _context.SaveChanges();
                        }

                        //save in shift Dept
                        //Loop and insert records.

                        //foreach (ShiftDept sDept in ShiftModel.ShiftDept)
                        //{
                        //    tbl_shift_department shiftDept = new tbl_shift_department();

                        //    shiftDept.shift_detail_id = shift_details_id;

                        //    if (sDept.dept_id != 0)
                        //    {
                        //        shiftDept.dept_id = sDept.dept_id;
                        //    }
                        //    shiftDept.is_deleted = 0;
                        //    _context.tbl_shift_department.Attach(shiftDept);
                        //    _context.Entry(shiftDept).State = EntityState.Added;

                        //}
                        //await _context.SaveChangesAsync();


                        //Save in weekly Off
                        //loop and insert records
                        foreach (ShiftWeekOff sWeekOf in ShiftModel.ShiftWeekOff)
                        {
                            tbl_shift_week_off ShiftWeekOf = new tbl_shift_week_off();
                            ShiftWeekOf.company_id = sWeekOf.company_id;
                            ShiftWeekOf.week_day = sWeekOf.week_day;
                            ShiftWeekOf.days = sWeekOf.days;
                            ShiftWeekOf.shift_detail_id = shift_details_id;
                            ShiftWeekOf.company_id = sWeekOf.company_id;
                            ShiftWeekOf.is_active = sWeekOf.is_active;
                            ShiftWeekOf.created_by = sWeekOf.created_by;
                            ShiftWeekOf.last_modified_by = sWeekOf.last_modified_by;
                            ShiftWeekOf.created_date = DateTime.Now;
                            ShiftWeekOf.last_modified_date = DateTime.Now;

                            _context.tbl_shift_week_off.Attach(ShiftWeekOf);
                            _context.Entry(ShiftWeekOf).State = EntityState.Added;

                        }

                        await _context.SaveChangesAsync();

                        Trans.Commit();
                        objResult.StatusCode = 1;
                        objResult.Message = "Shift Added Successfully...!";
                    }
                    catch (Exception ex)
                    {
                        objResult.StatusCode = 0;
                        objResult.Message = ex.Message;
                        Trans.Rollback();
                    }
                }

                return Ok(objResult);

            }
            catch (Exception ex)
            {
                objResult.Message = ex.ToString();
                return Ok(objResult);
            }

        }

        // DELETE: api/apiShift/5
        [HttpDelete("{id}")]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.CreateShift))]
        public async Task<IActionResult> Deletetbl_shift_details([FromRoute] int id)
        {
            ResponseMsg objResult = new ResponseMsg();

            try
            {

                //Create new obj of tbl_shift_details table
                tbl_shift_details tbl_shift_details = _context.tbl_shift_details.FirstOrDefault(a => a.shift_details_id == id);

                // Is Active 3 for delete
                tbl_shift_details.is_deleted = 1;

                _context.tbl_shift_details.Attach(tbl_shift_details);
                _context.Entry(tbl_shift_details).State = EntityState.Modified;
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

        private bool tbl_shift_detailsExists(int id)
        {
            return _context.tbl_shift_details.Any(e => e.shift_details_id == id);
        }

        //Get Last Shift id
        [Route("GetLastShiftId")]
        [HttpGet]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.ShiftReport))]
        public async Task<IActionResult> GetLastShiftId()
        {
            try
            {
                var result = (from a in _context.tbl_shift_master select a).OrderByDescending(x => x.shift_id).FirstOrDefault();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        //save OT Rules
        [Route("SaveOtRules")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.OTCompoffSetting))]
        public IActionResult SaveOtRules(OtRule objOt)
        {
            Response_Msg objResult = new Response_Msg();

            tbl_ot_rule_master otrules = new tbl_ot_rule_master();
            try
            {

                //Get Last LastOtRulesId
                var LastOtRulesId = _context.tbl_ot_rule_master.Where(a => a.is_active == 1).OrderByDescending(a => a.ot_rule_id).FirstOrDefault();
                //var LastOtRulesId = (from a in _context.tbl_ot_rule_master select a).OrderByDescending(x => x.ot_rule_id).First();

                if (LastOtRulesId != null)
                {
                    LastOtRulesId.is_active = 0;
                    LastOtRulesId.created_by = otrules.created_by;
                    LastOtRulesId.created_date = DateTime.Now;
                    LastOtRulesId.last_modified_by = otrules.created_by;
                    LastOtRulesId.last_modified_date = DateTime.Now;

                    _context.tbl_ot_rule_master.Attach(LastOtRulesId);
                    _context.Entry(LastOtRulesId).State = EntityState.Modified;
                    _context.SaveChanges();

                }

                otrules.grace_working_hour = objOt.grace_working_hour;
                otrules.grace_working_minute = objOt.grace_working_minute;
                otrules.is_active = 1;
                otrules.created_by = otrules.created_by;
                otrules.created_date = DateTime.Now;
                otrules.last_modified_by = otrules.created_by;
                otrules.last_modified_date = DateTime.Now;

                _context.Entry(otrules).State = EntityState.Added;
                _context.SaveChanges();



                objResult.StatusCode = 1;
                objResult.Message = "Data Save Successfully...!";
                return Ok(objResult);

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }


        //Get Last Shift id
        [Route("GetLastOtId")]
        [HttpGet]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.OTCompoffSetting))]
        public async Task<IActionResult> GetLastOtId()
        {
            try
            {
                var result = _context.tbl_ot_rule_master.Where(a => a.is_active == 1).OrderByDescending(a => a.ot_rule_id).FirstOrDefault();
                //var result = (from a in _context.tbl_ot_rule_master select a).OrderByDescending(x => x.ot_rule_id).First();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        //save OT Rules
        [Route("SaveCombOff")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.OTCompoffSetting))]
        public IActionResult SaveCombOff(CommbOffRule objOt)
        {
            Response_Msg objResult = new Response_Msg();
            tbl_comb_off_rule_master objComb = new tbl_comb_off_rule_master();
            try
            {

                //Get Last LastOtRulesId
                var LastCombOffId = _context.tbl_comb_off_rule_master.Where(a => a.is_active == 1).OrderByDescending(b => b.comp_off_rule_id).FirstOrDefault();
                // var LastCombOffId = (from a in _context.tbl_comb_off_rule_master select a).OrderByDescending(x => x.comp_off_rule_id).First();

                if (LastCombOffId != null)
                {
                    LastCombOffId.is_active = 0;
                    LastCombOffId.created_by = objOt.created_by;
                    LastCombOffId.created_date = DateTime.Now;
                    LastCombOffId.last_modified_by = objOt.created_by;
                    LastCombOffId.last_modified_date = DateTime.Now;

                    _context.tbl_comb_off_rule_master.Attach(LastCombOffId);
                    _context.Entry(LastCombOffId).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                objComb.is_active = 1;
                objComb.created_by = objOt.created_by;
                objComb.minimum_working_hours = objOt.minimum_working_hours;
                objComb.minimum_working_minute = objOt.minimum_working_minute;
                objComb.last_modified_by = objOt.last_modified_by;
                objComb.created_date = DateTime.Now;
                objComb.last_modified_date = DateTime.Now;

                _context.Entry(objComb).State = EntityState.Added;
                _context.SaveChanges();

                objResult.StatusCode = 1;
                objResult.Message = "Data Save Successfully...!";
                return Ok(objResult);

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        #region---get employee list/shift master/add roaster master for roaster master
        //created by : vibhav
        //created on : 27 Dec 2018
        [Route("GetEmployeeForRoaster/{companyid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.ShiftRoster))]
        public IActionResult GetEmployeeForRoaster([FromRoute] int companyid)
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
                var data = (from a in _context.tbl_employee_master
                            join b in _context.tbl_emp_officaial_sec
                            on a.employee_id equals b.employee_id
                            where b.is_applicable_for_all_comp == companyid && b.is_deleted == 0
                            select new
                            {
                                a.employee_id,
                                a.emp_code,
                                b.employee_first_name,
                                b.employee_middle_name,
                                b.employee_last_name
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

        [Route("GetShiftForRoaster/{companyid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.ShiftRoster))]
        public IActionResult GetShiftForRoaster([FromRoute] int companyid)
        {
            ResponseMsg objresponse = new ResponseMsg();

            try
            {

                //if (companyid > 0)
                //{
                //    if (!_clsCurrentUser.CompanyId.Contains(companyid))
                //    {
                //        objresponse.StatusCode = 1;
                //        objresponse.Message = "Unauthorize Company Access...!!";
                //        return Ok(objresponse);
                //    }

                //    var data = _context.tbl_shift_location.OrderByDescending(x => x.shift_detail_id).Where(x => x.company_id == companyid && x.tbl_shift_details.is_deleted == 0).Select(p => new
                //    {
                //        p.tbl_shift_details.shift_id,
                //        p.tbl_shift_details.shift_name,
                //        p.tbl_shift_details.shift_short_name
                //    }).ToList();

                //    return Ok(data);
                //}
                //else
                //{
                var data = (from a in _context.tbl_shift_details
                            where a.is_deleted == 0
                            select new
                            {
                                a.shift_id,
                                a.shift_name,
                                a.shift_short_name
                            }).ToList();

                return Ok(data);
                //}



            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("SaveRoasterMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.ShiftRoster))]
        public IActionResult SaveRoasterMaster([FromBody] RoasterMasterModel RstModel)
        {
            Response_Msg objResult = new Response_Msg();
            foreach (var ids in RstModel.emp_id)
            {
                if (!_clsCurrentUser.DownlineEmpId.Contains(Convert.ToInt32(ids)))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Company Access..!!";
                    return Ok(objResult);
                }
            }
            try
            {
                //var exist=_context.tbl_shift_roster_master.Where(x=> x.)
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < RstModel.emp_id.Count; i++)
                        {
                            tbl_shift_roster_master ObjRoaster = new tbl_shift_roster_master();
                            tbl_shift_roster_master_log Objlog = new tbl_shift_roster_master_log();
                            ObjRoaster.emp_id = Convert.ToInt32(RstModel.emp_id[i]);
                            ObjRoaster.applicable_from_date = Convert.ToDateTime(RstModel.applicable_from_date);
                            ObjRoaster.applicable_to_date = Convert.ToDateTime(RstModel.applicable_to_date);
                            ObjRoaster.shft_id1 = RstModel.shft_id1 == 0 ? ObjRoaster.shft_id1 : RstModel.shft_id1;
                            ObjRoaster.shft_id2 = RstModel.shft_id2 == 0 ? ObjRoaster.shft_id2 : RstModel.shft_id2;
                            ObjRoaster.shft_id3 = RstModel.shft_id3 == 0 ? ObjRoaster.shft_id3 : RstModel.shft_id3;
                            ObjRoaster.shft_id4 = RstModel.shft_id4 == 0 ? ObjRoaster.shft_id4 : RstModel.shft_id4;
                            ObjRoaster.shft_id5 = RstModel.shft_id5 == 0 ? ObjRoaster.shft_id5 : RstModel.shft_id5;
                            ObjRoaster.shift_rotat_in_day = RstModel.shift_rotat_in_day;

                            _context.Entry(ObjRoaster).State = EntityState.Added;
                            _context.SaveChanges();

                            //save in log table
                            //Objlog.r_id = ObjRoaster.shift_roster_id;
                            //Objlog.emp_id = Convert.ToInt32(RstModel.emp_id[i]);
                            //Objlog.applicable_from_date = Convert.ToDateTime(RstModel.applicable_from_date);
                            //Objlog.applicable_to_date = Convert.ToDateTime(RstModel.applicable_to_date);
                            //Objlog.shift_id1 = RstModel.shft_id1 == 0 ? ObjRoaster.shft_id1 : RstModel.shft_id1;
                            //Objlog.shift_id2 = RstModel.shft_id2 == 0 ? ObjRoaster.shft_id2 : RstModel.shft_id2;
                            //Objlog.shift_id3 = RstModel.shft_id3 == 0 ? ObjRoaster.shft_id3 : RstModel.shft_id3;
                            //Objlog.shift_id4 = RstModel.shft_id4 == 0 ? ObjRoaster.shft_id4 : RstModel.shft_id4;
                            //Objlog.shift_id5 = RstModel.shft_id5 == 0 ? ObjRoaster.shft_id5 : RstModel.shft_id5;
                            //Objlog.shift_rotat_in_day = RstModel.shift_rotat_in_day;
                            //Objlog.requested_by = 1;
                            //Objlog.requested_date = DateTime.Now;                        

                            //_context.Entry(Objlog).State = EntityState.Added;
                            //_context.SaveChanges();

                        }
                        objResult.StatusCode = 0;
                        objResult.Message = "Roaster master added successfully !!";
                        transaction.Commit();//after all data save commit transactions
                    }
                    catch (Exception ex)
                    {
                        objResult.StatusCode = 2;
                        objResult.Message = ex.Message;
                        transaction.Rollback();
                    }

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

        [Route("UpdateRoasterMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.ShiftRoster))]
        public IActionResult UpdateRoasterMaster([FromBody] RoasterMasterModel RstModel)
        {
            Response_Msg objResult = new Response_Msg();
            foreach (var id in RstModel.emp_id)
            {
                if (!_clsCurrentUser.DownlineEmpId.Contains(Convert.ToInt32(id)))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access...!!";
                    return Ok(objResult);
                }
            }
            try
            {
                var exist = _context.tbl_shift_roster_master.Where(x => x.shift_roster_id == RstModel.shift_roster_id).FirstOrDefault();
                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid shift roaster id please try again !!";
                    return Ok(objResult);
                }
                using (var transaction = _context.Database.BeginTransaction())
                {

                    tbl_shift_roster_master ObjRoaster = new tbl_shift_roster_master();
                    tbl_shift_roster_master_log Objlog = new tbl_shift_roster_master_log();

                    try
                    {
                        // exist.emp_id = Convert.ToInt32(RstModel.emp_id[0]);
                        exist.applicable_from_date = Convert.ToDateTime(RstModel.applicable_from_date);
                        exist.applicable_to_date = Convert.ToDateTime(RstModel.applicable_to_date);
                        exist.shft_id1 = RstModel.shft_id1 == 0 ? ObjRoaster.shft_id1 : RstModel.shft_id1;
                        exist.shft_id2 = RstModel.shft_id2 == 0 ? ObjRoaster.shft_id2 : RstModel.shft_id2;
                        exist.shft_id3 = RstModel.shft_id3 == 0 ? ObjRoaster.shft_id3 : RstModel.shft_id3;
                        exist.shft_id4 = RstModel.shft_id4 == 0 ? ObjRoaster.shft_id4 : RstModel.shft_id4;
                        exist.shft_id5 = RstModel.shft_id5 == 0 ? ObjRoaster.shft_id5 : RstModel.shft_id5;
                        exist.shift_rotat_in_day = RstModel.shift_rotat_in_day;

                        _context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();

                        //save in log table
                        //Objlog.r_id = ObjRoaster.shift_roster_id;
                        //Objlog.emp_id = Convert.ToInt32(RstModel.emp_id[i]);
                        //Objlog.applicable_from_date = Convert.ToDateTime(RstModel.applicable_from_date);
                        //Objlog.applicable_to_date = Convert.ToDateTime(RstModel.applicable_to_date);
                        //Objlog.shift_id1 = RstModel.shft_id1 == 0 ? ObjRoaster.shft_id1 : RstModel.shft_id1;
                        //Objlog.shift_id2 = RstModel.shft_id2 == 0 ? ObjRoaster.shft_id2 : RstModel.shft_id2;
                        //Objlog.shift_id3 = RstModel.shft_id3 == 0 ? ObjRoaster.shft_id3 : RstModel.shft_id3;
                        //Objlog.shift_id4 = RstModel.shft_id4 == 0 ? ObjRoaster.shft_id4 : RstModel.shft_id4;
                        //Objlog.shift_id5 = RstModel.shft_id5 == 0 ? ObjRoaster.shft_id5 : RstModel.shft_id5;
                        //Objlog.shift_rotat_in_day = RstModel.shift_rotat_in_day;
                        //Objlog.requested_by = 1;
                        //Objlog.requested_date = DateTime.Now;                        

                        //_context.Entry(Objlog).State = EntityState.Added;
                        //_context.SaveChanges();


                        transaction.Commit();
                        objResult.StatusCode = 0;
                        objResult.Message = "Roaster master updated successfully !!";
                    }
                    catch (Exception ex)
                    {
                        objResult.StatusCode = 0;
                        objResult.Message = ex.Message;
                        transaction.Rollback();
                    }

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

        [Route("GetRoasterDetails/{roasterid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.ShiftRoster))]
        public IActionResult GetRoasterDetails([FromRoute] int roasterid)
        {
            try
            {
                if (roasterid > 0)
                {
                    var data = _context.tbl_shift_roster_master.Where(x => x.shift_roster_id == roasterid).Select(p => new
                    {
                        p.emp_id,
                        p.shift_roster_id,
                        p.shift_rotat_in_day,
                        p.shft_id1,
                        p.shft_id2,
                        p.shft_id3,
                        p.shft_id4,
                        p.shft_id5,
                        p.applicable_from_date,
                        p.applicable_to_date,
                        p.tbl_employee_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).is_applicable_for_all_comp,
                        emp_name = string.Format("{0} {1} {2}",
                                 p.tbl_employee_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                                 p.tbl_employee_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                                 p.tbl_employee_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name),
                        emp_code = p.tbl_employee_master.emp_code,
                        p.tbl_employee_master.tbl_employee_company_map.FirstOrDefault(y => y.is_deleted == 0).company_id
                    }).FirstOrDefault();

                    var shiftdata = _context.tbl_shift_location.OrderByDescending(x => x.tbl_shift_details.shift_id).Where(x => x.tbl_shift_details.is_deleted == 0 && x.company_id == data.company_id).Select(p => new
                    {

                        p.tbl_shift_details.shift_id,
                        p.tbl_shift_details.shift_name,
                        p.tbl_shift_details.shift_short_name
                    }).ToList();



                    var alldata = new { data, shiftdata };

                    return Ok(alldata);
                }
                else
                {
                    var data = _context.tbl_shift_roster_master.OrderByDescending(z => z.shift_roster_id).Where(x => x.tbl_employee_master.is_active == 1 && _clsCurrentUser.DownlineEmpId.Contains(x.emp_id ?? 0)).Select(p => new
                    {


                        p.emp_id,
                        p.shift_roster_id,
                        p.shift_rotat_in_day,
                        p.shft_id1,
                        p.shft_id2,
                        p.shft_id3,
                        p.shft_id4,
                        p.shft_id5,
                        p.applicable_from_date,
                        p.applicable_to_date,
                        p.tbl_employee_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).is_applicable_for_all_comp,
                        emp_name = string.Format("{0} {1} {2}", p.tbl_employee_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                                  p.tbl_employee_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                                  p.tbl_employee_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name),
                        emp_code = p.tbl_employee_master.emp_code,
                        p.tbl_employee_master.tbl_employee_company_map.FirstOrDefault(y => y.is_deleted == 0).company_id,

                    }).ToList();


                    var shiftdata = (from a in _context.tbl_shift_details
                                     where a.is_deleted == 0
                                     select new
                                     {
                                         a.shift_id,
                                         a.shift_name,
                                         a.shift_short_name
                                     }).ToList();

                    var alldata = new { data, shiftdata };

                    return Ok(alldata);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #endregion

        //Get Last Comb Off id
        [Route("GetLastCombOffId")]
        [HttpGet]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.OTCompoffSetting))]
        public async Task<IActionResult> GetLastCombOffId()
        {
            try
            {
                var result = _context.tbl_comb_off_rule_master.Where(a => a.is_active == 1).OrderByDescending(a => a.comp_off_rule_id).FirstOrDefault();


                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        #region ** Start by Supriya, on 22-04-2020, Shift Assingment **


        [Route("Save_ShiftAllignment")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.ShiftAssignemnt))]
        public async Task<IActionResult> Save_ShiftAllignment([FromBody] ShiftAssign objshift_assign)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(objshift_assign.company_id ?? 0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }

            foreach (var id in objshift_assign.emp_)
            {
                if (!_clsCurrentUser.DownlineEmpId.Contains(Convert.ToInt32(id)))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unauthorize Access...!!";
                    return Ok(objresponse);
                }
            }



            try
            {

                using (var trans = _context.Database.BeginTransaction())
                {

                    try
                    {

                        for (int i = 0; i < objshift_assign.emp_.Count; i++)
                        {
                            //check shift already assign of this date or not
                            //var exist = _context.tbl_emp_shift_allocation.OrderByDescending(y => y.emp_shift_id).Where(x => x.employee_id == Convert.ToInt32(objshift_assign.emp_[i]) &&
                            //  x.applicable_from_date.Date <= objshift_assign.effective_dt.Date && objshift_assign.effective_dt.Date <= x.applicable_to_date.Date
                            //).FirstOrDefault();
                            //if (exist != null)
                            //{
                            //    exist.applicable_to_date = objshift_assign.effective_dt.AddDays(-1);

                            //    _context.UpdateRange(exist);
                            //}


                            tbl_emp_shift_allocation objemp_shift = new tbl_emp_shift_allocation();
                            objemp_shift.shift_id = objshift_assign.shift_id;
                            objemp_shift.employee_id = Convert.ToInt32(objshift_assign.emp_[i]);
                            objemp_shift.applicable_from_date = objshift_assign.effective_dt;
                            objemp_shift.created_date = DateTime.Now;
                            objemp_shift.is_deleted = 0;

                            //var exit_after_effective = _context.tbl_emp_shift_allocation.OrderByDescending(x => x.emp_shift_id).Where(x => x.employee_id == Convert.ToInt32(objshift_assign.emp_[i]) &&
                            //    x.applicable_from_date.Date.Date > objshift_assign.effective_dt.Date).FirstOrDefault();
                            //if (exit_after_effective != null)
                            //{
                            //    objemp_shift.applicable_to_date = exit_after_effective.applicable_from_date.AddDays(-1);
                            //}
                            //else
                            //{
                            //    objemp_shift.applicable_to_date = Convert.ToDateTime("2500-01-01");
                            //}



                            _context.tbl_emp_shift_allocation.AddRange(objemp_shift);
                        }


                        _context.SaveChanges();
                        trans.Commit();

                        objresponse.StatusCode = 0;
                        objresponse.Message = "Shift successfully assign";

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        objresponse.StatusCode = 1;
                        objresponse.Message = ex.Message;

                    }
                };
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;

            }

            return Ok(objresponse);
        }

        [Route("Get_EmpShiftDetails/{from_date}/{to_date}/{company_id}/{location_id}/{dept_id}/{EmpType}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.ShiftAssignemnt))]
        public async Task<IActionResult> Get_EmpShiftDetails(string from_date, string to_date, int company_id, int location_id, int dept_id, int EmpType)
        {
            ResponseMsg objresponse = new ResponseMsg();
            try
            {
                DateTime from_date1 = Convert.ToDateTime(from_date);
                DateTime to_date1 = Convert.ToDateTime(to_date);

                Int32 PayrollDate = 1;
                var payroll = _context.tbl_payroll_month_setting.Where(p => p.is_deleted == 0).FirstOrDefault();
                if (payroll != null)
                {
                    // PayrollDate = Convert.ToInt32(payroll.from_date.ToString("dd"));
                    PayrollDate = Convert.ToInt32(payroll.from_date);
                }
                List<shiftreport> shiftlist = new List<shiftreport>();
                using (var trans = _context.Database.BeginTransaction())
                {

                    try
                    {
                        if (company_id > 0 && location_id == 0 && dept_id == 0)
                        {
                            if (!_clsCurrentUser.CompanyId.Contains(company_id))
                            {
                                objresponse.StatusCode = 1;
                                objresponse.Message = "Unauthorize Access...!!";
                                return Ok(objresponse);
                            }

                            _clsEmployeeDetail._company_id = company_id;
                            var emp_dtl = _clsEmployeeDetail.GetEmployeeByDate(company_id, Convert.ToDateTime("01-01-2000"), DateTime.Now.AddDays(1), 1, EmpType).Where(x => x.emp_status < 100 && (EmpType == 1 ? _clsCurrentUser.DownlineEmpId.Contains(x.employee_id) : true));

                            List<int> emp_id = emp_dtl.Select(p => p.employee_id).ToList();

                            var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && emp_id.Contains(x.employee_id ?? 0)).ToList();


                            for (int Index = 0; Index < teos.Count; Index++)
                            {
                                Classes.clsCalcualteDailyAttendance CalculateAttendance = new Classes.clsCalcualteDailyAttendance(
                                 _context, teos[Index].employee_id ?? 0, teos[Index].emp_official_section_id, teos[Index].location_id ?? 0,
                                 teos[Index].religion_id ?? 0, Convert.ToByte(teos[Index].is_fixed_weekly_off), PayrollDate, teos[Index].punch_type,
                                 teos[Index].is_ot_allowed, Convert.ToByte(teos[Index].is_comb_off_allowed), from_date1
                                 , to_date1, teos[Index].date_of_joining);

                                var data = CalculateAttendance.Change_shift_dtl(false);


                                var department_ = _context.tbl_department_master.Where(x => x.department_id == teos[Index].department_id).FirstOrDefault();
                                var location_ = _context.tbl_location_master.Where(x => x.location_id == teos[Index].location_id).FirstOrDefault();
                                var emp_code = _context.tbl_employee_master.Where(x => x.is_active == EmpType && x.employee_id == teos[Index].employee_id).FirstOrDefault();

                                for (int j = 0; j < data.Count; j++)
                                {

                                    var shift_dtl = _context.tbl_shift_details.Where(x => x.is_deleted == 0 && x.shift_details_id == data[j].ShiftDetailId).FirstOrDefault();

                                    shiftreport obj_shft = new shiftreport();
                                    obj_shft.effective_Dt = data[j].shiftt_dt;
                                    obj_shft.shift_in_time = Convert.ToDateTime(data[j].InTime);
                                    obj_shft.shift_out_time = Convert.ToDateTime(data[j].OutTime);
                                    obj_shft.emp_name = string.Format("{0} {1} {2}", teos[Index].employee_first_name, teos[Index].employee_middle_name, teos[Index].employee_last_name);
                                    obj_shft.shift_name = shift_dtl != null ? shift_dtl.shift_name : "";
                                    obj_shft.department_name = department_ != null ? department_.department_name : "";
                                    obj_shft.emp_id = teos[Index].employee_id ?? 0;
                                    obj_shft.location_name = location_ != null ? location_.location_name : "";
                                    obj_shft.emp_code = emp_code != null ? emp_code.emp_code : "";
                                    shiftlist.Add(obj_shft);

                                }
                            }

                        }
                        else if (company_id > 0 && location_id > 0 && dept_id == 0)
                        {
                            if (!_clsCurrentUser.CompanyId.Contains(company_id))
                            {
                                objresponse.StatusCode = 1;
                                objresponse.Message = "Unauthorize Access...!!";
                                return Ok(objresponse);
                            }
                            //List<shiftreport> shiftlist = new List<shiftreport>();

                            var emp_dtl = _clsEmployeeDetail.GetEmployeeByDate(company_id, Convert.ToDateTime("01-01-2000"), DateTime.Now.AddDays(1), 1, EmpType).Where(x => x.emp_status < 100 && _clsCurrentUser.DownlineEmpId.Contains(x.employee_id));

                            List<int> emp_id = emp_dtl.Select(p => p.employee_id).ToList();

                            var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && emp_id.Contains(x.employee_id ?? 0) && !string.IsNullOrEmpty(x.employee_first_name)).ToList();


                            for (int Index = 0; Index < teos.Count; Index++)
                            {
                                Classes.clsCalcualteDailyAttendance CalculateAttendance = new Classes.clsCalcualteDailyAttendance(
                                 _context, teos[Index].employee_id ?? 0, teos[Index].emp_official_section_id, teos[Index].location_id ?? 0,
                                 teos[Index].religion_id ?? 0, Convert.ToByte(teos[Index].is_fixed_weekly_off), PayrollDate, teos[Index].punch_type,
                                 teos[Index].is_ot_allowed, Convert.ToByte(teos[Index].is_comb_off_allowed), from_date1
                                 , to_date1, teos[Index].date_of_joining);

                                var data = CalculateAttendance.Change_shift_dtl(false);


                                var department_ = _context.tbl_department_master.Where(x => x.department_id == teos[Index].department_id).FirstOrDefault();
                                var location_ = _context.tbl_location_master.Where(x => x.location_id == teos[Index].location_id).FirstOrDefault();
                                var emp_code = _context.tbl_employee_master.Where(x => x.is_active == EmpType && x.employee_id == teos[Index].employee_id).FirstOrDefault();

                                for (int j = 0; j < data.Count; j++)
                                {

                                    var shift_dtl = _context.tbl_shift_details.Where(x => x.is_deleted == 0 && x.shift_details_id == data[j].ShiftDetailId).FirstOrDefault();

                                    if (location_id == teos[Index].location_id)
                                    {
                                        shiftreport obj_shft = new shiftreport();
                                        obj_shft.effective_Dt = data[j].shiftt_dt;
                                        obj_shft.shift_in_time = Convert.ToDateTime(data[j].InTime);
                                        obj_shft.shift_out_time = Convert.ToDateTime(data[j].OutTime);
                                        obj_shft.emp_name = string.Format("{0} {1} {2}", teos[Index].employee_first_name, teos[Index].employee_middle_name, teos[Index].employee_last_name);
                                        obj_shft.shift_name = shift_dtl != null ? shift_dtl.shift_name : "";
                                        obj_shft.department_name = department_ != null ? department_.department_name : "";
                                        obj_shft.emp_id = teos[Index].employee_id ?? 0;
                                        obj_shft.location_name = location_ != null ? location_.location_name : "";
                                        obj_shft.emp_code = emp_code != null ? emp_code.emp_code : "";
                                        shiftlist.Add(obj_shft);
                                    }



                                }
                            }



                        }
                        else if (company_id == 0 && location_id > 0 && dept_id == 0)
                        {

                            // List<shiftreport> shiftlist = new List<shiftreport>();

                            // var emp_comp_dtl = _context.tbl_employee_company_map.Where(x => x.is_deleted == 0).Select(p => p.company_id).Distinct().ToList();
                            var emp_comp_dtl = _clsCurrentUser.CompanyId.ToList();
                            for (int comp_index = 0; comp_index < emp_comp_dtl.Count; comp_index++)
                            {

                                var emp_dtl = _clsEmployeeDetail.GetEmployeeByDate(emp_comp_dtl[comp_index], Convert.ToDateTime("01-01-2000"), DateTime.Now.AddDays(1), 1, EmpType).Where(x => x.emp_status < 100 && _clsCurrentUser.DownlineEmpId.Contains(x.employee_id));

                                List<int> emp_id = emp_dtl.Select(p => p.employee_id).ToList();

                                var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && emp_id.Contains(x.employee_id ?? 0) && !string.IsNullOrEmpty(x.employee_first_name)).ToList();


                                for (int Index = 0; Index < teos.Count; Index++)
                                {
                                    Classes.clsCalcualteDailyAttendance CalculateAttendance = new Classes.clsCalcualteDailyAttendance(
                                     _context, teos[Index].employee_id ?? 0, teos[Index].emp_official_section_id, teos[Index].location_id ?? 0,
                                     teos[Index].religion_id ?? 0, Convert.ToByte(teos[Index].is_fixed_weekly_off), PayrollDate, teos[Index].punch_type,
                                     teos[Index].is_ot_allowed, Convert.ToByte(teos[Index].is_comb_off_allowed), from_date1
                                     , to_date1, teos[Index].date_of_joining);

                                    var data = CalculateAttendance.Change_shift_dtl(false);


                                    var department_ = _context.tbl_department_master.Where(x => x.department_id == teos[Index].department_id).FirstOrDefault();
                                    var location_ = _context.tbl_location_master.Where(x => x.location_id == teos[Index].location_id).FirstOrDefault();
                                    var emp_code = _context.tbl_employee_master.Where(x => x.is_active == EmpType && x.employee_id == teos[Index].employee_id).FirstOrDefault();

                                    for (int j = 0; j < data.Count; j++)
                                    {

                                        var shift_dtl = _context.tbl_shift_details.Where(x => x.is_deleted == 0 && x.shift_details_id == data[j].ShiftDetailId).FirstOrDefault();

                                        if (location_id == teos[Index].location_id)
                                        {
                                            shiftreport obj_shft = new shiftreport();
                                            obj_shft.effective_Dt = data[j].shiftt_dt;
                                            obj_shft.shift_in_time = Convert.ToDateTime(data[j].InTime);
                                            obj_shft.shift_out_time = Convert.ToDateTime(data[j].OutTime);
                                            obj_shft.emp_name = string.Format("{0} {1} {2}", teos[Index].employee_first_name, teos[Index].employee_middle_name, teos[Index].employee_last_name);
                                            obj_shft.shift_name = shift_dtl != null ? shift_dtl.shift_name : "";
                                            obj_shft.department_name = department_ != null ? department_.department_name : "";
                                            obj_shft.emp_id = teos[Index].employee_id ?? 0;
                                            obj_shft.location_name = location_ != null ? location_.location_name : "";
                                            obj_shft.emp_code = emp_code != null ? emp_code.emp_code : "";
                                            shiftlist.Add(obj_shft);
                                        }




                                    }
                                }

                            }




                        }
                        else if (company_id == 0 && location_id == 0 && dept_id > 0)
                        {


                            var emp_comp_dtl = _clsCurrentUser.CompanyId.ToList(); //_context.tbl_employee_company_map.Where(x => x.is_deleted == 0).Select(p => p.company_id).Distinct().ToList();

                            for (int comp_index = 0; comp_index < emp_comp_dtl.Count; comp_index++)
                            {

                                var emp_dtl = _clsEmployeeDetail.GetEmployeeByDate(emp_comp_dtl[comp_index], Convert.ToDateTime("01-01-2000"), DateTime.Now.AddDays(1), 1, EmpType).Where(x => x.emp_status < 100 && _clsCurrentUser.DownlineEmpId.Contains(x.employee_id)).ToList();

                                List<int> emp_id = emp_dtl.Select(p => p.employee_id).ToList();

                                var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && emp_id.Contains(x.employee_id ?? 0) && !string.IsNullOrEmpty(x.employee_first_name)).ToList();


                                for (int Index = 0; Index < teos.Count; Index++)
                                {
                                    Classes.clsCalcualteDailyAttendance CalculateAttendance = new Classes.clsCalcualteDailyAttendance(
                                     _context, teos[Index].employee_id ?? 0, teos[Index].emp_official_section_id, teos[Index].location_id ?? 0,
                                     teos[Index].religion_id ?? 0, Convert.ToByte(teos[Index].is_fixed_weekly_off), PayrollDate, teos[Index].punch_type,
                                     teos[Index].is_ot_allowed, Convert.ToByte(teos[Index].is_comb_off_allowed), from_date1
                                     , to_date1, teos[Index].date_of_joining);

                                    var data = CalculateAttendance.Change_shift_dtl(false);


                                    var department_ = _context.tbl_department_master.Where(x => x.department_id == teos[Index].department_id).FirstOrDefault();
                                    var location_ = _context.tbl_location_master.Where(x => x.location_id == teos[Index].location_id).FirstOrDefault();
                                    var emp_code = _context.tbl_employee_master.Where(x => x.is_active == EmpType && x.employee_id == teos[Index].employee_id).FirstOrDefault();

                                    for (int j = 0; j < data.Count; j++)
                                    {

                                        var shift_dtl = _context.tbl_shift_details.Where(x => x.is_deleted == 0 && x.shift_details_id == data[j].ShiftDetailId).FirstOrDefault();

                                        if (dept_id == teos[Index].department_id)
                                        {
                                            shiftreport obj_shft = new shiftreport();
                                            obj_shft.effective_Dt = data[j].shiftt_dt;
                                            obj_shft.shift_in_time = Convert.ToDateTime(data[j].InTime);
                                            obj_shft.shift_out_time = Convert.ToDateTime(data[j].OutTime);
                                            obj_shft.emp_name = string.Format("{0} {1} {2}", teos[Index].employee_first_name, teos[Index].employee_middle_name, teos[Index].employee_last_name);
                                            obj_shft.shift_name = shift_dtl != null ? shift_dtl.shift_name : "";
                                            obj_shft.department_name = department_ != null ? department_.department_name : "";
                                            obj_shft.emp_id = teos[Index].employee_id ?? 0;
                                            obj_shft.location_name = location_ != null ? location_.location_name : "";
                                            obj_shft.emp_code = emp_code != null ? emp_code.emp_code : "";
                                            shiftlist.Add(obj_shft);
                                        }




                                    }
                                }

                            }




                        }
                        else if (company_id > 0 && location_id > 0 && dept_id > 0)
                        {
                            if (!_clsCurrentUser.CompanyId.Contains(company_id))
                            {
                                objresponse.StatusCode = 1;
                                objresponse.Message = "Unauthorize Access....!!";
                                return Ok(objresponse);
                            }

                            var emp_dtl = _clsEmployeeDetail.GetEmployeeByDate(company_id, Convert.ToDateTime("01-01-2000"), DateTime.Now.AddDays(1), 1, EmpType).Where(x => x.emp_status < 100 && _clsCurrentUser.DownlineEmpId.Contains(x.employee_id)).ToList();

                            List<int> emp_id = emp_dtl.Select(p => p.employee_id).ToList();

                            var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && emp_id.Contains(x.employee_id ?? 0) && !string.IsNullOrEmpty(x.employee_first_name)).ToList();


                            for (int Index = 0; Index < teos.Count; Index++)
                            {
                                Classes.clsCalcualteDailyAttendance CalculateAttendance = new Classes.clsCalcualteDailyAttendance(
                                 _context, teos[Index].employee_id ?? 0, teos[Index].emp_official_section_id, teos[Index].location_id ?? 0,
                                 teos[Index].religion_id ?? 0, Convert.ToByte(teos[Index].is_fixed_weekly_off), PayrollDate, teos[Index].punch_type,
                                 teos[Index].is_ot_allowed, Convert.ToByte(teos[Index].is_comb_off_allowed), from_date1
                                 , to_date1, teos[Index].date_of_joining);

                                var data = CalculateAttendance.Change_shift_dtl(false);


                                var department_ = _context.tbl_department_master.Where(x => x.department_id == teos[Index].department_id).FirstOrDefault();
                                var location_ = _context.tbl_location_master.Where(x => x.location_id == teos[Index].location_id).FirstOrDefault();
                                var emp_code = _context.tbl_employee_master.Where(x => x.is_active == EmpType && x.employee_id == teos[Index].employee_id).FirstOrDefault();

                                for (int j = 0; j < data.Count; j++)
                                {

                                    var shift_dtl = _context.tbl_shift_details.Where(x => x.is_deleted == 0 && x.shift_details_id == data[j].ShiftDetailId).FirstOrDefault();

                                    if (location_id == teos[Index].location_id && dept_id == teos[Index].department_id)
                                    {
                                        shiftreport obj_shft = new shiftreport();
                                        obj_shft.effective_Dt = data[j].shiftt_dt;
                                        obj_shft.shift_in_time = Convert.ToDateTime(data[j].InTime);
                                        obj_shft.shift_out_time = Convert.ToDateTime(data[j].OutTime);
                                        obj_shft.emp_name = string.Format("{0} {1} {2}", teos[Index].employee_first_name, teos[Index].employee_middle_name, teos[Index].employee_last_name);
                                        obj_shft.shift_name = shift_dtl != null ? shift_dtl.shift_name : "";
                                        obj_shft.department_name = department_ != null ? department_.department_name : "";
                                        obj_shft.emp_id = teos[Index].employee_id ?? 0;
                                        obj_shft.location_name = location_ != null ? location_.location_name : "";
                                        obj_shft.emp_code = emp_code != null ? emp_code.emp_code : "";
                                        shiftlist.Add(obj_shft);
                                    }



                                }
                            }


                        }
                        else if (company_id > 0 && location_id == 0 && dept_id > 0)
                        {
                            if (!_clsCurrentUser.CompanyId.Contains(company_id))
                            {
                                objresponse.StatusCode = 1;
                                objresponse.Message = "Unauthorize Company Access......!!";
                                return Ok(objresponse);
                            }
                            var emp_dtl = _clsEmployeeDetail.GetEmployeeByDate(company_id, Convert.ToDateTime("01-01-2000"), DateTime.Now.AddDays(1), 1, EmpType).Where(x => x.emp_status < 100 && _clsCurrentUser.DownlineEmpId.Contains(x.employee_id)).ToList();

                            List<int> emp_id = emp_dtl.Select(p => p.employee_id).ToList();

                            var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && emp_id.Contains(x.employee_id ?? 0) && !string.IsNullOrEmpty(x.employee_first_name)).ToList();


                            for (int Index = 0; Index < teos.Count; Index++)
                            {
                                Classes.clsCalcualteDailyAttendance CalculateAttendance = new Classes.clsCalcualteDailyAttendance(
                                 _context, teos[Index].employee_id ?? 0, teos[Index].emp_official_section_id, teos[Index].location_id ?? 0,
                                 teos[Index].religion_id ?? 0, Convert.ToByte(teos[Index].is_fixed_weekly_off), PayrollDate, teos[Index].punch_type,
                                 teos[Index].is_ot_allowed, Convert.ToByte(teos[Index].is_comb_off_allowed), from_date1
                                 , to_date1, teos[Index].date_of_joining);

                                var data = CalculateAttendance.Change_shift_dtl(false);


                                var department_ = _context.tbl_department_master.Where(x => x.department_id == teos[Index].department_id).FirstOrDefault();
                                var location_ = _context.tbl_location_master.Where(x => x.location_id == teos[Index].location_id).FirstOrDefault();
                                var emp_code = _context.tbl_employee_master.Where(x => x.is_active == EmpType && x.employee_id == teos[Index].employee_id).FirstOrDefault();

                                for (int j = 0; j < data.Count; j++)
                                {

                                    var shift_dtl = _context.tbl_shift_details.Where(x => x.is_deleted == 0 && x.shift_details_id == data[j].ShiftDetailId).FirstOrDefault();

                                    if (dept_id == teos[Index].department_id)
                                    {
                                        shiftreport obj_shft = new shiftreport();
                                        obj_shft.effective_Dt = data[j].shiftt_dt;
                                        obj_shft.shift_in_time = Convert.ToDateTime(data[j].InTime);
                                        obj_shft.shift_out_time = Convert.ToDateTime(data[j].OutTime);
                                        obj_shft.emp_name = string.Format("{0} {1} {2}", teos[Index].employee_first_name, teos[Index].employee_middle_name, teos[Index].employee_last_name);
                                        obj_shft.shift_name = shift_dtl != null ? shift_dtl.shift_name : "";
                                        obj_shft.department_name = department_ != null ? department_.department_name : "";
                                        obj_shft.emp_id = teos[Index].employee_id ?? 0;
                                        obj_shft.location_name = location_ != null ? location_.location_name : "";
                                        obj_shft.emp_code = emp_code != null ? emp_code.emp_code : "";
                                        shiftlist.Add(obj_shft);
                                    }



                                }
                            }
                        }
                        else if (company_id == 0 && location_id > 0 && dept_id > 0)
                        {
                            var emp_comp_dtl = _clsCurrentUser.CompanyId.ToList(); //_context.tbl_employee_company_map.Where(x => x.is_deleted == 0).Select(p => p.company_id).Distinct().ToList();

                            for (int comp_index = 0; comp_index < emp_comp_dtl.Count; comp_index++)
                            {
                                var emp_dtl = _clsEmployeeDetail.GetEmployeeByDate(emp_comp_dtl[comp_index], Convert.ToDateTime("01-01-2000"), DateTime.Now.AddDays(1), 1, EmpType).Where(x => x.emp_status < 100 && _clsCurrentUser.DownlineEmpId.Contains(x.employee_id));

                                List<int> emp_id = emp_dtl.Select(p => p.employee_id).ToList();

                                var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && emp_id.Contains(x.employee_id ?? 0) && !string.IsNullOrEmpty(x.employee_first_name)).ToList();


                                for (int Index = 0; Index < teos.Count; Index++)
                                {
                                    Classes.clsCalcualteDailyAttendance CalculateAttendance = new Classes.clsCalcualteDailyAttendance(
                                     _context, teos[Index].employee_id ?? 0, teos[Index].emp_official_section_id, teos[Index].location_id ?? 0,
                                     teos[Index].religion_id ?? 0, Convert.ToByte(teos[Index].is_fixed_weekly_off), PayrollDate, teos[Index].punch_type,
                                     teos[Index].is_ot_allowed, Convert.ToByte(teos[Index].is_comb_off_allowed), from_date1
                                     , to_date1, teos[Index].date_of_joining);

                                    var data = CalculateAttendance.Change_shift_dtl(false);


                                    var department_ = _context.tbl_department_master.Where(x => x.department_id == teos[Index].department_id).FirstOrDefault();
                                    var location_ = _context.tbl_location_master.Where(x => x.location_id == teos[Index].location_id).FirstOrDefault();
                                    var emp_code = _context.tbl_employee_master.Where(x => x.is_active == EmpType && x.employee_id == teos[Index].employee_id).FirstOrDefault();

                                    for (int j = 0; j < data.Count; j++)
                                    {

                                        var shift_dtl = _context.tbl_shift_details.Where(x => x.is_deleted == 0 && x.shift_details_id == data[j].ShiftDetailId).FirstOrDefault();

                                        if (location_id == teos[Index].location_id && dept_id == teos[Index].department_id)
                                        {
                                            shiftreport obj_shft = new shiftreport();
                                            obj_shft.effective_Dt = data[j].shiftt_dt;
                                            obj_shft.shift_in_time = Convert.ToDateTime(data[j].InTime);
                                            obj_shft.shift_out_time = Convert.ToDateTime(data[j].OutTime);
                                            obj_shft.emp_name = string.Format("{0} {1} {2}", teos[Index].employee_first_name, teos[Index].employee_middle_name, teos[Index].employee_last_name);
                                            obj_shft.shift_name = shift_dtl != null ? shift_dtl.shift_name : "";
                                            obj_shft.department_name = department_ != null ? department_.department_name : "";
                                            obj_shft.emp_id = teos[Index].employee_id ?? 0;
                                            obj_shft.location_name = location_ != null ? location_.location_name : "";
                                            obj_shft.emp_code = emp_code != null ? emp_code.emp_code : "";
                                            shiftlist.Add(obj_shft);
                                        }




                                    }
                                }

                            }


                        }
                        else
                        {
                            // List<shiftreport> shiftlist = new List<shiftreport>();

                            var emp_comp_dtl = _clsCurrentUser.CompanyId.ToList();//_context.tbl_employee_company_map.Where(x => x.is_deleted == 0).Select(p => p.company_id).Distinct().ToList();

                            for (int comp_index = 0; comp_index < emp_comp_dtl.Count; comp_index++)
                            {
                                var emp_dtl = _clsEmployeeDetail.GetEmployeeByDate(emp_comp_dtl[comp_index], Convert.ToDateTime("01-01-2000"), DateTime.Now.AddDays(1), 1, EmpType).Where(x => x.emp_status < 100 && _clsCurrentUser.DownlineEmpId.Contains(x.employee_id));

                                List<int> emp_id = emp_dtl.Select(p => p.employee_id).ToList();

                                var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && emp_id.Contains(x.employee_id ?? 0) && !string.IsNullOrEmpty(x.employee_first_name)).ToList();


                                for (int Index = 0; Index < teos.Count; Index++)
                                {
                                    Classes.clsCalcualteDailyAttendance CalculateAttendance = new Classes.clsCalcualteDailyAttendance(
                                     _context, teos[Index].employee_id ?? 0, teos[Index].emp_official_section_id, teos[Index].location_id ?? 0,
                                     teos[Index].religion_id ?? 0, Convert.ToByte(teos[Index].is_fixed_weekly_off), PayrollDate, teos[Index].punch_type,
                                     teos[Index].is_ot_allowed, Convert.ToByte(teos[Index].is_comb_off_allowed), from_date1
                                     , to_date1, teos[Index].date_of_joining);

                                    var data = CalculateAttendance.Change_shift_dtl(false);


                                    var department_ = _context.tbl_department_master.Where(x => x.department_id == teos[Index].department_id).FirstOrDefault();
                                    var location_ = _context.tbl_location_master.Where(x => x.location_id == teos[Index].location_id).FirstOrDefault();
                                    var emp_code = _context.tbl_employee_master.Where(x => x.is_active == EmpType && x.employee_id == teos[Index].employee_id).FirstOrDefault();
                                    for (int j = 0; j < data.Count; j++)
                                    {


                                        var shift_dtl = _context.tbl_shift_details.Where(x => x.is_deleted == 0 && x.shift_details_id == data[j].ShiftDetailId).FirstOrDefault();


                                        shiftreport obj_shft = new shiftreport();
                                        obj_shft.effective_Dt = data[j].shiftt_dt;
                                        obj_shft.shift_in_time = Convert.ToDateTime(data[j].InTime);
                                        obj_shft.shift_out_time = Convert.ToDateTime(data[j].OutTime);
                                        obj_shft.emp_name = string.Format("{0} {1} {2}", teos[Index].employee_first_name, teos[Index].employee_middle_name, teos[Index].employee_last_name);
                                        obj_shft.shift_name = shift_dtl != null ? shift_dtl.shift_name : "";
                                        obj_shft.department_name = department_ != null ? department_.department_name : "";
                                        obj_shft.emp_id = teos[Index].employee_id ?? 0;
                                        obj_shft.location_name = location_ != null ? location_.location_name : "";
                                        obj_shft.emp_code = emp_code != null ? emp_code.emp_code : "";
                                        shiftlist.Add(obj_shft);


                                    }
                                }

                            }



                        }

                        trans.Commit();

                        return Ok(shiftlist);
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
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }


        [Route("Shift_Change_Notification")]
        [HttpGet]
        //[Authorize(Policy ="9017")]
        public async Task<IActionResult> Shift_Change_Notification()
        {
            ResponseMsg objresponse = new ResponseMsg();
            try
            {
                Int32 PayrollDate = 1;
                var payroll = _context.tbl_payroll_month_setting.Where(p => p.is_deleted == 0).FirstOrDefault();
                if (payroll != null)
                {
                    // PayrollDate = Convert.ToInt32(payroll.from_date.ToString("dd"));
                    PayrollDate = Convert.ToInt32(payroll.from_date);
                }

                var companyids = _context.tbl_company_master.Where(x => x.is_active == 1).Select(p => p.company_id).ToList();
                List<int> empids = new List<int>();
                if (companyids.Count > 0)
                {
                    for (int i = 0; i < companyids.Count; i++)
                    {
                        var data = _clsEmployeeDetail.GetEmployeeByDate(companyids[i], new DateTime(2000, 1, 1), DateTime.Now.AddDays(1));
                        if (data.Count > 0)
                        {
                            empids.AddRange(data.Select(p => p.employee_id));
                        }
                    }

                }

                var teos = _context.tbl_emp_officaial_sec.Where(q => q.is_deleted == 0 && empids.Contains(q.employee_id ?? 0)).ToList();

                List<MailDetails> objmail_dtl = new List<MailDetails>();

                DateTime? fromdate; DateTime? todate;

                fromdate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy 00:00:00 tt"));
                todate = Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("dd-MMM-yyyy 00:00:00 tt"));

                if (teos.Count > 0)
                {
                    for (int Index = 0; Index < teos.Count; Index++)
                    {

                        MailDetails objmail = new MailDetails();

                        Classes.clsCalcualteDailyAttendance CalculateAttendance = new Classes.clsCalcualteDailyAttendance(
                        _context, teos[Index].employee_id ?? 0, teos[Index].emp_official_section_id, teos[Index].location_id ?? 0,
                        teos[Index].religion_id ?? 0, Convert.ToByte(teos[Index].is_fixed_weekly_off), PayrollDate, teos[Index].punch_type,
                        teos[Index].is_ot_allowed, Convert.ToByte(teos[Index].is_comb_off_allowed), fromdate.Value
                        , todate.Value, teos[Index].date_of_joining);


                        var att_dtl = CalculateAttendance.Change_shift_dtl(true).ToList();
                        if (att_dtl.Count > 0)
                        {
                            string shift_name = _context.tbl_shift_details.Where(x => x.is_deleted == 0 && x.shift_details_id == att_dtl[0].ShiftDetailId).FirstOrDefault().shift_name;

                            objmail.email_id = teos[Index].official_email_id;
                            objmail.punch_in = Convert.ToDateTime(att_dtl[0].InTime);
                            objmail.punch_out = Convert.ToDateTime(att_dtl[0].OutTime);
                            objmail.emp_id = teos[Index].employee_id ?? 0;
                            objmail.shift_name = shift_name;
                            objmail.effective_dt = att_dtl[0].shiftt_dt;
                            objmail.employee_first_name = string.Format("{0} {1} {2}", teos[Index].employee_first_name, teos[Index].employee_middle_name, teos[Index].employee_last_name);
                            //objmail.employee_code= string.Format("{0} {1} {2} ({3})", teos.employee_first_name, teos.employee_middle_name, teos.employee_last_name,data[Index].emp_code);


                            objmail_dtl.Add(objmail);
                        }


                    }


                }


                if (objmail_dtl.Count > 0)
                {
                    MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                    Task task = Task.Run(() => obj_ms.ShiftAssignmentNotification(objmail_dtl));
                    task.Wait();


                    objresponse.StatusCode = 1;
                    objresponse.Message = "Successfully";

                }
                return Ok(objresponse);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #endregion ** End by Supriya, on 22-04-2020, Shift Assingment **
    }
}
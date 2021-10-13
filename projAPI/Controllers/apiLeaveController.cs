using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
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
    public class apiLeaveController : ControllerBase
    {
        private readonly Context _context; 
        private IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IHttpContextAccessor _AC;
        private readonly IConfiguration _config;
        private readonly clsEmployeeDetail _clsEmployeeDetail;
        private readonly clsCurrentUser _clsCurrentUser;
        private readonly clsLeaveCredit _clsLeaveCredit;
        //public apiLeaveController(Context context)
        //{
        //    _context = context;
        //}
        public apiLeaveController(Context context, IHostingEnvironment environment, IOptions<AppSettings> appSettings, IHttpContextAccessor AC, IConfiguration config, clsEmployeeDetail _clsEmployeeDetail, clsCurrentUser _clsCurrentUser, clsLeaveCredit _clsLeaveCredit)
        {
            _context = context;
            _hostingEnvironment = environment;
            _appSettings = appSettings;
            _AC = AC;
            _config = config;
            this._clsEmployeeDetail = _clsEmployeeDetail;
            this._clsCurrentUser = _clsCurrentUser;
            this._clsLeaveCredit = _clsLeaveCredit;
        }

        // GET: api/apiLeave
        [HttpGet]
        //[Authorize(Policy = "5001")]
        public IEnumerable<tbl_leave_info> Gettbl_leave_info()
        {
            return _context.tbl_leave_info;
        }

        // GET: api/apiLeave/5
        [HttpGet("{id}")]
        ////[Authorize(Policy ="1")]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveSetting))]
        public async Task<IActionResult> Gettbl_leave_info([FromRoute] int id)
        {
            try
            {
                if (id > 0)
                {
                    tbl_leave_app_on_emp_type objleave_emp = new tbl_leave_app_on_emp_type();
                    tbl_leave_appcbl_on_company objappcompany = new tbl_leave_appcbl_on_company();
                    tbl_leave_app_on_dept objdept = new tbl_leave_app_on_dept();
                    tbl_leave_appcbl_on_religion objreligion = new tbl_leave_appcbl_on_religion();

                    tbl_leave_info objleaveinfo = _context.tbl_leave_info.Where(x => x.leave_info_id == id).FirstOrDefault();
                    tbl_leave_applicablity objapplicability = _context.tbl_leave_applicablity.Where(x => x.is_deleted == 0 && x.leave_info_id == objleaveinfo.leave_info_id).FirstOrDefault();
                    if (objapplicability != null)
                    {
                        objleave_emp = _context.tbl_leave_app_on_emp_type.Where(x => x.is_deleted == 0 && x.l_app_id == objapplicability.leave_applicablity_id).FirstOrDefault();
                        objappcompany = _context.tbl_leave_appcbl_on_company.Where(x => x.is_deleted == 0 && x.l_a_id == objapplicability.leave_applicablity_id).FirstOrDefault();
                        objdept = _context.tbl_leave_app_on_dept.Where(x => x.is_deleted == 0 && x.lid == objapplicability.leave_applicablity_id).FirstOrDefault();
                        objreligion = _context.tbl_leave_appcbl_on_religion.Where(x => x.is_deleted == 0 && x.l_app_id == objapplicability.leave_applicablity_id).FirstOrDefault();
                    }
                    else
                    {
                        objleave_emp = null;
                        objappcompany = null;
                        objdept = null;
                        objreligion = null;
                    }

                    //tbl_leave_app_on_emp_type tlaemp = _context.tbl_leave_app_on_emp_type.Where(p => p.l_app_id == Convert.ToInt32(objapplicability.leave_applicablity_id) && p.is_deleted == 0).FirstOrDefault();
                    //tbl_leave_appcbl_on_company tlacomp = _context.tbl_leave_appcbl_on_company.Where(p => p.l_a_id== Convert.ToInt32(objapplicability.leave_applicablity_id) && p.is_deleted == 0).FirstOrDefault();
                    //tbl_leave_app_on_dept tladept = _context.tbl_leave_app_on_dept.Where(p => p.lid== Convert.ToInt32(objapplicability.leave_applicablity_id) && p.is_deleted == 0).FirstOrDefault();
                    //tbl_leave_appcbl_on_religion tlareligion = _context.tbl_leave_appcbl_on_religion.Where(p => p.l_app_id== Convert.ToInt32(objapplicability.leave_applicablity_id) && p.is_deleted == 0).FirstOrDefault();

                    tbl_leave_credit objlcredit = _context.tbl_leave_credit.Where(x => x.is_deleted == 0 && x.leave_info_id == objleaveinfo.leave_info_id).FirstOrDefault();
                    tbl_leave_rule objlrule = _context.tbl_leave_rule.Where(x => x.is_deleted == 0 && x.leave_info_id == objleaveinfo.leave_info_id).FirstOrDefault();
                    tbl_leave_cashable objlcash = _context.tbl_leave_cashable.OrderByDescending(x => x.leave_cashable_id).Where(x => x.is_deleted == 0 && x.leave_info_id == objleaveinfo.leave_info_id).FirstOrDefault();

                    //set cretificate path
                    //string apipath = "http://localhost:44384";
                    //var path = Path.Combine(Directory.GetCurrentDirectory(), objlcredit.certificate_path);
                    //objlcredit.certificate_path = path.ToString();

                    var data = new
                    {
                        objleaveinfo,
                        objapplicability,
                        objleave_emp,
                        objappcompany,
                        objdept,
                        objreligion,
                        objlcredit,
                        objlrule,
                        objlcash

                    };

                    return Ok(data);
                }
                else
                {
                    var data = (from a in _context.tbl_leave_info
                                join b in _context.tbl_leave_type
                                on a.leave_type_id equals b.leave_type_id
                                select new
                                {
                                    a.leave_type_id,
                                    a.leave_info_id,
                                    a.leave_tenure_from_date,
                                    a.leave_tenure_to_date,
                                    a.leave_type,
                                    b.leave_type_name,
                                    b.description,
                                    a.is_active
                                }).AsEnumerable().Select((a, index) => new
                                {
                                    sno = index + 1,
                                    leave_type_id = a.leave_type_id,
                                    leave_info_id = a.leave_info_id,
                                    leave_tenure_to_date = a.leave_tenure_to_date,
                                    leave_tenure_from_date = a.leave_tenure_from_date,
                                    leave_type = a.leave_type,
                                    leave_type_name = a.leave_type_name,
                                    Status = a.is_active

                                }).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        // PUT: api/apiLeave/5
        [Route("Puttbl_leave_info")]
        [HttpPost]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveSetting))]
        public async Task<IActionResult> Puttbl_leave_info()
        {
            Response_Msg objResult = new Response_Msg();
            try
            {
                // var files1 = HttpContext.Request.Form.Files["name"];
                var files = HttpContext.Request.Form.Files;
                var a = HttpContext.Request.Form["AllData"];
                if (a.ToString() == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid json data please try again !!";
                    return Ok(objResult);
                }



                CommonClass com = new CommonClass();
                LeaveInfo_ApplicabilityModel ObjData = new LeaveInfo_ApplicabilityModel();

                ObjData = com.ToObjectFromJSON<LeaveInfo_ApplicabilityModel>(a.ToString());



                int LeaveInfoId = Convert.ToInt32(ObjData.leave_info_id);
                byte LeaveType = Convert.ToByte(ObjData.leave_type);
                int LeaveTypeId = Convert.ToByte(ObjData.leave_type_id);

                string leave_tenure_from = ObjData.leave_tenure_from_date;   // American style
                DateTime leave_tenure_from_date = DateTime.Parse(leave_tenure_from);

                string leave_tenure_to = ObjData.leave_tenure_to_date;   // American style
                DateTime leave_tenure_to_date = DateTime.Parse(leave_tenure_to);


                tbl_leave_info tbl_leave_info_ = _context.tbl_leave_info.FirstOrDefault(p => p.leave_info_id == LeaveInfoId);
                if (tbl_leave_info_ == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid leave info id please try again !!";
                    return Ok(objResult);
                }
                else
                {
                    var exist_detail1 = _context.tbl_leave_info.OrderByDescending(y => y.leave_info_id).Where(x => x.leave_info_id == LeaveInfoId &&
                        ((x.leave_tenure_from_date.Date <= Convert.ToDateTime(ObjData.leave_tenure_from_date).Date &&
                        Convert.ToDateTime(ObjData.leave_tenure_from_date).Date <= x.leave_tenure_to_date.Date) ||
                        (x.leave_tenure_from_date.Date <= Convert.ToDateTime(ObjData.leave_tenure_to_date).Date && Convert.ToDateTime(ObjData.leave_tenure_to_date).Date <= x.leave_tenure_to_date.Date)
                         || (Convert.ToDateTime(ObjData.leave_tenure_from_date).Date <= x.leave_tenure_from_date.Date && x.leave_tenure_to_date.Date <= Convert.ToDateTime(ObjData.leave_tenure_to_date).Date))
                        && x.leave_type_id == LeaveTypeId
                    ).FirstOrDefault();




                    //var exist_detail1 = _context.tbl_leave_appcbl_on_company.Where(x => x.tbl_leave_applicablity.tbl_leave_info.leave_info_id == LeaveInfoId &&
                    //x.tbl_leave_applicablity.tbl_leave_info.leave_tenure_from_date.Date == Convert.ToDateTime(ObjData.leave_tenure_from_date).Date &&
                    //  x.tbl_leave_applicablity.tbl_leave_info.leave_tenure_to_date.Date == Convert.ToDateTime(ObjData.leave_tenure_to_date).Date &&
                    // x.tbl_leave_applicablity.tbl_leave_info.leave_type_id == LeaveTypeId &&
                    // x.tbl_company_master.company_id == Convert.ToInt32(ObjData.is_aplicable_on_all_company)
                    //  // && x.tbl_leave_applicablity.tbl_leave_info.is_active==1 
                    //  ).OrderByDescending(y => y.tbl_leave_applicablity.tbl_leave_info.leave_info_id).FirstOrDefault();

                    if (exist_detail1 != null)
                    {
                        update_tbl_leave_info(ObjData);

                        objResult.StatusCode = 0;
                        objResult.Message = "Leave info updated successfully !!";
                    }
                    else
                    {
                        var Leaveinfo = _context.tbl_leave_info.Where(x => x.leave_info_id != LeaveInfoId && x.leave_type_id == LeaveTypeId &&
                       ((x.leave_tenure_from_date.Date <= Convert.ToDateTime(ObjData.leave_tenure_from_date).Date && Convert.ToDateTime(ObjData.leave_tenure_from_date).Date <= x.leave_tenure_to_date.Date) || (x.leave_tenure_from_date.Date <= Convert.ToDateTime(ObjData.leave_tenure_to_date).Date
                       && Convert.ToDateTime(ObjData.leave_tenure_to_date).Date <= x.leave_tenure_to_date.Date)
                    || (Convert.ToDateTime(ObjData.leave_tenure_from_date).Date <= x.leave_tenure_from_date.Date && x.leave_tenure_to_date.Date <= Convert.ToDateTime(ObjData.leave_tenure_to_date).Date))
                        ).FirstOrDefault();


                        //    var Leaveinfo = _context.tbl_leave_appcbl_on_company.Where(b =>b.tbl_leave_applicablity.leave_info_id!=LeaveInfoId && b.tbl_leave_applicablity.tbl_leave_info.leave_type_id == LeaveTypeId
                        //&& b.tbl_leave_applicablity.tbl_leave_info.leave_tenure_from_date.Date >= Convert.ToDateTime(ObjData.leave_tenure_from_date).Date
                        //&& b.tbl_leave_applicablity.tbl_leave_info.leave_tenure_to_date.Date <= Convert.ToDateTime(ObjData.leave_tenure_to_date).Date && b.c_id == Convert.ToInt32(ObjData.is_aplicable_on_all_company)).OrderByDescending(k => k.tbl_leave_applicablity.leave_applicablity_id).FirstOrDefault();

                        //var check_leave_type_id_change = _context.tbl_leave_info.Where(x => x.leave_info_id == LeaveInfoId && x.leave_type_id == LeaveTypeId).OrderByDescending(y => y.leave_info_id).FirstOrDefault();
                        //if (Leaveinfo == null)
                        if (Leaveinfo != null)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Leave Type Cannot be Change / Leave Type already exist...";
                        }
                        else
                        {

                            //var duplicate_yr_detail = _context.tbl_leave_appcbl_on_company.Where(x => x.tbl_leave_applicablity.tbl_leave_info.leave_type_id == LeaveTypeId
                            //  && x.tbl_leave_applicablity.tbl_leave_info.leave_type == LeaveType && x.tbl_leave_applicablity.tbl_leave_info.leave_info_id != LeaveInfoId &&
                            //  x.tbl_company_master.company_id == Convert.ToInt32(ObjData.is_aplicable_on_all_company)).ToList();

                            //if (duplicate_yr_detail.Count > 0)
                            //{
                            //    var duplicate_ = duplicate_yr_detail.Where(x => Convert.ToDateTime(x.tbl_leave_applicablity.tbl_leave_info.leave_tenure_from_date) >= leave_tenure_from_date && Convert.ToDateTime(x.tbl_leave_applicablity.tbl_leave_info.leave_tenure_to_date) <= leave_tenure_to_date).FirstOrDefault();
                            //    if (duplicate_ != null)
                            //    {
                            //        objResult.StatusCode = 1;
                            //        objResult.Message = "Leave info duplicate found please check & try !!";
                            //    }
                            //}
                            //else
                            //{
                            update_tbl_leave_info(ObjData);
                            objResult.StatusCode = 0;
                            objResult.Message = "Leave info updated successfully !!";
                            //}

                        }

                    }


                    //check for duplicate
                    //var duplicate = _context.tbl_leave_info.Where(x => x.leave_type_id == LeaveTypeId
                    //&& x.leave_type == LeaveType && x.leave_info_id != LeaveInfoId).ToList();

                    //if (duplicate.Count > 0)
                    //{
                    //    var duplicate_ = duplicate.Where(x => Convert.ToDateTime(x.leave_tenure_from_date) >= leave_tenure_from_date && Convert.ToDateTime(x.leave_tenure_to_date) <= leave_tenure_to_date).FirstOrDefault();


                    //    if (duplicate_ != null)
                    //    {
                    //        objResult.StatusCode = 1;
                    //        objResult.Message = "Leave info duplicate found please check & try !!";
                    //        return Ok(objResult);
                    //    }
                    //}
                }






                //using (var tran = _context.Database.BeginTransaction())
                //{
                //    try
                //    {
                //        List<tbl_leave_applicablity> tla = _context.tbl_leave_applicablity.Where(p => p.leave_info_id == LeaveInfoId && p.is_deleted == 0).ToList();
                //        tla.ForEach(p => { p.is_deleted = 1; });
                //        _context.tbl_leave_applicablity.UpdateRange(tla);
                //        _context.SaveChanges();
                //        int[] leApList = tla.Select(p => p.leave_applicablity_id).ToArray();
                //        List<tbl_leave_app_on_emp_type> tlae = _context.tbl_leave_app_on_emp_type.Where(p => leApList.Contains(p.l_app_id ?? 0) && p.is_deleted == 0).ToList();
                //        tlae.ForEach(p => { p.is_deleted = 1; });
                //        _context.tbl_leave_app_on_emp_type.UpdateRange(tlae);
                //        List<tbl_leave_appcbl_on_company> tlac = _context.tbl_leave_appcbl_on_company.Where(p => leApList.Contains(p.l_a_id ?? 0) && p.is_deleted == 0).ToList();
                //        tlac.ForEach(p => { p.is_deleted = 1; });
                //        _context.tbl_leave_appcbl_on_company.UpdateRange(tlac);
                //        List<tbl_leave_app_on_dept> tlad = _context.tbl_leave_app_on_dept.Where(p => leApList.Contains(p.lid ?? 0) && p.is_deleted == 0).ToList();
                //        tlad.ForEach(p => { p.is_deleted = 1; });
                //        _context.tbl_leave_app_on_dept.UpdateRange(tlad);
                //        List<tbl_leave_appcbl_on_religion> tlar = _context.tbl_leave_appcbl_on_religion.Where(p => leApList.Contains(p.l_app_id ?? 0) && p.is_deleted == 0).ToList();
                //        tlar.ForEach(p => { p.is_deleted = 1; });
                //        _context.tbl_leave_appcbl_on_religion.UpdateRange(tlar);
                //        List<tbl_leave_credit> tlc = _context.tbl_leave_credit.Where(p => LeaveInfoId == p.leave_info_id && p.is_deleted == 0).ToList();
                //        tlc.ForEach(p => { p.is_deleted = 1; });
                //        _context.tbl_leave_credit.UpdateRange(tlc);
                //        List<tbl_leave_rule> tlru = _context.tbl_leave_rule.Where(p => LeaveInfoId == p.leave_info_id && p.is_deleted == 0).ToList();
                //        tlru.ForEach(p => { p.is_deleted = 1; });
                //        _context.tbl_leave_rule.UpdateRange(tlru);
                //        List<tbl_leave_cashable> tlca = _context.tbl_leave_cashable.Where(p => LeaveInfoId == p.leave_info_id && p.is_deleted == 0).ToList();
                //        tlca.ForEach(p => { p.is_deleted = 1; });
                //        _context.tbl_leave_cashable.UpdateRange(tlca);


                //        tbl_leave_info_.leave_tenure_from_date = leave_tenure_from_date;
                //        tbl_leave_info_.leave_tenure_to_date = leave_tenure_to_date;
                //        tbl_leave_info_.leave_type = LeaveType;
                //        tbl_leave_info_.leave_type_id = LeaveTypeId;
                //        tbl_leave_info_.is_active = Convert.ToByte(ObjData.is_active);
                //        _context.Entry(tbl_leave_info_).State = EntityState.Modified;
                //        _context.SaveChanges();
                //        //add into the Log table
                //        tbl_leave_applicablity tla_ = new tbl_leave_applicablity();

                //        tla_.leave_info_id = tbl_leave_info_.leave_info_id;
                //        if (Convert.ToInt32(ObjData.is_aplicable_on_all_company) == 0)
                //        {
                //            tla_.is_aplicable_on_all_location = 1;
                //            tla_.is_aplicable_on_all_company = 1;
                //        }
                //        else if (Convert.ToInt32(ObjData.is_aplicable_on_all_company) > 0
                //            && Convert.ToInt32(ObjData.is_aplicable_on_all_location) == 0)
                //        {
                //            tla_.is_aplicable_on_all_location = 1;
                //            tla_.is_aplicable_on_all_company = 0;

                //        }
                //        else
                //        {
                //            tla_.is_aplicable_on_all_location = 0;
                //            tla_.is_aplicable_on_all_company = 0;
                //        }

                //        if (Convert.ToInt32(ObjData.is_aplicable_on_all_department) == 0)
                //        {
                //            tla_.is_aplicable_on_all_department = 1;
                //        }
                //        else
                //        {
                //            tla_.is_aplicable_on_all_department = 0;
                //        }

                //        if (Convert.ToInt32(ObjData.is_aplicable_on_all_religion) == 0)
                //        {
                //            tla_.is_aplicable_on_all_religion = 1;
                //        }
                //        else
                //        {
                //            tla_.is_aplicable_on_all_religion = 0;
                //        }

                //        if (Convert.ToInt32(ObjData.is_aplicable_on_all_emp_type) == 0)
                //        {
                //            tla_.is_aplicable_on_all_emp_type = 1;
                //        }
                //        else
                //        {
                //            tla_.is_aplicable_on_all_emp_type = 0;
                //        }

                //        tla_.leave_applicable_for = Convert.ToByte(ObjData.leave_applicable_for);
                //        tla_.day_part = Convert.ToByte(ObjData.day_part);
                //        if (Convert.ToByte(ObjData.leave_applicable_for) == 3)
                //        {
                //            tla_.leave_applicable_in_hours_and_minutes = Convert.ToDateTime(ObjData.leave_applicable_in_hours_and_minutes);
                //        }
                //        tla_.employee_can_apply = Convert.ToByte(ObjData.employee_can_apply);
                //        tla_.admin_can_apply = Convert.ToByte(ObjData.admin_can_apply);
                //        tla_.is_deleted = 0;
                //        _context.tbl_leave_applicablity.Add(tla_);
                //        _context.SaveChanges();

                //        if (tla_.is_aplicable_on_all_company == 0 || tla_.is_aplicable_on_all_location == 0)
                //        {
                //            tbl_leave_appcbl_on_company tlac_ = new tbl_leave_appcbl_on_company();
                //            tlac_.c_id = Convert.ToInt32(ObjData.is_aplicable_on_all_company);
                //            if (tla_.is_aplicable_on_all_location == 0)
                //            {
                //                tlac_.location_id = Convert.ToInt32(ObjData.is_aplicable_on_all_location);
                //            }
                //            tlac_.is_deleted = 0;
                //            tlac_.l_a_id = tla_.leave_applicablity_id;
                //            _context.tbl_leave_appcbl_on_company.Add(tlac_);
                //            _context.SaveChanges();

                //        }
                //        if (tla_.is_aplicable_on_all_department == 0)
                //        {
                //            tbl_leave_app_on_dept Temdata = new tbl_leave_app_on_dept();
                //            Temdata.id = Convert.ToInt32(ObjData.is_aplicable_on_all_department);
                //            Temdata.is_deleted = 0;
                //            Temdata.lid = tla_.leave_applicablity_id;
                //            _context.tbl_leave_app_on_dept.Add(Temdata);
                //            _context.SaveChanges();
                //        }

                //        if (tla_.is_aplicable_on_all_religion == 0)
                //        {
                //            tbl_leave_appcbl_on_religion Temdata = new tbl_leave_appcbl_on_religion();
                //            Temdata.religion_id = Convert.ToInt32(ObjData.is_aplicable_on_all_religion);
                //            Temdata.is_deleted = 0;
                //            Temdata.l_app_id = tla_.leave_applicablity_id;
                //            _context.tbl_leave_appcbl_on_religion.Add(Temdata);
                //            _context.SaveChanges();
                //        }

                //        if (tla_.is_aplicable_on_all_emp_type == 0)
                //        {
                //            tbl_leave_app_on_emp_type Temdata = new tbl_leave_app_on_emp_type();
                //            Temdata.employment_type = Convert.ToByte(ObjData.is_aplicable_on_all_emp_type);
                //            Temdata.is_deleted = 0;
                //            Temdata.l_app_id = tla_.leave_applicablity_id;
                //            _context.tbl_leave_app_on_emp_type.Add(Temdata);
                //            _context.SaveChanges();
                //        }

                //        tbl_leave_credit tlca_ = new tbl_leave_credit();
                //        tlca_.leave_info_id = tla_.leave_info_id;
                //        tlca_.frequency_type = Convert.ToByte(ObjData.frequency_type);
                //        tlca_.leave_credit_day = Convert.ToByte(ObjData.leave_credit_day);
                //        tlca_.leave_credit_number = Convert.ToDouble(ObjData.leave_credit_number);
                //        tlca_.is_half_day_applicable = Convert.ToByte(ObjData.is_half_day_applicable);
                //        tlca_.is_applicable_for_advance = Convert.ToByte(ObjData.is_applicable_for_advance);
                //        tlca_.advance_applicable_day = Convert.ToByte(ObjData.advance_applicable_day);
                //        tlca_.is_leave_accrue = Convert.ToByte(ObjData.is_leave_accrue);
                //        tlca_.max_accrue = Convert.ToByte(ObjData.max_accrue);
                //        tlca_.is_required_certificate = Convert.ToByte(ObjData.is_required_certificate);
                //        //tlca_.certificate_path = files == null;//|| files.Count == 0 ? creditexit.certificate_path : ObjData.certificate_path;
                //        tlca_.is_deleted = 0;
                //        _context.tbl_leave_credit.Add(tlca_);
                //        _context.SaveChanges();


                //        tbl_leave_rule tlra_ = new tbl_leave_rule();
                //        tlra_.leave_info_id = tla_.leave_info_id;
                //        tlra_.applicable_if_employee_joined_before_dt = Convert.ToDateTime(ObjData.applicable_if_employee_joined_before_dt);
                //        tlra_.applied_sandwich_rule = Convert.ToByte(ObjData.applied_sandwich_rule);
                //        tlra_.can_carried_forward = Convert.ToByte(ObjData.can_carried_forward);
                //        tlra_.certificate_require_for_min_no_of_day = Convert.ToByte(ObjData.certificate_require_for_min_no_of_day);
                //        tlra_.maximum_carried_forward = Convert.ToByte(ObjData.maximum_carried_forward);
                //        tlra_.maximum_leave_can_be_in_day = Convert.ToByte(ObjData.maximum_leave_can_be_in_day);
                //        tlra_.maximum_leave_can_be_taken = Convert.ToByte(ObjData.maximum_leave_can_be_taken);
                //        tlra_.maximum_leave_can_be_taken_in_quater = Convert.ToByte(ObjData.maximum_leave_can_be_taken_in_quater);
                //        tlra_.maximum_leave_can_be_taken_type = Convert.ToByte(ObjData.maximum_leave_can_be_taken_type);
                //        tlra_.maximum_leave_clubbed_in_tenure_number_of_leave = Convert.ToByte(ObjData.maximum_leave_clubbed_in_tenure_number_of_leave);
                //        tlra_.maxi_negative_leave_applicable = Convert.ToByte(ObjData.maxi_negative_leave_applicable);
                //        tlra_.minimum_leave_applicable = Convert.ToByte(ObjData.minimum_leave_applicable);
                //        tlra_.number_maximum_negative_leave_balance = Convert.ToByte(ObjData.number_maximum_negative_leave_balance);
                //        tlra_.is_deleted = 0;
                //        _context.tbl_leave_rule.Add(tlra_);
                //        _context.SaveChanges();


                //        tbl_leave_cashable tlc_ = new tbl_leave_cashable();
                //        tlc_.leave_info_id = tla_.leave_info_id;
                //        tlc_.maximum_cashable_leave = Convert.ToByte(ObjData.maximum_cashable_leave);
                //        tlc_.is_cashable = Convert.ToByte(ObjData.is_cashable);
                //        tlc_.cashable_type = Convert.ToByte(ObjData.cashable_type);
                //        tlc_.cashable_after_year = Convert.ToByte(ObjData.cashable_after_year);
                //        tlc_.is_deleted = 0;
                //        _context.tbl_leave_cashable.Add(tlc_);
                //        _context.SaveChanges();

                //        //foreach (var FileData in files)
                //        //{
                //        //    if (FileData != null && FileData.Length > 0)
                //        //    {
                //        //        var allowedExtensions = new[] { ".Jpg", ".pdf", "jpeg", ".jpg", ".Pdf", "Jpeg" };

                //        //        var ext = Path.GetExtension(FileData.FileName); //getting the extension
                //        //        if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                //        //        {
                //        //            string name = Path.GetFileNameWithoutExtension(FileData.FileName); //getting file name without extension  
                //        //            string MyFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ext; //Guid.NewGuid().ToString().Replace("-", "") +

                //        //            var webRoot = _hostingEnvironment.WebRootPath;

                //        //            if (!Directory.Exists(webRoot + "/LeaveCredit/Certificate/"))
                //        //            {
                //        //                Directory.CreateDirectory(webRoot + "/LeaveCredit/Certificate/");
                //        //            }

                //        //            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/LeaveCredit/Certificate/");
                //        //            //save file
                //        //            using (var fileStream = new FileStream(Path.Combine(path, MyFileName), FileMode.Create))
                //        //            {
                //        //                FileData.CopyTo(fileStream);
                //        //                ObjData.certificate_path = "/wwwroot/LeaveCredit/Certificate/" + MyFileName;
                //        //            }
                //        //        }
                //        //    }
                //        //}
                //        tran.Commit();
                //        objResult.StatusCode = 0;
                //        objResult.Message = "Leave info updated successfully !!";

                //    }
                //    catch (Exception ex)
                //    {
                //        objResult.StatusCode = 1;
                //        objResult.Message = ex.Message;
                //       // tran.Rollback();
                //    }
                //}




                return Ok(objResult);


            }
            catch (DbUpdateConcurrencyException ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }


        //supriya
        [Authorize(Policy = nameof(enmMenuMaster.LeaveSetting))]
        public void update_tbl_leave_info(LeaveInfo_ApplicabilityModel ObjData)
        {
            //LeaveInfo_ApplicabilityModel ObjData = new LeaveInfo_ApplicabilityModel();

            int LeaveInfoId = Convert.ToInt32(ObjData.leave_info_id);
            byte LeaveType = Convert.ToByte(ObjData.leave_type);
            int LeaveTypeId = Convert.ToByte(ObjData.leave_type_id);


            string leave_tenure_from = ObjData.leave_tenure_from_date;   // American style
            DateTime leave_tenure_from_date = DateTime.Parse(leave_tenure_from);

            string leave_tenure_to = ObjData.leave_tenure_to_date;   // American style
            DateTime leave_tenure_to_date = DateTime.Parse(leave_tenure_to);


            tbl_leave_info tbl_leave_info_ = _context.tbl_leave_info.FirstOrDefault(p => p.leave_info_id == LeaveInfoId);

            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    List<tbl_leave_applicablity> tla = _context.tbl_leave_applicablity.Where(p => p.leave_info_id == LeaveInfoId && p.is_deleted == 0).ToList();
                    tla.ForEach(p => { p.is_deleted = 1; });
                    _context.tbl_leave_applicablity.UpdateRange(tla);
                    _context.SaveChanges();
                    int[] leApList = tla.Select(p => p.leave_applicablity_id).ToArray();
                    List<tbl_leave_app_on_emp_type> tlae = _context.tbl_leave_app_on_emp_type.Where(p => leApList.Contains(p.l_app_id ?? 0) && p.is_deleted == 0).ToList();
                    tlae.ForEach(p => { p.is_deleted = 1; });
                    _context.tbl_leave_app_on_emp_type.UpdateRange(tlae);
                    List<tbl_leave_appcbl_on_company> tlac = _context.tbl_leave_appcbl_on_company.Where(p => leApList.Contains(p.l_a_id ?? 0) && p.is_deleted == 0).ToList();
                    tlac.ForEach(p => { p.is_deleted = 1; });
                    _context.tbl_leave_appcbl_on_company.UpdateRange(tlac);
                    List<tbl_leave_app_on_dept> tlad = _context.tbl_leave_app_on_dept.Where(p => leApList.Contains(p.lid ?? 0) && p.is_deleted == 0).ToList();
                    tlad.ForEach(p => { p.is_deleted = 1; });
                    _context.tbl_leave_app_on_dept.UpdateRange(tlad);
                    List<tbl_leave_appcbl_on_religion> tlar = _context.tbl_leave_appcbl_on_religion.Where(p => leApList.Contains(p.l_app_id ?? 0) && p.is_deleted == 0).ToList();
                    tlar.ForEach(p => { p.is_deleted = 1; });
                    _context.tbl_leave_appcbl_on_religion.UpdateRange(tlar);
                    List<tbl_leave_credit> tlc = _context.tbl_leave_credit.Where(p => LeaveInfoId == p.leave_info_id && p.is_deleted == 0).ToList();
                    tlc.ForEach(p => { p.is_deleted = 1; });
                    _context.tbl_leave_credit.UpdateRange(tlc);
                    List<tbl_leave_rule> tlru = _context.tbl_leave_rule.Where(p => LeaveInfoId == p.leave_info_id && p.is_deleted == 0).ToList();
                    tlru.ForEach(p => { p.is_deleted = 1; });
                    _context.tbl_leave_rule.UpdateRange(tlru);
                    List<tbl_leave_cashable> tlca = _context.tbl_leave_cashable.Where(p => LeaveInfoId == p.leave_info_id && p.is_deleted == 0).ToList();
                    tlca.ForEach(p => { p.is_deleted = 1; });
                    _context.tbl_leave_cashable.UpdateRange(tlca);


                    tbl_leave_info_.leave_tenure_from_date = leave_tenure_from_date;
                    tbl_leave_info_.leave_tenure_to_date = leave_tenure_to_date;
                    tbl_leave_info_.leave_type = LeaveType;
                    tbl_leave_info_.leave_type_id = LeaveTypeId;
                    tbl_leave_info_.is_active = Convert.ToByte(ObjData.is_active);
                    _context.Entry(tbl_leave_info_).State = EntityState.Modified;
                    _context.SaveChanges();
                    //add into the Log table
                    tbl_leave_applicablity tla_ = new tbl_leave_applicablity();

                    tla_.leave_info_id = tbl_leave_info_.leave_info_id;
                    if (Convert.ToInt32(ObjData.is_aplicable_on_all_company) == 0)
                    {
                        tla_.is_aplicable_on_all_location = 1;
                        tla_.is_aplicable_on_all_company = 1;
                    }
                    else if (Convert.ToInt32(ObjData.is_aplicable_on_all_company) > 0
                        && Convert.ToInt32(ObjData.is_aplicable_on_all_location) == 0)
                    {
                        tla_.is_aplicable_on_all_location = 1;
                        tla_.is_aplicable_on_all_company = 0;

                    }
                    else
                    {
                        tla_.is_aplicable_on_all_location = 0;
                        tla_.is_aplicable_on_all_company = 0;
                    }

                    if (Convert.ToInt32(ObjData.is_aplicable_on_all_department) == 0)
                    {
                        tla_.is_aplicable_on_all_department = 1;
                    }
                    else
                    {
                        tla_.is_aplicable_on_all_department = 0;
                    }

                    if (Convert.ToInt32(ObjData.is_aplicable_on_all_religion) == 0)
                    {
                        tla_.is_aplicable_on_all_religion = 1;
                    }
                    else
                    {
                        tla_.is_aplicable_on_all_religion = 0;
                    }

                    if (Convert.ToInt32(ObjData.is_aplicable_on_all_emp_type) == 0)
                    {
                        tla_.is_aplicable_on_all_emp_type = 1;
                    }
                    else
                    {
                        tla_.is_aplicable_on_all_emp_type = 0;
                    }

                    tla_.leave_applicable_for = Convert.ToByte(ObjData.leave_applicable_for);
                    tla_.day_part = Convert.ToByte(ObjData.day_part);
                    if (Convert.ToByte(ObjData.leave_applicable_for) == 3)
                    {
                        tla_.leave_applicable_in_hours_and_minutes = Convert.ToDateTime(ObjData.leave_applicable_in_hours_and_minutes);
                    }
                    tla_.employee_can_apply = Convert.ToByte(ObjData.employee_can_apply);
                    tla_.admin_can_apply = Convert.ToByte(ObjData.admin_can_apply);
                    tla_.is_deleted = 0;
                    _context.tbl_leave_applicablity.Add(tla_);
                    _context.SaveChanges();

                    if (tla_.is_aplicable_on_all_company == 0 || tla_.is_aplicable_on_all_location == 0)
                    {
                        tbl_leave_appcbl_on_company tlac_ = new tbl_leave_appcbl_on_company();
                        tlac_.c_id = Convert.ToInt32(ObjData.is_aplicable_on_all_company);
                        if (tla_.is_aplicable_on_all_location == 0)
                        {
                            tlac_.location_id = Convert.ToInt32(ObjData.is_aplicable_on_all_location);
                        }
                        tlac_.is_deleted = 0;
                        tlac_.l_a_id = tla_.leave_applicablity_id;
                        _context.tbl_leave_appcbl_on_company.Add(tlac_);
                        _context.SaveChanges();

                    }
                    if (tla_.is_aplicable_on_all_department == 0)
                    {
                        tbl_leave_app_on_dept Temdata = new tbl_leave_app_on_dept();
                        Temdata.id = Convert.ToInt32(ObjData.is_aplicable_on_all_department);
                        Temdata.is_deleted = 0;
                        Temdata.lid = tla_.leave_applicablity_id;
                        _context.tbl_leave_app_on_dept.Add(Temdata);
                        _context.SaveChanges();
                    }

                    if (tla_.is_aplicable_on_all_religion == 0)
                    {
                        tbl_leave_appcbl_on_religion Temdata = new tbl_leave_appcbl_on_religion();
                        Temdata.religion_id = Convert.ToInt32(ObjData.is_aplicable_on_all_religion);
                        Temdata.is_deleted = 0;
                        Temdata.l_app_id = tla_.leave_applicablity_id;
                        _context.tbl_leave_appcbl_on_religion.Add(Temdata);
                        _context.SaveChanges();
                    }

                    if (tla_.is_aplicable_on_all_emp_type == 0)
                    {
                        tbl_leave_app_on_emp_type Temdata = new tbl_leave_app_on_emp_type();
                        Temdata.employment_type = Convert.ToByte(ObjData.is_aplicable_on_all_emp_type);
                        Temdata.is_deleted = 0;
                        Temdata.l_app_id = tla_.leave_applicablity_id;
                        _context.tbl_leave_app_on_emp_type.Add(Temdata);
                        _context.SaveChanges();
                    }

                    tbl_leave_credit tlca_ = new tbl_leave_credit();
                    tlca_.leave_info_id = tla_.leave_info_id;
                    tlca_.frequency_type = Convert.ToByte(ObjData.frequency_type);
                    tlca_.leave_credit_day = Convert.ToByte(ObjData.leave_credit_day);
                    tlca_.leave_credit_number = Convert.ToDouble(ObjData.leave_credit_number);
                    tlca_.is_half_day_applicable = Convert.ToByte(ObjData.is_half_day_applicable);
                    tlca_.is_applicable_for_advance = Convert.ToByte(ObjData.is_applicable_for_advance);
                    tlca_.advance_applicable_day = Convert.ToByte(ObjData.advance_applicable_day);
                    tlca_.is_leave_accrue = Convert.ToByte(ObjData.is_leave_accrue);
                    tlca_.max_accrue = Convert.ToByte(ObjData.max_accrue);
                    tlca_.is_required_certificate = Convert.ToByte(ObjData.is_required_certificate);
                    //tlca_.certificate_path = files == null;//|| files.Count == 0 ? creditexit.certificate_path : ObjData.certificate_path;
                    tlca_.is_deleted = 0;
                    _context.tbl_leave_credit.Add(tlca_);
                    _context.SaveChanges();


                    tbl_leave_rule tlra_ = new tbl_leave_rule();
                    tlra_.leave_info_id = tla_.leave_info_id;
                    tlra_.applicable_if_employee_joined_before_dt = Convert.ToDateTime(ObjData.applicable_if_employee_joined_before_dt);
                    tlra_.applied_sandwich_rule = Convert.ToByte(ObjData.applied_sandwich_rule);
                    tlra_.can_carried_forward = Convert.ToByte(ObjData.can_carried_forward);
                    tlra_.certificate_require_for_min_no_of_day = Convert.ToByte(ObjData.certificate_require_for_min_no_of_day);
                    tlra_.maximum_carried_forward = Convert.ToByte(ObjData.maximum_carried_forward);
                    tlra_.maximum_leave_can_be_in_day = Convert.ToByte(ObjData.maximum_leave_can_be_in_day);
                    tlra_.maximum_leave_can_be_taken = Convert.ToByte(ObjData.maximum_leave_can_be_taken);
                    tlra_.maximum_leave_can_be_taken_in_quater = Convert.ToByte(ObjData.maximum_leave_can_be_taken_in_quater);
                    tlra_.maximum_leave_can_be_taken_type = Convert.ToByte(ObjData.maximum_leave_can_be_taken_type);
                    tlra_.maximum_leave_clubbed_in_tenure_number_of_leave = Convert.ToByte(ObjData.maximum_leave_clubbed_in_tenure_number_of_leave);
                    tlra_.maxi_negative_leave_applicable = Convert.ToByte(ObjData.maxi_negative_leave_applicable);
                    tlra_.minimum_leave_applicable = Convert.ToByte(ObjData.minimum_leave_applicable);
                    tlra_.number_maximum_negative_leave_balance = Convert.ToByte(ObjData.number_maximum_negative_leave_balance);
                    tlra_.is_deleted = 0;
                    _context.tbl_leave_rule.Add(tlra_);
                    _context.SaveChanges();


                    tbl_leave_cashable tlc_ = new tbl_leave_cashable();
                    tlc_.leave_info_id = tla_.leave_info_id;
                    tlc_.maximum_cashable_leave = Convert.ToByte(ObjData.maximum_cashable_leave);
                    tlc_.is_cashable = Convert.ToByte(ObjData.is_cashable);
                    tlc_.cashable_type = Convert.ToByte(ObjData.cashable_type);
                    tlc_.cashable_after_year = Convert.ToByte(ObjData.cashable_after_year);
                    tlc_.is_deleted = 0;
                    _context.tbl_leave_cashable.Add(tlc_);
                    _context.SaveChanges();

                    //foreach (var FileData in files)
                    //{
                    //    if (FileData != null && FileData.Length > 0)
                    //    {
                    //        var allowedExtensions = new[] { ".Jpg", ".pdf", "jpeg", ".jpg", ".Pdf", "Jpeg" };

                    //        var ext = Path.GetExtension(FileData.FileName); //getting the extension
                    //        if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                    //        {
                    //            string name = Path.GetFileNameWithoutExtension(FileData.FileName); //getting file name without extension  
                    //            string MyFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ext; //Guid.NewGuid().ToString().Replace("-", "") +

                    //            var webRoot = _hostingEnvironment.WebRootPath;

                    //            if (!Directory.Exists(webRoot + "/LeaveCredit/Certificate/"))
                    //            {
                    //                Directory.CreateDirectory(webRoot + "/LeaveCredit/Certificate/");
                    //            }

                    //            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/LeaveCredit/Certificate/");
                    //            //save file
                    //            using (var fileStream = new FileStream(Path.Combine(path, MyFileName), FileMode.Create))
                    //            {
                    //                FileData.CopyTo(fileStream);
                    //                ObjData.certificate_path = "/wwwroot/LeaveCredit/Certificate/" + MyFileName;
                    //            }
                    //        }
                    //    }
                    //}
                    tran.Commit();

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }

        }

        //supriya 


        // POST: api/apiLeave
        [HttpPost]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveSetting))]
        public async Task<IActionResult> Posttbl_leave_info()//[FromBody] tbl_leave_info objleaveinfo)
        {


            Response_Msg objResult = new Response_Msg();
            // var files1 = HttpContext.Request.Form.Files["name"];
            var files = HttpContext.Request.Form.Files;
            var a = HttpContext.Request.Form["AllData"];
            if (a.ToString() == null)
            {
                objResult.StatusCode = 1;
                objResult.Message = "Invalid json data please try again !!";
                return Ok(objResult);
            }

            try
            {
                CommonClass com = new CommonClass();
                LeaveInfo_ApplicabilityModel ObjData = com.ToObjectFromJSON<LeaveInfo_ApplicabilityModel>(a.ToString());


                tbl_leave_info objleaveinfo = new tbl_leave_info();
                tbl_leave_info_log objlog = new tbl_leave_info_log();
                tbl_leave_applicablity objapplicability = new tbl_leave_applicablity();

                tbl_leave_app_on_emp_type objleave_emp = new tbl_leave_app_on_emp_type();
                tbl_leave_appcbl_on_company objappcompany = new tbl_leave_appcbl_on_company();
                tbl_leave_app_on_dept objdept = new tbl_leave_app_on_dept();
                tbl_leave_appcbl_on_religion objreligion = new tbl_leave_appcbl_on_religion();
                tbl_leave_credit objlcredit = new tbl_leave_credit();
                tbl_leave_rule objlrule = new tbl_leave_rule();
                tbl_leave_cashable objlcash = new tbl_leave_cashable();

                objleaveinfo.leave_type_id = Convert.ToByte(ObjData.leave_type_id);
                objleaveinfo.leave_type = Convert.ToByte(ObjData.leave_type);

                //var Leaveinfo = _context.tbl_leave_appcbl_on_company.Where(b => b.tbl_leave_applicablity.tbl_leave_info.leave_type_id == objleaveinfo.leave_type_id
                //  && b.tbl_leave_applicablity.tbl_leave_info.leave_tenure_from_date.Date >= Convert.ToDateTime(ObjData.leave_tenure_from_date).Date
                //  && b.tbl_leave_applicablity.tbl_leave_info.leave_tenure_to_date.Date <= Convert.ToDateTime(ObjData.leave_tenure_to_date).Date && b.c_id == Convert.ToInt32(ObjData.is_aplicable_on_all_company)).OrderByDescending(k => k.tbl_leave_applicablity.leave_applicablity_id).FirstOrDefault();

                DateTime currentDt = Convert.ToDateTime(ObjData.leave_tenure_from_date).Date;
                var Leaveinfo = _context.tbl_leave_info.Where(p => p.is_active == 1 && p.leave_type_id == Convert.ToInt32(ObjData.leave_type_id) && p.leave_tenure_from_date <= currentDt && p.leave_tenure_to_date >= currentDt)
                     .Select(p => new { p.leave_info_id, p.leave_type, p.tbl_leave_type.leave_type_name }).Distinct().FirstOrDefault();



                if (Leaveinfo != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Leave info already exist !!";
                    return Ok(objResult);
                }
                else
                {
                    TransactionOptions options = new TransactionOptions();
                    options.Timeout = new TimeSpan(0, 3, 0);//hh,mm,ss
                    // using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, options))
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {

                            //save leave info
                            objleaveinfo.leave_type = Convert.ToByte(ObjData.leave_type);
                            objleaveinfo.leave_type_id = Convert.ToByte(ObjData.leave_type_id);
                            objleaveinfo.leave_tenure_from_date = Convert.ToDateTime(ObjData.leave_tenure_from_date);
                            objleaveinfo.leave_tenure_to_date = Convert.ToDateTime(ObjData.leave_tenure_to_date);
                            objleaveinfo.is_active = Convert.ToByte(ObjData.is_active);
                            _context.tbl_leave_info.Add(objleaveinfo);
                            _context.SaveChanges();

                            //save in log table
                            objlog.leave_info_id = objleaveinfo.leave_info_id;
                            objlog.leave_type = objleaveinfo.leave_type;
                            objlog.leave_type_id = objleaveinfo.leave_type_id;
                            objlog.leave_tenure_from_date = objleaveinfo.leave_tenure_from_date;
                            objlog.leave_tenure_to_date = objleaveinfo.leave_tenure_to_date;
                            objlog.requested_by = 1;
                            objlog.requested_date = DateTime.Now;

                            _context.tbl_leave_info_log.Add(objlog);
                            _context.SaveChanges();
                            //save leave applicability
                            objapplicability.leave_info_id = objleaveinfo.leave_info_id;



                            if (Convert.ToInt32(ObjData.is_aplicable_on_all_company) == 0)
                            {
                                objapplicability.is_aplicable_on_all_location = 1;
                                objapplicability.is_aplicable_on_all_company = 1;
                            }
                            else if (Convert.ToInt32(ObjData.is_aplicable_on_all_company) > 0 && Convert.ToInt32(ObjData.is_aplicable_on_all_location) == 0)
                            {
                                objapplicability.is_aplicable_on_all_location = 1;
                                objapplicability.is_aplicable_on_all_company = 0;

                            }
                            else
                            {
                                objapplicability.is_aplicable_on_all_location = 0;
                                objapplicability.is_aplicable_on_all_company = 0;
                            }
                            if (Convert.ToInt32(ObjData.is_aplicable_on_all_department) == 0)
                            {
                                objapplicability.is_aplicable_on_all_department = 1;
                            }
                            else
                            {
                                objapplicability.is_aplicable_on_all_department = 0;
                            }

                            if (Convert.ToInt32(ObjData.is_aplicable_on_all_religion) == 0)
                            {
                                objapplicability.is_aplicable_on_all_religion = 1;
                            }
                            else
                            {
                                objapplicability.is_aplicable_on_all_religion = 0;
                            }
                            // supriya 11-12-2019

                            if (Convert.ToInt32(ObjData.is_aplicable_on_all_emp_type) == 0)
                            {
                                objapplicability.is_aplicable_on_all_emp_type = 1;
                            }
                            else
                            {
                                objapplicability.is_aplicable_on_all_emp_type = 0;
                            }

                            // supriya 11-12-2019
                            objapplicability.is_aplicable_on_all_emp_type = Convert.ToByte(ObjData.is_aplicable_on_all_emp_type);
                            objapplicability.leave_applicable_for = Convert.ToByte(ObjData.leave_applicable_for);
                            objapplicability.day_part = Convert.ToByte(ObjData.day_part);
                            if (Convert.ToByte(ObjData.leave_applicable_for) == 3)
                            {
                                objapplicability.leave_applicable_in_hours_and_minutes = Convert.ToDateTime(ObjData.leave_applicable_in_hours_and_minutes);
                            }
                            objapplicability.employee_can_apply = Convert.ToByte(ObjData.employee_can_apply);
                            objapplicability.admin_can_apply = Convert.ToByte(ObjData.admin_can_apply);
                            objapplicability.is_deleted = 0;

                            _context.tbl_leave_applicablity.Add(objapplicability);
                            _context.SaveChanges();

                            //save leave applicability employee when not applicable for all
                            if (Convert.ToByte(ObjData.is_aplicable_on_all_emp_type) > 0)
                            {
                                objleave_emp.l_app_id = objapplicability.leave_applicablity_id;
                                objleave_emp.employment_type = Convert.ToByte(ObjData.employment_type);
                                objleave_emp.is_deleted = 0;
                                _context.tbl_leave_app_on_emp_type.Add(objleave_emp);
                                _context.SaveChanges();

                            }
                            //save tbl_leave_appcbl_on_company when not applicable for all
                            if (Convert.ToInt32(ObjData.is_aplicable_on_all_company) > 0)
                            {
                                List<tbl_leave_appcbl_on_company> tobedeleted = _context.tbl_leave_appcbl_on_company.Where(x => x.l_a_id == objapplicability.leave_applicablity_id).ToList();
                                if (tobedeleted != null)
                                {
                                    tobedeleted.ForEach(p => p.is_deleted = 1);
                                    _context.UpdateRange(tobedeleted);
                                    _context.SaveChanges();
                                }
                                objappcompany.l_a_id = objapplicability.leave_applicablity_id;
                                objappcompany.c_id = Convert.ToInt32(ObjData.is_aplicable_on_all_company);

                                if (Convert.ToInt32(ObjData.is_aplicable_on_all_location) > 0)
                                {
                                    objappcompany.location_id = Convert.ToInt32(ObjData.is_aplicable_on_all_location);
                                }

                                objappcompany.is_deleted = 0;
                                _context.tbl_leave_appcbl_on_company.Add(objappcompany);
                                _context.SaveChanges();

                            }

                            //save tbl_leave_app_on_dept when not applicable for all
                            if (Convert.ToInt32(ObjData.is_aplicable_on_all_department) > 0)
                            {
                                List<tbl_leave_app_on_dept> tobedeleted = _context.tbl_leave_app_on_dept.Where(x => x.lid == objapplicability.leave_applicablity_id).ToList();
                                if (tobedeleted != null)
                                {
                                    tobedeleted.ForEach(p => p.is_deleted = 1);
                                    _context.UpdateRange(tobedeleted);
                                    _context.SaveChanges();
                                }

                                objdept.lid = objapplicability.leave_applicablity_id;
                                objdept.id = Convert.ToInt32(ObjData.is_aplicable_on_all_department);

                                objdept.is_deleted = 0;
                                _context.tbl_leave_app_on_dept.Add(objdept);
                                _context.SaveChanges();
                            }

                            //save tbl_leave_appcbl_on_religion when not applicable for all religion
                            if (Convert.ToInt32(ObjData.is_aplicable_on_all_religion) > 0)
                            {
                                List<tbl_leave_appcbl_on_religion> tobedeteleted = _context.tbl_leave_appcbl_on_religion.Where(x => x.l_app_id == objapplicability.leave_applicablity_id).ToList();
                                if (tobedeteleted != null)
                                {
                                    tobedeteleted.ForEach(p => p.is_deleted = 1);
                                    _context.UpdateRange(tobedeteleted);
                                    _context.SaveChanges();
                                }

                                objreligion.l_app_id = objapplicability.leave_applicablity_id;
                                objreligion.religion_id = Convert.ToInt32(ObjData.religion_id);
                                objreligion.is_deleted = 0;
                                _context.tbl_leave_appcbl_on_religion.Add(objreligion);
                                _context.SaveChanges();

                            }

                            //save in leave credit table
                            objlcredit.leave_info_id = objleaveinfo.leave_info_id;
                            objlcredit.frequency_type = Convert.ToByte(ObjData.frequency_type);
                            objlcredit.leave_credit_day = Convert.ToByte(ObjData.leave_credit_day);
                            objlcredit.leave_credit_number = Convert.ToDouble(ObjData.leave_credit_number);
                            objlcredit.is_half_day_applicable = Convert.ToByte(ObjData.is_half_day_applicable);
                            objlcredit.is_applicable_for_advance = Convert.ToByte(ObjData.is_applicable_for_advance);
                            objlcredit.advance_applicable_day = Convert.ToByte(ObjData.advance_applicable_day);
                            objlcredit.is_leave_accrue = Convert.ToByte(ObjData.is_leave_accrue);
                            objlcredit.max_accrue = Convert.ToByte(ObjData.max_accrue);
                            objlcredit.is_required_certificate = Convert.ToByte(ObjData.is_required_certificate);
                            objlcredit.certificate_path = ObjData.certificate_path;
                            objlcredit.is_deleted = 0;
                            _context.tbl_leave_credit.Add(objlcredit);
                            _context.SaveChanges();


                            //save in leave rule table
                            objlrule.leave_info_id = objleaveinfo.leave_info_id;
                            objlrule.applicable_if_employee_joined_before_dt = Convert.ToDateTime(ObjData.applicable_if_employee_joined_before_dt);
                            objlrule.applied_sandwich_rule = Convert.ToByte(ObjData.applied_sandwich_rule);
                            objlrule.can_carried_forward = Convert.ToByte(ObjData.can_carried_forward);
                            objlrule.certificate_require_for_min_no_of_day = Convert.ToByte(ObjData.certificate_require_for_min_no_of_day);
                            objlrule.maximum_carried_forward = Convert.ToByte(ObjData.maximum_carried_forward);
                            objlrule.maximum_leave_can_be_in_day = Convert.ToByte(ObjData.maximum_leave_can_be_in_day);
                            objlrule.maximum_leave_can_be_taken = Convert.ToByte(ObjData.maximum_leave_can_be_taken);
                            objlrule.maximum_leave_can_be_taken_in_quater = Convert.ToByte(ObjData.maximum_leave_can_be_taken_in_quater);
                            objlrule.maximum_leave_can_be_taken_type = Convert.ToByte(ObjData.maximum_leave_can_be_taken_type);
                            objlrule.maximum_leave_clubbed_in_tenure_number_of_leave = Convert.ToByte(ObjData.maximum_leave_clubbed_in_tenure_number_of_leave);
                            objlrule.maxi_negative_leave_applicable = Convert.ToByte(ObjData.maxi_negative_leave_applicable);
                            objlrule.minimum_leave_applicable = Convert.ToByte(ObjData.minimum_leave_applicable);
                            objlrule.number_maximum_negative_leave_balance = Convert.ToByte(ObjData.number_maximum_negative_leave_balance);
                            objlrule.is_deleted = 0;
                            _context.tbl_leave_rule.Add(objlrule);
                            _context.SaveChanges();


                            //save in leave cashble table
                            objlcash.leave_info_id = objleaveinfo.leave_info_id;
                            objlcash.maximum_cashable_leave = Convert.ToByte(ObjData.maximum_cashable_leave);
                            objlcash.is_cashable = Convert.ToByte(ObjData.is_cashable);
                            objlcash.cashable_type = Convert.ToByte(ObjData.cashable_type);
                            objlcash.cashable_after_year = Convert.ToByte(ObjData.cashable_after_year);
                            objlcash.is_deleted = 0;
                            _context.tbl_leave_cashable.Add(objlcash);
                            _context.SaveChanges();

                            objResult.StatusCode = 0;
                            objResult.Message = "Leave settings save successfully !!";

                            transaction.Commit();

                            return Ok(objResult);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
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

        // DELETE: api/apiLeave/5
        [HttpDelete("{id}")]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveSetting))]
        public async Task<IActionResult> Deletetbl_leave_info([FromRoute] int id)
        {
            Response_Msg objResult = new Response_Msg();
            tbl_leave_info_log objlog = new tbl_leave_info_log();
            try
            {
                //check if not in operation
                var exist = _context.tbl_leave_applicablity.Where(x => x.leave_info_id == id).FirstOrDefault();

                if (exist == null)
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            var delete = _context.tbl_leave_info.Where(x => x.leave_info_id == id).FirstOrDefault();

                            if (delete == null)
                            {
                                objResult.StatusCode = 1;
                                objResult.Message = "Invalid leave info id please check and try !!";
                                return Ok(objResult);
                            }
                            //save in log table
                            objlog.leave_info_id = delete.leave_info_id;
                            objlog.leave_type = delete.leave_type;
                            objlog.leave_type_id = delete.leave_type_id;
                            objlog.leave_tenure_from_date = delete.leave_tenure_from_date;
                            objlog.leave_tenure_to_date = delete.leave_tenure_to_date;
                            objlog.requested_by = 1;
                            objlog.requested_date = DateTime.Now;

                            _context.Entry(objlog).State = EntityState.Added;
                            _context.SaveChanges();

                            //delete record from table                        
                            _context.Entry(delete).State = EntityState.Deleted;
                            _context.SaveChanges();

                            transaction.Commit();

                            objResult.StatusCode = 0;
                            objResult.Message = "Leave Info deleted successfully";

                            return Ok(objResult);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            objResult.StatusCode = 2;
                            objResult.Message = ex.Message;
                            return Ok(objResult);
                        }
                    }

                }
                else
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Leave Info already in operation !!";
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

        #region ----Leave Info List------
        [Route("GetLeaveTypeInfo")]
        [HttpGet]
        //[Authorize(Policy = "5005")]
        public IActionResult GetLeaveTypeInfo()
        {
            try
            {
                var data = (from a in _context.tbl_leave_info
                            join b in _context.tbl_leave_type
                            on a.leave_type_id equals b.leave_type_id
                            where b.is_active == 1 && a.leave_tenure_from_date.Date <= Convert.ToDateTime(DateTime.Now).Date
                            && a.leave_tenure_to_date.Date >= Convert.ToDateTime(DateTime.Now).Date
                            select new
                            {
                                a.leave_type_id,
                                a.leave_info_id,
                                a.leave_type,
                                b.leave_type_name

                            }).Distinct().ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #endregion

        #region--Leave Application Form created by vibhav on 28 dec 2018 
        [Route("SaveLeaveApplication")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveApplication))]

        public IActionResult SaveLeaveApplication([FromBody] LeaveApplicationModel LeaveModel)
        {
            Response_Msg objResult = new Response_Msg();
            bool IsLWP = false;
            LeaveLedgerModell leaveledger = null;
            if (!_clsCurrentUser.DownlineEmpId.Any(p => p == LeaveModel.r_e_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!";
                return Ok(objResult);
            }

            if (Convert.ToBoolean(FunIsApplicationFreezed()) == true)
            {
                objResult.StatusCode = 1;
                objResult.Message = "Leave Application has been freezed for this month";
                return Ok(objResult);
            }

            if (!_clsCurrentUser.CompanyId.Contains(LeaveModel.company_id ?? 0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access";
            }

            tbl_leave_request obkleavereq = new tbl_leave_request();
            double no_of_days_leave = 0;
            try
            {

                if (LeaveModel.is_final_approve != 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid data";
                    return Ok(objResult);
                }

                var varleaveinfo = _context.tbl_leave_info.FirstOrDefault(a => a.leave_type_id == LeaveModel.leave_type_id && a.is_active == 1 && a.leave_tenure_from_date <= LeaveModel.from_date && a.leave_tenure_to_date >= LeaveModel.from_date);
                if (varleaveinfo == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Leave Setting not defined !!";
                    return Ok(objResult);
                }
                else
                {
                    LeaveModel.leave_info_id = varleaveinfo.leave_info_id;
                }
                if (LeaveModel.leave_info_id != 0)
                {

                    //var exist_attendance = _context.tbl_attendace_request.Where(x =>x.company_id==LeaveModel.company_id && x.from_date == LeaveModel.from_date && x.r_e_id == LeaveModel.r_e_id && x.is_deleted == 0 && x.is_final_approve != 2).FirstOrDefault();
                    var exist = _context.tbl_leave_request.Where(x => x.leave_type_id == LeaveModel.leave_type_id && x.company_id == LeaveModel.company_id && x.r_e_id == LeaveModel.r_e_id /*&& x.leave_type_id == LeaveModel.leave_type_id*/ && x.is_deleted == 0 && x.is_final_approve != 2 &&
                    ((x.from_date.Date <= LeaveModel.from_date.Date && LeaveModel.from_date.Date <= x.to_date.Date) || (x.from_date <= LeaveModel.to_date && LeaveModel.to_date <= x.to_date)
                    || (LeaveModel.from_date.Date <= x.from_date.Date && x.to_date.Date <= LeaveModel.to_date.Date))).ToList();

                    //var exist_outdoor = _context.tbl_outdoor_request.OrderByDescending(x => x.is_final_approve).Where(x => x.company_id == LeaveModel.company_id && x.r_e_id == LeaveModel.r_e_id && (x.from_date == LeaveModel.from_date || x.from_date==LeaveModel.to_date)  && x.is_deleted == 0 && x.is_final_approve != 2).FirstOrDefault();
                    //var exist_CompOff = _context.tbl_comp_off_request_master.Where(x => x.company_id == LeaveModel.company_id && x.r_e_id == LeaveModel.r_e_id && x.compoff_against_date == LeaveModel.from_date && x.is_deleted == 0 && x.is_final_approve != 2).FirstOrDefault();

                    if (exist.Count > 0)
                    {
                        // leave applicable for
                        if (exist.Any(x => x.leave_applicable_for == 1) && LeaveModel.leave_applicable_for == 1)
                        {
                            if (exist.Any(x => x.leave_applicable_for == LeaveModel.leave_applicable_for))
                            {
                                if (exist.Any(x => x.leave_applicable_for == LeaveModel.leave_applicable_for && x.is_final_approve == 0))
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "Leave application already applied for the selected date range !!";
                                    return Ok(objResult);
                                }
                                else if (exist.Any(x => x.leave_applicable_for == LeaveModel.leave_applicable_for && x.is_final_approve == 1))
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "Leave application already Approved for the selected date range !!";
                                    return Ok(objResult);
                                }
                            }

                        }
                        else if (exist.Any(x => x.leave_applicable_for == 1) && LeaveModel.leave_applicable_for == 2)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Leave already available for full day";
                            return Ok(objResult);
                        }
                        else if (exist.Any(x => x.leave_applicable_for == 2) && LeaveModel.leave_applicable_for == 1)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "You can't apply for full day leave because half day leave already applied";
                            return Ok(objResult);
                        }
                        else if (exist.Any(x => x.leave_applicable_for == 2) && LeaveModel.leave_applicable_for == 2)
                        {
                            var halfday_leaves = exist.Where(x => x.leave_applicable_for == LeaveModel.leave_applicable_for).ToList();
                            if (halfday_leaves.Count > 0)
                            {
                                if (halfday_leaves.Any(x => x.day_part == LeaveModel.day_part)) // same first or second half
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "Leave is already applied of selected day part";
                                    return Ok(objResult);
                                }
                                else if (halfday_leaves.Any(x => x.day_part == 1 && x.day_part == 2)) // both first and second half day leave available
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "Leave is already avaialble for both first and second half";
                                    return Ok(objResult);
                                }
                            }
                        }
                        else
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Invalid Leave";
                            return Ok(objResult);
                        }
                    }


                    //start check attandance already freezed or not

                    var freeze_attandance = _context.tbl_daily_attendance.Where(x => x.emp_id == LeaveModel.r_e_id && x.tbl_employee_master.tbl_employee_company_map.FirstOrDefault(q => q.is_deleted == 0).company_id == LeaveModel.company_id && x.is_freezed == 1 &&
                    (LeaveModel.from_date.Date <= x.attendance_dt.Date && x.attendance_dt.Date <= LeaveModel.to_date.Date)).ToList();

                    if (freeze_attandance.Count > 0)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Attendance of selected leave period is freezed";
                        return Ok(objResult);

                    }
                    //end check attandance already freezed or not
#if false
                    if (!(_context.tbl_leave_type.Where(p => p.leave_type_id == varleaveinfo.leave_type_id && p.leave_type_name == "LWP").Count() > 0))
                    //if (!_context.tbl_leave_type.Any(p => p.leave_type_id == varleaveinfo.leave_type_id && p.leave_type_name == "LWP"))
                    {
                        leaveledger = _clsLeaveCredit.Get_Leave_Ledger().Where(x => x.e_id == LeaveModel.r_e_id && x.leave_type_id == LeaveModel.leave_type_id).FirstOrDefault();
                        if (leaveledger == null || leaveledger.balance <= 0)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Insufficient balance!!";

                            // objResult.Message = "Leave not available in your selected leave type acount, please check and try !!";
                            return Ok(objResult);
                        }
                    }
                    else
                    {
                        IsLWP = true;
                    }

#endif
                    //leave_holiday_weekoff check_holiday_and_weekoff = check_holiday_weekoff_exist_on_leaveapp(LeaveModel.from_date, LeaveModel.to_date, Convert.ToInt32(LeaveModel.r_e_id));
                    //if (check_holiday_and_weekoff.Holiday == 1)
                    //{
                    //    objResult.StatusCode = 1;
                    //    objResult.Message = "You can not apply for leave application on Holiday...!";
                    //    return Ok(objResult);
                    //}
                    //if (check_holiday_and_weekoff.WeekOff == 1)
                    //{
                    //    objResult.StatusCode = 1;
                    //    objResult.Message = "You can not apply for leave application on  Weekly Off...!";
                    //    return Ok(objResult);
                    //}

                    if (LeaveModel.leave_applicable_for == 1) // full day leave
                    {
                        if (LeaveModel.leave_type_id == 3)
                            no_of_days_leave = Convert.ToInt32(((LeaveModel.to_date - LeaveModel.from_date).TotalDays) + 1) * 0.5;
                        else
                            no_of_days_leave = Convert.ToInt32((LeaveModel.to_date - LeaveModel.from_date).TotalDays) + 1;
                    }
                    else if (LeaveModel.leave_applicable_for == 2) // half day leave
                    {
                        no_of_days_leave = Convert.ToInt32(((LeaveModel.to_date - LeaveModel.from_date).TotalDays) + 1) * 0.5;
                    }

                    if (!IsLWP && no_of_days_leave > leaveledger.balance)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Insufficient leave balance !!";
                        return Ok(objResult);
                    }

                    var check_already_request_leave = _context.tbl_leave_request.Where(x => x.leave_type_id == LeaveModel.leave_type_id && x.company_id == LeaveModel.company_id &&
                    x.r_e_id == LeaveModel.r_e_id && x.is_deleted == 0 && (x.is_final_approve == 0))
                    .GroupBy(l => new { l.r_e_id, l.leave_type_id }).Select(q => new { totalapplied = q.Sum(r => r.leave_qty) }).FirstOrDefault();

                    if (check_already_request_leave != null)
                    {
                        var total_applied_leave = check_already_request_leave.totalapplied + no_of_days_leave; // previous applied + current applying

                        if (!IsLWP && total_applied_leave > leaveledger.balance)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Insufficient balance you have pending application for approval !!";
                            // objResult.Message = "Insufficient leave balance beacuse you have already applied some leave also. !!";
                            return Ok(objResult);
                        }
                    }
                    //get leave qty. from date range
                    //double leaveqty = Convert.ToInt32((LeaveModel.to_date - LeaveModel.from_date).TotalDays) + 1;
                    obkleavereq.r_e_id = LeaveModel.r_e_id;
                    obkleavereq.from_date = LeaveModel.from_date;
                    obkleavereq.to_date = LeaveModel.to_date;
                    obkleavereq.leave_type_id = LeaveModel.leave_type_id;
                    obkleavereq.leave_qty = no_of_days_leave;//leaveqty;
                    obkleavereq.leave_info_id = LeaveModel.leave_info_id;
                    obkleavereq.leave_applicable_for = LeaveModel.leave_applicable_for;
                    obkleavereq.day_part = LeaveModel.leave_applicable_for == 2 ? LeaveModel.day_part : obkleavereq.day_part;
                    obkleavereq.leave_applicable_in_hours_and_minutes = LeaveModel.leave_applicable_for == 3 ? Convert.ToDateTime(LeaveModel.leave_applicable_in_hours_and_minutes) : obkleavereq.leave_applicable_in_hours_and_minutes;
                    obkleavereq.requester_remarks = LeaveModel.requester_remarks;
                    obkleavereq.requester_date = DateTime.Now;
                    obkleavereq.is_deleted = 0;
                    obkleavereq.created_dt = DateTime.Now;
                    obkleavereq.company_id = LeaveModel.company_id;
                    obkleavereq.is_admin_approve = 0;
                    obkleavereq.admin_id = null;
                    obkleavereq.is_final_approve = 0;
                    obkleavereq.is_approved1 = 0;
                    obkleavereq.is_approved2 = 0;
                    obkleavereq.is_approved3 = 0;
                    obkleavereq.a1_e_id = null;
                    obkleavereq.a2_e_id = null;
                    obkleavereq.a3_e_id = null;


                    _context.Entry(obkleavereq).State = EntityState.Added;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Leave application applied successfully !!";

                    MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                    Task task = Task.Factory.StartNew(() => obj_ms.LeaveApplicationRequestMail(Convert.ToInt32(LeaveModel.r_e_id), LeaveModel.from_date, LeaveModel.to_date, Convert.ToInt32(LeaveModel.leave_type_id), Convert.ToInt32(LeaveModel.leave_applicable_for), Convert.ToInt32(LeaveModel.day_part), LeaveModel.requester_remarks));
                    task.Wait();
                    return Ok(objResult);



                    //if (exist.Any(x=>x.leave_applicable_for==LeaveModel.leave_applicable_for)) // full day or half day
                    //{
                    //    if (exist.is_final_approve == 0)
                    //    {
                    //        objResult.StatusCode = 1;
                    //        objResult.Message = "Leave application already applied for the selected date range !!";
                    //        return Ok(objResult);
                    //    }
                    //    else if (exist.is_final_approve == 1)
                    //    {
                    //        objResult.StatusCode = 1;
                    //        objResult.Message = "Leave application already Approved for the selected date range !!";
                    //        return Ok(objResult);
                    //    }

                    //}






                    //else if (exist_outdoor != null)
                    //{
                    //    if (exist_outdoor.is_final_approve == 0)
                    //    {
                    //        objResult.StatusCode = 1;
                    //        objResult.Message = "Outdoor application already applied for the selected date range !!";
                    //        return Ok(objResult);
                    //    }
                    //    else if (exist_outdoor.is_final_approve == 1)
                    //    {
                    //        objResult.StatusCode = 1;
                    //        objResult.Message = "Outdoor application already Approved for the selected date range !!";
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

                    //    if (exist_CompOff != null)
                    //    {
                    //        objResult.StatusCode = 1;
                    //        objResult.Message = "CompOff request either already approved or pending";
                    //        return Ok(objResult);
                    //    }
                    //}





                    //var leaveledger = _context.tbl_leave_ledger.Where(x => x.e_id == LeaveModel.r_e_id && x.leave_type_id == LeaveModel.leave_type_id)
                    //.GroupBy(p => p.e_id).Select(q => new
                    //{
                    //    totalcredit = q.Sum(r => r.credit),
                    //    totaldebit = q.Sum(r => r.dredit),
                    //    leavebalance = q.Sum(x => x.credit) - q.Sum(x => x.dredit)
                    //}).FirstOrDefault();




                }
                else
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Leave Type not available";
                    return Ok(objResult);
                }




            }
            catch (Exception ex)
            {
                objResult.StatusCode = 1;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }


        [Route("UpdateLeaveApplication")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveApplication))]
        public IActionResult UpdateLeaveApplication([FromBody] LeaveApplicationModel LeaveModel)
        {
            Response_Msg objResult = new Response_Msg();
            tbl_leave_request obkleavereq = new tbl_leave_request();
            if (!_clsCurrentUser.DownlineEmpId.Any(p => p == LeaveModel.r_e_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!";
                return Ok(objResult);
            }
            bool IsLWP = false;


            try
            {

                if (LeaveModel.is_final_approve != 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid data";
                    return Ok(objResult);
                }
                var varleaveinfo = _context.tbl_leave_info.FirstOrDefault(a => a.leave_type_id == LeaveModel.leave_type_id && a.is_active == 1 && a.leave_tenure_from_date <= LeaveModel.from_date && a.leave_tenure_to_date >= LeaveModel.from_date);
                if (varleaveinfo == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Leave Setting not defined !!";
                    return Ok(objResult);
                }
                else
                {

                    if (_context.tbl_leave_type.Where(p => p.leave_type_id == varleaveinfo.leave_type_id && p.leave_type_name == "LWP").Count() > 0)
                    {
                        IsLWP = true;
                    }
                    LeaveModel.leave_info_id = varleaveinfo.leave_info_id;
                }

                if (LeaveModel.leave_info_id != 0)
                {

                    var exist = _context.tbl_leave_request.Where(x => x.company_id == LeaveModel.company_id && x.r_e_id == LeaveModel.r_e_id && x.is_deleted == 0 && x.is_final_approve != 2 && ((x.from_date.Date <= LeaveModel.from_date.Date && LeaveModel.from_date.Date <= x.to_date.Date) || (x.from_date <= LeaveModel.to_date && LeaveModel.to_date <= x.to_date) || (LeaveModel.from_date.Date <= x.from_date.Date && x.to_date.Date <= LeaveModel.to_date.Date))).ToList();

                    if (exist.Count > 0)
                    {
                        // leave applicable for
                        if (exist.Any(x => x.leave_applicable_for == 1) && LeaveModel.leave_applicable_for == 1)
                        {
                            if (exist.Any(x => x.leave_applicable_for == LeaveModel.leave_applicable_for))
                            {
                                if (exist.Any(x => x.leave_applicable_for == LeaveModel.leave_applicable_for && x.is_final_approve == 0))
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "Leave application already applied for the selected date range !!";
                                    return Ok(objResult);
                                }
                                else if (exist.Any(x => x.leave_applicable_for == LeaveModel.leave_applicable_for && x.is_final_approve == 1))
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "Leave application already Approved for the selected date range !!";
                                    return Ok(objResult);
                                }
                            }

                        }
                        else if (exist.Any(x => x.leave_applicable_for == 1) && LeaveModel.leave_applicable_for == 2)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Leave already applied for full day";
                            return Ok(objResult);
                        }
                        else if (exist.Any(x => x.leave_applicable_for == 2) && LeaveModel.leave_applicable_for == 1)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "You can't apply for full day leave because half day leave already applied";
                            return Ok(objResult);
                        }
                        else if (exist.Any(x => x.leave_applicable_for == 2) && LeaveModel.leave_applicable_for == 2)
                        {
                            var halfday_leaves = exist.Where(x => x.leave_applicable_for == LeaveModel.leave_applicable_for).ToList();
                            if (halfday_leaves.Count > 0)
                            {
                                if (halfday_leaves.Any(x => x.day_part == LeaveModel.day_part)) // same first or second half
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "Leave is already applied of selected day part";
                                    return Ok(objResult);
                                }
                                else if (halfday_leaves.Any(x => x.day_part == 1 && x.day_part == 2)) // both first and second half day leave available
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "Leave is already avaialble for both first and second half";
                                    return Ok(objResult);
                                }
                            }
                        }
                        else
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Invalid Leave";
                            return Ok(objResult);
                        }
                    }

                    //start check attandance already freezed or not

                    var freeze_attandance = _context.tbl_daily_attendance.Where(x => x.emp_id == LeaveModel.r_e_id && x.tbl_employee_master.tbl_employee_company_map.FirstOrDefault(q => q.is_deleted == 0).company_id == LeaveModel.company_id && x.is_freezed == 1 &&
                    (LeaveModel.from_date.Date <= x.attendance_dt.Date && x.attendance_dt.Date <= LeaveModel.to_date.Date)).ToList();

                    if (freeze_attandance.Count > 0)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Attendance of selected leave period is freezed";
                        return Ok(objResult);

                    }
                    //end check attandance already freezed or not

                }

                var rejected_rqst = _context.tbl_leave_request.Where(x => x.leave_request_id != LeaveModel.leave_request_id && x.r_e_id == LeaveModel.r_e_id && x.leave_info_id == LeaveModel.leave_info_id && x.from_date.Date >= LeaveModel.from_date.Date && x.to_date.Date <= LeaveModel.to_date.Date && x.is_deleted == 0 && x.is_final_approve == 2).OrderByDescending(x => x.leave_request_id).FirstOrDefault();


                var duplicate = _context.tbl_leave_request.Where(x => x.leave_request_id != LeaveModel.leave_request_id && x.r_e_id == LeaveModel.r_e_id && x.leave_info_id == LeaveModel.leave_info_id && x.from_date.Date >= LeaveModel.from_date.Date && x.to_date.Date <= LeaveModel.to_date.Date && x.is_deleted == 0 && x.is_final_approve == 0).OrderByDescending(x => x.leave_request_id).FirstOrDefault();

                if (rejected_rqst != null)
                {
                    double leaveqty = Convert.ToInt32((LeaveModel.to_date - LeaveModel.from_date).TotalDays) + 1;
                    if (LeaveModel.leave_applicable_for == 2) leaveqty = leaveqty / 2;

                    var exist = _context.tbl_leave_request.Where(x => x.leave_request_id == LeaveModel.leave_request_id && x.is_deleted == 0 && (int)x.is_final_approve == 0).FirstOrDefault();
                    if (exist == null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Invalid leave request id, please try after refresh page !!";
                        return Ok(objResult);
                    }
                    ////get leave type id by leave info id              
                    //LeaveModel.leave_type_id = _context.tbl_leave_info.Where(a => a.leave_info_id == LeaveModel.leave_info_id).Select(b => b.leave_type_id).FirstOrDefault();

                    //----without group by clause
                    var leaveledger = _context.tbl_leave_ledger.Where(x => x.e_id == LeaveModel.r_e_id && x.leave_type_id == LeaveModel.leave_type_id)
                       .GroupBy(p => p.e_id).Select(q => new
                       {
                           totalcredit = q.Sum(r => r.credit),
                           totaldebit = q.Sum(r => r.dredit),
                           leavebalance = q.Sum(x => x.credit) - q.Sum(x => x.dredit)
                       }).FirstOrDefault();

                    if (!IsLWP && (leaveledger == null || leaveledger.leavebalance <= leaveqty))
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Leave not available in your selected leave type account, please check and try !!";
                    }

                    //validate for hh:mm leave applicabvle for 3
                    if (LeaveModel.leave_applicable_for == 3)
                    {
                        var hhmm = DateTime.Parse(LeaveModel.leave_applicable_in_hours_and_minutes);

                        //validate from leave applcability
                        var valid_hhmm = _context.tbl_leave_applicablity.Where(x => x.leave_info_id == LeaveModel.leave_info_id && x.is_deleted == 0
                          && x.leave_applicable_in_hours_and_minutes.TimeOfDay < hhmm.TimeOfDay).Count();
                        if (valid_hhmm > 0)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Please enter valid HH:MM for short leave !!";

                        }
                    }


                    exist.r_e_id = LeaveModel.r_e_id;
                    exist.from_date = LeaveModel.from_date;
                    exist.to_date = LeaveModel.to_date;
                    exist.leave_type_id = LeaveModel.leave_type_id;
                    exist.leave_qty = leaveqty;
                    exist.leave_info_id = LeaveModel.leave_info_id;
                    exist.leave_applicable_for = LeaveModel.leave_applicable_for;
                    exist.day_part = LeaveModel.leave_applicable_for == 2 ? LeaveModel.day_part : exist.day_part;
                    exist.leave_applicable_in_hours_and_minutes = LeaveModel.leave_applicable_for == 3 ? Convert.ToDateTime(LeaveModel.leave_applicable_in_hours_and_minutes) : exist.leave_applicable_in_hours_and_minutes;
                    exist.requester_remarks = LeaveModel.requester_remarks;
                    exist.requester_date = DateTime.Now;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Leave application updated successfully !!";
                    MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                    Task task = Task.Factory.StartNew(() => obj_ms.LeaveApplicationRequestMail(Convert.ToInt32(LeaveModel.r_e_id), LeaveModel.from_date, LeaveModel.to_date, Convert.ToInt32(LeaveModel.leave_type_id), Convert.ToInt32(LeaveModel.leave_applicable_for), Convert.ToInt32(LeaveModel.day_part), LeaveModel.requester_remarks));
                    task.Wait();
                }

                else if (duplicate != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Leave application already applied for the selected date range still in pending stage !!";
                }
                else
                {
                    var exist = _context.tbl_leave_request.Where(x => x.leave_request_id == LeaveModel.leave_request_id && x.is_deleted == 0 && (int)x.is_final_approve == 0).FirstOrDefault();
                    if (exist == null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Invalid leave request id, please try after refresh page !!";
                    }

                    else
                    {
                        double leaveqty = Convert.ToInt32((LeaveModel.to_date - LeaveModel.from_date).TotalDays) + 1;
                        if (LeaveModel.leave_applicable_for == 2) leaveqty = leaveqty / 2;
                        //get leave type id by leave info id              
                        //LeaveModel.leave_type_id = _context.tbl_leave_info.Where(a => a.leave_info_id == LeaveModel.leave_info_id).Select(b => b.leave_type_id).FirstOrDefault();

                        //----without group by clause
                        var leaveledger = _context.tbl_leave_ledger.Where(x => x.e_id == LeaveModel.r_e_id && x.leave_type_id == LeaveModel.leave_type_id)
                           .GroupBy(p => p.e_id).Select(q => new
                           {
                               totalcredit = q.Sum(r => r.credit),
                               totaldebit = q.Sum(r => r.dredit),
                               leavebalance = q.Sum(x => x.credit) - q.Sum(x => x.dredit)
                           }).FirstOrDefault();

                        if (!IsLWP && (leaveledger == null || leaveledger.leavebalance < leaveqty))
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Leave not available in your selected leave type account, please check and try !!";
                            return Ok(objResult);
                        }

                        //validate for hh:mm leave applicabvle for 3
                        if (LeaveModel.leave_applicable_for == 3)
                        {
                            var hhmm = DateTime.Parse(LeaveModel.leave_applicable_in_hours_and_minutes);

                            //validate from leave applcability
                            var valid_hhmm = _context.tbl_leave_applicablity.Where(x => x.leave_info_id == LeaveModel.leave_info_id && x.is_deleted == 0
                              && x.leave_applicable_in_hours_and_minutes.TimeOfDay < hhmm.TimeOfDay).Count();
                            if (valid_hhmm > 0)
                            {
                                objResult.StatusCode = 1;
                                objResult.Message = "Please enter valid HH:MM for short leave !!";
                                return Ok(objResult);
                            }
                        }



                        exist.r_e_id = LeaveModel.r_e_id;
                        exist.from_date = LeaveModel.from_date;
                        exist.to_date = LeaveModel.to_date;
                        exist.leave_type_id = LeaveModel.leave_type_id;
                        exist.leave_qty = leaveqty;
                        exist.leave_info_id = LeaveModel.leave_info_id;
                        exist.leave_applicable_for = LeaveModel.leave_applicable_for;
                        exist.day_part = LeaveModel.leave_applicable_for == 2 ? LeaveModel.day_part : Convert.ToByte(0);
                        exist.leave_applicable_in_hours_and_minutes = LeaveModel.leave_applicable_for == 3 ? Convert.ToDateTime(LeaveModel.leave_applicable_in_hours_and_minutes) : exist.leave_applicable_in_hours_and_minutes;
                        exist.requester_remarks = LeaveModel.requester_remarks;
                        exist.requester_date = DateTime.Now;

                        _context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "Leave application updated successfully !!";
                        MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                        Task task = Task.Factory.StartNew(() => obj_ms.LeaveApplicationRequestMail(Convert.ToInt32(LeaveModel.r_e_id), LeaveModel.from_date, LeaveModel.to_date, Convert.ToInt32(LeaveModel.leave_type_id), Convert.ToInt32(LeaveModel.leave_applicable_for), Convert.ToInt32(LeaveModel.day_part), LeaveModel.requester_remarks));
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




        [Route("GetLeaveApplicationByEmpId")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveApplication))]

        public IActionResult GetLeaveApplicationByEmpId(int emp_id)
        {
            ResponseMsg objResult = new ResponseMsg();

            try
            {
                if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorized access...!!";
                    return Ok(objResult);
                }


                var data = _context.tbl_leave_request.OrderByDescending(y => y.leave_request_id).Where(x => _clsCurrentUser.DownlineEmpId.Contains(x.r_e_id ?? 0) && x.is_deleted == 0 && (x.is_final_approve == 0 || x.is_final_approve == 3)).Select(a => new
                {
                    leave_request_id = a.leave_request_id,
                    from_date = a.from_date,
                    to_date = a.to_date,
                    r_e_id = a.r_e_id,
                    requester_date = a.requester_date,
                    requester_remarks = a.requester_remarks,
                    leave_applicable_for = a.leave_applicable_for == 1 ? "Full Day" : a.leave_applicable_for == 2 ? "Half Day" : a.leave_applicable_for == 3 ? "HH:MM" : "",
                    leave_info_id = a.leave_info_id,
                    day_part = a.day_part,
                    leave_applicable_in_hours_and_minutes = a.leave_applicable_in_hours_and_minutes,
                    leave_type_name = a.tbl_leave_type.leave_type_name,
                    leave_qty = a.leave_qty,
                    approval1_remarks = a.approval1_remarks,
                    is_approved1 = a.is_approved1,
                    approval2_remarks = a.approval2_remarks,
                    is_approved2 = a.is_approved2,
                    approval3_remarks = a.approval3_remarks,
                    is_approved3 = a.is_approved3,
                    is_final_approve = a.is_final_approve,
                    is_deleted = a.is_deleted,
                    emp_code = a.tbl_employee_requester.emp_code,
                    emp_name = string.Format("{0} {1} {2}",
                                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name),

                    status_ = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approved" : a.is_final_approve == 2 ? "Rejected" : a.is_final_approve == 3 ? "In Process" : "") : a.is_deleted == 1 ? "Deleted" : a.is_deleted == 2 ? "Cancel" : "",
                }).Distinct().ToList();


                return Ok(data);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 1;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

#if false
        [Route("GetLeaveApplicationByEmp30march21")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveApplicationReport))]

        public IActionResult GetLeaveApplicationByEmp_30march21([FromBody] LeaveReport objmodel)
        {
            ResponseMsg objResult = new ResponseMsg();

            //try
            //{

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

            var leaveRequest = _context.tbl_leave_request.OrderByDescending(y => y.leave_request_id).Where(x => ((_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin) ? (objmodel.empdtl.Count > 1 ? true : objmodel.empdtl.Contains(x.r_e_id ?? 0)) : objmodel.empdtl.Contains(x.r_e_id ?? 0)) && ((x.from_date.Date <= DateTime.Parse(objmodel.from_date).Date && DateTime.Parse(objmodel.from_date).Date <= x.to_date.Date) || (x.from_date.Date <= DateTime.Parse(objmodel.to_date).Date && DateTime.Parse(objmodel.to_date).Date <= x.to_date.Date)
                             || (DateTime.Parse(objmodel.from_date).Date <= x.from_date.Date && x.to_date.Date <= DateTime.Parse(objmodel.to_date).Date))).Select(a => new
                             {
                                 leave_request_id = a.leave_request_id,
                                 from_date = a.from_date,
                                 to_date = a.to_date,
                                 r_e_id = a.r_e_id,
                                 requester_date = a.requester_date,
                                 requester_remarks = a.requester_remarks,
                                 leave_applicable_for = a.leave_applicable_for == 1 ? "Full Day" : a.leave_applicable_for == 2 ? "Half Day" : a.leave_applicable_for == 3 ? "HH:MM" : "",
                                 leave_info_id = a.leave_info_id.ToString(),
                                 day_part = a.day_part.ToString(),
                                 leave_applicable_in_hours_and_minutes = a.leave_applicable_in_hours_and_minutes,
                                 leave_type_name = a.tbl_leave_type.leave_type_name,
                                 leave_qty = a.leave_qty,
                                 approval1_remarks = a.approval1_remarks,
                                 is_approved1 = a.is_approved1,
                                 approval2_remarks = a.approval2_remarks,
                                 is_approved2 = a.is_approved2,
                                 approval3_remarks = a.approval3_remarks,
                                 is_approved3 = a.is_approved3,
                                 is_final_approve = a.is_final_approve,
                                 is_deleted = a.is_deleted,
                                 emp_code = a.tbl_employee_requester.emp_code,
                                 emp_name_code = string.Format("{0} {1} {2} ({3})",
                                              a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                              a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                              a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name,
                                              a.tbl_employee_requester.emp_code),
                                 emp_name = string.Format("{0} {1} {2}",
                                              a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                              a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                              a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name),
                                 dept_Name = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_department_master.department_name,
                                 designation = a.tbl_employee_requester.tbl_emp_desi_allocation.FirstOrDefault().tbl_designation_master.designation_name,
                                 location = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_location_master.location_name,
                                 approved_on = a.approval_date1 > a.requester_date ? a.approval_date1 : a.approval_date2 > a.requester_date ? a.approval_date2 : a.approval_date3 > a.requester_date ? a.approval_date3 : a.admin_ar_date > a.requester_date ? a.admin_ar_date : a.approval_date1,
                                 approved_by = Convert.ToInt32(a.is_approved1) == 1 ? string.Format("{0} {1} {2}",
                                            a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                            a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                            a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                            Convert.ToInt32(a.is_approved2) == 1 ? string.Format("{0} {1} {2}",
                                            a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                            a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                            a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                            Convert.ToInt32(a.is_approved3) == 1 ? string.Format("{0} {1} {2}",
                                            a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                            a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                            a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                            Convert.ToInt32(a.is_admin_approve) == 1 ? string.Format("{0} {1} {2}",
                                            a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                            a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                            a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) : "",
                                 approver_remarks = string.Concat(a.approval1_remarks ?? "", a.approval2_remarks ?? "", a.approval3_remarks ?? "", a.admin_remarks ?? ""),
                                 status_ = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approved" : a.is_final_approve == 2 ? "Rejected" : a.is_final_approve == 3 ? "In Process" : "") : a.is_deleted == 1 ? "Deleted" : a.is_deleted == 2 ? "Cancel" : "",
                             }).Distinct().ToList();

            var compoffRequest = _context.tbl_comp_off_request_master.OrderByDescending(y => y.comp_off_request_id).Where(x => ((_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin) ? (objmodel.empdtl.Count > 1 ? true : objmodel.empdtl.Contains(x.r_e_id ?? 0)) : objmodel.empdtl.Contains(x.r_e_id ?? 0)) && ((x.compoff_date.Date >= DateTime.Parse(objmodel.from_date).Date && x.compoff_date.Date <= DateTime.Parse(objmodel.to_date).Date))).Select(a => new
            {
                leave_request_id = a.comp_off_request_id,
                from_date = a.compoff_date,
                to_date = a.compoff_date,
                r_e_id = a.r_e_id,
                requester_date = a.requester_date,
                requester_remarks = a.requester_remarks,
                leave_applicable_for = "Full Day",
                leave_info_id = 0.ToString(),
                day_part = "",
                leave_applicable_in_hours_and_minutes = DateTime.MinValue,
                leave_type_name = "Compensatory Off",
                leave_qty = Convert.ToDouble(1),
                approval1_remarks = a.approval1_remarks,
                is_approved1 = a.is_approved1,
                approval2_remarks = a.approval2_remarks,
                is_approved2 = a.is_approved2,
                approval3_remarks = a.approval3_remarks,
                is_approved3 = a.is_approved3,
                is_final_approve = a.is_final_approve,
                is_deleted = a.is_deleted,
                emp_code = a.tbl_employee_requester.emp_code,
                emp_name_code = string.Format("{0} {1} {2} ({3})",
                                                   a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                   a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                   a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name,
                                                   a.tbl_employee_requester.emp_code),
                emp_name = string.Format("{0} {1} {2}",
                                                   a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                   a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                   a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name),
                dept_Name = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_department_master.department_name,
                designation = a.tbl_employee_requester.tbl_emp_desi_allocation.FirstOrDefault().tbl_designation_master.designation_name,
                location = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_location_master.location_name,
                approved_on = a.approval_date1 > a.requester_date ? a.approval_date1 : a.approval_date2 > a.requester_date ? a.approval_date2 : a.approval_date3 > a.requester_date ? a.approval_date3 : a.admin_ar_date > a.requester_date ? a.admin_ar_date : a.approval_date1,
                approved_by = Convert.ToInt32(a.is_approved1) == 1 ? string.Format("{0} {1} {2}",
                                                 a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                 a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                 a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                                 Convert.ToInt32(a.is_approved2) == 1 ? string.Format("{0} {1} {2}",
                                                 a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                 a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                 a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                                 Convert.ToInt32(a.is_approved3) == 1 ? string.Format("{0} {1} {2}",
                                                 a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                 a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                 a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                                 Convert.ToInt32(a.is_admin_approve) == 1 ? string.Format("{0} {1} {2}",
                                                 a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                 a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                 a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) : "",
                approver_remarks = string.Concat(a.approval1_remarks ?? "", a.approval2_remarks ?? "", a.approval3_remarks ?? "", a.admin_remarks ?? ""),
                status_ = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approved" : a.is_final_approve == 2 ? "Rejected" : a.is_final_approve == 3 ? "In Process" : "") : a.is_deleted == 1 ? "Deleted" : a.is_deleted == 2 ? "Cancel" : "",
            }).Distinct().ToList();

        #region single

            //var leaveRequest1 = _context.tbl_leave_request.OrderByDescending(y => y.leave_request_id).Where(x => ((_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin) ? (objmodel.empdtl.Count > 1 ? true : objmodel.empdtl.Contains(x.r_e_id ?? 0)) : objmodel.empdtl.Contains(x.r_e_id ?? 0)) && ((x.from_date.Date <= DateTime.Parse(objmodel.from_date).Date && DateTime.Parse(objmodel.from_date).Date <= x.to_date.Date) || (x.from_date.Date <= DateTime.Parse(objmodel.to_date).Date && DateTime.Parse(objmodel.to_date).Date <= x.to_date.Date)
            // || (DateTime.Parse(objmodel.from_date).Date <= x.from_date.Date && x.to_date.Date <= DateTime.Parse(objmodel.to_date).Date))).Select(a => new
            // {
            //     leave_request_id = a.leave_request_id,
            //     from_date = a.from_date,
            //     to_date = a.to_date,
            //     r_e_id = a.r_e_id,
            //     requester_date = a.requester_date,
            //     requester_remarks = a.requester_remarks,
            //     leave_applicable_for = a.leave_applicable_for == 1 ? "Full Day" : a.leave_applicable_for == 2 ? "Half Day" : a.leave_applicable_for == 3 ? "HH:MM" : "",
            //     leave_info_id = a.leave_info_id.ToString(),
            //     day_part = a.day_part.ToString(),
            //     leave_applicable_in_hours_and_minutes = a.leave_applicable_in_hours_and_minutes,
            //     leave_type_name = a.tbl_leave_type.leave_type_name,
            //     leave_qty = a.leave_qty,
            //     approval1_remarks = a.approval1_remarks,
            //     is_approved1 = a.is_approved1,
            //     approval2_remarks = a.approval2_remarks,
            //     is_approved2 = a.is_approved2,
            //     approval3_remarks = a.approval3_remarks,
            //     is_approved3 = a.is_approved3,
            //     is_final_approve = a.is_final_approve,
            //     is_deleted = a.is_deleted,
            //     emp_code = a.tbl_employee_requester.emp_code,
            //     emp_name_code = string.Format("{0} {1} {2} ({3})",
            //                                  a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
            //                                  a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
            //                                  a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name,
            //                                  a.tbl_employee_requester.emp_code),
            //     emp_name = string.Format("{0} {1} {2}",
            //                                  a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
            //                                  a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
            //                                  a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name),
            //     dept_Name = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_department_master.department_name,
            //     designation = a.tbl_employee_requester.tbl_emp_desi_allocation.FirstOrDefault().tbl_designation_master.designation_name,
            //     location = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_location_master.location_name,
            //     approved_on = a.approval_date1 > a.requester_date ? a.approval_date1 : a.approval_date2 > a.requester_date ? a.approval_date2 : a.approval_date3 > a.requester_date ? a.approval_date3 : a.admin_ar_date > a.requester_date ? a.admin_ar_date : a.approval_date1,
            //     approved_by = Convert.ToInt32(a.is_approved1) == 1 ? string.Format("{0} {1} {2}",
            //                                a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
            //                                a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
            //                                a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
            //                                Convert.ToInt32(a.is_approved2) == 1 ? string.Format("{0} {1} {2}",
            //                                a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
            //                                a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
            //                                a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
            //                                Convert.ToInt32(a.is_approved3) == 1 ? string.Format("{0} {1} {2}",
            //                                a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
            //                                a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
            //                                a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
            //                                Convert.ToInt32(a.is_admin_approve) == 1 ? string.Format("{0} {1} {2}",
            //                                a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
            //                                a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
            //                                a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) : "",
            //     approver_remarks = string.Concat(a.approval1_remarks ?? "", a.approval2_remarks ?? "", a.approval3_remarks ?? "", a.admin_remarks ?? ""),
            //     status_ = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approved" : a.is_final_approve == 2 ? "Rejected" : a.is_final_approve == 3 ? "In Process" : "") : a.is_deleted == 1 ? "Deleted" : a.is_deleted == 2 ? "Cancel" : "",
            // }).Union(_context.tbl_comp_off_request_master.OrderByDescending(y => y.comp_off_request_id).Where(x => ((_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin) ? (objmodel.empdtl.Count > 1 ? true : objmodel.empdtl.Contains(x.r_e_id ?? 0)) : objmodel.empdtl.Contains(x.r_e_id ?? 0)) && ((x.compoff_date.Date >= DateTime.Parse(objmodel.from_date).Date && x.compoff_date.Date <= DateTime.Parse(objmodel.to_date).Date))).Select(a => new
            // {
            //     leave_request_id = a.comp_off_request_id,
            //     from_date = a.compoff_date,
            //     to_date = a.compoff_date,
            //     r_e_id = a.r_e_id,
            //     requester_date = a.requester_date,
            //     requester_remarks = a.requester_remarks,
            //     leave_applicable_for = "Full Day",
            //     leave_info_id = 0.ToString(),
            //     day_part = "",
            //     leave_applicable_in_hours_and_minutes = DateTime.MinValue,
            //     leave_type_name = "Compensatory Off",
            //     leave_qty = Convert.ToDouble(1),
            //     approval1_remarks = a.approval1_remarks,
            //     is_approved1 = a.is_approved1,
            //     approval2_remarks = a.approval2_remarks,
            //     is_approved2 = a.is_approved2,
            //     approval3_remarks = a.approval3_remarks,
            //     is_approved3 = a.is_approved3,
            //     is_final_approve = a.is_final_approve,
            //     is_deleted = a.is_deleted,
            //     emp_code = a.tbl_employee_requester.emp_code,
            //     emp_name_code = string.Format("{0} {1} {2} ({3})",
            //                                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
            //                                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
            //                                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name,
            //                                       a.tbl_employee_requester.emp_code),
            //     emp_name = string.Format("{0} {1} {2}",
            //                                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
            //                                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
            //                                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name),
            //     dept_Name = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_department_master.department_name,
            //     designation = a.tbl_employee_requester.tbl_emp_desi_allocation.FirstOrDefault().tbl_designation_master.designation_name,
            //     location = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_location_master.location_name,
            //     approved_on = a.approval_date1 > a.requester_date ? a.approval_date1 : a.approval_date2 > a.requester_date ? a.approval_date2 : a.approval_date3 > a.requester_date ? a.approval_date3 : a.admin_ar_date > a.requester_date ? a.admin_ar_date : a.approval_date1,
            //     approved_by = Convert.ToInt32(a.is_approved1) == 1 ? string.Format("{0} {1} {2}",
            //                                     a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
            //                                     a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
            //                                     a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
            //                                     Convert.ToInt32(a.is_approved2) == 1 ? string.Format("{0} {1} {2}",
            //                                     a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
            //                                     a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
            //                                     a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
            //                                     Convert.ToInt32(a.is_approved3) == 1 ? string.Format("{0} {1} {2}",
            //                                     a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
            //                                     a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
            //                                     a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
            //                                     Convert.ToInt32(a.is_admin_approve) == 1 ? string.Format("{0} {1} {2}",
            //                                     a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
            //                                     a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
            //                                     a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) : "",
            //     approver_remarks = string.Concat(a.approval1_remarks ?? "", a.approval2_remarks ?? "", a.approval3_remarks ?? "", a.admin_remarks ?? ""),
            //     status_ = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approved" : a.is_final_approve == 2 ? "Rejected" : a.is_final_approve == 3 ? "In Process" : "") : a.is_deleted == 1 ? "Deleted" : a.is_deleted == 2 ? "Cancel" : "",
            // }));
        #endregion

            var data = leaveRequest;
            if (compoffRequest != null)
            {
                if (compoffRequest.Count > 0)
                {
                    data = leaveRequest.Union(compoffRequest).ToList();
                }
            }

            return Ok(data);
            //}
            //catch (Exception ex)
            //{
            //    objResult.StatusCode = 1;
            //    objResult.Message = ex.Message;
            //    return Ok(objResult);
            //}
        }

        [Route("GetLeaveApplicationByEmp")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveApplicationReport))]

        public IActionResult GetLeaveApplicationByEmp([FromBody] LeaveReport objmodel)
        {
            ResponseMsg objResult = new ResponseMsg();

            try
            {
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


                var leaveRequest = _context.tbl_leave_request.OrderByDescending(y => y.leave_request_id)
                    .Where(x => ((_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin)
                    ? (objmodel.empdtl.Count > 1 ? true : objmodel.empdtl.Contains(x.r_e_id.ToString()))
                    : objmodel.empdtl.Contains(x.r_e_id.ToString()))
                    && ((x.from_date.Date <= DateTime.Parse(objmodel.from_date).Date && DateTime.Parse(objmodel.from_date).Date <= x.to_date.Date)
                    || (x.from_date.Date <= DateTime.Parse(objmodel.to_date).Date && DateTime.Parse(objmodel.to_date).Date <= x.to_date.Date)
                    || (DateTime.Parse(objmodel.from_date).Date <= x.from_date.Date && x.to_date.Date <= DateTime.Parse(objmodel.to_date).Date))
                    && (objmodel.company_id == -1 || objmodel.company_id == x.company_id)
                    && (objmodel.location_id == -1 || objmodel.location_id == x.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).tbl_location_master.location_id)
                    && (objmodel.department_id == -1 || objmodel.department_id == x.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).tbl_department_master.department_id)
                    )
                    .Select(a => new
                    {
                        leave_request_id = a.leave_request_id,
                        from_date = a.from_date,
                        to_date = a.to_date,
                        r_e_id = a.r_e_id,
                        requester_date = a.requester_date,
                        requester_remarks = a.requester_remarks,
                        leave_applicable_for = a.leave_applicable_for == 1 ? "Full Day" : a.leave_applicable_for == 2 ? "Half Day" : a.leave_applicable_for == 3 ? "HH:MM" : "",
                        leave_info_id = a.leave_info_id.ToString(),
                        day_part = a.day_part.ToString(),
                        leave_applicable_in_hours_and_minutes = a.leave_applicable_in_hours_and_minutes,
                        leave_type_name = a.tbl_leave_type.leave_type_name,
                        leave_qty = a.leave_qty,
                        approval1_remarks = a.approval1_remarks,
                        is_approved1 = a.is_approved1,
                        approval2_remarks = a.approval2_remarks,
                        is_approved2 = a.is_approved2,
                        approval3_remarks = a.approval3_remarks,
                        is_approved3 = a.is_approved3,
                        is_final_approve = a.is_final_approve,
                        is_deleted = a.is_deleted,
                        emp_code = a.tbl_employee_requester.emp_code,
                        emp_name_code = string.Format("{0} {1} {2} ({3})",
                                                  a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                  a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                  a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name,
                                                  a.tbl_employee_requester.emp_code),
                        emp_name = string.Format("{0} {1} {2}",
                                                  a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                  a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                  a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name),
                        dept_Name = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_department_master.department_name,
                        designation = a.tbl_employee_requester.tbl_emp_desi_allocation.FirstOrDefault().tbl_designation_master.designation_name,
                        location = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_location_master.location_name,
                        approved_on = a.approval_date1 > a.requester_date ? a.approval_date1 : a.approval_date2 > a.requester_date ? a.approval_date2 : a.approval_date3 > a.requester_date ? a.approval_date3 : a.admin_ar_date > a.requester_date ? a.admin_ar_date : a.approval_date1,
                        approved_by = Convert.ToInt32(a.is_approved1) == 1 ? string.Format("{0} {1} {2}",
                                                a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                                Convert.ToInt32(a.is_approved2) == 1 ? string.Format("{0} {1} {2}",
                                                a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                                Convert.ToInt32(a.is_approved3) == 1 ? string.Format("{0} {1} {2}",
                                                a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                                Convert.ToInt32(a.is_admin_approve) == 1 ? string.Format("{0} {1} {2}",
                                                a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) : "",
                        approver_remarks = string.Concat(a.approval1_remarks ?? "", a.approval2_remarks ?? "", a.approval3_remarks ?? "", a.admin_remarks ?? ""),
                        status_ = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approved" : a.is_final_approve == 2 ? "Rejected" : a.is_final_approve == 3 ? "In Process" : "") : a.is_deleted == 1 ? "Deleted" : a.is_deleted == 2 ? "Cancel" : "",
                    }).Distinct().ToList();


                var compoffRequest = _context.tbl_comp_off_request_master.OrderByDescending(y => y.comp_off_request_id)
                    .Where(x => ((_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin)
                    ? (objmodel.empdtl.Count > 1 ? true : objmodel.empdtl.Contains(x.r_e_id.ToString()))
                    : objmodel.empdtl.Contains(x.r_e_id.ToString()))
                    && ((x.compoff_date.Date >= DateTime.Parse(objmodel.from_date).Date
                    && x.compoff_date.Date <= DateTime.Parse(objmodel.to_date).Date))
                    && (objmodel.company_id == -1 || objmodel.company_id == x.company_id)
                    && (objmodel.location_id == -1 || objmodel.location_id == x.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).tbl_location_master.location_id)
                    && (objmodel.department_id == -1 || objmodel.department_id == x.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).tbl_department_master.department_id)
                    )
                    .Select(a => new
                    {
                        leave_request_id = a.comp_off_request_id,
                        from_date = a.compoff_date,
                        to_date = a.compoff_date,
                        r_e_id = a.r_e_id,
                        requester_date = a.requester_date,
                        requester_remarks = a.requester_remarks,
                        leave_applicable_for = "Full Day",
                        leave_info_id = 0.ToString(),
                        day_part = "",
                        leave_applicable_in_hours_and_minutes = DateTime.MinValue,
                        leave_type_name = "Compensatory Off",
                        leave_qty = Convert.ToDouble(1),
                        approval1_remarks = a.approval1_remarks,
                        is_approved1 = a.is_approved1,
                        approval2_remarks = a.approval2_remarks,
                        is_approved2 = a.is_approved2,
                        approval3_remarks = a.approval3_remarks,
                        is_approved3 = a.is_approved3,
                        is_final_approve = a.is_final_approve,
                        is_deleted = a.is_deleted,
                        emp_code = a.tbl_employee_requester.emp_code,
                        emp_name_code = string.Format("{0} {1} {2} ({3})",
                                                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name,
                                                       a.tbl_employee_requester.emp_code),
                        emp_name = string.Format("{0} {1} {2}",
                                                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name),
                        dept_Name = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_department_master.department_name,
                        designation = a.tbl_employee_requester.tbl_emp_desi_allocation.FirstOrDefault().tbl_designation_master.designation_name,
                        location = a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_location_master.location_name,
                        approved_on = a.approval_date1 > a.requester_date ? a.approval_date1 : a.approval_date2 > a.requester_date ? a.approval_date2 : a.approval_date3 > a.requester_date ? a.approval_date3 : a.admin_ar_date > a.requester_date ? a.admin_ar_date : a.approval_date1,
                        approved_by = Convert.ToInt32(a.is_approved1) == 1 ? string.Format("{0} {1} {2}",
                                                     a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                     a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                     a.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                                     Convert.ToInt32(a.is_approved2) == 1 ? string.Format("{0} {1} {2}",
                                                     a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                     a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                     a.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                                     Convert.ToInt32(a.is_approved3) == 1 ? string.Format("{0} {1} {2}",
                                                     a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                     a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                     a.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) :
                                                     Convert.ToInt32(a.is_admin_approve) == 1 ? string.Format("{0} {1} {2}",
                                                     a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_first_name,
                                                     a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_middle_name,
                                                     a.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0).employee_last_name) : "",
                        approver_remarks = string.Concat(a.approval1_remarks ?? "", a.approval2_remarks ?? "", a.approval3_remarks ?? "", a.admin_remarks ?? ""),
                        status_ = a.is_deleted == 0 ? (a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approved" : a.is_final_approve == 2 ? "Rejected" : a.is_final_approve == 3 ? "In Process" : "") : a.is_deleted == 1 ? "Deleted" : a.is_deleted == 2 ? "Cancel" : "",
                    }).Distinct().ToList();

                var data = leaveRequest;
                if (compoffRequest != null)
                {
                    if (compoffRequest.Count > 0)
                    {
                        data = leaveRequest.Union(compoffRequest).ToList();
                    }
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 1;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        [Route("GetLeaveAppRequest/{requestid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveApplication))]
        public IActionResult GetLeaveAppRequest([FromRoute] int requestid)
        {
            Response_Msg objresponse = new Response_Msg();
            try
            {
                var data = _context.tbl_leave_request.Where(x => x.leave_request_id == requestid).FirstOrDefault();
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

#endif


        [Route("DeleteLeaveApplication")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveApplication))]
        public IActionResult DeleteLeaveApplication([FromBody] LeaveApplicationModel LeaveModel)
        {
            Response_Msg objResult = new Response_Msg();
            tbl_leave_request obkleavereq = new tbl_leave_request();
            try
            {
                var exist = _context.tbl_leave_request.Where(x => x.leave_request_id == Convert.ToInt32(LeaveModel.leave_request_id) && x.r_e_id == LeaveModel.r_e_id && x.is_deleted == 0).FirstOrDefault();
                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid request id, please check and try !!";
                    return Ok(objResult);
                }

                if (!_clsCurrentUser.DownlineEmpId.Contains(exist.r_e_id ?? 0) && _clsCurrentUser.CompanyId.Contains(exist.company_id ?? 0))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize access or Company access";
                    return Ok(objResult);
                }

                if (exist.is_final_approve == 1)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Leave application already approved can't delete !!";
                    return Ok(objResult);
                }
                else if (exist.is_final_approve == 2)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Leave application already rejected can't delete !!";
                    return Ok(objResult);
                }

                var tbl_daily_attendance = _context.tbl_daily_attendance.Where(x => x.emp_id == exist.r_e_id && exist.from_date.Date <= x.attendance_dt.Date && x.attendance_dt.Date <= exist.to_date.Date && x.is_freezed == 1).FirstOrDefault();
                if (tbl_daily_attendance != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Attendance of selected period is freezed";
                    return Ok(objResult);
                }


                exist.is_deleted = 1;
                // exist.leave_request_id = Convert.ToInt32(LeaveModel.leave_request_id);
                exist.deleted_by = Convert.ToInt32(LeaveModel.r_e_id);
                exist.deleted_dt = DateTime.Now;
                exist.deleted_remarks = "deleted by user/ Super User / Manager";

                _context.Entry(exist).State = EntityState.Modified;
                _context.SaveChanges();

                objResult.StatusCode = 0;
                objResult.Message = "Leave application deleted successfully !!";
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        //leave approval

        [Route("ApproveLeaveApplication")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveApproval))]
        public IActionResult ApproveLeaveApplication([FromBody] LeaveAppModel objmodel)
        {
            bool IsLWP = false;
            Response_Msg objResult = new Response_Msg();
            int StatusCode = 0;
            List<string> ErrorMessage = new List<string>();

            if (Convert.ToBoolean(FunIsApplicationFreezed()) == true)
            {
                objResult.StatusCode = 1;
                objResult.Message = "Leave Application has been freezed for this month";
                return Ok(objResult);
            }

            string remarksRegex = @"^[ A-Za-z0-9_.-;,']*$";
            Regex rename = new Regex(remarksRegex);

            if (!string.IsNullOrEmpty(objmodel.approval1_remarks))
            {
                if (!rename.IsMatch(objmodel.approval1_remarks))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid remarks, special characters are not allowed in remarks";
                    return Ok(objResult);
                }
            }

            // var reqempids = _context.tbl_leave_request.AsNoTracking().Where(x => x.is_deleted == 0 && x.is_final_approve != 1 && objmodel.leave_request_id.Contains(x.leave_request_id)).ToList();
            var reqempids = (from r in _context.tbl_leave_request.AsNoTracking()
                             join e in _context.tbl_employee_master.AsNoTracking() on r.r_e_id equals e.employee_id
                             join y in _context.tbl_leave_type.AsNoTracking() on r.leave_type_id equals y.leave_type_id
                             where r.is_deleted == 0 && r.is_final_approve != 1 && objmodel.leave_request_id.Contains(r.leave_request_id)
                             select new { r, y.leave_type_name, e.emp_code }).ToList();

            List<int?> _reqids = reqempids.Select(p => p.r.r_e_id).ToList();

            //foreach (var ids in _reqids)
            //{
            //    if (!_clsCurrentUser.DownlineEmpId.Any(p => p == ids))
            //    {
            //        objResult.StatusCode = 1;
            //        objResult.Message = "Unauthorize Access...!";
            //        return Ok(objResult);
            //    }
            //}

            ////if (reqempids.Any(y => y.is_final_approve == 1 || y.is_final_approve == 2))
            ////{
            ////    objResult.StatusCode = 1;
            ////    objResult.Message = "One of the Leave request aleady approve or rejected..";
            ////    return Ok(objResult);
            ////}

            List<ApplicatonMailDetails> app_mail_lst = new List<ApplicatonMailDetails>();
            ////tbl_leave_request obkleavereq = new tbl_leave_request();
            try
            {
                try
                {

                    for (int i = 0; i < objmodel.leave_request_id.Count; i++)
                    {
                        IsLWP = false;
                        //check req exist or not
                        var req = reqempids.Where(x => x.r.leave_request_id == Convert.ToInt32(objmodel.leave_request_id[i])).FirstOrDefault();

                        if (req == null)
                        {
                            StatusCode = 1;
                            if (!ErrorMessage.Any(p => p == "Select Leave Application already approve or reject, please check report !!"))
                            {
                                ErrorMessage.Add("Select Leave Application already approve or reject, please check report !!");
                            }
                            continue;
                        }
                        if (req.leave_type_name.Trim() == "LWP")
                        {
                            IsLWP = true;
                        }
                        //if (_context.tbl_leave_type.AsNoTracking().Where(p => p.leave_type_id == req.leave_type_id && p.leave_type_name.Trim() == "LWP").Count() > 0)
                        //{
                        //    IsLWP = true;
                        //}

                        var _get_frequency_type = _context.tbl_leave_credit.AsNoTracking().OrderBy(x => x.leave_credit_id).Where(p => p.leave_info_id == req.r.leave_info_id && p.is_deleted == 0).FirstOrDefault();

                        if (_get_frequency_type != null || IsLWP)
                        {

                            var leaveledger = (from a in _context.tbl_leave_ledger.AsNoTracking()
                                               where a.e_id == req.r.r_e_id && a.leave_type_id == req.r.leave_type_id
                                               group a by new
                                               {
                                                   a.e_id,
                                                   a.tbl_leave_type.leave_type_name,
                                                   a.leave_type_id,
                                                   a.tbl_leave_info.tbl_leave_type._is_el
                                               } into b
                                               select new
                                               {
                                                   e_id = b.Key.e_id,
                                                   leave_type_id = b.Key.leave_type_id,
                                                   leave_type_name = b.Key.leave_type_name,
                                                   _is_el = (b.Key._is_el == null || b.Key._is_el == 0) ? 0 : b.Key._is_el,
                                                   totalcredit = b.Sum(x => x.credit),
                                                   totaldebit = b.Sum(y => y.dredit),
                                                   leavebalance = b.Sum(x => x.credit) - b.Sum(y => y.dredit)

                                               }).FirstOrDefault();

                            //var empcode = (from a in _context.tbl_employee_master.AsNoTracking()
                            //               where a.employee_id == req.r_e_id
                            //               select a.emp_code).FirstOrDefault();


                            if (!IsLWP && objmodel.is_approved1 == 1)
                            {
                                if (leaveledger == null)
                                {
                                    StatusCode = 1;
                                    if (!ErrorMessage.Any(p => p == "Leave not available in employee (" + req.emp_code + ") account, please check and try !!"))
                                    {
                                        ErrorMessage.Add("Leave not available in employee (" + req.emp_code + ") account, please check and try !!");
                                    }
                                    continue;
                                }

                                if (leaveledger != null && leaveledger.leavebalance < req.r.leave_qty)
                                {
                                    StatusCode = 1;
                                    if (!ErrorMessage.Any(p => p == "Leave not available in employee (" + req.emp_code + ") account, please check and try !!"))
                                    {
                                        ErrorMessage.Add("Leave not available in employee (" + req.emp_code + ") account, please check and try !!");
                                    }
                                    continue;
                                }
                            }

                            //check attandance is freeze or not
                            if (objmodel.is_approved1 != 2)
                            {
                                var tbl_daily_attendance = _context.tbl_daily_attendance.Where(x => x.emp_id == req.r.r_e_id && req.r.from_date.Date <= x.attendance_dt.Date && x.attendance_dt.Date <= req.r.to_date.Date && x.is_freezed == 1).FirstOrDefault();
                                if (tbl_daily_attendance != null)
                                {
                                    StatusCode = 1;
                                    if (!ErrorMessage.Any(p => p == "Attendance of selected period is freezed"))
                                    {
                                        ErrorMessage.Add("Attendance of selected period is freezed");
                                    }
                                    continue;
                                }
                            }

                            if (_clsCurrentUser.Is_Hod == 1)
                            {
                                var emp_manager = _context.tbl_emp_manager.Where(a => a.employee_id == req.r.r_e_id && a.is_deleted == 0).OrderByDescending(b => b.emp_mgr_id).FirstOrDefault();
                                if (emp_manager != null)
                                {
                                    int final_approver = emp_manager.final_approval;
                                    int final_app_id = 0;

                                    if (emp_manager.m_one_id == objmodel.a1_e_id)
                                    {
                                        req.r.is_approved1 = objmodel.is_approved1;
                                        req.r.approval1_remarks = objmodel.approval1_remarks;
                                        req.r.a1_e_id = objmodel.a1_e_id;
                                        req.r.approval_date1 = DateTime.Now;
                                    }
                                    else if (emp_manager.m_two_id == objmodel.a1_e_id)
                                    {
                                        req.r.is_approved2 = objmodel.is_approved1;
                                        req.r.approval2_remarks = objmodel.approval1_remarks;
                                        req.r.a2_e_id = objmodel.a1_e_id;
                                        req.r.approval_date2 = DateTime.Now;
                                    }
                                    else if (emp_manager.m_three_id == objmodel.a1_e_id)
                                    {
                                        req.r.is_approved3 = objmodel.is_approved1;
                                        req.r.approval3_remarks = objmodel.approval1_remarks;
                                        req.r.a3_e_id = objmodel.a1_e_id;
                                        req.r.approval_date3 = DateTime.Now;
                                    }

                                    final_app_id = (final_approver == 1 ? emp_manager.m_one_id : final_approver == 2 ? emp_manager.m_two_id : final_approver == 3 ? emp_manager.m_three_id : 0) ?? 0;

                                    //if (final_app_id == objmodel.a1_e_id)
                                    //{
                                        req.r.is_final_approve = objmodel.is_approved1;
                                    //}

                                    // _context.tbl_leave_request.UpdateRange(req);
                                }
                                else
                                {
                                    StatusCode = 1;
                                    if (!ErrorMessage.Any(p => p == "You cannot approve leave application"))
                                    {
                                        ErrorMessage.Add("You cannot approve leave application");
                                    }
                                    continue;
                                }
                            }
                            else if (_clsCurrentUser.Is_Hod == 2)
                            {
                                req.r.is_final_approve = objmodel.is_approved1;
                                req.r.is_admin_approve = objmodel.is_approved1;
                                req.r.admin_id = objmodel.a1_e_id;
                                req.r.admin_remarks = objmodel.approval1_remarks;
                                req.r.admin_ar_date = DateTime.Now;
                            }
                            else
                            {
                                StatusCode = 1;
                                if (!ErrorMessage.Any(p => p == "You are not an approver of selected leave application"))
                                {
                                    ErrorMessage.Add("You are not an approver of selected leave application");
                                }
                                continue;
                            }

                            //start by supriya debit in leave ledger
                            if (req.r.is_final_approve == 1 && (!IsLWP))
                            {
                                tbl_leave_ledger objtbl_leave_ledger = new tbl_leave_ledger();
                                var a = _context.tbl_leave_ledger.AsNoTracking().Where(x => x.e_id == req.r.r_e_id
                                && x.transaction_type == Convert.ToByte(enmLeaveTransactionType.Consumed)
                                && x.leave_type_id == req.r.leave_type_id && x.transaction_date == req.r.from_date
                                && x.leave_info_id == req.r.leave_info_id && x.dredit == req.r.leave_qty
                                && (x.created_by == (req.r.a1_e_id > 0 ? req.r.a1_e_id : req.r.a2_e_id > 0 ? req.r.a2_e_id
                                : req.r.a3_e_id > 0 ? req.r.a3_e_id : req.r.admin_id))).ToList();

                                //if (a.Count() < 1)
                                //{
                                    objtbl_leave_ledger.leave_type_id = req.r.leave_type_id;
                                    objtbl_leave_ledger.leave_info_id = req.r.leave_info_id;
                                    objtbl_leave_ledger.transaction_date = req.r.from_date;
                                    objtbl_leave_ledger.entry_date = DateTime.Now;
                                    objtbl_leave_ledger.transaction_type = Convert.ToByte(enmLeaveTransactionType.Consumed); // leave consume
                                    objtbl_leave_ledger.monthyear = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
                                    objtbl_leave_ledger.transaction_no = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
                                    objtbl_leave_ledger.leave_addition_type = _get_frequency_type.frequency_type;
                                    objtbl_leave_ledger.credit = 0;
                                    objtbl_leave_ledger.dredit = req.r.leave_qty;
                                    objtbl_leave_ledger.remarks = objmodel.approval1_remarks;
                                    objtbl_leave_ledger.e_id = req.r.r_e_id;
                                    objtbl_leave_ledger.created_by = req.r.a1_e_id > 0 ? req.r.a1_e_id : req.r.a2_e_id > 0 ? req.r.a2_e_id : req.r.a3_e_id > 0 ? req.r.a3_e_id : req.r.admin_id;

                                    _context.tbl_leave_ledger.AddRange(objtbl_leave_ledger);
                                    //_context.Entry(objtbl_leave_ledger).State = EntityState.Added;
                                    _context.SaveChanges();
                                //}
                            }
                            // end by supriya debit in leave ledger
                            if (req.r.is_final_approve == 1 || req.r.is_final_approve == 2)
                            {
                                ApplicatonMailDetails objmail = new ApplicatonMailDetails();
                                objmail.req_id = req.r.leave_request_id;
                                objmail.emp_id = req.r.r_e_id ?? 0;
                                objmail.from_date = req.r.from_date;
                                objmail.to_date = req.r.to_date;
                                objmail.is_approve = req.r.is_final_approve;
                                objmail.approver_remarks = objmodel.approval1_remarks;
                                objmail.a_e_id = req.r.a1_e_id > 0 ? req.r.a1_e_id : req.r.a2_e_id > 0 ? req.r.a2_e_id : req.r.a3_e_id > 0 ? req.r.a3_e_id : req.r.admin_id;
                                objmail.leave_type_id = req.r.leave_type_id ?? 0;
                                //objmail.leave_type = req.tbl_leave_type.leave_type_name;
                                app_mail_lst.Add(objmail);

                            }
                            tbl_leave_request objrq = new tbl_leave_request();
                            objrq = req.r;
                            _context.tbl_leave_request.UpdateRange(objrq);
                        }
                        else
                        {
                            StatusCode = 1;
                            if (!ErrorMessage.Any(p => p == "Please enter leave credit details under Leave Settings"))
                            {
                                ErrorMessage.Add("Please enter leave credit details under Leave Settings");
                            }
                            continue;
                        }

                    }
                    _context.SaveChanges();
                    if (app_mail_lst.Count > 0)
                    {
                        MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);
                        Task task = Task.Run(() => obj_ms.LeaveApplicationApprovalMail(app_mail_lst));
                        task.Wait();
                    }

                    objResult.StatusCode = StatusCode;
                    if (objmodel.is_approved1 == 1) objResult.Message = "" + app_mail_lst.Count + " application approved successfully !! <br/>" + string.Join("<br/>", ErrorMessage);
                    if (objmodel.is_approved1 == 2) objResult.Message = "" + app_mail_lst.Count + " application rejected successfully !! <br/>" + string.Join("<br/>", ErrorMessage);

                    return Ok(objResult);
                }
                catch (Exception ex)
                {
                    objResult.StatusCode = 2;
                    objResult.Message = ex.Message;
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


#endregion


        [Route("GetOutdoorLeaveForApproval/{LoginEmployeeId}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.OutdoorApproval))]
        public IActionResult GetOutdoorLeaveForApproval([FromRoute] int LoginEmployeeId)
        {

            try
            {
                ResponseMsg objResult = new ResponseMsg();



                if (!_clsCurrentUser.DownlineEmpId.Any(p => p == LoginEmployeeId))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access...!";
                    return Ok(objResult);
                }

                List<int> emplst = new List<int>();


                if (!_clsCurrentUser.RoleId.Contains((int)enmRoleMaster.SuperAdmin))
                {

                    List<int> loginid = new List<int>();
                    loginid.Add(_clsCurrentUser.EmpId);
                    emplst = _clsCurrentUser.DownlineEmpId.Except(loginid).ToList();
                }
                else
                {
                    emplst = _clsCurrentUser.DownlineEmpId.ToList();
                }


                if (_clsCurrentUser.Is_Hod == 1)
                {
                    var result = _context.tbl_outdoor_request.OrderByDescending(y => y.leave_request_id).Where(x => _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0) && x.is_deleted == 0 && x.is_final_approve != 1 && x.is_final_approve != 2 &&
                                    emplst.Contains(x.r_e_id ?? 0) && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).Select(p => new
                                    {
                                        leave_request_id = p.leave_request_id,
                                        employee_id = p.r_e_id,
                                        requester_id = p.r_e_id,
                                        employee_name = string.Format("{0} {1} {2}",
                                              p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                                              p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                                              p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name),
                                        employee_code = p.tbl_employee_requester.emp_code,
                                        from_date = p.from_date,
                                        manual_in_time = p.manual_in_time,
                                        manual_out_time = p.manual_out_time,
                                        requester_remarks = p.requester_remarks,
                                        status = p.is_final_approve == 0 ? "Pending" : p.is_final_approve == 1 ? "Approve" : p.is_final_approve == 2 ? "Reject" : "",
                                        system_in_time = p.manual_in_time,
                                        system_out_time = p.manual_out_time,
                                        approver_remarks = (p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_one_id == _clsCurrentUser.EmpId ? p.approval1_remarks :
                                              p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_two_id == _clsCurrentUser.EmpId ? p.approval2_remarks :
                                              p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_three_id == _clsCurrentUser.EmpId ? p.approval3_remarks : ""),
                                        my_status = (p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_one_id == _clsCurrentUser.EmpId ? (p.is_approved1 == 0 ? "Pending" : p.is_approved1 == 1 ? "Approve" : p.is_approved1 == 2 ? "Reject" : "") :
                                              p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_two_id == _clsCurrentUser.EmpId ? (p.is_approved2 == 0 ? "Pending" : p.is_approved2 == 1 ? "Approve" : p.is_approved2 == 2 ? "Reject" : "") :
                                              p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_three_id == _clsCurrentUser.EmpId ? (p.is_approved3 == 0 ? "Pending" : p.is_approved3 == 1 ? "Approve" : p.is_approved3 == 2 ? "Reject" : "") : ""),
                                        my_status_level = (p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_one_id == _clsCurrentUser.EmpId ? 1 :
                                            p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_two_id == _clsCurrentUser.EmpId ? 2 :
                                            p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_three_id == _clsCurrentUser.EmpId ? 3 : 0),
                                        longitude = p.longitude,
                                        latitude = p.latitude,
                                        location = p.location,
                                    }).Distinct().ToList();

                    return Ok(result);
                }
                else if (_clsCurrentUser.Is_Hod == 2)
                {
                    var result = _context.tbl_outdoor_request.Where(p => emplst.Contains(p.r_e_id ?? 0) && _clsCurrentUser.CompanyId.Contains(p.company_id ?? 0))
                        .OrderByDescending(y => y.leave_request_id).Where(x => x.is_deleted == 0 && x.is_final_approve != 1 && x.is_final_approve != 2).Select(p => new
                        {
                            leave_request_id = p.leave_request_id,
                            employee_id = p.r_e_id,
                            requester_id = p.r_e_id,
                            employee_name = string.Format("{0} {1} {2}",
                                          p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                                          p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                                          p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name),
                            employee_code = p.tbl_employee_requester.emp_code,
                            from_date = p.from_date,
                            manual_in_time = p.manual_in_time,
                            manual_out_time = p.manual_out_time,
                            requester_remarks = p.requester_remarks,
                            status = p.is_final_approve == 0 ? "Pending" : p.is_final_approve == 1 ? "Approve" : p.is_final_approve == 2 ? "Reject" : "",
                            system_in_time = p.manual_in_time,
                            system_out_time = p.manual_out_time,
                            approver_remarks = p.admin_remarks,
                            my_status = p.is_admin_approve == 0 ? "Pending" : p.is_admin_approve == 1 ? "Approve" : p.is_admin_approve == 2 ? "Reject" : "",
                            my_status_level = 1,
                            longitude = p.longitude,
                            latitude = p.latitude,
                            location = p.location,
                        }).Distinct().ToList();

                    return Ok(result);
                }
                else
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "You are not an approver of any outdoor application";
                    return Ok(objResult);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("GetAttandenceApproval/{LoginEmployeeId}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.AttendanceApproval))]
        public IActionResult GetAttandenceApproval([FromRoute] int LoginEmployeeId)
        {

            try
            {
                ResponseMsg objResult = new ResponseMsg();


                if (!_clsCurrentUser.DownlineEmpId.Any(p => p == LoginEmployeeId))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access...!";
                    return Ok(objResult);
                }

                List<int> under_emp = new List<int>();
                if (!_clsCurrentUser.RoleId.Contains((int)enmRoleMaster.SuperAdmin))
                {

                    List<int> _loginid = new List<int>();
                    _loginid.Add(_clsCurrentUser.EmpId);
                    under_emp = _clsCurrentUser.DownlineEmpId.Except(_loginid).ToList();
                }
                else
                {
                    under_emp = _clsCurrentUser.DownlineEmpId.ToList();
                }


                if (_clsCurrentUser.Is_Hod == 1)
                {

                    var result = _context.tbl_attendace_request.Join(_context.tbl_daily_attendance, a => new { _date = a.from_date, emp_id = a.r_e_id }, b => new { _date = b.attendance_dt, emp_id = b.emp_id }, (a, b) => new
                    {

                        employee_id = a.r_e_id,
                        employee_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                                                     a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                                                     a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name),
                        employee_code = a.tbl_employee_requester.emp_code,
                        leave_request_id = a.leave_request_id,
                        from_date = a.from_date,
                        manual_in_time = a.manual_in_time,
                        manual_out_time = a.manual_out_time,
                        requester_remarks = a.requester_remarks,
                        status = a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : a.is_final_approve == 2 ? "Reject" : a.is_final_approve == 3 ? "In Process" : "",
                        system_in_time = b.in_time,
                        system_out_time = b.out_time,
                        approver_remarks = (a.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_one_id == _clsCurrentUser.EmpId ? a.approval1_remarks :
                                                a.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_two_id == _clsCurrentUser.EmpId ? a.approval2_remarks :
                                                a.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_three_id == _clsCurrentUser.EmpId ? a.approval3_remarks : ""),
                        my_status = (a.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_one_id == _clsCurrentUser.EmpId ? (a.is_approved1 == 0 ? "Pending" : a.is_approved1 == 1 ? "Approved" : a.is_approved1 == 2 ? "Reject" : "") :
                                    a.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_two_id == _clsCurrentUser.EmpId ? (a.is_approved2 == 0 ? "Pending" : a.is_approved2 == 1 ? "Approved" : a.is_approved2 == 2 ? "Reject" : "") :
                                    a.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_three_id == _clsCurrentUser.EmpId ? (a.is_approved3 == 0 ? "Pending" : a.is_approved3 == 1 ? "Approved" : a.is_approved3 == 2 ? "Reject" : "") : ""),
                        diff = b.out_time.Subtract(b.in_time),
                        diff_time = (Math.Round(b.out_time.Subtract(b.in_time).TotalHours, 2)) >= 0 ? Math.Round(b.out_time.Subtract(b.in_time).TotalHours, 2) : 0,
                        // workinghrs = (TempIndex.system_out_time - TempIndex.system_in_time).TotalHours
                        my_status_level = 1,
                        requester_date = a.requester_date,
                        a.is_deleted,
                        a.is_final_approve,
                        mgr1 = a.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_one_id,
                        mgr2 = a.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_two_id,
                        mgr3 = a.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_three_id,
                        b.is_freezed,
                        a.company_id,

                    }).Where(x => x.is_deleted == 0 && x.is_final_approve != 1 && x.is_final_approve != 2 && x.is_freezed == 0 && under_emp.Contains(x.employee_id ?? 0) && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).Distinct().ToList();

                    return Ok(result);

                }
                else if (_clsCurrentUser.Is_Hod == 2)
                {
                    var result = _context.tbl_attendace_request.Join(_context.tbl_daily_attendance, a => new { _date = a.from_date, emp_id = a.r_e_id }, b => new { _date = b.attendance_dt, emp_id = b.emp_id }, (a, b) => new
                    {

                        employee_id = a.r_e_id,
                        employee_name = string.Format("{0} {1} {2}", a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                                          a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                                          a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name),
                        employee_code = a.tbl_employee_requester.emp_code,
                        leave_request_id = a.leave_request_id,
                        from_date = a.from_date,
                        manual_in_time = a.manual_in_time,
                        manual_out_time = a.manual_out_time,
                        requester_remarks = a.requester_remarks,
                        status = a.is_final_approve == 0 ? "Pending" : a.is_final_approve == 1 ? "Approve" : a.is_final_approve == 2 ? "Reject" : a.is_final_approve == 3 ? "In Process" : "",
                        system_in_time = b.in_time,
                        system_out_time = b.out_time,
                        approver_remarks = a.admin_remarks,
                        my_status = a.is_admin_approve == 0 ? "Pending" : a.is_admin_approve == 1 ? "Approve" : a.is_admin_approve == 2 ? "Reject" : "",
                        diff = b.out_time.Subtract(b.in_time),
                        diff_time = (Math.Round(b.out_time.Subtract(b.in_time).TotalHours, 2)) >= 0 ? Math.Round(b.out_time.Subtract(b.in_time).TotalHours, 2) : 0,
                        // workinghrs = (TempIndex.system_out_time - TempIndex.system_in_time).TotalHours
                        my_status_level = 1,
                        requester_date = a.requester_date,
                        a.is_deleted,
                        a.is_final_approve,
                        b.is_freezed,
                        a.company_id

                    }).Where(x => x.is_deleted == 0 && x.is_final_approve != 1 && x.is_final_approve != 2 && x.is_freezed == 0 && under_emp.Contains(x.employee_id ?? 0) && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).ToList();

                    return Ok(result);
                }
                else
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "You are not an approver of any Attendance application";
                    return Ok(objResult);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("GetCompOffLeaveApproval/{LoginEmployeeId}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffApproval))]
        public IActionResult GetCompOffLeaveApproval([FromRoute] int LoginEmployeeId)
        {
            ResponseMsg objResult = new ResponseMsg();

            try
            {

                if (!_clsCurrentUser.DownlineEmpId.Any(p => p == LoginEmployeeId))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access...!";
                    return Ok(objResult);
                }

                List<int> emplst = new List<int>();
                if (!_clsCurrentUser.RoleId.Contains((int)enmRoleMaster.SuperAdmin))
                {
                    List<int> loginid = new List<int>();
                    emplst = _clsCurrentUser.DownlineEmpId.Except(loginid).ToList();
                }
                else
                {
                    emplst = _clsCurrentUser.DownlineEmpId.ToList();
                }

                var attandance_dtl = _context.tbl_daily_attendance.Where(x => x.is_freezed == 0 && emplst.Contains(x.emp_id ?? 0)).ToList();

                if (_clsCurrentUser.Is_Hod == 1)
                {

                    var pending_request = (from e in _context.tbl_comp_off_request_master
                                           join d in attandance_dtl on new { _date = e.compoff_date.Date, _empidd = e.r_e_id } equals new { _date = d.attendance_dt.Date, _empidd = d.emp_id } into ej
                                           from d in ej.DefaultIfEmpty()
                                           where e.is_deleted == 0 && e.is_final_approve != 1 && e.is_final_approve != 2 && emplst.Contains(e.r_e_id ?? 0) && _clsCurrentUser.CompanyId.Contains(e.company_id ?? 0)
                                           select new
                                           {

                                               employee_id = e.r_e_id,
                                               employee_name = string.Format("{0} {1} {2} ({3})", e.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                                                 e.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                                                 e.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                                                 e.tbl_employee_requester.emp_code),
                                               leave_request_id = e.comp_off_request_id,
                                               from_date = e.compoff_date,
                                               manual_in_time = e.compoff_against_date,
                                               manual_out_time = e.compoff_against_date,
                                               requester_remarks = e.requester_remarks,
                                               status = e.is_final_approve == 0 ? "Pending" : e.is_final_approve == 1 ? "Approve" : e.is_final_approve == 2 ? "Reject" : e.is_final_approve == 3 ? "In Process" : "",
                                               system_in_time = d != null ? d.in_time : Convert.ToDateTime("2000-01-01 00:00"),
                                               system_out_time = d != null ? d.out_time : Convert.ToDateTime("2000-01-01 00:00"),
                                               approver_remarks = (e.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_one_id == _clsCurrentUser.EmpId ? e.approval1_remarks :
                                                                     e.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_two_id == _clsCurrentUser.EmpId ? e.approval2_remarks :
                                                                     e.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_three_id == _clsCurrentUser.EmpId ? e.approval3_remarks : ""),
                                               my_status = (e.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_one_id == _clsCurrentUser.EmpId ? (e.is_approved1 == 0 ? "Pending" : e.is_approved1 == 1 ? "Approved" : e.is_approved1 == 2 ? "Rejected" : "") :
                                                                 e.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_two_id == _clsCurrentUser.EmpId ? (e.is_approved2 == 0 ? "Pending" : e.is_approved2 == 1 ? "Approved" : e.is_approved2 == 2 ? "Rejected" : "") :
                                                                e.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_three_id == _clsCurrentUser.EmpId ? (e.is_approved3 == 0 ? "Pending" : e.is_approved3 == 1 ? "Approved" : e.is_approved3 == 2 ? "Rejected" : "") : ""),
                                               my_status_level = (e.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_one_id == _clsCurrentUser.EmpId ? 1 :
                                                                     e.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_two_id == _clsCurrentUser.EmpId ? 2 :
                                                                     e.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_three_id == _clsCurrentUser.EmpId ? 3 : 0),

                                               e.is_deleted,
                                               e.is_final_approve,
                                               e.compoff_against_date,
                                           }).Distinct().ToList();




                    return Ok(pending_request);
                }
                else if (_clsCurrentUser.Is_Hod == 2)
                {
                    var pending_request = (from e in _context.tbl_comp_off_request_master
                                           join d in attandance_dtl on new { _date = e.compoff_date.Date, _empidd = e.r_e_id } equals new { _date = d.attendance_dt.Date, _empidd = d.emp_id } into ej
                                           from d in ej.DefaultIfEmpty()
                                           where e.is_deleted == 0 && e.is_final_approve != 1 && e.is_final_approve != 2 && emplst.Contains(e.r_e_id ?? 0) && _clsCurrentUser.CompanyId.Contains(e.company_id ?? 0)
                                           select new
                                           {

                                               employee_id = e.r_e_id,
                                               employee_name = string.Format("{0} {1} {2} ({3})", e.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                                                 e.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                                                 e.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                                                 e.tbl_employee_requester.emp_code),
                                               leave_request_id = e.comp_off_request_id,
                                               from_date = e.compoff_date,
                                               manual_in_time = e.compoff_against_date,
                                               manual_out_time = e.compoff_against_date,
                                               requester_remarks = e.requester_remarks,
                                               status = e.is_final_approve == 0 ? "Pending" : e.is_final_approve == 1 ? "Approve" : e.is_final_approve == 2 ? "Reject" : e.is_final_approve == 3 ? "In Process" : "",
                                               system_in_time = d != null ? d.in_time : Convert.ToDateTime("2000-01-01 00:00"),
                                               system_out_time = d != null ? d.out_time : Convert.ToDateTime("2000-01-01 00:00"),
                                               approver_remarks = e.admin_remarks,
                                               my_status = e.is_admin_approve == 0 ? "Pending" : e.is_admin_approve == 1 ? "Approved" : e.is_admin_approve == 2 ? "Rejected" : "",
                                               //my_status_level = 1,

                                               e.is_deleted,
                                               e.is_final_approve,
                                               e.compoff_against_date,
                                           }).Distinct().ToList();

                    return Ok(pending_request);
                }
                else
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "You are not an approver of any compoff application";
                    return Ok(objResult);
                }

                //var result = (from or in _context.tbl_comp_off_request_master
                //              join em in _context.tbl_emp_manager on or.r_e_id equals em.employee_id
                //              where or.is_final_approve == 0 && or.is_deleted == 0
                //              //&& 
                //              //or.r_e_id == em.employee_id && em.m_one_id == LoginEmployeeId || em.m_two_id == LoginEmployeeId ||
                //              //em.m_three_id == LoginEmployeeId
                //              select new
                //              {
                //                  comp_off_request_id = or.comp_off_request_id,
                //                  compoff_against_date = or.compoff_against_date,
                //                  compoff_date = or.compoff_date,
                //                  requester_remarks = or.requester_remarks
                //              }).ToList();



                //var result = (from or in _context.tbl_comp_off_request_master
                //              join em in _context.tbl_emp_manager on or.r_e_id equals em.employee_id
                //              join o in _context.tbl_emp_officaial_sec on or.r_e_id equals o.employee_id
                //              join d in _context.tbl_daily_attendance on or.compoff_against_date equals d.attendance_dt
                //              //join d in _context.tbl_daily_attendance on or.compoff_date equals d.attendance_dt
                //              where or.is_final_approve == 0 &&
                //              (em.m_one_id == LoginEmployeeId || em.m_two_id == LoginEmployeeId || em.m_three_id == LoginEmployeeId) && em.is_deleted == 0
                //              && o.is_deleted == 0 && o.employee_first_name != null && o.employee_first_name != "0"
                //              select new
                //              {
                //                  employee_name = string.Format("{0} {1}  {2} ({3})", o.employee_first_name, o.employee_middle_name, o.employee_last_name, o.tbl_employee_id_details.emp_code).Trim(),
                //                  employee_id = or.r_e_id,
                //                  leave_request_id = or.comp_off_request_id,
                //                  from_date = or.compoff_date,
                //                  manual_in_time = or.compoff_against_date,
                //                  manual_out_time = or.compoff_against_date,
                //                  requester_remarks = or.requester_remarks,
                //                  status = or.is_final_approve == 0 ? "Pending" : or.is_final_approve == 1 ? "Approve" : "Reject",
                //                  requester_id = or.r_e_id,
                //                  system_in_time = d.in_time,
                //                  system_out_time = d.out_time,
                //                  or.is_final_approve,
                //                  or.is_approved1,
                //                  or.is_approved2,
                //                  or.is_approved3,
                //                  or.approval1_remarks,
                //                  or.approval2_remarks,
                //                  or.approval3_remarks
                //              }).ToList();



                //var result1 = result.Where(a => a.employee_name != "").GroupBy(a => new { a.leave_request_id, a.employee_id, a.employee_name, a.from_date, a.manual_in_time, a.manual_out_time, a.requester_remarks, a.system_in_time, a.system_out_time, a.status, a.is_final_approve, a.is_approved1, a.is_approved2, a.is_approved3, a.approval1_remarks, a.approval2_remarks, a.approval3_remarks }).Select(a => a.Key).ToList();

                //var final_result = result1.OrderByDescending(y=>y.leave_request_id).Where(a => a.is_final_approve == 0).GroupBy(a => a.leave_request_id).Select(a => a.First()).ToList();

                //List<AllLeaveRequestReport> obj_leave_req = new List<AllLeaveRequestReport>();

                //for (int Index = 0; Index < final_result.Count; Index++)
                //{
                //    var tbl_emp_m = _context.tbl_emp_manager.Where(x => x.employee_id == final_result[Index].employee_id && x.is_deleted == 0).OrderByDescending(a => a.emp_mgr_id).FirstOrDefault();

                //    if (tbl_emp_m.m_one_id == LoginEmployeeId)
                //    {
                //        obj_leave_req = final_result.Select(TempIndex => new AllLeaveRequestReport
                //        {
                //            employee_id = Convert.ToInt32(TempIndex.employee_id),
                //            employee_name = TempIndex.employee_name,
                //            leave_request_id = TempIndex.leave_request_id,
                //            from_date = TempIndex.from_date,
                //            manual_in_time = TempIndex.manual_in_time,
                //            manual_out_time = TempIndex.manual_out_time,
                //            requester_remarks = TempIndex.requester_remarks,
                //            status = TempIndex.status,
                //            system_in_time = TempIndex.system_in_time,
                //            system_out_time = TempIndex.system_out_time,
                //            approver_remarks = TempIndex.approval1_remarks,
                //            my_status = TempIndex.is_approved1 == 0 ? "Pending" : TempIndex.is_approved1 == 1 ? "Approve" : "Reject",
                //            my_status_level = 1,
                //        }).ToList();
                //    }
                //    else if (tbl_emp_m.m_two_id == LoginEmployeeId)
                //    {
                //        obj_leave_req = final_result.Select(TempIndex => new AllLeaveRequestReport
                //        {
                //            employee_id = Convert.ToInt32(TempIndex.employee_id),
                //            employee_name = TempIndex.employee_name,
                //            leave_request_id = TempIndex.leave_request_id,
                //            from_date = TempIndex.from_date,
                //            manual_in_time = TempIndex.manual_in_time,
                //            manual_out_time = TempIndex.manual_out_time,
                //            requester_remarks = TempIndex.requester_remarks,
                //            status = TempIndex.status,
                //            system_in_time = TempIndex.system_in_time,
                //            system_out_time = TempIndex.system_out_time,
                //            approver_remarks = TempIndex.approval2_remarks,
                //            my_status = TempIndex.is_approved2 == 0 ? "Pending" : TempIndex.is_approved2 == 1 ? "Approve" : "Reject",
                //            my_status_level = 2,
                //        }).ToList();
                //    }
                //    else
                //    {

                //        obj_leave_req = final_result.Select(TempIndex => new AllLeaveRequestReport
                //        {
                //            employee_id = Convert.ToInt32(TempIndex.employee_id),
                //            employee_name = TempIndex.employee_name,
                //            leave_request_id = TempIndex.leave_request_id,
                //            from_date = TempIndex.from_date,
                //            manual_in_time = TempIndex.manual_in_time,
                //            manual_out_time = TempIndex.manual_out_time,
                //            requester_remarks = TempIndex.requester_remarks,
                //            status = TempIndex.status,
                //            system_in_time = TempIndex.system_in_time,
                //            system_out_time = TempIndex.system_out_time,
                //            approver_remarks = TempIndex.approval3_remarks,
                //            my_status = TempIndex.is_approved3 == 0 ? "Pending" : TempIndex.is_approved3 == 1 ? "Approve" : "Reject",
                //            my_status_level = 3,
                //        }).ToList();
                //    }



                //}
                //if (obj_leave_req == null)
                //{
                //    objResult.Message = "Record Not Found...!";
                //    objResult.StatusCode = 0;
                //    return Ok(objResult);
                //}
                //else
                //{
                //    return Ok(obj_leave_req);
                //}
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }


        //Save OutdoorLeaveForApproval
        [Route("Save_OutdoorLeaveForApproval")]
        [HttpPost]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.OutdoorApproval))]
        public async Task<IActionResult> Save_OutdoorLeaveForApproval([FromBody] LeaveAppModel objmodel)
        {
            Response_Msg objResult = new Response_Msg();
            try
            {

                if (Convert.ToBoolean(FunIsApplicationFreezed()) == true)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Outdoor Application Approval has been freezed for this month";
                    return Ok(objResult);
                }

                //check especial character in remarks

                string remarksRegex = @"^[ A-Za-z0-9_.-;,']*$";

                Regex rename = new Regex(remarksRegex);

                if (!string.IsNullOrEmpty(objmodel.approval1_remarks))
                {
                    if (!rename.IsMatch(objmodel.approval1_remarks))
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Invalid remarks, special characters are not allowed in remarks";
                        return Ok(objResult);
                    }
                }


                List<ApplicatonMailDetails> app_mail_lst = new List<ApplicatonMailDetails>();

                var ReqEmpIds = _context.tbl_outdoor_request.Where(x => _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0) && objmodel.leave_request_id.Contains(x.leave_request_id) && x.is_deleted == 0 && x.is_final_approve == 0).ToList();

                List<int?> _reqids = ReqEmpIds.Select(p => p.r_e_id).ToList();
                foreach (var ReqEmpId in _reqids)
                {
                    if (!_clsCurrentUser.DownlineEmpId.Any(p => p == ReqEmpId))
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Unauthorize Access...!";
                        return Ok(objResult);
                    }
                }

                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        for (int i = 0; i < objmodel.leave_request_id.Count; i++)
                        {
                            //check req exist or not
                            var req = ReqEmpIds.Where(x => x.leave_request_id == Convert.ToInt32(objmodel.leave_request_id[i])).FirstOrDefault();
                            if (req == null)
                            {
                                objResult.StatusCode = 1;
                                objResult.Message = "Invalid request id found or Unauthorize Company Access , please try after some time !!";
                                return Ok(objResult);
                            }

                            if (objmodel.is_approved1 != 2)
                            {
                                var tbl_daily_attendance = _context.tbl_daily_attendance.Where(x => x.emp_id == req.r_e_id && x.attendance_dt.Date == req.from_date.Date && x.is_freezed == 1 && _clsCurrentUser.CompanyId.Contains(req.company_id ?? 0)).FirstOrDefault();
                                if (tbl_daily_attendance != null)
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "Attendance of selected period is freezed";
                                    return Ok(objResult);
                                }
                            }



                            if (_clsCurrentUser.Is_Hod == 1)
                            {
                                var tbl_emp_m = _clsEmployeeDetail.Get_Emp_manager_dtl(req.r_e_id ?? 0).ToList();
                                if (tbl_emp_m != null)
                                {
                                    int final_app_mgr_id = 0;
                                    int final_approvel = tbl_emp_m[0].final_approval;

                                    if (tbl_emp_m[0].m_one_id == objmodel.a1_e_id)
                                    {
                                        req.is_approved1 = objmodel.is_approved1;
                                        req.approval1_remarks = objmodel.approval1_remarks;
                                        req.a1_e_id = objmodel.a1_e_id;
                                        req.approval_date1 = DateTime.Now;
                                    }
                                    else if (tbl_emp_m[0].m_two_id == objmodel.a1_e_id)
                                    {
                                        req.is_approved2 = objmodel.is_approved1;
                                        req.approval2_remarks = objmodel.approval1_remarks;
                                        req.a2_e_id = objmodel.a1_e_id;
                                        req.approval_date2 = DateTime.Now;
                                    }
                                    else if (tbl_emp_m[0].m_three_id == objmodel.a1_e_id)
                                    {
                                        req.is_approved3 = objmodel.is_approved1;
                                        req.approval3_remarks = objmodel.approval1_remarks;
                                        req.a3_e_id = objmodel.a1_e_id;
                                        req.approval_date3 = DateTime.Now;
                                    }

                                    final_app_mgr_id = final_approvel == 1 ? tbl_emp_m[0].m_one_id : final_approvel == 2 ? tbl_emp_m[0].m_two_id : final_approvel == 3 ? tbl_emp_m[0].m_three_id : 0;

                                    if (final_app_mgr_id == objmodel.a1_e_id)
                                    {
                                        req.is_final_approve = objmodel.is_approved1;
                                    }

                                }
                                else
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "Please Create Approver";
                                    return Ok(objResult);
                                }
                            }
                            else if (_clsCurrentUser.Is_Hod == 2)
                            {
                                req.is_final_approve = objmodel.is_approved1;
                                req.is_admin_approve = objmodel.is_approved1;
                                req.admin_id = objmodel.a1_e_id;
                                req.admin_remarks = objmodel.approval1_remarks;
                                req.admin_ar_date = DateTime.Now;
                            }
                            else
                            {
                                objResult.StatusCode = 1;
                                objResult.Message = "You cannot approve Leave applcation";
                                break;
                            }

                            if (req.is_final_approve == 1 || req.is_final_approve == 2)
                            {
                                ApplicatonMailDetails objmail = new ApplicatonMailDetails();
                                objmail.emp_id = req.r_e_id ?? 0;
                                objmail.req_id = req.leave_request_id;
                                objmail.from_date = req.from_date;
                                objmail.is_approve = req.is_final_approve;
                                objmail.mannual_in_time = req.manual_in_time;
                                objmail.mannual_out_time = req.manual_out_time;
                                objmail.system_in_time = req.system_in_time;
                                objmail.system_out_time = req.system_out_time;
                                objmail.approver_remarks = objmodel.approval1_remarks;
                                objmail.a_e_id = req.a1_e_id > 0 ? req.a1_e_id : req.a2_e_id > 0 ? req.a2_e_id : req.a3_e_id > 0 ? req.a3_e_id : req.admin_id;
                                app_mail_lst.Add(objmail);
                            }


                            _context.tbl_outdoor_request.UpdateRange(req);

                        }
                        _context.SaveChanges();
                        trans.Commit();

                        if (app_mail_lst.Count > 0)
                        {
                            MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                            Task task = Task.Run(() => obj_ms.OutdoorApplicationApprovalMail(app_mail_lst));

                            task.Wait();
                        }

                        objResult.StatusCode = 0;
                        objResult.Message = "Submited Successfully...!";
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
            catch (Exception ex)
            {

                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        //Save AttandenceLeaveForApproval
        [Route("Save_AttandenceLeaveForApproval")]
        [HttpPost]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.AttendanceApproval))]
        public async Task<IActionResult> Save_AttandenceLeaveForApproval([FromBody] LeaveAppModel objmodel)
        {
            Response_Msg objResult = new Response_Msg();

            var all_request = _context.tbl_attendace_request.Where(x => objmodel.leave_request_id.Contains(x.leave_request_id) && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0) && x.is_deleted == 0 && x.is_final_approve != 1 && x.is_final_approve != 2).ToList();
            if (all_request.Count == 0)
            {
                objResult.StatusCode = 1;
                objResult.Message = "Invalid Requests or Unauthorize Company Access...!!";
                return Ok(objResult);
            }


            if (Convert.ToBoolean(FunIsApplicationFreezed()) == true)
            {
                objResult.StatusCode = 1;
                objResult.Message = "Attendence Application Approval has been freezed for this month";
                return Ok(objResult);
            }

            List<int?> EmpReqIds = all_request.Select(p => p.r_e_id).ToList();
            foreach (var IDs in EmpReqIds)
            {
                if (!_clsCurrentUser.DownlineEmpId.Contains(IDs ?? 0))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access...!!";
                    return Ok(objResult);
                }

            }

            string remarksRegex = @"^[ A-Za-z0-9_.-;,']*$";

            Regex rename = new Regex(remarksRegex);

            if (!string.IsNullOrEmpty(objmodel.approval1_remarks))
            {
                if (!rename.IsMatch(objmodel.approval1_remarks))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid remarks, special characters are not allowed in remarks";
                    return Ok(objResult);
                }
            }


            List<ApplicatonMailDetails> objmail_lst = new List<ApplicatonMailDetails>();

            try
            {

                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {

                        for (int i = 0; i < objmodel.leave_request_id.Count; i++)
                        {
                            //check req exist or not
                            var req = all_request.Where(x => x.leave_request_id == Convert.ToInt32(objmodel.leave_request_id[i])).FirstOrDefault();

                            if (req == null)
                            {
                                objResult.StatusCode = 1;
                                objResult.Message = "Selected leave application already approve or reject please check report !!";
                                return Ok(objResult);
                            }

                            if (objmodel.is_approved1 != 2)
                            {
                                var tbl_daily_attendance = _context.tbl_daily_attendance.Where(x => x.emp_id == req.r_e_id && x.attendance_dt == req.from_date && x.is_freezed == 1).FirstOrDefault();
                                if (tbl_daily_attendance != null)
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "Attendance of selected period is freezed";
                                    return Ok(objResult);
                                }
                            }


                            if (_clsCurrentUser.Is_Hod == 1)
                            {
                                var tbl_emp_m = _clsEmployeeDetail.Get_Emp_manager_dtl(req.r_e_id ?? 0);
                                if (tbl_emp_m != null)
                                {
                                    int final_mgr_id = 0;
                                    int final_approvel = tbl_emp_m[0].final_approval;

                                    if (tbl_emp_m[0].m_one_id == objmodel.a1_e_id)
                                    {
                                        req.is_approved1 = objmodel.is_approved1;
                                        req.approval1_remarks = objmodel.approval1_remarks;
                                        req.a1_e_id = objmodel.a1_e_id;
                                        req.approval_date1 = DateTime.Now;
                                    }
                                    else if (tbl_emp_m[0].m_two_id == objmodel.a1_e_id)
                                    {
                                        req.is_approved2 = objmodel.is_approved1;
                                        req.approval2_remarks = objmodel.approval1_remarks;
                                        req.a2_e_id = objmodel.a1_e_id;
                                        req.approval_date2 = DateTime.Now;
                                    }
                                    else if (tbl_emp_m[0].m_three_id == objmodel.a1_e_id)
                                    {
                                        req.is_approved3 = objmodel.is_approved1;
                                        req.approval3_remarks = objmodel.approval1_remarks;
                                        req.a3_e_id = objmodel.a1_e_id;
                                        req.approval_date3 = DateTime.Now;
                                    }

                                    final_mgr_id = final_approvel == 1 ? tbl_emp_m[0].m_one_id : final_approvel == 2 ? tbl_emp_m[0].m_two_id : final_approvel == 3 ? tbl_emp_m[0].m_three_id : 0;

                                    if (final_mgr_id == objmodel.a1_e_id)
                                    {
                                        req.is_final_approve = objmodel.is_approved1;
                                    }
                                }
                                else
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "You cannot approve selected leave application";
                                    return Ok(objResult);
                                }
                            }
                            else if (_clsCurrentUser.Is_Hod == 2)
                            {
                                req.is_final_approve = objmodel.is_approved1;
                                req.is_admin_approve = objmodel.is_approved1;
                                req.admin_id = objmodel.a1_e_id;
                                req.admin_remarks = objmodel.approval1_remarks;
                                req.admin_ar_date = DateTime.Now;
                            }
                            else
                            {
                                objResult.StatusCode = 1;
                                objResult.Message = "You are not an approver of selected application";
                                return Ok(objResult);
                            }


                            if (req.is_final_approve == 1 || req.is_final_approve == 2)
                            {
                                ApplicatonMailDetails objmail_dtl = new ApplicatonMailDetails();
                                objmail_dtl.req_id = req.leave_request_id;
                                objmail_dtl.emp_id = req.r_e_id ?? 0;
                                objmail_dtl.from_date = req.from_date;
                                objmail_dtl.mannual_in_time = req.manual_in_time;
                                objmail_dtl.mannual_out_time = req.manual_out_time;
                                objmail_dtl.approver_remarks = objmodel.approval1_remarks;
                                objmail_dtl.is_approve = req.is_final_approve;
                                objmail_dtl.a_e_id = req.a1_e_id > 0 ? req.a1_e_id : req.a2_e_id > 0 ? req.a2_e_id : req.a3_e_id > 0 ? req.a3_e_id : req.admin_id;
                                objmail_lst.Add(objmail_dtl);
                            }

                            _context.tbl_attendace_request.UpdateRange(req);

                        }
                        _context.SaveChanges();
                        trans.Commit();

                        if (objmail_lst.Count > 0)
                        {
                            MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                            Task task = Task.Run(() => obj_ms.AttendanceApplicationApprovalMail(objmail_lst));

                            task.Wait();
                        }
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
        //Save AttandenceLeaveForApproval
        [Route("Save_CompOffLeaveForApproval")]
        [HttpPost]
        ////[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffApproval))]
        public async Task<IActionResult> Save_CompOffLeaveForApproval([FromBody] LeaveAppModel objmodel)
        {
            Response_Msg objResult = new Response_Msg();


            if (Convert.ToBoolean(FunIsApplicationFreezed()) == true)
            {
                objResult.StatusCode = 1;
                objResult.Message = "Comp Off Application Approval has been freezed for this month";
                return Ok(objResult);
            }

            string remarksRegex = @"^[ A-Za-z0-9_.-;,']*$";

            Regex rename = new Regex(remarksRegex);

            if (!string.IsNullOrEmpty(objmodel.approval1_remarks))
            {
                if (!rename.IsMatch(objmodel.approval1_remarks))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid remarks, special characters are not allowed in remarks";
                    return Ok(objResult);
                }
            }

            var ReqEmpIDs = _context.tbl_comp_off_request_master.Where(p => p.is_deleted == 0 && objmodel.leave_request_id.Contains(p.comp_off_request_id)).ToList();

            List<int?> req_empids = ReqEmpIDs.Select(p => p.r_e_id).ToList();

            foreach (var ReqEmpID in req_empids)
            {
                if (!_clsCurrentUser.DownlineEmpId.Any(p => p == ReqEmpID))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access...!";
                    return Ok(objResult);
                }
            }


            if (ReqEmpIDs.Any(x => x.is_final_approve == 1 || x.is_final_approve == 2))
            {
                objResult.StatusCode = 1;
                objResult.Message = "one of the selected request already approved or reject, please check report for further details";

                return Ok(objResult);
            }



            List<ApplicatonMailDetails> objmail_lst = new List<ApplicatonMailDetails>();

            try
            {

                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {

                        for (int i = 0; i < objmodel.leave_request_id.Count; i++)
                        {
                            //check req exist or not
                            var req = ReqEmpIDs.Where(x => x.comp_off_request_id == Convert.ToInt32(objmodel.leave_request_id[i])).FirstOrDefault();

                            if (req == null)
                            {
                                objResult.StatusCode = 1;
                                objResult.Message = "Invalid request id found , please try after some time !!";
                                return Ok(objResult);
                            }
                            if (objmodel.is_approved1 != 2)
                            {
                                var tbl_daily_attendance = _context.tbl_daily_attendance.Where(x => x.emp_id == req.r_e_id && x.attendance_dt.Date == req.compoff_date.Date && x.is_freezed == 1).FirstOrDefault();
                                if (tbl_daily_attendance != null)
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "Attendance of selected period is freezed";
                                    return Ok(objResult);
                                }
                            }

                            if (_clsCurrentUser.Is_Hod == 1)
                            {
                                var tbl_emp_m = _context.tbl_emp_manager.Where(x => x.employee_id == req.r_e_id && x.is_deleted == 0).OrderByDescending(a => a.emp_mgr_id).FirstOrDefault();
                                if (tbl_emp_m != null)
                                {
                                    int final_mgr_id = 0;
                                    int final_approvel = tbl_emp_m.final_approval;

                                    if (tbl_emp_m.m_one_id == objmodel.a1_e_id)
                                    {
                                        req.is_approved1 = objmodel.is_approved1;
                                        req.approval1_remarks = objmodel.approval1_remarks;
                                        req.a1_e_id = objmodel.a1_e_id;
                                        req.approval_date1 = DateTime.Now;
                                    }
                                    else if (tbl_emp_m.m_two_id == objmodel.a1_e_id)
                                    {
                                        req.is_approved2 = objmodel.is_approved1;
                                        req.approval2_remarks = objmodel.approval1_remarks;
                                        req.a2_e_id = objmodel.a1_e_id;
                                        req.approval_date2 = DateTime.Now;
                                    }
                                    else if (tbl_emp_m.m_three_id == objmodel.a1_e_id)
                                    {
                                        req.is_approved3 = objmodel.is_approved1;
                                        req.approval3_remarks = objmodel.approval1_remarks;
                                        req.a3_e_id = objmodel.a1_e_id;
                                        req.approval_date3 = DateTime.Now;
                                    }

                                    final_mgr_id = (final_approvel == 1 ? tbl_emp_m.m_one_id : final_approvel == 2 ? tbl_emp_m.m_two_id : final_approvel == 3 ? tbl_emp_m.m_three_id : 0) ?? 0;

                                    if (final_mgr_id == objmodel.a1_e_id)
                                    {
                                        req.is_final_approve = objmodel.is_approved1;
                                    }
                                }
                                else
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "Create Manager to approve or reject request";
                                    return Ok(objResult);
                                }
                            }
                            else if (_clsCurrentUser.Is_Hod == 2)
                            {
                                req.is_final_approve = objmodel.is_approved1;
                                req.is_admin_approve = objmodel.is_approved1;
                                req.admin_id = objmodel.a1_e_id;
                                req.admin_remarks = objmodel.approval1_remarks;
                                req.admin_ar_date = DateTime.Now;
                            }
                            else
                            {
                                objResult.StatusCode = 1;
                                objResult.Message = "You cannot approve or reject any compoff request";
                                return Ok(objResult);
                            }

                            //Start Debit from Comp Off Ledger Account

                            if (req.is_final_approve == 1)
                            {
                                //     var test = _context.tbl_comp_off_ledger.Where(x => x.e_id == exist.r_e_id && x.compoff_date.Date == exist.compoff_against_date.Date).GroupBy(q=>q.e_id).Select(p=>new { tt= p.Sum(m=>m.dredit)}).FirstOrDefault();

                                //     var leaveledger = _context.tbl_comp_off_ledger.Where(x => x.e_id == exist.r_e_id && x.compoff_date.Date == exist.compoff_against_date.Date)
                                //.GroupBy(p => p.e_id).Select(q => new
                                //{
                                //    totalcredit = q.Sum(r => r.credit),
                                //    totaldebit = q.Sum(r => r.dredit),
                                //    leavebalance = q.Sum(x => x.credit) - q.Sum(x => x.dredit)
                                //}).FirstOrDefault();


                                var exist_comp_off = _context.tbl_comp_off_ledger.Where(x => x.e_id == req.r_e_id && x.is_deleted == 0 && x.compoff_date.Date == req.compoff_against_date.Date).ToList()
                                    .GroupBy(p => p.e_id).Select(q => new
                                    {
                                        totalcredit = q.Sum(r => r.credit),
                                        totaldebit = q.Sum(r => r.dredit),
                                        leavebalance = q.Sum(x => x.credit) - q.Sum(x => x.dredit)
                                    }).FirstOrDefault();

                                if (exist_comp_off == null || exist_comp_off.leavebalance <= 0)
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "Compoff not available in your account, please check and try !!";
                                    return Ok(objResult);
                                }


                                if (req.compoff_request_qty > exist_comp_off.leavebalance)
                                {
                                    objResult.StatusCode = 1;
                                    objResult.Message = "" + req.compoff_request_qty + " leaves not available in selected leave type account, please check and try !!";
                                    return Ok(objResult);
                                }


                                tbl_comp_off_ledger objcomledger = new tbl_comp_off_ledger();

                                objcomledger.compoff_date = req.compoff_against_date.Date;
                                objcomledger.credit = 0;
                                objcomledger.dredit = req.compoff_request_qty;
                                objcomledger.transaction_date = DateTime.Now;
                                objcomledger.transaction_type = 2;
                                objcomledger.monthyear = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
                                objcomledger.transaction_no = Convert.ToInt32("-" + req.compoff_against_date.ToString("yyyyMMdd"));
                                objcomledger.remarks = objmodel.approval1_remarks;
                                objcomledger.e_id = req.r_e_id;

                                _context.tbl_comp_off_ledger.AddRange(objcomledger);
                                // _context.Entry(objcomledger).State = EntityState.Added;
                                // _context.SaveChanges();


                                // var exist_comp_off = _context.tbl_comp_off_ledger.FirstOrDefault(x => x.e_id == exist.r_e_id && x.compoff_date.Date == exist.compoff_against_date.Date);

                                //exist_comp_off.dredit = 1;
                                //exist_comp_off.credit = 0;
                                //exist_comp_off.remarks = objoutdoor_request.approval1_remarks;

                                //_context.Entry(exist_comp_off).State = EntityState.Modified;
                                // _context.SaveChanges();

                            }

                            //End Debit from Comp Off Ledger Account    

                            if (req.is_final_approve == 1 || req.is_final_approve == 2)
                            {
                                ApplicatonMailDetails maildtl = new ApplicatonMailDetails();
                                maildtl.req_id = req.comp_off_request_id;
                                maildtl.emp_id = req.r_e_id ?? 0;
                                maildtl.from_date = req.compoff_against_date;
                                maildtl.to_date = req.compoff_date;
                                maildtl.approver_remarks = objmodel.approval1_remarks;
                                maildtl.is_approve = req.is_final_approve;
                                maildtl.a_e_id = req.a1_e_id > 0 ? req.a1_e_id : req.a2_e_id > 0 ? req.a2_e_id : req.a3_e_id > 0 ? req.a3_e_id : req.admin_id;

                                objmail_lst.Add(maildtl);
                            }

                            _context.tbl_comp_off_request_master.UpdateRange(req);
                        }
                        _context.SaveChanges();
                        trans.Commit();

                        if (objmail_lst.Count > 0)
                        {
                            MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                            Task task = Task.Run(() => obj_ms.CompoffApplicationApprovalMail(objmail_lst));

                            task.Wait();
                        }
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



#region MANNUAL LEAVE TYPE ,STARTED BY SUPRIYA ON 21-06-2019

        [Route("Get_LeaveInfo_for_LeaveType")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.ManualLeave))]
        public IActionResult Get_LeaveInfo_for_LeaveType()
        {
            throw new NotImplementedException();
#if false
            try
            {
                //var only_date = transactiondate.ToString("yyyy-MM-dd 00:00:00"); 

                clsLeaveCredit objcls = new clsLeaveCredit(_context, _AC, _config, _clsCurrentUser);

                List<tbl_leave_type> objlist = objcls.Get_Leave_typesByStarting_Year_Date();

                return Ok(objlist);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
#endif
        }

        [Route("Get_LeaveInfo_byleave_typeid/{leave_type_id}")]
        [HttpPost]
        //[Authorize(Policy = "5019")]
        public IActionResult Get_LeaveInfo_byleave_typeid([FromRoute] int leave_type_id)
        {
            try
            {
                var data = _context.tbl_leave_info.OrderByDescending(a => a.leave_info_id).Where(a => a.leave_type_id == leave_type_id).ToList();
                var leave_info_idd = 0;
                if (data.Count > 0)
                {
                    leave_info_idd = data[0].leave_info_id;
                }
                return Ok(new { leave_info_idd = leave_info_idd });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }

        [Route("Save_Leave_Ledger")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveLedeger))]
        public IActionResult Save_Leave_Ledger([FromBody] LeaveLedgerModell objtbl)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(objtbl.company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }

            if (!_clsCurrentUser.DownlineEmpId.Contains(objtbl.e_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access..!!";
                return Ok(objresponse);
            }
            try
            {

                var data = _context.tbl_leave_ledger.Where(a => a.transaction_date == DateTime.Now.Date && a.e_id == objtbl.e_id && a.transaction_type == objtbl.transaction_type && a.leave_type_id == objtbl.leave_type_id).ToList();
                if (data.Count > 0)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Transaction Already Exist of Selected Employee";
                }
                else
                {
                    List<LeaveLedgerModell> objleave = new List<LeaveLedgerModell>();
                    objleave.Add(new LeaveLedgerModell
                    {
                        company_id = objtbl.company_id,
                        leave_type_id = objtbl.leave_type_id,
                        transaction_type = objtbl.transaction_type,
                        credit = objtbl.credit,
                        dredit = objtbl.dredit,
                        remarks = objtbl.remarks,
                        e_id = objtbl.e_id,
                        created_by = _clsCurrentUser.EmpId
                    });

                    clsLeaveCredit objcls = new clsLeaveCredit(_context, objtbl.company_id);

                    //var result = objcls.Save_Mannual_Leave(objleave);
                    //if (result == 1)
                    //{
                    //    objresponse.StatusCode = 0;
                    //    objresponse.Message = "Transaction Successfully Saved";
                    //}
                    //else
                    //{
                    //    objresponse.StatusCode = 1;
                    //    objresponse.Message = "Something went wrong, please try after sometime...!!";
                    //}

                }

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("GetLeaveLedger")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.ManualLeave))]
        public IActionResult GetLeaveLedger() // Get all Leaves
        {
            throw new NotImplementedException();
#if false
            try
            {

                //var data = _context.tbl_leave_ledger.GroupBy(l => new { l.e_id, l.leave_type_id }).Select(q => new
                //                    {

                //                        leave_type_id = q.FirstOrDefault().leave_type_id,
                //                        leave_type_name = q.FirstOrDefault().tbl_leave_type.leave_type_name,
                //                        leave_info_id = q.FirstOrDefault().tbl_leave_info.leave_info_id,
                //                        totalcredit = q.Sum(r => r.credit),
                //                        totaldebit = q.Sum(r => r.dredit),
                //                        leavebalance = q.Sum(x => x.credit) - q.Sum(x => x.dredit),
                //                        emp_id = q.FirstOrDefault().e_id,
                //                        total_leave_type = q.Count(),
                //                    }).ToList();

                clsLeaveCredit objleavecls = new clsLeaveCredit(_context, _AC, _config, _clsCurrentUser);

                List<LeaveLedgerModell> emp_leave_list = objleavecls.Get_Leave_Ledger();


                //for (int i = 0; i < data.Count; i++)
                //{

                //    var emp_dataa = _context.tbl_emp_officaial_sec.Where(x => x.employee_id == data[i].emp_id && !string.IsNullOrEmpty(x.employee_first_name)).Select(p => new {
                //        p.employee_first_name,
                //        p.employee_middle_name,
                //        p.employee_last_name,
                //        p.tbl_employee_id_details.emp_code,
                //        p.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(y => y.is_deleted == 0).company_id
                //    }).FirstOrDefault();


                //    LeaveLedgerModell objmodel = new LeaveLedgerModell();
                //    objmodel.emp_name_code = emp_dataa != null ? string.Format("{0} {1} {2} ({3})", emp_dataa.employee_first_name, emp_dataa.employee_middle_name, emp_dataa.employee_last_name, emp_dataa.emp_code) : "";
                //    objmodel.credit = data[i].totalcredit;
                //    objmodel.dredit = data[i].totaldebit;
                //    objmodel.leave_type_id = data[i].leave_type_id ?? 0;
                //    objmodel.leave_type_name = data[i].leave_type_name;
                //    objmodel.balance = data[i].leavebalance;
                //    objmodel.company_id = emp_dataa != null ? emp_dataa.company_id ?? 0 : 0;
                //    objmodel.e_id = data[i].emp_id ?? 0;


                //    emp_leave_list.Add(objmodel);

                //}


                return Ok(emp_leave_list);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
#endif
        }

        [Route("GetLeaveBalances")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveBalance))]
        public IActionResult GetLeaveBalances([FromBody] LeaveReport objmodel)
        {
            ResponseMsg objResult = new ResponseMsg();

            try
            {
                objmodel.empdtl.RemoveAll(p => Convert.ToInt32(p) < 0 || Convert.ToInt32(p) == 0);

                //foreach (var ids in objmodel.empdtl)
                //{

                //    if (!_clsCurrentUser.DownlineEmpId.Contains(Convert.ToInt32(ids)))
                //    {
                //        objResult.StatusCode = 1;
                //        objResult.Message = "Unauthorize Access....!!";
                //        return Ok(objResult);
                //    }

                //    //int id = Convert.ToInt32(ids);
                //}
                objmodel.to_date = DateTime.Parse(objmodel.to_date).AddDays(1).ToString();


                var Masterdata = (from t1 in _context.tbl_leave_type.Where(a => a.is_active == 1 && a.leave_type_name != "LWP").Select(p => new { p.leave_type_id, p.leave_type_name })
                                  from t2 in _context.tbl_emp_officaial_sec.Where(p => objmodel.empdtl.Contains(p.employee_id ?? 0) && p.is_deleted == 0)
                                  .Select(p => new { EmpId = p.employee_id.Value, p.tbl_employee_id_details.emp_code, p.employee_first_name, p.employee_middle_name, p.employee_last_name })
                                  select new { leaveTypeId = t1.leave_type_id, t1.leave_type_name, t2.EmpId, t2.emp_code, t2.employee_first_name, t2.employee_middle_name, t2.employee_last_name }).ToList();
                var CurrentMonthData = (_context.tbl_leave_ledger.Where(p => p.entry_date >= DateTime.Parse(objmodel.from_date) && p.entry_date < DateTime.Parse(objmodel.to_date) && objmodel.empdtl.Contains(p.e_id ?? 0))
                           .GroupBy(g => new { g.e_id, g.leave_type_id }, (k, d) => new { EmpId = k.e_id, leaveTypeId = k.leave_type_id, Credit = d.Sum(a => a.credit), Debit = d.Sum(a => a.dredit) }).
                           Select(p => new { EmpId = p.EmpId ?? 0, leaveTypeId = p.leaveTypeId ?? 0, p.Credit, p.Debit })).ToList();
                var OpeningData = (_context.tbl_leave_ledger.Where(p => p.entry_date < DateTime.Parse(objmodel.from_date) && objmodel.empdtl.Contains(p.e_id ?? 0))
                    .GroupBy(g => new { g.e_id, g.leave_type_id }, (k, d) => new { EmpId = k.e_id, leaveTypeId = k.leave_type_id, credit = d.Sum(a => a.credit), dredit = d.Sum(a => a.dredit) })
                    .Select(p => new
                    {
                        EmpId = p.EmpId ?? 0,
                        leaveTypeId = p.leaveTypeId ?? 0,
                        OpeningBalance = p.credit - p.dredit
                    })).ToList();
                var Data = (from t1 in Masterdata
                            join t2 in OpeningData on new { t1.leaveTypeId, t1.EmpId } equals new { t2.leaveTypeId, t2.EmpId } into t1t2
                            from _t1t2 in t1t2.DefaultIfEmpty()
                            join t3 in CurrentMonthData on new { t1.leaveTypeId, t1.EmpId } equals new { t3.leaveTypeId, t3.EmpId } into t1t3
                            from _t1t3 in t1t3.DefaultIfEmpty()
                            select new LeaveLedgerModellOpeningBalence
                            {
                                e_id = t1.EmpId,
                                emp_name = $"{t1.employee_first_name} {t1.employee_middle_name ?? string.Empty} {t1.employee_last_name ?? string.Empty}",
                                emp_code = t1.emp_code,
                                leave_type_id = t1.leaveTypeId,
                                leave_type_name = t1.leave_type_name,
                                OpeningBalance = (_t1t2 == null ? 0 : _t1t2.OpeningBalance),
                                credit = (_t1t3 == null ? 0 : _t1t3.Credit),
                                dredit = (_t1t3 == null ? 0 : _t1t3.Debit),
                                balance = (_t1t2 == null ? 0 : _t1t2.OpeningBalance) + (_t1t3 == null ? 0 : _t1t3.Credit) - (_t1t3 == null ? 0 : _t1t3.Debit)
                            }).ToList();
                return Ok(Data);





                //List < LeaveLedgerModell> list = new List<LeaveLedgerModell>();
                //if (totalleaves.Count > 0)
                //{


                //    foreach (var item in totalleaves)
                //    {
                //        var leaveledger = (from a in _context.tbl_leave_ledger
                //                           where a.leave_type_id == item.leave_type_id && objmodel.empdtl.Contains(a.e_id ?? 0)                                           
                //                           group a by new { a.e_id, a.tbl_leave_type.leave_type_name, a.leave_type_id } into b
                //                           select new
                //                           {
                //                               e_id = b.Key.e_id,
                //                               leave_type_id = b.Key.leave_type_id,
                //                               leave_type_name = b.Key.leave_type_name,
                //                               totalcredit = b.Sum(x => x.credit),
                //                               totaldebit = b.Sum(y => y.dredit),
                //                               leavebalance = b.Sum(x => x.credit) - b.Sum(y => y.dredit)
                //                           }).ToList();

                //        if (leaveledger.Count > 0)
                //        {
                //            foreach (var item1 in leaveledger)
                //            {
                //                LeaveLedgerModell objledger = new LeaveLedgerModell();

                //                var emp_data = _context.tbl_employee_master.Where(x => x.employee_id == item1.e_id && x.is_active == 1).Select(p => new
                //                {

                //                    emp_name_code = string.Format("{0} {1} {2} ({3})",
                //                      p.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                //                      p.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                //                      p.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                //                      p.emp_code)

                //                }).FirstOrDefault();



                //                objledger.e_id = Convert.ToInt32(item1.e_id);
                //                objledger.emp_name_code = emp_data.emp_name_code;
                //                objledger.leave_type_id = Convert.ToInt32(item1.leave_type_id);
                //                objledger.leave_type_name = item1.leave_type_name;
                //                objledger.credit = item1.totalcredit;
                //                objledger.dredit = item1.totaldebit;
                //                objledger.balance = item1.leavebalance;

                //                list.Add(objledger);
                //            }
                //        }
                //    }
                //}
                // return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }
#endregion MANNUAL LEAVE TYPE, END BY SUPRIYA ON 22-06-2019



#region ** STARTED BY SUPRIYA ON 28-06-2019

        [Route("GetLeaveApplicationForApproval/{loginempid}/{year}")]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveApproval))]
        [HttpGet]
        public IActionResult GetLeaveApplicationForApproval([FromRoute] int loginempid, int year)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {
                if (!_clsCurrentUser.DownlineEmpId.Any(p => p == loginempid))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access...!";
                    return Ok(objResult);
                }

                List<int> emplst = new List<int>();

                if (!_clsCurrentUser.RoleId.Contains((int)enmRoleMaster.SuperAdmin))
                {
                    List<int> loginid = new List<int>();
                    loginid.Add(_clsCurrentUser.EmpId);
                    emplst = _clsCurrentUser.DownlineEmpId.Except(loginid).ToList();
                }
                else
                {
                    emplst = _clsCurrentUser.DownlineEmpId.ToList();
                }

                if (_clsCurrentUser.Is_Hod == 1)
                {
                    var result = _context.tbl_leave_request.Where(x => x.requester_date.Year == year && x.is_deleted == 0 && x.is_final_approve != 1 && x.is_final_approve != 2 && emplst.Contains(x.r_e_id ?? 0) && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).Select(p => new
                    {

                        employee_id = p.r_e_id,
                        leave_request_id = p.leave_request_id,
                        employee_code = p.tbl_employee_requester.emp_code,
                        employee_name = string.Format("{0} {1} {2}", p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(g => g.is_deleted == 0).employee_first_name,
                                                                 p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(g => g.is_deleted == 0).employee_middle_name,
                                                                 p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(g => g.is_deleted == 0).employee_last_name),
                        from_date = p.from_date,
                        to_date = p.to_date,
                        leave_type = p.tbl_leave_type.leave_type_name,
                        //leave_applicable_for = p.leave_applicable_for,// 1 for Full day , 2 for Half day , 3 for hours and minutes (HH:MM)
                        leave_applicable_for = p.leave_applicable_for == 1 ? "Full Day" : p.leave_applicable_for == 2 ? "Half Day" : p.leave_applicable_for == 3 ? "HH:MM" : "",
                        leave_qty = p.leave_qty,
                        requester_date = p.requester_date,
                        requester_remarks = p.requester_remarks,
                        status = p.is_final_approve == 0 ? "Pending" : p.is_final_approve == 1 ? "Approve" : p.is_final_approve == 2 ? "Reject" : p.is_final_approve == 3 ? "In Process" : "",
                        approver_no = (p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_one_id == _clsCurrentUser.EmpId ? 1 :
                                                               p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_two_id == _clsCurrentUser.EmpId ? 2 :
                                                               p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_three_id == _clsCurrentUser.EmpId ? 3 : 0),
                        approver_remarks = (p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_one_id == _clsCurrentUser.EmpId ? p.approval1_remarks :
                                                               p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_two_id == _clsCurrentUser.EmpId ? p.approval2_remarks :
                                                               p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_three_id == _clsCurrentUser.EmpId ? p.approval3_remarks : ""),
                        my_status = (p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_one_id == _clsCurrentUser.EmpId ? (p.is_approved1 == 0 ? "Pending" : p.is_approved1 == 1 ? "Approved" : p.is_approved1 == 2 ? "Rejected" : "") :
                                                               p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_two_id == _clsCurrentUser.EmpId ? (p.is_approved2 == 0 ? "Pending" : p.is_approved2 == 1 ? "Approved" : p.is_approved2 == 2 ? "Rejected" : "") :
                                                               p.tbl_employee_requester.tbl_emp_manager.FirstOrDefault(q => q.is_deleted == 0).m_three_id == _clsCurrentUser.EmpId ? (p.is_approved3 == 0 ? "Pending" : p.is_approved3 == 1 ? "Approved" : p.is_approved3 == 2 ? "Rejected" : "") : ""),

                    }).OrderByDescending(y => y.leave_request_id).ToList();

                    return Ok(result);

                }
                else if (_clsCurrentUser.Is_Hod == 2)
                {
                    var result = _context.tbl_leave_request.Where(y => y.requester_date.Year == year && emplst.Contains(y.r_e_id ?? 0) && _clsCurrentUser.CompanyId.Contains(y.company_id ?? 0)).Where(x => x.is_deleted == 0 && x.is_final_approve != 1 && x.is_final_approve != 2).Select(p => new
                    {
                        employee_id = p.r_e_id,
                        leave_request_id = p.leave_request_id,
                        employee_code = p.tbl_employee_requester.emp_code,
                        employee_name = string.Format("{0} {1} {2}", p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(g => g.is_deleted == 0).employee_first_name,
                                                                 p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(g => g.is_deleted == 0).employee_middle_name,
                                                                 p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(g => g.is_deleted == 0).employee_last_name),
                        from_date = p.from_date,
                        to_date = p.to_date,
                        leave_type = p.tbl_leave_type.leave_type_name,
                        //leave_applicable_for = p.leave_applicable_for,
                        leave_applicable_for = p.leave_applicable_for == 1 ? "Full Day" : p.leave_applicable_for == 2 ? "Half Day" : p.leave_applicable_for == 3 ? "HH:MM" : "",
                        leave_qty = p.leave_qty,
                        requester_date = p.requester_date,
                        requester_remarks = p.requester_remarks,
                        status = p.is_final_approve == 0 ? "Pending" : p.is_final_approve == 1 ? "Approved" : p.is_final_approve == 2 ? "Rejected" : "",
                        approver_remarks = p.admin_remarks,
                        my_status = p.is_admin_approve == 0 ? "Pending" : p.is_admin_approve == 1 ? "Approved" : p.is_admin_approve == 2 ? "Reject" : "",

                    }).OrderByDescending(y => y.leave_request_id).ToList();

                    return Ok(result);
                }
                else
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "You are not an approver of any Leave applciation";
                    return Ok(objResult);
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //[Route("Save_LeaveApplicationForApprover")]
        //[HttpPost]
        ////[Authorize(Policy = "5024")]
        //public IActionResult Save_LeaveApplicationForApprover([FromBody] tbl_leave_request objtbl)
        //{

        //    Response_Msg objresponse = new Response_Msg();

        //    Classes.clsLoginEmpDtl ob = new clsLoginEmpDtl(_context, Convert.ToInt32(objtbl.a1_e_id), "read", _AC);
        //    if (!ob.is_valid())
        //    {
        //        objresponse.StatusCode = 1;
        //        objresponse.Message = "Unauthorize Access...!";
        //        return Ok(objresponse);
        //    }


        //    using (var trans = _context.Database.BeginTransaction())
        //    {
        //        try
        //        {

        //            //double leave_noofday = 0;

        //            var exist = _context.tbl_leave_request.Where(a => a.leave_request_id == objtbl.leave_request_id && a.is_deleted == 0 && a.is_final_approve == 0).FirstOrDefault();


        //            if (exist != null)
        //            {
        //                //start check attandance already freezed or not

        //                var freeze_attandance = _context.tbl_daily_attendance.Where(x => x.emp_id == exist.r_e_id && x.is_freezed == 1 && (exist.from_date <= x.attendance_dt && x.attendance_dt <= exist.to_date)).ToList();

        //                if (freeze_attandance.Count > 0)
        //                {
        //                    objresponse.StatusCode = 1;
        //                    objresponse.Message = "Attendance of Selected Leave Period already freezed";
        //                    return Ok(objresponse);

        //                }
        //                //end check attandance already freezed or not
        //                var _get_frequency_type = _context.tbl_leave_credit.OrderBy(x => x.leave_credit_id).Where(p => p.leave_info_id == exist.leave_info_id && p.is_deleted == 0).FirstOrDefault();

        //                if (_get_frequency_type != null)
        //                {
        //                    //START Debit from Leave Balance (In Leave Ledger) if Leave is Approved
        //                    var leaveledger = _context.tbl_leave_ledger.Where(x => x.e_id == exist.r_e_id && x.leave_type_id == exist.leave_type_id && x.leave_addition_type == _get_frequency_type.frequency_type).ToList()
        //                         .GroupBy(p => p.e_id).Select(q => new
        //                         {
        //                             totalcredit = q.Sum(r => r.credit),
        //                             totaldebit = q.Sum(r => r.dredit),
        //                             leavebalance = q.Sum(x => x.credit) - q.Sum(x => x.dredit)
        //                         }).FirstOrDefault();

        //                    if (leaveledger == null || leaveledger.leavebalance <= 0)
        //                    {
        //                        objresponse.StatusCode = 1;
        //                        objresponse.Message = "Leave not available in your selected leave type account, please check and try !!";
        //                        return Ok(objresponse);
        //                    }


        //                    if (exist.leave_qty > leaveledger.leavebalance)
        //                    {
        //                        objresponse.StatusCode = 1;
        //                        objresponse.Message = "" + exist.leave_qty + " leaves not available in selected leave type account, please check and try !!";
        //                        return Ok(objresponse);
        //                    }


        //                    var emp_manager = _context.tbl_emp_manager.Where(a => a.employee_id == exist.r_e_id && a.is_deleted == 0).OrderByDescending(b => b.emp_mgr_id).FirstOrDefault();

        //                    int final_approver = emp_manager.final_approval;
        //                    if (final_approver == 1)
        //                    {
        //                        if (emp_manager.m_one_id == objtbl.a1_e_id)
        //                        {
        //                            //exist.is_final_approve = 1;
        //                            exist.is_final_approve = objtbl.is_final_approve;
        //                            exist.is_approved1 = objtbl.is_approved1;
        //                            exist.approval1_remarks = objtbl.approval1_remarks;
        //                            exist.a1_e_id = objtbl.a1_e_id;
        //                            exist.approval_date1 = DateTime.Now;
        //                        }
        //                        else if (emp_manager.m_two_id == objtbl.a1_e_id)
        //                        {
        //                            exist.is_final_approve = 0;
        //                            exist.is_approved2 = objtbl.is_approved1;
        //                            exist.approval2_remarks = objtbl.approval1_remarks;
        //                            exist.a2_e_id = objtbl.a1_e_id;
        //                            exist.approval_date2 = DateTime.Now;
        //                        }
        //                        else if (emp_manager.m_three_id == objtbl.a1_e_id)
        //                        {
        //                            exist.is_final_approve = 0;
        //                            exist.is_approved3 = objtbl.is_approved1;
        //                            exist.approval3_remarks = objtbl.approval1_remarks;
        //                            exist.a3_e_id = objtbl.a1_e_id;
        //                            exist.approval_date3 = DateTime.Now;
        //                        }
        //                    }
        //                    else if (final_approver == 2)
        //                    {
        //                        if (emp_manager.m_one_id == objtbl.a1_e_id)
        //                        {
        //                            exist.is_final_approve = 0;
        //                            exist.is_approved1 = objtbl.is_approved1;
        //                            exist.approval1_remarks = objtbl.approval1_remarks;
        //                            exist.a1_e_id = objtbl.a1_e_id;
        //                            exist.approval_date1 = DateTime.Now;
        //                        }
        //                        else if (emp_manager.m_two_id == objtbl.a1_e_id)
        //                        {
        //                            exist.is_final_approve = objtbl.is_final_approve;
        //                            exist.is_approved2 = objtbl.is_approved1;
        //                            exist.approval2_remarks = objtbl.approval1_remarks;
        //                            exist.a2_e_id = objtbl.a1_e_id;
        //                            exist.approval_date2 = DateTime.Now;
        //                        }
        //                        else if (emp_manager.m_three_id == objtbl.a1_e_id)
        //                        {
        //                            exist.is_final_approve = 0;
        //                            exist.is_approved3 = objtbl.is_approved1;
        //                            exist.approval3_remarks = objtbl.approval1_remarks;
        //                            exist.a3_e_id = objtbl.a1_e_id;
        //                            exist.approval_date3 = DateTime.Now;
        //                        }
        //                    }
        //                    else if (final_approver == 3)
        //                    {
        //                        if (emp_manager.m_one_id == objtbl.a1_e_id)
        //                        {
        //                            exist.is_final_approve = 0;
        //                            exist.is_approved1 = objtbl.is_approved1;
        //                            exist.approval1_remarks = objtbl.approval1_remarks;
        //                            exist.a1_e_id = objtbl.a1_e_id;
        //                            exist.approval_date1 = DateTime.Now;
        //                        }
        //                        else if (emp_manager.m_two_id == objtbl.a1_e_id)
        //                        {
        //                            exist.is_final_approve = 0;
        //                            exist.is_approved2 = objtbl.is_approved1;
        //                            exist.approval2_remarks = objtbl.approval1_remarks;
        //                            exist.a2_e_id = objtbl.a1_e_id;
        //                            exist.approval_date2 = DateTime.Now;
        //                        }
        //                        else if (emp_manager.m_three_id == objtbl.a1_e_id)
        //                        {
        //                            exist.is_final_approve = objtbl.is_final_approve;
        //                            exist.is_approved3 = objtbl.is_approved1;
        //                            exist.approval3_remarks = objtbl.approval1_remarks;
        //                            exist.a3_e_id = objtbl.a1_e_id;
        //                            exist.approval_date3 = DateTime.Now;
        //                        }
        //                    }


        //                    // exist.approval_date1 = DateTime.Now;

        //                    _context.Entry(exist).State = EntityState.Modified;
        //                    _context.SaveChanges();



        //                    //start by supriya debit in leave ledger
        //                    if (exist.is_final_approve == 1)
        //                    {
        //                        tbl_leave_ledger objtbl_leave_ledger = new tbl_leave_ledger();

        //                        objtbl_leave_ledger.leave_type_id = exist.leave_type_id;
        //                        objtbl_leave_ledger.leave_info_id = exist.leave_info_id;
        //                        objtbl_leave_ledger.transaction_date = exist.from_date;
        //                        objtbl_leave_ledger.entry_date = DateTime.Now;
        //                        objtbl_leave_ledger.transaction_type = 2; // leave consume
        //                        objtbl_leave_ledger.monthyear = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
        //                        objtbl_leave_ledger.transaction_no = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
        //                        objtbl_leave_ledger.leave_addition_type = _get_frequency_type.frequency_type;
        //                        objtbl_leave_ledger.credit = 0;
        //                        objtbl_leave_ledger.dredit = exist.leave_qty;
        //                        objtbl_leave_ledger.remarks = objtbl.approval1_remarks;
        //                        objtbl_leave_ledger.e_id = exist.r_e_id;

        //                        _context.Entry(objtbl_leave_ledger).State = EntityState.Added;
        //                        _context.SaveChanges();
        //                    }
        //                    // end by supriya debit in leave ledger



        //                    trans.Commit();



        //                    objresponse.StatusCode = 0;
        //                    objresponse.Message = "Submited Successfully...!";


        //                    //MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString);

        //                    //Task task = Task.Run(() => obj_ms.LeaveApplicationApprovalMail(Convert.ToInt32(exist.r_e_id), exist.from_date.ToString(), exist.to_date.ToString(), objtbl.is_approved1, objtbl.approval1_remarks));

        //                    return Ok(objresponse);
        //                }
        //                else
        //                {
        //                    objresponse.StatusCode = 1;
        //                    objresponse.Message = "Please enter leave credit details under Leave Settings";
        //                    return Ok(objresponse);
        //                }
        //                //End Debit from Leave Balance (In Leave Ledger) if Leave is Approved

        //                //supriya

        //                //if (exist.leave_applicable_for == 1) // if full day
        //                //{
        //                //    leave_noofday =(exist.to_date- exist.from_date).Days+1;
        //                //}
        //                //else if (exist.leave_applicable_for == 2)// if half day
        //                //{
        //                //    leave_noofday = ((exist.to_date - exist.from_date).Days + 1) * 0.5;

        //                //}



        //                //supriya





        //            }
        //            else
        //            {
        //                objresponse.StatusCode = 1;
        //                objresponse.Message = "Invalid Leave Request / Leave Request already either approverd or rejected";
        //                return Ok(objresponse);
        //            }


        //        }
        //        catch (Exception ex)
        //        {
        //            return Ok(ex.Message);
        //        }

        //    }

        //}
#endregion ** END BY SUPRIYA ON 28-06-2019


#region ** STARTED BY SUPRIYA ON 24-07-2019

        [Route("GetLeaveBalancesByEmpID/{employee_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult GetLeaveBalancesByEmpID([FromRoute] int employee_id)
        {
            throw new NotImplementedException();
#if false

            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(employee_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize access...!!";
                return Ok(objResult);

            }


            try
            {
                var totalleaves = _context.tbl_leave_type.Where(a => a.is_active == 1).ToList();
                List<LeaveLedgerModell> list = new List<LeaveLedgerModell>();
                if (totalleaves.Count > 0)
                {


                    foreach (var item in totalleaves)
                    {
                        var leaveledger = (from a in _context.tbl_leave_ledger
                                           where a.e_id == employee_id && a.leave_type_id == item.leave_type_id
                                           group a by new { a.e_id, a.tbl_leave_type.leave_type_name, a.leave_type_id, a.tbl_leave_info.tbl_leave_type._is_el } into b
                                           select new
                                           {
                                               e_id = b.Key.e_id,
                                               leave_type_id = b.Key.leave_type_id,
                                               leave_type_name = b.Key.leave_type_name,
                                               _is_el = (b.Key._is_el == null || b.Key._is_el == 0) ? 0 : b.Key._is_el,
                                               totalcredit = b.Sum(x => x.credit),
                                               totaldebit = b.Sum(y => y.dredit),
                                               leavebalance = b.Sum(x => x.credit) - b.Sum(y => y.dredit)
                                           }).ToList();

                        if (leaveledger.Count > 0)
                        {
                            foreach (var item1 in leaveledger)
                            {
                                LeaveLedgerModell objledger = new LeaveLedgerModell();

                                var emp_data = _context.tbl_employee_master.Where(x => x.employee_id == item1.e_id && x.is_active == 1).Select(p => new
                                {

                                    emp_name_code = string.Format("{0} {1} {2} ({3})",
                                      p.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                                      p.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                                      p.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                                      p.emp_code),
                                    p.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).current_employee_type

                                }).FirstOrDefault();


                                if (item1._is_el == 1 && (emp_data.current_employee_type == 3 || emp_data.current_employee_type == 4 || emp_data.current_employee_type == 10))
                                {
                                    objledger.e_id = Convert.ToInt32(item1.e_id);
                                    objledger.emp_name_code = emp_data.emp_name_code;
                                    objledger.leave_type_id = Convert.ToInt32(item1.leave_type_id);
                                    objledger.leave_type_name = item1.leave_type_name;
                                    objledger.credit = item1.totalcredit;
                                    objledger.dredit = item1.totaldebit;
                                    objledger.balance = item1.leavebalance;
                                    objledger.current_emptype = Convert.ToString(emp_data.current_employee_type);
                                }
                                else if (item1._is_el == 0)
                                {
                                    objledger.e_id = Convert.ToInt32(item1.e_id);
                                    objledger.emp_name_code = emp_data.emp_name_code;
                                    objledger.leave_type_id = Convert.ToInt32(item1.leave_type_id);
                                    objledger.leave_type_name = item1.leave_type_name;
                                    objledger.credit = item1.totalcredit;
                                    objledger.dredit = item1.totaldebit;
                                    objledger.balance = item1.leavebalance;
                                    objledger.current_emptype = Convert.ToString(emp_data.current_employee_type);
                                }

                                if (objledger.e_id > 0)
                                {
                                    list.Add(objledger);
                                }


                            }
                        }
                    }
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
#endif
        }

        #endregion ** END BY SUPRIYA ON 24-07-2019



#if false
        [Route("Gettbl_leave_infoByCompID/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveSetting))]
        public async Task<IActionResult> Gettbl_leave_infoByCompID([FromRoute] int company_id)
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
                var result = _context.tbl_leave_info.Select(
                    p => new
                    {

                        leave_type_id = p.leave_type_id,
                        leave_info_id = p.leave_info_id,
                        leave_tenure_from_date = p.leave_tenure_from_date,
                        leave_tenure_to_date = p.leave_tenure_to_date,
                        leave_type = p.leave_type,
                        leave_type_name = p.tbl_leave_type.leave_type_name,
                        description = p.tbl_leave_type.description,
                        // p.tbl_leave_applicablity.tbl_leave_info,
                        Status = p.is_active
                    }).ToList();

                //var result = _context.tbl_leave_appcbl_on_company.Where(a => a.is_deleted == 0 && a.tbl_company_master.company_id == company_id).Select(p => new
                //{

                //    leave_type_id = p.tbl_leave_applicablity.tbl_leave_info.leave_type_id,
                //    leave_info_id = p.tbl_leave_applicablity.tbl_leave_info.leave_info_id,
                //    leave_tenure_from_date = p.tbl_leave_applicablity.tbl_leave_info.leave_tenure_from_date,
                //    leave_tenure_to_date = p.tbl_leave_applicablity.tbl_leave_info.leave_tenure_to_date,
                //    leave_type = p.tbl_leave_applicablity.tbl_leave_info.leave_type,
                //    leave_type_name = p.tbl_leave_applicablity.tbl_leave_info.tbl_leave_type.leave_type_name,
                //    description = p.tbl_leave_applicablity.tbl_leave_info.tbl_leave_type.description,
                //    // p.tbl_leave_applicablity.tbl_leave_info,
                //    Status = p.tbl_leave_applicablity.tbl_leave_info.is_active
                //}).ToList();

                return Ok(result);


            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

#endif


        #region CREATED BY AMARJEET ON 31-07-2019 FOR LEAVE APPROVAL

        [HttpGet("GetLeaveApplicationForAdmin/{company_id}")]
        //[Authorize]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveCancel))]
        public async Task<IActionResult> GetLeaveApplicationForAdmin([FromRoute] int company_id)
        {
            ResponseMsg objResult = new ResponseMsg();
            try
            {

                var leave_Req = _context.tbl_leave_request.Where(x => x.is_deleted == 0 && x.is_final_approve == 1 && _clsCurrentUser.DownlineEmpId.Contains(x.r_e_id ?? 0) && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).Select(p => new
                {

                    p.leave_request_id,
                    employee_name = string.Format("{0} {1} {2} ({3})",
                        p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                        p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                        p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name,
                        p.tbl_employee_requester.emp_code),
                    p.from_date,
                    p.to_date,
                    leave_type = p.tbl_leave_type.leave_type_name,
                    p.leave_applicable_for,
                    p.leave_qty,
                    p.requester_date,
                    p.requester_remarks,
                    p.deleted_remarks,
                    p.deleted_dt,
                    p.r_e_id,
                    fromdate_monthyeara = Convert.ToInt32(p.from_date.Year + p.from_date.Month.ToString().Length > 1 ? p.from_date.Month.ToString() : "0" + p.from_date.Month.ToString()),
                    todate_monthyr = p.to_date.Year + p.to_date.Month.ToString().Length > 1 ? p.to_date.Month.ToString() : "0" + p.to_date.Month.ToString(),

                }).OrderByDescending(z => z.from_date).ToList();




                var process_data = _context.tbl_payroll_process_status.Where(x => x.is_deleted == 0 && x.is_freezed == 1 && _clsCurrentUser.DownlineEmpId.Contains(x.emp_id ?? 0) && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).ToList();

                for (int i = leave_Req.Count() - 1; i >= 0; i--)
                {
                    int from_date = Convert.ToInt32(leave_Req[i].from_date.Year + Convert.ToString(leave_Req[i].from_date.Month.ToString().Length > 1 ? leave_Req[i].from_date.Month.ToString() : "0" + leave_Req[i].from_date.Month.ToString()));

                    int to_date = Convert.ToInt32(leave_Req[i].to_date.Year + Convert.ToString(leave_Req[i].to_date.Month.ToString().Length > 1 ? leave_Req[i].to_date.Month.ToString() : "0" + leave_Req[i].to_date.Month.ToString()));


                    bool exist = process_data.Any(x => x.emp_id == leave_Req[i].r_e_id && (x.payroll_month_year == from_date || x.payroll_month_year == to_date));
                    if (exist)
                    {
                        leave_Req.RemoveAll(x => x.leave_request_id == leave_Req[i].leave_request_id);
                    }

                }


                return Ok(leave_Req);
            }
            catch (Exception ex)
            {
                objResult.Message = ex.ToString();
                return Ok(objResult);
            }
        }


        [Route("Save_LeaveApplicationForApprovelByAdmin")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveCancel))]
        public async Task<IActionResult> Save_LeaveApplicationForApprovelByAdmin(LeaveAppModel tbl_leave_request)
        {

            Response_Msg objresponse = new Response_Msg();
            bool IsLWP = false;
            try
            {
                string remarksRegex = @"^[ A-Za-z0-9_.-;,']*$";

                Regex rename = new Regex(remarksRegex);

                if (!string.IsNullOrEmpty(tbl_leave_request.approval1_remarks))
                {
                    if (!rename.IsMatch(tbl_leave_request.approval1_remarks))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Invalid remarks, special characters are not allowed in remarks";
                        return Ok(objresponse);
                    }
                }

                var ReqEmpDtl = _context.tbl_leave_request.Where(a => a.is_deleted == 0 && a.is_final_approve == 1 && tbl_leave_request.leave_request_id.Contains(a.leave_request_id) && _clsCurrentUser.CompanyId.Contains(a.company_id ?? 0)).ToList();
                if (ReqEmpDtl.Count == 0)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Requests...!!";
                    return Ok(objresponse);
                }

                List<int?> _Reqempids = ReqEmpDtl.Select(p => p.r_e_id).ToList();
                foreach (var ReIds in _Reqempids)
                {
                    if (!_clsCurrentUser.DownlineEmpId.Contains(ReIds ?? 0))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Unauthorize Access";
                        return Ok(objresponse);
                    }
                }


                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        //  var exist_data = _context.tbl_leave_request.Where(a => a.is_deleted==0 && a.is_final_approve==1).ToList();

                        for (int i = 0; i < tbl_leave_request.leave_request_id.Count; i++)
                        {
                            IsLWP = false;
                            var _data = ReqEmpDtl.Where(x => x.leave_request_id == Convert.ToInt32(tbl_leave_request.leave_request_id[i])).FirstOrDefault();
                            if (_data != null)
                            {
                                if (_context.tbl_leave_type.Where(p => p.leave_type_id == _data.leave_type_id && p.leave_type_name == "LWP").Count() > 0)
                                {
                                    IsLWP = true;
                                }

                                var freeze_attandance = _context.tbl_daily_attendance.Where(x => x.emp_id == _data.r_e_id && x.is_freezed == 1 && (_data.from_date.Date <= x.attendance_dt.Date && x.attendance_dt.Date <= _data.to_date.Date)).ToList();

                                if (freeze_attandance.Count > 0)
                                {
                                    trans.Rollback();
                                    objresponse.StatusCode = 1;
                                    objresponse.Message = "" + _data.from_date.Date.ToString("dd-MM-yyyy") + "-" + _data.to_date.Date.ToString("dd-MM-yyyy") + " Attendance of Selected Leave Period already freezed";
                                    return Ok(objresponse);
                                }


                                //get frequency type whether it is monthly,quaterly, yearly or half yearly
                                var _get_frequency_type = _context.tbl_leave_credit.OrderBy(x => x.leave_credit_id).Where(p => p.leave_info_id == _data.leave_info_id && p.is_deleted == 0).FirstOrDefault();

                                if (!IsLWP)
                                {
                                    if (_get_frequency_type != null)
                                    {
                                        //START Debit from Leave Balance (In Leave Ledger) if Leave is Approved

                                        var leaveledger = _context.tbl_leave_ledger.Where(x => x.e_id == _data.r_e_id && x.leave_type_id == _data.leave_type_id && x.leave_addition_type == _get_frequency_type.frequency_type && x.dredit == _data.leave_qty && (x.entry_date.Date == _data.approval_date1.Date || x.entry_date.Date == _data.approval_date2.Date || x.entry_date.Date == _data.approval_date3.Date || x.entry_date.Date == _data.admin_ar_date.Date)).OrderByDescending(x => x.sno).ToList();

                                        if (leaveledger.Count == 0)
                                        {
                                            trans.Rollback();
                                            objresponse.StatusCode = 1;
                                            objresponse.Message = "Invalid Request, can't delete cancel leave request"; //"Leave is not debited from Leave Balance";
                                            return Ok(objresponse);
                                        }



                                        _data.is_deleted = 2;
                                        _data.deleted_by = tbl_leave_request.a1_e_id;
                                        _data.deleted_remarks = tbl_leave_request.approval1_remarks; //exist_data.deleted_remarks;
                                        _data.deleted_dt = DateTime.Now;

                                        _context.tbl_leave_request.UpdateRange(_data);
                                        //_context.Entry(exist_data).State = EntityState.Modified;



                                        tbl_leave_ledger objtbl_leave_ledger = new tbl_leave_ledger();

                                        objtbl_leave_ledger.leave_type_id = _data.leave_type_id;
                                        objtbl_leave_ledger.leave_info_id = _data.leave_info_id;
                                        objtbl_leave_ledger.transaction_date = _data.from_date;
                                        objtbl_leave_ledger.entry_date = DateTime.Now;
                                        objtbl_leave_ledger.transaction_type = 7; // Delete by system
                                        objtbl_leave_ledger.monthyear = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
                                        objtbl_leave_ledger.transaction_no = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
                                        objtbl_leave_ledger.leave_addition_type = _get_frequency_type.frequency_type;
                                        objtbl_leave_ledger.credit = _data.leave_qty; //0;
                                        objtbl_leave_ledger.dredit = 0;
                                        objtbl_leave_ledger.remarks = "Delete By Admin and reason is:-" + tbl_leave_request.approval1_remarks;
                                        objtbl_leave_ledger.e_id = _data.r_e_id;
                                        objtbl_leave_ledger.created_by = _data.a1_e_id > 0 ? _data.a1_e_id : _data.a2_e_id > 0 ? _data.a2_e_id : _data.a3_e_id > 0 ? _data.a3_e_id : _data.admin_id;

                                        //_context.Entry(objtbl_leave_ledger).State = EntityState.Added;

                                        _context.tbl_leave_ledger.AddRange(objtbl_leave_ledger);
                                    }
                                    else
                                    {
                                        trans.Rollback();
                                        objresponse.StatusCode = 1;
                                        objresponse.Message = "Leave Credit Details are not available under leave settings";
                                        return Ok(objresponse);

                                    }
                                }


                            }
                        }


                        _context.SaveChanges();

                        trans.Commit();

                        objresponse.StatusCode = 0;
                        objresponse.Message = "Request Deleted Successfully...!";
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
                objresponse.StatusCode = 1;
                objresponse.Message = ex.ToString();
                return Ok(objresponse);
            }
        }


#endregion

#region **Cancellation BY ADMIN ,STARTED BY SUPRIYA ON 31-07-2019


        [HttpGet("GetOutdoorLeaveForApprovalByAdmin/{company_id}")]
        [Authorize(Policy = nameof(enmMenuMaster.OutdoorCancel))]
        public IActionResult GetOutdoorLeaveForApprovalByAdmin([FromRoute] int company_id)
        {

            try
            {
                ResponseMsg objResult = new ResponseMsg();


                // var data = _context.tbl_daily_attendance.Where(x => x.is_freezed == 0).ToList();

                var result = _context.tbl_outdoor_request.Where(x => x.is_deleted == 0 && x.is_final_approve == 1 && _clsCurrentUser.DownlineEmpId.Contains(x.r_e_id ?? 0) && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).Select(a => new
                {

                    a.leave_request_id,
                    a.from_date,
                    a.system_in_time,
                    a.system_out_time,
                    a.manual_in_time,
                    a.manual_out_time,
                    a.r_e_id,
                    a.a1_e_id,
                    a.a2_e_id,
                    a.a3_e_id,
                    a.requester_date,
                    a.approval1_remarks,
                    a.approval2_remarks,
                    a.approval3_remarks,
                    a.approval_date1,
                    a.approval_date2,
                    a.approval_date3,
                    a.is_approved1,
                    a.is_approved2,
                    a.is_approved3,
                    a.is_deleted,
                    a.is_final_approve,
                    a.requester_remarks,
                    employee_name = string.Format("{0} {1} {2}",
                        a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_first_name != "").employee_first_name,
                         a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_middle_name != "").employee_middle_name,
                          a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_last_name != "").employee_last_name),
                    employee_code = a.tbl_employee_requester.emp_code,
                    a.deleted_remarks,
                    a.tbl_employee_requester.tbl_employee_company_map.FirstOrDefault(h => h.is_deleted == 0).company_id

                }).ToList();


                var process_data = _context.tbl_payroll_process_status.Where(x => x.is_deleted == 0 && x.is_freezed == 1 && _clsCurrentUser.DownlineEmpId.Contains(x.emp_id ?? 0) && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).ToList();


                for (int i = result.Count() - 1; i >= 0; i--)
                {
                    int from_date = Convert.ToInt32(result[i].from_date.Year.ToString() + Convert.ToString(result[i].from_date.Month.ToString().Length > 1 ? result[i].from_date.Month.ToString() : "0" + result[i].from_date.Month.ToString()));

                    bool exist = process_data.Any(x => x.emp_id == result[i].r_e_id && x.payroll_month_year == from_date);
                    if (exist)
                    {
                        result.RemoveAll(x => x.leave_request_id == result[i].leave_request_id);
                    }

                }




                return Ok(result);
                //}
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost("Save_OutdoorLeaveForApprovalByAdmin")]
        [Authorize(Policy = nameof(enmMenuMaster.OutdoorCancel))]
        public IActionResult Save_OutdoorLeaveForApprovalByAdmin([FromBody] LeaveAppModel objrequest)
        {
            Response_Msg objresponse = new Response_Msg();
            try
            {
                var all_requests = _context.tbl_outdoor_request.Where(x => x.is_deleted == 0 && x.is_final_approve == 1 && objrequest.leave_request_id.Contains(x.leave_request_id) && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).ToList();

                if (all_requests.Count == 0)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid requests!!";
                    return Ok(objresponse);
                }

                List<int?> ReqEmpIds = all_requests.Select(p => p.r_e_id).ToList();

                foreach (var reqempid in ReqEmpIds)
                {
                    if (!_clsCurrentUser.DownlineEmpId.Contains(reqempid ?? 0))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Unauthorize access....";
                        return Ok(objresponse);
                    }
                }


                string remarksRegex = @"^[ A-Za-z0-9_.-;,']*$";

                Regex rename = new Regex(remarksRegex);

                if (!string.IsNullOrEmpty(objrequest.approval1_remarks))
                {
                    if (!rename.IsMatch(objrequest.approval1_remarks))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Invalid remarks, special characters are not allowed in remarks";
                        return Ok(objresponse);
                    }
                }


                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {

                        for (int i = 0; i < objrequest.leave_request_id.Count; i++)
                        {
                            var exist_data = all_requests.Where(x => x.leave_request_id == Convert.ToInt32(objrequest.leave_request_id[i])).FirstOrDefault();
                            if (exist_data != null)
                            {

                                var tbl_daily_attendance = _context.tbl_daily_attendance.Where(x => x.emp_id == exist_data.r_e_id && x.attendance_dt.Date == exist_data.from_date.Date && x.is_freezed == 1).FirstOrDefault();
                                if (tbl_daily_attendance != null)
                                {
                                    trans.Rollback();
                                    objresponse.StatusCode = 1;
                                    objresponse.Message = "" + exist_data.from_date.Date.ToString("dd-MM-yyyy") + " Attendance of selected period is freezed";
                                    return Ok(objresponse);
                                }


                                exist_data.deleted_by = objrequest.a1_e_id;
                                exist_data.deleted_remarks = objrequest.approval1_remarks;
                                exist_data.is_deleted = 2;
                                exist_data.deleted_dt = DateTime.Now;

                                _context.tbl_outdoor_request.UpdateRange(exist_data);
                            }
                        }

                        _context.SaveChanges();
                        trans.Commit();


                        objresponse.StatusCode = 0;
                        objresponse.Message = "Outdoor Request Successfully Updated";
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        objresponse.StatusCode = 1;
                        objresponse.Message = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
            }
            return Ok(objresponse);
        }

        [HttpGet("GetAttandenceApprovalByAdmin/{company_id}")]
        [Authorize(Policy = nameof(enmMenuMaster.AttendanceCancel))]
        public IActionResult GetAttandenceApprovalByAdmin([FromRoute] int company_id)
        {
            try
            {

                var result = _context.tbl_attendace_request.Where(x => x.is_deleted == 0 && x.is_final_approve == 1).Join(_context.tbl_daily_attendance, a => new { date = a.from_date, emp_id = a.r_e_id }, b => new { date = b.attendance_dt, emp_id = b.emp_id }, (a, b) => new
                {
                    a.leave_request_id,
                    a.from_date,
                    a.system_in_time,
                    a.system_out_time,
                    a.manual_in_time,
                    a.manual_out_time,
                    a.r_e_id,
                    a.a1_e_id,
                    a.a2_e_id,
                    a.a3_e_id,
                    a.requester_date,
                    a.approval1_remarks,
                    a.approval2_remarks,
                    a.approval3_remarks,
                    a.approval_date1,
                    a.approval_date2,
                    a.approval_date3,
                    a.is_approved1,
                    a.is_approved2,
                    a.is_approved3,
                    a.is_deleted,
                    a.is_final_approve,
                    b.is_freezed,
                    b.emp_id,
                    a.requester_remarks,
                    employee_name = string.Format("{0} {1} {2}",
                      a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_first_name != "").employee_first_name,
                       a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_middle_name != "").employee_middle_name,
                        a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_last_name != "").employee_last_name),
                    employee_code = a.tbl_employee_requester.emp_code,
                    a.deleted_remarks,
                    a.company_id


                }).OrderByDescending(c => c.leave_request_id).Where(z => z.is_final_approve == 1 && z.is_deleted == 0 && z.is_freezed == 0 && _clsCurrentUser.DownlineEmpId.Contains(z.r_e_id ?? 0) && _clsCurrentUser.CompanyId.Contains(z.company_id ?? 0)).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost("Save_AttandenceLeaveForApprovalByAdmin")]
        [Authorize(Policy = nameof(enmMenuMaster.AttendanceCancel))]
        public IActionResult Save_AttandenceLeaveForApprovalByAdmin([FromBody] LeaveAppModel objreqst)
        {
            Response_Msg objresponse = new Response_Msg();
            try
            {
                var all_request = _context.tbl_attendace_request.Where(x => x.is_deleted == 0 && x.is_final_approve == 1 && objreqst.leave_request_id.Contains(x.leave_request_id) && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).ToList();
                if (all_request.Count == 0)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Requests or Unauthorize Company access";
                    return Ok(objresponse);
                }

                List<int?> ReqEmpIDs = all_request.Select(p => p.r_e_id).ToList();

                foreach (var IDs in ReqEmpIDs)
                {
                    if (!_clsCurrentUser.DownlineEmpId.Contains(IDs ?? 0))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Unauthorize Access";
                        return Ok(objresponse);
                    }
                }


                string remarksRegex = @"^[ A-Za-z0-9_.-;,']*$";

                Regex rename = new Regex(remarksRegex);

                if (!string.IsNullOrEmpty(objreqst.approval1_remarks))
                {
                    if (!rename.IsMatch(objreqst.approval1_remarks))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Invalid remarks, special characters are not allowed in remarks";
                        return Ok(objresponse);
                    }
                }

                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        //var _data = _context.tbl_attendace_request.Where(x => x.is_deleted == 0 && x.is_final_approve == 1).ToList();

                        for (int i = 0; i < objreqst.leave_request_id.Count; i++)
                        {
                            var exist = all_request.Where(x => x.leave_request_id == Convert.ToInt32(objreqst.leave_request_id[i])).FirstOrDefault();
                            if (exist != null)
                            {

                                var tbl_daily_attendance = _context.tbl_daily_attendance.Where(x => x.emp_id == exist.r_e_id && x.attendance_dt.Date == exist.from_date.Date && x.is_freezed == 1).FirstOrDefault();
                                if (tbl_daily_attendance != null)
                                {
                                    trans.Rollback();
                                    objresponse.StatusCode = 1;
                                    objresponse.Message = "" + exist.from_date.Date.ToString("dd-MM-yyyy") + " Attendance of selected period is freezed";
                                    return Ok(objresponse);
                                }


                                exist.is_deleted = 2;
                                exist.deleted_by = objreqst.a1_e_id;
                                exist.deleted_remarks = objreqst.approval1_remarks;
                                exist.deleted_dt = DateTime.Now;

                                _context.tbl_attendace_request.UpdateRange(exist);

                            }
                        }

                        _context.SaveChanges();
                        trans.Commit();

                        objresponse.StatusCode = 0;
                        objresponse.Message = "Attendance Request Successfully Updated";
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
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [HttpGet("GetCompOffLeaveApprovalByAdmin/{company_id}")]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffCancel))]
        public IActionResult GetCompOffLeaveApprovalByAdmin([FromRoute] int company_id)
        {
            try
            {
                var attandance_data = _context.tbl_daily_attendance.Where(x => x.is_freezed == 0 && _clsCurrentUser.DownlineEmpId.Contains(x.emp_id ?? 0)).ToList();

                var result = (from a in _context.tbl_comp_off_request_master
                              join b in attandance_data on new { date = a.compoff_against_date, emp_id = a.r_e_id } equals
new { date = b.attendance_dt, emp_id = b.emp_id } into ej
                              from b in ej.DefaultIfEmpty()
                              where
a.is_deleted == 0 && a.is_final_approve == 1 && _clsCurrentUser.DownlineEmpId.Contains(a.r_e_id ?? 0) && _clsCurrentUser.CompanyId.Contains(a.company_id ?? 0)
                              orderby a.comp_off_request_id descending
                              select new
                              {
                                  a.comp_off_request_id,
                                  a.compoff_against_date,
                                  a.compoff_date,
                                  a.r_e_id,
                                  a.a1_e_id,
                                  a.a2_e_id,
                                  a.a3_e_id,
                                  a.requester_date,
                                  a.approval1_remarks,
                                  a.approval2_remarks,
                                  a.approval3_remarks,
                                  a.approval_date1,
                                  a.approval_date2,
                                  a.approval_date3,
                                  a.is_approved1,
                                  a.is_approved2,
                                  a.is_approved3,
                                  a.is_deleted,
                                  a.is_final_approve,
                                  //b.is_freezed,
                                  //b.emp_id,
                                  a.requester_remarks,
                                  emp_code = a.tbl_employee_requester.emp_code,
                                  employee_name = string.Format("{0} {1} {2}",
                                                    a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).employee_first_name,
                                                        a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).employee_middle_name,
                                                        a.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).employee_last_name),
                                  a.deleted_remarks,
                                  a.tbl_employee_requester.tbl_employee_company_map.FirstOrDefault(h => h.is_deleted == 0).company_id


                              }).ToList();

                var process_data = _context.tbl_payroll_process_status.Where(x => x.is_deleted == 0 && x.is_freezed == 1 && _clsCurrentUser.DownlineEmpId.Contains(x.emp_id ?? 0) && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).ToList();


                for (int i = result.Count() - 1; i >= 0; i--)
                {
                    int from_date = Convert.ToInt32(result[i].compoff_date.Date.Year.ToString() + Convert.ToString(result[i].compoff_date.Month.ToString().Length > 1 ? result[i].compoff_date.Month.ToString() : "0" + result[i].compoff_date.Month.ToString()));

                    bool exist = process_data.Any(x => x.emp_id == result[i].r_e_id && x.payroll_month_year == from_date);
                    if (exist)
                    {
                        result.RemoveAll(x => x.comp_off_request_id == result[i].comp_off_request_id);
                    }

                }



                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpPost("Save_CompOffLeaveForApprovalByAdmin")]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffCancel))]
        public IActionResult Save_CompOffLeaveForApprovalByAdmin([FromBody] LeaveAppModel objrequest)
        {
            Response_Msg objresponse = new Response_Msg();
            try
            {
                string remarksRegex = @"^[ A-Za-z0-9_.-;,']*$";

                Regex rename = new Regex(remarksRegex);

                if (!string.IsNullOrEmpty(objrequest.approval1_remarks))
                {
                    if (!rename.IsMatch(objrequest.approval1_remarks))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Invalid remarks, special characters are not allowed in remarks";
                        return Ok(objresponse);
                    }
                }

                var all_request = _context.tbl_comp_off_request_master.Where(x => x.is_deleted == 0 && x.is_final_approve == 1 && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).ToList();
                List<int?> ReqEmpIds = all_request.Select(x => x.r_e_id).ToList();

                foreach (var ids in ReqEmpIds)
                {
                    if (!_clsCurrentUser.DownlineEmpId.Contains(ids ?? 0))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Unauthorize Access...!!";
                        return Ok(objresponse);
                    }
                }


                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // var _data = _context.tbl_comp_off_request_master.Where(x => x.is_deleted==0 && x.is_final_approve==1).ToList();

                        var freeze_attandance = _context.tbl_daily_attendance.Where(x => x.is_freezed == 1 && _clsCurrentUser.DownlineEmpId.Contains(x.emp_id ?? 0)).ToList();

                        for (int i = 0; i < objrequest.leave_request_id.Count; i++)
                        {
                            var exist_data = all_request.Where(x => x.comp_off_request_id == Convert.ToInt32(objrequest.leave_request_id[i])).FirstOrDefault();

                            if (exist_data != null)
                            {
                                var _attandance = freeze_attandance.Where(x => x.emp_id == exist_data.r_e_id && x.is_freezed == 1 && x.attendance_dt.Date == exist_data.compoff_date.Date).ToList();

                                if (freeze_attandance.Count > 0)
                                {
                                    trans.Rollback();
                                    objresponse.StatusCode = 1;
                                    objresponse.Message = "Attendance of Selected Leave Period already freezed";
                                    return Ok(objresponse);

                                }


                                //var exist_comp_off = _context.tbl_comp_off_ledger.Where(x => x.e_id == exist_data.r_e_id && x.compoff_date.Date == exist_data.compoff_against_date.Date).ToList()
                                //                  .GroupBy(p => p.e_id).Select(q => new
                                //                  {
                                //                      totalcredit = q.Sum(r => r.credit),
                                //                      totaldebit = q.Sum(r => r.dredit),
                                //                      leavebalance = q.Sum(x => x.credit) - q.Sum(x => x.dredit)
                                //                  }).FirstOrDefault();

                                //if (exist_comp_off == null)
                                //{
                                //    trans.Rollback();
                                //    objresponse.StatusCode = 1;
                                //    objresponse.Message = "Compoff not available in your account, please check and try !!";
                                //    return Ok(objresponse);
                                //}


                                exist_data.is_deleted = 2;
                                exist_data.deleted_remarks = objrequest.approval1_remarks;
                                exist_data.deleted_by = objrequest.a1_e_id;
                                exist_data.deleted_dt = DateTime.Now;

                                _context.tbl_comp_off_request_master.UpdateRange(exist_data);



                                //start if compoff leave request delete by admin, than again credit leave of same date in compoff leave ledger


                                tbl_comp_off_ledger objcomledger = new tbl_comp_off_ledger();

                                objcomledger.compoff_date = exist_data.compoff_against_date.Date;
                                objcomledger.credit = exist_data.compoff_request_qty;
                                objcomledger.dredit = 0;
                                objcomledger.transaction_date = DateTime.Now;
                                objcomledger.transaction_type = 7; //delete by system
                                objcomledger.monthyear = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
                                objcomledger.transaction_no = Convert.ToInt32("-" + exist_data.compoff_against_date.ToString("yyyyMMdd"));
                                objcomledger.remarks = "Delete by Admin:-" + objrequest.approval1_remarks;
                                objcomledger.e_id = exist_data.r_e_id;

                                _context.tbl_comp_off_ledger.AddRange(objcomledger);

                                //end if compoff leave request delete by admin, than again credit leave of same date in compoff leave ledger
                            }
                        }

                        _context.SaveChanges();
                        trans.Commit();
                        objresponse.StatusCode = 0;
                        objresponse.Message = "CompOff Request Successfully Updated";


                        return Ok(objresponse);
                    }
                    catch (Exception ex)
                    {
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
#endregion ** END BY SUPRIYA ON 31-07-2019


        [Route("Get_LeaveInfo_for_LeaveTypeByCompIDD/{transactiondate}/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "5036")]
        public IActionResult Get_LeaveInfo_for_LeaveTypeByCompIDD([FromRoute] DateTime transactiondate, int company_id)
        {
            try
            {

                var data = _context.tbl_leave_info.Where(x => x.is_active == 1 && x.leave_tenure_from_date.Date <= transactiondate.Date &&
                  transactiondate.Date <= x.leave_tenure_to_date.Date).Select(p => new { p.leave_type_id }).Distinct().ToList();

                //var only_date = transactiondate.ToString("yyyy-MM-dd 00:00:00"); 

                //var data = _context.tbl_leave_appcbl_on_company.Where(a => a.c_id == company_id &&
                //  a.tbl_leave_applicablity.tbl_leave_info.leave_tenure_from_date.Date <= transactiondate.Date &&
                //  a.tbl_leave_applicablity.tbl_leave_info.leave_tenure_to_date.Date >= transactiondate.Date && a.tbl_leave_applicablity.tbl_leave_info.is_active == 1).Select(a => new
                //  {
                //      a.tbl_leave_applicablity.tbl_leave_info.leave_type_id
                //  }).Distinct().ToList();
                // var data = _context.tbl_leave_info.AsEnumerable().Where(a => a.leave_tenure_from_date.Date <= transactiondate.Date && a.leave_tenure_to_date.Date >= transactiondate.Date).ToList();
                if (data.Count > 0)
                {
                    List<tbl_leave_type> objlist = new List<tbl_leave_type>();
                    foreach (var item in data)
                    {
                        tbl_leave_type objleavetype = new tbl_leave_type();

                        var leavetypee = _context.tbl_leave_type.OrderByDescending(a => a.leave_type_id).Where(b => b.leave_type_id == item.leave_type_id).FirstOrDefault();

                        objleavetype.leave_type_id = leavetypee.leave_type_id;
                        objleavetype.leave_type_name = leavetypee.leave_type_name;

                        objlist.Add(objleavetype);
                    }
                    return Ok(objlist);
                }
                else
                {
                    ResponseMsg objresponse = new ResponseMsg();
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Please Select another date";
                    return Ok(objresponse);
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }

        [Route("Get_LeaveInfo_forLeavetypebycurrentdateandCompID/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveApplication))]
        public IActionResult Get_LeaveInfo_forLeavetypebycurrentdateandCompID([FromRoute] int company_id)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                if (!_clsCurrentUser.CompanyId.Contains(company_id))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unauthorize Access...";
                    return Ok(objresponse);
                }

                DateTime currentDt = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));

                var data = _context.tbl_leave_info.Where(p => p.is_active == 1 && p.leave_tenure_from_date <= currentDt && p.leave_tenure_to_date >= currentDt)
                    .Select(p => new { p.leave_info_id, p.tbl_leave_type.leave_type_name, p.leave_type_id }).Distinct().ToList();

                //var data = _context.tbl_leave_appcbl_on_company.Where(x => x.c_id == company_id
                //&& x.tbl_leave_applicablity.tbl_leave_info.is_active == 1
                //&& x.tbl_leave_applicablity.tbl_leave_info.leave_tenure_from_date.Date <= Convert.ToDateTime(DateTime.Now).Date
                //&& x.tbl_leave_applicablity.tbl_leave_info.leave_tenure_to_date.Date >= Convert.ToDateTime(DateTime.Now).Date).Select(p => new
                //{
                //    p.tbl_leave_applicablity.tbl_leave_info.leave_info_id,
                //    p.tbl_leave_applicablity.tbl_leave_info.leave_type_id,
                //    p.tbl_leave_applicablity.tbl_leave_info.tbl_leave_type.leave_type_name
                //}).Distinct().ToList();

                return Ok(data);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        public class leave_holiday_weekoff
        {
            public int Holiday { get; set; }
            public int WeekOff { get; set; }
        }
        public leave_holiday_weekoff check_holiday_weekoff_exist_on_leaveapp(DateTime FromDate, DateTime ToDate, int emp_id)
        {

            throw new NotImplementedException();
#if false
            leave_holiday_weekoff objResult = new leave_holiday_weekoff();
            Dictionary<List<HolidayData>, List<ShiftWeekOffData>> Data = new Dictionary<List<HolidayData>, List<ShiftWeekOffData>>();
            try
            {
                DateTime? fromdate; DateTime? todate;

                fromdate = Convert.ToDateTime(FromDate.AddMonths(-1).ToString("dd-MMM-yyyy 00:00:00 tt"));
                todate = Convert.ToDateTime(ToDate.AddMonths(1).ToString("dd-MMM-yyyy 00:00:00 tt"));

                List<tbl_emp_officaial_sec> teos = _context.tbl_emp_officaial_sec.Where(p => p.employee_id == emp_id && p.is_deleted == 0).ToList();
                Int32 PayrollDate = 1;
                var payroll = _context.tbl_payroll_month_setting.Where(p => p.is_deleted == 0).FirstOrDefault();
                if (payroll != null)
                {
                    PayrollDate = Convert.ToInt32(payroll.from_date);
                }

                Classes.clsCalcualteDailyAttendance CalculateAttendance = new Classes.clsCalcualteDailyAttendance(
                    _context, teos[0].employee_id ?? 0, teos[0].emp_official_section_id, teos[0].location_id ?? 0,
                    teos[0].religion_id ?? 0, Convert.ToByte(teos[0].is_fixed_weekly_off), PayrollDate, teos[0].punch_type,
                    teos[0].is_ot_allowed, Convert.ToByte(teos[0].is_comb_off_allowed), fromdate.Value
                    , todate.Value, teos[0].date_of_joining);
                Data = CalculateAttendance.GetEmpWeekOff();
                List<HolidayData> HDL = new List<HolidayData>();
                List<ShiftWeekOffData> SDL = new List<ShiftWeekOffData>();
                foreach (KeyValuePair<List<HolidayData>, List<ShiftWeekOffData>> pair in Data)
                {
                    HDL = pair.Key;
                    SDL = pair.Value;
                }

                var h = HDL.FirstOrDefault(a => (FromDate <= a.HolidayDate && a.HolidayDate <= ToDate));
                int IsHoliday = h != null ? 1 : 0;

                var s = SDL.FirstOrDefault(a => (FromDate <= a.AttendanceDate && a.AttendanceDate <= ToDate));
                int IsWeekOff = s != null ? 1 : 0;

                objResult.Holiday = IsHoliday;
                objResult.WeekOff = IsWeekOff;

                return objResult;

            }
            catch
            {
                return objResult;
            }
#endif

        }

        [Route("LeaveDetailByEmpID/{leave_type}/{emp_id}/{fromdate}/{todate}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Leave))]
        public IActionResult LeaveDetailByEmpID(int leave_type, int emp_id, DateTime? fromdate, DateTime? todate)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }

            if (fromdate == null)
            {
                fromdate = Convert.ToDateTime("01-01-2000");
            }

            if (todate == null)
            {
                todate = Convert.ToDateTime("01-01-2000");
            }


            try
            {

                var leavedata = _context.tbl_leave_ledger.Where(x => x.e_id == emp_id && x.leave_type_id == leave_type && fromdate.Value.Date <= x.transaction_date.Date && x.transaction_date.Date <= todate.Value.Date).Select(p => new
                {
                    p.transaction_date,
                    p.credit,
                    p.dredit,
                    remarks = p.credit > 0 ? "Leave Accrued" : "Leave Approved",
                    p.e_id,
                    p.sno,
                }).ToList();


                var leaveledgertotal = leavedata.ToList().GroupBy(p => p.e_id).Select(q => new
                {

                    totalcredit = q.Sum(r => r.credit),
                    totaldebit = q.Sum(r => r.dredit),
                    totalbalance = q.Sum(r => r.credit) - q.Sum(r => r.dredit)
                }).FirstOrDefault();


                var data = new { leavedata = leavedata, leaveledgertotal = leaveledgertotal };

                return Ok(data);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }

        }

        [Route("GetLeaveApplicationByEmpIDandDate/{emp_id}/{s_no}/{leave_date}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Leave))]

        public IActionResult GetLeaveApplicationByEmpIDandDate(int emp_id, int s_no, DateTime? leave_date)
        {
            Response_Msg objresponse = new Response_Msg();
            try
            {

                if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unauthorized access...!!";
                    return Ok(objresponse);
                }

                if (leave_date == null)
                {
                    leave_date = Convert.ToDateTime("01-01-2000");
                }
                var _exist = _context.tbl_leave_ledger.Where(x => x.sno == s_no && x.transaction_date.Date == leave_date.Value.Date && x.e_id == emp_id).FirstOrDefault();
                if (_exist != null)
                {
                    var data = _context.tbl_leave_request.Where(x => x.is_deleted == 0 && x.r_e_id == emp_id && x.from_date.Date <= leave_date.Value.Date && leave_date.Value.Date <= x.to_date.Date && x.is_final_approve != 0).Select(p => new
                    {
                        p.leave_request_id,
                        p.from_date,
                        p.to_date,
                        p.leave_qty,
                        p.leave_applicable_for,
                        p.day_part,
                        p.r_e_id,
                        p.requester_remarks,
                        emp_name = string.Format("{0} {1} {2}",
                       p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                       p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                       p.tbl_employee_requester.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name),
                        emp_code = p.tbl_employee_requester.emp_code,
                        p.a1_e_id,
                        p.approval1_remarks,
                        p.approval_date1,
                        approver1_ = string.Format("{0} {1} {2}",
                       p.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                        p.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                         p.tbl_employee_approval1.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name),
                        p.a2_e_id,
                        p.approval2_remarks,
                        p.approval_date2,
                        approver2_ = string.Format("{0} {1} {2}",
                       p.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                        p.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                         p.tbl_employee_approval2.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name),
                        p.a3_e_id,
                        p.approval3_remarks,
                        p.approval_date3,
                        approver3_ = string.Format("{0} {1} {2}",
                       p.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                        p.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                         p.tbl_employee_approval3.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name),


                        p.admin_ar_date,
                        p.admin_remarks,
                        p.admin_id,
                        admin_ = string.Format("{0} {1} {2}", p.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                         p.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                         p.tem_admin.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name)
                    }).FirstOrDefault();
                    if (data != null)
                    {
                        return Ok(data);
                    }
                    else
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Sorry No Leave Request Available between this date";
                        return Ok(objresponse);
                    }
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Something went wrong,Please try again later";
                    return Ok(objresponse);
                }

            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(ex.Message);
            }
        }

        [Route("DeleteOutdoorleaveApplication")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.OutdoorApplication))]
        public IActionResult DeleteOutdoorleaveApplication([FromBody] LeaveApplicationModel LeaveModel)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.DownlineEmpId.Contains(LeaveModel.r_e_id ?? 0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }


            try
            {
                var exist = _context.tbl_outdoor_request.Where(x => x.leave_request_id == Convert.ToInt32(LeaveModel.leave_request_id) && x.r_e_id == LeaveModel.r_e_id && x.is_deleted == 0).FirstOrDefault();
                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid request id, please check and try !!";
                    return Ok(objResult);
                }

                if (exist.is_final_approve == 1)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Leave application already approved can't delete !!";
                    return Ok(objResult);
                }
                else if (exist.is_final_approve == 2)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Leave application already reject can't delete !!";
                    return Ok(objResult);
                }




                var freeze_attandance = _context.tbl_daily_attendance.Where(x => x.emp_id == exist.r_e_id && x.tbl_employee_master.tbl_employee_company_map.FirstOrDefault(q => q.is_deleted == 0).company_id == exist.company_id && x.attendance_dt.Date == exist.from_date.Date && x.is_freezed == 1).ToList();

                if (freeze_attandance.Count > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Attendance of selected leave period is freezed";
                    return Ok(objResult);

                }

                exist.is_deleted = 1;
                //exist.leave_request_id = Convert.ToInt32(LeaveModel.leave_request_id);
                exist.deleted_by = Convert.ToInt32(LeaveModel.r_e_id);
                exist.deleted_dt = DateTime.Now;
                exist.deleted_remarks = "deleted by user/Manager";

                _context.Entry(exist).State = EntityState.Modified;
                _context.SaveChanges();

                objResult.StatusCode = 0;
                objResult.Message = "Outdoor Leave application deleted successfully !!";
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        [Route("DeleteCompOffLeaveApplication")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.CompoffApplication))]
        public IActionResult DeleteCompOffLeaveApplication([FromBody] LeaveApplicationModel LeaveModel)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.DownlineEmpId.Contains(LeaveModel.r_e_id ?? 0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access....!!";
                return Ok(objResult);
            }

            try
            {
                var exist = _context.tbl_comp_off_request_master.Where(x => x.comp_off_request_id == Convert.ToInt32(LeaveModel.leave_request_id) && x.r_e_id == LeaveModel.r_e_id && x.is_deleted == 0).FirstOrDefault();
                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid request id, please check and try !!";
                    return Ok(objResult);
                }

                //check already approved
                var is_approved = _context.tbl_comp_off_request_master.Where(x => x.comp_off_request_id == Convert.ToInt32(LeaveModel.leave_request_id) && x.is_deleted == 0 && x.is_final_approve == 1).Count();
                if (is_approved > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Leave application already approved can't delete !!";
                    return Ok(objResult);
                }

                exist.is_deleted = 1;
                //exist.comp_off_request_id = Convert.ToInt32(LeaveModel.leave_request_id);
                exist.deleted_by = Convert.ToInt32(LeaveModel.r_e_id);
                exist.deleted_dt = DateTime.Now;
                exist.deleted_remarks = "deleted by user";

                _context.Entry(exist).State = EntityState.Modified;
                _context.SaveChanges();

                objResult.StatusCode = 0;
                objResult.Message = "CompOff application deleted successfully !!";
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        [Route("DeleteAttendanceLeaveApplication")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.AttendanceApplication))]
        public IActionResult DeleteAttendanceLeaveApplication([FromBody] LeaveApplicationModel LeaveModel)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {
                if (!_clsCurrentUser.DownlineEmpId.Contains(LeaveModel.r_e_id ?? 0))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access...!!";
                    return Ok(objResult);
                }

                var exist = _context.tbl_attendace_request.Where(x => x.leave_request_id == Convert.ToInt32(LeaveModel.leave_request_id) && x.r_e_id == LeaveModel.r_e_id && x.is_deleted == 0).FirstOrDefault();
                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid request id, please check and try !!";
                    return Ok(objResult);
                }

                if (exist.is_final_approve == 1)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Leave application already approved can't delete !!";
                    return Ok(objResult);
                }
                else if (exist.is_final_approve == 2)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Leave application already rejected can't delete !!";
                    return Ok(objResult);
                }


                var freeze_attandance = _context.tbl_daily_attendance.Where(x => x.emp_id == exist.r_e_id && x.tbl_employee_master.tbl_employee_company_map.FirstOrDefault(q => q.is_deleted == 0).company_id == exist.company_id && x.is_freezed == 1 && x.attendance_dt.Date == exist.from_date.Date).ToList();

                if (freeze_attandance.Count > 0)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Attendance of selected period is freezed";
                    return Ok(objResult);

                }


                exist.is_deleted = 1;
                //exist.comp_off_request_id = Convert.ToInt32(LeaveModel.leave_request_id);
                exist.deleted_by = Convert.ToInt32(LeaveModel.r_e_id);
                exist.deleted_dt = DateTime.Now;
                exist.deleted_remarks = "deleted by user";

                _context.Entry(exist).State = EntityState.Modified;
                _context.SaveChanges();

                objResult.StatusCode = 0;
                objResult.Message = "Attendance Leave application deleted successfully !!";
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }


#region ** START UPLOAD LEAVE CREDIT ,DEBIT 03-06-2020 **
        [Route("Upload_LeavefromExcel")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Upload))]
        public async Task<IActionResult> Upload_LeavefromExcel()
        {
            throw new NotImplementedException();
#if false
            try
            {
                ResponseMsg objresponse = new ResponseMsg();
                var files = HttpContext.Request.Form.Files;
                var a = HttpContext.Request.Form["AllData"];
                if (string.IsNullOrEmpty(a.ToString()))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Data !!";
                    return Ok(objresponse);
                }


                CommonClass objcom = new CommonClass();
                LeaveLedgerModell objleave = new LeaveLedgerModell();
                objleave = objcom.ToObjectFromJSON<LeaveLedgerModell>(a.ToString());

                if (!_clsCurrentUser.CompanyId.Contains(objleave.company_id))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unauthorize Company Access...!!";
                    return Ok(objresponse);
                }

                //open excel using openxml sdk
                StringBuilder excelResult = new StringBuilder();
                List<LeaveLedgerModell> Leaveledgerlst = new List<LeaveLedgerModell>();
                string get_file_path = "";

                if (files != null)
                {
                    var allowedExtensions = new[] { ".xlsx" };

                    var ext = Path.GetExtension(files[0].FileName); //getting the extension
                    if (allowedExtensions.Contains(ext.ToLower()))//check what type of extension  
                    {
                        string name = Path.GetFileNameWithoutExtension(files[0].FileName); //getting file name without extension  

                        string company_name = _context.tbl_company_master.OrderByDescending(x => x.company_id).Where(y => y.company_id == objleave.company_id && y.is_active == 1).Select(p => p.company_name).FirstOrDefault();
                        string MyFileName = "EmpLeaveDetail_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_tt") + ext; //Guid.NewGuid().ToString().Replace("-", "") +


                        var webRoot = _hostingEnvironment.WebRootPath;

                        string currentmonth = Convert.ToString(DateTime.Now.Month).Length.ToString() == "1" ? "0" + Convert.ToString(DateTime.Now.Month) : Convert.ToString(DateTime.Now.Month);

                        var currentyearmonth = Convert.ToString(DateTime.Now.Year) + currentmonth;


                        if (!Directory.Exists(webRoot + "/EmployeeDetail/" + company_name + "/EmpLeaveDetail/" + currentyearmonth + "/"))
                        {
                            Directory.CreateDirectory(webRoot + "/EmployeeDetail/" + company_name + "/EmpLeaveDetail/" + currentyearmonth + "/");

                        }

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/EmployeeDetail/" + company_name + "/EmpLeaveDetail/" + currentyearmonth + "/");

                        //save file
                        using (var fileStream = new FileStream(Path.Combine(path, MyFileName), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);

                            get_file_path = fileStream.Name;
                            //using (SpreadsheetDocument doc = SpreadsheetDocument.Open("F:\\Documentss\\Employee Officaial Section.xlsx", false))

                        }
                    }
                    else
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Please Select Only Excel File";
                        return Ok(objresponse);
                    }
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Please Select Excel File for Upload";
                    return Ok(objresponse);
                }


                //read Excel

                if (!string.IsNullOrEmpty(get_file_path))
                {
                    using (SpreadsheetDocument doc = SpreadsheetDocument.Open(get_file_path, false))
                    {
                        //create the object for workbook part  
                        WorkbookPart workbookPart = doc.WorkbookPart;
                        Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();
                        //StringBuilder excelResult = new StringBuilder();

                        //array list to store employee code

                        var pathh = Path.GetTempPath();
                        //using for each loop to get the sheet from the sheetcollection  
                        foreach (Sheet thesheet in thesheetcollection)
                        {
                            excelResult.AppendLine("Excel Sheet Name : " + thesheet.Name);
                            excelResult.AppendLine("----------------------------------------------- ");
                            //statement to get the worksheet object by using the sheet id  
                            Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;

                            SheetData thesheetdata = (SheetData)theWorksheet.GetFirstChild<SheetData>();



                            foreach (Row thecurrentrow in thesheetdata)
                            {
                                //skip header row
                                if (thecurrentrow.RowIndex != 1)
                                {
                                    LeaveLedgerModell objleave_ledger = new LeaveLedgerModell();
                                    string currentcolumnno = string.Empty;

                                    foreach (Cell thecurrentcell in thecurrentrow)
                                    {
                                        currentcolumnno = thecurrentcell.CellReference.ToString().Substring(0, 1).ToUpper();
                                        //skip sr no.
                                        if (currentcolumnno != "A")
                                        {

                                            //statement to take the integer value  
                                            string currentcellvalue = string.Empty;
                                            if (thecurrentcell.DataType != null)
                                            {
                                                if (thecurrentcell.DataType == CellValues.SharedString)
                                                {
                                                    int id;
                                                    if (Int32.TryParse(thecurrentcell.InnerText, out id))
                                                    {
                                                        SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                                        if (item.Text != null)
                                                        {
                                                            //code to take the string value  
                                                            excelResult.Append(item.Text.Text + " ");
#region ** START Add value in list **
                                                            if (!string.IsNullOrEmpty(item.Text.Text) && currentcolumnno == "B")
                                                            {
                                                                objleave_ledger.emp_name_code = item.Text.Text;
                                                            }
                                                            else if (!string.IsNullOrEmpty(item.Text.Text) && currentcolumnno == "C")
                                                            {
                                                                objleave_ledger.leave_type_name = item.Text.Text;
                                                            }
                                                            else if (!string.IsNullOrEmpty(item.Text.Text) && currentcolumnno == "D")
                                                            {
                                                                objleave_ledger.credit = Convert.ToDouble(item.Text.Text);
                                                            }
                                                            else if (!string.IsNullOrEmpty(item.Text.Text) && currentcolumnno == "E")
                                                            {
                                                                objleave_ledger.dredit = Convert.ToDouble(item.Text.Text);
                                                            }
                                                            else if (!string.IsNullOrEmpty(item.Text.Text) && currentcolumnno == "F")
                                                            {
                                                                objleave_ledger.remarks = item.Text.Text;
                                                            }

#endregion ** END value in list**
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //read columns value
                                                excelResult.Append(Convert.ToString(thecurrentcell.InnerText) + " ");
#region ** START Add value in list **

                                                if (!string.IsNullOrEmpty(thecurrentcell.InnerText) && currentcolumnno == "B")
                                                {
                                                    objleave_ledger.emp_name_code = thecurrentcell.InnerText;
                                                }
                                                else if (!string.IsNullOrEmpty(thecurrentcell.InnerText) && currentcolumnno == "C")
                                                {
                                                    objleave_ledger.leave_type_name = thecurrentcell.InnerText;
                                                }
                                                else if (!string.IsNullOrEmpty(thecurrentcell.InnerText) && currentcolumnno == "D")
                                                {
                                                    objleave_ledger.credit = Convert.ToDouble(thecurrentcell.InnerText);
                                                }
                                                else if (!string.IsNullOrEmpty(thecurrentcell.InnerText) && currentcolumnno == "E")
                                                {
                                                    objleave_ledger.dredit = Convert.ToDouble(thecurrentcell.InnerText);
                                                }
                                                else if (!string.IsNullOrEmpty(thecurrentcell.InnerText) && currentcolumnno == "F")
                                                {
                                                    objleave_ledger.remarks = thecurrentcell.InnerText;
                                                }

#endregion ** END value in list**

                                            }

                                        }

                                    }
                                    excelResult.AppendLine();
                                    objleave_ledger.created_by = objleave.created_by;
                                    objleave_ledger.company_id = objleave.company_id;


                                    Leaveledgerlst.Add(objleave_ledger);
                                }

                            }
                            excelResult.Append("");
                        }

                    }
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unable to Save File...";
                    return Ok(objresponse);
                }


                var data = excelResult.ToString(); // only to check
                var data1 = Leaveledgerlst; // only for checking a list

                //start check uplicate,missing details
                var dataaa = CheckLeaveFromExcel(Leaveledgerlst);

                var duplicate_dtl = dataaa.duplicateleavelst;
                var missing_dtll = dataaa.missingleavelst;
                var adddblistt = dataaa.adddbleavelst;
                var missingDtlMessage = dataaa.Message_;

                if (duplicate_dtl.Count > 0 || missing_dtll.Count > 0)
                {
                    return Ok(dataaa);
                }
                else
                {
                    //start save details

                    clsLeaveCredit objleave_cls = new clsLeaveCredit(_context, adddblistt[0].company_id);


                    int result = objleave_cls.Save_Mannual_Leave(adddblistt);
                    if (result == 1)
                    {
                        objresponse.StatusCode = 0;
                        objresponse.Message = "Leave details successfully saved..";
                        return Ok(objresponse);
                    }
                    else
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Something went wrong, please try after sometime...";
                        return Ok(objresponse);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
#endif
        }


        private LeaveLedgerList CheckLeaveFromExcel(List<LeaveLedgerModell> objlst)
        {
            throw new NotImplementedException();
#if false

            StringBuilder msg = new StringBuilder();

            clsLeaveCredit objcls_ = new clsLeaveCredit(_context, objlst[0].company_id);
            List<tbl_leave_type> leave_type_lst = objcls_.Get_Leave_typesByStarting_Year_Date();

            List<LeaveLedgerModell> missingleavelst = new List<LeaveLedgerModell>();
            List<LeaveLedgerModell> adddbleavelst = new List<LeaveLedgerModell>();
            List<LeaveLedgerModell> duplicateleavelst = new List<LeaveLedgerModell>();

            try
            {

                var selected_comp_emp = _context.tbl_employee_company_map.Where(x => x.is_deleted == 0 && x.is_default == true && x.company_id == objlst.FirstOrDefault().company_id).Select(p => p.employee_id).ToList();
                var errormsg = "";

                msg.Append("");

                for (int i = 0; i < objlst.Count; i++)
                {

                    bool _invalid = false;


                    if (!string.IsNullOrEmpty(objlst[i].emp_name_code))
                    {


                        var exist_code = _context.tbl_employee_master.OrderByDescending(x => x.employee_id).Where(y => y.is_active == 1 && y.emp_code == objlst[i].emp_name_code).FirstOrDefault();
                        if (exist_code == null)
                        {
                            // emp_code = true;
                            _invalid = true;
                            errormsg = objlst[i].emp_name_code + ",Employee Code Not exist";
                            msg.Append("" + objlst[i].emp_name_code + ",Employee Code Not exist</br>");
                        }
                        else
                        {
                            if (!selected_comp_emp.Contains(exist_code.employee_id))
                            {
                                _invalid = true;
                                errormsg = objlst[i].emp_name_code + ",Employee Code Not exist in selected company";
                                msg.Append("" + objlst[i].emp_name_code + ",Employee Code Not exist in selected company</br>");
                            }
                            else
                            {
                                objlst[i].e_id = exist_code.employee_id;
                            }

                        }

                    }
                    else
                    {
                        // emp_code = true;
                        _invalid = true;
                        errormsg = objlst[i].emp_name_code + ",Employee Code missing";
                        msg.Append("" + objlst[i].emp_name_code + ",Employee Code missing <br/>");

                    }

                    if (objlst[i].e_id != 0)
                    {
                        var company_ = _context.tbl_employee_company_map.OrderByDescending(x => x.sno).Where(y => y.is_deleted == 0 && y.employee_id == objlst[i].e_id).FirstOrDefault();
                        if (company_ == null || company_.company_id == 0)
                        {
                            // company_not_map = true;
                            _invalid = true;
                            errormsg = objlst[i].emp_name_code + ",Invalid Employee Code";
                            msg.Append("" + objlst[i].emp_name_code + ",Invalid Employee Code <br/>");
                        }
                    }


                    if (!string.IsNullOrEmpty(objlst[i].leave_type_name))
                    {
                        var leave_type_ = leave_type_lst.Where(x => x.leave_type_name.Trim().ToUpper() == objlst[i].leave_type_name.Trim().ToUpper()).FirstOrDefault();
                        if (leave_type_ == null)
                        {
                            //  component_name = true;
                            _invalid = true;
                            errormsg = objlst[i].leave_type_name + " , Invalid Leave Type";
                            msg.Append("" + objlst[i].leave_type_name + " , Invalid Leave Type, Please enter only that leave type which are available in dropdown of Mannaul Leave <br/>");
                        }
                        else
                        {
                            objlst[i].leave_type_id = leave_type_.leave_type_id;
                        }
                    }
                    else
                    {
                        // component_name = true;
                        _invalid = true;
                        errormsg = objlst[i].leave_type_name + "Leave Type Cannot be Blank";
                        msg.Append("" + objlst[i].leave_type_name + "Leave Type Cannot be Blank<br/>");
                    }

                    //if (objlst[i].credit == 0 && objlst[i].dredit == 0)
                    //{
                    //    _invalid = true;
                    //    msg.Append("" + objlst[i].emp_name_code + ", Credit and Debit both cannot be 0 <br/>");
                    //}
                    //else
                    //{
                    if (objlst[i].credit > 365 || objlst[i].dredit > 365)
                    {
                        _invalid = true;
                        errormsg = objlst[i].emp_name_code + ", Credit or Debit cannot be Greater than 365";
                        msg.Append("" + objlst[i].emp_name_code + ", Credit or Debit cannot be Greater than 365");
                    }
                    //}


                    if (_invalid)
                    {
                        // find missing detail
                        LeaveLedgerModell objmissinginput = new LeaveLedgerModell();
                        objmissinginput.emp_name_code = objlst[i].emp_name_code;
                        objmissinginput.leave_type_name = objlst[i].leave_type_name;
                        objmissinginput.credit = objlst[i].credit;
                        objmissinginput.dredit = objlst[i].dredit;
                        objmissinginput.remarks = errormsg;// objlst[i].remarks;
                        missingleavelst.Add(objmissinginput);
                        //msg.Append("Details are missing in Excel </br>");
                    }
                    else
                    {
                        //find duplicate detail
                        bool _checkduplicate = adddbleavelst.Any(x => x.e_id == objlst[i].e_id && x.leave_type_id == objlst[i].leave_type_id);


                        if (_checkduplicate) // Add in this list if any one field detail is found duplicate in list
                        {
                            LeaveLedgerModell objduplist = new LeaveLedgerModell();

                            objduplist.emp_name_code = objlst[i].emp_name_code;
                            objduplist.leave_type_name = objlst[i].leave_type_name;
                            objduplist.credit = objlst[i].credit;
                            objduplist.dredit = objlst[i].dredit;
                            objduplist.remarks = objlst[i].remarks;

                            duplicateleavelst.Add(objduplist);
                        }
                        else
                        {
                            if (objlst[i].credit != 0)
                            {
                                LeaveLedgerModell objdblist = new LeaveLedgerModell();
                                objdblist.e_id = objlst[i].e_id;
                                objdblist.leave_type_id = objlst[i].leave_type_id;
                                objdblist.transaction_type = 5; // Mannaul Add
                                objdblist.credit = objlst[i].credit;
                                objdblist.dredit = 0;
                                objdblist.remarks = objlst[i].remarks;
                                objdblist.created_by = objlst[i].created_by;
                                objdblist.company_id = objlst[i].company_id;

                                adddbleavelst.Add(objdblist);
                            }

                            if (objlst[i].dredit != 0)
                            {
                                LeaveLedgerModell objdblist = new LeaveLedgerModell();
                                objdblist.e_id = objlst[i].e_id;
                                objdblist.leave_type_id = objlst[i].leave_type_id;
                                objdblist.transaction_type = 6; // Mannaul Delete
                                objdblist.credit = 0;
                                objdblist.dredit = objlst[i].dredit;
                                objdblist.remarks = objlst[i].remarks;
                                objdblist.created_by = objlst[i].created_by;
                                objdblist.company_id = objlst[i].company_id;

                                adddbleavelst.Add(objdblist);
                            }

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                msg.Append(ex.Message);
            }
            return new LeaveLedgerList { missingleavelst = missingleavelst, duplicateleavelst = duplicateleavelst, adddbleavelst = adddbleavelst, Message_ = msg.ToString() };
#endif
        }

        [Route("GetLeaveLedgerByCLD_old")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveLedeger))]
        public IActionResult GetLeaveLedgerByCLD_old(LeaveLedgerMdl obj_leave) // Get all Leaves
        {
            throw new NotImplementedException();
#if false
            List<LeaveLedgerModell> emp_leave_list = new List<LeaveLedgerModell>();
            try
            {

                ResponseMsg objresponse = new ResponseMsg();

                clsLeaveCredit objcls_leave = new clsLeaveCredit(_context, _AC, _config, _clsCurrentUser);

                var data = objcls_leave.Get_Leave_Ledger();

                //int[] id_s = Array.ConvertAll(obj_leave.emp_ids.Split(','), int.Parse);


                data = data.Where(a => obj_leave.emp_ids.Contains(a.e_id) && a.year == obj_leave.year).ToList();

                if (obj_leave.company_id > 0 && obj_leave.department_id == 0 && obj_leave.location_id == 0)
                {
                    emp_leave_list = data.Where(x => x.company_id == obj_leave.company_id && obj_leave.emp_ids.Contains(x.e_id)).Select(p => new LeaveLedgerModell
                    {
                        e_id = p.e_id,
                        emp_name = p.emp_name,
                        emp_code = p.emp_code,
                        leave_type_id = p.leave_type_id,
                        dept_name = p.dept_name,
                        loc_name = p.loc_name,
                        leave_type_name = p.leave_type_name,
                        credit = p.credit,
                        dredit = p.dredit,
                        balance = p.balance,
                        company_id = p.company_id,
                    }).ToList();
                }
                else if (obj_leave.company_id > 0 && obj_leave.department_id > 0 && obj_leave.location_id == 0)
                {
                    emp_leave_list = data.Where(x => x.company_id == obj_leave.company_id && x.dept == obj_leave.department_id && obj_leave.emp_ids.Contains(x.e_id)).Select(p => new LeaveLedgerModell
                    {
                        e_id = p.e_id,
                        emp_name = p.emp_name,
                        emp_code = p.emp_code,
                        leave_type_id = p.leave_type_id,
                        dept_name = p.dept_name,
                        loc_name = p.loc_name,
                        leave_type_name = p.leave_type_name,
                        credit = p.credit,
                        dredit = p.dredit,
                        balance = p.balance,
                        company_id = p.company_id,
                    }).ToList();
                }
                else if (obj_leave.company_id > 0 && obj_leave.department_id == 0 && obj_leave.location_id > 0)
                {
                    emp_leave_list = data.Where(x => x.company_id == obj_leave.company_id && x.location_id == obj_leave.location_id && obj_leave.emp_ids.Contains(x.e_id)).Select(p => new LeaveLedgerModell
                    {
                        e_id = p.e_id,
                        emp_name = p.emp_name,
                        emp_code = p.emp_code,
                        leave_type_id = p.leave_type_id,
                        dept_name = p.dept_name,
                        loc_name = p.loc_name,
                        leave_type_name = p.leave_type_name,
                        credit = p.credit,
                        dredit = p.dredit,
                        balance = p.balance,
                        company_id = p.company_id,
                    }).ToList();
                }
                else if (obj_leave.company_id > 0 && obj_leave.department_id > 0 && obj_leave.location_id > 0)
                {
                    emp_leave_list = data.Where(x => x.company_id == obj_leave.company_id && x.dept == obj_leave.department_id && x.location_id == obj_leave.location_id && obj_leave.emp_ids.Contains(x.e_id)).Select(p => new LeaveLedgerModell
                    {
                        e_id = p.e_id,
                        emp_name = p.emp_name,
                        emp_code = p.emp_code,
                        leave_type_id = p.leave_type_id,
                        dept_name = p.dept_name,
                        loc_name = p.loc_name,
                        leave_type_name = p.leave_type_name,
                        credit = p.credit,
                        dredit = p.dredit,
                        balance = p.balance,
                        company_id = p.company_id,
                    }).ToList();
                }
                else if (obj_leave.company_id == 0 && obj_leave.department_id > 0 && obj_leave.location_id == 0)
                {
                    emp_leave_list = data.Where(x => x.dept == obj_leave.department_id && obj_leave.emp_ids.Contains(x.e_id)).Select(p => new LeaveLedgerModell
                    {
                        e_id = p.e_id,
                        emp_name = p.emp_name,
                        emp_code = p.emp_code,
                        leave_type_id = p.leave_type_id,
                        dept_name = p.dept_name,
                        loc_name = p.loc_name,
                        leave_type_name = p.leave_type_name,
                        credit = p.credit,
                        dredit = p.dredit,
                        balance = p.balance,
                        company_id = p.company_id,
                    }).ToList();
                }
                else if (obj_leave.company_id == 0 && obj_leave.department_id == 0 && obj_leave.location_id > 0)
                {
                    emp_leave_list = data.Where(x => x.location_id == obj_leave.location_id && obj_leave.emp_ids.Contains(x.e_id)).Select(p => new LeaveLedgerModell
                    {
                        e_id = p.e_id,
                        emp_name = p.emp_name,
                        emp_code = p.emp_code,
                        leave_type_id = p.leave_type_id,
                        dept_name = p.dept_name,
                        loc_name = p.loc_name,
                        leave_type_name = p.leave_type_name,
                        credit = p.credit,
                        dredit = p.dredit,
                        balance = p.balance,
                        company_id = p.company_id,
                    }).ToList();
                }
                else if (obj_leave.company_id == 0 && obj_leave.department_id > 0 && obj_leave.location_id > 0)
                {
                    emp_leave_list = data.Where(x => x.dept == obj_leave.department_id && x.location_id == obj_leave.location_id && obj_leave.emp_ids.Contains(x.e_id)).Select(p => new LeaveLedgerModell
                    {
                        e_id = p.e_id,
                        emp_name = p.emp_name,
                        emp_code = p.emp_code,
                        leave_type_id = p.leave_type_id,
                        dept_name = p.dept_name,
                        loc_name = p.loc_name,
                        leave_type_name = p.leave_type_name,
                        credit = p.credit,
                        dredit = p.dredit,
                        balance = p.balance,
                        company_id = p.company_id,
                    }).ToList();
                }
                else
                {
                    emp_leave_list = data.Select(p => new LeaveLedgerModell
                    {
                        e_id = p.e_id,
                        emp_name = p.emp_name,
                        emp_code = p.emp_code,
                        leave_type_id = p.leave_type_id,
                        dept_name = p.dept_name,
                        loc_name = p.loc_name,
                        leave_type_name = p.leave_type_name,
                        credit = p.credit,
                        dredit = p.dredit,
                        balance = p.balance,
                        company_id = p.company_id,
                    }).ToList();
                }

                emp_leave_list.RemoveAll(x => (!obj_leave.emp_ids.Contains(x.e_id) && !_clsCurrentUser.CompanyId.Contains(x.company_id)) || (x.credit == 0 && x.dredit == 0 && x.balance == 0));


            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }

            return Ok(emp_leave_list);
#endif
        }


        [Route("GetLeaveLedgerByCLD")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveLedeger))]
        public IActionResult GetLeaveLedgerByCLD(LeaveLedgerMdl obj_leave) // Get all Leaves
        {
            throw new NotImplementedException();
#if false
            // List<LeaveLedgerModell> emp_leave_list = new List<LeaveLedgerModell>();
            try
            {
                // objmodel.to_date = DateTime.Parse(objmodel.to_date).AddDays(1).ToString();



                var fromdate = Convert.ToDateTime(obj_leave.year.ToString() + "-01-01");
                var todate = Convert.ToDateTime(obj_leave.year.ToString() + "-12-31 23:59:59");

                var Masterdata = (from t1 in _context.tbl_leave_type.Where(a => a.is_active == 1 && a.leave_type_name != "LWP").Select(p => new { p.leave_type_id, p.leave_type_name })
                                  from t2 in _context.tbl_emp_officaial_sec.Where(p => obj_leave.emp_ids.Contains(p.employee_id ?? 0) && p.is_deleted == 0)
                                  .Select(p => new { company_id = p.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(y => y.is_deleted == 0).company_id, EmpId = p.employee_id.Value, p.tbl_employee_id_details.emp_code, p.employee_first_name, p.employee_middle_name, p.employee_last_name, p.department_id, p.location_id })
                                  join d in _context.tbl_department_master on t2.department_id equals d.department_id into t1t2
                                  from _t1t2 in t1t2.DefaultIfEmpty()
                                  join l in _context.tbl_location_master on t2.location_id equals l.location_id into t1t3
                                  from _t1t3 in t1t3.DefaultIfEmpty()
                                  where _t1t2.is_active == 1 && _t1t3.is_active == 1 //&& t1.leave_type_id==2
                                  select new
                                  {
                                      department_name = _t1t2.department_name,
                                      location_name = _t1t3.location_name,
                                      company_id = t2.company_id,
                                      leaveTypeId = t1.leave_type_id,
                                      t1.leave_type_name,
                                      t2.EmpId,
                                      t2.emp_code,
                                      t2.employee_first_name,
                                      t2.employee_middle_name,
                                      t2.employee_last_name
                                  }).ToList();

                var CurrentMonthData = (_context.tbl_leave_ledger.Where(p => p.entry_date >= fromdate && p.entry_date <= todate && obj_leave.emp_ids.Contains(p.e_id ?? 0))
                           .GroupBy(g => new { g.e_id, g.leave_type_id }, (k, d) => new { EmpId = k.e_id, leaveTypeId = k.leave_type_id, Credit = d.Sum(a => a.credit), Debit = d.Sum(a => a.dredit) }).
                           Select(p => new { EmpId = p.EmpId ?? 0, leaveTypeId = p.leaveTypeId ?? 0, p.Credit, p.Debit })).ToList();

                var OpeningData = (_context.tbl_leave_ledger.Where(p => p.entry_date < fromdate && obj_leave.emp_ids.Contains(p.e_id ?? 0))
                    .GroupBy(g => new { g.e_id, g.leave_type_id }, (k, d) => new { EmpId = k.e_id, leaveTypeId = k.leave_type_id, credit = d.Sum(a => a.credit), dredit = d.Sum(a => a.dredit) })
                    .Select(p => new
                    {
                        EmpId = p.EmpId ?? 0,
                        leaveTypeId = p.leaveTypeId ?? 0,
                        OpeningBalance = fromdate.Month == 1 && p.leaveTypeId != 2 ? 0 : p.credit - p.dredit
                    })).ToList();

                var Data = (from t1 in Masterdata
                            join t2 in OpeningData on new { t1.leaveTypeId, t1.EmpId } equals new { t2.leaveTypeId, t2.EmpId } into t1t2
                            from _t1t2 in t1t2.DefaultIfEmpty()
                            join t3 in CurrentMonthData on new { t1.leaveTypeId, t1.EmpId } equals new { t3.leaveTypeId, t3.EmpId } into t1t3
                            from _t1t3 in t1t3.DefaultIfEmpty()
                            select new LeaveLedgerModell
                            {
                                e_id = t1.EmpId,
                                emp_name = $"{t1.employee_first_name} {t1.employee_middle_name ?? string.Empty} {t1.employee_last_name ?? string.Empty}",
                                emp_code = t1.emp_code,
                                leave_type_id = t1.leaveTypeId,
                                leave_type_name = t1.leave_type_name,
                                credit = (_t1t3 == null ? 0 : _t1t3.Credit),
                                dredit = (_t1t3 == null ? 0 : _t1t3.Debit),
                                balance = (_t1t2 == null ? 0 : t1.leaveTypeId == 2 ? _t1t2.OpeningBalance : 0) + (_t1t3 == null ? 0 : _t1t3.Credit) - (_t1t3 == null ? 0 : _t1t3.Debit),
                                company_id = Convert.ToInt32(t1.company_id),
                                department_name = t1.department_name,
                                location_name = t1.location_name

                                // OpeningBalance = (_t1t2 == null ? 0 : _t1t2.OpeningBalance),

                            }).OrderBy(x => x.emp_code).ToList();

                // Data.RemoveAll(x => (!obj_leave.emp_ids.Contains(x.e_id) && !_clsCurrentUser.CompanyId.Contains(x.company_id)) || (x.credit == 0 && x.dredit == 0 && x.balance == 0));
                //  emp_leave_list.Add(Data);
                return Ok(Data);


                //ResponseMsg objresponse = new ResponseMsg();

                //clsLeaveCredit objcls_leave = new clsLeaveCredit(_context, _AC, _config, _clsCurrentUser);

                //var data = objcls_leave.Get_Leave_Ledger();

                ////int[] id_s = Array.ConvertAll(obj_leave.emp_ids.Split(','), int.Parse);


                //data = data.Where(a => obj_leave.emp_ids.Contains(a.e_id) && a.year == obj_leave.year).ToList();

                //if (obj_leave.company_id > 0 && obj_leave.department_id == 0 && obj_leave.location_id == 0)
                //{
                //    emp_leave_list = data.Where(x => x.company_id == obj_leave.company_id && obj_leave.emp_ids.Contains(x.e_id)).Select(p => new LeaveLedgerModell
                //    {
                //        e_id = p.e_id,
                //        emp_name = p.emp_name,
                //        emp_code = p.emp_code,sa
                //        leave_type_id = p.leave_type_id,
                //        dept_name = p.dept_name,
                //        loc_name = p.loc_name,
                //        leave_type_name = p.leave_type_name,
                //        credit = p.credit,
                //        dredit = p.dredit,
                //        balance = p.balance,
                //        company_id = p.company_id,
                //    }).ToList();
                //}
                //else if (obj_leave.company_id > 0 && obj_leave.department_id > 0 && obj_leave.location_id == 0)
                //{
                //    emp_leave_list = data.Where(x => x.company_id == obj_leave.company_id && x.dept == obj_leave.department_id && obj_leave.emp_ids.Contains(x.e_id)).Select(p => new LeaveLedgerModell
                //    {
                //        e_id = p.e_id,
                //        emp_name = p.emp_name,
                //        emp_code = p.emp_code,
                //        leave_type_id = p.leave_type_id,
                //        dept_name = p.dept_name,
                //        loc_name = p.loc_name,
                //        leave_type_name = p.leave_type_name,
                //        credit = p.credit,
                //        dredit = p.dredit,
                //        balance = p.balance,
                //        company_id = p.company_id,
                //    }).ToList();
                //}
                //else if (obj_leave.company_id > 0 && obj_leave.department_id == 0 && obj_leave.location_id > 0)
                //{
                //    emp_leave_list = data.Where(x => x.company_id == obj_leave.company_id && x.location_id == obj_leave.location_id && obj_leave.emp_ids.Contains(x.e_id)).Select(p => new LeaveLedgerModell
                //    {
                //        e_id = p.e_id,
                //        emp_name = p.emp_name,
                //        emp_code = p.emp_code,
                //        leave_type_id = p.leave_type_id,
                //        dept_name = p.dept_name,
                //        loc_name = p.loc_name,
                //        leave_type_name = p.leave_type_name,
                //        credit = p.credit,
                //        dredit = p.dredit,
                //        balance = p.balance,
                //        company_id = p.company_id,
                //    }).ToList();
                //}
                //else if (obj_leave.company_id > 0 && obj_leave.department_id > 0 && obj_leave.location_id > 0)
                //{
                //    emp_leave_list = data.Where(x => x.company_id == obj_leave.company_id && x.dept == obj_leave.department_id && x.location_id == obj_leave.location_id && obj_leave.emp_ids.Contains(x.e_id)).Select(p => new LeaveLedgerModell
                //    {
                //        e_id = p.e_id,
                //        emp_name = p.emp_name,
                //        emp_code = p.emp_code,
                //        leave_type_id = p.leave_type_id,
                //        dept_name = p.dept_name,
                //        loc_name = p.loc_name,
                //        leave_type_name = p.leave_type_name,
                //        credit = p.credit,
                //        dredit = p.dredit,
                //        balance = p.balance,
                //        company_id = p.company_id,
                //    }).ToList();
                //}
                //else if (obj_leave.company_id == 0 && obj_leave.department_id > 0 && obj_leave.location_id == 0)
                //{
                //    emp_leave_list = data.Where(x => x.dept == obj_leave.department_id && obj_leave.emp_ids.Contains(x.e_id)).Select(p => new LeaveLedgerModell
                //    {
                //        e_id = p.e_id,
                //        emp_name = p.emp_name,
                //        emp_code = p.emp_code,
                //        leave_type_id = p.leave_type_id,
                //        dept_name = p.dept_name,
                //        loc_name = p.loc_name,
                //        leave_type_name = p.leave_type_name,
                //        credit = p.credit,
                //        dredit = p.dredit,
                //        balance = p.balance,
                //        company_id = p.company_id,
                //    }).ToList();
                //}
                //else if (obj_leave.company_id == 0 && obj_leave.department_id == 0 && obj_leave.location_id > 0)
                //{
                //    emp_leave_list = data.Where(x => x.location_id == obj_leave.location_id && obj_leave.emp_ids.Contains(x.e_id)).Select(p => new LeaveLedgerModell
                //    {
                //        e_id = p.e_id,
                //        emp_name = p.emp_name,
                //        emp_code = p.emp_code,
                //        leave_type_id = p.leave_type_id,
                //        dept_name = p.dept_name,
                //        loc_name = p.loc_name,
                //        leave_type_name = p.leave_type_name,
                //        credit = p.credit,
                //        dredit = p.dredit,
                //        balance = p.balance,
                //        company_id = p.company_id,
                //    }).ToList();
                //}
                //else if (obj_leave.company_id == 0 && obj_leave.department_id > 0 && obj_leave.location_id > 0)
                //{
                //    emp_leave_list = data.Where(x => x.dept == obj_leave.department_id && x.location_id == obj_leave.location_id && obj_leave.emp_ids.Contains(x.e_id)).Select(p => new LeaveLedgerModell
                //    {
                //        e_id = p.e_id,
                //        emp_name = p.emp_name,
                //        emp_code = p.emp_code,
                //        leave_type_id = p.leave_type_id,
                //        dept_name = p.dept_name,
                //        loc_name = p.loc_name,
                //        leave_type_name = p.leave_type_name,
                //        credit = p.credit,
                //        dredit = p.dredit,
                //        balance = p.balance,
                //        company_id = p.company_id,
                //    }).ToList();
                //}
                //else
                //{
                //    emp_leave_list = data.Select(p => new LeaveLedgerModell
                //    {
                //        e_id = p.e_id,
                //        emp_name = p.emp_name,
                //        emp_code = p.emp_code,
                //        leave_type_id = p.leave_type_id,
                //        dept_name = p.dept_name,
                //        loc_name = p.loc_name,
                //        leave_type_name = p.leave_type_name,
                //        credit = p.credit,
                //        dredit = p.dredit,
                //        balance = p.balance,
                //        company_id = p.company_id,
                //    }).ToList();
                //}

                //emp_leave_list.RemoveAll(x => (!obj_leave.emp_ids.Contains(x.e_id) && !_clsCurrentUser.CompanyId.Contains(x.company_id)) || (x.credit == 0 && x.dredit == 0 && x.balance == 0));


            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }

            // return Ok(emp_leave_list);
#endif
        }


        [Route("GetLeaveLedgerByCompID/{company_id}/{leave_type}/{emp_id}/{year}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveLedeger))]
        public IActionResult GetLeaveLedgerByCompID([FromRoute] int company_id, int leave_type, int emp_id, int year)
        {
            throw new NotImplementedException();
#if false

            // List<LeaveLedgerModell> emp_leave_list = new List<LeaveLedgerModell>();
            try
            {
                // objmodel.to_date = DateTime.Parse(objmodel.to_date).AddDays(1).ToString();

                var tomonth = 0;
                // objmodel.to_date = DateTime.Parse(objmodel.to_date).AddDays(1).ToString();
                var todate = new DateTime();
                var fromdate = Convert.ToDateTime(year.ToString() + "-01-01");
                if (year == DateTime.Now.Year) // check if selected year is current year if yes then month will be current month for condition in todate
                {
                    DateTime now = DateTime.Now;
                    var startDate = new DateTime(now.Year, now.Month, 1);
                    var today = startDate.AddMonths(1).AddDays(-1).Day;
                    todate = Convert.ToDateTime(year.ToString() + "-" + now.Month + "-" + today + " 23:59:59");
                    tomonth = todate.Month;
                }
                else
                {
                    todate = Convert.ToDateTime(year.ToString() + "-12-31 23:59:59");
                    tomonth = todate.Month;
                }

                //var todate = new DateTime();
                //var fromdate = Convert.ToDateTime(year.ToString() + "-01-01");
                //if (year == DateTime.Now.Year)
                //{
                //    DateTime now = DateTime.Now;
                //    var startDate = new DateTime(now.Year, now.Month, 1);
                //    todate = startDate.AddMonths(1).AddDays(-1);
                //}
                //else
                //{
                //    todate = Convert.ToDateTime(year.ToString() + "-12-31 23:59:59");
                //}



                var Masterdata = (from t1 in _context.tbl_leave_type.Where(a => a.is_active == 1 && a.leave_type_name != "LWP").Select(p => new { p.leave_type_id, p.leave_type_name })
                                  from t2 in _context.tbl_emp_officaial_sec.Where(p => p.employee_id == emp_id && p.is_deleted == 0)
                                  .Select(p => new { company_id = p.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(y => y.is_deleted == 0).company_id, EmpId = p.employee_id.Value, p.tbl_employee_id_details.emp_code, p.employee_first_name, p.employee_middle_name, p.employee_last_name, p.department_id, p.location_id })
                                  join d in _context.tbl_department_master on t2.department_id equals d.department_id into t1t2
                                  from _t1t2 in t1t2.DefaultIfEmpty()
                                  join l in _context.tbl_location_master on t2.location_id equals l.location_id into t1t3
                                  from _t1t3 in t1t3.DefaultIfEmpty()
                                  where _t1t2.is_active == 1 && _t1t3.is_active == 1 && t1.leave_type_id == leave_type
                                  && t2.company_id == company_id
                                  select new
                                  {
                                      department_name = _t1t2.department_name,
                                      location_name = _t1t3.location_name,
                                      company_id = company_id,
                                      leaveTypeId = t1.leave_type_id,
                                      t1.leave_type_name,
                                      t2.EmpId,
                                      t2.emp_code,
                                      t2.employee_first_name,
                                      t2.employee_middle_name,
                                      t2.employee_last_name
                                  }).FirstOrDefault();

                var Returndata = _context.tbl_leave_ledger.Where(p => p.leave_type_id == leave_type && p.entry_date >= fromdate && p.entry_date <= todate && p.e_id == emp_id)
                    .Select(p => new { p.e_id, p.leave_type_id, p.credit, p.dredit, tmonth = p.entry_date.Month, monthyear = Convert.ToDateTime(p.entry_date).ToString("yyyyMM") })
                    .GroupBy(g => new { g.e_id, g.leave_type_id, g.tmonth, g.monthyear }, (k, d) =>
                     new LeaveLedgerModell { monthyear = Convert.ToInt32(k.monthyear), month_numb = k.tmonth, e_id = k.e_id ?? 0, leave_type_id = k.leave_type_id ?? 0, credit = d.Sum(a => a.credit), dredit = d.Sum(a => a.dredit) }).OrderBy(p => p.month_numb).ToList();

                double OpeningBalance = (_context.tbl_leave_ledger.Where(p => p.leave_type_id == leave_type && p.entry_date < fromdate && p.e_id == emp_id)
                    .GroupBy(g => new { g.e_id, g.leave_type_id }, (k, d) => new { EmpId = k.e_id, leaveTypeId = k.leave_type_id, credit = d.Sum(a => a.credit), dredit = d.Sum(a => a.dredit) })
                    .Select(p => new
                    {
                        EmpId = p.EmpId ?? 0,
                        leaveTypeId = p.leaveTypeId ?? 0,
                        OpeningBalance = p.credit - p.dredit,

                    })).FirstOrDefault()?.OpeningBalance ?? 0;

                List<LeaveLedgerModell> finalReturnData = new List<LeaveLedgerModell>();

                for (int i = 1; i <= tomonth; i++)
                {
                    var Data = Returndata.FirstOrDefault(p => p.month_numb == i);


                    if (Data != null)
                    {
                        Data.openingbalance = i == 1 && leave_type != 2 ? 0 : OpeningBalance;
                        Data.balance = i == 1 && leave_type != 2 ? Data.credit - Data.dredit : OpeningBalance + Data.credit - Data.dredit;
                        Data.transactionmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Data.month_numb);
                        Data.company_id = company_id;
                        OpeningBalance = Data.balance;
                        finalReturnData.Add(Data);
                    }
                    else
                    {
                        finalReturnData.Add(new LeaveLedgerModell()
                        {
                            monthyear = Convert.ToInt32(string.Concat(year, i.ToString("d2"))),
                            transactionmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i),
                            month_numb = i,
                            e_id = Masterdata?.EmpId ?? 0,
                            leave_type_id = leave_type,
                            credit = 0,
                            dredit = 0,
                            openingbalance = i == 1 && leave_type != 2 ? 0 : OpeningBalance,
                            balance = i == 1 && leave_type != 2 ? 0 : OpeningBalance,
                            company_id = company_id,
                        });
                    }
                }

                if (finalReturnData.Count > 0)
                {
                    for (int i = 1; i < finalReturnData.Count; i++)
                    {
                        finalReturnData[i].emp_name = $"{Masterdata.employee_first_name} {Masterdata.employee_middle_name ?? string.Empty} {Masterdata.employee_last_name ?? string.Empty}";
                        finalReturnData[i].e_id = Masterdata.EmpId;
                        finalReturnData[i].emp_code = Masterdata.emp_code;
                        finalReturnData[i].company_id = company_id;
                        finalReturnData[i].leave_type_id = Masterdata.leaveTypeId;
                        finalReturnData[i].leave_type_name = Masterdata.leave_type_name;
                        finalReturnData[i].department_name = Masterdata.department_name;
                        finalReturnData[i].location_name = Masterdata.location_name;
                        finalReturnData[i].monthyear = finalReturnData[i].monthyear;
                        finalReturnData[i].transactionmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(finalReturnData[i].month_numb);

                    }

                    return Ok(finalReturnData);
                }

                return Ok(1);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }

            // return Ok(emp_leave_list);
#endif
        }

        [Route("GetLeaveLedgerByCompID2121/{company_id}/{leave_type}/{emp_id}/{year}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveLedeger))]
        public IActionResult GetLeaveLedgerByCompID2121([FromRoute] int company_id, int leave_type, int emp_id, int year)
        {
            throw new NotImplementedException();
#if false
            // List<LeaveLedgerModell> emp_leave_list = new List<LeaveLedgerModell>();
            try
            {
                var tomonth = 0;
                // objmodel.to_date = DateTime.Parse(objmodel.to_date).AddDays(1).ToString();
                var todate = new DateTime();
                var fromdate = Convert.ToDateTime(year.ToString() + "-01-01");
                if (year == DateTime.Now.Year) // check if selected year is current year if yes then month will be current month for condition in todate
                {
                    DateTime now = DateTime.Now;
                    var startDate = new DateTime(now.Year, now.Month, 1);
                    todate = startDate.AddMonths(1).AddDays(-1);
                    tomonth = todate.Month;
                }
                else
                {
                    todate = Convert.ToDateTime(year.ToString() + "-12-31 23:59:59");
                    tomonth = todate.Month;
                }
                var Masterdata = (from t1 in _context.tbl_leave_type.Where(a => a.is_active == 1 && a.leave_type_name != "LWP").Select(p => new { p.leave_type_id, p.leave_type_name })
                                  from t2 in _context.tbl_emp_officaial_sec.Where(p => p.employee_id == emp_id && p.is_deleted == 0)
                                  .Select(p => new { company_id = p.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(y => y.is_deleted == 0).company_id, EmpId = p.employee_id.Value, p.tbl_employee_id_details.emp_code, p.employee_first_name, p.employee_middle_name, p.employee_last_name, p.department_id, p.location_id })
                                  join d in _context.tbl_department_master on t2.department_id equals d.department_id into t1t2
                                  from _t1t2 in t1t2.DefaultIfEmpty()
                                  join l in _context.tbl_location_master on t2.location_id equals l.location_id into t1t3
                                  from _t1t3 in t1t3.DefaultIfEmpty()
                                  where _t1t2.is_active == 1 && _t1t3.is_active == 1 && t1.leave_type_id == leave_type
                                  && t2.company_id == company_id
                                  select new
                                  {
                                      department_name = _t1t2.department_name,
                                      location_name = _t1t3.location_name,
                                      company_id = t2.company_id,
                                      leaveTypeId = t1.leave_type_id,
                                      t1.leave_type_name,
                                      t2.EmpId,
                                      t2.emp_code,
                                      t2.employee_first_name,
                                      t2.employee_middle_name,
                                      t2.employee_last_name
                                  }).FirstOrDefault();

                List<LeaveLedgerMonthwise> obj_leave_ldgr = new List<LeaveLedgerMonthwise>();

                if (leave_type == 2)
                {
                    for (int cm = 1; cm <= tomonth; cm++) // set list from jan to current month if year is current year otherwise jan to dec
                    {
                        LeaveLedgerMonthwise objlvr = new LeaveLedgerMonthwise();

                        objlvr.emp_name = $"{Masterdata.employee_first_name} {Masterdata.employee_middle_name ?? string.Empty} {Masterdata.employee_last_name ?? string.Empty}";
                        objlvr.e_id = Masterdata.EmpId;
                        objlvr.emp_code = Masterdata.emp_code;
                        objlvr.company_id = Masterdata.company_id ?? 0;
                        objlvr.leave_type_id = Masterdata.leaveTypeId;
                        objlvr.leave_type_name = Masterdata.leave_type_name;
                        objlvr.department_name = Masterdata.department_name;
                        objlvr.location_name = Masterdata.location_name;

                        var setmnth = cm.ToString().Length == 1 ? "0" + cm.ToString() : cm.ToString();
                        var frmdt = Convert.ToDateTime(year.ToString() + "-" + setmnth + "-01");
                        var strtDate = new DateTime(frmdt.Year, cm, 1);
                        var todt = strtDate.AddMonths(1).AddDays(-1); // get last date of month

                        var Returndata = _context.tbl_leave_ledger.Where(p => p.leave_type_id == leave_type && p.entry_date >= frmdt && p.entry_date < todt && p.e_id == emp_id)
                         .Select(p => new { p.e_id, p.leave_type_id, p.credit, p.dredit, tmonth = p.entry_date.Month, monthyear = Convert.ToDateTime(p.entry_date).ToString("yyyyMM") })
                         .GroupBy(g => new { g.e_id, g.leave_type_id, g.tmonth, g.monthyear }, (k, d) =>
                          new LeaveLedgerModell { monthyear = Convert.ToInt32(k.monthyear), month_numb = k.tmonth, e_id = k.e_id ?? 0, leave_type_id = k.leave_type_id ?? 0, credit = d.Sum(a => a.credit), dredit = d.Sum(a => a.dredit) }).OrderBy(p => p.month_numb).ToList();

                        var OpeningData = (_context.tbl_leave_ledger.Where(p => p.leave_type_id == leave_type && p.entry_date < frmdt && p.e_id == emp_id)
                       .GroupBy(g => new { g.e_id, g.leave_type_id }, (k, d) => new { EmpId = k.e_id, leaveTypeId = k.leave_type_id, credit = d.Sum(a => a.credit), dredit = d.Sum(a => a.dredit) })
                       .Select(p => new
                       {
                           EmpId = p.EmpId ?? 0,
                           leaveTypeId = p.leaveTypeId ?? 0,
                           OpeningBalance = p.credit - p.dredit,

                       })).FirstOrDefault();

                        if (Returndata.Count > 0)
                        {
                            if (OpeningData != null)
                            {
                                objlvr.openingbalance = OpeningData.OpeningBalance;
                                objlvr.balance = OpeningData.OpeningBalance + Returndata[0].credit - Returndata[0].dredit;
                                objlvr.credit = Returndata[0].credit;
                                objlvr.dredit = Returndata[0].dredit;
                                objlvr.monthyear = Returndata[0].monthyear;
                                objlvr.transactionmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Returndata[0].month_numb);
                            }
                            else
                            {
                                objlvr.openingbalance = 0;
                                objlvr.balance = Returndata[0].credit - Returndata[0].dredit;
                                objlvr.credit = Returndata[0].credit;
                                objlvr.dredit = Returndata[0].dredit;
                                objlvr.monthyear = Convert.ToInt32(year.ToString() + setmnth);
                                objlvr.transactionmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(setmnth));
                            }
                        }
                        else
                        {
                            if (OpeningData != null)
                            {
                                objlvr.openingbalance = OpeningData.OpeningBalance;
                                objlvr.balance = OpeningData.OpeningBalance;
                                objlvr.credit = 0;
                                objlvr.dredit = 0;
                                objlvr.monthyear = Convert.ToInt32(year.ToString() + setmnth);
                                objlvr.transactionmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(setmnth));
                            }
                            else
                            {
                                objlvr.openingbalance = 0;
                                objlvr.balance = 0;
                                objlvr.credit = 0;
                                objlvr.dredit = 0;
                                objlvr.monthyear = Convert.ToInt32(year.ToString() + setmnth);
                                objlvr.transactionmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(setmnth));
                            }

                        }
                        obj_leave_ldgr.Add(objlvr);
                    }

                    return Ok(obj_leave_ldgr);
                }
                else
                {
                    var Returndata = _context.tbl_leave_ledger.Where(p => p.leave_type_id == leave_type && p.entry_date >= fromdate && p.entry_date < todate && p.e_id == emp_id)
                   .Select(p => new { p.e_id, p.leave_type_id, p.credit, p.dredit, tmonth = p.entry_date.Month, monthyear = Convert.ToDateTime(p.entry_date).ToString("yyyyMM") })
                   .GroupBy(g => new { g.e_id, g.leave_type_id, g.tmonth, g.monthyear }, (k, d) =>
                    new LeaveLedgerModell { monthyear = Convert.ToInt32(k.monthyear), month_numb = k.tmonth, e_id = k.e_id ?? 0, leave_type_id = k.leave_type_id ?? 0, credit = d.Sum(a => a.credit), dredit = d.Sum(a => a.dredit) }).OrderBy(p => p.month_numb).ToList();

                    if (Returndata.Count > 0)
                    {
                        var OpeningData = (_context.tbl_leave_ledger.Where(p => p.leave_type_id == leave_type && p.entry_date < fromdate && p.e_id == emp_id)
                        .GroupBy(g => new { g.e_id, g.leave_type_id }, (k, d) => new { EmpId = k.e_id, leaveTypeId = k.leave_type_id, credit = d.Sum(a => a.credit), dredit = d.Sum(a => a.dredit) })
                        .Select(p => new
                        {
                            EmpId = p.EmpId ?? 0,
                            leaveTypeId = p.leaveTypeId ?? 0,
                            OpeningBalance = p.credit - p.dredit,

                        })).FirstOrDefault();
                        if (OpeningData != null && leave_type == 2)
                        {
                            Returndata[0].openingbalance = OpeningData.OpeningBalance;
                            Returndata[0].balance = OpeningData.OpeningBalance + Returndata[0].credit - Returndata[0].dredit;
                            Returndata[0].emp_name = $"{Masterdata.employee_first_name} {Masterdata.employee_middle_name ?? string.Empty} {Masterdata.employee_last_name ?? string.Empty}";
                            Returndata[0].e_id = Masterdata.EmpId;
                            Returndata[0].emp_code = Masterdata.emp_code;
                            Returndata[0].company_id = Masterdata.company_id ?? 0;
                            Returndata[0].leave_type_id = Masterdata.leaveTypeId;
                            Returndata[0].leave_type_name = Masterdata.leave_type_name;
                            Returndata[0].department_name = Masterdata.department_name;
                            Returndata[0].location_name = Masterdata.location_name;
                            Returndata[0].monthyear = Returndata[0].monthyear;
                            Returndata[0].transactionmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Returndata[0].month_numb);
                        }
                        else
                        {
                            Returndata[0].openingbalance = 0;
                            Returndata[0].balance = Returndata[0].credit - Returndata[0].dredit;
                            Returndata[0].emp_name = $"{Masterdata.employee_first_name} {Masterdata.employee_middle_name ?? string.Empty} {Masterdata.employee_last_name ?? string.Empty}";
                            Returndata[0].e_id = Masterdata.EmpId;
                            Returndata[0].emp_code = Masterdata.emp_code;
                            Returndata[0].company_id = Masterdata.company_id ?? 0;
                            Returndata[0].leave_type_id = Masterdata.leaveTypeId;
                            Returndata[0].leave_type_name = Masterdata.leave_type_name;
                            Returndata[0].department_name = Masterdata.department_name;
                            Returndata[0].location_name = Masterdata.location_name;
                            Returndata[0].monthyear = Returndata[0].monthyear;
                            Returndata[0].transactionmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Returndata[0].month_numb);
                        }

                    }

                    if (Returndata.Count > 0)
                    {
                        for (int i = 1; i < Returndata.Count; i++)
                        {
                            Returndata[i].openingbalance = Returndata[i - 1].balance;
                            Returndata[i].balance = Returndata[i].openingbalance + Returndata[i].credit - Returndata[i].dredit;
                            Returndata[i].emp_name = $"{Masterdata.employee_first_name} {Masterdata.employee_middle_name ?? string.Empty} {Masterdata.employee_last_name ?? string.Empty}";
                            Returndata[i].e_id = Masterdata.EmpId;
                            Returndata[i].emp_code = Masterdata.emp_code;
                            Returndata[i].company_id = Masterdata.company_id ?? 0;
                            Returndata[i].leave_type_id = Masterdata.leaveTypeId;
                            Returndata[i].leave_type_name = Masterdata.leave_type_name;
                            Returndata[i].department_name = Masterdata.department_name;
                            Returndata[i].location_name = Masterdata.location_name;
                            Returndata[i].monthyear = Returndata[i].monthyear;
                            Returndata[i].transactionmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Returndata[i].month_numb);

                        }

                        return Ok(Returndata);
                    }
                }
                return Ok(1);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }

            // return Ok(emp_leave_list);
#endif
        }



        [Route("GetLeaveLedgerByCompID29mar21/{company_id}/{leave_type}/{emp_id}/{year}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveLedeger))]
        public IActionResult GetLeaveLedgerByCompID_29mar21([FromRoute] int company_id, int leave_type, int emp_id, int year)
        {
            throw new NotImplementedException();
#if false
            // List<LeaveLedgerModell> emp_leave_list = new List<LeaveLedgerModell>();
            try
            {
                // objmodel.to_date = DateTime.Parse(objmodel.to_date).AddDays(1).ToString();
                var todate = new DateTime();
                var fromdate = Convert.ToDateTime(year.ToString() + "-01-01");
                if (year == DateTime.Now.Year)
                {
                    DateTime now = DateTime.Now;
                    var startDate = new DateTime(now.Year, now.Month, 1);
                    todate = startDate.AddMonths(1).AddDays(-1);
                }
                else
                {
                    todate = Convert.ToDateTime(year.ToString() + "-12-31 23:59:59");
                }
                var Masterdata = (from t1 in _context.tbl_leave_type.Where(a => a.is_active == 1 && a.leave_type_name != "LWP").Select(p => new { p.leave_type_id, p.leave_type_name })
                                  from t2 in _context.tbl_emp_officaial_sec.Where(p => p.employee_id == emp_id && p.is_deleted == 0)
                                  .Select(p => new { company_id = p.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(y => y.is_deleted == 0).company_id, EmpId = p.employee_id.Value, p.tbl_employee_id_details.emp_code, p.employee_first_name, p.employee_middle_name, p.employee_last_name, p.department_id, p.location_id })
                                  join d in _context.tbl_department_master on t2.department_id equals d.department_id into t1t2
                                  from _t1t2 in t1t2.DefaultIfEmpty()
                                  join l in _context.tbl_location_master on t2.location_id equals l.location_id into t1t3
                                  from _t1t3 in t1t3.DefaultIfEmpty()
                                  where _t1t2.is_active == 1 && _t1t3.is_active == 1 && t1.leave_type_id == leave_type
                                  && t2.company_id == company_id
                                  select new
                                  {
                                      department_name = _t1t2.department_name,
                                      location_name = _t1t3.location_name,
                                      company_id = t2.company_id,
                                      leaveTypeId = t1.leave_type_id,
                                      t1.leave_type_name,
                                      t2.EmpId,
                                      t2.emp_code,
                                      t2.employee_first_name,
                                      t2.employee_middle_name,
                                      t2.employee_last_name
                                  }).FirstOrDefault();

                var Returndata = _context.tbl_leave_ledger.Where(p => p.leave_type_id == leave_type && p.entry_date >= fromdate && p.entry_date < todate && p.e_id == emp_id)
                    .Select(p => new { p.e_id, p.leave_type_id, p.credit, p.dredit, tmonth = p.entry_date.Month, monthyear = Convert.ToDateTime(p.entry_date).ToString("yyyyMM") })
                    .GroupBy(g => new { g.e_id, g.leave_type_id, g.tmonth, g.monthyear }, (k, d) =>
                     new LeaveLedgerModell { monthyear = Convert.ToInt32(k.monthyear), month_numb = k.tmonth, e_id = k.e_id ?? 0, leave_type_id = k.leave_type_id ?? 0, credit = d.Sum(a => a.credit), dredit = d.Sum(a => a.dredit) }).OrderBy(p => p.month_numb).ToList();

                //var Returndata1 = _context.tbl_leave_ledger.Where(p => p.leave_type_id == leave_type && p.entry_date < fromdate && p.e_id == emp_id)
                //     .Select(p => new { p.e_id, p.leave_type_id, p.credit, p.dredit, tmonth = p.entry_date.Month, monthyear = Convert.ToDateTime(p.entry_date).ToString("yyyyMM") })
                //     .GroupBy(g => new { g.e_id, g.leave_type_id, g.tmonth, g.monthyear }, (k, d) =>
                //      new LeaveLedgerModell { monthyear = Convert.ToInt32(k.monthyear), month_numb = k.tmonth, e_id = k.e_id ?? 0, leave_type_id = k.leave_type_id ?? 0, credit = d.Sum(a => a.credit), dredit = d.Sum(a => a.dredit) }).OrderBy(p => p.month_numb).ToList();

                if (Returndata.Count > 0)
                {
                    var OpeningData = (_context.tbl_leave_ledger.Where(p => p.leave_type_id == leave_type && p.entry_date < fromdate && p.e_id == emp_id)
                    .GroupBy(g => new { g.e_id, g.leave_type_id }, (k, d) => new { EmpId = k.e_id, leaveTypeId = k.leave_type_id, credit = d.Sum(a => a.credit), dredit = d.Sum(a => a.dredit) })
                    .Select(p => new
                    {
                        EmpId = p.EmpId ?? 0,
                        leaveTypeId = p.leaveTypeId ?? 0,
                        OpeningBalance = p.credit - p.dredit,

                    })).FirstOrDefault();
                    if (OpeningData != null && leave_type == 2)
                    {
                        Returndata[0].openingbalance = OpeningData.OpeningBalance;
                        Returndata[0].balance = OpeningData.OpeningBalance + Returndata[0].credit - Returndata[0].dredit;
                        Returndata[0].emp_name = $"{Masterdata.employee_first_name} {Masterdata.employee_middle_name ?? string.Empty} {Masterdata.employee_last_name ?? string.Empty}";
                        Returndata[0].e_id = Masterdata.EmpId;
                        Returndata[0].emp_code = Masterdata.emp_code;
                        Returndata[0].company_id = Masterdata.company_id ?? 0;
                        Returndata[0].leave_type_id = Masterdata.leaveTypeId;
                        Returndata[0].leave_type_name = Masterdata.leave_type_name;
                        Returndata[0].department_name = Masterdata.department_name;
                        Returndata[0].location_name = Masterdata.location_name;
                        Returndata[0].monthyear = Returndata[0].monthyear;
                        Returndata[0].transactionmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Returndata[0].month_numb);
                    }
                    else
                    {
                        Returndata[0].openingbalance = 0;
                        Returndata[0].balance = Returndata[0].credit - Returndata[0].dredit;
                        Returndata[0].emp_name = $"{Masterdata.employee_first_name} {Masterdata.employee_middle_name ?? string.Empty} {Masterdata.employee_last_name ?? string.Empty}";
                        Returndata[0].e_id = Masterdata.EmpId;
                        Returndata[0].emp_code = Masterdata.emp_code;
                        Returndata[0].company_id = Masterdata.company_id ?? 0;
                        Returndata[0].leave_type_id = Masterdata.leaveTypeId;
                        Returndata[0].leave_type_name = Masterdata.leave_type_name;
                        Returndata[0].department_name = Masterdata.department_name;
                        Returndata[0].location_name = Masterdata.location_name;
                        Returndata[0].monthyear = Returndata[0].monthyear;
                        Returndata[0].transactionmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Returndata[0].month_numb);
                    }

                }

                if (Returndata.Count > 0)
                {
                    for (int i = 1; i < Returndata.Count; i++)
                    {
                        Returndata[i].openingbalance = Returndata[i - 1].balance;
                        Returndata[i].balance = Returndata[i].openingbalance + Returndata[i].credit - Returndata[i].dredit;
                        Returndata[i].emp_name = $"{Masterdata.employee_first_name} {Masterdata.employee_middle_name ?? string.Empty} {Masterdata.employee_last_name ?? string.Empty}";
                        Returndata[i].e_id = Masterdata.EmpId;
                        Returndata[i].emp_code = Masterdata.emp_code;
                        Returndata[i].company_id = Masterdata.company_id ?? 0;
                        Returndata[i].leave_type_id = Masterdata.leaveTypeId;
                        Returndata[i].leave_type_name = Masterdata.leave_type_name;
                        Returndata[i].department_name = Masterdata.department_name;
                        Returndata[i].location_name = Masterdata.location_name;
                        Returndata[i].monthyear = Returndata[i].monthyear;
                        Returndata[i].transactionmonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Returndata[i].month_numb);

                    }

                    return Ok(Returndata);
                }

                return Ok(1);
            #region commented
                //var CurrentMonthData1 = (_context.tbl_leave_ledger.Where(p => p.leave_type_id == leave_type && p.entry_date >= fromdate && p.entry_date < todate && p.e_id == emp_id).
                //          Select(p => new { p.e_id, p.leave_type_id, p.credit, p.dredit, tmonth= p.transaction_date.Month })).ToList();


                //var CurrentMonthData = CurrentMonthData1.GroupBy(g => new { g.e_id, g.leave_type_id,g.tmonth }, (k, d) => 
                //new {datemonth=k.tmonth, EmpId = k.e_id, leaveTypeId = k.leave_type_id, Credit = d.Sum(a => a.credit), Debit = d.Sum(a => a.dredit) }).
                //           Select(p => new { EmpId = p.EmpId ?? 0, leaveTypeId = p.leaveTypeId ?? 0, p.Credit, p.Debit,p.datemonth }).ToList();


                //var OpeningDatalist = (_context.tbl_leave_ledger.Where(p => p.leave_type_id == leave_type && p.entry_date < fromdate && p.e_id == emp_id)
                //    .GroupBy(g => new { g.e_id, g.leave_type_id, g.transaction_date }, (k, d) => new { datemonth = k.transaction_date, EmpId = k.e_id, leaveTypeId = k.leave_type_id, credit = d.Sum(a => a.credit), dredit = d.Sum(a => a.dredit) })
                //    .Select(p => new
                //    {
                //        EmpId = p.EmpId ?? 0,
                //        leaveTypeId = p.leaveTypeId ?? 0,
                //        OpeningBalance = p.credit - p.dredit,
                //        p.datemonth
                //    })).ToList();


                //var OpeningData= OpeningDatalist.GroupBy(g => new { g.EmpId, g.leaveTypeId, g.datemonth }, (k, d) => 
                //new { k.EmpId, k.leaveTypeId,k.datemonth, OpeningBalance = d.Sum(a => a.OpeningBalance)})
                //    .Select(p => new
                //    {
                //        p.EmpId,
                //        p.leaveTypeId,
                //        p.OpeningBalance,
                //        p.datemonth
                //    }).ToList();

                //var Data = (from t1 in Masterdata
                //            join t2 in OpeningData on new { t1.leaveTypeId, t1.EmpId } equals new { t2.leaveTypeId, t2.EmpId } into t1t2
                //            from _t1t2 in t1t2.DefaultIfEmpty()
                //            join t3 in CurrentMonthData on new { t1.leaveTypeId, t1.EmpId } equals new { t3.leaveTypeId, t3.EmpId } into t1t3
                //            from _t1t3 in t1t3.DefaultIfEmpty()
                //            select new LeaveLedgerModell
                //            {
                //                e_id = t1.EmpId,
                //                emp_name = $"{t1.employee_first_name} {t1.employee_middle_name ?? string.Empty} {t1.employee_last_name ?? string.Empty}",
                //                emp_code = t1.emp_code,
                //                leave_type_id = t1.leaveTypeId,
                //                leave_type_name = t1.leave_type_name,
                //                credit = (_t1t3 == null ? 0 : _t1t3.Credit),
                //                dredit = (_t1t3 == null ? 0 : _t1t3.Debit),
                //                balance = (_t1t2 == null ? 0 : _t1t2.OpeningBalance) + (_t1t3 == null ? 0 : _t1t3.Credit) - (_t1t3 == null ? 0 : _t1t3.Debit),
                //                company_id = Convert.ToInt32(t1.company_id),
                //                department_name = t1.department_name,
                //                location_name = t1.location_name,
                //                openingbalance = (_t1t2 == null ? 0 : _t1t2.OpeningBalance),
                //                transactionmonth= CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_t1t3.datemonth)

                //            }).ToList();

                // Data.RemoveAll(x => (!obj_leave.emp_ids.Contains(x.e_id) && !_clsCurrentUser.CompanyId.Contains(x.company_id)) || (x.credit == 0 && x.dredit == 0 && x.balance == 0));
                //  emp_leave_list.Add(Data);
                //return Ok(Data);

            #endregion
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }

            // return Ok(emp_leave_list);
#endif
        }

        [Route("GetLeaveDetails_monthwise/{company_id}/{leave_type}/{emp_id}/{monthyear}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveLedeger))]
        public IActionResult GetLeaveDetails_monthwise([FromRoute] int company_id, int leave_type, int emp_id, int monthyear)
        {
            ResponseMsg objresponse = new ResponseMsg();
            //if (!_clsCurrentUser.CompanyId.Contains(company_id))
            //{
            //    objresponse.StatusCode = 1;
            //    objresponse.Message = "Unauthorize Company Access...!!";
            //    return Ok(objresponse);
            //}

            //if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
            //{
            //    objresponse.StatusCode = 1;
            //    objresponse.Message = "Unauthorize Access...!!";
            //    return Ok(objresponse);
            //}
            try
            {
                var setyear = monthyear.ToString().Substring(0, 4);
                var setmonth = monthyear.ToString().Substring(monthyear.ToString().Length - 2);
                var fromdate = Convert.ToDateTime(Convert.ToInt32(setyear) + "-" + Convert.ToInt32(setmonth) + "-01");
                var today = Convert.ToDateTime(fromdate.AddMonths(1).AddDays(-1));
                var todate = Convert.ToDateTime(today.Year.ToString() + "-" + today.Month + "-" + today.Day + " 23:59:59");
                // var todate = Convert.ToDateTime(setyear + "-" + setmonth + "-31 23:59:59");

                var data = _context.tbl_leave_ledger.Where(x => x.entry_date >= fromdate && x.entry_date <= todate && x.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(g => g.is_deleted == 0).company_id == company_id && x.leave_type_id == leave_type && x.e_id == emp_id).Select(p => new
                //var data = _context.tbl_leave_ledger.Where(x => x.monthyear==monthyear && x.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(g => g.is_deleted == 0).company_id == company_id && x.leave_type_id == leave_type && x.e_id == emp_id).Select(p => new
                {
                    p.tbl_leave_type.leave_type_name,
                    p.tbl_leave_info.leave_info_id,
                    approve_date = Convert.ToDateTime(p.entry_date).ToString("dd-MMM-yyyy"),
                    leave_date = Convert.ToDateTime(p.transaction_date).ToString("dd-MMM-yyyy"),
                    p.entry_date,
                    p.transaction_type,
                    transaction_type_name = p.transaction_type == 1 ? "Add by System" : p.transaction_type == 2 ? "Consumed" : p.transaction_type == 3 ? "Expired" : p.transaction_type == 4 ? "In Cash" : p.transaction_type == 5 ? "Manual Add" : p.transaction_type == 6 ? "Manual Delete" : p.transaction_type == 7 ? "Add by System" : p.transaction_type == 100 ? "Previous Leave Credit by System" : "",
                    monthyear,
                    p.leave_addition_type,
                    leave_addition_type_name = p.leave_addition_type == 1 ? "Monthly" : p.leave_addition_type == 2 ? "Quaterly" : p.leave_addition_type == 3 ? "Half Yearly" : p.leave_addition_type == 4 ? "Annualy" : "",
                    p.credit,
                    p.dredit,
                    p.remarks,
                    emp_name_code = string.Format("{0} {1} {2} ({3})",
                         p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                         p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                         p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                         p.tbl_employee_id_details.emp_code)
                }).OrderBy(x => x.approve_date).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("GetLeaveLedgerByCompID_old_d/{company_id}/{leave_type}/{emp_id}/{year}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveLedeger))]
        public IActionResult GetLeaveLedgerByCompID_old_d([FromRoute] int company_id, int leave_type, int emp_id, int year)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }

            if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }
            try
            {
                var fromdate = Convert.ToDateTime(year.ToString() + "-01-01");
                var todate = Convert.ToDateTime(year.ToString() + "-12-31 23:59:59");



                var data = _context.tbl_leave_ledger.Where(x => x.entry_date >= fromdate && x.entry_date < todate && x.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(g => g.is_deleted == 0).company_id == company_id && x.leave_type_id == leave_type && x.e_id == emp_id).Select(p => new
                {
                    p.tbl_leave_type.leave_type_name,
                    p.tbl_leave_info.leave_info_id,
                    p.transaction_date,
                    p.entry_date,
                    p.transaction_type,
                    transaction_type_name = p.transaction_type == 1 ? "Add by System" : p.transaction_type == 2 ? "Consumed" : p.transaction_type == 3 ? "Expired" : p.transaction_type == 4 ? "In Cash" : p.transaction_type == 5 ? "Manual Add" : p.transaction_type == 6 ? "Manual Delete" : p.transaction_type == 7 ? "Deleted by System" : p.transaction_type == 100 ? "Previous Leave Credit by System" : "",
                    p.monthyear,
                    p.leave_addition_type,
                    leave_addition_type_name = p.leave_addition_type == 1 ? "Monthly" : p.leave_addition_type == 2 ? "Quaterly" : p.leave_addition_type == 3 ? "Half Yearly" : p.leave_addition_type == 4 ? "Annualy" : "",
                    p.credit,
                    p.dredit,
                    p.remarks,
                    emp_name_code = string.Format("{0} {1} {2} ({3})",
                         p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                         p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                         p.tbl_employee_id_details.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                         p.tbl_employee_id_details.emp_code)
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


#endregion ** END UPLOAD LEAVE CREDIT,DEBIT 03-06-2020 **

        protected string FunIsApplicationFreezed()
        {
            throw new NotImplementedException();
#if false
            Dictionary<string, string> app_setting_dic = _context.tbl_app_setting.Where(x => x.is_active == 1).ToDictionary(x => x.AppSettingKey.ToString(), y => y.AppSettingValue.ToString());
            string is_attendence_freezed_for_Emp = app_setting_dic.Where(x => x.Key.ToLower() == "attandance_application_freezed_for_emp").Select(y => y.Value).ToString();
            string is_attendence_freezed_for_Admin = app_setting_dic.Where(x => x.Key.ToLower() == "attandance_application_freezed__for_admin").Select(y => y.Value).ToString();
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
#endif
        }


    }
}
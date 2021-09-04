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
    public class apiNoDuesController : ControllerBase
    {
        private readonly IHttpContextAccessor _AC;
        private readonly Context _context;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IConfiguration _config;
        private readonly clsCurrentUser _clsCurrentUser;
        private readonly clsEmployeeDetail _clEmployeeDetail;
        DateTime FromDate;


        public apiNoDuesController(Context context, IConfiguration config, IHostingEnvironment environment, IOptions<AppSettings> appSettings, IHttpContextAccessor AC, clsCurrentUser _clsCurrentUser, clsEmployeeDetail _clEmployeeDetail)
        {
            _context = context;
            _config = config;
            _hostingEnvironment = environment;
            _appSettings = appSettings;
            _AC = AC;
            this._clsCurrentUser = _clsCurrentUser;
            this._clEmployeeDetail = _clEmployeeDetail;
        }

        /////////////////////////////////////////////////////-------------------------------------------------------------------------------------------------------------

        #region Start No Due Particular MasterS
        //Save No Due Particular Master
        [Route("Save_NoDueParticularMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.MachineMaster))]
        public IActionResult Save_NoDueParticularMaster([FromBody] NoDues_particular_master objndpm)
        {
            Response_Msg objResult = new Response_Msg();

            //if (!_clsCurrentUser.CompanyId.Contains(objndpm.company_id))
            //{
            //    objResult.StatusCode = 1;
            //    objResult.Message = "Unauthorize Company Access";
            //    return Ok(objResult);
            //}
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    for (int i = 0; i < objndpm.particularList.Count; i++)
                    {
                        tbl_No_dues_particular_master obj = new tbl_No_dues_particular_master();
                        var exist = _context.tbl_No_dues_particular_master.Where(x => x.department_id == objndpm.department_id && objndpm.particularList[i].Trim() == x.particular_name.ToUpper().Trim().ToString()).FirstOrDefault();
                        //   var exist = _context.tbl_No_dues_particular_master.Where(x => x.department_id == objndpm.department_id && x.company_id == objndpm.company_id && objndpm.particularList[i].Trim() == x.particular_name.ToUpper().Trim().ToString()).FirstOrDefault();

                        if (exist != null)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Particular ( " + objndpm.particularList[i] + " ) already exist...!";
                            return Ok(objResult);
                        }
                        else
                        {
                            //  obj.company_id = objndpm.company_id;
                            obj.department_id = objndpm.department_id;
                            obj.remarks = objndpm.remarks;
                            obj.particular_name = objndpm.particularList[i];
                            obj.created_by = _clsCurrentUser.EmpId;
                            obj.created_date = DateTime.Now;
                            obj.modified_date = DateTime.Now;
                            obj.is_deleted = 0;
                            _context.Entry(obj).State = EntityState.Added;

                        }
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    objResult.StatusCode = 0;
                    objResult.Message = "No Dues Particular Saved Successfully...!";
                    return Ok(objResult);

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    objResult.StatusCode = 0;
                    objResult.Message = ex.Message;
                    return Ok(objResult);
                }
            }
        }


        //Get NoDues Particular Master Data
        [Route("GetNoDuesParticularMasterData")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.MachineMaster))]
        public IActionResult GetNoDuesParticularMasterData()
        {
            try
            {
                ResponseMsg objResult = new ResponseMsg();
                var currUserDept = _context.tbl_emp_officaial_sec.Where(b => b.employee_id == _clsCurrentUser.EmpId && b.is_deleted == 0).FirstOrDefault().department_id;


                var result = new object();

                result = _context.tbl_No_dues_particular_master.Where(x => x.is_deleted == 0).Select(a => new
                {
                    comp_id = a.company_id,
                    dept_id = a.department_id,
                    particular_id = a.pkid_ParticularMaster,
                    dept_Name = a.dept_details.department_name,
                    // comp_name = a.comp_details.company_name,
                    particular_name = a.particular_name,
                    created_date = a.created_date.Date,
                    is_deleted = a.is_deleted,
                    created_by = string.Format("{0} {1} {2}",
                                _context.tbl_emp_officaial_sec.Where(b => b.employee_id == a.created_by && b.is_deleted == 0).FirstOrDefault().employee_first_name,
                                _context.tbl_emp_officaial_sec.Where(b => b.employee_id == a.created_by && b.is_deleted == 0).FirstOrDefault().employee_middle_name, _context.tbl_emp_officaial_sec.Where(b => b.employee_id == a.created_by && b.is_deleted == 0).FirstOrDefault().employee_last_name),
                    remarks = a.remarks
                }).Distinct().ToList();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [Route("DeleteNoDuesParticularMaster/{request_id}")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveApplication))]
        public IActionResult DeleteNoDuesParticularMaster(int request_id)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {
                var exist = _context.tbl_No_dues_particular_master.Where(x => x.pkid_ParticularMaster == Convert.ToInt32(request_id) && x.is_deleted == 0).FirstOrDefault();
                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid request id, please check and try !!";
                    return Ok(objResult);
                }

                if (!_clsCurrentUser.DownlineEmpId.Contains(exist.created_by))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize access or Company access";
                    return Ok(objResult);
                }


                exist.is_deleted = 1;
                exist.modified_by = _clsCurrentUser.EmpId;
                exist.modified_date = DateTime.Now;
                _context.Entry(exist).State = EntityState.Modified;
                _context.SaveChanges();

                objResult.StatusCode = 0;
                objResult.Message = "Particular Master deleted successfully !!";
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        #endregion End Particular Master

        /////////////////////////////////////////////////////---------------------------------------------------------------------------------------------------------------
        #region Start Particular Assignee

        [Route("Save_NoDueParticularResponsible")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.MachineMaster))]
        public IActionResult Save_NoDueParticularResponsible([FromBody] No_dues_particular_responsible objndpr)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(objndpr.company_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access";
                return Ok(objResult);
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    var a = _context.tbl_No_dues_particular_responsible.Where(y => y.department_id == objndpr.department_id && y.company_id == objndpr.company_id).Select(x => x.emp_id).ToList();
                    foreach (var item in a)
                    {
                        _context.tbl_user_role_map.Where(y => y.tbl_user_master.employee_id == item && y.role_id == (int)enmRoleMaster.NoDuesAdmin && y.is_deleted == 0).ToList().ForEach(z =>
                        {
                            z.is_deleted = 1;
                            z.last_modified_by = _clsCurrentUser.EmpId;
                            z.last_modified_date = DateTime.Now;
                        });
                    }

                    _context.tbl_No_dues_particular_responsible.Where(y => y.department_id == objndpr.department_id && y.company_id == objndpr.company_id).ToList().ForEach(x => { x.is_deleted = 1; x.modified_by = _clsCurrentUser.EmpId; x.modified_date = DateTime.Now; });

                    _context.SaveChanges();

                    for (int i = 0; i < objndpr.emp_ids.Count; i++)
                    {
                        tbl_No_dues_particular_responsible obj = new tbl_No_dues_particular_responsible();

                        var exist = _context.tbl_No_dues_particular_responsible.Where(x => x.department_id == objndpr.department_id && x.company_id == objndpr.company_id && Convert.ToInt32(objndpr.emp_ids[i]) == x.emp_id).FirstOrDefault();

                        if (exist != null)
                        {
                            exist.is_deleted = 0;
                            exist.modified_by = _clsCurrentUser.EmpId;
                            exist.modified_date = DateTime.Now;
                            _context.Entry(exist).State = EntityState.Modified;
                        }
                        else
                        {
                            obj.company_id = objndpr.company_id;
                            obj.department_id = objndpr.department_id;
                            obj.emp_id = Convert.ToInt32(objndpr.emp_ids[i]);
                            obj.created_by = _clsCurrentUser.EmpId;
                            obj.created_date = DateTime.Now;
                            obj.modified_date = DateTime.Now;
                            obj.is_deleted = 0;
                            _context.Entry(obj).State = EntityState.Added;
                        }

                        tbl_user_role_map _newrole = new tbl_user_role_map();
                        int userid = _context.tbl_user_master.Where(x => x.is_active == 1 && x.employee_id == Convert.ToInt32(objndpr.emp_ids[i])).Select(x => x.user_id).FirstOrDefault();
                        _newrole = new tbl_user_role_map
                        {
                            role_id = (int)enmRoleMaster.NoDuesAdmin,
                            user_id = userid,
                            is_deleted = 0,
                            created_by = _clsCurrentUser.EmpId,
                            created_date = DateTime.Now
                        };

                        _context.tbl_user_role_map.AddRange(_newrole);
                        _context.SaveChanges();

                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    objResult.StatusCode = 0;
                    objResult.Message = "No Dues Particular Responsible Saved Successfully...!";
                    return Ok(objResult);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    objResult.StatusCode = 0;
                    objResult.Message = ex.Message;
                    return Ok(objResult);
                }
            }
        }


        //Get NoDues Particular Responsible  Data
        [Route("GetNoDuesParticularResponsibleData")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.MachineMaster))]
        public IActionResult GetNoDuesParticularResponsibleData()
        {
            try
            {
                ResponseMsg objResult = new ResponseMsg();

                var currUserDept = _context.tbl_emp_officaial_sec.Where(b => b.employee_id == _clsCurrentUser.EmpId && b.is_deleted == 0).FirstOrDefault().department_id;


                var result = new object();

                result = _context.tbl_No_dues_particular_responsible.Where(x => x.is_deleted == 0 && _clsCurrentUser.Is_SuperAdmin ? true : x.department_id == currUserDept).Select(a => new
                {
                    comp_id = a.company_id,
                    dept_id = a.department_id,
                    part_responsible_id = a.pkid_ParticularResponsible,
                    dept_Name = a.dept_details.department_name,
                    comp_name = a.company_details.company_name,
                    emp_code = a.emp_details.emp_code,
                    created_date = a.created_date.Date,
                    is_deleted = a.is_deleted,
                    emp_name = string.Format("{0} {1} {2}",
                               a.emp_details.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).employee_first_name,
                               a.emp_details.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).employee_middle_name,
                               a.emp_details.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).employee_last_name),
                    created_by = string.Format("{0} {1} {2}",
                                      _context.tbl_emp_officaial_sec.Where(b => b.employee_id == a.created_by && b.is_deleted == 0).FirstOrDefault().employee_first_name,
                                      _context.tbl_emp_officaial_sec.Where(b => b.employee_id == a.created_by && b.is_deleted == 0).FirstOrDefault().employee_middle_name, _context.tbl_emp_officaial_sec.Where(b => b.employee_id == a.created_by && b.is_deleted == 0).FirstOrDefault().employee_last_name),
                }).Distinct().OrderBy(z => z.dept_id).ToList();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("DeleteNoDuesParticularResponsible/{request_id}")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LeaveApplication))]
        public IActionResult DeleteNoDuesParticularResponsible(int request_id)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {
                var exist = _context.tbl_No_dues_particular_responsible.Where(x => x.pkid_ParticularResponsible == Convert.ToInt32(request_id) && x.is_deleted == 0).FirstOrDefault();
                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid request id, please check and try !!";
                    return Ok(objResult);
                }

                if (!_clsCurrentUser.DownlineEmpId.Contains(exist.created_by))
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize access or Company access";
                    return Ok(objResult);
                }


                exist.is_deleted = 1;
                exist.modified_by = _clsCurrentUser.EmpId;
                exist.modified_date = DateTime.Now;
                _context.Entry(exist).State = EntityState.Modified;
                _context.SaveChanges();

                objResult.StatusCode = 0;
                objResult.Message = "Particular Responsible Person deleted successfully !!";
                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        #endregion End Particular Assignee 
        /////////////////////////////////////////////////////-------------------------------------------------------------------------------------------------------------

        [Route("Get_EmpResponsibleForNoDue/{company_id}/{dept_id}")]
        [HttpGet]
        public IActionResult Get_EmpResponsibleForNoDue(int company_id, int dept_id)
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
                    .Select(a => new { a.tbl_employee_id_details.emp_code, a.employee_id, a.employee_first_name, a.employee_middle_name, a.employee_last_name })
                    .Select(j => new
                    {
                        j.employee_id,
                        emp_code = j.emp_code,
                        emp_name = string.Format("{0} {1} {2}", j.employee_first_name, j.employee_middle_name, j.employee_last_name),
                        emp_name_code = string.Format("{0} {1} {2} ({3})", j.employee_first_name, j.employee_middle_name, j.employee_last_name, j.emp_code),
                        is_responsible = _context.tbl_No_dues_particular_responsible.Where(m => m.is_deleted == 0).Any(n => n.emp_id == j.employee_id) ? 1 : 0

                    }).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        #region NO Due Clearence Detail of Employee

        //Get NoDues Clearence Request Of Emps
        [Route("Get_NoDuesClearenceRequestOfEmps")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult Get_NoDuesClearenceRequestOfEmps() // 0 for all employee , 1 for selected emp
        {
            try
            {
                ResponseMsg objResult = new ResponseMsg();

                //if (!_clsCurrentUser.RoleId.Contains(Convert.ToInt32(enmRoleMaster.NoDuesAdmin)))
                //{
                //    return Ok("No Records Found");

                //}
                var currUserDept = _context.tbl_emp_officaial_sec.Where(b => b.employee_id == _clsCurrentUser.EmpId && b.is_deleted == 0).FirstOrDefault().department_id;

                var responsibleEmp = _context.tbl_No_dues_particular_responsible.Select(x => x.emp_id).ToList();

                var result = _context.tbl_emp_separation.Join(_context.tbl_No_dues_emp_particular_Clearence_detail, es => es.sepration_id, ndpc => ndpc.fkid_EmpSaperationId, (es, ndpc) => new { es, ndpc }).Where(y => y.es.is_final_approve == 1 && y.es.is_deleted == 0 && y.es.is_cancel == 0 && ((_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin) ? true : (y.es.final_relieve_dt.Date < DateTime.Now.AddDays(10).Date && y.es.final_relieve_dt.Date >= DateTime.Now.Date.AddDays(-5)) && responsibleEmp.Contains(_clsCurrentUser.EmpId)) && y.ndpc.is_deleted == 0 && y.es.is_deleted == 0).Select(p => new
                {
                    emp_id = p.es.emp_id,
                    sepration_id = p.es.sepration_id,
                    is_deleted = p.es.is_deleted,
                    emp_code = p.ndpc.empSepration_details.emp_mstr.emp_code,
                    emp_name = string.Format("{0} {1} {2}", p.ndpc.empSepration_details.emp_mstr.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).employee_first_name, p.ndpc.empSepration_details.emp_mstr.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).employee_middle_name,
                             p.ndpc.empSepration_details.emp_mstr.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).employee_last_name),
                    department_id = p.ndpc.fkid_department_id,
                    dept_Name = _context.tbl_department_master.Where(x => x.department_id == p.ndpc.fkid_department_id && x.is_active == 1).FirstOrDefault().department_name,
                    pending_dept = _context.tbl_department_master.Where(x => x.department_id == p.ndpc.fkid_department_id && x.is_active == 1 && p.ndpc.is_Outstanding == 1).FirstOrDefault().department_name,
                    company_id = p.es.company_id,
                    date_of_Resign = p.es.resignation_dt,
                    date_of_Reliving = p.es.final_relieve_dt,
                    _date_of_joining = p.ndpc.empSepration_details.emp_mstr.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).date_of_joining,
                    designation = "",
                    location = "",
                    is_outstanding = p.ndpc.is_Outstanding

                }).ToList();

                result = result.Where(x => x.is_deleted == 0).ToList();

                var data = result.GroupBy(y => new { y.emp_id, y.sepration_id, y.emp_code, y.emp_name, y.company_id, y.date_of_Resign, y.date_of_Reliving, y._date_of_joining, y.designation, y.location }).Select(x => new
                {
                    x.Key.emp_id,
                    x.Key.emp_code,
                    x.Key.sepration_id,
                    x.Key.emp_name,
                    x.Key.designation,
                    x.Key.company_id,
                    x.Key.date_of_Reliving,
                    x.Key.date_of_Resign,
                    department_id = string.Join(", ", result.Where(z => z.emp_id == x.Key.emp_id && z.sepration_id == x.Key.sepration_id).Select(a => a.department_id).Distinct()),
                    department_Name = string.Join(", ", result.Where(z => z.emp_id == x.Key.emp_id && z.sepration_id == x.Key.sepration_id).Select(a => a.dept_Name).Distinct()),
                    department_pending = string.Join(", ", result.Where(z => z.emp_id == x.Key.emp_id && z.sepration_id == x.Key.sepration_id).Select(a => a.pending_dept).Distinct()),
                    x.Key.location,
                    x.Key._date_of_joining,
                    is_outstanding = result.Where(z => z.emp_id == x.Key.emp_id && z.sepration_id == x.Key.sepration_id).Select(a => a.is_outstanding).Distinct().Any(y => y == 1) ? "Yes" : "No"
                }).ToList();

                #region old

                //var result1 = _context.tbl_No_dues_particular_master.Join(_context.tbl_emp_separation, tndp => tndp.department_id, tes => tes.emp_id, (tndp, tes) => new { tndp, tes }).Where(x => x.tndp.is_deleted == 0 && x.tes.resignation_dt > DateTime.MinValue && x.tes.is_deleted != 1 && x.tes.final_relieve_dt > DateTime.Now.AddDays(-6) && x.tes.final_relieve_dt <= DateTime.Now && _clsCurrentUser.RoleId.Contains(1) ? x.tndp.department_id == currUserDept : true).Select(a => new
                //{
                //    //emp_id = a.tndp.employee_id,
                //    comp_id = a.tndp.company_id,
                //    //a.tes.is_deleted,
                //    //     dept_id = a.department_id,
                //    ////       particular_id = a.pkid_ParticularMaster,
                //    //emp_code = _context.tbl_employee_master.Where(b => b.employee_id == a.tndp.employee_id && b.is_active == 1).FirstOrDefault().emp_code,

                //    //emp_name = string.Format("{0} {1} {2}", _context.tbl_employee_master.Where(b => b.employee_id == a.tndp.employee_id && b.is_active == 1).FirstOrDefault().tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                //    //                _context.tbl_employee_master.Where(b => b.employee_id == a.tndp.employee_id && b.is_active == 1).FirstOrDefault().tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                //    //                _context.tbl_employee_master.Where(b => b.employee_id == a.tndp.employee_id && b.is_active == 1).FirstOrDefault().tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name),

                //    //dept_Name = _context.tbl_employee_master.Where(b => b.employee_id == a.tndp.employee_id && b.is_active == 1).FirstOrDefault().tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).tbl_department_master.department_name,

                //    //designation = _context.tbl_employee_master.Where(b => b.employee_id == a.tndp.employee_id && b.is_active == 1).FirstOrDefault().tbl_emp_desi_allocation.FirstOrDefault().tbl_designation_master.designation_name,

                //    //location = _context.tbl_employee_master.Where(b => b.employee_id == a.tndp.employee_id && b.is_active == 1).FirstOrDefault().tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).tbl_location_master.location_name,

                //    //_date_of_joining = _context.tbl_employee_master.Where(b => b.employee_id == a.tndp.employee_id && b.is_active == 1).FirstOrDefault().tbl_emp_officaial_sec.FirstOrDefault().date_of_joining,
                //    date_of_Resign = a.tes.resignation_dt,
                //    date_of_Reliving = a.tes.final_relieve_dt,
                //    //date_of_Reliving = _context.tbl_emp_separation.Where(b => b.emp_id == a.tndp.employee_id && b.is_deleted == 0).FirstOrDefault().final_relieve_dt != null ? _context.tbl_emp_separation.Where(b => b.emp_id == a.tndp.employee_id && b.is_deleted == 0).FirstOrDefault().final_relieve_dt : DateTime.MinValue,
                //    status = a.tndp.is_deleted == 0 ? "Active" : a.tndp.is_deleted == 1 ? "Deleted" : "",
                //    requester_id = a.tndp.created_by
                //}).Distinct().ToList();
                #endregion

                return Ok(data);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [HttpPost("GetNoDuesParticularsofEmployee")]
        [Authorize(Policy = nameof(enmMenuMaster.Dashboard))]
        public IActionResult GetNoDuesParticularsofEmployee([FromBody] No_dues_emp_particular_Clearence_detail value)
        {
            ResponseMsg objresponse = new ResponseMsg();

            try
            {
                var responsibleEmp = _context.tbl_No_dues_particular_responsible.Select(x => x.emp_id).ToList();

                if (_clsCurrentUser.Is_SuperAdmin ? (!_clsCurrentUser.DownlineEmpId.Contains(value.fkid_EmpId)) : (!responsibleEmp.Contains(_clsCurrentUser.EmpId)))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unauthorize Access...!!";
                    return Ok(objresponse);
                }

                var currUserDept = _context.tbl_emp_officaial_sec.Where(b => b.employee_id == _clsCurrentUser.EmpId && b.is_deleted == 0).FirstOrDefault().department_id;

                var result = _context.tbl_No_dues_emp_particular_Clearence_detail.Where(x => x.is_deleted == 0 && x.fkid_EmpSaperationId == value.fkid_EmpSaperationId && (_clsCurrentUser.Is_SuperAdmin ? true : (responsibleEmp.Contains(_clsCurrentUser.EmpId)))).Select(a => new
                {
                    emp_id = a.empSepration_details.emp_id,
                    es_id = a.fkid_EmpSaperationId,
                    emp_code = string.Format("{0}", a.empSepration_details.emp_mstr.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).employee_first_name),
                    comp_id = a.empSepration_details.company_id,
                    dept_id = a.fkid_department_id,
                    dept_name = a.dept_details.department_name,
                    particular_id = a.fkid_ParticularId,
                    id = a.pkid_EmpParticularClearance,
                    particular_name = a.particular_details.particular_name,
                    a.is_Outstanding,
                    a.dept_details.department_name,
                    a.remarks
                }).OrderBy(y => y.id).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        //Save No Dues Clearence Form
        [Route("Save_NoDueClearenceForm")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.MachineMaster))]
        public IActionResult Save_NoDueClearenceForm([FromBody] NoDues_ClearenceForm objendcf)
        {
            Response_Msg objResult = new Response_Msg();


            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (objendcf.lst_dept_id != null && objendcf.lst_empSep_ids != null && objendcf.lst_part_id != null)
                    {

                        for (int i = 0; i < objendcf.lst_part_id.Count; i++)
                        {
                            tbl_No_dues_emp_particular_Clearence_detail exist = _context.tbl_No_dues_emp_particular_Clearence_detail.Where(x => x.fkid_department_id == objendcf.lst_dept_id[i] && x.pkid_EmpParticularClearance == objendcf.lst_part_id[i] && x.is_deleted == 0).ToList().FirstOrDefault();

                            if (exist != null)
                            {
                                exist.is_Outstanding = objendcf.lst_outstandings[i];
                                exist.remarks = objendcf.lst_final_Remarks[i];
                                exist.modified_by = _clsCurrentUser.EmpId;
                                exist.modified_date = DateTime.Now;

                                _context.tbl_No_dues_emp_particular_Clearence_detail.UpdateRange(exist);
                                _context.SaveChanges();
                            }
                            else
                            {
                                objResult.StatusCode = 1;
                                objResult.Message = "Particular does not exist...!";
                                return Ok(objResult);
                            }
                        }

                        var a = _context.tbl_No_dues_emp_particular_Clearence_detail.Where(x => x.fkid_department_id == objendcf.lst_dept_id[0] && x.fkid_EmpSaperationId == objendcf.lst_empSep_ids[0] && x.is_deleted == 0 && x.is_Outstanding == 1).ToList();

                        if (a.Count() < 1)
                        {
                            _context.tbl_No_dues_clearance_Department.Where(x => x.department_id == objendcf.lst_dept_id[0] && x.fkid_EmpSaperationId == objendcf.lst_empSep_ids[0] && x.is_deleted == 0).ToList().ForEach(z => { z.is_Cleared = 1; z.modified_date = DateTime.Now; z.modified_by = _clsCurrentUser.EmpId; });
                        }
                        else
                        {
                            _context.tbl_No_dues_clearance_Department.Where(x => x.department_id == objendcf.lst_dept_id[0] && x.fkid_EmpSaperationId == objendcf.lst_empSep_ids[0] && x.is_deleted == 0).ToList().ForEach(z => { z.is_Cleared = 0; z.modified_date = DateTime.Now; z.modified_by = _clsCurrentUser.EmpId; });
                        }
                        _context.SaveChanges();


                        var b = _context.tbl_No_dues_clearance_Department.Where(x => x.fkid_EmpSaperationId == objendcf.lst_empSep_ids[0] && x.is_deleted == 0 && x.is_Cleared == 0).ToList();
                        if (b.Count() < 1)
                        {
                            _context.tbl_emp_separation.Where(x => x.sepration_id == objendcf.lst_empSep_ids[0] && x.is_deleted == 0).ToList().ForEach(z => { z.is_no_due_cleared = 1; z.modified_dt = DateTime.Now; z.modified_by = _clsCurrentUser.EmpId; });
                        }
                        else
                        {
                            _context.tbl_emp_separation.Where(x => x.sepration_id == objendcf.lst_empSep_ids[0] && x.is_deleted == 0).ToList().ForEach(z => { z.is_no_due_cleared = 0; z.modified_dt = DateTime.Now; z.modified_by = _clsCurrentUser.EmpId; });
                        }
                        _context.SaveChanges();

                    }
                    transaction.Commit();
                    objResult.StatusCode = 0;
                    objResult.Message = "No Dues Particular Saved Successfully...!";
                    return Ok(objResult);

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    objResult.StatusCode = 0;
                    objResult.Message = ex.Message;
                    return Ok(objResult);
                }
            }
        }

        #endregion NO Due Clearence Detail of Employee

        [HttpPost("GetNoDuesClearenceReport")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpSalary))]
        public IActionResult GetNoDuesClearenceReport([FromBody] NoDues_particular_master value)
        {
            ResponseMsg objresponse = new ResponseMsg();
            //int _year = Convert.ToInt32(value.monthYear.Split('-')[0].ToString());
            //int _month = Convert.ToInt32(value.monthYear.Split('-')[1].ToString());

            try
            {
                //   var result = _context.tbl_No_dues_emp_particular_Clearence_detail.Where(x => x.is_deleted == 0 && x.empSepration_details.company_id == value.company_id && x.empSepration_details.is_deleted == 0 && x.created_date.Year == _year && x.created_date.Month == _month).Select(a => new
                var result = _context.tbl_No_dues_emp_particular_Clearence_detail.Where(x => x.is_deleted == 0 && x.empSepration_details.company_id == value.company_id && x.empSepration_details.is_deleted == 0 && x.created_date <= value.toDate && x.created_date >= value.fromDate).Select(a => new
                {
                    emp_id = a.empSepration_details.emp_id,
                    comp_id = a.empSepration_details.company_id,
                    dept_id = _context.tbl_department_master.Where(x => x.department_id == a.empSepration_details.emp_mstr.tbl_emp_officaial_sec.FirstOrDefault(e => e.is_deleted == 0).department_id && x.is_active == 1).FirstOrDefault().department_id,
                    //particular_id = a.particular_details.pkid_ParticularMaster,
                    emp_code = _context.tbl_employee_master.Where(b => b.employee_id == a.empSepration_details.emp_id && b.is_active == 1).FirstOrDefault().emp_code,

                    emp_name = string.Format("{0} {1} {2}",
                         _context.tbl_emp_officaial_sec.Where(b => b.employee_id == a.empSepration_details.emp_id && b.is_deleted == 0).FirstOrDefault().employee_first_name,
                         _context.tbl_emp_officaial_sec.Where(b => b.employee_id == a.empSepration_details.emp_id && b.is_deleted == 0).FirstOrDefault().employee_middle_name,
                         _context.tbl_emp_officaial_sec.Where(b => b.employee_id == a.empSepration_details.emp_id && b.is_deleted == 0).FirstOrDefault().employee_last_name),

                    dept_Name = _context.tbl_department_master.Where(x => x.department_id == a.empSepration_details.emp_mstr.tbl_emp_officaial_sec.FirstOrDefault(e => e.is_deleted == 0).department_id && x.is_active == 1).FirstOrDefault().department_name,
                    designation = _context.tbl_emp_desi_allocation.Where(b => b.employee_id == a.empSepration_details.emp_id).FirstOrDefault() != null ? _context.tbl_emp_desi_allocation.Where(b => b.employee_id == a.empSepration_details.emp_id).FirstOrDefault().tbl_designation_master.designation_name : "",

                    location = _context.tbl_emp_officaial_sec.Where(b => b.employee_id == a.empSepration_details.emp_id && b.is_deleted == 1).FirstOrDefault().tbl_location_master.location_name,

                    _date_of_joining = _context.tbl_emp_officaial_sec.Where(b => b.employee_id == a.empSepration_details.emp_id && b.is_deleted == 0).FirstOrDefault().date_of_joining,
                    date_of_Resign = _context.tbl_emp_separation.Where(b => b.emp_id == a.empSepration_details.emp_id && b.is_deleted == 0).FirstOrDefault().resignation_dt != null ? _context.tbl_emp_separation.Where(b => b.emp_id == a.empSepration_details.emp_id && b.is_deleted == 0).FirstOrDefault().resignation_dt : DateTime.MinValue,
                    date_of_Reliving = _context.tbl_emp_separation.Where(b => b.emp_id == a.empSepration_details.emp_id && b.is_deleted == 0).FirstOrDefault().final_relieve_dt != null ? _context.tbl_emp_separation.Where(b => b.emp_id == a.empSepration_details.emp_id && b.is_deleted == 0).FirstOrDefault().final_relieve_dt : DateTime.MinValue,

                    date_of_ndc_request = a.created_date.Date,

                    reporting_manager = string.Format("{0} {1} {2}",
                                _context.tbl_emp_officaial_sec.Where(b => b.employee_id == (_context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().final_approval == 1 ? _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().m_one_id : _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().final_approval == 2 ? _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().m_two_id : _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().final_approval == 3 ? _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().m_three_id : _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().m_one_id) && b.is_deleted == 0).FirstOrDefault().employee_first_name,
                                   _context.tbl_emp_officaial_sec.Where(b => b.employee_id == (_context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().final_approval == 1 ? _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().m_one_id : _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().final_approval == 2 ? _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().m_two_id : _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().final_approval == 3 ? _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().m_three_id : _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().m_one_id) && b.is_deleted == 0).FirstOrDefault().employee_middle_name,
                                      _context.tbl_emp_officaial_sec.Where(b => b.employee_id == (_context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().final_approval == 1 ? _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().m_one_id : _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().final_approval == 2 ? _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().m_two_id : _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().final_approval == 3 ? _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().m_three_id : _context.tbl_emp_manager.Where(y => y.employee_id == a.empSepration_details.emp_id && y.is_deleted == 0).FirstOrDefault().m_one_id) && b.is_deleted == 0).FirstOrDefault().employee_last_name),
                    ndc_department = string.Join(", ", _context.tbl_No_dues_clearance_Department.Where(z => z.fkid_EmpSaperationId == a.fkid_EmpSaperationId && z.is_deleted == 0).Select(n => n.dept_details.department_name).Distinct()),

                    ndc_department_authority_name = string.Format("{0} {1} {2}",
                                   _context.tbl_emp_officaial_sec.Where(b => b.employee_id == a.modified_by && b.is_deleted == 0).FirstOrDefault().employee_first_name,
                                    _context.tbl_emp_officaial_sec.Where(b => b.employee_id == a.modified_by && b.is_deleted == 0).FirstOrDefault().employee_middle_name,
                                   _context.tbl_emp_officaial_sec.Where(b => b.employee_id == a.modified_by && b.is_deleted == 0).FirstOrDefault().employee_last_name),

                    ndc_department_outstanding = _context.tbl_No_dues_emp_particular_Clearence_detail.Where(x => x.fkid_EmpSaperationId == a.fkid_EmpSaperationId && x.is_deleted == 0).Select(b => (b.is_Outstanding)).Contains(1) ? "YES" : "NO",
                    // //     ndc_department_outstanding = (_context.tbl_No_dues_particular_master.Where(x => x.employee_id == a.employee_id && x.is_deleted == 0).Select(b => Convert.ToInt32(b.outstanding)).ToList()).Contains(Convert.ToInt32(1)) ? "Yes" : "No",
                    ndc_department_remark = String.Join(". ", (_context.tbl_No_dues_emp_particular_Clearence_detail.Where(x => x.fkid_EmpSaperationId == a.fkid_EmpSaperationId && x.is_deleted == 0).Select(b => b.remarks).Distinct().ToArray())),
                    ndc_department_status = _context.tbl_No_dues_emp_particular_Clearence_detail.Where(x => x.fkid_EmpSaperationId == a.fkid_EmpSaperationId && x.is_deleted == 0).Select(b => (b.is_Outstanding)).Contains(1) ? "In-Progress" : "Completed"
                }).Distinct().ToList();



                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Schedular_NoDuesClearParticular")]
        [HttpGet]
        public IActionResult Schedular_NoDuesClearParticular()
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                int dayId = Convert.ToInt32(currentTime.ToString("yyyyMMdd"));

                tbl_scheduler_master NoDues = _context.tbl_scheduler_master.Where(p => p.scheduler_name == "NoDues" && p.is_deleted == 0).FirstOrDefault();
                if (NoDues == null)
                {
                    NoDues = new tbl_scheduler_master()
                    {
                        is_deleted = 0,
                        last_runing_date = currentTime,
                        schduler_type = enmSchdulerType.NoDues,
                        scheduler_name = "NoDues"
                    };
                    _context.tbl_scheduler_master.Add(NoDues);
                    _context.SaveChanges();
                }
                // check for one time run in one day
                var daily_schdeuler = _context.tbl_scheduler_details.Where(p => p.scheduler_id == NoDues.scheduler_id && p.is_deleted == 0 && p.transaction_no == dayId).FirstOrDefault();
                if (daily_schdeuler == null)
                {
                    var get_sepration_emp_nxt5days = _context.tbl_emp_separation.Where(x => x.final_relieve_dt >= DateTime.Now && x.final_relieve_dt <= DateTime.Now.AddDays(5) && x.is_final_approve == 1 && x.is_deleted == 0 && x.is_cancel == 0).ToList();
                    var getparticularlist = _context.tbl_No_dues_particular_master.Where(x => x.is_deleted == 0).ToList();
                    var getdepartment = getparticularlist.Select(x => x.department_id).Distinct().ToList();

                    if (get_sepration_emp_nxt5days != null && get_sepration_emp_nxt5days.Count > 0)
                    {
                        for (var i = 0; i < get_sepration_emp_nxt5days.Count; i++)
                        {
                            if (getparticularlist != null && getparticularlist.Count > 0)
                            {
                                for (var j = 0; j < getparticularlist.Count; j++)
                                {
                                    tbl_No_dues_emp_particular_Clearence_detail obj_c_details = new tbl_No_dues_emp_particular_Clearence_detail();

                                    var already_exists_details = _context.tbl_No_dues_emp_particular_Clearence_detail
                                        .Where(x => x.fkid_EmpSaperationId == get_sepration_emp_nxt5days[i].sepration_id &&
                                                  x.fkid_department_id == getparticularlist[j].department_id &&
                                                  x.fkid_ParticularId == getparticularlist[j].pkid_ParticularMaster).ToList();

                                    if (already_exists_details.Count == 0)
                                    {
                                        obj_c_details.fkid_EmpSaperationId = get_sepration_emp_nxt5days[i].sepration_id;
                                        obj_c_details.fkid_department_id = getparticularlist[j].department_id;
                                        obj_c_details.fkid_ParticularId = getparticularlist[j].pkid_ParticularMaster;
                                        obj_c_details.is_Outstanding = 2;
                                        obj_c_details.remarks = "";
                                        obj_c_details.is_deleted = 0;
                                        obj_c_details.created_by = 1451;
                                        obj_c_details.created_date = DateTime.Now;
                                        obj_c_details.modified_by = 0;
                                        obj_c_details.modified_date = DateTime.Now;
                                        _context.tbl_No_dues_emp_particular_Clearence_detail.Add(obj_c_details);
                                        _context.SaveChanges();
                                    }
                                }

                                for (var k = 0; k < getdepartment.Count; k++)
                                {
                                    var already_exists_dept = _context.tbl_No_dues_clearance_Department
                                         .Where(x => x.fkid_EmpSaperationId == get_sepration_emp_nxt5days[i].sepration_id &&
                                                   x.department_id == getdepartment[k]).ToList();

                                    if (already_exists_dept.Count == 0)
                                    {
                                        tbl_No_dues_clearance_Department obj_c_department = new tbl_No_dues_clearance_Department();
                                        obj_c_department.fkid_EmpSaperationId = get_sepration_emp_nxt5days[i].sepration_id;
                                        obj_c_department.department_id = getdepartment[k];
                                        obj_c_department.is_Cleared = 0;
                                        obj_c_department.is_deleted = 0;
                                        obj_c_department.created_by = 1451;
                                        obj_c_department.created_date = DateTime.Now;
                                        obj_c_department.modified_by = 1451;
                                        obj_c_department.modified_date = DateTime.Now;
                                        _context.tbl_No_dues_clearance_Department.Add(obj_c_department);
                                        _context.SaveChanges();
                                    }
                                }
                            }
                            // sepration_id
                        }

                        //mail send to responsible person department wise
                        foreach (var h in get_sepration_emp_nxt5days)
                        {
                            var empdetails = _context.tbl_emp_officaial_sec.Where(x => x.employee_id == h.emp_id && x.is_deleted == 0).FirstOrDefault();
                            var seperationname = $"{empdetails.employee_first_name} {empdetails.employee_middle_name ?? string.Empty} {empdetails.employee_last_name ?? string.Empty}";
                            var seperationemailid = empdetails.official_email_id;
                            var empcode = _context.tbl_employee_master.Where(x => x.employee_id == h.emp_id && x.is_active == 1).Select(x => x.emp_code).FirstOrDefault();


                            var responsible_person = _context.tbl_No_dues_particular_responsible.Where(x => x.is_deleted == 0 && getdepartment.Contains(x.department_id)).Select(x => x.emp_id).Distinct().ToList();
                            var resposible_emails = "";

                            foreach (var empid in responsible_person)
                            {
                                var rdetails = _context.tbl_emp_officaial_sec.Where(x => x.employee_id == empid && x.is_deleted == 0).FirstOrDefault();
                                var remailid = rdetails.official_email_id;
                                resposible_emails += remailid + ",";
                            }
                            resposible_emails = resposible_emails.Remove(resposible_emails.Length - 1);
                            MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);
                            obj_ms.SendMailNoDuesResponsiblePerson(resposible_emails, seperationemailid, seperationname, empcode);
                        }
                        _context.tbl_scheduler_details.Add(new tbl_scheduler_details() { transaction_no = dayId, is_deleted = 0, last_runing_date = currentTime, scheduler_id = NoDues.scheduler_id });
                        _context.SaveChanges();
                    }
                }
                return Ok("Process Successfully !!");

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("save_week_off_all_company_all_sunday_dynamic")]
        [HttpGet]
        public IActionResult save_week_off_all_company_all_sunday_dynamic()
        {
            try
            {
                var emp_list = _context.tbl_employee_company_map.Where(x => x.is_deleted == 0).ToList();
                foreach (var emp in emp_list)
                {
                    tbl_emp_weekoff tew = new tbl_emp_weekoff()
                    {
                        employee_id = emp.employee_id,
                        is_fixed_weekly_off = 2,
                        effective_from_date = Convert.ToDateTime("2020-06-29"),
                        is_deleted = 0,
                        created_by = 1451,
                        created_date = DateTime.Now,
                        modifed_by = 1451,
                        modifed_date = DateTime.Now
                    };

                    _context.tbl_emp_weekoff.Add(tew);
                   // _context.SaveChanges();

                    for (int i=1;i<=5;i++)
                    {
                        tbl_shift_week_off ShiftWeekOf = new tbl_shift_week_off();
                        ShiftWeekOf.company_id =Convert.ToInt32(emp.company_id);
                        ShiftWeekOf.week_day = 7;
                        ShiftWeekOf.days =Convert.ToByte(i);
                        ShiftWeekOf.emp_id = emp.employee_id;
                        ShiftWeekOf.emp_weekoff_id = tew.emp_weekoff_id;
                        ShiftWeekOf.is_active = 1;
                        ShiftWeekOf.created_by = 1451;
                        ShiftWeekOf.last_modified_by = 1451;
                        ShiftWeekOf.created_date = DateTime.Now;
                        ShiftWeekOf.last_modified_date = DateTime.Now;

                        _context.tbl_shift_week_off.Add(ShiftWeekOf);
                    }
                }
                _context.SaveChanges();

                return Ok("Process Successfully !!");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("save_week_off_sakshm_glaze_2nd_3rd_sat_weekoff")]
        [HttpGet]
        public IActionResult save_week_off_sakshm_glaze_2nd_3rd_sat_weekoff()
        {
            try
            {
                var emp_list = _context.tbl_employee_company_map.Where(x => x.is_deleted == 0 && x.is_default==true && (x.company_id==1 || x.company_id==2)).ToList();
                foreach (var emp in emp_list)
                {
                    var emp_weekoff_id = _context.tbl_emp_weekoff.Where(x => x.is_deleted==0 && x.employee_id == emp.employee_id).Select(x=>x.emp_weekoff_id).FirstOrDefault();
                    //tbl_emp_weekoff tew = new tbl_emp_weekoff()
                    //{
                    //    employee_id = emp.employee_id,
                    //    is_fixed_weekly_off = 2,
                    //    effective_from_date = Convert.ToDateTime("2020-06-29"),
                    //    is_deleted = 0,
                    //    created_by = 1451,
                    //    created_date = DateTime.Now,
                    //    modifed_by = 1451,
                    //    modifed_date = DateTime.Now
                    //};

                    //_context.tbl_emp_weekoff.Add(tew);
                    // _context.SaveChanges();

                    for (int i = 2; i <= 3; i++)
                    {
                        tbl_shift_week_off ShiftWeekOf = new tbl_shift_week_off();
                        ShiftWeekOf.company_id = Convert.ToInt32(emp.company_id);
                        ShiftWeekOf.week_day = 6;
                        ShiftWeekOf.days = Convert.ToByte(i);
                        ShiftWeekOf.emp_id = emp.employee_id;
                        ShiftWeekOf.emp_weekoff_id = emp_weekoff_id;
                        ShiftWeekOf.is_active = 1;
                        ShiftWeekOf.created_by = 1451;
                        ShiftWeekOf.last_modified_by = 1451;
                        ShiftWeekOf.created_date = DateTime.Now;
                        ShiftWeekOf.last_modified_date = DateTime.Now;

                        _context.tbl_shift_week_off.Add(ShiftWeekOf);
                    }
                }
                _context.SaveChanges();

                return Ok("Process Successfully !!");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

    }// Class End
}
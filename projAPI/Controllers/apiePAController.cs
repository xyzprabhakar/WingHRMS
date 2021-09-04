using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using projAPI.Classes;
using projAPI.Model;
using projContext;
using projContext.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static projAPI.Model.ePA;

namespace projAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class apiePAController : ControllerBase
    {
        private readonly Context _context;
        private readonly IOptions<AppSettings> _appSettings;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _AC;
        private readonly clsCurrentUser _clsCurrentUser;
        private readonly clsEmployeeDetail _clsEmployeeDetail;
        public apiePAController(Context context, IOptions<AppSettings> appSettings, IHostingEnvironment environment, IHttpContextAccessor AC,clsCurrentUser _clsCurrentUser,clsEmployeeDetail _clsEmployeeDetail)
        {
            _context = context;
            _appSettings = appSettings;
            _hostingEnvironment = environment;
            _AC = AC;
            this._clsCurrentUser = _clsCurrentUser;
            this._clsEmployeeDetail = _clsEmployeeDetail;
        }


        //save Financial year
        [Route("Save_FinancialMaster")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.EPAFiscalYear))]
        public IActionResult Save_FinancialMaster([FromBody] tbl_epa_fiscal_yr_mstr objfinancial)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(objfinancial.company_id??0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }
            try
            {
                var exist = _context.tbl_epa_fiscal_yr_mstr.Where(x => x.is_deleted == 0 && x.financial_year_name == objfinancial.financial_year_name && x.from_date >= objfinancial.from_date && x.from_date <= objfinancial.to_date).FirstOrDefault();

                if (exist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Financial Year already exist !!";
                    return Ok(objResult);
                }
                else
                {
                    objfinancial.financial_year_name = objfinancial.financial_year_name;
                    objfinancial.from_date = objfinancial.from_date;
                    objfinancial.to_date = objfinancial.to_date;
                    objfinancial.is_deleted = 0;
                    objfinancial.created_by = objfinancial.created_by;
                    objfinancial.company_id = objfinancial.company_id;
                    objfinancial.created_date = DateTime.Now;
                    _context.Entry(objfinancial).State = EntityState.Added;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Financial Year save successfully !!";
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

        //get Financial Year data
        [Route("Get_FinancialYearData/{fyi_id}/{company_id}")]
        [HttpGet]
        [Authorize(Policy =nameof(enmMenuMaster.EPAFiscalYear))]
        public IActionResult Get_FinancialYearData([FromRoute] int fyi_id, int company_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Compnay Access...!!";
                return Ok(objresponse);
            }
            try
            {
                if (fyi_id > 0)
                {
                    var data = _context.tbl_epa_fiscal_yr_mstr.Where(x => x.fiscal_year_id == fyi_id && x.company_id == company_id).FirstOrDefault();
                    return Ok(data);
                }
                else
                {

                    var data = _context.tbl_epa_fiscal_yr_mstr.Where(a => a.is_deleted == 0 && a.company_id == company_id).Select(a => new
                    {
                        fyi_id = a.fiscal_year_id,
                        financial_year_name = a.financial_year_name,
                        fromdate = a.from_date,
                        todate = a.to_date
                    }).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        //update Financial Year
        [Route("Update_FinancialYear")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.EPAFiscalYear))]
        public IActionResult Update_FinancialYear([FromBody] tbl_epa_fiscal_yr_mstr objfinancial)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(objfinancial.company_id??0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }
            try
            {
                var exist = _context.tbl_epa_fiscal_yr_mstr.Where(x => x.is_deleted == 0 && x.fiscal_year_id == objfinancial.fiscal_year_id && x.from_date >= objfinancial.from_date && x.from_date <= objfinancial.to_date).FirstOrDefault();

                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid FYI id please try again !!";
                    return Ok(objResult);
                }
                else
                {
                    //check with same grade name in other greade id
                    var duplicate = _context.tbl_epa_fiscal_yr_mstr.Where(x => x.fiscal_year_id != objfinancial.fiscal_year_id && x.financial_year_name == objfinancial.financial_year_name).FirstOrDefault();
                    if (duplicate != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Financial Year exist in the system please check & try !!";
                        return Ok(objResult);
                    }

                    exist.financial_year_name = objfinancial.financial_year_name;
                    exist.from_date = objfinancial.from_date;
                    exist.to_date = objfinancial.to_date;
                    exist.is_deleted = 0;
                    exist.last_modified_by = 1;
                    exist.last_modified_date = DateTime.Now;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Financial Year updated successfully !!";
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

        //Save_Epa_Cycle_Master
        [Route("Save_Epa_Cycle_Master")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.EPACycle))]
        public IActionResult Save_Epa_Cycle_Master([FromBody] tbl_epa_cycle_master obj_cycle)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(obj_cycle.company_id??0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access..!!";
                return Ok(objResult);
            }
            try
            {
                var exist = _context.tbl_epa_cycle_master.Where(x =>x.company_id==obj_cycle.company_id && x.cycle_type==obj_cycle.cycle_type && x.is_deleted == 0).FirstOrDefault();

                if (exist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "EPA Cycle already exist !!";
                    return Ok(objResult);
                }
                else
                {
                    obj_cycle.cycle_type = obj_cycle.cycle_type;
                    obj_cycle.from_date = obj_cycle.from_date;
                    obj_cycle.number_of_day = obj_cycle.number_of_day;
                    obj_cycle.is_deleted = 0;
                    obj_cycle.created_by = obj_cycle.created_by;
                    obj_cycle.company_id = obj_cycle.company_id;
                    obj_cycle.created_date = DateTime.Now;
                    _context.Entry(obj_cycle).State = EntityState.Added;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "EPA Cycle save successfully !!";
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

        //Get_Epa_Cycle_Master
        [Route("Get_Epa_Cycle_Master/{cycle_id}/{company_id}")]
        [HttpGet]
        [Authorize(Policy =nameof(enmMenuMaster.EPACycle))]
        public IActionResult Get_Epa_Cycle_Master([FromRoute] int cycle_id, int company_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access....!!";
                return Ok(objresponse);
            }
            try
            {
                if (cycle_id > 0)
                {
                    var data = _context.tbl_epa_cycle_master.Where(x => x.cycle_id == cycle_id && x.is_deleted == 0 && x.company_id == company_id).FirstOrDefault();
                    return Ok(data);
                }
                else
                {
                    var data = _context.tbl_epa_cycle_master.Where(a => a.is_deleted == 0 && a.company_id == company_id).Select(a => new
                    {
                        a.cycle_id,
                        a.cycle_type,
                        a.from_date,
                        a.number_of_day,
                        a.is_deleted,
                        a.created_by,
                        a.company_id,
                        a.created_date
                    }).ToList();

                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(ex.Message);
            }
        }

        //update Epa_Cycle_Master
        [Route("Update_Epa_Cycle_Master")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.EPACycle))]
        public IActionResult Update_Epa_Cycle_Master([FromBody] tbl_epa_cycle_master obj_cycle)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(obj_cycle.company_id??0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }
            try
            {
                var exist = _context.tbl_epa_cycle_master.Where(x =>x.cycle_id == obj_cycle.cycle_id && x.is_deleted == 0).FirstOrDefault();

                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid FYI id please try again !!";
                    return Ok(objResult);
                }
                else
                {
                    //check with same grade name in other greade id
                    var duplicate = _context.tbl_epa_cycle_master.Where(x => x.cycle_id != obj_cycle.cycle_id && x.company_id==obj_cycle.company_id && x.cycle_type==obj_cycle.cycle_type && x.is_deleted == 0).FirstOrDefault();
                    if (duplicate != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "EPA Cycle exist in the system please check & try !!";
                        return Ok(objResult);
                    }

                    exist.cycle_type = obj_cycle.cycle_type;
                    exist.from_date = obj_cycle.from_date;
                    exist.number_of_day = obj_cycle.number_of_day;
                    exist.is_deleted = 0;
                    exist.last_modified_by = obj_cycle.created_by;
                    exist.last_modified_date = DateTime.Now;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "EPA Cycle updated successfully !!";
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

        //Save_Epa_Cycle_Master
        [Route("Save_EpaWorkingRoleMaster")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.EPAWorkingRole))]
        public IActionResult Save_EpaWorkingRoleMaster([FromBody] tbl_working_role obj_work)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(obj_work.company_id??0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }
            try
            {
                var exist = _context.tbl_working_role.Where(x =>x.company_id==obj_work.company_id && x.working_role_name.Trim().ToUpper() == obj_work.working_role_name.Trim().ToUpper() && x.dept_id == obj_work.dept_id).FirstOrDefault();

                if (exist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "EPA Working Role Name already exist !!";
                    return Ok(objResult);
                }
                else
                {
                    if (obj_work.is_default == 1)
                    {
                        var exist_default = _context.tbl_working_role.Where(x =>x.company_id==obj_work.company_id && x.is_default == 1).ToList();
                        if (exist_default.Count > 0)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Only one working role set as default.";
                        }
                        else
                        {
                            obj_work.is_active = obj_work.is_active;
                            obj_work.created_by = obj_work.created_by;
                            obj_work.created_date = DateTime.Now;
                            obj_work.last_modified_by = 0;
                            obj_work.last_modified_date = Convert.ToDateTime("01-01-2000");


                            _context.Entry(obj_work).State = EntityState.Added;
                            _context.SaveChanges();

                            objResult.StatusCode = 0;
                            objResult.Message = "Working Role Save Successfully !!";
                        }
                    }
                    else
                    {
                        obj_work.is_active = obj_work.is_active;
                        obj_work.created_by = obj_work.created_by;
                        obj_work.created_date = DateTime.Now;
                        obj_work.last_modified_by = 0;
                        obj_work.last_modified_date = Convert.ToDateTime("01-01-2000");


                        _context.Entry(obj_work).State = EntityState.Added;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "Working Role Save Successfully !!";

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

        //get designation master data
        [Route("Get_EpaWorkingRoleMaster/{wr_id}/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.EPAWorkingRole))]
        public IActionResult Get_EpaWorkingRoleMaster([FromRoute] int wr_id, int company_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access....!!";
                return Ok(objresponse);
            }
            try
            {
                if (wr_id > 0)
                {
                    var data = _context.tbl_working_role.Where(x => x.working_role_id == wr_id && x.company_id == company_id).Select(a => new
                    {
                        a.working_role_name,
                        a.dept_id,
                        dept_name = a.tbl_dept_master.department_name,
                        a.working_role_id,
                        a.is_active,
                        a.created_by,
                        a.created_date,
                       a.is_default,
                       a.last_modified_date,
                    }).FirstOrDefault();

                    return Ok(data);
                }
                else
                {
                    var data = _context.tbl_working_role.Where(a => a.company_id == company_id).Select(a => new
                    {
                        a.working_role_name,
                        a.dept_id,
                        dept_name = a.tbl_dept_master.department_name,
                        a.working_role_id,
                        a.is_active,
                        a.created_by,
                        a.created_date,
                        a.is_default,
                        a.last_modified_date,
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
        [Route("Update_EpaWorkingRoleMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.EPAWorkingRole))]
        public IActionResult Update_EpaWorkingRoleMaster([FromBody] tbl_working_role obj_work)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(obj_work.company_id??0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }

            try
            {
                var exist = _context.tbl_working_role.Where(x =>x.company_id==obj_work.company_id && x.working_role_id == obj_work.working_role_id).FirstOrDefault();

                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid desgnation id please try again !!";
                    return Ok(objResult);
                }
                else
                {
                    //check with same designation name in other designation id
                    var duplicate = _context.tbl_working_role.Where(x => x.working_role_id != obj_work.working_role_id && x.company_id==obj_work.company_id && x.working_role_name.Trim().ToUpper() == obj_work.working_role_name.Trim().ToUpper() && x.dept_id == obj_work.dept_id).FirstOrDefault();
                    if (duplicate != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Working Role Name exist in the system please check & try !!";
                        return Ok(objResult);
                    }

                    if (obj_work.is_default == 1)
                    {

                        var exist_default = _context.tbl_working_role.Where(x => x.working_role_id != obj_work.working_role_id && x.company_id==obj_work.company_id && x.is_default == 1).FirstOrDefault();
                        if (exist_default != null)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Default Working role already exist !!";
                        }
                        else
                        {

                            exist.working_role_name = obj_work.working_role_name;
                            exist.dept_id = obj_work.dept_id;
                            exist.is_active = obj_work.is_active;
                            exist.is_default = obj_work.is_default;
                            exist.last_modified_by = obj_work.created_by;
                            exist.last_modified_date = DateTime.Now;

                            _context.Entry(exist).State = EntityState.Modified;
                            _context.SaveChanges();

                            objResult.StatusCode = 0;
                            objResult.Message = "Working Role Updated Successfully !!";
                        }
                        
                    }
                    else
                    {
                        exist.working_role_name = obj_work.working_role_name;
                        exist.dept_id = obj_work.dept_id;
                        exist.is_active = obj_work.is_active;
                        exist.is_default = obj_work.is_default;
                        exist.last_modified_by = obj_work.created_by;
                        exist.last_modified_date = DateTime.Now;

                        _context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "Working Role Updated Successfully !!";
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



        //Save_Epa_Cycle_Master
        [Route("Save_KpiCriteria")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.KPICriteria))]
        public IActionResult Save_KpiCriteria([FromBody] tbl_kpi_criteria obj_kpi_crit)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(obj_kpi_crit.company_id??0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }

            tbl_kpi_criteria kpi_cri = new tbl_kpi_criteria();
            try
            {

                var Lastkpi_crit = _context.tbl_kpi_criteria.Where(a => a.is_active == 1 && a.company_id == obj_kpi_crit.company_id).OrderByDescending(a => a.kpi_cr_id).FirstOrDefault();

                if (Lastkpi_crit != null)
                {
                    Lastkpi_crit.is_active = 0;
                    Lastkpi_crit.created_by = obj_kpi_crit.created_by;
                    Lastkpi_crit.created_date = DateTime.Now;
                    Lastkpi_crit.last_modified_by = obj_kpi_crit.created_by;
                    Lastkpi_crit.last_modified_date = DateTime.Now;

                    _context.tbl_kpi_criteria.Attach(Lastkpi_crit);
                    _context.Entry(Lastkpi_crit).State = EntityState.Modified;
                    _context.SaveChanges();

                }

                kpi_cri.criteria_count = obj_kpi_crit.criteria_count;
                kpi_cri.is_active = 1;
                kpi_cri.company_id = obj_kpi_crit.company_id;
                kpi_cri.created_by = obj_kpi_crit.created_by;
                kpi_cri.created_date = DateTime.Now;
                kpi_cri.last_modified_by = obj_kpi_crit.created_by;
                kpi_cri.last_modified_date = DateTime.Now;

                _context.Entry(kpi_cri).State = EntityState.Added;
                _context.SaveChanges();



                objResult.StatusCode = 0;
                objResult.Message = "Criteria Level Save Successfully !!";
                return Ok(objResult);

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 1;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }

        }


        //get designation master data
        [Route("Get_KpiCriteria/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.KPIKeyAreaMaster))]
        public IActionResult Get_KpiCriteria([FromRoute]  int company_id)
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
                var data = _context.tbl_kpi_criteria.Where(a => a.company_id == company_id && a.is_active == 1).AsEnumerable().Select(a => new
                {
                    a.criteria_count,
                    a.kpi_cr_id,
                    a.description,
                    a.is_active,
                    a.created_by,
                    a.created_date,
                    a.company_id,
                }).FirstOrDefault();

                return Ok(data);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [Route("Save_KpiKeyAreaMaster")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.KPIKeyAreaMaster))]
        public IActionResult Save_KpiKeyAreaMaster([FromBody] KeyArea obj_kpi_area)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(obj_kpi_area.company_id ?? 0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {

                        var exist = _context.tbl_kpi_key_area_master.Where(a => a.company_id == obj_kpi_area.company_id && a.is_deleted == 0 && a.key_area == obj_kpi_area.key_area).FirstOrDefault();
                        if (exist != null)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "key Area already exist...!";
                            return Ok(objResult);
                        }
                        else if (obj_kpi_area.key_area.Contains("script") || obj_kpi_area.key_area.Contains("SCRIPT") || obj_kpi_area.expected_result.Contains("script") || obj_kpi_area.expected_result.Contains("SCRIPT"))
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Wrong Data...!";
                            return Ok(objResult);
                        }
                        else
                        {

                            tbl_kpi_key_area_master kpi_k_area = new tbl_kpi_key_area_master();
                            kpi_k_area.company_id = obj_kpi_area.company_id;
                            // kpi_k_area.f_year_id = obj_kpi_area.f_year_id;
                            kpi_k_area.w_r_id = obj_kpi_area.w_r_id;
                            kpi_k_area.otype_id = obj_kpi_area.otype_id;
                            kpi_k_area.key_area = obj_kpi_area.key_area;
                            kpi_k_area.expected_result = obj_kpi_area.expected_result;
                            kpi_k_area.wtg = obj_kpi_area.wtg;
                            kpi_k_area.created_by = obj_kpi_area.created_by;
                            kpi_k_area.created_date = DateTime.Now;
                            kpi_k_area.comment = obj_kpi_area.comment;

                            _context.Entry(kpi_k_area).State = EntityState.Added;
                            _context.SaveChanges();

                            if (obj_kpi_area.criteria_number.Count > 0)
                            {
                                List<tbl_kpi_criteria_master> obj_list_cri = new List<tbl_kpi_criteria_master>();


                                for (int i = 0; i < obj_kpi_area.criteria_number.Count; i++)
                                {
                                    tbl_kpi_criteria_master obj = new tbl_kpi_criteria_master()
                                    {
                                        k_a_id = kpi_k_area.key_area_id,
                                        criteria_number = obj_kpi_area.criteria_number[i],
                                        criteria = obj_kpi_area.criteria[i],
                                        is_deleted = 0,
                                        created_by = obj_kpi_area.created_by,
                                        created_date = DateTime.Now,
                                        last_modified_by = obj_kpi_area.created_by,
                                        last_modified_date = DateTime.Now,
                                    };

                                    obj_list_cri.Add(obj);
                                }

                                _context.tbl_kpi_criteria_master.AddRange(obj_list_cri);
                                _context.SaveChanges();

                            }

                            trans.Commit();
                            objResult.StatusCode = 0;
                            objResult.Message = "Key Area Save Successfully !!";
                            return Ok(objResult);
                        }
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
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }

        }


        [Route("Get_KpiKeyAreaMasterByCompany/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.KPIKeyAreaMaster))]
        public IActionResult Get_KpiKeyAreaMasterByCompany([FromRoute]  int company_id)
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

                var get_data = _context.tbl_kpi_key_area_master.Where(a => a.is_deleted == 0 && a.company_id == company_id).
                    Select(a => new { a.key_area_id,
                        a.tbl_w_role.working_role_name,
                        a.tbl_obj_type.objective_name,
                        a.key_area, a.expected_result,
                        a.wtg, a.created_date,
                    a.comment})
                    .ToList();

                return Ok(get_data);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("Get_KpiKeyAreaMaster/{kpi_area_id}")]
        [HttpGet]
        [Authorize(Policy =nameof(enmMenuMaster.KPIKeyAreaMaster))]
        public IActionResult Get_KpiKeyAreaMasterById([FromRoute]  int kpi_area_id)
        {
            try
            {

                var get_data = _context.tbl_kpi_criteria_master.Where(a => a.is_deleted == 0 && a.k_a_id == kpi_area_id).FirstOrDefault();

                return Ok(get_data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Get_KpiKeyAreaBYCriteriaId/{kpi_area_criteria_id}")]
        [HttpGet]
        [Authorize(Policy =nameof(enmMenuMaster.KPIKeyAreaMaster))]
        public IActionResult Get_KpiKeyAreaByCriteriaId([FromRoute]  int kpi_area_criteria_id)
        {
            try
            {
                var get_kpi_key_area = _context.tbl_kpi_key_area_master.Where(a => a.is_deleted == 0 && a.key_area_id == kpi_area_criteria_id).
                    Select(a => new { a.key_area_id, a.key_area, a.expected_result, a.wtg, a.otype_id, a.w_r_id, a.company_id, a.tbl_w_role.dept_id,a.comment }).FirstOrDefault();

                var get_criteria_data = _context.tbl_kpi_criteria_master.Where(a => a.is_deleted == 0 && a.k_a_id == kpi_area_criteria_id).ToList();

                return Ok(new { get_kpi_key_area, get_criteria_data, count = get_criteria_data.Count });
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Update_KpiKeyAreaMaster")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.KPIKeyAreaMaster))]
        public IActionResult Update_KpiKeyAreaMaster([FromBody] KeyArea obj_kpi_area)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(obj_kpi_area.company_id??0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var exist_or_not = _context.tbl_kpi_key_area_master.Where(x => x.key_area_id == obj_kpi_area.key_area_id).FirstOrDefault();

                        if (exist_or_not == null)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Invalid desgnation id please try again !!";
                            return Ok(objResult);
                        }
                        else
                        {
                            var exist = _context.tbl_kpi_key_area_master.Where(a => a.key_area_id != obj_kpi_area.key_area_id && a.company_id == obj_kpi_area.company_id && a.is_deleted == 0 && a.key_area == obj_kpi_area.key_area).FirstOrDefault();
                            if (exist != null)
                            {
                                objResult.StatusCode = 1;
                                objResult.Message = "key Area already exist...!";
                                return Ok(objResult);
                            }
                            else if (obj_kpi_area.key_area.Contains("script") || obj_kpi_area.key_area.Contains("SCRIPT") || obj_kpi_area.expected_result.Contains("script") || obj_kpi_area.expected_result.Contains("SCRIPT"))
                            {
                                objResult.StatusCode = 1;
                                objResult.Message = "Wrong Data...!";
                                return Ok(objResult);
                            }
                            else
                            {

                                exist_or_not.is_deleted = 1;
                                exist_or_not.last_modified_date = DateTime.Now;
                                exist_or_not.last_modified_by = obj_kpi_area.created_by;
                                _context.Entry(exist_or_not).State = EntityState.Modified;

                                tbl_kpi_key_area_master obj_key_ar = new tbl_kpi_key_area_master();

                                obj_key_ar.company_id = obj_kpi_area.company_id;
                                obj_key_ar.w_r_id = obj_kpi_area.w_r_id;
                                obj_key_ar.otype_id = obj_kpi_area.otype_id;
                                obj_key_ar.key_area = obj_kpi_area.key_area;
                                obj_key_ar.expected_result = obj_kpi_area.expected_result;
                                obj_key_ar.wtg = obj_kpi_area.wtg;
                                obj_key_ar.created_by = exist_or_not.created_by; //obj_kpi_area.created_by;
                                //obj_key_ar.created_date = DateTime.Now;
                                obj_key_ar.created_date = exist_or_not.created_date;
                                obj_key_ar.last_modified_by = obj_kpi_area.created_by;
                                obj_key_ar.last_modified_date = DateTime.Now;
                                obj_key_ar.comment = obj_kpi_area.comment;

                                _context.Entry(obj_key_ar).State = EntityState.Added;
                                _context.SaveChanges();

                                var get_criteria_data = _context.tbl_kpi_criteria_master.Where(a => a.is_deleted == 0 && a.k_a_id == obj_kpi_area.key_area_id).ToList();
                                for (int Index = 0; Index < get_criteria_data.Count; Index++)
                                {
                                    get_criteria_data[Index].is_deleted = 1;
                                    get_criteria_data[Index].last_modified_by = Convert.ToInt32(obj_kpi_area.created_by);
                                    get_criteria_data[Index].last_modified_date = DateTime.Now;
                                    _context.Entry(get_criteria_data[Index]).State = EntityState.Modified;
                                }

                                _context.tbl_kpi_criteria_master.UpdateRange(get_criteria_data);
                                _context.SaveChanges();

                                if (obj_kpi_area.criteria_number.Count > 0)
                                {
                                    List<tbl_kpi_criteria_master> obj_list_cri = new List<tbl_kpi_criteria_master>();


                                    for (int i = 0; i < obj_kpi_area.criteria_number.Count; i++)
                                    {
                                        tbl_kpi_criteria_master obj = new tbl_kpi_criteria_master()
                                        {
                                            k_a_id = obj_key_ar.key_area_id,
                                            criteria_number = obj_kpi_area.criteria_number[i],
                                            criteria = obj_kpi_area.criteria[i],
                                            is_deleted = 0,
                                            created_by = obj_kpi_area.created_by,
                                            created_date = DateTime.Now,
                                            last_modified_by = obj_kpi_area.created_by,
                                            last_modified_date = DateTime.Now,
                                        };

                                        obj_list_cri.Add(obj);
                                    }

                                    _context.tbl_kpi_criteria_master.AddRange(obj_list_cri);
                                    _context.SaveChanges();

                                }

                                trans.Commit();
                                objResult.StatusCode = 0;
                                objResult.Message = "Key Area Details Update Successfully !!";
                                return Ok(objResult);
                            }
                        }
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
                objResult.StatusCode = 1;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }

        }

        [HttpGet("Get_EmpDetailsForEpa/{emp_id}/{company_id}")]
        public async Task<IActionResult> Get_EmpDetailsForEpa([FromRoute] int emp_id,int company_id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access....!!";
                return Ok(objresponse);
            }
            if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access....!!";
                return Ok(objresponse);
            }
            

            var manager_data = _clsEmployeeDetail.Get_Emp_manager_dtl(emp_id).Select(p=>new {
                master1 = p.manager_name_code,
                master2 = p.m_two_name_code,
                master3 = p.m_three_name_code,
                final_approval=p.final_approval
            }).FirstOrDefault();



            //var manager_data = await _context.tbl_emp_manager.Where(a => a.employee_id == emp_id && a.is_deleted == 0).OrderByDescending(a => a.emp_mgr_id).Select(a => new {
            //    master1 = a.tbl_employee_master1.tbl_emp_officaial_sec.Where(b => b.employee_id == a.m_one_id && b.is_deleted == 0).Select(b => b.employee_first_name).FirstOrDefault(),
            //    master2 = a.tbl_employee_master2.tbl_emp_officaial_sec.Where(b => b.employee_id == a.m_two_id && b.is_deleted == 0).Select(b => b.employee_first_name).FirstOrDefault(),
            //    master3 = a.tbl_employee_master3.tbl_emp_officaial_sec.Where(b => b.employee_id == a.m_three_id && b.is_deleted == 0).Select(b => b.employee_first_name).FirstOrDefault(),
            //    a.final_approval }).FirstOrDefaultAsync();

            var get_emp_data = await _context.tbl_emp_officaial_sec.Where(a => a.employee_id == emp_id && a.is_deleted == 0).
                Select(a => new { a.date_of_joining }).FirstOrDefaultAsync();

            var get_desi_data = await _context.tbl_emp_desi_allocation.GroupBy(a=>a.employee_id).Select(p=>p.OrderByDescending(q=>q.emp_grade_id).FirstOrDefault()).Where(a=>a.employee_id==emp_id && a.applicable_from_date.Date<=DateTime.Now.Date && DateTime.Now.Date<=a.applicable_to_date.Date).Select(a => new { a.tbl_designation_master.designation_name }).FirstOrDefaultAsync();

            var get_grad_data = await _context.tbl_emp_grade_allocation.GroupBy(a=>a.employee_id).Select(p=>p.OrderByDescending(q=>q.emp_grade_id).FirstOrDefault()).Where(a => a.employee_id == emp_id && a.applicable_from_date.Date<=DateTime.Now.Date && DateTime.Now.Date<=a.applicable_to_date.Date).Select(a => new { a.tbl_grade_master.grade_name, a.applicable_from_date }).FirstOrDefaultAsync();

            if (get_emp_data == null)
            {
                return NotFound();
            }

            return Ok(new { manager_data, get_emp_data, get_desi_data, get_grad_data });
        }


        ////update designation master
        //[Route("Update_KpiCriteria")]
        //[HttpPost]
        //public IActionResult Update_KpiCriteria([FromBody] tbl_kpi_criteria obj_crit)
        //{
        //    Response_Msg objResult = new Response_Msg();

        //    try
        //    {
        //        var exist = _context.tbl_kpi_criteria.Where(x => x.kpi_cr_id == obj_crit.kpi_cr_id).FirstOrDefault();

        //        if (exist == null)
        //        {
        //            objResult.StatusCode = 1;
        //            objResult.Message = "Invalid desgnation id please try again !!";
        //            return Ok(objResult);
        //        }
        //        else
        //        {
        //            //check with same designation name in other designation id
        //            var duplicate = _context.tbl_kpi_criteria.Where(x => x.kpi_cr_id != obj_crit.kpi_cr_id && x.criteria_name.Trim().ToUpper() == obj_crit.criteria_name.Trim().ToUpper()).FirstOrDefault();
        //            if (duplicate != null)
        //            {
        //                objResult.StatusCode = 1;
        //                objResult.Message = "Criteria exist in the system please check & try !!";
        //                return Ok(objResult);
        //            }

        //            exist.criteria_name = obj_crit.criteria_name;
        //            exist.description = obj_crit.description;
        //            exist.is_active = obj_crit.is_active;
        //            exist.last_modified_by = obj_crit.created_by;
        //            exist.last_modified_date = DateTime.Now;

        //            _context.Entry(exist).State = EntityState.Modified;
        //            _context.SaveChanges();

        //            objResult.StatusCode = 0;
        //            objResult.Message = "Criteria Updated Successfully !!";
        //            return Ok(objResult);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        objResult.StatusCode = 0;
        //        objResult.Message = ex.Message;
        //        return Ok(objResult);
        //    }
        //}


        //    //get Financial Year list 
        //    [Route("Get_FinancialList")]

        //    ////[Authorize(Policy = "70")]
        //    public IActionResult Get_FinancialList()
        //    {
        //        try
        //        {
        //            var data = (from a in _context.tbl_epa_fiscal_yr_mstr.Where(b => b.isview == 1)
        //                        select new
        //                        {
        //                            financial_year_Name = a.financial_year_name,
        //                            fiscal_year_id = a.fiscal_year_id
        //                        }).ToList();

        //            return Ok(data);
        //        }
        //        catch (Exception ex)
        //        {
        //            return Ok(ex.Message);
        //        }
        //    }

        //    //save Quarter Master
        //    [Route("Save_QuaterMaster")]
        //    [HttpPost]
        //    // add
        //    public IActionResult Save_QuaterMaster([FromBody] tbl_epa_quarter_master objquarter)
        //    {
        //        Response_Msg objResult = new Response_Msg();
        //        try
        //        {
        //            var exist = _context.tbl_epa_quarter_master.Where(x => x.quarter_name == objquarter.quarter_name && x.fiscal_year_id == objquarter.fiscal_year_id).FirstOrDefault();

        //            if (exist != null)
        //            {
        //                objResult.StatusCode = 0;
        //                objResult.Message = "Quarter already exist !!";
        //                return Ok(objResult);
        //            }
        //            else
        //            {
        //                objquarter.quarter_name = objquarter.quarter_name;
        //                objquarter.fiscal_year_id = objquarter.fiscal_year_id;
        //                objquarter.start_date = objquarter.start_date;
        //                objquarter.enddate = objquarter.enddate;
        //                objquarter.created_date = DateTime.Now;
        //                objquarter.is_deleted = 0; //Convert.ToByte(objfinancial.is_deleted);
        //                objquarter.created_by = 1;                                  
        //                _context.Entry(objquarter).State = EntityState.Added;
        //                _context.SaveChanges();

        //                objResult.StatusCode = 0;
        //                objResult.Message = "Quarter Master save successfully !!";
        //                return Ok(objResult);
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            objResult.StatusCode = 0;
        //            objResult.Message = ex.Message;
        //            return Ok(objResult);
        //        }
        //    }

        //    //get Quarter Master  Data
        //    [Route("Get_QuarterMasterData/{quarter_id}")]
        //    [HttpGet]
        //    public IActionResult Get_QuarterMasterData([FromRoute] int quarter_id)
        //    {
        //        try
        //        {
        //            if (quarter_id > 0)
        //            {
        //                var data = _context.tbl_epa_quarter_master.Where(x => x.quarter_id == quarter_id).FirstOrDefault();
        //                return Ok(data);
        //            }
        //            else
        //            {
        //                var result = (from a in _context.tbl_epa_quarter_master
        //                              join b in _context.tbl_epa_fiscal_yr_mstr on a.fiscal_year_id equals b.fiscal_year_id                                 
        //                              select new
        //                              {
        //                                 sno = a.quarter_id,
        //                                  quarter = a.quarter_name,
        //                                 financialyear= b.financial_year_name,
        //                                 startdate = a.start_date,
        //                                 to_date = a.enddate                                     
        //                             }).ToList();

        //                return Ok(result);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return Ok(ex.Message);
        //        }
        //    }

        //    //update Quarte rMaster
        //    [Route("Update_QuarterMaster")]
        //    [HttpPut]
        //    ////[Authorize(Policy = "69")]
        //    public IActionResult Update_QuarterMaster([FromBody] tbl_epa_quarter_master objquarter)
        //    {
        //        Response_Msg objResult = new Response_Msg();

        //        try
        //        {
        //            var exist = _context.tbl_epa_quarter_master.Where(x => x.quarter_id == objquarter.quarter_id).FirstOrDefault();

        //            if (exist == null)
        //            {
        //                objResult.StatusCode = 1;
        //                objResult.Message = "Invalid Quarter id please try again !!";
        //                return Ok(objResult);
        //            }
        //            else
        //            {
        //                //check with same grade name in other greade id
        //                var duplicate = _context.tbl_epa_quarter_master.Where(x => x.quarter_id != objquarter.quarter_id && x.quarter_name == objquarter.quarter_name).FirstOrDefault();
        //                if (duplicate != null)
        //                {
        //                    objResult.StatusCode = 1;
        //                    objResult.Message = "Quarter exist in the system please check & try !!";
        //                    return Ok(objResult);
        //                }

        //                exist.quarter_name = objquarter.quarter_name;
        //                exist.fiscal_year_id = objquarter.fiscal_year_id;
        //                exist.start_date = objquarter.start_date;
        //                exist.enddate = objquarter.enddate;
        //                exist.is_deleted = 0;
        //                exist.last_modified_by = 1;
        //                exist.last_modified_date = DateTime.Now;

        //                _context.Entry(exist).State = EntityState.Modified;
        //                _context.SaveChanges();

        //                objResult.StatusCode = 0;
        //                objResult.Message = "Quarter Master updated successfully !!";
        //                return Ok(objResult);
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            objResult.StatusCode = 0;
        //            objResult.Message = ex.Message;
        //            return Ok(objResult);
        //        }
        //    }
        //    //save Tab Master
        //    [Route("Save_TabMaster")]
        //    [HttpPost]
        //    // add
        //    public IActionResult Save_TabMaster([FromBody] tbl_epa_tab_mstr objtab)
        //    {
        //        Response_Msg objResult = new Response_Msg();
        //        try
        //        {
        //            var exist = _context.tbl_epa_tab_mstr.Where(x => x.tab_name == objtab.tab_name).FirstOrDefault();

        //            if (exist != null)
        //            {
        //                objResult.StatusCode = 0;
        //                objResult.Message = "Tab Name already exist !!";
        //                return Ok(objResult);
        //            }
        //            else
        //            {
        //                objtab.tab_name = objtab.tab_name;
        //                objtab.description = objtab.description;
        //                objtab.order = objtab.order;
        //                objtab.is_deleted = 0; //Convert.ToByte(objfinancial.is_deleted);
        //                objtab.created_by = objtab.created_by;
        //                objtab.created_date = DateTime.Now;
        //                objtab.is_view = objtab.is_view;
        //                objtab.have_save_button = objtab.have_save_button;
        //                _context.Entry(objtab).State = EntityState.Added;
        //                _context.SaveChanges();

        //                objResult.StatusCode = 0;
        //                objResult.Message = "Tab name save successfully !!";
        //                return Ok(objResult);
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            objResult.StatusCode = 0;
        //            objResult.Message = ex.Message;
        //            return Ok(objResult);
        //        }
        //    }

        //    //update Tab Master
        //    [Route("Update_TabMaster")]
        //    [HttpPut]
        //    ////[Authorize(Policy = "69")]
        //    public IActionResult Update_TabMaster([FromBody] tbl_epa_tab_mstr objTab)
        //    {
        //        Response_Msg objResult = new Response_Msg();

        //        try
        //        {
        //            var exist = _context.tbl_epa_tab_mstr.Where(x => x.tab_id == objTab.tab_id).FirstOrDefault();

        //            if (exist == null)
        //            {
        //                objResult.StatusCode = 1;
        //                objResult.Message = "Invalid Tab id please try again !!";
        //                return Ok(objResult);
        //            }
        //            else
        //            {
        //                //check with same grade name in other greade id
        //                var duplicate = _context.tbl_epa_quarter_master.Where(x => x.quarter_id != objTab.tab_id && x.quarter_name == objTab.tab_name).FirstOrDefault();
        //                if (duplicate != null)
        //                {
        //                    objResult.StatusCode = 1;
        //                    objResult.Message = "Tab name exists in the system, please try another name !!";
        //                    return Ok(objResult);
        //                }

        //                exist.tab_name = objTab.tab_name;
        //                exist.tab_id = objTab.tab_id;
        //                exist.description = objTab.description;
        //                exist.is_view = objTab.is_view;
        //                exist.is_deleted = 0;
        //                exist.last_modified_by = objTab.last_modified_by;
        //                exist.last_modified_date = DateTime.Now;

        //                _context.Entry(exist).State = EntityState.Modified;
        //                _context.SaveChanges();

        //                objResult.StatusCode = 0;
        //                objResult.Message = "Tab name updated successfully !!";
        //                return Ok(objResult);
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            objResult.StatusCode = 0;
        //            objResult.Message = ex.Message;
        //            return Ok(objResult);
        //        }
        //    }

        //    //get Tab master data
        //    [Route("Get_TabMasterData/{tab_id}")]
        //    [HttpGet]
        //    public IActionResult Get_TabMasterData([FromRoute] int tab_id)
        //    {
        //        try
        //        {
        //            if (tab_id > 0)
        //            {
        //                var data = _context.tbl_epa_tab_mstr.Where(x => x.tab_id == tab_id).FirstOrDefault();
        //                return Ok(data);
        //            }
        //            else
        //            {

        //                var result = _context.tbl_epa_tab_mstr.Join(_context.tbl_employee_master, a => a.created_by, b => b.created_by, (a, b) => new
        //                {
        //                    a.tab_id,
        //                    a.tab_name,
        //                    a.description,
        //                    a.have_save_button ,
        //                    a.is_view,
        //                    a.created_by,
        //                }).ToList();

        //                return Ok(result);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return Ok(ex.Message);
        //        }
        //    }

        //    // GET: api/apiMasters/GetQuarterList/0/5
        //    [Route("GetQuarterList/{quarterId}/{fyi_Id}")]
        //    [HttpGet]        
        //    ////[Authorize(Policy = "61")]
        //    public async Task<IActionResult> GetQuarterList([FromRoute] int quarterId, int fyi_Id)
        //    {
        //        try
        //        {

        //            if (quarterId > 0 && fyi_Id == 0)
        //            {
        //                var result = (from a in _context.tbl_epa_quarter_master.Where(x => x.quarter_id == quarterId) select a).ToList();

        //                return Ok(result);
        //            }
        //            else if (fyi_Id > 0)
        //            {
        //                var result = (from a in _context.tbl_epa_quarter_master.Where(x => x.fiscal_year_id == fyi_Id) select a).ToList();

        //                return Ok(result);
        //            }
        //            else
        //            {
        //                var result = _context.tbl_epa_quarter_master.Join(_context.tbl_epa_fiscal_yr_mstr, a => a.fiscal_year_id, b => b.fiscal_year_id, (a, b) => new
        //                {
        //                    a.quarter_id,
        //                    a.quarter_name,                       
        //                }).ToList();

        //                return Ok(result);
        //            }


        //        }
        //        catch (Exception ex)
        //        {
        //            return Ok(ex.Message);
        //        }
        //    }


        #region ** START BY SUPRIYA ON 09-01-2020**

        [Route("Save_StatusMaster")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.EPAStatusMaster))]
        public IActionResult Save_StatusMaster([FromBody] tbl_epa_status_master epastatus)
        {
            Response_Msg objresponse = new Response_Msg();
            try
            {
                var exist = _context.tbl_epa_status_master.Where(x => x.company_id == epastatus.company_id && x.status_name.Trim().ToUpper() == epastatus.status_name.Trim().ToUpper() && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Status Master Already exist";
                }
                else
                {
                    epastatus.is_deleted = 0;
                    epastatus.created_date = DateTime.Now;
                    epastatus.last_modified_by = 0;
                    epastatus.last_modified_date = Convert.ToDateTime("01-01-2000");


                    _context.Entry(epastatus).State = EntityState.Added;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Status Master Successfully Saved";
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

        [Route("Get_StatusMaster/{company_id}/{epa_status_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.EPAStatusMaster))]
        public IActionResult Get_StatusMaster(int company_id, int epa_status_id)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access..!!";
                return Ok(objresponse);
            }
            try
            {
                if (epa_status_id == 0)
                {
                    if (company_id > 0)
                    {
                        var data = _context.tbl_epa_status_master.Where(x => x.is_deleted == 0 && x.company_id == company_id).Select(p => new
                        {
                            p.epa_status_id,
                            p.company_id,
                            p.comp_master.company_name,
                            p.status_name,
                            p.description,
                            p.display_for,
                            p.display_for_rm1,
                            p.display_for_rm2,
                            p.display_for_rm3,
                            display_rm1 = p.display_for_rm1 == 1 ? "Yes" : "No",
                            display_rm2 = p.display_for_rm2 == 1 ? "Yes" : "No",
                            display_rm3 = p.display_for_rm3 == 1 ? "Yes" : "No",
                            display_user = p.display_for == 1 ? "Yes" : "No",
                            //display_for_name = p.display_for == 1 ? "Reporting Manger(RM) 1" : p.display_for == 2 ? "Reporting Manger(RM) 2" : p.display_for == 3 ? "Reporting Manger(RM) 3" : p.display_for == 4 ? "User" : "",
                            // p.display_order,
                            // order_name = p.display_order == 1 ? "Name" : p.display_order == 2 ? "Status" : p.display_order == 3 ? "Display For" : "",
                            p.status,
                            radio_status = p.status == 0 ? "Start" : p.status == 1 ? "Closed" : p.status == 2 ? "Middle Level" : "",
                            p.created_date,
                            p.last_modified_date

                        }).ToList();

                        return Ok(data);
                    }
                    else
                    {
                        var data = _context.tbl_epa_status_master.Where(x => x.is_deleted == 0).Select(p => new
                        {
                            p.epa_status_id,
                            p.company_id,
                            p.comp_master.company_name,
                            p.status_name,
                            p.description,
                            p.display_for,
                            p.display_for_rm1,
                            p.display_for_rm2,
                            p.display_for_rm3,
                            display_rm1 = p.display_for_rm1 == 1 ? "Yes" : "No",
                            display_rm2 = p.display_for_rm2 == 1 ? "Yes" : "No",
                            display_rm3 = p.display_for_rm3 == 1 ? "Yes" : "No",
                            display_user = p.display_for == 1 ? "Yes" : "No",
                            // display_for_name = p.display_for == 1 ? "Reporting Manger(RM) 1" : p.display_for == 2 ? "Reporting Manger(RM) 2" : p.display_for == 3 ? "Reporting Manger(RM) 3" : p.display_for == 4 ? "User" : "",
                            // p.display_order,
                            // order_name = p.display_order == 1 ? "Name" : p.display_order == 2 ? "Status" : p.display_order == 3 ? "Display For" : "",
                            p.status,
                            radio_status = p.status == 0 ? "Start" : p.status == 1 ? "Closed" : p.status == 2 ? "Middle Level" : "",
                            p.created_date,
                            p.last_modified_date

                        }).ToList();

                        return Ok(data);
                    }
                }
                else
                {
                    var data = _context.tbl_epa_status_master.Where(x => x.is_deleted == 0 && x.company_id == company_id && x.epa_status_id == epa_status_id).Select(p => new
                    {
                        p.epa_status_id,
                        p.company_id,
                        p.comp_master.company_name,
                        p.status_name,
                        p.description,
                        p.display_for,
                        p.display_for_rm1,
                        p.display_for_rm2,
                        p.display_for_rm3,
                        display_rm1 = p.display_for_rm1 == 1 ? "Yes" : "No",
                        display_rm2 = p.display_for_rm2 == 1 ? "Yes" : "No",
                        display_rm3 = p.display_for_rm3 == 1 ? "Yes" : "No",
                        display_user = p.display_for == 1 ? "Yes" : "No",
                        // display_for_name = p.display_for == 1 ? "Reporting Manger(RM) 1" : p.display_for == 2 ? "Reporting Manger(RM) 2" : p.display_for == 3 ? "Reporting Manger(RM) 3" : p.display_for == 4 ? "User" : "",
                        // p.display_order,
                        // order_name = p.display_order == 1 ? "Name" : p.display_order == 2 ? "Status" : p.display_order == 3 ? "Display For" : "",
                        p.status,
                        radio_status = p.status == 0 ? "Start" : p.status == 1 ? "Closed" : p.status == 2 ? "Middle Level" : "",
                        p.created_date,
                        p.last_modified_date

                    }).FirstOrDefault();

                    return Ok(data);
                }






            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("Update_StatusMaster")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.EPAStatusMaster))]
        public IActionResult Update_StatusMaster([FromBody] tbl_epa_status_master statusmaster)
        {
            Response_Msg objresponse = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(statusmaster.company_id??0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }

            try
            {
                var exist = _context.tbl_epa_status_master.Where(x => x.epa_status_id == statusmaster.epa_status_id && x.company_id == statusmaster.company_id && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    var duplicate = _context.tbl_epa_status_master.Where(x => x.epa_status_id != statusmaster.epa_status_id && x.company_id == statusmaster.company_id && x.status_name.Trim().ToUpper() == statusmaster.status_name.Trim().ToUpper() && x.is_deleted == 0).FirstOrDefault();
                    if (duplicate != null)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Status Master Already Exist";
                    }
                    else
                    {
                        using (var trans = _context.Database.BeginTransaction()) {

                            try
                            {
                                exist.is_deleted = 1;
                                exist.last_modified_by = statusmaster.last_modified_by;
                                exist.last_modified_date = DateTime.Now;

                                _context.Entry(exist).State = EntityState.Modified;

                                tbl_epa_status_master objmstr = new tbl_epa_status_master();

                                objmstr.company_id = statusmaster.company_id;
                                objmstr.status_name = statusmaster.status_name.Trim();
                                objmstr.description = statusmaster.description;
                                objmstr.display_for = statusmaster.display_for;
                                objmstr.display_for_rm1 = statusmaster.display_for_rm1;
                                objmstr.display_for_rm2 = statusmaster.display_for_rm2;
                                objmstr.display_for_rm3 = statusmaster.display_for_rm3;
                                objmstr.status = statusmaster.status;
                                objmstr.is_deleted = 0;
                                objmstr.created_by = exist.created_by;
                                objmstr.created_date = exist.created_date;
                                objmstr.last_modified_by = statusmaster.last_modified_by;
                                objmstr.last_modified_date = DateTime.Now;


                                _context.tbl_epa_status_master.AddRange(objmstr);

                                int new_staus_id = objmstr.epa_status_id;
                                var exist_status_flow = _context.tbl_status_flow_master.Where(x => x.is_deleted == 0 && (x.next_status_id == statusmaster.epa_status_id || x.start_status_id == statusmaster.epa_status_id)).ToList();

                                for (int i = 0; i < exist_status_flow.Count; i++)
                                {
                                    if (exist_status_flow[i].start_status_id == statusmaster.epa_status_id)
                                    {
                                        exist_status_flow[i].start_status_id = new_staus_id;
                                    }

                                    if (exist_status_flow[i].next_status_id == statusmaster.epa_status_id)
                                    {
                                        exist_status_flow[i].next_status_id = new_staus_id;
                                    }
                                }

                                //exist_status_flow.ForEach(p => { (p.next_status_id == statusmaster.epa_status_id) ? p.next_status_id = new_staus_id : (p.start_status_id == statusmaster.epa_status_id) ? p.start_status_id = new_staus_id : p.is_deleted=0; });

                                _context.tbl_status_flow_master.UpdateRange(exist_status_flow);


                                _context.SaveChanges();



                                trans.Commit();

                                objresponse.StatusCode = 0;
                                objresponse.Message = "Status Master Successfully Update";
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();

                                objresponse.StatusCode = 1;
                                objresponse.Message = ex.Message;
                            }
                        }


                            
                        //exist.status_name = statusmaster.status_name.Trim();
                        //exist.description = statusmaster.description;
                        //exist.display_for = statusmaster.display_for;
                        //exist.display_for_rm1 = statusmaster.display_for_rm1;
                        //exist.display_for_rm2 = statusmaster.display_for_rm2;
                        //exist.display_for_rm3 = statusmaster.display_for_rm3;
                        ////exist.display_order = statusmaster.display_order;
                        //exist.status = statusmaster.status;
                        //exist.last_modified_by = statusmaster.last_modified_by;
                        //exist.last_modified_date = DateTime.Now;


                       // _context.Entry(exist).State = EntityState.Modified;
                       // _context.SaveChanges();

                        
                    }
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Deltails";
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

        //public IActionResult Update_StatusMaster([FromBody] epa_StatusMaster statusmaster)
        //{
        //    Response_Msg objrespsonse = new Response_Msg();
        //    try
        //    {
        //        var exist = _context.tbl_epa_status_master.Where(x => x.epa_status_id == statusmaster.epa_status_id && x.company_id==statusmaster.company_id && x.status_name.Trim().ToUpper()==statusmaster.status_name.Trim().ToUpper()).FirstOrDefault();
        //        if (exist != null)
        //        {
        //            //var duplicate = _context.tbl_epa_status_master.Where(x =>x.epa_status_id!=statusmaster.epa_status_id && x.company_id == statusmaster.company_id && x.status_name.Trim().ToUpper() == statusmaster.status_name.Trim().ToUpper()).FirstOrDefault();


        //            for (int i = 0; i < statusmaster.display_for.Count; i++)
        //            {
        //                var duplicate = _context.tbl_epa_status_master.Where(x => x.epa_status_id != statusmaster.epa_status_id && x.company_id == statusmaster.company_id && x.display_for == Convert.ToInt32(statusmaster.display_for[i])).FirstOrDefault();
        //                if (duplicate != null)
        //                {
        //                    objrespsonse.StatusCode = 1;
        //                    string statusname = Convert.ToInt32(statusmaster.display_for[i]) == 1 ? "RM1" : Convert.ToInt32(statusmaster.display_for[i]) == 2 ? "RM2" : Convert.ToInt32(statusmaster.display_for[i]) == 3 ? "RM3" : Convert.ToInt32(statusmaster.display_for[i]) == 4 ? "User" : "";

        //                    objrespsonse.Message = "Status Alredy Exist for " + statusname + ",Please Delete previous record...";
        //                    return Ok(objrespsonse);
        //                }
        //                else
        //                {
        //                    if (exist.display_for == Convert.ToInt32(statusmaster.display_for[i]))
        //                    {
        //                        exist.description = statusmaster.description;
        //                        exist.display_order = statusmaster.display_order;
        //                        exist.display_for = Convert.ToInt32(statusmaster.display_for[i]);
        //                        exist.status = statusmaster.status;
        //                        exist.last_modified_by = statusmaster.last_modified_by;
        //                        exist.last_modified_date = DateTime.Now;


        //                        _context.tbl_epa_status_master.UpdateRange(exist);
        //                    }
        //                    else
        //                    {

        //                            tbl_epa_status_master objtbl = new tbl_epa_status_master();

        //                            objtbl.company_id = statusmaster.company_id;
        //                            objtbl.status_name = statusmaster.status_name;
        //                            objtbl.description = statusmaster.description;
        //                            objtbl.display_for = Convert.ToInt32(statusmaster.display_for[i]);
        //                            objtbl.display_order = statusmaster.display_order;
        //                            objtbl.status = statusmaster.status;
        //                            objtbl.is_deleted = 0;
        //                            objtbl.created_by = statusmaster.created_by;
        //                            objtbl.created_date = DateTime.Now;
        //                            objtbl.last_modified_by = 0;
        //                            objtbl.last_modified_date = Convert.ToDateTime("01-01-2000");


        //                            _context.tbl_epa_status_master.AddRange(objtbl);

        //                    }

        //                }
        //            }


        //            _context.SaveChanges();

        //            objrespsonse.StatusCode = 0;
        //                objrespsonse.Message = "Status Successfully Saved";
        //            }
        //        else
        //        {
        //            objrespsonse.StatusCode = 1;
        //            objrespsonse.Message = "Invalid Details";
        //        }
        //        return Ok(objrespsonse);
        //    }
        //    catch (Exception ex)
        //    {
        //        objrespsonse.StatusCode = 1;
        //        objrespsonse.Message = ex.Message;
        //        return Ok(objrespsonse);
        //    }
        //}

        [Route("Delete_StatusMaster/{companyid}/{status_master_id}/{login_empid}")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.EPAStatusMaster))]
        public IActionResult Delete_StatusMaster(int companyid, int status_master_id, int login_empid)
        {
            Response_Msg obresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(companyid))
            {
                obresponse.StatusCode = 1;
                obresponse.Message = "Unauthorize Company Access...!!";
                return Ok(obresponse);
            }
            try
            {
                var exist = _context.tbl_epa_status_master.Where(x => x.epa_status_id == status_master_id && x.company_id == companyid && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    exist.is_deleted = 1;
                    exist.last_modified_by = login_empid;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    obresponse.StatusCode = 0;
                    obresponse.Message = "Selected Staus Master Successfully Deleted";
                }
                else
                {
                    obresponse.StatusCode = 1;
                    obresponse.Message = "Invalid Detail";
                }
                return Ok(obresponse);
            }
            catch (Exception ex)
            {
                obresponse.StatusCode = 2;
                obresponse.Message = ex.Message;
                return Ok(obresponse);
            }
        }

        [Route("Save_KPIObjective")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.KPIObjective))]
        public IActionResult Save_KPIObjective([FromBody] tbl_kpi_objective_type objtbl)
        {
            Response_Msg objresponse = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(objtbl.company_id??0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company access...!!";
                return Ok(objresponse);
            }
            try
            {
                var exist = _context.tbl_kpi_objective_type.Where(x => x.company_id == objtbl.company_id && x.is_deleted == 0 && x.objective_name.Trim().ToUpper() == objtbl.objective_name.Trim().ToUpper()).FirstOrDefault();
                if (exist != null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Objective Type Already Exist";
                }
                else
                {
                    objtbl.is_deleted = 0;
                    objtbl.deleted_by = 0;
                    objtbl.created_date = DateTime.Now;
                    objtbl.last_modified_by = 0;
                    objtbl.last_modified_date = Convert.ToDateTime("01-01-2000");

                    _context.Entry(objtbl).State = EntityState.Added;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Objective Type Successfully Saved...";
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

        [Route("Get_KPIObjective/{companyid}/{objective_type_id}")]
        [HttpGet]
        [Authorize(Policy =nameof(enmMenuMaster.EPA))]
        public IActionResult Get_KPIObjective(int companyid, int objective_type_id)
        {
            Response_Msg objresponse = new Response_Msg();
            try
            {
                if (objective_type_id == 0)
                {
                    if (companyid > 0)
                    {
                        var data = _context.tbl_kpi_objective_type.Where(x => x.is_deleted == 0 && x.company_id == companyid && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).Select(p => new
                        {
                            p.company_id,
                            p.obj_type_id,
                            p.comp_master.company_name,
                            p.objective_name,
                            p.description,
                            p.created_date,
                            p.last_modified_date
                        }).ToList();
                        return Ok(data);
                    }
                    else
                    {
                        var data = _context.tbl_kpi_objective_type.Where(x => x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).Select(p => new
                        {
                            p.company_id,
                            p.obj_type_id,
                            p.comp_master.company_name,
                            p.objective_name,
                            p.description,
                            p.created_date,
                            p.last_modified_date
                        }).ToList();
                        return Ok(data);
                    }
                }
                else
                {

                    var data = _context.tbl_kpi_objective_type.Where(x => x.is_deleted == 0 && x.obj_type_id == objective_type_id && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).Select(p => new
                    {
                        p.company_id,
                        p.obj_type_id,
                        p.comp_master.company_name,
                        p.objective_name,
                        p.description,
                        p.created_date,
                        p.last_modified_date
                    }).FirstOrDefault();
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("Delete_KPIobjectType/{companyid}/{objective_type_id}/{loginemp}")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.KPIObjective))]
        public IActionResult Delete_KPIobjectType(int companyid, int objective_type_id, int loginemp)
        {
            Response_Msg objresponse = new Response_Msg();
            if(!_clsCurrentUser.CompanyId.Contains(companyid))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }
            try
            {
                var exist = _context.tbl_kpi_objective_type.Where(x => x.is_deleted == 0 && x.company_id == companyid && x.obj_type_id == objective_type_id).FirstOrDefault();
                if (exist != null)
                {
                    exist.is_deleted = 1;
                    exist.deleted_by = loginemp;
                    exist.last_modified_date = DateTime.Now;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Objective Type successfully Deleted";

                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Detail";
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

        [Route("Update_KPIObjective")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.KPIObjective))]
        public IActionResult Update_KPIObjective([FromBody] tbl_kpi_objective_type objtbl)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(objtbl.company_id??0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }
            try
            {
                var exist = _context.tbl_kpi_objective_type.Where(x => x.is_deleted == 0 && x.company_id == objtbl.company_id && x.obj_type_id == objtbl.obj_type_id).FirstOrDefault();
                if (exist != null)
                {
                    var duplicate = _context.tbl_kpi_objective_type.Where(x => x.obj_type_id != objtbl.obj_type_id && x.company_id == objtbl.company_id && x.objective_name.Trim().ToUpper() == objtbl.objective_name.Trim().ToUpper() && x.is_deleted == 0).FirstOrDefault();
                    if (duplicate != null)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Objective Type Already Exist";
                    }
                    else
                    {
                        exist.is_deleted = 1;
                        exist.last_modified_by = objtbl.last_modified_by;
                        exist.last_modified_date = DateTime.Now;

                        _context.Entry(exist).State = EntityState.Modified;

                        tbl_kpi_objective_type objkpi = new tbl_kpi_objective_type();

                        objkpi.company_id = objtbl.company_id;
                        objkpi.objective_name = objtbl.objective_name.Trim();
                        objkpi.description = objtbl.description;
                        objkpi.is_deleted = 0;
                        objkpi.deleted_by = 0;
                        objkpi.last_modified_by = objtbl.last_modified_by;
                        objkpi.last_modified_date = DateTime.Now;
                        objkpi.created_by = exist.created_by;
                        objkpi.created_date = exist.created_date;

                        _context.Entry(objkpi).State = EntityState.Added;

                        //exist.objective_name = objtbl.objective_name;
                        //exist.description = objtbl.description;
                        //exist.last_modified_by = objtbl.last_modified_by;
                        //exist.last_modified_date = DateTime.Now;

                        //_context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();


                        objresponse.StatusCode = 0;
                        objresponse.Message = "KPI Objective Type Successfully Updated";
                    }
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Detail";
                }

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(ex.Message);
            }
        }

        [Route("BindStatusMasterForFlow/{company_id}/{status_id}")]
        [HttpGet]
        [Authorize(Policy =nameof(enmMenuMaster.EPAStatusMaster))]
        public IActionResult BindStatusMasterForFlow(int company_id, int status_id)
        {
            Response_Msg objrespone = new Response_Msg();
            try
            {
                if (company_id > 0)
                {
                    //select only start
                    //if (status_id== 0)
                    //{
                    var data = _context.tbl_epa_status_master.Where(x => x.company_id == company_id && x.is_deleted == 0 && (x.status == 0 || x.status == 2) && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).ToList();
                    return Ok(data);
                    //}
                    //else
                    //{
                    //    var data = _context.tbl_epa_status_master.Where(x => x.company_id == company_id && x.status != 0 && x.is_deleted == 0).ToList();
                    //    return Ok(data);
                    //}
                }
                else
                {
                    var data = _context.tbl_epa_status_master.Where(x => x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).ToList();
                    return Ok(data); 
                }
            }
            catch (Exception ex)
            {
                objrespone.StatusCode = 2;
                objrespone.Message = ex.Message;
                return Ok(objrespone);
            }
        }

        [Route("Save_StatusFlowMaster")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.EPAStatusFlow))]
        public IActionResult Save_StatusFlowMaster([FromBody] epa_status_flow_master objflowmaster)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(objflowmaster.company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access....!!";
                return Ok(objresponse);
            }
            try
            {
                //var check_next_status_id = _context.tbl_epa_status_master.Where(x => x.company_id == objflowmaster.company_id && x.is_deleted == 0 && x.status!=0 && objflowmaster.next_status_id.Contains(x.epa_status_id)).ToList();

                var check_start_status = _context.tbl_epa_status_master.Where(x => x.company_id == objflowmaster.company_id && x.epa_status_id == objflowmaster.start_status_id && x.is_deleted == 0 && (x.status == 0||x.status==2)).FirstOrDefault();

                if (check_start_status != null)
                {
                    using (var trans = _context.Database.BeginTransaction())
                    {

                        try
                        {
                            var exist_details = _context.tbl_status_flow_master.Where(x => x.status_master.company_id == objflowmaster.company_id && x.start_status_id == objflowmaster.start_status_id && x.is_deleted == 0).ToList();
                            if (exist_details.Count > 0)
                            {
                                exist_details.ForEach(q => { q.is_deleted = 1; q.deleted_by = objflowmaster.created_by; q.modified_date = DateTime.Now; });

                                _context.tbl_status_flow_master.UpdateRange(exist_details);
                            }

                            for (int i = 0; i < objflowmaster.next_status_id.Count; i++)
                            {
                                tbl_status_flow_master objtbl = new tbl_status_flow_master();
                                objtbl.start_status_id = objflowmaster.start_status_id;
                                objtbl.next_status_id = Convert.ToInt32(objflowmaster.next_status_id[i]);
                                objtbl.description = objflowmaster.description;
                                objtbl.is_deleted = 0;
                                objtbl.deleted_by = 0;
                                objtbl.created_by = objflowmaster.created_by;
                                objtbl.created_dt = DateTime.Now;
                                objtbl.modified_by = 0;
                                objtbl.modified_date = Convert.ToDateTime("01-01-2000");
                                _context.tbl_status_flow_master.AddRange(objtbl);

                            }

                            _context.SaveChanges();

                            trans.Commit();
                            objresponse.StatusCode = 0;
                            objresponse.Message = "Status Flow Master Successfully Saved";
                        }
                        catch (Exception ex)
                        {
                            objresponse.StatusCode = 1;
                            objresponse.Message = ex.Message;
                        }
                    }


                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Detils";
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

        [Route("Get_StatusFlowMaster/{companyid}/{statusmasterid}")]
        [HttpGet]
        [Authorize(Policy =nameof(enmMenuMaster.EPAStatusFlow))]
        public IActionResult Get_StatusFlowMaster(int companyid, int statusmasterid)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(companyid))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }
            try
            {
                var data = _context.tbl_status_flow_master.Where(x => x.is_deleted == 0 && x.start_status_id == statusmasterid).ToList();

                var result = (from e in _context.tbl_epa_status_master
                              join d in data on e.epa_status_id equals d.next_status_id into ej
                              from d in ej.DefaultIfEmpty()
                              where e.is_deleted == 0 && e.status != 0 && e.company_id==companyid
                              select new
                              {
                                  company_id = e.company_id,
                                  e.epa_status_id,
                                  e.status_name,
                                  description = d == null ? "-" : d.description,
                                  start_statusid = d == null ? 0 : d.start_status_id,
                                  start_status = d == null ? "-" : d.status_master.status_name,
                                  nextstatus_id = d == null ? 0 : d.next_status_id,
                                  next_statusname = d == null ? "-" : d.next_status.status_name,
                                  created_dt = d == null ? Convert.ToDateTime("01-01-2000") : d.created_dt,
                                  modified_dt = d == null ? Convert.ToDateTime("01-01-2000") : d.modified_date,
                                  flow_id = d == null ? 0 : d.flow_mster_id
                              }).ToList();


                return Ok(result);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("Delete_StatusFlowMaster/{company_id}/{flow_id}/{loginid}")]
        [Authorize(Policy =nameof(enmMenuMaster.EPAStatusFlow))]
        public IActionResult Delete_StatusFlowMaster(int company_id, int flow_id, int loginid)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access..!!";
                return Ok(objresponse);
            }
            if (!_clsCurrentUser.DownlineEmpId.Contains(loginid))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access..!!";
                return Ok(objresponse);
            }
            try
            {
                var exist_data = _context.tbl_status_flow_master.Where(x => x.is_deleted == 0 && x.next_status.company_id == company_id && x.flow_mster_id == flow_id).FirstOrDefault();
                if (exist_data != null)
                {
                    exist_data.is_deleted = 1;
                    exist_data.deleted_by = loginid;
                    exist_data.modified_date = DateTime.Now;

                    _context.Entry(exist_data).State = EntityState.Modified;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Status Flow Successfully Deleted";
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Detail";
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

        [Route("Save_KpiRatingMaster")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.KRARatingMaster))]
        public IActionResult Save_KpiRatingMaster([FromBody] tbl_kpi_rating_master objtbl)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(objtbl.company_id??0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access....!!";
                return Ok(objresponse);
            }
            try
            {
                var exist = _context.tbl_kpi_rating_master.Where(x => x.is_deleted == 0 && x.company_id == objtbl.company_id && x.rating_name.Trim().ToUpper() == objtbl.rating_name.Trim().ToUpper()).FirstOrDefault();
                if (exist != null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "KPI rating already exist";
                }
                else
                {

                    objtbl.is_deleted = 0;
                    objtbl.deleted_by = 0;
                    objtbl.created_date = DateTime.Now;
                    objtbl.modified_by = 0;
                    objtbl.modified_date = Convert.ToDateTime("01-01-2000");

                    _context.Entry(objtbl).State = EntityState.Added;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "KPI rating successfully saved";
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

        [Route("Get_kpiRatingMaster/{company_id}/{rating_id}")]
        [HttpGet]
        [Authorize(Policy =nameof(enmMenuMaster.EPA))]
        public IActionResult Get_kpiRatingMaster(int company_id, int rating_id)
        {
            Response_Msg objresponse = new Response_Msg();
           
            try
            {
                if (rating_id == 0)
                {
                    if (company_id > 0)
                    {
                        var data = _context.tbl_kpi_rating_master.Where(x => x.company_id == company_id && x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).Select(p => new
                        {
                            p.kpi_rating_id,
                            p.rating_name,
                            p.company_id,
                            p.comp_master.company_name,
                            // p.display_order,
                            // display_order_name=p.display_order==1?"Name":p.display_order==2?"Number":"",
                            p.description,
                            p.created_date,
                            p.modified_date
                        }).ToList();

                        return Ok(data);
                    }
                    else
                    {
                        var data = _context.tbl_kpi_rating_master.Where(x => x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).Select(p => new
                        {
                            p.kpi_rating_id,
                            p.rating_name,
                            p.company_id,
                            p.comp_master.company_name,
                            // p.display_order,
                            // display_order_name = p.display_order == 1 ? "Name" : p.display_order == 2 ? "Number" : "",
                            p.description,
                            p.created_date,
                            p.modified_date
                        }).ToList();

                        return Ok(data);
                    }
                }
                else
                {
                    var data = _context.tbl_kpi_rating_master.Where(x => x.company_id == company_id && x.kpi_rating_id == rating_id && x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).FirstOrDefault();

                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("Update_KpiRatingMaster")]
        [HttpPost]
       [Authorize(Policy =nameof(enmMenuMaster.KRARatingMaster))]
        public IActionResult Update_KpiRatingMaster([FromBody] tbl_kpi_rating_master objtbl)
        {
            Response_Msg objrespone = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(objtbl.company_id??0))
            {
                objrespone.StatusCode = 1;
                objrespone.Message = "Unauthorize Company Access...!!";
                return Ok(objrespone);
            }
            try
            {
                var exist = _context.tbl_kpi_rating_master.Where(x =>x.kpi_rating_id == objtbl.kpi_rating_id && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    var duplicate = _context.tbl_kpi_rating_master.Where(x => x.kpi_rating_id != objtbl.kpi_rating_id && x.company_id == objtbl.company_id && x.rating_name.Trim().ToUpper() == objtbl.rating_name.Trim().ToUpper() && x.is_deleted == 0).FirstOrDefault();
                    if (duplicate != null)
                    {
                        objrespone.StatusCode = 1;
                        objrespone.Message = "KPI rating already exist";
                    }
                    else
                    {
                        exist.is_deleted = 1;
                        exist.modified_by = objtbl.modified_by;
                        exist.modified_date = DateTime.Now;

                        _context.Entry(exist).State = EntityState.Modified;

                        tbl_kpi_rating_master objkpi_rating = new tbl_kpi_rating_master();

                        objkpi_rating.company_id = objtbl.company_id;
                        objkpi_rating.rating_name = objtbl.rating_name.Trim();
                        objkpi_rating.description = objtbl.description;
                        objkpi_rating.is_deleted = 0;
                        objkpi_rating.deleted_by = 0;
                        objkpi_rating.created_by = exist.created_by;
                        objkpi_rating.created_date = exist.created_date;
                        objkpi_rating.modified_by = objtbl.modified_by;
                        objkpi_rating.modified_date = DateTime.Now;

                        _context.Entry(objkpi_rating).State = EntityState.Added;

                        //exist.company_id = objtbl.company_id;
                        //exist.rating_name = objtbl.rating_name;
                        //// exist.display_order = objtbl.display_order;
                        //exist.description = objtbl.description;
                        //exist.modified_by = objtbl.modified_by;
                        //exist.modified_date = DateTime.Now;

                        //_context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();

                        objrespone.StatusCode = 0;
                        objrespone.Message = "KPI rating succesfully updated";
                    }
                }
                else
                {
                    objrespone.StatusCode = 1;
                    objrespone.Message = "Invalid Details";
                }
                return Ok(objrespone);
            }
            catch (Exception ex)
            {
                objrespone.StatusCode = 1;
                objrespone.Message = ex.Message;
                return Ok(objrespone);
            }
        }

        [Route("Delete_kpiRatingMaster/{company_id}/{kpi_rating}/{loginempid}")]
        [Authorize(Policy =nameof(enmMenuMaster.KRARatingMaster))]
        public IActionResult Delete_kpiRatingMaster(int company_id, int kpi_rating, int loginempid)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }
            try
            {
                var exist = _context.tbl_kpi_rating_master.Where(x => x.kpi_rating_id == kpi_rating && x.company_id == company_id && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    exist.is_deleted = 1;
                    exist.deleted_by = loginempid;
                    exist.modified_date = DateTime.Now;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "KPI rating successfully deleted...";
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Details";
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

        [Route("Get_EPAFiscalYearByComp/{company_id}")]
        [HttpGet]
       [Authorize(Policy =nameof(enmMenuMaster.EPA))]
        public IActionResult Get_EPAFiscalYearByComp(int company_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            try
            {
                
                var data = _context.tbl_epa_fiscal_yr_mstr.Where(x => x.company_id == company_id && x.is_deleted == 0 
               && x.from_date.Date <= DateTime.Now.Date && DateTime.Now.Date <= x.to_date.Date 
                && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).Select(p => new
                {
                    p.fiscal_year_id,
                    p.financial_year_name
                    //from_to_year=string.Format("{0}-{1}",p.from_date.Year,p.to_date.Year)
                }).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        public class EPA_cycleList
        {
            public int cycle_id { get; set; }
            public string monthname { get; set; }
        }

        [Route("Get_EPACycleByComp/{company_id}")]
        [Authorize(Policy =nameof(enmMenuMaster.EPA))]
        public List<EPA_cycleList> Get_EPACycleByComp(int company_id)
        {
            List<EPA_cycleList> cyclelist = new List<EPA_cycleList>();
            try
            {
                var data = _context.tbl_epa_cycle_master.Where(x => x.company_id == company_id && x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).FirstOrDefault();

                // 0 Monthly, 1 Quaterly, 2 Half Yearly, 3 Yearly
                int loop_length = data.cycle_type == 0 ? 12 : data.cycle_type == 1 ? 4 : data.cycle_type == 2 ? 2 : data.cycle_type == 3 ? 1 : 0;



                if (data.cycle_type == 0)
                {
                    string[] names = DateTimeFormatInfo.CurrentInfo.MonthNames;

                    for (int i = 0; i < names.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(names[i]))
                        {
                            EPA_cycleList objcyle = new EPA_cycleList();

                            objcyle.cycle_id = i + 1;
                            objcyle.monthname = names[i];

                            cyclelist.Add(objcyle);
                        }

                    }
                }
                else if (data.cycle_type == 1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        EPA_cycleList objcyle = new EPA_cycleList();
                        objcyle.cycle_id = i + 1;
                        //objcyle.monthname = i == 0 ? "1st Quater" : i == 1 ? "2nd Quater" : i == 2 ? "3rd Quater" : i == 3 ? "4th Quater" : "";
                        objcyle.monthname = GetQuarterMonths(DateTime.Now.AddMonths(-i * 3));

                        cyclelist.Add(objcyle);

                        // string testt = GetQuarterMonths(DateTime.Now.AddMonths(-i * 3));
                        //Console.WriteLine(GetQuarterMonths(DateTime.Now.AddMonths(-i * 3)));
                    }

                }
                else if (data.cycle_type == 2)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        EPA_cycleList objcyle = new EPA_cycleList();
                        objcyle.cycle_id = i + 1;
                        objcyle.monthname = i == 0 ? "1st Half" : i == 1 ? "2nd Half" : "";

                        cyclelist.Add(objcyle);

                    }
                }
                else if (data.cycle_type == 3)
                {
                    var res = _context.tbl_epa_fiscal_yr_mstr.Where(a => a.company_id == company_id && a.is_deleted == 0).FirstOrDefault();
                    //for (int i = 0; i < 2; i++)
                    //  {
                    EPA_cycleList objcyle = new EPA_cycleList();
                        objcyle.cycle_id = 1;
                        objcyle.monthname = res.financial_year_name;

                        cyclelist.Add(objcyle);

                   // }
                }



            }
            catch (Exception ex)
            {

            }

            return cyclelist;
        }

        [Route("Get_EpaWorkingRoleMasterByDeptID/{companyid}/{department_id}")]
       // [Authorize(Policy =nameof(enmMenuMaster.EPAWorkingRole))]
        public IActionResult Get_EpaWorkingRoleMasterByDeptID(int companyid, int department_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            //if (!_clsCurrentUser.CompanyId.Contains(companyid))
            //{
            //    objresponse.StatusCode = 1;
            //    objresponse.Message = "Unauthorize Company Access...!!";
            //    return Ok(objresponse);
            //}
            try
            {
                var data = _context.tbl_working_role.Where(x => x.company_id == companyid && x.dept_id == department_id && x.is_active == 1).Select(p => new
                {
                    p.working_role_id,
                    p.working_role_name
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

        string GetQuarterMonths(DateTime date)
        {
            int quarterNumber = (date.Month - 1) / 3 + 1;
            DateTime firstDayOfQuarter = new DateTime(date.Year, (quarterNumber - 1) * 3 + 1, 1);
            DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddDays(-1);


            return String.Format("{0} {2} - {1} {2}",
                System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(firstDayOfQuarter.Month),
                System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(lastDayOfQuarter.Month),
                firstDayOfQuarter.Year
                );
        }

        [Route("Save_KRAMaster")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.KRACreation))]
        public IActionResult Save_KRAMaster([FromBody] kra_master objtbl)
        {
            Response_Msg objresponse = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(objtbl.company_id??0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauhtorize Company Access...!!";
                return Ok(objresponse);
            }
            try
            {
                var exist = _context.tbl_kra_master.Where(x => x.company_id == objtbl.company_id && x.department_id == objtbl.department_id && x.wrk_role_id == objtbl.wrk_role_id && x.description.Trim().ToUpper()==objtbl.description.Trim().ToUpper() && x.factor_result.Trim().ToUpper()==objtbl.factor_result.Trim().ToUpper() && x.is_deleted==0).FirstOrDefault();
                if (exist != null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "KRA Master already created";
                }
                else
                {
                    tbl_kra_master objtblkra = new tbl_kra_master();

                    objtblkra.company_id =objtbl.company_id;
                    objtblkra.department_id =objtbl.department_id;
                    objtblkra.wrk_role_id =objtbl.wrk_role_id;
                    objtblkra.description =objtbl.description;
                    objtblkra.factor_result =objtbl.factor_result;
                    objtblkra.is_deleted = 0;
                    objtblkra.deleted_by = 0;
                    objtblkra.created_by = objtbl.created_by;
                    objtblkra.creatd_dt = DateTime.Now;
                    objtblkra.modified_by = 0;
                    objtblkra.modified_dt = Convert.ToDateTime("01-01-2000");
                   
                    //objtbl.display_order = 0;



                    _context.Entry(objtblkra).State = EntityState.Added;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "KRA Master successfully created";
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
            }
            return Ok(objresponse);
        }

        [Route("Get_KRAMaster/{kra_mstrid}/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.KRACreation))]
        public IActionResult Get_KRAMaster(int kra_mstrid, int company_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (kra_mstrid > 0)
            {
                var data = _context.tbl_kra_master.Where(x => x.kra_mstr_id == kra_mstrid && x.company_id == company_id && x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).FirstOrDefault();

                return Ok(data);
            }
            else
            {
                // List<kra_master> objlist = new List<kra_master>();
                if (company_id > 0)
                {

                    var data = _context.tbl_kra_master.Where(x => x.is_deleted == 0 && x.company_id == company_id && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).Select(p => new
                    {
                        kra_mstr_id = p.kra_mstr_id,
                        p.company_id,
                        p.comp_master.company_name,
                        //p.financial_yr,
                        //p.financial_mstr.financial_year_name,
                        //p.cycle_id,
                        p.department_id,
                        p.depart_mstr.department_name,
                        p.wrk_role_id,
                        work_role_name = p.work_role.working_role_name,
                        description = p.description,
                        factor_result = p.factor_result,
                        creatd_dt = p.creatd_dt,
                        modified_dt = p.modified_dt,
                      
                    }).ToList();
                    return Ok(data);

                    //if (data.Count > 0)
                    //{
                    //    var cycle_master_list = Get_EPACycleByComp(company_id);


                    //    for (int i = 0; i < data.Count; i++)
                    //    {
                    //        kra_master objkra = new kra_master();
                    //        objkra.kra_mstr_id = data[i].kra_mstr_id;
                    //        objkra.company_id = data[i].company_id;
                    //        objkra.company_name = data[i].company_name;
                    //        //objkra.financial_yr = data[i].financial_yr;
                    //        //objkra.financial_name = data[i].financial_year_name;
                    //        //objkra.cycle_id = data[i].cycle_id;

                    //        //for (int k = 0; k < cycle_master_list.Count; k++)
                    //        //{
                    //        //    if (cycle_master_list[k].cycle_id == data[i].cycle_id)
                    //        //    {
                    //        //        objkra.cycle_name = cycle_master_list[k].monthname;
                    //        //        break;
                    //        //    }
                    //        //}
                    //        // objkra.cycle_name = Get_cycle_id_name(data[i].cycle_id, data[i].company_id ?? 0);
                    //        objkra.department_id = data[i].department_id;
                    //        objkra.department_name = data[i].department_name;
                    //        objkra.wrk_role_id = data[i].wrk_role_id;
                    //        objkra.work_role_name = data[i].working_role_name;
                    //        objkra.description = data[i].description;
                    //        objkra.factor_result = data[i].factor_result;
                    //        objkra.creatd_dt = data[i].creatd_dt;
                    //        objkra.modified_dt = data[i].modified_dt;


                    //        objlist.Add(objkra);
                    //    }
                    //}
                    //return Ok(data);
                }
                else
                {
                    var data = _context.tbl_kra_master.Where(x => x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).Select(p => new
                    {
                        kra_mstr_id = p.kra_mstr_id,
                        p.company_id,
                        p.comp_master.company_name,
                        //p.financial_yr,
                        //p.financial_mstr.financial_year_name,
                        //p.cycle_id,
                        p.department_id,
                        p.depart_mstr.department_name,
                        p.wrk_role_id,
                        work_role_name = p.work_role.working_role_name,
                        description = p.description,
                        factor_result = p.factor_result,
                        creatd_dt = p.creatd_dt,
                        modified_dt = p.modified_dt,
                       
                    }).ToList();


                    return Ok(data);
                    //if (data.Count > 0)
                    //{
                    //    var cycle_master_list = Get_EPACycleByComp(company_id);


                    //    for (int i = 0; i < data.Count; i++)
                    //    {
                    //        kra_master objkra = new kra_master();
                    //        objkra.kra_mstr_id = data[i].kra_mstr_id;
                    //        objkra.company_id = data[i].company_id;
                    //        objkra.company_name = data[i].company_name;
                    //        //objkra.financial_yr = data[i].financial_yr;
                    //        //objkra.financial_name = data[i].financial_year_name;
                    //        //objkra.cycle_id = data[i].cycle_id;

                    //        //for (int k = 0; k < cycle_master_list.Count; k++)
                    //        //{
                    //        //    if (cycle_master_list[k].cycle_id == data[i].cycle_id)
                    //        //    {
                    //        //        objkra.cycle_name = cycle_master_list[k].monthname;
                    //        //        break;
                    //        //    }
                    //        //}
                    //        // objkra.cycle_name = Get_cycle_id_name(data[i].cycle_id, data[i].company_id ?? 0);
                    //        objkra.department_id = data[i].department_id;
                    //        objkra.department_name = data[i].department_name;
                    //        objkra.wrk_role_id = data[i].wrk_role_id;
                    //        objkra.work_role_name = data[i].working_role_name;
                    //        objkra.description = data[i].description;
                    //        objkra.factor_result = data[i].factor_result;
                    //        objkra.creatd_dt = data[i].creatd_dt;
                    //        objkra.modified_dt = data[i].modified_dt;


                    //        objlist.Add(objkra);
                    //    }
                    //}

                    // return Ok(data);
                }

                //return Ok(objlist);
            }
        }



        [Route("Update_KRAMaster")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.KRACreation))]
        public IActionResult Update_KRAMaster(kra_master objtbl)
        {
            Response_Msg objresponse = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(objtbl.company_id ?? 0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthoriz Company Access...!!";
                return Ok(objresponse);
            }

            try
            {
                var exist = _context.tbl_kra_master.Where(x => x.kra_mstr_id == objtbl.kra_mstr_id && x.company_id == objtbl.company_id && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    var duplicate = _context.tbl_kra_master.Where(x => x.kra_mstr_id != objtbl.kra_mstr_id && x.company_id == objtbl.company_id && x.department_id == objtbl.department_id && x.wrk_role_id == objtbl.wrk_role_id && x.description.Trim().ToUpper()==objtbl.description.Trim().ToUpper() && x.factor_result.Trim().ToUpper()==objtbl.factor_result.Trim().ToUpper() && x.is_deleted == 0).FirstOrDefault();

                    if (duplicate != null)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "KRA Master Already Exist";
                    }
                    else
                    {
                        //exist.financial_yr = objtbl.financial_yr;

                        exist.is_deleted = 1;
                        exist.modified_by = objtbl.modified_by;
                        exist.modified_dt = DateTime.Now;

                        _context.Entry(exist).State = EntityState.Modified;

                        tbl_kra_master objkra = new tbl_kra_master();

                        objkra.company_id = objtbl.company_id;
                        objkra.department_id = objtbl.department_id;
                        objkra.wrk_role_id = objtbl.wrk_role_id;
                        objkra.description = objtbl.description;
                        objkra.factor_result = objtbl.factor_result;
                        objkra.is_deleted = 0;
                        objkra.deleted_by = 0;
                        objkra.modified_by = objtbl.modified_by;
                        objkra.modified_dt = DateTime.Now;
                        objkra.created_by = exist.created_by;
                        objkra.creatd_dt = exist.creatd_dt;
                      

                        _context.Entry(objkra).State = EntityState.Added;
                        _context.SaveChanges();

                        objresponse.StatusCode = 0;
                        objresponse.Message = "KRA Master successfully updated...";
                    }
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Details";
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
            }

            return Ok(objresponse);
        }

        [Route("Delete_KRAMaster/{kra_masterid}/{companyid}/{loginempid}/{_manager}/{for_all_emp}")]
        [Authorize(Policy =nameof(enmMenuMaster.KRACreation))]
        public IActionResult Delete_KRAMaster(int kra_masterid, int companyid, int loginempid,int _manager,int for_all_emp)
        {
            Response_Msg objresponse = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(companyid))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }
            try
            {
                var exist = _context.tbl_kra_master.Where(x => x.kra_mstr_id == kra_masterid && x.company_id == companyid && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    exist.is_deleted = 1;
                    exist.deleted_by = loginempid;
                    exist.modified_dt = DateTime.Now;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "KRA Master successfully deleted";
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Details";
                }

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
            }
            return Ok(objresponse);
        }

        [Route("Save_TabMaster")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.EPATabMaster))]
        public IActionResult Save_TabMaster([FromBody] tbl_tab_master objtbl)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(objtbl.company_id??0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }
            try
            {
                var exist = _context.tbl_tab_master.Where(x => x.company_id == objtbl.company_id && x.tab_name.Trim().ToUpper() == objtbl.tab_name.Trim().ToUpper() && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Tab Name Already Exist";
                }
                else
                {
                    objtbl.is_deleted = 0;
                    objtbl.deleted_by = 0;
                    objtbl.created_dt = DateTime.Now;
                    objtbl.modified_by = 0;
                    objtbl.modified_date = Convert.ToDateTime("01-01-2000");

                    _context.Entry(objtbl).State = EntityState.Added;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Tab details successfully saved";
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
            }
            return Ok(objresponse);
        }

        [Route("Get_TabMaster/{tab_id}/{company_id}")]
        [HttpGet]
        [Authorize(Policy =nameof(enmMenuMaster.EPA))]
        public IActionResult Get_TabMaster(int tab_id, int company_id)
        {
            if (tab_id > 0)
            {
                var data = _context.tbl_tab_master.Where(x => x.tab_mstr_id == tab_id && x.company_id == company_id && x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).FirstOrDefault();
                return Ok(data);
            }
            else
            {
                if (company_id > 0)
                {
                    var data = _context.tbl_tab_master.Where(x => x.company_id == company_id && x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).Select(p => new
                    {
                        p.tab_mstr_id,
                        p.company_id,
                        p.comp_master.company_name,
                        p.tab_name,
                        p.description,
                        p.for_user,
                        p.for_rm1,
                        p.for_rm2,
                        p.for_rm3,
                        enable_for_user = p.for_user == 1 ? "Yes" : p.for_user == 0 ? "No" : "",
                        enable_for_rm1 = p.for_rm1 == 1 ? "Yes" : p.for_rm1 == 0 ? "No" : "",
                        enable_for_rm2 = p.for_rm2 == 1 ? "Yes" : p.for_rm2 == 0 ? "No" : "",
                        enable_for_rm3 = p.for_rm3 == 1 ? "Yes" : p.for_rm3 == 0 ? "No" : "",
                        p.created_dt,
                        p.modified_date
                    }).ToList();

                    return Ok(data);
                }
                else
                {
                    var data = _context.tbl_tab_master.Where(x => x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).Select(p => new
                    {
                        p.tab_mstr_id,
                        p.company_id,
                        p.comp_master.company_name,
                        p.tab_name,
                        p.description,
                        p.for_user,
                        p.for_rm1,
                        p.for_rm2,
                        p.for_rm3,
                        p.created_dt,
                        p.modified_date,
                        enable_for_user = p.for_user == 1 ? "Yes" : p.for_user == 0 ? "No" : "",
                        enable_for_rm1 = p.for_rm1 == 1 ? "Yes" : p.for_rm1 == 0 ? "No" : "",
                        enable_for_rm2 = p.for_rm2 == 1 ? "Yes" : p.for_rm2 == 0 ? "No" : "",
                        enable_for_rm3 = p.for_rm3 == 1 ? "Yes" : p.for_rm3 == 0 ? "No" : "",
                    }).ToList();

                    return Ok(data);
                }
            }
        }

        [Route("Update_TabMaster")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.EPATabMaster))]
        public IActionResult Update_TabMaster([FromBody] tbl_tab_master objtbl)
        {
            Response_Msg objrespone = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(objtbl.company_id ?? 0))
            {
                objrespone.StatusCode = 1;
                objrespone.Message = "Unauthorize Company Access...!!";
                return Ok(objrespone);
            }
            try
            {
                var exist = _context.tbl_tab_master.Where(x => x.tab_mstr_id == objtbl.tab_mstr_id && x.company_id == objtbl.company_id && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    var duplicate = _context.tbl_tab_master.Where(x => x.tab_mstr_id != objtbl.tab_mstr_id && x.company_id == objtbl.company_id && x.tab_name.Trim().ToUpper() == objtbl.tab_name.Trim().ToUpper() && x.is_deleted == 0).FirstOrDefault();
                    if (duplicate != null)
                    {
                        objrespone.StatusCode = 1;
                        objrespone.Message = "Tab Name already exist";
                    }
                    else
                    {
                        using (var trans = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                exist.is_deleted = 1;
                                exist.modified_by = objtbl.modified_by;
                                exist.modified_date = DateTime.Now;

                                _context.Entry(exist).State = EntityState.Modified;

                                tbl_tab_master objtab = new tbl_tab_master();

                                objtab.company_id = objtbl.company_id;
                                objtab.tab_name = objtbl.tab_name.Trim();
                                objtab.description = objtbl.description.Trim();
                                objtab.for_rm1 = objtbl.for_rm1;
                                objtab.for_rm2 = objtbl.for_rm2;
                                objtab.for_rm3 = objtbl.for_rm3;
                                objtab.for_user = objtbl.for_user;
                                objtab.is_deleted = 0;
                                objtab.deleted_by = 0;
                                objtab.modified_by = objtbl.modified_by;
                                objtab.modified_date = DateTime.Now;
                                objtab.created_by = exist.created_by;
                                objtab.created_dt = exist.created_dt;

                                _context.Entry(objtab).State = EntityState.Added;
                                //_context.SaveChanges();
                                int testid = objtab.tab_mstr_id;
                                //int latest_tab_id = _context.tbl_tab_master.OrderByDescending(x=>x.tab_mstr_id).Where(x => x.is_deleted == 0).Select(p => p.tab_mstr_id).FirstOrDefault();

                                var exist_ques_for_tab = _context.tbl_question_master.Where(x => x.is_deleted == 0 && x.tab_id == exist.tab_mstr_id).ToList();
                                if (exist_ques_for_tab.Count > 0)
                                {

                                    exist_ques_for_tab.ForEach(x => { x.tab_id = testid; });

                                    _context.tbl_question_master.UpdateRange(exist_ques_for_tab);
                                    //_context.SaveChanges();
                                }

                                _context.SaveChanges();
                                trans.Commit();


                                objrespone.StatusCode = 0;
                                objrespone.Message = "Tab details successfully updated";
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                objrespone.StatusCode = 1;
                                objrespone.Message = ex.Message;
                            }
                        }

                        



                      
                    }
                }

            }
            catch (Exception ex)
            {
                objrespone.StatusCode = 2;
                objrespone.Message = ex.Message;
            }
            return Ok(objrespone);
        }

        [Route("Delete_TabMaster/{tab_id}/{companyid}/{loginempid}")]
        [Authorize(Policy =nameof(enmMenuMaster.EPATabMaster))]
        public IActionResult Delete_TabMaster(int tab_id, int companyid, int loginempid)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(companyid))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }
            try
            {
                var exist = _context.tbl_tab_master.Where(x => x.tab_mstr_id == tab_id && x.company_id == companyid && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    exist.is_deleted = 1;
                    exist.deleted_by = loginempid;
                    exist.modified_date = DateTime.Now;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Tab details successfully deleted";
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Details";

                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
            }
            return Ok(objresponse);
        }

        [Route("Save_QuestionMaster")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.EPAQuestionMaster))]
        public IActionResult Save_QuestionMaster([FromBody] mdlEpaTabQuestion objtbl)
        {
            Response_Msg objresponse = new Response_Msg();


            if (!_clsCurrentUser.CompanyId.Contains(objtbl.company_id??0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }

            if (objtbl.ans_type == 2) // drop down
            {
                string datasourceRegex = @"^[a-zA-Z0-9'\s'\,]{1,200}$";
                Regex re = new Regex(datasourceRegex);
                if (!re.IsMatch(objtbl.ans_type_ddl))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Datasouce, Only ',' allowed for separating different value";
                    return Ok(objresponse);
                }

            }


            try
            {
                var exist = _context.tbl_question_master.Where(x => x.company_id == objtbl.company_id && x.wrk_role.dept_id == objtbl.dept_id && x.wrk_role_id == objtbl.wrk_role_id && x.tab_id == objtbl.tab_id && x.ques.Trim().ToUpper() == objtbl.ques.Trim().ToUpper() && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Question already exist in selected tab name";
                }
                else
                {

                    tbl_question_master objquestbl = new tbl_question_master();

                    objquestbl.company_id = objtbl.company_id;
                    objquestbl.wrk_role_id = objtbl.wrk_role_id;
                    objquestbl.tab_id = objtbl.tab_id;
                    objquestbl.ques = objtbl.ques;
                    objquestbl.ans_type = objtbl.ans_type;
                    objquestbl.ans_type_ddl = objtbl.ans_type_ddl;
                    objquestbl.description = objtbl.description;
                    objquestbl.deleted_by = 0;
                    objquestbl.is_deleted = 0;
                    objquestbl.created_by = objtbl.created_by;
                    objquestbl.created_dt = DateTime.Now;
                    objquestbl.modified_by = 0;
                    objquestbl.modified_date = Convert.ToDateTime("01-01-2000");


                    _context.Entry(objquestbl).State = EntityState.Added;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Question details successfully saved...";
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
            }
            return Ok(objresponse);
        }

        [Route("Get_QuestionMaster/{ques_mstr_id}/{companyid}")]
        [HttpGet]
        [Authorize(Policy =nameof(enmMenuMaster.EPAQuestionMaster))]
        public IActionResult Get_QuestionMaster(int ques_mstr_id, int companyid)
        {
            Response_Msg objresponse = new Response_Msg();
            try
            {
                if (ques_mstr_id > 0)
                {
                    var data = _context.tbl_question_master.Where(x => x.ques_mstr_id == ques_mstr_id && x.company_id == companyid && x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).Select(p => new
                    {
                        p.ques_mstr_id,
                        p.company_id,
                        p.comp_mstr.company_name,
                        p.wrk_role_id,
                        p.wrk_role.working_role_name,
                        p.wrk_role.tbl_dept_master.department_name,
                        p.wrk_role.dept_id,
                        p.tab_id,
                        p.tab_mstr.tab_name,
                        p.ques,
                        p.ans_type,
                        anss_type = p.ans_type == 1 ? "TextBox" : p.ans_type == 2 ? "DropDown" : p.ans_type == 3 ? "Yes/No with Remarks" : p.ans_type == 4 ? "Yes/No without Remarks" : "",
                        p.ans_type_ddl,
                        p.description,
                        p.created_dt,
                        p.modified_date
                    }).FirstOrDefault();
                    return Ok(data);
                }
                else
                {
                    if (companyid > 0)
                    {
                        var data = _context.tbl_question_master.Where(x => x.company_id == companyid && x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).Select(p => new
                        {
                            p.ques_mstr_id,
                            p.company_id,
                            p.comp_mstr.company_name,
                            p.wrk_role_id,
                            p.wrk_role.working_role_name,
                            p.wrk_role.tbl_dept_master.department_name,
                            p.wrk_role.dept_id,
                            p.tab_id,
                            p.tab_mstr.tab_name,
                            p.ques,
                            p.ans_type,
                            anss_type = p.ans_type == 1 ? "TextBox" : p.ans_type == 2 ? "DropDown" : p.ans_type == 3 ? "Yes/No with Remarks" : p.ans_type == 4 ? "Yes/No without Remarks" : "",
                            p.ans_type_ddl,
                            p.description,
                            created_dt = p.created_dt.ToString("dd/MM/yyyy"),
                            modified_date = p.modified_date.Date < p.created_dt.Date ? "-" : p.modified_date.ToString("dd/MM/yyyy")
                        }).ToList();

                        return Ok(data);
                    }
                    else
                    {
                        var data = _context.tbl_question_master.Where(x => x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id??0)).Select(p => new
                        {
                            p.ques_mstr_id,
                            p.company_id,
                            p.comp_mstr.company_name,
                            p.wrk_role_id,
                            p.wrk_role.working_role_name,
                            p.wrk_role.tbl_dept_master.department_name,
                            p.wrk_role.dept_id,
                            p.tab_id,
                            p.tab_mstr.tab_name,
                            p.ques,
                            p.ans_type,
                            anss_type = p.ans_type == 1 ? "TextBox" : p.ans_type == 2 ? "DropDown" : p.ans_type == 3 ? "Yes/No with Remarks" : p.ans_type == 4 ? "Yes/No without Remarks" : "",
                            p.ans_type_ddl,
                            p.description,
                            created_dt = p.created_dt.ToString("dd/MM/yyyy"),
                            modified_date = p.modified_date.Date < p.created_dt.Date ? "-" : p.modified_date.ToString("dd/MM/yyyy")
                        }).ToList();
                        return Ok(data);
                    }
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("Update_QuestionMaster")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.EPAQuestionMaster))]
        public IActionResult Update_QuestionMaster([FromBody] mdlEpaTabQuestion objtbl)
        {
            Response_Msg objresponse = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(objtbl.company_id??0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!";
                return Ok(objresponse);
            }

            try
            {
                var exist = _context.tbl_question_master.Where(x => x.ques_mstr_id == objtbl.question_id && x.company_id == objtbl.company_id && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    var duplicate = _context.tbl_question_master.Where(x => x.ques_mstr_id != objtbl.question_id && x.company_id == objtbl.company_id && x.wrk_role.dept_id == objtbl.dept_id && x.wrk_role_id == objtbl.wrk_role_id && x.tab_id == objtbl.tab_id && x.ques.Trim().ToUpper() == objtbl.ques.Trim().ToUpper()).FirstOrDefault();
                    if (duplicate != null)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Question already exist in selected tab name";
                    }
                    else
                    {
                        exist.is_deleted = 1;
                        exist.modified_by = objtbl.created_by;
                        exist.modified_date = DateTime.Now;

                        _context.Entry(exist).State = EntityState.Modified;

                        tbl_question_master objmstr = new tbl_question_master();

                        objmstr.company_id = objtbl.company_id;
                        objmstr.wrk_role_id = objtbl.wrk_role_id;
                        objmstr.tab_id = objtbl.tab_id;

                        objmstr.ques = objtbl.ques;
                        objmstr.ans_type = objtbl.ans_type;
                        objmstr.ans_type_ddl = objtbl.ans_type_ddl;
                        objmstr.description = objtbl.description;
                        objmstr.is_deleted = 0;
                        objmstr.deleted_by = 0;
                        objmstr.modified_by = objtbl.created_by;
                        objmstr.modified_date = DateTime.Now;
                        objmstr.created_by = exist.created_by;
                        objmstr.created_dt = exist.created_dt;

                        _context.Entry(objmstr).State = EntityState.Added;
                        _context.SaveChanges();

                        objresponse.StatusCode = 0;
                        objresponse.Message = "Question details successfully updated...";
                    }
                }
                else
                {
                    objresponse.StatusCode = 0;
                    objresponse.Message = "Invalid Details";
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
            }
            return Ok(objresponse);
        }

        [Route("Delete_QuestionMaster/{ques_mstr_id}/{companyid}/{loginempid}")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.EPAQuestionMaster))]
        public IActionResult Delete_QuestionMaster(int ques_mstr_id, int companyid, int loginempid)
        {
            Response_Msg objresponse = new Response_Msg();

          
            if (!_clsCurrentUser.CompanyId.Contains(companyid))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!";
                return Ok(objresponse);
            }

            try
            {
                var exist = _context.tbl_question_master.Where(x => x.ques_mstr_id == ques_mstr_id && x.company_id == companyid && x.is_deleted == 0).FirstOrDefault();
                if (exist != null)
                {
                    exist.is_deleted = 1;
                    exist.modified_date = DateTime.Now;
                    exist.deleted_by = loginempid;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Question details successfully deleted";
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unable to Delete";
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
            }
            return Ok(objresponse);
        }

        [Route("Get_WorkingRoleComponents/{company_id}/{work_role_id}")]
        [HttpGet]
        [Authorize(Policy =nameof(enmMenuMaster.EPAAllignment))]
        public IActionResult Get_WorkingRoleComponents(int company_id, int work_role_id)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }
            try
            {

                var ques_master = _context.tbl_question_master.Where(x => x.company_id == company_id && x.wrk_role_id == work_role_id && x.is_deleted == 0).Select(p => new
                {
                    p.ques_mstr_id,
                    p.company_id,
                    p.comp_mstr.company_name,
                    p.wrk_role_id,
                    p.wrk_role.working_role_name,
                    p.tab_id,
                    p.tab_mstr.tab_name,
                    p.ques,
                    p.ans_type,
                    anss_type = p.ans_type == 1 ? "TextBox" : p.ans_type == 2 ? "Dropdown" : p.ans_type == 3 ? "Yes/No with Remarks" : p.ans_type == 4 ? "Yes/No without remarks" : "-",
                    p.ans_type_ddl,
                    p.description,
                    p.wrk_role.dept_id,
                    p.wrk_role.tbl_dept_master.department_name
                }).ToList();

                var kpi_key_area_mstr = _context.tbl_kpi_key_area_master.Where(x => x.company_id == company_id && x.w_r_id == work_role_id && x.is_deleted == 0).Select(p => new
                {
                    p.key_area_id,
                    p.company_id,
                    p.comp_master.company_name,
                    //p.f_year_id,
                    //p.tbl_f_year.financial_year_name,
                    p.w_r_id,
                    p.tbl_w_role.working_role_name,
                    p.otype_id,
                    p.tbl_obj_type.objective_name,
                    p.key_area,
                    p.expected_result,
                    p.wtg,
                    p.tbl_w_role.dept_id,
                    p.tbl_w_role.tbl_dept_master.department_name
                }).ToList();

                var kra_mstr = _context.tbl_kra_master.Where(x => x.company_id == company_id && x.wrk_role_id == work_role_id && x.is_deleted == 0).Select(p => new
                {
                    p.kra_mstr_id,
                    p.company_id,
                    p.comp_master.company_name,
                    //p.financial_yr,
                    //p.financial_mstr.financial_year_name,
                    //p.cycle_id,
                    p.department_id,
                    p.depart_mstr.department_name,
                    p.wrk_role_id,
                    p.work_role.working_role_name,
                    p.description,
                    p.factor_result,

                }).ToList();


                List<kra_master> kralist = new List<kra_master>();
                if (kra_mstr.Count > 0)
                {
                    var cycle_master_list = Get_EPACycleByComp(company_id);
                    for (int i = 0; i < kra_mstr.Count; i++)
                    {
                        kra_master objkra = new kra_master();
                        // objkra.financial_name = kra_mstr[i].financial_year_name;
                        objkra.company_id = kra_mstr[i].company_id;

                        //for (int j = 0; j < cycle_master_list.Count; j++)
                        //{
                        //    if (kra_mstr[i].cycle_id == cycle_master_list[j].cycle_id)
                        //    {
                        //        objkra.cycle_name = cycle_master_list[i].monthname;
                        //        break;
                        //    }
                        //}
                        objkra.description = kra_mstr[i].description;
                        objkra.factor_result = kra_mstr[i].factor_result;

                        kralist.Add(objkra);
                    }
                }



                var result = new { ques_master = ques_master, kpi_key_area_mstr = kpi_key_area_mstr, kra_mstr = kralist };

                return Ok(result);


            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("Save_WorkingRoleComponent")]
        [HttpPost]
        [Authorize(Policy =nameof(enmMenuMaster.EPAAllignment))]
        public IActionResult Save_WorkingRoleComponent([FromBody] workrolecomponent objworkcomp)
        {
            Response_Msg objresponse = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(objworkcomp.companyid))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }

            bool all_kra_exist = true;
            bool all_ques_exist = true;
            bool all_kpi_key_mstr = true;
            try
            {
                if (objworkcomp.from_wrk_role == objworkcomp.to_wrk_role)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Something went wrong";
                    return Ok(objresponse);
                }
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        //Kpi key area master
                        List<tbl_kpi_key_area_master> kpi_key_mstr_lst = _context.tbl_kpi_key_area_master.Where(x => x.company_id == objworkcomp.companyid && x.w_r_id == objworkcomp.from_wrk_role && x.is_deleted == 0).ToList();
                        if (kpi_key_mstr_lst.Count > 0)
                        {

                            for (int i = 0; i < kpi_key_mstr_lst.Count; i++)
                            {

                                var exist_kpi_key_mstr = _context.tbl_kpi_key_area_master.Where(x => x.company_id == objworkcomp.companyid && x.w_r_id == objworkcomp.to_wrk_role && x.otype_id == kpi_key_mstr_lst[i].otype_id && x.key_area.Trim().ToUpper()==kpi_key_mstr_lst[i].key_area.Trim().ToUpper() && x.is_deleted==0).FirstOrDefault(); ;

                                if (exist_kpi_key_mstr == null)
                                {
                                    tbl_kpi_key_area_master objkpimstr = new tbl_kpi_key_area_master();

                                    objkpimstr.company_id = objworkcomp.companyid;
                                    //objkpimstr.f_year_id = kpi_key_mstr_lst[i].f_year_id;
                                    objkpimstr.w_r_id = objworkcomp.to_wrk_role;
                                    objkpimstr.otype_id = kpi_key_mstr_lst[i].otype_id;
                                    objkpimstr.key_area = kpi_key_mstr_lst[i].key_area;
                                    objkpimstr.expected_result = kpi_key_mstr_lst[i].expected_result;
                                    objkpimstr.wtg = kpi_key_mstr_lst[i].wtg;
                                    objkpimstr.is_deleted = 0;
                                    objkpimstr.deleted_by = 0;
                                    objkpimstr.created_by = objworkcomp.created_by;
                                    objkpimstr.created_date = DateTime.Now;
                                    objkpimstr.last_modified_by = 0;
                                    objkpimstr.last_modified_date = Convert.ToDateTime("01-01-2000");


                                    _context.tbl_kpi_key_area_master.AddRange(objkpimstr);
                                    all_kpi_key_mstr = false;


                                    List<tbl_kpi_criteria_master> criteria_lst = _context.tbl_kpi_criteria_master.Where(x => x.k_a_id == kpi_key_mstr_lst[i].key_area_id && x.is_deleted == 0).ToList();
                                    if (criteria_lst.Count > 0)
                                    {
                                        for (int m = 0; m > criteria_lst.Count; m++)
                                        {
    
                                        tbl_kpi_criteria_master objcriteria = new tbl_kpi_criteria_master();

                                            objcriteria.k_a_id = criteria_lst[m].k_a_id;
                                            objcriteria.criteria_number = criteria_lst[m].criteria_number;
                                            objcriteria.criteria = criteria_lst[m].criteria;
                                            objcriteria.is_deleted = 0;
                                            objcriteria.created_by = objworkcomp.created_by;
                                            objcriteria.last_modified_by = 0;
                                            objcriteria.last_modified_date = Convert.ToDateTime("01-01-2000");


                                            _context.tbl_kpi_criteria_master.AddRange(objcriteria);
                                        }
                                    }



                                }



                            }
                        }

                        //Kra master
                        List<tbl_kra_master> kra_mstr_lst = _context.tbl_kra_master.Where(x => x.company_id == objworkcomp.companyid && x.wrk_role_id == objworkcomp.from_wrk_role && x.is_deleted == 0).ToList();
                        if (kra_mstr_lst.Count > 0)
                        {
                            for (int j = 0; j < kra_mstr_lst.Count; j++)
                            {

                                var exist_kra = _context.tbl_kra_master.Where(x => x.company_id == objworkcomp.companyid && x.wrk_role_id == objworkcomp.to_wrk_role && x.description.Trim().ToUpper() == kra_mstr_lst[j].description.Trim().ToUpper() && x.factor_result.Trim().ToUpper() == kra_mstr_lst[j].factor_result.Trim().ToUpper() && x.is_deleted == 0).FirstOrDefault();
                                if (exist_kra == null)
                                {
                                    tbl_kra_master objtbl = new tbl_kra_master();
                                    objtbl.company_id = objworkcomp.companyid;
                                    //objtbl.financial_yr = kra_mstr_lst[j].financial_yr;
                                    //objtbl.cycle_id = kra_mstr_lst[j].cycle_id ;
                                    objtbl.department_id = kra_mstr_lst[j].department_id;
                                    objtbl.wrk_role_id = objworkcomp.to_wrk_role;
                                    objtbl.description = kra_mstr_lst[j].description;
                                    objtbl.factor_result = kra_mstr_lst[j].factor_result;
                                    objtbl.is_deleted = 0;
                                    objtbl.deleted_by = 0;
                                    objtbl.created_by = objworkcomp.created_by;
                                    objtbl.creatd_dt = DateTime.Now;
                                    objtbl.modified_by = 0;
                                    objtbl.modified_dt = Convert.ToDateTime("01-01-2000");


                                    _context.tbl_kra_master.AddRange(objtbl);

                                    all_kra_exist = false;
                                }
                               

                                
                            }

                        }

                        //Question master
                        List<tbl_question_master> ques_mstr_lst = _context.tbl_question_master.Where(x => x.company_id == objworkcomp.companyid && x.wrk_role_id == objworkcomp.from_wrk_role && x.is_deleted == 0).ToList();
                        if (ques_mstr_lst.Count > 0)
                        {
                            for (int k = 0; k < ques_mstr_lst.Count; k++)
                            {
                                var exist_ques= _context.tbl_question_master.Where(x => x.company_id == objworkcomp.companyid && x.wrk_role_id == objworkcomp.to_wrk_role && x.ques.Trim().ToUpper()==ques_mstr_lst[k].ques.Trim().ToUpper() && x.is_deleted == 0).FirstOrDefault();
                                if (exist_ques == null)
                                {
                                    tbl_question_master objques_mstr = new tbl_question_master();

                                    objques_mstr.company_id = objworkcomp.companyid;
                                    objques_mstr.wrk_role_id = objworkcomp.to_wrk_role;
                                    objques_mstr.tab_id = ques_mstr_lst[k].tab_id;
                                    objques_mstr.ques = ques_mstr_lst[k].ques;
                                    objques_mstr.ans_type = ques_mstr_lst[k].ans_type;
                                    objques_mstr.ans_type_ddl = ques_mstr_lst[k].ans_type_ddl;
                                    objques_mstr.description = ques_mstr_lst[k].description;
                                    objques_mstr.is_deleted = 0;
                                    objques_mstr.deleted_by = 0;
                                    objques_mstr.created_by = 0;
                                    objques_mstr.created_dt = DateTime.Now;
                                    objques_mstr.modified_by = 0;
                                    objques_mstr.modified_date = Convert.ToDateTime("01-01-2000");


                                    _context.tbl_question_master.AddRange(objques_mstr);

                                    all_ques_exist = false;
                                }
                                

                            }
                        }

                        if (all_kra_exist == false || all_ques_exist == false || all_kpi_key_mstr == false)
                        {
                            _context.SaveChanges();
                            trans.Commit();

                            objresponse.StatusCode = 0;
                            objresponse.Message = "Working role details successfully copied";
                        }
                        else
                        {
                            objresponse.StatusCode = 1;
                            objresponse.Message = "Working Role details already exist";
                        }
                        
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
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
            }
            return Ok(objresponse);
        }

        #endregion ** END BY SUPRIYA ON 17-01-2020**


        #region ***************** EPA Submission *************************
        [Route("Get_EPASubmissionData/{empID}/{Fiscalyearid}/{cycleid}/{company_id}/{cyclename}")]
        [HttpGet]
        [Authorize(Policy =nameof(enmMenuMaster.EPASubmission))]
        public async Task<IActionResult> Get_EPASubmissionDataAsync(int empID, int Fiscalyearid, int cycleid, int company_id, string cyclename)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company access...!!";
                return Ok(objresponse);
            }

            if (!_clsCurrentUser.DownlineEmpId.Contains(empID))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }
            try
            {
                List<int> emp_id_ = new List<int>();
                emp_id_.Add(empID);
                Classes.EPASubmission ePASubmission = new EPASubmission(emp_id_, Fiscalyearid, cycleid, _context, company_id, "",_AC,_clsCurrentUser.EmpId);

               // var result = new { ques_master = ques_master, kpi_key_area_mstr = kpi_key_area_mstr, kra_mstr = kralist };
                return Ok(await ePASubmission.GetEPASubmissionData());

            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [HttpPost]
        [Route("Save_EPASubmissionData")]
        [Authorize(Policy =nameof(enmMenuMaster.EPASubmission))]
        public async Task<IActionResult> Save_EPASubmissionDataAsync([FromBody] List<mdlEpaSubmission> obj)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Any(q=>obj.Any(p => p.company_id==q)))
            {
                objresponse.StatusCode = 0;
                objresponse.Message = "Unauthorize Company Accesss...!!";
                return Ok(objresponse);
            }

            if (!_clsCurrentUser.DownlineEmpId.Any(q => obj.Any(p => p.emp_id == q)))
            {
                objresponse.StatusCode = 0;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }
            try
            {



                List<int> emp_id_ = new List<int>();
                emp_id_.AddRange(obj.Select(p => p.emp_id ?? 0).ToList());
                int Fiscalyearid = obj.FirstOrDefault().fiscal_year_id ?? 0;
                int cycleid = obj.FirstOrDefault().cycle_id;
                int company_id = obj.FirstOrDefault().company_id ?? 0;
                string cyclename = obj.FirstOrDefault().cycle_name;
                Classes.EPASubmission ePASubmission = new EPASubmission(emp_id_, Fiscalyearid, cycleid, _context, company_id, "",_AC,_clsCurrentUser.EmpId);

                if (await ePASubmission.SaveEPASubmissionData(obj))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Save Sucessfully";

                }
                else
                {
                    objresponse.StatusCode = 0;
                    objresponse.Message = ePASubmission.exp_msg_;
                }


                return Ok(objresponse);


            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 0;
                objresponse.Message = ex.Message;
            }
            return Ok(objresponse);
        }
        [Route("Get_BarChartDetails/{finalid}/{Fiscalyearid}/{cycleid}/{company_id}")]
        [HttpGet]
        [Authorize(Policy =nameof(enmMenuMaster.EPABarChart))]
        public async Task<IActionResult> Get_BarChartDetails(int finalid,int Fiscalyearid, int cycleid, int company_id)
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

                //ResponseMsg objResult = new ResponseMsg();

                //string ActionName = "";
                //int _emp_id = 0;
                //var temp_emp = _AC.HttpContext.User.Claims.Where(p => p.Type == "empid").FirstOrDefault();
                //if (temp_emp != null)
                //{
                //    _emp_id = Convert.ToInt32(temp_emp.Value);
                //}

                //if (for_all_emp == 0)
                //{
                //    ActionName = "read";
                //}
                //else
                //{
                //    if (_manager == 1 && _emp_id != emp_idd)
                //    {
                //        ActionName = "read";
                //    }
                //    else
                //    {
                //        ActionName = "write";
                //    }
                //}

                //Classes.clsLoginEmpDtl ob = new clsLoginEmpDtl(_context, Convert.ToInt32(emp_idd), ActionName, _AC);


                //if (!ob.is_valid())
                //{
                //    objResult.StatusCode = 1;
                //    objResult.Message = "Unauthorize Access...!";
                //    return Ok(objResult);
                //}
                //List<int> emp_id_lst = new List<int>();

                //if (ob.is_manager == 1)
                //{
                //    if (for_all_emp == 1)
                //    {
                //        var manager_dtl = _context.tbl_emp_manager.Where(x => x.employee_id == emp_idd && x.is_deleted == 0).FirstOrDefault();
                //        if (manager_dtl != null)
                //        {

                //            var manager_emp_list = _context.tbl_emp_manager.Where(a => (a.m_one_id == _emp_id || a.m_two_id == _emp_id || a.m_three_id == _emp_id) && a.is_deleted == 0).Select(p => new
                //            {
                //                p.employee_id
                //            }).Distinct().OrderBy(s => s.employee_id).ToList();


                //            for (int i = 0; i < manager_emp_list.Count; i++)
                //            {
                //                emp_id_lst.Add(manager_emp_list[i].employee_id ?? 0);
                //            }
                //        }
                //        else
                //        {
                //            objresponse.StatusCode = 1;
                //            objresponse.Message = "Invalid Detail";
                //            return Ok(objresponse);
                //        }
                //    }
                //    else
                //    {

                //        emp_id_lst.Add(_emp_id);


                //    }

                //}
                //else if (ob.is_Admin == 1)
                //{
                //    var emp_dtl = _context.tbl_user_master.Where(x => x.is_active == 1 && x.default_company_id == company_id).Select(p => p.employee_id).ToList();
                //    for (int i = 0; i < emp_dtl.Count; i++)
                //    {
                //        emp_id_lst.Add(emp_dtl[i] ?? 0);
                //    }
                //}
                //else
                //{
                //    emp_id_lst.Add(_emp_id);
                //}
               
                Classes.EPASubmission ePASubmission = new EPASubmission(_clsCurrentUser.DownlineEmpId, Fiscalyearid, cycleid, _context, company_id, "",_AC,_clsCurrentUser.EmpId);

                //var result = new { ques_master = ques_master, kpi_key_area_mstr = kpi_key_area_mstr, kra_mstr = kralist };
                var resultt = await ePASubmission.GetEPASubmissionData();
                if (finalid == -1)
                {
                    return Ok(resultt);
                }
                else
                {
                    resultt.RemoveAll(p => p.final_review != finalid);
                    return Ok(resultt);
                }
                //return Ok(await ePASubmission.GetEPASubmissionData());

            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }

        }


        #endregion


        [Route("Get_TabDetals/{tab_id}/{company_id}/{fiscal_yr}/{cycle_id}")]
        [HttpGet]
        [Authorize(Policy =nameof(enmMenuMaster.EPATabDetails))]
        public async Task<IActionResult> Get_TabDetals(int tab_id,int company_id, int fiscal_yr, int cycle_id)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }
            
            try
            {
                //    string ActionName = "";
                //    int _emp_id = 0;
                //    var temp_emp = _AC.HttpContext.User.Claims.Where(p => p.Type == "empid").FirstOrDefault();
                //    if (temp_emp != null)
                //    {
                //        _emp_id = Convert.ToInt32(temp_emp.Value);
                //    }

                //    if (for_all_emp == 0)
                //    {
                //        ActionName = "read";
                //    }
                //    else
                //    {
                //        if (_manager == 1 && _emp_id != emp_idd)
                //        {
                //            ActionName = "read";
                //        }
                //        else
                //        {
                //            ActionName = "write";
                //        }
                //    }

                //    Classes.clsLoginEmpDtl ob = new clsLoginEmpDtl(_context, Convert.ToInt32(emp_idd), ActionName, _AC);


                //    if (!ob.is_valid())
                //    {
                //        objresponse.StatusCode = 1;
                //        objresponse.Message = "Unauthorize Access...!";
                //        return Ok(objresponse);
                //    }

                //    List<int> emp_id_lst = new List<int>();

                //    if (ob.is_manager == 1)
                //    {
                //        if (for_all_emp == 1)
                //        {
                //            var manager_dtl = _context.tbl_emp_manager.Where(x => x.employee_id == emp_idd && x.is_deleted == 0).FirstOrDefault();
                //            if (manager_dtl != null)
                //            {

                //                var manager_emp_list = _context.tbl_emp_manager.Where(a => (a.m_one_id == _emp_id || a.m_two_id == _emp_id || a.m_three_id == _emp_id) && a.is_deleted == 0).Select(p => new
                //                {
                //                    p.employee_id
                //                }).Distinct().OrderBy(s => s.employee_id).ToList();


                //                for (int i = 0; i < manager_emp_list.Count; i++)
                //                {
                //                    emp_id_lst.Add(manager_emp_list[i].employee_id ?? 0);
                //                }
                //            }
                //            else
                //            {
                //                objresponse.StatusCode = 1;
                //                objresponse.Message = "Invalid Detail";
                //                return Ok(objresponse);
                //            }
                //        }
                //        else
                //        {

                //            emp_id_lst.Add(_emp_id);


                //        }

                //    }
                //    else if (ob.is_Admin == 1)
                //    {
                //        var emp_dtl = _context.tbl_user_master.Where(x => x.is_active == 1 && x.default_company_id == company_id).Select(p => p.employee_id).ToList();
                //        for (int i = 0; i < emp_dtl.Count; i++)
                //        {
                //            emp_id_lst.Add(emp_dtl[i] ?? 0);
                //        }
                //    }
                //    else
                //    {
                //        emp_id_lst.Add(_emp_id);
                //    }

               

                Classes.EPASubmission ePASubmission = new EPASubmission(_clsCurrentUser.DownlineEmpId, fiscal_yr, cycle_id, _context, company_id, "",_AC,_clsCurrentUser.EmpId);

                var resultt = await ePASubmission.GetEPASubmissionData();
                if (tab_id == -1)//KRA
                {
                    for (int i = 0; i < resultt.Count; i++)
                    {
                        resultt[i].mdlEpaTabQuestions = null;
                        resultt[i].mdlEpaKPIDetails = null;
                    }
                    resultt = resultt.Where(x => x.epa_close_status == 1).ToList();
                    //  resultt.RemoveAll(p => p.mdlEpaTabQuestions == exclude_tabdata);

                }
                else if (tab_id == -2)//KPI
                {
                    for (int i = 0; i < resultt.Count; i++)
                    {
                        resultt[i].mdlEpaTabQuestions = null;
                        resultt[i].mdlEpaKRADetails = null;
                    }
                    resultt = resultt.Where(x => x.epa_close_status == 1).ToList();
                }
                else if (tab_id == -3 || tab_id == -4)//Final Review or closed and current status
                {
                    for (int i = 0; i < resultt.Count; i++)
                    {
                        resultt[i].mdlEpaKRADetails = null;
                        resultt[i].mdlEpaKPIDetails = null;
                        resultt[i].mdlEpaTabQuestions = null;
                    }
                    if (tab_id == -3)
                    {
                      resultt = resultt.Where(p => p.epa_close_status == 1).ToList();
                    }
                }
                else //tab name
                {
                    for (int i = 0; i < resultt.Count; i++)
                    {
                        resultt[i].mdlEpaKPIDetails = null;
                        resultt[i].mdlEpaKRADetails = null;

                        if (resultt[i].mdlEpaTabQuestions != null)
                        {
                            resultt[i].mdlEpaTabQuestions.RemoveAll(p => p.tab_id != tab_id);
                        }
                        //  resultt[i].mdlEpaTabQuestions.RemoveAll(p => p.tab_id != tab_id);
                    }

                    resultt = resultt.Where(x => x.epa_close_status == 1).ToList();


                }
                return Ok(resultt);

            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("Get_EmpEPAScoreDetails/{companyid}/{empid}/{for_all_emp}")]
        [HttpGet]
        [Authorize(Policy =nameof(enmMenuMaster.EPAGraph))]
        public IActionResult Get_EmpEPAScoreDetails(int companyid, int empid, int for_all_emp)
        {
            ResponseMsg objResult = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(companyid))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access..!!";
                return Ok(objResult);
            }

            if (!_clsCurrentUser.DownlineEmpId.Contains(empid))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access..!!";
                return Ok(objResult);
            }
           
            List<int> _empid = new List<int>();
            if (for_all_emp == 1)
            {
                _empid = _clsCurrentUser.DownlineEmpId.ToList();
            }
            else
            {
                _empid.Add(empid);
            }
            try
            {
                var details = _context.tbl_epa_submission.OrderByDescending(x => x.submission_id).Where(x => x.company_id == companyid && _empid.Contains(x.emp_id??0) && x.is_deleted == 0).Take(25).Select(p => new
                {
                    p.total_score,
                    p.get_score,
                    p.fiscal_year_id,
                    p.tbl_epa_fiscal_yr_mstr.financial_year_name,
                    p.cycle_id,
                    p.cycle_name,
                    p.emp_id
                }).ToList();


                return Ok(details);
                //if (ob.is_manager == 1)
                //{
                //    //select particular employee detail
                //    if (for_all_emp == 1)
                //    {
                //        var details = _context.tbl_epa_submission.OrderByDescending(x => x.submission_id).Where(x => x.company_id == companyid && x.emp_id == empid && x.is_deleted == 0).Take(25).Select(p => new
                //        {
                //            p.total_score,
                //            p.get_score,
                //            p.fiscal_year_id,
                //            p.tbl_epa_fiscal_yr_mstr.financial_year_name,
                //            p.cycle_id,
                //            p.cycle_name,
                //            p.emp_id
                //        }).ToList();


                //        return Ok(details);
                //    }
                //    else
                //    {//select all employee detail

                //        var emp_lst = _context.tbl_emp_manager.Where(x => (x.m_one_id == _emp_id || x.m_two_id == _emp_id || x.m_three_id == _emp_id) && x.is_deleted == 0).Select(p => p.employee_id).ToList();

                //        var details = _context.tbl_epa_submission.OrderByDescending(x => x.submission_id).Where(x => x.company_id == companyid && emp_lst.Contains(x.emp_id) && x.is_deleted == 0).Take(25).Select(p => new
                //        {
                //            p.total_score,
                //            p.get_score,
                //            p.fiscal_year_id,
                //            p.tbl_epa_fiscal_yr_mstr.financial_year_name,
                //            p.cycle_id,
                //            p.cycle_name,
                //            p.emp_id
                //        }).ToList();


                //        return Ok(details);
                //    }
                //}
                //else if (ob.is_Admin == 1)
                //{
                //    int company_id = _context.tbl_user_master.Where(x => x.employee_id == empid && x.is_active == 1).FirstOrDefault().default_company_id;
                //    if (company_id == companyid)
                //    {
                //        if (for_all_emp == 1)
                //        {
                //            var details = _context.tbl_epa_submission.OrderByDescending(x => x.submission_id).Where(x => x.company_id == companyid && x.emp_id == empid && x.is_deleted == 0).Take(25).Select(p => new
                //            {
                //                p.total_score,
                //                p.get_score,
                //                p.fiscal_year_id,
                //                p.tbl_epa_fiscal_yr_mstr.financial_year_name,
                //                p.cycle_id,
                //                p.cycle_name,
                //                p.emp_id
                //            }).ToList();


                //            return Ok(details);
                //        }
                //        else
                //        {//select all employee detail

                //            var emp_lst = _context.tbl_employee_company_map.Where(x => x.company_id == companyid && x.is_deleted == 0).Select(p => p.employee_id).ToList();

                //            var details = _context.tbl_epa_submission.OrderByDescending(x => x.submission_id).Where(x => x.company_id == companyid && emp_lst.Contains(x.emp_id) && x.is_deleted == 0).Take(25).Select(p => new
                //            {
                //                p.total_score,
                //                p.get_score,
                //                p.fiscal_year_id,
                //                p.tbl_epa_fiscal_yr_mstr.financial_year_name,
                //                p.cycle_id,
                //                p.cycle_name,
                //                p.emp_id
                //            }).ToList();


                //            return Ok(details);
                //        }
                //    }
                //    else
                //    {
                //        objResult.StatusCode = 1;
                //        objResult.Message = "Some thing went wrong";
                //        return Ok(objResult);
                //    }

                //}
                //else
                //{
                //    //get selected employee data

                //    var details = _context.tbl_epa_submission.OrderByDescending(x => x.submission_id).Where(x => x.company_id == companyid && x.emp_id == empid && x.is_deleted == 0).Take(25).Select(p => new
                //    {
                //        p.total_score,
                //        p.get_score,
                //        p.fiscal_year_id,
                //        p.cycle_id,
                //        p.tbl_epa_fiscal_yr_mstr.financial_year_name,
                //        p.cycle_name
                //    }).ToList();


                //    return Ok(details);
                //}

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 1;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }

        }

    }
}
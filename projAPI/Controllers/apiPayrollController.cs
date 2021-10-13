using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using DinkToPdf;
using DinkToPdf.Contracts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
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
using static projAPI.Model.payroll;
using static projContext.CommonClass;
using Microsoft.Extensions.Configuration;

namespace projAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class apiPayrollController : ControllerBase
    {
        private readonly Context _context;
        //Payroll payroll_ = new Payroll();
        private IHostingEnvironment _hostingEnvironment;
        private IConverter _converter;
        private readonly IOptions<AppSettings> _appSettings;

        private readonly IHttpContextAccessor _AC;
        private readonly IConfiguration _config;
        clsEmployeeDetail _clsEmployeeDetail;
        clsCurrentUser _clsCurrentUser;
        public apiPayrollController(Context context, IConverter converter, IHostingEnvironment environment, IOptions<AppSettings> appSettings, IHttpContextAccessor AC, IConfiguration config, clsEmployeeDetail _clsEmployeeDetail, clsCurrentUser _clsCurrentUser)
        {
            _context = context;
            _converter = converter;
            _hostingEnvironment = environment;
            _appSettings = appSettings;
            _AC = AC;
            _config = config;
            this._clsEmployeeDetail = _clsEmployeeDetail;
            this._clsCurrentUser = _clsCurrentUser;
        }

        [Route("Get_Salarycompvalues/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8001")]
        public IActionResult Get_SalaryComponentValues([FromRoute] int company_id)
        {
            try
            {
                clsComponentValues cps = new clsComponentValues(Convert.ToInt32(company_id), _context, 1, 201901, 0, 1, "", _config);
                var result = cps.CalculateComponentValues(false);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        #region ******************************************* Methods added by Ranjeet **************************************************************
        // Save salary group master
        [Route("Save_SalaryGroupMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.SalaryGroup))]
        public IActionResult Save_SalaryGroupMaster([FromBody] salarygroupdetails objtab)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.DownlineEmpId.Contains(objtab.created_by))
            {
                objResult.StatusCode = 0;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }

            using (var db = _context.Database.BeginTransaction())
            {
                try
                {
                    var exist = _context.tbl_salary_group.Where(x => x.group_name.ToUpper().Trim() == objtab.group_name.ToUpper().Trim()).FirstOrDefault();

                    if (exist != null)
                    {
                        objResult.StatusCode = 0;
                        objResult.Message = "Salary Group Name already exist !!";
                        return Ok(objResult);
                    }
                    else
                    {
                        tbl_salary_group sg = new tbl_salary_group();
                        sg.group_name = objtab.group_name;
                        sg.description = objtab.description;
                        sg.minvalue = objtab.minvalue;
                        sg.maxvalue = objtab.maxvalue;
                        sg.grade_Id = null; //objtab.grade_Id;
                        sg.created_by = objtab.created_by;
                        sg.created_dt = DateTime.Now;
                        sg.modified_by = objtab.created_by;
                        sg.modified_dt = DateTime.Now;
                        sg.is_active = objtab.is_active;


                        _context.Entry(sg).State = EntityState.Added;
                        _context.SaveChanges();
                        //int newgroup_id = default(int);
                        //newgroup_id = sg.group_id;

                        //foreach (string key in objtab.key_id)
                        //{
                        //    tbl_salary_group_key_used ku = new tbl_salary_group_key_used();
                        //    ku.salary_group_id = newgroup_id;
                        //    ku.key_id = Convert.ToInt32(key);
                        //    ku.is_active = 0;
                        //    ku.is_user_selected = 0;
                        //    ku.created_by = objtab.created_by;
                        //    ku.created_dt = DateTime.Now;
                        //    ku.modified_by = objtab.created_by;
                        //    ku.modified_dt = DateTime.Now;
                        //    ku.is_active = objtab.is_active;
                        //    _context.Entry(ku).State = EntityState.Added;

                        //}
                        //_context.SaveChanges();
                        db.Commit();

                        objResult.StatusCode = 0;
                        objResult.Message = "Salary Group name save successfully !!";
                        return Ok(objResult);
                    }

                }
                catch (Exception ex)
                {
                    db.Rollback();
                    objResult.StatusCode = 1;
                    objResult.Message = ex.Message;
                    return Ok(objResult);
                }
            }
        }
        //update Salary group MAster
        [Route("Update_SalaryGroupMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.SalaryGroup))]
        public IActionResult Update_SalaryGroupMaster([FromBody] salarygroupdetails objTab)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(objTab.modified_by))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }

            using (var db = _context.Database.BeginTransaction())
            {
                try
                {
                    var exist = _context.tbl_salary_group.Where(x => x.group_id == objTab.group_id).FirstOrDefault();

                    if (exist == null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Invalid salary group please try again !!";
                        return Ok(objResult);
                    }
                    else
                    {
                        //check with same grade name in other greade id
                        var duplicate = _context.tbl_salary_group.Where(x => x.group_id != objTab.group_id && x.group_name.ToUpper().Trim() == objTab.group_name.ToUpper().Trim()).FirstOrDefault();
                        if (duplicate != null)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Salary Group name exists in the system, please try another name !!";
                            return Ok(objResult);
                        }

                        exist.group_id = objTab.group_id;
                        exist.group_name = objTab.group_name;
                        exist.group_id = objTab.group_id;
                        exist.description = objTab.description;
                        exist.minvalue = objTab.minvalue;
                        exist.maxvalue = objTab.maxvalue;
                        exist.grade_Id = null; //objTab.grade_Id;
                        exist.modified_by = objTab.modified_by;
                        exist.modified_dt = DateTime.Now;
                        exist.is_active = objTab.is_active;
                        _context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();
                        //if (objTab.is_active == 1)
                        //{
                        //    _context.tbl_salary_group_key_used.Where(x => x.salary_group_id == objTab.group_id).ToList().ForEach
                        //           (a =>
                        //           {
                        //               a.is_active = 0;
                        //               a.modified_by = objTab.modified_by;
                        //               a.modified_dt = DateTime.Now;

                        //           }
                        //           );
                        //    _context.SaveChanges();

                        //    foreach (string key in objTab.key_id)
                        //    {
                        //        var exist1 = _context.tbl_salary_group_key_used.Where(x => x.salary_group_id == objTab.group_id && x.key_id == Convert.ToInt32(key)).FirstOrDefault();

                        //        if (exist1 != null)
                        //        {
                        //            _context.tbl_salary_group_key_used.Where(x => x.salary_group_id == objTab.group_id && x.key_id == Convert.ToInt32(key)).ToList().ForEach
                        //           (b =>
                        //           {
                        //               b.is_active = 1;
                        //               b.modified_by = objTab.modified_by;
                        //               b.modified_dt = DateTime.Now;

                        //           }
                        //           );
                        //            _context.SaveChanges();
                        //        }
                        //        else
                        //        {
                        //            tbl_salary_group_key_used ku1 = new tbl_salary_group_key_used();
                        //            ku1.salary_group_id = objTab.group_id;
                        //            ku1.key_id = Convert.ToInt32(key);
                        //            ku1.is_active = 1;
                        //            ku1.is_user_selected = 0;
                        //            ku1.modified_by = objTab.modified_by;
                        //            ku1.modified_dt = DateTime.Now;
                        //            ku1.is_active = objTab.is_active;
                        //            _context.Entry(ku1).State = EntityState.Added;
                        //        }


                        //    }
                        //    _context.SaveChanges();
                        //}
                        //else
                        //{

                        //    var exist1 = _context.tbl_salary_group_key_used.Where(x => x.salary_group_id == objTab.group_id).FirstOrDefault();

                        //    if (exist1 != null)
                        //    {
                        //        //db.Rollback();
                        //        //objResult.StatusCode = 1;
                        //        //objResult.Message = "Salary Group key mapping  exists in the system, please try another name !!";
                        //        //return Ok(objResult);
                        //        _context.tbl_salary_group_key_used.Where(x => x.salary_group_id == objTab.group_id).ToList().ForEach
                        //           (a =>
                        //           {
                        //               a.is_active = 0;
                        //               a.modified_by = objTab.modified_by;
                        //               a.modified_dt = DateTime.Now;

                        //           }
                        //           );
                        //        _context.SaveChanges();
                        //    }
                        //}



                    }

                    db.Commit();
                    objResult.StatusCode = 0;
                    objResult.Message = "Salary name updated successfully !!";
                    return Ok(objResult);



                }
                catch (Exception ex)
                {
                    db.Rollback();
                    objResult.StatusCode = 1;
                    objResult.Message = ex.Message;
                    return Ok(objResult);
                }
            }
        }

        ////get all salary Group data
        //[Route("Get_SalaryGroupMasterData")]
        //[HttpGet]
        ////[Authorize(Policy = "8004")]
        //public IActionResult Get_SalaryGroupMasterData()
        //{
        //    try
        //    {
        //        //var result = _context.tbl_salary_group_key_used.Join(_context.tbl_emp_officaial_sec, a => a.created_by, b => b.employee_id, (a, b) => new
        //        var result = _context.tbl_salary_group_key_used.Select(a => new
        //        {
        //            a.tsg.group_id,
        //            a.tsg.group_name,
        //            a.tsg.description,
        //            a.tsg.minvalue,
        //            a.tsg.maxvalue,
        //            a.tkm.key_name,
        //            a.key_id,
        //            //a.tsg.tgm.grade_name,
        //            //a.tsg.tgm.grade_id,
        //            a.sno,
        //            is_active_ = a.is_active
        //        })
        //            // .ToList();
        //            .Where(a => a.is_active_ == 1).ToList();
        //        return Ok(result);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}

        //get salary Group by Id
        [Route("Get_SalaryGroupMasterData/{group_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.SalaryGroup))]
        public IActionResult Get_SalaryGroupMasterData([FromRoute] int group_id)
        {
            try
            {
                if (group_id > 0)
                {
                    var result = _context.tbl_salary_group.Select(a => new
                    {
                        a.group_id,
                        a.group_name,
                        a.description,
                        a.minvalue,
                        a.maxvalue,
                        //a.tgm.grade_name, a.tgm.grade_id,
                        a.is_active
                    })
                      //.Where(a =>a.sno == group_id).ToList();
                      .Where(b => b.group_id == group_id).ToList();
                    return Ok(result);
                }
                else
                {


                    var result = _context.tbl_salary_group.Select(b => new
                    {
                        b.group_id,
                        //b.grade_Id,
                        b.group_name,
                        b.description,
                        //b.tgm.grade_name,
                        b.minvalue,
                        b.maxvalue,
                        b.is_active
                    }).ToList();
                    //var result = _context.tbl_salary_group.Join(_context.tbl_emp_officaial_sec, a => a.created_by, b => b.employee_id, (a, b) => new
                    //{
                    //    a.group_id,
                    //    a.group_name,
                    //    a.description,
                    //    a.tgm.grade_name,
                    //    a.minvalue,
                    //    a.maxvalue,
                    //    a.is_active,
                    //    created_by = b.employee_first_name + " " + b.employee_middle_name + " " + b.employee_last_name,
                    //    a.created_dt,
                    //    b.is_deleted
                    //}).Where(c => c.is_deleted != 0).ToList();

                    return Ok(result);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //// Save salary group key used
        //[Route("Save_SalaryGroupKeyUsed")]
        //[HttpPost]
        ////[Authorize(Policy = "8006")]
        //public IActionResult Save_SalaryGroupKeyUsed([FromBody] tbl_salary_group_key_used objtab)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    try
        //    {
        //        var exist = _context.tbl_salary_group_key_used.Where(x => x.key_id == objtab.key_id && x.salary_group_id == objtab.salary_group_id).FirstOrDefault();

        //        if (exist != null)
        //        {
        //            objResult.StatusCode = 0;
        //            objResult.Message = "Salary Group Name already exist !!";
        //            return Ok(objResult);
        //        }
        //        else
        //        {
        //            objtab.key_id = objtab.key_id;
        //            objtab.salary_group_id = objtab.salary_group_id;
        //            objtab.is_user_selected = objtab.is_user_selected;
        //            objtab.is_active = objtab.is_active;
        //            objtab.created_by = objtab.created_by;
        //            objtab.created_dt = DateTime.Now;
        //            objtab.modified_by = objtab.modified_by;
        //            objtab.modified_dt = DateTime.Now;
        //            objtab.is_active = objtab.is_active;
        //            _context.Entry(objtab).State = EntityState.Added;
        //            _context.SaveChanges();

        //            objResult.StatusCode = 0;
        //            objResult.Message = "Tab name save successfully !!";
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

        ////get salary Group Key Used
        //[Route("Get_SalaryGroupKeyUsed/{group_id}")]
        //[HttpGet]
        ////[Authorize(Policy = "8007")]
        //public IActionResult Get_SalaryGroupKeyUsed([FromRoute] int group_id)
        //{
        //    try
        //    {
        //        if (group_id > 0)
        //        {

        //            List<Keymasterdetails> keyDetails = _context.tbl_key_master.Where(x => x.is_active == 1).Select(a => new Keymasterdetails
        //            {
        //                key_id = a.key_id,
        //                key_name = a.key_name,
        //                ischecked = false,


        //            }).ToList();
        //            var data = _context.tbl_salary_group_key_used.Where(x => x.salary_group_id == group_id && x.is_active == 1).ToList();

        //            // var data = _context.tbl_user_role_map.Where(p => p.role_id == roleid && p.is_deleted == 0).ToList();

        //            keyDetails.ForEach(p =>
        //            {
        //                var Temp = data.FirstOrDefault(a => a.key_id == p.key_id);
        //                p.ischecked = Temp != null ? true : false;
        //            });
        //            return Ok(keyDetails);
        //        }
        //        else
        //        {

        //            var result = _context.tbl_salary_group_key_used.Join(_context.tbl_key_master, a => a.key_id, b => b.key_id, (a, b) => new
        //            {
        //                a.key_id,
        //                a.salary_group_id,
        //                b.key_name,
        //                a.is_active,
        //            }).Where(c => c.salary_group_id == group_id).ToList();

        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}


        ////get Key Master details
        //[Route("Get_keyMasterDetails/{key_id}")]
        //[HttpGet]
        ////[Authorize(Policy = "8008")]
        //public IActionResult Get_keyMasterDetails([FromRoute] int key_id)
        //{
        //    try
        //    {
        //        if (key_id > 0)
        //        {
        //            var data = _context.tbl_key_master.Where(x => x.key_id == key_id).FirstOrDefault();
        //            return Ok(data);
        //        }
        //        else
        //        {

        //            var result = _context.tbl_key_master.Where(x => x.is_active == 1).ToList();

        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}

        //get Salary Componenet details  ------------------------
        [Route("Get_SalaryComponenetDetails/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.PayrollInput))]
        public IActionResult Get_SalaryComponenetDetails([FromRoute] int company_id = 0)
        {
            try
            {
                if (company_id == 0)
                {
                    return Ok("Please select valid company id");
                }
                else
                {
                    var result = _context.tbl_component_master.Where(c => c.is_active == 1 && c.parentid == 0).Select(a => new
                    {
                        a.component_id,
                        a.component_name,
                        a.property_details,
                        a.parentid,
                        a.is_system_key,
                        a.is_salary_comp,
                        a.is_active
                    })
                        .ToList();
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        //get Salary Componenet details
        [Route("Get_SalaryChildComponenetDetails/{component_id}/{emp_id}/{monthyear}/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.PayrollInput))]
        public IActionResult Get_SalaryChildComponenetDetails([FromRoute] int component_id, int emp_id, int monthyear, int company_id)
        {
            ResponseMsg objresult = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
            {
                objresult.StatusCode = 1;
                objresult.Message = "Unauthorize Access...!!";
                return Ok(objresult);
            }

            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresult.StatusCode = 1;
                objresult.Message = "Unauthorize Company access...";
                return Ok(objresult);
            }
            try
            {
                int indx = -1;
                if (component_id == 0)
                {
                    objresult.StatusCode = 1;
                    objresult.Message = "Please select valid component id";
                    return Ok(objresult);
                }
                else
                {
                    string strmonthyear = monthyear.ToString();
                    var _c = _context.tbl_component_master.Where(z => z.parentid == component_id).Select(y => y.component_id.ToString()).ToList().Distinct();
                    var _d = new List<string>();
                    if (_c.Count() > 0)
                    {
                        _d = _context.tbl_component_master.Where(z => _c.Contains(z.parentid.ToString())).Select(y => y.component_id.ToString()).Distinct().ToList();
                    }
                    var result = (from t1 in (from t in _context.tbl_component_master
                                              where (_d.Count() > 0 ? _d.Contains(t.component_id.ToString()) : _c.Contains(t.component_id.ToString())) && t.is_active == 1 && t.is_user_interface == 1
                                              select t)
                                  join t3 in (from t in _context.tbl_salary_input where t.is_active == 1 && t.emp_id == emp_id && t.monthyear == monthyear select t) on t1.component_id equals t3.component_id into t1t3
                                  from _t1t3 in t1t3.DefaultIfEmpty()
                                  join t4 in (from t in _context.tbl_salary_input_change where t.is_active == 1 && t.emp_id == emp_id && t.monthyear == strmonthyear select t) on t1.component_id equals t4.component_id into t1t4
                                  from _t1t4 in t1t4.DefaultIfEmpty()
                                  select new
                                  {
                                      t1.component_id,
                                      t1.component_name,
                                      t1.property_details,
                                      component_value = (_t1t4 != null ? _t1t4.values : (_t1t3 != null ? _t1t3.values : "0")),
                                      emp_id = emp_id,
                                      parentid = t1.parentid,
                                      t1.is_system_key,
                                      monthyear = monthyear,
                                      is_active = t1.is_active,
                                      is_data_entry_comp = t1.is_data_entry_comp,
                                      is_salary_comp = t1.is_salary_comp
                                  }).ToList();

                    //List < tbl_salary_input_change > sic = _context.tbl_salary_input_change.Where(x => x.emp_id == emp_id && Convert.ToInt32(x.monthyear) == monthyear && x.is_active == 1).ToList();
                    //List<tbl_salary_input> si = _context.tbl_salary_input.Where(x => x.emp_id == emp_id && x.monthyear == monthyear && x.is_active == 1).ToList();
                    //sic.ForEach(S =>
                    //{
                    //    indx = -1;
                    //    indx = si.FindIndex(a => a.component_id == S.component_id);
                    //    if (indx > -1)
                    //    {
                    //        double localvalue = 0;
                    //        double.TryParse(S.values, out localvalue );
                    //        si[indx].current_month_value = localvalue;
                    //    }
                    //});

                    //var result = _context.tbl_component_formula_details
                    //   .GroupJoin(si,
                    //              n => n.comp_master.component_id,
                    //              m => m.component_id,
                    //              (n, ms) => new { n, ms = ms.DefaultIfEmpty() }).Where(p => p.n.company_id == company_id
                    //              && p.n.comp_master.is_active == 1 && p.n.comp_master.parentid == component_id &&
                    //              (p.n.comp_master.is_user_interface == 1 ))
                    //   .SelectMany(z => z.ms.Select(m => new
                    //   {
                    //       z.n.comp_master.component_id,
                    //       z.n.comp_master.component_name,
                    //       z.n.comp_master.property_details,
                    //       component_value = m.current_month_value,
                    //       emp_id = emp_id,
                    //       z.n.comp_master.parentid,
                    //       z.n.comp_master.is_system_key,
                    //       monthyear = monthyear,
                    //       z.n.comp_master.is_active,
                    //       z.n.comp_master.is_data_entry_comp,
                    //       z.n.comp_master.is_salary_comp

                    //   })).Distinct().ToList();//new { n = z.n, m }).ToList();


                    //var result = _context.tbl_component_master.Where(c =>c.parentid == component_id && c.is_active == 1 && c.is_user_interface == 1 || c.is_data_entry_comp==1 ).ToList()
                    //    .Join(si, a => a.component_id, b => b.component_id, (a, b) =>
                    //   new
                    //   {
                    //       a.component_id,
                    //       a.component_name,
                    //       a.property_details,
                    //       component_value = b.values,
                    //       emp_id = emp_id,
                    //       a.parentid,
                    //       a.is_system_key,
                    //       monthyear = monthyear,
                    //       b.is_active,
                    //       a.is_data_entry_comp,
                    //       a.is_salary_comp
                    //   });
                    return Ok(result);


                }

            }
            catch (Exception ex)
            {
                objresult.StatusCode = 2;
                objresult.Message = ex.Message;
                return Ok(objresult);
            }

        }


        ////get Salary Componenet details
        //[Route("Get_SalaryDetailsbyEmployeeId/{employee_id}/{monthyear}/{component_id}")]
        //[HttpGet]
        ////////[Authorize(Policy = "150")]
        //public IActionResult Get_SalaryDetailsbyEmployeeId([FromRoute] int employee_id, string monthyear, int component_id)
        //{
        //    try
        //    {
        //        if (employee_id == 0 || monthyear == "" || monthyear == "0")
        //        {
        //            return Ok("Please select valid employee id or monthyear");
        //        }
        //        else
        //        {

        //            var exist = _context.tbl_salary_input_change.Where(x => x.emp_id == employee_id && x.monthyear == monthyear).FirstOrDefault();
        //            if (exist != null)
        //            {
        //                var result = _context.tbl_component_master.Join(_context.tbl_salary_input_change, a => a.component_id, b => b.component_id, (a, b) =>
        //              new { a.component_id, b.emp_id, b.monthyear, a.component_name, a.property_details, component_value = 0, a.parentid }).Where
        //              (c => c.emp_id == employee_id && c.monthyear == monthyear).Join(_context.tbl_component_property_details, d => d.component_id, e => e.component_id, (d, e) =>
        //                        new
        //                        {
        //                            e.component_id,
        //                            d.parentid,
        //                            d.emp_id,
        //                            d.component_name,
        //                            d.component_value,
        //                            d.monthyear,
        //                            e.is_data_entry_comp,
        //                            e.is_active,
        //                            e.is_salary_comp,
        //                            e.formula,
        //                        }).Where(f => f.emp_id == employee_id && f.monthyear == monthyear && f.is_active == 1 && f.component_id == component_id).ToList();
        //                return Ok(result);
        //            }
        //            else
        //            {
        //                var result_ = _context.tbl_component_master.Join(_context.tbl_salary_input, a => a.component_id, b => b.component_id, (a, b) =>
        //                new { a.component_id, b.emp_id, b.monthyear, a.component_name, a.property_details, component_value = 0, a.parentid }).Where
        //                (c => c.emp_id == employee_id && c.monthyear == Convert.ToInt32(monthyear)).Join(_context.tbl_component_property_details, d => d.component_id, e => e.component_id, (d, e) =>
        //                          new
        //                          {
        //                              e.component_id,
        //                              d.parentid,
        //                              d.emp_id,
        //                              d.component_name,
        //                              d.component_value,
        //                              d.monthyear,
        //                              e.is_data_entry_comp,
        //                              e.is_active,
        //                              e.is_salary_comp,
        //                              e.formula,
        //                          }).Where(f => f.emp_id == employee_id && f.monthyear == Convert.ToInt32(monthyear) && f.is_active == 1 && f.component_id == component_id).ToList();
        //                return Ok(result_);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}


        // Save salary group key used
        [Route("Save_SalaryInput")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.PayrollInput))]
        public IActionResult Save_SalaryInput([FromBody] tbl_salary_input_change objtab)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.DownlineEmpId.Contains(objtab.emp_id ?? 0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }

            try
            {

                var isfreezed = _context.tbl_payroll_process_status.Where(b => b.payroll_month_year == Convert.ToInt32(objtab.monthyear) && b.emp_id == objtab.emp_id).Select(x => new { x.is_freezed, x.emp_id, x.payroll_month_year }).FirstOrDefault();
                // return Ok(salaryvalues);
                if (isfreezed.is_freezed == 1)
                {
                    objResult.StatusCode = 0;
                    objResult.Message = "Salary already freezed.You cannot change salary values now !!!";
                    return Ok(objResult);
                }



                var result = _context.tbl_sg_maping.Where(b => b.is_active == 1 && b.emp_id == objtab.emp_id).Select(a => new { a.salary_group_id, a.emp_id, a.is_active }).FirstOrDefault();
                clsComponentValues cc = new clsComponentValues(Convert.ToInt32(objtab.company_id), _context, Convert.ToInt32(objtab.emp_id), Convert.ToInt32(objtab.monthyear), Convert.ToInt32(objtab.component_id), Convert.ToInt32(result.salary_group_id), Convert.ToString(objtab.values), _config
                    );
                List<mdlSalaryInputValues> salaryvalues = cc.CalculateComponentValues(true);

                var exist = _context.tbl_salary_input_change.Where(x => x.emp_id == objtab.emp_id && x.component_id == objtab.component_id && x.monthyear == objtab.monthyear).FirstOrDefault();
                foreach (var key in salaryvalues)
                {

                    if (key.compId == objtab.component_id)
                    {
                        var exist1 = _context.tbl_salary_input_change.Where(x => x.component_id == Convert.ToInt32(key.compId) && x.emp_id == Convert.ToInt32(objtab.emp_id) && x.monthyear == objtab.monthyear).FirstOrDefault();

                        if (exist1 != null)
                        {
                            exist1.is_active = 1;
                            exist1.values = key.compValue;
                            exist1.modified_by = objtab.modified_by;
                            exist1.modified_dt = DateTime.Now;
                            exist1.previousvalues = objtab.previousvalues;
                            _context.Entry(exist1).State = EntityState.Modified;


                            _context.SaveChanges();

                        }
                        else
                        {
                            tbl_salary_input_change sip = new tbl_salary_input_change();
                            sip.component_id = key.compId;
                            sip.emp_id = Convert.ToInt32(objtab.emp_id);
                            sip.is_active = 1;
                            sip.monthyear = objtab.monthyear;
                            sip.values = key.compValue;
                            sip.company_id = objtab.company_id;
                            if (objtab.component_id == key.compId)
                            {
                                sip.previousvalues = objtab.previousvalues;
                            }
                            else
                            {
                                sip.previousvalues = "0";
                            }
                            sip.created_by = objtab.created_by;
                            sip.modified_by = objtab.modified_by;
                            sip.modified_dt = DateTime.Now;
                            sip.created_dt = DateTime.Now;
                            _context.Entry(sip).State = EntityState.Added;
                            _context.SaveChanges();
                        }

                    }
                }

                clsComponentValues cc1 = new clsComponentValues(Convert.ToInt32(objtab.company_id), _context, Convert.ToInt32(objtab.emp_id), Convert.ToInt32(objtab.monthyear), 0, 8, "0", _config);
                List<mdlSalaryInputValues> salaryvalues1 = cc.CalculateComponentValues(false);

                objResult.StatusCode = 0;
                objResult.Message = "Salary Input Details successfully saved !!!";

                var resultt = new { salaryvalues1 = salaryvalues1, objresultmessage = objResult };

                //return Ok(salaryvalues1);
                return Ok(resultt);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        // Save salary group key used
        [Route("Save_SalaryInputt")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.PayrollInput))]
        public IActionResult Save_SalaryInputt([FromBody] Salary_input_change objtab)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.DownlineEmpId.Contains(objtab.emp_id ?? 0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var isfreezed = _context.tbl_payroll_process_status.Where(b => b.payroll_month_year == Convert.ToInt32(objtab.monthyear) && b.emp_id == objtab.emp_id).Select(x => new { x.is_freezed, x.emp_id, x.payroll_month_year }).FirstOrDefault();

                    if (isfreezed.is_freezed == 1)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Salary already freezed.You cannot change salary values now !!!";
                        return Ok(objResult);
                    }


                    for (int i = 0; i < objtab.component_ids.Count(); i++)
                    {

                        var result = _context.tbl_sg_maping.Where(b => b.is_active == 1 && b.emp_id == objtab.emp_id).Select(a => new { a.salary_group_id, a.emp_id, a.is_active }).FirstOrDefault();

                        clsComponentValues cc = new clsComponentValues(Convert.ToInt32(objtab.company_id), _context, Convert.ToInt32(objtab.emp_id), Convert.ToInt32(objtab.monthyear), Convert.ToInt32(objtab.component_ids[i]), Convert.ToInt32(result.salary_group_id), Convert.ToString(objtab.component_values[i]), _config);
                        List<mdlSalaryInputValues> salaryvalues = cc.CalculateComponentValues(true);

                        var exist = _context.tbl_salary_input_change.Where(x => x.emp_id == objtab.emp_id && x.component_id == objtab.component_ids[i] && x.monthyear == objtab.monthyear).FirstOrDefault();
                        foreach (var key in salaryvalues)
                        {

                            if (key.compId == objtab.component_ids[i])
                            {
                                var exist1 = _context.tbl_salary_input_change.Where(x => x.component_id == Convert.ToInt32(key.compId) && x.emp_id == Convert.ToInt32(objtab.emp_id) && x.monthyear == objtab.monthyear).FirstOrDefault();

                                if (exist1 != null)
                                {
                                    exist1.is_active = 1;
                                    exist1.values = key.compValue;
                                    exist1.modified_by = objtab.modified_by;
                                    exist1.modified_dt = DateTime.Now;
                                    exist1.previousvalues = objtab.previous_values[i];
                                    _context.Entry(exist1).State = EntityState.Modified;


                                    _context.SaveChanges();

                                }
                                else
                                {
                                    tbl_salary_input_change sip = new tbl_salary_input_change();
                                    sip.component_id = key.compId;
                                    sip.emp_id = Convert.ToInt32(objtab.emp_id);
                                    sip.is_active = 1;
                                    sip.monthyear = objtab.monthyear;
                                    sip.values = key.compValue;
                                    sip.company_id = objtab.company_id;
                                    if (objtab.component_id == key.compId)
                                    {
                                        sip.previousvalues = objtab.previous_values[i];
                                    }
                                    else
                                    {
                                        sip.previousvalues = "0";
                                    }
                                    sip.created_by = objtab.created_by;
                                    sip.modified_by = objtab.modified_by;
                                    sip.modified_dt = DateTime.Now;
                                    sip.created_dt = DateTime.Now;
                                    _context.Entry(sip).State = EntityState.Added;
                                    _context.SaveChanges();
                                }

                            }
                        }

                        //clsComponentValues cc1 = new clsComponentValues(Convert.ToInt32(objtab.company_id), _context, Convert.ToInt32(objtab.emp_id), Convert.ToInt32(objtab.monthyear), 0, 8, "0", _config);
                        //List<mdlSalaryInputValues> salaryvalues1 = cc.CalculateComponentValues(false);
                    }

                    trans.Commit();
                    objResult.StatusCode = 0;
                    objResult.Message = "Salary Input Details successfully saved !!!";

                    //var resultt = new { salaryvalues1 = salaryvalues1, objresultmessage = objResult };

                    //return Ok(salaryvalues1);
                    return Ok(objResult);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    objResult.StatusCode = 0;
                    objResult.Message = ex.Message;
                    return Ok(objResult);
                }
            }
        }

        ////get Salary Componenet details
        //[Route("Get_SalaryInput/{employee_id}/{monthyear}/{component_id}")]
        //[HttpGet]
        ////////[Authorize(Policy = "152")]
        //public IActionResult Get_SalaryInput([FromRoute] int employee_id, string monthyear, int component_id)
        //{
        //    try
        //    {
        //        if (employee_id == 0 || monthyear == "" || monthyear == "0")
        //        {
        //            return Ok("Please select valid employee id or monthyear");
        //        }
        //        else
        //        {
        //            clsComponentValues cc = new clsComponentValues(_context, Convert.ToInt32(employee_id), Convert.ToInt32(monthyear), 0, 8, "0");
        //            List<mdlSalaryInputValues> salaryvalues = cc.CalculateComponentValues();

        //            var result = _context.tbl_component_master.Join(salaryvalues, a => a.component_id, b => b.compId, (a, b) =>
        //              new { a.component_id, a.component_name, a.property_details, b.compValue, a.parentid }).Where(aa => aa.parentid == component_id)
        //              .Select(
        //              p=>
        //                        new
        //                        {
        //                            `.component_id,
        //                            d.parentid,
        //                            d.component_name,
        //                            component_value = d.compValue,
        //                            componet_value_old = d.compValue,
        //                            d.property_details,
        //                            e.is_data_entry_comp,
        //                            e.is_active,
        //                            e.is_salary_comp,

        //                        }).ToList();
        //            return Ok(result);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}

        // Save reimbursement category master
        [Route("Save_ReimbursementCategoryMaster")]
        [HttpPost]
        //[Authorize(Policy = "8012")]
        public IActionResult Save_ReimbursementCategoryMaster([FromBody] tbl_remimb_cat_mstr objtab)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.DownlineEmpId.Contains(objtab.created_by))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }
            try
            {
                // var exist = _context.tbl_remimb_cat_mstr.Where(x => x.reimbursement_category_name == objtab.reimbursement_category_name && x.is_active == 1).FirstOrDefault();
                var exist = _context.tbl_remimb_cat_mstr.Where(x => x.reimbursement_category_name == objtab.reimbursement_category_name.Trim()).FirstOrDefault();
                if (exist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Category Name already exist !!";
                    return Ok(objResult);
                }
                else
                {

                    objtab.created_dt = DateTime.Now;
                    objtab.modified_dt = DateTime.Now;
                    _context.Entry(objtab).State = EntityState.Added;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Tab name save successfully !!";
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
        //Update reimbursement category master
        [Route("Update_ReimbursementCategoryMaster")]
        [HttpPost]
        //[Authorize(Policy = "8013")]
        public IActionResult Update_ReimbursementCategoryMaster([FromBody] tbl_remimb_cat_mstr objtab)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {
                var exist = _context.tbl_remimb_cat_mstr.Where(x => x.rcm_id == objtab.rcm_id).FirstOrDefault();

                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid category name. Please try again !!";
                    return Ok(objResult);
                }
                else
                {
                    //check with same grade name in other greade id
                    var duplicate = _context.tbl_remimb_cat_mstr.Where(x => x.rcm_id != objtab.rcm_id && x.reimbursement_category_name == objtab.reimbursement_category_name.Trim()).FirstOrDefault();
                    if (duplicate != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Category Name already exists in the system, please try another name !!";
                        return Ok(objResult);
                    }

                    exist.rcm_id = objtab.rcm_id;
                    exist.reimbursement_category_name = objtab.reimbursement_category_name.Trim();
                    exist.is_active = objtab.is_active;
                    exist.modified_by = objtab.modified_by;
                    exist.modified_dt = DateTime.Now;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Reimbursement category name updated successfully !!";
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


        //get Reimbursement Category Master details
        [Route("Get_ReimbursementCategoryMaster/{category_id}")]
        [HttpGet]
        //[Authorize(Policy = "8014")]
        public IActionResult Get_ReimbursementCategoryMaster([FromRoute] int category_id)
        {
            try
            {
                if (category_id > 0)
                {
                    var data = _context.tbl_remimb_cat_mstr.Where(x => x.rcm_id == category_id).FirstOrDefault();
                    return Ok(data);
                }
                else
                {

                    var result = _context.tbl_remimb_cat_mstr.Select(a => new
                    {
                        a.rcm_id,
                        a.reimbursement_category_name,
                        a.is_active,
                        a.modified_by,
                        a.modified_dt,
                        a.created_by,
                        a.created_dt,


                    }).ToList();

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        //get Reimbursement Category Master details
        [Route("Get_ReimbursementCategoryMasterActive/{category_id}")]
        [HttpGet]
        //[Authorize(Policy = "8015")]
        public IActionResult Get_ReimbursementCategoryMasterActive([FromRoute] int category_id)
        {
            try
            {
                if (category_id > 0)
                {
                    var data = _context.tbl_remimb_cat_mstr.Where(x => x.rcm_id == category_id).FirstOrDefault();
                    return Ok(data);
                }
                else
                {

                    var result = _context.tbl_remimb_cat_mstr.Select(a => new
                    {
                        a.rcm_id,
                        a.reimbursement_category_name,
                        a.is_active,
                        a.modified_by,
                        a.modified_dt,
                        a.created_by,
                        a.created_dt

                    }).Where(b => b.is_active == 1).ToList();

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        // Save reimbursement category grade master
        [Route("Save_ReimbursementGradeLimitMaster")]
        [HttpPost]
        //[Authorize(Policy = "8016")]
        public IActionResult Save_ReimbursementGradeLimitMaster([FromBody] mdlReimbursementGradelimitMaster objtab)
        {
            Response_Msg objResult = new Response_Msg();
            using (var db = _context.Database.BeginTransaction())
            {
                try
                {

                    var exist = _context.tbl_rimb_grd_lmt_mstr.Where(x => x.grade_id == objtab.grade_id && x.fiscal_year_id == objtab.fiscal_year_id && x.is_active == 1).FirstOrDefault();

                    if (exist != null)
                    {
                        objResult.StatusCode = 0;
                        objResult.Message = "Grade reimbursement limit already set !!";
                        return Ok(objResult);
                    }
                    var exist1 = _context.tbl_rimb_grd_lmt_mstr.Where(x => x.emp_id == objtab.emp_id && x.fiscal_year_id == objtab.fiscal_year_id && x.is_active == 1).FirstOrDefault();

                    if (exist != null)
                    {
                        objResult.StatusCode = 0;
                        objResult.Message = "Employee reimbursement limit already set !!";
                        return Ok(objResult);
                    }

                    tbl_rimb_grd_lmt_mstr rglm = new tbl_rimb_grd_lmt_mstr();
                    rglm.fiscal_year_id = objtab.fiscal_year_id;
                    rglm.emp_id = objtab.emp_id;
                    rglm.grade_id = objtab.grade_id;
                    rglm.monthly_limit = objtab.monthly_limit;
                    rglm.yearly_limit = objtab.yearly_limit;
                    rglm.created_by = objtab.created_by;
                    rglm.created_dt = DateTime.Now;
                    rglm.modified_by = objtab.created_by;
                    rglm.modified_dt = DateTime.Now;
                    rglm.is_active = objtab.is_active;
                    if (objtab.emp_id == 0)
                    {
                        rglm.emp_id = null;
                    }
                    if (objtab.grade_id == 0)
                    {
                        rglm.grade_id = null;
                    }
                    _context.Entry(rglm).State = EntityState.Added;
                    _context.SaveChanges();
                    int rglm_id_ = default(int);
                    rglm_id_ = rglm.rglm_id;

                    foreach (var key in objtab.mdlrclm)
                    {
                        tbl_rimb_cat_lmt_mstr rclm_ = new tbl_rimb_cat_lmt_mstr();
                        rclm_.rglm_id = rglm_id_;
                        rclm_.rcm_id = Convert.ToInt32(key.category_id);
                        rclm_.is_active = objtab.is_active;
                        rclm_.monthly_limit = key.monthly_limit;
                        rclm_.yearly_limit = key.yearly_limit;
                        rclm_.created_by = objtab.created_by;
                        rclm_.created_dt = DateTime.Now;
                        rclm_.modified_by = objtab.created_by;
                        rclm_.modified_dt = DateTime.Now;
                        rclm_.is_active = objtab.is_active;
                        rclm_.is_delete = 0;
                        _context.Entry(rclm_).State = EntityState.Added;

                    }
                    _context.SaveChanges();

                    db.Commit();
                    objResult.StatusCode = 0;
                    objResult.Message = "Employee reimbursement limit save successfully !!";
                    return Ok(objResult);
                }


                catch (Exception ex)
                {
                    db.Rollback();
                    objResult.StatusCode = 1;
                    objResult.Message = ex.Message;
                    return Ok(objResult);
                }
            }
        }

        //Update reimbursement category grade master
        [Route("Update_ReimbursementGradeLimitMaster")]
        [HttpPost]
        //[Authorize(Policy = "8017")]
        public IActionResult Update_ReimbursementGradeLimitMaster([FromBody] mdlReimbursementGradelimitMaster objtab)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {
                var exist = _context.tbl_rimb_grd_lmt_mstr.Where(x => x.rglm_id == objtab.rglm_id).FirstOrDefault();

                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid category grade limit ID. Please try again !!";
                    return Ok(objResult);
                }
                else
                {

                    using (var db = _context.Database.BeginTransaction())
                    {
                        try
                        {

                            var exist_ = _context.tbl_rimb_grd_lmt_mstr.Where(x => x.rglm_id == objtab.rglm_id).FirstOrDefault();

                            if (exist_ != null)
                            {
                                foreach (var key in objtab.mdlrclm)
                                {
                                    _context.tbl_rimb_cat_lmt_mstr.Where(x => x.rglm_id == objtab.rglm_id && x.rclm_id == key.category_id).ToList().ForEach
                                   (a =>
                                   {
                                       a.is_delete = 1;
                                       a.modified_by = objtab.modified_by;
                                       a.modified_dt = DateTime.Now;

                                   });
                                    _context.SaveChanges();
                                }

                                exist_.is_delete = 1;
                                _context.Entry(exist_).State = EntityState.Modified;
                                _context.SaveChanges();



                            }
                            var exist2 = _context.tbl_rimb_grd_lmt_mstr.Where(x => x.grade_id == objtab.grade_id && x.fiscal_year_id == objtab.fiscal_year_id && x.is_active == 1 && x.rglm_id != objtab.rglm_id && x.is_delete == 0).FirstOrDefault();

                            if (exist2 != null)
                            {
                                objResult.StatusCode = 0;
                                objResult.Message = "Grade reimbursement limit already set !!";
                                return Ok(objResult);
                            }
                            var exist3 = _context.tbl_rimb_grd_lmt_mstr.Where(x => x.emp_id == objtab.emp_id && x.fiscal_year_id == objtab.fiscal_year_id && x.is_active == 1 && x.rglm_id != objtab.rglm_id && x.is_delete == 0).FirstOrDefault();

                            if (exist3 != null)
                            {
                                objResult.StatusCode = 0;
                                objResult.Message = "Employee reimbursement limit already set !!";
                                return Ok(objResult);
                            }

                            tbl_rimb_grd_lmt_mstr rglm = new tbl_rimb_grd_lmt_mstr();
                            rglm.fiscal_year_id = objtab.fiscal_year_id;
                            rglm.emp_id = objtab.emp_id;
                            rglm.grade_id = objtab.grade_id;
                            rglm.monthly_limit = objtab.monthly_limit;
                            rglm.yearly_limit = objtab.yearly_limit;
                            rglm.created_by = objtab.created_by;
                            rglm.created_dt = DateTime.Now;
                            rglm.modified_by = objtab.created_by;
                            rglm.modified_dt = DateTime.Now;
                            rglm.is_active = objtab.is_active;
                            if (objtab.emp_id == 0)
                            {
                                rglm.emp_id = null;

                            }
                            if (objtab.grade_id == 0)
                            {
                                rglm.grade_id = null;
                            }
                            _context.Entry(rglm).State = EntityState.Added;
                            _context.SaveChanges();
                            int rglm_id_ = default(int);
                            rglm_id_ = rglm.rglm_id;

                            foreach (var key in objtab.mdlrclm)
                            {
                                tbl_rimb_cat_lmt_mstr rclm_ = new tbl_rimb_cat_lmt_mstr();
                                rclm_.rglm_id = rglm_id_;
                                rclm_.rcm_id = Convert.ToInt32(key.category_id);
                                rclm_.is_active = objtab.is_active;
                                rclm_.monthly_limit = key.monthly_limit;
                                rclm_.yearly_limit = key.yearly_limit;
                                rclm_.created_by = objtab.created_by;
                                rclm_.created_dt = DateTime.Now;
                                rclm_.modified_by = objtab.created_by;
                                rclm_.modified_dt = DateTime.Now;
                                rclm_.is_active = objtab.is_active;
                                rclm_.is_delete = 0;
                                //_context.tbl_rimb_cat_lmt_mstr.Add(rclm_);
                                _context.Entry(rclm_).State = EntityState.Added;


                            }
                            _context.SaveChanges();

                            db.Commit();
                            objResult.StatusCode = 0;
                            objResult.Message = "Employee reimbursement limit Update successfully !!";
                            return Ok(objResult);
                        }


                        catch (Exception ex)
                        {
                            db.Rollback();
                            objResult.StatusCode = 1;
                            objResult.Message = ex.Message;
                            return Ok(objResult);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }



        //get Reimbursement GradeLimit Master details
        [Route("Get_ReimbursementGradeLimitMaster/{category_id}/{grade_id}/{emp_id}/{fiscal_year_id}")]
        [HttpGet]
        //[Authorize(Policy = "8018")]
        public IActionResult Get_ReimbursementGradeLimitMaster([FromRoute] int category_id, int grade_id, int emp_id, string fiscal_year_id)
        {
            try
            {

                var grade = _context.tbl_emp_grade_allocation.Where(a => a.employee_id == emp_id).Select(b => new { b.emp_grade_id, b.applicable_from_date }).FirstOrDefault();
                if (grade != null)
                {
                    if (grade_id == 0)
                    {
                        grade_id = grade.emp_grade_id;
                    }
                }
                if (category_id > 0)
                {
                    var result = _context.tbl_rimb_grd_lmt_mstr.Select(a => new
                    {
                        a.rglm_id,
                        a.emp_id,
                        a.tem.tbl_employee_company_map.FirstOrDefault(b => b.is_deleted == 0).company_id,
                        a.tem.emp_code,
                        a.grade_id,
                        a.tgm.grade_name,
                        a.monthly_limit,
                        a.yearly_limit,
                        a.is_active,
                        a.fiscal_year_id,
                        a.created_by,
                        a.created_dt,
                        a.modified_by,
                        a.modified_dt,
                        a.is_delete

                    }).Where(x => x.rglm_id == category_id && x.is_delete == 0).FirstOrDefault();
                    return Ok(result);
                }
                else
                {
                    var exists = _context.tbl_rimb_grd_lmt_mstr.Where(x => x.is_active == 1 && x.emp_id == emp_id && x.fiscal_year_id == fiscal_year_id).FirstOrDefault();
                    if (exists != null)
                    {
                        var result = _context.tbl_rimb_grd_lmt_mstr.Select(a => new
                        {
                            a.rglm_id,
                            a.emp_id,
                            a.tem.tbl_user_master.FirstOrDefault(p => p.is_active == 1).default_company_id,
                            a.tem.emp_code,
                            a.grade_id,
                            a.tgm.grade_name,
                            a.monthly_limit,
                            a.yearly_limit,
                            a.is_active,
                            a.fiscal_year_id,
                            a.created_by,
                            a.created_dt,
                            a.modified_by,
                            a.modified_dt,
                            a.is_delete
                        }).Where(x => x.is_active == 1 && x.emp_id == emp_id && x.fiscal_year_id == fiscal_year_id && x.is_delete == 0).FirstOrDefault();
                        return Ok(result);
                    }

                    else
                    {
                        if (grade_id == 0)
                        {
                            var result = _context.tbl_rimb_grd_lmt_mstr.Select(a => new
                            {
                                a.rglm_id,
                                a.emp_id,
                                a.tem.emp_code,
                                a.grade_id,
                                a.tgm.grade_name,
                                a.monthly_limit,
                                a.yearly_limit,
                                a.is_active,
                                a.fiscal_year_id,
                                a.created_by,
                                a.created_dt,
                                a.modified_by,
                                a.modified_dt,
                                a.is_delete


                            }).Where(x => x.is_delete == 0).ToList();//Where(x => x.is_active == 1 && x.is_delete == 0).ToList();
                            return Ok(result);
                        }
                        else
                        {
                            var result = _context.tbl_rimb_grd_lmt_mstr.Select(a => new
                            {
                                a.rglm_id,
                                a.emp_id,
                                // a.tem.tbl_user_master.FirstOrDefault(p => p.is_active == 1).default_company_id,
                                a.tem.emp_code,
                                a.grade_id,
                                a.tgm.grade_name,
                                a.monthly_limit,
                                a.yearly_limit,
                                a.is_active,
                                a.fiscal_year_id,
                                a.created_by,
                                a.created_dt,
                                a.modified_by,
                                a.modified_dt,
                                a.is_delete


                            }).Where(x => x.is_active == 1 && x.grade_id == grade_id && x.fiscal_year_id == fiscal_year_id && x.is_delete == 0).ToList();
                            return Ok(result);
                        }

                    }
                }
            }


            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        // Save reimbursement category grade master
        [Route("Save_ReimbursementCategoryLimitMaster")]
        [HttpPost]
        //[Authorize(Policy = "8019")]
        public IActionResult Save_ReimbursementCategoryLimitMaster([FromBody] tbl_rimb_cat_lmt_mstr objtab)
        {
            Response_Msg objResult = new Response_Msg();
            try
            {
                var exist = _context.tbl_rimb_cat_lmt_mstr.Where(x => x.rglm_id == objtab.rglm_id && x.rcm_id == objtab.rcm_id && x.is_active == 1).FirstOrDefault();

                if (exist != null)
                {
                    objResult.StatusCode = 0;
                    objResult.Message = "Reimbursement Category limit already set !!";
                    return Ok(objResult);
                }

                else
                {
                    objtab.created_dt = DateTime.Now;
                    objtab.modified_by = objtab.modified_by;
                    objtab.modified_dt = DateTime.Now;
                    objtab.is_delete = 0;
                    _context.Entry(objtab).State = EntityState.Added;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Limit save successfully !!";
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

        //Update reimbursement category grade master
        [Route("Update_ReimbursementCategoryLimitMaster")]
        [HttpPost]
        //[Authorize(Policy = "8020")]
        public IActionResult Update_ReimbursementCategoryLimitMaster([FromBody] tbl_rimb_cat_lmt_mstr objtab)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {
                var exist = _context.tbl_rimb_cat_lmt_mstr.Where(x => x.rclm_id == objtab.rclm_id).FirstOrDefault();

                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid gategory grade limit ID. Please try again !!";
                    return Ok(objResult);
                }
                else
                {
                    //check with same grade name in other greade id
                    exist.rclm_id = objtab.rclm_id;
                    exist.rglm_id = objtab.rglm_id;
                    exist.rcm_id = objtab.rcm_id;
                    exist.is_active = objtab.is_active;
                    exist.monthly_limit = objtab.monthly_limit;
                    exist.yearly_limit = objtab.yearly_limit;
                    exist.modified_by = objtab.modified_by;
                    exist.modified_dt = DateTime.Now;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "category limit updated successfully !!";
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

        ////get Reimbursement GradeLimit Master details
        //[Route("Get_ReimbursementCategoryLimitMaster/{category_id}/{grade_id}")]
        //[HttpGet]
        ////////[Authorize(Policy = "162")]
        //public IActionResult Get_ReimbursementCategory_by_emp_grade([FromRoute] int category_id, int grade_id)
        //{
        //    try
        //    {
        //        if (category_id > 0)
        //        {
        //            var result = _context.tbl_rimb_cat_lmt_mstr.Select(a => new
        //            {
        //                a.rclm_id,
        //                a.rglm_id,
        //                a.rcm_id,
        //                a.trcm.reimbursement_category_name,
        //                a.monthly_limit,
        //                a.yearly_limit,
        //                a.is_active,
        //                a.created_by,
        //                a.created_dt,
        //                a.modified_by,
        //                a.modified_dt,
        //                a.is_delete


        //            }).Where(x => x.rclm_id == category_id && x.is_active == 1 && x.is_delete == 0).ToList();
        //            return Ok(result);
        //        }
        //        else if (grade_id > 0)
        //        {

        //            var result = _context.tbl_rimb_cat_lmt_mstr.Select(a => new
        //            {
        //                a.rclm_id,
        //                a.rglm_id,
        //                a.rcm_id,
        //                a.trcm.reimbursement_category_name,
        //                a.monthly_limit,
        //                a.yearly_limit,
        //                a.is_active,
        //                a.created_by,
        //                a.created_dt,
        //                a.modified_by,
        //                a.modified_dt,
        //                a.is_delete


        //            }).Where(x => x.rglm_id == grade_id && x.is_active == 1 && x.is_delete == 0).ToList();
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            var result = _context.tbl_rimb_cat_lmt_mstr.Select(a => new
        //            {
        //                a.rclm_id,
        //                a.rglm_id,
        //                a.trcm.reimbursement_category_name,
        //                a.monthly_limit,
        //                a.yearly_limit,
        //                a.is_active,
        //                a.created_by,
        //                a.created_dt,
        //                a.modified_by,
        //                a.modified_dt,
        //                a.is_delete


        //            }).Where(b => b.is_active == 1).ToList();
        //            return Ok(result);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }

        //}
        //get Reimbursement GradeLimit Master details
        [Route("Get_ReimbursementCategoryLimitMaster/{category_id}/{grade_id}")]
        [HttpGet]
        //[Authorize(Policy = "8021")]
        public IActionResult Get_ReimbursementCategoryLimitMaster([FromRoute] int category_id, int grade_id)
        {
            try
            {

                if (category_id > 0)
                {
                    var result = _context.tbl_rimb_cat_lmt_mstr.Select(a => new
                    {
                        a.rclm_id,
                        a.rglm_id,
                        a.rcm_id,
                        a.trcm.reimbursement_category_name,
                        a.monthly_limit,
                        a.yearly_limit,
                        a.is_active,
                        a.created_by,
                        a.created_dt,
                        a.modified_by,
                        a.modified_dt,
                        a.is_delete,
                        isActive = a.trcm.is_active


                    }).Where(x => x.rclm_id == category_id && x.is_active == 1 && x.isActive == 1 && x.is_delete == 0).ToList();
                    return Ok(result);
                }
                else if (grade_id > 0)
                {

                    var result = _context.tbl_rimb_cat_lmt_mstr.Select(a => new
                    {
                        a.rclm_id,
                        a.rglm_id,
                        a.rcm_id,
                        a.trcm.reimbursement_category_name,
                        a.monthly_limit,
                        a.yearly_limit,
                        a.is_active,
                        a.created_by,
                        a.created_dt,
                        a.modified_by,
                        a.modified_dt,
                        a.is_delete,
                        isActive = a.trcm.is_active

                    }).Where(x => x.rglm_id == grade_id && x.is_delete == 0).ToList();//Where(x => x.rglm_id == grade_id && x.is_active == 1 && x.isActive == 1 && x.is_delete == 0).ToList();
                    return Ok(result);
                }

                else
                {
                    var result = _context.tbl_rimb_cat_lmt_mstr.Select(a => new
                    {
                        a.rclm_id,
                        a.rglm_id,
                        a.trcm.reimbursement_category_name,
                        a.monthly_limit,
                        a.yearly_limit,
                        a.is_active,
                        a.created_by,
                        a.created_dt,
                        a.modified_by,
                        a.modified_dt,
                        a.is_delete


                    }).Where(b => b.is_active == 1).ToList();
                    return Ok(result);
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        // Save salary group master
        [Route("Save_ReimbursementRequest")]
        [HttpPost]
        //[Authorize(Policy = "8022")]
        public IActionResult Save_ReimbursementRequest([FromBody] Category_Reimbursment_request objtab)
        {
            Response_Msg objResult = new Response_Msg();
            using (var db = _context.Database.BeginTransaction())
            {
                try
                {
                    var exist = _context.tbl_rimb_req_mstr.Where(x => x.emp_id == objtab.emp_id && x.fiscal_year_id == objtab.fiscal_year_id && x.request_month_year == objtab.monthyear && x.request_type == objtab.req_type).FirstOrDefault();

                    if (exist != null)
                    {
                        objResult.StatusCode = 0;
                        objResult.Message = "Request already submittted for this request type. Pleae try another request type !!";
                        return Ok(objResult);
                    }
                    else
                    {
                        tbl_rimb_req_mstr rrm = new tbl_rimb_req_mstr();
                        rrm.emp_id = objtab.emp_id;
                        rrm.fiscal_year_id = objtab.fiscal_year_id;
                        rrm.request_month_year = objtab.monthyear;
                        rrm.request_type = objtab.req_type;
                        rrm.total_request_amount = objtab.total_request_amount;
                        rrm.is_approvred = 0;
                        rrm.created_by = objtab.created_by;
                        rrm.created_dt = DateTime.Now;
                        //rrm.modified_by = objtab.created_by;
                        rrm.modified_dt = DateTime.Now;
                        rrm.is_active = 1;


                        _context.Entry(rrm).State = EntityState.Added;
                        _context.SaveChanges();
                        int new_rrm_id = default(int);
                        new_rrm_id = rrm.rrm_id;

                        foreach (var key in objtab.reimbursement_request_details)
                        {
                            tbl_rimb_req_details rrd = new tbl_rimb_req_details();
                            rrd.rrm_id = new_rrm_id;
                            rrd.rclm_id = Convert.ToInt32(key.rclm_id);
                            rrd.is_active = 1;
                            rrd.request_amount = key.approved_amount;
                            rrd.Bill_amount = key.Bill_amount;
                            rrd.Bill_date = key.Bill_date;
                            rrd.created_by = objtab.created_by;
                            rrd.created_dt = DateTime.Now;
                            //rrd.modified_by = objtab.created_by;
                            rrd.modified_dt = DateTime.Now;
                            rrd.is_active = objtab.is_active;
                            _context.Entry(rrd).State = EntityState.Added;

                        }
                        _context.SaveChanges();
                        db.Commit();

                        objResult.StatusCode = 0;
                        objResult.Message = "Reimbursement request submitted successfully !!!";
                        return Ok(objResult);
                    }

                }
                catch (Exception ex)
                {
                    db.Rollback();
                    objResult.StatusCode = 1;
                    objResult.Message = ex.Message;
                    return Ok(objResult);
                }
            }
        }


        //get Reimbursement Category Master details
        [Route("Get_EmployeeData/{employee_id}/{companyid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.PayrollInput))]
        public IActionResult Get_EmployeeData([FromRoute] int employee_id, int companyid)
        {
            throw new NotImplementedException();
#if false
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(employee_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize accesss...";
                return Ok(objresponse);
            }
            try
            {
                var data = _clsEmployeeDetail.GetEmployeeByDate(companyid, new DateTime(2000, 1, 1), DateTime.Now.AddDays(1));

                if (employee_id > 0)
                {
                    var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && x.employee_id == employee_id).Select(p => new
                    {
                        p.date_of_birth,
                        p.date_of_joining,
                        p.tbl_location_master.location_name,
                        p.gender
                    }).FirstOrDefault();

                    var result = data.Where(x => x.employee_id == employee_id).Select(a => new
                    {
                        a.employee_id,
                        dob = teos != null ? teos.date_of_birth.ToString("dd-MMM-yyyy") : "",
                        doj = teos != null ? teos.date_of_joining.ToString("dd-MMM-yyyy") : "",
                        employeeName = a.emp_name,
                        location_name = teos != null ? teos.location_name : "",
                        isPFEligible = 1,
                        isESIEligible = 1,
                        gender = teos != null ? teos.gender : 0,
                    }).FirstOrDefault();

                    return Ok(result);
                }
                else
                {
                    List<int> _empid = data.Select(p => p.employee_id).ToList();

                    var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && _empid.Contains(x.employee_id ?? 0)).Select(p => new
                    {
                        p.date_of_birth,
                        p.date_of_joining,
                        p.tbl_location_master.location_name,
                        p.gender,
                        p.employee_id
                    }).ToList();

                    var result = data.Select(a => new
                    {
                        a.employee_id,
                        dob = teos.FirstOrDefault(q => q.employee_id == a.employee_id).date_of_birth.ToString("dd-MMM-yyyy"),
                        doj = teos.FirstOrDefault(q => q.employee_id == a.employee_id).date_of_joining.ToString("dd-MMM-yyyy"),
                        employeeName = a.emp_name,
                        location_name = teos.FirstOrDefault(q => q.employee_id == a.employee_id).location_name,
                        isPFEligible = 1,
                        isESIEligible = 1,
                        gender = teos.FirstOrDefault(q => q.employee_id == a.employee_id).gender,
                    }).ToList();


                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
#endif
        }
        //get Reimbursement GradeLimit Master details
        [Route("Get_ProcessedMonthyear/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8024")]
        public IActionResult Get_ProcessedMonthyear([FromRoute] int company_id)
        {
            try
            {
                var result = _context.tbl_salary_input.Where(x => x.company_id == company_id).Select(a => a.monthyear).DefaultIfEmpty(0).Max();
                return Ok(result);


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //get Reimbursement GradeLimit Master details
        [Route("Get_two_ProcessedMonthyear/{employee_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.PayrollInput))]
        public IActionResult Get_two_ProcessedMonthyear([FromRoute] int employee_id)
        {
            ResponseMsg objresult = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(employee_id))
            {
                objresult.StatusCode = 1;
                objresult.Message = "Unauthorize Access...!!";
                return Ok(objresult);
            }

            try
            {

                var result = _context.tbl_salary_input.OrderByDescending(x => x.monthyear).Where(b => b.emp_id == employee_id && b.is_active == 1).Select(y => new { y.monthyear }).Distinct().Take(2).ToList();

                return Ok(result);


            }
            catch (Exception ex)
            {
                objresult.StatusCode = 2;
                objresult.Message = ex.Message;
                return Ok(objresult);
            }
        }


#endregion


#region ********************************** Code Added by : Rajesh Yati for Salary Group Maped with Employee ****************************

        // Save salary group mapped with employee
        [Route("Save_SalaryGroupMapEmp")]
        [HttpPost]
        //[Authorize(Policy = "8026")]
        public IActionResult Save_SalaryGroupMapEmp([FromBody] tbl_sg_maping objsal)
        {
            Response_Msg objResult = new Response_Msg();
            try
            {
                //var exist = _context.tbl_sg_maping.Where(x => (x.emp_id == objsal.emp_id && x.applicable_from_dt == objsal.applicable_from_dt) && (x.salary_group_id == objsal.salary_group_id || x.emp_id == objsal.emp_id)).FirstOrDefault();

                var exist = _context.tbl_sg_maping.OrderByDescending(x => x.map_id).Where(x => x.emp_id == objsal.emp_id && x.is_active == 1 && x.applicable_from_dt.Date >= objsal.applicable_from_dt.Date).FirstOrDefault();
                if (exist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Salary Group Name already mapped with employee. Please try another !!";
                    return Ok(objResult);
                }
                else
                {
                    objsal.emp_id = objsal.emp_id;
                    objsal.salary_group_id = objsal.salary_group_id;
                    objsal.applicable_from_dt = objsal.applicable_from_dt;
                    objsal.is_active = objsal.is_active;
                    objsal.created_by = objsal.created_by;
                    objsal.created_dt = DateTime.Now;
                    //objsal.is_active = objsal.is_active;
                    _context.Entry(objsal).State = EntityState.Added;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Salary Group successfully mapped with selected employee !!";
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
        //update Salary group MAster
        [Route("Update_SalaryGroupMapEmp")]
        [HttpPost]
        //[Authorize(Policy = "8027")]
        public IActionResult Update_SalaryGroupMapEmp([FromBody] tbl_sg_maping objsal)
        {
            Response_Msg objResult = new Response_Msg();

            try
            {
                var exist = _context.tbl_sg_maping.Where(x => x.map_id == objsal.map_id).FirstOrDefault();

                if (exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Invalid salary group employee mapping Id. Please try again !!";
                    return Ok(objResult);
                }
                else
                {
                    //check with same grade name in other greade id
                    var duplicate = _context.tbl_sg_maping.Where(x => x.map_id != objsal.map_id && x.salary_group_id == objsal.salary_group_id && x.emp_id == objsal.emp_id && x.applicable_from_dt == objsal.applicable_from_dt && x.is_active == 1).FirstOrDefault();



                    if (duplicate != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "salary group employee mapping already exists in the system, please try another name !!";
                        return Ok(objResult);
                    }

                    var exist_date_data = _context.tbl_sg_maping.OrderByDescending(x => x.map_id).Where(x => x.emp_id == objsal.emp_id && x.is_active == 1 && x.map_id < objsal.map_id && x.applicable_from_dt.Date >= objsal.applicable_from_dt.Date).FirstOrDefault();
                    if (exist_date_data != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "salary group employee mapping already exists in the system please select other date!!";
                        return Ok(objResult);
                    }


                    exist.emp_id = objsal.emp_id;
                    exist.salary_group_id = objsal.salary_group_id;
                    exist.applicable_from_dt = objsal.applicable_from_dt;
                    exist.modified_by = objsal.modified_by;
                    exist.modified_dt = DateTime.Now;
                    //exist.is_active = objsal.is_active;

                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Salary Group Map with Employee updated successfully !!";
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
        //get salary Group
        [Route("Get_SalaryGroupMapEmpData/{map_id}")]
        [HttpGet]
        //[Authorize(Policy = "8028")]
        public IActionResult Get_SalaryGroupMapEmpData([FromRoute] int map_id)
        {
            try
            {
                if (map_id > 0)
                {


                    var data = _context.tbl_sg_maping.Where(a => a.is_active == 1 && a.map_id == map_id).Select(b => new { b.map_id, emp_id = b.emp_id, emp_code = b.tem.emp_code, salary_group_id = b.salary_group_id, effective_dt = b.applicable_from_dt, emp_name = b.tem.tbl_emp_officaial_sec.Where(c => c.is_deleted == 0 && c.employee_id == b.emp_id).Select(d => d.employee_first_name).FirstOrDefault(), default_company_id = b.tem.tbl_user_master.Where(e => e.is_active == 1 && e.employee_id == b.emp_id).Select(f => f.default_company_id).FirstOrDefault(), grade_id = b.tsg.grade_Id }).ToList();


                    //var data = (from s in _context.tbl_sg_maping
                    //            join g in _context.tbl_salary_group
                    //                on s.salary_group_id equals g.group_id
                    //            join e1 in _context.tbl_emp_officaial_sec
                    //             on s.emp_id equals e1.employee_id
                    //            join e in _context.tbl_employee_master
                    //             on s.emp_id equals e.employee_id
                    //            join a in _context.tbl_emp_grade_allocation on e.employee_id equals a.employee_id
                    //            join d in _context.tbl_grade_master on a.grade_id equals d.grade_id
                    //            join u in _context.tbl_user_master on s.emp_id equals u.employee_id
                    //            where s.map_id == map_id && e1.is_deleted == 0 && s.is_active == 1 && e.is_active == 1
                    //            select new
                    //            {
                    //                map_id = s.map_id,
                    //                emp_id = e.employee_id,
                    //                emp_code = e.emp_code,
                    //                salary_group_id = g.group_id,
                    //                group_name = g.group_name,
                    //                effective_dt = s.applicable_from_dt,
                    //                emp_name = e1.employee_first_name,
                    //                grade_id = d.grade_id,
                    //                grade_name = d.grade_name,
                    //                is_active = s.is_active,
                    //                created_by = s.created_by,
                    //                created_dt = s.created_dt,
                    //                u.default_company_id,

                    //            }).ToList();
                    return Ok(data);
                }
                else
                {

                    var result = _context.tbl_sg_maping.Where(a => a.is_active == 1).Select(b => new
                    {
                        b.map_id,
                        emp_id = b.emp_id,
                        default_company_id = b.tem.tbl_employee_company_map.FirstOrDefault(q => q.is_deleted == 0).company_id,
                        company_name = b.tem.tbl_employee_company_map.FirstOrDefault(q => q.is_deleted == 0).tbl_company_master.company_name,
                        emp_name_code = string.Format("{0} {1} {2} ({3})", b.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && q.employee_first_name != "").employee_first_name,
                        b.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && q.employee_middle_name != "").employee_middle_name,
                        b.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && q.employee_last_name != "").employee_last_name,
                        b.tem.emp_code),
                        group_name = b.tsg.group_name,
                        grade_Id = b.tsg.grade_Id,
                        b.tsg.tgm.grade_name,
                        salary_group_id = b.salary_group_id,
                        effective_dt = b.applicable_from_dt

                    }).ToList();


                    //var result = _context.tbl_sg_maping.Where(a => a.is_active == 1).Select(b => new {
                    //b.map_id, emp_id = b.emp_id,
                    //emp_code = b.tem.emp_code,
                    //salary_group_id = b.salary_group_id,
                    //effective_dt = b.applicable_from_dt,
                    //emp_name = b.tem.tbl_emp_officaial_sec.Where(c => c.is_deleted == 0 && c.employee_id == b.emp_id).Select(d => d.employee_first_name).FirstOrDefault(),
                    //default_company_id = b.tem.tbl_user_master.Where(e => e.is_active == 1 && e.employee_id == b.emp_id).Select(f => f.default_company_id).FirstOrDefault(),
                    //group_name = b.tsg.group_name,
                    // b.tsg.grade_Id,
                    // b.tsg.tgm.grade_name }).ToList();

                    //var result = (from s in _context.tbl_sg_maping
                    //              join g in _context.tbl_salary_group
                    //                  on s.salary_group_id equals g.group_id
                    //              join e1 in _context.tbl_emp_officaial_sec
                    //               on s.emp_id equals e1.employee_id
                    //              join e in _context.tbl_employee_master
                    //               on s.emp_id equals e.employee_id
                    //              join a in _context.tbl_emp_grade_allocation on e.employee_id equals a.employee_id
                    //              join d in _context.tbl_grade_master on a.grade_id equals d.grade_id
                    //              join u in _context.tbl_user_master on s.emp_id equals u.employee_id
                    //              where e1.is_deleted == 0 && s.is_active == 1


                    //              select new
                    //              {
                    //                  map_id = s.map_id,
                    //                  emp_id = e.employee_id,
                    //                  emp_code = e.emp_code,
                    //                  salary_group_id = g.group_id,
                    //                  group_name = g.group_name,
                    //                  effective_dt = s.applicable_from_dt,
                    //                  emp_name = e1.employee_first_name,
                    //                  grade_id = d.grade_id,
                    //                  grade_name = d.grade_name,
                    //                  is_active = s.is_active,
                    //                  created_by = s.created_by,
                    //                  created_dt = s.created_dt,
                    //                  u.default_company_id,

                    //              }).ToList();

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        // GET: api/apiMasters/GetStateList/0/5
        [Route("GetSalaryGroupList/{salgroupId}/{GradeId}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.SalaryGroupAlignment))]
        public async Task<IActionResult> GetSalaryGroupList([FromRoute] int salgroupId, int GradeId)
        {
            try
            {

                if (salgroupId > 0 && GradeId == 0)
                {
                    var result = (from a in _context.tbl_salary_group.Where(x => x.group_id == salgroupId) select a).ToList();

                    return Ok(result);
                }
                else if (GradeId > 0)
                {
                    var result = (from a in _context.tbl_salary_group.Where(x => x.grade_Id == GradeId && x.is_active == 1) select a).ToList();

                    return Ok(result);
                }
                else
                {
                    var result = _context.tbl_salary_group.Where(x => x.is_active == 1).Select(p => new
                    {
                        p.group_id,
                        p.group_name

                    }).ToList();

                    //var result = _context.tbl_salary_group.Join(_context.tbl_grade_master, a => a.grade_Id, b => b.grade_id, (a, b) => new
                    //{
                    //    a.group_id,
                    //    a.group_name
                    //}).ToList();

                    return Ok(result);
                }


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        //// GET: api/apiMasters/GetStateList/0/5
        //[Route("GetFormulaList/{salgroupId}/{GradeId}")]
        //[HttpGet]
        ////[Authorize(Policy = "8030")]
        //public async Task<IActionResult> GetFormulaList([FromRoute] int salgroupId, int GradeId)
        //{
        //    try
        //    {

        //        if (GradeId == 0)
        //        {
        //            var result = (from a in _context.tbl_salary_group.Where(x => x.grade_Id == GradeId) select a).ToList();

        //            return Ok(result);
        //        }

        //        else
        //        {
        //            var result = _context.tbl_salary_group_key_used.Select(a => new { a.tkm.key_name, a.tkm.key_id, a.tsg.group_id, a.tsg.grade_Id, a.is_active }).Where(a => a.group_id == salgroupId && a.grade_Id == GradeId && a.is_active == 1).ToList();
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}
        //get Function list 
        //[Route("Get_FunctionList")]
        ////[Authorize(Policy = "8031")]
        //public IActionResult Get_FunctionList()
        //{
        //    try
        //    {

        //        var data = _context.tbl_function_master.Where(a => a.function_id == 26 && a.is_active == 0).Select(a => new { a.function_id, a.calling_function_name, a.is_active }).ToList();

        //        foreach (var item in data)
        //        {
        //            var name = item;
        //            string[] arry = name.ToString().Split(',');
        //            string a = arry[1];
        //            string[] final = a.Split("=");
        //            fun_name = final[1];

        //            Type PayrollType = typeof(Payroll);
        //            ConstructorInfo PayrollConstructor = PayrollType.GetConstructor(new[] { typeof(DateTime), typeof(DateTime), typeof(int) });
        //            object PayrollClassObject = PayrollConstructor.Invoke(new object[] { Convert.ToDateTime("04/08/2019 7:00 PM"), Convert.ToDateTime("04/15/2019 8:00 PM"), 0 });

        //            // Get the Value method and invoke with a parameter value of Date

        //            MethodInfo PayrollMethod = PayrollType.GetMethod(fun_name.Trim());
        //            object Value = PayrollMethod.Invoke(PayrollClassObject, new object[] { });
        //        }

        //        return Ok(data);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}
        //[Route("Save_KeyMaster")]
        //[HttpPost]
        ////[Authorize(Policy = "8032")]
        //// Here Save key Master details
        //public IActionResult Save_KeyMaster([FromBody] tbl_key_master objkey)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    try
        //    {
        //        var exist = _context.tbl_key_master.Where(x => x.key_name == objkey.key_name).FirstOrDefault();

        //        if (exist != null)
        //        {
        //            objResult.StatusCode = 0;
        //            objResult.Message = "Key already exist !!";
        //            return Ok(objResult);
        //        }
        //        else
        //        {
        //            objkey.key_name = objkey.key_name;
        //            objkey.display_name = objkey.display_name;
        //            objkey.description = objkey.description;
        //            objkey.calling_function_name = objkey.calling_function_name;
        //            objkey.data_type = objkey.data_type;
        //            objkey.forumal_name = objkey.forumal_name;
        //            objkey.type = 2;
        //            objkey.computation_order = 0;
        //            objkey.is_active = objkey.is_active;
        //            objkey.created_dt = DateTime.Now;
        //            objkey.defaultvalue = "0";
        //            objkey.created_by = objkey.created_by;
        //            _context.Entry(objkey).State = EntityState.Added;
        //            _context.SaveChanges();

        //            objResult.StatusCode = 0;
        //            objResult.Message = "Key Master save successfully !!";
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
        ////get Key master details
        //[Route("Get_KeyMaster/{key_id}")]
        //[HttpGet]
        ////[Authorize(Policy = "8033")]
        //public IActionResult Get_KeyMaster([FromRoute] int key_id)
        //{
        //    try
        //    {
        //        if (key_id > 0)
        //        {
        //            var data = _context.tbl_key_master.Where(x => x.key_id == key_id).FirstOrDefault();
        //            return Ok(data);
        //        }
        //        else
        //        {
        //            var data = _context.tbl_key_master.Select(a => new { a.key_id, a.key_name, a.display_name, a.calling_function_name, a.forumal_name, a.description, a.data_type, a.type, a.is_active })
        //            .Where(a => a.type == 2).ToList();
        //            return Ok(data);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}
        //[Route("Update_KeyMaster")]
        //[HttpPost]
        ////[Authorize(Policy = "8034")]
        //public IActionResult Update_KeyMaster([FromBody] tbl_key_master objkeym)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    try
        //    {
        //        var exist = _context.tbl_key_master.Where(x => x.key_id == objkeym.key_id).FirstOrDefault();

        //        if (exist == null)
        //        {
        //            objResult.StatusCode = 1;
        //            objResult.Message = "Invalid Key id please try again !!";
        //            return Ok(objResult);
        //        }
        //        else
        //        {
        //            //check with Key Name in other Key id
        //            var duplicate = _context.tbl_key_master.Where(x => x.key_id != objkeym.key_id && x.key_name == objkeym.key_name).FirstOrDefault();
        //            if (duplicate != null)
        //            {
        //                objResult.StatusCode = 1;
        //                objResult.Message = "Key name exist in the system please check & try !!";
        //                return Ok(objResult);
        //            }

        //            //exist.key_name = objkeym.key_name;
        //            exist.key_name = objkeym.key_name;
        //            exist.display_name = objkeym.display_name;
        //            exist.description = objkeym.description;
        //            exist.calling_function_name = objkeym.calling_function_name;
        //            exist.data_type = objkeym.data_type;
        //            exist.forumal_name = objkeym.forumal_name;
        //            exist.is_active = Convert.ToInt32(objkeym.is_active);
        //            exist.modified_by = objkeym.modified_by;
        //            exist.modified_dt = DateTime.Now;

        //            _context.Entry(exist).State = EntityState.Modified;
        //            _context.SaveChanges();

        //            objResult.StatusCode = 0;
        //            objResult.Message = "Key Details updated successfully !!";
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
        //get Employee data
        [Route("Get_Employee/{emp_id}")]
        [HttpGet]
        //[Authorize(Policy = "8035")]
        public IActionResult Get_Employee([FromRoute] int emp_id)
        {
            try
            {

                var result = _context.tbl_employee_master.Join(_context.tbl_emp_officaial_sec, a => a.employee_id, b => b.employee_id, (a, b) => new
                {
                    a.employee_id,
                    a.emp_code,
                    //emp_name = b.employee_first_name + " " + b.employee_middle_name + " " + b.employee_last_name,
                    b.employee_first_name,
                    b.employee_middle_name,
                    b.employee_last_name
                }).Where(c => c.employee_id == emp_id).FirstOrDefault();
                return Ok(result);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        //Get Interest Rate
        [Route("GetInterestRate")]
        [HttpGet]
        //[Authorize(Policy = "8036")]
        public IActionResult GetInterestRate()
        {
            try
            {
                List<object> list = new List<object>();
                foreach (enum_interest_rate func in Enum.GetValues(typeof(enum_interest_rate)))
                {
                    int value = (int)Enum.Parse(typeof(enum_interest_rate), Enum.GetName(typeof(enum_interest_rate), func));
                    string Str = Enum.GetName(typeof(enum_interest_rate), value);

                    Type type = func.GetType();
                    MemberInfo[] memInfo = type.GetMember(func.ToString());

                    if (memInfo != null && memInfo.Length > 0)
                    {
                        object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (attrs != null && attrs.Length > 0)
                        {
                            string strvalue = ((DescriptionAttribute)attrs[0]).Description;
                            list.Add(new
                            {
                                FunValue = strvalue,
                                FunText = strvalue + " " + '%'
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
        //here
        //public void SavePendingApproval(int empid)
        //{
        //    try
        //    {
        //        string loan_id = "";
        //        string emp_id = "";
        //        int approval_order = 0;

        //        var result = _context.tbl_loan_request.Select(a => new { a.loan_req_id, a.req_emp_id, a.is_final_approval, a.created_dt })
        //            .Where(a => a.req_emp_id == empid && a.is_final_approval == 0).ToList().OrderByDescending(u => u.created_dt).Take(1);

        //        foreach (var item in result)
        //        {
        //            var Details = item;
        //            string[] arry = Details.ToString().Split(',');

        //            string a = arry[0];
        //            string[] loan = a.Split("=");
        //            loan_id = loan[1];

        //            string b = arry[1];
        //            string[] emp = b.Split("=");
        //            emp_id = emp[1];
        //        }


        //        for (int Index = 0; Index < 5; Index++)
        //        {
        //            tbl_loan_approval objapproval = new tbl_loan_approval();

        //            approval_order = approval_order + 1;

        //            objapproval.loan_req_id = Convert.ToInt32(loan_id);
        //            objapproval.emp_id = Convert.ToInt32(emp_id);
        //            objapproval.is_approve = 0;
        //            objapproval.created_dt = DateTime.Now;
        //            objapproval.created_by = 0;
        //            objapproval.last_modified_by = 0;
        //            //objapproval.last_modified_date = DateTime.Now;
        //            objapproval.approval_order = 0;
        //            objapproval.is_final_approver = 0;
        //            _context.Entry(objapproval).State = EntityState.Added;
        //            _context.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}




#region STARTED BY SUPRIYA ON 05-07-2019

        public void SavePendingApproval(int empid, int companyid)
        {
            try
            {
                string loan_id = "";
                string emp_id = "";
                int approval_order = 0;
                int approval_role_id = 0;

                var result = _context.tbl_loan_request.Where(x => x.req_emp_id == empid && x.is_final_approval == 0 && x.is_deleted == 0 && x.is_closed == 0).Select(a => new
                {
                    a.loan_req_id,
                    a.req_emp_id,
                    a.is_final_approval,
                    a.created_dt,
                    a.emp_master
                }).OrderByDescending(u => u.created_dt).Take(1);

                //var result = _context.tbl_loan_request.Select(a => new { a.loan_req_id, a.req_emp_id, a.is_final_approval, a.created_dt, a.emp_master })
                //    .Where(a => a.req_emp_id == empid && a.is_final_approval == 0).ToList().OrderByDescending(u => u.created_dt).Take(1);

                foreach (var item in result)
                {
                    var Details = item;
                    string[] arry = Details.ToString().Split(',');

                    string a = arry[0];
                    string[] loan = a.Split("=");
                    loan_id = loan[1];

                    string b = arry[1];
                    string[] emp = b.Split("=");
                    emp_id = emp[1];


                }

                // int company_iddd = _context.tbl_employee_company_map.Where(a => a.employee_id == empid && a.is_deleted==0).Select(a => a.company_id).FirstOrDefault()??0;


                var approver_orders = _context.tbl_loan_approval_setting.Where(g => g.company_id == companyid && g.approver_type == 1 && g.is_active == 1).OrderBy(h => h.order).Select(p => new { p.order, p.approver_role_id }).ToList();

                List<tbl_loan_approval> objapproval = approver_orders.Select(p => new tbl_loan_approval
                {
                    approval_order = Convert.ToByte(p.order),
                    approver_role_id = p.approver_role_id ?? 0,
                    loan_req_id = Convert.ToInt32(loan_id),
                    emp_id = Convert.ToInt32(emp_id),
                    is_approve = 0,
                    created_by = _clsCurrentUser.EmpId,
                    created_dt = DateTime.Now,
                    last_modified_by = 0,
                    last_modified_date = Convert.ToDateTime("01-01-2000"),
                    is_final_approver = 0,
                }).ToList();

                _context.tbl_loan_approval.AddRange(objapproval);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }


#endregion ENDED BY SUPRIYA ON 05-07-2019





        [Route("Save_LoanRequest")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LoanRequest))]
        // Here Save Loan Request details
        public IActionResult Save_LoanRequest([FromBody] LoanRequest objloan)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(objloan.company_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access....!!";
                return Ok(objResult);
            }

            if (!_clsCurrentUser.DownlineEmpId.Contains(objloan.req_emp_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access....!!";
                return Ok(objResult);
            }

            try
            {
                int companyid = 0;
                companyid = objloan.company_id;

                var check_approver_orderavailability = _context.tbl_loan_approval_setting.Where(x => x.company_id == companyid && x.approver_type == 1 && x.is_active == 1).ToList();
                if (check_approver_orderavailability.Count > 0)
                {
                    var check_final_approver = check_approver_orderavailability.Where(q => q.is_final_approver != 0 && q.is_active == 1).Select(p => p.is_final_approver).FirstOrDefault();
                    if (check_final_approver != 0)
                    {
                        var monthly_emi = clsLoanCalculator.LoanEMICalculator((float)objloan.loan_amt, (float)objloan.loan_tenure, (float)objloan.interest_rate);
                        if (monthly_emi.ToString() == "NaN") monthly_emi = 0;
                        var exist = _context.tbl_loan_request.Where(x => x.loan_type == objloan.loan_type && x.req_emp_id == objloan.req_emp_id && x.is_closed == 0 && x.is_deleted == 0).FirstOrDefault();

                        if (exist != null)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Loan Request already exist !!";
                            //   return Ok(objResult);
                        }
                        else
                        {
                            tbl_loan_request objloanreq = new tbl_loan_request();
                            objloanreq.req_emp_id = objloan.req_emp_id;
                            objloanreq.emp_code = objloan.emp_code;
                            objloanreq.loan_type = objloan.loan_type;
                            objloanreq.loan_amt = Math.Round(objloan.loan_amt, 2);
                            objloanreq.loan_tenure = Convert.ToInt32(objloan.loan_tenure);
                            objloanreq.loan_purpose = objloan.loan_purpose;
                            objloanreq.is_final_approval = 0;
                            objloanreq.is_closed = 0;
                            objloanreq.interest_rate = objloan.interest_rate;
                            objloanreq.monthly_emi = Math.Round(monthly_emi, 0);
                            objloanreq.is_deleted = objloan.is_deleted;
                            objloanreq.created_dt = DateTime.Now;
                            objloanreq.created_by = objloan.created_by;
                            objloanreq.start_date = objloan.start_date;
                            objloanreq.last_modified_by = 0;
                            objloanreq.last_modified_date = DateTime.Now;

                            _context.Entry(objloanreq).State = EntityState.Added;




                            _context.SaveChanges();
                            SavePendingApproval(Convert.ToInt32(objloan.req_emp_id), Convert.ToInt32(objloan.company_id));
                            objResult.StatusCode = 0;
                            objResult.Message = "Loan Request save successfully !!";
                            //SavePendingApproval(Convert.ToInt32(objloan.req_emp_id));



                        }
                    }
                    else
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Create Final Approver of Company of Select Employee";
                    }
                }
                else
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Firstly Create Loan Approver";
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
        [Route("Update_LoanRequest")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LoanRequest))]
        public IActionResult Update_LoanRequest([FromBody] tbl_loan_request objloan)
        {
            Response_Msg objResult = new Response_Msg();
            try
            {
                var exist = _context.tbl_loan_request.Where(x => x.loan_req_id == objloan.loan_req_id && x.req_emp_id == objloan.req_emp_id).FirstOrDefault();

                {

                    exist.loan_req_id = objloan.loan_req_id;
                    exist.req_emp_id = objloan.req_emp_id;
                    exist.emp_code = objloan.emp_code;
                    exist.loan_type = objloan.loan_type;
                    exist.loan_tenure = objloan.loan_tenure;
                    exist.loan_purpose = objloan.loan_purpose;
                    exist.loan_amt = objloan.loan_amt;
                    exist.interest_rate = objloan.interest_rate;
                    exist.start_date = objloan.start_date;
                    exist.is_deleted = objloan.is_deleted;
                    exist.last_modified_by = objloan.last_modified_by;
                    exist.last_modified_date = DateTime.Now;
                    _context.Entry(exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Loan Request Details updated successfully !!";
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
        //Get Loan Request details
        [Route("Get_LoanRequest/{loan_req_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LoanRequest))]
        public IActionResult Get_LoanRequest([FromRoute] int loan_req_id)
        {
            try
            {
                if (loan_req_id > 0)
                {
                    var result = _context.tbl_loan_request.Where(c => c.loan_req_id == loan_req_id && c.is_deleted == 0).Select(a => new
                    {
                        a.loan_req_id,
                        a.req_emp_id,
                        a.emp_code,
                        loan_type_ud = a.loan_type,
                        loan_type = a.loan_type == 1 ? "Loan" : a.loan_type == 2 ? "Advance" : "",
                        a.loan_tenure,
                        a.loan_amt,
                        a.loan_purpose,
                        a.interest_rate,
                        a.start_date,

                    }).FirstOrDefault();


                    return Ok(result);
                }
                else
                {

                    var result = _context.tbl_loan_request.Where(x => x.is_deleted == 0 && _clsCurrentUser.DownlineEmpId.Contains(x.req_emp_id ?? 0)).Select(p => new
                    {
                        p.loan_req_id,
                        p.req_emp_id,
                        p.emp_code,
                        p.loan_type,
                        p.loan_amt,
                        p.loan_tenure,
                        p.loan_purpose,
                        p.interest_rate,
                        p.is_closed,
                        p.is_final_approval,
                        p.created_dt,
                        p.last_modified_date,
                        p.start_date
                    }).OrderBy(y => y.loan_req_id).ToList();


                    return Ok(result);
                }


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        //Get Loan Type Rate of Interest details
        [Route("Get_LoanType_ROI/{loan_req_id}/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8040")]
        public IActionResult Get_LoanType_ROI([FromRoute] int loan_req_id, int company_id)
        {
            try
            {

                var result = _context.tbl_loan_request_master.Select(a => new
                {
                    a.sno,
                    a.em_status,
                    a.loan_type,
                    loan_name = a.loan_type == 1 ? "Loan" : "Advance",
                    a.max_tenure,
                    a.min_top_up_duration,
                    a.rate_of_interest,
                    a.created_by,
                    a.created_dt,
                    a.companyid,
                    a.loan_amount
                }).Where(b => b.companyid == company_id).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        //Get Loan Request details
        [Route("Get_LoanRequestDetails/{login_emp_id}/{login_emp_role_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LoanApproval))]
        public IActionResult Get_LoanRequestDetails([FromRoute] int login_emp_id, int login_emp_role_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(login_emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }

            if (!_clsCurrentUser.RoleId.Contains(login_emp_role_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }
            try
            {
                var approver_id = _context.tbl_loan_approval_setting.Where(x => x.is_active == 1 && x.approver_type == 1 && (x.emp_id == _clsCurrentUser.EmpId || _clsCurrentUser.RoleId.Contains(x.approver_role_id ?? 0))).FirstOrDefault();
                if (approver_id != null)
                {
                    List<LoanRequest> objrequestlist = _context.tbl_loan_approval.Where(x => x.is_deleted == 0 && x.approval_order == approver_id.order && x.is_approve == 0 && x.is_final_approver == 0 && x.tbl_loan_request.is_closed == 0).Select(p => new LoanRequest
                    {

                        sno = p.sno,
                        loan_req_id = p.loan_req_id ?? 0,
                        emp_id = p.emp_id ?? 0,
                        emp_code = string.Format("{0} {1} {2} ({3})", p.tbl_employee_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                        p.tbl_employee_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                        p.tbl_employee_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                        p.tbl_employee_master.emp_code),
                        //a.is_final_approver,
                        is_final_approver = p.tbl_loan_request.is_final_approval,
                        is_approve = p.is_approve,
                        approval_order = p.approval_order,
                        created_dt = p.created_dt,
                        last_modified_date = p.last_modified_date,
                        loan_type = p.tbl_loan_request.loan_type,
                        loan_amt = p.tbl_loan_request.loan_amt,
                        interest_rate = p.tbl_loan_request.interest_rate,
                        loan_tenure = p.tbl_loan_request.loan_tenure,
                        loan_purpose = p.tbl_loan_request.loan_purpose,
                        start_date = p.tbl_loan_request.start_date,
                        is_deleted = p.is_deleted,
                        is_closed = p.tbl_loan_request.is_closed,
                        approver_role_id = p.approver_role_id,

                    }).ToList();

                    return Ok(objrequestlist);
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Your are not an approver of any Loan application";
                    return Ok(objresponse);
                }

                //var result = _context.tbl_loan_approval.Join(_context.tbl_loan_approval_setting, a => a.approval_order, b => b.order, (a, b) => new
                //{

                //    a.sno,
                //    a.loan_req_id,
                //    a.emp_id,
                //    a.tbl_loan_request.emp_code,
                //    //a.is_final_approver,
                //    a.tbl_loan_request.is_final_approval,
                //    a.is_approve,
                //    a.approval_order,
                //    a.created_dt,
                //    a.last_modified_date,
                //    a.tbl_loan_request.loan_type,
                //    a.tbl_loan_request.loan_amt,
                //    a.tbl_loan_request.interest_rate,
                //    a.tbl_loan_request.loan_tenure,
                //    a.tbl_loan_request.loan_purpose,
                //    a.tbl_loan_request.start_date,
                //    approver_emp_id = b.emp_id,
                //    a.is_deleted,
                //    a.tbl_loan_request.is_closed,
                //    b.approver_type,
                //    b.approver_role_id,
                //   approver_active= b.is_active

                //}).Where(x => x.is_deleted == 0 && x.approver_type == 1 && x.approver_active==1 && (x.approver_emp_id == login_emp_id || x.approver_role_id == login_emp_role_id)).OrderBy(y => y.sno).ToList();



                //List<LoanRequest> objrequestlist = new List<LoanRequest>();

                //if (result.Count > 0)
                //{
                //    foreach (var item in result)
                //    {
                //        bool containsItem = objrequestlist.Any(x => x.loan_req_id == item.loan_req_id);
                //        if (!containsItem)
                //        {

                //            LoanRequest objrequest = new LoanRequest();

                //            objrequest.sno = item.sno;
                //            objrequest.loan_req_id = Convert.ToInt32(item.loan_req_id);
                //            objrequest.emp_id = Convert.ToInt32(item.emp_id);
                //            objrequest.emp_code = item.emp_code;
                //            objrequest.is_final_approver = item.is_final_approval;
                //            objrequest.is_approve = item.is_approve;
                //            objrequest.approval_order = item.approval_order;
                //            objrequest.created_dt = item.created_dt;
                //            objrequest.last_modified_date = item.last_modified_date;
                //            objrequest.loan_type = item.loan_type;
                //            objrequest.loan_amt = item.loan_amt;
                //            objrequest.interest_rate = item.interest_rate;
                //            objrequest.loan_tenure = item.loan_tenure;
                //            objrequest.loan_purpose = item.loan_purpose;
                //            objrequest.start_date = item.start_date;
                //            objrequest.approver_emp_id = item.approver_emp_id;
                //            objrequest.is_deleted = item.is_deleted;
                //            objrequest.is_closed = item.is_closed;
                //            objrequest.approver_type = item.approver_type;
                //            objrequest.approver_role_id = item.approver_role_id;



                //            objrequestlist.Add(objrequest);
                //        }
                //        else
                //        {
                //            if (objrequestlist.Where(a => a.loan_req_id == item.loan_req_id && a.is_approve == 1).Count() > 0)
                //            {
                //                objrequestlist.Remove(objrequestlist.Single(s => s.loan_req_id == item.loan_req_id));

                //                LoanRequest objrequest = new LoanRequest();

                //                objrequest.sno = item.sno;
                //                objrequest.loan_req_id = Convert.ToInt32(item.loan_req_id);
                //                objrequest.emp_id = Convert.ToInt32(item.emp_id);
                //                objrequest.emp_code = item.emp_code;
                //                objrequest.is_final_approver = item.is_final_approval;
                //                objrequest.is_approve = item.is_approve;
                //                objrequest.approval_order = item.approval_order;
                //                objrequest.created_dt = item.created_dt;
                //                objrequest.last_modified_date = item.last_modified_date;
                //                objrequest.loan_type = item.loan_type;
                //                objrequest.loan_amt = item.loan_amt;
                //                objrequest.interest_rate = item.interest_rate;
                //                objrequest.loan_tenure = item.loan_tenure;
                //                objrequest.loan_purpose = item.loan_purpose;
                //                objrequest.start_date = item.start_date;
                //                objrequest.approver_emp_id = item.approver_emp_id;
                //                objrequest.is_deleted = item.is_deleted;
                //                objrequest.is_closed = item.is_closed;
                //                objrequest.approver_type = item.approver_type;
                //                objrequest.approver_role_id = item.approver_role_id;



                //                objrequestlist.Add(objrequest);
                //            }
                //        }

                //    }
                //}



            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }
        //get salary Revision 
        [Route("Get_SalaryRevisionEmpData/{emp_id}/{_effective_date}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.EmpSalary))]
        public IActionResult Get_SalaryRevisionEmpData([FromRoute] int emp_id, DateTime _effective_date)
        {
            throw new NotImplementedException();
#if false
            try
            {

                var result = _context.tbl_sg_maping.Where(x => x.emp_id == emp_id && x.is_active == 1 && x.applicable_from_dt <= _effective_date).
                    OrderByDescending(x => x.map_id).Select(p => new
                    {

                        map_id = p.map_id,
                        salary_group_id = p.salary_group_id,
                        group_name = p.tsg.group_name,
                        p.emp_id,
                        combine_dtl = p.tem.tbl_emp_officaial_sec.OrderByDescending(x => x.emp_official_section_id).Where(x => x.employee_id == emp_id && x.is_deleted == 0).Select(a => new { a.date_of_joining, dept = a.department_id, dept_name = a.tbl_department_master.department_name }),
                        designation_id = p.tem.tbl_emp_desi_allocation.OrderByDescending(x => x.emp_grade_id).Where(x => x.employee_id == emp_id && x.tbl_designation_master.is_active == 1 && x.applicable_from_date <= _effective_date && _effective_date <= x.applicable_to_date).GroupBy(g => g.employee_id).Select(g => g.OrderByDescending(t => t.emp_grade_id).FirstOrDefault()).Select(g => g.desig_id),
                        des_name = p.tem.tbl_emp_desi_allocation.OrderByDescending(x => x.emp_grade_id).FirstOrDefault(x => x.employee_id == emp_id && x.tbl_designation_master.is_active == 1 && x.applicable_from_date <= _effective_date && _effective_date <= x.applicable_to_date).tbl_designation_master.designation_name,
                        grade_id = p.tem.tbl_emp_grade_allocation.OrderByDescending(x => x.emp_grade_id).Where(x => x.employee_id == emp_id && x.tbl_grade_master.is_active == 1 && x.applicable_from_date <= _effective_date && _effective_date <= x.applicable_to_date).GroupBy(g => g.employee_id).Select(k => k.OrderByDescending(t => t.emp_grade_id).FirstOrDefault()).FirstOrDefault().grade_id,
                        grade_name = p.tem.tbl_emp_grade_allocation.OrderByDescending(x => x.emp_grade_id).FirstOrDefault(x => x.employee_id == emp_id && x.tbl_grade_master.is_active == 1 && x.applicable_from_date <= _effective_date && _effective_date <= x.applicable_to_date).tbl_grade_master.grade_name,
                        // dept=p.tem.tbl_emp_officaial_sec.OrderByDescending(x=>x.emp_official_section_id).FirstOrDefault(x=>x.employee_id==emp_id && x.is_deleted==0 && !string.IsNullOrEmpty(x.employee_first_name)).department_id,
                        //dept_name = p.tem.tbl_emp_officaial_sec.OrderByDescending(x => x.emp_official_section_id).FirstOrDefault(x => x.employee_id == emp_id && x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).tbl_department_master.department_name,
                    }).FirstOrDefault();




                if (result == null)
                {
                    var data = _context.tbl_employee_master.Where(x => x.employee_id == emp_id && x.is_active == 1).Select(p => new
                    {

                        emp_id = p.employee_id,

                        combine_dtl = p.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).Select(q => new { date_of_joining = q.date_of_joining, q.department_id, dept_name = q.tbl_department_master.department_name }),
                        p.tbl_emp_desi_allocation.OrderByDescending(x => x.emp_grade_id).FirstOrDefault(x => x.tbl_designation_master.is_active == 1 && x.applicable_from_date <= _effective_date && _effective_date <= x.applicable_to_date).tbl_designation_master.designation_name,
                        p.tbl_emp_grade_allocation.OrderByDescending(x => x.emp_grade_id).FirstOrDefault(x => x.tbl_grade_master.is_active == 1 && x.applicable_from_date <= _effective_date && _effective_date <= x.applicable_to_date).tbl_grade_master.grade_name,

                    }).FirstOrDefault();

                    var result1 = new { emp_id = data.emp_id, combine_dtl = data.combine_dtl, des_name = data.designation_name, grade_name = data.grade_name };

                    return Ok(result1);
                }

                // comment start on 24-10-2019
                //var result1 = _context.tbl_sg_maping.Where(a => a.is_active == 1 && a.emp_id == emp_id).OrderByDescending(a => a.applicable_from_dt)
                //    .Select(b => new
                //    {
                //       // b.map_id,
                //       // salary_group_id = b.salary_group_id,
                //       // group_name = _context.tbl_salary_group.Where(a => a.is_active == 1 && a.group_id == b.salary_group_id).Select(a => a.group_name).FirstOrDefault(),

                //       // grade_name = _context.tbl_grade_master.Where(g => g.is_active == 1 && g.grade_id == b.tem.tbl_emp_grade_allocation.Where(a => a.employee_id == emp_id).OrderByDescending(a => a.emp_grade_id).Select(a => a.grade_id).FirstOrDefault()).Select(f => f.grade_name).FirstOrDefault(),
                //        //emp_grade_id = b.tem.tbl_emp_grade_allocation.Where(a => a.employee_id == emp_id).OrderByDescending(a => a.emp_grade_id).Select(a => a.emp_grade_id).FirstOrDefault(),

                //        dept = b.tem.tbl_emp_officaial_sec.Where(a => a.is_deleted == 0 && a.employee_id == emp_id).Select(a => a.department_id).FirstOrDefault(),
                //        dept_name = _context.tbl_department_master.Where(g => g.is_active == 1 && g.department_id == b.tem.tbl_emp_officaial_sec.Where(a => a.is_deleted == 0 && a.employee_id == emp_id).Select(a => a.department_id).FirstOrDefault()).Select(a => a.department_name).FirstOrDefault(),

                //        //designation_id = b.tem.tbl_emp_desi_allocation.Where(a => a.employee_id == emp_id).OrderByDescending(a => a.emp_grade_id).Select(a => a.desig_id).FirstOrDefault(),

                //       // des_name = _context.tbl_designation_master.Where(g => g.is_active == 1 && g.designation_id == b.tem.tbl_emp_desi_allocation.Where(a => a.employee_id == emp_id).OrderByDescending(a => a.emp_grade_id).Select(a => a.desig_id).FirstOrDefault()).Select(a => a.designation_name).FirstOrDefault(),

                //        //emp_id = b.emp_id,
                //        emp_code = b.tem.emp_code,
                //        effective_dt = b.applicable_from_dt,
                //        emp_name = b.tem.tbl_emp_officaial_sec.Where(c => c.is_deleted == 0 && c.employee_id == b.emp_id).Select(d => d.employee_first_name).FirstOrDefault(),
                //       // joining_date = b.tem.tbl_emp_officaial_sec.Where(c => c.is_deleted == 0 && c.employee_id == b.emp_id).Select(d => d.date_of_joining).FirstOrDefault(),
                //        default_company_id = b.tem.tbl_user_master.Where(e => e.is_active == 1 && e.employee_id == b.emp_id).Select(f => f.default_company_id).FirstOrDefault(),
                //        //grade_id = b.tsg.grade_Id
                //    }).ToList();

                // comment end on 24-10-2019

                //var result = (from s in _context.tbl_sg_maping
                //              join g in _context.tbl_salary_group
                //              on s.salary_group_id equals g.group_id
                //              join e in _context.tbl_emp_officaial_sec
                //              on s.emp_id equals e.employee_id
                //              join a in _context.tbl_emp_grade_allocation on e.employee_id equals a.employee_id
                //              join d in _context.tbl_grade_master on a.grade_id equals d.grade_id
                //              join dept in _context.tbl_department_master on e.department_id equals dept.department_id
                //              join des in _context.tbl_emp_desi_allocation on e.employee_id equals des.employee_id
                //              join x in _context.tbl_designation_master on des.desig_id equals x.designation_id
                //              where s.emp_id == emp_id && e.is_deleted == 0
                //              select new
                //              {
                //                  s.map_id,
                //                  g.group_id,
                //                  e.emp_official_section_id,
                //                  a.emp_grade_id,
                //                  dept.department_id,
                //                  x.designation_id,
                //                  emp_id = e.employee_id,
                //                  dept_name = dept.department_name,
                //                  salary_group_id = g.group_id,
                //                  group_name = g.group_name,
                //                  effective_dt = s.applicable_from_dt,
                //                  emp_name = e.employee_first_name,
                //                  grade_id = d.grade_id,
                //                  grade_name = d.grade_name,
                //                  is_active = s.is_active,
                //                  created_by = s.created_by,
                //                  created_dt = s.created_dt,
                //                  joining_date = e.date_of_joining,
                //                  des_name = x.designation_name

                //              }).OrderByDescending(a => a.emp_grade_id).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
#endif
        }

        [HttpGet]
        [Route("GetComponentsByempId/{emp_id}/{reqDate}")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpSalary))]
        public IActionResult GetComponentsByempId([FromRoute] int emp_id, DateTime? reqDate)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...";
                return Ok(objresponse);
            }
            if (reqDate == null)
            {
                reqDate = Convert.ToDateTime(DateTime.Now.ToString("01-MMM-yyyy"));

            }

            ResponseMsg objResult = new ResponseMsg();
            DateTime currentTime = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));
            DateTime defaultDate = Convert.ToDateTime("1-jan-2000");
            try
            {

                List<tbl_component_master> tcm = _context.tbl_component_master.Where(a => a.is_salary_comp == 1 && a.is_active == 1).ToList();
                if (tcm.Count() == 0)
                {
                    return BadRequest("salary component not define");
                }
                List<tbl_emp_salary_master> tesms = _context.tbl_emp_salary_master.Where(a => a.emp_id == emp_id && a.is_active == 1 && a.applicable_from_dt <= reqDate).OrderByDescending(p => p.applicable_from_dt).Take(200).ToList();
                List<tbl_emp_salary_master> tesms_current = _context.tbl_emp_salary_master.Where(a => a.emp_id == emp_id && a.is_active != 1 && a.applicable_from_dt == reqDate).OrderByDescending(p => p.sno).ToList();
                if (tesms.Count() > 0)
                {
                    DateTime lastApplicabledt = tesms.Select(p => p.applicable_from_dt.Date).Max();
                    List<tbl_emp_salary_master> tesm = tesms.Where(p => p.applicable_from_dt.Date == lastApplicabledt.Date).ToList();

                    dynamic result = "";
                    if (tesms_current.Count > 0)
                    {
                        result = (from a in tcm
                                  join c in tesms_current on a.component_id equals c.component_id into Group1
                                  join b in tesm on a.component_id equals b.component_id into Group
                                  from m in Group.DefaultIfEmpty()
                                  from m1 in Group1.DefaultIfEmpty()
                                  select new
                                  {
                                      component_id = a.component_id,
                                      property_details = a.property_details,
                                      salary_group = a.is_salary_comp,
                                      is_active = a.is_active,
                                      emp_id = emp_id,
                                      applicable_dt = m != null ? m.applicable_from_dt : defaultDate,
                                      applicable_value = m != null ? m.applicable_value : 0,
                                      requested_dt = m != null ? m1.applicable_from_dt : defaultDate,
                                      requested_value = m != null ? m1.applicable_value : 0,
                                  }
                                 ).ToList();
                    }
                    else
                    {
                        result = (from a in tcm
                                  join b in tesm on a.component_id equals b.component_id into Group
                                  from m in Group.DefaultIfEmpty()
                                  select new
                                  {
                                      component_id = a.component_id,
                                      property_details = a.property_details,
                                      salary_group = a.is_salary_comp,
                                      is_active = a.is_active,
                                      emp_id = emp_id,
                                      applicable_dt = m != null ? m.applicable_from_dt : defaultDate,
                                      applicable_value = m != null ? m.applicable_value : 0,
                                      requested_dt = m != null ? m.applicable_from_dt : defaultDate,
                                      requested_value = m != null ? 0 : 0,
                                  }
                                ).ToList();
                    }
                    return Ok(result);

                }
                else
                {
                    return Ok(tcm.Select(b => new { b.component_id, b.property_details, salary_group = 1, is_active = 1, emp_id = emp_id, applicable_dt = defaultDate, applicable_value = 0, requested_dt = defaultDate, requested_value = 0 }));
                }
            }
            catch (Exception ex)
            {
                objResult.Message = ex.ToString();
                return Ok(objResult);
            }
        }
        //here
        [HttpGet("GetSalaryReqReport")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpSalary))]
        public IActionResult GetSalaryReqReport()
        {
            ResponseMsg objresponse = new ResponseMsg();

            //if (!_clsCurrentUser.DownlineEmpId.Contains(objrpt.emp_id))
            //{
            //    objresponse.StatusCode = 1;
            //    objresponse.Message = "Unauthorize Access....!!";
            //    return Ok(objresponse);
            //}

            try
            {
                DataTable dt = new DataTable();

                var data = _context.tbl_emp_salary_master.Select(a => new { a.component_id, a.component_master.property_details, applicable_value = Math.Round(a.applicable_value, 2), a.applicable_from_dt, a.is_active, a.emp_id, a.maker_remark })
                .Where(a => a.is_active == 2).ToList();


                var d = (from f in data
                         group f by new { f.applicable_from_dt, f.emp_id, f.maker_remark }
               into myGroup
                         where myGroup.Count() > 0
                         select new
                         {
                             myGroup.Key.applicable_from_dt,
                             myGroup.Key.emp_id,
                             company_id = _context.tbl_employee_company_map.FirstOrDefault(x => x.employee_id == myGroup.Key.emp_id && x.is_deleted == 0).company_id,
                             emp_code = _context.tbl_employee_master.FirstOrDefault(x => x.employee_id == myGroup.Key.emp_id && x.is_active == 1).emp_code,
                             emp_name = _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.employee_id == myGroup.Key.emp_id && x.is_deleted == 0).employee_first_name + ' ' +
                                        _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.employee_id == myGroup.Key.emp_id && x.is_deleted == 0).employee_middle_name + ' ' +
                                        _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.employee_id == myGroup.Key.emp_id && x.is_deleted == 0).employee_last_name,
                             maker_remark = myGroup.Key.maker_remark != null ? myGroup.Key.maker_remark : "",
                             component = myGroup.GroupBy(f => f.property_details).Select
                        (m => new { comp = m.Key, applicable_value = m.Sum(c => c.applicable_value) })
                         }).ToList();
                var component = _context.tbl_emp_salary_master.Select(a => new { a.component_master.property_details, a.is_active, a.emp_id, a.applicable_from_dt })
                .Where(a => a.is_active == 2).ToList();

                ArrayList objDataColumn = new ArrayList();

                if (data.Count() > 0)
                {

                    objDataColumn.Add("applicable_from_dt");
                    objDataColumn.Add("emp_id");
                    objDataColumn.Add("emp_code");
                    objDataColumn.Add("emp_name");
                    objDataColumn.Add("maker_remark");
                    objDataColumn.Add("company_id");
                    // objDataColumn.Add("applicable date");
                    //Add Subject Name as column in Datatable
                    for (int i = 0; i < component.Count; i++)
                    {
                        objDataColumn.Add(component[i].property_details.ToLower().Replace(" ", "_").ToString());
                    }
                }
                //Add dynamic columns name to datatable dt
                for (int i = 0; i < objDataColumn.Count; i++)
                {
                    if (!(dt.Columns.Contains(objDataColumn[i].ToString())))
                    {
                        dt.Columns.Add(objDataColumn[i].ToString());
                    }

                }
                //Add data into datatable with respect to dynamic columns and dynamic data
                List<object> tempList = new List<object>();
                for (int i = 0; i < d.Count; i++)
                {
                    tempList.Add(d[i].applicable_from_dt.ToString());
                    tempList.Add(d[i].emp_id.ToString());
                    tempList.Add(d[i].emp_code.ToString());
                    tempList.Add(d[i].emp_name.ToString());
                    tempList.Add(d[i].maker_remark.ToString());
                    tempList.Add(d[i].company_id.ToString());

                    var res = d[i].component.ToList();
                    for (int j = 0; j < res.Count; j++)
                    {
                        tempList.Add(res[j].applicable_value.ToString());
                    }

                    dt.Rows.Add(tempList.ToArray<object>());
                    tempList.Clear();

                }
                List<object> column = new List<object>();
                for (int index = 0; index < dt.Columns.Count; index++)
                {
                    column.Add(new
                    {
                        title = dt.Columns[index].ToString().ToUpper().Replace("_", " "),
                        data = dt.Columns[index].ToString()
                    });
                }

                return Ok(dt);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("ApproveSalaryRequest")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.EmpSalary))]
        public IActionResult ApproveSalaryRequest([FromBody] SalaryRevison objsal)
        {
            Response_Msg objResult = new Response_Msg();
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    if (objsal.emp_ids.Count() != objsal.applicable_dates.Count())
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Invalid Requests Please Try Again !...";
                        return Ok(objResult);
                    }

                    for (int i = 0; i < objsal.emp_ids.Count(); i++)
                    {
                        //start Check month salary of effective from date is freezed or not.
                        int CurrentMonthYear = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));
                        if (Convert.ToInt32(objsal.applicable_dates[i].ToString("yyyyMM")) <= CurrentMonthYear)
                        {
                            var _salarycalculate = _context.tbl_payroll_process_status.Where(x => x.payroll_month_year == Convert.ToInt32(objsal.applicable_dates[i].ToString("yyyyMM")) && x.emp_id == objsal.emp_ids[i] && x.is_deleted == 0 && (x.is_calculated == 1 || x.is_freezed == 1)).FirstOrDefault();
                            if (_salarycalculate != null)
                            {
                                objResult.StatusCode = 1;
                                objResult.Message = "Selected Month Salary is already calculated or freezed!...";
                                return Ok(objResult);
                            }
                        }


                        //end

                        DateTime applicable_date = Convert.ToDateTime(objsal.applicable_dates[i].ToString("01-MMM-yyyy"));

                        var exist = _context.tbl_emp_salary_master.Where(x => x.emp_id == objsal.emp_ids[i] && x.applicable_from_dt == applicable_date && x.is_active == 1).FirstOrDefault();

                        if (exist != null)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "Salary is already revised for the month " + applicable_date.ToString("MMM-yyyy");
                            return Ok(objResult);
                        }
                        else
                        {
                            DateTime currentdt = DateTime.Now;


                            //get all already mapped role data
                            var empSalDetail = _context.tbl_emp_salary_master.Where(p => p.emp_id == objsal.emp_ids[i] && p.applicable_from_dt == applicable_date).ToList();

                            empSalDetail.Where(x => x.emp_id == objsal.emp_ids[i] && x.applicable_from_dt == applicable_date).ToList().ForEach(p =>
                            {
                                p.is_active = objsal.is_active;
                                p.modified_by = _clsCurrentUser.EmpId;
                                p.modified_dt = DateTime.Now;
                                p.checker_remark = objsal.checker_remark;
                            });

                            //now update
                            _context.tbl_emp_salary_master.UpdateRange(empSalDetail);
                            _context.SaveChanges();
                        }
                    }

                    trans.Commit();
                    objResult.Message = "Salary Request Updated Successfully !!";
                    objResult.StatusCode = 0;
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

        [HttpPost("GetSalaryReport")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpSalary))]
        public IActionResult GetSalaryReport([FromBody] SalaryReport objrpt)
        {
            ResponseMsg objresponse = new ResponseMsg();

            if (!_clsCurrentUser.DownlineEmpId.Contains(objrpt.emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access....!!";
                return Ok(objresponse);
            }

            try
            {
                DataTable dt = new DataTable();

                var data = _context.tbl_emp_salary_master.Select(a => new { a.component_id, a.component_master.property_details, applicable_value = Math.Round(a.applicable_value, 2), a.applicable_from_dt, a.is_active, a.emp_id, a.maker_remark, a.checker_remark })
                .Where(a => (a.is_active == 1 || a.is_active == 0) && a.emp_id == objrpt.emp_id && objrpt.FromDate.Date <= a.applicable_from_dt.Date && a.applicable_from_dt.Date <= objrpt.ToDate.Date).ToList();


                var d = (from f in data
                         group f by new { f.applicable_from_dt, f.maker_remark, f.checker_remark, f.is_active }
               into myGroup
                         where myGroup.Count() > 0
                         select new
                         {
                             myGroup.Key.applicable_from_dt,
                             maker_remark = myGroup.Key.maker_remark != null ? myGroup.Key.maker_remark : "",
                             checker_remark = myGroup.Key.checker_remark != null ? myGroup.Key.checker_remark : "",
                             _status = myGroup.Key.is_active == 0 ? "Rejected" : myGroup.Key.is_active == 1 ? "Approved" : "",
                             //myGroup.Key.emp_id,
                             //emp_name = _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.employee_id == myGroup.Key.emp_id && x.is_deleted == 0).employee_first_name + ' ' +
                             //         _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.employee_id == myGroup.Key.emp_id && x.is_deleted == 0).employee_middle_name + ' ' +
                             //       _context.tbl_emp_officaial_sec.FirstOrDefault(x => x.employee_id == myGroup.Key.emp_id && x.is_deleted == 0).employee_last_name,
                             component = myGroup.GroupBy(f => f.property_details).Select
                        (m => new { comp = m.Key, applicable_value = m.Sum(c => c.applicable_value) })
                         }).ToList();
                var component = _context.tbl_emp_salary_master.Select(a => new { a.component_master.property_details, a.is_active, a.emp_id, a.applicable_from_dt, a.maker_remark })
                .Where(a => (a.is_active == 1 || a.is_active == 0) && a.emp_id == objrpt.emp_id && a.applicable_from_dt >= objrpt.FromDate && a.applicable_from_dt <= objrpt.ToDate).ToList();

                ArrayList objDataColumn = new ArrayList();

                if (data.Count() > 0)
                {

                    objDataColumn.Add("applicable_from_dt");
                    objDataColumn.Add("maker_remark");
                    objDataColumn.Add("checker_remark");
                    objDataColumn.Add("_status");
                    //objDataColumn.Add("emp_id");
                    //objDataColumn.Add("emp_name");
                    // objDataColumn.Add("applicable date");
                    //Add Subject Name as column in Datatable
                    for (int i = 0; i < component.Count; i++)
                    {
                        objDataColumn.Add(component[i].property_details.ToLower().Replace(" ", "_").ToString());
                    }
                }
                //Add dynamic columns name to datatable dt
                for (int i = 0; i < objDataColumn.Count; i++)
                {
                    if (!(dt.Columns.Contains(objDataColumn[i].ToString())))
                    {
                        dt.Columns.Add(objDataColumn[i].ToString());
                    }

                }
                //Add data into datatable with respect to dynamic columns and dynamic data
                List<object> tempList = new List<object>();
                for (int i = 0; i < d.Count; i++)
                {
                    tempList.Add(d[i].applicable_from_dt.ToString());
                    tempList.Add(d[i].maker_remark.ToString());
                    tempList.Add(d[i].checker_remark.ToString());
                    tempList.Add(d[i]._status.ToString());

                    //tempList.Add(d[i].emp_id.ToString());
                    //tempList.Add(d[i].emp_name.ToString());

                    var res = d[i].component.ToList();
                    if (component.Count > 0)
                    {
                        for (int j = 0; j < res.Count; j++)
                        {
                            tempList.Add(res[j].applicable_value.ToString());
                        }
                    }
                    dt.Rows.Add(tempList.ToArray<object>());
                    tempList.Clear();

                }
                List<object> column = new List<object>();
                for (int index = 0; index < dt.Columns.Count; index++)
                {
                    column.Add(new
                    {
                        title = dt.Columns[index].ToString().ToUpper().Replace("_", " "),
                        data = dt.Columns[index].ToString()
                    });
                }

                var d2 = new { list = dt, column };

                return Ok(dt);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost("GetPreviousSalaryDetails")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpSalary))]
        public IActionResult GetPreviousSalaryDetails([FromBody] GetSalaryCalculate value)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(value.emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }
            try
            {

                DateTime _effectivemonthyear = Convert.ToDateTime(value.monthyear);

                int salary_component_id = _context.tbl_component_master.Where(a => a.is_active == 1 && a.component_name == "@GrossSalary_sys" && a.is_system_key == 1).Select(a => a.component_id).FirstOrDefault();


                List<tbl_emp_salary_master> tesmPrevious; // old confirmed salary
                List<tbl_emp_salary_master> tesmRevised;  //Current requested not approved salary list 

                tesmRevised = _context.tbl_emp_salary_master.Where(a => a.emp_id == value.emp_id && a.is_active == 2 && a.applicable_from_dt == _effectivemonthyear).OrderByDescending(p => p.applicable_from_dt).Take(200).ToList();

                tesmPrevious = _context.tbl_emp_salary_master.Where(a => a.emp_id == value.emp_id && a.is_active == 1 && a.applicable_from_dt < _effectivemonthyear).OrderByDescending(p => p.applicable_from_dt).Take(200).ToList();

                if (tesmPrevious.Count() > 0)
                {
                    DateTime lastApplicabledt = tesmPrevious.Select(p => p.applicable_from_dt.Date).Max();
                    // fetching latest old confirmed salary
                    List<tbl_emp_salary_master> tesmCurrent = tesmPrevious.Where(p => p.applicable_from_dt.Date == lastApplicabledt.Date).ToList();


                    List<tbl_component_master> tcm = _context.tbl_component_master.Where(a => a.is_salary_comp == 1 && a.is_active == 1).ToList();
                    if (tesmCurrent.Count() > 0)
                    {
                        var result = (from a in tcm
                                      join b in tesmCurrent on a.component_id equals b.component_id into leftjoin1
                                      join c in tesmRevised on a.component_id equals c.component_id into leftjoin2
                                      from s in leftjoin1.DefaultIfEmpty()
                                      from t in leftjoin2.DefaultIfEmpty()
                                      where a.is_active == 1 && a.is_salary_comp == 1
                                      select new
                                      {
                                          component_id = a.component_id,
                                          property_details = a.property_details,
                                          applicable_value = s != null ? (s.applicable_value == 0 ? "0" : s.applicable_value.ToString()) : "0",
                                          requested_value = t != null ? (t.applicable_value == 0 ? "0" : t.applicable_value.ToString()) : "0",
                                          salary_group = a.is_salary_comp,
                                          a.is_active,
                                          emp_id = 0
                                      }).ToList();


                        return Ok(result);
                    }
                    else
                    {

                        var result = (from a in tcm
                                      where a.is_salary_comp == 1 && a.is_active == 1
                                      select new
                                      {
                                          component_id = a.component_id,
                                          property_details = a.property_details,
                                          applicable_value = 0,
                                          salary_group = a.is_salary_comp,
                                          is_active = a.is_active,
                                          emp_id = 0,

                                      }).ToList();
                        return Ok(result);
                    }



                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "No salary details available or S omething went wrong!!...";
                    return Ok(objresponse);
                }



            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        [HttpPost("Salaryrevision_Calculation")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpSalary))]
        public IActionResult Salaryrevision_Calculation([FromBody] GetSalaryCalculate value)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(value.emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }
            try
            {

                //get the All Context
                //value.componentvalue
                //_context.tbl_component_master.Where(p => p.is_salary_comp==1).ToList()
                //DateTime dt = DateTime.Now;
                //int month_year = 0;
                //var sal_inputmonthyear = _context.tbl_salary_input.Select(a => a.monthyear).DefaultIfEmpty(0).Max();
                //var monthyear = Convert.ToDateTime(value.monthyear).ToString("yyyyMM");
                //if (Convert.ToInt32(monthyear) <= Convert.ToInt32(sal_inputmonthyear))
                //    month_year = Convert.ToInt32(sal_inputmonthyear) + 1;
                //else
                //    month_year = Convert.ToInt32(monthyear);

                DateTime _effectivemonthyear = Convert.ToDateTime(value.monthyear.Substring(0, 4) + "-" + value.monthyear.Substring(4, 2) + "-01");

                int salary_component_id = _context.tbl_component_master.Where(a => a.is_active == 1 && a.component_name == "@GrossSalary_sys"/*"@Salary_sys"*/ && a.is_system_key == 1).Select(a => a.component_id).FirstOrDefault();

                clsComponentValues cc = new clsComponentValues(Convert.ToInt32(value.company_id), _context, Convert.ToInt32(value.emp_id), Convert.ToInt32(value.monthyear), Convert.ToInt32(salary_component_id), Convert.ToInt32(value.sgid), Convert.ToString(value.componentvalue), _config);
                List<mdlSalaryInputValues> salaryvalues = cc.CalculateComponentValues(false).ToList();
                //List<tbl_emp_salary_master> tesms = _context.tbl_emp_salary_master.Where(a => a.emp_id == value.emp_id && a.is_active == 1).ToList();
                if (salaryvalues.Count > 0)
                {
                    //List<tbl_emp_salary_master> tesms = _context.tbl_emp_salary_master.Where(a => a.emp_id == value.emp_id && a.is_active == 1 && a.applicable_from_dt <= _effectivemonthyear).OrderByDescending(p => p.applicable_from_dt).Take(200).ToList();

                    List<tbl_emp_salary_master> tesms;

                    tesms = _context.tbl_emp_salary_master.Where(a => a.emp_id == value.emp_id && a.is_active == 1 && a.applicable_from_dt == _effectivemonthyear).OrderByDescending(p => p.applicable_from_dt).Take(200).ToList();
                    if (tesms.Count == 0)
                    {
                        tesms = _context.tbl_emp_salary_master.Where(a => a.emp_id == value.emp_id && a.is_active == 1 && a.applicable_from_dt < _effectivemonthyear).OrderByDescending(p => p.applicable_from_dt).Take(200).ToList();
                    }


                    List<tbl_component_master> tcm = _context.tbl_component_master.Where(a => a.is_salary_comp == 1 && a.is_active == 1).ToList();
                    if (tesms.Count() > 0)
                    {
                        //DateTime lastApplicabledt = tesms.OrderByDescending(a => a.sno).Select(p => p.applicable_from_dt).FirstOrDefault();
                        //List<tbl_emp_salary_master> tesm = tesms.Where(p => p.applicable_from_dt == lastApplicabledt).ToList();

                        var result = (from a in tcm
                                      join b in tesms on a.component_id equals b.component_id into leftjoin1
                                      from s in leftjoin1.DefaultIfEmpty()
                                      join c in salaryvalues on a.component_id equals c.compId into leftjoin
                                      from s1 in leftjoin.DefaultIfEmpty()
                                      where a.is_active == 1 && a.is_salary_comp == 1
                                      select new
                                      {
                                          component_id = a.component_id,
                                          property_details = a.property_details,
                                          applicable_value = s != null ? (s.applicable_value == 0 ? 0 : s.applicable_value) : 0,
                                          salary_group = a.is_salary_comp,
                                          a.is_active,
                                          compValue = s1.compValue == null ? "0" : s1.compValue,
                                          emp_id = 0
                                      }).ToList();




                        //var result = (from a in tcm
                        //              join b in tesms on a.component_id equals b.component_id
                        //              join c in salaryvalues on a.component_id equals c.compId into leftjoin
                        //              from s1 in leftjoin.DefaultIfEmpty()
                        //              where a.is_active == 1 && a.is_salary_comp == 1
                        //              select new
                        //              {
                        //                  component_id = a.component_id,
                        //                  property_details = a.property_details,
                        //                  applicable_value = b.applicable_value == 0 ? 0 : b.applicable_value,
                        //                  salary_group = a.is_salary_comp,
                        //                  a.is_active,
                        //                  compValue = s1.compValue == null ? "0" : s1.compValue,
                        //                  emp_id = 0
                        //              }).ToList();


                        //var result = (from a in tcm
                        //              join S in salaryvalues on a.component_id equals S.compId into Group
                        //              from s1 in Group.DefaultIfEmpty()
                        //              join t in tesm on a.component_id equals t.component_id into Group1
                        //              from t1 in Group1.DefaultIfEmpty()
                        //              where a.is_salary_comp == 1 && a.is_active == 1
                        //              select new
                        //              {
                        //                  component_id = a.component_id,
                        //                  property_details = a.property_details,
                        //                  applicable_value = t1 == null ? 0 : t1.applicable_value,
                        //                  s1.compValue,
                        //                  salary_group = a.is_salary_comp,
                        //                  is_active = a.is_active,
                        //                  emp_id = 0,

                        //              }).ToList();




                        return Ok(result);
                    }
                    else
                    {

                        var result = (from a in tcm
                                      join S in salaryvalues
                                      on a.component_id equals S.compId into Group
                                      from s in Group
                                      where a.is_salary_comp == 1 && a.is_active == 1
                                      select new
                                      {
                                          component_id = a.component_id,
                                          property_details = a.property_details,
                                          applicable_value = 0,
                                          s.compValue,
                                          salary_group = a.is_salary_comp,
                                          is_active = a.is_active,
                                          emp_id = 0,

                                      }).ToList();
                        return Ok(result);
                    }


                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "No Salary Calculation Formula available or Something went wrong!!...";
                    return Ok(objresponse);
                }




            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }



        [Route("Save_SalaryRevision")]
        [HttpPost]
        public IActionResult Save_SalaryRevision([FromBody] SalaryRevison objsal)
        {
            throw new NotImplementedException();
#if false
            Response_Msg objResult = new Response_Msg();
            try
            {
                //int month_year = 0;
                //var sal_inputmonthyear = _context.tbl_salary_input.Select(a => a.monthyear).DefaultIfEmpty(0).Max();
                //var monthyear = Convert.ToDateTime(objsal.applicable_date).ToString("yyyyMM");
                //if (Convert.ToInt32(monthyear) <= Convert.ToInt32(sal_inputmonthyear))
                //    month_year = Convert.ToInt32(sal_inputmonthyear) + 1;
                //else
                //    month_year = Convert.ToInt32(monthyear);

                //string year = month_year.ToString().Substring(0, 4);
                //string month = month_year.ToString().Substring(4, 2);
                //string date_ = month + "-" + "01" + "-" + year;


                //start Check month salary of effective from date is freezed or not.
                int CurrentMonthYear = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));

                if (Convert.ToInt32(objsal.applicable_date.ToString("yyyyMM")) <= CurrentMonthYear)
                {
                    var _salarycalculate = _context.tbl_payroll_process_status.Where(x => x.payroll_month_year == Convert.ToInt32(objsal.applicable_date.ToString("yyyyMM")) && x.emp_id == objsal.emp_id && x.is_deleted == 0 && (x.is_calculated == 1 || x.is_freezed == 1)).FirstOrDefault();
                    if (_salarycalculate != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Selected Month Salary is already calculated or freezed!...";
                        return Ok(objResult);
                    }
                }


                //end

                DateTime applicable_date = Convert.ToDateTime(objsal.applicable_date.ToString("01-MMM-yyyy"));

                var exist = _context.tbl_emp_salary_master.Where(x => x.emp_id == objsal.emp_id && x.applicable_from_dt == applicable_date).ToList();

                if (exist != null && exist.Count > 0)
                {
                    if (exist.Any(x => x.is_active == 1))
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Salary is already revised for the month " + applicable_date.ToString("MMM-yyyy");
                        return Ok(objResult);
                    }
                    else
                    {
                        DateTime currentdt = DateTime.Now;
                        List<tbl_emp_salary_master> sobj = objsal.SalaryValue.Select(a => new tbl_emp_salary_master
                        {
                            emp_id = objsal.emp_id,
                            component_id = a.component_id,
                            applicable_value = Convert.ToDecimal(a.componentvalue),
                            salaryrevision = Convert.ToDecimal(objsal.salaryrevision),
                            applicable_from_dt = objsal.applicable_date,
                            is_active = objsal.is_active,
                            created_dt = currentdt,
                            created_by = objsal.created_by,
                            maker_remark = objsal.maker_remark
                        }).ToList();

                        foreach (tbl_emp_salary_master item in exist)
                        {
                            item.applicable_value = sobj.AsEnumerable().Where(x => x.component_id == item.component_id).FirstOrDefault().applicable_value;
                            item.salaryrevision = sobj.AsEnumerable().Where(x => x.component_id == item.component_id).FirstOrDefault().salaryrevision;
                            item.is_active = sobj.AsEnumerable().Where(x => x.component_id == item.component_id).FirstOrDefault().is_active;
                            item.modified_by = sobj.AsEnumerable().Where(x => x.component_id == item.component_id).FirstOrDefault().created_by;
                            item.maker_remark = sobj.AsEnumerable().Where(x => x.component_id == item.component_id).FirstOrDefault().maker_remark;
                            item.modified_dt = currentdt;
                        }

                        _context.UpdateRange(exist);

                        _context.SaveChanges();
                        objResult.StatusCode = 0;
                        objResult.Message = "Your request is updated successfully and sent to checker for approval!!";
                        return Ok(objResult);
                    }
                }
                else
                {
                    DateTime currentdt = DateTime.Now;

                    _context.AddRange(
                    objsal.SalaryValue.Select(a => new tbl_emp_salary_master
                    {
                        emp_id = objsal.emp_id,
                        component_id = a.component_id,
                        applicable_value = Convert.ToDecimal(a.componentvalue),
                        salaryrevision = Convert.ToDecimal(objsal.salaryrevision),
                        applicable_from_dt = objsal.applicable_date,
                        is_active = objsal.is_active,
                        created_dt = currentdt,
                        created_by = objsal.created_by,
                        maker_remark = objsal.maker_remark
                    }));
                    _context.SaveChanges();
                    objResult.StatusCode = 0;
                    objResult.Message = "Your request is successfully sent to checker for approval!!";
                    return Ok(objResult);
                }

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }

#endif
        }


        [Route("Get_EmpLoanData/{emp_id}/{loan_req_id}/{companyid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LoanApproval))]
        public IActionResult Get_EmpLoanData([FromRoute] int emp_id, int loan_req_id, int companyid)
        {
            throw new NotImplementedException();
#if false
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(companyid))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access..!!";
                return Ok(objresponse);
            }
            try
            {



                var emp_loan_data = _context.tbl_loan_request.Where(x => x.req_emp_id == emp_id && x.loan_req_id == loan_req_id && x.is_final_approval != 1 && x.is_final_approval != 2
                                    && x.is_closed == 0 && x.is_deleted == 0).Select(p => new
                                    {
                                        loan_req_id = p.loan_req_id,
                                        loan_type = p.loan_type == 1 ? "Loan" : "Advance",
                                        loan_amt = p.loan_amt,
                                        loan_tenure = p.loan_tenure,
                                        roi = p.interest_rate,
                                        emp_id = p.req_emp_id,
                                        loan_purpose = p.loan_purpose,
                                        emp_name = p.emp_code,
                                        dept_name = p.emp_master.tbl_emp_officaial_sec.OrderByDescending(z => z.emp_official_section_id).Take(1).FirstOrDefault(z => z.is_deleted == 0).tbl_department_master.department_name,
                                        //grade_id = p.emp_master.tbl_emp_grade_allocation.OrderByDescending(z => z.emp_grade_id).Take(1).FirstOrDefault(z => z.employee_id == emp_id).grade_id,
                                        grade_id = p.emp_master.tbl_emp_grade_allocation.GroupBy(h => h.employee_id).Select(h => h.OrderByDescending(q => q.emp_grade_id).FirstOrDefault()).FirstOrDefault(q => q.applicable_from_date.Date <= p.created_dt.Date && p.created_dt.Date <= q.applicable_to_date.Date).grade_id,
                                        grade_name = p.emp_master.tbl_emp_grade_allocation.GroupBy(h => h.employee_id).Select(h => h.OrderByDescending(q => q.emp_grade_id).FirstOrDefault()).FirstOrDefault(q => q.applicable_from_date.Date <= p.created_dt.Date && p.created_dt.Date <= q.applicable_to_date.Date).tbl_grade_master.grade_name,
                                        //grade_name = p.emp_master.tbl_emp_grade_allocation.OrderByDescending(z => z.emp_grade_id).Take(1).FirstOrDefault(z => z.tbl_grade_master.is_active == 1).tbl_grade_master.grade_name,
                                        //des_name = p.emp_master.tbl_emp_desi_allocation.OrderByDescending(z => z.emp_grade_id).Take(1).FirstOrDefault(z => z.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name,
                                        des_name = p.emp_master.tbl_emp_desi_allocation.GroupBy(h => h.employee_id).Select(h => h.OrderByDescending(g => g.emp_grade_id).FirstOrDefault()).FirstOrDefault(z => z.applicable_from_date.Date <= p.created_dt.Date && p.created_dt.Date <= z.applicable_to_date.Date).tbl_designation_master.designation_name,

                                    }).ToList();

                string year = DateTime.Now.Date.Year.ToString();
                string month = DateTime.Now.Date.Month.ToString();
                DateTime _ApplicableDate = Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1).AddDays(-1);

                decimal monthly_salary = 0;
                //var monthly_salary_data = _context.tbl_emp_salary_master.OrderByDescending(y => y.sno).Take(1).Where(y => y.is_active == 1 && y.emp_id == emp_id).FirstOrDefault();
                var monthly_salary_data = _context.tbl_emp_salary_master.Where(p => p.is_active == 1 && p.emp_id == emp_id && p.applicable_from_dt.Date <= _ApplicableDate).FirstOrDefault();
                if (monthly_salary_data != null)
                {
                    monthly_salary = monthly_salary_data.salaryrevision;
                }


                var result = new { emp_loan_data = emp_loan_data, monthly_salary = monthly_salary };







                //var result = (from L in _context.tbl_loan_request
                //              join e in _context.tbl_emp_officaial_sec
                //              on L.req_emp_id equals e.employee_id
                //              join a in _context.tbl_emp_grade_allocation on e.employee_id equals a.employee_id orderby a.emp_grade_id descending 
                //              join d in _context.tbl_grade_master on a.grade_id equals d.grade_id
                //              join dept in _context.tbl_department_master on e.department_id equals dept.department_id
                //              join des in _context.tbl_emp_desi_allocation on e.employee_id equals des.employee_id
                //              join x in _context.tbl_designation_master on des.desig_id equals x.designation_id
                //              where e.employee_id == emp_id && e.is_deleted == 0 && L.is_closed == 0 && L.is_final_approval == 0 && L.loan_req_id==loan_req_id 

                //              select new
                //              {

                //                  loan_req_id = L.loan_req_id,
                //                  loan_type = L.loan_type == 1 ? "Loan" : "Advance",
                //                  loan_amt = L.loan_amt,
                //                  loan_tenure = L.loan_tenure,
                //                  roi = L.interest_rate,
                //                  emp_id = e.employee_id,
                //                  dept_name = dept.department_name,
                //                  emp_name = e.employee_first_name,
                //                  grade_id = d.grade_id,
                //                  grade_name = d.grade_name,
                //                  des_name = x.designation_name,
                //                  loan_purpose = L.loan_purpose

                //              }).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
#endif
        }


        [Route("LoanRequestApproval")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LoanApproval))]
        public IActionResult LoanRequestApproval([FromBody] LoanRequest objloan)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(objloan.company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);

            }


            if (objloan.is_approve != 1 && objloan.is_approve != 2)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Invalid Action";
                return Ok(objresponse);
            }
            try
            {


                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        bool final_approver_exist = false;

                        // int company_idd = _context.tbl_employee_company_map.Where(x => x.employee_id == objloan.last_modified_by && x.is_deleted == 0).Select(p => p.company_id).FirstOrDefault()??0;
                        var check_approver = _context.tbl_loan_approval_setting.Where(x => x.company_id == objloan.company_id && x.approver_type == 1 && x.is_active == 1).ToList();
                        if (check_approver.Count > 0)
                        {
                            final_approver_exist = check_approver.Any(x => x.is_final_approver == 1);
                            if (final_approver_exist)
                            {
                                // check loan request
                                var loan_req_exist = _context.tbl_loan_request.Where(x => x.loan_req_id == objloan.loan_req_id && x.req_emp_id == objloan.emp_id && x.is_deleted == 0 && x.is_closed == 0).FirstOrDefault();
                                if (loan_req_exist != null)
                                {
                                    // check approver 
                                    var get_approver = check_approver.Where(x => x.emp_id == objloan.last_modified_by || x.approver_role_id == objloan.approver_role_id).FirstOrDefault();
                                    if (get_approver != null)
                                    {
                                        var loan_approval = _context.tbl_loan_approval.Where(x => x.loan_req_id == loan_req_exist.loan_req_id && x.emp_id == objloan.emp_id && get_approver.emp_id == _clsCurrentUser.EmpId && x.approval_order == get_approver.order && x.approver_role_id == Convert.ToInt32(get_approver.approver_role_id) && x.is_deleted == 0).FirstOrDefault();
                                        if (loan_approval != null)
                                        {
                                            loan_approval.is_approve = objloan.is_approve;

                                            // bool _its_final_approver = check_approver.Any(x => (x.emp_id == objloan.last_modified_by || x.approver_role_id == objloan.approver_role_id) && x.is_final_approver == 1);


                                            if (objloan.is_final_approver == 2) // if application reject
                                            {
                                                List<tbl_loan_approval> exist = _context.tbl_loan_approval.Where(x => x.is_deleted == 0 && x.loan_req_id == loan_req_exist.loan_req_id && x.emp_id == objloan.emp_id && x.approval_order == get_approver.order).ToList();
                                                // List<tbl_loan_approval> exist = _context.tbl_loan_approval.Where(x => x.is_deleted == 0 && x.loan_req_id == loan_req_exist.loan_req_id).ToList();
                                                exist.ForEach(p => { p.is_final_approver = 2; p.last_modified_by = objloan.last_modified_by; p.last_modified_date = DateTime.Now; });

                                                _context.tbl_loan_approval.UpdateRange(exist);


                                                loan_req_exist.is_final_approval = 2;
                                                loan_req_exist.is_closed = 1;
                                                loan_req_exist.last_modified_by = _clsCurrentUser.EmpId;
                                                loan_req_exist.last_modified_date = DateTime.Now;

                                                _context.Entry(loan_req_exist).State = EntityState.Modified;
                                            }
                                            else
                                            {
                                                if (get_approver.is_final_approver == 1)
                                                {
                                                    loan_approval.is_final_approver = objloan.is_final_approver;

                                                    loan_req_exist.is_final_approval = objloan.is_final_approver;
                                                    loan_req_exist.last_modified_by = _clsCurrentUser.EmpId;
                                                    loan_req_exist.last_modified_date = DateTime.Now;
                                                }
                                                else
                                                {
                                                    loan_approval.is_final_approver = 3; // In Process
                                                    loan_req_exist.is_final_approval = 3; // In Process

                                                }

                                                loan_req_exist.last_modified_by = _clsCurrentUser.EmpId;
                                                loan_req_exist.last_modified_date = DateTime.Now;
                                                loan_approval.last_modified_by = _clsCurrentUser.EmpId;
                                                loan_approval.last_modified_date = DateTime.Now;
                                            }


                                            _context.tbl_loan_approval.Update(loan_approval);
                                            _context.tbl_loan_request.Update(loan_req_exist);

                                            _context.SaveChanges();

                                            trans.Commit();



                                            objresponse.StatusCode = 0;
                                            objresponse.Message = "Loan successfully " + (objloan.is_approve == 1 ? "Approved" : objloan.is_approve == 2 ? "Rejected" : "") + "";
                                        }
                                        else
                                        {
                                            objresponse.StatusCode = 1;
                                            objresponse.Message = "Sorry loan request details not available";
                                        }

                                    }
                                    else
                                    {
                                        objresponse.StatusCode = 1;
                                        objresponse.Message = "Sorry you are not an approver of loan";
                                    }

                                    // int order_id = check_approver.Where(x => x.emp_id == objloan.last_modified_by || x.approver_role_id == objloan.approver_role_id).Select(p => p.order).FirstOrDefault();


                                }
                                else
                                {
                                    objresponse.StatusCode = 1;
                                    objresponse.Message = "Loan Request Not Exist";
                                }


                            }
                            else
                            {
                                objresponse.StatusCode = 1;
                                objresponse.Message = "Please create final approver of loan";
                            }
                        }
                        else
                        {
                            objresponse.StatusCode = 1;
                            objresponse.Message = "Please create loan approvers";
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
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
            }

            return Ok(objresponse);
        }

        //public IActionResult LoanRequestApproval([FromBody] tbl_loan_approval objloan)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    Classes.clsLoginEmpDtl ob = new clsLoginEmpDtl(_context, Convert.ToInt32(objloan.emp_id), "read", _AC);
        //    if (!ob.is_valid())
        //    {
        //        objResult.StatusCode = 1;
        //        objResult.Message = "Unauthorize Access...!";
        //        return Ok(objResult);
        //    }


        //    try
        //    {
        //        string approver_idd = ""; //contain final approver
        //        int orderr = 0;
        //        var get_approvers = _context.tbl_loan_approval_setting.Where(x => x.approver_type == 1 && (x.emp_id == objloan.last_modified_by || x.approver_role_id == objloan.approver_role_id)).OrderByDescending(q => q.order).Select(p => new { p.order, p.emp_id, p.is_final_approver }).ToList();
        //        if (get_approvers.Count > 1)
        //        {

        //            for (int i = 0; i < get_approvers.Count; i++)
        //            {
        //                var data = _context.tbl_loan_approval.Where(x => x.loan_req_id == objloan.loan_req_id && x.emp_id == objloan.emp_id && x.approval_order == get_approvers[i].order).OrderByDescending(x => x.sno).Select(a => new { a.sno, a.loan_req_id, a.emp_id, a.is_final_approver, a.is_approve, a.is_deleted, a.last_modified_by, a.last_modified_date, a.approval_order }).FirstOrDefault();
        //                if (data != null)
        //                {
        //                    //var exist = _context.tbl_loan_approval.Where(x => x.loan_req_id == objloan.loan_req_id && x.emp_id == objloan.emp_id && x.approval_order == approver_idd.order).OrderByDescending(y => y.sno).FirstOrDefault();

        //                    var exist = _context.tbl_loan_approval.Where(x => x.loan_req_id == objloan.loan_req_id && x.emp_id == objloan.emp_id && x.approval_order == get_approvers[i].order).OrderByDescending(y => y.sno).FirstOrDefault();

        //                    if (exist != null)
        //                    {
        //                        exist.sno = exist.sno;
        //                        exist.loan_req_id = objloan.loan_req_id;
        //                        exist.emp_id = objloan.emp_id;
        //                        exist.is_approve = objloan.is_approve;
        //                        if (get_approvers[i].is_final_approver > 0 && objloan.is_approve == 1)
        //                        {
        //                            exist.is_final_approver = objloan.is_approve;
        //                        }
        //                        else if (objloan.is_approve == 2)
        //                        {
        //                            exist.is_final_approver = objloan.is_approve;
        //                        }
        //                        else
        //                        {
        //                            exist.is_final_approver = 0;
        //                        }

        //                        exist.is_deleted = 0;
        //                        exist.last_modified_by = 1;
        //                        exist.last_modified_date = DateTime.Now;

        //                        _context.tbl_loan_approval.Attach(exist);
        //                        _context.Entry(exist).State = EntityState.Modified;


        //                        var exist1 = _context.tbl_loan_request.Where(y => y.loan_req_id == objloan.loan_req_id && y.req_emp_id == objloan.emp_id).FirstOrDefault();
        //                        if (objloan.is_approve == 2)
        //                        {
        //                            exist1.loan_req_id = Convert.ToInt32(exist1.loan_req_id);
        //                            exist1.req_emp_id = exist1.req_emp_id;
        //                            exist1.is_final_approval = objloan.is_approve;
        //                            exist1.last_modified_by = 1;
        //                            exist1.last_modified_date = DateTime.Now;

        //                            _context.tbl_loan_request.Attach(exist1);
        //                            _context.Entry(exist1).State = EntityState.Modified;

        //                        }

        //                        if (get_approvers[i].is_final_approver > 0)
        //                        {
        //                            exist1.loan_req_id = Convert.ToInt32(exist1.loan_req_id);
        //                            exist1.req_emp_id = exist1.req_emp_id;
        //                            //exist1.is_final_approval = 1;
        //                            exist1.is_final_approval = objloan.is_approve;
        //                            exist1.last_modified_by = 1;
        //                            exist1.last_modified_date = DateTime.Now;

        //                            _context.tbl_loan_request.Attach(exist1);
        //                            _context.Entry(exist1).State = EntityState.Modified;
        //                        }
        //                    }

        //                    _context.SaveChanges();

        //                }





        //            }


        //            objResult.StatusCode = 0;
        //            objResult.Message = "Loan Request Update successfully !!";
        //        }
        //        else
        //        {
        //            var get_approver = _context.tbl_loan_approval_setting.Where(x => x.approver_type == 1 && (x.emp_id == objloan.last_modified_by || x.approver_role_id == objloan.approver_role_id)).OrderByDescending(q => q.order).Select(p => new { p.order, p.emp_id, p.is_final_approver }).FirstOrDefault();

        //            approver_idd = get_approver.is_final_approver.ToString();
        //            orderr = get_approver.order;
        //        }

        //        //var approver_idd = _context.tbl_loan_approval_setting.Where(x => x.emp_id == objloan.last_modified_by).OrderByDescending(q=>q.order).Select(p => new { p.order, p.emp_id,p.is_final_approver }).FirstOrDefault();

        //        //if (approver_idd != null)
        //        if (!string.IsNullOrEmpty(approver_idd))
        //        {
        //            //var data = _context.tbl_loan_approval.Where(x => x.loan_req_id == objloan.loan_req_id && x.emp_id == objloan.emp_id && x.approval_order == approver_idd.order).OrderByDescending(x=>x.sno).Select(a => new { a.sno, a.loan_req_id, a.emp_id, a.is_final_approver, a.is_approve, a.is_deleted, a.last_modified_by, a.last_modified_date, a.approval_order }).FirstOrDefault();

        //            var data = _context.tbl_loan_approval.Where(x => x.loan_req_id == objloan.loan_req_id && x.emp_id == objloan.emp_id && x.approval_order == orderr).OrderByDescending(x => x.sno).Select(a => new { a.sno, a.loan_req_id, a.emp_id, a.is_final_approver, a.is_approve, a.is_deleted, a.last_modified_by, a.last_modified_date, a.approval_order }).FirstOrDefault();
        //            if (data != null)
        //            {
        //                //var exist = _context.tbl_loan_approval.Where(x => x.loan_req_id == objloan.loan_req_id && x.emp_id == objloan.emp_id && x.approval_order == approver_idd.order).OrderByDescending(y => y.sno).FirstOrDefault();

        //                var exist = _context.tbl_loan_approval.Where(x => x.loan_req_id == objloan.loan_req_id && x.emp_id == objloan.emp_id && x.approval_order == orderr).OrderByDescending(y => y.sno).FirstOrDefault();

        //                if (exist != null)
        //                {
        //                    exist.sno = exist.sno;
        //                    exist.loan_req_id = objloan.loan_req_id;
        //                    exist.emp_id = objloan.emp_id;
        //                    exist.is_approve = objloan.is_approve;
        //                    if (Convert.ToInt32(approver_idd) > 0 && objloan.is_approve == 1)
        //                    {
        //                        exist.is_final_approver = objloan.is_approve;
        //                    }
        //                    else if (objloan.is_approve == 2)
        //                    {
        //                        exist.is_final_approver = objloan.is_approve;
        //                    }
        //                    else
        //                    {
        //                        exist.is_final_approver = 0;
        //                    }

        //                    exist.is_deleted = 0;
        //                    exist.last_modified_by = 1;
        //                    exist.last_modified_date = DateTime.Now;

        //                    _context.tbl_loan_approval.Attach(exist);
        //                    _context.Entry(exist).State = EntityState.Modified;


        //                    var exist1 = _context.tbl_loan_request.Where(y => y.loan_req_id == objloan.loan_req_id && y.req_emp_id == objloan.emp_id).FirstOrDefault();
        //                    if (objloan.is_approve == 2)
        //                    {
        //                        exist1.loan_req_id = Convert.ToInt32(exist1.loan_req_id);
        //                        exist1.req_emp_id = exist1.req_emp_id;
        //                        exist1.is_final_approval = objloan.is_approve;
        //                        exist1.last_modified_by = 1;
        //                        exist1.last_modified_date = DateTime.Now;

        //                        _context.tbl_loan_request.Attach(exist1);
        //                        _context.Entry(exist1).State = EntityState.Modified;

        //                    }

        //                    if (Convert.ToInt32(approver_idd) > 0)
        //                    {
        //                        exist1.loan_req_id = Convert.ToInt32(exist1.loan_req_id);
        //                        exist1.req_emp_id = exist1.req_emp_id;
        //                        //exist1.is_final_approval = 1;
        //                        exist1.is_final_approval = objloan.is_approve;
        //                        exist1.last_modified_by = 1;
        //                        exist1.last_modified_date = DateTime.Now;

        //                        _context.tbl_loan_request.Attach(exist1);
        //                        _context.Entry(exist1).State = EntityState.Modified;
        //                    }
        //                }

        //                _context.SaveChanges();
        //                objResult.StatusCode = 0;
        //                objResult.Message = "Loan Request Update successfully !!";


        //            }
        //        }

        //        else if (objResult.StatusCode != 0 && objResult.Message != "")
        //        {
        //            objResult.StatusCode = 1;
        //            objResult.Message = "Sorry You are not an approver of this Loan";
        //        }
        //        return Ok(objResult);



        //    }
        //    catch (Exception ex)
        //    {
        //        objResult.StatusCode = 0;
        //        objResult.Message = ex.Message;
        //        return Ok(objResult);
        //    }
        //}

        [Route("Save_OTRateMaster")]
        [HttpPost]
        // Here Save OT rate details
        [Authorize(Policy = nameof(enmMenuMaster.OTRate))]
        public IActionResult Save_OTRateMaster([FromBody] tbl_ot_rate_details objOT)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(objOT.companyid))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                Ok(objResult);
            }

            if (!_clsCurrentUser.DownlineEmpId.Contains(objOT.created_by))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!!";
                Ok(objResult);
            }

            try
            {

                var exist = _context.tbl_ot_rate_details.Where(x => x.companyid == objOT.companyid && x.emp_id == objOT.emp_id && x.grade_id == objOT.grade_id && x.is_active == 1).FirstOrDefault();

                if (exist != null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Over Time rate already exist !!";
                    return Ok(objResult);
                }
                else
                {

                    objOT.companyid = objOT.companyid;
                    if (objOT.emp_id == 0)
                    {
                        objOT.emp_id = null;
                    }
                    else
                    {
                        objOT.emp_id = objOT.emp_id;
                    }
                    if (objOT.emp_id != 0 && objOT.emp_id != null && objOT.grade_id != 0)
                    {
                        objOT.grade_id = null;
                    }
                    else if (objOT.grade_id != 0)
                    {
                        objOT.grade_id = objOT.grade_id;
                    }
                    else
                    {
                        objOT.grade_id = null;
                    }

                    //if (objOT.emp_id != 0)
                    //{
                    //    if (!string.IsNullOrEmpty(objOT.grade_id.ToString()))
                    //    {
                    //        objOT.grade_id = objOT.grade_id;
                    //    }
                    //    else
                    //    {
                    //        objOT.grade_id = null;
                    //    }
                    //}
                    //else
                    //{
                    //    objOT.grade_id = objOT.grade_id;
                    //}
                    objOT.ot_amt = objOT.ot_amt;
                    objOT.is_active = objOT.is_active;
                    objOT.created_by = objOT.created_by;
                    objOT.created_dt = DateTime.Now;
                    _context.Entry(objOT).State = EntityState.Added;
                    _context.SaveChanges();
                    objResult.StatusCode = 0;
                    objResult.Message = "Over Time save successfully !!";
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

        //Get over Time details
        [Route("Get_OTRateMaster/{ot_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.OTRate))]
        public IActionResult Get_OTRateMaster([FromRoute] int ot_id)
        {
            try
            {

                if (ot_id > 0)
                {
                    var result = (from a in _context.tbl_ot_rate_details
                                  join b in _context.tbl_employee_master
                                       on a.emp_id equals b.employee_id into Group
                                  from m in Group.DefaultIfEmpty()
                                  join c in _context.tbl_company_master
                                       on a.companyid equals c.company_id
                                  join d in _context.tbl_grade_master
                                       on a.grade_id equals d.grade_id into Group1
                                  from m1 in Group1.DefaultIfEmpty()
                                  where a.is_active == 1 && _clsCurrentUser.CompanyId.Contains(a.companyid)
                                  select new
                                  {
                                      a.ot_id,
                                      a.ot_amt,
                                      a.emp_id,
                                      emp_code = a.emp_id == null || m.emp_code == null ? "All" : m.emp_code,
                                      c.company_id,
                                      c.company_name,
                                      a.grade_id,
                                      grade_name = a.grade_id == null || m1.grade_name == null ? "NA" : m1.grade_name,
                                      a.created_dt,
                                      a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.employee_id == a.emp_id && q.is_deleted == 0).employee_first_name,
                                      a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.employee_id == a.emp_id && q.is_deleted == 0).employee_middle_name,
                                      a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.employee_id == a.emp_id && q.is_deleted == 0).employee_last_name
                                  }).Where(c => c.ot_id == ot_id).AsEnumerable().Select((e, index) => new
                                  {
                                      employee_name_code = e.employee_first_name + " " + e.employee_middle_name + " " + e.employee_last_name + "(" + e.emp_code + ")",
                                      ot_amt = e.ot_amt,
                                      ot_id = e.ot_id,
                                      grade_name = e.grade_name,
                                      created_dt = e.created_dt,
                                      company_id = e.company_id,
                                      emp_id = e.emp_id,
                                      company_name = e.company_name,
                                      grade_id = e.grade_id

                                  }).FirstOrDefault();
                    return Ok(result);
                }
                else
                {

                    var result = (from a in _context.tbl_ot_rate_details
                                  join b in _context.tbl_employee_master
                                       on a.emp_id equals b.employee_id into Group
                                  from m in Group.DefaultIfEmpty()
                                  join c in _context.tbl_company_master
                                      on a.companyid equals c.company_id
                                  join d in _context.tbl_grade_master
                                      on a.grade_id equals d.grade_id into Group1
                                  from m1 in Group1.DefaultIfEmpty()
                                  where a.is_active == 1 && _clsCurrentUser.CompanyId.Contains(a.companyid)
                                  select new
                                  {
                                      a.ot_id,
                                      a.ot_amt,
                                      a.emp_id,
                                      emp_code = a.emp_id == null || m.emp_code == null ? "All" : m.emp_code,
                                      c.company_id,
                                      c.company_name,
                                      a.grade_id,
                                      grade_name = a.grade_id == null || m1.grade_name == null ? "NA" : m1.grade_name,
                                      a.created_dt,
                                      a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.employee_id == a.emp_id && q.is_deleted == 0).employee_first_name,
                                      a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.employee_id == a.emp_id && q.is_deleted == 0).employee_middle_name,
                                      a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.employee_id == a.emp_id && q.is_deleted == 0).employee_last_name
                                  }).AsEnumerable().Select((e, index) => new
                                  {
                                      employee_name_code = e.employee_first_name + " " + e.employee_middle_name + " " + e.employee_last_name + "(" + e.emp_code + ")",
                                      ot_amt = e.ot_amt,
                                      ot_id = e.ot_id,
                                      grade_name = e.grade_name,
                                      created_dt = e.created_dt,
                                      company_id = e.company_id,
                                      emp_id = e.emp_id,
                                      company_name = e.company_name,
                                      grade_id = e.grade_id
                                  }).ToList();
                    return Ok(result);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [Route("Update_OTRateMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.OTRate))]
        public IActionResult Update_OTRateMaster([FromBody] tbl_ot_rate_details objOT)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(objOT.companyid))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }
            if (!_clsCurrentUser.DownlineEmpId.Contains(objOT.created_by))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }
            try
            {
                var exist = _context.tbl_ot_rate_details.Where(x => x.ot_id == objOT.ot_id && x.companyid == objOT.companyid).FirstOrDefault();



                exist.companyid = objOT.companyid;
                if (objOT.emp_id == 0)
                {
                    exist.emp_id = null;
                }
                else
                {
                    exist.emp_id = objOT.emp_id;
                }
                if (objOT.emp_id != 0)
                {
                    // objOT.grade_id = null;
                    exist.grade_id = null;
                }
                else
                {
                    exist.grade_id = objOT.grade_id;
                }
                exist.ot_amt = objOT.ot_amt;
                //exist.is_active = objOT.is_active;
                exist.last_modified_by = objOT.last_modified_by;
                exist.last_modified_date = DateTime.Now;
                _context.Entry(exist).State = EntityState.Modified;
                _context.SaveChanges();
                objResult.StatusCode = 0;
                objResult.Message = "Over Time Rate details updated successfully !!";
                return Ok(objResult);

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }
        /// <summary>
        /// Here, Get employee details whose OT allowed
        /// </summary>
        /// <param name="company_id"></param>
        /// <returns></returns>
        //[Route("Get_EmployeeDetailsByComp/{company_id}")]
        //[HttpGet]
        ////[Authorize(Policy = "8052")]
        //public IActionResult Get_EmployeeCodeFromEmpMasterByComp(int company_id)
        //{
        //    try
        //    {

        //        var data = _context.tbl_emp_officaial_sec.Where(b => b.is_deleted == 0 && b.tbl_employee_id_details.is_active == 1 && b.is_ot_allowed == 1 && b.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(x => x.is_deleted == 0).company_id == company_id)
        //            .Select(a => new { a.tbl_employee_id_details.emp_code, a.employee_id, a.employee_first_name, a.employee_middle_name, a.employee_last_name })
        //            .Select(j => new
        //            {
        //                j.employee_id,
        //                emp_code = string.Format("{0} {1} {2} ({3})", j.employee_first_name, j.employee_middle_name, j.employee_last_name, j.emp_code)
        //            }).ToList();

        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}

        //Get over Time details
        [Route("Get_CategaryDetails_byEmpId/{emp_id}/{fy_id}")]
        [HttpGet]
        //[Authorize(Policy = "8053")]
        public IActionResult Get_CategaryDetails_byEmpId([FromRoute] int ot_id, string fy_id)
        {
            try
            {

                if (ot_id > 0)
                {
                    var result = (from a in _context.tbl_ot_rate_details
                                  join b in _context.tbl_employee_master
                                       on a.emp_id equals b.employee_id into Group
                                  from m in Group.DefaultIfEmpty()
                                  join c in _context.tbl_company_master
                                       on a.companyid equals c.company_id
                                  join d in _context.tbl_grade_master
                                       on a.grade_id equals d.grade_id into Group1
                                  from m1 in Group1.DefaultIfEmpty()
                                  where a.is_active == 1
                                  select new
                                  {
                                      a.ot_id,
                                      a.ot_amt,
                                      a.emp_id,
                                      emp_code = a.emp_id == null || m.emp_code == null ? "All" : m.emp_code,
                                      c.company_id,
                                      c.company_name,
                                      a.grade_id,
                                      grade_name = a.grade_id == null || m1.grade_name == null ? "NA" : m1.grade_name,
                                      a.created_dt
                                  }).Where(c => c.ot_id == ot_id).FirstOrDefault();
                    return Ok(result);
                }
                else
                {

                    var result = (from a in _context.tbl_ot_rate_details
                                  join b in _context.tbl_employee_master
                                       on a.emp_id equals b.employee_id into Group
                                  from m in Group.DefaultIfEmpty()
                                  join c in _context.tbl_company_master
                                      on a.companyid equals c.company_id
                                  join d in _context.tbl_grade_master
                                      on a.grade_id equals d.grade_id into Group1
                                  from m1 in Group1.DefaultIfEmpty()
                                  where a.is_active == 1
                                  select new
                                  {
                                      a.ot_id,
                                      a.ot_amt,
                                      a.emp_id,
                                      emp_code = a.emp_id == null || m.emp_code == null ? "All" : m.emp_code,
                                      c.company_id,
                                      c.company_name,
                                      a.grade_id,
                                      grade_name = a.grade_id == null || m1.grade_name == null ? "NA" : m1.grade_name,
                                      a.created_dt
                                  }).ToList();
                    return Ok(result);

                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Get_EmployeeDetailsFromEmpMasterByComp/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8054")]
        public IActionResult Get_EmployeeDetailsFromEmpMasterByComp(int company_id)
        {
            try
            {


                var data = _context.tbl_emp_officaial_sec.Join(_context.tbl_loan_request, a => a.employee_id, b => b.req_emp_id, (a, b) => new
                {

                    a.employee_id,
                    a.employee_first_name,
                    a.employee_middle_name,
                    a.employee_last_name,
                    a.is_deleted,
                    a.tbl_employee_id_details,
                    emp_code = string.Format("{0} {1} {2} ({3})", a.employee_first_name, a.employee_middle_name, a.employee_last_name, a.tbl_employee_id_details.emp_code),
                    a.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(p => p.is_deleted == 0).company_id
                }).Where(b => b.is_deleted == 0 && b.tbl_employee_id_details.is_active == 1 && b.company_id == company_id).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Save_LoanApprovalSetting")]
        [HttpPost]
        // Here Save Loan Approval Setting
        [Authorize(Policy = nameof(enmMenuMaster.LoanApprovalSettingMaster))]
        public IActionResult Save_LoanApprovalSetting([FromBody] tbl_loan_approval_setting objloanSetting)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(objloanSetting.company_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }



            try
            {
                //   var exist = _context.tbl_loan_approval_setting.Where(x => x.company_id == objloanSetting.company_id &&( x.emp_id == objloanSetting.emp_id || x.approver_role_id==objloanSetting.approver_role_id) && x.approver_type ==objloanSetting.approver_type ).ToList();
                bool duplicate = false;
                if (objloanSetting.emp_id > 0)
                {
                    var exist = _context.tbl_loan_approval_setting.Where(x => x.company_id == objloanSetting.company_id && x.approver_type == objloanSetting.approver_type && x.emp_id == objloanSetting.emp_id && x.is_active == 1).ToList();
                    if (exist.Count > 0)
                    {
                        duplicate = true;
                    }
                }
                else
                {
                    var existt = _context.tbl_loan_approval_setting.Where(x => x.company_id == objloanSetting.company_id && x.approver_type == objloanSetting.approver_type && x.approver_role_id == objloanSetting.approver_role_id && x.is_active == 1).ToList();
                    if (existt.Count > 0)
                    {
                        duplicate = true;
                    }
                }

                if (duplicate)
                {
                    objResult.StatusCode = 0;
                    objResult.Message = "Loan Approval already exist !!";
                    return Ok(objResult);
                }
                else
                {

                    var _emp_id_exist = _context.tbl_loan_approval_setting.Where(x => x.company_id == objloanSetting.company_id && x.approver_type == objloanSetting.approver_type && (x.emp_id == objloanSetting.emp_id && x.approver_role_id == objloanSetting.approver_role_id)).OrderByDescending(x => x.Sno).FirstOrDefault();
                    //var exist_approver_role_id = _context.tbl_loan_approval_setting.Where(x => x.company_id == objloanSetting.company_id && x.approver_role_id == objloanSetting.approver_role_id).ToList();
                    //if(exist_approver_role_id)
                    var _chekorder = _context.tbl_loan_approval_setting.Where(x => x.company_id == objloanSetting.company_id && x.order == objloanSetting.order && x.approver_type == objloanSetting.approver_type && x.is_active == 1).OrderByDescending(x => x.Sno).FirstOrDefault();
                    if (_chekorder != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "This Level of approver already exist in Company";
                    }
                    else if (_emp_id_exist != null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "Approver already exist...";
                    }
                    else
                    {
                        var check_final_approver = _context.tbl_loan_approval_setting.Where(x => x.company_id == objloanSetting.company_id && x.is_final_approver == objloanSetting.is_final_approver && x.approver_type == objloanSetting.approver_type && x.is_active == 1).FirstOrDefault();


                        if (check_final_approver != null && check_final_approver.is_final_approver == 1)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "More than one Final Approver are not exist in same company !!";
                        }
                        else
                        {
                            //objloanSetting.emp_id = objloanSetting.emp_id;
                            //objloanSetting.order = objloanSetting.order;
                            objloanSetting.is_active = 1;
                            //objloanSetting.company_id = objloanSetting.company_id;
                            //objloanSetting.is_final_approver = objloanSetting.is_final_approver;
                            //objloanSetting.created_by = objloanSetting.created_by;
                            objloanSetting.created_dt = DateTime.Now;

                            _context.Entry(objloanSetting).State = EntityState.Added;
                            _context.SaveChanges();

                            objResult.StatusCode = 0;
                            objResult.Message = "Loan Approval Setting Save Successfully !!";
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

        //Get Loan Approval Setting 
        [Route("Get_LoanApprovalSetting/{sno}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LoanApprovalSettingMaster))]
        public IActionResult Get_LoanApprovalSetting([FromRoute] int sno)
        {
            try
            {
                if (sno > 0)
                {
                    var result = _context.tbl_loan_approval_setting.Join(_context.tbl_employee_master, a => a.emp_id, b => b.employee_id, (a, b) => new
                    {
                        a.Sno,
                        a.emp_id,
                        b.emp_code,
                        order_id = a.order,
                        order = a.order == 1 ? "Approval 1" : a.order == 2 ? "Approval 2" : a.order == 3 ? "Approval 3" : a.order == 4 ? "Approval 4" : "Approval 5",
                        b.is_active,
                        a.created_by,
                        a.created_dt,

                    }).Join(_context.tbl_user_master, a => a.emp_id, b => b.employee_id, (a, b) => new
                    {
                        a.Sno,
                        a.emp_id,
                        a.emp_code,
                        a.order_id,
                        a.order,
                        b.is_active,
                        a.created_by,
                        a.created_dt,
                        b.default_company_id,

                    }).Where(c => c.Sno == sno && c.is_active == 1).FirstOrDefault();
                    return Ok(result);
                }
                else
                {
                    var result = (from e in _context.tbl_loan_approval_setting
                                  join d in _context.tbl_employee_master on e.emp_id equals d.employee_id into jointable
                                  from empidd in jointable.DefaultIfEmpty()
                                  join g in _context.tbl_role_master on e.approver_role_id equals g.role_id into joinntable2
                                  from approverroldid in joinntable2.DefaultIfEmpty()
                                  orderby e.Sno
                                  where _clsCurrentUser.CompanyId.Contains(e.company_id)
                                  select new
                                  {
                                      e.Sno,
                                      e.emp_id,
                                      e.order,
                                      e.is_active,
                                      e.created_dt,
                                      e.last_modified_date,
                                      e.is_final_approver,
                                      e.approver_role_id,
                                      e.company_id,
                                      e.company_master.company_name,
                                      e.approver_type,
                                      approver_emp_name_code = string.Format("{0} {1} {2} ({3})",
                             empidd.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                             empidd.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                             empidd.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                             empidd.emp_code),
                                      approverroldid.role_name
                                  }).ToList();

                    return Ok(result);

                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
#endregion


#region  *it.intern2*
        ////#it.intern2 

        //[Route("Save_BankMaster")]
        //[HttpPost]
        //public IActionResult Save_BankMaster([FromBody] tbl_bank_master objtab)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    try
        //    {//need to add bank master in context
        //        var exist = _context.tbl_bank_master.Where(x => x.bank_name == objtab.bank_name).FirstOrDefault();

        //        if (exist != null)
        //        {
        //            objResult.StatusCode = 0;
        //            objResult.Message = "Bank Name already exist !!";
        //            return Ok(objResult);
        //        }
        //        else
        //        {
        //            objtab.last_modified_date = DateTime.Now;
        //            objtab.created_dt = DateTime.Now;
        //            objtab.is_deleted = 0;
        //            _context.Entry(objtab).State = EntityState.Added;
        //            _context.SaveChanges();

        //            objResult.StatusCode = 0;
        //            objResult.Message = "Tab name save successfully !!";
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





        //[Route("Get_Bankname/{bank_id}")]
        //[HttpGet]
        //////[Authorize]

        //public async Task<IActionResult> Get_Bankname([FromBody] int bank_id)
        //{
        //    try
        //    {
        //        if (bank_id > 0)
        //        {
        //            var result = _context.tbl_bank_master.Where(a => a.bank_id == bank_id && a.is_deleted == 0).ToList();                   
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            var result = _context.tbl_bank_master.Where( a=>a.is_deleted == 0).ToList();
        //            return Ok(result);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}

        //[Route("Update_BankMaster")]
        //[HttpPut]
        //public IActionResult Update_BankMaster([FromBody] tbl_bank_master objkeym)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    try
        //    {
        //        var exist = _context.tbl_bank_master.Where(x => x.bank_id == objkeym.bank_id).FirstOrDefault();

        //        if (exist == null)
        //        {
        //            objResult.StatusCode = 1;
        //            objResult.Message = "Invalid Key id please try again !!";
        //            return Ok(objResult);
        //        }
        //        else
        //        {
        //            //check with bank Name in other bank id
        //            var duplicate = _context.tbl_bank_master.Where(x => x.bank_id != objkeym.bank_id && x.bank_name == objkeym.bank_name).FirstOrDefault();
        //            if (duplicate != null)
        //            {
        //                objResult.StatusCode = 1;
        //                objResult.Message = "Bank Name exist in the system please check & try !!";
        //                return Ok(objResult);
        //            }
        //            exist.bank_name = objkeym.bank_name;
        //            exist.is_deleted = objkeym.is_deleted;
        //            exist.last_modified_by = 1;
        //            exist.last_modified_date = DateTime.Now;

        //            _context.Entry(exist).State = EntityState.Modified;
        //            _context.SaveChanges();

        //            objResult.StatusCode = 0;
        //            objResult.Message = "Key Details updated successfully !!";
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

        //[Route("Save_BranchMaster")]
        //[HttpPost]
        //public IActionResult Save_BranchMaster([FromBody] tbl_branch_master objtab)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    try
        //    {//need to add bank master in context
        //        var exist = _context.tbl_branch_master.Where(x => x.branch_name == objtab.branch_name).FirstOrDefault();

        //        if (exist != null)
        //        {
        //            objResult.StatusCode = 0;
        //            objResult.Message = "Branch Name already exist !!";
        //            return Ok(objResult);
        //        }
        //        else
        //        {
        //            objtab.branch_name = objtab.branch_name;
        //            objtab.created_dt = DateTime.Now;
        //            objtab.is_deleted = 0;
        //            objtab.last_modified_date = DateTime.Now;
        //            _context.Entry(objtab).State = EntityState.Added;
        //            _context.SaveChanges();

        //            objResult.StatusCode = 0;
        //            objResult.Message = "Tab name save successfully !!";
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

        //[Route("Get_Branchmaster/{branch_id}")]
        //[HttpGet]
        //public async Task<IActionResult> Get_Branchmaster([FromBody] int branch_id)
        //{
        //    try
        //    {
        //        if(branch_id > 0)
        //        {
        //            var result = _context.tbl_branch_master.Where(a => a.branch_id == branch_id && a.is_deleted == 0).ToList();
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            var result = _context.tbl_branch_master.Where(x => x.is_deleted == 0).ToList();
        //            return Ok(result);
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        return Ok(e.Message);
        //    }

        //}

        //[Route("Update_BranchMaster")]
        //[HttpPut]
        //public IActionResult Update_BranchMaster([FromBody] tbl_branch_master objkeym)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    try
        //    {
        //        var exist = _context.tbl_branch_master.Where(x => x.branch_id == objkeym.branch_id).FirstOrDefault();

        //        if (exist == null)
        //        {
        //            objResult.StatusCode = 1;
        //            objResult.Message = "Invalid Key id please try again !!";
        //            return Ok(objResult);
        //        }
        //        else
        //        {
        //            //check with bank Name in other bank id
        //            var duplicate = _context.tbl_branch_master.Where(x => x.branch_id != objkeym.bank_id && x.branch_name == objkeym.branch_name).FirstOrDefault();
        //            if (duplicate != null)
        //            {
        //                objResult.StatusCode = 1;
        //                objResult.Message = "Key name exist in the system please check & try !!";
        //                return Ok(objResult);
        //            }
        //            exist.bank_id = objkeym.bank_id;
        //            exist.loc = objkeym.loc;
        //            exist.ifsc_code = objkeym.ifsc_code;
        //            exist.branch_name = objkeym.branch_name;
        //            exist.is_deleted = objkeym.is_deleted;
        //            exist.last_modified_by = 1;
        //            exist.last_modified_date = DateTime.Now;

        //            _context.Entry(exist).State = EntityState.Modified;
        //            _context.SaveChanges();

        //            objResult.StatusCode = 0;
        //            objResult.Message = "Key Details updated successfully !!";
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

        //[Route("save_loanmaster")]
        //[HttpPost]
        //public IActionResult save_loanmaster([FromBody] tbl_loan_master objtab)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    try
        //    {//need to add bank master in context and edit next line condtn
        //        var exist = _context.tbl_loan_master.Where(x => x.loan_id == objtab.loan_id).FirstOrDefault();

        //        if (exist != null)
        //        {
        //            objResult.StatusCode = 0;
        //            objResult.Message = "Loan already exist !!";
        //            return Ok(objResult);
        //        }
        //        else
        //        {
        //            objtab.created_dt = DateTime.Now;
        //            objtab.is_deleted = 0;
        //            objtab.last_modified_date = DateTime.Now;
        //            _context.Entry(objtab).State = EntityState.Added;
        //            _context.SaveChanges();
        //            objResult.StatusCode = 0;
        //            objResult.Message = "Tab name save successfully !!";
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

        //[Route("Get_loanmaster/{loan_id}")]
        //[HttpGet]
        //public async Task<IActionResult> Get_loanmaster([FromBody] int loan_id)
        //{
        //    try
        //    {
        //        if (loan_id > 0)
        //        {
        //            var result = _context.tbl_loan_master.Where(x => x.loan_id == loan_id && x.is_deleted == 0).ToList();
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            var result = _context.tbl_loan_master.Where(x => x.is_deleted == 0).ToList();
        //            return Ok(result);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}

        //[Route("Update_LoanMaster")]
        //[HttpPut]
        //public IActionResult Update_LoanMaster([FromBody] tbl_loan_master objkeym)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    try
        //    {
        //        var exist = _context.tbl_loan_master.Where(x => x.loan_id == objkeym.loan_id).FirstOrDefault();

        //        if (exist == null)
        //        {
        //            objResult.StatusCode = 1;
        //            objResult.Message = "Invalid Key id please try again !!";
        //            return Ok(objResult);
        //        }
        //        else
        //        {
        //            //check with bank Name in other bank id
        //            var duplicate = _context.tbl_loan_master.Where(x => x.loan_id != objkeym.loan_id && x.rate_of_interest == objkeym.rate_of_interest).FirstOrDefault(); //needs t o be edited later as roi can be same for 2 
        //            if (duplicate != null)
        //            {
        //                objResult.StatusCode = 1;
        //                objResult.Message ="Loan exist in the system please check & try !!";
        //                return Ok(objResult);
        //            }
        //            exist.rate_of_interest = objkeym.rate_of_interest;
        //            exist.no_of_tenure = objkeym.no_of_tenure;
        //            exist.max_loan = objkeym.max_loan;
        //            exist.from_date = objkeym.from_date;
        //            exist.is_deleted = objkeym.is_deleted;
        //            exist.min_no_days_work = objkeym.min_no_days_work;
        //            exist.emp_status = objkeym.emp_status;
        //            //exist.mul_salary = objkeym.mul_salary;
        //            exist.last_modified_by = 1;
        //            exist.last_modified_date = DateTime.Now;

        //            _context.Entry(exist).State = EntityState.Modified;
        //            _context.SaveChanges();

        //            objResult.StatusCode = 0;
        //            objResult.Message = "Key Details updated successfully !!";
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

        //[Route("Save_CompanyAccount")]
        //[HttpPost]
        //public IActionResult Save_CompanyAccount([FromBody] tbl_company_accounts objtab)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    try
        //    {//need to add bank master in context and edit next line condtn
        //        var exist = _context.tbl_company_accounts.Where(x => x.cmpny_acct_no == objtab.cmpny_acct_no).FirstOrDefault();

        //        if (exist != null)
        //        {
        //            objResult.StatusCode = 0;
        //            objResult.Message = "Account already exist !!";
        //            return Ok(objResult);
        //        }
        //        else
        //        {
        //            objtab.created_dt = DateTime.Now;
        //            objtab.is_deleted = 0;
        //            objtab.last_modified_date = DateTime.Now;
        //            _context.Entry(objtab).State = EntityState.Added;
        //            _context.SaveChanges();

        //            objResult.StatusCode = 0;
        //            objResult.Message = "Tab name save successfully !!";
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

        //[Route("Get_companyacc/{cmpny_acct_id}")]
        //[HttpGet]
        //public async Task<IActionResult> Get_companyacc([FromBody] int cmpny_acct_id)
        //{
        //    try
        //    {
        //        if(cmpny_acct_id > 0)
        //        {
        //            var result = _context.tbl_company_accounts.Where(x => x.cmpny_acct_id == cmpny_acct_id && x.is_deleted == 0).ToList();
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            var result = _context.tbl_company_accounts.Where(x => x.is_deleted == 0).ToList();
        //            return Ok(result);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}

        //[Route("Update_CompanyaccMaster")]
        //[HttpPut]
        //public IActionResult Update_CompanyaccMaster([FromBody] tbl_company_accounts objkeym)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    try
        //    {
        //        var exist = _context.tbl_company_accounts.Where(x => x.cmpny_acct_id == objkeym.cmpny_acct_id).FirstOrDefault();

        //        if (exist == null)
        //        {
        //            objResult.StatusCode = 1;
        //            objResult.Message = "Invalid Key id please try again !!";
        //            return Ok(objResult);
        //        }
        //        else
        //        {
        //            //check with bank Name in other bank id
        //            var duplicate = _context.tbl_company_accounts.Where(x => x.cmpny_acct_id != objkeym.cmpny_acct_id && x.cmpny_acct_no == objkeym.cmpny_acct_no).FirstOrDefault();
        //            if (duplicate != null)
        //            {
        //                objResult.StatusCode = 1;
        //                objResult.Message = "Account no exist in the system please check & try !!";
        //                return Ok(objResult);
        //            }
        //            exist.bank_id = objkeym.bank_id;
        //            exist.cmpny_acct_id = objkeym.cmpny_acct_id;
        //            exist.cmpny_acct_no = objkeym.cmpny_acct_no;
        //            exist.branch_id = objkeym.branch_id;
        //            exist.is_deleted = objkeym.is_deleted;
        //            exist.last_modified_by = 1;
        //            exist.last_modified_date = DateTime.Now;

        //            _context.Entry(exist).State = EntityState.Modified;
        //            _context.SaveChanges();

        //            objResult.StatusCode = 0;
        //            objResult.Message = "Key Details updated successfully !!";
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

        //[Route("Save_EmployeeAccount")]
        //[HttpPost]
        //public IActionResult Save_EmployeeAccount([FromBody] tbl_emp_accounts objtab)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    try
        //    {//need to add bank master in context and edit next line condtn
        //        var exist = _context.tbl_emp_account.Where(x => x.emp_ac_no == objtab.emp_ac_no).FirstOrDefault();

        //        if (exist != null)
        //        {
        //            objResult.StatusCode = 0;
        //            objResult.Message = "Account already exist !!";
        //            return Ok(objResult);
        //        }
        //        else
        //        {
        //            objtab.created_dt = DateTime.Now;
        //            objtab.is_deleted = 0;
        //            objtab.last_modified_date = DateTime.Now;
        //            _context.Entry(objtab).State = EntityState.Added;
        //            _context.SaveChanges();

        //            objResult.StatusCode = 0;
        //            objResult.Message = "Tab name save successfully !!";
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


        //[Route("Get_empacc/{emp_acct_id}")]
        //[HttpGet]
        //public async Task<IActionResult> Get_empacc([FromBody] int emp_acct_id)
        //{
        //    try
        //    {
        //        if (emp_acct_id > 0)
        //        {
        //            var result = _context.tbl_emp_account.Where(x => x.emp_ac_id == emp_acct_id && x.is_deleted == 0).ToList();
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            var result = _context.tbl_emp_account.Where(x => x.is_deleted == 0).ToList();
        //            return Ok(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}

        //[Route("Update_EmpaccMaster")]
        //[HttpPut]
        //public IActionResult Update_EmpaccMaster([FromBody] tbl_emp_accounts objkeym)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    try
        //    {
        //        var exist = _context.tbl_emp_account.Where(x => x.emp_ac_id == objkeym.emp_ac_id).FirstOrDefault();

        //        if (exist == null)
        //        {
        //            objResult.StatusCode = 1;
        //            objResult.Message = "Invalid Key id please try again !!";
        //            return Ok(objResult);
        //        }
        //        else
        //        {
        //            //check with bank Name in other bank id
        //            var duplicate = _context.tbl_emp_account.Where(x => x.emp_ac_id != objkeym.emp_ac_id && x.emp_ac_no == objkeym.emp_ac_no).FirstOrDefault();
        //            if (duplicate != null)
        //            {
        //                objResult.StatusCode = 1;
        //                objResult.Message = "Account no. exist in the system please check & try !!";
        //                return Ok(objResult);
        //            }
        //            exist.bank_id = objkeym.bank_id;
        //            exist.cmpny_acct_id = objkeym.cmpny_acct_id;
        //            exist.emp_ac_no = objkeym.emp_ac_no;
        //            exist.branch_id = objkeym.branch_id;
        //            exist.is_deleted = objkeym.is_deleted;
        //            exist.last_modified_by = 1;
        //            exist.last_modified_date = DateTime.Now;

        //            _context.Entry(exist).State = EntityState.Modified;
        //            _context.SaveChanges();

        //            objResult.StatusCode = 0;
        //            objResult.Message = "Key Details updated successfully !!";
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


#endregion

#region  ******************************CREATED BY AMARJEET, CREATED DATE 27-04-2019, PAYROLL ***********************************

        //get all employee type from enum
        [Route("GetFunctions")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Repository))]
        public IActionResult GetFunctions()
        {
            try
            {
                List<object> list = new List<object>();
                foreach (enm_payroll_functions func in Enum.GetValues(typeof(enm_payroll_functions)))
                {
                    int value = (int)Enum.Parse(typeof(enm_payroll_functions), Enum.GetName(typeof(enm_payroll_functions), func));
                    string Str = Enum.GetName(typeof(enm_payroll_functions), value);

                    Type type = func.GetType();
                    MemberInfo[] memInfo = type.GetMember(func.ToString());

                    if (memInfo != null && memInfo.Length > 0)
                    {
                        object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (attrs != null && attrs.Length > 0)
                        {
                            string strvalue = ((DescriptionAttribute)attrs[0]).Description;

                            list.Add(new
                            {
                                FunValue = strvalue,
                                FunText = strvalue
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


        //get all employee type from enum
        [Route("GetOperator")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Repository))]
        public IActionResult GetOperator()
        {
            try
            {
                List<object> list = new List<object>();
                foreach (enm_payroll_operator func in Enum.GetValues(typeof(enm_payroll_operator)))
                {
                    int value = (int)Enum.Parse(typeof(enm_payroll_operator), Enum.GetName(typeof(enm_payroll_operator), func));
                    string Str = Enum.GetName(typeof(enm_payroll_operator), value);

                    Type type = func.GetType();
                    MemberInfo[] memInfo = type.GetMember(func.ToString());

                    if (memInfo != null && memInfo.Length > 0)
                    {
                        object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (attrs != null && attrs.Length > 0)
                        {
                            string strvalue = ((DescriptionAttribute)attrs[0]).Description;

                            list.Add(new
                            {
                                FunValue = strvalue,
                                FunText = strvalue
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


        [HttpGet("GetComponents/{component_id}/{company_id}")]
        //might not work
        [Authorize(Policy = nameof(enmMenuMaster.Repository))]
        public async Task<IActionResult> GetComponents([FromRoute] int component_id, int company_id)
        {
            ResponseMsg objResult = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }
            try
            {
                if (component_id > 0)
                {
                    var comp_details = _context.tbl_component_master.Where(a => a.component_id == component_id && a.is_active == 1).FirstOrDefault();
                    // var comp_details1 = _context.tbl_component_master.Include(p => p.tbl_component_formula_details).Where(p =>  p.component_id == component_id && p.tbl_component_formula_details.Where(q => q.is_deleted == 0 && q.company_id == company_id).Count() > 0).ToList();
                    return Ok(comp_details);
                }
                else if (component_id == -1)
                {
                    var filter_comp_ = getComponentTree(0);

                    return Ok(filter_comp_);
                }
                else
                {
                    if (company_id > 0)
                    {
                        var comp_details = _context.tbl_component_formula_details.Where(a => a.comp_master.is_active == 1 && a.company_id == company_id && a.comp_master.is_system_key == 0).Select(p => new
                        {
                            p.comp_master.component_id,
                            p.comp_master.property_details,
                            p.function_calling_order,
                            p.comp_master.component_name
                        }).Distinct().ToList();

                        return Ok(comp_details);
                    }
                    else
                    {
                        var comp_details = _context.tbl_component_formula_details.Where(a => a.comp_master.is_active == 1 && a.comp_master.is_system_key == 0).Select(p => new
                        {
                            p.comp_master.component_id,
                            p.comp_master.property_details,
                            p.function_calling_order,
                            p.comp_master.component_name
                        }).Distinct().ToList();

                        return Ok(comp_details);
                    }

                    // var comp_details = _context.tbl_component_master.Where(a => a.is_active == 1).ToList();
                    // var comp_details = _context.tbl_component_master.Include(p => p.tbl_component_formula_details).Where(p => p.tbl_component_formula_details.Where(q => q.is_deleted == 0).Count() > 0).ToList();

                }
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.ToString();
                return Ok(objResult);
            }

        }

        [HttpGet("GetFormulaBySalaryGroupId/{component_id}/{salary_group_id}/{company_id}")]
        [Authorize(Policy = nameof(enmMenuMaster.Repository))]
        public async Task<IActionResult> GetFormulaBySalaryGroupId([FromRoute]int component_id, int salary_group_id, int company_id)
        {
            ResponseMsg objResult = new ResponseMsg();
            try
            {

                var formula_details = _context.tbl_component_formula_details.Where(a => a.component_id == component_id && a.salary_group_id == salary_group_id && a.company_id == company_id && a.is_deleted == 0).FirstOrDefault();
                return Ok(formula_details);

            }
            catch (Exception ex)
            {
                objResult.Message = ex.ToString();
                return Ok(objResult);
            }
        }

        //Tree Component Model
        public class TreeViewNode
        {
            public int id { get; set; }
            public string text { get; set; }
            public bool state { get; set; }
            public List<TreeViewNode> children { get; set; }
        }

        // Funcation for Get Componet Tree
        public List<TreeViewNode> getComponentTree(int ComponentId)
        {
            List<TreeViewNode> treeViewNodes = null;
            treeViewNodes = _context.tbl_component_master.
                Where(p => p.parentid == ComponentId && p.is_active == 1).Select(p => new TreeViewNode { id = p.component_id, text = p.property_details, state = true }).ToList();
            if (treeViewNodes != null || treeViewNodes.Count() > 0)
            {
                for (int Index = 0; Index < treeViewNodes.Count(); Index++)
                {
                    treeViewNodes[Index].children = getComponentTree(treeViewNodes[Index].id);
                }
            }
            return treeViewNodes;
        }

        private bool componenetNameAlreadyExist(string componentname, int componentId)
        {
            return _context.tbl_component_master.Where(p => p.component_name == componentname && p.component_id != componentId).Count() > 0 ? true : false;
        }
        private bool componenetDetailAlreadyExist(string property_details, int componentId)
        {
            return _context.tbl_component_master.Where(p => p.property_details == property_details && p.component_id != componentId).Count() > 0 ? true : false;
        }

        // POST: api/apiPayroll
        [Route("CreateFormula")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.Repository))]
        public async Task<IActionResult> CreateFormula([FromBody] CreateFormula CreateForm)
        {

            ResponseMsg objResult = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(CreateForm.company_id))
            {
                objResult.StatusCode = 0;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        tbl_component_master component_mast = null;
                        if (CreateForm.component_id > 0)
                        {
                            component_mast = _context.tbl_component_master.FirstOrDefault(p => p.component_id == CreateForm.component_id);
                            if (component_mast == null)
                            {
                                objResult.StatusCode = 0;
                                objResult.Message = "Invalid componenet!";
                                return Ok(objResult);
                            }
                            if (componenetNameAlreadyExist(CreateForm.component_name, CreateForm.component_id) || componenetNameAlreadyExist(CreateForm.property_details, CreateForm.component_id))
                            {
                                objResult.StatusCode = 0;
                                objResult.Message = "Component already exist for this company...!";
                                return Ok(objResult);
                            }
                            List<tbl_component_formula_details> tcfds = _context.tbl_component_formula_details.Where(p => p.component_id == CreateForm.component_id && p.salary_group_id == CreateForm.salary_group_id && p.is_deleted == 0).ToList();
                            tcfds.ForEach(p => { p.is_deleted = 1; p.deleted_by = CreateForm.modified_by; p.deleted_dt = DateTime.Now; });
                            _context.tbl_component_formula_details.UpdateRange(tcfds);
                            _context.SaveChanges();
                        }
                        else
                        {
                            component_mast = new tbl_component_master();
                            component_mast.created_by = CreateForm.modified_by;
                            component_mast.created_dt = DateTime.Now;
                        }

                        component_mast.component_name = CreateForm.component_name;
                        if (CreateForm.component_name == "@NetEarning" || CreateForm.component_name == "@SalaryComponent" || CreateForm.component_name == "CreateForm.component_name")
                        {
                            component_mast.parentid = 0;
                        }
                        else
                        {
                            component_mast.parentid = CreateForm.parentid;
                        }
                        component_mast.property_details = CreateForm.property_details;
                        component_mast.is_active = CreateForm.is_active;
                        component_mast.datatype = CreateForm.datatype;
                        component_mast.modified_by = CreateForm.modified_by;
                        component_mast.modified_dt = DateTime.Now;
                        component_mast.component_type = CreateForm.component_type;
                        component_mast.is_salary_comp = CreateForm.is_salary_comp;
                        component_mast.is_tds_comp = CreateForm.is_tds_comp;
                        component_mast.is_data_entry_comp = CreateForm.is_data_entry_comp;
                        component_mast.payment_type = CreateForm.payment_type;
                        component_mast.is_user_interface = CreateForm.user_interface;
                        component_mast.is_payslip = CreateForm.is_payslip;
                        _context.tbl_component_master.Attach(component_mast);
                        await _context.SaveChangesAsync();


                        var previous_formula_dtl = _context.tbl_component_formula_details.OrderByDescending(x => x.sno).Where(p => p.company_id == CreateForm.company_id && p.component_id == CreateForm.component_id && p.salary_group_id == CreateForm.salary_group_id).FirstOrDefault();

                        tbl_component_formula_details obj_ComponentPropertyDetails = new tbl_component_formula_details();
                        obj_ComponentPropertyDetails.component_id = component_mast.component_id;
                        obj_ComponentPropertyDetails.company_id = CreateForm.company_id;
                        obj_ComponentPropertyDetails.salary_group_id = CreateForm.salary_group_id;
                        obj_ComponentPropertyDetails.formula = CreateForm.formula;
                        obj_ComponentPropertyDetails.function_calling_order = previous_formula_dtl != null ? previous_formula_dtl.function_calling_order : CreateForm.function_calling_order;  //CreateForm.function_calling_order;
                        obj_ComponentPropertyDetails.is_deleted = 0;
                        obj_ComponentPropertyDetails.created_by = CreateForm.modified_by;
                        obj_ComponentPropertyDetails.deleted_by = CreateForm.modified_by;
                        obj_ComponentPropertyDetails.created_dt = DateTime.Now;
                        obj_ComponentPropertyDetails.deleted_dt = DateTime.Now;

                        _context.tbl_component_formula_details.Add(obj_ComponentPropertyDetails);
                        await _context.SaveChangesAsync();
                        objResult.StatusCode = 1;
                        objResult.Message = "Submitted successfully...!";
                        trans.Commit();
                        return Ok(objResult);


                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        objResult.Message = ex.Message;
                        objResult.StatusCode = 0;
                        return Ok(objResult);
                    }
                }
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.ToString();
                return Ok(objResult);
            }
        }



        [Route("GetLoanByEmployee/{employee_id}/{sp_mode}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Loan))]
        public IActionResult GetLoanByEmployee([FromRoute] int employee_id, int sp_mode)
        {

            try
            {
                ResponseMsg objResult = new ResponseMsg();

                //get all loan data by emp id
                if (sp_mode == 1)
                {
                    var final_result = _context.tbl_loan_request.Where(a => a.is_deleted == 0 && a.is_closed == 0 && a.req_emp_id == employee_id).ToList();
                    if (final_result == null)
                    {
                        objResult.Message = "Record Not Found...!";
                        objResult.StatusCode = 0;
                        return Ok(objResult);
                    }
                    else
                    {
                        return Ok(final_result);
                    }
                } // For ddl
                else
                {
                    var final_result = _context.tbl_loan_request.Select(a => new { a.loan_req_id, a.loan_purpose, a.is_deleted, a.is_closed, a.is_final_approval, a.req_emp_id }).Where(a => a.is_deleted == 0 && a.is_closed == 0 && a.is_final_approval == 1 && a.req_emp_id == employee_id).ToList();
                    if (final_result == null)
                    {
                        objResult.Message = "Record Not Found...!";
                        objResult.StatusCode = 0;
                        return Ok(objResult);
                    }
                    else
                    {
                        return Ok(final_result);
                    }
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [Route("Save_LoanRepayment")]
        [HttpPost]
        // Here Save Loan Data in loan reayment table
        [Authorize(Policy = nameof(enmMenuMaster.LoanRepayment))]
        public IActionResult Save_LoanRepayment([FromBody] tbl_loan_repayments objloan)
        {
            Response_Msg objResult = new Response_Msg();
            try
            {
                var exist = _context.tbl_loan_repayments.Where(a => a.loan_id == objloan.loan_id && a.date == objloan.date && a.is_deleted == 0).FirstOrDefault();

                if (exist != null)
                {
                    objResult.StatusCode = 0;
                    objResult.Message = "Loan already paid for this month...!";
                    return Ok(objResult);
                }
                else
                {

                    var check_paymant = _context.tbl_loan_repayments.Where(a => a.loan_id == objloan.loan_id && a.is_deleted == 0).OrderByDescending(a => a.loan_repay_id).FirstOrDefault();
                    if (check_paymant != null)
                    {
                        objloan.loan_balance = check_paymant.loan_balance - objloan.principal_amount;

                    }
                    else
                    {
                        var loan_amount = _context.tbl_loan_request.Select(a => new { a.loan_amt, a.loan_req_id, a.is_deleted, a.is_final_approval, a.is_closed })
                            .Where(a => a.is_closed == 0 && a.is_deleted == 0 && a.is_final_approval == 1 && a.loan_req_id == objloan.loan_id).FirstOrDefault();

                        objloan.loan_balance = Convert.ToDouble(loan_amount.loan_amt) - objloan.principal_amount;
                    }

                    objloan.created_date = DateTime.Now;
                    objloan.last_modified_date = DateTime.Now;
                    _context.Entry(objloan).State = EntityState.Added;
                    _context.SaveChanges();

                    objResult.StatusCode = 1;
                    objResult.Message = "Loan repayment successfully submited...!";
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



        [Route("GetLoanPaymentDeatil/{loan_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LoanRepayment))]
        public IActionResult GetLoanPaymentDeatil([FromRoute] int loan_id)
        {

            try
            {
                ResponseMsg objResult = new ResponseMsg();

                //get loan payment details 
                var final_result = _context.tbl_loan_repayments.Where(a => a.loan_id == loan_id && a.is_deleted == 0).Select(p => new
                {
                    p.loan_repay_id,
                    p.req_emp_id,
                    p.loan_id,
                    p.interest_component,
                    principal_amount = Math.Round((p.principal_amount), 2),
                    loan_balance = Math.Round((p.loan_balance), 2),
                    p.date,
                    p.remark,
                    p.status,
                    p.is_deleted,
                    p.created_date,
                    p.last_modified_date
                }).ToList();
                if (final_result == null)
                {
                    objResult.Message = "Record Not Found...!";
                    objResult.StatusCode = 0;
                    return Ok(objResult);
                }
                else
                {
                    return Ok(final_result);
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Save_EmployeeIncomeTaxAmount")]
        [HttpPost]
        // Here Save Loan Data in loan reayment table
        [Authorize(Policy = nameof(enmMenuMaster.EmployeeTax))]
        public IActionResult Save_EmployeeIncomeTaxAmount([FromBody] tbl_employee_income_tax_amount obj_income)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(obj_income.emp_id ?? 0))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access....!!";
                return Ok(objResult);
            }
            try
            {

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var exist = _context.tbl_employee_income_tax_amount.Where(a => a.emp_id == obj_income.emp_id && a.is_deleted == 0).FirstOrDefault();


                        if (exist != null)
                        {

                            exist.is_deleted = 1;
                            exist.last_modified_date = DateTime.Now;

                            _context.tbl_employee_income_tax_amount.Attach(exist);
                            _context.Entry(exist).State = EntityState.Modified;


                            obj_income.created_date = DateTime.Now;
                            obj_income.last_modified_date = DateTime.Now;

                            _context.tbl_employee_income_tax_amount.Add(obj_income);
                            _context.SaveChangesAsync();

                            transaction.Commit();

                            objResult.StatusCode = 0;
                            objResult.Message = "Tax amount updated successfully...!";
                            return Ok(objResult);
                        }
                        else
                        {

                            obj_income.created_date = DateTime.Now;
                            obj_income.last_modified_date = DateTime.Now;

                            _context.tbl_employee_income_tax_amount.Add(obj_income);
                            _context.SaveChangesAsync();

                            transaction.Commit();

                            objResult.StatusCode = 0;
                            objResult.Message = "Tax amount submited successfully...!";
                            return Ok(objResult);

                        }

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
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }


        [Route("Get_EmployeeIncomeTaxAmount/{Employee_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.EmployeeTax))]
        public IActionResult Get_EmployeeIncomeTaxAmount([FromRoute] int Employee_id)
        {
            ResponseMsg objResult = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(Employee_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access";
                return Ok(objResult);
            }


            try
            {


                //get emp tax amt
                var final_result = _context.tbl_employee_income_tax_amount.Where(a => a.emp_id == Employee_id && a.is_deleted == 0).Select(a => a.income_tax_amount).FirstOrDefault();
                if (final_result == 0)
                {
                    objResult.Message = "Record Not Found...!";
                    objResult.StatusCode = 0;
                    return Ok(objResult);
                }
                else
                {
                    return Ok(final_result);
                }

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        [Route("GetEmployeeListOfTaxAmount/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.EmployeeTax))]
        public IActionResult GetEmployeeListOfTaxAmount([FromRoute] int company_id)
        {
            ResponseMsg objResult = new ResponseMsg();

            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access";
                return Ok(objResult);
            }

            try
            {


                //get emp tax amt list
                var result = _context.tbl_employee_income_tax_amount
                    .Select(a => new
                    {
                        default_company_id = a.tbl_emp.tbl_user_master.Select(j => j.default_company_id).FirstOrDefault(),
                        employee_name = string.Format("{0} {1} {2} ({3})", a.tbl_emp.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).employee_first_name,
                        a.tbl_emp.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_middle_name != "").employee_middle_name,
                        a.tbl_emp.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.employee_last_name != "").employee_last_name,
                        a.tbl_emp.emp_code),
                        //employee_name = a.tbl_emp.tbl_emp_officaial_sec.Where(o => o.is_deleted == 0).Select(o => new
                        //{
                        //    o.employee_first_name,
                        //    o.employee_middle_name,
                        //    o.employee_last_name,
                        //    o.is_deleted
                        //}).FirstOrDefault(),
                        a.income_tax_amount,
                        a.created_date,
                        a.is_deleted
                    }).Where(a => a.default_company_id == company_id && a.is_deleted == 0).ToList();


                //var final_result = result.Select(a => new { a.employee_name.employee_first_name, a.employee_name.employee_middle_name, a.employee_name.employee_last_name, a.created_date, a.income_tax_amount });

                var final_result = result.Select(a => new { a.employee_name, a.created_date, a.income_tax_amount });


                if (final_result == null)
                {
                    objResult.Message = "Record Not Found...!";
                    objResult.StatusCode = 0;
                    return Ok(objResult);
                }
                else
                {
                    return Ok(final_result);
                }

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        //[Route("Save_Batch")]
        //[HttpPost]
        //// Here Save batch
        ////[Authorize(Policy = "8068")]
        //public IActionResult Save_Batch([FromBody] tbl_create_batch obj_batch)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    try
        //    {

        //        using (var transaction = _context.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var exist = _context.tbl_create_batch.Where(a => a.company_id == obj_batch.company_id && a.is_deleted == 0 && a.from_date >= obj_batch.from_date && a.to_date <= obj_batch.to_date).FirstOrDefault();

        //                if (exist != null)
        //                {
        //                    objResult.StatusCode = 0;
        //                    objResult.Message = "Batch already exist for this date...!";

        //                    return Ok(objResult);
        //                }
        //                else
        //                {
        //                    obj_batch.created_date = DateTime.Now;
        //                    obj_batch.last_modified_date = DateTime.Now;


        //                    _context.Entry(obj_batch).State = EntityState.Added;
        //                    _context.SaveChanges();

        //                    objResult.StatusCode = 1;
        //                    objResult.Message = "Submited Successfully...!";
        //                    return Ok(objResult);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();

        //                objResult.StatusCode = 0;
        //                objResult.Message = ex.Message;
        //                return Ok(objResult);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objResult.StatusCode = 0;
        //        objResult.Message = ex.Message;
        //        return Ok(objResult);
        //    }
        //}


        //[Route("Get_Batch/{company_id}")]
        //[HttpGet]
        ////[Authorize(Policy = "8069")]
        //public IActionResult Get_Batch([FromRoute] int company_id)
        //{

        //    try
        //    {
        //        ResponseMsg objResult = new ResponseMsg();

        //        var final_result = _context.tbl_create_batch.Where(a => a.is_deleted == 0 && a.company_id == company_id).ToList();

        //        if (final_result == null)
        //        {
        //            objResult.Message = "Record Not Found...!";
        //            objResult.StatusCode = 0;
        //            return Ok(objResult);
        //        }
        //        else
        //        {
        //            return Ok(final_result);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}


        [Route("GetEmployeeForPayroll/{company_id}/{month_year}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Payroll))]
        public IActionResult GetEmployeeForPayroll([FromRoute] int company_id, int month_year)
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
                _clsEmployeeDetail._company_id = company_id;
                return Ok(_clsEmployeeDetail.GetEmpPayrollData(month_year));
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }


        private void insertDatainPayrollProcess(List<int> NewEmpId, int company_id, int payroll_month_year, int created_by)
        {
            List<int> ExistingEmpId = _context.tbl_payroll_process_status.Where(p => p.company_id == company_id && p.payroll_month_year == payroll_month_year && p.is_deleted == 0).Select(p => p.emp_id ?? 0).Distinct().ToList();
            DateTime currentdateTime = DateTime.Now;
            NewEmpId.RemoveAll(p => ExistingEmpId.Contains(p));
            _context.tbl_payroll_process_status.AddRange(
            NewEmpId.Select(p => new tbl_payroll_process_status
            {
                company_id = company_id,
                payroll_month_year = payroll_month_year,
                emp_id = p,
                is_calculated = 0,
                is_lock = 0,
                is_freezed = 0,
                is_deleted = 0,
                payroll_status = 0,
                created_by = created_by,
                created_date = currentdateTime,
                last_modified_by = created_by,
                last_modified_date = currentdateTime
            }).ToList());
            _context.SaveChanges();

        }


        //Calculate Salary here
        [Route("ProcessPayroll")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.ProcessPayroll))]
        public IActionResult ProcessPayroll([FromBody] clsProcessEmployee obj_payroll)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(obj_payroll.company_id))
            {
                objResult.StatusCode = 0;
                objResult.Message = "Unauthorize Company Access";
                return Ok(objResult);
            }

            if (!_clsCurrentUser.DownlineEmpId.Any(p => obj_payroll.emp_id.Contains(p)))
            {
                objResult.StatusCode = 0;
                objResult.Message = "Unauthorize Access";
                return Ok(objResult);
            }

            try
            {

                string year = obj_payroll.payroll_month_year.ToString().Substring(0, 4);
                string month = obj_payroll.payroll_month_year.ToString().Substring(4, 2);
                DateTime _ApplicableDate = Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1).AddDays(-1);
                //For Cross check if previous month is calculated or not

                int previous_month_year = Convert.ToInt32(_ApplicableDate.AddMonths(-1).ToString("yyyyMM"));

                if (_context.tbl_payroll_process_status.Where(p => p.is_deleted == 0 && p.payroll_month_year != obj_payroll.payroll_month_year).Count() > 0)
                {
                    if (_context.tbl_payroll_process_status.Where(p => p.company_id == obj_payroll.company_id && p.is_deleted == 0 && p.payroll_month_year == previous_month_year).Count() == 0)
                    {
                        return Ok(new { StatusCode = 0, Message = "Please firstly Calculate or Lock Previous Payroll Month Year...!" });
                    }

                    if (_context.tbl_payroll_process_status.Where(p => p.company_id == obj_payroll.company_id && p.is_lock == 0 && p.is_deleted == 0 && p.payroll_month_year == previous_month_year).Count() > 0)
                    {
                        return Ok(new { StatusCode = 0, Message = "Please Lock Previous Payroll Month Year...!" });
                    }
                }

                DateTime currentdateTime = DateTime.Now;


                var selectEmpSalaryGroup = _context.tbl_sg_maping.Where(p => p.applicable_from_dt <= _ApplicableDate && p.is_active == 1).GroupBy(g => new { employee_id = g.emp_id })
                .Select(p => new { employee_id = p.Key.employee_id ?? 0, applicable_from_dt = p.Max(q => q.applicable_from_dt) })
                .Join(_context.tbl_sg_maping.Where(p => p.is_active == 1).Select(p => new { employee_id = p.emp_id ?? 0, applicable_from_dt = p.applicable_from_dt, salary_group_id = p.salary_group_id })
                , p => new { p.employee_id, p.applicable_from_dt }, q => new { q.employee_id, q.applicable_from_dt },
                (p, q) => new { p.employee_id, q.salary_group_id }).Join(_context.tbl_salary_group, p => p.salary_group_id, q => q.group_id,
                (p, q) => new { p.employee_id, q.group_name, q.group_id })
                .ToList();

                List<int> EmpID_sg = selectEmpSalaryGroup.Select(p => p.employee_id).Distinct().ToList();
                List<int> EmpID_sg_notAvaiblae = obj_payroll.emp_id.Where(p => !EmpID_sg.Contains(p)).ToList();
                var tbl_employee_masters = _context.tbl_employee_master.Where(p => p.tbl_user_master.Where(q => q.default_company_id == obj_payroll.company_id && q.is_active == 1).Count() > 0 && p.is_active == 1).Select(p => new { p.employee_id, p.emp_code }).ToList();
                //Need to store the All data in payroll process

                if (EmpID_sg_notAvaiblae.Count() > 0)
                {
                    StringBuilder Message = new StringBuilder("Salary group not define for Employees");
                    return Ok(new { StatusCode = 0, Message = Message.ToString() });
                }
                List<int> NewEmpId = obj_payroll.emp_id.Distinct().ToList();
                int[] NewEmpList = new int[NewEmpId.Count];
                NewEmpId.CopyTo(NewEmpList);
                insertDatainPayrollProcess(NewEmpList.ToList(), obj_payroll.company_id, obj_payroll.payroll_month_year, obj_payroll.created_by ?? 1);
                //Now Start in task an Call the fun
                List<tbl_payroll_process_status> tpps = _context.tbl_payroll_process_status.Where(p => obj_payroll.emp_id.Contains(p.emp_id ?? 0) && p.is_freezed == 0 && p.is_lock == 0 && p.is_deleted == 0 && p.payroll_month_year == obj_payroll.payroll_month_year).ToList();


                List<Task> tasklists = new List<Task>();
                tpps.ForEach(p =>
                {

                    Task tasklist = new Task(() =>
                    {
                        using (Context context = new Context())
                        {

                            Payroll pr = new Payroll(obj_payroll.company_id, _appSettings.Value.domain_url.ToString(), _converter, _hostingEnvironment, context,
                                                                   EmpID: p.emp_id ?? 0, PayrollMonthyear: obj_payroll.payroll_month_year, userId: obj_payroll.created_by ?? 1, SGID: selectEmpSalaryGroup.FirstOrDefault(q => q.employee_id == p.emp_id).group_id, config: _config);
                            pr.insert_into_tables();
                        }
                    });
                    tasklists.Add(tasklist);
                    tasklist.Start();
                });
                Task.WaitAll(tasklists.ToArray());

                return Ok(new { StatusCode = 1, Message = "Payroll Process Done...!" });

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }


        //Freeze Salary here
        [Route("ProcessForFreeze")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.ProcessPayroll))]
        public IActionResult ProcessForFreeze([FromBody] clsProcessEmployee obj_payroll)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(obj_payroll.company_id))
            {
                objresponse.StatusCode = 0;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }

            if (!_clsCurrentUser.DownlineEmpId.Any(p => obj_payroll.emp_id.Contains(p)))
            {
                objresponse.StatusCode = 0;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }
            try
            {

                var get_emp_data = obj_payroll.emp_id.ToList(); // contain employee id's
                                                                //check all employees of payroll process
                DateTime currentdateTime = DateTime.Now;
                if (get_emp_data.Count == 0)
                {
                    objresponse.StatusCode = 0;
                    objresponse.Message = "Please select employee";
                    return Ok(objresponse);
                }


                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {

                        int[] NewEmpList = new int[get_emp_data.Count];
                        get_emp_data.CopyTo(NewEmpList);
                        insertDatainPayrollProcess(NewEmpList.ToList(), obj_payroll.company_id, obj_payroll.payroll_month_year, obj_payroll.created_by ?? 1);
                        var ForFrezeingData = _context.tbl_payroll_process_status.Where(a => a.company_id == obj_payroll.company_id && a.payroll_month_year == obj_payroll.payroll_month_year && get_emp_data.Contains(a.emp_id ?? 0) && a.is_deleted == 0).ToList();

                        //get the Employee Data Which is not Exists in Process Table
                        ForFrezeingData.ForEach(p =>
                        {
                            p.is_freezed = 1; p.last_modified_by = obj_payroll.created_by ?? 1;
                            p.last_modified_date = currentdateTime;
                        });
                        _context.tbl_payroll_process_status.UpdateRange(ForFrezeingData);

                        _context.SaveChanges();



                        trans.Commit();

                        objresponse.StatusCode = 1;
                        objresponse.Message = "Freeze...";
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        objresponse.StatusCode = 0;
                        objresponse.Message = ex.Message;
                    }

                }


                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 0;
                objresponse.Message = ex.Message;
                return Ok(ex.Message);
            }
        }

        [Route("LockPayrollProcess")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.ProcessPayroll))]

        public IActionResult LockPayrollProcess([FromBody] clsProcessEmployee obj_payroll)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(obj_payroll.company_id))
            {
                objResult.StatusCode = 0;
                objResult.Message = "Unauthorize Company Access....!!";
                return Ok(objResult);
            }

            if (!_clsCurrentUser.DownlineEmpId.Any(p => obj_payroll.emp_id.Contains(p)))
            {
                objResult.StatusCode = 0;
                objResult.Message = "Unauthorize Access....";
                return Ok(objResult);
            }
            try
            {

                DateTime currentdateTime = DateTime.Now;
                var get_emp_data = obj_payroll.emp_id.ToList(); // contain employee id's
                if (get_emp_data.Count == 0)
                {
                    objResult.StatusCode = 0;
                    objResult.Message = "Please select employee";
                    return Ok(objResult);
                }


                using (var trans = _context.Database.BeginTransaction())
                {
                    int[] NewEmpList = new int[get_emp_data.Count];
                    get_emp_data.CopyTo(NewEmpList);
                    insertDatainPayrollProcess(NewEmpList.ToList(), obj_payroll.company_id, obj_payroll.payroll_month_year, obj_payroll.created_by ?? 1);
                    var ForFrezeingData = _context.tbl_payroll_process_status.Where(a => a.company_id == obj_payroll.company_id && a.payroll_month_year == obj_payroll.payroll_month_year && get_emp_data.Contains(a.emp_id ?? 0) && a.is_deleted == 0).ToList();
                    if (ForFrezeingData.Any(p => p.is_freezed == 0))
                    {
                        trans.Rollback();
                        objResult.StatusCode = 0;
                        objResult.Message = "Please Freeze first of all selected employee";
                        return Ok(objResult);
                    }
                    //get the Employee Data Which is not Exists in Process Table
                    ForFrezeingData.ForEach(p =>
                    {
                        p.is_lock = 1; p.last_modified_by = obj_payroll.created_by ?? 1;
                        p.last_modified_date = currentdateTime;
                    });
                    _context.tbl_payroll_process_status.UpdateRange(ForFrezeingData);

                    _context.SaveChanges();
                    trans.Commit();
                }

                objResult.StatusCode = 1;
                objResult.Message = "Locked...!";

                return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(ex.Message);

            }
        }







        [Obsolete("This Methode is no longer usable")]
        [Route("GetPayrollStatus/{company_id}/{month_year}")]
        [HttpGet]
        //[Authorize(Policy = "8074")]
        public IActionResult GetPayrollStatus([FromRoute] int company_id, int month_year)// status_type --0 for is_calculated / 1 is_freezed / 2 is_lock
        {
            try
            {

                //variables here
                int is_calculated = 0, is_all_calculated = 0, is_freezed = 0, is_all_freezed = 0, is_lock = 0, is_all_lock = 0;



                //If All  the payroll month year
                var get_all_month_year_data = _context.tbl_payroll_process_status.Where(a => a.payroll_month_year == month_year && a.company_id == company_id && a.is_deleted == 0).ToList();



                //if any one is calculated 
                var get_payroll_status_data = _context.tbl_payroll_process_status.Where(a => a.payroll_month_year == month_year && a.company_id == company_id && a.is_deleted == 0 && a.is_calculated == 1).ToList();

                if (get_payroll_status_data.Count > 0)
                {
                    is_calculated = 1;

                    if (get_all_month_year_data.Count == get_payroll_status_data.Count)
                    {
                        is_all_calculated = 1;
                    }
                }
                else
                {
                    is_calculated = 0;
                }



                //if any one is freezed 
                var get_any_one_freezed = _context.tbl_payroll_process_status.Where(a => a.payroll_month_year == month_year && a.company_id == company_id && a.is_deleted == 0 && a.is_freezed == 1).ToList();

                if (get_any_one_freezed.Count > 0)
                {
                    is_freezed = 1;

                    if (get_all_month_year_data.Count == get_any_one_freezed.Count)
                    {
                        is_all_freezed = 1;
                    }
                }
                else
                {
                    is_freezed = 0;
                }




                //if any one is lock 
                var get_any_one_lock = _context.tbl_payroll_process_status.Where(a => a.payroll_month_year == month_year && a.company_id == company_id && a.is_deleted == 0 && a.is_lock == 1).ToList();

                if (get_any_one_lock.Count > 0)
                {
                    is_lock = 1;

                    if (get_all_month_year_data.Count == get_any_one_lock.Count)
                    {
                        is_all_lock = 1;
                    }
                }
                else
                {
                    is_lock = 0;
                }




                return Ok(new { calculated = is_calculated, all_calculated = is_all_calculated, freezed = is_freezed, all_freezed = is_all_freezed, lock_ = is_lock, all_lock = is_all_lock });


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //Lock Payroll Process Salary here
        [Route("ResetPayrollProcess")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.ProcessPayroll))]

        public async Task<IActionResult> ResetPayrollProcess([FromBody] clsProcessEmployee obj_payroll)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(obj_payroll.company_id))
            {
                objResult.StatusCode = 0;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }

            if (!_clsCurrentUser.DownlineEmpId.Any(p => obj_payroll.emp_id.Contains(p)))
            {
                objResult.StatusCode = 0;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }
            try
            {
                //get data of payroll month year data for reset

                var get_if_lock = _context.tbl_payroll_process_status.Select(a => new { a.is_lock, a.is_deleted, a.company_id, a.payroll_month_year, a.emp_id }).Where(a => a.is_lock == 1 && a.is_deleted == 0 && a.payroll_month_year == obj_payroll.payroll_month_year && a.company_id == obj_payroll.company_id).ToList();
                var get_all_emp_payroll_status = _context.tbl_payroll_process_status.Select(a => new { a.is_lock, a.is_deleted, a.company_id, a.payroll_month_year, a.emp_id }).Where(a => a.is_deleted == 0 && a.payroll_month_year == obj_payroll.payroll_month_year && a.company_id == obj_payroll.company_id).ToList();
                ////if lock
                //if (get_if_lock.Count == get_all_emp_payroll_status.Count)
                if (get_if_lock.Count > 0 && get_all_emp_payroll_status.Any(x => x.emp_id == get_if_lock.FirstOrDefault().emp_id))
                {
                    objResult.StatusCode = 0;
                    objResult.Message = "Payroll Is Lock For " + obj_payroll.payroll_month_year + " Month year...!";
                    // return Ok(new { StatusCode = 0, Message = "Payroll Is Lock For " + obj_payroll.payroll_month_year + " Month year...!" });
                }
                else
                {
                    using (var trans = _context.Database.BeginTransaction())
                    {
                        try
                        {



                            List<tbl_daily_attendance> tbl_daily_attendances = _context.tbl_daily_attendance.Where(p => p.payrollmonthyear == obj_payroll.payroll_month_year && p.is_freezed == 0).ToList();
                            tbl_daily_attendances.ForEach(p => p.is_freezed = 0);

                            _context.tbl_daily_attendance.UpdateRange(tbl_daily_attendances);


                            //Reset from tbl_salary_input

                            List<tbl_salary_input> get_salary_input_data_for_reset = _context.tbl_salary_input.Where(a => a.is_active == 1 && a.monthyear == obj_payroll.payroll_month_year && a.tem.tbl_user_master.Select(b => new { b.default_company_id, b.employee_id, b.is_active }).Where(c => c.employee_id == a.emp_id && c.is_active == 1).FirstOrDefault().default_company_id == obj_payroll.company_id).ToList();

                            if (get_salary_input_data_for_reset.Count > 0)
                            {
                                _context.tbl_salary_input.RemoveRange(get_salary_input_data_for_reset);

                                List<tbl_payroll_process_status> get_data_from_payroll_status = _context.tbl_payroll_process_status.Where(a => a.company_id == obj_payroll.company_id && a.payroll_month_year == obj_payroll.payroll_month_year && a.is_deleted == 0).ToList();

                                if (get_data_from_payroll_status.Count > 0)
                                {
                                    _context.tbl_payroll_process_status.RemoveRange(get_data_from_payroll_status);

                                }

                                List<tbl_lossofpay_master> get_data_from_lop = _context.tbl_lossofpay_master.Where(a => a.company_id == obj_payroll.company_id && a.monthyear == obj_payroll.payroll_month_year && a.is_active == 1).ToList();

                                if (get_data_from_lop.Count > 0)
                                {
                                    _context.tbl_lossofpay_master.RemoveRange(get_data_from_lop);

                                }

                                //Reset Salary Change 

                                List<tbl_salary_input_change> get_data_from_salary_input_chang = _context.tbl_salary_input_change.Where(a => a.monthyear == obj_payroll.payroll_month_year.ToString() && a.company_id == obj_payroll.company_id && a.is_active == 1).ToList();

                                if (get_data_from_salary_input_chang.Count > 0)
                                {
                                    _context.tbl_salary_input_change.RemoveRange(get_data_from_salary_input_chang);

                                }



                                await _context.SaveChangesAsync();

                                trans.Commit();

                                objResult.StatusCode = 1;
                                objResult.Message = "Reset All Data Off " + obj_payroll.payroll_month_year + " Month year...!";
                            }
                            else
                            {
                                objResult.StatusCode = 0;
                                objResult.Message = "Payroll Is Not Calculated For This " + obj_payroll.payroll_month_year + " Month year...!";

                            }


                        }


                        catch (Exception ex)
                        {
                            trans.Rollback();
                            objResult.StatusCode = 1;
                            objResult.Message = ex.Message;

                            // return Ok(ex);
                        }


                    }
                }

                //return Ok(objResult);
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                //return Ok(objResult);
            }

            return Ok(objResult);
        }



        //public IActionResult ResetPayrollProcess([FromBody] clsProcessEmployee obj_payroll)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    try
        //    {
        //        //get data of payroll month year data for reset

        //        var get_if_lock = _context.tbl_payroll_process_status.Select(a => new { a.is_lock, a.is_deleted, a.company_id, a.payroll_month_year,a.emp_id }).Where(a => a.is_lock == 1 && a.is_deleted == 0 && a.payroll_month_year == obj_payroll.payroll_month_year && a.company_id == obj_payroll.company_id).ToList();
        //        var get_all_emp_payroll_status = _context.tbl_payroll_process_status.Select(a => new { a.is_lock, a.is_deleted, a.company_id, a.payroll_month_year,a.emp_id }).Where(a => a.is_deleted == 0 && a.payroll_month_year == obj_payroll.payroll_month_year && a.company_id == obj_payroll.company_id).ToList();
        //        ////if lock
        //        //if (get_if_lock.Count == get_all_emp_payroll_status.Count)
        //        if (get_if_lock.Count > 0 && get_all_emp_payroll_status.Any(x => x.emp_id == get_if_lock.FirstOrDefault().emp_id))
        //        {
        //            objResult.StatusCode = 0;
        //            objResult.Message = "Payroll Is Lock For " + obj_payroll.payroll_month_year + " Month year...!";
        //           // return Ok(new { StatusCode = 0, Message = "Payroll Is Lock For " + obj_payroll.payroll_month_year + " Month year...!" });
        //        }
        //        else
        //        {
        //            using (var trans = _context.Database.BeginTransaction())
        //            {
        //                try
        //                {

        //                    List<tbl_daily_attendance> tbl_daily_attendances = _context.tbl_daily_attendance.Where(p => p.payrollmonthyear == obj_payroll.payroll_month_year && p.is_freezed == 0).ToList();
        //                    tbl_daily_attendances.ForEach(p => p.is_freezed = 0);
        //                    //if (!(obj_payroll.emp_id == null || obj_payroll.emp_id.Count == 0))
        //                    //{
        //                    //    tbl_daily_attendances.RemoveAll(p => !obj_payroll.emp_id.Contains(p.emp_id ?? 0));
        //                    //}
        //                    _context.tbl_daily_attendance.UpdateRange(tbl_daily_attendances);
        //                   // _context.SaveChanges();

        //                    //Reset from tbl_salary_input
        //                    //var get_salary_input_data_for_reset = _context.tbl_salary_input.Where(a => a.is_active == 1 && a.monthyear == obj_payroll.payroll_month_year && a.tem.tbl_user_master.Select(b => new { b.default_company_id, b.employee_id }).Where(c => c.employee_id == a.emp_id).FirstOrDefault().default_company_id == obj_payroll.company_id).ToList();
        //                    List<tbl_salary_input> get_salary_input_data_for_reset = _context.tbl_salary_input.Where(a => a.is_active == 1 && a.monthyear == obj_payroll.payroll_month_year && a.tem.tbl_user_master.Select(b => new { b.default_company_id, b.employee_id ,b.is_active}).Where(c => c.employee_id == a.emp_id && c.is_active==1).FirstOrDefault().default_company_id == obj_payroll.company_id).ToList();


        //                    //remove if the data is already emp id exists
        //                    //if (!(obj_payroll.emp_id == null || obj_payroll.emp_id.Count == 0))
        //                    //{
        //                    //    get_salary_input_data_for_reset.RemoveAll(p => !obj_payroll.emp_id.Contains(p.emp_id ?? 0));
        //                    //}


        //                    if (get_salary_input_data_for_reset.Count > 0)
        //                    {

        //                        get_salary_input_data_for_reset.ForEach(p => { p.is_active = 0; p.modified_by = Convert.ToInt32(obj_payroll.created_by); p.modified_dt = DateTime.Now; });

        //                        _context.tbl_salary_input.UpdateRange(get_salary_input_data_for_reset);


        //                        for (int Index = 0; Index < get_salary_input_data_for_reset.Count; Index++)
        //                        {
        //                            get_salary_input_data_for_reset[Index].is_active = 0;
        //                            get_salary_input_data_for_reset[Index].modified_by = Convert.ToInt32(obj_payroll.created_by);
        //                            get_salary_input_data_for_reset[Index].modified_dt = DateTime.Now;

        //                            //  _context.tbl_salary_input.Add(get_salary_input_data_for_reset[Index]).State = EntityState.Modified;
        //                            _context.Entry(get_salary_input_data_for_reset[Index]).State = EntityState.Modified;

        //                        }

        //                        _context.tbl_salary_input.UpdateRange(get_salary_input_data_for_reset);
        //                        //_context.SaveChanges();


        //                        //Reset From tbl_payroll_process_status
        //                        //var get_data_from_payroll_status = _context.tbl_payroll_process_status.Where(a => a.company_id == obj_payroll.company_id && a.payroll_month_year == obj_payroll.payroll_month_year).ToList();

        //                        List<tbl_payroll_process_status> get_data_from_payroll_status = _context.tbl_payroll_process_status.Where(a => a.company_id == obj_payroll.company_id && a.payroll_month_year == obj_payroll.payroll_month_year && a.is_deleted == 0).ToList();

        //                        //if (!(obj_payroll.emp_id == null || obj_payroll.emp_id.Count == 0))
        //                        //{
        //                        //    get_data_from_payroll_status.RemoveAll(p => !obj_payroll.emp_id.Contains(p.emp_id ?? 0));
        //                        //}

        //                        if (get_data_from_payroll_status.Count > 0)
        //                        {

        //                            for (int Index1 = 0; Index1 < get_data_from_payroll_status.Count; Index1++)
        //                            {
        //                                get_data_from_payroll_status[Index1].is_deleted = 1;
        //                                get_data_from_payroll_status[Index1].last_modified_by = Convert.ToInt32(obj_payroll.created_by);
        //                                get_data_from_payroll_status[Index1].last_modified_date = DateTime.Now;

        //                                _context.Entry(get_data_from_payroll_status[Index1]).State = EntityState.Modified;

        //                                //_context.tbl_salary_input.Add(get_salary_input_data_for_reset[Index1]).State = EntityState.Modified;
        //                            }
        //                            _context.tbl_payroll_process_status.UpdateRange(get_data_from_payroll_status);
        //                           // _context.SaveChanges();
        //                        }


        //                        //var get_data_from_lop = _context.tbl_lossofpay_master.Where(a => a.company_id == obj_payroll.company_id && a.monthyear == obj_payroll.payroll_month_year).ToList();
        //                        List<tbl_lossofpay_master> get_data_from_lop = _context.tbl_lossofpay_master.Where(a => a.company_id == obj_payroll.company_id && a.monthyear == obj_payroll.payroll_month_year && a.is_active == 1).ToList();
        //                        //if (!(obj_payroll.emp_id == null || obj_payroll.emp_id.Count == 0))
        //                        //{
        //                        //    get_data_from_lop.RemoveAll(p => !obj_payroll.emp_id.Contains(p.emp_id ?? 0));
        //                        //}
        //                        if (get_data_from_lop.Count > 0)
        //                        {

        //                            for (int Index2 = 0; Index2 < get_data_from_lop.Count; Index2++)
        //                            {
        //                                get_data_from_lop[Index2].is_active = 0;
        //                                get_data_from_lop[Index2].modified_by = Convert.ToInt32(obj_payroll.created_by);
        //                                get_data_from_lop[Index2].modified_date = DateTime.Now;

        //                                //_context.tbl_lossofpay_master.Add(get_data_from_lop[Index2]).State = EntityState.Modified;
        //                                _context.Entry(get_data_from_lop[Index2]).State = EntityState.Modified;
        //                            }

        //                            _context.tbl_lossofpay_master.UpdateRange(get_data_from_lop);
        //                           // _context.SaveChanges();
        //                        }

        //                        //Reset Salary Change 
        //                        // var get_data_from_salary_input_chang = _context.tbl_salary_input_change.Where(a => a.emp_id == obj_payroll.payroll_month_year && a.is_active == 1).ToList();
        //                        List<tbl_salary_input_change> get_data_from_salary_input_chang = _context.tbl_salary_input_change.Where(a => a.monthyear == obj_payroll.payroll_month_year.ToString() && a.company_id == obj_payroll.company_id && a.is_active == 1).ToList();
        //                        //if (!(obj_payroll.emp_id == null || obj_payroll.emp_id.Count == 0))
        //                        //{
        //                        //    get_data_from_salary_input_chang.RemoveAll(p => !obj_payroll.emp_id.Contains(p.emp_id ?? 0));
        //                        //}
        //                        if (get_data_from_salary_input_chang.Count > 0)
        //                        {
        //                            for (int Index3 = 0; Index3 < get_data_from_salary_input_chang.Count; Index3++)
        //                            {
        //                                get_data_from_salary_input_chang[Index3].is_active = 0;
        //                                get_data_from_salary_input_chang[Index3].modified_by = Convert.ToInt32(obj_payroll.created_by);
        //                                get_data_from_salary_input_chang[Index3].modified_dt = DateTime.Now;

        //                                // _context.tbl_salary_input_change.Add(get_data_from_salary_input_chang[Index3]).State = EntityState.Modified;
        //                                _context.Entry(get_data_from_lop[Index3]).State = EntityState.Modified;
        //                            }
        //                            _context.tbl_salary_input_change.UpdateRange(get_data_from_salary_input_chang);
        //                            //_context.SaveChanges();
        //                        }


        //                        _context.SaveChanges();
        //                        trans.Commit();

        //                        objResult.StatusCode = 1;
        //                        objResult.Message = "Reset All Data Off " + obj_payroll.payroll_month_year + " Month year...!";
        //                    }
        //                    else
        //                    {
        //                        objResult.StatusCode = 0;
        //                        objResult.Message = "Payroll Is Not Calculated For This " + obj_payroll.payroll_month_year + " Month year...!";
        //                        //return Ok(new { StatusCode = 0, Message = "Payroll Is Not Calculated For This " + obj_payroll.payroll_month_year + " Month year...!" });
        //                    }

        //                   // return Ok(new { StatusCode = 1, Message = "Reset All Data Off " + obj_payroll.payroll_month_year + " Month year...!" });
        //                }


        //                catch (Exception ex)
        //                {
        //                    trans.Rollback();
        //                    objResult.StatusCode = 1;
        //                    objResult.Message = ex.Message;

        //                   // return Ok(ex);
        //                }


        //            }
        //        }

        //        //return Ok(objResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        objResult.StatusCode = 0;
        //        objResult.Message = ex.Message;
        //        //return Ok(objResult);
        //    }

        //    return Ok(objResult);
        //}



#endregion  ******************************CREATED BY AMARJEET, CREATED DATE 27-04-2019, PAYROLL ***********************************


#region ********************** CREATED BY AMARJEET, DATE 17-04-2019 **************************************************


        [Route("GetPayrollProcessedMonthyear/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Payroll))]
        public IActionResult GetPayrollProcessedMonthyear([FromRoute] int company_id)
        {
            ResponseMsg objResult = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objResult.StatusCode = 0;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }

            try
            {


                //get Payroll Processed Month Year
                var final_result = _context.tbl_payroll_process_status.Where(a => a.company_id == company_id && a.is_deleted == 0).Select(b => b.payroll_month_year).OrderBy(p => p).Distinct().ToList();
                if (final_result == null)
                {
                    objResult.Message = "Record Not Found...!";
                    objResult.StatusCode = 0;
                    return Ok(objResult);
                }
                else
                {
                    return Ok(final_result);
                }

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }


        [Route("GetPayrollProcessedMonthyearData/{company_id}/{Monthyear}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.Payroll))]
        public IActionResult GetPayrollProcessedMonthyearData([FromRoute] int company_id, int Monthyear)
        {
            ResponseMsg objResult = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }

            try
            {


                //Get Number of employee procssed in payroll
                var number_of_employee_procssed = _context.tbl_payroll_process_status.Where(a => a.company_id == company_id && a.is_deleted == 0 && a.payroll_month_year == Monthyear).Select(b => b.emp_id).OrderBy(p => p).Distinct().Count();

                //Get All Processed Emp
                var pending_employee_procssed_ = _context.tbl_payroll_process_status.Where(a => a.company_id == company_id && a.is_deleted == 0 && a.payroll_month_year == Monthyear && a.is_calculated == 0).Select(b => b.emp_id).OrderBy(p => p).Distinct().Count();

                //Get Component id for Gross Pay   
                var get_gross_component__ids = _context.tbl_component_master.Where(a => a.component_type == 1 && a.is_active == 1).Select(b => b.component_id).ToList();


                //Get Gross pay
                double gross_pay = _context.tbl_salary_input.Where(a => a.monthyear == Monthyear && a.is_active == 1 && a.company_id == company_id && get_gross_component__ids.Contains(a.component_id ?? 0)).Select(b => Convert.ToDouble(b.values)).Sum();

                //Get Component id for Deductions   
                var get_deductions_component_ids = _context.tbl_component_master.Where(a => a.component_type == 2 && a.is_active == 1).Select(b => b.component_id).ToList();

                //Get Gross pay
                double deductions = _context.tbl_salary_input.Where(a => a.monthyear == Monthyear && a.is_active == 1 && a.company_id == company_id && get_deductions_component_ids.Contains(a.component_id ?? 0)).Select(b => Convert.ToDouble(b.values)).Sum();

                //Get Net Payout
                double net_payout = gross_pay - deductions;

                //Get Days in Month
                string year = Monthyear.ToString().Substring(0, 4);
                string month = Monthyear.ToString().Substring(4, 2);
                string date_ = month + "-" + "01" + "-" + year;

                int days = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));

                return Ok(new { number_of_employee_procssed = number_of_employee_procssed, net_payout = net_payout, gross_pay = gross_pay, deductions = deductions, pending_employee_procssed_ = pending_employee_procssed_, days = days });


            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

#endregion ************************************************************************************************************

        //[HttpGet]
        //[Route("CreatePDF")]
        //public IActionResult CreatePDF()
        //{
        //    var globalSettings = new GlobalSettings
        //    {
        //        DocumentTitle = "PDF Report",
        //        Out = @"D:\PDFCreator\Employee_Report.pdf"
        //    };

        //    var objectSettings = new ObjectSettings
        //    {

        //        HtmlContent = PayslipGenerator.GetHTMLString(),
        //        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "/style_payslip.css") },
        //        //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
        //        //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
        //    };

        //    var pdf = new HtmlToPdfDocument()
        //    {
        //        GlobalSettings = globalSettings,
        //        Objects = { objectSettings }
        //    };

        //    _converter.Convert(pdf);

        //    return Ok("Successfully created PDF document.");
        //}


#region ** START BY SUPRIYA, CREATED DATE 23-05-2019**

        [Route("Save_LoanRequestMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LoanConfiguration2))]
        public IActionResult Save_LoanRequestMaster([FromBody] tbl_loan_request_master obj_loan_req_master)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(obj_loan_req_master.companyid))
            {
                objResult.StatusCode = 0;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }
            if (!_clsCurrentUser.DownlineEmpId.Contains(obj_loan_req_master.created_by))
            {
                objResult.StatusCode = 0;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }
            try
            {
                var exist = _context.tbl_loan_request_master.Where(a => a.companyid == obj_loan_req_master.companyid && a.grade_id == obj_loan_req_master.grade_id && a.loan_type == obj_loan_req_master.loan_type && a.is_deleted == 0).ToList();

                if (exist.Count > 0)
                {
                    objResult.StatusCode = 0;
                    objResult.Message = "Loan Master already exist..";
                    return Ok(objResult);

                }
                else
                {
                    //obj_loan_req_master.created_by = 1;
                    obj_loan_req_master.created_by = _clsCurrentUser.EmpId;
                    obj_loan_req_master.em_status = 0;
                    obj_loan_req_master.is_deleted = 0;
                    obj_loan_req_master.created_dt = DateTime.Now;

                    _context.Entry(obj_loan_req_master).State = EntityState.Added;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Loan Master Save Successfully..";
                    return Ok(objResult);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }
        [Route("Get_GradeMaster")]
        [Authorize(Policy = nameof(enmMenuMaster.Grade))]
        public IActionResult Get_GradeMaster()
        {
            try
            {
                var data = _context.tbl_grade_master.Select(a => new { a.grade_id, a.grade_name }).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }

        }

        [Route("Get_LoanRequestMaster/{s_no}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LoanConfiguration2))]
        public IActionResult Get_LoanRequestMaster([FromRoute] int s_no)
        {
            try
            {
                if (s_no > 0)
                {
                    var data = _context.tbl_loan_request_master.Where(a => a.sno == s_no).ToList();
                    return Ok(data);
                }
                else
                {
                    var dataa = (from a in _context.tbl_loan_request_master
                                 join b in _context.tbl_company_master on a.companyid equals b.company_id into Group
                                 join c in _context.tbl_grade_master on a.grade_id equals c.grade_id
                                 from m in Group
                                 where _clsCurrentUser.CompanyId.Contains(a.companyid)
                                 select new
                                 {
                                     a.sno,
                                     a.loan_type,
                                     a.em_status,
                                     a.loan_amount,
                                     a.rate_of_interest,
                                     a.on_salary,
                                     a.max_tenure,
                                     a.min_top_up_duration,
                                     a.created_dt,
                                     a.last_modified_date,
                                     m.company_name,
                                     c.grade_name,
                                 }).OrderByDescending(g => g.sno).ToList();
                    //AsEnumerable().Select((e, index) => new
                    //{
                    //    sno = e.sno,
                    //    loan_type = e.loan_type,
                    //    em_status = e.em_status,
                    //    loan_amount = e.loan_amount,
                    //    rate_of_interest = e.rate_of_interest,
                    //    on_salary = e.on_salary,
                    //    max_tenure = e.max_tenure,
                    //    min_top_up_duration = e.min_top_up_duration,
                    //    created_dt = e.created_dt,
                    //    last_modified_date = e.last_modified_date,
                    //    company_name = e.company_name,
                    //    grade_name = e.grade_name,
                    //    s_no = index + 1
                    //}).OrderByDescending(g=>g.sno).ToList();

                    return Ok(dataa);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }

        [Route("Edit_LoanRequestMaster")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LoanConfiguration2))]
        public IActionResult Edit_LoanRequestMaster([FromBody] tbl_loan_request_master obj_loan_reqmaster)
        {
            ResponseMsg objresult = new ResponseMsg();

            if (!_clsCurrentUser.CompanyId.Contains(obj_loan_reqmaster.companyid))
            {
                objresult.StatusCode = 1;
                objresult.Message = "Unauthorize Company Access...!!";
                return Ok(objresult);
            }

            if (!_clsCurrentUser.DownlineEmpId.Contains(obj_loan_reqmaster.last_modified_by))
            {
                objresult.StatusCode = 1;
                objresult.Message = "Unauthorize Access...!!";
                return Ok(objresult);
            }
            try
            {
                var exist = _context.tbl_loan_request_master.Where(a => a.sno == obj_loan_reqmaster.sno).FirstOrDefault();

                if (exist != null)
                {

                    var duplicate = _context.tbl_loan_request_master.Where(x => x.sno != obj_loan_reqmaster.sno && x.companyid == obj_loan_reqmaster.companyid && x.grade_id == obj_loan_reqmaster.grade_id && x.loan_type == obj_loan_reqmaster.loan_type && x.is_deleted == 0).FirstOrDefault();
                    if (duplicate != null)
                    {
                        objresult.StatusCode = 1;
                        objresult.Message = "Loan Detail of Selected Grade and Loan Type already exists....!!";
                        return Ok(objresult);
                    }
                    else
                    {
                        exist.em_status = 0;
                        exist.loan_amount = obj_loan_reqmaster.loan_amount;
                        exist.rate_of_interest = obj_loan_reqmaster.rate_of_interest;
                        exist.on_salary = obj_loan_reqmaster.on_salary;
                        exist.max_tenure = obj_loan_reqmaster.max_tenure;
                        exist.min_top_up_duration = obj_loan_reqmaster.min_top_up_duration;
                        exist.grade_id = obj_loan_reqmaster.grade_id;
                        exist.companyid = obj_loan_reqmaster.companyid;
                        exist.last_modified_by = _clsCurrentUser.EmpId;
                        exist.last_modified_date = DateTime.Now;


                        _context.Entry(exist).State = EntityState.Modified;
                        _context.SaveChanges();

                        objresult.StatusCode = 0;
                        objresult.Message = "Loan Master Update Successfully..";
                        return Ok(objresult);
                    }

                    //var exists = false;
                    //var whole_list = _context.tbl_loan_request_master.ToList();
                    //foreach (var item in whole_list)
                    //{
                    //    var comp_id = obj_loan_reqmaster.companyid;
                    //    var grade_idd = obj_loan_reqmaster.grade_id;
                    //    var loan_typee = obj_loan_reqmaster.loan_type;
                    //    if (item.companyid == comp_id && item.grade_id == grade_idd && item.loan_type == loan_typee)
                    //    {
                    //        exists = true;
                    //        break;
                    //    }
                    //}

                    //if (exists)
                    //{
                    //    var check_dup = _context.tbl_loan_request_master.Where(b => b.em_status == obj_loan_reqmaster.em_status && b.loan_amount == obj_loan_reqmaster.loan_amount &&
                    //      b.rate_of_interest == obj_loan_reqmaster.rate_of_interest && b.on_salary == obj_loan_reqmaster.on_salary && b.max_tenure == obj_loan_reqmaster.max_tenure &&
                    //      b.min_top_up_duration == obj_loan_reqmaster.min_top_up_duration).FirstOrDefault();
                    //    if (check_dup != null)
                    //    {
                    //        objresult.StatusCode = 1;
                    //        objresult.Message = "Data already exist";
                    //    }
                    //    else
                    //    {
                    //        exist.loan_type = obj_loan_reqmaster.loan_type;
                    //        exist.em_status = obj_loan_reqmaster.em_status;
                    //        exist.loan_amount = obj_loan_reqmaster.loan_amount;
                    //        exist.rate_of_interest = obj_loan_reqmaster.rate_of_interest;
                    //        exist.on_salary = obj_loan_reqmaster.on_salary;
                    //        exist.max_tenure = obj_loan_reqmaster.max_tenure;
                    //        exist.min_top_up_duration = obj_loan_reqmaster.min_top_up_duration;
                    //        exist.grade_id = obj_loan_reqmaster.grade_id;
                    //        exist.companyid = obj_loan_reqmaster.companyid;
                    //        exist.last_modified_by = obj_loan_reqmaster.last_modified_by;
                    //        exist.last_modified_date = DateTime.Now;

                    //        _context.Entry(exist).State = EntityState.Modified;
                    //        _context.SaveChanges();

                    //        objresult.StatusCode = 0;
                    //        objresult.Message = "Loan Master Update Successfully..";



                    //    }


                    //}
                    //else
                    //{
                    //    exist.loan_type = obj_loan_reqmaster.loan_type;
                    //    exist.em_status = obj_loan_reqmaster.em_status;
                    //    exist.loan_amount = obj_loan_reqmaster.loan_amount;
                    //    exist.rate_of_interest = obj_loan_reqmaster.rate_of_interest;
                    //    exist.on_salary = obj_loan_reqmaster.on_salary;
                    //    exist.max_tenure = obj_loan_reqmaster.max_tenure;
                    //    exist.min_top_up_duration = obj_loan_reqmaster.min_top_up_duration;
                    //    exist.grade_id = obj_loan_reqmaster.grade_id;
                    //    exist.companyid = obj_loan_reqmaster.companyid;
                    //    exist.last_modified_by = obj_loan_reqmaster.last_modified_by;
                    //    exist.last_modified_date = DateTime.Now;

                    //    _context.Entry(exist).State = EntityState.Modified;
                    //    _context.SaveChanges();

                    //    objresult.StatusCode = 0;
                    //    objresult.Message = "Loan Master Update Successfully..";
                    //}


                }
                else
                {
                    objresult.StatusCode = 1;
                    objresult.Message = "ID Not Exists / Invalid Details Please try again..";

                }

                return Ok(objresult);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }

#endregion ** END BY SUPRIYA, CREATED DATE 24-05-2019**
#region ** LOP (LOSS OF PAY) START BY SUPRIYA ON 28-05-2019
        [Route("Get_LOD_Setting/{id}/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.TotalPayrollDaysSetting))]
        public IActionResult Get_LOD_Setting([FromRoute] int id, int company_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            //if (!_clsCurrentUser.CompanyId.Contains(company_id))
            //{
            //    objresponse.StatusCode = 1;
            //    objresponse.Message = "Unauthorize Company Access...!!";
            //    return Ok(objresponse);
            //}


            try
            {


                if (id > 0)
                {
                    var data = _context.tbl_lossofpay_setting.Where(a => a.lop_setting_id == id).FirstOrDefault();
                    return Ok(data);
                }
                else
                {
                    if (company_id > 0)
                    {
                        var dataa = _context.tbl_lossofpay_setting.Join(
                        _context.tbl_company_master,
                        a => a.companyid,
                        b => b.company_id,
                        (a, b) => new
                        {
                            a.lop_setting_id,
                            a.lop_setting_name,
                            a.created_date,
                            a.modified_date,
                            a.is_active,
                            b.company_name,
                            a.companyid
                        }
                       ).AsEnumerable().Select((a, index) => new
                       {

                           lop_setting_id = a.lop_setting_id,
                           lop_setting = a.lop_setting_name,
                           created_date = a.created_date,
                           modified_date = a.modified_date,
                           is_active = a.is_active,
                           company_name = a.company_name,
                           companyid = a.companyid,
                           sno = index + 1

                       }).Where(x => x.companyid == company_id && x.is_active == 1 && _clsCurrentUser.CompanyId.Contains(x.companyid)).ToList();


                        return Ok(dataa);
                    }
                    else
                    {
                        var dataa = _context.tbl_lossofpay_setting.Join(
                        _context.tbl_company_master,
                        a => a.companyid,
                        b => b.company_id,
                        (a, b) => new
                        {
                            a.lop_setting_id,
                            a.lop_setting_name,
                            a.created_date,
                            a.modified_date,
                            a.is_active,
                            b.company_name,
                            a.companyid,
                        }
                       ).AsEnumerable().Select((a, index) => new
                       {

                           lop_setting_id = a.lop_setting_id,
                           lop_setting = a.lop_setting_name,
                           created_date = a.created_date,
                           modified_date = a.modified_date,
                           is_active = a.is_active,
                           company_name = a.company_name,
                           companyid = a.companyid,
                           sno = index + 1

                       }).Where(x => x.is_active == 1 && _clsCurrentUser.CompanyId.Contains(x.companyid)).ToList();

                        return Ok(dataa);
                    }




                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }

        [Route("Save_Lod_Setting")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.TotalPayrollDaysSetting))]
        public IActionResult Save_Lod_Setting([FromBody]tbl_lossofpay_setting objtbl)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(objtbl.companyid))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...";
                return Ok(objresponse);
            }
            try
            {

                var exist_data = _context.tbl_lossofpay_setting.Where(a => a.companyid == objtbl.companyid && a.is_active == 1).FirstOrDefault();
                if (exist_data != null)
                {
                    //objresponse.Message = "LOD Setting Already Saved";
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Monthly Salary Rule already exist";
                }
                else
                {
                    objtbl.is_active = 1;
                    objtbl.created_date = DateTime.Now;
                    _context.Entry(objtbl).State = EntityState.Added;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Monthly Salary Rule successfully saved";
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

        [Route("Edit_Lod_setting")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.TotalPayrollDaysSetting))]
        public IActionResult Edit_Lod_setting([FromBody] tbl_lossofpay_setting objtable)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(objtable.companyid))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }
            try
            {

                var data = _context.tbl_lossofpay_setting.Where(a => a.lop_setting_id == objtable.lop_setting_id).FirstOrDefault();
                if (data != null)
                {
                    if (data.companyid == objtable.companyid)
                    {
                        data.lop_setting_name = objtable.lop_setting_name;
                        //data.companyid = objtable.companyid;
                        data.modified_by = objtable.modified_by;
                        data.modified_date = DateTime.Now;

                        _context.Entry(data).State = EntityState.Modified;
                        _context.SaveChanges();
                        objresponse.StatusCode = 0;
                        //objresponse.Message = "LOD Setting Successfully Updated";
                        objresponse.Message = "Monthly Salary Rule Successfully Updated";
                    }
                    else
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Company Cannot be Changed";
                    }

                }
                else
                {
                    objresponse.StatusCode = 1;
                    //objresponse.Message = "LOD Setting ID not available/ Invalid Details";
                    objresponse.Message = "Monthly Salary Rule ID not available/ Invalid Details";
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

        [Route("Save_Lod_Master")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LOP))]
        public IActionResult Save_Lod_Master([FromBody] tbl_lossofpay_master objtable)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(objtable.company_id ?? 0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access....!!";
                return Ok(objresponse);
            }

            if (!_clsCurrentUser.DownlineEmpId.Contains(objtable.emp_id ?? 0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize access....";
                return Ok(objresponse);
            }

            try
            {
                var exist_data = _context.tbl_lossofpay_master.Where(a => a.company_id == objtable.company_id && a.emp_id == objtable.emp_id && a.monthyear == objtable.monthyear).FirstOrDefault();
                if (exist_data != null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "LOD Master Details Already Saved";
                }
                else
                {
                    objtable.is_active = 1;
                    objtable.created_date = DateTime.Now;

                    _context.Entry(objtable).State = EntityState.Added;
                    _context.SaveChanges();
                    objresponse.StatusCode = 0;
                    objresponse.Message = "LOD Master Details Saved";
                }

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }

        [Route("Get_Lod_Master/{id}/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LOP))]
        public IActionResult Get_Lod_Master([FromRoute] int id, int company_id)
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
                if (id > 0)
                {
                    var data = _context.tbl_lossofpay_master.Where(a => a.company_id == company_id && a.lop_master_id == id).FirstOrDefault();
                    return Ok(data);
                }
                else
                {
                    var Temp_Data = _context.tbl_lossofpay_master.Join(_context.tbl_company_master, a => a.company_id, b => b.company_id, (a, b) => new
                    {
                        a.lop_master_id,
                        a.monthyear,
                        a.emp_id,
                        a.company_id,
                        a.totaldays,
                        a.Holiday_days,
                        a.Week_off_days,
                        a.Present_days,
                        a.Absent_days,
                        a.Leave_days,
                        a.Total_Paid_days,
                        a.Actual_Paid_days,
                        a.Additional_Paid_days,
                        a.acutual_lop_days,
                        a.final_lop_days,
                        a.is_active,
                        a.created_date,
                        a.modified_date,
                        b.company_name
                    }).ToList();

                    var dataa = Temp_Data.Join(_context.tbl_emp_officaial_sec, c => c.emp_id, d => d.employee_id, (c, d) => new
                    {
                        c.lop_master_id,
                        c.monthyear,
                        c.emp_id,
                        c.company_id,
                        c.totaldays,
                        c.Holiday_days,
                        c.Week_off_days,
                        c.Present_days,
                        c.Absent_days,
                        c.Leave_days,
                        c.Total_Paid_days,
                        c.Actual_Paid_days,
                        c.Additional_Paid_days,
                        c.acutual_lop_days,
                        c.final_lop_days,
                        c.is_active,
                        c.created_date,
                        c.modified_date,
                        c.company_name,
                        d.is_deleted,
                        d.employee_first_name,
                        d.employee_middle_name,
                        d.employee_last_name
                    }).Where(c => c.is_active == 1 && c.is_deleted == 0).AsEnumerable().Select((e, index) => new
                    {
                        lop_master_id = e.lop_master_id,
                        monthyear = e.monthyear,
                        employee_id = e.emp_id,
                        company_id = e.company_id,
                        totaldays = e.totaldays,
                        holiday_days = e.Holiday_days,
                        week_off_days = e.Week_off_days,
                        present_days = e.Present_days,
                        absent_days = e.Absent_days,
                        leave_days = e.Leave_days,
                        total_Paid_days = e.Total_Paid_days,
                        actual_Paid_days = e.Actual_Paid_days,
                        additional_Paid_days = e.Additional_Paid_days,
                        acutual_lop_days = e.acutual_lop_days,
                        final_lop_days = e.final_lop_days,
                        e.is_active,
                        created_date = e.created_date,
                        modified_date = e.modified_date,
                        company_name = e.company_name,
                        employee_name = e.employee_first_name + " " + e.employee_middle_name + " " + e.employee_last_name,
                        s_no = index + 1
                    }).ToList();


                    return Ok(dataa);
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("Edit_Lod_Master")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LOP))]
        public IActionResult Edit_Lod_Master([FromBody] tbl_lossofpay_master objtable)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(objtable.company_id ?? 0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company access...!!";
                return Ok(objresponse);
            }

            if (!_clsCurrentUser.DownlineEmpId.Contains(objtable.emp_id ?? 0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company access...!!";
                return Ok(objresponse);
            }
            try
            {
                clsLossPay obj = new clsLossPay(_context);
                objresponse = obj.Update_LOP_Master(objtable);
                if (objresponse.StatusCode == 0)
                {
                    objresponse.Message = "LOD Master Details Successfully Updated";
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "LOD Master ID not available/Invalid Details";
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


        [Route("Get_Lod_MasterByMonthyear/{monthyear}")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LOP))]
        public IActionResult Get_Lod_MasterByMonthyear([FromRoute] int monthyear)
        {
            try
            {
                var Temp_Data = _context.tbl_lossofpay_master.Join(_context.tbl_company_master, a => a.company_id, b => b.company_id, (a, b) => new
                {
                    a.lop_master_id,
                    a.monthyear,
                    a.emp_id,
                    a.company_id,
                    a.totaldays,
                    a.Holiday_days,
                    a.Week_off_days,
                    a.Present_days,
                    a.Absent_days,
                    a.Leave_days,
                    a.Total_Paid_days,
                    a.Actual_Paid_days,
                    a.Additional_Paid_days,
                    a.acutual_lop_days,
                    a.final_lop_days,
                    a.is_active,
                    a.created_date,
                    a.modified_date,
                    b.company_name
                }).ToList();

                var dataa = Temp_Data.Join(_context.tbl_emp_officaial_sec, c => c.emp_id, d => d.employee_id, (c, d) => new
                {
                    c.lop_master_id,
                    c.monthyear,
                    c.emp_id,
                    c.company_id,
                    c.totaldays,
                    c.Holiday_days,
                    c.Week_off_days,
                    c.Present_days,
                    c.Absent_days,
                    c.Leave_days,
                    c.Total_Paid_days,
                    c.Actual_Paid_days,
                    c.Additional_Paid_days,
                    c.acutual_lop_days,
                    c.final_lop_days,
                    c.is_active,
                    c.created_date,
                    c.modified_date,
                    c.company_name,
                    d.is_deleted,
                    d.employee_first_name,
                    d.employee_middle_name,
                    d.employee_last_name
                }).Where(c => c.is_active == 1 && c.is_deleted == 0 && c.monthyear == monthyear).AsEnumerable().Select((e, index) => new
                {
                    lop_master_id = e.lop_master_id,
                    monthyear = e.monthyear,
                    employee_id = e.emp_id,
                    company_id = e.company_id,
                    totaldays = e.totaldays,
                    holiday_days = e.Holiday_days,
                    week_off_days = e.Week_off_days,
                    present_days = e.Present_days,
                    absent_days = e.Absent_days,
                    leave_days = e.Leave_days,
                    total_Paid_days = e.Total_Paid_days,
                    actual_Paid_days = e.Actual_Paid_days,
                    additional_Paid_days = e.Additional_Paid_days,
                    acutual_lop_days = e.acutual_lop_days,
                    final_lop_days = e.final_lop_days,
                    e.is_active,
                    created_date = e.created_date,
                    modified_date = e.modified_date,
                    company_name = e.company_name,
                    employee_name = e.employee_first_name + " " + e.employee_middle_name + " " + e.employee_last_name,
                    s_no = index + 1
                }).ToList();


                return Ok(dataa);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Get_Lod_MasterByMonthyearandCompID/{monthyear}/{company_id}")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LOP))]
        public IActionResult Get_Lod_MasterByMonthyearandCompID([FromRoute] int monthyear, int company_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company access...!!";
                return Ok(objresponse);
            }
            try
            {
                var Temp_Data = _context.tbl_lossofpay_master.Join(_context.tbl_company_master, a => a.company_id, b => b.company_id, (a, b) => new
                {
                    a.lop_master_id,
                    a.monthyear,
                    a.emp_id,
                    a.company_id,
                    a.totaldays,
                    a.Holiday_days,
                    a.Week_off_days,
                    a.Present_days,
                    a.Absent_days,
                    a.Leave_days,
                    a.Total_Paid_days,
                    a.Actual_Paid_days,
                    a.Additional_Paid_days,
                    a.acutual_lop_days,
                    a.final_lop_days,
                    a.is_active,
                    a.created_date,
                    a.modified_date,
                    b.company_name
                }).ToList();

                var data = Temp_Data.Join(_context.tbl_emp_officaial_sec, c => c.emp_id, d => d.employee_id, (c, d) => new
                {
                    c.lop_master_id,
                    c.monthyear,
                    c.emp_id,
                    c.company_id,
                    c.totaldays,
                    c.Holiday_days,
                    c.Week_off_days,
                    c.Present_days,
                    c.Absent_days,
                    c.Leave_days,
                    c.Total_Paid_days,
                    c.Actual_Paid_days,
                    c.Additional_Paid_days,
                    c.acutual_lop_days,
                    c.final_lop_days,
                    c.is_active,
                    c.created_date,
                    c.modified_date,
                    c.company_name,
                    d.is_deleted,
                    d.employee_first_name,
                    d.employee_middle_name,
                    d.employee_last_name
                }).ToList();

                var dataa = data.Where(c => c.is_active == 1 && c.is_deleted == 0 && c.monthyear == monthyear && (_clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.Is_HRAdmin) ? true : c.company_id == company_id).AsEnumerable().Select((e, index) => new
                {
                    lop_master_id = e.lop_master_id,
                    monthyear = e.monthyear,
                    employee_id = e.emp_id,
                    company_id = e.company_id,
                    totaldays = e.totaldays,
                    holiday_days = e.Holiday_days,
                    week_off_days = e.Week_off_days,
                    present_days = e.Present_days,
                    absent_days = e.Absent_days,
                    leave_days = e.Leave_days,
                    total_Paid_days = e.Total_Paid_days,
                    actual_Paid_days = e.Actual_Paid_days,
                    additional_Paid_days = e.Additional_Paid_days,
                    acutual_lop_days = e.acutual_lop_days,
                    final_lop_days = e.final_lop_days,
                    e.is_active,
                    created_date = e.created_date,
                    modified_date = e.modified_date,
                    company_name = e.company_name,
                    employee_name = e.employee_first_name + " " + e.employee_middle_name + " " + e.employee_last_name,
                    s_no = index + 1
                }).ToList();


                return Ok(dataa);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

#endregion ** LOP (LOSS OF PAY) END BY SUPRIYA ON 30-05-2019




#region ** PAYROLL MONTHLY CIRCLE, STARTED BY SUPRIYA 30-05-2019**
        [Route("Save_Payroll_Monthly_Setting")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.PayrollCycleSetting))]
        public IActionResult Save_Payroll_Monthly_Setting([FromBody] tbl_payroll_month_setting objtable)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(objtable.company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }
            try
            {

                var data = _context.tbl_payroll_month_setting.Where(a => a.company_id == objtable.company_id).FirstOrDefault();
                if (data != null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Payroll Monthly Setting Already Saved";
                }
                else
                {
                    objtable.is_deleted = 0;
                    objtable.created_date = DateTime.Now;
                    objtable.last_modified_by = 0;
                    objtable.last_modified_date = new DateTime(2000, 01, 01);

                    _context.Entry(objtable).State = EntityState.Added;
                    _context.SaveChanges();
                    objresponse.StatusCode = 0;
                    objresponse.Message = "Payroll Monthly Setting Saved";
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

        [Route("Get_Payroll_Monthly_Setting/{id}/{companyid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.PayrollCycleSetting))]
        public IActionResult Get_Payroll_Monthly_Setting([FromRoute] int id, int companyid)
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
                if (id > 0)
                {
                    var data = _context.tbl_payroll_month_setting.Where(a => a.company_id == companyid && a.payroll_month_setting_id == id).FirstOrDefault();
                    return Ok(data);
                }
                else
                {
                    if (companyid > 0)
                    {
                        var data = _context.tbl_payroll_month_setting.Join(_context.tbl_company_master, a => a.company_id, b => b.company_id, (a, b) => new
                        {
                            a.payroll_month_setting_id,
                            a.from_month,
                            a.from_date,
                            a.applicable_from_date,
                            a.applicable_to_date,
                            a.created_date,
                            a.last_modified_date,
                            a.is_deleted,
                            a.company_id,
                            b.company_name,
                            b.is_active
                        }).Where(c => c.company_id == companyid && c.is_deleted == 0 && c.is_active == 1 && _clsCurrentUser.CompanyId.Contains(companyid)).AsEnumerable().Select((e, index) => new
                        {

                            payroll_month_setting_id = e.payroll_month_setting_id,
                            //from_month=e.from_month,
                            from_date = e.from_date,
                            applicable_from_date = e.applicable_from_date,
                            applicable_to_date = e.applicable_to_date,
                            created_date = e.created_date,
                            last_modified_date = e.last_modified_date,
                            is_deleted = e.is_deleted,
                            company_id = e.company_id,
                            company_name = e.company_name,
                            sno = index + 1
                        }).ToList();

                        return Ok(data);

                    }
                    else
                    {

                        var data = _context.tbl_payroll_month_setting.Join(_context.tbl_company_master, a => a.company_id, b => b.company_id, (a, b) => new
                        {
                            a.payroll_month_setting_id,
                            a.from_month,
                            a.from_date,
                            a.applicable_from_date,
                            a.applicable_to_date,
                            a.created_date,
                            a.last_modified_date,
                            a.is_deleted,
                            a.company_id,
                            b.company_name,
                            b.is_active
                        }).Where(c => c.is_deleted == 0 && c.is_active == 1 && _clsCurrentUser.CompanyId.Contains(c.company_id)).AsEnumerable().Select((e, index) => new
                        {

                            payroll_month_setting_id = e.payroll_month_setting_id,
                            //from_month=e.from_month,
                            from_date = e.from_date,
                            applicable_from_date = e.applicable_from_date,
                            applicable_to_date = e.applicable_to_date,
                            created_date = e.created_date,
                            last_modified_date = e.last_modified_date,
                            is_deleted = e.is_deleted,
                            company_id = e.company_id,
                            company_name = e.company_name,
                            sno = index + 1
                        }).ToList();
                        return Ok(data);
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

        [Route("Edit_Payroll_Monthly_Setting")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.PayrollCycleSetting))]
        public IActionResult Edit_Payroll_Monthly_Setting([FromBody] tbl_payroll_month_setting objtable)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(objtable.company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }
            try
            {

                var exist_data = _context.tbl_payroll_month_setting.Where(a => a.payroll_month_setting_id == objtable.payroll_month_setting_id).FirstOrDefault();
                if (exist_data != null)
                {
                    exist_data.from_month = objtable.from_month;
                    exist_data.from_date = objtable.from_date;
                    exist_data.applicable_from_date = objtable.applicable_from_date;
                    exist_data.applicable_to_date = objtable.applicable_to_date;
                    exist_data.last_modified_by = objtable.last_modified_by;
                    exist_data.last_modified_date = objtable.last_modified_date;
                    exist_data.company_id = objtable.company_id;

                    exist_data.last_modified_date = DateTime.Now;

                    _context.Entry(exist_data).State = EntityState.Modified;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Payroll Month Circle Successfully Updated";
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Payroll Month Circle ID not Available/Invalid Details";
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

#endregion** PAYROLL MONTHLY CIRCLE, END BY SUPRIYA 31-05-2019**




#region ** REIMBURSMENT APPROVE REJECT SECTION, STARTED BY SUPRIYA ON 01-06-2019
        [Route("Get_ReimRquestDetail/{employee_id}/{companyid}")]
        [HttpGet]
        //[Authorize(Policy = "8093")]
        public IActionResult Get_ReimRquestDetail([FromRoute] int employee_id, int companyid)
        {
            throw new NotImplementedException();
#if false
            try
            {

                var first_final_manager = _context.tbl_emp_manager.OrderByDescending(b => b.emp_mgr_id).Where(a => a.m_one_id == employee_id && a.final_approval == 1 && a.is_deleted == 0).Select(p => p.employee_id).Distinct().ToList();

                var second_final_manager = _context.tbl_emp_manager.OrderByDescending(b => b.emp_mgr_id).Where(a => a.m_two_id == employee_id && a.final_approval == 2 && a.is_deleted == 0).Select(p => p.employee_id).Distinct().ToList();

                var third_final_mangaer = _context.tbl_emp_manager.OrderByDescending(b => b.emp_mgr_id).Where(a => a.m_three_id == employee_id && a.final_approval == 3 && a.is_deleted == 0).Select(p => p.employee_id).Distinct().ToList();

                List<int> emplist = new List<int>();
                for (int i = 0; i < first_final_manager.Count; i++)
                {
                    emplist.Add(Convert.ToInt32(first_final_manager[i]));
                }


                foreach (var item in second_final_manager)
                {
                    bool containitem = emplist.Contains(Convert.ToInt32(item));

                    if (!containitem)
                    {
                        emplist.Add(Convert.ToInt32(item));
                    }
                }

                //var data = _context.tbl_rimb_req_mstr.Where(x => x.is_delete == 0 && (x.tem.tbl_user_master.Where(q => q.default_company_id == (companyid > 0 ? companyid : q.default_company_id)).Count() > 0 && x.emp_id == (employee_id > 0 ? employee_id : x.emp_id))).
                var data = _context.tbl_rimb_req_mstr.OrderByDescending(a => a.rrm_id).Where(a => a.is_delete == 0 && emplist.Contains(Convert.ToInt32(a.emp_id))).
                Select(p => new
                {
                    p.tem.tbl_user_master.First(a => a.is_active == 1).default_company_id,
                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                    p.tem.tbl_employee_company_map.FirstOrDefault(b => b.company_id == b.tbl_company_master.company_id).tbl_company_master.company_name,
                    p.request_type,
                    p.request_month_year,
                    p.remarks,
                    p.fiscal_year_id,
                    p.total_request_amount,
                    p.tem.emp_code,
                    p.created_dt,
                    p.modified_dt,
                    p.is_approvred,
                    p.rrm_id
                }).AsEnumerable().Select((e, index) => new
                {
                    company_name = e.company_name,
                    employee_name = e.employee_first_name + " " + e.employee_middle_name + " " + e.employee_last_name,
                    request_type = e.request_type,
                    fiscal_year_id = e.fiscal_year_id,
                    request_month_year = e.request_month_year,
                    total_request_amount = e.total_request_amount,
                    created_dt = e.created_dt,
                    modified_dt = e.modified_dt,
                    remarks = e.remarks,
                    is_approvred = e.is_approvred,
                    rrm_id = e.rrm_id,
                    sno = index + 1
                }).ToList();

                return Ok(data);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
#endif
        }

        [Route("Edit_ReimbursementRequest")]
        [HttpPost]
        //[Authorize(Policy = "8094")]
        public IActionResult Edit_ReimbursementRequest([FromBody] tbl_rimb_req_mstr objtable)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();
                var _data = _context.tbl_rimb_req_mstr.Where(a => a.rrm_id == objtable.rrm_id).FirstOrDefault();
                if (_data != null)
                {
                    _data.remarks = objtable.remarks;
                    _data.is_approvred = objtable.is_approvred;
                    _data.modified_by = objtable.modified_by;
                    _data.modified_dt = DateTime.Now;

                    _context.Entry(_data).State = EntityState.Modified;
                    _context.SaveChanges();

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Reimbursment Request Successfully Updated";
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Something went wrong,Please try again later";
                }
                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }

        [Route("View_ReimbursementRequestDetail/{rrm_id}")]
        [HttpPost]
        //[Authorize(Policy = "8095")]
        public IActionResult View_ReimbursementRequestDetail([FromRoute] int rrm_id)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();
                if (rrm_id > 0)
                {
                    var data = _context.tbl_rimb_req_details.Where(x => x.rrm_id == rrm_id).Select(p => new
                    {
                        p.rrd_id,
                        p.rrm_id,
                        p.rclm_id,
                        p.Bill_amount,
                        p.Bill_date,
                        p.created_by,
                        p.created_dt,
                        p.modified_by,
                        p.modified_dt,
                        p.is_active,
                        p.is_delete,
                        p.request_amount,
                        p.trclm.rcm_id,
                        p.trclm.trcm.reimbursement_category_name
                    }).AsEnumerable().Select((k, index) => new
                    {
                        rrd_id = k.rrd_id,
                        rrm_id = k.rrm_id,
                        rclm_id = k.rclm_id,
                        bill_amount = k.Bill_amount,
                        bill_date = k.Bill_date,
                        created_by = k.created_by,
                        created_dt = k.created_dt,
                        modified_by = k.modified_by,
                        modified_dt = k.modified_dt,
                        is_active = k.is_active,
                        is_delete = k.is_delete,
                        request_amount = k.request_amount,
                        rcm_id = k.rcm_id,
                        reimbursement_category_name = k.reimbursement_category_name,
                        sno = index + 1
                    }).ToList();

                    return Ok(data);
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "ID / Details are not available...";

                    return Ok(objresponse);
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }

        }






        [Route("Get_ReimRquestDetail_forReport/{employee_id}/{companyid}")]
        [HttpGet]
        //[Authorize(Policy = "8096")]
        public IActionResult Get_ReimRquestDetail_forReport([FromRoute] int employee_id, int companyid)
        {
            try
            {

                var data = _context.tbl_rimb_req_mstr.Where(x => x.tem.tbl_user_master.Where(q => q.default_company_id == (companyid > 0 ? companyid : 0)).Count() > 0 && x.emp_id == (employee_id > 0 ? employee_id : x.emp_id)).
                     Select(p => new
                     {
                         p.tem.tbl_user_master.First(a => a.is_active == 1).default_company_id,
                         p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                         p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                         p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                         p.tem.tbl_employee_company_map.FirstOrDefault(b => b.company_id == b.tbl_company_master.company_id).tbl_company_master.company_name,
                         p.request_type,
                         p.request_month_year,
                         p.remarks,
                         p.fiscal_year_id,
                         p.total_request_amount,
                         p.tem.emp_code,
                         p.created_dt,
                         p.modified_dt,
                         p.is_approvred,
                         p.rrm_id,
                         p.is_delete
                     }).AsEnumerable().Select((e, index) => new
                     {
                         company_name = e.company_name,
                         employee_name = e.employee_first_name + " " + e.employee_middle_name + " " + e.employee_last_name,
                         request_type = e.request_type,
                         fiscal_year_id = e.fiscal_year_id,
                         request_month_year = e.request_month_year,
                         total_request_amount = e.total_request_amount,
                         created_dt = e.created_dt,
                         modified_dt = e.modified_dt,
                         remarks = e.remarks,
                         is_approvred = e.is_approvred,
                         rrm_id = e.rrm_id,
                         is_delete = e.is_delete,
                         sno = index + 1
                     }).ToList();

                return Ok(data);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }

#endregion ** REIMBURSMENT APPROVE REJECT SECTION, END BY SUPRIYA ON 03-06-2019






#region ** FORMULA COPY, STARTED BY SUPRIYA ON 04-06-2019
        [Route("Get_FromulaComponent/{companyid}/{salary_group_id}")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.SalaryGroupAlignment))]
        public IActionResult Get_FromulaComponent([FromRoute] int companyid, int salary_group_id)
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
                var data = _context.tbl_component_formula_details.Where(a => a.company_id == companyid && a.salary_group_id == salary_group_id).Select(k => new
                {
                    sno = k.sno,
                    component_id = k.component_id,
                    salary_group_id = k.salary_group_id,
                    component_type = 0,
                    formula = k.formula,
                    function_calling_order = k.function_calling_order,
                    created_dt = k.created_dt,
                    modified_dt = k.created_by,
                    company_id = k.company_id,
                    is_salary_comp = 0,
                    is_data_entry_comp = 0,
                    is_tds_comp = 0,
                    payment_type = 0,
                    is_user_interface = 0,
                    is_payslip = 0,
                    property_details = k.comp_master.property_details
                }).ToList();


                if (data.Count > 0)
                {
                    return Ok(data);
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Formula's component not available";

                    return Ok(objresponse);
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }


        }

        [Route("Copied_Component_Fromula")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.SalaryGroupAlignment))]
        public IActionResult Copied_Component_Fromula([FromBody] Component_property_master objmodel)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Any(p => objmodel.component_mstr.Any(q => q.company_id == p)))
            {
                objresponse.StatusCode = 3;
                objresponse.Message = "Unauthorize Company access....!!";
                return Ok(objresponse);
            }
            try
            {
                var all_data = objmodel.component_mstr;


                if (objmodel.component_mstr.Count > 0)
                {
                    for (int i = 0; i < all_data.Count; i++)
                    {
                        var salarygroupid = all_data[i].salary_group_id;
                        var companyid = all_data[i].company_id;
                        var formulaa = all_data[i].formula;
                        var exist_data = _context.tbl_component_formula_details.Where(a => a.company_id == companyid && a.salary_group_id == salarygroupid && a.formula == formulaa).FirstOrDefault();
                        if (exist_data != null)
                        {
                            objresponse.StatusCode = 1;
                            objresponse.Message = "Already Copied, Do you want to remove the previous data";
                            break;
                        }
                    }
                    if (objresponse.StatusCode != 1)
                    {
                        for (int j = 0; j < all_data.Count; j++)
                        {
                            tbl_component_formula_details objcomp = new tbl_component_formula_details();

                            objcomp.component_id = all_data[j].component_id;
                            objcomp.company_id = all_data[j].company_id;
                            objcomp.salary_group_id = all_data[j].salary_group_id;
                            objcomp.formula = all_data[j].formula;
                            objcomp.function_calling_order = all_data[j].function_calling_order;
                            objcomp.created_by = all_data[j].created_by;
                            objcomp.created_dt = DateTime.Now;
                            objcomp.is_deleted = 0;
                            objcomp.deleted_by = all_data[j].created_by;
                            objcomp.deleted_dt = DateTime.Now;


                            _context.Entry(objcomp).State = EntityState.Added;
                            _context.SaveChanges();
                        }

                        objresponse.StatusCode = 0;
                        objresponse.Message = "Formula Successfully Copied/Inserted into another Salary Group";
                    }

                }
                else
                {
                    objresponse.StatusCode = 2;
                    objresponse.Message = "Something went Wrong,Please try after some Time";
                }


                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 3;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("Delete_component_Property_Detail")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.SalaryGroupAlignment))]
        public IActionResult Delete_component_Property_Detail([FromBody] Component_property_master objmodel)
        {
            Response_Msg objresponse = new Response_Msg();
            objresponse.StatusCode = 1;
            objresponse.Message = "Property can not be deleted";
            return Ok(objresponse);
            //try
            //{
            //    var all_data = objmodel.component_mstr;
            //    Response_Msg objresponse = new Response_Msg();
            //    if (all_data.Count > 0)
            //    {
            //        var _isdeleted = 0;

            //        var exist_data = _context.tbl_comproperty_dtl_log.Where(a => a.salary_group_id == objmodel.component_mstr[0].salary_group_id && a.company_id == objmodel.component_mstr[0].company_id).OrderByDescending(p => p.is_deleted).Select(p => new { p.is_deleted }).FirstOrDefault();

            //        if (exist_data != null)
            //        {
            //            int total_exists_data = exist_data.is_deleted;
            //            _isdeleted = total_exists_data + 1;
            //        }
            //        else
            //        {
            //            _isdeleted = 1;
            //        }

            //        for (int j = 0; j < all_data.Count; j++)
            //        {
            //            tbl_comproperty_dtl_log objcomp = new tbl_comproperty_dtl_log();

            //            objcomp.component_id = all_data[j].component_id;
            //            objcomp.company_id = all_data[j].company_id;
            //            objcomp.salary_group_id = all_data[j].salary_group_id;
            //            objcomp.component_type = all_data[j].component_type;
            //            objcomp.is_salary_comp = all_data[j].is_salary_comp;
            //            objcomp.is_data_entry_comp = all_data[j].is_data_entry_comp;
            //            objcomp.is_tds_comp = all_data[j].is_tds_comp;
            //            objcomp.payment_type = all_data[j].payment_type;
            //            objcomp.formula = all_data[j].formula;
            //            objcomp.function_calling_order = all_data[j].function_calling_order;
            //            objcomp.is_user_interface = all_data[j].is_user_interface;
            //            objcomp.component_property_id = all_data[j].sno;
            //            objcomp.created_by = all_data[j].created_by;
            //            objcomp.created_dt = DateTime.Now;
            //            objcomp.is_payslip = all_data[j].is_payslip;
            //            objcomp.is_active = 1;
            //            objcomp.is_deleted = _isdeleted;


            //            #region **Save in Backup table of Componet property Detail
            //            _context.Entry(objcomp).State = EntityState.Added;
            //            _context.SaveChanges();
            //            #endregion

            //            #region ** Delete from tbl_componet Detail


            //            tbl_component_formula_details objcomp_prop = new tbl_component_formula_details();
            //            objcomp_prop.sno = all_data[j].sno;



            //            _context.Entry(objcomp_prop).State = EntityState.Deleted;
            //            _context.SaveChanges();
            //            #endregion
            //        }

            //        objresponse.StatusCode = 0;
            //        objresponse.Message = "Previous Record Successfully Deleted";
            //    }
            //    else
            //    {
            //        objresponse.StatusCode = 1;
            //        objresponse.Message = "Something went wrong,while Deleting Previous Data";
            //    }

            //    return Ok(objresponse);
            //}
            //catch (Exception ex)
            //{
            //    return Ok(ex.Message.ToString());
            //}
        }


#endregion** FORMULA COPY, END BY SUPRIYA ON 04-06-2019

#region ** SALARY INPUT REPORT, STARTED BY SUPRIYA ON 06-06-2019

        [Route("Get_Salary_input_Report")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.PayrollReport))]
        public IActionResult Get_Salary_input_Report()
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var salarydata = _context.tbl_salary_input.Where(x => x.is_active == 1).
                  Select(p => new
                  {
                      p.tem.tbl_user_master.First(a => a.is_active == 1).default_company_id,
                      p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                      p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                      p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                      p.salary_input_id,
                      p.component_id,
                      p.monthyear,
                      p.emp_id,
                      p.values,
                      p.tem.emp_code,
                      p.created_dt,
                      p.modified_dt,
                      p.is_active
                  }).AsEnumerable().Select((e, index) => new
                  {
                      employee_name = e.employee_first_name + " " + e.employee_middle_name + " " + e.employee_last_name + "(" + e.emp_code + ")",
                      salary_input_id = e.salary_input_id,
                      component_id = e.component_id,
                      monthyear = e.monthyear,
                      emp_id = e.emp_id,
                      valuess = e.values,
                      created_dt = e.created_dt,
                      modified_dt = e.modified_dt,
                      e.is_active,
                      sno = index + 1
                  }).ToList();


                var EmpIDData = salarydata.Select(p => new { p.emp_id, p.employee_name, p.monthyear }).Distinct().ToList();


                var _componentdata = _context.tbl_component_master.Where(x => x.is_active == 1).
                    Select(p => new
                    {
                        p.property_details,
                        p.component_id
                    }).ToList();


                var _componentdata_ = _componentdata.ToList();
                var _finaldata = new { salarydata, _componentdata_, EmpIDData };
                return Ok(_finaldata);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }


        [Route("Get_Salary_input_Report_by_monthyear/{monthyear}/{rptl_id}")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.PayrollReport))]
        public IActionResult Get_Salary_input_Report_by_monthyear([FromRoute] int monthyear, int rptl_id)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                List<tbl_salary_input> objlist = new List<tbl_salary_input>();

                var salary_input_changedata = _context.tbl_salary_input_change.Where(x => x.is_active == 1 && Convert.ToInt32(x.monthyear) == monthyear && _clsCurrentUser.DownlineEmpId.Contains(x.emp_id ?? 0) && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).
                 Select(p => new
                 {
                     p.tem.tbl_user_master.First(a => a.is_active == 1).default_company_id,
                     p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                     p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                     p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                     p.salary_input_id,
                     p.component_id,
                     p.monthyear,
                     p.emp_id,
                     p.values,
                     p.tem.emp_code,
                     p.created_dt,
                     p.modified_dt,
                     p.is_active
                 }).AsEnumerable().Select((e, index) => new
                 {
                     employee_name = e.employee_first_name + " " + e.employee_middle_name + " " + e.employee_last_name + "(" + e.emp_code + ")",
                     salary_input_id = e.salary_input_id,
                     component_id = e.component_id,
                     monthyear = e.monthyear,
                     emp_id = e.emp_id,
                     valuess = e.values,
                     created_dt = e.created_dt,
                     modified_dt = e.modified_dt,
                     e.is_active,
                     sno = index + 1
                 }).ToList();


                var salarydata = _context.tbl_salary_input.Where(x => x.is_active == 1 && x.monthyear == monthyear && _clsCurrentUser.DownlineEmpId.Contains(x.emp_id ?? 0) && _clsCurrentUser.CompanyId.Contains(x.company_id ?? 0)).
                  Select(p => new
                  {
                      p.tem.tbl_user_master.First(a => a.is_active == 1).default_company_id,
                      p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).employee_first_name,
                      p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).employee_middle_name,
                      p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).employee_last_name,
                      p.salary_input_id,
                      p.component_id,
                      p.monthyear,
                      p.emp_id,
                      p.values,
                      p.tem.emp_code,
                      p.created_dt,
                      p.modified_dt,
                      p.is_active,

                  }).AsEnumerable().Select((e, index) => new
                  {
                      e.emp_code,
                      employee_name = e.employee_first_name + " " + e.employee_middle_name + " " + e.employee_last_name + "(" + e.emp_code + ")",
                      salary_input_id = e.salary_input_id,
                      component_id = e.component_id,
                      monthyear = e.monthyear,
                      emp_id = e.emp_id,
                      valuess = salary_input_changedata.Any(a => a.emp_id == e.emp_id && a.component_id == e.component_id) ? salary_input_changedata.FirstOrDefault(a => a.emp_id == e.emp_id && a.component_id == e.component_id).valuess : e.values,
                      created_dt = e.created_dt,
                      modified_dt = e.modified_dt,
                      e.is_active,
                      sno = index + 1
                  }).ToList();

                var EmpIDData = salarydata.Select(p => new { p.emp_id, p.employee_name, p.monthyear, p.emp_code }).Distinct().ToList();

                var _componentdata = _context.tbl_rpt_title_master.Where(x => x.is_active == 1 && x.rpt_id == rptl_id && x.tbl_Component_Master.is_system_key == 0 && x.tbl_Component_Master.is_active == 1).Select(p => new
                {
                    property_details = p.rpt_title,
                    p.component_id
                }).ToList();

                //var _componentdata = _context.tbl_component_master.Where(x => x.is_active == 1 && x.is_system_key==0 ).
                //    Select(p => new
                //    {
                //        p.property_details,
                //        p.component_id
                //    }).ToList();


                var _componentdata_ = _componentdata.ToList();
                var _finaldata = new { salarydata, _componentdata_, EmpIDData };
                return Ok(_finaldata);

                //var exists = _context.tbl_salary_input.Where(x => x.is_active == 1 && x.monthyear == monthyear).FirstOrDefault();
                //if (exists != null)
                //{


                //    var EmpIDData = salarydata.Select(p => new { p.emp_id, p.employee_name, p.monthyear }).Distinct().ToList();
                //    var _componentdata = _context.tbl_component_master.Where(x => x.is_active == 1).
                //        Select(p => new
                //        {
                //            p.property_details,
                //            p.component_id
                //        }).ToList();


                //    var _componentdata_ = _componentdata.ToList();
                //    var _finaldata = new { salarydata, _componentdata_, EmpIDData };
                //    return Ok(_finaldata);
                //}
                //else
                //{



                //}




            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }

#endregion ** SALARY INPUT REPORT, END BY SUPRIYA ON 06-06-2019


#region ** UPDATE LOAN APPROVAL SETTING, CREATED BY SUPRIYA ON 12-06-2019

        [Route("Update_LoanApprovalSetting")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LoanApprovalSettingMaster))]


        public IActionResult Update_LoanApprovalSetting([FromBody] tbl_loan_approval_setting objtable)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(objtable.company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Comapny Access...!!";
                return Ok(objresponse);
            }

            if (!_clsCurrentUser.DownlineEmpId.Contains(objtable.emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access....";
                return Ok(objresponse);
            }

            try
            {
                var exist = _context.tbl_loan_approval_setting.Where(x => x.Sno == objtable.Sno).FirstOrDefault();
                if (exist != null)
                {
                    //make it as final approver
                    if (objtable.is_final_approver == 1)
                    {
                        var final_approver_exist = _context.tbl_loan_approval_setting.Where(x => x.company_id == objtable.company_id && x.emp_id != objtable.emp_id && x.approver_type == objtable.approver_type && x.is_final_approver == 1 && x.is_active == 1).FirstOrDefault();
                        if (final_approver_exist != null)
                        {
                            objresponse.StatusCode = 1;
                            objresponse.Message = "More than one Final Approver are not exist in company";
                        }
                        else
                        {
                            //check approver type exist or not
                            var exist_approver_type = _context.tbl_loan_approval_setting.Where(x => x.company_id == objtable.company_id && x.approver_type == objtable.approver_type && x.order == objtable.order && x.emp_id != objtable.emp_id).FirstOrDefault();
                            //same level of approver  exist
                            if (exist_approver_type != null)
                            {
                                objresponse.StatusCode = 1;
                                objresponse.Message = "Selected level of approver already exist..";
                            }
                            else
                            {
                                exist.is_final_approver = objtable.is_final_approver;
                                exist.is_active = objtable.is_active;
                                exist.last_modified_by = objtable.last_modified_by;
                                exist.last_modified_date = DateTime.Now;

                                _context.Entry(exist).State = EntityState.Modified;
                                _context.SaveChanges();

                                objresponse.StatusCode = 0;
                                objresponse.Message = "Loan Approval Setting Successfully Updated";
                            }
                        }
                    }
                    else
                    {
                        //check approver type exist or not
                        var exist_approver_type = _context.tbl_loan_approval_setting.Where(x => x.company_id == objtable.company_id && x.approver_type == objtable.approver_type && x.order == objtable.order && x.emp_id != objtable.emp_id).FirstOrDefault();
                        //same level of approver  exist
                        if (exist_approver_type != null)
                        {
                            objresponse.StatusCode = 1;
                            objresponse.Message = "Selected level of approver already exist..";
                        }
                        else
                        {
                            exist.is_final_approver = objtable.is_final_approver;
                            exist.is_active = objtable.is_active;
                            exist.last_modified_by = objtable.last_modified_by;
                            exist.last_modified_date = DateTime.Now;

                            _context.Entry(exist).State = EntityState.Modified;
                            _context.SaveChanges();

                            objresponse.StatusCode = 0;
                            objresponse.Message = "Loan Approval Setting Successfully Updated";
                        }
                    }

                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Something went wrong,Invalid Details";
                }
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
            }
            return Ok(objresponse);
        }
        //public IActionResult Update_LoanApprovalSetting([FromBody] tbl_loan_approval_setting objtable)
        //{
        //    try
        //    {
        //        Response_Msg objresponse = new Response_Msg();

        //        var exist_data = _context.tbl_loan_approval_setting.Where(x => x.Sno == objtable.Sno).FirstOrDefault();
        //        if (exist_data != null)
        //        {
        //            //var _final_approver_exist = false;
        //            if (objtable.is_final_approver == 1)
        //            {
        //                var check_final_approver = _context.tbl_loan_approval_setting.Where(x => x.company_id == objtable.company_id && x.is_final_approver == 1 && x.approver_type == objtable.approver_type).FirstOrDefault();
        //                if (check_final_approver != null)
        //                {
        //                    objresponse.StatusCode = 1;
        //                    objresponse.Message = "More than one final Approver are not exist in one company";
        //                }
        //                else
        //                {
        //                    var exist_approverand_order = _context.tbl_loan_approval_setting.Where(x =>x.company_id==objtable.company_id && x.approver_type == objtable.approver_type && x.order == objtable.order && x.is_active==1).FirstOrDefault();

        //                    if (exist_approverand_order != null)
        //                    {
        //                        objresponse.StatusCode = 1;
        //                        objresponse.Message = "This level of approver order already exist,Please select another one..";
        //                    }
        //                    else
        //                    {
        //                        exist_data.is_final_approver = objtable.is_final_approver;
        //                        exist_data.is_active = objtable.is_active;
        //                        exist_data.last_modified_by = objtable.last_modified_by;
        //                        exist_data.last_modified_date = DateTime.Now;

        //                        _context.Entry(exist_data).State = EntityState.Modified;
        //                        _context.SaveChanges();

        //                        objresponse.StatusCode = 0;
        //                        objresponse.Message = "Loan Approval Setting Successfully Updated";
        //                    }



        //                }
        //            }
        //            else
        //            {
        //                var exist_approverand_order = _context.tbl_loan_approval_setting.Where(x => x.company_id == objtable.company_id && x.approver_type == objtable.approver_type && x.order == objtable.order && x.is_active == 1).FirstOrDefault();

        //                if (exist_approverand_order != null)
        //                {
        //                    objresponse.StatusCode = 1;
        //                    objresponse.Message = "This level of approver order already exist,Please select another one..";
        //                }
        //                else
        //                {
        //                    exist_data.is_final_approver = objtable.is_final_approver;
        //                    exist_data.is_active = objtable.is_active;
        //                    exist_data.last_modified_by = objtable.last_modified_by;
        //                    exist_data.last_modified_date = DateTime.Now;

        //                    _context.Entry(exist_data).State = EntityState.Modified;
        //                    _context.SaveChanges();

        //                    objresponse.StatusCode = 0;
        //                    objresponse.Message = "Loan Approval Setting Successfully Updated";
        //                }



        //            }

        //        }
        //        else
        //        {
        //            objresponse.StatusCode = 1;
        //            objresponse.Message = "Some thing went wrong, while updating Loan Approval Setting";
        //        }

        //        return Ok(objresponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message.ToString());
        //    }
        //}

#endregion ** UPDATE LOAN APPROVAL SETTING, END BY SUPRIYA ON 12-06-2019


#region ** CANCEL REIMBURSMENT REQUEST, STARTED BY SUPRIYA ON 13-06-2019
        [Route("Cancel_ReimbursementRequest")]
        [HttpPost]
        //[Authorize(Policy = "8103")]
        public IActionResult Cancel_ReimbursementRequest([FromBody] Category_Reimbursment_request objrr)
        {
            using (var db = _context.Database.BeginTransaction())
            {
                try
                {
                    Response_Msg objresponse = new Response_Msg();
                    var exists_data = _context.tbl_rimb_req_mstr.Where(a => a.rrm_id == objrr.req_type).FirstOrDefault();
                    if (exists_data != null)
                    {
#region ** Set Is Deleted 0 in Master Table **
                        //  tbl_rimb_req_mstr objrrm = new tbl_rimb_req_mstr();

                        exists_data.is_delete = objrr.is_deleted;
                        exists_data.remarks = objrr.remarks;
                        exists_data.modified_by = objrr.modified_by;
                        exists_data.modified_dt = DateTime.Now;

                        _context.Entry(exists_data).State = EntityState.Modified;
                        _context.SaveChanges();
#endregion

#region ** Set Is Deleted 0 in Detail Table **
                        var rrd_data = _context.tbl_rimb_req_details.Where(b => b.rrm_id == objrr.req_type).ToList();

                        if (rrd_data.Count > 0)
                        {
                            foreach (var item in rrd_data)
                            {
                                tbl_rimb_req_details objdtl = new tbl_rimb_req_details();

                                item.is_delete = objrr.is_deleted;
                                item.modified_by = objrr.modified_by;
                                item.modified_dt = DateTime.Now;

                                _context.Entry(item).State = EntityState.Modified;

                            }
                            _context.SaveChanges();
                        }

#endregion
                        db.Commit();

                        objresponse.StatusCode = 0;
                        objresponse.Message = "Reimbursment Request successfully Cancel";
                    }


                    else
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Request Id not available..";
                    }
                    return Ok(objresponse);
                }
                catch (Exception ex)
                {
                    return Ok(ex.Message.ToString());
                }
            }

        }

#endregion ** CANCEL REIMBURSMENT REQUEST,END BY SUPRIYA ON 13-06-2019

#region **STARTED BY SUPRIYA ON 14-06-2019 **

        [Route("GetLoanRequestMasterByEmpandGrade/{emp_id}/{loan_type}/{company_id}")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.LoanRequest))]
        public IActionResult GetLoanRequestMasterByEmpandGrade([FromRoute] int emp_id, int loan_type, int company_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company access...!!";
                return Ok(objresponse);
            }

            if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company access...!!";
                return Ok(objresponse);
            }
            try
            {
                var empgrade = _context.tbl_emp_grade_allocation.Where(x => x.employee_id == emp_id && x.applicable_from_date.Date <= DateTime.Now.Date && DateTime.Now.Date <= x.applicable_to_date.Date).GroupBy(x => x.employee_id).Select(p => p.OrderByDescending(q => q.emp_grade_id).FirstOrDefault()).FirstOrDefault();
                if (empgrade != null)
                {
                    var data = _context.tbl_loan_request_master.Where(x => x.is_deleted == 0 && x.companyid == company_id && x.loan_type == loan_type && x.grade_id == empgrade.grade_id).Select(p => new
                    {
                        p.grade_id,
                        p.loan_type,
                        p.loan_amount,
                        p.rate_of_interest,
                        p.max_tenure,

                    }).FirstOrDefault();
                    return Ok(data);
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Loan request cannot be raised untill Loan Details are not available for Your Grade...!!";
                    return Ok(objresponse);
                }

                //var data = _context.tbl_loan_request_master.Join(_context.tbl_emp_grade_allocation, a => a.grade_id, b => b.grade_id, (a, b) => new
                //{
                //    b.grade_id,
                //    b.employee_id,
                //    a.loan_type,
                //    a.loan_amount,
                //    a.rate_of_interest,
                //    a.max_tenure,
                //    a.is_deleted,
                //    b.emp_grade_id,
                //    a.companyid
                //}).Where(c => c.loan_type == loan_type && c.employee_id == emp_id && c.is_deleted == 0 && c.companyid==company_id).OrderByDescending(x => x.emp_grade_id).Take(1).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }

#endregion ** END BY SUPRIIYA ON 14-06-2019 **


#region ** SALARY SLIP, START BY SUPRIYA ON 20-06-2019 **

        [Route("Get_SalarySlip/{companyid}/{employeeid}")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.SalarySlip))]
        public IActionResult Get_SalarySlip([FromRoute] int companyid, int employeeid)
        {

            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.DownlineEmpId.Any(p => p == employeeid))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!";
                return Ok(objResult);
            }

            try
            {

                if (_clsCurrentUser.Is_Hod == 2)
                {
                    if (companyid > 0)
                    {

                        var data = _context.tbl_payroll_process_status.Where(a => a.company_id == companyid && a.is_calculated == 1 && a.is_deleted == 0).Select(b => new
                        {

                            b.pay_pro_status_id,
                            b.company_id,
                            b.payroll_month_year,
                            b.emp_id,
                            emp_name = string.Format("{0} {1} {2}",
                                        b.tbl_emp.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                                        b.tbl_emp.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                                        b.tbl_emp.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name),
                            emp_code = b.tbl_emp.tbl_user_master.FirstOrDefault(q => q.is_active == 1).tbl_employee_id_details.emp_code,
                            b.is_calculated,
                            b.is_freezed,
                            b.is_lock,
                            b.payroll_status,
                            b.created_by,
                            b.created_date,
                            b.last_modified_by,
                            b.last_modified_date,
                            b.payslip_path
                        }).ToList();

                        return Ok(data);

                    }
                    else
                    {
                        var data = _context.tbl_payroll_process_status.Where(a => a.is_lock == 1 && a.is_deleted == 0).Select(b => new
                        {

                            b.pay_pro_status_id,
                            b.company_id,
                            b.payroll_month_year,
                            b.emp_id,
                            emp_name_code = string.Format("{0} {1} {2} ({3})",
                                            b.tbl_emp.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                                            b.tbl_emp.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                                            b.tbl_emp.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                                           b.tbl_emp.tbl_user_master.FirstOrDefault(q => q.is_active == 1).tbl_employee_id_details.emp_code),
                            b.is_calculated,
                            b.is_freezed,
                            b.is_lock,
                            b.payroll_status,
                            b.created_by,
                            b.created_date,
                            b.last_modified_by,
                            b.last_modified_date,
                            b.payslip_path
                        }).ToList();

                        return Ok(data);
                    }
                }
                else
                {
                    var data = _context.tbl_payroll_process_status.Where(a => a.company_id == companyid && a.emp_id == _clsCurrentUser.EmpId && a.is_lock == 1 && a.is_deleted == 0).Select(b => new
                    {

                        b.pay_pro_status_id,
                        b.company_id,
                        b.payroll_month_year,
                        b.emp_id,
                        emp_name_code = string.Format("{0} {1} {2} ({3})",
                             b.tbl_emp.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                             b.tbl_emp.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                             b.tbl_emp.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                             b.tbl_emp.tbl_user_master.FirstOrDefault(q => q.is_active == 1).tbl_employee_id_details.emp_code),
                        b.is_calculated,
                        b.is_freezed,
                        b.is_lock,
                        b.payroll_status,
                        b.created_by,
                        b.created_date,
                        b.last_modified_by,
                        b.last_modified_date,
                        b.payslip_path
                    }).ToList();

                    return Ok(data);
                }


            }
            catch (Exception ex)
            {
                objResult.StatusCode = 1;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }








            //Classes.clsLoginEmpDtl ob = new clsLoginEmpDtl(_context, Convert.ToInt32(employeeid), "Wite", _AC);
            //if (!ob.is_valid())
            //{
            //    objResult.StatusCode = 1;
            //    objResult.Message = "Unauthorize Access...!";
            //    return Ok(objResult);
            //}
        }

        [HttpGet]
        [Route("View_Employee_SalarySlip/{employeeid}/{payrollmonthyear}")]
        //[Authorize(Policy = "8106")]
        public IActionResult View_Employee_SalarySlip([FromRoute] int employeeid, int payrollmonthyear)
        {
            try
            {
                Response_Msg objResult = new Response_Msg();
                Classes.clsLoginEmpDtl ob = new clsLoginEmpDtl(_context, Convert.ToInt32(employeeid), "read", _AC);
                if (!ob.is_valid())
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access...!";
                    return Ok(objResult);
                }

                var _salaryslip_path = "";

                var data = _context.tbl_payroll_process_status.Where(x => x.emp_id == employeeid && x.payroll_month_year == payrollmonthyear).FirstOrDefault();

                string domain_url = _appSettings.Value.domain_url;

                if (data != null)
                {
                    var webRoot = _hostingEnvironment.WebRootPath;

                    _salaryslip_path = string.Format("{0}{1}", domain_url, data.payslip_path);

                }
                var result = new { path = _salaryslip_path };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }
#endregion ** SALARY SLIP, END BY SUPRIYA ON 20-06-2019 **

        [Route("Get_Pending_Loan_Request/{company_id}/{login_emp_id}/{login_emp_role_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LoanApproval))]
        public IActionResult Get_Pending_Loan_Request([FromRoute] int company_id, int login_emp_id, int login_emp_role_id)
        {
            ResponseMsg objrepsonse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objrepsonse.StatusCode = 1;
                objrepsonse.Message = "Unauthorize Company Access...!!";
                return Ok(objrepsonse);
            }

            if (!_clsCurrentUser.DownlineEmpId.Contains(login_emp_id))
            {
                objrepsonse.StatusCode = 1;
                objrepsonse.Message = "Unauthoriz Access...!!";
                return Ok(objrepsonse);
            }
            try
            {
                var order = _context.tbl_loan_approval_setting.Where(x => x.is_active == 1 && x.approver_type == 1 && x.company_id == company_id && (x.emp_id == _clsCurrentUser.EmpId || _clsCurrentUser.RoleId.Contains(x.approver_role_id ?? 0))).FirstOrDefault();
                if (order != null)
                {
                    var result = _context.tbl_loan_approval.Where(x => x.is_deleted == 0 && x.is_approve == 0 && x.is_final_approver != 1 && x.is_final_approver != 2 && x.approval_order == order.order && x.tbl_loan_request.is_closed == 0).Select(p => new LoanRequest
                    {
                        loan_req_id = Convert.ToInt32(p.loan_req_id),
                        emp_id = Convert.ToInt32(p.emp_id),
                        approval_order = p.approval_order,
                        approver_emp_id = order.emp_id,
                        is_deleted = p.is_deleted,
                        is_closed = p.tbl_loan_request.is_closed,
                        is_final_approver = p.is_final_approver,
                        is_approve = p.is_approve,
                        emp_code = string.Format("{0} {1} {2} ({3})", p.tbl_employee_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                      p.tbl_employee_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                      p.tbl_employee_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                      p.tbl_employee_master.emp_code),
                        //company_id = p.tbl_loan_requestcompany_id,
                        // approver_type = p.tbl_loan_request.;
                        approver_role_id = p.approver_role_id,

                    }).ToList();

                    return Ok(result);
                }
                else
                {
                    objrepsonse.StatusCode = 1;
                    objrepsonse.Message = "You are not approver of any Loan/Advance Request";
                    return Ok(objrepsonse);
                }
                //var result = _context.tbl_loan_approval.Join(_context.tbl_loan_approval_setting, a => a.approval_order, b => b.order, (a, b) => new
                //{

                //    a.loan_req_id,
                //    a.emp_id,
                //    a.approval_order,
                //    approver_id = b.emp_id,
                //    a.tbl_loan_request.is_deleted,
                //    a.tbl_loan_request.is_closed,
                //    a.is_final_approver,
                //    a.is_approve,
                //    a.tbl_loan_request.emp_code,
                //    b.company_id,
                //    b.approver_type,
                //    a.approver_role_id,
                //    approver_active=b.is_active
                //}).Where(x => x.company_id == company_id && x.approver_type == 1 && x.is_deleted == 0 && x.is_closed == 0 && x.is_approve == 0 && x.approver_active==1 &&(x.approver_id == login_emp_id || x.approver_role_id == login_emp_role_id)).Distinct().ToList();// && (x.approver_id==login_emp_id || x.approver_role_id== login_emp_id)).ToList();


                //List<LoanRequest> objrequestlist = new List<LoanRequest>();

                //if (result.Count > 0)
                //{
                //    foreach (var item in result)
                //    {
                //        bool containsItem = objrequestlist.Any(x => x.loan_req_id == item.loan_req_id);

                //        if (!containsItem)
                //        {
                //            LoanRequest objreq = new LoanRequest();


                //            objreq.loan_req_id = Convert.ToInt32(item.loan_req_id);
                //            objreq.emp_id = Convert.ToInt32(item.emp_id);
                //            objreq.approval_order = item.approval_order;
                //            objreq.approver_emp_id = item.approver_id;
                //            objreq.is_deleted = item.is_deleted;
                //            objreq.is_closed = item.is_closed;
                //            objreq.is_final_approver = item.is_final_approver;
                //            objreq.is_approve = item.is_approve;
                //            objreq.emp_code = item.emp_code;
                //            objreq.company_id = item.company_id;
                //            objreq.approver_type = item.approver_type;
                //            objreq.approver_role_id = item.approver_role_id;

                //            objrequestlist.Add(objreq);

                //        }
                //    }
                //}

                //return Ok(objrequestlist);
            }
            catch (Exception ex)
            {
                objrepsonse.StatusCode = 1;
                objrepsonse.Message = ex.Message;
                return Ok(objrepsonse);
            }
        }



#region **ASSETS APPROVAL ,STARTED BY SUPRIYA ON 18-07-2019**

        [Route("Save_AssetRequest")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.AssetRequest))]

        public IActionResult Save_AssetRequest([FromBody] AssetRequest objassetreq)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(objassetreq.emp_id ?? 0))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }

            if (!_clsCurrentUser.CompanyId.Contains(objassetreq.company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }

            try
            {
                List<AssetRequest> objassetreqlist = objassetreq.assetreqlist.ToList();

                var check_approver_orderavailability = _context.tbl_loan_approval_setting.Where(x => x.company_id == objassetreq.company_id && x.approver_type == 2 && x.is_active == 1).ToList();
                if (check_approver_orderavailability.Count > 0)
                {
                    var check_final_approver = check_approver_orderavailability.Where(q => q.is_final_approver != 0 && q.is_active == 1).Select(p => p.is_final_approver).FirstOrDefault();
                    if (check_final_approver != 0)
                    {
                        using (var trans = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                var asset_exist = _context.tbl_assets_master.Where(x => x.is_deleted == 0 && x.company_id == objassetreq.company_id && objassetreq.assetreqlist.Any(p => p.assets_master_id == x.asset_master_id && (p.asset_name.Trim().ToUpper() == x.asset_name.Trim().ToUpper() || p.asset_name.Trim().ToUpper() == x.short_name.Trim().ToUpper()))).ToList();

                                if (asset_exist.Count > 0)
                                {
                                    var asset_type_Req = objassetreq.assetreqlist.Any(p => (p.asset_type != 0 && p.reqt_type.Trim().ToUpper() != "NEW") && (p.asset_type != 1 && p.reqt_type.Trim().ToUpper() != "REPLACE") && (p.asset_type != 2 && p.reqt_type.Trim().ToUpper() != "SUBMIT"));
                                    if (asset_type_Req)
                                    {
                                        objresponse.StatusCode = 2;
                                        objresponse.Message = "wrong action type selected";
                                        return Ok(objresponse);
                                    }

                                    var req_ = _context.tbl_assets_request_master.Where(x => x.is_deleted == 0 && x.is_active == 1 && x.is_closed == 0 && x.company_id == objassetreq.company_id && x.req_employee_id == objassetreq.emp_id).ToList();

                                    var _assetrq = req_.Where(x => x.is_finalapprove != 1 && x.is_finalapprove != 2 && objassetreq.assetreqlist.Any(p => p.assets_master_id == x.assets_master_id)).GroupBy(p => new { p.req_employee_id, p.assets_master_id }).Select(g => g.OrderByDescending(t => t.asset_req_id).FirstOrDefault()).ToList();



                                    //var _assetrq = _context.tbl_assets_request_master.Where(x => x.is_deleted == 0 && x.is_active == 1 && x.is_closed == 0 && x.company_id == objassetreq.company_id 
                                    //&& x.req_employee_id == objassetreq.emp_id 
                                    //&& x.is_finalapprove!=1 && x.is_finalapprove!=2 && objassetreq.assetreqlist.Any(p=>p.assets_master_id==x.assets_master_id)).GroupBy(p=>new { p.req_employee_id,p.assets_master_id}).Select(g => g.OrderByDescending(t => t.asset_req_id).FirstOrDefault()).ToList();

                                    // var exist = _assetrq.Where(x => x.company_id == objassetreq.company_id && x.req_employee_id == objassetreq.emp_id && objassetreq.assetreqlist.Any(p => p.assets_master_id == x.assets_master_id)).ToList();
                                    if (_assetrq.Count > 0)
                                    {
                                        objresponse.StatusCode = 1;
                                        objresponse.Message = "One of selected Asset Rquest Already raised please check report";
                                        return Ok(objresponse);
                                    }
                                    else
                                    {
                                        if (_assetrq.Count > 0)
                                        {
                                            if (objassetreq.assetreqlist.Any(p => _assetrq.Any(y => y.assets_master_id != p.assets_master_id) && p.asset_type != 0))
                                            {
                                                objresponse.StatusCode = 1;
                                                objresponse.Message = "One of Selected asset are not exists so please select 'New' from Action Type,for futher details please check report...!!";
                                                return Ok(objresponse);
                                            }
                                        }
                                        else
                                        {
                                            if (objassetreq.assetreqlist.Any(p => p.asset_type != 0))
                                            {
                                                objresponse.StatusCode = 1;
                                                objresponse.Message = "One of Selected asset are not exists so please select 'New' from Action Type,for futher details please check report...!!";
                                                return Ok(objresponse);
                                            }
                                        }



                                        // var test = _assetrq.Where(x =>x.is_active == 1 && x.is_closed == 0 && (x.is_finalapprove != 1 && x.is_finalapprove != 2) && objassetreq.assetreqlist.Any(p => p.assets_master_id != x.assets_master_id)).ToList();


                                        //var test = objassetreq.assetreqlist.Any(x => _assetrq.Any(y => y.assets_master_id != x.assets_master_id));

                                        //var check_new_ = _assetrq.Where(x => x.company_id == objassetreq.company_id && x.req_employee_id == objassetreq.emp_id &&objassetreq.assetreqlist.Any(p=>p.assets_master_id==x.assets_master_id && p.asset_type==x.asset_type) && x.is_deleted == 0 && x.is_closed == 0 && x.is_active == 1).ToList();
                                        //if (test)
                                        //{
                                        //    if (objassetreq.assetreqlist.Any(p =>_assetrq.Any(y=>y.assets_master_id!=p.assets_master_id) && p.asset_type != 0))
                                        //    {
                                        //        objresponse.StatusCode = 1;
                                        //        objresponse.Message = "One of Selected asset are not exists so please select 'New' from Action Type,for futher details please check report...!!";
                                        //        return Ok(objresponse);
                                        //    }


                                        //}

                                        //if (check_new_ != null)
                                        //{
                                        //    check_new_.is_active = 0;
                                        //}

                                        List<tbl_assets_request_master> _req_assets = objassetreq.assetreqlist.Select(p => new tbl_assets_request_master
                                        {
                                            company_id = objassetreq.company_id,
                                            req_employee_id = objassetreq.emp_id ?? 0,
                                            asset_name = p.asset_name.Trim(),
                                            description = p.asset_description,// contain request remarks
                                            is_finalapprove = 0,
                                            is_active = 1,
                                            created_by = _clsCurrentUser.EmpId,
                                            modified_by = 0,
                                            modified_dt = Convert.ToDateTime("01-01-2000"),
                                            created_dt = DateTime.Now,
                                            asset_issue_date = p.asset_type != 0 ? req_.OrderByDescending(y => y.asset_req_id).FirstOrDefault(q => q.assets_master_id == p.assets_master_id).asset_issue_date : Convert.ToDateTime("01-01-2000"),
                                            is_deleted = 0,
                                            is_closed = 0,
                                            asset_number = p.asset_type != 0 ? req_.OrderByDescending(y => y.asset_req_id).FirstOrDefault(q => q.assets_master_id == p.assets_master_id).asset_number : "",
                                            asset_type = Convert.ToByte(p.asset_type),
                                            assets_master_id = p.assets_master_id,
                                            from_date = p.from_date,
                                            replace_dt = p.asset_type == 2 ? req_.OrderByDescending(y => y.asset_req_id).FirstOrDefault(q => q.assets_master_id == p.assets_master_id).replace_dt : Convert.ToDateTime("01-01-2000"),
                                            to_date = p.is_permanent == 1 ? Convert.ToDateTime("2500-01-01") : p.to_date,
                                            submission_date = Convert.ToDateTime("2500-01-01"),
                                            is_permanent = p.is_permanent

                                        }).ToList();


                                        _context.tbl_assets_request_master.AddRange(_req_assets);

                                    }
                                }
                                else
                                {
                                    objresponse.StatusCode = 1;
                                    objresponse.Message = "Requested Assets not available";
                                    return Ok(objresponse);
                                }





                                _context.SaveChanges();
                                SaveAssetRequestApproval(objassetreq.emp_id ?? 0);

                                trans.Commit();

                                objresponse.StatusCode = 0;
                                objresponse.Message = "Assets request successfully raised";


                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                objresponse.StatusCode = 1;
                                objresponse.Message = ex.Message;
                            }
                        }

                    }
                    else
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Create Final Approver of Company For Asset Request";
                    }
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Firstly Create Asset Approver";
                }


                if (objresponse.StatusCode == 0)
                {
                    for (int assetreqq = 0; assetreqq < objassetreqlist.Count; assetreqq++)
                    {
                        MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                        Task task = Task.Run(() => obj_ms.AssetRequestMail(Convert.ToInt32(objassetreq.emp_id), objassetreqlist[assetreqq].from_date, objassetreqlist[assetreqq].to_date, objassetreqlist[assetreqq].asset_type, objassetreq.asset_description, objassetreq.company_id, objassetreqlist[assetreqq].asset_name));//.ContinueWith(t => taskremaining--);
                        task.Wait();
                    }

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



        public void SaveAssetRequestApproval(int req_employee_id)
        {
            try
            {

                var asset_req_dtl = _context.tbl_assets_request_master.Where(x => x.req_employee_id == req_employee_id && x.is_finalapprove == 0 && x.is_deleted == 0 && x.is_active == 1 && x.is_closed == 0 && x.created_dt.Date == DateTime.Now.Date).ToList();

                if (asset_req_dtl.Count > 0)
                {

                    try
                    {

                        var approver_dtl = _context.tbl_loan_approval_setting.Where(x => x.company_id == asset_req_dtl[0].company_id && x.approver_type == 2 && x.is_active == 1).OrderBy(h => h.order).Select(p => new { p.order, p.approver_role_id }).ToList();

                        List<tbl_assets_approval> _approver_lst = new List<tbl_assets_approval>();

                        for (int m = 0; m < asset_req_dtl.Count; m++)
                        {
                            var exist_dtl = _context.tbl_assets_approval.Where(x => x.is_deleted == 0 && x.emp_id == asset_req_dtl[m].req_employee_id && x.asset_req_id == asset_req_dtl[m].asset_req_id).ToList();
                            if (exist_dtl.Count > 0)
                            {
                                exist_dtl.ForEach(p => { p.is_deleted = 1; p.last_modified_by = _clsCurrentUser.EmpId; p.last_modified_date = DateTime.Now; });
                                _context.tbl_assets_approval.RemoveRange(exist_dtl);
                            }
                            int approval_order = 0;
                            List<tbl_assets_approval> _asset_approval = approver_dtl.Select(p => new tbl_assets_approval
                            {

                                approval_order = Convert.ToByte(p.order),
                                asset_req_id = asset_req_dtl[m].asset_req_id,
                                emp_id = asset_req_dtl[m].req_employee_id,
                                is_approve = 0,
                                created_dt = DateTime.Now,
                                created_by = _clsCurrentUser.EmpId,
                                last_modified_by = 0,
                                approver_role_id = p.approver_role_id,
                                is_final_approver = 0,
                            }).ToList();

                            _approver_lst.AddRange(_asset_approval);
                        }

                        _context.tbl_assets_approval.AddRange(_approver_lst);


                        _context.SaveChanges();



                    }
                    catch (Exception ex)
                    {

                    }

                }


            }
            catch (Exception ex)
            {

            }
        }



        //public IActionResult Save_AssetRequest([FromBody] tbl_assets_request_master objassetsmster)
        //{
        //    try
        //    {
        //        Response_Msg objreponse = new Response_Msg();

        //        Classes.clsLoginEmpDtl ob = new clsLoginEmpDtl(_context, Convert.ToInt32(objassetsmster.req_employee_id), "Wite", _AC);
        //        if (!ob.is_valid())
        //        {
        //            objreponse.StatusCode = 1;
        //            objreponse.Message = "Unauthorize Access...!";
        //            return Ok(objreponse);
        //        }

        //        var check_approver_orderavailability = _context.tbl_loan_approval_setting.Where(x => x.company_id == objassetsmster.company_id && x.approver_type == 2).ToList();
        //        if (check_approver_orderavailability.Count > 0)
        //        {
        //            var check_final_approver = check_approver_orderavailability.Where(q => q.is_final_approver != 0 && q.is_active == 1).Select(p => p.is_final_approver).FirstOrDefault();
        //            if (check_final_approver != 0)
        //            {
        //                var exist = _context.tbl_assets_request_master.Where(x => x.company_id == objassetsmster.company_id && x.req_employee_id == objassetsmster.req_employee_id && x.asset_name == objassetsmster.asset_name && x.is_active == 1 && x.is_closed == 0 && x.is_deleted == 0).ToList();
        //                if (exist.Count > 0)
        //                {
        //                    objreponse.StatusCode = 1;
        //                    objreponse.Message = "Assets Request Already Raised by this Employee";
        //                }
        //                else
        //                {
        //                    objassetsmster.is_active = 1;
        //                    objassetsmster.is_closed = 0;
        //                    objassetsmster.is_deleted = 0;
        //                    objassetsmster.created_dt = DateTime.Now;
        //                    objassetsmster.is_finalapprove = 0;

        //                    _context.Entry(objassetsmster).State = EntityState.Added;
        //                    _context.SaveChanges();

        //                    SaveAssetRequestApproval(objassetsmster.req_employee_id);


        //                    objreponse.StatusCode = 0;
        //                    objreponse.Message = "Assets Request Successfully Saved";
        //                }
        //            }
        //            else
        //            {
        //                objreponse.StatusCode = 1;
        //                objreponse.Message = "Create Final Approver of Company For Asset Request";

        //            }
        //        }
        //        else
        //        {
        //            objreponse.StatusCode = 1;
        //            objreponse.Message = "Firstly Create Asset Approver";
        //        }


        //        return Ok(objreponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}


        //public void SaveAssetRequestApproval(int req_employee_id)
        //{
        //    try
        //    {

        //        var asset_req_dtl = _context.tbl_assets_request_master.Where(x => x.req_employee_id == req_employee_id && x.is_finalapprove == 0).OrderByDescending(x => x.asset_req_id).Take(1).FirstOrDefault();

        //        if (asset_req_dtl != null)
        //        {
        //            int approval_order = 0;

        //            //approver type 2 is used for asset
        //            var company_id = _context.tbl_employee_company_map.Where(x => x.employee_id == req_employee_id && x.is_deleted == 0).OrderByDescending(x => x.sno).Take(1).Select(p => p.company_id).FirstOrDefault();
        //            var approver_dtl = _context.tbl_loan_approval_setting.Where(x => x.company_id == company_id && x.approver_type == 2).OrderBy(h => h.order).Select(p => new { p.order, p.approver_role_id }).ToList();


        //            for (int Index = 0; Index < approver_dtl.Count; Index++)
        //            {
        //                tbl_assets_approval objapproval = new tbl_assets_approval();
        //                approval_order = approver_dtl[Index].order;

        //                objapproval.asset_req_id = asset_req_dtl.asset_req_id;
        //                objapproval.emp_id = asset_req_dtl.req_employee_id;
        //                objapproval.is_approve = 0;
        //                objapproval.created_dt = DateTime.Now;
        //                objapproval.created_by = asset_req_dtl.created_by;
        //                objapproval.last_modified_by = 0;
        //                objapproval.approval_order = Convert.ToByte(approval_order);

        //                objapproval.approver_role_id = approver_dtl[Index].approver_role_id;

        //                objapproval.is_final_approver = 0;
        //                _context.Entry(objapproval).State = EntityState.Added;
        //                _context.SaveChanges();
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        [Route("Get_AssetRequest/{asset_req_id}")]
        [HttpGet]
        //[Authorize(Policy = "8109")]
        public IActionResult Get_AssetRequest([FromRoute] int asset_req_id)
        {
            try
            {
                if (asset_req_id > 0)
                {
                    var exist = _context.tbl_assets_request_master.Where(x => x.asset_req_id == asset_req_id).FirstOrDefault();
                    return Ok(exist);
                }
                else
                {

                    var data = _context.tbl_assets_request_master.Where(x => x.is_active == 1 && x.is_deleted == 0).Select(p => new
                    {

                        p.asset_req_id,
                        p.company_id,
                        p.emp_master.tbl_employee_company_map.FirstOrDefault(y => y.is_deleted == 0).tbl_company_master.company_name,
                        p.req_employee_id,
                        req_emp_name_code = string.Format("{0} {1} {2} ({3})",
                        p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && q.employee_first_name != "").employee_first_name,
                        p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && q.employee_middle_name != "").employee_middle_name,
                        p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && q.employee_last_name != "").employee_last_name,
                        p.emp_master.emp_code),
                        p.asset_name,
                        p.description,
                        p.is_finalapprove,
                        p.created_dt,
                        p.asset_issue_date,
                        p.modified_dt,
                        p.is_closed
                    }).ToList();

                    return Ok(data);
                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Get_PendingAssetRequestFromEmpMasterByCom/{login_emp_id}/{login_emp_role}/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.AssetApproval))]
        public IActionResult Get_PendingAssetRequestFromEmpMasterByCom([FromRoute] int login_emp_id, int login_emp_role, int company_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Company Access...!!";
                return Ok(objresponse);
            }

            if (_clsCurrentUser.EmpId != login_emp_id)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }

            try
            {
                var data = _context.tbl_assets_approval.Join(_context.tbl_loan_approval_setting, a => a.approval_order, b => b.order, (a, b) => new
                {
                    a.asset_approval_sno,
                    a.asset_req_id,
                    a.approval_order,
                    a.is_approve,
                    a.is_final_approver,
                    a.asset_req_mastr.is_closed,
                    a.asset_req_mastr.is_deleted,
                    a.asset_req_mastr.is_active,
                    b.company_id,
                    approver_role_id = b.approver_role_id,
                    approver_id = b.emp_id,
                    req_emp_name_code = string.Format("{0} {1} {2} ({3})",
                           a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0 && p.employee_first_name != "").employee_first_name,
                            a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0 && p.employee_middle_name != "").employee_middle_name,
                             a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0 && p.employee_last_name != "").employee_last_name,
                             a.employee_master.emp_code),
                    a.emp_id,
                    approver_active = b.is_active
                }).Distinct().Where(y => (y.approver_id == login_emp_id || y.approver_role_id == login_emp_role) && y.is_deleted == 0 && y.is_closed == 0 && y.company_id == company_id && y.is_approve == 0 && y.approver_active == 1).ToList();


                List<AssetRequest> objrequestlist = new List<AssetRequest>();
                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        bool containsItem = objrequestlist.Any(x => x.asset_req_id == item.asset_req_id);
                        if (!containsItem)
                        {
                            AssetRequest objrequest = new AssetRequest();

                            objrequest.asset_req_id = Convert.ToInt32(item.asset_req_id);
                            objrequest.approval_order = item.approval_order;
                            objrequest.approver_emp_id = item.approver_id;
                            objrequest.approver_role_id = item.approver_role_id;
                            objrequest.emp_id = item.emp_id;
                            objrequest.emp_name_code = item.req_emp_name_code;

                            objrequestlist.Add(objrequest);
                        }
                    }

                }



                return Ok(objrequestlist);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("Get_PendingAssetRequestDetail/{login_emp_id}/{login_role_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.AssetApproval))]
        public IActionResult Get_PendingAssetRequestDetail([FromRoute] int login_emp_id, int login_role_id)
        {
            try
            {
                var data = _context.tbl_assets_approval.Join(_context.tbl_loan_approval_setting, a => a.approval_order, b => b.order, (a, b) => new
                {
                    a.asset_approval_sno,
                    a.asset_req_id,
                    a.approval_order,
                    b.order,
                    approver_emp_id = b.emp_id,
                    approver_role_id = b.approver_role_id,
                    a.asset_req_mastr.is_active,
                    a.asset_req_mastr.is_deleted,
                    a.asset_req_mastr.assets_master.asset_name,
                    a.created_dt,
                    a.asset_req_mastr.description,
                    a.is_approve,
                    a.asset_req_mastr.is_finalapprove,
                    a.emp_id,
                    req_emp_name_code = string.Format("{0} {1} {2} ({3})",
                             a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0 && p.employee_first_name != "").employee_first_name,
                             a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0 && p.employee_middle_name != "").employee_middle_name,
                             a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0 && p.employee_last_name != "").employee_last_name,
                             a.employee_master.emp_code),
                    a.employee_master.tbl_employee_company_map.FirstOrDefault(g => g.is_deleted == 0).tbl_company_master.company_name,
                    a.asset_req_mastr.asset_issue_date,
                    a.asset_req_mastr.is_closed,
                    b.approver_type,
                    a.asset_req_mastr.from_date,
                    a.asset_req_mastr.to_date,
                    a.asset_req_mastr.asset_number,
                    a.asset_req_mastr.asset_type,
                    a.asset_req_mastr.submission_date,
                    _reqtbl_finalapprove = a.asset_req_mastr.is_finalapprove,
                    b.company_id,
                    approver_active = b.is_active
                }).Where(x => x.approver_type == 2 && x.is_approve == 0 && x.approver_active == 1 && x._reqtbl_finalapprove == 0 && (x.approver_role_id == login_role_id || x.approver_emp_id == login_emp_id) && x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id)
                ).OrderBy(x => x.asset_req_id).ToList();


                List<AssetRequest> objrequestlist = new List<AssetRequest>();

                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        bool containsItem = objrequestlist.Any(x => x.asset_req_id == item.asset_req_id);

                        if (!containsItem)
                        {
                            AssetRequest objrequest = new AssetRequest();

                            objrequest.asset_req_id = Convert.ToInt32(item.asset_req_id);
                            objrequest.approval_order = item.approval_order;
                            objrequest.approver_emp_id = item.approver_emp_id;
                            objrequest.approver_role_id = item.approver_role_id;
                            objrequest.asset_name = item.asset_name;
                            objrequest.created_dt = item.created_dt;
                            objrequest.asset_description = item.description;
                            objrequest.is_approve = item.is_approve;
                            objrequest.is_final_approver = Convert.ToByte(item.is_finalapprove);
                            objrequest.emp_id = item.emp_id;
                            objrequest.emp_name_code = item.req_emp_name_code;
                            objrequest.company_name = item.company_name;
                            objrequest.asset_issue_date = item.asset_issue_date;
                            objrequest.is_closed = item.is_closed;
                            objrequest.approver_type = item.approver_type;
                            objrequest.asset_number = item.asset_number;
                            objrequest.asset_type = item.asset_type;
                            objrequest.from_date = item.from_date;
                            objrequest.to_date = item.to_date;
                            objrequest.submission_date = item.submission_date;

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
                                objrequest.asset_name = item.asset_name;
                                objrequest.created_dt = item.created_dt;
                                objrequest.asset_description = item.description;
                                objrequest.is_approve = item.is_approve;
                                objrequest.is_final_approver = Convert.ToByte(item.is_finalapprove);
                                objrequest.emp_id = item.emp_id;
                                objrequest.emp_name_code = item.req_emp_name_code;
                                objrequest.company_name = item.company_name;
                                objrequest.asset_issue_date = item.asset_issue_date;
                                objrequest.is_closed = item.is_closed;
                                objrequest.approver_type = item.approver_type;
                                objrequest.asset_number = item.asset_number;
                                objrequest.asset_type = item.asset_type;
                                objrequest.from_date = item.from_date;
                                objrequest.to_date = item.to_date;
                                objrequest.submission_date = item.submission_date;

                                objrequestlist.Add(objrequest);
                            }
                        }

                    }

                }

                return Ok(objrequestlist);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        [Route("GetEmployeeDetailForAssetApproval/{emp_id}/{asset_req_id}")]
        public IActionResult GetEmployeeDetailForAssetApproval([FromRoute] int emp_id, int asset_req_id)
        {
            try
            {
                //var emp_asset_data = _context.tbl_assets_approval.Where(x => x.emp_id == emp_id && x.asset_req_id == asset_req_id && x.is_approve == 0 && x.asset_req_mastr.is_closed == 0 && x.is_deleted == 0).Select(p => new
                //{
                //    asset_req_id = p.asset_req_id,
                //    asset_name = p.asset_req_mastr.asset_name,
                //    asset_description = p.asset_req_mastr.description,
                //    dept_name = p.asset_req_mastr.emp_master.tbl_emp_officaial_sec.OrderByDescending(z => z.emp_official_section_id).Take(1).FirstOrDefault(z => z.is_deleted == 0).tbl_department_master.department_name,
                //    grade_id = p.asset_req_mastr.emp_master.tbl_emp_grade_allocation.OrderByDescending(z => z.emp_grade_id).Take(1).FirstOrDefault(z => z.employee_id == emp_id).grade_id,
                //    grade_name = p.asset_req_mastr.emp_master.tbl_emp_grade_allocation.OrderByDescending(z => z.emp_grade_id).Take(1).FirstOrDefault(z => z.tbl_grade_master.is_active == 1).tbl_grade_master.grade_name,
                //    des_name = p.asset_req_mastr.emp_master.tbl_emp_desi_allocation.OrderByDescending(z => z.emp_grade_id).Take(1).FirstOrDefault(z => z.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name,
                //}).OrderByDescending(x => x.asset_req_id).Take(1).FirstOrDefault(); 

                var emp_asset_data = _context.tbl_assets_request_master.Where(x => x.req_employee_id == emp_id && x.asset_req_id == asset_req_id && x.is_finalapprove == 0
                                   && x.is_closed == 0 && x.is_deleted == 0).Select(p => new
                                   {
                                       asset_req_id = p.asset_req_id,
                                       asset_name = p.asset_name,
                                       asset_description = p.description,
                                       //loan_type = p.loan_type == 1 ? "Loan" : "Advance",
                                       // loan_amt = p.loan_amt,
                                       // loan_tenure = p.loan_tenure,
                                       // roi = p.interest_rate,
                                       // emp_id = p.req_emp_id,
                                       // loan_purpose = p.loan_purpose,
                                       // emp_name = p.emp_code,
                                      // dept_name = p.emp_master.tbl_emp_officaial_sec.OrderByDescending(z => z.emp_official_section_id).Take(1).FirstOrDefault(z => z.is_deleted == 0).tbl_department_master.department_name,
                                       grade_id = p.emp_master.tbl_emp_grade_allocation.OrderByDescending(z => z.emp_grade_id).Take(1).FirstOrDefault(z => z.employee_id == emp_id).grade_id,
                                       grade_name = p.emp_master.tbl_emp_grade_allocation.OrderByDescending(z => z.emp_grade_id).Take(1).FirstOrDefault(z => z.tbl_grade_master.is_active == 1).tbl_grade_master.grade_name,
                                       des_name = p.emp_master.tbl_emp_desi_allocation.OrderByDescending(z => z.emp_grade_id).Take(1).FirstOrDefault(z => z.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name,

                                   }).OrderByDescending(x => x.asset_req_id).Take(1).FirstOrDefault();


                return Ok(emp_asset_data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("AssetRequestApproval")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.AssetApproval))]

        public IActionResult AssetRequestApproval([FromBody] AssetApprovalSave objasset)
        {
            Response_Msg objResult = new Response_Msg();

            if (objasset.is_approve != 1 && objasset.is_approve != 2)
            {
                objResult.StatusCode = 1;
                objResult.Message = "Invalid Action...";
                return Ok(objasset);
            }

            try
            {
                var get_asset_approval_order = _context.tbl_loan_approval_setting.Where(x => (x.emp_id == _clsCurrentUser.EmpId || _clsCurrentUser.RoleId.Contains(x.approver_role_id ?? 0)) && x.approver_type == 2 && x.is_active == 1).OrderByDescending(q => q.order).FirstOrDefault();
                if (get_asset_approval_order != null)
                {
                    //update in tbl_asset_approval

                    using (var trans = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            var emp_asset = _context.tbl_assets_approval.Where(x => x.is_deleted == 0 && x.approval_order == get_asset_approval_order.order && objasset.asset_req_dtl.Any(p => p.asset_req_id == x.asset_req_id && p.emp_id == x.emp_id)).ToList();
                            if (emp_asset.Count > 0)
                            {
                                if (get_asset_approval_order.is_final_approver == 1)
                                {
                                    if (objasset.is_approve == 1 && objasset.asset_req_dtl.Any(p => string.IsNullOrEmpty(p.asset_number)))
                                    {
                                        trans.Rollback();
                                        objResult.StatusCode = 1;
                                        objResult.Message = "Asset No. cannot be blank";
                                        return Ok(objResult);
                                    }
                                }

                                emp_asset.ForEach(p =>
                                {

                                    p.is_approve = objasset.is_approve;
                                    p.is_final_approver = get_asset_approval_order.is_final_approver > 0 && objasset.is_approve == 1 ? objasset.is_approve : Convert.ToByte(3);
                                    p.last_modified_by = _clsCurrentUser.EmpId;
                                    p.last_modified_date = DateTime.Now;
                                });

                                //if (objasset.is_approve == 2)
                                //{
                                //    List<tbl_assets_approval> tbl_approval = _context.tbl_assets_approval.Where(x =>objasset.asset_req_dtl.Any(q=>q.asset_req_id==x.asset_req_id && q.emp_id==x.emp_id) && x.is_deleted == 0) .ToList();

                                //    tbl_approval.ForEach(p => { p.is_approve = 2; p.is_final_approver = 2; p.last_modified_by = _clsCurrentUser.EmpId; ; p.last_modified_date = DateTime.Now; });
                                //    _context.tbl_assets_approval.UpdateRange(tbl_approval);
                                //}
                                //else
                                //{
                                _context.tbl_assets_approval.UpdateRange(emp_asset);
                                //}

                                var exist1 = _context.tbl_assets_request_master.Where(y => objasset.asset_req_dtl.Any(q => q.asset_req_id == y.asset_req_id && q.emp_id == y.req_employee_id) && y.is_deleted == 0).ToList();
                                if (exist1.Count > 0)
                                {
                                    if (objasset.is_approve == 2) // update reject in asset request master application
                                    {
                                        exist1.ForEach(p =>
                                        {
                                            p.asset_number = p.asset_number;
                                            p.is_finalapprove = objasset.is_approve;
                                            p.modified_by = _clsCurrentUser.EmpId;
                                            p.modified_dt = DateTime.Now;
                                            p.is_closed = 1;
                                        });

                                        // _context.tbl_assets_request_master.UpdateRange(exist1);
                                    }
                                    else if (exist1.Any(p => string.IsNullOrEmpty(p.asset_number)))
                                    {
                                        exist1.ForEach(p =>
                                        {
                                            p.asset_number = objasset.asset_req_dtl.FirstOrDefault(q => q.emp_id == p.req_employee_id && q.asset_req_id == p.asset_req_id).asset_number;
                                            p.modified_by = _clsCurrentUser.EmpId;
                                            p.modified_dt = DateTime.Now;
                                        });
                                        //  _context.tbl_assets_request_master.UpdateRange(exist1);
                                    }
                                    if (get_asset_approval_order.is_final_approver > 0)
                                    {
                                        exist1.ForEach(p =>
                                        {
                                            p.is_finalapprove = objasset.is_approve;
                                            if (objasset.is_approve == 1)
                                            {
                                                p.asset_issue_date = p.asset_type == 0 ? objasset.is_approve == 1 ? DateTime.Now : p.asset_issue_date : p.asset_issue_date;
                                                p.replace_dt = p.asset_type == 1 ? DateTime.Now : p.replace_dt;

                                            }
                                            p.is_closed = p.asset_type == 2 ? Convert.ToByte(1) : Convert.ToByte(0);
                                            p.submission_date = p.asset_type == 2 ? DateTime.Now : p.submission_date;
                                            p.modified_by = _clsCurrentUser.EmpId;
                                            p.modified_dt = DateTime.Now;

                                        });


                                        _context.tbl_assets_request_master.UpdateRange(exist1);
                                    }


                                    if (exist1.Any(p => p.is_finalapprove == 1 || p.is_finalapprove == 2))
                                    {
                                        List<int> _empidd = exist1.Where(x => x.is_finalapprove == 1 || x.is_finalapprove == 2).Select(p => p.req_employee_id).ToList();

                                        for (int i = 0; i < _empidd.Count; i++)
                                        {
                                            MailDetails obj_maild = new MailDetails();


                                            var emp_data = _context.tbl_emp_officaial_sec.Where(q => q.employee_id == _empidd[i] && q.is_deleted == 0).Select(p => new { p.employee_first_name, p.employee_middle_name, p.employee_last_name, p.official_email_id }).FirstOrDefault();
                                            obj_maild.employee_first_name = emp_data != null ? emp_data.employee_first_name : "";
                                            obj_maild.employee_last_name = emp_data != null ? emp_data.employee_last_name : "";
                                            obj_maild.email_id = emp_data != null ? emp_data.official_email_id : "";
                                            obj_maild.day_status = objasset.is_approve.ToString();
                                            obj_maild.absent_date = exist1.FirstOrDefault(x => x.req_employee_id == _empidd[i]).asset_name;

                                            MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString, _config);

                                            Task task = Task.Run(() => obj_ms.AssetApprovalNotifaction(obj_maild));
                                            task.Wait();
                                        }

                                    }

                                }

                                else
                                {
                                    trans.Rollback();
                                    objResult.StatusCode = 1;
                                    objResult.Message = "Something went wrong";
                                    return Ok(objResult);
                                }


                            }
                            else
                            {
                                trans.Rollback();
                                objResult.StatusCode = 1;
                                objResult.Message = "Something went wrong";
                                return Ok(objResult);

                            }

                            _context.SaveChanges();

                            trans.Commit();

                            objResult.StatusCode = 0;
                            objResult.Message = "Successfully Submited..!";

                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            objResult.StatusCode = 1;
                            objResult.Message = ex.Message;
                        }
                    }
                }
                else
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "You are not an approver of this Asset";
                }

            }
            catch (Exception ex)
            {
                objResult.StatusCode = 0;
                objResult.Message = ex.Message;

            }

            return Ok(objResult);
        }





        //public IActionResult AssetRequestApproval([FromBody] AssetApprovalSave objasset)
        //{
        //    Response_Msg objResult = new Response_Msg();
        //    //Classes.clsLoginEmpDtl ob = new clsLoginEmpDtl(_context, Convert.ToInt32(objasset.emp_id), "read", _AC);
        //    //if (!ob.is_valid())
        //    //{
        //    //    objResult.StatusCode = 1;
        //    //    objResult.Message = "Unauthorize Access...!";
        //    //    return Ok(objResult);
        //    //}

        //    try
        //    {
        //        string approver_idd = "";
        //        int orderr = 0;

        //        var test = _context.tbl_loan_approval_setting.Where(x => (x.emp_id == objasset.last_modified_by || x.approver_role_id == objasset.approver_role_id) && x.approver_type == 2).OrderByDescending(q => q.order).ToList();
        //        if (test.Count > 1)
        //        {
        //            //var get_approver_dtl = _context.tbl_loan_approval_setting.Where(x => (x.emp_id == objasset.last_modified_by || x.approver_role_id == objasset.approver_role_id) && x.approver_type == 2 && x.is_final_approver==1).OrderByDescending(q => q.order).Select(p => new { p.order, p.emp_id, p.is_final_approver, p.approver_role_id }).FirstOrDefault();
        //            //  if (get_approver_dtl != null)
        //            //  {
        //            //      approver_idd = get_approver_dtl.is_final_approver;
        //            //      orderr = get_approver_dtl.order;
        //            //  }
        //            //  else
        //            //  {
        //            //update in tbl_asset_approval

        //            for (int i = 0; i < test.Count; i++)
        //            {
        //                for (int j = 0; j < objasset.asset_req_id.Count; j++)
        //                {
        //                    var data = _context.tbl_assets_approval.Where(x => x.asset_req_id == Convert.ToInt32(objasset.asset_req_id[j]) && x.emp_id == objasset.emp_id && x.approval_order == test[i].order).OrderByDescending(x => x.asset_approval_sno).Select(a => new { a.asset_approval_sno, a.asset_req_id, a.emp_id, a.is_final_approver, a.is_approve, a.is_deleted, a.last_modified_by, a.last_modified_date, a.approval_order, a.approver_role_id }).FirstOrDefault();
        //                    if (data != null)
        //                    {
        //                        //var exist = _context.tbl_assets_approval.Where(x => x.asset_req_id == objasset.asset_req_id && x.emp_id == objasset.emp_id && x.approval_order == approver_idd.order).OrderByDescending(y => y.asset_approval_sno).FirstOrDefault();
        //                        var exist = _context.tbl_assets_approval.Where(x => x.asset_req_id == Convert.ToInt32(objasset.asset_req_id[j]) && x.emp_id == objasset.emp_id && x.approval_order == test[i].order).OrderByDescending(y => y.asset_approval_sno).FirstOrDefault();

        //                        if (exist != null)
        //                        {
        //                            exist.asset_approval_sno = exist.asset_approval_sno;
        //                            exist.asset_req_id = Convert.ToInt32(objasset.asset_req_id[j]);
        //                            exist.emp_id = objasset.emp_id;
        //                            exist.is_approve = objasset.is_approve;
        //                            //if (approver_idd.is_final_approver > 0 && objasset.is_approve == 1)
        //                            if (test[i].is_final_approver > 0 && objasset.is_approve == 1)
        //                            {
        //                                exist.is_final_approver = objasset.is_approve;
        //                            }
        //                            else if (objasset.is_approve == 2)
        //                            {
        //                                //exist.is_final_approver = objasset.is_approve;
        //                                List<tbl_assets_approval> update_all = _context.tbl_assets_approval.Where(x => x.asset_req_id == Convert.ToInt32(objasset.asset_req_id[j])).ToList();

        //                                update_all.ForEach(p => { p.is_approve = 2; p.is_final_approver = 2; p.last_modified_by = objasset.last_modified_by; p.last_modified_date = DateTime.Now; });
        //                                _context.SaveChanges();
        //                            }
        //                            else
        //                            {
        //                                exist.is_final_approver = 3; // In Process
        //                            }
        //                            //else
        //                            //{
        //                            //    exist.is_final_approver = 0;
        //                            //}

        //                            exist.is_deleted = 0;
        //                            exist.last_modified_by =objasset.last_modified_by;
        //                            exist.last_modified_date = DateTime.Now;

        //                            _context.tbl_assets_approval.Attach(exist);
        //                            _context.Entry(exist).State = EntityState.Modified;





        //                            //17-02-2020 start Update asset no

        //                            if (objasset.is_approve == 1 && !string.IsNullOrEmpty(objasset.asset_number.ToString()))
        //                            {
        //                              List<tbl_assets_request_master> 
        //                            }

        //                            //17-02-2020 end






        //                            var exist1 = _context.tbl_assets_request_master.Where(y => y.asset_req_id == Convert.ToInt32(objasset.asset_req_id[j]) && y.req_employee_id == objasset.emp_id).FirstOrDefault();
        //                            if (objasset.is_approve == 2)
        //                            {
        //                                exist1.asset_req_id = Convert.ToInt32(exist1.asset_req_id);
        //                                exist1.req_employee_id = exist1.req_employee_id;
        //                                exist1.is_finalapprove = objasset.is_approve;
        //                                // exist1.last_modified_by = 1;
        //                                exist1.modified_dt = DateTime.Now;
        //                                exist1.is_closed = 1;
        //                                _context.tbl_assets_request_master.Attach(exist1);
        //                                _context.Entry(exist1).State = EntityState.Modified;

        //                            }

        //                            //if (approver_idd.is_final_approver > 0)
        //                            if (test[i].is_final_approver > 0)
        //                            {
        //                                exist1.asset_req_id = Convert.ToInt32(exist1.asset_req_id);
        //                                exist1.req_employee_id = exist1.req_employee_id;
        //                                exist1.is_finalapprove = objasset.is_approve;
        //                                if (objasset.is_approve == 1)
        //                                {
        //                                    exist1.asset_issue_date = DateTime.Now;

        //                                }
        //                                //exist1.last_modified_by = 1;
        //                                exist1.modified_dt = DateTime.Now;
        //                                exist1.is_closed = 1;

        //                                _context.tbl_assets_request_master.Attach(exist1);
        //                                _context.Entry(exist1).State = EntityState.Modified;
        //                            }



        //                        }




        //                    }

        //                }


        //            }
        //            _context.SaveChanges();
        //            objResult.StatusCode = 0;
        //            objResult.Message = "Successfully Submited..!";
        //            // }
        //            //orderr = _context.tbl_loan_approval_setting.Where(x => (x.emp_id == objasset.last_modified_by || x.approver_role_id == objasset.approver_role_id) && x.approver_type == 2 && x.is_final_approver == 1).OrderByDescending(q => q.order).Select(p => new { p.order, p.emp_id, p.is_final_approver, p.approver_role_id }).FirstOrDefault().order;
        //        }
        //        else
        //        {


        //            var get_approver_idd = _context.tbl_loan_approval_setting.Where(x => (x.emp_id == objasset.last_modified_by || x.approver_role_id == objasset.approver_role_id) && x.approver_type == 2).OrderByDescending(q => q.order).Select(p => new { p.order, p.emp_id, p.is_final_approver, p.approver_role_id }).FirstOrDefault();
        //            if (get_approver_idd != null)
        //            {
        //                approver_idd = get_approver_idd.is_final_approver.ToString();
        //                orderr = get_approver_idd.order;
        //            }
        //            // orderr=_context.tbl_loan_approval_setting.Where(x => (x.emp_id == objasset.last_modified_by || x.approver_role_id == objasset.approver_role_id) && x.approver_type == 2).OrderByDescending(q => q.order).Select(p => new { p.order, p.emp_id, p.is_final_approver, p.approver_role_id }).FirstOrDefault().order;
        //        }




        //        //var approver_idd = _context.tbl_loan_approval_setting.Where(x => (x.emp_id == objasset.last_modified_by||x.approver_role_id==objasset.approver_role_id)&& x.approver_type==2).OrderByDescending(q => q.order).Select(p => new { p.order, p.emp_id, p.is_final_approver,p.approver_role_id }).FirstOrDefault();

        //        if (!string.IsNullOrEmpty(approver_idd))
        //        {
        //            // var data = _context.tbl_assets_approval.Where(x => x.asset_req_id == objasset.asset_req_id && x.emp_id == objasset.emp_id && x.approval_order == approver_idd.order).OrderByDescending(x => x.asset_approval_sno).Select(a => new { a.asset_approval_sno, a.asset_req_id, a.emp_id, a.is_final_approver, a.is_approve, a.is_deleted, a.last_modified_by, a.last_modified_date, a.approval_order,a.approver_role_id }).FirstOrDefault();
        //            for (int j = 0; j < objasset.asset_req_id.Count; j++)
        //            {
        //                var data = _context.tbl_assets_approval.Where(x => x.asset_req_id == Convert.ToInt32(objasset.asset_req_id[j]) && x.approval_order == orderr).OrderByDescending(x => x.asset_approval_sno).Select(a => new { a.asset_approval_sno, a.asset_req_id, a.emp_id, a.is_final_approver, a.is_approve, a.is_deleted, a.last_modified_by, a.last_modified_date, a.approval_order, a.approver_role_id }).FirstOrDefault();
        //                if (data != null)
        //                {
        //                    //var exist = _context.tbl_assets_approval.Where(x => x.asset_req_id == objasset.asset_req_id && x.emp_id == objasset.emp_id && x.approval_order == approver_idd.order).OrderByDescending(y => y.asset_approval_sno).FirstOrDefault();
        //                    var exist = _context.tbl_assets_approval.Where(x => x.asset_req_id == Convert.ToInt32(objasset.asset_req_id[j]) && x.approval_order == orderr).OrderByDescending(y => y.asset_approval_sno).FirstOrDefault();

        //                    if (exist != null)
        //                    {
        //                        exist.asset_approval_sno = exist.asset_approval_sno;
        //                        exist.asset_req_id = Convert.ToInt32(objasset.asset_req_id[j]);
        //                        //  exist.emp_id = objasset.emp_id;
        //                        exist.is_approve = objasset.is_approve;
        //                        //if (approver_idd.is_final_approver > 0 && objasset.is_approve == 1)
        //                        if (Convert.ToInt32(approver_idd) > 0 && objasset.is_approve == 1)
        //                        {
        //                            exist.is_final_approver = objasset.is_approve;
        //                        }
        //                        else if (objasset.is_approve == 2)
        //                        {
        //                            //exist.is_final_approver = objasset.is_approve;

        //                            List<tbl_assets_approval> update_all = _context.tbl_assets_approval.Where(x => x.asset_req_id == Convert.ToInt32(objasset.asset_req_id[j])).ToList();

        //                            update_all.ForEach(p => { p.is_approve = 2; p.is_final_approver = 2; p.last_modified_by = objasset.last_modified_by; p.last_modified_date = DateTime.Now; });
        //                            _context.SaveChanges();


        //                        }
        //                        else
        //                        {
        //                            //exist.is_final_approver = 0;
        //                            exist.is_final_approver =3;
        //                        }

        //                        exist.is_deleted = 0;
        //                        exist.last_modified_by = objasset.last_modified_by;
        //                        exist.last_modified_date = DateTime.Now;

        //                        _context.tbl_assets_approval.Attach(exist);
        //                        _context.Entry(exist).State = EntityState.Modified;


        //                        var exist1 = _context.tbl_assets_request_master.Where(y => y.asset_req_id == Convert.ToInt32(objasset.asset_req_id[j])).FirstOrDefault();
        //                        if (objasset.is_approve == 2)
        //                        {
        //                            exist1.asset_req_id = Convert.ToInt32(exist1.asset_req_id);
        //                            exist1.req_employee_id = exist1.req_employee_id;
        //                            exist1.is_finalapprove = objasset.is_approve;
        //                            // exist1.last_modified_by = 1;
        //                            exist1.modified_dt = DateTime.Now;
        //                            exist1.is_closed = 1;
        //                            exist1.asset_number = objasset.asset_number[j].ToString();
        //                            _context.tbl_assets_request_master.Attach(exist1);
        //                            _context.Entry(exist1).State = EntityState.Modified;

        //                        }

        //                        //if (approver_idd.is_final_approver > 0)
        //                        if (Convert.ToInt32(approver_idd) > 0)
        //                        {
        //                            exist1.asset_req_id = Convert.ToInt32(exist1.asset_req_id);
        //                            exist1.req_employee_id = exist1.req_employee_id;
        //                            exist1.is_finalapprove = objasset.is_approve;
        //                            if (objasset.is_approve == 1)
        //                            {
        //                                exist1.asset_issue_date = DateTime.Now;

        //                            }
        //                            //exist1.last_modified_by = 1;
        //                            exist1.modified_dt = DateTime.Now;
        //                            exist1.is_closed = 1;
        //                            exist1.asset_number = objasset.asset_number[j].ToString();
        //                            _context.tbl_assets_request_master.Attach(exist1);
        //                            _context.Entry(exist1).State = EntityState.Modified;
        //                        }

        //                        if (exist1.is_finalapprove == 1)
        //                        {
        //                            MailDetails obj_maild = new MailDetails();
        //                            var emp_data = _context.tbl_emp_officaial_sec.Where(a => a.is_deleted == 0 && a.employee_id == exist1.req_employee_id).Select(b => new { b.employee_first_name, b.employee_last_name, b.official_email_id }).FirstOrDefault();
        //                            obj_maild.employee_first_name = emp_data.employee_first_name;
        //                            obj_maild.employee_last_name = emp_data.employee_last_name;
        //                            obj_maild.email_id = emp_data.official_email_id;
        //                            obj_maild.day_status = objasset.is_approve.ToString();
        //                            obj_maild.absent_date = exist1.asset_name;

        //                            MailSystem obj_ms = new MailSystem(_appSettings.Value.DbConnectionString);

        //                            Task task = Task.Run(() => obj_ms.AssetApprovalNotifaction(obj_maild));
        //                        }
        //                    }


        //                }
        //            }
        //            _context.SaveChanges();
        //            objResult.StatusCode = 0;
        //            objResult.Message = "Asset Request Successfully Updated!!";

        //        }
        //        else if (objResult.Message != "" && objResult.StatusCode != 0)
        //        {
        //            objResult.StatusCode = 1;
        //            objResult.Message = "Sorry You are not an approver of this Asset";
        //        }
        //        return Ok(objResult);



        //    }
        //    catch (Exception ex)
        //    {
        //        objResult.StatusCode = 0;
        //        objResult.Message = ex.Message;
        //        return Ok(objResult);
        //    }
        //}
#endregion ** ASSETS APPROVAL, END BY SUPRIYA ON 18-07-2019 **





#region ** STARTED BY SUPRIYA ON 24-07-2019

        [Route("Get_LoanRequestByEmpId/{EmpID}/{companyid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LoanRequest))]
        public IActionResult Get_LoanRequestByEmpId([FromRoute] int EmpID, int companyid)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.CompanyId.Contains(companyid))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }

            if (!_clsCurrentUser.DownlineEmpId.Contains(EmpID))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }
            try
            {


                var result = _context.tbl_loan_request.Where(x => x.req_emp_id == EmpID && x.is_deleted == 0).Select(p => new
                {
                    p.loan_req_id,
                    p.req_emp_id,
                    p.emp_code,
                    p.loan_type,
                    p.loan_amt,
                    p.loan_tenure,
                    p.loan_purpose,
                    p.interest_rate,
                    p.is_closed,
                    p.is_final_approval,
                    p.created_dt,
                    p.last_modified_date,
                    p.start_date,
                    p.emp_master.tbl_employee_company_map.FirstOrDefault(q => q.is_deleted == 0).tbl_company_master.company_name,
                }).OrderBy(y => y.loan_req_id).ToList();


                return Ok(result);


            }
            catch (Exception ex)
            {
                objResult.StatusCode = 1;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }


        [Route("Get_AssetRequestByEmpID/{EmpID}/{companyid}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.AssetRequest))]
        public IActionResult Get_AssetRequestByEmpID([FromRoute] int EmpID, int companyid)
        {
            Response_Msg objResult = new Response_Msg();

            if (!_clsCurrentUser.DownlineEmpId.Contains(EmpID))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }
            if (!_clsCurrentUser.CompanyId.Contains(companyid))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access...!!";
                return Ok(objResult);
            }
            try
            {


                var empdtl = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && x.tbl_employee_id_details.tbl_employee_company_map.FirstOrDefault(q => q.is_deleted == 0).company_id == companyid && x.employee_id == EmpID).FirstOrDefault();

                //var DesignationData = _context.tbl_emp_desi_allocation.OrderByDescending(q => q.emp_grade_id).Where(p => p.applicable_from_date <= PayrollLastdate && p.applicable_to_date >= PayrollLastdate && p.desig_id != null)
                //                  .GroupBy(t => t.employee_id).Select(g => g.OrderByDescending(t => t.emp_grade_id).FirstOrDefault()).Select(p => new { p.employee_id, p.tbl_designation_master.designation_name }).ToList();



                var data = _context.tbl_assets_request_master.OrderByDescending(y => y.asset_req_id).Where(q => q.company_id == companyid && q.req_employee_id == EmpID && q.is_deleted == 0 && q.is_active == 1
                && q.is_closed == 0).GroupBy(t => new { t.req_employee_id, t.assets_master_id }).Select(p => p.OrderByDescending(t => t.asset_req_id).FirstOrDefault()).ToList();


                var emp_data = (from e in _context.tbl_assets_master
                                join d in data on e.asset_master_id equals d.assets_master_id into ej
                                from d in ej.DefaultIfEmpty()
                                where e.is_deleted == 0 && e.company_id == companyid
                                select new
                                {
                                    e.asset_master_id,
                                    e.asset_name,
                                    e.description,
                                    empidd = d == null ? 0 : d.req_employee_id,
                                    from_date = d == null ? Convert.ToDateTime("01-01-2000") : d.from_date,
                                    to_date = d == null ? Convert.ToDateTime("01-01-2000") : d.to_date,
                                    asset_no = d == null ? "" : d.asset_number,
                                    asset_type = d == null ? 0 : d.asset_type,
                                    req_remarks = d == null ? "" : d.description,
                                    created_date = d == null ? Convert.ToDateTime("01-01-2000") : d.created_dt,
                                    modified_date = d == null ? Convert.ToDateTime("01-01-2000") : d.modified_dt,
                                    is_final_approve = d == null ? 0 : d.is_finalapprove,
                                    issue_date = d == null ? Convert.ToDateTime("01-01-2000") : d.asset_issue_date,
                                    submission_date = d == null ? Convert.ToDateTime("01-01-2000") : d.submission_date,
                                    is_closed = d == null ? 0 : d.is_closed,
                                    emp_name_code = d == null ? "-" : string.Format("{0} {1} {2} ({3})",
                                                (empdtl != null ? empdtl.employee_first_name : ""), (empdtl != null ? empdtl.employee_middle_name : ""), (empdtl != null ? empdtl.employee_last_name : ""),
                                    d.emp_master.emp_code),
                                    is_permanent = d == null ? 0 : d.is_permanent,
                                    // next_action_ = d == null ? "0,1,2" : d.is_finalapprove != 1 ? "0,1,2" : d.asset_type == 0 ? "1,2" : d.asset_type == 1 ? "2" : d.asset_type == 2 ? "0,1,2" : "",
                                    next_action_ = d == null ? "0,1,2" : d.is_finalapprove != 1 ? "0,1,2" : d.asset_type == 0 ? "1,2" : d.asset_type == 1 ? "2" : d.asset_type == 2 ? "2" : "0,1,2",
                                    asset_req_id = d == null ? 0 : d.asset_req_id,
                                }).OrderByDescending(y => y.asset_master_id).ToList();

                return Ok(emp_data);





                //if (_clsCurrentUser.Is_Hod==1)
                //{
                //    var manager_emp_list = _context.tbl_emp_manager.Where(a =>(a.m_one_id == EmpID || a.m_two_id == EmpID || a.m_three_id == EmpID ) && a.is_deleted == 0).Select(p => p.employee_id).Distinct().ToList();

                //    if (for_all_emp == 1)
                //    {
                //        var data = _context.tbl_assets_request_master.OrderByDescending(x=>x.asset_req_id).Where(q =>q.company_id==companyid && q.req_employee_id == EmpID && q.is_deleted == 0 && q.is_active==1 && q.is_closed==0 && q.is_finalapprove!=2).ToList();
                //        var emp_data = (from e in _context.tbl_assets_master
                //                        join d in data on e.asset_master_id equals d.assets_master_id into ej
                //                        from d in ej.DefaultIfEmpty() where e.is_deleted==0 && e.company_id==companyid
                //                        select new
                //                        {
                //                            e.asset_master_id,
                //                            e.asset_name,
                //                            e.description,
                //                            empidd = d == null ? 0 : d.req_employee_id,
                //                            from_date = d == null ? Convert.ToDateTime("01-01-2000") : d.from_date,
                //                            to_date = d == null ? Convert.ToDateTime("01-01-2000") : d.to_date,
                //                            asset_no = d == null ? "" : d.asset_number,
                //                            asset_type = d == null ? 0 : d.asset_type,
                //                            req_remarks = d == null ? "" : d.description,
                //                            created_date = d == null ? Convert.ToDateTime("01-01-2000") : d.created_dt,
                //                            modified_date = d == null ? Convert.ToDateTime("01-01-2000") : d.modified_dt,
                //                            is_final_approve = d == null ? 0 : d.is_finalapprove,
                //                            issue_date = d == null ? Convert.ToDateTime("01-01-2000") : d.asset_issue_date,
                //                            submission_date = d == null ? Convert.ToDateTime("01-01-2000") : d.submission_date,
                //                            is_closed = d == null ? 0 : d.is_closed,
                //                            emp_name_code = d == null ? "-" : string.Format("{0} {1} {2} ({3})",
                //                            d.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name) && x.employee_id == d.req_employee_id).employee_first_name,
                //                            d.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name) && x.employee_id == d.req_employee_id).employee_middle_name,
                //                            d.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name) && x.employee_id == d.req_employee_id).employee_last_name,
                //                            d.emp_master.emp_code),
                //                            is_permanent = d == null ? 0 : d.is_permanent,
                //                            next_action_=d==null?"0,1,2":d.is_finalapprove!=1 && d.asset_type==0?"0,1,2":d.asset_type==0?"1,2":d.asset_type==1?"2":d.asset_type==2?"0,1,2":"",
                //                            asset_req_id = d == null ? 0 : d.asset_req_id,
                //                        }).OrderByDescending(y=>y.asset_master_id).ToList();


                //        return Ok(emp_data);
                //    }
                //    else
                //    {
                //        var data = _context.tbl_assets_request_master.OrderByDescending(y=>y.asset_req_id).Where(q => q.company_id==companyid && ((manager_emp_list.Contains(q.req_employee_id))||q.req_employee_id== _emp_id) && q.is_deleted == 0 && q.is_active==1 && q.is_closed == 0 && q.is_finalapprove != 2).ToList();
                //        var emp_data = (from e in _context.tbl_assets_master
                //                        join d in data on e.asset_master_id equals d.assets_master_id into ej
                //                        from d in ej.DefaultIfEmpty() where e.is_deleted==0 && e.company_id==companyid
                //                        select new
                //                        {
                //                            e.asset_master_id,
                //                            e.asset_name,
                //                            e.description,
                //                            empidd = d == null ? 0 : d.req_employee_id,
                //                            from_date = d == null ? Convert.ToDateTime("01-01-2000") : d.from_date,
                //                            to_date = d == null ? Convert.ToDateTime("01-01-2000") : d.to_date,
                //                            asset_no = d == null ? "" : d.asset_number,
                //                            asset_type = d == null ? 0 : d.asset_type,
                //                            req_remarks = d == null ? "" : d.description,
                //                            created_date = d == null ? Convert.ToDateTime("01-01-2000") : d.created_dt,
                //                            modified_date = d == null ? Convert.ToDateTime("01-01-2000") : d.modified_dt,
                //                            is_final_approve = d == null ? 0 : d.is_finalapprove,
                //                            issue_date = d == null ? Convert.ToDateTime("01-01-2000") : d.asset_issue_date,
                //                            submission_date = d == null ? Convert.ToDateTime("01-01-2000") : d.submission_date,
                //                            is_closed = d == null ? 0 : d.is_closed,
                //                            emp_name_code = d == null ? "-" : string.Format("{0} {1} {2} ({3})",
                //                            d.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name) && x.employee_id == d.req_employee_id).employee_first_name,
                //                            d.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name) && x.employee_id == d.req_employee_id).employee_middle_name,
                //                            d.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name) && x.employee_id == d.req_employee_id).employee_last_name,
                //                            d.emp_master.emp_code),
                //                            is_permanent = d == null ? 0 : d.is_permanent,
                //                            next_action_ = d == null ? "0,1,2" : d.is_finalapprove != 1 ? "0,1,2" : d.asset_type == 0 ? "1,2" : d.asset_type == 1 ? "2" : d.asset_type == 2 ? "0,1,2" : "",
                //                           asset_req_id=d==null?0:d.asset_req_id,
                //                        }).OrderByDescending(y=>y.asset_master_id).ToList();


                //        return Ok(emp_data);



                //    }

                //}
                //else if (ob.is_Admin == 1)
                //{
                //    //verify companyid
                //    int company_idd = 0;
                //   var exist_comp = _context.tbl_employee_company_map.Where(x => x.is_deleted == 0 && x.company_id == companyid).FirstOrDefault();
                //    if (exist_comp != null)
                //    {
                //        company_idd = exist_comp.company_id??0;
                //    }
                //    if (company_idd != 0 && company_idd == companyid)
                //    {
                //        if (for_all_emp == 1)
                //        {
                //            var data = _context.tbl_assets_request_master.OrderByDescending(y=>y.asset_req_id).Where(q =>q.company_id==companyid && q.req_employee_id == EmpID && q.is_deleted == 0 && q.is_active==1 && q.is_closed == 0 && q.is_finalapprove != 2).ToList();
                //            var emp_data = (from e in _context.tbl_assets_master
                //                            join d in data on e.asset_master_id equals d.assets_master_id into ej
                //                            from d in ej.DefaultIfEmpty() where e.is_deleted==0 && e.company_id==companyid
                //                            select new
                //                            {
                //                                e.asset_master_id,
                //                                e.asset_name,
                //                                e.description,
                //                                empidd = d == null ? 0 : d.req_employee_id,
                //                                from_date = d == null ? Convert.ToDateTime("01-01-2000") : d.from_date,
                //                                to_date = d == null ? Convert.ToDateTime("01-01-2000") : d.to_date,
                //                                asset_no = d == null ? "" : d.asset_number,
                //                                asset_type = d == null ? 0 : d.asset_type,
                //                                req_remarks = d == null ? "" : d.description,
                //                                created_date = d == null ? Convert.ToDateTime("01-01-2000") : d.created_dt,
                //                                modified_date = d == null ? Convert.ToDateTime("01-01-2000") : d.modified_dt,
                //                                is_final_approve = d == null ? 0 : d.is_finalapprove,
                //                                issue_date = d == null ? Convert.ToDateTime("01-01-2000") : d.asset_issue_date,
                //                                submission_date = d == null ? Convert.ToDateTime("01-01-2000") : d.submission_date,
                //                                is_closed = d == null ? 0 : d.is_closed,
                //                                emp_name_code = d == null ? "-" : string.Format("{0} {1} {2} ({3})",
                //                                d.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name) && x.employee_id == d.req_employee_id).employee_first_name,
                //                                d.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name) && x.employee_id == d.req_employee_id).employee_middle_name,
                //                                d.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name) && x.employee_id == d.req_employee_id).employee_last_name,
                //                                d.emp_master.emp_code),
                //                                is_permanent = d == null ? 0 : d.is_permanent,
                //                                next_action_ = d == null ? "0,1,2" : d.is_finalapprove != 1 ? "0,1,2" : d.asset_type == 0 ? "1,2" : d.asset_type == 1 ? "2" : d.asset_type == 2 ? "0,1,2" : "",
                //                                asset_req_id = d == null ? 0 : d.asset_req_id,
                //                            }).OrderByDescending(y=>y.asset_master_id).ToList();

                //            return Ok(emp_data);
                //        }
                //        else
                //        {
                //            var data = _context.tbl_assets_request_master.OrderByDescending(y=>y.asset_req_id).Where(q => q.company_id == companyid && q.is_deleted == 0 && q.is_active==1 && q.is_closed == 0 && q.is_finalapprove != 2).ToList();

                //            var emp_data = (from e in _context.tbl_assets_master
                //                            join d in data on e.asset_master_id equals d.assets_master_id into ej
                //                            from d in ej.DefaultIfEmpty()
                //                            where e.is_deleted == 0 && e.company_id==companyid
                //                            select new
                //                            {
                //                                e.asset_master_id,
                //                                e.asset_name,
                //                                e.description,
                //                                empidd = d == null ? 0 : d.req_employee_id,
                //                                from_date = d == null ? Convert.ToDateTime("01-01-2000") : d.from_date,
                //                                to_date = d == null ? Convert.ToDateTime("01-01-2000") : d.to_date,
                //                                asset_no = d == null ? "" : d.asset_number,
                //                                asset_type = d == null ? "-" : d.asset_type==0?"New":d.asset_type==1?"Replace":d.asset_type==2?"Submission":"",
                //                                req_remarks = d == null ? "" : d.description,
                //                                created_date = d == null ? Convert.ToDateTime("01-01-2000") : d.created_dt,
                //                                modified_date = d == null ? Convert.ToDateTime("01-01-2000") : d.modified_dt,
                //                                is_final_approve = d == null ? "-" : d.is_finalapprove==0?"Pending":d.is_finalapprove==1?"Approve":d.is_finalapprove==2?"Reject":d.is_finalapprove==3?"In Process":"",
                //                                issue_date = d == null ? Convert.ToDateTime("01-01-2000") : d.asset_issue_date,
                //                                replace_date=d==null? Convert.ToDateTime("01-01-2000") : d.replace_dt,
                //                                submission_date = d == null ? Convert.ToDateTime("01-01-2000") : d.submission_date,
                //                                is_closed = d == null ? 0 : d.is_closed,
                //                                emp_name_code = d == null ? "-" : string.Format("{0} {1} {2} ({3})",
                //                                d.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name) && x.employee_id == d.req_employee_id).employee_first_name,
                //                                d.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name) && x.employee_id == d.req_employee_id).employee_middle_name,
                //                                d.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name) && x.employee_id == d.req_employee_id).employee_last_name,
                //                                d.emp_master.emp_code),
                //                                is_permanent = d == null ? 0 : d.is_permanent,
                //                                next_action_ = d == null ? "0,1,2" : d.is_finalapprove != 1 ? "0,1,2" : d.asset_type == 0 ? "1,2" : d.asset_type == 1 ? "2" : d.asset_type == 2 ? "0,1,2" : "",
                //                                asset_req_id = d == null ? 0 : d.asset_req_id,
                //                            }).OrderByDescending(x => x.asset_master_id).ToList();

                //            return Ok(emp_data);
                //        }


                //    }
                //    else
                //    {
                //        objResult.StatusCode = 1;
                //        objResult.Message = "Invalid Company Details";
                //        return Ok(objResult);
                //    }




                //}
                //else
                //{
                //    var data = _context.tbl_assets_request_master.OrderByDescending(x=>x.asset_req_id).Where(q =>q.company_id==companyid && q.req_employee_id == EmpID && q.is_deleted == 0 && q.is_active==1 && q.is_closed == 0 && q.is_finalapprove != 2).ToList();

                //    var emp_data = (from e in _context.tbl_assets_master
                //                    join d in data on e.asset_master_id equals d.assets_master_id into ej
                //                    from d in ej.DefaultIfEmpty() where e.is_deleted==0 && e.company_id==companyid
                //                    select new
                //                    {
                //                        e.asset_master_id,
                //                        e.asset_name,
                //                        e.description,
                //                        empidd = d == null ? 0 : d.req_employee_id,
                //                        from_date = d == null ? Convert.ToDateTime("01-01-2000") : d.from_date,
                //                        to_date = d == null ? Convert.ToDateTime("01-01-2000") : d.to_date,
                //                        asset_no = d == null ? "" : d.asset_number,
                //                        asset_type = d == null ? 0 : d.asset_type,
                //                        req_remarks = d == null ? "" : d.description,
                //                        created_date = d == null ? Convert.ToDateTime("01-01-2000") : d.created_dt,
                //                        modified_date = d == null ? Convert.ToDateTime("01-01-2000") : d.modified_dt,
                //                        is_final_approve = d == null ? 0 : d.is_finalapprove,
                //                        issue_date = d == null ? Convert.ToDateTime("01-01-2000") : d.asset_issue_date,
                //                        submission_date = d == null ? Convert.ToDateTime("01-01-2000") : d.submission_date,
                //                        is_closed = d == null ? 0 : d.is_closed,
                //                        emp_name_code = d==null?"-":string.Format("{0} {1} {2} ({3})",
                //                            d.emp_master.tbl_emp_officaial_sec.FirstOrDefault(h => h.is_deleted == 0 && !string.IsNullOrEmpty(h.employee_first_name) && h.employee_id==d.req_employee_id).employee_first_name,
                //                            d.emp_master.tbl_emp_officaial_sec.FirstOrDefault(h => h.is_deleted == 0 && !string.IsNullOrEmpty(h.employee_first_name) && h.employee_id == d.req_employee_id).employee_middle_name,
                //                            d.emp_master.tbl_emp_officaial_sec.FirstOrDefault(h => h.is_deleted == 0 && !string.IsNullOrEmpty(h.employee_first_name) && h.employee_id == d.req_employee_id).employee_last_name,
                //                            d.emp_master.emp_code),
                //                        is_permanent=d==null?0: d.is_permanent,
                //                        next_action_ = d == null ? "0,1,2" : d.asset_type==0 && d.is_finalapprove==0?"0,1,2":d.asset_type==0 && d.is_finalapprove==1?"1,2":d.asset_type==1 && d.is_finalapprove==0?"1,2":d.asset_type==1&& d.is_finalapprove==1?"2":d.asset_type==2&& d.is_finalapprove==0?"2":d.asset_type==2&& d.is_finalapprove==1?"0,1,2":"",
                //                        asset_req_id = d == null ? 0 : d.asset_req_id,
                //                    }).OrderByDescending(x=>x.asset_master_id).ToList();


                //    return Ok(emp_data);

                //}

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

#endregion ** STARTED BY SUPRIYA ON 24-07-2019

        [HttpGet("Get_SalaryGroupMapEmpDataByComID/{company_id}")]
        //[Authorize(Policy = "8115")]
        public IActionResult Get_SalaryGroupMapEmpDataByComID([FromRoute] int company_id)
        {
            var result = _context.tbl_sg_maping.Where(a => a.tem.tbl_employee_company_map.FirstOrDefault(q => q.is_deleted == 0).company_id == company_id && a.is_active == 1).Select(b => new
            {
                b.map_id,
                emp_id = b.emp_id,
                default_company_id = b.tem.tbl_employee_company_map.FirstOrDefault(q => q.is_deleted == 0).company_id,
                company_name = b.tem.tbl_employee_company_map.FirstOrDefault(q => q.is_deleted == 0).tbl_company_master.company_name,
                emp_name_code = string.Format("{0} {1} {2} ({3})", b.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && q.employee_first_name != "").employee_first_name,
                         b.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && q.employee_middle_name != "").employee_middle_name,
                         b.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && q.employee_last_name != "").employee_last_name,
                         b.tem.emp_code),
                // b.tem.tbl_emp_grade_allocation.OrderByDescending(x=>x.emp_grade_id).FirstOrDefault(x=>x.grade_id!=null).grade_id,
                b.tem.tbl_emp_grade_allocation.FirstOrDefault(x => x.tbl_grade_master.is_active == 1).grade_id,
                b.tem.tbl_emp_grade_allocation.FirstOrDefault(x => x.tbl_grade_master.is_active == 1).tbl_grade_master.grade_name,
                group_name = b.tsg.group_name,
                //grade_Id = b.tsg.grade_Id,
                //b.tsg.tgm.grade_name,
                salary_group_id = b.salary_group_id,
                effective_dt = b.applicable_from_dt

            }).ToList();

            return Ok(result);
        }

        [HttpGet("Get_OTRateMasterByCompID/{company_id}")]
        //[Authorize(Policy = "8116")]
        public IActionResult Get_OTRateMasterByCompID([FromRoute] int company_id)
        {
            var result = (from a in _context.tbl_ot_rate_details
                          join b in _context.tbl_employee_master
                               on a.emp_id equals b.employee_id into Group
                          from m in Group.DefaultIfEmpty()
                          join c in _context.tbl_company_master
                              on a.companyid equals c.company_id
                          join d in _context.tbl_grade_master
                              on a.grade_id equals d.grade_id into Group1
                          from m1 in Group1.DefaultIfEmpty()
                          where a.companyid == company_id && a.is_active == 1
                          select new
                          {
                              a.ot_id,
                              a.ot_amt,
                              a.emp_id,
                              emp_code = a.emp_id == null || m.emp_code == null ? "All" : m.emp_code,
                              c.company_id,
                              c.company_name,
                              a.grade_id,
                              grade_name = a.grade_id == null || m1.grade_name == null ? "NA" : m1.grade_name,
                              a.created_dt,
                              a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.employee_id == a.emp_id && q.is_deleted == 0).employee_first_name,
                              a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.employee_id == a.emp_id && q.is_deleted == 0).employee_middle_name,
                              a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(q => q.employee_id == a.emp_id && q.is_deleted == 0).employee_last_name
                          }).AsEnumerable().Select((e, index) => new
                          {
                              employee_name_code = e.employee_first_name + " " + e.employee_middle_name + " " + e.employee_last_name + "(" + e.emp_code + ")",
                              ot_amt = e.ot_amt,
                              ot_id = e.ot_id,
                              grade_name = e.grade_name,
                              created_dt = e.created_dt,
                              company_id = e.company_id,
                              emp_id = e.emp_id,
                              company_name = e.company_name,
                              grade_id = e.grade_id
                          }).ToList();
            return Ok(result);
        }


        [Route("Get_Salary_input_Report_by_monthyearandcompID/{monthyear}/{company_id}/{rpt_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.PayrollReport))]

        public IActionResult Get_Salary_input_Report_by_monthyearandcompID([FromRoute] int monthyear, int company_id, int rpt_id)
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
                // monthyear = monthyear + 1; // add month


                List<tbl_salary_input> objlist = new List<tbl_salary_input>();

                var salary_input_changedata = _context.tbl_salary_input_change.Where(x => x.is_active == 1 && Convert.ToInt32(x.monthyear) == monthyear && x.company_id == company_id).
                 Select(p => new
                 {
                     p.tem.tbl_user_master.First(a => a.is_active == 1).default_company_id,
                     p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                     p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                     p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                     p.salary_input_id,
                     p.component_id,
                     p.monthyear,
                     p.emp_id,
                     p.values,
                     p.tem.emp_code,
                     p.created_dt,
                     p.modified_dt,
                     p.is_active
                 }).AsEnumerable().Select((e, index) => new
                 {
                     e.emp_code,
                     employee_name = e.employee_first_name + " " + e.employee_middle_name + " " + e.employee_last_name,
                     salary_input_id = e.salary_input_id,
                     component_id = e.component_id,
                     monthyear = e.monthyear,
                     emp_id = e.emp_id,
                     valuess = e.values,
                     created_dt = e.created_dt,
                     modified_dt = e.modified_dt,
                     e.is_active,
                     sno = index + 1
                 }).ToList();


                var salarydata = _context.tbl_salary_input.Where(x => x.is_active == 1 && x.monthyear == monthyear && x.company_id == company_id).
                  Select(p => new
                  {
                      p.tem.tbl_user_master.First(a => a.is_active == 1).default_company_id,
                      p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).employee_first_name,
                      p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).employee_middle_name,
                      p.tem.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0 && !string.IsNullOrEmpty(q.employee_first_name)).employee_last_name,
                      p.salary_input_id,
                      p.component_id,
                      p.monthyear,
                      p.emp_id,
                      p.values,
                      p.tem.emp_code,
                      p.created_dt,
                      p.modified_dt,
                      p.is_active
                  }).AsEnumerable().Select((e, index) => new
                  {
                      e.emp_code,
                      employee_name = e.employee_first_name + " " + e.employee_middle_name + " " + e.employee_last_name,
                      salary_input_id = e.salary_input_id,
                      component_id = e.component_id,
                      monthyear = e.monthyear,
                      emp_id = e.emp_id,
                      valuess = salary_input_changedata.Any(a => a.emp_id == e.emp_id && a.component_id == e.component_id) ? salary_input_changedata.FirstOrDefault(a => a.emp_id == e.emp_id && a.component_id == e.component_id).valuess : e.values,
                      created_dt = e.created_dt,
                      modified_dt = e.modified_dt,
                      e.is_active,
                      sno = index + 1
                  }).ToList();

                var EmpIDData = salarydata.Select(p => new { p.emp_id, p.emp_code, p.employee_name, p.monthyear }).Distinct().ToList();
                //var _componentdata = _context.tbl_rpt_title_master.Where(x => x.is_active == 1 && x.rpt_id==rpt_id && x.tbl_Component_Master.is_active==1 && x.tbl_Component_Master.is_system_key==0).
                //   Select(p => new
                //   {
                //       property_details=p.rpt_title,
                //       p.component_id
                //   }).ToList();

                var _componentdata = _context.tbl_component_master.Where(x => x.is_active == 1 && x.is_payslip == 1).
                    Select(p => new
                    {
                        p.property_details,
                        p.component_id
                    }).ToList();


                var _componentdata_ = _componentdata.ToList();
                var _finaldata = new { salarydata, _componentdata_, EmpIDData };
                return Ok(_finaldata);

            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }




        [Route("Get_LoanRequestMasterByCompID/{s_no}/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LoanConfiguration2))]
        public IActionResult Get_LoanRequestMasterByCompID([FromRoute] int s_no, int company_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unuthorize Company Access...!!";
                return Ok(objresponse);
            }
            try
            {
                if (s_no > 0)
                {
                    var data = _context.tbl_loan_request_master.Where(a => a.sno == s_no).ToList();
                    return Ok(data);
                }
                else
                {
                    var dataa = (from a in _context.tbl_loan_request_master
                                 join b in _context.tbl_company_master on a.companyid equals b.company_id into Group
                                 join c in _context.tbl_grade_master on a.grade_id equals c.grade_id
                                 from m in Group
                                 where a.companyid == company_id
                                 select new
                                 {
                                     a.sno,
                                     a.loan_type,
                                     a.em_status,
                                     a.loan_amount,
                                     a.rate_of_interest,
                                     a.on_salary,
                                     a.max_tenure,
                                     a.min_top_up_duration,
                                     a.created_dt,
                                     a.last_modified_date,
                                     m.company_name,
                                     c.grade_name,
                                     a.companyid
                                 }).OrderByDescending(g => g.sno).ToList();

                    return Ok(dataa);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message.ToString());
            }
        }


        [Route("Get_LoanApprovalSettingByCompID/{sno}/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LoanApprovalSettingMaster))]
        public IActionResult Get_LoanApprovalSettingByCompID([FromRoute] int sno, int company_id)
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
                if (sno > 0)
                {
                    var result = _context.tbl_loan_approval_setting.Join(_context.tbl_employee_master, a => a.emp_id, b => b.employee_id, (a, b) => new
                    {
                        a.Sno,
                        a.emp_id,
                        b.emp_code,
                        order_id = a.order,
                        order = a.order == 1 ? "Approval 1" : a.order == 2 ? "Approval 2" : a.order == 3 ? "Approval 3" : a.order == 4 ? "Approval 4" : "Approval 5",
                        b.is_active,
                        a.created_by,
                        a.created_dt,

                    }).Join(_context.tbl_user_master, a => a.emp_id, b => b.employee_id, (a, b) => new
                    {
                        a.Sno,
                        a.emp_id,
                        a.emp_code,
                        a.order_id,
                        a.order,
                        b.is_active,
                        a.created_by,
                        a.created_dt,
                        b.default_company_id,

                    }).Where(c => c.Sno == sno && c.is_active == 1).FirstOrDefault();
                    return Ok(result);
                }
                else
                {
                    var result = (from e in _context.tbl_loan_approval_setting
                                  join d in _context.tbl_employee_master on e.emp_id equals d.employee_id into jointable
                                  from empidd in jointable.DefaultIfEmpty()
                                  join g in _context.tbl_role_master on e.approver_role_id equals g.role_id into joinntable2
                                  from approverroldid in joinntable2.DefaultIfEmpty()
                                  where e.company_id == company_id
                                  orderby e.Sno
                                  select new
                                  {
                                      e.Sno,
                                      e.emp_id,
                                      e.order,
                                      e.is_active,
                                      e.created_dt,
                                      e.last_modified_date,
                                      e.is_final_approver,
                                      e.approver_role_id,
                                      e.company_id,
                                      e.company_master.company_name,
                                      e.approver_type,
                                      approver_emp_name_code = string.Format("{0} {1} {2} ({3})",
                                                                empidd.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_first_name,
                                                                empidd.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_middle_name,
                                                                empidd.tbl_emp_officaial_sec.FirstOrDefault(q => q.is_deleted == 0).employee_last_name,
                                                                empidd.emp_code),
                                      approverroldid.role_name
                                  }).ToList();

                    return Ok(result);

                }

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Get_LoanRequestDetailsByCompIDD/{login_emp_id}/{login_emp_role_id}/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.LoanApproval))]
        public IActionResult Get_LoanRequestDetailsByCompIDD([FromRoute] int login_emp_id, int login_emp_role_id, int company_id)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(login_emp_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!!";
                return Ok(objResult);
            }

            if (!_clsCurrentUser.CompanyId.Contains(company_id))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Company Access..!!";
                return Ok(objResult);
            }
            try
            {
                var result = _context.tbl_loan_approval.Join(_context.tbl_loan_approval_setting, a => a.approval_order, b => b.order, (a, b) => new
                {

                    a.sno,
                    a.loan_req_id,
                    a.emp_id,
                    a.tbl_loan_request.emp_code,
                    //a.is_final_approver,
                    a.tbl_loan_request.is_final_approval,
                    a.is_approve,
                    a.approval_order,
                    a.created_dt,
                    a.last_modified_date,
                    a.tbl_loan_request.loan_type,
                    a.tbl_loan_request.loan_amt,
                    a.tbl_loan_request.interest_rate,
                    a.tbl_loan_request.loan_tenure,
                    a.tbl_loan_request.loan_purpose,
                    a.tbl_loan_request.start_date,
                    approver_emp_id = b.emp_id,
                    a.is_deleted,
                    a.tbl_loan_request.is_closed,
                    b.approver_type,
                    b.approver_role_id,
                    a.tbl_employee_master.tbl_employee_company_map.FirstOrDefault(z => z.is_deleted == 0).company_id

                }).Where(x => x.is_deleted == 0 && x.approver_type == 1 && x.company_id == company_id && (x.approver_emp_id == _clsCurrentUser.EmpId || _clsCurrentUser.RoleId.Contains(x.approver_role_id ?? 0))).OrderBy(y => y.sno).ToList();



                List<LoanRequest> objrequestlist = new List<LoanRequest>();

                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        bool containsItem = objrequestlist.Any(x => x.loan_req_id == item.loan_req_id);
                        if (!containsItem)
                        {

                            LoanRequest objrequest = new LoanRequest();

                            objrequest.sno = item.sno;
                            objrequest.loan_req_id = Convert.ToInt32(item.loan_req_id);
                            objrequest.emp_id = Convert.ToInt32(item.emp_id);
                            objrequest.emp_code = item.emp_code;
                            objrequest.is_final_approver = item.is_final_approval;
                            objrequest.is_approve = item.is_approve;
                            objrequest.approval_order = item.approval_order;
                            objrequest.created_dt = item.created_dt;
                            objrequest.last_modified_date = item.last_modified_date;
                            objrequest.loan_type = item.loan_type;
                            objrequest.loan_amt = item.loan_amt;
                            objrequest.interest_rate = item.interest_rate;
                            objrequest.loan_tenure = item.loan_tenure;
                            objrequest.loan_purpose = item.loan_purpose;
                            objrequest.start_date = item.start_date;
                            objrequest.approver_emp_id = item.approver_emp_id;
                            objrequest.is_deleted = item.is_deleted;
                            objrequest.is_closed = item.is_closed;
                            objrequest.approver_type = item.approver_type;
                            objrequest.approver_role_id = item.approver_role_id;



                            objrequestlist.Add(objrequest);
                        }
                        else
                        {
                            if (objrequestlist.Where(a => a.loan_req_id == item.loan_req_id && a.is_approve == 1).Count() > 0)
                            {
                                objrequestlist.Remove(objrequestlist.Single(s => s.loan_req_id == item.loan_req_id));

                                LoanRequest objrequest = new LoanRequest();

                                objrequest.sno = item.sno;
                                objrequest.loan_req_id = Convert.ToInt32(item.loan_req_id);
                                objrequest.emp_id = Convert.ToInt32(item.emp_id);
                                objrequest.emp_code = item.emp_code;
                                objrequest.is_final_approver = item.is_final_approval;
                                objrequest.is_approve = item.is_approve;
                                objrequest.approval_order = item.approval_order;
                                objrequest.created_dt = item.created_dt;
                                objrequest.last_modified_date = item.last_modified_date;
                                objrequest.loan_type = item.loan_type;
                                objrequest.loan_amt = item.loan_amt;
                                objrequest.interest_rate = item.interest_rate;
                                objrequest.loan_tenure = item.loan_tenure;
                                objrequest.loan_purpose = item.loan_purpose;
                                objrequest.start_date = item.start_date;
                                objrequest.approver_emp_id = item.approver_emp_id;
                                objrequest.is_deleted = item.is_deleted;
                                objrequest.is_closed = item.is_closed;
                                objrequest.approver_type = item.approver_type;
                                objrequest.approver_role_id = item.approver_role_id;



                                objrequestlist.Add(objrequest);
                            }
                        }

                    }
                }


                return Ok(objrequestlist);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [Route("Get_PendingAssetRequestDetailByCompIDD/{login_emp_id}/{login_role_id}/{company_idd}")]
        [HttpGet]
        //[Authorize(Policy = "8121")]
        public IActionResult Get_PendingAssetRequestDetailByCompIDD([FromRoute] int login_emp_id, int login_role_id, int company_idd)
        {
            try
            {
                Response_Msg objResult = new Response_Msg();

                Classes.clsLoginEmpDtl ob = new clsLoginEmpDtl(_context, Convert.ToInt32(login_emp_id), "Read", _AC);
                if (!ob.is_valid())
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Unauthorize Access...!";
                    return Ok(objResult);
                }

                var data = _context.tbl_assets_approval.Join(_context.tbl_loan_approval_setting, a => a.approval_order, b => b.order, (a, b) => new
                {
                    a.asset_approval_sno,
                    a.asset_req_id,
                    a.approval_order,
                    b.order,
                    approver_emp_id = b.emp_id,
                    approver_role_id = b.approver_role_id,
                    a.asset_req_mastr.is_active,
                    a.asset_req_mastr.is_deleted,
                    a.asset_req_mastr.asset_name,
                    a.created_dt,
                    a.asset_req_mastr.description,
                    a.is_approve,
                    a.asset_req_mastr.is_finalapprove,
                    a.emp_id,
                    req_emp_name_code = string.Format("{0} {1} {2} ({3})",
                             a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0 && p.employee_first_name != "").employee_first_name,
                             a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0 && p.employee_middle_name != "").employee_middle_name,
                             a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0 && p.employee_last_name != "").employee_last_name,
                             a.employee_master.emp_code),
                    a.employee_master.tbl_employee_company_map.FirstOrDefault(g => g.is_deleted == 0).tbl_company_master.company_name,
                    a.asset_req_mastr.asset_issue_date,
                    a.asset_req_mastr.is_closed,
                    b.approver_type,
                    a.employee_master.tbl_employee_company_map.FirstOrDefault(z => z.is_deleted == 0).company_id,
                    approver_active = b.is_active

                }).Where(x => x.approver_type == 2 && x.approver_active == 1 && x.company_id == company_idd && (x.approver_role_id == login_role_id || x.approver_emp_id == login_emp_id) && x.is_deleted == 0
                ).OrderByDescending(x => x.asset_req_id).ToList();


                List<AssetRequest> objrequestlist = new List<AssetRequest>();

                if (data.Count > 0)
                {
                    foreach (var item in data)
                    {
                        bool containsItem = objrequestlist.Any(x => x.asset_req_id == item.asset_req_id);

                        if (!containsItem)
                        {
                            AssetRequest objrequest = new AssetRequest();

                            objrequest.asset_req_id = Convert.ToInt32(item.asset_req_id);
                            objrequest.approval_order = item.approval_order;
                            objrequest.approver_emp_id = item.approver_emp_id;
                            objrequest.approver_role_id = item.approver_role_id;
                            objrequest.asset_name = item.asset_name;
                            objrequest.created_dt = item.created_dt;
                            objrequest.asset_description = item.description;
                            objrequest.is_approve = item.is_approve;
                            objrequest.is_final_approver = Convert.ToByte(item.is_finalapprove);
                            objrequest.emp_id = item.emp_id;
                            objrequest.emp_name_code = item.req_emp_name_code;
                            objrequest.company_name = item.company_name;
                            objrequest.asset_issue_date = item.asset_issue_date;
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
                                objrequest.asset_name = item.asset_name;
                                objrequest.created_dt = item.created_dt;
                                objrequest.asset_description = item.description;
                                objrequest.is_approve = item.is_approve;
                                objrequest.is_final_approver = Convert.ToByte(item.is_finalapprove);
                                objrequest.emp_id = item.emp_id;
                                objrequest.emp_name_code = item.req_emp_name_code;
                                objrequest.company_name = item.company_name;
                                objrequest.asset_issue_date = item.asset_issue_date;
                                objrequest.is_closed = item.is_closed;
                                objrequest.approver_type = item.approver_type;


                                objrequestlist.Add(objrequest);
                            }
                        }

                    }

                }

                return Ok(objrequestlist);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

#region ** STARTED BY SUPRIYA ON 19-08-2019,RATE OF WAGES AND RATE OF DEDUCTION**

        [Route("Save_RegisterOfFinesFormIMaster")]
        [HttpPost]
        //[Authorize(Policy = "8122")]
        public IActionResult Save_RegisterOfFinesFormIMaster([FromBody] tbl_muster_form1 objtbl)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                objtbl.is_deleted = 0;
                objtbl.created_date = DateTime.Now;

                _context.Entry(objtbl).State = EntityState.Added;
                _context.SaveChanges();

                objresponse.StatusCode = 0;
                objresponse.Message = "Successfully Saved...!";


                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Get_RegisterOfFinesFormIMaster/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8123")]
        public IActionResult Get_RegisterOfFinesFormIMaster(int company_id)
        {
            try
            {
                var data = _context.tbl_muster_form1.Where(x => x.is_deleted == 0 && x.comp_id == company_id).OrderByDescending(x => x.form_I_mstr_id).FirstOrDefault();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Get_DeductionComponent/{component_type}/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8124")]
        public IActionResult Get_DeductionComponent([FromRoute] int component_type, int company_id)
        {
            try
            {
                var data = _context.tbl_component_formula_details.Where(x => x.comp_master.component_type == component_type && x.company_id == company_id && x.comp_master.is_active == 1).Select(p => new
                {

                    p.comp_master.component_id,
                    p.comp_master.property_details
                }).Distinct().ToList();

                // var data = _context.tbl_component_master.Where(x => x.component_type == component_type && x.is_active == 1).OrderByDescending(y => y.component_id).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Save_RegisterofDeductionFormIIMaster")]
        [HttpPost]
        //[Authorize(Policy = "8125")]
        public IActionResult Save_RegisterofDeductionFormIIMaster([FromBody] tbl_muster_form2 objtbl)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                objtbl.is_deleted = 0;
                objtbl.created_date = DateTime.Now;

                _context.Entry(objtbl).State = EntityState.Added;
                _context.SaveChanges();


                objresponse.StatusCode = 0;
                objresponse.Message = "Successfully Saved...!";

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Get_RegisterOfDeductionFormIIMaster/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8126")]
        public IActionResult Get_RegisterOfDeductionFormIIMaster(int company_id)
        {
            try
            {
                var data = _context.tbl_muster_form2.Where(x => x.is_deleted == 0 && x.comp_id == company_id).OrderByDescending(x => x.form_II_mstr_id).FirstOrDefault();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        //[Route("Save_RateofDeduction")]
        //[HttpPost]
        //public IActionResult Save_RateofDeduction([FromBody] tbl_rate_ofdeduction objtbl)
        //{
        //    try
        //    {
        //        Response_Msg objresponse = new Response_Msg();
        //        var data = _context.tbl_rate_ofdeduction.Where(x =>x.comp_id==objtbl.comp_id && x.deduction_id == objtbl.deduction_id && x.is_deleted == 0).OrderByDescending(y=>y.rate_id).FirstOrDefault();
        //        if (data != null)
        //        {
        //            objresponse.StatusCode = 1;
        //            objresponse.Message = "Rate of Deduction Already Saved";
        //        }
        //        else
        //        {
        //            objtbl.created_date = DateTime.Now;
        //            objtbl.is_deleted = 0;

        //            _context.Entry(objtbl).State = EntityState.Added;
        //            _context.SaveChanges();

        //            objresponse.StatusCode = 0;
        //            objresponse.Message = "Rate of Deduction Successfully Saved";
        //        }

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}

        //[Route("Get_RateofDeudction")]
        //[HttpGet]
        //public IActionResult Get_RateofDeudction()
        //{
        //    try
        //    {
        //        var data = _context.tbl_rate_ofdeduction.Where(x => x.is_deleted == 0).OrderByDescending(y => y.rate_id).Select(p=>new {
        //            p.rate_id,
        //            p.deduction_id,
        //            p.comp_id,
        //            p.company_master.company_name,
        //            p.created_date,
        //            p.modified_date
        //        }).ToList();

        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}

        //[Route("Delete_RateofDeductions")]
        //[HttpPost]
        //public IActionResult Delete_RateofDeductions([FromBody] tbl_rate_ofdeduction objtbl)
        //{
        //    try
        //    {
        //        Response_Msg objresponse = new Response_Msg();
        //        var data = _context.tbl_rate_ofdeduction.Where(x => x.rate_id == objtbl.rate_id && x.comp_id == objtbl.comp_id).FirstOrDefault();
        //        if (data != null)
        //        {
        //            data.is_deleted = 1;
        //            data.modified_by = objtbl.modified_by;
        //            data.modified_date = DateTime.Now;

        //            _context.Entry(data).State = EntityState.Modified;
        //            _context.SaveChanges();

        //            objresponse.StatusCode = 0;
        //            objresponse.Message = "Rate of Deduction Successfully Deleted..";
        //        }
        //        else
        //        {
        //            objresponse.StatusCode = 1;
        //            objresponse.Message = "Invalid Deduction ID/Unable to Delete";
        //        }

        //        return Ok(objresponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}
#endregion ** END BY SUPRIYA ON 19-08-2019,RATE OF WAGES AND RATE OF DEDUCTION**



#region  STARTED BY AMARJEET ON 20-08-2019 The Register of Fines in Form-I, Rule 21(4)

        [Route("GetProcessedPayrollMonthYear/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8127")]
        public IActionResult GetProcessedPayrollMonthYear(int company_id)
        {
            try
            {
                var data = _context.tbl_payroll_process_status.Where(a => a.is_deleted == 0 && a.is_lock == 1 && a.company_id == company_id).Select(a => new { a.payroll_month_year }).Distinct().OrderByDescending(a => a.payroll_month_year).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("GetPayrollDetailsForMusterForm1/{payroll_month}/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8128")]
        public IActionResult GetPayrollDetailsForMusterForm1(int payroll_month, int company_id)
        {
            try
            {
                //Check If payroll_month data already freedz
                var get_freedz_data = _context.tbl_muster_form1_data.Where(a => a.company_id == company_id && a.payroll_month == payroll_month && a.is_deleted == 0)
                    .Select(a => new
                    {
                        a.employee_code,
                        employee_id = a.emp_id,
                        a.company_id,
                        a.payroll_month,
                        employe_name = a.employee_name,
                        father_or_husband = a.father_or_husband_name,
                        a.department,
                        a.nature_and_date,
                        a.whether_workman,
                        a.rate_of_wages,
                        a.date_of_fine_imposed,
                        a.amt_of_fine_imposed,
                        a.date_realised,
                        a.remarks
                    })
                    .ToList();

                if (get_freedz_data.Count > 0)
                {
                    return Ok(new { data = get_freedz_data, flag = 1 });
                }
                else
                {
                    throw new NotImplementedException();
#if false

                    var data = _context.tbl_salary_input.Where(a => a.company_id == company_id && a.monthyear == payroll_month && a.is_active == 1).Select(p => new
                    {

                        employee_code = p.tem.emp_code,
                        employee_id = p.emp_id,
                        company_id = p.company_id,
                        payroll_month = p.monthyear,
                        employe_name = string.Format("{0} {1} {2}", p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                       p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_middle_name,
                       p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_last_name
                       ),
                        father_or_husband = p.tem.tbl_emp_family_sec.FirstOrDefault(h => h.relation == "Father" && h.is_deleted == 0).name_as_per_aadhar_card,
                        department = p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).tbl_department_master.department_name,
                        a_is_active = p.is_active,

                        nature_and_date = _context.tbl_salary_input.Join(_context.tbl_muster_form1, an => an.component_id, anc => anc.nature_and_date_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_I_mstr_id, an.company_id }).OrderByDescending(v => v.form_I_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        whether_workman = _context.tbl_salary_input.Join(_context.tbl_muster_form1, an => an.component_id, anc => anc.whether_workman_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_I_mstr_id, an.company_id }).OrderByDescending(v => v.form_I_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        rate_of_wages = _context.tbl_salary_input.Join(_context.tbl_muster_form1, an => an.component_id, anc => anc.rate_of_wages_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_I_mstr_id, an.company_id }).OrderByDescending(v => v.form_I_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        date_of_fine_imposed = _context.tbl_salary_input.Join(_context.tbl_muster_form1, an => an.component_id, anc => anc.date_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_I_mstr_id, an.company_id }).OrderByDescending(v => v.form_I_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        amt_of_fine_imposed = _context.tbl_salary_input.Join(_context.tbl_muster_form1, an => an.component_id, anc => anc.amt_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_I_mstr_id, an.company_id }).OrderByDescending(v => v.form_I_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        date_realised = _context.tbl_salary_input.Join(_context.tbl_muster_form1, an => an.component_id, anc => anc.date_realised_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_I_mstr_id, an.company_id }).OrderByDescending(v => v.form_I_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        remarks = _context.tbl_salary_input.Join(_context.tbl_muster_form1, an => an.component_id, anc => anc.remarks_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_I_mstr_id, an.company_id }).OrderByDescending(v => v.form_I_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),

                    }).Distinct().ToList();

                    return Ok(new { data = data, flag = 0 });
#endif
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

#if false
        [Route("GetPayrollDetailsForMusterForm1ByEmployee/{payroll_month}/{employee_id}")]
        [HttpGet]
        //[Authorize(Policy = "8129")]
        public IActionResult GetPayrollDetailsForMusterForm1ByEmployee(int payroll_month, int employee_id)
        {
            try
            {

                var data = _context.tbl_muster_form1_data.Join(_context.tbl_emp_officaial_sec, a => a.emp_id, b => b.employee_id, (a, b) => new
                {
                    a.form_i_id,
                    employee_code = b.tbl_employee_id_details.emp_code,
                    employee_id = a.emp_id,
                    company_id = b.tbl_employee_id_details.tbl_user_master.Where(e => e.is_active == 1 && e.employee_id == a.emp_id).Select(e => e.default_company_id).FirstOrDefault(),
                    payroll_month = a.payroll_month,
                    employe_name = a.employee_name,
                    father_or_husband = a.father_or_husband_name,
                    department = b.tbl_department_master.department_name,
                    department_id = b.tbl_department_master.department_id,
                    a_is_active = a.is_deleted,
                    b_is_active = b.is_deleted,
                    nature_and_date = a.nature_and_date,
                    whether_workman = a.whether_workman,
                    rate_of_wages = a.rate_of_wages,
                    date_of_fine_imposed = a.date_of_fine_imposed,
                    amt_of_fine_imposed = a.amt_of_fine_imposed,
                    date_realised = a.date_realised,
                    remarks = a.remarks
                }).Where(a => a.employee_id == employee_id && a.payroll_month == payroll_month && a.a_is_active == 0 && a.b_is_active == 0).Distinct().FirstOrDefault(); ;


                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
#endif

#endregion END

#region START 22-08-2019  The Register of Fines in Form-I, Rule 21(4) ADD

        [Route("Save_PayrollMusterForm1Data")]
        [HttpPost]
        //[Authorize(Policy = "8130")]
        public IActionResult Save_PayrollMusterForm1Data([FromBody] tbl_muster_form1_data Obj_form_data)
        {
            throw new NotImplementedException();
#if false
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var data = _context.tbl_salary_input.Where(a => a.company_id == Obj_form_data.company_id && a.monthyear == Obj_form_data.payroll_month && a.is_active == 1).Select(p => new
                {

                    employee_code = p.tem.emp_code,
                    employee_id = p.emp_id,
                    company_id = p.company_id,
                    payroll_month = p.monthyear,
                    employe_name = string.Format("{0} {1} {2}", p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                     p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_middle_name,
                     p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_last_name
                     ),
                    father_or_husband = p.tem.tbl_emp_family_sec.FirstOrDefault(h => h.relation == "Father" && h.is_deleted == 0).name_as_per_aadhar_card,
                    department = p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).tbl_department_master.department_name,
                    a_is_active = p.is_active,

                    nature_and_date = _context.tbl_salary_input.Join(_context.tbl_muster_form1, an => an.component_id, anc => anc.nature_and_date_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_I_mstr_id, an.company_id }).OrderByDescending(v => v.form_I_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    whether_workman = _context.tbl_salary_input.Join(_context.tbl_muster_form1, an => an.component_id, anc => anc.whether_workman_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_I_mstr_id, an.company_id }).OrderByDescending(v => v.form_I_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    rate_of_wages = _context.tbl_salary_input.Join(_context.tbl_muster_form1, an => an.component_id, anc => anc.rate_of_wages_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_I_mstr_id, an.company_id }).OrderByDescending(v => v.form_I_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    date_of_fine_imposed = _context.tbl_salary_input.Join(_context.tbl_muster_form1, an => an.component_id, anc => anc.date_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_I_mstr_id, an.company_id }).OrderByDescending(v => v.form_I_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    amt_of_fine_imposed = _context.tbl_salary_input.Join(_context.tbl_muster_form1, an => an.component_id, anc => anc.amt_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_I_mstr_id, an.company_id }).OrderByDescending(v => v.form_I_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    date_realised = _context.tbl_salary_input.Join(_context.tbl_muster_form1, an => an.component_id, anc => anc.date_realised_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_I_mstr_id, an.company_id }).OrderByDescending(v => v.form_I_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    remarks = _context.tbl_salary_input.Join(_context.tbl_muster_form1, an => an.component_id, anc => anc.remarks_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_I_mstr_id, an.company_id }).OrderByDescending(v => v.form_I_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),

                }).Distinct().ToList();


                List<tbl_muster_form1_data> objform1data = new List<tbl_muster_form1_data>();

                for (int Index = 0; Index < data.Count; Index++)
                {
                    tbl_muster_form1_data obj = new tbl_muster_form1_data()
                    {
                        emp_id = data[Index].employee_id,
                        company_id = data[Index].company_id,
                        payroll_month = data[Index].payroll_month,
                        employee_code = data[Index].employee_code,
                        employee_name = data[Index].employe_name,
                        father_or_husband_name = data[Index].father_or_husband,
                        department = data[Index].department,
                        nature_and_date = data[Index].nature_and_date,
                        whether_workman = data[Index].whether_workman,
                        rate_of_wages = data[Index].rate_of_wages,
                        date_of_fine_imposed = data[Index].date_of_fine_imposed,
                        amt_of_fine_imposed = data[Index].amt_of_fine_imposed,
                        date_realised = data[Index].date_realised,
                        remarks = data[Index].remarks,
                        is_deleted = 0,
                        created_by = Obj_form_data.created_by,
                        created_date = DateTime.Now
                    };

                    objform1data.Add(obj);
                }


                _context.tbl_muster_form1_data.AddRange(objform1data);
                _context.SaveChangesAsync();

                objresponse.StatusCode = 0;
                objresponse.Message = "Data Successfully Saved...!";


                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
#endif
        }


        [Route("Update_PayrollMusterForm1Data")]
        [HttpPost]
        //[Authorize(Policy = "8131")]
        public IActionResult Update_PayrollMusterForm1Data([FromBody] tbl_muster_form1_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var get_data = _context.tbl_muster_form1_data.Where(a => a.emp_id == Obj_form_data.emp_id && a.form_i_id == Obj_form_data.form_i_id && a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month).FirstOrDefault();

                if (get_data == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid details please try again !!";
                    return Ok(objresponse);
                }
                else
                {
                    get_data.father_or_husband_name = Obj_form_data.father_or_husband_name;
                    get_data.nature_and_date = Obj_form_data.nature_and_date;
                    get_data.whether_workman = Obj_form_data.whether_workman;
                    get_data.rate_of_wages = Obj_form_data.rate_of_wages;
                    get_data.date_of_fine_imposed = Obj_form_data.date_of_fine_imposed;
                    get_data.amt_of_fine_imposed = Obj_form_data.amt_of_fine_imposed;
                    get_data.date_realised = Obj_form_data.date_realised;
                    get_data.remarks = Obj_form_data.remarks;


                    _context.Entry(get_data).State = EntityState.Modified;
                    _context.SaveChanges();

                }

                // var get_emp_data = _context.tbl_emp_officaial_sec.Where(a => a.is_deleted == 0 && a.employee_first_name != "" && a.employee_id == Obj_form_data.emp_id).Select(b => new { emp_name = b.employee_first_name ?? "" + " " + b.employee_middle_name ?? "" + " " + b.employee_last_name ?? "", emp_code = b.tbl_employee_id_details.emp_code, dept = b.tbl_department_master.department_name }).FirstOrDefault();


                objresponse.StatusCode = 0;
                objresponse.Message = "Updated successfully !!";
                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Add_PayrollMusterForm1Data")]
        [HttpPost]
        //[Authorize(Policy = "8132")]
        public IActionResult Add_PayrollMusterForm1Data([FromBody] tbl_muster_form1_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var get_data = _context.tbl_muster_form1_data.Where(a => a.emp_id == Obj_form_data.emp_id && a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month).FirstOrDefault();

                if (get_data != null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Employee details already exist...!";
                    return Ok(objresponse);
                }
                else
                {

                    var get_emp_data = _context.tbl_emp_officaial_sec.Where(a => a.is_deleted == 0 && !string.IsNullOrEmpty(a.employee_first_name) && a.employee_id == Obj_form_data.emp_id && !string.IsNullOrEmpty(a.employee_first_name)).Select(b => new { emp_name = string.Format("{0} {1} {2}", b.employee_first_name, b.employee_middle_name, b.employee_last_name), emp_code = b.tbl_employee_id_details.emp_code, dept =""
                    //    b.tbl_department_master.department_name 
                    }).FirstOrDefault();

                    Obj_form_data.created_date = DateTime.Now;
                    Obj_form_data.employee_code = get_emp_data.emp_code;
                    Obj_form_data.employee_name = get_emp_data.emp_name;
                    Obj_form_data.department = get_emp_data.dept;

                    _context.Entry(Obj_form_data).State = EntityState.Added;
                    _context.SaveChanges();

                }

                objresponse.StatusCode = 0;
                objresponse.Message = "Save successfully !!";
                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Reset_PayrollMusterForm1Data")]
        [HttpPost]
        //[Authorize(Policy = "8133")]
        public IActionResult Reset_PayrollMusterForm1Data([FromBody] tbl_muster_form1_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var get_data = _context.tbl_muster_form1_data.Where(a => a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month && a.company_id == Obj_form_data.company_id).Select(b =>
                    b.form_i_id
                ).ToList();

                if (get_data == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid details please try again !!";
                    return Ok(objresponse);
                }
                else
                {

                    var update_whole_data = _context.tbl_muster_form1_data.Where(a => a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month && a.company_id == Obj_form_data.company_id && get_data.Contains(a.form_i_id)).ToList();
                    update_whole_data.ForEach(a =>
                    {
                        a.is_deleted = 1;
                        a.modified_by = Obj_form_data.created_by;
                        a.modified_date = DateTime.Now;
                    });
                    _context.SaveChanges();


                    //List<tbl_muster_form1_data> tbl_obj = new List<tbl_muster_form1_data>();

                    //for (int Index = 0; Index < get_data.Count; Index++)
                    //{





                    //    var get_data_one_by_one = _context.tbl_muster_form1_data.Where(a => a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month && a.company_id == Obj_form_data.company_id && a.form_i_id == get_data[Index].form_i_id).FirstOrDefault();
                    //    //get_data_one_by_one.is_deleted = 1;
                    //    //get_data_one_by_one.modified_by = Obj_form_data.created_by;
                    //    //get_data_one_by_one.modified_date = DateTime.Now;
                    //    //tbl_obj.Add(get_data_one_by_one);



                    //}
                    //_context.tbl_muster_form1_data.AttachRange(tbl_obj);
                    //_context.Entry(tbl_obj).State = EntityState.Modified;
                    //_context.SaveChanges();

                }


                objresponse.StatusCode = 0;
                objresponse.Message = "Reset All Data Successfully !!";
                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



#endregion END

        [Route("GetPayrollDetailsForMusterForm2/{payroll_month}/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8134")]
        public IActionResult GetPayrollDetailsForMusterForm2(int payroll_month, int company_id)
        {
            var get_freedz_data = _context.tbl_muster_form2_data.Where(a => a.company_id == company_id && a.payroll_month == payroll_month && a.is_deleted == 0)
                   .Select(a => new
                   {
                       a.employee_code,
                       employee_id = a.emp_id,
                       a.company_id,
                       a.payroll_month,
                       employe_name = a.employee_name,
                       father_or_husband = a.father_or_husband_name,
                       a.department,
                       a.damage_orloss_and_date,
                       a.whether_workman,
                       a.no_of_installment,
                       a.date_of_deduc_imposed,
                       a.amt_of_deduc_imposed,
                       a.date_realised,
                       a.remarks,
                       a.gender
                   })
                   .ToList();

            if (get_freedz_data.Count > 0)
            {
                return Ok(new { data = get_freedz_data, flag = 1 });
            }
            else
            {

                var data = _context.tbl_salary_input.Where(a => a.company_id == company_id && a.monthyear == payroll_month && a.is_active == 1).Select(p => new
                {

                    employee_code = p.tem.emp_code,
                    employee_id = p.emp_id,
                    company_id = p.company_id,//b.tbl_employee_id_details.tbl_user_master.Where(e => e.is_active == 1 && e.employee_id == a.emp_id).Select(e => e.default_company_id).FirstOrDefault(),
                    payroll_month = p.monthyear,
                    employe_name = string.Format("{0} {1} {2}", p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_middle_name,
                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_last_name
                    ),
                    father_or_husband = p.tem.tbl_emp_family_sec.FirstOrDefault(h => h.relation == "Father" && h.is_deleted == 0).name_as_per_aadhar_card,
                    department ="",// p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).tbl_department_master.department_name,
                    a_is_active = p.is_active,
                    damage_orloss_and_date = _context.tbl_salary_input.Join(_context.tbl_muster_form2, an => an.component_id, anc => anc.damage_orloss_and_date_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_II_mstr_id, an.company_id }).OrderByDescending(v => v.form_II_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                    whether_workman = _context.tbl_salary_input.Join(_context.tbl_muster_form2, an => an.component_id, anc => anc.whether_workman_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_II_mstr_id, an.company_id }).OrderByDescending(v => v.form_II_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                    no_of_installment = _context.tbl_salary_input.Join(_context.tbl_muster_form2, an => an.component_id, anc => anc.no_of_installment_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_II_mstr_id, an.company_id }).OrderByDescending(v => v.form_II_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                    date_of_deduc_imposed = _context.tbl_salary_input.Join(_context.tbl_muster_form2, an => an.component_id, anc => anc.date_ofdeduc_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_II_mstr_id, an.company_id }).OrderByDescending(v => v.form_II_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                    amt_of_deduc_imposed = _context.tbl_salary_input.Join(_context.tbl_muster_form2, an => an.component_id, anc => anc.amt_ofdeduc_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_II_mstr_id, an.company_id }).OrderByDescending(v => v.form_II_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                    date_realised = _context.tbl_salary_input.Join(_context.tbl_muster_form2, an => an.component_id, anc => anc.date_realised_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_II_mstr_id, an.company_id }).OrderByDescending(v => v.form_II_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                    remarks = _context.tbl_salary_input.Join(_context.tbl_muster_form2, an => an.component_id, anc => anc.remarks_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_II_mstr_id, an.company_id }).OrderByDescending(v => v.form_II_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                    gender = p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).gender == 1 ? "Female" :
                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).gender == 2 ? "Male" :
                     p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).gender == 3 ? "Other" : "",

                }).Distinct().ToList();

                return Ok(new { data = data, flag = 0 });
            }
        }


        [Route("GetPayrollDetailsForMusterForm2ByEmployee/{payroll_month}/{employee_id}")]
        [HttpGet]
        //[Authorize(Policy = "8135")]
        public IActionResult GetPayrollDetailsForMusterForm2ByEmployee(int payroll_month, int employee_id)
        {
            try
            {

                var data = _context.tbl_muster_form2_data.Join(_context.tbl_emp_officaial_sec, a => a.emp_id, b => b.employee_id, (a, b) => new
                {
                    a.form_ii_id,
                    employee_code = b.tbl_employee_id_details.emp_code,
                    employee_id = a.emp_id,
                    company_id = b.tbl_employee_id_details.tbl_user_master.Where(e => e.is_active == 1 && e.employee_id == a.emp_id).Select(e => e.default_company_id).FirstOrDefault(),
                    payroll_month = a.payroll_month,
                    employe_name = a.employee_name,
                    father_or_husband = a.father_or_husband_name,
                    department ="",// b.tbl_department_master.department_name,
                    department_id = 0,//b.tbl_department_master.department_id,
                    a_is_active = a.is_deleted,
                    b_is_active = b.is_deleted,
                    damage_orloss_and_date = a.damage_orloss_and_date,
                    whether_workman = a.whether_workman,
                    no_of_installment = a.no_of_installment,
                    date_of_deduc_imposed = a.date_of_deduc_imposed,
                    amt_of_deduc_imposed = a.amt_of_deduc_imposed,
                    date_realised = a.date_realised,
                    remarks = a.remarks,
                    gender = b.gender == 1 ? "Female" : b.gender == 2 ? "Male" : b.gender == 3 ? "Other" : "",
                    b.employee_first_name
                }).Where(a => a.employee_id == employee_id && a.payroll_month == payroll_month && a.a_is_active == 0 && a.b_is_active == 0 && !string.IsNullOrEmpty(a.employee_first_name)).Distinct().FirstOrDefault(); ;


                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


#region START 26-08-2019  The Register of Deduction for Damage or Loss in Form-II, Rule 21(4) ADD

        [Route("Save_PayrollMusterForm2Data")]
        [HttpPost]
        //[Authorize(Policy = "8136")]
        public IActionResult Save_PayrollMusterForm2Data([FromBody] tbl_muster_form2_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var data = _context.tbl_salary_input.Where(a => a.company_id == Obj_form_data.company_id && a.monthyear == Obj_form_data.payroll_month && a.is_active == 1).Select(p => new
                {

                    employee_code = p.tem.emp_code,
                    employee_id = p.emp_id,
                    company_id = p.company_id, //b.tbl_employee_id_details.tbl_user_master.Where(e => e.is_active == 1 && e.employee_id == a.emp_id).Select(e => e.default_company_id).FirstOrDefault(),
                    payroll_month = p.monthyear,
                    employe_name = string.Format("{0} {1} {2}", p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_middle_name,
                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_last_name
                    ),
                    father_or_husband = p.tem.tbl_emp_family_sec.FirstOrDefault(h => h.relation == "Father" && h.is_deleted == 0).name_as_per_aadhar_card,
                    department ="",// p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).tbl_department_master.department_name,
                    a_is_active = p.is_active,
                    damage_orloss_and_date = _context.tbl_salary_input.Join(_context.tbl_muster_form2, an => an.component_id, anc => anc.damage_orloss_and_date_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_II_mstr_id, an.company_id }).OrderByDescending(v => v.form_II_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    whether_workman = _context.tbl_salary_input.Join(_context.tbl_muster_form2, an => an.component_id, anc => anc.whether_workman_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_II_mstr_id, an.company_id }).OrderByDescending(v => v.form_II_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    no_of_installment = _context.tbl_salary_input.Join(_context.tbl_muster_form2, an => an.component_id, anc => anc.no_of_installment_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_II_mstr_id, an.company_id }).OrderByDescending(v => v.form_II_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    date_of_deduc_imposed = _context.tbl_salary_input.Join(_context.tbl_muster_form2, an => an.component_id, anc => anc.date_ofdeduc_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_II_mstr_id, an.company_id }).OrderByDescending(v => v.form_II_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    amt_of_deduc_imposed = _context.tbl_salary_input.Join(_context.tbl_muster_form2, an => an.component_id, anc => anc.amt_ofdeduc_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_II_mstr_id, an.company_id }).OrderByDescending(v => v.form_II_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    date_realised = _context.tbl_salary_input.Join(_context.tbl_muster_form2, an => an.component_id, anc => anc.date_realised_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_II_mstr_id, an.company_id }).OrderByDescending(v => v.form_II_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    remarks = _context.tbl_salary_input.Join(_context.tbl_muster_form2, an => an.component_id, anc => anc.remarks_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_II_mstr_id, an.company_id }).OrderByDescending(v => v.form_II_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    gender = p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).gender == 1 ? "Female" :
                             p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).gender == 2 ? "Male" :
                             p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).gender == 3 ? "Other" : "",

                }).Distinct().ToList();


                List<tbl_muster_form2_data> objform1data = new List<tbl_muster_form2_data>();

#if false
                for (int Index = 0; Index < data.Count; Index++)
                {
                    tbl_muster_form2_data obj = new tbl_muster_form2_data()
                    {
                        emp_id = data[Index].employee_id,
                        company_id = data[Index].company_id,
                        payroll_month = data[Index].payroll_month,
                        employee_code = data[Index].employee_code,
                        employee_name = data[Index].employe_name,
                        father_or_husband_name = data[Index].father_or_husband,
                        department = data[Index].department,
                        damage_orloss_and_date = data[Index].damage_orloss_and_date,
                        whether_workman = data[Index].whether_workman,
                        no_of_installment = data[Index].no_of_installment,
                        date_of_deduc_imposed = data[Index].date_of_deduc_imposed,
                        amt_of_deduc_imposed = data[Index].amt_of_deduc_imposed,
                        date_realised = data[Index].date_realised,
                        remarks = data[Index].remarks,
                        is_deleted = 0,
                        created_by = Obj_form_data.created_by,
                        created_date = DateTime.Now,
                        gender = data[Index].gender,
                    };

                    objform1data.Add(obj);
                }
#endif

                _context.tbl_muster_form2_data.AddRange(objform1data);
                _context.SaveChangesAsync();

                objresponse.StatusCode = 0;
                objresponse.Message = "Data Successfully Saved...!";


                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Update_PayrollMusterForm2Data")]
        [HttpPost]
        //[Authorize(Policy = "8137")]
        public IActionResult Update_PayrollMusterForm2Data([FromBody] tbl_muster_form2_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var get_data = _context.tbl_muster_form2_data.Where(a => a.emp_id == Obj_form_data.emp_id && a.form_ii_id == Obj_form_data.form_ii_id && a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month).FirstOrDefault();

                if (get_data == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid details please try again !!";
                    return Ok(objresponse);
                }
                else
                {
                    get_data.father_or_husband_name = Obj_form_data.father_or_husband_name;
                    get_data.damage_orloss_and_date = Obj_form_data.damage_orloss_and_date;
                    get_data.whether_workman = Obj_form_data.whether_workman;
                    get_data.no_of_installment = Obj_form_data.no_of_installment;
                    get_data.date_of_deduc_imposed = Obj_form_data.date_of_deduc_imposed;
                    get_data.amt_of_deduc_imposed = Obj_form_data.amt_of_deduc_imposed;
                    get_data.date_realised = Obj_form_data.date_realised;
                    get_data.remarks = Obj_form_data.remarks;
                    get_data.modified_by = Obj_form_data.modified_by; ;
                    get_data.modified_date = DateTime.Now;
                    //get_data.gender = Obj_form_data.gender;


                    _context.Entry(get_data).State = EntityState.Modified;
                    _context.SaveChanges();

                }

                objresponse.StatusCode = 0;
                objresponse.Message = "Updated successfully !!";
                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Add_PayrollMusterForm2Data")]
        [HttpPost]
        //[Authorize(Policy = "8138")]
        public IActionResult Add_PayrollMusterForm2Data([FromBody] tbl_muster_form2_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var get_data = _context.tbl_muster_form2_data.Where(a => a.emp_id == Obj_form_data.emp_id && a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month).FirstOrDefault();

                if (get_data != null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Employee details already exist...!";
                    return Ok(objresponse);
                }
                else
                {

                    var get_emp_data = _context.tbl_emp_officaial_sec.Where(a => a.is_deleted == 0 && !string.IsNullOrEmpty(a.employee_first_name) && a.employee_id == Obj_form_data.emp_id).Select(b => new { emp_name = string.Format("{0} {1} {2}", b.employee_first_name, b.employee_middle_name, b.employee_last_name), emp_code = b.tbl_employee_id_details.emp_code, dept ="",
                        //b.tbl_department_master.department_name, 
                        gender = b.gender == 1 ? "Female" : b.gender == 2 ? "Male" : b.gender == 3 ? "Other" : "" }).FirstOrDefault();

                    Obj_form_data.created_date = DateTime.Now;
                    Obj_form_data.employee_code = get_emp_data.emp_code;
                    Obj_form_data.employee_name = get_emp_data.emp_name;
                    Obj_form_data.department = get_emp_data.dept;
                    Obj_form_data.gender = get_emp_data.gender;
                    Obj_form_data.created_date = DateTime.Now;

                    _context.Entry(Obj_form_data).State = EntityState.Added;
                    _context.SaveChanges();

                }

                objresponse.StatusCode = 0;
                objresponse.Message = "Save successfully !!";
                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Reset_PayrollMusterForm2Data")]
        [HttpPost]
        //[Authorize(Policy = "8139")]
        public IActionResult Reset_PayrollMusterForm2Data([FromBody] tbl_muster_form2_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var get_data = _context.tbl_muster_form2_data.Where(a => a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month && a.company_id == Obj_form_data.company_id).Select(b =>
                    b.form_ii_id
                ).ToList();

                if (get_data == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid details please try again !!";
                    return Ok(objresponse);
                }
                else
                {

                    var update_whole_data = _context.tbl_muster_form2_data.Where(a => a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month && a.company_id == Obj_form_data.company_id && get_data.Contains(a.form_ii_id)).ToList();
                    update_whole_data.ForEach(a =>
                    {
                        a.is_deleted = 1;
                        a.modified_by = Obj_form_data.created_by;
                        a.modified_date = DateTime.Now;
                    });
                    _context.SaveChanges();

                }


                objresponse.StatusCode = 0;
                objresponse.Message = "Reset All Data Successfully !!";
                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

#endregion


#region START 27-08-2019 PDF ,Form 1

        [Route("PayrollMuster1PdfGenerator/{company_id}/{payroll_month}")]
        [HttpGet]
        //[Authorize(Policy = "8140")]
        public IActionResult PayrollMuster1PdfGenerator(int company_id, int payroll_month)
        {
            try
            {
                var webRoot = _hostingEnvironment.WebRootPath;

                var company_name = _context.tbl_company_master.Where(a => a.is_active == 1 && a.company_id == company_id).Select(a => a.company_name).FirstOrDefault();

                if (!Directory.Exists(webRoot + "/PayrollMusters/" + company_name + "/" + payroll_month + "/"))
                {
                    Directory.CreateDirectory(webRoot + "/PayrollMusters/" + company_name + "/" + payroll_month + "/");
                }

                var globalSettings = new GlobalSettings
                {
                    DocumentTitle = "The Register of Fines in Form-I, Rule 21(4)",
                    PaperSize = PaperKind.A4Rotated,
                    Margins = new MarginSettings() { Top = 20, Bottom = 0, Left = 5, Right = 7 },
                    Out = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/PayrollMusters/" + company_name + "/" + payroll_month + "/" + "The Register of Fines in Form-I, Rule 21(4)_" + DateTime.Now.Second + ".pdf"),
                };

                string payslip_file_path = "PayrollMusters/" + company_name + "/" + payroll_month + "/" + "The Register of Fines in Form-I, Rule 21(4)_" + DateTime.Now.Second + ".pdf";

                PayrollMustersPdf paymuspdf = new PayrollMustersPdf(_context, company_id, payroll_month);


                var objectSettings = new ObjectSettings
                {
                    HtmlContent = paymuspdf.TheRegisterOfFinesForm1(),
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                _converter.Convert(pdf);


                return Ok(new { statusCode = "0", message = payslip_file_path });
            }
            catch (Exception ex)
            {
                return Ok(new { statusCode = "0", message = "There some problem please try after later...!" });
            }

        }

#endregion


#region **START BY SUPRIYA ON 26-08-2019,FORM 3 ADVANCE 


        [Route("Save_RegisterofAdvanceFormIIIMaster")]
        [HttpPost]
        //[Authorize(Policy = "8141")]
        public IActionResult Save_RegisterofAdvanceFormIIIMaster([FromBody] tbl_muster_form3 objtbl)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                objtbl.is_deleted = 0;
                objtbl.created_date = DateTime.Now;

                _context.Entry(objtbl).State = EntityState.Added;
                _context.SaveChanges();


                objresponse.StatusCode = 0;
                objresponse.Message = "Successfully Saved...!";

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Get_RegisterofAdvanceFormIIIMaster/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8142")]
        public IActionResult Get_RegisterofAdvanceFormIIIMaster(int company_id)
        {
            try
            {
                var data = _context.tbl_muster_form3.Where(x => x.is_deleted == 0 && x.comp_id == company_id).OrderByDescending(x => x.form_III_mstr_id).FirstOrDefault();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [Route("GetPayrollDetailsForMusterForm3/{payroll_month}/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8143")]
        public IActionResult GetPayrollDetailsForMusterForm3(int payroll_month, int company_id)
        {
            try
            {
                //Check If payroll_month data already freedz
                var get_freedz_data = _context.tbl_muster_form3_data.Where(a => a.company_id == company_id && a.payroll_month == payroll_month && a.is_deleted == 0)
                    .Select(a => new
                    {
                        a.employee_code,
                        employee_id = a.emp_id,
                        a.company_id,
                        a.payroll_month,
                        employe_name = a.employee_name,
                        father_or_husband = a.father_or_husband_name,
                        a.department,
                        advance_date = a.advance_date,
                        advance_amount = a.advance_amount,
                        advance_purpose = a.purpose,
                        no_of_installment = a.no_of_installment,
                        postponement_granted = a.postponse_granted,
                        date_of_repaid = a.date_of_repaid,
                        remarks = a.remarks
                    })
                    .ToList();

                if (get_freedz_data.Count > 0)
                {
                    return Ok(new { data = get_freedz_data, flag = 1 });
                }
                else
                {

                    var data = _context.tbl_salary_input.Where(a => a.company_id == company_id && a.monthyear == payroll_month && a.is_active == 1).Select(p => new
                    {

                        employee_code = p.tem.emp_code,
                        employee_id = p.emp_id,
                        company_id = p.company_id,
                        payroll_month = p.monthyear,
                        employe_name = string.Format("{0} {1} {2}", p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                         p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_middle_name,
                         p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_last_name),
                        father_or_husband = p.tem.tbl_emp_family_sec.FirstOrDefault(x => x.is_deleted == 0 && x.relation.Trim().ToUpper() == "FATHER" && x.employee_id == p.emp_id).name_as_per_aadhar_card,
                        department ="",// p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.tbl_department_master.is_active == 1).tbl_department_master.department_name,
                        a_is_active = p.is_active,
                        advance_date = _context.tbl_salary_input.Join(_context.tbl_muster_form3, an => an.component_id, anc => anc.advance_date_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_III_mstr_id, an.company_id }).OrderByDescending(v => v.form_III_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        advance_amount = _context.tbl_salary_input.Join(_context.tbl_muster_form3, an => an.component_id, anc => anc.advance_amt_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_III_mstr_id, an.company_id }).OrderByDescending(v => v.form_III_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        advance_purpose = _context.tbl_salary_input.Join(_context.tbl_muster_form3, an => an.component_id, anc => anc.purpose_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_III_mstr_id, an.company_id }).OrderByDescending(v => v.form_III_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        no_of_installment = _context.tbl_salary_input.Join(_context.tbl_muster_form3, an => an.component_id, anc => anc.no_of_installment_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_III_mstr_id, an.company_id }).OrderByDescending(v => v.form_III_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        postponement_granted = _context.tbl_salary_input.Join(_context.tbl_muster_form3, an => an.component_id, anc => anc.postponement_granted_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_III_mstr_id, an.company_id }).OrderByDescending(v => v.form_III_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        date_of_repaid = _context.tbl_salary_input.Join(_context.tbl_muster_form3, an => an.component_id, anc => anc.date_total_repaid_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_III_mstr_id, an.company_id }).OrderByDescending(v => v.form_III_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        remarks = _context.tbl_salary_input.Join(_context.tbl_muster_form3, an => an.component_id, anc => anc.remarks_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_III_mstr_id, an.company_id }).OrderByDescending(v => v.form_III_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),

                    }).Distinct().ToList();


                    return Ok(new { data = data, flag = 0 });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("GetPayrollDetailsForMusterForm3ByEmployee/{payroll_month}/{employee_id}")]
        [HttpGet]
        //[Authorize(Policy = "8144")]
        public IActionResult GetPayrollDetailsForMusterForm3ByEmployee(int payroll_month, int employee_id)
        {
            try
            {

                var data = _context.tbl_muster_form3_data.Join(_context.tbl_emp_officaial_sec, a => a.emp_id, b => b.employee_id, (a, b) => new
                {
                    a.form_iii_id,
                    employee_code = b.tbl_employee_id_details.emp_code,
                    employee_id = a.emp_id,
                    company_id = b.tbl_employee_id_details.tbl_user_master.Where(e => e.is_active == 1 && e.employee_id == a.emp_id).Select(e => e.default_company_id).FirstOrDefault(),
                    payroll_month = a.payroll_month,
                    employe_name = a.employee_name,
                    father_or_husband = a.father_or_husband_name,
                    department ="",// b.tbl_department_master.department_name,
                    department_id =0,// b.tbl_department_master.department_id,
                    a_is_active = a.is_deleted,
                    b_is_active = b.is_deleted,
                    advance_date = a.advance_date,
                    advance_amount = a.advance_amount,
                    purpose = a.purpose,
                    no_of_installment = a.no_of_installment,
                    postponse_granted = a.postponse_granted,
                    date_of_repaid = a.date_of_repaid,
                    remarks = a.remarks
                }).Where(a => a.employee_id == employee_id && a.payroll_month == payroll_month && a.a_is_active == 0 && a.b_is_active == 0).Distinct().FirstOrDefault(); ;


                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


#endregion ** END BY SUPRIYA ON 26-08-2019,FORM 3 ADVANCE




#region START 27-08-2019  The Register of Advance in Form-III, Rule 21(4) ADD

        [Route("Save_PayrollMusterForm3Data")]
        [HttpPost]
        //[Authorize(Policy = "8145")]
        public IActionResult Save_PayrollMusterForm3Data([FromBody] tbl_muster_form3_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();



                var data = _context.tbl_salary_input.Where(a => a.company_id == Obj_form_data.company_id && a.monthyear == Obj_form_data.payroll_month && a.is_active == 1).Select(p => new
                {

                    employee_code = p.tem.emp_code,
                    employee_id = p.emp_id,
                    company_id = p.company_id,
                    payroll_month = p.monthyear,
                    employe_name = string.Format("{0} {1} {2}", p.tem.tbl_emp_officaial_sec.FirstOrDefault(b => b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).employee_first_name,
                                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(b => b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).employee_middle_name,
                                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(b => b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name)).employee_last_name),
                    father_or_husband = p.tem.tbl_emp_family_sec.FirstOrDefault(b => b.relation.Trim().ToUpper() == "FATHER" && b.is_deleted == 0 && b.employee_id == p.emp_id).name_as_per_aadhar_card,//.FirstOrDefault();
                    department ="",// p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.tbl_department_master.is_active == 1).tbl_department_master.department_name,
                    a_is_active = p.is_active,
                    advance_date = _context.tbl_salary_input.Join(_context.tbl_muster_form3, an => an.component_id, anc => anc.advance_date_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_III_mstr_id, an.company_id }).OrderByDescending(v => v.form_III_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    advance_amount = _context.tbl_salary_input.Join(_context.tbl_muster_form3, an => an.component_id, anc => anc.advance_amt_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_III_mstr_id, an.company_id }).OrderByDescending(v => v.form_III_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    purpose = _context.tbl_salary_input.Join(_context.tbl_muster_form3, an => an.component_id, anc => anc.purpose_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_III_mstr_id, an.company_id }).OrderByDescending(v => v.form_III_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    no_of_installment = _context.tbl_salary_input.Join(_context.tbl_muster_form3, an => an.component_id, anc => anc.no_of_installment_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_III_mstr_id, an.company_id }).OrderByDescending(v => v.form_III_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    postponse_granted = _context.tbl_salary_input.Join(_context.tbl_muster_form3, an => an.component_id, anc => anc.postponement_granted_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_III_mstr_id, an.company_id }).OrderByDescending(v => v.form_III_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    date_of_repaid = _context.tbl_salary_input.Join(_context.tbl_muster_form3, an => an.component_id, anc => anc.date_total_repaid_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_III_mstr_id, an.company_id }).OrderByDescending(v => v.form_III_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    remarks = _context.tbl_salary_input.Join(_context.tbl_muster_form3, an => an.component_id, anc => anc.remarks_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_III_mstr_id, an.company_id }).OrderByDescending(v => v.form_III_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    // b.employee_first_name
                }).Distinct().ToList();


#if false
                List<tbl_muster_form3_data> objform3data = new List<tbl_muster_form3_data>();

                for (int Index = 0; Index < data.Count; Index++)
                {
                    tbl_muster_form3_data obj = new tbl_muster_form3_data()
                    {
                        emp_id = data[Index].employee_id,
                        company_id = data[Index].company_id,
                        payroll_month = data[Index].payroll_month,
                        employee_code = data[Index].employee_code,
                        employee_name = data[Index].employe_name,
                        father_or_husband_name = data[Index].father_or_husband,
                        department = data[Index].department,
                        advance_date = data[Index].advance_date,
                        advance_amount = data[Index].advance_amount,
                        purpose = data[Index].purpose,
                        no_of_installment = data[Index].no_of_installment,
                        postponse_granted = data[Index].postponse_granted,
                        date_of_repaid = data[Index].date_of_repaid,
                        remarks = data[Index].remarks,
                        is_deleted = 0,
                        created_by = Obj_form_data.created_by,
                        created_date = DateTime.Now
                    };

                    objform3data.Add(obj);
                }
#endif

                //_context.tbl_muster_form3_data.AddRange(objform3data);
                //_context.SaveChangesAsync();

                objresponse.StatusCode = 0;
                objresponse.Message = "Data Successfully Saved...!";


                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Update_PayrollMusterForm3Data")]
        [HttpPost]
        //[Authorize(Policy = "8146")]
        public IActionResult Update_PayrollMusterForm3Data([FromBody] tbl_muster_form3_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var get_data = _context.tbl_muster_form3_data.Where(a => a.emp_id == Obj_form_data.emp_id && a.form_iii_id == Obj_form_data.form_iii_id && a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month).FirstOrDefault();

                if (get_data == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid details please try again !!";
                    return Ok(objresponse);
                }
                else
                {
                    get_data.father_or_husband_name = Obj_form_data.father_or_husband_name;
                    get_data.advance_date = Obj_form_data.advance_date;
                    get_data.advance_amount = Obj_form_data.advance_amount;
                    get_data.purpose = Obj_form_data.purpose;
                    get_data.no_of_installment = Obj_form_data.no_of_installment;
                    get_data.postponse_granted = Obj_form_data.postponse_granted;
                    get_data.date_of_repaid = Obj_form_data.date_of_repaid;
                    get_data.remarks = Obj_form_data.remarks;
                    get_data.modified_by = Obj_form_data.modified_by;
                    get_data.modified_date = DateTime.Now;

                    _context.Entry(get_data).State = EntityState.Modified;
                    _context.SaveChanges();

                }

                // var get_emp_data = _context.tbl_emp_officaial_sec.Where(a => a.is_deleted == 0 && a.employee_first_name != "" && a.employee_id == Obj_form_data.emp_id).Select(b => new { emp_name = b.employee_first_name ?? "" + " " + b.employee_middle_name ?? "" + " " + b.employee_last_name ?? "", emp_code = b.tbl_employee_id_details.emp_code, dept = b.tbl_department_master.department_name }).FirstOrDefault();


                objresponse.StatusCode = 0;
                objresponse.Message = "Updated successfully !!";
                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Add_PayrollMusterForm3Data")]
        [HttpPost]
        //[Authorize(Policy = "8147")]
        public IActionResult Add_PayrollMusterForm3Data([FromBody] tbl_muster_form3_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var get_data = _context.tbl_muster_form3_data.Where(a => a.emp_id == Obj_form_data.emp_id && a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month).FirstOrDefault();

                if (get_data != null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Employee details already exist...!";
                    return Ok(objresponse);
                }
                else
                {

                    var get_emp_data = _context.tbl_emp_officaial_sec.Where(a => a.is_deleted == 0 && !string.IsNullOrEmpty(a.employee_first_name) && a.employee_id == Obj_form_data.emp_id).Select(b => new { emp_name = string.Format("{0} {1} {2}", b.employee_first_name, b.employee_middle_name, b.employee_last_name), emp_code = b.tbl_employee_id_details.emp_code, dept ="",
                        //b.tbl_department_master.department_name
                        }).FirstOrDefault();

                    Obj_form_data.created_date = DateTime.Now;
                    Obj_form_data.employee_code = get_emp_data.emp_code;
                    Obj_form_data.employee_name = get_emp_data.emp_name;
                    Obj_form_data.department = get_emp_data.dept;

                    _context.Entry(Obj_form_data).State = EntityState.Added;
                    _context.SaveChanges();

                }

                objresponse.StatusCode = 0;
                objresponse.Message = "Save successfully !!";
                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Reset_PayrollMusterForm3Data")]
        [HttpPost]
        //[Authorize(Policy = "8148")]
        public IActionResult Reset_PayrollMusterForm3Data([FromBody] tbl_muster_form3_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var get_data = _context.tbl_muster_form3_data.Where(a => a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month && a.company_id == Obj_form_data.company_id).Select(b =>
                    b.form_iii_id
                ).ToList();

                if (get_data == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid details please try again !!";
                    return Ok(objresponse);
                }
                else
                {

                    var update_whole_data = _context.tbl_muster_form3_data.Where(a => a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month && a.company_id == Obj_form_data.company_id && get_data.Contains(a.form_iii_id)).ToList();
                    update_whole_data.ForEach(a =>
                    {
                        a.is_deleted = 1;
                        a.modified_by = Obj_form_data.created_by;
                        a.modified_date = DateTime.Now;
                    });
                    _context.SaveChanges();

                }


                objresponse.StatusCode = 0;
                objresponse.Message = "Reset All Data Successfully !!";
                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



#endregion END






#region ** START BY SUPRIYA ON 27-08-2019,FORM 4 OVERTIME


        [Route("Save_RegisterofOvertimeFormIVMaster")]
        [HttpPost]
        //[Authorize(Policy = "8149")]
        public IActionResult Save_RegisterofOvertimeFormIVMaster([FromBody] tbl_muster_form4 objtbl)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                objtbl.is_deleted = 0;
                objtbl.created_date = DateTime.Now;

                _context.Entry(objtbl).State = EntityState.Added;
                _context.SaveChanges();


                objresponse.StatusCode = 0;
                objresponse.Message = "Successfully Saved...!";

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Get_RegisterofOvertimeFormIVMaster/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8150")]
        public IActionResult Get_RegisterofOvertimeFormIVMaster(int company_id)
        {
            try
            {
                var data = _context.tbl_muster_form4.Where(x => x.is_deleted == 0 && x.comp_id == company_id).OrderByDescending(x => x.form_IV_mstr_id).FirstOrDefault();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        [Route("GetPayrollDetailsForMusterForm4/{payroll_month}/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8151")]
        public IActionResult GetPayrollDetailsForMusterForm4(int payroll_month, int company_id)
        {
            throw new NotImplementedException();
#if false
            try
            {
                //Check If payroll_month data already freedz
                var get_freedz_data = _context.tbl_muster_form4_data.Where(a => a.company_id == company_id && a.payroll_month == payroll_month && a.is_deleted == 0)
                    .Select(a => new
                    {
                        a.employee_code,
                        employee_id = a.emp_id,
                        a.company_id,
                        a.payroll_month,
                        employee_name = a.employee_name,
                        father_or_husband_name = a.father_or_husband_name,
                        designation_anddepartment = string.Format("{0} and {1}", a.designation, a.department),
                        gender = a.sex,
                        overtime_work_dt = a.overtime_work_dt,
                        extent_overtime = a.extent_overtime,
                        total_overtime_worked = a.total_overtime_worked,
                        normal_hr = a.normal_hr,
                        normal_rate = a.normal_rate,
                        overtime_rate = a.overtime_rate,
                        normal_earning = a.normal_earning,
                        overtime_earning = a.overtime_earning,
                        total_earning = a.total_earning,
                        date_ofpayment = a.date_ofpayment

                    })
                    .ToList();

                if (get_freedz_data.Count > 0)
                {
                    return Ok(new { data = get_freedz_data, flag = 1 });
                }
                else
                {

                    var data = _context.tbl_salary_input.Where(a => a.company_id == company_id && a.monthyear == payroll_month && a.is_active == 1).Select(p => new
                    {

                        employee_code = p.tem.emp_code,
                        employee_id = p.emp_id,
                        company_id = p.company_id,
                        payroll_month = p.monthyear,
                        employee_name = string.Format("{0} {1} {2}", p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                        p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_middle_name,
                        p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_last_name),
                        father_or_husband_name = p.tem.tbl_emp_family_sec.FirstOrDefault(x => x.is_deleted == 0 && x.relation.Trim().ToUpper() == "FATHER" && x.employee_id == p.emp_id).name_as_per_aadhar_card,
                        department = p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).tbl_department_master.department_name,

                        designation = p.tem.tbl_emp_desi_allocation.OrderByDescending(y => y.emp_grade_id).FirstOrDefault(x => x.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name,

                        designation_anddepartment = string.Format("{0} and {1}", !string.IsNullOrEmpty(p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).tbl_department_master.department_name) ?
                        p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).tbl_department_master.department_name : "-",
                        !string.IsNullOrEmpty(p.tem.tbl_emp_desi_allocation.OrderByDescending(y => y.emp_grade_id).FirstOrDefault(x => x.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name) ?
                        p.tem.tbl_emp_desi_allocation.OrderByDescending(y => y.emp_grade_id).FirstOrDefault(x => x.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name : "-"),

                        a_is_active = p.is_active,
                        overtime_work_dt = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.overtime_date_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        extent_overtime = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.extent_overtime_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        total_overtime_worked = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.total_overtime_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        normal_hr = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.normal_hr_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        normal_rate = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.normal_rate_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        overtime_rate = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.overtime_rate_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),

                        normal_earning = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.normal_earning_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        overtime_earning = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.overtime_earning_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        total_earning = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.total_earning_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        date_ofpayment = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.date_ofpayment_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        gender = p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).gender == 1 ? "Female" :
                                p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).gender == 2 ? "Male" :
                                p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).gender == 3 ? "Other" : "",

                    }).Distinct().ToList();


                    return Ok(new { data = data, flag = 0 });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
#endif
        }


        [Route("GetPayrollDetailsForMusterForm4ByEmployee/{payroll_month}/{employee_id}")]
        [HttpGet]
        //[Authorize(Policy = "8152")]
        public IActionResult GetPayrollDetailsForMusterForm4ByEmployee(int payroll_month, int employee_id)
        {
            try
            {

                var data = _context.tbl_muster_form4_data.Join(_context.tbl_emp_officaial_sec, a => a.emp_id, b => b.employee_id, (a, b) => new
                {
                    a.form_iv_id,
                    employee_code = b.tbl_employee_id_details.emp_code,
                    employee_id = a.emp_id,
                    company_id = b.tbl_employee_id_details.tbl_user_master.Where(e => e.is_active == 1 && e.employee_id == a.emp_id).Select(e => e.default_company_id).FirstOrDefault(),
                    payroll_month = a.payroll_month,
                    employe_name = a.employee_name,
                    father_or_husband = a.father_or_husband_name,
                    //department = b.tbl_department_master.department_name,
                    //department_id = b.tbl_department_master.department_id,
                    a_is_active = a.is_deleted,
                    b_is_active = b.is_deleted,
                    desig_id = b.tbl_employee_id_details.tbl_emp_desi_allocation.OrderByDescending(g => g.emp_grade_id).FirstOrDefault(d => d.tbl_designation_master.is_active == 1).desig_id,
                    designation_name = b.tbl_employee_id_details.tbl_emp_desi_allocation.OrderByDescending(g => g.emp_grade_id).FirstOrDefault(d => d.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name,
                    overtime_work_dt = a.overtime_work_dt,
                    extent_overtime = a.extent_overtime,
                    total_overtime_worked = a.total_overtime_worked,
                    normal_hr = a.normal_hr,
                    normal_rate = a.normal_rate,
                    overtime_rate = a.overtime_rate,
                    normal_earning = a.normal_earning,
                    overtime_earning = a.overtime_earning,
                    total_earning = a.total_earning,
                    date_ofpayment = a.date_ofpayment,
                    gender = a.sex,
                    b.employee_first_name
                }).Where(a => a.employee_id == employee_id && a.payroll_month == payroll_month && a.a_is_active == 0 && a.b_is_active == 0 && !string.IsNullOrEmpty(a.employee_first_name)).Distinct().FirstOrDefault(); ;


                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }




#endregion ** END BY SUPRIYA ON 27-08-2019,FORM 4 OVERTIME



#region START 27-08-2019  The Register of Overtime in Form-IV, Rule 21(4) ADD

        [Route("Save_PayrollMusterForm4Data")]
        [HttpPost]
        //[Authorize(Policy = "8153")]
        public IActionResult Save_PayrollMusterForm4Data([FromBody] tbl_muster_form4_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();




                var data = _context.tbl_salary_input.Where(a => a.company_id == Obj_form_data.company_id && a.monthyear == Obj_form_data.payroll_month && a.is_active == 1).Select(p => new
                {

                    employee_code = p.tem.emp_code,
                    employee_id = p.emp_id,
                    company_id = p.company_id,
                    payroll_month = p.monthyear,
                    employe_name = string.Format("{0} {1} {2}", p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,

                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_middle_name,
                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_last_name
                    ),

                    father_or_husband = p.tem.tbl_emp_family_sec.FirstOrDefault(h => h.relation.Trim().ToUpper() == "FATHER" && h.is_deleted == 0).name_as_per_aadhar_card,
                    department ="",// p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && x.tbl_department_master.is_active == 1).tbl_department_master.department_name,
                    designation = p.tem.tbl_emp_desi_allocation.OrderByDescending(x => x.emp_grade_id).FirstOrDefault(x => x.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name,
                    a_is_active = p.is_active,
                    overtime_work_dt = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.overtime_date_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    extent_overtime = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.extent_overtime_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    total_overtime_worked = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.total_overtime_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    normal_hr = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.normal_hr_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    normal_rate = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.normal_rate_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    overtime_rate = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.overtime_rate_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),

                    normal_earning = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.normal_earning_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    overtime_earning = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.overtime_earning_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    total_earning = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.total_earning_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    date_ofpayment = _context.tbl_salary_input.Join(_context.tbl_muster_form4, an => an.component_id, anc => anc.date_ofpayment_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_IV_mstr_id, an.company_id }).OrderByDescending(v => v.form_IV_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    gender = p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).gender == 1 ? "Female" :
                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).gender == 2 ? "Male" :
                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).gender == 3 ? "Other" : "",

                }).Distinct().ToList();


                List<tbl_muster_form4_data> objform4data = new List<tbl_muster_form4_data>();

                for (int Index = 0; Index < data.Count; Index++)
                {
                    tbl_muster_form4_data obj = new tbl_muster_form4_data()
                    {
                        emp_id = data[Index].employee_id,
                        company_id = data[Index].company_id,
                        payroll_month = data[Index].payroll_month,
                        employee_code = data[Index].employee_code,
                        employee_name = data[Index].employe_name,
                        father_or_husband_name = data[Index].father_or_husband,
                        department = data[Index].department,
                        designation = data[Index].designation,
                        overtime_work_dt = data[Index].overtime_work_dt,
                        extent_overtime = data[Index].extent_overtime,
                        total_overtime_worked = data[Index].total_overtime_worked,
                        normal_hr = data[Index].normal_hr,
                        normal_rate = data[Index].normal_rate,
                        overtime_rate = data[Index].overtime_rate,
                        normal_earning = data[Index].normal_earning,
                        overtime_earning = data[Index].overtime_earning,
                        total_earning = data[Index].total_earning,
                        date_ofpayment = data[Index].date_ofpayment,
                        sex = data[Index].gender,
                        is_deleted = 0,
                        created_by = Obj_form_data.created_by,
                        created_date = DateTime.Now
                    };

                    objform4data.Add(obj);
                }


                _context.tbl_muster_form4_data.AddRange(objform4data);
                _context.SaveChangesAsync();

                objresponse.StatusCode = 0;
                objresponse.Message = "Data Successfully Saved...!";


                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Update_PayrollMusterForm4Data")]
        [HttpPost]
        //[Authorize(Policy = "8154")]
        public IActionResult Update_PayrollMusterForm4Data([FromBody] tbl_muster_form4_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var get_data = _context.tbl_muster_form4_data.Where(a => a.emp_id == Obj_form_data.emp_id && a.form_iv_id == Obj_form_data.form_iv_id && a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month).FirstOrDefault();

                if (get_data == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid details please try again !!";
                    return Ok(objresponse);
                }
                else
                {
                    get_data.father_or_husband_name = Obj_form_data.father_or_husband_name;
                    get_data.overtime_work_dt = Obj_form_data.overtime_work_dt;
                    get_data.extent_overtime = Obj_form_data.extent_overtime;
                    get_data.total_overtime_worked = Obj_form_data.total_overtime_worked;
                    get_data.normal_hr = Obj_form_data.normal_hr;
                    get_data.normal_rate = Obj_form_data.normal_rate;
                    get_data.overtime_rate = Obj_form_data.overtime_rate;
                    get_data.normal_earning = Obj_form_data.normal_earning;
                    get_data.overtime_earning = Obj_form_data.overtime_earning;
                    get_data.total_earning = Obj_form_data.total_earning;
                    get_data.date_ofpayment = Obj_form_data.date_ofpayment;
                    get_data.modified_by = Obj_form_data.modified_by;
                    get_data.modified_date = DateTime.Now;
                    _context.Entry(get_data).State = EntityState.Modified;
                    _context.SaveChanges();

                }

                // var get_emp_data = _context.tbl_emp_officaial_sec.Where(a => a.is_deleted == 0 && a.employee_first_name != "" && a.employee_id == Obj_form_data.emp_id).Select(b => new { emp_name = b.employee_first_name ?? "" + " " + b.employee_middle_name ?? "" + " " + b.employee_last_name ?? "", emp_code = b.tbl_employee_id_details.emp_code, dept = b.tbl_department_master.department_name }).FirstOrDefault();


                objresponse.StatusCode = 0;
                objresponse.Message = "Updated successfully !!";
                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Add_PayrollMusterForm4Data")]
        [HttpPost]
        //[Authorize(Policy = "8155")]
        public IActionResult Add_PayrollMusterForm4Data([FromBody] tbl_muster_form4_data Obj_form_data)
        {
            throw new NotImplementedException();
#if false
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var get_data = _context.tbl_muster_form4_data.Where(a => a.emp_id == Obj_form_data.emp_id && a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month).FirstOrDefault();

                if (get_data != null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Employee details already exist...!";
                    return Ok(objresponse);
                }
                else
                {

                    var get_emp_data = _context.tbl_emp_officaial_sec.Where(a => a.is_deleted == 0 && !string.IsNullOrEmpty(a.employee_first_name) && a.employee_id == Obj_form_data.emp_id).Select(b => new { emp_name = string.Format("{0} {1} {2}", b.employee_first_name, b.employee_middle_name, b.employee_last_name), emp_code = b.tbl_employee_id_details.emp_code, dept = b.tbl_department_master.department_name, designation = b.tbl_employee_id_details.tbl_emp_desi_allocation.FirstOrDefault(g => g.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name, gender = b.gender == 1 ? "Female" : b.gender == 2 ? "Male" : b.gender == 3 ? "Other" : "" }).FirstOrDefault();

                    Obj_form_data.created_date = DateTime.Now;
                    Obj_form_data.employee_code = get_emp_data.emp_code;
                    Obj_form_data.employee_name = get_emp_data.emp_name;
                    Obj_form_data.department = get_emp_data.dept;
                    Obj_form_data.designation = get_emp_data.designation;
                    Obj_form_data.sex = get_emp_data.gender;
                    Obj_form_data.is_deleted = 0;

                    _context.Entry(Obj_form_data).State = EntityState.Added;
                    _context.SaveChanges();

                }

                objresponse.StatusCode = 0;
                objresponse.Message = "Save successfully !!";
                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
#endif
        }

        [Route("Reset_PayrollMusterForm4Data")]
        [HttpPost]
        //[Authorize(Policy = "8156")]
        public IActionResult Reset_PayrollMusterForm4Data([FromBody] tbl_muster_form1_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var get_data = _context.tbl_muster_form4_data.Where(a => a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month && a.company_id == Obj_form_data.company_id).Select(b =>
                    b.form_iv_id
                ).ToList();

                if (get_data == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid details please try again !!";
                    return Ok(objresponse);
                }
                else
                {

                    var update_whole_data = _context.tbl_muster_form4_data.Where(a => a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month && a.company_id == Obj_form_data.company_id && get_data.Contains(a.form_iv_id)).ToList();
                    update_whole_data.ForEach(a =>
                    {
                        a.is_deleted = 1;
                        a.modified_by = Obj_form_data.modified_by;
                        a.modified_date = DateTime.Now;
                    });
                    _context.SaveChanges();

                }


                objresponse.StatusCode = 0;
                objresponse.Message = "Reset All Data Successfully !!";
                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



#endregion END



#region START 28-08-2019 PDF , FORM 2

        [Route("PayrollMuster2PdfGenerator/{company_id}/{payroll_month}")]
        [HttpGet]
        //[Authorize(Policy = "8157")]
        public IActionResult PayrollMuster2PdfGenerator(int company_id, int payroll_month)
        {
            try
            {
                var webRoot = _hostingEnvironment.WebRootPath;

                var company_name = _context.tbl_company_master.Where(a => a.is_active == 1 && a.company_id == company_id).Select(a => a.company_name).FirstOrDefault();

                if (!Directory.Exists(webRoot + "/PayrollMusters/" + company_name + "/" + payroll_month + "/"))
                {
                    Directory.CreateDirectory(webRoot + "/PayrollMusters/" + company_name + "/" + payroll_month + "/");
                }

                var globalSettings = new GlobalSettings
                {
                    DocumentTitle = "The Register of Fines in Form-I, Rule 21(4)",
                    PaperSize = PaperKind.A4Rotated,
                    Margins = new MarginSettings() { Top = 20, Bottom = 0, Left = 5, Right = 7 },
                    Out = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/PayrollMusters/" + company_name + "/" + payroll_month + "/" + "Register of Deduction For Damage or Loss in Form-II, Rule 21(4)_" + DateTime.Now.Second + ".pdf"),
                };

                string payslip_file_path = "PayrollMusters/" + company_name + "/" + payroll_month + "/" + "Register of Deduction For Damage or Loss in Form-II, Rule 21(4)_" + DateTime.Now.Second + ".pdf";

                PayrollMustersPdf paymuspdf = new PayrollMustersPdf(_context, company_id, payroll_month);


                var objectSettings = new ObjectSettings
                {
                    HtmlContent = paymuspdf.TheRegisterOfFinesForm2(),
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                _converter.Convert(pdf);


                return Ok(new { statusCode = "0", message = payslip_file_path });
            }
            catch (Exception ex)
            {
                return Ok(new { statusCode = "0", message = "There some problem please try after later...!" });
            }

        }

#endregion


#region START 29-08-2019 PDF , FORM 3

        [Route("PayrollMuster3PdfGenerator/{company_id}/{payroll_month}")]
        [HttpGet]
        //[Authorize(Policy = "8158")]
        public IActionResult PayrollMuster3PdfGenerator(int company_id, int payroll_month)
        {
            try
            {
                var webRoot = _hostingEnvironment.WebRootPath;

                var company_name = _context.tbl_company_master.Where(a => a.is_active == 1 && a.company_id == company_id).Select(a => a.company_name).FirstOrDefault();

                if (!Directory.Exists(webRoot + "/PayrollMusters/" + company_name + "/" + payroll_month + "/"))
                {
                    Directory.CreateDirectory(webRoot + "/PayrollMusters/" + company_name + "/" + payroll_month + "/");
                }

                var globalSettings = new GlobalSettings
                {
                    DocumentTitle = "The Register of Fines in Form-I, Rule 21(4)",
                    PaperSize = PaperKind.A4Rotated,
                    Margins = new MarginSettings() { Top = 20, Bottom = 0, Left = 5, Right = 7 },
                    Out = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/PayrollMusters/" + company_name + "/" + payroll_month + "/" + "The Register of Advance Made to Employed Person Form-III(PW)_" + DateTime.Now.Second + ".pdf"),
                };

                string payslip_file_path = "PayrollMusters/" + company_name + "/" + payroll_month + "/" + "The Register of Advance Made to Employed Person Form-III(PW)_" + DateTime.Now.Second + ".pdf";

                PayrollMustersPdf paymuspdf = new PayrollMustersPdf(_context, company_id, payroll_month);


                var objectSettings = new ObjectSettings
                {
                    HtmlContent = paymuspdf.TheRegisterOfFinesForm3(),
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                _converter.Convert(pdf);


                return Ok(new { statusCode = "0", message = payslip_file_path });
            }
            catch (Exception ex)
            {
                return Ok(new { statusCode = "0", message = "There some problem please try after later...!" });
            }

        }

#endregion


#region START 29-08-2019 PDF ,Form 4

        [Route("PayrollMuster4PdfGenerator/{company_id}/{payroll_month}")]
        [HttpGet]
        //[Authorize(Policy = "8159")]
        public IActionResult PayrollMuster4PdfGenerator(int company_id, int payroll_month)
        {
            try
            {
                var webRoot = _hostingEnvironment.WebRootPath;

                var company_name = _context.tbl_company_master.Where(a => a.is_active == 1 && a.company_id == company_id).Select(a => a.company_name).FirstOrDefault();

                if (!Directory.Exists(webRoot + "/PayrollMusters/" + company_name + "/" + payroll_month + "/"))
                {
                    Directory.CreateDirectory(webRoot + "/PayrollMusters/" + company_name + "/" + payroll_month + "/");
                }

                var globalSettings = new GlobalSettings
                {
                    DocumentTitle = "The Register of Fines in Form-I, Rule 21(4)",
                    PaperSize = PaperKind.A4Rotated,
                    Margins = new MarginSettings() { Top = 20, Bottom = 0, Left = 5, Right = 7 },
                    Out = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/PayrollMusters/" + company_name + "/" + payroll_month + "/" + "The Register of Overtime in Form-IV, Rule 25(2)_" + DateTime.Now.Second + ".pdf"),
                };

                string payslip_file_path = "PayrollMusters/" + company_name + "/" + payroll_month + "/" + "The Register of Overtime in Form-IV, Rule 25(2)_" + DateTime.Now.Second + ".pdf";

                PayrollMustersPdf paymuspdf = new PayrollMustersPdf(_context, company_id, payroll_month);


                var objectSettings = new ObjectSettings
                {
                    HtmlContent = paymuspdf.TheRegisterOfFinesForm4(),
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                _converter.Convert(pdf);


                return Ok(new { statusCode = "0", message = payslip_file_path });
            }
            catch (Exception ex)
            {
                return Ok(new { statusCode = "0", message = "There some problem please try after later...!" });
            }

        }

#endregion


#region Start 30-08-2019 , Form 5

        public class InputRows
        {
            public int empID { get; set; }
            public string EmpCode { get; set; }
            public string card_number { get; set; }
            public string employee_first_name { get; set; }
            public string attendancday { get; set; }
            public string attendancstatus { get; set; }
            public string department_name { get; set; }
            public int no_of_days_p { get; set; }
        }



        [Route("GetPayrollDetailsForMusterForm5/{company_id}/{payroll_month}/{location}")]
        [HttpGet]
        //[Authorize(Policy = "8160")]
        public IActionResult GetPayrollDetailsForMusterForm5(int company_id, int payroll_month, int location)
        {
            throw new NotImplementedException();
#if false
            try
            {
                bool allcompany_id = company_id == 0 ? true : false;
                bool alllocation_id = location == 0 ? true : false;

                int dt_year = Convert.ToInt32(payroll_month.ToString().Substring(0, 4));
                int dt_month = Convert.ToInt32(payroll_month.ToString().Substring(4, 2));
                string filter_dt = "";
                if (dt_month < 10)
                {
                    filter_dt = dt_year + "0" + dt_month;
                }
                else
                {
                    filter_dt = dt_year + "" + dt_month;
                }

                string p_year = payroll_month.ToString().Substring(0, 4);
                string p_month = payroll_month.ToString().Substring(4, 2);


                int days = DateTime.DaysInMonth(Convert.ToInt32(p_year), Convert.ToInt32(p_month));

                DateTime from_dt = Convert.ToDateTime((dt_year) + "-" + dt_month + "-" + 01);
                DateTime to_dt = Convert.ToDateTime(dt_year + "-" + dt_month + "-" + days);

                var data = (
                    from d in _context.tbl_daily_attendance
                    join em in _context.tbl_employee_master on d.emp_id equals em.employee_id
                    join e in _context.tbl_user_master on em.employee_id equals e.employee_id
                    join o in _context.tbl_emp_officaial_sec on d.emp_id equals o.employee_id
                    join s in _context.tbl_shift_details on d.shift_id equals s.shift_id

                    where
                    e.default_company_id == (allcompany_id ? e.default_company_id : company_id) &&
                    o.location_id == (alllocation_id ? o.location_id : location) &&
                    d.attendance_dt >= from_dt && d.attendance_dt <= to_dt &&
                    em.is_active == 1 && e.is_active == 1 && o.is_deleted == 0 && s.is_deleted == 0 &&
                    (d.day_status == 1 || d.day_status == 2 || d.day_status == 3 || d.day_status == 4 || d.day_status == 5 || d.day_status == 6 || d.day_status == 0)
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
                        s.shift_name,
                        o.tbl_department_master.department_name
                    }).ToList();

                //----------set payroll calender data--------------------------

                List<InputRows> list = data.Select(p => new InputRows
                {
                    empID = p.employee_id ?? 0,
                    EmpCode = p.emp_code,
                    card_number = p.card_number,
                    employee_first_name = p.employee_first_name,
                    attendancday = Convert.ToString(p.attendance_dt.Day),
                    attendancstatus = p.day_status == 1 ? "<span style='color:#8b8be8'>P</span>" : p.is_weekly_off == 1 ? "<span style='color:#b3aeae'>W/O</span>" : p.is_holiday == 1 ? "<span style='color:#9dea27'>H</span>" : p.day_status == 2 ? "<span style='color:#ef925e'>A</span>" : p.day_status == 3 ? "<span style='color:#e46ee4'>L</span>" : p.day_status == 4 ? "<span style='color:#ef925e'>HP/HA</span>" : p.day_status == 5 ? "<span style='color:#e46ee4'>HP/HL</span>" : p.day_status == 6 ? "<span style='color:#ef925e'>HL/HA</span>" : "<span style='color:red'>A</span>",
                    department_name = p.department_name,
                    no_of_days_p = _context.tbl_daily_attendance.Where(a => a.emp_id == p.employee_id && a.day_status == 1 && a.attendance_dt >= from_dt && a.attendance_dt <= to_dt).ToList().Count
                }).ToList();

                var result = list.Select(x => new
                {
                    //convert cols to list of rows
                    RowData = new List<Tuple<int, string, string, string, string, string, string>>()
                        {
                        System.Tuple.Create(x.empID ,x.attendancday, x.card_number, x.employee_first_name,x.attendancstatus, x.EmpCode, x.department_name),
                    }
                })
                    //get one result list
                    .SelectMany(x => x.RowData)
                    //group data by year
                    .GroupBy(x => x.Item1)
                    //finally get pivoted data
                    .Select((grp, counter) => new
                    {
                        card_number = counter + 1,
                        employee_code = list.FirstOrDefault(a => a.empID == grp.Key).EmpCode,
                        employee_first_name = list.FirstOrDefault(a => a.empID == grp.Key).employee_first_name,
                        department_name = list.FirstOrDefault(a => a.empID == grp.Key).department_name,
                        no_of_days_p = list.FirstOrDefault(a => a.empID == grp.Key).no_of_days_p,
                        remarks = "",
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


                List<object> column = new List<object>();
                column.Add(new { title = "Sr. No.", data = "card_number" });
                column.Add(new { title = "Code", data = "employee_code" });
                column.Add(new { title = "Employee Name", data = "employee_first_name" });
                column.Add(new { title = "Nature Of Work", data = "department_name" });

                for (int i = 1; i <= days; i++)
                {
                    column.Add(new
                    {
                        title = i,
                        data = "day" + i
                    });
                }


                column.Add(new { title = "No. of Days P", data = "no_of_days_p" });
                column.Add(new { title = "Remarks", data = "remarks" });

                var d2 = new { list = result, column };

                return Ok(d2);



            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
#endif
        }


        [Route("GetPayrollDetailsForMusterForm5DownloadPdf/{company_id}/{payroll_month}/{location}")]
        [HttpGet]
        //[Authorize(Policy = "8161")]
        public IActionResult GetPayrollDetailsForMusterForm5DownloadPdf(int company_id, int payroll_month, int location)
        {
            try
            {

                var webRoot = _hostingEnvironment.WebRootPath;

                var company_name = _context.tbl_company_master.Where(a => a.is_active == 1 && a.company_id == company_id).Select(a => a.company_name).FirstOrDefault();

                if (!Directory.Exists(webRoot + "/PayrollMusters/" + company_name + "/" + payroll_month + "/"))
                {
                    Directory.CreateDirectory(webRoot + "/PayrollMusters/" + company_name + "/" + payroll_month + "/");
                }

                var globalSettings = new GlobalSettings
                {
                    DocumentTitle = "Muster Role in Form-V, Rule 26(5)",
                    PaperSize = PaperKind.A4Rotated,
                    Margins = new MarginSettings() { Top = 20, Bottom = 0, Left = 5, Right = 7 },
                    Out = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/PayrollMusters/" + company_name + "/" + payroll_month + "/" + "Muster Role in Form-V, Rule 26(5)_" + DateTime.Now.Second + ".pdf"),
                };

                string payslip_file_path = "PayrollMusters/" + company_name + "/" + payroll_month + "/" + "Muster Role in Form-V, Rule 26(5)_" + DateTime.Now.Second + ".pdf";

                PayrollMustersForm5Pdf paymuspdf = new PayrollMustersForm5Pdf(_context, company_id, payroll_month, location);


                var objectSettings = new ObjectSettings
                {
                    HtmlContent = paymuspdf.TheRegisterOfFinesForm5(),
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                _converter.Convert(pdf);


                return Ok(new { statusCode = "0", message = payslip_file_path });


            }
            catch (Exception ex)
            {

                return Ok(new { statusCode = "0", message = "" });

            }
        }

#endregion

#region ** START BY SUPRIYA ON 29-08-2019,FORM 10 REGISTER OF WAGES

        [Route("Save_RegisterofOvertimeFormXMaster")]
        [HttpPost]
        //[Authorize(Policy = "8162")]
        public IActionResult Save_RegisterofOvertimeFormXMaster([FromBody] tbl_muster_form10 objtbl)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                objtbl.is_deleted = 0;
                objtbl.created_date = DateTime.Now;

                _context.Entry(objtbl).State = EntityState.Added;
                _context.SaveChanges();


                objresponse.StatusCode = 0;
                objresponse.Message = "Successfully Saved...!";

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Get_RegisterOfFinesFormXMaster/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8163")]
        public IActionResult Get_RegisterOfFinesFormXMaster(int company_id)
        {
            try
            {
                var data = _context.tbl_muster_form10.Where(x => x.is_deleted == 0 && x.comp_id == company_id).OrderByDescending(x => x.form_X_mstr_id).FirstOrDefault();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("GetPayrollDetailsForMusterForm10/{payroll_month}/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8164")]
        public IActionResult GetPayrollDetailsForMusterForm10(int payroll_month, int company_id)
        {
            try
            {
                //Check If payroll_month data already freedz
                var get_freedz_data = _context.tbl_muster_form10_data.Where(a => a.company_id == company_id && a.payroll_month == payroll_month && a.is_deleted == 0)
                    .Select(a => new
                    {
                        a.employee_code,
                        employee_id = a.emp_id,
                        a.company_id,
                        a.payroll_month,
                        employee_name = a.employee_name,
                        father_or_husband_name = a.father_or_husband_name,
                        designation = a.designation,
                        a.basic_minimum_payable,
                        a.da_minimum_payable,
                        a.basic_wages_actually_pay,
                        a.da_wages_actually_pay,
                        a.total_attand_or_unitof_work_done,
                        a.overtime_worked,
                        a.gross_wages_pay,
                        a.contri_of_employer_pf,
                        a.hr_deduction,
                        a.other_deduction,
                        a.total_deduction,
                        a.wages_paid,
                        a.date_of_payment,
                        a.emp_sign_orthump_exp,
                        a.created_date
                    })
                    .ToList();

                if (get_freedz_data.Count > 0)
                {
                    return Ok(new { data = get_freedz_data, flag = 1 });
                }
                else
                {
                    var data = _context.tbl_salary_input.Where(a => a.is_active == 1 && a.monthyear == payroll_month && a.company_id == company_id).Select(p => new
                    {

                        employee_code = p.tem.emp_code,
                        employee_id = p.emp_id,
                        company_id = p.tem.tbl_employee_company_map.FirstOrDefault(x => x.is_deleted == 0).company_id,
                        payroll_month = p.monthyear,
                        employee_name = string.Format("{0} {1} {2}", p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                        p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => !string.IsNullOrEmpty(x.employee_middle_name)).employee_middle_name,
                        p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => !string.IsNullOrEmpty(x.employee_last_name)).employee_last_name),
                        father_or_husband_name = p.tem.tbl_emp_family_sec.Where(h => h.relation == "Father" && h.is_deleted == 0 && h.employee_id == p.emp_id).Select(h => h.name_as_per_aadhar_card).FirstOrDefault(),
                        designation = p.tem.tbl_emp_desi_allocation.OrderByDescending(d => d.emp_grade_id).FirstOrDefault(g => g.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name,
                        p.is_active,
                        p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).is_deleted,
                        basic_minimum_payable = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.basic_min_rate_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        da_minimum_payable = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.da_min_rate_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        basic_wages_actually_pay = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.basic_wages_actually_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        da_wages_actually_pay = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.da_wages_actually_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        total_attand_or_unitof_work_done = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.total_attan_orunit_ofworkdone_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        overtime_worked = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.overtime_worked_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),

                        gross_wages_pay = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.gross_wages_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        contri_of_employer_pf = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.employers_pf_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        hr_deduction = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.hr_deduction_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        other_deduction = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.other_deduction_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),

                        total_deduction = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.total_deduction_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),

                        wages_paid = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.wages_paid_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),

                        date_of_payment = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.date_ofpayment_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),


                    }).Distinct().ToList();


                    return Ok(new { data = data, flag = 0 });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("GetPayrollDetailsForMusterForm10ByEmployee/{payroll_month}/{employee_id}")]
        [HttpGet]
        //[Authorize(Policy = "8165")]
        public IActionResult GetPayrollDetailsForMusterForm10ByEmployee(int payroll_month, int employee_id)
        {
            try
            {

                var data = _context.tbl_muster_form10_data.Join(_context.tbl_emp_officaial_sec, a => a.emp_id, b => b.employee_id, (a, b) => new
                {
                    a.form_x_id,
                    employee_code = b.tbl_employee_id_details.emp_code,
                    employee_id = a.emp_id,
                    company_id = b.tbl_employee_id_details.tbl_user_master.Where(e => e.is_active == 1 && e.employee_id == a.emp_id).Select(e => e.default_company_id).FirstOrDefault(),
                    payroll_month = a.payroll_month,
                    employe_name = a.employee_name,
                    father_or_husband = a.father_or_husband_name,
                    a_is_active = a.is_deleted,
                    b_is_active = b.is_deleted,
                    desig_id = b.tbl_employee_id_details.tbl_emp_desi_allocation.OrderByDescending(g => g.emp_grade_id).FirstOrDefault(d => d.tbl_designation_master.is_active == 1).desig_id,
                    designation_name = b.tbl_employee_id_details.tbl_emp_desi_allocation.OrderByDescending(g => g.emp_grade_id).FirstOrDefault(d => d.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name,
                    basic_minimum_payable = a.basic_minimum_payable,
                    da_minimum_payable = a.da_minimum_payable,
                    basic_wages_actually_pay = a.basic_wages_actually_pay,
                    da_wages_actually_pay = a.da_wages_actually_pay,
                    total_attand_or_unitof_work_done = a.total_attand_or_unitof_work_done,
                    overtime_worked = a.overtime_worked,
                    gross_wages_pay = a.gross_wages_pay,
                    contri_of_employer_pf = a.contri_of_employer_pf,
                    hr_deduction = a.hr_deduction,
                    other_deduction = a.other_deduction,
                    total_deduction = a.total_deduction,
                    wages_paid = a.wages_paid,
                    date_of_payment = a.date_of_payment,
                    emp_sign_orthump_exp = a.emp_sign_orthump_exp,
                    b.employee_first_name
                }).Where(a => a.employee_id == employee_id && a.payroll_month == payroll_month && a.a_is_active == 0 && a.b_is_active == 0 && !string.IsNullOrEmpty(a.employee_first_name)).Distinct().FirstOrDefault(); ;


                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



#endregion ** END BY SUPIRYA ON 30-08-2019, FORM 10 REGISTER OF WAGES


#region START 30-08-2019  The Register of Wages in Form-X, Rule 26(a) ADD

        [Route("Save_PayrollMusterForm10Data")]
        [HttpPost]
        //[Authorize(Policy = "8166")]
        public IActionResult Save_PayrollMusterForm10Data([FromBody] tbl_muster_form10_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var data = _context.tbl_salary_input.Where(a => a.company_id == Obj_form_data.company_id
                && a.monthyear == Obj_form_data.payroll_month && a.is_active == 1 && a.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).is_deleted == 0).Select(p => new
                {
                    employee_code = p.tem.emp_code,
                    employee_id = p.emp_id,
                    company_id = p.tem.tbl_employee_company_map.FirstOrDefault(x => x.is_deleted == 0).company_id,
                    payroll_month = p.monthyear,
                    employe_name = string.Format("{0} {1} {2}", p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                        p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_middle_name)).employee_middle_name,
                        p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_last_name)).employee_last_name),
                    father_or_husband = p.tem.tbl_emp_family_sec.Where(h => h.relation == "Father" && h.is_deleted == 0 && h.employee_id == p.emp_id).Select(h => h.name_as_per_aadhar_card).FirstOrDefault(),
                    designation = p.tem.tbl_emp_desi_allocation.FirstOrDefault(x => x.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name,
                    p.is_active,
                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).is_deleted,

                    basic_minimum_payable = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.basic_min_rate_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    da_minimum_payable = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.da_min_rate_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    basic_wages_actually_pay = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.basic_wages_actually_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    da_wages_actually_pay = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.da_wages_actually_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    total_attand_or_unitof_work_done = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.total_attan_orunit_ofworkdone_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    overtime_worked = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.overtime_worked_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),

                    gross_wages_pay = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.gross_wages_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    contri_of_employer_pf = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.employers_pf_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    hr_deduction = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.hr_deduction_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    other_deduction = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.other_deduction_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),

                    total_deduction = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.total_deduction_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    wages_paid = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.wages_paid_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    date_of_payment = _context.tbl_salary_input.Join(_context.tbl_muster_form10, an => an.component_id, anc => anc.date_ofpayment_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_X_mstr_id, an.company_id }).OrderByDescending(v => v.form_X_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault()

                }).Distinct().ToList();


                List<tbl_muster_form10_data> objform10data = new List<tbl_muster_form10_data>();

                for (int Index = 0; Index < data.Count; Index++)
                {
                    tbl_muster_form10_data obj = new tbl_muster_form10_data()
                    {
                        emp_id = data[Index].employee_id,
                        company_id = data[Index].company_id,
                        payroll_month = data[Index].payroll_month,
                        employee_code = data[Index].employee_code,
                        employee_name = data[Index].employe_name,
                        father_or_husband_name = data[Index].father_or_husband,
                        designation = data[Index].designation,
                        basic_minimum_payable = data[Index].basic_minimum_payable,
                        da_minimum_payable = data[Index].da_minimum_payable,
                        basic_wages_actually_pay = data[Index].basic_wages_actually_pay,
                        da_wages_actually_pay = data[Index].da_wages_actually_pay,
                        total_attand_or_unitof_work_done = data[Index].total_attand_or_unitof_work_done,
                        overtime_worked = data[Index].overtime_worked,
                        gross_wages_pay = data[Index].gross_wages_pay,
                        contri_of_employer_pf = data[Index].contri_of_employer_pf,
                        hr_deduction = data[Index].hr_deduction,
                        other_deduction = data[Index].other_deduction,
                        total_deduction = data[Index].total_deduction,
                        wages_paid = data[Index].wages_paid,
                        date_of_payment = data[Index].date_of_payment,
                        emp_sign_orthump_exp = Obj_form_data.emp_sign_orthump_exp,
                        is_deleted = 0,
                        created_by = Obj_form_data.created_by,

                        created_date = DateTime.Now
                    };

                    objform10data.Add(obj);
                }


                _context.tbl_muster_form10_data.AddRange(objform10data);
                _context.SaveChangesAsync();

                objresponse.StatusCode = 0;
                objresponse.Message = "Data Successfully Saved...!";


                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Update_PayrollMusterForm10Data")]
        [HttpPost]
        //[Authorize(Policy = "8167")]
        public async Task<IActionResult> Update_PayrollMusterForm10Data()
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();
                var files = HttpContext.Request.Form.Files;
                var a = HttpContext.Request.Form["AllData"];
                if (a.ToString() == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid data !!";
                }
                else
                {
                    CommonClass com = new CommonClass();
                    tbl_muster_form10_data Obj_form_data = new tbl_muster_form10_data();

                    Obj_form_data = com.ToObjectFromJSON<tbl_muster_form10_data>(a.ToString());

                    var get_data = _context.tbl_muster_form10_data.Where(b => b.emp_id == Obj_form_data.emp_id && b.form_x_id == Obj_form_data.form_x_id && b.is_deleted == 0 && b.payroll_month == Obj_form_data.payroll_month).FirstOrDefault();

                    if (get_data == null)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Invalid details please try again !!";
                        return Ok(objresponse);
                    }
                    else
                    {
                        if (files.Count > 0)
                        {
                            //file upload logic
                            foreach (var FileData in files)
                            {
                                if (FileData != null && FileData.Length > 0)
                                {
                                    var allowedExtensions = new[] { ".Jpg", ".pdf", "jpeg", ".jpg", ".Pdf", "Jpeg", ".png" };

                                    var ext = Path.GetExtension(FileData.FileName); //getting the extension
                                    if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                                    {
                                        string name = Path.GetFileNameWithoutExtension(FileData.FileName); //getting file name without extension  
                                                                                                           //string MyFileName = get_data.employee_name + ext; //Guid.NewGuid().ToString().Replace("-", "") +
                                        string MyFileName = get_data.employee_name + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_tt") + ext; //Guid.NewGuid().ToString().Replace("-", "") +

                                        var webRoot = _hostingEnvironment.WebRootPath;

                                        if (!Directory.Exists(webRoot + "/EmployeeSignorThump/" + Obj_form_data.payroll_month + "/"))
                                        {
                                            Directory.CreateDirectory(webRoot + "/EmployeeSignorThump/" + Obj_form_data.payroll_month + "/");

                                        }


                                        if (!Directory.Exists(webRoot + "/EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_data.employee_code + "/"))
                                        {
                                            Directory.CreateDirectory(webRoot + "/EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_data.employee_code + "/");
                                        }

                                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_data.employee_code + "/");

                                        //var path = Path.Combine(Directory.GetCurrentDirectory(), "/wwwroot/HealthCard/" + currentyearmonth + "/" + get_emp_name_code.emp_code + "/" + get_emp_name_code.emp_name + extension);

                                        // var path = Path.Combine(Directory.GetCurrentDirectory(), "/HealthCard/" + currentyearmonth + "/" + get_emp_name_code.emp_code + "/");
                                        //save file

                                        using (var fileStream = new FileStream(Path.Combine(path, MyFileName), FileMode.Create))
                                        {
                                            FileData.CopyTo(fileStream);
                                            // objhealthcard.health_card_path = "/wwwroot/HealthCard/" + currentyearmonth + "/" + get_emp_name_code.emp_code + "/" + MyFileName;
                                            get_data.emp_sign_orthump_exp = "/EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_data.employee_code + "/" + MyFileName;
                                        }
                                    }
                                }
                            }
                        }
                        get_data.father_or_husband_name = Obj_form_data.father_or_husband_name;
                        get_data.basic_minimum_payable = Obj_form_data.basic_minimum_payable;
                        get_data.da_minimum_payable = Obj_form_data.da_minimum_payable;
                        get_data.basic_wages_actually_pay = Obj_form_data.basic_wages_actually_pay;
                        get_data.da_wages_actually_pay = Obj_form_data.da_wages_actually_pay;
                        get_data.total_attand_or_unitof_work_done = Obj_form_data.total_attand_or_unitof_work_done;
                        get_data.overtime_worked = Obj_form_data.overtime_worked;
                        get_data.gross_wages_pay = Obj_form_data.gross_wages_pay;
                        get_data.contri_of_employer_pf = Obj_form_data.contri_of_employer_pf;
                        get_data.hr_deduction = Obj_form_data.hr_deduction;
                        get_data.other_deduction = Obj_form_data.other_deduction;
                        get_data.total_deduction = Obj_form_data.total_deduction;
                        get_data.wages_paid = Obj_form_data.wages_paid;
                        get_data.date_of_payment = Obj_form_data.date_of_payment;
                        get_data.modified_by = Obj_form_data.modified_by;
                        get_data.modified_date = DateTime.Now;
                        _context.Entry(get_data).State = EntityState.Modified;
                        _context.SaveChanges();

                    }

                    // var get_emp_data = _context.tbl_emp_officaial_sec.Where(a => a.is_deleted == 0 && a.employee_first_name != "" && a.employee_id == Obj_form_data.emp_id).Select(b => new { emp_name = b.employee_first_name ?? "" + " " + b.employee_middle_name ?? "" + " " + b.employee_last_name ?? "", emp_code = b.tbl_employee_id_details.emp_code, dept = b.tbl_department_master.department_name }).FirstOrDefault();


                    objresponse.StatusCode = 0;
                    objresponse.Message = "Updated successfully !!";
                }



                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Add_PayrollMusterForm10Data")]
        [HttpPost]
        //[Authorize(Policy = "8168")]
        public async Task<IActionResult> Add_PayrollMusterForm10Data()
        {
            try
            {

                Response_Msg objresponse = new Response_Msg();
                var files = HttpContext.Request.Form.Files;
                var a = HttpContext.Request.Form["AllData"];
                if (a.ToString() == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid data !!";
                }
                else
                {
                    CommonClass com = new CommonClass();
                    tbl_muster_form10_data Obj_form_data = new tbl_muster_form10_data();

                    Obj_form_data = com.ToObjectFromJSON<tbl_muster_form10_data>(a.ToString());

                    var get_data = _context.tbl_muster_form10_data.Where(b => b.emp_id == Obj_form_data.emp_id && b.is_deleted == 0 && b.payroll_month == Obj_form_data.payroll_month).FirstOrDefault();

                    if (get_data != null)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Employee details already exist...!";
                    }
                    else
                    {
                        var get_emp_data = _context.tbl_emp_officaial_sec.Where(b => b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name) && b.employee_id == Obj_form_data.emp_id).Select(b => new { emp_name = string.Format("{0} {1} {2}", b.employee_first_name, b.employee_middle_name, b.employee_last_name), emp_code = b.tbl_employee_id_details.emp_code, designation = b.tbl_employee_id_details.tbl_emp_desi_allocation.FirstOrDefault(g => g.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name }).FirstOrDefault();


                        //file upload logic
                        foreach (var FileData in files)
                        {
                            if (FileData != null && FileData.Length > 0)
                            {
                                var allowedExtensions = new[] { ".Jpg", ".pdf", "jpeg", ".jpg", ".Pdf", "Jpeg" };

                                var ext = Path.GetExtension(FileData.FileName); //getting the extension
                                if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                                {
                                    string name = Path.GetFileNameWithoutExtension(FileData.FileName); //getting file name without extension  
                                                                                                       //string MyFileName = get_emp_data.emp_name + ext; //Guid.NewGuid().ToString().Replace("-", "") +

                                    string MyFileName = get_emp_data.emp_name + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_tt") + ext; //Guid.NewGuid().ToString().Replace("-", "") +
                                    var webRoot = _hostingEnvironment.WebRootPath;

                                    if (!Directory.Exists(webRoot + "/EmployeeSignorThump/" + Obj_form_data.payroll_month + "/"))
                                    {
                                        Directory.CreateDirectory(webRoot + "/EmployeeSignorThump/" + Obj_form_data.payroll_month + "/");

                                    }


                                    if (!Directory.Exists(webRoot + "/EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_emp_data.emp_code + "/"))
                                    {
                                        Directory.CreateDirectory(webRoot + "/EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_emp_data.emp_code + "/");
                                    }

                                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_emp_data.emp_code + "/");

                                    //var path = Path.Combine(Directory.GetCurrentDirectory(), "/wwwroot/HealthCard/" + currentyearmonth + "/" + get_emp_name_code.emp_code + "/" + get_emp_name_code.emp_name + extension);

                                    // var path = Path.Combine(Directory.GetCurrentDirectory(), "/HealthCard/" + currentyearmonth + "/" + get_emp_name_code.emp_code + "/");
                                    //save file
                                    using (var fileStream = new FileStream(Path.Combine(path, MyFileName), FileMode.Create))
                                    {
                                        FileData.CopyTo(fileStream);
                                        // objhealthcard.health_card_path = "/wwwroot/HealthCard/" + currentyearmonth + "/" + get_emp_name_code.emp_code + "/" + MyFileName;
                                        Obj_form_data.emp_sign_orthump_exp = "/EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_emp_data.emp_code + "/" + MyFileName;
                                    }
                                }
                            }
                        }




                        Obj_form_data.created_date = DateTime.Now;
                        Obj_form_data.employee_code = get_emp_data.emp_code;
                        Obj_form_data.employee_name = get_emp_data.emp_name;
                        Obj_form_data.designation = get_emp_data.designation;
                        Obj_form_data.is_deleted = 0;
                        Obj_form_data.created_by = Obj_form_data.created_by;

                        _context.Entry(Obj_form_data).State = EntityState.Added;
                        _context.SaveChanges();

                    }

                    objresponse.StatusCode = 0;
                    objresponse.Message = "Save successfully !!";

                }

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("Reset_PayrollMusterForm10Data")]
        [HttpPost]
        //[Authorize(Policy = "8169")]
        public IActionResult Reset_PayrollMusterForm10Data([FromBody] tbl_muster_form1_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var get_data = _context.tbl_muster_form10_data.Where(a => a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month && a.company_id == Obj_form_data.company_id).Select(b =>
                    b.form_x_id
                ).ToList();

                if (get_data == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid details please try again !!";
                    return Ok(objresponse);
                }
                else
                {

                    var update_whole_data = _context.tbl_muster_form10_data.Where(a => a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month && a.company_id == Obj_form_data.company_id && get_data.Contains(a.form_x_id)).ToList();
                    update_whole_data.ForEach(a =>
                    {
                        a.is_deleted = 1;
                        a.modified_by = Obj_form_data.modified_by;
                        a.modified_date = DateTime.Now;
                    });
                    _context.SaveChanges();

                }


                objresponse.StatusCode = 0;
                objresponse.Message = "Reset All Data Successfully !!";
                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



#endregion END



#region START 28-08-2019 PDF , FORM 10

        [Route("PayrollMuster10PdfGenerator/{company_id}/{payroll_month}")]
        [HttpGet]
        //[Authorize(Policy = "8170")]
        public IActionResult PayrollMuster10PdfGenerator(int company_id, int payroll_month)
        {
            try
            {
                var webRoot = _hostingEnvironment.WebRootPath;

                var company_name = _context.tbl_company_master.Where(a => a.is_active == 1 && a.company_id == company_id).Select(a => a.company_name).FirstOrDefault();

                if (!Directory.Exists(webRoot + "/PayrollMusters/" + company_name + "/" + payroll_month + "/"))
                {
                    Directory.CreateDirectory(webRoot + "/PayrollMusters/" + company_name + "/" + payroll_month + "/");
                }

                var globalSettings = new GlobalSettings
                {
                    DocumentTitle = "The Register of Fines in Form-I, Rule 21(4)",
                    PaperSize = PaperKind.A4Rotated,
                    Margins = new MarginSettings() { Top = 20, Bottom = 0, Left = 5, Right = 7 },
                    Out = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/PayrollMusters/" + company_name + "/" + payroll_month + "/" + "Register of Wages in Form-X, Rule 26(a)_" + DateTime.Now.Second + ".pdf"),
                };

                string payslip_file_path = "PayrollMusters/" + company_name + "/" + payroll_month + "/" + "Register of Wages in Form-X, Rule 26(a)_" + DateTime.Now.Second + ".pdf";

                PayrollMustersPdf paymuspdf = new PayrollMustersPdf(_context, company_id, payroll_month);


                var objectSettings = new ObjectSettings
                {
                    HtmlContent = paymuspdf.TheRegisterOfFinesForm10(),
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                _converter.Convert(pdf);


                return Ok(new { statusCode = "0", message = payslip_file_path });
            }
            catch (Exception ex)
            {
                return Ok(new { statusCode = "0", message = "There some problem please try after later...!" });
            }

        }

#endregion


#region ** START BY SUPRIYA ON 31-08-2019,FORM 11 REGISTER OF WAGE SLIP

        [Route("Save_RegisterofWageSlipFormXIMaster")]
        [HttpPost]
        //[Authorize(Policy = "8171")]
        public IActionResult Save_RegisterofWageSlipFormXIMaster([FromBody] tbl_muster_form11 objtbl)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                objtbl.is_deleted = 0;
                objtbl.created_date = DateTime.Now;

                _context.Entry(objtbl).State = EntityState.Added;
                _context.SaveChanges();


                objresponse.StatusCode = 0;
                objresponse.Message = "Successfully Saved...!";

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Get_RegisterOfWageSlipFormXIMaster/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8172")]
        public IActionResult Get_RegisterOfWageSlipFormXIMaster(int company_id)
        {
            try
            {
                var data = _context.tbl_muster_form11.Where(x => x.is_deleted == 0 && x.comp_id == company_id).OrderByDescending(x => x.form_XI_mstr_id).FirstOrDefault();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("GetPayrollDetailsForMusterForm11/{payroll_month}/{company_id}")]
        [HttpGet]
        //[Authorize(Policy = "8173")]
        public IActionResult GetPayrollDetailsForMusterForm11(int payroll_month, int company_id)
        {
            try
            {
                //Check If payroll_month data already freedz
                var get_freedz_data = _context.tbl_muster_form11_data.Where(a => a.company_id == company_id && a.payroll_month == payroll_month && a.is_deleted == 0)
                    .Select(a => new
                    {
                        a.employee_code,
                        employee_id = a.emp_id,
                        a.company_id,
                        a.payroll_month,
                        employee_name = a.employee_name,
                        father_or_husband_name = a.father_or_husband_name,
                        designation = a.designation,
                        basic_wage_payable = a.basic_wage_payable,
                        da_wage_payable = a.da_wage_payable,
                        total_attand_orwork_done = a.total_attand_orwork_done,
                        overtime_wages = a.overtime_wages,
                        gross_wage_pay = a.gross_wage_pay,
                        total_deduction = a.total_deduction,
                        net_wage_pay = a.net_wage_pay,
                        pay_incharge = a.pay_incharge,
                        emp_sign_orthump_exp = a.emp_sign_orthump_exp
                    })
                    .ToList();

                if (get_freedz_data.Count > 0)
                {
                    return Ok(new { data = get_freedz_data, flag = 1 });
                }
                else
                {
                    var data = _context.tbl_salary_input.Where(a => a.company_id == company_id && a.monthyear == payroll_month && a.is_active == 1 && a.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).is_deleted == 0).Select(p => new
                    {
                        employee_code = p.tem.emp_code,
                        employee_id = p.emp_id,
                        company_id = p.tem.tbl_employee_company_map.FirstOrDefault(x => x.is_deleted == 0).company_id,
                        payroll_month = p.monthyear,
                        employee_name = string.Format("{0} {1} {2}", p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                                p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_middle_name)).employee_middle_name,
                                p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_last_name)).employee_last_name),
                        father_or_husband_name = p.tem.tbl_emp_family_sec.Where(h => h.relation == "Father" && h.is_deleted == 0 && h.employee_id == p.emp_id).Select(h => h.name_as_per_aadhar_card).FirstOrDefault(),
                        designation = p.tem.tbl_emp_desi_allocation.OrderByDescending(x => x.emp_grade_id).FirstOrDefault(x => x.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name,
                        a_is_active = p.is_active,
                        b_is_active = p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).is_deleted,

                        basic_wage_payable = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.basic_wages_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        da_wage_payable = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.da_wages_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        total_attand_orwork_done = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.total_attand_orwork_done_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        overtime_wages = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.overtime_wages_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        gross_wage_pay = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.gross_wages_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        total_deduction = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.total_deduction_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),

                        net_wage_pay = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.net_wages_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),
                        pay_incharge = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.pay_incharge_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == payroll_month && an.is_active == 1 && an.company_id == company_id).Select(anc => anc.values).FirstOrDefault(),

                    }).Distinct().ToList();



                    return Ok(new { data = data, flag = 0 });
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("GetPayrollDetailsForMusterForm11ByEmployee/{payroll_month}/{employee_id}")]
        [HttpGet]
        //[Authorize(Policy = "8174")]
        public IActionResult GetPayrollDetailsForMusterForm11ByEmployee(int payroll_month, int employee_id)
        {
            try
            {

                var data = _context.tbl_muster_form11_data.Join(_context.tbl_emp_officaial_sec, a => a.emp_id, b => b.employee_id, (a, b) => new
                {
                    a.form_xi_id,
                    employee_code = b.tbl_employee_id_details.emp_code,
                    employee_id = a.emp_id,
                    company_id = b.tbl_employee_id_details.tbl_user_master.Where(e => e.is_active == 1 && e.employee_id == a.emp_id).Select(e => e.default_company_id).FirstOrDefault(),
                    payroll_month = a.payroll_month,
                    employe_name = a.employee_name,
                    father_or_husband = a.father_or_husband_name,
                    a_is_active = a.is_deleted,
                    b_is_active = b.is_deleted,
                    desig_id = b.tbl_employee_id_details.tbl_emp_desi_allocation.OrderByDescending(g => g.emp_grade_id).FirstOrDefault(d => d.tbl_designation_master.is_active == 1).desig_id,
                    designation_name = b.tbl_employee_id_details.tbl_emp_desi_allocation.OrderByDescending(g => g.emp_grade_id).FirstOrDefault(d => d.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name,
                    basic_wage_payable = a.basic_wage_payable,
                    da_wage_payable = a.da_wage_payable,
                    total_attand_orwork_done = a.total_attand_orwork_done,
                    overtime_wages = a.overtime_wages,
                    gross_wage_pay = a.gross_wage_pay,
                    total_deduction = a.total_deduction,
                    net_wage_pay = a.net_wage_pay,
                    pay_incharge = a.pay_incharge,
                    emp_sign_orthump_exp = a.emp_sign_orthump_exp,
                    b.employee_first_name
                }).Where(a => a.employee_id == employee_id && a.payroll_month == payroll_month && a.a_is_active == 0 && a.b_is_active == 0 && !string.IsNullOrEmpty(a.employee_first_name)).Distinct().FirstOrDefault(); ;


                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


#endregion


#region START 31-08-2019  The Register of Wage Slip in Form-XI, Rule 26(2) ADD

        [Route("Save_PayrollMusterForm11Data")]
        [HttpPost]
        //[Authorize(Policy = "8175")]
        public IActionResult Save_PayrollMusterForm11Data([FromBody] tbl_muster_form11_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var data = _context.tbl_salary_input.Where(a => a.company_id == Obj_form_data.company_id
                && a.monthyear == Obj_form_data.payroll_month && a.is_active == 1 && a.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).is_deleted == 0).Select(p => new
                {
                    employee_code = p.tem.emp_code,
                    employee_id = p.emp_id,
                    company_id = p.tem.tbl_employee_company_map.FirstOrDefault(x => x.is_deleted == 0).company_id,
                    payroll_month = p.monthyear,
                    employe_name = string.Format("{0} {1} {2}", p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                        p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_middle_name)).employee_middle_name,
                        p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_last_name)).employee_last_name),
                    father_or_husband = p.tem.tbl_emp_family_sec.Where(h => h.relation == "Father" && h.is_deleted == 0 && h.employee_id == p.emp_id).Select(h => h.name_as_per_aadhar_card).FirstOrDefault(),
                    designation = p.tem.tbl_emp_desi_allocation.FirstOrDefault(x => x.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name,
                    p.is_active,
                    p.tem.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0).is_deleted,

                    basic_wage_payable = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.basic_wages_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    da_wage_payable = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.da_wages_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    total_attand_orwork_done = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.total_attand_orwork_done_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    overtime_wages = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.overtime_wages_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    gross_wage_pay = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.gross_wages_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    total_deduction = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.total_deduction_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),

                    net_wage_pay = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.net_wages_pay_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                    pay_incharge = _context.tbl_salary_input.Join(_context.tbl_muster_form11, an => an.component_id, anc => anc.pay_incharge_c_id, (an, anc) => new { an.emp_id, an.values, an.monthyear, an.is_active, anc.form_XI_mstr_id, an.company_id }).OrderByDescending(v => v.form_XI_mstr_id).Where(an => an.emp_id == p.emp_id && an.monthyear == Obj_form_data.payroll_month && an.is_active == 1 && an.company_id == Obj_form_data.company_id).Select(anc => anc.values).FirstOrDefault(),
                }).Distinct().ToList();


                List<tbl_muster_form11_data> objform11data = new List<tbl_muster_form11_data>();

                for (int Index = 0; Index < data.Count; Index++)
                {
                    tbl_muster_form11_data obj = new tbl_muster_form11_data()
                    {
                        emp_id = data[Index].employee_id,
                        company_id = data[Index].company_id,
                        payroll_month = data[Index].payroll_month,
                        employee_code = data[Index].employee_code,
                        employee_name = data[Index].employe_name,
                        father_or_husband_name = data[Index].father_or_husband,
                        designation = data[Index].designation,
                        basic_wage_payable = data[Index].basic_wage_payable,
                        da_wage_payable = data[Index].da_wage_payable,
                        total_attand_orwork_done = data[Index].total_attand_orwork_done,
                        overtime_wages = data[Index].overtime_wages,
                        gross_wage_pay = data[Index].gross_wage_pay,
                        total_deduction = data[Index].total_deduction,
                        net_wage_pay = data[Index].net_wage_pay,
                        pay_incharge = data[Index].pay_incharge,
                        is_deleted = 0,
                        created_by = Obj_form_data.created_by,

                        created_date = DateTime.Now
                    };

                    objform11data.Add(obj);
                }


                _context.tbl_muster_form11_data.AddRange(objform11data);
                _context.SaveChangesAsync();

                objresponse.StatusCode = 0;
                objresponse.Message = "Data Successfully Saved...!";


                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Update_PayrollMusterForm11Data")]
        [HttpPost]
        //[Authorize(Policy = "8176")]
        public async Task<IActionResult> Update_PayrollMusterForm11Data()
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();
                var files = HttpContext.Request.Form.Files;
                var a = HttpContext.Request.Form["AllData"];
                if (a.ToString() == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid data !!";
                    return Ok(objresponse);
                }
                else
                {
                    CommonClass com = new CommonClass();
                    tbl_muster_form11_data Obj_form_data = new tbl_muster_form11_data();

                    Obj_form_data = com.ToObjectFromJSON<tbl_muster_form11_data>(a.ToString());


                    string _result = validateparyollmuster(Obj_form_data).ToString();

                    if (_result != "")
                    {

                        objresponse.StatusCode = 1;
                        objresponse.Message = _result;
                        return Ok(_result);
                    }




                    var get_data = _context.tbl_muster_form11_data.Where(b => b.emp_id == Obj_form_data.emp_id && b.form_xi_id == Obj_form_data.form_xi_id && b.is_deleted == 0 && b.payroll_month == Obj_form_data.payroll_month).FirstOrDefault();

                    if (get_data == null)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Invalid details please try again !!";
                        return Ok(objresponse);
                    }
                    else
                    {
                        string _imagepathh = "";
                        if (files.Count > 0)
                        {
                            var allowedExtensions = new[] { ".Jpg", "jpeg", ".jpg", "Jpeg", };

                            var ext = Path.GetExtension(files[0].FileName); //getting the extension
                            if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                            {
                                string name = Path.GetFileNameWithoutExtension(files[0].FileName); //getting file name without extension  
                                                                                                   //string MyFileName = get_data.employee_name + ext; //Guid.NewGuid().ToString().Replace("-", "") +
                                string MyFileName = get_data.employee_name + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_tt") + ext; //Guid.NewGuid().ToString().Replace("-", "") +

                                var webRoot = _hostingEnvironment.WebRootPath;

                                if (!Directory.Exists(webRoot + "/Form11EmployeeSignorThump/" + Obj_form_data.payroll_month + "/"))
                                {
                                    Directory.CreateDirectory(webRoot + "/Form11EmployeeSignorThump/" + Obj_form_data.payroll_month + "/");

                                }


                                if (!Directory.Exists(webRoot + "/Form11EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_data.employee_code + "/"))
                                {
                                    Directory.CreateDirectory(webRoot + "/Form11EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_data.employee_code + "/");
                                }

                                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/Form11EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_data.employee_code + "/");

                                //var path = Path.Combine(Directory.GetCurrentDirectory(), "/wwwroot/HealthCard/" + currentyearmonth + "/" + get_emp_name_code.emp_code + "/" + get_emp_name_code.emp_name + extension);

                                // var path = Path.Combine(Directory.GetCurrentDirectory(), "/HealthCard/" + currentyearmonth + "/" + get_emp_name_code.emp_code + "/");
                                //save file

                                using (var fileStream = new FileStream(Path.Combine(path, MyFileName), FileMode.Create))
                                {
                                    files[0].CopyTo(fileStream);
                                    // objhealthcard.health_card_path = "/wwwroot/HealthCard/" + currentyearmonth + "/" + get_emp_name_code.emp_code + "/" + MyFileName;
                                    // get_data.emp_sign_orthump_exp = "/Form11EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_data.employee_code + "/" + MyFileName;

                                    _imagepathh = "/Form11EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_data.employee_code + "/" + MyFileName;
                                }

                            }
                            else
                            {
                                objresponse.StatusCode = 1;
                                objresponse.Message = "Please Upload only JPG, JPEG File";

                            }
                        }
                        else
                        {
                            _imagepathh = get_data.emp_sign_orthump_exp;
                        }

                        get_data.father_or_husband_name = Obj_form_data.father_or_husband_name;
                        get_data.basic_wage_payable = Obj_form_data.basic_wage_payable;
                        get_data.da_wage_payable = Obj_form_data.da_wage_payable;
                        get_data.total_attand_orwork_done = Obj_form_data.total_attand_orwork_done;
                        get_data.overtime_wages = Obj_form_data.overtime_wages;
                        get_data.gross_wage_pay = Obj_form_data.gross_wage_pay;
                        get_data.total_deduction = Obj_form_data.total_deduction;
                        get_data.net_wage_pay = Obj_form_data.net_wage_pay;
                        get_data.pay_incharge = Obj_form_data.pay_incharge;
                        get_data.emp_sign_orthump_exp = _imagepathh;
                        get_data.modified_by = Obj_form_data.modified_by;
                        get_data.modified_date = DateTime.Now;
                        _context.Entry(get_data).State = EntityState.Modified;
                        _context.SaveChanges();

                        objresponse.StatusCode = 0;
                        objresponse.Message = "Updated successfully !!";




                    }

                    return Ok(objresponse);
                }





            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        [Route("Add_PayrollMusterForm11Data")]
        [HttpPost]
        //[Authorize(Policy = "8177")]
        public async Task<IActionResult> Add_PayrollMusterForm11Data()
        {
            try
            {

                Response_Msg objresponse = new Response_Msg();
                var files = HttpContext.Request.Form.Files;
                var a = HttpContext.Request.Form["AllData"];
                if (a.ToString() == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid data !!";
                    return Ok(objresponse);
                }
                else
                {
                    CommonClass com = new CommonClass();
                    tbl_muster_form11_data Obj_form_data = new tbl_muster_form11_data();

                    Obj_form_data = com.ToObjectFromJSON<tbl_muster_form11_data>(a.ToString());

                    string _result = validateparyollmuster(Obj_form_data).ToString();

                    if (_result != "")
                    {

                        objresponse.StatusCode = 1;
                        objresponse.Message = _result;
                        return Ok(_result);
                    }




                    var get_data = _context.tbl_muster_form11_data.Where(b => b.emp_id == Obj_form_data.emp_id && b.is_deleted == 0 && b.payroll_month == Obj_form_data.payroll_month).FirstOrDefault();

                    if (get_data != null)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Employee details already exist...!";

                        return Ok(objresponse);
                    }
                    else
                    {
                        var get_emp_data = _context.tbl_emp_officaial_sec.Where(b => b.is_deleted == 0 && !string.IsNullOrEmpty(b.employee_first_name) && b.employee_id == Obj_form_data.emp_id).Select(b => new { emp_name = string.Format("{0} {1} {2}", b.employee_first_name, b.employee_middle_name, b.employee_last_name), emp_code = b.tbl_employee_id_details.emp_code, designation = b.tbl_employee_id_details.tbl_emp_desi_allocation.FirstOrDefault(g => g.tbl_designation_master.is_active == 1).tbl_designation_master.designation_name }).FirstOrDefault();

                        if (files.Count > 0)
                        {
                            //foreach (var FileData in files)
                            //{
                            var allowedExtensions = new[] { ".Jpg", "jpeg", ".jpg", "Jpeg" };

                            var ext = Path.GetExtension(files[0].FileName); //getting the extension
                            if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                            {
                                string name = Path.GetFileNameWithoutExtension(files[0].FileName); //getting file name without extension  
                                                                                                   //string MyFileName = get_emp_data.emp_name + ext; //Guid.NewGuid().ToString().Replace("-", "") +

                                string MyFileName = get_emp_data.emp_name + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_tt") + ext; //Guid.NewGuid().ToString().Replace("-", "") +
                                var webRoot = _hostingEnvironment.WebRootPath;

                                if (!Directory.Exists(webRoot + "/Form11EmployeeSignorThump/" + Obj_form_data.payroll_month + "/"))
                                {
                                    Directory.CreateDirectory(webRoot + "/Form11EmployeeSignorThump/" + Obj_form_data.payroll_month + "/");

                                }


                                if (!Directory.Exists(webRoot + "/Form11EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_emp_data.emp_code + "/"))
                                {
                                    Directory.CreateDirectory(webRoot + "/Form11EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_emp_data.emp_code + "/");
                                }

                                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/Form11EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_emp_data.emp_code + "/");

                                //var path = Path.Combine(Directory.GetCurrentDirectory(), "/wwwroot/HealthCard/" + currentyearmonth + "/" + get_emp_name_code.emp_code + "/" + get_emp_name_code.emp_name + extension);

                                // var path = Path.Combine(Directory.GetCurrentDirectory(), "/HealthCard/" + currentyearmonth + "/" + get_emp_name_code.emp_code + "/");
                                //save file
                                using (var fileStream = new FileStream(Path.Combine(path, MyFileName), FileMode.Create))
                                {
                                    files[0].CopyTo(fileStream);
                                    // objhealthcard.health_card_path = "/wwwroot/HealthCard/" + currentyearmonth + "/" + get_emp_name_code.emp_code + "/" + MyFileName;
                                    Obj_form_data.emp_sign_orthump_exp = "/Form11EmployeeSignorThump/" + Obj_form_data.payroll_month + "/" + get_emp_data.emp_code + "/" + MyFileName;
                                }


                                Obj_form_data.created_date = DateTime.Now;
                                Obj_form_data.employee_code = get_emp_data.emp_code;
                                Obj_form_data.employee_name = get_emp_data.emp_name;
                                Obj_form_data.designation = get_emp_data.designation;
                                Obj_form_data.is_deleted = 0;
                                Obj_form_data.created_by = Obj_form_data.created_by;

                                _context.Entry(Obj_form_data).State = EntityState.Added;
                                _context.SaveChanges();


                                objresponse.StatusCode = 0;
                                objresponse.Message = "Save successfully !!";

                            }
                            else
                            {
                                objresponse.StatusCode = 1;
                                objresponse.Message = "Please Upload only JPG, JPEG File";
                            }
                            //}

                        }
                        else
                        {
                            objresponse.StatusCode = 1;
                            objresponse.Message = "Please Select File";
                        }
                        //file upload logic
                        return Ok(objresponse);
                    }





                }


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        private string validateparyollmuster(tbl_muster_form11_data Obj_form_data)
        {

            StringBuilder message = new StringBuilder();

            string empnameRegex = @"^[a-zA-Z'\s'\.]{1,150}$";
            string amount = @"^[a-zA-Z0-9'\s'\.]{1,40}$";

            Regex rename = new Regex(empnameRegex);

            Regex reamount = new Regex(amount);

            //if (!rename.IsMatch(Obj_form_data.employee_name))
            //{
            //    result.Add("Invalid Employee Name");
            //}
            //else if (!rename.IsMatch(Obj_form_data.father_or_husband_name))
            //{
            //    result.Add("Invalid Father Name");
            //}
            if (!reamount.IsMatch(Obj_form_data.basic_wage_payable))
            {
                message.Append("Invalid Basic wages Payable </br>");
            }
            if (!reamount.IsMatch(Obj_form_data.da_wage_payable))
            {
                message.Append("Invalid DA Wages Payable</br>");
            }
            if (!reamount.IsMatch(Obj_form_data.total_attand_orwork_done))
            {
                message.Append("Invalid Total Attendance or Work Done</br>");
            }
            if (!reamount.IsMatch(Obj_form_data.overtime_wages))
            {
                message.Append("Invalid Overtime Wages</br>");
            }
            if (!reamount.IsMatch(Obj_form_data.gross_wage_pay))
            {
                message.Append("Invalid Gross Wages Pay</br>");
            }
            if (!reamount.IsMatch(Obj_form_data.total_deduction))
            {
                message.Append("Invalid Total Deduction</br>");
            }
            if (!reamount.IsMatch(Obj_form_data.net_wage_pay))
            {
                message.Append("Invalid Net Wages Pay</br>");
            }
            if (!reamount.IsMatch(Obj_form_data.pay_incharge))
            {
                message.Append("Invalid Payin Charge </br>");
            }

            return message.ToString();
        }

        [Route("Reset_PayrollMusterForm11Data")]
        [HttpPost]
        //[Authorize(Policy = "8178")]
        public IActionResult Reset_PayrollMusterForm11Data([FromBody] tbl_muster_form11_data Obj_form_data)
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();

                var get_data = _context.tbl_muster_form11_data.Where(a => a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month && a.company_id == Obj_form_data.company_id).Select(b =>
                    b.form_xi_id
                ).ToList();

                if (get_data == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid details please try again !!";
                    return Ok(objresponse);
                }
                else
                {

                    var update_whole_data = _context.tbl_muster_form11_data.Where(a => a.is_deleted == 0 && a.payroll_month == Obj_form_data.payroll_month && a.company_id == Obj_form_data.company_id && get_data.Contains(a.form_xi_id)).ToList();
                    update_whole_data.ForEach(a =>
                    {
                        a.is_deleted = 1;
                        a.modified_by = Obj_form_data.modified_by;
                        a.modified_date = DateTime.Now;
                    });
                    _context.SaveChanges();

                }


                objresponse.StatusCode = 0;
                objresponse.Message = "Reset All Data Successfully !!";
                return Ok(objresponse);

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



#endregion END


#region ** START BY SUPRIYA ON 02-11-2019,GET EMPLOYEE SALARY GROUP**

        [Route("Get_EmpSalaryGroup/{company_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.EmpSalary))]
        public IActionResult Get_EmpSalaryGroup([FromRoute] int company_id)
        {

            try
            {
                _clsEmployeeDetail._company_id = company_id;

                List<EmployeeOfficaialSection> data = _clsEmployeeDetail.BindEmpSalaryGroup();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }


        }
#endregion ** START BY SUPRIYA ON 02-11-2019,GET EMPLOYEE SALARY GROUP**




#region ** START BY SUPRIYA ON 07-11-2019, UPLOAD INCOME TAX DETAIL**

        // 
        [Route("Save_EmpTaxDetailUpload")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.EmployeeTax))]
        public async Task<IActionResult> Save_EmpTaxDetailUpload()
        {
            try
            {
                Response_Msg objresponse = new Response_Msg();
                var files = HttpContext.Request.Form.Files;
                var a = HttpContext.Request.Form["AllData"];
                if (string.IsNullOrEmpty(a.ToString()))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Invalid Data !!";
                    return Ok(objresponse);
                }

                CommonClass com = new CommonClass();
                EmployeeTaxDetail objemptaxdtl = new EmployeeTaxDetail();
                objemptaxdtl = com.ToObjectFromJSON<EmployeeTaxDetail>(a.ToString());


                //open the excel using openxml sdk  
                StringBuilder excelResult = new StringBuilder();
                List<EmployeeTaxDetail> emptaxlist = new List<EmployeeTaxDetail>();

                string get_file_path = "";

                if (files[0] != null && files[0].Length > 0)
                {
                    var allowedExtensions = new[] { ".xlsx" };
                    var ext = Path.GetExtension(files[0].FileName); //getting the extension
                    if (allowedExtensions.Contains(ext.ToLower()))//check what type of extension  
                    {
                        string name = Path.GetFileNameWithoutExtension(files[0].FileName); //getting file name without extension  
                                                                                           //string company_name = _context.tbl_company_master.OrderByDescending(x => x.company_id).Where(y => y.company_id == tbl.default_company_id && y.is_active == 1).Select(p => p.company_name).FirstOrDefault();
                        string MyFileName = "EmpTaxDetail_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_tt") + ext; //Guid.NewGuid().ToString().Replace("-", "") +

                        var webRoot = _hostingEnvironment.WebRootPath;

                        string currentmonth = Convert.ToString(DateTime.Now.Month).Length.ToString() == "1" ? "0" + Convert.ToString(DateTime.Now.Month) : Convert.ToString(DateTime.Now.Month);

                        var currentyearmonth = Convert.ToString(DateTime.Now.Year) + currentmonth;

                        if (!Directory.Exists(webRoot + "/EmployeeDetail/TaxDetail/" + currentyearmonth + "/"))
                        {
                            Directory.CreateDirectory(webRoot + "/EmployeeDetail/TaxDetail/" + currentyearmonth + "/");
                        }

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/EmployeeDetail/TaxDetail/" + currentyearmonth + "/");

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
                        objresponse.Message = "Please Upload Only Excel File";
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
                                    EmployeeTaxDetail objemp = new EmployeeTaxDetail();
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
                                                                objemp.emp_code = item.Text.Text;
                                                            }
                                                            else if (!string.IsNullOrEmpty(item.Text.Text) && currentcolumnno == "C")
                                                            {
                                                                objemp.income_tax_amount = Convert.ToDouble(item.Text.Text);
                                                            }

#endregion ** END value in list**
                                                        }
                                                        //else if (item.InnerText != null)
                                                        //{
                                                        //    currentcellvalue = item.InnerText;
                                                        //}
                                                        //else if (item.InnerXml != null)
                                                        //{
                                                        //    currentcellvalue = item.InnerXml;
                                                        //}
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
                                                    objemp.emp_code = thecurrentcell.InnerText;
                                                }
                                                else if (!string.IsNullOrEmpty(thecurrentcell.InnerText) && currentcolumnno == "C")
                                                {
                                                    objemp.income_tax_amount = Convert.ToDouble(thecurrentcell.InnerText);
                                                }

#endregion ** END value in list**

                                            }

                                        }

                                    }
                                    excelResult.AppendLine();
                                    objemp.created_by = objemptaxdtl.created_by;
                                    objemp.company_id = objemptaxdtl.company_id;
                                    emptaxlist.Add(objemp);
                                }

                            }
                            excelResult.Append("");
                        }

                    }

                    if (!_clsCurrentUser.CompanyId.Any(p => emptaxlist.Any(q => q.company_id == p)))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Unauthorize Company Access....!!";
                        return Ok(objresponse);
                    }


                    var data = excelResult.ToString();
                    var data1 = emptaxlist;

                    var dataaa = CheckEmpTaxDetailsfromexcel(emptaxlist);

                    var duplicate_dtl = dataaa.duplicateemptaxdtl;
                    var missing_dtll = dataaa.missingemptaxdtl;
                    var adddblistt = dataaa.addbemptaxdtl;
                    var missingDtlMessage = dataaa.MissingDtlMessage;

                    if (duplicate_dtl.Count > 0 || missing_dtll.Count > 0)
                    {
                        return Ok(dataaa);
                    }
                    else
                    {
                        if (!_clsCurrentUser.DownlineEmpId.Any(p => adddblistt.Any(q => q.emp_id == p)))
                        {
                            objresponse.StatusCode = 1;
                            objresponse.Message = "Unauthorize Access";
                            return Ok(objresponse);
                        }

                        int result = SaveEmpTaxDetailFromExcel(adddblistt);
                        if (result == 0)
                        {
                            objresponse.StatusCode = 0;
                            objresponse.Message = "Employee Tax details successfully saved..";
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
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unable to read File Path";
                    return Ok(objresponse);
                }


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        private EmployeeDetailListt CheckEmpTaxDetailsfromexcel(List<EmployeeTaxDetail> list)
        {
            List<EmployeeTaxDetail> missingdetaillist = new List<EmployeeTaxDetail>();
            List<EmployeeTaxDetail> adddblist = new List<EmployeeTaxDetail>();
            List<EmployeeTaxDetail> duplicatedetaillist = new List<EmployeeTaxDetail>();
            Response_Msg objresponse = new Response_Msg();
            bool _emp_code_not_exist = false;
            bool _company_not_map = false;
            bool _amount = false;

            int _company_id = 0;
            int employee_id = 0;

            StringBuilder MissingDtlMessage = new StringBuilder();
            try
            {
                MissingDtlMessage.Append("");

                for (int i = 0; i < list.Count; i++)
                {
                    MissingDtlMessage.Append(list[i].emp_code + " Detail:-");

                    if (string.IsNullOrEmpty(list[i].emp_code))
                    {
                        MissingDtlMessage.Append("Emp Code is missing,");
                    }
                    else
                    {
                        var _emp_code_exist = _context.tbl_employee_master.OrderByDescending(x => x.employee_id).FirstOrDefault(a => a.emp_code == list[i].emp_code && a.is_active == 1);
                        if (_emp_code_exist != null)
                        {
                            employee_id = _emp_code_exist.employee_id;
                            _company_id = _context.tbl_employee_company_map.OrderByDescending(x => x.sno).FirstOrDefault(x => x.employee_id == _emp_code_exist.employee_id && x.is_deleted == 0).company_id ?? 0;
                            if (_company_id == 0)
                            {
                                _company_not_map = true;
                                MissingDtlMessage.Append("Employee Detail Not Exist..,");
                            }
                        }
                        else
                        {
                            MissingDtlMessage.Append(" Emp Code not exist,");
                            _emp_code_not_exist = true;
                        }
                    }

                    if (list[i].income_tax_amount == 0)
                    {
                        _amount = true;
                        MissingDtlMessage.Append("Amount not available");
                    }

                    if (list[i].income_tax_amount.GetType() != typeof(double))
                    {
                        _amount = true;
                        MissingDtlMessage.Append("Inccorect amount");
                    }



                    if (_emp_code_not_exist || _company_not_map || _amount)
                    {
                        EmployeeTaxDetail objemptax = new EmployeeTaxDetail();
                        objemptax.emp_id = employee_id;
                        objemptax.emp_code = list[i].emp_code;
                        // objemptax.company_id = list[i].company_id;
                        objemptax.income_tax_amount = list[i].income_tax_amount;

                        missingdetaillist.Add(objemptax);

                        MissingDtlMessage.Append("</br>");
                    }
                    else
                    {

                        bool _checkduplicate = adddblist.Any(x => x.emp_code == list[i].emp_code && x.emp_id == employee_id);

                        if (_checkduplicate) // Add in this list if any one field detail is found duplicate in list
                        {
                            EmployeeTaxDetail objduplist = new EmployeeTaxDetail();

                            objduplist.emp_id = employee_id;
                            objduplist.emp_code = list[i].emp_code;
                            objduplist.income_tax_amount = list[i].income_tax_amount;

                            duplicatedetaillist.Add(objduplist);
                            MissingDtlMessage.Append("Duplicate Entry in Excel</br>");
                        }
                        else
                        {
                            EmployeeTaxDetail objlist = new EmployeeTaxDetail();
                            objlist.emp_id = employee_id;
                            objlist.emp_code = list[i].emp_code;
                            objlist.income_tax_amount = list[i].income_tax_amount;
                            objlist.created_by = list[i].created_by;
                            adddblist.Add(objlist);
                        }

                    }

                }
            }
            catch (Exception ex)
            {

            }

            return new EmployeeDetailListt { missingemptaxdtl = missingdetaillist, duplicateemptaxdtl = duplicatedetaillist, addbemptaxdtl = adddblist, MissingDtlMessage = MissingDtlMessage.ToString() };
        }

        private int SaveEmpTaxDetailFromExcel(List<EmployeeTaxDetail> objdblist)
        {
            try
            {
                bool _rollbanck = false;


                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {

                        for (int i = 0; i < objdblist.Count; i++)
                        {

                            tbl_employee_income_tax_amount objempincometax = new tbl_employee_income_tax_amount();

                            List<tbl_employee_income_tax_amount> update_income_tax_previous = _context.tbl_employee_income_tax_amount.Where(x => x.emp_id == objdblist[i].emp_id && x.is_deleted == 0).ToList();
                            if (update_income_tax_previous.Count > 0)
                            {
                                update_income_tax_previous.ForEach(p =>
                                {
                                    p.is_deleted = 1;
                                    p.last_modified_by = objdblist[i].created_by;
                                    p.last_modified_date = DateTime.Now;
                                });
                                _context.tbl_employee_income_tax_amount.UpdateRange(update_income_tax_previous);
                                _context.SaveChanges();

                            }



                            objempincometax.emp_id = objdblist[i].emp_id;
                            objempincometax.income_tax_amount = objdblist[i].income_tax_amount;
                            objempincometax.is_deleted = 0;
                            objempincometax.created_by = objdblist[i].created_by;
                            objempincometax.created_date = DateTime.Now;


                            _context.tbl_employee_income_tax_amount.Add(objempincometax);
                            _context.SaveChanges();

                        }


                        if (_rollbanck)
                        {
                            trans.Rollback();
                            return -1;
                        }
                        else
                        {
                            trans.Commit();
                            return 0;
                        }


                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return -2;
                    }


                }

            }
            catch (Exception ex)
            {
                return 1;

            }

        }



#endregion ** END BY SUPRIYA ON 07-11-2019, UPLOAD INCOME TAX DETAIL **


#region **START BY SUPRIYA ON 19-12-2019,UPLOAD SALARYINPUT
        [Route("Upload_SalaryInput")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.PayrollInput))]
        public async Task<IActionResult> Upload_SalaryInput()
        {
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
                SalaryInput objsalary = new SalaryInput();
                objsalary = objcom.ToObjectFromJSON<SalaryInput>(a.ToString());









                //open excel using openxml sdk
                StringBuilder excelResult = new StringBuilder();
                List<SalaryInput> salaryinputlst = new List<SalaryInput>();
                string get_file_path = "";



                for (int i = 0; i < files.Count; i++)
                {
                    if (files[i].Length > 0 && files[i] != null)
                    {
                        var allowedExtensions = new[] { ".xlsx" };

                        var ext = Path.GetExtension(files[i].FileName); //getting the extension
                        if (allowedExtensions.Contains(ext.ToLower()))//check what type of extension  
                        {
                            string name = Path.GetFileNameWithoutExtension(files[i].FileName); //getting file name without extension  

                            string company_name = _context.tbl_company_master.OrderByDescending(x => x.company_id).Where(y => y.company_id == objsalary.company_id && y.is_active == 1).Select(p => p.company_name).FirstOrDefault();
                            string MyFileName = "EmpOfficialDetail_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_tt") + ext; //Guid.NewGuid().ToString().Replace("-", "") +


                            var webRoot = _hostingEnvironment.WebRootPath;

                            string currentmonth = Convert.ToString(DateTime.Now.Month).Length.ToString() == "1" ? "0" + Convert.ToString(DateTime.Now.Month) : Convert.ToString(DateTime.Now.Month);

                            var currentyearmonth = Convert.ToString(DateTime.Now.Year) + currentmonth;


                            if (!Directory.Exists(webRoot + "/EmployeeDetail/" + company_name + "/SalaryInput/" + currentyearmonth + "/"))
                            {
                                Directory.CreateDirectory(webRoot + "/EmployeeDetail/" + company_name + "/SalaryInput/" + currentyearmonth + "/");

                            }

                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/EmployeeDetail/" + company_name + "/SalaryInput/" + currentyearmonth + "/");

                            //save file
                            using (var fileStream = new FileStream(Path.Combine(path, MyFileName), FileMode.Create))
                            {
                                files[i].CopyTo(fileStream);

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
                                    SalaryInput objsalaryinput = new SalaryInput();
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
                                                                objsalaryinput.emp_code = item.Text.Text;
                                                            }
                                                            else if (!string.IsNullOrEmpty(item.Text.Text) && currentcolumnno == "C")
                                                            {
                                                                objsalaryinput.property_details = item.Text.Text;
                                                            }
                                                            else if (!string.IsNullOrEmpty(item.Text.Text) && currentcolumnno == "D")
                                                            {
                                                                objsalaryinput.component_value = Convert.ToDecimal(item.Text.Text);
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
                                                    objsalaryinput.emp_code = thecurrentcell.InnerText;
                                                }
                                                else if (!string.IsNullOrEmpty(thecurrentcell.InnerText) && currentcolumnno == "C")
                                                {
                                                    objsalaryinput.property_details = thecurrentcell.InnerText;
                                                }
                                                else if (!string.IsNullOrEmpty(thecurrentcell.InnerText) && currentcolumnno == "D")
                                                {
                                                    objsalaryinput.component_value = Convert.ToDecimal(thecurrentcell.InnerText);
                                                }

#endregion ** END value in list**

                                            }

                                        }

                                    }
                                    excelResult.AppendLine();
                                    objsalaryinput.created_by = objsalary.created_by;
                                    objsalaryinput.company_id = objsalary.company_id;
                                    objsalaryinput.monthyear = objsalary.monthyear;

                                    salaryinputlst.Add(objsalaryinput);
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


                if (!_clsCurrentUser.CompanyId.Any(p => salaryinputlst.Any(q => q.company_id == p)))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unauthorize Company access...";
                    return Ok(objresponse);
                }



                var data = excelResult.ToString();
                var data1 = salaryinputlst;

                //start check uplicate,missing details
                var dataaa = CheckEmpSalaryInputfromexcel(salaryinputlst);

                var duplicate_dtl = dataaa.duplicatesalaryinputlst;
                var missing_dtll = dataaa.missingsalaryinputlst;
                var adddblistt = dataaa.adddbSalaryInputlst;
                var missingDtlMessage = dataaa.DtlMessage;

                if (duplicate_dtl.Count > 0 || missing_dtll.Count > 0)
                {
                    return Ok(dataaa);
                }
                else
                {
                    if (!_clsCurrentUser.DownlineEmpId.Any(p => adddblistt.Any(q => q.emp_id == p)))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Unauthorize Access....";
                        return Ok(objresponse);
                    }

                    var isfreezed = _context.tbl_payroll_process_status.FirstOrDefault(b => b.is_deleted == 0 && b.payroll_month_year == Convert.ToInt32(objsalary.monthyear) && adddblistt.Any(p => p.emp_id == b.emp_id) && (b.is_freezed == 1 || b.is_lock == 1));
                    // return Ok(salaryvalues);
                    if (isfreezed != null)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Employee Salary Input of Selected Month year already freezed or locked !!";
                        return Ok(objresponse);
                    }

                    //start save details
                    int result = await SaveEmpSalryInputFromExcel(adddblistt);
                    if (result == 0)
                    {
                        objresponse.StatusCode = 0;
                        objresponse.Message = "Salary Input details successfully saved..";
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
        }


        private SalaryInputList CheckEmpSalaryInputfromexcel(List<SalaryInput> objlst)
        {
            StringBuilder msg = new StringBuilder();

            List<SalaryInput> missingsalaryinputlst = new List<SalaryInput>();
            List<SalaryInput> adddbSalaryInputlst = new List<SalaryInput>();
            List<SalaryInput> duplicatesalaryinputlst = new List<SalaryInput>();
            Response_Msg objresponse = new Response_Msg();

            try
            {

                var get_dataentry_component = _context.tbl_component_master.Where(x => x.is_active == 1 && x.is_data_entry_comp == 1).Select(p => p.component_id).ToList();

                msg.Append("");

                for (int i = 0; i < objlst.Count; i++)
                {
                    int company_id = 0;
                    int emp_id = 0;

                    bool _vaid = false;
                    // bool company_not_map = false;
                    //bool emp_code = false;
                    //bool component_name = false;
                    //bool component_value = false;
                    //bool already_proccess = false;
                    //bool is_data_entry_comp = false;





                    if (!string.IsNullOrEmpty(objlst[i].emp_code))
                    {
                        var exist_code = _context.tbl_employee_master.OrderByDescending(x => x.employee_id).Where(y => y.is_active == 1 && y.emp_code == objlst[i].emp_code).FirstOrDefault();
                        if (exist_code == null)
                        {
                            // emp_code = true;
                            _vaid = true;
                            msg.Append("" + objlst[i].emp_code + ",Employee Code Not exist</br>");
                        }
                        else
                        {
                            emp_id = exist_code.employee_id;
                        }

                    }
                    else
                    {
                        // emp_code = true;
                        _vaid = true;
                        msg.Append("" + objlst[i].emp_code + ",Employee Code missing</br>");
                    }


                    if (emp_id != 0)
                    {
                        company_id = _context.tbl_employee_company_map.OrderByDescending(x => x.sno).FirstOrDefault(y => y.is_deleted == 0 && y.employee_id == emp_id).company_id ?? 0;
                        if (company_id == 0)
                        {
                            // company_not_map = true;
                            _vaid = true;
                            msg.Append("" + objlst[i].emp_code + ",Invalid Employee Code</br>");
                        }
                    }


                    if (!string.IsNullOrEmpty(objlst[i].property_details))
                    {
                        var property_detail_exist = _context.tbl_component_master.Where(y => y.is_active == 1 && y.property_details.Trim().ToLower() == objlst[i].property_details.Trim().ToLower()).FirstOrDefault();
                        if (property_detail_exist == null)
                        {
                            //  component_name = true;
                            _vaid = true;
                            msg.Append("" + objlst[i].emp_code + " , " + objlst[i].property_details + " Invalid Salary Component</br>");
                        }
                        else
                        {
                            objlst[i].component_id = property_detail_exist.component_id;
                        }
                    }
                    else
                    {
                        // component_name = true;
                        _vaid = true;
                        msg.Append("" + objlst[i].emp_code + " Salary Component Cannot be Blank</br>");
                    }

                    if (objlst[i].component_id != 0)
                    {
                        var is_dec = get_dataentry_component.Contains(objlst[i].component_id);
                        if (is_dec == false)
                        {
                            // is_data_entry_comp = true;
                            _vaid = true;
                            msg.Append("" + objlst[i].emp_code + ", " + objlst[i].property_details + " is not a Data Entry Component</br>");
                        }
                    }





                    //if (objlst[i].component_value == 0)
                    //{
                    //    component_value = true;
                    //    msg.Append(""+objlst[i].emp_code+",Value cannot be 0</br>");
                    //}

                    //if (emp_id != 0 && objlst[i].component_id != 0)
                    //{
                    //    var check_salary_process = _context.tbl_salary_input.Where(x => x.is_active == 1 && x.emp_id == emp_id && x.monthyear == objlst[i].monthyear && x.component_id == objlst[i].component_id).FirstOrDefault();
                    //    if (check_salary_process == null)
                    //    {
                    //        already_proccess = true;
                    //        msg.Append(""+objlst[i].emp_code+",Salary Not process</br>");
                    //    }
                    //}


                    //if (emp_code == true || company_not_map == true || component_name == true || component_value == true || is_data_entry_comp == true/* || already_proccess == true*/)
                    if (_vaid)
                    {
                        // find missing detail
                        SalaryInput objmissinginput = new SalaryInput();
                        objmissinginput.emp_code = objlst[i].emp_code;
                        objmissinginput.property_details = objlst[i].property_details;
                        objmissinginput.component_value = objlst[i].component_value;


                        missingsalaryinputlst.Add(objmissinginput);
                        //msg.Append("Details are missing in Excel </br>");
                    }
                    else
                    {
                        //find duplicate detail
                        bool _checkduplicate = adddbSalaryInputlst.Any(x => x.emp_code == objlst[i].emp_code && x.emp_id == emp_id && x.component_id == objlst[i].component_id);


                        if (_checkduplicate) // Add in this list if any one field detail is found duplicate in list
                        {
                            SalaryInput objduplist = new SalaryInput();

                            objduplist.emp_id = emp_id;
                            objduplist.emp_code = objlst[i].emp_code;
                            objduplist.property_details = objlst[i].property_details;
                            objduplist.component_value = objlst[i].component_value;

                            duplicatesalaryinputlst.Add(objduplist);
                            //msg.Append("Duplicate Entry in Excel</br>");
                        }
                        else
                        {
                            SalaryInput objdblist = new SalaryInput();
                            objdblist.monthyear = objlst[i].monthyear;
                            objdblist.emp_id = emp_id;
                            objdblist.component_id = objlst[i].component_id;
                            objdblist.component_value = objlst[i].component_value;
                            objdblist.created_by = objlst[i].created_by;
                            objdblist.creadted_dt = DateTime.Now;
                            objdblist.company_id = company_id;

                            adddbSalaryInputlst.Add(objdblist);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                msg.Append(ex.Message);
            }
            return new SalaryInputList { missingsalaryinputlst = missingsalaryinputlst, duplicatesalaryinputlst = duplicatesalaryinputlst, adddbSalaryInputlst = adddbSalaryInputlst, DtlMessage = msg.ToString() };
        }

        private async Task<int> SaveEmpSalryInputFromExcel(List<SalaryInput> objlstt)
        {
            try
            {




                using (var trans = _context.Database.BeginTransaction())
                {

                    var EmpID = objlstt.Select(p => p.emp_id).ToArray();
                    string MOnthyear = objlstt.FirstOrDefault().monthyear.ToString();
                    var salaryList = _context.tbl_salary_input_change.Where(p => EmpID.Contains(p.emp_id ?? 0) && p.monthyear == MOnthyear).ToList();

                    var linqq = (from t1 in salaryList
                                 join t2 in objlstt on t1.emp_id.Value equals t2.emp_id
                                 where t1.component_id == t2.component_id
                                 select t1).ToList();
                    _context.tbl_salary_input_change.RemoveRange(linqq);
                    var t1task = _context.SaveChangesAsync();
                    List<tbl_salary_input_change> objchangeinput = objlstt.Select(p => new tbl_salary_input_change
                    {
                        monthyear = MOnthyear,
                        emp_id = p.emp_id,
                        component_id = p.component_id,
                        values = Convert.ToString(p.component_value),
                        created_by = p.created_by,
                        company_id = p.company_id,
                        created_dt = DateTime.Now,
                        is_active = 1,
                        modified_by = 0,
                        modified_dt = DateTime.Now
                    }).ToList();
                    await t1task;
                    _context.AddRange(objchangeinput);
                    _context.SaveChanges();

                    trans.Commit();
                }
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }
#endregion ** END BY SUPRIYA ON 19-12-2019,UPLOAD SALARYINPUT


        [Route("Get_PendingAssetRequestDetailByAssetReqId/{asset_req_id}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.AssetApproval))]
        public IActionResult Get_PendingAssetRequestDetailByAssetReqId([FromRoute] int asset_req_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            try
            {

                var asset_approval_history = _context.tbl_assets_approval.Join(_context.tbl_loan_approval_setting, a => a.approval_order, b => b.order, (a, b) => new
                {
                    a_isdeleted = a.is_deleted,
                    b_active = b.is_active,
                    req_emp_id = a.emp_id,
                    req_id = a.asset_req_id,
                    approver_type = b.approver_type,
                    is_approve = a.is_approve == 0 ? "Pending" : a.is_approve == 1 ? "Approve" : a.is_approve == 2 ? "Reject" : "",
                    order_name = a.approval_order == 1 ? "Approver 1" : a.approval_order == 2 ? "Approver 2" : a.approval_order == 3 ? "Approver 3" : a.approval_order == 4 ? "Approver 4" : a.approval_order == 5 ? "Approver 5" : "",
                    approver_emp_name_code = string.Format("{0} {1} {2} ({3})",
                       _context.tbl_emp_officaial_sec.FirstOrDefault(g => g.is_deleted == 0 && g.employee_id == b.emp_id).employee_first_name,
                       _context.tbl_emp_officaial_sec.FirstOrDefault(g => g.is_deleted == 0 && g.employee_id == b.emp_id).employee_middle_name,
                       _context.tbl_emp_officaial_sec.FirstOrDefault(g => g.is_deleted == 0 && g.employee_id == b.emp_id).employee_last_name,
                       _context.tbl_employee_master.FirstOrDefault(g => g.is_active == 1 && g.employee_id == b.emp_id).emp_code),
                    approver_role_id = b.approver_role_id,

                }).Where(x => x.approver_type == 2 && x.a_isdeleted == 0 && x.req_id == asset_req_id).ToList();

                return Ok(asset_approval_history);

            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [Route("Get_AssetApprovalReport/{login_emp_id}/{login_role_id}/{_from_date}/{_to_date}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.AssetApproval))]
        public IActionResult Get_AssetApprovalReport([FromRoute] int login_emp_id, int login_role_id, DateTime _from_date, DateTime _to_date)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (_clsCurrentUser.EmpId != login_emp_id)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }
            try
            {

                var data = _context.tbl_assets_approval.Join(_context.tbl_loan_approval_setting, a => a.approval_order, b => b.order, (a, b) => new
                {
                    approver_active = b.is_active,
                    b.approver_type,
                    a.is_deleted,
                    approver_emp_id = b.emp_id,
                    approver_role_id = b.approver_role_id,
                    a.asset_req_mastr.from_date,
                    a.asset_req_mastr.to_date,

                    a.asset_req_mastr.asset_name,
                    a.asset_req_mastr.asset_number,
                    asset_type = a.asset_req_mastr.asset_type == 0 ? "New" : a.asset_req_mastr.asset_type == 1 ? "Replace" : a.asset_req_mastr.asset_type == 2 ? "Submission" : "",
                    a.asset_req_mastr.is_permanent,
                    req_remarks = a.asset_req_mastr.description,
                    a.asset_req_mastr.asset_issue_date,
                    a.asset_req_mastr.submission_date,
                    a.asset_req_mastr.replace_dt,
                    final_status = a.asset_req_mastr.is_finalapprove == 0 ? "Pending" : a.asset_req_mastr.is_finalapprove == 1 ? "Approve" : a.asset_req_mastr.is_finalapprove == 2 ? "Reject" : "",
                    mystatus = a.is_approve == 0 ? "Pending" : a.is_approve == 1 ? "Approve" : a.is_approve == 2 ? "Reject" : "",
                    a.asset_req_id,
                    requester_name_code = string.Format("{0} {1} {2} ({3})", a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                    a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                    a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name,
                    a.employee_master.emp_code),
                    b.company_id,

                }).OrderByDescending(y => y.asset_req_id).Where(x => x.approver_type == 2 && x.is_deleted == 0 && (_clsCurrentUser.RoleId.Contains(x.approver_role_id ?? 0) || x.approver_emp_id == _clsCurrentUser.EmpId) &&
                  ((x.from_date.Date <= _from_date.Date && _from_date.Date <= x.to_date.Date) || ((x.from_date.Date <= (x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : _to_date.Date)) && ((x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : _to_date.Date) <= x.to_date.Date))
                      || (_from_date.Date <= x.from_date.Date && (x.to_date.Date <= (x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : _to_date.Date))))
                ).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }
        // //[Authorize(Policy = "8111")]
        //public IActionResult Get_AssetApprovalReport([FromRoute] int login_emp_id, int login_role_id, DateTime _from_date, DateTime _to_date)
        //{
        //    try
        //    {
        //        var data = _context.tbl_assets_approval.Join(_context.tbl_loan_approval_setting, a => a.approval_order, b => b.order, (a, b) => new
        //        {
        //            b.Sno,
        //            a.asset_approval_sno,
        //            a.asset_req_id,
        //            a.approval_order,
        //            b.order,
        //            approver_emp_id = b.emp_id,
        //            approver_role_id = b.approver_role_id,
        //            ais_active = a.asset_req_mastr.is_active,
        //            bis_active = b.is_active,
        //            a.asset_req_mastr.is_deleted,
        //            a.asset_req_mastr.assets_master.asset_name,
        //            a.created_dt,
        //            a.asset_req_mastr.description,
        //            a.is_approve,
        //            a.is_final_approver,
        //            //a.asset_req_mastr.is_finalapprove,
        //            a.emp_id,
        //            req_emp_name_code = string.Format("{0} {1} {2} ({3})",
        //                      a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0 && p.employee_first_name != "").employee_first_name,
        //                      a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0 && p.employee_middle_name != "").employee_middle_name,
        //                      a.employee_master.tbl_emp_officaial_sec.FirstOrDefault(p => p.is_deleted == 0 && p.employee_last_name != "").employee_last_name,
        //                      a.employee_master.emp_code),
        //            a.employee_master.tbl_employee_company_map.FirstOrDefault(g => g.is_deleted == 0).tbl_company_master.company_name,
        //            a.asset_req_mastr.asset_issue_date,
        //            a.asset_req_mastr.is_closed,
        //            b.approver_type,
        //            a.asset_req_mastr.from_date,
        //            a.asset_req_mastr.to_date,
        //            a.asset_req_mastr.asset_number,
        //            a.asset_req_mastr.asset_type,
        //            a.asset_req_mastr.submission_date,

        //        }).OrderByDescending(y=>y.asset_req_id).Where(x => x.approver_type == 2 && (x.approver_role_id == login_role_id || x.approver_emp_id == login_emp_id) && x.is_deleted == 0 && x.ais_active==1 && x.bis_active==1 &&
        //        (x.from_date.Date<=_from_date.Date && _from_date.Date<=x.to_date.Date)||(x.from_date.Date<=_to_date.Date && _to_date.Date<=x.to_date.Date) ||(_from_date.Date<=x.from_date.Date && x.to_date.Date<=_to_date.Date)).ToList();

        //        //Where(x => x.approver_type == 2 && (x.approver_role_id == login_role_id || x.approver_emp_id == login_emp_id) && x.is_deleted == 0 && x.ais_active == 1
        //        //&& x.bis_active == 1 && x.from_date.Date >= _from_date.Date && x.to_date.Date <= _to_date.Date).OrderBy(x => x.asset_approval_sno).ToList();








        //        //.Where(x => x.approver_type == 2 && x.is_deleted == 0 &&x.ais_active == 1 && x.bis_active==1 && (x.approver_role_id == login_role_id || x.approver_emp_id == login_emp_id) 
        //        //               /* && x.from_date.Date >= from_date.Date && x.from_date.Date <= to_date.Date*/
        //        //               && x.from_date.Date>=from_date.Date && x.to_date.Date<=to_date.Date).OrderBy(x => x.asset_approval_sno).ToList();


        //        List<AssetRequest> objrequestlist = new List<AssetRequest>();

        //        if (data.Count > 0)
        //        {
        //            foreach (var item in data)
        //            {
        //                bool containsItem = objrequestlist.Any(x => x.asset_req_id == item.asset_req_id);

        //                if (!containsItem)
        //                {
        //                    AssetRequest objrequest = new AssetRequest();

        //                    objrequest.asset_req_id = Convert.ToInt32(item.asset_req_id);
        //                    objrequest.approval_order = item.approval_order;
        //                    objrequest.approver_emp_id = item.approver_emp_id;
        //                    objrequest.approver_role_id = item.approver_role_id;
        //                    objrequest.asset_name = item.asset_name;
        //                    objrequest.created_dt = item.created_dt;
        //                    objrequest.asset_description = item.description;
        //                    objrequest.is_approve = item.is_approve;
        //                    objrequest.is_final_approver = Convert.ToByte(item.is_final_approver);
        //                    objrequest.emp_id = item.emp_id;
        //                    objrequest.emp_name_code = item.req_emp_name_code;
        //                    objrequest.company_name = item.company_name;
        //                    objrequest.asset_issue_date = item.asset_issue_date;
        //                    objrequest.is_closed = item.is_closed;
        //                    objrequest.approver_type = item.approver_type;
        //                    objrequest.asset_number = item.asset_number;
        //                    objrequest.asset_type = item.asset_type;
        //                    objrequest.from_date = item.from_date;
        //                    objrequest.to_date = item.to_date;
        //                    objrequest.submission_date = item.submission_date;

        //                    objrequestlist.Add(objrequest);
        //                }
        //                else
        //                {

        //                    if (objrequestlist.Where(a => a.asset_req_id == item.asset_req_id && a.is_approve == 1).Count() > 0)
        //                    {
        //                        objrequestlist.Remove(objrequestlist.Single(s => s.asset_req_id == item.asset_req_id));

        //                        AssetRequest objrequest = new AssetRequest();

        //                        objrequest.asset_req_id = Convert.ToInt32(item.asset_req_id);
        //                        objrequest.approval_order = item.approval_order;
        //                        objrequest.approver_emp_id = item.approver_emp_id;
        //                        objrequest.approver_role_id = item.approver_role_id;
        //                        objrequest.asset_name = item.asset_name;
        //                        objrequest.created_dt = item.created_dt;
        //                        objrequest.asset_description = item.description;
        //                        objrequest.is_approve = item.is_approve;
        //                        objrequest.is_final_approver = Convert.ToByte(item.is_final_approver);
        //                        objrequest.emp_id = item.emp_id;
        //                        objrequest.emp_name_code = item.req_emp_name_code;
        //                        objrequest.company_name = item.company_name;
        //                        objrequest.asset_issue_date = item.asset_issue_date;
        //                        objrequest.is_closed = item.is_closed;
        //                        objrequest.approver_type = item.approver_type;
        //                        objrequest.asset_number = item.asset_number;
        //                        objrequest.asset_type = item.asset_type;
        //                        objrequest.from_date = item.from_date;
        //                        objrequest.to_date = item.to_date;
        //                        objrequest.submission_date = item.submission_date;

        //                        objrequestlist.Add(objrequest);
        //                    }
        //                }

        //            }

        //        }

        //        return Ok(objrequestlist);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}

        [Route("Get_AssetRequestReport")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.EmpAssetReports))] // add policy for test
        public IActionResult Get_AssetRequestReport([FromBody] AssetRequest objreport)
        {
            Response_Msg objresponse = new Response_Msg();
            if (!_clsCurrentUser.CompanyId.Contains(objreport.company_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }


            List<int> _empid = new List<int>();

            if (objreport.assetreqlist != null)
            {
                _empid = objreport.assetreqlist.Select(p => p.emp_id ?? 0).ToList();
            }

            if (_empid.Count == 0)
            {
                _empid.Add(objreport.emp_id ?? 0);
            }
            _empid.RemoveAll(p => p == -1 || p == 0);

            foreach (var id in _empid)
            {
                if (!_clsCurrentUser.DownlineEmpId.Contains(id))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unauthorze Access";
                    return Ok(objresponse);
                }
            }



            try
            {
                if (objreport.asset_req_id > 0)
                {

                    var asset_req_detail = _context.tbl_assets_request_master.Where(x => x.asset_req_id == objreport.asset_req_id && x.is_deleted == 0 && x.req_employee_id == objreport.emp_id).Select(p => new
                    {

                        p.asset_req_id,
                        p.asset_name,
                        asset_desc = p.assets_master.description,
                        p.submission_date,
                        p.asset_issue_date,
                        p.asset_number,
                        asset_type = p.asset_type == 0 ? "New" : p.asset_type == 1 ? "Replace" : p.asset_type == 2 ? "Submit" : "",
                        p.created_dt,
                        p.from_date,
                        p.to_date,
                        p.is_permanent,
                        final_status = p.is_finalapprove == 0 ? "Pending" : p.is_finalapprove == 1 ? "Approve" : p.is_finalapprove == 2 ? "Reject" : p.is_finalapprove == 3 ? "In Process" : "",
                        p.replace_dt
                    }).FirstOrDefault();


                    var asset_approval_history = _context.tbl_assets_approval.Join(_context.tbl_loan_approval_setting, a => a.approval_order, b => b.order, (a, b) => new
                    {
                        a_isdeleted = a.is_deleted,
                        b_active = b.is_active,
                        req_emp_id = a.emp_id,
                        req_id = a.asset_req_id,
                        approver_type = b.approver_type,
                        is_approve = a.is_approve == 0 ? "Pending" : a.is_approve == 1 ? "Approve" : a.is_approve == 2 ? "Reject" : "",
                        order_name = a.approval_order == 1 ? "Approver 1" : a.approval_order == 2 ? "Approver 2" : a.approval_order == 3 ? "Approver 3" : a.approval_order == 4 ? "Approver 4" : a.approval_order == 5 ? "Approver 5" : "",
                        approver_emp_name_code = string.Format("{0} {1} {2} ({3})",
                        _context.tbl_emp_officaial_sec.FirstOrDefault(g => g.is_deleted == 0 && g.employee_id == b.emp_id).employee_first_name,
                        _context.tbl_emp_officaial_sec.FirstOrDefault(g => g.is_deleted == 0 && g.employee_id == b.emp_id).employee_middle_name,
                        _context.tbl_emp_officaial_sec.FirstOrDefault(g => g.is_deleted == 0 && g.employee_id == b.emp_id).employee_last_name,
                        _context.tbl_employee_master.FirstOrDefault(g => g.is_active == 1 && g.employee_id == b.emp_id).emp_code),
                        approver_role_id = b.approver_role_id,

                    }).Where(x => x.approver_type == 2 && x.a_isdeleted == 0 && x.req_id == objreport.asset_req_id).ToList();

                    return Ok(new { asset_req_detail = asset_req_detail, asset_approval_history = asset_approval_history });


                }
                else
                {

                    var asset_dtl = _context.tbl_assets_request_master.OrderByDescending(y => y.asset_req_id).Where(x => x.is_deleted == 0 && x.company_id == objreport.company_id && _empid.Contains(x.req_employee_id)
                                       && ((x.from_date.Date <= objreport.from_date.Date && objreport.from_date.Date <= x.to_date.Date) || ((x.from_date.Date <= (x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : objreport.to_date.Date)) && ((x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : objreport.to_date.Date) <= x.to_date.Date))
                                       || (objreport.from_date.Date <= x.from_date.Date && (x.to_date.Date <= (x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : objreport.to_date.Date))))).Select(p => new
                                       {

                                           p.asset_req_id,
                                           p.company_id,
                                           p.req_employee_id,
                                           //emp_name_code = string.Format("{0} {1} {2} ({3})", p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                                           // p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_middle_name,
                                           // p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_last_name,
                                           // p.emp_master.emp_code),
                                           p.asset_name,
                                           p.description,
                                           p.is_finalapprove,
                                           _finalapprove = p.is_finalapprove == 0 ? "Pending" : p.is_finalapprove == 1 ? "Approve" : p.is_finalapprove == 2 ? "Rejected" : p.is_finalapprove == 3 ? "In Process" : "",
                                           asset_issue_date = (p.asset_issue_date.Date != Convert.ToDateTime("01-01-2000").Date && p.asset_issue_date.Date != Convert.ToDateTime("2500-01-01")) ? Convert.ToDateTime(p.asset_issue_date).ToString("dd-MM-yyyy") : "-",
                                           from_date = Convert.ToDateTime(p.from_date).ToString("dd-MM-yyyy"),
                                           to_date = p.to_date.Date != Convert.ToDateTime("2500-01-01").Date ? Convert.ToDateTime(p.to_date).ToString("dd-MM-yyyy") : "-",
                                           submission_date = (p.submission_date.Date != Convert.ToDateTime("01-01-2000") && p.submission_date.Date != Convert.ToDateTime("2500-01-01")) ? Convert.ToDateTime(p.submission_date).ToString("dd-MM-yyyy") : "-",
                                           replace_dt = (p.replace_dt.Date != Convert.ToDateTime("01-01-2000") && p.replace_dt.Date != Convert.ToDateTime("2500-01-01")) ? Convert.ToDateTime(p.replace_dt).ToString("dd-MM-yyyy") : "-",
                                           p.asset_number,
                                           //p.asset_type,
                                           _asset_type = p.asset_type == 0 ? "New Asset" : p.asset_type == 1 ? "Replace" : p.asset_type == 2 ? "Submit" : "",
                                           created_dt = Convert.ToDateTime(p.created_dt).ToString("dd-MM-yyyy"),
                                           p.modified_dt,
                                           p.is_permanent,
                                           req_emp_name = string.Format("{0} {1} {2} ({3})", p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_first_name,
                                                          p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_middle_name,
                                                          p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0).employee_last_name,
                                                          p.emp_master.emp_code),
                                       }).Distinct().ToList();

                    return Ok(asset_dtl);

                    //if (ob.is_manager==1)
                    //{
                    //    var manager_emp_list = _context.tbl_emp_manager.Where(a => (a.m_one_id == _emp_id || a.m_two_id == _emp_id || a.m_three_id == _emp_id) && a.is_deleted == 0).Select(p => p.employee_id).Distinct().ToList();

                    //    if (for_all_emp == 1)
                    //    {
                    //        var asset_dtl = _context.tbl_assets_request_master.OrderByDescending(y => y.asset_req_id).Where(x => x.req_employee_id == login_empid && x.is_deleted == 0 && x.company_id == company_id).Select(p => new {
                    //           // && ((x.from_date.Date <= from_date.Value.Date && from_date.Value.Date <= x.to_date.Date) || ((x.from_date.Date <= (x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : todate.Value.Date)) && ((x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : todate.Value.Date) <= x.to_date.Date))
                    //           //|| (from_date.Value.Date <= x.from_date.Date && (x.to_date.Date <= (x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : todate.Value.Date))))).Select(p => new {

                    //           p.asset_req_id,
                    //           p.company_id,
                    //           p.req_employee_id,
                    //           emp_name_code = string.Format("{0} {1} {2} ({3})", p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                    //               p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_middle_name,
                    //               p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_last_name,
                    //               p.emp_master.emp_code),
                    //           p.asset_name,
                    //           p.description,
                    //           p.is_finalapprove,
                    //           _finalapprove = p.is_finalapprove == 0 ? "Pending" : p.is_finalapprove == 1 ? "Approve" : p.is_finalapprove == 2 ? "Rejected" : "",
                    //           asset_issue_date = p.asset_issue_date.Date != Convert.ToDateTime("01-01-2000").Date ? Convert.ToDateTime(p.asset_issue_date).ToString("dd-MM-yyyy") : "-",
                    //           from_date = Convert.ToDateTime(p.from_date).ToString("dd-MM-yyyy"),
                    //           to_date = p.to_date.Date != Convert.ToDateTime("2500-01-01").Date ? Convert.ToDateTime(p.to_date).ToString("dd-MM-yyyy") : "-",
                    //           submission_date = p.submission_date.Date != Convert.ToDateTime("01-01-2000") ? Convert.ToDateTime(p.submission_date).ToString("dd-MM-yyyy") : "-",
                    //           replace_dt = p.replace_dt.Date != Convert.ToDateTime("01-01-2000") ? Convert.ToDateTime(p.replace_dt).ToString("dd-MM-yyyy") : "-",
                    //           p.asset_number,
                    //           p.asset_type,
                    //           _asset_type = p.asset_type == 0 ? "New Asset" : p.asset_type == 1 ? "Replace" : p.asset_type == 2 ? "Submit" : "",
                    //           created_dt = Convert.ToDateTime(p.created_dt).ToString("dd-MM-yyyy"),
                    //           p.modified_dt,
                    //           p.is_permanent,
                    //           req_emp_name = string.Format("{0} {1} {2} ({3})", p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                    //           p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                    //           p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name,
                    //           p.emp_master.emp_code),
                    //       }).Distinct().ToList();
                    //        return Ok(asset_dtl);
                    //    }
                    //    else
                    //    {
                    //        var asset_dtl = _context.tbl_assets_request_master.OrderByDescending(y => y.asset_req_id).Where(x => manager_emp_list.Contains(x.req_employee_id) && x.is_deleted == 0 && x.company_id == company_id).Select(p => new {
                    //           // && ((x.from_date.Date <= from_date.Value.Date && from_date.Value.Date <= x.to_date.Date) || ((x.from_date.Date <= (x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : todate.Value.Date)) && ((x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : todate.Value.Date) <= x.to_date.Date))
                    //           //|| (from_date.Value.Date <= x.from_date.Date && (x.to_date.Date <= (x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : todate.Value.Date))))).Select(p => new {

                    //           p.asset_req_id,
                    //           p.company_id,
                    //           p.req_employee_id,
                    //           emp_name_code = string.Format("{0} {1} {2} ({3})", p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                    //               p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_middle_name,
                    //               p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_last_name,
                    //               p.emp_master.emp_code),
                    //           p.asset_name,
                    //           p.description,
                    //           p.is_finalapprove,
                    //           _finalapprove = p.is_finalapprove == 0 ? "Pending" : p.is_finalapprove == 1 ? "Approve" : p.is_finalapprove == 2 ? "Rejected" : "",
                    //           asset_issue_date=p.asset_issue_date.Date != Convert.ToDateTime("01-01-2000").Date ? Convert.ToDateTime(p.asset_issue_date).ToString("dd-MM-yyyy") : "-",
                    //           from_date=Convert.ToDateTime(p.from_date).ToString("dd-MM-yyyy"),
                    //           to_date=p.to_date.Date!=Convert.ToDateTime("2500-01-01").Date?Convert.ToDateTime(p.to_date).ToString("dd-MM-yyyy"):"-",
                    //           submission_date=p.submission_date.Date!=Convert.ToDateTime("01-01-2000")?Convert.ToDateTime(p.submission_date).ToString("dd-MM-yyyy"):"-",
                    //           replace_dt= p.replace_dt.Date!= Convert.ToDateTime("01-01-2000")?Convert.ToDateTime(p.replace_dt).ToString("dd-MM-yyyy"):"-",
                    //           p.asset_number,
                    //           p.asset_type,
                    //           _asset_type = p.asset_type == 0 ? "New Asset" : p.asset_type == 1 ? "Replace" : p.asset_type == 2 ? "Submit" : "",
                    //           created_dt=Convert.ToDateTime(p.created_dt).ToString("dd-MM-yyyy"),
                    //           p.modified_dt,
                    //           p.is_permanent,
                    //           req_emp_name=string.Format("{0} {1} {2} ({3})",p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y=>y.is_deleted==0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                    //           p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                    //           p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name,
                    //           p.emp_master.emp_code),
                    //       }).Distinct().ToList();
                    //        return Ok(asset_dtl);
                    //    }

                    //}
                    //else if (ob.is_Admin==1)
                    //{
                    //    if (for_all_emp == 1)
                    //    {
                    //        var asset_dtl = _context.tbl_assets_request_master.OrderByDescending(y => y.asset_req_id).Where(x => x.req_employee_id == login_empid && x.is_deleted == 0 && x.company_id == company_id
                    //         && ((x.from_date.Date <= from_date.Value.Date && from_date.Value.Date <= x.to_date.Date) || ((x.from_date.Date <= (x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : todate.Value.Date)) && ((x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : todate.Value.Date) <= x.to_date.Date))
                    //        || (from_date.Value.Date <= x.from_date.Date && (x.to_date.Date <= (x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : todate.Value.Date))))).Select(p => new {

                    //            p.asset_req_id,
                    //            p.company_id,
                    //            p.req_employee_id,
                    //            emp_name_code = string.Format("{0} {1} {2} ({3})", p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                    //                p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_middle_name,
                    //                p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_last_name,
                    //                p.emp_master.emp_code),
                    //            p.asset_name,
                    //            p.description,
                    //            p.is_finalapprove,
                    //            _finalapprove = p.is_finalapprove == 0 ? "Pending" : p.is_finalapprove == 1 ? "Approve" : p.is_finalapprove == 2 ? "Rejected" : "",
                    //            asset_issue_date = p.asset_issue_date.Date != Convert.ToDateTime("01-01-2000").Date ? Convert.ToDateTime(p.asset_issue_date).ToString("dd-MM-yyyy") : "-",
                    //            from_date = Convert.ToDateTime(p.from_date).ToString("dd-MM-yyyy"),
                    //            to_date = p.to_date.Date != Convert.ToDateTime("2500-01-01").Date ? Convert.ToDateTime(p.to_date).ToString("dd-MM-yyyy") : "-",
                    //            submission_date = p.submission_date.Date != Convert.ToDateTime("01-01-2000") ? Convert.ToDateTime(p.submission_date).ToString("dd-MM-yyyy") : "-",
                    //            replace_dt = p.replace_dt.Date != Convert.ToDateTime("01-01-2000") ? Convert.ToDateTime(p.replace_dt).ToString("dd-MM-yyyy") : "-",
                    //            p.asset_number,
                    //            p.asset_type,
                    //            _asset_type = p.asset_type == 0 ? "New Asset" : p.asset_type == 1 ? "Replace" : p.asset_type == 2 ? "Submit" : "",
                    //            created_dt = Convert.ToDateTime(p.created_dt).ToString("dd-MM-yyyy"),
                    //            p.modified_dt,
                    //            p.is_permanent,
                    //            req_emp_name = string.Format("{0} {1} {2} ({3})", p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                    //           p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                    //           p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name,
                    //           p.emp_master.emp_code),
                    //        }).Distinct().ToList();

                    //        return Ok(asset_dtl);

                    //    }
                    //    else
                    //    {
                    //        int company_idd = 0;

                    //        var _comp_exist = _context.tbl_user_master.Where(x => x.default_company_id == company_id && x.is_active == 1).FirstOrDefault();
                    //        if (_comp_exist != null)
                    //        {
                    //            company_idd = _comp_exist.default_company_id;
                    //        }

                    //        if (company_idd == 0)
                    //        {
                    //            objresponse.StatusCode = 1;
                    //            objresponse.Message = "Invalid Company Details";
                    //            return Ok(objresponse);
                    //        }
                    //        else
                    //        {

                    //            var asset_dtl = _context.tbl_assets_request_master.OrderByDescending(y => y.asset_req_id).Where(x => x.is_deleted == 0 && x.company_id == company_id
                    //                    && ((x.from_date.Date <= from_date.Value.Date && from_date.Value.Date <= x.to_date.Date) || ((x.from_date.Date <= (x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : todate.Value.Date)) && ((x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : todate.Value.Date) <= x.to_date.Date))
                    //                    || (from_date.Value.Date <= x.from_date.Date && (x.to_date.Date <= (x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : todate.Value.Date))))).Select(p => new {

                    //                    p.asset_req_id,
                    //                    p.company_id,
                    //                    p.req_employee_id,
                    //                    emp_name_code = string.Format("{0} {1} {2} ({3})", p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                    //                         p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_middle_name,
                    //                         p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_last_name,
                    //                         p.emp_master.emp_code),
                    //                    p.asset_name,
                    //                    p.description,
                    //                    p.is_finalapprove,
                    //                    _finalapprove = p.is_finalapprove == 0 ? "Pending" : p.is_finalapprove == 1 ? "Approve" : p.is_finalapprove == 2 ? "Rejected" : "",
                    //                    asset_issue_date = p.asset_issue_date.Date != Convert.ToDateTime("01-01-2000").Date ? Convert.ToDateTime(p.asset_issue_date).ToString("dd-MM-yyyy") : "-",
                    //                    from_date = Convert.ToDateTime(p.from_date).ToString("dd-MM-yyyy"),
                    //                    to_date = p.to_date.Date != Convert.ToDateTime("2500-01-01").Date ? Convert.ToDateTime(p.to_date).ToString("dd-MM-yyyy") : "-",
                    //                    submission_date = p.submission_date.Date != Convert.ToDateTime("01-01-2000") ? Convert.ToDateTime(p.submission_date).ToString("dd-MM-yyyy") : "-",
                    //                    replace_dt = p.replace_dt.Date != Convert.ToDateTime("01-01-2000") ? Convert.ToDateTime(p.replace_dt).ToString("dd-MM-yyyy") : "-",
                    //                    p.asset_number,
                    //                    p.asset_type,
                    //                    _asset_type = p.asset_type == 0 ? "New Asset" : p.asset_type == 1 ? "Replace" : p.asset_type == 2 ? "Submit" : "",
                    //                    created_dt = Convert.ToDateTime(p.created_dt).ToString("dd-MM-yyyy"),
                    //                    p.modified_dt,
                    //                        p.is_permanent,
                    //                    req_emp_name = string.Format("{0} {1} {2} ({3})", p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                    //           p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                    //           p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name,
                    //           p.emp_master.emp_code),
                    //                }).Distinct().ToList();

                    //            return Ok(asset_dtl);
                    //        }
                    //    }



                    //}
                    //else
                    //{
                    //    var asset_dtl = _context.tbl_assets_request_master.OrderByDescending(y => y.asset_req_id).Where(x => x.req_employee_id == login_empid && x.is_deleted == 0 && x.company_id == company_id).Select(p => new {
                    //      //&& ((x.from_date.Date <= from_date.Value.Date && from_date.Value.Date <= x.to_date.Date) || ((x.from_date.Date <= (x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : todate.Value.Date)) && ((x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : todate.Value.Date) <= x.to_date.Date))
                    //      //|| (from_date.Value.Date <= x.from_date.Date && (x.to_date.Date <= (x.is_permanent == 1 ? Convert.ToDateTime("2500-01-01").Date : todate.Value.Date))))).Select(p => new {

                    //      p.asset_req_id,
                    //      p.company_id,
                    //      p.req_employee_id,
                    //      emp_name_code = string.Format("{0} {1} {2} ({3})", p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_first_name,
                    //           p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_middle_name,
                    //           p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(x => x.is_deleted == 0 && !string.IsNullOrEmpty(x.employee_first_name)).employee_last_name,
                    //           p.emp_master.emp_code),
                    //      p.asset_name,
                    //      p.description,
                    //      p.is_finalapprove,
                    //      _finalapprove = p.is_finalapprove == 0 ? "Pending" : p.is_finalapprove == 1 ? "Approve" : p.is_finalapprove == 2 ? "Rejected" : "",
                    //      asset_issue_date = p.asset_issue_date.Date != Convert.ToDateTime("01-01-2000").Date ? Convert.ToDateTime(p.asset_issue_date).ToString("dd-MM-yyyy") : "-",
                    //      from_date = Convert.ToDateTime(p.from_date).ToString("dd-MM-yyyy"),
                    //      to_date = p.to_date.Date != Convert.ToDateTime("2500-01-01").Date ? Convert.ToDateTime(p.to_date).ToString("dd-MM-yyyy") : "-",
                    //      submission_date = p.submission_date.Date != Convert.ToDateTime("01-01-2000") ? Convert.ToDateTime(p.submission_date).ToString("dd-MM-yyyy") : "-",
                    //      replace_dt = p.replace_dt.Date != Convert.ToDateTime("01-01-2000") ? Convert.ToDateTime(p.replace_dt).ToString("dd-MM-yyyy") : "-",
                    //      p.asset_number,
                    //      p.asset_type,
                    //      _asset_type = p.asset_type == 0 ? "New Asset" : p.asset_type == 1 ? "Replace" : p.asset_type == 2 ? "Submit" : "",
                    //      created_dt = Convert.ToDateTime(p.created_dt).ToString("dd-MM-yyyy"),
                    //      p.modified_dt,
                    //      p.is_permanent,
                    //      req_emp_name = string.Format("{0} {1} {2} ({3})", p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_first_name,
                    //           p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_middle_name,
                    //           p.emp_master.tbl_emp_officaial_sec.FirstOrDefault(y => y.is_deleted == 0 && !string.IsNullOrEmpty(y.employee_first_name)).employee_last_name,
                    //           p.emp_master.emp_code),
                    //  }).Distinct().ToList();
                    //    return Ok(asset_dtl);
                    //}


                    //var asset_dtl = _context.tbl_assets_request_master.Where(x => x.req_employee_id == login_empid && x.is_deleted == 0 && x.company_id == company_id && x.from_date.Date>=from_date.Value.Date && x.to_date.Date<=todate.Value.Date).Select(p => new {





                }

            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 1;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

#region **FNF SALARY ,START BY SUPRIYA ON 16-07-2020 **
        [HttpGet("Get_EmpFNFSalary/{empid}/{reqid}")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpFNFProcess))]
        public IActionResult Get_EmpFNFSalary([FromRoute] int empid, int reqid)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(empid))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access";
                return Ok(objresponse);
            }

            try
            {
                List<int> _empid = new List<int>();
                _empid.Add(empid);

                List<int> _reqid = new List<int>();
                _reqid.Add(reqid);

                clsFNFProcess _clsFNFProcess = new clsFNFProcess(_context, _config, _AC, _clsCurrentUser);
                _clsFNFProcess.EmpIDs = _empid;
                _clsFNFProcess.ReqID = _reqid;
                List<mdlFNFProcess> _mdlfnfprocess = new List<mdlFNFProcess>();
                var data = _clsFNFProcess.Get_FNFSalary(_mdlfnfprocess);

                return Ok(data);
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }
#endregion ** FNF SALARY, END BY SUPRIYA ON 16-07-2020 **

        [Route("Upload_SalaryRevision")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.PayrollInput))]
        public async Task<IActionResult> Upload_SalaryRevision()
        {
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
                SalaryInput objsalary = new SalaryInput();
                objsalary = objcom.ToObjectFromJSON<SalaryInput>(a.ToString());


                //open excel using openxml sdk
                StringBuilder excelResult = new StringBuilder();
                List<SalaryInput> salaryinputlst = new List<SalaryInput>();
                string get_file_path = "";



                for (int i = 0; i < files.Count; i++)
                {
                    if (files[i].Length > 0 && files[i] != null)
                    {
                        var allowedExtensions = new[] { ".xlsx" };

                        var ext = Path.GetExtension(files[i].FileName); //getting the extension
                        if (allowedExtensions.Contains(ext.ToLower()))//check what type of extension  
                        {
                            string name = Path.GetFileNameWithoutExtension(files[i].FileName); //getting file name without extension  

                            string company_name = _context.tbl_company_master.OrderByDescending(x => x.company_id).Where(y => y.company_id == objsalary.company_id && y.is_active == 1).Select(p => p.company_name).FirstOrDefault();
                            string MyFileName = "EmpSalaryRevision" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_tt") + ext; //Guid.NewGuid().ToString().Replace("-", "") +


                            var webRoot = _hostingEnvironment.WebRootPath;

                            string currentmonth = Convert.ToString(DateTime.Now.Month).Length.ToString() == "1" ? "0" + Convert.ToString(DateTime.Now.Month) : Convert.ToString(DateTime.Now.Month);

                            var currentyearmonth = Convert.ToString(DateTime.Now.Year) + currentmonth;


                            if (!Directory.Exists(webRoot + "/EmpSalaryRevision/" + company_name + "/" + currentyearmonth + "/"))
                            {
                                Directory.CreateDirectory(webRoot + "/EmpSalaryRevision/" + company_name + "/" + currentyearmonth + "/");

                            }

                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/EmpSalaryRevision/" + company_name + "/" + currentyearmonth + "/");

                            //save file
                            using (var fileStream = new FileStream(Path.Combine(path, MyFileName), FileMode.Create))
                            {
                                files[i].CopyTo(fileStream);

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
                                    SalaryInput objsalaryinput = new SalaryInput();
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
                                                                objsalaryinput.emp_code = item.Text.Text;
                                                            }
                                                            else if (!string.IsNullOrEmpty(item.Text.Text) && currentcolumnno == "C")
                                                            {
                                                                objsalaryinput.component_value = Convert.ToDecimal(item.Text.Text);
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
                                                    objsalaryinput.emp_code = thecurrentcell.InnerText;
                                                }
                                                else if (!string.IsNullOrEmpty(thecurrentcell.InnerText) && currentcolumnno == "C")
                                                {
                                                    objsalaryinput.component_value = Convert.ToDecimal(thecurrentcell.InnerText);
                                                }


#endregion ** END value in list**

                                            }

                                        }

                                    }
                                    excelResult.AppendLine();
                                    objsalaryinput.created_by = objsalary.created_by;
                                    objsalaryinput.company_id = objsalary.company_id;
                                    objsalaryinput.monthyear = objsalary.monthyear;

                                    salaryinputlst.Add(objsalaryinput);
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


                if (!_clsCurrentUser.CompanyId.Any(p => salaryinputlst.Any(q => q.company_id == p)))
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Unauthorize Company access...";
                    return Ok(objresponse);
                }



                // var data = excelResult.ToString();
                // var data1 = salaryinputlst;

                //start check uplicate,missing details
                var dataaa = CheckSalary_Revisionfromexcel(salaryinputlst);

                var duplicate_dtl = dataaa.duplicatesalaryinputlst;
                var missing_dtll = dataaa.missingsalaryinputlst;
                var adddblistt = dataaa.adddbSalaryInputlst;
                var missingDtlMessage = dataaa.DtlMessage;
                var employeenotexist = dataaa.employeenotexist;

                //if (duplicate_dtl.Count > 0 || missing_dtll.Count > 0)
                //{
                //    if(missingDtlMessage != "")
                //    {
                //        return Ok(dataaa);
                //    }
                //    return Ok(dataaa);
                //}
                //else
                //{
                //if(missingDtlMessage != "")
                //{
                //    return Ok(new { dataaa, objresponse });
                //}
                if (adddblistt.Count < 1)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Employee does not exist in selected company !! ";
                    return Ok(new { dataaa, objresponse });
                }

                var isfreezed = _context.tbl_payroll_process_status.FirstOrDefault(b => b.is_deleted == 0 && b.payroll_month_year == Convert.ToInt32(objsalary.monthyear) && adddblistt.Any(p => p.emp_id == b.emp_id) && (b.is_freezed == 1 || b.is_lock == 1));
                // return Ok(salaryvalues);
                if (isfreezed != null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Employee Salary Input of Selected Month year already freezed or locked !!";
                    return Ok(new { dataaa, objresponse });
                }

                //start save details
                int result = _clsEmployeeDetail.SaveSalaryRevisionFromExcel(adddblistt);
                if (result == 0)
                {
                    objresponse.StatusCode = 0;
                    objresponse.Message = "Salary Revision successfully saved..";
                    return Ok(new { dataaa, objresponse });
                }
                else if (result == 2)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Selected Month Salary is already calculated or freezed or Locked...!";
                    return Ok(new { dataaa, objresponse });
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Something went wrong, please try after sometime...";
                    return Ok(new { dataaa, objresponse });
                }
                // }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        private SalaryInputList CheckSalary_Revisionfromexcel(List<SalaryInput> objlst)
        {
            StringBuilder msg = new StringBuilder();

            List<SalaryInput> missingsalaryinputlst = new List<SalaryInput>();
            List<SalaryInput> adddbSalaryInputlst = new List<SalaryInput>();
            List<SalaryInput> duplicatesalaryinputlst = new List<SalaryInput>();
            List<SalaryInput> employee_not_exist = new List<SalaryInput>();

            Response_Msg objresponse = new Response_Msg();

            try
            {
                bool _invalid = false;
                msg.Append("");

                _clsEmployeeDetail._company_id = objlst.FirstOrDefault().company_id;


                var empids = _context.tbl_employee_company_map.Where(x => x.is_deleted == 0 && x.company_id == objlst.FirstOrDefault().company_id).Select(p => p.employee_id).ToList();
                var _codes = _context.tbl_employee_master.Where(x => x.is_active == 1 && empids.Contains(x.employee_id)).ToList();

                if (objlst.Any(x => string.IsNullOrEmpty(x.emp_code)))
                {
                    _invalid = true;
                    msg.Append("Employee Code cannot be blank...!!");
                }
                else
                {
                    var _invalidcode = objlst.Where(x => !_codes.Any(y => y.emp_code.Trim().ToUpper() == x.emp_code.Trim().ToUpper())).ToList();
                    //if (_invalidcode.Count > 0)
                    //{
                    //     _invalid = true;
                    msg.Append("Employee code does not exist!!");
                    _invalidcode.ForEach(x => msg.Append("" + x.emp_code + " ,"));
                    msg.Append("</br>");

                    for (int i = _invalidcode.Count - 1; i >= 0; i--)
                    {
                        SalaryInput objlst_for_exist_emp = new SalaryInput();
                        objlst_for_exist_emp.emp_code = _invalidcode[i].emp_code;
                        objlst.RemoveAll(item => item.emp_code == _invalidcode[i].emp_code);
                        employee_not_exist.Add(objlst_for_exist_emp);
                    }



                    //}
                    //else
                    //{
                    objlst.ForEach(x =>
                    {
                        x.emp_id = _codes.FirstOrDefault(y => y.is_active == 1 && y.emp_code.Trim().ToUpper() == x.emp_code.Trim().ToUpper()).employee_id;
                    });

                    var data = _clsEmployeeDetail.BindEmpSalaryGroup();
                    var sg_not_map = objlst.Where(x => !data.Any(y => y.employee_id == x.emp_id)).ToList();
                    if (sg_not_map.Count > 0)
                    {
                        //       _invalid = true;
                        msg.Append("Salary Group are not defined of following Employee Code:- ");
                        sg_not_map.ForEach(x => msg.Append("" + x.emp_code + " ,"));
                        msg.Append("</br>");
                        for (int i = sg_not_map.Count - 1; i >= 0; i--)
                        {

                            objlst.RemoveAll(item => item.emp_code == sg_not_map[i].emp_code);

                        }

                    }
                    else
                    {
                        var emp_sg = _context.tbl_sg_maping.Where(x => x.is_active == 1 && objlst.Any(y => y.emp_id == x.emp_id)).ToList();

                        objlst.ForEach(x =>
                        {
                            DateTime _effective = Convert.ToDateTime(x.monthyear.ToString().Substring(0, 4) + "-" + x.monthyear.ToString().Substring(4, 2) + "-01");
                            x.salary_group_id = emp_sg.FirstOrDefault(y => y.emp_id == x.emp_id && y.applicable_from_dt.Date <= _effective.Date).salary_group_id ?? 0;
                        });

                        if (objlst.Any(x => x.salary_group_id == 0))
                        {
                            //         _invalid = true;
                            msg.Append("Salary Group not available on selected effective of following Employee Code:- ");
                            objlst.Where(x => x.salary_group_id == 0).ToList().ForEach(y => msg.Append("" + y.emp_code + ""));
                            msg.Append("</br>");
                        }




                        //var effect_Sg =_context.tbl_sg_maping.Where(x => x.emp_id == emp_id && x.is_active == 1 && x.applicable_from_dt <= _effective_date)
                    }

                }
                //}


                if (_invalid)
                {
                    missingsalaryinputlst.AddRange(objlst);
                }
                else
                {
                    var _checkduplicate = objlst.GroupBy(x => x.emp_code).Where(x => x.Count() > 1).ToList();
                    if (_checkduplicate.Count > 0)
                    {
                        duplicatesalaryinputlst.AddRange(objlst);
                    }
                    else
                    {
                        adddbSalaryInputlst.AddRange(objlst);
                    }

                }

            }
            catch (Exception ex)
            {
                msg.Append(ex.Message);
            }
            return new SalaryInputList { missingsalaryinputlst = missingsalaryinputlst, duplicatesalaryinputlst = duplicatesalaryinputlst, adddbSalaryInputlst = adddbSalaryInputlst, DtlMessage = msg.ToString(), employeenotexist = employee_not_exist };
        }


#region schedular for LWP entry made by Anil
        [Route("LWPEntry_Schedular")]
        [HttpGet]
        public IActionResult LWPEntry_Schedular()
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                int dayId = Convert.ToInt32(currentTime.ToString("yyyyMMdd"));

                tbl_scheduler_master LWP = _context.tbl_scheduler_master.Where(p => p.scheduler_name == "LWP" && p.is_deleted == 0).FirstOrDefault();
                if (LWP == null)
                {
                    LWP = new tbl_scheduler_master()
                    {
                        is_deleted = 0,
                        last_runing_date = currentTime,
                        schduler_type = enmSchdulerType.NoDues,
                        scheduler_name = "LWP"
                    };
                    _context.tbl_scheduler_master.Add(LWP);
                    _context.SaveChanges();
                }
                // check for one time run in one day
                var daily_schdeuler = _context.tbl_scheduler_details.Where(p => p.scheduler_id == LWP.scheduler_id && p.is_deleted == 0 && p.transaction_no == dayId).FirstOrDefault();
                if (daily_schdeuler == null)
                {

                    var employeelist = (from e in _context.tbl_employee_master
                                        join c in _context.tbl_employee_company_map on e.employee_id equals c.employee_id
                                        where e.is_active == 1 && c.is_deleted == 0 //&& e.employee_id==1075
                                        select new { e.employee_id, c.company_id }).Distinct().ToList();
                    clsLossPay obj = new clsLossPay(_context);
                    var payroll_process = _context.tbl_payroll_process_status.Where(x => x.is_freezed == 0).OrderByDescending(x => x.pay_pro_status_id).FirstOrDefault();
                    if (payroll_process != null)
                    {
                        var payroll_process_monthyear = payroll_process.payroll_month_year;
                        obj.insert_into_loss_of_pay_schedular(1025, 1, 202105, 0, 30);
                        //foreach (var emp in employeelist)
                        //{
                        //    obj.insert_into_loss_of_pay_schedular(emp.employee_id, Convert.ToInt32(emp.company_id), payroll_process_monthyear, 0, 30);
                        //}

                        _context.tbl_scheduler_details.Add(new tbl_scheduler_details() { transaction_no = dayId, is_deleted = 0, last_runing_date = currentTime, scheduler_id = LWP.scheduler_id });
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

        private List<mdlDayStatus> GetDayStatus(DateTime fromdt, DateTime todate)
        {
            List<mdlDayStatus> dayStatuses = new List<mdlDayStatus>();
            while (fromdt <= todate)
            {
                dayStatuses.Add(new mdlDayStatus() { AttendanceDay = fromdt, DayStatus = 2, absentValue = 1 });
                fromdt = fromdt.AddDays(1);
            }
            return dayStatuses;
        }
#endregion


#region **************************************** FNF Process Region*******************************************************************************

        //get Salary Componenet All
        [Route("Get_SalaryComponenetDetailsALL/{emp_id}/{monthyear}")]
        [HttpGet]
        [Authorize(Policy = nameof(enmMenuMaster.PayrollInput))]
        public IActionResult Get_SalaryComponenetDetailsALL([FromRoute] int emp_id, int monthyear)
        {
            ResponseMsg objresult = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
            {
                objresult.StatusCode = 1;
                objresult.Message = "Unauthorize Access...!!";
                return Ok(objresult);
            }


            try
            {
                int indx = -1;
                if (emp_id <= 0)
                {
                    objresult.StatusCode = 1;
                    objresult.Message = "Invalid Employee Id";
                    return Ok(objresult);
                }
                else
                {
                    string strmonthyear = monthyear.ToString();
                    var result = (from t1 in (from t in _context.tbl_component_master where t.parentid > 0 && t.is_active == 1 && t.is_user_interface == 1 && t.is_fnf_component == 1 select t)
                                  join t3 in (from t in _context.tbl_salary_input where t.is_active == 1 && t.emp_id == emp_id && t.monthyear == monthyear select t) on t1.component_id equals t3.component_id into t1t3
                                  from _t1t3 in t1t3.DefaultIfEmpty()
                                  join t4 in (from t in _context.tbl_salary_input_change where t.is_active == 1 && t.emp_id == emp_id && t.monthyear == strmonthyear select t) on t1.component_id equals t4.component_id into t1t4
                                  from _t1t4 in t1t4.DefaultIfEmpty()
                                  select new
                                  {
                                      t1.component_id,
                                      t1.component_name,
                                      t1.property_details,
                                      t1.component_type,
                                      component_value = (_t1t4 != null ? _t1t4.values : (_t1t3 != null ? _t1t3.values : "0")),
                                      emp_id = emp_id,
                                      parentid = t1.parentid,
                                      t1.is_system_key,
                                      monthyear = monthyear,
                                      is_active = t1.is_active,
                                      is_data_entry_comp = t1.is_data_entry_comp,
                                      is_salary_comp = t1.is_salary_comp
                                  }).ToList().OrderBy(n => n.component_id);


                    return Ok(result);


                }

            }
            catch (Exception ex)
            {
                objresult.StatusCode = 2;
                objresult.Message = ex.Message;
                return Ok(objresult);
            }

        }

        [Route("Get_Lod_MasterByEmp/{emp_Id}/{company_id}/{payrollmonth}")]
        [HttpGet]
        //[Authorize(Policy = nameof(enmMenuMaster.LOP))]
        public IActionResult Get_Lod_MasterByEmp([FromRoute] int emp_Id, int company_id, string payrollmonth)
        {
            ResponseMsg objresponse = new ResponseMsg();
            //if (!_clsCurrentUser.CompanyId.Contains(company_id))
            //{
            //    objresponse.StatusCode = 1;
            //    objresponse.Message = "Unauthorize Company Access....!!";
            //    return Ok(objresponse);
            //}

            try
            {
                int _monthyear = Convert.ToInt32(payrollmonth);
                double ToalPayrollDay = 30, LopDays = 0, PaidDays = 0, additionalPaidDays = 0;
                List<fnf_attandance_dtl> objattendance = new List<fnf_attandance_dtl>();

                var lop_Data = _context.tbl_lossofpay_master.Where(x => x.is_active == 1 && x.monthyear == _monthyear && x.emp_id == emp_Id).GroupBy(y => y.emp_id).Select(p => p.OrderByDescending(h => h.lop_master_id).FirstOrDefault()).FirstOrDefault();
                if (lop_Data != null)
                {
                    ToalPayrollDay = lop_Data.totaldays ?? 0;
                    LopDays = Convert.ToDouble(lop_Data.final_lop_days);
                    additionalPaidDays = Convert.ToDouble(lop_Data.Additional_Paid_days);
                    PaidDays = ToalPayrollDay - LopDays;
                }
                objattendance = _context.tbl_fnf_attendance_dtl.Where(x => x.fnf_mstr.emp_id == emp_Id && x.fnf_mstr.emp_sep_mstr.is_deleted == 0).Select(p => new fnf_attandance_dtl
                {
                    fnf_attend_id = p.fnf_attend_id,
                    monthyear = p.monthyear,
                    totaldays = p.totaldays,
                    Holiday_days = p.Holiday_days,
                    Week_off_days = p.Week_off_days,
                    Present_days = p.Present_days,
                    Absent_days = p.Absent_days,
                    Leave_days = p.Leave_days,
                    acutual_lop_days = p.acutual_lop_days,
                    final_lop_days = p.final_lop_days,
                    Total_Paid_days = p.Total_Paid_days,
                    Additional_Paid_days = additionalPaidDays,
                }).ToList();

                if (objattendance.Count < 1)
                {
                    objattendance = _context.tbl_lossofpay_master.Where(x => x.is_active == 1 && x.monthyear == _monthyear && x.emp_id == emp_Id).Select(p => new fnf_attandance_dtl
                    {
                        fnf_attend_id = 0,
                        monthyear = (int)p.monthyear,
                        totaldays = (int)p.totaldays,
                        Holiday_days = (int)p.Holiday_days,
                        Week_off_days = (int)p.Week_off_days,
                        Present_days = (int)p.Present_days,
                        Absent_days = (int)p.Absent_days,
                        Leave_days = (int)p.Leave_days,
                        acutual_lop_days = (int)p.acutual_lop_days,
                        final_lop_days = (int)p.final_lop_days,
                        Total_Paid_days = (int)p.Total_Paid_days,
                        Additional_Paid_days = (int)additionalPaidDays,
                    }).ToList();

                }

                if (objattendance == null || objattendance.Count < 1)
                {
                    var a = new fnf_attandance_dtl
                    {
                        fnf_attend_id = 0,
                        monthyear = Convert.ToInt32(payrollmonth),
                        totaldays = 0,
                        Holiday_days = 0,
                        Week_off_days = 0,
                        Present_days = 0,
                        Absent_days = 0,
                        Leave_days = 0,
                        acutual_lop_days = 0,
                        final_lop_days = 0,
                        Total_Paid_days = 0,
                        Additional_Paid_days = 0,
                    };
                    objattendance.Add(a);
                }

                return Ok(objattendance);
                //}
            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [HttpGet("GetFNFDetails/{emp_id}/{sep_id}")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpFNFProcess))]
        public async Task<IActionResult> GetFNFDetails([FromRoute] int emp_id, int sep_id)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Uauthorize Access...!!";
                return Ok(objresponse);
            }

            try
            {
                List<int> _empid = new List<int>();
                _empid.Add(emp_id);
                List<int> _reqid = new List<int>();
                _reqid.Add(sep_id);
                var exist = _context.tbl_emp_separation.Where(x => x.sepration_id == sep_id && x.emp_id == emp_id && x.is_deleted == 0 && _clsCurrentUser.CompanyId.Contains(x.company_id)).FirstOrDefault();
                if (exist != null)
                {
                    clsFNFProcess _clsFNFProcess = new clsFNFProcess(_context, _config, _AC, _clsCurrentUser);
                    _clsFNFProcess.EmpIDs = _empid;
                    _clsFNFProcess.ReqID = _reqid;
                    var data = _clsFNFProcess.Get_FNFProcessData();

                    return Ok(await data);
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Sepration Request not exist...!!";
                    return Ok(objresponse);
                }

            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [HttpPost("Save_Emp_FNF_asset")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpFNFProcess))]
        public IActionResult Save_Emp_FNF_asset([FromBody] mdlFNFProcess objasset)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Any(p => objasset.mdl_assetrequest.Any(q => q.empid == p)))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access....!!";
                return Ok(objresponse);
            }
            try
            {

                clsFNFProcess _clsFNFProcess = new clsFNFProcess(_context, _config, _AC, _clsCurrentUser);

                int data = _clsFNFProcess.Save_AssetRecovery(objasset.mdl_assetrequest);
                if (data == 0)
                {
                    objresponse.StatusCode = 0;
                    objresponse.Message = "successfully saved";
                }
                else
                {
                    if (data == 2)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "FNF Process freezed, Now can't update";
                    }
                    else
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Invalid Asset request for recovery";
                    }

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


        [HttpPost("Save_Emp_LeaveEncash")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpFNFProcess))]
        public IActionResult Save_Emp_LeaveEncash([FromBody] mdlFNFProcess objleave_encash)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(objleave_encash.emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }

            try
            {
                var sep_req = _context.tbl_emp_separation.Where(x => x.emp_id == objleave_encash.emp_id && x.sepration_id == objleave_encash.sep_id && x.is_deleted == 0).FirstOrDefault();
                if (sep_req == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Sepration Request not exist, something went wrong";
                    return Ok(objresponse);
                }

                int resign_monthyear = Convert.ToInt32(sep_req.resignation_dt.Year.ToString() + (sep_req.resignation_dt.Month.ToString().Length > 1 ? sep_req.resignation_dt.Month.ToString() : "0" + sep_req.resignation_dt.Month));

                clsFNFProcess _clsFNFProcess = new clsFNFProcess(_context, _config, _AC, _clsCurrentUser);
                _clsFNFProcess._monthyear = resign_monthyear;
                int _result = _clsFNFProcess.Save_LeaveEncash(objleave_encash);
                if (_result == 0)
                {
                    objresponse.StatusCode = 0;
                    objresponse.Message = "Leave encash successfully saved...";
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Eror Occured! Leave encash not saved";
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

        [HttpPost("Save_FNF_Attendence")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpFNFProcess))]
        public IActionResult Save_FNF_Attendence([FromBody] mdlFNFProcess objfnf_att)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(objfnf_att.emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }

            try
            {
                clsFNFProcess _clsFNFProcess = new clsFNFProcess(_context, _config, _AC, _clsCurrentUser);
                _clsFNFProcess._monthyear = Convert.ToInt32(objfnf_att.monthYear);
                int _result = _clsFNFProcess.Save_FNFAttendence(objfnf_att);
                if (_result == 0)
                {
                    objresponse.StatusCode = 0;
                    objresponse.Message = "FNF Attendence successfully saved";
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Eror Occured! FNF Attendence not saved...";
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

        [HttpPost("Save_Emp_VariablePay")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpFNFProcess))]
        public IActionResult Save_Emp_VariablePay([FromBody] mdlFNFProcess objvar_pay)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(objvar_pay.emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }

            try
            {
                var sep_req = _context.tbl_emp_separation.Where(x => x.emp_id == objvar_pay.emp_id && x.sepration_id == objvar_pay.sep_id && x.is_deleted == 0).FirstOrDefault();
                if (sep_req == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Sepration Request not exist, something went wrong";
                    return Ok(objresponse);
                }

                clsFNFProcess _clsFNFProcess = new clsFNFProcess(_context, _config, _AC, _clsCurrentUser);
                _clsFNFProcess._monthyear = Convert.ToInt32(objvar_pay.monthYear);
                int _result = _clsFNFProcess.Save_VariablePay(objvar_pay);
                if (_result == 0)
                {
                    objresponse.StatusCode = 0;
                    objresponse.Message = "Variable Pay details successfully saved...";
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Eror Occured! variable pay details not saved";
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


        [HttpPost("Save_FNF_Reimbursment")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpFNFProcess))]
        public IActionResult Save_FNF_Reimbursment([FromBody] mdlFNFProcess objtbl)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(objtbl.emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }


            try
            {
                var sep_exist = _context.tbl_emp_separation.Where(x => x.sepration_id == objtbl.company_id && x.emp_id == objtbl.emp_id && x.is_deleted == 0).FirstOrDefault();
                if (sep_exist == null)
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Employee Sepration Request not exist";
                    return Ok(objresponse);
                }


                clsFNFProcess _clsFNFProcess = new clsFNFProcess(_context, _config, _AC, _clsCurrentUser);
                int result_ = _clsFNFProcess.Save_Reimbursmnet(objtbl);
                if (result_ == 0)
                {
                    objresponse.StatusCode = 0;
                    objresponse.Message = "Reimbursment successfully saved...!!";
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Reimbursment request already exist/ FNF Process is Freezed ";

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

        [HttpPost("DeleteFNFReimbursment")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpFNFProcess))]
        public IActionResult DeleteFNFReimbursment([FromBody] mdlFNFProcess objtbl_)
        {
            ResponseMsg objresponse = new ResponseMsg();

            var fnf_mstr_ = _context.tbl_fnf_master.Where(x => x.pkid_fnfMaster == objtbl_.fnf_id && x.emp_id == objtbl_.emp_id).ToList();
            if (fnf_mstr_.Count > 0)
            {
                foreach (var empid in fnf_mstr_)
                {
                    if (!_clsCurrentUser.DownlineEmpId.Contains(empid.emp_id ?? 0))
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Unauthorize Access";
                        return Ok(objresponse);
                    }
                }

                //clsFNFProcess _clsfnfprocess = new clsFNFProcess(_context, _config, _AC, _clsCurrentUser);
                //_clsfnfprocess.Deletereuest=new 

                return Ok(objresponse);
            }
            else
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Invalid Request";
                return Ok(objresponse);
            }

            try
            {

            }
            catch (Exception ex)
            {
                objresponse.StatusCode = 2;
                objresponse.Message = ex.Message;
                return Ok(objresponse);
            }
        }

        [HttpPost("Save_FNF_Process")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpFNFProcess))]
        public IActionResult Save_FNF_Process([FromBody] mdlFNFProcess objfnf_Process)
        {
            ResponseMsg objresponse = new ResponseMsg();
            if (!_clsCurrentUser.DownlineEmpId.Contains(objfnf_Process.emp_id))
            {
                objresponse.StatusCode = 1;
                objresponse.Message = "Unauthorize Access...!!";
                return Ok(objresponse);
            }
            if (!_clsCurrentUser.CompanyId.Contains(objfnf_Process.company_id))
            {
                objresponse.StatusCode = 0;
                objresponse.Message = "Unauthorize Company Access";
                return Ok(objresponse);
            }

            try
            {
                string year = objfnf_Process.monthYear.ToString().Substring(0, 4);
                string month = objfnf_Process.monthYear.ToString().Substring(4, 2);
                DateTime _ApplicableDate = Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1).AddDays(-1);

                clsFNFProcess _clsFNFProcess = new clsFNFProcess(_context, _config, _AC, _clsCurrentUser);
                _clsFNFProcess._monthyear = Convert.ToInt32(objfnf_Process.monthYear);
                int _result = _clsFNFProcess.Save_FNFProcess(objfnf_Process);

                if (_result == 0)
                {
                    var selectEmpSalaryGroup = _context.tbl_sg_maping.Where(p => p.applicable_from_dt <= _ApplicableDate && p.is_active == 1).GroupBy(g => new { employee_id = g.emp_id })
                    .Select(p => new { employee_id = p.Key.employee_id ?? 0, applicable_from_dt = p.Max(q => q.applicable_from_dt) })
                    .Join(_context.tbl_sg_maping.Where(p => p.is_active == 1).Select(p => new { employee_id = p.emp_id ?? 0, applicable_from_dt = p.applicable_from_dt, salary_group_id = p.salary_group_id })
                    , p => new { p.employee_id, p.applicable_from_dt }, q => new { q.employee_id, q.applicable_from_dt },
                    (p, q) => new { p.employee_id, q.salary_group_id })
                    .Join(_context.tbl_salary_group, p => p.salary_group_id, q => q.group_id,
                    (p, q) => new { p.employee_id, q.group_name, q.group_id })
                    .ToList();

                    int[] NewEmpList = { objfnf_Process.emp_id };

                    /// insertDatainPayrollProcess(NewEmpList.ToList(), objfnf_Process.company_id, Convert.ToInt32(objfnf_Process.monthYear), objfnf_Process.created_by ?? 1);
                    //Now Start in task an Call the fun
                    //List<tbl_payroll_process_status> tpps = _context.tbl_payroll_process_status.Where(p => objfnf_Process.emp_id==p.emp_id  && p.is_freezed == 0 && p.is_lock == 0 && p.is_deleted == 0 && p.payroll_month_year == Convert.ToInt32(objfnf_Process.monthYear)).ToList();

                    List<Task> tasklists = new List<Task>();
                    Task tasklist = new Task(() =>
                    {
                        using (Context context = new Context())
                        {

                            Payroll pr = new Payroll(objfnf_Process.company_id, _appSettings.Value.domain_url.ToString(), _converter, _hostingEnvironment, context,
                                                                   EmpID: objfnf_Process.emp_id, PayrollMonthyear: Convert.ToInt32(objfnf_Process.monthYear), userId: objfnf_Process.created_by ?? 1, SGID: selectEmpSalaryGroup.FirstOrDefault(q => q.employee_id == objfnf_Process.emp_id).group_id, config: _config, sepId: objfnf_Process.sep_id);
                            pr.insert_into_tables_fnf();
                        }
                    });
                    tasklists.Add(tasklist);
                    tasklist.Start();

                    Task.WaitAll(tasklists.ToArray());
                    var fnf_id = _context.tbl_fnf_master.Where(x => x.emp_id == objfnf_Process.emp_id && x.fkid_empSepration == objfnf_Process.sep_id && x.is_deleted == 0).FirstOrDefault().pkid_fnfMaster;
                    if (fnf_id > 0)
                    {
                        //decimal totolLeaveEncashAmount = Convert.ToDecimal(_context.tbl_fnf_leave_encash.Where(x => x.fnf_id == fnf_id).Sum(y => y.leave_encash_final));
                        decimal totolAttendenceAmount = Convert.ToDecimal(_context.tbl_salary_input.Where(p => p.is_fnf_comp == 1 && p.emp_id == objfnf_Process.emp_id && p.monthyear == Convert.ToInt32(objfnf_Process.monthYear) && p.component_id == (int)enmOtherComponent.Net && p.is_active == 1).FirstOrDefault().current_month_value);
                        //decimal totolComponentPAmount = Convert.ToDecimal(_context.tbl_fnf_component.Where(x => x.fnf_id == fnf_id && (x.component_type == 1 || x.component_type == 4)).Sum(y => y.variable_amt));
                        //decimal totolComponentNAmount = Convert.ToDecimal(_context.tbl_fnf_component.Where(x => x.fnf_id == fnf_id && (x.component_type == 2 || x.component_type == 5)).Sum(y => y.variable_amt));
                        decimal tot_Amount = totolAttendenceAmount;

                        _context.tbl_fnf_master.Where(x => x.pkid_fnfMaster == fnf_id).ForEachAsync(y => y.net_amt = tot_Amount);

                        _context.SaveChanges();
                    }
                    objresponse.StatusCode = 0;
                    objresponse.Message = "FNF Process successfully saved";
                }
                else
                {
                    objresponse.StatusCode = 1;
                    objresponse.Message = "Eror Occured! FNF Process not saved...";
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


#endregion **************************************** FNF Process Region*******************************************************************************

#region Save settlement data for FNF process made by Anil

        [HttpPost("Save_Settlement_FNF")]
        [Authorize(Policy = nameof(enmMenuMaster.EmpFNFProcess))]
        public IActionResult Save_Settlement_FNF([FromBody] tbl_fnf_master objset)
        {
            ResponseMsg objresponse = new ResponseMsg();

            try
            {
                clsFNFProcess _clsFNFProcess = new clsFNFProcess(_context, _config, _AC, _clsCurrentUser);

                int data = _clsFNFProcess.SettlementFNF(objset);
                if (data == 0)
                {
                    objresponse.StatusCode = 0;
                    objresponse.Message = "successfully saved";
                }
                else
                {
                    if (data == 2)
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "FNF Process freezed, Now can't update";
                    }
                    else
                    {
                        objresponse.StatusCode = 1;
                        objresponse.Message = "Invalid request";
                    }

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

        [Route("Get_FNFReport/{companyid}/{employeeid}")]
        [HttpPost]
        [Authorize(Policy = nameof(enmMenuMaster.SalarySlip))]
        public IActionResult Get_FNFReport([FromRoute] int companyid, int employeeid)
        {
            Response_Msg objResult = new Response_Msg();
            if (!_clsCurrentUser.DownlineEmpId.Any(p => p == employeeid))
            {
                objResult.StatusCode = 1;
                objResult.Message = "Unauthorize Access...!";
                return Ok(objResult);
            }
            try
            {
                if (companyid > 0)
                {
                    var data = (from f in _context.tbl_fnf_master
                                join s in _context.tbl_emp_separation on f.fkid_empSepration equals s.sepration_id
                                join emp in _context.tbl_emp_officaial_sec on f.emp_id equals emp.employee_id
                                join u in _context.tbl_user_master on emp.employee_id equals u.employee_id
                                where f.emp_id == s.emp_id && f.is_deleted == 0 && s.is_deleted == 0
                                && s.is_cancel == 0 && emp.is_deleted == 0
                                select new
                                {
                                    emp_id = f.emp_id,
                                    emp_code = u.tbl_employee_id_details.emp_code,
                                    emp_name = string.Format("{0} {1} {2}", emp.employee_first_name, emp.employee_middle_name, emp.employee_last_name),
                                    resignationdate = s.resignation_dt,
                                    reqreason = s.req_reason,
                                    noticedays = s.req_notice_days,
                                    isdue = s.is_no_due_cleared,
                                    settlment_amt = f.settlment_amt,
                                    created_date = f.created_dt,
                                    settlement_type = f.settlement_type,
                                    net_amt = f.net_amt,
                                    settlement_date = f.settlment_dt,
                                    company_id = s.company_id,
                                    isfreezed = f.is_freezed,
                                    fnf_month_year = f.monthYear

                                }).ToList();

                    return Ok(data);
                }
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

    }
}



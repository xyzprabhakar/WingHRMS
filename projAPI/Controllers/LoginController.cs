using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using projAPI.Model;
using projContext.DB;
using Microsoft.EntityFrameworkCore;
using System.Net;
using projAPI.Classes;
using System.IO;
using projContext;
using DocumentFormat.OpenXml.InkML;

namespace projAPI.Controllers
{
#if(false)
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private IConfiguration _config;
        projContext.Context _context;
        private readonly IOptions<AppSettings> _appSettings;
        private IHttpContextAccessor _accessor;
        clsEmployeeDetail _clsEmployeeDetail;
        clsCurrentUser _clsCurrentUser;
        public LoginController(projContext.Context context, IConfiguration config, IOptions<AppSettings> appSettings, IHttpContextAccessor accessor, clsEmployeeDetail _clsEmployeeDetail, clsCurrentUser _clsCurrentUser)
        {
            _context = context;
            _config = config;
            _appSettings = appSettings;
            _accessor = accessor;
            this._clsEmployeeDetail = _clsEmployeeDetail;
            this._clsCurrentUser = _clsCurrentUser;
        }



        // Funcation for Get Componet Tree




        #region ******************* Login New Function





        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody]mdlLogin login)
        {
            IActionResult response = Unauthorized();
            mdlLoginOutput mdl = new mdlLoginOutput();
            mdl.StatusCode = 0;
            mdl.version_ios = Convert.ToInt32(_config["appversions:version_ios"]);
            mdl.version_android = Convert.ToInt32(_config["appversions:version_android"]);

            try
            {
                //Create the Instance of User class
                Classes.clsUsersDetails ob = new clsUsersDetails(_context, _config, login.user_name.Trim().ToUpper(), login.password, 0);
                //Check if IP is Blocked 
                string userAgent = Request.Headers["User-Agent"];
                string ipAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                //userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36";
                //ipAddress = "14.98.159.241";

                if (ipAddress != "::1" && ipAddress != "14.143.183.84" && ipAddress != "14.98.159.241" && ob.IsIPBlock(ipAddress, userAgent))
                {
                    mdl.StatusMessage = "IP block...";
                    mdl.StatusCode = 0;
                    return Ok(mdl);
                }
                else if (login.fromexe != 32 && !ob.ValidateCaptcha(login.CcCode, login.CaptchaCode))//use for exe bypass captcha logic for login access
                {

                    mdl.StatusMessage = "Captcha Invalid...!";
                    mdl.StatusCode = 0;
                    return Ok(mdl);

                }
                else if (ob._UserId == 0)//Invalid Username and password
                {
                    //insert into log
                    mdl.StatusMessage = "Invalid User or Password...!";
                    ob.LoginLog(ipAddress, userAgent, 1);
                    mdl.StatusCode = 0;
                    return Ok(mdl);
                }
                else if (ob._tbl_user_master.is_active == 1 && ob._tbl_user_master.password == login.password)
                {
                    if (ob.IsLoginBlock())
                    {
                        mdl.StatusMessage = "Blocked Users...!";
                        ob.LoginLog(ipAddress, userAgent, 1);
                        mdl.StatusCode = 0;
                        return Ok(mdl);
                    }
                    //else if (Convert.ToString(_config["UserSetting:alreadylogin"]).Trim().ToLower() == "true" &&
                    //    ob.IsAlreadyLogedin(ipAddress, userAgent))
                    //{
                    //    mdl.StatusMessage = "Already loged in ...!";
                    //    mdl.StatusCode = 0;
                    //    return Ok(mdl);
                    //}
                    else
                    {
                        //if (login.fromexe != 32)
                        //{
                        //    ob.LoginLog(ipAddress, userAgent, 0);
                        //}

                        mdl.user_id = ob._UserId;
                        mdl.user_name = ob._tbl_user_master.username;
                        mdl.emp_id = ob._tbl_user_master.employee_id ?? 0;
                        ob.LoadEmpSpecificDetail(mdl);
                        if (mdl.employee_role_id.Contains((int)enmRoleMaster.Manager)
                            || mdl.employee_role_id.Contains((int)enmRoleMaster.SectionHead)
                            || mdl.employee_role_id.Contains((int)enmRoleMaster.TeamLeader)
                            || mdl.employee_role_id.Contains((int)enmRoleMaster.Management)
                            || mdl.employee_role_id.Contains((int)enmRoleMaster.Consultant)

                            )
                        {
                            mdl.is_reporting_manager = 1;
                            ob.LoadEmpDownlineEmployee(mdl);
                        }
                        if (mdl.employee_role_id.Any(p => p > 100) || mdl.employee_role_id.Contains((int)enmRoleMaster.SuperAdmin))
                        {
                            mdl.is_hod = 2;
                        }
                        else if (mdl.is_reporting_manager == 1)
                        {
                            mdl.is_hod = 1;
                        }
                        else
                        {
                            mdl.is_hod = 0;
                        }

                        ob.LoadCompanyData(mdl);

                        mdl.menu_lst = ob.LoadUserMenu(mdl.employee_role_id, mdl.is_hod);
                        mdl.emp_claim_id = ob._tbl_role_menu_master;
                        mdl.emp_company_lst = ob.Get_emp_company_lst(mdl.emp_id, mdl);
                        
                        _clsCurrentUser.EmpId = mdl.emp_id;
                        _clsCurrentUser.UserId = mdl.user_id;
                        mdl._under_emp_lst = ob.Get_Emp_dtl_under_login_emp(mdl, _clsEmployeeDetail);
                        // start check first time login or not

                        if (login.user_name.Trim().ToUpper() == AESEncrytDecry.DecryptStringAES(ob._tbl_user_master.password).Trim().ToUpper())
                        {
                            mdl._firsttimelogin = 1;
                        }
                        else
                        {
                            mdl._firsttimelogin = 0;
                        }

                        // end check first time login 

                        mdl.Token = GenerateJSONWebToken(mdl);
                        //Load the all data
                        mdl.StatusCode = 1;
                        mdl.app_setting = _context.tbl_app_setting.Where(a => a.is_active == 1).Select(a => a.AppSettingValue).FirstOrDefault();
                        //FunIsApplicationFreezed() = mdl.app_setting;
                        mdl.app_setting_dic = _context.tbl_app_setting.Where(x => x.is_active == 1).ToDictionary(x => x.AppSettingKey.ToString(), y => y.AppSettingValue.ToString());

                        return Ok(mdl);
                    }

                }
                else
                {
                    ob.LoginLog(ipAddress, userAgent, 1);
                    if (ob._UserId > 0)
                    {
                        //mdl.wrong_attempt = ob.BlockUserIdAfterFailAttempt();
                        mdl.wrong_attempt = 0;
                    }
                    mdl.StatusMessage = "Invalid User or Password...!";
                    return Ok(mdl);
                }

            }
            catch (Exception ex)
            {
                mdl.StatusMessage = "Invalid User or Password...!";
                mdl.StatusCode = 0;
                return Ok(mdl);
            }
        }
        #endregion


        private string GenerateJSONWebToken(mdlLoginOutput mdl)
        {
            IActionResult response = Unauthorized();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> _claim = new List<Claim>();
            _claim.Add(new Claim("__emp_id", mdl.emp_id.ToString()));
            _claim.Add(new Claim("__user_id", mdl.user_id.ToString()));
            _claim.Add(new Claim("__Is_HOD", mdl.is_hod.ToString()));
            _claim.Add(new Claim("__company_id_list", string.Join(",", mdl.company_list)));
            _claim.Add(new Claim("__employee_role_id", string.Join(",", mdl.employee_role_id)));
            _claim.Add(new Claim("__downline_emp_id", string.Join(",", string.Join(",", mdl._under_emp_lst.Select(p => p._empid)))));
            foreach (enmMenuMaster _enm in Enum.GetValues(typeof(enmMenuMaster)))
            {
                if (mdl.emp_claim_id.Contains((int)_enm))
                {
                    _claim.Add(new Claim(_enm.ToString(), ((int)_enm).ToString()));
                }
            }
            if (mdl.employee_role_id.Count > 0)
            {
                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              _claim,
              expires: DateTime.Now.AddHours(Convert.ToInt32(_config["Jwt:tokenExpireinhour"])),
              // expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                return "";
            }

        }


        [HttpGet]
        ////[Authorize(Policy = "1")]
        public ActionResult<IEnumerable<mdlLogin>> Get()
        {
            List<mdlLogin> mdlLogin_ = new List<mdlLogin>();
            mdlLogin_.Add(new mdlLogin { user_name = "Prabhakar", emp_name = "test.btest@gmail.com", emp_id = 1 });
            mdlLogin_.Add(new mdlLogin { user_name = "Prabhakar1", emp_name = "test.btest@gmail.com", emp_id = 2 });
            mdlLogin_.Add(new mdlLogin { user_name = "Prabhakar1", emp_name = "test.btest@gmail.com", emp_id = 3 });
            return mdlLogin_;
        }

        #region START BY SUPRIYA FOR GETTING DASHBOARD MENUS ON 27-05-2019

        //[HttpGet]
        //////[Authorize(Policy = "11001")]
        //[Route("GetDashBoardMenu/{role_id}")]
        //public IActionResult GetDashBoardMenu([FromRoute]int role_id)
        //{
        //    IActionResult response = Unauthorized();
        //    try
        //    {
        //        var role_menu_data = _context.tbl_role_menu_master.Where(a => a.role_id == role_id).FirstOrDefault();

        //        var menu_master_data = _context.tbl_menu_master.ToList();
        //        return Ok(new { rolee_menu_data = role_menu_data, menu_master_dataa = menu_master_data });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { errorr = ex.Message });
        //    }

        //}

        #endregion END BY SUPRIYA FOR GETTING DASHBOARD MENUS ON 27-05-2019


        #region ** START BY SUPRIYA ON 04-09-2019, UPDATE IN LOGIN LOGS TABLE WHEN LOGOUT**

        [Route("Updata_Login_Logs/{emp_id}/{user_id}")]
        [HttpPost]
        public IActionResult Updata_Login_Logs([FromRoute] int emp_id, int user_id)
        {
            try
            {
                ResponseMsg objresponse = new ResponseMsg();
                clsUsersDetails objuser = new clsUsersDetails(_context, _config, user_id, user_id);

                objuser.LoggedOut();


                //tbl_user_login_logs objtable = new tbl_user_login_logs();
                // List<tbl_user_login_logs> existdetail = _context.tbl_user_login_logs.Where(x => x.emp_id == emp_id && x.user_id == user_id && x.is_user_active == 2).ToList();
                // existdetail.ForEach(y => { y.is_user_active = 1; });
                // _context.tbl_user_login_logs.UpdateRange(existdetail);
                // _context.SaveChanges();

                objresponse.StatusCode = 0;
                objresponse.Message = "Successufully Logout";

                return Ok(objresponse);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        #endregion ** END BY SUPRIYA ON 04-09-2019, UPDATE IN LOGIN LOGS TABLE WHEN LOGOUT**



        #region Amarjeet created date 06-09-2019  update_login_user_login_time
        [Route("update_login_user_login_time/{emp_id}/{user_id}")]
        [HttpPost]
        public async Task<IActionResult> update_login_user_login_time([FromRoute] int emp_id, int user_id)
        {
            throw new NotImplementedException();

            //ResponseMsg objresponse = new ResponseMsg();
            //try
            //{
            //    tbl_user_login_logs existdetail = _context.tbl_user_login_logs.Where(x => x.emp_id == emp_id && x.user_id == user_id).OrderByDescending(a => a.login_date_time).FirstOrDefault();
            //    if (existdetail != null)
            //    {
            //        existdetail.login_date_time = DateTime.Now;
            //        _context.tbl_user_login_logs.Attach(existdetail);
            //        _context.Entry(existdetail).State = EntityState.Modified;
            //        await _context.SaveChangesAsync();
            //    }
            //    objresponse.StatusCode = 0;
            //    objresponse.Message = "Successufully Update...!";

            //    return Ok(objresponse);
            //}
            //catch (Exception ex)
            //{
            //    objresponse.Message = "Server busy please try again later...!";
            //    objresponse.StatusCode = 0;
            //    return Ok(objresponse);
            //}
        }


        #endregion



        [Route("get-captcha-image/{guid}")]
        [HttpGet]
        public IActionResult GetCaptchaImage(string guid)
        {
            throw new NotImplementedException();
#if (false)
            projContext.Context db = new projContext.Context();

            //var get_captcha_code = db.tbl_captcha_code_details.Where(a => a.genration_dt.Date < DateTime.Now.Date).ToList();

            //if (get_captcha_code != null)
            //{
            //    //remove data from captcha code table
            //    db.tbl_captcha_code_details.RemoveRange(get_captcha_code);
            //}

            int width = 100;
            int height = 36;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            MemoryStream s = new MemoryStream(result.CaptchaByteData);
            byte[] dt = s.ToArray();

            //////////////////////////////// Insert Data in tbl_captcha_code_details                        
            tbl_captcha_code_details tbl = new tbl_captcha_code_details();
            string gg = Guid.NewGuid().ToString().Replace("-", "");
            tbl.guid = gg;
            tbl.captcha_code = captchaCode;
            tbl.genration_dt = DateTime.Now;
            db.tbl_captcha_code_details.Add(tbl);
            db.SaveChanges();
            /////////////////////////////////

            return Ok(new { img = "data:image/jpeg;base64," + Convert.ToBase64String(dt), code = tbl.guid.ToString(), captcha_code = captchaCode });
        #endif
        }

        [Route("setformula")]
        public string setformula()
        {
            List<tbl_component_master> component_Masters = new List<tbl_component_master>();
            List<tbl_component_formula_details> Formula_Details = new List<tbl_component_formula_details>();

            #region
            component_Masters.Add(new tbl_component_master()
            {
                component_id = 1,
                component_name = "@SystemComponent",
                datatype = "3",
                defaultvalue = "-",
                parentid = 0,
                is_system_key = 1,
                System_function = "",
                System_table = null,
                component_type = (int)enmComponentType.Other,
                is_salary_comp = 0,
                is_tds_comp = 0,
                is_data_entry_comp = 0,
                payment_type = 0,
                is_user_interface = 1,
                is_payslip = 0,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                modified_by = 1,
                modified_dt = new DateTime(2020, 1, 1),
                is_active = 1,
                property_details = "System Component"
            });
            component_Masters.Add(new tbl_component_master()
            {
                component_id = 1000,
                component_name = "@EmployeeSalary",
                datatype = "3",
                defaultvalue = "-",
                parentid = 0,
                is_system_key = 1,
                System_function = "",
                System_table = null,
                component_type = (int)enmComponentType.Other,
                is_salary_comp = 0,
                is_tds_comp = 0,
                is_data_entry_comp = 0,
                payment_type = 0,
                is_user_interface = 1,
                is_payslip = 0,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                modified_by = 1,
                modified_dt = new DateTime(2020, 1, 1),
                is_active = 1,
                property_details = "Employee Salary"
            });
            component_Masters.Add(new tbl_component_master()
            {
                component_id = 2000,
                component_name = "@EmployeeIncome",
                datatype = "3",
                defaultvalue = "0",
                parentid = 1000,
                is_system_key = 1,
                System_function = "",
                System_table = null,
                component_type = (int)enmComponentType.Other,
                is_salary_comp = 0,
                is_tds_comp = 0,
                is_data_entry_comp = 0,
                payment_type = 0,
                is_user_interface = 1,
                is_payslip = 0,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                modified_by = 1,
                modified_dt = new DateTime(2020, 1, 1),
                is_active = 1,
                property_details = "Employee Income"
            });
            component_Masters.Add(new tbl_component_master()
            {
                component_id = 3000,
                component_name = "@EmployeeDeduction",
                datatype = "3",
                defaultvalue = "0",
                parentid = 1000,
                is_system_key = 1,
                System_function = "",
                System_table = null,
                component_type = (int)enmComponentType.Other,
                is_salary_comp = 0,
                is_tds_comp = 0,
                is_data_entry_comp = 0,
                payment_type = 0,
                is_user_interface = 1,
                is_payslip = 0,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                modified_by = 1,
                modified_dt = new DateTime(2020, 1, 1),
                is_active = 1,
                property_details = "Employee Deduction"
            });
            component_Masters.Add(new tbl_component_master()
            {
                component_id = 4000,
                component_name = "@EmployerDeduction",
                datatype = "3",
                defaultvalue = "0",
                parentid = 1000,
                is_system_key = 1,
                System_function = "",
                System_table = null,
                component_type = (int)enmComponentType.Other,
                is_salary_comp = 0,
                is_tds_comp = 0,
                is_data_entry_comp = 0,
                payment_type = 0,
                is_user_interface = 1,
                is_payslip = 0,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                modified_by = 1,
                modified_dt = new DateTime(2020, 1, 1),
                is_active = 1,
                property_details = "Employer Deduction"
            });

            Formula_Details.Add(new tbl_component_formula_details()
            {
                sno = 1,
                component_id = 1,
                company_id = 1,
                salary_group_id = 1,
                formula = "0",
                function_calling_order = 1,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                deleted_by = 1,
                deleted_dt = new DateTime(2020, 1, 1),
                is_deleted = 1,

            });
            Formula_Details.Add(new tbl_component_formula_details()
            {
                sno = 1000,
                component_id = 1000,
                company_id = 1,
                salary_group_id = 1,
                formula = "0",
                function_calling_order = 1,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                deleted_by = 1,
                deleted_dt = new DateTime(2020, 1, 1),
                is_deleted = 1,

            });
            Formula_Details.Add(new tbl_component_formula_details()
            {
                sno = 2000,
                component_id = 2000,
                company_id = 1,
                salary_group_id = 1,
                formula = "0",
                function_calling_order = 1,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                deleted_by = 1,
                deleted_dt = new DateTime(2020, 1, 1),
                is_deleted = 1,

            });
            Formula_Details.Add(new tbl_component_formula_details()
            {
                sno = 3000,
                component_id = 3000,
                company_id = 1,
                salary_group_id = 1,
                formula = "0",
                function_calling_order = 1,
                created_by = 1,
                created_dt = new DateTime(2020, 1, 1),
                deleted_by = 1,
                deleted_dt = new DateTime(2020, 1, 1),
                is_deleted = 1,

            });


            //foreach (enmOtherComponent _enm in Enum.GetValues(typeof(enmOtherComponent)))
            //{
            //    PayrollComponent payrollComponent = _enm.GetComponentDetails();
            //    component_Masters.Add(new tbl_component_master()
            //    {
            //        component_id = (int)_enm,
            //        component_name = "@" + _enm,
            //        datatype = payrollComponent.datatype.ToString(),
            //        defaultvalue = payrollComponent.defaultvalue,
            //        parentid = payrollComponent.parentid,
            //        is_system_key = payrollComponent.is_system_key,
            //        System_function = payrollComponent.System_function,
            //        System_table = null,
            //        component_type = (int)payrollComponent.ComponentType,
            //        is_salary_comp = payrollComponent.is_salary_comp,
            //        is_tds_comp = 0,
            //        is_data_entry_comp = payrollComponent.is_data_entry_comp,
            //        payment_type = 0,
            //        is_user_interface = payrollComponent.is_user_interface,
            //        is_payslip = payrollComponent.is_payslip,
            //        created_by = 1,
            //        created_dt = new DateTime(2020, 1, 1),
            //        modified_by = 1,
            //        modified_dt = new DateTime(2020, 1, 1),
            //        is_active = 1,
            //        property_details = payrollComponent.component_name
            //    });
            //    Formula_Details.Add(new tbl_component_formula_details()
            //    {
            //        sno = (int)_enm,
            //        component_id = (int)_enm,
            //        company_id = 1,
            //        salary_group_id = 1,
            //        formula = payrollComponent.formula,
            //        function_calling_order = payrollComponent.function_calling_order,
            //        created_by = 1,
            //        created_dt = new DateTime(2020, 1, 1),
            //        deleted_by = 1,
            //        deleted_dt = new DateTime(2020, 1, 1),
            //        is_deleted = 0,

            //    });

            //}

            _context.tbl_component_master.AddRange(component_Masters);
            _context.SaveChanges();
            _context.tbl_component_formula_details.AddRange(Formula_Details);
            _context.SaveChanges();
            //modelBuilder.Entity<tbl_component_master>().HasData(component_Masters);
            //modelBuilder.Entity<tbl_component_formula_details>().HasData(Formula_Details);

            #endregion

            #region **************** Salary Report ***************************

            //List<tbl_report_master> tbl_Report_Masters = new List<tbl_report_master>();
            //tbl_Report_Masters.Add(new tbl_report_master() { rpt_id = 1, rpt_name = "Salary Report(Arrear)", rpt_description = "salary Report with arrear", is_active = 1, created_by = 1, created_date = new DateTime(2020, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2020, 1, 1) });
            //tbl_Report_Masters.Add(new tbl_report_master() { rpt_id = 2, rpt_name = "Salary Report", rpt_description = "salary Report", is_active = 1, created_by = 1, created_date = new DateTime(2020, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2020, 1, 1) });

            //modelBuilder.Entity<tbl_report_master>().HasData(tbl_Report_Masters);



            List<tbl_rpt_title_master> tbl_rpt_title_masters = new List<tbl_rpt_title_master>();
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 1, component_id = (int)enmOtherComponent.Gender, rpt_title = "Gender", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 1, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 2, component_id = (int)enmOtherComponent.EmpFatherHusbandName, rpt_title = "Father Husband Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 2, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 3, component_id = (int)enmOtherComponent.EmpDOB, rpt_title = "Date of Birth", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 3, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 4, component_id = (int)enmOtherComponent.EmpNationality, rpt_title = "Nationality", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 4, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 5, component_id = (int)enmOtherComponent.EducationLevel, rpt_title = "Education Level", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 5, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 6, component_id = null, rpt_title = "Category Address", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 6, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 7, component_id = null, rpt_title = "Type Of Employment", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 7, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 8, component_id = (int)enmOtherComponent.EmpContact, rpt_title = "Mobile", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 8, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 9, component_id = (int)enmOtherComponent.Uan, rpt_title = "UAN Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 9, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 10, component_id = (int)enmOtherComponent.PanNo, rpt_title = "PAN No", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 10, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 11, component_id = (int)enmOtherComponent.PanName, rpt_title = "PAN Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 11, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 12, component_id = (int)enmOtherComponent.ESICNo, rpt_title = "ESIC Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 12, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 13, component_id = (int)enmOtherComponent.AdharNo, rpt_title = "Aadhar card Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 13, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 14, component_id = (int)enmOtherComponent.AdharName, rpt_title = "Aadhar name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 14, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 15, component_id = (int)enmOtherComponent.BankAccountNo, rpt_title = "Salary Account No", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 15, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 16, component_id = (int)enmOtherComponent.BankName, rpt_title = "Bank Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 16, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 17, component_id = (int)enmOtherComponent.IFSCCode, rpt_title = "Bank IFSC Code", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 17, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 18, component_id = null, rpt_title = "Address", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 18, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 19, component_id = null, rpt_title = "service book no", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 19, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 20, component_id = null, rpt_title = "Resignation date", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 20, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 21, component_id = null, rpt_title = "Last working date", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 21, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 22, component_id = null, rpt_title = "Reason Master", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 22, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 23, component_id = null, rpt_title = "mark of identification", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 23, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 24, component_id = null, rpt_title = "specimen impression", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 24, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 25, component_id = null, rpt_title = "Is Active", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 25, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 26, component_id = (int)enmOtherComponent.CompanyName, rpt_title = "Company Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 26, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 27, component_id = null, rpt_title = "Division Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 27, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 28, component_id = (int)enmOtherComponent.EmpWorkingState, rpt_title = "State Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 28, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 29, component_id = (int)enmOtherComponent.EmpLocation, rpt_title = "Branch Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 29, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 30, component_id = (int)enmOtherComponent.EmpLocation, rpt_title = "Location Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 30, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 31, component_id = (int)enmOtherComponent.EmpDepartment, rpt_title = "Department Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 31, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 32, component_id = (int)enmOtherComponent.EmpJoiningDt, rpt_title = "Date Of Joining", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 32, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 33, component_id = (int)enmOtherComponent.EmpDesignation, rpt_title = "Designation Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 33, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 34, component_id = (int)enmOtherComponent.EmpGrade, rpt_title = "Grade", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 34, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 35, component_id = (int)enmOtherComponent.PfApplicableYesNo, rpt_title = "PF Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 35, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 36, component_id = (int)enmOtherComponent.PfNo, rpt_title = "PF Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 36, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 37, component_id = (int)enmOtherComponent.PfGroup, rpt_title = "PF Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 37, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 38, component_id = (int)enmOtherComponent.pf_celling, rpt_title = "PF Ceiling", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 38, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 39, component_id = null, rpt_title = "ESIC Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 39, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 40, component_id = null, rpt_title = "PT Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 40, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 41, component_id = null, rpt_title = "PT Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 41, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 42, component_id = (int)enmOtherComponent.Vpf_applicableYesNo, rpt_title = "VPF Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 42, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 43, component_id = (int)enmOtherComponent.vpf_percentage, rpt_title = "VPF Percentage", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 43, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 44, component_id = null, rpt_title = "Is daily", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 44, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 45, component_id = null, rpt_title = "Salary On Hold", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 45, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 46, component_id = null, rpt_title = "Salary Process Type", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 46, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 47, component_id = (int)enmOtherComponent.GrossSalary, rpt_title = "Monthly Gross ", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 47, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 48, component_id = (int)enmOtherComponent.PaidDays, rpt_title = "Days Worked", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 48, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 49, component_id = (int)enmOtherComponent.ArrearDays, rpt_title = "Arrears Days", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 49, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 50, component_id = (int)enmOtherComponent.ToalPayrollDay, rpt_title = "Days In Month", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 50, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 51, component_id = (int)enmOtherComponent.Gratuity, rpt_title = "Gratuity", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 51, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 52, component_id = (int)enmOtherComponent.Leen, rpt_title = "Leen", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 52, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 53, component_id = null, rpt_title = "Notice Pay", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 53, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 54, component_id = null, rpt_title = "Pre hold  salary released", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 54, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 55, component_id = (int)enmOtherComponent.BasicSalary, rpt_title = "Basic Salary", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 55, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 56, component_id = (int)enmOtherComponent.BasicSalary, rpt_title = "Arrear Basic Salary", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 56, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 57, component_id = (int)enmOtherComponent.HRA, rpt_title = "HRA Allowance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 57, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 58, component_id = (int)enmOtherComponent.HRA, rpt_title = "Arrear HRA Allowance", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 58, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 59, component_id = (int)enmOtherComponent.Conveyance, rpt_title = "Conveyance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 59, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 60, component_id = (int)enmOtherComponent.Conveyance, rpt_title = "Arrear conveyance", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 60, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 61, component_id = (int)enmOtherComponent.Medical_Allowance, rpt_title = "Medical Allowance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 61, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 62, component_id = (int)enmOtherComponent.Medical_Allowance, rpt_title = "Arrear Medical Allowance", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 62, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 63, component_id = (int)enmOtherComponent.SPL, rpt_title = "Special Allowance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 63, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 64, component_id = (int)enmOtherComponent.SPL, rpt_title = "Arrear Special Allowance", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 64, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 65, component_id = (int)enmOtherComponent.Pf_amount, rpt_title = "PF Employee", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 65, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 66, component_id = (int)enmOtherComponent.Pf_amount, rpt_title = "Arrear PF Employee", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 66, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 67, component_id = (int)enmOtherComponent.EsicAmount, rpt_title = "ESIC Employee", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 67, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 68, component_id = (int)enmOtherComponent.EsicAmount, rpt_title = "Arrear ESIC Employee", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 68, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 69, component_id = (int)enmOtherComponent.ChildrenEducationAllowance, rpt_title = "Children Education Allowance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 69, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 70, component_id = (int)enmOtherComponent.ChildrenEducationAllowance, rpt_title = "Arrear Children Education Allowance", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 70, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 71, component_id = (int)enmOtherComponent.TotalGross, rpt_title = "Gross", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 71, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 72, component_id = (int)enmOtherComponent.TotalGross, rpt_title = "Arrear Gross", payroll_report_property = enmPayrollReportProperty.ArrearValue, display_order = 72, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 73, component_id = (int)enmOtherComponent.OT, rpt_title = "Over Time", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 73, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 74, component_id = (int)enmOtherComponent.CityCompensatoryAllowances, rpt_title = "CityCompensatoryAllowances", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 74, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 75, component_id = null, rpt_title = "OtherAllowance", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 75, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 76, component_id = (int)enmOtherComponent.TotalGross, rpt_title = "Gross Salary", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 76, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 77, component_id = (int)enmOtherComponent.Pf_amount, rpt_title = "Provident Fund", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 77, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 78, component_id = (int)enmOtherComponent.PT, rpt_title = "Professional Tax", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 78, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 79, component_id = (int)enmOtherComponent.EsicAmount, rpt_title = "ESIC", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 79, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 80, component_id = null, rpt_title = "LWF", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 80, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 81, component_id = (int)enmOtherComponent.Tax, rpt_title = "TDS", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 81, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 82, component_id = (int)enmOtherComponent.Vpf_amount, rpt_title = "Voluntary Provident Fund", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 82, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 83, component_id = null, rpt_title = "Notice Reco", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 83, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 84, component_id = (int)enmOtherComponent.OtherDeduction, rpt_title = "Other Deduction", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 84, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 85, component_id = (int)enmOtherComponent.Advance_Loan, rpt_title = "Advance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 85, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 86, component_id = (int)enmOtherComponent.Recovery, rpt_title = "Recovery", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 86, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 87, component_id = (int)enmOtherComponent.Deduction, rpt_title = "Gross Deduction", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 87, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 88, component_id = (int)enmOtherComponent.Net, rpt_title = "Net Salary", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 88, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 89, component_id = null, rpt_title = "Remarks", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 89, rpt_id = 1, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 501, component_id = (int)enmOtherComponent.Gender, rpt_title = "Gender", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 1, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 502, component_id = (int)enmOtherComponent.EmpFatherHusbandName, rpt_title = "Father Husband Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 2, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 503, component_id = (int)enmOtherComponent.EmpDOB, rpt_title = "Date of Birth", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 3, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 504, component_id = (int)enmOtherComponent.EmpNationality, rpt_title = "Nationality", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 4, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 505, component_id = (int)enmOtherComponent.EducationLevel, rpt_title = "Education Level", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 5, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 506, component_id = null, rpt_title = "Category Address", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 6, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 507, component_id = null, rpt_title = "Type Of Employment", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 7, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 508, component_id = (int)enmOtherComponent.EmpContact, rpt_title = "Mobile", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 8, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 509, component_id = (int)enmOtherComponent.Uan, rpt_title = "UAN Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 9, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 510, component_id = (int)enmOtherComponent.PanNo, rpt_title = "PAN No", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 10, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 511, component_id = (int)enmOtherComponent.PanName, rpt_title = "PAN Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 11, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 512, component_id = (int)enmOtherComponent.ESICNo, rpt_title = "ESIC Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 12, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 513, component_id = (int)enmOtherComponent.AdharNo, rpt_title = "Aadhar card Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 13, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 514, component_id = (int)enmOtherComponent.AdharName, rpt_title = "Aadhar name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 14, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 515, component_id = (int)enmOtherComponent.BankAccountNo, rpt_title = "Salary Account No", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 15, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 516, component_id = (int)enmOtherComponent.BankName, rpt_title = "Bank Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 16, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 517, component_id = (int)enmOtherComponent.IFSCCode, rpt_title = "Bank IFSC Code", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 17, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 518, component_id = null, rpt_title = "Address", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 18, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 519, component_id = null, rpt_title = "service book no", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 19, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 520, component_id = null, rpt_title = "Resignation date", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 20, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 521, component_id = null, rpt_title = "Last working date", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 21, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 522, component_id = null, rpt_title = "Reason Master", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 22, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 523, component_id = null, rpt_title = "mark of identification", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 23, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 524, component_id = null, rpt_title = "specimen impression", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 24, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 525, component_id = null, rpt_title = "Is Active", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 25, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 526, component_id = (int)enmOtherComponent.CompanyName, rpt_title = "Company Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 26, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 527, component_id = null, rpt_title = "Division Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 27, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 528, component_id = (int)enmOtherComponent.EmpWorkingState, rpt_title = "State Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 28, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 529, component_id = (int)enmOtherComponent.EmpLocation, rpt_title = "Branch Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 29, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 530, component_id = (int)enmOtherComponent.EmpLocation, rpt_title = "Location Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 30, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 531, component_id = (int)enmOtherComponent.EmpDepartment, rpt_title = "Department Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 31, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 532, component_id = (int)enmOtherComponent.EmpJoiningDt, rpt_title = "Date Of Joining", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 32, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 533, component_id = (int)enmOtherComponent.EmpDesignation, rpt_title = "Designation Name", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 33, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 534, component_id = (int)enmOtherComponent.EmpGrade, rpt_title = "Grade", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 34, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 535, component_id = (int)enmOtherComponent.PfApplicableYesNo, rpt_title = "PF Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 35, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 536, component_id = (int)enmOtherComponent.PfNo, rpt_title = "PF Number", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 36, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 537, component_id = (int)enmOtherComponent.PfGroup, rpt_title = "PF Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 37, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 538, component_id = (int)enmOtherComponent.pf_celling, rpt_title = "PF Ceiling", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 38, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 539, component_id = null, rpt_title = "ESIC Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 39, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 540, component_id = null, rpt_title = "PT Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 40, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 541, component_id = null, rpt_title = "PT Group", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 41, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 542, component_id = (int)enmOtherComponent.Vpf_applicableYesNo, rpt_title = "VPF Applicable", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 42, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 543, component_id = (int)enmOtherComponent.vpf_percentage, rpt_title = "VPF Percentage", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 43, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 544, component_id = null, rpt_title = "Is daily", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 44, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 545, component_id = null, rpt_title = "Salary On Hold", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 45, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 546, component_id = null, rpt_title = "Salary Process Type", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 46, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 547, component_id = (int)enmOtherComponent.GrossSalary, rpt_title = "Monthly Gross ", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 47, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 548, component_id = (int)enmOtherComponent.PaidDays, rpt_title = "Days Worked", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 48, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 549, component_id = (int)enmOtherComponent.ArrearDays, rpt_title = "Arrears Days", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 49, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 550, component_id = (int)enmOtherComponent.ToalPayrollDay, rpt_title = "Days In Month", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 50, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 551, component_id = (int)enmOtherComponent.Gratuity, rpt_title = "Gratuity", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 51, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 552, component_id = (int)enmOtherComponent.Leen, rpt_title = "Leen", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 52, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 553, component_id = null, rpt_title = "Notice Pay", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 53, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 554, component_id = null, rpt_title = "Pre hold  salary released", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 54, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 555, component_id = (int)enmOtherComponent.BasicSalary, rpt_title = "Basic Salary", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 55, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 556, component_id = (int)enmOtherComponent.HRA, rpt_title = "HRA Allowance", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 56, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 557, component_id = (int)enmOtherComponent.Conveyance, rpt_title = "Conveyance", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 57, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 558, component_id = (int)enmOtherComponent.Medical_Allowance, rpt_title = "Medical Allowance", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 58, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 559, component_id = (int)enmOtherComponent.SPL, rpt_title = "Special Allowance", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 59, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 560, component_id = (int)enmOtherComponent.Pf_amount, rpt_title = "PF Employee", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 60, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 561, component_id = (int)enmOtherComponent.EsicAmount, rpt_title = "ESIC Employee", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 61, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 562, component_id = (int)enmOtherComponent.ChildrenEducationAllowance, rpt_title = "Children Education Allowance", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 62, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 563, component_id = (int)enmOtherComponent.TotalGross, rpt_title = "Gross", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 63, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 564, component_id = (int)enmOtherComponent.OT, rpt_title = "Over Time", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 64, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 565, component_id = (int)enmOtherComponent.CityCompensatoryAllowances, rpt_title = "CityCompensatoryAllowances", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 65, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 566, component_id = null, rpt_title = "OtherAllowance", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 66, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 567, component_id = (int)enmOtherComponent.TotalGross, rpt_title = "Gross Salary", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 67, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 568, component_id = (int)enmOtherComponent.Pf_amount, rpt_title = "Provident Fund", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 68, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 569, component_id = (int)enmOtherComponent.PT, rpt_title = "Professional Tax", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 69, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 570, component_id = (int)enmOtherComponent.EsicAmount, rpt_title = "ESIC", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 70, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 571, component_id = null, rpt_title = "LWF", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 71, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 572, component_id = (int)enmOtherComponent.Tax, rpt_title = "TDS", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 72, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 573, component_id = (int)enmOtherComponent.Vpf_amount, rpt_title = "Voluntary Provident Fund", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 73, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 574, component_id = null, rpt_title = "Notice Reco", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 74, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 575, component_id = (int)enmOtherComponent.OtherDeduction, rpt_title = "Other Deduction", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 75, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 576, component_id = (int)enmOtherComponent.Advance_Loan, rpt_title = "Advance", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 76, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 577, component_id = (int)enmOtherComponent.Recovery, rpt_title = "Recovery", payroll_report_property = enmPayrollReportProperty.CurrentMonthValue, display_order = 77, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 578, component_id = (int)enmOtherComponent.Deduction, rpt_title = "Gross Deduction", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 78, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 579, component_id = (int)enmOtherComponent.Net, rpt_title = "Net Salary", payroll_report_property = enmPayrollReportProperty.TotalValue, display_order = 79, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });
            tbl_rpt_title_masters.Add(new tbl_rpt_title_master() { title_id = 580, component_id = null, rpt_title = "Remarks", payroll_report_property = enmPayrollReportProperty.NormalValue, display_order = 80, rpt_id = 2, created_by = 1, created_date = new DateTime(2000, 1, 1), last_modified_by = 1, last_modified_date = new DateTime(2000, 1, 1), is_active = 1 });

            _context.tbl_rpt_title_master.AddRange(tbl_rpt_title_masters);
            _context.SaveChanges();
            return "Save data";
            #endregion
            //modelBuilder.Entity<tbl_rpt_title_master>().HasData(tbl_rpt_title_masters);

        }

    }

#endif
}
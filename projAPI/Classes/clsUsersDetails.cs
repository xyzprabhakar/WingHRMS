using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using projAPI.Model;
using projContext.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Classes
{

    public class clsCurrentUser
    {
        public int EmpId { get; set; }
        public int UserId { get; set; }
        public int Is_Hod { get; set; }
        public int UserDepID { get; set; }
        public string UserDepName { get; set; }
        public bool Is_SuperAdmin { get; set; }
        public bool Is_HRAdmin { get; set; }

        public List<int> CompanyId { get; set; }
        public List<int> RoleId { get; set; }
        public List<int> DownlineEmpId { get; set; }
        public string is_application_freezed { get; set; }
    }


    public class clsUsersDetails
    {
        int _LoginUserId = 0;
        IConfiguration _config;
        projContext.Context _context;
        public int _UserId;
        public tbl_user_master _tbl_user_master { get; set; }
        public tbl_employee_master _tbl_employee_master { get; set; }
        public List<tbl_menu_master> _tbl_menu_master { get; set; }
        public List<int> _tbl_role_menu_master { get; set; }

        public int _is_hod { get; set; }
        public int is_mobile_access { get; set; }
        enum UserLogType : byte
        {
            ChangePasswordSelf = 1,
            ChangePasswordAdmin = 2,
            LoginBlockByWrongAttempt = 11,
            LoginBlockByAdmin = 12,
            UserBlockByAdmin = 13,
            LoginUnblockByAdmin = 21,
            UserUnBlockByAdmin = 22
        }



        public clsUsersDetails(projContext.Context context, IConfiguration config, int UserID, int LoginUserId)
        {
            _context = context;
            _config = config;
            _LoginUserId = LoginUserId;
            LoadUser(UserID);
        }
        public clsUsersDetails(projContext.Context context, IConfiguration config, string UserName, string Password, int LoginUserId)
        {
            _context = context;
            _config = config;
            _LoginUserId = LoginUserId;
            LoadUser(_UserId, UserName, Password);
        }

        private void LoadUser(int UserID, string UserName = "", string Password = "")
        {
            if (UserID > 0)
            {
                _tbl_user_master = _context.tbl_user_master.FirstOrDefault(p => p.user_id == UserID && p.is_active == 1);

            }
            else
            {
                //_tbl_user_master = _context.tbl_user_master.FirstOrDefault(p => p.username == UserName && p.is_active == 1 );
                _tbl_user_master = _context.tbl_user_master.FirstOrDefault(p => p.username.Trim().ToUpper() == UserName.Trim().ToUpper());

            }
            if (_tbl_user_master != null)
            {
                _tbl_employee_master = _context.tbl_employee_master.FirstOrDefault(p => p.employee_id == _tbl_user_master.employee_id);
                _UserId = _tbl_user_master.user_id;
                if (_LoginUserId == 0)
                {
                    _LoginUserId = _UserId;
                }
            }


        }

        public bool ValidateCaptcha(string guid, string captcha)
        {

            var get_captcha_code = _context.tbl_captcha_code_details.FirstOrDefault(a => a.guid == guid);
            if (get_captcha_code == null)
            {
                return false;
            }
            if (get_captcha_code.captcha_code != captcha)
            {
                return false;
            }
            //delete the captcha which is genrated on days before
            _context.AttachRange(get_captcha_code);
            _context.Entry(get_captcha_code).State = EntityState.Deleted;
            _context.SaveChanges();
            return true;
        }

        public int BlockUserIdAfterFailAttempt()
        {
            int WrongAttempt = 0;
            int BlockUserAfterLoginFailAttempets = Convert.ToInt32(_config["UserSetting:BlockUserAfterLoginFailAttempets"]);
            DateTime fromdt = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));
            DateTime toDt = fromdt.AddDays(1);
            List<tbl_user_login_logs> tbl_User_Login_Logs = _context.tbl_user_login_logs.Where(p => p.user_id == _UserId && p.login_date_time >= fromdt && p.login_date_time <= toDt).OrderByDescending(p => p.login_date_time).Take(BlockUserAfterLoginFailAttempets).ToList();
            if (tbl_User_Login_Logs.Count >= BlockUserAfterLoginFailAttempets)
            {
                if (!tbl_User_Login_Logs.Any(p => p.is_wrong_attempt == 0))
                {
                    Block_UnBlock_UserId(1);
                }
                else
                {
                    var login_date_time = tbl_User_Login_Logs.Where(p => p.is_wrong_attempt == 0).FirstOrDefault();
                    if (login_date_time == null)
                    {
                        WrongAttempt = tbl_User_Login_Logs.Count;
                    }
                    else
                    {
                        WrongAttempt = tbl_User_Login_Logs.Where(p => p.login_date_time > login_date_time.login_date_time).Count();
                    }
                }
            }
            else
            {
                var login_date_time = tbl_User_Login_Logs.Where(p => p.is_wrong_attempt == 0).FirstOrDefault();
                if (login_date_time == null)
                {
                    WrongAttempt = tbl_User_Login_Logs.Count;
                }
                else
                {
                    WrongAttempt = tbl_User_Login_Logs.Where(p => p.login_date_time > login_date_time.login_date_time).Count();
                }
            }
            return WrongAttempt;
        }

        /// <summary>
        /// This Will check is Ip Block or not 
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="UserAgent"></param>
        /// <returns></returns>


        public bool Active_InActive_UserId(byte is_active, byte isadmin = 0)
        {
            //if (isadmin == 1)
            //{
            //    _tbl_user_master.is_active = is_block;
            //    if (is_block == 0)              
            //        insert_into_logs((byte)UserLogType.UserUnBlockByAdmin, "");              
            //    else                
            //        insert_into_logs((byte)UserLogType.UserBlockByAdmin, "");

            //}
            _tbl_user_master.is_active = is_active;
            _tbl_user_master.last_modified_by = _LoginUserId;
            _tbl_user_master.last_modified_date = DateTime.Now;
            _tbl_user_master.logged_blocked_dt = is_active == 0 ? Convert.ToDateTime("1-jan-2000") : DateTime.Now;
            _context.Attach(_tbl_user_master);
            _context.Entry(_tbl_user_master).State = EntityState.Modified;
            _context.SaveChanges();
            _tbl_employee_master.is_active = is_active;
            _tbl_employee_master.last_modified_by = _LoginUserId;
            _tbl_employee_master.last_modified_date = DateTime.Now;
            _context.Attach(_tbl_employee_master);
            _context.Entry(_tbl_employee_master).State = EntityState.Modified;
            _context.SaveChanges();
            if (is_active == 1)
            {
                insert_into_logs((byte)UserLogType.UserUnBlockByAdmin, "");
            }
            else
            {
                insert_into_logs((byte)UserLogType.UserBlockByAdmin, "");
                //if (_LoginUserId != _UserId)
                //{
                //    insert_into_logs((byte)UserLogType.UserBlockByAdmin, "");
                //}
                //else
                //{
                //    insert_into_logs((byte)UserLogType.UserBlockByAdmin, "");
                //}
            }

            return true;
        }



        public bool IsIPBlock(string IP, string UserAgent)
        {
            int BlockUserAfterLoginFailAttempets = Convert.ToInt32(_config["UserSetting:AfterFailAttemptBlockIp"]);
            int FailedTime = Convert.ToInt32(_config["UserSetting:AfterFailAttemptBlockIpForTime"]);
            DateTime fromdt = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));
            DateTime toDt = fromdt.AddDays(1);
            List<tbl_user_login_logs> tbl_User_Login_Logs = _context.tbl_user_login_logs.Where(p => p.login_date_time >= fromdt && p.login_date_time <= toDt && p.login_ip == IP && p.user_agent == UserAgent).OrderByDescending(p => p.login_date_time).Take(BlockUserAfterLoginFailAttempets).ToList();
            if (tbl_User_Login_Logs.Count >= BlockUserAfterLoginFailAttempets)
            {
                if (tbl_User_Login_Logs.Any(p => p.is_wrong_attempt == 0))
                {
                    return false;
                }
                var tll = tbl_User_Login_Logs.FirstOrDefault();
                if (DateTime.Compare(DateTime.Now, tll.login_date_time.AddMinutes(FailedTime)) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public bool IsLoginBlock()
        {
            if (_tbl_user_master == null)
            {
                return true;
            }
            int FailedTime = Convert.ToInt32(_config["UserSetting:BlockUserAfterLoginFailAttempetsForTime"]);
            if (_tbl_user_master.is_logged_blocked == 1)
            //&& DateTime.Compare(DateTime.Now, _tbl_user_master.logged_blocked_dt.AddMinutes(FailedTime)) > 0)
            {
                return true;
            }
            return false;
        }

        public bool Block_UnBlock_UserId(byte is_block, byte isadmin = 0)
        {
            //if (isadmin == 1)
            //{
            //    _tbl_user_master.is_active = is_block;
            //    if (is_block == 0)              
            //        insert_into_logs((byte)UserLogType.UserUnBlockByAdmin, "");              
            //    else                
            //        insert_into_logs((byte)UserLogType.UserBlockByAdmin, "");

            //}
            _tbl_user_master.is_logged_blocked = is_block;
            _tbl_user_master.last_modified_by = _LoginUserId;
            _tbl_user_master.last_modified_date = DateTime.Now;
            _tbl_user_master.logged_blocked_dt = is_block == 0 ? Convert.ToDateTime("1-jan-2000") : DateTime.Now;
            _context.Attach(_tbl_user_master);
            _context.Entry(_tbl_user_master).State = EntityState.Modified;
            _context.SaveChanges();
            if (is_block == 0)
            {
                insert_into_logs((byte)UserLogType.LoginUnblockByAdmin, "");
            }
            else
            {
                if (_LoginUserId != _UserId)
                {
                    insert_into_logs((byte)UserLogType.LoginBlockByAdmin, "");
                }
                else
                {
                    insert_into_logs((byte)UserLogType.LoginBlockByWrongAttempt, "");
                }
            }

            return true;
        }
        /// <summary>
        /// Change the Password  of a User
        /// </summary>
        /// <param name="NewPassword"></param>
        /// <returns></returns>
        public bool ChangePassword(string NewPassword)
        {
            string EncryptedPass = AESEncrytDecry.EncryptStringAES(NewPassword);

            if (EncryptedPass == _tbl_user_master.password)
            {
                throw new Exception("New Password and Old Password could not be same");
            }
            _tbl_user_master.password = EncryptedPass;
            _tbl_user_master.last_modified_by = _LoginUserId;
            _tbl_user_master.last_modified_date = DateTime.Now;
            _tbl_user_master.logged_blocked_dt = DateTime.Now;
            _context.Attach(_tbl_user_master);
            _context.Entry(_tbl_user_master).State = EntityState.Modified;
            _context.SaveChanges();
            if (_LoginUserId != _UserId)
            {
                insert_into_logs((byte)UserLogType.ChangePasswordAdmin, "");
            }
            else
            {
                insert_into_logs((byte)UserLogType.ChangePasswordSelf, "");
            }

            return true;
        }

        public bool IsAlreadyLogedin(string IP, string UserAgent)
        {
            int SessionTime = Convert.ToInt32(_config["UserSetting:SessionTime"]);
            if (_tbl_user_master == null)
            {
                return false;
            }
            if (_tbl_user_master.is_logged_in == 1)//In case of User already login
            {
                if (DateTime.Compare(DateTime.Now, _tbl_user_master.last_logged_dt.AddMinutes(SessionTime)) <= 0)
                {
                    //check the user agent of last IP Address
                    tbl_user_login_logs tull = _context.tbl_user_login_logs.Where(p => p.user_id == _UserId && p.is_wrong_attempt == 0).OrderByDescending(p => p.login_date_time).FirstOrDefault();
                    if (tull != null)
                    {
                        if (tull.login_ip.Trim().ToLower().Equals(IP.Trim().ToLower()) && tull.user_agent.Trim().ToLower().Equals(UserAgent.Trim().ToLower()))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void LoadEmpSpecificDetail(mdlLoginOutput mdlLoginOutput)
        {
            int EmpID = 0;
            if (_tbl_user_master != null)
            {
                if (_tbl_user_master.employee_id != null)
                {
                    EmpID = _tbl_user_master.employee_id ?? 0;
                    var tempdata = _context.tbl_emp_officaial_sec.Where(p => p.employee_id == EmpID && p.is_deleted == 0).FirstOrDefault();
                    if (tempdata != null)
                    {
                        mdlLoginOutput.employee_first_name = tempdata.employee_first_name;
                        mdlLoginOutput.employee_middle_name = tempdata.employee_middle_name;
                        mdlLoginOutput.employee_last_name = tempdata.employee_last_name;
                        mdlLoginOutput.default_company = _tbl_user_master.default_company_id;
                        mdlLoginOutput.employee_photo_path = Convert.ToString(_config["domain_url"]) + (string.IsNullOrEmpty(tempdata.employee_photo_path) ? "/EmployeeImage/DefaultUser/defaultimage.jpg" : tempdata.employee_photo_path);
                        mdlLoginOutput._appSetting_domainn = Convert.ToString(_config["domain_url"]);
                        mdlLoginOutput.is_mobile_access = tempdata.is_mobile_access;
                        mdlLoginOutput.is_mobile_attendence_access = tempdata.is_mobile_attendence_access;
                        mdlLoginOutput.user_Dep_id = tempdata.department_id ?? 0;
                        if (tempdata.department_id != null)
                        {
                            mdlLoginOutput.user_Dep_name = _context.tbl_department_master.Where(x => x.department_id == tempdata.department_id).FirstOrDefault().department_name.Trim();
                        }
                        else
                        {
                            mdlLoginOutput.user_Dep_name = "";

                        }

                    }
                    // Now Load if Emp is Role
                    mdlLoginOutput.employee_role_id = _context.tbl_user_role_map.Where(p => p.user_id == _UserId && p.is_deleted == 0).Select(p => (int)p.role_id).ToList();

                }
            }

        }

        public void LoadEmpDownlineEmployee(mdlLoginOutput mdlLoginOutput)
        {
            int EmpID = 0;
            if (_tbl_user_master != null)
            {
                if (_tbl_user_master.employee_id != null)
                {
                    EmpID = _tbl_user_master.employee_id ?? 0;

                    var tempdata = _context.tbl_emp_manager.Where(a => (a.m_one_id == EmpID || a.m_two_id == EmpID || a.m_three_id == EmpID) && a.is_deleted == 0).ToList();
                    mdlLoginOutput.manager_emp_list = tempdata.Where(p => p.employee_id != null).Select(p => p.employee_id.Value).Distinct().ToList();
                }
            }
        }



        public bool LoggedOut()
        {
            _tbl_user_master.is_logged_in = 0;
            _context.Attach(_tbl_user_master);
            _context.Entry(_tbl_user_master).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        public bool LoginLog(string IP, string UserAgent, byte IsWrongAttempt = 0)
        {
            tbl_user_login_logs tull = new tbl_user_login_logs()
            {
                login_ip = IP,
                user_agent = UserAgent,
                is_wrong_attempt = IsWrongAttempt,
                login_date_time = DateTime.Now,

            };
            if (_UserId > 0)
            {
                tull.user_id = _UserId;
                if (IsWrongAttempt == 0)
                {
                    _tbl_user_master.is_logged_in = 1;
                    _tbl_user_master.last_logged_dt = DateTime.Now;
                    _context.tbl_user_master.Attach(_tbl_user_master);
                    _context.Entry(_tbl_user_master).State = EntityState.Modified;

                }
            }


            _context.tbl_user_login_logs.Add(tull);
            _context.SaveChanges();
            return true;
        }

        private bool insert_into_logs(byte transaction_type, string Remarks)
        {
            tbl_active_inactive_user_log tbl = new tbl_active_inactive_user_log();
            if (_UserId > 0)
            {
                tbl.user_id = _UserId;
            }
            tbl.transaction_type = transaction_type;
            tbl.remarks = Convert.ToString(Remarks).Trim();
            tbl.created_by = _LoginUserId;
            tbl.modified_by = _LoginUserId;
            tbl.is_deleted = 0;
            tbl.modified_date = tbl.created_on = DateTime.Now;
            _context.tbl_active_inactive_user_log.Add(tbl);
            _context.SaveChanges();
            return true;
        }

        public void LoadCompanyData(mdlLoginOutput objlogin)
        {
            try
            {

                var company_data = _context.tbl_company_master.Where(a => a.is_active == 1 && a.company_id == objlogin.default_company).Select(a => new { a.company_name, a.company_logo }).FirstOrDefault();
                if (company_data != null)
                {
                    objlogin.company_name = company_data.company_name;
                    objlogin.company_logo = company_data.company_logo;
                }

            }
            catch (Exception ex)
            {
            }
        }

        public List<TreeViewNode> LoadUserMenu(List<int> roleids, int is_hod)
        {
            _is_hod = is_hod;
            _tbl_menu_master = _context.tbl_menu_master.Where(p => p.is_active == 1).ToList();
            _tbl_role_menu_master = _context.tbl_role_menu_master.Where(p => roleids.Contains((int)p.role_id)).Select(p => (int)p.menu_id).Distinct().ToList();
            return getComponentTree(0);


        }




        public List<TreeViewNode> getComponentTree(int menu_id)
        {
            List<TreeViewNode> treeViewNodes = null;
            treeViewNodes = _tbl_menu_master.
                Where(p => p.parent_menu_id == menu_id && p.is_active == 1 && _tbl_role_menu_master.Contains((int)p.menu_id))
                .Select(p => new TreeViewNode { id = (int)p.menu_id, text = p.menu_name, Urll = p.urll, icon_url = p.IconUrl, type = p.type, parent_id = p.parent_menu_id ?? 0 }).ToList();
            foreach (var treeViewNode in treeViewNodes)
            {
                //if (_is_hod != 2 && treeViewNode.Urll == "Dashboard")
                //{
                //    treeViewNode.Urll = "View/Dashboard";
                //}
                treeViewNode.children = getComponentTree(treeViewNode.id);
            }
            return treeViewNodes;
        }

        public List<object> Get_emp_company_lst(int emp_idd, mdlLoginOutput mdl = null)
        {
            List<object> emp_comp_lst = new List<object>();

            var data_ = _context.tbl_employee_company_map.Where(x => x.employee_id == emp_idd && x.is_deleted == 0).Select(p => new { p.company_id, p.tbl_company_master.company_name,p.is_default }).ToList();
            if (mdl != null)
            {
                mdl.company_list = data_.Select(p => p.company_id).ToArray();
            }
            if (data_.Count > 0)
            {
                for (int i = 0; i < data_.Count; i++)
                {
                    emp_comp_lst.Add(new
                    {
                        company_id = data_[i].company_id,
                        company_name = data_[i].company_name
                    });
                }
                mdl.default_company = data_.Where(P => P.is_default).FirstOrDefault().company_id ?? 1;

            }

            return emp_comp_lst;

        }

        public List<EmployeeList> Get_Emp_dtl_under_login_emp(mdlLoginOutput mdl_output, clsEmployeeDetail _clsEmployeeDetail)
        {
            List<EmployeeList> emp_off_dtl = new List<EmployeeList>();
            foreach (var com in mdl_output.company_list)
            {
                emp_off_dtl.AddRange(_clsEmployeeDetail.GetEmployeeByDate(com.Value, new DateTime(1900, 1, 1), DateTime.Now, 2)
                    .Select(p => new EmployeeList
                    {
                        company_id = p.company_id,
                        company_name = p.company_name,
                        dept_id = p.dept_id,
                        dept_name = p.dept_name,
                        emp_code = p.emp_code,
                        emp_name = p.emp_name,
                        emp_name_code = p.emp_code + " - " + p.emp_name,
                        location_id = p.location_id,
                        location_name = p.location_name,
                        state_id = p.state_id,
                        state_name = p.state_name,
                        emp_status = p.emp_status,
                        _empid = p.employee_id
                    }));
                ;
            }
            return emp_off_dtl;
        }

        public List<EmployeeList> Get_Emp_dtl_under_login_emp_Active_Inactive(mdlLoginOutput mdl_output, clsEmployeeDetail _clsEmployeeDetail)
        {
            List<EmployeeList> emp_off_dtl = new List<EmployeeList>();
            foreach (var com in mdl_output.company_list)
            {
                emp_off_dtl.AddRange(_clsEmployeeDetail.GetEmployeeByDate_Active_Inactive(com.Value, new DateTime(1900, 1, 1), DateTime.Now, 2)
                    .Select(p => new EmployeeList
                    {
                        company_id = p.company_id,
                        company_name = p.company_name,
                        dept_id = p.dept_id,
                        dept_name = p.dept_name,
                        emp_code = p.emp_code,
                        emp_name = p.emp_name,
                        emp_name_code = p.emp_code + " - " + p.emp_name,
                        location_id = p.location_id,
                        location_name = p.location_name,
                        state_id = p.state_id,
                        state_name = p.state_name,
                        emp_status = p.emp_status,
                        _empid = p.employee_id
                    }));
                ;
            }
            return emp_off_dtl;
        }

        #region Get employee list for directory/birthday/aniversary created by Anil kumar on 2 dec 2020
        public List<EmployeeList> Get_Emp_dtl_for_dir(clsEmployeeDetail _clsEmployeeDetail)
        {
            List<EmployeeList> emp_off_dtl = new List<EmployeeList>();

            emp_off_dtl.AddRange(_clsEmployeeDetail.GetEmployee_list_dir()
                .Select(p => new EmployeeList
                {
                    company_id = p.company_id,
                    company_name = p.company_name,
                    dept_id = p.dept_id,
                    dept_name = p.dept_name,
                    emp_code = p.emp_code,
                    emp_name = p.emp_name,
                    emp_status = p.emp_status,
                    emp_name_code = p.emp_code + " - " + p.emp_name,
                    location_id = p.location_id,
                    location_name = p.location_name,
                    state_id = p.state_id,
                    state_name = p.state_name,
                    _empid = p.employee_id,
                    mobileno = p.mobileno,
                    email = p.email,
                    desig_name = p.desig_name,
                    emp_img = p.emp_img,
                    official_contact_no=p.official_contact_no,
                    Official_email_id=p.Official_email_id
                }));
            ;

            return emp_off_dtl;
        }
        public List<EmployeeList> Get_Emp_dtl_for_birthday(clsEmployeeDetail _clsEmployeeDetail)
        {
            List<EmployeeList> emp_off_dtl = new List<EmployeeList>();

            emp_off_dtl.AddRange(_clsEmployeeDetail.GetEmployee_list_birthday()
                .Select(p => new EmployeeList
                {
                    company_id = p.company_id,
                    company_name = p.company_name,
                    dept_id = p.dept_id,
                    dept_name = p.dept_name,
                    emp_code = p.emp_code,
                    emp_name = p.emp_name,
                    emp_name_code = p.emp_code + " - " + p.emp_name,
                    location_id = p.location_id,
                    location_name = p.location_name,
                    state_id = p.state_id,
                    state_name = p.state_name,
                    _empid = p.employee_id,
                    mobileno = p.mobileno,
                    email = p.email,
                    desig_name = p.desig_name,
                    emp_img = p.emp_img,
                    dob = p.dob// Convert.ToDateTime(p.dob).ToString("dd-MMM"),


                }));
            ;

            return emp_off_dtl;
        }
        public List<EmployeeList> Get_Emp_dtl_for_aniversary(clsEmployeeDetail _clsEmployeeDetail)
        {
            List<EmployeeList> emp_off_dtl = new List<EmployeeList>();

            emp_off_dtl.AddRange(_clsEmployeeDetail.GetEmployee_list_aniversary()
                .Select(p => new EmployeeList
                {
                    company_id = p.company_id,
                    company_name = p.company_name,
                    dept_id = p.dept_id,
                    dept_name = p.dept_name,
                    emp_code = p.emp_code,
                    emp_name = p.emp_name,
                    emp_name_code = p.emp_code + " - " + p.emp_name,
                    location_id = p.location_id,
                    location_name = p.location_name,
                    state_id = p.state_id,
                    state_name = p.state_name,
                    _empid = p.employee_id,
                    mobileno = p.mobileno,
                    email = p.email,
                    desig_name = p.desig_name,
                    emp_img = p.emp_img,
                    doanv = p.doanv// Convert.ToDateTime(p.doanv).ToString("dd-MMM")
                }));
            ;

            return emp_off_dtl;
        }
        #endregion

    }
}

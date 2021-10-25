using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Classes
{
#if(false)
    public class clsLoginEmpDtl
    {
        IConfiguration _config;
        projContext.Context _context;
        public int _emp_id, _roleid;
        public int _emp_perform_action;
        public string _action_name;
        private readonly IHttpContextAccessor _AC;
        public int is_manager, is_Admin;


        public clsLoginEmpDtl(projContext.Context context, int emp_perform_action, string action_name, IHttpContextAccessor AC)
        {
            _context = context;
            _AC = AC;
            _emp_perform_action = emp_perform_action;
            _action_name = action_name;
            var temp_emp = _AC.HttpContext.User.Claims.Where(p => p.Type == "empid").FirstOrDefault();
            if (temp_emp != null)
            {
                _emp_id = Convert.ToInt32(temp_emp.Value);
            }
            var temp_role = _AC.HttpContext.User.Claims.Where(p => p.Type == "roleid").FirstOrDefault();
            if (temp_role != null)
            {
                _roleid = Convert.ToInt32(temp_role.Value);
            }
        }

        private void fnc_is_managerr()
        {
            throw new NotImplementedException();
#if false
            var is_manager = _context.tbl_emp_manager.Where(x => x.is_deleted == 0 && x.employee_id == _emp_perform_action).FirstOrDefault();
            //if login employee is manager
            if (is_manager != null)
            {
                if (is_manager.m_one_id == _emp_id || is_manager.m_two_id == _emp_id || is_manager.m_three_id == _emp_id)
                {
                    this.is_manager = 1;
                }
            }
#endif
        }

        public bool is_valid()
        {
            if (_emp_id == _emp_perform_action)// if it is a user want to see report
            {
                return true;
            }
            fnc_is_managerr();
            if (is_manager == 1 && _action_name.ToLower() == "read")// if it is a manager and want to see only report but manager cant raised request on behalf of user
            {
                return true;
            }
            fnc_is_Admin();
            if (is_Admin == 1 && _action_name.ToLower() == "read")// if it is a manager and want to see only report but manager cant raised request on behalf of user
            {
                return true;
            }
            return false;
        }

        private void fnc_is_Admin()
        {

            //string actionName = "Is Company Admin";
            //if (!this._action_name.Trim().ToLower().Equals("read"))
            //{
            //    actionName = this._action_name.Trim();
            //}
            //var _tbl_menu_master = _context.tbl_menu_master.Where(p => p.is_active == 1).ToList();


            //var temp_menu = _tbl_menu_master.Where(p => p.menu_name == actionName).FirstOrDefault();
            //if (temp_menu != null)
            //{
            //    var assign_menu_data = _context.tbl_role_menu_master.Where(a => a.role_id == _roleid).FirstOrDefault();
            //    if (assign_menu_data != null)
            //    {
            //        var tempdata = assign_menu_data.menu_id.Split(',');
            //        if (tempdata.Contains(temp_menu.menu_id.ToString()))
            //        {
            //            is_Admin = 1;
            //        }
            //    }
            //}
        }
    }
#endif
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projContext;

namespace projAPI.Services
{



    public class srvEmployee
    {



        private readonly Context _context;

        public srvEmployee(Context context)
        {
            _context = context;
        }
        private int _EmpId,_UserId,_DefaultCompany;
        private List<int> _Role, _EmpCompany,_DownlineEmpId;
        public int EmpId { get { return _EmpId; } set { _EmpId = value; } }
        public int UserId { get { return _UserId; } set { _UserId = value; } }
        public int DefaultCompany { get { return _DefaultCompany; } set { _DefaultCompany = value; } }
        public List<int> Role { get { return _Role; } set { _Role = value; } }
        public List<int> EmpCompany { get { return _EmpCompany; } set { _EmpCompany = value; } }
        public List<int> DownlineEmpId { get { return _DownlineEmpId; } set { _DownlineEmpId = value; } }

        public void LoadRole()
        {
            _Role = _context.tbl_user_role_map.Where(p => p.user_id == _UserId).Select(p => p.role_id??0).ToList();
        }


        public void LoadEmpCompany()
        {
            var EmpList=_context.tbl_employee_company_map.Where(p => p.employee_id== _UserId && p.is_deleted==0).ToList();
            _EmpCompany=EmpList.Select(p => p.company_id??0).ToList();            
            _DefaultCompany = EmpList.Where(p => p.is_default).FirstOrDefault()?.company_id ?? 0;
            if (_EmpCompany == null)
            {
                _EmpCompany = new List<int>();
            }
            if (_Role == null)
            {
                LoadRole();
            }
            if (_Role.Where(p => p == (int)enmRoleMaster.SuperAdmin || p == (int)enmRoleMaster.HRAdmin).Any())
            {
                _EmpCompany = _context.tbl_company_master.Select(p => p.company_id).ToList();
            }
        }

        public void GetDownline()
        {
            if (_EmpCompany==null)
            {
                LoadEmpCompany();
            }
            _context.tbl_emp_manager.Where(p => p.m_one_id == _EmpId && p.is_deleted == 0);//.Select();
           

        }



        #region
        public class mdlManagerId
        { 
        }

        public class mdlEmployeeCurrentStatus
        {
            public int EmpId { get; set; }
         
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projAPI.Model;
using projContext;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace projAPI.Services
{
    public interface IsrvEmployee
    {
        int DefaultCompany { get; set; }
        List<int> DownlineEmpId { get; set; }
        List<int> EmpCompany { get; set; }
        int EmpId { get; set; }
        List<int> Role { get; set; }
        int UserId { get; set; }

        string GenrateEmpCode(int companyId);
        List<mdlEmployee> GetDownline(DateTime processingDate, bool OnlyActive, bool OnlyConfirmed, bool IsRequiredName);
        bool IsActiveEmpExistsById(int EmpId);
        void LoadEmpCompany();
    }

    public class srvEmployee : IsrvEmployee
    {

        private readonly Context _context;
        private readonly IsrvSettings _IsrvSettings;
        private readonly IConfiguration _config;
        public srvEmployee(Context context, IsrvSettings isrvSettings,  IConfiguration config)
        {
            _context = context;
            _IsrvSettings = isrvSettings;
            _config=config;
        }
        private int _EmpId, _UserId, _DefaultCompany;
        private List<int> _Role, _EmpCompany, _DownlineEmpId;
        public int EmpId { get { return _EmpId; } set { _EmpId = value; } }
        public int UserId { get { return _UserId; } set { _UserId = value; } }
        public int DefaultCompany { get { return _DefaultCompany; } set { _DefaultCompany = value; } }
        public List<int> Role { get { return _Role; } set { _Role = value; } }
        public List<int> EmpCompany { get { return _EmpCompany; } set { _EmpCompany = value; } }
        public List<int> DownlineEmpId { get { return _DownlineEmpId; } set { _DownlineEmpId = value; } }


        public void LoadEmpCompany()
        {
            var EmpList = _context.tbl_employee_company_map.Where(p => p.employee_id == _EmpId && p.is_deleted == 0).ToList();
            _EmpCompany = EmpList.Select(p => p.company_id ?? 0).ToList();
            _DefaultCompany = EmpList.Where(p => p.is_default).FirstOrDefault()?.company_id ?? 0;
            if (_EmpCompany == null)
            {
                _EmpCompany = new List<int>();
            }
            if (_Role == null)
            {
                throw new Exception("Roles not Defined");
            }
            if (_Role.Where(p => p == (int)enmRoleMaster.SuperAdmin || p == (int)enmRoleMaster.HRAdmin).Any())
            {
                _EmpCompany = _context.tbl_company_master.Select(p => p.company_id).ToList();
            }
        }

        public List<mdlEmployee> GetDownline(DateTime processingDate, bool OnlyActive, bool OnlyConfirmed, bool IsRequiredName)
        {
            List<mdlEmployee> AllDownline = new List<mdlEmployee>();
            if (_EmpCompany == null)
            {
                LoadEmpCompany();
            }
            int ManagerDepth = 1;
            int.TryParse(_IsrvSettings.GetSettings("ReportingManagers", "Depth"), out ManagerDepth);
            using (var mySqlConnection = new MySqlConnection(_config.GetConnectionString("HRMS")))
            {
                MySqlCommand mySqlCommand = new MySqlCommand("proc_emp_down_line");
                mySqlCommand.Connection = mySqlConnection;
                mySqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlParameter proc_type = new MySqlParameter("_proc_type", MySqlDbType.Int32);
                MySqlParameter companyId = new MySqlParameter("_companyId", MySqlDbType.Int32);
                MySqlParameter currentEmpId = new MySqlParameter("_currentEmpId", MySqlDbType.Int32);
                MySqlParameter currentUserId = new MySqlParameter("_currentUserId", MySqlDbType.Int32);
                MySqlParameter prcessDate = new MySqlParameter("_prcessDate", MySqlDbType.DateTime);
                MySqlParameter IsOnlyActive = new MySqlParameter("_IsOnlyActive", MySqlDbType.Bool);
                MySqlParameter IsOnlyConfirmed = new MySqlParameter("_IsOnlyConfirmed", MySqlDbType.Bool);
                MySqlParameter managerDepth = new MySqlParameter("_managerDepth", MySqlDbType.Int32);

                mySqlCommand.Parameters.Add(proc_type); proc_type.Value = 1;
                mySqlCommand.Parameters.Add(companyId);
                mySqlCommand.Parameters.Add(currentEmpId); currentEmpId.Value = _EmpId;
                mySqlCommand.Parameters.Add(currentUserId); currentUserId.Value = _UserId;
                mySqlCommand.Parameters.Add(prcessDate); prcessDate.Value = processingDate.ToString("yyyy-MM-dd HH:mm");
                mySqlCommand.Parameters.Add(IsOnlyActive); IsOnlyActive.Value = OnlyActive;
                mySqlCommand.Parameters.Add(managerDepth); managerDepth.Value = ManagerDepth;
                mySqlCommand.Parameters.Add(IsOnlyConfirmed); IsOnlyConfirmed.Value = OnlyConfirmed;
                mySqlConnection.Open();

                int empId_, company_id_, state_id_, location_id_, dept_id_, isActive_, empstatus_, depthLevel_;
                foreach (var _com in _EmpCompany)
                {
                    companyId.Value = _com;
                    using (var Result = mySqlCommand.ExecuteReader())
                    {
                        while (Result.Read())
                        {
                            int.TryParse(Convert.ToString(Result["EmpId"]), out empId_);
                            int.TryParse(Convert.ToString(Result["company_id"]), out company_id_);
                            int.TryParse(Convert.ToString(Result["state_id"]), out state_id_);
                            int.TryParse(Convert.ToString(Result["location_id"]), out location_id_);
                            int.TryParse(Convert.ToString(Result["dept_id"]), out dept_id_);
                            int.TryParse(Convert.ToString(Result["isActive"]), out isActive_);
                            int.TryParse(Convert.ToString(Result["empstatus"]), out empstatus_);
                            int.TryParse(Convert.ToString(Result["depthLevel"]), out depthLevel_);
                            AllDownline.Add(new mdlEmployee()
                            {
                                empId = _EmpId,
                                empCode = Convert.ToString(Result["EmpCode"]),
                                empName = Convert.ToString(Result["EmpName"]),
                                company_id = company_id_,
                                dept_id = dept_id_,
                                depthLevel = depthLevel_,
                                empstatus = empstatus_,
                                isActive = isActive_,
                                location_id = location_id_,
                                state_id = state_id_
                            });
                        }
                    }
                }
                mySqlConnection.Close();
            }

            return AllDownline;
        }


        public string GenrateEmpCode(int companyId)
        {
            string Prefix = string.Empty;
            var res = _context.tbl_company_emp_setting.Where(p => p.is_active == 1 && p.company_id == companyId).FirstOrDefault();
            if (res == null)
            {
                var Company = _context.tbl_company_master.Where(p => p.company_id == companyId && p.is_active == 1).FirstOrDefault();
                if (Company == null)
                {
                    throw new Exception("Invalid company");
                }
                Prefix = string.Concat(Company.company_code.Substring(0, 1).ToUpper(), _IsrvSettings.GenrateCharcter(false, 2));
                res = new projContext.DB.tbl_company_emp_setting()
                {
                    company_id = companyId,
                    is_active = 1,
                    prefix_for_employee_code = Prefix,
                    current_range = 2,
                    from_range = 1,
                    last_genrated = DateTime.Now,
                    number_of_character_for_employee_code = 5,
                    to_range = 99999
                };
                _context.tbl_company_emp_setting.Add(res);
                _context.SaveChanges();
                Prefix = string.Concat(Prefix, (1).ToString("D5"));
            }
            else
            {
                res.current_range = res.current_range + 1;
                _context.tbl_company_emp_setting.Update(res);
                _context.SaveChanges();
                Prefix = string.Concat(res.prefix_for_employee_code, res.current_range.ToString("D" + (res.number_of_character_for_employee_code < 2 ? 2 : res.number_of_character_for_employee_code)));
            }
            return Prefix;
        }


        public bool IsActiveEmpExistsById(int EmpId)
        {
            return _context.tbl_employee_master.Where(p => p.is_active == 1 && p.employee_id == EmpId).Count() > 0 ? true : false;
        }








    }
}

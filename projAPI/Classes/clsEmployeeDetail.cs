using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projContext;
using projContext.DB;
using projAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;

namespace projAPI.Classes
{
#if (false)
    public class clsEmployeeDetail
    {
        public int _company_id;
        private DateTime PayrollLastdate = DateTime.Now;
        private DateTime PayrollStartdate = DateTime.Now;
        private readonly IHttpContextAccessor _AC;
        private readonly Context _context;
        private readonly IConfiguration _config;
        private readonly clsCurrentUser _clsCurrentUser;
        private readonly DateTime _CurrentDateTime;
        public List<int> EmpIDs;

        public List<int> ReqID;


        public clsEmployeeDetail(Context context, IConfiguration config, IHttpContextAccessor AC, clsCurrentUser _clsCurrentUser)
        {
            _context = context;
            _config = config;
            _AC = AC;
            this._clsCurrentUser = _clsCurrentUser;
            this._CurrentDateTime = DateTime.Now;
        }


        public List<EmployeeOfficaialSection> BindEmpSalaryGroup()
        {
            List<tbl_sg_maping> list;
            List<EmployeeOfficaialSection> _emplist = new List<EmployeeOfficaialSection>();

            if (_company_id > 0)
            {
                list = _context.tbl_sg_maping.Where(x => x.is_active == 1 && x.tem.tbl_employee_company_map.FirstOrDefault(z => z.is_deleted == 0 && z.tbl_company_master.is_active == 1).company_id == _company_id).Distinct().ToList();

            }
            else
            {
                list = _context.tbl_sg_maping.Where(x => x.is_active == 1 && x.tem.tbl_employee_company_map.FirstOrDefault(z => z.is_deleted == 0 && z.tbl_company_master.is_active == 1).company_id == _company_id).Distinct().ToList();
            }
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    var empdtll = _context.tbl_emp_officaial_sec.Where(x => x.employee_id == item.emp_id && x.is_deleted == 0).OrderByDescending(p => p.emp_official_section_id).Select(
                        p => new
                        {
                            p.tbl_employee_id_details.emp_code,
                            p.employee_first_name,
                            p.employee_middle_name,
                            p.employee_last_name
                        }
                        ).FirstOrDefault();

                    if (empdtll != null)
                    {
                        EmployeeOfficaialSection sec = new EmployeeOfficaialSection();
                        if (!_emplist.Any(x => x.employee_id == item.emp_id))
                        {
                            sec.emp_code = string.Format("{0} {1} {2} ({3})", empdtll.employee_first_name, empdtll.employee_middle_name, empdtll.employee_last_name, empdtll.emp_code);
                            sec.employee_id = item.emp_id ?? 0;

                            _emplist.Add(sec);
                        }

                    }
                }
            }



            return _emplist;
        }

        public List<int> ChangeEmployementType()
        {
            throw new NotImplementedException();
#if false
            DateTime todaydate = DateTime.Now;

            List<mdlChnageEmployementType> _mdc = _context.tbl_emp_officaial_sec.Where(p => p.is_deleted == 0 && p.tbl_employee_id_details.is_active == 1).Select(p => new
               mdlChnageEmployementType
            { empid = p.employee_id ?? 0, current_status = p.current_employee_type, new_status = p.current_employee_type, status_change_date = todaydate }).ToList();
            List<int> EmpIDs = _mdc.Select(p => p.empid).Distinct().ToList();
            var data = _context.tbl_employment_type_master.Where(p => p.is_deleted == 0 && p.effective_date <= DateTime.Now && EmpIDs.Contains(p.employee_id ?? 0)).ToList();
            for (int i = 0; i < _mdc.Count; i++)
            {
                var tempdata = data.Where(p => p.employee_id == _mdc[i].empid).OrderByDescending(p => p.employment_type_id).FirstOrDefault();
                if (tempdata != null)
                {
                    _mdc[i].new_status = tempdata.employment_type;
                    _mdc[i].status_change_date = tempdata.effective_date;
                }
            }


            _mdc.RemoveAll(p => p.current_status == p.new_status);

            for (int j = 0; j < _mdc.Count; j++)
            {
                var data_ = _context.tbl_emp_officaial_sec.Where(p => p.is_deleted == 0 && p.tbl_employee_id_details.is_active == 1 && p.employee_id == _mdc[j].empid).FirstOrDefault();

                data_.current_employee_type = _mdc[j].new_status;
                data_.last_modified_date = _mdc[j].status_change_date;

                _context.tbl_emp_officaial_sec.Update(data_);

            }

            _context.SaveChanges();

            //return for update leaves
            return _mdc.Select(p => p.empid).ToList();

#endif
        }

        class mdlChnageEmployementType
        {
            public int empid { get; set; }
            public byte current_status { get; set; }
            public byte new_status { get; set; }
            public DateTime status_change_date { get; set; }
        }

        public bool func_is_user_profile()
        {
            return false;
            //bool _result = false;
            //int _role_id = 0;
            //var temp_role_id = _AC.HttpContext.User.Claims.Where(p => p.Type == "roleid").FirstOrDefault();
            //if (temp_role_id != null)
            //{
            //    _role_id = Convert.ToInt32(temp_role_id.Value);
            //}
            //var _assign_menu_id = _context.tbl_role_menu_master.Where(x => (int)x.role_id == _role_id).FirstOrDefault();

            //string[] menuidd = _assign_menu_id.menu_id.Split(',');
            //string[] menu_name = new string[menuidd.Length];

            //if (_assign_menu_id != null)
            //{
            //    for (int i = 0; i < menuidd.Length; i++)
            //    {
            //        menu_name[i] = _context.tbl_menu_master.Where(x => x.is_active == 1 && (int)x.menu_id == Convert.ToInt32(menuidd[i])).FirstOrDefault().menu_name;
            //    }
            //}


            //if (menu_name.Length > 0)
            //{
            //    for (int j = 0; j < menu_name.Length; j++)
            //    {
            //        if ((menu_name[j].ToString() == "Display Company List") || (menu_name[j].ToString() == "Is Company Admin"))
            //        {
            //            _result = true;
            //            break;
            //        }
            //    }
            //}

            //if (_result)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }


#region *****************   Get the Employee List

        public List<EmployeeBasicData> GetEmployeeByPayrollMonthyear(int company_id, int payroll_month_year)
        {
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now.AddMonths(1);
            //First Check the Payroll Monthyear exist or not
            int Startingday = 1;
            var Data = _context.tbl_payroll_month_setting.FirstOrDefault(p => p.company_id == company_id && p.is_deleted == 0);
            if (Data != null)
            {
                Startingday = Data.from_date;
            }
            if (Startingday == 1)
            {
                FromDate = Convert.ToDateTime(payroll_month_year.ToString().Substring(0, 4) + "-" + payroll_month_year.ToString().Substring(4, 2) + "-" + Startingday);
                ToDate = FromDate.AddMonths(1).AddDays(-1);
            }
            else
            {
                ToDate = Convert.ToDateTime(payroll_month_year.ToString().Substring(0, 4) + "-" + payroll_month_year.ToString().Substring(4, 2) + "-" + (Startingday - 1));
                FromDate = ToDate.AddMonths(-1).AddDays(1);
            }
            PayrollLastdate = ToDate;
            PayrollStartdate = FromDate;
            return GetEmployeeByDate(company_id, FromDate, ToDate);
        }
        public List<EmployeeBasicData> GetEmployeeByEPACycle(int company_id, int FiscalYearId, int CycleId)
        {
            DateTime FromDate = DateTime.Now;
            DateTime ToDate = DateTime.Now.AddMonths(1);
            var epaData = _context.tbl_epa_fiscal_yr_mstr.FirstOrDefault(p => p.fiscal_year_id == FiscalYearId && p.company_id == company_id && p.is_deleted == 0);
            if (epaData == null)
            {
                throw new Exception("Invalid Fiscal Year ID");
            }
            var CycleData = _context.tbl_epa_cycle_master.FirstOrDefault(p => p.company_id == company_id && p.is_deleted == 0);
            if (CycleData == null || CycleId <= 0 || CycleId > 12)
            {
                throw new Exception("Invalid EPA Cycle data");
            }
            FromDate = epaData.from_date;
            if (CycleData.cycle_type == 0)
            {
                FromDate.AddMonths(CycleId);
                ToDate = FromDate.AddMonths(1).AddDays(-1);
            }
            else if (CycleData.cycle_type == 1)
            {
                FromDate.AddMonths(CycleId * 3);
                ToDate = FromDate.AddMonths(3).AddDays(-1);
            }
            else if (CycleData.cycle_type == 2)
            {
                FromDate.AddMonths(CycleId * 6);
                ToDate = FromDate.AddMonths(6).AddDays(-1);
            }
            else
            {
                ToDate = epaData.to_date;
            }

            return GetEmployeeByDate(company_id, FromDate, ToDate);

        }
        public List<EmployeeBasicData> GetEmployeeByDate(int company_id, DateTime FromDate, DateTime ToDate, byte ProcType = 1, int _isActive = 1)
        {
            List<EmployeeBasicData> data = new List<EmployeeBasicData>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_config.GetConnectionString("HRMS")))
                {
                    using (MySqlCommand cmd = new MySqlCommand("proc_get_emp_data", connection))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new MySqlParameter("_proc_type", ProcType));
                        cmd.Parameters.Add(new MySqlParameter("_currentEmpId", _clsCurrentUser.EmpId));
                        cmd.Parameters.Add(new MySqlParameter("_currentUserId", _clsCurrentUser.UserId));
                        cmd.Parameters.Add(new MySqlParameter("_Company_Id", company_id));
                        cmd.Parameters.Add(new MySqlParameter("toDate", ToDate.AddDays(1).AddSeconds(-1)));

                        cmd.Parameters.Add(new MySqlParameter("_isActive", _isActive)); //_isActive 0 for InActive, 1 For Active and 2 for Both Active or InActive

                        connection.Open();
                        MySqlDataReader rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            data.Add(new EmployeeBasicData()
                            {
                                user_id = Convert.ToInt32(rd["user_id"]),
                                isActive = Convert.ToInt32(rd["isActive"]),
                                employee_id = Convert.ToInt32(rd["employee_id"]),
                                emp_code = Convert.ToString(rd["emp_code"]),
                                emp_name = Convert.ToString(rd["emp_name"]),
                                company_id = Convert.ToInt32(rd["company_id"]),
                                company_name = Convert.ToString(rd["company_name"]),
                                dept_id = Convert.ToInt32(rd["dept_id"]),
                                dept_name = Convert.ToString(rd["dept_name"]),
                                location_id = Convert.ToInt32(rd["location_id"]),
                                location_name = Convert.ToString(rd["location_name"]),
                                state_id = Convert.ToInt32(rd["state_id"]),
                                state_name = Convert.ToString(rd["state_name"]),
                                emp_status = Convert.ToInt32(rd["Emp_status"])
                            });
                        }
                        rd.Close();
                        connection.Close();
                    }
                }


                if (_clsCurrentUser.RoleId != null)
                    if (!(_clsCurrentUser.RoleId.Contains((int)enmRoleMaster.SuperAdmin) || _clsCurrentUser.RoleId.Contains((int)enmRoleMaster.HRAdmin)
                        || _clsCurrentUser.RoleId.Contains((int)enmRoleMaster.MastersAdmin)))
                        data = data.Where(x => x.emp_status != (int)EmployeeType.FNF && x.emp_status != (int)EmployeeType.Terminate).Distinct().ToList();

            }
            catch (Exception ex)
            {

            }


            return data;

        }

        public List<EmployeeBasicData> GetEmployeeByDate_Active_Inactive(int company_id, DateTime FromDate, DateTime ToDate, byte ProcType = 1, int _isActive = 1)
        {
            List<EmployeeBasicData> data = new List<EmployeeBasicData>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_context._connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand("proc_get_emp_data", connection))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new MySqlParameter("_proc_type", ProcType));
                        cmd.Parameters.Add(new MySqlParameter("_currentEmpId", _clsCurrentUser.EmpId));
                        cmd.Parameters.Add(new MySqlParameter("_currentUserId", _clsCurrentUser.UserId));
                        cmd.Parameters.Add(new MySqlParameter("_Company_Id", company_id));
                        cmd.Parameters.Add(new MySqlParameter("toDate", ToDate.AddDays(1).AddSeconds(-1)));
                        if (_clsCurrentUser.Is_HRAdmin || _clsCurrentUser.Is_SuperAdmin || _clsCurrentUser.EmpId == 1472 || _clsCurrentUser.EmpId == 1473 || _clsCurrentUser.EmpId == 1474 || _clsCurrentUser.EmpId == 1475 || _clsCurrentUser.EmpId == 1451)
                        {
                            cmd.Parameters.Add(new MySqlParameter("_isActive", 2)); //_isActive 0 for InActive, 1 For Active and 2 for Both Active or InActive
                        }
                        else
                        {
                            cmd.Parameters.Add(new MySqlParameter("_isActive", _isActive)); //_isActive 0 for InActive, 1 For Active and 2 for Both Active or InActive
                        }
                        connection.Open();
                        MySqlDataReader rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            data.Add(new EmployeeBasicData()
                            {
                                user_id = Convert.ToInt32(rd["user_id"]),
                                isActive = Convert.ToInt32(rd["isActive"]),
                                employee_id = Convert.ToInt32(rd["employee_id"]),
                                emp_code = Convert.ToString(rd["emp_code"]),
                                emp_name = Convert.ToString(rd["emp_name"]),
                                company_id = Convert.ToInt32(rd["company_id"]),
                                company_name = Convert.ToString(rd["company_name"]),
                                dept_id = Convert.ToInt32(rd["dept_id"]),
                                dept_name = Convert.ToString(rd["dept_name"]),
                                location_id = Convert.ToInt32(rd["location_id"]),
                                location_name = Convert.ToString(rd["location_name"]),
                                state_id = Convert.ToInt32(rd["state_id"]),
                                state_name = Convert.ToString(rd["state_name"]),
                                emp_status = Convert.ToInt32(rd["Emp_status"])
                            });
                        }
                        rd.Close();
                        connection.Close();
                    }
                }


                if (_clsCurrentUser.RoleId != null)
                    if (!(_clsCurrentUser.RoleId.Contains((int)enmRoleMaster.SuperAdmin) || _clsCurrentUser.RoleId.Contains((int)enmRoleMaster.HRAdmin)
                        || _clsCurrentUser.RoleId.Contains((int)enmRoleMaster.MastersAdmin)))
                        data = data.Where(x => x.emp_status != (int)EmployeeType.FNF && x.emp_status != (int)EmployeeType.Terminate).Distinct().ToList();

            }
            catch (Exception ex)
            {

            }


            return data;

        }

#endregion

#region Getemployee list for dir / birthday/ aniversary created by anil on 2 dec 2020
        public List<EmployeeBasicData> GetEmployee_list_dir()
        {
            List<EmployeeBasicData> data = new List<EmployeeBasicData>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_context._connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand("proc_get_emp_data_dir", connection))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new MySqlParameter("_proc_type", 2));
                        connection.Open();
                        MySqlDataReader rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            data.Add(new EmployeeBasicData()
                            {
                                user_id = Convert.ToInt32(rd["user_id"]),
                                employee_id = Convert.ToInt32(rd["employee_id"]),
                                emp_code = Convert.ToString(rd["emp_code"]),
                                emp_name = Convert.ToString(rd["emp_name"]),
                                company_id = Convert.ToInt32(rd["company_id"]),
                                company_name = Convert.ToString(rd["company_name"]),
                                dept_id = Convert.ToInt32(rd["dept_id"]),
                                dept_name = Convert.ToString(rd["dept_name"]),
                                location_id = Convert.ToInt32(rd["location_id"]),
                                location_name = Convert.ToString(rd["location_name"]),
                                state_id = Convert.ToInt32(rd["state_id"]),
                                state_name = Convert.ToString(rd["state_name"]),
                                emp_status = Convert.ToInt32(rd["Emp_status"]),
                                mobileno = Convert.ToString(rd["mobileno"]),
                                email = Convert.ToString(rd["email"]),
                                desig_name = Convert.ToString(rd["desig_name"]),
                                emp_img = Convert.ToString(rd["emp_img"]),
                                official_contact_no= Convert.ToString(rd["official_contact_no"]),
                                Official_email_id= Convert.ToString(rd["Official_email_id"]),
                            });
                        }
                        rd.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return data;

        }
        public List<EmployeeBasicData> GetEmployee_list_birthday()
        {
            List<EmployeeBasicData> data = new List<EmployeeBasicData>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_context._connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand("proc_get_emp_data_dir", connection))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new MySqlParameter("_proc_type", 3));
                        connection.Open();
                        MySqlDataReader rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            data.Add(new EmployeeBasicData()
                            {
                                user_id = Convert.ToInt32(rd["user_id"]),
                                employee_id = Convert.ToInt32(rd["employee_id"]),
                                emp_code = Convert.ToString(rd["emp_code"]),
                                emp_name = Convert.ToString(rd["emp_name"]),
                                company_id = Convert.ToInt32(rd["company_id"]),
                                company_name = Convert.ToString(rd["company_name"]),
                                dept_id = Convert.ToInt32(rd["dept_id"]),
                                dept_name = Convert.ToString(rd["dept_name"]),
                                location_id = Convert.ToInt32(rd["location_id"]),
                                location_name = Convert.ToString(rd["location_name"]),
                                state_id = Convert.ToInt32(rd["state_id"]),
                                state_name = Convert.ToString(rd["state_name"]),
                                emp_status = Convert.ToInt32(rd["Emp_status"]),
                                mobileno = Convert.ToString(rd["mobileno"]),
                                email = Convert.ToString(rd["email"]),
                                desig_name = Convert.ToString(rd["desig_name"]),
                                emp_img = Convert.ToString(rd["emp_img"]),
                                dob = Convert.ToString(rd["dob"]),
                                // doanv = Convert.ToString(rd["doj"])

                            });
                        }
                        rd.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return data;

        }
        public List<EmployeeBasicData> GetEmployee_list_aniversary()
        {
            List<EmployeeBasicData> data = new List<EmployeeBasicData>();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(_context._connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand("proc_get_emp_data_dir", connection))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new MySqlParameter("_proc_type", 4));
                        connection.Open();
                        MySqlDataReader rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            data.Add(new EmployeeBasicData()
                            {
                                user_id = Convert.ToInt32(rd["user_id"]),
                                employee_id = Convert.ToInt32(rd["employee_id"]),
                                emp_code = Convert.ToString(rd["emp_code"]),
                                emp_name = Convert.ToString(rd["emp_name"]),
                                company_id = Convert.ToInt32(rd["company_id"]),
                                company_name = Convert.ToString(rd["company_name"]),
                                dept_id = Convert.ToInt32(rd["dept_id"]),
                                dept_name = Convert.ToString(rd["dept_name"]),
                                location_id = Convert.ToInt32(rd["location_id"]),
                                location_name = Convert.ToString(rd["location_name"]),
                                state_id = Convert.ToInt32(rd["state_id"]),
                                state_name = Convert.ToString(rd["state_name"]),
                                emp_status = Convert.ToInt32(rd["Emp_status"]),
                                mobileno = Convert.ToString(rd["mobileno"]),
                                email = Convert.ToString(rd["email"]),
                                desig_name = Convert.ToString(rd["desig_name"]),
                                emp_img = Convert.ToString(rd["emp_img"]),
                                // dob = Convert.ToString(rd["dob"]),
                                doanv = Convert.ToString(rd["doj"]),
                            });
                        }
                        rd.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return data;

        }
#endregion

#region ******************** Save Employee Details Section *******************

        public KeyValuePair<bool, string> EmpPermissionExists(IEnumerable<string> EmpCodes)
        {
            List<EmployeeBasicData> data = new List<EmployeeBasicData>();
            foreach (var comp in _clsCurrentUser.CompanyId)
            {
                data.AddRange(GetEmployeeByDate(comp, new DateTime(2000, 1, 1), DateTime.Now.AddDays(1)));
            }
            var tempEmp = data.Select(p => p.emp_code).ToArray();
            var notexistdata = EmpCodes.Where(p => !tempEmp.Contains(p)).ToList();
            if (notexistdata.Count > 0)
            {
                return new KeyValuePair<bool, string>(false, string.Join(", ", notexistdata));
            }
            else
            {
                return new KeyValuePair<bool, string>(true, "");
            }
        }
        public KeyValuePair<bool, string> EmpPermissionExists(IEnumerable<int> Empids)
        {


            List<EmployeeBasicData> data = new List<EmployeeBasicData>();
            foreach (var comp in _clsCurrentUser.CompanyId)
            {
                data.AddRange(GetEmployeeByDate(comp, new DateTime(2000, 1, 1), DateTime.Now.AddDays(1)));
            }

            var tempEmp = data.Select(p => p.employee_id).ToList();
            EmpIDs = tempEmp;
            var notexistdata = Empids.Where(p => !tempEmp.Contains(p)).ToList();
            if (notexistdata.Count > 0)
            {
                return new KeyValuePair<bool, string>(false, string.Join(", ", data.Where(p => notexistdata.Contains(p.employee_id)).Select(p => p.emp_code)));
            }
            else
            {
                return new KeyValuePair<bool, string>(true, "");
            }
        }

        public bool Save_tbl_employee_master(tbl_employee_master mdl)
        {
            var tem = _context.tbl_employee_master.Where(p => p.emp_code == mdl.emp_code).FirstOrDefault();
            if (tem != null)
            {
                throw new Exception(string.Format("Employee Code ({0}) already Exists", mdl.emp_code));
            }
            else
            {
                mdl.last_modified_by = mdl.created_by = _clsCurrentUser.UserId;
                mdl.last_modified_date = mdl.created_date = _CurrentDateTime;
                mdl.is_active = 1;
                _context.Add(mdl);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Save_tbl_emp_officaial_sec(tbl_emp_officaial_sec mdl, byte[] imageBytes, string rootPath)
        {
            throw new NotImplementedException();
            //try
            //{
            //    var tem = _context.tbl_emp_officaial_sec.Where(p => p.employee_id == mdl.employee_id && p.is_deleted == 0).FirstOrDefault();
            //    if (tem != null)
            //    {
            //        tem.is_deleted = 1;
            //        tem.last_modified_by = _clsCurrentUser.EmpId;
            //        tem.last_modified_date = _CurrentDateTime;
            //        _context.Update(tem);
            //        _context.SaveChanges();

            //    }
            //    if (imageBytes != null)
            //    {
            //        try
            //        {
            //            if (!Directory.Exists(rootPath + "/EmployeeImage/EID" + mdl.employee_id + "/"))
            //            {
            //                Directory.CreateDirectory(rootPath + "/EmployeeImage/EID" + mdl.employee_id + "/");
            //            }


            //            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/EmployeeImage/EID" + mdl.employee_id + "/ig_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".jpg");
            //            System.IO.File.WriteAllBytes(path, imageBytes);//save image file
            //                                                           //update file name
            //            mdl.employee_photo_path = "/EmployeeImage/EID" + mdl.employee_id + "/ig_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".jpg";
            //        }
            //        catch (Exception EX)
            //        {
            //            throw new Exception("not able to save image " + EX.Message);
            //        }
            //    }

            //    if (string.IsNullOrEmpty(mdl.employee_photo_path))
            //    {
            //        if (tem != null)
            //        {
            //            mdl.employee_photo_path = tem.employee_photo_path;
            //            if (mdl.current_employee_type == 0)
            //            {
            //                mdl.current_employee_type = tem.current_employee_type;
            //            }
            //        }
            //        else
            //        {
            //            mdl.current_employee_type = (int)enm_employment_type.Probation;
            //        }

            //    }
            //    if (mdl.location_id == 0)
            //    {
            //        mdl.location_id = null;
            //    }
            //    if (mdl.sub_location_id == 0)
            //    {
            //        mdl.sub_location_id = null;
            //    }
            //    if (mdl.department_id == 0)
            //    {
            //        mdl.department_id = null;
            //    }
            //    if (mdl.sub_dept_id == 0)
            //    {
            //        mdl.sub_dept_id = null;
            //    }
            //    if (mdl.religion_id == 0)
            //    {
            //        mdl.religion_id = null;
            //    }
            //    if (mdl.user_type == 0)
            //    {
            //        mdl.user_type = (int)enmRoleMaster.Employee;
            //    }
            //    mdl.emp_official_section_id = 0;
            //    mdl.last_modified_by = mdl.created_by = _clsCurrentUser.UserId;
            //    mdl.last_modified_date = mdl.created_date = _CurrentDateTime;
            //    mdl.is_deleted = 0;
            //    _context.tbl_emp_officaial_sec.Add(mdl);
            //    _context.SaveChanges();
            //    var user_id = _context.tbl_user_master.Where(p => p.employee_id == mdl.employee_id).FirstOrDefault()?.user_id;
            //    Save_tbl_user_role_map(new tbl_user_role_map() { user_id = user_id, role_id = mdl.user_type });
            //    if (mdl.emp_father_name != null)
            //    {
            //        bool UpdateHusband = false;
            //        if (mdl.gender == 1)
            //        {
            //            if (mdl.marital_status == 1)
            //            {
            //                UpdateHusband = true;
            //            }
            //        }

            //        var mdlF = UpdateHusband ? _context.tbl_emp_family_sec.Where(p => p.employee_id == mdl.employee_id && p.relation == "Husband" && p.is_deleted == 0).FirstOrDefault()
            //         : _context.tbl_emp_family_sec.Where(p => p.employee_id == mdl.employee_id && p.relation == "Father" && p.is_deleted == 0).FirstOrDefault();
            //        if (mdlF == null)
            //        {
            //            mdlF = new tbl_emp_family_sec()
            //            {
            //                employee_id = mdl.employee_id,
            //                gender = "2",
            //                is_nominee = 1,
            //                dependent = 2,
            //                relation = UpdateHusband ? "Husband" : "Father",
            //                last_modified_by = _clsCurrentUser.EmpId,
            //                last_modified_date = _CurrentDateTime,
            //                created_by = _clsCurrentUser.UserId,
            //                created_date = _CurrentDateTime,
            //                aadhar_card_no = "",
            //                date_of_birth = new DateTime(2000, 1, 1),
            //                nominee_percentage = 0,
            //                document_image = null,
            //                occupation = "Self Employed",
            //                name_as_per_aadhar_card = mdl.emp_father_name,
            //                remark = "",
            //                is_deleted = 0,

            //            };
            //            _context.tbl_emp_family_sec.Add(mdlF);
            //            _context.SaveChanges();
            //        }

            //    }

            //    return true;
            //}
            //catch (Exception ex)
            //{

            //    ExceptionHandle expt = new ExceptionHandle();
            //    expt.EF_ErrorHandler(ex);
            //    return false;
            //}
        }

        public bool Save_tbl_user_role_map(tbl_user_role_map mdl)
        {

            var tem = _context.tbl_user_role_map.Where(p => p.user_id == mdl.user_id && p.is_deleted == 0).FirstOrDefault();
            if (tem != null)
            {
                tem.is_deleted = 1;
                tem.last_modified_by = _clsCurrentUser.UserId;
                tem.last_modified_date = _CurrentDateTime;
                _context.Update(tem);
                _context.SaveChanges();
            }
            mdl.last_modified_by = mdl.created_by = _clsCurrentUser.UserId;
            mdl.last_modified_date = mdl.created_date = _CurrentDateTime;
            _context.tbl_user_role_map.Add(mdl);
            _context.SaveChanges();
            return true;
        }



#endregion


        public int SaveWeekOffAllocDetailExcel(List<EmployeeOfficaialSection> objdblist)
        {
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    //for (int i = 0; i < objdblist.Count; i++)
                    //{
                    //    tbl_emp_officaial_sec tbl_emp_officaial = (from a in _context.tbl_emp_officaial_sec select a).Where(x => x.employee_id == objdblist[i].employee_id && x.is_deleted == 0).OrderByDescending(x => x.emp_official_section_id).First();
                    //    tbl_emp_officaial.is_fixed_weekly_off = objdblist[i].is_fixed_weekly_off;

                    //    _context.tbl_emp_officaial_sec.Attach(tbl_emp_officaial);
                    //    _context.Entry(tbl_emp_officaial).State = EntityState.Modified;
                    //    _context.SaveChangesAsync();
                    //}


                    trans.Commit();
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 1;

            }
        }


        public List<clsEmployeeListForPayroll> GetEmpPayrollData(int month_year)
        {
            throw new NotImplementedException();
#if (false)
            string year = month_year.ToString().Substring(0, 4);
            string month = month_year.ToString().Substring(4, 2);
            DateTime _ApplicableDate = Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1).AddDays(-1);


            List<EmployeeBasicData> getEmpCode = GetEmployeeByPayrollMonthyear(_company_id, month_year);
            List<tbl_payroll_process_status> tpps = _context.tbl_payroll_process_status.Where(a => a.is_deleted == 0 && a.payroll_month_year == month_year && a.company_id == _company_id).ToList();
            var DepartmentData = _context.tbl_emp_officaial_sec.Where(p => p.is_deleted == 0).Select(p => new {
                employee_id = p.employee_id, department_name=""
                //p.tbl_department_master.department_name 
            }).Distinct().ToList();
            // var DesignationData1 = _context.tbl_emp_desi_allocation.OrderByDescending(q => q.emp_grade_id).Where(p => p.applicable_from_date <= PayrollLastdate && p.applicable_to_date >= PayrollLastdate && p.desig_id != null).Distinct().Select(p => new { p.employee_id, p.tbl_designation_master.designation_name }).ToList();

            var DesignationData = _context.tbl_emp_desi_allocation.OrderByDescending(q => q.emp_grade_id).Where(p => p.applicable_from_date <= PayrollLastdate && p.applicable_to_date >= PayrollLastdate && p.desig_id != null)
                                    .GroupBy(t => t.employee_id).Select(g => g.OrderByDescending(t => t.emp_grade_id).FirstOrDefault()).Select(p => new { p.employee_id, p.tbl_designation_master.designation_name }).ToList();


            var selectEmpSalaryGroup = _context.tbl_sg_maping.Where(p => p.applicable_from_dt <= _ApplicableDate && p.is_active == 1).GroupBy(g => new { employee_id = g.emp_id })
                .Select(p => new { employee_id = p.Key.employee_id ?? 0, applicable_from_dt = p.Max(q => q.applicable_from_dt) })
                .Join(_context.tbl_sg_maping.Where(p => p.is_active == 1).Select(p => new { employee_id = p.emp_id ?? 0, applicable_from_dt = p.applicable_from_dt, salary_group_id = p.salary_group_id })
                , p => new { p.employee_id, p.applicable_from_dt }, q => new { q.employee_id, q.applicable_from_dt },
                (p, q) => new { p.employee_id, q.salary_group_id }).Join(_context.tbl_salary_group, p => p.salary_group_id, q => q.group_id,
                (p, q) => new { p.employee_id, q.group_name })
                .ToList();
            var UserData = _context.tbl_user_master.Where(x => x.is_active == 1 && x.default_company_id == _company_id).Select(p => new { p.user_id, p.employee_id }).ToList();

            var EmpSalary = _context.tbl_emp_salary_master.Where(p => p.is_active == 1 && p.applicable_from_dt.Date <= _ApplicableDate).ToList();
            List<clsEmployeeListForPayroll> Returndata =
                (from t1 in getEmpCode
                 join t2 in tpps on t1.employee_id equals t2.emp_id into t1t2
                 join t3 in DepartmentData on t1.employee_id equals t3.employee_id into t1t3
                 join t4 in DesignationData on t1.employee_id equals t4.employee_id into t1t4
                 join t5 in selectEmpSalaryGroup on t1.employee_id equals t5.employee_id into t1t5
                 //join t6 in UserData on t1.employee_id equals t6.employee_id into t1t6
                 from t1t2_ in t1t2.DefaultIfEmpty()
                 from t1t3_ in t1t3.DefaultIfEmpty()
                 from t1t4_ in t1t4.DefaultIfEmpty()
                 from t1t5_ in t1t5.DefaultIfEmpty()
                     //from t1t6_ in t1t6.DefaultIfEmpty()
                 select new clsEmployeeListForPayroll
                 {
                     empid = t1.employee_id,
                     empcode = t1.emp_code,
                     empname = t1.emp_name,
                     userid = t1.user_id,
                     empdept = t1t3_?.department_name ?? string.Empty,
                     empdesig = t1t4_?.designation_name ?? string.Empty,
                     salary_group = t1t5_?.group_name ?? string.Empty,
                     is_lock = t1t2_?.is_lock == 1 ? "Yes" : "No",
                     partially_freezed = t1t2_?.is_freezed == 1 ? "Yes" : "No",
                     partially_calculated = t1t2_?.is_calculated == 1 ? "Yes" : "No",
                     salary = EmpSalary.Where(p => p.emp_id == t1.employee_id).OrderByDescending(a => a.applicable_from_dt).Select(p => p.salaryrevision).FirstOrDefault()
                 }).Distinct().ToList();

            return Returndata;

#endif

        }

        public int SaveQualificationDetailExcel(List<EmployeeQualificationSection> objdblist)
        {
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    var EmpID = objdblist.Select(p => p.employee_id).ToArray();

                    var EmpData = _context.tbl_emp_qualification_sec.Where(a => EmpID.Contains(a.employee_id ?? 0) && a.is_deleted == 0).ToList();

                    var LinqData = (from t1 in EmpData
                                    join t2 in objdblist on t1.employee_id.Value equals t2.employee_id
                                    select t1
                                    ).ToList();

                    _context.tbl_emp_qualification_sec.RemoveRange(LinqData);
                    var t1task = _context.SaveChangesAsync();

                    List<tbl_emp_qualification_sec> objchangeinput = objdblist.Select(p => new tbl_emp_qualification_sec
                    {

                        board_or_university = p.board_or_university,
                        institute_or_school = p.institute_or_school,
                        passing_year = p.passing_year,
                        stream = p.stream,
                        education_type = Convert.ToByte(p.education_type_),
                        education_level = Convert.ToByte(p.education_level_),
                        marks_division_cgpa = p.marks_division_cgpa,
                        remark = p.remark,
                        employee_id = p.employee_id,
                        created_date = DateTime.Now,
                        created_by = p.created_by,
                        last_modified_date = Convert.ToDateTime("01-01-2000"),

                    }).ToList();
                    _context.AddRange(objchangeinput);
                    _context.SaveChanges();

                    trans.Commit();
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 1;

            }

        }

        public int SaveFamilyDetailExcel(List<EmployeeFamilySection> objdblist)
        {
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    var EmpID = objdblist.Select(p => p.employee_id).ToArray();

                    var EmpData = _context.tbl_emp_family_sec.Where(a => EmpID.Contains(a.employee_id ?? 0) && a.is_deleted == 0).ToList();

                    var LinqData = (from t1 in EmpData
                                    join t2 in objdblist on t1.employee_id.Value equals t2.employee_id
                                    select t1
                                    ).ToList();

                    _context.tbl_emp_family_sec.RemoveRange(LinqData);
                    var t1task = _context.SaveChangesAsync();

                    List<tbl_emp_family_sec> objchangeinput = objdblist.Select(p => new tbl_emp_family_sec
                    {
                        relation = p.relation,
                        occupation = p.occupation,
                        name_as_per_aadhar_card = p.name_as_per_aadhar_card,
                        date_of_birth = p.date_of_birth,
                        gender = (p.gender.Trim().ToUpper() == "FEMALE" ? 1 : p.gender.Trim().ToUpper() == "MALE" ? 2 : p.gender.Trim().ToUpper() == "OTHER" || p.gender.Trim().ToUpper() == "OTHERS" ? 3 : 0).ToString(),
                        dependent = Convert.ToByte(p.dependent.Trim().ToUpper() == "YES" ? 1 : p.dependent.Trim().ToUpper() == "NO" ? 2 : 0),
                        is_nominee = Convert.ToByte(p.is_nominee.Trim().ToUpper() == "YES" ? 1 : p.is_nominee.Trim().ToUpper() == "NO" ? 2 : 0),
                        remark = p.remark,
                        employee_id = p.employee_id,
                        created_date = DateTime.Now,
                        created_by = p.created_by,
                        aadhar_card_no = p.aadhar_card_no,
                        last_modified_by = 0,
                        last_modified_date = Convert.ToDateTime("01-01-2000"),

                    }).ToList();
                    _context.AddRange(objchangeinput);
                    _context.SaveChanges();

                    trans.Commit();
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 1;

            }
        }

        public List<EmployeeManager> Get_Emp_manager_dtl(int empid)
        {
            throw new NotImplementedException();
#if false
            List<EmployeeManager> emp_mgr = new List<EmployeeManager>();
            try
            {
                var empmgr_ = _context.tbl_emp_manager.Where(a => a.employee_id == empid && a.is_deleted == 0).OrderByDescending(b => b.emp_mgr_id).FirstOrDefault();

               // var empmgr_ = _context.tbl_emp_manager.Where(x => x.is_deleted == 0 && x.employee_id == empid).FirstOrDefault();
                if (empmgr_ != null)
                {
                    EmployeeManager _mgr = new EmployeeManager();

                    // List<int> mgr_id = new List<int>();
                    if (empmgr_.m_one_id != null)
                    {
                        _mgr.m_one_id = empmgr_.m_one_id ?? 0;
                        //  mgr_id.Add(empmgr_.m_one_id ?? 0);
                    }
                    if (empmgr_.m_two_id != null)
                    {
                        _mgr.m_two_id = empmgr_.m_two_id ?? 0;
                        //mgr_id.Add(empmgr_.m_two_id ?? 0);
                    }
                    if (empmgr_.m_three_id != null)
                    {
                        _mgr.m_three_id = empmgr_.m_three_id ?? 0;
                        //mgr_id.Add(empmgr_.m_three_id ?? 0);
                    }

                    _mgr.applicable_from_date1 = empmgr_.applicable_from_date;
                    _mgr.applicable_to_date1 = empmgr_.applicable_to_date;
                    _mgr.notify_manager_1 = empmgr_.notify_manager_1;
                    _mgr.notify_manager_2 = empmgr_.notify_manager_2;
                    _mgr.notify_manager_3 = empmgr_.notify_manager_3;

                    _mgr.final_approval = empmgr_.final_approval;

                    emp_mgr.Add(_mgr);

                    var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && emp_mgr.Any(y => y.m_one_id == x.employee_id || y.m_two_id == x.employee_id || y.m_three_id == x.employee_id)).Select(p => new
                    {
                        p.employee_id,
                        emp_name_code = string.Format("{0} {1} {2}", p.employee_first_name, p.employee_middle_name, p.employee_last_name),
                    }).Distinct().ToList();

                    emp_mgr.ForEach(x =>
                    {
                        x.manager_name_code = teos.FirstOrDefault(q => q.employee_id == x.m_one_id)?.emp_name_code;
                        x.m_two_name_code = teos.FirstOrDefault(q => q.employee_id == x.m_two_id)?.emp_name_code;
                        x.m_three_name_code = teos.FirstOrDefault(q => q.employee_id == x.m_three_id)?.emp_name_code;

                    });

                    //emp_mgr = teos.Select(p => new EmployeeManager
                    //{
                    //    m_one_id=(empmgr_.m_one_id!=null? empmgr_.m_one_id:0)??0,
                    //    m_two_id = (empmgr_.m_two_id != null ? empmgr_.m_two_id : 0) ?? 0,
                    //    m_three_id = (empmgr_.m_three_id != null ? empmgr_.m_three_id : 0) ?? 0,
                    //    final_approval = empmgr_.final_approval,
                    //    manager_name_code = p.employee_id == empmgr_.m_one_id ? p.emp_name_code : "",
                    //    m_two_name_code = p.employee_id == empmgr_.m_two_id ? p.emp_name_code : "",
                    //    m_three_name_code = p.employee_id == empmgr_.m_three_id ? p.emp_name_code : "",

                    //}).ToList();
                }
            }
            catch (Exception ex)
            {

            }

            return emp_mgr;
#endif
        }



        public int SaveShiftAllocDetailExcel(List<EmployeeShiftAlloc> objdblist)
        {
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    List<tbl_emp_shift_allocation> objchangeinput = objdblist.Select(p => new tbl_emp_shift_allocation
                    {
                        shift_id = Convert.ToInt32(p.shift_id),
                        employee_id = p.employee_id,
                        applicable_from_date = p.applicable_from_date,
                    }).ToList();
                    _context.AddRange(objchangeinput);
                    _context.SaveChanges();

                    trans.Commit();
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 1;

            }
        }



        public int SaveGradeAllocDetailExcel(List<EmployeeGradeAlloc> objdblist)
        {
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    List<tbl_emp_grade_allocation> objchangeinput = objdblist.Select(p => new tbl_emp_grade_allocation
                    {
                        grade_id = Convert.ToInt32(p.grade_id),
                        employee_id = p.employee_id,
                        applicable_from_date = p.applicable_from_date,
                        applicable_to_date = p.applicable_to_date
                    }).ToList();
                    _context.AddRange(objchangeinput);
                    _context.SaveChanges();

                    trans.Commit();
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 1;

            }
        }

        public int SaveDesignationAllocDetailExcel(List<EmployeeDesignationAlloc> objdblist)
        {
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {
                    List<tbl_emp_desi_allocation> objchangeinput = objdblist.Select(p => new tbl_emp_desi_allocation
                    {
                        desig_id = Convert.ToInt32(p.designation_id),
                        employee_id = p.employee_id,
                        applicable_from_date = p.applicable_from_date,
                        applicable_to_date = p.applicable_to_date
                    }).ToList();
                    _context.AddRange(objchangeinput);
                    _context.SaveChanges();

                    trans.Commit();
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 1;

            }
        }

        public int SaveManagerAllocDetailExcel(List<EmployeeManagerAlloc> objdblist)
        {
            throw new NotImplementedException();
#if false
            try
            {
                using (var trans = _context.Database.BeginTransaction())
                {

                    var EmpID = objdblist.Select(p => p.employee_id).ToArray();

                    var EmpData = _context.tbl_emp_manager.Where(a => EmpID.Contains(a.employee_id ?? 0) && a.is_deleted == 0).ToList();

                    var LinqData = (from t1 in EmpData
                                    join t2 in objdblist on t1.employee_id.Value equals t2.employee_id
                                    select t1
                                    ).ToList();

                    _context.tbl_emp_manager.RemoveRange(LinqData);
                    var t1task = _context.SaveChangesAsync();


                    List<tbl_emp_manager> objchangeinput = objdblist.Select(p => new tbl_emp_manager
                    {
                        employee_id = p.employee_id,
                        final_approval = p.final_approval,
                        m_one_id = p.m_one,
                        m_two_id = p.m_two,
                        m_three_id = p.m_three,
                        is_deleted = 0,
                        notify_manager_1 = p.m_one != null && p.m_one != 0 ? Convert.ToByte(1) : Convert.ToByte(0),
                        notify_manager_2 = p.m_two != null && p.m_two != 0 ? Convert.ToByte(1) : Convert.ToByte(0),
                        notify_manager_3 = p.m_three != null && p.m_three != 0 ? Convert.ToByte(1) : Convert.ToByte(0),
                        applicable_from_date = p.applicable_from_date,
                        applicable_to_date = p.applicable_to_date
                    }).ToList();
                    _context.AddRange(objchangeinput);

                    //start update usertype in official section
                    var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && objchangeinput.Any(y => y.m_one_id == x.employee_id || y.m_two_id == x.employee_id || y.m_three_id == x.employee_id)).ToList();
                    if (teos.Count > 0)
                    {
                       // teos.ForEach(p => { p.user_type = (int)enmRoleMaster.Manager; p.last_modified_by = _clsCurrentUser.EmpId; p.last_modified_date = DateTime.Now; });
                    }
                    _context.tbl_emp_officaial_sec.UpdateRange(teos);

                    var user_role = _context.tbl_user_master.Where(x => x.is_active == 1 && teos.Any(y => y.employee_id == x.employee_id)).Select(p => p.user_id).ToList();
                    if (user_role.Count > 0)
                    {
                        var roles = _context.tbl_user_role_map.Where(x => x.is_deleted == 0 && user_role.Contains(x.user_id ?? 0)).ToList();
                        if (roles.Count > 0)
                        {
                            roles.ForEach(p => { p.role_id = (int)enmRoleMaster.Manager; p.last_modified_by = _clsCurrentUser.EmpId; p.last_modified_date = DateTime.Now; });
                        }

                        _context.tbl_user_role_map.UpdateRange(roles);
                    }



                    //end update usertype in official section

                    _context.SaveChanges();

                    trans.Commit();
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 1;

            }
#endif
        }



        public int SaveEmpPersonalDetailFromExcel(List<EmployeePersonalSection> objdblist)
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

                            tbl_emp_personal_sec emp_personal_sec = new tbl_emp_personal_sec();

                            List<tbl_emp_personal_sec> tbl_emp_personal = _context.tbl_emp_personal_sec.OrderByDescending(x => x.emp_personal_section_id).Where(x => x.employee_id == objdblist[i].employee_id && x.is_deleted == 0).ToList();
                            if (tbl_emp_personal.Count > 0)
                            {
                                tbl_emp_personal.ForEach(y => { y.is_deleted = 1; y.last_modified_by = objdblist[i].created_by; y.last_modified_date = DateTime.Now; });
                                _context.UpdateRange(tbl_emp_personal);
                            }



                            emp_personal_sec.employee_id = objdblist[i].employee_id;

                            emp_personal_sec.blood_group = objdblist[i].blood_group;
                            emp_personal_sec.primary_contact_number = objdblist[i].primary_contact_number;
                            emp_personal_sec.secondary_contact_number = objdblist[i].secondary_contact_number;
                            emp_personal_sec.primary_email_id = objdblist[i].primary_email_id;
                            emp_personal_sec.secondary_email_id = objdblist[i].secondary_email_id;
                            emp_personal_sec.permanent_address_line_one = objdblist[i].permanent_address_line_one;
                            emp_personal_sec.permanent_address_line_two = objdblist[i].permanent_address_line_two;
                            emp_personal_sec.permanent_country = objdblist[i].permanent_country;
                            emp_personal_sec.permanent_state = objdblist[i].permanent_state;
                            emp_personal_sec.permanent_city = objdblist[i].permanent_city;
                            emp_personal_sec.permanent_pin_code = objdblist[i].permanent_pin_code;
                            emp_personal_sec.permanent_document_type = objdblist[i].permanent_document_type;

                            emp_personal_sec.corresponding_address_line_one = objdblist[i].permanent_address_line_one;
                            emp_personal_sec.corresponding_address_line_two = objdblist[i].permanent_address_line_two;
                            emp_personal_sec.corresponding_country = objdblist[i].permanent_country;
                            emp_personal_sec.corresponding_state = objdblist[i].permanent_state;
                            emp_personal_sec.corresponding_city = objdblist[i].permanent_city;
                            emp_personal_sec.corresponding_pin_code = objdblist[i].permanent_pin_code;
                            emp_personal_sec.corresponding_document_type = objdblist[i].corresponding_document_type;


                            emp_personal_sec.emergency_contact_name = objdblist[i].emergency_contact_name;
                            emp_personal_sec.emergency_contact_relation = objdblist[i].emergency_contact_relation;
                            emp_personal_sec.emergency_contact_mobile_number = objdblist[i].emergency_contact_mobile_number;
                            emp_personal_sec.emergency_contact_line_one = objdblist[i].permanent_address_line_one;
                            emp_personal_sec.emergency_contact_line_two = objdblist[i].permanent_address_line_two;
                            emp_personal_sec.emergency_contact_country = objdblist[i].permanent_country;
                            emp_personal_sec.emergency_contact_state = objdblist[i].permanent_state;
                            emp_personal_sec.emergency_contact_city = objdblist[i].permanent_city;
                            emp_personal_sec.emergency_contact_pin_code = objdblist[i].permanent_pin_code;
                            emp_personal_sec.emergency_contact_document_type = objdblist[i].emergency_contact_document_type;
                            emp_personal_sec.is_deleted = 0;
                            emp_personal_sec.created_by = objdblist[i].created_by;
                            emp_personal_sec.created_date = DateTime.Now;

                            _context.tbl_emp_personal_sec.AddRange(emp_personal_sec);



                        }



                        if (_rollbanck)
                        {
                            trans.Rollback();
                            return -1;
                        }
                        else
                        {
                            _context.SaveChanges();
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


        public int SaveEmpAccountDetailFromExcel(List<EployeeAccountDetailsForUpload> objdblist)
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

                            tbl_emp_bank_details emp_personal_sec = new tbl_emp_bank_details();

                            List<tbl_emp_bank_details> tbl_emp_bank_details = _context.tbl_emp_bank_details.OrderByDescending(x => x.bank_details_id).Where(x => x.employee_id == objdblist[i].employee_id && x.is_deleted == 0).ToList();
                            if (tbl_emp_bank_details.Count > 0)
                            {
                                tbl_emp_bank_details.ForEach(y => { y.is_deleted = 1; y.last_modified_by = objdblist[i].created_by; y.last_modified_date = DateTime.Now; });
                                _context.UpdateRange(tbl_emp_bank_details);
                            }

                            emp_personal_sec.payment_mode = (enmPaymentMode)Convert.ToInt32(objdblist[i].payment_mode);
                            emp_personal_sec.bank_id = Convert.ToInt32(objdblist[i].bank_id);
                            emp_personal_sec.branch_name = objdblist[i].branch_name;
                            emp_personal_sec.ifsc_code = objdblist[i].ifsc_code;
                            emp_personal_sec.bank_acc = objdblist[i].bank_acc;
                            emp_personal_sec.employee_id = objdblist[i].employee_id;
                            emp_personal_sec.is_deleted = 0;
                            emp_personal_sec.created_by = objdblist[i].created_by;
                            emp_personal_sec.created_date = DateTime.Now;

                            _context.tbl_emp_bank_details.Attach(emp_personal_sec);
                            _context.Entry(emp_personal_sec).State = EntityState.Added;
                            _context.SaveChangesAsync();


                            tbl_emp_pan_details emp_pan_details = new tbl_emp_pan_details();

                            List<tbl_emp_pan_details> tbl_emp_pan_details = _context.tbl_emp_pan_details.OrderByDescending(x => x.pan_details_id).Where(x => x.employee_id == objdblist[i].employee_id && x.is_deleted == 0).ToList();
                            if (tbl_emp_pan_details.Count > 0)
                            {
                                tbl_emp_pan_details.ForEach(y => { y.is_deleted = 1; y.last_modified_by = objdblist[i].created_by; y.last_modified_date = DateTime.Now; });
                                _context.UpdateRange(tbl_emp_pan_details);
                            }

                            emp_pan_details.pan_card_name = objdblist[i].pan_card_name;
                            emp_pan_details.pan_card_number = objdblist[i].pan_card_number;
                            emp_pan_details.employee_id = objdblist[i].employee_id;
                            emp_pan_details.is_deleted = 0;
                            emp_pan_details.created_by = objdblist[i].created_by;
                            emp_pan_details.created_date = DateTime.Now;

                            _context.tbl_emp_pan_details.Attach(emp_pan_details);
                            _context.Entry(emp_pan_details).State = EntityState.Added;
                            _context.SaveChangesAsync();

                            tbl_emp_adhar_details emp_adhar_details = new tbl_emp_adhar_details();

                            List<tbl_emp_adhar_details> tbl_emp_adhar_details = _context.tbl_emp_adhar_details.OrderByDescending(x => x.pan_details_id).Where(x => x.employee_id == objdblist[i].employee_id && x.is_deleted == 0).ToList();
                            if (tbl_emp_adhar_details.Count > 0)
                            {
                                tbl_emp_adhar_details.ForEach(y => { y.is_deleted = 1; y.last_modified_by = objdblist[i].created_by; y.last_modified_date = DateTime.Now; });
                                _context.UpdateRange(tbl_emp_adhar_details);
                            }

                            emp_adhar_details.aadha_card_name = objdblist[i].aadha_card_name;
                            emp_adhar_details.aadha_card_number = objdblist[i].aadha_card_number;
                            emp_adhar_details.employee_id = objdblist[i].employee_id;
                            emp_adhar_details.is_deleted = 0;
                            emp_adhar_details.created_by = objdblist[i].created_by;
                            emp_adhar_details.created_date = DateTime.Now;

                            _context.tbl_emp_adhar_details.Attach(emp_adhar_details);
                            _context.Entry(emp_adhar_details).State = EntityState.Added;
                            _context.SaveChangesAsync();

                        }


                        if (_rollbanck)
                        {
                            trans.Rollback();
                            return -1;
                        }
                        else
                        {
                            _context.SaveChanges();
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

        public List<LeaveLedgerModell> Get_employee_total_compoff_ledger(List<int> _empidd)
        {
            List<LeaveLedgerModell> _compoff_ledger = new List<LeaveLedgerModell>();

            try
            {
                _compoff_ledger = _context.tbl_comp_off_ledger.Where(x => x.compoff_date.Year == DateTime.Now.Year && x.is_deleted == 0 &&
                                                                    _empidd.Contains(x.e_id ?? 0)).ToList()
                                       .GroupBy(p => p.e_id).Select(q => new LeaveLedgerModell
                                       {
                                           e_id = q.Key ?? 0,
                                           credit = q.Sum(r => r.credit),
                                           dredit = q.Sum(r => r.dredit),
                                           balance = q.Sum(x => x.credit) - q.Sum(x => x.dredit)
                                       }).ToList();
            }
            catch (Exception ex)
            {

            }
            return _compoff_ledger;
        }

        public int SaveEmpUanEsicDetailFromExcel(List<EployeeUanEsicDetailsForUpload> objdblist)
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

                            tbl_emp_pf_details emp_pf_details = new tbl_emp_pf_details();

                            List<tbl_emp_pf_details> tbl_emp_pf_details = _context.tbl_emp_pf_details.OrderByDescending(x => x.pf_details_id).Where(x => x.employee_id == objdblist[i].employee_id && x.is_deleted == 0).ToList();
                            if (tbl_emp_pf_details.Count > 0)
                            {
                                tbl_emp_pf_details.ForEach(y => { y.is_deleted = 1; y.last_modified_by = objdblist[i].created_by; y.last_modified_date = DateTime.Now; });
                                _context.UpdateRange(tbl_emp_pf_details);
                            }

                            emp_pf_details.is_pf_applicable = 1;
                            emp_pf_details.uan_number = objdblist[i].uan_number;
                            emp_pf_details.pf_number = objdblist[i].pf_number;
                            emp_pf_details.pf_group = (enmPFGroup)Convert.ToInt32(objdblist[i].pf_group);
                            emp_pf_details.is_vpf_applicable = 1;
                            emp_pf_details.vpf_Group = (enmVPFGroup)Convert.ToInt32(objdblist[i].vpf_Group);
                            emp_pf_details.vpf_amount = objdblist[i].vpf_amount;
                            emp_pf_details.pf_celing = objdblist[i].pf_celing;
                            emp_pf_details.bank_id = Convert.ToInt32(objdblist[i].bank_id);
                            emp_pf_details.ifsc_code = objdblist[i].ifsc_code;
                            emp_pf_details.bank_acc = objdblist[i].bank_acc;
                            emp_pf_details.employee_id = objdblist[i].employee_id;
                            emp_pf_details.is_eps_applicable = 1;
                            emp_pf_details.is_deleted = 0;
                            emp_pf_details.created_by = objdblist[i].created_by;
                            emp_pf_details.created_date = DateTime.Now;



                            _context.tbl_emp_pf_details.Attach(emp_pf_details);
                            _context.Entry(emp_pf_details).State = EntityState.Added;
                            _context.SaveChangesAsync();


                            tbl_emp_esic_details emp_esic_details = new tbl_emp_esic_details();

                            List<tbl_emp_esic_details> tbl_emp_esic_details = _context.tbl_emp_esic_details.OrderByDescending(x => x.esic_details_id).Where(x => x.employee_id == objdblist[i].employee_id && x.is_deleted == 0).ToList();
                            if (tbl_emp_esic_details.Count > 0)
                            {
                                tbl_emp_esic_details.ForEach(y => { y.is_deleted = 1; y.last_modified_by = objdblist[i].created_by; y.last_modified_date = DateTime.Now; });
                                _context.UpdateRange(tbl_emp_esic_details);
                            }

                            emp_esic_details.is_esic_applicable = 1;
                            emp_esic_details.esic_number = objdblist[i].esic_number;
                            emp_esic_details.employee_id = objdblist[i].employee_id;
                            emp_esic_details.is_deleted = 0;
                            emp_esic_details.created_by = objdblist[i].created_by;
                            emp_esic_details.created_date = DateTime.Now;

                            _context.tbl_emp_esic_details.Attach(emp_esic_details);
                            _context.Entry(emp_esic_details).State = EntityState.Added;
                            _context.SaveChangesAsync();

                        }


                        if (_rollbanck)
                        {
                            trans.Rollback();
                            return -1;
                        }
                        else
                        {
                            _context.SaveChanges();
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



        public int SaveEmpAllDetailFromExcel(List<EmployeeAllDataUpload> objdblist)
        {
            try
            {
                List<EmployeeAllDataUpload> objdblistIssue = new List<EmployeeAllDataUpload>();
                int[] companies = objdblist.Where(z => string.IsNullOrEmpty(z.error_message)).Select(x => x.company_id).Distinct().ToArray();
                //bool _rollbanck = false;
                //int totalempinExcel = objdblist.Count;
                foreach (int item in companies)
                {
                    // string url = Convert.ToString(_config["License_domain_url"]) + item + "/" + Convert.ToString(_config["Instance_ID"]);// sample url



                    int total_no_of_employee = 1000;

                    //using (HttpClient client = new HttpClient())
                    //{
                    //    total_no_of_employee = Convert.ToInt32(client.GetStringAsync(url).Result);
                    //}

                    // total no of employee already added
                    int get_exicest_emp = (from t1 in _context.tbl_employee_master
                                           join t2 in _context.tbl_employee_company_map on t1.employee_id equals t2.employee_id
                                           where t2.is_default && t2.company_id == item
                                           select t1.employee_id).Distinct().Count();


                    //remaining no of employee 
                    int remaining_no_of_emp
                        = total_no_of_employee - get_exicest_emp;
                    if (remaining_no_of_emp < objdblist.Where(p => p.emp_id == null).Count())
                    {
                        while (objdblist.Where(x => x.company_id == item).ToList().Count() > 0)
                        {
                            objdblist.Remove(objdblist.Where(x => x.company_id == item).FirstOrDefault());
                        }
                        objdblistIssue = objdblist.Where(x => x.company_id == item).ToList();
                        objdblistIssue.ForEach(x => x.error_message = string.Format("Employee limit exceed, Remaining limit ({0})!!!", remaining_no_of_emp));
                        continue;
                    }
                }

                using (var trans = _context.Database.BeginTransaction())
                {

                    try
                    {

                        for (int i = 0; i < objdblist.Count; i++)
                        {
                            if (objdblist[i].emp_id == null)
                            {
                                tbl_employee_master tem = new tbl_employee_master()
                                {
                                    emp_code = objdblist[i].emp_code.Trim().ToUpper(),
                                    created_by = _clsCurrentUser.UserId,
                                    created_date = _CurrentDateTime,
                                    is_active = 1,
                                    last_modified_by = _clsCurrentUser.UserId,
                                    last_modified_date = _CurrentDateTime,
                                };
                                _context.tbl_employee_master.Add(tem);
                                _context.SaveChanges();
                                objdblist[i].emp_id = tem.employee_id;


                                string encryemppwd = AESEncrytDecry.EncryptStringAES(objdblist[i].emp_code.Trim().ToLower());


                                tbl_user_master tbl_user_master = new tbl_user_master();
                                tbl_user_master.username = objdblist[i].emp_code.Trim().ToUpper();
                                tbl_user_master.password = encryemppwd;//EmployeeCode;
                                //tbl_user_master.user_type = 1;
                                tbl_user_master.is_active = objdblist[i].is_active_id;
                                tbl_user_master.created_by = _clsCurrentUser.UserId;
                                tbl_user_master.created_date = DateTime.Now;
                                tbl_user_master.last_modified_by = 0; //user_master.last_modified_by;
                                tbl_user_master.last_modified_date = Convert.ToDateTime("01-01-2000"); //DateTime.Now;
                                //tbl_user_master.default_company_id = objdblist[i].company_id;
                                tbl_user_master.employee_id = tem.employee_id;
                                _context.tbl_user_master.Add(tbl_user_master);
                                _context.SaveChanges();

                                tbl_user_role_map emp_user_role_map = new tbl_user_role_map();
                                emp_user_role_map.role_id = (int)enmRoleMaster.Employee;
                                emp_user_role_map.user_id = tbl_user_master.user_id;
                                emp_user_role_map.is_deleted = 0;
                                emp_user_role_map.created_by = _clsCurrentUser.UserId;
                                emp_user_role_map.created_date = DateTime.Now;
                                emp_user_role_map.last_modified_by = _clsCurrentUser.UserId;//user_master.created_by;
                                emp_user_role_map.last_modified_date = DateTime.Now;//DateTime.Now;
                                                                                    //Save data in tbl_emp_managers
                                _context.tbl_user_role_map.Add(emp_user_role_map);
                                _context.SaveChanges();



                            }
                        }


                        List<int> emp_ids = objdblist.Select(p => p.emp_id ?? 0).ToList();

                        //User Master Sec
                        var existinguserMastersecs = _context.tbl_user_master.Where(p => emp_ids.Contains(p.employee_id ?? 0) && p.is_active == 1).ToList();
                        foreach (var item in existinguserMastersecs)
                        {
                            item.is_active = objdblist.Where(x => x.emp_id == item.employee_id).FirstOrDefault().is_active_id;
                            //item.default_company_id = objdblist.Where(x => x.emp_id == item.employee_id).FirstOrDefault().company_id;
                            item.last_modified_by = _clsCurrentUser.UserId;
                            item.last_modified_date = _CurrentDateTime;
                        }
                        _context.tbl_user_master.UpdateRange(existinguserMastersecs);
                        _context.SaveChanges();


                        //Official Sec
                        var existingOfficalsecs = _context.tbl_emp_officaial_sec.Where(p => emp_ids.Contains(p.employee_id ?? 0) && p.is_deleted == 0).ToList();
                        foreach (var existingOfficalsec in existingOfficalsecs)
                        {
                            existingOfficalsec.is_deleted = 1;
                            existingOfficalsec.last_modified_by = _clsCurrentUser.UserId;
                            existingOfficalsec.last_modified_date = _CurrentDateTime;
                        }
                        _context.tbl_emp_officaial_sec.UpdateRange(existingOfficalsecs);

                        var newOfficalsec = objdblist.Select(p => new tbl_emp_officaial_sec
                        {
                            //is_applicable_for_all_comp = 0,
                            //employee_id = p.emp_id,
                            //location_id = p.location_id,
                            //state_id = p.state_id,
                            //sub_location_id = null,
                            //department_id = p.dept_id,
                            //sub_dept_id = null,
                            //card_number = p.card_number.ToString(),
                            //gender = p.gender_id,
                            //salutation = p.salutation_id.ToString(),
                            //employee_first_name = p.emp_name.Split(" ")[0],
                            //employee_middle_name = p.emp_name.Split(" ").Count() >= 3 ? p.emp_name.Split(" ")[1] : "",
                            //employee_last_name = p.emp_name.Split(" ").Count() == 1 ? "" : p.emp_name.Split(" ").Count() == 2 ? p.emp_name.Split(" ")[1] : p.emp_name.Split(" ").Count() == 3 ? p.emp_name.Split(" ")[2] : (p.emp_name.Split(" ").Count() == 4 ? p.emp_name.Split(" ")[2] + " " + p.emp_name.Split(" ")[3] : (p.emp_name.Split(" ").Count() == 5 ? p.emp_name.Split(" ")[2] + " " + p.emp_name.Split(" ")[3] + " " + p.emp_name.Split(" ")[4] : p.emp_name.Split(" ")[2] + " " + p.emp_name.Split(" ")[3] + " " + p.emp_name.Split(" ")[4] + p.emp_name.Split(" ")[2] + " " + p.emp_name.Split(" ")[3] + " " + p.emp_name.Split(" ")[5])),
                            //emp_father_name = p.father_husband_name,
                            //nationality = p.nationality,
                            //group_joining_date = p.date_of_joining,
                            //date_of_joining = p.date_of_joining,
                            //date_of_birth = p.date_of_birth,
                            //religion_id = 3,
                            //marital_status = Convert.ToByte(p.marital_status_id),
                            //official_email_id = p.email_work,
                            //is_ot_allowed = 0,
                            //is_comb_off_allowed = 1,
                            //current_employee_type = Convert.ToByte(p.employee_status_id),
                            //mobile_punch_from_date = new DateTime(2000, 01, 01),
                            //mobile_punch_to_date = new DateTime(2000, 01, 01),
                            //user_type = (int)enmRoleMaster.Employee,
                            //department_date_of_joining = p.date_of_joining,
                            //punch_type = 0,
                            hr_spoc = "",
                            employee_photo_path = "",
                            //last_working_date = p.resignation_date.Date != new DateTime(0001, 01, 01).Date ? p.resignation_date : new DateTime(2000, 01, 01),
                            //is_fixed_weekly_off = 1,
                            is_deleted = 0,
                            created_by = _clsCurrentUser.UserId,
                            created_date = _CurrentDateTime,
                            last_modified_by = _clsCurrentUser.UserId,
                            last_modified_date = _CurrentDateTime,
                        }).ToList();
                        newOfficalsec.ForEach(p =>
                        {
                            var tempdata = existingOfficalsecs.FirstOrDefault(q => p.employee_id == q.employee_id);
                            if (tempdata != null)
                            {
                                p.religion_id = tempdata.religion_id;
                                p.hr_spoc = tempdata.hr_spoc;
                                p.employee_photo_path = tempdata.employee_photo_path;
                                //p.is_ot_allowed = tempdata.is_ot_allowed;
                                //p.department_date_of_joining = tempdata.department_date_of_joining;
                                //p.is_comb_off_allowed = tempdata.is_comb_off_allowed;
                                //p.last_working_date = tempdata.last_working_date;
                                //p.punch_type = tempdata.punch_type;
                                //p.is_fixed_weekly_off = tempdata.is_fixed_weekly_off;
                                //p.sub_dept_id = tempdata.sub_dept_id;
                                //p.sub_location_id = tempdata.sub_location_id;
                                //p.is_mobile_access = tempdata.is_mobile_access;
                                //p.is_mobile_attendence_access = tempdata.is_mobile_attendence_access;
                            }
                        });
                        _context.tbl_emp_officaial_sec.AddRange(newOfficalsec);
                        _context.SaveChanges();

                        //Company Sec
                        var existingCompanySec = _context.tbl_employee_company_map.Where(p => emp_ids.Contains(p.employee_id ?? 0) && p.is_deleted == 0).ToList();
                        foreach (var item in existingCompanySec)
                        {
                            item.is_deleted = 1;
                            item.last_modified_by = _clsCurrentUser.UserId;
                            item.last_modified_date = _CurrentDateTime;
                        }
                        _context.tbl_employee_company_map.UpdateRange(existingCompanySec);

                        _context.tbl_employee_company_map.AddRange(objdblist.Select(p => new tbl_employee_company_map
                        {
                            company_id = p.company_id,
                            is_default = true,
                            employee_id = p.emp_id,
                            last_modified_by = _clsCurrentUser.UserId,
                            created_by = _clsCurrentUser.UserId,
                            created_date = _CurrentDateTime,
                            is_deleted = 0,
                            last_modified_date = _CurrentDateTime,

                        }));
                        _context.SaveChanges();


                        //update family section
                        var existingFamilySec = _context.tbl_emp_family_sec.Where(p => emp_ids.Contains(p.employee_id ?? 0) && p.is_deleted == 0).ToList();
                        foreach (var item in existingFamilySec)
                        {
                            item.is_deleted = 1;
                            item.last_modified_by = _clsCurrentUser.UserId;
                            item.last_modified_date = _CurrentDateTime;
                        }
                        _context.tbl_emp_family_sec.UpdateRange(existingFamilySec);

                        var mdlF_Husband = objdblist.Select(p => new tbl_emp_family_sec()
                        {
                            employee_id = p.emp_id,
                            gender = "2",
                            is_nominee = 1,
                            dependent = 2,
                            relation = p.gender_id == 1 && p.marital_status_id == 1 ? "Husband" : "Father",
                            last_modified_by = _clsCurrentUser.UserId,
                            last_modified_date = _CurrentDateTime,
                            created_by = _clsCurrentUser.UserId,
                            created_date = _CurrentDateTime,
                            aadhar_card_no = "",
                            date_of_birth = new DateTime(1900, 1, 1),
                            nominee_percentage = 0,
                            document_image = null,
                            occupation = "Self Employed",
                            name_as_per_aadhar_card = p.father_husband_name == null ? "" : p.father_husband_name,
                            remark = "",
                            is_deleted = 0,

                        });
                        _context.tbl_emp_family_sec.AddRange(mdlF_Husband);
                        _context.SaveChanges();


                        //update Employment type section
                        var existingEmpTypeSec = _context.tbl_employment_type_master.Where(p => emp_ids.Contains(p.employee_id ?? 0) && p.is_deleted == 0).ToList();
                        foreach (var item in existingEmpTypeSec)
                        {
                            item.is_deleted = 1;
                            item.last_modified_by = _clsCurrentUser.UserId;
                            item.last_modified_date = _CurrentDateTime;
                        }
                        _context.tbl_employment_type_master.UpdateRange(existingEmpTypeSec);


#if false
                        _context.tbl_employment_type_master.AddRange(objdblist.Where(p => p.employee_status_id == (int)enm_employment_type.Temporary).Select(p => new tbl_employment_type_master()
                        {
                            employee_id = p.emp_id,
                            actual_duration_days = 0,
                            actual_duration_start_period = p.date_of_joining,
                            actual_duration_end_period = p.date_of_joining.AddMonths(p.probation_period),
                            duration_start_period = p.date_of_joining,
                            duration_end_period = p.date_of_joining.AddMonths(p.probation_period),
                            effective_date = p.date_of_joining,
                            is_deleted = 0,
                            created_by = _clsCurrentUser.UserId,
                            created_date = _CurrentDateTime,
                            employment_type = Convert.ToByte(enm_employment_type.Temporary),
                            duration_days = (p.date_of_joining.AddMonths(p.probation_period) - p.date_of_joining).Days,
                            last_modified_by = _clsCurrentUser.UserId,
                            last_modified_date = _CurrentDateTime,
                        }));
                        _context.tbl_employment_type_master.AddRange(objdblist.Where(p => p.employee_status_id == (int)enm_employment_type.Probation).Select(p => new tbl_employment_type_master()
                        {
                            employee_id = p.emp_id,
                            actual_duration_days = 0,
                            actual_duration_start_period = p.date_of_joining,
                            actual_duration_end_period = p.date_of_joining.AddMonths(p.probation_period),
                            duration_start_period = p.date_of_joining,
                            duration_end_period = p.date_of_joining.AddMonths(p.probation_period),
                            effective_date = p.date_of_joining,
                            is_deleted = 0,
                            created_by = _clsCurrentUser.UserId,
                            created_date = _CurrentDateTime,
                            employment_type = Convert.ToByte((int)enm_employment_type.Probation),
                            duration_days = (p.date_of_joining.AddMonths(p.probation_period) - p.date_of_joining).Days,
                            last_modified_by = _clsCurrentUser.UserId,
                            last_modified_date = _CurrentDateTime,
                        }));
                        _context.tbl_employment_type_master.AddRange(objdblist.Where(p => p.employee_status_id == (int)enm_employment_type.Confirmed).Select(p => new tbl_employment_type_master()
                        {
                            employee_id = p.emp_id,
                            actual_duration_days = 0,
                            actual_duration_start_period = p.confirmation_date,
                            actual_duration_end_period = p.date_of_birth.AddYears(60),
                            duration_start_period = p.confirmation_date.Date == new DateTime(01, 01, 0001).Date ? DateTime.Now : p.confirmation_date,
                            duration_end_period = p.date_of_birth.AddYears(60),
                            effective_date = p.confirmation_date.Date == new DateTime(01, 01, 0001).Date ? DateTime.Now : p.confirmation_date,
                            is_deleted = 0,
                            created_by = _clsCurrentUser.UserId,
                            created_date = _CurrentDateTime,
                            employment_type = Convert.ToByte(enm_employment_type.Confirmed),
                            duration_days = ((p.employee_status_id == (int)enm_employment_type.Notice ? p.resignation_date : p.date_of_birth.AddYears(60)) - p.confirmation_date).Days,
                            last_modified_by = _clsCurrentUser.UserId,
                            last_modified_date = _CurrentDateTime,
                        }));
                        _context.tbl_employment_type_master.AddRange(objdblist.Where(p => p.employee_status_id == (int)enm_employment_type.Notice).Select(p => new tbl_employment_type_master()
                        {
                            employee_id = p.emp_id,
                            actual_duration_days = 0,
                            actual_duration_start_period = p.resignation_date,
                            actual_duration_end_period = p.resignation_date.AddDays(p.notice_period) > p.last_working_date ? p.resignation_date.AddDays(p.notice_period) : p.last_working_date,
                            duration_start_period = p.resignation_date,
                            duration_end_period = p.resignation_date.AddDays(p.notice_period) > p.last_working_date ? p.resignation_date.AddDays(p.notice_period) : p.last_working_date,
                            effective_date = p.resignation_date,
                            is_deleted = 0,
                            created_by = _clsCurrentUser.UserId,
                            created_date = _CurrentDateTime,
                            employment_type = Convert.ToByte(enm_employment_type.Notice),
                            duration_days = ((p.resignation_date.AddDays(p.notice_period) > p.last_working_date ? p.resignation_date.AddDays(p.notice_period) : p.last_working_date) - p.resignation_date).Days,
                            last_modified_by = _clsCurrentUser.UserId,
                            last_modified_date = _CurrentDateTime,
                        }));
#endif
                        _context.SaveChanges();


                        //update Employee Grade section
                        var existingEmpGradeSec = _context.tbl_emp_grade_allocation.Where(p => emp_ids.Contains(p.employee_id ?? 0)).Select(y => y.emp_grade_id).ToList();
                        _context.tbl_emp_grade_allocation.RemoveRange(_context.tbl_emp_grade_allocation.Where(x => existingEmpGradeSec.Contains(x.emp_grade_id)));

                        _context.tbl_emp_grade_allocation.AddRange(objdblist.Select(p => new tbl_emp_grade_allocation
                        {
                            applicable_from_date = _CurrentDateTime,
                            applicable_to_date = _CurrentDateTime.AddYears(10),
                            grade_id = p.grade_id,
                            employee_id = p.emp_id
                        }));
                        _context.SaveChanges();


                        //update Employee Designation section
                        var existingEmpDesigSec = _context.tbl_emp_desi_allocation.Where(p => emp_ids.Contains(p.employee_id ?? 0)).Select(y => y.emp_grade_id).ToList();
                        _context.tbl_emp_desi_allocation.RemoveRange(_context.tbl_emp_desi_allocation.Where(x => existingEmpDesigSec.Contains(x.emp_grade_id)));

                        _context.tbl_emp_desi_allocation.AddRange(objdblist.Select(p => new tbl_emp_desi_allocation
                        {
                            applicable_from_date = _CurrentDateTime,
                            applicable_to_date = _CurrentDateTime.AddYears(10),
                            desig_id = p.desig_id,
                            employee_id = p.emp_id
                        }));
                        _context.SaveChanges();

                        //Adhar Section
                        var mdladhar = _context.tbl_emp_adhar_details.Where(p => emp_ids.Contains(p.employee_id ?? 0) && p.is_deleted == 0).ToList();
                        mdladhar.ForEach(p => { p.is_deleted = 1; p.last_modified_by = _clsCurrentUser.UserId; p.last_modified_date = _CurrentDateTime; });
                        _context.tbl_emp_adhar_details.UpdateRange(mdladhar);

                        _context.tbl_emp_adhar_details.AddRange(objdblist.Where(p => !string.IsNullOrEmpty(p.adhar_no)).Select(p => new tbl_emp_adhar_details
                        {

                            aadha_card_name = p.adhar_name == null ? p.emp_name : p.adhar_name,
                            employee_id = p.emp_id,
                            aadha_card_number = p.adhar_no.Trim().ToCharArray().Length < 12 ? p.adhar_no.Trim() : p.adhar_no.Trim().Substring(0, 12),
                            created_by = _clsCurrentUser.EmpId,
                            last_modified_by = _clsCurrentUser.EmpId,
                            last_modified_date = _CurrentDateTime,
                            created_date = _CurrentDateTime,
                            is_deleted = 0,
                        }));
                        _context.SaveChanges();

                        // Pan Section
                        var mdlpan = _context.tbl_emp_pan_details.Where(p => emp_ids.Contains(p.employee_id ?? 0) && p.is_deleted == 0).ToList();
                        mdlpan.ForEach(p => { p.is_deleted = 1; p.last_modified_by = _clsCurrentUser.UserId; p.last_modified_date = _CurrentDateTime; });
                        _context.tbl_emp_pan_details.UpdateRange(mdlpan);

                        _context.tbl_emp_pan_details.AddRange(objdblist.Where(p => p.PAN_No != null).Select(p => new tbl_emp_pan_details
                        {

                            pan_card_number = p.PAN_No == null ? "" : p.PAN_No.Trim().Substring(0, Math.Min(10, p.PAN_No.Trim().Length)),
                            employee_id = p.emp_id,
                            pan_card_name = p.emp_name,
                            created_by = _clsCurrentUser.EmpId,
                            last_modified_by = _clsCurrentUser.EmpId,
                            last_modified_date = _CurrentDateTime,
                            created_date = _CurrentDateTime,
                            is_deleted = 0,
                        }));
                        _context.SaveChanges();


                        // Bank Section
                        var mdlbank = _context.tbl_emp_bank_details.Where(p => emp_ids.Contains(p.employee_id ?? 0) && p.is_deleted == 0).ToList();

                        mdlbank.ForEach(p =>
                        {
                            var mdldatatemp = objdblist.FirstOrDefault(q => q.emp_id == p.employee_id);
                            if (mdldatatemp != null)
                            {
                                if (mdldatatemp.bank_id != null)
                                {
                                    p.is_deleted = 1; p.last_modified_by = _clsCurrentUser.UserId; p.last_modified_date = _CurrentDateTime;
                                }
                            }
                        });
                        _context.tbl_emp_bank_details.UpdateRange(mdlbank);

                        _context.tbl_emp_bank_details.AddRange(objdblist.Where(p => p.bank_id != null).Select(p => new tbl_emp_bank_details
                        {
                            bank_id = p.bank_id,
                            ifsc_code = p.bank_IFSC_Code == null ? "" : p.bank_IFSC_Code.Substring(0, Math.Min(p.bank_IFSC_Code.Length, 11)),
                            employee_id = p.emp_id,
                            bank_acc = p.salary_account_no,
                            branch_name = p.salary_bank_branch,
                            payment_mode = (enmPaymentMode)p.payment_mode_id,
                            created_by = _clsCurrentUser.UserId,
                            last_modified_by = _clsCurrentUser.UserId,
                            last_modified_date = _CurrentDateTime,
                            created_date = _CurrentDateTime,
                            is_deleted = 0,
                        }));
                        _context.SaveChanges();

                        var mdlpersonal = _context.tbl_emp_personal_sec.Where(p => emp_ids.Contains(p.employee_id ?? 0) && p.is_deleted == 0).ToList();
                        mdlpersonal.ForEach(p => { p.is_deleted = 1; p.last_modified_by = _clsCurrentUser.UserId; p.last_modified_date = _CurrentDateTime; });
                        _context.tbl_emp_personal_sec.UpdateRange(mdlpersonal);

                        var mdlpersonalnew = objdblist.Select(p => new tbl_emp_personal_sec
                        {

                            employee_id = p.emp_id,
                            blood_group = Convert.ToByte(p.blood_group_id),
                            permanent_address_line_one = p.permenant_address1 ?? "",
                            permanent_address_line_two = p.permenant_address2 ?? "",
                            permanent_pin_code = p.permenant_pincode,
                            permanent_country = p.country_id,
                            permanent_city = p.city_id ?? 0,
                            permanent_state = p.state_id,
                            corresponding_address_line_one = p.current_address1 ?? "",
                            corresponding_address_line_two = p.current_address2 ?? "",
                            corresponding_pin_code = p.current_pincode,
                            corresponding_city = p.city_id ?? 0,
                            corresponding_state = p.state_id,
                            corresponding_country = p.country_id,
                            is_emg_same_as_permanent = 0,
                            emergency_contact_pin_code = 0,
                            emergency_contact_city = 0,
                            emergency_contact_state = 0,
                            emergency_contact_country = 0,
                            is_deleted = 0,
                            created_by = _clsCurrentUser.UserId,
                            created_date = _CurrentDateTime,
                            last_modified_by = _clsCurrentUser.UserId,
                            last_modified_date = _CurrentDateTime,
                        }).ToList();
                        mdlpersonalnew.ForEach(q =>
                        {
                            var tempdata = mdlpersonal.FirstOrDefault(p => p.employee_id == q.employee_id);
                            if (tempdata != null)
                            {
                                q.emergency_contact_relation = tempdata.emergency_contact_relation;
                                q.emergency_contact_mobile_number = tempdata.emergency_contact_mobile_number;
                                q.emergency_contact_line_one = tempdata.emergency_contact_line_one;
                                q.emergency_contact_line_two = tempdata.emergency_contact_line_two;
                                q.emergency_contact_name = tempdata.emergency_contact_name;
                                q.emergency_contact_city = tempdata.emergency_contact_city;
                                q.emergency_contact_country = tempdata.emergency_contact_country;
                                q.emergency_contact_state = tempdata.emergency_contact_state;
                                q.emergency_contact_address_proof_document = tempdata.emergency_contact_address_proof_document;
                                q.corresponding_state = tempdata.corresponding_state;
                                q.corresponding_country = tempdata.corresponding_country;
                                q.corresponding_pin_code = tempdata.corresponding_pin_code;
                                q.emergency_contact_pin_code = tempdata.emergency_contact_pin_code;
                                q.blood_group_doc = tempdata.blood_group_doc;
                            }
                        });
                        _context.tbl_emp_personal_sec.AddRange(mdlpersonalnew);
                        _context.SaveChanges();


                        //Esic Section
                        var mdlesic = _context.tbl_emp_esic_details.Where(p => emp_ids.Contains(p.employee_id ?? 0) && p.is_deleted == 0).ToList();
                        mdlesic.ForEach(p => { p.is_deleted = 1; p.last_modified_by = _clsCurrentUser.UserId; p.last_modified_date = _CurrentDateTime; });
                        _context.tbl_emp_esic_details.UpdateRange(mdlesic);
                        _context.tbl_emp_esic_details.AddRange(objdblist.Select(p => new tbl_emp_esic_details
                        {

                            is_esic_applicable = Convert.ToByte(p.Is_ESIC_applicable),
                            employee_id = p.emp_id,
                            esic_number = p.ESIC_number ?? "",
                            created_by = _clsCurrentUser.UserId,
                            last_modified_by = _clsCurrentUser.UserId,
                            last_modified_date = _CurrentDateTime,
                            created_date = _CurrentDateTime,
                            is_deleted = 0,
                        }));
                        _context.SaveChanges();


                        // Salary Group Sec
                        var mdsalary_group = _context.tbl_sg_maping.Where(p => emp_ids.Contains(p.emp_id ?? 0) && p.is_active == 1).ToList();
                        mdsalary_group.ForEach(p =>
                        {
                            p.is_active = 0; p.modified_by = _clsCurrentUser.UserId; p.modified_dt = _CurrentDateTime;
                        });
                        _context.tbl_sg_maping.UpdateRange(mdsalary_group);

                        _context.tbl_sg_maping.AddRange(objdblist.Select(p => new tbl_sg_maping
                        {

                            salary_group_id = Convert.ToByte(p.salary_group_id),
                            emp_id = p.emp_id,
                            applicable_from_dt = new DateTime(_CurrentDateTime.Year, _CurrentDateTime.Month, _CurrentDateTime.Day),
                            created_by = _clsCurrentUser.UserId,
                            modified_by = _clsCurrentUser.UserId,
                            modified_dt = _CurrentDateTime,
                            created_dt = _CurrentDateTime,
                            is_active = 1,
                        }));
                        _context.SaveChanges();


                        // PF Section
                        var mdpf = _context.tbl_emp_pf_details.Where(p => emp_ids.Contains(p.employee_id ?? 0) && p.is_deleted == 0).ToList();
                        mdpf.ForEach(p => { p.is_deleted = 1; p.last_modified_by = _clsCurrentUser.UserId; p.last_modified_date = _CurrentDateTime; });
                        _context.tbl_emp_pf_details.UpdateRange(mdpf);

                        _context.tbl_emp_pf_details.AddRange(objdblist.Select(p => new tbl_emp_pf_details
                        {

                            is_pf_applicable = Convert.ToByte(p.IS_PF_applicable),
                            pf_group = (enmPFGroup)p.pf_group_id,
                            pf_celing = p.pf_celing_value,
                            pf_number = p.PF_number ?? "",
                            uan_number = p.UAN_number ?? "",
                            is_vpf_applicable = 0,
                            vpf_Group = 0,
                            is_pt_applicable = Convert.ToByte(p.is_PT_applicable),
                            employee_id = p.emp_id,
                            created_by = _clsCurrentUser.UserId,
                            last_modified_by = _clsCurrentUser.UserId,
                            last_modified_date = _CurrentDateTime,
                            created_date = _CurrentDateTime,
                            is_deleted = 0,
                        }));
                        _context.SaveChanges();

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        if (ex.InnerException != null)
                        {
                            throw ex.InnerException;
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;
        }

        public int SaveSalaryRevisionFromExcel(List<SalaryInput> objlst)
        {
            int result = 0;
            try
            {
                //start Check month salary of effective from date is freezed or not.
                int CurrentMonthYear = Convert.ToInt32(DateTime.Now.ToString("yyyyMM"));

                if (objlst.Any(x => x.monthyear <= CurrentMonthYear))
                {
                    var _salarycalculate = _context.tbl_payroll_process_status.Where(x => x.is_deleted == 0 && (x.is_calculated == 1 || x.is_freezed == 1 || x.is_lock == 1) && objlst.Any(y => y.emp_id == x.emp_id && y.monthyear == x.payroll_month_year)).FirstOrDefault();
                    if (_salarycalculate != null)
                    {
                        result = 2; // Freezed or Lock
                        return result;
                    }
                }
                //end

                using (var trans = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var exist = _context.tbl_emp_salary_master.Where(x => objlst.Any(y => y.emp_id == x.emp_id && Convert.ToDateTime("01-" + y.monthyear.ToString().Substring(4, 2) + "-" + y.monthyear.ToString().Substring(0, 4)) == x.applicable_from_dt.Date)).ToList();

                        if (exist.Count > 0)
                        {
                            exist.ForEach(y => { y.is_active = 0; y.modified_by = objlst.FirstOrDefault().created_by; y.modified_dt = DateTime.Now; });
                        }

                        List<mdlSalaryRevision> _emp_salary = new List<mdlSalaryRevision>();

                        objlst.ForEach(x =>
                        {
                            DateTime _applicable = Convert.ToDateTime(x.monthyear.ToString().Substring(0, 4) + "-" + x.monthyear.ToString().Substring(4, 2) + "-01");
                            List<SalaryComponent> _lst = GetComponent_Value(x);

                            mdlSalaryRevision _salcomp = new mdlSalaryRevision();
                            _salcomp.company_id = x.company_id;
                            _salcomp._applicabledt = _applicable;
                            _salcomp._Salcomponent = _lst;
                            _salcomp.gross_salary = x.component_value;
                            _emp_salary.Add(_salcomp);
                        });




                        _emp_salary.ForEach(x =>
                        {
                            List<tbl_emp_salary_master> _lst = x._Salcomponent.Select(a => new tbl_emp_salary_master
                            {
                                emp_id = a.emp_id,
                                component_id = a.component_id,
                                applicable_value = Convert.ToDecimal(a.compValue),
                                salaryrevision = x.gross_salary,
                                applicable_from_dt = x._applicabledt,
                                is_active = 1,
                                created_dt = DateTime.Now,
                                created_by = _clsCurrentUser.EmpId,
                                modified_by = 0,
                                modified_dt = new DateTime(2000, 01, 01)
                            }).ToList();

                            _context.tbl_emp_salary_master.AddRange(_lst);

                        });

                        _context.SaveChanges();
                        trans.Commit();

                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        result = 1;
                        return result;
                    }
                }

            }
            catch (Exception ex)
            {
                result = 1;
            }
            return result;

        }


        public List<SalaryComponent> GetComponent_Value(SalaryInput obj_salary)
        {
            int salary_component_id = _context.tbl_component_master.Where(a => a.is_active == 1 && a.component_name == "@GrossSalary_sys" && a.is_system_key == 1).Select(a => a.component_id).FirstOrDefault();


            clsComponentValues cc = new clsComponentValues(obj_salary.company_id, _context, obj_salary.emp_id, obj_salary.monthyear, Convert.ToInt32(salary_component_id), obj_salary.salary_group_id, Convert.ToString(obj_salary.component_value), _config);

            List<mdlSalaryInputValues> salaryvalues = cc.CalculateComponentValues(false).ToList();
            List<tbl_component_master> tcm = _context.tbl_component_master.Where(a => a.is_salary_comp == 1 && a.is_active == 1).ToList();
            var result = (from a in tcm
                          join S in salaryvalues
                          on a.component_id equals S.compId into Group
                          from s in Group
                          where a.is_salary_comp == 1 && a.is_active == 1
                          select new SalaryComponent
                          {
                              component_id = a.component_id,
                              property_details = a.property_details,
                              applicable_value = "0",
                              compValue = s.compValue,
                              is_salary_comp = a.is_salary_comp,
                              is_active = a.is_active,
                              emp_id = obj_salary.emp_id,

                          }).ToList();
            return result;
        }


    }
#endif


}

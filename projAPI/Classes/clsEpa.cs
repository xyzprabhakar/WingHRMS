using projContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projAPI.Model;
using Microsoft.EntityFrameworkCore;
using projContext.DB;
using Microsoft.AspNetCore.Http;

namespace projAPI.Classes
{

#if(false)
    public class KeyArea
    {
        public int key_area_id { get; set; }
        public int? company_id { get; set; }
        public int? f_year_id { get; set; } // fiscal_year_id
        public int? w_r_id { get; set; } // working_role_id
        public int? otype_id { get; set; }//objective_type id
        public string key_area { get; set; } // need to 
        public string expected_result { get; set; }
        public int wtg { get; set; }
        public int created_by { get; set; }
        public List<int> criteria_number { get; set; }
        public List<string> criteria { get; set; }

        public string comment { get; set; }

    }





    #region *************** EPA Submission (Prabhakar)************************************


    public class EPASubmission
    {
        private readonly IHttpContextAccessor _AC;
        private readonly List<int> emp_id_ = null;
        private readonly int fiscalyearid_, login_emp_id;
        private readonly int cycle_id_;
        private readonly Context _context;
        private readonly int company_id_;
        private readonly string cycle_name_;
        private readonly string fiscalyear_name_;
        public string exp_msg_;
       
        public string GetCyclename()
        {
            var epacycle = _context.tbl_epa_cycle_master.FirstOrDefault(p => p.is_deleted == 0);
            if (epacycle != null)
            {
                switch (epacycle.cycle_type)
                {
                    case 0:
                        switch (cycle_id_)
                        {
                            case 1: return "Jan";
                            case 2: return "Feb";
                            case 3: return "Mar";
                            case 4: return "Apr";
                            case 5: return "May";
                            case 6: return "Jun";
                            case 7: return "Jul";
                            case 8: return "Aug";
                            case 9: return "Sep";
                            case 10: return "Oct";
                            case 11: return "Nov";
                            case 12: return "Dec";
                            default: this.exp_msg_ = "Invalid Cycle ID"; throw new Exception(this.exp_msg_);
                        };
                    case 1:
                        switch (cycle_id_)
                        {
                            case 1: return "Quater 1";
                            case 2: return "Quater 2";
                            case 3: return "Quater 3";
                            case 4: return "Quater 4";
                            default: this.exp_msg_ = "Invalid Cycle ID"; throw new Exception(this.exp_msg_);
                        }
                    case 2:
                        switch (cycle_id_)
                        {
                            case 1: return "Cycle 1";
                            case 2: return "Cycle 2";
                            default: this.exp_msg_ = "Invalid Cycle ID"; throw new Exception(this.exp_msg_);
                        }
                    case 3:
                        switch (cycle_id_)
                        {
                            case 1: return "Cycle 1";
                            default: this.exp_msg_ = "Invalid Cycle ID"; throw new Exception(this.exp_msg_);
                        }

                }
                this.exp_msg_ = "ePA Cycle not defined";
                throw new Exception(this.exp_msg_);

            }
            else
            {
                this.exp_msg_ = "ePA Cycle not defined";
                throw new Exception(this.exp_msg_);
            }
        }

        public EPASubmission(List<int> emp_id, int fiscalyearid, int cycle_id, Context context, int company_id, string cycle_name,IHttpContextAccessor AC,int login_emp_id)
        {
            _context = context;
            var tempdata = _context.tbl_epa_fiscal_yr_mstr.FirstOrDefault(p => p.fiscal_year_id == fiscalyearid && p.is_deleted == 0);
            if (tempdata == null)
            {
                this.exp_msg_ = "Invalid Fiscal year";
                throw new Exception(exp_msg_);
            }
            {
                fiscalyear_name_ = tempdata.financial_year_name;
            }
            emp_id_ = emp_id;
            cycle_id_ = cycle_id;
            fiscalyearid_ = fiscalyearid;
            if (cycle_name.Trim() == "")
            {
                cycle_name_ = GetCyclename();
            }
            else
            {
                cycle_name_ = cycle_name;
            }

            company_id_ = company_id;

            _AC = AC;
            this.login_emp_id = login_emp_id;

            //register delegates            
        }

        public async Task<List<mdlEpaSubmission>> GetEPASubmissionData()
        {
            bool haveSubmissionData = true;
            List<mdlEpaSubmission> varEpaSubmissions = new List<mdlEpaSubmission>();
            if (emp_id_ == null || emp_id_.Count == 0)
            {
                return varEpaSubmissions;
            }
            //Check Wheather the data is already save or not
            varEpaSubmissions = _context.tbl_epa_submission.Where(p => p.fiscal_year_id == fiscalyearid_ && p.cycle_id == cycle_id_ && emp_id_.Contains(p.emp_id ?? 0) && p.is_deleted == 0)
                .Select(p => new mdlEpaSubmission
                {
                    submission_id = p.submission_id,
                    company_id = p.company_id,
                    emp_id = p.emp_id,
                    emp_off_id = p.emp_off_id,
                    fiscal_year_id = p.fiscal_year_id,
                    cycle_id = p.cycle_id,
                    cycle_name = p.cycle_name,
                    desig_id = p.desig_id,
                    department_id = p.department_id,
                    epa_start_date = p.epa_start_date,
                    epa_end_date = p.epa_end_date,
                    epa_close_status = p.epa_close_status,
                    epa_close_status_name = (p.epa_close_status == 0 ? "Not initiated" : p.epa_close_status == 1 ? "Closed" : "Processing"),
                    epa_current_status = p.epa_current_status,
                    w_r_id = p.w_r_id,
                    total_score = p.total_score,
                    get_score = p.get_score,
                    final_review = p.final_review,
                    final_remarks = p.final_remarks,
                    final_review_rm1 = p.final_review_rm1,
                    final_remarks_rm1 = p.final_remarks_rm1,
                    final_review_rm2 = p.final_review_rm2,
                    final_remarks_rm2 = p.final_remarks_rm2,
                    final_review_rm3 = p.final_review_rm3,
                    final_remarks_rm3 = p.final_remarks_rm3,
                    rm_id1 = p.rm_id1,
                    rm_id2 = p.rm_id2,
                    rm_id3 = p.rm_id3,
                }).ToList();
            if (varEpaSubmissions == null || varEpaSubmissions.Count == 0 || varEpaSubmissions.Count != emp_id_.Count)
            {
                haveSubmissionData = false;


                var tbl_epa_cycle_master = _context.tbl_epa_cycle_master.OrderByDescending(y=>y.cycle_id).FirstOrDefault(p =>p.company_id==company_id_ && p.is_deleted == 0);
                DateTime startdate = tbl_epa_cycle_master!=null? tbl_epa_cycle_master.from_date:DateTime.Now.Date;
                DateTime endDate = tbl_epa_cycle_master!=null?startdate.AddDays(tbl_epa_cycle_master.number_of_day):DateTime.Now.Date;

                var epa_sub_emp_id=varEpaSubmissions.Select(x=>x.emp_id).ToList();
                
                List<mdlEpaSubmission> varEpaSubmissions_temp = _context.tbl_emp_officaial_sec.Where(p => emp_id_.Contains(p.employee_id ?? 0) && !epa_sub_emp_id.Contains(p.employee_id??0) && p.is_deleted == 0 && !string.IsNullOrEmpty(p.employee_first_name))
                    .Select(p => new mdlEpaSubmission
                    {
                        submission_id = 0,
                        emp_name = string.Format("{0} {1} {2}",p.employee_first_name,p.employee_middle_name,p.employee_last_name),//string.Concat(p.employee_first_name, " ", p.employee_middle_name, " ", p.employee_last_name),
                        emp_code = "",
                        //joining_dt = p.date_of_joining,
                        company_id = company_id_,
                        emp_id = p.employee_id,
                        emp_off_id = p.emp_official_section_id,
                        fiscal_year_id = fiscalyearid_,
                        cycle_id = cycle_id_,
                        cycle_name = cycle_name_,
                        desig_id = null,
                        //department_id = p.department_id,
                        epa_start_date = startdate,
                        epa_end_date = endDate,
                        epa_close_status = 0,
                        epa_close_status_name = "Not initiated",
                        epa_current_status = null,
                        w_r_id = null,
                        total_score = 0,
                        get_score = 0,
                        final_review = 0,
                        final_remarks = "",
                        final_review_rm1 = 0,
                        final_remarks_rm1 = "",
                        final_review_rm2 = 0,
                        final_remarks_rm2 = "",
                        final_review_rm3 = 0,
                        final_remarks_rm3 = "",
                        rm_id1 = null,
                        rm_id2 = null,
                        rm_id3 = null,
                    }).ToList();
                //get Emp Working Role
                var emp_working_role_allocation = _context.tbl_emp_working_role_allocation.Where(p => emp_id_.Contains(p.employee_id ?? 0) && p.is_deleted == 0).ToList();
                //Get EMP Default Role
                //Also set the emp Designation
                var emp_desi_allocation = _context.tbl_emp_desi_allocation.Where(p => emp_id_.Contains(p.employee_id ?? 0) && p.applicable_from_date <= startdate && p.applicable_to_date >= startdate).ToList();
                //set EmpRM
                var emp_managers = _context.tbl_emp_manager.Where(p => emp_id_.Contains(p.employee_id ?? 0) && p.is_deleted == 0);
                //set EPA Default Status
                var emp_epa_status = _context.tbl_epa_status_master.FirstOrDefault(p => p.is_deleted == 0 && p.status == 0);
               // var login_emp_idd = _AC.HttpContext.User.Claims.Where(g => g.Type == "empid").FirstOrDefault();

                

                
                if (emp_epa_status == null)
                {
                    this.exp_msg_ = "Default EPA Status not defined";
                    throw new Exception(exp_msg_);
                }
                varEpaSubmissions_temp.ForEach(p =>
                {

                    var wrorkingrole = emp_working_role_allocation.FirstOrDefault(q => q.employee_id == p.emp_id);
                    if (wrorkingrole != null) { p.w_r_id = wrorkingrole.work_r_id; }
                    var emp_desg = emp_desi_allocation.FirstOrDefault(q => q.employee_id == p.emp_id);
                    if (emp_desg != null) { p.desig_id = emp_desg.desig_id; }
                    var emp_manager = emp_managers.FirstOrDefault(q => q.employee_id == p.emp_id);
                    if (emp_manager != null) { p.rm_id1 = emp_manager.m_one_id;}
                    if (login_emp_id !=0)
                    {
                        if ((p.rm_id1 == login_emp_id && emp_epa_status.display_for_rm1 == 1) || (p.rm_id2 == login_emp_id && emp_epa_status.display_for_rm2 == 1)
                    || (p.rm_id3 == login_emp_id && emp_epa_status.display_for_rm3 == 1) || (p.emp_id == login_emp_id && emp_epa_status.display_for == 1))
                        {
                            p.epa_current_status = emp_epa_status.epa_status_id;
                        }
                        else
                        {
                            p.epa_current_status = 0;
                        }
                    }
                    else
                    {
                        p.epa_current_status = 0;
                    }
                    

                    //p.epa_current_status = emp_epa_status.epa_status_id;
                });

                if (varEpaSubmissions_temp.Any(p => p.w_r_id == null))
                {
                    var empDefaultRoleID = _context.tbl_working_role.FirstOrDefault(p => p.is_default == 1 && p.is_active==1);
                    if (empDefaultRoleID == null)
                    {
                        this.exp_msg_ = "default Working role is not defined";
                        throw new Exception(this.exp_msg_);
                    }
                    varEpaSubmissions_temp.Where(p => p.w_r_id == null).ToList().ForEach(p => p.w_r_id = empDefaultRoleID.working_role_id);
                }

                varEpaSubmissions.AddRange(varEpaSubmissions_temp);

                //supriya
                varEpaSubmissions.ForEach(p => {
                    var _context = new projContext.Context();

                    var empnamecode = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && epa_sub_emp_id.Contains(p.emp_id) && x.employee_id==p.emp_id && !string.IsNullOrEmpty(x.employee_first_name)).FirstOrDefault();
                    if (empnamecode != null)
                    {
                        p.emp_name = string.Format("{0} {1} {2}", empnamecode.employee_first_name, empnamecode.employee_middle_name, empnamecode.employee_last_name);
                       // p.joining_dt = empnamecode.date_of_joining;
                    }

                });
                //supriya

            }

            //set EmpName
            Task setEmpnmae = null;
            if (haveSubmissionData)
            {
                setEmpnmae = Task.Run(() => setEmployeeName(varEpaSubmissions));
            }
            else
            {
                setEmpnmae = Task.CompletedTask;
            }

            try
            {
                //Set Emp Code
                Task setEmpCode = Task.Run(() => setEmployeeCode(varEpaSubmissions));

                setEmpCode.Wait();
                Task setEmpDesignation = Task.Run(() => setEmployeeDesignation(varEpaSubmissions));
                setEmpDesignation.Wait();
                Task setEmpDepartment = Task.Run(() => setEmployeeDepartment(varEpaSubmissions));
                setEmpDepartment.Wait();
                Task setEmpManager = Task.Run(() => setEmployeeManager(varEpaSubmissions));
                setEmpManager.Wait();
                Task setEmpEPAStatusName = Task.Run(() => setEmployeeEPAStatusName(varEpaSubmissions));
                setEmpEPAStatusName.Wait();
                Task setEmpKPI = Task.Run(() => setEmployeeKPI(varEpaSubmissions));
                setEmpKPI.Wait();
                Task setEmpKRAStatus = Task.Run(() => setEmployeeKRAStatus(varEpaSubmissions));
                setEmpKRAStatus.Wait();
                Task setEmpTabQuestion = Task.Run(() => setEmployeeTabQuestion(varEpaSubmissions));
                setEmpTabQuestion.Wait();
                await setEmpnmae; await setEmpCode; await setEmpDesignation; await setEmpDepartment;
                await setEmpManager; await setEmpEPAStatusName;
                await setEmpKPI; await setEmpKRAStatus; await setEmpTabQuestion;

                //start add by supriya to bind next status from current epa status 

                Task setEPAStatus = Task.Run(() => SetEPAStatus(varEpaSubmissions));
                setEPAStatus.Wait();

                await setEPAStatus;

                //end add by supriya to bind next status from current epa status 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //if (final_idd == -1)
            // {
            return varEpaSubmissions;
            //}
            //else
            //{
            //    varEpaSubmissions.RemoveAll(p => p.final_review != final_idd);
            //    return varEpaSubmissions;
            //}


        }

        private async Task setEmployeeCode(List<mdlEpaSubmission> varEpaSubmissions)
        {
            var _context = new projContext.Context();

            var empcodes = _context.tbl_employee_master.Where(p => emp_id_.Contains(p.employee_id)).ToList();
            varEpaSubmissions.ForEach(p =>
            {
                p.fiscal_year_name = fiscalyear_name_;
                p.cycle_name = cycle_name_;
                var emp = empcodes.FirstOrDefault(q => q.employee_id == p.emp_id);
                if (emp != null) { p.emp_code = emp.emp_code; }
            });

        }

        private async Task setEmployeeName(List<mdlEpaSubmission> varEpaSubmissions)
        {
            var _context = new projContext.Context();
            var empcodes = _context.tbl_emp_officaial_sec.Where(p => emp_id_.Contains(p.employee_id ?? 0) && p.is_deleted==0).ToList();
            varEpaSubmissions.ForEach(p =>
            {

                var emp = empcodes.FirstOrDefault(q => q.employee_id == p.emp_id && !string.IsNullOrEmpty(q.employee_first_name));
                if (emp != null)
                {
                    p.emp_name = string.Concat(emp.employee_first_name, " ", emp.employee_middle_name, " ", emp.employee_last_name);
                    //p.joining_dt = emp.date_of_joining;
                }
            });
        }

        private async Task setEmployeeDesignation(List<mdlEpaSubmission> varEpaSubmissions)
        {
            var _context = new projContext.Context();
            int[] arrDesig = varEpaSubmissions.Select(p => p.desig_id ?? 0).Distinct().ToArray();
            var varesignation = _context.tbl_designation_master.Where(p => arrDesig.Contains(p.designation_id));

            varEpaSubmissions.ForEach(p =>
            {
                var tempdata = varesignation.FirstOrDefault(q => q.designation_id == p.desig_id);
                if (tempdata != null) { p.desig_name = tempdata.designation_name; }
            });
        }

        private async Task setEmployeeDepartment(List<mdlEpaSubmission> varEpaSubmissions)
        {
            var _context = new projContext.Context();
            int[] arrDesig = varEpaSubmissions.Select(p => p.department_id ?? 0).Distinct().ToArray();
            var varesignation = _context.tbl_department_master.Where(p => arrDesig.Contains(p.department_id));

            varEpaSubmissions.ForEach(p =>
            {
                var tempdata = varesignation.FirstOrDefault(q => q.department_id == p.department_id);
                if (tempdata != null) { p.department_name = tempdata.department_name; }
            });
        }

        private async Task setEmployeeManager(List<mdlEpaSubmission> varEpaSubmissions)
        {
            var _context = new projContext.Context();
            List<int> ManagerIDs = new List<int>();
            ManagerIDs.AddRange(varEpaSubmissions.Select(p => p.rm_id1 ?? 0).Distinct().ToList());
            ManagerIDs.AddRange(varEpaSubmissions.Select(p => p.rm_id2 ?? 0).Distinct().ToList());
            ManagerIDs.AddRange(varEpaSubmissions.Select(p => p.rm_id3 ?? 0).Distinct().ToList());
            var empNames = _context.tbl_emp_officaial_sec.Where(p => ManagerIDs.Contains(p.employee_id ?? 0) && p.is_deleted == 0).ToList();
            var empcodes = _context.tbl_employee_master.Where(p => ManagerIDs.Contains(p.employee_id)).ToList();
            varEpaSubmissions.ForEach(p =>
            {
                var emp1 = empNames.FirstOrDefault(q => q.employee_id == p.rm_id1);
                if (emp1 != null)
                {
                    var empcode = empcodes.FirstOrDefault(q => q.employee_id == p.rm_id1);
                    p.rm1_name = string.Concat(empcode.emp_code, " ", emp1.employee_first_name, " ", emp1.employee_middle_name, " ", emp1.employee_last_name);
                }
                var emp2 = empNames.FirstOrDefault(q => q.employee_id == p.rm_id2);
                if (emp2 != null)
                {
                    var empcode = empcodes.FirstOrDefault(q => q.employee_id == p.rm_id2);
                    p.rm2_name = string.Concat(empcode.emp_code, " ", emp2.employee_first_name, " ", emp2.employee_middle_name, " ", emp2.employee_last_name);
                }
                var emp3 = empNames.FirstOrDefault(q => q.employee_id == p.rm_id3);
                if (emp3 != null)
                {
                    var empcode = empcodes.FirstOrDefault(q => q.employee_id == p.rm_id3);
                    p.rm3_name = string.Concat(empcode.emp_code, " ", emp3.employee_first_name, " ", emp3.employee_middle_name, " ", emp3.employee_last_name);
                }
            });
        }

        private async Task setEmployeeEPAStatusName(List<mdlEpaSubmission> varEpaSubmissions)
        {
            var _context = new projContext.Context();
            var emp_epa_status = _context.tbl_epa_status_master.Where(p => p.is_deleted == 0).ToList();
            int[] arrStatus = varEpaSubmissions.Select(p => p.epa_current_status ?? 0).Distinct().ToArray();
            //get the Working role name
            int[] workingrole = varEpaSubmissions.Select(p => p.w_r_id ?? 0).Distinct().ToArray();
            var emp_epa_working_role = _context.tbl_working_role.Where(p => workingrole.Contains(p.working_role_id)).ToList();
            varEpaSubmissions.ForEach(p =>
            {
                var tempdata = emp_epa_status.FirstOrDefault(q => q.epa_status_id == p.epa_current_status);
                if (tempdata != null) { p.epa_current_status_name = tempdata.status_name; }
                var tempdata1 = emp_epa_working_role.FirstOrDefault(q => q.working_role_id == p.w_r_id);
                if (tempdata1 != null) { p.w_r_name = tempdata1.working_role_name; }
            });
        }

        private async Task setEmployeeKPI(List<mdlEpaSubmission> varEpaSubmissions)
        {
            var _context = new projContext.Context();
            //load the KPI of selected Employee
            int[] arrSubmissionId = varEpaSubmissions.Select(p => p.submission_id).Distinct().ToArray();
            var task_epa_kpi_submission_data = _context.tbl_epa_kpi_submission.Where(p => arrSubmissionId.Contains(p.submission_id ?? 0) && p.is_deleted == 0).ToListAsync();
            //Load the All KPI Selected
            int[] arrworkingrole = varEpaSubmissions.Select(p => p.w_r_id ?? 0).Distinct().ToArray();
            var task_all_epa_kpi_submission_data = _context.tbl_kpi_key_area_master.Where(p => arrworkingrole.Contains(p.w_r_id ?? 0) && p.is_deleted==0).Include(p => p.tbl_kpi_criteria_master).ToListAsync();

            List<tbl_kpi_objective_type> epa_kpi_obective_type = _context.tbl_kpi_objective_type.Where(x=>x.is_deleted==0).ToList();
            List<tbl_epa_kpi_submission> epa_kpi_submission_datas = await task_epa_kpi_submission_data;
            List<tbl_kpi_key_area_master> all_epa_kpi_datas = await task_all_epa_kpi_submission_data;
            varEpaSubmissions.ForEach(p =>
            {
                List<mdlEpaKPIDetail> mdlEpaKPIDetails = new List<mdlEpaKPIDetail>();
                mdlEpaKPIDetails.AddRange(
                    epa_kpi_submission_datas.Where(q => q.submission_id == p.submission_id)
                .Select(q => new mdlEpaKPIDetail
                {
                    kpi_submission_id = q.kpi_submission_id,
                    key_area_id = q.key_area_id ?? 0,
                    self = q.self,
                    aggreed = q.aggreed,
                    score = q.score,
                    comment = q.comment,
                }));

                if (p.epa_close_status != 1)
                {
                    int[] arrKeyAreayid = mdlEpaKPIDetails.Select(q => q.key_area_id).Distinct().ToArray();
                    //get distinct Criteria id
                    mdlEpaKPIDetails.AddRange(all_epa_kpi_datas.Where(q => q.w_r_id == p.w_r_id && q.is_deleted == 0 && 
                    !arrKeyAreayid.Contains(q.key_area_id)).Select(q => new mdlEpaKPIDetail
                      {
                         kpi_submission_id = 0,
                         key_area_id = q.key_area_id,
                         self = 0,
                         aggreed = 0,
                         score = 0,
                         comment = q.comment,
                     }));

                }
                
                //Load the objective type
                mdlEpaKPIDetails.ForEach(q =>
                {
                    var all_epa_kpi_data = all_epa_kpi_datas.FirstOrDefault(r => r.key_area_id == q.key_area_id);
                    if (all_epa_kpi_data != null)
                    {

                        q.otype_id = all_epa_kpi_data.otype_id;
                        q.key_area = all_epa_kpi_data.key_area;
                        q.expected_result = all_epa_kpi_data.expected_result;
                        q.wtg = all_epa_kpi_data.wtg;
                        q.mdlEpaKPICriteriaDetails = all_epa_kpi_data.tbl_kpi_criteria_master.Where(r => r.is_deleted == 0)
                        .Select(r => new mdlEpaKPICriteriaDetail
                        {
                            crit_id = r.crit_id,
                            crit_number = r.criteria_number,
                            criteria_name = r.criteria
                        }).ToList();
                        //set the objective type name
                        var quot = epa_kpi_obective_type.FirstOrDefault(r => r.obj_type_id == q.otype_id);
                        if (quot != null)
                        {
                            q.objective_name = quot.objective_name;
                        }
                        q.comment = all_epa_kpi_data.comment;
                    }
                    p.mdlEpaKPIDetails = mdlEpaKPIDetails;
                });
            });
        }

        private async Task setEmployeeKRAStatus(List<mdlEpaSubmission> varEpaSubmissions)
        {
            var _context = new projContext.Context();
            //load the KPI of selected Employee
            int[] arrSubmissionId = varEpaSubmissions.Select(p => p.submission_id).Distinct().ToArray();
            var task_epa_kra_submission_data = _context.tbl_epa_kra_submission.Where(p => arrSubmissionId.Contains(p.submission_id ?? 0) && p.is_deleted == 0).ToListAsync();
            //Load the All KPI Selected
            int[] arrworkingrole = varEpaSubmissions.Select(p => p.w_r_id ?? 0).Distinct().ToArray();
            var task_all_epa_kra_submission_data = _context.tbl_kra_master.Where(p => arrworkingrole.Contains(p.wrk_role_id ?? 0)).ToListAsync();

            List<tbl_kpi_rating_master> epa_kra_rating = _context.tbl_kpi_rating_master.ToList();
            List<tbl_epa_kra_submission> epa_kra_submission_datas = await task_epa_kra_submission_data;
            List<tbl_kra_master> all_epa_kra_datas = await task_all_epa_kra_submission_data;

            varEpaSubmissions.ForEach(p =>
            {
                List<mdlEpaKRADetail> mdlEpaKRADetails = new List<mdlEpaKRADetail>();
                mdlEpaKRADetails.AddRange(
                    epa_kra_submission_datas.Where(q => q.submission_id == p.submission_id)
                .Select(q => new mdlEpaKRADetail
                {
                    kra_submission_id = q.kra_submission_id,
                    kra_id = q.kra_id ?? 0,
                    description = null,
                    rating_id = q.rating_id,

                }));

                if (p.epa_close_status != 1)
                {
                    int[] arrKeyAreayid = mdlEpaKRADetails.Select(q => q.kra_id ?? 0).Distinct().ToArray();
                    //get distinct Criteria id
                    mdlEpaKRADetails.AddRange(all_epa_kra_datas.Where(q => q.wrk_role_id == p.w_r_id && q.is_deleted == 0 &&
                     !arrKeyAreayid.Contains(q.kra_mstr_id)).Select(q => new mdlEpaKRADetail
                     {
                         kra_submission_id = 0,
                         kra_id = q.kra_mstr_id,
                         description = null,
                         rating_id = null,
                     }));
                }

                //set the all data of KRA
                mdlEpaKRADetails.ForEach(q =>
                {
                    var all_epa_kra_data = all_epa_kra_datas.FirstOrDefault(r => r.kra_mstr_id == q.kra_id);
                    if (all_epa_kra_data != null)
                    {
                        q.factor_result = all_epa_kra_data.factor_result;
                        q.description = all_epa_kra_data.description;

                        //set the objective type name
                        var quot = epa_kra_rating.FirstOrDefault(r => r.kpi_rating_id == q.rating_id);
                        if (quot != null)
                        {
                            q.rating_name = quot.rating_name;
                        }
                    }
                    p.mdlEpaKRADetails = mdlEpaKRADetails;
                });
            });
        }

        private async Task setEmployeeTabQuestion(List<mdlEpaSubmission> varEpaSubmissions)
        {
            var _context = new projContext.Context();
            //load the KPI of selected Employee
            int[] arrSubmissionId = varEpaSubmissions.Select(p => p.submission_id).Distinct().ToArray();
            var task_epa_question_submission_data = _context.tbl_epa_question_submission.Where(p => arrSubmissionId.Contains(p.submission_id ?? 0) && p.is_deleted == 0 && p.tbl_tab_master.is_deleted==0).ToListAsync();
            //Load the All KPI Selected
            int[] arrworkingrole = varEpaSubmissions.Select(p => p.w_r_id ?? 0).Distinct().ToArray();
            var task_all_epa_question_submission_data = _context.tbl_question_master.Where(p => arrworkingrole.Contains(p.wrk_role_id ?? 0) && p.tab_mstr.is_deleted==0).ToListAsync();

            List<tbl_tab_master> epa_tab_data = _context.tbl_tab_master.Where(x=>x.is_deleted==0).ToList();
            List<tbl_epa_question_submission> epa_question_submission_datas = await task_epa_question_submission_data;
            List<tbl_question_master> all_epa_question_datas = await task_all_epa_question_submission_data;

            varEpaSubmissions.ForEach(p =>
            {
                List<mdlEpaTabQuestion> mdlEpaTabQuestions = new List<mdlEpaTabQuestion>();
                mdlEpaTabQuestions.AddRange(
                    epa_question_submission_datas.Where(q => q.submission_id == p.submission_id)
                .Select(q => new mdlEpaTabQuestion
                {
                    question_submission_id = q.question_submission_id,
                    tab_id = q.tab_id,
                    question_id = q.question_id ?? 0,
                    question_answer = q.question_answer,
                    remarks = (q.remarks == null) ? "" : q.remarks,
                   
                }));

                if (p.epa_close_status != 1)
                {
                    int[] arrKeyAreayid = mdlEpaTabQuestions.Select(q => q.question_id ?? 0).Distinct().ToArray();
                    //get distinct Criteria id
                    mdlEpaTabQuestions.AddRange(all_epa_question_datas.Where(q => q.wrk_role_id == p.w_r_id && q.is_deleted == 0 &&
                     !arrKeyAreayid.Contains(q.ques_mstr_id)).Select(q => new mdlEpaTabQuestion
                     {
                         question_submission_id = 0,
                         tab_id = q.tab_id,
                         question_id = q.ques_mstr_id,
                         question_answer = "",
                         remarks = "",
                     }));
                }

                //set the all data of KRA
                mdlEpaTabQuestions.ForEach(q =>
                {
                    var all_epa_kra_data = all_epa_question_datas.FirstOrDefault(r => r.ques_mstr_id == q.question_id);
                    if (all_epa_kra_data != null)
                    {
                        q.ques = all_epa_kra_data.ques;
                        q.ans_type = all_epa_kra_data.ans_type;
                        q.ans_type_ddl = all_epa_kra_data.ans_type_ddl;
                        //set the objective type name
                        var quot = epa_tab_data.FirstOrDefault(r => r.tab_mstr_id == q.tab_id);
                       
                       // var login_emp_idd = _AC.HttpContext.User.Claims.Where(g => g.Type == "empid").FirstOrDefault();

                        if (quot != null)
                        {
                            q.tab_name = quot.tab_name;
                            q.is_manager = ((p.rm_id1 == login_emp_id && quot.for_rm1 == 1) ? 1 : (p.rm_id2== login_emp_id && quot.for_rm2==1)?1:(p.rm_id3==login_emp_id && quot.for_rm3==1)?1:0);
                            q.for_all_emp = quot.for_user == 1 ? 1 : 0;
                        }
                    }
                    p.mdlEpaTabQuestions = mdlEpaTabQuestions;
                });
            });
        }

        public async Task<bool> SaveEPASubmissionData(List<mdlEpaSubmission> mdlEpaSubmissions)
        {

            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    //delete existing  data
                    var _context = new projContext.Context();
                    DateTime dateTime = DateTime.Now;
                    if (emp_id_ == null || emp_id_.Count == 0)
                    {
                        return false;
                    }
                    var tbl_epa_cycle_master = _context.tbl_epa_cycle_master.OrderByDescending(y=>y.cycle_id).FirstOrDefault(p => p.is_deleted == 0 && p.company_id==company_id_);
                    DateTime startdate = tbl_epa_cycle_master!=null? tbl_epa_cycle_master.from_date:DateTime.Now;
                    DateTime endDate = tbl_epa_cycle_master!=null? startdate.AddDays(tbl_epa_cycle_master.number_of_day):DateTime.Now;
                    if (!(DateTime.Now > startdate && DateTime.Now < endDate))
                    {
                        this.exp_msg_ = "invalid ePA for Submission";
                        throw new Exception(this.exp_msg_);
                    }

                    //Check Wheather the data is already save or not
                    var varEpaSubmissions = _context.tbl_epa_submission.Where(p => p.fiscal_year_id == fiscalyearid_ && p.cycle_id == cycle_id_ && emp_id_.Contains(p.emp_id ?? 0) && p.is_deleted == 0).ToList();
                    if (varEpaSubmissions.Any(p => p.epa_close_status == 1))
                    {
                        this.exp_msg_ = "can't update ePA close";
                        throw new Exception(this.exp_msg_);
                    }
                    varEpaSubmissions.ForEach(p => p.is_deleted = 1);
                    _context.tbl_epa_submission.AttachRange(varEpaSubmissions);
                    _context.SaveChanges();

                    //Validate total KPI Criteria with master 
                    Task<bool> tvalidateEmpKPI = Task.Run(() => validateEmployeeKPI(mdlEpaSubmissions));
                    Task<bool> tvalidateEmpKRAStatus = Task.Run(() => validateEmployeeKRA(mdlEpaSubmissions));
                    Task<bool> tvalidateEmpTabQuestion = Task.Run(() => validateEmployeeQuestion(mdlEpaSubmissions));

                    bool validateEmpKPI = await tvalidateEmpKPI;
                    bool validateEmpKRAStatus = await tvalidateEmpKRAStatus;
                    bool validateEmpTabQuestion = await tvalidateEmpTabQuestion;
                    if (!validateEmpKPI)
                    {
                        //this.exp_msg_ = "invalid KPI Data";
                        throw new Exception(this.exp_msg_);
                    }
                    if (!validateEmpKRAStatus)
                    {
                        // this.exp_msg_ = "invalid KPI Data";
                        throw new Exception(this.exp_msg_);
                    }
                    if (!validateEmpKRAStatus)
                    {
                        // this.exp_msg_ = "invalid Question Data";
                        throw new Exception(this.exp_msg_);
                    }

                    var FirstStatus = _context.tbl_epa_status_master.FirstOrDefault(p => p.status == 0 && p.is_deleted == 0);
                    if (FirstStatus == null)
                    {
                        this.exp_msg_ = "FirstStatus Status not defined";
                        throw new Exception(this.exp_msg_);
                    }
                    var LastStatus = _context.tbl_epa_status_master.FirstOrDefault(p => p.status == 1 && p.is_deleted == 0);
                    if (LastStatus == null)
                    {
                        this.exp_msg_ = "LastStatus Status not defined";
                        throw new Exception(this.exp_msg_);
                    }
                    //Now Save the data
                    List<tbl_epa_submission> tbl_epa_submissions;
                    tbl_epa_submissions = mdlEpaSubmissions.Select(p => new tbl_epa_submission
                    {
                        company_id = p.company_id,
                        emp_id = p.emp_id,
                        emp_off_id = p.emp_off_id,
                        fiscal_year_id = p.fiscal_year_id,
                        cycle_id = p.cycle_id,
                        cycle_name = p.cycle_name,
                        desig_id = p.desig_id == 0 ? null : p.desig_id,

                        department_id = p.department_id,
                        epa_start_date = p.epa_start_date,
                        epa_end_date = p.epa_end_date,
                        epa_current_status = p.epa_current_status,
                        epa_close_status = Convert.ToByte((p.epa_current_status == FirstStatus.epa_status_id) ? 0 : (p.epa_current_status == LastStatus.epa_status_id) ? 1 : 2),
                        w_r_id = p.w_r_id,
                        total_score = p.total_score,
                        get_score = p.get_score,
                        final_review = (p.final_review_rm3 != 0 ? p.final_review_rm3 : p.final_review_rm2 != 0 ? p.final_review_rm2 : p.final_review_rm1),
                        final_remarks = (p.final_review_rm3 != 0 ? p.final_remarks_rm3 : p.final_review_rm2 != 0 ? p.final_remarks_rm2 : p.final_remarks_rm1),
                        final_remarks_rm1 = p.final_remarks_rm1,
                        final_remarks_rm2 = p.final_remarks_rm2,
                        final_remarks_rm3 = p.final_remarks_rm3,
                        final_review_rm1 = p.final_review_rm1,
                        final_review_rm2 = p.final_review_rm2,
                        final_review_rm3 = p.final_review_rm3,
                        rm_id1 = p.rm_id1,
                        rm_id2 = p.rm_id2,
                        rm_id3 = p.rm_id3,
                        user_id = p.user_id,
                        submission_dt = dateTime,
                        is_deleted = 0,
                        tbl_epa_kpi_submissions = p.mdlEpaKPIDetails.Select(q => new tbl_epa_kpi_submission
                        {
                            key_area_id = q.key_area_id,
                            self = q.self,
                            aggreed = q.aggreed,
                            score = q.score,
                            comment = q.comment,
                            change_dt = dateTime,
                            is_deleted = 0,
                            tbl_epa_kpi_criteria_submissions = q.mdlEpaKPICriteriaDetails.Select(r => new tbl_epa_kpi_criteria_submission
                            {
                                crit_id = r.crit_id
                            }).ToArray()
                        }).ToArray(),
                        tbl_epa_kra_submissions = p.mdlEpaKRADetails.Select(q => new tbl_epa_kra_submission
                        {
                            kra_id = q.kra_id,
                            rating_id = q.rating_id,
                            change_dt = dateTime,
                            is_deleted = 0,
                        }).ToArray(),
                        tbl_epa_question_submissions = p.mdlEpaTabQuestions.Select(q => new tbl_epa_question_submission
                        {
                            tab_id = q.tab_id,
                            question_id = q.question_id,
                            question_answer = q.question_answer,
                            remarks = q.remarks,
                            change_dt = dateTime,
                            is_deleted = 0,
                        }).ToArray(),
                    }).ToList();

                    _context.tbl_epa_submission.AddRange(tbl_epa_submissions);
                    _context.SaveChanges();
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    return false;
                }
            }
        }


        private async Task<bool> validateEmployeeKPI(List<mdlEpaSubmission> varEpaSubmissions)
        {
            try
            {
                var _context = new projContext.Context();

                //Load the All KPI Selected
                int[] arrworkingrole = varEpaSubmissions.Select(p => p.w_r_id ?? 0).Distinct().ToArray();
                var task_all_epa_kpi_submission_data = _context.tbl_kpi_key_area_master.Where(p => arrworkingrole.Contains(p.w_r_id ?? 0)).Include(p => p.tbl_kpi_criteria_master).ToListAsync();

                List<tbl_kpi_objective_type> epa_kpi_obective_type = _context.tbl_kpi_objective_type.ToList();
                List<tbl_kpi_key_area_master> all_epa_kpi_datas = await task_all_epa_kpi_submission_data;
                varEpaSubmissions.ForEach(p =>
                {
                    //check the total COunt are valid or not
                    int[] arrKeyAreayid = p.mdlEpaKPIDetails.Select(q => q.key_area_id).Distinct().ToArray();
                    if (all_epa_kpi_datas.Any(q => q.w_r_id == p.w_r_id && q.is_deleted == 0 && !arrKeyAreayid.Contains(q.key_area_id)))
                    {
                        this.exp_msg_ = "Some KPI are not define";
                        throw new Exception(this.exp_msg_);

                    }
                    int totalCriteriacount = all_epa_kpi_datas.FirstOrDefault(q => q.is_deleted == 0).tbl_kpi_criteria_master.Where(r => r.is_deleted == 0).Count();

                    if (p.mdlEpaKPIDetails.Any(q => (q.self > totalCriteriacount || q.self < 0)))
                    {
                        this.exp_msg_ = "Invalid KPI self";
                        throw new Exception(this.exp_msg_);

                    }
                    if (p.mdlEpaKPIDetails.Any(q => (q.aggreed > totalCriteriacount || q.aggreed < 0)))
                    {
                        this.exp_msg_ = "Invalid KPI aggred";
                        throw new Exception(this.exp_msg_);

                    }


                    //Load the objective type
                    p.mdlEpaKPIDetails.ForEach(q =>
                        {
                            var all_epa_kpi_data = all_epa_kpi_datas.FirstOrDefault(r => r.key_area_id == q.key_area_id);
                            if (all_epa_kpi_data != null)
                            {
                                if (q.wtg != all_epa_kpi_data.wtg)
                                {
                                    this.exp_msg_ = "There are diffrence in wtg of criteria";
                                    throw new Exception(this.exp_msg_);
                                }
                                else
                                {
                                    q.score = q.wtg * q.aggreed;
                                }
                            }
                            else
                            {
                                this.exp_msg_ = "invalid KRA";
                                throw new Exception(this.exp_msg_);
                                //return isValid;
                            }
                        });
                    p.total_score = totalCriteriacount * p.mdlEpaKPIDetails.Sum(r => r.wtg);
                    p.get_score = p.mdlEpaKPIDetails.Sum(r => r.score);
                });

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> validateEmployeeKRA(List<mdlEpaSubmission> varEpaSubmissions)
        {
            try
            {
                var _context = new projContext.Context();
                //Load the All KPI Selected
                int[] arrworkingrole = varEpaSubmissions.Select(p => p.w_r_id ?? 0).Distinct().ToArray();
                var task_all_epa_kra_submission_data = _context.tbl_kra_master.Where(p => arrworkingrole.Contains(p.wrk_role_id ?? 0)).ToListAsync();

                List<tbl_kpi_rating_master> epa_kra_rating = _context.tbl_kpi_rating_master.ToList();
                List<tbl_kra_master> all_epa_kra_datas = await task_all_epa_kra_submission_data;

                varEpaSubmissions.ForEach(p =>
                {
                    //check the total COunt are valid or not
                    int[] arrKeyAreayid = p.mdlEpaKRADetails.Select(q => q.kra_id ?? 0).Distinct().ToArray();
                    if (all_epa_kra_datas.Any(q => q.wrk_role_id == p.w_r_id && q.is_deleted == 0 && !arrKeyAreayid.Contains(q.kra_mstr_id)))
                    {
                        this.exp_msg_ = "Some KRA are not define";
                        throw new Exception(this.exp_msg_);

                    }



                    //Load the objective type
                    p.mdlEpaKRADetails.ForEach(q =>
                    {
                        var all_epa_kpi_data = all_epa_kra_datas.FirstOrDefault(r => r.kra_mstr_id == q.kra_id);
                        if (all_epa_kpi_data != null)
                        {
                            if (q.rating_id == null || q.rating_id == 0)
                            {
                                if (epa_kra_rating.Where(P => P.is_deleted == 0).Count() == 0)
                                {
                                    this.exp_msg_ = "Rating master not defined";
                                    throw new Exception(this.exp_msg_);
                                }
                                q.rating_id = epa_kra_rating.Where(P => P.is_deleted == 0).First().kpi_rating_id;
                            }

                            if (!epa_kra_rating.Any(r => r.kpi_rating_id == q.rating_id && r.is_deleted == 0))
                            {
                                this.exp_msg_ = "invalid Rating";
                                throw new Exception(this.exp_msg_);
                            }
                        }
                        else
                        {
                            this.exp_msg_ = "invalid KRA";
                            throw new Exception(this.exp_msg_);
                            //return isValid;
                        }
                    });
                });

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> validateEmployeeQuestion(List<mdlEpaSubmission> varEpaSubmissions)
        {
            try
            {
                var _context = new projContext.Context();
                int[] arrworkingrole = varEpaSubmissions.Select(p => p.w_r_id ?? 0).Distinct().ToArray();
                var task_all_epa_question_submission_data = _context.tbl_question_master.Where(p => arrworkingrole.Contains(p.wrk_role_id ?? 0) && p.is_deleted==0 && p.tab_mstr.is_deleted==0).ToListAsync();

                List<tbl_tab_master> epa_tab_data = _context.tbl_tab_master.Where(x=>x.is_deleted==0).ToList();
                List<tbl_question_master> all_epa_question_datas = await task_all_epa_question_submission_data;


                varEpaSubmissions.ForEach(p =>
                {
                    //check the total COunt are valid or not
                    int[] arrKeyAreayid = p.mdlEpaTabQuestions.Select(q => q.question_id ?? 0).Distinct().ToArray();
                    if (all_epa_question_datas.Any(q => q.wrk_role_id == p.w_r_id && q.is_deleted == 0 && !arrKeyAreayid.Contains(q.ques_mstr_id)))
                    {
                        this.exp_msg_ = "Some Question are not define";
                        throw new Exception(this.exp_msg_);
                    }



                    //Load the objective type
                    p.mdlEpaTabQuestions.ForEach(q =>
                    {

                        var all_epa_question_data = all_epa_question_datas.FirstOrDefault(r => r.ques_mstr_id == q.question_id);
                        if (all_epa_question_data != null)
                        {
                            if (all_epa_question_data.tab_id != q.tab_id)
                            {
                                this.exp_msg_ = "invalid Tab ID";
                                throw new Exception(this.exp_msg_);
                            }
                            if (all_epa_question_data.ans_type == 2)
                            {
                                string[] ddloutput = all_epa_question_data.ans_type_ddl.ToLower().Split(",").ToArray();
                                if (ddloutput.Count() > 0)
                                {
                                    q.question_answer = q.question_answer == null ? ddloutput.First() : q.question_answer;
                                    if (!ddloutput.Any(r => r.ToLower().Trim() == q.question_answer))
                                    {
                                        this.exp_msg_ = "invalid answer";
                                        throw new Exception(this.exp_msg_);
                                    }
                                }

                            }
                        }
                        else
                        {
                            this.exp_msg_ = "invalid Question";
                            throw new Exception(this.exp_msg_);
                            //return isValid;
                        }
                    });
                });

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        #region Add by supriya
        private async Task SetEPAStatus(List<mdlEpaSubmission> varEpaSubmissions)
        {

            //var login_emp_idd = _AC.HttpContext.User.Claims.Where(g => g.Type == "empid").FirstOrDefault();

            //if (quot != null)
            //{
            //    q.tab_name = quot.tab_name;
            //    q.is_manager = ((p.rm_id1 == Convert.ToInt32(login_emp_idd.Value) && quot.for_rm1 == 1) ? 1 : (p.rm_id2 == Convert.ToInt32(login_emp_idd.Value) && quot.for_rm2 == 1) ? 1 : (p.rm_id3 == Convert.ToInt32(login_emp_idd.Value) && quot.for_rm3 == 1) ? 1 : 0);
            //    q.for_all_emp = quot.for_user == 1 ? 1 : 0;
            //}

            //var login_emp_id=_AC.HttpContext.User.Claims
            var status_flow_master = _context.tbl_status_flow_master.Where(x => x.is_deleted == 0).Select(p=>new {
                next_status_id=p.next_status_id,
                next_status_name = p.next_status.status_name,
                start_status_id =p.start_status_id,
                start_status_name=p.status_master.status_name,
              start_status_for_user= p.status_master.display_for,
              start_status_for_rm1=p.status_master.display_for_rm1,
              start_status_for_rm2=p.status_master.display_for_rm2,
              start_status_for_rm3=p.status_master.display_for_rm3,
              next_status_for_user=p.next_status.display_for,
              next_status_for_rm1=p.next_status.display_for_rm1,
              next_status_for_rm2=p.next_status.display_for_rm2,
              next_status_for_rm3=p.next_status.display_for_rm3,
            }).ToList();


            varEpaSubmissions.ForEach(p => {

                List<EPAStatus> mdlEPANextStatusDetails = new List<EPAStatus>();
                mdlEPANextStatusDetails.AddRange(
                    status_flow_master.Where(q=>q.start_status_id==p.epa_current_status && 
                    ((p.rm_id1==login_emp_id && q.next_status_for_rm1==1)||(p.rm_id2==login_emp_id && q.next_status_for_rm2==1)
                    ||(p.rm_id3==login_emp_id && q.next_status_for_rm3==1)||(p.emp_id==login_emp_id && q.next_status_for_user==1))
                    ).Select(g=>new EPAStatus{
                        next_status_id = g.next_status_id,
                        next_Status_name=g.next_status_name,
                        current_status_id=g.start_status_id,
                        current_status_name=g.start_status_name
                    }));

                p.mdlEPANextStatusDetails = mdlEPANextStatusDetails;
            });

        }

        #endregion end by supriya
    }



    #endregion

#endif
}

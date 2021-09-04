using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using projAPI.Model;
using projContext;
using projContext.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Classes
{
    public class clsFNFProcess
    {

        public int _company_id;
        private readonly IHttpContextAccessor _AC;
        private readonly Context _context;
        private readonly IConfiguration _config;
        private readonly clsCurrentUser _clsCurrentUser;
        private readonly DateTime _CurrentDateTime;
        public List<int> EmpIDs;
        public int _monthyear;


        public List<int> ReqID;


        public clsFNFProcess(Context context, IConfiguration config, IHttpContextAccessor AC, clsCurrentUser _clsCurrentUser)
        {
            _context = context;
            _config = config;
            _AC = AC;
            this._clsCurrentUser = _clsCurrentUser;
            this._CurrentDateTime = DateTime.Now;
        }

        private List<mdlFNFProcess> Get_EmpBasicFNFData()
        {
            List<mdlFNFProcess> fnf_temp_data = _context.tbl_emp_separation.Where(x => ReqID.Contains(x.sepration_id) && EmpIDs.Contains(x.emp_id)).Select(p => new mdlFNFProcess
            {
                sep_id = p.sepration_id,
                emp_id = p.emp_id,
                company_id = p.company_id,
                company_name = "",
                emp_name = "",
                emp_code = "",
                dept_id = 0,
                dept_name = "",
                desig_id = 0,
                desig_name = "",
                lcoation = "",
                nationality = "",
                //1 temporary,2 Probation,3 Confirmend, 4 Contract, 10 notice,99 FNF,(100 terminate     no entry coreposnding to 100   )
                emptype = "",
                dob = Convert.ToDateTime("01-01-2000"),
                doj = Convert.ToDateTime("01-01-2000"),
                religion = "",
                grade_id = 0,
                grade = "",
                resign_dt = p.resignation_dt,
                last_working_date = p.final_relieve_dt,
                notice_days = p.req_notice_days,
                pf_number = "",
                pan_card_number = "",
                esic_number = "",
            }).ToList();

            var teos = _context.tbl_emp_officaial_sec.Where(x => x.is_deleted == 0 && EmpIDs.Contains(x.employee_id ?? 0)).Select(p => new
            {
                p.emp_official_section_id,
                p.employee_id,
                p.employee_first_name,
                p.employee_middle_name,
                p.employee_last_name,
                p.tbl_employee_id_details.emp_code,
                p.date_of_birth,
                p.date_of_joining,
                p.department_id,
                p.current_employee_type,
                p.tbl_department_master.department_name,
                p.tbl_location_master.location_name,
                p.tbl_religion_master.religion_name,
            }).ToList();

            var emp_grade = _context.tbl_emp_grade_allocation.Where(x => EmpIDs.Contains(x.employee_id ?? 0)).Select(p => new
            {
                p.emp_grade_id,
                p.employee_id,
                p.grade_id,
                p.tbl_grade_master.grade_name,
                p.applicable_from_date,
                p.applicable_to_date
            }).ToList();

            var emp_desig = _context.tbl_emp_desi_allocation.Where(x => EmpIDs.Contains(x.employee_id ?? 0)).Select(p => new
            {
                p.emp_grade_id,
                p.employee_id,
                p.desig_id,
                p.tbl_designation_master.designation_name,
                p.applicable_from_date,
                p.applicable_to_date
            }).ToList();

            var personal_dtl = _context.tbl_emp_personal_sec.Where(x => x.is_deleted == 0 && EmpIDs.Contains(x.employee_id ?? 0)).Select(p => new
            {
                p.employee_id,
                country = _context.tbl_country.FirstOrDefault(x => x.country_id == p.permanent_country).name
            }).ToList();

            var emp_pan_dtl = _context.tbl_emp_pan_details.Where(x => x.is_deleted == 0 && EmpIDs.Contains(x.employee_id ?? 0)).Select(p => new
            {

                p.pan_details_id,
                p.pan_card_number,
                p.pan_card_name,
                p.employee_id,

            }).ToList();

            var emp_pf_dtl = _context.tbl_emp_pf_details.Where(x => x.is_deleted == 0 && EmpIDs.Contains(x.employee_id ?? 0)).Select(p => new
            {
                p.pf_details_id,
                p.pf_number,
                p.employee_id,
                p.is_pf_applicable,
            }).ToList();

            var emp_esic_dtl = _context.tbl_emp_esic_details.Where(x => x.is_deleted == 0 && EmpIDs.Contains(x.employee_id ?? 0)).Select(p => new
            {
                p.esic_details_id,
                p.employee_id,
                p.esic_number,
                p.is_esic_applicable
            }).ToList();

            var emp_fnf_dtl = _context.tbl_fnf_master.Where(x => x.is_deleted == 0 && EmpIDs.Contains(x.emp_id ?? 0) && ReqID.Contains(Convert.ToInt32(x.fkid_empSepration))).Select(p => new
            {
                p.emp_id,
                p.last_working_date,
                p.is_gratuity,
                p.settlement_type,
                p.settlment_amt,
                p.settlment_dt,
                p.remarks,
                p.net_amt,
                p.notice_payment_days,
                p.notice_recovery_days
            }).ToList();

            var company_data = _context.tbl_company_master.Where(x => fnf_temp_data.Any(q => q.company_id == x.company_id)).Select(p => new { p.company_id, p.company_name }).ToList();

            //Get Employee Details
            fnf_temp_data.ForEach(p =>
            {
                var data_ = teos.Where(q => q.employee_id == p.emp_id).FirstOrDefault();
                if (data_ != null)
                {
                    p.emp_name = string.Format("{0} {1} {2}", data_.employee_first_name, data_.employee_middle_name, data_.employee_last_name);
                    p.emp_code = data_.emp_code;
                    p.dept_id = data_.department_id ?? 0;
                    p.dob = data_.date_of_birth;
                    p.doj = data_.date_of_joining;
                    p.lcoation = data_.location_name;
                    p.religion = data_.religion_name;
                    p.dept_name = data_.department_name;
                    switch (data_.current_employee_type)
                    {
                        case 1: p.emptype = "Temporary"; break;
                        case 2: p.emptype = "Probation"; break;
                        case 3: p.emptype = "Confirmed"; break;
                        case 4: p.emptype = "Contract"; break;
                        case 10: p.emptype = "Notice"; break;
                        case 99: p.emptype = "FNF"; break;
                        case 100: p.emptype = "Terminate"; break;
                        default: p.emptype = ""; break;
                    }
                }



                var personal_ = personal_dtl.Where(x => x.employee_id == p.emp_id).FirstOrDefault();
                if (personal_ != null)
                {
                    p.nationality = personal_.country;
                }
                var grade_ = emp_grade.Where(x => x.employee_id == p.emp_id && x.applicable_from_date.Date <= p.resign_dt.Date && p.resign_dt.Date <= x.applicable_to_date.Date).GroupBy(g => g.employee_id).Select(q => q.OrderByDescending(h => h.emp_grade_id).FirstOrDefault()).FirstOrDefault();
                if (grade_ != null)
                {
                    p.grade_id = grade_.grade_id ?? 0;
                    p.grade = grade_.grade_name;
                }

                var desig_ = emp_desig.Where(x => x.employee_id == p.emp_id && x.applicable_from_date.Date <= p.resign_dt.Date && p.resign_dt.Date <= x.applicable_to_date.Date).GroupBy(g => g.employee_id).Select(q => q.OrderByDescending(h => h.emp_grade_id).FirstOrDefault()).FirstOrDefault();
                if (desig_ != null)
                {
                    p.desig_id = desig_.desig_id ?? 0;
                    p.desig_name = desig_.designation_name;
                }

                var comp_data = company_data.Where(x => x.company_id == p.company_id).FirstOrDefault();
                if (company_data != null)
                {
                    p.company_name = comp_data.company_name;
                }

                var pf_ = emp_pf_dtl.Where(x => x.employee_id == p.emp_id).FirstOrDefault();
                if (pf_ != null)
                {
                    p.pf_number = pf_.pf_number;
                }

                var pan_ = emp_pan_dtl.Where(x => x.employee_id == p.emp_id).FirstOrDefault();
                if (pan_ != null)
                {
                    p.pan_card_number = pan_.pan_card_number;
                }

                var esic = emp_esic_dtl.Where(x => x.employee_id == p.emp_id).FirstOrDefault();
                if (esic != null)
                {
                    p.esic_number = esic.esic_number;
                }

                var fnf_data = emp_fnf_dtl.Where(x => x.emp_id == p.emp_id).FirstOrDefault();
                if (fnf_data != null)
                {
                    p.is_gratuity = fnf_data.is_gratuity;
                    p.last_working_date = fnf_data.last_working_date;
                    p.notice_recovery_days = fnf_data.notice_recovery_days;
                    p.notice_Payment = fnf_data.notice_payment_days;
                    p.settlment_amt = fnf_data.settlment_amt;
                    p.settlment_dt = fnf_data.settlment_dt;
                    p.settlement_type = fnf_data.settlement_type;
                    p.net_amt = fnf_data.net_amt;
                    p.remarks = fnf_data.remarks;
                }

            });

            return fnf_temp_data;
        }
        public async Task<List<mdlFNFProcess>> Get_FNFProcessData()
        {
            List<mdlFNFProcess> FNFdata = new List<mdlFNFProcess>();
            if (EmpIDs.Count == 0 || ReqID.Count == 0)
            {
                return FNFdata;
            }

            try
            {
                List<mdlFNFProcess> emp_fnf_dtl = Get_EmpBasicFNFData();


                FNFdata.AddRange(emp_fnf_dtl);
                Task set_assettab = await Task.Run(async () => setAssetData(FNFdata));

                Task attandence_tab = await Task.Run(async () => set_AttendanceTab(FNFdata));

                Task reimbursmenttab = await Task.Run(async () => Get_ReimbursmentData(FNFdata));

                //await set_assettab; await attandence_tab;
            }
            catch (Exception ex)
            {

            }



            return FNFdata;
        }


        private async Task setAssetData(List<mdlFNFProcess> varFNFProcess)
        {

            var emp_asset_req = _context.tbl_assets_request_master.Where(x => x.is_deleted == 0 && x.is_active == 1 && varFNFProcess.Any(g => g.emp_id == x.req_employee_id)).GroupBy(h => h.assets_master_id).Select(g => g.OrderByDescending(h => h.asset_req_id).FirstOrDefault()).Select(q => new
            {
                asset_req_id = q.asset_req_id,
                asset_name = q.asset_name,
                from_dt = q.from_date,
                to_dt = q.to_date,
                req_remarks = q.description,
                asset_type = q.asset_type == 0 ? "New" : q.asset_type == 1 ? "Replacement" : q.asset_type == 2 ? "Submission" : "",
                asset_no = q.asset_number,
                is_permanent = q.is_permanent,
                issue_dt = q.asset_issue_date,
                replace_dt = q.replace_dt,
                submission_dt = q.submission_date,
                final_approve = q.is_finalapprove == 0 ? "Pending" : q.is_finalapprove == 1 ? "Approve" : q.is_finalapprove == 2 ? "Reject" : q.is_finalapprove == 3 ? "In Process" : "",
                final_status = q.is_finalapprove,
                recovery_amt = 0,
                empid = q.req_employee_id,
                company_id = q.company_id
            }).ToList();

            varFNFProcess.ForEach(p =>
            {
                List<asset_request> asset_req = (from e in emp_asset_req
                                                 join d in _context.tbl_emp_fnf_asset on e.empid equals d.fn_mstr.emp_id into ej
                                                 from d in ej.DefaultIfEmpty()
                                                 where e.empid == p.emp_id && e.company_id == p.company_id
                                                 select new asset_request
                                                 {
                                                     asset_req_id = e.asset_req_id,
                                                     asset_name = e.asset_name,
                                                     from_dt = e.from_dt,
                                                     to_dt = e.to_dt,
                                                     req_remarks = e.req_remarks,
                                                     asset_type = e.asset_type,
                                                     asset_no = e.asset_no,
                                                     is_permanent = e.is_permanent,
                                                     issue_dt = e.issue_dt,
                                                     replace_dt = e.replace_dt,
                                                     submission_dt = e.submission_dt,
                                                     final_status = e.final_status,
                                                     final_approve = e.final_approve,
                                                     recovery_amt = d == null ? 0 : d.recovery_amt,
                                                     empid = e.empid,
                                                 }).ToList();

                asset_req.ForEach(q =>
                {
                    p.mdl_assetrequest = asset_req;
                });
            });

            //varFNFProcess.ForEach(p => {
            //    List<asset_request> asset_req = _context.tbl_assets_request_master.Where(x => x.is_deleted == 0 && x.is_active == 1 && x.req_employee_id == p.emp_id).GroupBy(h=>h.assets_master_id).Select(g=>g.OrderByDescending(h=>h.asset_req_id).FirstOrDefault()).Select(q => new asset_request {
            //    asset_req_id=q.asset_req_id,
            //    asset_name=q.asset_name,
            //    from_dt=q.from_date,
            //    to_dt=q.to_date,
            //    req_remarks=q.description,
            //    asset_type=q.asset_type==0?"New":q.asset_type==1?"Replacement":q.asset_type==2?"Submission":"",
            //    asset_no=q.asset_number,
            //    is_permanent=q.is_permanent,
            //    issue_dt = q.asset_issue_date,
            //    replace_dt = q.replace_dt,
            //    submission_dt =q.submission_date,
            //    final_approve=q.is_finalapprove==0?"Pending":q.is_finalapprove==1?"Approve":q.is_finalapprove==2?"Reject":q.is_finalapprove==3?"In Process": "",
            //        final_status=q.is_finalapprove,
            //        recovery_amt =0,
            //        empid=q.req_employee_id,
            //    }).ToList();

            //    asset_req.ForEach(q => {
            //        p.mdl_assetrequest = asset_req;
            //    });
            //});

        }


        public async Task set_AttendanceTab(List<mdlFNFProcess> attendancetab)
        {

            try
            {


                // fnf_attandance objatt = new fnf_attandance();
                if (attendancetab.Count == 0 && _monthyear != 0)
                {
                    EmpIDs.ForEach(q =>
                    {
                        mdlFNFProcess objfnf = new mdlFNFProcess();
                        objfnf.emp_id = q;

                        attendancetab.Add(objfnf);
                    });

                }


                attendancetab.ForEach(p =>
                {
                    fnf_attandance objatt = new fnf_attandance();
                    Task.WaitAll();
                    objatt.mdl_LeaveEncashed = Task.Run(() => setLeavesLedger(p.emp_id,p.sep_id)).Result;
                    objatt.mdl_FNFAttendance = Task.Run(() => setAttendance(p.emp_id)).Result;
                    objatt.mdl_VariablePay = Task.Run(() => setVariable(p.emp_id)).Result;

                    p.mdl_attendance = objatt;
                });
            }
            catch (Exception ex)
            {

            }

        }

        private async Task<List<LeaveLedgerModell>> setLeavesLedger(int empid,int sep_id)
        {
            List<LeaveLedgerModell> objleaves = new List<LeaveLedgerModell>();

            List<LeaveLedgerModell> emp_leaves = _context.tbl_leave_ledger.Where(x => x.e_id == empid && (x.tbl_leave_type.leave_type_id == 2 || x.tbl_leave_type.leave_type_id == 4)).GroupBy(h => new { h.e_id, h.leave_type_id }).Select(q => new LeaveLedgerModell
            {
                leave_type_id = q.FirstOrDefault().leave_type_id ?? 0,
                leave_type_name = q.FirstOrDefault().tbl_leave_type.leave_type_name,
                credit = (int)q.Sum(r => r.credit),
                dredit = (int)q.Sum(r => r.dredit),
                balance = (int)q.Sum(x => x.credit) - q.Sum(x => x.dredit),
                e_id = q.FirstOrDefault().e_id ?? 0,
            }).ToList();


            var leave_encash = (from a in emp_leaves
                                join b in _context.tbl_fnf_leave_encash.Where(x=>x.emp_sep_id==sep_id && x.is_deleted==0) //.Where(x => x.fnf_mstr.emp_id == empid && x.fnf_mstr.emp_sep_mstr.is_deleted == 0) 
                                on a.leave_type_id equals b.leave_type_id into ab
                                from b in ab.DefaultIfEmpty()
                                select new LeaveLedgerModell
                                {
                                    leave_type_id = a.leave_type_id,
                                    leave_type_name = a.leave_type_name,
                                    e_id = a.e_id,
                                    balance = b != null ? (int)b.leave_balance : (int)a.balance, // Leave Balance
                                    credit = b != null ? (int)b.leave_encash_day : 0, // Leave Encash
                                }).ToList();


            objleaves.AddRange(leave_encash);

            return objleaves;

        }


        private async Task<List<fnf_attandance_dtl>> setAttendance(int empid)
        {
            List<fnf_attandance_dtl> objattendance = new List<fnf_attandance_dtl>();

            double ToalPayrollDay = 30, LopDays = 0, PaidDays = 0;

            var lop_Data = _context.tbl_lossofpay_master.Where(x => x.is_active == 1 && x.monthyear == _monthyear && x.emp_id == empid).GroupBy(y => y.emp_id).Select(p => p.OrderByDescending(h => h.lop_master_id).FirstOrDefault()).FirstOrDefault();
            if (lop_Data != null)
            {
                ToalPayrollDay = lop_Data.totaldays ?? 0;
                LopDays = Convert.ToDouble(lop_Data.final_lop_days);

                PaidDays = ToalPayrollDay - LopDays;
            }
            objattendance = _context.tbl_fnf_attendance_dtl.Where(x => x.fnf_mstr.emp_id == empid && x.fnf_mstr.emp_sep_mstr.is_deleted == 0).Select(p => new fnf_attandance_dtl
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
            }).ToList();


            return objattendance;
        }

        private async Task<List<fnf_variable>> setVariable(int emp_id)
        {
            List<fnf_variable> _variable = new List<fnf_variable>();

            _variable = _context.tbl_fnf_component.Where(x => x.fnf_mstr.emp_id == emp_id && x.fnf_mstr.is_deleted == 0).Select(q => new fnf_variable
            {
                variable_id = q.variable_id,
                monthyear = q.monthyear,
                component_id = q.component_id,
                component_name = q.component_mstr.component_name,
                amt = q.variable_amt,
                remarks = q.remarks,
            }).ToList();

            return _variable;
        }
        private async Task Get_ReimbursmentData(List<mdlFNFProcess> varFNFProcess)
        {
            varFNFProcess.ForEach(y =>
            {
                List<tbl_fnf_reimburesment> fnf_reim = _context.tbl_fnf_reimburesment.Where(x => x.fnf_mstr.emp_id == y.emp_id).Select(p => new tbl_fnf_reimburesment
                {
                    fnf_reim_id = p.fnf_reim_id,
                    reim_name = p.reim_name,
                    reim_amt = p.reim_amt,
                    reim_bal = p.reim_bal

                }).ToList();

                fnf_reim.ForEach(z =>
                {
                    y.mdl_reimbursment = fnf_reim;
                });
            });

        }


        public int Save_AssetRecovery(List<asset_request> objasset)
        {
            int result = 0;

            var emp_asset = _context.tbl_assets_request_master.Where(x => x.is_deleted == 0 && objasset.Any(y => y.asset_req_id == x.asset_req_id && y.empid == x.req_employee_id)).ToList();
            if (emp_asset.Count > 0)
            {
                objasset.ForEach(x =>
                {
                    var dtl = emp_asset.Where(q => q.asset_req_id == x.asset_req_id).FirstOrDefault();
                    if (dtl != null)
                    {
                        x.asset_name = dtl.asset_name;
                        x.asset_no = dtl.asset_number;
                    }
                });


                var fnf_mstr_dtl = _context.tbl_fnf_master.Where(x => x.is_deleted == 0 && objasset.Any(y => y.empid == x.emp_id)).FirstOrDefault();
                if (fnf_mstr_dtl != null)
                {
                    if (fnf_mstr_dtl.is_freezed == 1)
                    {

                        return result = 2; // FNF Freezed;
                    }
                    else
                    {
                        var exist_emp_fnf_asset = _context.tbl_emp_fnf_asset.Where(x => objasset.Any(y => y.empid == x.fn_mstr.emp_id && y.asset_name.Trim().ToUpper() == x.asset_name.Trim().ToUpper())).ToList();
                        if (exist_emp_fnf_asset.Count > 0)
                        {

                        }

                        else
                        {
                            List<tbl_emp_fnf_asset> objempfnf = objasset.Select(p => new tbl_emp_fnf_asset
                            {
                                asset_name = p.asset_name,
                                asset_number = p.asset_no,
                                recovery_amt = p.recovery_amt,
                                fnf_id = fnf_mstr_dtl.pkid_fnfMaster,
                            }).ToList();


                            _context.tbl_emp_fnf_asset.AddRange(objempfnf);
                            _context.SaveChanges();
                        }
                    }

                }

            }
            else
            {
                return result = 1;// Invalid Asset Request for recovery
            }

            return result;
        }


        public int Save_LeaveEncash(mdlFNFProcess objfnf)
        {
            int result = 0;

            try
            {
                int total_days = 30;
                DateTime resign_dt = new DateTime();
                var fnf_mstr = _context.tbl_fnf_master.Where(x => x.emp_id == objfnf.emp_id && x.emp_sep_mstr.is_deleted == 0 && x.is_deleted == 0 && x.is_freezed == 0).FirstOrDefault();

                if (fnf_mstr != null)
                {
                    resign_dt = Convert.ToDateTime(_monthyear.ToString().Substring(0, 4) + "-" + _monthyear.ToString().Substring(4, 2) + "-01");
                    var emp_salary = _context.tbl_emp_salary_master.OrderByDescending(y => y.sno).Where(x => x.emp_id == objfnf.emp_id && x.applicable_from_dt.Date <= resign_dt.Date && x.is_active == 1).OrderByDescending(z => z.sno).FirstOrDefault();
                    if (emp_salary != null)
                    {
                        var exist_encash = _context.tbl_fnf_leave_encash.Where(x => x.fnf_id == fnf_mstr.pkid_fnfMaster && x.emp_sep_id== objfnf.sep_id).ToList();
                        
                        if (exist_encash.Count > 0)
                        {
                            exist_encash.ForEach(p => { p.is_deleted = 1; p.modified_by = _clsCurrentUser.EmpId; p.modified_dt = DateTime.Now; });
                            _context.tbl_fnf_leave_encash.UpdateRange(exist_encash);
                          // _context.Entry(exist_encash).State = EntityState.Modified;

                            //  _context.tbl_fnf_leave_encash.RemoveRange(exist_encash);
                            _context.SaveChanges();
                        }


                        List<tbl_fnf_leave_encash> leave_encash = objfnf.mdl_attendance.mdl_LeaveEncashed.Select(p => new tbl_fnf_leave_encash
                        {
                            fnf_id = fnf_mstr.pkid_fnfMaster,
                            leave_type_id = p.leave_type_id,
                            leave_balance = p.balance,
                            leave_encash_day = p.leave_encash,
                            leave_encash_cal = (Convert.ToDecimal(p.leave_encash / total_days) * (emp_salary != null ? emp_salary.salaryrevision : 0)),
                            leave_encash_final = (Convert.ToDecimal(p.leave_encash / total_days) * (emp_salary != null ? emp_salary.salaryrevision : 0)),
                            x_id = objfnf.x_id,
                            created_by = _clsCurrentUser.EmpId,
                            created_dt = DateTime.Now,
                            modified_dt = DateTime.Now,
                            emp_sep_id=objfnf.sep_id
                        }).ToList();

                        _context.tbl_fnf_leave_encash.AddRange(leave_encash);
                        _context.SaveChanges();

                    }
                }
                else
                {
                    resign_dt = Convert.ToDateTime(_monthyear.ToString().Substring(0, 4) + "-" + _monthyear.ToString().Substring(4, 2) + "-01");
                    var emp_salary = _context.tbl_emp_salary_master.OrderByDescending(y => y.sno).Where(x => x.emp_id == objfnf.emp_id && x.applicable_from_dt.Date <= resign_dt.Date && x.is_active == 1).OrderByDescending(z => z.sno).FirstOrDefault();
                    if (emp_salary != null)
                    {
                        var exist_Leave_encash = _context.tbl_fnf_leave_encash.Where(x => x.emp_sep_id== objfnf.sep_id).ToList();
                        if (exist_Leave_encash.Count>0)
                        {
                            exist_Leave_encash.ForEach(p => { p.is_deleted = 1; p.modified_by = _clsCurrentUser.EmpId; p.modified_dt = DateTime.Now; });
                            _context.tbl_fnf_leave_encash.UpdateRange(exist_Leave_encash);

                            //  _context.tbl_fnf_leave_encash.RemoveRange(exist_encash);
                            _context.SaveChanges();
                           // _context.tbl_fnf_leave_encash.RemoveRange(exist_Leave_encash);
                            
                        }

                        List<tbl_fnf_leave_encash> leave_encash = objfnf.mdl_attendance.mdl_LeaveEncashed.Select(p => new tbl_fnf_leave_encash
                        {
                            leave_type_id = p.leave_type_id,
                            leave_balance = p.balance,
                            leave_encash_day = p.leave_encash,
                            leave_encash_cal = (Convert.ToDecimal(p.leave_encash / total_days) * (emp_salary != null ? emp_salary.salaryrevision : 0)),
                            leave_encash_final = (Convert.ToDecimal(p.leave_encash / total_days) * (emp_salary != null ? emp_salary.salaryrevision : 0)),
                            x_id = objfnf.x_id,
                            created_by = _clsCurrentUser.EmpId,
                            created_dt = DateTime.Now,
                            modified_dt = DateTime.Now,
                            emp_sep_id = objfnf.sep_id
                        }).ToList();

                        _context.tbl_fnf_leave_encash.AddRange(leave_encash);
                        _context.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {
                result = 1;
            }

            return result;
        }

        public int Save_FNFAttendence(mdlFNFProcess objfnf)
        {
            int result = 0;

            try
            {
                int year = Convert.ToInt32(objfnf.monthYear.ToString().Substring(0, 4));
                int month = Convert.ToInt32(objfnf.monthYear.ToString().Substring(4, 2));
                int total_days = DateTime.DaysInMonth(year, month);

                DateTime resign_dt = new DateTime();

                #region LOP Data Update
                clsLossPay ob = new clsLossPay(_context);
                tbl_lossofpay_master tblLOP = new tbl_lossofpay_master();
                tblLOP.emp_id = objfnf.emp_id;
                tblLOP.company_id = objfnf.company_id;
                tblLOP.fkid_sepration = objfnf.sep_id;
                tblLOP.monthyear = Convert.ToInt32(objfnf.monthYear);
                tblLOP.acutual_lop_days = Convert.ToDecimal(objfnf.mdl_attendance.mdl_FNFAttendance[0].acutual_lop_days);
                tblLOP.final_lop_days = Convert.ToDecimal(objfnf.mdl_attendance.mdl_FNFAttendance[0].final_lop_days);
                tblLOP.Holiday_days = Convert.ToDecimal(objfnf.mdl_attendance.mdl_FNFAttendance[0].Holiday_days);
                tblLOP.Week_off_days = Convert.ToDecimal(objfnf.mdl_attendance.mdl_FNFAttendance[0].Week_off_days);
                tblLOP.Present_days = Convert.ToDecimal(objfnf.mdl_attendance.mdl_FNFAttendance[0].Present_days);
                tblLOP.Absent_days = Convert.ToDecimal(objfnf.mdl_attendance.mdl_FNFAttendance[0].Absent_days);
                tblLOP.Leave_days = Convert.ToDecimal(objfnf.mdl_attendance.mdl_FNFAttendance[0].Leave_days);
                tblLOP.Actual_Paid_days = Convert.ToDecimal(objfnf.mdl_attendance.mdl_FNFAttendance[0].Actual_Paid_days);
                tblLOP.Additional_Paid_days = Convert.ToDecimal(objfnf.mdl_attendance.mdl_FNFAttendance[0].Additional_Paid_days);
                tblLOP.totaldays = Convert.ToInt32(objfnf.mdl_attendance.mdl_FNFAttendance[0].totaldays);
                tblLOP.Total_Paid_days = Convert.ToDecimal(objfnf.mdl_attendance.mdl_FNFAttendance[0].Total_Paid_days);
                tblLOP.created_by = _clsCurrentUser.EmpId;
                tblLOP.remarks = objfnf.mdl_attendance.mdl_FNFAttendance[0].remarks;

                ob.Update_LOP_Master(tblLOP);


                #endregion


                var fnf_mstr = _context.tbl_fnf_master.Where(x => x.emp_id == objfnf.emp_id && x.emp_sep_mstr.is_deleted == 0 && x.is_deleted == 0 && x.is_freezed == 0).FirstOrDefault();

                if (fnf_mstr != null)
                {
                    resign_dt = Convert.ToDateTime(_monthyear.ToString().Substring(0, 4) + "-" + _monthyear.ToString().Substring(4, 2) + "-01");
                    var emp_salary = _context.tbl_emp_salary_master.OrderByDescending(y => y.sno).Where(x => x.emp_id == objfnf.emp_id && x.applicable_from_dt.Date <= resign_dt.Date && x.is_active == 1).OrderByDescending(z => z.sno).FirstOrDefault();
                    if (emp_salary != null)
                    {
                        var exist_att = _context.tbl_fnf_attendance_dtl.Where(x => x.fnf_id == fnf_mstr.pkid_fnfMaster && x.emp_sep_id==objfnf.sep_id).ToList();
                        if (exist_att.Count > 0)
                        {
                            exist_att.ForEach(p => { p.is_deleted = 1; p.modified_by = _clsCurrentUser.EmpId; p.modified_dt = DateTime.Now; });
                            _context.tbl_fnf_attendance_dtl.UpdateRange(exist_att);
                            //_context.tbl_fnf_attendance_dtl.RemoveRange(exist_att);
                            _context.SaveChanges();
                        }

                        tbl_fnf_attendance_dtl att_dtl = objfnf.mdl_attendance.mdl_FNFAttendance.Select(p => new tbl_fnf_attendance_dtl
                        {
                            fnf_id = fnf_mstr.pkid_fnfMaster,
                            monthyear = Convert.ToInt32(objfnf.monthYear),
                            totaldays = p.totaldays,
                            Holiday_days = p.Holiday_days,
                            Week_off_days = p.Week_off_days,
                            Present_days = p.Present_days,
                            Absent_days = p.Absent_days,
                            Leave_days = p.Leave_days,
                            acutual_lop_days = p.acutual_lop_days,
                            final_lop_days = p.final_lop_days,
                            Total_Paid_days = p.Actual_Paid_days,
                            paid_amount = p.final_lop_days > p.acutual_lop_days ?
                            Convert.ToDouble(Convert.ToDecimal(p.final_lop_days / total_days) * (emp_salary != null ? emp_salary.salaryrevision : 0))
                            : Convert.ToDouble(Convert.ToDecimal(p.acutual_lop_days / total_days) * (emp_salary != null ? emp_salary.salaryrevision : 0)),
                            x_id = objfnf.x_id,
                            created_by = _clsCurrentUser.EmpId,
                            created_dt = DateTime.Now,
                            modified_dt = DateTime.Now,
                            emp_sep_id=objfnf.sep_id
                        }).ToList().FirstOrDefault();

                        _context.tbl_fnf_attendance_dtl.AddRange(att_dtl);
                        _context.SaveChanges();

                    }
                }
                else
                {
                    resign_dt = Convert.ToDateTime(_monthyear.ToString().Substring(0, 4) + "-" + _monthyear.ToString().Substring(4, 2) + "-01");
                    var emp_salary = _context.tbl_emp_salary_master.OrderByDescending(y => y.sno).Where(x => x.emp_id == objfnf.emp_id && x.applicable_from_dt.Date <= resign_dt.Date && x.is_active == 1).OrderByDescending(z => z.sno).FirstOrDefault();
                    if (emp_salary != null)
                    {
                        var exist_att_dtl = _context.tbl_fnf_attendance_dtl.Where(x => x.emp_sep_id==objfnf.sep_id).ToList();
                        if (exist_att_dtl.Count>0)
                        {
                            exist_att_dtl.ForEach(p => { p.is_deleted = 1; p.modified_by = _clsCurrentUser.EmpId; p.modified_dt = DateTime.Now; });
                            _context.tbl_fnf_attendance_dtl.UpdateRange(exist_att_dtl);
                            //_context.tbl_fnf_attendance_dtl.RemoveRange(exist_att_dtl);
                            _context.SaveChanges();
                        }

                        tbl_fnf_attendance_dtl att_dtl = objfnf.mdl_attendance.mdl_FNFAttendance.Select(p => new tbl_fnf_attendance_dtl
                        {
                            monthyear = Convert.ToInt32(objfnf.monthYear),
                            totaldays = total_days,
                            Holiday_days = p.Holiday_days,
                            Week_off_days = p.Week_off_days,
                            Present_days = p.Present_days,
                            Absent_days = p.Absent_days,
                            Leave_days = p.Leave_days,
                            acutual_lop_days = p.acutual_lop_days,
                            final_lop_days = p.final_lop_days,
                            Total_Paid_days = p.Actual_Paid_days,
                            paid_amount = p.final_lop_days > p.acutual_lop_days ?
                             Convert.ToDouble(Convert.ToDecimal(p.final_lop_days / total_days) * (emp_salary != null ? emp_salary.salaryrevision : 0))
                             : Convert.ToDouble(Convert.ToDecimal(p.acutual_lop_days / total_days) * (emp_salary != null ? emp_salary.salaryrevision : 0)),
                            x_id = objfnf.x_id,
                            created_by = _clsCurrentUser.EmpId,
                            created_dt = DateTime.Now,
                            modified_dt = DateTime.Now,
                            emp_sep_id=objfnf.sep_id
                        }).ToList().FirstOrDefault();

                        _context.tbl_fnf_attendance_dtl.AddRange(att_dtl);
                        _context.SaveChanges();

                    }
                }
            }
            catch (Exception)
            {
                result = 1;
            }

            return result;
        }


        public int Save_VariablePay(mdlFNFProcess objfnf)
        {
            int result = 0;

            try
            {
                int year = Convert.ToInt32(objfnf.monthYear.ToString().Substring(0, 4));
                int month = Convert.ToInt32(objfnf.monthYear.ToString().Substring(4, 2));
                int total_days = DateTime.DaysInMonth(year, month);

                DateTime resign_dt = new DateTime();

                #region Salary Input Change Update

                var exist_data = _context.tbl_salary_input_change.Where(a => a.emp_id == objfnf.emp_id && a.is_fnf_comp == 1 && Convert.ToString(a.monthyear) == objfnf.monthYear).FirstOrDefault();
                if (exist_data != null)
                {
                    _context.tbl_salary_input_change.RemoveRange(exist_data);
                    _context.SaveChanges();
                }

                List<tbl_salary_input_change> leave_encash = objfnf.mdl_attendance.mdl_VariablePay.Select(p => new tbl_salary_input_change
                {
                    company_id = objfnf.company_id,
                    monthyear = objfnf.monthYear,
                    emp_id = objfnf.emp_id,
                    component_id = p.component_id,
                    values = p.amt.ToString(),
                    is_active = 1,
                    is_fnf_comp = 1,
                    created_by = _clsCurrentUser.EmpId,
                    created_dt = DateTime.Now,
                    modified_dt = DateTime.Now,
                }).ToList();

                _context.tbl_salary_input_change.AddRange(leave_encash);
                _context.SaveChanges();

                #endregion


                var fnf_mstr = _context.tbl_fnf_master.Where(x => x.emp_id == objfnf.emp_id && x.emp_sep_mstr.is_deleted == 0 && x.is_deleted == 0 && x.is_freezed == 0).FirstOrDefault();

                if (fnf_mstr != null)
                {
                    resign_dt = Convert.ToDateTime(_monthyear.ToString().Substring(0, 4) + "-" + _monthyear.ToString().Substring(4, 2) + "-01");
                    var emp_salary = _context.tbl_emp_salary_master.OrderByDescending(y => y.sno).Where(x => x.emp_id == objfnf.emp_id && x.applicable_from_dt.Date <= resign_dt.Date && x.is_active == 1).OrderByDescending(z => z.sno).FirstOrDefault();
                    if (emp_salary != null)
                    {
                        //  var exist_vaypay = _context.tbl_fnf_component.Where(x => x.fnf_id == fnf_mstr.pkid_fnfMaster && objfnf.mdl_attendance.mdl_VariablePay.Any(y => y.component_id == x.component_id)).ToList();
                        var exist_vaypay = _context.tbl_fnf_component.Where(x => x.fnf_id == fnf_mstr.pkid_fnfMaster && x.emp_sep_id==objfnf.sep_id).ToList();
                        if (exist_vaypay.Count > 0)
                        {
                            exist_vaypay.ForEach(p => { p.is_deleted = 1; p.modified_by = _clsCurrentUser.EmpId; p.modified_dt = DateTime.Now; });
                            _context.tbl_fnf_component.UpdateRange(exist_vaypay);

                            // _context.tbl_fnf_component.RemoveRange(exist_vaypay);
                            _context.SaveChanges();
                        }


                        List<tbl_fnf_component> fnf_comp = objfnf.mdl_attendance.mdl_VariablePay.Select(p => new tbl_fnf_component
                        {
                            fnf_id = fnf_mstr.pkid_fnfMaster,
                            component_id = p.component_id,
                            component_type = p.component_type,
                            monthyear = Convert.ToInt32(objfnf.monthYear),
                            variable_amt = Convert.ToDouble(p.amt),
                            x_id = objfnf.x_id,
                            created_by = _clsCurrentUser.EmpId,
                            created_dt = DateTime.Now,
                            modified_dt = DateTime.Now,
                            emp_sep_id=objfnf.sep_id
                        }).ToList();

                        _context.tbl_fnf_component.AddRange(fnf_comp);
                        _context.SaveChanges();

                    }
                }
                else
                {
                    resign_dt = Convert.ToDateTime(_monthyear.ToString().Substring(0, 4) + "-" + _monthyear.ToString().Substring(4, 2) + "-01");
                    var emp_salary = _context.tbl_emp_salary_master.OrderByDescending(y => y.sno).Where(x => x.emp_id == objfnf.emp_id && x.applicable_from_dt.Date <= resign_dt.Date && x.is_active == 1).OrderByDescending(z => z.sno).FirstOrDefault();
                    if (emp_salary != null)
                    {

                        var exist_fnf_comp = _context.tbl_fnf_component.Where(x => x.emp_sep_id==objfnf.sep_id).ToList();
                        if (exist_fnf_comp.Count>0)
                        {
                            exist_fnf_comp.ForEach(p => { p.is_deleted = 1; p.modified_by = _clsCurrentUser.EmpId; p.modified_dt = DateTime.Now; });
                            _context.tbl_fnf_component.UpdateRange(exist_fnf_comp);

                            //_context.tbl_fnf_component.RemoveRange(exist_fnf_comp);
                            _context.SaveChanges();
                        }

                        List<tbl_fnf_component> fnf_comp = objfnf.mdl_attendance.mdl_VariablePay.Select(p => new tbl_fnf_component
                        {
                            component_id = p.component_id,
                            component_type = p.component_type,
                            monthyear = Convert.ToInt32(objfnf.monthYear),
                            variable_amt = Convert.ToDouble(p.amt),
                            x_id = objfnf.x_id,
                            created_by = _clsCurrentUser.EmpId,
                            created_dt = DateTime.Now,
                            modified_dt = DateTime.Now,
                            emp_sep_id=objfnf.sep_id
                        }).ToList();

                        _context.tbl_fnf_component.AddRange(fnf_comp);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                result = 1;
            }

            return result;
        }

        public int Save_FNFProcess(mdlFNFProcess objfnf)
        {
            int result = 0;
            int fnf_id = 0;
            try
            {
                DateTime resign_dt = new DateTime();
                var fnf_mstr = _context.tbl_fnf_master.Where(x => x.emp_id == objfnf.emp_id && x.fkid_empSepration == objfnf.sep_id && x.is_deleted == 0 && x.is_freezed == 0).FirstOrDefault();

                if (fnf_mstr != null)
                {
                    fnf_mstr.is_gratuity = objfnf.is_gratuity;
                    fnf_mstr.net_amt = 0;
                    if (objfnf.is_notice_recovery == 1) fnf_mstr.notice_recovery_days = objfnf.notice_recovery_days;
                    else fnf_mstr.notice_recovery_days = 0;
                    if (objfnf.is_notice_payment == 1) fnf_mstr.notice_payment_days = objfnf.notice_Payment;
                    else fnf_mstr.notice_payment_days = 0;
                    fnf_mstr.monthYear = Convert.ToInt32(objfnf.monthYear);
                    fnf_mstr.last_modified_by = _clsCurrentUser.EmpId;
                    fnf_mstr.last_modified_date = DateTime.Now;

                    _context.tbl_fnf_master.UpdateRange(fnf_mstr);
                    _context.SaveChanges();
                    fnf_id = fnf_mstr.pkid_fnfMaster;

                }
                else
                {
                    tbl_fnf_master obj_fnnf = new tbl_fnf_master();
                    obj_fnnf.emp_id = objfnf.emp_id;
                    obj_fnnf.fkid_empSepration = objfnf.sep_id;
                    obj_fnnf.is_deleted = 0;
                    obj_fnnf.is_gratuity = objfnf.is_gratuity;
                    obj_fnnf.net_amt = 0;
                    obj_fnnf.resign_dt = objfnf.resign_dt;
                    obj_fnnf.monthYear = Convert.ToInt32(objfnf.monthYear);
                    obj_fnnf.last_working_date = objfnf.last_working_date;
                    if (objfnf.is_notice_recovery == 1) obj_fnnf.notice_recovery_days = objfnf.notice_recovery_days;
                    else obj_fnnf.notice_recovery_days = 0;
                    if (objfnf.is_notice_payment == 1) obj_fnnf.notice_payment_days = objfnf.notice_Payment;
                    else obj_fnnf.notice_payment_days = 0;
                    obj_fnnf.created_by = _clsCurrentUser.EmpId;
                    obj_fnnf.created_dt = DateTime.Now;
                    _context.tbl_fnf_master.AddRange(obj_fnnf);
                    _context.SaveChanges();
                    fnf_id = obj_fnnf.pkid_fnfMaster;
                }

                //if (fnf_id > 0)
                //{
                //    _context.tbl_fnf_leave_encash.Where(x => x.x_id == objfnf.x_id).ForEachAsync(y => y.fnf_id = fnf_id);
                //    _context.tbl_fnf_attendance_dtl.Where(x => x.x_id == objfnf.x_id).ForEachAsync(y => y.fnf_id = fnf_id);
                //    _context.tbl_fnf_component.Where(x => x.x_id == objfnf.x_id).ForEachAsync(y => y.fnf_id = fnf_id);
                //}


            }
            catch (Exception ex)
            {
                result = 1;
            }

            return result;
        }


        public int Save_Reimbursmnet(mdlFNFProcess objfnf_reim)
        {
            int result = 0;
            var fnf_mstr = _context.tbl_fnf_master.Where(x => x.is_deleted == 0 && x.is_freezed == 0 && x.emp_id == objfnf_reim.emp_id).FirstOrDefault();
            if (fnf_mstr != null)
            {
                var exist_reim = _context.tbl_fnf_reimburesment.Where(x => x.fnf_mstr.emp_id == objfnf_reim.emp_id
                     && objfnf_reim.mdl_reimbursment.Any(y => y.reim_name.Trim().ToUpper() == x.reim_name.Trim().ToUpper())).FirstOrDefault();
                if (exist_reim != null)
                {

                    return result = 2; // Reimbursment already exist
                }

                objfnf_reim.mdl_reimbursment.ForEach(y =>
                {
                    tbl_fnf_reimburesment objreim = new tbl_fnf_reimburesment();
                    objreim.fnf_id = fnf_mstr.pkid_fnfMaster;
                    objreim.reim_name = y.reim_name;
                    objreim.reim_amt = y.reim_amt;

                    _context.tbl_fnf_reimburesment.Add(objreim);
                });

                _context.SaveChanges();

                return result;
            }
            else
            {
                return result = 1; // Invalid request or already freezed
            }

        }







        //public async Task GetLoanDetail(List<mdlFNFProcess> varFNFLoanRecover)
        //{
        //    //var emp_recive = _context.tbl_loan_repayments.Where(a => a.is_deleted == 0 && varFNFLoanRecover.Any(p=>p.emp_id==a.req_emp_id) && a.status == 0).
        //    //  Select(a => new { monthly_emi = a.interest_component + a.principal_amount }).Sum(p => p.monthly_emi);

        //    if (varFNFLoanRecover.Count == 0)
        //    {
        //        EmpIDs.ForEach(q => {
        //            mdlFNFProcess objfnf = new mdlFNFProcess();
        //            objfnf.emp_id = q;

        //            varFNFLoanRecover.Add(objfnf);
        //        });

        //    }

        //    varFNFLoanRecover.ForEach(q => {


        //        var loan_recive_amts = _context.tbl_loan_repayments.Where(a => a.is_deleted == 0 && a.req_emp_id==q.emp_id && a.status == 0).
        //             GroupBy(h => new { h.req_emp_id, h.loan_id }).Select(h => new
        //             {
        //                 h.FirstOrDefault().req_emp_id,
        //                 _EMI = h.Sum(r => (r.interest_component + r.principal_amount)),
        //                 loan_req_idd = h.FirstOrDefault().loan_id
        //             }).ToList();

        //        var loan_reqs = _context.tbl_loan_request.Where(x => x.req_emp_id == q.emp_id && x.is_deleted == 0 && x.is_final_approval == 1 && x.is_closed == 0).Select(p => new 
        //        {
        //            loan_req_id = p.loan_req_id,
        //            empid = p.req_emp_id ?? 0,
        //            loan_type = p.loan_type == 1 ? "Loan" : p.loan_type == 2 ? "Advance" : "",
        //            loan_amt = p.loan_amt,
        //            monthly_emi = p.monthly_emi,
        //            credit = loan_recive_amts.Where(h=>h.loan_req_idd==p.loan_req_id).FirstOrDefault() != null ? loan_recive_amts.Where(h => h.loan_req_idd == p.loan_req_id).FirstOrDefault()._EMI : 0, // amount received
        //            debit = (p.loan_amt - Convert.ToDecimal(loan_recive_amts.Where(h => h.loan_req_idd == p.loan_req_id).FirstOrDefault() != null ? loan_recive_amts.Where(h => h.loan_req_idd == p.loan_req_id).FirstOrDefault()._EMI : 0)),// pending amount, which to be received
        //            remaining_balance = loan_recive_amts.Where(h => h.loan_req_idd == p.loan_req_id).FirstOrDefault() != null ? loan_recive_amts.Where(h => h.loan_req_idd == p.loan_req_id).FirstOrDefault()._EMI : 0, // recived balance
        //            recover_amt = 0,
        //        }).ToList();


        //        List<LoanModel> loan_dtl = (from e in loan_reqs
        //                                    join d in _context.tbl_fnf_loan_recover.Where(x=>x.is_deleted==0) on e.loan_req_id equals d.loan_req_id into ej
        //                                    from d in ej.DefaultIfEmpty() select new LoanModel {
        //                                       loan_recovery_id=d!=null?d.loan_recovery_id:0,
        //                                        loan_req_id = e.loan_req_id,
        //                                        empid = e.empid,
        //                                        loan_type =e.loan_type,
        //                                        loan_amt = e.loan_amt,
        //                                        monthly_emi = e.monthly_emi,
        //                                        credit =Math.Round((Convert.ToDecimal(e.credit)),2),
        //                                        debit =Math.Round(e.debit,2),
        //                                        remaining_balance =Math.Round(Convert.ToDecimal(e.remaining_balance),2),
        //                                        recover_amt =d!=null?Math.Round(d.loan_recover_amt,2): 0,
        //                                    }).ToList();

        //        loan_dtl.ForEach(g =>
        //        {
        //            q.mdl_Loan = loan_dtl;
        //        });
        //    });
        //}

        //public int Save_LoanRecover(mdlFNFProcess objloanrecover)
        //{
        //    int result = 0;

        //    var exist = _context.tbl_fnf_loan_recover.Where(x => x.is_deleted == 0 && x.is_process==1 && objloanrecover.mdl_Loan.Any(y => y.loan_req_id == x.loan_req_id)).ToList();
        //    if (exist.Count > 0)
        //    {
        //            result = 1;   
        //    }

        //    var exist_req = _context.tbl_fnf_loan_recover.Where(x => x.is_deleted == 0 && objloanrecover.mdl_Loan.Any(y => y.loan_req_id == x.loan_req_id)).ToList();
        //    if (exist_req.Count > 0)
        //    {
        //        exist_req.ForEach(p => { p.is_deleted = 1;p.modified_by = _clsCurrentUser.EmpId;p.modified_dt = DateTime.Now; });
        //        _context.Entry(exist_req).State = EntityState.Modified;
        //    }

        //    List<tbl_fnf_loan_recover> objtbl = objloanrecover.mdl_Loan.Select(p => new tbl_fnf_loan_recover {
        //        loan_req_id = p.loan_req_id,
        //        loan_recover_amt = p.recover_amt,
        //        is_process = 0,
        //        is_deleted = 0,
        //        created_by = _clsCurrentUser.EmpId,
        //        created_dt = DateTime.Now,
        //        modified_by = 0,
        //        modified_dt=Convert.ToDateTime("01-01-2000"),
        //    }).ToList();


        //    _context.Entry(objtbl).State = EntityState.Added;
        //    _context.SaveChanges();
        //    return result;
        //}

        public async Task Get_FNFSalary(List<mdlFNFProcess> varFNFSalary)
        {

            var sep_Req = _context.tbl_emp_separation.Where(x => x.is_deleted == 0 && EmpIDs.Contains(x.emp_id) && ReqID.Contains(x.sepration_id)).ToList();
            if (sep_Req.Count > 0)
            {
                if (varFNFSalary.Count == 0)
                {
                    EmpIDs.ForEach(q =>
                    {
                        mdlFNFProcess objfnf = new mdlFNFProcess();
                        objfnf.emp_id = q;

                        varFNFSalary.Add(objfnf);
                    });

                }

                varFNFSalary.ForEach(p =>
                {
                    p.company_id = sep_Req.FirstOrDefault(x => x.emp_id == p.emp_id).company_id;
                });
            }
        }


        public int SettlementFNF(tbl_fnf_master objfnf)
        {
            int result = 0;
            var fnf_mstr = _context.tbl_fnf_master.Where(x => x.is_deleted == 0 && x.emp_id == objfnf.emp_id && x.fkid_empSepration == objfnf.fkid_empSepration).FirstOrDefault();
            if (fnf_mstr != null)
            {
                fnf_mstr.net_amt = objfnf.net_amt;
                fnf_mstr.settlment_amt = objfnf.settlment_amt;
                fnf_mstr.settlement_type = objfnf.settlement_type;
                fnf_mstr.settlment_dt = objfnf.settlment_dt;
                fnf_mstr.last_modified_date = DateTime.Now;
                _context.tbl_fnf_master.Update(fnf_mstr);
                _context.SaveChanges();

                return result;
            }
            else
            {
                return result = 1; // Invalid request or already freezed
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using projContext;
using projContext.DB;
//using MySql.Data;
//using MySql.Data.MySqlClient;


namespace projAPI
{
    public class clsComponentValues
    {


        private readonly int _EmpID;
        private readonly int _SepId;
        private readonly int _ComponentId;
        private readonly int _PayrollMonthyear;
        private readonly int _SGID;
        private readonly string _ComponentValue;
        private readonly Context _context;
        clsSystemKeysfunction _clsSystemKeysfunction;
        private readonly int _company_id;
        Type _type = null;
        private readonly IConfiguration _config;

        public clsComponentValues(int CompanyID, Context context, int EmpID, int PayrollMonthyear, int ComponentId, int SGID, string ComponentValue, IConfiguration config, int sepId = 0)
        {
            _company_id = CompanyID;
            _context = context;
            _EmpID = EmpID;
            _PayrollMonthyear = PayrollMonthyear;
            _ComponentId = ComponentId;
            _ComponentValue = ComponentValue;
            _SGID = SGID;
            _SepId = sepId;
            _config = config;
            _clsSystemKeysfunction = new clsSystemKeysfunction(context, EmpID, PayrollMonthyear, SGID, _config, _SepId);
            _type = typeof(clsSystemKeysfunction);

        }

        private List<tbl_component_master> GetAllComponent(byte LoadAllKey = 1, byte LoadOnlySystemkeys = 0)
        {
            return LoadAllKey == 1 ? _context.tbl_component_master.Where(p => 1 == (LoadOnlySystemkeys == 1 ? p.is_system_key : 1) && p.is_active == 1).ToList() :
                _context.tbl_component_master.Where(p => p.is_active == 1 && 1 == (LoadOnlySystemkeys == 1 ? p.is_system_key : 1)).ToList();
        }

        private List<tbl_component_formula_details> GetAllComponentDetail()
        {
            return _context.tbl_component_formula_details.Include(p => p.comp_master).Where(p => p.salary_group_id == _SGID && p.is_deleted == 0).OrderBy(p => p.function_calling_order).ToList();
        }

        private List<tbl_salary_input> GetSalaryInput()
        {
            string PayrollMonthyear = Convert.ToString(_PayrollMonthyear);
            return _context.tbl_salary_input.Where(p => p.emp_id == _EmpID && p.monthyear == Convert.ToInt32(PayrollMonthyear) && p.company_id == _company_id && p.is_active == 1).ToList();
        }

        private List<tbl_salary_input_change> GetSalaryInputChange()
        {
            string PayrollMonthyear = Convert.ToString(_PayrollMonthyear);
            return _context.tbl_salary_input_change.Where(p => p.emp_id == _EmpID && p.monthyear == PayrollMonthyear && p.company_id == _company_id && p.is_active == 1).ToList();
        }

        public List<mdlSalaryInputValues> CalculateComponentValues(bool CalculateSalary)
        {
            //First Calculate all System Key
            List<mdlSalaryInputValues> mdl = new List<mdlSalaryInputValues>();
            StringBuilder Sb = new StringBuilder("");
            var TempCompDatas = GetAllComponentDetail();
            var TempSalaryInputDatas = GetSalaryInput();
            var TempSalaryInputChangeDatas = GetSalaryInputChange();
            int compoID = 0;
            List<MySqlParameter> mySqlParameter = new List<MySqlParameter>();
            foreach (var TempCompData in TempCompDatas)
            {
                compoID = TempCompData.comp_master.component_id;

                if (TempSalaryInputDatas.Any(p => p.component_id == compoID))
                {
                    var TempSalaryInputData = TempSalaryInputDatas.FirstOrDefault(p => p.component_id == compoID);
                    Sb.AppendFormat("SET {0}:= '{1}'; ", TempCompData.comp_master.component_name, TempSalaryInputData.values);
                    //mySqlParameter.Add(new MySqlParameter(string.Format("{0}", TempCompData.comp_master.component_name), TempSalaryInputData.values));
                }
                else
                {
                    if (Convert.ToString(TempCompData.comp_master.System_function ?? "").Trim().Length > 0)
                    {
                        MethodInfo methodInfo = _type.GetMethod(TempCompData.comp_master.System_function);
                        if (methodInfo == null)
                        {
                            throw new Exception("System function " + TempCompData.comp_master.System_function + " not implemented");
                        }
                        else
                        {
                            Sb.AppendFormat("SET {0}:= '{1}'; ", TempCompData.comp_master.component_name, methodInfo.Invoke(_clsSystemKeysfunction, null));//declare the all variable with Default value
                            //mySqlParameter.Add(new MySqlParameter(string.Format("{0}", TempCompData.comp_master.component_name), methodInfo.Invoke(_clsSystemKeysfunction, null)));

                        }
                    }
                    else
                    {
                        Sb.AppendFormat("SET {0}:= '{1}'; ", TempCompData.comp_master.component_name, TempCompData.comp_master.defaultvalue);//declare the all variable with Default value
                        //mySqlParameter.Add(new MySqlParameter(string.Format("{0}", TempCompData.comp_master.component_name), TempCompData.comp_master.defaultvalue));
                    }
                    //Now Define the another forumla 
                }


                if (TempCompData.comp_master.component_id == _ComponentId && _ComponentId > 0)
                {
                    Sb.AppendFormat("set {0}= {1}; ", TempCompData.comp_master.component_name, _ComponentValue);
                }
                else
                {
                    if (TempCompData.comp_master.is_system_key == 0)
                    {
                        if (TempSalaryInputChangeDatas.Any(p => p.component_id == compoID))
                        {
                            var TempSalaryInputChangeData = TempSalaryInputChangeDatas.FirstOrDefault(p => p.component_id == compoID);
                            Sb.AppendFormat("set {0}= '{1}'; ", TempCompData.comp_master.component_name, TempSalaryInputChangeData.values);
                        }
                        else
                        {
                            Sb.AppendFormat("set {0}= {1}; ", TempCompData.comp_master.component_name, TempCompData.formula);
                        }
                    }
                }



                //Now Select the All varaibale

            }

            for (int Index = 0; Index < TempCompDatas.Count(); Index++)
            {
                compoID = TempCompDatas[Index].comp_master.component_id;
                if (Index > 0)
                {
                    Sb.AppendFormat("union all ");
                }

                Sb.AppendFormat(" select {0} as 'compId','{1}' as 'compName',{1} as 'compValue',0 as 'rate',0 as current_month_value,0 as arrear_value ,{2} as  'component_type'",
                    TempCompDatas[Index].component_id, TempCompDatas[Index].comp_master.component_name, TempCompDatas[Index].comp_master.component_type);
            }

            if (Sb.Length > 0)
            {

                mdl = _context.mdlSalaryInputValues.FromSql(Sb.ToString()).AsNoTracking().ToList();
                if (!CalculateSalary)
                {
                    return mdl;
                }

                double ToalPayrollDay = 30, LopDays = 0, PaidDays = 0, NetPaidDays = 0;
                string ToalPayrollDaytemp = mdl.Where(p => p.compId == (int)enmOtherComponent.ToalPayrollDay).FirstOrDefault()?.compValue;
                double.TryParse(ToalPayrollDaytemp, out ToalPayrollDay);
                string LopDaystemp = mdl.Where(p => p.compId == (int)enmOtherComponent.LopDays).FirstOrDefault()?.compValue;
                double.TryParse(LopDaystemp, out LopDays);
                PaidDays = ToalPayrollDay - LopDays;
                if (PaidDays < 0)
                {
                    PaidDays = 0;
                }

                string netPaid = mdl.Where(p => p.compId == (int)enmOtherComponent.NetPaidDays).FirstOrDefault()?.compValue;
                double.TryParse(netPaid, out NetPaidDays);
                if (NetPaidDays < 0)
                {
                    NetPaidDays = 0;
                }

                for (int index = 0; index < mdl.Count; index++)
                {
                    if (mdl[index].compId == (int)enmOtherComponent.LopDays)
                    {
                        mdl[index].current_month_value = LopDays;
                    }
                    if (mdl[index].compId == (int)enmOtherComponent.ToalPayrollDay)
                    {
                        mdl[index].current_month_value = ToalPayrollDay;
                    }
                    if (mdl[index].compId == (int)enmOtherComponent.PaidDays)
                    {
                        mdl[index].current_month_value = PaidDays;
                    }
                    if (mdl[index].compId == (int)enmOtherComponent.NetPaidDays)
                    {
                        mdl[index].current_month_value = NetPaidDays;
                    }

                    double rateTemp = 0;

                    if (mdl[index].component_type == (int)enmComponentType.Income || mdl[index].component_type == (int)enmComponentType.Deduction)
                    {
                        double.TryParse(mdl[index].compValue, out rateTemp);
                        mdl[index].rate = rateTemp;
                        if (mdl[index].compId == (int)enmOtherComponent.EsicAmount || mdl[index].compId == (int)enmOtherComponent.EmployerEsicAmount)
                        {
                            mdl[index].current_month_value = Math.Ceiling((rateTemp * NetPaidDays) / ToalPayrollDay);
                        }
                        else
                        {
                            mdl[index].current_month_value = Math.Round((rateTemp * NetPaidDays) / ToalPayrollDay);
                        }

                    }
                    else if (mdl[index].component_type == (int)enmComponentType.OtherIncome || mdl[index].component_type == (int)enmComponentType.OtherDeduction)
                    {
                        double.TryParse(mdl[index].compValue, out rateTemp);
                        mdl[index].current_month_value = rateTemp;
                    }
                }

                calculatePf(mdl, NetPaidDays, ToalPayrollDay);
                calculateArrear(mdl);
                int indexinner = -1;
                double CurrentNet = 0, Arrearnet = 0;

                indexinner = mdl.FindIndex(p => p.compId == (int)enmOtherComponent.GrossSalary);
                if (indexinner >= 0)
                {
                    mdl[indexinner].rate = Convert.ToDouble(mdl[indexinner].compValue);
                    CurrentNet = mdl[indexinner].current_month_value = mdl.Where(p => p.component_type == (int)enmComponentType.Income || p.component_type == (int)enmComponentType.OtherIncome).Sum(p => p.current_month_value);
                    Arrearnet = mdl[indexinner].arrear_value = mdl.Where(p => p.component_type == (int)enmComponentType.Income || p.component_type == (int)enmComponentType.OtherIncome).Sum(p => p.arrear_value);

                }
                indexinner = -1;
                indexinner = mdl.FindIndex(p => p.compId == (int)enmOtherComponent.TotalGross);
                if (indexinner >= 0)
                {
                    mdl[indexinner].compValue = (Arrearnet + CurrentNet).ToString();
                    mdl[indexinner].current_month_value = CurrentNet + Arrearnet;
                }

                indexinner = -1;
                indexinner = mdl.FindIndex(p => p.compId == (int)enmOtherComponent.Deduction);
                if (indexinner >= 0)
                {
                    mdl[indexinner].current_month_value = mdl.Where(p => p.component_type == (int)enmComponentType.Deduction || p.component_type == (int)enmComponentType.OtherDeduction).Sum(p => p.current_month_value);
                    mdl[indexinner].arrear_value = mdl.Where(p => p.component_type == (int)enmComponentType.Deduction || p.component_type == (int)enmComponentType.OtherDeduction).Sum(p => p.arrear_value);

                    CurrentNet = CurrentNet - (mdl[indexinner].current_month_value);
                    Arrearnet = Arrearnet - (mdl[indexinner].arrear_value);
                }
                indexinner = -1;
                indexinner = mdl.FindIndex(p => p.compId == (int)enmOtherComponent.Net);
                if (indexinner >= 0)
                {
                    mdl[indexinner].arrear_value = Arrearnet;
                    mdl[indexinner].current_month_value = CurrentNet;
                }



            }
            return mdl;
        }
        public List<mdlSalaryInputValues> CalculateComponentValuesFNF(bool CalculateSalary)
        {
            //First Calculate all System Key
            List<mdlSalaryInputValues> mdl = new List<mdlSalaryInputValues>();
            StringBuilder Sb = new StringBuilder("");
            var TempCompDatas = GetAllComponentDetail();
            var TempSalaryInputDatas = GetSalaryInput();
            var TempSalaryInputChangeDatas = GetSalaryInputChange();
            int compoID = 0;
            List<MySqlParameter> mySqlParameter = new List<MySqlParameter>();
            foreach (var TempCompData in TempCompDatas)
            {
                compoID = TempCompData.comp_master.component_id;

                if (TempSalaryInputDatas.Any(p => p.component_id == compoID))
                {
                    var TempSalaryInputData = TempSalaryInputDatas.FirstOrDefault(p => p.component_id == compoID);
                    Sb.AppendFormat("SET {0}:= '{1}'; ", TempCompData.comp_master.component_name, TempSalaryInputData.values);
                    //mySqlParameter.Add(new MySqlParameter(string.Format("{0}", TempCompData.comp_master.component_name), TempSalaryInputData.values));
                }
                else
                {
                    if (Convert.ToString(TempCompData.comp_master.System_function ?? "").Trim().Length > 0)
                    {
                        MethodInfo methodInfo = _type.GetMethod(TempCompData.comp_master.System_function);
                        if (methodInfo == null)
                        {
                            throw new Exception("System function " + TempCompData.comp_master.System_function + " not implemented");
                        }
                        else
                        {
                            Sb.AppendFormat("SET {0}:= '{1}'; ", TempCompData.comp_master.component_name, methodInfo.Invoke(_clsSystemKeysfunction, null));//declare the all variable with Default value
                            //mySqlParameter.Add(new MySqlParameter(string.Format("{0}", TempCompData.comp_master.component_name), methodInfo.Invoke(_clsSystemKeysfunction, null)));

                        }
                    }
                    else
                    {
                        Sb.AppendFormat("SET {0}:= '{1}'; ", TempCompData.comp_master.component_name, TempCompData.comp_master.defaultvalue);//declare the all variable with Default value
                        //mySqlParameter.Add(new MySqlParameter(string.Format("{0}", TempCompData.comp_master.component_name), TempCompData.comp_master.defaultvalue));
                    }
                    //Now Define the another forumla 
                }


                if (TempCompData.comp_master.component_id == _ComponentId && _ComponentId > 0)
                {
                    Sb.AppendFormat("set {0}= {1}; ", TempCompData.comp_master.component_name, _ComponentValue);
                }
                else
                {
                    if (TempCompData.comp_master.is_system_key == 0)
                    {
                        if (TempSalaryInputChangeDatas.Any(p => p.component_id == compoID))
                        {
                            var TempSalaryInputChangeData = TempSalaryInputChangeDatas.FirstOrDefault(p => p.component_id == compoID);
                            Sb.AppendFormat("set {0}= '{1}'; ", TempCompData.comp_master.component_name, TempSalaryInputChangeData.values);
                        }
                        else
                        {
                            Sb.AppendFormat("set {0}= {1}; ", TempCompData.comp_master.component_name, TempCompData.formula);
                        }
                    }
                }



                //Now Select the All varaibale

            }

            for (int Index = 0; Index < TempCompDatas.Count(); Index++)
            {
                compoID = TempCompDatas[Index].comp_master.component_id;
                if (Index > 0)
                {
                    Sb.AppendFormat("union all ");
                }

                Sb.AppendFormat(" select {0} as 'compId','{1}' as 'compName',{1} as 'compValue',0 as 'rate',0 as current_month_value,0 as arrear_value ,{2} as  'component_type'",
                    TempCompDatas[Index].component_id, TempCompDatas[Index].comp_master.component_name, TempCompDatas[Index].comp_master.component_type);
            }

            if (Sb.Length > 0)
            {

                mdl = _context.mdlSalaryInputValues.FromSql(Sb.ToString()).AsNoTracking().ToList();
                if (!CalculateSalary)
                {
                    return mdl;
                }

                double ToalPayrollDay = 30, LopDays = 0, PaidDays = 0;
                string ToalPayrollDaytemp = mdl.Where(p => p.compId == (int)enmOtherComponent.ToalPayrollDay).FirstOrDefault()?.compValue;
                double.TryParse(ToalPayrollDaytemp, out ToalPayrollDay);
                string LopDaystemp = mdl.Where(p => p.compId == (int)enmOtherComponent.LopDays).FirstOrDefault()?.compValue;
                double.TryParse(LopDaystemp, out LopDays);
                PaidDays = ToalPayrollDay - LopDays;
                var paidDays = 0;
                var finalLopDays = 0;
                //var d = _context.tbl_fnf_attendance_dtl.Where(x => x.fnf_mstr.emp_id == _EmpID).ToList().FirstOrDefault();
                //if (d != null)
                //{
                //    paidDays = (int)d.Total_Paid_days;
                //    finalLopDays = (int)d.final_lop_days;
                //}



                if (paidDays > 0)
                {
                    PaidDays = Convert.ToInt32(paidDays);
                }

                if (PaidDays < 0)
                {
                    PaidDays = 0;
                }


                for (int index = 0; index < mdl.Count; index++)
                {
                    if (mdl[index].compId == (int)enmOtherComponent.LopDays)
                    {
                        mdl[index].current_month_value = LopDays;
                        if (finalLopDays > 0)
                        {
                            mdl[index].current_month_value = finalLopDays;
                        }
                    }
                    if (mdl[index].compId == (int)enmOtherComponent.ToalPayrollDay)
                    {
                        mdl[index].current_month_value = ToalPayrollDay;
                    }
                    if (mdl[index].compId == (int)enmOtherComponent.PaidDays)
                    {
                        mdl[index].current_month_value = PaidDays;
                    }

                    double rateTemp = 0;

                    if (mdl[index].component_type == (int)enmComponentType.Income || mdl[index].component_type == (int)enmComponentType.Deduction)
                    {
                        double.TryParse(mdl[index].compValue, out rateTemp);
                        mdl[index].rate = rateTemp;
                        if (mdl[index].compId == (int)enmOtherComponent.EsicAmount || mdl[index].compId == (int)enmOtherComponent.EmployerEsicAmount)
                        {
                            mdl[index].current_month_value = Math.Ceiling((rateTemp * PaidDays) / ToalPayrollDay);
                        }
                        else
                        {
                            mdl[index].current_month_value = Math.Round((rateTemp * PaidDays) / ToalPayrollDay);
                        }

                    }
                    else if (mdl[index].component_type == (int)enmComponentType.OtherIncome || mdl[index].component_type == (int)enmComponentType.OtherDeduction)
                    {
                        double.TryParse(mdl[index].compValue, out rateTemp);
                        mdl[index].current_month_value = rateTemp;
                    }
                }

                calculatePf(mdl, PaidDays, ToalPayrollDay);
                calculateArrear(mdl);
                int indexinner = -1;
                double CurrentNet = 0, Arrearnet = 0;

                indexinner = mdl.FindIndex(p => p.compId == (int)enmOtherComponent.GrossSalary);
                if (indexinner >= 0)
                {
                    mdl[indexinner].rate = Convert.ToDouble(mdl[indexinner].compValue);
                    CurrentNet = mdl[indexinner].current_month_value = mdl.Where(p => p.component_type == (int)enmComponentType.Income || p.component_type == (int)enmComponentType.OtherIncome).Sum(p => p.current_month_value);
                    Arrearnet = mdl[indexinner].arrear_value = mdl.Where(p => p.component_type == (int)enmComponentType.Income || p.component_type == (int)enmComponentType.OtherIncome).Sum(p => p.arrear_value);

                }
                indexinner = -1;
                indexinner = mdl.FindIndex(p => p.compId == (int)enmOtherComponent.TotalGross);
                if (indexinner >= 0)
                {
                    mdl[indexinner].compValue = (Arrearnet + CurrentNet).ToString();
                    mdl[indexinner].current_month_value = CurrentNet + Arrearnet;
                }

                indexinner = -1;
                indexinner = mdl.FindIndex(p => p.compId == (int)enmOtherComponent.Deduction);
                if (indexinner >= 0)
                {
                    mdl[indexinner].current_month_value = mdl.Where(p => p.component_type == (int)enmComponentType.Deduction || p.component_type == (int)enmComponentType.OtherDeduction).Sum(p => p.current_month_value);
                    mdl[indexinner].arrear_value = mdl.Where(p => p.component_type == (int)enmComponentType.Deduction || p.component_type == (int)enmComponentType.OtherDeduction).Sum(p => p.arrear_value);

                    CurrentNet = CurrentNet - (mdl[indexinner].current_month_value);
                    Arrearnet = Arrearnet - (mdl[indexinner].arrear_value);
                }
                indexinner = -1;
                indexinner = mdl.FindIndex(p => p.compId == (int)enmOtherComponent.Net);
                if (indexinner >= 0)
                {
                    mdl[indexinner].arrear_value = Arrearnet;
                    mdl[indexinner].current_month_value = CurrentNet;
                }



            }
            return mdl;
        }

        private double GetModelValue(List<mdlSalaryInputValues> mdl, enmOtherComponent otherComponent, byte LoadType = 0)
        {
            double ReturnData = 0;
            int index = -1;
            index = mdl.FindIndex(p => p.compId == (int)otherComponent);
            if (index >= 0)
            {
                if (LoadType == 0)
                {
                    double.TryParse(mdl[index].compValue, out ReturnData);
                }
                else if (LoadType == 1)
                {
                    return mdl[index].current_month_value;
                }
                else if (LoadType == 2)
                {
                    return mdl[index].arrear_value;
                }

            }

            return ReturnData;
        }

        private void calculatePf(List<mdlSalaryInputValues> mdl, double paidDays, double totalpayrollday)
        {
            byte is_pf_applicable = 0, is_vpf_applicable = 0, is_Eps_applicable = 0;
            enmPFGroup PFGroup = enmPFGroup.PercentageOnMinBasicSlab;
            enmVPFGroup vPFGroup = enmVPFGroup.FixedAmount;
            double BasicSalary = 0, PfSalarySlab = 0, pf_basic = 0, Pf_ceiling = 0;

            double PfPercentage = 0, Employer_pf_percentage = 0, Employer_Pension_Scheme_percentage = 0,
                Employer_EDLIS_percetage = 0, EPF_Administration_charges_percentage = 0, EDLIS_Administration_charges_percentage = 0,
                vpf_percentage = 0;
            double PFAmount = 0;

            //calculate Pf
            is_pf_applicable = Convert.ToByte(GetModelValue(mdl, enmOtherComponent.PfApplicableManual));
            Pf_ceiling = GetModelValue(mdl, enmOtherComponent.pf_celling);

            BasicSalary = GetModelValue(mdl, enmOtherComponent.BasicSalary, 1);
            if (Pf_ceiling == 0)
            {
                Pf_ceiling = BasicSalary;
            }
            if (is_pf_applicable == 1)
            {
                PFGroup = (enmPFGroup)GetModelValue(mdl, enmOtherComponent.PfGroup);
                PfPercentage = GetModelValue(mdl, enmOtherComponent.PfPercentage);
                Employer_pf_percentage = GetModelValue(mdl, enmOtherComponent.Employer_pf_percentage);
                Employer_Pension_Scheme_percentage = GetModelValue(mdl, enmOtherComponent.Employer_Pension_Scheme_percentage);
                Employer_EDLIS_percetage = GetModelValue(mdl, enmOtherComponent.Employer_EDLIS_percetage);
                EPF_Administration_charges_percentage = GetModelValue(mdl, enmOtherComponent.EPF_Administration_charges_percentage);
                EDLIS_Administration_charges_percentage = GetModelValue(mdl, enmOtherComponent.EDLIS_Administration_charges_percentage);
                is_Eps_applicable = Convert.ToByte(GetModelValue(mdl, enmOtherComponent.EpsApplicable));
                if (PFGroup == enmPFGroup.PercentageOnMinBasicSlab)
                {
                    PfSalarySlab = GetModelValue(mdl, enmOtherComponent.PfSalarySlab);
                    if (BasicSalary >= PfSalarySlab)
                    {
                        pf_basic = PfSalarySlab;
                    }
                    else
                    {
                        pf_basic = BasicSalary;
                    }

                }
                else
                {
                    pf_basic = BasicSalary;
                }
                if (pf_basic < Pf_ceiling)
                {
                    Pf_ceiling = pf_basic;
                }
                for (int i = 0; i < mdl.Count; i++)
                {
                    if (mdl[i].compId == (int)enmOtherComponent.Pf_amount)
                    {

                        mdl[i].current_month_value = Math.Round(pf_basic * PfPercentage / 100.0);
                        PFAmount = mdl[i].current_month_value;
                        mdl[i].compValue = Convert.ToString(PFAmount);
                    }
                    else if (mdl[i].compId == (int)enmOtherComponent.Employer_pf_amount)
                    {
                        if (is_Eps_applicable == 1)
                        {
                            mdl[i].current_month_value = Math.Round(pf_basic * Employer_pf_percentage / 100.0);
                            mdl[i].compValue = Convert.ToString(mdl[i].current_month_value);
                        }
                        else
                        {
                            mdl[i].current_month_value = Math.Round(pf_basic * (Employer_pf_percentage + Employer_Pension_Scheme_percentage) / 100.0);
                            mdl[i].compValue = Convert.ToString(mdl[i].current_month_value);
                        }

                    }
                    else if (mdl[i].compId == (int)enmOtherComponent.Employer_Pension_Scheme_amount)
                    {
                        if (is_Eps_applicable == 1)
                        {
                            mdl[i].current_month_value = Math.Round(pf_basic * Employer_Pension_Scheme_percentage / 100.0);
                            mdl[i].compValue = Convert.ToString(mdl[i].current_month_value);
                        }
                        else
                        {
                            mdl[i].current_month_value = 0;
                            mdl[i].compValue = "0";
                        }
                    }
                    else if (mdl[i].compId == (int)enmOtherComponent.Employer_EDLIS_amount)
                    {
                        mdl[i].current_month_value = Math.Round(pf_basic * Employer_EDLIS_percetage / 100.0);
                        mdl[i].compValue = Convert.ToString(mdl[i].current_month_value);
                    }
                    else if (mdl[i].compId == (int)enmOtherComponent.EPF_Administration_charges_amount)
                    {
                        mdl[i].current_month_value = Math.Round(pf_basic * EPF_Administration_charges_percentage / 100.0);
                        mdl[i].compValue = Convert.ToString(mdl[i].current_month_value);
                    }
                    else if (mdl[i].compId == (int)enmOtherComponent.EDLIS_Administration_charges_amount)
                    {
                        mdl[i].current_month_value = Math.Round(pf_basic * EDLIS_Administration_charges_percentage / 100.0);
                        mdl[i].compValue = Convert.ToString(mdl[i].current_month_value);
                    }
                }

            }

            is_vpf_applicable = (byte)GetModelValue(mdl, enmOtherComponent.Vpf_applicable);
            if (is_vpf_applicable == 1)
            {
                vPFGroup = (enmVPFGroup)GetModelValue(mdl, enmOtherComponent.VPfGroup);
                if (vPFGroup == enmVPFGroup.BasicPercentage)
                {
                    vpf_percentage = GetModelValue(mdl, enmOtherComponent.vpf_percentage);
                    int index = mdl.FindIndex(p => p.compId == (uint)enmOtherComponent.Vpf_amount);
                    if (index >= 0)
                    {
                        mdl[index].current_month_value = Math.Round(BasicSalary * vpf_percentage / 100.0);
                        if ((mdl[index].current_month_value + PFAmount) > Pf_ceiling)
                        {
                            mdl[index].current_month_value = Pf_ceiling - PFAmount;
                        }
                        mdl[index].compValue = Convert.ToString(mdl[index].current_month_value);
                    }
                }
                else
                {
                    vpf_percentage = GetModelValue(mdl, enmOtherComponent.vpf_percentage);
                    int index = mdl.FindIndex(p => p.compId == (uint)enmOtherComponent.Vpf_amount);
                    if (index >= 0)
                    {
                        mdl[index].current_month_value = vpf_percentage;
                        if ((mdl[index].current_month_value + PFAmount) > Pf_ceiling)
                        {
                            mdl[index].current_month_value = Pf_ceiling - PFAmount;
                        }
                        mdl[index].compValue = Convert.ToString(mdl[index].current_month_value);
                    }
                }

            }
        }

        private void calculatearrearPf(List<mdlSalaryInputValues> arrearmdl, List<mdlSalaryInputValues> mdl, double arrearpaidDays, double arreartotalpayrollday)
        {

            byte is_pf_applicable = 0, is_Eps_applicable = 0;
            enmPFGroup PFGroup = enmPFGroup.PercentageOnMinBasicSlab;
            double BasicSalary = 0, BasicArrear = 0, PfSalarySlab = 0, pf_basic = 0, Pf_ceiling = 0;

            double AlreadyPaidDays = GetModelValue(arrearmdl, enmOtherComponent.PaidDays);

            double PfPercentage = 0, Employer_pf_percentage = 0, Employer_Pension_Scheme_percentage = 0,
                Employer_EDLIS_percetage = 0, EPF_Administration_charges_percentage = 0, EDLIS_Administration_charges_percentage = 0;


            //calculate Pf
            is_pf_applicable = (byte)GetModelValue(arrearmdl, enmOtherComponent.PfApplicableManual);
            BasicSalary = GetModelValue(arrearmdl, enmOtherComponent.BasicSalary);
            BasicArrear = GetModelValue(mdl, enmOtherComponent.BasicSalary, 2);
            Pf_ceiling = GetModelValue(arrearmdl, enmOtherComponent.pf_celling);
            if (is_pf_applicable == 1)
            {
                PFGroup = (enmPFGroup)GetModelValue(arrearmdl, enmOtherComponent.PfGroup);
                PfPercentage = GetModelValue(arrearmdl, enmOtherComponent.PfPercentage);
                Employer_pf_percentage = GetModelValue(arrearmdl, enmOtherComponent.Employer_pf_percentage);
                Employer_Pension_Scheme_percentage = GetModelValue(arrearmdl, enmOtherComponent.Employer_Pension_Scheme_percentage);
                Employer_EDLIS_percetage = GetModelValue(arrearmdl, enmOtherComponent.Employer_EDLIS_percetage);
                EPF_Administration_charges_percentage = GetModelValue(arrearmdl, enmOtherComponent.EPF_Administration_charges_percentage);
                EDLIS_Administration_charges_percentage = GetModelValue(arrearmdl, enmOtherComponent.EDLIS_Administration_charges_percentage);
                is_Eps_applicable = Convert.ToByte(GetModelValue(arrearmdl, enmOtherComponent.EpsApplicable));
                if (PFGroup == enmPFGroup.PercentageOnMinBasicSlab)
                {
                    PfSalarySlab = GetModelValue(arrearmdl, enmOtherComponent.PfSalarySlab);
                    if (Math.Round(BasicSalary * AlreadyPaidDays / arreartotalpayrollday) >= PfSalarySlab)
                    {
                        for (int i = 0; i < mdl.Count; i++)
                        {
                            if (mdl[i].compId == (int)enmOtherComponent.Pf_amount)
                            {
                                mdl[i].arrear_value = 0;
                            }
                            else if (mdl[i].compId == (int)enmOtherComponent.Employer_pf_amount)
                            {
                                mdl[i].arrear_value = 0;
                            }
                            else if (mdl[i].compId == (int)enmOtherComponent.Employer_Pension_Scheme_amount)
                            {
                                mdl[i].arrear_value = 0;
                            }
                            else if (mdl[i].compId == (int)enmOtherComponent.Employer_EDLIS_amount)
                            {
                                mdl[i].arrear_value = 0;
                            }
                            else if (mdl[i].compId == (int)enmOtherComponent.EPF_Administration_charges_amount)
                            {
                                mdl[i].arrear_value = 0;

                            }
                            else if (mdl[i].compId == (int)enmOtherComponent.EDLIS_Administration_charges_amount)
                            {
                                mdl[i].arrear_value = 0;
                            }
                        }
                    }
                    else if ((BasicSalary * (AlreadyPaidDays + arrearpaidDays) / arreartotalpayrollday) <= PfSalarySlab)
                    {
                        pf_basic = PfSalarySlab;
                        for (int i = 0; i < mdl.Count; i++)
                        {
                            if (mdl[i].compId == (int)enmOtherComponent.Pf_amount)
                            {
                                mdl[i].arrear_value = Math.Round(BasicArrear * PfPercentage / 100.0);
                            }
                            else if (mdl[i].compId == (int)enmOtherComponent.Employer_pf_amount)
                            {
                                if (is_Eps_applicable == 1)
                                {
                                    mdl[i].arrear_value = Math.Round(BasicArrear * Employer_pf_percentage / 100.0);
                                }
                                else
                                {
                                    mdl[i].arrear_value = Math.Round(BasicArrear * (Employer_pf_percentage + Employer_Pension_Scheme_percentage) / 100.0);
                                }


                            }
                            else if (mdl[i].compId == (int)enmOtherComponent.Employer_Pension_Scheme_amount)
                            {
                                if (is_Eps_applicable == 1)
                                {
                                    mdl[i].arrear_value = Math.Round(BasicArrear * Employer_Pension_Scheme_percentage / 100.0);
                                }
                                else
                                {
                                    mdl[i].arrear_value = 0;
                                }
                            }
                            else if (mdl[i].compId == (int)enmOtherComponent.Employer_EDLIS_amount)
                            {
                                mdl[i].arrear_value = Math.Round(BasicArrear * Employer_EDLIS_percetage / 100.0);

                            }
                            else if (mdl[i].compId == (int)enmOtherComponent.EPF_Administration_charges_amount)
                            {
                                mdl[i].arrear_value = Math.Round(BasicArrear * EPF_Administration_charges_percentage / 100.0);

                            }
                            else if (mdl[i].compId == (int)enmOtherComponent.EDLIS_Administration_charges_amount)
                            {
                                mdl[i].arrear_value = Math.Round(BasicArrear * EDLIS_Administration_charges_percentage / 100.0);

                            }
                        }
                    }
                    else
                    {
                        pf_basic = PfSalarySlab - (BasicSalary * AlreadyPaidDays / arreartotalpayrollday);
                        for (int i = 0; i < mdl.Count; i++)
                        {
                            if (mdl[i].compId == (int)enmOtherComponent.Pf_amount)
                            {
                                mdl[i].arrear_value = Math.Round(pf_basic * PfPercentage / 100.0);
                            }
                            else if (mdl[i].compId == (int)enmOtherComponent.Employer_pf_amount)
                            {
                                if (is_Eps_applicable == 1)
                                {
                                    mdl[i].arrear_value = Math.Round(pf_basic * Employer_pf_percentage / 100.0);
                                }
                                else
                                {
                                    mdl[i].arrear_value = Math.Round(pf_basic * (Employer_pf_percentage + Employer_Pension_Scheme_percentage) / 100.0);
                                }
                            }
                            else if (mdl[i].compId == (int)enmOtherComponent.Employer_Pension_Scheme_amount)
                            {
                                if (is_Eps_applicable == 1)
                                {
                                    mdl[i].arrear_value = Math.Round(pf_basic * Employer_Pension_Scheme_percentage / 100.0);
                                }
                                else
                                {
                                    mdl[i].arrear_value = 0;
                                }


                            }
                            else if (mdl[i].compId == (int)enmOtherComponent.Employer_EDLIS_amount)
                            {
                                mdl[i].arrear_value = Math.Round(pf_basic * Employer_EDLIS_percetage / 100.0);

                            }
                            else if (mdl[i].compId == (int)enmOtherComponent.EPF_Administration_charges_amount)
                            {
                                mdl[i].arrear_value = Math.Round(pf_basic * EPF_Administration_charges_percentage / 100.0);

                            }
                            else if (mdl[i].compId == (int)enmOtherComponent.EDLIS_Administration_charges_amount)
                            {
                                mdl[i].arrear_value = Math.Round(pf_basic * EDLIS_Administration_charges_percentage / 100.0);

                            }
                        }
                    }
                }
                else
                {

                    for (int i = 0; i < mdl.Count; i++)
                    {
                        if (mdl[i].compId == (int)enmOtherComponent.Pf_amount)
                        {
                            mdl[i].arrear_value = Math.Round(BasicArrear * PfPercentage / 100.0);
                        }
                        else if (mdl[i].compId == (int)enmOtherComponent.Employer_pf_amount)
                        {
                            if (is_Eps_applicable == 1)
                            {
                                mdl[i].arrear_value = Math.Round(BasicArrear * Employer_pf_percentage / 100.0);

                            }
                            else
                            {
                                mdl[i].arrear_value = Math.Round(BasicArrear * (Employer_pf_percentage + Employer_Pension_Scheme_percentage) / 100.0);
                            }


                        }
                        else if (mdl[i].compId == (int)enmOtherComponent.Employer_Pension_Scheme_amount)
                        {
                            if (is_Eps_applicable == 1)
                            {
                                mdl[i].arrear_value = Math.Round(BasicArrear * Employer_Pension_Scheme_percentage / 100.0);

                            }
                            else
                            {
                                mdl[i].arrear_value = 0;
                            }


                        }
                        else if (mdl[i].compId == (int)enmOtherComponent.Employer_EDLIS_amount)
                        {
                            mdl[i].arrear_value = Math.Round(BasicArrear * Employer_EDLIS_percetage / 100.0);

                        }
                        else if (mdl[i].compId == (int)enmOtherComponent.EPF_Administration_charges_amount)
                        {
                            mdl[i].arrear_value = Math.Round(BasicArrear * EPF_Administration_charges_percentage / 100.0);

                        }
                        else if (mdl[i].compId == (int)enmOtherComponent.EDLIS_Administration_charges_amount)
                        {
                            mdl[i].arrear_value = Math.Round(BasicArrear * EDLIS_Administration_charges_percentage / 100.0);
                        }
                    }
                }
            }

        }

        private List<mdlSalaryInputValues> GetArrearModel(List<mdlSalaryInputValues> Currentmdl, int Monthyear)
        {
            List<mdlSalaryInputValues> mdl = new List<mdlSalaryInputValues>();
            if (Monthyear == 0)
            {
                return Currentmdl;
            }
            else
            {
                mdl = _context.tbl_salary_input.Where(p => p.monthyear == Monthyear && p.emp_id == _EmpID && p.is_active == 1)
                   .Select(p => new mdlSalaryInputValues { compId = p.component_id ?? 0, compValue = p.values, rate = p.rate, current_month_value = p.current_month_value, component_type = p.component_type }).ToList();
                if (mdl.Count == 0)
                {
                    return Currentmdl;
                }
                else
                {
                    return mdl;
                }
            }


        }

        private void calculateArrear(List<mdlSalaryInputValues> mdl)
        {
            int index = -1;
            double arrearDays = 0, ToalarrearPayrollDay = 1;
            int arrearMonthyear = 0;
            arrearDays = GetModelValue(mdl, enmOtherComponent.ArrearDays);
            if (arrearDays > 0)
            {
                arrearMonthyear = (int)GetModelValue(mdl, enmOtherComponent.ArrearMonthYear);
                var arrearModel = GetArrearModel(mdl, arrearMonthyear);
                ToalarrearPayrollDay = GetModelValue(arrearModel, enmOtherComponent.ToalPayrollDay);
                if (ToalarrearPayrollDay == 0)
                {
                    ToalarrearPayrollDay = 30;
                }
                for (int i = 0; i < arrearModel.Count; i++)
                {
                    if (arrearModel[i].component_type == (int)enmComponentType.Income || arrearModel[i].component_type == (int)enmComponentType.Deduction)
                    {
                        index = -1;
                        //check Model contain this component or not
                        index = mdl.FindIndex(p => p.compId == arrearModel[i].compId);
                        if (index == -1)
                        {
                            index = mdl.Count;
                            mdl.Add(new mdlSalaryInputValues()
                            {
                                compId = arrearModel[i].compId,
                                compValue = "0",
                                rate = arrearModel[i].rate,
                                arrear_value = 0,
                                current_month_value = 0,
                                component_type = arrearModel[i]
                            .component_type
                            });
                        }
                        mdl[index].arrear_value = Math.Round(arrearModel[i].rate * arrearDays / ToalarrearPayrollDay);
                    }
                }

                //calculate PF value for Arrea
                calculatearrearPf(arrearModel, mdl, arrearDays, ToalarrearPayrollDay);

            }
        }




    }

    public class clsSystemKeysfunction
    {

        IConfiguration _config;

        private readonly int _EmpID;
        private readonly int _SepId;
        private readonly int _Monthyear;
        private readonly Context _context;
        private readonly DateTime _ApplicableDate;
        private readonly int _SalaryGroupId;

        private byte _EsicApplicable, _PFApplicable, _VpfApplicable, _EpsApplicable;
        private int _EmpDeptId, _EmpLocationId, _no_of_child, _CompanyId, _ToalPayrollDay, _EmpGradeId, _EmpDesignationId, _PfGroupId, _VpfGroupId, _EL_Encashment, _Comp_off_Encasment, _AdditionalPaidDays, _NoticePaymentDay, _NoticeRecoveryDay;
        private double _LOD, _EMI, _Salary, _IncomeTax, _OTRate, _OTHour, _PFCeling;
        private double _EsicPercentage, _EmployerEsicPercentage;
        private double _PfSalarySlab, _PfPercentage, _Employer_pf_percentage, _Employer_Pension_Scheme_percentage, _Employer_EDLIS_percetage, _EPF_Administration_charges_percentage, _EDLIS_Administration_charges_percentage, _VpfPercentage;
        private string _EMP_Code, _EmpJoiningDt, _EmpLastDate, _EmpDOB, _Empcard_number, _EmpStatus, _EmpName, _EmpFatherHusbandName, _Gender;
        private string _EMPGradeName, _EmpDesignation, _EmpDepartment, _EmpLocation, _EmpWorkingState;
        private string _BankAccountNo, _IFSCCode, _BankName, _EmpEmailId;
        private string _AdharNo, _AdharName, _PanNo, _PanName, _UanNo, _EsicNo, _PfNo, _Nationality;
        private string _EducationLevel, _CompanyName, _EmpSalaryGroupName, _CompanyLogoPath, _EmpContact, _EmpAddress;

        private async Task LoadCompany()
        {
            using (var context = new Context())
            {
                _CompanyId = 1;// context.tbl_user_master.Where(a => a.employee_id == _EmpID && a.is_active == 1).Select(a => a.default_company_id).FirstOrDefault();
                if (_CompanyId > 0)
                {
                    var tempdata = context.tbl_company_master.FirstOrDefault(a => a.company_id == _CompanyId);
                    if (tempdata != null)
                    {
                        _CompanyName = tempdata.company_name + " - " + tempdata.company_code;
                        _CompanyLogoPath = tempdata.company_logo;
                    }
                }
            }
        }

        private async Task LoadEmpOfficialSection()
        {
            throw new NotImplementedException();
#if false
            using (var context = new Context())
            {
                var empmaster = context.tbl_employee_master.Where(p => p.employee_id == _EmpID).FirstOrDefault();
                if (empmaster != null)
                {
                    _EMP_Code = empmaster.emp_code;
                }
                var empDetails = context.tbl_emp_officaial_sec.Where(p => p.employee_id == _EmpID && p.is_deleted == 0).FirstOrDefault();
                if (empDetails != null)
                {
                    if (empDetails.gender == 1)
                    {
                        _Gender = "Female";
                    }
                    if (empDetails.gender == 2)
                    {
                        _Gender = "Male";
                    }
                    else
                        _Gender = "TransGender";
                    _Nationality = empDetails.nationality;
                    ///set the name
                    _EmpName = empDetails.employee_first_name + " " + empDetails.employee_middle_name + " " + empDetails.employee_last_name;
                    _EmpEmailId = empDetails.official_email_id;
                    //Set the joining date
                    _EmpJoiningDt = empDetails.date_of_joining.ToString("dd-MM-yyyy");

                    //Set the Last Working date
                    if (!(empDetails.last_working_date >= Convert.ToDateTime("2100-01-01")))
                    {
                        _EmpLastDate = empDetails.last_working_date.ToString("dd-MM-yyyy");
                    }

                    switch (empDetails.current_employee_type)
                    {
                        case 1: _EmpStatus = "Temporary"; break;
                        case 2: _EmpStatus = "Probation"; break;
                        case 3: _EmpStatus = "Confirmed"; break;
                        case 4: _EmpStatus = "Contract"; break;
                        case 10: _EmpStatus = "Notice"; break;
                        case 99: _EmpStatus = "FNF"; break;
                        case 100: _EmpStatus = "Terminate"; break;
                    }


                    // Set the DOB
                    _EmpDOB = empDetails.date_of_birth.ToString("dd-MM-yyyy");

                    // Set Emp Card Number
                    _Empcard_number = empDetails.card_number;

                    // Set Emp Department
                    _EmpDeptId = empDetails.department_id ?? 0;
                    var dept = context.tbl_department_master.Where(p => p.department_id == _EmpDeptId).FirstOrDefault();
                    if (dept != null)
                    {
                        _EmpDepartment = dept.department_name;
                    }

                    _EmpLocationId = empDetails.location_id ?? 0;
                    var location = context.tbl_location_master.Where(a => a.location_id == _EmpLocationId).FirstOrDefault();
                    if (location != null)
                    {
                        _EmpLocation = location.location_name;
                        var stateid = location.state_id ?? 0;
                        var states = context.tbl_state.Where(p => p.state_id == stateid).FirstOrDefault();
                        if (states != null)
                        {
                            _EmpWorkingState = states.name;
                        }

                    }
                }
            }
#endif
        }

        private async Task LoadBankDetails()
        {
            using (var context = new Context())
            {
                var BankData = _context.tbl_emp_bank_details.Where(p => p.employee_id == _EmpID && p.is_deleted == 0).Select(p => new { p.bank_mstr.bank_name, p.branch_name, p.bank_acc, p.ifsc_code }).FirstOrDefault();
                if (BankData != null)
                {
                    _BankName = BankData.bank_name;
                    _BankAccountNo = BankData.bank_acc;
                    _IFSCCode = BankData.ifsc_code;
                }
            }
        }


        private async Task LoadEmpPersonalSection()
        {
            using (var context = new Context())
            {
                var PanDetails = context.tbl_emp_pan_details.Where(p => p.employee_id == _EmpID && p.is_deleted == 0).FirstOrDefault();
                if (PanDetails != null)
                {
                    _PanName = PanDetails.pan_card_name;
                    _PanNo = PanDetails.pan_card_number;
                }
                var AdharDetails = context.tbl_emp_adhar_details.Where(p => p.employee_id == _EmpID && p.is_deleted == 0).FirstOrDefault();
                if (AdharDetails != null)
                {
                    _AdharName = AdharDetails.aadha_card_name;
                    _AdharNo = AdharDetails.aadha_card_number;
                }
                var EsicDetails = _context.tbl_emp_esic_details.Where(p => p.employee_id == _EmpID && p.is_deleted == 0).FirstOrDefault();
                if (EsicDetails != null)
                {
                    _EsicApplicable = EsicDetails.is_esic_applicable;
                    _EsicNo = EsicDetails.esic_number;
                }

                var empPfDetail = _context.tbl_emp_pf_details.Where(p => p.employee_id == _EmpID && p.is_deleted == 0).FirstOrDefault();
                if (empPfDetail != null)
                {

                    _PFApplicable = empPfDetail.is_pf_applicable;
                    _EpsApplicable = empPfDetail.is_eps_applicable;
                    _PfGroupId = (int)empPfDetail.pf_group;
                    _VpfGroupId = (int)empPfDetail.vpf_Group;
                    _PfNo = empPfDetail.pf_number;
                    _UanNo = empPfDetail.uan_number;
                    _PFCeling = empPfDetail.pf_celing;
                    _VpfApplicable = empPfDetail.is_vpf_applicable;
                    _VpfPercentage = empPfDetail.vpf_amount;
                }


                var empPerDetails = context.tbl_emp_personal_sec.Where(p => p.employee_id == _EmpID && p.is_deleted == 0).FirstOrDefault();
                if (empPerDetails != null)
                {
                    _EmpContact = empPerDetails.primary_contact_number;
                    _EmpAddress = empPerDetails.permanent_address_line_one + empPerDetails.permanent_address_line_two;

                }
                //    //_PanName = empPerDetails.pan_card_name;
                //    //_PanNo = empPerDetails.pan_card_number;
                //    //_AdharNo = empPerDetails.aadha_card_number;
                //    //_AdharName = empPerDetails.aadha_card_name;
                //    //_EsicNo = empPerDetails.esic;
                //    //_PfNo = empPerDetails.pf_number;

                //    //if (empPerDetails.bank_id > 0)
                //    //{
                //    //    var bankdata = context.tbl_bank_master.Where(p => p.bank_id == empPerDetails.bank_id).FirstOrDefault();
                //    //    if (bankdata != null)
                //    //    {
                //    //        _BankName = bankdata.bank_name;
                //    //    }
                //    //}

                //    //_IFSCCode = empPerDetails.ifsc_code;
                //    //_BankAccountNo = empPerDetails.bank_acc;
                //    //_UanNo = empPerDetails.uan;
                //    //_PFApplicable = empPerDetails.pf_applicable == 1 ? "Yes" : "No";
                //    //_PFCeling = empPerDetails.pf_ceilling;
                //    //_PfGroupId = empPerDetails.pf_group;
                //}
            }
        }

        private async Task LoadGradeDesignation()
        {
            using (var context = new Context())
            {
                var empdesgi = context.tbl_emp_desi_allocation.Where(p => p.employee_id == _EmpID && p.applicable_from_date <= _ApplicableDate && p.applicable_to_date >= _ApplicableDate).FirstOrDefault();
                if (empdesgi != null)
                {
                    _EmpDesignationId = empdesgi.desig_id ?? 0;
                    if (_EmpDesignationId > 0)
                    {
                        var temp = context.tbl_designation_master.Where(p => p.designation_id == _EmpDesignationId).FirstOrDefault();
                        if (temp != null)
                        {
                            _EmpDesignation = temp.designation_name;
                        }
                    }
                }
                var empgrade = context.tbl_emp_grade_allocation.Where(p => p.employee_id == _EmpID && p.applicable_from_date <= _ApplicableDate && p.applicable_to_date >= _ApplicableDate).FirstOrDefault();
                if (empgrade != null)
                {
                    _EmpGradeId = empgrade.grade_id ?? 0;
                    if (_EmpGradeId > 0)
                    {
                        var temp = context.tbl_grade_master.Where(p => p.grade_id == _EmpGradeId).FirstOrDefault();
                        if (temp != null)
                        {
                            _EMPGradeName = temp.grade_name;
                        }
                    }
                }
            }
        }

        private async Task LoadEmpFamilySection()
        {
            using (var context = new Context())
            {
                var tempdata = context.tbl_emp_family_sec.Where(a => a.employee_id == _EmpID && a.is_deleted == 0).ToList();
                if (!(tempdata == null || tempdata.Count == 0))
                {
                    var fatherdata = tempdata.FirstOrDefault(p => p.relation.Trim().ToLower() == "father");
                    if (fatherdata != null)
                    {
                        _EmpFatherHusbandName = fatherdata.name_as_per_aadhar_card.Trim();
                    }

                    var Husbanddata = tempdata.FirstOrDefault(p => p.relation.Trim().ToLower() == "husband");
                    if (Husbanddata != null)
                    {
                        _EmpFatherHusbandName = Husbanddata.name_as_per_aadhar_card.Trim();
                    }
                    //No of Child
                    _no_of_child = tempdata.Where(p => p.relation.Trim().ToLower() == "child").Count();

                }
            }

        }

        private async Task LoadEmpEducation()
        {
            using (var context = new Context())
            {
                _EducationLevel = "Not Educated";

                var tempdata = context.tbl_emp_qualification_sec.Where(a => a.employee_id == _EmpID && a.is_deleted == 0).ToList();
                if (!(tempdata == null || tempdata.Count == 0))
                {
                    var edulelvel = tempdata.Max(p => p.education_level);
                    switch (edulelvel)
                    {
                        case 1: _EducationLevel = "Not Educated"; break;
                        case 2: _EducationLevel = "Primary Education"; break;
                        case 3: _EducationLevel = "Secondary"; break;
                        case 4: _EducationLevel = "Sr Secondary"; break;
                        case 5: _EducationLevel = "Graduation"; break;
                        case 6: _EducationLevel = "Post Graduation"; break;
                        case 7: _EducationLevel = "Doctorate"; break;
                    }
                }
            }
        }

        private async Task LoadEmpEMI()
        {
            using (var context = new Context())
            {
                _EMI = context.tbl_loan_repayments.Where(a => a.date.ToString("yyyyMM") == _Monthyear.ToString() && a.is_deleted == 0 && a.req_emp_id == _EmpID && a.status == 0).
              Select(a => new { monthly_emi = a.interest_component + a.principal_amount }).Sum(p => p.monthly_emi);
            }
        }

        private async Task LoadEmpSalary()
        {
            using (var context = new Context())
            {
                _Salary = (double)context.tbl_emp_salary_master.Where(p => p.emp_id == _EmpID && p.is_active == 1 && p.applicable_from_dt.Date <= _ApplicableDate).OrderByDescending(a => a.applicable_from_dt).Select(p => p.salaryrevision).DefaultIfEmpty(0).FirstOrDefault();
            }
        }

        private async Task LoadTotalPayrollDay()
        {
            using (var context = new Context())
            {
                _ToalPayrollDay = 30;
                int get_loss_of_day_setting_data = context.tbl_lossofpay_setting.Where(a => a.companyid == _CompanyId && a.is_active == 1).Select(a => a.lop_setting_name).DefaultIfEmpty(0).FirstOrDefault();
                if (get_loss_of_day_setting_data == 1)
                {
                    DateTime FromDate = new DateTime(_ApplicableDate.Year, _ApplicableDate.Month, 1);
                    DateTime ToDate = FromDate.AddMonths(1);
                    _ToalPayrollDay = (ToDate - FromDate).Days;

                    //var d = context.tbl_lossofpay_master.Where(a => a.emp_id == _EmpID && a.is_active == 1 && a.fkid_sepration == _SepId).FirstOrDefault();
                    //if (d != null)
                    //{
                    //    _ToalPayrollDay = (int)d.totaldays;

                    //}
                    //if (_EL_Encashment > 0) _ToalPayrollDay += _EL_Encashment;
                    //if (_Comp_off_Encasment > 0) _ToalPayrollDay += _Comp_off_Encasment;

                }
            }
        }

        private async Task LoadLosOfDay()
        {
            using (var context = new Context())
            {
                _LOD = Convert.ToDouble(context.tbl_lossofpay_master.Where(a => a.monthyear == _Monthyear && a.emp_id == _EmpID && a.is_active == 1).Select(b => b.final_lop_days).FirstOrDefault());
                _AdditionalPaidDays = 0;
                var d = context.tbl_lossofpay_master.Where(a => a.emp_id == _EmpID && a.is_active == 1 && a.monthyear == _Monthyear &&
                a.fkid_sepration == _SepId).FirstOrDefault();
                if (d != null)
                {
                    _LOD = (int)d.final_lop_days;
                    _AdditionalPaidDays = (int)d.Additional_Paid_days;
                }

            }
        }
        private async Task LoadLeaveEncashment()
        {
            _EL_Encashment = 0;
            _Comp_off_Encasment = 0;
            _NoticePaymentDay = 0;
            _NoticeRecoveryDay = 0;
            using (var context = new Context())
            {
                var leave_data = context.tbl_fnf_leave_encash.Where(a => a.fnf_mstr.emp_id == _EmpID && a.fnf_mstr.fkid_empSepration == _SepId && a.is_deleted == 0).ToList();
                if (leave_data.Count>0)
                {
                    _EL_Encashment = Convert.ToInt32(leave_data.Where(x => x.leave_type_id == 2).FirstOrDefault().leave_encash_day);
                    _Comp_off_Encasment = Convert.ToInt32(leave_data.Where(x => x.leave_type_id == 4).FirstOrDefault().leave_encash_day);
                }
                var notice_Data = context.tbl_fnf_master.Where(a => a.emp_id == _EmpID && a.emp_sep_mstr.sepration_id == _SepId && a.is_deleted == 0).ToList();
                if (notice_Data.Count > 0)
                {
                    _NoticePaymentDay = Convert.ToInt32(notice_Data.FirstOrDefault().notice_payment_days);
                    _NoticeRecoveryDay = Convert.ToInt32(notice_Data.FirstOrDefault().notice_recovery_days);
                }
            }
        }

        private async Task LoadIncomeTax()
        {
            using (var context = new Context())
            {

                _IncomeTax = context.tbl_employee_income_tax_amount.Where(a => a.emp_id == _EmpID && a.is_deleted == 0).Select(b => b.income_tax_amount).DefaultIfEmpty(0).FirstOrDefault();
            }
        }

        private async Task LoadSalaryGroupName()
        {
            using (var context = new Context())
            {
                var tempdata = context.tbl_salary_group.Where(a => a.group_id == _SalaryGroupId).FirstOrDefault();
                if (tempdata != null)
                {
                    _EmpSalaryGroupName = tempdata.group_name;
                }
            }
        }

        private async Task LoadOTRate_OtHours()
        {
            using (var context = new Context())
            {
                _OTRate = 0.0;

                var tempdata = context.tbl_ot_rate_details.Where(a => a.emp_id == _EmpID && a.is_active == 1).FirstOrDefault();
                if (tempdata != null)
                {
                    _OTRate = tempdata.ot_amt;
                }
                else
                {
                    _OTRate = context.tbl_ot_rate_details.Where(a => a.grade_id == _EmpGradeId && a.is_active == 1).Select(b => b.ot_amt).FirstOrDefault();
                }
                _OTHour = context.tbl_ot_rule_master.Where(a => a.is_active == 1).Select(b => b.grace_working_hour).FirstOrDefault();

            }
        }

        private async void LoadAlldata()
        {
            //Load data from Configuration
            _EsicPercentage = Convert.ToDouble(_config["Esic:EmployeePercentage"]);
            _EmployerEsicPercentage = Convert.ToDouble(_config["Esic:EmployerPercentage"]);

            _PfSalarySlab = Convert.ToDouble(_config["PF:MinPfSlab"]);
            _PfPercentage = Convert.ToDouble(_config["PF:Employee:EpfPercentage"]);
            _Employer_pf_percentage = Convert.ToDouble(_config["PF:Employer:EpfPercentage"]);
            _Employer_Pension_Scheme_percentage = Convert.ToDouble(_config["PF:Employer:EpsPercentage"]);
            _Employer_EDLIS_percetage = Convert.ToDouble(_config["PF:Employer:EdlisPercentage"]);
            _EPF_Administration_charges_percentage = Convert.ToDouble(_config["PF:Employer:AdminCharge:EpfPercentage"]);
            _EDLIS_Administration_charges_percentage = Convert.ToDouble(_config["PF:Employer:AdminCharge:EdlisPercentage"]);

            var taskLoadCompany = LoadCompany();
            var taskLoadEmpOfficialSection = LoadEmpOfficialSection();
            var taskLoadEmpPersonalSection = LoadEmpPersonalSection();
            var taskLoadBankDetails = LoadBankDetails();
            var taskLoadGradeDesignation = LoadGradeDesignation();
            var taskLoadEmpFamilySection = LoadEmpFamilySection();
            var taskLoadEmpEducation = LoadEmpEducation();
            var taskLoadEmpEMI = LoadEmpEMI();
            var taskLoadEmpSalary = LoadEmpSalary();
            var taskLoadLeaveEncashme = LoadLeaveEncashment();
            var taskLoadTotalPayrollDay = LoadTotalPayrollDay();
            var taskLoadLosOfDay = LoadLosOfDay();
            var taskLoadIncomeTax = LoadIncomeTax();
            var taskLoadSalaryGroupName = LoadSalaryGroupName();
            await taskLoadGradeDesignation;
            var taskLoadOTRate_OtHours = LoadOTRate_OtHours();
            await taskLoadCompany; await taskLoadEmpOfficialSection; await taskLoadEmpPersonalSection; await taskLoadBankDetails; await taskLoadEmpFamilySection;
            await taskLoadEmpEducation; await taskLoadEmpEMI; await taskLoadEmpSalary; await taskLoadLeaveEncashme; await taskLoadTotalPayrollDay;
            await taskLoadLosOfDay; await taskLoadIncomeTax; await taskLoadSalaryGroupName; await taskLoadOTRate_OtHours;
        }




        public clsSystemKeysfunction(Context context, int EmpID, int Monthyear, int SalaryGroupId, IConfiguration config, int sepId = 0)
        {
            _config = config;
            _context = context;
            _EmpID = EmpID;
            _SepId = sepId;
            _Monthyear = Monthyear;
            string year = _Monthyear.ToString().Substring(0, 4);
            string month = _Monthyear.ToString().Substring(4, 2);
            _ApplicableDate = Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1).AddDays(-1);
            _SalaryGroupId = SalaryGroupId;
            LoadAlldata();
        }


#region *****************Dynamic All Function ************************************
        public int fncCompanyId_sys()
        {
            return _CompanyId;
        }

        public string fncEMP_Code_sys()
        {
            return _EMP_Code;
        }
        public string fncEmpName_sys()
        {
            return _EmpName;
        }
        public string fncEmpContact_sys()
        {
            return _EmpContact;
        }
        public string fncEmpFatherHusbandName_sys()
        {
            return _EmpFatherHusbandName;
        }
        public string fncEmpJoiningDt_sys()
        {
            return _EmpJoiningDt;
        }
        //public string fncEmpLastDate_sys()
        //{
        //    return _EmpLastDate;
        //}
        public string fncGender_sys()
        {
            return _Gender;
        }
        public string fncEmpDOB_sys()
        {
            return _EmpDOB;
        }
        public string fncEmpStatus_sys()
        {
            return _EmpStatus;
        }

        public string fncCompanyName_sys()
        {
            return _CompanyName;
        }
        public string fncCompanyLogoPath_sys()
        {
            return _CompanyLogoPath;
        }
        public string fncEmpEmailId_sys()
        {
            return _EmpEmailId;
        }
        public string fncAdharNo_sys()
        {
            return _AdharNo;
        }
        public string fncAdharName_sys()
        {
            return _AdharName;
        }
        public string fncPanNo_sys()
        {
            return _PanNo;
        }
        public string fncPanName_sys()
        {
            return _PanName;
        }

        public string fncEsicNo_sys()
        {
            return _EsicNo;
        }
        public byte fncEsicAppliable_sys()
        {
            return _EsicApplicable;
        }
        public string fncEsicAppliableName_sys()
        {
            return _EsicApplicable == 1 ? "Yes" : "No";
        }
        public double fncEsicPercentage_sys()
        {
            return _EsicPercentage;
        }
        public double fncEmployerEsicPercentage_sys()
        {
            return _EmployerEsicPercentage;
        }

        public string fncUanNo_sys()
        {
            return _UanNo;
        }
        public byte fncPFApplicable_sys()
        {
            return _PFApplicable;
        }
        public string fncPFApplicableyesNo_sys()
        {
            return _PFApplicable == 1 ? "Yes" : "No";
        }
        public byte fncVPFApplicable_sys()
        {
            return _VpfApplicable;
        }
        public string fncVPFApplicableyesNo_sys()
        {
            return _VpfApplicable == 1 ? "Yes" : "No";

        }

        public byte fncEPSApplicable_sys()
        {
            return _EpsApplicable;
        }
        public string fncEPSApplicableyesNo_sys()
        {
            return _EpsApplicable == 1 ? "Yes" : "No";
        }
        public int fncVPFGroup_sys()
        {
            return _VpfGroupId;
        }
        public double fncVpfPercentage_sys()
        {
            return _VpfPercentage;
        }
        public string fncPfNo_sys()
        {
            return _PfNo;
        }
        public int fncPFGroupid_sys()
        {
            return _PfGroupId;
        }

        public double fncPFCeling_sys()
        {
            return _PFCeling;
        }
        public double fncPfSalarySlab_sys()
        {
            return _PfSalarySlab;
        }
        public double fncPfPercentage_sys()
        {
            return _PfPercentage;
        }
        public double fncEmployer_pf_percentage_sys()
        {
            return _Employer_pf_percentage;
        }
        public double fncEmployer_Pension_Scheme_percentage_sys()
        {
            return _Employer_Pension_Scheme_percentage;
        }
        public double fncEmployer_EDLIS_percetage_sys()
        {
            return _Employer_EDLIS_percetage;
        }
        public double fncEPF_Administration_charges_percentage_sys()
        {
            return _EPF_Administration_charges_percentage;
        }
        public double fncEDLIS_Administration_charges_percentage_sys()
        {
            return _EDLIS_Administration_charges_percentage;
        }

        public int fncSalaryGroupId_sys()
        {
            return _SalaryGroupId;
        }
        public string fncEmpSalaryGroupName_sys()
        {
            return _EmpSalaryGroupName;
        }
        public string fncEducationLevel_sys()
        {
            return _EducationLevel;
        }
        public string fncNationality_sys()
        {
            return _Nationality;
        }
        public string fncEMPGradeName_sys()
        {
            return _EMPGradeName;
        }
        public string fncEmpDesignation_sys()
        {
            return _EmpDesignation;
        }
        public string fncEmpDepartment_sys()
        {
            return _EmpDepartment;
        }
        public string fncEmpLocation_sys()
        {
            return _EmpLocation;
        }
        public string fncEmpWorkingState_sys()
        {
            return _EmpWorkingState;
        }
        public double fncLOD_sys()
        {
            return _LOD;
        }
        public double fncEMI_sys()
        {
            return _EMI;
        }
        public double fncSalary_sys()
        {
            return _Salary;
        }
        public double fncIncomeTax_sys()
        {
            return _IncomeTax;
        }
        public double fncOTRate_sys()
        {
            return _OTRate;
        }
        public double fncOTHour_sys()
        {
            return _OTHour;
        }
        public int fncno_of_child_sys()
        {
            return _no_of_child;
        }

        public string fncBankAccountNo_sys()
        {
            return _BankAccountNo;
        }
        public string fncIFSCCode_sys()
        {
            return _IFSCCode;
        }
        public string fncBankName_sys()
        {
            return _BankName;
        }

        public int fncToalPayrollDay_sys()
        {
            return _ToalPayrollDay;
        }

        public int fncELEncasment_sys()
        {
            return _EL_Encashment;
        }

        public int fncComp_off_Encasment_sys()
        {
            return _Comp_off_Encasment;
        }
        public int fncAddtitionalPaidDay_sys()
        {
            return _AdditionalPaidDays;
        }
        public int fncNoticeRecoveryDay_sys()
        {
            return _NoticeRecoveryDay;
        }
        public int fncNoticePaymentDay_sys()
        {
            return _NoticePaymentDay;
        }
#endregion


    }
}



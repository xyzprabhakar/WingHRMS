using projAPI.Model;
using projContext;
using projContext.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projAPI
{

//    public class PayslipGenerator
//    {


//        public class tree
//        {
//            public int comp_id { get; set; }
//            public string comp_name { get; set; }
//            public string comp_value { get; set; }

//            public bool is_diplay { get; set; }
//            public List<tree> ChildData { get; set; }
//        }
//        public List<tbl_component_master> compoData { get; set; }
//        public List<tbl_salary_input> salary_Inputs { get; set; }
//        CultureInfo hindi = new CultureInfo("hi-IN");

//        List<tree> setTree(int parentId)
//        {
//            List<tree> trees = (from t1 in compoData
//                                join t2 in salary_Inputs on t1.component_id equals t2.component_id
//                                where t1.parentid == parentId
//                                select new tree
//                                {
//                                    comp_id = t1.component_id,
//                                    comp_name = t1.component_name,
//                                    is_diplay = t1.is_payslip == 1 ? true : false,
//                                    comp_value = t2.values
//                                }).ToList();
//            foreach (var tr in trees)
//            {
//                tr.ChildData = setTree(tr.comp_id);
//            }
//            return trees;
//        }

//        StringBuilder GetHtml(List<tree> trees, int padding)
//        {
//            StringBuilder stringBuilder = new StringBuilder("");
//            double valuedata = 0;
//            foreach (var tr in trees)
//            {
//                valuedata = 0;
//                StringBuilder inner = new StringBuilder("");
//                if (tr.ChildData.Count > 0)
//                {
//                    if (tr.is_diplay == true)
//                    {
//                        Double.TryParse(tr.comp_value, out valuedata);
//                        if (valuedata > 0)
//                        {
//                            stringBuilder.Append(@"
//                                <div class='nti' style='margin-bottom: 10px;background: rgba(0, 0, 0, 0.04);width:100%;'>
//                                <div class='entry' style='padding: 0 #padding#px;margin: 6px 0;width:96%;'>
//                                <div class='label' style=' font-weight: 500;line-height: 16px;margin: 5px 0px;width:50%;float:left;'><b style='font-size: 10pt;'>
//                                #comp_name#</b></div> 
//                                <div class='amount' style='text-align: right;font-weight: 400;width: 90px;width:50%;float:left;'>#valuedata#</div>
//                                <div class='clear'></div></div>
//                                </div>");
//                            stringBuilder.Replace("#padding#", (padding + 10).ToString());
//                            stringBuilder.Replace("#comp_name#", tr.comp_name);
//                            stringBuilder.Replace("#valuedata#", string.Format(hindi, "{0:c}", valuedata));
//                            //, padding + 10, tr.comp_name, string.Format(hindi, "{0:c}", valuedata));

//                        }
//                    }
//                    stringBuilder.Append(GetHtml(tr.ChildData, tr.is_diplay == true ? (padding + 10) : padding));
//                }
//                else
//                {
//                    if (tr.is_diplay == true)
//                    {
//                        Double.TryParse(tr.comp_value, out valuedata);
//                        if (valuedata > 0)
//                        {
//                            stringBuilder.Append(@"
//                                <div class='entry' style='padding: 0 #padding#px;margin: 6px 0;width:96%;'>
//                                <div class='label' style=' font-weight: 500;line-height: 16px;margin: 5px 0px;width:50%;float:left;'>#comp_name#</div> 
//                                <div class='amount' style='text-align: right;font-weight: 400;width: 90px;width:50%;float:left;'>#valuedata#</div>
//                                <div class='clear'></div></div>");
//                            stringBuilder.Replace("#padding#", (padding + 10).ToString());
//                            stringBuilder.Replace("#comp_name#", tr.comp_name);
//                            stringBuilder.Replace("#valuedata#", string.Format(hindi, "{0:c}", valuedata));
//                            //,padding +20, tr.comp_name, string.Format(hindi, "{0:c}", valuedata));
//                            //"" + string.Format(hindi, "{0:c}", val) + "</div><div class='clear'></div></div>");
//                        }
//                    }
//                }

//            }

//            return stringBuilder;
//        }

//        public string GetHTMLString(int emp_id, int month_year, string logobasepath)
//        {
//            try
//            {
//                projContext.Context _context = new projContext.Context();
//                salary_Inputs = _context.tbl_salary_input.Where(p => p.emp_id == emp_id && p.monthyear == month_year && p.is_active == 1).ToList();
//                if (salary_Inputs.Count == 0)
//                {
//                    return "";
//                }
//                var salary_input_changes = _context.tbl_salary_input_change.Where(p => p.emp_id == emp_id && p.monthyear == month_year.ToString() && p.is_active == 1).ToList();
//                int index = -1;
//                foreach (var sc in salary_input_changes)
//                {
//                    index = salary_Inputs.FindIndex(p => p.component_id == sc.component_id);
//                    if (index >= 0)
//                    {
//                        salary_Inputs[index].values = sc.values;
//                    }
//                }

//                compoData = _context.tbl_component_master.Where(p => p.is_active == 1).ToList();
//                var treedata = setTree(0);
//                string htmlcompDetails = GetHtml(treedata, 10).ToString();

//                var sb = new StringBuilder();
//                //double Total_Earning = Convert.ToDouble(obj_incom_comp.Select(a => new { val = Convert.ToDouble(a.component_value) }).Sum(a => a.val));
//                CultureInfo hindi = new CultureInfo("hi-IN");
//                var templogo = salary_Inputs.FirstOrDefault(p => p.component_id == 988);
//                string company_logo = "";
//                if (templogo != null)
//                {
//                    string path = logobasepath + "/" + templogo.values;
//                    byte[] byt = System.IO.File.ReadAllBytes(path);
//                    company_logo = "data:image/png;base64," + Convert.ToBase64String(byt);
//                }

//                var tempcompnayname = salary_Inputs.FirstOrDefault(p => p.company_id == 989);
//                string company_name = " ";
//                if (tempcompnayname != null)
//                {
//                    company_name = tempcompnayname.values;
//                }

//                string payroll_date = DateTime.Now.ToString("dd-MM-yyyy");
//                if (month_year.ToString().Length == 6)
//                {
//                    string year = month_year.ToString().Substring(0, 4);
//                    string month = month_year.ToString().Substring(4, 2);
//                    payroll_date = Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1).AddDays(-1).ToString("dd-MM-yyyy");
//                }

//                var tempemp_name = salary_Inputs.FirstOrDefault(p => p.component_id == 997);
//                string emp_name = "-";
//                if (tempemp_name != null)
//                {
//                    emp_name = tempemp_name.values;
//                }
//                var tempemail_id = salary_Inputs.FirstOrDefault(p => p.component_id == 987);
//                string email_id = "-";
//                if (tempemail_id != null)
//                {
//                    email_id = tempemail_id.values;
//                }

//                var tempemployee_code = salary_Inputs.FirstOrDefault(p => p.component_id == 998);
//                string employee_code = "-";
//                if (tempemployee_code != null)
//                {
//                    employee_code = tempemployee_code.values;
//                }

//                var tempemp_grade = salary_Inputs.FirstOrDefault(p => p.component_id == 974);
//                string emp_grade = "-";
//                if (tempemp_grade != null)
//                {
//                    emp_grade = tempemp_grade.values;
//                }

//                var tempdateofjoining = salary_Inputs.FirstOrDefault(p => p.component_id == 995);
//                string dateofjoining = "-";
//                if (tempdateofjoining != null)
//                {
//                    dateofjoining = tempdateofjoining.values;
//                }
//                var tempdept_name = salary_Inputs.FirstOrDefault(p => p.component_id == 972); //salary_Inputs.FirstOrDefault(p => p.component_id == 995);
//                string dept_name = "-";
//                if (tempdept_name != null)
//                {
//                    dept_name = tempdept_name.values;
//                }
//                var tempdesignation_name = salary_Inputs.FirstOrDefault(p => p.component_id == 973); //salary_Inputs.FirstOrDefault(p => p.component_id == 995);
//                string designation_name = "-";
//                if (tempdesignation_name != null)
//                {
//                    designation_name = tempdesignation_name.values;
//                }
//                var tempdays_in_month = salary_Inputs.FirstOrDefault(p => p.component_id == 959);
//                string days_in_month = "30";
//                if (tempdays_in_month != null)
//                {
//                    days_in_month = tempdays_in_month.values;
//                }
//                var tempprsnt_count_in_mnth = salary_Inputs.FirstOrDefault(p => p.component_id == 957);
//                string prsnt_count_in_mnth = "0";
//                if (tempprsnt_count_in_mnth != null)
//                {
//                    prsnt_count_in_mnth = tempprsnt_count_in_mnth.values;
//                }
//                string absnt_count_in_mnth = string.Format("{0:.##}", (Convert.ToDouble(days_in_month) - Convert.ToDouble(prsnt_count_in_mnth)));

//                var tempemp_pan_number = salary_Inputs.FirstOrDefault(p => p.component_id == 984);
//                string emp_pan_number = "-";
//                if (tempemp_pan_number != null)
//                {
//                    emp_pan_number = tempemp_pan_number.values;
//                }

//                sb.Append(@"<style>
//                body {
//                    background: #f0f0f0;
//                    padding:0% 5%;
//                }");
//                sb.Append(@"@import url('https://fonts.googleapis.com/css?family=Roboto:200,300,400,600,700');");
//                sb.Append(@"{
//    font-family: 'Roboto', sans-serif;
//    font-size: 12px;
//    color: #444;
//}

//#payslip {
//    background: #fff;
//    padding: 30px 40px;
//    box-sizing: content-box;
//    width: 1024px;
//    margin: 0 auto;
//}

//#title {
//    margin-bottom: 20px;
//    font-size: 25px;
//    font-weight: 600;
//}

//#scope {
//    border-top: 1px solid #ccc;
//    border-bottom: 1px solid #ccc;
//    padding: 7px 0 4px 0;
//    display: flex;
//    justify-content: space-around;
//}

//    #scope > .scope-entry {
//        text-align: center;
//    }

//        #scope > .scope-entry > .value {
//            font-size: 14px;
//            font-weight: 700;
//        }

//.content {
//    display: flex;
//    border-bottom: 1px solid #ccc;
    
//}

//    .content .left-panel {
//        border-right: 1px solid #ccc;
//        min-width: 326px;
//        padding: 20px 16px 0 0;
//    }

//    .content .right-panel {
//        width: 100%;
//        padding: 10px 0 0 16px;
//    }

//#employee {
//    text-align: center;
//    margin-bottom: 20px;
//}

//    #employee #name {
//        font-size: 16px;
//        font-weight: 700;
//        margin: 3px 0px;
//    }

//    #employee #email {
//        font-size: 11px;
//        font-weight: 300;
//    }

//.details, .contributions, .ytd, .gross {
//    margin-bottom: 20px;
//}

//    .details .entry, .contributions .entry, .ytd .entry {
//        display: flex;
//        justify-content: space-between;
//        margin-bottom: 6px;
//    }

//        .details .entry .value, .contributions .entry .value, .ytd .entry .value {
//            font-weight: 700;
//            max-width: 130px;
//            text-align: right;
//        }

//    .gross .entry .value {
//        font-weight: 700;
//        text-align: right;
//        font-size: 16px;
//    }

//    .contributions .title, .ytd .title, .gross .title {
//        font-size: 15px;
//        font-weight: 700;
//        border-bottom: 1px solid #ccc;
//        padding-bottom: 4px;
//        margin-bottom: 6px;
//    }

//.content .right-panel .details {
//    width: 100%;
//}

//    .content .right-panel .details .entry {
//        display: flex;
//        padding: 0 10px;
//        margin: 6px 0;
//    }

//    .content .right-panel .details .label {
//        font-weight: 700;
//        line-height: 16px;
//        margin: 5px 0px;
//    }

//    .content .right-panel .details .detail {
//        font-weight: 600;
//        width: 130px;
//    }

//    .content .right-panel .details .rate {
//        font-weight: 400;
        
//        font-style: italic;
//        letter-spacing: 1px;
//    }

//    .content .right-panel .details .amount {
//        text-align: right;
//        font-weight: 700;
//        width: 90px;
//    }

//    .content .right-panel .details .net_pay div, .content .right-panel .details .nti div {
//        font-weight: 600;
//        font-size: 12px;
//    }

//    .content .right-panel .details .net_pay, .content .right-panel .details .nti {
//        padding: 3px 0 2px 0;
//        margin-bottom: 10px;
//        background: rgba(0, 0, 0, 0.04);
//    }
//.payslip .label {
//    margin: 5px 0px;
//}

//</style>
//<div>
//<div style='padding: 0% 0%;'>
//    <div id='payslip' style='background: #fff;box-sizing: content-box;margin: 0 auto;width: 95%;'>
//        <div style='width: 100%; margin-bottom: 6%;'>
//            <div style=' width: 0%; float: left;'>
//                <img src='#company_logo#' style='height: 100px;'>
//            </div>
//            <div>
//                <div id='title' style='text-align: center;font-size: 20px;font-weight: 600;text-transform:uppercase;padding-top: 4%;'> #company_name#</div>
//            </div>
//        </div>

//        <div id='scope' style='border-top: 1px solid #ccc;border-bottom: 1px solid #ccc;padding: 7px 0 4px 0;'>
//            <div class='scope-entry' style='text-align: center;width:100%;float:left;'>
//                <div class='title'>Payslip for the month</div>
//                <div class='value' style='font-size: 12px;font-weight: 700;'>#payroll_date#</div>
//            </div>
//            <div class='clear'></div>
//        </div>

//        <div class='content payslip' style='border-bottom: 1px solid #ccc;'>
//            <div class='left-panel' style=' border-right: 1px solid #ccc;width:46%;padding: 20px 16px 0 0;float:left;'>
//                <div id='employee' style='text-align: center;margin-bottom: 20px;border-top: 1px solid #ccc;border-left: 1px solid #ccc;border-right: 1px solid #ccc;margin: 0px;'>
//                    <div id='name' style=' font-size: 16px;font-weight: 700;margin: 3px 0px;'>
//                        #emp_name#
//                    </div>
//                    <div id='email' style='font-size: 14px;font-weight: 500;'>
//                        #email_id#
//                    </div>
//                </div>
//                <div class='details' style='margin-bottom: 20px;border: 1px solid #ccc; padding: 12px;'>
//                    <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;background-color: whitesmoke;'>
//                        <div class='label' style='margin: 5px 0px;width: 45%;float:left;padding-left: 7px;'>Employee Code</div>
//                        <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px;'>
//                        #employee_code#</div>
//                        <div class='clear'></div>
//                    </div>

//                    <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>Grade</div>
//                        <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>
//                        #emp_grade#</div>
//                        <div class='clear'></div>
//                    </div>

//                    <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;background-color: whitesmoke;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>Date of Joining</div>
//                        <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>
//                        #dateofjoining#</div>
//                        <div class='clear'></div>
//                    </div>

//                    <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>Department</div>
//                        <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>
//                        #dept_name#</div>
//                        <div class='clear'></div>
//                    </div>
//                    <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;background-color: whitesmoke;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>Desgination</div>
//                        <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>
//                        #designation_name#</div>
//                        <div class='clear'></div>
//                    </div>
//                    <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>Total days in Month</div>
//                        <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>
//                        #days_in_month#</div>
//                        <div class='clear'></div>
//                    </div>
//                    <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;background-color: whitesmoke;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>Present days</div>
//                        <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>
//                        #prsnt_count_in_mnth#</div>
//                        <div class='clear'></div>
//                    </div>
//                    <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>Leave without pay</div>
//                        <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>
//                        #absnt_count_in_mnth#</div>
//                        <div class='clear'></div>
//                    </div>
//                    <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;background-color: whitesmoke;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>PAN No</div>
//                        <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>
//                        #emp_pan_number#</div>
//                        <div class='clear'></div>
//                    </div>
                    
//                </div>
//            </div>
//            <div class='right-panel' style='width: 48%;float: left;padding: 20px 0 0 16px;'>
//                <div class='details' style=' width: 100%;'>
//                    #htmlcompDetails#
//                </div>
//            </div>
//        </div>
//    </div>
//    <div class='clear'></div>
//</div>
//<div class='clear'></div>
//<div>

//    <div id='title' style='text-align: center;font-size: 13px;font-weight: 400;'> This is system generated reportand does not require signature or stamp</div>
//</div>
//</div>");
//                //, company_logo, company_name, payroll_date, emp_name, email_id, employee_code, 
//                //emp_grade, dateofjoining, dept_name,
//                //    designation_name, days_in_month, prsnt_count_in_mnth, absnt_count_in_mnth, emp_pan_number, htmlcompDetails);

//                sb.Replace("#company_logo#", company_logo);
//                sb.Replace("#company_name#", company_name);
//                sb.Replace("#payroll_date#", payroll_date);
//                sb.Replace("#emp_name#", emp_name);
//                sb.Replace("#email_id#", email_id);
//                sb.Replace("#employee_code#", employee_code);
//                sb.Replace("#emp_grade#", emp_grade);
//                sb.Replace("#dateofjoining#", dateofjoining);
//                sb.Replace("#dept_name#", dept_name);
//                sb.Replace("#designation_name#", designation_name);
//                sb.Replace("#days_in_month#", days_in_month);
//                sb.Replace("#prsnt_count_in_mnth#", prsnt_count_in_mnth);
//                sb.Replace("#absnt_count_in_mnth#", absnt_count_in_mnth);
//                sb.Replace("#emp_pan_number#", emp_pan_number);
//                sb.Replace("#htmlcompDetails#", htmlcompDetails);
//                return sb.ToString();
//            }
//            catch (Exception EX)
//            {
//                return EX.Message;
//            }


//        }

//        public static string GetHTMLString(string company_name, string payroll_date, string emp_name, string email_id, string employee_code, string emp_grade, string dateofjoining, string dept_name, string designation_name, int days_in_month, double prsnt_count_in_mnth, double absnt_count_in_mnth, string emp_pan_number, string EmpMonthlyCTC, List<IncomeComponent> obj_incom_comp, string company_logo)
//        {

//            var sb = new StringBuilder();


//            double Total_Earning = Convert.ToDouble(obj_incom_comp.Select(a => new { val = Convert.ToDouble(a.component_value) }).Sum(a => a.val));
//            CultureInfo hindi = new CultureInfo("hi-IN");
//            if (EmpMonthlyCTC != null)
//            {
//                decimal parsed = decimal.Parse(EmpMonthlyCTC, CultureInfo.InvariantCulture);

//                EmpMonthlyCTC = string.Format(hindi, "{0:c}", parsed);
//            }
//            sb.Append(@"<div style='padding: 0% 0%;'>
//                      <div id = 'payslip' style='background: #fff;box-sizing: content-box;margin: 0 auto;width: 95%;'>
//                        <div style='width: 100%; margin-bottom: 6%;'>
//                            <div style=' width: 0%; float: left;'>
//                                <img src='" + company_logo + @"'  style='height: 100px;'>
//                            </div>
//                            <div>
//                                <div id='title' style='text-align: center;font-size: 20px;font-weight: 600;text-transform:uppercase;padding-top: 4%;'> " + company_name + @"</div>
//                            </div>
//                        </div>                       

//                       <div id = 'scope' style='border-top: 1px solid #ccc;border-bottom: 1px solid #ccc;padding: 7px 0 4px 0;'>
//                                        <div class='scope-entry' style='text-align: center;width:100%;float:left;'>
//                            <div class='title'>Payslip for the month</div>
//                            <div class='value' style='font-size: 12px;font-weight: 700;'>" + payroll_date + @"</div>
//                        </div>
//             <div class='clear'></div>
//                    </div>

//                    <div class='content payslip' style='border-bottom: 1px solid #ccc;'>
//                        <div class='left-panel' style=' border-right: 1px solid #ccc;width:46%;padding: 20px 16px 0 0;float:left;'>
//                            <div id = 'employee' style='text-align: center;margin-bottom: 20px;border-top: 1px solid #ccc;border-left: 1px solid #ccc;border-right: 1px solid #ccc;margin: 0px;'>
//                                <div id='name' style=' font-size: 16px;font-weight: 700;margin: 3px 0px;'>
//                                 " + emp_name + @"
//                                </div>
//                                <div id = 'email' style='font-size: 14px;font-weight: 500;'>
//                                   " + email_id + @"
//                                </div>
//                            </div>
//                            <div class='details' style='margin-bottom: 20px;border: 1px solid #ccc; padding: 12px;'>
//                                <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;background-color: whitesmoke;'>
//                        <div class='label' style='margin: 5px 0px;width: 45%;float:left;padding-left: 7px;'>Employee Code</div>
//                                    <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px;'>" + employee_code + @"</div>
//            <div class='clear'></div>
//                                </div>

//                                <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>Grade</div>
//                                     <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>" + emp_grade + @"</div>
//            <div class='clear'></div>
//                                </div>     

//                                    <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;background-color: whitesmoke;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>Date of Joining</div>
//                                     <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>" + dateofjoining + @"</div>
//            <div class='clear'></div>
//                                </div>

//                                <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>Department</div>
//                                     <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>" + dept_name + @"</div>
//            <div class='clear'></div>
//                                </div>
//                                <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;background-color: whitesmoke;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>Desgination</div>
//                                     <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>" + designation_name + @"</div>
//            <div class='clear'></div>
//                                </div>
//                                <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>Total days in Month</div>
//                                     <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>" + days_in_month + @"</div>
//            <div class='clear'></div>
//                                </div>
//                                <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;background-color: whitesmoke;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>Present days</div>
//                                     <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>" + prsnt_count_in_mnth + @"</div>
//            <div class='clear'></div>
//                                </div>
//                                <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>Leave without pay</div>
//                                     <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>" + absnt_count_in_mnth + @"</div>
//            <div class='clear'></div>
//                                </div>
//                                <div class='entry' style='margin-bottom: 6px;width:100%;border-bottom: 1px solid #ccc;background-color: whitesmoke;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'>PAN No</div>
//                                     <div class='value' style='float: left; width: 45%; height: 17px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'>" + emp_pan_number + @"</div>
//            <div class='clear'></div>
//                                </div>
//<div class='entry' style='width:100%;'>
//                        <div class='label' style='width: 45%;float:left;padding-left: 7px;'><b style='font-size: 10pt;'>Gross</b></div>
//                         <div class='value' style='float: left; width: 45%; height: 22px; border-left: 1px solid #ccc; padding-top: 4px; padding-left: 10px; margin-top: -6px;'><b style='font-size: 10pt;'>" + EmpMonthlyCTC + @"</b></div>
//                        <div class='clear'></div>
//                    </div>                               
//                            </div>
//                        </div>
//                        <div class='right-panel' style='width: 48%;float: left;padding: 20px 0 0 16px;'>
//                            <div class='details' style=' width: 100%;'>

//                                <div class='nti' style='margin-bottom: 10px;background: rgba(0, 0, 0, 0.04);width:100%;'>
//                                    <div class='entry' style='padding: 0 10px;margin: 6px 0;font-weight: 600;font-size: 10px;'>
//                                        <div class='label' style=' font-weight: 700;line-height: 40px;margin: 5px 0px;font-weight: 600;font-size: 14px;'>Earning / Deduction</div>
//                                        <div class='detail' style='font-weight: 600;width: 130px;font-weight: 600;font-size: 10px;'></div>
//                                        <div class='rate' style='font-weight: 400;font-style: italic;letter-spacing: 1px;font-weight: 600;font-size: 10px;'></div>
//                                        <div class='amount' style='text-align: right;font-weight: 700;width: 90px;font-weight: 600;font-size: 10px;'>"); Total_Earning.ToString(); sb.AppendFormat(@"</div>
//                                    </div>
//                                </div>
//                                <div class='non_taxable_allowance' style='width:100%;'> ");
//            foreach (var emp_inc in obj_incom_comp)
//            {
//                decimal val = decimal.Parse(emp_inc.component_value, CultureInfo.InvariantCulture);
//                sb.AppendFormat(@"<div class='entry' style='padding: 0 10px;margin: 6px 0;width:96%;'><div class='label' style=' font-weight: 500;line-height: 16px;margin: 5px 0px;width:50%;float:left;'>" + emp_inc.component_name + "</div> <div class='amount' style='text-align: right;font-weight: 400;width: 90px;width:50%;float:left;'>" + string.Format(hindi, "{0:c}", val) + "</div><div class='clear'></div></div>");

//            }
//            sb.AppendFormat(@"   </div> 
//                                </div>
//                            </div>
//                        </div>
//                    </div>
//             <div class='clear'></div>
//                </div>
//             <div class='clear'></div>
//                <div>
        
//            <div id='title' style='text-align: center;font-size: 13px;font-weight: 400;'> This is system generated reportand does not require signature or stamp</div>
//        </div>
//             </div>
//            </div>");


//            return sb.ToString();
//        }


//        //public string GetHTMLPayrollData(int emp_id, int month_year, string logobasepath)
//        //{
//        //    projContext.Context _context = new projContext.Context();
//        //    List < tbl_salary_input > tbl_salary_inputs = _context.tbl_salary_input.Where(p => p.emp_id == emp_id && p.monthyear == month_year && p.is_active == 1).ToList();
//        //    StringBuilder stringBuilder = new StringBuilder();
//        //    stringBuilder.Append("<table>");
//        //}

//        public string GetPayrollHTMLString_old(int emp_id, int month_year, string logobasepath)
//        {
//            try
//            {
//                projContext.Context _context = new projContext.Context();
//                salary_Inputs = _context.tbl_salary_input.Where(p => p.emp_id == emp_id && p.monthyear == month_year && p.is_active == 1).ToList();
//                if (salary_Inputs.Count == 0)
//                {
//                    return "";
//                }
//                var salary_input_changes = _context.tbl_salary_input_change.Where(p => p.emp_id == emp_id && p.monthyear == month_year.ToString() && p.is_active == 1).ToList();
//                int index = -1;
//                foreach (var sc in salary_input_changes)
//                {
//                    index = salary_Inputs.FindIndex(p => p.component_id == sc.component_id);
//                    if (index >= 0)
//                    {
//                        salary_Inputs[index].values = sc.values;
//                    }
//                }

//                compoData = _context.tbl_component_master.Where(p => p.is_active == 1).ToList();



//                var sb = new StringBuilder();
//                //double Total_Earning = Convert.ToDouble(obj_incom_comp.Select(a => new { val = Convert.ToDouble(a.component_value) }).Sum(a => a.val));
//                CultureInfo hindi = new CultureInfo("hi-IN");
//                var templogo = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.CompanyLogoPath);
//                string company_logo = "";
//                if (templogo != null)
//                {
//                    string path = logobasepath + "/" + templogo.values;
//                    byte[] byt = System.IO.File.ReadAllBytes(path);
//                    company_logo = "data:image/png;base64," + Convert.ToBase64String(byt);
//                }

//                var tempcompnayname = salary_Inputs.FirstOrDefault(p => p.company_id == (int)enmOtherComponent.CompanyName);
//                string company_name = " ";
//                if (tempcompnayname != null)
//                {
//                    company_name = tempcompnayname.values;
//                }

//                string company_address1 = "";
//                string company_address2 = "";
//                var tempcompnay_address = _context.tbl_company_master.Where(p => p.company_id == (int)enmOtherComponent.CompanyId).Select(
//                    p => new
//                    {
//                        address_line_one = p.address_line_one,
//                        address_line_two = p.address_line_two,
//                        state = p.tbl_state.name,
//                        country = p.tbl_country.name,
//                        city = p.tbl_city.name,
//                        pin_code = p.pin_code
//                    }
//                    ).FirstOrDefault();
//                if (tempcompnay_address != null)
//                {
//                    company_address1 = tempcompnay_address.address_line_one;
//                    company_address2 = string.Concat(tempcompnay_address.address_line_two, tempcompnay_address, " ", tempcompnay_address.state, tempcompnay_address.city, tempcompnay_address.country);
//                }

//                string payroll_date = DateTime.Now.ToString("dd-MM-yyyy");
//                if (month_year.ToString().Length == 6)
//                {
//                    string year = month_year.ToString().Substring(0, 4);
//                    string month = month_year.ToString().Substring(4, 2);
//                    payroll_date = Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1).AddDays(-1).ToString("dd-MM-yyyy");
//                }

//                var tempemp_name = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpName);
//                string emp_name = "-";
//                if (tempemp_name != null)
//                {
//                    emp_name = tempemp_name.values;
//                }
//                var tempemail_id = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpEmail);
//                string email_id = "-";
//                if (tempemail_id != null)
//                {
//                    email_id = tempemail_id.values;
//                }

//                var tempemployee_code = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpCode);
//                string employee_code = "-";
//                if (tempemployee_code != null)
//                {
//                    employee_code = tempemployee_code.values;
//                }

//                var tempemp_grade = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpGrade);
//                string emp_grade = "-";
//                if (tempemp_grade != null)
//                {
//                    emp_grade = tempemp_grade.values;
//                }


//                var tempdept_name = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpDepartment); //salary_Inputs.FirstOrDefault(p => p.component_id == 995);
//                string dept_name = "-";
//                if (tempdept_name != null)
//                {
//                    dept_name = tempdept_name.values;
//                }
//                var tempdesignation_name = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpDesignation); //salary_Inputs.FirstOrDefault(p => p.component_id == 995);
//                string designation_name = "-";
//                if (tempdesignation_name != null)
//                {
//                    designation_name = tempdesignation_name.values;
//                }

//                string emp_father_husband = "";
//                var emp_father_husband_name = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpFatherHusbandName);
//                if (emp_father_husband_name != null)
//                {
//                    emp_father_husband = emp_father_husband_name.values;
//                }

//                var tempdateofjoining = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpJoiningDt);
//                string dateofjoining = "-";
//                if (tempdateofjoining != null)
//                {
//                    dateofjoining = tempdateofjoining.values;
//                }


//                string emp_uan = "";
//                var tempemp_uan = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.Uan);
//                if (tempemp_uan != null)
//                {
//                    emp_uan = tempemp_uan.values;
//                }

//                string emp_pf_no = "";
//                var tempemp_pf_no = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.PfNo);
//                if (tempemp_pf_no != null)
//                {
//                    emp_pf_no = tempemp_pf_no.values;
//                }

//                string emp_esic_no = "";
//                var temp_emp_esic_no = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.ESICNo);
//                if (temp_emp_esic_no != null)
//                {
//                    emp_esic_no = temp_emp_esic_no.values;
//                }


//                var tempdays_in_month = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.ToalPayrollDay);
//                string days_in_month = "30";
//                if (tempdays_in_month != null)
//                {
//                    days_in_month = tempdays_in_month.values;
//                }
//                var tempprsnt_count_in_mnth = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.PaidDays);
//                string prsnt_count_in_mnth = "0";
//                if (tempprsnt_count_in_mnth != null)
//                {
//                    prsnt_count_in_mnth = tempprsnt_count_in_mnth.values;
//                }

//                string arrear_Day = "0";
//                var temp_arrear_day = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.ArrearDays);
//                if (temp_arrear_day != null)
//                {
//                    arrear_Day = temp_arrear_day.values;
//                }

//                string absnt_count_in_mnth = string.Format("{0:.##}", (Convert.ToDouble(days_in_month) - Convert.ToDouble(prsnt_count_in_mnth)));

//                var tempemp_pan_number = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.PanNo);
//                string emp_pan_number = "-";
//                if (tempemp_pan_number != null)
//                {
//                    emp_pan_number = tempemp_pan_number.values;
//                }



//                StringBuilder salary_component = new StringBuilder();
//                var component_mstr = _context.tbl_component_master;
//                var income_comp = component_mstr.Where(x => x.component_type == (int)enmComponentType.Income || x.component_type == (int)enmComponentType.OtherIncome && x.is_active == 1).OrderBy(p => p.component_id).ToList();
//                var deduction_comp = component_mstr.Where(x => x.component_type == (int)enmComponentType.Deduction || x.component_type == (int)enmComponentType.OtherDeduction && x.is_active == 1).OrderBy(p => p.component_id).ToList();

//                //remove the income compenen
//                for (int i = income_comp.Count - 1; i >= 0; i--)
//                {
//                    if (!(salary_Inputs.Any(p => p.component_id == income_comp[i].component_id && (p.current_month_value > 0 || p.arrear_value > 0))))
//                    {
//                        income_comp.RemoveAt(i);
//                    }
//                }
//                //remove the deduction compenent
//                for (int i = deduction_comp.Count - 1; i >= 0; i--)
//                {
//                    if (!(salary_Inputs.Any(p => p.component_id == deduction_comp[i].component_id && (p.current_month_value > 0 || p.arrear_value > 0))))
//                    {
//                        deduction_comp.RemoveAt(i);
//                    }
//                }

//                int maxloop = Math.Max(income_comp.Count, deduction_comp.Count);
//                int incomePayabale = 0, incomeArrear = 0, deductionPayabale = 0, deductionArrear = 0;
//                tbl_salary_input tsi = null;
//                for (int i = 0; i < maxloop; i++)
//                {
//                    incomePayabale = 0; incomeArrear = 0; deductionPayabale = 0; deductionArrear = 0; tsi = null;
//                    salary_component.AppendLine();
//                    salary_component.Append("<tr>");
//                    if (i < income_comp.Count)
//                    {
//                        tsi = salary_Inputs.FirstOrDefault(p => p.component_id == income_comp[i].component_id);
//                        if (tsi != null)
//                        {
//                            incomePayabale = Convert.ToInt32(tsi.current_month_value);
//                            incomeArrear = Convert.ToInt32(tsi.arrear_value);
//                        }
//                        salary_component.AppendFormat("<th colspan='2'> {0} </th><td>{1}</td><td class='myAlign'> {2} </td>", income_comp[i].property_details, incomePayabale, incomeArrear);
//                    }
//                    else
//                    {
//                        salary_component.Append("<th colspan='2'>  </th><td></td><td class='myAlign'>  </td>");
//                    }
//                    tsi = null;
//                    if (i < deduction_comp.Count)
//                    {
//                        tsi = salary_Inputs.FirstOrDefault(p => p.component_id == deduction_comp[i].component_id);
//                        if (tsi != null)
//                        {
//                            deductionPayabale = Convert.ToInt32(tsi.current_month_value);
//                            deductionArrear = Convert.ToInt32(tsi.arrear_value);
//                        }
//                        salary_component.AppendFormat("<th colspan='2'> {0} </th><td>{1}</td><td class='myAlign'> {2} </td>", deduction_comp[i].property_details, deductionPayabale, deductionArrear);
//                    }
//                    else
//                    {
//                        salary_component.Append("<th colspan='2'>  </th><td></td><td class='myAlign'>  </td>");
//                    }
//                }

//                double total_income = 0;
//                var temtotal_income = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.TotalGross);
//                if (temtotal_income != null)
//                {
//                    total_income = temtotal_income.current_month_value;
//                }

//                double total_deduction = 0, net = 0;
//                var temp_totaldeduc = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.Deduction);
//                if (temp_totaldeduc != null)
//                {
//                    total_deduction = temp_totaldeduc.current_month_value;
//                }
//                var temp_net = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.Net);
//                if (temp_net != null)
//                {
//                    net = temp_net.current_month_value;
//                }

//                net = total_income - total_deduction;


//                sb.Append(@"<style>
//.salary-slip .head{margin:10px;margin-bottom:50px;width:100%}.salary-slip .companyName{text-align:right;font-size:25px;font-weight:700}.salary-slip .salaryMonth{text-align:center}.salary-slip .table-border-bottom{border-bottom:1px solid #a3a4a5}.salary-slip .table-border-right{border-right:1px solid #a3a4a5}.salary-slip .myBackground{padding-top:10px;text-align:left;border:1px solid #a3a4a5;height:40px}.salary-slip .myAlign{text-align:center;border-right:1px solid #a3a4a5}.salary-slip .myTotalBackground{padding-top:10px;text-align:left;background-color:#ebf1de;border-spacing:0}.salary-slip .align-4{width:25%;float:left}.salary-slip .tail{margin-top:35px}.salary-slip .align-2{margin-top:25px;width:50%;float:left}.salary-slip .border-center{text-align:center}.salary-slip .border-center td,.salary-slip .border-center th{border:1px solid #a3a4a5}.salary-slip td,.salary-slip th{padding-left:6px}
//</style>");



//                // sb.Append(@"@import url('https://fonts.googleapis.com/css?family=Roboto:200,300,400,600,700');");
//                sb.Append(@"
//<div  class='salary-slip'>
//<table class='empDetail' style='max-width:1100px;margin:0 auto;font-size:13px;font-family:sans-serif;background:#fff;border-radius:3px;width:100%;text-align:left;border:1px solid #a3a4a5;border-collapse:collapse'>
//    <tr height='110px' style='background-color: #eaf5ff; '>
//      <td colspan='4'><img height='90px' src=#company_logo# /></td>
//      <td colspan='4' class='companyName' style='font-size: 13px; line-height: 18px; padding-right: 5px;'><strong style='font-size: 14px;'>#company_name#</strong><br>
//        #company_address1#<br>
//        #company_address2#<br>
//        #payroll_date#</td>
//    </tr>
//    <tr>
//      <th> Code </th>
//      <td>#employee_code# </td>
//      <td></td>
//      <th> PAN </th>
//      <td> #emp_pan_number# </td>
//      <td></td>
//      <th> Grade </th>
//      <td> #emp_grade# </td>
//    </tr>
//    <tr>
//      <th> Name </th>
//      <td> #emp_name# </td>
//      <td></td>
//      <th> Department </th>
//      <td> #dept_name# </td>
//      <td></td>
//      <th> Designaion </th>
//      <td> #designation_name# </td>
//    </tr>
//    <tr>
//      <th> Father/Husband Name </th>
//      <td> #emp_father_husband# </td>
//      <td></td>
//      <th> DOJ </th>
//      <td> #dateofjoining#</td>
//      <td></td>
//      <th> </th>
//      <td></td>
//    </tr>
//    <tr>
//      <th> UAN: </th>
//      <td> #emp_uan# </td>
//      <td></td>
//      <th> P.F. No </th>
//      <td> #emp_pf_no# </td>
//      <td></td>
//      <th> ESIC No </th>
//      <td> #emp_esic_no# </td>
//    </tr>
//    <tr class='myBackground'>
//      <th colspan='2'> Earning </th>
//      <th> Payable </th>
//      <th class='table-border-right'> Arrear Payable </th>
//      <th colspan='2'> Deductions</th>
//      <th> Amount </th>
//      <th> Arrear Amount </th>
//    </tr>
//    #salary_component#
//    <tr class='myBackground'>
//      <th colspan='3'> Total Payments </th>
//      <td class='myAlign'> #total_income# </td>
//      <th colspan='3'> Total Deductions </th>
//      <td class='myAlign'> #total_deduction# </td>
//    </tr>
//    <tbody class='border-center'>
//      <tr>
//        <th> Payroll Days </th>
//        <th colspan='2'> Paid Days </th>
//        <th> Arrear Days </th>
//        <th colspan='4'> </th>
//      </tr>
//      <tr>
//        <th>#days_in_month#</th>
//        <td colspan='2'>#prsnt_count_in_mnth#</td>
//        <td>#arrear_Day#</td>
//        <td colspan='4'></td>
//      </tr>
//      <tr>
//        <td colspan='4'>&nbsp;</td>
//        <td colspan='4'></td>
//      </tr>
//      <tr>
//        <td colspan='2'>Net</td>
//        <td colspan='6'>#net#</td>
//      </tr>
//    </tbody>
//  </table>
//</div>");
//                //, company_logo, company_name, payroll_date, emp_name, email_id, employee_code, 
//                //emp_grade, dateofjoining, dept_name,
//                //    designation_name, days_in_month, prsnt_count_in_mnth, absnt_count_in_mnth, emp_pan_number, htmlcompDetails);

//                sb.Replace("#company_logo#", company_logo);
//                sb.Replace("#company_name#", company_name);
//                sb.Replace("#company_address1", company_address1);
//                sb.Replace("#company_address2#", company_address2);
//                sb.Replace("#company_name#", company_name);
//                sb.Replace("#payroll_date#", payroll_date);
//                sb.Replace("#employee_code#", employee_code);
//                sb.Replace("#emp_pan_number#", emp_pan_number);
//                sb.Replace("#emp_grade#", emp_grade);
//                sb.Replace("#emp_name#", emp_name);
//                sb.Replace("#dept_name#", dept_name);
//                sb.Replace("#designation_name#", designation_name);
//                sb.Replace("#emp_father_husband#", emp_father_husband);
//                sb.Replace("#dateofjoining#", dateofjoining);
//                sb.Replace("#emp_uan#", emp_uan);
//                sb.Replace("#emp_pf_no#", emp_pf_no);
//                sb.Replace("#emp_esic_no#", emp_esic_no);
//                sb.Replace("#salary_component#", salary_component.ToString());
//                sb.Replace("#total_income#", total_income.ToString());
//                sb.Replace("#total_deduction#", total_deduction.ToString());
//                sb.Replace("#days_in_month#", days_in_month);
//                sb.Replace("#prsnt_count_in_mnth#", prsnt_count_in_mnth.ToString());
//                sb.Replace("#arrear_Day#", arrear_Day);
//                sb.Replace("#net#", net.ToString());
//                //sb.Replace("#email_id#", email_id);





//                //sb.Replace("#days_in_month#", days_in_month);
//                //sb.Replace("#prsnt_count_in_mnth#", prsnt_count_in_mnth);
//                //sb.Replace("#absnt_count_in_mnth#", absnt_count_in_mnth);

//                //sb.Replace("#htmlcompDetails#", htmlcompDetails);
//                return sb.ToString();
//            }
//            catch (Exception EX)
//            {
//                return EX.Message;
//            }


//        }

//        public string convertrupees(string fare)
//        {
//            if (fare == "")
//            { fare = "0"; }
//            decimal parsed = decimal.Parse(fare, CultureInfo.InvariantCulture);
//            CultureInfo hindi = new CultureInfo("hi-IN");
//            string text = string.Format(hindi, "{0:c}", parsed);
//            return text;
//        }
//        public string NumbersToWords(int inputNumber)
//        {
//            int inputNo = inputNumber;

//            if (inputNo == 0)
//                return "Zero";

//            int[] numbers = new int[4];
//            int first = 0;
//            int u, h, t;
//            System.Text.StringBuilder sb = new System.Text.StringBuilder();

//            if (inputNo < 0)
//            {
//                sb.Append("Minus ");
//                inputNo = -inputNo;
//            }

//            string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
//            "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
//            string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
//            "Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};
//            string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
//            "Seventy ","Eighty ", "Ninety "};
//            string[] words3 = { "Thousand ", "Lakh ", "Crore " };

//            numbers[0] = inputNo % 1000; // units
//            numbers[1] = inputNo / 1000;
//            numbers[2] = inputNo / 100000;
//            numbers[1] = numbers[1] - 100 * numbers[2]; // thousands
//            numbers[3] = inputNo / 10000000; // crores
//            numbers[2] = numbers[2] - 100 * numbers[3]; // lakhs

//            for (int i = 3; i > 0; i--)
//            {
//                if (numbers[i] != 0)
//                {
//                    first = i;
//                    break;
//                }
//            }
//            for (int i = first; i >= 0; i--)
//            {
//                if (numbers[i] == 0) continue;
//                u = numbers[i] % 10; // ones
//                t = numbers[i] / 10;
//                h = numbers[i] / 100; // hundreds
//                t = t - 10 * h; // tens
//                if (h > 0) sb.Append(words0[h] + "Hundred ");
//                if (u > 0 || t > 0)
//                {
//                    if (h > 0 || i == 0) sb.Append("");
//                    if (t == 0)
//                        sb.Append(words0[u]);
//                    else if (t == 1)
//                        sb.Append(words1[u]);
//                    else
//                        sb.Append(words2[t - 2] + words0[u]);
//                }
//                if (i != 0) sb.Append(words3[i - 1]);
//            }
//            return sb.ToString().TrimEnd() + " Only";
//        }
//        public string GetPayrollHTMLString(int emp_id, int month_year, string logobasepath)
//        {
//            try
//            {
//                projContext.Context _context = new projContext.Context();
//                salary_Inputs = _context.tbl_salary_input.Where(p => p.emp_id == emp_id && p.monthyear == month_year && p.is_active == 1).ToList();
//                if (salary_Inputs.Count == 0)
//                {
//                    return "";
//                }
//                var salary_input_changes = _context.tbl_salary_input_change.Where(p => p.emp_id == emp_id && p.monthyear == month_year.ToString() && p.is_active == 1).ToList();
//                int index = -1;
//                foreach (var sc in salary_input_changes)
//                {
//                    index = salary_Inputs.FindIndex(p => p.component_id == sc.component_id);
//                    if (index >= 0)
//                    {
//                        salary_Inputs[index].values = sc.values;
//                    }
//                }

//                compoData = _context.tbl_component_master.Where(p => p.is_active == 1).ToList();

//                var sb = new StringBuilder();
//                //double Total_Earning = Convert.ToDouble(obj_incom_comp.Select(a => new { val = Convert.ToDouble(a.component_value) }).Sum(a => a.val));
//                CultureInfo hindi = new CultureInfo("hi-IN");
//                var templogo = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.CompanyLogoPath);
//                string company_logo = "";
//                if (templogo != null)
//                {
//                    string path = logobasepath + "/" + templogo.values;
//                    byte[] byt = System.IO.File.ReadAllBytes(path);
//                    company_logo = "data:image/png;base64," + Convert.ToBase64String(byt);
//                }

//                var tempcompnayname = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.CompanyName);
//                string company_name = " ";
//                if (tempcompnayname != null)
//                {
//                    company_name = tempcompnayname.values;
//                }

//                string company_address1 = "";
//                string company_address2 = "";
//                //var tempcompnay_address = _context.tbl_company_master.Where(p => p.company_id == (int)enmOtherComponent.CompanyId).Select(
//                //    p => new
//                //    {
//                //        address_line_one = p.address_line_one,
//                //        address_line_two = p.address_line_two,
//                //        state = p.tbl_state.name,
//                //        country = p.tbl_country.name,
//                //        city = p.tbl_city.name,
//                //        pin_code = p.pin_code
//                //    }
//                //    ).FirstOrDefault();
//                var company_idget = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.CompanyId);
//                int company_idd = 0;
//                if (company_idget != null)
//                {
//                    company_idd = Convert.ToInt32(company_idget.values);
//                }

//                var tempcompnay_address = _context.tbl_company_master.Where(p => p.company_id == company_idd).Select(
//                    p => new
//                    {
//                        address_line_one = p.address_line_one,
//                        address_line_two = p.address_line_two,
//                        state = p.tbl_state.name,
//                        country = p.tbl_country.name,
//                        city = p.tbl_city.name,
//                        pin_code = p.pin_code
//                    }
//                    ).FirstOrDefault();
//                if (tempcompnay_address != null)
//                {
//                    company_address1 = tempcompnay_address.address_line_one;
//                    company_address2 = string.Concat(tempcompnay_address.address_line_two, " ", tempcompnay_address.state, tempcompnay_address.city, tempcompnay_address.country);
//                }

//                string payroll_date = DateTime.Now.ToString("dd-MM-yyyy");
//                if (month_year.ToString().Length == 6)
//                {
//                    string year = month_year.ToString().Substring(0, 4);
//                    string month = month_year.ToString().Substring(4, 2);
//                    payroll_date = Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1).AddDays(-1).ToString("dd-MM-yyyy");
//                }

//                var tempemp_name = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpName);
//                string emp_name = "-";
//                if (tempemp_name != null)
//                {
//                    emp_name = tempemp_name.values;
//                }
//                var tempemail_id = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpEmail);
//                string email_id = "-";
//                if (tempemail_id != null)
//                {
//                    email_id = tempemail_id.values;
//                }

//                var tempemail_adhno = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.AdharNo);
//                string adhaarno = "-";
//                if (tempemail_adhno != null)
//                {
//                    adhaarno = tempemail_adhno.values;
//                }

//                var tempemployee_code = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpCode);
//                string employee_code = "-";
//                if (tempemployee_code != null)
//                {
//                    employee_code = tempemployee_code.values;
//                }

//                var tempemp_grade = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpGrade);
//                string emp_grade = "-";
//                if (tempemp_grade != null)
//                {
//                    emp_grade = tempemp_grade.values;
//                }


//                var tempdept_name = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpDepartment); //salary_Inputs.FirstOrDefault(p => p.component_id == 995);
//                string dept_name = "-";
//                if (tempdept_name != null)
//                {
//                    dept_name = tempdept_name.values;
//                }
//                var tempdesignation_name = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpDesignation); //salary_Inputs.FirstOrDefault(p => p.component_id == 995);
//                string designation_name = "-";
//                if (tempdesignation_name != null)
//                {
//                    designation_name = tempdesignation_name.values;
//                }

//                string emp_father_husband = "";
//                var emp_father_husband_name = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpFatherHusbandName);
//                if (emp_father_husband_name != null)
//                {
//                    emp_father_husband = emp_father_husband_name.values;
//                }

//                var tempdateofjoining = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpJoiningDt);
//                string dateofjoining = "-";
//                if (tempdateofjoining != null)
//                {
//                    dateofjoining = tempdateofjoining.values;
//                }


//                string emp_uan = "";
//                var tempemp_uan = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.Uan);
//                if (tempemp_uan != null)
//                {
//                    emp_uan = tempemp_uan.values;
//                }

//                string emp_pf_no = "";
//                var tempemp_pf_no = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.PfNo);
//                if (tempemp_pf_no != null)
//                {
//                    emp_pf_no = tempemp_pf_no.values;
//                }

//                string emp_esic_no = "";
//                var temp_emp_esic_no = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.ESICNo);
//                if (temp_emp_esic_no != null)
//                {
//                    emp_esic_no = temp_emp_esic_no.values;
//                }


//                var tempdays_in_month = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.ToalPayrollDay);
//                string days_in_month = "30";
//                if (tempdays_in_month != null)
//                {
//                    days_in_month = tempdays_in_month.values;
//                }
//                var tempprsnt_count_in_mnth = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.PaidDays);
//                string prsnt_count_in_mnth = "0";
//                if (tempprsnt_count_in_mnth != null)
//                {
//                    prsnt_count_in_mnth = tempprsnt_count_in_mnth.values;
//                }

//                string arrear_Day = "0";
//                var temp_arrear_day = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.ArrearDays);
//                if (temp_arrear_day != null)
//                {
//                    arrear_Day = temp_arrear_day.values;
//                }

//                string absnt_count_in_mnth = string.Format("{0:.##}", (Convert.ToDouble(days_in_month) - Convert.ToDouble(prsnt_count_in_mnth)));

//                var tempemp_pan_number = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.PanNo);
//                string emp_pan_number = "-";
//                if (tempemp_pan_number != null)
//                {
//                    emp_pan_number = tempemp_pan_number.values;
//                }



//                StringBuilder salary_component = new StringBuilder();
//                var component_mstr = _context.tbl_component_master;
//                var income_comp = component_mstr.Where(x => x.component_type == (int)enmComponentType.Income || x.component_type == (int)enmComponentType.OtherIncome && x.is_active == 1).OrderBy(p => p.component_id).ToList();
//                var deduction_comp = component_mstr.Where(x => x.component_type == (int)enmComponentType.Deduction || x.component_type == (int)enmComponentType.OtherDeduction && x.is_active == 1).OrderBy(p => p.component_id).ToList();

//                var total_gross = 0;
//                var total_arrear_earning = 0;
//                var total_arrear_deduction = 0;
//                //remove the income compenen
//                for (int i = income_comp.Count - 1; i >= 0; i--)
//                {
//                    if (!(salary_Inputs.Any(p => p.component_id == income_comp[i].component_id && (p.current_month_value > 0 || p.arrear_value > 0))))
//                    {
//                        income_comp.RemoveAt(i);
//                    }
//                }
//                //remove the deduction compenent
//                for (int i = deduction_comp.Count - 1; i >= 0; i--)
//                {
//                    if (!(salary_Inputs.Any(p => p.component_id == deduction_comp[i].component_id && (p.current_month_value > 0 || p.arrear_value > 0))))
//                    {
//                        deduction_comp.RemoveAt(i);
//                    }
//                }

//                int maxloop = Math.Max(income_comp.Count, deduction_comp.Count);
//                int incomePayabale = 0, incomeArrear = 0, deductionPayabale = 0, deductionArrear = 0;
//                tbl_salary_input tsi = null;
//                for (int i = 0; i < maxloop; i++)
//                {
//                    incomePayabale = 0; incomeArrear = 0; deductionPayabale = 0; deductionArrear = 0; tsi = null;
//                    salary_component.AppendLine();
//                    salary_component.Append("<tr>");
//                    if (i < income_comp.Count)
//                    {
//                        tsi = salary_Inputs.FirstOrDefault(p => p.component_id == income_comp[i].component_id);
//                        if (tsi != null)
//                        {
//                            incomePayabale = Convert.ToInt32(tsi.current_month_value);
//                            total_gross += incomePayabale;
//                            incomeArrear = Convert.ToInt32(tsi.arrear_value);
//                            total_arrear_earning += incomeArrear;
//                        }
//                        salary_component.AppendFormat("<th colspan='2'> {0} </th><td>{1}</td><td class='myAlign'> {2} </td>", income_comp[i].property_details, incomePayabale, incomeArrear);
//                    }
//                    else
//                    {
//                        salary_component.Append("<th colspan='2'>  </th><td></td><td class='myAlign'>  </td>");
//                    }
//                    tsi = null;
//                    if (i < deduction_comp.Count)
//                    {
//                        if (deduction_comp[i].property_details.ToUpper() != "ER ESIC")
//                        {
//                            tsi = salary_Inputs.FirstOrDefault(p => p.component_id == deduction_comp[i].component_id);
//                            if (tsi != null)
//                            {
//                                deductionPayabale = Convert.ToInt32(tsi.current_month_value);
//                                deductionArrear = Convert.ToInt32(tsi.arrear_value);
//                                total_arrear_deduction += deductionArrear;
//                            }
//                            salary_component.AppendFormat("<th colspan='2'> {0} </th><td>{1}</td><td class='myAlign'> {2} </td>", deduction_comp[i].property_details, deductionPayabale, deductionArrear);
//                        }
//                    }
//                    else
//                    {
//                        salary_component.Append("<th colspan='2'>  </th><td></td><td class='myAlign'>  </td>");
//                    }
//                }
//                string LOP_Days = "0";
//                var temLOP_Days = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.LopDays);
//                if (temLOP_Days != null)
//                {
//                    LOP_Days = temLOP_Days.values;
//                }
//                double total_income = 0;
//                var temtotal_income = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.TotalGross);
//                if (temtotal_income != null)
//                {
//                    total_income = temtotal_income.current_month_value;
//                }

//                double total_deduction = 0, net = 0;
//                var temp_totaldeduc = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.Deduction);
//                if (temp_totaldeduc != null)
//                {
//                    total_deduction = temp_totaldeduc.current_month_value;
//                }
//                var temp_net = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.Net);
//                if (temp_net != null)
//                {
//                    net = temp_net.current_month_value;
//                }

//                net = total_income - total_deduction;

//                var rupeesnet = convertrupees(net.ToString());
//                var rupees_in_words = NumbersToWords(Convert.ToInt32(net));


//                string GrossSalary = "";
//                var temp_GrossSalary = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.GrossSalary);
//                if (temp_GrossSalary != null)
//                {
//                    GrossSalary = temp_GrossSalary.values;
//                }

//                string arreardays = "";
//                var temp_arreardays = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.ArrearDays);
//                if (temp_arreardays != null)
//                {
//                    arreardays = temp_arreardays.values;
//                }

//                sb.Append(@"<style>
//.salary-slip .head{margin:10px;margin-bottom:50px;width:100%}.salary-slip .companyName{text-align:right;font-size:25px;font-weight:700}.salary-slip .salaryMonth{text-align:center}.salary-slip .table-border-bottom{border-bottom:1px solid #a3a4a5}.salary-slip .table-border-right{border-right:1px solid #a3a4a5}.salary-slip .myBackground{padding-top:10px;text-align:left;border:1px solid #a3a4a5;height:40px}.salary-slip .myAlign{text-align:center;border-right:1px solid #a3a4a5}.salary-slip .myTotalBackground{padding-top:10px;text-align:left;background-color:#ebf1de;border-spacing:0}.salary-slip .align-4{width:25%;float:left}.salary-slip .tail{margin-top:35px}.salary-slip .align-2{margin-top:25px;width:50%;float:left}.salary-slip .border-center{text-align:center}.salary-slip .border-center td,.salary-slip .border-center th{border:1px solid #a3a4a5}.salary-slip td,.salary-slip th{padding-left:6px}
//</style>");



//                // sb.Append(@"@import url('https://fonts.googleapis.com/css?family=Roboto:200,300,400,600,700');");
//                sb.Append(@"
//<div  class='salary-slip'>
//<table class='empDetail' style='max-width:1100px;margin:0 auto;font-size:13px;font-family:sans-serif;background:#fff;border-radius:3px;width:100%;text-align:left;border:1px solid #a3a4a5;border-collapse:collapse'>
//            <tr height='110px' style='background-color: #eaf5ff; '>
//                <td><img height='90px' src=#company_logo# /></td>
//                <td colspan='6' class='companyName' style='font-size: 13px; text-align:center; line-height: 18px; padding-right: 5px;'>
//                    <strong style='font-size: 18px;'>#company_name#</strong><br>
//                    <span style='font-size: 13px;'>#company_address1#</span><br>
//                    <span style='font-size: 13px;'>#company_address2#</span><br>
//                </td>
//                <td colspan='1' class='myAlign'></td>
//            </tr>
//            <tr>
//                <td colspan='8' style='text-align: center; padding-bottom: 7px;font-size: 14px;border-bottom: 1px solid #a3a4a5;'>
//                    Payslip for the #payroll_date#
//                </td>
//            </tr>
//            <tr>
//                <th colspan='2'>Employee Code </th>
//                <td>#employee_code# </td>
//                <td></td>
//                <th colspan='3'>Employee Name </th>
//                <td style='text-align:left;padding-top: 7px;'  class='myAlign'> #emp_name# </td>
                
//            </tr>
//            <tr>
//                <th colspan='2'>Branch </th>
//                <td>#employee_code# </td>
//                <td></td>
//                <th colspan='3'> Work Location </th>
//                <td style='text-align:left' class='myAlign'> #emp_pan_number# </td>
                 
//            </tr>
//            <tr>

//                <th colspan='2'> Department </th>
//                <td> #dept_name# </td>
//                <td></td>
//                <th colspan='3'> Designaion </th>
//                <td  style='text-align:left' class='myAlign'> #designation_name# </td>
                 
//            </tr>
//            <tr>
//                <th colspan='2'> Grade </th>
//                <td> #emp_grade# </td>
//                <td></td>
//                <th colspan='3'> PAN No.</th>
//                <td  style='text-align:left' class='myAlign'> #emp_pan_number# </td>
                 
//            </tr>
//            <tr>
//                <th colspan='2'> Date Of Joining </th>
//                <td> #dateofjoining#</td>
//                <td></td>
//                <th colspan='3'>Aadhar No. </th>
//                <td  style='text-align:left' class='myAlign'> #aadhar_no# </td>
                 
//            </tr>
//            <tr>
//                <th colspan='2'> PF Account No. </th>
//                <td> #emp_pf_no# </td>
//                <td></td>
//                <th colspan='3'> ESIC Number </th>
//                <td  style='text-align:left' class='myAlign'> #emp_esic_no# </td>
                 
//            </tr>
//            <tr>
//                <th colspan='2'> UAN Number </th>
//                <td> #emp_uan# </td>
//                <td></td>
//                <th colspan='3'>Total Days in Month</th>
//                <td style='text-align:left' class='myAlign'> #days_in_month# </td>
                 
//            </tr>
           
//            <tr>
//                <th colspan='2'>Days Paid</th>
//                <td> #prsnt_count_in_mnth# </td>
//                <td></td>
//                <th colspan='3'>LWP Days</th>
//                <td  style='text-align:left;padding-bottom: 7px;' class='myAlign'>#lwp_days#</td>
                 
//            </tr>
//            <tr>
//                <th colspan='2'>Gross Salary</th>
//                <td> #gross_salary# </td>
//                <td></td>
//                <th colspan='3'>Arrear Days</th>
//                <td  style='text-align:left;padding-bottom: 7px;' class='myAlign'>#arrear_days#</td>
                 
//            </tr>

//            <tr class='myBackground'>
//                <th colspan='2'> Earnings </th>
//                <th> Amount </th>
//                <th class='myAlign'> Arrear/Adj </th>
//                <th colspan='2'> Deductions</th>
//                <th> Amount </th>
//                <th class='myAlign'> Arrear/Adj </th>
//            </tr>
//            #salary_component#
//            <tr class='myBackground'>
//                <td colspan='2' style='font-weight:bold'>Gross Earning</td>
//                <td> #gross_earning# </td>
//                <td class='myAlign'>#arrear_Day#</td>
//                <td colspan='2' style='font-weight:bold'>Gross Deduction</td>
//                <td> #total_deduction# </td>
//                <td class='myAlign'>#arrear_deduction#</td>
//            </tr>
//            <tr class='myBackground'>
//                <td colspan='2' style='font-weight:bold'>Net Earning</td>
//                <td> #net# </td>
//                <td></td>
//                <td> </td>
//                <td></td>
//                <td colspan='2' class='myAlign'></td>

//            </tr>
//            <tr class='myBackground'>
//                <td colspan='8' style='padding-top: 12px;padding-bottom: 15px;line-height: 20px;'>
//                    <div>
//                        <span style='font-weight:bold'>Net Earning:</span>
//                        <span> #net1# /-</span>
//                    </div>
//                    <div>
//                        <span style='font-weight:bold'>Net Earning in words : </span>
//                        <span>Rupees #netrupees#</span>
//                    </div>
//                </td>
//            </tr>
//            <tr class='myBackground' style='text-align:center'>
//                <td colspan='8'>This is a system generated report and does not require signature or stamp</td>
//            </tr>
//        </table>
//</div>");
//                //, company_logo, company_name, payroll_date, emp_name, email_id, employee_code, 
//                //emp_grade, dateofjoining, dept_name,
//                //    designation_name, days_in_month, prsnt_count_in_mnth, absnt_count_in_mnth, emp_pan_number, htmlcompDetails);

//                sb.Replace("#company_logo#", company_logo);
//                sb.Replace("#company_name#", company_name.Replace(" - C00001", ""));
//                sb.Replace("#company_address1", company_address1);
//                sb.Replace("#company_address2#", company_address2);
//                sb.Replace("#payroll_date#", payroll_date);
//                sb.Replace("#employee_code#", employee_code);
//                sb.Replace("#emp_pan_number#", emp_pan_number);
//                sb.Replace("#emp_grade#", emp_grade);
//                sb.Replace("#emp_name#", emp_name);
//                sb.Replace("#dept_name#", dept_name);
//                sb.Replace("#designation_name#", designation_name);
//                sb.Replace("#emp_father_husband#", emp_father_husband);
//                sb.Replace("#dateofjoining#", dateofjoining);
//                sb.Replace("#emp_uan#", emp_uan);
//                sb.Replace("#emp_pf_no#", emp_pf_no);
//                sb.Replace("#emp_esic_no#", emp_esic_no);
//                sb.Replace("#salary_component#", salary_component.ToString());
//                sb.Replace("#total_income#", total_income.ToString());
//                sb.Replace("#total_deduction#", total_deduction.ToString());
//                sb.Replace("#days_in_month#", days_in_month);
//                sb.Replace("#prsnt_count_in_mnth#", prsnt_count_in_mnth.ToString());
//                sb.Replace("#arrear_Day#", total_arrear_earning.ToString());// arrear_Day);
//                sb.Replace("#gross_earning#", total_gross.ToString());
//                sb.Replace("#net#", net.ToString());
//                sb.Replace("#net1#", rupeesnet.ToString());
//                sb.Replace("#netrupees#", rupees_in_words.ToString());
//                sb.Replace("#aadhar_no#", adhaarno.ToString());
//                sb.Replace("#arrear_deduction#", total_arrear_deduction.ToString());
//                sb.Replace("#lwp_days#", LOP_Days);
//                sb.Replace("#gross_salary#", GrossSalary);
//                sb.Replace("#arrear_days#", arreardays);

//                //sb.Replace("#email_id#", email_id);





//                //sb.Replace("#days_in_month#", days_in_month);
//                //sb.Replace("#prsnt_count_in_mnth#", prsnt_count_in_mnth);
//                //sb.Replace("#absnt_count_in_mnth#", absnt_count_in_mnth);

//                //sb.Replace("#htmlcompDetails#", htmlcompDetails);
//                return sb.ToString();
//            }
//            catch (Exception EX)
//            {
//                return EX.Message;
//            }


//        }

//        public string GetFNFHTMLString(int emp_id, int month_year, string logobasepath)
//        {
//            try
//            {
//                projContext.Context _context = new projContext.Context();

//                salary_Inputs = _context.tbl_salary_input.Where(p => p.is_fnf_comp==1 && p.emp_id == emp_id && p.monthyear == month_year && p.is_active == 1).ToList();
//                if (salary_Inputs.Count == 0)
//                {
//                    return "";
//                }

//                var fnf_mstr = _context.tbl_fnf_master.Where(x => x.emp_id == emp_id && x.is_deleted == 0).OrderByDescending(x=>x.pkid_fnfMaster).FirstOrDefault();
//                var sp = _context.tbl_emp_separation.Where(x => x.sepration_id == fnf_mstr.fkid_empSepration && x.is_cancel == 0 && x.is_deleted == 0).FirstOrDefault();
//                var fnf_attn = _context.tbl_lossofpay_master.Where(x => x.fkid_sepration == sp.sepration_id && x.is_active == 1).FirstOrDefault();
//                var fnf_leave = _context.tbl_fnf_leave_encash.Where(x => x.fnf_id == fnf_mstr.pkid_fnfMaster && x.is_deleted == 0)
//                    .Select(x => x.leave_encash_day).Sum();

//                var salary_input_changes = _context.tbl_salary_input_change.Where(p => p.emp_id == emp_id && p.monthyear == month_year.ToString() && p.is_active == 1).ToList();
//                int index = -1;
//                foreach (var sc in salary_input_changes)
//                {
//                    index = salary_Inputs.FindIndex(p => p.component_id == sc.component_id);
//                    if (index >= 0)
//                    {
//                        salary_Inputs[index].values = sc.values;
//                    }
//                }

//                compoData = _context.tbl_component_master.Where(p => p.is_active == 1).ToList();

//                var sb = new StringBuilder();
//                //double Total_Earning = Convert.ToDouble(obj_incom_comp.Select(a => new { val = Convert.ToDouble(a.component_value) }).Sum(a => a.val));
//                CultureInfo hindi = new CultureInfo("hi-IN");
//                var templogo = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.CompanyLogoPath);
//                string company_logo = "";
//                if (templogo != null)
//                {
//                    string path = logobasepath + "/" + templogo.values;
//                    byte[] byt = System.IO.File.ReadAllBytes(path);
//                    company_logo = "data:image/png;base64," + Convert.ToBase64String(byt);
//                }

//                var tempcompnayname = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.CompanyName);
//                string company_name = " ";
//                if (tempcompnayname != null)
//                {
//                    company_name = tempcompnayname.values;
//                }

//                string company_address1 = "";
//                string company_address2 = "";
//                //var tempcompnay_address = _context.tbl_company_master.Where(p => p.company_id == (int)enmOtherComponent.CompanyId).Select(
//                //    p => new
//                //    {
//                //        address_line_one = p.address_line_one,
//                //        address_line_two = p.address_line_two,
//                //        state = p.tbl_state.name,
//                //        country = p.tbl_country.name,
//                //        city = p.tbl_city.name,
//                //        pin_code = p.pin_code
//                //    }
//                //    ).FirstOrDefault();
//                var company_idget = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.CompanyId);
//                int company_idd = 0;
//                if (company_idget != null)
//                {
//                    company_idd = Convert.ToInt32(company_idget.values);
//                }

//                var tempcompnay_address = _context.tbl_company_master.Where(p => p.company_id == company_idd).Select(
//                    p => new
//                    {
//                        address_line_one = p.address_line_one,
//                        address_line_two = p.address_line_two,
//                        state = p.tbl_state.name,
//                        country = p.tbl_country.name,
//                        city = p.tbl_city.name,
//                        pin_code = p.pin_code
//                    }
//                    ).FirstOrDefault();
//                if (tempcompnay_address != null)
//                {
//                    company_address1 = tempcompnay_address.address_line_one;
//                    company_address2 = string.Concat(tempcompnay_address.address_line_two, " ", tempcompnay_address.state, tempcompnay_address.city, tempcompnay_address.country);
//                }

//                var company_location = _context.tbl_location_master.Where(x => x.company_id == company_idd && x.is_active == 1).FirstOrDefault();

//                string payroll_date = DateTime.Now.ToString("dd-MM-yyyy");
//                if (month_year.ToString().Length == 6)
//                {
//                    string year = month_year.ToString().Substring(0, 4);
//                    string month = month_year.ToString().Substring(4, 2);
//                    payroll_date = Convert.ToDateTime(year + "-" + month + "-01").AddMonths(1).AddDays(-1).ToString("dd-MM-yyyy");
//                }

//                var tempemp_name = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpName);
//                string emp_name = "-";
//                if (tempemp_name != null)
//                {
//                    emp_name = tempemp_name.values;
//                }
//                var tempemail_id = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpEmail);
//                string email_id = "-";
//                if (tempemail_id != null)
//                {
//                    email_id = tempemail_id.values;
//                }

//                var tempemail_adhno = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.AdharNo);
//                string adhaarno = "-";
//                if (tempemail_adhno != null)
//                {
//                    adhaarno = tempemail_adhno.values;
//                }

//                var tempemployee_code = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpCode);
//                string employee_code = "-";
//                if (tempemployee_code != null)
//                {
//                    employee_code = tempemployee_code.values;
//                }

//                var tempemp_grade = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpGrade);
//                string emp_grade = "-";
//                if (tempemp_grade != null)
//                {
//                    emp_grade = tempemp_grade.values;
//                }


//                var tempdept_name = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpDepartment); //salary_Inputs.FirstOrDefault(p => p.component_id == 995);
//                string dept_name = "-";
//                if (tempdept_name != null)
//                {
//                    dept_name = tempdept_name.values;
//                }
//                var tempdesignation_name = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpDesignation); //salary_Inputs.FirstOrDefault(p => p.component_id == 995);
//                string designation_name = "-";
//                if (tempdesignation_name != null)
//                {
//                    designation_name = tempdesignation_name.values;
//                }

//                string emp_father_husband = "";
//                var emp_father_husband_name = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpFatherHusbandName);
//                if (emp_father_husband_name != null)
//                {
//                    emp_father_husband = emp_father_husband_name.values;
//                }

//                var tempdateofjoining = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.EmpJoiningDt);
//                string dateofjoining = "-";
//                if (tempdateofjoining != null)
//                {
//                    dateofjoining = tempdateofjoining.values;
//                }


//                string emp_uan = "";
//                var tempemp_uan = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.Uan);
//                if (tempemp_uan != null)
//                {
//                    emp_uan = tempemp_uan.values;
//                }

//                string emp_pf_no = "";
//                var tempemp_pf_no = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.PfNo);
//                if (tempemp_pf_no != null)
//                {
//                    emp_pf_no = tempemp_pf_no.values;
//                }

//                string emp_esic_no = "";
//                var temp_emp_esic_no = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.ESICNo);
//                if (temp_emp_esic_no != null)
//                {
//                    emp_esic_no = temp_emp_esic_no.values;
//                }


//                var tempdays_in_month = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.ToalPayrollDay);
//                string days_in_month = "30";
//                if (tempdays_in_month != null)
//                {
//                    days_in_month = tempdays_in_month.values;
//                }
//                var tempprsnt_count_in_mnth = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.PaidDays);
//                string prsnt_count_in_mnth = "0";
//                if (tempprsnt_count_in_mnth != null)
//                {
//                    prsnt_count_in_mnth = tempprsnt_count_in_mnth.values;
//                }

//                string arrear_Day = "0";
//                var temp_arrear_day = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.ArrearDays);
//                if (temp_arrear_day != null)
//                {
//                    arrear_Day = temp_arrear_day.values;
//                }

//                string absnt_count_in_mnth = string.Format("{0:.##}", (Convert.ToDouble(days_in_month) - Convert.ToDouble(prsnt_count_in_mnth)));

//                var tempemp_pan_number = salary_Inputs.FirstOrDefault(p => p.component_id == (int)enmOtherComponent.PanNo);
//                string emp_pan_number = "-";
//                if (tempemp_pan_number != null)
//                {
//                    emp_pan_number = tempemp_pan_number.values;
//                }



//                StringBuilder salary_component = new StringBuilder();
//                var component_mstr = _context.tbl_component_master;
//                var income_comp = component_mstr.Where(x => x.component_type == (int)enmComponentType.Income || x.component_type == (int)enmComponentType.OtherIncome && x.is_active == 1).OrderBy(p => p.component_id).ToList();
//                var deduction_comp = component_mstr.Where(x => x.component_type == (int)enmComponentType.Deduction || x.component_type == (int)enmComponentType.OtherDeduction && x.is_active == 1).OrderBy(p => p.component_id).ToList();

//                var total_gross = 0;
//                var total_arrear_earning = 0;
//                var total_arrear_deduction = 0;
//                //remove the income compenen
//                for (int i = income_comp.Count - 1; i >= 0; i--)
//                {
//                    if (!(salary_Inputs.Any(p => p.component_id == income_comp[i].component_id && (p.current_month_value > 0 || p.arrear_value > 0))))
//                    {
//                        income_comp.RemoveAt(i);
//                    }
//                }
//                //remove the deduction compenent
//                for (int i = deduction_comp.Count - 1; i >= 0; i--)
//                {
//                    if (!(salary_Inputs.Any(p => p.component_id == deduction_comp[i].component_id && (p.current_month_value > 0 || p.arrear_value > 0))))
//                    {
//                        deduction_comp.RemoveAt(i);
//                    }
//                }

//                int maxloop = Math.Max(income_comp.Count, deduction_comp.Count);
//                int incomePayabale = 0, incomeArrear = 0, deductionPayabale = 0, deductionArrear = 0;
//                tbl_salary_input tsi = null;
//                for (int i = 0; i < maxloop; i++)
//                {
//                    incomePayabale = 0; incomeArrear = 0; deductionPayabale = 0; deductionArrear = 0; tsi = null;
//                    salary_component.AppendLine();
//                    salary_component.Append("<tr>");
//                    if (i < income_comp.Count)
//                    {
//                        tsi = salary_Inputs.FirstOrDefault(p => p.component_id == income_comp[i].component_id);
//                        if (tsi != null)
//                        {
//                            incomePayabale = Convert.ToInt32(tsi.current_month_value);
//                            total_gross += incomePayabale;
//                            incomeArrear = Convert.ToInt32(tsi.arrear_value);
//                            total_arrear_earning += incomeArrear;
//                        }
//                        salary_component.AppendFormat("<th colspan='2'> {0} </th><td>{1}</td><td class='myAlign'> {2} </td>", income_comp[i].property_details, incomePayabale, incomeArrear);
//                    }
//                    else
//                    {
//                        salary_component.Append("<th colspan='2'>  </th><td></td><td class='myAlign'>  </td>");
//                    }
//                    tsi = null;
//                    if (i < deduction_comp.Count)
//                    {
//                        if (deduction_comp[i].property_details.ToUpper() != "ER ESIC")
//                        {
//                            tsi = salary_Inputs.FirstOrDefault(p => p.component_id == deduction_comp[i].component_id);
//                            if (tsi != null)
//                            {
//                                deductionPayabale = Convert.ToInt32(tsi.current_month_value);
//                                deductionArrear = Convert.ToInt32(tsi.arrear_value);
//                                total_arrear_deduction += deductionArrear;
//                            }
//                            salary_component.AppendFormat("<th colspan='2'> {0} </th><td>{1}</td><td class='myAlign'> {2} </td>", deduction_comp[i].property_details, deductionPayabale, deductionArrear);
//                        }
//                    }
//                    else
//                    {
//                        salary_component.Append("<th colspan='2'>  </th><td></td><td class='myAlign'>  </td>");
//                    }
//                }
//                string LOP_Days = "0";
//                var temLOP_Days = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.LopDays);
//                if (temLOP_Days != null)
//                {
//                    LOP_Days = temLOP_Days.values;
//                }
//                double total_income = 0;
//                var temtotal_income = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.TotalGross);
//                if (temtotal_income != null)
//                {
//                    total_income = temtotal_income.current_month_value;
//                }

//                double total_deduction = 0, net = 0;
//                var temp_totaldeduc = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.Deduction);
//                if (temp_totaldeduc != null)
//                {
//                    total_deduction = temp_totaldeduc.current_month_value;
//                }
//                var temp_net = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.Net);
//                if (temp_net != null)
//                {
//                    net = temp_net.current_month_value;
//                }

//                net = total_income - total_deduction;

//                var rupeesnet = convertrupees(net.ToString());
//              //  var rupees_in_words = NumbersToWords(Convert.ToInt32(net));


//                string GrossSalary = "";
//                var temp_GrossSalary = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.GrossSalary);
//                if (temp_GrossSalary != null)
//                {
//                    GrossSalary = temp_GrossSalary.values;
//                }

//                string arreardays = "";
//                var temp_arreardays = salary_Inputs.FirstOrDefault(x => x.component_id == (int)enmOtherComponent.ArrearDays);
//                if (temp_arreardays != null)
//                {
//                    arreardays = temp_arreardays.values;
//                }

//                sb.Append(@"<style>
//.salary-slip .head{margin:10px;margin-bottom:50px;width:100%}.salary-slip .companyName{text-align:right;font-size:25px;font-weight:700}.salary-slip .salaryMonth{text-align:center}.salary-slip .table-border-bottom{border-bottom:1px solid #a3a4a5}.salary-slip .table-border-right{border-right:1px solid #a3a4a5}.salary-slip .myBackground{padding-top:10px;text-align:left;border:1px solid #a3a4a5;height:40px}.salary-slip .myAlign{text-align:center;border-right:1px solid #a3a4a5}.salary-slip .myTotalBackground{padding-top:10px;text-align:left;background-color:#ebf1de;border-spacing:0}.salary-slip .align-4{width:25%;float:left}.salary-slip .tail{margin-top:35px}.salary-slip .align-2{margin-top:25px;width:50%;float:left}.salary-slip .border-center{text-align:center}.salary-slip .border-center td,.salary-slip .border-center th{border:1px solid #a3a4a5}.salary-slip td,.salary-slip th{padding-left:6px}
//</style>");



//                // sb.Append(@"@import url('https://fonts.googleapis.com/css?family=Roboto:200,300,400,600,700');");
//                sb.Append(@"
//<div  class='salary-slip'>
//<table class='empDetail' style='max-width:1100px;margin:0 auto;font-size:13px;font-family:sans-serif;background:#fff;border-radius:3px;width:100%;text-align:left;border:1px solid #a3a4a5;border-collapse:collapse'>
//            <tr height='110px' style='background-color: #eaf5ff; '>
//                <td><img height='90px' src=#company_logo# /></td>
//                <td colspan='6' class='companyName' style='font-size: 13px; text-align:center; line-height: 18px; padding-right: 5px;'>
//                    <strong style='font-size: 18px;'>#company_name#</strong><br>
//                    <span style='font-size: 13px;'>#company_address1#</span><br>
//                    <span style='font-size: 13px;'>#company_address2#</span><br>
//                </td>
//                <td colspan='1' class='myAlign'></td>
//            </tr>
//            <tr>
//                <td colspan='8' style='text-align: center; padding-bottom: 7px;font-size: 20px;border-top: 1px solid #a3a4a5;border-bottom: 1px solid #a3a4a5;'>
//                    Full and Final Settlement
//                </td>
//            </tr>
//            <tr>
//                <th colspan='2'>Employee Code </th>
//                <td>#employee_code# </td>
//                <td></td>
//                <th colspan='3'>Employee Name </th>
//                <td style='text-align:left;padding-top: 7px;'  class='myAlign'> #emp_name# </td>
                
//            </tr>
//            <tr>
//                <th colspan='2'>Branch </th>
//                <td>#branch# </td>
//                <td></td>
//                <th colspan='3'> Work Location </th>
//                <td style='text-align:left' class='myAlign'> #location# </td>
                 
//            </tr>
//            <tr>

//                <th colspan='2'> Department </th>
//                <td> #dept_name# </td>
//                <td></td>
//                <th colspan='3'> Designaion </th>
//                <td  style='text-align:left' class='myAlign'> #designation_name# </td>
                 
//            </tr>
//            <tr>
//                <th colspan='2'> Grade </th>
//                <td> #emp_grade# </td>
//                <td></td>
//                <th colspan='3'> Category</th>
//                <td  style='text-align:left' class='myAlign'> #emp_category# </td>
                 
//            </tr>
//            <tr>
//                <th colspan='2'> Date Of Joining </th>
//                <td> #dateofjoining#</td>
//                <td></td>
//                <th colspan='3'> PAN No.</th>
//                <td  style='text-align:left' class='myAlign'> #emp_pan_number# </td>
               
//            </tr>
//            <tr>
//                <th colspan='2'> PF Account No. </th>
//                <td> #emp_pf_no# </td>
//                <td></td>
//                <th colspan='3'> ESIC Number </th>
//                <td  style='text-align:left' class='myAlign'> #emp_esic_no# </td>
                 
//            </tr>
//            <tr>
//                <th colspan='2'> UAN Number </th>
//                <td> #emp_uan# </td>
//                <td></td>
//                <th colspan='3'>Total Days in Month</th>
//                <td style='text-align:left' class='myAlign'> #days_in_month# </td>
                 
//            </tr>
           
//            <tr>
//                <th colspan='2'>Aadhar No. </th>
//                <td> #aadhar_no# </td>
                 
//                 <td></td>
//                <th colspan='3'>Resignation Date</th>
//                <td  style='text-align:left;' class='myAlign'>#resignation_date#</td>
//            </tr>

//           <tr>
//                <th colspan='2'>Days Paid</th>
//                <td> #prsnt_count_in_mnth# </td>
//                <td></td>
//                 <th colspan='3'>Monthly Gross</th>
//                <td  style='text-align:left;' class='myAlign'>#gross_salary#</td>
                 
//            </tr>
//            <tr>
//                <th colspan='2'>Last Working Date</th>
//                <td> #last_working_date# </td>
//                <td></td>
//                <th colspan='3'>Encashment Days</th>
//                <td  style='text-align:left;' class='myAlign'>#encashment_days#</td>
                 
//            </tr>
//            <tr>
//                <th colspan='2'>Notice Payment Days</th>
//                <td> #notice_payment_days# </td>
//                <td></td>
//                <th colspan='3'>Notice Recovery Days</th>
//                <td  style='text-align:left;padding-bottom: 7px;' class='myAlign'>#notice_recovery_days#</td>
                 
//            </tr>
//            <tr class='myBackground'>
//                <th colspan='2'> Earnings </th>
//                <th> Amount </th>
//                <th class='myAlign'> Arrear/Adj </th>
//                <th colspan='2'> Deductions</th>
//                <th> Amount </th>
//                <th class='myAlign'> Arrear/Adj </th>
//            </tr>
//            #salary_component#
//            <tr class='myBackground'>
//                <td colspan='2' style='font-weight:bold'>Gross Earning</td>
//                <td> #gross_earning# </td>
//                <td class='myAlign'>#arrear_Day#</td>
//                <td colspan='2' style='font-weight:bold'>Gross Deduction</td>
//                <td> #total_deduction# </td>
//                <td class='myAlign'>#arrear_deduction#</td>
//            </tr>
//            <tr class='myBackground'>
//                <td colspan='2' style='font-weight:bold'>Net Earning</td>
//                <td> #net# </td>
//                <td></td>
//                <td> </td>
//                <td></td>
//                <td colspan='2' class='myAlign'></td>

//            </tr>
          
//            <tr class='myBackground'>
//                <td colspan='8' style='padding-top: 12px;padding-bottom: 15px;line-height: 20px;'>
//                    <div>
//                        <span style='font-weight:bold'>Settlement Amount:</span>
//                        <span> #settlement_amt# /-</span>
//                    </div>
//                    <div>
//                        <span style='font-weight:bold'>Net Pay:</span>
//                        <span> #net1# /-</span>
//                    </div>
//                    <div>
//                        <span style='font-weight:bold'>Net Earning in words : </span>
//                        <span>Rupees #netrupees# Only</span>
//                    </div>
//                </td>
//            </tr>
//            <tr class='myBackground' style='text-align:center'>
//                <td colspan='8'>This is a system generated report and does not require signature or stamp</td>
//            </tr>
//        </table>
//</div>");
//                //, company_logo, company_name, payroll_date, emp_name, email_id, employee_code, 
//                //emp_grade, dateofjoining, dept_name,
//                //    designation_name, days_in_month, prsnt_count_in_mnth, absnt_count_in_mnth, emp_pan_number, htmlcompDetails);

//                sb.Replace("#company_logo#", company_logo);
//                sb.Replace("#company_name#", company_name.Replace(" - C00001", ""));
//                sb.Replace("#company_address1", company_address1);
//                sb.Replace("#company_address2#", company_address2);
//                sb.Replace("#payroll_date#", payroll_date);
//                sb.Replace("#employee_code#", employee_code);
//                sb.Replace("#emp_pan_number#", emp_pan_number);
//                sb.Replace("#emp_grade#", emp_grade);
//                sb.Replace("#emp_name#", emp_name);
//                sb.Replace("#dept_name#", dept_name);
//                sb.Replace("#designation_name#", designation_name);
//                sb.Replace("#emp_father_husband#", emp_father_husband);
//                sb.Replace("#dateofjoining#", dateofjoining);
//                sb.Replace("#emp_uan#", emp_uan);
//                sb.Replace("#emp_pf_no#", emp_pf_no);
//                sb.Replace("#emp_esic_no#", emp_esic_no);
//                sb.Replace("#salary_component#", salary_component.ToString());
//                sb.Replace("#total_income#", total_income.ToString());
//                sb.Replace("#total_deduction#", total_deduction.ToString());
//                sb.Replace("#days_in_month#", days_in_month);
//                sb.Replace("#prsnt_count_in_mnth#", prsnt_count_in_mnth.ToString());
//                sb.Replace("#arrear_Day#", total_arrear_earning.ToString());// arrear_Day);
//                sb.Replace("#gross_earning#", total_gross.ToString());
//                sb.Replace("#net#", net.ToString());
//              //  sb.Replace("#net1#", rupeesnet.ToString());
//                //sb.Replace("#netrupees#", rupees_in_words.ToString());
//                sb.Replace("#aadhar_no#", adhaarno.ToString());
//                sb.Replace("#arrear_deduction#", total_arrear_deduction.ToString());
//                //sb.Replace("#lwp_days#", LOP_Days);
//                sb.Replace("#gross_salary#", GrossSalary);
//                sb.Replace("#arrear_days#", arreardays);

//                sb.Replace("#location#", company_location.location_name);
//                sb.Replace("#branch#", company_location.location_code);

//                var emp_off_sec = 0;// _context.tbl_emp_officaial_sec.Where(x => x.employee_id == emp_id && x.is_deleted == 0).FirstOrDefault().user_type;
//                var emp_category = "";

//                sb.Replace("#emp_category#", company_location.location_code);


//                sb.Replace("#settlement_amt#", fnf_mstr.settlment_amt.ToString());
//                sb.Replace("#net1#", fnf_mstr.net_amt.ToString());

//               // var rupees_in_words = NumbersToWords(Convert.ToInt32(net));

//                sb.Replace("#netrupees#", NumbersToWords(Convert.ToInt32(net)).ToString());
//                if (sp.resignation_dt != null)
//                {
//                    sb.Replace("#resignation_date#", sp.resignation_dt.ToString("dd-MM-yyyy"));
//                }
//                if (fnf_mstr.last_working_date != null)
//                {
//                    sb.Replace("#last_working_date#", fnf_mstr.last_working_date.ToString("dd-MM-yyyy"));
//                }
//                sb.Replace("#notice_payment_days#", fnf_mstr.notice_payment_days.ToString());
//                sb.Replace("#notice_recovery_days#", fnf_mstr.notice_recovery_days.ToString());
//                sb.Replace("#encashment_days#", fnf_leave.ToString());


//                //sb.Replace("#email_id#", email_id);





//                //sb.Replace("#days_in_month#", days_in_month);
//                //sb.Replace("#prsnt_count_in_mnth#", prsnt_count_in_mnth);
//                //sb.Replace("#absnt_count_in_mnth#", absnt_count_in_mnth);

//                //sb.Replace("#htmlcompDetails#", htmlcompDetails);
//                return sb.ToString();
//            }
//            catch (Exception EX)
//            {
//                return EX.Message;
//            }


//        }
        
//    }

}

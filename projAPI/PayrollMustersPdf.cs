using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using projContext;
using System.Globalization;

namespace projAPI
{
    public class PayrollMustersPdf
    {
        private readonly Context _context;
        private readonly int _company_id;
        private readonly int _payroll_month;
        private readonly string payroll_month_name_and_year;
        public PayrollMustersPdf(Context context, int company_id, int payroll_month)
        {
            _context = context;
            _company_id = company_id;
            _payroll_month = payroll_month;
            payroll_month_name_and_year = string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(_payroll_month.ToString().Substring(4, 2))),_payroll_month.ToString().Substring(0,4));
        }

        public string TheRegisterOfFinesForm1()
        {
            var sb = new StringBuilder();

            var company_name = _context.tbl_company_master.Where(a => a.is_active == 1 && a.company_id == _company_id).Select(a => new { a.company_name, a.address_line_one, a.address_line_two, a.city_id, a.state_id, a.pin_code }).FirstOrDefault();

            var state_name = _context.tbl_state.Where(a => a.is_deleted == 0 && a.state_id == company_name.state_id).Select(a => a.name).FirstOrDefault();

            var city_name = _context.tbl_city.Where(a => a.is_deleted == 0 && a.city_id == company_name.city_id).Select(a => a.name).FirstOrDefault();

            var get_freedz_data = _context.tbl_muster_form1_data.Where(a => a.company_id == _company_id && a.payroll_month == _payroll_month && a.is_deleted == 0)
                   .AsEnumerable().Select((a, index) => new
                   {
                       a.employee_code,
                       employee_id = a.emp_id,
                       a.company_id,
                       a.payroll_month,
                       employe_name = a.employee_name,
                       father_or_husband = a.father_or_husband_name,
                       a.department,
                       a.nature_and_date,
                       a.whether_workman,
                       a.rate_of_wages,
                       a.date_of_fine_imposed,
                       a.amt_of_fine_imposed,
                       a.date_realised,
                       a.remarks,
                      payroll_month_name_year=string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(a.payroll_month.ToString().Substring(4,2))),a.payroll_month.ToString().Substring(0,4)),
                       sno = index + 1
                   }).ToList();

            sb.Append(@"<!DOCTYPE html>
<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta charset='utf-8' />
    <title></title>
</head>
<body style='font-family: sans-serif;'>
    <div style='width: 1500px;height: 743px;    margin: 0 auto;'>
        <div style='position: relative;height: 108px;border: 2px solid black;margin-top: 20px;'>
            <h2 style='font-family: sans-serif; margin: 4px 0px;text-align: center;font-size: 22px;font-weight: 600;text-decoration: underline;'>Minimum Wages (Central) Rules</h2>
            <label style='top: 35px;font-weight: 600;font-size: 18px; margin: 0px 0px 0px 4px;position: absolute;'>Name & Address of Establishment:</label>
            <p style='position: absolute;top: 21px;left: 320px;font-size: 18px;font-weight: 600;'>" + company_name.company_name + @"</p>
            <p style='position: absolute;top: 42px;left: 320px;font-size: 18px;font-weight: 600;margin: 25px 0px 0px 0px;'>" + company_name.address_line_one + @", " + company_name.address_line_two + @" " + city_name + @" " + state_name + @"-" + company_name.pin_code + @"</p>
            <label style='position: absolute;top: 53px;left: 0;font-family: sans-serif;font-size: 18px;font-weight: 600; margin: 10px 0px 0px 4px;'>Month & Year:</label>
            <p style='position: absolute;top: 39px;left: 150px;font-size: 18px;font-weight: 600; margin: 25px 0px 0px 0px;'>" + payroll_month_name_and_year + @"</p>
            <label style='position: absolute;right: 100px;font-weight: 600;font-size: 18px;top:37px;'>Form ‐ I</label>
            <label style='position: absolute;right: 67px;top: 57px;font-weight: 600;font-size: 18px;'>Register of Fines</label>
        </div>
        <table style='margin: 0;border-collapse: collapse;width:100%;'>
            <thead>
                <tr>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Serial No</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Name</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Father's/Husband's Name</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Department</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 152px;line-height: 16px;'>Nature and date of the offence for which Fine imposed</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>Whether Workman Showed Cause Against Fine or not, if so enter date</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Rate of Wages</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 0px;padding-top: 10px;'>
                        Date & Amount of fine Imposed
                        <table style='margin: 0;border-collapse: collapse;width: 100%;margin-top: 11px;line-height:27px;'>
                            <tr>
                                <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;border-left:0'>Date</th>
                                <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;'>Rs.</th>
                                <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;border-right:0;'>P.</th>
                            </tr>
                        </table>
                    </th>
                    <th style='font-size: 15px;border: 1px solid black;width: 98px;'>Date on which fine realised</th>
                    <th style='font-size: 15px;border: 1px solid black;'>Remarks</th>
                </tr>
            </thead>
            <tbody>");
            foreach (var emp_details in get_freedz_data)
            {
                sb.AppendFormat(@"<tr>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.sno + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.employe_name + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.father_or_husband + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.department + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 152px;line-height: 16px;'>" + emp_details.nature_and_date + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + emp_details.whether_workman + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.rate_of_wages + @"</td>
                    <td style='padding:0px 0px 0px 0px;border: 1px solid;'>
                        <table style='border-collapse: collapse;width: 100%;height: 28px;'>
                            <tr>
                                <td style='font-size: 15px;border-right:1px solid;width: 45.33%;'>"+emp_details.date_of_fine_imposed+@"</td>
                                <td style='font-size: 15px;border-right:1px solid;width: 34.8%;'>" + emp_details.amt_of_fine_imposed + @"</td>
                                <td style='font-size: 15px;'>00</td>
                            </tr>
                        </table>
                    </td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.date_realised + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.remarks + @"</td>
                </tr>");
            }
            sb.AppendFormat(@"</tbody>
        </table>
    </div>
</body>
</html>");
            return sb.ToString();
        }


        public string TheRegisterOfFinesForm2()
        {
            var sb = new StringBuilder();

            var company_name = _context.tbl_company_master.Where(a => a.is_active == 1 && a.company_id == _company_id).Select(a => new { a.company_name, a.address_line_one, a.address_line_two, a.city_id, a.state_id, a.pin_code }).FirstOrDefault();

            var state_name = _context.tbl_state.Where(a => a.is_deleted == 0 && a.state_id == company_name.state_id).Select(a => a.name).FirstOrDefault();

            var city_name = _context.tbl_city.Where(a => a.is_deleted == 0 && a.city_id == company_name.city_id).Select(a => a.name).FirstOrDefault();

            var get_freedz_data = _context.tbl_muster_form2_data.Where(a => a.company_id == _company_id && a.payroll_month == _payroll_month && a.is_deleted == 0)
                    .AsEnumerable().Select((a, index) => new
                    {
                        a.employee_code,
                        employee_id = a.emp_id,
                        a.company_id,
                        a.payroll_month,
                        employe_name = a.employee_name,
                        father_or_husband = a.father_or_husband_name,
                        a.department,
                        a.damage_orloss_and_date,
                        a.whether_workman,
                        a.date_of_deduc_imposed,
                        a.amt_of_deduc_imposed,
                        a.no_of_installment,
                        a.date_realised,
                        a.remarks,
                        a.gender,
                        payroll_month_name_year = string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(a.payroll_month.ToString().Substring(4, 2))), a.payroll_month.ToString().Substring(0, 4)),
                        sno = index + 1
                    }).ToList();

            sb.Append(@"<!DOCTYPE html>
<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta charset='utf-8' />
    <title></title>
</head>
<body style='font-family: sans-serif;'>
    <div style='width: 1500px;height: 743px margin: 10px 5px 0px 5px;'>
        <div style='position: relative;height: 150px;border: 2px solid black;margin-top: 20px;'>
            <h2 style='font-family: sans-serif; margin: 10px 0px 0px 200px;text-align: left;font-size: 15px;font-weight: 600;'>Minimum Wages (Central) Rules</h2>
            <label style='position: absolute;font-weight: 600;font-size: 18px;top:37px; margin: 10px 0px 20px 100px;'>Register of Deductions for Damage or Loss Caused to the Employer By the Neglect or Default of the Employed Persons</label></br>
            <label style='top: 35px;font-weight: 600;font-size: 14px; margin: 50px 0px 0px 4px;position: absolute;'>Name & Address of Establishment:</label>
            <p style='position: absolute;top: 70px;left: 320px;font-size: 14px;font-weight: 600;'>" + company_name.company_name + @"</p>
            <p style='position: absolute;top: 90px;left: 320px;font-size: 14px;font-weight: 600;margin: 25px 0px 0px 0px;'>" + company_name.address_line_one + @", " + company_name.address_line_two + @" " + city_name + @" " + state_name + @"-" + company_name.pin_code + @"</p>
            <label style='position: absolute;top: 100px;left: 0;font-family: sans-serif;font-size: 14px;font-weight: 600; margin: 10px 0px 0px 4px;'>Month & Year:</label>
            <p style='position: absolute;top: 85px;left: 150px;font-size: 14px;font-weight: 600; margin: 25px 0px 0px 0px;'>" + payroll_month_name_and_year + @"</p>
            <label style='position: absolute;right: 100px;font-weight: 600;font-size: 14px; margin: -30px 400px 0px 0px'>Form ‐ II</label>
           
           
        </div>
        <table style='margin: 0px 0px 0px 0px;border-collapse: collapse;width:100%;'>
            <thead>
                <tr>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Serial No</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Name</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Father's/Husband's Name</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Sex</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Department</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 152px;line-height: 16px;'>Damage or Loss Caused with Date</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>Whether Worker Showed Cause Against Deduction, if so enter date</th>
                    
                    <th style='font-size: 15px;border: 1px solid black;padding: 0px;padding-top: 10px;'>
                        Date & Amount of Deduction Imposed
                        <table style='margin: 0;border-collapse: collapse;width: 100%;margin-top: 11px;line-height:27px;'>
                            <tr>
                                <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;border-left:0'>Date</th>
                                <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;'>Rs.</th>
                                <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;border-right:0;'>P.</th>
                            </tr>
                        </table>
                    </th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>No of Instalment if any</th>
                    <th style='font-size: 15px;border: 1px solid black;width: 98px;'>Date on which Total Amount realised</th>
                    <th style='font-size: 15px;border: 1px solid black;'>Remarks</th>
                </tr>
            </thead>
            <tbody>");
            foreach (var emp_details in get_freedz_data)
            {
                sb.AppendFormat(@"<tr>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.sno + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.employe_name + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.father_or_husband + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.gender + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.department + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 152px;line-height: 16px;'>" + emp_details.damage_orloss_and_date + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + emp_details.whether_workman + @"</td>
                    
                    <td style='padding:0px 0px 0px 0px;border: 1px solid;'>
                        <table style='border-collapse: collapse;width: 100%;height: 28px;'>
                            <tr>
                                <td style='font-size: 15px;border-right:1px solid;width: 45.33%;'>"+emp_details.date_of_deduc_imposed + @"</td>
                                <td style='font-size: 15px;border-right:1px solid;width: 34.8%;'>" + emp_details.amt_of_deduc_imposed + @"</td>
                                <td style='font-size: 15px;'>00</td>
                            </tr>
                        </table>
                    </td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.no_of_installment + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.date_realised + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.remarks + @"</td>
                </tr>");
            }
            sb.AppendFormat(@"</tbody>
        </table>
    </div>
</body>
</html>");
            return sb.ToString();
        }


        public string TheRegisterOfFinesForm3()
        {
            var sb = new StringBuilder();

            var company_name = _context.tbl_company_master.Where(a => a.is_active == 1 && a.company_id == _company_id).Select(a => new { a.company_name, a.address_line_one, a.address_line_two, a.city_id, a.state_id, a.pin_code }).FirstOrDefault();

            var state_name = _context.tbl_state.Where(a => a.is_deleted == 0 && a.state_id == company_name.state_id).Select(a => a.name).FirstOrDefault();

            var city_name = _context.tbl_city.Where(a => a.is_deleted == 0 && a.city_id == company_name.city_id).Select(a => a.name).FirstOrDefault();

            var get_freedz_data = _context.tbl_muster_form3_data.Where(a => a.company_id == _company_id && a.payroll_month == _payroll_month && a.is_deleted == 0)
                   .AsEnumerable().Select((a, index) => new
                   {
                       a.employee_code,
                       employee_id = a.emp_id,
                       a.company_id,
                       a.payroll_month,
                       employe_name = a.employee_name,
                       father_or_husband = a.father_or_husband_name,
                       a.department,
                       a.advance_amount,
                       a.advance_date,
                       a.purpose,
                       a.no_of_installment,
                       a.postponse_granted,
                       a.date_of_repaid,
                       a.remarks,
                       payroll_month_name_year = string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(a.payroll_month.ToString().Substring(4, 2))), a.payroll_month.ToString().Substring(0, 4)),
                       sno = index + 1
                   }).ToList();

            sb.Append(@"<!DOCTYPE html>
<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta charset='utf-8' />
    <title></title>
</head>
<body style='font-family: sans-serif;'>
    <div style='width: 1500px;height: 743px;    margin: 0 auto;'>
        <div style='position: relative;height: 108px;border: 2px solid black;margin-top: 20px;'>
            <h2 style='font-family: sans-serif; margin: 4px 0px;text-align: center;font-size: 22px;font-weight: 600;'>Register of Advance Made to Employed Persons</h2>
            <label style='top: 35px;font-weight: 600;font-size: 18px; margin: 0px 0px 0px 4px;position: absolute;'>Name & Address of Establishment:</label>
            <p style='position: absolute;top: 21px;left: 320px;font-size: 18px;font-weight: 600;'>" + company_name.company_name + @"</p>
            <p style='position: absolute;top: 42px;left: 320px;font-size: 18px;font-weight: 600;margin: 25px 0px 0px 0px;'>" + company_name.address_line_one + @", " + company_name.address_line_two + @" " + city_name + @" " + state_name + @"-" + company_name.pin_code + @"</p>
            <label style='position: absolute;top: 53px;left: 0;font-family: sans-serif;font-size: 18px;font-weight: 600; margin: 10px 0px 0px 4px;'>Month & Year:</label>
            <p style='position: absolute;top: 39px;left: 150px;font-size: 18px;font-weight: 600; margin: 25px 0px 0px 0px;'>" + payroll_month_name_and_year + @"</p>
            <label style='position: absolute;right: 100px;font-weight: 600;font-size: 18px;top:37px;'>Form ‐ III (PW)</label>
            
        </div>
        <table style='margin: 0;border-collapse: collapse;width:100%;'>
            <thead>
                <tr>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Serial No</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Name of Employee</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Father's/Husband's Name</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Department</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 0px;padding-top: 10px;'>
                        Date and amount of Advance Made
                        <table style='margin: 0;border-collapse: collapse;width: 100%;margin-top: 11px;line-height:27px;'>
                            <tr>
                                <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;border-left:0'>Date</th>
                                <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;'>Rs.</th>
                                <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;border-right:0;'>P.</th>
                            </tr>
                        </table>
                    </th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>Purpose(s) for which advance made</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>No of Instalments by which advance to be repaid</th>
                    
                    <th style='font-size: 15px;border: 1px solid black;width: 98px;'>Postponement granted</th>
                    <th style='font-size: 15px;border: 1px solid black;width: 98px;'>Date on which Total repaid</th>
                    <th style='font-size: 15px;border: 1px solid black;'>Remarks</th>
                </tr>
            </thead>
            <tbody>");
            foreach (var emp_details in get_freedz_data)
            {
                sb.AppendFormat(@"<tr>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.sno + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.employe_name + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.father_or_husband + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.department + @"</td>
                    <td style='padding:0px 0px 0px 0px;border: 1px solid;'>
                        <table style='border-collapse: collapse;width: 100%;height: 28px;'>
                            <tr>
                                <td style='font-size: 15px;border-right:1px solid;width: 45.33%;'>" + emp_details.advance_date + @"</td>
                                <td style='font-size: 15px;border-right:1px solid;width: 34.8%;'>" + emp_details.advance_amount + @"</td>
                                <td style='font-size: 15px;'>00</td>
                            </tr>
                        </table>
                    </td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + emp_details.purpose + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.no_of_installment + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.postponse_granted + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.date_of_repaid + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.remarks + @"</td>
                </tr>");
            }
            sb.AppendFormat(@"</tbody>
        </table>
    </div>
</body>
</html>");
            return sb.ToString();
        }


        public string TheRegisterOfFinesForm4()
        {
            var sb = new StringBuilder();

            var company_name = _context.tbl_company_master.Where(a => a.is_active == 1 && a.company_id == _company_id).Select(a => new { a.company_name, a.address_line_one, a.address_line_two, a.city_id, a.state_id, a.pin_code }).FirstOrDefault();

            var state_name = _context.tbl_state.Where(a => a.is_deleted == 0 && a.state_id == company_name.state_id).Select(a => a.name).FirstOrDefault();

            var city_name = _context.tbl_city.Where(a => a.is_deleted == 0 && a.city_id == company_name.city_id).Select(a => a.name).FirstOrDefault();

            var get_freedz_data = _context.tbl_muster_form4_data.Where(a => a.company_id == _company_id && a.payroll_month == _payroll_month && a.is_deleted == 0)
                   .AsEnumerable().Select((a, index) => new
                   {
                       a.employee_code,
                       employee_id = a.emp_id,
                       a.company_id,
                       a.payroll_month,
                       employe_name = a.employee_name,
                       father_or_husband = a.father_or_husband_name,
                       a.designation,
                       a.department,
                       designation_or_department = string.Format("{0} and {1}", !string.IsNullOrEmpty(a.designation) ? a.designation : "-", !string.IsNullOrEmpty(a.department) ? a.department : "-"),
                       a.overtime_work_dt,
                       a.extent_overtime,
                       a.total_overtime_worked,
                       a.normal_hr,
                       a.normal_rate,
                       a.overtime_rate,
                       a.normal_earning,
                       a.overtime_earning,
                       a.total_earning,
                       a.date_ofpayment,
                       a.sex,
                       payroll_month_name_year = string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(a.payroll_month.ToString().Substring(4, 2))), a.payroll_month.ToString().Substring(0, 4)),
                       sno = index + 1
                   }).ToList();

            sb.Append(@"<!DOCTYPE html>
<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta charset='utf-8' />
    <title></title>
</head>
<body style='font-family: sans-serif;'>
    <div style='width: 1500px;height: 743px;    margin: 0 auto;'>
        <div style='position: relative;height: 108px;border: 2px solid black;margin-top: 20px;'>
            <h2 style='font-family: sans-serif; margin: 4px 0px;text-align: center;font-size: 22px;font-weight: 600;'>Minimum Wages (Central) Rules</h2>
             <h2 style='font-family: sans-serif; margin: 4px 0px;text-align: center;font-size: 22px;font-weight: 600;'>Overtime Register for Workers</h2>
            <label style='top: 35px;font-weight: 600;font-size: 18px; margin: 0px 0px 0px 4px;position: absolute;'>Name & Address of Establishment:</label>
            <p style='position: absolute;top: 21px;left: 320px;font-size: 18px;font-weight: 600;'>" + company_name.company_name + @"</p>
            <p style='position: absolute;top: 42px;left: 320px;font-size: 18px;font-weight: 600;margin: 25px 0px 0px 0px;'>" + company_name.address_line_one + @", " + company_name.address_line_two + @" " + city_name + @" " + state_name + @"-" + company_name.pin_code + @"</p>
            <label style='position: absolute;top: 53px;left: 0;font-family: sans-serif;font-size: 18px;font-weight: 600; margin: 10px 0px 0px 4px;'>Month & Year:</label>
            <p style='position: absolute;top: 39px;left: 150px;font-size: 18px;font-weight: 600; margin: 25px 0px 0px 0px;'>" + payroll_month_name_and_year + @"</p>
            <label style='position: absolute;right: 100px;font-weight: 600;font-size: 18px;top:37px;'>Form ‐ IV</label>
           <label style='position: absolute;right: 100px;font-weight: 600;font-size: 18px;top:37px; margin: 25px 8px 0px 0px;'>Rule 25(2)</label>
        </div>
        <table style='margin: 0;border-collapse: collapse;width:100%;'>
            <thead>
                <tr>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Serial No</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Name</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Father's/Husband's Name</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Sex</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Designation and Department</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 152px;line-height: 16px;'>Date on which Overtime Worked</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Extent of Overtime on each Occasion</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>Total Overtime Worked or Production in case of piece-workers</th>
                    <th style='font-size: 15px;border: 1px solid black;width: 98px;'>Normal hours</th>
                    <th style='font-size: 15px;border: 1px solid black;width: 98px;'>Normal Rate</th>
                    <th style='font-size: 15px;border: 1px solid black;width: 98px;'>Overtime Rate</th>
                    <th style='font-size: 15px;border: 1px solid black;width: 98px;'>Normal Earnings</th>
                    <th style='font-size: 15px;border: 1px solid black;width: 98px;'>Overtime Earnings</th>
                    <th style='font-size: 15px;border: 1px solid black;width: 98px;'>Total Earnings</th>
                    <th style='font-size: 15px;border: 1px solid black;'>Date on which Overtime Payment made</th>
                </tr>
            </thead>
            <tbody>");
            foreach (var emp_details in get_freedz_data)
            {
                sb.AppendFormat(@"<tr>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.sno + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.employe_name + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.father_or_husband + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.sex + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.designation_or_department + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 152px;line-height: 16px;'>" + emp_details.overtime_work_dt + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.extent_overtime + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + emp_details.total_overtime_worked + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.normal_hr + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.normal_rate + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.overtime_rate + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.normal_earning + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.overtime_earning + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.total_earning + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.date_ofpayment + @"</td>
                </tr>");
            }
            sb.AppendFormat(@"</tbody>
        </table>
    </div>
</body>
</html>");
            return sb.ToString();
        }



        public string TheRegisterOfFinesForm10()
        {
            var sb = new StringBuilder();

            var company_name = _context.tbl_company_master.Where(a => a.is_active == 1 && a.company_id == _company_id).Select(a => new { a.company_name, a.address_line_one, a.address_line_two, a.city_id, a.state_id, a.pin_code }).FirstOrDefault();

            var state_name = _context.tbl_state.Where(a => a.is_deleted == 0 && a.state_id == company_name.state_id).Select(a => a.name).FirstOrDefault();

            var city_name = _context.tbl_city.Where(a => a.is_deleted == 0 && a.city_id == company_name.city_id).Select(a => a.name).FirstOrDefault();

            var get_freedz_data = _context.tbl_muster_form10_data.Where(a => a.company_id == _company_id && a.payroll_month == _payroll_month && a.is_deleted == 0)
                   .AsEnumerable().Select((a, index) => new
                   {
                       a.employee_code,
                       employee_id = a.emp_id,
                       a.company_id,
                       a.payroll_month,
                       employe_name = a.employee_name,
                       father_or_husband = a.father_or_husband_name,
                       a.designation,
                       a.basic_minimum_payable,
                       a.da_minimum_payable,
                       a.basic_wages_actually_pay,
                       a.da_wages_actually_pay,
                       a.total_attand_or_unitof_work_done,
                       a.overtime_worked,
                       a.gross_wages_pay,
                       a.contri_of_employer_pf,
                       a.hr_deduction,
                       a.other_deduction,
                       a.total_deduction,
                       a.wages_paid,
                       a.date_of_payment,
                       a.emp_sign_orthump_exp,
                       payroll_month_name_year = string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(a.payroll_month.ToString().Substring(4, 2))), a.payroll_month.ToString().Substring(0, 4)),
                       sno = index + 1
                   }).ToList();

            sb.Append(@"<!DOCTYPE html>
<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta charset='utf-8' />
    <title></title>
</head>
<body style='font-family: sans-serif;'>
    <div style='width: 1500px;height: 743px;    margin: 0 auto;'>
        <div style='position: relative;height: 108px;border: 2px solid black;margin-top: 20px;'>
            <h2 style='font-family: sans-serif; margin: 4px 0px;text-align: center;font-size: 22px;font-weight: 600;'>Minimum Wages (Central) Rules</h2>
            <label style='top: 35px;font-weight: 600;font-size: 18px; margin: 0px 0px 0px 4px;position: absolute;'>Name of Establishment:</label>
            <p style='position: absolute;top: 21px;left: 320px;font-size: 18px;font-weight: 600;'>" + company_name.company_name + @"</p>
            <label style='position: absolute;top: 53px;left: 0;font-family: sans-serif;font-size: 18px;font-weight: 600; margin: 10px 0px 0px 4px;'>Month & Year:</label>
            <p style='position: absolute;top: 39px;left: 150px;font-size: 18px;font-weight: 600; margin: 25px 0px 0px 0px;'>" + payroll_month_name_and_year + @"</p>
            <label style='position: absolute;top: 53px;left: 0;font-family: sans-serif;font-size: 18px;font-weight: 600; margin: 10px 0px 0px 4px;'>Place:</label>
            <p style='position: absolute;top: 42px;left: 320px;font-size: 18px;font-weight: 600;margin: 25px 0px 0px 0px;'>" + company_name.address_line_one + @", " + company_name.address_line_two + @" " + city_name + @" " + state_name + @"-" + company_name.pin_code + @"</p>
            <label style='position: absolute;right: 100px;font-weight: 600;font-size: 18px;top:37px;'>Form ‐ X</label>
            <label style='position: absolute;right: 67px;top: 57px;font-weight: 600;font-size: 18px;'>Register of Wages Rules 26(a)</label>
        </div>
        <table style='margin: 0;border-collapse: collapse;width:100%;'>
            <thead>
                <tr>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Serial No</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Name</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Father's/Husband's Name</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Designation</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 0px;padding-top: 10px;'>
                        Minimum Rates of Payable
                        <table style='margin: 0;border-collapse: collapse;width: 100%;margin-top: 11px;line-height:27px;'>
                            <tr>
                                <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;border-left:0'>Basic</th>
                                <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;border-right:0;'>D.A.</th>
                            </tr>
                        </table>
                    </th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 0px;padding-top: 10px;'>
                        Rate of Wages Actually Paid
                        <table style='margin: 0;border-collapse: collapse;width: 100%;margin-top: 11px;line-height:27px;'>
                            <tr>
                                <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;border-left:0'>Basic</th>
                                <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;border-right:0;'>D.A.</th>
                            </tr>
                        </table>
                    </th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 152px;line-height: 16px;'>Total Attandance/ Unit of Work Done</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>Overtime Worked</th>
                    <th style='font-size: 15px;border: 1px solid black;width: 98px;'>Gross Wages Payable</th>

                    <th style='font-size: 15px;border: 1px solid black;width: 98px;'>Employer'(s) Contribution to P.F</th>
                    <th style='font-size: 15px;border: 1px solid black;padding: 0px;padding-top: 10px;'>
                                            Dedution
                                            <table style='margin: 0;border-collapse: collapse;width: 100%;margin-top: 11px;line-height:27px;'>
                                                <tr>
                                                    <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;border-left:0'>H.R.</th>
                                                    <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;border-left:0'>Other Deduction</th>
                                                    <th style='font-size: 15px;border: 1px solid black;border-bottom: 0;border-right:0;'>Total Deduction</th>
                                                </tr>
                                            </table>
                                        </th>
                    <th style='font-size: 15px;border: 1px solid black;width: 98px;'>Wages Paid</th>
                    <th style='font-size: 15px;border: 1px solid black;width: 98px;'>Date of Payment</th>
                </tr>
            </thead>
            <tbody>");
            foreach (var emp_details in get_freedz_data)
            {
                sb.AppendFormat(@"<tr>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.sno + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.employe_name + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.father_or_husband + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.designation + @"</td>
                    <td style='padding:0px 0px 0px 0px;border: 1px solid;'>
                        <table style='border-collapse: collapse;width: 100%;height: 28px;'>
                            <tr>
                                 <td style='font-size: 15px;border-right:1px solid;width: 34.8%;'>" + emp_details.basic_minimum_payable + @"</td>
                                <td style='font-size: 15px;border-right:1px solid;width: 34.8%;'>" + emp_details.da_minimum_payable + @"</td>
                            </tr>
                        </table>
                    </td>

                    <td style='padding:0px 0px 0px 0px;border: 1px solid;'>
                        <table style='border-collapse: collapse;width: 100%;height: 28px;'>
                            <tr>
                                 <td style='font-size: 15px;border-right:1px solid;width: 34.8%;'>" + emp_details.basic_wages_actually_pay + @"</td>
                                <td style='font-size: 15px;border-right:1px solid;width: 34.8%;'>" + emp_details.da_wages_actually_pay + @"</td>
                            </tr>
                        </table>
                    </td>


                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 152px;line-height: 16px;'>" + emp_details.total_attand_or_unitof_work_done + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + emp_details.overtime_worked + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.gross_wages_pay + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + emp_details.contri_of_employer_pf + @"</td>
                    <td style='padding:0px 0px 0px 0px;border: 1px solid;'>
                        <table style='border-collapse: collapse;width: 100%;height: 28px;'>
                            <tr>
                                 <td style='font-size: 15px;border-right:1px solid;width: 34.8%;'>" + emp_details.hr_deduction + @"</td>
                                <td style='font-size: 15px;border-right:1px solid;width: 34.8%;'>" + emp_details.other_deduction + @"</td>
                                <td style='font-size: 15px;border-right:1px solid;width: 34.8%;'>" + emp_details.total_deduction + @"</td>
                            </tr>
                        </table>
                    </td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.wages_paid + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;'>" + emp_details.date_of_payment + @"</td>
                </tr>");
            }
            sb.AppendFormat(@"</tbody>
        </table>
    </div>
</body>
</html>");
            return sb.ToString();
        }


    }
}

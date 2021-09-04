using projContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using projContext;
using System.Globalization;

namespace projAPI
{
    public class PayrollMustersForm5Pdf
    {
        private readonly Context _context;
        private readonly int _company_id;
        private readonly int _payroll_month;
        private readonly int _location;

        public PayrollMustersForm5Pdf(Context context, int company_id, int payroll_month, int location)
        {
            _context = context;
            _company_id = company_id;
            _payroll_month = payroll_month;
            _location = location;
        }

        public class InputRows
        {
            public int empID { get; set; }
            public string EmpCode { get; set; }
            public string card_number { get; set; }
            public string employee_first_name { get; set; }
            public string attendancday { get; set; }
            public string attendancstatus { get; set; }
            public string department_name { get; set; }
            public int no_of_days_p { get; set; }
            public string father_name { get; set; }
        }
        public string TheRegisterOfFinesForm5()
        {
            bool allcompany_id = _company_id == 0 ? true : false;
            bool alllocation_id = _location == 0 ? true : false;

            int dt_year = Convert.ToInt32(_payroll_month.ToString().Substring(0, 4));
            int dt_month = Convert.ToInt32(_payroll_month.ToString().Substring(4, 2));
            string filter_dt = "";
            if (dt_month < 10)
            {
                filter_dt = dt_year + "0" + dt_month;
            }
            else
            {
                filter_dt = dt_year + "" + dt_month;
            }

            string p_year = _payroll_month.ToString().Substring(0, 4);
            string p_month = _payroll_month.ToString().Substring(4, 2);


            int days = DateTime.DaysInMonth(Convert.ToInt32(p_year), Convert.ToInt32(p_month));

            DateTime from_dt = Convert.ToDateTime((dt_year) + "-" + dt_month + "-" + 01);
            DateTime to_dt = Convert.ToDateTime(dt_year + "-" + dt_month + "-" + days);

            var data = (
                from d in _context.tbl_daily_attendance
                join em in _context.tbl_employee_master on d.emp_id equals em.employee_id
                join e in _context.tbl_user_master on em.employee_id equals e.employee_id
                join o in _context.tbl_emp_officaial_sec on d.emp_id equals o.employee_id
                join s in _context.tbl_shift_details on d.shift_id equals s.shift_id

                where
                e.default_company_id == (allcompany_id ? e.default_company_id : _company_id) &&
                o.location_id == (alllocation_id ? o.location_id : _location) &&
                d.attendance_dt >= from_dt && d.attendance_dt <= to_dt &&
                em.is_active == 1 && e.is_active == 1 && o.is_deleted == 0 && s.is_deleted == 0 &&
                (d.day_status == 1 || d.day_status == 2 || d.day_status == 3 || d.day_status == 4 || d.day_status == 5 || d.day_status == 6 || d.day_status == 0)
                select new
                {
                    e.employee_id,
                    o.card_number,
                    o.employee_first_name,
                    o.employee_middle_name,
                    o.employee_last_name,
                    d.shift_in_time,
                    d.shift_out_time,
                    d.in_time,
                    d.out_time,
                    d.attendance_dt,
                    d.day_status,
                    d.is_weekly_off,
                    d.is_holiday,
                    d.working_hour_done,
                    d.working_minute_done,
                    em.emp_code,
                    s.shift_name,
                    o.tbl_department_master.department_name,
                    father_name = _context.tbl_emp_family_sec.Where(a => a.employee_id == o.employee_id && a.is_deleted == 0 && a.relation == "Father").Select(a => a.name_as_per_aadhar_card).FirstOrDefault()
                }).ToList();

            //----------set payroll calender data--------------------------

            List<InputRows> list = data.Select(p => new InputRows
            {
                empID = p.employee_id ?? 0,
                EmpCode = p.emp_code,
                card_number = p.card_number,
                employee_first_name = p.employee_first_name,
                attendancday = Convert.ToString(p.attendance_dt.Day),
                attendancstatus = p.day_status == 1 ? "<span style='color:#8b8be8'>P</span>" : p.is_weekly_off == 1 ? "<span style='color:#b3aeae'>W/O</span>" : p.is_holiday == 1 ? "<span style='color:#9dea27'>H</span>" : p.day_status == 2 ? "<span style='color:#ef925e'>A</span>" : p.day_status == 3 ? "<span style='color:#e46ee4'>L</span>" : p.day_status == 4 ? "<span style='color:#ef925e'>HP/HA</span>" : p.day_status == 5 ? "<span style='color:#e46ee4'>HP/HL</span>" : p.day_status == 6 ? "<span style='color:#ef925e'>HL/HA</span>" : "<span style='color:red'>A</span>",
                department_name = p.department_name,
                no_of_days_p = _context.tbl_daily_attendance.Where(a => a.emp_id == p.employee_id && a.day_status == 1 && a.attendance_dt >= from_dt && a.attendance_dt <= to_dt).ToList().Count,
                father_name = p.father_name
            }).ToList();

            var result = list.Select(x => new
            {
                //convert cols to list of rows
                RowData = new List<Tuple<int, string, string, string, string, string, string>>()
                        {
                        Tuple.Create(x.empID ,x.attendancday, x.card_number, x.employee_first_name,x.attendancstatus, x.EmpCode, x.department_name),
                    }
            })
                //get one result list
                .SelectMany(x => x.RowData)
                //group data by year
                .GroupBy(x => x.Item1)
                //finally get pivoted data
                .Select((grp, counter) => new
                {
                    card_number = counter + 1,
                    employee_code = list.FirstOrDefault(a => a.empID == grp.Key).EmpCode,
                    employee_first_name = list.FirstOrDefault(a => a.empID == grp.Key).employee_first_name,
                    department_name = list.FirstOrDefault(a => a.empID == grp.Key).department_name,
                    no_of_days_p = list.FirstOrDefault(a => a.empID == grp.Key).no_of_days_p,
                    remarks = "",
                    father_name = list.FirstOrDefault(a => a.empID == grp.Key).father_name,
                    day1 = grp.Where(y => y.Item2 == "1").Select(y => y.Item5).FirstOrDefault(),
                    day2 = grp.Where(y => y.Item2 == "2").Select(y => y.Item5).FirstOrDefault(),
                    day3 = grp.Where(y => y.Item2 == "3").Select(y => y.Item5).FirstOrDefault(),
                    day4 = grp.Where(y => y.Item2 == "4").Select(y => y.Item5).FirstOrDefault(),
                    day5 = grp.Where(y => y.Item2 == "5").Select(y => y.Item5).FirstOrDefault(),
                    day6 = grp.Where(y => y.Item2 == "6").Select(y => y.Item5).FirstOrDefault(),
                    day7 = grp.Where(y => y.Item2 == "7").Select(y => y.Item5).FirstOrDefault(),
                    day8 = grp.Where(y => y.Item2 == "8").Select(y => y.Item5).FirstOrDefault(),
                    day9 = grp.Where(y => y.Item2 == "9").Select(y => y.Item5).FirstOrDefault(),
                    day10 = grp.Where(y => y.Item2 == "10").Select(y => y.Item5).FirstOrDefault(),
                    day11 = grp.Where(y => y.Item2 == "11").Select(y => y.Item5).FirstOrDefault(),
                    day12 = grp.Where(y => y.Item2 == "12").Select(y => y.Item5).FirstOrDefault(),
                    day13 = grp.Where(y => y.Item2 == "13").Select(y => y.Item5).FirstOrDefault(),
                    day14 = grp.Where(y => y.Item2 == "14").Select(y => y.Item5).FirstOrDefault(),
                    day15 = grp.Where(y => y.Item2 == "15").Select(y => y.Item5).FirstOrDefault(),
                    day16 = grp.Where(y => y.Item2 == "16").Select(y => y.Item5).FirstOrDefault(),
                    day17 = grp.Where(y => y.Item2 == "17").Select(y => y.Item5).FirstOrDefault(),
                    day18 = grp.Where(y => y.Item2 == "18").Select(y => y.Item5).FirstOrDefault(),
                    day19 = grp.Where(y => y.Item2 == "19").Select(y => y.Item5).FirstOrDefault(),
                    day20 = grp.Where(y => y.Item2 == "20").Select(y => y.Item5).FirstOrDefault(),
                    day21 = grp.Where(y => y.Item2 == "21").Select(y => y.Item5).FirstOrDefault(),
                    day22 = grp.Where(y => y.Item2 == "22").Select(y => y.Item5).FirstOrDefault(),
                    day23 = grp.Where(y => y.Item2 == "23").Select(y => y.Item5).FirstOrDefault(),
                    day24 = grp.Where(y => y.Item2 == "24").Select(y => y.Item5).FirstOrDefault(),
                    day25 = grp.Where(y => y.Item2 == "25").Select(y => y.Item5).FirstOrDefault(),
                    day26 = grp.Where(y => y.Item2 == "26").Select(y => y.Item5).FirstOrDefault(),
                    day27 = grp.Where(y => y.Item2 == "27").Select(y => y.Item5).FirstOrDefault(),
                    day28 = grp.Where(y => y.Item2 == "28").Select(y => y.Item5).FirstOrDefault(),
                    day29 = grp.Where(y => y.Item2 == "29").Select(y => y.Item5).FirstOrDefault(),
                    day30 = grp.Where(y => y.Item2 == "30").Select(y => y.Item5).FirstOrDefault(),
                    day31 = grp.Where(y => y.Item2 == "31").Select(y => y.Item5).FirstOrDefault(),
                }).ToList();


            List<object> column = new List<object>();
            column.Add("Sr. No.");
            column.Add("Code");
            column.Add("Employee Name");
            column.Add("Father's Name");
            column.Add("Nature Of Work");

            for (int i = 1; i <= days; i++)
            {
                column.Add(
                    i);
            }


            column.Add("No. of Days P");
            column.Add("Remarks");



            var d2 = new { list = result, column };

            var sb = new StringBuilder();

            var company_name = _context.tbl_company_master.Where(a => a.is_active == 1 && a.company_id == _company_id).Select(a => new { a.company_name, a.address_line_one, a.address_line_two, a.city_id, a.state_id, a.pin_code }).FirstOrDefault();

            if (company_name != null)
            {
                var state_name = _context.tbl_state.Where(a => a.is_deleted == 0 && a.state_id == company_name.state_id).Select(a => a.name).FirstOrDefault();
                if (state_name != null)
                {
                    var city_name = _context.tbl_city.Where(a => a.is_deleted == 0 && a.city_id == company_name.city_id).Select(a => a.name).FirstOrDefault();
                }
            }

            string location_name = "";
            if (_location > 0)
            {
                location_name = _context.tbl_location_master.Where(a => a.is_active == 1 && a.location_id == _location).Select(a => a.location_name).FirstOrDefault();
            }
            else
            {
                location_name = "All";
            }

            string payroll_month_name_and_year = string.Format("{0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(_payroll_month.ToString().Substring(4, 2))), _payroll_month.ToString().Substring(0, 4));

            sb.Append(@"<!DOCTYPE html>
<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta charset='utf-8' />
    <title></title>
</head>
<body style='font-family: sans-serif;'>
    <div style='width: 1680px;height: 743px; margin: 0px 0px 0px 0px'>
        <div style='position: relative;height: 150px;border: 2px solid black;margin-top: 10px 0px 0px 0px;'>
            <h2 style='font-family: sans-serif; margin: 4px 0px;text-align: center;font-size: 22px;font-weight: 600;text-decoration: underline;'>Minimum Wages (Central) Rules</h2>
            <label style='top: 35px;font-weight: 600;font-size: 18px; margin: 0px 0px 0px 4px;position: absolute;'>Name of Establishment:</label>
            <p style='position: absolute;top: 21px;left: 320px;font-size: 18px;font-weight: 600;'>" + (company_name != null ? company_name.company_name : "All Company Data") + @"</p>
            
            <label style='position: absolute;top: 53px;left: 0;font-family: sans-serif;font-size: 18px;font-weight: 600; margin: 10px 0px 0px 4px;'>Place:</label>
            <p style='position: absolute;top: 42px;left: 320px;font-size: 18px;font-weight: 600;margin: 25px 0px 0px 0px;'>" + location_name + @"</p>

            <label style='position: absolute;top: 85px;left: 0;font-family: sans-serif;font-size: 18px;font-weight: 600; margin: 10px 0px 0px 4px;'>Month Year:</label>
            <p style='position: absolute;top: 70px;left: 320px;font-size: 18px;font-weight: 600;margin: 25px 0px 0px 0px;'>" + payroll_month_name_and_year + @"</p>

            <label style='position: absolute;right: 100px;font-weight: 600;font-size: 18px;top:37px;'>Form ‐ V</label>
            <label style='position: absolute;right: 67px;top: 57px;font-weight: 600;font-size: 18px;'>Muster Roll 26(5)</label>
        </div>
        <table style='margin:  0px 0px 0px 0px;;border-collapse: collapse;width:100%;'>
            <thead>
                <tr>");
            // for columns name


            foreach (var columnss in column)
            {
                sb.AppendFormat(@"<th style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + columnss + "</th>");
            }

            sb.AppendFormat(@"</tr>
                    </th>
                </tr>
            </thead>
            <tbody>");

            for (int j = 0; j < result.Count; j++)
            {
                sb.AppendFormat(@"<tr>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + result[j].card_number + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + result[j].employee_code + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + result[j].employee_first_name + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;'>" + result[j].father_name + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 152px;line-height: 16px;'>" + result[j].department_name + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day1 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day2 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day3 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day4 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day5 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day6 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day7 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day8 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day9 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day10 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day11 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day12 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day13 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day14 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day15 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day16 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day17 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day18 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day19 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day20 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day21 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day22 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day23 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day24 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day25 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day26 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day27 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day28 + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day29 + @"</td>                   
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day30 + @"</td>");
                if (days == 31)
                {
                    sb.AppendFormat(@"<td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].day31 + @"</td>");
                }
                sb.AppendFormat(@"<td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 188px;line-height:17px;'>" + result[j].no_of_days_p + @"</td>
                    <td style='font-size: 15px;border: 1px solid black;padding: 2px 4px;width: 140px;line-height:17px;'>" + result[j].remarks + @"</td>
                </tr>");
            }
            sb.AppendFormat(@"</tbody>
        </table>
    </div>
</body>
</html>");
            string dfgsdfgs = sb.ToString();
            return sb.ToString();

        }
    }
}


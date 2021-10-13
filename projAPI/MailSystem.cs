using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MimeKit;
using projAPI.Model;
using projContext;

namespace projAPI
{
    public class MailSystem
    {
        private Context _context;
        private IConfiguration _config;
        public MailSystem(string connectionstring, IConfiguration config)
        {
            _context = new Context(); //context;
            _config = config;
        }
        // public static string AuthenticateId = "noreply@sakshemit.com"; // main email.
        //public static string AuthenticatePassword = "glaze@1234";
        //public static string AuthenticateId = "ourhrms@sakshemit.com"; // main email. 
        //public static string AuthenticatePassword = "xsqjskhgkweielng"; //"glaze@123";  // old
        //public static string AuthenticateId = "no@com.com";
        //public static string AuthenticatePassword = "gl";

        static MailData obj_mail = new MailData();


        //Mauil Sending funcation
        public bool SendMail(MailData obj_mail_data)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("OUR HRMS", _config["email_id_for_send_email"]);
                message.From.Add(from);


                for (int Index = 0; Index < obj_mail_data.MailSendTo.Count; Index++)
                {
                    //MailboxAddress to = new MailboxAddress("noreply", obj_mail_data.MailSendTo[Index]);
                    MailboxAddress to = new MailboxAddress(obj_mail_data.MailSendTo[Index]);
                    message.To.Add(to);
                    obj_mail_data.MailSendTo[Index].Remove(Index);
                }

                message.Subject = obj_mail_data.MailSubject;


                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = obj_mail_data.HtmlBody;

                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();

                int port = Convert.ToInt32(_config["email_id_Port"]);
                string host = _config["email_id_Host"];
                //client.Connect("smtp.gmail.com", 465, true);
                client.Connect(host, port, true);
                client.Authenticate(_config["email_id_for_send_email"], _config["email_id_password_for_send_email"]);

                client.Send(message);
                client.Disconnect(true);
                client.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;

            }

        }

        //Get managers
        public void SetManasgerData(int employee_id)
        {
            int m1_eid = 0, m2_eid = 0, m3_eid = 0;
            int[] m_ids = new int[] { m1_eid, m2_eid, m3_eid };


            obj_mail.MailSendTo.Clear();

            var get_manager_data = _context.tbl_emp_manager.OrderByDescending(a => a.emp_mgr_id).Where(a => a.employee_id == employee_id && a.is_deleted == 0).Select(a => new { a.m_one_id, a.m_two_id, a.m_three_id, a.notify_manager_1, a.notify_manager_2, a.notify_manager_3 }).ToList();
            if (get_manager_data.Count > 0)
            {
                if (get_manager_data[0].notify_manager_1 == 1)
                {
                    m1_eid = Convert.ToInt32(get_manager_data[0].m_one_id);
                }
                if (get_manager_data[0].notify_manager_2 == 1)
                {
                    m2_eid = Convert.ToInt32(get_manager_data[0].m_two_id);
                }
                if (get_manager_data[0].notify_manager_3 == 1)
                {
                    m3_eid = Convert.ToInt32(get_manager_data[0].m_three_id);
                }

                m_ids[0] = m1_eid;
                m_ids[1] = m2_eid;
                m_ids[2] = m3_eid;
            }


            var manager_data = _context.tbl_emp_officaial_sec.Where(a => a.is_deleted == 0 && m_ids.Contains(Convert.ToInt32(a.employee_id))).Select(a => new { a.official_email_id, a.employee_first_name }).ToList();
            for (int Index = 0; Index < manager_data.Count; Index++)
            {
                obj_mail.MailSendTo.Add(manager_data[Index].official_email_id);
            }
        }

        //For outdoor application requeast 
        public void OutdoorApplicationRequestMail(int employee_id, DateTime from_date, DateTime mannual_in_time, DateTime mannual_out_time, DateTime systemin_time, DateTime systemout_time, string remark)
        {


            SetManasgerData(employee_id);


            var emp_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == employee_id && a.is_deleted == 0).Select(a => new { req_email = a.official_email_id, emp_name = a.employee_first_name, emp_code = a.tbl_employee_id_details.emp_code }).FirstOrDefault();

            obj_mail.MailSendTo.Add(emp_data.req_email);
            var sb = new StringBuilder();

            sb.Append(string.Format(@"<html>
                      <body>                  
                        <p> Outdoor request raised by " + (emp_data != null ? emp_data.emp_name : "") + @" </p>
                     <table border='1' cellspacing='0' cellpadding='0' width='600' style='width: 6.25in; border - collapse:collapse; border: none'>
                       <tbody>
                            <tr><td style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Employee Code<u></u><u></u></span></b></p></td><td style = 'border:solid #666666 1.0pt;border-left:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {0} </span><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Employee Name<u></u><u></u></span></b></p></td><td style = 'border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'> {1} </ span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Date<u></u><u></u></span></b></p></td><td style = 'border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {2} <u></u><u></ u></span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Mannual In Time<u></u><u></u></span></b></p></td><td style = 'border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {3} <u></u><u></ u></span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Mannual Out Time<u> </u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {4}<u> </u><u></u></ span></p></td></tr>
<tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Shift In Time<u> </u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {5}<u> </u><u></u></ span></p></td></tr>
<tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Shift Out Time<u> </u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {6}<u> </u><u></u></ span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Requester Reason<u> </u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {7}<u></u><u></u></span></p></td></tr>
                            <tr>
                                <td style = 'border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Status<u> </u><u></u></span></b></p></td>
                                <td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
                                 <p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> Pending<u> </u><u></u></span></p>
                                </td>
                            </tr>
                      </tbody>
            </table>
            </body>
            </html>", emp_data.emp_code, emp_data.emp_name, from_date.Date.ToString("dd-MM-yyyy"), mannual_in_time.ToString("hh:mm tt"), mannual_out_time.ToString("hh:mm tt"), systemin_time.ToString("hh:mm tt"), systemout_time.ToString("hh:mm tt"), remark));

            obj_mail.HtmlBody = sb.ToString();
            obj_mail.MailSubject = "Outdoor Application Request";
            SendMail(obj_mail);
        }

        // For Leave Applicable request
        public void LeaveApplicationRequestMail(int employee_id, DateTime from_date, DateTime to_date, int LeaveType, int Duration, int day_part, string remark)
        {


            SetManasgerData(employee_id);


            var emp_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == employee_id && a.is_deleted == 0).Select(a => new { req_email = a.official_email_id, emp_name = a.employee_first_name, emp_code = a.tbl_employee_id_details.emp_code }).FirstOrDefault();

            obj_mail.MailSendTo.Add(emp_data != null ? emp_data.req_email : "");

            var LeaveTypeName = _context.tbl_leave_type.Where(a => a.leave_type_id == LeaveType && a.is_active == 1).Select(a => a.leave_type_name).FirstOrDefault();

            string DurationName = "";
            string daypart = "";

            if (Duration == 1)
            {
                DurationName = "Full Day";
            }
            else
            {
                DurationName = "Half Day";

                if (day_part == 1)
                {
                    daypart = " - First Half";
                }
                else
                {
                    daypart = " - Second Half";
                }
            }




            var sb = new StringBuilder();

            sb.Append(string.Format(@"<html>
                      <body>                  
                        <p> Leave request raised by " + (emp_data != null ? emp_data.emp_name : "") + @". </p><table border='1' cellspacing='0' cellpadding='0' width='600' style='width:6.25in;border-collapse:collapse;border:none'><tbody>
        <tr><td colspan='2' style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Leave Application Details<u></u><u></u></span></b></p></td></tr>
        <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Code<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{0}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
        <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Name<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{1}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
        <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Leave Type<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{2}<u></u><u></u></span></p></td></tr>
        <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>From Date<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{3}<u></u><u></u></span></p></td></tr>
        <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>To Date<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{4}<u></u><u></u></span></p></td></tr>
        <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Duration<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{5}{6}<u></u><u></u></span></p></td></tr>
        <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Requester Reason<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u>{7}</u><u></u></span></p></td></tr>
        <tr>
            <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Status<u></u><u></u></span></b></p></td>
            <td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
                <p class='MsoNormal'>
                    <span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>Pending<u></u><u></u></span>
                </p>
            </td>
        </tr>
    </tbody>
</table></body></html>", emp_data.emp_code, emp_data.emp_name, LeaveTypeName, from_date.Date.ToString("dd-MM-yyyy"), to_date.Date.ToString("dd-MM-yyyy"), DurationName, daypart, remark));

            obj_mail.HtmlBody = sb.ToString();
            obj_mail.MailSubject = "Leave Application Request";
            SendMail(obj_mail);
        }

        //Attendance Application Request
        public void AttendanceApplicationRequestMail(int employee_id, DateTime RegularizeDate, DateTime manual_in_time, DateTime manual_out_time, string requester_remarks)
        {
            try
            {


                SetManasgerData(employee_id);

                var get_attendance_data = _context.tbl_daily_attendance.Where(a => a.emp_id == employee_id && a.attendance_dt == RegularizeDate).Select(a => new { a.in_time, a.out_time, a.day_status, a.working_hour_done, a.working_minute_done }).FirstOrDefault();
                if (get_attendance_data != null)
                {
                    string DayStatus = "";
                    if (get_attendance_data.day_status == 1)
                    {
                        DayStatus = "Full day Present";
                    }
                    else if (get_attendance_data.day_status == 2)
                    {
                        DayStatus = DayStatus = "Full day Absent";
                    }
                    else if (get_attendance_data.day_status == 3)
                    {
                        DayStatus = DayStatus = "Full Day Leave";
                    }
                    else if (get_attendance_data.day_status == 4)
                    {
                        DayStatus = DayStatus = "Half Day";  // "Half Day Present - Half Day Absent";
                    }
                    else if (get_attendance_data.day_status == 5)
                    {
                        DayStatus = DayStatus = "Half Day"; //"Half Day Present - Half Day Leave";
                    }
                    else if (get_attendance_data.day_status == 6)
                    {
                        DayStatus = DayStatus = "Half Day"; //"Half Day Leave - Half Day Absent";
                    }
                    else
                    {
                        DayStatus = DayStatus = "N/A";
                    }

                    var emp_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == employee_id && a.is_deleted == 0).Select(a => new { req_email = a.official_email_id, emp_name = a.employee_first_name, emp_code = a.tbl_employee_id_details.emp_code }).FirstOrDefault();


                    obj_mail.MailSendTo.Add(emp_data.req_email);

                    string WorkingHours = get_attendance_data.working_hour_done + ":" + get_attendance_data.working_minute_done;

                    string in_time = get_attendance_data.in_time.ToString("hh:mm tt");
                    string out_time = get_attendance_data.out_time.ToString("hh:mm tt");
                    if (in_time == "12:00 AM")
                    {
                        in_time = "00:00";
                    }
                    if (out_time == "12:00 AM")
                    {
                        out_time = "00:00";
                    }
                    //string in_time_ = get_attendance_data.in_time.ToString().Replace(@"/", string.Empty).Trim();
                    //string in_time = in_time_.Replace(@" ", string.Empty).Substring(8, 5);
                    //string out_time_ = get_attendance_data.out_time.ToString().Replace(@"/", string.Empty).Trim();
                    //string out_time = out_time_.Replace(@" ", string.Empty).Substring(8, 5);

                    //string m_in_time_ = manual_in_time.ToString().Replace(@"/", string.Empty).Trim();
                    //string m_in_time = m_in_time_.Replace(@" ", string.Empty).Substring(8, 5);

                    //string m_out_time_ = manual_out_time.ToString().Replace(@"/", string.Empty).Trim();
                    //string m_out_time = m_out_time_.Replace(@" ", string.Empty).Substring(8, 5);

                    var sb = new StringBuilder();

                    sb.Append(string.Format(@"<html>
                    <body> <p> Attendance request raised by " + (emp_data != null ? emp_data.emp_name : "") + @". </p>
                        <table border='1' cellspacing='0' cellpadding='0' width='600' style='width:6.25in;border-collapse:collapse;border:none'>
                            <tbody>
                                <tr><td style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Code<u></u><u></u></span></b></p></td><td style='border:solid #666666 1.0pt;border-left:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{0}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Name<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{1}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Regularized Date<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{2}<u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>System In Time<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{3}<u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>System Out Time<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{4}<u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Full Day Status<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{5}<u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Working Hours<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{6}<u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Manual In Time<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{7}<u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Manual Out Time<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{8}<u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Requester Remarks<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{9}<u></u><u></u></span></p></td></tr>
                                <tr>
                                    <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Status<u></u><u></u></span></b></p></td>
                                    <td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
                                        <p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>Pending<u></u><u></u></span></p>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </body>
                    </html>", emp_data.emp_code, emp_data.emp_name, RegularizeDate.Date.ToString("dd-MM-yyyy"), in_time, out_time, DayStatus, WorkingHours, manual_in_time.ToString("hh:mm tt"), manual_out_time.ToString("hh:mm tt"), requester_remarks));


                    obj_mail.HtmlBody = sb.ToString();
                    obj_mail.MailSubject = "Attendance Application Request";
                    SendMail(obj_mail);
                }
            }
            catch (Exception ex)
            {

            }
        }

        //Compoff Application Request
        public void CompoffApplicationRequestMail(int employee_id, DateTime against_date, DateTime compoff_date, string requester_remarks)
        {
            try
            {


                SetManasgerData(employee_id);

                var emp_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == employee_id && a.is_deleted == 0).Select(a => new
                {
                    requster_mail = a.official_email_id,
                    emp_name = string.Format("{0} {1} {2}", a.employee_first_name, a.employee_middle_name, a.employee_last_name),
                    emp_code = a.tbl_employee_id_details.emp_code
                }).FirstOrDefault();
                obj_mail.MailSendTo.Add(emp_data.requster_mail);
                var sb = new StringBuilder();

                sb.Append(string.Format(@"<html>
                    <body> <p> CompOff request raised by " + (emp_data != null ? emp_data.emp_name : "") + @". </p>
                            <table border='1' cellspacing='0' cellpadding='0' width='600' style='width:6.25in;border-collapse:collapse;border:none'>
                                <tbody>
                                    <tr><td style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Code<u></u><u></u></span></b></p></td><td style='border:solid #666666 1.0pt;border-left:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{0}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Name<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{1} <u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Against Date<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{2}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Compoff Date<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{3}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Requester Remarks<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{4}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Status<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>Pending<u></u><u></u></span></p></td></tr>
                                </tbody>
                            </table>
                    </body>
                    </html>", emp_data.emp_code, emp_data.emp_name, against_date.Date.ToString("dd-MM-yyyy"), compoff_date.Date.ToString("dd-MM-yyyy"), requester_remarks));


                obj_mail.HtmlBody = sb.ToString();
                obj_mail.MailSubject = "CompOff Application Request";
                SendMail(obj_mail);

            }
            catch (Exception ex)
            {

            }
        }

        //For outdoor application Approval 
        public void OutdoorApplicationApprovalMail(List<ApplicatonMailDetails> objmaillst)
        {


            for (int Index = 0; Index < objmaillst.Count; Index++)
            {

                var emp_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == objmaillst[Index].emp_id && a.is_deleted == 0).Select(a => new
                {
                    emp_name = string.Format("{0} {1} {2}", a.employee_first_name, a.employee_middle_name, a.employee_last_name),
                    emp_code = a.tbl_employee_id_details.emp_code,
                    a.official_email_id
                }).FirstOrDefault();

                var approver_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == objmaillst[Index].a_e_id && a.is_deleted == 0).Select(a => new
                {
                    emp_name = string.Format("{0} {1} {2}", a.employee_first_name, a.employee_middle_name, a.employee_last_name),
                    emp_code = a.tbl_employee_id_details.emp_code,
                    a.official_email_id
                }).FirstOrDefault();

                obj_mail.MailSendTo.Clear();
                SetManasgerData(objmaillst[Index].emp_id);
                obj_mail.MailSendTo.Add(emp_data.official_email_id);


                string status_ = "";
                string a_taken = "Approver Name";
                if (objmaillst[Index].is_approve == 1)
                {
                    status_ = "Approved";
                }
                else if (objmaillst[Index].is_approve == 2)
                {
                    status_ = "Rejected";
                }

                var sb = new StringBuilder();


                sb.Append(string.Format(@"<html>
                      <body>                  
                     <table border='1' cellspacing='0' cellpadding='0' width='600' style='width: 6.25in; border - collapse:collapse; border: none'>
                       <tbody>
                            <tr><td style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Employee Code<u></u><u></u></span></b></p></td><td style = 'border:solid #666666 1.0pt;border-left:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {0} </span><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Employee Name<u></u><u></u></span></b></p></td><td style = 'border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'> {1} </ span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Date<u></u><u></u></span></b></p></td><td style = 'border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {2} <u></u><u></ u></span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Mannual In Time<u></u><u></u></span></b></p></td><td style = 'border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {3} <u></u><u></ u></span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Mannual Out Time<u> </u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {4}<u> </u><u></u></ span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Shift In Time<u></u><u></u></span></b></p></td><td style = 'border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {5} <u></u><u></u></ span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Shift Out Time<u></u><u></u></span></b></p></td><td style = 'border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {6} <u></u><u></u></ span></p></td></tr>
                           <tr> <td style = 'border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Status<u> </u><u></u></span></b></p></td> <td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'> <p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {7}<u> </u><u></u></span></p> </td> </tr>
                            <tr> <td style = 'border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> {8}<u> </u><u></u></span></b></p></td> <td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'> <p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {9}<u> </u><u></u></span></p> </td> </tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Approver Remarks<u> </u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {10}<u></u><u></u></span></p></td></tr>
                             </tbody>
                               </table>
                               </body>
                               </html>", emp_data.emp_code, emp_data.emp_name, objmaillst[Index].from_date.Date.ToString("dd-MM-yyyy"), objmaillst[Index].mannual_in_time.ToString("hh:mm tt"), objmaillst[Index].mannual_out_time.ToString("hh:mm tt"), objmaillst[Index].system_in_time.ToString("hh:mm tt"), objmaillst[Index].system_out_time.ToString("hh:mm tt"), status_, a_taken, approver_data.emp_name, objmaillst[Index].approver_remarks));

                obj_mail.HtmlBody = sb.ToString();
                obj_mail.MailSubject = "Outdoor Application Request " + status_;
                SendMail(obj_mail);

            }
        }


        //For Leave Applicable Approval
        public void LeaveApplicationApprovalMail(List<ApplicatonMailDetails> mail_lst)
        {
            var leave_type = _context.tbl_leave_type.ToList();

            for (int i = 0; i < mail_lst.Count; i++)
            {
                var emp_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == mail_lst[i].emp_id && a.is_deleted == 0).Select(a => new
                {

                    emp_name = string.Format("{0} {1} {2}", a.employee_first_name, a.employee_middle_name, a.employee_last_name),
                    emp_code = a.tbl_employee_id_details.emp_code,
                    a.official_email_id
                }).FirstOrDefault();

                var approver_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == mail_lst[i].a_e_id && a.is_deleted == 0).Select(a => new
                {

                    emp_name = string.Format("{0} {1} {2}", a.employee_first_name, a.employee_middle_name, a.employee_last_name),
                    emp_code = a.tbl_employee_id_details.emp_code,
                    a.official_email_id
                }).FirstOrDefault();

                obj_mail.MailSendTo.Clear();
                obj_mail.MailSendTo.Add(emp_data.official_email_id);

                string status_ = "";
                string a_taken = "Approver Name";
                if (mail_lst[i].is_approve == 1)
                {
                    status_ = "Approved";
                }
                else if (mail_lst[i].is_approve == 2)
                {
                    status_ = "Rejected";
                }

                string leave_typename = leave_type.Where(x => x.leave_type_id == mail_lst[i].leave_type_id).FirstOrDefault() != null ? leave_type.FirstOrDefault(x => x.leave_type_id == mail_lst[i].leave_type_id).leave_type_name : "";

                var leave_details = _context.tbl_leave_request.Where(x => x.leave_request_id == mail_lst[i].req_id).FirstOrDefault();
                string DurationName = "";
                string daypart = "";

                if (leave_details.leave_applicable_for == 1)
                {
                    DurationName = "Full Day";
                }
                else
                {
                    DurationName = "Half Day";

                    if (leave_details.day_part == 1)
                    {
                        daypart = " - First Half";
                    }
                    else
                    {
                        daypart = " - Second Half";
                    }
                }
                string durn = DurationName + daypart;
                var sb = new StringBuilder();

                sb.Append(string.Format(@"<html>
                      <body>                                          
                     <table border='1' cellspacing='0' cellpadding='0' width='600' style='width: 6.25in; border - collapse:collapse; border: none'>
                       <tbody>
                             <tr><td colspan='2' style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Leave Application Details<u></u><u></u></span></b></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Code<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{0}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Name<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{1}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Leave Type<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{2}<u></u><u></u></span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>From Date<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{3}<u></u><u></u></span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>To Date<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{4}<u></u><u></u></span></p></td></tr>
                            <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Duration<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{5}<u></u><u></u></span></p></td></tr>
                            <tr> <td style = 'border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> Status<u> </u><u></u></span></b></p></td> <td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'> <p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {6}<u> </u><u></u></span></p></td></tr>
                            <tr> <td style = 'border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style = 'font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'> {7}<u> </u><u></u></span></b></p></td> <td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'> <p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'> {8}<u> </u><u></u></span></p> </td> </tr>
                         <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Approver Remarks<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u>{9}</u><u></u></span></p></td></tr>
                      </tbody> 
                        </table>
                        </body>
                        </html>", emp_data.emp_code, emp_data.emp_name, leave_typename, mail_lst[i].from_date.Date.ToString("dd-MM-yyyy"), mail_lst[i].to_date.Date.ToString("dd-MM-yyyy"), durn, status_, a_taken, approver_data.emp_name, mail_lst[i].approver_remarks));//emp_data.emp_code, emp_data.emp_name, leave_typename, mail_lst[i].from_date.Date.ToString("dd-MM-yyyy"), mail_lst[i].to_date.Date.ToString("dd-MM-yyyy"), Convert.ToInt32((Convert.ToDateTime(mail_lst[i].to_date) - Convert.ToDateTime(mail_lst[i].from_date)).TotalDays) + 1, status_, a_taken, approver_data.emp_name, mail_lst[i].approver_remarks));

                obj_mail.HtmlBody = sb.ToString();
                obj_mail.MailSubject = "Leave Application Request - " + status_;
                SendMail(obj_mail);
                obj_mail.MailSendTo.Clear();

            }

        }

        //Attendance Application Approval

        public void AttendanceApplicationApprovalMail(List<ApplicatonMailDetails> maillst_)
        {
            try
            {
                for (int i = 0; i < maillst_.Count; i++)
                {
                    var get_attendance_data = _context.tbl_daily_attendance.Where(a => a.emp_id == maillst_[i].emp_id && a.attendance_dt.Date == maillst_[i].from_date).Select(a => new
                    {
                        a.in_time,
                        a.out_time,
                        a.day_status,
                        a.working_hour_done,
                        a.working_minute_done
                    }).FirstOrDefault();


                    if (get_attendance_data != null)
                    {
                        string DayStatus = "";
                        if (get_attendance_data.day_status == 1)
                        {
                            DayStatus = "Full day Present";
                        }
                        else if (get_attendance_data.day_status == 2)
                        {
                            DayStatus = DayStatus = "Full day Absent";
                        }
                        else if (get_attendance_data.day_status == 3)
                        {
                            DayStatus = DayStatus = "Full Day Leave";
                        }
                        else if (get_attendance_data.day_status == 4)
                        {
                            DayStatus = DayStatus = "Half Day"; //"Half Day Present - Half Day Absent";
                        }
                        else if (get_attendance_data.day_status == 5)
                        {
                            DayStatus = DayStatus = "Half Day"; //"Half Day Present - Half Day Leave";
                        }
                        else if (get_attendance_data.day_status == 6)
                        {
                            DayStatus = DayStatus = "Half Day"; //"Half Day Leave - Half Day Absent";
                        }
                        else
                        {
                            DayStatus = DayStatus = "N/A";
                        }


                        string status_ = "";
                        string a_taken = "Approver Name";
                        if (maillst_[i].is_approve == 1)
                        {
                            status_ = "Approved";
                        }
                        else if (maillst_[i].is_approve == 2)
                        {
                            status_ = "Rejected";
                        }

                        SetManasgerData(maillst_[i].emp_id);
                        var emp_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == maillst_[i].emp_id && a.is_deleted == 0).Select(a => new
                        {
                            emp_name = string.Format("{0} {1} {2}", a.employee_first_name, a.employee_middle_name, a.employee_last_name),
                            emp_code = a.tbl_employee_id_details.emp_code,
                            a.official_email_id
                        }).FirstOrDefault();

                        var approver_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == maillst_[i].a_e_id && a.is_deleted == 0).Select(a => new
                        {
                            emp_name = string.Format("{0} {1} {2}", a.employee_first_name, a.employee_middle_name, a.employee_last_name),
                            emp_code = a.tbl_employee_id_details.emp_code,
                            a.official_email_id
                        }).FirstOrDefault();


                        obj_mail.MailSendTo.Add(emp_data.official_email_id);

                        string WorkingHours = get_attendance_data.working_hour_done + ":" + get_attendance_data.working_minute_done;

                        string in_time = get_attendance_data.in_time.ToString("hh:ss tt");
                        string out_time = get_attendance_data.out_time.ToString("hh:ss tt");

                        //string in_time_ = get_attendance_data.in_time.ToString().Replace(@"/", string.Empty).Trim();
                        ////string in_time = in_time_.Replace(@" ", string.Empty).Substring(8, 5);
                        //string in_time = in_time_.Substring(6, 6).Trim();
                        //string out_time_ = get_attendance_data.out_time.ToString().Replace(@"/", string.Empty).Trim();
                        ////string out_time = out_time_.Replace(@" ", string.Empty).Substring(8, 5);
                        //string out_time = out_time_.Substring(6, 6).Trim();
                        //string m_in_time_ = maillst_[i].mannual_in_time.ToString().Replace(@"/", string.Empty).Trim();
                        ////string m_in_time = m_in_time_.Replace(@" ", string.Empty).Substring(8, 5);
                        //string m_in_time = m_in_time_.Substring(6, 6).Trim();
                        //string m_out_time_ = maillst_[i].mannual_out_time.ToString().Replace(@"/", string.Empty).Trim();
                        ////string m_out_time = m_out_time_.Replace(@" ", string.Empty).Substring(8, 5);
                        //string m_out_time = m_out_time_.Substring(6, 6).Trim();

                        var sb = new StringBuilder();

                        sb.Append(string.Format(@"<html>
                    <body>
                        <table border='1' cellspacing='0' cellpadding='0' width='600' style='width:6.25in;border-collapse:collapse;border:none'>
                            <tbody>
                                <tr><td style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Code<u></u><u></u></span></b></p></td><td style='border:solid #666666 1.0pt;border-left:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{0}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Name<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{1}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Regularized Date<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{2}<u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>System In Time<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{3}<u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>System Out Time<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{4}<u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Full Day Status<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{5}<u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Working Hours<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{6}<u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Manual In Time<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{7}<u></u><u></u></span></p></td></tr>
                                <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Manual Out Time<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{8}<u></u><u></u></span></p></td></tr>
                                  <tr> <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Status<u></u><u></u></span></b></p></td>
                                    <td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'> <p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{9}<u></u><u></u></span></p> </td> </tr>
                                <tr> <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>{10}<u></u><u></u></span></b></p></td> <td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'> <p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{11}<u></u><u></u></span></p> </td> </tr>
                               <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Approver Remarks<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{12}<u></u><u></u></span></p></td></tr>
                            </tbody>
                        </table>
                    </body>
                    </html>", emp_data.emp_code, emp_data.emp_name, maillst_[i].from_date.Date.ToString("dd-MM-yyyy"), in_time, out_time, DayStatus, WorkingHours, maillst_[i].mannual_in_time.ToString("hh:ss tt"), maillst_[i].mannual_out_time.ToString("hh:ss tt"), status_, a_taken, approver_data.emp_name, maillst_[i].approver_remarks));


                        obj_mail.HtmlBody = sb.ToString();
                        obj_mail.MailSubject = "Attendance Application Request - " + status_;
                        SendMail(obj_mail);
                        obj_mail.MailSendTo.Clear();
                    }


                }




            }
            catch (Exception ex)
            {

            }
        }

        //Compoff Application Approval
        public void CompoffApplicationApprovalMail(List<ApplicatonMailDetails> mail_lst)
        {
            try
            {
                for (int i = 0; i < mail_lst.Count; i++)
                {
                    var emp_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == mail_lst[i].emp_id && a.is_deleted == 0).Select(a => new
                    {
                        emp_name = string.Format("{0} {1} {2}", a.employee_first_name, a.employee_middle_name, a.employee_last_name),
                        emp_code = a.tbl_employee_id_details.emp_code,
                        a.official_email_id
                    }).FirstOrDefault();

                    var approver_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == mail_lst[i].a_e_id && a.is_deleted == 0).Select(a => new
                    {
                        emp_name = string.Format("{0} {1} {2}", a.employee_first_name, a.employee_middle_name, a.employee_last_name),
                        emp_code = a.tbl_employee_id_details.emp_code,
                        a.official_email_id
                    }).FirstOrDefault();

                    obj_mail.MailSendTo.Clear();
                    obj_mail.MailSendTo.Add(emp_data.official_email_id);

                    string status_ = "";
                    string a_taken = "Approver Name";
                    if (mail_lst[i].is_approve == 1)
                    {
                        status_ = "Approved";
                    }
                    else if (mail_lst[i].is_approve == 2)
                    {
                        status_ = "Rejected";
                    }

                    var sb = new StringBuilder();

                    sb.Append(string.Format(@"<html>
                    <body> 
                            <table border='1' cellspacing='0' cellpadding='0' width='600' style='width:6.25in;border-collapse:collapse;border:none'>
                                <tbody>
                                    <tr><td style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Code<u></u><u></u></span></b></p></td><td style='border:solid #666666 1.0pt;border-left:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{0}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Name<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{1} <u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Against Date<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{2}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Compoff Date<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{3}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Status<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{4}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>{5}<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{6}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Approver Remarks<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{7}<u></u><u></u></span></p></td></tr>
                                </tbody>
                            </table>
                    </body>
                    </html>", emp_data.emp_code, emp_data.emp_name, mail_lst[i].from_date.Date.ToString("dd-MM-yyyy"), mail_lst[i].to_date.Date.ToString("dd-MM-yyyy"), status_, a_taken, approver_data.emp_name, mail_lst[i].approver_remarks));


                    obj_mail.HtmlBody = sb.ToString();
                    obj_mail.MailSubject = "CompOff Application Request - " + status_;
                    SendMail(obj_mail);
                }
            }
            catch (Exception ex)
            {

            }
        }

        //Forgot password
        public bool ForgotPasswordMail(string email_id, string user_password, string username)
        {
            try
            {
                obj_mail.MailSendTo.Clear();
                obj_mail.MailSendTo.Add(email_id);

                var sb = new StringBuilder();

                sb.Append(string.Format(@"<html>
                    <body> 
                            <table border='1' cellspacing='0' cellpadding='0' width='600' style='width:6.25in;border-collapse:collapse;border:none'>
                                <tbody>
                                    <tr><td style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Username<u></u><u></u></span></b></p></td><td style='border:solid #666666 1.0pt;border-left:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{0}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Password<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{1} <u></u><u></u></span></p></td></tr>
                                  </tbody>
                            </table>
                    </body>
                    </html>", username, user_password));


                obj_mail.HtmlBody = sb.ToString();
                obj_mail.MailSubject = "Login Credential";
                return SendMail(obj_mail);

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void LastDayAbsentNotification(List<MailDetails> objmail_det)
        {
            try
            {
                for (int Index = 0; Index < objmail_det.Count; Index++)
                {
                    obj_mail.MailSendTo.Clear();
                    obj_mail.MailSendTo.Add(objmail_det[Index].email_id);

                    var sb = new StringBuilder();

                    sb.Append(string.Format(@"<html>
                    <body> 
                            <p>
                                Dear <b> {0} {1} - ({2}) </b>,
                                <br/><br/>
                                This is to inform you that you have not updated the leave details with regards to your absenteeism On <b> {3} </b>. You are requested to do the necessary leave updations / regularisation as the case maybe so as to avoid deduction of salary.
                            </p>
                    <p> With Regards
                           <br/>     
                    HR Team </p>
                    </body>
                    </html>", objmail_det[Index].employee_first_name, objmail_det[Index].employee_last_name, objmail_det[Index].employee_code, objmail_det[Index].absent_date));


                    obj_mail.HtmlBody = sb.ToString();
                    obj_mail.MailSubject = "Reminder - Absent On " + objmail_det[Index].absent_date;
                    SendMail(obj_mail);
                }
            }
            catch (Exception ex)
            {

            }
        }


        //CompoffRaise Addition Application Request
        public void CompoffRaiseApplicationRequestMail(int employee_id, DateTime compoff_date, string requester_remarks)
        {
            try
            {


                SetManasgerData(employee_id);

                var emp_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == employee_id && a.is_deleted == 0).Select(a => new { req_email = a.official_email_id, emp_name = string.Format("{0} {1} {2}", a.employee_first_name, a.employee_middle_name, a.employee_last_name), emp_code = a.tbl_employee_id_details.emp_code }).FirstOrDefault();

                obj_mail.MailSendTo.Add(emp_data.req_email);
                var sb = new StringBuilder();



                sb.Append(string.Format(@"<html>
                    <body> <p> CompOff Credit Request raised by " + (emp_data != null ? emp_data.emp_name : "") + @". </p>
                            <table border='1' cellspacing='0' cellpadding='0' width='600' style='width:6.25in;border-collapse:collapse;border:none'>
                                <tbody>
                                    <tr><td style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Code<u></u><u></u></span></b></p></td><td style='border:solid #666666 1.0pt;border-left:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{0}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Name<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{1} <u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Compoff Date<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{2}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Reason<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{3}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Status<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>Pending<u></u><u></u></span></p></td></tr>
                                </tbody>
                            </table>
                    </body>
                    </html>", emp_data.emp_code, emp_data.emp_name, compoff_date.ToString().Substring(0, 10), requester_remarks));


                obj_mail.HtmlBody = sb.ToString();
                obj_mail.MailSubject = "CompOff Addition Request";
                SendMail(obj_mail);
                obj_mail.MailSendTo.Clear();

            }
            catch (Exception ex)
            {

            }
        }


        //Compoff Raise Approval
        public void CompoffRaiseApplicationApprovalMail(List<ApplicatonMailDetails> _maillst)
        {
            try
            {

                for (int i = 0; i < _maillst.Count; i++)
                {
                    var emp_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == _maillst[i].emp_id && a.is_deleted == 0).Select(a => new
                    {
                        emp_name = string.Format("{0} {1} {2}", a.employee_first_name, a.employee_middle_name, a.employee_last_name),
                        emp_code = a.tbl_employee_id_details.emp_code,
                        a.official_email_id
                    }).FirstOrDefault();

                    var approver_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == _maillst[i].a_e_id && a.is_deleted == 0).Select(a => new
                    {
                        emp_name = string.Format("{0} {1} {2}", a.employee_first_name, a.employee_middle_name, a.employee_last_name),
                        emp_code = a.tbl_employee_id_details.emp_code,
                        a.official_email_id
                    }).FirstOrDefault();

                    obj_mail.MailSendTo.Clear();
                    obj_mail.MailSendTo.Add(emp_data.official_email_id);

                    string status_ = "";
                    string a_taken = "Approver Name";
                    if (_maillst[i].is_approve == 1)
                    {
                        status_ = "Approved";
                    }
                    else if (_maillst[i].is_approve == 2)
                    {
                        status_ = "Rejected";
                    }


                    var sb = new StringBuilder();

                    sb.Append(string.Format(@"<html>
                    <body> 
                            <table border='1' cellspacing='0' cellpadding='0' width='600' style='width:6.25in;border-collapse:collapse;border:none'>
                                <tbody>
                                    <tr><td style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Code<u></u><u></u></span></b></p></td><td style='border:solid #666666 1.0pt;border-left:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{0}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Name<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{1} <u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Compoff Date<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{2}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Remarks<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{3}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Status<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{4}<u></u><u></u></span></p></td></tr>
                                    <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>{5}<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{6}<u></u><u></u></span></p></td></tr>
                                </tbody>
                            </table>
                    </body>
                    </html>", emp_data.emp_code, emp_data.emp_name, _maillst[i].from_date.ToString().Substring(0, 10), _maillst[i].approver_remarks, status_, a_taken, approver_data.emp_name));


                    obj_mail.HtmlBody = sb.ToString();
                    obj_mail.MailSubject = "CompOff Raise Request - " + status_;
                    SendMail(obj_mail);
                }





            }
            catch (Exception ex)
            {

            }
        }

        public void WeeklyAttendanceNotification(List<MailDetails> objmail_det)
        {
            try
            {

                for (int Index = 0; Index < objmail_det.Count; Index++)
                {
                    var get_single_employee_data = objmail_det.Where(a => a.employee_code == objmail_det[Index].employee_code).Select(a => new
                    {
                        employee_first_name = a.employee_first_name,
                        employee_last_name = a.employee_last_name,
                        email_id = a.email_id,
                        employee_code = a.employee_code,
                        absent_date = a.absent_date,
                        day_status = a.day_status
                    }).ToList();

                    obj_mail.MailSendTo.Clear();
                    obj_mail.MailSendTo.Add(objmail_det[Index].email_id);

                    var sb = new StringBuilder();

                    sb.Append(string.Format(@"<html>
                    <body> 
                            <p>
                                Dear <b>" + get_single_employee_data[Index].employee_first_name + " " + get_single_employee_data[Index].employee_last_name + @"</b>,
                                <br/><br/> 
<p>Here is your Weekly Attendance Status :-</p>
 <br/>
<table border='1' cellspacing='0' cellpadding='0' width='600' style='width:6.25in;border-collapse:collapse;border:none'>
<tr> <th>Date </th><th>Status </th> </tr>"));
                    for (int Index2 = 0; Index2 < get_single_employee_data.Count; Index2++)
                    {
                        sb.AppendFormat(@"                            
                          
<tr><td style='border:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>" + get_single_employee_data[Index2].absent_date + @"<u></u><u></u></span></p></td>
<td style='border:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>" + get_single_employee_data[Index2].day_status + @"<u></u><u></u></span></p></td></tr>                      
                            
                            ");
                    }
                    sb.AppendFormat(@"
</table>
                   <p> With Regards
                           <br/>     
                    HR Team </p>
                    </body>
                    </html>");

                    objmail_det.RemoveAll(a => a.employee_code == get_single_employee_data[Index].employee_code);

                    obj_mail.HtmlBody = sb.ToString();
                    obj_mail.MailSubject = "Weekly Attendance Status";
                    SendMail(obj_mail);
                }
            }
            catch (Exception ex)
            {

            }
        }



        public void AssetApprovalNotifaction(MailDetails objmail_det)
        {
            try
            {
                obj_mail.MailSendTo.Clear();
                obj_mail.MailSendTo.Add(objmail_det.email_id);

                var sb = new StringBuilder();
                string status = "";

                if (objmail_det.day_status == "1")
                {
                    status = "Approved";
                }
                else
                {
                    status = "Rejected";
                }
                sb.Append(string.Format(@"<html>
                    <body> 
                            <p>
                                Dear <b> {0} {1}</b>,
                                <br/><br/>
                                This is to inform you that your request for the asset {2} has been {3}.
                            </p>
                    <p> With Regards
                            </p>
                    </body>
                    </html>", objmail_det.employee_first_name, objmail_det.employee_last_name, objmail_det.absent_date, status));


                obj_mail.HtmlBody = sb.ToString();
                obj_mail.MailSubject = "Asset Request - " + status;
                SendMail(obj_mail);

            }
            catch (Exception ex)
            {

            }
        }



        // For Asset request
        public void AssetRequestMail(int employee_id, DateTime from_date, DateTime to_date, int assettype, string remark, int companyid, string AssetName)
        {


            // SetManasgerData(employee_id);

            var asset_approvers = _context.tbl_loan_approval_setting.Where(x => x.is_active == 1 && x.approver_type == 2).Select(p => new { p.emp_id, p.approver_role_id }).ToList();

            List<int> approver_id = new List<int>();

            for (int i = 0; i < asset_approvers.Count; i++)
            {

                if (asset_approvers[i].emp_id != 0)
                {
                    approver_id.Add(asset_approvers[i].emp_id);
                }
                else if (asset_approvers[i].approver_role_id != 0 && asset_approvers[i].approver_role_id != null)
                {
                    var all_user_id = _context.tbl_user_role_map.Where(x => x.is_deleted == 0 && x.role_id == asset_approvers[i].approver_role_id && x.tbl_user_master.default_company_id == companyid).Select(p => new
                    {
                        p.tbl_user_master.employee_id
                    }).ToList();

                    for (int j = 0; j < all_user_id.Count; j++)
                    {
                        bool _already_exist = approver_id.Contains(all_user_id[j].employee_id ?? 0);
                        if (!_already_exist)
                        {
                            approver_id.Add(all_user_id[j].employee_id ?? 0);
                        }
                    }

                }
            }



            var approver_data = _context.tbl_emp_officaial_sec.Where(a => a.is_deleted == 0 && approver_id.Contains(Convert.ToInt32(a.employee_id))).Select(a => new { a.official_email_id, a.employee_first_name }).ToList();
            for (int Index = 0; Index < approver_data.Count; Index++)
            {
                if (!string.IsNullOrEmpty(approver_data[Index].official_email_id))
                {
                    obj_mail.MailSendTo.Add(approver_data[Index].official_email_id);
                }

            }


            //get requested empdata
            var emp_data = _context.tbl_emp_officaial_sec.Where(a => a.employee_id == employee_id && a.is_deleted == 0).Select(a => new { emp_name = string.Format("{0} {1} {2}", a.employee_first_name, a.employee_middle_name, a.employee_last_name), emp_code = a.tbl_employee_id_details.emp_code }).FirstOrDefault();

            // var LeaveTypeName = _context.tbl_leave_type.Where(a => a.leave_type_id == LeaveType && a.is_active == 1).Select(a => a.leave_type_name).FirstOrDefault();

            string AssetType = "";

            if (assettype == 0)
            {
                AssetType = "New";
            }
            else if (assettype == 1)
            {
                AssetType = "Replace";
            }
            else if (assettype == 2)
            {
                AssetType = "Submit";
            }


            var sb = new StringBuilder();

            sb.Append(string.Format(@"<html>
                      <body>                  
                        <p> Please approve the following Asset request. </p><table border='1' cellspacing='0' cellpadding='0' width='600' style='width:6.25in;border-collapse:collapse;border:none'><tbody>
        <tr><td colspan='2' style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Asset Request Details<u></u><u></u></span></b></p></td></tr>
        <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Code<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{0}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
        <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Name<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{1}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
<tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Asset Name<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{2}<u></u><u></u></span></p></td></tr>        
<tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Asset Type<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{3}<u></u><u></u></span></p></td></tr>
        <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>From Date<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{4}<u></u><u></u></span></p></td></tr>
        <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>To Date<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{5}<u></u><u></u></span></p></td></tr>
        <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Reason<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u>{6}</u><u></u></span></p></td></tr>
        <tr>
            <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Status<u></u><u></u></span></b></p></td>
            <td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
                <p class='MsoNormal'>
                    <span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>Pending<u></u><u></u></span>
                </p>
            </td>
        </tr>
    </tbody>
</table></body></html>", emp_data.emp_code, emp_data.emp_name, AssetName, AssetType, from_date.ToString("dd-MM-yyyy"), to_date.ToString("dd-MM-yyyy").Substring(0, 10), remark));

            obj_mail.HtmlBody = sb.ToString();
            obj_mail.MailSubject = "Asset Request";
            SendMail(obj_mail);
        }


        public void ShiftAssignmentNotification(List<MailDetails> objmail_dtl)
        {
            try
            {
                for (int Index = 0; Index < objmail_dtl.Count; Index++)
                {

                    obj_mail.MailSendTo.Clear();
                    obj_mail.MailSendTo.Add(objmail_dtl[Index].email_id);

                    var sb = new StringBuilder();

                    sb.Append(string.Format(@"<html>
                      <body>  
<p>
                                Dear " + objmail_dtl[Index].employee_first_name + @",
                                <br/><br/> 
<p>Here is your Shift Assignment Detail :-</p>
 <br/>
                        <table border='1' cellspacing='0' cellpadding='0' width='600' style='width:6.25in;border-collapse:collapse;border:none'><tbody>
        <tr><td colspan='2' style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='center'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Shift Assignment<u></u><u></u></span></b></p></td></tr>
        <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Name<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{0}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
        <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Shift Name<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{1}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td></tr>
<tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Applicable Date<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{2}<u></u><u></u></span></p></td></tr>        
<tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Shift In Time<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{3}<u></u><u></u></span></p></td></tr>
        <tr><td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Shift Out Time<u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'>{4}<u></u><u></u></span></p></td></tr>
        

    </tbody>
</table></body></html>", objmail_dtl[Index].employee_first_name, objmail_dtl[Index].shift_name, objmail_dtl[Index].effective_dt.ToString("dd-MM-yyyy"), objmail_dtl[Index].punch_in.ToString("hh:mm tt"), objmail_dtl[Index].punch_out.ToString("hh:mm tt")));


                    objmail_dtl.RemoveAll(a => a.emp_id == objmail_dtl[Index].emp_id);

                    obj_mail.HtmlBody = sb.ToString();
                    obj_mail.MailSubject = "Shift Assignment Notification";
                    SendMail(obj_mail);
                }


            }
            catch (Exception ex)
            {

            }
        }


        //E-Sepration Request Mail
        public void EmpSeprationRequestMail(int employee_id, DateTime resign_dt, DateTime final_relieve_dt, int notice_days, int diff_notice_days, DateTime policy_relive_dt, string reason, string req_remarks)
        {
            throw new NotImplementedException();

#if false
            SetManasgerData(employee_id);

            var emp_dtl = _context.tbl_emp_officaial_sec.Where(x => x.employee_id == employee_id && x.is_deleted == 0).Select(p => new
            {
                emp_name_code = string.Format("{0} {1} {2} ({3})", p.employee_first_name, p.employee_middle_name, p.employee_last_name, p.tbl_employee_id_details.emp_code),
                p.tbl_department_master.department_name,
                date_of_joining = p.date_of_joining,
                designation = p.tbl_employee_id_details.tbl_emp_desi_allocation.OrderByDescending(q => q.emp_grade_id).FirstOrDefault(q => q.desig_id != null && q.applicable_from_date.Date <= resign_dt.Date && resign_dt.Date <= q.applicable_to_date.Date).tbl_designation_master.designation_name,
                grade_ = p.tbl_employee_id_details.tbl_emp_grade_allocation.OrderByDescending(q => q.emp_grade_id).FirstOrDefault(q => q.grade_id != null && q.applicable_from_date.Date <= resign_dt.Date && resign_dt.Date <= q.applicable_to_date.Date).tbl_grade_master.grade_name,
                p.official_email_id,
                p.tbl_location_master.location_name,
            }).FirstOrDefault();

            //here append admin id also
            List<string> data = _config.GetSection("email_id_to:Mail_to_EmpSep").Get<List<string>>();
            if (data.Count > 0)
                foreach (var item in data)
                {
                    obj_mail.MailSendTo.Add(item);
                }
            obj_mail.MailSendTo.Add(emp_dtl.official_email_id);


            var sb = new StringBuilder();

            sb.Append(string.Format(@"<html>
                      <body>  
<p>
                                E-Sepration request raised by " + emp_dtl.emp_name_code + @",
                                <br/><br/> 
<p>Here is E-Sepration Detail :-</p>
 <br/>
                        <table border='1' cellspacing='0' cellpadding='0' width='600' style='width:8.25in;border-collapse:collapse;border:none'><tbody>
        <tr><td colspan='4' style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='center'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Sepration<u></u><u></u></span></b></p></td></tr>
        <tr>
            <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Name(Code) <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{0}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>
        </tr>
        <tr>
        <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Date of Joining <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{1}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>
        </tr>
        <tr>
            <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Department <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{2}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>            
        </tr>
        <tr>
             <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Designation <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{3}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>            
        </tr>
        <tr>
             <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Location <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{4}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>            
        </tr>
        <tr>
            <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Resignation Date <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{5}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>
        </tr>
        <tr>
            <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Last Working Date <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{6}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>
        </tr>

    </tbody>
</table></body></html>", emp_dtl.emp_name_code, emp_dtl.date_of_joining.Date.ToString("dd-MM-yyyy"), emp_dtl.department_name, emp_dtl.designation, emp_dtl.location_name,
                        resign_dt.Date.ToString("dd-MM-yyyy"), final_relieve_dt.Date.ToString("dd-MM-yyyy")));



            obj_mail.HtmlBody = sb.ToString();
            obj_mail.MailSubject = "E-Sepration Request";
            SendMail(obj_mail);

#endif
        }


        public void EmpSeprationApprovalMail(List<SeprationMailDetails> sep_dtl)
        {
#if false
            for (int Index = 0; Index < sep_dtl.Count; Index++)
            {
                SetManasgerData(sep_dtl[Index].emp_id);
                var emp_dtl = _context.tbl_emp_officaial_sec.Where(x => x.employee_id == sep_dtl[Index].emp_id && x.is_deleted == 0).Select(p => new
                {
                    //emp_name_code = string.Format("{0} {1} {2} ({3})", p.employee_first_name, p.employee_middle_name, p.employee_last_name, p.tbl_employee_id_details.emp_code),
                    //p.tbl_department_master.department_name,
                    //date_of_joining = p.date_of_joining,
                    //designation = p.tbl_employee_id_details.tbl_emp_desi_allocation.OrderByDescending(q => q.emp_grade_id).FirstOrDefault(q => q.desig_id != null && q.applicable_from_date.Date <= sep_dtl[Index].resign_dt.Date && sep_dtl[Index].resign_dt.Date <= q.applicable_to_date.Date).tbl_designation_master.designation_name,
                    //grade_ = p.tbl_employee_id_details.tbl_emp_grade_allocation.OrderByDescending(q => q.emp_grade_id).FirstOrDefault(q => q.grade_id != null && q.applicable_from_date.Date <= sep_dtl[Index].resign_dt.Date && sep_dtl[Index].resign_dt.Date <= q.applicable_to_date.Date).tbl_grade_master.grade_name,
                    //p.official_email_id,
                    //p.tbl_location_master.location_name
                }).FirstOrDefault();
                ////here append admin id also
                //obj_mail.MailSendTo.Add("supriya.kakkar@sakshemit.com");
                ////obj_mail.MailSendTo.Add(emp_dtl.official_email_id);

                //here append admin id also
                List<string> data = _config.GetSection("email_id_to:Mail_to_EmpSep").Get<List<string>>();
                if (data.Count > 0)
                    foreach (var item in data)
                    {
                        obj_mail.MailSendTo.Add(item);
                    }
                obj_mail.MailSendTo.Add(emp_dtl.official_email_id);

                var sb = new StringBuilder();


                sb.Append(string.Format(@"<html>
                      <body>  
<p>
                               Dear " + emp_dtl.emp_name_code + @" your e-Sepration " + (sep_dtl[Index].is_cancel == 0 ? "" : sep_dtl[Index].is_cancel == 1 ? "Cancellation " : "") + "request is " + (sep_dtl[Index].is_final_approve == 1 ? "Approved" : sep_dtl[Index].is_final_approve == 2 ? "Rejected" : "") + @",
                                <br/><br/> 
<p>Here is your E-Sepration Detail :-</p>
 <br/>
                        <table border='1' cellspacing='0' cellpadding='0' width='600' style='width:8.25in;border-collapse:collapse;border:none'><tbody>
        <tr><td colspan='4' style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='center'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Sepration<u></u><u></u></span></b></p></td></tr>
        <tr>
            <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Name(Code) <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{0}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>
        </tr>
        <tr>
        <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Date of Joining <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{1}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>
        </tr>
        <tr>
            <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Department <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{2}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>            
        </tr>
        <tr>
             <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Designation <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{3}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>            
        </tr>
        <tr>
             <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Location <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{4}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>            
        </tr>
        <tr>
            <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Resignation Date <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{5}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>
        </tr>
        <tr>
            <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Last Working Date <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{6}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>
        </tr>
        
    </tbody>
</table></body></html>", emp_dtl.emp_name_code, emp_dtl.date_of_joining.Date.ToString("dd-MM-yyyy"), emp_dtl.department_name, emp_dtl.designation, emp_dtl.location_name,
                            sep_dtl[Index].resign_dt.Date.ToString("dd-MM-yyyy"), sep_dtl[Index].final_relieve_dt.Date.ToString("dd-MM-yyyy")));



                obj_mail.HtmlBody = sb.ToString();
                obj_mail.MailSubject = "E-Sepration Request";
                SendMail(obj_mail);

            }


            //obj_mail.MailSendTo.Add(emp_dtl.official_email_id);

        #endif
        }

        public void EmpSeprationCancellationMail(List<SeprationMailDetails> sep_dtl)
        {
#if false
            for (int Index = 0; Index < sep_dtl.Count; Index++)
            {
                SetManasgerData(sep_dtl[Index].emp_id);
                var emp_dtl = _context.tbl_emp_officaial_sec.Where(x => x.employee_id == sep_dtl[Index].emp_id && x.is_deleted == 0).Select(p => new
                {
                    emp_name_code = string.Format("{0} {1} {2} ({3})", p.employee_first_name, p.employee_middle_name, p.employee_last_name, p.tbl_employee_id_details.emp_code),
                    //p.tbl_department_master.department_name,
                    //date_of_joining = p.date_of_joining,
                    designation = p.tbl_employee_id_details.tbl_emp_desi_allocation.OrderByDescending(q => q.emp_grade_id).FirstOrDefault(q => q.desig_id != null && q.applicable_from_date.Date <= sep_dtl[Index].resign_dt.Date && sep_dtl[Index].resign_dt.Date <= q.applicable_to_date.Date).tbl_designation_master.designation_name,
                    grade_ = p.tbl_employee_id_details.tbl_emp_grade_allocation.OrderByDescending(q => q.emp_grade_id).FirstOrDefault(q => q.grade_id != null && q.applicable_from_date.Date <= sep_dtl[Index].resign_dt.Date && sep_dtl[Index].resign_dt.Date <= q.applicable_to_date.Date).tbl_grade_master.grade_name,
                    p.official_email_id,
                    //p.tbl_location_master.location_name,
                }).FirstOrDefault();
                ////here append admin id also
                //obj_mail.MailSendTo.Add("supriya.kakkar@sakshemit.com");
                ////obj_mail.MailSendTo.Add(emp_dtl.official_email_id);

                //here append admin id also
                List<string> data = _config.GetSection("email_id_to:Mail_to_EmpSep").Get<List<string>>();
                if (data.Count > 0)
                    foreach (var item in data)
                    {
                        obj_mail.MailSendTo.Add(item);
                    }
                obj_mail.MailSendTo.Add(emp_dtl.official_email_id);


                var sb = new StringBuilder();


                sb.Append(string.Format(@"<html>
                      <body>  
<p>
                               Dear " + emp_dtl.emp_name_code + @" your e-Sepration " + (sep_dtl[Index].is_cancel == 0 ? "" : sep_dtl[Index].is_cancel == 1 ? "Cancellation " : "") + "request is " + (sep_dtl[Index].is_deleted == 1 ? "Successfully Cancelled" : (sep_dtl[Index].is_final_approve == 0 ? "Successfully raised" : sep_dtl[Index].is_final_approve == 1 ? "Approved" : sep_dtl[Index].is_final_approve == 2 ? "Rejected" : "")) + @",
                                <br/><br/> 
<p>Here is your E-Sepration Detail :-</p>
 <br/>
                        <table border='1' cellspacing='0' cellpadding='0' width='600' style='width:8.25in;border-collapse:collapse;border:none'><tbody>
        <tr><td colspan='4' style='border:solid #666666 1.0pt;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='center'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Sepration<u></u><u></u></span></b></p></td></tr>
        <tr>
            <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Employee Name(Code) <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{0}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>
        </tr>
        <tr>
        <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Date of Joining <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{1}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>
        </tr>
        <tr>
            <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Department <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{2}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>            
        </tr>
        <tr>
             <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Designation <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{3}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>            
        </tr>
        <tr>
             <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Location <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{4}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>            
        </tr>
        <tr>
            <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Resignation Date <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{5}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>
        </tr>
        <tr>
            <td style='border:solid #666666 1.0pt;border-top:none;background:#edf3f4;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal' align='right' style='text-align:right'><b><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#667e99'>Last Working Date <u></u><u></u></span></b></p></td><td style='border-top:none;border-left:none;border-bottom:solid #666666 1.0pt;border-right:solid #666666 1.0pt;padding:3.75pt 3.75pt 3.75pt 3.75pt'><p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#1f497d'>{6}</span><span style='font-size:10.0pt;font-family:&quot;Verdana&quot;,sans-serif;color:#666666'><u></u><u></u></span></p></td>
        </tr>
        
    </tbody>
</table></body></html>", emp_dtl.emp_name_code, emp_dtl.date_of_joining.Date.ToString("dd-MM-yyyy"), emp_dtl.department_name, emp_dtl.designation, emp_dtl.location_name,
                            sep_dtl[Index].resign_dt.Date.ToString("dd-MM-yyyy"), sep_dtl[Index].final_relieve_dt.Date.ToString("dd-MM-yyyy")
                            ));



                obj_mail.HtmlBody = sb.ToString();
                obj_mail.MailSubject = "E-Sepration Request";
                SendMail(obj_mail);

            }


            //obj_mail.MailSendTo.Add(emp_dtl.official_email_id);

#endif
        }

        public void SendMailNoDuesResponsiblePerson(string emailto, string seperation_email,string seperation_name,string seperationempcode)
        {
            var emailtxt = "Dear Sir/Madam,<br>Employee - " + seperation_name + " - " + seperationempcode + " is on notice period. So please clear all dues from your side if emplpoyee have no any dues for all particulars.";
            var emails = emailto.Split(",");
            foreach (var em in emails)
            {
                obj_mail.MailSendTo.Add(em);
            }
            var sb = new StringBuilder();
          
            obj_mail.HtmlBody = emailtxt.ToString();
            obj_mail.MailSubject = "No Dues Clearance";
            SendMail(obj_mail);
        }

    }
}

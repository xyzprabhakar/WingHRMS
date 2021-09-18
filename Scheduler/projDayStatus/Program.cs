using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Sockets;
//using projAttend_DLL;

namespace projDayStatus
{
    class Program
    {
        static void Main(string[] args)
        {
            List<clsInsatnce> InstanceList = new List<clsInsatnce>();
            InstanceList.Add(new clsInsatnce()
            {
                api_project_url = Properties.Resources.api_project_url,
                company_count = 1,
                created_by = 1,
                created_date = DateTime.Now,
                db_connection = "",
                design_project_url = "",
                instance_id = "1",
                is_active = 1,
                last_modified_by = 1,
                last_modified_date = DateTime.Now,
                organisation = Properties.Resources.DefaultOrganisation,
                premises_type = "1",
                superadmin_password = Properties.Resources.superadmin_password,
                superadmin_username = Properties.Resources.superadmin_username
            });

            try
            {
                Console.WriteLine("Schedule Started");

                //Leave credit
                Console.WriteLine("---Update Leave Credit Start---");
                LeaveCredit ObjLeave = new LeaveCredit();
                string LeaveResult = ObjLeave.LeaveCredit_AllInstance(InstanceList);
                Console.WriteLine(LeaveResult);
                Console.WriteLine("---Update Leave Credit End---");
                

                //Update Employment Type
                Console.WriteLine("--Update Employment Type in official section start--");
                EmploymentType objemptype = new EmploymentType();
                string EmpType = objemptype.EmploymentType_Update(InstanceList);
                Console.WriteLine(EmpType);
                Console.WriteLine("--Update Employment Type in official section end--");
                

                //Update Day Status
                Console.WriteLine("---Update Day Status Start---");
                InstanceData ObjInstance = new InstanceData();
                string result = ObjInstance.Update_DayStatus_AllInstance(InstanceList);
                Console.WriteLine(result);
                Console.WriteLine("---Update Day Status End---");

                //Shift Notification
                Console.WriteLine("--Start Shift Change Notification--");
                ShiftNotification objshift = new ShiftNotification();
                string objshift_notification_result = objshift.ShiftAssignmentNotification(InstanceList);
                Console.WriteLine(objshift_notification_result);
                Console.WriteLine("--End Shift Change Notification--");

                //Yesterday Absent Notification
                Console.WriteLine("---Yesterday Absent Notification Start---");
                AttendanceNotification ObjAtt = new AttendanceNotification();
                string ObjAttYesterdayResult = ObjAtt.LastDayAbsentNotification(InstanceList);
                Console.WriteLine(ObjAttYesterdayResult);
                Console.WriteLine("---Yesterday Absent Notification End---");


                //Yesterday Absent Notification
                Console.WriteLine("---Weekly Attendance Notification Start---");
                AttendanceNotification objweekatt = new AttendanceNotification();
                string ObjAttWeeklyResult = objweekatt.WeeklyAttendanceNotification(InstanceList);
                Console.WriteLine(ObjAttWeeklyResult);
                Console.WriteLine("---Weekly Attendance Notification End---");

                //No Dues Clearance Particular Department Wise Entry
                Console.WriteLine("---Update No Dues Clearance Particular Department Wise Start ---");
                NoDues ObjNoDues = new NoDues();
                string NoDuesResult = ObjNoDues.DoDuesEmployeeEntry(InstanceList);
                Console.WriteLine(NoDuesResult);
                Console.WriteLine("---Update No Dues Clearance Particular Department Wise End---");

                //LWP Entry
                Console.WriteLine("---LWP Entry Start ---");
                LWP ObjLWP = new LWP();
                string LWPResult = ObjLWP.LWPEntry(InstanceList);
                Console.WriteLine(LWPResult);
                Console.WriteLine("---LWP Entry End---");


                //Console.WriteLine(result);
                Console.WriteLine("Process completed.");
                Console.WriteLine("Thankyou!! Have a nice day");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}

using projAttend_DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projAttend_Cui_new
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Schduel Started");
                clsCalculation ob = new clsCalculation();
                if (projAttend_DLL.Properties.Settings.Default.user_name == "" || projAttend_DLL.Properties.Settings.Default.password == "")
                {
                    throw new Exception("user name and apssword required");
                }
                Console.WriteLine("Set Token..");
                ob.login_set_tokken();
                Console.WriteLine("Load data from Machine..");
                DataTable dt = ob.get_all_machine_data(Convert.ToDateTime(DateTime.Now.AddDays(projAttend_DLL.Properties.Settings.Default.fromday).ToString("dd-MMM-yyyy 00:00:00")),
                         Convert.ToDateTime(DateTime.Now.AddDays(projAttend_DLL.Properties.Settings.Default.today).AddMinutes(-1).ToString("dd-MMM-yyyy 00:00:00")), 0);
                //DataTable dt = ob.get_all_machine_data(Convert.ToDateTime(("01-nov-2019")),
                //        Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("dd-MMM-yyyy 00:00:00")), 0);
                Console.WriteLine("Save Data..");
                ob.save_data(dt);
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

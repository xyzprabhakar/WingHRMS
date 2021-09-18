using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using RestSharp;
using System.Net.Sockets;

namespace projAttend_DLL
{

    public class mdlLogin
    {
        public string token { get; set; }
        public string statusCode { get; set; }
        public int default_company { get; set; }
    }
    public class mdlMachine
    {
        public int machine_id { get; set; }
        public string machine_description { get; set; }
        public int port_number { get; set; }
        public string ip_address { get; set; }
        public byte machine_type { get; set; }
    }


    public class clsCalculation
    {
        string ApibaseUrl = Convert.ToString(Properties.Resources.ApiUrl);        
        public string tokken = "a";
        public string username = "";
        public string password = "";
        public int emp_id = 0;
        public static int is_login_success = 0;
        public DataTable dt_Machine = null;

        public clsCalculation()
        {
            this.username = Properties.Settings.Default.user_name;
            this.password = Properties.Settings.Default.password;
            this.tokken = Properties.Settings.Default.tokken;
        }




        public void login_set_tokken()
        {
            RestClient client = new RestClient(ApibaseUrl);
            client.AddDefaultHeader("authorization", "Bearer " + tokken);
            var request = new RestRequest(Convert.ToString(Properties.Resources.res_login), Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\n\t\"user_name\":\"" + this.username + "\",\n\t\"password\":\"" + this.password + "\",\n\t\"emp_id\":\"1\",\n\t\"emp_name\":\"hi\",\n\t\"tokken\":\"sdas\"\n,\n\t\"CaptchaCode\":\"abcc\"\n,\n\t\"CcCode\":\"sdas\"\n,\n\t\"Fromexe\":\"32\"\n}", ParameterType.RequestBody);
            
            IRestResponse<mdlLogin> mdlLogin_ = client.Execute<mdlLogin>(request);
            if (mdlLogin_.IsSuccessful)
            {
                int.TryParse(mdlLogin_.Data.statusCode, out is_login_success);
                if (is_login_success == 1)
                {
                    this.tokken = mdlLogin_.Data.token;
                    Properties.Settings.Default.tokken = this.tokken;
                    Properties.Settings.Default.company_id = "1";
                    //Properties.Settings.Default.company_id = Convert.ToString(mdlLogin_.Data.default_company);
                    Properties.Settings.Default.Save();
                }
            }

        }

        void LoadMachineThread(mdlMachine mdl)
        {
            //if (mdl.ip_address != "192.168.30.108")
            //{
            //    return;
            //}
            //if (mdl.ip_address != "192.168.20.162")
            //{
            //    return;
            //}

            //if (mdl.ip_address != "192.168.30.46")
            //{
            //    return;
            //}
            DataRow dr = dt_Machine.NewRow();
            //Check Wheather the machine are ping on local 
            if (device_are_connected_or_not(mdl.ip_address, mdl.port_number))
            {
                dr["disptext"] = "Connected - " + mdl.ip_address;
            }
            else
            {
                dr["disptext"] = "Not Connected - " + mdl.ip_address;
            }


            dr["machine_id"] = mdl.machine_id;
            dr["machine_description"] = mdl.machine_description;

            dr["port_number"] = mdl.port_number;
            dr["ip_address"] = mdl.ip_address;
            dr["machine_type"] = mdl.machine_type;
            dt_Machine.Rows.Add(dr);
        }


        public void Load_all_machine()
        {
            RestClient client = new RestClient(ApibaseUrl);
            client.AddDefaultHeader("authorization", "Bearer " + Properties.Settings.Default.tokken);
            var request = new RestRequest(Convert.ToString(Properties.Resources.res_machine) + "/"+Properties.Resources.companyid, Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse<List<mdlMachine>> mdlMachine_ = client.Execute<List<mdlMachine>>(request);
            if (mdlMachine_.IsSuccessful)
            {
                this.dt_Machine = new DataTable();
                dt_Machine.Columns.Add("machine_id", typeof(int));
                dt_Machine.Columns.Add("machine_description", typeof(string));
                dt_Machine.Columns.Add("disptext", typeof(string));
                dt_Machine.Columns.Add("port_number", typeof(int));
                dt_Machine.Columns.Add("ip_address", typeof(string));
                dt_Machine.Columns.Add("machine_type", typeof(byte));
                if (mdlMachine_.Data != null)
                {
                    DataRow dr1 = dt_Machine.NewRow();
                    dr1["machine_id"] = 0;
                    dr1["machine_description"] = "All Machine";
                    dr1["disptext"] = "All Machine";
                    dr1["port_number"] = 0;
                    dr1["ip_address"] = "";
                    dr1["machine_type"] = 0;
                    dt_Machine.Rows.Add(dr1);

                    List<Task> tasklists = new List<Task>();
                    for (int i = 0; i < mdlMachine_.Data.Count; i++)
                    {
                        int index = i;
                        Task tasklist = new Task(() =>
                        {
                            LoadMachineThread(mdlMachine_.Data[index]);
                        });
                        tasklists.Add(tasklist);
                        tasklist.Start();

                        //DataRow dr = dt_Machine.NewRow();
                        ////Check Wheather the machine are ping on local 
                        //if (device_are_connected_or_not(mdlMachine_.Data[i].ip_address, mdlMachine_.Data[i].port_number))
                        //{
                        //    dr["disptext"] = "Connected - " + mdlMachine_.Data[i].ip_address;
                        //}
                        //else
                        //{
                        //    dr["disptext"] = "Not Connected - " + mdlMachine_.Data[i].ip_address;
                        //}


                        //dr["machine_id"] = mdlMachine_.Data[i].machine_id;
                        //dr["machine_description"] = mdlMachine_.Data[i].machine_description;

                        //dr["port_number"] = mdlMachine_.Data[i].port_number;
                        //dr["ip_address"] = mdlMachine_.Data[i].ip_address;
                        //dr["machine_type"] = mdlMachine_.Data[i].machine_type;
                        //dt_Machine.Rows.Add(dr);
                        System.Threading.Thread.Sleep(1000);
                    }

                    //DataRow dr2 = dt_Machine.NewRow();
                    //dr2["machine_id"] = 200;
                    //dr2["machine_description"] = "";
                    //dr2["disptext"] = "192.168.30.46";
                    //dr2["port_number"] = 4370;
                    //dr2["ip_address"] = "192.168.30.46";
                    //dr2["machine_type"] = "1";
                    //dt_Machine.Rows.Add(dr2);

                    Task.WaitAll(tasklists.ToArray());
                }
            }
        }

        public bool device_are_connected_or_not(string hostUri, int portNumber)
        {
            try
            {
                using (var client = new TcpClient(hostUri, portNumber))
                    return true;
            }
            catch (SocketException ex)
            {
                return false;
            }
        }

        public DataTable get_all_machine_data(DateTime fromdate, DateTime todate, int machine_id)
        {
            if (machine_id == 0)
            {
                Load_all_machine();
            }
            

            DataTable dt = new DataTable();
            dt.Columns.Add("MachineID", typeof(int));
            dt.Columns.Add("MachineIP");
            dt.Columns.Add("empCardNO");
            dt.Columns.Add("punchtime", typeof(DateTime));
            clsZkemkeepr_read ob = new clsZkemkeepr_read();
            List<MachineInfoNew> lstMachineInfo = null;


            for (int i = 0; i < dt_Machine.Rows.Count; i++)
            {
                if (dt_Machine.Rows[i]["machine_id"].ToString() != "0")
                {
                    lstMachineInfo = null;
                    if (machine_id > 0)
                    {
                        if (machine_id != Convert.ToInt32(dt_Machine.Rows[i]["machine_id"]))
                        {
                            continue;
                        }
                    }
                    if (Convert.ToString(dt_Machine.Rows[i]["disptext"]).Contains("Not Connected"))
                    {
                        continue;
                    }
                    if (Convert.ToInt32(dt_Machine.Rows[i]["machine_type"]) == 1)
                    {
                        lstMachineInfo = ob.connectDevice(fromdate, todate, Convert.ToString(dt_Machine.Rows[i]["ip_address"]), Convert.ToInt32(dt_Machine.Rows[i]["port_number"]));
                    }
                    if (lstMachineInfo != null)
                    {
                        for (int j = 0; j < lstMachineInfo.Count; j++)
                        {
                            DataRow dr = dt.NewRow();
                            dr["MachineID"] = dt_Machine.Rows[i]["machine_id"];
                            dr["MachineIP"] = dt_Machine.Rows[i]["ip_address"];
                            dr["empCardNO"] = lstMachineInfo[j].IndRegID;
                            dr["punchtime"] = lstMachineInfo[j].DateTimeRecord;
                            dt.Rows.Add(dr);
                        }
                    }

                }
            }
            return dt;

        }

        public DataTable ReadFromFile(string filePath, DateTime fromDate, DateTime Todate, int machineID)
        {
            int counter = 0;
            string line;
            int EmpCode = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("MachineID", typeof(int));
            dt.Columns.Add("MachineIP");
            dt.Columns.Add("empCardNO");
            dt.Columns.Add("punchtime", typeof(DateTime));
            System.IO.StreamReader file =
            new System.IO.StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                var data = line.Replace("	", "@").Split('@');
                if (data.Length > 0)
                {
                    DataRow dr = dt.NewRow();
                    if (int.TryParse(data[0], out EmpCode))
                    {
                        if (DateTime.Compare(fromDate, Convert.ToDateTime(data[1].Trim())) <= 0 && DateTime.Compare(Convert.ToDateTime(data[1].Trim()), Todate) <= 0)
                        {
                            dr["MachineID"] = machineID;
                            dr["MachineIP"] = "File Import";
                            dr["empCardNO"] = EmpCode;
                            dr["punchtime"] = Convert.ToDateTime(data[1].Trim()).ToString("dd-MMM-yyyy hh:mm:ss tt");
                            dt.Rows.Add(dr);
                        }
                    }
                }
                counter++;
            }
            return dt;
        }

        public string getGuid()
        {
            RestClient client1 = new RestClient(ApibaseUrl);
            var request1 = new RestRequest("api/Values/GetGUID", Method.GET);
            request1.AddHeader("cache-control", "no-cache");
            request1.AddHeader("Content-Type", "application/json");
            string guid = client1.Execute<dynamic>(request1).Data;
            return guid;
        }


        public bool save_data(DataTable dt)
        {
            if (dt == null)
            {
                return false;
            }
            if (dt.Rows.Count == 0)
            {
                return false;
            }




            StringBuilder prepare_param = new StringBuilder("");
            int TotalRows_to_be_proce = 1000;
            int totalProcesed = 0;
            RestClient client = new RestClient(ApibaseUrl);
            client.AddDefaultHeader("authorization", "Bearer " + Properties.Settings.Default.tokken);
            
            while (totalProcesed < dt.Rows.Count)
            {
                if (totalProcesed == dt.Rows.Count - 1 || totalProcesed + 1 % TotalRows_to_be_proce == 0)
                {
                    prepare_param.Append("\n{\n\t\t\"card_number\":\"" + Convert.ToInt32(dt.Rows[totalProcesed]["empCardNO"]) + "\"" +
                    ",\n\t\t\"punch_time\":\"" + Convert.ToDateTime(dt.Rows[totalProcesed]["punchtime"]).ToString("dd-MMM-yyyy hh:mm:ss tt") + "\"" +
                    ",\n\t\t\"machine_id\": \"" + dt.Rows[totalProcesed]["MachineID"] + "\"\n\t}\n");
                    var request = new RestRequest(Convert.ToString(Properties.Resources.res_save_attendance) + Properties.Settings.Default.company_id, Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("undefined", "[\n" + prepare_param.ToString() + "\n]", ParameterType.RequestBody);
                    
                    client.AddDefaultHeader("salt", getGuid());
                    IRestResponse res = client.Execute(request);
                    if (!res.IsSuccessful)
                    {
                        throw new Exception("Error occured" + res.StatusCode);
                    }
                    
                    prepare_param.Clear();

                }
                else
                {
                    prepare_param.Append("\n{\n\t\t\"card_number\":\"" + Convert.ToInt32(dt.Rows[totalProcesed]["empCardNO"]) + "\"" +
                    ",\n\t\t\"punch_time\":\"" + Convert.ToDateTime(dt.Rows[totalProcesed]["punchtime"]).ToString("dd-MMM-yyyy hh:mm:ss tt") + "\"" +
                    ",\n\t\t\"machine_id\": \"" + dt.Rows[totalProcesed]["MachineID"] + "\"\n\t},\n");
                }
                totalProcesed++;

            }


            return true;
        }
    }
    
}

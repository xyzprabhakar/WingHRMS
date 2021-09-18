using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using RestSharp;

namespace projDayStatus
{
    public class mdlLogin
    {
        public string token { get; set; }
        public string statusCode { get; set; }
        public int default_company { get; set; }
        public int emp_id { get; set; }
    }
    public class tokenizer
    {
        public string ApibaseUrl = "";
        public string tokken = "";
        public string username = "";
        public string password = "";
        public int emp_id = 0;
        public static int is_login_success = 0;
        public string company_id = "0";

        public tokenizer()
        {
            this.username = "";//Properties.Settings.Default.user_name;
            this.password = "";// Properties.Settings.Default.password;
            this.tokken = "";// Properties.Settings.Default.tokken;
        }

        public void login_set_tokken()
        {
            
            RestClient client = new RestClient(ApibaseUrl);
            client.AddDefaultHeader("authorization", "Bearer " + tokken);
            var request = new RestRequest("/Login", Method.POST);//RestRequest(Convert.ToString(Properties.Resources.res_login), Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", "{\n\t\"user_name\":\"" + username + "\",\n\t\"password\":\"" + password + "\",\n\t\"emp_id\":\"1\",\n\t\"emp_name\":\"hi\",\n\t\"tokken\":\"sdas\"\n,\n\t\"CaptchaCode\":\"abcc\"\n,\n\t\"CcCode\":\"sdas\"\n,\n\t\"Fromexe\":\"32\"\n}", ParameterType.RequestBody);
            IRestResponse<mdlLogin> mdlLogin_ = client.Execute<mdlLogin>(request);
            if (mdlLogin_.IsSuccessful)
            {
                int.TryParse(mdlLogin_.Data.statusCode, out is_login_success);
                if (is_login_success == 1)
                {
                    this.tokken = mdlLogin_.Data.token;
                    emp_id = mdlLogin_.Data.emp_id;
                    //Properties.Settings.Default.user_name = this.username;
                    //Properties.Settings.Default.password = this.password;
                    //Properties.Settings.Default.tokken = this.tokken;
                    //Properties.Settings.Default.company_id = Convert.ToString(mdlLogin_.Data.default_company);

                    company_id = Convert.ToString(mdlLogin_.Data.default_company);
                    //Settings.Default.Save();
                }
            }

        }

      


    }
}

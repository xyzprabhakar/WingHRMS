using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace projDayStatus
{
   public class EmploymentType
    {
        string ApibaseUrl = Convert.ToString(Properties.Resources.BaseUrl);

        public string EmploymentType_Update()
        {
            Api_log log = new Api_log();
            try
            {
                //call api to get instance data for process Attendance Status
                RestClient client = new RestClient(ApibaseUrl);
                var request = new RestRequest("/apiLicense/GetInsatnceData", Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/json");
                IRestResponse<List<clsInsatnce>> ObjResult = client.Execute<List<clsInsatnce>>(request);

                if (ObjResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<clsInsatnce> InstanceList = ObjResult.Data;

                    for (int Index = 0; Index < InstanceList.Count; Index++)
                    {
                        
                        if ((InstanceList[Index].organisation == "localhost") || (InstanceList[Index].organisation == "Sakshem IT") || (InstanceList[Index].organisation == "Sakshem IT-test"))
                        {
                            continue;
                        }

                        try
                        {
                            tokenizer ObjToken = new tokenizer();
                            ObjToken.username = InstanceList[Index].superadmin_username;
                            ObjToken.password = InstanceList[Index].superadmin_password;
                            ObjToken.ApibaseUrl = InstanceList[Index].api_project_url;
                            //call login
                            ObjToken.login_set_tokken();

                            if (!string.IsNullOrEmpty(ObjToken.tokken))
                            {
                                //call and get guid
                                //call api to get instance data for process Attendance Status
                                RestClient client_guid = new RestClient(ObjToken.ApibaseUrl);
                                var request_guid = new RestRequest("/Values/GetGUID", Method.GET);
                                request_guid.AddHeader("cache-control", "no-cache");
                                request_guid.AddHeader("Content-Type", "application/json");
                                request_guid.AddParameter("id", ObjToken.emp_id, ParameterType.QueryString);
                                IRestResponse<List<dynamic>> ObjGuid = client_guid.Execute<List<dynamic>>(request_guid);

                                if (ObjGuid.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    string guid_ = ObjGuid.Data[0].ToString();
                                    //call api for daily attendance status update
                                    RestClient client1 = new RestClient(ObjToken.ApibaseUrl);
                                    client1.AddDefaultHeader("authorization", "Bearer " + ObjToken.tokken);
                                    var request1 = new RestRequest("/apiEmployee/UpdatOfficialEmploymentType", Method.GET);
                                    request1.AddHeader("cache-control", "no-cache");
                                    request1.AddParameter("salt", guid_, ParameterType.HttpHeader);
                                   
                                    IRestResponse ObjResult_ = client1.Execute(request1);
                                    if (ObjResult_.StatusCode == System.Net.HttpStatusCode.OK)
                                    {

                                    }
                                    else
                                    {

                                    }

                                    log.api_type = "Employment Type";
                                    log.entrydate = DateTime.Now;
                                    log.instance_id = InstanceList[Index].instance_id;
                                    log.response = ObjResult_.Content == "" ? ObjResult_.StatusDescription : ObjResult_.Content;

                                    RestClient client_err = new RestClient(ApibaseUrl);
                                    var request_err = new RestRequest("/apiLicense/SaveApiLogData", Method.POST);
                                    request_err.AddHeader("cache-control", "no-cache");
                                    request_err.AddJsonBody(log);
                                    IRestResponse ObjErrResult = client_err.Execute(request_err);

                                }
                            }
                        }
                        catch (Exception exx)
                        {
                            log.api_type = "Employment Type";
                            log.entrydate = DateTime.Now;
                            log.instance_id = InstanceList[Index].instance_id;
                            log.response = exx.Message;

                            RestClient client_err = new RestClient(ApibaseUrl);
                            var request_err = new RestRequest("/apiLicense/SaveApiLogData", Method.POST);
                            request_err.AddHeader("cache-control", "no-cache");
                            request_err.AddJsonBody(log);
                            IRestResponse ObjErrResult = client_err.Execute(request_err);
                        }
                    }


                    return "Data process successfully !!";
                }
                else
                {
                    return ObjResult.Data.ToString();
                }


            }
            catch (Exception ex)
            {
                //save error log
                log.api_type = "Employment Type";
                log.entrydate = DateTime.Now;
                log.instance_id = "";
                log.response = ex.Message;

                RestClient client = new RestClient(ApibaseUrl);
                var request = new RestRequest("/apiLicense/SaveApiLogData", Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddJsonBody(log);
                IRestResponse ObjResult = client.Execute(request);

                return ex.Message;
            }

        }

        public string EmploymentType_Update(List<clsInsatnce> InstanceList)
        {
            Api_log log = new Api_log();
            try
            {
                for (int Index = 0; Index < InstanceList.Count; Index++)
                {

                    if ((InstanceList[Index].organisation == "localhost") || (InstanceList[Index].organisation == "Sakshem IT") || (InstanceList[Index].organisation == "Sakshem IT-test"))
                    {
                        continue;
                    }

                    try
                    {
                        tokenizer ObjToken = new tokenizer();
                        ObjToken.username = InstanceList[Index].superadmin_username;
                        ObjToken.password = InstanceList[Index].superadmin_password;
                        ObjToken.ApibaseUrl = InstanceList[Index].api_project_url;
                        //call login
                        ObjToken.login_set_tokken();

                        if (!string.IsNullOrEmpty(ObjToken.tokken))
                        {
                            //call and get guid
                            //call api to get instance data for process Attendance Status
                            RestClient client_guid = new RestClient(ObjToken.ApibaseUrl);
                            var request_guid = new RestRequest("/Values/GetGUID", Method.GET);
                            request_guid.AddHeader("cache-control", "no-cache");
                            request_guid.AddHeader("Content-Type", "application/json");
                            request_guid.AddParameter("id", ObjToken.emp_id, ParameterType.QueryString);
                            IRestResponse<List<dynamic>> ObjGuid = client_guid.Execute<List<dynamic>>(request_guid);

                            if (ObjGuid.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                string guid_ = ObjGuid.Data[0].ToString();
                                //call api for daily attendance status update
                                RestClient client1 = new RestClient(ObjToken.ApibaseUrl);
                                client1.AddDefaultHeader("authorization", "Bearer " + ObjToken.tokken);
                                var request1 = new RestRequest("/apiEmployee/UpdatOfficialEmploymentType", Method.GET);
                                request1.AddHeader("cache-control", "no-cache");
                                request1.AddParameter("salt", guid_, ParameterType.HttpHeader);

                                IRestResponse ObjResult_ = client1.Execute(request1);
                                if (ObjResult_.StatusCode == System.Net.HttpStatusCode.OK)
                                {

                                }
                                else
                                {

                                }

                                log.api_type = "Employment Type";
                                log.entrydate = DateTime.Now;
                                log.instance_id = InstanceList[Index].instance_id;
                                log.response = ObjResult_.Content == "" ? ObjResult_.StatusDescription : ObjResult_.Content;


                            }
                        }
                    }
                    catch (Exception exx)
                    {
                        log.api_type = "Employment Type";
                        log.entrydate = DateTime.Now;
                        log.instance_id = InstanceList[Index].instance_id;
                        log.response = exx.Message;

                    }
                }


                return "Data process successfully !!";

            }
            catch (Exception ex)
            {
                //save error log
                log.api_type = "Employment Type";
                log.entrydate = DateTime.Now;
                log.instance_id = "";
                log.response = ex.Message;


                return ex.Message;
            }

        }
    }
}

using Microsoft.Extensions.Configuration;
using projContext.DB.CRM.Travel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using projContext;
using System.IO.Compression;
using Newtonsoft.Json;

namespace projAPI.Model.Travel
{
    public class TripJack
    {
        private readonly TravelContext  _context;
        private readonly IConfiguration _config;
        public TripJack(TravelContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        private mdlReturnData GetResponse(string requestData, string url)
        {
            mdlReturnData mdl = new mdlReturnData();
            mdl.MessageType = enmMessageType.None ;
            mdl.Message = string.Empty;
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(requestData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("apikey", _config["Travel:TripJack:Credential:apikey"]);
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();
                WebResponse webResponse = request.GetResponse();
                var rsp = webResponse.GetResponseStream();
                if (rsp == null)
                {
                    mdl.Message = "No Response Found";
                }
                using (StreamReader readStream = new StreamReader(rsp))
                {
                    mdl.MessageType = enmMessageType.Success;
                    mdl.Message = readStream.ReadToEnd();//JsonConvert.DeserializeXmlNode(readStream.ReadToEnd(), "root").InnerXml;
                }
                return mdl;
            }
            catch (WebException webEx)
            {
                mdl.MessageType = enmMessageType.Error;
                //get the response stream
                WebResponse response = webEx.Response;
                if (response == null)
                {
                    mdl.Message = "No Response from server";
                }
                else
                {
                    Stream stream = response.GetResponseStream();
                    String responseMessage = new StreamReader(stream).ReadToEnd();
                    mdl.Message = responseMessage;
                }


            }
            catch (Exception ex)
            {
                mdl.MessageType = enmMessageType.Error;
                mdl.Message = ex.Message;
            }
            return mdl;
        }

        private mdlReturnData GetResponseZip(string requestData, string url)
        {
            mdlReturnData mdl = new mdlReturnData();
            mdl.MessageType = enmMessageType.None;
            mdl.Message = string.Empty;
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(requestData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Accept-Encoding", "gzip");
                request.Headers.Add("apikey", _config["Travel:TripJack:Credential:apikey"]);
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();
                WebResponse webResponse = request.GetResponse();
                var rsp = webResponse.GetResponseStream();
                if (rsp == null)
                {
                    mdl.Message = "No Response Found";
                }
                using (StreamReader readStream = new StreamReader(new GZipStream(rsp, CompressionMode.Decompress)))
                {
                    mdl.MessageType = enmMessageType.Success;
                    mdl.Message = readStream.ReadToEnd();//JsonConvert.DeserializeXmlNode(readStream.ReadToEnd(), "root").InnerXml;
                }
                return mdl;
            }
            catch (WebException webEx)
            {
                mdl.MessageType = enmMessageType.Error;
                //get the response stream
                WebResponse response = webEx.Response;
                if (response != null)
                {
                    Stream stream = response.GetResponseStream();
                    String responseMessage = new StreamReader(stream).ReadToEnd();
                    mdl.Message = responseMessage;
                }
                else
                {
                    mdl.Message = "Invalid Connection";
                }

            }
            catch (Exception ex)
            {
                mdl.MessageType = enmMessageType.Error;
                mdl.Message = ex.Message;
            }
            return mdl;
        }
        public Task<mdlSearchResponse> SearchAsync(mdlSearchRequest request)
        { 

        }
        private SearchqueryWraper SearchRequestMap(mdlSearchRequest request)
        {
            enmCabinClass enmCabin = request.Segments[0].FlightCabinClass == enmCabinClass.ALLClasses ? enmCabinClass.ECONOMY : request.Segments[0].FlightCabinClass;
            List<Routeinfo> routeinfos = new List<Routeinfo>();
            for (int i = 0; i < request.Segments.Count(); i++)
            {
                if (i > 0 && (request.JourneyType == enmJourneyType.OneWay || request.JourneyType == enmJourneyType.AdvanceSearch))
                {
                    break;
                }
                if (i > 1 && (request.JourneyType == enmJourneyType.Return || request.JourneyType == enmJourneyType.SpecialReturn))
                {
                    break;
                }
                Routeinfo routeinfo = new Routeinfo()
                {
                    fromCityOrAirport = new cityorairport()
                    {
                        code = request.Segments[i].Origin
                    },
                    toCityOrAirport = new cityorairport()
                    {
                        code = request.Segments[i].Destination
                    },
                    travelDate = request.Segments[i].TravelDt.ToString("yyyy-MM-dd"),
                };
                routeinfos.Add(routeinfo);
            }



            Searchquery mdl = new Searchquery()
            {
                cabinClass = enmCabin.ToString(),
                paxInfo = new Paxinfo()
                {
                    ADULT = request.AdultCount,
                    CHILD = request.ChildCount,
                    INFANT = request.InfantCount
                },
                routeInfos = routeinfos.ToArray(),
                searchModifiers = new Searchmodifiers()
                {
                    isDirectFlight = true,
                    isConnectingFlight = true
                },

            };

            if (request.PreferredAirlines != null)
            {
                mdl.preferredAirline = request.PreferredAirlines.Select(p => new cityorairport { code = p }).ToArray();
            }
            SearchqueryWraper mdlW = new SearchqueryWraper()
            {
                searchQuery = mdl
            };
            return mdlW;
        }


        private async Task<mdlSearchResponse> SearchFromTripJackAsync(mdlSearchRequest request)
        {
            SearchresultWraper mdl = null;
            mdlSearchResponse mdlS = null;
            string tboUrl = _config["Travel:TripJack:API:Search"];

            string jsonString = JsonConvert.SerializeObject(SearchRequestMap(request));
            var HaveResponse = GetResponseZip(jsonString, tboUrl);
            {
                if (HaveResponse.MessageType == enmMessageType.Success)
                {
                    HaveResponse.Message = HaveResponse.Message.Replace(",\"messages\":[]", "");
                    mdl = (JsonConvert.DeserializeObject<SearchresultWraper>(HaveResponse.Message));
                }
                if (mdl != null)
                {
                    if (mdl.status.success)//success
                    {
                        if (mdl.searchResult?.tripInfos == null)
                        {
                            mdlS = new mdlSearchResponse()
                            {
                                ResponseStatus = 3,
                                Error = new mdlError()
                                {
                                    Code = mdl.status.httpStatus,
                                    Message = "No data found",
                                }
                            };
                        }
                        string TraceId = Guid.NewGuid().ToString();                        
                        List<List<mdlSearchResult>> AllResults = new List<List<mdlSearchResult>>();
                        List<mdlSearchResult> Result1 = new List<mdlSearchResult>();
                        if (mdl.searchResult.tripInfos != null)
                        {
                            if (mdl.searchResult.tripInfos.ONWARD != null)
                            {   
                                Result1.AddRange(SearchResultMap(mdl.searchResult.tripInfos.ONWARD, TraceId));
                            }
                            

                        }
                        if (Result1.Count() > 0)
                        {
                            AllResults.Add(Result1);
                        }
                        mdlS = new mdlSearchResponse()
                        {
                            ServiceProvider = enmServiceProvider.TripJack,
                            TraceId = TraceId,
                            ResponseStatus = 1,
                            Error = new mdlError()
                            {
                                Code = 0,
                                Message = "-"
                            },
                            Origin = request.Segments[0].Origin,
                            Destination = request.Segments[0].Destination,
                            Results = AllResults
                        };

                        await result;
                    }
                    else
                    {
                        mdlS = new mdlSearchResponse()
                        {
                            ResponseStatus = 3,
                            Error = new mdlError()
                            {
                                Code = mdl.status.httpStatus,
                                Message = "Invalid Request",
                            }
                        };
                    }

                }
                else
                {
                    mdlS = new mdlSearchResponse()
                    {
                        ResponseStatus = 100,
                        Error = new mdlError()
                        {
                            Code = 100,
                            Message = "Unable to Process",
                        }
                    };
                }
            }

            return mdlS;
        }


        #region ******************** Inner classes **********************
        #region ****************** Request ******************************

        public class SearchqueryWraper
        {
            public Searchquery searchQuery { get; set; }
        }

        public class Searchquery
        {
            public string cabinClass { get; set; }
            public Paxinfo paxInfo { get; set; }
            public Routeinfo[] routeInfos { get; set; }
            public Searchmodifiers searchModifiers { get; set; }
            public cityorairport[] preferredAirline { get; set; }
        }

        public class Paxinfo
        {
            public int ADULT { get; set; }
            public int CHILD { get; set; }
            public int INFANT { get; set; }
        }

        public class Searchmodifiers
        {
            public bool isDirectFlight { get; set; }
            public bool isConnectingFlight { get; set; }
        }

        public class Routeinfo
        {
            public cityorairport fromCityOrAirport { get; set; }
            public cityorairport toCityOrAirport { get; set; }
            public string travelDate { get; set; }
        }

        public class cityorairport
        {
            public string code { get; set; }
        }


        #endregion

        #region *************** Response **********************************

        public class Error
        {
            public string errCode { get; set; }
            public string message { get; set; }
            public string details { get; set; }
        }

        public class SearchresultWraper
        {
            public Searchresult searchResult { get; set; }
            public Status status { get; set; }
            public Metainfo metaInfo { get; set; }
        }


        public class SearchresultMulticity
        {
            public ONWARD_RETURN_COMBO[] tripInfos { get; set; }
        }

        public class Searchresult
        {
            public Tripinfos tripInfos { get; set; }
            public Alert[] alerts { get; set; }
            public Searchquery searchQuery { get; set; }
            public string bookingId { get; set; }
            public Totalpriceinfo totalPriceInfo { get; set; }
            public Status status { get; set; }
            public Conditions conditions { get; set; }
        }

        public class Tripinfos
        {
            public ONWARD_RETURN_COMBO[] ONWARD { get; set; }
            public ONWARD_RETURN_COMBO[] RETURN { get; set; }
            public ONWARD_RETURN_COMBO[] COMBO { get; set; }
            [JsonProperty(PropertyName = "0")]
            public ONWARD_RETURN_COMBO[] _0 { get; set; }
            [JsonProperty(PropertyName = "1")]
            public ONWARD_RETURN_COMBO[] _1 { get; set; }
            [JsonProperty(PropertyName = "2")]
            public ONWARD_RETURN_COMBO[] _2 { get; set; }
            [JsonProperty(PropertyName = "3")]
            public ONWARD_RETURN_COMBO[] _3 { get; set; }
            [JsonProperty(PropertyName = "4")]
            public ONWARD_RETURN_COMBO[] _4 { get; set; }
            [JsonProperty(PropertyName = "5")]
            public ONWARD_RETURN_COMBO[] _5 { get; set; }
            [JsonProperty(PropertyName = "6")]
            public ONWARD_RETURN_COMBO[] _6 { get; set; }

        }

        public class ONWARD_RETURN_COMBO
        {
            public Si[] sI { get; set; }
            public Totalpricelist[] totalPriceList { get; set; }

        }

        public class Si
        {
            public int id { get; set; }
            public Fd fD { get; set; }
            public int stops { get; set; }
            public So[] so { get; set; }
            public int duration { get; set; }
            public int cT { get; set; }
            public Da da { get; set; }
            public Aa aa { get; set; }
            public DateTime dt { get; set; }
            public DateTime at { get; set; }
            public bool iand { get; set; }
            public int sN { get; set; }
            public Ob oB { get; set; }
            public mdlSsrInfo ssrInfo { get; set; }
        }

        public class Fd
        {
            public Ai aI { get; set; }
            public string fN { get; set; }
            public string eT { get; set; }
        }

        public class Ai
        {
            public string code { get; set; }
            public string name { get; set; }
            public bool isLcc { get; set; }
        }

        public class Da
        {
            public string code { get; set; }
            public string name { get; set; }
            public string cityCode { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string countryCode { get; set; }
            public string terminal { get; set; }
        }

        public class Aa
        {
            public string code { get; set; }
            public string name { get; set; }
            public string cityCode { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string countryCode { get; set; }
            public string terminal { get; set; }
        }

        public class Ob
        {
            public string code { get; set; }
            public string name { get; set; }
            public bool isLcc { get; set; }
        }

        public class So
        {
            public string code { get; set; }
            public string name { get; set; }
            public string cityCode { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string countryCode { get; set; }
        }

        public class Totalpricelist
        {
            public Fd1 fd { get; set; }
            public string fareIdentifier { get; set; }
            public string id { get; set; }
            public string[] messages { get; set; }
            public string[] msri { get; set; }
            public string sri { get; set; }
            public mdlFareRule farerule { get; set; }
        }

        public class Fd1
        {
            public Passenger INFANT { get; set; }
            public Passenger CHILD { get; set; }
            public Passenger ADULT { get; set; }
        }

        public class Passenger
        {
            public Fc fC { get; set; }
            public Afc afC { get; set; }
            public int sR { get; set; }
            public Bi bI { get; set; }
            public int rT { get; set; }
            public string cc { get; set; }
            public string cB { get; set; }
            public string fB { get; set; }
            public bool mi { get; set; }

        }


        public class Fc
        {
            public double TAF { get; set; }
            public double NF { get; set; }
            public double BF { get; set; }
            public double TF { get; set; }
            public double IGST { get; set; }
            public double NCM { get; set; }
            public double SSRP { get; set; }

        }

        public class Afc
        {
            public TAF TAF { get; set; }
            public NCM NCM { get; set; }
        }
        public class NCM
        {
            public double TDS { get; set; }
            public double OC { get; set; }
        }
        public class TAF
        {
            public double MF { get; set; }
            public double OT { get; set; }
            public double MFT { get; set; }
            public double AGST { get; set; }
            public double YQ { get; set; }
            public double YR { get; set; }
            public double WO { get; set; }
        }

        public class Bi
        {
            public string iB { get; set; }
            public string cB { get; set; }
        }

        public class Status
        {
            public bool success { get; set; }
            public int httpStatus { get; set; }
        }

        public class Metainfo
        {
        }

        public class SsrInfo
        {
            public SsrServices[] SEAT { get; set; }
            public SsrServices[] BAGGAGE { get; set; }
            public SsrServices[] MEAL { get; set; }
            public SsrServices[] EXTRASERVICES { get; set; }
        }
        public class SsrServices
        {
            public string code { get; set; }
            public double amount { get; set; }
            public string desc { get; set; }

        }

        public class Alert
        {
            public double oldFare { get; set; }
            public double newFare { get; set; }
            public string type { get; set; }
        }
        public class Pc
        {
            public string code { get; set; }
            public string name { get; set; }
            public bool isLcc { get; set; }
        }
        public class pcs
        {
            public bool pped { get; set; }
            public bool pid { get; set; }
            public bool pm { get; set; }

        }

        public class Conditions
        {
            public pcs pcs { get; set; }
            public object[] ffas { get; set; }
            public bool isa { get; set; }
            public Dob dob { get; set; }
            public bool isBA { get; set; }
            public int st { get; set; }
            public DateTime sct { get; set; }
            public Gst gst { get; set; }
        }
        public class Dob
        {
            public bool adobr { get; set; }
            public bool cdobr { get; set; }
            public bool idobr { get; set; }
        }

        public class Gst
        {
            public bool gstappl { get; set; }
            public bool igm { get; set; }
        }

        public class Totalpriceinfo
        {
            public Totalfaredetail totalFareDetail { get; set; }
        }

        public class Totalfaredetail
        {
            public Fc fC { get; set; }
            public Afc afC { get; set; }
        }


        #endregion

        #endregion


    }
}

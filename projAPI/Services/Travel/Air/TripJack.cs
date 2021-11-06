using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using projAPI.Model;
using projAPI.Model.Travel;
using projContext;
using projContext.DB.CRM.Travel;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace projAPI.Services.Travel.Air
{
    public interface ITripJack : IWingFlight
    {
        
    }

    public class TripJack : ITripJack
    {
        private readonly TravelContext _context;
        private readonly IConfiguration _config;
        public TripJack(TravelContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        private mdlReturnData GetResponse(string requestData, string url)
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
        public async Task<mdlSearchResponse> SearchAsync(mdlSearchRequest request)
        {
            var mdl = GetDbSearch(request);
            if (mdl == null)
            {
                mdl = await GetApiSearch(request);
                if (mdl.ResponseStatus == enmMessageType.Success)
                {
                    SetDbSearch(mdl, request);
                    return GetDbSearch(request);
                }
            }
            return mdl;
        }
        private SearchqueryWraper SearchRequestMap(mdlSearchRequest request)
        {
            enmCabinClass enmCabin = request.Segments[0].FlightCabinClass;
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

        private List<mdlSearchResult> SearchResultMap(ONWARD_RETURN_COMBO[] sr, string Traceid)
        {
            List<mdlSearchResult> mdls = new List<mdlSearchResult>();
            mdls.AddRange(sr.Select(p => new mdlSearchResult
            {
                ServiceProvider = enmServiceProvider.TripJack,
                traceid = Traceid,

                Segment = p.sI.Select(q => new mdlSegment
                {
                    Airline = new mdlAirline()
                    {
                        Code = q.fD?.aI?.code,
                        Name = q.fD?.aI?.name,
                        isLcc = q.fD?.aI?.isLcc ?? false,
                        FlightNumber = q.fD?.fN ?? string.Empty,
                        OperatingCarrier = q.oB?.code ?? string.Empty,
                    },
                    Id = q.id,
                    ArrivalTime = q.at,
                    DepartureTime = q.dt,
                    Duration = q.duration,
                    Mile = 0,
                    TripIndicator = q.sN,
                    Layover = q.cT,
                    Origin = new mdlAirport()
                    {
                        AirportCode = q.da?.code ?? string.Empty,
                        AirportName = q.da?.name ?? string.Empty,
                        CityCode = q.da?.cityCode ?? string.Empty,
                        CityName = q.da?.city ?? string.Empty,
                        CountryCode = q.da?.countryCode ?? string.Empty,
                        CountryName = q.da?.country ?? string.Empty,
                        Terminal = q.da?.terminal ?? string.Empty,
                    },
                    Destination = new mdlAirport()
                    {
                        AirportCode = q.aa?.code ?? string.Empty,
                        AirportName = q.aa?.name ?? string.Empty,
                        CityCode = q.aa?.cityCode ?? string.Empty,
                        CityName = q.aa?.city ?? string.Empty,
                        CountryCode = q.aa?.countryCode ?? string.Empty,
                        CountryName = q.aa?.country ?? string.Empty,
                        Terminal = q.aa?.terminal ?? string.Empty,
                    },
                    sinfo = new mdlSsrInfo()
                    {
                        MEAL = q?.ssrInfo?.MEAL ?? null,
                        BAGGAGE = q?.ssrInfo?.BAGGAGE ?? null,
                        SEAT = q?.ssrInfo?.SEAT ?? null,
                        EXTRASERVICES = q?.ssrInfo?.EXTRASERVICES ?? null,
                    }

                }).ToList(),
                TotalPriceList = p.totalPriceList.Select(q => new mdlTotalpricelist
                {
                    Identifier = q.fareIdentifier,
                    ResultIndex = q.id,
                    sri = q.sri,
                    msri = q.msri == null ? new List<string>() : q.msri.ToList(),
                    CabinClass = (enmCabinClass)Enum.Parse(typeof(enmCabinClass), q.fd?.ADULT?.cc ?? (nameof(enmCabinClass.ECONOMY)), true),
                    ClassOfBooking = q.fd?.ADULT?.cB ?? string.Empty,
                    ADULT = new mdlTotalpriceDetail()
                    {
                        BaseFare = q.fd?.ADULT?.fC?.BF ?? 0,
                        Tax = q.fd?.ADULT?.fC?.TAF ?? 0,
                        WingMarkup = 0,
                        TotalFare = q.fd?.ADULT?.fC?.TF ?? 0,
                        Discount = 0,
                        NetFare = q.fd?.ADULT?.fC?.NF ?? 0,
                        CabinBaggage = q.fd?.ADULT?.bI?.cB ?? string.Empty,
                        CheckingBaggage = q.fd?.ADULT?.bI?.iB ?? string.Empty,
                        IsFreeMeal = q.fd?.ADULT?.mi ?? false,
                        IsRefundable = Convert.ToByte(q.fd?.ADULT?.rT ?? 0),
                    },
                    CHILD = new mdlTotalpriceDetail()
                    {
                        BaseFare = q.fd?.CHILD?.fC?.BF ?? 0,
                        Tax = q.fd?.CHILD?.fC?.TAF ?? 0,
                        WingMarkup = 0,
                        TotalFare = q.fd?.CHILD?.fC?.TF ?? 0,
                        Discount = 0,
                        NetFare = q.fd?.CHILD?.fC?.NF ?? 0,
                        CabinBaggage = q.fd?.CHILD?.bI?.cB ?? string.Empty,
                        CheckingBaggage = q.fd?.CHILD?.bI?.iB ?? string.Empty,
                        IsFreeMeal = q.fd?.CHILD?.mi ?? false,
                        IsRefundable = Convert.ToByte(q.fd?.CHILD?.rT ?? 0),
                    },
                    INFANT = new mdlTotalpriceDetail()
                    {
                        BaseFare = q.fd?.INFANT?.fC?.BF ?? 0,
                        Tax = q.fd?.INFANT?.fC?.TAF ?? 0,
                        WingMarkup = 0,
                        TotalFare = q.fd?.INFANT?.fC?.TF ?? 0,
                        Discount = 0,
                        NetFare = q.fd?.INFANT?.fC?.NF ?? 0,
                        CabinBaggage = q.fd?.INFANT?.bI?.cB ?? string.Empty,
                        CheckingBaggage = q.fd?.INFANT?.bI?.iB ?? string.Empty,
                        IsFreeMeal = q.fd?.INFANT?.mi ?? false,
                        IsRefundable = Convert.ToByte(q.fd?.INFANT?.rT ?? 0),
                    },
                    FareRule = new mdlFareRuleResponse()
                    {
                        FareRule = q.farerule
                    }
                }
                   ).ToList()
            }));
            return mdls;
        }

        private async Task<mdlSearchResponse> GetApiSearch(mdlSearchRequest request)
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
                                ResponseStatus = enmMessageType.Error,
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
                            ResponseStatus = enmMessageType.Success,
                            Error = new mdlError()
                            {
                                Code = 0,
                                Message = "-"
                            },
                            Origin = request.Segments[0].Origin,
                            Destination = request.Segments[0].Destination,
                            Results = AllResults
                        };

                        return mdlS;
                    }
                    else
                    {
                        mdlS = new mdlSearchResponse()
                        {
                            ResponseStatus = enmMessageType.Error,
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
                        ResponseStatus = enmMessageType.Error,
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


        private mdlSearchResponse GetDbSearch(mdlSearchRequest request)
        {
            mdlSearchResponse mdl = null;
            DateTime CurrentTime = DateTime.Now;
            var segment = request.Segments.FirstOrDefault();
            if (request.JourneyType != enmJourneyType.OneWay)
            {
                return null;
            }
            var tempData = _context.tblFlightSearchRequest_Caching.Where(p => p.ServiceProvider == enmServiceProvider.TripJack && p.ExpiredDt >= DateTime.Now
                 && p.AdultCount == request.AdultCount && p.ChildCount == request.ChildCount && p.InfantCount == request.InfantCount
                 && p.FlightCabinClass == segment.FlightCabinClass && p.Destination == segment.Destination && p.Origin == segment.Origin
                 && !p.IsDeleted
                ).OrderByDescending(p => p.ExpiredDt).FirstOrDefault();
            if (tempData == null)
            {
                return null;
            }
            var tempDataRes = _context.tblFlightSearchResponses_Caching.Where(p => p.ResponseId == tempData.CachingId).Include(q => q.tblFlightSearchSegment_Caching)
               .Include(q => q.tblFlightFare_Caching).ThenInclude(p => p.tblFlightFareDetail_Caching_Adult)
               .Include(q => q.tblFlightFare_Caching).ThenInclude(p => p.tblFlightFareDetail_Caching_Child)
               .Include(q => q.tblFlightFare_Caching).ThenInclude(p => p.tblFlightFareDetail_Caching_Infant)
               .ToList();
            List<mdlSearchResult> AllResults = new List<mdlSearchResult>();
            List<List<mdlSearchResult>> tempResults = new List<List<mdlSearchResult>>();
            foreach (var tempDataRe in tempDataRes)
            {
                mdlSearchResult SearchResult = new mdlSearchResult();
                SearchResult.Segment = new List<mdlSegment>();
                SearchResult.TotalPriceList = new List<mdlTotalpricelist>();
                SearchResult.ServiceProvider = tempData.ServiceProvider;
                SearchResult.traceid = tempData.ProviderTraceId;
                SearchResult.Segment.AddRange(tempDataRe.tblFlightSearchSegment_Caching.Select(p => new mdlSegment
                {
                    Airline = new mdlAirline() { Code = p.Code, FlightNumber = p.FlightNumber, isLcc = p.isLcc, Name = p.Name, OperatingCarrier = p.OperatingCarrier },
                    Destination = new mdlAirport()
                    {
                        AirportCode = p.DestinationAirportCode,
                        AirportName = p.DestinationAirportName,
                        CityCode = p.DestinationCityCode,
                        CityName = p.DestinationCityName,
                        CountryCode = p.DestinationCountryCode,
                        CountryName = p.DestinationCountryName,
                        Terminal = p.DestinationTerminal
                    },
                    Origin = new mdlAirport()
                    {
                        AirportCode = p.OriginAirportCode,
                        AirportName = p.OriginAirportName,
                        CityCode = p.OriginCityCode,
                        CityName = p.OriginCityName,
                        CountryCode = p.OriginCountryCode,
                        CountryName = p.OriginCountryName,
                        Terminal = p.OriginTerminal
                    },
                    ArrivalTime = p.ArrivalTime,
                    DepartureTime = p.DepartureTime,
                    Duration = p.Duration,
                    Mile = p.Mile,
                    TripIndicator = p.TripIndicator,
                    Layover = p.Layover,
                }));
                SearchResult.TotalPriceList.AddRange(tempDataRe.tblFlightFare_Caching.Select(p => new mdlTotalpricelist
                {
                    ADULT = new mdlTotalpriceDetail()
                    {
                        BaseFare = p.tblFlightFareDetail_Caching_Adult.BaseFare,
                        YQTax = p.tblFlightFareDetail_Caching_Adult.YQTax,
                        Tax = p.tblFlightFareDetail_Caching_Adult.Tax,
                        WingMarkup = p.tblFlightFareDetail_Caching_Adult.WingMarkup,
                        TotalFare = p.tblFlightFareDetail_Caching_Adult.TotalFare,
                        Discount = p.tblFlightFareDetail_Caching_Adult.Discount,
                        NetFare = p.tblFlightFareDetail_Caching_Adult.NetFare,
                        CheckingBaggage = p.tblFlightFareDetail_Caching_Adult.CheckingBaggage,
                        CabinBaggage = p.tblFlightFareDetail_Caching_Adult.CabinBaggage,
                        IsFreeMeal = p.tblFlightFareDetail_Caching_Adult.IsFreeMeal,
                        IsRefundable = p.tblFlightFareDetail_Caching_Adult.IsRefundable,
                    },
                    CHILD = new mdlTotalpriceDetail()
                    {
                        BaseFare = p.tblFlightFareDetail_Caching_Child?.BaseFare ?? 0,
                        YQTax = p.tblFlightFareDetail_Caching_Child?.YQTax ?? 0,
                        Tax = p.tblFlightFareDetail_Caching_Child?.Tax ?? 0,
                        WingMarkup = p.tblFlightFareDetail_Caching_Child?.WingMarkup ?? 0,
                        TotalFare = p.tblFlightFareDetail_Caching_Child?.TotalFare ?? 0,
                        Discount = p.tblFlightFareDetail_Caching_Child?.Discount ?? 0,
                        NetFare = p.tblFlightFareDetail_Caching_Child?.NetFare ?? 0,
                        CheckingBaggage = p.tblFlightFareDetail_Caching_Child.CheckingBaggage ?? string.Empty,
                        CabinBaggage = p.tblFlightFareDetail_Caching_Child.CabinBaggage ?? string.Empty,
                        IsFreeMeal = p.tblFlightFareDetail_Caching_Child?.IsFreeMeal ?? false,
                        IsRefundable = p.tblFlightFareDetail_Caching_Child?.IsRefundable ?? 0,
                    },
                    INFANT = new mdlTotalpriceDetail()
                    {
                        BaseFare = p.tblFlightFareDetail_Caching_Infant?.BaseFare ?? 0,
                        YQTax = p.tblFlightFareDetail_Caching_Infant?.YQTax ?? 0,
                        Tax = p.tblFlightFareDetail_Caching_Infant?.Tax ?? 0,
                        WingMarkup = p.tblFlightFareDetail_Caching_Infant?.WingMarkup ?? 0,
                        TotalFare = p.tblFlightFareDetail_Caching_Infant?.TotalFare ?? 0,
                        Discount = p.tblFlightFareDetail_Caching_Infant?.Discount ?? 0,
                        NetFare = p.tblFlightFareDetail_Caching_Infant?.NetFare ?? 0,
                        CheckingBaggage = p.tblFlightFareDetail_Caching_Infant.CheckingBaggage ?? string.Empty,
                        CabinBaggage = p.tblFlightFareDetail_Caching_Infant.CabinBaggage ?? string.Empty,
                        IsFreeMeal = p.tblFlightFareDetail_Caching_Infant?.IsFreeMeal ?? false,
                        IsRefundable = p.tblFlightFareDetail_Caching_Infant?.IsRefundable ?? 0,
                    },

                    BaseFare = p.BaseFare,
                    CustomerMarkup = p.CustomerMarkup,
                    WingMarkup = p.WingMarkup,
                    Convenience = p.Convenience,
                    TotalFare = p.TotalFare,
                    Discount = p.Discount,
                    PromoCode = p.PromoCode,
                    PromoDiscount = p.PromoDiscount,
                    NetFare = p.NetFare,
                    ResultIndex = p.ProviderFareDetailId,
                    Identifier = p.Identifier,
                    SeatRemaning = p.SeatRemaning,
                    CabinClass = p.CabinClass,
                    ClassOfBooking = p.ClassOfBooking,
                }));
                AllResults.Add(SearchResult);
            }
            tempResults.Add(AllResults);

            mdl = new mdlSearchResponse()
            {
                Origin = request.Segments.FirstOrDefault().Origin,
                Destination = request.Segments.FirstOrDefault().Destination,
                WingSearchId = tempData.CachingId,
                ResponseStatus = enmMessageType.Success,
                ServiceProvider = tempData.ServiceProvider,
                TraceId = tempData.ProviderTraceId,
                Results = tempResults

            };
            return mdl;




        }

        private bool SetDbSearch(mdlSearchResponse resoponse, mdlSearchRequest request)
        {
            if (request == null || request.Segments == null || request.Segments.Count == 0 || resoponse.ResponseStatus != enmMessageType.Success)
            {
                return false;
            }
            int ExpiryTime = 30;
            int.TryParse(_config["Travel:TripJack:TraceIdExpiryTime"], out ExpiryTime);


            List<tblFlightSearchResponses_Caching> SearchResponses = new List<tblFlightSearchResponses_Caching>();

            var SearchResult = resoponse.Results.FirstOrDefault();
            if (SearchResult == null || SearchResult.Count == 0)
            {
                return false;
            };

            SearchResult.ForEach(p =>
            {

                List<tblFlightSearchSegment_Caching> FlightSearchSegment = new List<tblFlightSearchSegment_Caching>();
                FlightSearchSegment.AddRange(p.Segment.Select(q => new tblFlightSearchSegment_Caching
                {
                    Code = q.Airline.Code,
                    Name = q.Airline.Name,
                    FlightNumber = q.Airline.FlightNumber,
                    OperatingCarrier = q.Airline.OperatingCarrier,
                    AirlineType = string.Empty,
                    OriginAirportCode = q.Origin.AirportCode,
                    OriginAirportName = q.Origin.AirportName,
                    OriginCityCode = q.Origin.CityCode,
                    OriginCityName = q.Origin.CityName,
                    OriginCountryCode = q.Origin.CountryCode,
                    OriginCountryName = q.Origin.CountryName,
                    DestinationAirportCode = q.Destination.AirportCode,
                    DestinationAirportName = q.Destination.AirportName,
                    DestinationCityCode = q.Destination.CityCode,
                    DestinationCityName = q.Destination.CityName,
                    DestinationCountryCode = q.Destination.CountryCode,
                    DestinationCountryName = q.Destination.CountryName,
                    ArrivalTime = q.ArrivalTime,
                    DepartureTime = q.DepartureTime,
                    DestinationTerminal = q.Destination.Terminal,
                    OriginTerminal = q.Origin.Terminal,
                    Duration = q.Duration,
                    isLcc = q.Airline.isLcc,
                    Layover = q.Layover,
                    Mile = q.Mile,
                    TripIndicator = q.TripIndicator,
                }));
                List<tblFlightFare_Caching> FlightFare = new List<tblFlightFare_Caching>();
                FlightFare.AddRange(p.TotalPriceList.Select(q => new tblFlightFare_Caching
                {
                    tblFlightFareDetail_Caching_Adult = new tblFlightFareDetail_Caching()
                    {
                        BaseFare = q.ADULT.BaseFare,
                        YQTax = q.ADULT.YQTax,
                        Tax = q.ADULT.Tax,
                        WingMarkup = q.ADULT.WingMarkup,
                        TotalFare = q.ADULT.TotalFare,
                        Discount = q.ADULT.Discount,
                        NetFare = q.ADULT.NetFare,
                        CheckingBaggage = q.ADULT.CheckingBaggage,
                        CabinBaggage = q.ADULT.CabinBaggage,
                        IsFreeMeal = q.ADULT.IsFreeMeal,
                        IsRefundable = q.ADULT.IsRefundable
                    },
                    tblFlightFareDetail_Caching_Child = new tblFlightFareDetail_Caching()
                    {
                        BaseFare = q.CHILD?.BaseFare ?? 0,
                        YQTax = q.CHILD?.YQTax ?? 0,
                        Tax = q.CHILD?.Tax ?? 0,
                        WingMarkup = q.CHILD?.WingMarkup ?? 0,
                        TotalFare = q.CHILD?.TotalFare ?? 0,
                        Discount = q.CHILD?.Discount ?? 0,
                        NetFare = q.CHILD?.NetFare ?? 0,
                        CheckingBaggage = q.CHILD.CheckingBaggage ?? string.Empty,
                        CabinBaggage = q.CHILD.CabinBaggage ?? string.Empty,
                        IsFreeMeal = q.CHILD?.IsFreeMeal ?? false,
                        IsRefundable = q.CHILD?.IsRefundable ?? 0
                    },
                    tblFlightFareDetail_Caching_Infant = new tblFlightFareDetail_Caching()
                    {
                        BaseFare = q.INFANT?.BaseFare ?? 0,
                        YQTax = q.INFANT?.YQTax ?? 0,
                        Tax = q.INFANT?.Tax ?? 0,
                        WingMarkup = q.INFANT?.WingMarkup ?? 0,
                        TotalFare = q.INFANT?.TotalFare ?? 0,
                        Discount = q.INFANT?.Discount ?? 0,
                        NetFare = q.INFANT?.NetFare ?? 0,
                        CheckingBaggage = q.INFANT.CheckingBaggage ?? string.Empty,
                        CabinBaggage = q.INFANT.CabinBaggage ?? string.Empty,
                        IsFreeMeal = q.INFANT?.IsFreeMeal ?? false,
                        IsRefundable = q.INFANT?.IsRefundable ?? 0
                    },

                    ProviderFareDetailId = q.ResultIndex,
                    Identifier = q.Identifier,
                    SeatRemaning = q.SeatRemaning,
                    CabinClass = q.CabinClass,
                    ClassOfBooking = q.ClassOfBooking,
                    BaseFare = q.BaseFare,
                    CustomerMarkup = q.CustomerMarkup,
                    WingMarkup = q.WingMarkup,
                    Convenience = q.Convenience,
                    TotalFare = q.TotalFare,
                    Discount = q.Discount,
                    PromoCode = q.PromoCode,
                    PromoDiscount = q.PromoCode,
                    NetFare = q.NetFare,


                }));
                tblFlightSearchResponses_Caching sr = new tblFlightSearchResponses_Caching()
                {
                    tblFlightSearchSegment_Caching = FlightSearchSegment,
                    tblFlightFare_Caching = FlightFare
                };
                SearchResponses.Add(sr);
            });

            tblFlightSearchRequest_Caching mdl = new tblFlightSearchRequest_Caching()
            {

                CachingId = Convert.ToString(Guid.NewGuid()).Replace("-", ""),
                JourneyType = request.JourneyType,
                Origin = request.Segments.FirstOrDefault().Origin,
                Destination = request.Segments.FirstOrDefault().Destination,
                AdultCount = request.AdultCount,
                ChildCount = request.ChildCount,
                InfantCount = request.InfantCount,
                FlightCabinClass = request.Segments.FirstOrDefault().FlightCabinClass,
                MinmumPrice = 0,
                TravelDt = request.Segments.FirstOrDefault().TravelDt,
                CreatedDt = DateTime.Now,
                ExpiredDt = DateTime.Now.AddMinutes(ExpiryTime),
                ProviderTraceId = resoponse.TraceId,
                ServiceProvider = enmServiceProvider.TripJack,
                tblFlightSearchResponses_Caching = SearchResponses
            };
            _context.tblFlightSearchRequest_Caching.Add(mdl);
            _context.SaveChanges();
            return true;
        }


        private FareQuotRequest FareQuoteRequestMap(mdlFareQuotRequest request)
        {
            string[] ri = { request.ResultIndex };
            FareQuotRequest mdl = new FareQuotRequest()
            {
                priceIds = ri
            };
            return mdl;
        }


        public async Task<mdlFareQuotResponse> FareQuoteAsync(mdlFareQuotRequest request)
        {
            
            mdlFareQuotResponse mdlS = null;
            FareQuotResponse mdl = null;
            DateTime DepartureDt = DateTime.Now, ArrivalDt = DateTime.Now;
            string tboUrl = _config["Travel:TripJack:API:FareQuote"];            
            string jsonString = JsonConvert.SerializeObject(FareQuoteRequestMap(request));
            var HaveResponse = GetResponse(jsonString, tboUrl);
            if (HaveResponse.MessageType == enmMessageType.Success)
            {
                mdl = (JsonConvert.DeserializeObject<FareQuotResponse>(HaveResponse.Message));
            }

            if (mdl != null)
            {

                if (mdl.status.success)//success
                {
                    List<List<mdlSearchResult>> AllResults = new List<List<mdlSearchResult>>();
                    List<mdlSearchResult> Result1 = new List<mdlSearchResult>();
                    int ServiceProvider = (int)enmServiceProvider.TripJack;
                    if (mdl.tripInfos != null)
                    {
                        mdlFareRuleRequest mfr = new mdlFareRuleRequest();
                        for (int i = 0; i < mdl.tripInfos.FirstOrDefault().totalPriceList.Count(); i++)
                        {
                            mfr.id = mdl.tripInfos?.FirstOrDefault()?.totalPriceList[i].id;
                            mfr.flowType = "REVIEW";
                            //var mfs = FareRuleAsync(mfr);
                            //mdl.tripInfos.FirstOrDefault().totalPriceList[i].farerule = mfs.Result.FareRule;
                        }
                        Result1.AddRange(SearchResultMap(mdl.tripInfos, request.TraceId));
                    }
                    if (Result1.Count() > 0)
                    {
                        AllResults.Add(Result1);
                    }
                    DateTime.TryParse(mdl.searchQuery?.routeInfos?.FirstOrDefault()?.travelDate, out DepartureDt);
                    mdlS = new mdlFareQuotResponse()
                    {

                        ServiceProvider = enmServiceProvider.TripJack,
                        TraceId = request.TraceId,
                        BookingId = ServiceProvider + "_" + mdl.bookingId,
                        ResponseStatus =  enmMessageType.Success,
                        IsPriceChanged = mdl.alerts?.Any(p => p.oldFare != p.newFare) ?? false,
                        Error = new mdlError()
                        {
                            Code = 0,
                            Message = ""
                        },
                        Origin = mdl.searchQuery.routeInfos.FirstOrDefault()?.fromCityOrAirport.code,
                        Destination = mdl.searchQuery.routeInfos.FirstOrDefault()?.toCityOrAirport.code,
                        Results = AllResults,
                        TotalPriceInfo = new mdlTotalPriceInfo()
                        {
                            BaseFare = mdl.totalPriceInfo?.totalFareDetail?.fC?.BF ?? 0,
                            TaxAndFees = mdl.totalPriceInfo?.totalFareDetail?.fC?.TAF ?? 0,
                            TotalFare = mdl.totalPriceInfo?.totalFareDetail?.fC?.TF ?? 0,
                            NetFare= mdl.totalPriceInfo?.totalFareDetail?.fC.NF??0
                        },
                        SearchQuery = new  mdlFlightSearchWraper()
                        {
                            AdultCount = mdl.searchQuery?.paxInfo?.ADULT ?? 0,
                            ChildCount = mdl.searchQuery?.paxInfo?.CHILD ?? 0,
                            InfantCount = mdl.searchQuery?.paxInfo?.INFANT ?? 0,
                            CabinClass = (enmCabinClass)Enum.Parse(typeof(enmCabinClass), mdl.searchQuery?.cabinClass ?? (nameof(enmCabinClass.ECONOMY)), true),
                            JourneyType = enmJourneyType.OneWay,
                            DepartureDt = DepartureDt,
                            From = mdl.searchQuery?.routeInfos?.FirstOrDefault()?.fromCityOrAirport?.code,
                            To = mdl.searchQuery?.routeInfos?.FirstOrDefault()?.toCityOrAirport?.code
                        },
                        FareQuoteCondition = new mdlFareQuoteCondition()
                        {
                            dob = new mdlDobCondition()
                            {
                                adobr = mdl.conditions?.dob?.adobr ?? false,
                                cdobr = mdl.conditions?.dob?.cdobr ?? false,
                                idobr = mdl.conditions?.dob?.idobr ?? false,
                            },
                            GstCondition = new mdlGstCondition()
                            {
                                IsGstMandatory = mdl.conditions?.gst?.igm ?? false,
                                IsGstApplicable = mdl.conditions?.gst?.gstappl ?? true,
                            },
                            IsHoldApplicable = mdl.conditions?.isBA ?? false,
                            PassportCondition = new mdlPassportCondition()
                            {
                                IsPassportExpiryDate = mdl.conditions?.pcs?.pped ?? false,
                                isPassportIssueDate = mdl.conditions?.pcs?.pid ?? false,
                                isPassportRequired = mdl.conditions?.pcs?.pm ?? false,
                            }

                        }

                    };
                }
                else
                {
                    mdlS = new mdlFareQuotResponse()
                    {
                        ResponseStatus = enmMessageType.Success,
                        Error = new mdlError()
                        {
                            Code = mdl.status.httpStatus,
                            Message = "Unable to Process",
                        }
                    };
                }

            }
            else
            {
                if (HaveResponse == null)
                {
                    mdlS = new mdlFareQuotResponse()
                    {
                        ResponseStatus = enmMessageType.Error,
                        Error = new mdlError()
                        {
                            Code = 100,
                            Message = "Unable to Process",
                        }
                    };
                }
                else
                {
                    mdlS = new mdlFareQuotResponse()
                    {
                        ResponseStatus = enmMessageType.Error,
                        Error = new mdlError()
                        {
                            Code = 100,
                            Message = HaveResponse.Message,
                        }
                    };
                }
            }

            return mdlS;
        }



        private BookingRequest BookingRequestMap(mdlBookingRequest request)
        {
            GstInfo _gstInfo = null;
            if (request.gstInfo != null)
            {
                _gstInfo = new GstInfo()
                {
                    address = request.gstInfo.address,
                    email = request.gstInfo.email,
                    gstNumber = request.gstInfo.gstNumber,
                    mobile = request.gstInfo.mobile,
                    registeredName = request.gstInfo.registeredName,
                };
            }
            PaymentInfos[] paymentInfos = null;
            if (request.paymentInfos != null)
            {
                paymentInfos = request.paymentInfos.Select(p => new PaymentInfos { amount = p.amount }).ToArray();
            }

            BookingRequest mdl = new BookingRequest()
            {
                bookingId = request.BookingId,
                gstInfo = _gstInfo,
                deliveryInfo = new Deliveryinfo()
                {
                    contacts = request.deliveryInfo?.contacts,
                    emails = request.deliveryInfo?.emails,
                },
                travellerInfo = request.travellerInfo.Select(p => new Travellerinfo
                {
                    ti = p.Title.ToString().ToUpper(),
                    fN = p.FirstName,
                    lN = p.LastName,
                    dob = p.dob.HasValue ? p.dob.Value.ToString("yyyy-MM-dd") : null,
                    eD = p.PassportExpiryDate.ToString("yyyy-MM-dd"),
                    pid = p.PassportIssueDate.ToString("yyyy-MM-dd"),
                    pNum = p.pNum,
                    pt = p.passengerType.ToString().Trim().ToUpper(),
                    ssrBaggageInfos = p.ssrBaggageInfoslist,
                    ssrSeatInfos = p.ssrSeatInfoslist,
                    ssrMealInfos = p.ssrMealInfoslist,
                    ssrExtraServiceInfos = p.ssrExtraServiceInfoslist,

                }).ToArray(),
                paymentInfos = paymentInfos
            };
            return mdl;
        }
        public async Task<mdlBookingResponse> BookingAsync(mdlBookingRequest request)
        {
            mdlBookingResponse mdlS = null;
            BookingResponse mdl = null;
            //set the Upper case in pax type

            string tboUrl = _config["Travel:TripJack:API:Book"];
            string jsonString = JsonConvert.SerializeObject(BookingRequestMap(request));
            var HaveResponse = GetResponse(jsonString, tboUrl);
            if (HaveResponse.MessageType == enmMessageType.Success )
            {
                mdl = (JsonConvert.DeserializeObject<BookingResponse>(HaveResponse.Message));
            }

            if (mdl != null)
            {
                if (mdl.status.success)//success
                {
                    mdlS = new mdlBookingResponse()
                    {
                        bookingId = mdl.bookingId,
                        Error = new mdlError()
                        {
                            Code = 0,
                            Message = ""
                        },
                        ResponseStatus = 1,

                    };
                }
                else
                {
                    mdlS = new mdlBookingResponse()
                    {
                        ResponseStatus = 3,
                        Error = new mdlError()
                        {
                            Code = 12,
                            Message = mdl.errors?.FirstOrDefault()?.message ?? "",
                        }
                    };
                }

            }
            else
            {   
                mdlS = new mdlBookingResponse()
                {
                    ResponseStatus = 100,
                    Error = new mdlError()
                    {
                        Code = 100,
                        Message = HaveResponse.Message,
                    }
                };
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




        #region ************************ Fare Quote Classes****************
        public class FareQuotRequest
        {
            public string[] priceIds { get; set; }
        }
        public class FareQuotResponse : SearchresultMulticity
        {
            public Alert[] alerts { get; set; }
            public Searchquery searchQuery { get; set; }
            public string bookingId { get; set; }
            public Totalpriceinfo totalPriceInfo { get; set; }
            public Status status { get; set; }
            public Conditions conditions { get; set; }
            public Error[] errors { get; set; }
        }

        #endregion

        #region *****************Booking Request classes *************************
        public class BookingRequest
        {
            public string bookingId { get; set; }
            public Travellerinfo[] travellerInfo { get; set; }
            public Deliveryinfo deliveryInfo { get; set; }
            public GstInfo gstInfo { get; set; }
            public PaymentInfos[] paymentInfos { get; set; }
        }

        public class PaymentInfos
        {
            public double amount { get; set; }
        }

        public class Deliveryinfo
        {
            public List<string> emails { get; set; }
            public List<string> contacts { get; set; }
        }

        public class Travellerinfo
        {
            public string ti { get; set; }
            public string fN { get; set; }
            public string lN { get; set; }
            public string pt { get; set; }
            public string dob { get; set; }
            public string pNum { get; set; }
            public string eD { get; set; }
            public string pid { get; set; }
            public List<mdlSSRS> ssrBaggageInfos { get; set; }
            public List<mdlSSRS> ssrMealInfos { get; set; }
            public List<mdlSSRS> ssrSeatInfos { get; set; }
            public List<mdlSSRS> ssrExtraServiceInfos { get; set; }
        }


        public class GstInfo
        {
            public string gstNumber { get; set; }
            public string email { get; set; }
            public string registeredName { get; set; }
            public string mobile { get; set; }
            public string address { get; set; }
        }

        public class SSRS
        {
            public string key { get; set; }
            public string value { get; set; }
        }


        public class BookingResponse
        {
            public string bookingId { get; set; }
            public Status status { get; set; }
            public Metainfo metaInfo { get; set; }
            public Error[] errors { get; set; }
        }
        #endregion

        #endregion


    }
}

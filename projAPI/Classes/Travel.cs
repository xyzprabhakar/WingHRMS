using projAPI.Model;
using projAPI.Model.Travel;
using projContext.DB.CRM.Travel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projContext;
using projAPI.Services;

namespace projAPI.Classes
{
    public class Travel
    {
        private readonly TravelContext _travelContext;
        public Travel(TravelContext travelContext)
        {
            _travelContext = travelContext;
        }
        public mdlReturnData SetFareQuote(mdlFareQuotRequestWraper request, IsrvCurrentUser _IsrvCurrentUser, List<mdlFareQuotResponse> response,int OrgId,ulong Nid)
        {
            bool IsUpdate = false;
            string VisitorId;
            DateTime BookingDt = DateTime.Now; 
            mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.None };
            if (response == null || response.Count==0)
            {
                mdl.MessageType = enmMessageType.Error;
                mdl.Message = enmMessage.DataNotFound.GetDescription();
                return mdl;
            }
            var Master =_travelContext.tblFlightBookingMaster.Where(p => p.VisitorId == request.TraceId).FirstOrDefault();
            var res=response.FirstOrDefault();
            if (Master == null)
            {
                Master = new tblFlightBookingMaster();
                IsUpdate = false;
            }
            else
            {
                IsUpdate = true;
            }   
            Master.AdultCount = res.SearchQuery?.AdultCount ?? 0;
            Master.ChildCount = res.SearchQuery?.ChildCount ?? 0;
            Master.InfantCount = res.SearchQuery?.InfantCount ?? 0;
            Master.BookingDate = BookingDt;
            Master.CabinClass = res.SearchQuery?.CabinClass ?? enmCabinClass.ECONOMY;
            Master.BookingStatus = enmBookingStatus.Pending;
            Master.From = res.SearchQuery?.From ?? string.Empty;
            Master.To = res.SearchQuery?.To ?? string.Empty;
            Master.JourneyType = response.Count == 1 ? enmJourneyType.OneWay : response.Count == 2 ? enmJourneyType.Return : enmJourneyType.MultiStop;
            Master.DepartureDt = res.SearchQuery?.DepartureDt ?? BookingDt;
            Master.ReturnDt = res.SearchQuery?.ReturnDt;
            Master.OrgId = OrgId;
            Master.Nid = Nid;
            if (!IsUpdate)
            {
                Master.VisitorId = request.TraceId;
                Master.CreatedBy = _IsrvCurrentUser.UserId;
                Master.CreatedDt = BookingDt;
                Master.ModifiedBy= _IsrvCurrentUser.UserId;
                Master.ModifiedDt = BookingDt;
                _travelContext.tblFlightBookingMaster.Add(Master);
            }
            else
            {
                Master.ModifiedBy = _IsrvCurrentUser.UserId;
                Master.ModifiedDt = BookingDt;
                _travelContext.tblFlightBookingMaster.Update(Master);
            }
            _travelContext.SaveChanges();
            VisitorId = Master.VisitorId;

            SetFlightBookingSearchDetails(1);
            if (response.Count > 1)
            {
                SetFlightBookingSearchDetails(2);
            }

            mdl.Message = "Save Successfully";
            mdl.MessageType = enmMessageType.Success;
            return mdl;

            void SetFlightBookingSearchDetails(int SegmentId)
            {
                string BookingId= Guid.NewGuid().ToString();
                var tempSerach=_travelContext.tblFlightBookingSearchDetails.Where(p => p.SegmentId == SegmentId && p.VisitorId == VisitorId && p.IsDeleted==false).FirstOrDefault();
                if (tempSerach != null)
                {
                    tempSerach.IsDeleted = true;
                }
                tblFlightBookingSearchDetails mdlSearch = new tblFlightBookingSearchDetails()
                {
                    
                    BookingId = BookingId,
                    VisitorId = VisitorId,
                    SegmentId = SegmentId,
                    PurchaseIdentifier = response[SegmentId - 1].PurchaseIdentifier,
                    PurchaseCabinClass = response[SegmentId - 1].PurchaseCabinClass,
                    PurchaseClassOfBooking = response[SegmentId - 1].PurchaseClassOfBooking,
                    BookedIdentifier = response[SegmentId - 1].BookedIdentifier,
                    BookedCabinClass = response[SegmentId - 1].BookedCabinClass,
                    BookedClassOfBooking = response[SegmentId - 1].BookedClassOfBooking,
                    ProviderBookingId = response[SegmentId - 1].BookingId,
                    ServiceProvider = response[SegmentId - 1].ServiceProvider,
                    BookingStatus = enmBookingStatus.Pending,
                    PaymentStatus= enmPaymentStatus.Pending,
                    IncludeBaggageServices = false,
                    IncludeMealServices = false,
                    IncludeSeatServices = false,
                    PurchaseAmount = response[SegmentId - 1].TotalPriceInfo?.NetFare ?? 0,
                    IncentiveAmount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].MLMMarkup,
                    ConvenienceAmount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].Convenience,
                    MarkupAmount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].WingMarkup,
                    CustomerMarkupAmount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CustomerMarkup,
                    DiscountAmount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].Discount,
                    SaleAmount= response[SegmentId - 1].Results[0][0].TotalPriceList[0].NetFare
                };
                mdlSearch.NetSaleAmount = mdlSearch.SaleAmount + mdlSearch.IncentiveAmount + mdlSearch.ConvenienceAmount
                    + mdlSearch.MarkupAmount - mdlSearch.DiscountAmount;
                if (tempSerach != null)
                {
                    _travelContext.tblFlightBookingSearchDetails.Update(tempSerach);
                }                
                _travelContext.tblFlightBookingSearchDetails.Add(mdlSearch);
                
                foreach(var seg in  response[SegmentId - 1].Results[0][0].Segment)
                {
                    tblFlightSearchSegment fsg = new tblFlightSearchSegment()
                    {
                        ProviderSegmentId= seg.Id.ToString(),
                        BookingId = BookingId,
                        isLcc = seg.Airline.isLcc,
                        Code = seg.Airline.Code,
                        Name = seg.Airline.Name,
                        FlightNumber = seg.Airline.FlightNumber,
                        OperatingCarrier = seg.Airline.OperatingCarrier,
                        OriginAirportCode = seg.Origin.AirportCode,
                        OriginAirportName = seg.Origin.AirportName,
                        OriginTerminal = seg.Origin.Terminal,
                        OriginCityCode = seg.Origin.CityCode,
                        OriginCityName = seg.Origin.CityName,
                        OriginCountryCode = seg.Origin.CountryCode,
                        OriginCountryName = seg.Origin.CountryName,
                        DestinationAirportCode = seg.Destination.AirportCode,
                        DestinationAirportName = seg.Destination.AirportName,
                        DestinationTerminal = seg.Destination.Terminal,
                        DestinationCityCode = seg.Destination.CityCode,
                        DestinationCityName = seg.Destination.CityName,
                        DestinationCountryCode = seg.Destination.CountryCode,
                        DestinationCountryName = seg.Destination.CountryName,
                        TripIndicator = seg.TripIndicator,
                        DepartureTime = seg.DepartureTime,
                        ArrivalTime = seg.ArrivalTime,
                        Mile = seg.Mile,
                        Duration = seg.Duration,
                        Layover = seg.Layover,
                    };
                    _travelContext.tblFlightSearchSegment.Add(fsg);
                }
                tblFlightFare flightFare = new tblFlightFare()
                {
                    BookingId = BookingId,
                    tblFlightFareDetail_Adult = new tblFlightFareDetail()
                    {
                        BaseFare = response[SegmentId - 1].Results[0][0].TotalPriceList[0].ADULT.BaseFare,
                        TotalFare = response[SegmentId - 1].Results[0][0].TotalPriceList[0].ADULT.TotalFare,
                        CabinBaggage = response[SegmentId - 1].Results[0][0].TotalPriceList[0].ADULT.CabinBaggage,
                        CheckingBaggage = response[SegmentId - 1].Results[0][0].TotalPriceList[0].ADULT.CheckingBaggage,
                        Discount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].ADULT.Discount,
                        Tax = response[SegmentId - 1].Results[0][0].TotalPriceList[0].ADULT.Tax,
                        YQTax = response[SegmentId - 1].Results[0][0].TotalPriceList[0].ADULT.YQTax,
                        WingMarkup = response[SegmentId - 1].Results[0][0].TotalPriceList[0].ADULT.WingMarkup,
                        NetFare = response[SegmentId - 1].Results[0][0].TotalPriceList[0].ADULT.NetFare,
                        IsFreeMeal = response[SegmentId - 1].Results[0][0].TotalPriceList[0].ADULT.IsFreeMeal,
                        IsRefundable = response[SegmentId - 1].Results[0][0].TotalPriceList[0].ADULT.IsRefundable,
                        MLMMarkup = response[SegmentId - 1].Results[0][0].TotalPriceList[0].ADULT.MLMMarkup,
                        Convenience = response[SegmentId - 1].Results[0][0].TotalPriceList[0].ADULT.Convenience,
                    },
                    tblFlightFareDetail_Child = new tblFlightFareDetail()
                    {
                        BaseFare = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CHILD.BaseFare,
                        TotalFare = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CHILD.TotalFare,
                        CabinBaggage = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CHILD.CabinBaggage,
                        CheckingBaggage = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CHILD.CheckingBaggage,
                        Discount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CHILD.Discount,
                        Tax = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CHILD.Tax,
                        YQTax = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CHILD.YQTax,
                        WingMarkup = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CHILD.WingMarkup,
                        NetFare = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CHILD.NetFare,
                        IsFreeMeal = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CHILD.IsFreeMeal,
                        IsRefundable = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CHILD.IsRefundable,
                        MLMMarkup = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CHILD.MLMMarkup,
                        Convenience = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CHILD.Convenience,
                    },
                    tblFlightFareDetail_Infant = new tblFlightFareDetail()
                    {
                        BaseFare = response[SegmentId - 1].Results[0][0].TotalPriceList[0].INFANT.BaseFare,
                        TotalFare = response[SegmentId - 1].Results[0][0].TotalPriceList[0].INFANT.TotalFare,
                        CabinBaggage = response[SegmentId - 1].Results[0][0].TotalPriceList[0].INFANT.CabinBaggage,
                        CheckingBaggage = response[SegmentId - 1].Results[0][0].TotalPriceList[0].INFANT.CheckingBaggage,
                        Discount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].INFANT.Discount,
                        Tax = response[SegmentId - 1].Results[0][0].TotalPriceList[0].INFANT.Tax,
                        YQTax = response[SegmentId - 1].Results[0][0].TotalPriceList[0].INFANT.YQTax,
                        WingMarkup = response[SegmentId - 1].Results[0][0].TotalPriceList[0].INFANT.WingMarkup,
                        NetFare = response[SegmentId - 1].Results[0][0].TotalPriceList[0].INFANT.NetFare,
                        IsFreeMeal = response[SegmentId - 1].Results[0][0].TotalPriceList[0].INFANT.IsFreeMeal,
                        IsRefundable = response[SegmentId - 1].Results[0][0].TotalPriceList[0].INFANT.IsRefundable,
                        MLMMarkup = response[SegmentId - 1].Results[0][0].TotalPriceList[0].INFANT.MLMMarkup,
                        Convenience = response[SegmentId - 1].Results[0][0].TotalPriceList[0].INFANT.Convenience,
                    },
                    CustomerMarkup = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CustomerMarkup,
                    Convenience = response[SegmentId - 1].Results[0][0].TotalPriceList[0].Convenience,
                    BaseFare = response[SegmentId - 1].Results[0][0].TotalPriceList[0].BaseFare,
                    Discount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].Discount,
                    PromoCode = response[SegmentId - 1].Results[0][0].TotalPriceList[0].PromoCode,
                    PromoDiscount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].PromoDiscount,
                    WingMarkup = response[SegmentId - 1].Results[0][0].TotalPriceList[0].WingMarkup,
                    NetFare = response[SegmentId - 1].Results[0][0].TotalPriceList[0].NetFare,
                    TotalFare = response[SegmentId - 1].Results[0][0].TotalPriceList[0].TotalFare,
                };
                _travelContext.tblFlightFare.Add(flightFare);
                _travelContext.SaveChanges();
            }


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using projAPI.Model;
using projAPI.Model.Travel;
using projAPI.Services.Travel.Air;
using projContext;
using projContext.DB.CRM.Travel;

namespace projAPI.Services.Travel
{

    public interface IWingFlight
    {
        Task<mdlSearchResponse> SearchAsync(mdlSearchRequest request);
        Task<mdlFareQuotResponse> FareQuoteAsync(mdlFareQuotRequest request);
        //Task<mdlFareRuleResponse> FareRuleAsync(mdlFareRuleRequest request);
        Task<mdlBookingResponse> BookingAsync(mdlBookingRequest request, enmBookingStatus BookingStatus);
        //Task<mdlFlightCancellationChargeResponse> CancelationChargeAsync(mdlCancellationRequest request);
        //Task<mdlFlightCancellationResponse> CancellationAsync(mdlCancellationRequest request);
        //Task<mdlCancelationDetails> CancelationDetailsAsync(string request);
    }
    public interface IsrvAir
    {
        void AlterSeachIndex(List<mdlSearchResult> searchResults);
        void ClearAllCharge();
        Task<List<mdlFareQuotResponse>> FareQuoteAsync(mdlFareQuotRequestWraper request);
        Task<mdlSearchResponse> FlightSearchAsync(mdlFlightSearchWraper mdl, enmCustomerType customerType, int CustomerId, ulong Nid);
        IEnumerable<tblAirport> GetAirport(bool OnlyActive = true, bool IsDomestic = false);
        void GetCharges(mdlFlightSearchWraper searchRequest, mdlSearchResult searchResult, List<mdlTravellerinfo> travellerinfos, bool IsOnward, enmCustomerType CustomerType, int CustomerId, ulong Nid, DateTime bookingDate);
        List<tblFlightCustomerMarkup> GetCustomerMarkup(bool AllMarkup, bool AllActiveMarkup, DateTime ProcessingDate, int CustomerId, ulong Nid, enmCustomerType CustomerType);
        List<mdlFlightAlter> GetFlightBookingAlterMaster();
        List<mdlFlightFareFilter> GetFlightFareFilter(bool ApplyCustomerFilter, enmCustomerType CustomerType);
        List<tblFlightInstantBooking> GetInstantBookingSeting(bool FilterDate, DateTime ProcessDate);
        List<tblFlightSerivceProvider> GetServiceProvider(DateTime ProcessDate, bool IsOnlyActive);
        List<tblFlightSerivceProviderPriority> GetServiceProviderPriority(DateTime ProcessDate, bool IsOnlyActive);
        List<mdlWingMarkup_Air> GetWingConvenience(bool OnlyActive, bool FilterDateCriteria, enmCustomerType CustomerType, int customerId, DateTime TravelDt, DateTime BookingDate);
        List<mdlWingMarkup_Air> GetWingDiscount(bool OnlyActive, bool FilterDateCriteria, enmCustomerType CustomerType, int customerId, DateTime TravelDt, DateTime BookingDate);
        List<mdlWingMarkup_Air> GetWingMarkup(bool OnlyActive, bool FilterDateCriteria, enmCustomerType CustomerType, int customerId, DateTime TravelDt, DateTime BookingDate);
        mdlReturnData RemoveCustomerMarkup(int MarkupID, ulong UserId, string Remarks);
        mdlReturnData SaveFareQuote(mdlFareQuotRequestWraper request, IsrvCurrentUser _IsrvCurrentUser, List<mdlFareQuotResponse> response, int OrgId, ulong Nid);
        mdlReturnData SetCustomerMarkup(double MarkupAmount, DateTime EffectiveFromDt, DateTime EffectiveToDt, ulong UserId, int CustomerId, int Nid, string Remarks);
        mdlReturnData SetFlightBookingAlterMaster(mdlFlightAlter mdl, ulong UserId);
        mdlReturnData SetFlightFareFilter(mdlFlightFareFilter mdl, ulong UserId);
        mdlReturnData SetInstantBookingSeting(DateTime EffectiveFromDate, enmCustomerType CustomerType, bool InstantDomestic, bool InstantNonDomestic, ulong UserId, string Remarks);
        mdlReturnData SetServiceProvider(DateTime EffectiveFromDate, enmServiceProvider ServiceProvider, bool IsEnable, ulong UserId, string Remarks);
        mdlReturnData SetServiceProviderPriority(DateTime EffectiveFromDate, enmServiceProvider ServiceProvider, int priority, ulong UserId, string Remarks);
    }

    public partial class srvAir :  IsrvAir
    {
        private readonly TravelContext _travelContext;
        private readonly ITripJack _tripJack;
        private List<mdlWingMarkup_Air> _WingMarkupOnward, _WingDiscountOnward, _WingConvenienceOnward,
            _WingMarkupInward, _WingDiscountInward, _WingConvenienceInward;
        private List<tblFlightCustomerMarkup> _CustomerMarkupOnward, _CustomerMarkupInward;
        private readonly IConfiguration _config;
        public srvAir(IConfiguration config, TravelContext travelContext, ITripJack tripJack)
        {
            _travelContext = travelContext;
            _tripJack = tripJack;
            _config = config;
        }


        public IEnumerable<tblAirport> GetAirport(bool OnlyActive = true, bool IsDomestic = false)
        {
            var TempData = _travelContext.tblAirport.Where(p => !p.IsDeleted).AsQueryable();
            if (OnlyActive)
            {
                TempData = TempData.Where(p => p.IsActive);
            }
            if (IsDomestic)
            {
                TempData = TempData.Where(p => p.IsDomestic);
            }
            return TempData.AsEnumerable();
        }

        private IWingFlight GetFlightObject(enmServiceProvider serviceProvider)
        {
            switch (serviceProvider)
            {
                case enmServiceProvider.TBO:
                    return null;
                case enmServiceProvider.TripJack:
                    return _tripJack;
            }
            return null;
        }

        public void ClearAllCharge()
        {
            _WingMarkupOnward = null; _WingDiscountOnward = null; _WingConvenienceOnward = null;
            _WingMarkupInward = null; _WingDiscountInward = null; _WingConvenienceInward = null;
            _CustomerMarkupOnward = null; _CustomerMarkupInward = null;
        }

        public void AlterSeachIndex(List<mdlSearchResult> searchResults)
        {
            var tempData = GetFlightBookingAlterMaster();
            foreach (var sr in searchResults)
            {
                foreach (var price in sr.TotalPriceList)
                {
                    var tempAlter = tempData.Where(p => p.CabinClass == price.CabinClass && p.ClassOfBooking == price.ClassOfBooking && p.Identifier == price.Identifier).FirstOrDefault();
                    if (tempAlter == null)
                    {
                        continue;
                    }
                    string ResultIndex = price.ResultIndex;
                    double MinPrice = price.NetFare;
                    FindMinimumIndex(sr.Segment, tempAlter.AlterDetails, ref ResultIndex, ref MinPrice);
                    if (price.NetFare != MinPrice)
                    {
                        price.alterPrices = MinPrice;
                        price.AlterResultIndex = ResultIndex;
                    }
                }

            }
            void FindMinimumIndex(List<mdlSegment> Segment, List<Tuple<enmCabinClass, string, string>> bookingClass, ref string ResultIndex, ref double MinPrice)
            {
                bool IsAllSegmentAreEqual = true;
                foreach (var innerSr in searchResults)
                {
                    if (innerSr.Segment.Count != Segment.Count)
                    {
                        continue;
                    }
                    IsAllSegmentAreEqual = true;
                    for (int i = 0; i < innerSr.Segment.Count; i++)
                    {
                        if (!(string.Equals(innerSr.Segment[i].Airline.Code, Segment[i].Airline.Code, StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(innerSr.Segment[i].Airline.Name, Segment[i].Airline.Name, StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(innerSr.Segment[i].Airline.FlightNumber, Segment[i].Airline.FlightNumber, StringComparison.OrdinalIgnoreCase)
                            ))
                        {

                            IsAllSegmentAreEqual = false;
                            i = innerSr.Segment.Count;
                        }

                    }
                    if (IsAllSegmentAreEqual)
                    {
                        for (int j = 0; j < innerSr.TotalPriceList.Count; j++)
                        {
                            if (innerSr.TotalPriceList[j].NetFare < MinPrice && !string.Equals(ResultIndex, innerSr.TotalPriceList[j].ResultIndex, StringComparison.OrdinalIgnoreCase))
                            {
                                bookingClass.Where(q => q.Item1 == innerSr.TotalPriceList[j].CabinClass &&
                            q.Item2 == innerSr.TotalPriceList[j].Identifier &&
                            q.Item3 == innerSr.TotalPriceList[j].ClassOfBooking);
                            }
                            {
                                MinPrice = innerSr.TotalPriceList[j].NetFare;
                                ResultIndex = innerSr.TotalPriceList[j].ResultIndex;
                            }
                        }
                    }
                }

            }
        }

        private void SetBasicPrice(List<List<mdlSearchResult>> Results)
        {
            for (int i = Results.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < Results[i].Count; j++)
                {
                    for (int k = 0; k < Results[i][j].TotalPriceList.Count; k++)
                    {
                        if (Results[i][j].TotalPriceList[k].BaseFare == 0)
                        {
                            Results[i][j].TotalPriceList[k].BaseFare =
                            Results[i][j].TotalPriceList[k].ADULT.NetFare +
                            Results[i][j].TotalPriceList[k].CHILD.NetFare +
                            Results[i][j].TotalPriceList[k].INFANT.NetFare;
                        }
                        if (Results[i][j].TotalPriceList[k].NetFare == 0)
                        {
                            Results[i][j].TotalPriceList[k].NetFare =
                            Results[i][j].TotalPriceList[k].ADULT.NetFare +
                            Results[i][j].TotalPriceList[k].CHILD.NetFare +
                            Results[i][j].TotalPriceList[k].INFANT.NetFare;
                        }
                        if (Results[i][j].TotalPriceList[k].TotalFare == 0)
                        {
                            Results[i][j].TotalPriceList[k].TotalFare =
                            Results[i][j].TotalPriceList[k].ADULT.TotalFare +
                            Results[i][j].TotalPriceList[k].CHILD.TotalFare +
                            Results[i][j].TotalPriceList[k].INFANT.TotalFare;
                        }

                        if (Results[i][j].TotalPriceList[k].ADULT.FareBreakup == null)
                        {
                            Results[i][j].TotalPriceList[k].ADULT.FareBreakup = new List<mdlWingFaredetails>();
                        }
                        if (Results[i][j].TotalPriceList[k].CHILD.FareBreakup == null)
                        {
                            Results[i][j].TotalPriceList[k].CHILD.FareBreakup = new List<mdlWingFaredetails>();
                        }
                        if (Results[i][j].TotalPriceList[k].INFANT.FareBreakup == null)
                        {
                            Results[i][j].TotalPriceList[k].INFANT.FareBreakup = new List<mdlWingFaredetails>();
                        }
                        if (Results[i][j].TotalPriceList[k].ConsolidateFareBreakup == null)
                        {
                            Results[i][j].TotalPriceList[k].ConsolidateFareBreakup = new List<mdlWingFaredetails>();
                        }
                    }
                }
            }
        }

        private void SetBasicPriceWithMarkup(List<List<mdlSearchResult>> Results)
        {

            for (int i = Results.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < Results[i].Count; j++)
                {
                    for (int k = 0; k < Results[i][j].TotalPriceList.Count; k++)
                    {


                        Results[i][j].TotalPriceList[k].ADULT.WingMarkup =
                        Results[i][j].TotalPriceList[k].ADULT.FareBreakup.
                        Where(p => p.type == enmFlighWingCharge.WingMarkup && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);

                        Results[i][j].TotalPriceList[k].CHILD.WingMarkup =
                             Results[i][j].TotalPriceList[k].CHILD.FareBreakup.
                             Where(p => p.type == enmFlighWingCharge.WingMarkup && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);
                        Results[i][j].TotalPriceList[k].INFANT.WingMarkup =
                             Results[i][j].TotalPriceList[k].INFANT.FareBreakup.
                             Where(p => p.type == enmFlighWingCharge.WingMarkup && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);

                        Results[i][j].TotalPriceList[k].ADULT.MLMMarkup =
                            Results[i][j].TotalPriceList[k].ADULT.FareBreakup.
                            Where(p => p.type == enmFlighWingCharge.MLMCharge && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);
                        Results[i][j].TotalPriceList[k].CHILD.MLMMarkup =
                             Results[i][j].TotalPriceList[k].CHILD.FareBreakup.
                             Where(p => p.type == enmFlighWingCharge.MLMCharge && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);
                        Results[i][j].TotalPriceList[k].INFANT.MLMMarkup =
                             Results[i][j].TotalPriceList[k].INFANT.FareBreakup.
                             Where(p => p.type == enmFlighWingCharge.MLMCharge && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);

                        Results[i][j].TotalPriceList[k].ADULT.Discount =
                            Results[i][j].TotalPriceList[k].ADULT.FareBreakup.
                            Where(p => p.type == enmFlighWingCharge.Discount && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);
                        Results[i][j].TotalPriceList[k].CHILD.Discount =
                             Results[i][j].TotalPriceList[k].CHILD.FareBreakup.
                             Where(p => p.type == enmFlighWingCharge.Discount && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);
                        Results[i][j].TotalPriceList[k].INFANT.Discount =
                             Results[i][j].TotalPriceList[k].INFANT.FareBreakup.
                             Where(p => p.type == enmFlighWingCharge.Discount && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);

                        Results[i][j].TotalPriceList[k].ADULT.Convenience =
                            Results[i][j].TotalPriceList[k].ADULT.FareBreakup.
                            Where(p => p.type == enmFlighWingCharge.Convenience && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);
                        Results[i][j].TotalPriceList[k].CHILD.Convenience =
                             Results[i][j].TotalPriceList[k].CHILD.FareBreakup.
                             Where(p => p.type == enmFlighWingCharge.Convenience && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);
                        Results[i][j].TotalPriceList[k].INFANT.Convenience =
                             Results[i][j].TotalPriceList[k].INFANT.FareBreakup.
                             Where(p => p.type == enmFlighWingCharge.Convenience && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);

                        Results[i][j].TotalPriceList[k].ADULT.TotalFare = Results[i][j].TotalPriceList[k].ADULT.TotalFare +
                            Results[i][j].TotalPriceList[k].ADULT.WingMarkup + Results[i][j].TotalPriceList[k].ADULT.MLMMarkup
                            + Results[i][j].TotalPriceList[k].Convenience;
                        Results[i][j].TotalPriceList[k].ADULT.NetFare = Results[i][j].TotalPriceList[k].ADULT.NetFare +
                            Results[i][j].TotalPriceList[k].ADULT.WingMarkup + Results[i][j].TotalPriceList[k].ADULT.MLMMarkup
                            - Results[i][j].TotalPriceList[k].ADULT.Discount + Results[i][j].TotalPriceList[k].Convenience;

                        Results[i][j].TotalPriceList[k].CHILD.TotalFare = Results[i][j].TotalPriceList[k].CHILD.TotalFare +
                           Results[i][j].TotalPriceList[k].CHILD.WingMarkup + Results[i][j].TotalPriceList[k].CHILD.MLMMarkup
                           + Results[i][j].TotalPriceList[k].Convenience;
                        Results[i][j].TotalPriceList[k].CHILD.NetFare = Results[i][j].TotalPriceList[k].CHILD.NetFare +
                            Results[i][j].TotalPriceList[k].CHILD.WingMarkup + Results[i][j].TotalPriceList[k].CHILD.MLMMarkup
                            - Results[i][j].TotalPriceList[k].CHILD.Discount + Results[i][j].TotalPriceList[k].Convenience;

                        Results[i][j].TotalPriceList[k].INFANT.TotalFare = Results[i][j].TotalPriceList[k].INFANT.TotalFare +
                            Results[i][j].TotalPriceList[k].INFANT.WingMarkup + Results[i][j].TotalPriceList[k].INFANT.MLMMarkup
                            + Results[i][j].TotalPriceList[k].Convenience;
                        Results[i][j].TotalPriceList[k].INFANT.NetFare = Results[i][j].TotalPriceList[k].INFANT.NetFare +
                            Results[i][j].TotalPriceList[k].INFANT.WingMarkup + Results[i][j].TotalPriceList[k].INFANT.MLMMarkup
                            - Results[i][j].TotalPriceList[k].INFANT.Discount + Results[i][j].TotalPriceList[k].Convenience;


                        Results[i][j].TotalPriceList[k].BaseFare =
                            Results[i][j].TotalPriceList[k].ADULT.NetFare +
                            Results[i][j].TotalPriceList[k].CHILD.NetFare +
                            Results[i][j].TotalPriceList[k].INFANT.NetFare;


                        Results[i][j].TotalPriceList[k].WingMarkup =
                            Results[i][j].TotalPriceList[k].ConsolidateFareBreakup.
                            Where(p => p.type == enmFlighWingCharge.WingMarkup && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);

                        Results[i][j].TotalPriceList[k].MLMMarkup =
                            Results[i][j].TotalPriceList[k].ConsolidateFareBreakup.
                            Where(p => p.type == enmFlighWingCharge.MLMCharge && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);

                        Results[i][j].TotalPriceList[k].Discount =
                            Results[i][j].TotalPriceList[k].ConsolidateFareBreakup.
                            Where(p => p.type == enmFlighWingCharge.Discount && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);

                        Results[i][j].TotalPriceList[k].Convenience =
                            Results[i][j].TotalPriceList[k].ConsolidateFareBreakup.
                            Where(p => p.type == enmFlighWingCharge.Convenience && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);

                        Results[i][j].TotalPriceList[k].CustomerMarkup =
                            Results[i][j].TotalPriceList[k].ConsolidateFareBreakup.
                            Where(p => p.type == enmFlighWingCharge.CustomerMarkup && (p.OnGender == enmGender.ALL || p.OnGender == enmGender.None)).Sum(p => p.amount);

                        Results[i][j].TotalPriceList[k].TotalFare =
                            Results[i][j].TotalPriceList[k].TotalFare +
                            Results[i][j].TotalPriceList[k].WingMarkup +
                            Results[i][j].TotalPriceList[k].MLMMarkup +
                            Results[i][j].TotalPriceList[k].CustomerMarkup +
                            Results[i][j].TotalPriceList[k].Convenience;


                        Results[i][j].TotalPriceList[k].NetFare =
                            Results[i][j].TotalPriceList[k].NetFare +
                            Results[i][j].TotalPriceList[k].WingMarkup +
                            Results[i][j].TotalPriceList[k].MLMMarkup +
                            Results[i][j].TotalPriceList[k].CustomerMarkup +
                            Results[i][j].TotalPriceList[k].Convenience -
                            Results[i][j].TotalPriceList[k].Discount ;

                    }
                }
            }
        }

        public bool IsDomecticFlight(string From, string To)
        {
           return _travelContext.tblAirport.Where(p => p.AirportCode == From || p.AirportCode == To && !p.IsDomestic).Count() > 0 ? false : true;
        }

        public async Task<mdlSearchResponse> FlightSearchAsync(mdlFlightSearchWraper mdl, enmCustomerType customerType, int CustomerId, ulong Nid)
        {
            DateTime CurrentDate = DateTime.Now;
            var ServiceProviders = GetServiceProvider(CurrentDate, true).Select(p => p.ServiceProvider);

            mdlSearchResponse FinalResult = null;
            foreach (var sp in ServiceProviders)
            {
                IWingFlight tempObj = GetFlightObject(sp);
                if (tempObj == null)
                {
                    continue;
                }

                List<mdlSegmentRequest> onwardSegments = new List<mdlSegmentRequest>();
                onwardSegments.Add(new mdlSegmentRequest()
                { Destination = mdl.To, Origin = mdl.From, FlightCabinClass = mdl.CabinClass, TravelDt = mdl.DepartureDt });
                mdlSearchRequest request = new mdlSearchRequest()
                {
                    AdultCount = mdl.AdultCount,
                    ChildCount = mdl.ChildCount,
                    InfantCount = mdl.InfantCount,
                    JourneyType = enmJourneyType.OneWay,
                    DirectFlight = mdl.DirectFlight,
                    Segments = onwardSegments,
                };

                AppendProvider(await tempObj.SearchAsync(request), true, sp);
                if (mdl.JourneyType == enmJourneyType.Return && mdl.ReturnDt.HasValue)
                {
                    List<mdlSegmentRequest> inwardSegments = new List<mdlSegmentRequest>();
                    inwardSegments.Add(new mdlSegmentRequest()
                    { Destination = mdl.From, Origin = mdl.To, FlightCabinClass = mdl.CabinClass, TravelDt = mdl.ReturnDt.Value });
                    mdlSearchRequest inwardrequest = new mdlSearchRequest()
                    {
                        AdultCount = mdl.AdultCount,
                        ChildCount = mdl.ChildCount,
                        InfantCount = mdl.InfantCount,
                        JourneyType = enmJourneyType.OneWay,
                        DirectFlight = mdl.DirectFlight,
                        Segments = onwardSegments,
                    };
                    AppendProvider(await tempObj.SearchAsync(inwardrequest), false, sp);
                }
            }
            if (FinalResult == null)
            {
                FinalResult = new mdlSearchResponse() { ResponseStatus = enmMessageType.Error, Error = new mdlError() { Code = 100, Message = "No data found" } };
                return FinalResult;
            }
            if (FinalResult.Results == null || FinalResult.Results.Count() == 0 || (FinalResult.Results.FirstOrDefault()?.Count ?? 0) == 0)
            {
                FinalResult.ResponseStatus = enmMessageType.Error;
                FinalResult.Error = new mdlError() { Code = 100, Message = "No data found" };
                return FinalResult;
            }
            if (mdl.JourneyType == enmJourneyType.Return && (FinalResult.Results.Count() < 2 || (FinalResult.Results[1]?.Count ?? 0) == 0))
            {
                FinalResult.ResponseStatus = enmMessageType.Error;
                FinalResult.Error = new mdlError() { Code = 100, Message = "No data found in return flight" };
                return FinalResult;
            }
            SetBasicPrice(FinalResult.Results);
            ClearAllCharge();

            bool IsDomectic= IsDomecticFlight(mdl.From,mdl.To);

            if (GetInstantBookingSeting(true, CurrentDate).Where(p => p.CustomerType == customerType &&  (( p.InstantDomestic && IsDomectic)|| (p.InstantNonDomestic && !IsDomectic))).Any())
            {
                AlterSeachIndex(FinalResult.Results[0]);
                if (mdl.JourneyType == enmJourneyType.Return)
                    AlterSeachIndex(FinalResult.Results[0]);
            }


            mdlFlightFareFilter tempFlightFareFilter = GetFlightFareFilter(true, customerType).FirstOrDefault();

            if (!(tempFlightFareFilter == null || tempFlightFareFilter?.IsEanableAllFare == true))
            {
                FlightFareFilterRemoval();
            }


            var spPriority = GetServiceProviderPriority(CurrentDate, true);
            RemoveDuplicate();
            SetBasicPriceWithMarkup(FinalResult.Results);
            void AppendProvider(mdlSearchResponse tempResult, bool IsOnWard, enmServiceProvider spv)
            {
                tempResult.Results.FirstOrDefault().ForEach(p =>
                {
                    p.TotalPriceList.ForEach(q => { q.ResultIndex = string.Concat(spv, '_',  q.ResultIndex); });
                });


                if (FinalResult == null)
                {
                    FinalResult = tempResult;
                }
                else
                {
                    if (IsOnWard)
                    {
                        FinalResult.Results[0].AddRange(tempResult.Results.FirstOrDefault());
                    }
                    else
                    {
                        if (FinalResult.Results.Count == 1)
                        {
                            FinalResult.Results.Add(new List<mdlSearchResult>());
                        }
                        FinalResult.Results[1].AddRange(tempResult.Results.FirstOrDefault());
                    }
                }
            }
            void FlightFareFilterRemoval()
            {

                for (int i = FinalResult.Results.Count - 1; i >= 0; i--)
                {
                    for (int j = FinalResult.Results[i].Count - 1; j >= 0; j--)
                    {
                        for (int k = FinalResult.Results[i][j].TotalPriceList.Count - 1; k >= 0; k--)
                        {
                            if (!tempFlightFareFilter.FilterDetails.Where(p => string.Equals(p.Item1, FinalResult.Results[i][j].TotalPriceList[k].Identifier, StringComparison.OrdinalIgnoreCase)
                             &&
                             string.Equals(p.Item2, FinalResult.Results[i][j].TotalPriceList[k].ClassOfBooking, StringComparison.OrdinalIgnoreCase)).Any())
                            {
                                FinalResult.Results[i][j].TotalPriceList.RemoveAt(k);
                            }
                        }
                        if (FinalResult.Results[i][j].TotalPriceList.Count() == 0)
                        {
                            FinalResult.Results[i].RemoveAt(j);
                        }

                    }
                }

            }
            void RemoveDuplicate()
            {
                enmServiceProvider FromServiceProvider;
                enmServiceProvider ToServiceProvider;

                for (int i = FinalResult.Results.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < FinalResult.Results[i].Count; j++)
                    {
                        for (int j1 = FinalResult.Results[i].Count - 1; j1 >= j; j1--)
                        {
                            if (FinalResult.Results[i][j].Segment.Count == FinalResult.Results[i][j1].Segment.Count)
                            {
                                for (int s = 0; s < FinalResult.Results[i][j].Segment.Count; s++)
                                {
                                    if (!(string.Equals(FinalResult.Results[i][j].Segment[s].Airline.Code, FinalResult.Results[i][j1].Segment[s].Airline.Code, StringComparison.OrdinalIgnoreCase) &&
                                    string.Equals(FinalResult.Results[i][j].Segment[s].Airline.Name, FinalResult.Results[i][j1].Segment[s].Airline.Name, StringComparison.OrdinalIgnoreCase) &&
                                    string.Equals(FinalResult.Results[i][j].Segment[s].Airline.FlightNumber, FinalResult.Results[i][j1].Segment[s].Airline.FlightNumber, StringComparison.OrdinalIgnoreCase)
                                        ))
                                    {
                                        goto MoveNextResult;
                                    }
                                }
                            }
                            else
                            {
                                goto MoveNextResult;
                            }
                            for (int k = 0; k < FinalResult.Results[i][j].TotalPriceList.Count; k++)
                            {
                                for (int k1 = FinalResult.Results[i][j].TotalPriceList.Count - 1; k1 >= 0; k1--)
                                {
                                    if (
                                        FinalResult.Results[i][j].TotalPriceList[k].CabinClass == FinalResult.Results[i][j1].TotalPriceList[k1].CabinClass &&
                                        string.Equals(FinalResult.Results[i][j].TotalPriceList[k].Identifier, FinalResult.Results[i][j1].TotalPriceList[k1].Identifier, StringComparison.OrdinalIgnoreCase) &&
                                        string.Equals(FinalResult.Results[i][j].TotalPriceList[k].ClassOfBooking, FinalResult.Results[i][j1].TotalPriceList[k1].ClassOfBooking, StringComparison.OrdinalIgnoreCase)
                                        )
                                    {
                                        if (FinalResult.Results[i][j].TotalPriceList[k].NetFare > FinalResult.Results[i][j1].TotalPriceList[k1].NetFare)
                                        {
                                            FinalResult.Results[i][j].TotalPriceList.RemoveAt(k);
                                            FinalResult.Results[i][j].TotalPriceList.Add(FinalResult.Results[i][j1].TotalPriceList[k1]);
                                            FinalResult.Results[i][j1].TotalPriceList.RemoveAt(k1);
                                        }
                                        else if (FinalResult.Results[i][j].TotalPriceList[k].NetFare < FinalResult.Results[i][j1].TotalPriceList[k1].NetFare)
                                        {
                                            FinalResult.Results[i][j1].TotalPriceList.RemoveAt(k1);
                                        }
                                        else
                                        {
                                            Enum.TryParse(FinalResult.Results[i][j].TotalPriceList[k].ResultIndex.Split("_").FirstOrDefault(), out FromServiceProvider);
                                            Enum.TryParse(FinalResult.Results[i][j1].TotalPriceList[k1].ResultIndex.Split("_").FirstOrDefault(), out ToServiceProvider);
                                            var FirstProvider = spPriority.Where(p => p.ServiceProvider == FromServiceProvider || p.ServiceProvider == ToServiceProvider).OrderBy(p => p.priority).FirstOrDefault();
                                            if (FirstProvider == null || FirstProvider.ServiceProvider == FromServiceProvider)
                                            {
                                                FinalResult.Results[i][j1].TotalPriceList.RemoveAt(k1);
                                            }
                                            else
                                            {
                                                FinalResult.Results[i][j].TotalPriceList.RemoveAt(k);
                                                FinalResult.Results[i][j].TotalPriceList.Add(FinalResult.Results[i][j1].TotalPriceList[k1]);
                                                FinalResult.Results[i][j1].TotalPriceList.RemoveAt(k1);
                                            }
                                        }
                                    }
                                }
                            }

                            if (FinalResult.Results[i][j1].TotalPriceList.Count > 0)
                            {
                                FinalResult.Results[i][j].TotalPriceList.AddRange(FinalResult.Results[i][j1].TotalPriceList);
                                FinalResult.Results[i].RemoveAt(j1);
                            }
                            else
                            {
                                FinalResult.Results[i].RemoveAt(j1);
                            }

                        }
                    MoveNextResult:;
                    }
                }
            }
            return FinalResult;

            //GetCharges();
        }

        public async Task<List<mdlFareQuotResponse>> FareQuoteAsync(mdlFareQuotRequestWraper request)
        {
            List<mdlFareQuotResponse> mdl = new List<mdlFareQuotResponse>();
            DateTime CurrentDate = DateTime.Now;
            var ServiceProviders = GetServiceProvider(CurrentDate, true).Select(p => p.ServiceProvider);

            //mdlSearchResponse FinalResult = null;
            enmServiceProvider tempServiceProvider1 = enmServiceProvider.None;
            enmServiceProvider tempServiceProvider2 = enmServiceProvider.None;
            int FlightPriceVarienceAlert = 100;
            int.TryParse(_config["Travel:Setting:FlightPriceVarienceAlert"], out FlightPriceVarienceAlert);
            for (int i = 0; i < request.ResultIndex.Count; i++)
            {
                mdlFareQuotResponse BookingRes = null; ;
                mdlFareQuotResponse AlterBookingRes = null; ;

                if (!string.IsNullOrEmpty(request.ResultIndex[i].Item2))
                {
                    var temp = request.ResultIndex[i].Item2.Split("_");
                    Enum.TryParse(temp.FirstOrDefault(), out tempServiceProvider1);
                    if (tempServiceProvider1 == enmServiceProvider.None)
                    {
                        throw new Exception(enmMessage.InvalidServiceProvider.GetDescription());
                    }
                    if (!ServiceProviders.Any(p => p == tempServiceProvider1))
                    {
                        throw new Exception(enmMessage.InvalidServiceProvider.GetDescription());
                    }
                    IWingFlight tempObj = GetFlightObject(tempServiceProvider1);
                    if (tempObj == null)
                    {
                        throw new Exception(enmMessage.ProviderNotImplemented.GetDescription());
                    }
                    AlterBookingRes = await tempObj.FareQuoteAsync(new mdlFareQuotRequest() { TraceId = request.TraceId, ResultIndex = GetOriginalResultIndex(request.ResultIndex[i].Item2) });

                }

                if (!string.IsNullOrEmpty(request.ResultIndex[i].Item1))
                {
                    var temp = request.ResultIndex[i].Item1.Split("_");
                    Enum.TryParse(temp.FirstOrDefault(), out tempServiceProvider2);
                    if (tempServiceProvider2 == enmServiceProvider.None)
                    {
                        throw new Exception(enmMessage.InvalidServiceProvider.GetDescription());
                    }
                    if (!ServiceProviders.Any(p => p == tempServiceProvider2))
                    {
                        throw new Exception(enmMessage.InvalidServiceProvider.GetDescription());
                    }
                    IWingFlight tempObj = GetFlightObject(tempServiceProvider2);
                    if (tempObj == null)
                    {
                        throw new Exception(enmMessage.ProviderNotImplemented.GetDescription());
                    }
                    BookingRes = await tempObj.FareQuoteAsync(new mdlFareQuotRequest() { TraceId = request.TraceId, ResultIndex = GetOriginalResultIndex(request.ResultIndex[i].Item1) });

                }
                if (BookingRes == null || BookingRes.ResponseStatus != enmMessageType.Success)
                {
                    throw new Exception("Not able to genrate Quotation");
                }
                if (BookingRes.IsPriceChanged && AlterBookingRes != null)
                {
                    throw new Exception("Price has been changed");
                }
                if (AlterBookingRes != null)
                {
                    if (AlterBookingRes.ResponseStatus != enmMessageType.Success)
                    {
                        throw new Exception("Not able to genrate Quotation.");
                    }
                    if (AlterBookingRes.IsPriceChanged)
                    {
                        throw new Exception("Price has been changed.");
                    }
                    if (BookingRes.TotalPriceInfo.NetFare - AlterBookingRes.TotalPriceInfo.NetFare < FlightPriceVarienceAlert)
                    {
                        throw new Exception("Price has been changed");
                    }
                }
                if (BookingRes.Results.Count == 0 || BookingRes.Results[0].Count == 0 || BookingRes.Results[0][0].TotalPriceList.Count == 0)
                {
                    throw new Exception("Not able to genrate Quotation.");
                }
                //BookingRes.PurchaseCabinClass = BookingRes.Results[0][0].TotalPriceList[0].CabinClass;
                //BookingRes.PurchaseClassOfBooking = BookingRes.Results[0][0].TotalPriceList[0].ClassOfBooking;
                //BookingRes.PurchaseIdentifier = BookingRes.Results[0][0].TotalPriceList[0].Identifier;
                ClearAllCharge();
                SetBasicPrice(BookingRes.Results);
                SetBasicPriceWithMarkup(BookingRes.Results);
                if (AlterBookingRes != null)
                {
                    SetBasicPriceWithMarkup(AlterBookingRes.Results);
                    if (BookingRes.Results.Count == 0 || AlterBookingRes.Results.Count == 0)
                    {
                        throw new Exception(enmMessage.InvalidData.GetDescription());
                    }
                    if (BookingRes.Results[0].Count == 0 || AlterBookingRes.Results[0].Count == 0)
                    {
                        throw new Exception(enmMessage.InvalidData.GetDescription());
                    }
                    BookingRes.Results[0][0].Segment = AlterBookingRes.Results[0][0].Segment;

                    if (AlterBookingRes.Results.Count == 0 || AlterBookingRes.Results[0].Count == 0 || AlterBookingRes.Results[0][0].TotalPriceList.Count == 0)
                    {
                        throw new Exception("Not able to genrate Quotation.");
                    }
                    //BookingRes.BookedCabinClass = AlterBookingRes.Results[0][0].TotalPriceList[0].CabinClass;
                    //BookingRes.BookedClassOfBooking = AlterBookingRes.Results[0][0].TotalPriceList[0].ClassOfBooking;
                    //BookingRes.BookedIdentifier = AlterBookingRes.Results[0][0].TotalPriceList[0].Identifier;
                    BookingRes.ServiceProvider = AlterBookingRes.ServiceProvider;
                }
                mdl.Add(BookingRes);
            }



            string GetOriginalResultIndex(string resultIndex)
            {
                int FirstIndex = resultIndex.IndexOf('_');
                int SecondIndex = resultIndex.IndexOf('_', FirstIndex + 1);
                return resultIndex.Substring(SecondIndex + 1);
            }

            return mdl;
        }


        private tblFlightBookingMaster ConvertFareQuoteToFlightBookingMaster(List<List<mdlSearchResult>> Results,
             mdlFlightSearchWraper searchWraper, mdlBookingRequest BookingRequest, string VisitorId, int OrgId, ulong Nid,
             enmCustomerType CustomerType,ulong UserId
             )
        {
            DateTime bookingDate = DateTime.Now;
            bool IsDirectFlight = false;
            //check Wheather the Ticket is Booked if Ticket is Booked then Don't Remove the Markup and Convenive
            //else Remove the Markup and Conveince
            var ExistingBooking=_travelContext.tblFlightBookingMaster.Where(q => q.VisitorId == VisitorId).FirstOrDefault();
            if (ExistingBooking == null)
            {
                List<tblFlighBookingPassengerDetails> PassengerDetails = BookingRequest?.travellerInfo.Select(q=>new tblFlighBookingPassengerDetails { 
                    DOB= q.dob,
                    FirstName=q.FirstName,
                    LastName=q.LastName,
                    PassengerDetailId=Guid.NewGuid().ToString(),
                    PassengerType=q.passengerType,
                    PassportExpiryDate=q.PassportExpiryDate,
                    PassportIssueDate=q.PassportIssueDate,
                    PassportNumber=q.pNum,
                    Title=q.Title,
                    VisitorId=VisitorId,
                    PassengerServices_Baggage= q.ssrBaggageInfoslist.Select(r=>new  Tuple<string, string, double,string>(r.key,r.code,0,r.desc )).ToList(),
                    PassengerServices_Meal= q.ssrMealInfoslist.Select(r => new Tuple<string, string, double, string>(r.key, r.code, 0, r.desc)).ToList(),
                    PassengerServices_Seat= q.ssrSeatInfoslist.Select(r => new Tuple<string, string, double, string>(r.key, r.code, 0, r.desc)).ToList(),
                    PassengerServices_ExtraService = q.ssrExtraServiceInfoslist.Select(r => new Tuple<string, string, double, string>(r.key, r.code, 0, r.desc)).ToList(),
                }).ToList();



                
                tblFlightBookingMaster fbm = null;
                tblFlightBookingSearchDetails InwardSearchDetails = null, ReturnSearchDetails = null;
                List <tblFlightFareMarkupDetail> FlightFareMarkupDetail = null, ReturnFlightFareMarkupDetail=null;
                List<tblFlightFareMLMMarkup> FlightFareMLMMarkup = null, ReturnFlightFareMLMMarkup = null;
                List<tblFlightFareDiscount> FlightFareDiscount = null, ReturnFlightFareDiscount = null;
                List<tblFlightFareConvenience> FlightFareConvenience = null, ReturnFlightFareConvenience = null;
                List<tblFlightFareDetail> tblFlightFareDetails = new List<tblFlightFareDetail>();
                List<tblFlightFareDetail> ReturntblFlightFareDetails = new List<tblFlightFareDetail>();
                List<tblFlightSearchSegment> tblFlightSearchSegmentInward = null, tblFlightSearchSegmentReturn = null;
                //onward
                {
                    enmServiceProvider ServiceProvider = enmServiceProvider.None;
                     fbm = new tblFlightBookingMaster() {
                        AdultCount = searchWraper?.AdultCount ?? 0,
                        ChildCount = searchWraper?.ChildCount ?? 0,
                        InfantCount = searchWraper?.InfantCount ?? 0,
                        BookingDate = bookingDate,
                        CabinClass = searchWraper?.CabinClass ?? enmCabinClass.ECONOMY,
                        From = searchWraper?.From ?? string.Empty,
                        To = searchWraper?.To ?? string.Empty,
                        JourneyType = searchWraper?.JourneyType ?? enmJourneyType.OneWay,
                        DepartureDt = searchWraper?.DepartureDt ?? bookingDate,
                        ReturnDt = searchWraper?.ReturnDt,
                        OrgId = OrgId,
                        Nid = Nid,
                        EmailNo = BookingRequest?.deliveryInfo?.emails.FirstOrDefault(),
                        PhoneNo = BookingRequest?.deliveryInfo?.contacts.FirstOrDefault(),
                        PaymentMode = BookingRequest?.deliveryInfo?.PaymentMode ?? enmPaymentMode.PaymentGateway,
                        GatewayId = BookingRequest?.deliveryInfo?.GatewayId ?? enmPaymentGateway.None,
                        PaymentType = BookingRequest?.deliveryInfo?.PaymentType ?? enmPaymentSubType.None,
                        PaymentTransactionNumber = BookingRequest?.deliveryInfo?.PaymentTransactionNumber,
                        CardNo = BookingRequest?.deliveryInfo?.CardNo,
                        AccountNumber = BookingRequest?.deliveryInfo?.AccountNumber,
                        BankName = BookingRequest?.deliveryInfo?.BankName,
                        IncludeLoaylty = BookingRequest?.deliveryInfo?.IncludeLoaylty ?? false,
                        ConsumedLoyaltyPoint = BookingRequest?.deliveryInfo?.ConsumedLoyaltyPoint ?? 0,
                        ConsumedLoyaltyAmount = BookingRequest?.deliveryInfo?.ConsumedLoyaltyAmount ?? 0,
                        BookingAmount = BookingRequest?.deliveryInfo?.BookingAmount ?? 0,
                        GatewayCharge = BookingRequest?.deliveryInfo?.GatewayCharge ?? 0,
                        NetAmount = BookingRequest?.deliveryInfo?.NetAmount ?? 0,
                        PaidAmount = BookingRequest?.deliveryInfo?.PaidAmount ?? 0,
                        WalletAmount = BookingRequest?.deliveryInfo?.WalletAmount ?? 0,
                        LoyaltyAmount = BookingRequest?.deliveryInfo?.LoyaltyAmount ?? 0,
                        BookingStatus = enmBookingStatus.Pending,
                        PaymentStatus = enmPaymentStatus.Pending,
                        HaveRefund = false,
                        CreatedBy = UserId,
                        ModifiedBy = UserId,
                        CreatedDt = bookingDate,
                        ModifiedDt = bookingDate,
                        VisitorId = VisitorId
                    };
                    
                    var Inward = Results.FirstOrDefault();
                    var totalPriceList = Inward.FirstOrDefault()?.TotalPriceList?.FirstOrDefault();
                    if ((Inward.FirstOrDefault()?.Segment?.Count ?? 0) == 1)
                    {
                        IsDirectFlight = true;
                    }
                    ServiceProvider = Inward.FirstOrDefault()?.ServiceProvider ?? enmServiceProvider.None;
                    List<string> Airline = Inward.FirstOrDefault()?.Segment?.Select(p => p.Airline.Name).Distinct().ToList();
                    InwardSearchDetails = new tblFlightBookingSearchDetails()
                    {
                        BookingId = Guid.NewGuid().ToString(),
                        VisitorId = VisitorId,
                        SegmentId = 1,
                        PurchaseIdentifier = totalPriceList?.Identifier,
                        PurchaseCabinClass = totalPriceList?.CabinClass ?? enmCabinClass.ECONOMY,
                        PurchaseClassOfBooking = totalPriceList?.ClassOfBooking,
                        BookedIdentifier = totalPriceList?.alterIdentifier ?? totalPriceList.Identifier,
                        BookedCabinClass = totalPriceList?.alterCabinClass ?? totalPriceList.CabinClass,
                        BookedClassOfBooking = totalPriceList?.ClassOfBooking,
                        ProviderBookingId = totalPriceList?.ProviderBookingId,
                        ServiceProvider = totalPriceList?.ServiceProvider ?? enmServiceProvider.None,
                        BookingStatus = enmBookingStatus.Pending,
                        PaymentStatus = enmPaymentStatus.Pending,
                        ConvenienceAmount = totalPriceList?.Convenience ?? 0,
                        CustomerMarkupAmount = totalPriceList?.CustomerMarkup ?? 0,
                        DiscountAmount = totalPriceList?.Discount ?? 0,
                        HaveRefund = false,
                        IncentiveAmount = totalPriceList?.MLMMarkup ?? 0,
                        IncludeBaggageServices = totalPriceList?.IncludeBaggageServices ?? false,
                        IncludeMealServices = totalPriceList?.IncludeMealServices ?? false,
                        IncludeSeatServices = totalPriceList?.IncludeSeatServices ?? false,
                        MarkupAmount = totalPriceList?.WingMarkup ?? 0,
                        NetSaleAmount = totalPriceList?.NetFare ?? 0,
                        PurchaseAmount = totalPriceList?.PurchaseAmount ?? 0,
                        PassengerConvenienceAmount = totalPriceList?.PassengerConvenienceAmount ?? 0,
                        PassengerMarkupAmount = totalPriceList?.PassengerMarkupAmount ?? 0,
                        PassengerDiscountAmount = totalPriceList?.PassengerDiscountAmount ?? 0,
                        PassengerIncentiveAmount = totalPriceList?.PassengerIncentiveAmount ?? 0,
                        IsDeleted = false,
                        SaleAmount = totalPriceList?.SaleAmount ?? 0,
                    };
                    //Get Markup on ticket
                    {
                        FlightFareMarkupDetail =
                        GetMarkup(OrgId, CustomerType, searchWraper.DepartureDt, bookingDate, false, enmFlightSearvices.OnTicket,
                            IsDirectFlight, Airline, searchWraper.From, searchWraper.To, ServiceProvider,
                        totalPriceList.BaseFare, enmPassengerType.Adult, enmTitle.MR
                        ).Select(q => new tblFlightFareMarkupDetail
                        {
                            Amount = q.Item2,
                            BookingId = InwardSearchDetails.BookingId,
                            MarkupId = q.Item1
                        }).ToList();
                        FlightFareMLMMarkup =
                       GetMarkup(OrgId, CustomerType, searchWraper.DepartureDt, bookingDate, true, enmFlightSearvices.OnTicket,
                           IsDirectFlight, Airline, searchWraper.From, searchWraper.To, ServiceProvider,
                       totalPriceList.BaseFare, enmPassengerType.Adult, enmTitle.MR
                       ).Select(q => new tblFlightFareMLMMarkup
                       {
                           Amount = q.Item2,
                           BookingId = InwardSearchDetails.BookingId,
                           MLMMarkupId = q.Item1
                       }).ToList();

                        FlightFareDiscount =
                       GetDiscount(OrgId, CustomerType, searchWraper.DepartureDt, bookingDate, enmFlightSearvices.OnTicket,
                           IsDirectFlight, Airline, searchWraper.From, searchWraper.To, ServiceProvider,
                       totalPriceList.BaseFare, enmPassengerType.Adult, enmTitle.MR
                       ).Select(q => new tblFlightFareDiscount
                       {
                           Amount = q.Item2,
                           BookingId = InwardSearchDetails.BookingId,
                           DiscountId = q.Item1
                       }).ToList();
                        FlightFareConvenience =
                        GetConvenience(OrgId, CustomerType, searchWraper.DepartureDt, bookingDate, enmFlightSearvices.OnTicket,
                            IsDirectFlight, Airline, searchWraper.From, searchWraper.To, ServiceProvider,
                        totalPriceList.BaseFare, enmPassengerType.Adult, enmTitle.MR
                        ).Select(q => new tblFlightFareConvenience
                        {
                            Amount = q.Item2,
                            BookingId = InwardSearchDetails.BookingId,
                            ConvenienceId = q.Item1
                        }).ToList();
                    }

                    tblFlightSearchSegmentInward = Inward.FirstOrDefault()?.Segment.Select(
                    p => new tblFlightSearchSegment()
                    {
                        Id = Guid.NewGuid().ToString(),
                        BookingId = InwardSearchDetails.BookingId,
                        ProviderSegmentId = p.Id,
                        isLcc = p.Airline.isLcc,
                        Code = p.Airline.Code,
                        FlightNumber = p.Airline.FlightNumber,
                        Name = p.Airline.Name,
                        Mile = p.Mile,
                        TripIndicator = p.TripIndicator,
                        Duration = p.Duration,
                        Layover = p.Layover,
                        OperatingCarrier = p.Airline.OperatingCarrier,
                        OriginAirportCode = p.Origin.AirportCode,
                        OriginAirportName = p.Origin.AirportName,
                        OriginCityCode = p.Origin.CityCode,
                        OriginCityName = p.Origin.CityName,
                        OriginCountryCode = p.Origin.CountryCode,
                        OriginCountryName = p.Origin.CountryName,
                        OriginTerminal = p.Origin.Terminal,
                        DestinationAirportCode = p.Destination.AirportCode,
                        DestinationAirportName = p.Destination.AirportName,
                        DestinationCityCode = p.Destination.CityCode,
                        DestinationCityName = p.Destination.CityName,
                        DestinationCountryCode = p.Destination.CountryCode,
                        DestinationCountryName = p.Destination.CountryName,
                        DestinationTerminal = p.Destination.Terminal,
                        Stops = p.Stops,
                        tblFlightSearchSegmentStops = p.StopDetail?.Select(q => new tblFlightSearchSegmentStops
                        {
                            AirportCode = q.AirportCode,
                            AirportName = q.AirportName,
                            CityCode = q.CityCode,
                            CityName = q.CityName,
                            CountryCode = q.CountryCode,
                            CountryName = q.CountryName,
                            Id = Guid.NewGuid().ToString(),
                        }).ToList()
                    }).ToList();                    
                    foreach (var passenger in PassengerDetails)
                    {
                        double basePrice = 0, YqTax = 0, Tax = 0;
                        if (passenger.PassengerType == enmPassengerType.Adult)
                        {
                            basePrice = totalPriceList.ADULT.BaseFare;
                            YqTax = totalPriceList.ADULT.YQTax;
                            Tax = totalPriceList.ADULT.NetFare - totalPriceList.ADULT.BaseFare;

                        }
                        else if (passenger.PassengerType == enmPassengerType.Child)
                        {
                            basePrice = totalPriceList.CHILD.BaseFare;
                            YqTax = totalPriceList.CHILD.YQTax;
                            Tax = totalPriceList.CHILD.NetFare - totalPriceList.CHILD.BaseFare;
                        }
                        else if (passenger.PassengerType == enmPassengerType.Infant)
                        {
                            basePrice = totalPriceList.INFANT.BaseFare;
                            YqTax = totalPriceList.INFANT.YQTax;
                            Tax = totalPriceList.INFANT.NetFare - totalPriceList.INFANT.BaseFare;
                        }
                        string FareDetailId = Guid.NewGuid().ToString();

                        List<tblFlightFareDetailMarkupDetail> tblFlightFareDetailMarkupDetails = null;
                        List<tblFlightFareDetailMLMMarkup> tblFlightFareDetailMLMMarkup = null;
                        List<tblFlightFareDetailDiscount> tblFlightFareDetailDiscount = null;
                        List<tblFlightFareDetailConvenience> tblFlightFareDetailConvenience = null;
                        List<tblFlightServices> tblFlightServices = new List<tblFlightServices>();

                        //Pasenger and Seat markups
                        {
                            tblFlightFareDetailMarkupDetails = GetMarkup(OrgId, CustomerType, searchWraper.DepartureDt, bookingDate, false, enmFlightSearvices.OnPassenger,
                            IsDirectFlight, Airline, searchWraper.From, searchWraper.To, ServiceProvider,
                            totalPriceList.BaseFare, passenger.PassengerType, passenger.Title
                            ).Select(q => new tblFlightFareDetailMarkupDetail
                            {
                                Amount = q.Item2,
                                FareDetailId = FareDetailId,
                                MarkupId = q.Item1
                            }).ToList();
                            tblFlightFareDetailMLMMarkup = GetMarkup(OrgId, CustomerType, searchWraper.DepartureDt, bookingDate, true, enmFlightSearvices.OnPassenger,
                            IsDirectFlight, Airline, searchWraper.From, searchWraper.To, ServiceProvider,
                            totalPriceList.BaseFare, passenger.PassengerType, passenger.Title
                            ).Select(q => new tblFlightFareDetailMLMMarkup
                            {
                                Amount = q.Item2,
                                FareDetailId = FareDetailId,
                                MLMMarkupId = q.Item1
                            }).ToList();
                            tblFlightFareDetailDiscount = GetDiscount(OrgId, CustomerType, searchWraper.DepartureDt, bookingDate, enmFlightSearvices.OnPassenger,
                            IsDirectFlight, Airline, searchWraper.From, searchWraper.To, ServiceProvider,
                            totalPriceList.BaseFare, passenger.PassengerType, passenger.Title
                            ).Select(q => new tblFlightFareDetailDiscount
                            {
                                Amount = q.Item2,
                                FareDetailId = FareDetailId,
                                DiscountId = q.Item1
                            }).ToList();
                            tblFlightFareDetailConvenience = GetConvenience(OrgId, CustomerType, searchWraper.DepartureDt, bookingDate, enmFlightSearvices.OnPassenger,
                            IsDirectFlight, Airline, searchWraper.From, searchWraper.To, ServiceProvider,
                            totalPriceList.BaseFare, passenger.PassengerType, passenger.Title
                            ).Select(q => new tblFlightFareDetailConvenience
                            {
                                Amount = q.Item2,
                                FareDetailId = FareDetailId,
                                ConvenienceId = q.Item1
                            }).ToList();
                            //bagage and Seat Services
                            {
                                passenger.PassengerServices_Baggage.ForEach(a => {
                                    double ServiceChargeAmount = Inward.FirstOrDefault()?.Segment?.Select(q => new { amount = q.sinfo.BAGGAGE.Where(p => p.code == a.Item2).FirstOrDefault().amount }).Select(p => p.amount).FirstOrDefault() ?? 0;
                                    if (ServiceChargeAmount > 0)
                                    {
                                        tblFlightServices.AddRange(GetMarkup(OrgId, CustomerType, searchWraper.DepartureDt, bookingDate, false, enmFlightSearvices.OnBaggageServices,
                                        IsDirectFlight, Airline, searchWraper.From, searchWraper.To, ServiceProvider,
                                        ServiceChargeAmount, passenger.PassengerType, passenger.Title
                                        ).Select(q => new tblFlightServices
                                        {
                                            ServiceType = enmFlightSearvices.OnBaggageServices,
                                            ServiceChargeAmount = ServiceChargeAmount,
                                            MarkupAmount = q.Item2,
                                            ServiceCode = a.Item2,
                                            ServiceKey = a.Item1,
                                            ServiceDescription = a.Item4
                                        }).ToList());
                                    }
                                    
                                });
                                passenger.PassengerServices_Meal.ForEach(a => {
                                    double ServiceChargeAmount = Inward.FirstOrDefault()?.Segment?.Select(q => new { amount = q.sinfo.MEAL.Where(p => p.code == a.Item2).FirstOrDefault().amount }).Select(p => p.amount).FirstOrDefault() ?? 0;
                                    if (ServiceChargeAmount > 0)
                                    {
                                        tblFlightServices.AddRange(GetMarkup(OrgId, CustomerType, searchWraper.DepartureDt, bookingDate, false, enmFlightSearvices.OnMealServices,
                                        IsDirectFlight, Airline, searchWraper.From, searchWraper.To, ServiceProvider,
                                        ServiceChargeAmount, passenger.PassengerType, passenger.Title
                                        ).Select(q => new tblFlightServices
                                        {
                                            ServiceType = enmFlightSearvices.OnMealServices,
                                            ServiceChargeAmount = ServiceChargeAmount,
                                            MarkupAmount = q.Item2,
                                            ServiceCode = a.Item2,
                                            ServiceKey = a.Item1,
                                            ServiceDescription = a.Item4
                                        }).ToList());
                                    }
                                    
                                });
                                passenger.PassengerServices_Seat.ForEach(a => {
                                    double ServiceChargeAmount = Inward.FirstOrDefault()?.Segment?.Select(q => new { amount = q.sinfo.SEAT.Where(p => p.code == a.Item2).FirstOrDefault().amount }).Select(p => p.amount).FirstOrDefault() ?? 0;
                                    if (ServiceChargeAmount > 0)
                                    {
                                        tblFlightServices.AddRange(GetMarkup(OrgId, CustomerType, searchWraper.DepartureDt, bookingDate, false, enmFlightSearvices.OnMealServices,
                                        IsDirectFlight, Airline, searchWraper.From, searchWraper.To, ServiceProvider,
                                        ServiceChargeAmount, passenger.PassengerType, passenger.Title
                                        ).Select(q => new tblFlightServices
                                        {
                                            ServiceType = enmFlightSearvices.OnSeatServices,
                                            ServiceChargeAmount = ServiceChargeAmount,
                                            MarkupAmount = q.Item2,
                                            ServiceCode = a.Item2,
                                            ServiceKey = a.Item1,
                                            ServiceDescription = a.Item4
                                        }).ToList());
                                    }
                                    
                                });
                                passenger.PassengerServices_ExtraService.ForEach(a => {
                                    double ServiceChargeAmount = Inward.FirstOrDefault()?.Segment?.Select(q => new { amount = q.sinfo.EXTRASERVICES.Where(p => p.code == a.Item2).FirstOrDefault().amount }).Select(p => p.amount).FirstOrDefault() ?? 0;
                                    if (ServiceChargeAmount > 0)
                                    {
                                        tblFlightServices.AddRange(GetMarkup(OrgId, CustomerType, searchWraper.DepartureDt, bookingDate, false, enmFlightSearvices.OnMealServices,
                                        IsDirectFlight, Airline, searchWraper.From, searchWraper.To, ServiceProvider,
                                        ServiceChargeAmount, passenger.PassengerType, passenger.Title
                                        ).Select(q => new tblFlightServices
                                        {
                                            ServiceType = enmFlightSearvices.OnExtraService,
                                            ServiceChargeAmount = ServiceChargeAmount,
                                            MarkupAmount = q.Item2,
                                            ServiceCode = a.Item2,
                                            ServiceKey = a.Item1,
                                            ServiceDescription = a.Item4
                                        }).ToList());
                                    }
                                    
                                });

                            }
                        }

                        var ServiceChargeAmountList= from c in tblFlightServices
                                                 group c by new
                                                 {
                                                     c.ServiceType,
                                                     c.ServiceCode,
                                                     c.ServiceChargeAmount,
                                                 } into gcs
                                                 select new 
                                                 {
                                                     ServiceType = gcs.Key.ServiceType,
                                                     ServiceCode = gcs.Key.ServiceCode,
                                                     ServiceChargeAmount = gcs.Key.ServiceChargeAmount,
                                                     MarkupAmount = gcs.Sum(p=>p.MarkupAmount),
                                                 };

                        tblFlightFareDetail tblFlightFareDetail = new tblFlightFareDetail()
                        {
                            FlightFareDetailId = FareDetailId,
                            YQTax = YqTax,
                            BaseFare = basePrice,
                            Tax = Tax,
                            WingMarkup = tblFlightFareDetailMarkupDetails.Sum(q => q.Amount),
                            MLMMarkup = tblFlightFareDetailMLMMarkup.Sum(q => q.Amount),
                            Convenience = tblFlightFareDetailConvenience.Sum(q => q.Amount),
                            TotalFare = 0,
                            Discount = tblFlightFareDetailDiscount.Sum(q => q.Amount),
                            ServiceChargeAmount = ServiceChargeAmountList.Sum(p=>p.ServiceChargeAmount),
                            ServiceChargeMarkup= ServiceChargeAmountList.Sum(p => p.MarkupAmount),
                            NetFare = 0,
                            BookingId = InwardSearchDetails.BookingId,
                            PassengerDetailId = passenger.PassengerDetailId,
                            tblFlightFareDetailMarkupDetail = tblFlightFareDetailMarkupDetails,
                            tblFlightFareDetailMLMMarkup = tblFlightFareDetailMLMMarkup,
                            tblFlightFareDetailConvenience = tblFlightFareDetailConvenience,
                            tblFlightFareDetailDiscount = tblFlightFareDetailDiscount,
                            tblFlightServices= tblFlightServices,
                        };
                        tblFlightFareDetail.TotalFare = tblFlightFareDetail.BaseFare + tblFlightFareDetail.Tax + tblFlightFareDetail.WingMarkup + tblFlightFareDetail.MLMMarkup + tblFlightFareDetail.Convenience
                            + tblFlightFareDetail.ServiceChargeAmount + tblFlightFareDetail.ServiceChargeMarkup;
                        tblFlightFareDetail.NetFare = tblFlightFareDetail.TotalFare - tblFlightFareDetail.Discount;
                        tblFlightFareDetails.Add(tblFlightFareDetail);
                    }
                    InwardSearchDetails.PurchaseAmount = totalPriceList.PurchaseAmount;
                    InwardSearchDetails.PassengerIncentiveAmount = tblFlightFareDetails.Sum(p => p.MLMMarkup);
                    InwardSearchDetails.IncentiveAmount = FlightFareMLMMarkup.Sum(p => p.Amount);
                    InwardSearchDetails.PassengerServiceMarkupAmount = tblFlightFareDetails.Sum(p => p.ServiceChargeMarkup);
                    InwardSearchDetails.PassengerMarkupAmount = tblFlightFareDetails.Sum(p => p.WingMarkup);
                    InwardSearchDetails.MarkupAmount = FlightFareMarkupDetail.Sum(p => p.Amount);
                    InwardSearchDetails.PassengerConvenienceAmount = tblFlightFareDetails.Sum(p => p.Convenience);
                    InwardSearchDetails.ConvenienceAmount = FlightFareConvenience.Sum(p => p.Amount);
                    InwardSearchDetails.PassengerDiscountAmount = tblFlightFareDetails.Sum(p => p.Discount);
                    InwardSearchDetails.DiscountAmount = FlightFareDiscount.Sum(p => p.Amount);
                    InwardSearchDetails.SaleAmount = tblFlightFareDetails.Sum(p => p.NetFare)
                        + InwardSearchDetails.IncentiveAmount +
                        InwardSearchDetails.MarkupAmount + InwardSearchDetails.ConvenienceAmount;
                    InwardSearchDetails.TotalAmount = InwardSearchDetails.SaleAmount + InwardSearchDetails.CustomerMarkupAmount;
                    InwardSearchDetails.NetSaleAmount = InwardSearchDetails.TotalAmount - InwardSearchDetails.DiscountAmount;

                    

                }

                //Return
                if(searchWraper.JourneyType== enmJourneyType.Return)
                {
                    if (Results.Count < 2)
                    {
                        throw new Exception("Return flight not found");
                    }
                    enmServiceProvider ServiceProvider = enmServiceProvider.None;                    
                    var Inward = Results.LastOrDefault();
                    var totalPriceList = Inward.FirstOrDefault()?.TotalPriceList?.FirstOrDefault();
                    if ((Inward.FirstOrDefault()?.Segment?.Count ?? 0) == 1)
                    {
                        IsDirectFlight = true;
                    }
                    ServiceProvider = Inward.FirstOrDefault()?.ServiceProvider ?? enmServiceProvider.None;
                    List<string> Airline = Inward.FirstOrDefault()?.Segment?.Select(p => p.Airline.Name).Distinct().ToList();
                    ReturnSearchDetails = new tblFlightBookingSearchDetails()
                    {
                        BookingId = Guid.NewGuid().ToString(),
                        VisitorId = VisitorId,
                        SegmentId = 2,
                        PurchaseIdentifier = totalPriceList?.Identifier,
                        PurchaseCabinClass = totalPriceList?.CabinClass ?? enmCabinClass.ECONOMY,
                        PurchaseClassOfBooking = totalPriceList?.ClassOfBooking,
                        BookedIdentifier = totalPriceList?.alterIdentifier ?? totalPriceList.Identifier,
                        BookedCabinClass = totalPriceList?.alterCabinClass ?? totalPriceList.CabinClass,
                        BookedClassOfBooking = totalPriceList?.ClassOfBooking,
                        ProviderBookingId = totalPriceList?.ProviderBookingId,
                        ServiceProvider = totalPriceList?.ServiceProvider ?? enmServiceProvider.None,
                        BookingStatus = enmBookingStatus.Pending,
                        PaymentStatus = enmPaymentStatus.Pending,
                        ConvenienceAmount = totalPriceList?.Convenience ?? 0,
                        CustomerMarkupAmount = totalPriceList?.CustomerMarkup ?? 0,
                        DiscountAmount = totalPriceList?.Discount ?? 0,
                        HaveRefund = false,
                        IncentiveAmount = totalPriceList?.MLMMarkup ?? 0,
                        IncludeBaggageServices = totalPriceList?.IncludeBaggageServices ?? false,
                        IncludeMealServices = totalPriceList?.IncludeMealServices ?? false,
                        IncludeSeatServices = totalPriceList?.IncludeSeatServices ?? false,
                        MarkupAmount = totalPriceList?.WingMarkup ?? 0,
                        NetSaleAmount = totalPriceList?.NetFare ?? 0,
                        PurchaseAmount = totalPriceList?.PurchaseAmount ?? 0,
                        PassengerConvenienceAmount = totalPriceList?.PassengerConvenienceAmount ?? 0,
                        PassengerMarkupAmount = totalPriceList?.PassengerMarkupAmount ?? 0,
                        PassengerDiscountAmount = totalPriceList?.PassengerDiscountAmount ?? 0,
                        PassengerIncentiveAmount = totalPriceList?.PassengerIncentiveAmount ?? 0,
                        IsDeleted = false,
                        SaleAmount = totalPriceList?.SaleAmount ?? 0,
                    };
                    //Get Markup on ticket
                    {
                        ReturnFlightFareMarkupDetail =
                        GetMarkup(OrgId, CustomerType, searchWraper.ReturnDt.Value, bookingDate, false, enmFlightSearvices.OnTicket,
                            IsDirectFlight, Airline,  searchWraper.To, searchWraper.From, ServiceProvider,
                        totalPriceList.BaseFare, enmPassengerType.Adult, enmTitle.MR
                        ).Select(q => new tblFlightFareMarkupDetail
                        {
                            Amount = q.Item2,
                            BookingId = ReturnSearchDetails.BookingId,
                            MarkupId = q.Item1
                        }).ToList();
                        ReturnFlightFareMLMMarkup =
                       GetMarkup(OrgId, CustomerType, searchWraper.ReturnDt.Value, bookingDate, true, enmFlightSearvices.OnTicket,
                           IsDirectFlight, Airline,  searchWraper.To, searchWraper.From, ServiceProvider,
                       totalPriceList.BaseFare, enmPassengerType.Adult, enmTitle.MR
                       ).Select(q => new tblFlightFareMLMMarkup
                       {
                           Amount = q.Item2,
                           BookingId = ReturnSearchDetails.BookingId,
                           MLMMarkupId = q.Item1
                       }).ToList();

                        ReturnFlightFareDiscount =
                       GetDiscount(OrgId, CustomerType, searchWraper.ReturnDt.Value, bookingDate, enmFlightSearvices.OnTicket,
                           IsDirectFlight, Airline,  searchWraper.To, searchWraper.From, ServiceProvider,
                       totalPriceList.BaseFare, enmPassengerType.Adult, enmTitle.MR
                       ).Select(q => new tblFlightFareDiscount
                       {
                           Amount = q.Item2,
                           BookingId = ReturnSearchDetails.BookingId,
                           DiscountId = q.Item1
                       }).ToList();
                        ReturnFlightFareConvenience =
                        GetConvenience(OrgId, CustomerType, searchWraper.ReturnDt.Value, bookingDate, enmFlightSearvices.OnTicket,
                            IsDirectFlight, Airline,  searchWraper.To, searchWraper.From, ServiceProvider,
                        totalPriceList.BaseFare, enmPassengerType.Adult, enmTitle.MR
                        ).Select(q => new tblFlightFareConvenience
                        {
                            Amount = q.Item2,
                            BookingId = ReturnSearchDetails.BookingId,
                            ConvenienceId = q.Item1
                        }).ToList();
                    }

                    tblFlightSearchSegmentReturn = Inward.FirstOrDefault()?.Segment.Select(
                    p => new tblFlightSearchSegment()
                    {
                        Id = Guid.NewGuid().ToString(),
                        BookingId = ReturnSearchDetails.BookingId,
                        ProviderSegmentId = p.Id,
                        isLcc = p.Airline.isLcc,
                        Code = p.Airline.Code,
                        FlightNumber = p.Airline.FlightNumber,
                        Name = p.Airline.Name,
                        Mile = p.Mile,
                        TripIndicator = p.TripIndicator,
                        Duration = p.Duration,
                        Layover = p.Layover,
                        OperatingCarrier = p.Airline.OperatingCarrier,
                        OriginAirportCode = p.Origin.AirportCode,
                        OriginAirportName = p.Origin.AirportName,
                        OriginCityCode = p.Origin.CityCode,
                        OriginCityName = p.Origin.CityName,
                        OriginCountryCode = p.Origin.CountryCode,
                        OriginCountryName = p.Origin.CountryName,
                        OriginTerminal = p.Origin.Terminal,
                        DestinationAirportCode = p.Destination.AirportCode,
                        DestinationAirportName = p.Destination.AirportName,
                        DestinationCityCode = p.Destination.CityCode,
                        DestinationCityName = p.Destination.CityName,
                        DestinationCountryCode = p.Destination.CountryCode,
                        DestinationCountryName = p.Destination.CountryName,
                        DestinationTerminal = p.Destination.Terminal,
                        Stops = p.Stops,
                        tblFlightSearchSegmentStops = p.StopDetail?.Select(q => new tblFlightSearchSegmentStops
                        {
                            AirportCode = q.AirportCode,
                            AirportName = q.AirportName,
                            CityCode = q.CityCode,
                            CityName = q.CityName,
                            CountryCode = q.CountryCode,
                            CountryName = q.CountryName,
                            Id = Guid.NewGuid().ToString(),
                        }).ToList()
                    }).ToList();
                    foreach (var passenger in PassengerDetails)
                    {
                        double basePrice = 0, YqTax = 0, Tax = 0;
                        if (passenger.PassengerType == enmPassengerType.Adult)
                        {
                            basePrice = totalPriceList.ADULT.BaseFare;
                            YqTax = totalPriceList.ADULT.YQTax;
                            Tax = totalPriceList.ADULT.NetFare - totalPriceList.ADULT.BaseFare;

                        }
                        else if (passenger.PassengerType == enmPassengerType.Child)
                        {
                            basePrice = totalPriceList.CHILD.BaseFare;
                            YqTax = totalPriceList.CHILD.YQTax;
                            Tax = totalPriceList.CHILD.NetFare - totalPriceList.CHILD.BaseFare;
                        }
                        else if (passenger.PassengerType == enmPassengerType.Infant)
                        {
                            basePrice = totalPriceList.INFANT.BaseFare;
                            YqTax = totalPriceList.INFANT.YQTax;
                            Tax = totalPriceList.INFANT.NetFare - totalPriceList.INFANT.BaseFare;
                        }
                        string FareDetailId = Guid.NewGuid().ToString();

                        List<tblFlightFareDetailMarkupDetail> tblFlightFareDetailMarkupDetails = null;
                        List<tblFlightFareDetailMLMMarkup> tblFlightFareDetailMLMMarkup = null;
                        List<tblFlightFareDetailDiscount> tblFlightFareDetailDiscount = null;
                        List<tblFlightFareDetailConvenience> tblFlightFareDetailConvenience = null;
                        List<tblFlightServices> tblFlightServices = new List<tblFlightServices>();

                        //Pasenger and Seat markups
                        {
                            tblFlightFareDetailMarkupDetails = GetMarkup(OrgId, CustomerType, searchWraper.ReturnDt.Value, bookingDate, false, enmFlightSearvices.OnPassenger,
                            IsDirectFlight, Airline, searchWraper.To, searchWraper.From,  ServiceProvider,
                            totalPriceList.BaseFare, passenger.PassengerType, passenger.Title
                            ).Select(q => new tblFlightFareDetailMarkupDetail
                            {
                                Amount = q.Item2,
                                FareDetailId = FareDetailId,
                                MarkupId = q.Item1
                            }).ToList();
                            tblFlightFareDetailMLMMarkup = GetMarkup(OrgId, CustomerType, searchWraper.ReturnDt.Value, bookingDate, true, enmFlightSearvices.OnPassenger,
                            IsDirectFlight, Airline, searchWraper.To, searchWraper.From,  ServiceProvider,
                            totalPriceList.BaseFare, passenger.PassengerType, passenger.Title
                            ).Select(q => new tblFlightFareDetailMLMMarkup
                            {
                                Amount = q.Item2,
                                FareDetailId = FareDetailId,
                                MLMMarkupId = q.Item1
                            }).ToList();
                            tblFlightFareDetailDiscount = GetDiscount(OrgId, CustomerType, searchWraper.ReturnDt.Value, bookingDate, enmFlightSearvices.OnPassenger,
                            IsDirectFlight, Airline, searchWraper.To, searchWraper.From,  ServiceProvider,
                            totalPriceList.BaseFare, passenger.PassengerType, passenger.Title
                            ).Select(q => new tblFlightFareDetailDiscount
                            {
                                Amount = q.Item2,
                                FareDetailId = FareDetailId,
                                DiscountId = q.Item1
                            }).ToList();
                            tblFlightFareDetailConvenience = GetConvenience(OrgId, CustomerType, searchWraper.ReturnDt.Value, bookingDate, enmFlightSearvices.OnPassenger,
                            IsDirectFlight, Airline, searchWraper.To, searchWraper.From, ServiceProvider,
                            totalPriceList.BaseFare, passenger.PassengerType, passenger.Title
                            ).Select(q => new tblFlightFareDetailConvenience
                            {
                                Amount = q.Item2,
                                FareDetailId = FareDetailId,
                                ConvenienceId = q.Item1
                            }).ToList();
                            //bagage and Seat Services
                            {
                                passenger.PassengerServices_Baggage.ForEach(a => {
                                    double ServiceChargeAmount = Inward.FirstOrDefault()?.Segment?.Select(q => new { amount = q.sinfo.BAGGAGE.Where(p => p.code == a.Item2).FirstOrDefault().amount }).Select(p => p.amount).FirstOrDefault() ?? 0;
                                    if (ServiceChargeAmount > 0)
                                    {
                                        tblFlightServices.AddRange(GetMarkup(OrgId, CustomerType, searchWraper.ReturnDt.Value, bookingDate, false, enmFlightSearvices.OnBaggageServices,
                                        IsDirectFlight, Airline, searchWraper.To, searchWraper.From, ServiceProvider,
                                        ServiceChargeAmount, passenger.PassengerType, passenger.Title
                                        ).Select(q => new tblFlightServices
                                        {
                                            ServiceType = enmFlightSearvices.OnBaggageServices,
                                            ServiceChargeAmount = ServiceChargeAmount,
                                            MarkupAmount = q.Item2,
                                            ServiceCode = a.Item2,
                                            ServiceKey = a.Item1,
                                            ServiceDescription = a.Item4
                                        }).ToList());
                                    }

                                });
                                passenger.PassengerServices_Meal.ForEach(a => {
                                    double ServiceChargeAmount = Inward.FirstOrDefault()?.Segment?.Select(q => new { amount = q.sinfo.MEAL.Where(p => p.code == a.Item2).FirstOrDefault().amount }).Select(p => p.amount).FirstOrDefault() ?? 0;
                                    if (ServiceChargeAmount > 0)
                                    {
                                        tblFlightServices.AddRange(GetMarkup(OrgId, CustomerType, searchWraper.ReturnDt.Value, bookingDate, false, enmFlightSearvices.OnMealServices,
                                        IsDirectFlight, Airline, searchWraper.To, searchWraper.From,  ServiceProvider,
                                        ServiceChargeAmount, passenger.PassengerType, passenger.Title
                                        ).Select(q => new tblFlightServices
                                        {
                                            ServiceType = enmFlightSearvices.OnMealServices,
                                            ServiceChargeAmount = ServiceChargeAmount,
                                            MarkupAmount = q.Item2,
                                            ServiceCode = a.Item2,
                                            ServiceKey = a.Item1,
                                            ServiceDescription = a.Item4
                                        }).ToList());
                                    }

                                });
                                passenger.PassengerServices_Seat.ForEach(a => {
                                    double ServiceChargeAmount = Inward.FirstOrDefault()?.Segment?.Select(q => new { amount = q.sinfo.SEAT.Where(p => p.code == a.Item2).FirstOrDefault().amount }).Select(p => p.amount).FirstOrDefault() ?? 0;
                                    if (ServiceChargeAmount > 0)
                                    {
                                        tblFlightServices.AddRange(GetMarkup(OrgId, CustomerType, searchWraper.ReturnDt.Value, bookingDate, false, enmFlightSearvices.OnMealServices,
                                        IsDirectFlight, Airline, searchWraper.To, searchWraper.From,  ServiceProvider,
                                        ServiceChargeAmount, passenger.PassengerType, passenger.Title
                                        ).Select(q => new tblFlightServices
                                        {
                                            ServiceType = enmFlightSearvices.OnSeatServices,
                                            ServiceChargeAmount = ServiceChargeAmount,
                                            MarkupAmount = q.Item2,
                                            ServiceCode = a.Item2,
                                            ServiceKey = a.Item1,
                                            ServiceDescription = a.Item4
                                        }).ToList());
                                    }

                                });
                                passenger.PassengerServices_ExtraService.ForEach(a => {
                                    double ServiceChargeAmount = Inward.FirstOrDefault()?.Segment?.Select(q => new { amount = q.sinfo.EXTRASERVICES.Where(p => p.code == a.Item2).FirstOrDefault().amount }).Select(p => p.amount).FirstOrDefault() ?? 0;
                                    if (ServiceChargeAmount > 0)
                                    {
                                        tblFlightServices.AddRange(GetMarkup(OrgId, CustomerType, searchWraper.ReturnDt.Value, bookingDate, false, enmFlightSearvices.OnMealServices,
                                        IsDirectFlight, Airline, searchWraper.To, searchWraper.From, ServiceProvider,
                                        ServiceChargeAmount, passenger.PassengerType, passenger.Title
                                        ).Select(q => new tblFlightServices
                                        {
                                            ServiceType = enmFlightSearvices.OnExtraService,
                                            ServiceChargeAmount = ServiceChargeAmount,
                                            MarkupAmount = q.Item2,
                                            ServiceCode = a.Item2,
                                            ServiceKey = a.Item1,
                                            ServiceDescription = a.Item4
                                        }).ToList());
                                    }

                                });

                            }
                        }

                        var ServiceChargeAmountList = from c in tblFlightServices
                                                      group c by new
                                                      {
                                                          c.ServiceType,
                                                          c.ServiceCode,
                                                          c.ServiceChargeAmount,
                                                      } into gcs
                                                      select new
                                                      {
                                                          ServiceType = gcs.Key.ServiceType,
                                                          ServiceCode = gcs.Key.ServiceCode,
                                                          ServiceChargeAmount = gcs.Key.ServiceChargeAmount,
                                                          MarkupAmount = gcs.Sum(p => p.MarkupAmount),
                                                      };

                        tblFlightFareDetail tblFlightFareDetail = new tblFlightFareDetail()
                        {
                            FlightFareDetailId = FareDetailId,
                            YQTax = YqTax,
                            BaseFare = basePrice,
                            Tax = Tax,
                            WingMarkup = tblFlightFareDetailMarkupDetails.Sum(q => q.Amount),
                            MLMMarkup = tblFlightFareDetailMLMMarkup.Sum(q => q.Amount),
                            Convenience = tblFlightFareDetailConvenience.Sum(q => q.Amount),
                            TotalFare = 0,
                            Discount = tblFlightFareDetailDiscount.Sum(q => q.Amount),
                            ServiceChargeAmount = ServiceChargeAmountList.Sum(p => p.ServiceChargeAmount),
                            ServiceChargeMarkup = ServiceChargeAmountList.Sum(p => p.MarkupAmount),
                            NetFare = 0,
                            BookingId = ReturnSearchDetails.BookingId,
                            PassengerDetailId = passenger.PassengerDetailId,
                            tblFlightFareDetailMarkupDetail = tblFlightFareDetailMarkupDetails,
                            tblFlightFareDetailMLMMarkup = tblFlightFareDetailMLMMarkup,
                            tblFlightFareDetailConvenience = tblFlightFareDetailConvenience,
                            tblFlightFareDetailDiscount = tblFlightFareDetailDiscount,
                            tblFlightServices = tblFlightServices,
                        };
                        tblFlightFareDetail.TotalFare = tblFlightFareDetail.BaseFare + tblFlightFareDetail.Tax + tblFlightFareDetail.WingMarkup + tblFlightFareDetail.MLMMarkup + tblFlightFareDetail.Convenience
                            + tblFlightFareDetail.ServiceChargeAmount + tblFlightFareDetail.ServiceChargeMarkup;
                        tblFlightFareDetail.NetFare = tblFlightFareDetail.TotalFare - tblFlightFareDetail.Discount;
                        ReturntblFlightFareDetails.Add(tblFlightFareDetail);
                    }
                    ReturnSearchDetails.PurchaseAmount = totalPriceList.PurchaseAmount;
                    ReturnSearchDetails.PassengerIncentiveAmount = ReturntblFlightFareDetails.Sum(p => p.MLMMarkup);
                    ReturnSearchDetails.IncentiveAmount = ReturnFlightFareMLMMarkup.Sum(p => p.Amount);
                    ReturnSearchDetails.PassengerServiceMarkupAmount = ReturntblFlightFareDetails.Sum(p => p.ServiceChargeMarkup);
                    ReturnSearchDetails.PassengerMarkupAmount = ReturntblFlightFareDetails.Sum(p => p.WingMarkup);
                    ReturnSearchDetails.MarkupAmount = ReturnFlightFareMarkupDetail.Sum(p => p.Amount);
                    ReturnSearchDetails.PassengerConvenienceAmount = ReturntblFlightFareDetails.Sum(p => p.Convenience);
                    ReturnSearchDetails.ConvenienceAmount = ReturnFlightFareConvenience.Sum(p => p.Amount);
                    ReturnSearchDetails.PassengerDiscountAmount = ReturntblFlightFareDetails.Sum(p => p.Discount);
                    ReturnSearchDetails.DiscountAmount = ReturnFlightFareDiscount.Sum(p => p.Amount);
                    ReturnSearchDetails.SaleAmount = ReturntblFlightFareDetails.Sum(p => p.NetFare)
                        + ReturnSearchDetails.IncentiveAmount +
                        ReturnSearchDetails.MarkupAmount + ReturnSearchDetails.ConvenienceAmount;
                    ReturnSearchDetails.TotalAmount = ReturnSearchDetails.SaleAmount + ReturnSearchDetails.CustomerMarkupAmount;
                    ReturnSearchDetails.NetSaleAmount = ReturnSearchDetails.TotalAmount - ReturnSearchDetails.DiscountAmount;

                }
                //Set Master Total Value
                {

                    fbm.BookingAmount = InwardSearchDetails?.SaleAmount ?? 0 + ReturnSearchDetails?.NetSaleAmount ?? 0;
                }

                _travelContext.tblFlightBookingMaster.Add(fbm);
                _travelContext.tblFlighBookingPassengerDetails.AddRange(PassengerDetails);
                _travelContext.tblFlightBookingSearchDetails.Add(InwardSearchDetails);
                _travelContext.tblFlightSearchSegment.AddRange(tblFlightSearchSegmentInward);
                _travelContext.tblFlightFareMarkupDetail.AddRange(FlightFareMarkupDetail);
                _travelContext.tblFlightFareMLMMarkup.AddRange(FlightFareMLMMarkup);
                _travelContext.tblFlightFareDiscount.AddRange(FlightFareDiscount);
                _travelContext.tblFlightFareConvenience.AddRange(FlightFareConvenience);
                _travelContext.tblFlightFareDetail.AddRange(tblFlightFareDetails);
                

                _travelContext.tblFlightBookingSearchDetails.Add(ReturnSearchDetails);
                _travelContext.tblFlightSearchSegment.AddRange(tblFlightSearchSegmentReturn);
                _travelContext.tblFlightFareMarkupDetail.AddRange(ReturnFlightFareMarkupDetail);
                _travelContext.tblFlightFareMLMMarkup.AddRange(ReturnFlightFareMLMMarkup);
                _travelContext.tblFlightFareDiscount.AddRange(ReturnFlightFareDiscount);
                _travelContext.tblFlightFareConvenience.AddRange(ReturnFlightFareConvenience);
                _travelContext.tblFlightFareDetail.AddRange(ReturntblFlightFareDetails);
                _travelContext.SaveChanges();
                return fbm;

            }
            else
            {   
                if (ExistingBooking.BookingStatus == enmBookingStatus.Pending)
                {   
                    var FlightBookingSearchDetails = _travelContext.tblFlightBookingSearchDetails.Where(q => q.VisitorId == VisitorId).ToList();
                    FlightBookingSearchDetails.ForEach(p => {
                        RemoveAllMarkup(p.BookingId);
                    });
                }
                return ExistingBooking;
            }            
            
            

            void RemoveAllMarkup(string BookingId)
            {
                _travelContext.Database.ExecuteSqlCommand("delete from tblFlightFareMarkupDetail Where BookingId=@p1", BookingId);
                _travelContext.Database.ExecuteSqlCommand("delete from tblFlightFareMLMMarkup Where BookingId=@p1", BookingId);
                _travelContext.Database.ExecuteSqlCommand("delete from tblFlightFareDiscount Where BookingId=@p1", BookingId);
                _travelContext.Database.ExecuteSqlCommand("delete from tblFlightFareConvenience Where BookingId=@p1", BookingId);
                _travelContext.Database.ExecuteSqlCommand("delete from tblFlightFareDetailMarkupDetail Where FareDetailId in ( select FlightFareDetailId from tblFlightFareDetail Where BookingId=@p1)", BookingId);
                _travelContext.Database.ExecuteSqlCommand("delete from tblFlightFareDetailMLMMarkup Where FareDetailId in ( select FlightFareDetailId from tblFlightFareDetail Where BookingId=@p1)", BookingId);
                _travelContext.Database.ExecuteSqlCommand("delete from tblFlightFareDetailDiscount Where FareDetailId in ( select FlightFareDetailId from tblFlightFareDetail Where BookingId=@p1)", BookingId);
                _travelContext.Database.ExecuteSqlCommand("delete from tblFlightFareDetailConvenience Where FareDetailId in ( select FlightFareDetailId from tblFlightFareDetail Where BookingId=@p1)", BookingId);
                _travelContext.Database.ExecuteSqlCommand("delete from tblFlightFareDetail Where BookingId=@p1", BookingId);                

            }

        }


        public mdlReturnData SaveFareQuote(mdlFareQuotRequestWraper request, IsrvCurrentUser _IsrvCurrentUser, List<mdlFareQuotResponse> response, int OrgId, ulong Nid)
        {
            bool IsUpdate = false;
            string VisitorId;
            DateTime BookingDt = DateTime.Now;
            mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.None };
            if (response == null || response.Count == 0)
            {
                mdl.MessageType = enmMessageType.Error;
                mdl.Message = enmMessage.DataNotFound.GetDescription();
                return mdl;
            }
            var Master = _travelContext.tblFlightBookingMaster.Where(p => p.VisitorId == request.TraceId).FirstOrDefault();
            var res = response.FirstOrDefault();
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
                Master.ModifiedBy = _IsrvCurrentUser.UserId;
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
                string BookingId = Guid.NewGuid().ToString();
                var tempSerach = _travelContext.tblFlightBookingSearchDetails.Where(p => p.SegmentId == SegmentId && p.VisitorId == VisitorId && p.IsDeleted == false).FirstOrDefault();
                if (tempSerach != null)
                {
                    tempSerach.IsDeleted = true;
                }
                tblFlightBookingSearchDetails mdlSearch = new tblFlightBookingSearchDetails()
                {
                    BookingId = BookingId,
                    VisitorId = VisitorId,
                    SegmentId = SegmentId,
                    //PurchaseIdentifier = response[SegmentId - 1].PurchaseIdentifier,
                    //PurchaseCabinClass = response[SegmentId - 1].PurchaseCabinClass,
                    //PurchaseClassOfBooking = response[SegmentId - 1].PurchaseClassOfBooking,
                    //BookedIdentifier = response[SegmentId - 1].BookedIdentifier,
                    //BookedCabinClass = response[SegmentId - 1].BookedCabinClass,
                    //BookedClassOfBooking = response[SegmentId - 1].BookedClassOfBooking,
                    ProviderBookingId = response[SegmentId - 1].BookingId,
                    ServiceProvider = response[SegmentId - 1].ServiceProvider,
                    BookingStatus = enmBookingStatus.Pending,
                    PaymentStatus = enmPaymentStatus.Pending,
                    IncludeBaggageServices = false,
                    IncludeMealServices = false,
                    IncludeSeatServices = false,
                    PurchaseAmount = response[SegmentId - 1].TotalPriceInfo?.NetFare ?? 0,
                    IncentiveAmount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].MLMMarkup,
                    ConvenienceAmount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].Convenience,
                    MarkupAmount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].WingMarkup,
                    CustomerMarkupAmount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].CustomerMarkup,
                    DiscountAmount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].Discount,
                    SaleAmount = response[SegmentId - 1].Results[0][0].TotalPriceList[0].NetFare
                };
                mdlSearch.NetSaleAmount = mdlSearch.SaleAmount + mdlSearch.IncentiveAmount + mdlSearch.ConvenienceAmount
                    + mdlSearch.MarkupAmount - mdlSearch.DiscountAmount;
                if (tempSerach != null)
                {
                    _travelContext.tblFlightBookingSearchDetails.Update(tempSerach);
                }
                _travelContext.tblFlightBookingSearchDetails.Add(mdlSearch);

                foreach (var seg in response[SegmentId - 1].Results[0][0].Segment)
                {
                    tblFlightSearchSegment fsg = new tblFlightSearchSegment()
                    {
                        ProviderSegmentId = seg.Id.ToString(),
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
                
                //_travelContext.tblFlightFare.Add(flightFare);
                _travelContext.SaveChanges();
            }


        }

        public async Task<mdlReturnData> BookingAsync(IsrvWallet isrvWallet, mdlBookingRequest mdlRq,
            int CustomerId, int EmployeeId, ulong Nid, ulong UserId, enmCustomerType customerType)
        {
            mdlReturnData ReturnData = new mdlReturnData() { MessageType = enmMessageType.None };
            DateTime CurrentDate = DateTime.Now;
            mdlBookingResponse mdlRs = new mdlBookingResponse();
            var Quote=_travelContext.tblFlightBookingMaster.Where(p => p.VisitorId == mdlRq.VisitorId).FirstOrDefault();
            if (Quote == null)
            {
                throw new Exception("Invalid Visitor ID");
            }
            else if (Quote.BookingStatus != enmBookingStatus.Pending)
            {
                throw new Exception("Ticket is already booked");
            }
            if (mdlRq.WalletAmount > 0)
            {
                if (mdlRq.WalletAmount < isrvWallet.GetWalletBalance(CustomerId, Nid, EmployeeId, customerType))
                {
                    throw new Exception(enmMessage.InsufficientBalance.GetDescription());
                }      
            }
            
            var BookingDatas=  _travelContext.tblFlightBookingSearchDetails.Where(p => p.VisitorId == mdlRq.VisitorId && !p.IsDeleted).ToList();
            bool IsDomectic = IsDomecticFlight(Quote.From, Quote.To);
            enmBookingStatus BookingStatus = enmBookingStatus.Pending;
            if (GetInstantBookingSeting(true, CurrentDate).Where(p => p.CustomerType == customerType && ((p.InstantDomestic && IsDomectic) || (p.InstantNonDomestic && !IsDomectic))).Any())
            {
                BookingStatus = enmBookingStatus.Booked;
            }
            else
            {
                BookingStatus = enmBookingStatus.Hold;
            }
            List<mdlBookingResponse> bres = new List<mdlBookingResponse>();

            foreach (var bd in BookingDatas)
            {

                IWingFlight wingflight = GetFlightObject(bd.ServiceProvider);
                mdlBookingRequest br = new mdlBookingRequest()
                {
                    VisitorId = mdlRq.VisitorId,
                    BookingId = mdlRq.BookingId,
                    travellerInfo = mdlRq.travellerInfo,
                    deliveryInfo = mdlRq.deliveryInfo,
                    gstInfo = mdlRq.gstInfo,
                    NetAmount = bd.PurchaseAmount
                };
                
                bres.Add(await wingflight.BookingAsync(br, BookingStatus));
            }

            if (!bres.Any(p => p.ResponseStatus == enmMessageType.Error))
            {
                ReturnData.MessageType = enmMessageType.Success;
            }
            else if(bres.All(p => p.ResponseStatus == enmMessageType.Error))
            {
                ReturnData.MessageType = enmMessageType.Error;
            }
            else if (bres.Any(p => p.ResponseStatus == enmMessageType.Error))
            {
                ReturnData.MessageType = enmMessageType.Warning;
            }
            ReturnData.ReturnId = bres;

            return ReturnData;
        }
    }





}

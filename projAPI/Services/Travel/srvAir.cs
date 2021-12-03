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

    public class srvAir :  IsrvAir
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




        #region *********************** Setting **************************

        public mdlReturnData SetServiceProvider(DateTime EffectiveFromDate, enmServiceProvider ServiceProvider, bool IsEnable, ulong UserId, string Remarks)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.None };
            var TempData = _travelContext.tblFlightSerivceProvider.Where(p => p.ServiceProvider == ServiceProvider && p.EffectiveFromDate == EffectiveFromDate && !p.IsDeleted).FirstOrDefault();
            if (TempData != null)
            {
                TempData.IsDeleted = true;
                TempData.ModifiedDt = DateTime.Now;
                TempData.ModifiedBy = UserId;
                TempData.ModifyRemarks = TempData.ModifyRemarks + Remarks;
                _travelContext.tblFlightSerivceProvider.Update(TempData);
            }
            tblFlightSerivceProvider mdl = new tblFlightSerivceProvider()
            {
                CreatedBy = UserId,
                ModifiedBy = UserId,
                CreatedDt = DateTime.Now,
                ModifiedDt = DateTime.Now,
                IsDeleted = false,
                ModifyRemarks = Remarks ?? String.Empty,
                ServiceProvider = ServiceProvider,
                EffectiveFromDate = EffectiveFromDate,
                IsEnabled = IsEnable,
            };
            _travelContext.tblFlightSerivceProvider.Add(mdl);
            _travelContext.SaveChanges();
            returnData.MessageType = enmMessageType.Success;
            returnData.ReturnId = mdl;
            return returnData;
        }

        public List<tblFlightSerivceProvider> GetServiceProvider(DateTime ProcessDate, bool IsOnlyActive)
        {
            List<tblFlightSerivceProvider> Providers = new List<tblFlightSerivceProvider>();
            foreach (enmServiceProvider provider in Enum.GetValues(typeof(enmServiceProvider)))
            {
                if (provider == enmServiceProvider.None)
                {
                    continue;
                }
                var sp = _travelContext.tblFlightSerivceProvider.Where(p => !p.IsDeleted && p.ServiceProvider == provider && p.EffectiveFromDate <= ProcessDate).OrderByDescending(p => p.EffectiveFromDate).Take(1).FirstOrDefault();
                if (sp == null)
                {
                    var tempData = SetServiceProvider(ProcessDate, provider, true, 1, string.Empty);
                    if (tempData.MessageType == enmMessageType.Success)
                    {
                        Providers.Add(tempData.ReturnId);
                    }
                }
                else
                {
                    Providers.Add(sp);
                }
            }
            if (IsOnlyActive)
            {
                return Providers.Where(p => p.IsEnabled).ToList();
            }
            else
            {
                return Providers;
            }

        }

        public mdlReturnData SetServiceProviderPriority(DateTime EffectiveFromDate, enmServiceProvider ServiceProvider, int priority, ulong UserId, string Remarks)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.None };
            var TempData = _travelContext.tblFlightSerivceProviderPriority.Where(p => p.ServiceProvider == ServiceProvider && p.EffectiveFromDate == EffectiveFromDate && !p.IsDeleted).FirstOrDefault();
            if (TempData != null)
            {
                TempData.IsDeleted = true;
                TempData.ModifiedDt = DateTime.Now;
                TempData.ModifiedBy = UserId;
                TempData.ModifyRemarks = TempData.ModifyRemarks + Remarks;
                _travelContext.tblFlightSerivceProviderPriority.Update(TempData);
            }
            tblFlightSerivceProviderPriority mdl = new tblFlightSerivceProviderPriority()
            {
                CreatedBy = UserId,
                ModifiedBy = UserId,
                CreatedDt = DateTime.Now,
                ModifiedDt = DateTime.Now,
                IsDeleted = false,
                ModifyRemarks = Remarks ?? String.Empty,
                ServiceProvider = ServiceProvider,
                EffectiveFromDate = EffectiveFromDate,
                priority = priority,
            };
            _travelContext.tblFlightSerivceProviderPriority.Add(mdl);
            _travelContext.SaveChanges();
            returnData.MessageType = enmMessageType.Success;
            returnData.ReturnId = mdl;
            return returnData;
        }

        public List<tblFlightSerivceProviderPriority> GetServiceProviderPriority(DateTime ProcessDate, bool IsOnlyActive)
        {
            List<tblFlightSerivceProviderPriority> Providers = new List<tblFlightSerivceProviderPriority>();
            foreach (enmServiceProvider provider in Enum.GetValues(typeof(enmServiceProvider)))
            {
                if (provider == enmServiceProvider.None)
                {
                    continue;
                }

                var sp = _travelContext.tblFlightSerivceProviderPriority.Where(p => !p.IsDeleted && p.ServiceProvider == provider && p.EffectiveFromDate <= ProcessDate).OrderByDescending(p => p.EffectiveFromDate).Take(1).FirstOrDefault();
                if (sp == null)
                {
                    var tempData = SetServiceProviderPriority(ProcessDate, provider, (int)provider, 1, string.Empty);
                    if (tempData.MessageType == enmMessageType.Success)
                    {
                        Providers.Add(tempData.ReturnId);
                    }
                }
                else
                {
                    Providers.Add(sp);
                }
            }

            return Providers;


        }


        public mdlReturnData SetInstantBookingSeting(DateTime EffectiveFromDate, enmCustomerType CustomerType, bool InstantDomestic, bool InstantNonDomestic, ulong UserId, string Remarks)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.None };
            var TempData = _travelContext.tblFlightInstantBooking.Where(p => p.CustomerType == CustomerType && p.EffectiveFromDate == EffectiveFromDate && !p.IsDeleted).FirstOrDefault();
            if (TempData != null)
            {
                TempData.IsDeleted = true;
                TempData.ModifiedDt = DateTime.Now;
                TempData.ModifiedBy = UserId;
                TempData.ModifyRemarks = TempData.ModifyRemarks + Remarks;
                _travelContext.tblFlightInstantBooking.Update(TempData);
            }
            tblFlightInstantBooking mdl = new tblFlightInstantBooking()
            {
                CreatedBy = UserId,
                ModifiedBy = UserId,
                CreatedDt = DateTime.Now,
                ModifiedDt = DateTime.Now,
                IsDeleted = false,
                ModifyRemarks = Remarks ?? String.Empty,
                CustomerType = CustomerType,
                EffectiveFromDate = EffectiveFromDate,
                InstantDomestic = InstantDomestic,
                InstantNonDomestic = InstantNonDomestic,
            };
            _travelContext.tblFlightInstantBooking.Add(mdl);
            _travelContext.SaveChanges();
            returnData.MessageType = enmMessageType.Success;
            returnData.ReturnId = mdl;
            return returnData;
        }

        public List<tblFlightInstantBooking> GetInstantBookingSeting(bool FilterDate, DateTime ProcessDate)
        {
            List<tblFlightInstantBooking> returnData = new List<tblFlightInstantBooking>();
            foreach (enmCustomerType ctype in Enum.GetValues(typeof(enmCustomerType)))
            {
                if (FilterDate)
                {
                    var sp = _travelContext.tblFlightInstantBooking.Where(p => !p.IsDeleted && p.CustomerType == ctype && p.EffectiveFromDate <= ProcessDate).OrderByDescending(p => p.EffectiveFromDate).Take(1).FirstOrDefault();
                    if (sp == null)
                    {
                        var tempData = SetInstantBookingSeting(ProcessDate, ctype, true, false, 1, string.Empty);
                        if (tempData.MessageType == enmMessageType.Success)
                        {
                            returnData.Add(tempData.ReturnId);
                        }
                    }
                    else
                    {
                        returnData.Add(sp);
                    }
                }
                else
                {
                    returnData.AddRange(_travelContext.tblFlightInstantBooking.Where(p => !p.IsDeleted && p.CustomerType == ctype).OrderByDescending(p => p.EffectiveFromDate).Take(20));
                }

            }

            return returnData;
        }

        public mdlReturnData SetFlightBookingAlterMaster(mdlFlightAlter mdl, ulong UserId)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.None };
            //Check is data already Exist if yes then Remove Existing
            var tempData = _travelContext.tblFlightBookingAlterMaster.Where(p => p.CabinClass == mdl.CabinClass && p.ClassOfBooking == mdl.ClassOfBooking && p.Identifier == mdl.Identifier && !p.IsDeleted).ToList();
            if (tempData != null && tempData.Count > 0)
            {
                tempData.ForEach(p => { p.IsDeleted = true; p.ModifiedBy = UserId; p.ModifiedDt = DateTime.Now; p.ModifyRemarks = "data alter"; });
                _travelContext.tblFlightBookingAlterMaster.UpdateRange(tempData);
            }
            tblFlightBookingAlterMaster mdl1 = new tblFlightBookingAlterMaster()
            {
                CreatedBy = UserId,
                ModifiedBy = UserId,
                CreatedDt = DateTime.Now,
                ModifiedDt = DateTime.Now,
                IsDeleted = false,
                ModifyRemarks = mdl.Remarks ?? string.Empty,
                CabinClass = mdl.CabinClass,
                ClassOfBooking = mdl.ClassOfBooking,
                Identifier = mdl.Identifier,
                tblFlightBookingAlterDetails = mdl.AlterDetails.Select(q => new tblFlightBookingAlterDetails { CabinClass = q.Item1, Identifier = q.Item2, ClassOfBooking = q.Item3 }).ToList()
            };
            _travelContext.tblFlightBookingAlterMaster.Add(mdl1);
            _travelContext.SaveChanges();
            returnData.MessageType = enmMessageType.Success;
            returnData.ReturnId = mdl;
            return returnData;
        }

        public List<mdlFlightAlter> GetFlightBookingAlterMaster()
        {
            return _travelContext.tblFlightBookingAlterMaster.Where(p => !p.IsDeleted).Select(p => new mdlFlightAlter
            {
                AlterId = p.AlterId,
                CabinClass = p.CabinClass,
                Identifier = p.Identifier,
                ClassOfBooking = p.ClassOfBooking,
                AlterDetails = p.tblFlightBookingAlterDetails.Select(q => new Tuple<enmCabinClass, string, string>(q.CabinClass, q.Identifier, q.ClassOfBooking)).ToList()

            }
            ).ToList();
        }

        public mdlReturnData SetFlightFareFilter(mdlFlightFareFilter mdl, ulong UserId)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.None };
            //Check is data already Exist if yes then Remove Existing
            var tempData = _travelContext.tblFlightFareFilter.Where(p => p.CustomerType == mdl.CustomerType && !p.IsDeleted).ToList();
            if (tempData != null && tempData.Count > 0)
            {
                tempData.ForEach(p => { p.IsDeleted = true; p.ModifiedBy = UserId; p.ModifiedDt = DateTime.Now; p.ModifyRemarks = "data alter"; });
                _travelContext.tblFlightFareFilter.UpdateRange(tempData);
            }
            tblFlightFareFilter mdl1 = new tblFlightFareFilter()
            {
                CreatedBy = UserId,
                ModifiedBy = UserId,
                CreatedDt = DateTime.Now,
                ModifiedDt = DateTime.Now,
                IsDeleted = false,
                ModifyRemarks = mdl.Remarks ?? string.Empty,
                CustomerType = mdl.CustomerType,
                IsEanableAllFare = mdl.IsEanableAllFare,
                tblFlightFareFilterDetails = mdl.FilterDetails.Select(q => new tblFlightFareFilterDetails { Identifier = q.Item1, ClassOfBooking = q.Item2 }).ToList()
            };
            _travelContext.tblFlightFareFilter.Add(mdl1);
            _travelContext.SaveChanges();
            returnData.MessageType = enmMessageType.Success;
            returnData.ReturnId = mdl;
            return returnData;
        }

        public List<mdlFlightFareFilter> GetFlightFareFilter(bool ApplyCustomerFilter, enmCustomerType CustomerType)
        {
            if (ApplyCustomerFilter)
            {
                return _travelContext.tblFlightFareFilter.Where(p => !p.IsDeleted && p.CustomerType == CustomerType).Select(p => new mdlFlightFareFilter
                {
                    FilterId = p.FilterId,
                    IsEanableAllFare = p.IsEanableAllFare,
                    CustomerType = p.CustomerType,
                    FilterDetails = p.tblFlightFareFilterDetails.Select(q => new Tuple<string, string>(q.Identifier, q.ClassOfBooking)).ToList()
                }
           ).ToList();
            }
            else
            {
                return _travelContext.tblFlightFareFilter.Where(p => !p.IsDeleted).Select(p => new mdlFlightFareFilter
                {
                    FilterId = p.FilterId,
                    IsEanableAllFare = p.IsEanableAllFare,
                    CustomerType = p.CustomerType,
                    FilterDetails = p.tblFlightFareFilterDetails.Select(q => new Tuple<string, string>(q.Identifier, q.ClassOfBooking)).ToList()
                }
           ).ToList();
            }

        }


        #endregion

        #region ****************** Get All Markup/Discount/convenience **********************
        public List<tblFlightCustomerMarkup> GetCustomerMarkup(bool AllMarkup, bool AllActiveMarkup, DateTime ProcessingDate, int CustomerId, ulong Nid, enmCustomerType CustomerType)
        {

            IQueryable<tblFlightCustomerMarkup> returnData = CustomerType == enmCustomerType.MLM ?
                _travelContext.tblFlightCustomerMarkup.Where(p => p.Nid == Nid).AsQueryable() :
                _travelContext.tblFlightCustomerMarkup.Where(p => p.CustomerId == CustomerId).AsQueryable();

            if (AllMarkup)
            {
                returnData.ToList();
            }
            else if (AllActiveMarkup)
            {
                returnData = returnData.Where(p => !p.IsDeleted);
            }
            else
            {
                returnData = returnData.Where(p => !p.IsDeleted && p.EffectiveFromDt <= ProcessingDate && p.EffectiveToDt > ProcessingDate);
            }
            return returnData.ToList();

        }
        public mdlReturnData SetCustomerMarkup(double MarkupAmount, DateTime EffectiveFromDt, DateTime EffectiveToDt, ulong UserId, int CustomerId, int Nid, string Remarks)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.None };
            tblFlightCustomerMarkup mdl = new tblFlightCustomerMarkup()
            {
                CustomerId = CustomerId,
                EffectiveFromDt = EffectiveFromDt,
                EffectiveToDt = EffectiveToDt,
                MarkupAmount = MarkupAmount,
                ModifyRemarks = Remarks ?? string.Empty,
                CreatedBy = UserId,
                ModifiedBy = UserId,
                CreatedDt = DateTime.Now,
                ModifiedDt = DateTime.Now,
                IsDeleted = false,
            };
            _travelContext.tblFlightCustomerMarkup.Add(mdl);
            _travelContext.SaveChanges();
            returnData.ReturnId = mdl;
            returnData.MessageType = enmMessageType.Success;
            return returnData;
        }
        public mdlReturnData RemoveCustomerMarkup(int MarkupID, ulong UserId, string Remarks)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.None };
            var TempData = _travelContext.tblFlightCustomerMarkup.Where(p => p.Id == MarkupID).FirstOrDefault();
            if (TempData == null)
            {
                returnData.MessageType = enmMessageType.Error;
                returnData.Message = "Invalid Markup";
                return returnData;
            }
            TempData.ModifiedBy = UserId;
            TempData.ModifiedDt = DateTime.Now;
            TempData.ModifyRemarks = TempData.ModifyRemarks + ", " + (Remarks ?? string.Empty);
            _travelContext.tblFlightCustomerMarkup.Update(TempData);
            _travelContext.SaveChanges();
            returnData.MessageType = enmMessageType.Success;
            return returnData;
        }

        public List<mdlWingMarkup_Air> GetWingMarkup(bool OnlyActive, bool FilterDateCriteria, enmCustomerType CustomerType, int customerId,
            DateTime TravelDt, DateTime BookingDate)
        {
            List<mdlWingMarkup_Air> mdl = new List<mdlWingMarkup_Air>();

            IQueryable<tblFlightMarkupMaster> returnData = (CustomerType == enmCustomerType.InHouse && customerId == 0) ?
                _travelContext.tblFlightMarkupMaster.AsQueryable() :
                _travelContext.tblFlightMarkupMaster.Where(p =>
                p.IsAllCustomerType ||
                ((!p.IsAllCustomerType) && p.tblFlightMarkupCustomerType.Where(q => q.customerType == CustomerType).Count() > 0 && p.IsAllCustomer) ||
                ((!p.IsAllCustomer) && p.tblFlightMarkupCustomerDetails.Where(q => q.CustomerId == customerId).Count() > 0)
                ).AsQueryable();

            if (OnlyActive && !FilterDateCriteria)
            {
                returnData = returnData.Where(p => p.IsDeleted).AsQueryable();
            }
            else
            {
                if (!OnlyActive)
                {
                    returnData = returnData.Where(p => p.BookingFromDt <= BookingDate && p.BookingToDt > BookingDate
                      && p.TravelFromDt <= TravelDt && p.TravelToDt > TravelDt
                    );
                }
                else
                {
                    returnData = returnData.Where(p => !p.IsDeleted && p.BookingFromDt <= BookingDate && p.BookingToDt > BookingDate
                      && p.TravelFromDt <= TravelDt && p.TravelToDt > TravelDt
                    );
                }
            }
            mdl = returnData
                .Select(p => new mdlWingMarkup_Air
                {
                    Id = p.Id,
                    Applicability = p.Applicability,
                    IsAllProvider = p.IsAllProvider,
                    IsAllCustomerType = p.IsAllCustomerType,
                    IsAllCustomer = p.IsAllCustomer,
                    IsAllPessengerType = p.IsAllPessengerType,
                    IsAllFlightClass = p.IsAllFlightClass,
                    IsAllAirline = p.IsAllAirline,
                    IsAllSegment = p.IsAllSegment,
                    IsMLMIncentive = p.IsMLMIncentive,
                    FlightType = p.FlightType,
                    IsPercentage = p.IsPercentage,
                    Gender = p.Gender,
                    PercentageValue = p.PercentageValue,
                    Amount = p.Amount,
                    AmountCaping = p.AmountCaping,
                    TravelFromDt = p.TravelFromDt,
                    TravelToDt = p.TravelToDt,
                    BookingFromDt = p.BookingFromDt,
                    BookingToDt = p.BookingToDt,
                    IsDeleted = p.IsDeleted,
                    ServiceProviders = p.tblFlightMarkupServiceProvider.Select(q => q.ServiceProvider).ToList(),
                    CustomerTypes = p.tblFlightMarkupCustomerType.Select(q => q.customerType).ToList(),
                    PassengerType = p.tblFlightMarkupPassengerType.Select(q => q.PassengerType).ToList(),
                    CabinClass = p.tblFlightMarkupFlightClass.Select(q => q.CabinClass).ToList(),
                    CustomerIds = p.tblFlightMarkupCustomerDetails.
                    Select(q => new { CustomerId = q.CustomerId ?? 0, CustomerCode = q.tblCustomerMaster.OrganisationCode, CustomerName = q.tblCustomerMaster.OrganisationName }).ToList()
                    .Select(q => new Tuple<int, string>(q.CustomerId, q.CustomerCode)).ToList(),
                    Segments = p.tblFlightMarkupSegment.Select(q => new Tuple<string, string>(q.orign, q.destination)).ToList(),
                    Airline = p.tblFlightMarkupAirline.Select(q => new Tuple<int, string>(q.AirlineId ?? 0, q.tblAirline.Code)).ToList()
                }).ToList();
            return mdl;

        }
        public List<mdlWingMarkup_Air> GetWingDiscount(bool OnlyActive, bool FilterDateCriteria, enmCustomerType CustomerType, int customerId,
            DateTime TravelDt, DateTime BookingDate)
        {
            List<mdlWingMarkup_Air> mdl = new List<mdlWingMarkup_Air>();

            IQueryable<tblFlightDiscount> returnData = (CustomerType == enmCustomerType.InHouse && customerId == 0) ?
                _travelContext.tblFlightDiscount.AsQueryable() :
                _travelContext.tblFlightDiscount.Where(p =>
                p.IsAllCustomerType ||
                ((!p.IsAllCustomerType) && p.tblFlightDiscountCustomerType.Where(q => q.customerType == CustomerType).Count() > 0 && p.IsAllCustomer) ||
                ((!p.IsAllCustomer) && p.tblFlightDiscountCustomerDetails.Where(q => q.CustomerId == customerId).Count() > 0)
                ).AsQueryable();

            if (OnlyActive && !FilterDateCriteria)
            {
                returnData = returnData.Where(p => p.IsDeleted).AsQueryable();
            }
            else
            {
                if (!OnlyActive)
                {
                    returnData = returnData.Where(p => p.BookingFromDt <= BookingDate && p.BookingToDt > BookingDate
                      && p.TravelFromDt <= TravelDt && p.TravelToDt > TravelDt
                    );
                }
                else
                {
                    returnData = returnData.Where(p => !p.IsDeleted && p.BookingFromDt <= BookingDate && p.BookingToDt > BookingDate
                      && p.TravelFromDt <= TravelDt && p.TravelToDt > TravelDt
                    );
                }
            }
            mdl = returnData
                .Select(p => new mdlWingMarkup_Air
                {
                    Id = p.Id,
                    Applicability = p.Applicability,
                    IsAllProvider = p.IsAllProvider,
                    IsAllCustomerType = p.IsAllCustomerType,
                    IsAllCustomer = p.IsAllCustomer,
                    IsAllPessengerType = p.IsAllPessengerType,
                    IsAllFlightClass = p.IsAllFlightClass,
                    IsAllAirline = p.IsAllAirline,
                    IsAllSegment = p.IsAllSegment,
                    IsMLMIncentive = p.IsMLMIncentive,
                    FlightType = p.FlightType,
                    IsPercentage = p.IsPercentage,
                    Gender = p.Gender,
                    PercentageValue = p.PercentageValue,
                    Amount = p.Amount,
                    AmountCaping = p.AmountCaping,
                    TravelFromDt = p.TravelFromDt,
                    TravelToDt = p.TravelToDt,
                    BookingFromDt = p.BookingFromDt,
                    BookingToDt = p.BookingToDt,
                    IsDeleted = p.IsDeleted,
                    ServiceProviders = p.tblFlightDiscountServiceProvider.Select(q => q.ServiceProvider).ToList(),
                    CustomerTypes = p.tblFlightDiscountCustomerType.Select(q => q.customerType).ToList(),
                    PassengerType = p.tblFlightDiscountPassengerType.Select(q => q.PassengerType).ToList(),
                    CabinClass = p.tblFlightDiscountFlightClass.Select(q => q.CabinClass).ToList(),
                    CustomerIds = p.tblFlightDiscountCustomerDetails.
                    Select(q => new { CustomerId = q.CustomerId ?? 0, CustomerCode = q.tblCustomerMaster.OrganisationCode, CustomerName = q.tblCustomerMaster.OrganisationName }).ToList()
                    .Select(q => new Tuple<int, string>(q.CustomerId, q.CustomerCode)).ToList(),
                    Segments = p.tblFlightDiscountSegment.Select(q => new Tuple<string, string>(q.orign, q.destination)).ToList(),
                    Airline = p.tblFlightDiscountAirline.Select(q => new Tuple<int, string>(q.AirlineId ?? 0, q.tblAirline.Code)).ToList()
                }).ToList();
            return mdl;

        }
        public List<mdlWingMarkup_Air> GetWingConvenience(bool OnlyActive, bool FilterDateCriteria, enmCustomerType CustomerType, int customerId,
            DateTime TravelDt, DateTime BookingDate)
        {
            List<mdlWingMarkup_Air> mdl = new List<mdlWingMarkup_Air>();

            IQueryable<tblFlightConvenience> returnData = (CustomerType == enmCustomerType.InHouse && customerId == 0) ?
                _travelContext.tblFlightConvenience.AsQueryable() :
                _travelContext.tblFlightConvenience.Where(p =>
                p.IsAllCustomerType ||
                ((!p.IsAllCustomerType) && p.tblFlightConvenienceCustomerType.Where(q => q.customerType == CustomerType).Count() > 0 && p.IsAllCustomer) ||
                ((!p.IsAllCustomer) && p.tblFlightConvenienceCustomerDetails.Where(q => q.CustomerId == customerId).Count() > 0)
                ).AsQueryable();

            if (OnlyActive && !FilterDateCriteria)
            {
                returnData = returnData.Where(p => p.IsDeleted).AsQueryable();
            }
            else
            {
                if (!OnlyActive)
                {
                    returnData = returnData.Where(p => p.BookingFromDt <= BookingDate && p.BookingToDt > BookingDate
                      && p.TravelFromDt <= TravelDt && p.TravelToDt > TravelDt
                    );
                }
                else
                {
                    returnData = returnData.Where(p => !p.IsDeleted && p.BookingFromDt <= BookingDate && p.BookingToDt > BookingDate
                      && p.TravelFromDt <= TravelDt && p.TravelToDt > TravelDt
                    );
                }
            }
            mdl = returnData
                .Select(p => new mdlWingMarkup_Air
                {
                    Id = p.Id,
                    Applicability = p.Applicability,
                    IsAllProvider = p.IsAllProvider,
                    IsAllCustomerType = p.IsAllCustomerType,
                    IsAllCustomer = p.IsAllCustomer,
                    IsAllPessengerType = p.IsAllPessengerType,
                    IsAllFlightClass = p.IsAllFlightClass,
                    IsAllAirline = p.IsAllAirline,
                    IsAllSegment = p.IsAllSegment,
                    IsMLMIncentive = p.IsMLMIncentive,
                    FlightType = p.FlightType,
                    IsPercentage = p.IsPercentage,
                    Gender = p.Gender,
                    PercentageValue = p.PercentageValue,
                    Amount = p.Amount,
                    AmountCaping = p.AmountCaping,
                    TravelFromDt = p.TravelFromDt,
                    TravelToDt = p.TravelToDt,
                    BookingFromDt = p.BookingFromDt,
                    BookingToDt = p.BookingToDt,
                    IsDeleted = p.IsDeleted,
                    ServiceProviders = p.tblFlightConvenienceServiceProvider.Select(q => q.ServiceProvider).ToList(),
                    CustomerTypes = p.tblFlightConvenienceCustomerType.Select(q => q.customerType).ToList(),
                    PassengerType = p.tblFlightConveniencePassengerType.Select(q => q.PassengerType).ToList(),
                    CabinClass = p.tblFlightConvenienceFlightClass.Select(q => q.CabinClass).ToList(),
                    CustomerIds = p.tblFlightConvenienceCustomerDetails.
                    Select(q => new { CustomerId = q.CustomerId ?? 0, CustomerCode = q.tblCustomerMaster.OrganisationCode, CustomerName = q.tblCustomerMaster.OrganisationName }).ToList()
                    .Select(q => new Tuple<int, string>(q.CustomerId, q.CustomerCode)).ToList(),
                    Segments = p.tblFlightConvenienceSegment.Select(q => new Tuple<string, string>(q.orign, q.destination)).ToList(),
                    Airline = p.tblFlightConvenienceAirline.Select(q => new Tuple<int, string>(q.AirlineId ?? 0, q.tblAirline.Code)).ToList()
                }).ToList();
            return mdl;

        }

        public void GetCharges(mdlFlightSearchWraper searchRequest, mdlSearchResult searchResult, List<mdlTravellerinfo> travellerinfos, bool IsOnward, enmCustomerType CustomerType, int CustomerId, ulong Nid, DateTime bookingDate)
        {
            if (IsOnward)
            {
                if (_WingMarkupOnward == null)
                {
                    _WingMarkupOnward = GetWingMarkup(true, true, CustomerType, CustomerId, searchRequest.DepartureDt, bookingDate);
                }
                if (_WingDiscountOnward == null)
                {
                    _WingDiscountOnward = GetWingDiscount(true, true, CustomerType, CustomerId, searchRequest.DepartureDt, bookingDate);
                }
                if (_WingConvenienceOnward == null)
                {
                    _WingConvenienceOnward = GetWingConvenience(true, true, CustomerType, CustomerId, searchRequest.DepartureDt, bookingDate);
                }
                if (_CustomerMarkupOnward == null)
                {
                    _CustomerMarkupOnward = GetCustomerMarkup(false, false, bookingDate, CustomerId, Nid, CustomerType);
                }

                SetWingMarkupDiscountConvenience(_WingMarkupOnward, enmFlighWingCharge.WingMarkup);
                SetWingMarkupDiscountConvenience(_WingDiscountOnward, enmFlighWingCharge.Discount);
                SetWingMarkupDiscountConvenience(_WingConvenienceOnward, enmFlighWingCharge.Convenience);

            }
            if (!IsOnward)
            {
                if (_WingMarkupInward == null)
                {
                    _WingMarkupInward = GetWingMarkup(true, true, CustomerType, CustomerId, searchRequest.ReturnDt.Value, bookingDate);
                }
                if (_WingDiscountInward == null)
                {
                    _WingDiscountInward = GetWingDiscount(true, true, CustomerType, CustomerId, searchRequest.ReturnDt.Value, bookingDate);
                }
                if (_WingConvenienceInward == null)
                {
                    _WingConvenienceInward = GetWingConvenience(true, true, CustomerType, CustomerId, searchRequest.ReturnDt.Value, bookingDate);
                }
                if (_CustomerMarkupInward == null)
                {
                    _CustomerMarkupInward = GetCustomerMarkup(false, false, bookingDate, CustomerId, Nid, CustomerType);
                }
                SetWingMarkupDiscountConvenience(_WingMarkupInward, enmFlighWingCharge.WingMarkup);
                SetWingMarkupDiscountConvenience(_WingDiscountInward, enmFlighWingCharge.Discount);
                SetWingMarkupDiscountConvenience(_WingConvenienceInward, enmFlighWingCharge.Convenience);
            }
            void SetWingMarkupDiscountConvenience(List<mdlWingMarkup_Air> tempData, enmFlighWingCharge cType)
            {
                bool IsDirect = true;
                if (searchResult.Segment.Count() > 0)
                {
                    IsDirect = false;
                }
                List<string> Airline = searchResult.Segment.Select(p => p.Airline.Code.ToUpper()).ToList();

                var FirstSegment = searchResult.Segment.FirstOrDefault();
                var LastSegment = searchResult.Segment.LastOrDefault();
                if (FirstSegment == null)
                {
                    return;
                }

                foreach (var pricelist in searchResult.TotalPriceList)
                {
                    enmServiceProvider serviceProvider = enmServiceProvider.None;
                    Enum.TryParse<enmServiceProvider>(pricelist.ResultIndex.Split("_").FirstOrDefault(), out serviceProvider);
                    if (serviceProvider == enmServiceProvider.None)
                    {
                        continue;
                    }



                    var tempD = tempData.Where(p => (p.IsAllAirline || p.Airline.Where(q => Airline.Contains(q.Item2.ToUpper())).Any())
                       && (p.FlightType == enmFlightType.All || (p.FlightType == enmFlightType.Connected && !IsDirect) || (p.FlightType == enmFlightType.Direct && IsDirect))
                       && (p.IsAllFlightClass || (p.CabinClass.Contains(pricelist.CabinClass)))
                       && (p.IsAllSegment ||
                       (
                         (IsOnward && p.Segments.Any(q => q.Item1.Equals(FirstSegment.Origin.AirportCode, StringComparison.OrdinalIgnoreCase) && q.Item2.Equals(LastSegment.Destination.AirportCode, StringComparison.OrdinalIgnoreCase))) ||
                         (!IsOnward && p.Segments.Any(q => q.Item1.Equals(LastSegment.Destination.AirportCode, StringComparison.OrdinalIgnoreCase) && q.Item1.Equals(FirstSegment.Origin.AirportCode, StringComparison.OrdinalIgnoreCase)))

                       ))
                       && (p.IsAllProvider || p.ServiceProviders.Contains(serviceProvider))
                    ).ToList();

                    if (pricelist.ADULT.FareBreakup == null)
                    {
                        pricelist.ADULT.FareBreakup = new List<mdlWingFaredetails>();
                    }
                    if (pricelist.CHILD.FareBreakup == null)
                    {
                        pricelist.CHILD.FareBreakup = new List<mdlWingFaredetails>();
                    }
                    if (pricelist.INFANT.FareBreakup == null)
                    {
                        pricelist.INFANT.FareBreakup = new List<mdlWingFaredetails>();
                    }

                    pricelist.ADULT.FareBreakup.RemoveAll(p => p.type == cType);
                    pricelist.CHILD.FareBreakup.RemoveAll(p => p.type == cType);
                    pricelist.INFANT.FareBreakup.RemoveAll(p => p.type == cType);
                    pricelist.ConsolidateFareBreakup.RemoveAll(p => p.type == cType);

                    //All markup on Passengertype 
                    pricelist.ADULT.FareBreakup.AddRange(
                    tempD.Where(p => p.Applicability == enmFlightSearvices.OnPassenger && (p.IsAllPessengerType || p.PassengerType.Contains(enmPassengerType.Adult)))
                        .Select(p => new mdlWingFaredetails
                        {
                            ID = p.Id,
                            amount = p.IsPercentage ? pricelist.ADULT.BaseFare * p.PercentageValue / 100.0 > p.AmountCaping ? p.AmountCaping : pricelist.ADULT.BaseFare * p.PercentageValue / 100.0 : p.Amount,
                            OnGender = p.Gender,
                            type = cType
                        }));
                    pricelist.CHILD.FareBreakup.AddRange(
                    tempD.Where(p => p.Applicability == enmFlightSearvices.OnPassenger && (p.IsAllPessengerType || p.PassengerType.Contains(enmPassengerType.Child)))
                        .Select(p => new mdlWingFaredetails
                        {
                            ID = p.Id,
                            amount = p.IsPercentage ? pricelist.CHILD.BaseFare * p.PercentageValue / 100.0 > p.AmountCaping ? p.AmountCaping : pricelist.CHILD.BaseFare * p.PercentageValue / 100.0 : p.Amount,
                            OnGender = p.Gender,
                            type = cType
                        }));

                    pricelist.INFANT.FareBreakup.AddRange(
                    tempD.Where(p => p.Applicability == enmFlightSearvices.OnPassenger && (p.IsAllPessengerType || p.PassengerType.Contains(enmPassengerType.Infant)))
                        .Select(p => new mdlWingFaredetails
                        {
                            ID = p.Id,
                            amount = p.IsPercentage ? pricelist.INFANT.BaseFare * p.PercentageValue / 100.0 > p.AmountCaping ? p.AmountCaping : pricelist.INFANT.BaseFare * p.PercentageValue / 100.0 : p.Amount,
                            OnGender = p.Gender,
                            type = cType
                        }));
                    pricelist.ConsolidateFareBreakup.AddRange(
                    tempD.Where(p => p.Applicability == enmFlightSearvices.OnTicket && (p.IsAllPessengerType || p.PassengerType.Contains(enmPassengerType.Infant)))
                        .Select(p => new mdlWingFaredetails
                        {
                            ID = p.Id,
                            amount = p.IsPercentage ? pricelist.BaseFare * p.PercentageValue / 100.0 > p.AmountCaping ? p.AmountCaping : pricelist.BaseFare * p.PercentageValue / 100.0 : p.Amount,
                            OnGender = p.Gender,
                            type = cType
                        }));

                }

            }
            void SetCustomerMarkup(List<tblFlightCustomerMarkup> tempData, enmFlighWingCharge cType)
            {
                foreach (var pricelist in searchResult.TotalPriceList)
                {
                    pricelist.ConsolidateFareBreakup.RemoveAll(p => p.type == cType);
                    pricelist.ConsolidateFareBreakup.AddRange(
                    tempData
                        .Select(p => new mdlWingFaredetails
                        {
                            ID = p.Id,
                            amount = p.MarkupAmount,
                            OnGender = enmGender.None,
                            type = cType
                        }));
                }
            }

        }

        #endregion

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
                            Results[i][j].TotalPriceList[k].Discount -
                            Results[i][j].TotalPriceList[k].PromoDiscount;

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
                    p.TotalPriceList.ForEach(q => { q.ResultIndex = string.Concat(spv, '_', tempResult.TraceId, '_', q.ResultIndex); });
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
                BookingRes.PurchaseCabinClass = BookingRes.Results[0][0].TotalPriceList[0].CabinClass;
                BookingRes.PurchaseClassOfBooking = BookingRes.Results[0][0].TotalPriceList[0].ClassOfBooking;
                BookingRes.PurchaseIdentifier = BookingRes.Results[0][0].TotalPriceList[0].Identifier;
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
                    BookingRes.BookedCabinClass = AlterBookingRes.Results[0][0].TotalPriceList[0].CabinClass;
                    BookingRes.BookedClassOfBooking = AlterBookingRes.Results[0][0].TotalPriceList[0].ClassOfBooking;
                    BookingRes.BookedIdentifier = AlterBookingRes.Results[0][0].TotalPriceList[0].Identifier;
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
             mdlFlightSearchWraper searchWraper, mdlDeliveryinfo Deliveryinfo,
             string VisitorId, int OrgId, ulong Nid,
             ulong UserId
             )
        {
            DateTime bookingDt = DateTime.Now;




            List<tblFlightBookingSearchDetails> searchDetails = new List<tblFlightBookingSearchDetails>();
            foreach (var result in Results)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    var totalPriceList = result[i].TotalPriceList.FirstOrDefault();
                    tblFlightBookingSearchDetails sd = new tblFlightBookingSearchDetails()
                    {
                        BookingId = Guid.NewGuid().ToString(),
                        VisitorId = VisitorId,
                        SegmentId = i + 1,
                        PurchaseIdentifier = totalPriceList.Identifier,
                        PurchaseCabinClass = totalPriceList.CabinClass,
                        PurchaseClassOfBooking = totalPriceList.ClassOfBooking,
                        BookedIdentifier = totalPriceList.alterIdentifier ?? totalPriceList.Identifier,
                        BookedCabinClass = totalPriceList.alterCabinClass ?? totalPriceList.CabinClass,
                        BookedClassOfBooking = totalPriceList.ClassOfBooking,
                        ProviderBookingId = totalPriceList.ProviderBookingId,
                        ServiceProvider = totalPriceList.ServiceProvider,
                        BookingStatus = enmBookingStatus.Pending,
                        PaymentStatus = enmPaymentStatus.Pending,
                        ConvenienceAmount = totalPriceList.Convenience,
                        CustomerMarkupAmount = totalPriceList.CustomerMarkup,
                        DiscountAmount = totalPriceList.Discount,
                        HaveRefund = false,
                        IncentiveAmount = totalPriceList.MLMMarkup,
                        IncludeBaggageServices = totalPriceList.IncludeBaggageServices,
                        IncludeMealServices = totalPriceList.IncludeMealServices,
                        IncludeSeatServices = totalPriceList.IncludeSeatServices,
                        MarkupAmount = totalPriceList.WingMarkup,
                        NetSaleAmount = totalPriceList.NetFare,
                        PurchaseAmount = totalPriceList.PurchaseAmount,
                        PassengerConvenienceAmount = totalPriceList.PassengerConvenienceAmount,
                        PassengerMarkupAmount = totalPriceList.PassengerMarkupAmount,
                        PassengerDiscountAmount = totalPriceList.PassengerDiscountAmount,
                        PassengerIncentiveAmount = totalPriceList.PassengerIncentiveAmount,
                        IsDeleted = false,
                        SaleAmount = totalPriceList.SaleAmount,
                    };
                }

                
            }

            searchDetails.Add(new tblFlightBookingSearchDetails() { 
                VisitorId= VisitorId,                
            });

            tblFlightBookingMaster mdl = new tblFlightBookingMaster()
            {
                AdultCount = searchWraper?.AdultCount ?? 0,
                ChildCount = searchWraper?.ChildCount ?? 0,
                InfantCount = searchWraper?.InfantCount ?? 0,
                BookingDate = bookingDt,
                CabinClass = searchWraper?.CabinClass ?? enmCabinClass.ECONOMY,                
                From = searchWraper?.From ?? string.Empty,
                To = searchWraper?.To ?? string.Empty,
                JourneyType = searchWraper?.JourneyType ?? enmJourneyType.OneWay,
                DepartureDt = searchWraper?.DepartureDt ?? bookingDt,
                ReturnDt = searchWraper?.ReturnDt,
                OrgId = OrgId,
                Nid = Nid,
                EmailNo = Deliveryinfo.emails.FirstOrDefault(),
                PhoneNo = Deliveryinfo.contacts.FirstOrDefault(),
                PaymentMode = Deliveryinfo.PaymentMode,
                GatewayId = Deliveryinfo.GatewayId,
                PaymentType = Deliveryinfo.PaymentType,
                PaymentTransactionNumber = Deliveryinfo.PaymentTransactionNumber,
                CardNo = Deliveryinfo.CardNo,
                AccountNumber = Deliveryinfo.AccountNumber,
                BankName = Deliveryinfo.BankName,
                IncludeLoaylty = Deliveryinfo.IncludeLoaylty,
                ConsumedLoyaltyPoint = Deliveryinfo.ConsumedLoyaltyPoint,
                ConsumedLoyaltyAmount = Deliveryinfo.ConsumedLoyaltyAmount,
                BookingAmount = Deliveryinfo.BookingAmount,
                GatewayCharge = Deliveryinfo.GatewayCharge,
                NetAmount = Deliveryinfo.NetAmount,
                PaidAmount = Deliveryinfo.PaidAmount,
                WalletAmount = Deliveryinfo.WalletAmount,
                LoyaltyAmount = Deliveryinfo.LoyaltyAmount,
                BookingStatus = Deliveryinfo.BookingStatus,
                PaymentStatus = enmPaymentStatus.Pending,
                HaveRefund = Deliveryinfo.HaveRefund,
                CreatedBy= UserId,
                ModifiedBy=UserId,CreatedDt=bookingDt, ModifiedDt=bookingDt,VisitorId=VisitorId
            };

            


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
                    PurchaseIdentifier = response[SegmentId - 1].PurchaseIdentifier,
                    PurchaseCabinClass = response[SegmentId - 1].PurchaseCabinClass,
                    PurchaseClassOfBooking = response[SegmentId - 1].PurchaseClassOfBooking,
                    BookedIdentifier = response[SegmentId - 1].BookedIdentifier,
                    BookedCabinClass = response[SegmentId - 1].BookedCabinClass,
                    BookedClassOfBooking = response[SegmentId - 1].BookedClassOfBooking,
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
                
                _travelContext.tblFlightFare.Add(flightFare);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        //Task<mdlFareQuotResponse> FareQuoteAsync(mdlFareQuotRequest request);
        //Task<mdlFareRuleResponse> FareRuleAsync(mdlFareRuleRequest request);
        //Task<mdlBookingResponse> BookingAsync(mdlBookingRequest request);
        //Task<mdlFlightCancellationChargeResponse> CancelationChargeAsync(mdlCancellationRequest request);
        //Task<mdlFlightCancellationResponse> CancellationAsync(mdlCancellationRequest request);
        //Task<mdlCancelationDetails> CancelationDetailsAsync(string request);
    }


    public interface IsrvAir
    {
        IEnumerable<tblAirport> GetAirport(bool OnlyActive = true, bool IsDomestic = false);
    }

    public class srvAir : IsrvAir
    {
        private readonly TravelContext _travelContext;
        private readonly ITripJack _tripJack;
          
        public srvAir(TravelContext travelContext, ITripJack tripJack)
        {
            _travelContext = travelContext;
            _tripJack = tripJack;
        }

        #region *********************** Setting **************************

        public mdlReturnData SetServiceProvider(DateTime EffectiveFromDate,enmServiceProvider ServiceProvider, bool IsEnable, ulong UserId,string Remarks)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.None };
            var TempData= _travelContext.tblFlightSerivceProvider.Where(p => p.ServiceProvider == ServiceProvider && p.EffectiveFromDate == EffectiveFromDate && !p.IsDeleted).FirstOrDefault();
            if (TempData != null)
            {
                TempData.IsDeleted = true;
                TempData.ModifiedDt = DateTime.Now;
                TempData.ModifiedBy = UserId;
                TempData.ModifyRemarks = TempData.ModifyRemarks+ Remarks;
                _travelContext.tblFlightSerivceProvider.Update(TempData);
            }
            tblFlightSerivceProvider mdl = new tblFlightSerivceProvider()
            {
                CreatedBy=UserId,
                ModifiedBy= UserId,
                CreatedDt=DateTime.Now,
                ModifiedDt= DateTime.Now,
                IsDeleted =false,
                ModifyRemarks=Remarks??String.Empty,
                ServiceProvider= ServiceProvider,
                EffectiveFromDate=EffectiveFromDate,
                IsEnabled=IsEnable,                
            };
            _travelContext.tblFlightSerivceProvider.Add(mdl);
            _travelContext.SaveChanges();
            returnData.MessageType = enmMessageType.Success;
            returnData.ReturnId = mdl;
            return returnData;
        }

        public List<tblFlightSerivceProvider> GetServiceProvider(DateTime ProcessDate,bool IsOnlyActive)
        {
            List<tblFlightSerivceProvider> Providers = new List<tblFlightSerivceProvider>();
            foreach (enmServiceProvider provider in Enum.GetValues(typeof( enmServiceProvider)))
            {
                if (provider == enmServiceProvider.None)
                {
                    continue;
                }
                var sp = _travelContext.tblFlightSerivceProvider.Where(p => !p.IsDeleted && p.ServiceProvider == provider && p.EffectiveFromDate <=  ProcessDate).OrderByDescending(p => p.EffectiveFromDate).Take(1).FirstOrDefault();
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
                return Providers.Where(p=>p.IsEnabled).ToList();
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


        public mdlReturnData SetInstantBookingSeting(DateTime EffectiveFromDate, enmCustomerType CustomerType, bool InstantDomestic,bool InstantNonDomestic, ulong UserId, string Remarks)
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
                InstantDomestic=InstantDomestic,
                InstantNonDomestic=InstantNonDomestic,
            };
            _travelContext.tblFlightInstantBooking.Add(mdl);
            _travelContext.SaveChanges();
            returnData.MessageType = enmMessageType.Success;
            returnData.ReturnId = mdl;
            return returnData;
        }

        public List<tblFlightInstantBooking> GetInstantBookingSeting(DateTime ProcessDate)
        {
            List<tblFlightInstantBooking> returnData = new List<tblFlightInstantBooking>();
            foreach (enmCustomerType ctype in Enum.GetValues(typeof(enmCustomerType)))
            {
                var sp = _travelContext.tblFlightInstantBooking.Where(p => !p.IsDeleted && p.CustomerType == ctype && p.EffectiveFromDate <= ProcessDate).OrderByDescending(p => p.EffectiveFromDate).Take(1).FirstOrDefault();
                if (sp == null)
                {
                    var tempData = SetInstantBookingSeting(ProcessDate, ctype, true, false,1, string.Empty);
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
            
            return returnData;
        }

        #endregion

        #region ****************** Get All Markup/Discount/convenience **********************
        public List<tblFlightCustomerMarkup> GetCustomerMarkup(bool AllMarkup, bool AllActiveMarkup, DateTime ProcessingDate,int CustomerId, ulong Nid, enmCustomerType CustomerType )
        {
            
            IQueryable<tblFlightCustomerMarkup> returnData = CustomerType== enmCustomerType.MLM?
                _travelContext.tblFlightCustomerMarkup.Where(p=>p.Nid==Nid).AsQueryable():
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
                returnData = returnData.Where(p => !p.IsDeleted && p.EffectiveFromDt<= ProcessingDate && p.EffectiveToDt>ProcessingDate);                
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
        public mdlReturnData RemoveCustomerMarkup(int MarkupID, ulong UserId,string Remarks)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.None };
            var TempData=_travelContext.tblFlightCustomerMarkup.Where(p => p.Id == MarkupID).FirstOrDefault();
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

        public List<mdlWingMarkup_Air> GetWingMarkup(bool AllMarkup,bool OnlyActive,enmCustomerType CustomerType, int customerId,
            DateTime TravelDt,DateTime BookingDate)
        {
            List<mdlWingMarkup_Air> mdl = new List<mdlWingMarkup_Air>();

            IQueryable<tblFlightMarkupMaster> returnData = (CustomerType == enmCustomerType.InHouse && customerId == 0) ?
                _travelContext.tblFlightMarkupMaster.AsQueryable() :
                _travelContext.tblFlightMarkupMaster.Where(p=>
                p.IsAllCustomerType || 
                ((!p.IsAllCustomerType) && p.tblFlightMarkupCustomerType.Where(q=>q.customerType==CustomerType).Count()>0  && p.IsAllCustomer)||
                (( !p.IsAllCustomer) && p.tblFlightMarkupCustomerDetails.Where(q=>q.CustomerId==customerId).Count()>0)
                ).AsQueryable();

            if (!AllMarkup && OnlyActive)
            {
                returnData = returnData.Where(p => p.IsDeleted).AsQueryable();
            }
            else
            {
                if (!OnlyActive)
                {
                    returnData=returnData.Where(p => p.BookingFromDt <= BookingDate && p.BookingToDt > BookingDate
                    && p.TravelFromDt <= TravelDt && p.TravelToDt > TravelDt
                    );
                }
                else
                {
                    returnData = returnData.Where(p =>!p.IsDeleted && p.BookingFromDt <= BookingDate && p.BookingToDt > BookingDate
                      && p.TravelFromDt <= TravelDt && p.TravelToDt > TravelDt
                    );
                }
            }
            mdl = returnData.Include(p=>p.tblFlightMarkupAirline)                
                .Select(p=>new mdlWingMarkup_Air{
                    Applicability=p.Applicability,
                    IsAllProvider=p.IsAllProvider,
                    IsAllCustomerType=p.IsAllCustomerType,
                    IsAllCustomer=p.IsAllCustomer,
                    IsAllPessengerType=p.IsAllPessengerType,
                    IsAllFlightClass=p.IsAllFlightClass,
                    IsAllAirline=p.IsAllAirline,
                    IsAllSegment=p.IsAllSegment,
                    Gender=p.Gender,
                    Amount=p.Amount,
                    DayCount=p.DayCount,
                    TravelFromDt = p.TravelFromDt,
                    TravelToDt=p.TravelToDt,
                    BookingFromDt= p.BookingFromDt,
                    BookingToDt=p.BookingToDt,
                    IsDeleted=p.IsDeleted,
                    ServiceProviders=p.tblFlightMarkupServiceProvider.Select(q=>q.ServiceProvider).ToList(),
                    CustomerTypes=p.tblFlightMarkupCustomerType.Select(q => q.customerType).ToList(),
                    PassengerType= p.tblFlightMarkupPassengerType.Select(q => q.PassengerType).ToList(),
                    CustomerIds = p.tblFlightMarkupCustomerDetails.
                    Select(q=>new {CustomerId= q.CustomerId??0,CustomerCode= q.tblCustomerMaster.OrganisationCode, CustomerName= q.tblCustomerMaster.OrganisationName }).ToList()
                    .Select(q =>new Tuple<int,string>(q.CustomerId, q.CustomerCode)).ToList(),
                    Segments = p.tblFlightMarkupSegment.Select(q => new Tuple<string, string> ( q.orign,q.destination )).ToList(),
                    Airline= p.tblFlightMarkupAirline.Select(q=> new Tuple<int,string>(q.AirlineId??0, q.tblAirline.Code)).ToList()
                }).ToList();
            return mdl;

        }
        public List<mdlWingMarkup_Air> GetWingDiscount(bool AllMarkup, bool OnlyActive, enmCustomerType CustomerType, int customerId,
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

            if (!AllMarkup && OnlyActive)
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
            mdl = returnData.Include(p => p.tblFlightMarkupAirline)
                .Select(p => new mdlWingMarkup_Air
                {
                    Applicability = p.Applicability,
                    IsAllProvider = p.IsAllProvider,
                    IsAllCustomerType = p.IsAllCustomerType,
                    IsAllCustomer = p.IsAllCustomer,
                    IsAllPessengerType = p.IsAllPessengerType,
                    IsAllFlightClass = p.IsAllFlightClass,
                    IsAllAirline = p.IsAllAirline,
                    IsAllSegment = p.IsAllSegment,
                    Gender = p.Gender,
                    Amount = p.Amount,
                    DayCount = p.DayCount,
                    TravelFromDt = p.TravelFromDt,
                    TravelToDt = p.TravelToDt,
                    BookingFromDt = p.BookingFromDt,
                    BookingToDt = p.BookingToDt,
                    IsDeleted = p.IsDeleted,
                    ServiceProviders = p.tblFlightMarkupServiceProvider.Select(q => q.ServiceProvider).ToList(),
                    CustomerTypes = p.tblFlightMarkupCustomerType.Select(q => q.customerType).ToList(),
                    PassengerType = p.tblFlightMarkupPassengerType.Select(q => q.PassengerType).ToList(),
                    CustomerIds = p.tblFlightMarkupCustomerDetails.
                    Select(q => new { CustomerId = q.CustomerId ?? 0, CustomerCode = q.tblCustomerMaster.OrganisationCode, CustomerName = q.tblCustomerMaster.OrganisationName }).ToList()
                    .Select(q => new Tuple<int, string>(q.CustomerId, q.CustomerCode)).ToList(),
                    Segments = p.tblFlightMarkupSegment.Select(q => new Tuple<string, string>(q.orign, q.destination)).ToList(),
                    Airline = p.tblFlightMarkupAirline.Select(q => new Tuple<int, string>(q.AirlineId ?? 0, q.tblAirline.Code)).ToList()
                }).ToList();
            return mdl;

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

        public void FlightSearch(mdlFlightSearchWraper mdl, enmCustomerType customerType)
        {
            DateTime CurrentDate = DateTime.Now;
            var ServiceProviders=GetServiceProvider(CurrentDate,true);            
        }


    }

    


    
}

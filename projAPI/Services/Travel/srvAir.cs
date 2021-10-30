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
        private List<mdlWingMarkup_Air> _WingMarkupOnward, _WingDiscountOnward, _WingConvenienceOnward,
            _WingMarkupInward, _WingDiscountInward, _WingConvenienceInward;
        private List<tblFlightCustomerMarkup> _CustomerMarkupOnward, _CustomerMarkupInward;

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

        public List<tblFlightInstantBooking> GetInstantBookingSeting(bool FilterDate ,DateTime ProcessDate)
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
                    returnData.AddRange(_travelContext.tblFlightInstantBooking.Where(p => !p.IsDeleted && p.CustomerType == ctype ).OrderByDescending(p => p.EffectiveFromDate).Take(20));
                }
                
            }
            
            return returnData;
        }

        public mdlReturnData SetFlightBookingAlterMaster(mdlFlightAlter mdl ,ulong UserId,string Remarks)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.None };
            //Check is data already Exist if yes then Remove Existing
            var tempData=_travelContext.tblFlightBookingAlterMaster.Where(p => p.CabinClass == mdl.CabinClass && p.ClassOfBooking== mdl.ClassOfBooking && p.Identifier==mdl.Identifier && !p.IsDeleted).ToList();
            if (tempData!= null && tempData.Count > 0)
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
                ModifyRemarks = Remarks ?? String.Empty,
                CabinClass = mdl.CabinClass ,
                ClassOfBooking =mdl.ClassOfBooking ,Identifier = mdl.Identifier,
                tblFlightBookingAlterDetails= mdl.AlterDetails.Select(q=>new tblFlightBookingAlterDetails {CabinClass=q.Item1, Identifier=q.Item2, ClassOfBooking=q.Item3,IsDeleted=false }).ToList()
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

        public List<mdlWingMarkup_Air> GetWingMarkup(bool OnlyActive,bool FilterDateCriteria ,enmCustomerType CustomerType, int customerId,
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

            if (OnlyActive && !FilterDateCriteria)
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
            mdl = returnData
                .Select(p=>new mdlWingMarkup_Air{
                    Id=p.Id,
                    Applicability=p.Applicability,
                    IsAllProvider=p.IsAllProvider,
                    IsAllCustomerType=p.IsAllCustomerType,
                    IsAllCustomer=p.IsAllCustomer,
                    IsAllPessengerType=p.IsAllPessengerType,
                    IsAllFlightClass=p.IsAllFlightClass,
                    IsAllAirline=p.IsAllAirline,
                    IsAllSegment=p.IsAllSegment,
                    IsMLMIncentive=p.IsMLMIncentive,
                    FlightType=p.FlightType,
                    IsPercentage=p.IsPercentage,
                    Gender =p.Gender,
                    PercentageValue=p.PercentageValue,
                    Amount=p.Amount,
                    AmountCaping=p.AmountCaping,
                    TravelFromDt = p.TravelFromDt,
                    TravelToDt=p.TravelToDt,
                    BookingFromDt= p.BookingFromDt,
                    BookingToDt=p.BookingToDt,
                    IsDeleted=p.IsDeleted,
                    ServiceProviders=p.tblFlightMarkupServiceProvider.Select(q=>q.ServiceProvider).ToList(),
                    CustomerTypes=p.tblFlightMarkupCustomerType.Select(q => q.customerType).ToList(),
                    PassengerType= p.tblFlightMarkupPassengerType.Select(q => q.PassengerType).ToList(),
                    CabinClass= p.tblFlightMarkupFlightClass.Select(q=>q.CabinClass).ToList(),
                    CustomerIds = p.tblFlightMarkupCustomerDetails.
                    Select(q=>new {CustomerId= q.CustomerId??0,CustomerCode= q.tblCustomerMaster.OrganisationCode, CustomerName= q.tblCustomerMaster.OrganisationName }).ToList()
                    .Select(q =>new Tuple<int,string>(q.CustomerId, q.CustomerCode)).ToList(),
                    Segments = p.tblFlightMarkupSegment.Select(q => new Tuple<string, string> ( q.orign,q.destination )).ToList(),
                    Airline= p.tblFlightMarkupAirline.Select(q=> new Tuple<int,string>(q.AirlineId??0, q.tblAirline.Code)).ToList()
                }).ToList();
            return mdl;

        }
        public List<mdlWingMarkup_Air> GetWingDiscount( bool OnlyActive, bool FilterDateCriteria, enmCustomerType CustomerType, int customerId,
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
        public List<mdlWingMarkup_Air> GetWingConvenience( bool OnlyActive, bool FilterDateCriteria, enmCustomerType CustomerType, int customerId,
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

        public void GetCharges(mdlFlightSearchWraper searchRequest, mdlSearchResult searchResult, List<mdlTravellerinfo> travellerinfos,bool IsOnward, enmCustomerType CustomerType,int CustomerId,ulong Nid, DateTime bookingDate)
        {
            if (IsOnward)
            {
                if (_WingMarkupOnward == null)
                {
                    _WingMarkupOnward = GetWingMarkup( true,true, CustomerType, CustomerId,searchRequest.DepartureDt, bookingDate);
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

                SetWingMarkupDiscountConvenience( _WingMarkupOnward, enmFlighWingCharge.WingMarkup);
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
                SetWingMarkupDiscountConvenience( _WingMarkupInward, enmFlighWingCharge.WingMarkup);
                SetWingMarkupDiscountConvenience( _WingDiscountInward, enmFlighWingCharge.Discount);
                SetWingMarkupDiscountConvenience(_WingConvenienceInward, enmFlighWingCharge.Convenience);
            }
            void SetWingMarkupDiscountConvenience(List<mdlWingMarkup_Air> tempData, enmFlighWingCharge cType)
            {
                bool IsDirect = true;
                if (searchResult.Segment.Count() > 0)
                {
                    IsDirect = false;
                }
                List<string> Airline=searchResult.Segment.Select(p => p.Airline.Code.ToUpper()).ToList();
                
                var FirstSegment = searchResult.Segment.FirstOrDefault();
                var LastSegment = searchResult.Segment.LastOrDefault();
                if (FirstSegment == null)
                {
                    return;
                }
                
                foreach (var pricelist in searchResult.TotalPriceList)
                {
                    enmServiceProvider serviceProvider = enmServiceProvider.None;
                    Enum.TryParse<enmServiceProvider>(pricelist.ResultIndex.Split("_").FirstOrDefault(),out serviceProvider);
                    if (serviceProvider == enmServiceProvider.None)
                    {
                        continue;
                    }



                    var tempD= tempData.Where(p => (p.IsAllAirline || p.Airline.Where(q => Airline.Contains(q.Item2.ToUpper())).Any())
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
                    tempD.Where(p =>p.Applicability==enmFlightSearvices.OnPassenger && (p.IsAllPessengerType || p.PassengerType.Contains(enmPassengerType.Adult)) )
                        .Select(p => new mdlWingFaredetails {ID=p.Id, 
                            amount=p.IsPercentage? pricelist.ADULT.BaseFare * p.PercentageValue/100.0>p.AmountCaping? p.AmountCaping: pricelist.ADULT.BaseFare * p.PercentageValue / 100.0:p.Amount,
                           OnGender=p.Gender,
                           type= cType
                        }));
                    pricelist.CHILD.FareBreakup.AddRange(
                    tempD.Where(p => p.Applicability == enmFlightSearvices.OnPassenger && (p.IsAllPessengerType || p.PassengerType.Contains(enmPassengerType.Child)) )
                        .Select(p => new mdlWingFaredetails
                        {
                            ID = p.Id,
                            amount = p.IsPercentage ? pricelist.CHILD.BaseFare * p.PercentageValue / 100.0 > p.AmountCaping ? p.AmountCaping : pricelist.CHILD.BaseFare * p.PercentageValue / 100.0 : p.Amount,
                            OnGender = p.Gender,
                            type = cType
                        }));

                    pricelist.INFANT.FareBreakup.AddRange(
                    tempD.Where(p => p.Applicability == enmFlightSearvices.OnPassenger && (p.IsAllPessengerType || p.PassengerType.Contains(enmPassengerType.Infant)) )
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
            void SetCustomerMarkup(List<tblFlightCustomerMarkup>tempData ,enmFlighWingCharge cType)
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
                            OnGender =enmGender.None,
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
            

            var tempData=GetFlightBookingAlterMaster();
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
                foreach (var innerSr in searchResults)
                {
                    if (innerSr.Segment.Count != Segment.Count)
                    {
                        continue;
                    }
                    for (int i = 0; i < innerSr.Segment.Count; i++)
                    {
                        if (string.Equals(innerSr.Segment[i].Airline.Code, Segment[i].Airline.Code, StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(innerSr.Segment[i].Airline.Name, Segment[i].Airline.Name, StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(innerSr.Segment[i].Airline.FlightNumber, Segment[i].Airline.FlightNumber, StringComparison.OrdinalIgnoreCase)
                            )
                        {
                            for (int j = 0; j < innerSr.TotalPriceList.Count; j++)
                            {
                                if (innerSr.TotalPriceList[j].NetFare < MinPrice && !string.Equals( ResultIndex , innerSr.TotalPriceList[j].ResultIndex,StringComparison.OrdinalIgnoreCase))
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
                        else
                        {
                            i = innerSr.Segment.Count;
                        }
                    }
                }
                
            }

        }

        public async Task FlightSearchAsync(mdlFlightSearchWraper mdl, enmCustomerType customerType, int CustomerId,ulong Nid)
        {
            DateTime CurrentDate = DateTime.Now;
            var ServiceProviders=GetServiceProvider(CurrentDate,true).Select(p=>p.ServiceProvider);

            mdlSearchResponse FinalResult =null;            
            foreach (var sp in ServiceProviders)
            {
                IWingFlight tempObj =GetFlightObject(sp);
                if (tempObj == null)
                {
                    continue;
                }

                List<mdlSegmentRequest> onwardSegments = new List<mdlSegmentRequest>();
                onwardSegments.Add(new mdlSegmentRequest() 
                {Destination= mdl.To, Origin= mdl.From, FlightCabinClass= mdl.CabinClass, TravelDt= mdl.DepartureDt });
                mdlSearchRequest request = new mdlSearchRequest() { 
                    AdultCount = mdl.AdultCount,
                    ChildCount = mdl.ChildCount,
                    InfantCount= mdl.InfantCount,
                    JourneyType= enmJourneyType.OneWay,
                    DirectFlight=mdl.DirectFlight,
                    Segments= onwardSegments,
                };
                AppendProvider(await tempObj.SearchAsync(request), true,sp);
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
                    AppendProvider(await tempObj.SearchAsync(inwardrequest), false,sp);                    
                }
            }
            ClearAllCharge();
            void AppendProvider(mdlSearchResponse tempResult,bool IsOnWard, enmServiceProvider spv)
            {
                tempResult.Results.FirstOrDefault().ForEach(p => {
                    p.TotalPriceList.ForEach(q => { q.ResultIndex = string.Concat(spv,'_', tempResult.TraceId,'_', q.ResultIndex); });
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
                        FinalResult.Results[1].AddRange(tempResult.Results.FirstOrDefault());
                    }
                }
            }
            if (GetInstantBookingSeting(true, CurrentDate).Where(p => p.CustomerType == customerType).Any())
            { 
            }

            //GetCharges();
        }

        


        



    }

    


    
}

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
    public partial class srvAir : IsrvAir
    {
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
                ?.Select(p => new mdlWingMarkup_Air
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
                    Select(q => Tuple.Create(q.CustomerId, q.tblCustomerMaster.OrganisationCode)).ToList(),
                    Segments = p.tblFlightMarkupSegment.Select(q => new Tuple<string, string>(q.orign, q.destination)).ToList(),
                    Airline = p.tblFlightMarkupAirline.Select(q => new Tuple<int, string>(q.AirlineId ?? 0, q.tblAirline.Code)).ToList()
                })?.ToList() ?? new List<mdlWingMarkup_Air>();
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
                ?.Select(p => new mdlWingMarkup_Air
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
                    Select(q => Tuple.Create(q.CustomerId, q.tblCustomerMaster.OrganisationCode)).ToList(),
                    Segments = p.tblFlightDiscountSegment.Select(q => new Tuple<string, string>(q.orign, q.destination)).ToList(),
                    Airline = p.tblFlightDiscountAirline.Select(q => new Tuple<int, string>(q.AirlineId ?? 0, q.tblAirline.Code)).ToList()
                })?.ToList() ?? new List<mdlWingMarkup_Air>();
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
                ?.Select(p => new mdlWingMarkup_Air
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
                    Select(q => Tuple.Create(q.CustomerId, q.tblCustomerMaster.OrganisationCode)).ToList(),
                    Segments = p.tblFlightConvenienceSegment.Select(q => new Tuple<string, string>(q.orign, q.destination)).ToList(),
                    Airline = p.tblFlightConvenienceAirline.Select(q => new Tuple<int, string>(q.AirlineId ?? 0, q.tblAirline.Code)).ToList()
                })?.ToList() ?? new List<mdlWingMarkup_Air>();
            return mdl;

        }


        List<Tuple<int, double>> GetMarkup(int customerId, enmCustomerType CustomerType, DateTime TravelDt, DateTime bookingDate,
                bool IsMLM, enmFlightSearvices FlightSearvices, bool IsDirectFlight,
                List<string> Airline, string FromAirport, string ToAirport, enmServiceProvider ServiceProvider,
                double BasePrice, enmPassengerType PassengerType, enmTitle Title
                )
        {
            List<Tuple<int, double>> mdl = null;
            var MasterQuery = _travelContext.tblFlightMarkupMaster.Where(p => !p.IsDeleted && p.IsMLMIncentive == IsMLM &&
              (p.FlightType == enmFlightType.All || (p.FlightType == enmFlightType.Connected && !IsDirectFlight) || (p.FlightType == enmFlightType.Direct && IsDirectFlight)) &&
              p.BookingFromDt <= bookingDate && p.BookingToDt > bookingDate
              && p.TravelFromDt <= TravelDt && p.TravelToDt > TravelDt
              && (p.IsAllCustomerType ||
                  ((!p.IsAllCustomerType) && p.tblFlightMarkupCustomerType.Where(q => q.customerType == CustomerType).Count() > 0 && p.IsAllCustomer) ||
                  ((!p.IsAllCustomer) && p.tblFlightMarkupCustomerDetails.Where(q => q.CustomerId == customerId).Count() > 0))
              && (p.IsAllAirline || p.tblFlightMarkupAirline.Where(q => Airline.Contains(q.tblAirline.Name)).Count() > 0)
              && (p.IsAllProvider || p.tblFlightMarkupServiceProvider.Where(r => r.ServiceProvider == ServiceProvider).Count() > 0)
              && (p.IsAllSegment || (p.tblFlightMarkupSegment.Where(r => r.orign == FromAirport && r.destination == ToAirport).Count() > 0))
            ).Include(p => p.tblFlightMarkupPassengerType).AsQueryable();

            if (FlightSearvices == enmFlightSearvices.OnPassenger)
            {
                MasterQuery = MasterQuery.Where(p =>
                    p.Applicability == enmFlightSearvices.OnPassenger &&
                    (p.IsAllPessengerType || p.tblFlightMarkupPassengerType.Where(q => q.PassengerType == PassengerType).Count() > 0)
                    && (p.Gender == enmGender.ALL ||
                            (p.Gender == enmGender.Male && (Title == enmTitle.MR || Title == enmTitle.MASTER)) ||
                            (p.Gender == enmGender.Female && (Title == enmTitle.MISS || Title == enmTitle.MRS))
                        )
                );
            }
            else
            {
                MasterQuery = MasterQuery.Where(p => p.Applicability == FlightSearvices);
            }
            mdl = MasterQuery?.Select(q => new
            {
                Id = q.Id,
                Amount = q.IsPercentage ?
                (BasePrice * q.PercentageValue / 100.0) > q.AmountCaping ? q.AmountCaping : (BasePrice * q.PercentageValue / 100.0) : q.Amount
            })?.Select(r => new Tuple<int, double>(r.Id, r.Amount))?.ToList();
            return mdl ?? new List<Tuple<int, double>>();
        }

        List<Tuple<int, double>> GetConvenience(int customerId, enmCustomerType CustomerType, DateTime TravelDt, DateTime bookingDate,
            enmFlightSearvices FlightSearvices, bool IsDirectFlight,
            List<string> Airline, string FromAirport, string ToAirport, enmServiceProvider ServiceProvider,
            double BasePrice, enmPassengerType PassengerType, enmTitle Title
            )
        {
            List<Tuple<int, double>> mdl = null;
            var MasterQuery = _travelContext.tblFlightConvenience.Where(p => !p.IsDeleted &&
              (p.FlightType == enmFlightType.All || (p.FlightType == enmFlightType.Connected && !IsDirectFlight) || (p.FlightType == enmFlightType.Direct && IsDirectFlight)) &&
              p.BookingFromDt <= bookingDate && p.BookingToDt > bookingDate
              && p.TravelFromDt <= TravelDt && p.TravelToDt > TravelDt
              && (p.IsAllCustomerType ||
                  ((!p.IsAllCustomerType) && p.tblFlightConvenienceCustomerType.Where(q => q.customerType == CustomerType).Count() > 0 && p.IsAllCustomer) ||
                  ((!p.IsAllCustomer) && p.tblFlightConvenienceCustomerDetails.Where(q => q.CustomerId == customerId).Count() > 0))
              && (p.IsAllAirline || p.tblFlightConvenienceAirline.Where(q => Airline.Contains(q.tblAirline.Name)).Count() > 0)
              && (p.IsAllProvider || p.tblFlightConvenienceServiceProvider.Where(r => r.ServiceProvider == ServiceProvider).Count() > 0)
              && (p.IsAllSegment || (p.tblFlightConvenienceSegment.Where(r => r.orign == FromAirport && r.destination == ToAirport).Count() > 0))
            ).Include(p => p.tblFlightConveniencePassengerType).AsQueryable();

            if (FlightSearvices == enmFlightSearvices.OnPassenger)
            {
                MasterQuery = MasterQuery.Where(p =>
                    p.Applicability == enmFlightSearvices.OnPassenger &&
                    (p.IsAllPessengerType || p.tblFlightConveniencePassengerType.Where(q => q.PassengerType == PassengerType).Count() > 0)
                    && (p.Gender == enmGender.ALL ||
                            (p.Gender == enmGender.Male && (Title == enmTitle.MR || Title == enmTitle.MASTER)) ||
                            (p.Gender == enmGender.Female && (Title == enmTitle.MISS || Title == enmTitle.MRS))
                        )
                );
            }
            else
            {
                MasterQuery = MasterQuery.Where(p => p.Applicability == FlightSearvices);
            }
            mdl = MasterQuery?.Select(q => new
            {
                Id = q.Id,
                Amount = q.IsPercentage ?
                (BasePrice * q.PercentageValue / 100.0) > q.AmountCaping ? q.AmountCaping : (BasePrice * q.PercentageValue / 100.0) : q.Amount
            })?.Select(r => new Tuple<int, double>(r.Id, r.Amount))?.ToList();
            return mdl ?? new List<Tuple<int, double>>();
        }
        List<Tuple<int, double>> GetDiscount(int customerId, enmCustomerType CustomerType, DateTime TravelDt, DateTime bookingDate,
             enmFlightSearvices FlightSearvices, bool IsDirectFlight,
            List<string> Airline, string FromAirport, string ToAirport, enmServiceProvider ServiceProvider,
            double BasePrice, enmPassengerType PassengerType, enmTitle Title
            )
        {
            List<Tuple<int, double>> mdl = null;
            var MasterQuery = _travelContext.tblFlightDiscount.Where(p => !p.IsDeleted && (p.FlightType == enmFlightType.All || (p.FlightType == enmFlightType.Connected && !IsDirectFlight) || (p.FlightType == enmFlightType.Direct && IsDirectFlight)) &&
              p.BookingFromDt <= bookingDate && p.BookingToDt > bookingDate
              && p.TravelFromDt <= TravelDt && p.TravelToDt > TravelDt
              && (p.IsAllCustomerType ||
                  ((!p.IsAllCustomerType) && p.tblFlightDiscountCustomerType.Where(q => q.customerType == CustomerType).Count() > 0 && p.IsAllCustomer) ||
                  ((!p.IsAllCustomer) && p.tblFlightDiscountCustomerDetails.Where(q => q.CustomerId == customerId).Count() > 0))
              && (p.IsAllAirline || p.tblFlightDiscountAirline.Where(q => Airline.Contains(q.tblAirline.Name)).Count() > 0)
              && (p.IsAllProvider || p.tblFlightDiscountServiceProvider.Where(r => r.ServiceProvider == ServiceProvider).Count() > 0)
              && (p.IsAllSegment || (p.tblFlightDiscountSegment.Where(r => r.orign == FromAirport && r.destination == ToAirport).Count() > 0))
            ).Include(p => p.tblFlightDiscountPassengerType).AsQueryable();

            if (FlightSearvices == enmFlightSearvices.OnPassenger)
            {
                MasterQuery = MasterQuery.Where(p =>
                    p.Applicability == enmFlightSearvices.OnPassenger &&
                    (p.IsAllPessengerType || p.tblFlightDiscountPassengerType.Where(q => q.PassengerType == PassengerType).Count() > 0)
                    && (p.Gender == enmGender.ALL ||
                            (p.Gender == enmGender.Male && (Title == enmTitle.MR || Title == enmTitle.MASTER)) ||
                            (p.Gender == enmGender.Female && (Title == enmTitle.MISS || Title == enmTitle.MRS))
                        )
                );
            }
            else
            {
                MasterQuery = MasterQuery.Where(p => p.Applicability == FlightSearvices);
            }
            mdl = MasterQuery?.Select(q => new
            {
                Id = q.Id,
                Amount = q.IsPercentage ?
                (BasePrice * q.PercentageValue / 100.0) > q.AmountCaping ? q.AmountCaping : (BasePrice * q.PercentageValue / 100.0) : q.Amount
            })?.Select(r => new Tuple<int, double>(r.Id, r.Amount))?.ToList();
            return mdl ?? new List<Tuple<int, double>>();
        }


        public void GetCharges(mdlFlightSearchWraper searchRequest, mdlSearchResult searchResult, bool IsOnward, enmCustomerType CustomerType, int CustomerId, ulong Nid, DateTime bookingDate)
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
    }
}

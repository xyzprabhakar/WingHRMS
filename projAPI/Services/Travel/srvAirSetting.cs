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
        //
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
    }
}

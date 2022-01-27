using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projAPI.Model;
using projAPI.Services.Travel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projContext;
using projAPI.Model.Travel;
using projAPI.Services;
using projAPI.Classes;
using projContext.DB.CRM.Travel;
using Microsoft.AspNetCore.Authorization;

namespace projAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirController : ControllerBase
    {
        private readonly IsrvAir _IsrvAir;
        private readonly IsrvCurrentUser _IsrvCurrentUser;
        private readonly Context _context;
        private readonly TravelContext _travelContext;
        public AirController(IsrvAir isrvAir, IsrvCurrentUser isrvCurrentUser, Context context, TravelContext travelContext)
        {
            _IsrvAir = isrvAir;
            _IsrvCurrentUser = isrvCurrentUser;
            _context = context;
            _travelContext = travelContext;
        }

        #region ************ Master **************************

        [Route("Master/CheckAirFareCode")]
        public bool CheckAirFareCode(string txtCode,int BookingClassId)
        {
           return _travelContext.tblFlightClassOfBooking.Count(p => p.BookingClassCode == txtCode && p.BookingClassId!= BookingClassId) > 0?false:true;
            
        }

        [HttpGet]
        [Route("Master/getAirFareCode/{BookingClassId}")]
        public mdlReturnData getAirFareCode(int BookingClassId)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {
                tblFlightClassOfBooking tempData = null;
                if (BookingClassId > 0)
                {
                    tempData = _travelContext.tblFlightClassOfBooking.Where(p => p.BookingClassId == BookingClassId).FirstOrDefault();
                    if (tempData == null)
                    {
                        returnData.MessageType = enmMessageType.Error;
                        returnData.Message = "Invalid Data";
                        return returnData;
                    }
                }
                else
                {
                    tempData = new tblFlightClassOfBooking();
                }
                returnData.ReturnId = tempData;
                returnData.MessageType = enmMessageType.Success;
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.MessageType = enmMessageType.Error;
                returnData.Message = ex.Message;
                return returnData;
            }

        }

        [HttpPost]
        [Route("Master/setAirFareCode")]
        [Authorize(nameof(enmDocumentMaster.Travel_Air_FareClass) + nameof(enmDocumentType.Update))]
        public mdlReturnData setAirFareCode(tblFlightClassOfBooking mdl)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {
                tblFlightClassOfBooking tempData = null;
                if (mdl == null)
                {
                    returnData.MessageType = enmMessageType.Error;
                    returnData.Message = "Invalid Data";
                    return returnData;
                }
                if (_travelContext.tblFlightClassOfBooking.Count(p => p.BookingClassCode == mdl.BookingClassCode && p.BookingClassId != mdl.BookingClassId) > 0)
                {
                    returnData.MessageType = enmMessageType.Error;
                    returnData.Message = "Code already exists";
                    return returnData;
                }

                if (mdl.BookingClassId > 0)
                {
                    tempData = _travelContext.tblFlightClassOfBooking.Where(p => p.BookingClassId == mdl.BookingClassId).FirstOrDefault();
                    if (tempData == null)
                    {
                        returnData.MessageType = enmMessageType.Error;
                        returnData.Message = "Invalid Data";
                        return returnData;
                    }
                }
                else
                {
                    tempData = new tblFlightClassOfBooking();
                    tempData.CreatedBy = _IsrvCurrentUser.UserId;
                    tempData.CreatedDt = DateTime.Now;
                }
                tempData.BookingClassCode = mdl.BookingClassCode;
                tempData.Name = mdl.Name;
                tempData.GenerlizedName = mdl.GenerlizedName;
                tempData.IsActive = mdl.IsActive;
                tempData.ModifyRemarks = mdl.ModifyRemarks;
                tempData.ModifiedBy = _IsrvCurrentUser.UserId;
                tempData.ModifiedDt= DateTime.Now;

                if (tempData.BookingClassId>0)
                {
                    _travelContext.tblFlightClassOfBooking.Update(tempData);
                }
                else
                {
                    _travelContext.tblFlightClassOfBooking.Add(tempData);
                }
                _travelContext.SaveChanges();
                returnData.MessageType = enmMessageType.Success;
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.MessageType = enmMessageType.Error;
                returnData.Message = ex.Message;
                return returnData;
            }

        }

        [HttpGet]
        [Route("Master/getAirFareCodes")]
        public mdlReturnData getAirFareCodes([FromServices] IsrvUsers srvUsers)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {
                List<tblFlightClassOfBooking> tempData = new List<tblFlightClassOfBooking>();
               tempData = _travelContext.tblFlightClassOfBooking.ToList();
                var ModifiedBys = tempData.Select(p => p.ModifiedBy ?? 0).Distinct().ToArray();
                var ModifiedByName = srvUsers.GetUsers(ModifiedBys);
                tempData.ForEach(d =>
                {
                    d.ModifiedByName = ModifiedByName.FirstOrDefault(p => p.Id == d.ModifiedBy)?.Name;
                });
                returnData.ReturnId = tempData.AsEnumerable().Select((p, iterator) => new
                {
                    Sno = iterator + 1,
                    modifiedByName = p.ModifiedByName,
                    modifiedDt = p.ModifiedDt,
                    modifyRemarks = p.ModifyRemarks,
                    p.BookingClassId,
                    p.BookingClassCode,
                    p.Name,p.GenerlizedName,p.IsActive
                });
                 returnData.MessageType = enmMessageType.Success;
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.MessageType = enmMessageType.Error;
                returnData.Message = ex.Message;
                return returnData;
            }

        }



        #endregion

        [Route("GetAirport/{onlyActive}/{isDomestic}")]
        public mdlReturnData GetAirport(bool onlyActive, bool isDomestic)
        {
            mdlReturnData mdl = new mdlReturnData() {  MessageType= enmMessageType.Success};
            mdl.ReturnId= _IsrvAir.GetAirport(onlyActive, isDomestic);
            return mdl;            
        }

        [AllowAnonymous]
        [Route("GetAirline/{onlyActive}")]
        public mdlReturnData GetAirline(bool onlyActive)
        {
            mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.Success };
            mdl.ReturnId = onlyActive ? _travelContext.tblAirline.Where(p => p.IsActive).Select(p => new { p.Id, p.Code, p.Name }).OrderBy(p => p.Code).ThenBy(p => p.Name) : _travelContext.tblAirline.Select(p => new { p.Id, p.Code, p.Name }).OrderBy(p=>p.Code).ThenBy(p=>p.Name); 
            return mdl;
        }

        #region ************************ Flight Booking ******************************

        [HttpPost]
        [Route("SearchFlight/{orgCode}")]
        public async Task<mdlReturnData> SearchFlightAsync( mdlFlightSearchWraper request,string orgCode)
        {
            mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.Success };
            Organisation org = new Organisation();
            var tempData=org.ValidateOrganisationForFlight(_context, _IsrvCurrentUser, orgCode);
            if (tempData.MessageType != enmMessageType.Success)
            {
                return tempData;
            }
            var md= await _IsrvAir.FlightSearchAsync(request, tempData.ReturnId.CustomerType, tempData.ReturnId.CustomerId, _IsrvCurrentUser.DistributorId);

            if (md.ResponseStatus == enmMessageType.Success)
            {
                mdl.MessageType = enmMessageType.Success;
                mdl.ReturnId = md;
            }
            else
            {
                mdl.MessageType = enmMessageType.Error;
                mdl.Message = md.Error?.Message;
            }
                  
            return mdl;
        }


        [HttpPost]
        [Route("FareQuote/{orgCode}/{nid}")]
        public async Task<mdlReturnData> FareQuoteAsync(mdlFareQuotRequestWraper request,string orgCode,ulong nid)
        {
            mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {
                Organisation org = new Organisation();
                var tempData = org.ValidateOrganisationForFlight(_context, _IsrvCurrentUser, orgCode);
                if (tempData.MessageType != enmMessageType.Success)
                {
                    return tempData;
                }
                var md = await _IsrvAir.FareQuoteAsync(request, tempData.ReturnId.CustomerType, tempData.ReturnId.CustomerId, _IsrvCurrentUser.DistributorId, _IsrvCurrentUser.UserId);                
                mdl.ReturnId = md;
            }
            catch(Exception ex)
            {
                mdl.MessageType = enmMessageType.Error;
                mdl.Message = ex.Message;
                return mdl;
            }
            mdl.MessageType = enmMessageType.Success;            
            return mdl;
        }
        #endregion

        #region **********settings*********

        #region *************** Provider ***********************
        //service provider management start
        [HttpPost]
        [Route("settings/setserviceprovider")]
        [Authorize(nameof(enmDocumentMaster.Travel_Air_Provider) + nameof(enmDocumentType.Create))]
        public mdlReturnData setserviceprovider(tblFlightSerivceProvider mdl)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {
                var tempData = _IsrvAir.SetServiceProvider(mdl.EffectiveFromDate, mdl.ServiceProvider, mdl.IsEnabled, _IsrvCurrentUser.UserId, mdl.ModifyRemarks);
                return tempData;
            }
            catch (Exception ex)
            {
                returnData.MessageType = enmMessageType.Error;
                returnData.Message = ex.Message;
                return returnData;
            }
            
        }
        [HttpPost]
        [Route("settings/DeleteServiceProvider")]
        [Authorize(nameof(enmDocumentMaster.Travel_Air_Provider) + nameof(enmDocumentType.Delete))]
        public mdlReturnData DeleteServiceProvider(tblFlightSerivceProvider mdl)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {
                var tempData = _travelContext.tblFlightSerivceProvider.Where(p => p.Id == mdl.Id && !p.IsDeleted).FirstOrDefault();
                if (tempData != null)
                {
                    tempData.IsDeleted = true;
                    tempData.ModifiedBy = _IsrvCurrentUser.UserId;
                    tempData.ModifiedDt = DateTime.Now;
                    tempData.ModifyRemarks = string.Concat(tempData.ModifyRemarks ?? "", mdl.ModifyRemarks ?? "");
                    _travelContext.tblFlightSerivceProvider.Update(tempData);
                    _travelContext.SaveChanges();
                    returnData.MessageType = enmMessageType.Success;
                }
                else
                {
                    returnData.MessageType = enmMessageType.Error;
                    returnData.Message = "Invalid data";
                }
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.MessageType = enmMessageType.Error;
                returnData.Message = ex.Message;
                return returnData;
            }

        }

        [HttpGet]
        [Route("settings/getserviceproviders")]
        public mdlReturnData getserviceproviders([FromServices]IsrvUsers srvUsers, bool IsDateFitlter,enmServiceProvider ServiceProvider, DateTime EffectiveFromDate, DateTime EffectiveToDate)
        {
            mdlReturnData mdl = new mdlReturnData();
            try
            {
                List<tblFlightSerivceProvider> Datas = new List<tblFlightSerivceProvider>();
                if (!IsDateFitlter)
                {
                    Datas = _travelContext.tblFlightSerivceProvider.Where(p => !p.IsDeleted && p.ServiceProvider == ServiceProvider).OrderByDescending(p => p.EffectiveFromDate).Take(10).ToList();
                }
                else
                {
                    Datas = _travelContext.tblFlightSerivceProvider.Where(p => !p.IsDeleted && p.EffectiveFromDate>= EffectiveFromDate && p.EffectiveFromDate<=EffectiveToDate).OrderByDescending(p => p.EffectiveFromDate).ToList();
                }
                

                var ModifiedBys = Datas.Select(p => p.ModifiedBy ?? 0).Distinct().ToArray();
                var ModifiedByName = srvUsers.GetUsers(ModifiedBys);
                Datas.ForEach(d =>
                 {
                     d.ModifiedByName = ModifiedByName.FirstOrDefault(p => p.Id == d.ModifiedBy)?.Name;
                 });
                mdl.ReturnId = Datas.Select(p=>new {p.Id,p.ModifiedByName,p.EffectiveFromDate,p.IsEnabled,p.ModifyRemarks, ServiceProvider = p.ServiceProvider.ToString(),p.ModifiedDt });
                mdl.MessageType = enmMessageType.Success;
                return mdl;

            }
            catch (Exception ex)
            {
                mdl.MessageType = enmMessageType.Error;
                mdl.Message = ex.Message;
                return mdl;
            }

        }
        #endregion


        #region *************** Provider priority ***********************
        //service provider management start
        [HttpPost]
        [Route("settings/setServiceProviderPriority")]
        [Authorize(nameof(enmDocumentMaster.Travel_Air_ProviderPriority) + nameof(enmDocumentType.Create))]
        public mdlReturnData setServiceProviderPriority(tblFlightSerivceProviderPriority mdl)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {
                var tempData = _IsrvAir.SetServiceProviderPriority(mdl.EffectiveFromDate, mdl.ServiceProvider, mdl.priority, _IsrvCurrentUser.UserId, mdl.ModifyRemarks);
                return tempData;

            }
            catch (Exception ex)
            {
                returnData.MessageType = enmMessageType.Error;
                returnData.Message = ex.Message;
                return returnData;
            }

        }
        [HttpPost]
        [Route("settings/DeleteServiceProviderPriority")]
        [Authorize(nameof(enmDocumentMaster.Travel_Air_ProviderPriority) + nameof(enmDocumentType.Delete))]
        public mdlReturnData DeleteServiceProviderPriority(tblFlightSerivceProviderPriority mdl)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {
                var tempData = _travelContext.tblFlightSerivceProviderPriority.Where(p => p.Id == mdl.Id && !p.IsDeleted).FirstOrDefault();
                if (tempData != null)
                {
                    tempData.IsDeleted = true;
                    tempData.ModifiedBy = _IsrvCurrentUser.UserId;
                    tempData.ModifiedDt = DateTime.Now;
                    tempData.ModifyRemarks = string.Concat(tempData.ModifyRemarks ?? "", mdl.ModifyRemarks ?? "");
                    _travelContext.tblFlightSerivceProviderPriority.Update(tempData);
                    _travelContext.SaveChanges();
                    returnData.MessageType = enmMessageType.Success;
                }
                else
                {
                    returnData.MessageType = enmMessageType.Error;
                    returnData.Message = "Invalid data";
                }
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.MessageType = enmMessageType.Error;
                returnData.Message = ex.Message;
                return returnData;
            }

        }

        [HttpGet]
        [Route("settings/getServiceProvidersPriority")]
        public mdlReturnData getServiceProvidersPriority([FromServices] IsrvUsers srvUsers, bool IsDateFitlter, enmServiceProvider ServiceProvider, DateTime EffectiveFromDate, DateTime EffectiveToDate)
        {
            mdlReturnData mdl = new mdlReturnData();
            try
            {
                List<tblFlightSerivceProviderPriority> Datas = new List<tblFlightSerivceProviderPriority>();
                if (!IsDateFitlter)
                {
                    
                    Datas = _IsrvAir.GetServiceProviderPriority(DateTime.Now, true); 
                }
                else
                {
                    Datas = _travelContext.tblFlightSerivceProviderPriority.Where(p => !p.IsDeleted && p.EffectiveFromDate >= EffectiveFromDate && p.EffectiveFromDate <= EffectiveToDate).OrderByDescending(p => p.EffectiveFromDate).ToList();
                }


                var ModifiedBys = Datas.Select(p => p.ModifiedBy ?? 0).Distinct().ToArray();
                var ModifiedByName = srvUsers.GetUsers(ModifiedBys);
                Datas.ForEach(d =>
                {
                    d.ModifiedByName = ModifiedByName.FirstOrDefault(p => p.Id == d.ModifiedBy)?.Name;
                });
                mdl.ReturnId = Datas.Select(p => new { p.Id, p.ModifiedByName, p.EffectiveFromDate, p.priority, p.ModifyRemarks, ServiceProvider = p.ServiceProvider.ToString(), p.ModifiedDt }).OrderBy(p=>p.priority);
                mdl.MessageType = enmMessageType.Success;
                return mdl;

            }
            catch (Exception ex)
            {
                mdl.MessageType = enmMessageType.Error;
                mdl.Message = ex.Message;
                return mdl;
            }

        }
        #endregion


        #region ***************** Instant Booking **********************
        //instant booking management start
        [HttpPost]
        [Route("settings/SetInstantBookingSetting")]
        [Authorize(nameof(enmDocumentMaster.Travel_Air_InstantBooking) + nameof(enmDocumentType.Create))]
        public mdlReturnData SetInstantBookingSetting(tblFlightInstantBooking mdl)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {

                var tempData = _IsrvAir.SetInstantBookingSeting(mdl.EffectiveFromDate, mdl.CustomerType, mdl.InstantDomestic, mdl.InstantNonDomestic, _IsrvCurrentUser.UserId, mdl.ModifyRemarks);
                return tempData;
            }
            catch (Exception ex)
            {
                returnData.MessageType = enmMessageType.Error;
                returnData.Message = ex.Message;
                return returnData;
            }


        }

        [HttpPost]
        [Route("settings/DeleteInstantBookingSetting")]
        [Authorize(nameof(enmDocumentMaster.Travel_Air_InstantBooking) + nameof(enmDocumentType.Delete))]
        public mdlReturnData DeleteInstantBookingSetting(tblFlightInstantBooking mdl)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {
                var tempData = _travelContext.tblFlightInstantBooking.Where(p => p.Id == mdl.Id && !p.IsDeleted).FirstOrDefault();
                if (tempData != null)
                {
                    tempData.IsDeleted = true;
                    tempData.ModifiedBy = _IsrvCurrentUser.UserId;
                    tempData.ModifiedDt = DateTime.Now;
                    tempData.ModifyRemarks = string.Concat(tempData.ModifyRemarks ?? "", mdl.ModifyRemarks ?? "");
                    _travelContext.tblFlightInstantBooking.Update(tempData);
                    _travelContext.SaveChanges();
                    returnData.MessageType = enmMessageType.Success;
                }
                else
                {
                    returnData.MessageType = enmMessageType.Error;
                    returnData.Message = "Invalid data";
                }
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.MessageType = enmMessageType.Error;
                returnData.Message = ex.Message;
                return returnData;
            }

        }


        [HttpGet]
        [Route("settings/getInstantBookingSetting")]
        public mdlReturnData GetInstantBookingSetting([FromServices] IsrvUsers srvUsers, bool IsDateFitlter, enmCustomerType CustomerType, DateTime EffectiveFromDate, DateTime EffectiveToDate)
        {
            {
                mdlReturnData mdl = new mdlReturnData();
                try
                {
                    List<tblFlightInstantBooking> Datas = new List<tblFlightInstantBooking>();
                    if (!IsDateFitlter)
                    {
                        Datas =   _travelContext.tblFlightInstantBooking.Where(p => !p.IsDeleted && p.CustomerType == CustomerType).OrderByDescending(p => p.EffectiveFromDate).Take(10).ToList();
                    }
                    else
                    {
                        Datas = _travelContext.tblFlightInstantBooking.Where(p => !p.IsDeleted && p.EffectiveFromDate >= EffectiveFromDate && p.EffectiveFromDate <= EffectiveToDate).OrderByDescending(p => p.EffectiveFromDate).ToList();
                    }


                    var ModifiedBys = Datas.Select(p => p.ModifiedBy ?? 0).Distinct().ToArray();
                    var ModifiedByName = srvUsers.GetUsers(ModifiedBys);
                    Datas.ForEach(d =>
                    {
                        d.ModifiedByName = ModifiedByName.FirstOrDefault(p => p.Id == d.ModifiedBy)?.Name;
                    });
                    mdl.ReturnId = Datas.Select(p => new { p.Id, p.ModifiedByName, p.EffectiveFromDate, p.InstantDomestic,p.InstantNonDomestic, p.ModifyRemarks, CustomerType = p.CustomerType.ToString(), p.ModifiedDt });
                    mdl.MessageType = enmMessageType.Success;
                    return mdl;
                }
                catch (Exception ex)
                {
                    mdl.MessageType = enmMessageType.Error;
                    mdl.Message = ex.Message;
                    return mdl;
                }
            }
        }
        //FlightBookingAlterMaster management start
        #endregion


        #region ********************** flight Alter **************************

        [HttpPost]
        [Route("settings/SetFlightBookingAlterMaster")]
        [Authorize(nameof(enmDocumentMaster.Travel_Air_FlightClassAlter) + nameof(enmDocumentType.Create))]
        public mdlReturnData SetFlightBookingAlterMaster(mdlFlightAlter mdl)
        {
            mdlReturnData tempData = new mdlReturnData() { MessageType = enmMessageType.Success };
            if (!ModelState.IsValid)
            {
                tempData.MessageType = enmMessageType.Error;
                tempData.Message = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(p=>p.ErrorMessage));
                return tempData;
            }
            try
            {
                 tempData = _IsrvAir.SetFlightBookingAlterMaster(mdl, _IsrvCurrentUser.UserId);
                return tempData;

            }
            catch (Exception ex)
            {
                tempData.MessageType = enmMessageType.Error;
                tempData.Message = ex.Message;
                return tempData;
            }

        }

        [HttpPost]
        [Route("settings/DeleteFlightBookingAlterMaster")]
        [Authorize(nameof(enmDocumentMaster.Travel_Air_FlightClassAlter) + nameof(enmDocumentType.Delete))]
        public mdlReturnData DeleteFlightBookingAlterMaster(mdlFlightAlter mdl)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.Success };
            
            try
            {
                var tempData = _travelContext.tblFlightBookingAlterMaster.Where(p=>p.AlterId==mdl.AlterId && !p.IsDeleted).FirstOrDefault();
                if (tempData == null)
                {
                    returnData.MessageType = enmMessageType.Error;
                    returnData.Message = "No Data";
                    return returnData;
                }
                tempData.IsDeleted = true;
                tempData.ModifiedBy = _IsrvCurrentUser.UserId;
                tempData.ModifiedDt = DateTime.Now;
                tempData.ModifyRemarks = tempData.ModifyRemarks ?? "" + mdl.Remarks;
                _travelContext.tblFlightBookingAlterMaster.Update(tempData);
                _travelContext.SaveChanges();
                return returnData;

            }
            catch (Exception ex)
            {
                returnData.MessageType = enmMessageType.Error;
                returnData.Message = ex.Message;
                return returnData;
            }

        }

        [HttpGet]
        [Route("settings/getFlightBookingAlterMaster")]
        public mdlReturnData GetFlightBookingAlterMaster([FromServices]IsrvUsers srvUsers )
        {
            
            mdlReturnData tempData = new mdlReturnData();
            try
            {
                List<mdlFlightAlter> mdl = new List<mdlFlightAlter>();
                mdl = _IsrvAir.GetFlightBookingAlterMaster();
                var ModifiedBys = mdl.Select(p => p.ModifiedBy?? 0).Distinct().ToArray();
                var ModifiedByName = srvUsers.GetUsers(ModifiedBys);
                mdl.ForEach(d =>
                {
                    d.ModifiedByName = ModifiedByName.FirstOrDefault(p => p.Id == d.ModifiedBy)?.Name;
                });
                tempData.ReturnId = mdl.AsEnumerable().Select((p, iterator) => new
                {
                    Sno = iterator + 1,
                    modifiedByName = p.ModifiedByName,
                    modifiedDt = p.ModifiedDt,
                    modifyRemarks = p.Remarks,
                    alterId=p.AlterId,
                    cabinClass=p.CabinClass.ToString(),
                    identifier=p.Identifier,
                    classOfBooking=p.ClassOfBooking,
                    alterDetails=p.AlterDetails.Select(q=>new { Item1 = q.Item1.ToString(),q.Item2,q.Item3})
                });
                tempData.MessageType = enmMessageType.Success;
                return tempData;
            }
            catch (Exception ex)
            {
                tempData.MessageType = enmMessageType.Error;
                tempData.Message = ex.Message;
                return tempData;
            }

        }
        #endregion
        //FlightBookingAlterMaster management end

        //SetFlightFareFilter management start
        [HttpPost]
        [Route("air/settings/SetFlightFareFilter")]
        public mdlReturnData SetFlightFareFilter(mdlFlightFareFilter mdl, ulong UserId)
        {
            mdlReturnData tempData = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {

                tempData = _IsrvAir.SetFlightFareFilter(mdl, UserId);

                return tempData;

            }
            catch (Exception ex)
            {
                tempData.MessageType = enmMessageType.Error;
                tempData.Message = ex.Message;
                return tempData;
            }

        }

        [HttpGet]
        [Route("air/settings/GetFlightFareFilter")]
        public List<mdlFlightFareFilter> GetFlightFareFilter(bool ApplyCustomerFilter, enmCustomerType CustomerType)
        {
            try
            {

                var tempData = _IsrvAir.GetFlightFareFilter(ApplyCustomerFilter, CustomerType);
                return tempData;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        //lightBookingAlterMaster management end
        #endregion

        #region **********customer markup*********
        //customer markup  management start
        [HttpPost]
        [Route("air/settings/SetCustomerMarkup")]
        public mdlReturnData SetCustomerMarkup(double MarkupAmount, DateTime EffectiveFromDt, DateTime EffectiveToDt, int CustomerId, int Nid, string Remarks)
        {
            mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {
                var tempData = _IsrvAir.SetCustomerMarkup(MarkupAmount, EffectiveFromDt, EffectiveToDt, _IsrvCurrentUser.UserId, CustomerId, Nid, Remarks);
                return tempData;

            }
            catch (Exception ex)
            {
                mdl.MessageType = enmMessageType.Error;
                mdl.Message = ex.Message;
                return mdl;
            }

        }

        [HttpGet]
        [Route("air/settings/GetCustomerMarkup")]
        public List<tblFlightCustomerMarkup> GetCustomerMarkup(bool AllMarkup,bool AllActiveMarkup,DateTime ProcessingDate,int CustomerId,ulong Nid,enmCustomerType CustomerType)
        {
            try
            {
                var tempData = _IsrvAir.GetCustomerMarkup(AllMarkup, AllActiveMarkup,ProcessingDate,CustomerId,Nid,CustomerType);
                return tempData;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpPost]
        [Route("air/settings/RemoveCustomerMarkup")]
        public mdlReturnData RemoveCustomerMarkup(int MarkupID, ulong UserId, string Remarks)
        {
            mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {
                var tempData = _IsrvAir.RemoveCustomerMarkup(MarkupID, UserId, Remarks);
                return tempData;

            }
            catch (Exception ex)
            {
                mdl.MessageType = enmMessageType.Error;
                mdl.Message = ex.Message;
                return mdl;
            }

        }


        //customer markup management end
        #endregion


        public mdlReturnData ValidateBasicMarkup(mdlWingMarkup_Air mdl)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.None};
            if (mdl.TravelFromDt > mdl.TravelToDt)
            {
                returnData.Message = returnData.Message+ ", TravelFrom date should be less then TravelToDt ";
                returnData.MessageType = enmMessageType.Error;
            }
            if (mdl.BookingFromDt > mdl.BookingToDt)
            {
                returnData.Message = returnData.Message + ", BookingFromDt date should be less then BookingToDt";
                returnData.MessageType = enmMessageType.Error;
            }
            if (mdl.BookingFromDt > mdl.TravelFromDt)
            {
                returnData.Message = returnData.Message + ", BookingFromDt date should be less then TravelFromDt";
                returnData.MessageType = enmMessageType.Error;
            }
            if (!mdl.IsAllCustomerType)
            {
                if (!mdl.IsAllCustomer)
                {
                    returnData.Message = returnData.Message + ", select All Customer Type";
                    returnData.MessageType = enmMessageType.Error;
                }
            }
            if (!mdl.IsAllProvider)
            {
                if ((mdl.ServiceProviders?.Count() ?? 0) == 0)
                {
                    returnData.Message = returnData.Message + ", Select Providers";
                    returnData.MessageType = enmMessageType.Error;
                }
            }
            if (!mdl.IsAllCustomerType)
            {
                if ((mdl.CustomerTypes?.Count() ?? 0) == 0)
                {
                    returnData.Message = returnData.Message + ", Select Customer Types";
                    returnData.MessageType = enmMessageType.Error;
                }
            }
            if (!mdl.IsAllCustomer)
            {
                if ((mdl.CustomerIds?.Count() ?? 0) == 0)
                {
                    returnData.Message = returnData.Message + ", Select Customer";
                    returnData.MessageType = enmMessageType.Error;
                }
            }
            if (!mdl.IsAllPessengerType)
            {
                if ((mdl.PassengerType?.Count() ?? 0) == 0)
                {
                    returnData.Message = returnData.Message + ", Select Passenger Type";
                    returnData.MessageType = enmMessageType.Error;
                }
            }
            if (!mdl.IsAllFlightClass)
            {
                if ((mdl.CabinClass?.Count() ?? 0) == 0)
                {
                    returnData.Message = returnData.Message + ", Select Cabin class";
                    returnData.MessageType = enmMessageType.Error;
                }
            }
            if (!mdl.IsAllAirline)
            {
                if ((mdl.Airline?.Count() ?? 0) == 0)
                {
                    returnData.Message = returnData.Message + ", Select Airline";
                    returnData.MessageType = enmMessageType.Error;
                }
            }
            if (!mdl.IsAllSegment)
            {
                if ((mdl.Segments?.Count() ?? 0) == 0)
                {
                    returnData.Message = returnData.Message + ", Select Segments";
                    returnData.MessageType = enmMessageType.Error;
                }
            }
            returnData.MessageType = enmMessageType.Success;
            return returnData;
        }

        #region **********wing markup*********
        [HttpPost]
        [Route("Markups/SetMarkup")]
        public mdlReturnData SetMarkup([FromServices]IsrvCustomer isrvCustomer ,mdlWingMarkup_Air mdl,[FromHeader]int OrgId)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.Success };
            if (!ModelState.IsValid)
            {
                returnData.MessageType = enmMessageType.Error;
                returnData.Message = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(p => p.ErrorMessage));
                return returnData;
            }
            returnData = ValidateBasicMarkup(mdl);
            if (returnData.MessageType == enmMessageType.Error)
            {
                return returnData;
            }

            try
            {
                
                {
                    List<tblFlightMarkupServiceProvider> markupServiceProvider = null;
                    List<tblFlightMarkupCustomerType> markupCustomerType = null;
                    List<tblFlightMarkupCustomerDetails> markupCustomer = null;
                    List<tblFlightMarkupPassengerType> markupPassengerType = null;
                    List<tblFlightMarkupFlightClass> markupFlightClass = null;
                    List<tblFlightMarkupAirline> markupAirline = null;
                    List<tblFlightMarkupSegment> markupSegment = null;
                    if (!mdl.IsAllProvider)
                    {
                        markupServiceProvider = new List<tblFlightMarkupServiceProvider>();
                        markupServiceProvider.AddRange(mdl.ServiceProviders.Select(p => new tblFlightMarkupServiceProvider {ServiceProvider=p }));
                    }
                    if (!mdl.IsAllCustomerType)
                    {
                        markupCustomerType = new List<tblFlightMarkupCustomerType>();
                        markupCustomerType.AddRange(mdl.CustomerTypes.Select(p => new tblFlightMarkupCustomerType { customerType = p }));
                    }
                    if (!mdl.IsAllCustomer)
                    {
                        var CustomerList= mdl.CustomerIds.Select(p => p.Item2).Distinct().ToList();
                        var allCustomerList = isrvCustomer.GetCustomers(OrgId, CustomerList);
                        for (int i = 0; i < mdl.CustomerIds.Count(); i++)
                        {
                            string codes = mdl.CustomerIds[i].Item2;
                            mdl.CustomerIds.RemoveAt(i);
                            mdl.CustomerIds.Insert(i, new Tuple<int?,string>(allCustomerList.FirstOrDefault(q => q.Code == mdl.CustomerIds[i].Item2)?.CustomerId, codes) ) ;
                        }
                        if (CustomerList.Count() != mdl.CustomerIds.Count())
                        {
                            returnData.Message = "Duplicate Customer Ids";
                            returnData.MessageType = enmMessageType.Error;
                            return returnData;
                        }
                        if (mdl.CustomerIds.Any(p => p.Item1 == null))
                        {
                            returnData.Message = "Invalid Customer "+string.Concat(", "+ mdl.CustomerIds.Select(p=>p.Item2));
                            returnData.MessageType = enmMessageType.Error;
                            return returnData;
                        }
                        markupCustomer = new List<tblFlightMarkupCustomerDetails>();
                        markupCustomer.AddRange(mdl.CustomerIds.Select(p => new tblFlightMarkupCustomerDetails { CustomerId = p.Item1 }));
                    }
                    if (!mdl.IsAllPessengerType)
                    {
                        markupPassengerType = new List<tblFlightMarkupPassengerType>();
                        markupPassengerType.AddRange(mdl.PassengerType.Select(p => new tblFlightMarkupPassengerType { PassengerType = p }));
                    }
                    if (!mdl.IsAllFlightClass)
                    {
                        markupFlightClass = new List<tblFlightMarkupFlightClass>();
                        markupFlightClass.AddRange(mdl.CabinClass.Select(p => new tblFlightMarkupFlightClass { CabinClass= p }));
                    }
                    if (!mdl.IsAllAirline)
                    {
                        markupAirline= new List<tblFlightMarkupAirline>();
                        markupAirline.AddRange(mdl.Airline.Select(p => new tblFlightMarkupAirline { AirlineId = p.Item1 }));
                    }
                    if (!mdl.IsAllSegment)
                    {
                        markupSegment = new List<tblFlightMarkupSegment>();
                        markupSegment.AddRange(mdl.Segments.Select(p => new tblFlightMarkupSegment { orign=p.Item1, destination = p.Item2 }));
                    }


                    tblFlightMarkupMaster markupMaster = new tblFlightMarkupMaster() {
                        Applicability= mdl.Applicability,
                        IsAllProvider=mdl.IsAllProvider,
                        IsAllCustomerType=mdl.IsAllCustomerType,
                        IsAllCustomer=mdl.IsAllCustomer,
                        IsAllPessengerType=mdl.IsAllPessengerType,
                        IsAllFlightClass=mdl.IsAllFlightClass,
                        IsAllAirline=mdl.IsAllAirline,
                        IsMLMIncentive= mdl.IsMLMIncentive,//only acceptable in Markup not in ( Convienve and Discount)
                        IsAllSegment=mdl.IsAllSegment,
                        FlightType=mdl.FlightType,
                        Gender=mdl.Gender,
                        IsPercentage=mdl.IsPercentage,
                        PercentageValue=mdl.PercentageValue,
                        Amount=mdl.Amount,
                        AmountCaping=mdl.AmountCaping,
                        TravelFromDt=mdl.TravelFromDt,
                        TravelToDt= mdl.TravelToDt,
                        BookingFromDt= mdl.BookingFromDt,
                        BookingToDt = mdl.BookingToDt,
                        IsDeleted=false,
                        tblFlightMarkupServiceProvider=markupServiceProvider,
                        tblFlightMarkupCustomerType=markupCustomerType,
                        tblFlightMarkupCustomerDetails=markupCustomer,
                        tblFlightMarkupPassengerType=markupPassengerType,
                        tblFlightMarkupFlightClass=markupFlightClass,
                        tblFlightMarkupAirline=markupAirline,
                        tblFlightMarkupSegment=markupSegment


                    };
                    _travelContext.tblFlightMarkupMaster.Add(markupMaster);
                    _travelContext.SaveChanges();
                }

                returnData.MessageType = enmMessageType.Success;
                returnData.Message = "Save successfully";
                
                return returnData;

            }
            catch (Exception ex)
            {
                returnData.MessageType = enmMessageType.Error;
                returnData.Message = ex.Message;
                return returnData;
            }

        }

        [HttpGet]
        [Route("air/settings/GetWingMarkup")]
        public List<mdlWingMarkup_Air> GetWingMarkup(bool OnlyActive, bool FilterDateCriteria, enmCustomerType CustomerType, int customerId,
            DateTime TravelDt, DateTime BookingDate)
        {
            try
            {

                var tempData = _IsrvAir.GetWingMarkup(OnlyActive, FilterDateCriteria, CustomerType, customerId,TravelDt, BookingDate);
                return tempData;

            }
            catch (Exception ex)
            {
                return null;
            }

        }


        //wing markup management end
        #endregion

        #region **********wing discount*********
        //wing markup  management start
        //[HttpPost]
        //[Route("air/settings/SetCustomerMarkup")]
        //public mdlReturnData SetCustomerMarkup(double MarkupAmount, DateTime EffectiveFromDt, DateTime EffectiveToDt, ulong UserId, int CustomerId, int Nid, string Remarks)
        //{
        //    mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.Success };
        //    try
        //    {
        //        var tempData = _IsrvAir.SetCustomerMarkup(MarkupAmount, EffectiveFromDt, EffectiveToDt, UserId, CustomerId, Nid, Remarks);
        //        return tempData;

        //    }
        //    catch (Exception ex)
        //    {
        //        mdl.MessageType = enmMessageType.Error;
        //        mdl.Message = ex.Message;
        //        return mdl;
        //    }

        //}

        [HttpGet]
        [Route("air/settings/GetWingDiscount")]
        public List<mdlWingMarkup_Air> GetWingDiscount(bool OnlyActive, bool FilterDateCriteria, enmCustomerType CustomerType, int customerId,
            DateTime TravelDt, DateTime BookingDate)
        {
            try
            {

                var tempData = _IsrvAir.GetWingDiscount(OnlyActive, FilterDateCriteria, CustomerType, customerId, TravelDt, BookingDate);
                return tempData;

            }
            catch (Exception ex)
            {
                return null;
            }

        }


        //wing discount management end
        #endregion

        #region **********wing Convenience*********
        //wing markup  management start
        //[HttpPost]
        //[Route("air/settings/SetCustomerMarkup")]
        //public mdlReturnData SetCustomerMarkup(double MarkupAmount, DateTime EffectiveFromDt, DateTime EffectiveToDt, ulong UserId, int CustomerId, int Nid, string Remarks)
        //{
        //    mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.Success };
        //    try
        //    {
        //        var tempData = _IsrvAir.SetCustomerMarkup(MarkupAmount, EffectiveFromDt, EffectiveToDt, UserId, CustomerId, Nid, Remarks);
        //        return tempData;

        //    }
        //    catch (Exception ex)
        //    {
        //        mdl.MessageType = enmMessageType.Error;
        //        mdl.Message = ex.Message;
        //        return mdl;
        //    }

        //}

        [HttpGet]
        [Route("air/settings/GetWingConvenience")]
        public List<mdlWingMarkup_Air> GetWingConvenience(bool OnlyActive, bool FilterDateCriteria, enmCustomerType CustomerType, int customerId,
            DateTime TravelDt, DateTime BookingDate)
        {
            try
            {

                var tempData = _IsrvAir.GetWingConvenience(OnlyActive, FilterDateCriteria, CustomerType, customerId, TravelDt, BookingDate);
                return tempData;

            }
            catch (Exception ex)
            {
                return null;
            }

        }


        //wing Convenience management end
        #endregion
    }
}

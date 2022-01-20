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
        [Route("GetAirport/{onlyActive}/{isDomestic}")]
        public mdlReturnData GetAirport(bool onlyActive, bool isDomestic)
        {
            mdlReturnData mdl = new mdlReturnData() {  MessageType= enmMessageType.Success};
            mdl.ReturnId= _IsrvAir.GetAirport(onlyActive, isDomestic);
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
        //service provider management start
        [HttpPost]
        [Route("settings/setserviceprovider")]
        [Authorize(nameof(enmDocumentMaster.Travel_Air_Provider) + nameof(enmDocumentType.Create))]
        public mdlReturnData setserviceprovider([FromForm] tblFlightSerivceProvider mdl)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {   
                var tempData = _IsrvAir.SetServiceProvider(mdl.EffectiveFromDate ,mdl.ServiceProvider , mdl.IsEnabled, _IsrvCurrentUser.UserId , mdl.ModifyRemarks);
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
        [Authorize(nameof(enmDocumentMaster.Travel_Air_Provider) + nameof(enmDocumentType.Update))]
        public mdlReturnData DeleteServiceProvider(tblFlightSerivceProvider mdl)
        {
            mdlReturnData returnData = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {
                var tempData = _travelContext.tblFlightSerivceProvider.Where(p => p.Id == mdl.Id && !p.IsDeleted).FirstOrDefault();
                if (tempData == null)
                {
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
                mdl.ReturnId = Datas;
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
        //service provider management end

        //instant booking management start
        [HttpPost]
        [Route("settings/SetInstantBookingSetting")]
        public mdlReturnData SetInstantBookingSetting(DateTime EffectiveFromDate, enmCustomerType CustomerType, bool InstantDomestic, bool InstantNonDomestic, ulong UserId, string Remarks)
        {
            mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {

                var tempData = _IsrvAir.SetInstantBookingSeting(EffectiveFromDate, CustomerType, InstantDomestic, InstantNonDomestic, UserId, Remarks);

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
        [Route("air/settings/getInstantBookingSetting")]
        public List<tblFlightInstantBooking> GetInstantBookingSetting(bool FilterDate, DateTime ProcessDate)
        {
            try
            {

                var tempData = _IsrvAir.GetInstantBookingSeting(FilterDate,ProcessDate);
                return tempData;

            }
            catch (Exception ex)
            {

                return null;
            }

        }
        //FlightBookingAlterMaster management start
        [HttpPost]
        [Route("air/settings/SetFlightBookingAlterMaster")]
        public mdlReturnData SetFlightBookingAlterMaster(mdlFlightAlter mdl, ulong UserId)
        {
           mdlReturnData tempData = new mdlReturnData() { MessageType = enmMessageType.Success };
            try
            {

                 tempData = _IsrvAir.SetFlightBookingAlterMaster(mdl, UserId);

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
        [Route("air/settings/GetFlightBookingAlterMaster")]
        public List<mdlFlightAlter> GetFlightBookingAlterMaster()
        {
            try
            {

                var tempData = _IsrvAir.GetFlightBookingAlterMaster();
                return tempData;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
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

        #region **********wing markup*********
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

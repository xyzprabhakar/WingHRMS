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

namespace projAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirController : ControllerBase
    {
        private readonly IsrvAir _IsrvAir;
        private readonly IsrvCurrentUser _IsrvCurrentUser;
        private readonly Context _context;
        public AirController(IsrvAir isrvAir, IsrvCurrentUser isrvCurrentUser, Context context)
        {
            _IsrvAir = isrvAir;
            _IsrvCurrentUser = isrvCurrentUser;
            _context = context;
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
        [Route("SearchFlight")]
        public async Task<mdlReturnData> SearchFlightAsync( mdlFlightSearchWraper request,[FromQuery]string OrgCode)
        {
            mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.Success };
            Organisation org = new Organisation();
            var tempData=org.ValidateOrganisationForFlight(_context, _IsrvCurrentUser, OrgCode);
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
        [Route("FareQuote")]
        public async Task<mdlReturnData> FareQuoteAsync(mdlFareQuotRequestWraper request, [FromQuery] string OrgCode)
        {
            mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.Success };
            Organisation org = new Organisation();
            var tempData = org.ValidateOrganisationForFlight(_context, _IsrvCurrentUser, OrgCode);
            if (tempData.MessageType != enmMessageType.Success)
            {
                return tempData;
            }
            var md = await _IsrvAir.FareQuoteAsync(request);

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
        #endregion

    }
}

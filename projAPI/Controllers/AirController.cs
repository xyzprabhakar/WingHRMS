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
            var OrgData=_context.tblCustomerOrganisation.Where(p => p.OrganisationCode == OrgCode && p.IsActive).FirstOrDefault();
            if (OrgData == null)
            {
                mdl.MessageType = enmMessageType.Error;
                mdl.Message = "Invalid company code";
                return mdl;
            }

            if (!(OrgData.CustomerId == 1 || OrgData.CustomerId == 2))
            {
                if (_IsrvCurrentUser.user_type != enmUserType.Employee)
                {
                    if (_IsrvCurrentUser.CustomerId != OrgData.CustomerId)
                    {
                        mdl.MessageType = enmMessageType.Error;
                        mdl.Message = "Unauthorize access";
                        return mdl;
                    }
                }
            }

           var md= await _IsrvAir.FlightSearchAsync(request, OrgData.CustomerType, OrgData.CustomerId, _IsrvCurrentUser.DistributorId);

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

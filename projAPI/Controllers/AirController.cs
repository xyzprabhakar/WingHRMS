using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projAPI.Model;
using projAPI.Services.Travel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projContext;

namespace projAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirController : ControllerBase
    {
        private readonly IsrvAir _IsrvAir;
        public AirController(IsrvAir isrvAir)
        {
            _IsrvAir = isrvAir;
        }
        [Route("GetAirport/{onlyActive}/{isDomestic}")]
        public mdlReturnData GetAirport(bool onlyActive, bool isDomestic)
        {
            mdlReturnData mdl = new mdlReturnData() {  MessageType= enmMessageType.Success};
            mdl.ReturnId= _IsrvAir.GetAirport(onlyActive, isDomestic);
            return mdl;
            
        }

    }
}

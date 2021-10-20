using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projAPI.Model;
using projAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using projContext;

namespace projAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IsrvUsers _IsrvUsers;
        private readonly IsrvSettings _IsrvSettings;
        
        public UserController(IsrvUsers IsrvUsers, IsrvSettings isrvSettings)
        {
            _IsrvUsers = IsrvUsers;
            _IsrvSettings = isrvSettings;
            
        }

        [Route("GetTempUser")]
        public mdlReturnData GetTempUser([FromServices] IHttpContextAccessor httpContext)
        {
            mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.Error };
            string _RemoteIpAddress = _IsrvSettings.GetClientIP(httpContext);
            string _DeviceId = _IsrvSettings.GetDeviceDetails(_RemoteIpAddress);
            var TempUserID =_IsrvUsers.GenrateTempUser(_DeviceId,_DeviceId);
            mdl.ReturnId = new { TempUserId = TempUserID };
            return mdl;
        }

        //public mdlLoginRequest Login()
        //{
        //    mdlLoginRequest mdl = new mdlLoginRequest();
        //    _IsrvSettings.GenrateCaptcha()
        //}

    }
}

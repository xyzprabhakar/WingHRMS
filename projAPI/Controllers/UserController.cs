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

        
        [Route("GenrateLoginCaptcha")]
        public mdlReturnData GenrateLoginCaptcha([FromServices] IHttpContextAccessor httpContext,string TempUserId)
        {
            mdlReturnData mdl = new mdlReturnData();
            if (string.IsNullOrEmpty(TempUserId))
            {
                string _RemoteIpAddress = _IsrvSettings.GetClientIP(httpContext);
                string _DeviceId = _IsrvSettings.GetDeviceDetails(_RemoteIpAddress);
                TempUserId = _IsrvUsers.GenrateTempUser(_DeviceId, _DeviceId);
            }
            
            var tempData=_IsrvSettings.GenrateCaptcha(0, TempUserId, "LoginCaptchaExpiryTime");
            if (tempData.MessageType == enmMessageType.Success)
            {
                var image = _IsrvSettings.GenerateImage(100, 36, tempData.ReturnId.SecurityStampValue);
                mdl.MessageType = enmMessageType.Success;
                mdl.ReturnId = new { TempUserId = TempUserId, CaptchaId = tempData.ReturnId.SecurityStamp, CaptchaImage = "data:image/jpeg;base64," + Convert.ToBase64String(image) };
                return mdl;
            }
            else
            {
                return tempData;
            }
        }

        [HttpPost]
        [Route("Login")]
        public mdlReturnData Login([FromServices] IHttpContextAccessor httpContext, mdlLoginRequest mdlRequest)
        {
            string _RemoteIpAddress = _IsrvSettings.GetClientIP(httpContext);
            string _DeviceId = _IsrvSettings.GetDeviceDetails(_RemoteIpAddress);

            mdlReturnData mdl = new mdlReturnData() {MessageType= enmMessageType.Error};
            var tempData=_IsrvSettings.ValidateCaptcha(mdlRequest.CaptchaValue, mdlRequest.CaptchaId);
            if (tempData.MessageType != enmMessageType.Success)
            {
                return tempData;
            }
            mdlReturnData tempDataValidate = _IsrvUsers.ValidateUser(mdlRequest.UserName, Classes.AESEncrytDecry.EncryptStringAES(mdlRequest.Password),mdlRequest.OrgCode, mdlRequest.UserType);
            if (tempDataValidate.MessageType != enmMessageType.Success)
            {
                _IsrvUsers.SaveLoginLog(_RemoteIpAddress, _DeviceId, false, mdlRequest.FromLocation, mdlRequest.Longitute, mdlRequest.Longitute);
                return tempDataValidate;
            }
            if (mdlRequest.UserType.HasFlag( enmUserType.Employee))
            { 
                
            }

            return mdl;
        }

    }
}

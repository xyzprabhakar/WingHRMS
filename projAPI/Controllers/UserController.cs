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
using Microsoft.Extensions.Configuration;

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
        public mdlReturnData Login([FromServices] IHttpContextAccessor httpContext,
            [FromServices] IsrvEmployee isrvEmployee,
            [FromServices] IsrvDistributer isrvDistributer,
            [FromServices] IConfiguration config,
            mdlLoginRequest mdlRequest)
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
                int EmpId = 0;
                int.TryParse(tempDataValidate.ReturnId.employee_id,out EmpId);
                if (!isrvEmployee.IsActiveEmpExistsById(EmpId))
                {
                    mdl.Message = "Inactive Employee";
                    return mdl;
                }
            }
            if (mdlRequest.UserType.HasFlag(enmUserType.Consolidator))
            {
                ulong DistributorId = 0;
                ulong.TryParse(tempDataValidate.ReturnId.DistributorId, out DistributorId);
                if (!isrvDistributer.IsActiveDistributerExistsByNid(DistributorId))
                {
                    mdl.Message = "Terminated Distributer";
                    return mdl;
                }
            }
            string JSONWebToken= _IsrvUsers.GenerateJSONWebToken(config["Jwt:Key"], config["Jwt:Issuer"],
                tempDataValidate.ReturnId.UserId ?? 0,
                tempDataValidate.ReturnId.employee_id ?? 0,
                tempDataValidate.ReturnId.user_type ?? 0,
                tempDataValidate.ReturnId.CustomerId??0,
                tempDataValidate.ReturnId.DistributorId??0);

            mdl.MessageType = enmMessageType.Success;
            mdl.ReturnId =
                new
                {
                    UserId = tempDataValidate.ReturnId.UserId,
                    NormalizedName = tempDataValidate.ReturnId.NormalizedName,
                    employee_id = tempDataValidate.ReturnId.employee_id,
                    user_type = tempDataValidate.ReturnId.user_type,
                    CustomerId = tempDataValidate.ReturnId.CustomerId,
                    DistributorId = tempDataValidate.ReturnId.DistributorId,
                    JSONWebToken= JSONWebToken
                };
            return mdl;
        }

    }
}

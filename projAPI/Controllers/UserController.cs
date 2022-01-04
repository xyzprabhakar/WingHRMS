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
using Microsoft.AspNetCore.Authorization;

namespace projAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IsrvUsers _IsrvUsers;
        private readonly IsrvSettings _IsrvSettings;
        private readonly IsrvCurrentUser _IsrvCurrentUser;
        public UserController(IsrvUsers IsrvUsers, IsrvSettings isrvSettings, IsrvCurrentUser isrvCurrentUser)
        {
            _IsrvUsers = IsrvUsers;
            _IsrvSettings = isrvSettings;
            _IsrvCurrentUser = isrvCurrentUser;            
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

        
        [Route("GenrateLoginCaptcha/{TempUserId}")]
        public mdlReturnData GenrateLoginCaptcha([FromServices] IHttpContextAccessor httpContext,string TempUserId)
        {
            mdlReturnData mdl = new mdlReturnData();
            if (string.IsNullOrEmpty(TempUserId) || TempUserId=="0")
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
            
            int EmpId = 0, CustomerId=0;
            ulong DistributorId = 0, UserId=0;

            mdlReturnData mdl = new mdlReturnData() {MessageType= enmMessageType.Error};
            var tempData=_IsrvSettings.ValidateCaptcha(mdlRequest.CaptchaValue, mdlRequest.CaptchaId);
            if (tempData.MessageType != enmMessageType.Success)
            {
                return tempData;
            }
            string EncPassword = Classes.AESEncrytDecry.EncryptStringAES(mdlRequest.Password);
            mdlReturnData tempDataValidate = _IsrvUsers.ValidateUser(mdlRequest.UserName, EncPassword, mdlRequest.OrgCode, mdlRequest.UserType);
            if (tempDataValidate.MessageType != enmMessageType.Success)
            {
                _IsrvUsers.SaveLoginLog(_RemoteIpAddress, _DeviceId, false, mdlRequest.FromLocation, mdlRequest.Longitute, mdlRequest.Longitute);
                return tempDataValidate;
            }
            if (mdlRequest.UserType.HasFlag( enmUserType.Employee))
            {
                
                int.TryParse(Convert.ToString( tempDataValidate.ReturnId.employee_id),out EmpId);
                if (!isrvEmployee.IsActiveEmpExistsById(EmpId))
                {
                    mdl.Message = "Inactive Employee";
                    return mdl;
                }
            }
            if (mdlRequest.UserType.HasFlag(enmUserType.Consolidator))
            {
                
                ulong.TryParse(Convert.ToString(tempDataValidate.ReturnId.DistributorId), out DistributorId);
                if (!isrvDistributer.IsActiveDistributerExistsByNid(DistributorId))
                {
                    mdl.Message = "Terminated Distributer";
                    return mdl;
                }
            }
            ulong.TryParse(Convert.ToString(tempDataValidate.ReturnId.UserId),out UserId);
            int.TryParse(Convert.ToString(tempDataValidate.ReturnId.CustomerId), out CustomerId);

            string JSONWebToken= _IsrvUsers.GenerateJSONWebToken(config["Jwt:Key"], config["Jwt:Issuer"],
                UserId ,EmpId,tempDataValidate.ReturnId.user_type,CustomerId,
                DistributorId);

            _IsrvUsers.SaveLoginLog(_RemoteIpAddress, _DeviceId, true, mdlRequest.FromLocation, mdlRequest.Longitute, mdlRequest.Longitute);
            mdl.MessageType = enmMessageType.Success;
            mdl.ReturnId =
                new
                {
                    UserId = UserId,
                    NormalizedName = tempDataValidate.ReturnId.NormalizedName,
                    employee_id = EmpId,
                    user_type = tempDataValidate.ReturnId.user_type,
                    CustomerId = CustomerId,
                    DistributorId = DistributorId,
                    JSONWebToken= JSONWebToken
                };
            return mdl;
        }

        [Authorize]
        [Route("GetUserApplication")]
        public mdlReturnData GetUserApplication()
        {
            mdlReturnData mdl = new mdlReturnData() { Message="",MessageType= enmMessageType.Success};
            mdl.ReturnId=_IsrvUsers.GetUserApplication(_IsrvCurrentUser.UserId);
            return mdl;
        }

        [Authorize]
        [Route("GetUserDocuments")]
        public mdlReturnData GetUserDocuments()
        {
            mdlReturnData mdl = new mdlReturnData() { Message = "", MessageType = enmMessageType.Success };
            mdl.ReturnId = _IsrvUsers.GetUserDocuments(_IsrvCurrentUser.UserId);
            return mdl;
        }

        [Authorize]
        [Route("GetDownlineEmployee")]
        public mdlReturnData GetDownlineEmployee([FromServices]IsrvEmployee isrvEmployee,
            DateTime processingDate, bool OnlyActive, bool IsRequiredName
            )
        {
            mdlReturnData mdl = new mdlReturnData() { Message = "", MessageType = enmMessageType.None };
            try
            {
                isrvEmployee.EmpId = _IsrvCurrentUser.employee_id;
                isrvEmployee.UserId = _IsrvCurrentUser.UserId;
                isrvEmployee.Role= _IsrvUsers.GetUserRole(_IsrvCurrentUser.UserId);
                mdl.ReturnId = isrvEmployee.GetDownline(processingDate, OnlyActive, false, IsRequiredName);
                mdl.MessageType = enmMessageType.Success;
            }
            catch (Exception ex)
            {
                mdl.MessageType = enmMessageType.Error;
                mdl.Message = ex.Message;
            }
            return mdl;
        }


    }
}

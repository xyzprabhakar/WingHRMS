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
        private readonly IsrvMasters _IsrvMasters;
        public UserController(IsrvUsers IsrvUsers, IsrvSettings isrvSettings, IsrvCurrentUser isrvCurrentUser, IsrvMasters  IsrvMasters)
        {
            _IsrvUsers = IsrvUsers;
            _IsrvSettings = isrvSettings;
            _IsrvCurrentUser = isrvCurrentUser;
            _IsrvMasters = IsrvMasters;


        }

        [Route("GetTempUser")]
        public mdlReturnData GetTempUser([FromServices] IHttpContextAccessor httpContext)
        {
            mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.Error };
            if (httpContext == null)
            {
                mdl.Message= "httpcontext is null";
            }
            try
            {
                string _RemoteIpAddress = _IsrvSettings.GetClientIP(httpContext);
                string _DeviceId = _IsrvSettings.GetDeviceDetails(_RemoteIpAddress);
                var TempUserID = _IsrvUsers.GenrateTempUser(_DeviceId, _DeviceId);
                mdl.ReturnId = new { TempUserId = TempUserID };
                return mdl;
            }
            catch (Exception ex)
            {
                mdl.Message= ex.Message;
                return mdl;
            }
            
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
            [FromServices] IsrvCustomer isrvCustomer,
            [FromServices] IConfiguration config,
            mdlLoginRequest mdlRequest)
        {
            string _RemoteIpAddress = _IsrvSettings.GetClientIP(httpContext);
            string _DeviceId = _IsrvSettings.GetDeviceDetails(_RemoteIpAddress);
            
            int EmpId = 0, CustomerId=0,OrgId=0,VendorId=0;
            ulong DistributorId = 0, UserId=0;
            enmCustomerType CustomerType=enmCustomerType.B2C;

            mdlReturnData mdl = new mdlReturnData() {MessageType= enmMessageType.Error};
            var tempData=_IsrvSettings.ValidateCaptcha(mdlRequest.CaptchaValue, mdlRequest.CaptchaId);
            if (tempData.MessageType != enmMessageType.Success)
            {
                return tempData;
            }
            if (string.IsNullOrEmpty(mdlRequest.OrgCode))
            {
                mdlRequest.OrgCode = config["OrganisationSetting:Organisation:OrganisationCode"];
                if (string.IsNullOrEmpty(mdlRequest.OrgCode))
                {
                    tempData.MessageType = enmMessageType.Error;
                    tempData.Message = "Required Organisation Code";
                    return tempData;
                }                
            }
            var orgData = _IsrvMasters.GetOrganisation(mdlRequest.OrgCode);
            if (orgData == null)
            {
                tempData.MessageType = enmMessageType.Error;
                tempData.Message = "Invalid Organisation";
                return tempData;
            }
            if (!orgData.IsActive)
            {
                tempData.MessageType = enmMessageType.Error;
                tempData.Message = "Inactive Organisation";
                return tempData;
            }
            OrgId = orgData.Id;

            if (mdlRequest.UserType.HasFlag(enmUserType.Customer))
            {
                var CustomerData = isrvCustomer.GetCustomer(OrgId, mdlRequest.CustomerCode);
                if (CustomerData==null)
                {
                    tempData.MessageType = enmMessageType.Error;
                    tempData.Message = "Invalid Customer";
                    return tempData;
                }
                if (!CustomerData.IsActive)
                {
                    tempData.MessageType = enmMessageType.Error;
                    tempData.Message = "Inactive Customer";
                    return tempData;
                }
                CustomerId = CustomerData.CustomerId;
                CustomerType = CustomerData.CustomerType;
                if (CustomerType == enmCustomerType.MLM)
                {
                    throw new NotImplementedException();
                }
            }

            string EncPassword = Classes.AESEncrytDecry.EncryptStringAES(mdlRequest.Password);
            mdlReturnData tempDataValidate = _IsrvUsers.ValidateUser(mdlRequest.UserName, EncPassword, OrgId, CustomerId, VendorId, mdlRequest.UserType);
            if (tempDataValidate.MessageType != enmMessageType.Success)
            {
                _IsrvUsers.SaveLoginLog(_RemoteIpAddress, _DeviceId, false, mdlRequest.FromLocation, mdlRequest.Longitute, mdlRequest.Longitute);
                return tempDataValidate;
            }
            UserId = tempDataValidate.ReturnId.UserId;
            if (mdlRequest.UserType.HasFlag(enmUserType.Employee))
            {   
                int.TryParse(Convert.ToString( tempDataValidate.ReturnId.EmpId),out EmpId);
                if (!isrvEmployee.IsActiveEmpExistsById(EmpId))
                {
                    mdl.Message = "Inactive Employee";
                    return mdl;
                }
            }
            if (mdlRequest.UserType.HasFlag(enmUserType.Customer) || enmCustomerType.MLM ==CustomerType )
            {   
                ulong.TryParse(Convert.ToString(tempDataValidate.ReturnId.DistributorId), out DistributorId);
                if (!isrvDistributer.IsActiveDistributerExistsByNid(DistributorId))
                {
                    mdl.Message = "Terminated Distributer";
                    return mdl;
                }
            }
            

            string JSONWebToken= _IsrvUsers.GenerateJSONWebToken(config["Jwt:Key"], config["Jwt:Issuer"],
                UserId ,CustomerId,EmpId,VendorId,DistributorId,mdlRequest.UserType,CustomerType,OrgId);
            _IsrvUsers.SaveLoginLog(_RemoteIpAddress, _DeviceId, true, mdlRequest.FromLocation, mdlRequest.Longitute, mdlRequest.Longitute);
            mdl.MessageType = enmMessageType.Success;
            mdl.ReturnId =
                new
                {
                    UserId = UserId,
                    NormalizedName = tempDataValidate.ReturnId.NormalizedName,
                    Email = tempDataValidate.ReturnId.Email,
                    PhoneNumber = tempDataValidate.ReturnId.PhoneNumber,
                    EmpId = EmpId,
                    CustomerId = CustomerId,
                    DistributorId = DistributorId,
                    VendorId = VendorId ,
                    UserType = mdlRequest.UserType,
                    CustomerType= CustomerType,
                    JSONWebToken = JSONWebToken,
                    OrgId=OrgId,
                };
            return mdl;
        }

        [Authorize]
        [Route("GetUserApplication")]
        public mdlReturnData GetUserApplication()
        {
            mdlReturnData mdl = new mdlReturnData() { Message="",MessageType= enmMessageType.Success};
            //mdl.ReturnId=_IsrvUsers.GetUserApplication(_IsrvCurrentUser.UserId);
            return mdl;
        }

        [Authorize]
        [Route("GetCurrentUserOrganisation")]
        public mdlReturnData GetCurrentUserOrganisation()
        {
            mdlReturnData mdl = new mdlReturnData() { Message = "", MessageType = enmMessageType.Success };
            mdl.ReturnId=_IsrvUsers.GetUserOrganisation(_IsrvCurrentUser.UserId);
            return mdl;
        }
        [Authorize]
        [Route("GetCurrentUserCompany")]
        public mdlReturnData GetCurrentUserCompany(int? OrgId )
        {
            mdlReturnData mdl = new mdlReturnData() { Message = "", MessageType = enmMessageType.Success };
            mdl.ReturnId = _IsrvUsers.GetUserCompany(_IsrvCurrentUser.UserId, OrgId, null);
            return mdl;
        }
        [Authorize]
        [Route("GetCurrentUserZone")]
        public mdlReturnData GetCurrentUserZone(int? OrgId,int? CompanyId )
        {
            mdlReturnData mdl = new mdlReturnData() { Message = "", MessageType = enmMessageType.Success };
            mdl.ReturnId = _IsrvUsers.GetUserZone(_IsrvCurrentUser.UserId, OrgId, CompanyId,null);
            return mdl;
        }
        [Authorize]
        [Route("GetCurrentUserLocation")]
        public mdlReturnData GetCurrentUserLocation(bool ClearCache,int? OrgId, int? CompanyId, int? ZoneId)
        {
            mdlReturnData mdl = new mdlReturnData() { Message = "", MessageType = enmMessageType.Success };
            mdl.ReturnId = _IsrvUsers.GetUserLocation(ClearCache, _IsrvCurrentUser.UserId, OrgId, CompanyId, ZoneId);
            return mdl;
        }

        [Authorize]
        [Route("GetUserDocuments/{OnlyDisplayMenu}/{IncludeApplication}/{IncludeModule}/{IncludeSubModule}")]
        public mdlReturnData GetUserDocuments(bool OnlyDisplayMenu, bool IncludeApplication, bool IncludeModule, bool IncludeSubModule)
        {
            mdlReturnData mdl = new mdlReturnData() { Message = "", MessageType = enmMessageType.Success };
            List<Document> documents = _IsrvUsers.GetUserDocuments(_IsrvCurrentUser.UserId, OnlyDisplayMenu).OrderBy(p=>p.DisplayOrder).ToList();
            List<Module> modules = new List<Module>();
            List<SubModule> submodules = new List<SubModule>();
            List<Application> applications = new List<Application>();
            if (IncludeModule)
            {
                modules = documents.Where(p => p.EnmModule.HasValue).Select(p => p.EnmModule).Distinct().Select(p => p.Value.GetModuleDetails()).OrderBy(p => p.DisplayOrder).ToList();
            }
            if (IncludeSubModule)
            {
                submodules = documents.Where(p => p.EnmSubModule.HasValue).Select(p => p.EnmSubModule).Distinct().Select(p => p.Value.GetSubModuleDetails()).OrderBy(p => p.DisplayOrder).ToList();
            }
            if (IncludeApplication)
            {
                applications = documents.Where(p => p.EnmApplication.HasValue).Select(p => p.EnmApplication).Distinct().Select(p => p.Value.GetApplicationDetails()).OrderBy(p => p.DisplayOrder).ToList();
            }

            List<mdlMenuWraper> menuWraper = new List<mdlMenuWraper>();
            menuWraper.Add(new mdlMenuWraper { applicationId=0, menuData= getMenudata(null) });
            //Genrate Menu 
            foreach (var app in applications)
            {
                menuWraper.Add(new mdlMenuWraper { applicationId = app.Id, menuData = getMenudata(app.Id) });
            }
            List<mdlMenu> getMenudata(int? MenuId)
            {   
                List<mdlMenu> mdlMs = new List<mdlMenu>();
                mdlMs.AddRange(modules.Where(q=> (int?)q.EnmApplication== MenuId).OrderBy(q=>q.DisplayOrder).Select(p => new mdlMenu { id=p.Id , icon_url= p.Icon,urll="#", text=p.Name, sortingorder=p.DisplayOrder,children=new List<mdlMenu>()}));
                foreach (var mdlM in mdlMs)
                {
                    //check for sub module
                    var mdlsubMs = submodules.Where(q => (int?)q.EnmModule == mdlM.id).Select(p => new mdlMenu { id = p.Id, icon_url = p.Icon, urll = "#", text = p.Name, sortingorder = p.DisplayOrder, children = new List<mdlMenu>() }).ToList();
                    foreach (var mdlsubM in mdlsubMs)
                    {   
                        mdlsubM.children.AddRange( documents.Where(q => (int?)q.EnmSubModule == mdlsubM.id && q.DocumentType.HasFlag(enmDocumentType.DisplayMenu)).Select(p => new mdlMenu { id = p.Id, icon_url = p.Icon, urll = p.ActionName??"#", text = p.Name, sortingorder = p.DisplayOrder, children = new List<mdlMenu>() }));
                    }
                    var document_inner1 = documents.Where(q => (int?)q.EnmModule == mdlM.id && q.EnmSubModule==null && q.DocumentType.HasFlag(enmDocumentType.DisplayMenu)).Select(p => new mdlMenu { id = p.Id, icon_url = p.Icon, urll = p.ActionName??"#", text = p.Name, sortingorder = p.DisplayOrder, children = new List<mdlMenu>() }).ToList();
                    if (document_inner1 == null)
                    {
                        mdlM.children.AddRange(mdlsubMs.OrderBy(p => p.sortingorder).ThenBy(p => p.text));
                    }
                    else
                    {
                        mdlM.children.AddRange(mdlsubMs.Union(document_inner1).OrderBy(p => p.sortingorder).ThenBy(p => p.text));
                    }
                        
                }
                var document_inner2 = documents.Where(q => q.EnmModule == null && q.EnmSubModule == null && (int?)q.EnmApplication== MenuId &&  q.DocumentType.HasFlag(enmDocumentType.DisplayMenu)).Select(p => new mdlMenu { id = p.Id, icon_url = p.Icon, urll = p.ActionName ?? "#", text = p.Name, sortingorder = p.DisplayOrder, children = new List<mdlMenu>()}).ToList();
                if (document_inner2 == null)
                {
                    return mdlMs.OrderBy(p => p.sortingorder).ThenBy(p => p.text).ToList();
                }
                else
                {
                    return mdlMs.Union(document_inner2).OrderBy(p => p.sortingorder).ThenBy(p => p.text).ToList();
                }
                    
            }
            mdl.ReturnId=new { _document= documents, _module = modules ,_submodule=submodules, _application = applications, _muenuList= menuWraper };
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
                isrvEmployee.EmpId = _IsrvCurrentUser.EmployeeId;
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

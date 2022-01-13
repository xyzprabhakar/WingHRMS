﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projAPI.Model;
using projAPI.Services;
using projContext;
using projContext.DB.CRM;
using projContext.DB.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : Controller
    {
        
        private readonly IsrvCustomer _srvCustomers;
        private readonly MasterContext _masterContext;
        private readonly IsrvCurrentUser _srvCurrentUser;
        private readonly CrmContext _crmContext;

        public CustomerController(IsrvCustomer srvCustomers, MasterContext mc, CrmContext crmContext, IsrvCurrentUser srvCurrentUser)
        {
            _srvCustomers= srvCustomers;
            _masterContext = mc;
            _crmContext = crmContext;
            _srvCurrentUser = srvCurrentUser;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[Route("GetCustomers/{OrgId}/{PageId}/{PageSize}")]
        //[Authorize(policy:nameof(enmValidateRequestHeader.ValidateOrganisation))]
        //public mdlReturnData GetCustomers([FromHeader]int OrgId,int PageId,int PageSize)
        //{
        //    mdlReturnData returnData = new mdlReturnData();            
        //    returnData.ReturnId = _srvCustomers.GetCustomers();
        //    returnData.MessageType = enmMessageType.Success;
        //    return returnData;
        //}


        [HttpGet]
        [Route("GetCustomer/{CustomerId}/{IncludeCountryState}/{IncludeUsername}")]
        [Authorize(nameof(enmDocumentMaster.CRM_Create_Customer) + nameof(enmDocumentType.Update))]
        [Authorize(nameof(enmValidateRequestHeader.ValidateOrganisation))]
        public mdlReturnData GetCompany( [FromServices] IsrvMasters _srvMasters, [FromHeader]int OrgId 
            ,int CustomerId)
        {
            mdlReturnData returnData = new mdlReturnData();
            var tempData = CustomerId > 0 ? _crmContext.tblCustomerMaster.Where(p => p.CustomerId == CustomerId).FirstOrDefault() : null;
            if (tempData == null)
            {
                tempData = new tblCustomerMaster();
            }
            
            if (tempData.Logo != null)
            {
                var tempImages = _srvMasters.GetImage(tempData.Logo);
                if (tempImages != null)
                {
                    tempData.LogoImage = Convert.ToBase64String(tempImages.File);
                    tempData.LogoImageType = tempImages.FileType.GetDescription();
                }
            }
            returnData.MessageType = enmMessageType.Success;
            returnData.ReturnId = tempData;
            return returnData;
        }

        [HttpPost]
        [Route("SetCustomer")]
        [Authorize(nameof(enmDocumentMaster.CRM_Create_Customer) + nameof(enmDocumentType.Update))]
        [Authorize(nameof(enmValidateRequestHeader.ValidateOrganisation))]
        public mdlReturnData SetCustomer([FromServices] IsrvMasters _srvMasters, [FromServices] IsrvUsers srvUsers,
            [FromForm] mdlCustomer mdl, [FromHeader] int OrgId)
        {
            bool IsUpdate = false;
            mdlReturnData returnData = new mdlReturnData();
            if (mdl == null)
            {
                returnData.Message = "Invalid Data";
                returnData.MessageType = enmMessageType.Error;
                return returnData;
            }
            if (mdl.CustomerId > 0)
            {
                IsUpdate = true;
            }
            
            if (!(mdl.LogoImageFile == null))
            {
                mdl.NewFileName = _srvMasters.SetImage(mdl.LogoImageFile, enmFileType.ImageICO, _srvCurrentUser.UserId);
                mdl.Logo = mdl.NewFileName;
            }
            mdl.ModifiedBy = _srvCurrentUser.UserId;
            mdl.ModifiedDt = DateTime.Now;
            if (!IsUpdate)
            {
                mdl.OrgId = OrgId;
                mdl.Code = _srvMasters.GenrateCode(genrationType: enmCodeGenrationType.Customer, Prefix: "CST", OrgId: OrgId, UserId: _srvCurrentUser.UserId);
            }
            returnData= _srvCustomers.SetCustomerMaster(mdl, _srvCurrentUser.UserId);
            if (returnData.MessageType != enmMessageType.Success)
            {
                return returnData;
            }
            int CustomerId = returnData.ReturnId;
            ulong UserId = 0;
            string EncPassword = mdl.ChangePassword? Classes.AESEncrytDecry.EncryptStringAES(mdl.Password): mdl.Password;
            returnData = srvUsers.SetUserMaster(UserId, mdl.Name.ToUpper(), mdl.Code.ToUpper(), mdl.Email, mdl.ContactNo, EncPassword, enmUserType.Customer, OrgId, 0, 0, CustomerId, 0);
            if (returnData.MessageType != enmMessageType.Success)
            {
                return returnData;
            }
            UserId = returnData.ReturnId;
            List<int> CustomerRole = new List<int>();
            CustomerRole.Add((int)enmDefaultRole.CustomerAdmin);
            srvUsers.SetUserRole(UserId , CustomerRole,false,_srvCurrentUser.UserId);

            returnData.MessageType = enmMessageType.Success;
            returnData.Message = "Save successfully";
            return returnData;
        }


        [Route("GetCompanys/{OrgId}")]
        public mdlReturnData GetCompanys([FromServices] IsrvUsers srvUsers, int OrgId)
        {
            mdlReturnData returnData = new mdlReturnData();
            if (_masterContext.tblUserOrganisationPermission.Where(p => p.OrgId == OrgId && !p.IsDeleted).Count() == 0)
            {
                returnData.Message = "Unauthorize access";
                returnData.MessageType = enmMessageType.Error;
                return returnData;
            }
            returnData.ReturnId = (from t1 in _masterContext.tblCompanyMaster
                                   join t2 in _masterContext.tblCountry on t1.CountryId equals t2.CountryId
                                   join t3 in _masterContext.tblState on t1.StateId equals t3.StateId
                                   join t4 in _masterContext.tblUsersMaster on t1.ModifiedBy equals t4.UserId
                                   where t1.OrgId == OrgId
                                   select new
                                   {
                                       t1.CompanyId,
                                       t1.Code,
                                       t1.Name,
                                       t1.OfficeAddress,
                                       t1.Locality,
                                       t1.City,
                                       StateName = t3.Name,
                                       CountryName = t2.Name,
                                       t1.Pincode,
                                       t1.ContactNo,
                                       t1.AlternateContactNo,
                                       t1.Email,
                                       t1.AlternateEmail,
                                       t1.ModifiedDt,
                                       t4.UserName,
                                       t1.IsActive
                                   }).AsEnumerable().Select(p => new {
                                       companyId = p.CompanyId,
                                       code = p.Code,
                                       name = p.Name,
                                       address = string.Concat(p.OfficeAddress + ", " + p.Locality ?? string.Empty + ", " + p.City ?? string.Empty),
                                       state = p.StateName,
                                       country = p.CountryName,
                                       pincode = p.Pincode,
                                       contactNo = string.Concat(p.ContactNo, string.Concat(", ", p.AlternateContactNo) ?? string.Empty),
                                       email = string.Concat(p.Email, string.Concat(", ", p.AlternateEmail) ?? string.Empty),
                                       modifiedDt = p.ModifiedDt,
                                       modifiedBy = p.UserName,
                                       isActive = p.IsActive
                                   });
            returnData.MessageType = enmMessageType.Success;
            return returnData;
        }



    }




}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using projAPI.Model;
using projAPI.Services;
using projContext;
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
        public IActionResult Index()
        {
            return View();
        }
        private readonly IsrvCustomer _srvCustomers;
        private readonly MasterContext _masterContext;

        public CustomerController(IsrvCustomer srvCustomers, MasterContext mc)
        {
            _srvCustomers= srvCustomers;
            _masterContext = mc;
        }
        
        [Route("GetCustomers/{OrgId}")]
        [Authorize(policy:nameof(enmValidateRequestHeader.ValidateOrganisation))]
        public mdlReturnData GetCustomers([FromHeader]int OrgId)
        {
            mdlReturnData returnData = new mdlReturnData();            
            returnData.ReturnId = _srvCustomers.GetCustomers(OrgId);
            returnData.MessageType = enmMessageType.Success;
            return returnData;
        }


        [HttpGet]
        [Route("GetCustomer/{CustomerId}/{IncludeCountryState}/{IncludeUsername}")]
        [Authorize(nameof(enmDocumentMaster.Company) + nameof(enmDocumentType.Create))]
        [Authorize(nameof(enmValidateRequestHeader.ValidateOrganisation))]
        public mdlReturnData GetCompany([FromServices]IsrvUsers srvUsers, [FromServices] IsrvMasters _srvMasters, [FromHeader]int OrgId ,int CompanyId,
            bool IncludeCountryState, bool IncludeUsername)
        {
            mdlReturnData returnData = new mdlReturnData();
            
            var tempData = CompanyId > 0 ? _masterContext.tblCompanyMaster.Where(p => p.CompanyId == CompanyId).FirstOrDefault() : null;
            if (tempData == null)
            {
                tempData = new tblCompanyMaster();
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
        [Route("SetCompany")]
        [Authorize(nameof(enmDocumentMaster.Company) + nameof(enmDocumentType.Update))]
        public mdlReturnData SetCompany([FromServices] IsrvUsers srvUsers, [FromForm] tblCompanyWraper mdl)
        {
            mdlReturnData returnData = new mdlReturnData();
            if (mdl == null)
            {
                returnData.Message = "Invalid Data";
                returnData.MessageType = enmMessageType.Error;
                return returnData;
            }
            if (mdl.CompanyId > 0)
            {
                if (srvUsers.GetUserCompany(_srvCurrentUser.UserId, null, null).Where(p => p.Id == mdl.CompanyId).Count() == 0)
                {
                    returnData.Message = "Unauthorize access";
                    returnData.MessageType = enmMessageType.Error;
                    return returnData;
                }
            }
            if (_masterContext.tblCompanyMaster.Where(p => p.Code == mdl.Code && p.OrgId == mdl.OrgId && p.CompanyId != mdl.CompanyId).Count() > 0)
            {
                returnData.Message = "Code already exists";
                returnData.MessageType = enmMessageType.Error;
                return returnData;
            }
            if (mdl.OrgId > 0 && mdl.OrgId != _srvCurrentUser.OrgId && _srvCurrentUser.OrgId != 1)
            {
                returnData.MessageType = enmMessageType.Error;
                returnData.Message = "Invalid Organisation";
                return returnData;
            }
            else if (mdl.OrgId == 0 && _srvCurrentUser.OrgId != 1)
            {
                returnData.MessageType = enmMessageType.Error;
                returnData.Message = "Unauthorized Access";
                return returnData;
            }
            string FileName = null;
            if (!(mdl.LogoImageFile == null))
            {
                FileName = _srvMasters.SetImage(mdl.LogoImageFile, enmFileType.ImageICO, _srvCurrentUser.UserId);
                mdl.Logo = FileName;
            }
            mdl.ModifiedBy = _srvCurrentUser.UserId;
            mdl.ModifiedDt = DateTime.Now;
            if (mdl.OrgId == 0)
            {
                _masterContext.tblCompanyMaster.Add(mdl);
                mdl.CreatedBy = mdl.ModifiedBy.Value;
                mdl.CreatedDt = mdl.ModifiedDt.Value;
            }
            else
            {
                _masterContext.tblCompanyMaster.Update(mdl);
            }
            _masterContext.SaveChanges();
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

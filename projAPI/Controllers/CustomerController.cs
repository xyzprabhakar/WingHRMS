using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [Route("CheckEmail")]
        public bool CheckEmail([FromServices] IsrvMasters _srvMasters, int OrgId
            , string  txtEmail, int CustomerId)
        {
            mdlReturnData returnData = new mdlReturnData();
            if (_srvCustomers.CustomerEmailExists(txtEmail, CustomerId, OrgId))
            {
                return false;
            }
            else
            {
                return true;
                
            }
            //return returnData;
        }
        [HttpGet]
        [Route("CheckCustomerCode")]
        public bool CheckCustomerCode([FromServices] IsrvMasters _srvMasters, int OrgId
            , string txtEmail, string txtCustomerCodes)
        {
            mdlReturnData returnData = new mdlReturnData();
            var customer=txtCustomerCodes.Split(",").ToList();
            var cst = _srvCustomers.GetCustomers(OrgId, customer);
            foreach (var c in customer)
            {
                if (!cst.Any(p => p.Code.Trim().ToLower() == c.Trim().ToLower()))
                {
                    return false;
                }
                    
            }
            return true;
        }
        [HttpGet]
        [Route("CheckContact")]
        public bool CheckContact([FromServices] IsrvMasters _srvMasters, int OrgId
            , string txtContact, int CustomerId)
        {
            mdlReturnData returnData = new mdlReturnData();
            if (_srvCustomers.CustomerContactNoExists(txtContact, CustomerId,OrgId))
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }

        [HttpGet]
        [Route("GetCustomer/{CustomerId}/{IncludeCountryState}/{IncludeUsername}")]
        [Authorize(nameof(enmDocumentMaster.CRM_Create_Customer) + nameof(enmDocumentType.Update))]
        [Authorize(nameof(enmValidateRequestHeader.ValidateOrganisation))]
        public mdlReturnData GetCompany( [FromServices] IsrvMasters _srvMasters, [FromHeader]int OrgId 
            ,int CustomerId)
        {
            mdlReturnData returnData = new mdlReturnData();
            mdlCustomer mdl = new mdlCustomer();
            tblCustomerMaster tempData = CustomerId > 0 ? _crmContext.tblCustomerMaster.Where(p => p.CustomerId == CustomerId).FirstOrDefault() : null;
            if (tempData != null)
            {
                mdl = new mdlCustomer(tempData);
                var tempuserData=_masterContext.tblUsersMaster.Where(p => p.OrgId == OrgId && p.UserType == enmUserType.Customer && p.UserName == tempData.Code).FirstOrDefault();
                if (tempuserData != null)
                {
                    mdl.Password = tempuserData.Password;
                    mdl.UserId = tempuserData.UserId;
                }
                
            }
            
            
            if (mdl.Logo != null)
            {
                var tempImages = _srvMasters.GetImage(mdl.Logo);
                if (tempImages != null)
                {
                    mdl.LogoImage = Convert.ToBase64String(tempImages.File);
                    mdl.LogoImageType = tempImages.FileType.GetDescription();
                }
            }
            returnData.MessageType = enmMessageType.Success;
            returnData.ReturnId = mdl;
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
            ulong UserId = mdl.UserId;
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

        [HttpPost]
        [Route("GetCustomers")]
        [Authorize(nameof(enmValidateRequestHeader.ValidateOrganisation))]
        public mdlReturnData GetCustomers([FromServices] IsrvMasters srvMasters,[FromServices] IsrvUsers srvUsers,DataTableParameters dtp , [FromHeader]int OrgId )
        {
            
            mdlReturnData returnData = new mdlReturnData();
            try {
                IQueryable<tblCustomerMaster> CustomerData = _crmContext.tblCustomerMaster.Where(p => p.OrgId == OrgId).AsQueryable();
                int recordsTotal = _crmContext.tblCustomerMaster.Where(p => p.OrgId == OrgId).Count();
                int recordsFiltered = recordsTotal;
                if (dtp.search != null)
                {
                    if (dtp.search.value.Contains("@"))
                    {
                        CustomerData = CustomerData.Where(p => p.Email.Contains(dtp.search.value) ||
                                            p.AlternateEmail.Contains(dtp.search.value));
                    }
                    else
                    {
                        CustomerData = CustomerData.Where(p => p.Code.Contains(dtp.search.value) ||
                       p.Name.Contains(dtp.search.value) ||
                       p.OfficeAddress.Contains(dtp.search.value) ||
                       p.Locality.Contains(dtp.search.value) ||
                       p.ContactNo.Contains(dtp.search.value) ||
                       p.AlternateContactNo.Contains(dtp.search.value) 
                       );
                    }
                    
                }
                if (!(dtp.order == null || dtp.order.Count() == 0))
                {
                    if (dtp.order[0].dir == "asc")
                    {
                        switch (dtp.order[0].column)
                        {
                            //case 0: CustomerData = CustomerData.OrderBy(p => EF.Property<object>(p, "Id"));break;
                            case 0: CustomerData = CustomerData.OrderBy(p => p.CustomerId); break;
                            case 1: CustomerData = CustomerData.OrderBy(p => p.Code); break;
                            case 2: CustomerData = CustomerData.OrderBy(p => p.Name); break;
                            case 3: CustomerData = CustomerData.OrderBy(p => p.CustomerType); break;
                            case 4: CustomerData = CustomerData.OrderBy(p => p.IsActive); break;
                            case 5: CustomerData = CustomerData.OrderBy(p => p.OfficeAddress); break;
                            case 6: CustomerData = CustomerData.OrderBy(p => p.StateId); break;
                            case 8: CustomerData = CustomerData.OrderBy(p => p.Pincode); break;
                            case 9: CustomerData = CustomerData.OrderBy(p => p.ContactNo); break;
                            case 10: CustomerData = CustomerData.OrderBy(p => p.Email); break;
                            case 11: CustomerData = CustomerData.OrderBy(p => p.ModifiedBy); break;
                            case 12: CustomerData = CustomerData.OrderBy(p => p.ModifiedDt); break;
                        }
                    }
                    else
                    {
                        switch (dtp.order[0].column)
                        {
                            //case 0: CustomerData = CustomerData.OrderBy(p => EF.Property<object>(p, "Id"));break;
                            case 0: CustomerData = CustomerData.OrderByDescending(p => p.CustomerId); break;
                            case 1: CustomerData = CustomerData.OrderByDescending(p => p.Code); break;
                            case 2: CustomerData = CustomerData.OrderByDescending(p => p.Name); break;
                            case 3: CustomerData = CustomerData.OrderByDescending(p => p.CustomerType); break;
                            case 4: CustomerData = CustomerData.OrderByDescending(p => p.IsActive); break;
                            case 5: CustomerData = CustomerData.OrderByDescending(p => p.OfficeAddress); break;
                            case 6: CustomerData = CustomerData.OrderByDescending(p => p.StateId); break;
                            case 8: CustomerData = CustomerData.OrderByDescending(p => p.Pincode); break;
                            case 9: CustomerData = CustomerData.OrderByDescending(p => p.ContactNo); break;
                            case 10: CustomerData = CustomerData.OrderByDescending(p => p.Email); break;
                            case 11: CustomerData = CustomerData.OrderByDescending(p => p.ModifiedBy); break;
                            case 12: CustomerData = CustomerData.OrderByDescending(p => p.ModifiedDt); break;
                        }
                    }
                }

                if (dtp.length > 0)
                {
                    CustomerData = CustomerData.Skip(dtp.start).Take(dtp.length);
                }

                var data = CustomerData.ToList();
                var CountryList = data.Select(p => p.CountryId).ToArray();
                var StateList = data.Select(p => p.StateId).ToArray();
                var userList = data.Select(p => p.ModifiedBy ?? 0).ToArray();
                var AllCountryName = srvMasters.GetCountry(CountryList);
                var AllStateName = srvMasters.GetStates(StateList);
                var AllUserList = srvUsers.GetUsers(userList);
                data.ForEach(p =>
                {
                    var tempCountry = AllCountryName.Where(q => q.Id == p.CountryId).FirstOrDefault();
                    p.CountryName = String.Concat(tempCountry?.Code ?? "", " - ", tempCountry?.Name ?? "");
                    var tempState = AllStateName.Where(q => q.Id == p.StateId).FirstOrDefault();
                    p.StateName = String.Concat(tempState?.Code ?? "", " - ", tempState?.Name ?? "");
                    var tempUserName = AllUserList.Where(q => q.Id == p.ModifiedBy).FirstOrDefault();
                    p.ModifiedByName = String.Concat(tempUserName?.Name ?? "");
                });

                var jsonData = new
                {
                    draw = dtp.draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = data.AsEnumerable().Select((p, iterator) =>
                    new
                    {
                    Sno= iterator+1,
                    customerId = p.CustomerId,
                    code = p.Code,
                    name = p.Name,
                    customerType = p.CustomerType,
                    address = string.Concat(p.OfficeAddress + ", " + p.Locality ?? string.Empty + ", " + p.City ?? string.Empty),
                    state = p.StateName,
                    country = p.CountryName,
                    pincode = p.Pincode,
                    contactNo = string.Concat(p.ContactNo, string.Concat(", ", p.AlternateContactNo) ?? string.Empty),
                    email = string.Concat(p.Email, string.Concat(", ", p.AlternateEmail) ?? string.Empty),
                    modifiedDt = p.ModifiedDt,
                    modifiedBy = p.ModifiedByName,
                    isActive = p.IsActive
                    }
                    )
                };
                returnData.ReturnId = jsonData;
                returnData.MessageType = enmMessageType.Success;
                return returnData;
            }
            catch (Exception ex)
            {
                returnData.Message = ex.Message;
                returnData.MessageType = enmMessageType.Error;
                return returnData;
            }

            
        }



    }




}

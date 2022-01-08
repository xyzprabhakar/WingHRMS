using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projContext.DB;
using projContext;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.ComponentModel;
using static projContext.CommonClass;
using Microsoft.AspNetCore.Authorization;
using projAPI.Model;
using projContext.DB.Masters;
using projAPI.Services;

namespace projAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MastersController : Controller
    {
        private readonly MasterContext _masterContext;
        private readonly IsrvCurrentUser _srvCurrentUser;
        
        private readonly IsrvMasters _srvMasters;
        public MastersController([FromServices] IsrvMasters srvMasters ,IsrvCurrentUser isrvCurrentUser, MasterContext mc)
        {
            _srvMasters = srvMasters;
            _srvCurrentUser = isrvCurrentUser;
            _masterContext = mc;
        }

        [HttpGet]
        [Route("GetOrganisation/{IncludeCountryState}/{IncludeUsername}")]
        public mdlReturnData GetOrganisation([FromServices] IsrvUsers srvUsers,
            bool IncludeCountryState, bool IncludeUsername)
        {   
            mdlReturnData returnData = new mdlReturnData();
            int OrgId = _srvCurrentUser.OrgId;
            var tempData = _masterContext.tblOrganisation.Where(p=>p.OrgId==OrgId).FirstOrDefault();
            if (tempData == null)
            {
                tempData = new tblOrganisation();
            }
            if (IncludeCountryState)
            {
                tempData.StateName = _srvMasters.GetState(tempData.StateId)?.Name;
                tempData.CountryName = _srvMasters.GetCountry(tempData.CountryId)?.Name;
            }
            if (IncludeUsername)
            {
                tempData.ModifiedByName = srvUsers.GetUser(tempData.ModifiedBy)?.Name;
            }
            if (tempData.Logo != null)
            {
                var tempImages=_srvMasters.GetImage(tempData.Logo);
                if (tempImages != null)
                {
                    tempData.LogoImage =Convert.ToBase64String( tempImages.File);
                    tempData.LogoImageType = tempImages.FileType.GetDescription();
                }
            }
            returnData.MessageType = enmMessageType.Success;
            returnData.ReturnId = tempData;
            return returnData;
        }
        [HttpPost]
        [Route("SetOrganisation")]
        [Authorize(nameof(enmDocumentMaster.Organisation)+nameof(enmDocumentType.Update))]        
        public mdlReturnData SetOrganisation([FromForm]tblOrganisationWraper mdl)
         {
            mdlReturnData returnData = new mdlReturnData();
            if (mdl == null)
            {
                returnData.Message = "Invalid Data";
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
                _masterContext.tblOrganisation.Add(mdl);
                mdl.CreatedBy = mdl.ModifiedBy.Value;
                mdl.CreatedDt = mdl.ModifiedDt.Value;
            }
            else
            {
                _masterContext.tblOrganisation.Update(mdl);
            }
            _masterContext.SaveChanges();
            returnData.MessageType = enmMessageType.Success;
            returnData.Message = "Save successfully";
            return returnData;
        }


        [HttpGet]
        [Route("GetCompany/{CompanyId}/{IncludeCountryState}/{IncludeUsername}")]
        [Authorize(nameof(enmDocumentMaster.Company) + nameof(enmDocumentType.Create))]
        public mdlReturnData GetCompany([FromServices] IsrvUsers srvUsers,int CompanyId,
            bool IncludeCountryState, bool IncludeUsername)
        {
            mdlReturnData returnData = new mdlReturnData();
            if (CompanyId > 0)
            {
                if (srvUsers.GetUserCompany(_srvCurrentUser.UserId, null, null).Where(p => p.Id == CompanyId).Count() == 0)
                {
                    returnData.Message = "Unauthorize access";
                    returnData.MessageType = enmMessageType.Error;
                    return returnData;
                }
            }
            
            var tempData = CompanyId>0? _masterContext.tblCompanyMaster.Where(p => p.CompanyId == CompanyId).FirstOrDefault():null;
            if (tempData == null)
            {
                tempData = new tblCompanyMaster();
            }
            if (IncludeCountryState)
            {
                tempData.StateName = _srvMasters.GetState(tempData.StateId)?.Name;
                tempData.CountryName = _srvMasters.GetCountry(tempData.CountryId)?.Name;
            }
            if (IncludeUsername)
            {
                tempData.ModifiedByName = srvUsers.GetUser(tempData.ModifiedBy)?.Name;
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
        public mdlReturnData SetCompany([FromServices] IsrvUsers srvUsers,[FromForm] tblCompanyWraper mdl)
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
            returnData.ReturnId=( from t1 in _masterContext.tblCompanyMaster
            join t2 in _masterContext.tblCountry on t1.CountryId equals t2.CountryId
            join t3 in _masterContext.tblState on t1.StateId equals t3.StateId
            join t4 in _masterContext.tblUsersMaster on t1.ModifiedBy equals t4.UserId
            where t1.OrgId == OrgId
            select new { t1.CompanyId, t1.Code, t1.Name,t1.OfficeAddress,t1.Locality,t1.City,
            StateName=t3.Name,CountryName =t2.Name,t1.Pincode,t1.ContactNo,t1.AlternateContactNo,
            t1.Email,t1.AlternateEmail,t1.ModifiedDt, t4.UserName,t1.IsActive
            }).AsEnumerable().Select(p=>new {
                companyId = p.CompanyId,
                code = p.Code,
                name = p.Name,
                address = string.Concat( p.OfficeAddress + ", " +  p.Locality??string.Empty + ", " + p.City??string.Empty),
                state = p.StateName,
                country = p.CountryName,
                pincode = p.Pincode,
                contactNo = string.Concat( p.ContactNo,string.Concat( ", " , p.AlternateContactNo)??string.Empty),
                email = string.Concat(p.Email, string.Concat(", ", p.AlternateEmail) ?? string.Empty),
                modifiedDt = p.ModifiedDt,
                modifiedBy = p.UserName,
                isActive = p.IsActive
            });
            returnData.MessageType = enmMessageType.Success;
            return returnData;
        }



        [HttpGet]
        [Route("GetZone/{ZoneId}/{IncludeCountryState}/{IncludeUsername}")]
        [Authorize(nameof(enmDocumentMaster.Zone) + nameof(enmDocumentType.Create))]
        public mdlReturnData GetZone([FromServices] IsrvUsers srvUsers, int ZoneId,
            bool IncludeCountryState, bool IncludeUsername)
        {
            mdlReturnData returnData = new mdlReturnData();
            if (ZoneId > 0)
            {
                if (srvUsers.GetUserZone(_srvCurrentUser.UserId, null, null,null).Where(p => p.Id == ZoneId).Count() == 0)
                {
                    returnData.Message = "Unauthorize access";
                    returnData.MessageType = enmMessageType.Error;
                    return returnData;
                }
            }

            var tempData = ZoneId > 0 ? _masterContext.tblZoneMaster.Where(p => p.ZoneId == ZoneId).FirstOrDefault() : null;
            if (tempData == null)
            {
                tempData = new tblZoneMaster();
            }
            if (IncludeCountryState)
            {
                tempData.StateName = _srvMasters.GetState(tempData.StateId)?.Name;
                tempData.CountryName = _srvMasters.GetCountry(tempData.CountryId)?.Name;
            }
            if (IncludeUsername)
            {
                tempData.ModifiedByName = srvUsers.GetUser(tempData.ModifiedBy)?.Name;
            }
           
            returnData.MessageType = enmMessageType.Success;
            returnData.ReturnId = tempData;
            return returnData;
        }

        [HttpPost]
        [Route("SetZone")]
        [Authorize(nameof(enmDocumentMaster.Zone) + nameof(enmDocumentType.Update))]
        public mdlReturnData SetZone([FromServices] IsrvUsers srvUsers, [FromForm] tblZoneMaster mdl)
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
                if (srvUsers.GetUserZone(_srvCurrentUser.UserId, null, null,null).Where(p => p.Id == mdl.ZoneId).Count() == 0)
                {
                    returnData.Message = "Unauthorize access";
                    returnData.MessageType = enmMessageType.Error;
                    return returnData;
                }
            }
            if (_masterContext.tblZoneMaster.Where(p => p.Name == mdl.Name && p.OrgId == mdl.OrgId && p.ZoneId != mdl.ZoneId).Count() > 0)
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
            mdl.ModifiedBy = _srvCurrentUser.UserId;
            mdl.ModifiedDt = DateTime.Now;
            if (mdl.OrgId == 0)
            {
                _masterContext.tblZoneMaster.Add(mdl);
                mdl.CreatedBy = mdl.ModifiedBy.Value;
                mdl.CreatedDt = mdl.ModifiedDt.Value;
            }
            else
            {
                _masterContext.tblZoneMaster.Update(mdl);
            }
            _masterContext.SaveChanges();
            returnData.MessageType = enmMessageType.Success;
            returnData.Message = "Save successfully";
            return returnData;
        }


        [Route("GetZones/{OrgId}")]
        public mdlReturnData GetZones([FromServices] IsrvUsers srvUsers, int OrgId)
        {
            mdlReturnData returnData = new mdlReturnData();
            if (_masterContext.tblUserOrganisationPermission.Where(p => p.OrgId == OrgId && !p.IsDeleted).Count() == 0)
            {
                returnData.Message = "Unauthorize access";
                returnData.MessageType = enmMessageType.Error;
                return returnData;
            }
            returnData.ReturnId = (from t1 in _masterContext.tblCompanyMaster
                                   join t5 in _masterContext.tblZoneMaster on t1.CompanyId equals t5.ZoneId
                                   join t2 in _masterContext.tblCountry on t5.CountryId equals t2.CountryId
                                   join t3 in _masterContext.tblState on t5.StateId equals t3.StateId
                                   join t4 in _masterContext.tblUsersMaster on t5.ModifiedBy equals t4.UserId                                   
                                   where t1.OrgId == OrgId
                                   select new
                                   {
                                       t5.ZoneId,
                                       t1.Code,
                                       t1.Name,
                                       ZoneName=t5.Name,
                                       t5.OfficeAddress,
                                       t5.Locality,
                                       t5.City,
                                       StateName = t3.Name,
                                       CountryName = t2.Name,
                                       t5.Pincode,
                                       t5.ContactNo,
                                       t5.AlternateContactNo,
                                       t5.Email,
                                       t5.AlternateEmail,
                                       t5.ModifiedDt,
                                       t4.UserName,
                                       t5.IsActive
                                   }).AsEnumerable().Select(p => new {
                                       zoneId = p.ZoneId,
                                       companyName = string.Concat( p.Code," - ",p.Name),
                                       zoneName = p.ZoneName,
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


        [HttpGet]
        [Route("GetLocation/{LocationId}/{IncludeCountryState}/{IncludeUsername}")]
        [Authorize(nameof(enmDocumentMaster.Location) + nameof(enmDocumentType.Update))]
        public mdlReturnData GetLocation([FromServices] IsrvUsers srvUsers, int LocationId,
            bool IncludeCountryState, bool IncludeUsername)
        {
            mdlReturnData returnData = new mdlReturnData();
            if (LocationId > 0)
            {
                if (srvUsers.GetUserLocation(true,_srvCurrentUser.UserId, null, null, null).Where(p => p.Id ==LocationId).Count() == 0)
                {
                    returnData.Message = "Unauthorize access";
                    returnData.MessageType = enmMessageType.Error;
                    return returnData;
                }
            }

            var tempData = LocationId > 0 ? _masterContext.tblLocationMaster.Where(p => p.LocationId == LocationId).FirstOrDefault() : null;
            if (tempData == null)
            {
                tempData = new tblLocationMaster();
            }
            if (IncludeCountryState)
            {
                tempData.StateName = _srvMasters.GetState(tempData.StateId)?.Name;
                tempData.CountryName = _srvMasters.GetCountry(tempData.CountryId)?.Name;
            }
            if (IncludeUsername)
            {
                tempData.ModifiedByName = srvUsers.GetUser(tempData.ModifiedBy)?.Name;
            }
            tempData.CompanyId = _masterContext.tblZoneMaster.Where(p => p.ZoneId == tempData.ZoneId).FirstOrDefault()?.CompanyId ?? 0;
            returnData.MessageType = enmMessageType.Success;
            returnData.ReturnId = tempData;
            return returnData;
        }

        [HttpPost]
        [Route("SetLocation")]
        [Authorize(nameof(enmDocumentMaster.Location) + nameof(enmDocumentType.Update))]
        public mdlReturnData SetLocation([FromServices] IsrvUsers srvUsers, [FromForm] tblLocationMaster mdl)
        {
            mdlReturnData returnData = new mdlReturnData();
            if (mdl == null)
            {
                returnData.Message = "Invalid Data";
                returnData.MessageType = enmMessageType.Error;
                return returnData;
            }
            if (mdl.LocationId > 0)
            {
                if (srvUsers.GetUserLocation(true,_srvCurrentUser.UserId, null, null,null).Where(p => p.Id == mdl.LocationId).Count() == 0)
                {
                    returnData.Message = "Unauthorize access";
                    returnData.MessageType = enmMessageType.Error;
                    return returnData;
                }
            }
            if (_masterContext.tblLocationMaster.Where(p => p.Name == mdl.Name && p.OrgId == mdl.OrgId && p.LocationId != mdl.LocationId).Count() > 0)
            {
                returnData.Message = "Code already exists";
                returnData.MessageType = enmMessageType.Error;
                return returnData;
            }
            if (mdl.LocationId != 0)
            {
                var HeadOffice = (from t1 in _masterContext.tblLocationMaster
                                  join t2 in _masterContext.tblZoneMaster on t1.ZoneId equals t2.ZoneId
                                  where t2.CompanyId == mdl.CompanyId && t1.LocationType == enmLocationType.HeadOffice && t1.LocationId != mdl.LocationId
                                  select t1.LocationId
                                 ).Count();
                if (HeadOffice > 0)
                {
                    returnData.Message = "One HeadOffice can be define in a company";
                    returnData.MessageType = enmMessageType.Error;
                    return returnData;
                }
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
            mdl.ModifiedBy = _srvCurrentUser.UserId;
            mdl.ModifiedDt = DateTime.Now;
            if (mdl.OrgId == 0)
            {
                _masterContext.tblLocationMaster.Add(mdl);
                mdl.CreatedBy = mdl.ModifiedBy.Value;
                mdl.CreatedDt = mdl.ModifiedDt.Value;
            }
            else
            {
                _masterContext.tblLocationMaster.Update(mdl);
            }
            _masterContext.SaveChanges();
            returnData.MessageType = enmMessageType.Success;
            returnData.Message = "Save successfully";
            return returnData;
        }

        [Route("GetLocations/{OrgId}")]
        public mdlReturnData GetLocations([FromServices] IsrvUsers srvUsers, int OrgId)
        {
            mdlReturnData returnData = new mdlReturnData();
            if (_masterContext.tblUserOrganisationPermission.Where(p => p.OrgId == OrgId && !p.IsDeleted).Count() == 0)
            {
                returnData.Message = "Unauthorize access";
                returnData.MessageType = enmMessageType.Error;
                return returnData;
            }
            returnData.ReturnId = (from t1 in _masterContext.tblCompanyMaster
                                   join t5 in _masterContext.tblZoneMaster on t1.CompanyId equals t5.ZoneId
                                   join t6 in _masterContext.tblLocationMaster on t5.ZoneId equals t6.ZoneId
                                   join t2 in _masterContext.tblCountry on t6.CountryId equals t2.CountryId
                                   join t3 in _masterContext.tblState on t6.StateId equals t3.StateId
                                   join t4 in _masterContext.tblUsersMaster on t6.ModifiedBy equals t4.UserId
                                   where t1.OrgId == OrgId
                                   select new
                                   {
                                       t6.LocationId,
                                       CompanyName=t1.Code+" - "+ t1.Name,
                                       ZoneName = t5.Name,
                                       LocationName = t6.Name,
                                       t6.LocationType,
                                       t5.OfficeAddress,
                                       t5.Locality,
                                       t5.City,
                                       StateName = t3.Name,
                                       CountryName = t2.Name,
                                       t5.Pincode,
                                       t5.ContactNo,
                                       t5.AlternateContactNo,
                                       t5.Email,
                                       t5.AlternateEmail,
                                       t5.ModifiedDt,
                                       t4.UserName,
                                       t6.IsActive
                                   }).AsEnumerable().Select(p => new {
                                       locationId = p.LocationId,
                                       companyName = p.CompanyName,
                                       zoneName = p.ZoneName,
                                       locationName=p.LocationName,
                                       locationType=p.LocationType.GetDescription(),
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

        [AllowAnonymous]
        [Route("GetCountry/{IncludeUsername}")]
        public mdlReturnData GetCountry([FromServices] IsrvUsers srvUsers,bool IncludeUsername)
        {
            mdlReturnData returnData = new mdlReturnData();
            var AllCountry=_masterContext.tblCountry.ToList();
            if (IncludeUsername)
            {
                var tempUserIds=AllCountry.Select(p => p.ModifiedBy??0).Distinct().ToArray();
                var AllUsers=srvUsers.GetUsers(tempUserIds);
                AllCountry.ForEach(p => { p.Name = AllUsers.Where(q => q.Id == p.ModifiedBy).FirstOrDefault()?.Name; });
            }
            returnData.MessageType = enmMessageType.Success;
            returnData.ReturnId = AllCountry.OrderBy(p=>p.Name);
            return returnData;
        }
        [AllowAnonymous]
        [Route("GetState/{CountryId}/{AllStates}/{IncludeUsername}")]
        public mdlReturnData GetState([FromServices] IsrvUsers srvUsers,int CountryId,bool AllStates,  bool IncludeUsername)
        {
            mdlReturnData returnData = new mdlReturnData();
            List<tblState> State = new List<tblState>();
            if (AllStates)
            {
                State.AddRange(_masterContext.tblState);
            }
            else
            {
                State.AddRange(_masterContext.tblState.Where(p=>p.CountryId==CountryId));
            }
            if (IncludeUsername)
            {
                var tempUserIds = State.Select(p => p.ModifiedBy ?? 0).Distinct().ToArray();
                var AllUsers = srvUsers.GetUsers(tempUserIds);
                State.ForEach(p => { p.Name = AllUsers.Where(q => q.Id == p.ModifiedBy).FirstOrDefault()?.Name; });
            }
            returnData.MessageType = enmMessageType.Success;
            returnData.ReturnId = State.OrderBy(p=>p.Name);
            return returnData;
        }

    }

#if (false)
    [Route("api/[controller]")]
    [ApiController]
    public class MastresController : Controller
    {
        private readonly Context _context;
        private IHostingEnvironment _hostingEnvironment;

        public MastresController(Context context, IHostingEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }


        // POST: api/Masters

        [Route("Save_Location_Master")]
        [HttpPost]
        //[ActionName("Save_Location_Master")]////[Authorize]
        //[Authorize(Policy = "12001")]
        public async Task<IActionResult> Save_Location_Master([FromBody]tbl_location_master objlocation)
        {
            Response_Msg objResult = new Response_Msg();
            try
            {

                string[] replaceThese = { "data:image/png;base64,", "data:image/jpeg;base64,", "data:image/jpg;base64," };
                string data = objlocation.image;


                if (data != null && data != "")
                {
                    foreach (string curr in replaceThese)
                    {
                        data = data.Replace(curr, string.Empty);
                    }

                    byte[] imageBytes = System.Convert.FromBase64String(data);
                    string imageName = objlocation.location_name + ".jpg";

                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };

                    var ext = ".jpg"; //getting the extension
                    if (allowedExtensions.Contains(ext.ToLower())) //check what type of extension  
                    {
                        string name = Path.GetFileNameWithoutExtension(imageName); //getting file name without extension  
                        string MyFileName = name + ext;

                        var webRoot = _hostingEnvironment.WebRootPath;

                        if (!Directory.Exists(webRoot + "/UserFiles/" + objlocation.location_name + "/Image/"))
                        {
                            Directory.CreateDirectory(webRoot + "/UserFiles/" + objlocation.location_name + "/Image/");
                        }
                        //Delete User Files if exist
                        var dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/UserFiles/" + objlocation.location_name + "/Image/" + MyFileName);
                        Directory.Delete(dirPath, true);

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + "/UserFiles/" + objlocation.location_name + "/Image/", MyFileName);
                        System.IO.File.WriteAllBytes(path, imageBytes);//save image file
                        //update file name
                        objlocation.image = MyFileName;
                    }
                }
                //save data
                _context.tbl_location_master.Add(objlocation);
                await _context.SaveChangesAsync();

                objResult.StatusCode = 0;
                objResult.Message = "File Uploaded !!";
                return Ok(objResult);
            }

            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;
                return Ok(objResult);
            }
        }

        [Route("SaveCountry")]
        [HttpPost]
        //[Authorize(Policy = "12002")]
        public IActionResult SaveCountry(string CountryName, string ShortName)
        {

            Response_Msg objResult = new Response_Msg();
            try
            {
                var Exist = (from a in _context.tbl_country.Where(x => x.name == CountryName) select a).FirstOrDefault();

                if (Exist == null)
                {
                    //save
                    tbl_country objcountry = new tbl_country();
                    objcountry.name = CountryName;
                    objcountry.sort_name = ShortName;
                    _context.Entry(objcountry).State = EntityState.Added;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Country Added Successfully !";

                    return Ok(objResult);
                }
                else
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Country Name Already Exist !!";

                    return Ok(objResult);
                }
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;

                return Ok(objResult);
            }
        }

        [HttpPost]
        //[Authorize(Policy = "12002")]
        public IActionResult UpdateCountry(string CountryName, string ShortName, int CountryId)
        {

            Response_Msg objResult = new Response_Msg();
            try
            {
                var Exist = (from a in _context.tbl_country.Where(x => x.country_id == CountryId) select a).FirstOrDefault();

                if (Exist == null)
                {
                    objResult.StatusCode = 1;
                    objResult.Message = "Country does not exist, please check and try !!";

                    return Ok(objResult);
                }
                else
                {

                    Exist.name = CountryName;
                    Exist.sort_name = ShortName;
                    _context.tbl_country.Add(Exist).State = EntityState.Modified;
                    _context.SaveChanges();

                    objResult.StatusCode = 0;
                    objResult.Message = "Country Updated Successfully !";

                    return Ok(objResult);

                }
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;

                return Ok(objResult);
            }
        }

        //[Route("[GetCountryList]/{CountryId}")]
        //[HttpGet]
        //public async Task<IActionResult> GetCountryList([FromRoute] int CountryId)
        //{
        //    try
        //    {
        //        if (CountryId > 0)
        //        {
        //            var result = (from a in _context.tbl_country.Where(x => x.country_id == CountryId) select a).ToList();
        //            var data = new { data = result };
        //            return Ok(data);
        //        }
        //        else
        //        {
        //            var result = (from a in _context.tbl_country select a).ToList();
        //            var data = new { data = result };
        //            return Ok(data);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}

        //[Route("[GetCountryList]/{CountryId}")]

        [Route("GetCountryList/{CountryId}")]
        [HttpGet]
        ////[Authorize]
        //[Authorize(Policy = "12003")]
        public async Task<IActionResult> GetCountryList([FromBody] int CountryId)
        {
            try
            {
                if (CountryId > 0)
                {
                    var result = (from a in _context.tbl_country.Where(x => x.country_id == CountryId) select a).ToList();
                    var data = new { data = result };
                    return Ok(data);
                }
                else
                {
                    var result = (from a in _context.tbl_country select a).ToList();
                    var data = new { data = result };
                    return Ok(data);
                }


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }



        /*-------state master-----------*/
        #region-----state master------

        [HttpPost]
        //[Authorize(Policy ="12004")]
        //  [ValidateAntiForgeryToken]
        public IActionResult SaveAndUpdateState(int CountryId, string StateName, int StateId, string ActionMode)
        {

            Response_Msg objResult = new Response_Msg();
            try
            {
                //add new state
                if (ActionMode == "Add")
                {
                    var Exist = (from a in _context.tbl_state.Where(x => x.name == StateName) select a).FirstOrDefault();

                    if (Exist == null)
                    {
                        //save
                        tbl_state objstate = new tbl_state();
                        objstate.name = StateName;
                        objstate.country_id = CountryId;
                        _context.tbl_state.Add(objstate).State = EntityState.Added;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "State Added Successfully !";

                        return Ok(objResult);
                    }
                    else
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "State Name Already Exist !!";

                        return Ok(objResult);
                    }
                }
                else //update state data
                {
                    var Exist = (from a in _context.tbl_state.Where(x => x.state_id == StateId) select a).FirstOrDefault();

                    if (Exist == null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "State does not eixst, please check and try !!";

                        return Ok(objResult);

                    }
                    else
                    {
                        //check if same state name exist in same country more than 1
                        var StateCount = (from a in _context.tbl_state.Where(x => x.state_id != StateId && x.name == StateName && x.country_id == CountryId) select a).Count();
                        if (StateCount > 0)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "State name already exist in country, please check and try !!";

                            return Ok(objResult);
                        }
                        else
                        {

                            tbl_state objstate = new tbl_state();
                            Exist.name = StateName;
                            Exist.country_id = CountryId;
                            _context.tbl_state.Add(Exist).State = EntityState.Modified;
                            _context.SaveChanges();

                            objResult.StatusCode = 0;
                            objResult.Message = "State Updated Successfully !";

                            return Ok(objResult);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;

                return Ok(objResult);
            }
        }

        [Route("GetStateLists/{StateId}/{CountryId}")]
        [HttpGet]
        //[Authorize(Policy ="12005")]
        ////[Authorize]
        public async Task<IActionResult> GetStateList([FromRoute] int StateId, int CountryId)
        {
            try
            {

                if (StateId > 0 && CountryId == 0)
                {
                    var result = (from a in _context.tbl_state.Where(x => x.state_id == StateId) select a).ToList();
                    var data = new { data = result };
                    return Ok(data);
                }
                else if (CountryId > 0)
                {
                    var result = (from a in _context.tbl_state.Where(x => x.country_id == CountryId) select a).ToList();
                    var data = new { data = result };
                    return Ok(data);
                }
                else
                {
                    var result = (from a in _context.tbl_state
                                  join b in _context.tbl_country on a.country_id equals b.country_id
                                  select new
                                  {
                                      stateid = a.state_id,
                                      statename = a.name,
                                      countryname = b.name,
                                      shortname = b.sort_name.ToUpper()
                                  }).ToList();

                    var data = new { data = result };
                    return Ok(data);
                }


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        #endregion
        /*-------city master-----------*/
        #region ----------city master-------------

        [HttpPost]
        //[Authorize(Policy ="12006")]
        //  [ValidateAntiForgeryToken]
        public IActionResult SaveAndUpdateCity(string CityName, int CityId, int StateId, string ActionMode)
        {

            Response_Msg objResult = new Response_Msg();
            try
            {
                //add new city
                if (ActionMode == "Add")
                {
                    var Exist = (from a in _context.tbl_city.Where(x => x.name == CityName) select a).FirstOrDefault();

                    if (Exist == null)
                    {
                        //save
                        tbl_city objcity = new tbl_city();
                        objcity.name = CityName;
                        objcity.state_id = StateId;
                        _context.tbl_city.Add(objcity).State = EntityState.Added;
                        _context.SaveChanges();

                        objResult.StatusCode = 0;
                        objResult.Message = "City Added Successfully !";

                        return Ok(objResult);
                    }
                    else
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "City Name Already Exist !!";

                        return Ok(objResult);
                    }
                }
                else //update city data
                {
                    var Exist = (from a in _context.tbl_city.Where(x => x.city_id == CityId) select a).FirstOrDefault();

                    if (Exist == null)
                    {
                        objResult.StatusCode = 1;
                        objResult.Message = "City does not eixst, please check and try !!";

                        return Ok(objResult);

                    }
                    else
                    {
                        //check if same state name exist in same country more than 1
                        var CityCount = (from a in _context.tbl_city.Where(x => x.city_id != CityId && x.name == CityName && x.state_id == StateId) select a).Count();
                        if (CityCount > 0)
                        {
                            objResult.StatusCode = 1;
                            objResult.Message = "City name already exist in state, please check and try !!";

                            return Ok(objResult);
                        }
                        else
                        {
                            Exist.name = CityName;
                            Exist.state_id = StateId;
                            _context.tbl_city.Add(Exist).State = EntityState.Modified;
                            _context.SaveChanges();

                            objResult.StatusCode = 0;
                            objResult.Message = "City Updated Successfully !";

                            return Ok(objResult);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                objResult.StatusCode = 2;
                objResult.Message = ex.Message;

                return Ok(objResult);
            }
        }

        [HttpGet]
        //[Authorize(Policy ="12007")]
        public IActionResult GetCityList(int CityId, int StateId)
        {
            try
            {
                if (CityId > 0 && StateId == 0)
                {
                    var result = (from a in _context.tbl_city.Where(x => x.city_id == CityId) select a).ToList();
                    var data = new { data = result };
                    return Ok(data);
                }
                else if (StateId > 0)
                {
                    var result = (from a in _context.tbl_city.Where(x => x.state_id == StateId) select a).ToList();
                    var data = new { data = result };
                    return Ok(data);
                }
                else
                {
                    var result = (from a in _context.tbl_city
                                  join b in _context.tbl_state on a.state_id equals b.state_id
                                  join c in _context.tbl_country on
                                  b.country_id equals c.country_id
                                  select new
                                  {
                                      cityid = a.city_id,
                                      cityname = a.name,
                                      stateid = a.state_id,
                                      statename = b.name,
                                      countryname = c.name,
                                      shortname = c.sort_name.ToUpper()
                                  }).ToList();

                    var data = new { data = result };
                    return Ok(data);
                }


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        #endregion

        //get location list from enum
        // [Route("GetLocationType")]
        [HttpGet]
        //[Authorize(Policy = "12008")]
        [ActionName("GetLocationType")]
        public IActionResult GetLocationType()
        {
            try
            {
                List<object> list = new List<object>();
                foreach (LocationType location in Enum.GetValues(typeof(LocationType)))
                {
                    int value = (int)Enum.Parse(typeof(LocationType), Enum.GetName(typeof(LocationType), location));
                    // string Str = Enum.GetName(typeof(LocationType), value);

                    Type type = location.GetType();
                    MemberInfo[] memInfo = type.GetMember(location.ToString());

                    if (memInfo != null && memInfo.Length > 0)
                    {
                        object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (attrs != null && attrs.Length > 0)
                        {
                            string strvalue = ((DescriptionAttribute)attrs[0]).Description;
                            list.Add(new
                            {
                                LocationId = value,
                                LocationName = strvalue
                            });
                        }
                    }
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        //#region **CREATED BY SUPRIYA ON 10-01-2019, GUI GENERATE
        [Route("Generate_GuiId/{generate_by}")]
        [HttpGet]
        public IActionResult Generate_GuiId([FromRoute] int generate_by)
        {
            try
            {
                tbl_guid_detail objdtl = new tbl_guid_detail();
                //objdtl.genrated_by = generate_by;
                objdtl.genration_dt = DateTime.Now;

                _context.Entry(objdtl).State = EntityState.Added;
                _context.SaveChanges();
                return Ok("Successfully Generated");
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
        // #endregion ** END BY SUPRIYA ON 10-01-2019,GUI GENERATE


        
    }

#endif
    }
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using projAPI.Model;
using projAPI.Services;
using projContext;
using projContext.DB.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HrmsMasterController : ControllerBase
    {
        private readonly HRMSContext _HRMSContext;
        private readonly IsrvCurrentUser _IsrvCurrentUser;
        public HrmsMasterController(HRMSContext HrmsContext, IsrvCurrentUser IsrvCurrentUser)
        {
            _HRMSContext = HrmsContext;
            _IsrvCurrentUser = IsrvCurrentUser;
        }

        [HttpGet]
        [Authorize(nameof(enmDocumentMaster.Organisation) + nameof(enmDocumentType.Update))]
        [Route("GetHoliday/{OrgId}/{IncludeUsername}")]
        public mdlReturnData GetHoliday([FromHeader]int OrgId, int HolidayId)
        {
            mdlReturnData returnData = new mdlReturnData();
            if (!_IsrvCurrentUser.HaveOrganisationPermission(OrgId))
            {
                //_IsrvCurrentUser
            }
            //var tempData = _masterContext.tblOrganisation.Where(p => p.OrgId == OrgId).FirstOrDefault();
            //if (tempData == null)
            //{
            //    tempData = new tblOrganisation();
            //}
            //if (tempData.Logo != null)
            //{
            //    var tempImages = _srvMasters.GetImage(tempData.Logo);
            //    if (tempImages != null)
            //    {
            //        tempData.LogoImage = Convert.ToBase64String(tempImages.File);
            //        tempData.LogoImageType = tempImages.FileType.GetDescription();
            //    }
            //}
            //returnData.MessageType = enmMessageType.Success;
            //returnData.ReturnId = tempData;
            return returnData;
        }






    }
}

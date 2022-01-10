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
        public mdlReturnData GetCustomers(int OrgId)
        {
            mdlReturnData returnData = new mdlReturnData();
            if (_masterContext.tblUserOrganisationPermission.Where(p => p.OrgId == OrgId && !p.IsDeleted).Count() == 0)
            {
                returnData.Message = "Unauthorize access";
                returnData.MessageType = enmMessageType.Error;
                return returnData;
            }

            returnData.ReturnId = _srvCustomers.GetCustomers(OrgId);
            returnData.MessageType = enmMessageType.Success;
            return returnData;
        }


    }




}

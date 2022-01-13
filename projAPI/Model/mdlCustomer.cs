using Microsoft.AspNetCore.Http;
using projContext.DB.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model
{
    public class mdlCustomer: tblCustomerMaster
    {
        public IFormFile LogoImageFile { get; set; }
        public string NewFileName { get; set; }
        public bool HaveAllCompanyAccess { get; set; }
        public List<int> CompanyId { get; set;}
        public string Password { get; set; }


    }
}

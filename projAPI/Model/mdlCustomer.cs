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
        public mdlCustomer() { }
        public mdlCustomer(tblCustomerMaster mdl)
        {
            this.CustomerId = mdl.CustomerId;
            this.OrgId = mdl.OrgId;
            this.Code = mdl.Code;
            this.Name = mdl.Name;
            this.EffectiveFromDt = mdl.EffectiveFromDt;
            this.EffectiveToDt = mdl.EffectiveToDt;
            this.IsActive = mdl.IsActive;
            this.CustomerType = mdl.CustomerType;
            this.Logo = mdl.Logo;
            this.LogoImage = mdl.LogoImage;
            this.LogoImageType = mdl.LogoImageType;
            this.Email = mdl.Email;
            this.AlternateEmail = mdl.AlternateEmail;
            this.ContactNo = mdl.ContactNo;
            this.AlternateContactNo = mdl.AlternateContactNo;
            this.OfficeAddress = mdl.OfficeAddress;
            this.Locality = mdl.Locality;
            this.City = mdl.City;
            this.Pincode = mdl.Pincode;
            this.StateId = mdl.StateId;
            this.CountryId = mdl.CountryId;
            this.StateName = mdl.StateName;
            this.CountryName = mdl.CountryName;
            this.CreatedBy = mdl.CreatedBy;
            this.CreatedDt = mdl.CreatedDt;
            this.ModifyRemarks = mdl.ModifyRemarks;
            this.ModifiedBy = mdl.ModifiedBy;
            this.ModifiedDt = mdl.ModifiedDt;
            this.ModifiedByName = mdl.ModifiedByName;

        }

        public IFormFile LogoImageFile { get; set; }
        public string NewFileName { get; set; }
        public bool HaveAllCompanyAccess { get; set; }
        public List<int> CompanyId { get; set;}
        public string Password { get; set; }
        public bool ChangePassword { get; set; }
        public ulong UserId { get; set; }

    }
}

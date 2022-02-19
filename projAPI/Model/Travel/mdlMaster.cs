using Microsoft.AspNetCore.Http;
using projContext.DB.CRM.Travel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model.Travel
{
    public class mdlAirline_ : tblAirline
    {
        public mdlAirline_() { }
        public mdlAirline_(tblAirline mdl)
        {
            this.Id = mdl.Id;
            this.Code = mdl.Code;
            this.Name = mdl.Name;
            this.CreatedBy = mdl.CreatedBy;
            this.CreatedDt = mdl.CreatedDt;
            this.ModifyRemarks = mdl.ModifyRemarks;
            this.ModifiedBy = mdl.ModifiedBy;
            this.ModifiedDt = mdl.ModifiedDt;
            this.ModifiedByName = mdl.ModifiedByName;
            this.isLcc = mdl.isLcc;
            this.ImagePath = mdl.ImagePath;
            this.LogoImage = mdl.LogoImage;
            this.LogoImageType = mdl.LogoImageType;
        }

        public IFormFile LogoImageFile { get; set; }
        public string NewFileName { get; set; }
    }
}

using projAPI.Model;
using projAPI.Services;
using projContext;
using projContext.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Classes
{
    public class Organisation
    {
        
        public tblCustomerOrganisation GetOrganisation(Context context, string OrgCode)
        {
            return context.tblCustomerOrganisation.Where(p => p.OrganisationCode == OrgCode && p.IsActive).FirstOrDefault();
        }

        public mdlReturnData ValidateOrganisationForFlight(Context context, IsrvCurrentUser _IsrvCurrentUser,string OrgCode)
        {
            mdlReturnData mdl = new mdlReturnData() { MessageType = enmMessageType.Success };
            var OrgData = GetOrganisation(context, OrgCode);
            if (OrgData == null)
            {
                mdl.MessageType = enmMessageType.Error;
                mdl.Message = "Invalid company code";
                return mdl;
            }
            if (!(OrgData.CustomerId == 1 || OrgData.CustomerId == 2))
            {
                if (_IsrvCurrentUser.user_type != enmUserType.Employee)
                {
                    if (_IsrvCurrentUser.CustomerId != OrgData.CustomerId)
                    {
                        mdl.MessageType = enmMessageType.Error;
                        mdl.Message = "Unauthorize access";
                        return mdl;
                    }
                }
            }
            mdl.ReturnId = OrgData;

            return mdl;

        }
    }
}

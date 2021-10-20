using projAPI.Model;
using projContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Services
{
    public class srvUsers
    {
        private readonly Context _context;
        private readonly IsrvSettings _IsrvSettings;
        public srvUsers(Context context, IsrvSettings isrvSettings)
        {
            _context = context;
            _IsrvSettings =isrvSettings;
        }
        public mdlReturnData ValidateUser(string UserName,string Password,string OrgCode,enmUserType userType)
        {
            mdlReturnData ReturnData = new mdlReturnData() { MessageType = enmMessageType.None };
            if (userType.HasFlag(enmUserType.B2B) || userType.HasFlag(enmUserType.B2C))
             {
                if (string.IsNullOrEmpty(OrgCode))
                {
                    ReturnData.MessageType = enmMessageType.Error;
                    ReturnData.Message = "Invalid Organisation Code";
                    return ReturnData;
                }
                else
                {
                   var temOrgData= _context.tblCustomerOrganisation.Where(p => p.OrganisationCode == OrgCode).FirstOrDefault();
                    if (temOrgData == null)
                    {
                        ReturnData.MessageType = enmMessageType.Error;
                        ReturnData.Message = "Invalid Organisation Code";
                        return ReturnData;
                    }
                    if (!temOrgData.IsActive )
                    {
                        ReturnData.MessageType = enmMessageType.Error;
                        ReturnData.Message = "Blocked Organisation";
                        return ReturnData;
                    }
                }
             }
            
            

            var tempData=_context.tbl_user_master.Where(p => p.username == UserName && p.password == Password).FirstOrDefault();
            if (tempData == null)
            { 
            }
            return ReturnData;
        }


    }
}

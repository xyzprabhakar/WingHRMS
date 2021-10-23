using Microsoft.AspNetCore.Authorization;
using projContext;

namespace projAPI
{
    public class AccessRightRequirement : IAuthorizationRequirement
    {
        public enmDocumentMaster accessRight; 

        public AccessRightRequirement(enmDocumentMaster accessRight)
        {
            this.accessRight = accessRight;
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using projContext;

namespace projAPI
{
    public class AccessRightRequirement : IAuthorizationRequirement
    {
        public enmDocumentMaster accessRight;
        public enmDocumentType accessRightType;
        public AccessRightRequirement(enmDocumentMaster accessRight, enmDocumentType accessRightType)
        {
            this.accessRight = accessRight;
            this.accessRightType = accessRightType;
        }
    }
}
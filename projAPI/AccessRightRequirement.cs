using Microsoft.AspNetCore.Authorization;
using projContext;

namespace projAPI
{
    public class AccessRightRequirement : IAuthorizationRequirement
    {
        public bool IsValidateOrganisation;
        public enmDocumentMaster accessRight;
        public enmDocumentType accessRightType;
        public enmValidateRequestHeader RequestHeader;
        public AccessRightRequirement(enmDocumentMaster accessRight, enmDocumentType accessRightType)
        {
            IsValidateOrganisation = false;
            this.accessRight = accessRight;
            this.accessRightType = accessRightType;
        }
        public AccessRightRequirement(enmValidateRequestHeader RequestHeader)
        {
            IsValidateOrganisation = true;
            this.RequestHeader = RequestHeader;
        }
    }
}
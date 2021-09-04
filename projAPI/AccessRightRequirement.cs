using Microsoft.AspNetCore.Authorization;

namespace projAPI
{
    public class AccessRightRequirement : IAuthorizationRequirement
    {
        public string accessRight;       

        public AccessRightRequirement(string accessRight)
        {
            this.accessRight = accessRight;            
        }
    }
}
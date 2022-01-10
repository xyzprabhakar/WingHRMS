using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using projAPI.Classes;
using projContext;
using projContext.DB.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI
{
    public class AccessRightHandler : AuthorizationHandler<AccessRightRequirement>
    {
        private readonly MasterContext _Dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;        
        public AccessRightHandler(MasterContext Dbcontext, IHttpContextAccessor httpContextAccessor)
        {
            _Dbcontext = Dbcontext;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessRightRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "__UserId"))
            {
                ulong UserId = 0;
                ulong.TryParse(context.User.Claims.Where(p => p.Type == "__UserId").FirstOrDefault()?.Value,out UserId);
                if (UserId > 0)
                {
                    if (requirement.IsValidateOrganisation)
                    {
                        if (requirement.RequestHeader == enmValidateRequestHeader.ValidateOrganisation)
                        {
                            int OrgId = 0;
                            int.TryParse(_httpContextAccessor.HttpContext.Request.Headers.Where(p => string.Equals(p.Key, "OrgId", StringComparison.OrdinalIgnoreCase))?.FirstOrDefault().Value, out OrgId);
                            if (OrgId > 0)
                            {
                                string tempOrg = _httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__OrgId").FirstOrDefault()?.Value ?? string.Empty;
                                int[] _OrgIds = tempOrg.Split(",")?.Select(p => Convert.ToInt32(p))?.ToArray() ?? null;
                                if (_OrgIds.Any(p => p == OrgId))
                                {
                                    context.Succeed(requirement);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (_Dbcontext.tblUserRole.Where(p => p.UserId == UserId && p.tblRoleMaster.RoleName == "SuperAdmin" && !p.IsDeleted).Count() > 0)
                        {
                            context.Succeed(requirement);
                        }
                        else
                        {
                            var tempData = _Dbcontext.tblUserAllClaim.Where(p => p.DocumentMaster == requirement.accessRight && p.PermissionType == requirement.accessRightType).FirstOrDefault();
                            if (tempData != null)
                            {
                                context.Succeed(requirement);
                            }
                        }
                    }
                }
                
            }
            return Task.CompletedTask;
            
        }
    }
}

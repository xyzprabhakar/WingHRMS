using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using projAPI.Classes;
using projContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI
{
    public class AccessRightHandler : AuthorizationHandler<AccessRightRequirement>
    {
        private readonly Context _Dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly clsCurrentUser _clsCurrentUser;
        public AccessRightHandler(Context Dbcontext, IHttpContextAccessor httpContextAccessor, clsCurrentUser _clsCurrentUser)
        {
            _Dbcontext = Dbcontext;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this._clsCurrentUser = _clsCurrentUser;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessRightRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "__UserId"))
            {
                ulong UserId = 0;
                ulong.TryParse(context.User.Claims.Where(p => p.Type == "__UserId").FirstOrDefault()?.Value,out UserId);
                if (UserId > 0)
                {
                    if (_Dbcontext.tbl_user_role_map.Where(p => p.user_id == UserId && p.role_id == 1 && p.is_deleted == 0).Count() > 0)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        var tempData = (from t1 in _Dbcontext.tbl_role_claim_map
                                        join t2 in _Dbcontext.tbl_user_role_map on t1.role_id equals t2.role_id
                                        where t1.DocumentMaster == requirement.accessRight && t1.PermissionType == requirement.accessRightType && t1.is_deleted == 0
                                        && t2.user_id == UserId && t2.is_deleted == 0
                                        select t1.claim_master_id).Count();
                        if (tempData > 0)
                        {
                            context.Succeed(requirement);
                        }
                    }
                    
                }
                
            }
            return Task.CompletedTask;
            
        }
    }
}

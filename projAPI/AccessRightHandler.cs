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
            if (context.User.HasClaim(c => c.Type == requirement.accessRight))
            {
                _Dbcontext.tbl_role_claim_map.Where(p=>p.claim_master_id== requirement.accessRight)

                var tempData= from t1 in _Dbcontext.tbl_user_role_map
                join t2 in _Dbcontext.tbl_role_claim_map on t1.claim_master_id equals t2.claim_master_id
                

                _Dbcontext.tbl_user_role_map.Where()
                context.Succeed(requirement);
                var empId = context.User.Claims.FirstOrDefault(c => c.Type == "__emp_id");
                var userId = context.User.Claims.FirstOrDefault(c => c.Type == "__user_id");
                var empRole = context.User.Claims.FirstOrDefault(c => c.Type == "__employee_role_id");
                var is_HOD = context.User.Claims.FirstOrDefault(c => c.Type == "__Is_HOD");
                var CompanyList= context.User.Claims.FirstOrDefault(c => c.Type == "__company_id_list");

                int _empId = 0, _userId=0,_is_Hod=0;
                int.TryParse(empId.Value, out _empId);
                int.TryParse(userId.Value, out _userId);
                int.TryParse(is_HOD.Value, out _is_Hod);
                _clsCurrentUser.EmpId = _empId;
                _clsCurrentUser.UserId = _userId;
                _clsCurrentUser.Is_Hod = _is_Hod;
                var tempRoles=empRole.Value.Split(",");
                _clsCurrentUser.RoleId = new List<int>();
                foreach (var temprole in tempRoles)
                {
                    _clsCurrentUser.RoleId.Add(Convert.ToInt32( temprole));
                }
                int x=0;
                var downlineempId = context.User.Claims.FirstOrDefault(c => c.Type == "__downline_emp_id");
                _clsCurrentUser.DownlineEmpId = downlineempId.Value.Split(",").Where(str => int.TryParse(str, out x)).Select(str => x).ToList();
                _clsCurrentUser.CompanyId = CompanyList.Value.Split(",").Where(str => int.TryParse(str, out x)).Select(str => x).ToList();

                _clsCurrentUser.Is_SuperAdmin = _clsCurrentUser.RoleId.Contains((int)enmRoleMaster.SuperAdmin);
                _clsCurrentUser.Is_HRAdmin = _clsCurrentUser.RoleId.Contains((int)enmRoleMaster.HRAdmin);



            }
            return Task.CompletedTask;
            
        }
    }
}

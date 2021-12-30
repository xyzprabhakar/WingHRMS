using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using projAPI.Model;
using projContext;
using projContext.DB;
using projContext.DB.Masters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace projAPI.Services
{
    
    public interface IsrvUsers
    {
        ulong? UserId { get; set; }

        void BlockUnblockUser(ulong UserId, byte is_logged_blocked);
        string GenerateJSONWebToken(string JWTKey, string JWTIssuer, ulong UserId, int employee_id, enmUserType user_type, int CustomerId, ulong DistributorId);
        string GenrateTempUser(string IP, string DeviceId);
        mdlCommonReturnUlong GetUser(ulong? UserId);
        List<Document> GetUserDocuments(ulong UserId, bool OnlyDisplayMenu);
        List<int> GetUserRole(ulong UserId);
        List<mdlCommonReturnUlong> GetUsers(ulong[] UserId);
        bool IsTempUserIDExist(string TempUserID);
        void SaveLoginLog(string IPAddress, string DeviceDetails, bool LoginStatus, string FromLocation, string Longitude, string Latitude);
        bool SetRoleDocument(mdlRoleMaster roleDocument, ulong CreatedBy);
        void SetTempOrganisation(ulong UserId);
        void SetTempRoleClaim(ulong UserId);
        bool SetUserRole(mdlUserRolesWraper userRoles, ulong CreatedBy);
        mdlReturnData ValidateUser(string UserName, string Password, int orgId, ulong Customer_Vendor_Id, enmUserType userType);
    }

    public class srvUsers : IsrvUsers
    {
        private readonly MasterContext _masterContext;
        private readonly Context _context;
        private readonly IsrvSettings _IsrvSettings;
        public ulong? UserId { get; set; }
        public srvUsers(Context context, MasterContext masterContext, IsrvSettings isrvSettings)
        {
            _context = context;
            _IsrvSettings = isrvSettings;
            _masterContext = masterContext;
        }
        public mdlCommonReturnUlong GetUser(ulong? UserId)
        {
            if (UserId > 0)
            {
                return new mdlCommonReturnUlong();
            }
            else
            {
                return new mdlCommonReturnUlong();
            }
        }
        public List<mdlCommonReturnUlong> GetUsers(ulong[] UserId)
        {
            return new List<mdlCommonReturnUlong>();
        }

        public mdlReturnData ValidateUser(string UserName,
            string Password, int orgId, ulong Customer_Vendor_Id, enmUserType userType)
        {
            int? OrganisationId = null;
            mdlReturnData ReturnData = new mdlReturnData() { MessageType = enmMessageType.None };
            tblUsersMaster tempData = null;
            IQueryable<tblUsersMaster> Query = _masterContext.tblUsersMaster.Where(p => p.UserName == UserName && p.OrgId == orgId && p.UserType == userType).AsQueryable();

            if (userType.HasFlag(enmUserType.Customer) || userType.HasFlag(enmUserType.Vendor))
            {
                Query = Query.Where(q => q.Id == Customer_Vendor_Id);
            }
            tempData = Query.FirstOrDefault();
            if (tempData == null)
            {
                ReturnData.MessageType = enmMessageType.Error;
                ReturnData.Message = "Invalid Username or Password !!";
                return ReturnData;
            }
            else
            {
                this.UserId = tempData.UserId;
            }
            if (tempData.Password != Password)
            {
                BlockUnblockUser(tempData.UserId, 1);
                ReturnData.MessageType = enmMessageType.Error;
                ReturnData.Message = "Invalid Username or Password !!";
                return ReturnData;
            }
            else if (!tempData.IsActive)
            {
                ReturnData.MessageType = enmMessageType.Error;
                ReturnData.Message = "Invalid User";
                return ReturnData;
            }
            else if (tempData.is_logged_blocked == 1)
            {
                if (tempData.logged_blocked_Enddt < DateTime.Now)
                {
                    BlockUnblockUser(tempData.UserId, 0);
                }
                else
                {
                    ReturnData.MessageType = enmMessageType.Error;
                    ReturnData.Message = "User is Blocked";
                    return ReturnData;
                }
            }
            ReturnData.MessageType = enmMessageType.Success;
            ReturnData.Message = "Validate Successfully";
            ReturnData.ReturnId = new
            {
                UserId = tempData.UserId,
                NormalizedName = tempData.NormalizedName,
                Id = tempData.Id,
                UserType = tempData.UserType,
                OrgId = tempData.OrgId
            };
            return ReturnData;
        }


        public void BlockUnblockUser(ulong UserId, byte is_logged_blocked)
        {
            bool AllowBlockonFail = false;
            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));
            int BlockUserAfterLoginFailAttempets = 10, BlockUserAfterLoginFailAttempetsForTime = 30;
            var tempData = _masterContext.tblUsersMaster.Where(p => p.UserId == UserId).FirstOrDefault();
            if (tempData == null)
            {
                return;
            }
            if (is_logged_blocked == 1)
            {
                bool.TryParse(_IsrvSettings.GetSettings("UserSetting", "AllowBlockonFail"), out AllowBlockonFail);
                if (AllowBlockonFail)
                {
                    int.TryParse(_IsrvSettings.GetSettings("UserSetting", "BlockUserAfterLoginFailAttempets"), out BlockUserAfterLoginFailAttempets);
                    int.TryParse(_IsrvSettings.GetSettings("UserSetting", "BlockUserAfterLoginFailAttempetsForTime"), out BlockUserAfterLoginFailAttempetsForTime);
                    if (tempData.LoginFailCount >= BlockUserAfterLoginFailAttempets && DateTime.Compare(tempData.LoginFailCountdt, CurrentDate) == 0)
                    {
                        tempData.is_logged_blocked = is_logged_blocked;
                        tempData.logged_blocked_dt = DateTime.Now;
                        tempData.logged_blocked_Enddt = DateTime.Now.AddMinutes(BlockUserAfterLoginFailAttempetsForTime);

                    }
                    else
                    {
                        if (DateTime.Compare(tempData.LoginFailCountdt, CurrentDate) != 0)
                        {
                            tempData.LoginFailCount = 1;
                            tempData.LoginFailCountdt = CurrentDate;
                        }
                        else
                        {
                            tempData.LoginFailCount = ++tempData.LoginFailCount;
                        }
                    }
                }
            }
            else
            {
                tempData.is_logged_blocked = 0;
            }
            _masterContext.tblUsersMaster.Update(tempData);
            _masterContext.SaveChanges();

        }


        public void SaveLoginLog(string IPAddress, string DeviceDetails, bool LoginStatus, string FromLocation, string Longitude, string Latitude)
        {
            if (this.UserId == null)
            {
                return;
            }

            _masterContext.tblUserLoginLog.Add(new projContext.DB.Masters.tblUserLoginLog()
            {
                UserId = UserId.Value,
                IPAddress = IPAddress,
                DeviceDetails = DeviceDetails,
                LoginStatus = LoginStatus,
                FromLocation = FromLocation,
                LoginDateTime = DateTime.Now,
                Longitude = Longitude,
                Latitude = Latitude
            });
            _context.SaveChanges();
        }


        public string GenrateTempUser(string IP, string DeviceId)
        {
            string tempUserID = Guid.NewGuid().ToString().Replace("-", "");
            _context.tbl_guid_detail.Add(new tbl_guid_detail() { DeviceId = DeviceId, FromIP = IP, genration_dt = DateTime.Now, id = tempUserID });
            _context.SaveChanges();
            return tempUserID;
        }

        public bool IsTempUserIDExist(string TempUserID)
        {
            return _context.tbl_guid_detail.Where(p => p.id == TempUserID && p.genration_dt >= DateTime.Now.AddDays(-30)).Count() > 0 ? true : false;
        }


        public string GenerateJSONWebToken(string JWTKey, string JWTIssuer,
            ulong UserId, int employee_id, enmUserType user_type, int CustomerId, ulong DistributorId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> _claim = new List<Claim>();
            _claim.Add(new Claim("__UserId", Convert.ToString(UserId)));
            _claim.Add(new Claim("__employee_id", Convert.ToString(employee_id)));
            _claim.Add(new Claim("__user_type", Convert.ToString(user_type)));
            _claim.Add(new Claim("__CustomerId", Convert.ToString(CustomerId)));
            _claim.Add(new Claim("__DistributorId", Convert.ToString(DistributorId)));
            int TokenExpiryTime = 10080;
            int.TryParse(_IsrvSettings.GetSettings("UserSetting", "TokenExpiryTime"), out TokenExpiryTime);
            var token = new JwtSecurityToken(JWTIssuer, JWTIssuer, _claim, expires: DateTime.Now.AddMinutes(TokenExpiryTime),
              signingCredentials: credentials);
            string Token = new JwtSecurityTokenHandler().WriteToken(token);
            return Token;
        }


        //public List<Application> GetUserApplication(ulong UserId)
        //{
        //    return _context.tblUsersApplication.Where(p => p.UserId == UserId && p.IsActive).Select(p => p.Applications.GetApplicationDetails()).ToList();
        //}

        //public bool SetUserApplication(ulong UserId, List<enmApplication> Applications, ulong CreatedBy)
        //{
        //    DateTime currentDate = DateTime.Now;
        //    var ExistingApplication = _context.tblUsersApplication.Where(p => p.UserId == UserId).Select(p => p.Applications).ToList();
        //    Applications.RemoveAll(q => ExistingApplication.Contains(q));
        //    _context.tblUsersApplication.AddRange(Applications.Select(q => new tblUsersApplication
        //    {
        //        Applications = q,
        //        CreatedBy = CreatedBy,
        //        CreatedDt = currentDate,
        //        IsActive = true,
        //        ModifiedBy = CreatedBy,
        //        ModifiedDt = currentDate,
        //        ModifyRemarks = string.Empty,
        //        UserId = UserId
        //    }));
        //    _context.SaveChanges();
        //    return true;
        //}

        #region Document

        public void SetTempRoleClaim(ulong UserId)
        {

            _masterContext.Database.ExecuteSqlCommand("delete from tblUserAllClaim Where UserId=@p1;" +
                @"insert into tblUserAllClaim(UserId,DocumentMaster,PermissionType)
            select Distinct @p1,DocumentMaster,PermissionType from 
             (select t2.DocumentMaster,t2.PermissionType from tblUserRole t1 inner join tblRoleClaim  t2 on t1.RoleId=t2.RoleId Where t1.UserId=@p1 and t1.IsDeleted=0 and t2.IsDeleted=0
             union 
             select t2.DocumentMaster,t2.PermissionType  from tblUserClaim where UserId=@p1 and IsDeleted=0) t1;", UserId);
        }
        public void SetTempOrganisation(ulong UserId)
        {

            _masterContext.Database.ExecuteSqlCommand(@"delete from tblUserAllLocationPermission  Where UserId=@p1;
             insert into tblUserAllLocationPermission(UserId,LocationId)
             select Distinct @p1,LocationId  from 
             (select t2.LocationId from tblUserOrganisationPermission t1 inner join tblLocationMaster t2 on t1.OrgId=t2.OrgId and t1.HaveAllCompanyAccess=1 
             Where t1.UserId=@p1 and t1.IsDeleted=0 
             union
             select t3.LocationId from tblUserOrganisationPermission t1 inner join tblZoneMaster t2 on t1.OrgId=t2.OrgId and t1.UserId=@p1 and t1.IsDeleted=0 and t1.HaveAllCompanyAccess=0 
             inner join tblLocationMaster t3 on t2.ZoneId=t3.ZoneId
             inner join tblUserCompanyPermission t4 on t4.CompanyId=t2.CompanyId and t4.UserId=@p1 and t4.IsDeleted=0  and t4.HaveAllZoneAccess=1
             union
             select t3.LocationId from tblUserOrganisationPermission t1 inner join tblZoneMaster t2 on t1.OrgId=t2.OrgId and t1.UserId=@p1 and t1.IsDeleted=0 and t1.HaveAllCompanyAccess=0 
             inner join tblLocationMaster t3 on t2.ZoneId=t3.ZoneId
             inner join tblUserCompanyPermission t4 on t4.CompanyId=t2.CompanyId and t4.UserId=@p1 and t4.IsDeleted=0  and t4.HaveAllZoneAccess=0
             inner join tblUserZonePermission t5 on t5.ZoneId=t2.ZoneId and t5.UserId=@p1 and t5.IsDeleted=0  and t5.HaveAllLocationAccess=1
             union
             select LocationId from tblUserLocationPermission Where UserId=@p1 and IsDeleted=0 )t;", UserId);
        }


        public List<Document> GetUserDocuments(ulong UserId, bool OnlyDisplayMenu)
        {
            List<Document> documents = new List<Document>();
            var tempData = (from t1 in _context.tbl_role_claim_map
                            join t2 in _context.tbl_user_role_map on t1.role_id equals t2.role_id
                            where t1.is_deleted == 0 && t2.user_id == UserId && t2.is_deleted == 0
                            select new { t1.DocumentMaster, t1.PermissionType }).ToList();
            documents.AddRange(tempData.Select(p => p.DocumentMaster).Distinct().Select(p => p.GetDocumentDetails()).OrderBy(p => p.DisplayOrder));
            documents.ForEach(p =>
            {
                enmDocumentType ExistingPermissionType = p.DocumentType;
                enmDocumentType permissionType = enmDocumentType.None;
                if (p.DocumentType.HasFlag(enmDocumentType.Create) && tempData.Any(q => (int)q.DocumentMaster == p.Id && q.PermissionType.HasFlag(enmDocumentType.Create)))
                    permissionType = permissionType | enmDocumentType.Create;
                if (p.DocumentType.HasFlag(enmDocumentType.Update) && tempData.Any(q => (int)q.DocumentMaster == p.Id && q.PermissionType.HasFlag(enmDocumentType.Update)))
                    permissionType = permissionType | enmDocumentType.Update;
                if (p.DocumentType.HasFlag(enmDocumentType.Approval) && tempData.Any(q => (int)q.DocumentMaster == p.Id && q.PermissionType.HasFlag(enmDocumentType.Approval)))
                    permissionType = permissionType | enmDocumentType.Approval;
                if (p.DocumentType.HasFlag(enmDocumentType.Delete) && tempData.Any(q => (int)q.DocumentMaster == p.Id && q.PermissionType.HasFlag(enmDocumentType.Delete)))
                    permissionType = permissionType | enmDocumentType.Delete;
                if (p.DocumentType.HasFlag(enmDocumentType.Report) && tempData.Any(q => (int)q.DocumentMaster == p.Id && q.PermissionType.HasFlag(enmDocumentType.Report)))
                    permissionType = permissionType | enmDocumentType.Report;
                if (p.DocumentType.HasFlag(enmDocumentType.DisplayMenu))
                    permissionType = permissionType | enmDocumentType.DisplayMenu;
            });
            if (OnlyDisplayMenu)
            {
                documents.RemoveAll(p => !p.DocumentType.HasFlag(enmDocumentType.DisplayMenu));
            }
            return documents;
        }
        public bool SetRoleDocument(mdlRoleMaster roleDocument, ulong CreatedBy)
        {
            DateTime dateTime = DateTime.Now;
            var tempData = _context.tbl_role_claim_map.Where(q => q.role_id == roleDocument.roleId && q.is_deleted == 0).ToList();
            tempData.ForEach(q => { q.is_deleted = 1; q.last_modified_by = CreatedBy; q.last_modified_date = dateTime; });
            _context.tbl_role_claim_map.UpdateRange(tempData);
            _context.tbl_role_claim_map.AddRange(roleDocument.roleDocument.Select(p => new tbl_role_claim_map
            {
                role_id = roleDocument.roleId,
                created_by = CreatedBy,
                created_date = dateTime,
                last_modified_by = CreatedBy,
                last_modified_date = dateTime,
                DocumentMaster = p.documentId,
                is_deleted = 0,
                PermissionType = p.PermissionType
            }));
            _context.SaveChanges();
            return true;
        }
        #endregion

        #region UserRole
        public List<int> GetUserRole(ulong UserId)
        {
            return _context.tbl_user_role_map.Where(p => p.user_id == UserId && p.is_deleted == 0).Select(p => p.role_id ?? 0).ToList();
        }
        public bool SetUserRole(mdlUserRolesWraper userRoles, ulong CreatedBy)
        {
            DateTime dateTime = DateTime.Now;
            List<tbl_user_role_map> TobeAdded = new List<tbl_user_role_map>();
            List<tbl_user_role_map> TobeUpdate = new List<tbl_user_role_map>();
            //Get Existsing Role
            var tempData = _context.tbl_user_role_map.Where(p => p.user_id == userRoles.userMaster.userId && p.is_deleted == 0).ToList();
            userRoles.userRoles.ForEach(q =>
            {
                var innerTemp = tempData.FirstOrDefault(p => p.role_id == q.roleMaster.roleId);
                if (innerTemp == null)
                {
                    TobeAdded.Add(new tbl_user_role_map()
                    {
                        role_id = q.roleMaster.roleId,
                        IsActive = q.isActive,
                        created_by = CreatedBy,
                        last_modified_by = CreatedBy,
                        created_date = dateTime,
                        last_modified_date = dateTime,
                        is_deleted = 0,
                        user_id = userRoles.userMaster.userId

                    });
                }
                else
                {
                    if (q.isActive != innerTemp.IsActive)
                    {
                        innerTemp.IsActive = q.isActive;
                        innerTemp.last_modified_by = CreatedBy;
                        innerTemp.last_modified_date = dateTime;
                        TobeUpdate.Add(innerTemp);
                    }
                }


            });
            _context.tbl_user_role_map.AddRange(TobeAdded);
            _context.tbl_user_role_map.UpdateRange(TobeAdded);
            _context.SaveChanges();
            return true;
        }
        #endregion

    }

    public interface IsrvCurrentUser
    {
        int CustomerId { get; }
        ulong DistributorId { get; }
        int employee_id { get; }
        enmUserType user_type { get; }
        ulong UserId { get; }
    }

    public class srvCurrentUser : IsrvCurrentUser
    {
        private ulong _UserId = 0, _DistributorId = 0;
        private int _employee_id = 0, _CustomerId = 0;
        private enmUserType _user_type;
        public srvCurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _user_type = enmUserType.Customer;
            ulong.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__UserId").FirstOrDefault()?.Value, out _UserId);
            ulong.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__DistributorId").FirstOrDefault()?.Value, out _DistributorId);
            int.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__CustomerId").FirstOrDefault()?.Value, out _CustomerId);
            int.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__employee_id").FirstOrDefault()?.Value, out _employee_id);
            Enum.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__user_type").FirstOrDefault()?.Value, out _user_type);

        }
        public ulong UserId { get { return _UserId; } private set { } }
        public int employee_id { get { return _employee_id; } private set { } }
        public enmUserType user_type { get { return _user_type; } private set { } }
        public ulong DistributorId { get { return _DistributorId; } private set { } }
        public int CustomerId { get { return _CustomerId; } private set { } }

    }

}

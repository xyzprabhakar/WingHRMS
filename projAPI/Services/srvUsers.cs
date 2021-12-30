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
        string GenerateJSONWebToken(string JWTKey, string JWTIssuer, ulong UserId, int CustomerId, int EmployeeId, int VendorId, ulong DistributorId, enmUserType userType, enmCustomerType CustomerType);
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
        bool SetUserDocument(ulong UserId, List<mdlRoleDocument> usrClaim, ulong CreatedBy);
        bool SetUserRole(mdlUserRolesWraper userRoles, ulong CreatedBy);
        mdlReturnData ValidateUser(string UserName, string Password, int orgId, int CustomerId, int VendorId, enmUserType userType);
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
            string Password, int orgId, int CustomerId, int VendorId, enmUserType userType)
        {
            mdlReturnData ReturnData = new mdlReturnData() { MessageType = enmMessageType.None };
            tblUsersMaster tempData = null;
            IQueryable<tblUsersMaster> Query = _masterContext.tblUsersMaster.Where(p => p.UserName == UserName && p.OrgId == orgId && p.UserType == userType).AsQueryable();

            if (userType.HasFlag(enmUserType.Customer))
            {
                Query = Query.Where(q => q.CustomerId == CustomerId);
            }
            if (userType.HasFlag(enmUserType.Vendor))
            {
                Query = Query.Where(q => q.VendorId == VendorId);
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
            SetTempRoleClaim(tempData.UserId);
            SetTempOrganisation(tempData.UserId);
            ReturnData.MessageType = enmMessageType.Success;
            ReturnData.Message = "Validate Successfully";
            ReturnData.ReturnId = new
            {
                UserId = tempData.UserId,
                NormalizedName = tempData.NormalizedName,
                VendorId = tempData.VendorId,
                EmpId = tempData.EmpId,
                DistributorId = tempData.DistributorId,
                CustomerId = tempData.CustomerId,
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
            ulong UserId, int CustomerId,
            int EmployeeId, int VendorId, ulong DistributorId
            , enmUserType userType, enmCustomerType CustomerType

            )
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> _claim = new List<Claim>();
            _claim.Add(new Claim("__UserId", Convert.ToString(UserId)));
            _claim.Add(new Claim("__CustomerId", Convert.ToString(CustomerId)));
            _claim.Add(new Claim("__EmployeeId", Convert.ToString(EmployeeId)));
            _claim.Add(new Claim("__VendorId", Convert.ToString(VendorId)));
            _claim.Add(new Claim("__DistributorId", Convert.ToString(DistributorId)));
            _claim.Add(new Claim("__UserType", Convert.ToString(userType)));
            _claim.Add(new Claim("__CustomerType", Convert.ToString(CustomerType)));

            int TokenExpiryTime = 10080;
            int.TryParse(_IsrvSettings.GetSettings("UserSetting", "TokenExpiryTime"), out TokenExpiryTime);
            var token = new JwtSecurityToken(JWTIssuer, JWTIssuer, _claim, expires: DateTime.Now.AddMinutes(TokenExpiryTime),
              signingCredentials: credentials);
            string Token = new JwtSecurityTokenHandler().WriteToken(token);
            return Token;
        }
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
            List<tblUserAllClaim> alluserClaims = _masterContext.tblUserAllClaim.Where(p => p.UserId == UserId).ToList();
            var tempData = alluserClaims.Select(p => p.DocumentMaster).Distinct();
            List<Document> documents = new List<Document>();
            foreach (var d in tempData)
            {
                enmDocumentType permissionType = enmDocumentType.None;
                Document doc = d.GetDocumentDetails();
                if (doc.HaveCreate && alluserClaims.Any(p => p.PermissionType == enmDocumentType.Create))
                {
                    permissionType = permissionType | enmDocumentType.Create;
                }
                if (doc.HaveUpdate && alluserClaims.Any(p => p.PermissionType == enmDocumentType.Update))
                {
                    permissionType = permissionType | enmDocumentType.Update;
                }
                if (doc.HaveApproval && alluserClaims.Any(p => p.PermissionType == enmDocumentType.Approval))
                {
                    permissionType = permissionType | enmDocumentType.Update;
                }
                if (doc.HaveDelete && alluserClaims.Any(p => p.PermissionType == enmDocumentType.Delete))
                {
                    permissionType = permissionType | enmDocumentType.Update;
                }
                if (doc.HaveReport && alluserClaims.Any(p => p.PermissionType == enmDocumentType.Report))
                {
                    permissionType = permissionType | enmDocumentType.Update;
                }
                if (doc.HaveDisplayMenu && alluserClaims.Any(p => p.PermissionType == enmDocumentType.DisplayMenu))
                {
                    permissionType = permissionType | enmDocumentType.Update;
                }
                doc.DocumentType = permissionType;
                documents.Add(doc);
            }

            if (OnlyDisplayMenu)
            {
                documents.RemoveAll(q => !q.HaveDisplayMenu);
            }
            return documents;
        }
        public bool SetRoleDocument(mdlRoleMaster roleDocument, ulong CreatedBy)
        {
            DateTime dateTime = DateTime.Now;
            var tempData = _masterContext.tblRoleClaim.Where(q => q.RoleId == roleDocument.roleId && !q.IsDeleted).ToList();
            tempData.ForEach(q => { q.IsDeleted = true; q.ModifiedBy = CreatedBy; q.ModifiedDt = dateTime; });
            _masterContext.tblRoleClaim.UpdateRange(tempData);
            _masterContext.tblRoleClaim.AddRange(roleDocument.roleDocument.Where(p => p.PermissionType.HasFlag(enmDocumentType.Create)).Select(p => new tblRoleClaim
            {
                RoleId = roleDocument.roleId,
                CreatedBy = CreatedBy,
                CreatedDt = dateTime,
                ModifiedBy = CreatedBy,
                ModifiedDt = dateTime,
                DocumentMaster = p.documentId,
                IsDeleted = false,
                PermissionType = enmDocumentType.Create
            }));
            _masterContext.tblRoleClaim.AddRange(roleDocument.roleDocument.Where(p => p.PermissionType.HasFlag(enmDocumentType.Update)).Select(p => new tblRoleClaim
            {
                RoleId = roleDocument.roleId,
                CreatedBy = CreatedBy,
                CreatedDt = dateTime,
                ModifiedBy = CreatedBy,
                ModifiedDt = dateTime,
                DocumentMaster = p.documentId,
                IsDeleted = false,
                PermissionType = enmDocumentType.Update
            }));
            _masterContext.tblRoleClaim.AddRange(roleDocument.roleDocument.Where(p => p.PermissionType.HasFlag(enmDocumentType.Delete)).Select(p => new tblRoleClaim
            {
                RoleId = roleDocument.roleId,
                CreatedBy = CreatedBy,
                CreatedDt = dateTime,
                ModifiedBy = CreatedBy,
                ModifiedDt = dateTime,
                DocumentMaster = p.documentId,
                IsDeleted = false,
                PermissionType = enmDocumentType.Delete
            }));
            _masterContext.tblRoleClaim.AddRange(roleDocument.roleDocument.Where(p => p.PermissionType.HasFlag(enmDocumentType.Approval)).Select(p => new tblRoleClaim
            {
                RoleId = roleDocument.roleId,
                CreatedBy = CreatedBy,
                CreatedDt = dateTime,
                ModifiedBy = CreatedBy,
                ModifiedDt = dateTime,
                DocumentMaster = p.documentId,
                IsDeleted = false,
                PermissionType = enmDocumentType.Approval
            }));
            _masterContext.tblRoleClaim.AddRange(roleDocument.roleDocument.Where(p => p.PermissionType.HasFlag(enmDocumentType.Report)).Select(p => new tblRoleClaim
            {
                RoleId = roleDocument.roleId,
                CreatedBy = CreatedBy,
                CreatedDt = dateTime,
                ModifiedBy = CreatedBy,
                ModifiedDt = dateTime,
                DocumentMaster = p.documentId,
                IsDeleted = false,
                PermissionType = enmDocumentType.Report
            }));
            _masterContext.tblRoleClaim.AddRange(roleDocument.roleDocument.Where(p => p.PermissionType.HasFlag(enmDocumentType.DisplayMenu)).Select(p => new tblRoleClaim
            {
                RoleId = roleDocument.roleId,
                CreatedBy = CreatedBy,
                CreatedDt = dateTime,
                ModifiedBy = CreatedBy,
                ModifiedDt = dateTime,
                DocumentMaster = p.documentId,
                IsDeleted = false,
                PermissionType = enmDocumentType.DisplayMenu
            }));
            _masterContext.SaveChanges();
            return true;
        }
        public bool SetUserDocument(ulong UserId, List<mdlRoleDocument> usrClaim, ulong CreatedBy)
        {
            DateTime dateTime = DateTime.Now;
            var tempData = _masterContext.tblUserClaim.Where(q => q.UserId == UserId && !q.IsDeleted).ToList();
            tempData.ForEach(q => { q.IsDeleted = true; q.ModifiedBy = CreatedBy; q.ModifiedDt = dateTime; });
            _masterContext.tblUserClaim.UpdateRange(tempData);
            _masterContext.tblUserClaim.AddRange(usrClaim.Where(p => p.PermissionType.HasFlag(enmDocumentType.Create)).Select(p => new tblUserClaim
            {
                UserId = UserId,
                CreatedBy = CreatedBy,
                CreatedDt = dateTime,
                ModifiedBy = CreatedBy,
                ModifiedDt = dateTime,
                DocumentMaster = p.documentId,
                IsDeleted = false,
                PermissionType = enmDocumentType.Create
            }));
            _masterContext.tblUserClaim.AddRange(usrClaim.Where(p => p.PermissionType.HasFlag(enmDocumentType.Update))
                .Select(p => new tblUserClaim
                {
                    UserId = UserId,
                    CreatedBy = CreatedBy,
                    CreatedDt = dateTime,
                    ModifiedBy = CreatedBy,
                    ModifiedDt = dateTime,
                    DocumentMaster = p.documentId,
                    IsDeleted = false,
                    PermissionType = enmDocumentType.Update
                }));
            _masterContext.tblUserClaim.AddRange(usrClaim.Where(p => p.PermissionType.HasFlag(enmDocumentType.Delete))
                .Select(p => new tblUserClaim
                {
                    UserId = UserId,
                    CreatedBy = CreatedBy,
                    CreatedDt = dateTime,
                    ModifiedBy = CreatedBy,
                    ModifiedDt = dateTime,
                    DocumentMaster = p.documentId,
                    IsDeleted = false,
                    PermissionType = enmDocumentType.Delete
                }));
            _masterContext.tblUserClaim.AddRange(usrClaim.Where(p => p.PermissionType.HasFlag(enmDocumentType.Approval))
                .Select(p => new tblUserClaim
                {
                    UserId = UserId,
                    CreatedBy = CreatedBy,
                    CreatedDt = dateTime,
                    ModifiedBy = CreatedBy,
                    ModifiedDt = dateTime,
                    DocumentMaster = p.documentId,
                    IsDeleted = false,
                    PermissionType = enmDocumentType.Approval
                }));
            _masterContext.tblUserClaim.AddRange(usrClaim.Where(p => p.PermissionType.HasFlag(enmDocumentType.Report)).
                Select(p => new tblUserClaim
                {
                    UserId = UserId,
                    CreatedBy = CreatedBy,
                    CreatedDt = dateTime,
                    ModifiedBy = CreatedBy,
                    ModifiedDt = dateTime,
                    DocumentMaster = p.documentId,
                    IsDeleted = false,
                    PermissionType = enmDocumentType.Report
                }));
            _masterContext.tblUserClaim.AddRange(usrClaim.Where(p => p.PermissionType.HasFlag(enmDocumentType.DisplayMenu))
                .Select(p => new tblUserClaim
                {
                    UserId = UserId,
                    CreatedBy = CreatedBy,
                    CreatedDt = dateTime,
                    ModifiedBy = CreatedBy,
                    ModifiedDt = dateTime,
                    DocumentMaster = p.documentId,
                    IsDeleted = false,
                    PermissionType = enmDocumentType.DisplayMenu
                }));
            _masterContext.SaveChanges();
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
        enmCustomerType customerType { get; }
        ulong DistributorId { get; }
        int EmployeeId { get; }
        ulong UserId { get; }
        enmUserType UserType { get; }
        int VendorId { get; }
    }

    public class srvCurrentUser : IsrvCurrentUser
    {
        private ulong _UserId = 0, _DistributorId = 0;
        private int _EmployeeId = 0, _CustomerId = 0, _VendorId;
        private enmUserType _UserType = enmUserType.Customer;
        private enmCustomerType _CustomerType = enmCustomerType.None;
        public srvCurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            ulong.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__UserId").FirstOrDefault()?.Value, out _UserId);
            int.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__CustomerId").FirstOrDefault()?.Value, out _CustomerId);
            int.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__EmployeeId").FirstOrDefault()?.Value, out _EmployeeId);
            int.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__VendorId").FirstOrDefault()?.Value, out _VendorId);
            ulong.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__DistributorId").FirstOrDefault()?.Value, out _DistributorId);

            Enum.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__UserType").FirstOrDefault()?.Value, out _UserType);
            Enum.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__CustomerType").FirstOrDefault()?.Value, out _CustomerType);

        }
        public ulong UserId { get { return _UserId; } private set { } }
        public int CustomerId { get { return _CustomerId; } private set { } }
        public int EmployeeId { get { return _EmployeeId; } private set { } }
        public int VendorId { get { return _VendorId; } private set { } }
        public ulong DistributorId { get { return _DistributorId; } private set { } }
        public enmUserType UserType { get { return _UserType; } private set { } }
        public enmCustomerType customerType { get { return _CustomerType; } private set { } }

    }

}

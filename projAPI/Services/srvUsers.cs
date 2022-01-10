using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using projAPI.Model;
using projContext;
using projContext.DB;
using projContext.DB.Masters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        string GenerateJSONWebToken(string JWTKey, string JWTIssuer, ulong UserId, int CustomerId, int EmployeeId, int VendorId, ulong DistributorId, enmUserType userType, enmCustomerType CustomerType, int OrgId);
        string GenrateTempUser(string IP, string DeviceId);
        mdlCommonReturnUlong GetUser(ulong? UserId);
        List<mdlCommonReturnWithParentID> GetUserCompany(ulong UserId, int? OrgId, List<int> OrgIds);
        List<Document> GetUserDocuments(ulong UserId, bool OnlyDisplayMenu);
        List<mdlCommonReturnWithParentID> GetUserLocation(bool ClearCache, ulong UserId, int? OrgId, int? CompanyId, int? ZoneId);
        List<mdlCommonReturnWithParentID> GetUserOrganisation(ulong UserId);
        List<int> GetUserRole(ulong UserId);
        List<mdlCommonReturnUlong> GetUsers(ulong[] UserId);
        List<mdlCommonReturnWithParentID> GetUserZone(ulong UserId, int? OrgId, int? CompanyId, List<int> CompanyIds);
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
            , enmUserType userType, enmCustomerType CustomerType, int OrgId

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
            _claim.Add(new Claim("__OrgId", Convert.ToString(OrgId)));
            _claim.Add(new Claim("__OrgIds", string.Join(",", _masterContext.tblUserOrganisationPermission.Where(p => p.UserId == UserId && !p.IsDeleted).Select(p => p.OrgId))));
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
            _masterContext.Database.ExecuteSqlCommand(string.Format(@"delete from tblUserAllClaim Where UserId={0};
                insert into tblUserAllClaim(UserId,DocumentMaster,PermissionType)
            select Distinct {0},DocumentMaster,PermissionType from 
             (select t2.DocumentMaster,t2.PermissionType from tblUserRole t1 inner join tblRoleClaim  t2 on t1.RoleId=t2.RoleId Where t1.UserId={0} and t1.IsDeleted=0 and t2.IsDeleted=0
             union 
             select DocumentMaster,PermissionType  from tblUserClaim where UserId={0} and IsDeleted=0) t1;", UserId));

        }
        public void SetTempOrganisation(ulong UserId)
        {

            _masterContext.Database.ExecuteSqlCommand(string.Format(@"delete from tblUserAllLocationPermission  Where UserId={0};
             insert into tblUserAllLocationPermission(UserId,LocationId)
             select Distinct {0},LocationId  from 
             (select t2.LocationId from tblUserOrganisationPermission t1 inner join tblLocationMaster t2 on t1.OrgId=t2.OrgId and t1.HaveAllCompanyAccess=1 
             Where t1.UserId={0} and t1.IsDeleted=0 
             union
             select t3.LocationId from tblUserOrganisationPermission t1 inner join tblZoneMaster t2 on t1.OrgId=t2.OrgId and t1.UserId={0} and t1.IsDeleted=0 and t1.HaveAllCompanyAccess=0 
             inner join tblLocationMaster t3 on t2.ZoneId=t3.ZoneId
             inner join tblUserCompanyPermission t4 on t4.CompanyId=t2.CompanyId and t4.UserId={0} and t4.IsDeleted=0  and t4.HaveAllZoneAccess=1
             union
             select t3.LocationId from tblUserOrganisationPermission t1 inner join tblZoneMaster t2 on t1.OrgId=t2.OrgId and t1.UserId={0} and t1.IsDeleted=0 and t1.HaveAllCompanyAccess=0 
             inner join tblLocationMaster t3 on t2.ZoneId=t3.ZoneId
             inner join tblUserCompanyPermission t4 on t4.CompanyId=t2.CompanyId and t4.UserId={0} and t4.IsDeleted=0  and t4.HaveAllZoneAccess=0
             inner join tblUserZonePermission t5 on t5.ZoneId=t2.ZoneId and t5.UserId={0} and t5.IsDeleted=0  and t5.HaveAllLocationAccess=1
             union
             select LocationId from tblUserLocationPermission Where UserId={0} and IsDeleted=0 )t;", UserId));

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
                    permissionType = permissionType | enmDocumentType.Approval;
                }
                if (doc.HaveDelete && alluserClaims.Any(p => p.PermissionType == enmDocumentType.Delete))
                {
                    permissionType = permissionType | enmDocumentType.Delete;
                }
                if (doc.HaveReport && alluserClaims.Any(p => p.PermissionType == enmDocumentType.Report))
                {
                    permissionType = permissionType | enmDocumentType.Report;
                }
                if (doc.HaveDisplayMenu && alluserClaims.Any(p => p.PermissionType == enmDocumentType.DisplayMenu))
                {
                    permissionType = permissionType | enmDocumentType.DisplayMenu;
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

        public List<mdlCommonReturnWithParentID> GetUserOrganisation(ulong UserId)
        {
            List<mdlCommonReturnWithParentID> returnData = new List<mdlCommonReturnWithParentID>();
            returnData.AddRange(
            _masterContext.tblUserOrganisationPermission.Where(p => p.UserId == UserId && !p.IsDeleted).
                Select(p => new mdlCommonReturnWithParentID { Code = p.tblOrganisation.Code, Name = p.tblOrganisation.Name, IsActive = p.tblOrganisation.IsActive, Id = p.OrgId ?? 0 }
                ));
            return returnData;
        }

        public List<mdlCommonReturnWithParentID> GetUserCompany(ulong UserId, int? OrgId, List<int> OrgIds)
        {
            List<mdlCommonReturnWithParentID> returnData = new List<mdlCommonReturnWithParentID>();
            if (OrgId == 0)
            {
                return returnData;
            }
            var queryData = (from t1 in _masterContext.tblUserOrganisationPermission
                             join t2 in _masterContext.tblCompanyMaster on t1.OrgId equals t2.OrgId
                             where !t1.IsDeleted && t1.UserId == UserId && t1.HaveAllCompanyAccess
                             select new mdlCommonReturnWithParentID { ParentId = t2.OrgId ?? 0, Id = t2.CompanyId, Code = t2.Code, Name = t2.Name, IsActive = t2.IsActive }
                    ).Union(
                    from t1 in _masterContext.tblCompanyMaster
                    join t2 in _masterContext.tblUserOrganisationPermission on t1.OrgId equals t2.OrgId
                    join t3 in _masterContext.tblUserCompanyPermission on t1.CompanyId equals t3.CompanyId
                    where !t2.IsDeleted && t2.UserId == UserId && !t2.HaveAllCompanyAccess &&
                    !t3.IsDeleted && t3.UserId == UserId
                    select new mdlCommonReturnWithParentID { ParentId = t1.OrgId ?? 0, Id = t1.CompanyId, Code = t1.Code, Name = t1.Name, IsActive = t1.IsActive }
                    );
            if (OrgId > 0)
            {
                returnData.AddRange(queryData.Where(p => p.ParentId == OrgId));
            }
            else if ((OrgIds?.Count ?? 0) > 0)
            {
                returnData.AddRange(queryData.Where(p => OrgIds.Contains(p.ParentId)));
            }
            else
            {
                returnData.AddRange(queryData);
            }

            return returnData;
        }

        public List<mdlCommonReturnWithParentID> GetUserZone(ulong UserId, int? OrgId, int? CompanyId, List<int> CompanyIds)
        {
            List<mdlCommonReturnWithParentID> returnData = new List<mdlCommonReturnWithParentID>();
            if (CompanyId == 0)
            {
                return returnData;
            }
            var QuerableData = (from t1 in _masterContext.tblUserOrganisationPermission
                                join t2 in _masterContext.tblCompanyMaster on t1.OrgId equals t2.OrgId
                                join t3 in _masterContext.tblZoneMaster on t2.CompanyId equals t3.CompanyId
                                where !t1.IsDeleted && t1.UserId == UserId && t1.HaveAllCompanyAccess
                                select new { OrgId = t3.OrgId, ParentId = t3.CompanyId ?? 0, Id = t3.ZoneId, Code = string.Empty, Name = t3.Name, IsActive = t3.IsActive }
                    ).Union(
                         from t1 in _masterContext.tblUserOrganisationPermission
                         join t2 in _masterContext.tblCompanyMaster on t1.OrgId equals t2.OrgId
                         join t3 in _masterContext.tblZoneMaster on t2.CompanyId equals t3.CompanyId
                         join t4 in _masterContext.tblUserCompanyPermission on t2.CompanyId equals t4.CompanyId
                         where !t1.IsDeleted && t1.UserId == UserId && !t1.HaveAllCompanyAccess
                         && t4.UserId == UserId && !t4.IsDeleted && t4.HaveAllZoneAccess
                         select new { OrgId = t3.OrgId, ParentId = t3.CompanyId ?? 0, Id = t3.ZoneId, Code = string.Empty, Name = t3.Name, IsActive = t3.IsActive }
                     )
                    .Union(
                    from t1 in _masterContext.tblUserOrganisationPermission
                    join t2 in _masterContext.tblCompanyMaster on t1.OrgId equals t2.OrgId
                    join t3 in _masterContext.tblZoneMaster on t2.CompanyId equals t3.CompanyId
                    join t4 in _masterContext.tblUserCompanyPermission on t2.CompanyId equals t4.CompanyId
                    join t5 in _masterContext.tblUserZonePermission on t3.ZoneId equals t5.ZoneId
                    where !t1.IsDeleted && t1.UserId == UserId && !t1.HaveAllCompanyAccess
                    && t4.UserId == UserId && !t4.IsDeleted && !t4.HaveAllZoneAccess
                    && t5.UserId == UserId && !t5.IsDeleted
                    select new { OrgId = t3.OrgId, ParentId = t3.CompanyId ?? 0, Id = t3.ZoneId, Code = string.Empty, Name = t3.Name, IsActive = t3.IsActive }
                    );
            if (CompanyId > 0)
            {
                returnData.AddRange(QuerableData.Where(p => p.ParentId == CompanyId).Select(p => new mdlCommonReturnWithParentID { ParentId = p.ParentId, Id = p.Id, Code = p.Code, Name = p.Name, IsActive = p.IsActive }));
            }
            else if (OrgId > 0)
            {
                returnData.AddRange(QuerableData.Where(p => p.OrgId == OrgId).Select(p => new mdlCommonReturnWithParentID { ParentId = p.ParentId, Id = p.Id, Code = p.Code, Name = p.Name, IsActive = p.IsActive }));
            }
            else if ((CompanyIds?.Count ?? 0) > 0)
            {
                returnData.AddRange(QuerableData.Where(p => CompanyIds.Contains(p.ParentId)).Select(p => new mdlCommonReturnWithParentID { ParentId = p.ParentId, Id = p.Id, Code = p.Code, Name = p.Name, IsActive = p.IsActive }));
            }
            else
            {
                returnData.AddRange(QuerableData.Select(p => new mdlCommonReturnWithParentID { ParentId = p.ParentId,Id=p.Id ,Code = p.Code, Name = p.Name, IsActive = p.IsActive }));
            }

            return returnData;
        }

        public List<mdlCommonReturnWithParentID> GetUserLocation(bool ClearCache, ulong UserId, int? OrgId, int? CompanyId, int? ZoneId)
        {
            List<mdlCommonReturnWithParentID> returnData = new List<mdlCommonReturnWithParentID>();
            if (ClearCache)
            {
                SetTempOrganisation(UserId);
            }
            if (ZoneId != null)
            {
                returnData.AddRange(
                _masterContext.tblUserAllLocationPermission.Where(p => p.UserId == UserId && p.tblLocationMaster.ZoneId == ZoneId).
                    Select(p => new mdlCommonReturnWithParentID { Name = p.tblLocationMaster.Name, Code = string.Empty, Id = p.LocationId ?? 0, IsActive = p.tblLocationMaster.IsActive, ParentId = p.tblLocationMaster.ZoneId ?? 0 }));
            }
            else if (CompanyId != null)
            {
                returnData.AddRange(
                from t1 in _masterContext.tblUserAllLocationPermission
                join t2 in _masterContext.tblLocationMaster on t1.LocationId equals t2.LocationId
                join t3 in _masterContext.tblZoneMaster on t2.ZoneId equals t3.ZoneId
                where t3.CompanyId == CompanyId && t1.UserId == UserId
                select new mdlCommonReturnWithParentID { Name = t2.Name, Code = string.Empty, Id = t2.LocationId, IsActive = t2.IsActive, ParentId = t3.ZoneId });
            }
            else if (OrgId != null)
            {
                returnData.AddRange(
                from t1 in _masterContext.tblUserAllLocationPermission
                join t2 in _masterContext.tblLocationMaster on t1.LocationId equals t2.LocationId
                where t2.OrgId == OrgId && t1.UserId == UserId
                select new mdlCommonReturnWithParentID { Name = t2.Name, Code = string.Empty, Id = t2.LocationId, IsActive = t2.IsActive, ParentId = t2.ZoneId ?? 0 });
            }
            else 
            {
                returnData.AddRange(
                from t1 in _masterContext.tblUserAllLocationPermission
                join t2 in _masterContext.tblLocationMaster on t1.LocationId equals t2.LocationId                
                where t1.UserId == UserId
                select new mdlCommonReturnWithParentID { Name = t2.Name, Code = string.Empty, Id = t2.LocationId, IsActive = t2.IsActive, ParentId = t2.ZoneId ?? 0 });
            }
            return returnData;
        }

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
        int OrgId { get; }
        int []OrgIds { get; }
        bool HaveOrganisationPermission(int OrgId);
    }

    public class srvCurrentUser : IsrvCurrentUser
    {
        private ulong _UserId = 0, _DistributorId = 0;
        private int _EmployeeId = 0, _CustomerId = 0, _VendorId = 0, _OrgId = 0;
        private int[] _OrgIds;
        private enmUserType _UserType = enmUserType.Customer;
        private enmCustomerType _CustomerType = enmCustomerType.None;
        public srvCurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            ulong.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__UserId").FirstOrDefault()?.Value, out _UserId);
            int.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__CustomerId").FirstOrDefault()?.Value, out _CustomerId);
            int.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__EmployeeId").FirstOrDefault()?.Value, out _EmployeeId);
            int.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__VendorId").FirstOrDefault()?.Value, out _VendorId);
            ulong.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__DistributorId").FirstOrDefault()?.Value, out _DistributorId);
            int.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__OrgId").FirstOrDefault()?.Value, out _OrgId);
            Enum.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__UserType").FirstOrDefault()?.Value, out _UserType);
            Enum.TryParse(httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__CustomerType").FirstOrDefault()?.Value, out _CustomerType);
            string tempOrg = httpContextAccessor.HttpContext.User.Claims.Where(p => p.Type == "__OrgId").FirstOrDefault()?.Value ?? string.Empty;
            _OrgIds = tempOrg.Split(",")?.Select(p => Convert.ToInt32(p))?.ToArray() ?? null;
            if (_OrgIds == null)
            {
                _OrgIds = new int[1];
                _OrgIds[0] = _OrgId;
            }
        }
        public ulong UserId { get { return _UserId; } private set { } }
        public int OrgId { get { return _OrgId; } private set { } }
        public int[] OrgIds { get { return _OrgIds; } private set { } }
        public int CustomerId { get { return _CustomerId; } private set { } }
        public int EmployeeId { get { return _EmployeeId; } private set { } }
        public int VendorId { get { return _VendorId; } private set { } }
        public ulong DistributorId { get { return _DistributorId; } private set { } }
        public enmUserType UserType { get { return _UserType; } private set { } }
        public enmCustomerType customerType { get { return _CustomerType; } private set { } }
        public bool HaveOrganisationPermission(int OrgId)
        {
            return _OrgIds.Any(p => p == OrgId);
        }
    }

}

using projAPI.Model;
using projContext;
using projContext.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Services
{
    

    public interface IsrvUsers
    {
        void BlockUnblockUser(ulong UserId, byte is_logged_blocked);
        string GenrateTempUser(string IP, string DeviceId);
        bool IsTempUserIDExist(string TempUserID);
        void SaveLoginLog(string IPAddress, string DeviceDetails, bool LoginStatus, string FromLocation, string Longitude, string Latitude);
        mdlReturnData ValidateUser(string UserName, string Password, string OrgCode, enmUserType userType);
    }

    public class srvUsers : IsrvUsers

    {
        private readonly Context _context;
        private readonly IsrvSettings _IsrvSettings;
        public ulong? UserId { get; set; }
        public srvUsers(Context context, IsrvSettings isrvSettings)
        {
            _context = context;
            _IsrvSettings = isrvSettings;
        }
        public mdlReturnData ValidateUser(string UserName, string Password, string OrgCode, enmUserType userType)
        {
            int? OrganisationId = null;
            mdlReturnData ReturnData = new mdlReturnData() { MessageType = enmMessageType.None };
            if (userType.HasFlag(enmUserType.B2B) || userType.HasFlag(enmUserType.B2C))
            {
                if (string.IsNullOrEmpty(OrgCode))
                {
                    ReturnData.MessageType = enmMessageType.Error;
                    ReturnData.Message = "Invalid Organisation Code";
                    return ReturnData;
                }
                else
                {
                    var temOrgData = _context.tblCustomerOrganisation.Where(p => p.OrganisationCode == OrgCode).FirstOrDefault();
                    if (temOrgData == null)
                    {
                        ReturnData.MessageType = enmMessageType.Error;
                        ReturnData.Message = "Invalid Organisation Code";
                        return ReturnData;
                    }
                    if (!temOrgData.IsActive)
                    {
                        ReturnData.MessageType = enmMessageType.Error;
                        ReturnData.Message = "Blocked Organisation";
                        return ReturnData;
                    }
                    OrganisationId = temOrgData.CustomerId;
                }
            }
            tbl_user_master tempData = null;
            if (OrganisationId.HasValue)
            {
                tempData = _context.tbl_user_master.Where(p => p.username == UserName && p.CustomerId == OrganisationId  && p.user_type== userType ).FirstOrDefault();
            }
            else
            {
                tempData = _context.tbl_user_master.Where(p => p.username == UserName && p.password == Password && p.user_type == userType).FirstOrDefault();
            }
            if (tempData == null)
            {
                ReturnData.MessageType = enmMessageType.Error;
                ReturnData.Message = "Invalid Username or Password !!";
                return ReturnData;
            }
            else
            {
                this.UserId= tempData.user_id;
            }

            if (tempData.password != Password)
            {
                
                BlockUnblockUser(tempData.user_id, 1);
                ReturnData.MessageType = enmMessageType.Error;
                ReturnData.Message = "Invalid Username or Password !!";
                return ReturnData;
            }
            else if (tempData.is_active == 1)
            {
                ReturnData.MessageType = enmMessageType.Error;
                ReturnData.Message = "Invalid User";
                return ReturnData;
            }
            else if (tempData.is_logged_blocked == 1)
            {
                if (tempData.logged_blocked_Enddt < DateTime.Now)
                {
                    BlockUnblockUser(tempData.user_id, 0);
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
                UserId = tempData.user_id,
                NormalizedName = tempData.NormalizedName,
                employee_id = tempData.employee_id,
                user_type = tempData.user_type,
                CustomerId = tempData.CustomerId,
                DistributorId = tempData.DistributorId,
            };
            return ReturnData;
        }


        public void BlockUnblockUser(ulong UserId, byte is_logged_blocked)
        {
            bool AllowBlockonFail = false;
            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));
            int BlockUserAfterLoginFailAttempets = 10, BlockUserAfterLoginFailAttempetsForTime = 30;
            var tempData = _context.tbl_user_master.Where(p => p.user_id == UserId).FirstOrDefault();
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
            _context.tbl_user_master.Update(tempData);
            _context.SaveChanges();

        }


        public void SaveLoginLog( string IPAddress, string DeviceDetails, bool LoginStatus, string FromLocation,string Longitude,string Latitude)
        {
            if (this.UserId == null)
            {
                return;
            }

            _context.tblUserLoginLog.Add(new tblUserLoginLog()
            {
                user_id = UserId.Value,
                IPAddress = IPAddress,
                DeviceDetails = DeviceDetails,
                LoginStatus = LoginStatus,
                FromLocation = FromLocation,
                LoginDateTime = DateTime.Now,
                Longitude=Longitude,
                Latitude=Latitude
            });
            _context.SaveChanges();
        }


        public string GenrateTempUser(string IP, string DeviceId)
        {
            string tempUserID = Guid.NewGuid().ToString().Replace("-","");
            _context.tbl_guid_detail.Add(new tbl_guid_detail() { DeviceId = DeviceId, FromIP = IP, genration_dt = DateTime.Now, id = tempUserID });
            _context.SaveChanges();
            return tempUserID;
        }

        public bool IsTempUserIDExist(string TempUserID)
        {
            return _context.tbl_guid_detail.Where(p => p.id == TempUserID && p.genration_dt >= DateTime.Now.AddDays(-30)).Count() > 0 ? true : false;
        }


    }
}

using Microsoft.Extensions.Configuration;
using projContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projContext.DB;
using System.Text;
using projAPI.Model;

namespace projAPI.Services
{
    
    
    public interface IsrvSettings
    {
        mdlReturnData GenrateCaptcha(ulong UserId, string TempUserId, string LoginCaptchaExpiryTime);
        string GenrateCharcter(bool IsAlphanumeric, int NumberOfCharcter);
        string GetSettings(string SettingGroup, string SettingName);
        mdlReturnData ValidateCaptcha(string SecurityStampValue, string SecurityStamp);
    }

    public class srvSettings : IsrvSettings
    {
        private readonly Context _context;
        private readonly IConfiguration _config;
        public srvSettings(Context context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public string GetSettings(string SettingGroup, string SettingName)
        {
            string SettingValue = string.Empty;
            var DbSetting = _context.tbl_app_setting.Where(p => p.GroupName == SettingGroup && p.AppSettingKey == SettingName && p.is_active == 1).FirstOrDefault();
            if (DbSetting == null)
            {
                try
                {
                    SettingValue = _config[string.Concat("OrganisationSetting:", SettingGroup, ":", SettingName)];
                    _context.tbl_app_setting.Add(new tbl_app_setting()
                    {
                        AppSettingKey = SettingName,
                        AppSettingValue = SettingValue,
                        created_by = 1,
                        created_dt = DateTime.Now,
                        GroupName = SettingGroup,
                        last_modified_by = 1,
                        last_modified_date = DateTime.Now,
                        is_active = 1
                    });
                    _context.SaveChanges();
                }
                catch { }
            }
            else
            {
                SettingValue = DbSetting.AppSettingValue;
            }
            return SettingValue;
        }

        public string GenrateCharcter(bool IsAlphanumeric, int NumberOfCharcter)
        {
            const string AlphanumericLetters = "2346789ABCDEFGHJKLMNPRTUVWXYZ";
            const string Letters = "ABCDEFGHJKLMNPRTUVWXYZ";
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            if (!IsAlphanumeric)
            {
                int maxRand = Letters.Length - 1;
                for (int i = 0; i < NumberOfCharcter; i++)
                {
                    int index = rand.Next(maxRand);
                    sb.Append(Letters[index]);
                }
            }
            else
            {
                int maxRand = AlphanumericLetters.Length - 1;
                for (int i = 0; i < NumberOfCharcter; i++)
                {
                    int index = rand.Next(maxRand);
                    sb.Append(AlphanumericLetters[index]);
                }
            }
            return sb.ToString();
        }


        public mdlReturnData GenrateCaptcha(ulong UserId, string TempUserId, string LoginCaptchaExpiryTime)
        {
            mdlReturnData ReturnData = new mdlReturnData() { MessageType = enmMessageType.None };
            int LoginCaptchaExpiry = 30;
            int.TryParse(GetSettings("Captcha", LoginCaptchaExpiryTime), out LoginCaptchaExpiry);
            tblUserOTPValidation mdl = new tblUserOTPValidation()
            {
                UserId = UserId,
                TempUserId = TempUserId,
                EffectiveFromDt = DateTime.Now,
                EffectiveToDt = DateTime.Now.AddMinutes(LoginCaptchaExpiry),
                SecurityStamp = Convert.ToString(Guid.NewGuid()),
                SecurityStampValue = GenrateCharcter(true, 4),
            };
            _context.tblUserOTPValidation.Add(mdl);
            ReturnData.MessageType = enmMessageType.Success;
            ReturnData.ReturnId = new { EffectiveToDt = mdl.EffectiveToDt.ToString("dd-MMM-yyyy"), SecurityStamp = mdl.SecurityStamp, SecurityStampValue = mdl.SecurityStampValue };
            return ReturnData;
        }

        public mdlReturnData ValidateCaptcha(string SecurityStampValue, string SecurityStamp)
        {
            mdlReturnData ReturnData = new mdlReturnData() { MessageType = enmMessageType.None };
            var tempResult = _context.tblUserOTPValidation.Where(p => p.SecurityStamp == SecurityStamp && p.SecurityStampValue == SecurityStampValue && p.EffectiveToDt > DateTime.Now).FirstOrDefault();
            if (tempResult == null)
            {
                ReturnData.MessageType = enmMessageType.Error;
                ReturnData.Message = "Invalid Token";
            }
            else
            {
                ReturnData.MessageType = enmMessageType.Success;
                ReturnData.ReturnId = new { UserId = tempResult.UserId, TempUserId = tempResult.TempUserId };
            }
            return ReturnData;
        }

    }
}

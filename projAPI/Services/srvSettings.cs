using Microsoft.Extensions.Configuration;
using projContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projContext.DB;
using System.Text;
using projAPI.Model;
using System.Net;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace projAPI.Services
{
    
    


    public interface IsrvSettings
    {
        byte[] GenerateImage(int width, int height, string captchaCode);
        mdlReturnData GenrateCaptcha(ulong UserId, string TempUserId, string LoginCaptchaExpiryTime);
        string GenrateCharcter(bool IsAlphanumeric, int NumberOfCharcter);
        string GetClientIP(IHttpContextAccessor httpContext);
        string GetDeviceDetails(string IP);
        string GetSettings(string SettingGroup, string SettingName);
        mdlReturnData ValidateCaptcha(string SecurityStampValue, string SecurityStamp);
    }

    public class srvSettings :  IsrvSettings
    {
        private readonly Context _context;
        private readonly IConfiguration _config;
        public srvSettings(Context context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public string GetClientIP(IHttpContextAccessor httpContext)
        {
            string ip = Convert.ToString(httpContext.HttpContext.Connection.RemoteIpAddress);
            if (ip == "::1")
            {
                var addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                if (addressList.Length > 1)
                {
                    return Convert.ToString(addressList[1]);
                }
                else if (addressList.Length > 0)
                {
                    return Convert.ToString(addressList[0]);
                }

                return ip;
            }
            else
            {
                return ip;
            }
        }

        public string GetDeviceDetails(string IP)
        {
            return Dns.GetHostEntry(IP).HostName;
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
                SecurityStamp = Convert.ToString(Guid.NewGuid()).Replace("-",""),
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
                _context.tblUserOTPValidation.Remove(tempResult);
                _context.SaveChanges();
                ReturnData.MessageType = enmMessageType.Success;
                ReturnData.ReturnId = new { UserId = tempResult.UserId, TempUserId = tempResult.TempUserId };
            }
            return ReturnData;
        }

        public byte[] GenerateImage(int width, int height, string captchaCode)
        {
            using (Bitmap baseMap = new Bitmap(width, height))
            using (Graphics graph = Graphics.FromImage(baseMap))
            {
                Random rand = new Random();

                graph.Clear(GetRandomLightColor());

                DrawCaptchaCode();
                // DrawDisorderLine();
                // AdjustRippleEffect();

                MemoryStream ms = new MemoryStream();
                baseMap.Save(ms, ImageFormat.Png);
                return ms.ToArray();

                int GetFontSize(int imageWidth, int captchCodeCount)
                {
                    var averageSize = imageWidth / captchCodeCount;
                    return Convert.ToInt32(averageSize);
                }

                Color GetRandomDeepColor()
                {
                    int redlow = 160, greenLow = 100, blueLow = 160;
                    return Color.FromArgb(rand.Next(redlow), rand.Next(greenLow), rand.Next(blueLow));
                }

                Color GetRandomLightColor()
                {
                    int low = 180, high = 255;

                    int nRend = rand.Next(high) % (high - low) + low;
                    int nGreen = rand.Next(high) % (high - low) + low;
                    int nBlue = rand.Next(high) % (high - low) + low;

                    return Color.FromArgb(nRend, nGreen, nBlue);
                }

                void DrawCaptchaCode()
                {
                    SolidBrush fontBrush = new SolidBrush(Color.Black);
                    int fontSize = GetFontSize(width, captchaCode.Length);
                    Font font = new Font(FontFamily.GenericSerif, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                    for (int i = 0; i < captchaCode.Length; i++)
                    {
                        fontBrush.Color = GetRandomDeepColor();

                        int shiftPx = fontSize / 6;

                        float x = i * fontSize + rand.Next(-shiftPx, shiftPx) + rand.Next(-shiftPx, shiftPx);
                        int maxY = height - fontSize;
                        if (maxY < 0) maxY = 0;
                        float y = rand.Next(0, maxY);

                        graph.DrawString(captchaCode[i].ToString(), font, fontBrush, x, y);
                    }
                }


            }
        }


        public string Encrypte(string)
    }
}

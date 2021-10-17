using Microsoft.Extensions.Configuration;
using projContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projContext.DB;
using System.Text;

namespace projAPI.Services
{
    
    public interface IsrvSettings
    {
        string GenrateCharcter(bool IsAlphanumeric, int NumberOfCharcter);
        string GetSettings(string SettingGroup, string SettingName);
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
    }
}

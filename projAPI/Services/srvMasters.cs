using projAPI.Model;
using projContext.DB.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using projContext;
using System.Text;

namespace projAPI.Services
{
    
    
    public interface IsrvMasters
    {
        string GenrateCode(enmCodeGenrationType genrationType, string Prefix = "", string CountryCode = "", string StateCode = "", string CompanyCode = "", string ZoneCode = "", string LocationCode = "", int MonthYear = 0, int Year = 0, int YearWeek = 0, bool IncludeCountryCode = false, bool IncludeStateCode = false, bool IncludeCompanyCode = false, bool IncludeZoneCode = false, bool IncludeLocationCode = false, bool IncludeYear = false, bool IncludeMonthYear = false, bool IncludeYearWeek = false, byte DigitFormate = 5, int OrgId = 1, ulong UserId = 0);
        mdlCommonReturn GetCountry(int CountryId);
        List<mdlCommonReturn> GetCountry(int[] CountryIds);
        tblFileMaster GetImage(string FileId);
        List<tblFileMaster> GetImages(string[] FileIds);
        mdlCommonReturn GetOrganisation(string OrgCode);
        mdlCommonReturn GetState(int StateId);
        List<mdlCommonReturn> GetStates(int CountryId);
        List<mdlCommonReturn> GetStates(int[] StateId);
        string SetImage(IFormFile fromFile, enmFileType mimeType, ulong userId);
    }

    public class srvMasters : IsrvMasters
    {
        private readonly MasterContext _masterContext;
        public srvMasters(MasterContext masterContext)
        {
            _masterContext = masterContext;
        }
        public mdlCommonReturn GetState(int StateId)
        {
            if (StateId == 0)
            {
                return new mdlCommonReturn();
            }
            return _masterContext.tblState.Where(q => q.StateId == StateId).Select(p => new mdlCommonReturn()
            { Id = p.StateId, Code = p.Code, Name = p.Name }).FirstOrDefault();
        }



        public List<mdlCommonReturn> GetStates(int CountryId)
        {
            if (CountryId == 0)
            {
                return new List<mdlCommonReturn>();
            }
            return _masterContext.tblState.Where(q => q.CountryId == CountryId)
                .Select(p => new mdlCommonReturn()
                { Id = p.StateId, Code = p.Code, Name = p.Name }).ToList();
        }
        public List<mdlCommonReturn> GetStates(int[] StateId)
        {
            return _masterContext.tblState.Where(q => StateId.Contains(q.StateId))
                .Select(p => new mdlCommonReturn()
                { Id = p.StateId, Code = p.Code, Name = p.Name }).ToList();
        }

        public mdlCommonReturn GetCountry(int CountryId)
        {
            return _masterContext.tblCountry.Where(q => q.CountryId == CountryId)
                .Select(p => new mdlCommonReturn()
                { Id = p.CountryId, Code = p.Code, Name = p.Name }).FirstOrDefault();
        }

        public List<mdlCommonReturn> GetCountry(int[] CountryIds)
        {
            return _masterContext.tblCountry.Where(q => CountryIds.Contains(q.CountryId))
                .Select(p => new mdlCommonReturn()
                { Id = p.CountryId, Code = p.Code, Name = p.Name }).ToList();
        }

        public tblFileMaster GetImage(string FileId)
        {
            return _masterContext.tblFileMaster.Where(p => p.FileId == FileId).FirstOrDefault();
        }
        public List<tblFileMaster> GetImages(string[] FileIds)
        {
            return _masterContext.tblFileMaster.Where(p => FileIds.Contains(p.FileId)).ToList();
        }
        public string SetImage(IFormFile fromFile, enmFileType mimeType, ulong userId)
        {
            string FileName = null;
            DateTime dateTime = DateTime.Now;
            if (fromFile != null)
            {
                if (fromFile.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        fromFile.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        //string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                        FileName = Guid.NewGuid().ToString().Replace("-", "");
                        _masterContext.tblFileMaster.Add(new tblFileMaster()
                        {
                            FileId = FileName,
                            File = fileBytes,
                            CreatedBy = userId,
                            CreatedDt = dateTime,
                            ModifiedBy = userId,
                            ModifiedDt = dateTime,
                            FileType = mimeType
                        });
                        _masterContext.SaveChanges();
                    }
                }
            }
            return FileName;
        }

        public mdlCommonReturn GetOrganisation(string OrgCode)
        {
            if (string.IsNullOrEmpty(OrgCode))
            {
                return null;
            }
            return _masterContext.tblOrganisation.Where(q => q.Code == OrgCode).Select(p => new mdlCommonReturn()
            { Id = p.OrgId, Code = p.Code, Name = p.Name, IsActive = p.IsActive }).FirstOrDefault();
        }

        public string GenrateCode(enmCodeGenrationType genrationType, string Prefix = "", string CountryCode = "", string StateCode = "",
            string CompanyCode = "", string ZoneCode = "", string LocationCode = "", int MonthYear = 0, int Year = 0, int YearWeek = 0,
            bool IncludeCountryCode = false, bool IncludeStateCode = false, bool IncludeCompanyCode = false, bool IncludeZoneCode = false,
            bool IncludeLocationCode = false, bool IncludeYear = false, bool IncludeMonthYear = false,
            bool IncludeYearWeek = false, byte DigitFormate = 5, int OrgId = 1, ulong UserId = 0
            )
        {
            StringBuilder Code = new StringBuilder("");
            var data = _masterContext.tblCodeGenrationMaster.Where(p => p.CodeGenrationType == genrationType).FirstOrDefault();
            if (data == null)
            {
                data = new tblCodeGenrationMaster()
                {
                    CodeGenrationType = genrationType,
                    IncludeCountryCode = IncludeCountryCode,
                    IncludeStateCode = IncludeStateCode,
                    IncludeCompanyCode = IncludeCompanyCode,
                    IncludeZoneCode = IncludeZoneCode,
                    IncludeLocationCode = IncludeLocationCode,
                    IncludeYear = IncludeYear,
                    IncludeMonthYear = IncludeMonthYear,
                    IncludeYearWeek = IncludeYearWeek,
                    DigitFormate = DigitFormate,
                    OrgId = OrgId,
                    CreatedBy = UserId,
                    CreatedDt = DateTime.Now,
                    ModifiedBy = UserId,
                    ModifiedDt = DateTime.Now,
                };
                _masterContext.tblCodeGenrationMaster.Add(data);
                _masterContext.SaveChanges();
            }
            Code.Append(data.Prefix);
            IQueryable<tblCodeGenrationDetails> query = _masterContext.tblCodeGenrationDetails.Where(p => p.Id == data.Id).AsQueryable();
            if (data.IncludeCompanyCode)
            {
                query = query.Where(p => p.CompanyCode == CompanyCode);
            }
            if (data.IncludeZoneCode)
            {
                query = query.Where(p => p.ZoneCode == ZoneCode);
            }
            if (data.IncludeLocationCode)
            {
                query = query.Where(p => p.LocationCode == LocationCode);
            }
            if (data.IncludeCountryCode)
            {
                query = query.Where(p => p.CountryCode == CountryCode);
            }
            if (data.IncludeStateCode)
            {
                query = query.Where(p => p.StateCode == StateCode);
            }
            if (data.IncludeYear)
            {
                query = query.Where(p => p.Year == Year);
            }
            if (data.IncludeMonthYear)
            {
                query = query.Where(p => p.MonthYear == MonthYear);
            }
            if (data.IncludeYearWeek)
            {
                query = query.Where(p => p.YearWeek == YearWeek);
            }
            var details = query.FirstOrDefault();
            if (details == null)
            {
                details = new tblCodeGenrationDetails()
                {
                    Id = data.Id,
                    CountryCode = CountryCode,
                    StateCode = StateCode,
                    CompanyCode = CompanyCode,
                    ZoneCode = ZoneCode,
                    LocationCode = LocationCode,
                    MonthYear = MonthYear,
                    Year = Year,
                    YearWeek = YearWeek,
                    Counter = 1,
                    ModifiedDt = DateTime.Now
                };
                _masterContext.tblCodeGenrationDetails.Add(details);
                _masterContext.SaveChanges();
            }

            if (data.IncludeCompanyCode)
            {
                Code.Append(details.CompanyCode);
            }
            if (data.IncludeZoneCode)
            {
                Code.Append(details.ZoneCode);
            }
            if (data.IncludeLocationCode)
            {
                Code.Append(details.LocationCode);
            }
            if (data.IncludeCountryCode)
            {
                Code.Append(details.CountryCode);
            }
            if (data.IncludeStateCode)
            {
                Code.Append(details.StateCode);
            }
            if (data.IncludeYear)
            {
                Code.Append(details.Year);
            }
            if (data.IncludeMonthYear)
            {
                Code.Append(details.MonthYear);
            }
            if (data.IncludeYearWeek)
            {
                Code.Append(details.YearWeek);
            }
            Code.Append(details.Counter.ToString("d" + data.DigitFormate));
            details.Counter = details.Counter + 1;
            details.ModifiedDt = DateTime.Now;
            _masterContext.tblCodeGenrationDetails.Update(details);
            _masterContext.SaveChanges();
            return Code.ToString();
        }

    }
}

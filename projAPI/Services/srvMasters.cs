using projAPI.Model;
using projContext.DB.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using projContext;

namespace projAPI.Services
{
    

    public interface IsrvMasters
    {
        mdlCommonReturn GetCountry(int CountryId);
        List<mdlCommonReturn> GetCountry(int[] CountryIds);
        tblFileMaster GetImage(string FileId);
        List<tblFileMaster> GetImages(string[] FileIds);
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
            if (fromFile == null)
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


    }
}

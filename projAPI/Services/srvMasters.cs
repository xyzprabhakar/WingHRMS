using projAPI.Model;
using projContext.DB.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Services
{
    
    public interface IsrvMasters
    {
        mdlCommonReturn GetCountry(int CountryId);
        List<mdlCommonReturn> GetCountry(int[] CountryIds);
        mdlCommonReturn GetState(int StateId);
        List<mdlCommonReturn> GetStates(int CountryId);
        List<mdlCommonReturn> GetStates(int[] StateId);
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
            if (StateId==0)
            {
                return new mdlCommonReturn();
            }
            return _masterContext.tblState.Where(q => q.StaeId == StateId).Select(p => new mdlCommonReturn()
            { Id = p.StaeId, Code = p.Code, Name = p.Name }).FirstOrDefault();
        }
        public List<mdlCommonReturn> GetStates(int CountryId)
        {
            if (CountryId == 0)
            {
                return new List<mdlCommonReturn>();
            }
            return _masterContext.tblState.Where(q => q.CountryId == CountryId)
                .Select(p => new mdlCommonReturn()
                { Id = p.StaeId, Code = p.Code, Name = p.Name }).ToList();
        }
        public List<mdlCommonReturn> GetStates(int[] StateId)
        {
            return _masterContext.tblState.Where(q => StateId.Contains(q.StaeId))
                .Select(p => new mdlCommonReturn()
                { Id = p.StaeId, Code = p.Code, Name = p.Name }).ToList();
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


    }
}

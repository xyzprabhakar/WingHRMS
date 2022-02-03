using projAPI.Model;
using projContext;
using projContext.DB.CRM;
using projContext.DB.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Services
{
    

    public interface IsrvCustomer
    {
        bool CustomerContactNoExists(string ContactNo, int Customerid, int OrgId);
        bool CustomerEmailExists(string Email, int Customerid, int OrgId);
        tblCustomerMaster GetCustomer(int OrgId, string CustomerCode);
        List<mdlCommonReturn> GetCustomers(int OrgId, List<string> CustomerCodes);
        List<mdlCommonReturn> GetCustomers(int OrgId, List<int> CustomerId);
        mdlReturnData SetCustomerMaster(mdlCustomer mdl, ulong UserId);
    }

    public class srvCustomer : IsrvCustomer
    {
        private readonly CrmContext _crmContext;
        public srvCustomer(CrmContext crmContext)
        {
            _crmContext = crmContext;
        }

        //public List<mdlCustomer> GetCustomers(OrgId)
        //{ 
        //}

        public tblCustomerMaster GetCustomer(int OrgId, string CustomerCode)
        {
            if (string.IsNullOrEmpty(CustomerCode))
            {
                return null;
            }
            DateTime currentDate = DateTime.Now;
            var Data = _crmContext.tblCustomerMaster.Where(p => p.OrgId == OrgId && p.Code == CustomerCode).FirstOrDefault();
            if (Data != null)
            {
                if (Data.IsActive && Data.EffectiveFromDt <= currentDate
                && Data.EffectiveToDt >= currentDate)
                {
                    Data.IsActive = true;
                }
                else
                {
                    Data.IsActive = false;
                }
            }
            return Data;
        }

        public List<mdlCommonReturn> GetCustomers(int OrgId, List<string> CustomerCodes)
        {
            if (CustomerCodes == null)
            {
                return new List<mdlCommonReturn>();
            }
            if (CustomerCodes.Count == 0)
            {
                return new List<mdlCommonReturn>();
            }
            DateTime currentDate = DateTime.Now;
            var Data = _crmContext.tblCustomerMaster.Where(p => p.OrgId == OrgId && CustomerCodes.Contains(p.Code)).
                Select(p => new mdlCommonReturn { Id = p.CustomerId, Code = p.Code, Name = p.Name, IsActive = p.IsActive }).ToList();
            return Data;
        }

        public List<mdlCommonReturn> GetCustomers(int OrgId, List<int> CustomerId)
        {
            
            if (CustomerId == null)
            {
                return new List<mdlCommonReturn>();
            }
            if (CustomerId.Count == 0)
            {
                return new List<mdlCommonReturn>();
            }
            DateTime currentDate = DateTime.Now;
            var Data = _crmContext.tblCustomerMaster.Where(p =>  p.OrgId == OrgId && CustomerId.Contains(p.CustomerId)).
                Select(p=>new  mdlCommonReturn{ Id=p.CustomerId, Code=p.Code, Name=p.Name,IsActive=p.IsActive }).ToList();
            return Data;
        }

        public bool CustomerEmailExists(string Email, int Customerid,int OrgId)
        {
            return _crmContext.tblCustomerMaster.Where(p => p.Email == Email && p.CustomerId != Customerid).Count() > 0;
        }
        public bool CustomerContactNoExists(string ContactNo, int Customerid, int OrgId)
        {
            return _crmContext.tblCustomerMaster.Where(p => p.ContactNo == ContactNo && p.CustomerId != Customerid).Count() > 0;
        }

        public mdlReturnData SetCustomerMaster(mdlCustomer mdl, ulong UserId)
        {
            mdlReturnData returnData = new mdlReturnData();
            bool IsUpdate = false;

            if (_crmContext.tblCustomerMaster.Where(p => p.OrgId == mdl.OrgId && p.Code == mdl.Code && p.CustomerId != mdl.CustomerId).Count() > 0)
            {
                returnData.Message = "Code already exists";
                returnData.MessageType = enmMessageType.Error;
                return returnData;
            }
            if (CustomerEmailExists(mdl.Email, mdl.CustomerId, mdl.OrgId))
            {
                returnData.Message = "Email already exists";
                returnData.MessageType = enmMessageType.Error;
                return returnData;
            }
            if (CustomerContactNoExists(mdl.ContactNo, mdl.CustomerId, mdl.OrgId))
            {
                returnData.Message = "Contact No already exists";
                returnData.MessageType = enmMessageType.Error;
                return returnData;
            }
            tblCustomerMaster tbl = null;
            if (mdl.CustomerId > 0)
            {
                tbl = _crmContext.tblCustomerMaster.Where(p => p.OrgId == mdl.OrgId && p.CustomerId == mdl.CustomerId).FirstOrDefault();
                if (tbl == null)
                {
                    returnData.Message = "Data not exists";
                    returnData.MessageType = enmMessageType.Error;
                    return returnData;
                }
                IsUpdate = true;

            }
            else
            {
                tbl = new tblCustomerMaster();
                tbl.CreatedBy = UserId;
                tbl.CreatedDt = DateTime.Now;

            }

            tbl.CustomerType = mdl.CustomerType;
            tbl.Code = mdl.Code;
            tbl.Name = mdl.Name;
            tbl.OrgId = mdl.OrgId;
            tbl.OfficeAddress = mdl.OfficeAddress;
            tbl.Locality = mdl.Locality;
            tbl.City = mdl.City;
            tbl.StateId = mdl.StateId;
            tbl.CountryId = mdl.CountryId;
            tbl.Pincode = mdl.Pincode;
            tbl.Email = mdl.Email;
            tbl.AlternateEmail = mdl.AlternateEmail;
            tbl.ContactNo = mdl.ContactNo;
            tbl.AlternateContactNo = mdl.AlternateContactNo;
            tbl.ModifyRemarks = mdl.ModifyRemarks;
            tbl.IsActive = mdl.IsActive;
            if (mdl.NewFileName != null && tbl.Logo != mdl.NewFileName)
            {
                tbl.Logo = mdl.NewFileName;
            }
            tbl.ModifiedBy = UserId;
            tbl.ModifiedDt = DateTime.Now;
            tbl.EffectiveFromDt = mdl.EffectiveFromDt;
            tbl.EffectiveToDt = mdl.EffectiveToDt;
            if (IsUpdate)
            {
                _crmContext.tblCustomerMaster.Update(tbl);
            }
            else
            {
                _crmContext.tblCustomerMaster.Add(tbl);
            }
            _crmContext.SaveChanges();
            returnData.ReturnId = tbl.CustomerId;
            mdl.CustomerId = tbl.CustomerId;
            returnData.MessageType = enmMessageType.Success;
            return returnData;
        }


    }
}

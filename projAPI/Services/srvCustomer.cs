using projAPI.Model;
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
        tblCustomerMaster GetCustomer(int OrgId, string CustomerCode);
    }

    public class srvCustomer : IsrvCustomer
    {
        private readonly CrmContext _crmContext;
        public srvCustomer(CrmContext crmContext)
        {
            _crmContext = crmContext;
        }
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
    }
}

using projContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Services
{
    public interface IsrvDistributer
    {
        bool IsActiveDistributerExistsByNid(ulong DistributerId);
    }

    public class srvDistributer : IsrvDistributer
    {
        private readonly Context _context;
        private readonly IsrvSettings _IsrvSettings;

        public srvDistributer(Context context, IsrvSettings isrvSettings)
        {
            _context = context;
            _IsrvSettings = isrvSettings;
        }

        public bool IsActiveDistributerExistsByNid(ulong DistributerId)
        {
            return _context.tblDistributorMaster.Where(p => !p.IsTerminate && p.Nid == DistributerId).Count() > 0 ? true : false;
        }

    }
}

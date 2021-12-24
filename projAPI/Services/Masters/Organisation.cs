using projContext.DB.Masters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Services.Masters
{
    public class srvOrganisation
    {
        private readonly MasterContext _MasterContext;
        public srvOrganisation(MasterContext MasterContext)
        {
            _MasterContext = MasterContext;
        }
        
    }
}

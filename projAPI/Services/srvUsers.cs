using projContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Services
{
    public class srvUsers
    {
        private readonly Context _context;
        private readonly IsrvSettings _IsrvSettings;
        public srvUsers(Context context, IsrvSettings isrvSettings)
        {
            _context = context;
            _IsrvSettings =isrvSettings;
        }
        public bool ValidateUser(string UserName,string Password,int UserType)
        {
            return true;
        }
    }
}

using projAPI.Model;
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
        public mdlReturnData GetUser(string UserName,string Password)
        {
            throw new NotImplementedException();
            //return true;
            //_context.tbl_user_master.Where(p => p.username == UserName && p.password == Password);
        }


    }
}

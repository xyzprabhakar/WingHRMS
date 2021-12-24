using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace projContext.DB.Masters
{
    public class MasterContext : DbContext
    {
        //add-migration 31_Oct_20211 -Context projContext.DB.Masters.MasterContext
        //update-database -Context projContext.DB.Masters.MasterContext
        public MasterContext(DbContextOptions<Context> options) : base(options)
        {
            
        }

        #region ***************** Organisation *********************
        public DbSet<tblOrganisation> tblOrganisation { get; set; }
        public DbSet<tblCompanyMaster> tblCompanyMaster { get; set; }
        public DbSet<tblZoneMaster> tblZoneMaster { get; set; }
        public DbSet<tblLocationMaster> tblLocationMaster { get; set; }
        #endregion
    }
}

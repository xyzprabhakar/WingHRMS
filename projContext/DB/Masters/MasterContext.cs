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

        #region ********************** Masters **************************

        public DbSet<tblCountry> tblCountry { get; set; }
        public DbSet<tblState> tblState { get; set; }
        public DbSet<tblBankMaster> tblBankMaster { get; set; }
        public DbSet<tblCurrency> tblCurrency { get; set; }        
        #endregion

        public DbSet<tblFileMaster> tblFileMaster { get; set; }
    }
}

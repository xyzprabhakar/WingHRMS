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
        public MasterContext(DbContextOptions<MasterContext> options) : base(options)
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

        #region ******************* Users *************************
        public DbSet<tblUsersMaster> tblUsersMaster { get; set; }
        public DbSet<tblRoleMaster> tblRoleMaster { get; set; }
        public DbSet<tblRoleClaim> tblRoleClaim { get; set; }
        public DbSet<tblUserClaim> tblUserClaim { get; set; }
        public DbSet<tblUserAllClaim> tblUserAllClaim { get; set; }
        public DbSet<tblUserOTP> tblUserOTP { get; set; }
        public DbSet<tblUserLoginLog> tblUserLoginLog { get; set; }
        public DbSet<tblUserOrganisationPermission> tblUserOrganisationPermission { get; set; }
        public DbSet<tblUserCompanyPermission> tblUserCompanyPermission { get; set; }
        public DbSet<tblUserZonePermission> tblUserZonePermission { get; set; }
        public DbSet<tblUserLocationPermission> tblUserLocationPermission { get; set; }
        public DbSet<tblUserAllLocationPermission> tblUserAllLocationPermission { get; set; }
        #endregion
    }
}

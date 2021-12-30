using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace projContext.DB.CRM
{
    public class CrmContext : DbContext
    {
        //add-migration -s projApi CrmMaster -Context projContext.DB.CRM.CrmContext -o Migrations.CRM
        //update-database -s projApi -Context projContext.DB.CRM.CrmContext

        public CrmContext(DbContextOptions<CrmContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        #region ******************************* Customer Setting ***************************
        public DbSet<tblCustomerMaster> tblCustomerMaster { get; set; }
        #endregion

        #region ******************************* Customer Setting ***************************
        public DbSet<tblCustomerIPFilter> tblCustomerIPFilter { get; set; }
        public DbSet<tblCustomerIPFilterDetails> tblCustomerIPFilterDetails { get; set; }
        public DbSet<tblCustomerMarkup> tblCustomerMarkup { get; set; }
        public DbSet<tblWalletBalanceAlert> tblWalletBalanceAlert { get; set; }
        public DbSet<tblCustomerWalletAmount> tblCustomerWalletAmount { get; set; }
        public DbSet<tblWalletDetailLedger> tblWalletDetailLedger { get; set; }
        public DbSet<tblPaymentRequest> tblPaymentRequest { get; set; }
        public DbSet<tblCustomerNotification> tblCustomerNotification { get; set; }        
        #endregion
    }
}

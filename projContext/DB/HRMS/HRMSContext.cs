using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace projContext.DB.HRMS
{
    public class HRMSContext : DbContext
    {
        //add-migration -s projApi 31_Oct_20211 -Context projContext.DB.HRMS.HRMSContext
        //update-database -s projApi -Context projContext.DB.HRMS.HRMSContext
        public HRMSContext(DbContextOptions<HRMSContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<tblHolidayMaster>().HasIndex(c => new { c.OrgId });
            modelBuilder.Entity<tblDepartment>().HasIndex(c => new { c.OrgId });
            modelBuilder.Entity<tblDepartWorkingRole>().HasIndex(c => new { c.OrgId });
            modelBuilder.Entity<tblGrade>().HasIndex(c => new { c.OrgId });
            modelBuilder.Entity<tblDesignation>().HasIndex(c => new { c.OrgId });
            modelBuilder.Entity<tblReligionMaster>().HasIndex(c => new { c.OrgId });
        }
        public DbSet<tblHolidayMaster> tblHolidayMaster { get; set; }
        public DbSet<tblDepartment> tblDepartment { get; set; }
        public DbSet<tblDepartWorkingRole> tblDepartWorkingRole { get; set; }
        public DbSet<tblGrade> tblGrade { get; set; }
        public DbSet<tblDesignation> tblDesignation { get; set; }
        public DbSet<tblReligionMaster> tblReligionMaster { get; set; }
        
    }
}

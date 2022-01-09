using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace projContext.DB.PigyRent
{
    public  class PigyContext : DbContext
    {
        //add-migration -s projApi 31_Oct_20211 -Context projContext.DB.HRMS.PigyContext
        //update-database -s projApi -Context projContext.DB.HRMS.PigyContext
        public PigyContext(DbContextOptions<PigyContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
        }
        public DbSet<tblCategory> tblCategory { get; set; }
        public DbSet<tblSubCategory> tblSubCategory { get; set; }
        public DbSet<tblCategoryAttribute> tblCategoryAttribute { get; set; }
        public DbSet<tblCategoryKeywords> tblCategoryKeywords { get; set; }
        public DbSet<tblProduct> tblProduct { get; set; }
        public DbSet<tblProductPrice> tblProductPrice { get; set; }
        public DbSet<tblProductBooking> tblProductBooking { get; set; }
        public DbSet<tblProductRating> tblProductRating { get; set; }
        public DbSet<tblProductBookingChat> tblProductBookingChat { get; set; }
        public DbSet<tblProductDescription> tblProductDescription { get; set; }
        public DbSet<tblProductImage> tblProductImage { get; set; }
        public DbSet<tblProductAttributeValue> tblProductAttributeValue { get; set; }
        public DbSet<tblProductKeyword> tblProductKeyword { get; set; }
        public DbSet<tblProductAddress> tblProductAddress { get; set; }
        
            
    }
}

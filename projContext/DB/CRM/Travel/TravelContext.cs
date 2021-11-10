using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace projContext.DB.CRM.Travel
{
    public class TravelContext : DbContext
    {
        //add-migration OrganizationMaster -Context projContext.DB.CRM.Travel.TravelContext -o Migrations.Travel1
        //update-database -Context projContext.DB.CRM.Travel.TravelContext


        public TravelContext(DbContextOptions<TravelContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        #region *********************************** Air Services *****************************
        public DbSet<tblAirline> tblAirline { get; set; }
        public DbSet<tblFlightClassOfBooking> tblFlightClassOfBooking { get; set; }
        public DbSet<tblAirport> tblAirport { get; set; }
        public DbSet<tblFlightCustomerMarkup> tblFlightCustomerMarkup { get; set; }
        public DbSet<tblFlightMarkupMaster> tblFlightMarkupMaster { get; set; }
        public DbSet<tblFlightMarkupServiceProvider> tblFlightMarkupServiceProvider { get; set; }
        public DbSet<tblFlightMarkupCustomerType> tblFlightMarkupCustomerType { get; set; }
        public DbSet<tblFlightMarkupCustomerDetails> tblFlightMarkupCustomerDetails { get; set; }
        public DbSet<tblFlightMarkupPassengerType> tblFlightMarkupPassengerType { get; set; }
        public DbSet<tblFlightMarkupFlightClass> tblFlightMarkupFlightClass { get; set; }
        public DbSet<tblFlightMarkupAirline> tblFlightMarkupAirline { get; set; }
        public DbSet<tblFlightConvenience> tblFlightConvenience { get; set; }
        public DbSet<tblFlightConvenienceServiceProvider> tblFlightConvenienceServiceProvider { get; set; }
        public DbSet<tblFlightConvenienceCustomerType> tblFlightConvenienceCustomerType { get; set; }
        public DbSet<tblFlightConvenienceCustomerDetails> tblFlightConvenienceCustomerDetails { get; set; }
        public DbSet<tblFlightConveniencePassengerType> tblFlightConveniencePassengerType { get; set; }
        public DbSet<tblFlightConvenienceFlightClass> tblFlightConvenienceFlightClass { get; set; }
        public DbSet<tblFlightConvenienceAirline> tblFlightConvenienceAirline { get; set; }
        public DbSet<tblFlightDiscount> tblFlightDiscount { get; set; }
        public DbSet<tblFlightDiscountServiceProvider> tblFlightDiscountServiceProvider { get; set; }
        public DbSet<tblFlightDiscountCustomerType> tblFlightDiscountCustomerType { get; set; }
        public DbSet<tblFlightDiscountCustomerDetails> tblFlightDiscountCustomerDetails { get; set; }
        public DbSet<tblFlightDiscountPassengerType> tblFlightDiscountPassengerType { get; set; }
        public DbSet<tblFlightDiscountFlightClass> tblFlightDiscountFlightClass { get; set; }
        public DbSet<tblFlightDiscountAirline> tblFlightDiscountAirline { get; set; }

        #region ************************* Caching **************************
        public DbSet<tblFlightSearchRequest_Caching> tblFlightSearchRequest_Caching { get; set; }        
        public DbSet<tblFlightSearchResponses_Caching> tblFlightSearchResponses_Caching { get; set; }
        public DbSet<tblFlightSearchSegment_Caching> tblFlightSearchSegment_Caching { get; set; }
        public DbSet<tblFlightFare_Caching> tblFlightFare_Caching { get; set; }
        public DbSet<tblFlightFareDetail_Caching> tblFlightFareDetail_Caching { get; set; }
        #endregion

        #region ************************* Flight Booking **************************
        public DbSet<tblFlightBookingMaster> tblFlightBookingMaster { get; set; }
        public DbSet<tblFlightBookingSearchDetails> tblFlightBookingSearchDetails { get; set; }
        public DbSet<tblFlightFareMarkupDetail> tblFlightFareMarkupDetail { get; set; }
        public DbSet<tblFlightFareMLMMarkup> tblFlightFareMLMMarkup { get; set; }
        public DbSet<tblFlightFareDiscount> tblFlightFareDiscount { get; set; }
        public DbSet<tblFlightFareConvenience> tblFlightFareConvenience { get; set; }
        public DbSet<tblFlightSearchSegment> tblFlightSearchSegment { get; set; }
        public DbSet<tblFlightServices> tblFlightServices { get; set; }
        public DbSet<tblFlightFareDetail> tblFlightFareDetail { get; set; }
        public DbSet<tblFlightFareDetailMarkupDetail> tblFlightFareDetailMarkupDetail { get; set; }
        public DbSet<tblFlightFareDetailMLMMarkup> tblFlightFareDetailMLMMarkup { get; set; }
        public DbSet<tblFlightFareDetailDiscount> tblFlightFareDetailDiscount { get; set; }
        public DbSet<tblFlightFareDetailConvenience> tblFlightFareDetailConvenience { get; set; }
        public DbSet<tblFlighBookingPassengerDetails> tblFlighBookingPassengerDetails { get; set; }
        public DbSet<tblFlightRefundStatusDetails> tblFlightRefundStatusDetails { get; set; }        
        public DbSet<tblFlighRefundPassengerDetails> tblFlighRefundPassengerDetails { get; set; }
        #endregion

        #region ****************************Setting ***********************
        public DbSet<tblFlightSerivceProvider> tblFlightSerivceProvider { get; set; }
        public DbSet<tblFlightInstantBooking> tblFlightInstantBooking { get; set; }
        public DbSet<tblFlightSerivceProviderPriority> tblFlightSerivceProviderPriority { get; set; }
        public DbSet<tblFlightBookingAlterMaster> tblFlightBookingAlterMaster { get; set; }
        public DbSet<tblFlightBookingAlterDetails> tblFlightBookingAlterDetails { get; set; }
        public DbSet<tblFlightFareFilter> tblFlightFareFilter { get; set; }
        public DbSet<tblFlightFareFilterDetails> tblFlightFareFilterDetails { get; set; }
        #endregion

        #endregion
    }
}

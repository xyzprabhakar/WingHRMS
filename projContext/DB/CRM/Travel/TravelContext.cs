using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace projContext.DB.CRM.Travel
{
    public class TravelContext:  DbContext
    {



        #region *********************************** Air Services *****************************
        public DbSet<tblAirline> tblAirline { get; set; }
        public DbSet<tblFlightClassOfBooking> tblFlightClassOfBooking { get; set; }
        public DbSet<tblAirport> tblAirport { get; set; }
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
        public DbSet<tblFlightSearchResponse_Caching> tblFlightSearchResponse_Caching { get; set; }
        public DbSet<tblFlightSearchResponses_Caching> tblFlightSearchResponses_Caching { get; set; }
        public DbSet<tblFlightSearchSegment_Caching> tblFlightSearchSegment_Caching { get; set; }
        public DbSet<tblFlightFare_Caching> tblFlightFare_Caching { get; set; }
        public DbSet<tblFlightFareDetail_Caching> tblFlightFareDetail_Caching { get; set; }
        #endregion

        #region ************************* Flight Booking **************************
        public DbSet<tblFlightBookingMaster> tblFlightBookingMaster { get; set; }
        public DbSet<tblFlilghtBookingSearchDetails> tblFlilghtBookingSearchDetails { get; set; }
        public DbSet<tblFlightRefundStatusDetails> tblFlightRefundStatusDetails { get; set; }
        public DbSet<tblFlightPurchaseDetails> tblFlightPurchaseDetails { get; set; }
        public DbSet<tblFlighBookingPassengerDetails> tblFlighBookingPassengerDetails { get; set; }
        public DbSet<tblFlighRefundPassengerDetails> tblFlighRefundPassengerDetails { get; set; }
        #endregion
        #endregion
    }
}

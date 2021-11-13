using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB.CRM.Travel
{
    #region ********************* Common Class ******************************
    public class DbWingCharge : d_ApprovedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public enmFlightSearvices Applicability { get; set; }
        public bool IsAllProvider { get; set; }
        public bool IsAllCustomerType { get; set; }
        public bool IsAllCustomer { get; set; }
        public bool IsAllPessengerType { get; set; }//Applicable For All Pasenger
        public bool IsAllFlightClass { get; set; }
        public bool IsAllAirline { get; set; }
        public bool IsAllSegment { get; set; }
        public bool IsMLMIncentive { get; set; }
        public enmFlightType FlightType { get; set; }
        public enmGender Gender { get; set; }
        public bool IsPercentage { get; set; }
        public double PercentageValue { get; set; }
        public double Amount { get; set; }
        public double AmountCaping { get; set; }
        public DateTime TravelFromDt { get; set; }
        public DateTime TravelToDt { get; set; }
        public DateTime BookingFromDt { get; set; }
        public DateTime BookingToDt { get; set; }
        public bool IsDeleted { get; set; }
    }


    public class DbWingServiceProvider
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual int? ChargeId { get; set; }
        public enmServiceProvider ServiceProvider { get; set; }
    }

    public class DbWingCustomerType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual int? ChargeId { get; set; }
        public enmCustomerType customerType { get; set; }
    }

    public class DbWingCustomerDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual int? ChargeId { get; set; }
        [ForeignKey("tblCustomerMaster")] // Foreign Key here
        public  int? CustomerId { get; set; }
        public tblCustomerOrganisation tblCustomerMaster { get; set; }
        

    }

    public class DbWingPassengerType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual int? ChargeId { get; set; }
        public enmPassengerType PassengerType { get; set; }
    }


    public class DbWingFlightClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual int? ChargeId { get; set; }
        public enmCabinClass CabinClass { get; set; }
    }

    public class DbWingAirline
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual int? ChargeId { get; set; }
        [ForeignKey("tblAirline")] // Foreign Key here
        public int? AirlineId { get; set; }
        public tblAirline tblAirline { get; set; }
    }
    public class DbWingSegment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual int? ChargeId { get; set; }
        public string orign { get; set; }
        public string destination { get; set; }

    }

    public class DbWingFare
    {
        public double BaseFare { get; set; }
        public double NetFare { get; set; }
        public double Tax { get; set; }
        public double YQTax { get; set; }

    }

    #endregion 


    public class tblAirline : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Code { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        public bool isLcc { get; set; }
        [MaxLength(500)]
        public string ImagePath { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class tblFlightClassOfBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingClassId { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(20)]
        public string GenerlizedName { get; set; }//SME, FlexPlue, Corporate
    }

    public class tblAirport : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(200)]
        public string AirportCode { get; set; }
        [MaxLength(200)]
        public string AirportName { get; set; }
        [MaxLength(200)]
        public string Terminal { get; set; }
        [MaxLength(200)]
        public string CityCode { get; set; }
        [MaxLength(200)]
        public string CityName { get; set; }
        [MaxLength(200)]
        public string CountryCode { get; set; }
        [MaxLength(200)]
        public string CountryName { get; set; }
        public bool IsDomestic { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

    }


    #region *************** Markup, Discount, Convenience ***************************

    public class tblFlightCustomerMarkup : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public ulong Nid { get; set; }
        public double MarkupAmount { get; set; }
        public DateTime EffectiveFromDt { get; set; }
        public DateTime EffectiveToDt { get; set; }
        public bool IsDeleted { get; set; }

    }

    public class tblFlightMarkupMaster : DbWingCharge
    {
        [InverseProperty("tblFlightMarkupMaster")]
        public ICollection<tblFlightMarkupServiceProvider> tblFlightMarkupServiceProvider { get; set; }
        [InverseProperty("tblFlightMarkupMaster")]
        public ICollection<tblFlightMarkupCustomerType> tblFlightMarkupCustomerType { get; set; }
        [InverseProperty("tblFlightMarkupMaster")]
        public ICollection<tblFlightMarkupCustomerDetails> tblFlightMarkupCustomerDetails { get; set; }
        [InverseProperty("tblFlightMarkupMaster")]
        public ICollection<tblFlightMarkupPassengerType> tblFlightMarkupPassengerType { get; set; }
        [InverseProperty("tblFlightMarkupMaster")]
        public ICollection<tblFlightMarkupFlightClass> tblFlightMarkupFlightClass { get; set; }
        [InverseProperty("tblFlightMarkupMaster")]
        public ICollection<tblFlightMarkupAirline> tblFlightMarkupAirline { get; set; }
        [InverseProperty("tblFlightMarkupMaster")]
        public ICollection<tblFlightMarkupSegment> tblFlightMarkupSegment { get; set; }
    }


    public class tblFlightMarkupServiceProvider : DbWingServiceProvider
    {
        [ForeignKey("tblFlightMarkupMaster")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightMarkupMaster tblFlightMarkupMaster { get; set; }
    }
    public class tblFlightMarkupCustomerType : DbWingCustomerType
    {
        [ForeignKey("tblFlightMarkupMaster")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightMarkupMaster tblFlightMarkupMaster { get; set; }
    }
    public class tblFlightMarkupCustomerDetails : DbWingCustomerDetails
    {
        [ForeignKey("tblFlightMarkupMaster")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightMarkupMaster tblFlightMarkupMaster { get; set; }

    }
    public class tblFlightMarkupPassengerType : DbWingPassengerType
    {
        [ForeignKey("tblFlightMarkupMaster")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightMarkupMaster tblFlightMarkupMaster { get; set; }
    }

    public class tblFlightMarkupFlightClass : DbWingFlightClass
    {
        [ForeignKey("tblFlightMarkupMaster")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightMarkupMaster tblFlightMarkupMaster { get; set; }
    }
    public class tblFlightMarkupAirline : DbWingAirline
    {
        [ForeignKey("tblFlightMarkupMaster")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightMarkupMaster tblFlightMarkupMaster { get; set; }
    }

    public class tblFlightMarkupSegment : DbWingSegment
    {
        [ForeignKey("tblFlightMarkupMaster")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightMarkupMaster tblFlightMarkupMaster { get; set; }
    }

    public class tblFlightConvenience : DbWingCharge
    {
        [InverseProperty("tblFlightConvenience")]
        public ICollection<tblFlightConvenienceServiceProvider> tblFlightConvenienceServiceProvider { get; set; }
        [InverseProperty("tblFlightConvenience")]
        public ICollection<tblFlightConvenienceCustomerType> tblFlightConvenienceCustomerType { get; set; }
        [InverseProperty("tblFlightConvenience")]
        public ICollection<tblFlightConvenienceCustomerDetails> tblFlightConvenienceCustomerDetails { get; set; }
        [InverseProperty("tblFlightConvenience")]
        public ICollection<tblFlightConveniencePassengerType> tblFlightConveniencePassengerType { get; set; }
        [InverseProperty("tblFlightConvenience")]
        public ICollection<tblFlightConvenienceFlightClass> tblFlightConvenienceFlightClass { get; set; }
        [InverseProperty("tblFlightConvenience")]
        public ICollection<tblFlightConvenienceAirline> tblFlightConvenienceAirline { get; set; }
        [InverseProperty("tblFlightConvenience")]
        public ICollection<tblFlightConvenienceSegment> tblFlightConvenienceSegment { get; set; }
    }


    public class tblFlightConvenienceServiceProvider : DbWingServiceProvider
    {
        [ForeignKey("tblFlightConvenience")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightConvenience tblFlightConvenience { get; set; }
    }
    public class tblFlightConvenienceCustomerType : DbWingCustomerType
    {
        [ForeignKey("tblFlightConvenience")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightConvenience tblFlightConvenience { get; set; }
    }
    public class tblFlightConvenienceCustomerDetails : DbWingCustomerDetails
    {
        [ForeignKey("tblFlightConvenience")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightConvenience tblFlightConvenience { get; set; }

    }
    public class tblFlightConveniencePassengerType : DbWingPassengerType
    {
        [ForeignKey("tblFlightConvenience")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightConvenience tblFlightConvenience { get; set; }
    }

    public class tblFlightConvenienceFlightClass : DbWingFlightClass
    {
        [ForeignKey("tblFlightConvenience")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightConvenience tblFlightConvenience { get; set; }
    }
    public class tblFlightConvenienceAirline : DbWingAirline
    {
        [ForeignKey("tblFlightConvenience")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightConvenience tblFlightConvenience { get; set; }
    }

    public class tblFlightConvenienceSegment : DbWingSegment
    {
        [ForeignKey("tblFlightConvenience")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightConvenience tblFlightConvenience { get; set; }
    }



    public class tblFlightDiscount : DbWingCharge
    {
        [InverseProperty("tblFlightDiscount")]
        public ICollection<tblFlightDiscountServiceProvider> tblFlightDiscountServiceProvider { get; set; }
        [InverseProperty("tblFlightDiscount")]
        public ICollection<tblFlightDiscountCustomerType> tblFlightDiscountCustomerType { get; set; }
        [InverseProperty("tblFlightDiscount")]
        public ICollection<tblFlightDiscountCustomerDetails> tblFlightDiscountCustomerDetails { get; set; }
        [InverseProperty("tblFlightDiscount")]
        public ICollection<tblFlightDiscountPassengerType> tblFlightDiscountPassengerType { get; set; }
        [InverseProperty("tblFlightDiscount")]
        public ICollection<tblFlightDiscountFlightClass> tblFlightDiscountFlightClass { get; set; }
        [InverseProperty("tblFlightDiscount")]
        public ICollection<tblFlightDiscountAirline> tblFlightDiscountAirline { get; set; }        
        [InverseProperty("tblFlightDiscount")]
        public ICollection<tblFlightDiscountSegment> tblFlightDiscountSegment { get; set; }

    }


    public class tblFlightDiscountServiceProvider : DbWingServiceProvider
    {
        [ForeignKey("tblFlightDiscount")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightDiscount tblFlightDiscount { get; set; }
    }
    public class tblFlightDiscountCustomerType : DbWingCustomerType
    {
        [ForeignKey("tblFlightDiscount")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightDiscount tblFlightDiscount { get; set; }
    }
    public class tblFlightDiscountCustomerDetails : DbWingCustomerDetails
    {
        [ForeignKey("tblFlightDiscount")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightDiscount tblFlightDiscount { get; set; }

    }
    public class tblFlightDiscountPassengerType : DbWingPassengerType
    {
        [ForeignKey("tblFlightDiscount")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightDiscount tblFlightDiscount { get; set; }
    }

    public class tblFlightDiscountFlightClass : DbWingFlightClass
    {
        [ForeignKey("tblFlightDiscount")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightDiscount tblFlightDiscount { get; set; }
    }
    public class tblFlightDiscountAirline : DbWingAirline
    {
        [ForeignKey("tblFlightDiscount")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightDiscount tblFlightDiscount { get; set; }
    }

    public class tblFlightDiscountSegment : DbWingSegment
    {
        [ForeignKey("tblFlightDiscount")] // Foreign Key here
        public override int? ChargeId { get; set; }
        public tblFlightDiscount tblFlightDiscount { get; set; }
    }


    #endregion



    #region **************************Flight Search Caching **************************

    public class tblFlightSearchRequest_Caching
    {
        [Key]
        [MaxLength(64)]
        public string CachingId { get; set; }
        public enmJourneyType JourneyType { get; set; } = enmJourneyType.OneWay;
        [MaxLength(24)]
        public string Origin { get; set; }
        [MaxLength(24)]
        public string Destination { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int InfantCount { get; set; }
        public enmCabinClass FlightCabinClass { get; set; }
        public double MinmumPrice { get; set; }
        public DateTime TravelDt { get; set; }
        public DateTime CreatedDt { get; set; }
        public DateTime ExpiredDt { get; set; }
        public bool IsDeleted { get; set; }

        [MaxLength(64)]
        public string ProviderTraceId { get; set; }
        public enmServiceProvider ServiceProvider { get; set; }
        public ICollection<tblFlightSearchResponses_Caching> tblFlightSearchResponses_Caching { get; set; }
        
    }
    
    public class tblFlightSearchResponses_Caching
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IndexId { get; set; }
        [ForeignKey("tblFlightSearchRequest_Caching")] // Foreign Key here
        [MaxLength(64)]
        public string ResponseId { get; set; }
        public tblFlightSearchRequest_Caching tblFlightSearchRequest_Caching { get; set; }
        public ICollection<tblFlightSearchSegment_Caching> tblFlightSearchSegment_Caching { get; set; }
        public ICollection<tblFlightFare_Caching> tblFlightFare_Caching { get; set; }
    }
    public class tblFlightSearchSegment_Caching
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SegmentId { get; set; }
        [ForeignKey("tblFlightSearchResponses_Caching")] // Foreign Key here
        public int? SearchIndexId { get; set; }
        public tblFlightSearchResponses_Caching tblFlightSearchResponses_Caching { get; set; }        
        public bool isLcc { get; set; }
        [MaxLength(32)]
        public string Code { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string FlightNumber { get; set; }
        [MaxLength(128)]
        public string OperatingCarrier { get; set; }

        [MaxLength(32)]
        public string OriginAirportCode { get; set; }
        [MaxLength(256)]
        public string OriginAirportName { get; set; }
        [MaxLength(64)]
        public string OriginTerminal { get; set; }
        [MaxLength(64)]
        public string OriginCityCode { get; set; }
        [MaxLength(256)]
        public string OriginCityName { get; set; }
        [MaxLength(64)]
        public string OriginCountryCode { get; set; }
        [MaxLength(256)]
        public string OriginCountryName { get; set; }

        [MaxLength(32)]
        public string DestinationAirportCode { get; set; }
        [MaxLength(256)]
        public string DestinationAirportName { get; set; }
        [MaxLength(64)]
        public string DestinationTerminal { get; set; }
        [MaxLength(64)]
        public string DestinationCityCode { get; set; }
        [MaxLength(256)]
        public string DestinationCityName { get; set; }
        [MaxLength(64)]
        public string DestinationCountryCode { get; set; }
        [MaxLength(256)]
        public string DestinationCountryName { get; set; }

        public int TripIndicator { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        [MaxLength(30)]
        public string AirlineType { get; set; }
        public int Mile { get; set; }
        public int Duration { get; set; }
        public int Layover { get; set; }
        public int SegmentNumber { get; set; }
    }
    public class tblFlightFare_Caching
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FareId { get; set; }
        [ForeignKey("tblFlightSearchResponses_Caching")] // Foreign Key here
        public int? SearchIndexId { get; set; }
        public tblFlightSearchResponses_Caching tblFlightSearchResponses_Caching { get; set; }
        [MaxLength(256)]
        public string ProviderFareDetailId { get; set; }
        [MaxLength(64)]
        public string Identifier { get; set; }//Corepreate, Publish, SME
        public int SeatRemaning { get; set; }
        public enmCabinClass CabinClass { get; set; }
        public string ClassOfBooking { get; set; }
        [ForeignKey("tblFlightFareDetail_Caching_Adult")] // Foreign Key here
        public int? AdultPrice { get; set; }
        public tblFlightFareDetail_Caching tblFlightFareDetail_Caching_Adult { get; set; }
        [ForeignKey("tblFlightFareDetail_Caching_Child")] // Foreign Key here
        public int? ChildPrice { get; set; }
        public tblFlightFareDetail_Caching tblFlightFareDetail_Caching_Child { get; set; }
        [ForeignKey("tblFlightFareDetail_Caching_Infant")] // Foreign Key here
        public int? InfantPrice { get; set; }
        public tblFlightFareDetail_Caching tblFlightFareDetail_Caching_Infant { get; set; }

        public double BaseFare { get; set; }
        public double CustomerMarkup { get; set; }
        public double WingMarkup { get; set; }
        public double Convenience { get; set; }
        public double TotalFare { get; set; }
        public double Discount { get; set; }
        public double PromoCode { get; set; }
        public double PromoDiscount { get; set; }
        public double NetFare { get; set; }

    }
    public class tblFlightFareDetail_Caching
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        public double YQTax { get; set; }
        public double BaseFare { get; set; }
        public double Tax { get; set; }
        public double WingMarkup { get; set; }
        public double TotalFare { get; set; }
        public double Discount { get; set; }
        public double NetFare { get; set; }
        public string CheckingBaggage { get; set; }
        public string CabinBaggage { get; set; }
        public bool IsFreeMeal { get; set; }
        public byte IsRefundable { get; set; }
    }

    #endregion


    public class tblFlightBookingMaster : d_ModifiedBy
    {
        [Key]
        [MaxLength(128)]
        public string VisitorId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public enmJourneyType JourneyType { get; set; }
        public enmCabinClass CabinClass { get; set; }
        public DateTime DepartureDt { get; set; }
        public DateTime? ReturnDt { get; set; }
        public int AdultCount { get; set; }
        public int ChildCount { get; set; }
        public int InfantCount { get; set; }
        public ulong Nid { get; set; }
        public int OrgId { get; set; }
        public DateTime BookingDate { get; set; }
        [MaxLength(64)]
        public string PhoneNo { get; set; }
        [MaxLength(128)]
        public string EmailNo { get; set; }
        public enmPaymentMode PaymentMode { get; set; }
        public enmPaymentGateway GatewayId { get; set; }
        public enmPaymentSubType PaymentType { get; set; }
        [MaxLength(128)]
        public string PaymentTransactionNumber { get; set; }
        [MaxLength(32)]
        public string CardNo { get; set; }
        [MaxLength(32)]
        public string AccountNumber { get; set; }
        [MaxLength(128)]
        public string BankName { get; set; }
        public bool IncludeLoaylty { get; set; }
        public int ConsumedLoyaltyPoint { get; set; }
        public double ConsumedLoyaltyAmount { get; set; }
        public double BookingAmount { get; set; }
        public double GatewayCharge { get; set; }
        public double NetAmount { get; set; }
        public double PaidAmount { get; set; }
        public double WalletAmount { get; set; }
        public double LoyaltyAmount { get; set; }
        public enmBookingStatus BookingStatus { get; set; }
        public enmPaymentStatus PaymentStatus { get; set; }
        public bool HaveRefund { get; set; } = false;
    }
    public class tblFlightBookingSearchDetails
    {
        [Key]
        [MaxLength(128)]
        public string BookingId { get; set; }
        [ForeignKey("tblFlightBookingMaster")] // Foreign Key here
        public string VisitorId { get; set; }
        public tblFlightBookingMaster tblFlightBookingMaster { get; set; }
        public int SegmentId { get; set; }
        //Purchased by Customer
        [MaxLength(64)]
        public string PurchaseIdentifier { get; set; }//Corepreate, Publish, SME        
        public enmCabinClass PurchaseCabinClass { get; set; }
        [MaxLength(256)]
        public string PurchaseClassOfBooking { get; set; }
        //Booked by Software
        [MaxLength(64)]
        public string BookedIdentifier { get; set; }//Corepreate, Publish, SME        
        public enmCabinClass BookedCabinClass { get; set; }
        [MaxLength(256)]
        public string BookedClassOfBooking { get; set; }
        [MaxLength(128)]
        public string ProviderBookingId { get; set; }
        public enmServiceProvider ServiceProvider { get; set; }
        public bool IncludeBaggageServices { get; set; }
        public bool IncludeMealServices { get; set; }
        public bool IncludeSeatServices { get; set; }        
        public double PurchaseAmount { get; set; }//Price at Which wing Purchase the ticket
        public double PassengerIncentiveAmount { get; set; }
        public double IncentiveAmount { get; set; }//Incentive that We have to Distribute the Distributors        
        public double PassengerMarkupAmount { get; set; }
        public double MarkupAmount { get; set; }//Wing Markup Amount
        public double PassengerDiscountAmount { get; set; }
        public double DiscountAmount { get; set; }//Discount Applied by Wing
        public double PassengerConvenienceAmount { get; set; }
        public double ConvenienceAmount { get; set; }
        public double SaleAmount { get; set; }
        public double CustomerMarkupAmount { get; set; }
        public double NetSaleAmount { get; set; }
        public enmBookingStatus BookingStatus { get; set; }
        public enmPaymentStatus PaymentStatus { get; set; }
        public bool HaveRefund { get; set; }
        [MaxLength(25)]
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class tblFlightFareMarkupDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        [ForeignKey("tblFlightMarkupMaster")] // Foreign Key here        
        public int? MarkupId { get; set; }
        public tblFlightMarkupMaster tblFlightMarkupMaster { get; set; }
        [ForeignKey("tblFlightBookingSearchDetails")] // Foreign Key here        
        [MaxLength(128)]
        public string BookingId { get; set; }
        public tblFlightBookingSearchDetails tblFlightBookingSearchDetails { get; set; }
        public double Amount { get; set; }
    }
    public class tblFlightFareMLMMarkup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        [ForeignKey("tblFlightMarkupMaster")] // Foreign Key here        
        public int? MLMMarkupId { get; set; }
        public tblFlightMarkupMaster tblFlightMarkupMaster { get; set; }
        [ForeignKey("tblFlightBookingSearchDetails")] // Foreign Key here        
        [MaxLength(128)]
        public string BookingId { get; set; }
        public tblFlightBookingSearchDetails tblFlightBookingSearchDetails { get; set; }
        public double Amount { get; set; }
    }
    public class tblFlightFareDiscount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        [ForeignKey("tblFlightDiscount")] // Foreign Key here        
        public int? DiscountId { get; set; }
        public tblFlightDiscount tblFlightDiscount { get; set; }
        [ForeignKey("tblFlightBookingSearchDetails")] // Foreign Key here        
        [MaxLength(128)]
        public string BookingId { get; set; }
        public tblFlightBookingSearchDetails tblFlightBookingSearchDetails { get; set; }
        public double Amount { get; set; }
    }
    public class tblFlightFareConvenience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        [ForeignKey("tblFlightConvenience")] // Foreign Key here        
        public int? ConvenienceId { get; set; }
        public tblFlightConvenience tblFlightConvenience { get; set; }
        [ForeignKey("tblFlightBookingSearchDetails")] // Foreign Key here        
        [MaxLength(128)]
        public string BookingId { get; set; }
        public tblFlightBookingSearchDetails tblFlightBookingSearchDetails { get; set; }
        public double Amount { get; set; }
    }
    public class tblFlightSearchSegment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("tblFlightBookingMaster")] // Foreign Key here
        [MaxLength(128)]
        public string BookingId { get; set; }
        public tblFlightBookingSearchDetails tblFlightBookingSearchDetails { get; set; }
        public string ProviderSegmentId { get; set; }
        public bool isLcc { get; set; }
        [MaxLength(32)]
        public string Code { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string FlightNumber { get; set; }
        [MaxLength(128)]
        public string OperatingCarrier { get; set; }
        [MaxLength(32)]
        public string OriginAirportCode { get; set; }
        [MaxLength(256)]
        public string OriginAirportName { get; set; }
        [MaxLength(64)]
        public string OriginTerminal { get; set; }
        [MaxLength(64)]
        public string OriginCityCode { get; set; }
        [MaxLength(256)]
        public string OriginCityName { get; set; }
        [MaxLength(64)]
        public string OriginCountryCode { get; set; }
        [MaxLength(256)]
        public string OriginCountryName { get; set; }

        [MaxLength(32)]
        public string DestinationAirportCode { get; set; }
        [MaxLength(256)]
        public string DestinationAirportName { get; set; }
        [MaxLength(64)]
        public string DestinationTerminal { get; set; }
        [MaxLength(64)]
        public string DestinationCityCode { get; set; }
        [MaxLength(256)]
        public string DestinationCityName { get; set; }
        [MaxLength(64)]
        public string DestinationCountryCode { get; set; }
        [MaxLength(256)]
        public string DestinationCountryName { get; set; }
        public int TripIndicator { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        [MaxLength(30)]
        public string AirlineType { get; set; }
        public int Mile { get; set; }
        public int Duration { get; set; }
        public int Layover { get; set; }        
    }
    public class tblFlightServices
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("tblFlightSearchSegment")] // Foreign Key here
        [MaxLength(128)]
        public string FlightSearchSegmentId { get; set; }
        public tblFlightSearchSegment tblFlightSearchSegment { get; set; }
        public enmFlightSearvices ServiceType { get; set; }
        [ForeignKey("tblFlighBookingPassengerDetails")] // Foreign Key here
        public int? PassengerDetailId { get; set; }
        public tblFlighBookingPassengerDetails tblFlighBookingPassengerDetails { get; set; }
        [MaxLength(128)]
        public string ServiceCode { get; set; }
        [MaxLength(256)]
        public string ServiceDescription { get; set; }
        public double ServiceChargeAmount { get; set; }
        public double MarkupAmount { get; set; }
    }
    public class tblFlightFareDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlightFareDetailId {get; set;}
        public double YQTax { get; set; }
        public double BaseFare { get; set; }
        public double Tax { get; set; }
        public double WingMarkup { get; set; }
        public double MLMMarkup { get; set; }
        public double Convenience { get; set; }
        public double TotalFare { get; set; }
        public double Discount { get; set; }
        public double ServiceChargeAmount { get; set; }
        public double NetFare { get; set; }
        [MaxLength(256)]
        public string CheckingBaggage { get; set; }
        [MaxLength(256)]
        public string CabinBaggage { get; set; }
        public bool IsFreeMeal { get; set; }
        public byte IsRefundable { get; set; }
        [ForeignKey("tblFlighBookingPassengerDetails")] // Foreign Key here
        public int? PassengerDetailId { get; set; }
        public tblFlighBookingPassengerDetails tblFlighBookingPassengerDetails { get; set; }
    }
    public class tblFlightFareDetailMarkupDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        [ForeignKey("tblFlightMarkupMaster")] // Foreign Key here        
        public int? MarkupId { get; set; }
        public tblFlightMarkupMaster tblFlightMarkupMaster { get; set; }
        [ForeignKey("tblFlightFareDetail")] // Foreign Key here        
        public int? FareDetailId { get; set; }
        public tblFlightFareDetail tblFlightFareDetail { get; set; }
        public double Amount { get; set; }
    }
    public class tblFlightFareDetailMLMMarkup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        [ForeignKey("tblFlightMarkupMaster")] // Foreign Key here        
        public int? MLMMarkupId { get; set; }
        public tblFlightMarkupMaster tblFlightMarkupMaster { get; set; }
        [ForeignKey("tblFlightFareDetail")] // Foreign Key here        
        public int? FareDetailId { get; set; }
        public tblFlightFareDetail tblFlightFareDetail { get; set; }
        public double Amount { get; set; }
    }
    public class tblFlightFareDetailDiscount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        [ForeignKey("tblFlightDiscount")] // Foreign Key here        
        public int? DiscountId { get; set; }
        public tblFlightDiscount tblFlightDiscount { get; set; }
        [ForeignKey("tblFlightFareDetail")] // Foreign Key here        
        public int? FareDetailId { get; set; }
        public tblFlightFareDetail tblFlightFareDetail { get; set; }
        public double Amount { get; set; }
    }
    public class tblFlightFareDetailConvenience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        [ForeignKey("tblFlightConvenience")] // Foreign Key here        
        public int? ConvenienceId { get; set; }
        public tblFlightConvenience tblFlightConvenience { get; set; }
        [ForeignKey("tblFlightFareDetail")] // Foreign Key here        
        public int? FareDetailId { get; set; }
        public tblFlightFareDetail tblFlightFareDetail { get; set; }
        public double Amount { get; set; }
    }
    public class tblFlightRefundStatusDetails : d_CreatedBy
    {
        [Key]
        [MaxLength(128)]
        public int RefundId { get; set; }
        [ForeignKey("tblFlilghtBookingSearchDetails")] // Foreign Key here
        [MaxLength(128)]
        public string BookingId { get; set; }
        public tblFlightBookingSearchDetails tblFlightBookingSearchDetails { get; set; }
        public enmServiceProvider ServiceProvider { get; set; }
        [MaxLength(256)]
        public string ProviderBookingId { get; set; }
        public double RefundAmount { get; set; }
        public enmRefundStatus RefundStatus { get; set; }
        [MaxLength(25)]
        public string Remarks { get; set; }
    }
    public class tblFlighBookingPassengerDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        [ForeignKey("tblFlightBookingMaster")] // Foreign Key here
        public string VisitorId { get; set; }
        public tblFlightBookingMaster tblFlightBookingMaster { get; set; }
        public enmPassengerType PassengerType { get; set; }
        [MaxLength(16)]
        public string Title { get; set; }
        [MaxLength(64)]
        public string FirstName { get; set; }
        [MaxLength(64)]
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        [MaxLength(64)]
        public string PassportNumber { get; set; }
        public DateTime? PassportIssueDate { get; set; }
        public DateTime? PassportExpiryDate { get; set; }
        
    }
    public class tblFlighRefundPassengerDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        public string RefundId { get; set; }
        public enmPassengerType PassengerType { get; set; }
        [MaxLength(16)]
        public string Title { get; set; }
        [MaxLength(64)]
        public string FirstName { get; set; }
        [MaxLength(64)]
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        [MaxLength(64)]
        public string PassportNumber { get; set; }
        public DateTime? PassportIssueDate { get; set; }
        public DateTime? PassportExpiryDate { get; set; }
    }


    #region ****************** Flight Setting ******************************
    public class tblFlightSerivceProvider : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public enmServiceProvider ServiceProvider { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class tblFlightInstantBooking : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public  enmCustomerType CustomerType { get; set; }
        public bool InstantDomestic { get; set; }
        public bool InstantNonDomestic { get; set; }        
        public DateTime EffectiveFromDate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class tblFlightSerivceProviderPriority : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public enmServiceProvider ServiceProvider { get; set; }
        public int priority { get; set; }        
        public DateTime EffectiveFromDate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class tblFlightBookingAlterMaster : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AlterId { get; set; }
        public enmCabinClass CabinClass { get; set; }
        [MaxLength(64)]
        public string Identifier { get; set; }
        public string ClassOfBooking { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<tblFlightBookingAlterDetails> tblFlightBookingAlterDetails { get; set; }
    }
    public class tblFlightBookingAlterDetails 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("tblFlightBookingAlterMaster")] // Foreign Key here
        public int? AlterId { get; set; }
        public tblFlightBookingAlterMaster tblFlightBookingAlterMaster { get; set; }
        public enmCabinClass CabinClass { get; set; }
        [MaxLength(64)]
        public string Identifier { get; set; }
        public string ClassOfBooking { get; set; }
        
    }

    public class tblFlightFareFilter : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FilterId { get; set; }
        public enmCustomerType CustomerType { get; set; }
        public bool IsEanableAllFare { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<tblFlightFareFilterDetails> tblFlightFareFilterDetails { get; set; }
    }

    public class tblFlightFareFilterDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("tblFlightBookingAlterMaster")] // Foreign Key here
        public int? FilterId { get; set; }
        public tblFlightBookingAlterMaster tblFlightBookingAlterMaster { get; set; }        
        [MaxLength(64)]
        public string Identifier { get; set; }
        public string ClassOfBooking { get; set; }
    }


    #endregion



}

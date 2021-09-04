using System;
using System.Collections.Generic;
using System.Text;

//namespace projContext
//{

//    public class Rootobject
//    {
//        public Response Response { get; set; }
//    }

//    public class Response
//    {
//        public int ResponseStatus { get; set; }
//        public Error Error { get; set; }
//        public string TraceId { get; set; }
//        public string Origin { get; set; }
//        public string Destination { get; set; }
//        public Result[][] Results { get; set; }
//    }

//    public class Error
//    {
//        public int ErrorCode { get; set; }
//        public string ErrorMessage { get; set; }
//    }

//    public class Result
//    {
//        public bool IsHoldAllowedWithSSR { get; set; }
//        public string ResultIndex { get; set; }
//        public int Source { get; set; }
//        public bool IsLCC { get; set; }
//        public bool IsRefundable { get; set; }
//        public bool IsPanRequiredAtBook { get; set; }
//        public bool IsPanRequiredAtTicket { get; set; }
//        public bool IsPassportRequiredAtBook { get; set; }
//        public bool IsPassportRequiredAtTicket { get; set; }
//        public bool GSTAllowed { get; set; }
//        public bool IsCouponAppilcable { get; set; }
//        public bool IsGSTMandatory { get; set; }
//        public string AirlineRemark { get; set; }
//        public string ResultFareType { get; set; }
//        public Fare Fare { get; set; }
//        public Farebreakdown[] FareBreakdown { get; set; }
//        public Segment[][] Segments { get; set; }
//        public object LastTicketDate { get; set; }
//        public string TicketAdvisory { get; set; }
//        public Farerule[] FareRules { get; set; }
//        public string AirlineCode { get; set; }
//        public string ValidatingAirline { get; set; }
//        public bool IsUpsellAllowed { get; set; }
//    }

//    public class Fare
//    {
//        public string Currency { get; set; }
//        public int BaseFare { get; set; }
//        public int Tax { get; set; }
//        public Taxbreakup[] TaxBreakup { get; set; }
//        public int YQTax { get; set; }
//        public int AdditionalTxnFeeOfrd { get; set; }
//        public int AdditionalTxnFeePub { get; set; }
//        public int PGCharge { get; set; }
//        public float OtherCharges { get; set; }
//        public Chargebu[] ChargeBU { get; set; }
//        public int Discount { get; set; }
//        public float PublishedFare { get; set; }
//        public float CommissionEarned { get; set; }
//        public float PLBEarned { get; set; }
//        public float IncentiveEarned { get; set; }
//        public float OfferedFare { get; set; }
//        public float TdsOnCommission { get; set; }
//        public float TdsOnPLB { get; set; }
//        public float TdsOnIncentive { get; set; }
//        public int ServiceFee { get; set; }
//        public int TotalBaggageCharges { get; set; }
//        public int TotalMealCharges { get; set; }
//        public int TotalSeatCharges { get; set; }
//        public int TotalSpecialServiceCharges { get; set; }
//    }

//    public class Taxbreakup
//    {
//        public string key { get; set; }
//        public int value { get; set; }
//    }

//    public class Chargebu
//    {
//        public string key { get; set; }
//        public float value { get; set; }
//    }

//    public class Farebreakdown
//    {
//        public string Currency { get; set; }
//        public int PassengerType { get; set; }
//        public int PassengerCount { get; set; }
//        public int BaseFare { get; set; }
//        public int Tax { get; set; }
//        public Taxbreakup1[] TaxBreakUp { get; set; }
//        public int YQTax { get; set; }
//        public int AdditionalTxnFeeOfrd { get; set; }
//        public int AdditionalTxnFeePub { get; set; }
//        public int PGCharge { get; set; }
//        public int SupplierReissueCharges { get; set; }
//    }

//    public class Taxbreakup1
//    {
//        public string key { get; set; }
//        public int value { get; set; }
//    }

//    public class Segment
//    {
//        public string Baggage { get; set; }
//        public string CabinBaggage { get; set; }
//        public int CabinClass { get; set; }
//        public int TripIndicator { get; set; }
//        public int SegmentIndicator { get; set; }
//        public Airline Airline { get; set; }
//        public int NoOfSeatAvailable { get; set; }
//        public Origin Origin { get; set; }
//        public Destination Destination { get; set; }
//        public int Duration { get; set; }
//        public int GroundTime { get; set; }
//        public int Mile { get; set; }
//        public bool StopOver { get; set; }
//        public string FlightInfoIndex { get; set; }
//        public string StopPoint { get; set; }
//        public object StopPointArrivalTime { get; set; }
//        public object StopPointDepartureTime { get; set; }
//        public string Craft { get; set; }
//        public object Remark { get; set; }
//        public bool IsETicketEligible { get; set; }
//        public string FlightStatus { get; set; }
//        public string Status { get; set; }
//        public int AccumulatedDuration { get; set; }
//    }

//    public class Airline
//    {
//        public string AirlineCode { get; set; }
//        public string AirlineName { get; set; }
//        public string FlightNumber { get; set; }
//        public string FareClass { get; set; }
//        public string OperatingCarrier { get; set; }
//    }

//    public class Origin
//    {
//        public Airport Airport { get; set; }
//        public DateTime DepTime { get; set; }
//    }

//    public class Airport
//    {
//        public string AirportCode { get; set; }
//        public string AirportName { get; set; }
//        public string Terminal { get; set; }
//        public string CityCode { get; set; }
//        public string CityName { get; set; }
//        public string CountryCode { get; set; }
//        public string CountryName { get; set; }
//    }

//    public class Destination
//    {
//        public Airport1 Airport { get; set; }
//        public DateTime ArrTime { get; set; }
//    }

//    public class Airport1
//    {
//        public string AirportCode { get; set; }
//        public string AirportName { get; set; }
//        public string Terminal { get; set; }
//        public string CityCode { get; set; }
//        public string CityName { get; set; }
//        public string CountryCode { get; set; }
//        public string CountryName { get; set; }
//    }

//    public class Farerule
//    {
//        public string Origin { get; set; }
//        public string Destination { get; set; }
//        public string Airline { get; set; }
//        public string FareBasisCode { get; set; }
//        public string FareRuleDetail { get; set; }
//        public string FareRestriction { get; set; }
//        public string FareFamilyCode { get; set; }
//        public string FareRuleIndex { get; set; }
//    }

//}

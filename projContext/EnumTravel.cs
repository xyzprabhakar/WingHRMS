using System;
using System.Collections.Generic;
using System.Text;

namespace projContext
{
    public enum enmJourneyType
    {
        OneWay = 1, Return = 2, MultiStop = 3, AdvanceSearch = 4, SpecialReturn = 5
    }

    public enum enmCabinClass
    {
        //ALL=1,
        ECONOMY = 2,
        PREMIUM_ECONOMY = 3,
        BUSINESS = 4,
        //PremiumBusiness=5,
        FIRST = 6
    }
    public enum enmMarkupApplicability
    {
        OnTicket = 1,
        OnPassenger = 2,
        OnBaggageServices = 4,
        OnMealServices = 8,
        OnSeatServices = 16,
        OnExtraService = 32
    }


    public enum enmFlightSearvices
    {
        OnTicket = 1,
        OnPassenger = 2,
        OnBaggageServices = 4,
        OnMealServices = 8,
        OnSeatServices = 16,
        OnExtraService = 32
    }

    public enum enmPreferredDepartureTime
    {
        AnyTime = 1,
        Morning = 2,
        AfterNoon = 3,
        Evening = 4,
        Night = 5
    }

    public enum enmBookingStatus
    {
        Pending = 0,
        Booked = 1,
        Cancel = 2,
        PendingAtPayment = 4,

    }

    public enum enmRefundStatus
    {
        Pending = 0,
        Settled = 1,
        Initiated = 2,
        Cancel = 4,
        RefundReturned = 8,
    }


    public enum enmBankTransactionType
    {
        None = 0,
        UPI = 1,
        NEFT = 2,
        RTGS = 3,
        CHEQUE = 4
    }
    

    public enum enmServiceProvider
    {
        None = 0,
        TripJack = 1,
        TBO = 2,        
        Kafila = 3
    }

    public enum enmPassengerType
    {
        Adult = 1,
        Child = 2,
        Infant = 3,
    }

    public enum enmFlighWingCharge
    { 
        CustomerMarkup=1,
        WingMarkup=2,
        Discount=3,
        Convenience=4,
        PaymentGateway=5,
        MLMCharge=6
    }
}

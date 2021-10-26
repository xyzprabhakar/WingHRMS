using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projAPI.Model.Travel;
using projContext.DB.CRM.Travel;

namespace projAPI.Services.Travel
{

    public interface IWingFlight
    {
        Task<mdlSearchResponse> SearchAsync(mdlSearchRequest request, int CustomerId);
        Task<mdlFareQuotResponse> FareQuoteAsync(mdlFareQuotRequest request);
        Task<mdlFareRuleResponse> FareRuleAsync(mdlFareRuleRequest request);
        Task<mdlBookingResponse> BookingAsync(mdlBookingRequest request);
        Task<mdlFlightCancellationChargeResponse> CancelationChargeAsync(mdlCancellationRequest request);
        Task<mdlFlightCancellationResponse> CancellationAsync(mdlCancellationRequest request);
        Task<mdlCancelationDetails> CancelationDetailsAsync(string request);
    }


    public interface IsrvAir
    {
        IEnumerable<tblAirport> GetAirport(bool OnlyActive = true, bool IsDomestic = false);
    }

    public class srvAir : IsrvAir
    {
        private readonly TravelContext _travelContext;
        public srvAir(TravelContext travelContext)
        {
            _travelContext = travelContext;
        }

        public IEnumerable<tblAirport> GetAirport(bool OnlyActive = true, bool IsDomestic = false)
        {
            var TempData = _travelContext.tblAirport.Where(p => !p.IsDeleted).AsQueryable();
            if (OnlyActive)
            {
                TempData = TempData.Where(p => p.IsActive);
            }
            if (IsDomestic)
            {
                TempData = TempData.Where(p => p.IsDomestic);
            }
            return TempData.AsEnumerable();
        }

    }


    
}

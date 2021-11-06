using projAPI.Model;
using projAPI.Model.Travel;
using projContext.DB.CRM.Travel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projContext;
using projAPI.Services;

namespace projAPI.Classes
{
    public class Travel
    {
        private readonly TravelContext _travelContext;
        public Travel(TravelContext travelContext)
        {
            _travelContext = travelContext;
        }
        
    }
}

using projContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model
{
    public class mdlReturnData
    {
        public enmMessageType MessageType { get; set; }
        public string  Message { get; set; }
        public dynamic ReturnId { get; set; }        
    }
    
}

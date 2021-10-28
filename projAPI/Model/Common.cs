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
    public class mdlDropDown_Int
    {
        public int Id{ get; set; }
        public int Code { get; set; }
        public string Display { get; set; }
    }
    public class mdlDropDown_String
    {
        public string Id { get; set; }        
        public string Display { get; set; }
    }
}

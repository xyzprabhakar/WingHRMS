﻿using projContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model
{
    public class mdlReturnData
    {
        public enmMessageType MessageType { get; set; } = enmMessageType.None;
        public string  Message { get; set; }
        public dynamic ReturnId { get; set; }        
    }
    public class mdlCommonReturn
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name{ get; set; }
    }
    public class mdlCommonReturnUlong
    {
        public ulong Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

}

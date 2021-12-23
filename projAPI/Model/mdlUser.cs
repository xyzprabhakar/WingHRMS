using projContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model
{

    public class mdlUserRolesWraper
    {
        public mdlUserMaster userMaster { get; set; }
        public List<mdlUserRoles> userRoles { get; set; }
    }
    public class mdlUserRoles
    {
        public mdlRoleMaster roleMaster { get; set; }
        public bool isActive { get; set; }
        public DateTime modifyDt { get; set; }
    }
    public class mdlRoleMaster
    { 
        public int roleId { get; set; }
        public string roleName { get; set; }
        public bool isActive { get; set; }
        public List<mdlRoleDocument> roleDocument { get; set; }    
    }
    public class mdlUserMaster
    {
        public ulong userId { get; set; }
        public string userName{ get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
    }
    public class mdlRoleDocument
    { 
        public enmDocumentMaster documentId { get; set; }
        public enmDocumentType PermissionType { get; set; }
    }
    
}

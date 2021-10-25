using projContext.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Classes
{
    public class GenrateGUID
    {
        private int _UserID = 1;
        public string GetGuid
        {
            get
            {
                return _Guid();
            }
        }
        public GenrateGUID()
        {

        }
        public GenrateGUID(int UserID)
        {
            _UserID = UserID;
        }


        string _Guid()
        {
            return "";
            //projContext.Context db = new projContext.Context();
            //tbl_guid_detail tbl = new tbl_guid_detail();
            //string gg = Guid.NewGuid().ToString().Replace("-", "");
            //tbl.id = gg;
            //tbl.genrated_by = _UserID;
            //tbl.genration_dt = DateTime.Now;
            //db.tbl_guid_detail.Add(tbl);
            //db.SaveChanges();
            //return tbl.id.ToString();
        }
    }
}

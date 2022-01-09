using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using projContext;
using projContext.DB;
using projAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using MySql.Data.MySqlClient;
using System.Data;
using static projContext.CommonClass;

namespace projAPI.Classes
{
#if(false)
    public class clsCompanyDetails
    {
        //public int _company_id;
        private readonly IHttpContextAccessor _AC;
        private readonly Context _context;
        private readonly IConfiguration _config;
        private readonly clsCurrentUser _clsCurrentUser;



        public clsCompanyDetails(Context context, IConfiguration config, IHttpContextAccessor AC, clsCurrentUser _clsCurrentUser)
        {
            _context = context;
            _config = config;
            _AC = AC;
            this._clsCurrentUser = _clsCurrentUser;
        }


        public List<clsLocationMaster> GetLocations()
        {
            List<clsLocationMaster> location_lst = new List<clsLocationMaster>();

            try
            {

                location_lst = _context.tbl_location_master.Join(_context.tbl_company_master, a => a.company_id, b => b.company_id, (a, b) => new clsLocationMaster
                {
                    location_id = a.location_id,
                    location_code = a.location_code,
                    location_name = a.location_name,
                    company_name = b.company_name,
                    state = a.tbl_state.name,
                    city = a.tbl_city.name,
                    created_on = a.created_date,
                    is_active = Convert.ToBoolean(a.is_active),
                    company_id = a.company_id,
                    location_type_id = Convert.ToInt32(a.type_of_location),
                    address_line_one = a.address_line_one,
                    address_line_two = a.address_line_two,
                    pin_code = Convert.ToString(a.pin_code),
                    //countryid = e.tbl_country.country_id,
                    //stateid = a.state_id,
                    //cityid = a.city_id,
                    primary_email_id = a.primary_email_id,
                    secondary_email_id = a.secondary_email_id,
                    primary_contact_number = a.primary_contact_number,
                    secondary_contact_number = a.secondary_contact_number,
                    website = a.website,
                    type_of_location = Enum.GetName(typeof(LocationType), Convert.ToInt16(a.type_of_location)),


                }).Distinct().ToList();

            }
            catch (Exception ex)
            {

            }

           

            return location_lst;
        }


        public List<clsLocationMaster> GetSubLocations()
        {
            List<clsLocationMaster> sublocation_lst = new List<clsLocationMaster>();

            var locations = GetLocations();

            sublocation_lst = (from a in _context.tbl_sub_location_master
                               join b in locations on a.location_id equals b.location_id into ab
                               from c in ab.DefaultIfEmpty()
                               select new clsLocationMaster
                               {
                                   location_id = a.location_id??0,
                                   sub_location_id = a.sub_location_id,
                                   location_name =locations!=null && locations.Count>0 ?c.location_name:"", //master location name
                                   company_name = locations != null && locations.Count > 0 ? c.company_name : "",
                                   sub_location_name=a.location_name, //sub location name
                                  is_active=Convert.ToBoolean(a.is_active),
                                  created_on=a.created_date,
                                   type_of_location=locations!=null&& locations.Count>0?c.type_of_location:"",
                                   company_id=locations!=null&& locations.Count>0?c.company_id:0,
                               }).ToList();


            return sublocation_lst;
        }
    }

#endif
}

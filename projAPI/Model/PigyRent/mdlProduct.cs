using projContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Model.PigyRent
{
    public class mdlProductAtribute
    {
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public enmAttributeDataType DataType { get; set; }
        public string DefaultValue { get; set; }
        public byte DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public string AttributeValue { get; set; }
    }
    public class mdlProduct
    {   
        public ulong ProductId { get; set; }        
        public string ProductName { get; set; }
        public string Remarks{ get; set; }
        public int CategoryId { get; set; }
        public string  CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string Subcategory { get; set; }        
        public string ThumbnailPath { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime ApplicableFromDate { get; set; } = DateTime.Now;
        public DateTime ApplicableToDate { get; set; }
        public int CurrencyId { get; set; }
        public enmProductStatus ProductStatus { get; set; }
        public bool IsMultipleBooking { get; set; }//Enable Parellel Booking
        public short MultipleBookingCount { get; set; }
        public bool IsMultipleDay { get; set; }
        public  enmPriceType PriceType { get; set; }
        public double SecurityAmount { get; set; }
        public double ProductAmount { get; set; }
        public double TotalAmount { get; set; }
        public List<mdlProductAtribute> ProductAtribute { get; set; }
        public List<string>Keywords { get; set; }        
        public ulong Nid { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string Locality { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}

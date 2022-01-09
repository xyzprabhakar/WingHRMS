using projAPI.Model;
using projAPI.Model.PigyRent;
using projContext.DB.PigyRent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projAPI.Services.PigyRent
{
    public class ProductServices
    {
        private readonly PigyContext _pigyContext;
        public ProductServices(PigyContext pigyContext)
        {
            _pigyContext = pigyContext;
        }
        

        public mdlReturnData SaveProduct(mdlProduct mdlproduct,int UserId)
        {
            mdlReturnData returnData = new mdlReturnData();
            tblProduct Product = new tblProduct()
            {
                ProductName = mdlproduct.ProductName,
                CategoryId= mdlproduct.CategoryId,
                SubCategoryId = mdlproduct.SubCategoryId,
                ShortDescription= mdlproduct.ShortDescription,
                IsBlocked=false,
                ApplicableToDate= mdlproduct.ApplicableToDate,
                CurrencyId= mdlproduct.CurrencyId,
                ProductStatus= mdlproduct.ProductStatus,
                IsMultipleBooking= mdlproduct.IsMultipleBooking,
                MultipleBookingCount= mdlproduct.MultipleBookingCount,
                IsMultipleDay = mdlproduct.IsMultipleDay,
                PriceType= mdlproduct.PriceType,
                RequestedBy = mdlproduct.Nid,
                RequestedDt=DateTime.Now,
                RequestedRemarks= mdlproduct.Remarks,
                ThumbnailPath= mdlproduct.ThumbnailPath,
            };
            List<tblProductAddress> ProductAddress = new List<tblProductAddress>();
            ProductAddress.Add(new tblProductAddress() {
                CountryId= mdlproduct.CountryId,
                StateId= mdlproduct.StateId,
                Locality = mdlproduct.Locality,
                Address= mdlproduct.Address,
                Latitude= mdlproduct.Latitude,
                Longitude= mdlproduct.Longitude


            });
            Product.tblProductAddress = new List<tblProductAddress>();

            _pigyContext.tblProduct.Add(Product);
            _pigyContext.SaveChanges();

            return returnData;
        }

    }
}

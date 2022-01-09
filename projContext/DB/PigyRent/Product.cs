using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace projContext.DB.PigyRent
{
    public class tblCategory :d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
    public class tblSubCategory : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubCategoryId { get; set; }
        [ForeignKey("tblCategory")]
        public int? CategoryId { get; set; }
        public tblCategory tblCategory { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }


    public class tblCategoryAttribute : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttributeId { get; set; }
        [ForeignKey("tblCategory")]
        public int? CategoryId { get; set; }
        public tblCategory tblCategory { get; set; }
        [ForeignKey("tblSubCategory")]
        public int? SubCategoryId { get; set; }
        public tblSubCategory tblSubCategory { get; set; }
        public string AttributeName { get; set; }
        public enmAttributeDataType DataType { get; set; }
        [MaxLength(256)]
        public string DefaultValue { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(256)]
        public string SuggestedValue { get; set; }
        public byte DisplayOrder { get; set; }
        public bool IsFilterCriteria { get; set; }

    }
    public class tblCategoryKeywords : d_ModifiedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        [ForeignKey("tblSubCategory")]
        public int? SubCategoryId { get; set; }
        public tblSubCategory tblSubCategory { get; set; }
        [MaxLength(256)]
        public string Keyword { get; set; }
        public bool IsActive { get; set; }

    }

    public class tblProduct : d_ApprovedBy
    {
        [Key]
        public ulong ProductId { get; set; }
        [MaxLength(32)]
        public string ProductName { get; set; }
        [ForeignKey("tblCategory")]
        public int? CategoryId { get; set; }
        public tblCategory tblCategory { get; set; }
        [ForeignKey("tblSubCategory")]
        public int? SubCategoryId { get; set; }
        public tblSubCategory tblSubCategory { get; set; }
        [MaxLength(256)]
        public string ThumbnailPath { get; set; }
        [MaxLength(256)]
        public string ShortDescription { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime ApplicableFromDate { get; set; } = DateTime.Now;
        public DateTime ApplicableToDate { get; set; }        
        public int CurrencyId { get; set; }
        public enmProductStatus ProductStatus { get; set; }
        public bool IsMultipleBooking { get; set; }//Enable Parellel Booking
        public short MultipleBookingCount { get; set; }
        public bool IsMultipleDay { get; set; }
        public enmPriceType PriceType { get; set; }
        public ulong Nid { get; set; }
        public List<tblProductPrice> tblProductPrice { get; set; }
        public List<tblProductBooking> tblProductBooking { get; set; }
        public List<tblProductRating> tblProductRating { get; set; }
        public List<tblProductDescription> tblProductDescription { get; set; }
        public List<tblProductImage> tblProductImage { get; set; }
        public List<tblProductAttributeValue> tblProductAttributeValue { get; set; }
        public List<tblProductKeyword> tblProductKeyword { get; set; }
        public List<tblProductAddress> tblProductAddress { get; set; }
        
    }

    public class tblProductPrice : d_ApprovedBy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Sno { get; set; }
        [ForeignKey("tblProduct")]
        public ulong? ProductId { get; set; }
        public tblProduct tblProduct { get; set; }
        public DateTime EffectiveFromDt { get; set; } = DateTime.Now;
        public DateTime EffectiveToDt { get; set; } = DateTime.Now;
        public double SecurityAmount { get; set; }
        public double ProductAmount { get; set; }
        public double TotalAmount { get; set; }
        public bool IsFinal { get; set; }
    }

    public class tblProductBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Sno { get; set; }
        [ForeignKey("tblProduct")]
        public ulong? ProductId { get; set; }
        public tblProduct tblProduct { get; set; }
        public int BookingCounter { get; set; }
        public enmProductStatus ProductStatus { get; set; }
        public ulong BookedBy { get; set; }
        public double SecurityAmount { get; set; }
        public double ProductAmount { get; set; }
        public double TotalAmount { get; set; }
        public double PaidAmount { get; set; }
        public DateTime BookingFromDt { get; set; } = DateTime.Now;
        public DateTime BookingToDt { get; set; } = DateTime.Now;
        public DateTime BookingDt { get; set; } = DateTime.Now;
        public bool IsCancel { get; set; }

    }
    public class tblProductRating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Sno { get; set; }
        [ForeignKey("tblProduct")]
        public ulong? ProductId { get; set; }
        public tblProduct tblProduct { get; set; }
        public ulong UserId { get; set; }
        public byte Rating { get; set; }
        [MaxLength(256)]
        public string Review { get; set; }
    }

    public class tblProductBookingChat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Sno { get; set; }
        [ForeignKey("tblProduct")]
        public ulong? ProductId { get; set; }
        public tblProduct tblProduct { get; set; }
        public ulong UserId { get; set; }
        [MaxLength(256)]
        public string ChatMessage { get; set; }
        public bool IsRead { get; set; }
    }


    public class tblProductDescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Sno { get; set; }
        [ForeignKey("tblProduct")]
        public ulong? ProductId { get; set; }
        public tblProduct tblProduct { get; set; }
        [MaxLength(1024)]
        public string LongDescription { get; set; }
    }

    public class tblProductImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        [ForeignKey("tblProduct")]
        public ulong? ProductId { get; set; }
        public tblProduct tblProduct { get; set; }
        [MaxLength(256)]
        public string ImagePath { get; set; }
        public byte DisplayOrder { get; set; }
    }

    public class tblProductAttributeValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        [ForeignKey("tblProduct")]
        public ulong? ProductId { get; set; }
        public tblProduct tblProduct { get; set; }
        public int AttributeId { get; set; }
        [MaxLength(256)]
        public string AttributeValue { get; set; }
    }

    public class tblProductKeyword
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sno { get; set; }
        [ForeignKey("tblProduct")]
        public ulong? ProductId { get; set; }
        public tblProduct tblProduct { get; set; }
        public string keyword { get; set; }
    }

    public class tblProductAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Sno { get; set; }
        [ForeignKey("tblProduct")]
        public ulong? ProductId { get; set; }
        public tblProduct tblProduct { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        [MaxLength(64)]
        public string Locality { get; set; }
        [MaxLength(64)]
        public string Address{ get; set; }
        [MaxLength(64)]
        public string Longitude { get; set; }
        [MaxLength(64)]
        public string Latitude { get; set; }

    }
}

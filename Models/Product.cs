using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCamping.Models
{
    [Table("Products")]
    public class Product : BaseModel
    {
        [Key]
        public int PRO_ID { get; set; }
        public required int CAT_ID { get; set; }
        public required int BRA_ID { get; set; }
        public string? Avatar { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        [DisplayName("Discount Price")]
        public decimal? DiscountPrice { get; set; }
        public double? Rate { get; set; }
        public string? Description { get; set; }
        public string? Intro { get; set; }
        [ForeignKey("CAT_ID")]
        public virtual Category? Category { get; set; }
        [ForeignKey("BRA_ID")]
        public virtual Brand? Brand { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}

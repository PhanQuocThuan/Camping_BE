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
        public string? Avatar { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string? Unit { get; set; }
        public double? Rate { get; set; }
        public string? Description { get; set; }
        public string? Details { get; set; }

        [ForeignKey("CAT_ID")]
        public virtual Category? Category { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}

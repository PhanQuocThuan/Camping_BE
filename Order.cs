using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCamping.Models
{
    [Table("Orders")]
    public class Order : BaseModel
    {
        [Key]
        public int ORD_ID { get; set; }
        public required int USE_ID { get; set; }
        public required DateTime OrderDate { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public decimal TotalPrice { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Note { get; set; }
        [ForeignKey("MEN_ID")]
        public virtual User? Menber { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}

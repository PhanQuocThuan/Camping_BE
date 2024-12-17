using System.ComponentModel;
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
        [DisplayName("Order Date")]
        public required DateTime OrderDate { get; set; }
        [DisplayName("Customer Name")]
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        [DisplayName("Totol Price")]
        public decimal TotalPrice { get; set; }
        [DisplayName("Payment Method")]
        public string? PaymentMethod { get; set; }
        public string? Note { get; set; }
        [ForeignKey("MEN_ID")]
        public virtual User? Menber { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}

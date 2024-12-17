using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCamping.Models
{
    [Table("Brands")]
    public class Brand : BaseModel
    {
        [Key]
        public int BRA_ID { get; set; }
        public string? Name { get; set; }
        public string? Avatar {  get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? BRA_Address { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCamping.Models
{
    [Table("Places")]
    public class Place : BaseModel
    {
        [Key]
        public int PLA_ID { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Image { get; set; }
        
    }
}

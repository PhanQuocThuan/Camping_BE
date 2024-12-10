using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCamping.Models
{
    [Table("Settings")]
    public class Setting : BaseModel
    {
        [Key]
        public int SET_ID { get; set; }
        public required string Name { get; set; }
        public string? Value { get; set; }
    }
}

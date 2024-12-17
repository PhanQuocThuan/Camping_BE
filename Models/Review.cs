using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCamping.Models
{
    [Table("Review")]
    public class Review : BaseModel
    {
        [Key]
        public int REV_ID { get; set; }
        public required int USE_ID { get; set; }
        public required int PRO_ID { get; set; }
        public required int BRA_ID { get; set; }
        public required string Content { get; set; }
        public required double Rate { get; set; }

        [ForeignKey("PRO_ID")]
        public virtual Product? Product { get; set; }

        [ForeignKey("MEN_ID")]
        public virtual User? Menber { get; set; }
    }
}

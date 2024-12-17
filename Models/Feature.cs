using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCamping.Models
{
    [Table("Features")]
    public class Feature : BaseModel
    {
        [Key]
        public int FEA_ID { get; set; }
        public required string Icon { get; set; }
        public required string Title { get; set; }
        [DisplayName("Sub Title")]
        public int Subtitle { get; set; }
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}

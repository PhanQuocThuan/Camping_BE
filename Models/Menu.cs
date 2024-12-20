using NuGet.Protocol.Plugins;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCamping.Models
{
    [Table("Menus")]
    public class Menu : BaseModel
    {
        [Key]
        public int MEN_ID { get; set; }
        public int PARENT_ID { get; set; }
        public required string Title { get; set; }
        public string? Url { get; set; }
        public int? DisplayOrder { get; set; }
        [ForeignKey("PARENT_ID")]
        public ICollection<Menu>? Children { get; set; }
    }

}

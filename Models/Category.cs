using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCamping.Models
{
    [Table("Categories")]
    public class Category : BaseModel
    {
        [Key]
        public int CAT_ID { get; set; }
        public required string Name { get; set; }
        public int DisplayOrder { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}

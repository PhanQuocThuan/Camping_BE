using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCamping.Models
{
    [Table("AdminUsers")]
    public class AdminUser :  BaseModel
    {
        [Key]
        public int AUSE_ID { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? Displayname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}

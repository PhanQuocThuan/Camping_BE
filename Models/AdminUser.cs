using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCamping.Models
{
    [Table("AdminUsers")]
    public class AdminUser :  BaseModel
    {
        [Key]
        public int ADU_ID { get; set; }

        [DisplayName("User Name")]
        public required string Username { get; set; }
        public required string Password { get; set; }
        [DisplayName("Display Name")]
        public string? Displayname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}

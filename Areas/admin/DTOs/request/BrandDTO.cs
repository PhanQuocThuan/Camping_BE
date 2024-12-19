using WebCamping.Models;

namespace WebCamping.Areas.Admin.DTOs.request
{
    public class BrandDTO
    {
        public int BRA_ID { get; set; }
        public string? Name { get; set; }
        public IFormFile? Avatar { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}

using System.ComponentModel;

namespace WebCamping.Areas.Admin.DTOs.request
{
    public class BannerDTO
    {
        public int BAN_ID { get; set; }
        public required string Title { get; set; }
        public IFormFile? Image { get; set; }
        public string? Url { get; set; }
        public int DisplayOrder { get; set; }
    }
}

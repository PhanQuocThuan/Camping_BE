using System.ComponentModel;

namespace WebCamping.Areas.Admin.DTOs.request
{
    public class FeatureDTO
    {
        public int FEA_ID { get; set; }
        public IFormFile? Icon { get; set; }
        public required string Title { get; set; }
        [DisplayName("Sub Title")]
        public string? Subtitle { get; set; }
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}

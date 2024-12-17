using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using WebCamping.Models;

namespace WebCamping.Areas.Admin.DTOs.request
{
    public class ProductDTO
    {
        public int PRO_ID { get; set; }
        public required int CAT_ID { get; set; }
        public required int BRA_ID { get; set; }
        public IFormFile? Avatar { get; set; }
        public required string Name { get; set; }
        public string? Intro { get; set; }
        public required decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public double? Rate { get; set; }
        public string? Description { get; set; }
        
    }
}

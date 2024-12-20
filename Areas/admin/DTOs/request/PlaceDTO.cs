namespace WebCamping.Areas.Admin.DTOs.request
{
    public class PlaceDTO
    {
        public int PLA_ID { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public IFormFile? Image { get; set; }

    }
}

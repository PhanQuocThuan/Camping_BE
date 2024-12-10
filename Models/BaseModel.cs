namespace WebCamping.Models
{
    public class BaseModel
    {
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public string? CreateBy { get; set; }
        public DateTime UpdateDate { get; set; } = DateTime.Now;
        public string? UpdateBy { get; set; }
    }
}

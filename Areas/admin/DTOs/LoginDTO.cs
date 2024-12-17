namespace WebCamping.Areas.admin.DTOs
{
    public class LoginDTO
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}

namespace WebApi.Core.Models
{
    public class AppSettings
    {
        public string ApplicationName { get; set; }
        public string JwtSecret { get; set; }
        public string JwtIssuer { get; set; }
    }
}

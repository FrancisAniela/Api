namespace WebApi.Core.Models
{
    public partial class ClientApplication
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ClientSecret { get; set; }
        public bool IsActive { get; set; }
    }
}

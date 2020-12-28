using System;

namespace WebApi.Core.Models
{
    public partial class AppConfig
    {
        public int Id { get; set; }
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Description { get; set; }
    }
}

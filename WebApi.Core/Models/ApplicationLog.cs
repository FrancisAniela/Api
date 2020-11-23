using System;

namespace WebApi.Core.Models
{
    public partial class ApplicationLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string LogType { get; set; }
        public string ApplicationName { get; set; }
        public string Message { get; set; }
        public string FormattedMessage { get; set; }
    }
}

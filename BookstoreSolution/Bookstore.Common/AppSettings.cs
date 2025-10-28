using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Common
{
    /// <summary>
    /// Application-wide configuration class to bind appsettings.json.
    /// </summary>
    public class AppSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public JwtSettings Jwt { get; set; } = new JwtSettings();
        public LoggingSettings Logging { get; set; } = new LoggingSettings();
    }

    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; } = 60;
    }

    public class LoggingSettings
    {
        public string LogPath { get; set; } = "Logs/log-.txt";
        public string MinimumLevel { get; set; } = "Information";
    }
}

using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Common
{
    /// <summary>
    /// Simple Serilog helper wrapper used by both API & Web.
    /// </summary>
    public static class Logger
    {
        private static bool _isInitialized = false;

        public static void Initialize(string logPath = "Logs/log-.txt", string minLevel = "Information")
        {
            if (_isInitialized) return;

            LogEventLevel level = Enum.TryParse(minLevel, out LogEventLevel parsedLevel)
                ? parsedLevel
                : LogEventLevel.Information;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(level)
                .WriteTo.Console()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
                .CreateLogger();

            _isInitialized = true;
        }

        public static void Information(string message) => Log.Information(message);
        public static void Warning(string message) => Log.Warning(message);
        public static void Error(string message, Exception? ex = null)
        {
            if (ex == null)
                Log.Error(message);
            else
                Log.Error(ex, message);
        }

        public static void CloseAndFlush() => Log.CloseAndFlush();
    }
}

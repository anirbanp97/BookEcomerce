using Serilog;
using Serilog.Events;

namespace Bookstore.Web.SerilogConfig
{
    public static class SerilogConfiguration
    {
        public static void ConfigureSerilog(WebApplicationBuilder builder)
        {
            var logPath = builder.Configuration["Logging:LogPath"] ?? "Logs/web-log-.txt";
            var level = builder.Configuration["Logging:MinimumLevel"] ?? "Information";

            LogEventLevel parsedLevel = Enum.TryParse(level, out LogEventLevel lvl)
                ? lvl : LogEventLevel.Information;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(parsedLevel)
                .WriteTo.Console()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}

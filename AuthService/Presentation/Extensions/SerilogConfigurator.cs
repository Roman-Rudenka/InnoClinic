using Serilog;

namespace Presentation.Extensions;

public static class SerilogConfigurator
{
    public static LoggerConfiguration Configure()
    {
        return new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
            .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
            .WriteTo.Console()
            .Enrich.FromLogContext();
    }
}
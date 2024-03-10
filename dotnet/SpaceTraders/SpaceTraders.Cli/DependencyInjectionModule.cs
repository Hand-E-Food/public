using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging;

namespace SpaceTraders.Cli
{
    public class DependencyInjectionModule : IDependencyInjectionModule
    {
        public void AddServices(IServiceCollection services)
        {
            services.AddLogging(ConfigureLogging);
            services.AddSingleton<IGlobalStateRepository, GlobalStateRepository>();
            services.AddSingleton<JsonStorage>();
            services.AddModule(new SpaceTraders.DependencyInjectionModule());
        }

        private void ConfigureLogging(ILoggingBuilder builder)
        {
            builder.AddSimpleConsole(ConfigureSimpleConsoleLogging);
            builder.SetMinimumLevel(LogLevel.Debug);
        }

        private void ConfigureSimpleConsoleLogging(SimpleConsoleFormatterOptions options)
        {
            options.ColorBehavior = LoggerColorBehavior.Enabled;
            options.IncludeScopes = false;
            options.SingleLine = true;
            options.TimestampFormat = "HH:mm:ss.fff ";
            options.UseUtcTimestamp = false;
        }
    }
}

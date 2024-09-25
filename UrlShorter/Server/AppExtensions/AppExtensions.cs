using Serilog;
using Server.Services.Caching;

namespace Server.AppExtensions
{
    public static class AppExtensions
    {

        public static void SerilogConfiguration(this IHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, logger) =>
            {
                logger.WriteTo.Console();
            });
        }
        public static void AddRedisCachingService(this IServiceCollection serviceDescriptors)
        {
            serviceDescriptors.AddTransient<IRedisCaching, RedisCaching>();
        }
    }
}

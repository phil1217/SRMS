
using SRMS.Models.Config;

namespace SRMS.Extensions
{
    public static class ConfigServiceCollectionExtensions
    {
        public static IServiceCollection AddConfig(
             this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AdminOptions>(config.GetSection(AdminOptions.Admin));
            services.Configure<MemberOptions>(config.GetSection(MemberOptions.Member));
            return services;
        }

        public static IServiceCollection AddConfigDependencyGroup(
             this IServiceCollection services)
        {
            services.AddScoped<AdminOptions>();
            services.AddScoped<MemberOptions>();
            return services;
        }
    }
}

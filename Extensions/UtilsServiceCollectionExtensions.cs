using SRMS.Data.Repository;
using SRMS.Interfaces;
using SRMS.Models.Database;
using SRMS.Utils;

namespace SRMS.Extensions
{
    public static class UtilsServiceCollectionExtensions
    {
        public static IServiceCollection AddUtilsDependencyGroup(
            this IServiceCollection services)
        {

            services.AddScoped<TokenProvider>();
            return services;
        }
    }
}

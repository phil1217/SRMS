using SRMS.Data.Repository;
using SRMS.Interfaces;
using SRMS.Models.Database;

namespace SRMS.Extensions
{
    public static class DataServiceCollectionExtensions
    {
        public static IServiceCollection AddDataDependencyGroup(
             this IServiceCollection services)
        {

            services.AddScoped<IRepository<StudentRecordModel>, StudentRecord>();
            services.AddScoped<IRepository<AcademicMemberModel>,AcademicMember>();
            services.AddScoped<IRepository<ProfileDetailsModel>, ProfileDetails>();
            return services;
        }
    }
}

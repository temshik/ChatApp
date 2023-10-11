using Microsoft.EntityFrameworkCore;
using ChatApp.Api.Modules;
using ChatApp.Api.Profiles;
using ChatApp.Bll.Interfaces;
using ChatApp.Bll.Services;
using ChatApp.DAL;

namespace ChatApp.Api.AppDependenciesConfiguration
{
    /// <summary>
    /// Static partial class for dependencies configuration.
    /// </summary>
    public static partial class AppDependenciesConfiguration
    {
        /// <summary>
        /// Adding an CatalogContext to interact with the account database.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>A <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureServicesApplication(option =>
                option.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        /// <summary>
        /// Function to add services to the application service.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>A <see cref="IServiceCollection"/></returns>
        public static IServiceCollection ConfigureService(this IServiceCollection services)
        {
            services.AddScoped<IDataService, DataService>();

            return services;
        }

        /// <summary>
        /// Function to add automapper to the application service.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>A <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(JsonConfigProfile));

            return services;
        }
    }
}

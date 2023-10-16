using ChatApp.DAL.EF;
using ChatApp.DAL.Entities;
using ChatApp.DAL.Interfaces;
using ChatApp.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.DAL
{
    /// <summary>
    /// Static partial class for dependencies configuration.
    /// </summary>
    public static partial class DataAccessDependenciesConfiguration
    {
        /// <summary>
        /// Function to add services to the application.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>A <see cref="IServiceCollection"/></returns>
        public static IServiceCollection ConfigureServicesApplication(this IServiceCollection services, Action<DbContextOptionsBuilder> options)
        {
            services
                .AddDbContext<ApplicationContext>(options)
                .AddScoped(serviceProvider =>
            serviceProvider.GetRequiredService<ApplicationContext>().Set<Room>())
                .AddScoped(serviceProvider =>
            serviceProvider.GetRequiredService<ApplicationContext>().Set<ApplicationUser>())
                 .AddScoped(serviceProvider =>
            serviceProvider.GetRequiredService<ApplicationContext>().Set<Message>());

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            /*services.AddScoped<IMessagesRepository, MessagesRepository>();
            services.AddScoped<IRoomsRepository, RoomsRepository>();*/

            return services;
        }
    }
}

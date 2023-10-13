using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ChatApp.DAL.EF;
using ChatApp.DAL.Interfaces;
using ChatApp.DAL.Repositories;
using ChatApp.DAL.Entities;

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
                .AddDbContext<ApplicationContext>(options);
            services
                .AddIdentityCore<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
                    options.User.RequireUniqueEmail = true;
                }).AddEntityFrameworkStores<ApplicationContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMessagesRepository, MessagesRepository>();
            services.AddScoped<IRoomsRepository, RoomsRepository>();

            return services;
        }
    }
}

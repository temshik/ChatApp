using ChatApp.Api.AppDependenciesConfiguration;

namespace ChatApp.AppDependenciesConfiguration
{
    public static partial class AppDependenciesConfiguration
    {
        /// <summary>
        /// Function to configure the service to connect to database
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            builder.Services
                    .AddServices(configuration)
                    .AddMappings()
                    .AddSingleR()
                    .ConfigureService();

            //.AddLogger(configuration);

            return builder;
        }
    }
}

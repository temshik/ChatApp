using ChatApp.Api.AppDependenciesConfiguration;
using ChatApp.Api.Config;

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
            configuration.AddJsonFile("config.json", optional: true, reloadOnChange: true);
            builder.Services
                    .AddServices(configuration)
                    .AddMappings()
                    .ConfigureService();
            builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
                builder.WithOrigins("http://localhost:3000", "http://localhost")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()));

            builder.Services.Configure<JsonConfig>(configuration.GetSection("JsonConfig"));
            /*.AddMappings()
            .AddLogger(configuration);*/

            return builder;
        }
    }
}

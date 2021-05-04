using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace J3space.Auth
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<J3AuthModule>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();
        }
    }
}
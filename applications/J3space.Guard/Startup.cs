using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace J3space.Guard
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<J3GuardModule>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.InitializeApplication();
        }
    }
}
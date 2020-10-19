using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace J3space.Auth.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<AuthIdentityServerModule>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();
        }
    }
}
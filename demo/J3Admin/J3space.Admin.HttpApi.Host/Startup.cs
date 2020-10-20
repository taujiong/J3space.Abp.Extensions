using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace J3space.Admin.HttpApi.Host
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication<AdminHttpApiHostModule>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.InitializeApplication();
        }
    }
}
using System;
using System.Linq;
using J3space.Abp.IdentityServer;
using J3space.Admin.EfCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.Threading;

namespace J3space.Admin
{
    [DependsOn(
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpAutofacModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpFeatureManagementEntityFrameworkCoreModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpIdentityServerApplicationModule),
        typeof(AbpIdentityServerEntityFrameworkCoreModule),
        typeof(AbpIdentityServerHttpApiModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpTenantManagementApplicationModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule),
        typeof(AbpTenantManagementHttpApiModule)
    )]
    public class J3AdminModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpAntiForgeryOptions>(options => { options.AutoValidate = false; });

            Configure<AbpAuditingOptions>(options => { options.ApplicationName = "J3Admin"; });

            Configure<AbpDbContextOptions>(options => { options.UseMySQL(); });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            });

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = bool.Parse(configuration["MultiTenancy"]);
            });

            context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.Audience = configuration["AuthServer:Audience"];
                    options.TokenValidationParameters.ValidIssuer = configuration["AuthServer:Authority"];
                });

            context.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            context.Services.AddAbpDbContext<AdminDbContext>(options => { options.AddDefaultRepositories(); });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();
            var configuration = context.GetConfiguration();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();
            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            app.UseAuthentication();
            app.UseAuthorization();

            if (bool.Parse(configuration["MultiTenancy"]))
            {
                app.UseMultiTenancy();
            }

            app.UseAuditing();
            app.UseConfiguredEndpoints();
        }

        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            AsyncHelper.RunSync(async () =>
            {
                using var scope = context.ServiceProvider.CreateScope();
                await scope.ServiceProvider
                    .GetRequiredService<AdminDbContext>()
                    .Database
                    .EnsureCreatedAsync();
                await scope.ServiceProvider
                    .GetRequiredService<IDataSeeder>()
                    .SeedAsync();
            });
        }
    }
}
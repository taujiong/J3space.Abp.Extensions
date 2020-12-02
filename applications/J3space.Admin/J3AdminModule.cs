using System;
using System.Collections.Generic;
using System.Linq;
using J3space.Abp.IdentityServer;
using J3space.Abp.SettingManagement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.Threading;

namespace J3space.Admin
{
    [DependsOn(
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpAutofacModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpFeatureManagementEntityFrameworkCoreModule),
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityServerApplicationModule),
        typeof(AbpIdentityServerEntityFrameworkCoreModule),
        typeof(AbpIdentityServerHttpApiModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementApplicationModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementHttpApiModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule)
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
                    options.RequireHttpsMetadata = bool.Parse(configuration["AuthServer:RequireHttpsMetadata"]);
                    options.Audience = configuration["AuthServer:Audience"];
                });

            context.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "J3space Admin Api",
                    Description =
                        "Contains audit logging, feature management, identity server and setting management",
                    Version = "v1"
                });
                options.DocInclusionPredicate((docName, description) => true);

                options.AddSecurityDefinition("J3Auth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    BearerFormat = JwtBearerDefaults.AuthenticationScheme,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{configuration["AuthServer:Authority"]}/connect/authorize"),
                            TokenUrl = new Uri($"{configuration["AuthServer:Authority"]}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"J3Admin", "Manage the features, identity server resources and settings"}
                            }
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "J3Auth"}
                        },
                        new[] {"J3Admin"}
                    }
                });
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

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "J3space Admin API");

                var swaggerSection = configuration.GetSection("SeedData:IdentityServer:Clients:AdminSwagger");
                options.OAuthConfigObject = new OAuthConfigObject
                {
                    ClientId = swaggerSection["ClientId"],
                    ClientSecret = swaggerSection["ClientSecret"],
                    AppName = configuration["AuthServer:Audience"]
                };
            });

            app.UseAuditing();
            app.UseConfiguredEndpoints();
        }

        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            AsyncHelper.RunSync(async () =>
            {
                using var scope = context.ServiceProvider.CreateScope();
                await scope.ServiceProvider
                    .GetRequiredService<IDataSeeder>()
                    .SeedAsync();
            });
        }
    }
}
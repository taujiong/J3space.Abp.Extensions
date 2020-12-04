using System;
using System.Collections.Generic;
using J3space.Abp.IdentityServer;
using J3space.Abp.SettingManagement;
using J3space.Blogging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;

namespace J3space.Guard
{
    [DependsOn(
        typeof(AbpAccountApplicationModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpAutofacModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpFeatureManagementApplicationModule),
        typeof(AbpFeatureManagementEntityFrameworkCoreModule),
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpIdentityServerHttpApiModule),
        typeof(AbpPermissionManagementApplicationModule),
        typeof(AbpPermissionManagementDomainIdentityModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpSettingManagementApplicationModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementHttpApiModule),
        typeof(AbpTenantManagementHttpApiModule),
        typeof(BloggingHttpApiModule)
    )]
    public class J3GuardModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpAntiForgeryOptions>(options => { options.AutoValidate = false; });

            Configure<AbpAuditingOptions>(options => { options.ApplicationName = "J3Guard"; });

            Configure<AbpDbContextOptions>(options => { options.UseMySQL(); });

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
                    options.TokenValidationParameters.ValidIssuer = configuration["AuthServer:Authority"];
                });

            context.Services.AddOcelot();

            context.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "J3space Gateway Api",
                    Description =
                        "Manage all the services of Abp and J3space",
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
                                {"J3Admin", "Manage the identity, identity server resources and tenancy of Abp system"},
                                {"J3Guard", "Manage the features, permission and settings of Abp system"},
                                {"J3Blogging", "Manage the blogging of J3space system"}
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
                        new[] {"J3Admin", "J3Guard", "J3Blogging"}
                    }
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var configuration = context.GetConfiguration();

            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();

            if (bool.Parse(configuration["MultiTenancy"]))
            {
                app.UseMultiTenancy();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "J3space Gateway API");

                options.OAuthConfigObject = new OAuthConfigObject
                {
                    ClientId = configuration["AuthServer:ClientId"],
                    ClientSecret = configuration["AuthServer:ClientSecret"],
                    AppName = configuration["AuthServer:Audience"]
                };
            });

            app.MapWhen(
                // TODO: 考虑扩展性
                ctx =>
                    ctx.Request.Path.ToString().StartsWith("/api/blogging/")
                    || ctx.Request.Path.ToString().StartsWith("/api/identity/")
                    || ctx.Request.Path.ToString().StartsWith("/api/ids/")
                    || ctx.Request.Path.ToString().StartsWith("/api/multi-tenancy/"),
                app2 => { app2.UseOcelot().Wait(); }
            );

            app.UseConfiguredEndpoints();
        }
    }
}
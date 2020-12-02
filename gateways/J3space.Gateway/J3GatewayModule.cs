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
using Volo.Abp.Autofac;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.TenantManagement;

namespace J3space.Gateway
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpFeatureManagementHttpApiModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpIdentityServerHttpApiModule),
        typeof(AbpSettingManagementHttpApiModule),
        typeof(AbpPermissionManagementHttpApiModule),
        typeof(AbpTenantManagementHttpApiModule),
        typeof(BloggingHttpApiModule)
    )]
    public class J3GatewayModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpMultiTenancyOptions>(options => { options.IsEnabled = false; });

            context.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "J3space Gateway Api",
                    Description =
                        "Management of all the services",
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
                                {"J3Admin", "Manage the features, identity server resources and settings"},
                                {"J3Auth", "Manage the identity, tenancy and permission"},
                                {"J3Blogging", "Manage all the settings for the blogging server"}
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
                        new[] {"J3Blogging"}
                    }
                });
            });

            context.Services.AddOcelot();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var configuration = context.GetConfiguration();

            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseAbpClaimsMap();

            if (bool.Parse(configuration["MultiTenancy"]))
            {
                // app.UseMultiTenancy();
            }

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
                ctx => ctx.Request.Path.ToString().StartsWith("/api/abp/") ||
                       ctx.Request.Path.ToString().StartsWith("/Abp/"),
                app2 =>
                {
                    app2.UseRouting();
                    app2.UseConfiguredEndpoints();
                }
            );

            app.UseOcelot().Wait();
        }
    }
}
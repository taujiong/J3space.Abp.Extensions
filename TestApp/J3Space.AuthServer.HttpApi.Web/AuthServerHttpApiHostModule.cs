using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using J3space.Abp.IdentityServer;
using J3space.Abp.IdentityServer.Web;
using J3space.AuthServer.MongoDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Identity.AspNetCore;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;

namespace J3space.AuthServer
{
    [DependsOn(
        typeof(AuthServerHttpApiModule),
        typeof(AbpAutofacModule),
        typeof(AuthServerApplicationModule),
        typeof(AuthServerMongoDbModule),
        // typeof(AbpAccountWebModule),
        typeof(AbpIdentityServerWebModule),
        typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
        typeof(AbpIdentityAspNetCoreModule),
        typeof(AbpAspNetCoreSerilogModule)
    )]
    public class AuthServerHttpApiHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            ConfigureAuthentication(context, configuration);
            ConfigureCors(context, configuration);
            ConfigureLocalization();
            ConfigureSwaggerServices(context);
            ConfigureUrls(configuration);
            ConfigureVirtualFileSystem(context);
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
            });
        }

        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<AuthServerDomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}J3space.AuthServer.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AuthServerDomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}J3space.AuthServer.Domain"));
                    options.FileSets
                        .ReplaceEmbeddedByPhysical<AuthServerApplicationContractsModule>(
                            Path.Combine(hostingEnvironment.ContentRootPath,
                                $"..{Path.DirectorySeparatorChar}J3space.AuthServer.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<AuthServerApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}J3space.AuthServer.Application"));
                });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context,
            IConfiguration configuration)
        {
            context.Services.AddAuthentication()
                .AddGitHub(options =>
                {
                    options.SignInScheme =
                        IdentityConstants.ExternalScheme;
                    options.ClientId = "22383998789876a9623d";
                    options.ClientSecret = "bc2556b4b4c9b1b8587ea894170f2e842228ca86";
                })
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "AuthServer";
                    options.JwtBackChannelHandler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler
                            .DangerousAcceptAnyServerCertificateValidator
                    };
                });

            Configure<RazorPagesOptions>(options =>
            {
                options.Conventions.AuthorizePage("/Account/Manage");
                options.Conventions.AuthorizePage("/Ids/Client",
                    IdentityServerPermissions.Client.Delete);
                options.Conventions.AuthorizePage("/Ids/ApiResource",
                    IdentityServerPermissions.ApiResource.Default);
                options.Conventions.AuthorizePage("/Ids/IdentityResource",
                    IdentityServerPermissions.IdentityResource.Default);
            });
        }

        private static void ConfigureSwaggerServices(ServiceConfigurationContext context)
        {
            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1",
                        new OpenApiInfo {Title = "AuthServer API", Version = "v1"});
                    options.DocInclusionPredicate((docName, description) => true);
                });
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            });
        }

        private void ConfigureCors(ServiceConfigurationContext context,
            IConfiguration configuration)
        {
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

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            app.UseAbpRequestLocalization();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthServer API"); });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}

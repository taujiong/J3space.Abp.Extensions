﻿using System;
using System.Linq;
using J3space.Abp.IdentityServer.Web;
using J3space.Auth.EntityFrameworkCore.DbMigrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Auditing;
using Volo.Abp.Autofac;
using Volo.Abp.Emailing;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;
using Volo.Abp.UI.Navigation.Urls;

namespace J3space.Auth.Web
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAccountApplicationModule),
        typeof(AbpAccountHttpApiModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAspNetCoreSerilogModule),
        typeof(AuthEntityFrameworkCoreDbMigrationsModule),
        typeof(AbpIdentityServerWebModule),
        typeof(AbpSettingManagementDomainModule)
    )]
    public class AuthIdentityServerModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                options.Languages.Add(new LanguageInfo("zh-Hant", "zh-Hant", "繁體中文"));
            });

            Configure<AbpAuditingOptions>(options => { options.ApplicationName = "J3Auth"; });

            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:RootUrl"];
            });

            context.Services.AddAuthentication()
                .AddGitHub(options =>
                {
                    options.SignInScheme =
                        IdentityConstants.ExternalScheme;
                    options.ClientId = configuration["ExternalIdentityProviders:GitHub:ClientId"];
                    options.ClientSecret = configuration["ExternalIdentityProviders:GitHub:ClientSecret"];
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
            var settingManager = context.ServiceProvider.GetService<SettingManager>();

            // 确保 Abp.Mailing.Smtp.Password 的 setting 值被加密
            var smtpPasswordName = EmailSettingNames.Smtp.Password;
            var password = configuration[$"Settings:{smtpPasswordName}"];
            settingManager.SetGlobalAsync(smtpPasswordName, password);

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
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }
    }
}
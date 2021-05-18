using System;
using System.Linq;
using IdentityServer4.Configuration;
using J3space.Abp.IdentityServer.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Emailing;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.UI.Navigation.Urls;

namespace J3space.Auth
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAccountApplicationModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreMySQLModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityServerEntityFrameworkCoreModule),
        typeof(AbpIdentityServerWebModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule)
    )]
    public class J3AuthModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpAuditingOptions>(options => { options.ApplicationName = "J3Auth"; });

            Configure<AbpDbContextOptions>(options => { options.UseMySQL(); });

            Configure<IdentityServerOptions>(options => { options.IssuerUri = configuration["AuthServer:IssuerUri"]; });

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

            Configure<AppUrlOptions>(options =>
            {
                var appUrls = configuration.GetSection("AppUrls");
                foreach (var urls in appUrls.GetChildren())
                {
                    options.Applications[urls.Key].RootUrl = urls.Value;
                    options.RedirectAllowedUrls.Add(urls.Value);
                }
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
                options.AddDefaultPolicy(builder =>
                {
                    var origins = configuration
                        .GetSection("CorsOrigins").GetChildren()
                        .Select(o => o.Value.RemovePostFix("/"))
                        .ToArray();
                    builder
                        .WithOrigins(origins)
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

            // 确保 Abp.Mailing.Smtp.Password 的 setting 值被加密
            var settingManager = context.ServiceProvider.GetService<SettingManager>();
            const string smtpPasswordName = EmailSettingNames.Smtp.Password;

            /* 需要加密的内容不能放在 appsettings.json 的 Settings 下面
             * 因为在加密过程中会依次查询该 SettingProvider 次序之前（优先级更低）的设置项是否存在该字段的值并进行比较
             * 当在 GlobalProvider 设置值，程序会在 ConfigurationProvider 发现该字段的未加密内容
             * 由于 appsettings.json 中保存的是未加密的值，所以进行解密操作时会报错
             */
            var password = configuration["AuthServer:EmailPassword"];
            settingManager.SetGlobalAsync(smtpPasswordName, password);

            // 程序运行在 http 时会设置 Cookie 的 SameSite=None
            // 在 Chrome 这类浏览器上进行登录操作时无法将身份认证的 Cookie 保存到浏览器，导致登录不成功
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Lax
            });

            app.UseAbpRequestLocalization();
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            if (bool.Parse(configuration["MultiTenancy"])) app.UseMultiTenancy();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseCors();

            app.UseAuditing();
            app.UseConfiguredEndpoints();
        }
    }
}
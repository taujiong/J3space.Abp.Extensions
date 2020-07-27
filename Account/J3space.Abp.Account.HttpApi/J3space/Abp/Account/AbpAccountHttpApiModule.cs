using J3space.Abp.Account.Localization;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace J3space.Abp.Account
{
    [DependsOn(
        typeof(AbpAccountApplicationContractsModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpAspNetCoreMvcModule))]
    public class AbpAccountHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpAccountHttpApiModule).Assembly);
            });
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<AbpAccountResource>()
                    .AddBaseTypes(typeof(AbpUiResource));
            });
        }
    }
}
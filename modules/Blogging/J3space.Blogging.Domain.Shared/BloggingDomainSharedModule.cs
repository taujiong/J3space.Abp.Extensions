using J3space.Blogging.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace J3space.Blogging
{
    [DependsOn(
        typeof(AbpLocalizationModule)
    )]
    public class BloggingDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<BloggingDomainSharedModule>("J3space.Blogging");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<BloggingResource>("en")
                    .AddVirtualJson("/Localization/Blogging");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Blogging", typeof(BloggingResource));
            });
        }
    }
}
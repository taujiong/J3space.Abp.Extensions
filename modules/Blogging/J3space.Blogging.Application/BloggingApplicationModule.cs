using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.BlobStoring;
using Volo.Abp.Modularity;

namespace J3space.Blogging
{
    [DependsOn(
        typeof(AbpAutoMapperModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpBlobStoringModule),
        typeof(BloggingDomainModule),
        typeof(BloggingApplicationContractsModule)
    )]
    public class BloggingApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<BloggingApplicationModule>();
            Configure<AbpAutoMapperOptions>(options => { options.AddMaps<BloggingApplicationModule>(validate: true); });
        }
    }
}
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace J3space.Blogging
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(BloggingDomainSharedModule)
    )]
    public class BloggingDomainModule : AbpModule
    {
    }
}
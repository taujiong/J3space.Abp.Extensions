using Volo.Abp.Application;
using Volo.Abp.IdentityServer;
using Volo.Abp.Modularity;

namespace J3space.Abp.IdentityServer
{
    [DependsOn(
        typeof(AbpDddApplicationModule),
        typeof(AbpIdentityServerDomainModule)
        )]
    public class AbpIdentityServerApplicationContractsModule : AbpModule
    {
    }
}

using J3space.Abp.IdentityServer.TestBase;
using Volo.Abp.Modularity;

namespace J3space.Abp.IdentityServer.Application.Test
{
    [DependsOn(
        typeof(AbpIdentityServerTestBaseModule),
        typeof(AbpIdentityServerApplicationModule)
    )]
    public class AbpIdentityServerApplicationTestModule : AbpModule
    {
    }
}
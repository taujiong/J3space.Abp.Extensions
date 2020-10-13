using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.Testing;

namespace J3space.Abp.IdentityServer.TestBase
{
    public class AbpIdentityServerTestBase<TStartupModule> : AbpIntegratedTest<TStartupModule>
        where TStartupModule : IAbpModule
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
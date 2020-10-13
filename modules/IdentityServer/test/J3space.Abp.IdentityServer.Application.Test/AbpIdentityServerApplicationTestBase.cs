using Volo.Abp;
using Volo.Abp.Testing;

namespace J3space.Abp.IdentityServer.Application.Test
{
    public class AbpIdentityServerApplicationTestBase : AbpIntegratedTest<AbpIdentityServerApplicationTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
using Volo.Abp;
using Volo.Abp.Testing;

namespace J3space.Abp.SettingManagement.Application.Test
{
    public class AbpSettingManagementApplicationTestBase : AbpIntegratedTest<AbpSettingManagementApplicationTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
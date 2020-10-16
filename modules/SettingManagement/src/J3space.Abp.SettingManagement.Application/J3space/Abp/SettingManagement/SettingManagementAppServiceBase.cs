using Volo.Abp.Application.Services;

namespace J3space.Abp.SettingManagement
{
    public abstract class SettingManagementAppServiceBase : ApplicationService
    {
        protected SettingManagementAppServiceBase()
        {
            ObjectMapperContext = typeof(AbpSettingManagementApplicationModule);
        }
    }
}
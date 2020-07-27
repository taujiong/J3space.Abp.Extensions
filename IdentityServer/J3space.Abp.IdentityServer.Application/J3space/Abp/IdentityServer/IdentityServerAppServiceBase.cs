using Volo.Abp.Application.Services;
using Volo.Abp.IdentityServer.Localization;

namespace J3space.Abp.IdentityServer
{
    public abstract class IdentityServerAppServiceBase : ApplicationService
    {
        protected IdentityServerAppServiceBase()
        {
            LocalizationResource = typeof(AbpIdentityServerResource);
            ObjectMapperContext = typeof(AbpIdentityServerApplicationModule);
        }
    }
}
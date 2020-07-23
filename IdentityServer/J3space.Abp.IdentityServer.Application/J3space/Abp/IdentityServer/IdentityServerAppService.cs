using Volo.Abp.Application.Services;
using Volo.Abp.IdentityServer.Localization;

namespace J3space.Abp.IdentityServer
{
    public abstract class IdentityServerAppService : ApplicationService
    {
        protected IdentityServerAppService()
        {
            LocalizationResource = typeof(AbpIdentityServerResource);
        }
    }
}

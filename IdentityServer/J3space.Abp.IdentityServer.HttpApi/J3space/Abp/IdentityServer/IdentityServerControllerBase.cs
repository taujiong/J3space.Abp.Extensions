using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.IdentityServer.Localization;

namespace J3space.Abp.IdentityServer
{
    public abstract class IdentityServerControllerBase : AbpController
    {
        protected IdentityServerControllerBase()
        {
            LocalizationResource = typeof(AbpIdentityServerResource);
        }
    }
}

using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.IdentityServer.Localization;

namespace J3space.Abp.IdentityServer
{
    public abstract class IdentityServerController : AbpController
    {
        protected IdentityServerController()
        {
            LocalizationResource = typeof(AbpIdentityServerResource);
        }
    }
}

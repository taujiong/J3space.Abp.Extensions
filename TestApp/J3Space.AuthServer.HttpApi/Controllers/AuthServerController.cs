using J3space.AuthServer.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace J3space.AuthServer.Controllers
{
    public abstract class AuthServerController : AbpController
    {
        protected AuthServerController()
        {
            LocalizationResource = typeof(AuthServerResource);
        }
    }
}

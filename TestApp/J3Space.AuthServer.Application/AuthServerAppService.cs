using J3space.AuthServer.Localization;
using Volo.Abp.Application.Services;

namespace J3space.AuthServer
{
    public abstract class AuthServerAppService : ApplicationService
    {
        protected AuthServerAppService()
        {
            LocalizationResource = typeof(AuthServerResource);
        }
    }
}
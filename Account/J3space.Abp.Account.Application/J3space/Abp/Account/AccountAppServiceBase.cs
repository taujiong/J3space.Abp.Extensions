using J3space.Abp.Account.Localization;
using Volo.Abp.Application.Services;

namespace J3space.Abp.Account
{
    public class AccountAppServiceBase : ApplicationService
    {
        protected AccountAppServiceBase()
        {
            LocalizationResource = typeof(AbpAccountResource);
            ObjectMapperContext = typeof(AbpAccountApplicationModule);
        }
    }
}

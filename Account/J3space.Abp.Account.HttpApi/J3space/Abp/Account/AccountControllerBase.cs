using J3space.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace J3space.Abp.Account
{
    public abstract class AccountControllerBase : AbpController
    {
        protected AccountControllerBase()
        {
            LocalizationResource = typeof(AbpAccountResource);
        }
    }
}
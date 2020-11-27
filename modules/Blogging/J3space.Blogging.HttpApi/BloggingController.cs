using J3space.Blogging.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace J3space.Blogging
{
    public abstract class BloggingController : AbpController
    {
        protected BloggingController()
        {
            LocalizationResource = typeof(BloggingResource);
        }
    }
}
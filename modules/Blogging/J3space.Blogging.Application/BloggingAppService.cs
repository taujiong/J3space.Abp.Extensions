using J3space.Blogging.Localization;
using Volo.Abp.Application.Services;

namespace J3space.Blogging
{
    public abstract class BloggingAppService : ApplicationService
    {
        protected BloggingAppService()
        {
            LocalizationResource = typeof(BloggingResource);
            ObjectMapperContext = typeof(BloggingApplicationModule);
        }
    }
}
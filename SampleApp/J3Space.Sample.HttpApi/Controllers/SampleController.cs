using J3space.Sample.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace J3space.Sample.Controllers
{
    public abstract class SampleController : AbpController
    {
        protected SampleController()
        {
            LocalizationResource = typeof(SampleResource);
        }
    }
}
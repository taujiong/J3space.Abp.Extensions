using J3space.Sample.Localization;
using Volo.Abp.Application.Services;

namespace J3space.Sample
{
    public abstract class SampleAppService : ApplicationService
    {
        protected SampleAppService()
        {
            LocalizationResource = typeof(SampleResource);
        }
    }
}
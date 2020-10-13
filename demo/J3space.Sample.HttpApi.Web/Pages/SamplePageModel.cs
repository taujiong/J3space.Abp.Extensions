using J3space.Sample.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace J3space.Sample.Pages
{
    public class SamplePageModel : AbpPageModel
    {
        protected SamplePageModel()
        {
            LocalizationResourceType = typeof(SampleResource);
        }
    }
}
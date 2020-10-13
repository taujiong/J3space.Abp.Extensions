using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace J3space.Sample
{
    [Dependency(ReplaceServices = true)]
    public class SampleBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Sample";
    }
}
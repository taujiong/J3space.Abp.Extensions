using J3space.Sample.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace J3space.Sample.Settings
{
    public class SampleSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    SampleSettings.App.Name,
                    "MyApplication",
                    L($"DisplayName:{SampleSettings.App.Name}"),
                    L($"Description:{SampleSettings.App.Name}"),
                    true),
                new SettingDefinition(
                    SampleSettings.App.Url,
                    null,
                    L($"DisplayName:{SampleSettings.App.Url}"),
                    L($"Description:{SampleSettings.App.Url}"),
                    true),
                new SettingDefinition(
                    SampleSettings.App.LogoUrl,
                    null,
                    L($"DisplayName:{SampleSettings.App.LogoUrl}"),
                    L($"Description:{SampleSettings.App.LogoUrl}"),
                    true)
            );
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<SampleResource>(name);
        }
    }
}
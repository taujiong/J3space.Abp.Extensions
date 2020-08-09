using J3space.AuthServer.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace J3space.AuthServer.Settings
{
    public class AuthServerSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                AuthServerSettings.App.Name,
                "MyApplication",
                L($"DisplayName:{AuthServerSettings.App.Name}"),
                L($"Description:{AuthServerSettings.App.Name}"),
                true),

                new SettingDefinition(
                    AuthServerSettings.App.Url,
                    null,
                    L($"DisplayName:{AuthServerSettings.App.Url}"),
                    L($"Description:{AuthServerSettings.App.Url}"),
                    true),

                new SettingDefinition(
                    AuthServerSettings.App.LogoUrl,
                    null,
                    L($"DisplayName:{AuthServerSettings.App.LogoUrl}"),
                    L($"Description:{AuthServerSettings.App.LogoUrl}"),
                    true)
            );
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AuthServerResource>(name);
        }

    }
}

using Volo.Abp.Settings;

namespace J3space.Abp.SettingManagement.TestBase
{
    public class TestSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(new SettingDefinition("SettingWithoutDefaultValue"));
            context.Add(new SettingDefinition("SettingWithDefaultValue", "default_value_1"));
            context.Add(new SettingDefinition("SettingWithProperty", defaultValue: "default_value_2")
                .WithProperty("MainGroup", "main")
                .WithProperty("SubGroup", "sub"));
        }
    }
}
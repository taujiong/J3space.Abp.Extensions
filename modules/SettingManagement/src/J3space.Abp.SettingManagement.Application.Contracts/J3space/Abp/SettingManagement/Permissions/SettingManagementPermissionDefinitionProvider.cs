using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement.Localization;

namespace J3space.Abp.SettingManagement.Permissions
{
    public class SettingManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var settingManagementGroup = context.AddGroup(
                SettingManagementPermissions.GroupName,
                L($"Permission:{SettingManagementPermissions.GroupName}"));

            settingManagementGroup.AddPermission(
                SettingManagementPermissions.Setting.Manage,
                L($"Permission:{SettingManagementPermissions.Setting.Manage}"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpSettingManagementResource>(name);
        }
    }
}
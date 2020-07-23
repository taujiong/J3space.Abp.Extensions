using Volo.Abp.Authorization.Permissions;
using Volo.Abp.IdentityServer.Localization;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace J3space.Abp.IdentityServer
{
    public class IdentityServerPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var identityServerGroup = context.AddGroup(IdentityServerPermissions.GroupName,
                L("Permission:IdentityServerManagement"));

            var clientsPermission = identityServerGroup.AddPermission(IdentityServerPermissions.Client.Default,
                L("Permission:ClientManagement"));
            clientsPermission.AddChild(IdentityServerPermissions.Client.Create, L("Permission:Create"));
            clientsPermission.AddChild(IdentityServerPermissions.Client.Update, L("Permission:Edit"));
            clientsPermission.AddChild(IdentityServerPermissions.Client.Delete, L("Permission:Delete"));
            clientsPermission.AddChild(IdentityServerPermissions.Client.ManagePermissions,
                L("Permission:ChangePermissions"),
                MultiTenancySides.Host);

            var apiResourcesPermission = identityServerGroup.AddPermission(IdentityServerPermissions.ApiResource.Default,
                L("Permission:ApiResourceManagement"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.ApiResource.Create,
                L("Permission:Create"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.ApiResource.Update,
                L("Permission:Edit"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.ApiResource.Delete,
                L("Permission:Delete"));

            var identityResourcesPermission = identityServerGroup.AddPermission(
                IdentityServerPermissions.IdentityResource.Default,
                L("Permission:IdentityResourceManagement"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.IdentityResource.Create,
                L("Permission:Create"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.IdentityResource.Update,
                L("Permission:Edit"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.IdentityResource.Delete,
                L("Permission:Delete"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpIdentityServerResource>(name);
        }
    }
}

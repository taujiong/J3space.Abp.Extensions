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

            var clientsPermission = identityServerGroup.AddPermission(IdentityServerPermissions.Clients.Default,
                L("Permission:ClientManagement"));
            clientsPermission.AddChild(IdentityServerPermissions.Clients.Create, L("Permission:Create"));
            clientsPermission.AddChild(IdentityServerPermissions.Clients.Update, L("Permission:Edit"));
            clientsPermission.AddChild(IdentityServerPermissions.Clients.Delete, L("Permission:Delete"));
            clientsPermission.AddChild(IdentityServerPermissions.Clients.ManagePermissions,
                L("Permission:ChangePermissions"),
                MultiTenancySides.Host);

            var apiResourcesPermission = identityServerGroup.AddPermission(IdentityServerPermissions.ApiResources.Default,
                L("Permission:ApiResourcesManagement"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.ApiResources.Create,
                L("Permission:Create"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.ApiResources.Update,
                L("Permission:Edit"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.ApiResources.Delete,
                L("Permission:Delete"));

            var identityResourcesPermission = identityServerGroup.AddPermission(
                IdentityServerPermissions.IdentityResources.Default,
                L("Permission:IdentityResourcesManagement"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.IdentityResources.Create,
                L("Permission:Create"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.IdentityResources.Update,
                L("Permission:Edit"));
            apiResourcesPermission.AddChild(IdentityServerPermissions.IdentityResources.Delete,
                L("Permission:Delete"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AbpIdentityServerResource>(name);
        }
    }
}

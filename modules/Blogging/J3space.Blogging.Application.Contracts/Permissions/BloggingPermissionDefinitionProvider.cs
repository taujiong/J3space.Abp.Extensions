using J3space.Blogging.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace J3space.Blogging.Permissions
{
    public class BloggingPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var bloggingGroup = context.AddGroup(BloggingPermissions.GroupName, L("Permission:Blogging"));

            var posts = bloggingGroup.AddPermission(BloggingPermissions.Posts.Default, L("Permission:Posts"));
            posts.AddChild(BloggingPermissions.Posts.Update, L("Permission:Edit"));
            posts.AddChild(BloggingPermissions.Posts.Delete, L("Permission:Delete"));
            posts.AddChild(BloggingPermissions.Posts.Create, L("Permission:Create"));

            var blobs = bloggingGroup.AddPermission(BloggingPermissions.Blobs.Default, L("Permission:Blobs"));
            blobs.AddChild(BloggingPermissions.Blobs.Create, L("Permission:Create"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<BloggingResource>(name);
        }
    }
}
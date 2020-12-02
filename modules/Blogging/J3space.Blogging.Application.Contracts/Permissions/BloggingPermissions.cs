using Volo.Abp.Reflection;

namespace J3space.Blogging.Permissions
{
    public class BloggingPermissions
    {
        public const string GroupName = "Blogging";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(BloggingPermissions));
        }

        public static class Posts
        {
            public const string Default = GroupName + ".Post";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
        }

        public static class Blobs
        {
            public const string Default = GroupName + ".Blob";
            public const string Create = Default + ".Create";
        }
    }
}
namespace J3space.Abp.IdentityServer
{
    public static class IdentityServerPermissions
    {
        public const string GroupName = "IdentityServer";

        public static class Client
        {
            public const string Default = GroupName + ".Client";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class ApiResource
        {
            public const string Default = GroupName + ".ApiResource";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static class IdentityResource
        {
            public const string Default = GroupName + ".IdentityResource";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
    }
}
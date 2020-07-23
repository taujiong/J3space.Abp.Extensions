﻿namespace J3space.Abp.IdentityServer
{
    public static class IdentityServerPermissions
    {
        public const string GroupName = "IdentityServer";

        public static class Clients
        {
            public const string Default = GroupName + ".Clients";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            public const string ManagePermissions = Default + ".ManagePermissions";
        }

        public static class ApiResources
        {
            public const string Default = GroupName + ".ApiResources";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static class IdentityResources
        {
            public const string Default = GroupName + ".IdentityResources";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
    }
}

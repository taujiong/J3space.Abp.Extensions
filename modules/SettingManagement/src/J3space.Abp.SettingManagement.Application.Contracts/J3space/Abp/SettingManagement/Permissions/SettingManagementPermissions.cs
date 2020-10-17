namespace J3space.Abp.SettingManagement.Permissions
{
    public static class SettingManagementPermissions
    {
        public const string GroupName = "SettingManagement";

        public static class Setting
        {
            public const string Default = GroupName + ".Setting";

            public const string Manage = Default + ".Manage";
        }
    }
}
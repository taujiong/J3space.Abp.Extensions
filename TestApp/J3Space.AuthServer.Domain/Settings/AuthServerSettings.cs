namespace J3space.AuthServer.Settings
{
    public static class AuthServerSettings
    {
        private const string Prefix = "AuthServer";

        public static class App
        {
            private const string AppPrefix = "App";
            public const string Name = AppPrefix + ".Name";
            public const string Url = AppPrefix + ".Url";
            public const string LogoUrl = AppPrefix + ".LogoUrl";
        }
    }
}

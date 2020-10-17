namespace J3space.Abp.Account.Web
{
    public class AbpAccountOptions
    {
        public AbpAccountOptions()
        {
            //TODO: This makes us depend on the Microsoft.AspNetCore.Server.IISIntegration package.
            WindowsAuthenticationSchemeName =
                "Windows"; //Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;
        }

        public string WindowsAuthenticationSchemeName { get; set; }
    }
}
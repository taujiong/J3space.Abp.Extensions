using IdentityServer4.Models;

namespace J3space.Abp.IdentityServer.Web.Pages.Consent
{
    public class ClientInfoModel
    {
        public ClientInfoModel(Client client)
        {
            ClientName = client.ClientId;
            ClientUrl = client.ClientUri;
            ClientLogoUrl = client.LogoUri;
            AllowRememberConsent = client.AllowRememberConsent;
        }

        public string ClientName { get; set; }

        public string ClientUrl { get; set; }

        public string ClientLogoUrl { get; set; }

        public bool AllowRememberConsent { get; set; }
    }
}
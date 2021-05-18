using System.Collections.Generic;

namespace J3space.Abp.IdentityServer.Clients.Dto
{
    public class ClientUpdateDto
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public bool Enabled { get; set; }

        public bool RequireConsent { get; set; }
        public bool RequireRequestObject { get; set; }
        public bool AllowRememberConsent { get; set; }
        public bool AllowOfflineAccess { get; set; }

        public bool BackChannelLogoutSessionRequired { get; set; }
        public string BackChannelLogoutUri { get; set; }
        public bool FrontChannelLogoutSessionRequired { get; set; }
        public string FrontChannelLogoutUri { get; set; }

        public int AccessTokenLifetime { get; set; }
        public int AccessTokenType { get; set; }
        public int? ConsentLifetime { get; set; }
        public int? UserSsoLifetime { get; set; }
        public bool RequirePkce { get; set; }
        public bool RequireClientSecret { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }

        public List<ClientClaimDto> Claims { get; set; }
        public List<string> AllowedScopes { get; set; }
        public List<string> AllowedGrantTypes { get; set; }
        public List<string> RedirectUris { get; set; }
        public List<string> PostLogoutRedirectUris { get; set; }
        public List<string> AllowedCorsOrigins { get; set; }
        public List<ClientSecretDto> ClientSecrets { get; set; }
        public List<ClientPropertyDto> Properties { get; set; }
    }
}
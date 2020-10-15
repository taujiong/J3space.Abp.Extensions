using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace J3space.Abp.IdentityServer.Clients.Dto
{
    public class ClientGetUpdateDto : FullAuditedEntityDto<Guid>
    {
        public ClientGetUpdateDto()
        {
            Claims = new List<ClientClaimDto>();
            Properties = new List<ClientPropertyDto>();
            AllowedScopes = new List<string>();
            ClientSecrets = new List<ClientSecretDto>();
            RedirectUris = new List<string>();
            AllowedGrantTypes = new List<string>();
            AllowedCorsOrigins = new List<string>();
            PostLogoutRedirectUris = new List<string>();
            IdentityProviderRestrictions = new List<string>();
        }

        #region Basic

        public string ClientName { get; set; }
        public string Description { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public bool Enabled { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public bool EnableLocalLogin { get; set; }

        #endregion

        #region Consent

        public bool RequireConsent { get; set; }
        public int? ConsentLifetime { get; set; }
        public bool AllowRememberConsent { get; set; }

        #endregion

        #region Application Urls

        public List<string> AllowedCorsOrigins { get; set; }
        public List<string> RedirectUris { get; set; }
        public List<string> PostLogoutRedirectUris { get; set; }

        #endregion

        #region Token

        public int AccessTokenLifetime { get; set; }
        public int IdentityTokenLifetime { get; set; }
        public int AuthorizationCodeLifetime { get; set; }
        public int AbsoluteRefreshTokenLifetime { get; set; }
        public int SlidingRefreshTokenLifetime { get; set; }
        public int? UserSsoLifetime { get; set; }
        public int DeviceCodeLifetime { get; set; }
        public bool RequireClientSecret { get; set; }
        public bool RequirePkce { get; set; }
        public bool AllowOfflineAccess { get; set; }

        #endregion

        #region Others

        public List<ClientSecretDto> ClientSecrets { get; set; }
        public List<string> AllowedScopes { get; set; }
        public List<ClientClaimDto> Claims { get; set; }
        public List<string> AllowedGrantTypes { get; set; }
        public List<string> IdentityProviderRestrictions { get; set; }
        public List<ClientPropertyDto> Properties { get; set; }

        #endregion
    }
}
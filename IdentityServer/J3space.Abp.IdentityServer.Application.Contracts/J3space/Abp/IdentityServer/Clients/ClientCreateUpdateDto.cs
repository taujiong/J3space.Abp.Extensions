using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;

namespace J3space.Abp.IdentityServer.Clients
{
    public class ClientCreateUpdateDto
    {
        [Required]
        [StringLength(ClientConsts.ClientIdMaxLength)]
        public string ClientId { get; set; }

        public string LogoUri { get; set; }

        [StringLength(ClientConsts.DescriptionMaxLength)]
        public string Description { get; set; }

        public List<string> AllowedScopes { get; set; }
        public List<string> AllowedGrantTypes { get; set; }
        public List<string> AllowedCorsOrigins { get; set; }
        public List<string> RedirectUris { get; set; }
        public List<string> PostLogoutRedirectUris { get; set; }
        public List<string> ClientSecrets { get; set; }
        public bool RequireConsent { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
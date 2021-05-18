using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.Validation;

namespace J3space.Abp.IdentityServer.Clients.Dto
{
    public class ClientCreateDto
    {
        [Required]
        [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.ClientIdMaxLength))]
        public string ClientId { get; set; }

        [Required]
        [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.ClientNameMaxLength))]
        public string ClientName { get; set; }

        [DynamicStringLength(typeof(ClientConsts), nameof(ClientConsts.DescriptionMaxLength))]
        public string Description { get; set; }

        public string ClientUri { get; set; }

        public string LogoUri { get; set; }

        public bool RequireConsent { get; set; }

        public List<string> AllowedScopes { get; set; }

        public List<string> RedirectUrls { get; set; }

        public List<string> PostLogoutUrls { get; set; }

        public List<ClientSecretDto> ClientSecrets { get; set; }
    }
}
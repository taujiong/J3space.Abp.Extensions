using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.Clients;

namespace J3space.Abp.IdentityServer.Clients.Dto
{
    public class ClientCreateDto
    {
        [Required]
        [StringLength(ClientConsts.ClientIdMaxLength)]
        public string ClientId { get; set; }

        [Required]
        [StringLength(ClientConsts.ClientNameMaxLength)]
        public string ClientName { get; set; }

        [StringLength(ClientConsts.DescriptionMaxLength)]
        public string Description { get; set; }

        public string ClientUri { get; set; }

        public string LogoUri { get; set; }

        public string LogoutUri { get; set; }

        public ClientType ClientType { get; set; } = ClientType.Empty;
    }
}
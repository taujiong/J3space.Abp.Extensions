using System;
using IdentityServer4;

namespace J3space.Abp.IdentityServer.Clients.Dto
{
    public class ClientSecretDto
    {
        public string Type { get; set; } = IdentityServerConstants.SecretTypes.SharedSecret;
        public string Value { get; set; }
        public string Description { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
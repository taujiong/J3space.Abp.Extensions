using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace J3space.Abp.IdentityServer.Clients
{
    public class ClientDto : ExtensibleEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string ClientId { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public bool Enabled { get; set; }
        public string Description { get; set; }
        public List<string> AllowedScopes { get; set; }
        public List<string> AllowedGrantTypes { get; set; }
        public List<string> AllowedCorsOrigins { get; set; }
        public List<string> RedirectUris { get; set; }
        public List<string> PostLogoutRedirectUris { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace J3space.Abp.IdentityServer.ApiResources.Dto
{
    public class ApiResourceDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public List<string> Scopes { get; set; }
        public List<string> UserClaims { get; set; }
    }
}
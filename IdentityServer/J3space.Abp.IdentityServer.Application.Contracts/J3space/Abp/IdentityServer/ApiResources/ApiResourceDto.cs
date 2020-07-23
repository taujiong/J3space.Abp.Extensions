using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace J3space.Abp.IdentityServer.ApiResources
{
    public class ApiResourceDto : EntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<string> Scopes { get; set; }
        public List<string> UserClaims { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
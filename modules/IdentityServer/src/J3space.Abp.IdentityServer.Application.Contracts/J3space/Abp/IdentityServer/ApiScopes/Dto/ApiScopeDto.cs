﻿using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace J3space.Abp.IdentityServer.ApiScopes.Dto
{
    public class ApiScopeDto : ExtensibleFullAuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }

        public List<string> UserClaims { get; set; }
        public List<ApiScopePropertyDto> Properties { get; set; }
    }
}
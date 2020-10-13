﻿using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace J3space.Abp.IdentityServer.IdentityResources
{
    public class IdentityResourceDto : Entity<Guid>, IHasConcurrencyStamp
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public List<string> UserClaims { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
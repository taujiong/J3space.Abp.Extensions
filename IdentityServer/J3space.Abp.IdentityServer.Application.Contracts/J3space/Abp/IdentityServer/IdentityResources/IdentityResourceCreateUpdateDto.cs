﻿using System.Collections.Generic;

 namespace J3space.Abp.IdentityServer.IdentityResources
{
    public class IdentityResourceCreateUpdateDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public List<string> UserClaims { get; set; }
    }
}
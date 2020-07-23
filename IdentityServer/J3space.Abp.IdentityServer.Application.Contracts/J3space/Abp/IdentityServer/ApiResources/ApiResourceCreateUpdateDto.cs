﻿using System.Collections.Generic;

 namespace J3space.Abp.IdentityServer.ApiResources
{
    public class ApiResourceCreateUpdateDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<string> Scopes { get; set; }
        public List<string> UserClaims { get; set; }
    }
}
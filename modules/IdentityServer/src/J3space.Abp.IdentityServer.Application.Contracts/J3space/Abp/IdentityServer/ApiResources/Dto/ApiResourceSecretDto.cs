using System;

namespace J3space.Abp.IdentityServer.ApiResources.Dto
{
    public class ApiResourceSecretDto
    {
        public string Value { get; set; }
        public string Description { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
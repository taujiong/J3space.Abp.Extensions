using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using J3space.Abp.IdentityServer.IdentityResources.Dto;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.Validation;

namespace J3space.Abp.IdentityServer.ApiScopes.Dto
{
    public class ApiScopeCreateUpdateDto
    {
        [Required]
        [DynamicStringLength(typeof(ApiScopeConsts),
            nameof(ApiScopeConsts.NameMaxLength))]
        public string Name { get; set; }

        [DynamicStringLength(typeof(ApiScopeConsts),
            nameof(ApiScopeConsts.DisplayNameMaxLength))]
        public string DisplayName { get; set; }

        [DynamicStringLength(typeof(ApiScopeConsts),
            nameof(ApiScopeConsts.DescriptionMaxLength))]
        public string Description { get; set; }

        public bool Enabled { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }

        public List<string> UserClaims { get; set; }
        public List<IdentityResourcePropertyDto> Properties { get; set; }
    }
}
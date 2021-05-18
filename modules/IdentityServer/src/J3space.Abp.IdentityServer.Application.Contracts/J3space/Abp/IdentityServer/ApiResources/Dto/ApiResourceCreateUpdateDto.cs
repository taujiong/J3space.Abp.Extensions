using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.Validation;

namespace J3space.Abp.IdentityServer.ApiResources.Dto
{
    public class ApiResourceCreateUpdateDto
    {
        [Required]
        [DynamicStringLength(typeof(ApiResourceConsts), nameof(ApiResourceConsts.NameMaxLength))]
        public string Name { get; set; }

        [DynamicStringLength(typeof(ApiResourceConsts), nameof(ApiResourceConsts.DisplayNameMaxLength))]
        public string DisplayName { get; set; }

        [DynamicStringLength(typeof(ApiResourceConsts), nameof(ApiResourceConsts.DescriptionMaxLength))]
        public string Description { get; set; }

        public bool Enabled { get; set; }

        public bool ShowInDiscoveryDocument { get; set; }

        public List<ApiResourceSecretDto> Secrets { get; set; }
        public List<string> Scopes { get; set; }
        public List<string> UserClaims { get; set; }
        public List<ApiResourcePropertyDto> Properties { get; set; }
    }
}
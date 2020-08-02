using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.IdentityResources;
using Volo.Abp.Validation;

namespace J3space.Abp.IdentityServer.IdentityResources
{
    public class IdentityResourceCreateUpdateDto
    {
        [Required]
        [DynamicStringLength(typeof(IdentityResourceConsts),
            nameof(IdentityResourceConsts.NameMaxLength))]
        public string Name { get; set; }

        [DynamicStringLength(typeof(IdentityResourceConsts),
            nameof(IdentityResourceConsts.DisplayNameMaxLength))]
        public string DisplayName { get; set; }

        [DynamicStringLength(typeof(IdentityResourceConsts),
            nameof(IdentityResourceConsts.DescriptionMaxLength))]
        public string Description { get; set; }

        public bool Enabled { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public List<string> UserClaims { get; set; }
    }
}
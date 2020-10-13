using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.IdentityServer.ApiResources;

namespace J3space.Abp.IdentityServer.ApiResources
{
    public class ApiResourceCreateUpdateDto
    {
        [Required]
        [StringLength(ApiResourceConsts.NameMaxLength)]
        public string Name { get; set; }

        [StringLength(ApiResourceConsts.DisplayNameMaxLength)]
        public string DisplayName { get; set; }

        [StringLength(ApiResourceConsts.DescriptionMaxLength)]
        public string Description { get; set; }

        public bool Enabled { get; set; }
        public List<string> Scopes { get; set; }
        public List<string> UserClaims { get; set; }
    }
}
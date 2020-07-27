using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace J3space.Abp.Account
{
    public class LoginDto
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts),
            nameof(IdentityUserConsts.MaxEmailLength))]
        public string UserNameOrEmailAddress { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts),
            nameof(IdentityUserConsts.MaxPasswordLength))]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Account;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Validation;

namespace J3space.Abp.Account.Web.Pages.Account
{
    public class ResetPasswordModel : AccountPageModel
    {
        public ResetPasswordModel(ITenantResolveResultAccessor tenantResolveResultAccessor)
        {
            TenantResolveResultAccessor = tenantResolveResultAccessor;
        }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid? TenantId { get; set; }

        [Required]
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid UserId { get; set; }

        [Required]
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ResetToken { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DisableAuditing]
        public string Password { get; set; }

        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [DisableAuditing]
        public string ConfirmPassword { get; set; }

        protected virtual ITenantResolveResultAccessor TenantResolveResultAccessor { get; }

        public virtual Task<IActionResult> OnGetAsync()
        {
            //TODO: It would be good to try to switch tenant if needed
            CheckCurrentTenant(TenantId);
            return Task.FromResult<IActionResult>(Page());
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            try
            {
                ValidateModel();

                await AccountAppService.ResetPasswordAsync(
                    new ResetPasswordDto
                    {
                        UserId = UserId,
                        ResetToken = ResetToken,
                        Password = Password
                    }
                );
            }
            catch (Exception e)
            {
                var message = GetLocalizeExceptionMessage(e);
                MyAlerts.Warning(message, L["OperationFailed"]);
                return await OnGetAsync();
            }

            return RedirectToPage("./ResetPasswordConfirmation", new
            {
                returnUrl = ReturnUrl,
                returnUrlHash = ReturnUrlHash
            });
        }

        protected override void ValidateModel()
        {
            if (!Equals(Password, ConfirmPassword))
                ModelState.AddModelError("ConfirmPassword",
                    L["'{0}' and '{1}' do not match.", L["ConfirmPassword"], L["DisplayName:Password"]]);

            base.ValidateModel();
        }
    }
}
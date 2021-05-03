using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;

namespace J3space.Auth
{
    public class IdentityClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
    {
        public async Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
        {
            var userManager = context.ServiceProvider.GetRequiredService<IdentityUserManager>();
            var user = await userManager.GetUserAsync(context.ClaimsPrincipal);
            if (user != null)
            {
                var preferredName = user.Surname + user.Name;
                var identity = context.ClaimsPrincipal.Identities.FirstOrDefault();
                identity.AddOrReplace(new Claim(JwtClaimTypes.PreferredUserName, preferredName));
            }
        }
    }
}
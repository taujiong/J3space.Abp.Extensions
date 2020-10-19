using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace J3space.Admin.Domain.Data
{
    public class EmptyIdentityDataSeeder : IIdentityDataSeeder, ITransientDependency
    {
        public Task<IdentityDataSeedResult> SeedAsync(string adminEmail, string adminPassword, Guid? tenantId = null)
        {
            return Task.FromResult(new IdentityDataSeedResult
            {
                CreatedAdminRole = true,
                CreatedAdminUser = false
            });
        }
    }
}
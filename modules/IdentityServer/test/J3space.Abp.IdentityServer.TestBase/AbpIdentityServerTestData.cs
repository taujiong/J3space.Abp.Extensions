using System;
using Volo.Abp.DependencyInjection;

namespace J3space.Abp.IdentityServer.TestBase
{
    public class AbpIdentityServerTestData : ISingletonDependency
    {
        public Guid Client1Id { get; } = Guid.NewGuid();

        public Guid ApiResource1Id { get; } = Guid.NewGuid();

        public Guid IdentityResource1Id { get; } = Guid.NewGuid();
    }
}
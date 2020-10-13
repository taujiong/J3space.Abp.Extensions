using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.IdentityResources;
using J3space.Abp.IdentityServer.TestBase;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.IdentityResources;
using Xunit;

namespace J3space.Abp.IdentityServer.Application.Test
{
    public sealed class IdentityResourceAppServiceTests : AbpIdentityServerApplicationTestBase
    {
        private readonly IIdentityResourceAppService _identityResourceAppService;
        private readonly AbpIdentityServerTestData _testData;

        public IdentityResourceAppServiceTests()
        {
            _identityResourceAppService = GetRequiredService<IIdentityResourceAppService>();
            _testData = GetRequiredService<AbpIdentityServerTestData>();
        }

        [Fact]
        public async Task Should_Get_ApiResource_By_Id()
        {
            var result = await _identityResourceAppService.GetAsync(_testData.IdentityResource1Id);
            result.ShouldNotBeNull();
            result.Name.ShouldBe("NewIdentityResource1");
            result.UserClaims.ShouldContain(nameof(ApiResourceClaim.Type));
        }

        [Fact]
        public async Task Should_Throw_Exception_With_Wrong_Id()
        {
            var id = Guid.NewGuid();

            var e = await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _identityResourceAppService.GetAsync(id));
            e.EntityType.ShouldBe(typeof(IdentityResource));

            e = await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _identityResourceAppService.DeleteAsync(id));
            e.EntityType.ShouldBe(typeof(IdentityResource));
        }

        [Fact]
        public async Task Should_Create_Update_Success()
        {
            var input = new IdentityResourceCreateUpdateDto()
            {
                Name = "test",
                UserClaims = new List<string> {"Age"}
            };
            (await _identityResourceAppService.CreateAsync(input)).ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Get_List_Of_ApiResources()
        {
            var result = await _identityResourceAppService.GetListAsync(new PagedAndSortedResultRequestDto());
            result.TotalCount.ShouldBe(3);
        }

        [Fact]
        public async Task Should_Delete_ApiResource_By_Id()
        {
            await _identityResourceAppService.DeleteAsync(_testData.IdentityResource1Id);
            var list = await _identityResourceAppService.GetListAsync(new PagedAndSortedResultRequestDto());
            list.TotalCount.ShouldBe(2);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.ApiResources;
using J3space.Abp.IdentityServer.TestBase;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.IdentityServer.ApiResources;
using Xunit;

namespace J3space.Abp.IdentityServer.Application.Test
{
    public sealed class ApiResourceAppServiceTests : AbpIdentityServerApplicationTestBase
    {
        private readonly IApiResourceAppService _apiResourceAppService;
        private readonly AbpIdentityServerTestData _testData;

        public ApiResourceAppServiceTests()
        {
            _apiResourceAppService = GetRequiredService<IApiResourceAppService>();
            _testData = GetRequiredService<AbpIdentityServerTestData>();
        }

        [Fact]
        public async Task Should_Get_ApiResource_By_Id()
        {
            var result = await _apiResourceAppService.GetAsync(_testData.ApiResource1Id);
            result.ShouldNotBeNull();
            result.Name.ShouldBe("NewApiResource1");
            result.Scopes.ShouldContain(nameof(ApiScope.Name));
        }

        [Fact]
        public async Task Should_Throw_Exception_With_Wrong_Id()
        {
            var id = Guid.NewGuid();

            var e = await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _apiResourceAppService.GetAsync(id));
            e.EntityType.ShouldBe(typeof(ApiResource));

            e = await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _apiResourceAppService.DeleteAsync(id));
            e.EntityType.ShouldBe(typeof(ApiResource));
        }

        [Fact]
        public async Task Should_Create_Update_Success()
        {
            var input = new ApiResourceCreateUpdateDto()
            {
                Name = "test",
                Scopes = new List<string> {"openid", "Age", "NewIdentityResource1"},
                UserClaims = new List<string> {"Age"}
            };
            (await _apiResourceAppService.CreateAsync(input)).ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Get_List_Of_ApiResources()
        {
            var result = await _apiResourceAppService.GetListAsync(new PagedAndSortedResultRequestDto());
            result.TotalCount.ShouldBe(3);
        }

        [Fact]
        public async Task Should_Delete_ApiResource_By_Id()
        {
            await _apiResourceAppService.DeleteAsync(_testData.ApiResource1Id);
            var list = await _apiResourceAppService.GetListAsync(new PagedAndSortedResultRequestDto());
            list.TotalCount.ShouldBe(2);
        }
    }
}
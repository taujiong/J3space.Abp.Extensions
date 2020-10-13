using System;
using System.Threading.Tasks;
using J3space.Abp.IdentityServer.Clients;
using J3space.Abp.IdentityServer.TestBase;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.IdentityServer.Clients;
using Xunit;

namespace J3space.Abp.IdentityServer.Application.Test
{
    public sealed class ClientAppServiceTests : AbpIdentityServerApplicationTestBase
    {
        private readonly IClientAppService _clientAppService;
        private readonly AbpIdentityServerTestData _testData;

        public ClientAppServiceTests()
        {
            _clientAppService = GetRequiredService<IClientAppService>();
            _testData = GetRequiredService<AbpIdentityServerTestData>();
        }

        [Fact]
        public async Task Should_Get_Client_By_Id()
        {
            var result = await _clientAppService.GetAsync(_testData.Client1Id);
            result.ShouldNotBeNull();
            result.ClientId.ShouldBe("ClientId1");
            result.AllowedCorsOrigins.ShouldContain("https://client1-origin.com");
        }

        [Fact]
        public async Task Should_Throw_Exception_With_Wrong_Id()
        {
            var id = Guid.NewGuid();

            var e = await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _clientAppService.GetAsync(id));
            e.EntityType.ShouldBe(typeof(Client));

            e = await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await _clientAppService.DeleteAsync(id));
            e.EntityType.ShouldBe(typeof(Client));
        }

        [Fact]
        public async Task Should_Create_Update_Success()
        {
            var input = new ClientCreateUpdateDto
            {
                ClientId = "test"
            };
            (await _clientAppService.CreateAsync(input)).ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Get_List_Of_Clients()
        {
            var result = await _clientAppService.GetListAsync(new PagedAndSortedResultRequestDto());
            result.TotalCount.ShouldBe(3);
        }

        [Fact]
        public async Task Should_Delete_ApiResource_By_Id()
        {
            await _clientAppService.DeleteAsync(_testData.Client1Id);
            var list = await _clientAppService.GetListAsync(new PagedAndSortedResultRequestDto());
            list.TotalCount.ShouldBe(2);
        }
    }
}
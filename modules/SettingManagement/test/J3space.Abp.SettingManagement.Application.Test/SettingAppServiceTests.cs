using J3space.Abp.SettingManagement.Setting;
using Shouldly;
using Xunit;

namespace J3space.Abp.SettingManagement.Application.Test
{
    public sealed class SettingAppServiceTests : AbpSettingManagementApplicationTestBase
    {
        private readonly ISettingAppService _appService;

        public SettingAppServiceTests()
        {
            _appService = GetRequiredService<ISettingAppService>();
        }

        [Fact]
        public void Should_Get_All_Info()
        {
            var result = _appService.GetList();
            result.Count.ShouldBe(3);
        }
    }
}
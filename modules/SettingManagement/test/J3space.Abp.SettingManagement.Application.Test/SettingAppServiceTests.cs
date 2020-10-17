using System.Collections.Generic;
using System.Linq;
using J3space.Abp.SettingManagement.Setting;
using Shouldly;
using Volo.Abp.SettingManagement;
using Xunit;

namespace J3space.Abp.SettingManagement.Application.Test
{
    public sealed class SettingAppServiceTests : AbpSettingManagementApplicationTestBase
    {
        private readonly ISettingAppService _appService;
        private readonly ISettingManager _manager;

        public SettingAppServiceTests()
        {
            _appService = GetRequiredService<ISettingAppService>();
            _manager = GetRequiredService<ISettingManager>();
        }

        [Fact]
        public void Should_Get_All_Info()
        {
            var result = _appService.GetList();
            result.Count.ShouldBe(5); // 框架自带 2 个 setting + 种子数据 3 个

            result.Where(x => x.Name != "SettingWithProperty")
                .ToList()
                .ForEach(x =>
                {
                    x.MainGroup.ShouldBe("Default");
                    x.SubGroup.ShouldBe("General");
                });

            result.Where(x => x.Name == "SettingWithProperty")
                .ToList()
                .ForEach(x =>
                {
                    x.MainGroup.ShouldBe("main");
                    x.SubGroup.ShouldBe("sub");
                });
        }

        [Fact]
        public async void Should_Change_And_Revert()
        {
            var sd = new Dictionary<string, string>
            {
                {"SettingWithProperty", "new-prop"},
                {"SettingWithDefaultValue", "new-default"},
                {"SettingWithoutDefaultValue", "new-no-default"}
            };

            await _appService.UpdateAsync(sd);

            (await _manager.GetOrNullForCurrentTenantAsync("SettingWithProperty"))
                .ShouldBe("new-prop");
            (await _manager.GetOrNullForCurrentTenantAsync("SettingWithDefaultValue"))
                .ShouldBe("new-default");
            (await _manager.GetOrNullForCurrentTenantAsync("SettingWithoutDefaultValue"))
                .ShouldBe("new-no-default");

            await _appService.DeleteAsync(new List<string>
            {
                "SettingWithProperty",
                "SettingWithDefaultValue",
                "SettingWithoutDefaultValue"
            });

            (await _manager.GetOrNullForCurrentTenantAsync("SettingWithProperty"))
                .ShouldBe("default_value_2");
            (await _manager.GetOrNullForCurrentTenantAsync("SettingWithDefaultValue"))
                .ShouldBe("default_value_1");
            (await _manager.GetOrNullForCurrentTenantAsync("SettingWithoutDefaultValue"))
                .ShouldBe("no-default"); // fallback 机制
            (await _manager.GetOrNullForCurrentTenantAsync("SettingWithoutDefaultValue", false))
                .ShouldBeNull();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using J3space.Abp.SettingManagement.Permissions;
using J3space.Abp.SettingManagement.Setting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace J3space.Abp.SettingManagement
{
    [Authorize(SettingManagementPermissions.Setting.Default)]
    public class SettingAppService : SettingManagementAppServiceBase, ISettingAppService
    {
        private readonly IStringLocalizerFactory _factory;
        private readonly ISettingDefinitionManager _manager;
        private readonly ISettingManager _settingManager;

        public SettingAppService(
            ISettingDefinitionManager manager,
            ISettingManager settingManager,
            IStringLocalizerFactory factory)
        {
            _manager = manager;
            _settingManager = settingManager;
            _factory = factory;
        }

        public List<SettingDefinitionDto> GetList()
        {
            return _manager.GetAll()
                .Select(x => new SettingDefinitionDto
                {
                    Name = x.Name,
                    DisplayName = x.DisplayName.Localize(_factory),
                    Description = x.Description.Localize(_factory),
                    DefaultValue = x.DefaultValue,
                    MainGroup = x.Properties.ContainsKey(SettingDefinitionPropertyConstants.MainGroup)
                        ? x.Properties[SettingDefinitionPropertyConstants.MainGroup] as string
                        : "Default",
                    SubGroup = x.Properties.ContainsKey(SettingDefinitionPropertyConstants.SubGroup)
                        ? x.Properties[SettingDefinitionPropertyConstants.SubGroup] as string
                        : "General"
                })
                .ToList();
        }

        [Authorize(SettingManagementPermissions.Setting.Manage)]
        public async Task<Dictionary<string, string>> UpdateAsync(Dictionary<string, string> settings)
        {
            foreach (var (key, value) in settings)
            {
                await _settingManager.SetForCurrentTenantAsync(key, value);
            }

            return settings;
        }

        [Authorize(SettingManagementPermissions.Setting.Manage)]
        public async Task DeleteAsync(List<string> settingNames)
        {
            foreach (var name in settingNames)
            {
                var setting = _manager.GetOrNull(name);
                if (setting == null)
                {
                    continue;
                }

                await _settingManager.SetForCurrentTenantAsync(name, setting.DefaultValue);
            }
        }
    }
}
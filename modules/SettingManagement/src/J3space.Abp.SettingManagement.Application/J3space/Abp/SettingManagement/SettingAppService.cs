using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;
using Volo.Abp.Settings;

namespace J3space.Abp.SettingManagement
{
    public class SettingAppService : SettingManagementAppServiceBase, ISettingAppService
    {
        private readonly IStringLocalizerFactory _factory;
        private readonly ISettingDefinitionManager _manager;

        public SettingAppService(
            ISettingDefinitionManager manager,
            IStringLocalizerFactory factory)
        {
            _manager = manager;
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
    }
}
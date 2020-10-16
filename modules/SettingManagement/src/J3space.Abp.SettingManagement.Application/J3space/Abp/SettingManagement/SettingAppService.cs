using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Settings;

namespace J3space.Abp.SettingManagement
{
    public class SettingAppService : SettingManagementAppServiceBase, ISettingAppService
    {
        private readonly ISettingDefinitionManager _manager;

        public SettingAppService(ISettingDefinitionManager manager)
        {
            _manager = manager;
        }

        public List<SettingDefinitionDto> GetList()
        {
            var settings = _manager.GetAll().ToList();
            return ObjectMapper.Map<List<SettingDefinition>, List<SettingDefinitionDto>>(settings);
        }
    }
}
using System.Collections.Generic;

namespace J3space.Abp.SettingManagement
{
    public interface ISettingAppService
    {
        public List<SettingDefinitionDto> GetList();
    }
}
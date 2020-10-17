using System.Collections.Generic;
using System.Threading.Tasks;

namespace J3space.Abp.SettingManagement.Setting
{
    public interface ISettingAppService
    {
        public List<SettingDefinitionDto> GetList();
        public Task<Dictionary<string, string>> UpdateAsync(Dictionary<string, string> settings);
        public Task DeleteAsync(List<string> settingNames);
    }
}
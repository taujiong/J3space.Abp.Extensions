using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;

namespace J3space.Abp.SettingManagement
{
    [RemoteService(Name = SettingManagementRemoteServiceConstants.RemoteServiceName)]
    [Area("Setting")]
    [Route("api/setting-management/settings")]
    public class SettingController : SettingManagementControllerBase, ISettingAppService
    {
        private readonly ISettingAppService _settingAppService;

        public SettingController(ISettingAppService settingAppService)
        {
            _settingAppService = settingAppService;
        }

        [HttpGet]
        public List<SettingDefinitionDto> GetList()
        {
            return _settingAppService.GetList();
        }

        [HttpPut]
        public Task<Dictionary<string, string>> UpdateAsync(Dictionary<string, string> settings)
        {
            return _settingAppService.UpdateAsync(settings);
        }

        [HttpDelete]
        public Task DeleteAsync(List<string> settingNames)
        {
            return _settingAppService.DeleteAsync(settingNames);
        }
    }
}
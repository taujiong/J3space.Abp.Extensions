using System.Collections.Generic;
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
    }
}
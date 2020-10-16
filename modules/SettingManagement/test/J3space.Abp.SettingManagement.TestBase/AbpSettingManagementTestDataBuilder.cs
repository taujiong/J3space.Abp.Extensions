using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;

namespace J3space.Abp.SettingManagement.TestBase
{
    public class AbpSettingManagementTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly ISettingRepository _settingRepository;

        public AbpSettingManagementTestDataBuilder(
            ISettingRepository settingRepository,
            IGuidGenerator guidGenerator)
        {
            _settingRepository = settingRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task BuildAsync()
        {
            await AddSettingsAsync();
        }

        private async Task AddSettingsAsync()
        {
            await _settingRepository.InsertAsync(
                new Setting(
                    _guidGenerator.Create(),
                    "SettingWithProperty",
                    "prop",
                    GlobalSettingValueProvider.ProviderName
                )
            );

            await _settingRepository.InsertAsync(
                new Setting(
                    _guidGenerator.Create(),
                    "SettingWithDefaultValue",
                    "default",
                    GlobalSettingValueProvider.ProviderName
                )
            );

            await _settingRepository.InsertAsync(
                new Setting(
                    _guidGenerator.Create(),
                    "SettingWithoutDefaultValue",
                    "no-default",
                    GlobalSettingValueProvider.ProviderName
                )
            );
        }
    }
}
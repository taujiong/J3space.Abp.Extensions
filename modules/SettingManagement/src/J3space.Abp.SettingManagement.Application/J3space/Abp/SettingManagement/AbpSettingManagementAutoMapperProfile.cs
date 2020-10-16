using AutoMapper;
using Volo.Abp.Settings;

namespace J3space.Abp.SettingManagement
{
    public class AbpSettingManagementAutoMapperProfile : Profile
    {
        public AbpSettingManagementAutoMapperProfile()
        {
            CreateMap<SettingDefinition, SettingDefinitionDto>()
                // .ForMember(des => des.DisplayName,
                //     opt => opt.MapFrom(src => src.DisplayName.Localize(factory).Value))
                // .ForMember(des => des.Description,
                //     opt => opt.MapFrom(src => src.Description.Localize(factory).Value))
                .ForMember(des => des.Directory,
                    opt => opt.MapFrom(src => src.Properties["Directory"] ?? "Others"))
                .ForMember(des => des.SubDirectory,
                    opt => opt.MapFrom(src => src.Properties["SubDirectory"] ?? "Others"));
        }
    }
}
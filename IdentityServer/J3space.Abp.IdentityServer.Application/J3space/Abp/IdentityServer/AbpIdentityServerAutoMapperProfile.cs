using AutoMapper;
using J3space.Abp.IdentityServer.ApiResources;
using J3space.Abp.IdentityServer.Clients;
using J3space.Abp.IdentityServer.IdentityResources;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;

namespace J3space.Abp.IdentityServer
{
    public class AbpIdentityServerAutoMapperProfile : Profile
    {
        public AbpIdentityServerAutoMapperProfile()
        {
            CreateMap<Client, ClientDto>();
            CreateMap<ApiScope, string>().ConstructUsing(src => src.Name);
            CreateMap<ApiResourceClaim, string>().ConstructUsing(src => src.Type);
            CreateMap<ApiResource, ApiResourceDto>();
            CreateMap<IdentityResource, IdentityResourceDto>();
        }
    }
}
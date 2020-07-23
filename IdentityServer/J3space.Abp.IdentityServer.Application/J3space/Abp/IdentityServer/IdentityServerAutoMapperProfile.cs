using AutoMapper;
using J3space.Abp.IdentityServer.ApiResources;
using J3space.Abp.IdentityServer.Clients;
using J3space.Abp.IdentityServer.IdentityResources;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;

namespace J3space.Abp.IdentityServer
{
    public class IdentityServerAutoMapperProfile : Profile
    {
        public IdentityServerAutoMapperProfile()
        {
            CreateMap<Client, ClientDto>();
            CreateMap<ApiScope, string>().ConstructUsing(src => src.Name);
            CreateMap<ApiResourceClaim, string>().ConstructUsing(src => src.Type);
            CreateMap<ApiResource, ApiResourceDto>();
            CreateMap<IdentityResource, IdentityResourceDto>();
        }
    }
}

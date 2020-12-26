using System.Linq;
using AutoMapper;
using IdentityServer4.Models;
using J3space.Abp.IdentityServer.ApiResources.Dto;
using J3space.Abp.IdentityServer.Clients.Dto;
using J3space.Abp.IdentityServer.IdentityResources.Dto;
using Volo.Abp.AutoMapper;
using Volo.Abp.IdentityServer.Clients;
using ApiResource = Volo.Abp.IdentityServer.ApiResources.ApiResource;
using Client = Volo.Abp.IdentityServer.Clients.Client;
using ClientClaim = Volo.Abp.IdentityServer.Clients.ClientClaim;
using IdentityResource = Volo.Abp.IdentityServer.IdentityResources.IdentityResource;

namespace J3space.Abp.IdentityServer
{
    public class AbpIdentityServerAutoMapperProfile : Profile
    {
        public AbpIdentityServerAutoMapperProfile()
        {
            #region Client

            CreateMap<Client, ClientDto>()
                .ForMember(des => des.IdentityProviderRestrictions,
                    opt => opt.MapFrom(src => src.IdentityProviderRestrictions.Select(x => x.Provider))
                )
                .ForMember(des => des.PostLogoutRedirectUris,
                    opt => opt.MapFrom(src => src.PostLogoutRedirectUris.Select(x => x.PostLogoutRedirectUri))
                )
                .ForMember(des => des.RedirectUris,
                    opt => opt.MapFrom(src => src.RedirectUris.Select(x => x.RedirectUri))
                )
                .ForMember(des => des.AllowedCorsOrigins,
                    opt => opt.MapFrom(src => src.AllowedCorsOrigins.Select(x => x.Origin))
                )
                .ForMember(des => des.AllowedGrantTypes,
                    opt => opt.MapFrom(src => src.AllowedGrantTypes.Select(x => x.GrantType))
                )
                .ForMember(des => des.AllowedScopes,
                    opt => opt.MapFrom(src => src.AllowedScopes.Select(x => x.Scope))
                );

            CreateMap<ClientSecret, ClientSecretDto>()
                .ReverseMap()
                .ForPath(des => des.Value,
                    opt => opt.MapFrom(src => HashExtensions.Sha256(src.Value)));
            CreateMap<ClientProperty, ClientPropertyDto>().ReverseMap();
            CreateMap<ClientClaim, ClientClaimDto>().ReverseMap();

            CreateMap<ClientUpdateDto, Client>()
                .Ignore(des => des.IdentityProviderRestrictions)
                .Ignore(des => des.PostLogoutRedirectUris)
                .Ignore(des => des.RedirectUris)
                .Ignore(des => des.AllowedCorsOrigins)
                .Ignore(des => des.AllowedGrantTypes)
                .Ignore(des => des.AllowedScopes)
                .Ignore(des => des.Id)
                .Ignore(des => des.Properties)
                .Ignore(des => des.ExtraProperties)
                .Ignore(des => des.ConcurrencyStamp)
                .Ignore(des => des.RequireRequestObject)
                .Ignore(des => des.AllowedIdentityTokenSigningAlgorithms)
                .IgnoreFullAuditedObjectProperties();

            #endregion

            #region Identity Resource

            CreateMap<IdentityResource, IdentityResourceDto>()
                .ForMember(des => des.UserClaims,
                    opt => opt.MapFrom(src => src.UserClaims.Select(x => x.Type)));

            CreateMap<IdentityResourceCreateUpdateDto, IdentityResource>()
                .Ignore(des => des.UserClaims)
                .Ignore(des => des.Id)
                .Ignore(des => des.Properties)
                .Ignore(des => des.ExtraProperties)
                .Ignore(des => des.ConcurrencyStamp)
                .IgnoreFullAuditedObjectProperties();

            #endregion

            #region Api Resource

            CreateMap<ApiResource, ApiResourceDto>()
                .ForMember(des => des.UserClaims,
                    opt => opt.MapFrom(src => src.UserClaims.Select(x => x.Type)))
                .ForMember(des => des.Scopes,
                    opt => opt.MapFrom(src => src.Scopes.Select(x => x.Scope)));

            CreateMap<ApiResourceCreateUpdateDto, ApiResource>()
                .Ignore(des => des.Secrets)
                .Ignore(des => des.UserClaims)
                .Ignore(des => des.Scopes)
                .Ignore(des => des.Id)
                .Ignore(des => des.Properties)
                .Ignore(des => des.ExtraProperties)
                .Ignore(des => des.ConcurrencyStamp)
                .Ignore(des => des.AllowedAccessTokenSigningAlgorithms)
                .IgnoreFullAuditedObjectProperties();

            #endregion
        }
    }
}
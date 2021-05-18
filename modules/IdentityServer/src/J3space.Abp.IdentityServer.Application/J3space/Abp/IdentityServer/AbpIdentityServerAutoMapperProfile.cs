using System.Linq;
using AutoMapper;
using J3space.Abp.IdentityServer.ApiResources.Dto;
using J3space.Abp.IdentityServer.ApiScopes.Dto;
using J3space.Abp.IdentityServer.Clients.Dto;
using J3space.Abp.IdentityServer.IdentityResources.Dto;
using Volo.Abp.AutoMapper;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.ApiScopes;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;

namespace J3space.Abp.IdentityServer
{
    public class AbpIdentityServerAutoMapperProfile : Profile
    {
        public AbpIdentityServerAutoMapperProfile()
        {
            #region Client

            CreateMap<Client, ClientDto>()
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

            CreateMap<ClientSecret, ClientSecretDto>();
            CreateMap<ClientProperty, ClientPropertyDto>();
            CreateMap<ClientClaim, ClientClaimDto>();

            CreateMap<ClientUpdateDto, Client>()
                .Ignore(des => des.ClientSecrets)
                .Ignore(des => des.Claims)
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
                .Ignore(des => des.ProtocolType)
                .Ignore(des => des.AlwaysIncludeUserClaimsInIdToken)
                .Ignore(des => des.AllowPlainTextPkce)
                .Ignore(des => des.IdentityTokenLifetime)
                .Ignore(des => des.AuthorizationCodeLifetime)
                .Ignore(des => des.AbsoluteRefreshTokenLifetime)
                .Ignore(des => des.SlidingRefreshTokenLifetime)
                .Ignore(des => des.RefreshTokenUsage)
                .Ignore(des => des.UpdateAccessTokenClaimsOnRefresh)
                .Ignore(des => des.RefreshTokenExpiration)
                .Ignore(des => des.EnableLocalLogin)
                .Ignore(des => des.IncludeJwtId)
                .Ignore(des => des.AlwaysSendClientClaims)
                .Ignore(des => des.ClientClaimsPrefix)
                .Ignore(des => des.PairWiseSubjectSalt)
                .Ignore(des => des.UserCodeType)
                .Ignore(des => des.DeviceCodeLifetime)
                .IgnoreFullAuditedObjectProperties();

            #endregion

            #region Identity Resource

            CreateMap<IdentityResource, IdentityResourceDto>()
                .ForMember(des => des.UserClaims,
                    opt => opt.MapFrom(src => src.UserClaims.Select(x => x.Type)));

            CreateMap<IdentityResourceProperty, IdentityResourcePropertyDto>();

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

            CreateMap<ApiResourceSecret, ApiResourceSecretDto>();
            CreateMap<ApiResourceProperty, ApiResourcePropertyDto>();

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

            #region Api Scope

            CreateMap<ApiScope, ApiScopeDto>()
                .ForMember(des => des.UserClaims,
                    opt => opt.MapFrom(src => src.UserClaims.Select(x => x.Type)));

            CreateMap<ApiScopeProperty, ApiScopePropertyDto>();

            CreateMap<ApiScopeCreateUpdateDto, ApiScope>()
                .Ignore(des => des.UserClaims)
                .Ignore(des => des.Id)
                .Ignore(des => des.Properties)
                .Ignore(des => des.ExtraProperties)
                .Ignore(des => des.ConcurrencyStamp)
                .IgnoreFullAuditedObjectProperties();

            #endregion
        }
    }
}
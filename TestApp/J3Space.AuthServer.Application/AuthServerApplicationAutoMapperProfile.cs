using AutoMapper;
using J3space.AuthServer.IdentityServer;
using Volo.Abp.IdentityServer.ApiResources;
using Volo.Abp.IdentityServer.Clients;
using Volo.Abp.IdentityServer.IdentityResources;

namespace J3space.AuthServer
{
    public class AuthServerApplicationAutoMapperProfile : Profile
    {
        public AuthServerApplicationAutoMapperProfile()
        {
            #region Identity Server

            // 查看源码后发现 abp 内部已经实现了诸如 ClientScope 类和 string 的映射配置
            // https://github.com/abpframework/abp/blob/dev/modules/identityserver/src/Volo.Abp.IdentityServer.Domain/Volo/Abp/IdentityServer/IdentityServerAutoMapperProfile.cs

            #endregion
        }
    }
}

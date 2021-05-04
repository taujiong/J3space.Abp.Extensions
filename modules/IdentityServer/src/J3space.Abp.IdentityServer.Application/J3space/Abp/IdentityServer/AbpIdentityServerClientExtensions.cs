using System;
using IdentityServer4;
using J3space.Abp.IdentityServer.Clients.Dto;
using Volo.Abp.IdentityServer.Clients;

namespace J3space.Abp.IdentityServer
{
    public static class AbpIdentityServerClientExtensions
    {
        public static void SetDefaultValues(this Client client, ClientType clientType)
        {
            // fix "code_challenge is missing" problem
            // TODO: How to support pkce?
            client.RequirePkce = false;

            switch (clientType)
            {
                case ClientType.Empty:
                    break;
                case ClientType.Device:
                    client.AddGrantType(IdentityServer4.Models.GrantType.DeviceFlow);
                    client.AddScope(IdentityServerConstants.StandardScopes.OpenId);
                    client.RequireClientSecret = false;
                    client.AllowOfflineAccess = true;
                    break;
                case ClientType.WebServer:
                    client.AddGrantType(IdentityServer4.Models.GrantType.AuthorizationCode);
                    client.AllowAccessTokensViaBrowser = false;
                    client.RequireClientSecret = true;
                    client.RequirePkce = true;
                    client.AddScope(IdentityServerConstants.StandardScopes.OpenId);
                    client.AddScope(IdentityServerConstants.StandardScopes.Profile);
                    ConfigureClientDefaultUrls(client);
                    break;
                case ClientType.Spa:
                    client.AddGrantType(IdentityServer4.Models.GrantType.AuthorizationCode);
                    client.RequireClientSecret = false;
                    client.RequirePkce = true;
                    client.AddScope(IdentityServerConstants.StandardScopes.OpenId);
                    client.AddScope(IdentityServerConstants.StandardScopes.Profile);
                    ConfigureClientDefaultUrls(client);
                    break;
                case ClientType.WebHybrid:
                    client.AddGrantType(IdentityServer4.Models.GrantType.Hybrid);
                    client.AddScope(IdentityServerConstants.StandardScopes.OpenId);
                    client.AddScope(IdentityServerConstants.StandardScopes.Profile);
                    ConfigureClientDefaultUrls(client);
                    break;
                case ClientType.Native:
                    client.AddGrantType(IdentityServer4.Models.GrantType.AuthorizationCode);
                    client.RequireClientSecret = false;
                    client.AddScope(IdentityServerConstants.StandardScopes.OpenId);
                    client.AddScope(IdentityServerConstants.StandardScopes.Profile);
                    break;
                case ClientType.Machine:
                    client.AddGrantType(IdentityServer4.Models.GrantType.ClientCredentials);
                    client.AddScope(IdentityServerConstants.StandardScopes.OpenId);
                    client.RequireConsent = false;
                    client.RequireClientSecret = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(clientType));
            }
        }

        private static void ConfigureClientDefaultUrls(Client myClient)
        {
            if (string.IsNullOrEmpty(myClient.ClientUri)) return;
            myClient.AddCorsOrigin(myClient.ClientUri);
        }
    }
}
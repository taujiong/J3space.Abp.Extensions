FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY applications/J3space.Auth/*.csproj applications/J3space.Auth/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Web/*.csproj modules/IdentityServer/src/J3space.Abp.IdentityServer.Web/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/*.csproj modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/
COPY modules/Account/J3space.Abp.Account.Web/*.csproj modules/Account/J3space.Abp.Account.Web/
WORKDIR applications/J3space.Auth
RUN dotnet restore

COPY common.props ./
COPY applications/J3space.Auth/ applications/J3space.Auth/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Web/ modules/IdentityServer/src/J3space.Abp.IdentityServer.Web/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/ modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/
COPY modules/Account/J3space.Abp.Account.Web/ modules/Account/J3space.Abp.Account.Web/
WORKDIR applications/J3space.Auth
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "J3space.Auth.dll"]
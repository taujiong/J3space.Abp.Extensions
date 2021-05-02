FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY applications/J3space.Admin/*.csproj applications/J3space.Admin/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Application/*.csproj modules/IdentityServer/src/J3space.Abp.IdentityServer.Application/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/*.csproj modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.HttpApi/*.csproj modules/IdentityServer/src/J3space.Abp.IdentityServer.HttpApi/
COPY modules/Account/J3space.Abp.Account.Web/*.csproj modules/Account/J3space.Abp.Account.Web/
WORKDIR applications/J3space.Admin
RUN dotnet restore

COPY common.props ./
COPY applications/J3space.Admin/ applications/J3space.Admin/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Application/ modules/IdentityServer/src/J3space.Abp.IdentityServer.Application/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/ modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.HttpApi/ modules/IdentityServer/src/J3space.Abp.IdentityServer.HttpApi/
WORKDIR applications/J3space.Admin
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "J3space.Admin.dll"]
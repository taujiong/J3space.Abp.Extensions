FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY demo/J3Auth/J3space.Auth.Web/*.csproj demo/J3Auth/J3space.Auth.Web/
COPY demo/J3Auth/J3space.Auth.EntityFrameworkCore.DbMigrations/*.csproj demo/J3Auth/J3space.Auth.EntityFrameworkCore.DbMigrations/
COPY demo/J3Auth/J3space.Auth.EntityFrameworkCore/*.csproj demo/J3Auth/J3space.Auth.EntityFrameworkCore/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Web/*.csproj modules/IdentityServer/src/J3space.Abp.IdentityServer.Web/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/*.csproj modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/
COPY modules/Account/J3space.Abp.Account.Web/*.csproj modules/Account/J3space.Abp.Account.Web/
WORKDIR demo/J3Auth/J3space.Auth.Web
RUN dotnet restore

COPY common.props ./
COPY demo/J3Auth/J3space.Auth.Web/ demo/J3Auth/J3space.Auth.Web/
COPY demo/J3Auth/J3space.Auth.EntityFrameworkCore.DbMigrations/ demo/J3Auth/J3space.Auth.EntityFrameworkCore.DbMigrations/
COPY demo/J3Auth/J3space.Auth.EntityFrameworkCore/ demo/J3Auth/J3space.Auth.EntityFrameworkCore/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Web/ modules/IdentityServer/src/J3space.Abp.IdentityServer.Web/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/ modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/
COPY modules/Account/J3space.Abp.Account.Web/ modules/Account/J3space.Abp.Account.Web/
WORKDIR demo/J3Auth/J3space.Auth.Web
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "J3space.Auth.Web.dll"]
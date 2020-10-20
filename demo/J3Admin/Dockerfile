FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

COPY demo/J3Admin/J3space.Admin.Application/*.csproj demo/J3Admin/J3space.Admin.Application/
COPY demo/J3Admin/J3space.Admin.Application.Contracts/*.csproj demo/J3Admin/J3space.Admin.Application.Contracts/
COPY demo/J3Admin/J3space.Admin.Domain/*.csproj demo/J3Admin/J3space.Admin.Domain/
COPY demo/J3Admin/J3space.Admin.Domain.Shared/*.csproj demo/J3Admin/J3space.Admin.Domain.Shared/
COPY demo/J3Admin/J3space.Admin.EntityFrameworkCore/*.csproj demo/J3Admin/J3space.Admin.EntityFrameworkCore/
COPY demo/J3Admin/J3space.Admin.EntityFrameworkCore.DbMigrations/*.csproj demo/J3Admin/J3space.Admin.EntityFrameworkCore.DbMigrations/
COPY demo/J3Admin/J3space.Admin.HttpApi/*.csproj demo/J3Admin/J3space.Admin.HttpApi/
COPY demo/J3Admin/J3space.Admin.HttpApi.Host/*.csproj demo/J3Admin/J3space.Admin.HttpApi.Host/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Application/*.csproj modules/IdentityServer/src/J3space.Abp.IdentityServer.Application/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/*.csproj modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.HttpApi/*.csproj modules/IdentityServer/src/J3space.Abp.IdentityServer.HttpApi/
COPY modules/SettingManagement/src/J3space.Abp.SettingManagement.Application/*.csproj modules/SettingManagement/src/J3space.Abp.SettingManagement.Application/
COPY modules/SettingManagement/src/J3space.Abp.SettingManagement.Application.Contracts/*.csproj modules/SettingManagement/src/J3space.Abp.SettingManagement.Application.Contracts/
COPY modules/SettingManagement/src/J3space.Abp.SettingManagement.HttpApi/*.csproj modules/SettingManagement/src/J3space.Abp.SettingManagement.HttpApi/
WORKDIR demo/J3Admin/J3space.Admin.HttpApi.Host
RUN dotnet restore

COPY common.props ./
COPY demo/J3Admin/J3space.Admin.Application/ demo/J3Admin/J3space.Admin.Application/
COPY demo/J3Admin/J3space.Admin.Application.Contracts/ demo/J3Admin/J3space.Admin.Application.Contracts/
COPY demo/J3Admin/J3space.Admin.Domain/ demo/J3Admin/J3space.Admin.Domain/
COPY demo/J3Admin/J3space.Admin.Domain.Shared/ demo/J3Admin/J3space.Admin.Domain.Shared/
COPY demo/J3Admin/J3space.Admin.EntityFrameworkCore/ demo/J3Admin/J3space.Admin.EntityFrameworkCore/
COPY demo/J3Admin/J3space.Admin.EntityFrameworkCore.DbMigrations/ demo/J3Admin/J3space.Admin.EntityFrameworkCore.DbMigrations/
COPY demo/J3Admin/J3space.Admin.HttpApi/ demo/J3Admin/J3space.Admin.HttpApi/
COPY demo/J3Admin/J3space.Admin.HttpApi.Host/ demo/J3Admin/J3space.Admin.HttpApi.Host/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Application/ modules/IdentityServer/src/J3space.Abp.IdentityServer.Application/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/ modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.HttpApi/ modules/IdentityServer/src/J3space.Abp.IdentityServer.HttpApi/
COPY modules/SettingManagement/src/J3space.Abp.SettingManagement.Application/ modules/SettingManagement/src/J3space.Abp.SettingManagement.Application/
COPY modules/SettingManagement/src/J3space.Abp.SettingManagement.Application.Contracts/ modules/SettingManagement/src/J3space.Abp.SettingManagement.Application.Contracts/
COPY modules/SettingManagement/src/J3space.Abp.SettingManagement.HttpApi/ modules/SettingManagement/src/J3space.Abp.SettingManagement.HttpApi/
WORKDIR demo/J3Admin/J3space.Admin.HttpApi.Host
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "J3space.Admin.HttpApi.Host.dll"]
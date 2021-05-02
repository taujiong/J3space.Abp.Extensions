FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY applications/J3space.Guard/*.csproj applications/J3space.Guard/
COPY modules/Blogging/J3space.Blogging.Application.Contracts/*.csproj modules/Blogging/J3space.Blogging.Application.Contracts/
COPY modules/Blogging/J3space.Blogging.Domain/*.csproj modules/Blogging/J3space.Blogging.Domain/
COPY modules/Blogging/J3space.Blogging.Domain.Shared/*.csproj modules/Blogging/J3space.Blogging.Domain.Shared/
COPY modules/Blogging/J3space.Blogging.EntityFrameworkCore/*.csproj modules/Blogging/J3space.Blogging.EntityFrameworkCore/
COPY modules/Blogging/J3space.Blogging.HttpApi/*.csproj modules/Blogging/J3space.Blogging.HttpApi/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/*.csproj modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.HttpApi/*.csproj modules/IdentityServer/src/J3space.Abp.IdentityServer.HttpApi/
COPY modules/SettingManagement/src/J3space.Abp.SettingManagement.Application/*.csproj modules/SettingManagement/src/J3space.Abp.SettingManagement.Application/
COPY modules/SettingManagement/src/J3space.Abp.SettingManagement.Application.Contracts/*.csproj modules/SettingManagement/src/J3space.Abp.SettingManagement.Application.Contracts/
COPY modules/SettingManagement/src/J3space.Abp.SettingManagement.HttpApi/*.csproj modules/SettingManagement/src/J3space.Abp.SettingManagement.HttpApi/
WORKDIR applications/J3space.Guard
RUN dotnet restore

COPY common.props ./
COPY applications/J3space.Guard/ applications/J3space.Guard/
COPY modules/Blogging/J3space.Blogging.Application.Contracts/ modules/Blogging/J3space.Blogging.Application.Contracts/
COPY modules/Blogging/J3space.Blogging.Domain/ modules/Blogging/J3space.Blogging.Domain/
COPY modules/Blogging/J3space.Blogging.Domain.Shared/ modules/Blogging/J3space.Blogging.Domain.Shared/
COPY modules/Blogging/J3space.Blogging.EntityFrameworkCore/ modules/Blogging/J3space.Blogging.EntityFrameworkCore/
COPY modules/Blogging/J3space.Blogging.HttpApi/ modules/Blogging/J3space.Blogging.HttpApi/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/ modules/IdentityServer/src/J3space.Abp.IdentityServer.Application.Contracts/
COPY modules/IdentityServer/src/J3space.Abp.IdentityServer.HttpApi/ modules/IdentityServer/src/J3space.Abp.IdentityServer.HttpApi/
COPY modules/SettingManagement/src/J3space.Abp.SettingManagement.Application/ modules/SettingManagement/src/J3space.Abp.SettingManagement.Application/
COPY modules/SettingManagement/src/J3space.Abp.SettingManagement.Application.Contracts/ modules/SettingManagement/src/J3space.Abp.SettingManagement.Application.Contracts/
COPY modules/SettingManagement/src/J3space.Abp.SettingManagement.HttpApi/ modules/SettingManagement/src/J3space.Abp.SettingManagement.HttpApi/
WORKDIR applications/J3space.Guard
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "J3space.Guard.dll"]
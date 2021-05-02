FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app

COPY applications/J3space.Blogging/*.csproj applications/J3space.Blogging/
COPY modules/Blogging/J3space.Blogging.Application/*.csproj modules/Blogging/J3space.Blogging.Application/
COPY modules/Blogging/J3space.Blogging.Application.Contracts/*.csproj modules/Blogging/J3space.Blogging.Application.Contracts/
COPY modules/Blogging/J3space.Blogging.Domain/*.csproj modules/Blogging/J3space.Blogging.Domain/
COPY modules/Blogging/J3space.Blogging.Domain.Shared/*.csproj modules/Blogging/J3space.Blogging.Domain.Shared/
COPY modules/Blogging/J3space.Blogging.EntityFrameworkCore/*.csproj modules/Blogging/J3space.Blogging.EntityFrameworkCore/
COPY modules/Blogging/J3space.Blogging.HttpApi/*.csproj modules/Blogging/J3space.Blogging.HttpApi/
WORKDIR applications/J3space.Blogging
RUN dotnet restore

COPY common.props ./
COPY applications/J3space.Blogging/ applications/J3space.Blogging/
COPY modules/Blogging/J3space.Blogging.Application/ modules/Blogging/J3space.Blogging.Application/
COPY modules/Blogging/J3space.Blogging.Application.Contracts/ modules/Blogging/J3space.Blogging.Application.Contracts/
COPY modules/Blogging/J3space.Blogging.Domain/ modules/Blogging/J3space.Blogging.Domain/
COPY modules/Blogging/J3space.Blogging.Domain.Shared/ modules/Blogging/J3space.Blogging.Domain.Shared/
COPY modules/Blogging/J3space.Blogging.EntityFrameworkCore/ modules/Blogging/J3space.Blogging.EntityFrameworkCore/
COPY modules/Blogging/J3space.Blogging.HttpApi/ modules/Blogging/J3space.Blogging.HttpApi/
WORKDIR applications/J3space.Blogging
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "J3space.Blogging.dll"]
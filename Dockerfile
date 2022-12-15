# https://hub.docker.com/_/microsoft-dotnet

FROM mcr.microsoft.com/dotnet/aspnet:7.0.1-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0.1-bullseye-slim AS build
WORKDIR /bld

COPY . .
RUN dotnet restore

WORKDIR "/bld/src/SoarBeyond.Web"
RUN dotnet build "SoarBeyond.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SoarBeyond.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SoarBeyond.Web.dll"]
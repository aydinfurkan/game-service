# Base ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
LABEL org.opencontainers.image.source="https://github.com/aydinfurkan/game-service"
EXPOSE 5000

# Build layer
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
COPY . .
WORKDIR /GameService.Application
RUN dotnet build GameService.Application.csproj -c Release -o /app

# Publish dll
FROM build AS publish
RUN dotnet publish GameService.Application.csproj -c Release -o /app 

# Entrypoint
FROM base AS final
COPY --from=publish /app .
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "GameService.Application.dll"]

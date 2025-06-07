#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GestaoDefeitos.WebApi/GestaoDefeitos.WebApi.csproj", "GestaoDefeitos.WebApi/"]
COPY ["GestaoDefeitos.Core/GestaoDefeitos.Domain.csproj", "GestaoDefeitos.Core/"]
COPY ["GestaoDefeitos.Infrastructure/GestaoDefeitos.Infrastructure.csproj", "GestaoDefeitos.Infrastructure/"]
COPY ["GestaoDefeitos.Application/GestaoDefeitos.Application.csproj", "GestaoDefeitos.Application/"]
RUN dotnet restore "./GestaoDefeitos.WebApi/GestaoDefeitos.WebApi.csproj"
COPY . .
WORKDIR "/src/GestaoDefeitos.WebApi"
RUN dotnet build "./GestaoDefeitos.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GestaoDefeitos.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GestaoDefeitos.WebApi.dll"]
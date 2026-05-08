# Consulte https://aka.ms/customizecontainer para aprender a personalizar su contenedor de depuración y cómo Visual Studio usa este Dockerfile para compilar sus imágenes para una depuración más rápida.

# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
#FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
RUN apk add --no-cache icu-libs
USER $APP_UID
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
#FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Novacollege.WebApi/Novacollege.WebApi.csproj", "Novacollege.WebApi/"]
COPY ["Novacollege.Data/Novacollege.Data.csproj", "Novacollege.Data/"]
COPY ["Novacollege.Business/Novacollege.Business.csproj", "Novacollege.Business/"]
RUN dotnet restore "./Novacollege.WebApi/Novacollege.WebApi.csproj"
COPY . .
WORKDIR "/src/Novacollege.WebApi"
RUN dotnet build "./Novacollege.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Novacollege.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Novacollege.WebApi.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

USER root
RUN apt-get update && apt-get install -y \
    libfontconfig1 \
    libfreetype6 \
    libpng16-16 \
    libjpeg-turbo8 \
    libglib2.0-0 \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["APIRelatorios.WebAPI/APIRelatorios.WebAPI.csproj", "APIRelatorios.WebAPI/"]
COPY ["APIRelatorios.IOC/APIRelatorios.IOC.csproj", "APIRelatorios.IOC/"]
COPY ["APIRelatorios.Application/APIRelatorios.Application.csproj", "APIRelatorios.Application/"]
COPY ["APIRelatorios.Dommain/APIRelatorios.Dommain.csproj", "APIRelatorios.Dommain/"]
COPY ["APIRelatorios.Infra/APIRelatorios.Infra.csproj", "APIRelatorios.Infra/"]
RUN dotnet restore "./APIRelatorios.WebAPI/APIRelatorios.WebAPI.csproj"
COPY . .
WORKDIR "/src/APIRelatorios.WebAPI"
RUN dotnet build "./APIRelatorios.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./APIRelatorios.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "APIRelatorios.WebAPI.dll"]
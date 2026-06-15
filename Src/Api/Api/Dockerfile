# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Src/Api/Api/Api.csproj", "Src/Api/Api/"]
COPY ["Src/BuildingBlocks/Shared.Jobs/Shared.Jobs.csproj", "Src/BuildingBlocks/Shared.Jobs/"]
COPY ["Src/BuildingBlocks/Shared/Shared.csproj", "Src/BuildingBlocks/Shared/"]
COPY ["Src/BuildingBlocks/Shared.Contract/Shared.Contract.csproj", "Src/BuildingBlocks/Shared.Contract/"]
COPY ["Src/BuildingBlocks/Shared.Web/Shared.Web.csproj", "Src/BuildingBlocks/Shared.Web/"]
COPY ["Src/BuildingBlocks/Shared.Mailing/Shared.Mailing.csproj", "Src/BuildingBlocks/Shared.Mailing/"]
COPY ["Src/BuildingBlocks/Shared.Message/Shared.Message.csproj", "Src/BuildingBlocks/Shared.Message/"]
COPY ["Src/Modules/Basket/Eshop.Module.Basket/Eshop.Module.Basket.csproj", "Src/Modules/Basket/Eshop.Module.Basket/"]
COPY ["Src/Modules/Catalog/CatalogContract/CatalogContract.csproj", "Src/Modules/Catalog/CatalogContract/"]
COPY ["Src/Modules/Core/EShop.Module.Core.Contract/EShop.Module.Core.Contract.csproj", "Src/Modules/Core/EShop.Module.Core.Contract/"]
COPY ["Src/Modules/Identity/Module.Identity.Contract/Module.Identity.Contract.csproj", "Src/Modules/Identity/Module.Identity.Contract/"]
COPY ["Src/Modules/Basket/Eshop.Module.Basket.Contract/Eshop.Module.Basket.Contract.csproj", "Src/Modules/Basket/Eshop.Module.Basket.Contract/"]
COPY ["Src/Modules/Catalog/Catalog/Catalog.csproj", "Src/Modules/Catalog/Catalog/"]
COPY ["Src/Modules/Localization/Eshop.Module.Localization.Contract/Eshop.Module.Localization.Contract.csproj", "Src/Modules/Localization/Eshop.Module.Localization.Contract/"]
COPY ["Src/Modules/Localization/Eshop.Module.Localization/Eshop.Module.Localization.csproj", "Src/Modules/Localization/Eshop.Module.Localization/"]
COPY ["Src/BuildingBlocks/Shared.Caching/Shared.Caching.csproj", "Src/BuildingBlocks/Shared.Caching/"]
COPY ["Src/Modules/Core/Eshop.Module.Core/Eshop.Module.Core.csproj", "Src/Modules/Core/Eshop.Module.Core/"]
COPY ["Src/BuildingBlocks/Shared.Storage/Shared.Storage.csproj", "Src/BuildingBlocks/Shared.Storage/"]
COPY ["Src/Modules/Identity/IdentityModule/IdentityModule.csproj", "Src/Modules/Identity/IdentityModule/"]
COPY ["Src/BuildingBlocks/Shared.Eventing/Shared.Eventing.csproj", "Src/BuildingBlocks/Shared.Eventing/"]
COPY ["Src/BuildingBlocks/Shared.Eventing.Contract/Shared.Eventing.Contract.csproj", "Src/BuildingBlocks/Shared.Eventing.Contract/"]
RUN dotnet restore "./Src/Api/Api/Api.csproj"
COPY . .
WORKDIR "/src/Src/Api/Api"
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
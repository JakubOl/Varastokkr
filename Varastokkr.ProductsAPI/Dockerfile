FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]

COPY ["Varastokkr.ProductsAPI/Varastokkr.ProductsAPI.csproj", "Varastokkr.ProductsAPI/"]
COPY ["Varastokkr.Shared/Varastokkr.Shared.csproj", "Varastokkr.Shared/"]

RUN dotnet restore "./Varastokkr.ProductsAPI/Varastokkr.ProductsAPI.csproj"
COPY . .

RUN dotnet build "Varastokkr.ProductsAPI/Varastokkr.ProductsAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Varastokkr.ProductsAPI/Varastokkr.ProductsAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Varastokkr.ProductsAPI.dll"]
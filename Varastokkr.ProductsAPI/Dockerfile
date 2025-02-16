FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]

COPY ["Varastokkr.ProductAPI/Varastokkr.ProductAPI.csproj", "Varastokkr.ProductAPI/"]
COPY ["Varastokkr.Shared/Varastokkr.Shared.csproj", "Varastokkr.Shared/"]

RUN dotnet restore "./Varastokkr.ProductAPI/Varastokkr.ProductAPI.csproj"
COPY . .

RUN dotnet build "Varastokkr.ProductAPI/Varastokkr.ProductAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Varastokkr.ProductAPI/Varastokkr.ProductAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Varastokkr.ProductAPI.dll"]
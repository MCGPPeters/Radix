#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Samples/Shop/Radix.Shop.Catalog/Radix.Shop.Catalog.csproj", "src/Samples/Shop/Radix.Shop.Catalog/"]
RUN dotnet restore "src/Samples/Shop/Radix.Shop.Catalog/Radix.Shop.Catalog.csproj"
COPY . .
WORKDIR "/src/src/Samples/Shop/Radix.Shop.Catalog"
RUN dotnet build "Radix.Shop.Catalog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Radix.Shop.Catalog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Radix.Shop.Catalog.dll"]
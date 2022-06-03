#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Samples/Shop/Radix.Shop/Radix.Shop.csproj", "src/Samples/Shop/Radix.Shop/"]
COPY ["src/Samples/Shop/Radix.Shop.Catalog/Radix.Shop.Catalog.csproj", "src/Samples/Shop/Radix.Shop.Catalog/"]
COPY ["src/Radix/Radix.csproj", "src/Radix/"]
COPY ["src/Radix.Generators/Radix.Generators.csproj", "src/Radix.Generators/"]
COPY ["src/Radix.Components.Generators/Radix.Components.Generators.csproj", "src/Radix.Components.Generators/"]
COPY ["src/Radix.Components/Radix.Components.csproj", "src/Radix.Components/"]
RUN dotnet restore "src/Samples/Shop/Radix.Shop/Radix.Shop.csproj"
COPY . .
WORKDIR "/src/src/Samples/Shop/Radix.Shop"
RUN dotnet build "Radix.Shop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Radix.Shop.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Radix.Shop.dll"]

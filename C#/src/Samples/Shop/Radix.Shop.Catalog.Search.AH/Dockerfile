#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcgppeters/netfunctions4-playwright AS base
WORKDIR /home/site/wwwroot
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./Radix.Shop.Catalog.Search.AH.csproj", "src/Samples/Shop/Radix.Shop.Catalog.Search.AH/"]
RUN dotnet restore "src/Samples/Shop/Radix.Shop.Catalog.Search.AH/Radix.Shop.Catalog.Search.AH.csproj"
COPY . .
WORKDIR "/src/src/Samples/Shop/Radix.Shop.Catalog.Search.AH"
RUN dotnet build "Radix.Shop.Catalog.Search.AH.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Radix.Shop.Catalog.Search.AH.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

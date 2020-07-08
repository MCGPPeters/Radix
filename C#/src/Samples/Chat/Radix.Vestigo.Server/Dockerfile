#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Samples/Chat/Radix.Vestigo.Server/Radix.Vestigo.Server.csproj", "src/Samples/Chat/Radix.Vestigo.Server/"]
COPY ["src/Samples/Chat/Radix.Vestigo.Shared/Radix.Vestigo.Shared.csproj", "src/Samples/Chat/Radix.Vestigo.Shared/"]
COPY ["src/Radix/Radix.csproj", "src/Radix/"]
COPY ["src/Radix.Components/Radix.Components.csproj", "src/Radix.Components/"]
COPY ["src/Samples/Chat/Radix.Vestigo.Client/Radix.Vestigo.Client.csproj", "src/Samples/Chat/Radix.Vestigo.Client/"]
RUN dotnet restore "src/Samples/Chat/Radix.Vestigo.Server/Radix.Vestigo.Server.csproj"
COPY . .
WORKDIR "/src/src/Samples/Chat/Radix.Vestigo.Server"
RUN dotnet build "Radix.Vestigo.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Radix.Vestigo.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Radix.Vestigo.Server.dll"]
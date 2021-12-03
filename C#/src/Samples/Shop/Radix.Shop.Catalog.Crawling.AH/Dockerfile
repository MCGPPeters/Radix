#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:4 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80
RUN apt-get update && apt-get install -y curl && \
    curl -sL https://deb.nodesource.com/setup_14.x | bash - && \
    apt-get install -y nodejs
RUN DEBIAN_FRONTEND=noninteractive npx playwright install && \
    DEBIAN_FRONTEND=noninteractive npx playwright install-deps

    # It's a good idea to use dumb-init to help prevent zombie chrome processes.
ADD https://github.com/Yelp/dumb-init/releases/download/v1.2.0/dumb-init_1.2.0_amd64 /usr/local/bin/dumb-init
RUN chmod +x /usr/local/bin/dumb-init

# tools to let browser run in headfull mode without having a screen
RUN apt-get update && apt-get install -y --no-install-recommends \
    xvfb

# generic host dependencies
RUN apt-get update && apt-get install -y --no-install-recommends \
    libgtk-3-0 \
    libgdk3.0 \
    libxcomposite-dev \
    libatk1.0-0

# Install Chromium dependencies
RUN apt-get update && apt-get install -y --no-install-recommends \
    libnss3 \
    libxss1 \
    libasound2 \
    fonts-noto-color-emoji \
    libxtst6

# Install Firefox dependencies
RUN apt-get update && apt-get install -y --no-install-recommends \
    libdbus-glib-1-2 \
    libxt6

# Install ffmpeg to bring in audio and video codecs necessary for playing videos in Firefox.
RUN apt-get update && apt-get install -y --no-install-recommends \
    ffmpeg

# (Optional) Install XVFB if there's a need to run browsers in headful mode
RUN apt-get update && apt-get install -y --no-install-recommends \
    xvfb

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
RUN wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb
RUN apt-get install -y apt-transport-https && \
    apt-get update && \
    apt-get install -y dotnet-sdk-5.0
# Install node14
RUN apt-get update && apt-get install -y curl && \
    curl -sL https://deb.nodesource.com/setup_14.x | bash - && \
    apt-get install -y nodejs

# Upgrade to NPM7 (see https://github.com/microsoft/playwright/pull/8915)
RUN npm install -g npm@7

# Feature-parity with node.js base images.
RUN apt-get update && apt-get install -y --no-install-recommends git ssh && \
    npm install -g yarn

RUN dotnet tool install --global Microsoft.Playwright.CLI
RUN npx playwright install --with-deps

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Samples/Shop/Radix.Shop.Catalog.Crawling.AH/Radix.Shop.Catalog.Crawling.AH.csproj", "src/Samples/Shop/Radix.Shop.Catalog.Crawling.AH/"]
COPY ["src/Radix/Radix.csproj", "src/Radix/"]
COPY ["src/Radix.Generators/Radix.Generators.csproj", "src/Radix.Generators/"]
COPY ["src/Samples/Shop/Radix.Shop.Catalog.Search/Radix.Shop.Catalog.Search.csproj", "src/Samples/Shop/Radix.Shop.Catalog.Search/"]
COPY ["src/Samples/Inventory/Radix.Inventory.Domain/Radix.Inventory.Domain.csproj", "src/Samples/Inventory/Radix.Inventory.Domain/"]
COPY ["src/Samples/Shop/Radix.Shop.Catalog.Domain/Radix.Shop.Catalog.Domain.csproj", "src/Samples/Shop/Radix.Shop.Catalog.Domain/"]

RUN dotnet restore "src/Samples/Shop/Radix.Shop.Catalog.Crawling.AH/Radix.Shop.Catalog.Crawling.AH.csproj"
COPY . .
WORKDIR "/src/src/Samples/Shop/Radix.Shop.Catalog.Crawling.AH"
RUN dotnet build "Radix.Shop.Catalog.Crawling.AH.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Radix.Shop.Catalog.Crawling.AH.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

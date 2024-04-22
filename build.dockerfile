FROM mcr.microsoft.com/dotnet/sdk:9.0

ARG GITHUB_TOKEN
ENV GITHUB_TOKEN=${GITHUB_TOKEN}

ARG NUGETORG_API_KEY
ENV NUGETORG_API_KEY=${NUGETORG_API_KEY}

RUN dotnet tool install -g nbgv
RUN dotnet nuget add source --name mcgppeters-github --username github \
    --password $GITHUB_TOKEN --store-password-in-clear-text \
    https://nuget.pkg.github.com/mcgppeters/index.json

# Install DotNet 6.0 for Azure function v4
COPY --from=mcr.microsoft.com/dotnet/sdk:6.0 /usr/share/dotnet/shared /usr/share/dotnet/shared

COPY . .

WORKDIR .

RUN dotnet restore
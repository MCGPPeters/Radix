$ErrorActionPreference = "Stop"

if ($args[0] -eq "local") {
	Write-Host "Building on local system..."
	dotnet run --project build -- $args[1..($args.Count)]
	exit 0;
}

Write-Host "Building in docker (use './build.ps1 local' to build without using docker)..."

$GitHubToken=$Env:PACKAGE_PUSH_TOKEN
$GitHubRunNumber=$Env:GITHUB_RUN_NUMBER
$NugetOrgApiKey=$Env:NUGETORG_API_KEY

if ($GitHubToken -eq $null -or $GitHubToken -eq "") {
	Write-Warning "PACKAGE_PUSH_TOKEN environment variable empty or missing."
}

if ($GitHubRunNumber -eq $null -or $GitHubRunNumber -eq "") {
	Write-Warning "GITHUB_RUN_NUMBER environment variable empty or missing."
}

if ($NugetOrgApiKey -eq $null -or $NugetOrgApiKey -eq "") {
	Write-Warning "NUGETORG_API_KEY environment variable empty or missing."
}

$tag="radix-build"

# Build the build environment image.
docker build `
 --build-arg GITHUB_TOKEN=$GitHubToken `
 --build-arg NUGETORG_API_KEY=$NugetOrgApiKey `
 -f build.dockerfile `
 --tag $tag.

# Build inside build environment
docker run --rm --name $tag `
 -v /var/run/docker.sock:/var/run/docker.sock `
 -v $PWD/artifacts:/artifacts `
 -v $PWD/.git:/.git `
 -v $PWD/temp:/temp `
 -e NUGET_PACKAGES=/temp/nuget-packages `
 -e BUILD_NUMBER=$GitHubRunNumber `
 --network host `
 $tag `
 dotnet run --project ./src/Build/Build.csproj -c Release -- $args

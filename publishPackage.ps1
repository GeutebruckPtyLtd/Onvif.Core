$projectBase = "./"
$projectFile = "Onvif.Core.sln"

$GITHUB_PAT = $env:github_nuget_PAT

if (-not $GITHUB_PAT) {
    Write-Host "Error: GitHub PAT is not set in the environment variables." -ForegroundColor Red
    exit 1
}

# Building Projects
Write-Host "Building the project..."
dotnet build $projectFile --configuration Release

# Build the NuGet package
Write-Host "Building the NuGet package..."
dotnet pack $projectFile --configuration Release

# Push the package to GitHub Packages
$nugetSource = "https://nuget.pkg.github.com/GeutebruckPtyLtd/index.json"


# Get the most recent .nupkg file in the bin/Release directory
$packageDir = $projectBase + "/bin/Release"
$package = Get-ChildItem -Path $projectBase -Filter "*.nupkg" -Recurse |
    Sort-Object LastWriteTime -Descending |
    Select-Object -First 1
#$package = Get-ChildItem -Path $projectBase -Filter "*.nupkg" | Sort-Object LastWriteTime -Descending | Select-Object -First 1

if (-not $package) {
    Write-Host "Error: No .nupkg file found in bin/Release." -ForegroundColor Red
    exit 1
}

# Get full path of the latest package
$packagePath = $package.FullName
Write-Host "Publishing the NuGet Package" + $packagePath

dotnet nuget push $packagePath --skip-duplicate --source $nugetSource --api-key $GITHUB_PAT
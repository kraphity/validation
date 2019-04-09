param (
	[string]$configuration = "Release",
	[string]$output = $env:BUILD_ARTIFACTSTAGINGDIRECTORY,
	[string]$preReleaseLabel = $env:BUILD_SOURCEBRANCHNAME,
	[string]$buildName = $env:BUILD_BUILDNUMBER,
	[switch]$final
)

function has-error() {
	return $LASTEXITCODE -ne 0
}

$scriptPath = Split-Path $script:MyInvocation.MyCommand.Path

if([string]::IsNullOrEmpty($output)) {
	$output = "$scriptPath/Artifacts"
}

if([string]::IsNullOrEmpty($preReleaseLabel)) {
	$preReleaseLabel = "local"
}

$buildNumber = 0

if($buildName -match "^.*-(?<rev>\d+)$") {
	$buildNumber = $matches["rev"]
}

if(!$final) {
	$versionSuffix = "{0}{1}" -f $preReleaseLabel, $buildNumber
}

$dotnetBuildParams = @('--configuration', $configuration)

if($versionSuffix){
	$dotnetBuildParams += @('--version-suffix', $versionSuffix)
}

write-host "BuildName: $buildName"
write-host "BuildNumber: $buildNumber"
write-host "BuildParams: $dotnetBuildParams"

dotnet build $dotnetBuildParams

if(has-error) {
	exit 1
}

$testFailure = $false

get-childitem $scriptPath/src/**/*.*Tests.csproj | foreach-object {
	dotnet test $_.FullName --configuration $configuration --no-build --no-restore --logger:trx --results-directory $output/testresults

	if(has-error) { 
		$testFailure = $true
	}
}

if($testFailure)
{
	exit 1
}

get-childitem $scriptPath/src/**/*.csproj -Exclude *.UnitTests.csproj | foreach-object {
	dotnet pack $_.FullName --no-build --no-restore --output $output/nupkgs $dotnetBuildParams

	if(has-error) {
		exit 1
	}
}
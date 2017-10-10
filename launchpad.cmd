@ECHO OFF

pushd %~dp0

IF NOT EXIST bin\Launchpad.dll (
	dotnet publish -c Release -o ..\..\bin\ src\Launchpad\Launchpad.csproj || EXIT /B 1
)

dotnet bin\Launchpad.dll %*

popd

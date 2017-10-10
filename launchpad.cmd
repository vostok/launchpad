@ECHO OFF

IF NOT EXIST %~dp0bin\Launchpad.dll (
	dotnet publish -c Release -o ..\..\bin\ %~dp0src\Launchpad\Launchpad.csproj || EXIT /B 1
)

dotnet %~dp0bin\Launchpad.dll %*

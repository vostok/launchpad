@ECHO OFF

IF NOT EXIST %~dp0bin\Launchpad.dll (
	CALL dotnet publish -c Release -o ..\..\bin\ src\Launchpad\Launchpad.csproj  || EXIT /B 1
)

%~dp0dotnet bin\Launchpad.dll %*

@ECHO OFF

IF NOT EXIST %~dp0src\Launchpad\bin\Release\net461\Launchpad.exe (
	CALL powershell .\build.ps1 || EXIT /B 1
)

%~dp0src\Launchpad\bin\Release\net461\Launchpad.exe %*

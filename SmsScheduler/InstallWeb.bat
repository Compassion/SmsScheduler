powershell.exe Set-ExecutionPolicy remotesigned
powershell.exe . '%~dp0Installer.ps1'; SetupInfrastructure
powershell.exe Set-ExecutionPolicy remotesigned
powershell.exe . '%~dp0Installer.ps1'; InstallWeb
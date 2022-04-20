@echo off
IF defined DOTNET_HOME (
	%DOTNET_HOME%\installutil.exe /U %~dp0\COAD.SCHEDULER.EXE
	
) ELSE (
	
		ECHO Erro. Defina a variável de ambiente de nome 'DOTNET_HOME' contendo a pasta de instalação do .NET 
		ECHO Ex: C:\Windows\Microsoft.NET\Framework64\v4.0.30319
	
)
pause


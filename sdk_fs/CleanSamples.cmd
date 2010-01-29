%windir%\Microsoft.NET\Framework\v3.5\MSBuild /target:Clean "MogreSamples.sln"
@IF %ERRORLEVEL% NEQ 0 GOTO err
@exit /B 0
:err
@PAUSE
@exit /B 1
%windir%\Microsoft.NET\Framework\v3.5\MSBuild /property:Configuration=Release /target:Clean "MogreSamples.sln"
@IF %ERRORLEVEL% NEQ 0 GOTO err
%windir%\Microsoft.NET\Framework\v3.5\MSBuild /property:Configuration=Debug /target:Clean "MogreSamples.sln"
@IF %ERRORLEVEL% NEQ 0 GOTO err
@exit /B 0
:err
@PAUSE
@exit /B 1
@echo off

set RMPATH=%RMPATH%;.\copies

if "%1"==""   goto help

rmcobol %$ g=compiler.cfg Y=3 l
goto end

:help
  echo Uso: compilar programa [opciones]
  goto end

:end


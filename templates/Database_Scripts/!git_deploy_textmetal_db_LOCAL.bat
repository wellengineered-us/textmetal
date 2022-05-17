@echo off

REM
REM	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
REM	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
REM

set SHOULD_CREATE_DB=true
set SQLCMD_EXE=C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\130\Tools\Binn\sqlcmd.exe
set SQL_DIR=.

set DB_SERVER=(local)
set DB_DATABASE_MASTER=master
set DB_SA_USERNAME=sa
set DB_SA_PASSWORD={Pass@word1}

set DB_DATABASE=textmetal_sample
set DB_LOGIN=textmetal_sample_mssql_lcl_login
set DB_PASSWORD=LrJGmP6UfW8TEp7x3wWhECUYULE6zzMcWQ03R6UxeB4xzVmnq5S4Lx0vApegZVH
set DB_USER=textmetal_sample_mssql_lcl_user


CALL deploy_textmetal_db.bat
IF %ERRORLEVEL% NEQ 0 goto pkgError


goto pkgSuccess


:pkgError
echo An error occured.
pause > nul
goto :eof

:pkgSuccess
echo Completed successfully.
pause > nul
goto :eof

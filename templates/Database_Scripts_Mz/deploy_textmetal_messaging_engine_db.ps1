#
#	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
#	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
#

cls
$root = [System.Environment]::CurrentDirectory

$should_create_db = $Env:SHOULD_CREATE_DB
$sqlcmd_exe = $Env:SQLCMD_EXE
$sql_dir = $Env:SQL_DIR

$db_server = $Env:DB_SERVER
$db_sa_username = $Env:DB_SA_USERNAME
$db_sa_password = $Env:DB_SA_PASSWORD
$db_database_master = $Env:DB_DATABASE_MASTER
$db_database = $Env:DB_DATABASE
$db_login = $Env:DB_LOGIN
$db_password = $Env:DB_PASSWORD
$db_user = $Env:DB_USER

echo "The operation is starting..."

if ($should_create_db -eq $true)
{
	&"$sqlcmd_exe" `
		-S "$db_server" `
		-U "$db_sa_username" `
		-P "$db_sa_password" `
		-d "$db_database_master" `
		-v VAR_DB_DATABASE="$db_database" `
		-v VAR_DB_LOGIN="$db_login" `
		-v VAR_DB_PASSWORD="$db_password" `
		-v VAR_DB_USER="$db_user" `
		-i "$sql_dir\deploy_textmetal_messaging_engine_db_0000.sql"

	if (!($LastExitCode -eq $null -or $LastExitCode -eq 0))
	{ echo "An error occurred during the operation."; return; }
}

if (!($LastExitCode -eq $null -or $LastExitCode -eq 0))
{ echo "An error occurred during the operation."; return; }

&"$sqlcmd_exe" `
	-S "$db_server" `
	-U "$db_sa_username" `
	-P "$db_sa_password" `
	-d "$db_database" `
	-i "$sql_dir\deploy_textmetal_messaging_engine_db_0001.sql"

if (!($LastExitCode -eq $null -or $LastExitCode -eq 0))
{ echo "An error occurred during the operation."; return; }

echo "The operation completed successfully."
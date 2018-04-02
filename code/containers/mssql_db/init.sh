#!/bin/bash
##
## init.sh - Container initalization and startup script
##
SLEEP_TIME=5
DB_INIT_SQL_FILE='schema.sql'

## start MSSQL in the background
echo "[INFO] init.sh: Starting SQL Server in the background..."
/opt/mssql/bin/sqlservr &
MSSQL_PID=$!

## wait for the database to come up
SQL_OUTPUT=`/opt/mssql-tools/bin/sqlcmd -l 5 -S localhost -U sa -P $SA_PASSWORD -d master -Q "Select @@VERSION"`
while [ $? -ne 0 ]; do
    echo "[INFO] init.sh: SQL Server still starting. Retry in ${SLEEP_TIME} seconds..."
    sleep $SLEEP_TIME
    SQL_OUTPUT=`/opt/mssql-tools/bin/sqlcmd -l 5 -S localhost -U sa -P $SA_PASSWORD -d master -Q "Select @@VERSION"`
done
echo "[INFO] init.sh: SQL Server is UP!"
echo $SQL_OUTPUT

## import the initial schema
echo "[INFO] init.sh: Initalizing database..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -e -i $DB_INIT_SQL_FILE

## if initial schema load was successful, wait on MSSQL server process (forever) if not, die.
if [ $? -eq 0 ]; then
    echo "[INFO] init.sh: Database initalized sucessfully!"
    echo "[INFO] init.sh: SQL Server is up and database is ready for connections."
    ## wait on mssql
    wait $MSSQL_PID
else
    echo "[ERROR] init.sh: Database initalization failed..."
    exit 1
fi
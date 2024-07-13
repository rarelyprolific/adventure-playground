echo 'Deferring database setup for 20 seconds to allow SQL Server to start up!'
sleep 20s

echo 'Starting database setup (after waiting 20 seconds for SQL Server to start up!)'

/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $MSSQL_SA_PASSWORD -i create-database.sql

echo 'Finished database setup!'

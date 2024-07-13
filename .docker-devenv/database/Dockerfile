FROM mcr.microsoft.com/mssql/server:2022-latest

WORKDIR /src

COPY create-database.sql ./
COPY setup-database.sh ./

CMD bash ./setup-database.sh & /opt/mssql/bin/sqlservr

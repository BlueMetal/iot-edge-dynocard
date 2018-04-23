## https://hub.docker.com/r/microsoft/mssql-server-linux/
FROM microsoft/mssql-server-linux:2017-latest

## accept the SQL Server EULA
ENV ACCEPT_EULA=true

ENV SA_PASSWORD='Strong!Passw0rd'

## MSSQL Product ID (Developer, Express, Standard, EnterpriseCore, Enterprise, etc...)
ENV MSSQL_PID='Developer'

## create app directory
RUN mkdir -p /opt/app/mssql_db
WORKDIR /opt/app/mssql_db

## copy files into container
COPY . /opt/app/mssql_db

## set permissions
RUN chmod +x /opt/app/mssql_db/init.sh

## set default container command
CMD /bin/bash /opt/app/mssql_db/init.sh
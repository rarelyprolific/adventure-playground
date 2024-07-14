# adventure-playground

Just a sandpit repository for playing around with github and docker!

## Figure out the following!

- Is it possible to selectively enable/disable services in compose.yml via parameters (or another method)? You may not want to run every service all the time.

- Figure out how to run and use devcontainers! (https://code.visualstudio.com/docs/devcontainers/containers)

- Figure out how to do SSL inside docker compose!

## Docker Compose Example

The `.docker-dev-env/compose.yml` builds the following containers:

- **simplewebapi** - Basic web API which connects to a database.
- **sqlserver** - Custom SQL Server container which builds a database upon initialisation using a `create-database.sql` script.

All containers are added to a `devenv-network` network. If the `SimpleWebApi.csproj` is run in Visual Studio via Docker it is added to the same network based on the following entry in the project file:

```xml
<DockerfileRunArguments>--network devenv-network</DockerfileRunArguments>
```

This allows you to have some services running directly in docker via docker compose and then selectively debug other services in the same network using Visual Studio.

The `.docker-devenv/compose.yml` file expects the SQL SA password to be set in a `.docker-devenv/.env` file using the key `SQL_SERVER_SA_PASSWORD`.

If debugging SimpleWebApi in Visual Studio, you'll need to add the database connection string as an environment variables in `launchSettings.json` as so:

```
DATABASE_CONNECTION_STRING: "Data Source=sqlserver;Initial Catalog=SimpleDatabase;User id=SA;Password=[REDACTED!];TrustServerCertificate=True;"
```

## Docker Tips and Troubleshooting

If you generate a multistage Dockerfile automatically with Visual Studio the paths will be set up to expect the docker build to be run based on the same location as the solution file. If you run the docker build yourself, you'll need to point it to the Dockerfile in the project folder as below:

```
docker build -f SimpleWebApi/Dockerfile -t simplewebapi .
```

It looks like this in a docker compose YML file:

```yml
services:
  simplewebapi:
    build:
      context: "../src/SimpleWebApi"
      dockerfile: "SimpleWebApi/Dockerfile"
    ports:
      - "8080:8080"
```

Exposing the ports or an ASP.NET web application for any IP instead of just localhost seems to be done automatically now. But if you have problems see: https://andrewlock.net/why-isnt-my-aspnetcore-app-in-docker-working/

By default, the Swagger UI will not be active in an ASP.NET Core application unless the `ASPNETCORE_ENVIRONMENT=Development` environment variable is set.

Executing a command inside a SQL Server container:

```
cd /opt/mssql-tools/bin

./sqlcmd -U sa -P [REDACTED!] -q "select name from sys.databases"
```

If you are having problems adding a container to a network after changing the compose file because the old network still exists, you can use `docker-compose up --force-recreate`. Alternatively, if you run `docker compose down` it should remove the previous network if no containers are using it.

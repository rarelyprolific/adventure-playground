# adventure-playground

Just a sandpit repository for playing around with github and docker! This is a work in progress.

## Figure out the following!

- Figure out how to run and use devcontainers! (https://code.visualstudio.com/docs/devcontainers/containers)

## Hosting Ports and HTTPS/SSL

ASP NET Core websites automatically host over port 8080 but you can explicitly set the hosting ports via these environment variables:

```
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_HTTPS_PORTS=8081
```

To host over HTTPS/SSL during development you need to set up a self-signed certificate using `dotnet devcerts`. See https://github.com/dotnet/dotnet-docker/blob/main/samples/run-aspnetcore-https-development.md for details.

## Selectively running containers

Run all containers (default): `docker compose up`

Run a single container: `docker compose up simplewebapi`

Run multiple containers: `docker compose up simplewebapi sqlserver`

## Known Issues

- The `create-database.sql` script needs to be idempotent! It isn't currently so you'll keep getting additional rows inserted if you re-run `docker compose up`.

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

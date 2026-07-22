# Dievas 
Dievas - A Dashboarding server written in [.NET](https://docs.microsoft.com/en-us/dotnet/csharp/).


## Requirements
- backend
	- .NET 7 or later with command line tools. You can use `$ dotnet --version` to check the installed version.
	- [Newtonsoft.json](https://www.newtonsoft.com/json) (included in the project)


## Installation
Clone this repository and initialize submodules:

```bash
git clone --recurse-submodules https://github.com/porcej/Dievas.git
cd Dievas
```

If you already cloned without submodules:

```bash
git submodule update --init --recursive
```

## Configuration

Dievas uses layered configuration. **Never commit environment-specific values or secrets.**

| Source | Committed | Purpose |
| --- | --- | --- |
| `appsettings.json` | Yes | Base defaults and configuration schema |
| `appsettings.Development.json.example` | Yes | Template for local development |
| `appsettings.Production.json.example` | Yes | Template for non-Docker production |
| `appsettings.Development.json` | No (gitignored) | Your local development overrides |
| `appsettings.Production.json` | No (gitignored) | Your production overrides (IIS, bare metal) |
| `.env.example` | Yes | Template for Docker environment variables |
| `.env` | No (gitignored) | Your Docker deployment values |

### Local setup

Copy the development template and fill in your endpoints and credentials:

```bash
cp appsettings.Development.json.example appsettings.Development.json
```

For non-Docker production deployments:

```bash
cp appsettings.Production.json.example appsettings.Production.json
```

Alternatively, use [ASP.NET Core User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) for local development (the project already has a `UserSecretsId`).

## Usage
The server can be run from [.NET CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/), Docker, or can be packaged in an IIS instance.

### Docker

Prerequisites: [Docker](https://docs.docker.com/get-docker/) with Docker Compose v2.

Docker uses environment variables from `.env` rather than `appsettings.*.json`. No appsettings copy is required.

1. Copy the example environment file and adjust values as needed:

   ```bash
   cp .env.example .env
   ```

2. Build and start the application:

   ```bash
   docker compose up --build
   ```

3. Verify the API is reachable at [http://localhost:8080/api/dashboard/incidents](http://localhost:8080/api/dashboard/incidents) (or the host port set in `.env`).

#### Ports

| Setting | Default | Description |
| --- | --- | --- |
| `DIEVAS_HOST_PORT` | `8080` | Host port mapped to container port `8080` |

The container listens on HTTP port `8080` internally. TLS termination should be handled by a reverse proxy in production deployments.

#### Environment variables

Configuration uses ASP.NET Core's environment variable convention (`Section__Key`). Copy `.env.example` to `.env` and set values for your deployment.

| Variable | Required | Description |
| --- | --- | --- |
| `ASPNETCORE_ENVIRONMENT` | No | `Production` (default) or `Development` |
| `UseHttpsRedirection` | No | Set to `false` for HTTP-only container deployments (default in Compose) |
| `Origins` | Yes | Semicolon-separated CORS origins (e.g. `http://localhost:8080`) |
| `AppName` | No | Application name shown in Swagger |
| `Version` | No | Application version |
| `NWS__Url` | Yes | National Weather Service API base URL |
| `NWS__ForecastEndpoint` | Yes | NWS forecast endpoint path |
| `NWS__User-agent` | Yes | User-agent sent to NWS |
| `NWS__UpdateIntervalMinutes` | No | Weather refresh interval in minutes |
| `Telestaff__Url` | Yes | Telestaff API base URL |
| `Telestaff__User-agent` | Yes | User-agent sent to Telestaff |
| `Telestaff__TimeFormat` | No | Date format for Telestaff requests |
| `Telestaff__UpdateIntervalMinutes` | No | Telestaff refresh interval in minutes |
| `Telestaff__ExpirationTimeInMinutes` | No | Cached roster expiration in minutes |
| `Telestaff__Username` | No | Telestaff basic auth username |
| `Telestaff__Password` | No | Telestaff basic auth password |
| `Telestaff__AllowInvalidCertificates` | No | `true` to skip TLS certificate validation |
| `ConnectionStrings__CAD_Reporting` | No | SQL Server connection string (optional) |

#### Volume mounts

Dievas is stateless and does not require volume mounts for normal operation. Optional bind mounts:

| Host path | Container path | Purpose |
| --- | --- | --- |
| `./appsettings.json` | `/app/appsettings.json` | Override base configuration (read-only recommended) |

### Stand alone [.NET CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/)

Ensure `appsettings.Development.json` exists (see [Configuration](#configuration)):

```bash
cp appsettings.Development.json.example appsettings.Development.json
dotnet run
```

Swagger is available in Development at [https://localhost:5001/swagger](https://localhost:5001/swagger).

### IIS Instance
```
TODO
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## License
[MIT](https://choosealicense.com/licenses/mit/)

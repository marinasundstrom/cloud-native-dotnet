# BlazorApp1

Exploration of Cloud-native, with telemetry and distributed tracing - using OpenTelemetry, Prometheus, Loki, and Grafana.

Based on ChatApp.

## Project
The app consists of a Frontend built with Blazor WebAssembly, and a Backend with ASP.NET Core.

Major technical characteristics of the project are listed below.

###  Architecture
* Clean Architecture (CA) in app project, with Vertical-slices architecture (VSA)
  * Combining all layers into one project with focus on features.
  * Using Domain-driven design (DDD) practices
* Event-driven architecture - Domain Events
  
### Technologies
* ASP.NET Core
  * Endpoints - "Minimal API" with versioning
  * SignalR
  * OpenAPI

* Frontend/UI
  * Blazor
  * MudBlazor (component framework)

* Azure SQL Server
* IdentityServer for authentication - with seeded users Alice and Bob.

Unused but available technologies:
* RabbitMQ (for asynchronous messaging)
  * MassTransit 
* Redis (for distributed cache)

Other:
* Open Telemetry - with Zipkin
* Health checks

### Tests
* Application logic tests
* Domain model test
* Integration tests - with Test host and Testcontainers

## Running the project with Docker Compose

```sh
docker compose up
```

```sh
docker compose up -d
```

### Seeding the databases

In order for the databases to be created and for the app to function, you need to seed the databases for the ```Web``` and ```IdentityService``` projects.

The services, in particular the databases, have to be running for this to work. 

The seeding code target databases that have been defined in the ```appsettings.json``` files in each project.

#### Web

When in the Web project:

```sh
dotnet run -- --seed
```

#### IdentityService

When in the IdentityService project:

```sh
dotnet run -- /seed
```

### Services

These are the services:

* Frontend: https://localhost:8081/
* Backend: https://localhost:5001/
  * Swagger UI: https://localhost:5001/swagger/

### Login credentials

Here are the users available to login as, provided that you have seeded the database.

```
Username: alice 
Password: alice

Username: bob 
Password: bob
```

### Swagger UI

Hosted at: https://localhost:5001/swagger/

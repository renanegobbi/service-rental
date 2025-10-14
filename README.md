# 🏍️ Overview
This project is an implementation developed using .NET 8, PostgreSQL, RabbitMQ (with EasyNetQ), and the CQRS architecture.
The main goal is to simulate a motorcycle rental management system, handling the registration of couriers and motorcycles, order creation, and event-driven communication between services.

### ⚙️ Tech Stack

| Category | Technology |
|-----------|-------------|
| 🧱 **Framework** | .NET 8 (ASP.NET Core Web API) |
| 🐘 **Database** | PostgreSQL (`rental_service` schema) |
| 📨 **Messaging** | RabbitMQ with EasyNetQ |
| 🧩 **Pattern** | CQRS + DDD |
| 🐳 **Containerization** | Docker & Docker Compose |
| 🗄️ **ORM** | Entity Framework Core |
| 🧪 **Testing** | xUnit / FluentAssertions (optional) |


# 🧱 Architecture

The solution follows a Domain-Driven Design (DDD) and CQRS (Command Query Responsibility Segregation) approach, with a clear separation between application concerns, domain logic, and infrastructure.
Each project in the solution has a specific responsibility and communicates through events and the message bus.

```
🗂️ Solution Structure
src/
.
├── .git/                   
├── .github/                → GitHub Actions workflows and CI/CD configuration
├── .vs/                    → Visual Studio local files
│
├── docker/                 → Docker environment configuration
│   ├── Dockerfile          → Defines API container build
│   ├── entrypoint.sh       → API startup script
│   ├── import-data.sh      → Optional script to seed data into PostgreSQL
│   └── rental_production.yml → Docker Compose configuration for production
│
├── sql/                    → Database initialization and schema scripts
│   └── init-database.sql   → Creates schema, tables, and constraints for PostgreSQL
│
├── src/                    → Application source code
│   ├── Building Blocks/
│   │   ├── Core/           → Common domain abstractions and base classes
│   │   │    └── Rental.Core
│   │   ├── MessageBus/     → Message bus abstractions and RabbitMQ integration
│   │   │    └── Rental.MessageBus
│   │   └── Services/       → Shared service implementations or utilities
│   │        └── Rental.Services
│   │
│   ├── Services/
│   │   ├── Rental.Api/     → Main API responsible for handling HTTP requests
│   │   └── Rental.Notification/ → Background worker responsible for persisting notification events
│
├── .dockerignore           
├── .gitignore              
├── LICENSE                 
├── README.md               
└── ServiceRental.sln       → Visual Studio solution file

```

📦 Projects Overview
| Project | Description |
|----------|-------------|
| **Rental.Core** | Contains core building blocks such as entities, value objects, events, and base command/query abstractions used across the solution. |
| **Rental.MessageBus** | Encapsulates integration with RabbitMQ using **EasyNetQ**. Handles publishing and subscribing of integration events. |
| **Rental.Services** | Contains reusable services and utilities shared across bounded contexts. |
| **Rental.Api** | The main entry point (**REST API**). Handles commands for courier registration, motorcycle management, and rental creation. Publishes integration events to RabbitMQ. |
| **Rental.Notification** | A background worker (microservice) that consumes integration events from RabbitMQ and records them into the `rental_service.notification` table. Ensures reliable event persistence and monitoring. |


### 🐘 Database Schema

All data is stored in the **`rental_service`** schema in **PostgreSQL**.

| Table | Description |
|--------|-------------|
| **courier** | Stores driver information (CNPJ, driver license, image, etc.). |
| **motorcycle** | Stores motorcycles available for rental. |
| **rental_plan** | Defines pricing models (daily rate, penalty percent, duration). |
| **rental** | Represents an active or completed rental, linking courier, motorcycle, and plan. |
| **notification** | Stores consumed integration events, including payload and delivery status. |


### 🐳 Running the Application with Docker Compose

The project includes a `docker-compose.yml` file to simplify environment setup and orchestration.

#### 🧩 Services Defined
- **PostgreSQL** — main relational database (`ServiceRentalDb`)  
- **RabbitMQ** — message broker with management UI  
- *(optional)* **Rental.Api** — the main .NET 8 Web API (commented out for manual build)

#### 🛠️ Steps to Start the Environment

1. **Navigate to the docker folder**
```
cd docker
```

2. **Start PostgreSQL and RabbitMQ**
```
docker compose -f rental_production.yml -p rental up
```
This will:

- Launch PostgreSQL on port 15432
- Launch RabbitMQ on ports 5672 (AMQP) and 15672 (Admin UI)
- Automatically execute the init-database.sql script located in the ../sql folder

3. **Verify services**
- PostgreSQL → localhost:15432
- RabbitMQ Management → http://localhost:15672
- (default credentials: guest / guest)


4 **Run the API locally (if not using Docker build)**
```
dotnet run --project ../src/services/Rental.Api
````


# 🏍️ Overview
This project is an implementation of the Mottu Backend Challenge, developed using .NET 8, PostgreSQL, RabbitMQ (with EasyNetQ), and CQRS architecture.
The main goal is to simulate a motorcycle rental management system, handling the registration of couriers and motorcycles, order creation, and event-driven communication between services.


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

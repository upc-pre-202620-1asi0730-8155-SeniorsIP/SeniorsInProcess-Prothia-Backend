# Prothia Platform — RESTful API & Backend Engine

[![Release](https://img.shields.io/badge/release-v0.1.8-blue.svg)](CHANGELOG.md)
[![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-14.0-blue.svg)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Database](https://img.shields.io/badge/Database-MySQL%208.0-orange.svg)](https://www.mysql.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE.md)

## Project Overview

The **Prothia Platform** is a biomedical telemetry and clinical management backend application designed to optimize the rehabilitation journey of lower-limb amputees. Built with **.NET 10** and **C# 14**, it adheres to **Domain-Driven Design (DDD)** tactical patterns and implements **Command Query Responsibility Segregation (CQRS)** across modular bounded contexts.

Prothia bridges the gap between **physiotherapists**, **amputee patients**, and **orthopedic prosthetic workshops**, capturing real-time gait telemetry (cadence, bilateral symmetry, impact peaks), automating postural alert escalation, managing home exercise prescriptions, and tracking mechanical prosthesis cycle wear.

---

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [Domain-Driven Design (DDD) Concepts](#domain-driven-design-ddd-concepts)
3. [Key Features & Best Practices](#key-features--best-practices)
4. [Bounded Contexts](#bounded-contexts)
   * [Patients](#patients)
   * [Prescriptions](#prescriptions)
   * [Monitoring & Telemetry](#monitoring--telemetry)
   * [Workshop & Fleet Inventory](#workshop--fleet-inventory)
   * [Communication](#communication)
   * [Analytics](#analytics)
   * [Billing](#billing)
   * [IAM (Identity & Access Management)](#iam-identity--access-management)
5. [Technologies Used](#technologies-used)
6. [Getting Started](#getting-started)
   * [Prerequisites](#prerequisites)
   * [Setup Instructions](#setup-instructions)
7. [Project Structure](#project-structure)
8. [Documentation](#documentation)
9. [Contributing & Changelog](#contributing--changelog)
10. [License](#license)

---

## Architecture Overview

The system architecture is structured around **Clean Architecture**, **Domain-Driven Design (DDD)**, and **CQRS**:

* **Domain Layer**: Contains aggregate roots, domain entities, value objects, domain events, domain error catalogs, and repository contracts. Possesses zero dependencies on frameworks, databases, or transport layers.
* **Application Layer**: Coordinates use cases through segregated **Command Services** (write side) and **Query Services** (read side), managing aggregate transactions and anti-corruption layers.
* **Infrastructure Layer**: Implements persistence via Entity Framework Core 10 (`Pomelo.EntityFrameworkCore.MySql`), schema configurations, password hashing (BCrypt), and JWT token generation.
* **Interfaces Layer (Presentation)**: Exposes RESTful controllers under kebab-case route conventions (`/api/v1/...`), DTO transport resources, and transform assemblers.

```
       ┌────────────────────────────────────────────────────────┐
       │                Interfaces Layer (REST)                │
       │    Controllers  •  Resources (DTOs)  •  Assemblers     │
       └───────────────────────────┬────────────────────────────┘
                                   │
       ┌───────────────────────────▼────────────────────────────┐
       │                   Application Layer                    │
       │    Command Services  •  Query Services  •  ACL Facades │
       └───────────────────────────┬────────────────────────────┘
                                   │
       ┌───────────────────────────▼────────────────────────────┐
       │                     Domain Layer                       │
       │  Aggregates  •  Value Objects  •  Events  •  Contracts │
       └───────────────────────────▲────────────────────────────┘
                                   │
       ┌───────────────────────────┴────────────────────────────┐
       │                  Infrastructure Layer                  │
       │    EF Core 10  •  MySQL Repositories  •  BCrypt / JWT  │
       └────────────────────────────────────────────────────────┘
```

---

## Domain-Driven Design (DDD) Concepts

* **Bounded Contexts**: Eight decoupled business domains (`Patients`, `Prescriptions`, `Monitoring`, `Workshop`, `Communication`, `Analytics`, `Billing`, `IAM`) and a shared technical kernel (`Shared`).
* **Aggregates & Invariant Protection**: Aggregates (`Patient`, `Prescription`, `Alert`, `ThresholdProfile`, `GaitSession`, `Prosthesis`, `MaintenanceSchedule`, `ClinicalReport`, `ClinicSubscription`, `User`) inherit from `AggregateRoot` and guard internal business consistency.
* **Value Objects**: Immutable domain concepts with structural equality (`Dni`, `EAmputation`, `EKLevel`, `SerialNumber`, `ReportCode`, `PaymentMethod`).
* **Anti-Corruption Layer (ACL)**: Outbound facades (`IPatientsContextFacade`, `ExternalPatientService`) prevent domain model contamination across bounded contexts.
* **Domain Events**: Aggregates record domain events (`ClinicalReportIssuedEvent`, etc.) in memory, dispatched upon transactional commit.

---

## Key Features & Best Practices

* **CQRS Application Architecture**:
  * Distinct command contracts (e.g. `IPatientCommandService`) handling aggregate state mutations with `IUnitOfWork.CompleteAsync()`.
  * Dedicated query services (e.g. `IPatientQueryService`) providing fast, side-effect-free data retrieval.
* **Query Performance Optimizations**:
  * Read services use `.AsNoTracking()` to eliminate Entity Framework change-tracking memory overhead.
* **Modernized Functional Result Pattern**:
  * Methods return `Result<T>` and strongly-typed `Error` records (`NotFound`, `Conflict`, `Validation`, `Failure`), eliminating exception-based control flow.
* **Interactive Scalar OpenAPI Documentation**:
  * Integrated Microsoft OpenAPI and Scalar API Reference at `/scalar/v1` with automated JWT Bearer authorization support.
* **Standardized Error Handling**:
  * Centralized ASP.NET Core `IExceptionHandler` (`GlobalExceptionHandler`) producing sanitized RFC 9457 `ProblemDetails` payloads.
* **Self-Healing Multi-Environment Startup**:
  * Automatic EF Core database migration on startup (`context.Database.Migrate()`) with baseline table detection and idempotent initial demo data seeding (`SeedDatabaseIfEmpty`).
* **Minimal API Options Tree**:
  * High-speed `/api/v1/options` endpoint supplying dynamic frontend navigation hierarchies and badge telemetry counts.

---

## Bounded Contexts

### Patients
* **Scope**: Amputee demographic profiles, amputation level classification, and functional mobility (K-Level) tiering.
* **Aggregates**: `Patient`
* **Value Objects**: `Dni`, `EAmputation`, `EKLevel`, `EStatus`
* **RESTful Endpoints**:
  * `POST /api/v1/patients` — Register new patient (`201 Created`)
  * `GET /api/v1/patients` — List all registered patients
  * `GET /api/v1/patients/{id}` — Fetch patient by ID
  * `PUT /api/v1/patients/{id}` — Update patient clinical profile
  * `DELETE /api/v1/patients/{id}` — Remove patient record

### Prescriptions
* **Scope**: Structured physical therapy regimens, rehabilitation phases, and exercise libraries.
* **Aggregates & Entities**: `Prescription`, `PrescribedExercise`, `Exercise`
* **RESTful Endpoints**:
  * `POST /api/v1/prescriptions` — Issue exercise prescription (`201 Created`)
  * `GET /api/v1/prescriptions` — List all prescriptions
  * `GET /api/v1/prescriptions/{id}` — Fetch prescription by ID
  * `GET /api/v1/prescriptions/patient/{patientId}` — Get active prescription by patient
  * `GET /api/v1/exercises` — List rehabilitation exercise library

### Monitoring & Telemetry
* **Scope**: Sensor gait sessions, cadence and bilateral symmetry evaluation, postural alert tracking, and threshold calibration.
* **Aggregates**: `Alert`, `ThresholdProfile`, `GaitSession`
* **Value Objects**: `AlertSeverity`, `AlertStatus`
* **RESTful Endpoints**:
  * `POST /api/v1/gait-sessions` — Stream/record gait session telemetry (`201 Created`)
  * `GET /api/v1/gait-sessions` — Query gait telemetry records
  * `GET /api/v1/gait-sessions/patient/{patientId}` — Query telemetry series by patient
  * `GET /api/v1/alerts` — Query clinical postural alerts
  * `PUT /api/v1/alerts/{id}/resolve` — Mark postural alert as attended/resolved
  * `POST /api/v1/threshold-profiles` — Create baseline threshold calibration
  * `PUT /api/v1/threshold-profiles/{id}` — Update threshold parameters

### Workshop & Fleet Inventory
* **Scope**: Prosthesis hardware fleet inventory, cycle accumulation, mechanical fatigue thresholds, and preventive maintenance scheduling.
* **Aggregates**: `Prosthesis`, `MaintenanceSchedule`, `MechanicalAlert`
* **Value Objects**: `SerialNumber`, `ProthesisStatus`, `WorkshopKLevel`, `MaintenanceStatus`
* **RESTful Endpoints**:
  * `POST /api/v1/prostheses` — Register / link prosthesis (`201 Created`)
  * `GET /api/v1/prostheses` — List prosthesis fleet inventory
  * `GET /api/v1/prostheses/{id}` — Fetch prosthesis hardware details
  * `POST /api/v1/maintenance-schedules` — Schedule preventive service (`201 Created`)
  * `GET /api/v1/maintenance-schedules` — List maintenance schedule
  * `POST /api/v1/mechanical-alerts` — Dispatch hardware fatigue alert
  * `PUT /api/v1/mechanical-alerts/{id}/resolve` — Resolve mechanical alert

### Communication
* **Scope**: Inter-professional messaging and technical case file transmissions between clinical therapists and orthopedic technicians.
* **Aggregates**: `ClinicalMessage`, `ClinicalTransmission`
* **RESTful Endpoints**:
  * `POST /api/v1/clinical-transmissions` — Transmit clinical summary to workshop (`201 Created`)
  * `GET /api/v1/clinical-transmissions` — List case file transmissions
  * `POST /api/v1/clinical-messages` — Send clinical coordination message
  * `GET /api/v1/clinical-messages` — Retrieve messaging thread

### Analytics
* **Scope**: Consolidated biomechanical progress reports, quantitative gait indicators, and CSV telemetry exports.
* **Aggregates & Entities**: `ClinicalReport`, `ReportIndicator`
* **Value Objects**: `ReportCode`, `IndicatorStatus`, `TherapistRegistry`
* **RESTful Endpoints**:
  * `POST /api/v1/reports` — Issue official clinical progress report (`201 Created`)
  * `GET /api/v1/reports` — Query clinical reports
  * `GET /api/v1/reports/{id}` — Get report detail sheet

### Billing
* **Scope**: Clinical center subscription management, follow-up quota tracking, and electronic tax invoices.
* **Aggregates**: `ClinicSubscription`, `ElectronicInvoice`
* **Value Objects**: `InvoiceNumber`, `InvoiceStatus`, `PaymentMethod`
* **RESTful Endpoints**:
  * `GET /api/v1/clinic-subscriptions` — Query active clinic subscription
  * `PUT /api/v1/clinic-subscriptions/{id}` — Update subscription tier / quota
  * `GET /api/v1/invoices` — List electronic tax invoices
  * `POST /api/v1/invoices` — Issue electronic invoice

### IAM (Identity & Access Management)
* **Scope**: User registration, credential hashing with BCrypt, and secure JWT token issuance.
* **Aggregates**: `User`
* **RESTful Endpoints**:
  * `POST /api/v1/authentication/sign-up` — Register user account (`201 Created`)
  * `POST /api/v1/authentication/sign-in` — Authenticate and receive JWT bearer token (`200 OK`)
  * `GET /api/v1/users` — List registered users
  * `GET /api/v1/users/{id}` — Get user profile by ID

---

## Technologies Used

* **.NET 10 (ASP.NET Core Web API)**: Modern runtime and web host.
* **C# 14**: Primary language utilizing records, pattern matching, and primary constructors.
* **Entity Framework Core 10**: Object-relational mapping with code-first migrations.
* **Pomelo.EntityFrameworkCore.MySql**: MySQL database provider.
* **MySQL Server 8.0**: Relational database engine.
* **Scalar.AspNetCore & Microsoft.AspNetCore.OpenApi**: Next-generation interactive API reference.
* **BCrypt.Net-Next**: Password hashing.
* **System.IdentityModel.Tokens.Jwt**: RFC 7519 compliant JSON Web Token management.

---

## Getting Started

### Prerequisites
* [.NET 10 SDK](https://dotnet.microsoft.com/download)
* [MySQL Server 8.0+](https://dev.mysql.com/downloads/installer/) (or Docker MySQL container)
* JetBrains Rider, Visual Studio 2026, or Visual Studio Code

### Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone https://github.com/SeniorsInProcess-Prothia/SeniorsInProcess-Prothia-Backend.git
   cd SeniorsInProcess-Prothia-Backend
   ```

2. **Configure Database Connection:**
   Ensure MySQL is running locally on port `3306`. Check `Prothia.API/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Port=3306;Database=prothia-db;Uid=root;Pwd=12345Mysql$;"
   }
   ```

3. **Restore and Build:**
   ```bash
   dotnet restore
   dotnet build
   ```

4. **Run the Application:**
   ```bash
   dotnet run --project Prothia.API
   ```
   *The database `prothia-db` will be created, migrated, and seeded with baseline clinical records automatically on first startup.*

5. **Explore Interactive Scalar API Reference:**
   * Interactive UI: [http://localhost:5178/scalar/v1](http://localhost:5178/scalar/v1)
   * OpenAPI Specification: [http://localhost:5178/openapi/v1.json](http://localhost:5178/openapi/v1.json)

---

## Project Structure

```
Prothia.API/
├── Analytics/                  # Analytics Bounded Context
│   ├── Application/            # Command & Query Services, Event Handlers
│   ├── Domain/                 # ClinicalReport Aggregate, ReportIndicator Entity
│   ├── Infrastructure/         # ClinicalReportRepository
│   └── Interfaces/Rest/        # ReportsController, Resources, Assemblers
├── Billing/                    # Billing Bounded Context
│   ├── Application/            # ClinicSubscription & Invoice Services
│   ├── Domain/                 # ClinicSubscription, ElectronicInvoice Aggregates
│   ├── Infrastructure/         # Subscription & Invoice Repositories
│   └── Interfaces/Rest/        # ClinicSubscriptionsController, InvoicesController
├── Communication/              # Communication Bounded Context
│   ├── Application/            # Message & Case File Transmission Services
│   ├── Domain/                 # ClinicalMessage, ClinicalTransmission Aggregates
│   ├── Infrastructure/         # Message & Transmission Repositories
│   └── Interfaces/Rest/        # ClinicalMessagesController, ClinicalTransmissionsController
├── Iam/                        # IAM Bounded Context
│   ├── Application/            # User Services, Token & Hashing Services
│   ├── Domain/                 # User Aggregate, Repositories
│   ├── Infrastructure/         # UserRepository, BCrypt, JWT TokenService
│   └── Interfaces/Rest/        # AuthenticationController, UsersController
├── Monitoring/                 # Monitoring Bounded Context
│   ├── Application/            # Alert, Threshold & Gait Telemetry Services
│   ├── Domain/                 # Alert, ThresholdProfile, GaitSession Aggregates
│   ├── Infrastructure/         # Alert, Threshold & GaitSession Repositories
│   └── Interfaces/Rest/        # AlertsController, ThresholdProfilesController, GaitSessionsController
├── Patients/                   # Patients Bounded Context
│   ├── Application/            # Patient Command & Query Services, ACL Facades
│   ├── Domain/                 # Patient Aggregate, Dni ValueObject
│   ├── Infrastructure/         # PatientRepository
│   └── Interfaces/Rest/        # PatientsController, Resources, Assemblers
├── Prescriptions/              # Prescriptions Bounded Context
│   ├── Application/            # Prescription & Exercise Services, ACL Outbound
│   ├── Domain/                 # Prescription Aggregate, PrescribedExercise, Exercise
│   ├── Infrastructure/         # Prescription & Exercise Repositories
│   └── Interfaces/Rest/        # PrescriptionsController, ExercisesController
├── Workshop/                   # Workshop Bounded Context
│   ├── Application/            # Prosthesis, Maintenance & Mechanical Alert Services
│   ├── Domain/                 # Prosthesis, MaintenanceSchedule, MechanicalAlert Aggregates
│   ├── Infrastructure/         # Prosthesis, Maintenance & Alert Repositories
│   └── Interfaces/Rest/        # ProsthesesController, MaintenanceSchedulesController, MechanicalAlertsController
├── Shared/                     # Shared Kernel & Infrastructure
│   ├── Domain/                 # AggregateRoot, IEntityId, Result<T>, Error, Base Repositories
│   ├── Infrastructure/         # AppDbContext, UnitOfWork, GlobalExceptionHandler
│   └── Interfaces/             # KebabCaseRouteNamingConvention, OpenAPI Transformers
├── Program.cs                  # Web application entry point, DI, and startup pipeline
└── appsettings.json            # Configuration settings and connection strings
```

---

## Documentation

* **Architecture Decision Records (ADRs)**: [`docs/adrs.md`](docs/adrs.md)
* **REST API Technical User Stories**: [`docs/user-stories.md`](docs/user-stories.md)
* **PlantUML Tactical Class Diagram**: [`docs/class-diagram.puml`](docs/class-diagram.puml)
* **Structurizr C4 Software Architecture DSL**: [`docs/software-architecture.dsl`](docs/software-architecture.dsl)
* **Study Guide Specification**: [`docs/study-guide-spec.md`](docs/study-guide-spec.md)
* **Software Architecture Study Guide**: [`docs/study-guide.md`](docs/study-guide.md)

---

## Contributing & Changelog

* Review [`CONTRIBUTING.md`](CONTRIBUTING.md) for branch policies, commit standards, and workflow.
* Review [`CHANGELOG.md`](CHANGELOG.md) for version release tracking and recent changes.

---

## License

This project is licensed under the **MIT License**. See [`LICENSE.md`](LICENSE.md) for details.

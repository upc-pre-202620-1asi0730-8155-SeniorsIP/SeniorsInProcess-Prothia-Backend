# Prothia Platform — Software Architecture & Engineering Study Guide

## Table of Contents
1. [Overview](#1-overview)
2. [Architecture Patterns](#2-architecture-patterns)
3. [Domain-Driven Design (DDD)](#3-domain-driven-design-ddd)
4. [CQRS Pattern](#4-cqrs-pattern)
5. [.NET Framework Features](#5-net-framework-features)
6. [C# Language Features](#6-c-language-features)
7. [Entity Framework Core](#7-entity-framework-core)
8. [Security & Authentication](#8-security-and-authentication)
9. [API Design & Best Practices](#9-api-design--best-practices)
10. [Verification, Testing & Quality](#10-verification-testing--quality)
11. [References](#11-references)

---

## 1. Overview

The **Prothia Platform** is an enterprise-grade RESTful API and cloud platform built with **.NET 10** and **C# 14**. It addresses lower-limb amputee rehabilitation by providing real-time biomechanical telemetry, postural gait asymmetry monitoring, physical therapy prescription management, and multi-facility coordination between clinical centers and orthopedic prosthetic workshops.

### Technology Stack
- **Framework**: .NET 10 (ASP.NET Core Web API)
- **Language**: C# 14
- **Database / ORM**: MySQL Server 8.0 via Entity Framework Core 10 (`Pomelo.EntityFrameworkCore.MySql`)
- **Authentication**: Stateless JSON Web Tokens (JWT) with BCrypt password hashing (`BCrypt.Net-Next`)
- **API Documentation**: Microsoft OpenAPI with Scalar API Reference (`Scalar.AspNetCore`)
- **Architectural Paradigm**: Domain-Driven Design (DDD), Clean Architecture, and Command Query Responsibility Segregation (CQRS)

### Bounded Contexts

The application domain is partitioned into eight functional bounded contexts and one shared technical kernel:

| Context | Responsibility | Key Aggregates & Entities |
|---------|----------------|---------------------------|
| **Shared** | Cross-cutting DDD primitives, repository abstractions, unit of work, and database context | `AggregateRoot`, `Result<T>`, `Error`, `AppDbContext` |
| **Patients** | Amputee clinical demographic data, amputation level classification, and K-Levels | `Patient`, `Dni` |
| **Prescriptions** | Home physical rehabilitation regimens, exercise phases, repetitions, and catalogs | `Prescription`, `PrescribedExercise`, `Exercise` |
| **Monitoring** | Biomechanical gait telemetry, postural compensation alerts, and threshold calibrations | `Alert`, `ThresholdProfile`, `GaitSession` |
| **Workshop** | Prosthetic device inventory, cycle accumulation, hardware fatigue, and preventive maintenance | `Prosthesis`, `MaintenanceSchedule`, `MechanicalAlert` |
| **Communication** | Inter-professional messaging and technical case file transmissions to orthopedic workshops | `ClinicalMessage`, `ClinicalTransmission` |
| **Analytics** | Longitudinal biomechanical progress evaluation, official clinical reports, and CSV telemetry exports | `ClinicalReport`, `ReportIndicator` |
| **Billing** | Clinic subscription tiers, patient follow-up quotas, and electronic tax invoicing | `ClinicSubscription`, `ElectronicInvoice` |
| **IAM** | Identity and Access Management, role-based authorization, credential hashing, and JWT tokens | `User` |

Each bounded context follows a four-layer hexagonal/clean architectural layout:
```
Prothia.API/
├── {BoundedContext}/
│   ├── Application/
│   │   ├── Acl/                      # Anti-Corruption Layer implementations
│   │   ├── CommandServices/          # Command service contracts
│   │   ├── QueryServices/            # Query service contracts
│   │   └── Internal/                 # Internal service implementations
│   ├── Domain/
│   │   ├── Model/
│   │   │   ├── Aggregates/           # Consistency roots
│   │   │   ├── Commands/             # Mutation records
│   │   │   ├── Queries/              # Read records
│   │   │   ├── Events/               # Domain event records
│   │   │   ├── ValueObjects/         # Immutable value objects
│   │   │   └── Errors/               # Domain error catalogs
│   │   └── Repositories/             # Repository interfaces
│   ├── Infrastructure/
│   │   └── Persistence/              # EF Core configurations and repositories
│   └── Interfaces/
│       └── Rest/
│           ├── Resources/            # Transport DTOs
│           └── Transform/            # Assemblers mapping entities <-> resources
```

---

## 2. Architecture Patterns

### 2.1 Clean Architecture

**Concept**: Clean Architecture arranges software systems into concentric circles where dependencies point strictly inward toward the business domain. The Domain layer has zero dependencies on external frameworks, databases, or HTTP transport concerns.

**In the Codebase**: The `Patient` aggregate root in `Patients/Domain/Model/Aggregates/Patient.cs` is pure domain logic with no references to ASP.NET Core or EF Core:

```csharp
// Path: Prothia.API/Patients/Domain/Model/Aggregates/Patient.cs
public class Patient : AggregateRoot
{
    public string FullName { get; private set; }
    public Dni Dni { get; private set; }
    public int Age { get; private set; }
    public EAmputation Amputation { get; private set; }
    public EKLevel KLevel { get; private set; }
    public string? ProsthesisCode { get; private set; }
    public int Adherence { get; private set; }
    public int Symmetry { get; private set; }
    public EStatus Status { get; private set; }

    public Patient(CreatePatientCommand command)
    {
        FullName = command.FullName;
        Dni = new Dni(command.Dni);
        Age = command.Age;
        Amputation = command.Amputation;
        KLevel = command.KLevel;
        ProsthesisCode = command.ProsthesisCode;
        Adherence = 0;
        Symmetry = 0;
        Status = EStatus.Stable;
    }
}
```

**Why It Matters**: The clinical business rules of lower-limb amputee management can be tested, refactored, and maintained completely independent of MySQL or Web API frameworks.

---

### 2.2 Repository Pattern & Unit of Work

**Concept**: The Repository pattern mediates between the domain and data mapping layers using a collection-like interface for accessing domain objects. The Unit of Work pattern coordinates writes and commits across multiple repositories within a single business transaction.

**In the Codebase**:
- `IBaseRepository<TEntity>` defines generic persistence operations.
- `IUnitOfWork` exposes `CompleteAsync()` to wrap EF Core's `SaveChangesAsync()`.

```csharp
// Path: Prothia.API/Shared/Domain/Repositories/IBaseRepository.cs
public interface IBaseRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TEntity?> FindByIdAsNoTrackingAsync(int id, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<IEnumerable<TEntity>> ListAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> ListAsNoTrackingAsync(CancellationToken cancellationToken = default);
}

// Path: Prothia.API/Shared/Infrastructure/Persistence/EntityFrameworkCore/Repositories/UnitOfWork.cs
public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task CompleteAsync() => await context.SaveChangesAsync();
}
```

**Why It Matters**: Centralizes entity querying logic and ensures that mutations across aggregates either commit completely or roll back together.

---

### 2.3 Functional Result Pattern

**Concept**: The Result pattern replaces throwing exceptions for expected business failures (e.g. duplicate DNI, patient not found, invalid threshold range) with an explicit return object indicating either success with a value or failure with a typed error.

**In the Codebase**:
```csharp
// Path: Prothia.API/Shared/Domain/Model/Result.cs
public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public Error Error { get; }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(Error error) => new(error);

    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure) =>
        IsSuccess ? onSuccess(Value!) : onFailure(Error);
}
```

**Why It Matters**: Eliminates performance overhead from stack-trace generation and produces clean, self-documenting controller methods without verbose `try/catch` blocks.

---

### 2.4 Assembler / Transform Pattern

**Concept**: Assemblers are dedicated stateless mapper components that translate between internal domain models (Commands, Aggregates) and external REST transport DTOs (Resources).

**In the Codebase**:
```csharp
// Path: Prothia.API/Patients/Interfaces/Rest/Transform/PatientResourceFromEntityAssembler.cs
public static class PatientResourceFromEntityAssembler
{
    public static PatientResource ToResourceFromEntity(Patient entity) =>
        new(
            entity.Id,
            entity.FullName,
            entity.Dni.Value,
            entity.Age,
            entity.Amputation.ToString(),
            entity.KLevel.ToString(),
            entity.ProsthesisCode ?? string.Empty,
            entity.Adherence,
            entity.Symmetry,
            entity.Status.ToString().ToLowerInvariant()
        );
}
```

**Why It Matters**: Prevents internal database schemas or sensitive aggregate attributes from leaking over the public HTTP API.

---

### 2.5 Anti-Corruption Layer (ACL)

**Concept**: When one bounded context must consume data or services from another context, an ACL translates foreign domain models into concepts native to the consuming context, preventing model contamination.

**In the Codebase**: The `Prescriptions` and `Analytics` contexts consume `IPatientContextFacade` or `IExternalPatientService` rather than directly querying the `Patients` database repository:

```csharp
// Path: Prothia.API/Prescriptions/Application/Internal/OutboundServices/ACL/ExternalPatientService.cs
public class ExternalPatientService(IPatientsContextFacade patientsContextFacade) : IExternalPatientService
{
    public async Task<bool> ExistsPatientByIdAsync(int patientId) =>
        await patientsContextFacade.ExistsPatientByIdAsync(patientId);
}
```

---

## 3. Domain-Driven Design (DDD)

### 3.1 Aggregates & Aggregate Roots
An Aggregate is a cluster of associated objects treated as a unit for data changes. Every aggregate has an **Aggregate Root** that ensures all invariants within the boundary are preserved:
- In `Prothia.API/Prescriptions/Domain/Model/Aggregates/Prescription.cs`, the root `Prescription` manages its internal collection of `PrescribedExercise` entities. Outside callers cannot add exercises directly to the database; they must call `prescription.AddExercise(...)`.
- In `Prothia.API/Workshop/Domain/Model/Aggregates/Prosthesis.cs`, lifecycle transitions (such as accumulated cycle updates and battery degradation) are encapsulated within aggregate methods.

### 3.2 Value Objects
Value Objects are immutable domain concepts defined by their attributes rather than a persistent identity:
- `Dni`: Enforces the Peruvian 8-digit numeric identification rule.
- `EAmputation`: Strongly typed enumeration of lower-limb amputation levels (`TranstibialRight`, `TransfemoralLeft`, `TranstibialBilateral`, etc.).
- `EKLevel`: Medicare functional mobility levels (`K1`, `K2`, `K3`, `K4`).
- `PaymentMethod`: Encapsulates credit card brand, masked digits, and expiry date in `Billing`.

### 3.3 Domain Events
Aggregates inherit from `AggregateRoot` to accumulate domain events during business state transitions:
- `ClinicalReportIssuedEvent`: Raised when a clinical director issues an official report, triggering archival or notification handlers.

---

## 4. CQRS Pattern

Command Query Responsibility Segregation separates read models from write models.

```
Incoming Request
      │
      ├─ Write Request ──► Controller ──► CommandService ──► Aggregate ──► Repository ──► UnitOfWork.CompleteAsync()
      │
      └─ Read Request  ──► Controller ──► QueryService   ──► Repository (AsNoTracking) ──► Assembler ──► Resource
```

### Command Service Example:
```csharp
// Path: Prothia.API/Patients/Application/Internal/CommandServices/PatientCommandService.cs
public class PatientCommandService(IPatientRepository patientRepository, IUnitOfWork unitOfWork) : IPatientCommandService
{
    public async Task<Result<Patient>> Handle(CreatePatientCommand command)
    {
        if (await patientRepository.ExistsByDniAsync(new Dni(command.Dni)))
            return Result<Patient>.Failure(PatientErrors.DuplicateDni);

        var patient = new Patient(command);
        await patientRepository.AddAsync(patient);
        await unitOfWork.CompleteAsync();
        return Result<Patient>.Success(patient);
    }
}
```

### Query Service Example:
```csharp
// Path: Prothia.API/Patients/Application/Internal/QueryServices/PatientQueryService.cs
public class PatientQueryService(IPatientRepository patientRepository) : IPatientQueryService
{
    public async Task<IEnumerable<Patient>> Handle(GetAllPatientsQuery query) =>
        await patientRepository.ListAsNoTrackingAsync();

    public async Task<Patient?> Handle(GetPatientByIdQuery query) =>
        await patientRepository.FindByIdAsNoTrackingAsync(query.Id);
}
```

---

## 5. .NET Framework Features

### 5.1 Kebab-Case URL Routing
Configured globally in `Program.cs` via `KebabCaseRouteNamingConvention`:
```csharp
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new KebabCaseRouteNamingConvention());
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});
```
This automatically maps `ThresholdProfilesController` to `/api/v1/threshold-profiles` and `MaintenanceSchedulesController` to `/api/v1/maintenance-schedules`.

### 5.2 Minimal API Navigation Endpoint
In `Program.cs`, a fast in-memory Minimal API endpoint provides the frontend menu navigation tree with route badges:
```csharp
app.MapGet("/api/v1/options", () => Results.Ok(new object[]
{
    new { id = 1, label = "option.dashboard", to = "/home", icon = "pi-th-large", exact = true, badge = 0, order = 1 },
    new { id = 2, label = "option.patients", to = "/patients", icon = "pi-users", exact = false, badge = 0, order = 2 },
    new { id = 3, label = "option.prescriptions", to = "/prescriptions", icon = "pi-clipboard", exact = false, badge = 0, order = 3 },
    new { id = 4, label = "option.alerts", to = "/alerts", icon = "pi-exclamation-triangle", exact = false, badge = 3, order = 4 }
}));
```

### 5.3 Global Exception Handler & Problem Details
Using .NET's `IExceptionHandler`:
```csharp
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
```
Transforms unhandled exceptions into RFC 9457 compliant error responses without leaking server stack traces.

### 5.4 Scalar OpenAPI Documentation
```csharp
builder.Services.AddOpenApi();
// In Development:
app.MapOpenApi();
app.MapScalarApiReference();
app.MapGet("/", () => Results.Redirect("/scalar/v1"));
```
Available at `http://localhost:5178/scalar/v1`.

---

## 6. C# Language Features

1. **Records**: Used for immutable command definitions, queries, and resource DTOs:
   ```csharp
   public record CreatePatientCommand(string FullName, string Dni, int Age, EAmputation Amputation, EKLevel KLevel, string? ProsthesisCode);
   public record GetPatientByIdQuery(int Id);
   ```
2. **Primary Constructors**: Clean dependency injection syntax without boilerplate field assignments:
   ```csharp
   public class PatientsController(IPatientCommandService commandService, IPatientQueryService queryService) : ControllerBase
   ```
3. **Pattern Matching & Switch Expressions**: Used in enum conversions and HTTP response status mapping:
   ```csharp
   public static IActionResult ToActionResult(Error error) => error.Type switch
   {
       ErrorType.NotFound => new NotFoundObjectResult(error),
       ErrorType.Conflict => new ConflictObjectResult(error),
       ErrorType.Validation => new BadRequestObjectResult(error),
       _ => new BadRequestObjectResult(error)
   };
   ```
4. **File-Scoped Namespaces & Nullable Reference Types**: Eliminates nesting indentation and enforces null-safety checks at compile time.

---

## 7. Entity Framework Core

### 7.1 Database Context & Pomelo MySQL
Configured in `Program.cs`:
```csharp
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionString = Environment.ExpandEnvironmentVariables(builder.Configuration.GetConnectionString("DefaultConnection")!);
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
```

### 7.2 Fluent Mapping via ModelBuilderExtensions
Each bounded context isolates its schema mappings inside extension methods:
- Table naming: `patients`, `prescriptions`, `gait_sessions`, `alerts`, `prostheses`.
- Value object conversion: mapping `Dni` value objects to database `VARCHAR(8)` columns.
- Audit shadow properties: `created_at` and `updated_at` automatically populated via `AppDbContext`.

### 7.3 Automatic Startup Migrations & Seeding Resilience
`Program.cs` incorporates an intelligent startup check that runs `context.Database.Migrate()` and seeds baseline clinical demo records (`SeedDatabaseIfEmpty`) without crashing if tables already exist.

---

## 8. Security & Authentication

### 8.1 Password Hashing with BCrypt
```csharp
// Path: Prothia.API/Iam/Infrastructure/Hashing/BCrypt/Services/HashingService.cs
public class HashingService : IHashingService
{
    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    public bool VerifyPassword(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
}
```

### 8.2 JSON Web Tokens (JWT)
`TokenService` signs tokens with HMAC-SHA256 containing `ClaimTypes.NameIdentifier`, `ClaimTypes.Name`, and `ClaimTypes.Role` with configurable expiration.

---

## 9. API Design & Best Practices

- **URI Resource Hierarchy**: Plural nouns (`/api/v1/patients`, `/api/v1/alerts/{id}/resolve`).
- **Standard HTTP Verbs**:
  - `GET`: Safe, idempotent read operations.
  - `POST`: Create new resources, returning `201 Created` with a `Location` header.
  - `PUT`: Update existing resources or idempotent state transitions.
  - `DELETE`: Remove resources, returning `200 OK` or `204 No Content`.
- **Content Negotiation**: `application/json; charset=utf-8`.

---

## 10. Verification, Testing & Quality

### End-to-End Integration Test Suite
The Prothia API is validated using an automated 26-endpoint integration test suite verifying:
- Patient lifecycle (`POST` -> `GET` -> `PUT` -> `DELETE`)
- Exercise prescription creation and retrieval
- Biomechanical gait telemetry persistence and query
- Postural and mechanical alert generation and resolution
- Multi-context ACL integrity between Prescriptions and Patients

---

## 11. References

1. **Evans, Eric (2003)**: *Domain-Driven Design: Tackling Complexity in the Heart of Software*. Addison-Wesley.
2. **Vernon, Vaughn (2013)**: *Implementing Domain-Driven Design*. Addison-Wesley.
3. **Martin, Robert C. (2017)**: *Clean Architecture: A Craftsman's Guide to Software Structure and Design*. Prentice Hall.
4. **Fowler, Martin (2002)**: *Patterns of Enterprise Application Architecture*. Addison-Wesley.
5. **Microsoft Learn**: *ASP.NET Core Web API Best Practices & Architecture Guides*.

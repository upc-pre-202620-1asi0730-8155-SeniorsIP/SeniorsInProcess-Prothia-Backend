# Architecture Decision Records (ADRs)

This document records the key architectural decisions made for the **Prothia Platform**, detailing the context, options considered, decisions, and consequences.

---

## ADR Index

1. [ADR-001: Adoption of Domain-Driven Design (DDD) and Tactical Patterns](#adr-001-adoption-of-domain-driven-design-ddd-and-tactical-patterns)
2. [ADR-002: Command Query Responsibility Segregation (CQRS) with Application Command & Query Services](#adr-002-command-query-responsibility-segregation-cqrs-with-application-command--query-services)
3. [ADR-003: Modernized Functional Result Pattern and Discriminated Error Model](#adr-003-modernized-functional-result-pattern-and-discriminated-error-model)
4. [ADR-004: Strongly-Typed Identifiers with Zero-Allocation Value Objects](#adr-004-strongly-typed-identifiers-with-zero-allocation-value-objects)
5. [ADR-005: Query Optimization with Non-Tracking Reads and Eager Projections](#adr-005-query-optimization-with-non-tracking-reads-and-eager-projections)
6. [ADR-006: In-Memory Domain Event Accumulation and Transactional Persistence](#adr-006-in-memory-domain-event-accumulation-and-transactional-persistence)
7. [ADR-007: Adoption of Scalar API Reference and Built-in ASP.NET Core OpenAPI](#adr-007-adoption-of-scalar-api-reference-and-built-in-aspnet-core-openapi)
8. [ADR-008: RESTful IAM Design for Clinician and Technician Token Authentication](#adr-008-restful-iam-design-for-clinician-and-technician-token-authentication)
9. [ADR-009: Centralized Exception Handling with IExceptionHandler and RFC 9457 Problem Details](#adr-009-centralized-exception-handling-with-iexceptionhandler-and-rfc-9457-problem-details)
10. [ADR-010: EF Core Code-First Migrations with MySQL Baseline Resilience](#adr-010-ef-core-code-first-migrations-with-mysql-baseline-resilience)

---

## ADR-001: Adoption of Domain-Driven Design (DDD) and Tactical Patterns

- **Status:** Accepted
- **Deciders:** Architecture Team
- **Date:** 2026-10-25

### Context
Prothia operates in a specialized biomedical domain involving lower-limb amputee rehabilitation, biomechanical telemetry, orthopedic hardware fatigue, and multi-facility clinical coordination. An anemic or flat CRUD monolithic architecture causes business rule fragmentation and uncontrolled coupling across domains.

### Decision
Structure the platform into explicit **Bounded Contexts**: `Patients`, `Prescriptions`, `Monitoring`, `Workshop`, `Communication`, `Analytics`, `Billing`, `Iam`, and `Shared`:
- **Aggregates & Aggregate Roots:** Protect consistency boundaries and enforce clinical invariants (`Patient`, `Prescription`, `Alert`, `ThresholdProfile`, `GaitSession`, `Prosthesis`, `MaintenanceSchedule`, `ClinicalTransmission`, `ClinicalReport`, `ClinicSubscription`, `User`).
- **Value Objects:** Encapsulate immutable domain concepts (`Dni`, `EAmputation`, `EKLevel`, `SerialNumber`, `ReportCode`, `InvoiceNumber`, `PaymentMethod`).
- **Anti-Corruption Layers (ACL):** Context facades (such as `IPatientsContextFacade`) mediate inter-context queries without domain model leakage.

### Consequences
- **Positive:** Clear separation of clinical and orthopedic responsibilities; domain logic is isolated and testable without database dependencies.
- **Negative:** Requires mapping layers (assemblers) between REST resources, command records, and domain entities.

---

## ADR-002: Command Query Responsibility Segregation (CQRS) with Application Command & Query Services

- **Status:** Accepted
- **Deciders:** Architecture Team
- **Date:** 2026-10-25

### Context
Mixing transactional write operations (such as registering patients, issuing prescriptions, recording sensor telemetry) and high-frequency read queries inside single monolithic service classes degrades performance and violates Single Responsibility Principle.

### Decision
Segregate application operations into distinct **Command Services** and **Query Services**:
- Command Services (e.g. `PatientCommandService`, `GaitSessionCommandService`, `MaintenanceScheduleCommandService`) handle state changes, aggregate mutations, and call `IUnitOfWork.CompleteAsync()`.
- Query Services (e.g. `PatientQueryService`, `AlertQueryService`, `ProsthesisQueryService`) perform side-effect-free, optimized queries using `AsNoTracking()`.

### Consequences
- **Positive:** High performance on read queries, clear write transactions, and independent evolution of command vs query logic.
- **Negative:** More interfaces and service classes per bounded context.

---

## ADR-003: Modernized Functional Result Pattern and Discriminated Error Model

- **Status:** Accepted
- **Deciders:** Architecture Team
- **Date:** 2026-10-25

### Context
Using exceptions for flow control (e.g. invalid amputation level, duplicate DNI, patient not found) incurs CPU overhead and pollutes application logs with non-fatal operational outcomes.

### Decision
Implement a generic functional `Result<T>` and `Error` model within the `Shared` bounded context:
- `Result<T>.Success(value)` and `Result<T>.Failure(error)`.
- Standardized `ErrorType` classification: `Failure`, `Validation`, `NotFound`, `Conflict`, `Unauthorized`, `Forbidden`.
- Controllers unwrap results into standardized HTTP responses using action result assemblers.

### Consequences
- **Positive:** Explicit, type-safe return contracts that communicate operational success or domain failure cleanly.
- **Negative:** Developers must consistently check `result.IsSuccess` or use `Match()` rather than catching exceptions.

---

## ADR-004: Strongly-Typed Identifiers with Zero-Allocation Value Objects

- **Status:** Accepted
- **Deciders:** Architecture Team
- **Date:** 2026-10-25

### Context
Passing primitive integers (`int id`) for different entities leads to subtle bugs (e.g., passing a `patientId` where a `prescriptionId` or `prosthesisId` was expected).

### Decision
Implement strongly-typed identifier records and value objects implementing `IEntityId<int>` or record wrappers (e.g. `PatientId`, `PrescriptionId`, `ProsthesisId`, `AlertId`).

### Consequences
- **Positive:** Compile-time verification prevents accidental identifier swapping across domains.
- **Negative:** Requires EF Core value converters to map identifiers to underlying integer primary/foreign keys.

---

## ADR-005: Query Optimization with Non-Tracking Reads and Eager Projections

- **Status:** Accepted
- **Deciders:** Architecture Team
- **Date:** 2026-10-26

### Context
Entity Framework Core by default tracks every entity loaded into memory. In read-heavy scenarios (e.g. patient directories, alert feeds, telemetry history), tracking causes unnecessary memory allocation and CPU overhead.

### Decision
Enforce `AsNoTracking()` across all repository query methods (`ListAsNoTrackingAsync()`, `FindByIdAsNoTrackingAsync()`) in `IBaseRepository<TEntity>` and custom repositories.

### Consequences
- **Positive:** Drastically reduced memory allocations and faster query execution times.
- **Negative:** Entities retrieved via query services cannot be mutated and committed directly without explicit re-attaching or fetching through tracking repositories.

---

## ADR-006: In-Memory Domain Event Accumulation and Transactional Persistence

- **Status:** Accepted
- **Deciders:** Architecture Team
- **Date:** 2026-10-26

### Context
When clinical state changes occur (e.g. patient registered, threshold calibrated, alert resolved), other contexts or event handlers need notifications without tightly coupling the write pipeline.

### Decision
Implement in-memory domain event accumulation via `IAggregateRoot` and `AggregateRoot`:
- Aggregates call `AddDomainEvent(IEvent event)` during business operations.
- Events are dispatched and cleared during unit-of-work transactions.

### Consequences
- **Positive:** Decoupled event propagation and strict aggregate consistency.
- **Negative:** In-memory dispatch does not guarantee cross-process delivery without an external message broker (acceptable for monolithic modular deployment).

---

## ADR-007: Adoption of Scalar API Reference and Built-in ASP.NET Core OpenAPI

- **Status:** Accepted
- **Deciders:** Architecture Team
- **Date:** 2026-10-27

### Context
Swashbuckle is no longer shipped with modern .NET 9/10 templates, and its maintenance has slowed down. Developers require rich interactive API documentation.

### Decision
Adopt the built-in Microsoft ASP.NET Core OpenAPI document generator (`Microsoft.AspNetCore.OpenApi`) paired with **Scalar API Reference** (`Scalar.AspNetCore`):
- `builder.Services.AddOpenApi()`
- `app.MapOpenApi()` and `app.MapScalarApiReference()`
- Custom OpenAPI transformers for JWT Bearer security schemes.

### Consequences
- **Positive:** Modern, high-performance interactive API documentation UI available at `/scalar/v1`, fully compatible with .NET 10.
- **Negative:** Configuration differs from legacy Swagger annotations.

---

## ADR-008: RESTful IAM Design for Clinician and Technician Token Authentication

- **Status:** Accepted
- **Deciders:** Architecture Team
- **Date:** 2026-10-27

### Context
Prothia requires role-based access for physiotherapists, orthopedic technicians, and clinic administrators. The system requires stateless authentication across web and mobile portals.

### Decision
Implement a dedicated `IAM` bounded context providing:
- Secure password hashing using BCrypt (`BCrypt.Net-Next`).
- Cryptographic JWT token generation with role claims (`Fisioterapeuta`, `Tecnico`, `Administrador`).
- Endpoints: `POST /api/v1/authentication/sign-up` and `POST /api/v1/authentication/sign-in`.

### Consequences
- **Positive:** Stateless, horizontally scalable token authentication adhering to RFC 7519.
- **Negative:** Token revocation requires short lifetimes or a token blocklist.

---

## ADR-009: Centralized Exception Handling with IExceptionHandler and RFC 9457 Problem Details

- **Status:** Accepted
- **Deciders:** Architecture Team
- **Date:** 2026-10-28

### Context
Unhandled exceptions leak internal stack traces and server details to API consumers, creating security vulnerabilities and inconsistent error formats.

### Decision
Implement ASP.NET Core's native `IExceptionHandler` interface via `GlobalExceptionHandler` and configure `builder.Services.AddProblemDetails()`:
- Maps unexpected exceptions to RFC 9457 `ProblemDetails` payloads.
- Logs structured error telemetry while returning safe, sanitized error descriptions to clients.

### Consequences
- **Positive:** Consistent error response schema for all API consumers across all endpoints.
- **Negative:** Custom domain errors must still be mapped carefully to appropriate HTTP status codes.

---

## ADR-010: EF Core Code-First Migrations with MySQL Baseline Resilience

- **Status:** Accepted
- **Deciders:** Architecture Team
- **Date:** 2026-11-10

### Context
Team members working across different machines frequently experience setup failures when MySQL already contains existing tables or when initial migrations encounter duplicate table creation errors on startup.

### Decision
Implement an automatic, self-healing startup migration routine in `Program.cs`:
- Detects existing tables in `prothia-db` and seeds baseline migration history in `__EFMigrationsHistory` if needed.
- Executes `context.Database.Migrate()` on startup followed by idempotent initial data seeding (`SeedDatabaseIfEmpty`).

### Consequences
- **Positive:** Zero-friction team onboarding; developers can clone the repository, run the project, and have the database automatically migrated and seeded.
- **Negative:** In production, migrations should be executed via separate CI/CD migration bundles rather than on web app startup.

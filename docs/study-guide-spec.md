# Study Guide Specification

## Using AI to Analyze and Learn from a Codebase

Use the following prompt to create a study guide specification for analyzing a Codebase:

---

```
Given I am a Software Engineering Student and this Codebase is an example of a RESTful API, part of the Web 
Applications course I am enrolled in, so analyze the codebase and help me understand the approaches, patterns, 
Framework and language features applied, so I can apply them in my own projects. Based on this, create a 
Study Guide (docs/study-guide.md), including detailed explanations and references for each relevant topic, 
so I as an student can use it as a source for learning and developing my skills.
```

---

A more specific prompt can be created by adding the following information:

```
Given I am a Software Engineering Student and this Codebase is an example of a production-quality application, 
analyze the entire codebase and help me understand the approaches, patterns, frameworks, and language features applied, 
so I can apply them in my own projects.

Based on this analysis, create a comprehensive Study Guide document at `docs/study-guide.md`.

## Role & Audience

Write the guide for a software engineering student who wants to learn from this codebase as a reference implementation. 
Each topic should be explained at a level that helps the student understand not just what the code does, but why 
it was designed that way and how they can replicate the pattern.

## Document Structure

Organize the study guide into the following sections, adjusting the technical content to match what the codebase 
actually uses (omit sections that don't apply, add sections for patterns or features not listed here but present 
in the codebase):

### 1. Overview
- Summarize the project: what it builds (Prothia - Telemetry and Clinical Management Platform for Lower-Limb Prosthetic Rehabilitation), 
  the technology stack (framework version, language version, database, etc.), and the key architectural approaches.
- Describe the modular decomposition across Bounded Contexts (Patients, Prescriptions, Monitoring, Workshop, Communication, Analytics, Billing, IAM, Shared).

### 2. Architecture Patterns
For each architectural pattern found in the codebase, provide a subsection that includes:
- **Concept**: A clear definition of the pattern and when to use it.
- **In the Codebase**: A code snippet extracted from the actual codebase showing the pattern in use, with the 
  relevant file paths listed.
- **Key Files**: Paths to the files that best demonstrate this pattern.
- **Why It Matters**: A practical explanation of the benefit this pattern provides.

Patterns to cover: Clean Architecture, Layered Architecture, Repository, Unit of Work, Dependency Injection, 
Middleware, Assembler, Service Layer, CQRS (Command & Query Services), Anti-Corruption Layer (ACL), and Functional Result Pattern.

### 3. Domain-Driven Design (DDD)
Cover the DDD tactical and strategic patterns found:
- **Bounded Contexts**: How the domain is partitioned and why.
- **Aggregates & Aggregate Roots**: Invariants and consistency boundaries.
- **Entities & Value Objects**: Distinction between identity-based and structural-equality concepts.
- **Domain Events**: Event generation on state transitions.
- **Repositories & Factory/Assembler Methods**: Encapsulating persistence and DTO mappings.

### 4. CQRS (Command Query Responsibility Segregation)
- Separation of Command Services (write model) from Query Services (read model).
- Usage of Command and Query records.
- Transaction management through Unit of Work.

### 5. .NET Framework Features
- ASP.NET Core Web API with Controllers and Minimal API endpoints.
- Dependency Injection configuration via Program.cs extensions.
- Global exception handling and RFC 9457 Problem Details.
- Kebab-case URL routing convention.
- OpenAPI specification with Scalar API Reference.

### 6. C# Language Features
- Strongly-typed identifier records and zero-allocation record structs.
- Pattern matching and switch expressions.
- Primary constructors and file-scoped namespaces.
- Nullable reference types and data annotations.

### 7. Entity Framework Core (EF Core)
- DbContext configuration and entity type mapping with ModelBuilderExtensions.
- Code-First Migrations with MySQL (`Pomelo.EntityFrameworkCore.MySql`).
- Query optimization with `AsNoTracking()` for read operations.
- Shadow properties, audit columns, and snake_case naming conventions.

### 8. Security & Authentication
- JWT (JSON Web Tokens) bearer authentication.
- Password hashing with BCrypt.
- Protected endpoints via authorize attributes and middleware.

### 9. API Design & Best Practices
- RESTful resource hierarchy under `/api/v1`.
- HTTP status codes and idempotency.
- Localized response messages and error catalogs.

### 10. Verification, Testing & Quality
- End-to-end integration testing against real database instances.
- Error resilience and baseline migration recovery.

### 11. References
- Official documentation links and recommended textbooks.
```

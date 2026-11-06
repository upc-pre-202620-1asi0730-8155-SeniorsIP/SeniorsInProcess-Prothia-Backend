# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and
[Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Planned work to harden API validation, security boundaries, and persistence workflows.
- Roadmap items for expanding bounded-context capabilities and stabilizing shared infrastructure.

### Changed
- Release tracking is being maintained in line with repository-wide documentation best practices.

### Fixed
- No fixes recorded for the current unreleased cycle.

## [0.1.2] - 2026-11-06

### Added
- Project version metadata finalized at `0.1.1` and aligned with the current .NET 10 solution setup.
- ASP.NET Core API foundation with controller registration, lowercase routing, and kebab-case endpoint naming conventions.
- OpenAPI support through `Microsoft.AspNetCore.OpenApi`, along with Scalar UI for local API exploration.
- DDD-oriented modular structure spanning the main bounded contexts, including Analytics, Billing, Patients, Shared, and supporting infrastructure.
- Shared application, infrastructure, and repository patterns for command/query separation and extensibility.
- EF Core and MySQL persistence scaffolding to support future domain and data access work.
- Resource localization support for common and domain-specific messages in English and Spanish.
- Centralized error handling and problem-details responses for consistent API behavior.
- Solution and project metadata configuration in `prothia.sln`, `global.json`, and `Prothia.API/Prothia.API.csproj`.

### Changed
- Standardized runtime and SDK configuration around `.NET 10` to keep builds consistent across environments.
- Organized the solution around modular, DDD-inspired service boundaries to support incremental feature delivery.
- Clarified API bootstrap and dependency conventions for routing, documentation, and persistence setup.
- Adopted changelog conventions aligned with Keep a Changelog for project-level release documentation.

### Fixed
- No production fixes were required for this maintenance-level version update.

### Security
- No authentication, authorization, or explicit security-hardening changes were introduced in this release.

### Deprecated
- No deprecated APIs or features were recorded for this release.

## [0.0.1] - 2026-11-05

### Added
- Initial Prothia backend API scaffold built on .NET 10.
- Initial ASP.NET Core configuration with controller registration, lowercase URL routing, and kebab-case endpoint naming.
- Initial OpenAPI documentation setup with Scalar UI integration for API exploration.
- Initial modular backend layout grouped by bounded contexts, including Analytics, Billing, Patients, Monitoring, and Shared infrastructure.
- Initial DDD foundation with application services, domain aggregates, value objects, repositories, and infrastructure layers.
- Initial persistence and data access foundation using EF Core with MySQL integration.
- Initial shared middleware and extension-based configuration for API bootstrap and common behavior.
- Initial global error-handling and problem-details infrastructure for consistent API responses.
- Initial resource localization support for English and Spanish messages.
- Initial solution and project metadata configuration in `prothia.sln`, `global.json`, and `Prothia.API/Prothia.API.csproj`.

### Changed
- Standardized the SDK and runtime configuration using `global.json` for a consistent .NET 10 build experience.
- Organized the codebase around modular, DDD-flavored service boundaries to support incremental feature delivery.
- Adopted consistent interface and configuration conventions for routing, OpenAPI, and persistence setup.

### Fixed
- No production fixes were recorded for the initial release milestone.

### Security
- No explicit security hardening or authentication changes were recorded in this initial release.

### Deprecated
- No deprecated APIs or features were recorded in this initial release.
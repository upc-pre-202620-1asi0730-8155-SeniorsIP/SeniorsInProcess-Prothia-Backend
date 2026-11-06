# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and
[Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Planned improvements for API stability, validation, and feature expansion.
- Ongoing work to strengthen the modular backend architecture across bounded contexts.

### Changed
- Changelog structure aligned with project-level release tracking best practices.
- Release documentation is maintained as a living record of the repository's evolution.

### Fixed
- No fixes recorded yet for this unreleased cycle.

## [0.0.1] - 2026-11-05

### Added
- Initial Prothia backend API scaffold built on .NET 10.
- ASP.NET Core application configuration with controller registration, lowercase URL routing, and kebab-case endpoint naming.
- OpenAPI documentation support via Microsoft ASP.NET Core OpenAPI, with Scalar UI integration for API exploration.
- Modular backend structure organized by bounded contexts, including:
  - Analytics
  - Billing
  - Patients
  - Monitoring
  - Shared infrastructure
- Domain-driven design foundation with application services, domain aggregates, value objects, repositories, and infrastructure layers.
- Persistence and data-access foundation using EF Core with MySQL integration.
- Shared middleware and configuration extensions for API bootstrap and common behavior.
- Global error-handling and problem-details infrastructure for consistent API responses.
- Resource localization support in English and Spanish for shared and domain-specific messaging.
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
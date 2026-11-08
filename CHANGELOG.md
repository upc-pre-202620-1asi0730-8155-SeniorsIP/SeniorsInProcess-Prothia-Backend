# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and
[Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Planned work to harden input validation, security boundaries, and persistence workflows.
- Roadmap items for expanding domain capabilities and stabilizing shared infrastructure.

### Changed
- Kept release tracking aligned with repository-wide documentation best practices.

### Fixed
- None recorded.

## [0.1.5] - 2026-11-08

### Added
- Communication bounded context with domain entities, application services, repositories, and API endpoints.
- Release metadata aligned to the 0.1.5 project version.
- Changelog entries updated to reflect the current backend release milestone.

### Changed
- Updated project documentation and release tracking to reflect the Communication bounded context integration.
- Kept the project documentation synchronized with the active 0.1.5 release branch.
- Standardized release-note ordering and wording to match Keep a Changelog conventions.

### Fixed
- No functional regressions were recorded for this maintenance release.

### Security
- No security-related changes were introduced in this release.

### Deprecated
- No deprecated APIs or features were recorded for this release.

## [0.1.4] - 2026-11-07

### Added
- Release metadata aligned to the 0.1.4 project version.
- Documentation updated to reflect the current backend release state.

### Changed
- Updated project version tracking and changelog ordering to match the active solution version.
- Kept release notes aligned with Keep a Changelog best practices.

### Fixed
- No functional fixes were recorded for this maintenance release.

### Security
- No security-related changes were introduced in this release.

### Deprecated
- No deprecated APIs or features were recorded for this release.

## [0.1.3] - 2026-11-06

### Added
- Prescriptions bounded context with aggregate roots, commands, queries, repositories, and REST API endpoints.
- Domain and infrastructure support for patient-related workflows, clinical reporting, and billing operations across the backend.
- Additional resource and transformation layers for localized API responses and action-result assembly.
- Project-level release documentation aligned with Keep a Changelog conventions for the active backend iteration.

### Changed
- Consolidated the backend release notes to reflect the current modular DDD architecture more accurately.
- Standardized the changelog structure and wording for versioned project documentation.
- Kept platform configuration and runtime metadata aligned with the .NET 10 solution baseline.

### Fixed
- Corrected version documentation inconsistencies between the project release notes and the current solution metadata.

### Security
- No authentication, authorization, or explicit security-hardening changes were introduced in this patch-level update.

### Deprecated
- No deprecated APIs or features were recorded for this release.

## [0.1.2] - 2026-11-06

### Added
- ASP.NET Core API foundation with controller registration, lowercase routing, and kebab-case endpoint naming conventions.
- OpenAPI support through `Microsoft.AspNetCore.OpenApi`, plus Scalar UI for local API exploration.
- DDD-oriented modular structure covering analytics, billing, patients, prescriptions, and shared infrastructure.
- Shared application, infrastructure, and repository patterns for service composition and extensibility.
- EF Core and MySQL persistence scaffolding for future domain and data-access work.
- Resource localization support for common and domain-specific messages in English and Spanish.
- Centralized error handling and problem-details responses for consistent API behavior.
- Solution and project metadata configuration in `prothia.sln`, `global.json`, and `Prothia.API/Prothia.API.csproj`.

### Changed
- Standardized runtime and SDK configuration around .NET 10 for consistent builds across environments.
- Organized the solution around modular, bounded-context service boundaries to support incremental feature delivery.
- Clarified API bootstrap and dependency conventions for routing, documentation, and persistence setup.
- Updated changelog formatting to align with Keep a Changelog best practices.

### Fixed
- None required for this maintenance release.

### Security
- No explicit authentication, authorization, or security-hardening work was introduced in this release.

### Deprecated
- No deprecated APIs or features were recorded for this release.

## [0.0.1] - 2026-11-05

### Added
- Initial Prothia backend API scaffold built on .NET 10.
- Initial ASP.NET Core configuration with controller registration, lowercase URL routing, and kebab-case endpoint naming.
- Initial OpenAPI documentation setup with Scalar UI integration for API exploration.
- Initial modular layout grouped by bounded contexts, including Analytics, Billing, Patients, Monitoring, and Shared infrastructure.
- Initial DDD foundation with application services, domain aggregates, value objects, repositories, and infrastructure layers.
- Initial persistence and data-access foundation using EF Core with MySQL integration.
- Initial shared middleware and extension-based configuration for API bootstrap and common behavior.
- Initial global error-handling and problem-details infrastructure for consistent API responses.
- Initial resource localization support for English and Spanish messages.
- Initial solution and project metadata configuration in `prothia.sln`, `global.json`, and `Prothia.API/Prothia.API.csproj`.

### Changed
- Standardized the SDK and runtime configuration with `global.json` for a consistent .NET 10 build experience.
- Organized the codebase around modular, DDD-flavored service boundaries to support incremental feature delivery.
- Adopted consistent interface and configuration conventions for routing, OpenAPI, and persistence setup.

### Fixed
- No production fixes were recorded for the initial release milestone.

### Security
- No explicit security hardening or authentication changes were recorded in this initial release.

### Deprecated
- No deprecated APIs or features were recorded in this initial release.
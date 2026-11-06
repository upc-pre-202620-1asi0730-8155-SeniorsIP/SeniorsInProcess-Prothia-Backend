# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and
[Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Baseline for upcoming backend improvements and feature releases.
- Planned work to expand and stabilize the modular domain architecture.

### Changed
- Documentation structure to be maintained using a project-level changelog.
- Release tracking to follow a consistent versioning and maintenance workflow.

### Fixed
- No fixes recorded yet for this unreleased cycle.

---

## [0.0.1] - 2026-11-05

### Added
- Initial backend project scaffold for Prothia API using .NET 10.
- ASP.NET Core web application setup with routing conventions and controller registration.
- OpenAPI support enabled via Microsoft ASP.NET Core OpenAPI and Scalar integration.
- Modular architecture foundation for:
    - Analytics
    - Billing
    - Patients
    - Monitoring
    - Shared infrastructure
- Domain-driven design-inspired structure with:
    - Application services
    - Domain model folders
    - Repositories
    - Infrastructure persistence layers
    - Resources and localized message files
- Support for EF Core and MySQL Entity Framework Core integration.
- Common middleware and configuration extensions for API bootstrapping.
- Resource localization entries in English and Spanish for common and domain-level messages.
- Initial package configuration and project metadata in `Prothia.API/Prothia.API.csproj`.

### Changed
- Standardized project SDK to .NET 10 with `global.json` pinning and roll-forward configuration.
- Configured lowercase URL routing and kebab-case route naming conventions for API endpoints.
- Organized project by bounded contexts to align with DDD-friendly service boundaries.

### Fixed
- No production fixes recorded for the initial release milestone.

### Security
- No security-specific changes recorded in this initial version.

### Deprecated
- No deprecated APIs or features recorded yet.

---

## Notes

This changelog reflects the current project state as defined by the repository structure and the initial configuration in:
- `Prothia.API/Program.cs`
- `Prothia.API/Prothia.API.csproj`
- `global.json`

If you want, I can also tailor the changelog to a more formal enterprise style or a more conventional release style for an MVP.
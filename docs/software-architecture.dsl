workspace "Prothia Platform - Vue.js / ASP.NET Core Stack Edition" {
    model {
        physiotherapist = person "Physiotherapist" "Clinical professional who monitors gait biomechanics, prescribes rehabilitation regimens, and reviews postural alerts."
        technician = person "Orthopedic Technician" "Workshop specialist who manages prosthetic hardware fleet, monitors cycle fatigue, and conducts preventive maintenance."
        patientUser = person "Amputee Patient" "Rehabilitation patient who follows prescribed daily exercises and tracks recovery progress."

        softwareSystem = softwareSystem "Prothia Platform" "Biomechanical telemetry and clinical management platform for lower-limb prosthetic rehabilitation." {
            webApplication = container "Prothia Web Application" "Delivers static web assets and bundles to client browsers." {
                tags "webapp"
                technology "HTML, CSS, JavaScript"
            }

            singlePageApplication = container "Prothia Single Page Application" "Provides interactive clinical dashboards, patient portal, and workshop fleet management." {
                tags "spa"
                technology "JavaScript, Vue 3, PrimeVue 4, Vue-i18n, Axios, Pinia, Vue-Router"
            }

            apiApplication = container "Prothia Platform API Application" "Exposes domain-driven RESTful endpoints under /api/v1." {
                tags "api"
                technology "C#, ASP.NET Core (.NET 10), Entity Framework Core 10"

                patients = component "Patients Bounded Context" "Manages amputee demographic profiles, amputation levels, and K-Levels." "ASP.NET Core Component"
                prescriptions = component "Prescriptions Bounded Context" "Manages rehabilitation exercise libraries, treatment phases, and routines." "ASP.NET Core Component"
                monitoring = component "Monitoring Bounded Context" "Captures gait telemetry sessions, evaluates symmetry, and triggers clinical alerts." "ASP.NET Core Component"
                workshop = component "Workshop Bounded Context" "Monitors prosthesis fleet inventory, mechanical fatigue limits, and maintenance." "ASP.NET Core Component"
                communication = component "Communication Bounded Context" "Transmits clinical case files to workshops and manages messaging." "ASP.NET Core Component"
                analytics = component "Analytics Bounded Context" "Compiles longitudinal clinical reports and exports gait kinematic datasets." "ASP.NET Core Component"
                billing = component "Billing Bounded Context" "Oversees clinic subscription tiers, patient quotas, and electronic invoices." "ASP.NET Core Component"
                iam = component "IAM Bounded Context" "Provides user authentication, BCrypt hashing, and JWT token issuance." "ASP.NET Core Component"
                shared = component "Shared Bounded Context" "Provides cross-cutting base repositories, Unit of Work, and AppDbContext." "ASP.NET Core Component"
            }

            database = container "Database" "Stores all relational entities, telemetry records, and credentials." {
                tags "database"
                technology "MySQL Server 8.0"
            }
        }

        # People to Containers
        physiotherapist -> webApplication "Visits clinical portal using" "HTTPS"
        technician -> webApplication "Visits workshop management portal using" "HTTPS"
        patientUser -> webApplication "Visits patient rehabilitation portal using" "HTTPS"

        webApplication -> singlePageApplication "Delivers to browser"
        singlePageApplication -> apiApplication "Consumes RESTful endpoints via" "JSON/HTTPS"

        # SPA to Components
        singlePageApplication -> patients "Registers and queries amputee patient profiles"
        singlePageApplication -> prescriptions "Prescribes therapy regimens and queries exercise library"
        singlePageApplication -> monitoring "Calibrates thresholds, resolves alerts, and streams gait sessions"
        singlePageApplication -> workshop "Tracks prosthesis fleet status and schedules maintenance"
        singlePageApplication -> communication "Transmits case summaries and coordinates with technicians"
        singlePageApplication -> analytics "Generates clinical reports and exports CSV series"
        singlePageApplication -> billing "Manages subscriptions and retrieves electronic invoices"
        singlePageApplication -> iam "Signs up and signs in to obtain JWT bearer tokens"

        # Inter-Context Dependencies (ACL & Facades)
        prescriptions -> patients "Validates patient existence via ExternalPatientService (ACL)"
        analytics -> patients "Queries patient demographic data for official clinical reports"
        communication -> patients "Links transmitted case files to registered amputee identifiers"
        workshop -> patients "Associates manufactured prostheses with recipient patients"

        # Contexts to Database
        patients -> database "Reads from and writes to" "EF Core / Pomelo MySQL"
        prescriptions -> database "Reads from and writes to" "EF Core / Pomelo MySQL"
        monitoring -> database "Reads from and writes to" "EF Core / Pomelo MySQL"
        workshop -> database "Reads from and writes to" "EF Core / Pomelo MySQL"
        communication -> database "Reads from and writes to" "EF Core / Pomelo MySQL"
        analytics -> database "Reads from and writes to" "EF Core / Pomelo MySQL"
        billing -> database "Reads from and writes to" "EF Core / Pomelo MySQL"
        iam -> database "Reads from and writes to" "EF Core / Pomelo MySQL"
        shared -> database "Configures schema mappings, migrations, and DbContext" "EF Core / Pomelo MySQL"

        # Shared dependencies
        patients -> shared "Extends BaseRepository, UnitOfWork, AggregateRoot"
        prescriptions -> shared "Extends BaseRepository, UnitOfWork, AggregateRoot"
        monitoring -> shared "Extends BaseRepository, UnitOfWork, AggregateRoot"
        workshop -> shared "Extends BaseRepository, UnitOfWork, AggregateRoot"
        communication -> shared "Extends BaseRepository, UnitOfWork, AggregateRoot"
        analytics -> shared "Extends BaseRepository, UnitOfWork, AggregateRoot"
        billing -> shared "Extends BaseRepository, UnitOfWork, AggregateRoot"
        iam -> shared "Extends BaseRepository, UnitOfWork, AggregateRoot"
    }

    views {
        systemContext softwareSystem "SystemContext" {
            include *
            autoLayout
        }

        container softwareSystem "Containers" {
            include *
            autoLayout
        }

        component apiApplication "Components" {
            include *
            autoLayout
        }

        theme default

        styles {
            element "person" {
                shape Person
                background #08427B
                color #ffffff
            }
            element "webapp" {
                shape Box
                background #1168BD
                color #ffffff
            }
            element "spa" {
                shape WebBrowser
                background #00BB7A
                color #ffffff
            }
            element "api" {
                shape Box
                background #85BBF0
                color #0f172a
            }
            element "database" {
                shape Cylinder
                background #438DD5
                color #ffffff
            }
        }
    }
}

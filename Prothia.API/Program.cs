using Cortex.Mediator.Commands;
using Cortex.Mediator.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Scalar.AspNetCore;

// Shared
using Prothia.API.Shared.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Mediator.Cortex.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Prothia.API.Shared.Infrastructure.Pipeline.Middleware.Components;
using Prothia.API.Shared.Interfaces.Asp.Configuration;
using Prothia.API.Shared.Interfaces.Asp.OpenApi;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources;
using Prothia.API.Shared.Resources.Errors;

// Analytics
using Prothia.API.Analytics.Application.CommandServices;
using Prothia.API.Analytics.Application.Internal.CommandServices;
using Prothia.API.Analytics.Application.Internal.QueryServices;
using Prothia.API.Analytics.Application.QueryServices;
using Prothia.API.Analytics.Domain.Repositories;
using Prothia.API.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Prothia.API.Analytics.Resources;
using AnalyticsExternalPatientService = Prothia.API.Analytics.Application.Internal.OutboundServices.ACL.ExternalPatientService;

// Billing
using Prothia.API.Billing.Application.CommandServices;
using Prothia.API.Billing.Application.Internal.CommandServices;
using Prothia.API.Billing.Application.Internal.QueryServices;
using Prothia.API.Billing.Application.QueryServices;
using Prothia.API.Billing.Domain.Repositories;
using Prothia.API.Billing.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Prothia.API.Billing.Resources;

// Communication
using Prothia.API.Communication.Application.CommandServices;
using Prothia.API.Communication.Application.Internal.CommandServices;
using Prothia.API.Communication.Application.Internal.QueryServices;
using Prothia.API.Communication.Application.QueryServices;
using Prothia.API.Communication.Domain.Repositories;
using Prothia.API.Communication.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Prothia.API.Communication.Resources;
using CommunicationExternalPatientService = Prothia.API.Communication.Application.Internal.OutboundServices.ACL.ExternalPatientService;

// Monitoring
using Prothia.API.Monitoring.Application.CommandServices;
using Prothia.API.Monitoring.Application.Internal.CommandServices;
using Prothia.API.Monitoring.Application.Internal.QueryServices;
using Prothia.API.Monitoring.Application.QueryServices;
using Prothia.API.Monitoring.Domain.Repositories;
using Prothia.API.Monitoring.Infrastructure.Repositories;
using Prothia.API.Monitoring.Resources;
using MonitoringExternalPatientService = Prothia.API.Monitoring.Application.Internal.Acl.ExternalPatientService;

// Patients
using Prothia.API.Patients.Application.Acl;
using Prothia.API.Patients.Application.CommandServices;
using Prothia.API.Patients.Application.Internal.CommandServices;
using Prothia.API.Patients.Application.Internal.QueryServices;
using Prothia.API.Patients.Application.QueryServices;
using Prothia.API.Patients.Domain.Repositories;
using Prothia.API.Patients.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Prothia.API.Patients.Interfaces.ACL;
using Prothia.API.Patients.Resources;

// Prescriptions
using Prothia.API.Prescriptions.Application.CommandServices;
using Prothia.API.Prescriptions.Application.Internal.CommandServices;
using Prothia.API.Prescriptions.Application.Internal.QueryServices;
using Prothia.API.Prescriptions.Application.QueryServices;
using Prothia.API.Prescriptions.Domain.Repositories;
using Prothia.API.Prescriptions.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Prothia.API.Prescriptions.Resources;
using PrescriptionsExternalPatientService = Prothia.API.Prescriptions.Application.Internal.OutboundServices.ACL.ExternalPatientService;

// Workshop
using Prothia.API.Workshop.Application.CommandServices;
using Prothia.API.Workshop.Application.Internal.CommandServices;
using Prothia.API.Workshop.Application.Internal.QueryServices;
using Prothia.API.Workshop.Application.QueryServices;
using Prothia.API.Workshop.Domain.Repositories;
using Prothia.API.Workshop.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Prothia.API.Workshop.Resources;

var builder = WebApplication.CreateBuilder(args);

// Add routing and lowercase conventions
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options =>
    {
        options.Conventions.Add(new KebabCaseRouteNamingConvention());
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    })
    .AddDataAnnotationsLocalization();

// Add ProblemDetails and ExceptionHandler services
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy", policy =>
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configure Database Context (MySQL)
builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrWhiteSpace(connectionStringTemplate))
        throw new InvalidOperationException("Database connection string is not set in configuration.");

    var connectionString = Environment.ExpandEnvironmentVariables(connectionStringTemplate);
    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Database connection string is not set in configuration.");

    options.UseMySQL(connectionString)
        .UseLoggerFactory(serviceProvider.GetRequiredService<ILoggerFactory>())
        .AddInterceptors(serviceProvider.GetRequiredService<DispatchDomainEventsInterceptor>())
        .EnableDetailedErrors();

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

// Localization
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddSingleton<IStringLocalizer<ErrorMessages>, StringLocalizer<ErrorMessages>>();
builder.Services.AddSingleton<IStringLocalizer<CommonMessages>, StringLocalizer<CommonMessages>>();
builder.Services.AddSingleton<IStringLocalizer<AnalyticsMessages>, StringLocalizer<AnalyticsMessages>>();
builder.Services.AddSingleton<IStringLocalizer<BillingMessages>, StringLocalizer<BillingMessages>>();
builder.Services.AddSingleton<IStringLocalizer<CommunicationMessages>, StringLocalizer<CommunicationMessages>>();
builder.Services.AddSingleton<IStringLocalizer<MonitoringMessages>, StringLocalizer<MonitoringMessages>>();
builder.Services.AddSingleton<IStringLocalizer<PatientsMessages>, StringLocalizer<PatientsMessages>>();
builder.Services.AddSingleton<IStringLocalizer<PrescriptionsMessages>, StringLocalizer<PrescriptionsMessages>>();
builder.Services.AddSingleton<IStringLocalizer<WorkshopMessages>, StringLocalizer<WorkshopMessages>>();

// ProblemDetailsFactory
builder.Services.AddSingleton<ProblemDetailsFactory>();

// OpenAPI / Scalar Documentation
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    options.AddOperationTransformer<IamBearerSecurityOperationTransformer>();
});

// Mediator Behaviors & Cortex.Mediator
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(LoggingCommandBehavior<>));
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(ExceptionHandlingCommandBehavior<>));
builder.Services.AddScoped(typeof(ICommandPipelineBehavior<>), typeof(TransactionCommandBehavior<>));
builder.Services.AddCortexMediator([typeof(Program)]);

// Dependency Injection - Shared
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<DispatchDomainEventsInterceptor>();

// Dependency Injection - Patients
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientCommandService, PatientCommandService>();
builder.Services.AddScoped<IPatientQueryService, PatientQueryService>();
builder.Services.AddScoped<IPatientsContextFacade, Prothia.API.Patients.Application.Acl.PatientsContextFacade>();

// Dependency Injection - Analytics
builder.Services.AddScoped<IClinicalReportRepository, ClinicalReportRepository>();
builder.Services.AddScoped<IClinicalReportCommandService, ClinicalReportCommandService>();
builder.Services.AddScoped<IClinicalReportQueryService, ClinicalReportQueryService>();
builder.Services.AddScoped<AnalyticsExternalPatientService>();

// Dependency Injection - Billing
builder.Services.AddScoped<IClinicSubscriptionRepository, ClinicSubscriptionRepository>();
builder.Services.AddScoped<IElectronicInvoiceRepository, ElectronicInvoiceRepository>();
builder.Services.AddScoped<IClinicSubscriptionCommandService, ClinicSubscriptionCommandService>();
builder.Services.AddScoped<IElectronicInvoiceCommandService, ElectronicInvoiceCommandService>();
builder.Services.AddScoped<IClinicSubscriptionQueryService, ClinicSubscriptionQueryService>();
builder.Services.AddScoped<IElectronicInvoiceQueryService, ElectronicInvoiceQueryService>();

// Dependency Injection - Communication
builder.Services.AddScoped<IClinicalMessageRepository, ClinicalMessageRepository>();
builder.Services.AddScoped<IClinicalTransmissionRepository, ClinicalTransmissionRepository>();
builder.Services.AddScoped<IClinicalMessageCommandService, ClinicalMessageCommandService>();
builder.Services.AddScoped<IClinicalTransmissionCommandService, ClinicalTransmissionCommandService>();
builder.Services.AddScoped<IClinicalMessageQueryService, ClinicalMessageQueryService>();
builder.Services.AddScoped<IClinicalTransmissionQueryService, ClinicalTransmissionQueryService>();
builder.Services.AddScoped<CommunicationExternalPatientService>();

// Dependency Injection - Monitoring
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IGaitSessionRepository, GaitSessionRepository>();
builder.Services.AddScoped<IThresholdProfileRepository, ThresholdProfileRepository>();
builder.Services.AddScoped<IAlertCommandService, AlertCommandService>();
builder.Services.AddScoped<IGaitSessionCommandService, GaitSessionCommandService>();
builder.Services.AddScoped<IThresholdProfileCommandService, ThresholdProfileCommandService>();
builder.Services.AddScoped<IAlertQueryService, AlertQueryService>();
builder.Services.AddScoped<IGaitSessionQueryService, GaitSessionQueryService>();
builder.Services.AddScoped<IThresholdProfileQueryService, ThresholdProfileQueryService>();
builder.Services.AddScoped<MonitoringExternalPatientService>();

// Dependency Injection - Prescriptions
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IExerciseCommandService, ExerciseCommandService>();
builder.Services.AddScoped<IPrescriptionCommandService, PrescriptionCommandService>();
builder.Services.AddScoped<IExerciseQueryService, ExerciseQueryService>();
builder.Services.AddScoped<IPrescriptionQueryService, PrescriptionQueryService>();
builder.Services.AddScoped<PrescriptionsExternalPatientService>();

// Dependency Injection - Workshop
builder.Services.AddScoped<IMaintenanceScheduleRepository, MaintenanceScheduleRepository>();
builder.Services.AddScoped<IMechanicalAlertRepository, MechanicalAlertRepository>();
builder.Services.AddScoped<IProsthesisRepository, ProsthesisRepository>();
builder.Services.AddScoped<IMaintenanceScheduleCommandService, MaintenanceScheduleCommandService>();
builder.Services.AddScoped<IMechanicalAlertCommandService, MechanicalAlertCommandService>();
builder.Services.AddScoped<IProsthesisCommandService, ProsthesisCommandService>();
builder.Services.AddScoped<IMaintenanceScheduleQueryService, MaintenanceScheduleQueryService>();
builder.Services.AddScoped<IMechanicalAlertQueryService, MechanicalAlertQueryService>();
builder.Services.AddScoped<IProsthesisQueryService, ProsthesisQueryService>();

var app = builder.Build();

// Apply pending migrations and seed database on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        EnsureLocalMySqlRunning(logger);
        var context = services.GetRequiredService<AppDbContext>();
        try
        {
            context.Database.Migrate();
        }
        catch (MySql.Data.MySqlClient.MySqlException ex) when (ex.Number == 1050)
        {
            context.Database.ExecuteSqlRaw("""
                CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
                    `MigrationId` varchar(150) NOT NULL,
                    `ProductVersion` varchar(32) NOT NULL,
                    PRIMARY KEY (`MigrationId`)
                );
                INSERT IGNORE INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
                VALUES ('20261110210229_InitialCreate', '10.0.13');
                """);
            context.Database.Migrate();
        }
        SeedDatabaseIfEmpty(context, logger);
        logger.LogInformation("Database 'prothia-db' migrations applied and verified successfully.");
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "Could not initialize database on startup. Ensure MySQL is running and connection string is configured.");
    }
}

// Exception Handler
app.UseExceptionHandler();

// Localization
var supportedCultures = new[] { "en", "es" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// OpenAPI & Scalar Documentation in Development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGet("/", () => Results.Redirect("/scalar/v1"));
}

app.UseCors("AllowAllPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

// Navigation Options endpoint for Frontend sidebar
app.MapGet("/api/v1/options", () => Results.Ok(new object[]
{
    new { id = 1, label = "option.dashboard", to = "/home", icon = "pi-th-large", exact = true, badge = 0, order = 1 },
    new { id = 2, label = "option.patients", to = "/patients", icon = "pi-users", exact = false, badge = 0, order = 2 },
    new { id = 3, label = "option.prescriptions", to = "/prescriptions", icon = "pi-clipboard", exact = false, badge = 0, order = 3 },
    new { id = 4, label = "option.alerts", to = "/alerts", icon = "pi-exclamation-triangle", exact = false, badge = 3, order = 4 },
    new { id = 5, label = "option.workshops", to = "/workshops", icon = "pi-arrow-right-arrow-left", exact = false, badge = 0, order = 5 },
    new { id = 6, label = "option.reports", to = "/reports", icon = "pi-file", exact = false, badge = 0, order = 6 },
    new { id = 7, label = "option.subscription", to = "/subscription", icon = "pi-credit-card", exact = false, badge = 0, order = 7 }
}));

app.MapControllers();

app.Run();

static void SeedDatabaseIfEmpty(AppDbContext context, ILogger logger)
{
    if (context.Set<Prothia.API.Patients.Domain.Model.Aggregates.Patient>().Any()) return;

    logger.LogInformation("Seeding initial clinical dataset into 'prothia-db'...");
    context.Database.ExecuteSqlRaw("""
        INSERT IGNORE INTO patients (id, full_name, dni, age, e_amputation, ek_level, prosthesis_code, adherence, symmetry, status, created_at, updated_at) VALUES
        (1, 'Carlos Mendoza Arias', '45892104', 38, 'TranstibialRight', 'K3', 'PR-2026-TT-084', 88, 92, 'Alert', NOW(), NOW()),
        (2, 'Roberto Sánchez P.', '40129845', 45, 'TransfemoralLeft', 'K2', 'PR-2026-TF-012', 72, 81, 'Critical', NOW(), NOW()),
        (3, 'María Vega Castro', '10845921', 52, 'TranstibialBilateral', 'K3', 'PR-2026-TT-055', 95, 96, 'Stable', NOW(), NOW()),
        (4, 'Jorge Alarcón Ruiz', '72458912', 29, 'TranstibialLeft', 'K4', 'PR-2026-TT-091', 91, 94, 'Stable', NOW(), NOW());

        INSERT IGNORE INTO exercises (id, name, focus, created_at, updated_at) VALUES
        (1, 'Transferencia de Peso Lateral', 'Equilibrio|Carga progresiva en muñón', NOW(), NOW()),
        (2, 'Elevación de Talón Asistida', 'Fuerza|Tríceps sural y estabilidad', NOW(), NOW()),
        (3, 'Puente de Glúteos con Prótesis', 'Extensión de cadera|Anti-compensación lumbar', NOW(), NOW()),
        (4, 'Subida de Escalón (Step-up) 10 cm', 'Coordinación|Potencia cuádriceps', NOW(), NOW());

        INSERT IGNORE INTO prescriptions (id, patient_id, phase_number, phase_name, duration_weeks, frequency_per_week, created_at, updated_at) VALUES
        (1, 1, 2, 'Ajuste', 4, 5, NOW(), NOW());

        INSERT IGNORE INTO prescribed_exercises (id, prescription_id, exercise_id, name, sets, reps, rest_seconds, created_at, updated_at) VALUES
        (1, 1, 1, 'Transferencia de Peso Lateral y Apoyo Unipodal', 3, 12, 45, NOW(), NOW()),
        (2, 1, 2, 'Elevación de Talón Asistida en Barra', 3, 10, 60, NOW(), NOW());

        INSERT IGNORE INTO alerts (id, patient_id, patient_name, deviation_type, recorded_value, threshold, severity, status, occurred_at, resolved_at) VALUES
        (1, 1, 'Carlos Mendoza Arias', 'Inclinación Pélvica Lateral', '+14°', '>12°', 'Warning', 'Pending', '2026-09-12 10:15:00', NULL),
        (2, 2, 'Roberto Sánchez P.', 'Pico de Carga en Talón', '1,150 N', '>950 N', 'Critical', 'Pending', '2026-09-11 18:20:00', NULL),
        (3, 4, 'Jorge Alarcón Ruiz', 'Asimetría Bilateral Brusca', '22%', '>15%', 'Warning', 'Resolved', '2026-09-10 11:00:00', '2026-09-10 15:30:00');

        INSERT IGNORE INTO threshold_profiles (id, patient_id, pelvic_tilt, asymmetry, cadence, impact_peak, created_at, updated_at) VALUES
        (1, 1, 19, 15, 102, 1.3, NOW(), NOW()),
        (2, 2, 10, 12, 60, 1.2, NOW(), NOW()),
        (3, 3, 12, 15, 80, 1.3, NOW(), NOW()),
        (4, 4, 14, 15, 90, 1.5, NOW(), NOW());

        INSERT IGNORE INTO gait_sessions (id, patient_id, recorded_at, duration_minutes, steps, cadence, symmetry_percent, alerts_count, impact_peak_newtons, created_at, updated_at) VALUES
        (1, 1, '2026-09-12 17:45:00', 24, 2340, 96, 93, 0, 610, NOW(), NOW()),
        (2, 1, '2026-09-12 09:20:00', 18, 1710, 95, 91, 1, 645, NOW(), NOW()),
        (3, 1, '2026-09-11 18:10:00', 30, 2820, 94, 92, 0, 615, NOW(), NOW()),
        (4, 1, '2026-09-10 16:30:00', 22, 1980, 90, 89, 2, 710, NOW(), NOW()),
        (5, 1, '2026-09-09 10:15:00', 25, 2200, 88, 90, 0, 625, NOW(), NOW());

        INSERT IGNORE INTO prostheses (id, serial_number, type, patient_name, clinic_name, accumulated_cycles, cycle_limit, last_service, status, battery_level, sensor_status, k_level, socket_type, knee_type, foot_type, created_at, updated_at) VALUES
        (1, 'PR-2026-TT-084', 'Transtibial Carbono (Vacío Activo)', 'Carlos Mendoza Arias', 'Rehab Lima Sur', 342100, 800000, '15/05/2026 (Encaje)', 'Optimal', 94, 'Online', 'K3', 'Fibra de Carbono Termoformada (Vacío Activo)', 'N/A (Transtibial)', 'Pie Dinámico de Carbono', NOW(), NOW()),
        (2, 'PR-2025-TF-004', 'Transfemoral Hidráulica', 'Hugo Paredes', 'Rehab Lima Sur', 814200, 800000, '10/01/2026 (Revisión)', 'Critical', 52, 'Online', 'K2', 'Encaje Cuadrilateral con liner de silicona', 'Rodilla Monocéntrica Hidráulica', 'Pie de carbono multieje', NOW(), NOW()),
        (3, 'PR-2026-TT-099', 'Transtibial Ensamblada', 'Disponible sin asignar', 'Taller Central', 0, 800000, 'En ensamblaje', 'Storage', 100, 'Online', 'K2', 'Socket modular termoplástico', 'N/A (Transtibial)', 'Pie Sach Estándar', NOW(), NOW()),
        (4, 'PR-2026-TF-012', 'Transfemoral Hidráulica', 'Roberto Sánchez P.', 'Rehab Lima Sur', 512400, 800000, '20/03/2026 (Alineación)', 'Warning', 68, 'Online', 'K2', 'Encaje Isquiático de Contacto Total', 'Rodilla Monocéntrica Hidráulica 3R80', 'Pie articulado SACH reforzado', NOW(), NOW()),
        (5, 'PR-2026-TT-055', 'Transtibial Bilateral Carbono', 'María Vega Castro', 'Rehab Lima Sur', 195000, 800000, '02/06/2026 (Alineación)', 'Optimal', 88, 'Online', 'K3', 'Socket de vacío activo Harmony', 'N/A (Transtibial)', 'Triton Carbon Foot', NOW(), NOW()),
        (6, 'PR-2026-TT-091', 'Transtibial Carbono', 'Jorge Alarcón Ruiz', 'Rehab Lima Sur', 280000, 800000, '12/04/2026 (Ajuste)', 'Optimal', 91, 'Online', 'K4', 'Liner con pin distal y lanzadera', 'N/A (Transtibial)', 'Vari-Flex XC con rotador', NOW(), NOW());

        INSERT IGNORE INTO mechanical_alerts (id, prosthesis_id, serial_number, cycles, cycle_limit, patient_name, k_level, component, description, severity, status, occurred_at, created_at, updated_at) VALUES
        (1, 2, 'PR-2025-TF-004', 814200, 800000, 'Hugo Paredes', 'K2', 'Amortiguador hidráulico de rodilla', 'El amortiguador hidráulico de rodilla superó el ciclo de vida recomendado de 800,000 ciclos. Riesgo de pérdida de fluido.', 'Critical', 'Pending', 'Hoy 08:30 AM', NOW(), NOW()),
        (2, 4, 'PR-2026-TF-012', 512400, 800000, 'Roberto Sánchez P.', 'K2', 'Elastómero de amortiguación', 'Desgaste preventivo del elastómero superando el 60% de vida proyectada. Recambio urgente recomendado.', 'Warning', 'Pending', 'Ayer 16:45 PM', NOW(), NOW());

        INSERT IGNORE INTO maintenance_schedules (id, serial_number, patient_name, scheduled_date, intervention_type, technician_name, status, notes, created_at, updated_at) VALUES
        (1, 'PR-2025-TF-004', 'Hugo Paredes', '07/10/2026', 'Recambio de amortiguador hidráulico', 'Ing. Roberto Valdivia', 'Scheduled', 'Recambio por superar 800k ciclos.', NOW(), NOW()),
        (2, 'PR-2026-TF-012', 'Roberto Sánchez P.', '08/10/2026', 'Inspección de elastómero y ajuste de pernos', 'Ing. Roberto Valdivia', 'Scheduled', 'Revisión preventiva de alineación dinámica.', NOW(), NOW()),
        (3, 'PR-2026-TT-084', 'Carlos Mendoza Arias', '09/10/2026', 'Chequeo de sensor telemétrico IMU', 'Ing. Roberto Valdivia', 'Scheduled', 'Verificación de batería y calibración de celda de carga.', NOW(), NOW()),
        (4, 'PR-2026-TT-055', 'María Vega Castro', '10/10/2026', 'Ajuste de vacío y válvula de suspensión', 'Ing. Roberto Valdivia', 'Scheduled', 'Revisión bimestral programada.', NOW(), NOW()),
        (5, 'PR-2026-TT-091', 'Jorge Alarcón Ruiz', '11/10/2026', 'Inspección cosmética y torque de pernos de titanio', 'Ing. Roberto Valdivia', 'Scheduled', 'Paciente K4 de alta actividad física.', NOW(), NOW());

        INSERT IGNORE INTO clinical_messages (id, sender, sender_name, sender_role, text, timestamp, is_system, status, created_at, updated_at) VALUES
        (1, 'User', 'Carlos Mendoza', 'Paciente', 'Hola Lic. Diego, completé la rutina de hoy. Noté una ligera presión en el borde lateral distal del encaje al minuto 20 de la caminata, pero sin dolor agudo. ¿Debo colocarme una capa extra de media o lo mantengo igual?', 'Ayer 18:20', 0, 'Delivered', NOW(), NOW()),
        (2, 'Therapist', 'Lic. Diego Salazar', 'Fisioterapeuta', '¡Hola Carlos! Revisé los datos de tu sesión telemétrica de ayer: tu simetría de apoyo estuvo en 94%, lo cual es un resultado excelente. Esa ligera presión es esperable mientras el muñón se tonifica. Mantén el mismo número de medias hoy y descansa 1 minuto extra entre series. Si mañana persiste enrojecimiento por más de 15 minutos al retirar el encaje, avísame para coordinar una alineación con el taller ortopédico.', 'Hoy 08:45', 0, 'Delivered', NOW(), NOW()),
        (3, 'System', 'Sistema Prothia', 'Sistema', 'El sistema compartió automáticamente tu sesión de hoy (3 de 4 ejercicios completados - 75%)', 'Hoy 09:30', 1, 'Delivered', NOW(), NOW());

        INSERT IGNORE INTO clinical_transmissions (id, patient_id, patient_name, patient_dni, workshop_name, shared_modules, technical_notes, status, status_label, summary, created_at_date, created_at, updated_at) VALUES
        (1, 1, 'Carlos Mendoza Arias', '45892104', 'Ortopedia Avanzada S.A.C. (Ing. Roberto Valdivia)', 'symmetry|impact_alerts|clinical_notes', 'Paciente Carlos Mendoza presenta leve compensación con inclinación lateral hacia el lado sano a partir del minuto 18 de marcha. Se recomienda revisar el ángulo estático del pilón o eventual recalibración de la flexión del tobillo en el siguiente mantenimiento preventivo.', 'Confirmed', 'Recibido y Confirmado por Taller', 'Informe de adaptación inicial y 300,000 ciclos acumulados.', '12/09/2026', NOW(), NOW());

        INSERT IGNORE INTO clinical_reports (id, report_code, issue_date, clinic_name, clinic_unit, patient_id, patient_name, dni, age, diagnosis, prosthesis_code, evaluation_period, summary, medical_recommendation, therapist_name, therapist_role, therapist_registry, created_at, updated_at) VALUES
        (1, 'EXP-CLIN-2026-084', '12/09/2026', 'CENTRO DE REHABILITACIÓN FÍSICA LIMA SUR', 'Unidad de Biomecánica y Rehabilitación Protésica • RUC 20608945123', 1, 'Carlos Mendoza Arias', '45892104', 38, 'Transtibial Derecho (K3)', 'PR-2026-TT-084', 'Último Mes (12/08/2026 - 12/09/2026)', 'Durante el período evaluado de 30 días, el paciente registró un total de 24 sesiones de caminata telemétrica y completó el 88% de los ejercicios domiciliarios prescritos. La simetría de apoyo bilateral promedio se situó en 92.4%, representando una mejoría estadísticamente significativa de +10.4% frente a la evaluación basal de encaje inicial.', 'Continuar con la Fase 2 de fortalecimiento de abductores. Citar a control conjunto con protesista en 60 días para inspección de alineación estática.', 'Lic. Diego Salazar Mendoza', 'Tecnólogo Médico - Fisioterapeuta', 'CTMP 14820', NOW(), NOW()),
        (2, 'EXP-CLIN-2026-085', '12/09/2026', 'CENTRO DE REHABILITACIÓN FÍSICA LIMA SUR', 'Unidad de Biomecánica y Rehabilitación Protésica • RUC 20608945123', 2, 'Roberto Sánchez P.', '40129845', 45, 'Transfemoral Izquierdo (K2)', 'PR-2026-TF-012', 'Último Mes (12/08/2026 - 12/09/2026)', 'Durante el período evaluado de 30 días, el paciente registró 18 sesiones telemétricas y 72% de adherencia. La simetría de apoyo bilateral se sitúa en 81.0%, requiriendo fortalecimiento de cadera contralateral y ajuste de alineación de rodilla hidráulica.', 'Coordinar ajuste de amortiguación hidráulica con el taller ortopédico y priorizar sesiones de fortalecimiento glúteo medio.', 'Lic. Diego Salazar Mendoza', 'Tecnólogo Médico - Fisioterapeuta', 'CTMP 14820', NOW(), NOW()),
        (3, 'EXP-CLIN-2026-086', '12/09/2026', 'CENTRO DE REHABILITACIÓN FÍSICA LIMA SUR', 'Unidad de Biomecánica y Rehabilitación Protésica • RUC 20608945123', 3, 'María Vega Castro', '10845921', 52, 'Transtibial Bilateral (K3)', 'PR-2026-TT-055', 'Último Mes (12/08/2026 - 12/09/2026)', 'Paciente con excelente adherencia domiciliaria (95%) y marcha fluida. La simetría bilateral de carga promedia 96.0%, superando el umbral clínico objetivo de bipedestación.', 'Mantener régimen de marcha y programar revisión de liners y suspensión de vacío en 90 días.', 'Lic. Diego Salazar Mendoza', 'Tecnólogo Médico - Fisioterapeuta', 'CTMP 14820', NOW(), NOW()),
        (4, 'EXP-CLIN-2026-087', '12/09/2026', 'CENTRO DE REHABILITACIÓN FÍSICA LIMA SUR', 'Unidad de Biomecánica y Rehabilitación Protésica • RUC 20608945123', 4, 'Jorge Alarcón Ruiz', '72458912', 29, 'Transtibial Izquierdo (K4)', 'PR-2026-TT-091', 'Último Mes (12/08/2026 - 12/09/2026)', 'Paciente de alto impacto K4 con 91% de cumplimiento en rutina de acondicionamiento aeróbico. Patrón cinemático de marcha de alta eficiencia biomecánica.', 'Habilitar transición a actividades deportivas de impacto moderado y control de alineación semestral.', 'Lic. Diego Salazar Mendoza', 'Tecnólogo Médico - Fisioterapeuta', 'CTMP 14820', NOW(), NOW());

        INSERT IGNORE INTO report_indicators (id, clinical_report_id, name, value, normal_range, evaluation, status, created_at, updated_at) VALUES
        (1, 1, 'Cadencia Media de Paso', '98 pasos/min', '90 - 105 pasos/min', 'Normal / Fluido', 'Normal', NOW(), NOW()),
        (2, 1, 'Simetría Bilateral de Carga', '92.4%', '> 90.0%', 'Excelente equilibrio', 'Normal', NOW(), NOW()),
        (3, 1, 'Ángulo de Flexión de Rodilla', '58°', '55° - 65°', 'Rango anatómico', 'Normal', NOW(), NOW()),
        (4, 1, 'Inclinación Lateral de Tronco', '14° (Máx. en fatiga)', '< 12°', 'En observación', 'Warning', NOW(), NOW()),
        (5, 2, 'Cadencia Media de Paso', '84 pasos/min', '90 - 105 pasos/min', 'En observación / Lento', 'Warning', NOW(), NOW()),
        (6, 2, 'Simetría Bilateral de Carga', '81.0%', '> 90.0%', 'Asimétrico / Compensatorio', 'Warning', NOW(), NOW()),
        (7, 2, 'Ángulo de Flexión de Rodilla', '48°', '55° - 65°', 'Rango limitado', 'Warning', NOW(), NOW()),
        (8, 2, 'Inclinación Lateral de Tronco', '16° (Máx. en fatiga)', '< 12°', 'Compensación severa', 'Warning', NOW(), NOW()),
        (9, 3, 'Cadencia Media de Paso', '94 pasos/min', '90 - 105 pasos/min', 'Normal / Fluido', 'Normal', NOW(), NOW()),
        (10, 3, 'Simetría Bilateral de Carga', '96.0%', '> 90.0%', 'Excelente equilibrio', 'Normal', NOW(), NOW()),
        (11, 3, 'Ángulo de Flexión de Rodilla', '60°', '55° - 65°', 'Rango anatómico', 'Normal', NOW(), NOW()),
        (12, 3, 'Inclinación Lateral de Tronco', '9°', '< 12°', 'Estable / Controlado', 'Normal', NOW(), NOW()),
        (13, 4, 'Cadencia Media de Paso', '104 pasos/min', '90 - 105 pasos/min', 'Dinámica / Alta cadencia', 'Normal', NOW(), NOW()),
        (14, 4, 'Simetría Bilateral de Carga', '94.0%', '> 90.0%', 'Excelente equilibrio', 'Normal', NOW(), NOW()),
        (15, 4, 'Ángulo de Flexión de Rodilla', '62°', '55° - 65°', 'Rango dinámico K4', 'Normal', NOW(), NOW()),
        (16, 4, 'Inclinación Lateral de Tronco', '10°', '< 12°', 'Estable', 'Normal', NOW(), NOW());

        INSERT IGNORE INTO clinic_subscriptions (id, plan_name, status, plan_type, annual_fee, monthly_equivalent, start_date, renewal_date, used_quota, total_quota, payment_method_brand, payment_method_card_title, payment_method_expiry, payment_method_auto_renew, created_at, updated_at) VALUES
        (1, 'Plan Centro de Rehabilitación', 'Activo • Vigente', 'Plan Clínico Anual', 1428.00, 119.00, '12 de Septiembre de 2026', '12 de Septiembre de 2027', 24, 50, 'VISA', 'Tarjeta Corporativa terminada en 4281', '08/29', 1, NOW(), NOW());

        INSERT IGNORE INTO electronic_invoices (id, invoice_number, issue_date, concept, amount, currency, status, status_label, download_url, created_at, updated_at) VALUES
        (1, 'F001-000842', '12/09/2026', 'Suscripción Anual Plataforma Prothia (50 Pacientes)', 1428.00, '$', 'Paid', 'Pagado', '/invoices/F001-000842.pdf', NOW(), NOW()),
        (2, 'F001-000619', '12/09/2025', 'Suscripción Anual Plataforma Prothia (50 Pacientes)', 1428.00, '$', 'Paid', 'Pagado', '/invoices/F001-000619.pdf', NOW(), NOW()),
        (3, 'F001-000405', '15/03/2025', 'Ampliación de Cupo Clínico (+20 Pacientes)', 480.00, '$', 'Paid', 'Pagado', '/invoices/F001-000405.pdf', NOW(), NOW());
        """);
}

static void EnsureLocalMySqlRunning(ILogger logger)
{
    static bool IsPortOpen(int port)
    {
        try
        {
            using var client = new System.Net.Sockets.TcpClient();
            var result = client.BeginConnect("127.0.0.1", port, null, null);
            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(500));
            return success && client.Connected;
        }
        catch
        {
            return false;
        }
    }

    if (IsPortOpen(3306)) return;

    const string mysqldPath = @"C:\Program Files\MySQL\MySQL Server 8.4\bin\mysqld.exe";
    if (!File.Exists(mysqldPath)) return;

    var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    var dataDir = Path.Combine(userProfile, "mysql-prothia-data");
    var initSqlFile = Path.Combine(userProfile, "mysql-prothia-init.sql");

    File.WriteAllText(initSqlFile, """
        ALTER USER 'root'@'localhost' IDENTIFIED BY 'Mysql123$';
        FLUSH PRIVILEGES;
        """);

    if (!Directory.Exists(dataDir) || !File.Exists(Path.Combine(dataDir, "ibdata1")))
    {
        if (Directory.Exists(dataDir)) Directory.Delete(dataDir, true);
        Directory.CreateDirectory(dataDir);

        logger.LogInformation("Initializing local MySQL 8.4 data directory at {DataDir}...", dataDir);
        var initProcess = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = mysqldPath,
            Arguments = $"--initialize-insecure --datadir=\"{dataDir}\"",
            UseShellExecute = false,
            CreateNoWindow = true
        });
        initProcess?.WaitForExit(30000);
    }

    logger.LogInformation("Starting local MySQL 8.4 Server on port 3306...");
    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
    {
        FileName = mysqldPath,
        Arguments = $"--datadir=\"{dataDir}\" --init-file=\"{initSqlFile}\" --port=3306",
        UseShellExecute = false,
        CreateNoWindow = true
    });

    for (var i = 0; i < 20; i++)
    {
        if (IsPortOpen(3306))
        {
            Thread.Sleep(1000);
            logger.LogInformation("Local MySQL 8.4 Server is online on port 3306.");
            return;
        }
        Thread.Sleep(500);
    }
}

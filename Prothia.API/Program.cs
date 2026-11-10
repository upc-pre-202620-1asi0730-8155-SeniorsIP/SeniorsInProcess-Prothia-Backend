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
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
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

// Ensure Database is created on startup if MySQL is available
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        EnsureLocalMySqlRunning(logger);
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
        logger.LogInformation("Database 'prothia-db' and all tables verified/created successfully.");
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

app.MapControllers();

app.Run();

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

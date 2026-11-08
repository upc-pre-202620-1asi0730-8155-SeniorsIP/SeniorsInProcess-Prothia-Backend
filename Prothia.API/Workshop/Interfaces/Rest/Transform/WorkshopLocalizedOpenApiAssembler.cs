using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Prothia.API.Shared.Interfaces.Asp.Configuration;
using Prothia.API.Workshop.Interfaces.Rest.Resources;

namespace Prothia.API.Workshop.Interfaces.Rest.Transform;

public static class WorkshopLocalizedOpenApiAssembler
{
    private static readonly IReadOnlyDictionary<string, OperationDocumentation> Operations =
        new Dictionary<string, OperationDocumentation>
        {
            ["CreateProsthesis"] = LocalizedOpenApiTransformers.Operation("CreateProsthesis",
                ("201", "Created"), ("400", "BadRequest"), ("409", "Conflict"), ("500", "ServerError")),
            ["GetProstheses"] = LocalizedOpenApiTransformers.Operation("GetProstheses",
                ("200", "Success")),
            ["GetProsthesisById"] = LocalizedOpenApiTransformers.Operation("GetProsthesisById",
                ("200", "Success"), ("404", "NotFound")),
            ["CreateMechanicalAlert"] = LocalizedOpenApiTransformers.Operation("CreateMechanicalAlert",
                ("201", "Created"), ("400", "BadRequest"), ("404", "NotFound"), ("500", "ServerError")),
            ["GetMechanicalAlerts"] = LocalizedOpenApiTransformers.Operation("GetMechanicalAlerts",
                ("200", "Success"), ("400", "BadRequest")),
            ["GetMechanicalAlertById"] = LocalizedOpenApiTransformers.Operation("GetMechanicalAlertById",
                ("200", "Success"), ("404", "NotFound")),
            ["ResolveMechanicalAlert"] = LocalizedOpenApiTransformers.Operation("ResolveMechanicalAlert",
                ("200", "Success"), ("404", "NotFound"), ("409", "Conflict"), ("500", "ServerError")),
            ["CreateMaintenanceSchedule"] = LocalizedOpenApiTransformers.Operation("CreateMaintenanceSchedule",
                ("201", "Created"), ("400", "BadRequest"), ("500", "ServerError")),
            ["GetMaintenanceSchedules"] = LocalizedOpenApiTransformers.Operation("GetMaintenanceSchedules",
                ("200", "Success")),
            ["GetMaintenanceScheduleById"] = LocalizedOpenApiTransformers.Operation("GetMaintenanceScheduleById",
                ("200", "Success"), ("404", "NotFound"))
        };

    private static readonly IReadOnlyDictionary<Type, string> Schemas = new Dictionary<Type, string>
    {
        [typeof(CreateProsthesisResource)] = "CreateProsthesisSchema",
        [typeof(ProsthesisResource)] = "ProsthesisSchema",
        [typeof(CreateMechanicalAlertResource)] = "CreateMechanicalAlertSchema",
        [typeof(MechanicalAlertResource)] = "MechanicalAlertSchema",
        [typeof(CreateMaintenanceScheduleResource)] = "CreateMaintenanceScheduleSchema",
        [typeof(MaintenanceScheduleResource)] = "MaintenanceScheduleSchema"
    };

    private static readonly IReadOnlyDictionary<(Type, string), string> Properties = BuildProperties();

    private static Dictionary<(Type, string), string> BuildProperties()
    {
        var properties = new Dictionary<(Type, string), string>();

        // Prostheses
        foreach (var type in new[] { typeof(ProsthesisResource), typeof(CreateProsthesisResource) })
        {
            properties[(type, "serialNumber")] = "ProsthesisSerialNumber";
            properties[(type, "type")] = "ProsthesisType";
            properties[(type, "patientName")] = "ProsthesisPatientName";
            properties[(type, "clinicName")] = "ProsthesisClinicName";
            properties[(type, "accumulatedCycles")] = "ProsthesisAccumulatedCycles";
            properties[(type, "cycleLimit")] = "ProsthesisCycleLimit";
            properties[(type, "lastService")] = "ProsthesisLastService";
            properties[(type, "status")] = "ProsthesisStatus";
            properties[(type, "batteryLevel")] = "ProsthesisBatteryLevel";
            properties[(type, "sensorStatus")] = "ProsthesisSensorStatus";
            properties[(type, "kLevel")] = "ProsthesisKLevel";
            properties[(type, "socketType")] = "ProsthesisSocketType";
            properties[(type, "kneeType")] = "ProsthesisKneeType";
            properties[(type, "footType")] = "ProsthesisFootType";
        }

        properties[(typeof(ProsthesisResource), "id")] = "ProsthesisId";

        // MechanicalAlerts
        foreach (var type in new[] { typeof(MechanicalAlertResource), typeof(CreateMechanicalAlertResource) })
        {
            properties[(type, "prosthesisId")] = "ProsthesisId";
            properties[(type, "serialNumber")] = "ProsthesisSerialNumber";
            properties[(type, "cycles")] = "ProsthesisAccumulatedCycles";
            properties[(type, "cycleLimit")] = "ProsthesisCycleLimit";
            properties[(type, "patientName")] = "ProsthesisPatientName";
            properties[(type, "kLevel")] = "ProsthesisKLevel";
            properties[(type, "component")] = "MechanicalAlertComponent";
            properties[(type, "description")] = "MechanicalAlertDescription";
            properties[(type, "severity")] = "MechanicalAlertSeverity";
            properties[(type, "occurredAt")] = "MechanicalAlertOccurredAt";
        }

        properties[(typeof(MechanicalAlertResource), "id")] = "MechanicalAlertId";
        properties[(typeof(MechanicalAlertResource), "status")] = "MechanicalAlertStatus";

        // MaintenanceSchedules
        foreach (var type in new[] { typeof(MaintenanceScheduleResource), typeof(CreateMaintenanceScheduleResource) })
        {
            properties[(type, "serialNumber")] = "ProsthesisSerialNumber";
            properties[(type, "patientName")] = "ProsthesisPatientName";
            properties[(type, "scheduledDate")] = "MaintenanceScheduledDate";
            properties[(type, "interventionType")] = "MaintenanceInterventionType";
            properties[(type, "technicianName")] = "MaintenanceTechnicianName";
            properties[(type, "status")] = "MaintenanceScheduleStatus";
            properties[(type, "notes")] = "MaintenanceNotes";
        }

        properties[(typeof(MaintenanceScheduleResource), "id")] = "MaintenanceScheduleId";

        return properties;
    }

    public static Task TransformOperationAsync(OpenApiOperation operation,
        OpenApiOperationTransformerContext context, CancellationToken cancellationToken) =>
        LocalizedOpenApiTransformers.TransformOperationAsync(operation, context, Operations);

    public static Task TransformSchemaAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken) =>
        LocalizedOpenApiTransformers.TransformSchemaAsync(schema, context, Schemas, Properties);
}

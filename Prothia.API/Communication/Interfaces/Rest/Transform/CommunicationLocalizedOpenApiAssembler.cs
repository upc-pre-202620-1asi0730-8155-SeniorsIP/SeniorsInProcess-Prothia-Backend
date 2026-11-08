using Prothia.API.Communication.Interfaces.Rest.Resources;
using Prothia.API.Shared.Interfaces.Asp.Configuration;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Prothia.API.Communication.Interfaces.Rest.Transform;

/// <summary>
///     Declares the localized OpenAPI metadata (summaries, descriptions and response texts)
///     of the Communication REST endpoints and resources.
/// </summary>
public static class CommunicationLocalizedOpenApiAssembler
{
    private static readonly IReadOnlyDictionary<string, OperationDocumentation> Operations =
        new Dictionary<string, OperationDocumentation>
        {
            ["CreateMessage"] = LocalizedOpenApiTransformers.Operation("CreateMessage",
                ("201", "Created"), ("400", "BadRequest"), ("500", "ServerError")),
            ["GetMessages"] = LocalizedOpenApiTransformers.Operation("GetMessages",
                ("200", "Success")),
            ["GetMessageById"] = LocalizedOpenApiTransformers.Operation("GetMessageById",
                ("200", "Success"), ("404", "NotFound")),
            ["CreateTransmission"] = LocalizedOpenApiTransformers.Operation("CreateTransmission",
                ("201", "Created"), ("400", "BadRequest"), ("404", "NotFound"), ("500", "ServerError")),
            ["GetTransmissions"] = LocalizedOpenApiTransformers.Operation("GetTransmissions",
                ("200", "Success"), ("400", "BadRequest")),
            ["GetTransmissionById"] = LocalizedOpenApiTransformers.Operation("GetTransmissionById",
                ("200", "Success"), ("404", "NotFound"))
        };

    private static readonly IReadOnlyDictionary<Type, string> Schemas = new Dictionary<Type, string>
    {
        [typeof(CreateClinicalMessageResource)] = "CreateClinicalMessageSchema",
        [typeof(ClinicalMessageResource)] = "ClinicalMessageSchema",
        [typeof(CreateClinicalTransmissionResource)] = "CreateClinicalTransmissionSchema",
        [typeof(ClinicalTransmissionResource)] = "ClinicalTransmissionSchema"
    };

    private static readonly IReadOnlyDictionary<(Type, string), string> Properties = BuildProperties();

    private static Dictionary<(Type, string), string> BuildProperties()
    {
        var properties = new Dictionary<(Type, string), string>();

        foreach (var type in new[] { typeof(ClinicalMessageResource), typeof(CreateClinicalMessageResource) })
        {
            properties[(type, "sender")] = "ClinicalMessageSender";
            properties[(type, "senderName")] = "ClinicalMessageSenderName";
            properties[(type, "senderRole")] = "ClinicalMessageSenderRole";
            properties[(type, "text")] = "ClinicalMessageText";
            properties[(type, "timestamp")] = "ClinicalMessageTimestamp";
            properties[(type, "isSystem")] = "ClinicalMessageIsSystem";
            properties[(type, "status")] = "ClinicalMessageStatus";
        }

        properties[(typeof(ClinicalMessageResource), "id")] = "ClinicalMessageId";

        foreach (var type in new[]
                     { typeof(ClinicalTransmissionResource), typeof(CreateClinicalTransmissionResource) })
        {
            properties[(type, "patientId")] = "CommunicationPatientId";
            properties[(type, "patientDni")] = "ClinicalTransmissionPatientDni";
            properties[(type, "workshopName")] = "ClinicalTransmissionWorkshopName";
            properties[(type, "sharedModules")] = "ClinicalTransmissionSharedModules";
            properties[(type, "technicalNotes")] = "ClinicalTransmissionTechnicalNotes";
            properties[(type, "summary")] = "ClinicalTransmissionSummary";
            properties[(type, "createdAtDate")] = "ClinicalTransmissionCreatedAtDate";
        }

        properties[(typeof(ClinicalTransmissionResource), "id")] = "ClinicalTransmissionId";
        properties[(typeof(ClinicalTransmissionResource), "patientName")] = "ClinicalTransmissionPatientName";
        properties[(typeof(ClinicalTransmissionResource), "status")] = "ClinicalTransmissionStatus";
        properties[(typeof(ClinicalTransmissionResource), "statusLabel")] = "ClinicalTransmissionStatusLabel";

        return properties;
    }

    public static Task TransformOperationAsync(OpenApiOperation operation,
        OpenApiOperationTransformerContext context, CancellationToken cancellationToken) =>
        LocalizedOpenApiTransformers.TransformOperationAsync(operation, context, Operations);

    public static Task TransformSchemaAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken) =>
        LocalizedOpenApiTransformers.TransformSchemaAsync(schema, context, Schemas, Properties);
}

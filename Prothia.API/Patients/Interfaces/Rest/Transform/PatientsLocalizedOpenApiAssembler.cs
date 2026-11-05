using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Prothia.API.Patients.Interfaces.Rest.Resources;

namespace Prothia.API.Patients.Interfaces.Rest.Transform;

public class PatientsLocalizedOpenApiAssembler
{
     private static readonly IReadOnlyDictionary<string, OperationDocumentation> Operations =
        new Dictionary<string, OperationDocumentation>
        {
            ["CreatePatient"] = LocalizedOpenApiTransformers.Operation("CreatePatient",
                ("201", "Created"), ("400", "BadRequest"), ("409", "Conflict"), ("500", "ServerError")),
            ["GetPatients"] = LocalizedOpenApiTransformers.Operation("GetPatients",
                ("200", "Success"), ("400", "BadRequest"), ("404", "NotFound")),
            ["GetPatientById"] = LocalizedOpenApiTransformers.Operation("GetPatientById",
                ("200", "Success"), ("404", "NotFound"))
        };

    private static readonly IReadOnlyDictionary<Type, string> Schemas = new Dictionary<Type, string>
    {
        [typeof(CreatePatientResource)] = "CreatePatientSchema",
        [typeof(PatientResource)] = "PatientSchema"
    };

    private static readonly IReadOnlyDictionary<(Type, string), string> Properties =
        new Dictionary<(Type, string), string>
        {
            [(typeof(PatientResource), "id")] = "PatientId",
            [(typeof(PatientResource), "fullName")] = "PatientFullName",
            [(typeof(PatientResource), "dni")] = "PatientDni",
            [(typeof(PatientResource), "age")] = "PatientAge",
            [(typeof(PatientResource), "amputation")] = "PatientAmputation",
            [(typeof(PatientResource), "kLevel")] = "PatientKLevel",
            [(typeof(PatientResource), "prosthesisCode")] = "PatientProsthesisCode",
            [(typeof(PatientResource), "adherence")] = "PatientAdherence",
            [(typeof(PatientResource), "symmetry")] = "PatientSymmetry",
            [(typeof(PatientResource), "status")] = "PatientStatus",
            [(typeof(CreatePatientResource), "fullName")] = "PatientFullName",
            [(typeof(CreatePatientResource), "dni")] = "PatientDni",
            [(typeof(CreatePatientResource), "age")] = "PatientAge",
            [(typeof(CreatePatientResource), "amputation")] = "PatientAmputation",
            [(typeof(CreatePatientResource), "kLevel")] = "PatientKLevel",
            [(typeof(CreatePatientResource), "prosthesisCode")] = "PatientProsthesisCode"
        };

    public static Task TransformOperationAsync(OpenApiOperation operation,
        OpenApiOperationTransformerContext context, CancellationToken cancellationToken) =>
        LocalizedOpenApiTransformers.TransformOperationAsync(operation, context, Operations);

    public static Task TransformSchemaAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken) =>
        LocalizedOpenApiTransformers.TransformSchemaAsync(schema, context, Schemas, Properties);
}
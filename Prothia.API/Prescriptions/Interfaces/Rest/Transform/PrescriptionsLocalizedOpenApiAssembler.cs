using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Prothia.API.Prescriptions.Interfaces.Rest.Resources;
using Prothia.API.Shared.Interfaces.Asp.Configuration;

namespace Prothia.API.Prescriptions.Interfaces.Rest.Transform;

/// <summary>
///     Declares the localized OpenAPI metadata (summaries, descriptions and response texts)
///     of the Prescriptions REST endpoints and resources.
/// </summary>
public static class PrescriptionsLocalizedOpenApiAssembler
{
    private static readonly IReadOnlyDictionary<string, OperationDocumentation> Operations =
        new Dictionary<string, OperationDocumentation>
        {
            ["CreateExercise"] = LocalizedOpenApiTransformers.Operation("CreateExercise",
                ("201", "Created"), ("400", "BadRequest"), ("409", "Conflict"), ("500", "ServerError")),
            ["GetExercises"] = LocalizedOpenApiTransformers.Operation("GetExercises",
                ("200", "Success")),
            ["GetExerciseById"] = LocalizedOpenApiTransformers.Operation("GetExerciseById",
                ("200", "Success"), ("404", "NotFound")),
            ["CreatePrescription"] = LocalizedOpenApiTransformers.Operation("CreatePrescription",
                ("201", "Created"), ("400", "BadRequest"), ("404", "NotFound"), ("500", "ServerError")),
            ["GetPrescriptions"] = LocalizedOpenApiTransformers.Operation("GetPrescriptions",
                ("200", "Success"), ("400", "BadRequest")),
            ["GetPrescriptionById"] = LocalizedOpenApiTransformers.Operation("GetPrescriptionById",
                ("200", "Success"), ("404", "NotFound")),
            ["UpdatePrescription"] = LocalizedOpenApiTransformers.Operation("UpdatePrescription",
                ("200", "Success"), ("400", "BadRequest"), ("404", "NotFound"), ("500", "ServerError"))
        };

    private static readonly IReadOnlyDictionary<Type, string> Schemas = new Dictionary<Type, string>
    {
        [typeof(CreateExerciseResource)] = "CreateExerciseSchema",
        [typeof(ExerciseResource)] = "ExerciseSchema",
        [typeof(PrescribedExerciseResource)] = "PrescribedExerciseSchema",
        [typeof(CreatePrescriptionResource)] = "CreatePrescriptionSchema",
        [typeof(UpdatePrescriptionResource)] = "UpdatePrescriptionSchema",
        [typeof(PrescriptionResource)] = "PrescriptionSchema"
    };

    private static readonly IReadOnlyDictionary<(Type, string), string> Properties = BuildProperties();

    private static Dictionary<(Type, string), string> BuildProperties()
    {
        var properties = new Dictionary<(Type, string), string>();

        // Exercises
        foreach (var type in new[] { typeof(ExerciseResource), typeof(CreateExerciseResource) })
        {
            properties[(type, "name")] = "ExerciseName";
            properties[(type, "focus")] = "ExerciseFocus";
        }

        properties[(typeof(ExerciseResource), "id")] = "ExerciseId";

        // PrescribedExerciseResource
        properties[(typeof(PrescribedExerciseResource), "exerciseId")] = "ExerciseId";
        properties[(typeof(PrescribedExerciseResource), "name")] = "ExerciseName";
        properties[(typeof(PrescribedExerciseResource), "sets")] = "PrescribedExerciseSets";
        properties[(typeof(PrescribedExerciseResource), "reps")] = "PrescribedExerciseReps";
        properties[(typeof(PrescribedExerciseResource), "restSeconds")] = "PrescribedExerciseRestSeconds";

        // Prescriptions
        foreach (var type in new[]
                 {
                     typeof(PrescriptionResource), typeof(CreatePrescriptionResource),
                     typeof(UpdatePrescriptionResource)
                 })
        {
            properties[(type, "phaseNumber")] = "PrescriptionPhaseNumber";
            properties[(type, "phaseName")] = "PrescriptionPhaseName";
            properties[(type, "durationWeeks")] = "PrescriptionDurationWeeks";
            properties[(type, "frequencyPerWeek")] = "PrescriptionFrequencyPerWeek";
            properties[(type, "exercises")] = "PrescriptionExercises";
        }

        properties[(typeof(PrescriptionResource), "id")] = "PrescriptionId";
        properties[(typeof(PrescriptionResource), "patientId")] = "PrescriptionsPatientId";
        properties[(typeof(CreatePrescriptionResource), "patientId")] = "PrescriptionsPatientId";

        return properties;
    }

    public static Task TransformOperationAsync(OpenApiOperation operation,
        OpenApiOperationTransformerContext context, CancellationToken cancellationToken) =>
        LocalizedOpenApiTransformers.TransformOperationAsync(operation, context, Operations);

    public static Task TransformSchemaAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken) =>
        LocalizedOpenApiTransformers.TransformSchemaAsync(schema, context, Schemas, Properties);
}

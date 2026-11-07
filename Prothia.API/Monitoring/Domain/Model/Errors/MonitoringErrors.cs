namespace Prothia.API.Monitoring.Domain.Model.Errors;

public static class MonitoringErrors
{
    public static readonly Error PatientNotFound =
        Error.NotFound("Monitoring.PatientNotFound", "The specified patient does not exist.");

    public static readonly Error AlertNotFound =
        Error.NotFound("Monitoring.AlertNotFound", "The specified biomechanical alert was not found.");

    public static readonly Error AlertAlreadyResolved =
        Error.Conflict("Monitoring.AlertAlreadyResolved", "The specified biomechanical alert is already resolved.");

    public static readonly Error ThresholdProfileNotFound =
        Error.NotFound("Monitoring.ThresholdProfileNotFound", "The specified threshold profile was not found.");

    public static readonly Error DuplicateThresholdProfileForPatient =
        Error.Conflict("Monitoring.DuplicateThresholdProfileForPatient",
            "A threshold profile already exists for the specified patient.");

    public static readonly Error GaitSessionNotFound =
        Error.NotFound("Monitoring.GaitSessionNotFound", "The specified gait session was not found.");

    public static readonly Error InvalidMonitoringData =
        Error.Validation("Monitoring.InvalidMonitoringData", "The monitoring data provided is invalid.");
}
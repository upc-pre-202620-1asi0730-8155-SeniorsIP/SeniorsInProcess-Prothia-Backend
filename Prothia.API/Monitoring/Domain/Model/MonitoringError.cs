namespace Prothia.API.Monitoring.Domain.Model;

public enum MonitoringError
{
    None,
    PatientNotFound,
    AlertNotFound,
    AlertAlreadyResolved,
    ThresholdProfileNotFound,
    DuplicateThresholdProfileForPatient,
    GaitSessionNotFound,
    InvalidMonitoringData,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
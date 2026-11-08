namespace Prothia.API.Communication.Domain.Model;

public enum CommunicationError
{
    None,
    PatientNotFound,
    ClinicalMessageNotFound,
    ClinicalTransmissionNotFound,
    InvalidCommunicationData,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
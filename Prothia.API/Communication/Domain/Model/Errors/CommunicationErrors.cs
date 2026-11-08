using Prothia.API.Shared.Domain.Model.Errors;

namespace Prothia.API.Communication.Domain.Model.Errors;

public static class CommunicationErrors
{
    public static readonly Error PatientNotFound =
        Error.NotFound("Communication.PatientNotFound", "The specified patient does not exist.");

    public static readonly Error ClinicalMessageNotFound =
        Error.NotFound("Communication.ClinicalMessageNotFound", "The requested clinical message was not found.");

    public static readonly Error ClinicalTransmissionNotFound =
        Error.NotFound("Communication.ClinicalTransmissionNotFound", "The requested clinical transmission was not found.");

    public static readonly Error InvalidCommunicationData =
        Error.Validation("Communication.InvalidData", "The provided communication data is invalid.");
}
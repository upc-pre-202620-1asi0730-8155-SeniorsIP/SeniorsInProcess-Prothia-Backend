using System.Security.Claims;

namespace Prothia.API.Iam.Infrastructure.Pipeline.Claims;

public static class IamClaimsPrincipalExtensions
{
    public const string PatientIdClaimType = "patient_id";

    public static bool IsLinkedToPatient(this ClaimsPrincipal principal, int patientId) =>
        int.TryParse(principal.FindFirst(PatientIdClaimType)?.Value, out var linkedPatientId) &&
        linkedPatientId == patientId;
}

using System.Globalization;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Prothia.API.Iam.Application.Internal.OutboundServices;
using Prothia.API.Iam.Domain.Model.ValueObjects;

namespace Prothia.API.Iam.Infrastructure.Tokens.Jwt.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? Principal => httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;

    public int? UserId
    {
        get
        {
            var rawUserId = Principal?.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? Principal?.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return int.TryParse(rawUserId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var userId) && userId > 0
                ? userId
                : null;
        }
    }

    public string? Username =>
        Principal?.FindFirstValue(ClaimTypes.Name)
        ?? Principal?.FindFirstValue(JwtRegisteredClaimNames.UniqueName)
        ?? Principal?.Identity?.Name;

    public string? FullName
    {
        get
        {
            var fullName = Principal?.FindFirstValue("fullName");
            return string.IsNullOrWhiteSpace(fullName) ? null : fullName.Trim();
        }
    }

    public ERole? Role
    {
        get
        {
            var rawRole = Principal?.FindFirstValue(ClaimTypes.Role)
                          ?? Principal?.FindFirstValue("role");
            if (string.IsNullOrWhiteSpace(rawRole) || int.TryParse(rawRole, out _))
                return null;

            return Enum.TryParse<ERole>(rawRole.Trim(), ignoreCase: false, out var parsed) && Enum.IsDefined(parsed)
                ? parsed
                : null;
        }
    }

    public int? PatientId
    {
        get
        {
            var rawPatientId = Principal?.FindFirstValue("patientId");
            return int.TryParse(rawPatientId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var patientId) && patientId > 0
                ? patientId
                : null;
        }
    }

    public int? CreatedByUserId
    {
        get
        {
            var rawCreatedBy = Principal?.FindFirstValue("createdByUserId");
            return int.TryParse(rawCreatedBy, NumberStyles.Integer, CultureInfo.InvariantCulture, out var ownerId) && ownerId > 0
                ? ownerId
                : null;
        }
    }

    public int? ClinicOwnerUserId => IsClinicalProfessional ? UserId : CreatedByUserId;

    public bool IsClinicalProfessional => Role == ERole.ClinicalProfessional;
    public bool IsPatient => Role == ERole.Patient;
    public bool IsTechnician => Role == ERole.Technician;
}

namespace Prothia.API.Patients.Interfaces.ACL;

public interface IPatientContextFacade
{
    Task<bool> ExistsPatientByIdAsync(int patientId, CancellationToken cancellationToken = default);
    
    Task<string?> FetchPatientFullNameByIdAsync(int patientId, CancellationToken cancellationToken = default);
}
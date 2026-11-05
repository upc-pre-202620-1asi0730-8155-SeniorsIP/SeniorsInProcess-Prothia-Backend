namespace Prothia.API.Patients.Interfaces.ACL;

public interface IPatientsContextFacade
{
    Task<bool> ExistsPatientByIdAsync(int patientId, CancellationToken cancellationToken = default);
    
    Task<string?> FetchPatientFullNameByIdAsync(int patientId, CancellationToken cancellationToken = default);
}
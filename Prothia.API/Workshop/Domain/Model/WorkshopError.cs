namespace Prothia.API.Workshop.Domain.Model;

public enum WorkshopError
{
    None,
    ProsthesisNotFound,
    DuplicateSerialNumber,
    MechanicalAlertNotFound,
    MechanicalAlertAlreadyResolved,
    MaintenanceScheduleNotFound,
    InvalidWorkshopData,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
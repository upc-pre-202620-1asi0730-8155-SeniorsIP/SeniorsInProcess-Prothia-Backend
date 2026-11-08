using Prothia.API.Shared.Domain.Model.Errors;

namespace Prothia.API.Workshop.Domain.Model.Errors;

public static class WorkshopErrors
{
    public static readonly Error ProsthesisNotFound = Error.NotFound(
        "Workshop.ProsthesisNotFound",
        "The specified prosthesis was not found in the workshop inventory.");

    public static readonly Error DuplicateSerialNumber = Error.Conflict(
        "Workshop.DuplicateSerialNumber",
        "A prosthesis with the specified serial number is already registered.");

    public static readonly Error MechanicalAlertNotFound = Error.NotFound(
        "Workshop.MechanicalAlertNotFound",
        "The specified mechanical alert was not found.");

    public static readonly Error MechanicalAlertAlreadyResolved = Error.Conflict(
        "Workshop.MechanicalAlertAlreadyResolved",
        "The specified mechanical alert has already been resolved.");

    public static readonly Error MaintenanceScheduleNotFound = Error.NotFound(
        "Workshop.MaintenanceScheduleNotFound",
        "The specified maintenance schedule was not found.");

    public static readonly Error InvalidWorkshopData = Error.Validation(
        "Workshop.InvalidWorkshopData",
        "The provided workshop data is invalid.");
}
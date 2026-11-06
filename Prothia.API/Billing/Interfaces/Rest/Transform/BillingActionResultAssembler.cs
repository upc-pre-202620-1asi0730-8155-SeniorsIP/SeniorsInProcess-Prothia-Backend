using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Billing.Domain.Model;
using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Shared.Application.Model;
using Prothia.API.Shared.Interfaces.Rest.ProblemDetails;
using Prothia.API.Shared.Resources.Errors;

namespace Prothia.API.Billing.Interfaces.Rest.Transform;

public static class BillingActionResultAssembler
{
    private static int ToStatusCodeFromBillingError(BillingError error)
    {
        return error switch
        {
            BillingError.ClinicSubscriptionNotFound => StatusCodes.Status404NotFound,
            BillingError.ElectronicInvoiceNotFound => StatusCodes.Status404NotFound,
            BillingError.DuplicateInvoiceNumber => StatusCodes.Status409Conflict,
            BillingError.InvalidBillingData => StatusCodes.Status400BadRequest,
            BillingError.OperationCancelled => StatusCodes.Status409Conflict,
            BillingError.DatabaseError => StatusCodes.Status500InternalServerError,
            BillingError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    public static IActionResult ToActionResultFromClinicSubscriptionResult(
        ControllerBase controller,
        Result<ClinicSubscription> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<ClinicSubscription, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromBillingError((BillingError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetClinicSubscriptionByIdResult(
        ControllerBase controller,
        ClinicSubscription? subscription,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<ClinicSubscription, IActionResult> successAction)
    {
        if (subscription is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromBillingError(BillingError.ClinicSubscriptionNotFound),
                BillingError.ClinicSubscriptionNotFound,
                errorLocalizer[$"BillingError.{nameof(BillingError.ClinicSubscriptionNotFound)}"]
            );
        return successAction(subscription);
    }

    public static IActionResult ToActionResultFromElectronicInvoiceResult(
        ControllerBase controller,
        Result<ElectronicInvoice> result,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<ElectronicInvoice, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromBillingError((BillingError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetElectronicInvoiceByIdResult(
        ControllerBase controller,
        ElectronicInvoice? invoice,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<ElectronicInvoice, IActionResult> successAction)
    {
        if (invoice is null)
            return problemDetailsFactory.CreateProblemDetails(
                controller,
                ToStatusCodeFromBillingError(BillingError.ElectronicInvoiceNotFound),
                BillingError.ElectronicInvoiceNotFound,
                errorLocalizer[$"BillingError.{nameof(BillingError.ElectronicInvoiceNotFound)}"]
            );
        return successAction(invoice);
    }
}

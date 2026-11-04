using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Prothia.API.Billing.Application.Errors;
using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Shared.Application.Patterns;

namespace Prothia.API.Billing.Interfaces.Rest.Transform;

public static class ActionResultFromCreateClinicSubscriptionResultAssembler
{
  public static ActionResult ToActionResultFromCreateClinicSubscriptionResult(
    Result<ClinicSubscription, ECreateClinicSubscriptionError> result,
    ControllerBase controller,
    IStringLocalizer<ErrorMessages> localizer,
    string getClinicSubscriptionByIdActionName) => result switch
  {
    Result<ClinicSubscription, ECreateClinicSubscriptionError>.Success succes =>
      controller.CreatedAtAction(getClinicSubscriptionByIdActionName, new { id = succes.Value.Id },
        ClinicSubscriptionResourceFromEntityAssembler.ToResourceFromEntity(succes.Value)),

    Result<ClinicSubscription, ECreateClinicSubscriptionError>.Failure failure =>
      failure.Error switch
      {
        ECreateClinicSubscriptionError.InvalidClinicSubscription =>
          controller.BadRequest(localizer["InvalidClinicSubscriptionRequest"].Value),
        ECreateClinicSubscriptionError.UnexpectedError =>
          controller.Problem(
            title: localizer["UnexpectedServerError"].Value,
            detail: localizer["UnexpectedServerErrorCreatingClinicSubscription"].Value,
            statusCode: 500),
        _ => controller.Problem(
          title: localizer["UnexpectedServerError"].Value,
          detail: localizer["UnexpectedErrorProcessingRequest"].Value,
          statusCode: 500)
      },
    _ => controller.Problem(
      title: localizer["UnexpectedServerError"].Value,
      detail: localizer["UnexpectedErrorProcessingRequest"].Value,
      statusCode: 500)

  };
}
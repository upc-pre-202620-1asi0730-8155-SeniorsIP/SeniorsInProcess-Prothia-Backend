using Prothia.API.Billing.Application.Errors;
using Prothia.API.Billing.Application.Services;
using Prothia.API.Billing.Domain.Model.Aggregates;
using Prothia.API.Billing.Domain.Model.Commands;
using Prothia.API.Billing.Domain.Repositories;
using Prothia.API.Shared.Application.Extensions;
using Prothia.API.Shared.Application.Patterns;
using Prothia.API.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Prothia.API.Billing.Application.Internal.CommandServices;

public class ElectronicInvoiceCommandService(
    IElectronicInvoiceRepository electronicInvoiceRepository,
    IUnitOfWork unitOfWork,
    ILogger<ElectronicInvoiceCommandService> logger)
    : IElectronicInvoiceCommandService
{
    /// <inheritdoc />
    public async Task<Result<ElectronicInvoice, ECreateElectronicInvoiceError>> Handle(
        CreateElectronicInvoiceCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var existing =
                await electronicInvoiceRepository.FindByInvoiceNumberAsync(command.InvoiceNumber, cancellationToken);
            if (existing is not null)
            {
                logger.LogWarning("Duplicate electronic invoice rejected for invoice number {InvoiceNumber}",
                    command.InvoiceNumber);
                return new Result<ElectronicInvoice, ECreateElectronicInvoiceError>.Failure(
                    ECreateElectronicInvoiceError.DuplicateInvoiceNumber);
            }

            var invoice = new ElectronicInvoice(command);
            await electronicInvoiceRepository.AddAsync(invoice, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return new Result<ElectronicInvoice, ECreateElectronicInvoiceError>.Success(invoice);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid arguments while creating electronic invoice {InvoiceNumber}",
                command.InvoiceNumber);
            return new Result<ElectronicInvoice, ECreateElectronicInvoiceError>.Failure(
                ECreateElectronicInvoiceError.InvalidElectronicInvoice);
        }
        catch (DbUpdateException ex) when (ex.IsDuplicateKeyViolation())
        {
            logger.LogWarning(ex, "Duplicate key violation creating electronic invoice {InvoiceNumber}",
                command.InvoiceNumber);
            return new Result<ElectronicInvoice, ECreateElectronicInvoiceError>.Failure(
                ECreateElectronicInvoiceError.DuplicateInvoiceNumber);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed creating electronic invoice {InvoiceNumber}",
                command.InvoiceNumber);
            return new Result<ElectronicInvoice, ECreateElectronicInvoiceError>.Failure(
                ECreateElectronicInvoiceError.UnexpectedError);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating electronic invoice {InvoiceNumber}", command.InvoiceNumber);
            return new Result<ElectronicInvoice, ECreateElectronicInvoiceError>.Failure(
                ECreateElectronicInvoiceError.UnexpectedError);
        }
    }
}

using Prothia.API.Billing.Domain.Model.Commands;
using Prothia.API.Billing.Domain.Model.ValueObjects;

namespace Prothia.API.Billing.Domain.Model.Aggregates;

/// <summary>
///     Electronic invoice aggregate root issued for subscription billing.
/// </summary>
public partial class ElectronicInvoice
{
    public const int IssueDateMaxLength = 30;
    public const int ConceptMaxLength = 250;
    public const int CurrencyMaxLength = 10;
    public const int StatusLabelMaxLength = 50;
    public const int DownloadUrlMaxLength = 500;

    public int Id { get; private set; }
    public InvoiceNumber InvoiceNumber { get; private set; }
    public string IssueDate { get; private set; }
    public string Concept { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public string StatusLabel { get; private set; }
    public string DownloadUrl { get; private set; }

    protected ElectronicInvoice()
    {
        InvoiceNumber = null!;
        IssueDate = string.Empty;
        Concept = string.Empty;
        Currency = string.Empty;
        StatusLabel = string.Empty;
        DownloadUrl = string.Empty;
    }

    public ElectronicInvoice(CreateElectronicInvoiceCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.Amount < 0)
            throw new ArgumentOutOfRangeException(nameof(command), command.Amount, "Amount cannot be negative.");
        if (!Enum.IsDefined(command.Status))
            throw new ArgumentOutOfRangeException(nameof(command), command.Status, "Invalid invoice status.");

        InvoiceNumber = command.InvoiceNumber ??
                        throw new ArgumentNullException(nameof(command), "InvoiceNumber is required.");
        IssueDate = ValidateRequiredString(command.IssuaDate, IssueDateMaxLength, nameof(command.IssuaDate));
        Concept = ValidateRequiredString(command.Concept, ConceptMaxLength, nameof(command.Concept));
        Amount = Math.Round(command.Amount, 2);
        Currency = ValidateRequiredString(command.currency, CurrencyMaxLength, nameof(command.currency))
            .ToUpperInvariant();
        Status = command.Status;
        StatusLabel = ValidateRequiredString(command.StatusLabel, StatusLabelMaxLength, nameof(command.StatusLabel));
        DownloadUrl = ValidateRequiredString(command.DownloadUrl, DownloadUrlMaxLength, nameof(command.DownloadUrl));
    }

    private static string ValidateRequiredString(string? value, int maxLength, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{paramName} cannot be null or whitespace.", paramName);

        var trimmed = value.Trim();
        if (trimmed.Length > maxLength)
            throw new ArgumentException($"{paramName} cannot exceed {maxLength} characters.", paramName);

        return trimmed;
    }
}

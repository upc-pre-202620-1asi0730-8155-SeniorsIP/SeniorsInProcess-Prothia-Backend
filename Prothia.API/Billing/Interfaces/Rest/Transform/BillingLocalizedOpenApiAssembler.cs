using Prothia.API.Billing.Interfaces.Rest.Resources;
using Prothia.API.Shared.Interfaces.Asp.Configuration;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Prothia.API.Billing.Interfaces.Rest.Transform;

public static class BillingLocalizedOpenApiAssembler
{
    private static readonly IReadOnlyDictionary<string, OperationDocumentation> Operations =
        new Dictionary<string, OperationDocumentation>
        {
            ["CreateClinicSubscription"] = LocalizedOpenApiTransformers.Operation("CreateClinicSubscription",
                ("201", "Created"), ("400", "BadRequest"), ("500", "ServerError")),
            ["GetClinicSubscriptions"] = LocalizedOpenApiTransformers.Operation("GetClinicSubscriptions",
                ("200", "Success")),
            ["GetClinicSubscriptionById"] = LocalizedOpenApiTransformers.Operation("GetClinicSubscriptionById",
                ("200", "Success"), ("404", "NotFound")),
            ["UpdateClinicSubscription"] = LocalizedOpenApiTransformers.Operation("UpdateClinicSubscription",
                ("200", "Success"), ("400", "BadRequest"), ("404", "NotFound"), ("500", "ServerError")),
            ["CreateInvoice"] = LocalizedOpenApiTransformers.Operation("CreateInvoice",
                ("201", "Created"), ("400", "BadRequest"), ("409", "Conflict"), ("500", "ServerError")),
            ["GetInvoices"] = LocalizedOpenApiTransformers.Operation("GetInvoices",
                ("200", "Success")),
            ["GetInvoiceById"] = LocalizedOpenApiTransformers.Operation("GetInvoiceById",
                ("200", "Success"), ("404", "NotFound"))
        };

    private static readonly IReadOnlyDictionary<Type, string> Schemas = new Dictionary<Type, string>
    {
        [typeof(CreateClinicSubscriptionResource)] = "CreateClinicSubscriptionSchema",
        [typeof(UpdateClinicSubscriptionResource)] = "UpdateClinicSubscriptionSchema",
        [typeof(ClinicSubscriptionResource)] = "ClinicSubscriptionSchema",
        [typeof(PaymentMethodResource)] = "PaymentMethodSchema",
        [typeof(CreateElectronicInvoiceResource)] = "CreateElectronicInvoiceSchema",
        [typeof(ElectronicInvoiceResource)] = "ElectronicInvoiceSchema"
    };

    private static readonly IReadOnlyDictionary<(Type, string), string> Properties = BuildProperties();

    private static Dictionary<(Type, string), string> BuildProperties()
    {
        var properties = new Dictionary<(Type, string), string>();

        foreach (var type in new[]
                 {
                     typeof(ClinicSubscriptionResource),
                     typeof(CreateClinicSubscriptionResource),
                     typeof(UpdateClinicSubscriptionResource)
                 })
        {
            properties[(type, "planName")] = "SubscriptionPlanName";
            properties[(type, "status")] = "SubscriptionStatus";
            properties[(type, "planType")] = "SubscriptionPlanType";
            properties[(type, "annualFee")] = "SubscriptionAnnualFee";
            properties[(type, "monthlyEquivalent")] = "SubscriptionMonthlyEquivalent";
            properties[(type, "startDate")] = "SubscriptionStartDate";
            properties[(type, "renewalDate")] = "SubscriptionRenewalDate";
            properties[(type, "usedQuota")] = "SubscriptionUsedQuota";
            properties[(type, "totalQuota")] = "SubscriptionTotalQuota";
            properties[(type, "paymentMethod")] = "SubscriptionPaymentMethod";
        }

        properties[(typeof(ClinicSubscriptionResource), "id")] = "SubscriptionId";

        properties[(typeof(PaymentMethodResource), "brand")] = "PaymentMethodBrand";
        properties[(typeof(PaymentMethodResource), "cardTitle")] = "PaymentMethodCardTitle";
        properties[(typeof(PaymentMethodResource), "expiry")] = "PaymentMethodExpiry";
        properties[(typeof(PaymentMethodResource), "autoRenew")] = "PaymentMethodAutoRenew";

        foreach (var type in new[] { typeof(ElectronicInvoiceResource), typeof(CreateElectronicInvoiceResource) })
        {
            properties[(type, "invoiceNumber")] = "InvoiceNumber";
            properties[(type, "issueDate")] = "InvoiceIssueDate";
            properties[(type, "concept")] = "InvoiceConcept";
            properties[(type, "amount")] = "InvoiceAmount";
            properties[(type, "currency")] = "InvoiceCurrency";
            properties[(type, "status")] = "InvoiceStatus";
            properties[(type, "statusLabel")] = "InvoiceStatusLabel";
            properties[(type, "downloadUrl")] = "InvoiceDownloadUrl";
        }

        properties[(typeof(ElectronicInvoiceResource), "id")] = "InvoiceId";

        return properties;
    }

    public static Task TransformOperationAsync(OpenApiOperation operation,
        OpenApiOperationTransformerContext context, CancellationToken cancellationToken) =>
        LocalizedOpenApiTransformers.TransformOperationAsync(operation, context, Operations);

    public static Task TransformSchemaAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken) =>
        LocalizedOpenApiTransformers.TransformSchemaAsync(schema, context, Schemas, Properties);
}

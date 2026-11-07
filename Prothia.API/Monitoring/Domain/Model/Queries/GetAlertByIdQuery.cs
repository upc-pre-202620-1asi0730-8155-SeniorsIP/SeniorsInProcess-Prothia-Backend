using Prothia.API.Monitoring.Domain.Model.ValueObjects;

namespace Prothia.API.Monitoring.Domain.Model.Queries;

public record GetAlertByIdQuery(AlertId Id)
{
    public GetAlertByIdQuery(int id) : this(new AlertId(id))
    {
    }
}
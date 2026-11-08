using Prothia.API.Monitoring.Domain.Model.ValueObjects;

namespace Prothia.API.Monitoring.Domain.Model.Queries;

public record GetGaitSessionByIdQuery(GaitSessionId Id)
{
    public GetGaitSessionByIdQuery(int id) : this(new GaitSessionId(id))
    {
    }
}
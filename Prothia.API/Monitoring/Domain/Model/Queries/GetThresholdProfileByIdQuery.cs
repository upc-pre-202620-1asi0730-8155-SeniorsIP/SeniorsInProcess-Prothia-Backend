using Prothia.API.Monitoring.Domain.Model.ValueObjects;

namespace Prothia.API.Monitoring.Domain.Model.Queries;

public record GetThresholdProfileByIdQuery(ThresholdProfileId Id)
{
    public GetThresholdProfileByIdQuery(int id) : this(new ThresholdProfileId(id))
    {
    }
}

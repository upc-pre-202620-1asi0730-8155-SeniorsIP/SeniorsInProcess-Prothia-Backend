using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Shared.Domain.Repositories;

namespace Prothia.API.Communication.Domain.Repositories;

public interface IClinicalMessageRepository : IBaseRepository<ClinicalMessage>
{
}
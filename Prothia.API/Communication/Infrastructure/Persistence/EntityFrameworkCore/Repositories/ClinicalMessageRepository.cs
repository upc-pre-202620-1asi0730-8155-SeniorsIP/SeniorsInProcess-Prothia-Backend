using Prothia.API.Communication.Domain.Model.Aggregates;
using Prothia.API.Communication.Domain.Repositories;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Prothia.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Prothia.API.Communication.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ClinicalMessageRepository(AppDbContext context)
    : BaseRepository<ClinicalMessage>(context), IClinicalMessageRepository
{
}
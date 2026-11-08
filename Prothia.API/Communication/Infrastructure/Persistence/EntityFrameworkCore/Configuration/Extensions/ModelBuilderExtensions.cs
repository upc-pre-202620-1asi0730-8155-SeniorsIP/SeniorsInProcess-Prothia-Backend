using Microsoft.EntityFrameworkCore;

namespace Prothia.API.Communication.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyCommunicationConfiguration(this ModelBuilder builder)
    {
        // Aquí registramos las configuraciones de tu módulo
        builder.ApplyConfiguration(new MessageConfiguration());
        builder.ApplyConfiguration(new UserNotificationConfiguration());
    }
}
using Microsoft.Extensions.DependencyInjection;
using Neo4j.Migration;
using Neo4j.Migration.Journal;
using System;

namespace Neo4jMigration.AspNetCore
{
    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddNeo4jMigrations(this IServiceCollection services, Func<MigrationConfigurationBuilder> builder)
        {
            if(builder is null)
            {
                throw new Exception("You must provide the configuration.");
            }

            var configuration = builder.Invoke().Build();
            return services
                .AddSingleton(configuration)
                .AddScoped<IMigrate, Migrate>()
                .AddScoped<IJournalRepository>(ctx =>
                {
                    using var scope = ctx.CreateScope();
                    var configuration = scope.ServiceProvider.GetRequiredService<MigrationConfiguration>();
                    return new JournalRepository(configuration.Driver);
                });
        }

    }
}

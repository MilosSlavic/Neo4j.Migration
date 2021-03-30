using System.Threading.Tasks;

namespace Neo4j.Migration
{
    public interface IMigrate
    {
        Task ExecuteAsync(MigrationConfiguration configuration);
    }
}

using FluentMigrator.Runner;

namespace Pomnesh.Infrastructure.Migrations;

public class MigrationRunner(IMigrationRunner migrationRunner)
{
    public void Run()
    {
        migrationRunner.MigrateUp();
    }
}
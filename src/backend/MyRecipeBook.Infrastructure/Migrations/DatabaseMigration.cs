using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;
using System.Text.RegularExpressions;

namespace MyRecipeBook.Infrastructure.Migrations
{



    public static class DatabaseMigration
    {
        private static readonly Regex _schemaNameRegex = new(@"^[A-Za-z0-9_]+$", RegexOptions.Compiled);
        public static void Migrate(string connectionString, IServiceProvider serviceProvider)
        {
            EnsureDatabaseCreate(connectionString);
            MigrationDatabase(serviceProvider);
        }

        private static void EnsureDatabaseCreate(string connectionString)
        {
            var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);

            var databaseName = connectionStringBuilder.Database;

            if (!_schemaNameRegex.IsMatch(databaseName))
                throw new InvalidOperationException(
                    $"Nome de schema inválido: {databaseName}");

            var quotedName = $"`{databaseName.Replace("`", "``")}`";

            connectionStringBuilder.Remove("Database");

            using var dbConnection = new MySqlConnection(connectionStringBuilder.ConnectionString);

            var parameters = new DynamicParameters();
            parameters.Add("name", databaseName);

            var records = dbConnection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @name", parameters);

            if(!records.Any())
                dbConnection.Execute($"CREATE DATABASE {quotedName}");
            
        }

        private static void MigrationDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.ListMigrations();
            runner.MigrateUp();
        }


    
    }
}

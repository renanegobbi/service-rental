using Npgsql;

namespace RentalService.Tests.Integration.Utils
{
    public static class DatabaseSeeder
    {
        public static void ApplyInitScript(string connectionString)
        {
            var scriptPath = Path.Combine(AppContext.BaseDirectory, "init-database.sql");

            if (!File.Exists(scriptPath))
                throw new FileNotFoundException($"Init script not found: {scriptPath}");

            var sql = File.ReadAllText(scriptPath);

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using var transaction = connection.BeginTransaction();

            var commands = sql.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var cmdText in commands)
            {
                using var cmd = new NpgsqlCommand(cmdText, connection, transaction);
                cmd.ExecuteNonQuery();
            }

            transaction.Commit();
        }
    }
}

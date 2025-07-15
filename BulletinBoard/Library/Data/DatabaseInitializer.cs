using System.Data;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Library.Data;

public class DatabaseInitializer(IConfiguration configuration)
{
    private readonly IConfiguration _config = configuration;

    public async Task InitializeAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();

        // 1) Run the "InitializeDatabase.sql" on master connection (to create DB)
        var masterConnStr = _config.GetConnectionString("MasterConnection");
        await using (var masterConnection = new SqlConnection(masterConnStr))
        {
            await masterConnection.OpenAsync();

            var masterResourceName = "Library.Data.Setup.InitializeDatabase.sql";
            await ExecuteEmbeddedSqlScriptAsync(masterConnection, assembly, masterResourceName);
        }

        // 2) Run the "CreateTables.sql" on default connection (to create tables)
        var defaultConnStr = _config.GetConnectionString("DefaultConnection");
        await using (var defaultConnection = new SqlConnection(defaultConnStr))
        {
            await defaultConnection.OpenAsync();

            var defaultResourceName = "Library.Data.Setup.InitializeSchema.sql"; // replace with your actual filename
            await ExecuteEmbeddedSqlScriptAsync(defaultConnection, assembly, defaultResourceName);
        }
    }

    private static async Task ExecuteEmbeddedSqlScriptAsync(SqlConnection connection, Assembly assembly,
        string resourceName)
    {
        await using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream is null)
            throw new FileNotFoundException($"Embedded SQL resource '{resourceName}' not found.");

        using var reader = new StreamReader(stream);
        var sql = await reader.ReadToEndAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;

        await command.ExecuteNonQueryAsync();
    }
}
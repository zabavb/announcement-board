using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Library.Data;

public class DatabaseInitializer(IConfiguration configuration)
{
    private readonly IConfiguration _config = configuration;

    public async Task InitializeAsync()
    {
        var masterConnStr = _config.GetConnectionString("MasterConnection");
        await using var connection = new SqlConnection(masterConnStr);

        var sql = await File.ReadAllTextAsync("Data/Setup/InitializeDatabase.sql");
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        command.CommandType = CommandType.Text;

        await command.ExecuteNonQueryAsync();
    }
}
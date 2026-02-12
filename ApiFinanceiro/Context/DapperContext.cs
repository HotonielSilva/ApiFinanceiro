using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiFinanceiro.Context;

public class DapperContext(IConfiguration configuration)
{
    private readonly string? _connectionstring = configuration.GetConnectionString("DefaultConnection");

    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionstring);
}

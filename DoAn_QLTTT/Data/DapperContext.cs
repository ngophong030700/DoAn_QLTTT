using System.Data;
using Microsoft.Data.SqlClient;

namespace DoAn_QLTTT.Data;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("TODO: Bổ sung ConnectionStrings:DefaultConnection trong appsettings.json.");
    }

    public IDbConnection CreateConnection()
    {
        // TODO: Khi có SQL Server thật, kiểm tra lại server, database, user/password và TrustServerCertificate.
        return new SqlConnection(_connectionString);
    }
}

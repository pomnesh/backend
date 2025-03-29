using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Pomnesh.Infrastructure;

public class DapperContext : IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly NpgsqlDataSource _dataSource;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        _dataSource = NpgsqlDataSource.Create(connectionString);
    }

    public IDbConnection CreateConnection() => _dataSource.CreateConnection();

    public void Dispose()
    {
        _dataSource?.Dispose();
    }
}
using System.Data;
using Microsoft.Extensions.Configuration;

using Npgsql;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Pomnesh.Infrastructure;

public class DapperContext : IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly NpgsqlDataSource _dataSource;
    private readonly ILogger _logger;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _logger = Log.ForContext<DapperContext>();
        
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        if (connectionString is null)
        {
            _logger.Error("No connection string provided in configuration");
            throw new Exception("No connection string provided");
        }
        
        try
        {
            _logger.Information("Creating Npgsql data source");
            _dataSource = NpgsqlDataSource.Create(connectionString);
            _logger.Information("Npgsql data source created successfully");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to create Npgsql data source");
            throw;
        }
    }

    public IDbConnection CreateConnection()
    {
        _logger.Debug("Creating new database connection");
        var connection = _dataSource.CreateConnection();
        _logger.Debug("Database connection created successfully");
        return connection;
    }

    public void Dispose()
    {
        _logger.Debug("Disposing DapperContext");
        _dataSource?.Dispose();
        _logger.Debug("DapperContext disposed successfully");
    }
}
using Dapper;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;
using Serilog;

namespace Pomnesh.Infrastructure.Repositories;

public class RecollectionRepository : IBaseRepository<Recollection>
{
    private readonly DapperContext _context;
    private readonly ILogger _logger;

    public RecollectionRepository(DapperContext context)
    {
        _context = context;
        _logger = Log.ForContext<RecollectionRepository>();
    }

    public async Task<int> Add(Recollection recollection)
    {
        _logger.Information("Adding new recollection for UserId: {UserId}", recollection.UserId);
        var sql = @"
            INSERT INTO ""Recollections"" (""UserId"", ""DownloadLink"") 
            VALUES (@UserId, @DownloadLink)
            RETURNING ""Id"";
        ";
        
        try
        {
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.ExecuteScalarAsync<int>(sql, recollection);
                _logger.Information("Successfully added recollection with Id: {Id}", id);
                return id;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to add recollection for UserId: {UserId}", recollection.UserId);
            throw;
        }
    }

    public async Task<Recollection?> GetById(long id)
    {
        _logger.Debug("Retrieving recollection with Id: {Id}", id);
        var sql = @"SELECT ""Id"", ""UserId"", ""CreatedAt"", ""DownloadLink""  FROM ""Recollections"" WHERE ""Id"" = @id";
        
        try
        {
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<Recollection>(sql, new { id });
                if (result == null)
                {
                    _logger.Warning("Recollection with Id: {Id} not found", id);
                }
                else
                {
                    _logger.Debug("Successfully retrieved recollection with Id: {Id}", id);
                }
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to retrieve recollection with Id: {Id}", id);
            throw;
        }
    }
    
    public async Task<IEnumerable<Recollection>> GetAll()
    {
        _logger.Debug("Retrieving all recollections");
        var sql = @"SELECT ""Id"", ""UserId"", ""CreatedAt"", ""DownloadLink"" FROM ""Recollections"";";
        
        try
        {
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<Recollection>(sql);
                _logger.Debug("Retrieved {Count} recollections", result.Count());
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to retrieve all recollections");
            throw;
        }
    }

    public async Task Update(Recollection recollection)
    {
        _logger.Information("Updating recollection with Id: {Id}", recollection.Id);
        var sql = @"
        UPDATE ""Recollections""
        SET
            ""UserId"" = @UserId,
            ""CreatedAt"" = @CreatedAt,
            ""DownloadLink"" = @DownloadLink
        WHERE
            ""Id"" = @Id
        ";

        try
        {
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, recollection);
                _logger.Information("Successfully updated recollection with Id: {Id}", recollection.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to update recollection with Id: {Id}", recollection.Id);
            throw;
        }
    }

    public async Task Delete(long id)
    {
        _logger.Information("Deleting recollection with Id: {Id}", id);
        var sql = @"
        DELETE
        FROM ""Recollections""
        WHERE
            ""Id"" = @id
        ";

        try
        {
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { id });
                _logger.Information("Successfully deleted recollection with Id: {Id}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to delete recollection with Id: {Id}", id);
            throw;
        }
    }
}
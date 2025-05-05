using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;
using Serilog;

namespace Pomnesh.Infrastructure.Repositories;

public class AttachmentRepository : IBaseRepository<Attachment>
{
    private readonly DapperContext _context;
    private readonly ILogger _logger;

    public AttachmentRepository(DapperContext context)
    {
        _context = context;
        _logger = Log.ForContext<AttachmentRepository>();
    }

    public async Task<int> Add(Attachment attachment)
    {
        _logger.Information("Adding new attachment with Type: {Type}, FileId: {FileId}", attachment.Type, attachment.FileId);
        var sql = @"
        INSERT INTO ""Attachments"" (""Type"", ""FileId"", ""OwnerId"", ""OriginalLink"", ""ContextId"") 
        VALUES (@Type, @FileId, @OwnerId, @OriginalLink, @ContextId)
        RETURNING ""Id""";
        
        try
        {
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.ExecuteScalarAsync<int>(sql, attachment);
                _logger.Information("Successfully added attachment with Id: {Id}", id);
                return id;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to add attachment with Type: {Type}, FileId: {FileId}", attachment.Type, attachment.FileId);
            throw;
        }
    }

    public async Task<Attachment?> GetById(long id)
    {
        _logger.Debug("Retrieving attachment with Id: {Id}", id);
        var sql = @"SELECT ""Id"", ""Type"", ""FileId"", ""OwnerId"", ""OriginalLink"", ""ContextId""  FROM ""Attachments"" WHERE ""Id"" = @id";
        
        try
        {
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<Attachment>(sql, new { id });
                if (result == null)
                {
                    _logger.Warning("Attachment with Id: {Id} not found", id);
                }
                else
                {
                    _logger.Debug("Successfully retrieved attachment with Id: {Id}", id);
                }
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to retrieve attachment with Id: {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Attachment>> GetAll()
    {
        _logger.Debug("Retrieving all attachments");
        var sql = @"SELECT ""Id"", ""Type"", ""FileId"", ""OwnerId"", ""OriginalLink"", ""ContextId"" FROM ""Attachments""";
        
        try
        {
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<Attachment>(sql);
                _logger.Debug("Retrieved {Count} attachments", result.Count());
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to retrieve all attachments");
            throw;
        }
    }

    public async Task Update(Attachment attachment)
    {
        _logger.Information("Updating attachment with Id: {Id}", attachment.Id);
        var sql = @"
        UPDATE ""Attachments"" 
        SET
            ""Type"" = @Type,
            ""FileId"" = @FileId, 
            ""OwnerId"" = @OwnerId,
            ""OriginalLink"" = @OriginalLink,
            ""ContextId"" = @ContextId
        WHERE ""Id"" = @Id";

        try
        {
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, attachment);
                _logger.Information("Successfully updated attachment with Id: {Id}", attachment.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to update attachment with Id: {Id}", attachment.Id);
            throw;
        }
    }

    public async Task Delete(long id)
    {
        _logger.Information("Deleting attachment with Id: {Id}", id);
        var sql = @"
        DELETE
        FROM ""Attachments""
        WHERE
            ""Id"" = @id
        ";
    
        try
        {
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { id });
                _logger.Information("Successfully deleted attachment with Id: {Id}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to delete attachment with Id: {Id}", id);
            throw;
        }
    }
}
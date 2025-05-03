using Dapper;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;
using Serilog;

namespace Pomnesh.Infrastructure.Repositories;

public class ChatContextRepository : IBaseRepository<ChatContext>
{
    private readonly DapperContext _context;
    private readonly ILogger _logger;

    public ChatContextRepository(DapperContext context)
    {
        _context = context;
        _logger = Log.ForContext<ChatContextRepository>();
    }

    public async Task<int> Add(ChatContext chatContext)
    {
        _logger.Information("Adding new chat context with MessageId: {MessageId}", chatContext.MessageId);
        var sql = @"
        INSERT INTO ""ChatContexts"" (""MessageId"", ""MessageText"", ""MessageDate"")
        VALUES (@MessageId, @MessageText, @MessageDate)
        RETURNING ""Id"";";
        
        try
        {
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.ExecuteScalarAsync<int>(sql, chatContext);
                _logger.Information("Successfully added chat context with Id: {Id}", id);
                return id;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to add chat context with MessageId: {MessageId}", chatContext.MessageId);
            throw;
        }
    }

    public async Task<ChatContext?> GetById(long id)
    {
        _logger.Debug("Retrieving chat context with Id: {Id}", id);
        var sql = @"SELECT ""Id"", ""MessageId"", ""MessageText"", ""MessageDate"" FROM ""ChatContexts"" WHERE ""Id"" = @id";
        
        try
        {
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ChatContext>(sql, new { id });
                if (result == null)
                {
                    _logger.Warning("Chat context with Id: {Id} not found", id);
                }
                else
                {
                    _logger.Debug("Successfully retrieved chat context with Id: {Id}", id);
                }
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to retrieve chat context with Id: {Id}", id);
            throw;
        }
    }
    
    public async Task<IEnumerable<ChatContext>> GetAll()
    {
        _logger.Debug("Retrieving all chat contexts");
        var sql = @"SELECT ""Id"", ""MessageId"", ""MessageText"", ""MessageDate"" FROM ""ChatContexts""";
        
        try
        {
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<ChatContext>(sql);
                _logger.Debug("Retrieved {Count} chat contexts", result.Count());
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to retrieve all chat contexts");
            throw;
        }
    }

    public async Task Update(ChatContext chatContext)
    {
        _logger.Information("Updating chat context with Id: {Id}", chatContext.Id);
        var sql = @"
        UPDATE ""ChatContexts""
        SET
            ""MessageId"" = @MessageId,
            ""MessageText"" = @MessageText,
            ""MessageDate"" = @MessageDate
        WHERE
            ""Id"" = @Id
        ";

        try
        {
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, chatContext);
                _logger.Information("Successfully updated chat context with Id: {Id}", chatContext.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to update chat context with Id: {Id}", chatContext.Id);
            throw;
        }
    }
    
    public async Task Delete(long id)
    {
        _logger.Information("Deleting chat context with Id: {Id}", id);
        var sql = @"
        DELETE
        FROM ""ChatContexts""
        WHERE
            ""Id"" = @id
        ";
        
        try
        {
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { id });
                _logger.Information("Successfully deleted chat context with Id: {Id}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to delete chat context with Id: {Id}", id);
            throw;
        }
    }
}
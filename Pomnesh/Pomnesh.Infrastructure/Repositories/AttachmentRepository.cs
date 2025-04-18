using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;


namespace Pomnesh.Infrastructure.Repositories;

public class AttachmentRepository : IBaseRepository<Attachment>
{
    private readonly DapperContext _context;
    public AttachmentRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<int> Add(Attachment attachment)
    {
        var sql = @"
        INSERT INTO ""Attachments"" (""Type"", ""FileId"", ""OwnerId"", ""OriginalLink"", ""ContextId"") 
        VALUES (@Type, @FileId, @OwnerId, @OriginalLink, @ContextId)
        RETURNING ""Id""";
        using (var connection = _context.CreateConnection())
        {
            return await connection.ExecuteScalarAsync<int>(sql, attachment);
        }
    }

    public async Task<Attachment?> GetById(long id)
    {
        var sql = @"SELECT ""Id"", ""Type"", ""FileId"", ""OwnerId"", ""OriginalLink"", ""ContextId""  FROM ""Attachments"" WHERE ""Id"" = @id";
        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Attachment>(sql, new { id });
        }
    }

    public async Task<IEnumerable<Attachment>> GetAll()
    {
        var sql = @"SELECT ""Id"", ""Type"", ""FileId"", ""OwnerId"", ""OriginalLink"", ""ContextId"" FROM ""Attachments""";
        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<Attachment>(sql);
            return result;
        }
    }

    public async Task Update(Attachment attachment)
    {
        var sql = @"
        UPDATE ""Attachments"" 
        SET
            ""Type"" = @Type,
            ""FileId"" = @FileId, 
            ""OwnerId"" = @OwnerId,
            ""OriginalLink"" = @OriginalLink,
            ""ContextId"" = @ContextId
        WHERE ""Id"" = @Id";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(sql, attachment);
        }
    }

    public async Task Delete(long id)
    {
        var sql = @"
        DELETE
        FROM ""Attachments""
        WHERE
            ""Id"" = @id
        ";
    
        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(sql, new { id });
        }
    }
}
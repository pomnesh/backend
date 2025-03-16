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

    public async Task AddAsync(Attachment attachment)
    {
        var sql = "INSERT INTO Attachments (Type, FileId, OwnerId, OriginalLink, ContextId) VALUES (@Type, @FileId, @OwnerId, @OriginalLink, @ContextId)";
        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(sql, attachment);
        }
    }

    public async Task<Attachment?> GetById(long id)
    {
        var sql = "SELECT * FROM Attachments WHERE Id = @id";
        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Attachment>(sql, new { id });
        }
    }
}
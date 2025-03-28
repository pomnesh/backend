using Dapper;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Infrastructure.Repositories;

public class ChatContextRepository : IBaseRepository<ChatContext>
{
    private readonly DapperContext _context;
    public ChatContextRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<int> Add(ChatContext chatContext)
    {
        var sql = @"
            INSERT INTO ChatContexts (MessageId, MessageText, MessageDate)
            VALUES (@MessageId, @MessageText, @MessageDate)
            RETURNING Id;
        ";
        using (var connection = _context.CreateConnection())
        {
            return await connection.ExecuteScalarAsync<int>(sql, chatContext);
        }
    }

    public async Task<ChatContext?> GetById(long id)
    {
        var sql = "SELECT Id, MessageId, MessageText, MessageDate  FROM ChatContexts WHERE Id = @id";
        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<ChatContext>(sql, new { id });
        }
    }
}
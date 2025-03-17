using Dapper;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Infrastructure.Repositories;

public class ContextRepository : IBaseRepository<Context>
{
    private readonly DapperContext _context;
    public ContextRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(Context context)
    {
        var sql = @"
            INSERT INTO Contexts (MessageId, MessageText, MessageDate)
            VALUES (@MessageId, @MessageText, @MessageDate)
            RETURNING Id;
        ";
        using (var connection = _context.CreateConnection())
        {
            return await connection.ExecuteScalarAsync<int>(sql, context);
        }
    }

    public async Task<Context?> GetById(long id)
    {
        var sql = "SELECT * FROM Contexts WHERE Id = @id";
        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Context>(sql, new { id });
        }
    }
}
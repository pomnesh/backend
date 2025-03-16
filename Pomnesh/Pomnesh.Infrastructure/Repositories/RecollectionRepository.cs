using Dapper;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Infrastructure.Repositories;

public class RecollectionRepository : IBaseRepository<Recollection>
{
    private readonly DapperContext _context;
    public RecollectionRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Recollection recollection)
    {
        var sql = "INSERT INTO Recollections (UserId, DownloadLink) VALUES (@UserId, @DownloadLink)";
        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(sql, recollection);
        }
    }

    public async Task<Recollection?> GetById(long id)
    {
        var sql = "SELECT * FROM Recollections WHERE Id = @id";
        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Recollection>(sql, new { id });
        }
    }
}
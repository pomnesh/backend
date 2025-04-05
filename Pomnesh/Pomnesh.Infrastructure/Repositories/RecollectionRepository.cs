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

    public async Task<int> Add(Recollection recollection)
    {
        var sql = @"
            INSERT INTO ""Recollections"" (""UserId"", ""DownloadLink"") 
            VALUES (@UserId, @DownloadLink)
            RETURNING ""Id"";
        ";
        using (var connection = _context.CreateConnection())
        {
            return await connection.ExecuteScalarAsync<int>(sql, recollection);
        }
    }

    public async Task<Recollection?> GetById(long id)
    {
        var sql = @"SELECT ""Id"", ""UserId"", ""CreatedAt"", ""DownloadLink""  FROM ""Recollections"" WHERE ""Id"" = @id";
        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<Recollection>(sql, new { id });
        }
    }
    
    public async Task<IEnumerable<Recollection>> GetAll()
    {
        var sql = @"SELECT ""Id"", ""UserId"", ""CreatedAt"", ""DownloadLink"" FROM ""Recollections"";";
        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<Recollection>(sql);
        }
    }

    public async Task Update(Recollection recollection)
    {
        var sql = @"
        UPDATE ""Recollections""
        SET
            ""UserId"" = @UserId,
            ""CreatedAt"" = @CreatedAt,
            ""DownloadLink"" = @DownloadLink
        WHERE
            ""Id"" = @Id
        ";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(sql, recollection);
        }
    }

    public async Task Delete(long id)
    {
        var sql = @"
        DELETE
        FROM ""Recollections""
        WHERE
            ""Id"" = @id
        ";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(sql, new { id });
        }
    }
}
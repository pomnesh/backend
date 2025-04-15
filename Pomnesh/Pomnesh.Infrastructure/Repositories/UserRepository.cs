using Dapper;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;

namespace Pomnesh.Infrastructure.Repositories;


public class UserRepository : IBaseRepository<User>
{
    private readonly DapperContext _context;
    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<int> Add(User user)
    {
        var sql = @"
            INSERT INTO ""Users"" (""VkId"", ""VkToken"")
            VALUES (@VkId, @VkToken)
            RETURNING ""Id"";
        ";
        using (var connection = _context.CreateConnection())
        {
            return await connection.ExecuteAsync(sql, user);
        }
    }

    public async Task<User?> GetById(long id)
    {
        var sql = @"SELECT ""Id"", ""VkId"", ""VkToken"" FROM ""Users"" WHERE ""Id"" = @id";
        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { id });
        }
    }
    
    public async  Task<IEnumerable<User>> GetAll()
    {
        var sql = @"SELECT ""Id"", ""VkId"", ""VkToken"" FROM ""Users""";
        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryAsync<User>(sql);
        }
    }

    public async Task Update(User user)
    {
        var sql = @"
        UPDATE ""Users""
        SET
            ""VkId"" = @VkId,
            ""VkToken"" = @VkToken
        WHERE
            ""Id"" = @Id
        ";

        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(sql, user);
        }
    }
    
    public async Task Delete(long id)
    {
        var sql = @"
        DELETE
        FROM ""Users""
        WHERE
            ""Id"" = @id
        ";
        
        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(sql, new { id });
        }
    }
}

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
            INSERT INTO Users (VkId, VkToken)
            VALUES (@VkId, @VkToken)
            RETURNING Id;
        ";
        using (var connection = _context.CreateConnection())
        {
            return await connection.ExecuteAsync(sql, user);
        }
    }

    public async Task<User?> GetById(long id)
    {
        var sql = "SELECT Id, VkId, VkToken FROM Users WHERE Id = @id";
        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { id });
        }
    }
}

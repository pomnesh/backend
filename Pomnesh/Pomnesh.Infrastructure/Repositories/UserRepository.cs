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

    public async Task AddAsync(User user)
    {
        var sql = "INSERT INTO Users (VkId, VkToken) VALUES (@VkId, @VkToken)";
        using (var connection = _context.CreateConnection())
        {
            await connection.ExecuteAsync(sql, user);
        }
    }

    public async Task<User?> GetById(long id)
    {
        var sql = "SELECT * FROM Users WHERE Id = @id";
        using (var connection = _context.CreateConnection())
        {
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { id });
        }
    }
}

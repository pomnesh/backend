using Dapper;
using Pomnesh.Domain.Entity;
using Pomnesh.Infrastructure.Interfaces;
using Serilog;

namespace Pomnesh.Infrastructure.Repositories;

public class UserRepository : IBaseRepository<User>
{
    private readonly DapperContext _context;
    private readonly ILogger _logger;

    public UserRepository(DapperContext context)
    {
        _context = context;
        _logger = Log.ForContext<UserRepository>();
    }

    public async Task<int> Add(User user)
    {
        _logger.Information("Adding new user with Username: {Username}", user.Username);
        var sql = @"
            INSERT INTO ""Users"" (""Username"", ""Email"", ""PasswordHash"", ""VkId"", ""VkToken"", ""CreatedAt"")
            VALUES (@Username, @Email, @PasswordHash, @VkId, @VkToken, @CreatedAt)
            RETURNING ""Id"";
        ";
        
        try
        {
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.ExecuteScalarAsync<int>(sql, user);
                _logger.Information("Successfully added user with Id: {Id}", id);
                return id;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to add user with Username: {Username}", user.Username);
            throw;
        }
    }

    public async Task<User?> GetById(long id)
    {
        _logger.Debug("Retrieving user with Id: {Id}", id);
        var sql = @"
            SELECT ""Id"", ""Username"", ""Email"", ""PasswordHash"", ""VkId"", ""VkToken"", ""CreatedAt"", ""LastLoginAt"" 
            FROM ""Users"" 
            WHERE ""Id"" = @id";
        
        try
        {
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<User>(sql, new { id });
                if (result == null)
                {
                    _logger.Warning("User with Id: {Id} not found", id);
                }
                else
                {
                    _logger.Debug("Successfully retrieved user with Id: {Id}", id);
                }
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to retrieve user with Id: {Id}", id);
            throw;
        }
    }
    
    public async Task<IEnumerable<User>> GetAll()
    {
        _logger.Debug("Retrieving all users");
        var sql = @"
            SELECT ""Id"", ""Username"", ""Email"", ""PasswordHash"", ""VkId"", ""VkToken"", ""CreatedAt"", ""LastLoginAt"" 
            FROM ""Users""";
        
        try
        {
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<User>(sql);
                _logger.Debug("Retrieved {Count} users", result.Count());
                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to retrieve all users");
            throw;
        }
    }

    public async Task Update(User user)
    {
        _logger.Information("Updating user with Id: {Id}", user.Id);
        var sql = @"
            UPDATE ""Users""
            SET ""Username"" = @Username,
                ""Email"" = @Email,
                ""PasswordHash"" = @PasswordHash,
                ""VkId"" = @VkId,
                ""VkToken"" = @VkToken,
                ""LastLoginAt"" = @LastLoginAt
            WHERE ""Id"" = @Id";
        
        try
        {
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, user);
                _logger.Information("Successfully updated user with Id: {Id}", user.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to update user with Id: {Id}", user.Id);
            throw;
        }
    }

    public async Task Delete(long id)
    {
        _logger.Information("Deleting user with Id: {Id}", id);
        var sql = @"DELETE FROM ""Users"" WHERE ""Id"" = @id";
        
        try
        {
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(sql, new { id });
                _logger.Information("Successfully deleted user with Id: {Id}", id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to delete user with Id: {Id}", id);
            throw;
        }
    }
}

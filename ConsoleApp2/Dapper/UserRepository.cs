using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Dapper;

using SimpleOrmApplication.Models;

namespace SimpleOrmApplication.Dapper;

public class UserRepository
{
    private readonly IDbConnection _dbConnection;

    public UserRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<User> GetAsync(int id)
    {
        const string sql = @"
            SELECT Id, Name, Email, Active 
            FROM Users 
            WHERE Id = @Id";

        return await _dbConnection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        const string sql = @"
            SELECT Id, Name, Email, Active 
            FROM Users 
            WHERE Active = true 
            ORDER BY Name";

        return await _dbConnection.QueryAsync<User>(sql);
    }

    public async Task<int> CreateAsync(User user)
    {
        const string sql = @"
            INSERT INTO Users (Name, Email, Active) 
            VALUES (@Name, @Email, @Active)
            RETURNING Id";  // Используем RETURNING для PostgreSQL

        var userId = await _dbConnection.ExecuteScalarAsync<int>(sql, user);
        return userId;
    }

    public async Task<bool> UpdateAsync(int id, User user)
    {
        const string sql = @"
            UPDATE Users 
            SET Name = @Name, 
                Email = @Email, 
                Active = @Active 
            WHERE Id = @Id";

        user.Id = id;
        var affectedRows = await _dbConnection.ExecuteAsync(sql, user);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = @"
            UPDATE Users 
            SET Active = false 
            WHERE Id = @Id";
        
        var affectedRows = await _dbConnection.ExecuteAsync(sql, new { Id = id });
        return affectedRows > 0;
    }

    public async Task<User> GetUserWithOrdersAsync(int id)
    {
        const string sql = @"
            SELECT u.*, o.*
            FROM Users u
            LEFT JOIN Orders o ON u.Id = o.user_id
            WHERE u.Id = @Id";

        User user = null;
        await _dbConnection.QueryAsync<User, Order, User>(
            sql,
            (u, o) =>
            {
                if (user == null)
                {
                    user = u;
                }

                if (o != null)
                {
                    user.Orders.Add(o);
                }
                
                return null; 
            },
            new { Id = id },
            splitOn: "Id");

        return user;
    }
}
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SimpleOrmApplication.Models;

namespace SimpleOrmApplication.Dapper;

public class OrderRepository
{
    private readonly IDbConnection _dbConnection;

    public OrderRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Order> GetAsync(int id)
    {
        const string sql = @"
            SELECT Id, Created, Name, Active, user_id as UserId 
            FROM Orders 
            WHERE Id = @Id";

        return await _dbConnection.QueryFirstOrDefaultAsync<Order>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        const string sql = @"
            SELECT Id, Created, Name, Active, user_id as UserId 
            FROM Orders 
            WHERE Active = true 
            ORDER BY Created";

        return await _dbConnection.QueryAsync<Order>(sql);
    }

    public async Task<int> CreateAsync(Order order)
    {
        const string sql = @"
            INSERT INTO Orders (Created, Name, Active, user_id) 
            VALUES (@Created, @Name, @Active, @UserId)
            RETURNING Id";

        var orderId = await _dbConnection.ExecuteScalarAsync<int>(sql, order);
        return orderId;
    }

    public async Task<bool> UpdateAsync(int id, Order order)
    {
        const string sql = @"
            UPDATE Orders 
            SET Created = @Created, 
                Name = @Name, 
                Active = @Active,
                user_id = @UserId
            WHERE Id = @Id";

        order.Id = id;
        var affectedRows = await _dbConnection.ExecuteAsync(sql, order);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = @"
            UPDATE Orders 
            SET Active = false 
            WHERE Id = @Id";
        
        var affectedRows = await _dbConnection.ExecuteAsync(sql, new { Id = id });
        return affectedRows > 0;
    }
}


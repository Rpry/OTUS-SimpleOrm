using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LinqToDB;

using SimpleOrmApplication.Models;

namespace SimpleOrmApplication.Linq2Db;

public class UserRepository
{
    private readonly AppDataConnection _dbConnection;

    public UserRepository(AppDataConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<int> CreateAsync(User user)
    {
        user.Id = await _dbConnection.InsertWithInt32IdentityAsync(user);
        return user.Id;
    }

    public async Task<User?> GetAsync(int id)
    {
        ITable<User> users = _dbConnection.GetTable<User>();
        
        return await users
            .LoadWith(user => user.Orders)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> UpdateAsync(int id, User user)
    {
        ITable<User> table = _dbConnection.GetTable<User>();
        var userToUpdate = await table.FirstOrDefaultAsync(u => u.Id == id);

        if (userToUpdate == null)
        {
            return false;
        }

        userToUpdate.Name = user.Name;
        userToUpdate.Email = user.Email;

        await _dbConnection.UpdateAsync(userToUpdate);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        ITable<User> table = _dbConnection.GetTable<User>();
        var user = await table.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return true;
        }

        var affected = await table.DeleteAsync();
        //user.Active = false;
        //var affected = await _dbConnection.UpdateAsync(user);
        return affected > 0;
    }

    public async Task<IEnumerable<User?>> GetActiveAsync()
    {
        IQueryable<User> query = _dbConnection.GetTable<User>();
        query = query
                .Where(u => u.Active == true);

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<User?>> GetActive2Async()
    {
        IQueryable<User> query = _dbConnection.GetTable<User>();
        return query
            .ToList()
            .Where(u => u.Active == true);

        //return await query
        //            .ToListAsync();
    }
}
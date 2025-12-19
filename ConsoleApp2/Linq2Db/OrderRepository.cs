using System.Threading.Tasks;
using LinqToDB;
using SimpleOrmApplication.Models;

namespace SimpleOrmApplication.Linq2Db
{
    public class OrderRepository
    {
        private readonly AppDataConnection _dbConnection;

        public OrderRepository(AppDataConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> CreateAsync(Order order)
        {
            order.Id = await _dbConnection.InsertWithInt32IdentityAsync(order);
            return order.Id;
        }

        public async Task<Order?> GetAsync(int id)
        {
            ITable<Order> table = _dbConnection.GetTable<Order>();
            return await table
                //.LoadWith(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<bool> UpdateAsync(int id, Order order)
        {
            ITable<Order> table = _dbConnection.GetTable<Order>();
            var orderToUpdate = await table.FirstOrDefaultAsync(o => o.Id == id);

            if (orderToUpdate == null)
            {
                return false;
            }

            orderToUpdate.Name = order.Name;
            orderToUpdate.Active = order.Active;
            orderToUpdate.UserId = order.UserId;

            await _dbConnection.UpdateAsync(orderToUpdate);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            ITable<Order> table = _dbConnection.GetTable<Order>();
            var order = await table.FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return true;
            }

            order.Active = false;
            var affected = await _dbConnection.UpdateAsync(order);
            return affected > 0;
        }
    }
}
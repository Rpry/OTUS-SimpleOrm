using System;
using System.Threading.Tasks;

using SimpleOrmApplication.Models;

namespace SimpleOrmApplication.Linq2Db;

public static class Linq2DbExperiments
{
    public static async Task Do1()
    {
        await using var connection = new AppDataConnection();
        UserRepository repo = new UserRepository(connection);
        var user = new User
        {
            Active = true,
            Email = "myEmail",
            Name = "myName",
        };
        var createdUserId = await repo.CreateAsync(user);
        var savedUser = await repo.GetAsync(createdUserId);
        user.Name = "myNewName";
        user.Email = "myNewEmail";
        var updateResult = await repo.UpdateAsync(createdUserId, user);
        savedUser = await repo.GetAsync(createdUserId);
        await repo.DeleteAsync(createdUserId);
        savedUser = await repo.GetAsync(createdUserId);
    }

    public static async Task Do2()
    {
        await using var connection = new AppDataConnection();
        OrderRepository orderRepository = new OrderRepository(connection);
        var userId = 10001017;
        var order = new Order
        {
            Active = true,
            Created = DateTime.UtcNow,
            Name = "myName",
            UserId = userId
        };
        var createdOrderId = await orderRepository.CreateAsync(order);
        var createdOrder = await orderRepository.GetAsync(createdOrderId);

        UserRepository userRepository = new UserRepository(connection);
        var user = await userRepository.GetAsync(userId);
    }

    public static async Task DeferredExecution()
    {
        await using var connection = new AppDataConnection();
        UserRepository repo = new UserRepository(connection);
        var savedUser = await repo.GetActiveAsync();
    }
}
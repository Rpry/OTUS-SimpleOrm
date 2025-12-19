using System;
using System.Threading.Tasks;

using Npgsql;

using SimpleOrmApplication.Models;

namespace SimpleOrmApplication.Dapper;

public static class DapperExperiments
{
    public static async Task Do1()
    {
        await using var connection = new NpgsqlConnection("Host=localhost;Port=5432;Database=PureDb;UserId=postgres;Password=password");
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
        var updateResult = await repo.UpdateAsync(createdUserId, user);
        savedUser = await repo.GetAsync(createdUserId);
        await repo.DeleteAsync(createdUserId);
        savedUser = await repo.GetAsync(createdUserId);
    }

    public static async Task Do2()
    {
        await using var connection = new NpgsqlConnection("Host=localhost;Port=5432;Database=PureDb;UserId=postgres;Password=password");

        var userRepo = new UserRepository(connection);
        var user = new User
        {
            Active = true,
            Email = "userForOrder@email.com",
            Name = "UserForOrder",
        };
        var createdUserId = await userRepo.CreateAsync(user);

        var orderRepo = new OrderRepository(connection);
        var order = new Order
        {
            Created = DateTime.UtcNow,
            Name = "MyOrder",
            Active = true,
            UserId = createdUserId
        };

        var createdOrderId = await orderRepo.CreateAsync(order);
        var savedOrder = await orderRepo.GetAsync(createdOrderId);

        order.Name = "MyNewOrderName";
        await orderRepo.UpdateAsync(createdOrderId, order);
        savedOrder = await orderRepo.GetAsync(createdOrderId);

        await orderRepo.DeleteAsync(createdOrderId);
        savedOrder = await orderRepo.GetAsync(createdOrderId);

        await userRepo.DeleteAsync(createdUserId);
    }

    public static async Task Do3()
    {
        await using var connection = new NpgsqlConnection("Host=localhost;Port=5432;Database=PureDb;UserId=postgres;Password=password");
        
        var userRepo = new UserRepository(connection);
        var user = new User
        {
            Active = true,
            Email = "userForOrder@email.com",
            Name = "UserForOrder",
        };
        var createdUserId = await userRepo.CreateAsync(user);

        var orderRepo = new OrderRepository(connection);
        var order1 = new Order
        {
            Created = DateTime.UtcNow,
            Name = "MyOrder1",
            Active = true,
            UserId = createdUserId
        };
        var order2 = new Order
        {
            Created = DateTime.UtcNow,
            Name = "MyOrder2",
            Active = true,
            UserId = createdUserId
        };

        await orderRepo.CreateAsync(order1);
        await orderRepo.CreateAsync(order2);

        var userWithOrders = await userRepo.GetUserWithOrdersAsync(createdUserId);
    }
}
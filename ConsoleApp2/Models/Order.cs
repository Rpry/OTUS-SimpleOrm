using System;

namespace SimpleOrmApplication.Models;

public sealed class Order
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }
    public int UserId { get; set; }

    public User User { get; set; }
}
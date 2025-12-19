using System.Collections.Generic;

namespace SimpleOrmApplication.Models;

public sealed class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool Active { get; set; }
    
    public List<Order> Orders { get; set; } = new List<Order>();
}
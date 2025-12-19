using System;

using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

using SimpleOrmApplication.Models;

namespace SimpleOrmApplication.Linq2Db;

public class AppDataConnection : DataConnection
{
    public AppDataConnection()
        : base(
            ProviderName.PostgreSQL95,
            ConfigurationHelper.GetConnectionString("DefaultConnection"),
            CreateMappingSchema())
    {
        TurnTraceSwitchOn();
        OnTraceConnection = traceInfo =>
        {
            if (traceInfo.SqlText != null)
            {
                Console.WriteLine($"SQL: {traceInfo.SqlText}");
                Console.WriteLine();
            }
        };
    }

    public ITable<User> Users => GetTable<User>();
    public ITable<Order> Orders => GetTable<Order>();

    private static MappingSchema CreateMappingSchema()
    {
        var schema = new MappingSchema();
        var builder = schema.GetFluentMappingBuilder();

        builder.Entity<User>()
            .HasTableName("users")
            .Property(e => e.Id).HasColumnName("id").IsPrimaryKey().IsIdentity()
            .Property(e => e.Name).HasColumnName("name").IsNullable(false)
            .Property(e => e.Email).HasColumnName("email").IsNullable(false)
            .Property(e => e.Active).HasColumnName("active")
            .Association(u => u.Orders,
                u => u.Id, o => o.UserId);

        builder.Entity<Order>()
            .HasTableName("orders")
            .Property(o => o.Id).HasColumnName("id").IsPrimaryKey().IsIdentity()
            .Property(o => o.Created).HasColumnName("created").IsNullable(false)
            .Property(o => o.Name).HasColumnName("name").IsNullable(false)
            .Property(o => o.Active).HasColumnName("active").IsNullable(false)
            .Property(o => o.UserId).HasColumnName("user_id").IsNullable(false);

        return schema;
    }
}
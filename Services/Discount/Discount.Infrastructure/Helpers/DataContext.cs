using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace Discount.Infrastructure.Helpers;

public class DataContext
{
    private DbSettings _dbSettings;

    public DataContext(IOptions<DbSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString = $"Host={_dbSettings.Server}; Database={_dbSettings.Database}; Username={_dbSettings.UserId}; Password={_dbSettings.Password};";
        return new NpgsqlConnection(connectionString);
    }

    public async Task InitAsync()
    {
        await _initDatabase();
        await _initTables();
        await _seedData();
    }

    private async Task _initDatabase()
    {
        // create database if it doesn't exist
        var connectionString = $"Host={_dbSettings.Server}; Database=postgres; Username={_dbSettings.UserId}; Password={_dbSettings.Password};";
        using var connection = new NpgsqlConnection(connectionString);
        var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_dbSettings.Database}';";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);
        if (dbCount == 0)
        {
            var sql = $"CREATE DATABASE \"{_dbSettings.Database}\"";
            await connection.ExecuteAsync(sql);
        }
    }

    private async Task _initTables()
    {
        // create tables if they don't exist
        using var connection = CreateConnection();
        await _initUsers();

        async Task _initUsers()
        {
            var sql = """
                CREATE TABLE IF NOT EXISTS Coupons (
                    Id SERIAL PRIMARY KEY, 
                    ProductName VARCHAR(500) NOT NULL,
                    Description TEXT,
                    Amount INT
                );
            """;
            await connection.ExecuteAsync(sql);
        }
    }

    private async Task _seedData()
    {
        using IDbConnection? connection = CreateConnection();
        await _seedCoupons();

        async Task _seedCoupons()
        {
            var sql = """
                INSERT INTO Coupons(ProductName, Description, Amount) VALUES('Adidas Quick Force Indoor Badminton Shoes', 'Shoe Discount', 500);
            """;
            await connection.ExecuteAsync(sql);

            sql = """
                INSERT INTO Coupons(ProductName, Description, Amount) VALUES('Yonex VCORE Pro 100 A Tennis Racquet (270gm, Strung)', 'Racquet Discount', 700);
            """;
            await connection.ExecuteAsync(sql);
        }
    }
}
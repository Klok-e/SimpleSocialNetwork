using System;
using DataAccess;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Tests
{
    public class SqliteInMemory : IDisposable
    {
        private readonly SqliteConnection _connection;

        public SqliteInMemory()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
        }

        public SocialDbContext InMemoryDatabase()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlite(_connection)
                .Options;
            return new SocialDbContext(options);
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
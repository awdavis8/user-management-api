using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserManagementAPI.Data;
using UserManagementAPI.Models;

namespace UserManagementAPI.Tests.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private SqliteConnection? _connection;

        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Find the existing DbContextOptions<AppDbContext> registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                // Remove it if found, so we can add our own
                if (descriptor != null)
                    services.Remove(descriptor);

                // Create and open a new in-memory SQLite connection
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                // Register AppDbContext to use the in-memory SQLite connection
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite(_connection)); 

                // Build the service provider to resolve services
                var sp = services.BuildServiceProvider();

                // Create a scope to get a DbContext instance
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Ensure the database schema is created
                db.Database.EnsureCreated();
            });
        }

        public CustomWebApplicationFactory Seed(IEnumerable<User> users)
        {
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Users.RemoveRange(db.Users);
            db.Users.AddRange(users);
            db.SaveChanges();

            return this;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _connection?.Dispose();
                _connection = null;
            }
        }
    }
}
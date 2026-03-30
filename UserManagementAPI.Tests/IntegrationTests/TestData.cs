using System;
using System.Collections.Generic;
using UserManagementAPI.Models;

namespace UserManagementAPI.Tests.IntegrationTests
{
    internal static class TestData
    {
        private static readonly List<User> _twentyUsers = new List<User>
        {
            new User { Name = "Alice Johnson", Email = "alice.johnson@example.com", DateOfBirth = new DateTime(1990, 5, 12), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Bob Smith", Email = "bob.smith@example.com", DateOfBirth = new DateTime(1985, 3, 22), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Carla Gomez", Email = "carla.gomez@example.com", DateOfBirth = new DateTime(1992, 7, 8), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "David Lee", Email = "david.lee@example.com", DateOfBirth = new DateTime(1988, 11, 30), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Emma Brown", Email = "emma.brown@example.com", DateOfBirth = new DateTime(1995, 1, 15), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Frank Miller", Email = "frank.miller@example.com", DateOfBirth = new DateTime(1983, 9, 5), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Grace Kim", Email = "grace.kim@example.com", DateOfBirth = new DateTime(1991, 4, 18), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Henry Wilson", Email = "henry.wilson@example.com", DateOfBirth = new DateTime(1987, 6, 27), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Isabel Martinez", Email = "isabel.martinez@example.com", DateOfBirth = new DateTime(1993, 2, 10), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Jack Chen", Email = "jack.chen@example.com", DateOfBirth = new DateTime(1989, 8, 3), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Karen Patel", Email = "karen.patel@example.com", DateOfBirth = new DateTime(1994, 12, 21), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Liam O'Brien", Email = "liam.obrien@example.com", DateOfBirth = new DateTime(1986, 10, 14), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Mia Rossi", Email = "mia.rossi@example.com", DateOfBirth = new DateTime(1990, 3, 19), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Noah Dubois", Email = "noah.dubois@example.com", DateOfBirth = new DateTime(1984, 7, 25), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Olivia Novak", Email = "olivia.novak@example.com", DateOfBirth = new DateTime(1992, 5, 2), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Paul Schmidt", Email = "paul.schmidt@example.com", DateOfBirth = new DateTime(1987, 11, 9), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Quinn Murphy", Email = "quinn.murphy@example.com", DateOfBirth = new DateTime(1991, 6, 13), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Rosa Silva", Email = "rosa.silva@example.com", DateOfBirth = new DateTime(1985, 9, 28), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Samir Ali", Email = "samir.ali@example.com", DateOfBirth = new DateTime(1993, 1, 7), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new User { Name = "Tina Nguyen", Email = "tina.nguyen@example.com", DateOfBirth = new DateTime(1996, 4, 23), CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        public static List<User> OneUser => _twentyUsers.GetRange(0, 1);

        public static List<User> FiveUsers => _twentyUsers.GetRange(0, 5);

        public static List<User> TwentyUsers => [.. _twentyUsers];

        public static List<User> OneHundredFiftyUsers
        {
            get
            {
                var users = new List<User>();
                for (int i = 1; i <= 150; i++)
                {
                    users.Add(new User
                    {
                        Name = $"Test User {i}",
                        Email = $"testuser{i}@example.com",
                        DateOfBirth = DateTime.UtcNow.AddYears(-20).AddDays(i),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    });
                }
                return users;
            }
        }
    }
}

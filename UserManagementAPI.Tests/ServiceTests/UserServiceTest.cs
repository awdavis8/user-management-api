using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using UserManagementAPI.Data;
using UserManagementAPI.DTOs;
using UserManagementAPI.Models;
using UserManagementAPI.Services;
using Xunit;

namespace UserManagementAPI.Tests.ServiceTests
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _repoMock;
        private readonly UserService _service;
        private int _nextId = 1;

        public UserServiceTest()
        {
            _repoMock = new Mock<IUserRepository>();
            _service = new UserService(_repoMock.Object);

            // Default AddAsync mock: assign incrementing Id
            _repoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
                .Callback<User>(u => u.Id = _nextId++)
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task CreateUserAsync_Success()
        {
            var dto = new CreateUserDto
            {
                Name = "Alice",
                Email = "alice@example.com",
                DateOfBirth = DateTime.UtcNow.AddYears(-20)
            };

            _repoMock.Setup(r => r.EmailExistsAsync(dto.Email, null)).ReturnsAsync(false);
            _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _service.CreateUserAsync(dto);

            Assert.False(result.IsFailure);
            Assert.NotNull(result.Value);
            Assert.Equal(dto.Name, result.Value.Name);
            Assert.Equal(dto.Email, result.Value.Email);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact]
        public async Task CreateUserAsync_Fails_When_Underage()
        {
            var dto = new CreateUserDto
            {
                Name = "Bob",
                Email = "bob@example.com",
                DateOfBirth = DateTime.UtcNow.AddYears(-17)
            };

            var result = await _service.CreateUserAsync(dto);

            Assert.True(result.IsFailure);
            Assert.NotNull(result.Errors);
            Assert.True(result.Errors.ContainsKey("DateOfBirth"));
            Assert.Contains("User must be at least 18 years of age.", result.Errors["DateOfBirth"]);
        }

        [Fact]
        public async Task CreateUserAsync_Fails_When_EmailNotUnique()
        {
            var dto = new CreateUserDto
            {
                Name = "Charlie",
                Email = "charlie@example.com",
                DateOfBirth = DateTime.UtcNow.AddYears(-25)
            };

            _repoMock.Setup(r => r.EmailExistsAsync(dto.Email, null)).ReturnsAsync(true);

            var result = await _service.CreateUserAsync(dto);

            Assert.True(result.IsFailure);
            Assert.NotNull(result.Errors);
            Assert.True(result.Errors.ContainsKey("Email"));
            Assert.Contains("A user with this email address already exists.", result.Errors["Email"]);
        }

        [Fact]
        public async Task GetUserByIdAsync_Returns_User()
        {
            var user = new User
            {
                Id = _nextId++,
                Name = "Dana",
                Email = "dana@example.com",
                DateOfBirth = DateTime.UtcNow.AddYears(-22)
            };
            _repoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);

            var result = await _service.GetUserByIdAsync(user.Id);

            Assert.False(result.IsFailure);
            Assert.Equal(user.Name, result.Value!.Name);
        }

        [Fact]
        public async Task GetUserByIdAsync_Returns_Failure_When_NotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User?)null);

            var result = await _service.GetUserByIdAsync(999);

            Assert.True(result.IsFailure);
            Assert.NotNull(result.Errors);
            Assert.True(result.Errors.ContainsKey("Id"));
            Assert.Contains("User not found.", result.Errors["Id"]);
        }

        [Fact]
        public async Task UpdateUserAsync_Success()
        {
            var user = new User
            {
                Id = _nextId++,
                Name = "Eve",
                Email = "eve@example.com",
                DateOfBirth = DateTime.UtcNow.AddYears(-19)
            };
            var updateDto = new UpdateUserDto
            {
                Name = "Eve Updated",
                Email = "eve.updated@example.com",
                DateOfBirth = DateTime.UtcNow.AddYears(-19)
            };

            _repoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _repoMock.Setup(r => r.EmailExistsAsync(updateDto.Email, user.Id)).ReturnsAsync(false);
            _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _service.UpdateUserAsync(user.Id, updateDto);

            Assert.False(result.IsFailure);
            Assert.Equal("Eve Updated", result.Value!.Name);
            Assert.Equal("eve.updated@example.com", result.Value.Email);
        }

        [Fact]
        public async Task UpdateUserAsync_Fails_When_IdNotFound()
        {
            var updateDto = new UpdateUserDto
            {
                Name = "Ghost",
                Email = "ghost@example.com",
                DateOfBirth = DateTime.UtcNow.AddYears(-20)
            };

            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User?)null);

            var result = await _service.UpdateUserAsync(999, updateDto);

            Assert.True(result.IsFailure);
            Assert.NotNull(result.Errors);
            Assert.True(result.Errors.ContainsKey("Id"));
            Assert.Contains("User not found.", result.Errors["Id"]);
        }

        [Fact]
        public async Task UpdateUserAsync_Fails_When_EmailNotUnique()
        {
            var user = new User
            {
                Id = _nextId++,
                Name = "Francis",
                Email = "francis@example.com",
                DateOfBirth = DateTime.UtcNow.AddYears(-22)
            };
            var updateDto = new UpdateUserDto
            {
                Name = "Francis Updated",
                Email = "frank@example.com",
                DateOfBirth = DateTime.UtcNow.AddYears(-22)
            };

            _repoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _repoMock.Setup(r => r.EmailExistsAsync(updateDto.Email, user.Id)).ReturnsAsync(true);

            var result = await _service.UpdateUserAsync(user.Id, updateDto);

            Assert.True(result.IsFailure);
            Assert.NotNull(result.Errors);
            Assert.True(result.Errors.ContainsKey("Email"));
            Assert.Contains("A user with this email address already exists.", result.Errors["Email"]);
        }

        [Fact]
        public async Task UpdateUserAsync_Fails_When_Underage()
        {
            var user = new User
            {
                Id = _nextId++,
                Name = "Youngster",
                Email = "youngster@example.com",
                DateOfBirth = DateTime.UtcNow.AddYears(-19)
            };
            var updateDto = new UpdateUserDto
            {
                Name = "Youngster",
                Email = "youngster@example.com",
                DateOfBirth = DateTime.UtcNow.AddYears(-17) // Too young
            };

            _repoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _repoMock.Setup(r => r.EmailExistsAsync(updateDto.Email, user.Id)).ReturnsAsync(false);

            var result = await _service.UpdateUserAsync(user.Id, updateDto);

            Assert.True(result.IsFailure);
            Assert.NotNull(result.Errors);
            Assert.True(result.Errors.ContainsKey("DateOfBirth"));
            Assert.Contains("User must be at least 18 years of age.", result.Errors["DateOfBirth"]);
        }

        [Fact]
        public async Task DeleteUserAsync_Success()
        {
            var user = new User
            {
                Id = _nextId++,
                Name = "Grace",
                Email = "grace@example.com",
                DateOfBirth = DateTime.UtcNow.AddYears(-23)
            };

            _repoMock.Setup(r => r.GetByIdAsync(user.Id)).ReturnsAsync(user);
            _repoMock.Setup(r => r.RemoveAsync(user)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _service.DeleteUserAsync(user.Id);

            Assert.False(result.IsFailure);
        }

        [Fact]
        public async Task DeleteUserAsync_Fails_When_NotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User?)null);

            var result = await _service.DeleteUserAsync(999);

            Assert.True(result.IsFailure);
            Assert.NotNull(result.Errors);
            Assert.True(result.Errors.ContainsKey("Id"));
            Assert.Contains("User not found.", result.Errors["Id"]);
        }

        [Fact]
        public async Task GetAllUsersAsync_Returns_All_Users()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = _nextId++,
                    Name = "Henry",
                    Email = "henry@example.com",
                    DateOfBirth = DateTime.UtcNow.AddYears(-24)
                },
                new User
                {
                    Id = _nextId++,
                    Name = "Helen",
                    Email = "helen@example.com",
                    DateOfBirth = DateTime.UtcNow.AddYears(-25)
                }
            };

            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);

            var result = await _service.GetAllUsersAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetUsersAsync_Pagination_And_Sorting()
        {
            var usersAsc = new List<User>
            {
                new User
                {
                    Id = _nextId++,
                    Name = "Isaac",
                    Email = "isaac@example.com",
                    DateOfBirth = DateTime.UtcNow.AddYears(-26)
                }
            };
            var usersDesc = new List<User>
            {
                new User
                {
                    Id = _nextId++,
                    Name = "Ivy",
                    Email = "ivy@example.com",
                    DateOfBirth = DateTime.UtcNow.AddYears(-27)
                }
            };

            _repoMock.Setup(r => r.GetUsersAsync(1, 1, "name", "asc")).ReturnsAsync(usersAsc);
            _repoMock.Setup(r => r.GetUsersAsync(1, 1, "name", "desc")).ReturnsAsync(usersDesc);

            var users = await _service.GetUsersAsync(page: 1, pageSize: 1, sortBy: "name", sortOrder: "asc");
            Assert.Single(users);
            Assert.Equal("Isaac", users.First().Name);

            var usersDescResult = await _service.GetUsersAsync(page: 1, pageSize: 1, sortBy: "name", sortOrder: "desc");
            Assert.Single(usersDescResult);
            Assert.Equal("Ivy", usersDescResult.First().Name);
        }

        [Fact]
        public async Task GetUsersAsync_Sorts_By_Name_Ascending_And_Descending()
        {
            var usersAsc = new List<User>
            {
                new User
                {
                    Id = _nextId++,
                    Name = "Anna",
                    Email = "anna@example.com",
                    DateOfBirth = DateTime.UtcNow.AddYears(-25)
                },
                new User
                {
                    Id = _nextId++,
                    Name = "Mike",
                    Email = "mike@example.com",
                    DateOfBirth = DateTime.UtcNow.AddYears(-28)
                },
                new User
                {
                    Id = _nextId++,
                    Name = "Zoe",
                    Email = "zoe@example.com",
                    DateOfBirth = DateTime.UtcNow.AddYears(-30)
                }
            };
            var usersDesc = usersAsc.OrderByDescending(u => u.Name).ToList();

            _repoMock.Setup(r => r.GetUsersAsync(null, null, "name", "asc")).ReturnsAsync(usersAsc);
            _repoMock.Setup(r => r.GetUsersAsync(null, null, "name", "desc")).ReturnsAsync(usersDesc);

            var asc = await _service.GetUsersAsync(null, null, "name", "asc");
            Assert.Equal(["Anna", "Mike", "Zoe"], asc.Select(u => u.Name));

            var desc = await _service.GetUsersAsync(null, null, "name", "desc");
            Assert.Equal(["Zoe", "Mike", "Anna"], desc.Select(u => u.Name));
        }

        [Fact]
        public async Task GetUsersAsync_Sorts_By_Age_Ascending_And_Descending()
        {
            var usersAsc = new List<User>
            {
                new User
                {
                    Id = _nextId++,
                    Name = "Young",
                    Email = "young@example.com",
                    DateOfBirth = DateTime.UtcNow.AddYears(-20)
                },
                new User
                {
                    Id = _nextId++,
                    Name = "Middle",
                    Email = "middle@example.com",
                    DateOfBirth = DateTime.UtcNow.AddYears(-25)
                },
                new User
                {
                    Id = _nextId++,
                    Name = "Old",
                    Email = "old@example.com",
                    DateOfBirth = DateTime.UtcNow.AddYears(-30)
                }
            };
            var usersDesc = usersAsc.OrderBy(u => u.DateOfBirth).ToList();

            _repoMock.Setup(r => r.GetUsersAsync(null, null, "age", "asc")).ReturnsAsync(usersAsc);
            _repoMock.Setup(r => r.GetUsersAsync(null, null, "age", "desc")).ReturnsAsync(usersDesc);

            var asc = await _service.GetUsersAsync(null, null, "age", "asc");
            Assert.Equal(["Young", "Middle", "Old"], asc.Select(u => u.Name));

            var desc = await _service.GetUsersAsync(null, null, "age", "desc");
            Assert.Equal(["Old", "Middle", "Young"], desc.Select(u => u.Name));
        }

        [Fact]
        public async Task GetUsersAsync_Sorts_By_Email_Ascending_And_Descending()
        {
            var usersAsc = new List<User>
            {
                new User
                {
                    Id = _nextId++,
                    Name = "User2",
                    Email = "a@example.com",
                    DateOfBirth = DateTime.UtcNow.AddYears(-25)
                },
                new User
                {
                    Id = _nextId++,
                    Name = "User3",
                    Email = "b@example.com",
                    DateOfBirth = DateTime.UtcNow.AddYears(-30)
                },
                new User
                {
                    Id = _nextId++,
                    Name = "User1",
                    Email = "c@example.com",
                    DateOfBirth = DateTime.UtcNow.AddYears(-20)
                }
            };
            var usersDesc = usersAsc.OrderByDescending(u => u.Email).ToList();

            _repoMock.Setup(r => r.GetUsersAsync(null, null, "email", "asc")).ReturnsAsync(usersAsc);
            _repoMock.Setup(r => r.GetUsersAsync(null, null, "email", "desc")).ReturnsAsync(usersDesc);

            var asc = await _service.GetUsersAsync(null, null, "email", "asc");
            Assert.Equal(["a@example.com", "b@example.com", "c@example.com"], asc.Select(u => u.Email));

            var desc = await _service.GetUsersAsync(null, null, "email", "desc");
            Assert.Equal(["c@example.com", "b@example.com", "a@example.com"], desc.Select(u => u.Email));
        }
    }
}
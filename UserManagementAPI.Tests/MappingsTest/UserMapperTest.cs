using UserManagementAPI.Mappings;
using UserManagementAPI.Models;
using UserManagementAPI.DTOs;

namespace UserManagementAPI.Tests.MappingsTest
{
    public class UserMapperTest
    {
        [Fact]
        public void ToResponseDto_MapsUserToDtoCorrectly()
        {
            var user = new User
            {
                Id = 1,
                Name = "Alice",
                Email = "alice@example.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                CreatedAt = new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2021, 1, 1, 12, 0, 0, DateTimeKind.Utc)
            };

            var dto = UserMapper.ToResponseDto(user);

            Assert.Equal(user.Id, dto.Id);
            Assert.Equal(user.Name, dto.Name);
            Assert.Equal(user.Email, dto.Email);
            Assert.Equal(user.DateOfBirth, dto.DateOfBirth);
            Assert.Equal(user.CreatedAt, dto.CreatedAt);
            Assert.Equal(user.UpdatedAt, dto.UpdatedAt);
        }

        [Fact]
        public void ToResponseDtos_MapsUserCollectionToDtoCollection()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Alice",
                    Email = "alice@example.com",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    CreatedAt = new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2021, 1, 1, 12, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    Id = 2,
                    Name = "Bob",
                    Email = "bob@example.com",
                    DateOfBirth = new DateTime(1985, 5, 5),
                    CreatedAt = new DateTime(2020, 2, 2, 12, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2021, 2, 2, 12, 0, 0, DateTimeKind.Utc)
                },
                new User
                {
                    Id = 3,
                    Name = "Carol",
                    Email = "carol@example.com",
                    DateOfBirth = new DateTime(1975, 7, 7),
                    CreatedAt = new DateTime(2020, 3, 3, 12, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2021, 3, 3, 12, 0, 0, DateTimeKind.Utc)
                }
            };

            var dtos = UserMapper.ToResponseDtos(users).ToList();

            Assert.Equal(users.Count, dtos.Count);

            for (int i = 0; i < users.Count; i++)
            {
                Assert.Equal(users[i].Id, dtos[i].Id);
                Assert.Equal(users[i].Name, dtos[i].Name);
                Assert.Equal(users[i].Email, dtos[i].Email);
                Assert.Equal(users[i].DateOfBirth, dtos[i].DateOfBirth);
                Assert.Equal(users[i].CreatedAt, dtos[i].CreatedAt);
                Assert.Equal(users[i].UpdatedAt, dtos[i].UpdatedAt);
            }
        }

        [Fact]
        public void ToModel_MapsCreateUserDtoToUser()
        {
            var dto = new CreateUserDto
            {
                Name = "Charlie",
                Email = "charlie@example.com",
                DateOfBirth = new DateTime(2000, 2, 2)
            };

            var user = UserMapper.ToModel(dto);

            Assert.Equal(dto.Name, user.Name);
            Assert.Equal(dto.Email, user.Email);
            Assert.Equal(dto.DateOfBirth, user.DateOfBirth);
        }

        [Fact]
        public void ApplyUpdate_UpdatesUserWithUpdateUserDto()
        {
            var user = new User
            {
                Name = "Dave",
                Email = "dave@example.com",
                DateOfBirth = new DateTime(1995, 3, 3),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            };
            var dto = new UpdateUserDto
            {
                Name = "Bob",
                Email = "bob@example.com",
                DateOfBirth = new DateTime(1995, 3, 3)
            };

            var beforeUpdate = user.UpdatedAt;
            UserMapper.ApplyUpdate(dto, user);

            Assert.Equal(dto.Name, user.Name);
            Assert.Equal(dto.Email, user.Email);
            Assert.Equal(dto.DateOfBirth, user.DateOfBirth);
            Assert.True(user.UpdatedAt > beforeUpdate);
        }
    }
}

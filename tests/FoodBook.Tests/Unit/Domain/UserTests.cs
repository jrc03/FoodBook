using FoodBook.Domain.Entities;
using FoodBook.Domain.Enums;

namespace FoodBook.Tests.Unit.Domain;

public class UserTests
{
    [Fact]
    public void User_ShouldInitializeWithDefaultValues()
    {
        // Act
        var user = new User();

        // Assert
        Assert.Equal(0, user.Id);
        Assert.Equal(string.Empty, user.FullName);
        Assert.Equal(string.Empty, user.Email);
        Assert.Equal(string.Empty, user.PasswordHash);
        Assert.Null(user.PhoneNumber);
        Assert.Equal(UserRole.Cliente, user.Role);
        Assert.True(user.IsActive);
        Assert.Null(user.UpdatedAt);
        Assert.True((DateTime.UtcNow - user.CreatedAt).TotalSeconds < 5);
    }

    [Fact]
    public void User_ShouldAllowSettingAndGettingProperties()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            FullName = "Juan Perez",
            Email = "juan.perez@example.com",
            PasswordHash = "hashed_pw_123",
            PhoneNumber = "+18091234567",
            Role = UserRole.Cliente,
            IsActive = true,
            UpdatedAt = DateTime.UtcNow
        };

        // Assert
        Assert.Equal(1, user.Id);
        Assert.Equal("Juan Perez", user.FullName);
        Assert.Equal("juan.perez@example.com", user.Email);
        Assert.Equal("hashed_pw_123", user.PasswordHash);
        Assert.Equal("+18091234567", user.PhoneNumber);
        Assert.Equal(UserRole.Cliente, user.Role);
        Assert.True(user.IsActive);
        Assert.NotNull(user.UpdatedAt);
    }

    [Theory]
    [InlineData(UserRole.Cliente, UserRole.Customer)]
    [InlineData(UserRole.Propietario, UserRole.Owner)]
    [InlineData(UserRole.Admin, UserRole.Administrator)]
    public void UserRole_AliasesShouldMatchExpectedValues(UserRole spanishRole, UserRole englishRole)
    {
        Assert.Equal(spanishRole, englishRole);
    }
}

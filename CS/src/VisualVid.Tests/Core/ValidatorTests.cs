using Xunit;
using VisualVid.Core.Helpers;

namespace VisualVid.Tests.Core;

public class ValidatorTests
{
    [Theory]
    [InlineData("user@example.com", true)]
    [InlineData("user.name@example.co.uk", true)]
    [InlineData("user+tag@example.com", true)]
    [InlineData("user-name@example.com", true)]
    [InlineData("user@sub.domain.com", true)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("notanemail", false)]
    [InlineData("@example.com", false)]
    [InlineData("user@", false)]
    [InlineData("user@.com", false)]
    public void IsValidEmail_ReturnsExpected(string email, bool expected)
    {
        Assert.Equal(expected, Validator.IsValidEmail(email));
    }

    [Fact]
    public void IsValidEmail_Null_ReturnsFalse()
    {
        Assert.False(Validator.IsValidEmail(null!));
    }
}

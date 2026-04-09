using Xunit;
using VisualVid.Core.Security;

namespace VisualVid.Tests.Core;

public class HashHelperTests
{
    [Fact]
    public void CreateSalt_ReturnsBase64String()
    {
        var salt = HashHelper.CreateSalt();
        Assert.False(string.IsNullOrWhiteSpace(salt));

        // Should be valid base64
        var bytes = Convert.FromBase64String(salt);
        Assert.Equal(16, bytes.Length);
    }

    [Fact]
    public void CreateSalt_CustomSize_ReturnsCorrectLength()
    {
        var salt = HashHelper.CreateSalt(32);
        var bytes = Convert.FromBase64String(salt);
        Assert.Equal(32, bytes.Length);
    }

    [Fact]
    public void CreateSalt_GeneratesUniqueSalts()
    {
        var salt1 = HashHelper.CreateSalt();
        var salt2 = HashHelper.CreateSalt();
        Assert.NotEqual(salt1, salt2);
    }

    [Theory]
    [InlineData(HashAlgorithmType.MD5)]
    [InlineData(HashAlgorithmType.SHA1)]
    [InlineData(HashAlgorithmType.SHA256)]
    [InlineData(HashAlgorithmType.SHA384)]
    [InlineData(HashAlgorithmType.SHA512)]
    public void ComputeHash_WithSalt_ProducesNonEmptyHash(HashAlgorithmType algorithm)
    {
        var salt = HashHelper.CreateSalt();
        var hash = HashHelper.ComputeHash("password123", algorithm, salt);
        Assert.False(string.IsNullOrWhiteSpace(hash));
    }

    [Theory]
    [InlineData(HashAlgorithmType.MD5)]
    [InlineData(HashAlgorithmType.SHA1)]
    [InlineData(HashAlgorithmType.SHA256)]
    [InlineData(HashAlgorithmType.SHA384)]
    [InlineData(HashAlgorithmType.SHA512)]
    public void VerifyHash_CorrectPassword_ReturnsTrue(HashAlgorithmType algorithm)
    {
        var salt = HashHelper.CreateSalt();
        var hash = HashHelper.ComputeHash("MyPassword", algorithm, salt);
        Assert.True(HashHelper.VerifyHash("MyPassword", algorithm, hash));
    }

    [Theory]
    [InlineData(HashAlgorithmType.MD5)]
    [InlineData(HashAlgorithmType.SHA1)]
    [InlineData(HashAlgorithmType.SHA256)]
    [InlineData(HashAlgorithmType.SHA384)]
    [InlineData(HashAlgorithmType.SHA512)]
    public void VerifyHash_WrongPassword_ReturnsFalse(HashAlgorithmType algorithm)
    {
        var salt = HashHelper.CreateSalt();
        var hash = HashHelper.ComputeHash("MyPassword", algorithm, salt);
        Assert.False(HashHelper.VerifyHash("WrongPassword", algorithm, hash));
    }

    [Fact]
    public void ComputeHash_WithoutSalt_ProducesConsistentHash()
    {
        var hash1 = HashHelper.ComputeHash("test", HashAlgorithmType.SHA256);
        var hash2 = HashHelper.ComputeHash("test", HashAlgorithmType.SHA256);
        // Without salt, same input should NOT produce same output because
        // the method embeds an empty salt. Both calls with no salt should match.
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void ComputeHash_DifferentAlgorithms_ProduceDifferentHashes()
    {
        var salt = HashHelper.CreateSalt();
        var md5 = HashHelper.ComputeHash("test", HashAlgorithmType.MD5, salt);
        var sha256 = HashHelper.ComputeHash("test", HashAlgorithmType.SHA256, salt);
        Assert.NotEqual(md5, sha256);
    }
}

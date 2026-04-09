using System.Security.Cryptography;
using System.Text;

namespace VisualVid.Core.Security;

public enum HashAlgorithmType
{
    MD5,
    SHA1,
    SHA256,
    SHA384,
    SHA512
}

public static class HashHelper
{
    public static string CreateSalt(int size = 16)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(size);
        return Convert.ToBase64String(saltBytes);
    }

    public static string ComputeHash(string plainText, HashAlgorithmType algorithm, string? salt = null)
    {
        byte[] saltBytes = salt != null ? Convert.FromBase64String(salt) : [];

        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];

        Buffer.BlockCopy(plainTextBytes, 0, plainTextWithSaltBytes, 0, plainTextBytes.Length);
        Buffer.BlockCopy(saltBytes, 0, plainTextWithSaltBytes, plainTextBytes.Length, saltBytes.Length);

        byte[] hashBytes;
        using (var hash = CreateAlgorithm(algorithm))
        {
            hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
        }

        byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];
        Buffer.BlockCopy(hashBytes, 0, hashWithSaltBytes, 0, hashBytes.Length);
        Buffer.BlockCopy(saltBytes, 0, hashWithSaltBytes, hashBytes.Length, saltBytes.Length);

        return Convert.ToBase64String(hashWithSaltBytes);
    }

    public static bool VerifyHash(string plainText, HashAlgorithmType algorithm, string hashValue)
    {
        byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

        int hashSizeInBits = algorithm switch
        {
            HashAlgorithmType.MD5 => 128,
            HashAlgorithmType.SHA1 => 160,
            HashAlgorithmType.SHA256 => 256,
            HashAlgorithmType.SHA384 => 384,
            HashAlgorithmType.SHA512 => 512,
            _ => 0
        };

        int hashSizeInBytes = hashSizeInBits / 8;

        if (hashWithSaltBytes.Length < hashSizeInBytes)
            return false;

        byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];
        Buffer.BlockCopy(hashWithSaltBytes, hashSizeInBytes, saltBytes, 0, saltBytes.Length);

        string salt = Convert.ToBase64String(saltBytes);
        string expectedHashString = ComputeHash(plainText, algorithm, salt);

        return string.Equals(hashValue, expectedHashString, StringComparison.Ordinal);
    }

    private static HashAlgorithm CreateAlgorithm(HashAlgorithmType algorithm)
    {
        return algorithm switch
        {
            HashAlgorithmType.MD5 => MD5.Create(),
            HashAlgorithmType.SHA1 => SHA1.Create(),
            HashAlgorithmType.SHA256 => SHA256.Create(),
            HashAlgorithmType.SHA384 => SHA384.Create(),
            HashAlgorithmType.SHA512 => SHA512.Create(),
            _ => SHA256.Create()
        };
    }
}

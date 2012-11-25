using System;
using System.Text;
using System.Security.Cryptography;

namespace DES
{
    namespace Security
    {
        public enum Algorithm : int
        {
            MD5 = 128,
            SHA1 = 160,
            SHA256 = 256,
            SHA384 = 384,
            SHA512 = 512
        };

        public abstract class Hashing
        {
            public static byte[] CreateSalt(int intSize)
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] byteBuff = new byte[intSize];
                rng.GetBytes(byteBuff);
                return byteBuff;
            }

            public static string ComputeHash(string strText, Algorithm algo, byte[] byteSalt)
            {
                HashAlgorithm algoHash;
                if (byteSalt == null)
                {
                    byteSalt = CreateSalt(255);
                }

                byte[] byteText = Encoding.UTF8.GetBytes(strText);
                byte[] byteTextSalt = new byte[byteText.Length + byteSalt.Length];
                byteText.CopyTo(byteTextSalt, 0);
                byteSalt.CopyTo(byteTextSalt, byteText.Length);

                switch (algo)
                {
                    case Algorithm.MD5:
                        algoHash = new MD5CryptoServiceProvider();
                        break;
                    case Algorithm.SHA1:
                        algoHash = new SHA1Managed();
                        break;
                    case Algorithm.SHA256:
                        algoHash = new SHA256Managed();
                        break;
                    case Algorithm.SHA384:
                        algoHash = new SHA384Managed();
                        break;
                    case Algorithm.SHA512:
                        algoHash = new SHA512Managed();
                        break;
                    default:
                        algoHash = new MD5CryptoServiceProvider();
                        break;
                }

                byte[] byteHash = algoHash.ComputeHash(byteTextSalt);
                byte[] byteHashSalt = new byte[byteHash.Length + byteSalt.Length];
                byteHash.CopyTo(byteHashSalt, 0);
                byteSalt.CopyTo(byteHashSalt, byteHash.Length);

                return Convert.ToBase64String(byteHashSalt);
            }

            public static string ComputeHash(string strText, Algorithm algo)
            {
                HashAlgorithm algoHash;
                switch (algo)
                {
                    case Algorithm.MD5:
                        algoHash = new MD5CryptoServiceProvider();
                        break;
                    case Algorithm.SHA1:
                        algoHash = new SHA1Managed();
                        break;
                    case Algorithm.SHA256:
                        algoHash = new SHA256Managed();
                        break;
                    case Algorithm.SHA384:
                        algoHash = new SHA384Managed();
                        break;
                    case Algorithm.SHA512:
                        algoHash = new SHA512Managed();
                        break;
                    default:
                        algoHash = new MD5CryptoServiceProvider();
                        break;
                }
                byte[] byteHash = algoHash.ComputeHash(Encoding.UTF8.GetBytes(strText));
                return Convert.ToBase64String(byteHash);
            }

            public static bool VerifyHash(string strText, Algorithm algo, string strHash, bool IsSalted)
            {
                if (IsSalted)
                {
                    int intBits = (int)algo;
                    int intBytes;
                    intBytes = intBits / 8;
                    try
                    {
                        byte[] byteHashSalt = Convert.FromBase64String(strHash);
                        byte[] byteSalt = new byte[byteHashSalt.Length - intBytes];
                        for (int x = 0; x < byteSalt.Length; x++)
                        {
                            byteSalt[x] = byteHashSalt[intBytes + x];
                        }
                        string strMatch = ComputeHash(strText, algo, byteSalt);
                        return (strHash == strMatch);
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    try
                    {
                        string strMatch = ComputeHash(strText, algo);
                        return (strHash == strMatch);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
        }
    }
}
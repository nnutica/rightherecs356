using System;
using System.Security.Cryptography;
using System.Text;

namespace Righthere_Demo.Services;

public class HashPassword
{
    public static string GetHash256(string input)
    {
        var sBuilder = new StringBuilder();

        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
        }
        return sBuilder.ToString();
    }

    public static bool VerifyHash256(string input, string hash)
    {
        string hashOfInput = GetHash256(input);

        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        return comparer.Compare(hashOfInput, hash) == 0;
    }

}

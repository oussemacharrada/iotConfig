using System.Security.Cryptography;
using System.Text;

namespace SuitSolution.Services.Utility;

public class UniqueIdGenerator
{
    private readonly byte[] _namespace;

    public UniqueIdGenerator()
    {
        _namespace = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8").ToByteArray();
    }

    public Guid Generate(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentNullException(nameof(input));


        var formattedString = input.ToLower();
        var formattedBytes = Encoding.UTF8.GetBytes(formattedString);

        var combinedBytes = new byte[_namespace.Length + formattedBytes.Length];
        _namespace.CopyTo(combinedBytes, 0);
        formattedBytes.CopyTo(combinedBytes, _namespace.Length);

        using var md5 = MD5.Create();
        var hashBytes = md5.ComputeHash(combinedBytes);


        var guidFromMD5 = new Guid(hashBytes);

        return guidFromMD5;
    }
}
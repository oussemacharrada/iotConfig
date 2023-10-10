using System;
using System.IO;
using System.Security.Cryptography;

public class FileHashResult
{
    public byte[] Hash { get; set; }
    public long FileSize { get; set; }


public static FileHashResult HashFile(string filePath, string algorithmName)
{
    long fileSize = 0;
    byte[] hashValue;

    using (var algorithm = HashAlgorithm.Create(algorithmName))
    {
        if (algorithm == null)
        {
            throw new NotSupportedException($"Hash algorithm '{algorithmName}' is not supported.");
        }

        using (var stream = File.OpenRead(filePath))
        {
            fileSize = stream.Length;
            hashValue = algorithm.ComputeHash(stream);
        }
    }

    return new FileHashResult
    {
        Hash = hashValue,
        FileSize = fileSize
    };
}
}
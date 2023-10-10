
    using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
    using PeterO.Cbor;
    using Xunit;
/*
public class ImplementationTests
{
    [Fact]
    public void ToSUIT_SerializesCorrectly()
    {
        SUITEnvelope suitEnvelope = new SUITEnvelope();
        suitEnvelope.InitializeRandomData();

        CBORObject cborObject = suitEnvelope.ToSUIT("sha256");

        Assert.True(cborObject.ContainsKey("auth"));
        Assert.True(cborObject.ContainsKey("manifest"));

        using (var stream = new FileStream("C:/Users/ousama.charada/OneDrive - ML!PA Consulting GmbH/Desktop/suit/suit-manifest-generator/TestCase/object.cbor", FileMode.Create))
        {
            byte[] cborBytes = cborObject.EncodeToBytes();

            byte[] hash = ComputeHash(cborBytes, "sha256");

            stream.Write(hash, 0, hash.Length);

            byte[] newline = System.Text.Encoding.UTF8.GetBytes(Environment.NewLine);
            stream.Write(newline, 0, newline.Length);

            stream.Write(cborBytes, 0, cborBytes.Length);
        }
    }

    private byte[] ComputeHash(byte[] data, string algorithmName)
    {
        using (var algorithm = HashAlgorithm.Create(algorithmName))
        {
            if (algorithm == null)
            {
                throw new InvalidOperationException($"Hash algorithm '{algorithmName}' not found.");
            }

            return algorithm.ComputeHash(data);
        }
    }
  
    [Fact]
    public void CheckCBORData()
    {
        byte[] cborData;
        try
        {
            cborData = File.ReadAllBytes("C:/Users/ousama.charada/OneDrive - ML!PA Consulting GmbH/Desktop/suit/suit-manifest-generator/TestCase/object.cbor");
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException("File not found");
        }

        CBORObject cborObject = CBORObject.DecodeFromBytes(cborData);

        Assert.True(cborObject.Type == CBORType.Map, "The decoded CBOR data is not a dictionary");

        foreach (CBORObject key in cborObject.Keys)
        {
            CBORObject value = cborObject[key];
            Console.WriteLine($"Key: {key}");

            if (value.HasOneTag(24))
            {
                CBORObject taggedValue = CBORObject.FromObject(value.MostOuterTag);
                Console.WriteLine($"Tagged Value: {taggedValue}");
            }
            else
            {
                Console.WriteLine($"Value: {value}");
            }
        }
    }
}

*/
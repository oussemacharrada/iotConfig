using System;
using System.IO;
using System.Linq;
using Xunit;

public class TLVFileGenerationTest
{
    [Fact]
    public void GenerateTLVFile()
    {
        string filePath = @"C:\Users\ousama.charada\OneDrive - ML!PA Consulting GmbH\Desktop\suit\suit-manifest-generator\TestCase\sample.tlv";

        byte[] tag1Data = GenerateRandomData();
        byte[] tag2Data = GenerateRandomData();

        byte[] tag1TLV = CreateTLV(1, tag1Data);
        byte[] tag2TLV = CreateTLV(2, tag2Data);

        byte[] tlvData = tag1TLV.Concat(tag2TLV).ToArray();

        // Write the TLV data to the file
        File.WriteAllBytes(filePath, tlvData);

        // Perform any assertions or additional checks as needed
        Assert.True(File.Exists(filePath));
    }

    // Helper method to generate random data
    private byte[] GenerateRandomData()
    {
        Random random = new Random();
        byte[] data = new byte[16]; // Adjust the size as needed
        random.NextBytes(data);
        return data;
    }

    // Helper method to create TLV bytes
    private byte[] CreateTLV(uint tag, byte[] data)
    {
        using (MemoryStream stream = new MemoryStream())
        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            // Write the tag (1 byte)
            writer.Write((byte)tag);

            // Write the length (2 bytes)
            ushort length = (ushort)data.Length;
            writer.Write(BitConverter.GetBytes(length));

            // Write the data
            writer.Write(data);

            return stream.ToArray();
        }
    }
}
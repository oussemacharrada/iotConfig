using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Xunit;

public class TlvParserTests
{
    [Fact]
    public void TestExtractAndConvertTlvString()
    {
        string tlvHexData = "0102030405060708";
        byte[] tlvBytes = ConvertHexStringToByteArray(tlvHexData);

        using (MemoryStream stream = new MemoryStream(tlvBytes))
        {
            Dictionary<string, object> extractedData = ExtractAndConvertTlvString(stream);

            Assert.NotNull(extractedData);
            Assert.True(extractedData.ContainsKey("Key1"));
            Assert.True(extractedData.ContainsKey("Key2"));

            foreach (var kvp in extractedData)
            {
                Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
            }
        }
    }

    private static Dictionary<string, object> ExtractAndConvertTlvString(Stream stream)
    {
        Dictionary<string, object> tlvData = new Dictionary<string, object>
        {
            { "Key1", "Value1" },
            { "Key2", 12345 }
        };


        return tlvData;
    }

    private static byte[] ConvertHexStringToByteArray(string hex)
    {
        hex = hex.Replace("-", "");
        int length = hex.Length / 2;
        byte[] bytes = new byte[length];
        for (int i = 0; i < length; i++)
        {
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }
        return bytes;
    }
}

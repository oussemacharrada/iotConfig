using System;
using System.Collections.Generic;
using System.IO;
using PeterO.Cbor;

public class SuitManifestParser
{
    public static Dictionary<string, object> ParseSuitManifest(string filePath)
    {
        Dictionary<string, object> manifest = new Dictionary<string, object>();

        try
        {
            byte[] inputBytes;
            
            string fileContent = File.ReadAllText(filePath).Trim();
            if (fileContent.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                fileContent = fileContent.Substring(2);
                
                inputBytes = HexStringToBytes(fileContent);
            }
            else
            {
                inputBytes = File.ReadAllBytes(filePath);
            }

            var cbor = CBORObject.DecodeFromBytes(inputBytes, CBOREncodeOptions.Default);

            ParseCborRecursive(cbor, manifest);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading CBOR data: {ex.Message}");
        }

        return manifest;
    }

 

   private static void ParseCborRecursive(CBORObject cborObject, Dictionary<string, object> manifest, string indent = "")
{
    if (cborObject == null)
    {
        return;
    }

    if (cborObject.Type == CBORType.Map)
    {
        foreach (CBORObject key in cborObject.Keys)
        {
            object keyValue = null;

            if (key.Type == CBORType.TextString)
            {
                keyValue = key.AsString();
            }
            else if (key.Type == CBORType.ByteString)
            {
                keyValue = key.ToJSONString();
            }

            CBORObject value = cborObject[key];

            if (value != null)
            {
                object parsedValue = null;

                if (value.Type == CBORType.Array || value.Type == CBORType.Map)
                {
                    Dictionary<string, object> nestedManifest = new Dictionary<string, object>();
                    ParseCborRecursive(value, nestedManifest, indent + "  "); // Increase the indent for nested levels
                    parsedValue = nestedManifest;
                }
                else if (value.Type == CBORType.TextString)
                {
                    parsedValue = value.AsString();
                }
                else if (value.Type == CBORType.ByteString)
                {
                    parsedValue = value.ToJSONString();

                }
                else if (value.Type == CBORType.Integer)
                {
                    parsedValue = value.AsInt64();
                }

                manifest[keyValue?.ToString()] = parsedValue;

                // Log the key-value pair
            }
        }
    }
}

private static bool IsHexString(string hexString)
{
    return hexString.Length % 2 == 0 && System.Text.RegularExpressions.Regex.IsMatch(hexString, @"\A\b[0-9a-fA-F]+\b\Z");
}

// Helper function to convert a hex string to bytes
private static byte[] HexStringToBytes(string hexString)
{
    int length = hexString.Length;
    byte[] bytes = new byte[length / 2];
    for (int i = 0; i < length; i += 2)
    {
        bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
    }
    return bytes;
}





}
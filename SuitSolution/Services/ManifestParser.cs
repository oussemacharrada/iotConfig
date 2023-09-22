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
            var cbor = CBORObject.DecodeFromBytes(File.ReadAllBytes(filePath), CBOREncodeOptions.Default);

            ParseCborRecursive(cbor, manifest);
        }
        catch (Exception ex)    
        {
            Console.WriteLine($"Error reading CBOR data: {ex.Message}");
        }

        return manifest;
    }

    private static void ParseCborRecursive(CBORObject cborObject, Dictionary<string, object> manifest)
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
    
                if (value != null)                 {
                    object parsedValue = null;

                    if (value.Type == CBORType.Array || value.Type == CBORType.Map)
                    {
                        Dictionary<string, object> nestedManifest = new Dictionary<string, object>();
                        ParseCborRecursive(value, nestedManifest);
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
                }
            }
        }
    }



}
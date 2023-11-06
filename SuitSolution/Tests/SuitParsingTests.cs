using System;
using System.Collections.Generic;
using System.IO;
using PeterO.Cbor;
using SuitSolution.Services;
using Xunit;

public class SuitManifestParserTests
{
    [Fact]
    public void ParseSuitManifest_ShouldSucceed()
    {
        string filePath = "C:\\Users\\ousama.charada\\OneDrive - ML!PA Consulting GmbH\\Desktop\\suit\\suit-manifest-generator\\TestCase\\object.mani";
        var cbor = CBORObject.DecodeFromBytes(
            File.ReadAllBytes(filePath), CBOREncodeOptions.Default);
    
        Dictionary<string, object> manifest = SuitManifestParser.ParseSuitManifest(filePath);
            Console.WriteLine("Manifest Data:");
            
    }
    
}
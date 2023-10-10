using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xunit;
using SuitSolution.Services;
/*
public class SUITManifestTests
{
    [Fact]
    public void TestSUITManifestSerialization()
    {
        var manifest = SUITManifestDataSeeder.SeedSUITManifest();

        var suitJson = manifest.ToJson();
        Console.WriteLine("To Json =  "+ suitJson);
    }


    [Fact]
    public void TestSUITManifestSerializationAndDeserialization()
    {
        // Arrange
        var manifest = SUITManifestDataSeeder.SeedSUITManifest();

        // Act
        var suitJson = manifest.ToJson();
        var deserializedManifest = new SUITManifest().FromJson(suitJson);
        Assert.Equal(deserializedManifest.version.v, manifest.version.v);
        Assert.Equal(deserializedManifest.sequence.v, manifest.sequence.v);
        Assert.Equal(deserializedManifest.common.v, manifest.common.v);

        Console.WriteLine("To Json =  "+ deserializedManifest);

    }
    [Fact]
    public void TestSUITConversion()
    {
        // Arrange
        var originalManifest = SUITManifestDataSeeder.SeedSUITManifest();
        var suitJson = originalManifest.ToSUIT();

        // Act
        var convertedManifest = new SUITManifest();
        convertedManifest.FromSUIT(suitJson);

        // Assert
        Console.WriteLine("Original Manifest:");
        Console.WriteLine(originalManifest.ToDebug(""));
        
        Console.WriteLine("Converted Manifest:");
        Console.WriteLine(convertedManifest.ToDebug(""));
        
        // Now, you can add assertions to compare the properties of the two manifests
        Assert.Equal(originalManifest.version.v, convertedManifest.version.v);
        Assert.Equal(originalManifest.sequence.v, convertedManifest.sequence.v);
        // Add more assertions for other properties as needed

        // Example assertion for comparing the 'common' property
        Assert.Equal(originalManifest.common.ToJson(), convertedManifest.common.ToJson());
    }

  
}   */
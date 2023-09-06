using System;
using System.Collections.Generic;
using PeterO.Cbor;
using SuitSolution.Services;
using Xunit;
namespace SuitSolution.Tests;

public class SUITManifestTests
{
    [Fact]
    public void ToJson_SerializesToJson()
    {
        var manifest = new SUITManifest
        {
            ManifestVersion = 1,
            ManifestSequenceNumber = 42,
            Common = new SUITCommon
            {
                Sequence = 100,
                Install = new List<int> { 1, 2, 3 },
                Validate = new List<int> { 4, 5 }
            },
            Components = new List<SUITComponents>
            {
                new SUITComponents {  },
                new SUITComponents {  }
            },
            Dependencies = new List<SUITDependencies>
            {
                new SUITDependencies {  },
                new SUITDependencies {}
            }
        };

        var json = manifest.ToJson();

        Assert.NotNull(json);
    }

  
    [Fact]
    public void ToCBOR_SerializesToCBORObject()
    {
        var manifest = new SUITManifest
        {
            ManifestVersion = 1,
            ManifestSequenceNumber = 42,
            Common = new SUITCommon
            {
                Sequence = 100,
                Install = new List<int> { 1, 2, 3 },
                Validate = new List<int> { 4, 5 }
            },
            Components = new List<SUITComponents>
            {
                new SUITComponents {  },
                new SUITComponents { }
            },
            Dependencies = new List<SUITDependencies>
            {
                new SUITDependencies { },
                new SUITDependencies {  }
            }
        };

        var cborObject = manifest.ToCBOR();

        Assert.NotNull(cborObject);
    }

    [Fact]
    public void FromCBOR_DeserializesFromCBORObject()
    {
        var cborObject = CBORObject.FromObject(new Dictionary<string, object>
        {
            { "manifest-version", 1 },
            { "manifest-sequence-number", 42 },
            { "common", new Dictionary<string, object>
                {
                    { "sequence", 100 },
                    { "install", new List<int> { 1, 2, 3 } },
                    { "validate", new List<int> { 4, 5 } }
                }
            },
            { "components", new List<object> {  } },
            { "dependencies", new List<object> {} }
        });

        var manifest = new SUITManifest();
        manifest.FromCBOR(cborObject);

        Assert.NotNull(manifest);
    }

    [Fact]
    public void ToSUIT_ConvertsToSUITFormat()
    {
        var manifest = new SUITManifest
        {
            ManifestVersion = 1,
            ManifestSequenceNumber = 42,
            Common = new SUITCommon
            {
                Sequence = 100,
                Install = new List<int> { 1, 2, 3 },
                Validate = new List<int> { 4, 5 }
            },
            Components = new List<SUITComponents>
            {
                new SUITComponents {  },
                new SUITComponents { }
            },
            Dependencies = new List<SUITDependencies>
            {
                new SUITDependencies { },
                new SUITDependencies {  }
            }
        };

        var suitList = manifest.ToSUIT();

        Assert.NotNull(suitList);
    }

    [Fact]
    public void FromSUIT_ConvertsFromSUITFormat()
    {
        var common = new SUITCommon
        {
            Sequence = 100,
            Install = new List<int> { 1, 2, 3 },
            Validate = new List<int> { 4, 5 }
        };

        var components = new List<SUITComponents>
        {
            new SUITComponents
            {
            },
        };

        var dependencies = new List<SUITDependencies>
        {
            new SUITDependencies
            {
            },
        };

        var manifest = new SUITManifest
        {
            ManifestVersion = 1,
            ManifestSequenceNumber = 42,
            Common = common,
            Components = components,
            Dependencies = dependencies
        };

        var json = manifest.ToJson();
        var deserializedManifest = SUITManifest.FromJson(json);

        Assert.NotNull(deserializedManifest);
    }

}

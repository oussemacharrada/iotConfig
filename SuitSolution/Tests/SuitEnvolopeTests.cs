using System;
using PeterO.Cbor;
using SuitSolution.Services;
using Xunit;
namespace SuitSolution.Tests;

public class SUITEnvelopeTests : IDisposable
{
    private SUITEnvelope suitEnvelope;

    public SUITEnvelopeTests()
    {
        suitEnvelope = new SUITEnvelope(
            new SUITBWrapField<COSEList>(),
            new SUITBWrapField<SUITManifest>(),
            new SUITBWrapField<DependencyResolution>(),
            new SUITBWrapField<PayloadFetch>(),
            new SUITBWrapField<SUITInstall>(),
            new SUITBWrapField<SUITSequence>(),
            new SUITBWrapField<SUITSequence>(),
            new SUITBWrapField<SUITSequence>(),
            new SUITBWrapField<SUITText>(),
            new SUITBytes()
        );
    }

    [Fact]
    public void GeneratedCBORStructureMatchesExpectations()
    {
        SUITEnvelope suitEnvelope = new SUITEnvelope();
        suitEnvelope.InitializeRandomData();

        CBORObject cborObject = suitEnvelope.ToSUIT("sha256");

        Assert.True(cborObject.ContainsKey("auth"));
        Assert.Equal(CBORType.Map, cborObject["auth"].Type);

        Assert.True(cborObject.ContainsKey("manifest"));
        Assert.Equal(CBORType.Map, cborObject["manifest"].Type);

        Assert.True(cborObject.ContainsKey("deres"));
        Assert.Equal(CBORType.Map, cborObject["deres"].Type);

        Assert.True(cborObject.ContainsKey("fetch"));
        Assert.Equal(CBORType.Map, cborObject["fetch"].Type);

        Assert.True(cborObject.ContainsKey("install"));
        Assert.Equal(CBORType.Map, cborObject["install"].Type);

        Assert.True(cborObject.ContainsKey("validate"));
        Assert.Equal(CBORType.Map, cborObject["validate"].Type);

        Assert.True(cborObject.ContainsKey("load"));
        Assert.Equal(CBORType.Map, cborObject["load"].Type);

        Assert.True(cborObject.ContainsKey("run"));
        Assert.Equal(CBORType.Map, cborObject["run"].Type);

        Assert.True(cborObject.ContainsKey("text"));
        Assert.Equal(CBORType.Map, cborObject["text"].Type);

        Assert.True(cborObject.ContainsKey("coswid"));
        Assert.Equal(CBORType.ByteString, cborObject["coswid"].Type);

      if (cborObject.ContainsKey("auth"))
        {
            var authMap = cborObject["auth"].ToObject<CBORObject>();
            Assert.True(authMap.ContainsKey("key"));
            Assert.Equal(CBORType.TextString, authMap["key"].Type);
        }
    }

    [Fact]
    public void InitializeRandomData_InitializesFields()
    {
        suitEnvelope.InitializeRandomData();
        Assert.NotNull(suitEnvelope.manifest.WrappedObject);
        Assert.NotNull(suitEnvelope.deres.WrappedObject);
        Assert.NotNull(suitEnvelope.fetch.WrappedObject);
        Assert.NotNull(suitEnvelope.install.WrappedObject);
        Assert.NotNull(suitEnvelope.validate.WrappedObject);
        Assert.NotNull(suitEnvelope.load.WrappedObject);
        Assert.NotNull(suitEnvelope.run.WrappedObject);
        Assert.NotNull(suitEnvelope.text.WrappedObject);
        Assert.NotNull(suitEnvelope.coswid.Bytes);
    }

    [Fact]
    public void ToJson_SerializesCorrectly()
    {
        string json = suitEnvelope.ToJson();
        SUITEnvelope deserializedEnvelope = SUITEnvelope.FromJson(json);

        Assert.Equal(suitEnvelope.auth.ToJson(), deserializedEnvelope.auth.ToJson());
        Assert.Equal(suitEnvelope.manifest.ToJson(), deserializedEnvelope.manifest.ToJson());
    }

    [Fact]
    public void FromJson_DeserializesCorrectly()
    {
        suitEnvelope.InitializeRandomData();
        string json = suitEnvelope.ToJson();

        SUITEnvelope deserializedEnvelope = SUITEnvelope.FromJson(json);

        Assert.Equal(suitEnvelope.auth.ToJson(), deserializedEnvelope.auth.ToJson());
        Assert.Equal(suitEnvelope.manifest.ToJson(), deserializedEnvelope.manifest.ToJson());
    }

    [Fact]
    public void ToSUIT_SerializesCorrectly()
    {
        suitEnvelope.InitializeRandomData();
        CBORObject cborObject = suitEnvelope.ToSUIT("sha256");

    }

    [Fact]
    public void Createsuit()
    {
        SUITEnvelope suitEnvelope = new SUITEnvelope();
        suitEnvelope.InitializeRandomData();

        CBORObject cborObject = CBORObject.NewMap();

         cborObject= suitEnvelope.ToSUIT("sha256");
        Console.WriteLine(cborObject.ToJSONString());
        string filePath = @"C:/Users/ousama.charada/OneDrive - ML!PA Consulting GmbH/Desktop/suit/suit-manifest-generator/TestCase/object.mani";

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            byte[] cborBytes = cborObject.EncodeToBytes();
            stream.Write(cborBytes, 0, cborBytes.Length);
        }

    }


    

 [Fact]
 public void FromSUIT_DeserializesCorrectly()
 {
     suitEnvelope.InitializeRandomData();
     CBORObject cborObject = suitEnvelope.ToSUIT("sha256");

 SUITEnvelope.FromSUIT(cborObject);

 }

 public void Dispose()
 {
 }
}

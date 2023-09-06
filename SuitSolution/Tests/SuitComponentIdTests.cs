using PeterO.Cbor;
using Xunit;
using SuitSolution.Services; 
namespace SuitSolution.Tests;

public class SUITComponentIdTests
{
    
    [Fact]
    public void ToJson_SerializesCorrectly()
    {
        var suitComponentId = new SUITComponentId
        {
            VendorId = "vendor123",
            ClassId = "class456",
            ImageDigest = "digest789",
            Id = "id123"
        };

        var json = suitComponentId.ToJson();

        var expectedJson = "{\r\n  \"vendor-id\": \"vendor123\",\r\n  \"class-id\": \"class456\",\r\n  \"image-digest\": \"digest789\",\r\n  \"Id\": \"id123\"\r\n}";
        Assert.Equal(expectedJson, json);
    }

    [Fact]
    public void FromJson_DeserializesCorrectly()
    {
        var json = "{\"vendor-id\":\"vendor123\",\"class-id\":\"class456\",\"image-digest\":\"digest789\",\"Id\":\"id123\"}";

        var suitComponentId = SUITComponentId.FromJson(json);

        Assert.Equal("vendor123", suitComponentId.VendorId);
        Assert.Equal("class456", suitComponentId.ClassId);
        Assert.Equal("digest789", suitComponentId.ImageDigest);
        Assert.Equal("id123", suitComponentId.Id);
    }
    [Fact]
    public void ToSUIT_ConvertsToCBORObject()
    {
        var componentId = new SUITComponentId
        {
            VendorId = "vendor1",
            ClassId = "class1",
            ImageDigest = "digest1",
            Id = "id1"
        };

        var cborObject = componentId.ToSUIT();

        Assert.NotNull(cborObject);
        Assert.Equal(PeterO.Cbor.CBORType.Map, cborObject.Type);

        Assert.True(cborObject.ContainsKey("vendor-id"));
        Assert.True(cborObject.ContainsKey("class-id"));
        Assert.True(cborObject.ContainsKey("image-digest"));
        Assert.True(cborObject.ContainsKey("Id"));

        Assert.Equal("vendor1", cborObject["vendor-id"].AsString());
        Assert.Equal("class1", cborObject["class-id"].AsString());
        Assert.Equal("digest1", cborObject["image-digest"].AsString());
        Assert.Equal("id1", cborObject["Id"].AsString());
    }

    [Fact]
    public void FromSUIT_ConvertsFromCBORObject()
    {
        var cborObject = CBORObject.NewMap()
            .Add("vendor-id", "vendor2")
            .Add("class-id", "class2")
            .Add("image-digest", "digest2")
            .Add("Id", "id2");

        var componentId = SUITComponentId.FromSUIT(cborObject);

        Assert.NotNull(componentId);
        Assert.Equal("vendor2", componentId.VendorId);
        Assert.Equal("class2", componentId.ClassId);
        Assert.Equal("digest2", componentId.ImageDigest);
        Assert.Equal("id2", componentId.Id);
    }
    
}

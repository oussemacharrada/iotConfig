using System;
using System.Collections.Generic;
using PeterO.Cbor;
using Xunit;
using SuitSolution.Services;
namespace SuitSolution.Tests;

public class SUITSequenceTests
{
  

    [Fact]
    public void FromJson_DeserializesCorrectly()
    {
        var json = "{\"seq\":[{\"vendor-id\":\"vendor1\",\"class-id\":\"class1\",\"image-digest\":\"digest1\",\"Id\":\"id1\"},{\"vendor-id\":\"vendor2\",\"class-id\":\"class2\",\"image-digest\":\"digest2\",\"Id\":\"id2\"}]}";

        var sequence = SUITSequence.FromJson(json);

        Assert.NotNull(sequence.Sequence);
        Assert.Equal(2, sequence.Sequence.Count);

        Assert.Equal("vendor1", sequence.Sequence[0].VendorId);
        Assert.Equal("class1", sequence.Sequence[0].ClassId);
        Assert.Equal("digest1", sequence.Sequence[0].ImageDigest);
        Assert.Equal("id1", sequence.Sequence[0].Id);

        Assert.Equal("vendor2", sequence.Sequence[1].VendorId);
        Assert.Equal("class2", sequence.Sequence[1].ClassId);
        Assert.Equal("digest2", sequence.Sequence[1].ImageDigest);
        Assert.Equal("id2", sequence.Sequence[1].Id);
    }

    [Fact]
    public void ToSUIT_ConvertsListToSUIT()
    {
        var sequence = new SUITSequence();
        sequence.Sequence.Add(new SUITComponentId { Id = "id1" });
        sequence.Sequence.Add(new SUITComponentId { Id = "id2" });

        var suitObject = sequence.ToSUIT();

        Assert.NotNull(suitObject);
        Assert.IsType<List<object>>(suitObject); 
    }

    [Fact]
    public void FromSUIT_ConvertsSUITToList()
    {
        var suitList = new List<object>
        {
            new SUITComponentId { VendorId = "vendor1", ClassId = "class1", ImageDigest = "digest1", Id = "id1" },
            new SUITComponentId { VendorId = "vendor2", ClassId = "class2", ImageDigest = "digest2", Id = "id2" }
        };

        var sequence = new SUITSequence();
        sequence.FromSUIT(suitList);

        Assert.NotNull(sequence.Sequence);
        Assert.Equal(2, sequence.Sequence.Count);

        Assert.Equal("vendor1", sequence.Sequence[0].VendorId);
        Assert.Equal("class1", sequence.Sequence[0].ClassId);
        Assert.Equal("digest1", sequence.Sequence[0].ImageDigest);
        Assert.Equal("id1", sequence.Sequence[0].Id);

        Assert.Equal("vendor2", sequence.Sequence[1].VendorId);
        Assert.Equal("class2", sequence.Sequence[1].ClassId);
        Assert.Equal("digest2", sequence.Sequence[1].ImageDigest);
        Assert.Equal("id2", sequence.Sequence[1].Id);
    }
}

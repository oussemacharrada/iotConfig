using PeterO.Cbor;

namespace SuitSolution.Tests;

using System;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;
using SuitSolution.Services;

public class COSEListTests
{
    [Fact]
    public void ToJson_SerializesListDataToJson()
    {
        var coseList = new COSEList();
        coseList.ListData = new CBORObject[]
        {
            CBORObject.FromObject("string"),
            CBORObject.FromObject(42),
            CBORObject.FromObject(true),
            CBORObject.Null
        };

        var json = coseList.ToJson();

        Assert.Contains("string", json);
        Assert.Contains("42", json);
        Assert.Contains("true", json);
        Assert.Contains("null", json);
    }
    [Fact]
    public void FromJson_DeserializesJsonToListData()
    {
        var json = "{\"stringValue\": \"string\", \"intValue\": 42, \"booleanValue\": true, \"nullValue\": null}";
        var coseList = new COSEList();

        coseList.FromJson(json);

        Assert.NotNull(coseList.ListData);
        for (int i = 0; i < coseList.ListData.Length; i++)
        {
            Console.WriteLine($"Element {i}: {coseList.ListData[i].ToString()}");

            if (coseList.ListData[i].IsNull)
            {
                Assert.True(coseList.ListData[i].IsNull);
            }
            else if (coseList.ListData[i].Type == CBORType.TextString)
            {
                Assert.Equal("string", coseList.ListData[i].AsString());
            }
            else if (coseList.ListData[i].Type == CBORType.Integer)
            {
                Assert.Equal(42, coseList.ListData[i].AsInt32());
            }
            else if (coseList.ListData[i].Type == CBORType.Boolean)
            {
                bool booleanValue = coseList.ListData[i].AsBoolean();
                if (booleanValue)
                {
                    Assert.True(booleanValue);
                }
                else
                {
                    Assert.False(booleanValue);
                }
            }
            else
            {
                Assert.True(false, $"Unsupported type for element {i}");
            }
        }
    }


    [Fact]
    public void ToSUIT_ConvertsListDataToSUIT()
    {
        var coseList = new COSEList();
        coseList.ListData = new CBORObject[] { CBORObject.FromObject("string"), CBORObject.FromObject(42), CBORObject.FromObject(true), CBORObject.Null };

        var suitList = coseList.ToSUIT();

        Assert.NotNull(suitList);
        Assert.Equal("string", suitList[0] as string);
        Assert.Equal(42, suitList[1] as int?);
        Assert.Equal(true, suitList[2] as bool?);
        Assert.Null(suitList[3]);
    }


    [Fact]
    public void FromSUIT_ConvertsSUITToListData()
    {
        var suitList = new List<object>
        {
            "string",
            42,
            true,
            null
        };
        var coseList = new COSEList();

        coseList.FromSUIT(suitList);

        Assert.NotNull(coseList.ListData);
        Assert.Equal("string", coseList.ListData[0].AsString());
        Assert.Equal(42, coseList.ListData[1].AsInt32());
        Assert.True(coseList.ListData[2].AsBoolean());
        Assert.True(coseList.ListData[3].IsNull);
    }
}

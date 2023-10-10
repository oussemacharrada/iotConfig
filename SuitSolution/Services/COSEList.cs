using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class COSEList : SUITManifestArray<COSETaggedAuth>, ISUITConvertible<COSEList>
{
    public SUITBWrapField<COSETaggedAuth> field { get; set; }

    public COSEList()
    {
        field = new SUITBWrapField<COSETaggedAuth>
        {
            v = new COSETaggedAuth()
        };
    }

    public Dictionary<string, object> ToSUIT()
    {
        var suitList = new List<object>();
        foreach (var item in items)
        {
            suitList.Add(item.ToSUIT());
        }
        return new Dictionary<string, object>
        {
            { "items", suitList }
        };
    }

    public Dictionary<string, object> ToJson()
    {
        var jsonList = new List<object>();
        foreach (var item in items)
        {
            jsonList.Add(item.ToJson());
        }
        return new Dictionary<string, object>
        {
            { "items", jsonList }
        };
    }

    public string ToDebug(string indent)
    {
        throw new NotImplementedException();
    }

    public COSEList FromSUIT(Dictionary<string, object> suitDict)
    {
        if (suitDict == null || !suitDict.ContainsKey("items"))
        {
            throw new ArgumentException("Invalid SUIT data.");
        }

        var itemsList = (List<object>)suitDict["items"];
        items = new List<COSETaggedAuth>();

        foreach (var item in itemsList)
        {
            var coseTaggedAuth = new COSETaggedAuth();
            coseTaggedAuth.FromSUIT((Dictionary<string, object>)item);
            items.Add(coseTaggedAuth);
        }

        return this;
    }

    public COSEList FromJson(Dictionary<string, object> jsonData)
    {
        if (jsonData == null || !jsonData.ContainsKey("items"))
        {
            throw new ArgumentException("Invalid JSON data.");
        }

        var itemsList = (List<object>)jsonData["items"];
        items = new List<COSETaggedAuth>();

        foreach (var item in itemsList)
        {
            var coseTaggedAuth = new COSETaggedAuth();
            coseTaggedAuth.FromJson((Dictionary<string, object>)item);
            items.Add(coseTaggedAuth);
        }

        return this;
    }
}

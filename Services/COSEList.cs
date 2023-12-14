using System;
using System.Collections.Generic;
using System.Linq;
using PeterO.Cbor;
using SuitSolution.Interfaces;

public class COSEList : SUITManifestArray<COSETaggedAuth>, ISUITConvertible<COSEList>
{
    public COSEList()
    {
        Items = new List<COSETaggedAuth>();
    }

    public new dynamic ToSUIT()
    {
        var suitlist = base.ToSuit();
        return suitlist.ToArray();
    }

    public new Dictionary<string, object> ToJson()
    {
        return new Dictionary<string, object>
        {
            { "items", Items.Select(item => item.ToJson()).ToList() }
        };
    }

    public new string ToDebug(string indent)
    {
        return base.ToDebug(indent);
    }

    public new COSEList FromSUIT(object suitData)
    {
        if (suitData is List<object> itemsList)
        {
            Items = itemsList.Select(item =>
            {
                var coseTaggedAuth = new COSETaggedAuth();
                coseTaggedAuth.FromSUIT(item as Dictionary<object, object>);
                return coseTaggedAuth;
            }).ToList();
        }
        else
        {
            throw new ArgumentException("Invalid SUIT data format for COSEList.");
        }

        return this;
    }

    public COSEList FromSUIT(Dictionary<object, object> suitDict)
    {
        throw new NotImplementedException();
    }

    public new COSEList FromJson(Dictionary<string, object> jsonData)
    {
        if (jsonData.TryGetValue("items", out var items) && items is List<object> itemsList)
        {
            Items = itemsList.Select(item =>
            {
                var coseTaggedAuth = new COSETaggedAuth();
                coseTaggedAuth.FromJson(item as Dictionary<string, object>);
                return coseTaggedAuth;
            }).ToList();
        }
        else
        {
            throw new ArgumentException("Invalid JSON data format for COSEList.");
        }

        return this;
    }
}
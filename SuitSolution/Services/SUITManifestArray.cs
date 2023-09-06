
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;
using SuitSolution.Interfaces;
namespace SuitSolution.Services;

 public interface ISUITManifestItem
    {
        bool Equals(ISUITManifestItem other);
        ISUITManifestItem FromJson(string json);
        string ToJson();
        ISUITManifestItem FromSUIT(CBORObject data);
        CBORObject ToSUIT();
        void Append(ISUITManifestItem element);
        string ToDebug(string indent);
    }

/*public class SUITManifestArray : ISUITManifestItem
{
    protected List<ISUITManifestItem> Items { get; set; }

    public SUITManifestArray()
    {
        Items = new List<ISUITManifestItem>();
    }

    public bool Equals(SUITManifestArray other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (ReferenceEquals(null, other)) return false;
        if (Items.Count != other.Items.Count) return false;

        for (int i = 0; i < Items.Count; i++)
        {
            if (!Items[i].Equals(other.Items[i]))
            {
                return false;
            }
        }

        return true;
    }

    public bool Equals(ISUITManifestItem other)
    {
        throw new NotImplementedException();
    }

    ISUITManifestItem ISUITManifestItem.FromJson(string json)
    {
        return FromJson(json);
    }

    public SUITManifestArray FromJson(string json)
    {
        Items = new List<ISUITManifestItem>();
        var jsonArray = JsonSerializer.Deserialize<List<JsonElement>>(json);

        foreach (var jsonElement in jsonArray)
        {
            var itemType = jsonElement.GetProperty("itemType").GetString();

            ISUITManifestItem newItem;
            switch (itemType)
            {
                case "COSETaggedAuth":
                    newItem = new SUITBWrapField<CoseTaggedAuth>().FromJson(jsonElement.ToString());
                    break;

                default:
                    throw new NotSupportedException($"Unsupported item type: {itemType}");
            }

            Items.Add(newItem);
        }

        return this;
    }

    public string ToJson()
    {
        var jsonElements = new List<JsonElement>();

        foreach (var item in Items)
        {
            if (item is SUITBWrapField<CoseTaggedAuth> coseTaggedAuthItem)
            {
                var jsonElement = coseTaggedAuthItem.ToJsonElement();
                jsonElement.GetProperty("itemType").GetString();
                jsonElements.Add(jsonElement);
            }
        }

        return JsonSerializer.Serialize(jsonElements);
    }

    ISUITManifestItem ISUITManifestItem.FromSUIT(CBORObject data)
    {
        return FromSUIT(data);
    }

    public SUITManifestArray FromSUIT(CBORObject data)
    {
        Items = new List<ISUITManifestItem>();

        foreach (var cborItem in data.Values)
        {
            var itemType = cborItem["itemType"].AsString();

            ISUITManifestItem newItem;
            switch (itemType)
            {
                case "COSETaggedAuth":
                    newItem = new SUITBWrapField<CoseTaggedAuth>().FromSUIT(cborItem);
                    break;

                default:
                    throw new NotSupportedException($"Unsupported item type: {itemType}");
            }

            Items.Add(newItem);
        }

        return this;
    }

    public CBORObject ToSUIT()
    {
        var cborArray = CBORObject.NewArray();

        foreach (var item in Items)
        {
            if (item is SUITBWrapField<CoseTaggedAuth> coseTaggedAuthItem)
            {
                cborArray.Add(coseTaggedAuthItem.ToSUIT());
            }
        }

        return cborArray;
    }

    public void Append(ISUITManifestItem element)
    {
        if (!ReferenceEquals(element.GetType(), typeof(SUITBWrapField<CoseTaggedAuth>)))
        {
            throw new Exception($"element {element} is not a {typeof(SUITBWrapField<CoseTaggedAuth>)}");
        }

        Items.Add(element);
    }

    public string ToDebug(string indent)
    {
        var newIndent = indent + "  ";
        var s = "[\n";

        s += string.Join(",\n", Items.ConvertAll(item => newIndent + item.ToDebug(newIndent))) + "\n";
        s += indent + "]";

        return s;
    }
}*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SuitSolution.Services;
using PeterO.Cbor;
using SuitSolution.Interfaces;

public class COSETaggedAuth : COSETagChoice, ISUITConvertible<COSETaggedAuth>
{
    public COSESign? CoseSign { get; set; }
    public COSESign1? CoseSign1 { get; set; }
    public COSE_Mac CoseMac { get; set; }
    public COSE_Mac0 CoseMac0 { get; set; }

    private readonly ReadOnlyDictionary<string, ManifestKey> fields;

    public COSETaggedAuth()
    {
        fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields(
            ("cose_sign", 96, () => CoseSign,"CoseSign"),
            ("cose_sign1", 18, () => CoseSign1,"CoseSign1"),
            ("cose_mac", 97, () => CoseMac,"CoseMac"),
            ("cose_mac0", 17, () => CoseMac0,"CoseMac0")
        ));
    }

    public dynamic ToSUIT()
    {
        var suitcbor =
            CBORObject.FromObjectAndTag(CoseSign1.ToSUIT(), 18).EncodeToBytes();
        return suitcbor;

    }

    public dynamic ToJson()
    {
        var jsonDict = new Dictionary<string, object>();

        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.JsonKey;
            var fieldValue = GetType().GetProperty(fieldName)?.GetValue(this);

            if (fieldValue != null)
            {
                jsonDict.Add(fieldName, ((ISUITConvertible)fieldValue).ToJson());
            }
        }

        return new Dictionary<string, object>
        {
            { "COSETaggedAuth", jsonDict }
        };
    }

    public COSETaggedAuth FromSUIT(Dictionary<object, object> suitDict)
    {
        if (suitDict == null || !suitDict.ContainsKey("COSETaggedAuth"))
        {
            throw new ArgumentException("Invalid SUIT data.");
        }

        var cborMap = (CBORObject)suitDict["COSETaggedAuth"];

        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.JsonKey;
            var suitKey = fieldInfo.SuitKey;

            if (cborMap.ContainsKey(suitKey))
            {
                var suitValue = cborMap[suitKey];
                var property = GetType().GetProperty(fieldName);
                if (property != null)
                {
                    var value = Activator.CreateInstance(property.PropertyType);
                    if (value is ISUITConvertible convertible)
                    {
                        if (suitValue is CBORObject cborObject)
                        {
                            var dictValue = cborObject.ToObject<Dictionary<string, object>>();
                            convertible.FromSUIT(dictValue);
                            property.SetValue(this, value);
                        }
                    }

                }
            }
        }

        return this;
    }

    public COSETaggedAuth FromJson(Dictionary<string, object> jsonData)
    {
        if (jsonData == null || !jsonData.ContainsKey("COSETaggedAuth"))
        {
            throw new ArgumentException("Invalid JSON data.");
        }

        var jsonDict = (Dictionary<string, object>)jsonData["COSETaggedAuth"];

        foreach (var fieldInfo in fields.Values)
        {
            var fieldName = fieldInfo.JsonKey;

            if (jsonDict.ContainsKey(fieldName))
            {
                var fieldValue = jsonDict[fieldName];
                var property = GetType().GetProperty(fieldName);
                if (property != null)
                {
                    var value = Activator.CreateInstance(property.PropertyType);
                    if (value is ISUITConvertible convertible)
                    {
                        convertible.FromJson((Dictionary<string, object>)fieldValue);
                        property.SetValue(this, value);
                    }
                }
            }
        }

        return this;
    }
}

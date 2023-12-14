using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using PeterO.Cbor;

public class COSETagChoice : SUITManifestDict
{
    private readonly Dictionary<string, ManifestKey> fields;

   
    public new CBORObject ToSUIT()
    {
        foreach (var field in fields)
        {
            var property = this.GetType().GetProperty(field.Key);
            if (property?.GetValue(this) is ISUITManifestObject value)
            {
                var cborElement = value.ToSUIT() as CBORObject;
                if (cborElement != null)
                {
                    return CBORObject.FromObjectAndTag(cborElement, field.Value.SuitKey);
                }
            }
        }
        return null;
    }

  
    public new string ToDebug(string indent)
    {
        foreach (var field in fields)
        {
            var property = this.GetType().GetProperty(field.Key);
            if (property?.GetValue(this) is ISUITManifestObject value)
            {
                string newIndent = indent + "    ";
                return $"{field.Value.SuitKey}({value.ToDebug(newIndent)})";
            }
        }
        return string.Empty;
    }

    // Implement or adjust this method based on your CBOR library
    private int ExtractTagNumber(CBORObject element)
    {
        if (element.IsTagged)
        {
            return element.MostInnerTag.ToInt32Checked();
        }
        throw new InvalidOperationException("Element is not a tagged CBOR object.");
    }
}

public interface ISUITManifestObject
{
    object ToSUIT();
    ISUITManifestObject FromSUIT(object data);
    string ToDebug(string indent);
}

public class ManifestKey
{
    public int SuitKey { get; }
    public Type FieldType { get; }

    public ManifestKey(int suitKey, Type fieldType)
    {
        SuitKey = suitKey;
        FieldType = fieldType;
    }
}

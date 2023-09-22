using PeterO.Cbor;
using System;
using System.Collections.Generic;
using System.Linq;

public static class CBORExtensions
{
    public static Dictionary<object, object> ConvertToDictionary(this CBORObject cborObject)
    {
        if (cborObject == null || cborObject.Type != CBORType.Map)
        {
            throw new ArgumentException("CBORObject is not a map or is null.");
        }

        var dict = new Dictionary<object, object>();
        foreach (var key in cborObject.Keys)
        {
            dict[key.ToObject<object>()] = cborObject[key].ToObject<object>();
        }


        return dict;
    }
}
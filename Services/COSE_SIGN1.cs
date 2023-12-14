using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PeterO.Cbor;
using SuitSolution.Services;

public class COSESign1 : SuitManifestNamedList
{
    public SUITBWrapField<COSEHeaderMap> Protected { get; set; }
    public COSEHeaderMap Unprotected { get; set; }
    public SUITBWrapField<SUITDigest> Payload { get; set; }
    public SUITBytes Signature { get; set; }

    public COSESign1()
    {
        fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields(
            ("protected", 0, () => Protected = new SUITBWrapField<COSEHeaderMap>(new COSEHeaderMap()), "Protected"),
            ("unprotected", 1, () => Unprotected = new COSEHeaderMap(), "Unprotected"),
            ("payload", 2, () => Payload = new SUITBWrapField<SUITDigest>(new SUITDigest()), "Payload"),
            ("signature", 3, () => Signature = new SUITBytes(), "Signature")
        ));
    }
    public dynamic ToSUIT()
    {
        var cborMap = new Dictionary<int,object>();

        if (Protected != null && Protected.v != null)
        {
            cborMap.Add(0, Protected.ToSUIT()); 
        }

        if (Unprotected != null)
        {
            cborMap.Add(1, Unprotected.ToSUIT()); 
        }

        if (Payload != null && Payload.v != null)
        {
            cborMap.Add(2, Payload.ToSUIT()); 
        }

        if (Signature != null && Signature.v != null)
        {
            cborMap.Add(3, Signature.ToSUIT());
        }

        return cborMap;
    }

}
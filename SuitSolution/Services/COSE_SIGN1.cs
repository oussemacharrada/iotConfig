using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SuitSolution.Services;

public class COSESign1 : SUITManifestNamedList
{
    public COSESign1()
    {
        fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields(
            ("protected", "protected", () => new SUITBWrapField<COSEHeaderMap>(new COSEHeaderMap())),
            ("unprotected", "unprotected", () => new COSEHeaderMap()),
            ("payload", "payload", () => new SUITBWrapField<SUITDigest>(new SUITDigest()))
            
        ));
    }
}
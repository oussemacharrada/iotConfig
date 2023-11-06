using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SuitSolution.Services;

public class COSESign1 : SuitManifestNamedList
{
    public COSESign1()
    {
        fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields(
            ("protected", 0, () => new SUITBWrapField<COSEHeaderMap>(new COSEHeaderMap()),"COSEHeaderMap"),
            ("unprotected", 1, () => new COSEHeaderMap(),"COSEHeaderMap"),
            ("payload", 2, () => new SUITBWrapField<SUITDigest>(new SUITDigest()),"SUITDigest")
            
        ));
    }
}
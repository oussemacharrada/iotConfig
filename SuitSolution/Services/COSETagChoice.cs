using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public class COSETagChoice : SUITManifestDict
{
    public COSETagChoice()
    {
        fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields(
            ("field1", 1, () => new COSETagChoiceField1(),"COSETagChoiceField1"),
            ("field2", 2, () => new COSETagChoiceField2(),"COSETagChoiceField2")
        ));
    }

  
}

public class COSETagChoiceField1
{
}

public class COSETagChoiceField2
{
}
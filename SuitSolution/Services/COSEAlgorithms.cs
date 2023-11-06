using System;
using System.Collections.Generic;
using SuitSolution.Services;

public class COSEAlgorithms : SUITKeyMap<object>
{
    public COSEAlgorithms(Dictionary<object, object> keymap) : base(keymap)
    {
    }

    public static COSEAlgorithms CreateDefault()
    {
        return new COSEAlgorithms(new Dictionary<object, object>
        {
            { "ES256", -7 },
            { "ES384", -35 },
            { "ES512", -36 },
            { "EdDSA", -8 },
            { "HSS-LMS", -46 },
        });
    }
}
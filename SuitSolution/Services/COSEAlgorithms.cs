using System;
using System.Collections.Generic;
using SuitSolution.Services;

public class COSEAlgorithms : SUITKeyMap<int>
{
    public COSEAlgorithms(Dictionary<string, int> keymap) : base(keymap)
    {
    }

    public static COSEAlgorithms CreateDefault()
    {
        return new COSEAlgorithms(new Dictionary<string, int>
        {
            { "ES256", -7 },
            { "ES384", -35 },
            { "ES512", -36 },
            { "EdDSA", -8 },
            { "HSS-LMS", -46 },
        });
    }
}
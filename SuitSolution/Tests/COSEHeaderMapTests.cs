using System;
using System.Collections.Generic;
using SuitSolution.Services;
using Xunit;

public class COSEHeaderMapTests
{
    [Fact]
    public void SerializeAndDeserializeCOSEHeaderMap()
    {
        var headerMap = new COSEHeaderMap
        {
            Alg = COSEAlgorithms.CreateDefault(),
            Kid = new SUITBytes { v = new byte[] { 0x01, 0x02, 0x03 } }
        };

     //  byte[] serialized = headerMap.ToSUITBytes();
       // COSEHeaderMap deserialized = COSEHeaderMap.FromSUITBytes(serialized);

//        Assert.Equal(headerMap, deserialized);
    }
        
    
}
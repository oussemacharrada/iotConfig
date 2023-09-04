using System;
using PeterO.Cbor;
using System.Collections.Generic;
using System.Linq;

namespace SuitSolution.Services
{
    public class CoseTaggedAuth
    {
        public COSEHeaderMap Alg { get; set; }
        public byte[] Data { get; set; }

      public static CoseTaggedAuth FromSUIT(CBORObject cborObject)
{
    var taggedAuth = new CoseTaggedAuth();

    if (cborObject.HasTag(2))
    {
        var taggedData = cborObject[2];
        
        if (taggedData.Type == CBORType.ByteString)
        {
            taggedAuth.Alg = COSEHeaderMap.FromSUIT(taggedData["alg"]);
            taggedAuth.Data = taggedData["data"].ToObject<byte[]>();
        }
    }

    return taggedAuth;
}

        public CBORObject ToSUIT()
        {
            CBORObject cborObject = CBORObject.NewMap();
            cborObject.Add("alg", Alg.ToSUIT());
            cborObject.Add("data", CBORObject.FromObject(Data));
            return cborObject;
        }
    }
}
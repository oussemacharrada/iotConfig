using System;
using System.Collections.Generic;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    public class COSEHeaderMap
    {
        public int Alg { get; set; }

        public static COSEHeaderMap FromSUIT(CBORObject cborObject)
        {
            var headerMap = new COSEHeaderMap
            {
                Alg = cborObject[1].AsInt32()
            };
            return headerMap;
        }

        public CBORObject ToSUIT()
        {
            CBORObject cborObject = CBORObject.NewArray();
            cborObject.Add(1);
            cborObject.Add(Alg);
            return cborObject;
        }
    }
}
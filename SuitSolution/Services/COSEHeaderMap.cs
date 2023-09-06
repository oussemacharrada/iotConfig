using System;
using System.Collections.Generic;
using PeterO.Cbor;
using SuitSolution.Services.SuitSolution.Services;

namespace SuitSolution.Services
{
    public class COSEHeaderMap : ISUITConvertible
    {
        public int Alg { get; set; }

        public CBORObject ToCBOR()
        {
            CBORObject cborObject = CBORObject.NewArray();
            cborObject.Add(1);
            cborObject.Add(Alg);
            return cborObject;
        }

        public void FromCBOR(CBORObject cborObject)
        {
            if (cborObject.Type != CBORType.Array || cborObject.Count != 2)
            {
                throw new ArgumentException("Invalid CBOR object or type for COSEHeaderMap.");
            }

            if (cborObject[0].AsInt32() != 1)
            {
                throw new ArgumentException("Invalid CBOR object: unexpected value for COSEHeaderMap tag.");
            }

            Alg = cborObject[1].AsInt32();
        }

        public List<object> ToSUIT()
        {
            var suitList = new List<object>
            {
                1, 
                Alg
            };

            return suitList;
        }

        public void FromSUIT(List<object> suitList)
        {
            if (suitList == null || suitList.Count < 2)
            {
                throw new ArgumentException("Invalid SUIT list for COSEHeaderMap.");
            }

            if ((int)suitList[0] != 1)
            {
                throw new ArgumentException("Invalid SUIT list: unexpected value for COSEHeaderMap tag.");
            }

            Alg = (int)suitList[1];
        }

        public static COSEHeaderMap FromSUIT(CBORObject cborObject)
        {
            if (cborObject.Type != CBORType.Array || cborObject.Count != 2)
            {
                throw new ArgumentException("Invalid CBOR object or type for COSEHeaderMap.");
            }

            if (cborObject[0].AsInt32() != 1)
            {
                throw new ArgumentException("Invalid CBOR object: unexpected value for COSEHeaderMap tag.");
            }

            return new COSEHeaderMap
            {
                Alg = cborObject[1].AsInt32()
            };
        }
    }
}

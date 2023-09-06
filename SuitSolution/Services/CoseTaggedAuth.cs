using System;
using PeterO.Cbor;
using System.Collections.Generic;
using System.Linq;
using SuitSolution.Services.SuitSolution.Services;

namespace SuitSolution.Services
{
    public class CoseTaggedAuth : ISUITConvertible
    {
        public COSEHeaderMap Alg { get; set; }
        public byte[] Data { get; set; }

        public CBORObject ToCBOR()
        {
            var cborObject = CBORObject.NewMap();
            cborObject.Add("alg", Alg.ToSUIT());
            cborObject.Add("data", CBORObject.FromObject(Data));
            return cborObject;
        }

        public void FromCBOR(CBORObject cborObject)
        {
            Alg = COSEHeaderMap.FromSUIT(cborObject["alg"]);
            Data = cborObject["data"].ToObject<byte[]>();
        }

        public List<object> ToSUIT()
        {
            var suitList = new List<object>
            {
                Alg.ToSUIT(),
                Data
            };

            return suitList;
        }

        public void FromSUIT(List<object> suitList)
        {
            if (suitList == null || suitList.Count < 2)
            {
                throw new ArgumentException("Invalid SUIT list.");
            }

            Alg = COSEHeaderMap.FromSUIT(CBORObject.FromObject(suitList[0]));
            Data = (byte[])suitList[1];
        }

        public static CoseTaggedAuth FromSUIT(CBORObject cborObject)
        {
            var taggedAuth = new CoseTaggedAuth();

            if (cborObject.HasTag(2))
            {
                var taggedData = cborObject[2];

                if (taggedData.Type == CBORType.Map)
                {
                    taggedAuth.Alg = COSEHeaderMap.FromSUIT(taggedData["alg"]);
                    taggedAuth.Data = taggedData["data"].ToObject<byte[]>();
                }
            }

            return taggedAuth;
        }
    }
}
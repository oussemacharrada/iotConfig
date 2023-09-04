using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Dahomey.Cbor.ObjectModel;
using Dahomey.Cbor.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    // COSE_Sign1 class
    public class COSE_SIGN1
    {
        // COSE Headers
        public CBORObject ProtectedHeaders { get; set; }
        public CBORObject UnprotectedHeaders { get; set; }

        // Payload
        public byte[] Payload { get; set; }

        // Signature
        public byte[] Signature { get; set; }

        // Constructor
        public COSE_SIGN1(CBORObject protectedHeaders, CBORObject unprotectedHeaders, byte[] payload, byte[] signature)
        {
            ProtectedHeaders = protectedHeaders;
            UnprotectedHeaders = unprotectedHeaders;
            Payload = payload;
            Signature = signature;
        }

        // Serialize COSE_SIGN1 to CBOR
        public CBORObject EncodeToCBOR()
        {
            var coseMap = CBORObject.NewMap();

            if (ProtectedHeaders != null)
            {
                coseMap.Add(1, ProtectedHeaders);
            }

            if (UnprotectedHeaders != null)
            {
                coseMap.Add(4, UnprotectedHeaders);
            }

            coseMap.Add(18, CBORObject.FromObject(Payload));
            coseMap.Add(98, CBORObject.FromObject(Signature));

            return coseMap;
        }
        public static COSE_SIGN1 DecodeFromCBOR(CBORObject cbor)
        {
            var coseSign1 = new COSE_SIGN1(null, null, null, null); // Create an instance with default values

            if (cbor.Type == CBORType.Map)
            {
                if (cbor.ContainsKey(1))
                {
                    coseSign1.ProtectedHeaders = cbor[1];
                }

                if (cbor.ContainsKey(4))
                {
                    coseSign1.UnprotectedHeaders = cbor[4];
                }

                if (cbor.ContainsKey(18))
                {
                    coseSign1.Payload = cbor[18].ToObject<byte[]>();
                }

                if (cbor.ContainsKey(98))
                {
                    coseSign1.Signature = cbor[98].ToObject<byte[]>();
                }
            }

            return coseSign1;
        }
    }
}
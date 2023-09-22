using System;
using System.Collections.Generic;
using PeterO.Cbor;
using SuitSolution.Services.SuitSolution.Services;

namespace SuitSolution.Services
{
    public class SUITBytes : ISUITConvertible
    {
        public byte[] Bytes { get; set; }

        public SUITBytes()
        {
            Bytes = new byte[0];
        }

        public SUITBytes(byte[] bytes)
        {
            Bytes = bytes;
        }

        public void InitializeRandomData()
        {
            var random = new Random();
            var length = random.Next(10, 50); 
            var bytes = new byte[length];
            random.NextBytes(bytes);

            Bytes = bytes;
        }

        public List<object> ToSUIT()
        {
            return new List<object> { Bytes };
        }

        public void FromSUIT(List<object> suitList)
        {
            if (suitList == null || suitList.Count == 0 || !(suitList[0] is CBORObject cborBytes))
            {
                throw new ArgumentException("Invalid input data for SUITBytes.");
            }

            if (cborBytes.Type == CBORType.ByteString)
            {
                Bytes = cborBytes.GetByteString();
            }
            else
            {
                throw new ArgumentException("Invalid format for byte data in SUITBytes.");
            }
        }


    }
}
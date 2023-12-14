using System;
using System.Collections.Generic;
using System.Linq;
using SuitSolution.Exceptions;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITBytes : ISUITConvertible<SUITBytes>
    {
        public byte[] v { get; set; }

        public SUITBytes()
        {
            v = Array.Empty<byte>();
        }

        public SUITBytes(byte[] bytes)
        {
            v = bytes ?? throw new ArgumentNullException(nameof(bytes));
        }
        public dynamic ToSUIT()
        {
            return v;
        }

        public string ToDebug(string indent)
        {
            return $"h'{ToHexString(v)}'";
        }

        private byte[] FromHexString(string hexString)
        {
            return Enumerable.Range(0, hexString.Length / 2)
                .Select(x => Convert.ToByte(hexString.Substring(x * 2, 2), 16))
                .ToArray();
        }

        private string ToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLowerInvariant();
        }

        public bool Equals(SUITBytes other)
        {
            return other != null && v.SequenceEqual(other.v);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SUITBytes);
        }

        public override int GetHashCode()
        {
            return v != null ? v.Aggregate(17, (current, val) => current * 23 + val.GetHashCode()) : 0;
        }
        public SUITBytes FromData(Dictionary<object, object> dataDict)
        {
            TreeBranch.Append(typeof(SUITBytes).FullName);

            if (dataDict == null || !dataDict.TryGetValue("hex", out var hexValue))
            {
                throw new ArgumentException("Invalid data for SUITBytes.");
            }

            if (hexValue is string hexString)
            {
                v = FromHexString(hexString);
            }
            else
            {
                throw new ArgumentException("Invalid hex value in data.");
            }

            TreeBranch.Pop();
            return this;
        }
        public SUITBytes FromJsonData(Dictionary<string, object> dataDict)
        {
            TreeBranch.Append(typeof(SUITBytes).FullName);

            if (dataDict == null || !dataDict.TryGetValue("hex", out var hexValue))
            {
                throw new ArgumentException("Invalid data for SUITBytes.");
            }

            if (hexValue is string hexString)
            {
                v = FromHexString(hexString);
            }
            else
            {
                throw new ArgumentException("Invalid hex value in data.");
            }

            TreeBranch.Pop();
            return this;
        }

        public Dictionary<string, object> ToData()
        {
            return new Dictionary<string, object>
            {
                { "hex", ToHexString(v) }
            };
        }

        public SUITBytes FromSUIT(Dictionary<object, object> suitDict) => FromData(suitDict);
        public SUITBytes FromJson(Dictionary<string, object> jsonData) => FromJsonData(jsonData);
        public dynamic ToJson() => ToData();

      

     
    }
}

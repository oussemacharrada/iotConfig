using System;
using System.Text;
using System.Text.Json;
using SuitSolution.Exceptions;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITBytes : ISUITConvertible<SUITBytes>
    {
        public byte[] v { get; set; }

        public SUITBytes()
        {
            v = new byte[0];
        }

        public SUITBytes(byte[] bytes)
        {
            v = bytes ?? throw new ArgumentNullException(nameof(bytes));
        }

        
        public SUITBytes FromSUIT(Dictionary<string, object> suitDict)
        {
            if (suitDict == null || !suitDict.TryGetValue("hex", out var hexValue))
            {
                throw new ArgumentException("Invalid SUIT data for SUITBytes.");
            }

            if (hexValue is string hexString)
            {
                v = FromHexString(hexString);
            }
            else
            {
                throw new ArgumentException("Invalid hex value in SUIT data.");
            }

            return this;
        }

        public Dictionary<string, object> ToSUIT()
        {
            return new Dictionary<string, object>
            {
                { "hex", ToHexString(v) }
            };
        }

        public SUITBytes FromJson(Dictionary<string, object> jsonData)
        {
            if (jsonData == null || !jsonData.TryGetValue("hex", out var hexValue))
            {
                throw new ArgumentException("Invalid JSON data for SUITBytes.");
            }

            if (hexValue is string hexString)
            {
                v = FromHexString(hexString);
            }
            else
            {
                throw new ArgumentException("Invalid hex value in JSON data.");
            }

            return this;
        }

        public Dictionary<string, object> ToJson()
        {
            return new Dictionary<string, object>
            {
                { "hex", ToHexString(v) }
            };
        }

        public string ToDebug(string indent)
        {
            return $"h'{ToHexString(v)}'";
        }

        private byte[] FromHexString(string hexString)
        {
            hexString = hexString.Trim();
            int numChars = hexString.Length;
            byte[] bytes = new byte[numChars / 2];

            for (int i = 0; i < numChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return bytes;
        }

        private string ToHexString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:X2}", b);
            }
            return hex.ToString();
        }
    }
}

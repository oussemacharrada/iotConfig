using System;
using System.Collections.Generic;
using System.Text.Json;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITUUID 
    {
        private Guid _uuid;

        public SUITUUID()
        {
        }

        public SUITUUID(Guid uuid)
        {
            _uuid = uuid;
        }

       

        public byte[] ToSUIT()
        {
            byte[] bytes = _uuid.ToByteArray();
            // Swap the bytes to match big-endian format
            Array.Reverse(bytes, 0, 4);
            Array.Reverse(bytes, 4, 2);
            Array.Reverse(bytes, 6, 2);
            return bytes;
        }


        public void FromSUIT(Dictionary<string, object> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.TryGetValue("uuid", out var uuidValue))
            {
                if (uuidValue is string uuidString)
                {
                    _uuid = Guid.Parse(uuidString);
                }
                else if (uuidValue is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.String)
                {
                    _uuid = Guid.Parse(jsonElement.GetString());
                }
                else
                {
                    throw new ArgumentException("The 'uuid' key must be associated with a string value.");
                }
            }
            else
            {
                throw new KeyNotFoundException("The key 'uuid' was not found in the provided dictionary.");
            }
        }

        public void FromSUIT(byte[] data)
        {
            _uuid = new Guid(data);
        }

        public string ToJson()
        {
            return _uuid.ToString();
        }

        public SUITUUID FromJson(string jsonData)
        {
            _uuid = Guid.Parse(jsonData);
            return this;
        }

        public string ToDebug(string indent)
        {
            return $"h'{JsonSerializer.Serialize(ToJson())}' / {_uuid} /";
        }

        public void FromSUIT(Dictionary<object, object> vendorIdDict)
        {
            if (vendorIdDict == null)
                throw new ArgumentNullException(nameof(vendorIdDict));

            // Assuming the dictionary has a string representation of the UUID with a known key.
            // Replace "uuidKey" with the actual key used in your dictionary.
            object uuidKey = "uuidKey"; // The key used in the dictionary for the UUID.
            if (vendorIdDict.TryGetValue(uuidKey, out var uuidValue))
            {
                if (uuidValue is string uuidString)
                {
                    // If the value is a string, parse it into a Guid.
                    _uuid = Guid.Parse(uuidString);
                }
                else
                {
                    throw new ArgumentException($"The value associated with the key '{uuidKey}' is not a string.");
                }
            }
            else
            {
                throw new KeyNotFoundException($"The key '{uuidKey}' was not found in the provided dictionary.");
            }
        }

    }
}
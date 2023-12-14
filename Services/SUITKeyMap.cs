using System;
using System.Collections.Generic;
using System.Text.Json;

namespace SuitSolution.Services
{
    public class SUITKeyMap<T>
    {
        public readonly Dictionary<object, T> KeyMap;
        public readonly Dictionary<T, object> reverseKeyMap;
        public T currentValue;

        public SUITKeyMap(Dictionary<object, T> keyMap)
        {
            this.KeyMap = keyMap ?? throw new ArgumentNullException(nameof(keyMap));
            reverseKeyMap = new Dictionary<T, object>();
            foreach (var entry in keyMap)
            {
                reverseKeyMap[entry.Value] = entry.Key;
            }
        }

        public string ToJson()
        {
            if (reverseKeyMap.TryGetValue(currentValue, out object key))
            {
                return JsonSerializer.Serialize(key);
            }
            else
            {
                throw new InvalidOperationException("Current value is not in the key map.");
            }
        }

        public SUITKeyMap<T> FromJson(string jsonData)
        {
            var deserializedKey = JsonSerializer.Deserialize<object>(jsonData);
            if (deserializedKey != null && KeyMap.TryGetValue(deserializedKey, out T value))
            {
                currentValue = value;
                return this;
            }
            else
            {
                throw new ArgumentException("Invalid JSON data.");
            }
        }

        public T ToSUIT()
        {
            return currentValue;
        }

        public SUITKeyMap<T> FromSUIT(object key)
        {
            if (KeyMap.TryGetValue(key, out T value))
            {
                currentValue = value;
                return this;
            }
            else
            {
                throw new ArgumentException("Key not found in the key map.");
            }
        }

        public string ToDebug()
        {
            var jsonRepresentation = ToJson();
            return $"{currentValue} / {jsonRepresentation} /";
        }
    }
}

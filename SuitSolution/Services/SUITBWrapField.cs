using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;
using SuitSolution.Services.SuitSolution.Services;

namespace SuitSolution.Services
{
    using System.Collections.Generic;

    namespace SuitSolution.Services
    {
        public interface ISUITConvertible
        {
            List<object> ToSUIT();

            void FromSUIT(List<Object> suitList);
        }
    }


    public class SUITBWrapField<T> : ISUITConvertible where T : ISUITConvertible, new()
    {
        [JsonPropertyName("obj")] public T WrappedObject { get; set; }

        public SUITBWrapField(T wrappedObject)
        {
            WrappedObject = wrappedObject;
        }
        public void SetValue(T newValue)
        {
            WrappedObject = newValue;
        }
        public T GetValue()
        {
            return WrappedObject;
        }

        public SUITBWrapField()
        {
            WrappedObject = new T();
        }

        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        public SUITBWrapField<T> FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITBWrapField<T>>(json);
        }

        public List<object> ToSUIT()
        {
            return WrappedObject.ToSUIT();
        }

        public void FromSUIT(List<Object> suitList)
        {
            WrappedObject.FromSUIT(suitList);
        }
        public byte[] EncodeToBytes()
        {
          throw new NotImplementedException();
        }

        public void DecodeFromBytes(byte[] bytes)
        {
            throw new NotImplementedException();
        }
      
    }
}


   
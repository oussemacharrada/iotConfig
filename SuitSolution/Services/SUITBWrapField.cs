using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SuitSolution.Services
{
    // SUITBWrapField class
    public class SUITBWrapField<T>
    {
        // Property for wrapped object
        [JsonPropertyName("obj")]
        public static T WrappedObject { get; set; }

        // Constructor
        public SUITBWrapField(T wrappedObject)
        {
            WrappedObject = wrappedObject;
        }
       
    
        // Serialization methods
        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        public static SUITBWrapField<T> FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITBWrapField<T>>(json);
        }

        // Method for converting to SUIT format
        public List<object> ToSUIT()
        {
            // Convert the WrappedObject to SUIT format as needed
            // For example:
            if (WrappedObject is ISUITConvertible suitConvertible)
            {
                return suitConvertible.ToSUIT();
            }
            else
            {
                throw new NotSupportedException($"WrappedObject type {typeof(T)} does not support SUIT conversion.");
            }
        }

        // Method for converting from SUIT format
        public static void FromSUIT(List<object> suitList)
        {
            // Convert suitList to the WrappedObject as needed
            // For example:
            if (WrappedObject is ISUITConvertible suitConvertible)
            {
                suitConvertible.FromSUIT(suitList);
            }
            else
            {
                throw new NotSupportedException($"WrappedObject type {typeof(T)} does not support SUIT conversion.");
            }
        }
    }

    // Add any necessary interfaces for SUIT conversion
    public interface ISUITConvertible
    {
        List<object> ToSUIT();
        void FromSUIT(List<object> suitList);
    }

    // Sample wrapped class
    public class SampleWrappedClass : ISUITConvertible
    {
        [JsonPropertyName("field1")]
        public string Field1 { get; set; }

        [JsonPropertyName("field2")]
        public int Field2 { get; set; }

        // Implement ISUITConvertible methods
        public List<object> ToSUIT()
        {
            return new List<object>
            {
                Field1,
                Field2
            };
        }

        public void FromSUIT(List<object> suitList)
        {
            if (suitList.Count != 2)
            {
                throw new Exception("Invalid SUIT list size for SampleWrappedClass");
            }

            Field1 = (string)suitList[0];
            Field2 = (int)suitList[1];
        }
    }
}

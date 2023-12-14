using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

public class SerializationHelper
{
    public static byte[] SerializeToBytes(object obj)
    {
        if (obj == null)
            return null;

        return JsonSerializer.SerializeToUtf8Bytes(obj);
    }

    [Obsolete("Obsolete")]
    private static byte[] SerializeWithBinaryFormatter(object obj)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.TypeFormat = FormatterTypeStyle.TypesWhenNeeded; // Omit type information
            formatter.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }
    }

   
}
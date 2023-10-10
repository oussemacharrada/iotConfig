using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using MessagePack;

public class SerializationHelper
{
  
    public static byte[] SerializeToBytes(object obj)
    {
        if (obj == null)
            return null;

        return MessagePackSerializer.Serialize(obj);
    }



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


    private static byte[] SerializeWithMessagePack(object obj)
    {
        return MessagePackSerializer.Serialize(obj);
    }
}
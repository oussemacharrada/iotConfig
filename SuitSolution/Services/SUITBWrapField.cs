using System;
using System.Text;
using Newtonsoft.Json;
using PeterO.Cbor;
using SuitSolution.Exceptions;
using SuitSolution.Interfaces;

public class SUITBWrapField<T> where T : ISUITConvertible<T>, new()
{
    public T v { get; set; }

    public SUITBWrapField()
    {
        v = new T();
    }

    public SUITBWrapField(T initialValue)
    {
        v = initialValue;
    }

    public byte[] ToSUIT()
    {
        var cborObject = v.ToSUIT();
        var cbor = CBORObject.FromObject(cborObject);
        byte[] cborBytes = cbor.EncodeToBytes();
        // Convert the bytes to a hexadecimal string
        string hexString = BitConverter.ToString(cborBytes).Replace("-", "");

        // Convert the hexadecimal string to bytes (UTF-8 encoded)
        byte[] utf8Bytes = Encoding.UTF8.GetBytes(hexString);

   
        return utf8Bytes;
    }

    public string ToJson()
    {
        var suitDict = v.ToSUIT();
        return JsonConvert.SerializeObject(suitDict);
    }

    public SUITBWrapField<T> FromSUIT(byte[] cborBytes)
    {
        if (cborBytes == null)
        {
            throw new ArgumentNullException(nameof(cborBytes));
        }

        try
        {
            var suitDict = CBORObject.DecodeFromBytes(cborBytes).ToObject<Dictionary<string, object>>();
            v = new T().FromSUIT(suitDict);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to load CBOR data: {BitConverter.ToString(cborBytes)}");
            throw new SUITException(
                message: $"Failed to load CBOR data: {BitConverter.ToString(cborBytes)}");
        }

        return this;
    }

    public SUITBWrapField<T> FromJson(string data)
    {
        try
        {
            var suitDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            v = new T().FromSUIT(suitDict);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to parse JSON data: {data}");
            throw new SUITException(
                message: $"Failed to parse JSON data: {data}");
        }
        return this;
    }

    public string ToDebug(string indent)
    {
        var sb = new StringBuilder();
        sb.Append("h'");
        sb.Append(BitConverter.ToString(ToSUIT()).Replace("-", ""));
        sb.Append("' / ");
        sb.Append(v.ToDebug(indent));
        sb.Append(" /");
        return sb.ToString();
    }
}

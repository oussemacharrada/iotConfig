using System;
using System.Collections.Generic;
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
        TreeBranch.Append(typeof(SUITBWrapField<T>).FullName);
        var cborObject = v.ToSUIT();
        var cbor = CBORObject.FromObject(cborObject);
        byte[] cborBytes = cbor.EncodeToBytes();
        TreeBranch.Pop();
        return cborBytes;
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(v.ToSUIT());
    }

    public SUITBWrapField<T> FromSUIT(byte[] cborBytes)
    {
        TreeBranch.Append(typeof(SUITBWrapField<T>).FullName);
        if (cborBytes == null)
        {
            throw new ArgumentNullException(nameof(cborBytes));
        }

        try
        {
            var suitDict = CBORObject.DecodeFromBytes(cborBytes).ToObject<Dictionary<object, object>>();
            v = new T().FromSUIT(suitDict);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to load CBOR data: {BitConverter.ToString(cborBytes)}");
            throw new SUITException($"Failed to load CBOR data: {BitConverter.ToString(cborBytes)}");
        }
        finally
        {
            TreeBranch.Pop();
        }

        return this;
    }

    public SUITBWrapField<T> FromJson(string data)
    {
        TreeBranch.Append(typeof(SUITBWrapField<T>).FullName);
        try
        {
            var suitDict = JsonConvert.DeserializeObject<Dictionary<object, object>>(data);
            v = new T().FromSUIT(suitDict);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to parse JSON data: {data}");
            throw new SUITException($"Failed to parse JSON data: {data}");
        }
        finally
        {
            TreeBranch.Pop();
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

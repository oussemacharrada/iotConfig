namespace SuitSolution.Interfaces;
using PeterO.Cbor;

public interface ISUITUUID
{
    Guid UUID { get; set; }
    byte[] ToSUIT();
    void FromSUIT(Dictionary<string, object> data);
}
public interface ISUITConvertible
{
    Dictionary<string, object> ToSUIT();
    Dictionary<string, object> ToJson();
    string ToDebug(string indent);
    void FromSUIT(Dictionary<string, object> suitDict);
    object FromJson(Dictionary<string, object> jsonData);
}

public interface ISUITConvertible<T> 
{
    new  T FromSUIT(Dictionary<object, object> suitDict);
    new T FromJson(Dictionary<string, object>  jsonData);
    dynamic ToSUIT();
    dynamic ToJson();
    string ToDebug(string indent);


}

public interface IJsonSerializable
{
    dynamic ToJson();
}

public interface ISUITObject
{
    ISUITObject FromJson(string json);
    string ToJson();
    ISUITObject FromSUIT(CBORObject cborObject);
    object ToSUIT();
    string ToDebug(string indent);
}
public interface ISUITMember
{
    object ToJson();
    void FromJson(object data);
    object ToSUIT();
    void FromSUIT(object data);
    string ToDebug(int indent);
}
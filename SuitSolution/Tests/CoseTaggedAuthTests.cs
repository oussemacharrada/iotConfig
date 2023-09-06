using Xunit;
using PeterO.Cbor;
using SuitSolution.Services;
namespace SuitSolution.Tests;
public class CoseTaggedAuthTests
{
    [Fact]
    public void ToCBOR_SerializesToCBORObject()
    {
        var alg = new COSEHeaderMap
        {
            Alg = 1, 
        };

        var data = new byte[] { 0x01, 0x02, 0x03 }; 

        var coseTaggedAuth = new CoseTaggedAuth
        {
            Alg = alg,
            Data = data,
        };

        var cborObject = coseTaggedAuth.ToCBOR();

        Assert.NotNull(cborObject);
    }

    [Fact]
    public void FromCBOR_DeserializesFromCBORObject()
    {
        var alg = new COSEHeaderMap
        {
            Alg = 1
            
        };

        var data = new byte[] { 0x01, 0x02, 0x03 }; 

        var cborObject = CBORObject.NewMap();
        cborObject.Add("alg", alg.ToSUIT());
        cborObject.Add("data", CBORObject.FromObject(data));

        var coseTaggedAuth = new CoseTaggedAuth();
        coseTaggedAuth.FromCBOR(cborObject);

        Assert.NotNull(coseTaggedAuth);
    }

    [Fact]
    public void ToSUIT_SerializesToSUITList()
    {
        var alg = new COSEHeaderMap
        {
            Alg = 1, 
        };

        var data = new byte[] { 0x01, 0x02, 0x03 };

        var coseTaggedAuth = new CoseTaggedAuth
        {
            Alg = alg,
            Data = data,
        };

        var suitList = coseTaggedAuth.ToSUIT();

        Assert.NotNull(suitList);
    }

    [Fact]
    public void FromSUIT_DeserializesFromSUITList()
    {
        var alg = new COSEHeaderMap
        {
            Alg = 1, 
        };

        var data = new byte[] { 0x01, 0x02, 0x03 }; 

        var suitList = new System.Collections.Generic.List<object>
        {
            alg.ToSUIT(),
            data
        };

        var coseTaggedAuth = new CoseTaggedAuth();
        coseTaggedAuth.FromSUIT(suitList);

        Assert.NotNull(coseTaggedAuth);
    }

    [Fact]
    public void FromSUIT_CreatesFromCBORObject()
    {
        var alg = new COSEHeaderMap
        {
            Alg = 1, 
        };

        var data = new byte[] { 0x01, 0x02, 0x03 };

        var cborObject = CBORObject.NewMap();
        cborObject.Add("alg", alg.ToSUIT());
        cborObject.Add("data", CBORObject.FromObject(data));

        var coseTaggedAuth = CoseTaggedAuth.FromSUIT(cborObject);

        Assert.NotNull(coseTaggedAuth);
    }
}

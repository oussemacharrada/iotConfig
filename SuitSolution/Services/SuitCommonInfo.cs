using System;
using PeterO.Cbor;

public class SUITCommonInfo
{
    public string Label { get; set; }
    public string Uri { get; set; }
    public string PayloadDigest { get; set; }
    public string VendorIdentifier { get; set; }
    public string ClassIdentifier { get; set; }
    public string ImageDigest { get; set; }
    public int ImageSize { get; set; }
    public int VendorInfoLength { get; set; }
    public int ClassInfoLength { get; set; }
    public int UriLength { get; set; }

    public SUITCommonInfo()
    {
        Label = string.Empty;
        Uri = string.Empty;
        PayloadDigest = string.Empty;
        VendorIdentifier = string.Empty;
        ClassIdentifier = string.Empty;
        ImageDigest = string.Empty;
        ImageSize = 0;
        VendorInfoLength = 0;
        ClassInfoLength = 0;
        UriLength = 0;
    }

    public CBORObject ToCBORObject()
    {
        var cbor = CBORObject.NewMap();

        cbor.Add("label", Label);
        cbor.Add("uri", Uri);
        cbor.Add("payload-digest", PayloadDigest);
        cbor.Add("vendor-identifier", VendorIdentifier);
        cbor.Add("class-identifier", ClassIdentifier);
        cbor.Add("image-digest", ImageDigest);
        cbor.Add("image-size", ImageSize);
        cbor.Add("vendor-info-length", VendorInfoLength);
        cbor.Add("class-info-length", ClassInfoLength);
        cbor.Add("uri-length", UriLength);

        return cbor;
    }

    public void FromCBORObject(CBORObject cbor)
    {
        if (cbor.Type != CBORType.Map)
        {
            throw new FormatException("Invalid CBOR data for SUITCommonInfo");
        }

        Label = cbor["label"].AsString();
        Uri = cbor["uri"].AsString();
        PayloadDigest = cbor["payload-digest"].AsString();
        VendorIdentifier = cbor["vendor-identifier"].AsString();
        ClassIdentifier = cbor["class-identifier"].AsString();
        ImageDigest = cbor["image-digest"].AsString();
        ImageSize = cbor["image-size"].AsInt32();
        VendorInfoLength = cbor["vendor-info-length"].AsInt32();
        ClassInfoLength = cbor["class-info-length"].AsInt32();
        UriLength = cbor["uri-length"].AsInt32();
    }
}

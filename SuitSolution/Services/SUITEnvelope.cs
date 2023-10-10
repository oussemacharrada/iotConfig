using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using PeterO.Cbor;
using SuitSolution.Services;
using SuitSolution.Services.Utility;
using JsonSerializer = System.Text.Json.JsonSerializer;

public class SUITEnvelope:SUITManifestDict {
    public SUITBWrapField<COSEList> Auth { get; set; }    = new SUITBWrapField<COSEList>();     
    public SUITBWrapField<SUITManifest> Manifest { get; set; } = new SUITBWrapField<SUITManifest>();
    public SUITBWrapField<SUITSequence> deres { get; set; }= new SUITBWrapField<SUITSequence>();
    public SUITBWrapField<SUITSequence> fetch { get; set; }= new SUITBWrapField<SUITSequence>();
    public SUITBWrapField<SUITSequence> install { get; set; }= new SUITBWrapField<SUITSequence>();
    public SUITBWrapField<SUITSequence> validate { get; set; }= new SUITBWrapField<SUITSequence>();
    public SUITBWrapField<SUITSequence> load { get; set; }= new SUITBWrapField<SUITSequence>();
    public SUITBWrapField<SUITSequence> run { get; set; }= new SUITBWrapField<SUITSequence>();
    public SUITBWrapField<SUITText> text { get; set; }= new SUITBWrapField<SUITText>();
    public SUITBytes coswid { get; set; }= new SUITBytes();
    public SUITEnvelope()
    {
        fields = new ReadOnlyDictionary<string, ManifestKey>(MkFields(
            ("Auth", "2", () => Auth),
            ("Manifest", "3", () => Manifest)
         /*   ("deres", "7", () => deres),
            ("fetch", "8", () => fetch),
            ("install", "9", () => install),
            ("validate", "10    ", () => validate),
            ("load", "11", () => load),
            ("run", "12", () => run),
            ("text", "13", () => text),
            ("coswid", "14", () => coswid)*/
        ));
   
    }

 

    public SUITEnvelope(
        COSEList auth,
        SUITManifest manifest
)
    {
        if (auth != null)
        {
            Auth = new SUITBWrapField<COSEList> { v = auth };
        }

        if (manifest != null)
        {
            Manifest = new SUITBWrapField<SUITManifest> { v = manifest };
        }

     
    }

  

    /*  public Dictionary<string, object> ToSUIT(string digestAlg)
      {
     var  map =    base.ToSUIT();
  
            var suitList = base.ToSUIT();
    
            var map = CBORObject.NewMap();
           if (Auth != null)
           {
               map.Add("2", CBORObject.FromObject(Auth.v.ToSUIT()).EncodeToBytes());
            }
          
            if (Manifest != null)
            {
                map.Add("3", Manifest.v.ToSUIT().EncodeToBytes());
            }
    */
        //    suitList.Add("7", SerializationHelper.SerializeToBytes(deres.v.ToSUIT()));

        //   suitList.Add("8",SerializationHelper.SerializeToBytes(fetch.v.ToSUIT()));

        //  suitList.Add("9",SerializationHelper.SerializeToBytes(install.v.ToSUIT()));

        //   suitList.Add( "10",SerializationHelper.SerializeToBytes(validate.v.ToSUIT()));

        // suitList.Add("11",SerializationHelper.SerializeToBytes(load.v.ToSUIT()));

        //suitList.Add("12",SerializationHelper.SerializeToBytes(run.v.ToSUIT()));

        //          suitList.Add("13",SerializationHelper.SerializeToBytes(text.v.ToSUIT()));

//            suitList.Add("14" , coswid.ToSUIT());

//Console.WriteLine(suitList);
        //CBORObject sd =  CBORObject.FromObject(suitList);


         //     return map;
    //}

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public static SUITEnvelope FromJson(string json)
    {
        return JsonSerializer.Deserialize<SUITEnvelope>(json);
    }

    public static byte[] ComputeHash(byte[] data, string algorithmName)
    {
        using (HashAlgorithm algorithm = HashAlgorithm.Create(algorithmName))
        {
            if (algorithm == null)
            {
                throw new NotSupportedException($"Hash algorithm '{algorithmName}' is not supported.");
            }

            return algorithm.ComputeHash(data);
        }
    }
    public static byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;

        string json = JsonConvert.SerializeObject(obj);
        return Encoding.UTF8.GetBytes(json);
    }
}

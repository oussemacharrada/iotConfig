using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Dahomey.Cbor;
using PeterO.Cbor;
using SuitSolution.Interfaces;
using SuitSolution.Services;
using Xunit.Sdk;

public class SUITEnvelope : SUITManifestDict
{
    public SUITBWrapField<COSEList> auth { get; set; }
    public SUITBWrapField<SUITManifest> manifest { get; set; }
    public SUITBWrapField<DependencyResolution> deres { get; set; }
    public SUITBWrapField<PayloadFetch> fetch { get; set; }
    public SUITBWrapField<SUITInstall> install { get; set; }
    public SUITBWrapField<SUITSequence> validate { get; set; }
    public SUITBWrapField<SUITSequence> load { get; set; }
    public SUITBWrapField<SUITSequence> run { get; set; }
    public SUITBWrapField<SUITText> text { get; set; }
    public SUITBytes coswid { get; set; }

    private readonly HashSet<string> severableFields = new HashSet<string>
        { "deres", "fetch", "install", "text", "coswid" };

    private Dictionary<string, HashAlgorithmName> digestAlgorithms = new Dictionary<string, HashAlgorithmName>
    {
        { "sha256", HashAlgorithmName.SHA256 },
        { "sha384", HashAlgorithmName.SHA384 },
        { "sha512", HashAlgorithmName.SHA512 }
    };

    public static SUITEnvelope fromLists(List<object> Run,
        List<object> Auth,
        List<object> Load,
        List<object> Text,
        List<object> Deres,
        List<object> Fetch,
        List<object> Install,
        List<object> Manifest,
        List<object> Validate , List<object> Coswid )
    {
        SUITBWrapField<COSEList> auth = new SUITBWrapField<COSEList>();
        SUITBWrapField<SUITManifest>   manifest = new SUITBWrapField<SUITManifest>();
        SUITBWrapField<DependencyResolution>  deres = new SUITBWrapField<DependencyResolution>();
        SUITBWrapField<PayloadFetch>  fetch = new SUITBWrapField<PayloadFetch>();
        SUITBWrapField<SUITInstall>  install = new SUITBWrapField<SUITInstall>();
        SUITBWrapField<SUITSequence>validate = new SUITBWrapField<SUITSequence>();
        SUITBWrapField<SUITSequence>  load = new SUITBWrapField<SUITSequence>();
        SUITBWrapField<SUITSequence>  run = new SUITBWrapField<SUITSequence>();
        SUITBWrapField<SUITText>   text = new SUITBWrapField<SUITText>();
        SUITBytes  coswid = new SUITBytes();
        auth.FromSUIT(Auth); 
        manifest.FromSUIT(Manifest);
        deres.FromSUIT(Deres);
        fetch.FromSUIT(Fetch);
        
        install.FromSUIT(Install); 
        load.FromSUIT(Load);
        run.FromSUIT(Run);
        text.FromSUIT(Deres);
        validate.FromSUIT(Validate);
        coswid.FromSUIT(Coswid);
        
        SUITEnvelope suitEnvelope = new SUITEnvelope(auth,
manifest,
 deres,
 fetch,
 install,
 validate,
 load,
 run,
 text,
coswid);
        return suitEnvelope;




    }
    public SUITEnvelope()
    {
        auth = new SUITBWrapField<COSEList>();
        manifest = new SUITBWrapField<SUITManifest>();
        deres = new SUITBWrapField<DependencyResolution>();
        fetch = new SUITBWrapField<PayloadFetch>();
        install = new SUITBWrapField<SUITInstall>();
        validate = new SUITBWrapField<SUITSequence>();
        load = new SUITBWrapField<SUITSequence>();
        run = new SUITBWrapField<SUITSequence>();
        text = new SUITBWrapField<SUITText>();
        coswid = new SUITBytes();
    }

    public SUITEnvelope(
        SUITBWrapField<COSEList> auth,
        SUITBWrapField<SUITManifest> manifest,
        SUITBWrapField<DependencyResolution> deres,
        SUITBWrapField<PayloadFetch> fetch,
        SUITBWrapField<SUITInstall> install,
        SUITBWrapField<SUITSequence> validate,
        SUITBWrapField<SUITSequence> load,
        SUITBWrapField<SUITSequence> run,
        SUITBWrapField<SUITText> text,
        SUITBytes coswid)
    {
        this.auth = auth ?? new SUITBWrapField<COSEList>();
        this.manifest = manifest ?? new SUITBWrapField<SUITManifest>();
        this.deres = deres ?? new SUITBWrapField<DependencyResolution>();
        this.fetch = fetch ?? new SUITBWrapField<PayloadFetch>();
        this.install = install ?? new SUITBWrapField<SUITInstall>();
        this.validate = validate ?? new SUITBWrapField<SUITSequence>();
        this.load = load ?? new SUITBWrapField<SUITSequence>();
        this.run = run ?? new SUITBWrapField<SUITSequence>();
        this.text = text ?? new SUITBWrapField<SUITText>();
        this.coswid = coswid ?? new SUITBytes();
    }


    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public static SUITEnvelope FromJson(string json)
    {
        return JsonSerializer.Deserialize<SUITEnvelope>(json);
    }


    public static void  FromSUIT(CBORObject cborObject)
    {
        var authTagged1 = cborObject.ContainsKey(2);
        var testing = cborObject.EncodeToBytes();

        Console.WriteLine(cborObject);

     List<object> Run;
     List<object> Auth;
     List<object> Load;
     List<object> Text;
     List<object> Deres;
     List<object> Fetch;
     List<object> Install;
     List<object> Manifest;
     List<object> Validate;
     List<object> Coswid; 

        Run = cborObject.ContainsKey("run") ? cborObject["run"].ToObject<List<object>>() : null;
        Auth = cborObject.ContainsKey("auth") ? cborObject["auth"].ToObject<List<object>>() : null;
        Load = cborObject.ContainsKey("load") ? cborObject["load"].ToObject<List<object>>() : null;
        Text = cborObject.ContainsKey("text") ? cborObject["text"].ToObject<List<object>>() : null;
        Deres = cborObject.ContainsKey("deres") ? cborObject["deres"].ToObject<List<object>>() : null;
        Fetch = cborObject.ContainsKey("fetch") ? cborObject["fetch"].ToObject<List<object>>() : null;
        Install = cborObject.ContainsKey("install") ? cborObject["install"].ToObject<List<object>>() : null;
        Manifest = cborObject.ContainsKey("manifest") ? cborObject["manifest"].ToObject<List<object>>() : null;
        Validate = cborObject.ContainsKey("validate") ? cborObject["validate"].ToObject<List<object>>() : null;
        Coswid = cborObject.ContainsKey("coswid") ? cborObject["coswid"].ToObject<List<object>>() : null;

        SUITEnvelope suit = SUITEnvelope.fromLists(Run,
             Auth,
             Load,
             Text,
             Deres,
             Fetch,
             Install,
            Manifest,
            Validate, Coswid);
        Console.WriteLine(Auth);
        Console.WriteLine(Load);
        Console.WriteLine(Text);
        Console.WriteLine(Deres);
        Console.WriteLine(Fetch);
        Console.WriteLine(Install);
        Console.WriteLine(Manifest);
        Console.WriteLine(Run);
        Console.WriteLine(Validate);

        Console.WriteLine(Coswid);




    }
    private string BytesToHexString(byte[] bytes)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            sb.Append(b.ToString("X2"));
        }
        return sb.ToString();
    }
    public CBORObject ToSUIT(string digestAlg)
    {
        CBORObject cborObject = CBORObject.NewMap();

        SerializeField("auth", auth, cborObject);

        return cborObject;
    }

    private void SerializeField<T>(string fieldName, T fieldValue, CBORObject cborObject)
    {
        if (fieldValue != null)
        {
            var cborValue = ConvertComplexStructureToCBOR(fieldValue);
            if (cborValue != null)
            {
                var hexString = BytesToHexString(cborValue.EncodeToBytes());
                cborObject.Add(2, CBORObject.FromObject(hexString));
            }
        }
    }

    private CBORObject ConvertComplexStructureToCBOR(object complexObject)
    {
        if (complexObject == null)
        {
            return CBORObject.NewMap();
        }

        if (complexObject is IDictionary<string, object> dictionary)
        {
            var cborMap = CBORObject.NewMap();
            foreach (var kvp in dictionary)
            {
                var key = kvp.Key;
                var value = kvp.Value;
                var cborValue = ConvertComplexStructureToCBOR(value);
                cborMap.Add(key, cborValue);
            }
            return cborMap;
        }
        else if (complexObject is IList<object> list)
        {
            var cborArray = CBORObject.NewArray();
            foreach (var item in list)
            {
                var cborItem = ConvertComplexStructureToCBOR(item);
                cborArray.Add(cborItem);
            }
            return cborArray;
        }
        else
        {
            return CBORObject.FromObject(complexObject);
        }
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
 


    public string ToDebug(string prefix)
    {
        StringBuilder debugText = new StringBuilder();
        debugText.AppendLine($"{prefix}SUITEnvelope Debug Output:");

        if (fetch != null)
        {
            debugText.AppendLine($"{prefix}Fetch: ");
        }

        if (install != null)
        {
            debugText.AppendLine($"{prefix}Install: ");
        }

        if (manifest != null)
        {
            debugText.AppendLine($"{prefix}Manifest: ");
        }



        return debugText.ToString();
    }

   

    
    private bool HasField(string fieldName)
    {
        var property = GetType().GetProperty(fieldName);
        return property != null;
    }

    private object GetField(string fieldName)
    {
        var property = GetType().GetProperty(fieldName);
        return property?.GetValue(this);
    }

    public void InitializeRandomData()
    {
        COSEList co = new COSEList();
        co.InitializeRandomData();
        auth.SetValue(co);

        manifest.WrappedObject.InitializeRandomData();
        deres.WrappedObject.InitializeRandomData();

        fetch.WrappedObject.InitializeRandomData();
        install.WrappedObject.InitializeRandomData();

        validate.WrappedObject.InitializeRandomData();

        load.WrappedObject.InitializeRandomData();

        run.WrappedObject.InitializeRandomData();

        text.WrappedObject.InitializeRandomData();

            coswid.InitializeRandomData();
    }
}



public class SUITEnvelopeBuilder
{
    private SUITEnvelope suitEnvelope;

    public SUITEnvelopeBuilder()
    {
        suitEnvelope = new SUITEnvelope();
    }

    public SUITEnvelopeBuilder SetAuth(SUITBWrapField<COSEList> auth)
    {
        suitEnvelope.auth = auth ?? throw new ArgumentNullException(nameof(auth));
        return this;
    }

    public SUITEnvelopeBuilder SetManifest(SUITBWrapField<SUITManifest> manifest)
    {
        suitEnvelope.manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
        return this;
    }

    public SUITEnvelopeBuilder SetDeres(SUITBWrapField<DependencyResolution> deres)
    {
        suitEnvelope.deres = deres ?? throw new ArgumentNullException(nameof(deres));
        return this;
    }

    public SUITEnvelopeBuilder SetFetch(SUITBWrapField<PayloadFetch> fetch)
    {
        suitEnvelope.fetch = fetch ?? throw new ArgumentNullException(nameof(fetch));
        return this;
    }

    public SUITEnvelopeBuilder SetInstall(SUITBWrapField<SUITInstall> install)
    {
        suitEnvelope.install = install ?? throw new ArgumentNullException(nameof(install));
        return this;
    }

    public SUITEnvelopeBuilder SetValidate(SUITBWrapField<SUITSequence> validate)
    {
        suitEnvelope.validate = validate ?? throw new ArgumentNullException(nameof(validate));
        return this;
    }

    public SUITEnvelopeBuilder SetLoad(SUITBWrapField<SUITSequence> load)
    {
        suitEnvelope.load = load ?? throw new ArgumentNullException(nameof(load));
        return this;
    }

    public SUITEnvelopeBuilder SetRun(SUITBWrapField<SUITSequence> run)
    {
        suitEnvelope.run = run ?? throw new ArgumentNullException(nameof(run));
        return this;
    }

    public SUITEnvelopeBuilder SetText(SUITBWrapField<SUITText> text)
    {
        suitEnvelope.text = text ?? throw new ArgumentNullException(nameof(text));
        return this;
    }

    public SUITEnvelopeBuilder SetCoswid(SUITBytes coswid)
    {
        suitEnvelope.coswid = coswid ?? throw new ArgumentNullException(nameof(coswid));
        return this;
    }

    public SUITEnvelopeBuilder InitializeRandomData()
    {
        suitEnvelope.InitializeRandomData();
        return this;
    }

    public SUITEnvelope Build()
    {
        return suitEnvelope;
    }

    public string ToJson()
    {
        return JsonSerializer.Serialize(suitEnvelope);
    }

    public static SUITEnvelopeBuilder FromJson(string json)
    {
        return new SUITEnvelopeBuilder().SetSUITEnvelope(JsonSerializer.Deserialize<SUITEnvelope>(json));
    }

    public SUITEnvelopeBuilder SetSUITEnvelope(SUITEnvelope existingSUITEnvelope)
    {
        suitEnvelope = existingSUITEnvelope ?? throw new ArgumentNullException(nameof(existingSUITEnvelope));
        return this;
    }
}



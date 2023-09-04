using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using PeterO.Cbor;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class SUITEnvelope : SUITManifestDict
{
    // Fields
    public SUITBWrapField<COSEList> auth { get; set; }
    public SUITBWrapField<SUITManifest> manifest { get; set; }
    public SUITBWrapField<SUITSequence> deres { get; set; }
    public SUITBWrapField<SUITSequence> fetch { get; set; }
    public SUITBWrapField<SUITSequence> install { get; set; }
    public SUITBWrapField<SUITSequence> validate { get; set; }
    public SUITBWrapField<SUITSequence> load { get; set; }
    public SUITBWrapField<SUITSequence> run { get; set; }
    public SUITBWrapField<SUITText> text { get; set; }
    public SUITBytes coswid { get; set; }

    // Fields for severable handling
    private readonly HashSet<string> severableFields = new HashSet<string>
        { "deres", "fetch", "install", "text", "coswid" };

    private Dictionary<string, HashAlgorithmName> digestAlgorithms = new Dictionary<string, HashAlgorithmName>
    {
        { "sha256", HashAlgorithmName.SHA256 },
        { "sha384", HashAlgorithmName.SHA384 },
        { "sha512", HashAlgorithmName.SHA512 }
    };



    public SUITEnvelope(
        SUITBWrapField<COSEList> auth,
        SUITBWrapField<SUITManifest> manifest,
        SUITBWrapField<SUITSequence> deres,
        SUITBWrapField<SUITSequence> fetch,
        SUITBWrapField<SUITSequence> install,
        SUITBWrapField<SUITSequence> validate,
        SUITBWrapField<SUITSequence> load,
        SUITBWrapField<SUITSequence> run,
        SUITBWrapField<SUITText> text,
        SUITBytes coswid)
    {
        auth = auth;
        manifest = manifest;
        deres = deres;
        fetch = fetch;
        install = install;
        validate = validate;
        load = load;
        run = run;
        text = text;
        coswid = coswid;
    }


    // Serialization methods
    public CBORObject ToSUIT(string digestAlg)
    {
        CBORObject cborObject = base.ToSUIT();

        if (auth != null)
        {
            cborObject["auth"] = CBORObject.FromObject(auth);
        }

        if (manifest != null)
        {
            cborObject["manifest"] = CBORObject.FromObject(manifest);
        }

        if (deres != null)
        {
            cborObject["deres"] = CBORObject.FromObject(deres);
        }

        if (fetch != null)
        {
            cborObject["fetch"] = CBORObject.FromObject(fetch);
        }

        if (install != null)
        {
            cborObject["install"] = CBORObject.FromObject(install);
        }

        if (validate != null)
        {
            cborObject["validate"] = CBORObject.FromObject(validate);
        }

        if (load != null)
        {
            cborObject["load"] = CBORObject.FromObject(load);
        }

        if (run != null)
        {
            cborObject["run"] = CBORObject.FromObject(run);
        }

        if (text != null)
        {
            cborObject["text"] = CBORObject.FromObject(text);
        }

        if (coswid != null)
        {
            cborObject["coswid"] = CBORObject.FromObject(coswid);
        }

        foreach (string fieldName in severableFields)
        {
            if (HasField(fieldName))
            {
                var field = GetField(fieldName);
                if (field != null)
                {
                    var cborField = CBORObject.FromObject(field);
                    var digest = HashAlgorithm.Create(digestAlgorithms[digestAlg].ToString());
                    var cborDigest = CBORObject.FromObject(digest.ComputeHash(cborField.EncodeToBytes()));
                    if (cborDigest.Count < cborField.Count)
                    {
                        cborObject[fieldName] = cborDigest;
                    }
                }
            }
        }

        return cborObject;
    }


    // Method for creating an instance of the class from CBOR data
    public static SUITEnvelope FromSUIT(CBORObject cborObject)
    {
        var authTagged1 = cborObject.ContainsKey(2);
        var testing = cborObject.EncodeToBytes();

        Console.WriteLine(cborObject);

        var authField = cborObject.ContainsKey("auth")
            ? cborObject["auth"].ToObject<SUITBWrapField<COSEList>>()
            : null;

        var manifestField = cborObject.ContainsKey("manifest")
            ? cborObject["manifest"].ToObject<SUITBWrapField<SUITManifest>>()
            : null;

        var deresField = cborObject.ContainsKey("deres")
            ? cborObject["deres"].ToObject<SUITBWrapField<SUITSequence>>()
            : null;

        var fetchField = cborObject.ContainsKey("fetch")
            ? cborObject["fetch"].ToObject<SUITBWrapField<SUITSequence>>()
            : null;

        var installField = cborObject.ContainsKey("install")
            ? cborObject["install"].ToObject<SUITBWrapField<SUITSequence>>()
            : null;

        var validateField = cborObject.ContainsKey("validate")
            ? cborObject["validate"].ToObject<SUITBWrapField<SUITSequence>>()
            : null;

        var loadField = cborObject.ContainsKey("load")
            ? cborObject["load"].ToObject<SUITBWrapField<SUITSequence>>()
            : null;

        var runField = cborObject.ContainsKey("run")
            ? cborObject["run"].ToObject<SUITBWrapField<SUITSequence>>()
            : null;

        var textField = cborObject.ContainsKey("text")
            ? cborObject["text"].ToObject<SUITBWrapField<SUITText>>()
            : null;

        var coswidField = cborObject.ContainsKey("coswid")
            ? cborObject["coswid"].ToObject<SUITBytes>()
            : null;

        var envelope = new SUITEnvelope(
            authField,
            manifestField,
            deresField,
            fetchField,
            installField,
            validateField,
            loadField,
            runField,
            textField,
            coswidField
        );
Console.WriteLine(envelope.ToString());



        return envelope;
    }


    public string ToDebug(string prefix)
    {
        StringBuilder debugText = new StringBuilder();
        debugText.AppendLine($"{prefix}SuitEnvelope Debug Output:");

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

        // ... add more properties as needed ...

        return debugText.ToString();
    }

    private bool HasField(string fieldName)
    {
        return GetType().GetProperty(fieldName) != null;
    }

    private object GetField(string fieldName)
    {
        return GetType().GetProperty(fieldName)?.GetValue(this);
    }
    
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;
using SuitSolution.Interfaces;
using SuitSolution.Services;

public class SUITManifestDict
    {
        // Properties for fields
        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("sequence")]
        public int Sequence { get; set; }

        [JsonPropertyName("dependencies")]
        public List<SUITDependency> Dependencies { get; set; }

        // Constructors
        public SUITManifestDict()
        {
            Dependencies = new List<SUITDependency>();
        }

        // Method for converting to debug string
        public string ToDebugString()
        {
            return $"Version: {Version}, Sequence: {Sequence}, Dependencies Count: {Dependencies.Count}";
        }

        // Serialization methods
        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        public static SUITManifestDict FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITManifestDict>(json);
        }

        // Method for converting to SUIT format
        public List<SUITBWrapField<COSEList>> dependencies { get; set; }
        public List<SUITBWrapField<COSEList>> components { get; set; }
        public List<SUITBWrapField<SUITBWrapField<CoseTaggedAuth>>> manifests { get; set; }

        public CBORObject ToSUIT()
        {
            CBORObject cborObject = CBORObject.NewMap();
            cborObject.Add("dependencies", CBORObject.NewArray().Add(dependencies.Select(dep => dep.ToSUIT()).ToArray()));
            cborObject.Add("components", CBORObject.NewArray().Add(components.Select(comp => comp.ToSUIT()).ToArray()));
            cborObject.Add("manifests", CBORObject.NewArray().Add(manifests.Select(manifest => manifest.ToSUIT()).ToArray()));
            return cborObject;
        }

       /* public static SUITManifestDict FromSUIT(CBORObject cborObject)
        {
            return new SUITManifestDict
            {
                dependencies = cborObject["dependencies"].Values.Select(dep => SUITBWrapField<COSEList>.FromSUIT(dep.EncodeToBytes())).ToList(),
                components = cborObject["components"].Values.Select(comp => SUITBWrapField<COSEList>.FromSUIT(comp.EncodeToBytes())).ToList(),
                manifests = cborObject["manifests"].Values.Select(manifest => SUITBWrapField<SUITBWrapField<CoseTaggedAuth>>.FromSUIT(manifest.EncodeToBytes())).ToList()
            };
        }*/
    }

    // SUITDependency class
  

    /* Main class for testing
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create an instance of SUITManifestDict
            var manifestDict = new SUITManifestDict
            {
                Version = 1,
                Sequence = 123
            };

            // Add a sample dependency
            manifestDict.Dependencies.Add(new SUITDependency
            {
                Digest = "1234567890",
                Prefix = "abcdef"
            });

            // Convert to debug string
            string debugString = manifestDict.ToDebugString();
            Console.WriteLine(debugString);

            // Serialize to JSON
            string jsonString = manifestDict.ToJson();
            Console.WriteLine(jsonString);

            // Deserialize from JSON
            SUITManifestDict deserializedManifestDict = SUITManifestDict.FromJson(jsonString);
            Console.WriteLine($"Deserialized Version: {deserializedManifestDict.Version}");
            Console.WriteLine($"Deserialized Sequence: {deserializedManifestDict.Sequence}");
            Console.WriteLine($"Deserialized Dependencies Count: {deserializedManifestDict.Dependencies.Count}");

            // Convert to SUIT format
            List<object> suitList = manifestDict.ToSUIT();
            Console.WriteLine($"SUIT List: {string.Join(", ", suitList)}");

            // Convert from SUIT format
            var newManifestDict = new SUITManifestDict();
            newManifestDict.FromSUIT(suitList);
            Console.WriteLine($"New Manifest Dict: Version={newManifestDict.Version}, Sequence={newManifestDict.Sequence}");
        }
    }*/
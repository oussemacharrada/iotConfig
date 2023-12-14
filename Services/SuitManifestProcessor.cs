using PeterO.Cbor;
using System;
using System.Collections.Generic;
using System.IO;

namespace SuitSolution.Services
{
    public class SuitManifestProcessor
    {
        public string UrlRoot { get; set; }
        public int Seqnr { get; set; }
        public string Output { get; set; }
        public string UuidVendor { get; set; }
        public string UuidClass { get; set; }
        public List<string> SlotFiles { get; set; }
        public string KeyPath { get; set; }
        public string Encryptionpass { get; set; }
        public string SourceDirectory { get; set; }
        public SuitManifestProcessor(string sourceDirectory, string urlRoot, int seqnr, string uuidVendor, string uuidClass, List<string> slotFiles, string keyPath, string encryptionpass)
        {
            this.SourceDirectory = sourceDirectory;
            this.UrlRoot = urlRoot;
            this.Seqnr = seqnr;
            this.UuidVendor = uuidVendor;
            this.UuidClass = uuidClass;
            this.SlotFiles = slotFiles;
            this.KeyPath = keyPath;
            this.Encryptionpass = encryptionpass;
        }

        public void ProcessSUITFileSigning()
        {
            ParsedArguments GetParsedArguments()
            {
                return new ParsedArguments
                {
                    UrlRoot = this.UrlRoot,
                    Seqnr = this.Seqnr,
                    Output = this.Output,
                    UuidVendor = this.UuidVendor,
                    UuidClass = this.UuidClass,
                    SlotFiles = this.SlotFiles
                };
            }

            var parsedArgs = GetParsedArguments();
            var manifestTemplate = ManifestJSONGen.GenerateManifest(parsedArgs).GetAwaiter().GetResult();

            var compiler = new ManifestCompiler(manifestTemplate);
            var manifest = compiler.CompileManifest();

            // Paths to the SUIT file and PEM key
            string suitFilePath = Path.Combine(this.SourceDirectory, "hwr-0_fwt-13_fwr-1unsigned.mani");
            string pemKeyFilePath = Path.Combine(this.SourceDirectory, this.KeyPath); // Use the provided KeyPath property
            string password = this.Encryptionpass; // Use the provided Encryptionpass property

            // Load the SUIT file and PEM key
            byte[] suitFileContent = File.ReadAllBytes(suitFilePath);
            string pemKey = File.ReadAllText(pemKeyFilePath);

        

            SUITSigner signer = new SUITSigner(pemKey, suitFileContent, password);

            // Set the desired digest algorithm (e.g., SHA256)
            signer.DigestAlgorithm.SetAlgorithmValue(2);

            // Sign the SUIT file
            var env = new SUITEnvelope();
            env = manifest;
            SUITBWrapField<COSEList> signedSUITFile = signer.SignEnvelope(env);
            manifest.Auth = signedSUITFile;

            // Output to CBOR file
            Dictionary<int, object> cbor = manifest.ToSUIT();
            CBORObject cborObject = CBORObject.FromObject(cbor);
            byte[] cborData = cborObject.EncodeToBytes();
            var filePath = "C:/Users/ousama.charada/OneDrive - ML!PA Consulting GmbH/Desktop/mysuit/suit-manifest-generator/TestCase/object.cbor";
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                fs.Write(cborData, 0, cborData.Length);
            }
        }
    }
}

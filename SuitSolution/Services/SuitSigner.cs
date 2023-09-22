using System;
using System.IO;
using System.Security.Cryptography;
using Dahomey.Cbor;
using Newtonsoft.Json.Linq;

/*
namespace SuitSolution.Services
{
    public class SUITSigner
    {
        private readonly string privateKeyPath;
        private readonly string password;
        private readonly string outputPath;

        public SUITSigner(string privateKeyPath, string password, string outputPath)
        {
            this.privateKeyPath = privateKeyPath ?? throw new ArgumentNullException(nameof(privateKeyPath));
            this.password = password;
            this.outputPath = outputPath ?? throw new ArgumentNullException(nameof(outputPath));
        }

        public bool SignManifest(string manifestFilePath)
        {
            try
            {
                SUITManifest manifest = LoadManifestFromFile(manifestFilePath);

                string privateKeyPem = File.ReadAllText(privateKeyPath);

                CngKey privateKey = LoadPrivateKeyFromPem(privateKeyPem, password);

                bool success = SignManifest(manifest, privateKey);

                if (success)
                {
                    SaveManifestToFile(manifest, outputPath);
                    return true;
                }
                else
                {
                    Console.WriteLine("Failed to sign the manifest.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        private SUITManifest LoadManifestFromFile(string filePath)
        {

            return null;
        }

        private CngKey LoadPrivateKeyFromPem(string privateKeyPem, string password)
        {

            return null
        }

        private bool SignManifest(SUITManifest manifest, CngKey privateKey)
        {
            try
            {
                CBORObject manifestCbor = manifest.EncodeToCBOR();

                COSE_Sign1 coseSign1 = new COSE_Sign1();
                coseSign1.protectedHeaders.Add(COSE_KeyCommonParameter.Algorithm, AlgorithmIdentifiers.ECDSA_256);
                coseSign1.payload = manifestCbor.EncodeToBytes();
                coseSign1.signer = privateKey;
                coseSign1.ComputeSignature();

                manifestCbor.Add("signature", CBORObject.FromObject(coseSign1.EncodeToBytes()));

                manifest.DecodeFromCBOR(manifestCbor);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error signing manifest: {ex.Message}");
                return false;
            }
        }

    }
}
*/
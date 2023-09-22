using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using PeterO.Cbor;
using SuitSolution.Services;
using Xunit;
/*
namespace SuitSigner.Tests
{
    public class SuitSignerTests
    {
        private const string PrivateKeyFilePath = "path/to/your/private_key.pem";
        private const string ManifestFilePath = "path/to/your/manifest.cbor";
        private const string OutputFilePath = "path/to/output.cbor";
        private const string KeyType = "ES256"; 
        [Fact]
        public void SignManifest_Success()
        {
            var suitSigner = new SuitSigner();

            int result = suitSigner.SignManifest(PrivateKeyFilePath, ManifestFilePath, OutputFilePath);

            Assert.Equal(0, result);
            Assert.True(File.Exists(OutputFilePath));

            File.Delete(OutputFilePath);
        }

        [Fact]
        public void SignManifest_InvalidKey()
        {
            var suitSigner = new SuitSigner();

            int result = suitSigner.SignManifest("invalid_key.pem", ManifestFilePath, OutputFilePath);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public void SignManifest_InvalidManifest()
        {
            var suitSigner = new SuitSigner();

            int result = suitSigner.SignManifest(PrivateKeyFilePath, "invalid_manifest.cbor", OutputFilePath);

            Assert.NotEqual(0, result);
        }
    }

    public class SuitSigner
    {
        public int SignManifest(string privateKeyFilePath, string manifestFilePath, string outputFilePath)
        {
            try
            {
                var privateKeyBytes = File.ReadAllBytes(privateKeyFilePath);
                X509Certificate2 certificate = new X509Certificate2(privateKeyBytes, (string)null, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
                using RSA rsa = certificate.GetRSAPrivateKey();

                var manifestBytes = File.ReadAllBytes(manifestFilePath);
                CBORObject wrapper = CBORObject.DecodeFromBytes(manifestBytes);

                string key_type = KeyType; 

                HashAlgorithmName hashAlgorithmName = key_type switch
                {
                    "ES256" => HashAlgorithmName.SHA256,
                    "ES384" => HashAlgorithmName.SHA384,
                    "ES512" => HashAlgorithmName.SHA512,
                    "EdDSA" => HashAlgorithmName.SHA256, 
                    _ => null,
                };

                if (hashAlgorithmName == null)
                {
                    Console.WriteLine("Unsupported key type.");
                    return 1;
                }

                using var hasher = HashAlgorithm.Create(hashAlgorithmName.ToString());
                var digest = hasher.ComputeHash(wrapper["SUITEnvelope"]["manifest"].ToByteArray());


                File.WriteAllBytes(outputFilePath, wrapper.EncodeToBytes());
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 1;
            }
        }
    }
}*/

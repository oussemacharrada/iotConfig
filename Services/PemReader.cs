using System;
using System.IO;
using System.Text;

namespace SuitSolution.Services
{
    internal class PemReader
    {
        private readonly string pem;

        public bool IsEcKey { get; private set; }
        public bool IsRsaKey { get; private set; }
        public bool IsEd25519Key { get; private set; }

        public PemReader(string pem)
        {
            this.pem = pem;
            DetermineKeyType();
        }

        private void DetermineKeyType()
        {
            // Reset flags
            IsEcKey = false;
            IsRsaKey = false;
            IsEd25519Key = false;

            if (pem.Contains("BEGIN EC PRIVATE KEY"))
            {
                IsEcKey = true;
            }
            else if (pem.Contains("BEGIN RSA PRIVATE KEY"))
            {
                IsRsaKey = true;
            }
            else if (pem.Contains("BEGIN PRIVATE KEY")) // Common header for Ed25519 keys
            {
                // Additional checks can be implemented here to distinguish Ed25519 from other key types
                // that use the "BEGIN PRIVATE KEY" header, based on specific requirements or key structure.
                IsEd25519Key = true;
            }
        }

        public byte[] ExtractKeyBytes()
        {
            string base64Key = ExtractBase64Section();
            return Convert.FromBase64String(base64Key);
        }

        private string ExtractBase64Section()
        {
            StringBuilder builder = new StringBuilder();

            using (StringReader reader = new StringReader(pem))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("-----BEGIN") || line.StartsWith("-----END"))
                        continue;

                    builder.Append(line);
                }
            }

            return builder.ToString();
        }
    }
}
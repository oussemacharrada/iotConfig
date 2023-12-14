using System;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

public class PemReaderUtility
{
    public AsymmetricCipherKeyPair ReadKeyPair(string pemContent, string password = null)
    {
        using (var reader = new StringReader(pemContent))
        {
            PemReader pemReader;
            if (!string.IsNullOrEmpty(password))
            {
                // Use the password to decrypt the key
                pemReader = new PemReader(reader, new PasswordFinder(password));
            }
            else
            {
                // No password provided
                pemReader = new PemReader(reader);
            }

            var objectFromPem = pemReader.ReadObject();

            if (objectFromPem is AsymmetricCipherKeyPair keyPair)
            {
                return keyPair;
            }
            else
            {
                throw new InvalidOperationException("The PEM content did not contain a valid key pair.");
            }
        }
    }

    private class PasswordFinder : IPasswordFinder
    {
        private readonly string password;

        public PasswordFinder(string password)
        {
            this.password = password;
        }

        public char[] GetPassword()
        {
            return password.ToCharArray();
        }
    }


    public AsymmetricKeyParameter ReadPublicKey(string pemFilePath)
    {
        using (var reader = File.OpenText(pemFilePath))
        {
            var pemReader = new PemReader(reader);
            var objectFromPem = pemReader.ReadObject();

            if (objectFromPem is AsymmetricKeyParameter publicKey)
            {
                return publicKey;
            }
            else
            {
                throw new InvalidOperationException("The PEM file did not contain a valid public key.");
            }
        }
    }

    public AsymmetricKeyParameter ReadPrivateKey(string pemContent, string password = null)
    {
        using (var reader = new StringReader(pemContent))
        {
            PemReader pemReader;
            if (!string.IsNullOrEmpty(password))
            {
                pemReader = new PemReader(reader, new PasswordFinder(password));
            }
            else
            {
                pemReader = new PemReader(reader);
            }

            
            var objectFromPem = pemReader.ReadObject();

            if (objectFromPem is AsymmetricKeyParameter)
            {
                return (AsymmetricKeyParameter)objectFromPem;
            }
            else
            {
                throw new InvalidOperationException("The PEM content did not contain a valid private key.");
            }
        }
    }

}
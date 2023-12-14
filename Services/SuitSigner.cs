using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using PeterO.Cbor;
using SuitSolution.Services;

public class SUITSigner
{
    private object privateKey; // Bouncy Castle private key
    private string keyType;
    private byte[] manifest;
    private string password;

    public SUITDigestAlgo DigestAlgorithm { get; private set; }

    public SUITSigner(string privateKeyPem, byte[] manifest, string password = null)
    {
        LoadPrivateKey(privateKeyPem, password);
        this.manifest = manifest;
        this.password = password;
        DetermineKeyType();
        DigestAlgorithm = new SUITDigestAlgo();
    }

    public SUITBWrapField<COSEList> SignEnvelope(SUITEnvelope envelope)
    {
        Console.WriteLine($"envelope: {envelope}");
        Console.WriteLine($"keyType: {keyType}");
        
        var env = new SUITEnvelope();
        env = envelope;

        var serializedEnvelope = SerializeCbor(env.ToSUIT());

        byte[] digest;
        using (var hashAlgorithm = DigestAlgorithm.CreateHashAlgorithm())
        {
            digest = hashAlgorithm.ComputeHash(serializedEnvelope);
        }

        Console.WriteLine($"Digest: {BitConverter.ToString(digest)}");

        var coseSignature = new COSESign1();

        Console.WriteLine($"coseSignature (initialized): {coseSignature}");

        if (COSEAlgorithms.TryGetAlgorithmValue(keyType, out var algValue))
        {
            Console.WriteLine($"algValue: {algValue}");

            if (algValue != null)
            {
                coseSignature.Protected = new SUITBWrapField<COSEHeaderMap>(new COSEHeaderMap
                {
                    Alg = new COSEAlgorithms(algValue)
                });
            }
            else
            {
                Console.WriteLine("algValue is null or invalid.");
            }
        }
        else
        {
            throw new InvalidOperationException($"Unsupported algorithm: {keyType}");
        }
        
        coseSignature.Unprotected = new COSEHeaderMap();

        coseSignature.Payload = new SUITBWrapField<SUITDigest>(new SUITDigest
        {
            AlgorithmId = new SUITDigestAlgo(),
            DigestBytes = new SUITBytes(digest)
        });

        var sigStructure = GetSignatureStructure(coseSignature);
        byte[] signature = SignData(sigStructure);

        Console.WriteLine($"Signature: {BitConverter.ToString(signature)}");

        coseSignature.Signature = new SUITBytes { v = signature };

        var coseTaggedAuth = new COSETaggedAuth { CoseSign1 = coseSignature };
        var coseList = new COSEList();
        coseList.Items.Add(coseTaggedAuth);
        envelope.Auth = new SUITBWrapField<COSEList> { v = coseList };

        return envelope.Auth;
    }

    private void LoadPrivateKey(string pem, string password)
    {
        var pemReaderUtility = new PemReaderUtility();
        AsymmetricKeyParameter privateKeyParam = pemReaderUtility.ReadPrivateKey(pem, password);

        if (privateKeyParam is Ed25519PrivateKeyParameters ed25519PrivateKey)
        {
            privateKey = ed25519PrivateKey;
            keyType = "EdDSA";
        }
        else
        {
            throw new InvalidOperationException("Unsupported key type. Only Ed25519 is supported.");
        }
    }

    private byte[] SignData(byte[] data)
    {
        if (privateKey is Ed25519PrivateKeyParameters ed25519Key)
        {
            var signer = new Ed25519Signer();
            signer.Init(true, ed25519Key);
            signer.BlockUpdate(data, 0, data.Length);
            return signer.GenerateSignature();
        }

        throw new InvalidOperationException("Unsupported key type for signing. Only Ed25519 is supported.");
    }

    private void DetermineKeyType()
    {
        if (keyType != "EdDSA")
        {
            throw new InvalidOperationException("Unsupported key type. Only Ed25519 is supported.");
        }
    }

    private byte[] GetSignatureStructure(COSESign1 coseSignature)
    {
        var protectedHeadersSerialized = SerializeCbor(coseSignature.Protected);
        byte[] payloadBytes = coseSignature.Payload.v.DigestBytes.v;
        
        var sigStructure = new List<byte[]>
        {
            Encoding.UTF8.GetBytes("Signature1"),
            protectedHeadersSerialized,
            new byte[0],
            payloadBytes
        };

        return ConcatenateByteArrays(sigStructure);
    }

    private byte[] ConcatenateByteArrays(List<byte[]> arrays)
    {
        var combined = new byte[arrays.Sum(a => a.Length)];
        int offset = 0;
        foreach (var array in arrays)
        {
            Buffer.BlockCopy(array, 0, combined, offset, array.Length);
            offset += array.Length;
        }
        return combined;
    }

    private byte[] SerializeCbor<T>(T data)
    {
        try
        {
            var cborObject = CBORObject.FromObject(data);
            return cborObject.EncodeToBytes();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error serializing data to CBOR", ex);
        }
    }
}

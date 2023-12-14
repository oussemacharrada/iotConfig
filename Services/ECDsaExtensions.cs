namespace SuitSolution.Services;
using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

public static class ECDsaExtensions
{
    public static ECDsa ToECDsa(this ECPrivateKeyParameters privateKey)
    {
        // Extract the domain parameters
        var domainParams = privateKey.Parameters;
        var q = domainParams.G.Multiply(privateKey.D).Normalize();

        // Determine the curve name
        string curveName = GetCurveName(domainParams.Curve);

        // Create an ECDsa object and import parameters
        ECDsa ecdsa = ECDsa.Create(ECCurve.CreateFromFriendlyName(curveName));
        ecdsa.ImportParameters(new ECParameters
        {
            Curve = ECCurve.CreateFromFriendlyName(curveName),
            D = privateKey.D.ToByteArrayUnsigned(),
            Q = new ECPoint
            {
                X = q.AffineXCoord.GetEncoded(),
                Y = q.AffineYCoord.GetEncoded()
            }
        });

        return ecdsa;
    }

    private static string GetCurveName(Org.BouncyCastle.Math.EC.ECCurve curve)
    {
        // This is a basic mapping, you might need to extend this based on the curves you expect to handle
        if (curve.Equals(Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp256r1").Curve))
        {
            return "nistP256";
        }
        else if (curve.Equals(Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp384r1").Curve))
        {
            return "nistP384";
        }
        else if (curve.Equals(Org.BouncyCastle.Asn1.Sec.SecNamedCurves.GetByName("secp521r1").Curve))
        {
            return "nistP521";
        }
        else
        {
            throw new ArgumentException("Unsupported curve");
        }
    }

}

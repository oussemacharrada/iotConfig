using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using PeterO.Cbor;
using SuitSolution.Services;

namespace SuitSolution.Tests
{
    public class SUITManifestDictTests
    {
        [Fact]
        public void CanEncodeAndDecodeFromBytes()
        {
            var suitDependency1 = new SUITDependency { Prefix = "dep1", Digest = "010203" };
            var suitDependency2 = new SUITDependency { Prefix = "dep2", Digest = "040506" };
            var suitDependencyList = new List<SUITDependency> { suitDependency1, suitDependency2 };

            var suitManifestDict = new SUITManifestDict
            {
                Version = 1,
                Sequence = 42,
                Dependencies = suitDependencyList,
            };

            suitManifestDict.DependenciesWrap.Add(new SUITBWrapField<COSEList>());

            var bytes = suitManifestDict.EncodeToBytes();

            var decodedManifestDict = new SUITManifestDict();
            decodedManifestDict.DecodeFromBytes(bytes);

            Assert.Equal(suitManifestDict.Version, decodedManifestDict.Version);
            Assert.Equal(suitManifestDict.Sequence, decodedManifestDict.Sequence);

            Assert.Equal(suitManifestDict.Dependencies.Count, decodedManifestDict.Dependencies.Count);
            Assert.Equal(
                suitManifestDict.Dependencies[0].Prefix,
                decodedManifestDict.Dependencies[0].Prefix);
            Assert.Equal(
                suitManifestDict.Dependencies[0].Digest,
                decodedManifestDict.Dependencies[0].Digest);

            Assert.Single(decodedManifestDict.DependenciesWrap); 
        }

        [Fact]
        public void CanAddAndDecodeDependenciesWrap()
        {
            var suitManifestDict = new SUITManifestDict
            {
                Version = 1,
                Sequence = 42,
                Dependencies = new List<SUITDependency>(),
            };

            var depWrap1 = new SUITBWrapField<COSEList>();
            var depWrap2 = new SUITBWrapField<COSEList>();

            suitManifestDict.DependenciesWrap.AddRange(new[] { depWrap1, depWrap2 });

            var bytes = suitManifestDict.EncodeToBytes();

            var decodedManifestDict = new SUITManifestDict();
            decodedManifestDict.DecodeFromBytes(bytes);

            Assert.Equal(2, decodedManifestDict.DependenciesWrap.Count);
        }

      /*  [Fact]
        public void ThrowsOnInvalidDecode()
        {
            var invalidBytes = Encoding.UTF8.GetBytes("{\"version\":1,\"sequence\":42,\"dependencies\":[],\"dependenciesWrap\":[null]}");

            var decodedManifestDict = new SUITManifestDict();

            Assert.Throws<ArgumentException>(() => decodedManifestDict.DecodeFromBytes(invalidBytes));
        }
        
        */
  
    }
}
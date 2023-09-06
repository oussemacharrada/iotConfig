using System.Collections.Generic;
using PeterO.Cbor;
using SuitSolution.Services;
using Xunit;

namespace SuitSolution.Tests
{
    public class SUITCommonTests
    {
        [Fact]
        public void ToCbor_SerializesToCBORObject()
        {
            var common = new SUITCommon
            {
                Sequence = 123,
                Install = new List<int> { 1, 2, 3 },
                Validate = new List<int> { 4, 5 }
            };

            var cborObject = common.ToCbor();

            Assert.NotNull(cborObject);
            Assert.Equal(CBORType.Map, cborObject.Type);

            Assert.True(cborObject.ContainsKey("sequence"));
            Assert.True(cborObject.ContainsKey("install"));
            Assert.True(cborObject.ContainsKey("validate"));

            Assert.Equal(123, cborObject["sequence"].AsInt32());

            var installArray = cborObject["install"];
            Assert.NotNull(installArray);
            Assert.Equal(1, installArray[0].AsInt32());
            Assert.Equal(2, installArray[1].AsInt32());
            Assert.Equal(3, installArray[2].AsInt32());

            var validateArray = cborObject["validate"];
            Assert.NotNull(validateArray);
            Assert.Equal(4, validateArray[0].AsInt32());
            Assert.Equal(5, validateArray[1].AsInt32());
        }


        [Fact]
        public void FromSUIT_DeserializesFromList()
        {
            var suitList = new List<object>
            {
                123,
                new List<object> { 1, 2, 3 },
                new List<object> { 4, 5 }
            };

            var common = new SUITCommon();

            common.FromSUIT(suitList);

            Assert.Equal(123, common.Sequence);
            Assert.NotNull(common.Install);
            Assert.Equal(3, common.Install.Count);
            Assert.Equal(1, common.Install[0]);
            Assert.Equal(2, common.Install[1]);
            Assert.Equal(3, common.Install[2]);

            Assert.NotNull(common.Validate);
            Assert.Equal(2, common.Validate.Count);
            Assert.Equal(4, common.Validate[0]);
            Assert.Equal(5, common.Validate[1]);
        }

        [Fact]
        public void ToSUITDict_SerializesToDictionary()
        {
            var common = new SUITCommon
            {
                Sequence = 123,
                Install = new List<int> { 1, 2, 3 },
                Validate = new List<int> { 4, 5 }
            };
             var suitDict = common.ToSUITDict();

            Assert.NotNull(suitDict);
            Assert.Equal(123, suitDict["sequence"]);
            Assert.NotNull(suitDict["install"]);
            Assert.NotNull(suitDict["validate"]);
            Assert.Equal(new List<int> { 1, 2, 3 }, (List<int>)suitDict["install"]);
            Assert.Equal(new List<int> { 4, 5 }, (List<int>)suitDict["validate"]);
        }

        [Fact]
        public void FromSUITDict_DeserializesFromDictionary()
        {
            var suitDict = new Dictionary<object, object>
            {
                { "sequence", 123 },
                { "install", new List<object> { 1, 2, 3 } },
                { "validate", new List<object> { 4, 5 } }
            };

            var common = new SUITCommon();

            common.FromSUITDict(suitDict);

            Assert.Equal(123, common.Sequence);
            Assert.NotNull(common.Install);
            Assert.Equal(3, common.Install.Count);
            Assert.Equal(1, common.Install[0]);
            Assert.Equal(2, common.Install[1]);
            Assert.Equal(3, common.Install[2]);

            Assert.NotNull(common.Validate);
            Assert.Equal(2, common.Validate.Count);
            Assert.Equal(4, common.Validate[0]);
            Assert.Equal(5, common.Validate[1]);
        }
    }
}

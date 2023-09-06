using System;
using System.Collections.Generic;
using Xunit;
using SuitSolution.Services;
using SuitSolution.Services.SuitSolution.Services;

namespace SuitSolution.Tests
{
    public class SUITBWrapFieldTests
    {
        private class SampleConvertible :  ISUITConvertible
        {
            public string Name { get; set; }
            public int Age { get; set; }

            public List<object> ToSUIT()
            {
                return new List<object> { Name, Age };
            }

            public void FromSUIT(List<Object> suitList)
            {
                if (suitList.Count != 2)
                {
                    throw new Exception("Invalid SUIT list size for SampleConvertible");
                }

                Name = suitList[0] as string ?? throw new Exception("Name should be a string.");
                Age = Convert.ToInt32(suitList[1]);
            }
        }

        [Fact]
        public void ToJson_SerializesObjectToJson()
        {
            var wrappedObject = new SampleConvertible { Name = "John", Age = 30 };
            var sut = new SUITBWrapField<SampleConvertible>(wrappedObject);

            var json = sut.ToJson();

            Assert.Contains("John", json);
            Assert.Contains("30", json);
        }

        [Fact]
        public void FromJson_DeserializesJsonToObject()
        {
            var json = "{\"obj\":{\"Name\":\"Alice\",\"Age\":25}}";
            var sut = new SUITBWrapField<SampleConvertible>().FromJson(json);

            Assert.NotNull(sut);
            Assert.NotNull(sut.WrappedObject);
            Assert.Equal("Alice", sut.WrappedObject.Name);
            Assert.Equal(25, sut.WrappedObject.Age);
        }

        [Fact]
        public void ToSUIT_ConvertsObjectToSUIT()
        {
            var wrappedObject = new SampleConvertible { Name = "Bob", Age = 35 };
            var sut = new SUITBWrapField<SampleConvertible>(wrappedObject);

            var suitList = sut.ToSUIT();

            Assert.NotNull(suitList);
            Assert.Collection(suitList,
                item => Assert.Equal("Bob", item),
                item => Assert.Equal(35, item));
        }

        [Fact]
        public void FromSUIT_ConvertsSUITToObject()
        {
            var suitList = new List<object> { "Eve", 40 };

            var sut = new SUITBWrapField<SampleConvertible>();
            sut.FromSUIT(suitList);

            Assert.NotNull(sut.WrappedObject);
            Assert.Equal("Eve", sut.WrappedObject.Name);
            Assert.Equal(40, sut.WrappedObject.Age);
        }
    }
}

using System;
using System.Collections.Generic;
using SuitSolution.Services;
using Xunit;

namespace YourNamespace.Tests
{
    public class COSETaggedAuthTests
    {
        [Fact]
        public void COSETaggedAuth_ToSUITAndFromSUIT_Success()
        {
            var coseTaggedAuth = new COSETaggedAuth
            {
                 CoseSign = new COSESign {  },
                 CoseSign1 = new COSESign1 {  },
                 CoseMac = new COSE_Mac {  },
                 CoseMac0 = new COSE_Mac0 {  },
            };

            var suitList = coseTaggedAuth.ToSUIT();
            var newCOSETaggedAuth = new COSETaggedAuth();
            newCOSETaggedAuth.FromSUIT(suitList);

            Assert.Equal(coseTaggedAuth, newCOSETaggedAuth); 
        }
    }
}
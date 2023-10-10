using System;
using System.Collections.Generic;
using SuitSolution.Services;

namespace SuitSolution.Services
{
    public struct ComponentIndex
    {
        private readonly int value;

        public ComponentIndex(int value)
        {
            this.value = value;
        }

        public int ToInt32()
        {
            return value;
        }

        public static implicit operator ComponentIndex(int value)
        {
            return new ComponentIndex(value);
        }

        public static implicit operator int(ComponentIndex componentIndex)
        {
            return componentIndex.value;
        }
    }



            public class SUITReportingPolicy : SUITPosInt
            {
                public int default_policy { get; }

                public SUITReportingPolicy(int policy)
                {
                    default_policy = policy;
                }

                public  Dictionary<string, object> ToSUIT()
                {
                    return new Dictionary<string, object>
                    {
                        { "policy", default_policy }
                    };
                }
            }

        public class SUITReportingPolicyCommand : SUITCommand
        {
            public string JsonKey { get; }
            public string SuitKey { get; }
            public List<string> DepParams { get; }

            public SUITReportingPolicyCommand(string jsonKey, string suitKey, List<string> depParams)
            {
                JsonKey = jsonKey;
                SuitKey = suitKey;
                DepParams = depParams;
            }

            public  List<object> ToSUIT()
            {
                var suitDict = new Dictionary<string, object>
                {
                    { "command-id", JsonKey },
                    { "command-arg", new SUITReportingPolicy(0).ToSUIT() }, 
                    { "component-id", SUITCommonInfo.CurrentIndex }
                };

                if (DepParams != null && DepParams.Any())
                {
                    suitDict.Add("dep-params", DepParams);
                }

                return new List<object> { suitDict };
            }
        }
    

}

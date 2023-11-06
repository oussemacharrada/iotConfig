using System;
using System.Collections.Generic;
using SuitSolution.Interfaces;
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
        public int DefaultPolicy { get; private set; }

        public SUITReportingPolicy(int policy)
        {
            DefaultPolicy = policy;
        }

        public SUITReportingPolicy()
        {
        }

        public static SUITReportingPolicy Create(int policy)
        {
            return new SUITReportingPolicy(policy);
        }

        public new SUITReportingPolicy FromSUIT(Dictionary<object, object> suitDict)
        {
            if (suitDict == null)
            {
                throw new ArgumentNullException(nameof(suitDict));
            }

            if (suitDict.TryGetValue("policy", out var policyValue) && policyValue is int policy)
            {
                DefaultPolicy = policy;
            }
            else
            {
                throw new ArgumentException("The 'policy' key is missing or is not an integer in the SUIT dictionary.");
            }

            return this;
        }

        public new SUITReportingPolicy FromJson(Dictionary<string, object> jsonData)
        {
            if (jsonData == null)
            {
                throw new ArgumentNullException(nameof(jsonData));
            }

            if (jsonData.TryGetValue("policy", out var policyValue) && policyValue is int policy)
            {
                DefaultPolicy = policy;
            }
            else
            {
                throw new ArgumentException("The 'policy' key is missing or is not an integer in the JSON data.");
            }

            return this;
        }

        public Dictionary<string, object> ToSUIT()
        {
            return new Dictionary<string, object>
            {
                { "policy", DefaultPolicy }
            };
        }

        public dynamic ToJson()
        {
            return new Dictionary<string, object>
            {
                { "policy", DefaultPolicy }
            };
        }

        public new SUITReportingPolicy FromJson(string j)
        {
            if (int.TryParse(j, out var policy))
            {
                DefaultPolicy = policy;
                return this;
            }
            else
            {
                throw new ArgumentException("The input string is not a valid integer.");
            }
        }
    }
}

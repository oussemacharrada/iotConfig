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
    public class SUITReportingPolicy
    {
        public int Policy { get; set; }

        public SUITReportingPolicy(int defaultPolicy)
        {
            Policy = defaultPolicy;
        }
        public object ToSUIT()
        {
            return this.Policy;
        }

        public void FromJson(Dictionary<string, object> jsonData)
        {
            if (jsonData != null && jsonData.TryGetValue("policy", out var policyValue))
            {
                Policy = Convert.ToInt32(policyValue);
            }
        }
        private static readonly Dictionary<string, int> CommandDefaultPolicies = new Dictionary<string, int>
        {
            {"condition-vendor-identifier", 0xF},
            {"condition-class-identifier", 0xF},
            {"condition-image-match", 0xF},
            {"condition-use-before", 0xA},
            {"condition-component-offset", 0x5},
            {"condition-device-identifier", 0xF},
            {"condition-image-not-match", 0xF},
            {"condition-minimum-battery", 0xA},
            {"condition-update-authorised", 0x3},
            {"condition-version", 0xF},
            {"directive-set-component-index", 0}, 
            {"directive-set-dependency-index", 0},
            {"directive-abort", 0x2},
            {"directive-try-each", 0}, 
            {"directive-process-dependency", 0},
            {"directive-set-parameters", 0},
            {"directive-override-parameters", 0},
            {"directive-fetch", 0x2},
            {"directive-copy", 0x2},
            {"directive-run", 0x2},
            {"directive-wait", 0x2},
            {"directive-run-sequence", 0},
            {"directive-run-with-arguments", 0},
            {"directive-swap", 0x2},
        };

        public static int GetDefaultPolicyForCommand(string jsonKey)
        {
            if (CommandDefaultPolicies.TryGetValue(jsonKey, out int defaultPolicy))
            {
                return defaultPolicy;
            }

            throw new ArgumentException($"No default policy defined for command type: {jsonKey}");
        }

    public static SUITReportingPolicy MkPolicy(int policy)
    {
        return new SUITReportingPolicy(policy);
    }

}
}
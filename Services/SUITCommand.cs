using System;
using System.Collections.Generic;
using System.Linq;
using PeterO.Cbor;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITCommand 
    {
        private static readonly Dictionary<string, SUITCommandContainer> jcommands = new Dictionary<string, SUITCommandContainer>();
        private static readonly Dictionary<int, SUITCommandContainer> scommands = new Dictionary<int, SUITCommandContainer>();
        
        public static List<SUITCommandContainer> commands = new List<SUITCommandContainer>
{
    new SUITCommandContainer("condition-vendor-identifier", 1, typeof(SUITReportingPolicy), new List<string> { "vendor-id" }),
    new SUITCommandContainer("condition-class-identifier", 2, typeof(SUITReportingPolicy), new List<string> { "class-id" }),
    new SUITCommandContainer("condition-image-match", 3, typeof(SUITReportingPolicy), new List<string> { "digest" }),
    new SUITCommandContainer("condition-use-before", 4, typeof(SUITReportingPolicy)),
    new SUITCommandContainer("condition-component-offset", 5, typeof(SUITReportingPolicy), new List<string> { "offset" }),
    new SUITCommandContainer("condition-device-identifier", 24, typeof(SUITReportingPolicy)),
    new SUITCommandContainer("condition-image-not-match", 25, typeof(SUITReportingPolicy)),
    new SUITCommandContainer("condition-minimum-battery", 26, typeof(SUITReportingPolicy)),
    new SUITCommandContainer("condition-update-authorised", 27, typeof(SUITReportingPolicy)),
    new SUITCommandContainer("condition-version", 28, typeof(SUITReportingPolicy)),
    new SUITCommandContainer("directive-set-component-index", 12, typeof(SUITPosInt)),
    new SUITCommandContainer("directive-set-dependency-index", 13, typeof(SUITPosInt)),
    new SUITCommandContainer("directive-abort", 14, typeof(SUITReportingPolicy)),
    new SUITCommandContainer("directive-try-each", 15, typeof(SUITTryEach)),
    new SUITCommandContainer("directive-process-dependency", 18, typeof(SUITReportingPolicy)),
    new SUITCommandContainer("directive-set-parameters", 19, typeof(SUITParameters)),
    new SUITCommandContainer("directive-override-parameters", 20, typeof(SUITParameters)),
    new SUITCommandContainer("directive-fetch", 21, typeof(SUITReportingPolicy)),
    new SUITCommandContainer("directive-copy", 22, typeof(SUITReportingPolicy)),
    new SUITCommandContainer("directive-run", 23, typeof(SUITReportingPolicy)),
    new SUITCommandContainer("directive-wait", 29, typeof(SUITReportingPolicy)),
    new SUITCommandContainer("directive-run-sequence", 30, typeof(SUITRaw)),
    new SUITCommandContainer("directive-run-with-arguments", 31, typeof(SUITRaw)),
    new SUITCommandContainer("directive-swap", 32, typeof(SUITReportingPolicy))
};
        
 static SUITCommand()
        {
            foreach (var c in commands)
            {
                jcommands[c.JsonKey] = c;
                scommands[c.SuitKey] = c;
            }
        }


 public SUITCommand FromJson(Dictionary<string, object> j)
        {
            if (j == null)
            {
                throw new ArgumentNullException(nameof(j));
            }

            if (!j.TryGetValue("command-id", out var commandIdValue) || !(commandIdValue is string commandId))
            {
                throw new ArgumentException("Missing or invalid 'command-id' in the JSON dictionary.");
            }

            if (!jcommands.TryGetValue(commandId, out var commandContainer))
            {
                throw new Exception($"Unknown JSON Key: {commandId}");
            }

            var commandInstance = commandContainer.CreateCommand().FromJson(j);
            return commandInstance;
        }


        public dynamic ToJson(string commandId)
        {
            if (!jcommands.TryGetValue(commandId, out var commandContainer))
            {
                throw new Exception($"Unknown JSON Key: {commandId}");
            }

            var commandInstance = commandContainer.CreateCommand();
            return commandInstance.ToJson();
        }
        public dynamic ToSUIT(int suitKey)
        {
            if (!scommands.TryGetValue(suitKey, out var commandContainer))
            {
                throw new Exception($"Unknown SUIT Key: {suitKey}");
            }

            var commandInstance = commandContainer.CreateCommand();
            return commandInstance.ToSUIT();
        }


        public SUITCommand FromSUIT(List<object> s)
        {
            if (s == null || s.Count < 1)
            {
                throw new ArgumentException("Invalid SUIT data.");
            }

            if (!(s[0] is int suitKey))
            {
                throw new ArgumentException("Invalid 'suitKey' in the SUIT data.");
            }

            if (!scommands.TryGetValue(suitKey, out var commandContainer))
            {
                throw new Exception($"Unknown SUIT Key: {suitKey}");
            }

            var commandInstance = commandContainer.CreateCommand().FromSUIT(s);
            return commandInstance;
        }

        public dynamic ToJson()
        {
            var jsonRepresentation = new Dictionary<string, object>();

            foreach (var command in commands)
            {
                var commandInstance = command.CreateCommand();
                jsonRepresentation[command.JsonKey] = commandInstance.ToJson();
            }

            return jsonRepresentation;
        }

      
    }
}
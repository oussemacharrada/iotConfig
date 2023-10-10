using System;
using System.Collections.Generic;
using System.Linq;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITCommand : ISUITConvertible<SUITCommand>
    {
        private static readonly Dictionary<string, SUITCommandContainer> jcommands =
            new Dictionary<string, SUITCommandContainer>();

        private static readonly Dictionary<string, SUITCommandContainer> scommands =
            new Dictionary<string, SUITCommandContainer>();

        public SUITCommand()
        {
            foreach (var c in commands)
            {
                jcommands[c.json_key] = c;
                scommands[c.suit_key] = c;
            }
        }

         public List<SUITCommandContainer> commands = new List<SUITCommandContainer>
        {
            SUITCommandContainer.Create("condition-vendor-identifier", "1", typeof(SUITReportingPolicy),
                dp: new List<string> { "vendor-id" }),
            SUITCommandContainer.Create("condition-class-identifier", "2", typeof(SUITReportingPolicy),
                dp: new List<string> { "class-id" }),
            SUITCommandContainer.Create("condition-image-match", "3", typeof(SUITReportingPolicy),
                dp: new List<string> { "digest" }),
            SUITCommandContainer.Create("condition-use-before", "4", typeof(SUITReportingPolicy),
                dp: new List<string>()),
            SUITCommandContainer.Create("condition-component-offset", "5", typeof(SUITReportingPolicy),
                dp: new List<string> { "offset" }),
            SUITCommandContainer.Create("condition-device-identifier", "24", typeof(SUITReportingPolicy)),
            SUITCommandContainer.Create("condition-image-not-match", "25", typeof(SUITReportingPolicy)),
            SUITCommandContainer.Create("condition-minimum-battery", "26", typeof(SUITReportingPolicy)),
            SUITCommandContainer.Create("condition-update-authorised", "27", typeof(SUITReportingPolicy)),
            SUITCommandContainer.Create("condition-version", "28", typeof(SUITReportingPolicy)),
            SUITCommandContainer.Create("directive-set-component-index", "12", typeof(SUITPosInt)),
            SUITCommandContainer.Create("directive-set-dependency-index", "13", typeof(SUITPosInt)),
            SUITCommandContainer.Create("directive-abort", "14", typeof(SUITReportingPolicy)),
            SUITCommandContainer.Create("directive-try-each", "15", typeof(SUITTryEach)),
            SUITCommandContainer.Create("directive-process-dependency", "18", typeof(SUITReportingPolicy)),
            SUITCommandContainer.Create("directive-set-parameters", "19", typeof(SUITParameters)),
            SUITCommandContainer.Create("directive-override-parameters", "20", typeof(SUITParameters)),
            SUITCommandContainer.Create("directive-fetch", "21", typeof(SUITReportingPolicy)),
            SUITCommandContainer.Create("directive-copy", "22", typeof(SUITReportingPolicy)),
            SUITCommandContainer.Create("directive-run", "23", typeof(SUITReportingPolicy)),
            SUITCommandContainer.Create("directive-wait", "29", typeof(SUITReportingPolicy)),
            SUITCommandContainer.Create("directive-run-sequence", "30", typeof(SUITRaw)),
            SUITCommandContainer.Create("directive-run-with-arguments", "31", typeof(SUITRaw)),
            SUITCommandContainer.Create("directive-swap", "32", typeof(SUITReportingPolicy)),
        };

        public SUITCommand FromJson(Dictionary<string, object> j)
        {
            if (j == null)
            {
                throw new ArgumentNullException(nameof(j));
            }

            if (!j.TryGetValue("command-id", out var commandIdValue) || !(commandIdValue is string commandId))
            {
                throw new ArgumentException("Missing or invalid 'command-id' in the SUIT dictionary.");
            }

            if (!jcommands.ContainsKey(commandId))
            {
                throw new Exception($"Unknown JSON Key: {commandId}");
            }

            var commandContainer = jcommands[commandId];

            if (!j.TryGetValue("command-arg", out var commandArgsValue) ||
                !(commandArgsValue is string commandArgs))
            {
                throw new ArgumentException("Missing or invalid 'command-arg' in the SUIT dictionary.");
            }

            var commandInstance = commandContainer.CreateDic(j);

            return commandInstance;
        }

        public Dictionary<string, object> ToSUIT()
        {
            var suitDict = new Dictionary<string, object>
            {
                { "command-id", "some-command-id" },
            };

            return suitDict;
        }

        public Dictionary<string, object> ToJson()
        {
            var jsonDict = new Dictionary<string, object>
            {
                { "command-id", "some-command-id" },
                { "command-arg", "some-command-arg" },
            };

            return jsonDict;
        }

        public string ToDebug(string indent)
        {
            return $"Debug info for SUITCommand with indent {indent}";
        }

        public SUITCommand FromSUIT(Dictionary<string, object> suitDict)
        {
            if (suitDict == null)
            {
                throw new ArgumentNullException(nameof(suitDict));
            }

            if (!suitDict.TryGetValue("command-id", out var commandIdValue) || !(commandIdValue is string commandId))
            {
                throw new ArgumentException("Missing or invalid 'command-id' in the SUIT dictionary.");
            }

            if (!jcommands.ContainsKey(commandId))
            {
                throw new Exception($"Unknown JSON Key: {commandId}");
            }

            var commandContainer = jcommands[commandId];

            if (!suitDict.TryGetValue("command-arg", out var commandArgsValue) ||
                !(commandArgsValue is Dictionary<string, object> commandArgs))
            {
                throw new ArgumentException("Missing or invalid 'command-arg' in the SUIT dictionary.");
            }

            var commandInstance = commandContainer.CreateDic(commandArgs);

            return commandInstance;
        }
    }
}

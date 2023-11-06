using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITCommandContainer
    {
        public string JsonKey { get; }
        public string SuitKey { get; }
        public Type ArgType { get; }
        public List<string> DepParams { get; }

        public SUITCommandContainer(string jkey, string skey, Type argtype, List<string> dp = null)
        {
            JsonKey = jkey;
            SuitKey = skey;
            ArgType = argtype;
            DepParams = dp ?? new List<string>();
        }

        public SUITCmd CreateCommand()
        {
            return new SUITCmd(this);
        }

        public class SUITCmd : SUITCommand
        {
            public string JsonKey { get; }
            public string SuitKey { get; set; }
            public List<string> DepParams { get; set; }
            public Type ArgType { get; }
            private object argInstance;
            private object cid;

            public SUITCmd(SUITCommandContainer container)
            {
                JsonKey = container.JsonKey;
                SuitKey = container.SuitKey;
                DepParams = new List<string>(container.DepParams);
                ArgType = container.ArgType;
                argInstance = Activator.CreateInstance(ArgType);
            }

            public List<object> ToSUIT()
            {
                var suitRepresentation = new List<object> { SuitKey, argInstance };

                // Include dependent parameters if any
                if (DepParams != null && DepParams.Count > 0)
                {
                    suitRepresentation.AddRange(DepParams.Select(dp => (object)dp));
                }

                return suitRepresentation;
            }

            public Dictionary<string, object> ToJson()
            {
                var jsonRepresentation = new Dictionary<string, object>
                {
                    { "command-id", JsonKey }
                };

                if (JsonKey != "directive-set-component-index")
                {
                    jsonRepresentation["command-arg"] = ((dynamic)argInstance).ToJson();
                    jsonRepresentation["component-id"] = ((dynamic)cid).ToJson();
                }

                // Include dependent parameters if any
                if (DepParams != null && DepParams.Count > 0)
                {
                    jsonRepresentation["dependent-params"] = DepParams;
                }

                return jsonRepresentation;
            }

       public SUITCmd FromJson(Dictionary<string, object> jsonData)
{
    if (jsonData == null)
    {
        throw new ArgumentNullException(nameof(jsonData));
    }

    if (!jsonData.TryGetValue("command-id", out var commandId) || (string)commandId != JsonKey)
    {
        throw new ArgumentException("JSON Key mismatch error");
    }

    if (JsonKey != "directive-set-component-index")
    {
        if (jsonData.TryGetValue("command-arg", out var commandArg))
        {
            if (commandArg is Dictionary<string, object> commandArgDict)
            {
                var instance = Activator.CreateInstance(ArgType);
                var convertibleInterface = typeof(ISUITConvertible<>).MakeGenericType(ArgType);
                if (convertibleInterface.IsInstanceOfType(instance))
                {
                    var fromJsonMethod = convertibleInterface.GetMethod("FromJson");
                    argInstance = fromJsonMethod.Invoke(instance, new object[] { commandArgDict });
                }
                else if (instance is ISUITObject suitObject)
                {
                    argInstance = suitObject.FromJson(JsonConvert.SerializeObject(commandArgDict));
                }
                else
                {
                    throw new InvalidOperationException($"The type {ArgType.Name} does not implement ISUITConvertible<{ArgType.Name}> or ISUITObject.");
                }
            }
            else
            {
                throw new ArgumentException("Expected 'command-arg' to be a dictionary.");
            }
        }

        if (jsonData.TryGetValue("component-id", out var componentId) && componentId is Dictionary<string, object> componentIdDict)
        {
            var componentInstance = Activator.CreateInstance(typeof(SUITComponentId));
            var convertibleInterface = typeof(ISUITConvertible<>).MakeGenericType(componentInstance.GetType());
            if (convertibleInterface.IsInstanceOfType(componentInstance))
            {
                var fromJsonMethod = convertibleInterface.GetMethod("FromJson");
                cid = fromJsonMethod.Invoke(componentInstance, new object[] { componentIdDict });
            }
            else if (componentInstance is ISUITObject objectConvertible)
            {
                cid = objectConvertible.FromJson(JsonConvert.SerializeObject(componentIdDict));
            }
            else
            {
                throw new InvalidOperationException("The type for 'component-id' does not implement ISUITConvertible<T> or ISUITObject.");
            }
        }
    }

    if (jsonData.TryGetValue("dependent-params", out var dependentParams) && dependentParams is IEnumerable<object> depParamsEnum)
    {
        DepParams = depParamsEnum.Cast<string>().ToList();
    }

    return this;
}


            // Implement the FromSUIT method
            public SUITCmd FromSUIT(List<object> suitList)
            {
                if (suitList == null)
                {
                    throw new ArgumentNullException(nameof(suitList));
                }

                if (suitList[0] as string != SuitKey)
                {
                    throw new ArgumentException("SUIT Key mismatch error");
                }

                // Handle the command argument and component id
                if (suitList.Count > 1)
                {
                    argInstance = ((dynamic)Activator.CreateInstance(ArgType)).FromSUIT((List<object>)suitList[1]);
                }

                // Handle dependent parameters
                if (suitList.Count > 2)
                {
                    DepParams = suitList.Skip(2).Cast<string>().ToList();
                }

                return this;
            }

            // Implement the ToDebug method
            public string ToDebug(string indent)
            {
                var debugString = $"{indent}/ {JsonKey} / {SuitKey},";
                debugString += ((dynamic)argInstance).ToDebug(indent);
                return debugString;
            }
        }
    }
}

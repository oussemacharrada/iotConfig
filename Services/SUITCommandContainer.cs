using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PeterO.Cbor;
using SuitSolution.Interfaces;

namespace SuitSolution.Services
{
    public class SUITCommandContainer
    {
        public string JsonKey { get; }
        public int SuitKey { get; }
        public Type ArgType { get; }
        public List<string> DepParams { get; }

        public SUITCommandContainer(string jkey, int skey, Type argtype, List<string> dp = null)
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
            public string JsonKey { get; private set; }
            public int SuitKey { get; private set; }
            public List<string> DepParams { get; private set; }
            public Type ArgType { get; private set; }
            public SUITComponentId cid { get; private set; }
            public object argInstance { get; private set; }

            public SUITCmd(string jsonKey, int suitKey, Type argType, List<string> depParams = null)
            {
                JsonKey = jsonKey;
                SuitKey = suitKey;
                ArgType = argType;
                DepParams = depParams ?? new List<string>();
            }
            

            public SUITCmd(SUITCommandContainer suitCommandContainer)
            {
                if (suitCommandContainer == null)
                    throw new ArgumentNullException(nameof(suitCommandContainer));

                JsonKey = suitCommandContainer.JsonKey;
                SuitKey = suitCommandContainer.SuitKey;
                ArgType = suitCommandContainer.ArgType;
                DepParams = new List<string>(suitCommandContainer.DepParams);
            }
            public dynamic ToSUIT()
            {
                var suitRepresentation = new List<object>();

                if (argInstance != null)
                {
                    var toSUITMethod = argInstance.GetType().GetMethod("ToSUIT");
                    if (toSUITMethod != null)
                    {
                        suitRepresentation.Add(SuitKey);

                        
                        if (ArgType == typeof(SUITReportingPolicy))
                        {

                            suitRepresentation.Add(toSUITMethod.Invoke(argInstance, null));
 
                        }
                        else
                        {    

                            
                            suitRepresentation.Add((toSUITMethod.Invoke(argInstance, null)));

                            
                        }
                    }
                }

                return suitRepresentation.ToArray(); 
            }




            public Dictionary<string, object> ToJson()
            {
                var jsonRepresentation = new Dictionary<string, object>();

                // Skip serialization for specific command types if needed
                if (JsonKey == "directive-set-component-index" || JsonKey == "directive-set-dependency-index")
                {
                    return jsonRepresentation;
                }

                jsonRepresentation["command-id"] = JsonKey;

                // Serialize 'argInstance' if it implements a ToJson method
                if (argInstance != null && argInstance is ISUITConvertible<dynamic>)
                {
                    jsonRepresentation["command-arg"] = ((ISUITConvertible<dynamic>)argInstance).ToJson();
                }
                else
                {
                    // Handle the case where 'argInstance' does not implement ISUITConvertible
                    // You might want to serialize it differently or log a warning
                }

                // Serialize 'cid' if it implements a ToJson method
                if (cid != null && cid is ISUITConvertible<dynamic>)
                {
                    jsonRepresentation["component-id"] = ((ISUITConvertible<dynamic>)cid).ToJson();
                }
                else
                {
                    // Handle the case where 'cid' does not implement ISUITConvertible
                    // You might want to serialize it differently or log a warning
                }

                return jsonRepresentation;
            }
    

            public SUITCmd FromJson(Dictionary<string, object> jsonData)
            {
                if (jsonData == null)
                    throw new ArgumentNullException(nameof(jsonData));

                if (!jsonData.TryGetValue("command-id", out var commandId) || (string)commandId != JsonKey)
                    throw new ArgumentException("JSON Key mismatch error");

                try
                {
                    // Handle 'command-arg'
                    if (jsonData.TryGetValue("command-arg", out var commandArg) && commandArg is Dictionary<string, object> commandArgDict)
                    {
                        // Create an instance of the argument type dynamically
                        if (ArgType == typeof(SUITReportingPolicy))
                        {
                            // Determine the default policy based on the command type
                            int defaultPolicy = SUITReportingPolicy.GetDefaultPolicyForCommand(JsonKey);
                            var policyInstance = SUITReportingPolicy.MkPolicy(defaultPolicy);

                            // Now use FromJson to potentially update the policy based on jsonData
                            policyInstance.FromJson(commandArgDict as Dictionary<string, object>);
                            argInstance = policyInstance;
                        }  else if (ArgType == typeof(SUITTryEach))
                        {
                            if (commandArg is SUITTryEach tryEachInstance)
                            {
                                argInstance = tryEachInstance;
                            }
                            else if (commandArg is Dictionary<string, object> commandArgDictforsuit)
                            {
                                // If commandArg is a dictionary, use FromJson to populate the instance
                                tryEachInstance = new SUITTryEach();
                                tryEachInstance.FromJson(commandArgDictforsuit);
                                argInstance = tryEachInstance;
                            }
                            else
                            {
                                Console.WriteLine("Invalid format for SUITTryEach command argument.");
                            }
                        }

                        else
                        {
                            argInstance = Activator.CreateInstance(ArgType);
                            var convertibleInterface = typeof(ISUITConvertible<>).MakeGenericType(ArgType);
                            if (convertibleInterface.IsInstanceOfType(argInstance))
                            {
                                var fromJsonMethod = convertibleInterface.GetMethod("FromJson");
                                if (fromJsonMethod != null)
                                {
                                    // If FromJson returns the modified instance, capture it
                                    argInstance = fromJsonMethod.Invoke(argInstance, new object[] { commandArgDict });
                                }
                                else
                                {
                                    Console.WriteLine("FromJson method not found.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"The type {ArgType.Name} does not implement ISUITConvertible<{ArgType.Name}>.");
                            }
                        }
                    }else if (jsonData.TryGetValue("command-arg", out var commandArgument) &&
                              commandArg is SUITTryEach commandSuitTryeach)
                    {
                        argInstance = commandSuitTryeach;
                    }else if (jsonData.TryGetValue("command-arg", out var commandint) &&
                              commandArg is int commandnewint)
                    {
                        if (ArgType == typeof(SUITReportingPolicy))
                        {
                            // Determine the default policy based on the command type
                            int defaultPolicy = SUITReportingPolicy.GetDefaultPolicyForCommand(JsonKey);
                            var policyInstance = SUITReportingPolicy.MkPolicy(defaultPolicy);

                            // Now use FromJson to potentially update the policy based on jsonData
                            policyInstance.FromJson(commandArg as Dictionary<string, object>);
                            argInstance = policyInstance;
                        }                     }
                        
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    // Handle or log the exception as needed
                }

                // Handle 'component-id'
                // Handle 'component-id'
                if (jsonData.TryGetValue("component-id", out var componentId) && componentId is Dictionary<string, object> componentIdDict)
                {
                    cid = new SUITComponentId(); // Assuming SUITComponentId has a suitable constructor or method to handle this

                    if (componentIdDict.TryGetValue("componentIds", out var idValue))
                    {
                        if (idValue is List<object> idList)
                        {
                            cid.componentIds = idList.Select(idObj =>
                            {
                                if (idObj is Dictionary<string, object> idDict)
                                {
                                    return new SUITBytes().FromJson(idDict);
                                }
                                throw new ArgumentException("Invalid format for component id");
                            }).ToList();
                        }
                        else
                        {
                            throw new ArgumentException("Invalid format for component id");
                        }
                    }
                }


                // Handle 'dependent-params' if present
                if (jsonData.TryGetValue("dependent-params", out var dependentParams) && dependentParams is IEnumerable<object> depParamsEnum)
                {
                    DepParams = depParamsEnum.Cast<string>().ToList();
                }
                Console.WriteLine("FromJson() completed.");
    
                return this;
            }


            public SUITCmd FromSUIT(List<object> suitList)
            {
                if (suitList == null || suitList.Count < 2)
                {
                    throw new ArgumentException("Invalid SUIT data.");
                }

                if (!(suitList[0] is int suitKey) || suitKey != SuitKey)
                {
                    throw new ArgumentException("Invalid 'suitKey' in the SUIT data.");
                }

                try
                {
                    // Handle different types appropriately
                    if (ArgType == typeof(SUITReportingPolicy))
                    {
                        // Example: Assuming the policy value is the second item in the list
                        int defaultPolicy = SUITReportingPolicy.GetDefaultPolicyForCommand(JsonKey);
                        var policyInstance = SUITReportingPolicy.MkPolicy(defaultPolicy);

                        // Now use FromJson to potentially update the policy based on jsonData
                        argInstance = policyInstance;   }
                    else
                    {
                        // For other types, use Activator.CreateInstance and then populate
                        argInstance = Activator.CreateInstance(ArgType);
                        var fromSUITMethod = ArgType.GetMethod("FromSUIT");
                        if (fromSUITMethod != null)
                        {
                            argInstance = fromSUITMethod.Invoke(argInstance, new object[] { suitList[1] });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred in FromSUIT: {ex.Message}");
                    // Handle or log the exception as needed
                }

                // Handle dependent parameters if present
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

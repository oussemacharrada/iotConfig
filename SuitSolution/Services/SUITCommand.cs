using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using PeterO.Cbor;

namespace SuitSolution.Services
{
    public class SUITCommand
    {
        [JsonPropertyName("sequence-number")]
        public SUITComponentId ComponentId { get; set; }

        [JsonPropertyName("command")]
        public Dictionary<string, object> CommandId { get; set; }

        [JsonPropertyName("args")]
        public List<CBORObject> Args { get; set; }

        public SUITCommand()
        {
            Args = new List<CBORObject>();
        }

        public SUITCommand(Dictionary<string, object> cid, Dictionary<string, object> name, object jarg)
        {
            this.CommandId = cid;
            this.CommandId = name;
            this.Args = (List<CBORObject>?)jarg;
        }

        public string ToJson()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }

        public static SUITCommand FromJson(string json)
        {
            return JsonSerializer.Deserialize<SUITCommand>(json);
        }

       
    }
    
    
    
    public class SUITCommandBuilder
    {
        private SUITCommand command;

        public SUITCommandBuilder()
        {
            command = new SUITCommand();
        }

        
        public SUITCommandBuilder AddArgument(CBORObject argument)
        {
            if (command.Args == null)
            {
                command.Args = new List<CBORObject>();
            }
            command.Args.Add(argument);
            return this;
        }

        public SUITCommand Build()
        {
            return command;
        }

        public string ToJson()
        {
            return command.ToJson();
        }

        public static SUITCommandBuilder FromJson(string json)
        {
            return new SUITCommandBuilder().SetCommand(SUITCommand.FromJson(json));
        }

        public SUITCommandBuilder SetCommand(SUITCommand existingCommand)
        {
            command = existingCommand;
            return this;
        }
    }
}
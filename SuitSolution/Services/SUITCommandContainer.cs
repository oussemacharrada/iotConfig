using System;
using System.Collections.Generic;
using System.Linq;

namespace SuitSolution.Services
{
    public class SUITCommandContainer
    {
        public string json_key { get; init; }
        public string suit_key { get; }
        public Type argtype { get; }
        public List<string> dep_params { get; }

        public SUITCommandContainer(string jkey, string skey, Type argtype, List<string> dp = null)
        {
            json_key = jkey;
            suit_key = skey;
            dep_params = dp;
            this.argtype = argtype;
        }
        public SUITCommand CreateDic(Dictionary<string, object> j)
        {
            if (j.ContainsKey("command-id"))
            {
                string jkey = (string)j["command-id"];
                string skey = (string)j["command-arg"];
                List<string> dp = j.ContainsKey("dep-params") ? (List<string>)j["dep-params"] : null;

                return new SUITCmd(this);
            }
            else
            {
                return null; 
            }
        }
        public static SUITCommandContainer Create(string jkey, string skey, Type argtype, List<string> dp = null)
        {
            return new SUITCommandContainer(jkey, skey, argtype, dp);
        }

        public SUITCommand CreateCommand()
        {
            return new SUITCmd(this);
        }


        public class SUITCmd : SUITCommand
        {
            public string json_key { get; }
            public string suit_key { get; }
            public Type argtype { get; }
            public List<string> dep_params { get; }

            public SUITCmd(string jsonKey, string suitKey, List<string> depParams)
            {
                json_key = jsonKey;
                suit_key = suitKey;
                dep_params = depParams;
            }
            public SUITCmd(SUITCommandContainer container)
            {
                json_key = container.json_key;
                suit_key = container.suit_key;
                dep_params = container.dep_params;
                argtype = container.argtype; 
            }



            public List<object> ToSUIT()
            {
                var argInstance = Activator.CreateInstance(argtype);

                var suitDict = new Dictionary<string, object>
                {
                    { "command-id", json_key },
                    { "command-arg", argInstance },
                    { "component-id", SUITCommonInfo.CurrentIndex }
                };

                if (dep_params != null && dep_params.Any())
                {
                    suitDict.Add("dep-params", dep_params);
                }

                return new List<object> { suitDict };
            }
       


        }
    }
}

using SuitSolution.Interfaces;
using SuitSolution.Services;

public class testsuit
{

    public SUITCommand MkCommand(ISUITConvertible cid, string name, object arg)
    {
        object jarg;

        if (arg is ISUITConvertible suitConvertible)
        {
            jarg = suitConvertible.ToJson();
        }
        else
        {
            jarg = arg;
        }

        var suitDict = new Dictionary<string, object>
        {
            { "component-id", cid.ToJson() },
            { "command-id", name },
            { "command-arg", jarg }
        };

        return new SUITCommand().FromJson(suitDict);
    }

    public class CheckResult
    {
        public Dictionary<string, string> Equal { get; set; }
        public Dictionary<string, string> NotEqual { get; set; }
    }

    
    public CheckResult CheckEq(Dictionary<string, string> ids, List<Dictionary<string, string>> choices)
    {
        var equal = new Dictionary<string, string>();
        var notEqual = new Dictionary<string, string>();

        Func<List<string>, bool> check = x => x.Take(x.Count - 1).SequenceEqual(x.Skip(1));
        Func<string, List<Dictionary<string, string>>, List<string>> get = (k, l) => l.Select(d => d.ContainsKey(k) ? d[k] : null).ToList();

        foreach (var key in ids.Keys)
        {
            if (choices.Any(c => c.ContainsKey(key) && check(get(key, choices))))
            {
                equal[key] = ids[key];
            }
        }

        check = x => !x.Take(x.Count - 1).SequenceEqual(x.Skip(1));

        foreach (var key in ids.Keys)
        {
            if (choices.Any(c => c.ContainsKey(key) && check(get(key, choices))))
            {
                notEqual[key] = ids[key];
            }
        }

        return new CheckResult { Equal = equal, NotEqual = notEqual };
    }
    
    
    
}
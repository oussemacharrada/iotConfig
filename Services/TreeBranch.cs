public static class TreeBranch
{
    private static Stack<string> _branch = new Stack<string>();

    public static void Append(string typeName)
    {
        _branch.Push(typeName);
    }

    public static void Pop()
    {
        if (_branch.Count > 0)
        {
            _branch.Pop();
        }
    }

    public static IEnumerable<string> GetCurrentBranch() // Adjusted return type here
    {
        return _branch.Reverse();
    }
}
namespace SuitSolution.Exceptions;

class SUITException : Exception
{
    public object Data { get; }
    public List<Type> TreeBranch { get; }

    public SUITException(string message, object data, List<Type> treeBranch)
        : base(message)
    {
        Data = data;
        TreeBranch = treeBranch;
    }
}

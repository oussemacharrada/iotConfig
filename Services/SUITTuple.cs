namespace SuitSolution.Services;

public class SUITTuple
{
    public List<byte[]> Items { get; set; }

    public SUITTuple(List<byte[]> items)
    {
        Items = items;
    }

    public SUITTuple()
    {
    }
}

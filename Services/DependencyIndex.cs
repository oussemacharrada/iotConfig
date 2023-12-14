public class DependencyIndex
{
    public int Value { get; private set; }

    public DependencyIndex(int value)
    {
        Value = value;
    }

    // Implicit conversion from DependencyIndex to int
    public static implicit operator int(DependencyIndex d) => d.Value;

    // Implicit conversion from int to DependencyIndex
    public static implicit operator DependencyIndex(int value) => new DependencyIndex(value);

    // Override Equals and GetHashCode if needed
    public override bool Equals(object obj)
    {
        if (obj is DependencyIndex other)
        {
            return this.Value == other.Value;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    // Override ToString for easy debugging
    public override string ToString()
    {
        return Value.ToString();
    }
}
namespace DeepEqualsGenerator;

public interface IDeepEqualsContext
{
    static abstract KeyValuePair<Type, Delegate>[] EqualsMethods { get; }
}
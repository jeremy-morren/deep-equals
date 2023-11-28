namespace DeepEqualsGenerator.Graph;

internal interface IInterfaceEqualityType : IEqualityType
{
    INamedTypeSymbol InterfaceType { get; }

    object ElementType { get; }
}
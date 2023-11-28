namespace DeepEqualsGenerator.Graph;

internal record EnumerableType(INamedTypeSymbol InterfaceType, object ElementType) : IInterfaceEqualityType
{
    ITypeSymbol IEqualityType.Type => InterfaceType;
}
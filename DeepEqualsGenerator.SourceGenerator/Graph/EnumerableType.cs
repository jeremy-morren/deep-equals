namespace DeepEqualsGenerator.SourceGenerator.Graph;

internal record EnumerableType(INamedTypeSymbol InterfaceType, object ElementType) : IInterfaceEqualityType
{
    ITypeSymbol IEqualityType.Type => InterfaceType;
}
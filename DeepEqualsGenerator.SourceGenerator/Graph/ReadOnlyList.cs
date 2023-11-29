namespace DeepEqualsGenerator.SourceGenerator.Graph;

internal record ReadOnlyList(INamedTypeSymbol InterfaceType, object ElementType) : IInterfaceEqualityType
{
    ITypeSymbol IEqualityType.Type => InterfaceType;
}
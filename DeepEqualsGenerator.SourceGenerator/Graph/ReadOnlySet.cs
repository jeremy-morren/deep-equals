namespace DeepEqualsGenerator.SourceGenerator.Graph;

public record ReadOnlySet(INamedTypeSymbol InterfaceType, object ElementType) : IInterfaceEqualityType
{
    ITypeSymbol IEqualityType.Type => InterfaceType;
}
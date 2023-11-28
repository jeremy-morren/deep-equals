namespace DeepEqualsGenerator.Graph;

[PublicAPI]
internal record ReadOnlyDictionary(INamedTypeSymbol InterfaceType, 
    ITypeSymbol KeyType, 
    object ValueType) : IInterfaceEqualityType
{
    ITypeSymbol IEqualityType.Type => InterfaceType;

    object IInterfaceEqualityType.ElementType => ValueType;
}
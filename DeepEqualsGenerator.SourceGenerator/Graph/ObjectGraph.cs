using System.Runtime.InteropServices.ComTypes;

namespace DeepEqualsGenerator.SourceGenerator.Graph;

internal class ObjectGraph : IEqualityType
{
    public required ITypeSymbol Type { get; init; }
    
    public required IReadOnlyList<ISymbol> PrimitiveMembers { get; init; }

    public List<ComplexMember> ComplexMembers { get; } = new();

    public bool IsConcreteType => Type.TypeKind is TypeKind.Class or TypeKind.Struct;
}
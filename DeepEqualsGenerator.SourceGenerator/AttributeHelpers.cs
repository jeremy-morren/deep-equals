namespace DeepEqualsGenerator.SourceGenerator;

internal static class AttributeHelpers
{
    public static IReadOnlyList<ITypeSymbol> GetDeepEqualsAttributes(ITypeSymbol type)
    {
        var attributes = type.GetAttributes()
            .Where(a => a.AttributeClass?.CSharpName() == "global::DeepEqualsGenerator.GenerateDeepEqualsAttribute")
            .ToList();
        
        var set = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
        foreach (var arg in attributes.Select(a => a.ConstructorArguments[0]))
            set.Add((ITypeSymbol)arg.Value!);

        return set.ToList();
    }
}
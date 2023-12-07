namespace DeepEqualsGenerator.SourceGenerator;

internal static class AttributeHelpers
{
    public static bool HasDeepEqualsAttribute(this ITypeSymbol type, out IReadOnlyList<ITypeSymbol> args)
    {
        var attributes = type.GetAttributes()
            .Where(a => a.AttributeClass?.Name == "GenerateDeepEqualsAttribute")
            .ToList();
        if (attributes.Count == 0)
        {
            args = null!;
            return false;
        }

        var set = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
        foreach (var a in attributes)
        {
            if (a.ConstructorArguments.Length == 0)
            {
                set.Add(type);
            }
            else
            {
                var arg = a.ConstructorArguments[0];
                if (arg.Value == null)
                    set.Add(type);
                else
                    set.Add((ITypeSymbol)arg.Value);
            }
        }

        args = set.ToList();
        return true;
    }
}
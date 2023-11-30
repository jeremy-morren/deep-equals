using System.Numerics;
using DeepEqualsGenerator.SourceGenerator.Framework;
using DeepEqualsGenerator.SourceGenerator.Graph;

namespace DeepEqualsGenerator.SourceGenerator;

internal class GraphWalker
{
    private readonly Dictionary<ITypeSymbol, IEqualityType> _generated = new(SymbolEqualityComparer.Default);

    public IEnumerable<IEqualityType> Walk(IEnumerable<ITypeSymbol> types) => types
        .Where(t => !t.IsStatic && !t.IsPrimitive())
        .Select(WalkGraph)
        .Cast<IEqualityType>()
        .DistinctBy(t => t.Type, SymbolEqualityComparer.Default);
    
    public object WalkGraph(ITypeSymbol type)
    {
        if (type.IsStatic)
            throw new NotImplementedException($"Cannot generate equals method for static type {type}");

        if (type.IsPrimitive()) 
            return type; //For primitive, return raw type
        
        if (_generated.TryGetValue(type, out var current))
            return current;

        if (IsIReadOnlyDictionary(type, out var interfaceType, out var keyType, out var valueType))
        {
            var dictionary = new ReadOnlyDictionary(interfaceType, keyType, WalkGraph(valueType));
            _generated.Add(type, dictionary);
            return dictionary;
        }

        if (IsIReadOnlySet(type, out interfaceType, out var elementType))
        {
            var set = new ReadOnlySet(interfaceType, WalkGraph(elementType));
            _generated.Add(type, set);
            return set;
        }
        
        if (IsIReadOnlyList(type, out interfaceType, out elementType))
        {
            var list = new ReadOnlyList(interfaceType, WalkGraph(elementType));
            _generated.Add(type, list);
            return list;
        }
        
        if (IsIEnumerable(type, out interfaceType, out elementType))
        {
            var enumerable = new EnumerableType(interfaceType, WalkGraph(elementType));
            _generated.Add(type, enumerable);
            return enumerable;
        }
        
        var members = GetMembers(type)
            .Where(m => !m.IsStatic && !IsCompilerGenerated(m) && m.IsPublic())
            .ToList();

        var properties = members.OfType<IPropertySymbol>()
            .Where(p => p.GetMethod != null && !p.IsIndexer)
            .ToList();
        
        var fields = members.OfType<IFieldSymbol>()
            .ToList();

        var graph = new ObjectGraph()
        {
            Type = type,
            PrimitiveMembers = fields
                .Where(f => f.Type.IsPrimitive())
                .Cast<ISymbol>()
                .Concat(properties
                    .Where(p => p.Type.IsPrimitive()))
                .ToArray(),
        };
        _generated.Add(type, graph);

        foreach (var f in fields.Where(f => !f.Type.IsPrimitive()))
        {
            graph.ComplexMembers.Add(new ComplexMember(f, WalkGraph(f.Type)));
        }
        foreach (var p in properties.Where(p => !p.Type.IsPrimitive()))
        {
            graph.ComplexMembers.Add(new ComplexMember(p, WalkGraph(p.Type)));
        }
        return graph;
    }

    private static bool IsCompilerGenerated(ISymbol symbol)
    {
        return symbol.GetAttributes().Any(a => a.AttributeClass?.Name == "CompilerGeneratedAttribute");
    }
    
    private static IEnumerable<ISymbol> GetMembers(ITypeSymbol type)
    {
        var members = new List<ISymbol>();
        while (true)
        {
            members.AddRange(type.GetMembers());
            if (type.BaseType == null)
                break;
            type = type.BaseType;
        }

        return members.Distinct(SymbolEqualityComparer.Default);
    }

    private static bool IsIReadOnlyDictionary(ITypeSymbol type, 
        out INamedTypeSymbol interfaceType, 
        out ITypeSymbol keyType, 
        out ITypeSymbol valueType)
    {
        if (type is INamedTypeSymbol { IsGenericType: true } named && named.Name.StartsWith("IReadOnlyDictionary"))
        {
            interfaceType = named;
            keyType = named.TypeArguments[0];
            valueType = named.TypeArguments[1];
        }
        
        var @interface = type.AllInterfaces
            .FirstOrDefault(i => i.IsGenericType && i.Name.StartsWith("IReadOnlyDictionary"));

        if (@interface != null)
        {
            interfaceType = @interface;
            keyType = @interfaceType.TypeArguments[0];
            valueType = @interfaceType.TypeArguments[1];
            return true;
        }
        
        interfaceType = null!;
        keyType = null!;
        valueType = null!;
        return false;
    }

    private static bool IsIReadOnlySet(ITypeSymbol type, 
        out INamedTypeSymbol interfaceType,
        out ITypeSymbol elementType)
    {
        if (type is INamedTypeSymbol { IsGenericType: true } named && named.Name.StartsWith("IReadOnlySet"))
        {
            interfaceType = named;
            elementType = named.TypeArguments[0];
            return true;
        }
        
        var @interface = type.AllInterfaces
            .FirstOrDefault(i => i.IsGenericType && i.Name.StartsWith("IReadOnlySet"));
        
        if (@interface != null)
        {
            interfaceType = @interface;
            elementType = @interface.TypeArguments[0];
            return true;
        }

        interfaceType = null!;
        elementType = null!;
        return false;
    }
    
    private static bool IsIReadOnlyList(ITypeSymbol type, out INamedTypeSymbol interfaceType, out ITypeSymbol elementType)
    {
        if (type is INamedTypeSymbol { IsGenericType: true } named && named.Name.StartsWith("IReadOnlyList"))
        {
            interfaceType = named;
            elementType = named.TypeArguments[0];
            return true;
        }
        
        var @interface = type.AllInterfaces
            .FirstOrDefault(i => i.IsGenericType && i.Name.StartsWith("IReadOnlyList"));
        
        if (@interface != null)
        {
            interfaceType = @interface;
            elementType = @interface.TypeArguments[0];
            return true;
        }

        interfaceType = null!;
        elementType = null!;
        return false;
    }
    
    private static bool IsIEnumerable(ITypeSymbol type, out INamedTypeSymbol interfaceType, out ITypeSymbol elementType)
    {
        if (type is INamedTypeSymbol { IsGenericType: true } named && named.Name.StartsWith("IEnumerable"))
        {
            interfaceType = named;
            elementType = named.TypeArguments[0];
            return true;
        }
        
        var @interface = type.AllInterfaces
            .FirstOrDefault(i => i.IsGenericType && i.Name.StartsWith("IEnumerable"));

        if (@interface != null)
        {
            interfaceType = @interface;
            elementType = @interface.TypeArguments[0];
            return true;
        }

        interfaceType = null!;
        elementType = null!;
        return false;
    }
}
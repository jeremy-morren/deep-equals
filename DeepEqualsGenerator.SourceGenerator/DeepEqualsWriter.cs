using System.Runtime.Serialization;
using DeepEqualsGenerator.SourceGenerator.Framework;
using DeepEqualsGenerator.SourceGenerator.Graph;
using Delegate = System.Delegate;
// ReSharper disable SuggestBaseTypeForParameter

// ReSharper disable StringLiteralTypo
// ReSharper disable InvertIf
// ReSharper disable TailRecursiveCall

namespace DeepEqualsGenerator.SourceGenerator;

internal class DeepEqualsWriter
{
    private readonly IndentedWriter _writer = new();
    
    //TODO: Handle reference cycles
    
    public string Generate(IReadOnlyList<ITypeSymbol> contexts)
    {
        _writer.WriteFileHeader("disable");
        //See https://github.com/dotnet/docs/issues/34893
        _writer.WriteLine("#pragma warning disable SYSLIB0050 // ObjectIDGenerator obsolete");
        _writer.WriteLine("using System;");
        
        foreach (var context in contexts)
        {
            _writer.WriteLine($"namespace {context.ContainingNamespace}");
            _writer.WriteLineThenPush('{');

            _writer.WriteClassAttributes();
            _writer.WriteLine($"partial class {context.Name}");
            _writer.WriteLineThenPush('{');
            
            var contextWriter = new ContextWriter(_writer, context);
            
            var types = AttributeHelpers.GetDeepEqualsAttributes(context);
            
            contextWriter.WriteAllMethodsProperty(types);
            _writer.WriteLine();
            contextWriter.GenerateMethods(types);

            _writer.PopThenWriteLine('}'); //Class
            _writer.PopThenWriteLine('}'); //Namespace
        }

        return _writer.ToString();
    }

    private class ContextWriter
    {
        private readonly IndentedWriter _writer;
        private readonly ITypeSymbol _context;

        public ContextWriter(IndentedWriter writer, ITypeSymbol context)
        {
            _writer = writer;
            _context = context;
        }
        
        public void WriteAllMethodsProperty(IReadOnlyList<ITypeSymbol> topLevelTypes)
        {
            var walker = new GraphWalker();
            
            var pair = $"global::{typeof(KeyValuePair<Type, Delegate>).Namespace}.KeyValuePair<global::{typeof(Type).FullName}, Delegate>";
            
            var added = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);

            void Add(ITypeSymbol type, string method)
            {
                if (added.Add(type))
                    _writer.WriteLine($"new {pair}(typeof({type.CSharpName()}), {method}),");
            }
            
            void Recurse(object o)
            {
                var method = MethodName(o);
                switch (o)
                {
                    case ObjectGraph graph:
                        if (added.Contains(graph.Type)) return;
                        
                        method = HasReferenceObjectInGraph(graph.Type)
                            ? $"Static_{method}"
                            : method;
                        Add(graph.Type, method);
                        foreach (var c in graph.ComplexMembers)
                            Recurse(c.Type);
                        break;
                    case IInterfaceEqualityType i:
                        if (added.Contains(i.InterfaceType)) return;
                        
                        method = HasReferenceObjectInGraph(i.InterfaceType)
                            ? $"Static_{method}"
                            : method;
                        Add(i.InterfaceType, method);
                        if (i.ElementType is not ITypeSymbol)
                            Recurse(i.ElementType);
                        break;
                    default:
                        throw new NotImplementedException($"Unknown type {o}");
                }
            }
            _writer.WriteLine($"public static {pair}[] EqualsMethods => new {pair}[]");
            _writer.WriteLineThenPush('{');
            foreach (var type in walker.Walk(topLevelTypes))
            {
                Recurse(type);
            }
            
            //Generate top-level types that have an interface
            foreach (var type in topLevelTypes)
            {
                Add(type, MethodName(type));
            }

            _writer.PopThenWriteLine("};");
        }

        #region Individual method generation
        
        public void GenerateMethods(IReadOnlyList<ITypeSymbol> topLevelTypes)
        {
            var idGenerator = $"global::{typeof(ObjectIDGenerator).FullName}";
            
            _writer.WriteLine($"private readonly {idGenerator} _lIds = new {idGenerator}();");
            _writer.WriteLine($"private readonly {idGenerator} _rIds = new {idGenerator}();");
            
            var generatedMethods = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);

            void Recurse(object g)
            {
                switch (g)
                {
                    case ObjectGraph graph:
                        if (generatedMethods.Contains(graph.Type))
                            return;
                        Graph(graph);
                        generatedMethods.Add(graph.Type);
                        foreach (var c in graph.ComplexMembers)
                            Recurse(c.Type);
                        break;
                    case ReadOnlyDictionary dictionary:
                        if (generatedMethods.Contains(dictionary.InterfaceType))
                            return;
                        Dictionary(dictionary);
                        generatedMethods.Add(dictionary.InterfaceType);
                        
                        if (dictionary.ValueType is not ITypeSymbol)
                            Recurse(dictionary.ValueType);
                        break;
                    case ReadOnlyList list:
                        if (generatedMethods.Contains(list.InterfaceType))
                            return;
                        List(list);
                        generatedMethods.Add(list.InterfaceType);
                        
                        if (list.ElementType is not ITypeSymbol)
                            Recurse(list.ElementType);
                        break;
                    case EnumerableType enumerable:
                        if (generatedMethods.Contains(enumerable.InterfaceType))
                            return;
                        Enumerable(enumerable);
                        generatedMethods.Add(enumerable.InterfaceType);
                        
                        if (enumerable.ElementType is not ITypeSymbol)
                            Recurse(enumerable.ElementType);
                        break;
                    case ReadOnlySet set:
                        if (generatedMethods.Contains(set.InterfaceType))
                            return;
                        Set(set);
                        generatedMethods.Add(set.InterfaceType);
                        
                        if (set.ElementType is not ITypeSymbol)
                            Recurse(set.ElementType);
                        break;
                    default:
                        throw new NotImplementedException($"Unknown type {g}");
                }
            }

            var walker = new GraphWalker();

            foreach (var g in walker.Walk(topLevelTypes))
                Recurse(g);

            foreach (var type in topLevelTypes)
            {
                if (generatedMethods.Contains(type)) continue;
                
                //We have an interface method for this type
                //Therefore we create a static method that calls it
                
                var name = type.CSharpName();
                var method = MethodName(type);
                
                var compare = (IInterfaceEqualityType)walker.WalkGraph(type);
                var innerMethod = MethodName(compare.InterfaceType);

                _writer.WriteLine();
                
                _writer.WriteLine(HasReferenceObjectInGraph(compare.InterfaceType)
                    ? $"private static bool {method}({name} l, {name} r) => new {_context.Name}().{innerMethod}(l,r);"
                    : $"private static bool {method}({name} l, {name} r) => {innerMethod}(l,r);");
            }
        }

        private static string PrimitiveEquals(ITypeSymbol s, string left, string right)
        {
            //For value types (including Nullable<>), use Equals method
            //For reference types (string/object), use == operator
            
            return s.IsValueType ? $"{left}.Equals({right})" : $"{left} == {right}";
        }
        
        private static string PrimitiveNotEquals(ITypeSymbol s, string left, string right)
        {
            //For value types (including Nullable<>), use Equals method
            //For reference types (string/object), use == operator
            
            return s.IsValueType ? $"!{left}.Equals({right})" : $"{left} != {right}";
        }

        private void Dictionary(ReadOnlyDictionary dictionary)
        {
            StartMethod(dictionary.InterfaceType);

            _writer.WriteLine("if (l.Count != r.Count) return false;");
            
            _writer.WriteStatement("foreach (var pair in l)", () =>
            {
                _writer.WriteLine("var key = pair.Key;");
                _writer.WriteLine("var lv = pair.Value;");
                
                _writer.WriteLine("if (!r.TryGetValue(key, out var rv)) return false;");
                switch (dictionary.ValueType)
                {
                    case ITypeSymbol s:
                        //Primitive
                        _writer.WriteLine($"if ({PrimitiveNotEquals(s, "lv", "rv")}) return false;");
                        break;
                    default:
                        //NB: TODO: Handle complex keys
                        _writer.WriteLine($"if (!{MethodName(dictionary.ValueType)}(lv, rv)) return false;");
                        break;
                }
            });
            _writer.WriteLine("return true;");
            
            _writer.PopThenWriteLine('}');
        }

        private void Set(ReadOnlySet set)
        {
            StartMethod(set.InterfaceType);
            
            _writer.WriteLine("var count = l.Count;");

            _writer.WriteLine("if (count != r.Count) return false;");
            
            _writer.WriteStatement("foreach (var lv in l)", () =>
            {
                if (set.ElementType is not ITypeSymbol)
                    throw new NotImplementedException("Cannot generate equals method for complex set elements");
                
                //NB: We rely on the set comparer here
                _writer.WriteLine("if (!r.Contains(lv)) return false;");
            });
            _writer.WriteLine("return true;");
            _writer.PopThenWriteLine('}');
        }

        private void List(ReadOnlyList list)
        {
            StartMethod(list.InterfaceType);

            _writer.WriteLine("var count = l.Count;");
            _writer.WriteLine("if (count != r.Count) return false;");
            
            //Compare members
            _writer.WriteStatement("for (var i = 0; i < count; ++i)", () =>
            {
                switch (list.ElementType)
                {
                    case ITypeSymbol s:
                        _writer.WriteLine($"if ({PrimitiveNotEquals(s, "l[i]", "r[i]")}) return false;");
                        break;
                    default:
                        _writer.WriteLine($"if (!{MethodName(list.ElementType)}(l[i], r[i])) return false;");
                        break;
                }
            });

            _writer.WriteLine("return true;");

            _writer.PopThenWriteLine('}');
        }

        private void Enumerable(EnumerableType enumerable)
        {
            StartMethod(enumerable.InterfaceType);
            
            _writer.WriteLine("using var le = l.GetEnumerator();");
            _writer.WriteLine("using var re = r.GetEnumerator();");
            
            _writer.WriteStatement("while (true)", () =>
            {
                _writer.WriteLine("var ln = le.MoveNext();");
                _writer.WriteLine("var rn = re.MoveNext();");
                
                _writer.WriteLine("if (ln != rn) return false;");
                _writer.WriteLine("if (!ln) return true;"); //Both false - end of enumeration, same count
                
                switch (enumerable.ElementType)
                {
                    case ITypeSymbol s:
                        //Primitive
                        _writer.WriteLine($"if ({PrimitiveNotEquals(s, "le.Current", "re.Current")}) return false;");
                        break;
                    default:
                        _writer.WriteLine($"if (!{MethodName(enumerable.ElementType)}(le.Current, re.Current)) return false;");
                        break;
                }
            });

            _writer.WriteLine("throw new NotImplementedException();");

            _writer.PopThenWriteLine('}');
        }

        private void Graph(ObjectGraph graph)
        {
            StartMethod(graph.Type);
            
            if (graph.Type.IsNullable(out var underlying))
            {
                /*
                 * l.HasValue == r.HasValue - both null or both not null
                 *
                 * !l.HasValue || Equals(l.Value, r.Value) - Either left is null (hence both null) or compare values
                 */
                _writer.WriteLine($"return l.HasValue == r.HasValue && (!l.HasValue || {MethodName(underlying)}(l.Value, r.Value));");
            }
            else
            {
                _writer.WriteLineThenPush("return");
                
                //Compare members
                
                foreach (var m in graph.PrimitiveMembers)
                {
                    var type = m switch
                    {
                        IFieldSymbol f => f.Type,
                        IPropertySymbol p => p.Type,
                        _ => throw new ArgumentOutOfRangeException(nameof(m), m, $"Unknown member type {m}")
                    };
                    _writer.WriteLine($"{PrimitiveEquals(type, $"l.{m.Name}", $"r.{m.Name}")} &&");
                }
                
                foreach (var m in graph.ComplexMembers)
                    _writer.WriteLine($"{MethodName(m.Type)}(l.{m.Member.Name}, r.{m.Member.Name}) &&");
            
                _writer.TrimEnd(" &&\n".Length);
                _writer.WriteRawLineAndPop(";");
            }
            
            _writer.PopThenWriteLine('}');
        }
        
        private static bool HasReferenceObjectInGraph(ITypeSymbol type)
        {
            if (type.IsReferenceType) return true;
            
            if (type.IsPrimitive() || type.IsStatic) return false;

            if (type.IsNullable(out var u))
                return HasReferenceObjectInGraph(u);
            
            if (type is INamedTypeSymbol { IsGenericType: true } named && named.TypeArguments.Any(HasReferenceObjectInGraph))
                return true;
            
            return type
                .GetMembers()
                .Where(m => m.IsPublic())
                .Any(m => (m is IFieldSymbol f && HasReferenceObjectInGraph(f.Type))
                          || (m is IPropertySymbol { IsIndexer: false } p && HasReferenceObjectInGraph(p.Type)));
        }

        private void StartMethod(ITypeSymbol type)
        {
            if (type.IsPrimitive())
                throw new NotImplementedException("Cannot generate equals method for primitive type");
            
            _writer.WriteLine();
            
            var csharpName = type.CSharpName();
            var method = MethodName(type);

            if (HasReferenceObjectInGraph(type))
            {
                //Reference object. First write a static version, which is what everyone else can use
                _writer.WriteLine($"private static bool Static_{method}({csharpName} l, {csharpName} r) => new {_context.Name}().{method}(l,r);");

                _writer.WriteLine();
                
                //Write the instance version
                _writer.WriteLine($"private bool {method}({csharpName} l, {csharpName} r)");
            }
            else
            {
                //No reference children. 1 method only (static)
                _writer.WriteLine($"private static bool {method}({csharpName} l, {csharpName} r)");
            }
            
            _writer.WriteLineThenPush('{');

            if (!type.IsReferenceType) return;
            
            //Standard reference checks
            _writer.WriteLine("if (object.ReferenceEquals(l, r)) return true;");
            _writer.WriteLine("if (object.ReferenceEquals(l, null) || ReferenceEquals(r, null)) return false;");
            _writer.WriteLine();
            
            const string ids = $"Ids.{nameof(ObjectIDGenerator.GetId)}";
            
            //Add both objects to graph
            //And ensure that the ID (i.e. the sequential generation) is identical
            _writer.WriteLine($"long lId = _l{ids}(l, out bool lFirst);");
            _writer.WriteLine($"long rId = _r{ids}(r, out bool rFirst);");

            _writer.WriteLine("if (lFirst != rFirst || lId != rId) return false;");
            _writer.WriteLine();
        }
    
        #endregion
    }

    private static string MethodName(object o)
    {
        return o switch
        {
            IInterfaceEqualityType i => MethodName(i.InterfaceType),
            ObjectGraph g => MethodName(g.Type),
            _ => throw new ArgumentOutOfRangeException(nameof(o), o, $"Unknown type {o}")
        };
    }
    
    private static string MethodName(ITypeSymbol type)
    {
        var fullName = type.ToDisplayString(NullableFlowState.None, SymbolDisplayFormat.FullyQualifiedFormat);
        fullName = fullName.Replace("global::", string.Empty)
            .Replace("[]", "Array")
            .Replace("?", "_Nullable");

        if (fullName.EndsWith(">"))
            fullName = fullName.Substring(0, fullName.Length - 1); //Trim trailing '>'

        //Remove System namespaces
        fullName = SystemNamespaces.Aggregate(fullName, (current, n) => current.Replace(n, string.Empty));

        return new[] { '.', '<', '>', ',', ' ' }.Aggregate(fullName, (str, c) => str.Replace(c, '_'));
    }

    private static readonly string[] SystemNamespaces = new[]
        {
            typeof(List<>).Namespace,
            typeof(int[]).Namespace
        }
        .Select(n => $"{n}.")
        .ToArray();
}
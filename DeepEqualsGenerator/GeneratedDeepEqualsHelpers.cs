using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using DeepEqual.Syntax;
using JetBrains.Annotations;

namespace DeepEqualsGenerator;

[PublicAPI]
public static class GeneratedDeepEqualsHelpers
{
    /// <summary>
    /// Returns an <see cref="IEqualityComparer{T}"/> that uses the generated deep equals method to compare objects of type <typeparamref name="T"/>
    /// </summary>
    [System.Diagnostics.Contracts.Pure] //Not actually pure, but should be used as such
    public static IEqualityComparer<T> GetFastDeepEqualityComparer<T>()
    {
        return new DeepEqualsEqualityComparer<T>(GetFastDeepEqual<T>());
    }
    
    internal static Func<T, T, bool> GetFastDeepEqual<T>()
    {
        if (GeneratedMethods.TryGetValue(typeof(T), out var d))
            return (Func<T, T, bool>)d;
        
        //Add loaded generated methods
        AddGeneratedMethods();
        
        if (GeneratedMethods.TryGetValue(typeof(T), out d))
            return (Func<T, T, bool>)d;

        throw new Exception($"No generated deep equals method found for {typeof(T)}");
    }
    
    private class DeepEqualsEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _delegate;

        public DeepEqualsEqualityComparer(Func<T, T, bool> @delegate)
        {
            _delegate = @delegate;
        }

        public bool Equals(T? x, T? y)
        {
            if (ReferenceEquals(x, y)) return true;
            
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
            
            if (x.GetType() != y.GetType())
                throw new NotImplementedException("Cannot compare values of different types");
#if RELEASE
        return _delegate(x,y);
#else
            var slow = x.IsDeepEqual(y);
            var fast = _delegate(x, y);
            if (slow == fast) return fast;
        
            // TODO: Fix fast and slow occasionally not matching
            
            var json = JsonSerializer.Serialize(x) == JsonSerializer.Serialize(y);
            
            throw new Exception($"Slow did not match fast for {typeof(T).FullName}. Slow: {slow}, Fast: {fast}, JSON: {json}");
#endif
        }

        public int GetHashCode([DisallowNull] T obj) => 0; //Not used
    }

    /// <summary>
    /// Adds generated methods from all types that implement <see cref="IDeepEqualsContext"/>
    /// </summary>
    private static void AddGeneratedMethods()
    {
        var contexts = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a =>
            {
                var name = a.GetName().Name;
                if (name == null) return false;

                //Exclude assemblies that won't have generated methods
                return !name.StartsWith("Microsoft", StringComparison.Ordinal)
                       && !name.StartsWith("System", StringComparison.Ordinal)
                       && !name.StartsWith("Windows", StringComparison.Ordinal)
                       && !name.StartsWith("netstandard", StringComparison.Ordinal)
                       && !name.StartsWith("DeepEquals", StringComparison.Ordinal)
                       && name != "mscorlib";
            })
            .SelectMany(a => a.GetTypes())
            .Where(typeof(IDeepEqualsContext).IsAssignableFrom)
            .ToList();
        
        var addMethod = typeof(GeneratedDeepEqualsHelpers).GetMethod(nameof(AddMethodsFromContext), BindingFlags.NonPublic | BindingFlags.Static)!;
        lock (ProcessedTypes)
        {
            foreach (var type in contexts.Where(t => !ProcessedTypes.Contains(t)))
            {
                addMethod.MakeGenericMethod(type).Invoke(null, null);
                ProcessedTypes.Add(type);
            }
        }
    }

    private static void AddMethodsFromContext<TContext>() where TContext : IDeepEqualsContext
    {
        foreach (var pair in TContext.EqualsMethods)
            if (!GeneratedMethods.TryAdd(pair.Key, pair.Value))
                throw new Exception($"{pair.Key} has multiple generated DeepEquals methods");
    }
    
    private static readonly HashSet<Type> ProcessedTypes = new();
    
    private static readonly ConcurrentDictionary<Type, Delegate> GeneratedMethods;

    static GeneratedDeepEqualsHelpers()
    {
        GeneratedMethods = new ConcurrentDictionary<Type, Delegate>();
        
        //Add primitive types
        static void Add<T>(Func<T,T,bool> d)
        {
            Debug.Assert(GeneratedMethods.TryAdd(typeof(T), d));
        }

        Add<bool>((a, b) => a == b);
        Add<byte>((a, b) => a == b);
        Add<sbyte>((a, b) => a == b);
        Add<char>((a, b) => a == b);
        Add<decimal>((a, b) => a == b);
        Add<double>((a, b) => a.Equals(b));
        Add<float>((a, b) => a.Equals(b));
        Add<int>((a, b) => a == b);
        Add<uint>((a, b) => a == b);
        Add<nint>((a, b) => a == b);
        Add<nuint>((a, b) => a == b);
        Add<long>((a, b) => a == b);
        Add<ulong>((a, b) => a == b);
        Add<short>((a, b) => a == b);
        Add<ushort>((a, b) => a == b);
        
        Add<bool?>((a, b) => a == b);
        Add<byte?>((a, b) => a == b);
        Add<sbyte?>((a, b) => a == b);
        Add<char?>((a, b) => a == b);
        Add<decimal?>((a, b) => a == b);
        Add<double?>((a, b) => a.Equals(b));
        Add<float?>((a, b) => a.Equals(b));
        Add<int?>((a, b) => a == b);
        Add<uint?>((a, b) => a == b);
        Add<nint?>((a, b) => a == b);
        Add<nuint?>((a, b) => a == b);
        Add<long?>((a, b) => a == b);
        Add<ulong?>((a, b) => a == b);
        Add<short?>((a, b) => a == b);
        Add<ushort?>((a, b) => a == b);
        
        Add<string>((a, b) => a == b);
        
        Add<object>((_, _) => throw new NotImplementedException("Raw object comparison is not supported"));
    }
}